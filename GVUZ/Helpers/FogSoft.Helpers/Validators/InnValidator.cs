using System;
using System.Linq;

namespace FogSoft.Helpers.Validators
{
    public static class InnValidator
    {
        /// <summary>
        ///     Проверяет правильность ИНН.
        /// </summary>
        /// <param name="inn">
        ///     ИНН (если пустой, обрабатывается в соответствии с <see cref="required" />).
        /// </param>
        /// <param name="forOrganization">Проверяется ли ИНН для организации (для физ. лиц и ИП другой алгоритм).</param>
        /// <param name="required">
        ///     Является ли <see cref="inn" /> обязательным
        ///     (если да и передали пустой с учетом пробелов ИНН - метод вернет false).
        /// </param>
        /// <remarks>
        ///     Описание алгоритма: http://ru.wikipedia.org/wiki/ИНН
        ///     Для получения реальных ИНН: http://www.valaam-info.ru/fns/index.php
        /// </remarks>
        public static bool IsValid(string inn, bool forOrganization = true, bool required = false)
        {
            inn = (inn ?? string.Empty).Trim();

            if (inn.Length == 0) return !required;

            if ((forOrganization && inn.Length != 10) || (!forOrganization && inn.Length != 12)) return false;

            try
            {
                if (forOrganization)
                {
                    var factors = new byte[] {2, 4, 10, 3, 5, 9, 4, 6, 8};

                    if (inn.Length != factors.Length + 1) return false;

                    return IsValidFactors(inn, factors);
                }

                var factors1 = new byte[] {7, 2, 4, 10, 3, 5, 9, 4, 6, 8};
                var factors2 = new byte[] {3, 7, 2, 4, 10, 3, 5, 9, 4, 6, 8};

                if (inn.Length != Math.Max(factors1.Length, factors2.Length) + 1) return false;

                return IsValidFactors(inn, factors1) && IsValidFactors(inn, factors2);
            }
            catch
            {
                return false;
            }
        }

        private static bool IsValidFactors(string inn, byte[] factors)
        {
            int sum = factors.Select((f, i) => inn[i].ToString().To<int>()*f).Sum()%11%10;

            return sum == inn[factors.Length].ToString().To<int>();
        }
    }
}