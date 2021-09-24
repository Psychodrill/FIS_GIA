namespace Rdms.Communication.Entities.Constraint
{
	public class IntConstraint : IConstraint
	{
		public int? Min { get; set; }
		public int? Max { get; set; }

		public override string ToString()
		{
			if (!Min.HasValue && !Max.HasValue)
				return "";
			else
				return string.Format("{0};{1}",
				                     Min.HasValue ? Min.Value.ToString() : "-\u221E",
				                     Max.HasValue ? Max.Value.ToString() : "\u221E");
		}

		public void ParseString(string s)
		{
			if (s != "")
			{
				var parts = s.Split(';');
				if (parts[0] != "-\u221E")
					Min = int.Parse(parts[0]);
				if (parts[1] != "\u221E")
					Max = int.Parse(parts[1]);
			}
		}

		public bool CheckValue(object value)
		{
			int val = (int) value;
			if (Min.HasValue && val < Min.Value) return false;
			if (Max.HasValue && val > Max.Value) return false;
			return true;
		}
	}
}