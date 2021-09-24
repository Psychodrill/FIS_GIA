using System;
using FogSoft.Helpers;

namespace GVUZ.ServiceModel.Import.Core
{
    public class ImportException : Exception
    {
        public ImportException(string message) : base(message)
        {
            LogHelper.Log.Error(message);
        }

        public ImportException(string message, Exception innerException) : base(message, innerException)
        {
            LogHelper.Log.Error(message, innerException);
        }
    }
}