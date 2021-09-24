using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fbs.Core.CatalogElements;

namespace Fbs.Core.Organizations
{

    /// <summary>
    /// Описание организации, созданное регистирующимся пользователем
    /// </summary>
    public    class OrganizationRequest : Organization
    {
        int? OrganizationId_;
        /// <summary>
        /// Ссылка на организацию из справочника
        /// </summary>
        public int? OrganizationId
        {
            get
            {
                if (OrganizationId_ <= 0)
                    return null;
                return OrganizationId_; }
            set { OrganizationId_ = value; }
        }

        internal OrganizationRequest():base()
        {
        }

        public OrganizationRequest(string fullName, int regionId, int typeId, int kindId, bool isPrivate, string INN_In, string OGRN_In)
            : base(fullName, regionId, typeId, kindId, isPrivate, INN_In, OGRN_In, 1)
        {

        }

        public OrganizationRequest(int organizationId)
        {
        }
    }
}
