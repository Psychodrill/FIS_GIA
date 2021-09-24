using System;

namespace FogSoft.Helpers.Validators
{
    public static class OgrnValidator
    {
        // private const int RfRegionsCount = 85;
        private const int LengthForOrganization = 13;
        private const int LengthForIndividualEnterpreneur = 15;

        /// <summary>
        ///     Проверка ОГРН (для организаций или ИП).
        /// </summary>
        /// <param name="ogrn"> ОГРН (должен быть непустым, для пустых значений метод вернет false). </param>
        /// <param name="forOrganization"> Проверяется ли ОГРН для организации (С ГГ КК ХХХХХХХ Ч) или ИП - ОГРНИП (С ГГ КК ХХХХХХХXX Ч). </param>
        /// <param name="required">
        ///     Является ли <see cref="ogrn" /> обязательным (если да и передали пустой с учетом пробелов ИНН - метод вернет false).
        /// </param>
        /// <remarks>
        ///     Описание алгоритма: http://ru.wikipedia.org/wiki/ОГРН Для получения реальных ОГРН: http://www.valaam-info.ru/fns/index.php
        /// </remarks>
        public static bool IsValid(string ogrn, bool forOrganization = true, bool required = false)
        {
            int requiredLength = forOrganization ? LengthForOrganization : LengthForIndividualEnterpreneur;

            ogrn = (ogrn ?? string.Empty).Trim();

            if (ogrn.Length == 0) return !required;

            if (ogrn.Length != requiredLength)
                return false;

            // проверка разряда С
            // согласно http://base.consultant.ru/nbu/cgi/online.cgi?req=doc;base=NBU;n=82640;div=LAW;mb=NBU;opt=1;ts=DE381563EAB83A81E7A249C272190CA1
            // ОГРН можен начинаться с 1 и 5 
            // иные гос рег номера с 2, 6, 7, 8, 9
            // если не будет срабатывать валидатор, то добавить необходимые цифры
            if ((forOrganization && !"15".Contains(ogrn.Substring(0, 1))) ||
                (!forOrganization && ogrn.Substring(0, 1) != "3"))
                return false;

            uint tmp;
            // проверка разрядов ГГ
            if (!uint.TryParse(ogrn.Substring(1, 2), out tmp))
                return false;

            if (DateTime.Today.Year.ToString().Substring(2, 2).CompareTo(tmp.ToString()) > 0)
                return false;

            // проверка разрядов КК - отключена, поскольку изредка встречаются "невалидные" в этом плане, но используемые ОГРН
            /*if (!uint.TryParse(ogrn.Substring(3, 2), out tmp))
				return false;
			if (tmp > RfRegionsCount)
				return false;*/

            // проверка разрядов Х
            if (!uint.TryParse(ogrn.Substring(5, requiredLength - 6), out tmp))
                return false;

            // проверка контрольного разряда
            long number;
            if (!long.TryParse(ogrn.Substring(0, requiredLength - 1), out number))
                return false;

            long remainder = number%(requiredLength - 2);
            if ((remainder%10).ToString() != ogrn.Substring(requiredLength - 1))
                return false;

            return true;
        }
    }
}