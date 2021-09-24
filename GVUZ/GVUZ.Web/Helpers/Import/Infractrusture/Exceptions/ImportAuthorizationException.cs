namespace GVUZ.Web.Import.Infractrusture.Exceptions
{
    /// <summary>
    ///     Ошибка авторизации в импорте
    /// </summary>
    public class ImportAuthorizationException : ImportExceptionBase
    {
        public ImportAuthorizationException(string message) : base(message)
        {
        }

        public override ImportExceptionType ExceptionType
        {
            get { return ImportExceptionType.Authorization; }
        }
    }
}