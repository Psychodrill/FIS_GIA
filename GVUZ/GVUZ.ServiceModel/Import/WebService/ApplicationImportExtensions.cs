using System.Collections.Generic;
using System.Linq;
using GVUZ.ServiceModel.Import.Core.Operations.Importing;

namespace GVUZ.ServiceModel.Import.WebService
{
    public static class ApplicationImportExtensions
    {
        /// <summary>
        ///     Получение общих льгот для заявления
        /// </summary>
        public static ApplicationEntranceTestDocument[] GetApplicationCommonBenefits(
            this ObjectImporter objectImporter, Application application)
        {
            ApplicationEntranceTestDocument[] appEntranceTestDocs = objectImporter.ObjectLinkManager
                                                                                  .ApplicationLinkWithEntranceTestResultsCommonBenefit
                (application);

            // выбираем только общие льготы
            IEnumerable<ApplicationEntranceTestDocument> checkAppCommonBenefitDbQuery =
                appEntranceTestDocs.Where(x => !x.EntranceTestItemID.HasValue && !x.SourceID.HasValue);

            return checkAppCommonBenefitDbQuery.ToArray();
        }
    }
}