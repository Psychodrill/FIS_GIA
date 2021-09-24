using System;

namespace GVUZ.Model.Import.WebService
{
	public class WebServiceImportSettings : IImportServiceSettings
	{
		public string ImportPackageFolder
		{
			get { return "Import"; }
		} 

		public string ProcessingPackageFolder
		{
			get { return "Processing"; }
		}

		public string FinishedPackageFolder
		{
			get { return "Finished"; }
		}
	}

	public interface IImportServiceSettings
	{
		string ImportPackageFolder { get; }
		string ProcessingPackageFolder { get; }
		string FinishedPackageFolder { get; }		
	}
}
