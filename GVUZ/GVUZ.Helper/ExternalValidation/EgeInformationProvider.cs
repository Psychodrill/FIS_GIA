using System;
using System.Net;
using System.Threading;
using System.Web.Services.Protocols;
using FogSoft.Helpers;
using GVUZ.Helper.EgeChecks;
using Microsoft.Practices.ServiceLocation;

namespace GVUZ.Helper.ExternalValidation
{
    public class EgeInformationProvider : IEgeInformationProvider
    {
        public const int ServiceTimeout = 100000;

        public static EgeResultAndStatus GetEgeInfo(EgePacket packet)
        {
            return ServiceLocator.Current.GetInstance<IEgeInformationProvider>().GetEgeInformation(packet);
        }

        public EgeResultAndStatus GetEgeInformation(EgePacket packet)
        {
            if (packet == null) throw new ArgumentNullException("packet");

            try
            {
                using (var wsChecks = new WSChecks {Timeout = ServiceTimeout})
                {
                    EgeResult result;
                    if (packet.Queries.Count == 1)
                    {
                        result = GetResultWithAuthentication(wsChecks, packet);
                    }
                    else
                    {
                        var loginPacket = new EgePacket
                            {
                                ClientUserName = packet.ClientUserName,
                                Login = packet.Login,
                                Password = packet.Password
                            };
                        result = GetResultWithAuthentication(wsChecks, loginPacket, packet);
                    }

                    return new EgeResultAndStatus(result, EgeResultAndStatus.Succeded);
                }
            }
            catch (SoapException ex)
            {
                LogHelper.Log.Error(ex.Message, ex);
                return new EgeResultAndStatus(EgeResult.CreateError(Messages.EgeValidator_FbsCallFailed),
                                              EgeResultAndStatus.TransferError);
            }
            catch (WebException ex)
            {
                LogHelper.Log.Error(ex.Message, ex);
                return new EgeResultAndStatus(EgeResult.CreateError(Messages.EgeValidator_FbsCallFailed),
                                              EgeResultAndStatus.TransferError);
            }
        }

        /// <summary>
        ///     Получает результат от сервиса ФБС по заданным параметрам.
        /// </summary>
        /// <param name="wsChecks">Прокси для сервиса ФБС.</param>
        /// <param name="queryPacket">Запрос свидетельства, в случае единичной проверки и аутентификационный запрос в случае пакетной проверки.</param>
        /// <param name="batchPacket">
        ///     <b>null</b>, в случае единичной проверки и пакет запросов в случае пакетной проверки.
        /// </param>
        internal static EgeResult GetResultWithAuthentication(WSChecks wsChecks, EgePacket queryPacket,
                                                              EgePacket batchPacket = null)
        {
            wsChecks.CookieContainer = new CookieContainer();
            string queryXml = queryPacket.ToString();
            //if (!wsChecks.Auhenticate(queryXml))
            //	return EgeResult.CreateError(Messages.EgeValidator_AccessDenied);
            wsChecks.UserCredentialsValue = new UserCredentials
                {
                    Login = queryPacket.Login,
                    Password = queryPacket.Password,
                    Client = queryPacket.ClientUserName
                };

            if (batchPacket == null)
                return EgeResult.Create(wsChecks.SingleCheck(queryXml));

            EgeResult result = EgeResult.Create(wsChecks.BatchCheck(batchPacket.ToString()));
            if (string.IsNullOrEmpty(result.BatchId) && result.Errors.Count == 0)
            {
                result.Errors.Add(Messages.EgeValidator_GetResultWithAuthentication_NoBatch);
                return result;
            }
            var packet = new EgePacket
                {
                    BatchId = result.BatchId,
                    ClientUserName = batchPacket.ClientUserName,
                    Login = batchPacket.Login,
                    Password = batchPacket.Password
                };

            int egeBatchDelay = AppSettings.Get("EgeBatchDelaySec", 10);
            int attempts = AppSettings.Get("EgeBatchAttempts", 5);

            string batchXml = packet.ToString();

            while (attempts >= 0)
            {
                //if (!wsChecks.Auhenticate(batchXml))
                //	return EgeResult.CreateError(Messages.EgeValidator_AccessDenied);
                wsChecks.UserCredentialsValue = new UserCredentials
                    {
                        Login = queryPacket.Login,
                        Password = queryPacket.Password,
                        Client = queryPacket.ClientUserName
                    };

                result = EgeResult.Create(wsChecks.GetBatchCheckResult(batchXml));
                if (result.StatusCode != "1")
                    return result;
                if (--attempts < 0)
                    break;
                Thread.Sleep(new TimeSpan(0, 0, egeBatchDelay));
            }

            result.Errors.Add("Количество попыток для пакетной проверки исчерпано, попробуйте повторить запрос позже.");
            return result;
        }
    }
}