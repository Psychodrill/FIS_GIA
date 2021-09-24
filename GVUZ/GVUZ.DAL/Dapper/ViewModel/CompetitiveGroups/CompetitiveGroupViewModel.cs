using GVUZ.DAL.Dapper.ViewModel.Dictionary;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GVUZ.DAL.Dapper.ViewModel.Common;
using GVUZ.DAL.Dapper.Model.CompetitiveGroups;
using System.Linq;
using GVUZ.DAL.Dapper.Model.TargetOrganization;
using GVUZ.DAL.Dapper.Model.Dictionary;
using GVUZ.DAL.Dapper.Model.Benefit;
using GVUZ.DAL.Dto;
using System;
using GVUZ.DAL.Dapper.Model.LevelBudgets;

namespace GVUZ.DAL.Dapper.ViewModel.CompetitiveGroups
{
    public class CompetitiveGroupViewModel
    {
        public CompetitiveGroupViewModel()
        {
            this.CompetitiveGroupEdit = new CompetitiveGroupEditModel();
            this.EntranceTestItemsEdit = new EntranceTestItemsEditModel();
            this.CompetitiveGroupProgramsEdit = new CompetitiveGroupProgramsEditModel();
            this.CompetitiveGroupTargetsEdit = new CompetitiveGroupTargetsEditModel();

            IsFromKrymCollection = new SelectListViewModel<bool?>();
            //IsFromKrymCollection.Add(null, "[Не важно]");
            IsFromKrymCollection.Add(true, "Да");
            IsFromKrymCollection.Add(false, "Нет");

            IsAdditionalCollection = new SelectListViewModel<bool?>();
            //IsAdditionalCollection.Add(null, "[Не важно]");
            IsAdditionalCollection.Add(true, "Да");
            IsAdditionalCollection.Add(false, "Нет");

            PageNumber = 0;
            TotalPageCount = 0;
        }

        public int? SortID { get; set; }
        public int? PageNumber { get; set; }
        public int TotalPageCount { get; set; }
        public int TotalItemCount { get; set; }
        public int TotalFilteredCount { get; set; }
        public IEnumerable<CompetitiveGroupDataModel> CompetitiveGroupList { get; set; }
        //public IEnumerable<CompetitiveGroupProperty> Properties { get; set; }

        public CompetitiveGroupDataModel DisplayColumns
        {
            get { return new CompetitiveGroupDataModel(); }
        }

        public IEnumerable CampaignStartYears { get; set; }
        public IEnumerable<CampaignTypesView> CampaignTypes { get; set; }
        public IEnumerable<AdmissionItemTypeView> EducationLevels { get; set; }
        public IEnumerable<AdmissionItemTypeView> EducationForms { get; set; }
        public IEnumerable<AdmissionItemTypeView> EducationFinanceSources { get; set; }

        public SelectListViewModel<bool?> IsFromKrymCollection { get; private set; }
        public SelectListViewModel<bool?> IsAdditionalCollection { get; private set; }

        public IEnumerable<CampaignWithTypeViewModel> Campaigns { get; set; }

        public IEnumerable<DirectionViewModel> Directions { get; set; }

        public IEnumerable<SubjectView> Subjects { get; set; }

        public IEnumerable<LevelBudget> LevelBudgets { get; set; }

        public bool IsMultiProfile { get; set; }



        //public CampaignEditModel CampaignEdit { get; set; }
        //public bool isCountCampaignType { get; set; }

        public class CompetitiveGroupDataModel
        {


            [DisplayName("Действие")]
            public int CompetitiveGroupID { get; set; }

            public int InstitutionID { get; set; }

            [DisplayName("Наименование")]
            [Required]
            [StringLength(250)]
            public string CompetitiveGroupName { get; set; }

            public string CampaignTypeName { get; set; }

            [DisplayName("Год ПК")]
            public int CampaignYearStart { get; set; }

            [DisplayName("Приемная кампания")]
            public string CampaignName { get { return string.Format("{0} ({1})", CampaignTypeName, CampaignYearStart); } }

            [DisplayName("Уровень образования")]
            public string EducationLevelName { get; set; }

            [DisplayName("Направление подготовки")]
            public string DirectionName { get; set; }

            [DisplayName("УГС")]
            public string UGSNAME { get; set; }

            [DisplayName("Источник финансирования")]
            public string EducationSourceName { get; set; }

            [DisplayName("Форма обучения")]
            public string EducationFormName { get; set; }

            [DisplayName("Прием жителей Крыма и Севастополя")]
            public bool IsFromKrym { get; set; }

            [DisplayName("Прием граждан Крыма")]
            public string IsFromKrymName { get { return IsFromKrym ? "Да" : "Нет"; } }

            [DisplayName("Дополнительный набор")]
            public bool IsAdditional { get; set; }

