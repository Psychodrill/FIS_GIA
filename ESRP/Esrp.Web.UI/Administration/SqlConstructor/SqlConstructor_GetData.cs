namespace Esrp.Web.Administration.SqlConstructor
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.Data.SqlClient;

    /// <summary>
    /// The sql constructor_ get data.
    /// </summary>
    public abstract class SqlConstructor_GetData
    {
        #region Constants and Fields

        public string defaultOrderField = "ID";

        public int defaultPageSize = 20;

        protected readonly StringCollection AllowedFieldNames = new StringCollection(); // список разрешённых полей

        protected NameValueCollection m_urlParameters;

        #endregion

        // список разрешённых полей
        #region Public Properties

        /// <summary>
        /// Gets Fields.
        /// </summary>
        public StringCollection Fields
        {
            get
            {
                return this.AllowedFieldNames;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add sql param_int.
        /// </summary>
        /// <param name="sqlParameters">
        /// The sql parameters.
        /// </param>
        /// <param name="paramName">
        /// The param name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public static void AddSqlParam_int(List<SqlParameter> sqlParameters, string paramName, int value)
        {
            CheckSqlParam(sqlParameters, paramName);
            var param = new SqlParameter(paramName, SqlDbType.Int);
            param.Value = value;
            sqlParameters.Add(param);
        }

        /// <summary>
        /// The add sql param_str.
        /// </summary>
        /// <param name="sqlParameters">
        /// The sql parameters.
        /// </param>
        /// <param name="paramName">
        /// The param name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="strLength">
        /// The str length.
        /// </param>
        public static void AddSqlParam_str(
            List<SqlParameter> sqlParameters, string paramName, string value, int strLength)
        {
            CheckSqlParam(sqlParameters, paramName);

            if (strLength <= 0)
            {
                strLength = 255;
            }

            var param = new SqlParameter(paramName, SqlDbType.VarChar, strLength);
            param.Value = value;
            sqlParameters.Add(param);
        }

        /// <summary>
        /// The get count orgs sql.
        /// </summary>
        /// <returns>
        /// </returns>
        public SqlCommand GetCountOrgsSQL()
        {
            List<SqlParameter> sqlParameters;
            string where = this.GetWhere(out sqlParameters);

            string sql = string.Format(
                "SELECT @rowCount=count(*) FROM (" + this.getMainSQL() + ") T {0} ", 
                where.Length > 0 ? "WHERE " + where : string.Empty);

            var rowCount = new SqlParameter("rowCount", SqlDbType.Int);
            rowCount.Direction = ParameterDirection.Output;
            sqlParameters.Add(rowCount);

            var cmd = new SqlCommand(sql);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddRange(sqlParameters.ToArray());

            return cmd;
        }

        /// <summary>
        /// The get sql.
        /// </summary>
        /// <returns>
        /// </returns>
        public SqlCommand GetSQL()
        {
            string sort = this.GetSort();

            List<SqlParameter> sqlParameters;
            string where = this.GetWhere(out sqlParameters);

            string sql =
                string.Format(
                    @"
                SELECT * FROM
                (   SELECT row_number() over (order by {0} desc) RowNum, T.*
                    FROM ("
                    + this.getMainSQL()
                    +
                    @"
                ) T {1} ) T
                WHERE RowNum BETWEEN @RowNum_start AND @RowNum_end
                ORDER BY RowNum", 
                    sort.Length > 0 ? sort + ", " + this.defaultOrderField : this.defaultOrderField, 
                    where.Length > 0 ? "WHERE " + where : string.Empty);

            var RowNum_start = new SqlParameter("RowNum_start", SqlDbType.Int);
            var RowNum_end = new SqlParameter("RowNum_end", SqlDbType.Int);
            sqlParameters.Add(RowNum_start);
            sqlParameters.Add(RowNum_end);

            int pageNum = this.GetVal_int("pageNum");
            if (pageNum < 0)
            {
                pageNum = 0;
            }

            int pageSize = this.GetVal_int("pageSize");
            if (pageSize < 1)
            {
                pageSize = this.defaultPageSize;
            }

            RowNum_start.Value = 1 + pageNum * pageSize;
            RowNum_end.Value = (int)RowNum_start.Value + pageSize - 1;

            var cmd = new SqlCommand(sql);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddRange(sqlParameters.ToArray());

            return cmd;
        }

        /// <summary>
        /// The get val_ str.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The get val_ str.
        /// </returns>
        public string GetVal_Str(string name)
        {
            if (this.m_urlParameters[name] != null)
            {
                return this.m_urlParameters[name];
            }

            return string.Empty;
        }

        /// <summary>
        /// The get val_int.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The get val_int.
        /// </returns>
        public int GetVal_int(string name)
        {
            int val = 0;

            if (this.m_urlParameters[name] != null)
            {
                int.TryParse(this.m_urlParameters[name], out val);
            }
            else
            {
                val = 0;
            }

            return val;
        }

        /// <summary>
        /// Получить список int значений для параметра из строки значений
        /// </summary>
        /// <param name="name">Имя параметра в строке запроса</param>
        /// <returns>список int значении параметра</returns>
        public List<int> GetListValueInt(string name)
        {
            var result = new List<int>();

            if (this.m_urlParameters[name] != null)
            {
                var values = this.m_urlParameters[name].Split(Convert.ToChar(","));
                foreach (var value in values)
                {
                    int val;
                    if (int.TryParse(value, out val))
                    {
                        result.Add(val);
                    }
                }
            }
            else
            {
                 result.Add(0);
            }

            return result;
        }

        /// <summary>
        /// The has filter.
        /// </summary>
        /// <returns>
        /// The has filter.
        /// </returns>
        public bool HasFilter()
        {
            var parameters = new List<SqlParameter>();
            var whereExpr = new List<string>();
            this.CreateParameters(parameters, whereExpr);

            return whereExpr.Count > 0;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The create parameters.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="whereExpr">
        /// The where expr.
        /// </param>
        protected abstract void CreateParameters(List<SqlParameter> parameters, List<string> whereExpr);

        /// <summary>
        /// The get sort.
        /// </summary>
        /// <returns>
        /// The get sort.
        /// </returns>
        protected string GetSort()
        {
            string sortField = this.GetVal_Str("sort");
            if (this.AllowedFieldNames.Contains(sortField))
            {
                if (this.GetVal_int("sortorder") == 0)
                {
                    sortField += " DESC";
                }
            }
            else
            {
                sortField = string.Empty;
            }

            return sortField;
        }

        /// <summary>
        /// The get where.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// The get where.
        /// </returns>
        protected string GetWhere(out List<SqlParameter> parameters)
        {
            parameters = new List<SqlParameter>();
            var whereExpr = new List<string>();
            this.CreateParameters(parameters, whereExpr);
            return string.Join(" AND ", whereExpr.ToArray());
        }

        /// <summary>
        /// The get main sql.
        /// </summary>
        /// <returns>
        /// The get main sql.
        /// </returns>
        protected abstract string getMainSQL();

        private static void CheckSqlParam(List<SqlParameter> sqlParameters, string paramName)
        {
            foreach (SqlParameter param in sqlParameters)
            {
                if (param.ParameterName.ToLower() == paramName.ToLower())
                {
                    throw new ApplicationException("В коллекции уже есть параметр с именем '" + paramName + "'!");
                }
            }
        }

        #endregion
    }
}