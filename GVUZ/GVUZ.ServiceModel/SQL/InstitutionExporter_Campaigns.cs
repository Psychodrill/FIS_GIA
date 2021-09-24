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
using GVUZ.Model.Institutions;

namespace GVUZ.ServiceModel.SQL {

	public partial class InstitutionExporter {
		private XElement Campaigns() {

			#region
			/*
			InstitutionExports\InstitutionExport\Campaigns
			InstitutionExports\InstitutionExport\Campaigns\Campaign
			InstitutionExports\InstitutionExport\Campaigns\Campaign\UID
			InstitutionExports\InstitutionExport\Campaigns\Campaign\AdditionalSet
			InstitutionExports\InstitutionExport\Campaigns\Campaign\CampaignID
			InstitutionExports\InstitutionExport\Campaigns\Campaign\Name
			InstitutionExports\InstitutionExport\Campaigns\Campaign\StatusID
			InstitutionExports\InstitutionExport\Campaigns\Campaign\YearEnd
			InstitutionExports\InstitutionExport\Campaigns\Campaign\YearStart
			InstitutionExports\InstitutionExport\Campaigns\Campaign\EducationForms
			InstitutionExports\InstitutionExport\Campaigns\Campaign\EducationForms\EducationFormID
			InstitutionExports\InstitutionExport\Campaigns\Campaign\EducationLevels
			InstitutionExports\InstitutionExport\Campaigns\Campaign\EducationLevels\EducationLevel
			InstitutionExports\InstitutionExport\Campaigns\Campaign\EducationLevels\EducationLevel\Course
			InstitutionExports\InstitutionExport\Campaigns\Campaign\EducationLevels\EducationLevel\EducationLevelID
			InstitutionExports\InstitutionExport\Campaigns\Campaign\CampaignDates
			InstitutionExports\InstitutionExport\Campaigns\Campaign\CampaignDates\CampaignDate
			InstitutionExports\InstitutionExport\Campaigns\Campaign\CampaignDates\CampaignDate\UID
			InstitutionExports\InstitutionExport\Campaigns\Campaign\CampaignDates\CampaignDate\Course
			InstitutionExports\InstitutionExport\Campaigns\Campaign\CampaignDates\CampaignDate\EducationLevelID
			InstitutionExports\InstitutionExport\Campaigns\Campaign\CampaignDates\CampaignDate\EducationFormID
			InstitutionExports\InstitutionExport\Campaigns\Campaign\CampaignDates\CampaignDate\EducationSourceID
			InstitutionExports\InstitutionExport\Campaigns\Campaign\CampaignDates\CampaignDate\Stage
			InstitutionExports\InstitutionExport\Campaigns\Campaign\CampaignDates\CampaignDate\DateStart
			InstitutionExports\InstitutionExport\Campaigns\Campaign\CampaignDates\CampaignDate\DateEnd
			InstitutionExports\InstitutionExport\Campaigns\Campaign\CampaignDates\CampaignDate\DateOrder
			InstitutionExports\InstitutionExport\Campaigns\Campaign\IsForKrym
			 */
			#endregion

            string resul = "";

		    if (_filter != null)
		    {
		        if (_filter.Campaigns != null)
		        {
		            if (_filter.Campaigns.Any())
		            {
                        resul = string.Format("AND C.UID IN ('{0}')",
		                    string.Join("','", _filter.Campaigns));
		            }
		        }
		    }
		    var xCampaigns=new XElement("Campaigns");
			string sql=string.Format(@"
SELECT CampaignID, [UID], Name, StatusID, YearStart, YearEnd, EducationFormFlag
FROM [Campaign] (NOLOCK)  C
WHERE C.InstitutionID=@InstitutionID {0}

SELECT CEL.CampaignID, CEL.Course, EducationLevelID 
FROM CampaignEducationLevel (NOLOCK)  CEL 
INNER JOIN Campaign (NOLOCK)  C ON CEL.CampaignID=C.CampaignID
WHERE C.InstitutionID=@InstitutionID
ORDER BY CEL.CampaignID 

", resul);
			var com=getSqlCommand(sql);
			com.Parameters.Add(new SqlParameter("InstitutionID",SqlDbType.Int) { Value=this._institutionID });
			DataRow r=null;
			try {
				// com.Connection.Open должен быть открыт перед загрузкой всего пакета
				SqlDataAdapter da=new SqlDataAdapter(com);
				DataSet ds=new DataSet();
				da.Fill(ds);
				int EducationFormFlag;

				for(int i=0;i<ds.Tables[0].Rows.Count;i++) {
					r=ds.Tables[0].Rows[i];
					int CampaignID=(int)r["CampaignID"];
					XElement xCampaign=new XElement("Campaign");
                    xCampaign.Add(new XElement("CampaignID", O2Str(r["CampaignID"])));
					xCampaign.Add(new XElement("UID",O2Str(r["UID"])));
					xCampaign.Add(new XElement("Name",Str2Str(r["Name"])));
					//xCampaign.Add(new XElement("AdditionalSet",Bool2Str(r["AdditionalSet"]))); // <AdditionalSet xsi:nil="true" /> ????
                    xCampaign.Add(new XElement("StatusID", O2Str(r["StatusID"])));
					xCampaign.Add(new XElement("YearStart",O2Str(r["YearStart"])));
					xCampaign.Add(new XElement("YearEnd",O2Str(r["YearEnd"])));
					//xCampaign.Add(new XElement("IsFromKrym",Bool2Str(r["IsFromKrym"])));					

					EducationFormFlag=(int)r["EducationFormFlag"];

					XElement xEducationForms=CampaignEducationForms(EducationFormFlag);
					xCampaign.Add(xEducationForms);

					XElement xCampaignEducationLevels=EducationLevels(ds.Tables[1], CampaignID);
					xCampaign.Add(xCampaignEducationLevels);

					//XElement xCampaignDates=CampaignDates(ds.Tables[2], CampaignID);
					//xCampaign.Add(xCampaignDates);

					xCampaigns.Add(xCampaign);				
				}
				// Теперь снова бежим по xCampaigns и CampaignEducationForms(int CampaignId)
			} catch(Exception) {
			} finally {
			}
			return xCampaigns;
		}
		private XElement CampaignEducationForms(int EducationFormFlag) {
			var xEducationForms=new XElement("EducationForms");
			if((EducationFormFlag&1)!=0) { xEducationForms.Add(new XElement("EducationFormID",AdmissionItemTypeConstants.FullTimeTuition.ToString())); }
			if((EducationFormFlag&2)!=0) { xEducationForms.Add(new XElement("EducationFormID",AdmissionItemTypeConstants.MixedTuition.ToString())); }
			if((EducationFormFlag&4)!=0) { xEducationForms.Add(new XElement("EducationFormID",AdmissionItemTypeConstants.PostalTuition.ToString())); }
			return xEducationForms;
		}
		private XElement EducationLevels(DataTable T,int CampaignID) {
			XElement xEducationLevels=new XElement("EducationLevels");
			bool found=false;
			XElement xEducationLevel=new XElement("EducationLevel");
			int id=0;
			DataRow r;
			for(int i=0; i<T.Rows.Count;i++){
				r=T.Rows[i];
				id=(int)r["CampaignID"];
				if(id==CampaignID) {
					found=true;
					xEducationLevel=new XElement("EducationLevel");
					xEducationLevel.Add(new XElement("Course",O2Str(r["Course"])));
					xEducationLevel.Add(new XElement("EducationLevelID",O2Str(r["EducationLevelID"])));
					xEducationLevels.Add(xEducationLevel);
				} else {
					if(found) { break; }
				}
			}
			return xEducationLevels;
		}

		private XElement CampaignDates(DataTable T,int CampaignID) {
			XElement xCampaignDates=new XElement("CampaignDates");
            return xCampaignDates;
            // TODO: Выпилить, этой таблицы уже нет!

			#region
			/*
InstitutionExports\InstitutionExport\Campaigns\Campaign\CampaignDates\CampaignDate
InstitutionExports\InstitutionExport\Campaigns\Campaign\CampaignDates\CampaignDate\UID
InstitutionExports\InstitutionExport\Campaigns\Campaign\CampaignDates\CampaignDate\Course
InstitutionExports\InstitutionExport\Campaigns\Campaign\CampaignDates\CampaignDate\EducationLevelID
InstitutionExports\InstitutionExport\Campaigns\Campaign\CampaignDates\CampaignDate\EducationFormID
InstitutionExports\InstitutionExport\Campaigns\Campaign\CampaignDates\CampaignDate\EducationSourceID
InstitutionExports\InstitutionExport\Campaigns\Campaign\CampaignDates\CampaignDate\Stage
InstitutionExports\InstitutionExport\Campaigns\Campaign\CampaignDates\CampaignDate\DateStart
InstitutionExports\InstitutionExport\Campaigns\Campaign\CampaignDates\CampaignDate\DateEnd
InstitutionExports\InstitutionExport\Campaigns\Campaign\CampaignDates\CampaignDate\DateOrder

				dto.CampaignDates = dbCampaign.CampaignDate.Where(x => x.IsActive)
					.Select(x => new CampaignDateDto
					             {
					             	Course = x.Course.ToString(),
									EducationLevelID = x.EducationLevelID.ToString(),
									EducationFormID = x.EducationFormID.ToString(),
									EducationSourceID = x.EducationSourceID.ToString(),
									Stage = x.Stage != 0 ? x.Stage.ToString() : null,
									UID = x.UID,
									DateStart = x.DateStart.GetNullableDateAsString(),
									DateEnd = x.DateEnd.GetNullableDateAsString(),
									DateOrder = x.DateOrder.GetNullableDateAsString()
					             }).ToArray();
			*/
			#endregion
			//XElement xCampaignDate=new XElement("CampaignDate");
			//int id=0;
			//bool found=false;
			//DataRow r;
			//for(int i=0;i<T.Rows.Count;i++) {
			//	r=T.Rows[i];
			//	id=Convert.ToInt32(r["CampaignID"] as Int32?);
			//	if(id==CampaignID) {
			//		found=true;
			//		xCampaignDate=new XElement("CampaignDate");
			//		xCampaignDate.Add(new XElement("UID",O2Str(r["UID"])));
			//		xCampaignDate.Add(new XElement("Course",O2Str(r["Course"])));
			//		xCampaignDate.Add(new XElement("EducationLevelID",O2Str(r["EducationLevelID"])));
			//		xCampaignDate.Add(new XElement("EducationFormID",O2Str(r["EducationFormID"])));
			//		xCampaignDate.Add(new XElement("EducationSourceID",O2Str(r["EducationSourceID"])));
			//		xCampaignDate.Add(new XElement("Stage",O2Str(r["Stage"])));
			//		xCampaignDate.Add(new XElement("DateStart",Date2Str(r["DateStart"])));
			//		xCampaignDate.Add(new XElement("DateEnd",Date2Str(r["DateEnd"])));
			//		xCampaignDate.Add(new XElement("DateOrder",O2Str(r["DateOrder"])));
			//		xCampaignDates.Add(xCampaignDate);
			//	} else {
			//		if(found) { 
			//			break; 
			//		}
			//	}
			//}
			//return xCampaignDates;
		}
	}
}
