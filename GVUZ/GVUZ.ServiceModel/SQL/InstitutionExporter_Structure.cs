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
		private XElement Structure() {
			var xStructure=new XElement("Structure");

            #region SQL

		    string sql =@"
SELECT
i.FullName AS Name
,i.InstitutionID AS InstitutionID
FROM InstitutionStructure (NOLOCK)  AS is1
INNER JOIN InstitutionItem (NOLOCK)  AS ii
		  ON ii.InstitutionItemID = is1.InstitutionItemID
INNER JOIN Institution (NOLOCK)  AS i
          ON i.InstitutionID = ii.InstitutionID
WHERE i.InstitutionID=@InstitutionID

GROUP BY i.FullName
,i.InstitutionID";

            #endregion

            var com = getSqlCommand(sql);
            com.Parameters.Add(new SqlParameter("InstitutionID", SqlDbType.Int) { Value = this._institutionID });

            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DataRow drRow = null;

//            InstitutionExports\InstitutionExport\Structure
//InstitutionExports\InstitutionExport\Structure\ChildStructure
//InstitutionExports\InstitutionExport\Structure\ChildStructure\Structure
//InstitutionExports\InstitutionExport\Structure\ChildStructure\Structure\Name
//InstitutionExports\InstitutionExport\Structure\ChildStructure\Structure\InstitutionID

		    try
		    {
		        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
		        {
		            drRow = ds.Tables[0].Rows[i];
		            xStructure.Add(new XElement("ChildStructure",
		                new XElement("Structure", new XElement("Name", Str2Str(drRow["Name"])),
		                    new XElement("InstitutionID", Int2Str(drRow["InstitutionID"])))));
		        }
		    }
            catch (Exception)
            {
            }
            finally
            {

            }
		    return xStructure;
		}
	}
}
