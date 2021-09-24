namespace GVUZ.Web.Portlets
{
	public static class TriStateCheckBox
	{
		public static bool? GetValue(string checkboxTextValue)
		{
			//"checked" -> "unchecked" -> "unselected"
			bool? isChecked = null;
			isChecked = (checkboxTextValue == "checked"
			             	? true
			             	: (checkboxTextValue == "unchecked" ? false : isChecked));
			return isChecked;
		}
	}
}