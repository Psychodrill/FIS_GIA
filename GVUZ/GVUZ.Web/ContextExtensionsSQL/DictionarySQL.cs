using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using GVUZ.Web.ViewModels;
using System.Configuration;
using System.Web.Configuration;

namespace GVUZ.Web.ContextExtensionsSQL {

	public class DictionarySQL {
		private static string _connectionString;

		private static string ConnectionString {
			get {
				if(String.IsNullOrEmpty(_connectionString)) {
					ConnectionStringSettings css=ConfigurationManager.ConnectionStrings["Main"];
					_connectionString=css.ConnectionString;
				}
				return _connectionString;
			}
		}

		public static List<DictionaryModel> GetDictionary(DictionaryFilterModel filter) {
			List<DictionaryModel> list=new List<DictionaryModel>();
			List<SqlParameter> Params=new List<SqlParameter>();
			string sql="";
			string where="WHERE 1=1";

			//InstitutionList=dbContext.Institution.OrderBy(x => x.FullName).Select(x => x.FullName).ToArray();
			//QualificationList=dbContext.LoadQualifications().ToArray();
			//SpecialityList=dbContext.FindSpecialityByQualification(QualificationTypeName).ToArray();

			switch(filter.Dictionary){
				case "IdentityDocumentType":
					//sql=@"SELECT IdentityDocumentType as key, as as Title FROM IdentityDocumentType ORDER BY Name";
					if(!String.IsNullOrEmpty(filter.Title)  ){
						where=" AND (like @Title+'%') ";
						Params.Add(new SqlParameter(filter.Key, filter.Title));
					}
					foreach(var ff in filter.Fields){
						switch(ff.Key){
							case "parentId":		where+=" AND parentId="+ff.Value;			break;
							default:					where+=" AND "+ff.Key+"="+ff.Value;			break;
						}
					}
					sql=@"SELECT IdentityDocumentType as key, as as title FROM IdentityDocumentType (NOLOCK) "+where+" ORDER BY Name";
				break;

				default:
				sql=@"SELECT id as key, Name as title FROM "+filter.Dictionary+" (NOLOCK)  "+where+" ORDER BY Title";
				break;
			}

			using(SqlConnection con=new SqlConnection(ConnectionString)) {
				SqlCommand com=new SqlCommand(sql,con);
				foreach(var p in Params) {		com.Parameters.Add(p);}
				DictionaryModel d=null;
				try {
					con.Open();
					SqlDataReader r=com.ExecuteReader();
					if(r.Read()) {
						d=new DictionaryModel();
						d.Key=r["key"].ToString();
						d.Title=r["title"] as string;
						//d.Fields допопля
						if(r.FieldCount>2){
							string fname, svalue=null;
							object fvalue;
							for(int i=0;i< r.FieldCount;i++){
								fname=r.GetName(i);
								if(fname=="key"||fname=="title") { continue; }
								fvalue=r.GetValue(i);
								if(fvalue!=null) {	svalue=fvalue.ToString();}
								d.Fields.Add(fname, svalue);
							}
						}
						list.Add(d);
					}
					r.Close();
					con.Close();
				} catch(SqlException e) {
					throw e;
				} catch(Exception e) {
					throw e;
				} finally {
					if(con.State!=ConnectionState.Closed) { con.Close(); }
				}
			}
			return list;
		}

		public static List<string> GetQualifications(string filter) {
			List<string> list=new List<string>();
			string sql=@"
SELECT DISTINCT TOP 10 QUALIFICATIONNAME as Name  
FROM Direction (NOLOCK) 
WHERE QUALIFICATIONNAME IS NOT NULL AND  QUALIFICATIONNAME!=''  AND  QUALIFICATIONNAME like @filter+'%'
ORDER BY QUALIFICATIONNAME
";
			using(SqlConnection con=new SqlConnection(ConnectionString)) {
				SqlCommand com=new SqlCommand(sql,con);
				com.Parameters.Add(new SqlParameter("filter",SqlDbType.VarChar) { Value=filter });
				try {
					con.Open();
					SqlDataReader r=com.ExecuteReader();
					while(r.Read()) {
						list.Add(r["Name"] as string);
					}
					r.Close();
					con.Close();
				} catch(SqlException e) {
					throw e;
				} catch(Exception e) {
					throw e;
				} finally {
					if(con.State!=ConnectionState.Closed) { con.Close(); }
				}
			}
			return list;
		}

