using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fbs.Core
{
    /// <summary>
    /// Администратор сайта
    /// </summary>
    public class AdministratorAccount : IntrantAccount 
    {
        static public AdministratorAccount GetAdministratorAccount(string login)
        {
            return IntrantAccount.GetIntrantAccount(login) as AdministratorAccount;
        }
    }
}
