namespace Ege.Check.Logic.Services.Dtos.Metadata
{
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class BulkMergeProcedureAttribute : Attribute
    {
        public readonly string ProcedureName;

        public BulkMergeProcedureAttribute(string procedureName)
        {
            if (string.IsNullOrEmpty(procedureName))
            {
                throw new ArgumentNullException("procedureName", "Procedure name can not be null or empty");
            }
            ProcedureName = procedureName;
        }
    }
}