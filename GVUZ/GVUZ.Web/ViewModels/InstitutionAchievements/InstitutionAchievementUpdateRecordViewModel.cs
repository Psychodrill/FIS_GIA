using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GVUZ.Web.ViewModels.InstitutionAchievements
{
    public class InstitutionAchievementUpdateRecordViewModel
    {
        public const string FieldRequiredMessage = "Поле \"{0}\" должно быть заполнено";

        [Required]
        public int Id { get; set; }

        //[Required(ErrorMessage = FieldRequiredMessage)]
        [DisplayName("Идентификатор (UID)")]
        public string UID { get; set; }

        [Required(ErrorMessage = FieldRequiredMessage)]
        [DisplayName("Наименование")]
        public string Name { get; set; }

        [Required(ErrorMessage = FieldRequiredMessage)]
        [DisplayName("Категория")]
        public int? CategoryId { get; set; }

        [Required(ErrorMessage = FieldRequiredMessage)]
        [DisplayName("Приемная кампания")]
        public int? CampaignId { get; set; }

        [Required(ErrorMessage = FieldRequiredMessage)]
        [DisplayName("Макс. балл")]
        [MarkValidation(ErrorMessage = "Значения в поле макс. балл должно содержать не более 3 знаков в целой части (с учетом округления дробной до 4 знаков)")]
        public decimal? MaxValue { get; set; }
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