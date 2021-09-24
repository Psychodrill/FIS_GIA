using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;

namespace Fbs.Core
{
    /// <summary>
    /// Сотрудник проверящей организации.
    /// </summary>
    public class AuditorAccount : IntrantAccount
    {
        static public AuditorAccount GetAuditorAccount(string login)
        {
            return (AuditorAccount)IntrantAccount.GetIntrantAccount(typeof(AuditorAccount), login);
        }
    }
}
