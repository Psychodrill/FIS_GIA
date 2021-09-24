using System.Text.RegularExpressions;

namespace FogSoft.Helpers.Validators
{
    public static class PhoneNumberValidator
    {
        /// <summary>
        ///     Проверяет правильность телефонного номера.
        /// </summary>
        public static bool IsValid(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber)) return false;
            return new Regex(@"^(8|\+7)(\(\d{3}\)|\d{3})[\d]{7}$").IsMatch(phoneNumber) ||
                   new Regex(@"^(8|\+7)(\(\d{4}\)|\d{4})[\d]{6}$").IsMatch(phoneNumber) ||
                   new Regex(@"^(8|\+7)(\(\d{5}\)|\d{5})[\d]{5}$").IsMatch(phoneNumber);
        }
    }
}