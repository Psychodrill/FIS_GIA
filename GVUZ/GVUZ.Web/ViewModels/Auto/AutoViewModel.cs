using GVUZ.DAL.Dapper.ViewModel.AutoOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GVUZ.Web.ViewModels.Auto
{
    public class AutoViewModel
    {
        public AutoViewModel()
        {
            MenuElements = new List<MenuElement>();
            ReportParameters = new List<ReportParameter>();
            ReportFileFormats = new List<SelectListItem>();
        }

        public List<MenuElement> MenuElements { get; private set; }
        public List<ReportParameter> ReportParameters { get; private set; }
        public List<SelectListItem> ReportFileFormats { get; private set; }

        public string ReportCode { get; set; }
        public string ReportFileFormat { get; set; }
        public string ReportName { get; set; }
        public int InstitutionId { get; set; }

        public string ReportHTML { get; set; }

        public void MergeParameters(IEnumerable<ReportParameter> otherReportParameters)
        {
            foreach (ReportParameter parameter in ReportParameters)
            {
                ReportParameter otherParameter = otherReportParameters.FirstOrDefault(x => x.DBName == parameter.DBName);
                if (otherParameter != null)
                {
                    parameter.Value = otherParameter.Value;
                }
            }
        }

        public List<AutoOrderCheckBoxModel> CheckBoxes { get; set; }
    }
    

    public class MenuElement
    {
        public string Text { get; set; }
        public string ReportCode { get; set; }
    }

    public class ReportParameter
    {
        public ReportParameter()
        {
            ItemsForSelect = new List<SelectListItem>();
        }

        public string Text { get; set; }
        public string InputType { get; set; }
        public string DBName { get; set; }
        public string Value { get; set; }

        public bool ItemsForSelectLoaded { get; set; }
        public List<SelectListItem> ItemsForSelect { get; private set; }
    } 
}