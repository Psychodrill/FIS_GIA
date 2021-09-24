using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Objects.SqlClient;
using System.Xml;
using System.Xml.Linq;
using GVUZ.ServiceModel.Import.Package;

namespace GVUZ.ServiceModel.SQL {
	public partial class InstitutionExporter {
		private XElement RecommendedLists() {

			var xRecommendedLists=new XElement("RecommendedLists");
            var xRecommendedList = new XElement("RecommendedList");
            var xStage = new XElement("Stage");
            var xRecLists = new XElement("RecLists");
            var xRecList = new XElement("RecList");
            var xFinSourceAndEduForms = new XElement("FinSourceAndEduForms");
            var xFinSourceAndEduForm = new XElement("FinSourceEduForm");
            var xEducationalLevelID = new XElement("EducationalLevelID");
            var xEducationFormID = new XElement("EducationFormID");
            var xCompetitiveGroupID = new XElement("CompetitiveGroupID");
            var xDirectionID = new XElement("DirectionID");

            var xApplication = new XElement("Application");
            var xApplicationNumber = new XElement("ApplicationNumber");
            var xRegistrationDate = new XElement("RegistrationDate");

            #region SQL

            string sql =@"

    SELECT rl.Stage FROM RecomendedLists (NOLOCK)  AS rl GROUP BY rl.Stage
	SELECT rl.RecListID FROM RecomendedLists (NOLOCK)  AS rl
	SELECT rl.EduFormID FROM RecomendedLists (NOLOCK)  AS rl GROUP BY rl.EduFormID

";

            #endregion

            var com = getSqlCommand(sql);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DataRow drRow = null;

		    try
		    {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    drRow = ds.Tables[0].Rows[i];
                    xRecommendedList = new XElement("RecommendedList");
                    xRecommendedList.Add(new XElement("Stage", Str2Str(drRow["Stage"])));
                    xRecommendedLists.Add(xRecommendedList);
		        }
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    drRow = ds.Tables[1].Rows[i];
                    xRecLists = new XElement("RecLists");
                    xRecList = new XElement("RecList");
                    

                    
                    xRecLists.Add(xRecList);
                }
                for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
		        {
		            drRow = ds.Tables[2].Rows[i];
                    xFinSourceAndEduForms = new XElement("FinSourceAndEduForms");
                    xFinSourceAndEduForm = new XElement("xFinSourceAndEduForm");
                    xFinSourceAndEduForms.Add(xFinSourceAndEduForm);
                    xRecList.Add(xFinSourceAndEduForms);
		        }
                
		        
		    }
		    catch (Exception)
		    {
		    }
		    finally
		    {

		    }

		    return xRecommendedLists;
		}	
	}
}
