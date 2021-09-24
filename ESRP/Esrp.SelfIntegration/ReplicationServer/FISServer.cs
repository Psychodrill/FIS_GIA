using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using Esrp.DB;

namespace Esrp.SelfIntegration.ReplicationServer
{
    public class FISServer
    {
        private string esrpConnectionString_;
        private string fisConnectionString_;
        private int rowCount_;
        public FISServer(string esrpConnectionString, string fisConnectionString, int rowCount)
        {
            if (rowCount == 0)
                throw new ArgumentException("rowCount");

            esrpConnectionString_ = esrpConnectionString;
            fisConnectionString_ = fisConnectionString;
            rowCount_ = rowCount;
        }

        private string[] synchronizableTables_ = 
        { 
            ESRPTables.EducationalDirectionType.TableName,
            ESRPTables.Organization2010.TableName, 
            ESRPTables.EducationalDirectionGroup.TableName,
            ESRPTables.EducationalDirection.TableName,
            ESRPTables.AllowedEducationalDirection.TableName,
            ESRPTables.License.TableName
        };

        public void RunSynchronization()
        {
            Prepare();
            lock (ServerLocker.Locker)
            {
                try
                {
                    using (EsrpRepository esrpRepository = EsrpRepository.Create())
                    using (FisRepository fisRepository = FisRepository.Create())
                    {
                        string synchronizableTablesStr = String.Format("'{0}'", String.Join("','", synchronizableTables_));
                        esrpRepository.DataContext.ExecuteCommand(String.Format("DELETE FROM ReplicationData WHERE ReplicationTableName NOT IN ({0})", synchronizableTablesStr));

                        RemainingCount = esrpRepository.DataContext.ReplicationDatas.Count();

                        IEnumerable<ReplicationData> records = esrpRepository.DataContext.ReplicationDatas
                            .OrderBy(x => x.Id)
                            .Take(rowCount_)
                            .ToArray();

                        Dictionary<string, HashSet<string>> dataToDelete = new Dictionary<string, HashSet<string>>();
                        Dictionary<string, HashSet<string>> dataToEnsure = new Dictionary<string, HashSet<string>>();

                        PopulateActionDictionaries(records, dataToEnsure, dataToDelete);

                        RunEnsureActions(fisRepository, esrpRepository, dataToEnsure);
                        RunDeleteActions(fisRepository.DataContext.Connection, dataToDelete);

                        fisRepository.DataContext.SubmitChanges();

                        esrpRepository.DataContext.ReplicationDatas.DeleteAllOnSubmit(records);
                        esrpRepository.DataContext.SubmitChanges();

                        RemainingCount = esrpRepository.DataContext.ReplicationDatas.Count();
                    }
                    Success = true;
                }
                catch (Exception ex)
                {
                    Success = false;
                    Exception = ex;
                }
            }
        }

        private void PopulateActionDictionaries(IEnumerable<ReplicationData> records, Dictionary<string, HashSet<string>> dataToEnsure, Dictionary<string, HashSet<string>> dataToDelete)
        {
            foreach (ReplicationData record in records)
            {
                if (record.ReplicationType == CommandTypes.Delete)
                {
                    if (!dataToDelete.ContainsKey(record.ReplicationTableName))
                    {
                        dataToDelete.Add(record.ReplicationTableName, new HashSet<string>());
                    }
                    if (!dataToDelete[record.ReplicationTableName].Contains(record.ReplicationRecordId))
                    {
                        dataToDelete[record.ReplicationTableName].Add(record.ReplicationRecordId);
                    }

                    if ((dataToEnsure.ContainsKey(record.ReplicationTableName))
                        && (dataToEnsure[record.ReplicationTableName].Contains(record.ReplicationRecordId)))
                    {
                        dataToEnsure[record.ReplicationTableName].Remove(record.ReplicationRecordId);
                    }
                }
                else if ((!dataToDelete.ContainsKey(record.ReplicationTableName))
                    || (!dataToDelete[record.ReplicationTableName].Contains(record.ReplicationRecordId)))
                {
                    if (!dataToEnsure.ContainsKey(record.ReplicationTableName))
                    {
                        dataToEnsure.Add(record.ReplicationTableName, new HashSet<string>());
                    }
                    if (!dataToEnsure[record.ReplicationTableName].Contains(record.ReplicationRecordId))
                    {
                        dataToEnsure[record.ReplicationTableName].Add(record.ReplicationRecordId);
                    }
                }
            }
        }

