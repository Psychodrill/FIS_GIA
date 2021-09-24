using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FbsHashedCNEsReplicator
{
    public class  ValidationResult
    {
        string Message_;
        public string Message
        {
            get { return Message_; }
        }
        ValidationResultType ResultType_;
        public ValidationResultType ResultType
        {
            get { return ResultType_; }
        }

        public ValidationResult(string message, ValidationResultType resultType)
        {
            Message_ = message;
            ResultType_ = resultType;
        }
    }
    public enum ValidationResultType
    {
        Valid, Incomplete, NotValid,WrongFormat
    }
}
