using FogSoft.Helpers;

namespace GVUZ.Model.Helpers
{
	public class InstitutionLogger
	{
		private readonly int _institutionID;
		private readonly string _institutionInfo = "Институт ID: {0}. ";

		public InstitutionLogger(int institutionID)
		{
			_institutionID = institutionID;
			_institutionInfo = _institutionInfo.FormatWith(_institutionID);
		}

		public void ErrorFormat(string format, params object[] args)
		{
			LogHelper.Log.ErrorFormat(_institutionInfo + format, args);
		}

		public void DebugFormat(string format, params object[] args)
		{
			LogHelper.Log.DebugFormat(_institutionInfo + format, args);
		}

		public bool IsNullError(object obj, string format, params object[] args)
		{
			if (obj == null)
			{
				LogHelper.Log.ErrorFormat(format, args);
				return true;
			}

			return false;
		}
	}
}
