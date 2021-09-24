using System.ComponentModel;

namespace GVUZ.Web.Import.Infractrusture.Exceptions
{
    public enum ImportExceptionType
    {
        [Description("Ошибка авторизации")] Authorization,

        [Description("Ошибка валидации XML")] XmlValidation
    }
}