using GVUZ.ImportService2016.Core.Dto.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GVUZ.ImportService2016.Core.Main.Extensions;
using GVUZ.ImportService2016.Core.Main.Dictionaries.Olympic;

namespace GVUZ.ImportService2016.Core.Dto.DataReaders.AdmissionInfo
{
    public class BulkBenefitItemReader : BulkReaderBase<PackageDataAdmissionInfoCompetitiveGroupCommonBenefitItem>
    {
        public BulkBenefitItemDataReader BulkBenefitItemDataReader { get; set; } 

        public BulkBenefitItemReader(PackageData packageData)
        {
            BulkBenefitItemDataReader = new BulkBenefitItemDataReader(packageData);

            foreach (var cg in packageData.CompetitiveGroupsToImport())
            {
                if (cg.CommonBenefit != null)
                    foreach (var b in cg.CommonBenefit)
                    {
                        b.ParentCompetitiveGroup = cg.GUID;
                        _records.Add(b);

                        HashSet<int> items = new HashSet<int>();
                        if (b.OlympicsLevels != null)
                            foreach (var olympicLevel in b.OlympicsLevels)
                            {
                                if (!items.Contains(olympicLevel.OlympicID.To(0)))
                                {
                                    items.Add(olympicLevel.OlympicID.To(0));
                                    var newItem = new BulkBenefitItemData() {
                                        ParentID = b.GUID,
                                        OlympicTypeID = olympicLevel.OlympicID.To(0),
                                        SubjectId = null, EgeMinValue = null, ProfileID = null,
                                        LevelFlags = b.LevelForAllOlympicsSpecified ? (olympicLevel.LevelFlag & b.LevelForAllOlympicsFlag) : olympicLevel.LevelFlag,
                                        ClassFlags = b.ClassForAllOlympicsSpecified ? (olympicLevel.ClassID.To(0) & b.ClassForAllOlympics.To(0)) : olympicLevel.ClassID.To(0),
                                    };

                                    BulkBenefitItemDataReader.Add(newItem);
                                    if (olympicLevel.Profiles != null)
                                    {
                                        foreach (var p in olympicLevel.Profiles)
                                        {
                                            BulkBenefitItemDataReader.Add(new BulkBenefitItemData() { ParentID = newItem.GUID, ProfileID = p.To(0) });
                                        }
                                    }
                                }
                            }
                        if (b.Olympics != null)
                            foreach (var olympic in b.Olympics)
                            {
                                if (!items.Contains(olympic.To(0)))
                                {
                                    items.Add(olympic.To(0));
                                    BulkBenefitItemDataReader.Add(new BulkBenefitItemData() { ParentID = b.GUID, OlympicTypeID = olympic.To(0), SubjectId = null, EgeMinValue = null });
                                }
                            }

                        if (b.MinEgeMarks != null)
                        {
                            var mark = b.MinEgeMarks.MinMarks;
                            BulkBenefitItemDataReader.Add(new BulkBenefitItemData() { ParentID = b.GUID, OlympicTypeID = null, SubjectId = mark.SubjectID.To(0), EgeMinValue = mark.MinMark.To(0) });
                        }
                        if (b.ProfileForAllOlympics != null)
                            foreach(var profileID in b.ProfileForAllOlympics)
                            {
                                BulkBenefitItemDataReader.Add(new BulkBenefitItemData() { ParentID = b.GUID, ProfileID = profileID.To(0) });
                            }
                    }
            }

            AddGetter("ID", dto => dto.ID);
            AddGetter("GUID", dto => dto.GUID);
            AddGetter("ParentCompetitiveGroup", dto => dto.ParentCompetitiveGroup);
            AddGetter("ParentEntranceTestItem", dto => dto.ParentEntranceTestItem);
            AddGetter("InstitutionID", dto => packageData.InstitutionId);
            AddGetter("ImportPackageID", dto => packageData.ImportPackageId);

	        AddGetter("OlympicDiplomTypeID", dto => dto.OlympicDiplomTypesParsed);
	        AddGetter("OlympicLevelFlags", dto => dto.LevelForAllOlympicsSpecified ? dto.LevelForAllOlympicsFlag : OlympicTypeVoc.All_Olympic_Level); // check
            AddGetter("BenefitID", dto => dto.BenefitKindID.To(0));
	        AddGetter("IsForAllOlympic", dto => dto.IsForAllOlympics);
            AddGetter("IsProfileSubject", dto => dto.IsProfileSubject);

	        AddGetter("UID", dto => dto.UID);
            AddGetter("OlympicYear", dto => dto.OlympicYear); // DateTime.Now.Year); //
            AddGetter("EgeMinValue", dto => (object)DBNull.Value);

            AddGetter("ClassFlags", dto => dto.ClassForAllOlympicsSpecified ? dto.ClassForAllOlympics.To(0) : (dto.IsVsosh ? OlympicTypeVoc.All_Olympic_Class_Vsosh : OlympicTypeVoc.All_Olympic_Class));
            AddGetter("IsCreative", dto => dto.IsCreative);
            AddGetter("IsAthletic", dto => dto.IsAthletic);
        }
    }
}
