using System;
using System.ComponentModel;

namespace GVUZ.Web.ViewModels.OlympicDiplomant
{
    public class OlympicDiplomantFilterViewModel
    {
        public OlympicDiplomantFilterViewModel()
        {
        }

        [DisplayName("Страница")]
        public int? Page { get; set; }

        [DisplayName("Год олимпиады")]
        public int Year { get; set; }

        [DisplayName("Наименование олимпиады")]
        public int Name { get; set; }

        [DisplayName("Профиль олимпиады")]
        public int Profile { get; set; }

        [DisplayName("Сортировка")]
        public int Sort { get; set; }

        [DisplayName("Имя")]
        public string FirstName { get; set; }

        [DisplayName("Фамилия")]
        public string LastName { get; set; }

        [DisplayName("Отчество")]
        public string MiddleName { get; set; }

        [DisplayName("Серия документа")]
        public string DocumentSeries { get; set; }

        [DisplayName("Номер документа")]
        public string DocumentNumber { get; set; }

        [DisplayName("Серия диплома")]
        public string DiplomaSeries { get; set; }

        [DisplayName("Номер диплома")]
        public string DiplomaNumber { get; set; }


        [DisplayName("Степень диплома")]
        public int ResultLevelID { get; set; }
        [DisplayName("Статус поиска")]
        public int StatusID { get; set; }
        [DisplayName("Год окончания ОО")]
        public int EndingDate { get; set; } 

        public bool HasValue { get; set; }

        public bool Filtered
        {
            get
            {
                return
                    HasValue ||
                    (Year > 0) || (Name > 0) || (Profile > 0) ||
                    (ResultLevelID > 0) || (StatusID > 0) || (EndingDate > 0) ||
                    (!String.IsNullOrEmpty(FirstName)) || (!String.IsNullOrEmpty(LastName)) || (!String.IsNullOrEmpty(MiddleName)) ||
                    (!String.IsNullOrEmpty(DocumentSeries)) || (!String.IsNullOrEmpty(DocumentNumber)) ||
                    (!String.IsNullOrEmpty(DiplomaSeries)) || (!String.IsNullOrEmpty(DiplomaNumber));
            }
        }


        public void CopyFilter(OlympicDiplomantFilterViewModel copyFrom)
        {
            if (copyFrom == null)
                throw new ArgumentNullException("copyFrom");

            this.Year = copyFrom.Year;
            this.Name = copyFrom.Name;
            this.Profile = copyFrom.Profile;
            this.Sort = copyFrom.Sort;
            this.FirstName = copyFrom.FirstName;
            this.LastName = copyFrom.LastName;
            this.MiddleName = copyFrom.MiddleName;
            this.DocumentSeries = copyFrom.DocumentSeries;
            this.DocumentNumber = copyFrom.DocumentNumber;
            this.DiplomaSeries = copyFrom.DiplomaSeries;
            this.DiplomaNumber = copyFrom.DiplomaNumber;
            this.ResultLevelID = copyFrom.ResultLevelID;
            this.StatusID = copyFrom.StatusID;
            this.EndingDate = copyFrom.EndingDate;
        }
    }
}

