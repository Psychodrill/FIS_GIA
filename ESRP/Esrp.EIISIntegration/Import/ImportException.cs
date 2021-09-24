using System;

namespace Esrp.EIISIntegration.Import
{
    internal class ImportException : Exception
    {
        public string EIISObjectCode { get; private set; }

        public ImportException(string message)
            : base(message)
        { }

        public ImportException(string eIISObjectCode, string message)
            : base(String.Format("Ошибка при загрузке из ЕИИС. Объект: {0}. Ошибка: {1}.", eIISObjectCode, message))
        {
            EIISObjectCode = eIISObjectCode;
        }
    }
}
