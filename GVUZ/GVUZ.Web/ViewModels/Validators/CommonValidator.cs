using System;
using System.ComponentModel.DataAnnotations;


namespace GVUZ.Web.ViewModels.Validators
{
	public static class CommonValidator
	{
		public static readonly DateTime MinSqlDateTime = new DateTime(1753, 1, 1);

		public static ValidationResult ValidateSqlDateTimeDiapason(DateTime valDate)
		{
			if (valDate >= MinSqlDateTime)
				return ValidationResult.Success;

			return new ValidationResult(Messages.DateRange);
		}
	}
}