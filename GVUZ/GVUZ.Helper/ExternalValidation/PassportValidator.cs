using System;
using System.Collections.Generic;

namespace GVUZ.Helper.ExternalValidation
{
    public class PassportValidator
    {
        /// <summary>
        ///     ���������� ������ ������, ���� �������� ���������� ������ ������ ������� ��� ������ � ��������.
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

            // TODO: implement passport validator (������ GVUZ-657 ��������, �������� �� ����� ������)
            return new string[0];
        }
    }
}