using System.Reflection;
using log4net;

namespace GVUZ.Web.Auth
{
	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:ersp:v1")]
	public class UserDetails
	{

		private string emailField;

		private string firstNameField;

		private UserGroup[] groupsField;

		private string lastNameField;

		private string loginField;

		private System.Nullable<int> organizationIDField;

		private string patronymicNameField;

		private string phoneField;

		private string statusField;

		private int userIDField;

		/// <remarks/>
		public string Email
		{
			get
			{
				return this.emailField;
			}
			set
			{
				this.emailField = value;
			}
		}

		/// <remarks/>
		public string FirstName
		{
			get
			{
				return this.firstNameField;
			}
			set
			{
				this.firstNameField = value;
			}
		}

		/// <remarks/>
		public UserGroup[] Groups
		{
			get
			{
				return this.groupsField;
			}
			set
			{
				this.groupsField = value;
			}
		}

		/// <remarks/>
		public string LastName
		{
			get
			{
				return this.lastNameField;
			}
			set
			{
				this.lastNameField = value;
			}
		}

		/// <remarks/>
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

		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
		public System.Nullable<int> OrganizationID
		{
			get
			{
				return this.organizationIDField;
			}
			set
			{
				this.organizationIDField = value;
			}
		}

		/// <remarks/>
		public string PatronymicName
		{
			get
			{
				return this.patronymicNameField;
			}
			set
			{
				this.patronymicNameField = value;
			}
		}

		/// <remarks/>
		public string Phone
		{
			get
			{
				return this.phoneField;
			}
			set
			{
				this.phoneField = value;
			}
		}

		/// <remarks/>
		public string Status
		{
			get
			{
				return this.statusField;
			}
			set
			{
				this.statusField = value;
			}
		}