        private void RunEnsureActions(FisRepository fisRepository, EsrpRepository esrpRepository, Dictionary<string, HashSet<string>> dataToEnsure)
        {
            EnsureDirectionTypes(fisRepository, esrpRepository, dataToEnsure);
            EnsureInstitutions(fisRepository, esrpRepository, dataToEnsure);
            EnsureLicenses(fisRepository, esrpRepository, dataToEnsure);
            EnsureDirectionGroups(fisRepository, esrpRepository, dataToEnsure);
            EnsureDirections(fisRepository, esrpRepository, dataToEnsure);
            EnsureAllowedDirections(fisRepository, esrpRepository, dataToEnsure);
        }

        private void EnsureInstitutions(FisRepository fisRepository, EsrpRepository esrpRepository, Dictionary<string, HashSet<string>> dataToEnsure)
        {
            if (!dataToEnsure.ContainsKey(ESRPTables.Organization2010.TableName))
                return;

            HashSet<int> fisRegionIds = new HashSet<int>(fisRepository.DataContext.RegionTypes.Select(x => x.RegionId));

            foreach (string id in dataToEnsure[ESRPTables.Organization2010.TableName])
            {
                int esrpId = Int32.Parse(id);
                Organization2010 esrpObject = esrpRepository.DataContext.Organization2010s.FirstOrDefault(x => x.Id == esrpId);
                if (esrpObject == null)
                    continue;
                if (!ValidateInstitutionType(esrpObject.TypeId))
                    continue;

                Institution fisObject = fisRepository.DataContext.Institutions.FirstOrDefault(x => x.EsrpOrgID == esrpId);

                if (fisObject == null)
                {
                    fisObject = new Institution();
                    fisObject.CreatedDate = DateTime.Now;
                    fisObject.EsrpOrgID = esrpId;
                    fisRepository.DataContext.Institutions.InsertOnSubmit(fisObject);
                }
                fisObject.EIIS_ID = esrpObject.Eiis_Id;
                fisObject.InstitutionTypeID = (short)esrpObject.TypeId;
                fisObject.FullName = esrpObject.FullName;
                fisObject.BriefName = esrpObject.ShortName;
                fisObject.FormOfLawID = esrpObject.KindId;
                fisObject.RegionID = MapRegionId(fisRegionIds, esrpObject.RegionId);
                fisObject.IsPrivate = esrpObject.IsPrivate;
                fisObject.IsFilial = esrpObject.IsFilial;
                fisObject.LawAddress = esrpObject.LawAddress;
                fisObject.City = esrpObject.TownName;
                fisObject.Phone = esrpObject.Phone;
                fisObject.Fax = esrpObject.Fax;
                fisObject.INN = esrpObject.INN;
                fisObject.OGRN = esrpObject.OGRN;
                fisObject.KPP = esrpObject.KPP;
                fisObject.DateUpdated = DateTime.Now;
                fisObject.ModifiedDate = DateTime.Now;
                fisObject.OwnerDepartment = esrpObject.OwnerDepartment;
                fisObject.MainEsrpOrgId = esrpObject.MainId;
                fisObject.StatusId = esrpObject.StatusId;

                fisObject.FounderEsrpOrgId = esrpObject.DepartmentId;
            }
        }

        private void EnsureLicenses(FisRepository fisRepository, EsrpRepository esrpRepository, Dictionary<string, HashSet<string>> dataToEnsure)
        {
            if (!dataToEnsure.ContainsKey(ESRPTables.License.TableName))
                return;

            Dictionary<int, int> fisInstitutionIdsCache = fisRepository.DataContext.Institutions
               .Where(x => x.EsrpOrgID.HasValue)
               .ToDictionary((x) => x.EsrpOrgID.Value, (x) => x.InstitutionID);

            foreach (string id in dataToEnsure[ESRPTables.License.TableName])
            {
                int esrpId = Int32.Parse(id);
                License esrpObject = esrpRepository.DataContext.Licenses.FirstOrDefault(x => x.Id == esrpId);
                if (esrpObject == null)
                    continue;

                if (!fisInstitutionIdsCache.ContainsKey(esrpObject.Id))
                    continue;

                if (!esrpObject.OrderDocumentDate.HasValue)
                    continue;
                if (String.IsNullOrEmpty(esrpObject.OrderDocumentNumber))
                    continue;

                InstitutionLicense fisObject = fisRepository.DataContext.InstitutionLicenses.FirstOrDefault(x => x.Esrp_Id == esrpId);

                if (fisObject == null)
                {
                    fisObject = new InstitutionLicense();
                    fisObject.CreatedDate = DateTime.Now;
                    fisObject.Esrp_Id = esrpId;
                    fisRepository.DataContext.InstitutionLicenses.InsertOnSubmit(fisObject);
                }
                fisObject.EIIS_ID = esrpObject.Eiis_Id;
                fisObject.ModifiedDate = DateTime.Now;
                fisObject.LicenseDate = esrpObject.OrderDocumentDate.Value;
                fisObject.LicenseNumber = esrpObject.OrderDocumentNumber;
                fisObject.InstitutionID = fisInstitutionIdsCache[esrpObject.Id];
            }
        }

