using System;
using System.Configuration;
using System.Linq;
using System.Text;
using GVUZ.ServiceModel.Import.Package;
using GVUZ.ServiceModel.Import.Schemas;

namespace GVUZ.ServiceModel.Import.Core.Packages.Repositories
{
	/// <summary>
	/// Датабазное хранилище пакетов
	/// </summary>
	public class DbPackageRepository : IPackageRepository
	{
		/// <summary>
		/// Получение пакета по идентификатору
		/// </summary>
		public ImportPackage GetPackage(int packageID)
		{
            using (var dbContext = new ImportEntities())
            {
                var importPackage = dbContext.ImportPackage.Where(x => x.PackageID == packageID).ToList().FirstOrDefault();
                importPackage.PackageData = XmlPreparations.Prepare(importPackage.PackageData);
                return importPackage;
            }
		}

		/// <summary>
		/// Устанавливаем признак проверки пакета
		/// </summary>
		public void SetPackageChecked(int packageID, string checkData)
		{
			using (var dbContext = new ImportEntities())
			{
				var dbPackage = dbContext.ImportPackage.Single(x => x.PackageID == packageID);
				dbPackage.CheckStatusID = (int)PackageStatus.Processed;
				dbPackage.CheckResultInfo = checkData;
				dbPackage.LastDateChanged = DateTime.Now;
				dbContext.SaveChanges();
			}
		}

		/// <summary>
		/// Сохраняем пакет
		/// </summary>
		public void SavePackage(ImportPackage importPackage)
		{
		       if (importPackage.InstitutionID == 0)
                		return;
			DateTime curTime = DateTime.Now;
			if (importPackage.CreateDate == DateTime.MinValue)
				importPackage.CreateDate = curTime;
			if (importPackage.LastDateChanged == DateTime.MinValue)
				importPackage.LastDateChanged = curTime;

			using (var dbContext = new ImportEntities())
			{
				var existingPackage = dbContext.ImportPackage.FirstOrDefault(x => x.PackageID == importPackage.PackageID);
				if (existingPackage == null) //нет существующего добавляем
				{
                    importPackage.Content = GetContent(importPackage.TypeID, importPackage.PackageData);
					dbContext.ImportPackage.AddObject(importPackage);
				}
				else
				{
					var createdDate = existingPackage.CreateDate;
					existingPackage = AutoMapper.Mapper.Map(importPackage, existingPackage);
					
					if (createdDate != DateTime.MinValue)
						importPackage.CreateDate = existingPackage.CreateDate = createdDate;

                    if (string.IsNullOrEmpty(importPackage.Content))
                        importPackage.Content = GetContent(importPackage.TypeID, importPackage.PackageData);

					dbContext.ImportPackage.ApplyCurrentValues(existingPackage);
				}

				if (importPackage.StatusID == (int)PackageStatus.None)
					importPackage.StatusID = (int) PackageStatus.Processed;

				dbContext.SaveChanges();
			}
		}

        /// <summary>
        /// Текстовое представление содержимого пакета
        /// </summary>
        public static string GetContent(int typeID, string packageData)
        {
            if (typeID != 1 && typeID != 2) return "";

            var content = new StringBuilder();
            if (packageData.Contains("<Campaign>")) content.Append("Приемные кампании, ");
            if (packageData.Contains("<AdmissionInfo>")) content.Append("Сведения о приемной компании, ");
            if (packageData.Contains("<Applications>")) content.Append("Заявления абитуриентов, ");
            if (packageData.Contains("<OrdersOfAdmission>")) content.Append("Списки заявлений, включенных в приказ, ");
            if (packageData.Contains("<CompetitiveGroupItems>")) content.Append("Направления подготовки конкурсов, ");
            if (packageData.Contains("<EntranceTestResults>")) content.Append("Результаты вступительных испытаний, ");
            if (packageData.Contains("<ApplicationCommonBenefits>")) content.Append("Общие льготы, предоставляемые абитуриентам, ");

            return content.Length == 0 ? string.Empty : content.ToString(0, content.Length - 2);
        }

