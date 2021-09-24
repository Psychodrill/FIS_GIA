namespace Esrp.Core.Organizations
{
    using System.Collections.Generic;

    using Esrp.Core.Users;

    public class OrgBrief
	{
		public string OrgFullName;
		public string OrgShortName;
		public string FounderName;
		public string LawAddress;
		public string FactAddress;
		public string DirectorFullName;
		public string DirectorPosition;
		public string OrgTypeName;
		public string OrgKindName;
		public string RegionName;
		public string Phone;
		public string PhoneCityCode;
		public string EMail;
		public string Site;
		public string Fax;
		public int OrganizationId;
		public bool IsPrivate;
		public bool IsFilial;
		public string INN;
		public string OGRN;
		public string AccreditationSertificate;

        /// <summary>
        /// Прием по результатам ЕГЭ (для ССУЗов и ВУЗов)
        /// </summary>
	    public int? ReceptionOnResultsCNE;

        public int? OrgTypeId;

        /// <summary>
        /// КПП
        /// </summary>
        public string KPP;


		// активные пользователи организации
		public List<OrgUserBrief> ActivatedUsers = new List<OrgUserBrief>();
	}
}