        private const int HigherEducationalOrganizationType = 1;
        private const int MediumEducationalOrganizationType = 2;
        private bool ValidateInstitutionType(int esrpTypeId)
        {
            if ((esrpTypeId != HigherEducationalOrganizationType) && (esrpTypeId != MediumEducationalOrganizationType))
                return false;
            return true;
        }

        private const int OtherRegionTypeId = 1001;
        private int MapRegionId(HashSet<int> fisRegionIds, int esrpRegionId)
        {
            if (fisRegionIds.Contains(esrpRegionId))
                return esrpRegionId;

            return OtherRegionTypeId;
        }

        private void EnsureDirections(FisRepository fisRepository, EsrpRepository esrpRepository, Dictionary<string, HashSet<string>> dataToEnsure)
        {
            if (!dataToEnsure.ContainsKey(ESRPTables.EducationalDirection.TableName))
                return;

            Dictionary<int, string> esrpEducationalDirectionTypesCache = esrpRepository.DataContext.EducationalDirectionTypes
                .Where(x => x.Eiis_Id != null)
                .ToDictionary((x) => x.Id, (x) => x.Eiis_Id);
            Dictionary<string, int> fisEducationalDirectionTypesCache = fisRepository.DataContext.EDU_PROGRAM_TYPEs
                .Where(x => x.EIIS_ID != null)
                .ToDictionary((x) => x.EIIS_ID, (x) => x.ID);

            Dictionary<int, string> esrpEducationalDirectionGroupsCache = esrpRepository.DataContext.EducationalDirectionGroups
                .Where(x => x.Eiis_Id != null)
                .ToDictionary((x) => x.Id, (x) => x.Eiis_Id);
            Dictionary<string, int> fisEducationalDirectionGroupsCache = fisRepository.DataContext.ParentDirections
                .Where(x => x.EIIS_ID != null)
                .ToDictionary((x) => x.EIIS_ID, (x) => x.ParentDirectionID);

            foreach (string id in dataToEnsure[ESRPTables.EducationalDirection.TableName])
            {
                int esrpId = Int32.Parse(id);
                EducationalDirection esrpObject = esrpRepository.DataContext.EducationalDirections.FirstOrDefault(x => x.Id == esrpId);
                if ((esrpObject == null) || (String.IsNullOrEmpty(esrpObject.Eiis_Id)))
                    continue;

                Direction fisObject = fisRepository.DataContext.Directions.FirstOrDefault(x => x.EIIS_ID == esrpObject.Eiis_Id);
                if (fisObject == null)
                {
                    fisObject = new Direction();
                    fisObject.CreatedDate = DateTime.Now;
                    fisObject.EIIS_ID = esrpObject.Eiis_Id;
                    fisRepository.DataContext.Directions.InsertOnSubmit(fisObject);
                }

                fisObject.Esrp_ID = esrpId;
                fisObject.ModifiedDate = DateTime.Now;
                fisObject.Code = esrpObject.Code;
                fisObject.Name = esrpObject.Name;
                //fisObject.QUALIFICATIONCODE = esrpObject.QualificationCode;
                //fisObject.QUALIFICATIONNAME = esrpObject.QualificationName;
                fisObject.PERIOD = esrpObject.Period;
                //FIS-1785
                //fisObject.UGSCODE = esrpObject.EducationalDirectionGroupCode;
                //fisObject.UGSNAME = esrpObject.EducationalDirectionGroupName;
                fisObject.EDU_DIRECTORY = esrpObject.DirectoryName;
                fisObject.NewCode = esrpObject.Code;
                if (esrpObject.MappedEducationalLevelId.HasValue)
                {
                    fisObject.EducationLevelId = (short)esrpObject.MappedEducationalLevelId.Value;
                }

                string educationalDirectionTypeEiisId = null;
                if (esrpObject.EducationalDirectionTypeId.HasValue)
                {
                    if (esrpEducationalDirectionTypesCache.ContainsKey(esrpObject.EducationalDirectionTypeId.Value))
                    {
                        educationalDirectionTypeEiisId = esrpEducationalDirectionTypesCache[esrpObject.EducationalDirectionTypeId.Value];
                    }
                }
                if (!String.IsNullOrEmpty(educationalDirectionTypeEiisId))
                {
                    if (fisEducationalDirectionTypesCache.ContainsKey(educationalDirectionTypeEiisId))
                    {
                        fisObject.EducationProgramTypeId = fisEducationalDirectionTypesCache[educationalDirectionTypeEiisId];
                    }
                }

                string educationalDirectionGroupEiisId = null;
                if (esrpObject.EducationalDirectionGroupId.HasValue)
                {
                    if (esrpEducationalDirectionGroupsCache.ContainsKey(esrpObject.EducationalDirectionGroupId.Value))
                    {
                        educationalDirectionGroupEiisId = esrpEducationalDirectionGroupsCache[esrpObject.EducationalDirectionGroupId.Value];
                    }
                }
                if (!String.IsNullOrEmpty(educationalDirectionGroupEiisId))
                {
                    if (fisEducationalDirectionGroupsCache.ContainsKey(educationalDirectionGroupEiisId))
                    {
                        fisObject.ParentID = fisEducationalDirectionGroupsCache[educationalDirectionGroupEiisId];
                    }
                }
            }
        }

