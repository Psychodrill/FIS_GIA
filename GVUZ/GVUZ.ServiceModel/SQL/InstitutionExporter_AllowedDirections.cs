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


		private XElement AllowedDirections() {
			var xAllowedDirections=new XElement("AllowedDirections");


			string sql=@"
SELECT AD.DirectionID, AD.AdmissionItemTypeID, D.Name
FROM AllowedDirections (NOLOCK)  AD
INNER JOIN Direction (NOLOCK)  D on AD.DirectionID=D.DirectionID
WHERE AD.InstitutionID=@InstitutionID
			";

			var com=getSqlCommand(sql);
			com.Parameters.Add(new SqlParameter("InstitutionID",SqlDbType.Int) { Value=this._institutionID });
			SqlDataReader r=null;
			try {
				// com.Connection.Open должен быть открыт перед загрузкой всего пакета
				r=com.ExecuteReader();
			    while (r.Read())
			    {
			        xAllowedDirections.Add(new XElement("DirectionID", Int2Str(r["DirectionID"])));
			        xAllowedDirections.Add(new XElement("AdmissionItemTypeID", Int2Str(r["AdmissionItemTypeID"])));
			        xAllowedDirections.Add(new XElement("Name", Str2Str(r["Name"])));
			    }

			} catch(Exception) {
			} finally {
				if(r!=null) { r.Close(); }
			}
			return xAllowedDirections;
		}

	}
}
