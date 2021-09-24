using System.Text.RegularExpressions;

namespace Rdms.Communication.Entities.Constraint
{
	public class TextConstraint : IConstraint
	{
		public string Pattern { get; set; }

		public override string ToString()
		{
			return Pattern;
		}

		public void ParseString(string s)
		{
			Pattern = s;
		}

		public bool CheckValue(object value)
		{
			if (!string.IsNullOrEmpty(Pattern))
			{
				string val = (string) value;
				return Regex.IsMatch(val, Pattern);
			}
			else
				return true;
		}
	}
}