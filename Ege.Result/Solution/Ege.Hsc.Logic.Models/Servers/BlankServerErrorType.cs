namespace Ege.Hsc.Logic.Models.Servers
{
    using System.ComponentModel;

    public enum BlankServerErrorType
    {
        [Description("Бланк отсутствует на сервере РЦОИ")]
        MissingOnServer,
        [Description("Бланк отсутствует в БД")]
        MissingInDb,
    }
}
