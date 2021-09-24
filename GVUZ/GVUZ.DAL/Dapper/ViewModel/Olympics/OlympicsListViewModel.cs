using GVUZ.DAL.Dapper.Repository.Model.Olympics;
using GVUZ.DAL.Helpers;
using GVUZ.Data.Helpers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System;

namespace GVUZ.DAL.Dapper.ViewModel.Olympics
{
    public class OlympicsListViewModel
    {
        //----------------------------------------------------------------------------------------------------

        public OlympicsListViewModel()
        {
        }

        public OlympicsListViewModel(FilterData filter) : base()
        {
            Initial(filter);
        }

        //----------------------------------------------------------------------------------------------------

        OlympicsRepository repository;
        public OlympicsRepository Repository
        {
            get
            {
                if (repository == null)
                    repository = new OlympicsRepository();
                return repository;
            }
        }

        //----------------------------------------------------------------------------------------------------

        void Initial(FilterData filter)
        {
            // комбобоксы фильтра нужно заполнить уникальными значениями из всей выборки

            Data = Repository.GetOlympicsData();
            SubjectData = Repository.GetOlympicsSubjectsData();

            if (filter != null)
            {
                filter.Organizer = string.IsNullOrEmpty(filter.Organizer) ? "" : filter.Organizer.Trim().ToUpper();

                Data = filter.Name > 0 ? Data.Where(x => x.OlympicID == filter.Name) : Data;
                Data = filter.Year > 0 ? Data.Where(x => x.OlympicYear == filter.Year) : Data;
                Data = filter.Profile > 0 ? Data.Where(x => x.OlympicProfileID == filter.Profile) : Data;
                Data = filter.Level > 0 ? Data.Where(x => x.OlympicLevelID == filter.Level) : Data;

                if(filter.Vosh > 0)
                    Data = filter.Vosh == 1 ? Data.Where(x => x.OlympicNumber == null) : Data.Where(x => x.OlympicNumber != null);

                Data = filter.Organizer != "" ? Data.Where(x =>
                x.OrganizerName != null &&  x.OrganizerName.ToUpper().Contains(filter.Organizer)) : Data;


                // специальная обработка фильтра по предмету
                if (filter.Subject > 0)
                {
                    var subjects = SubjectData.Where(x => x.SubjectID == filter.Subject).Select(s => s.OlympicTypeProfileID);
                    Data = Data.Where(x => subjects.Contains(x.OlympicTypeProfileID));
                }

                switch (filter.Sort)
                {
                    case 1:
                        Data = Data.OrderBy(x => x.OlympicNumber);
                        break;
                    case -1:
                        Data = Data.OrderByDescending(x => x.OlympicNumber);
                        break;
                    case 2:
                        Data = Data.OrderBy(x => x.Name);
                        break;
                    case -2:
                        Data = Data.OrderByDescending(x => x.Name);
                        break;
                    case 3:
                        Data = Data.OrderBy(x => x.ProfileName);
                        break;
                    case -3:
                        Data = Data.OrderByDescending(x => x.ProfileName);
                        break;
                    case 4:
                        Data = Data.OrderBy(x => x.OrganizerName);
                        break;
                    case -4:
                        Data = Data.OrderByDescending(x => x.OrganizerName);
                        break;
                    case 5:
                        Data = Data.OrderBy(x => x.OlympicYear);
                        break;
                    case -5:
                        Data = Data.OrderByDescending(x => x.OlympicYear);
                        break;
                    default:
                        break;
                }

                Filter = filter;
            }

            Paging();
        }

        //----------------------------------------------------------------------------------------------------

        void Paging()
        {
            if (PagedData != null)
                return;

            Filter.Page = (Filter.Page.HasValue && Filter.Page > 0) ? Filter.Page  : 1;

            PagedData = Data.ToPagedList((int)Filter.Page, 10);
        }

        public IPagedList<RowData> PagedData { get; set; }


        //----------------------------------------------------------------------------------------------------

        public IEnumerable<RowData> Data { get; set; }
        public RowData Description { get { return null; } }
        public class RowData
        {
            public int OlympicID { get; set; }
            public int OlympicProfileID { get; set; }
            public int OlympicLevelID { get; set; }
            public int OlympicTypeProfileID { get; set; }

            [DisplayName("Наименование олимпиады")]
            public string Name { get; set; }

            [DisplayName("№ олимпиады")]
            public int? OlympicNumber { get; set; }

            [DisplayName("Год олимпиады")]
            public int OlympicYear { get; set; }

            [DisplayName("Профиль олимпиады")]
            public string ProfileName { get; set; }

            [DisplayName("Организатор олимпиады")]
            public string OrganizerName { get; set; }

            [DisplayName("Уровень олимпиады")]
            public string LevelName { get; set; }
        }

        //----------------------------------------------------------------------------------------------------

        public IEnumerable<RowSubjectData> SubjectData { get; set; }
        public class RowSubjectData
        {
            public int OlympicTypeProfileID { get; set; }
            public int SubjectID { get; set; }

            [DisplayName("Наименование предмета")]
            public string SubjectName { get; set; }

        }

        //----------------------------------------------------------------------------------------------------