            [DisplayName("Дополнительный набор")]
            public string IsAdditionalName { get { return IsAdditional ? "Да" : "Нет"; } }

            [DisplayName("Уровень образования")]
            public int EducationLevelID { get; set; }
            [DisplayName("Источник финансирования")]
            public int EducationSourceID { get; set; }
            [DisplayName("Форма обучения")]
            public int EducationFormID { get; set; }
            [DisplayName("Направление подготовки")]
            public int? DirectionID { get; set; }
            [DisplayName("УГС")]
            public int? ParentDirectionID { get; set; }

            [DisplayName("Уровень бюджета")]
            public int? IdLevelBudget { get; set; }

            protected DateTime studyBeginningDate;
            protected DateTime studyEndingDate;
            protected int studyPeriod;
            protected IEnumerable<CompetitiveGroupProperty> properties;


        }

        #region Фильтр
        public FilterData Filter { get; set; }
        public class FilterData
        {
            [DisplayName("Наименование конкурса")]
            public string Name { get; set; }

            [DisplayName("Год начала проведения ПК")]
            public int CampaignYearStart { get; set; }

            [DisplayName("Тип ПК")]
            public int CampaignTypeID { get; set; }

            [DisplayName("Уровень образования")]
            public int EducationLevelID { get; set; }

            [DisplayName("Источник финансированя")]
            public int EducationFinanceSourceID { get; set; }

            [DisplayName("Форма обучения")]
            public int EducationFormID { get; set; }

            [DisplayName("Направление подготовки/специальность")]
            public string Direction { get; set; }

            [DisplayName("Прием жителей Крыма и Севастополя")]
            public bool? IsFromKrym { get; set; }

            [DisplayName("Дополнительный набор")]
            public bool? IsAdditional { get; set; }

            public override string ToString()
            {
                return string.Format("{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}_{8}",
                    Name, CampaignYearStart, CampaignTypeID, EducationLevelID, EducationFinanceSourceID, EducationFormID, Direction, IsFromKrym, IsAdditional);
            }
        }
        #endregion

        #region Конкурс - скалярные поля
        public CompetitiveGroupEditModel CompetitiveGroupEdit { get; set; }
        public class CompetitiveGroupEditModel : CompetitiveGroupDataModel
        {
            public CompetitiveGroupEditModel()
            {
                CanEdit = true;
            }

            [DisplayName("UID")]
            public string Uid { get; set; }

            public IEnumerable<AdmissionItemTypeView> EducationLevels { get; set; }
            public IEnumerable<AdmissionItemTypeView> EducationForms { get; set; }
            public IEnumerable<AdmissionItemTypeView> EducationFinanceSources { get; set; }

            private List<CompetitiveGroupProperty> innerList = new List<CompetitiveGroupProperty>();

            public IEnumerable<CompetitiveGroupProperty> Properties
            {
                get
                {
                    if (properties == null && innerList.Count == 0)
                    {
                        innerList.Add(new CompetitiveGroupProperty() { PropertyTypeCode = 1, PropertyValue = "0" });
                        innerList.Add(new CompetitiveGroupProperty() { PropertyTypeCode = 2, PropertyValue = DateTime.MinValue.ToShortDateString() });
                        innerList.Add(new CompetitiveGroupProperty() { PropertyTypeCode = 3, PropertyValue = DateTime.MinValue.ToShortDateString() });
                        properties = innerList;
                    }

                    return properties ?? innerList;
                }
                set
                {
                    properties = value;
                    if (properties.Count()==0)
                    {
                        innerList.Add(new CompetitiveGroupProperty() { PropertyTypeCode = 1, PropertyValue = "0" });
                        innerList.Add(new CompetitiveGroupProperty() { PropertyTypeCode = 2, PropertyValue = DateTime.MinValue.ToShortDateString() });
                        innerList.Add(new CompetitiveGroupProperty() { PropertyTypeCode = 3, PropertyValue = DateTime.MinValue.ToShortDateString() });
                        properties = innerList;
                    }


                }
            }

            // ...

            /// <summary>
            /// Вуз МВД
            /// </summary>
            public bool IsMVD { get; set; }
            public bool IsMultiProfile { get; set; }

            public bool CanEdit { get; set; }
            [DisplayName("Тип ПК")]
            public int? CampaignID { get; set; }
            public int? CampaignStatusID { get; set; }

            [DisplayName("Количество мест")]
            public int Value { get; set; }


            [DisplayName("Срок обучения в мес.")]
            public int StudyPeriod
            {
                get
                {
                    if (studyPeriod == 0)
                    {
                        return Convert.ToInt32(this.Properties?.Where(x => x.PropertyTypeCode == 1).Select(x => x.PropertyValue).FirstOrDefault());
                    }
                    return studyPeriod;

                }
                set
                {
                    studyPeriod = value;

                }
            }

