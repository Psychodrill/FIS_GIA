using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Web;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Web.UI;

namespace Esrp.Web.Extentions
{
    public static class Helper
    {
        /// <summary>
        /// Удаление из строки пробелов по краям; конвертация многих пробелов к одному, недопущение обрамляющих пробелов у дефисов
        /// Например: "    Абдуллина - Билялетдинов    " -> "Абдуллина-Билялетдинов"
        /// Например: "    12  -12345678 -34    " -> "12-12345678-34"
        /// См. также: StringUnitTest.cs
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FullTrim(this string input)
        {
            return Regex.Replace(Regex.Replace(input.Trim(), "\\s+", " "), " ?- ?", "-");
        }

        public static RSACryptoServiceProvider GetCurrentRsaProvider(bool encrypt=false)
        {
            
            CspParameters CSPParam = new CspParameters();
            CSPParam.Flags = CspProviderFlags.UseMachineKeyStore;
            CSPParam.KeyContainerName = "Esrp Service Keys";

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(CSPParam) { PersistKeyInCsp = true };
           
            return rsa;
        }

        public static T FindControl<T>(this Control parent, string controlId) where T : Control
        {
            T found = default(T);
            
            found = parent.FindControl(controlId) as T;
            if (found == null)
            {
                foreach (Control c in parent.Controls)
                {
                    found = c.FindControl<T>(controlId);
                    if (found != null)
                        break;
                }
            }
            return found;

        }
    }
}
