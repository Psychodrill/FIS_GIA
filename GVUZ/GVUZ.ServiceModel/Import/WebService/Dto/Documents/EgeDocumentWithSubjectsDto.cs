using System;
using System.ComponentModel;
using System.Xml.Serialization;
using FogSoft.Helpers;
using GVUZ.Model.Entrants.Documents;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents.Base;

namespace GVUZ.ServiceModel.Import.WebService.Dto.Documents
{
    [Description("Свидетельство о результатах ЕГЭ")]
    public class EgeDocumentWithSubjectsDto : BaseDocumentDto
    {
        public string RawDocumentYear;

        [XmlArrayItem(ElementName = "SubjectData")] public SubjectDataDto[] Subjects;

        public override EntrantDocumentType EntrantDocumentType
        {
            get { return EntrantDocumentType.EgeDocument; }
        }

        /// <summary>
        ///     Проставляем год из переданной даты, если есть
        /// </summary>
        public string DocumentYear
        {
            get
            {
                return string.IsNullOrEmpty(DocumentDate)
                           ? DocumentDate
                           : DocumentDate.GetStringOrEmptyAsDate().GetValueOrDefault().Year.ToString();
            }
            set
            {
                RawDocumentYear = value;
                //если есть дата, ничего не трогаем
                if (!String.IsNullOrEmpty(DocumentDate))
                    return;
                //если нет даты, ставим по году
                if (String.IsNullOrEmpty(DocumentDate))
                {
                    int year = value.To(0);
                    if (year < 1 || year > 3000) value = null;
                    RawDocumentYear = null;
                    DocumentDate = string.IsNullOrEmpty(value) ? value : new DateTime(year, 1, 1).GetDateAsString();
                }
            }
        }
    }
}