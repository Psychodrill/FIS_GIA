using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GVUZ.Web.ViewModels.TargetOrganizations
{
    public class TargetOrganizationUpdateRecordViewModel
    {
        public const string FieldRequiredMessage = "Поле \"{0}\" должно быть заполнено";
        public int CompetitiveGroupTargetID { get; set; }
        [Required(ErrorMessage = FieldRequiredMessage)]
        [DisplayName("Наименование органа власти или организации")]
        public string Name { get; set; }
        [DisplayName("Идентификатор (UID)")]
        public string UID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int InstitutionID { get; set; }

    }
    public sealed class MarkValidationAttribute : RegularExpressionAttribute
    {
        private const string ValidationPattern = @"^\d{0,3}((\.|\,)\d*)?$";
        public MarkValidationAttribute()
            : base(ValidationPattern)
        {
        }

        public override bool IsValid(object value)
        {
            decimal? d = value as decimal?;

            if (d == null)
            {
                return true;
            }

            if (d.GetValueOrDefault() == 0)
            {
                return true;
            }

            decimal v = Math.Round(d.GetValueOrDefault(), 4);
            return base.IsValid(v);
        }
    }
}