        private void EnsureAllowedDirections(FisRepository fisRepository, EsrpRepository esrpRepository, Dictionary<string, HashSet<string>> dataToEnsure)
        {
            if (!dataToEnsure.ContainsKey(ESRPTables.AllowedEducationalDirection.TableName))
                return;

            Dictionary<int, EsrpLicenseSupplementItem> esrpLicenseSupplementsCache = esrpRepository.DataContext.LicenseSupplements
              .ToDictionary((x) => x.Id, (x) => new EsrpLicenseSupplementItem() { Id = x.Id, OrganizationId = x.OrganizationId, StatusName = x.StatusName });
            Dictionary<int, int> fisInstitutionsCache = fisRepository.DataContext.Institutions
                .Where(x => x.EsrpOrgID.HasValue)
                .Select(x => new { EsrpId = x.EsrpOrgID.Value, InstitutionID = x.InstitutionID })
                .GroupBy(x => x.EsrpId)
                .Select(x => x.First())
                .ToDictionary((x) => x.EsrpId, (x) => x.InstitutionID);

            Dictionary<int, string> esrpEducationalDirectionsCache = esrpRepository.DataContext.EducationalDirections
                .Where(x => x.Eiis_Id != null)
                .ToDictionary((x) => x.Id, (x) => x.Eiis_Id);
            Dictionary<string, int> fisEducationalDirectionsCache = fisRepository.DataContext.Directions
                .Where(x => x.EIIS_ID != null)
                .ToDictionary((x) => x.EIIS_ID, (x) => x.DirectionID);

            foreach (string id in dataToEnsure[ESRPTables.AllowedEducationalDirection.TableName])
            {
                int esrpId = Int32.Parse(id);
                AllowedEducationalDirection esrpObject = esrpRepository.DataContext.AllowedEducationalDirections.FirstOrDefault(x => x.Id == esrpId);
                if ((esrpObject == null) || (String.IsNullOrEmpty(esrpObject.Eiis_Id)))
                    continue;

                int? fisInstitutionId = null;
                EsrpLicenseSupplementItem esrpLicenseSupplement = null;
                if (esrpObject.LicenseSupplementId.HasValue)
                {
                    if (esrpLicenseSupplementsCache.ContainsKey(esrpObject.LicenseSupplementId.Value))
                    {
                        esrpLicenseSupplement = esrpLicenseSupplementsCache[esrpObject.LicenseSupplementId.Value];
                    }
                }
                if (esrpLicenseSupplement == null)
                    continue;

                if (!ValidateLicenseSupplementStatusName(esrpLicenseSupplement.StatusName))
                    continue;

                if (fisInstitutionsCache.ContainsKey(esrpLicenseSupplement.OrganizationId))
                {
                    fisInstitutionId = fisInstitutionsCache[esrpLicenseSupplement.OrganizationId];
                }
                if (!fisInstitutionId.HasValue)
                    continue;

                int? fisDirectionId = null;
                string educationalDirectionEiisId = null;
                if (esrpObject.EducationalDirectionId.HasValue)
                {
                    if (esrpEducationalDirectionsCache.ContainsKey(esrpObject.EducationalDirectionId.Value))
                    {
                        educationalDirectionEiisId = esrpEducationalDirectionsCache[esrpObject.EducationalDirectionId.Value];
                    }
                }
                if (!String.IsNullOrEmpty(educationalDirectionEiisId))
                {
                    if (fisEducationalDirectionsCache.ContainsKey(educationalDirectionEiisId))
                    {
                        fisDirectionId = fisEducationalDirectionsCache[educationalDirectionEiisId];
                    }
                }

                if (!fisDirectionId.HasValue)
                    continue;

                //FIS-1785
                //if (!ValidateAdmissionItemTypeId(esrpObject.MappedEducationalLevelId))
                //    continue;

                AllowedDirection fisObject = fisRepository.DataContext.AllowedDirections.FirstOrDefault(x => x.EIIS_ID == esrpObject.Eiis_Id);
                if (fisObject == null)
                {
                    fisObject = new AllowedDirection();
                    fisObject.CreatedDate = DateTime.Now;
                    fisObject.EIIS_ID = esrpObject.Eiis_Id;
                    fisObject.AllowedDirectionStatusID = 2;
                    fisRepository.DataContext.AllowedDirections.InsertOnSubmit(fisObject);
                }

                fisObject.Esrp_ID = esrpId;
                fisObject.ModifiedDate = DateTime.Now;
                //FIS-1785
                //fisObject.AdmissionItemTypeID = (short)esrpObject.MappedEducationalLevelId.Value;

                fisObject.InstitutionID = fisInstitutionId.Value;
                fisObject.DirectionID = fisDirectionId.Value;
            }
        }