	    public bool IsPackageProcessed(int packageId)
	    {
	        using (var dbContext = new ImportEntities())
	        {
	            return dbContext.ImportPackage.Any(c => c.PackageID == packageId && c.StatusID == 3);
	        }
	    }

	    /// <summary>
		/// Получаем пакет в порядке очереди (используется только в тестовых целях)
		/// </summary>
		/// <returns></returns>
		public ImportPackage GetPackageAsLIFO()
		{
			using (var dbContext = new ImportEntities())
			{
				var importPackage = dbContext.ImportPackage
					.Where(x => x.StatusID == (int) PackageStatus.PlacedInQueue)
					.OrderByDescending(x => x.PackageID).FirstOrDefault();
				if (importPackage == null) return null;

                importPackage.PackageData = XmlPreparations.Prepare(importPackage.PackageData);

				importPackage.ChangeStatus(PackageStatus.Processing, dbContext);

				switch((PackageType)importPackage.TypeID)
				{
					case PackageType.Import:
						return importPackage;
					default:
						throw new ImportException("Wrong package type");
				}
			}
		}

		/// <summary>
		/// Получение непроверенного пакета
		/// </summary>
        public ImportPackage GetUncheckedPackage(int[] excludedInstitutions)
		{
		    if (ConfigurationManager.AppSettings["FBS_Disabled"] != null)
                return null;
		    
		    using (var dbContext = new ImportEntities())
			{
                //var packagesInWait = dbContext.ImportPackage.Any(c => c.TypeID == 1 && (c.StatusID == 1 || c.StatusID == 2));
                //if (packagesInWait) return null;

                // Roman.N.Bukin - есть очень медленно выполняется запрос, но есть индексирование по статусу
                //var importPackage = dbContext.ImportPackage.Where(x => (x.CheckStatusID == (int)PackageStatus.PlacedInQueue || x.CheckStatusID == null) 
                //        && x.TypeID == (int)PackageType.Import && x.StatusID == (int)PackageStatus.Processed
                //        && !excludedInstitutions.Contains(x.InstitutionID))
                //    .OrderBy(x => x.PackageID).FirstOrDefault();
                var importPackages = dbContext.ImportPackage.Where(x => x.CheckStatusID == (int)PackageStatus.PlacedInQueue && x.TypeID == (int)PackageType.Import).ToList();
                System.Diagnostics.Debug.WriteLine("importPackages.Count: " + importPackages.Count);
                var importPackage = importPackages.Where(x => x.StatusID == (int)PackageStatus.Processed
                        && !excludedInstitutions.Contains(x.InstitutionID)).OrderBy(x => x.PackageID).FirstOrDefault();

				if (importPackage == null) return null;
				importPackage.ChangeCheckStatus(PackageStatus.Processing, dbContext);

                importPackage.PackageData = XmlPreparations.Prepare(importPackage.PackageData);

				return importPackage;
			}
		}

		/// <summary>
		/// Получение необработанного пакета за исключением определённых вузов
		/// </summary>
		public ImportPackage GetUnprocessedPackage(int[] excludedInstitutions)
		{
			using (var dbContext = new ImportEntities())
			{
				var importPackage = dbContext.ImportPackage
					.Where(x => (x.StatusID == (int)PackageStatus.PlacedInQueue) && 
                        (x.TypeID == (int)PackageType.Import || 
                         x.TypeID == (int)PackageType.Delete ||
                         x.TypeID == (int)PackageType.ImportApplicationSingle) 

						&& !excludedInstitutions.Contains(x.InstitutionID))
					.OrderBy(x => x.PackageID).FirstOrDefault();
				if (importPackage == null) return null;

                importPackage.PackageData = XmlPreparations.Prepare(importPackage.PackageData);

				importPackage.ChangeStatus(PackageStatus.Processing, dbContext);

				return importPackage;
			}
		}
	}
}
