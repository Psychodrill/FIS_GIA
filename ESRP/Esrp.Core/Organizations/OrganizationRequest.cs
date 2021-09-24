namespace Esrp.Core.Organizations
{
    /// <summary>
    /// Описание организации, созданное регистирующимся пользователем
    /// </summary>
    /// <remarks>
	/// Заявка на регистрацию пользователя на доступ к системам для работы с организацией.
    /// </remarks>
    public class OrganizationRequest : Organization
    {
        int? OrganizationId_;

        /// <summary>
        /// Ссылка на организацию из справочника
        /// </summary>
        /// <remarks>Идентификатор организации. В БД - поле OrganizationRequest.OrganizationID.</remarks>
        public int? OrganizationId
        {
            get
            {
                if (this.OrganizationId_ <= 0)
                {
                    return null;
                }

                return this.OrganizationId_;
            }

            set
            {
                this.OrganizationId_ = value;
            }
        }

        internal OrganizationRequest():base()
        {
        }

        public OrganizationRequest(string fullName, int regionId, int typeId, int kindId, bool isPrivate, string INN_In, string OGRN_In, int ISId, string AnotherNameIs)
            : base(fullName, regionId, typeId, kindId, isPrivate, INN_In, OGRN_In, (int)OrganizationStatus.Operating, null, string.Empty, ISId, AnotherNameIs)
        {
        }

        public OrganizationRequest(int organizationId)
        {
        }
    }
}
