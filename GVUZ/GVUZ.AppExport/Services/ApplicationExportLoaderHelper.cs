using System;
using System.Collections.Generic;
using System.Linq;
using GVUZ.Model.Entrants;

namespace GVUZ.AppExport.Services
{
    public static class ApplicationExportLoaderHelper
    {
        public static ICollection<ApplicationExportFinSourceDto> GetFinSourceAndEduForms(this Application app, int yearStart)
        {
            if (app.ApplicationSelectedCompetitiveGroupItem == null)
            {
                return null;
            }

            var forms = app.ApplicationCompetitiveGroupItem.Where(x => x.Priority.HasValue)
                           .Select(x => new ApplicationExportFinSourceDto
                               {
                                   EducationLevelId = x.CompetitiveGroupItem.EducationLevelID,
                                   DirectionId = x.CompetitiveGroupItem.DirectionID,
                                   FinanceSourceId = x.EducationSourceId,
                                   EducationFormId = x.EducationFormId,
                                   IsForBeneficiary = app.GetIsForBeneficiary(),
                                   UseBeneficiarySubject = app.GetUseBeneficiarySubject(),
                                   CommonBeneficiaryDocTypeId = app.GetCommonBeneficiaryDocTypeId(),
                                   Number = x.GetNumber(yearStart),
                                   OrderTypeId = x.GetOrderTypeId(),
                                   EntranceTestResults = app.GetEntranceTestResults()
                               }).ToList();

            if (app.StatusID == 8 && app.OrderEducationSourceID.GetValueOrDefault() == 20)
            {
                // Если есть приказ с источником финансирования = 20,
                // и при этом для него нет сведений
                // то необходимо проставить сведения о приказе из любого другого блока где эти сведения есть
                // и очистить сведения во всех других блоках т.к. заявление может быть включено только в один приказ
                var p20 = forms.Where(x => x.FinanceSourceId == 20 && !x.OrderTypeId.HasValue).ToArray();
                if (p20.Any())
                {
                    var px = forms.Where(x => x.FinanceSourceId != 20 && x.OrderTypeId.HasValue).ToArray();
                    var rest = forms.Where(x => !px.Contains(x) && !p20.Contains(x)).ToArray();

                    var src = px.FirstOrDefault();

                    if (src != null)
                    {
                        for (int i = 0; i < p20.Length; i++)
                        {
                            var p = p20[i];
                            p.OrderTypeId = src.OrderTypeId;
                            p.IsForBeneficiary = src.IsForBeneficiary;
                            p.UseBeneficiarySubject = src.UseBeneficiarySubject;
                            p.CommonBeneficiaryDocTypeId = src.CommonBeneficiaryDocTypeId;
                            p.EntranceTestResults = new List<ApplicationExportEntranceTestDto>(src.EntranceTestResults);
                        }
                    }

                    for (int i = 0; i < px.Length; i++)
                    {
                        var p = px[i];
                        p.OrderTypeId = null;
                        p.IsForBeneficiary = null;
                        p.UseBeneficiarySubject = null;
                        p.CommonBeneficiaryDocTypeId = null;
                        p.EntranceTestResults = null;
                    }

                    forms = new List<ApplicationExportFinSourceDto>();
                    forms.AddRange(rest);
                    forms.AddRange(p20);
                    forms.AddRange(px);
                }
            }

            return forms;
        }

        public static long? GetIsForBeneficiary(this Application app)
        {
            if (app.StatusID == 8)
            {
                return app.OrderOfAdmission.IsForBeneficiary ? 1 : 0;
            }

            return null;
        }

        public static ICollection<ApplicationExportEntranceTestDto> GetEntranceTestResults(this Application app)
        {
            if (app.StatusID == 8 && app.OrderCompetitiveGroupID.HasValue)
            {
                return app.CompetitiveGroup.EntranceTestItemC.SelectMany(x => x.ApplicationEntranceTestDocument)
                          .Where(x => x.ApplicationID == app.ApplicationID)
                          .Select(y => new ApplicationExportEntranceTestDto
                              {
                                  EntranceTestResultId = y.ID,
                                  EntranceTestTypeId = y.EntranceTestItemC.EntranceTestTypeID,
                                  ResultSourceTypeId = y.SourceID.GetValueOrDefault(),
                                  ResultValue = y.ResultValue.GetValueOrDefault()
                              }).ToList();
            }

            return null;
        }

        public static string GetNumber(this ApplicationCompetitiveGroupItem appItem, int yearStart)
        {
            return GetNumberCount(appItem, yearStart).ToString();
        }

