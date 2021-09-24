using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;
	
namespace Esrp.Core
{
    /// <summary>
    /// Сотрудник горячей линии.
    /// </summary>
    public class SupportAccount : IntrantAccount 
    {
        static public SupportAccount GetSupportAccount(string login)
        {
            return IntrantAccount.GetIntrantAccount(login) as SupportAccount;
        }
    }
}
