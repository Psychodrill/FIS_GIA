using Microsoft.Practices.ServiceLocation;

namespace GVUZ.Model.Import.WebService
{
	public class WebServiceBaseImport
	{
		public static IImportServiceSettings ServiceSettings = ServiceLocator.Current.GetInstance<IImportServiceSettings>();
	}
}