        private string[] validStatusNames = 
        { 
            "Действует/ Выслано заявителю",
            "Действует",
            "Действует/ Подписано руководителем",
            "Действует/ Получено заявителем",
            "Действует/ Готово к выдаче" 
        };
        private bool ValidateLicenseSupplementStatusName(string esrpLicenseSupplementStatusName)
        {
            if (String.IsNullOrEmpty(esrpLicenseSupplementStatusName))
                return false;
            return validStatusNames.Contains(esrpLicenseSupplementStatusName);
        }

        //FIS-1785
        //private bool ValidateAdmissionItemTypeId(int? esrpAdmissionItemTypeId)
        //{
        //    if (!esrpAdmissionItemTypeId.HasValue)
        //        return false;

        //    if ((esrpAdmissionItemTypeId != 5)
        //        && (esrpAdmissionItemTypeId != 4)
        //        && (esrpAdmissionItemTypeId != 3)
        //        && (esrpAdmissionItemTypeId != 2)
        //        && (esrpAdmissionItemTypeId != 17)
        //        && (esrpAdmissionItemTypeId != 18)
        //        && (esrpAdmissionItemTypeId != 19))

        //        return false;
        //    return true;
        //}

        private void EnsureDirectionTypes(FisRepository fisRepository, EsrpRepository esrpRepository, Dictionary<string, HashSet<string>> dataToEnsure)
        {
            if (!dataToEnsure.ContainsKey(ESRPTables.EducationalDirectionType.TableName))
                return;

            foreach (string id in dataToEnsure[ESRPTables.EducationalDirectionType.TableName])
            {
                int esrpId = Int32.Parse(id);
                EducationalDirectionType esrpObject = esrpRepository.DataContext.EducationalDirectionTypes.FirstOrDefault(x => x.Id == esrpId);
                if ((esrpObject == null) || (String.IsNullOrEmpty(esrpObject.Eiis_Id)))
                    continue;

                EDU_PROGRAM_TYPE fisObject = fisRepository.DataContext.EDU_PROGRAM_TYPEs.FirstOrDefault(x => x.EIIS_ID == esrpObject.Eiis_Id);
                if (fisObject == null)
                {
                    fisObject = new EDU_PROGRAM_TYPE();
                    fisObject.EIIS_ID = esrpObject.Eiis_Id;
                    fisRepository.DataContext.EDU_PROGRAM_TYPEs.InsertOnSubmit(fisObject);
                }

                fisObject.Esrp_Id = esrpId;
                fisObject.NAME = esrpObject.Name;
                fisObject.SHORTNAME = esrpObject.ShortName;
            }
        }

