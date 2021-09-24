using System.Security.Cryptography;
using System.Text;

namespace GVUZ.Helper
{
    public static class RandomHelper
    {
        /// <summary>
        ///     Возвращает 6 случайных цифр.
        /// </summary>
        public static string GetSixRandomDigits()
        {
            const int halfLength = 3;
            var random = new byte[halfLength];
            RandomNumberGenerator.Create().GetBytes(random);
            var builder = new StringBuilder(halfLength*2);
            for (int i = 0; i < halfLength; i++)
                builder.Append((random[i]%100)/10).Append(random[i]%10);

            return builder.ToString();
        }
    }
}