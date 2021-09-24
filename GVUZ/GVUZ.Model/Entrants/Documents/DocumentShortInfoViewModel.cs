using System;
using System.ComponentModel;
using System.Globalization;
using System.Web.Script.Serialization;
using FogSoft.Helpers;

namespace GVUZ.Model.Entrants.Documents
{
    public class DocumentShortInfoViewModel
    {
        public DocumentShortInfoViewModel()
        {
            CanBeModified = false;
        }

        public DocumentShortInfoViewModel(BaseDocumentViewModel model)
        {
            EntrantDocumentID = model.EntrantDocumentID;
            DocumentTypeName = model.DocumentTypeName;
            DocumentSeriesNumber = model.DocumentSeries + " " + model.DocumentNumber;
            DocumentDate = model.DocumentDate == null
                               ? ""
                               : model.DocumentDate.Value.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
            DocumentOrganization = model.DocumentOrganization;
            DocumentAttachmentID = model.DocumentAttachmentID;
            DocumentAttachmentName = model.DocumentAttachmentName;
            if (model is CustomDocumentViewModel)
            {
                DocumentTypeName += " (" + ((CustomDocumentViewModel) model).DocumentTypeNameText + ")";
            }
        }

        public int EntrantDocumentID { get; set; }

        [DisplayName("Тип документа")]
        public string DocumentTypeName { get; set; }

        [DisplayName("Серия и номер документа")]
        public string DocumentSeriesNumber { get; set; }

        [DisplayName("Дата выдачи")]
        public string DocumentDate { get; set; }

        public DateTime? DocDate { get; set; }
        public string DocNumber { get; set; }
        public string DocSeries { get; set; }
        public int DocTypeID { get; set; }
        public string DocSpecificData { get; set; }

        [DisplayName("Кем выдан")]
        public string DocumentOrganization { get; set; }

        [DisplayName("Ссылка на документ")]
        public Guid DocumentAttachmentID { get; set; }

        public string DocumentAttachmentName { get; set; }

        public bool CanBeModified { get; set; }
        public bool ShowWarnBeforeModifying { get; set; }
        public bool CanBeDetached { get; set; }


        [DisplayName("Дата предоставления")]
        public string OriginalReceivedDate { get; set; }

        [DisplayName("Оригиналы/заверенные копии предоставлены / Заявление с обязательством предоставления оригинала в течение первого учебного года/ЕПГУ")]
        public bool OriginalReceived { get; set; }

        public bool CanNotSetReceived { get; set; }

        public int MaxFileSize
        {
            get { return AppSettings.Get("MaxPostFileLength", 10000); }
        }

        public void FillData() {
            DocumentSeriesNumber = DocSeries + " " + DocNumber;
            //для свидетельства ЕГЭ только год
            DocumentDate = DocDate == null ? "" : DocDate.Value.ToString(DocTypeID != 2 ? "dd.MM.yyyy" : "yyyy", CultureInfo.InvariantCulture);
            if (DocTypeID == 15)
            {
                var model =
                    new JavaScriptSerializer().Deserialize<CustomDocumentViewModel>(DocSpecificData);
                DocumentTypeName += " (" + model.DocumentTypeNameText + ")";
            }
        }
    }
}