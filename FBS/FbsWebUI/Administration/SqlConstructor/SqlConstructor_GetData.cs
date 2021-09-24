using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;

namespace Fbs.Web.Administration.SqlConstructor
{
    public abstract class SqlConstructor_GetData
    {
        protected readonly StringCollection AllowedFieldNames = new StringCollection(); // список разрешённых полей
        protected NameValueCollection m_urlParameters;
        public int defaultPageSize = 20;
        public string defaultOrderField="ID";

        // список разрешённых полей
        public StringCollection Fields
        {
            get { return AllowedFieldNames ; }
        }

        public bool HasFilter()
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<string> whereExpr = new List<string>();
            CreateParameters(parameters, whereExpr);

            return whereExpr.Count > 0;
        }

        protected abstract void CreateParameters(List<SqlParameter> parameters, List<string> whereExpr);
        protected abstract string getMainSQL();

        protected string GetWhere(out List<SqlParameter> parameters)
        {
            parameters = new List<SqlParameter>();
            List<string> whereExpr = new List<string>();
            CreateParameters(parameters, whereExpr);
            return string.Join(" AND ", whereExpr.ToArray());
        }



        public SqlCommand GetCountOrgsSQL()
        {
            List<SqlParameter> sqlParameters;
            string where = GetWhere(out sqlParameters);

            string sql = string.Format("SELECT @rowCount=count(*) FROM (" + getMainSQL() + ") T {0} ",
                (where.Length > 0 ? "WHERE " + where : ""));

            SqlParameter rowCount = new SqlParameter("rowCount", SqlDbType.Int);
            rowCount.Direction = ParameterDirection.Output;
            sqlParameters.Add(rowCount);

            SqlCommand cmd = new SqlCommand(sql);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddRange(sqlParameters.ToArray());

            return cmd;
        }

        public SqlCommand GetSQL()
        {
            string sort = GetSort();

            List<SqlParameter> sqlParameters;
            string where = GetWhere(out sqlParameters);

            string sql = string.Format(@"
                SELECT * FROM
                (   SELECT row_number() over (order by {0}) RowNum, T.*
                    FROM ("
                + getMainSQL() + @"
                ) T {1} ) T
                WHERE RowNum BETWEEN @RowNum_start AND @RowNum_end
                ORDER BY RowNum"
                , (sort.Length > 0 ? sort + ", " + defaultOrderField : defaultOrderField)
                , (where.Length > 0 ? "WHERE " + where : "")

             );

            SqlParameter RowNum_start = new SqlParameter("RowNum_start", SqlDbType.Int);
            SqlParameter RowNum_end = new SqlParameter("RowNum_end", SqlDbType.Int);
            sqlParameters.Add(RowNum_start);
            sqlParameters.Add(RowNum_end);

            int pageNum = GetVal_int("pageNum");
            if (pageNum < 0)
                pageNum = 0;

            int pageSize = GetVal_int("pageSize");
            if (pageSize < 1)
                pageSize = defaultPageSize;

            RowNum_start.Value = 1 + pageNum * pageSize;
            RowNum_end.Value = (int)RowNum_start.Value + pageSize - 1;

            SqlCommand cmd = new SqlCommand(sql);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddRange(sqlParameters.ToArray());

            return cmd;
        }

        protected string GetSort()
        {
            string sortField = GetVal_Str("sort");
            if (AllowedFieldNames.Contains(sortField))
            {
                if (GetVal_int("sortorder") == 0)
                    sortField += " DESC";

            }
            else sortField = "";
            return sortField;
        }

        public string GetVal_Str(string name)
        {
            if (m_urlParameters[name] != null)
                return m_urlParameters[name];

            return "";
        }
        public int GetVal_int(string name)
        {
            int val = 0;

            if (m_urlParameters[name] != null)
            {
                int.TryParse(m_urlParameters[name], out val);
            }
            else
                val = 0;
            return val;
        }


        private static void CheckSqlParam(List<SqlParameter> sqlParameters, string paramName)
        {
            foreach(SqlParameter param in sqlParameters)
            {
                if (param.ParameterName.ToLower()==paramName.ToLower())
                {
                    throw new ApplicationException("В коллекции уже есть параметр с именем '" + paramName + "'!");
                }
            }
        }

        public static void AddSqlParam_int(List<SqlParameter> sqlParameters, string paramName,int value)
        {
            CheckSqlParam(sqlParameters, paramName);
            SqlParameter param = new SqlParameter(paramName, SqlDbType.Int);
            param.Value = value;
            sqlParameters.Add(param);
        }

        public static void AddSqlParam_str(List<SqlParameter> sqlParameters, string paramName, string value, int strLength)
        {
            CheckSqlParam(sqlParameters, paramName);
            
            if (strLength <= 0)
                strLength = 255;
            SqlParameter param = new SqlParameter(paramName, SqlDbType.VarChar, strLength);
            param.Value = value;
            sqlParameters.Add(param);
        }

    }
}
