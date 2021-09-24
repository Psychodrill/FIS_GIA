using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Esrp.EIISIntegration.Import;
using Esrp.EIISIntegration.Import.Importers;
using Esrp.Integration.Common;

namespace Esrp.EIISIntegration
{
    public class EIISEntryPoint
    {
        private static bool showExceptionDetails_;
        private static Logger logger_;
        public static void Run(bool showExceptionDetails)
        {
            logger_ = new Logger("Log_EIISImport");

            showExceptionDetails_ = showExceptionDetails;

            string serviceUrl = ConfigurationManager.AppSettings["EIISUrl"];
            string login = ConfigurationManager.AppSettings["EIISLogin"];
            string password = ConfigurationManager.AppSettings["EIISPassword"];

            string connectionString = null;
            if (ConfigurationManager.ConnectionStrings["ESRP_Public"] != null)
            {
                connectionString = ConfigurationManager.ConnectionStrings["ESRP_Public"].ConnectionString;
            }

            if ((String.IsNullOrEmpty(serviceUrl))
                || (String.IsNullOrEmpty(login))
                || (String.IsNullOrEmpty(password))
                || (String.IsNullOrEmpty(connectionString)))
            {
                logger_.WriteLine("Не указаны необходимые настройки");
                return;
            }

            string connectionError;
            if (!SqlConnectionChecker.CheckConnection(connectionString, out connectionError))
            {
                logger_.WriteLine(String.Format("Невозможно соединиться с БД ЕСРП: {0}", connectionError));
                return;
            }

            EIIS eiis = new EIIS(serviceUrl, login, password, connectionString);
            eiis.ImportError += new EventHandler<ExceptionEventArgs>(EIIS_ImportError);
            eiis.CriticalError += new EventHandler<ExceptionEventArgs>(EIIS_Error);
            eiis.Message += new EventHandler<MessageEventArgs>(EIIS_Message);
            eiis.ImportStart += new EventHandler<ImportEventArgs>(EIIS_ImportStart);
            eiis.ImportComplete += new EventHandler<Import.ImportEventArgs>(EIIS_ImportComplete);

            logger_.WriteLine("Импорт запущен");

            List<string> eiisCatalogsToImport = new List<string>() 
            {
                EIISObjectCodes.Regions,
                EIISObjectCodes.OrganizationStatuses, 
                EIISObjectCodes.OrganizationKinds,
                EIISObjectCodes.EducationalLevels,
                EIISObjectCodes.EducationalDirectionTypes,
                EIISObjectCodes.FounderTypes,
                EIISObjectCodes.Founders,
                EIISObjectCodes.Organizations,
                EIISObjectCodes.FoundersToOrganizationsLink,
                EIISObjectCodes.OrganizationLimitations,
                EIISObjectCodes.EducationalDirectionGroups,
                EIISObjectCodes.EducationalDirections,
                EIISObjectCodes.Licenses,
                EIISObjectCodes.LicenseSupplements,
                EIISObjectCodes.AllowedEducationalDirections,
                EIISObjectCodes.Qualification, 
                EIISObjectCodes.AllowedEducationalDirectionQualificationLink, 
            };

            List<string> eiisCodesToSkipList = null;
            string eiisCodesToSkip = ConfigurationManager.AppSettings["EIISSkipCodes"];
            if (!String.IsNullOrEmpty(eiisCodesToSkip))
            {
                eiisCodesToSkipList = eiisCodesToSkip.Split(',', ';').ToList();

                foreach (string codeToSkip in eiisCodesToSkipList)
                {
                    if (codeToSkip.Replace(" ", "") == String.Empty)
                        continue;

                    eiisCatalogsToImport.Remove(codeToSkip.Replace(" ", ""));
                }
            }

            eiis.RunImport(eiisCatalogsToImport);
            logger_.WriteLine("Импорт завершен");
        }

        private const bool ShowStackTraces = false;

        private static void EIIS_ImportStart(object sender, ImportEventArgs e)
        {
            logger_.WriteLine(String.Format("Импорт объекта {0} ({1}) начат", e.ImporterCode, e.ImporterName));
        }

        private static void EIIS_ImportComplete(object sender, ImportEventArgs e)
        {
            logger_.WriteLine(String.Format("Импорт объекта {0} ({1}) успешно завершен ({2})", e.ImporterCode, e.ImporterName, e.Data));
        }

        static void EIIS_Error(object sender, ExceptionEventArgs e)
        {
            if (showExceptionDetails_)
            {
                logger_.WriteLine(String.Format("При импорте произошла критическая ошибка: {0} ({1})", e.Exception.Message, e.Exception.StackTrace));
            }
            else
            {
                logger_.WriteLine(String.Format("При импорте произошла критическая ошибка: {0}", e.Exception.Message));
            }
        }

        private static void EIIS_ImportError(object sender, ExceptionEventArgs e)
        {
            if (showExceptionDetails_)
            {
                logger_.WriteLine(String.Format("При импорте произошла ошибка: {0} ({1})", e.Exception.Message, e.Exception.StackTrace));
            }
            else
            {
                logger_.WriteLine(String.Format("При импорте произошла ошибка: {0}", e.Exception.Message));
            }
        }

        private static void EIIS_Message(object sender, MessageEventArgs e)
        {
            logger_.WriteLine(e.Message);
        }
    }
}
