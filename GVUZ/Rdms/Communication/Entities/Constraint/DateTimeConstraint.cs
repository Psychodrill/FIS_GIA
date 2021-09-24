using System;

namespace Rdms.Communication.Entities.Constraint
{
	public class DateTimeConstraint : IConstraint
	{
		public DateTime? Min { get; set; }
		public DateTime? Max { get; set; }

		public override string ToString()
		{
			if (!Min.HasValue && !Max.HasValue)
				return "";
			else
				return string.Format("{0};{1}",
				                     Min.HasValue ? Min.Value.ToString("dd.MM.yyyy") : "-\u221E",
				                     Max.HasValue ? Max.Value.ToString("dd.MM.yyyy") : "\u221E");
		}

		public void ParseString(string s)
		{
			if (s != "")
			{
				var parts = s.Split(';');
				if (parts[0] != "-\u221E")
					Min = DateTime.ParseExact(parts[0], "dd.MM.yyyy", null);
				if (parts[1] != "\u221E")
					Max = DateTime.ParseExact(parts[1], "dd.MM.yyyy", null);
			}
		}

		public bool CheckValue(object value)
		{
			DateTime val = (DateTime) value;
			if (Min.HasValue && val < Min.Value) return false;
			if (Max.HasValue && val > Max.Value) return false;
			return true;
		}
	}
}