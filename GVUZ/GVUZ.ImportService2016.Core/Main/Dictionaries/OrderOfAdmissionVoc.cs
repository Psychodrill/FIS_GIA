using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{
    public class OrderOfAdmissionVoc : VocabularyBase<OrderOfAdmissionVocDto>
    {
        public OrderOfAdmissionVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class OrderOfAdmissionVocDto : VocabularyBaseDto
    {
        public static readonly int OrderStatusNoApplications = 1;
        public static readonly int OrderStatusHasApplications = 2;
        public static readonly int OrderStatusPublished = 3;
        public static readonly int OrderStatusDeleted = 4;

        public static readonly int OrderTypeAdmission = 1;
        public static readonly int OrderTypeException = 2;

        public int OrderID { get; set; }
        public override int ID
        {
            get { return OrderID; }
            set { OrderID = value; }
        }

        public int CampaignID { get; set; }

        public string ApplicationNumber { get; set; }
        public DateTime ApplicationRegistrationDate { get; set; }

        //public int OrderStatus { get; set; }
        public DateTime DatePublished { get; set; }

        //public int DirectionID { get; set; }
        //public int Course { get; set; }
        public int EducationLevelID { get; set; }
        public int EducationFormID { get; set; }
        public int EducationSourceID { get; set; }
        public int Stage { get; set; }
        //public bool IsForBeneficiary { get; set; }
        //public bool IsForeigner { get; set; }

        public string OrderName { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }

        public int OrderOfAdmissionStatusID { get; set; }
        public int OrderOfAdmissionTypeID { get; set; }
        public int HasApplications { get; set; }
    }
}
