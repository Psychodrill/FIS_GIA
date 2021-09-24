using GVUZ.Web.ViewModels.Shared;

namespace GVUZ.Web.ViewModels.OrderOfAdmission
{
    public class ApplicationOrderListViewModel : PagedListViewModelBase<ApplicationOrderRecordViewModel>
    {
        public ApplicationOrderFilterViewModel Filter { get; set; }

        public ApplicationOrderRecordViewModel RecordInfo { get { return null; } }

        public int TotalApplicationOrderCount { get; set; }

        public int OrderId { get; set; }

        //Режим просмотра (запрет редактирования всей информации)
        public bool IsReadOnly { get; set; }

        //Отображение приказа о зачислении в приказе об отказе от зачисления
        public bool ShowOrderOfAdmissionInfo { get; set; }

        //Возможность редактировать поля IsDisagreed и IsDisagreedDate (для приказа о зачислении)
        public bool AllowEditDisagreedInfo { get; set; }

        //Возможность отказаться от зачисления (для приказа о зачислении)
        public bool AllowRefuseAdmission { get; set; }
    }
}