
namespace Esrp.EIISIntegration.Import
{
    public class ErrorMessage
    {
        public const string RequiredFieldMessage = "отсутствует или не заполнено обязательное поле";
        public const string RelatedCatalogNotFoundMessage = "связанный объект (элемент справочника) не найден";
        public const string RelatedOrganizationNotFoundMessage = "связанный объект (организация) не найден";
        public const string RelatedHeadOrganizationNotFoundMessage = "связанный объект (головная организация) не найден";
        public const string RelatedFounderNotFoundMessage = "связанный объект (учредитель) не найден";
        public const string RelatedLicenseNotFoundMessage = "связанный объект (лицензия) не найден";
        public const string RelatedOldLicenseNotFoundMessage = "связанный объект (переоформленная лицензия) не найден";
        public const string RelatedLicenseSupplementNotFoundMessage = "связанный объект (приложение к лицензии) не найден";
        public const string RelatedOldLicenseSupplementNotFoundMessage = "связанный объект (переоформленное приложение к лицензии) не найден";
        public const string RelatedDirectionNotFoundMessage = "связанный объект (направление подготовки) не найден";
        public const string RelatedAllowedDirectionNotFoundMessage = "связанный объект (лицензированное направление подготовки) не найден";
        public const string RelatedQualificationNotFoundMessage = "связанный объект (квалификация) не найден";

        public const string ObjectSkippedMessage = "объект не подлежит импорту";

        public ErrorMessage(string type, string message)
        {
            Type = type;
            Message = message;
        }

        public string Type { get; set; }
        public string Message { get; set; }
    }
}
