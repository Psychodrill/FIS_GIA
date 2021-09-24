using System.Drawing;
using System.Web.UI.WebControls;

namespace Esrp.Web
{
	public static class ControlExtensions
	{
		public static void ReadOnly(this TextBox textBox, bool readOnly)
		{
			textBox.ReadOnly = readOnly;
			textBox.BackColor = readOnly ? Color.LightGray : Color.White;			
		}		
	}
}