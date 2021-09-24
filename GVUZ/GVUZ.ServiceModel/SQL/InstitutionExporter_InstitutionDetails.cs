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

        private List<XElement> InstitutionDetails()
        {
            string sql = @"
SELECT I.InstitutionID, 
CAST((CASE WHEN ( (I.MainEsrpOrgId IS NOT NULL ) AND (I.MainEsrpOrgId != 0) ) THEN 1 ELSE 0 END ) as bit)  as IsFilial,
FullName, BriefName, FormOfLawID, RegionID, [Address], Phone, HasMilitaryDepartment, INN, OGRN, IL.LicenseNumber, LicenseDate, IA.Accreditation
FROM Institution (NOLOCK)   I 
LEFT JOIN InstitutionLicense (NOLOCK)  IL on I.InstitutionID=IL.InstitutionID 
LEFT JOIN InstitutionAccreditation (NOLOCK)  IA on I.InstitutionID=IA.InstitutionID 
WHERE I.InstitutionID= @InstitutionID

UNION

SELECT I.InstitutionID, 
CAST((CASE WHEN ( (I.MainEsrpOrgId IS NOT NULL ) AND (I.MainEsrpOrgId != 0) ) THEN 1 ELSE 0 END ) as bit)  as IsFilial,
FullName, BriefName, FormOfLawID, RegionID, [Address], Phone, HasMilitaryDepartment, INN, OGRN, IL.LicenseNumber, LicenseDate, IA.Accreditation
FROM Institution (NOLOCK)   I 
LEFT JOIN InstitutionLicense (NOLOCK)  IL on I.InstitutionID=IL.InstitutionID 
LEFT JOIN InstitutionAccreditation (NOLOCK)  IA on I.InstitutionID=IA.InstitutionID 
WHERE I.MainEsrpOrgId =(SELECT I.EsrpOrgID FROM Institution(NOLOCK) I WHERE I.InstitutionID = @InstitutionID)
";
            var com = getSqlCommand(sql);
            com.Parameters.Add(new SqlParameter("InstitutionID", SqlDbType.Int) {Value = this._institutionID});

            List<XElement> xList = new List<XElement>();
            
            SqlDataReader r = null;
            try
            {
                r = com.ExecuteReader();
                while (r.Read())
                {
                    var xInstitutionDetails = new XElement("InstitutionDetails");
                    xInstitutionDetails.Add(new XElement("InstitutionID", Int2Str(r["InstitutionID"])),
                        new XElement("IsFilial", Bool2Str(r["IsFilial"])),
                        new XElement("FullName", Str2Str(r["FullName"])),
                        new XElement("BriefName", Str2Str(r["BriefName"])),
                        new XElement("FormOfLawID", Int2Str(r["FormOfLawID"])),
                        new XElement("RegionID", Int2Str(r["RegionID"])),
                        new XElement("Address", Str2Str(r["Address"])),
                        new XElement("Phone", Str2Str(r["Phone"])),
                        new XElement("HasMilitaryDepartment", Bool2Str(r["HasMilitaryDepartment"])),
                        new XElement("INN", Str2Str(r["INN"])),
                        new XElement("OGRN", Str2Str(r["OGRN"])),
                        new XElement("LicenseNumber", Str2Str(r["LicenseNumber"])),
                        new XElement("LicenseDate", Date2Str(r["LicenseDate"])),
                        new XElement("Accreditation", Str2Str(r["Accreditation"])));
                    xList.Add(xInstitutionDetails);
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

            return xList;
        }
    }
}
