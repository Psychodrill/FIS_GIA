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
		private XElement AdmissionVolume() {
			#region
			/*
			InstitutionExports\InstitutionExport\AdmissionVolume
			InstitutionExports\InstitutionExport\AdmissionVolume\AdmissionVolumeItems
			InstitutionExports\InstitutionExport\AdmissionVolume\AdmissionVolumeItems\Item
			InstitutionExports\InstitutionExport\AdmissionVolume\AdmissionVolumeItems\Item\UID
			InstitutionExports\InstitutionExport\AdmissionVolume\AdmissionVolumeItems\Item\CampaignUID
			InstitutionExports\InstitutionExport\AdmissionVolume\AdmissionVolumeItems\Item\Course
			InstitutionExports\InstitutionExport\AdmissionVolume\AdmissionVolumeItems\Item\DirectionID
			InstitutionExports\InstitutionExport\AdmissionVolume\AdmissionVolumeItems\Item\EducationLevelID
			InstitutionExports\InstitutionExport\AdmissionVolume\AdmissionVolumeItems\Item\NumberBudgetO
			InstitutionExports\InstitutionExport\AdmissionVolume\AdmissionVolumeItems\Item\NumberBudgetOZ
			InstitutionExports\InstitutionExport\AdmissionVolume\AdmissionVolumeItems\Item\NumberBudgetZ
			InstitutionExports\InstitutionExport\AdmissionVolume\AdmissionVolumeItems\Item\NumberPaidO
			InstitutionExports\InstitutionExport\AdmissionVolume\AdmissionVolumeItems\Item\NumberPaidOZ
			InstitutionExports\InstitutionExport\AdmissionVolume\AdmissionVolumeItems\Item\NumberPaidZ
			InstitutionExports\InstitutionExport\AdmissionVolume\AdmissionVolumeItems\Item\NumberTargetO
			InstitutionExports\InstitutionExport\AdmissionVolume\AdmissionVolumeItems\Item\NumberTargetOZ
			InstitutionExports\InstitutionExport\AdmissionVolume\AdmissionVolumeItems\Item\NumberTargetZ
			InstitutionExports\InstitutionExport\AdmissionVolume\AdmissionVolumeItems\Item\NumberQuotaO
			InstitutionExports\InstitutionExport\AdmissionVolume\AdmissionVolumeItems\Item\NumberQuotaOZ
			InstitutionExports\InstitutionExport\AdmissionVolume\AdmissionVolumeItems\Item\NumberQuotaZ

		private static AdmissionVolumeCollectionDto GenerateAdmissionVolume(int institutionID, EntrantsEntities dbContext, InstitutionInformationFilter filter)
		{
			var allowedDirs = dbContext.AllowedDirections.Where(x => x.InstitutionID == institutionID);
            
            var campaigns = dbContext.Campaign
                .Include(x => x.CampaignEducationLevel)
                .Include(x => x.CampaignDate)
                .Where(x => x.InstitutionID == institutionID);

            if (!filter.IsEmpty && filter.AdmissionVolume != null)
            {
                var campaignUids = new HashSet<string>();
                campaignUids.UnionWith(filter.AdmissionVolume.Where(c => !string.IsNullOrEmpty(c)).Select(c => c));

                if (campaignUids.Count > 0)
                    campaigns = campaigns.WhereIn(c => c.UID, campaignUids, chunkSize: 500).AsQueryable();
            }

			return new AdmissionVolumeCollectionDto
			{
				Collection = dbContext.AdmissionVolume
					.Include(x => x.Campaign)
				    .Where(x => x.InstitutionID == institutionID)
				    .ToList()
					.Where(x => allowedDirs.Any(y => y.DirectionID == x.DirectionID && y.AdmissionItemTypeID == x.AdmissionItemTypeID))
                    .Where(x => campaigns.Select(y => y.CampaignID).Contains(x.Campaign.CampaignID))
				    .Select(x =>
				           	{
				           		var r = Mapper.Map(x, new AdmissionVolumeDto());
				           		r.CampaignUID = x.Campaign.UID;
				           		return r;
				           	})
				    .ToArray()
			};
		}
			*/
			#endregion

            string resul = "";

		    if (_filter != null)
		    {
		        if (_filter.AdmissionVolume != null)
		        {
		            if (_filter.AdmissionVolume.Any())
		            {
		                resul = string.Format("AND AV.AdmissionVolumeID IN ({0})",
		                    string.Join(",", _filter.AdmissionVolume));
		            }
		        }
		    }
		    string sql=string.Format(@"
SELECT AV.UID, C.UID as CampaignUID, AV.Course, AV.DirectionID, AdmissionItemTypeID as EducationLevelID, 
NumberBudgetO, NumberBudgetOZ, NumberBudgetZ, NumberPaidO, NumberPaidOZ, NumberPaidZ,
NumberTargetO, NumberTargetOZ, NumberTargetZ, NumberQuotaO, NumberQuotaOZ, NumberQuotaZ, AV.ParentDirectionID
FROM AdmissionVolume (NOLOCK)  AV  
INNER JOIN Campaign (NOLOCK)  C ON AV.CampaignID=C.CampaignID
WHERE AV.InstitutionID=@InstitutionID {0}
",resul);
			var xAdmissionVolume=new XElement("AdmissionVolume");
			var xAdmissionVolumeItems=new XElement("AdmissionVolumeItems");
			xAdmissionVolume.Add(xAdmissionVolumeItems);

			var com=getSqlCommand(sql);
			com.Parameters.Add(new SqlParameter("InstitutionID",SqlDbType.Int) { Value=this._institutionID });
			SqlDataReader r=null;
			try {
				r=com.ExecuteReader();
				while(r.Read()) {
					var xItem=new XElement("Item");
					xItem.Add(new XElement("UID",O2Str(r["UID"])));
					xItem.Add(new XElement("CampaignUID",O2Str(r["CampaignUID"])));
					xItem.Add(new XElement("Course",O2Str(r["Course"])));
					xItem.Add(new XElement("DirectionID",O2Str(r["DirectionID"])));
					xItem.Add(new XElement("EducationLevelID",O2Str(r["EducationLevelID"])));
                    xItem.Add(new XElement("ParentDirectionID", O2Str(r["ParentDirectionID"])));

                    xItem.Add(new XElement("NumberBudgetO",O2Str(r["NumberBudgetO"])));
					xItem.Add(new XElement("NumberBudgetOZ",O2Str(r["NumberBudgetOZ"])));
					xItem.Add(new XElement("NumberBudgetZ",O2Str(r["NumberBudgetZ"])));

					xItem.Add(new XElement("NumberPaidO",O2Str(r["NumberPaidO"])));
					xItem.Add(new XElement("NumberPaidOZ",O2Str(r["NumberPaidOZ"])));
					xItem.Add(new XElement("NumberPaidZ",O2Str(r["NumberPaidZ"])));

					xItem.Add(new XElement("NumberTargetO",O2Str(r["NumberTargetO"])));
					xItem.Add(new XElement("NumberTargetOZ",O2Str(r["NumberTargetOZ"])));
					xItem.Add(new XElement("NumberTargetZ",O2Str(r["NumberTargetZ"])));

					xItem.Add(new XElement("NumberQuotaO",O2Str(r["NumberQuotaO"])));
					xItem.Add(new XElement("NumberQuotaOZ",O2Str(r["NumberQuotaOZ"])));
					xItem.Add(new XElement("NumberQuotaZ",O2Str(r["NumberQuotaZ"])));
					xAdmissionVolumeItems.Add(xItem);
				}
			} catch(Exception) {
			} finally {
				if(r!=null) { r.Close(); }
			}
			return xAdmissionVolume;
		}

	}
}
