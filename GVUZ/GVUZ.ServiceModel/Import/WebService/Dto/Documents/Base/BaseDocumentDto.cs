using System.ComponentModel;
using System.Web.Script.Serialization;
using AutoMapper;
using GVUZ.Model.Entrants.ContextExtensions;
using GVUZ.Model.Entrants.Documents;

namespace GVUZ.ServiceModel.Import.WebService.Dto.Documents.Base
{
    [Description("Документ")]
    public abstract class BaseDocumentDto : BaseDto
    {
        public string DocumentDate;
        public string DocumentNumber;
        public string DocumentOrganization;
        public string OriginalReceived;
        public string OriginalReceivedDate;
        public abstract EntrantDocumentType EntrantDocumentType { get; }
    }

    public static class Extensions
    {
        public static string GetDocumentSpecificData(this BaseDocumentDto document)
        {
#warning Тормозящая сериализация!!!
            return new JavaScriptSerializer().Serialize(
                Mapper.Map(document,
                           EntrantDocumentExtensions.InstantiateDocumentByType((int) document.EntrantDocumentType),
                           document.GetType(),
                           EntrantDocumentExtensions.GetDocumentViewModelType((int) document.EntrantDocumentType)));
        }
    }
}