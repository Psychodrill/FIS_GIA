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
        private XElement OrdersOfAdmission() {
            /*
             InstitutionExports\InstitutionExport\OrdersOfAdmission
InstitutionExports\InstitutionExport\OrdersOfAdmission\OrderOfAdmission
InstitutionExports\InstitutionExport\OrdersOfAdmission\OrderOfAdmission\Application
InstitutionExports\InstitutionExport\OrdersOfAdmission\OrderOfAdmission\Application\ApplicationNumber
InstitutionExports\InstitutionExport\OrdersOfAdmission\OrderOfAdmission\Application\OriginalDocumentsReceivedDate
InstitutionExports\InstitutionExport\OrdersOfAdmission\OrderOfAdmission\Application\RegistrationDate
InstitutionExports\InstitutionExport\OrdersOfAdmission\OrderOfAdmission\Application\OrderIdLevelBudget
InstitutionExports\InstitutionExport\OrdersOfAdmission\OrderOfAdmission\Application\FirstName
InstitutionExports\InstitutionExport\OrdersOfAdmission\OrderOfAdmission\Application\LastName
InstitutionExports\InstitutionExport\OrdersOfAdmission\OrderOfAdmission\Application\MiddleName
InstitutionExports\InstitutionExport\OrdersOfAdmission\OrderOfAdmission\OrderOfAdmissionUID
InstitutionExports\InstitutionExport\OrdersOfAdmission\OrderOfAdmission\OrderName
InstitutionExports\InstitutionExport\OrdersOfAdmission\OrderOfAdmission\OrderNumber
InstitutionExports\InstitutionExport\OrdersOfAdmission\OrderOfAdmission\OrderDate
InstitutionExports\InstitutionExport\OrdersOfAdmission\OrderOfAdmission\DirectionID
InstitutionExports\InstitutionExport\OrdersOfAdmission\OrderOfAdmission\DirectionName
InstitutionExports\InstitutionExport\OrdersOfAdmission\OrderOfAdmission\EducationFormID
InstitutionExports\InstitutionExport\OrdersOfAdmission\OrderOfAdmission\EducationLevelID
InstitutionExports\InstitutionExport\OrdersOfAdmission\OrderOfAdmission\FinanceSourceID
InstitutionExports\InstitutionExport\OrdersOfAdmission\OrderOfAdmission\IsBeneficiary
InstitutionExports\InstitutionExport\OrdersOfAdmission\OrderOfAdmission\Stage
InstitutionExports\InstitutionExport\OrdersOfAdmission\OrderOfAdmission\IsForeigner
             */

            #region sql

            string sql = @"
SELECT
	a.ApplicationNumber AS ApplicationNumber
	, a.OriginalDocumentsReceivedDate AS OriginalDocumentsReceivedDate
	, a.RegistrationDate AS RegistrationDate
	, a.OrderIdLevelBudget AS OrderIdLevelBudget
	, e.FirstName AS FirstName
	, e.LastName AS LastName
	, e.MiddleName AS MiddleName
	, ooa.[UID] AS OrderOfAdmissionUID
	, ooa.OrderName AS OrderName
	, ooa.OrderNumber AS OrderNumber
	, ooa.OrderDate AS OrderDate
    , ooa.DatePublished AS OrderDatePublished
	, cg.DirectionID AS DirectionID
	, d.Name AS DirectionName
	, ooa.EducationFormID AS EducationFormID
	, ooa.EducationLevelID AS EducationLevelID
	, ooa.EducationSourceID AS FinanceSourceID
	, ooa.IsForBeneficiary AS IsBeneficiary
	, ooa.Stage AS Stage
	, ooa.IsForeigner AS IsForeigner
FROM OrderOfAdmission AS ooa 
INNER JOIN [Application] AS a ON a.OrderOfAdmissionID = ooa.OrderID
INNER JOIN Entrant AS e ON e.EntrantID = a.EntrantID
INNER JOIN CompetitiveGroup AS cg ON cg.CompetitiveGroupID = a.OrderCompetitiveGroupID
INNER JOIN Direction AS d ON d.DirectionID = cg.DirectionID
WHERE a.InstitutionID=@InstitutionID AND a.StatusID=8 AND a.OrderOfAdmissionID IS NOT NULL
";

            #endregion

            var com = getSqlCommand(sql);

            #region Parameters
            com.Parameters.Add(new SqlParameter("InstitutionID", SqlDbType.Int) { Value = this._institutionID });
            #endregion

            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);

            var xOrdersOfAdmission = new XElement("OrdersOfAdmission");
            try {
                for(int i = 0; i < ds.Tables[0].Rows.Count; i++) {
                    DataRow drRow = ds.Tables[0].Rows[i];
                    xOrdersOfAdmission.Add(new XElement("OrderOfAdmission", new XElement(XApplication(ds.Tables[0], i)),
                        new XElement("OrderOfAdmissionUID", O2Str(drRow["OrderOfAdmissionUID"])),
                        new XElement("OrderName", O2Str(drRow["OrderName"])),
                        new XElement("OrderNumber", O2Str(drRow["OrderNumber"])),
                        new XElement("OrderDate", Date2Str(drRow["OrderDate"])),
                        new XElement("OrderDatePublished", Date2Str(drRow["OrderDatePublished"])),
                        new XElement("DirectionID", O2Str(drRow["DirectionID"])),
                        new XElement("DirectionName", O2Str(drRow["DirectionName"])),
                        new XElement("EducationFormID", O2Str(drRow["EducationFormID"])),
                        new XElement("EducationLevelID", O2Str(drRow["EducationLevelID"])),
                        new XElement("FinanceSourceID", O2Str(drRow["FinanceSourceID"])),
                        new XElement("IsBeneficiary", O2Str(drRow["IsBeneficiary"])),
                        new XElement("Stage", O2Str(drRow["Stage"])),
                        new XElement("IsForeigner", O2Str(drRow["IsForeigner"]))
                        ));
                }
            } catch(Exception) {
            }
            return xOrdersOfAdmission;
        }

        private XElement XApplication(DataTable dtTable, int rowsNumber) {
            var xApplication = new XElement("Application");
                DataRow drRow = dtTable.Rows[rowsNumber];
            xApplication.Add(
                new XElement("ApplicationNumber", O2Str(drRow["ApplicationNumber"])),
                new XElement("OriginalDocumentsReceivedDate", Date2Str(drRow["OriginalDocumentsReceivedDate"])),
                new XElement("RegistrationDate", Date2Str(drRow["RegistrationDate"])),
                new XElement("OrderIdLevelBudget", O2Str(drRow["OrderIdLevelBudget"])),
                new XElement("FirstName", O2Str(drRow["FirstName"])),
                new XElement("LastName", O2Str(drRow["LastName"])),
                new XElement("MiddleName", O2Str(drRow["MiddleName"]))
                );
            return xApplication;
        }
    }
}
