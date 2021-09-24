using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Esrp.Utility
{
	public static class WebPageExtensions
	{
		public static void AddError(this Page page, string message)
		{
			CustomValidator customValidator = new CustomValidator();
			customValidator.IsValid = false;
			customValidator.ErrorMessage = message;
			page.Validators.Add(customValidator);
		}
	}
}