        // Для комбобокса в фильтре - уникальные значения лет проведения олимпиад
        IEnumerable<SelectorItem> years;
        public IEnumerable<SelectorItem> Years
        {
            get
            {
                if (years == null)
                {
                    years = Data.DistinctBy(p => p.OlympicYear).Select(s => new SelectorItem
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

        //----------------------------------------------------------------------------------------------------

        // Для комбобокса в фильтре - уникальные наименования олимпиад
        IEnumerable<SelectorItem> names;
        public IEnumerable<SelectorItem> Names
        {
            get
            {
                if (names == null)
                {
                    names = Data.DistinctBy(p => p.Name).Select(s => new SelectorItem
                    {
                        Id = s.OlympicID,
                        Name = s.Name
                    });
                    names = new List<SelectorItem>() { new SelectorItem
                    {
                        Id = 0,
                        Name = "Не выбрано"
                    }}.Concat(names);
                }
                return names;
            }
        }

        //----------------------------------------------------------------------------------------------------

        // Для комбобокса в фильтре - уникальные профили олимпиад
        IEnumerable<SelectorItem> profiles;
        public IEnumerable<SelectorItem> Profiles
        {
            get
            {
                if (profiles == null)
                {
                    profiles = Data.DistinctBy(p => p.ProfileName).Select(s =>
                            new SelectorItem
                            {
                                Id = s.OlympicProfileID,
                                Name = s.ProfileName
                            });

                    profiles = new List<SelectorItem>() { new SelectorItem
                    {
                        Id = 0,
                        Name = "Не выбрано"
                    }}.Concat(profiles);
                }
                return profiles;
            }
        }

        //----------------------------------------------------------------------------------------------------

        // Для комбобокса в фильтре - уникальные уровни олимпиад
        IEnumerable<SelectorItem> levels;
        public IEnumerable<SelectorItem> Levels
        {
            get
            {
                if (levels == null)
                {
                    levels = Data.DistinctBy(p => p.LevelName).Select(s =>
                            new SelectorItem
                            {
                                Id = s.OlympicLevelID,
                                Name = s.LevelName
                            });

                    levels = new List<SelectorItem>() { new SelectorItem
                    {
                        Id = 0,
                        Name = "Не выбрано"
                    }}.Concat(levels);
                }
                return levels;
            }
        }

        //----------------------------------------------------------------------------------------------------

        // Для комбобокса в фильтре - уникальные наименования предметов олимпиад
        IEnumerable<SelectorItem> subjects;
        public IEnumerable<SelectorItem> Subjects
        {
            get
            {
                if (subjects == null)
                {
                    subjects = SubjectData.DistinctBy(p => p.SubjectName).Select(s =>
                            new SelectorItem
                            {
                                Id = s.SubjectID,
                                Name = s.SubjectName
                            });

                    subjects = new List<SelectorItem>() { new SelectorItem
                    {
                        Id = 0,
                        Name = "Не выбрано"
                    }}.Concat(subjects);
                }
                return subjects;
            }
        }

        //----------------------------------------------------------------------------------------------------

        // Для комбобокса в фильтре ВОШ
        IEnumerable<SelectorItem> voshs;
        public IEnumerable<SelectorItem> Voshs
        {
            get
            {
                if (voshs == null)
                {
                    voshs = new List<SelectorItem>() {
                        new SelectorItem { Id = 0, Name = "Не выбрано" },
                        new SelectorItem { Id = 1, Name = "Да" },
                        new SelectorItem { Id = 2, Name = "Нет" }
                    };
                }
                return voshs;
            }
        }

        //----------------------------------------------------------------------------------------------------

        FilterData filter;
        public FilterData Filter
        {
            get
            {
                if (filter == null) filter = new FilterData();
                return filter;
            }
            set
            {
                filter = value;
            }
        }

        public class FilterData
        {
            [DisplayName("Год олимпиады")]
            public int Year { get; set; }

            [DisplayName("Наименование олимпиады")]
            public int Name { get; set; }

            [DisplayName("Профиль олимпиады")]
            public int Profile { get; set; }

            [DisplayName("Уровень олимпиады")]
            public int Level { get; set; }

            [DisplayName("Наименование организатора")]
            public string Organizer { get; set; }

            [DisplayName("Предмет")]
            public int Subject { get; set; }

            [DisplayName("Всероссийская олимпиада школьников")]
            public int Vosh { get; set; }

            [DisplayName("Сортировка")]
            public int Sort { get; set; }

            [DisplayName("Страница")]
            public int? Page { get; set; }

            public bool HasValue { get; set; }

            public bool Filtered
            {
                get
                {
                    return
                        HasValue ||
                        (Year > 0) || (Name > 0) || (Profile > 0) ||
                        (Level > 0) || (Subject > 0) || (Vosh > 0) ||
                        (!String.IsNullOrEmpty(Organizer));
                }
            }

            public void CopyFilter(FilterData copyFrom)
            {
                if (copyFrom == null)
                    throw new ArgumentNullException("copyFrom");

                this.Year = copyFrom.Year;
                this.Name = copyFrom.Name;
                this.Profile = copyFrom.Profile;
                this.Level = copyFrom.Level;
                this.Organizer = copyFrom.Organizer;
                this.Subject = copyFrom.Subject;
                this.Vosh = copyFrom.Vosh;
            }
        }

        //----------------------------------------------------------------------------------------------------

    }
}
