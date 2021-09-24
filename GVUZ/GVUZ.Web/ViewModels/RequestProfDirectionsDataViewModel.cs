using GVUZ.DAL.Dapper.Model.AllowedDirections;
using GVUZ.DAL.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GVUZ.Web.ViewModels
{
    public class RequestProfDirectionsDataViewModel
    {
        private static readonly SelectListItem[] _yearList;

        static RequestProfDirectionsDataViewModel()
        {
            int startYear = 2016, yearRange = 1;

            //if (!int.TryParse(ConfigurationManager.AppSettings["CampaignYearRangeStart"], out startYear))
            //{
            //    startYear = DateTime.Now.Year;
            //    yearRange = 1;
            //}
            //else
            //{
            //    if (!int.TryParse(ConfigurationManager.AppSettings["CampaignYearRangeLength"], out yearRange))
            //    {
            //        yearRange = 1;
            //    }
            //}

            _yearList = Enumerable.Range(startYear, yearRange).Select(y => new SelectListItem { Value = y.ToString(), Text = y.ToString() }).ToArray();
        }

        private List<RequestDirectionItemViewModel> _addedItems;

        private List<RequestDirectionItemViewModel> _deniedItems;

        public RequestProfDirectionsDataViewModel()
        {
            Year = DateTime.Now.Year;
        }

        public RequestProfDirectionsDataViewModel(IEnumerable<InstitutionDirectionRequestDto> items) : this()
        {
            _addedItems = items.Where(x => x.RequestType == InstitutionDirectionRequestType.AddProfDirection && !x.IsDenied).Select(x => new RequestDirectionItemViewModel(x)).ToList();
            _deniedItems = items.Where(x => x.RequestType == InstitutionDirectionRequestType.AddProfDirection && x.IsDenied).Select(x => new RequestDirectionItemViewModel(x)).ToList();
        }

        [DisplayName("Год")]
        public int? Year { get; set; }

        public IEnumerable<SelectListItem> YearList
        {
            get { return _yearList; }
        }
        /// <summary>
        /// Направления в заявках на добавление в список с профильными ВИ
        /// </summary>
        public List<RequestDirectionItemViewModel> AddedItems
        {
            get { return _addedItems ?? (_addedItems = new List<RequestDirectionItemViewModel>()); }
            set { _addedItems = value; }
        }

        /// <summary>
        /// Направления в отклоненных заявках
        /// </summary>
        public List<RequestDirectionItemViewModel> DeniedItems
        {
            get { return _deniedItems ?? (_deniedItems = new List<RequestDirectionItemViewModel>()); }
            set { _deniedItems = value; }
        }

        public IEnumerable<SubmitDirectionRequestDto> GetSubmits()
        {
            return AddedItems.Select(x => x.GetDto(InstitutionDirectionRequestType.AddProfDirection));
        }
    }
}