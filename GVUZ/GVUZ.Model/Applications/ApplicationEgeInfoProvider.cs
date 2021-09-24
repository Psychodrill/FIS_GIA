using System;
using System.Linq;
using GVUZ.Helper.ExternalValidation;
using GVUZ.Model.Administration;

namespace GVUZ.Model.Applications
{
    internal static class ApplicationEgeInfoProvider
    {
        /// <summary>
        /// 	Возвращает XML, содержащий ответ от сервиса проверки ФБС.
        /// </summary>
        public static EgeResultAndStatus GetEgeInformation(EgeQuery query, string pinCode)
        {
            if (query == null)
                throw new ArgumentNullException(Messages.ApplicationValidator_GetEgePacket_NoQuery);

            //запрос на изменение возможности пользователя проверять свидетельства
            const string updateAndSelect =
                @"DECLARE @userName VARCHAR(255), @checkCount INT 
                    UPDATE UserPolicy SET @userName = UserName,	@checkCount = AvailableEgeCheckCount 
		                    = CASE WHEN AvailableEgeCheckCount > 0 THEN AvailableEgeCheckCount - 1 ELSE -1 END
	                    WHERE PinCode = {0}
                    SELECT @userName AS UserName, @checkCount + 1 AS AvailableEgeCheckCount";

            PinCodeInfo info;
            using (var entities = new AdministrationEntities())
            {
                info = entities.ExecuteStoreQuery<PinCodeInfo>(updateAndSelect, pinCode).FirstOrDefault();
            }

            // проверка на возможность пользователя проверять свидетельства
            if (info == null || !info.AvailableEgeCheckCount.HasValue || info.AvailableEgeCheckCount == 0
                || string.IsNullOrEmpty(info.UserName))
                return new EgeResultAndStatus(EgeResult.CreateError(Messages.ApplicationValidator_InvalidPin),
                                              EgeResultAndStatus.InvalidPin);

            if (string.IsNullOrEmpty(info.UserName))
                return new EgeResultAndStatus(EgeResult.CreateError(Messages.ApplicationValidator_NoClientUserName),
                                              EgeResultAndStatus.InvalidPin);
            EgePacket packet =EgePacketHelper. GetEgePacket(info.UserName, query);
            return EgeInformationProvider.GetEgeInfo(packet);
        }

        private class PinCodeInfo
        {
            public string UserName { get; set; }

            public int? AvailableEgeCheckCount { get; set; }
        }
    }
}
