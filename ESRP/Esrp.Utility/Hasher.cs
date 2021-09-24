using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Esrp.Utility
{
    public class Hasher
    {
        private static readonly SHA1Managed HashProvider = new SHA1Managed();

        /// <summary>
        /// Хэширование пароля с солью. Соль обычно - это UserId или UserName.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string Hash(string str)
        {
            byte[] StrBytes =Encoding.UTF8.GetBytes(str.ToUpper());
            byte[] StrHash = HashProvider.ComputeHash(StrBytes);
            return Convert.ToBase64String(StrHash, 0, StrHash.Length).Replace("=", "");
        }
    }
}
