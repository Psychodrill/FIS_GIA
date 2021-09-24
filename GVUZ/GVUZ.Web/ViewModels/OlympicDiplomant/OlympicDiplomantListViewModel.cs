using GVUZ.DAL.Helpers;
using GVUZ.Data.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace GVUZ.Web.ViewModels.OlympicDiplomant
{
    public class OlympicDiplomantListViewModel : OlympicDiplomantBaseViewModel
    {
        //-----------------------------------------------------------------------------------------------------
        public IEnumerable<GVUZ.Data.Model.OlympicDiplomant> Data { get; set; }

        public OlympicDiplomantListViewModel() : base()
        {
        }

        int? institutionID;
        public OlympicDiplomantListViewModel(OlympicDiplomantFilterViewModel filter, int? institutionID) : this()
        {
            this.institutionID = institutionID;

            Data = Repository.GetOlympicDiplomantAll(this.institutionID);
            Filter = filter;
            Filtering();
            Paging();
        }

        //-----------------------------------------------------------------------------------------------------

        void Filtering()
        {
            Data = filter.Year > 0 ? Data.Where(x => x.OlympicTypeProfile.OlympicType.OlympicYear == filter.Year) : Data;
            Data = filter.Name > 0 ? Data.Where(x => x.OlympicTypeProfile.OlympicType.Name == Names.Where(n => n.Id == filter.Name).FirstOrDefault().Name) : Data;
            Data = filter.Profile > 0 ? Data.Where(x => x.OlympicTypeProfile.OlympicProfileID == filter.Profile) : Data;

            filter.FirstName = string.IsNullOrEmpty(filter.FirstName) ? "" : filter.FirstName.Trim().ToUpper();
            Data = filter.FirstName != "" ? Data.Where(x => x.OlympicDiplomantDocument.FirstName != null && 
                x.OlympicDiplomantDocument.FirstName.ToUpper().Contains(filter.FirstName)) : Data;

            filter.LastName = string.IsNullOrEmpty(filter.LastName) ? "" : filter.LastName.Trim().ToUpper();
            Data = filter.LastName != "" ? Data.Where(x => x.OlympicDiplomantDocument.LastName != null &&
                x.OlympicDiplomantDocument.LastName.ToUpper().Contains(filter.LastName)) : Data;

            filter.MiddleName = string.IsNullOrEmpty(filter.MiddleName) ? "" : filter.MiddleName.Trim().ToUpper();
            Data = filter.MiddleName != "" ? Data.Where(x => x.OlympicDiplomantDocument.MiddleName != null &&
                x.OlympicDiplomantDocument.MiddleName.ToUpper().Contains(filter.MiddleName)) : Data;

            filter.DocumentSeries = string.IsNullOrEmpty(filter.DocumentSeries) ? "" : filter.DocumentSeries.Trim().ToUpper();
            Data = filter.DocumentSeries != "" ? Data.Where(x => x.OlympicDiplomantDocument.DocumentSeries != null &&
                x.OlympicDiplomantDocument.DocumentSeries.ToUpper().Contains(filter.DocumentSeries)) : Data;

            filter.DocumentNumber = string.IsNullOrEmpty(filter.DocumentNumber) ? "" : filter.DocumentNumber.Trim().ToUpper();
            Data = filter.DocumentNumber != "" ? Data.Where(x => x.OlympicDiplomantDocument.DocumentNumber != null &&
                x.OlympicDiplomantDocument.DocumentNumber.ToUpper().Contains(filter.DocumentNumber)) : Data;

            filter.DiplomaSeries = string.IsNullOrEmpty(filter.DiplomaSeries) ? "" : filter.DiplomaSeries.Trim().ToUpper();
            Data = filter.DiplomaSeries != "" ? Data.Where(x => x.DiplomaSeries != null &&
                x.DiplomaSeries.ToUpper().Contains(filter.DiplomaSeries)) : Data;

            filter.DiplomaNumber = string.IsNullOrEmpty(filter.DiplomaNumber) ? "" : filter.DiplomaNumber.Trim().ToUpper();
            Data = filter.DiplomaNumber != "" ? Data.Where(x => x.DiplomaNumber != null &&
                x.DiplomaNumber.ToUpper().Contains(filter.DiplomaNumber)) : Data;


            Data = filter.ResultLevelID > 0 ? Data.Where(x => x.ResultLevelID == filter.ResultLevelID) : Data;
            Data = filter.StatusID > 0 ? Data.Where(x => x.StatusID == filter.StatusID) : Data;
            Data = filter.EndingDate > 0 ? Data.Where(x => x.EndingDate == filter.EndingDate) : Data;


            switch (filter.Sort)
            {
                case 2:
                    Data = Data.OrderBy(x => x.OlympicDiplomantDocument.LastName);
                    break;
                case -2:
                    Data = Data.OrderByDescending(x => x.OlympicDiplomantDocument.LastName);
                    break;
                case 3:
                    Data = Data.OrderBy(x => x.OlympicDiplomantDocument.FirstName);
                    break;
                case -3:
                    Data = Data.OrderByDescending(x => x.OlympicDiplomantDocument.FirstName);
                    break;
                case 4:
                    Data = Data.OrderBy(x => x.OlympicDiplomantDocument.MiddleName);
                    break;
                case -4:
                    Data = Data.OrderByDescending(x => x.OlympicDiplomantDocument.MiddleName);
                    break;
                case 6:
                    Data = Data.OrderBy(x => x.FormNumber);
                    break;
                case -6:
                    Data = Data.OrderByDescending(x => x.FormNumber);
                    break;
                case 8:
                    Data = Data.OrderBy(x => x.ResultLevelID);
                    break;
                case -8:
                    Data = Data.OrderByDescending(x => x.ResultLevelID);
                    break;
                case 9:
                    Data = Data.OrderBy(x => x.StatusID);
                    break;
                case -9:
                    Data = Data.OrderByDescending(x => x.StatusID);
                    break;
                case 5:
                    Data = Data.OrderBy(x => x.OlympicDiplomantDocument.DocumentSeries).OrderBy(x => x.OlympicDiplomantDocument.DocumentNumber);
                    break;
                case -5:
                    Data = Data.OrderByDescending(x => x.OlympicDiplomantDocument.DocumentSeries).OrderByDescending(x => x.OlympicDiplomantDocument.DocumentNumber);
                    break;
                case 7:
                    Data = Data.OrderBy(x => x.DiplomaSeries).OrderBy(x => x.DiplomaNumber);
                    break;
                case -7:
                    Data = Data.OrderByDescending(x => x.DiplomaSeries).OrderByDescending(x => x.DiplomaNumber);
                    break;
                default:
                    break;
            }
        }

        //-----------------------------------------------------------------------------------------------------

        void Paging()
        {
            if (PagedData != null)
                return;

            Filter.Page = (Filter.Page.HasValue && Filter.Page > 0) ? Filter.Page : 1;
            PagedData = Data.ToPagedList((int)Filter.Page, 10);
            Filter.Page = PagedData.PageNumber;
        }

        public IPagedList<GVUZ.Data.Model.OlympicDiplomant> PagedData { get; set; }


        //-----------------------------------------------------------------------------------------------------

        OlympicDiplomantFilterViewModel filter;
        public OlympicDiplomantFilterViewModel Filter
        {
            get
            {
                if (filter == null) filter = new OlympicDiplomantFilterViewModel();
                return filter;
            }
            set
            {
                filter = value;
            }
        }

        //-----------------------------------------------------------------------------------------------------

        // Для комбобокса в фильтре - уникальные значения лет проведения олимпиад
        IEnumerable<SelectorItem> years;
        public IEnumerable<SelectorItem> Years
        {
            get
            {
                if (years == null)
                {
                    years = Repository.GetOlympicTypeAll().DistinctBy(p => p.OlympicYear).Select(s => new SelectorItem
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

        //-----------------------------------------------------------------------------------------------------

        // Для комбобокса в фильтре - уникальные наименования олимпиад
        IEnumerable<SelectorItem> names;
        public IEnumerable<SelectorItem> Names
        {
            get
            {
                if (names == null)
                {
                    names = Repository.GetOlympicsData().
                        Where(p => p.OrgOlympicEnterID == institutionID).
                        DistinctBy(p => p.OlympicType.Name).
                        Select(s => new SelectorItem
                        {
                            Id = s.OlympicType.OlympicID,
                            Name = s.OlympicType.Name
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

        //-----------------------------------------------------------------------------------------------------

        // Для комбобокса в фильтре - уникальные профили олимпиад
        IEnumerable<SelectorItem> profiles;
        public IEnumerable<SelectorItem> Profiles
        {
            get
            {
                if (profiles == null)
                {
                    profiles = Repository.GetOlympicsData().
                        Where(p => p.OrgOlympicEnterID == institutionID).
                        DistinctBy(p => p.OlympicProfile.ProfileName).
                        Select(s => new SelectorItem
                        {
                            Id = s.OlympicProfile.OlympicProfileID,
                            Name = s.OlympicProfile.ProfileName
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

        //-----------------------------------------------------------------------------------------------------

        IEnumerable<SelectorItem> resultLevels;
        public IEnumerable<SelectorItem> ResultLevels
        {
            get
            {
                if (resultLevels == null)
                {
                    resultLevels = Repository.GetOlympicDiplomTypeAll().
                        Where(p => p.OlympicDiplomTypeID < 3).
                        Select(s => new SelectorItem
                        {
                            Id = s.OlympicDiplomTypeID,
                            Name = s.Name
                        });

                    resultLevels = new List<SelectorItem>() { new SelectorItem
                    {
                        Id = 0,
                        Name = "Не выбрано"
                    }}.Concat(resultLevels);
                }
                return resultLevels;
            }
        }

        //-----------------------------------------------------------------------------------------------------

        IEnumerable<SelectorItem> status;
        public IEnumerable<SelectorItem> Status
        {
            get
            {
                if (status == null)
                {
                    status = Repository.GetOlympicDiplomantStatusAll().
                        Select(s => new SelectorItem
                        {
                            Id = s.OlympicDiplomantStatusID,
                            Name = s.Name
                        });

                    status = new List<SelectorItem>() { new SelectorItem
                    {
                        Id = 0,
                        Name = "Не выбрано"
                    }}.Concat(status);
                }
                return status;
            }
        }

        //-----------------------------------------------------------------------------------------------------

        IEnumerable<SelectorItem> endingDates;
        public IEnumerable<SelectorItem> EndingDates
        {
            get
            {
                if (endingDates == null)
                {
                    endingDates = Data.
                        DistinctBy(p => p.EndingDate).
                        Select(s => new SelectorItem
                        {
                            Id = s.EndingDate ?? 0,
                            Name = s.EndingDate.ToString()
                        });

                    endingDates = new List<SelectorItem>() { new SelectorItem
                    {
                        Id = 0,
                        Name = "Не выбрано"
                    }}.Concat(endingDates);
                }
                return endingDates;
            }
        }

        //-----------------------------------------------------------------------------------------------------

    }
}

