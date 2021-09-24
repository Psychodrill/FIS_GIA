using System;
using System.Collections.Generic;
using System.Linq;
using Esrp.EIISIntegration.Import;
using Esrp.EIISIntegration.Import.Importers;
using Esrp.Integration.Common;

namespace Esrp.EIISIntegration
{
    internal class EIIS
    {
        private EIISClientFactory clientFactory;
        private string connectionString_;

        private EIIS() { }
        public EIIS(string serviceUrl, string login, string password, string connectionString)
        {
            clientFactory = new EIISClientFactory(serviceUrl, login, password);
            connectionString_ = connectionString;
        }

        private List<IImporter> registeredImporters_ = new List<IImporter>();
        private void RegisterImporters()
        {
            registeredImporters_.Clear();

            registeredImporters_.Add(new RegionsImporter());
            registeredImporters_.Add(new OrganizationStatusEIISMapImporter());
            registeredImporters_.Add(new OrganizationKindEIISMapImporter());
            registeredImporters_.Add(new EducationalLevelEIISMapImporter());
            registeredImporters_.Add(new EducationalDirectionTypesImporter());
            registeredImporters_.Add(new FounderTypesImporter());
            registeredImporters_.Add(new FoundersImporter());
            registeredImporters_.Add(new OrganizationsImporter());
            registeredImporters_.Add(new OrganizationFoundersImporter());
            registeredImporters_.Add(new OrganizationLimitationsImporter());
            registeredImporters_.Add(new EducationalDirectionGroupsImporter());
            registeredImporters_.Add(new EducationalDirectionsImporter());
            registeredImporters_.Add(new LicensesImporter());
            registeredImporters_.Add(new LicenseSupplementsImporter());
            registeredImporters_.Add(new AllowedEducationalDirectionsImporter());
            registeredImporters_.Add(new QualificationsImporter());
            registeredImporters_.Add(new AllowedEducationalDirectionQualificationsImporter());
        }

        private void InitImporters()
        {
            foreach (IImporter importer in registeredImporters_)
            {
                importer.Init(clientFactory.SessionId, clientFactory.Client, connectionString_);
                importer.OnMessage += new EventHandler<MessageEventArgs>(Importer_OnMessage);
            }
        }

        public void Importer_OnMessage(object sender, MessageEventArgs e)
        {
            RaiseMesssage(e.Message);
        }

        public static Dictionary<string, string> GetAllEIISObjects()
        {
            EIIS eiis = new EIIS();
            eiis.RegisterImporters();
            return eiis.registeredImporters_.ToDictionary(obj => obj.EIISObjectCode, obj => obj.Name);
        }

        private bool prepared_;
        private void Prepare()
        {
            if (prepared_)
                return;

            try
            {
                clientFactory.OpenSession();
            }
            catch (Exception ex)
            {
                RaiseCriticalError(ex);
                return;
            }
            RegisterImporters();
            InitImporters();

            prepared_ = true;
        }

        public void RunImport(IEnumerable<string> eiisObjectCodes)
        {
            Prepare();

            foreach (IImporter importer in registeredImporters_)
            {
                if (!eiisObjectCodes.Any(obj => obj == importer.EIISObjectCode))
                    continue;
                try
                {
                    RaiseImportStarted(importer.Name, importer.EIISObjectCode);
                    importer.ImportData();
                    RaiseImportComplete(importer.Name, importer.EIISObjectCode, importer.CreatedObjects, importer.UpdatedObjects, importer.DeletedObjects, importer.FailedObjects, importer.SkippedObjects, importer.TotalObjects);
                }
                catch (ImportValidationException ex)
                {
                    RaiseImportError(ex);
                    RaiseImportComplete(importer.Name, importer.EIISObjectCode, importer.CreatedObjects, importer.UpdatedObjects, importer.DeletedObjects, importer.FailedObjects, importer.SkippedObjects, importer.TotalObjects);
                }
                catch (ImportException ex)
                {
                    RaiseImportError(ex);
                }
                catch (Exception ex)
                {
                    RaiseCriticalError(ex);
                }
            }
        }

        private void RaiseImportError(Exception ex)
        {
            if (ImportError != null)
            {
                ImportError(this, new ExceptionEventArgs(ex));
            }
        }

        private void RaiseCriticalError(Exception ex)
        {
            if (CriticalError != null)
            {
                CriticalError(this, new ExceptionEventArgs(ex));
            }
        }

        private void RaiseMesssage(string message)
        {
            if (Message != null)
            {
                Message(this, new MessageEventArgs(message));
            }
        }

        private void RaiseImportStarted(string importerName, string importerCode)
        {
            if (ImportStart != null)
            {
                ImportStart(this, new ImportEventArgs(importerName, importerCode));
            }
        }

        private void RaiseImportComplete(string importerName, string importerCode, int created, int updated, int deleted, int failed, int skipped, int total)
        {  
            if (ImportComplete != null)
            {
                string data = String.Format("создано: {0}; обновлено: {1}; удалено: {2}; ошибок: {3}; пропущено: {4}; всего: {5}", created, updated, deleted, failed, skipped, total);
                ImportComplete(this, new ImportEventArgs(importerName, importerCode, data));
            }
        }

        public event EventHandler<ExceptionEventArgs> ImportError;
        public event EventHandler<ExceptionEventArgs> CriticalError;
        public event EventHandler<MessageEventArgs> Message;
        public event EventHandler<ImportEventArgs> ImportStart;
        public event EventHandler<ImportEventArgs> ImportComplete;
    }
}
