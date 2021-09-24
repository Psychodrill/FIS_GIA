using GVUZ.DAL.Dapper.Model.AdmissionVolumes;
using GVUZ.DAL.Dapper.Model.AllowedDirections;
using GVUZ.DAL.Dapper.ViewModel.Dictionary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GVUZ.DAL.Dapper.Model.LevelBudgets;



namespace GVUZ.DAL.Dapper.ViewModel.Admission
{
    public class AdmissionVolumeViewModel
    {
        
        //public AdmissionVolumeViewModel()
        //{

        //}
        //public AdmissionVolumeViewModel(Web.ViewModels.AdmissionVolumeViewModel model) : this()
        //{
        //    this.InstitutionID = model.Items.FirstOrDefault().;
        //    this.SelectedCampaignID = model.SelectedCampaignID;

        //    List<RowData> items = new List<RowData>();
        //    foreach (var item in model.Items)
        //    {
        //        items.Add(new RowData(item));

        //    }
        //    this.Items = items.ToArray();

        //}

        public int InstitutionID { get; set; }

        [DisplayName("Приемная кампания")]
        public int SelectedCampaignID { get; set; }
        public IEnumerable<CampaignData> AllowedCampaigns { get; set; }

        public bool HasPlan { get; set; }
        public RowData DisplayData { get { return null; } }
        public RowData[] Items { get; set; }
        public List<List<List<RowData>>> TreeItems { get; set; }
        public class RowData
        {
            //public RowData()
            //{

            //}
            //public RowData(Web.ViewModels.AdmissionVolumeViewModel.RowData item):this()
            //{
            //    ParentDirectionID = item.ParentDirectionID;
            //    AdmissionItemTypeID = item.AdmissionItemTypeID;
            //    AdmissionItemTypeName = item.AdmissionItemTypeName;
            //    DirectionID = item.DirectionID;
            //    DirectionName = item.DirectionName;
            //    DirectionCode = item.DirectionCode;
            //    DirectionNewCode = item.DirectionNewCode;
            //    ParentDirectionName = item.ParentDirectionName;
            //    ParentDirectionCode = item.ParentDirectionCode;
            //    QualificationCode = item.QualificationCode;
            //    NumberBudgetO = item.NumberBudgetO;
            //    NumberBudgetOZ = item.NumberBudgetOZ;
            //    NumberBudgetZ = item.NumberBudgetZ;
            //    NumberPaidO = item.NumberPaidO;
            //    NumberPaidOZ = item.NumberPaidOZ;
            //    NumberPaidZ = item.NumberPaidZ;
            //    NumberTargetO = item.NumberTargetO;
            //    NumberTargetOZ = item.NumberTargetOZ;
            //    NumberTargetZ = item.NumberTargetZ;
            //    NumberQuotaO = item.NumberQuotaO;
            //    NumberQuotaOZ = item.NumberQuotaOZ;
            //    NumberQuotaZ = item.NumberQuotaZ;



            //}

            public int? ParentDirectionID { get; set; }
            [DisplayName("Уровень образования")]
            public short AdmissionItemTypeID { get; set; }
            public string AdmissionItemTypeName { get; set; }
            [DisplayName("Специальность")]
            public int? DirectionID { get; set; }
            public string DirectionName { get; set; }
            [DisplayName("Код")]
            public string DirectionCode { get; set; }
            public string DirectionNewCode { get; set; }
            public string ParentDirectionName { get; set; }
            public string ParentDirectionCode { get; set; }
            public string QualificationCode { get; set; }
            [DisplayName("Контрольные цифры приема (общий конкурс)")]
            public string BudgetName { get { return null; } }
            [DisplayName("Очное обучение")]
            public int NumberBudgetO { get; set; }
            [DisplayName("Очно-заочное обучение")]
            public int NumberBudgetOZ { get; set; }
            [DisplayName("Заочное обучение")]
            public int NumberBudgetZ { get; set; }
            [DisplayName("Планируемый прием на места с оплатой стоимости обучения")]
            public string PaidName { get { return null; } }
            [DisplayName("Очное обучение")]
            public int NumberPaidO { get; set; }
            [DisplayName("Очно-заочное обучение")]
            public int NumberPaidOZ { get; set; }
            [DisplayName("Заочное обучение")]
            public int NumberPaidZ { get; set; }
            [DisplayName("Целевой прием")]
            public string TargetName { get { return null; } }
            [DisplayName("Очное обучение")]
            public int NumberTargetO { get; set; }
            [DisplayName("Очно-заочное обучение")]
            public int NumberTargetOZ { get; set; }
            [DisplayName("Заочное обучение")]
            public int NumberTargetZ { get; set; }
            [DisplayName("Квота приёма лиц, имеющих особое право")]
            public string QuotaName { get { return null; } }
            [DisplayName("Очное обучение")]
            public int? NumberQuotaO { get; set; }
            [DisplayName("Очно-заочное обучение")]
            public int? NumberQuotaOZ { get; set; }
            [DisplayName("Заочное обучение")]
            public int? NumberQuotaZ { get; set; }
            public bool GroupLast { get; set; }
            public bool DisableForEditing { get; set; }
            [DisplayName("UID")]
            public string UID { get; set; }
            public int AdmissionVolumeId { get; set; }

