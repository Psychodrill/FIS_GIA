using System.Collections.Generic;
using System.Linq;

namespace GVUZ.AppExport.Services
{
    public static class DbApplicationExportLoaderHelper
    {
        public static ICollection<ApplicationExportFinSourceDto> GetFinSourceAndEduForms(
            this DataSetApplicationExport.ApplicationsRow app)
        {
            var forms = app.GetFinSourceAndEduFormsRows().Select(GetFinSourceAndEduForm).ToList();

            if (app.StatusId == 8 && app.OrderEducationSourceId == 20)
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

        public static ApplicationExportFinSourceDto GetFinSourceAndEduForm(
            this DataSetApplicationExport.FinSourceAndEduFormsRow row)
        {
            var dto = new ApplicationExportFinSourceDto
                {
                    CommonBeneficiaryDocTypeId =
                        row.IsCommonBeneficiaryDocTypeIdNull() ? (long?) null : row.CommonBeneficiaryDocTypeId,
                    DirectionId = row.DirectionId,
                    EducationFormId = row.EducationFormId,
                    EducationLevelId = row.EducationLevelId,
                    FinanceSourceId = row.FinanceSourceId,
                    IsForBeneficiary = row.IsIsForBeneficiaryNull() ? (long?) null : row.IsForBeneficiary ? 1 : 0,
                    Number = row.IsNumberNull() ? null : row.Number.ToString(),
                    OrderTypeId = row.IsOrderTypeIdNull() ? (long?) null : row.OrderTypeId,
                    UseBeneficiarySubject =
                        row.IsUseBeneficiarySubjectNull() ? (long?) null : row.UseBeneficiarySubject ? 1 : 0,
                    EntranceTestResults = row.GetEntranceTestResults()
                };

            return dto;
        }

        public static ICollection<ApplicationExportEntranceTestDto> GetEntranceTestResults(this DataSetApplicationExport.FinSourceAndEduFormsRow row)
        {
            return row.GetEntranceTestResultsRows().Select(GetEntranceTestResult).ToList();
        }

        public static ApplicationExportEntranceTestDto GetEntranceTestResult(
            this DataSetApplicationExport.EntranceTestResultsRow row)
        {
            return new ApplicationExportEntranceTestDto
                {
                    EntranceTestResultId = row.EntranceTestResultId,
                    EntranceTestTypeId = row.EntranceTestTypeId,
                    ResultSourceTypeId = row.ResultSourceTypeId,
                    ResultValue = row.ResultValue
                };
        }
    }
}