using System;
using System.Collections.Generic;
using System.Linq;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using GVUZ.ServiceModel.Import.Core.Storages;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents;

namespace GVUZ.ServiceModel.Import.WebService.Dto
{
    public static class DtoValidationExtensions
    {
#warning arzyanin added
        public static void CheckIdentityDocumentsDuplicates(this ApplicationDto[] applicationDtos,
                                                            ConflictStorage conflictStorage)
        {
            /* Проверяем что, если у нас импортируются несколько заявлений от 1 абитуриента 
             * с IdentityDocument, то если эти документы одинкаковые - грузим только 1 документ, если разные
             * не грузим оба заявления с ошибкой */
            var duplicatedEntrantUIDs = from c in applicationDtos
                                        group c by new {c.Entrant.UID, c.Entrant.FIO}
                                        into g
                                        where g.Count() > 1
                                        select g.Key;

            foreach (var key in duplicatedEntrantUIDs)
            {
                IEnumerable<IdentityDocumentDto> identityDocuments = applicationDtos.Where(
                    c => c.Entrant.UID == key.UID && c.Entrant.FIO == key.FIO
                         && c.ApplicationDocuments != null &&
                         c.ApplicationDocuments.IdentityDocument != null)
                                                                                    .Select(
                                                                                        c =>
                                                                                        c.ApplicationDocuments
                                                                                         .IdentityDocument);

                /* Если это пройдет в импорт, то одинаковые документы должны иметь одинаковые Id,
                 * чтобы их не размножить в EntrantDocument таблице для каждого из заявлений */
                Guid documentId = Guid.NewGuid();
                foreach (IdentityDocumentDto identityDocumentDto in identityDocuments)
                    identityDocumentDto.Id = documentId;

                if (identityDocuments.Distinct().Count() > 1)
                {
                    foreach (
                        ApplicationDto applicationDto in
                            applicationDtos.Where(c => c.Entrant.UID == key.UID && c.Entrant.FIO == key.FIO))
                        conflictStorage.AddNotImportedDto(applicationDto,
                                                          ConflictMessages.TooManyIdentityDocumentsForEntrant);
                }
            }
        }
    }
}