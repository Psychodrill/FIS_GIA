using GVUZ.ImportService2016.Core.Main.Conflicts;
using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.ImportService2016.Core.Main.Log;
using GVUZ.ImportService2016.Core.Main.Repositories;
using GVUZ.ServiceModel.Import.Package;
using GVUZ.ServiceModel.Import.WebService.Dto.Result.Import;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Delete
{
    public abstract class BaseDeleter
    {

        protected GVUZ.ImportService2016.Core.Dto.Delete.DataForDelete dataForDelete;
        protected VocabularyStorage vocabularyStorage;
        protected DeleteConflictStorage conflictStorage;
        protected bool deleteBulk;

        public BaseDeleter(GVUZ.ImportService2016.Core.Dto.Delete.DataForDelete dataForDelete, VocabularyStorage vocabularyStorage, DeleteConflictStorage conflictStorage, bool deleteBulk)
        {
            this.dataForDelete = dataForDelete;
            this.vocabularyStorage = vocabularyStorage;
            this.conflictStorage = conflictStorage;
            this.deleteBulk = deleteBulk;
        }

        protected abstract void Validate();

        protected abstract void PrepareDataForDelete();

        protected List<Tuple<string, IDataReader>> bulks;
        protected virtual List<string> DeleteFromDB()
        {
            bulks = new List<Tuple<string, IDataReader>>();
            PrepareDataForDelete();

            var res = ADOPackageRepository.BulkDeleteData(dataForDelete, bulks, "DeleteServiceData", deleteBulk);
            DataSet dsResult = res.Item1;
            if (dsResult != null && dsResult.Tables.Count >= 8)
            {
                conflictStorage.removedCompetitiveGroupItemsCount += dsResult.Tables[0].Rows.Count;
                conflictStorage.removedCompetitiveGroupsCount += dsResult.Tables[1].Rows.Count;
                conflictStorage.removedCampaignsCount += dsResult.Tables[2].Rows.Count;
                conflictStorage.removedAppCount += dsResult.Tables[3].Rows.Count;
                conflictStorage.removedInstitutionAchievementsCount += dsResult.Tables[4].Rows.Count;
                conflictStorage.removedOrdersCount += dsResult.Tables[5].Rows.Count;
                conflictStorage.removedOrderApplicationsCount += dsResult.Tables[6].Rows.Count;
                conflictStorage.removedTargetOrganizationsCount += dsResult.Tables[7].Rows.Count;

                return res.Item2;
            }
            else
                throw new Exception("Хранимая процедура DeleteServiceData должна возвращать данные по всем основным удаляемым таблицам!");
        }

        public virtual List<string> DoDelete()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Validate();
            sw.Stop();
            Console.WriteLine(this.GetType().Name + ".Validate(): {0} сек", sw.Elapsed.TotalSeconds);
            LogHelper.Log.InfoFormat(this.GetType().Name + ".Validate(): {0} сек", sw.Elapsed.TotalSeconds);
            sw.Restart();
            var res = DeleteFromDB();
            sw.Stop();
            Console.WriteLine(this.GetType().Name + ".DeleteFromDB(): {0} сек", sw.Elapsed.TotalSeconds);
            LogHelper.Log.InfoFormat(this.GetType().Name + ".DeleteFromDB(): {0} сек", sw.Elapsed.TotalSeconds);
            return res;
        }
    }
}