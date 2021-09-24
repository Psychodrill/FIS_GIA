using System;
using System.Collections.Generic;
using FogSoft.Helpers;
using GVUZ.Helper.ExternalValidation;
using GVUZ.Model.Entrants;
using GVUZ.Model.Entrants.ContextExtensions;
using GVUZ.Model.Entrants.Documents;

namespace GVUZ.Model.Applications
{
	public static class ApplicationPassportValidator
	{
		/// <summary>
		/// 	Возвращает пустой список, если проверка паспортных данных прошла успешно или строки с ошибками.
		/// </summary>
		internal static IEnumerable<string> ValidatePassport(this EntrantsEntities entities, Application application)
		{
			if (!application.Entrant.IdentityDocumentID.HasValue)
				return new[] { Messages.ApplicationValidator_NoPassport };

			int identityDocumentId = application.Entrant.IdentityDocumentID.Value;

			IdentityDocumentViewModel document;
			try
			{
				document = (IdentityDocumentViewModel)entities.LoadEntrantDocument(identityDocumentId);
                if (document == null)
                    return new[] { Messages.ApplicationValidator_IdentityDocumentIsNotPassport };
			}
			catch (InvalidCastException ex) //ДУЛ не ДУЛ. Какая-то странность
			{
                LogHelper.Log.Error(ex.Message, ex);
				return new[] { Messages.ApplicationValidator_IdentityDocumentIsNotPassport };
			}

			var isSeriesRequired = IdentityDocumentViewModel.IsSeriesRequired(document.IdentityDocumentTypeID);
			return new PassportValidator().Validate(document.DocumentSeries, 
                document.DocumentNumber, document.DocumentDate, document.SubdivisionCode, isSeriesRequired);
		}
	}
}