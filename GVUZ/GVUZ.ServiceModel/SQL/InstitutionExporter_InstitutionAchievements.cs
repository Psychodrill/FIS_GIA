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

		private XElement InstitutionAchievements() {
			#region
			/*
InstitutionExports\InstitutionExport\InstitutionAchievements
InstitutionExports\InstitutionExport\InstitutionAchievements\InstitutionAchievement
InstitutionExports\InstitutionExport\InstitutionAchievements\InstitutionAchievement\IAUID
InstitutionExports\InstitutionExport\InstitutionAchievements\InstitutionAchievement\Name
InstitutionExports\InstitutionExport\InstitutionAchievements\InstitutionAchievement\IdCategory
InstitutionExports\InstitutionExport\InstitutionAchievements\InstitutionAchievement\MaxValue
InstitutionExports\InstitutionExport\InstitutionAchievements\InstitutionAchievement\CampaignUID
			*/
			#endregion

			string sql=@"
SELECT IA.UID as IAUID,  IA.Name, IA.IdCategory, IA.MaxValue, C.UID as CampaignUID
FROM [Campaign] (NOLOCK)  C INNER JOIN InstitutionAchievements (NOLOCK)  IA on IA.CampaignID=C.CampaignID
WHERE C.InstitutionID=@InstitutionID
";
			var xInstitutionAchievements=new XElement("InstitutionAchievements");
			var com=getSqlCommand(sql);
			com.Parameters.Add(new SqlParameter("InstitutionID",SqlDbType.Int) { Value=this._institutionID });
			SqlDataReader r=null;
			try {
				r=com.ExecuteReader();
				while(r.Read()) {
					var xInstitutionAchievement=new XElement("InstitutionAchievement");
					xInstitutionAchievement.Add(new XElement("IAUID",O2Str(r["IAUID"])));
					xInstitutionAchievement.Add(new XElement("Name",O2Str(r["Name"])));
					xInstitutionAchievement.Add(new XElement("IdCategory",O2Str(r["IdCategory"])));
					xInstitutionAchievement.Add(new XElement("MaxValue",O2Str(r["MaxValue"])));
					xInstitutionAchievement.Add(new XElement("CampaignUID",O2Str(r["CampaignUID"])));

					xInstitutionAchievements.Add(xInstitutionAchievement);
				}
			} catch(Exception) {
			} finally {
				if(r!=null) { r.Close(); }
			}


			return xInstitutionAchievements;
		}

	}
}
