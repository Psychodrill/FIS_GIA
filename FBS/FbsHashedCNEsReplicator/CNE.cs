using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace FbsHashedCNEsReplicator
{
    public class CNE
    {
        ValidationResult ValidationSummary_;
        public ValidationResult ValidationSummary
        {
            get { return ValidationSummary_; }
            set { ValidationSummary_ = value; }
        }

        long  Id_;
        public long Id
        {
            get { return Id_; }
            set { Id_ = value; }
        }

        string Number_;
        public string Number
        {
            get { return Number_; }
            set { Number_ = value; }
        }

        string EducationInstitutionCode_;
        public string EducationInstitutionCode
        {
            get { return EducationInstitutionCode_; }
            set { EducationInstitutionCode_ = value; }
        }

        int Year_;
        public int Year
        {
            get { return Year_; }
            set { Year_ = value; }
        }

        string LastName_;
        public string LastName
        {
            get { return LastName_; }
            set { LastName_ = value; }
        }

        string FirstName_;
        public string FirstName
        {
            get { return FirstName_; }
            set { FirstName_ = value; }
        }

        string PatronymicName_;
        public string PatronymicName
        {
            get { return PatronymicName_; }
            set { PatronymicName_ = value; }
        }

        string PassportSeria_;
        public string PassportSeria
        {
            get { return PassportSeria_; }
            set { PassportSeria_ = value; }
        }

        string PassportNumber_;
        public string PassportNumber
        {
            get { return PassportNumber_; }
            set { PassportNumber_ = value; }
        }

        bool  Sex_;
        public bool Sex
        {
            get { return Sex_; }
            set { Sex_ = value; }
        }

        string Class_;
        public string Class
        {
            get { return Class_; }
            set { Class_ = value; }
        }

        string EntrantNumber_;
        public string EntrantNumber
        {
            get { return EntrantNumber_; }
            set { EntrantNumber_ = value; }
        }

        int RegionId_;
        public int RegionId
        {
            get { return RegionId_; }
            set { RegionId_ = value; }
        }


        string TypographicNumber_;
        public string TypographicNumber
        {
            get { return TypographicNumber_; }
            set { TypographicNumber_ = value; }
        }

        List<CNESubjectMark> SubjectMarks_ = new List<CNESubjectMark>();
        public List<CNESubjectMark> SubjectMarks
        {
            get { return SubjectMarks_; }
        }

        public CNE()
        {
            ValidationSummary_ = new ValidationResult("", ValidationResultType.Valid);
        }

        public CNE(string validationMessage, ValidationResultType validationResultType)
        {
            ValidationSummary_ = new ValidationResult(validationMessage, validationResultType);
        }
    }
}
