using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel;
using GVUZ.Model.Entrants.Documents;

namespace GVUZ.Web.ViewModels
{
    public class IndividualAchivementsViewModel
    {
        public int ApplicationID { get; set; }
        public int EntrantID { get; set; }

        public IndividualAchivementViewModel[] Items { get; set; }

        /// <summary>
        /// Для отображения меток. Всегда Null
        /// </summary>
        public IndividualAchivementViewModel FakedAchivement { get { return null; } }

        public class IndividualAchivementViewModel
        {
            public int IAID { get; set; }

            [StringLength(50)]
            [DisplayName("UID")]
            public string UID { get; set; }

            [StringLength(100)]
            [DisplayName("Наименование индивидуального достижения")]
            public string IAName { get; set; }

            [DisplayName("Дополнительный балл")]
            public decimal? IAMark { get; set; }

            public string IAMarkString { get; set; }

            public CustomDocumentViewModel IADocument { get; set; }
            

            public int? IADocumentID { get; set; } // это свойство нужно для передачи ID подтверждающего документа при создании ИД.

            [DisplayName("Сведения о подтверждающем документе")]
            public string IADocumentDisplay
            {
                get
                {
                    return string.Format("{0} № {1}{2} от {3:dd.MM.yyyy}", IADocument.DocumentTypeNameText, IADocument.DocumentSeries, IADocument.DocumentNumber, IADocument.DocumentDate);
                }
            }

            [DisplayName("Преимущественное право")]
            public string isAdvantageRight { get; set; }
        }
    }
}