		// SpecialityByQualification
		public static List<string> GetSpecialityByQualification(string Qualification) {
			List<string> list=new List<string>();
			if(String.IsNullOrEmpty(Qualification)) {	return list;}
			string sql=@"
SELECT ltrim(rtrim(ISNULL(NewCode, '') + ' / ' + ISNULL(ltrim(rtrim(Code)), '') + ' ' + ISNULL(ltrim(Name), ''))) as Name
              FROM Direction (NOLOCK) 
              WHERE  QUALIFICATIONNAME = @Qualification
              ORDER BY newcode
";
			using(SqlConnection con=new SqlConnection(ConnectionString)) {
				SqlCommand com=new SqlCommand(sql,con);
				com.Parameters.Add(new SqlParameter("Qualification",SqlDbType.VarChar) { Value=Qualification });
				try {
					con.Open();
					SqlDataReader r=com.ExecuteReader();
					while(r.Read()) {
						list.Add(r["Name"] as string);
					}
					r.Close();
					con.Close();
				} catch(SqlException e) {
					throw e;
				} catch(Exception e) {
					throw e;
				} finally {
					if(con.State!=ConnectionState.Closed) { con.Close(); }
				}
			}
			return list;
		}

		public static List<IDName> GetOlympicDiplomType() {
			List<IDName> list=new List<IDName>();
			list.Add(new IDName() { ID=1,Name="победитель" });
			list.Add(new IDName() { ID=2,Name="призер" });
			//			OlympicDiplomTypeID	Name
			return list;
		}

		public static List<IDName> GetDisabilityList() {
			List<IDName> l=new List<IDName>();
			string sql=@" SELECT DisabilityID as ID, [Name] FROM [DisabilityType] (NOLOCK)  ";
			using(SqlConnection con=new SqlConnection(ConnectionString)) {
				SqlCommand com=new SqlCommand(sql,con);
				try {
					con.Open();
					SqlDataReader r=com.ExecuteReader();
					while(r.Read()) {
						l.Add(new IDName() { ID=(int)r["ID"],Name=r["Name"] as string });
					}
					r.Close();
					con.Close();
				} catch(SqlException e) {
					throw e;
				} catch(Exception e) {
					throw e;
				} finally {
					if(con.State!=ConnectionState.Closed) { con.Close(); }
				}
			}
			return l;
		}

        public static List<GVUZ.Model.Entrants.UniDocuments.SubjectBriefData> GetSubjectList()
	    {
            List<GVUZ.Model.Entrants.UniDocuments.SubjectBriefData> l = new List<GVUZ.Model.Entrants.UniDocuments.SubjectBriefData>();
				string sql=@"SELECT s.SubjectID, s.Name, s.IsEge FROM [Subject] AS s (NOLOCK) ";
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                try
                {
                    con.Open();
                    SqlDataReader r = com.ExecuteReader();
                    while (r.Read())
                    {
                        l.Add(new GVUZ.Model.Entrants.UniDocuments.SubjectBriefData() { SubjectID = (int)r["SubjectID"], SubjectName = r["Name"] as string, IsEge = Convert.ToBoolean(r["IsEge"] as bool?) });
                    }
                    r.Close();
                    con.Close();
                }
                catch (SqlException e)
                {
                    throw e;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    if (con.State != ConnectionState.Closed) { con.Close(); }
                }
            }
            return l;
	    }

	}
}