using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FogSoft.Web.Mvc
{
	public class MandatoryAttribute : ValidationAttribute, IClientValidatable
	{
		public override bool IsValid(object value)
		{
			return ((bool) value);
		}

		public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata,
		                                                                       ControllerContext context)
		{
			ModelClientValidationRule rule = new ModelClientValidationRule();
			rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
			rule.ValidationType = "mandatory";
			yield return rule;
		}
	}
}
