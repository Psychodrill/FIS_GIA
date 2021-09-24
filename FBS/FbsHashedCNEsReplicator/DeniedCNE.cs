using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FbsHashedCNEsReplicator
{
   public  class DeniedCNE
    {
        ValidationResult ValidationSummary_;
        public ValidationResult ValidationSummary
        {
            get { return ValidationSummary_; }
            set { ValidationSummary_ = value; }
        }

        long Id_;
        public long Id
        {
            get { return Id_; }
            set { Id_ = value; }
        }

        string CertificateNumber_;
        public string CertificateNumber
        {
            get { return CertificateNumber_; }
            set { CertificateNumber_ = value; }
        }

        int Year_;
        public int Year
        {
            get { return Year_; }
            set { Year_ = value; }
        }

        string Comment_;
        public string Comment
        {
            get { return Comment_; }
            set { Comment_ = value; }
        }

        string NewCNENumber_;
        public string NewCNENumber
        {
            get { return NewCNENumber_; }
            set { NewCNENumber_ = value; }
        }

         public DeniedCNE()
        {
            ValidationSummary_ = new ValidationResult("", ValidationResultType.Valid);
        }

         public DeniedCNE(string validationMessage, ValidationResultType validationResultType)
        {
            ValidationSummary_ = new ValidationResult(validationMessage, validationResultType);
        }
    }
}
