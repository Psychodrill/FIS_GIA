using System.Data.Entity;
using System.Linq;
using System.Transactions;
using GVUZ.Model.Entrants;

namespace GVUZ.Model.Applications
{
	public static class ApplicationRatingCalculator
	{
		/// <summary>
		/// Вычисление рейтинга заявления по всем КГ
		/// </summary>
		public static void CalculateApplicationRating(int applicationID)
		{
            // MISSING: ApplicationSelectedCompetitiveGroup, ApplicationSelectedCompetitiveGroupItem - пустые таблицы!
            // 
            //using (var dbContext = new EntrantsEntities())
            //{
            //    //using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
            //    //    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            //    //{
            //        //берём РВИ и льготы
            //        ApplicationEntranceTestDocument[] etDocsAll = dbContext.ApplicationEntranceTestDocument
            //            .Include(x => x.EntranceTestItemC)
            //            .Where(x => x.ApplicationID == applicationID && x.EntranceTestItemID != null).ToArray();
            //        ApplicationEntranceTestDocument[] globalDocAll = dbContext.ApplicationEntranceTestDocument
            //            .Where(x => x.ApplicationID == applicationID && x.EntranceTestItemID == null)
            //            .ToArray();

            //        Application app = dbContext.Application
            //            .Include(x => x.ApplicationSelectedCompetitiveGroup).Single(x => x.ApplicationID == applicationID);
            //        //по всем выбранным группам
            //        foreach (var ascg in app.ApplicationSelectedCompetitiveGroup)
            //        {
            //            decimal totalRating = 0;
            //            //суммируем баллы
            //            var etDocs = etDocsAll.Where(x => x.EntranceTestItemC.CompetitiveGroupID == ascg.CompetitiveGroupID).ToArray();
            //            foreach (var doc in etDocs)
            //            {
            //                if (doc.ResultValue.HasValue)
            //                    totalRating += doc.ResultValue.Value;
            //            }

            //            ascg.CalculatedRating = totalRating;
            //            //вычисляем льготу
            //            var globalDoc = globalDocAll.FirstOrDefault(x => x.CompetitiveGroupID == ascg.CompetitiveGroupID);
            //            int benefitType = 0;
            //            if (globalDoc != null && globalDoc.BenefitID != null)
            //                benefitType = globalDoc.BenefitID.Value;
            //            else
            //            {
            //                //если есть дипломы для рви и льгота не типа 2 (Без доп. испытаний) проставляем эту льготу
            //                foreach (var doc in etDocs)
            //                {
            //                    if (doc.BenefitID.HasValue)
            //                        if (benefitType != 2)
            //                            benefitType = doc.BenefitID.Value;
            //                }
            //            }

            //            ascg.CalculatedBenefitID = (benefitType > 0 ? (short?)benefitType : null);
            //            //если заявление в приказе, ставим приказный рейтинг
            //            if (ascg.CompetitiveGroupID == app.OrderCompetitiveGroupID)
            //            {
            //                app.OrderCalculatedRating = ascg.CalculatedRating;
            //                app.OrderCalculatedBenefitID = ascg.CalculatedBenefitID;
            //            }
            //        }

            //        dbContext.SaveChanges();
            //    //    transaction.Complete();
            //    //}
            //}
        }
    }
}