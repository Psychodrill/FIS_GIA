namespace GVUZ.Web.Import.Infractrusture.Exceptions
{
    /// <summary>
    ///     Ошибка авторизации в импорте
    /// </summary>
    public class ImportXmlValidationException : ImportExceptionBase
    {
        public ImportXmlValidationException(string message)
            : base(message)
        {
        }

        public override ImportExceptionType ExceptionType
        {
            get { return ImportExceptionType.XmlValidation; }
        }
    }

    public class ImportXmlElementNotFoudException : ImportXmlValidationException
    {
        public ImportXmlElementNotFoudException(string element) : base(
            string.Format("Не найден тег {0}", element))
        {
        }
    }
}