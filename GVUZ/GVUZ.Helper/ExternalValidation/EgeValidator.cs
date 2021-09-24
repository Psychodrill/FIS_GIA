using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services.Protocols;
using GVUZ.Helper.EgeChecks;
using Microsoft.Practices.ServiceLocation;

namespace GVUZ.Helper.ExternalValidation
{
    /// <summary>
    ///     Возвращает результаты ЕГЭ через сервис ФБС.
    /// </summary>
    /// <remarks>
    ///     Для проверок (Фамилия, Имя, Отчество, Номер св-ва, Серия паспорта, Номер паспорта):
    ///     "Евстафьева", "Люсия", "Николаевна", "21-234567008-10", "4619", "513316"
    ///     "Добина", "Елена", "Алексеевна", "21-234567007-10", "5751", "29581"
    ///     "Митрохина", "Галина", "Ксенофонтовна", "21-234567009-10", "8537", "156651"
    /// </remarks>
    public class EgeValidator
    {
        public const int ServiceTimeout = 100000;

        /// <summary>
        ///     Проверяет одно свидетельство о ЕГЭ. Используется в тестах.
        ///     Возвращает пустой список, если указанный результат соответствует возвращенному из ФБС или строки с ошибками.
        /// </summary>
        //[Obsolete]
        public IEnumerable<string> Validate(EgePacket packet, EgeResult expectedResult)
        {
            if (packet == null) throw new ArgumentNullException("packet");
            if (expectedResult == null) throw new ArgumentNullException("expectedResult");
            EgeResult result;
            using (var wsChecks = new WSChecks {Timeout = ServiceTimeout})
            {
                try
                {
                    result = EgeInformationProvider.GetResultWithAuthentication(wsChecks, packet);
                }
                catch (SoapException)
                {
                    return new[] {Messages.EgeValidator_FbsCallFailed};
                }
                if (result.Errors != null && result.Errors.Count > 0)
                    return result.Errors;
            }

            if (result.Certificates.Count == 0)
                return new[] {"Свидетельство о ЕГЭ не найдено в АИС ФБС."};

            bool isStatusError;
            return
                expectedResult.Certificates[0].Validate(result.Certificates[0], out isStatusError).Select(x => x.Error);
        }

        /// <summary>
        ///     Возвращает ответ от сервиса проверки ФБС.
        /// </summary>
        public EgeResultAndStatus GetEgeInformation(EgePacket packet)
        {
            return ServiceLocator.Current.GetInstance<IEgeInformationProvider>().GetEgeInformation(packet);
        }
    }
}