            [DisplayName("Дата начала обучения")]
            public DateTime StudyBeginningDate
            {
                get
                {

                    if (studyBeginningDate == DateTime.MinValue)
                    {
                        return Convert.ToDateTime(this.Properties?.Where(x => x.PropertyTypeCode == 2).Select(x => x.PropertyValue).FirstOrDefault());
                    }
                    return studyBeginningDate;
                }
                set
                {
                    studyBeginningDate = value;
                }
            }

            [DisplayName("Дата окончания обучения")]
            public DateTime StudyEndingDate
            {
                get
                {
                    if (studyEndingDate == DateTime.MinValue)
                    {
                        return Convert.ToDateTime(this.Properties?.Where(x => x.PropertyTypeCode == 3).Select(x => x.PropertyValue).FirstOrDefault());
                    }
                    return studyEndingDate;

                }
                set
                {
                    studyEndingDate = value;

                }
            }


            public int NumberBudgetO { get; set; }
            public int NumberBudgetOZ { get; set; }
            public int NumberBudgetZ { get; set; }

            public int NumberPaidO { get; set; }
            public int NumberPaidOZ { get; set; }
            public int NumberPaidZ { get; set; }

            public int NumberQuotaO { get; set; }
            public int NumberQuotaOZ { get; set; }
            public int NumberQuotaZ { get; set; }

            public int NumberTargetO { get; set; }
            public int NumberTargetOZ { get; set; }
            public int NumberTargetZ { get; set; }

            public int ApplicationState8 { get; set; }
            public int ApplicationStateNot8 { get; set; }

            // Для клиентской проверки уникальности UID
            public IEnumerable<UidViewModel> Uids { get; set; }

            /// <summary>
            /// Срок действия олимпиад
            /// </summary>
            public int OlympicValidityYears { get; set; }

            private List<CompetitiveGroupProperty> editedProperties = new List<CompetitiveGroupProperty>();

        }
        #endregion

        #region Образовательные программы
        public CompetitiveGroupProgramsEditModel CompetitiveGroupProgramsEdit { get; set; }
        public class CompetitiveGroupProgramsEditModel
        {
            [DisplayName("Образовательная программа")]
            [StringLength(500)]
            public string Program { get; set; }

            //[DisplayName("UID")]
            //[StringLength(200)]
            //public string UID { get; set; }


            //[DisplayName("Наименование")]
            //[StringLength(200)]
            //public string Name { get; set; }

            //[DisplayName("Код")]
            //[StringLength(10)]
            //public string Code {get; set;}

            //программы этого конкурса
            public IEnumerable<CompetitiveGroupProgram> Programs { get; set; }

            //все программы ОО
            public IEnumerable<CompetitiveGroupInstitutionProgram> InstitutionPrograms { get; set; }
        }
        #endregion

        #region Целевые организации
        public CompetitiveGroupTargetsEditModel CompetitiveGroupTargetsEdit { get; set; }
        public class CompetitiveGroupTargetsEditModel
        {
            [DisplayName("UID")]
            [StringLength(200)]
            public string UID { get; set; }


            [DisplayName("Наименование")]
            [StringLength(250)]
            public string Name { get; set; }

            [DisplayName("Наименование")]
            [StringLength(250)]
            public string ContractOrganizationName { get; set; }

            [DisplayName("Количество мест")]
            [StringLength(10)]
            public int Value { get; set; }

            /// <summary>
            /// Все целевые организации данного ОО
            /// </summary>
            public IEnumerable<CompetitiveGroupTargetItemViewModel> TargetOrganizations { get; set; }

            /// <summary>
            /// Целевые организации данного Конкурса
            /// </summary>
            public IEnumerable<CompetitiveGroupTargetItemViewModel> Targets { get; set; }

            public int TargetsCount { get { return Targets != null ? Targets.Count() : 0; } }
        }

        public IEnumerable<CompetitiveGroupTargetsEditResultModel> CompetitiveGroupTargetsEditResult { get; set; }
        public class CompetitiveGroupTargetsEditResultModel
        {
            public int CompetitiveGroupTargetID { get; set; }
            public string DisplayName { get; set; }
            public int Value { get; set; }
        }

        #endregion

        #region Вступительные испытания
        public EntranceTestItemsEditModel EntranceTestItemsEdit { get; set; }
        public class EntranceTestItemsEditModel
        {
            public EntranceTestItemsEditModel()
            {
                BenefitItemColumns = new BenefitItemViewModel();
            }

            public IEnumerable<EntranceTestItemDataViewModel> TestItems { get; set; }
            public IEnumerable<EntranceTestItemDataViewModel> CreativeTestItems { get; set; }
            public IEnumerable<EntranceTestItemDataViewModel> ProfileTestItems { get; set; }

