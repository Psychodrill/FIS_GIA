using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections;
using GVUZ.Data.Model;
using GVUZ.Web.ContextExtensionsSQL;
using System.Web.Mvc;
using GVUZ.DAL.Dapper.Repository.Model;

namespace GVUZ.Web.ViewModels
{
    public class ApplicationPriorityViewModel
    {
        public ApplicationPriorityViewModel()
        {
            AllowEdit = true;
        }

        public ApplicationPriorityViewModel(int institutionId, CompetitiveGroup group)
            : this()
        {
            if(group?.Direction == null && group?.ParentDirection == null)
            {
                throw new ArgumentNullException("В конкурсной группе не указаны направления подготовки");
            }
            else 
            {
                CompetitiveGroupItemName = group.Direction?.Name ?? group.ParentDirection.Name;
            }


            InstitutionId = institutionId;

            CompetitiveGroupId = group.CompetitiveGroupID;
            EducationFormId = group.Form.ItemTypeID;
            EducationSourceId = group.Source.ItemTypeID;
            CompetitiveGroupName = group.Name;
            EducationSourceName = group.Source.Name;
            EducationFormName = group.Form.Name;
            LevelName = group.Level.Name;
            CompetitiveGroupProgramRow = group.CompetitiveGroupProgramRow;
            LevelBudgetRow = group.LevelBudgetRow;
            CampaignId = group.CampaignID;
            CampaignTypeId = group.Campaign.CampaignTypeID;

            //IsForSPOandVO = false;
            //CalculatedRating = 0;
            //IsDisagreedDate = DateTime.Now;
            //IsDisagreed = false;
            //IsAgreedDate = DateTime.Now;
            //IsAgreed = false;
            //CompetitiveGroupTargetId = 0;
        }

        List<SelectListItem> sources = new List<SelectListItem>();
        public List<SelectListItem> Sources
        {
            get
            {
                if (AllowSourceSelect)
                {

                    //if (this.TargetOrganizations == null)
                    //{
                        var prioritiesData = SQL.GetPrioritiesData(CompetitiveGroupId);
                        if (prioritiesData.ApplicationPriorities.Count > 0)
                        {
                            this.TargetOrganizations = prioritiesData.ApplicationPriorities[0].TargetOrganizations;
                        }
                    //}

                    if (this.TargetOrganizations != null)
                    {
                        foreach (IDName item in this.TargetOrganizations)
                        {
                            sources.Add(new SelectListItem() { Value = item.ID.ToString(), Text = item.Name });
                        }
                    }
                }
                return sources;
            }
        }

        public bool AllowSourceSelect { get { return EducationSourceId == 16; } }

        /// <summary>
        /// Идентификатор записи в БД
        /// </summary>
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public int InstitutionId { get; set; }
        public int? CampaignId { get; set; }
        public int? CampaignTypeId { get; set; }

        /// <summary>
        /// Наименование уровня для отображения
        /// </summary>
        [DisplayName("Уровень образования")]
        public string LevelName { get; set; }

        /// <summary>
        /// Название конкурсной группы для отображения
        /// </summary>
        [DisplayName("Конкурс")]
        public string CompetitiveGroupName { get; set; }

        /// <summary>
        /// Наименование направления для отображения
        /// </summary>
        [DisplayName("Направление подготовки")]
        public string CompetitiveGroupItemName { get; set; }

        /// <summary>
        /// Наименование формы обучения для отображения
        /// </summary>
        [DisplayName("Форма обучения")]
        public string EducationFormName { get; set; }

        /// <summary>
        /// Наименование источника финансирования для отображения
        /// </summary>
        [DisplayName("Источник финансирования")]
        public string EducationSourceName { get; set; }

        //ICollection<CompetitiveGroupProgram> programs;
        /// <summary>
        /// Название программ для отображения
        /// </summary>
        //[DisplayName("ОП")]
        //public ICollection<CompetitiveGroupProgram> Programs
        //{
        //    get
        //    {
        //        //programs = "";
        //        //if (CompetitiveGroupProgram != null)
        //        //foreach (var program in CompetitiveGroupProgram)
        //        //{
        //        //    if (programs != "")
        //        //        programs = programs + ';';
        //        //    programs = programs + program.Code + " " + program.Name + "\n";
        //        //}
        //        return CompetitiveGroupProgram;

        //    }
        //}

        /// <summary>
        /// Идентификатор конкурсной группы
        /// </summary>
        public int CompetitiveGroupId { get; set; }

        /// <summary>
        /// Идентификатор направления
        /// </summary>
        public int CompetitiveGroupItemId { get; set; }

        /// <summary>
        /// Идентификатор формы обучения
        /// </summary>
        public int EducationFormId { get; set; }

        /// <summary>
        /// Код источника финансирования
        /// </summary>
        public int EducationSourceId { get; set; }

        /// <summary>
        /// Приоритет
        /// </summary>
        [DisplayName("Приоритет")]
        public int? Priority { get; set; }

        /// <summary>
        /// Код организации целевого приёма
        /// </summary>
        public int? CompetitiveGroupTargetId { get; set; }

        public IEnumerable TargetOrganizations { get; set; }

        public string TargetOrganizationName { get; set; }

        public string ContractOrganizationName { get; set; }

        public override string ToString()
        {
            string result = String.Format("{0} - {1} - {2}", CompetitiveGroupName, CompetitiveGroupItemName, EduFormAndSource);
            return result;
        }

        public string EduFormAndSource
        {
            get
            {
                return String.Format("{0} - {1}", EducationFormName, (EducationSourceId == 16) ? (EducationSourceName + String.Format("({0})", TargetOrganizationName)) : EducationSourceName);
            }
        }

        public int DirectionID { get; set; }
        public short EducationLevelID { get; set; }
        public bool? IsForSPOandVO { get; set; }
        public bool? IsAgreed { get; set; }
        public bool? IsDisagreed { get; set; }
        public DateTime? IsAgreedDate { get; set; }
        public DateTime? IsDisagreedDate { get; set; }
        public decimal? CalculatedRating { get; set; }
        public bool AllowEdit { get; set; }
        public bool UnlimitedAgreements { get; set; }

        public string CompetitiveGroupProgramRow { get; set; }
        public string LevelBudgetRow { get; set; }

        //public ICollection<CompetitiveGroupProgram> CompetitiveGroupProgram { get; set; }
        //public ICollection<LevelBudget> LevelBudget { get; set; }
    }

    public class PrioritySaveViewModel
    {
        public int Id { get; set; }
        public int? Priority { get; set; }
    }
}
