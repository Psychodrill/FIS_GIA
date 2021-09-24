using System.Xml.Serialization;
using GVUZ.ServiceModel.Import.WebService.Dto.Result;
using GVUZ.ServiceModel.Import.WebService.Dto.Result.Import;

namespace GVUZ.ServiceModel.Import.Package
{
    public class ImportResultPackage
    {
        public ConflictsResultDto Conflicts;
        public LogDto Log;
        public string PackageID;
    }

    public class DeleteResultPackage
    {
        public ConflictsResultDto Conflicts;
        public LogDto Log;
    }

    public class ImportPackageInfo
    {
        public string PackageID;
    }

    public class DeletePackageInfo
    {
        public string PackageID;
    }

    public class GetResultImportApplication
    {
        public string PackageGUID;
        public string PackageID;

        public string GetCorrectPackageID()
        {
            return PackageGUID ?? PackageID;
        }
    }

    public class GetResultDeleteApplication
    {
        public string PackageGUID;
        public string PackageID;

        public string GetCorrectPackageID()
        {
            return PackageGUID ?? PackageID;
        }
    }

    // валидация импортируемого пакета
    // http://support.microsoft.com/kb/307379
    public class ValidationResultPackage
    {
        public string Message;
        public string StatusCode;
    }

    /// <summary>
    ///     Результат импорта и проверки одного заявления
    /// </summary>
    public class AppSingleImportResult
    {
        public ConflictsResultDto Conflicts;
        [XmlArrayItem(ElementName = "EgeDocument")] public EgeDocumentCheckItemDto[] EgeDocuments;
        public SingleAppImportError Log;
    }

    public class SingleAppImportError
    {
        public string Error;
        public SuccessfulImportStatisticsDto Successful;
        public FailedImportInfoDto Failed;
    }
}