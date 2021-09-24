namespace GVUZ.ServiceModel.Import.Package
{
    public class GetResultCheckApplication
    {
        public string PackageGUID;
        public string PackageID;

        public string GetCorrectPackageID()
        {
            return PackageGUID ?? PackageID;
        }
    }
}