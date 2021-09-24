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

namespace GVUZ.ServiceModel.SQL
{
    public partial class InstitutionExporter
    {
        private XElement DistributedAdmissionVolume()
        {
            #region

            /*
InstitutionExports\InstitutionExport\DistributedAdmissionVolume
InstitutionExports\InstitutionExport\DistributedAdmissionVolume\Item
InstitutionExports\InstitutionExport\DistributedAdmissionVolume\Item\AdmissionVolumeUID
InstitutionExports\InstitutionExport\DistributedAdmissionVolume\Item\LevelBudget
InstitutionExports\InstitutionExport\DistributedAdmissionVolume\Item\NumberBudgetO
InstitutionExports\InstitutionExport\DistributedAdmissionVolume\Item\NumberBudgetOZ
InstitutionExports\InstitutionExport\DistributedAdmissionVolume\Item\NumberBudgetZ
InstitutionExports\InstitutionExport\DistributedAdmissionVolume\Item\NumberTargetO
InstitutionExports\InstitutionExport\DistributedAdmissionVolume\Item\NumberTargetOZ
InstitutionExports\InstitutionExport\DistributedAdmissionVolume\Item\NumberTargetZ
InstitutionExports\InstitutionExport\DistributedAdmissionVolume\Item\NumberQuotaO
InstitutionExports\InstitutionExport\DistributedAdmissionVolume\Item\NumberQuotaOZ
InstitutionExports\InstitutionExport\DistributedAdmissionVolume\Item\NumberQuotaZ


			*/

            #endregion

            string sql =@"
SELECT DAV.DistributedAdmissionVolumeID, DAV.IdLevelBudget as LevelBudget, 
DAV.NumberBudgetO, DAV.NumberBudgetOZ, DAV.NumberBudgetZ, 
DAV.NumberTargetO, DAV.NumberTargetOZ, DAV.NumberTargetZ, 
DAV.NumberQuotaO, DAV.NumberQuotaOZ, DAV.NumberQuotaZ,AV.UID AS AdmissionVolumeUID
FROM DistributedAdmissionVolume (NOLOCK)  DAV  
INNER JOIN AdmissionVolume (NOLOCK)  AV on DAV.AdmissionVolumeID=AV.AdmissionVolumeID
WHERE AV.InstitutionID=@InstitutionID
";

            var xDistributedAdmissionVolume = new XElement("DistributedAdmissionVolume");

            var com = getSqlCommand(sql);
            com.Parameters.Add(new SqlParameter("InstitutionID", SqlDbType.Int) {Value = this._institutionID});
            SqlDataReader r = null;
            try
            {
                r = com.ExecuteReader();
                while (r.Read())
                {
                    var xItem = new XElement("Item");
                    xItem.Add(new XElement("AdmissionVolumeUID", O2Str(r["AdmissionVolumeUID"])));
                    xItem.Add(new XElement("LevelBudget", O2Str(r["LevelBudget"])));

                    xItem.Add(new XElement("NumberBudgetO", O2Str(r["NumberBudgetO"])));
                    xItem.Add(new XElement("NumberBudgetOZ", O2Str(r["NumberBudgetOZ"])));
                    xItem.Add(new XElement("NumberBudgetZ", O2Str(r["NumberBudgetZ"])));

                    xItem.Add(new XElement("NumberTargetO", O2Str(r["NumberTargetO"])));
                    xItem.Add(new XElement("NumberTargetOZ", O2Str(r["NumberTargetOZ"])));
                    xItem.Add(new XElement("NumberTargetZ", O2Str(r["NumberTargetZ"])));

                    xItem.Add(new XElement("NumberQuotaO", O2Str(r["NumberQuotaO"])));
                    xItem.Add(new XElement("NumberQuotaOZ", O2Str(r["NumberQuotaOZ"])));
                    xItem.Add(new XElement("NumberQuotaZ", O2Str(r["NumberQuotaZ"])));
                    xDistributedAdmissionVolume.Add(xItem);
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (r != null)
                {
                    r.Close();
                }
            }

            return xDistributedAdmissionVolume;
        }
    }
}
