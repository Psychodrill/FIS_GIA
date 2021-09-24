using System;
using System.ComponentModel;
using System.Xml.Serialization;
using FogSoft.Helpers;

namespace GVUZ.ServiceModel.Import.WebService.Dto
{
    /// <summary>
    ///     Рассматриваемое заявление абитуриента
    /// </summary>
    [Description("Рассматриваемое заявление абитуриента")]
    public class ConsideredApplicationDto : BaseDto, IEquatable<ConsideredApplicationDto>
    {
        /// <summary>
        ///     Заявление
        /// </summary>
        public ApplicationShortRef Application { get; set; }

        /// <summary>
        ///     ИД направления подготовки (Справочник 10 "Направления подготовки")
        /// </summary>
        public int DirectionID { get; set; }

        /// <summary>
        ///     ИД Формы обучения(Справочник 14 "Форма обучения")
        /// </summary>
        public int EducationFormID { get; set; }

        /// <summary>
        ///     ИД источника финансирования (Справочник 15 "Источник финансирования")
        /// </summary>
        public int FinanceSourceID { get; set; }

        /// <summary>
        ///     ИД Уровня образования(Справочник 2 "Уровень образования")
        /// </summary>
        public int EducationLevelID { get; set; }

        public override string ToString()
        {
            return XmlSerializerHelper.SerializeToString(this, new XmlSerializer(GetType()));
        }

        #region IEquatable<ConsideredApplicationDto> Members

        public bool Equals(ConsideredApplicationDto other)
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
            return Equals(obj as ConsideredApplicationDto);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = 17;
                result = result*37 + Application.ApplicationNumber.GetHashCode();
                result = result*37 + Application.RegistrationDateDate.GetHashCode();
                result = result*37 + DirectionID.GetHashCode();
                result = result*37 + EducationLevelID.GetHashCode();
                result = result*37 + EducationFormID.GetHashCode();
                result = result*37 + FinanceSourceID.GetHashCode();
                return result;
            }
        }

        #endregion
    }
}