        private void EnsureDirectionGroups(FisRepository fisRepository, EsrpRepository esrpRepository, Dictionary<string, HashSet<string>> dataToEnsure)
        {
            if (!dataToEnsure.ContainsKey(ESRPTables.EducationalDirectionGroup.TableName))
                return;

            foreach (string id in dataToEnsure[ESRPTables.EducationalDirectionGroup.TableName])
            {
                int esrpId = Int32.Parse(id);
                EducationalDirectionGroup esrpObject = esrpRepository.DataContext.EducationalDirectionGroups.FirstOrDefault(x => x.Id == esrpId);
                if ((esrpObject == null) || (String.IsNullOrEmpty(esrpObject.Eiis_Id)))
                    continue;

                ParentDirection fisObject = fisRepository.DataContext.ParentDirections.FirstOrDefault(x => x.EIIS_ID == esrpObject.Eiis_Id);
                if (fisObject == null)
                {
                    fisObject = new ParentDirection();
                    fisObject.CreatedDate = DateTime.Now;

                    fisObject.EIIS_ID = esrpObject.Eiis_Id;
                    fisRepository.DataContext.ParentDirections.InsertOnSubmit(fisObject);
                }

                fisObject.Esrp_Id = esrpId;
                fisObject.ModifiedDate = DateTime.Now;
                fisObject.Code = esrpObject.Code;
                fisObject.Name = esrpObject.Name;
            }
        }

        private void RunDeleteActions(IDbConnection fisConnection, Dictionary<string, HashSet<string>> dataToDelete)
        {
            DeleteRecords(ESRPTables.AllowedEducationalDirection.TableName, fisConnection, dataToDelete);
            DeleteRecords(ESRPTables.EducationalDirection.TableName, fisConnection, dataToDelete);
            DeleteRecords(ESRPTables.EducationalDirectionGroup.TableName, fisConnection, dataToDelete);
            DeleteRecords(ESRPTables.License.TableName, fisConnection, dataToDelete);
            DeleteRecords(ESRPTables.Organization2010.TableName, fisConnection, dataToDelete);
            DeleteRecords(ESRPTables.EducationalDirectionType.TableName, fisConnection, dataToDelete);
        }

        private void DeleteRecords(string esrpTableName, IDbConnection fisConnection, Dictionary<string, HashSet<string>> dataToDelete)
        {
            if (!dataToDelete.ContainsKey(esrpTableName))
                return;

            if (fisConnection.State != ConnectionState.Open)
            {
                fisConnection.Open();
            }

            string fisTableName = GetFisTableName(esrpTableName);
            string fisIdColumn = Hardcoded.GetFISIdColumnName(fisTableName);
            IDbCommand command = fisConnection.CreateCommand();
            command.CommandText = String.Format("DELETE FROM [{0}] WHERE [{1}] = @id", fisTableName, fisIdColumn);

            IDbDataParameter idParamter = command.CreateParameter();
            idParamter.ParameterName = "id";
            command.Parameters.Add(idParamter);

            foreach (string idToDelete in dataToDelete[esrpTableName])
            {
                idParamter.Value = idToDelete;
                command.ExecuteNonQuery();
            }
        }

        private string GetFisTableName(string esrpTableName)
        {
            switch (esrpTableName)
            {
                case ESRPTables.AllowedEducationalDirection.TableName:
                    return FISTables.AllowedDirections.TableName;
                case ESRPTables.EducationalDirection.TableName:
                    return FISTables.Direction.TableName;
                case ESRPTables.Organization2010.TableName:
                    return FISTables.Institution.TableName;
                case ESRPTables.EducationalDirectionGroup.TableName:
                    return FISTables.ParentDirection.TableName;
                case ESRPTables.EducationalDirectionType.TableName:
                    return FISTables.EDU_PROGRAM_TYPES.TableName;
                case ESRPTables.License.TableName:
                    return FISTables.InstitutionLicense.TableName;
                default:
                    return null;
            }
        }

        private bool prepared_;
        private void Prepare()
        {
            if (prepared_)
                return;

            EsrpRepository.Init(esrpConnectionString_);
            FisRepository.Init(fisConnectionString_);
            prepared_ = true;
        }

        public bool Success { get; private set; }
        public int RemainingCount { get; private set; }
        public Exception Exception { get; private set; }

        private class EsrpLicenseSupplementItem
        {
            public int Id { get; set; }
            public int OrganizationId { get; set; }
            public string StatusName { get; set; }
        }
    }
}
