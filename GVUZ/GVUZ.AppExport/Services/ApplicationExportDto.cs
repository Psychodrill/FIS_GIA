using System;
using System.Collections.Generic;

namespace GVUZ.AppExport.Services
{
    public class ApplicationExportDto
    {
        public long AppId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime? LastDenyDate { get; set; }
        public int StatusId { get; set; }
        private ICollection<ApplicationExportFinSourceDto> _finSourceAndEduForms;

        public ICollection<ApplicationExportFinSourceDto> FinSourceAndEduForms
        {
            get { return _finSourceAndEduForms ?? (_finSourceAndEduForms = new List<ApplicationExportFinSourceDto>()); }
            set { _finSourceAndEduForms = value; }
        }
    }
}