        public static long GetNumberCount(this ApplicationCompetitiveGroupItem appItem, int yearStart)
        {
            Func<AdmissionVolume, long> summary = null;
            if (appItem.CompetitiveGroupItem == null)
            {
                return 0;
            }

            if (appItem.EducationFormId == 10) // Z
            {
                switch (appItem.EducationSourceId)
                {
                    case 14: // NumBudgetZ
                        summary = volume => volume.NumberBudgetZ;
                        break;
                    case 15: // NumPaidZ
                        summary = volume => volume.NumberPaidZ;
                        break;
                    case 16: // NumTargetZ
                        summary = volume => volume.NumberTargetZ;
                        break;
                    case 20: // NumQuotaZ
                        summary = volume => volume.NumberQuotaZ.GetValueOrDefault();
                        break;
                }
            }
            else if (appItem.EducationFormId == 11) // O
            {
                switch (appItem.EducationSourceId)
                {
                    case 14: // NumBudgetO
                        summary = volume => volume.NumberBudgetO;
                        break;
                    case 15: // NumPaidO
                        summary = volume => volume.NumberPaidO;
                        break;
                    case 16: // NumTargetO
                        summary = volume => volume.NumberTargetO;
                        break;
                    case 20: // NumQuotaO
                        summary = volume => volume.NumberQuotaO.GetValueOrDefault();
                        break;
                }
            }
            else if (appItem.EducationFormId == 12) // OZ
            {
                switch (appItem.EducationSourceId)
                {
                    case 14: // NumBudgetOZ
                        summary = volume => volume.NumberBudgetOZ;
                        break;
                    case 15: // NumPaidOZ
                        summary = volume => volume.NumberPaidOZ;
                        break;
                    case 16: // NumTargetOZ
                        summary = volume => volume.NumberTargetOZ;
                        break;
                    case 20: // NumQuotaOZ
                        summary = volume => volume.NumberQuotaOZ.GetValueOrDefault();
                        break;
                }
            }

            if (summary != null)
            {
                return GetNumberCountInternal(appItem, yearStart, summary);
            }

            return 0;
        }

        private static long GetNumberCountInternal(this ApplicationCompetitiveGroupItem appItem,  int yearStart, Func<AdmissionVolume, long> summary)
        {
            return
                appItem.Application.Institution.Campaign.Where(x => x.YearStart == yearStart)
                       .SelectMany(x => x.AdmissionVolume)
                       .Where(
                           x =>
                           x.Course == 1 && x.AdmissionItemTypeID == appItem.CompetitiveGroupItem.EducationLevelID &&
                           x.DirectionID == appItem.CompetitiveGroupItem.DirectionID)
                       .Sum(summary);
        }

        public static long? GetUseBeneficiarySubject(this Application app)
        {
            if (app.StatusID == 8 && app.OrderCompetitiveGroupID.HasValue)
            {
                return app.ApplicationEntranceTestDocument.Where(x => x.EntranceTestItemC != null && x.EntranceTestItemC.CompetitiveGroupID == app.OrderCompetitiveGroupID).Any(y => y.SourceID == 3) ? 1 : 0;
            }

            return null;
        }

        public static long? GetOrderTypeId(this ApplicationCompetitiveGroupItem item)
        {
            if (item.Application.StatusID == 8 && 
                item.CompetitiveGroupItem != null && 
                item.Application.OrderOfAdmission != null &&
                item.CompetitiveGroupItem.EducationLevelID == item.Application.OrderOfAdmission.EducationLevelID && 
                item.CompetitiveGroupItem.DirectionID == item.Application.CompetitiveGroupItem.DirectionID && 
                item.EducationFormId == item.Application.OrderOfAdmission.EducationFormID &&
                item.EducationSourceId == item.Application.OrderOfAdmission.EducationSourceID)
            {

                if (item.Application.OrderOfAdmission.IsForBeneficiary || item.EducationSourceId == 16)
                {
                    return 0;
                }
                if (item.Application.OrderOfAdmission.Stage == 1)
                {
                    return 1;
                }
                if (item.Application.OrderOfAdmission.Stage == 2)
                {
                    return 2;
                }

                return 3;
            }

            return null;
        }

        public static long? GetCommonBeneficiaryDocTypeId(this Application item)
        {
            if (item.StatusID == 8)
            {
                if (!item.OrderCalculatedBenefitID.HasValue || item.OrderCalculatedBenefitID.Value == 3)
                {
                    return 0;
                }

                if (item.OrderCompetitiveGroupID.HasValue)
                {
                    var testDoc =
                        item.ApplicationEntranceTestDocument.FirstOrDefault(
                            x =>
                            x.BenefitID == item.OrderCalculatedBenefitID &&
                            x.CompetitiveGroupID == item.OrderCompetitiveGroupID);
                    if (testDoc != null && testDoc.EntrantDocument != null)
                    {
                        return testDoc.EntrantDocument.DocumentTypeID;
                    }
                }
            }

            return null;
        }
    }
}