		/// <remarks/>
		public int UserID
		{
			get
			{
				return this.userIDField;
			}
			set
			{
				this.userIDField = value;
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:ersp:v1")]
	public class OrganizationType
	{

		private int idField;

		private string nameField;

		/// <remarks/>
		public int ID
		{
			get
			{
				return this.idField;
			}
			set
			{
				this.idField = value;
			}
		}

		/// <remarks/>
		public string Name
		{
			get
			{
				return this.nameField;
			}
			set
			{
				this.nameField = value;
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:ersp:v1")]
	public  class RegionData
	{

		private string codeField;

		private int idField;

		private string nameField;

		/// <remarks/>
		public string Code
		{
			get
			{
				return this.codeField;
			}
			set
			{
				this.codeField = value;
			}
		}

		/// <remarks/>
		public int ID
		{
			get
			{
				return this.idField;
			}
			set
			{
				this.idField = value;
			}
		}

		/// <remarks/>
		public string Name
		{
			get
			{
				return this.nameField;
			}
			set
			{
				this.nameField = value;
			}
		}
	}

	/// <remarks/>
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganizationDataExtended))]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:ersp:v1")]
	public class OrganizationData
	{

		private int idField;

		private string fullNameField;

		private string shortNameField;

		private int regionIdField;

		private int typeIdField;

		private int kindIdField;

		private string iNNField;

		private string oGRNField;

		private string ownerDepartmentField;

		private bool isPrivateField;

		private bool isfilialField;

		private string directorPositionField;

		private string directorFullNameField;

		private bool isAccreditedField;

		private string accreditationCertificateField;

		private string lawAddressField;

		private string factAddressField;

		private string phoneCityCodeField;

		private string phoneField;

		private string faxField;

		private string eMailField;

		private string siteField;

		private int mainIDField;

		private int departmentIDField;

		private RegionData regionField;

		private OrganizationType typeField;

		/// <remarks/>
		public int ID
		{
			get
			{
				return this.idField;
			}
			set
			{
				this.idField = value;
			}
		}

		/// <remarks/>
		public string FullName
		{
			get
			{
				return this.fullNameField;
			}
			set
			{
				this.fullNameField = value;
			}
		}

		/// <remarks/>
		public string ShortName
		{
			get
			{
				return this.shortNameField;
			}
			set
			{
				this.shortNameField = value;
			}
		}

		/// <remarks/>
		public int RegionId
		{
			get
			{
				return this.regionIdField;
			}
			set
			{
				this.regionIdField = value;
			}
		}

		/// <remarks/>
		public int TypeId
		{
			get
			{
				return this.typeIdField;
			}
			set
			{
				this.typeIdField = value;
			}
		}

		/// <remarks/>
		public int KindId
		{
			get
			{
				return this.kindIdField;
			}
			set
			{
				this.kindIdField = value;
			}
		}

		/// <remarks/>
		public string INN
		{
			get
			{
				return this.iNNField;
			}
			set
			{
				this.iNNField = value;
			}
		}

		/// <remarks/>
		public string OGRN
		{
			get
			{
				return this.oGRNField;
			}
			set
			{
				this.oGRNField = value;
			}
		}

		/// <remarks/>
		public string OwnerDepartment
		{
			get
			{
				return this.ownerDepartmentField;
			}
			set
			{
				this.ownerDepartmentField = value;
			}
		}

		/// <remarks/>
		public bool IsPrivate
		{
			get
			{
				return this.isPrivateField;
			}
			set
			{
				this.isPrivateField = value;
			}
		}

		/// <remarks/>
		public bool Isfilial
		{
			get
			{
				return this.isfilialField;
			}
			set
			{
				this.isfilialField = value;
			}
		}

		/// <remarks/>
		public string DirectorPosition
		{
			get
			{
				return this.directorPositionField;
			}
			set
			{
				this.directorPositionField = value;
			}
		}

		/// <remarks/>
		public string DirectorFullName
		{
			get
			{
				return this.directorFullNameField;
			}
			set
			{
				this.directorFullNameField = value;
			}
		}

		/// <remarks/>
		public bool IsAccredited
		{
			get
			{
				return this.isAccreditedField;
			}
			set
			{
				this.isAccreditedField = value;
			}
		}

		/// <remarks/>
		public string AccreditationCertificate
		{
			get
			{
				return this.accreditationCertificateField;
			}
			set
			{
				this.accreditationCertificateField = value;
			}
		}

		/// <remarks/>
		public string LawAddress
		{
			get
			{
				return this.lawAddressField;
			}
			set
			{
				this.lawAddressField = value;
			}
		}

		/// <remarks/>
		public string FactAddress
		{
			get
			{
				return this.factAddressField;
			}
			set
			{
				this.factAddressField = value;
			}
		}

		/// <remarks/>
		public string PhoneCityCode
		{
			get
			{
				return this.phoneCityCodeField;
			}
			set
			{
				this.phoneCityCodeField = value;
			}
		}

		/// <remarks/>
		public string Phone
		{
			get
			{
				return this.phoneField;
			}
			set
			{
				this.phoneField = value;
			}
		}

		/// <remarks/>
		public string Fax
		{
			get
			{
				return this.faxField;
			}
			set
			{
				this.faxField = value;
			}
		}

		/// <remarks/>
		public string EMail
		{
			get
			{
				return this.eMailField;
			}
			set
			{
				this.eMailField = value;
			}
		}

		/// <remarks/>
		public string Site
		{
			get
			{
				return this.siteField;
			}
			set
			{
				this.siteField = value;
			}
		}

		/// <remarks/>
		public int MainID
		{
			get
			{
				return this.mainIDField;
			}
			set
			{
				this.mainIDField = value;
			}
		}

		/// <remarks/>
		public int DepartmentID
		{
			get
			{
				return this.departmentIDField;
			}
			set
			{
				this.departmentIDField = value;
			}
		}

		/// <remarks/>
		public RegionData Region
		{
			get
			{
				return this.regionField;
			}
			set
			{
				this.regionField = value;
			}
		}

		/// <remarks/>
		public OrganizationType Type
		{
			get
			{
				return this.typeField;
			}
			set
			{
				this.typeField = value;
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:ersp:v1")]
	public  class OrganizationDataExtended : OrganizationData
	{

		private string requestedLoginField;

		/// <remarks/>
		public string RequestedLogin
		{
			get
			{
				return this.requestedLoginField;
			}
			set
			{
				this.requestedLoginField = value;
			}
		}
	}

	/// <remarks/>
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ActualizationDataExtended))]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:ersp:v1")]
	public class ActualizationData
	{

		private bool shouldRenewUserField;

		private bool shouldRenewOrganizationField;

		/// <remarks/>
		public bool ShouldRenewUser
		{
			get
			{
				return this.shouldRenewUserField;
			}
			set
			{
				this.shouldRenewUserField = value;
			}
		}

		/// <remarks/>
		public bool ShouldRenewOrganization
		{
			get
			{
				return this.shouldRenewOrganizationField;
			}
			set
			{
				this.shouldRenewOrganizationField = value;
			}
		}
	}


	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:ersp:v1")]
	public  class ActualizationDataExtended : ActualizationData
	{

		private bool shouldRenewOrganizationsField;

		private bool shouldRenewFounderField;

		private bool shouldRenewMainOrgField;

		/// <remarks/>
		public bool ShouldRenewOrganizations
		{
			get
			{
				return this.shouldRenewOrganizationsField;
			}
			set
			{
				this.shouldRenewOrganizationsField = value;
			}
		}

		/// <remarks/>
		public bool ShouldRenewFounder
		{
			get
			{
				return this.shouldRenewFounderField;
			}
			set
			{
				this.shouldRenewFounderField = value;
			}
		}

		/// <remarks/>
		public bool ShouldRenewMainOrg
		{
			get
			{
				return this.shouldRenewMainOrgField;
			}
			set
			{
				this.shouldRenewMainOrgField = value;
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:ersp:v1")]
	public class UserGroup
	{

		private string codeField;

		private int idField;

		private string nameField;

		/// <remarks/>
		public string Code
		{
			get
			{
				return this.codeField;
			}
			set
			{
				this.codeField = value;
			}
		}

		/// <remarks/>
		public int ID
		{
			get
			{
				return this.idField;
			}
			set
			{
				this.idField = value;
			}
		}

		/// <remarks/>
		public string Name
		{
			get
			{
				return this.nameField;
			}
			set
			{
				this.nameField = value;
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
//	[System.Diagnostics.DebuggerStepThroughAttribute]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Web.Services.WebServiceBindingAttribute(Name = "GetDataSoap", Namespace = "urn:ersp:v1")]
	public class GetEsrpUserData : System.Web.Services.Protocols.SoapHttpClientProtocol
	{
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		/// <remarks/>
		public GetEsrpUserData(string url)
		{
			this.Url = url;
		}

		/// <remarks/>
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:ersp:v1/GetUserDetails", RequestNamespace = "urn:ersp:v1", ResponseNamespace = "urn:ersp:v1", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		public UserDetails GetUserDetails(string userLogin, int systemID)
		{
			object[] results = this.Invoke("GetUserDetails", new object[] {
                        userLogin,
                        systemID});
			return ((UserDetails)(results[0]));
		}


		/// <remarks/>
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:ersp:v1/GetActualizationData", RequestNamespace = "urn:ersp:v1", ResponseNamespace = "urn:ersp:v1", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		public ActualizationData GetActualizationData(string userLogin, System.DateTime lastChangeUser, System.DateTime lastChangeOrganization)
		{
			object[] results = this.Invoke("GetActualizationData", new object[] {
                        userLogin,
                        lastChangeUser,
                        lastChangeOrganization});
			return ((ActualizationData)(results[0]));
		}

		/// <remarks/>
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:ersp:v1/FindOrganizations", RequestNamespace = "urn:ersp:v1", ResponseNamespace = "urn:ersp:v1", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		public OrganizationData[] FindOrganizations(string searchBy, string Inn, string organizerName, string organizerType, string region, int pageNumber, out int total)
		{
			object[] results = this.Invoke("FindOrganizations", new object[] {
                        searchBy,
                        Inn,
                        organizerName,
                        organizerType,
                        region,
                        pageNumber});
			total = ((int)(results[1]));
			return ((OrganizationData[])(results[0]));
		}

		/// <remarks/>
		[System.Web.Services.WebMethodAttribute(MessageName = "GetActualizationData1")]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:ersp:v1/GetActualizationDataExtended", RequestElementName = "GetActualizationDataExtended", RequestNamespace = "urn:ersp:v1", ResponseElementName = "GetActualizationDataExtendedResponse", ResponseNamespace = "urn:ersp:v1", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("GetActualizationDataExtendedResult")]
		public ActualizationDataExtended GetActualizationData(string userLogin, System.DateTime lastChangeUser, System.DateTime lastChangeOrganization, int numberOfOrganizations, System.DateTime lastChangeFounder, System.DateTime lastChangeMainOrg)
		{
			object[] results = this.Invoke("GetActualizationData1", new object[] {
                        userLogin,
                        lastChangeUser,
                        lastChangeOrganization,
                        numberOfOrganizations,
                        lastChangeFounder,
                        lastChangeMainOrg});
			return ((ActualizationDataExtended)(results[0]));
		}


		/// <remarks/>
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:ersp:v1/GetOrganizationDataExtended", RequestElementName = "GetOrganizationDataExtended", RequestNamespace = "urn:ersp:v1", ResponseElementName = "GetOrganizationDataExtendedResponse", ResponseNamespace = "urn:ersp:v1", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlArrayAttribute("GetOrganizationDataExtendedResult")]
		public OrganizationData[] GetOrganizationData(int orgId, int updateType)
		{
			object[] results = this.Invoke("GetOrganizationData", new object[] {
                        orgId,
                        updateType});
			return ((OrganizationData[])(results[0]));
		}

		/// <remarks/>
		[System.Web.Services.WebMethodAttribute(MessageName = "GetOrganizationData1")]
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:ersp:v1/GetOrganizationData", RequestElementName = "GetOrganizationData", RequestNamespace = "urn:ersp:v1", ResponseElementName = "GetOrganizationDataResponse", ResponseNamespace = "urn:ersp:v1", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlArrayAttribute("GetOrganizationDataResult")]
		public OrganizationDataExtended[] GetOrganizationData(string[] userLogins)
		{
			object[] results = this.Invoke("GetOrganizationData1", new object[] {
                        userLogins});
			return ((OrganizationDataExtended[])(results[0]));
		}


		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:ersp:v1/UpdateUserStatus", RequestNamespace = "urn:ersp:v1", ResponseNamespace = "urn:ersp:v1", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		public string UpdateUserStatus(string userLogin, string newStatus)
		{
			object[] results = this.Invoke("UpdateUserStatus", new object[] {
                        userLogin, newStatus});
			return ((string)(results[0]));
		}

        /// <summary>
        /// Получение из ЕСРП статуса организации по её идентифиактору
        /// </summary>
        /// <param name="orgId">Идентифиактор организации</param>
        /// <returns>Текущий статус (один из набора (1, 2, 3) или -1 если получить статус не удалось)</returns>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:ersp:v1/GetOrgStatus", RequestNamespace = "urn:ersp:v1", ResponseNamespace = "urn:ersp:v1", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int GetOrgStatus(int orgId)
        {
            object[] results = this.Invoke("GetOrgStatus", new object[] { orgId });

            return ((int)(results[0]));
        }
    }

}