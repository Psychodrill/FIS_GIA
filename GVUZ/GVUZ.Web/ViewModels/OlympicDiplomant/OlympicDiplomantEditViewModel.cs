using GVUZ.Data.Helpers;
using GVUZ.Data.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GVUZ.Web.ViewModels.OlympicDiplomant
{
    public class OlympicDiplomantEditViewModel : OlympicDiplomantBaseViewModel
    {
        //-----------------------------------------------------------------------------------------------------
        public GVUZ.Data.Model.OlympicDiplomant Data { get; set; }
        IEnumerable<OlympicTypeProfile> olympics;

        public OlympicDiplomantEditViewModel() : base()
        {
            OlympicTypeID = 0;
            OlympicYearID = 0;
        }

        int? institutionID;
        public OlympicDiplomantEditViewModel(long id, int? institutionID) : this()
        {
            this.institutionID = institutionID;

            if (id > 0)
                Data = Repository.GetOlympicDiplomantById(id);
            else
            {
                Data = new GVUZ.Data.Model.OlympicDiplomant();
                Data.RVIPersons = new RVIPersons();
            }

            olympics = Repository.GetOlympicsData().Where(p => p.OrgOlympicEnterID == institutionID);

            var olympicTypeProfileId = Data.OlympicTypeProfileID;
            if (olympicTypeProfileId > 0)
            {
                var profile = Repository.GetOlympicTypeProfileById(olympicTypeProfileId);
                OlympicTypeID = profile.OlympicTypeID;
                OlympicYearID = profile.OlympicType.OlympicYear;
            }

        }

        //-----------------------------------------------------------------------------------------------------

        public void Save()
        {
            if (Data.OlympicDiplomantID > 0)
                Repository.UpdateOlympicDiplomant(Data);
            else
                Repository.InsertOlympicDiplomant(Data);
        }

        //-----------------------------------------------------------------------------------------------------

        IEnumerable<SelectorItem> regionType;
        public IEnumerable<SelectorItem> RegionType
        {
            get
            {
                if (regionType == null)
                {
                    regionType = Repository.GetRegionTypeAll().Select(s => new SelectorItem
                    {
                        Id = s.RegionId,
                        Name = s.Name
                    });
                    regionType = new List<SelectorItem>(){ new SelectorItem
                    {
                        Id = 0,
                        Name = "Не выбрано"
                    }}.Concat(regionType);
                }
                return regionType;
            }
        }


        //-----------------------------------------------------------------------------------------------------

        IEnumerable<SelectorItem> formNumber;
        int[] forms = new int[] { 7, 8, 9, 10, 11 };
        public IEnumerable<SelectorItem> FormNumber
        {
            get
            {
                if (formNumber == null)
                {
                    formNumber = forms.Select(s => new SelectorItem
                    {
                        Id = s,
                        Name = s.ToString() + " класс"
                    });
                    formNumber = new List<SelectorItem>(){ new SelectorItem
                    {
                        Id = 0,
                        Name = "Не выбрано"
                    }}.Concat(formNumber);
                }
                return formNumber;
            }
        }


        //-----------------------------------------------------------------------------------------------------

        IEnumerable<SelectorItem> resultLevel;
        public IEnumerable<SelectorItem> ResultLevel
        {
            get
            {
                if (resultLevel == null)
                {
                    resultLevel = Repository.GetOlympicDiplomTypeAll().
                        Where(s => s.OlympicDiplomTypeID < 3).
                        Select(s => new SelectorItem
                        {
                            Id = s.OlympicDiplomTypeID,
                            Name = s.Name
                        });

                    resultLevel = new List<SelectorItem>(){ new SelectorItem
                    {
                        Id = 0,
                        Name = "Не выбрано"
                    }}.Concat(resultLevel);
                }
                return resultLevel;
            }
        }


        //-----------------------------------------------------------------------------------------------------

        IEnumerable<SelectorItem> olympicProfile;
        public IEnumerable<SelectorItem> OlympicProfile
        {
            get
            {
                if (olympicProfile == null)
                {
                    olympicProfile = olympics.Select(s => new SelectorItem
                    {
                        Id = s.OlympicTypeProfileID,
                        Name = s.OlympicProfile.ProfileName
                    });
                    olympicProfile = new List<SelectorItem>(){ new SelectorItem
                    {
                        Id = 0,
                        Name = "Не выбрано"
                    }}.Concat(olympicProfile);
                }
                return olympicProfile;
            }
        }


        //-----------------------------------------------------------------------------------------------------

        [Display(Name = "Год проведения")]
        public int? OlympicYearID { get; set; }
        IEnumerable<SelectorItem> olympicYear;
        public IEnumerable<SelectorItem> OlympicYear
        {
            get
            {
                if (olympicYear == null)
                {
                    olympicYear = olympics.
                        DistinctBy(p => p.OlympicType.OlympicYear).
                        OrderBy(p => p.OlympicType.OlympicYear).
                        Select(s => new SelectorItem
                        {
                            Id = s.OlympicType.OlympicYear,
                            Name = s.OlympicType.OlympicYear.ToString()
                        });
                    olympicYear = new List<SelectorItem>(){ new SelectorItem
                    {
                        Id = 0,
                        Name = "Не выбрано"
                    }}.Concat(olympicYear);
                }
                return olympicYear;
            }
        }

        //-----------------------------------------------------------------------------------------------------

        [Display(Name = "Наименование")]
        public int? OlympicTypeID { get; set; }
        IEnumerable<SelectorItem> olympicType;
        public IEnumerable<SelectorItem> OlympicType
        {
            get
            {
                if (olympicType == null)
                {
                    olympicType = olympics.DistinctBy(p => p.OlympicTypeID).OrderBy(p => p.OlympicType.Name).
                        Select(s => new SelectorItem
                        {
                            Id = s.OlympicTypeID,
                            Name = s.OlympicType.Name
                        });
                    olympicType = new List<SelectorItem>(){ new SelectorItem
                    {
                        Id = 0,
                        Name = "Не выбрано"
                    }}.Concat(olympicType);
                }
                return olympicType;
            }
        }


        //-----------------------------------------------------------------------------------------------------

    }
}

