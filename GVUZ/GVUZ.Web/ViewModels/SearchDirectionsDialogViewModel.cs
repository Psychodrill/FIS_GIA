using GVUZ.DAL.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace GVUZ.Web.ViewModels
{
    public class SearchDirectionsDialogViewModel
    {
        private List<SimpleDto> _educationLevels;
        private List<SimpleDto> _ugs;

        public List<SimpleDto> EducationLevels
        {
            get { return _educationLevels ?? (_educationLevels = new List<SimpleDto>()); }
            set { _educationLevels = value; }
        }

        public List<SimpleDto> Ugs
        {
            get { return _ugs ?? (_ugs = new List<SimpleDto>()); }
            set { _ugs = value; }
        }

        [DisplayName("Уровень образования")]
        public int EducationLevelId { get; set; }

        [DisplayName("Укрупнённая группа специальностей")]
        public int UgsId { get; set; }

        public int? Year { get; set; }

        public int[] TempDirectionsId { get; set; }

        public int SearchType { get; set; }

        public InstitutionDirectionSearchCommand GetSearchCommand()
        {
            return new InstitutionDirectionSearchCommand
            {
                SearchType = (InstitutionDirectionSearchType)SearchType,
                EducationLevelId = EducationLevelId,
                TempDirectionsId = TempDirectionsId,
                UgsId = UgsId,
                Year = Year
            };
        }
    }
}