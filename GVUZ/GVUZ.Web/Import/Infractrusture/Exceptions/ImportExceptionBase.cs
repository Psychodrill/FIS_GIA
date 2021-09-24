using System;
using FogSoft.Helpers;

namespace GVUZ.Web.Import.Infractrusture.Exceptions
{
    public abstract class ImportExceptionBase : ApplicationException, IImportException
    {
        protected ImportExceptionBase(string message) : base(message)
        {
        }

        public abstract ImportExceptionType ExceptionType { get; }

        public override string ToString()
        {
            return string.Format("{0}. {1}", ExceptionType.GetEnumDescription(), Message);
        }
    }
}