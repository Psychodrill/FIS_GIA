using System.Web.Services;
using AutoMapper;
using GVUZ.Helper.ExternalValidation;
using GVUZ.Model.Applications;
using GVUZ.Web.Helpers;

namespace GVUZ.Web
{
	/// <summary>
	/// Сервис, предоставляющий информацию, в т.ч. о результатах ЕГЭ.
	/// </summary>
	[WebService(Namespace = "urn:fbd:v1")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.ComponentModel.ToolboxItem(false)]
	public class InformationService : WebService
	{
		[WebMethod]
		public EgeResultAndStatus GetEgeInformation(string AppId, EgeParameters AppXml)
		{
			if (AppXml == null)
				return new EgeResultAndStatus(EgeResult.CreateError("Отсутствуют параметры для проверки"), EgeResultAndStatus.TransferError);

			return ApplicationValidatorExtensions.GetEgeInformation(Mapper.Map<EgeParameters, EgeQuery>(AppXml), AppXml.CheckCode);
		}
	}
}
