using GVUZ.DAL.Dapper.Repository.Model.Olympics;
using GVUZ.DAL.Dapper.ViewModel.Common;
using GVUZ.Data.Helpers;
using GVUZ.Data.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

namespace GVUZ.DAL.Dapper.ViewModel.Olympics
{
    public class OlympicsCatalogEditViewModel
    {

        //-----------------------------------------------------------------------------------------------------

        OlympicsRepository repository;

        OlympicsRepository Repository
        {
            get
            {
                if (repository == null)
                    repository = new OlympicsRepository();
                return repository;
            }
        }

        //-----------------------------------------------------------------------------------------------------

        public OlympicsCatalogEditViewModel()
        {
            Data = new OlympicTypeProfile();
        }

        public OlympicTypeProfile Data { get; set; }

        //-----------------------------------------------------------------------------------------------------

        public void InitialEdit(int id)
        {
            Data = id > 0 ? Repository.GetOlympicTypeProfile(id) : new OlympicTypeProfile();

            OlympicTypeYears = Repository.GetOlympicTypeYears();
            OlympicProfiles = Repository.GetAllOlympicProfile();
            OlympicLevels = Repository.GetAllOlympicLevel();

            // год проведения олимпиады в фильтре лет
            SelectedYear = id > 0 ? Data.OlympicType.OlympicYear : 0;

            // список олимпиад по году в комбобокс
            OlympicTypeOlympicsByYear = id > 0 ? 
                Repository.GetOlympicTypeNamesByYear(Data.OlympicType.OlympicYear) : 
                Enumerable.Empty<OlympicType>();

            // предметы по олимпиаде
            SelectedSubjects = id > 0?  Data.OlympicSubject.Select(s => s.SubjectID.ToString()).ToArray() : new string[] { };

            // все предметы
            Subjects = Repository.GetOlympicSubjects().Select(s => new SelectListItem
            {
                Value = s.SubjectID.ToString(),
                Text = s.Name,
            });

            if (Data.CoOrganizerID != null)
                CoOrganizer = Repository.GetFullNameInstitution((int)Data.CoOrganizerID).FullName;
        }

        //-----------------------------------------------------------------------------------------------------

        public IEnumerable<OlympicType> OlympicTypeYears { get; set; }

        // Для комбобокса в фильтре - уникальные значения лет проведения олимпиад
        IEnumerable<SelectorItem> years;

        public IEnumerable<SelectorItem> Years
        {
            get
            {
                if (years == null)
                {
                    years = OlympicTypeYears.Select(s => new SelectorItem
                    {
                        Id = s.OlympicYear,
                        Name = s.OlympicYear.ToString()
                    });
                    years = new List<SelectorItem>(){ new SelectorItem
                    {
                        Id = 0,
                        Name = "Не выбрано"
                    }}.Concat(years);
                }
                return years;
            }
        }

        // используется только для фильтра по годам
        [DisplayName("Год олимпиады")]
        public int SelectedYear { get; set; }


        //-----------------------------------------------------------------------------------------------------

        public IEnumerable<OlympicType> OlympicTypeOlympicsByYear { get; set; }


        // Для комбобокса в фильтре - нфименования олимпиад по одному году
        IEnumerable<SelectorItem> olympics;

        public IEnumerable<SelectorItem> Olympics
        {
            get
            {
                if (olympics == null)
                {
                    olympics = OlympicTypeOlympicsByYear.Select(s => new SelectorItem
                    {
                        Id = s.OlympicID,
                        Name = s.Name
                    });
                    olympics = new List<SelectorItem>(){ new SelectorItem
                    {
                        Id = 0,
                        Name = "Не выбрано"
                    }}.Concat(olympics);
                }
                return olympics;
            }
        }

        //-----------------------------------------------------------------------------------------------------

        public IEnumerable<OlympicProfile> OlympicProfiles { get; set; }

        // Для комбобокса в фильтре - наименования профилей олимпиад
        IEnumerable<SelectorItem> profiles;

        public IEnumerable<SelectorItem> Profiles
        {
            get
            {
                if (profiles == null)
                {
                    profiles = OlympicProfiles.Select(s => new SelectorItem
                    {
                        Id = s.OlympicProfileID,
                        Name = s.ProfileName
                    });
                    profiles = new List<SelectorItem>(){ new SelectorItem
                    {
                        Id = 0,
                        Name = "Не выбрано"
                    }}.Concat(profiles);
                }
                return profiles;
            }
        }

        //-----------------------------------------------------------------------------------------------------

        public IEnumerable<OlympicLevel> OlympicLevels { get; set; }

        // Для комбобокса в фильтре - наименования уровней олимпиад
        IEnumerable<SelectorItem> levels;

        public IEnumerable<SelectorItem> Levels
        {
            get
            {
                if (levels == null)
                {
                    levels = OlympicLevels.Select(s => new SelectorItem
                    {
                        Id = s.OlympicLevelID,
                        Name = s.Name
                    });
                    levels = new List<SelectorItem>(){ new SelectorItem
                    {
                        Id = 0,
                        Name = "Не выбрано"
                    }}.Concat(levels);
                }
                return levels;
            }
        }

        //-----------------------------------------------------------------------------------------------------

        public string Connected { get
            {
                if (Data.OrganizerConnected.HasValue)
                    if ((bool)Data.OrganizerConnected)
                        return "1";
                    else
                        return "2";
                else
                    return "0";
            }
        }
        public string CoConnected
        {
            get
            {
                if (Data.CoOrganizerConnected.HasValue)
                    if ((bool)Data.CoOrganizerConnected)
                        return "1";
                    else
                        return "2";
                else
                    return "0";
            }
        }

        [DisplayName("Соорганизатор олимпиады")]
        public string CoOrganizer { get; set; }

        //-----------------------------------------------------------------------------------------------------

        public string[] SelectedSubjects { get; set; }

        public IEnumerable<SelectListItem> Subjects { get; set; }


        //-----------------------------------------------------------------------------------------------------

    }
}