            [DisplayName("Доступно для распределения")]
            public int AvailableForDistribution { get; set; }

            [DisplayName("Из них распределено")]
            public int TotalDistributed { get; set; }

            public bool DisableFormO { get; set; }
            public bool DisableFormOZ { get; set; }
            public bool DisableFormZ { get; set; }

            public bool IsUGS { get; set; }
            public bool IsForUGS { get; set; }
            public int? ParentID { get; set; }
            

        }
        public class CampaignData
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }
        public class AvailFormsInfo
        {
            public int FormID { get; set; }
            public int LevelID { get; set; }
            public int SourceID { get; set; }
        }

        public class AllowedDirectionView
        {
            public short AdmissionItemTypeID { get; set; }
            public string AdmissionTypeName { get; set; }
            public int DirectionID { get; set; }
            public string Code { get; set; }
            public string NewCode { get; set; }
            public string DirectionName { get; set; }
            public int? ParentDirectionID { get; set; }
            public string ParentDirectionName { get; set; }
            public string ParentDirectionCode { get; set; }
            public string QualificationCode { get; set; }
        }

        public AvailFormsInfo[] AvailForms { get; set; }
        public bool IsFormAvail(int levelID, int sourceID, int formID)
        {
            // для уровней образования - магистратура, СПО, кадры высшей квалификации - необходимо убрать возможность ввода чисел 
            // для столбца "Квота приёма лиц, имеющих особое право" для всех форм обучения (очное, очно-заочное, заочное)
            // if (Edsou)
            return ((sourceID != (int)EDSourceConst.Quota) || (levelID != (int)EDLevelConst.Magistracy && levelID != (int)EDLevelConst.SPO && levelID != (int)EDLevelConst.HighQualification));
            // return true;//AvailForms != null && AvailForms.Any(x => x.FormID == formID && x.LevelID == levelID && x.SourceID == sourceID);
        }

        public bool IsQuotaEnabled(int levelId)
        {
            // для уровней образования - магистратура, СПО, кадры высшей квалификации - необходимо убрать возможность ввода чисел 
            // для столбца "Квота приёма лиц, имеющих особое право" для всех форм обучения (очное, очно-заочное, заочное)
            return (levelId != (int)EDLevelConst.Magistracy && levelId != (int)EDLevelConst.SPO && levelId != (int)EDLevelConst.HighQualification);
            //return true;// (levelId == 2 || levelId == 3 || levelId == 5 || levelId == 19);
        }

        [DisplayName("По УГС")]
        public bool IsForUGS(int? ParentDirectionID)
        {
            return (ParentDirectionID != null);
        }

        public class DirectionInfo
        {
            public int DirectionID { get; set; }
            public int ParentID { get; set; }

            [DisplayName("Код")]
            public string DirectionCode { get; set; }
            [DisplayName("Наименование")]
            public string DirectionName { get; set; }
            [DisplayName("Код УГС")]
            public string ParentCode { get; set; }
            [DisplayName("Наименование УГС")]
            public string ParentName { get; set; }
            [DisplayName("Уровень образования")]
            public string EducationLevelName { get; set; }

            public string QualificationCode { get; set; }

            [DisplayName("Новый код")]
            public string NewCode { get; set; }
        }
        public bool CanEdit { get; set; }
        public bool CanTransfer { get; set; }
        public DirectionInfo DirectionDisp { get { return null; } }
        public Dictionary<string, DirectionInfo> CachedDirections { get; set; }
        public IEnumerable <LevelBudget> BudgetLevels { get; set; }
        //public string[] BudgetLevels { get; set; }
        [DisplayName("Контрольные цифры приема (общий конкурс)")]
        public string BudgetName { get { return null; } }
        [DisplayName("Квота приёма лиц, имеющих особое право")]
        public string QuotaName { get { return null; } }
        [DisplayName("Планируемый прием на места с оплатой стоимости обучения")]
        public string PaidName { get { return null; } }
        [DisplayName("Целевой прием")]
        public string TargetName { get { return null; } }
    }
}
