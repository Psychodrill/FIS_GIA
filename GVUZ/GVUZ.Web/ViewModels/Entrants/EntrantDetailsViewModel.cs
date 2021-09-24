using System.Collections.Generic;
using System.ComponentModel;

namespace GVUZ.Web.ViewModels.Entrants
{
    public class EntrantDetailsViewModel
    {
        public static readonly EntrantDetailsViewModel MetadataInstance = new EntrantDetailsViewModel();

        private List<EntrantDetailsApplicationViewModel> _applicationList;
        public int EntrantId { get; set; }

        public string EntrantName { get; set; }
        
        [DisplayName("Вид документа")]
        public string IdentityDocumentType { get; set; }

        [DisplayName("Серия и номер")]
        public string IdentityDocumentNumber { get; set; }

        [DisplayName("Дата рождения")]
        public string DateOfBirth { get; set; }

        [DisplayName("Место рождения")]
        public string PlaceOfBirth { get; set; }

        [DisplayName("Пол")]
        public string Gender { get; set; }

        [DisplayName("Идентификатор в БД ОО (UID)")]
        public string UID { get; set; }

        public List<EntrantDetailsApplicationViewModel> ApplicationList
        {
            get { return _applicationList ?? (_applicationList = new List<EntrantDetailsApplicationViewModel>()); }
            set { _applicationList = value; }
        }

        public EntrantDetailsApplicationViewModel RecordInfo = null;
    }
}