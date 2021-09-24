using System;
using System.Collections.Generic;

namespace GVUZ.Helper.ExternalValidation
{
    public class PassportValidator
    {
        /// <summary>
        ///     Возвращает пустой список, если проверка паспортных данных прошла успешно или строки с ошибками.
        /// </summary>
        public IEnumerable<string> Validate(string documentSeries, string documentNumber, DateTime? documentDate,
                                            string subdivisionCode, bool isSeriesRequired)
        {
            if (string.IsNullOrEmpty(documentSeries) && isSeriesRequired)
                return new[] {Messages.PassportValidator_NoSeries};
            if (string.IsNullOrEmpty(documentNumber))
                return new[] {Messages.PassportValidator_NoNumber};
            if (!documentDate.HasValue)
                return new[] {Messages.PassportValidator_NoDate};

            // TODO: implement passport validator (задачу GVUZ-657 отложили, возможно не будем делать)
            return new string[0];
        }
    }
}