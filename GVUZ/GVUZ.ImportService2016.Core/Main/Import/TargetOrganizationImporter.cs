using GVUZ.ImportService2016.Core.Dto.DataReaders.AdmissionInfo;
using GVUZ.ImportService2016.Core.Dto.DataReaders.TargetOrganizations;
using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.ImportService2016.Core.Main.Extensions;
using GVUZ.ImportService2016.Core.Main.Repositories;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GVUZ.ImportService2016.Core.Main.Import
{
    class TargetOrganizationImporter : BaseImporter
    {
        public TargetOrganizationImporter(Dto.Import.PackageData packageData, VocabularyStorage vocabularyStorage, ImportConflictStorage importConflictStorage, bool deleteBulk) : base(packageData, vocabularyStorage, importConflictStorage, deleteBulk) { }

        protected override void Validate()
        {
            if (packageData.TargetOrganizations == null) return;

            // Проверка на дубли по Name
            //foreach (var group in packageData.TargetOrganizations.GroupBy(t => t.Name))
            //    if (group.Count() > 1)
            //    {
            //        foreach (var item in group)
            //        {
            //            conflictStorage.SetObjectIsBroken(item, ConflictMessages.NameMustBeUniqueInCollection);
            //        }
            //    }
            // Проверка на дубли по UID
            foreach (var group in packageData.TargetOrganizations.GroupBy(t => t.UID))
                if (group.Count() > 1)
                {
                    foreach (var item in group)
                    {
                        conflictStorage.SetObjectIsBroken(item, ConflictMessages.UIDMustBeUniqueInCollection);
                    }
                }

            //foreach(var to in packageData.TargetOrganizations)
            //{
            //    // Если среди CompetitiveGroupTargetOrganization есть с таким же Name, но отличным UID то конфликт!
            //    if (vocabularyStorage.CompetitiveGroupTargetVoc.Items.Any(t=> t.Name == to.Name && t.UID != to.UID))
            //    {
            //        conflictStorage.SetObjectIsBroken(to, ConflictMessages.TargetOrganizationSameNameDifferentUID);
            //    }

            //    if (to.INN != null)
            //    {
            //        if (CheckINN(to.INN) != int.Parse(to.INN.Substring(9)))
            //        {
            //            conflictStorage.SetObjectIsBroken(to, ConflictMessages.CrcINNIsBroken, to.INN);
            //        }
            //    }                
            //}

        }

        protected override List<string> ImportDb()
        {
            List<Tuple<string, IDataReader>> bulks = new List<Tuple<string, IDataReader>>();

            #region TargetOrganizations
            bulks.Add(new Tuple<string, IDataReader>("bulk_CompetitiveGroupTarget", new BulkTargetOrganizationReader(packageData)));
            #endregion TargetOrganizations

            // Должен прийти список импортированных кампаний - пополнить справочники и записать количество в successfulImportStatisticsDto 
            var res = ADOPackageRepository.BulkInsertData(packageData, bulks, "ImportTargetOrganizations", deleteBulk);
            DataSet dsResult = res.Item1;
            if (dsResult != null && dsResult.Tables.Count > 0)
            {
                conflictStorage.successfulImportStatisticsDto.TargetOrganizations = dsResult.Tables[0].Rows.Count.ToString();

                vocabularyStorage.CompetitiveGroupTargetVoc.AddItems(dsResult.Tables[0]);
            }
            return res.Item2;
        }

        protected int CheckINN(string INN)
        {
            int[] multipliers = { 2, 4, 10, 3, 5, 9, 4, 6, 8, 0 };

            int [] innn = INN.Select((c,i) => (c - 48)* multipliers[i++]).ToArray();

            int crc = innn.Sum();
            crc %= 11;
            return crc;

        }

    }
}
