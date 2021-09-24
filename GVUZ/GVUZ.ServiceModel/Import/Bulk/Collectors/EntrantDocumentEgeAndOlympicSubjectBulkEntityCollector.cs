using System;
using System.Collections.Generic;
using System.Linq;
using GVUZ.ServiceModel.Import.Bulk.Collectors.Base;
using GVUZ.ServiceModel.Import.Bulk.Extensions;
using GVUZ.ServiceModel.Import.Bulk.Model;
using GVUZ.ServiceModel.Import.Bulk.Model.Base;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Bulk.Collectors
{
    public class EntrantDocumentEgeAndOlympicSubjectBulkEntityCollector : BulkEntityCollectorBase<ApplicationDto, EntrantDocumentEgeAndOlympicSubjectBulkEntity>
    {
        public EntrantDocumentEgeAndOlympicSubjectBulkEntityCollector(IEnumerable<ApplicationDto> collection, int packageId, int institutionId)
            : base(collection, packageId, institutionId) { }

        public override IEnumerable<IBulkEntity> Collect()
        {
            var subjects = new List<EntrantDocumentEgeAndOlympicSubjectBulkEntity>();

            /*
             * [EgeDocuments]
             */
            subjects.AddRange(
                    _collection.Where(c =>
                        c.ApplicationDocuments != null &&
                        c.ApplicationDocuments.EgeDocuments != null)
                    .Select(c => c.ApplicationDocuments.EgeDocuments)
                    .Aggregate(new List<EntrantDocumentEgeAndOlympicSubjectBulkEntity>(), (total, next) =>
                    {
                        var items = next.Select(x => x.Subjects.Select(n => new EntrantDocumentEgeAndOlympicSubjectBulkEntity
                        {
                            Value = n.ValueDecimal,
                            SubjectId = n.SubjectID.ToIntNullable(),
                            ParentId = x.Id,
                            ImportPackageId = _packageId,
                            InstitutionId = _institutionId
                        })).Aggregate(new List<EntrantDocumentEgeAndOlympicSubjectBulkEntity>(), (t, n) =>
                        {
                            t.AddRange(n);
                            return t;
                        });
                        total.AddRange(items);
                        return total;
                    }));

            /*
             * [GiaDocuments]
             */
            subjects.AddRange(
                    _collection.Where(c =>
                        c.ApplicationDocuments != null &&
                        c.ApplicationDocuments.GiaDocuments != null)
                    .Select(c => c.ApplicationDocuments.GiaDocuments)
                    .Aggregate(new List<EntrantDocumentEgeAndOlympicSubjectBulkEntity>(), (total, next) =>
                    {
                        var items = next.Select(x => x.Subjects.Select(n => new EntrantDocumentEgeAndOlympicSubjectBulkEntity
                        {
                            Value = n.ValueDecimal,
                            SubjectId = n.SubjectID.ToIntNullable(),
                            ParentId = x.Id,
                            ImportPackageId = _packageId,
                            InstitutionId = _institutionId
                        })).Aggregate(new List<EntrantDocumentEgeAndOlympicSubjectBulkEntity>(), (t, n) =>
                        {
                            t.AddRange(n);
                            return t;
                        });
                        total.AddRange(items);
                        return total;
                    }));

            /*
             * [ApplicationCommonBenefits]
             */
            subjects.AddRange(
                   _collection.Where(c =>
                       (c.ApplicationCommonBenefits != null && c.ApplicationCommonBenefits.Any(x =>
                           x.With(t => t.DocumentReason).Return(t => t.OlympicTotalDocument, null) != null)) ||
                       (c.ApplicationCommonBenefit != null &&
                        c.ApplicationCommonBenefit.DocumentReason != null &&
                        c.ApplicationCommonBenefit.DocumentReason.OlympicTotalDocument != null))
                    .Select(c => c.GetCommonBenefits())
                    .Aggregate(new List<EntrantDocumentEgeAndOlympicSubjectBulkEntity>(), (total, next) =>
                    {
                        var items = next
                            .Where(x => x.DocumentReason.OlympicTotalDocument != null)
                            .Select(x => x.DocumentReason.OlympicTotalDocument.Subjects.Select(n => new EntrantDocumentEgeAndOlympicSubjectBulkEntity
                            {
                                SubjectId = n.SubjectID.ToIntNullable(),
                                ParentId = x.DocumentReason.OlympicTotalDocument.Id,
                                ImportPackageId = _packageId,
                                InstitutionId = _institutionId
                            })).Aggregate(new List<EntrantDocumentEgeAndOlympicSubjectBulkEntity>(), (t, n) =>
                            {
                                t.AddRange(n);
                                return t;
                            });
                        total.AddRange(items);
                        return total;
                    }));

            /*
             * [ApplicationCommonBenefits]
             */
            subjects.AddRange(
                   _collection.Where(c => c.EntranceTestResults != null && c.EntranceTestResults.Any(x =>
                       x.With(t => t.ResultDocument).Return(t => t.OlympicTotalDocument, null) != null))
                    .Select(c => c.EntranceTestResults)
                    .Aggregate(new List<EntrantDocumentEgeAndOlympicSubjectBulkEntity>(), (total, next) =>
                    {
                        var items = next
                            .Where(x => x.ResultDocument != null && x.ResultDocument.OlympicTotalDocument != null)
                            .Select(x => x.ResultDocument.OlympicTotalDocument.Subjects.Select(n => new EntrantDocumentEgeAndOlympicSubjectBulkEntity
                            {
                                SubjectId = n.SubjectID.ToIntNullable(),
                                ParentId = x.ResultDocument.OlympicTotalDocument.Id,
                                ImportPackageId = _packageId,
                                InstitutionId = _institutionId
                            })).Aggregate(new List<EntrantDocumentEgeAndOlympicSubjectBulkEntity>(), (t, n) =>
                            {
                                t.AddRange(n);
                                return t;
                            });
                        total.AddRange(items);
                        return total;
                    }));

            return subjects;
        }
    }
}
