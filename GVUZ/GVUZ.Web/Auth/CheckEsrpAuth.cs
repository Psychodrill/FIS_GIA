using System;
using System.Configuration;
using System.Reflection;
using log4net;

namespace GVUZ.Web.Auth
{
	/// <summary>
	/// Веб-сервис авторизации ЕСРП
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
	[System.Diagnostics.DebuggerStepThroughAttribute]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Web.Services.WebServiceBindingAttribute(Name = "CheckAuthSoap", Namespace = "urn:esrp:v1")]
	public class CheckEsrpAuth : System.Web.Services.Protocols.SoapHttpClientProtocol
	{
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		
		/// <summary>
		/// Получаем статус авторизации пользователя
		/// </summary>
		public static int CheckUserAuth(Guid userGuid, string userLogin)
		{
			string esrpPath = ConfigurationManager.AppSettings["ESRPAuth.Path"];
			if (!esrpPath.EndsWith("/")) esrpPath += "/";
			CheckEsrpAuth ca = new CheckEsrpAuth(esrpPath + "auth/CheckAuth.asmx");
			try
			{
                var guidCheckResult = ca.CheckUserTicket(userLogin, userGuid, 3);
				if (guidCheckResult.StatusID == 1)
				{
					if (guidCheckResult.Login == userLogin)
						return 1;
				}

				return 0;
			}
			catch (Exception ex)
			{
				Log.Error("ESRP Auth check", ex);
				return -1;
			}
		}

		/// <summary>
		/// Проверяем статус авторизации пользователя по логину и паролю
		/// </summary>
		public static int CheckUserAccess(string userLogin, string userPassword)
		{
			string esrpPath = ConfigurationManager.AppSettings["ESRPAuth.Path"];
			if (!esrpPath.EndsWith("/")) esrpPath += "/";
			CheckEsrpAuth ca = new CheckEsrpAuth(esrpPath + "auth/CheckAuth.asmx");
			try
			{
				var guidCheckResult = ca.CheckUserAccess(userLogin, userPassword, 3);
				if (guidCheckResult.StatusID == 1)
				{
					if (guidCheckResult.Login == userLogin)
						return 1;
				}

				return 0;
			}
			catch (Exception ex)
			{
				Log.Error("ESRP Auth check", ex);
				return -1;
			}
		}

		public CheckEsrpAuth(string url)
		{
			this.Url = url;
		}

        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:ersp:v1/CheckUserTicket", RequestNamespace = "urn:ersp:v1", ResponseNamespace = "urn:ersp:v1", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public UserCheckResult CheckUserTicket(string login, Guid receivedUID, int systemID)
		{
            object[] results = this.Invoke("CheckUserTicket", new object[]
			{
				login, receivedUID, systemID
			});
			return ((UserCheckResult)(results[0]));
		}

		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:ersp:v1/CheckUserAccess", RequestNamespace = "urn:ersp:v1", ResponseNamespace = "urn:ersp:v1", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		public UserCheckResult CheckUserAccess(string userLogin, string userPassword, int systemID)
		{
			object[] results = this.Invoke("CheckUserAccess", new object[] {
                        userLogin, userPassword, systemID});
			return ((UserCheckResult)(results[0]));
		}
	}

	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.225")]
	[SerializableAttribute]
	[System.Diagnostics.DebuggerStepThroughAttribute]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:ersp:v1")]
	public class UserCheckResult
	{
		private int statusIDField;

		private string loginField;

		public int StatusID
		{
			get
			{
				return this.statusIDField;
			}

			set
			{
				this.statusIDField = value;
			}
		}

		public string Login
		{
			get
			{
				return this.loginField;
			}

			set
			{
				this.loginField = value;
			}
		}
	}
}