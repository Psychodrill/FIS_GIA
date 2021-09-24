using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esrp.Web.Auth
{
	[Serializable]
	public class UserCheckResult
	{
		public int StatusID { get; set; }
		public string Login { get; set; }
	}
}