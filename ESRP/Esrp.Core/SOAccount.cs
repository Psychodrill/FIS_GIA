using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Esrp.Core
{
	public class SOAccount : IntrantAccount
	{
		static public SOAccount GetSupportAccount(string login)
		{
			return IntrantAccount.GetIntrantAccount(login) as SOAccount;
		}
	}
}
