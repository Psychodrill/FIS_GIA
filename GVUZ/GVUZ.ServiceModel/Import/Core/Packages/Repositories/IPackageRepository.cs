namespace GVUZ.ServiceModel.Import.Core.Packages.Repositories
{
	/// <summary>
	/// Интерфейс хранилища паккетов
	/// </summary>
	public interface IPackageRepository
	{
		ImportPackage GetPackageAsLIFO();
		ImportPackage GetUnprocessedPackage(int[] excludedInstitutions);
        ImportPackage GetUncheckedPackage(int[] excludedInstitutions);
		ImportPackage GetPackage(int packageID);
		void SetPackageChecked(int packageID, string checkResult);
		void SavePackage(ImportPackage importPackage);
	    bool IsPackageProcessed(int packageId);
	}
}
