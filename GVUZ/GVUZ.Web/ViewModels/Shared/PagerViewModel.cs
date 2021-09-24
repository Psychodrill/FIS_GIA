using System;
using GVUZ.Web.Infrastructure;
using GVUZ.DAL.Dto;
using System.Web;

namespace GVUZ.Web.ViewModels.Shared
{
    public class PagerViewModel : IPagination, IPageable
    {
        public const int MinPageSize = 10;
        public const int MaxPageSize = 500;

        private int _pageSize = MinPageSize;
        private int _currentPage = 1;

        public const string cook = "_pageSize";

        public int PageSize
        {
            get {

                var cookie = HttpContext.Current.Session[cook];
                if (cookie != null)
                    _pageSize = Convert.ToInt32(cookie);

                return _pageSize;
            }
            set
            {
                if (value < MinPageSize)
                {
                    _pageSize = MinPageSize;
                }
                else if (value > MaxPageSize)
                {
                    _pageSize = MaxPageSize;
                }
                else
                {
                    _pageSize = value;
                }


                HttpContext.Current.Session[cook] = _pageSize;
            }
        }

        public int CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value < 1 ? 1 : value; }
        }

        public int TotalRecords { get; set; }

        public int TotalPages
        {
            get { return (int) Math.Ceiling((double) TotalRecords/ _pageSize); }
        }

        public int FirstRecordOffset
        {
            get { return ((CurrentPage - 1)* _pageSize) + 1; }
        }

        public int LastRecordOffset
        {
            get { return FirstRecordOffset + _pageSize - 1; }
        }

        public int PageSizeInternal
        {
            set
            {
                _pageSize = value;
            }
        }


    }
}