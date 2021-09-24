using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.ImportService2016.Core.Dto.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GVUZ.ImportService2016.Core.Main.Extensions;
using GVUZ.DAL.Dapper.ViewModel.Dictionary;

namespace GVUZ.ImportService2016.Core.Dto.DataReaders.AdmissionInfo
{
    public class BulkCompetitiveGroupItemReader : BulkReaderBase<PackageDataAdmissionInfoCompetitiveGroupCompetitiveGroupItem>
    {
        public BulkCompetitiveGroupItemReader(PackageData packageData)
        {
            foreach (var cg in packageData.CompetitiveGroupsToImport())
            {
                if (cg.CompetitiveGroupItem != null)
                {
                    var cgi = cg.CompetitiveGroupItem;
                    cgi.ParentID = cg.GUID;

                    if (cg.EducationSourceID == EDSourceConst.Budget && cg.EducationFormID == EDFormsConst.O) cgi.ItemElementName = ItemChoiceType.NumberBudgetO;
                    if (cg.EducationSourceID == EDSourceConst.Budget && cg.EducationFormID == EDFormsConst.OZ) cgi.ItemElementName = ItemChoiceType.NumberBudgetOZ;
                    if (cg.EducationSourceID == EDSourceConst.Budget && cg.EducationFormID == EDFormsConst.Z) cgi.ItemElementName = ItemChoiceType.NumberBudgetZ;

                    if (cg.EducationSourceID == EDSourceConst.Paid && cg.EducationFormID == EDFormsConst.O) cgi.ItemElementName = ItemChoiceType.NumberPaidO;
                    if (cg.EducationSourceID == EDSourceConst.Paid && cg.EducationFormID == EDFormsConst.OZ) cgi.ItemElementName = ItemChoiceType.NumberPaidOZ;
                    if (cg.EducationSourceID == EDSourceConst.Paid && cg.EducationFormID == EDFormsConst.Z) cgi.ItemElementName = ItemChoiceType.NumberPaidZ;

                    if (cg.EducationSourceID == EDSourceConst.Quota && cg.EducationFormID == EDFormsConst.O) cgi.ItemElementName = ItemChoiceType.NumberQuotaO;
                    if (cg.EducationSourceID == EDSourceConst.Quota && cg.EducationFormID == EDFormsConst.OZ) cgi.ItemElementName = ItemChoiceType.NumberQuotaOZ;
                    if (cg.EducationSourceID == EDSourceConst.Quota && cg.EducationFormID == EDFormsConst.Z) cgi.ItemElementName = ItemChoiceType.NumberQuotaZ;

                    if (cg.EducationSourceID == EDSourceConst.Target && cg.EducationFormID == EDFormsConst.O) cgi.ItemElementName = ItemChoiceType.NumberTargetO;
                    if (cg.EducationSourceID == EDSourceConst.Target && cg.EducationFormID == EDFormsConst.OZ) cgi.ItemElementName = ItemChoiceType.NumberTargetOZ;
                    if (cg.EducationSourceID == EDSourceConst.Target && cg.EducationFormID == EDFormsConst.Z) cgi.ItemElementName = ItemChoiceType.NumberTargetZ;

                    _records.Add(cgi);
                }

            }

            AddGetter("ID", dto => dto.ID);
            AddGetter("GUID", dto => dto.GUID);
            AddGetter("ParentID", dto => dto.ParentID);
            AddGetter("InstitutionID", dto => packageData.InstitutionId);
            AddGetter("ImportPackageID", dto => packageData.ImportPackageId);

            AddGetter("NumberBudgetO", dto => dto.ItemElementName == ItemChoiceType.NumberBudgetO ? dto.Item : 0);
            AddGetter("NumberBudgetOZ", dto => dto.ItemElementName == ItemChoiceType.NumberBudgetOZ ? dto.Item : 0);
            AddGetter("NumberBudgetZ", dto => dto.ItemElementName == ItemChoiceType.NumberBudgetZ ? dto.Item : 0);
            AddGetter("NumberPaidO", dto => dto.ItemElementName == ItemChoiceType.NumberPaidO ? dto.Item : 0);
            AddGetter("NumberPaidOZ", dto => dto.ItemElementName == ItemChoiceType.NumberPaidOZ ? dto.Item : 0);
            AddGetter("NumberPaidZ", dto => dto.ItemElementName == ItemChoiceType.NumberPaidZ ? dto.Item : 0);

            AddGetter("NumberQuotaO", dto => dto.ItemElementName == ItemChoiceType.NumberQuotaO ? dto.Item : 0);
            AddGetter("NumberQuotaOZ", dto => dto.ItemElementName == ItemChoiceType.NumberQuotaOZ ? dto.Item : 0);
            AddGetter("NumberQuotaZ", dto => dto.ItemElementName == ItemChoiceType.NumberQuotaZ ? dto.Item : 0);

            AddGetter("NumberTargetO", dto => dto.ItemElementName == ItemChoiceType.NumberTargetO ? dto.Item : 0);
            AddGetter("NumberTargetOZ", dto => dto.ItemElementName == ItemChoiceType.NumberTargetOZ ? dto.Item : 0);
            AddGetter("NumberTargetZ", dto => dto.ItemElementName == ItemChoiceType.NumberTargetZ ? dto.Item : 0);

        }
    }
}
