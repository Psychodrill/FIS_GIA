using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Package
{
    /// <summary>
    ///     Информация об одном заявлении отправляемом на проваерку в ФБС
    /// </summary>
    public class CheckApp : ApplicationShortRef
    {
        public int StatusID { get; set; }
    }
}