using GVUZ.ImportService2016.Core.Dto.DataReaders.AdmissionInfo;
using GVUZ.ImportService2016.Core.Dto.DataReaders.InstitutionPrograms;
using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.ImportService2016.Core.Main.Repositories;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GVUZ.ImportService2016.Core.Main.Import
{
    class InstitutionProgramsImporter : BaseImporter
    {
        public InstitutionProgramsImporter(Dto.Import.PackageData packageData, VocabularyStorage vocabularyStorage, ImportConflictStorage importConflictStorage, bool deleteBulk) : base(packageData, vocabularyStorage, importConflictStorage, deleteBulk) { }

        protected override void Validate()
        {
            if (packageData.InstitutionPrograms == null) return;

            foreach (var program in packageData.InstitutionPrograms)
            {

                // UID должен быть уникальный в пакете
                if (packageData.InstitutionPrograms != null && packageData.InstitutionPrograms.Any(t => t.UID == program.UID && t.GUID != program.GUID))
                    conflictStorage.SetObjectIsBroken(program, ConflictMessages.UIDMustBeUniqueInCollection);

                //Проверка на дубли по Name + CODE
                //в пакете
                if (packageData.InstitutionPrograms.Any(t => t.Name == program.Name && t.Code == program.Code && t.UID != program.UID))
                {
                    conflictStorage.SetObjectIsBroken(program, ConflictMessages.InstitutionProgramSameNameAndCodeDifferentUID);
                }
                //и БД
                var sameProgram = vocabularyStorage.InstitutionProgramVoc.Items.Where(t => t.Name == program.Name && t.Code == program.Code && t.UID != program.UID);
                if (sameProgram.Count() > 0)
                {
                    conflictStorage.SetObjectIsBroken(program, ConflictMessages.InstitutionProgramSameNameAndCodeDifferentUID);
                }
            }
        }

        protected override List<string> ImportDb()
        {
            List<Tuple<string, IDataReader>> bulks = new List<Tuple<string, IDataReader>>();

            #region InstitutionPrograms
            bulks.Add(new Tuple<string, IDataReader>("bulk_InstitutionProgram", new BulkInstitutionProgramReader(packageData)));
            #endregion InstitutionPrograms

            // Должен прийти список импортированных ОП - пополнить справочники и записать количество в successfulImportStatisticsDto 
            var res = ADOPackageRepository.BulkInsertData(packageData, bulks, "ImportInstitutionPrograms", deleteBulk);
            DataSet dsResult = res.Item1;
            if (dsResult != null && dsResult.Tables.Count > 0)
            {
                conflictStorage.successfulImportStatisticsDto.InstitutionPrograms = dsResult.Tables[0].Rows.Count.ToString();

                vocabularyStorage.InstitutionProgramVoc.AddItems(dsResult.Tables[0]);
            }
            return res.Item2;
        }

    }
}
