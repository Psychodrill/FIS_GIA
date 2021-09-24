using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace GVUZ.ServiceModel.SQL {
	public class InstitutionExporterException: Exception {

		public InstitutionExporterException(string message)
			: base(message) { 
		
		}
	}
}
