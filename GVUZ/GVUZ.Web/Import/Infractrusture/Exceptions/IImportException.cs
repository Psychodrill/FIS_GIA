namespace GVUZ.Web.Import.Infractrusture.Exceptions
{
    public interface IImportException
    {
        ImportExceptionType ExceptionType { get; }
        string Message { get; }
    }
}