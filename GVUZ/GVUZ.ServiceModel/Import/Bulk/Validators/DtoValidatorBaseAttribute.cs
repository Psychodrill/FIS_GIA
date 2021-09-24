using System;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;

namespace GVUZ.ServiceModel.Import.Bulk.Validators
{
    public abstract class DtoValidatorBaseAttribute : Attribute
    {
        public abstract int ErrorCode { get; }

        public virtual string ErrorMessage
        {
            get { return ConflictMessages.GetMessage(ErrorCode); }
        }

        public abstract void Validate(object dto, object obj, object[] datasource);
    }
}