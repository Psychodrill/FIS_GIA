using System;
using System.ComponentModel;

namespace GVUZ.ServiceModel.Import.WebService.Dto
{
    /// <summary>
    ///     Рекомендованное к зачислению заявление абитуриента
    /// </summary>
    [Description("Рекомендованное к зачислению заявление абитуриента")]
    public class RecommendedApplicationDto : ConsideredApplicationDto, IEquatable<RecommendedApplicationDto>
    {
        /// <summary>
        ///     Этап приемной кампании(обязательно для бакалавров и специалистов)
        /// </summary>
        public int? Stage { get; set; }

        #region IEquatable<ConsideredApplicationDto> Members

        public bool Equals(RecommendedApplicationDto other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            bool result = true;
            result &= other.Application.RegistrationDateDate == Application.RegistrationDateDate;
            result &= other.Application.ApplicationNumber.Equals(Application.ApplicationNumber);
            result &= other.DirectionID == DirectionID;
            result &= other.EducationLevelID == EducationLevelID;
            result &= other.EducationFormID == EducationFormID;
            result &= other.FinanceSourceID == FinanceSourceID;
            return result;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as RecommendedApplicationDto);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = 18;
                result = result*38 + Application.ApplicationNumber.GetHashCode();
                result = result*38 + Application.RegistrationDateDate.GetHashCode();
                result = result*38 + DirectionID.GetHashCode();
                result = result*38 + EducationLevelID.GetHashCode();
                result = result*38 + EducationFormID.GetHashCode();
                result = result*38 + FinanceSourceID.GetHashCode();
                return result;
            }
        }

        #endregion
    }
}