            public IEnumerable<UidViewModel> Uids { get; set; }

            [DisplayName("Вступительное испытание")]
            [StringLength(100)]
            public string EntranceTestName { get; set; }

            [DisplayName("UID")]
            [StringLength(200)]
            public string UID { get; set; }

            [DisplayName("Мин. балл")]
            public decimal? MinScore { get; set; }

            [DisplayName("Приоритет (От 1 до 10, 1 – максимальный приоритет)")]
            public int? EntranceTestPriority { get; set; }

            //для показа
            [DisplayName("ВИ для профильных СПО/ВО")]
            public bool IsForProfile { get; set; }

            [DisplayName("Заменяемое ВИ")]
            [StringLength(100)]
            public string EntranceTestNameForChange { get; set; }

            [DisplayName("ВИ №3 с предметами по выбору")]
            [StringLength(100)]
            public string ComplexEntranceTestItem { get; set; }

            //[DisplayName("ВИ №2 с предметами по выбору")]
            //[StringLength(100)]
            //public string SecondComplexEntranceTestItem { get; set; }



            public IEnumerable<BenefitItemViewModel> BenefitItems { get; set; }

            public BenefitItemViewModel BenefitItemColumns { get; set; }


            // Справочники 
            public List<BenefitViewModel> BenefitList { get; set; } // Это чтобы не путать с BenefitItems (которых раньше называли Benefits)
            public List<SimpleDto> OlympicLevels { get; set; }
            public List<SimpleDto> OlympicProfiles { get; set; }
            public List<SimpleDto> OlympicDiplomTypes { get; set; }
            public List<Olympic.OlympicTypeViewModel> OlympicTypes { get; set; }
            public List<int> OlympicTypeYears { get; set; }

            public string GetBenefit1Name {
                get
                {
                    return (BenefitList != null && BenefitList.Any(t => t.BenefitID == 1)) ? BenefitList.First(t => t.BenefitID == 1).Name : "Зачисление без вступительных испытаний";
                }
            }
            public string GetBenefit3Name
            {
                get
                {
                    return (BenefitList != null && BenefitList.Any(t => t.BenefitID == 3)) ? BenefitList.First(t => t.BenefitID == 3).Name : "Приравнивание к лицам, набравшим максимальное количество баллов по ЕГЭ";
                }
            }

            public List<GlobalMinEge> GlobalMinEge { get; set; }
        }

        //public EntranceTestItemsEditResultModel EntranceTestItemsEditResult { get; set; }

        public class EntranceTestItemsEditResultModel
        {
            public IEnumerable<EntranceTestItemDataViewModel> TestItems { get; set; }
            public IEnumerable<EntranceTestItemDataViewModel> CreativeTestItems { get; set; }
            public IEnumerable<EntranceTestItemDataViewModel> ProfileTestItems { get; set; }
        }
        #endregion


        public Tuple<List<EntranceTestItemDataViewModel>, List<BenefitItemViewModel>> GetTestsAndBenefits()
        {
            List<EntranceTestItemDataViewModel> allTests = new List<EntranceTestItemDataViewModel>();
            List<BenefitItemViewModel> allBenefitItems = new List<BenefitItemViewModel>();
            var entranceTestItemsModel = this.EntranceTestItemsEdit; // model.EntranceTestItemsEditResult;
            if (entranceTestItemsModel != null)
            {
                if (entranceTestItemsModel.BenefitItems != null)
                    allBenefitItems.AddRange(entranceTestItemsModel.BenefitItems);

                if (entranceTestItemsModel.TestItems != null)
                {
                    allTests.AddRange(entranceTestItemsModel.TestItems);
                    allBenefitItems.AddRange(entranceTestItemsModel.TestItems.Where(t => t.BenefitItems != null).SelectMany(t => t.BenefitItems));
                }
                if (entranceTestItemsModel.CreativeTestItems != null)
                {
                    allTests.AddRange(entranceTestItemsModel.CreativeTestItems);
                    allBenefitItems.AddRange(entranceTestItemsModel.CreativeTestItems.Where(t => t.BenefitItems != null).SelectMany(t => t.BenefitItems));
                }
                if (entranceTestItemsModel.ProfileTestItems != null)
                {
                    allTests.AddRange(entranceTestItemsModel.ProfileTestItems);
                    allBenefitItems.AddRange(entranceTestItemsModel.ProfileTestItems.Where(t => t.BenefitItems != null).SelectMany(t => t.BenefitItems));
                }
            }
            return new Tuple<List<EntranceTestItemDataViewModel>, List<BenefitItemViewModel>>(allTests, allBenefitItems);
        }
    }
}
