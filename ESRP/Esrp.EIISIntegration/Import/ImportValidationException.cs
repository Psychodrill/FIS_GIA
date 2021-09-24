
namespace Esrp.EIISIntegration.Import
{
    internal class ImportValidationException : ImportException
    {
        public ImportValidationException(string message)
            : base(message)
        { }

        public ImportValidationException(string eIISObjectCode, string message)
            : base(eIISObjectCode, message)
        { }
    }
}
