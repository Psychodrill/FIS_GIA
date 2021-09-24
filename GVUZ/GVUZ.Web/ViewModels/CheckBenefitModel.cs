using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GVUZ.Web.ViewModels
{
    public class CheckBenefitModel
    {
        public CheckBenefitModel()
        {
            CheckBenefitOlympicModel = new CheckBenefitOlympicModel();
        }

        public int? CompetitiveGroupId { get; set; }
        public int? EntranceTestItemId { get; set; }
        public int? EntrantDocumentId { get; set; }
        public int? DocumentTypeId { get; set; }

        public CheckBenefitOlympicModel CheckBenefitOlympicModel { get;   set; }
    }

    public class CheckBenefitOlympicModel
    {
        public int? OlympicTypeProfileId { get; set; }
        public int? DiplomaTypeId { get; set; }
        public int? OlympicId { get; set; }
        public int? ClassNumber { get; set; }
    }
}