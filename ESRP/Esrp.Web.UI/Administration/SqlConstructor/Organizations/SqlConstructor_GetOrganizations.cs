namespace Esrp.Web.Administration.SqlConstructor.Organizations
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data.SqlClient;

    using Esrp.Core.Organizations;

    /// <summary>
    /// The sql constructor_ get organizations.
    /// </summary>
    public abstract class SqlConstructor_GetOrganizations : SqlConstructor_GetData
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlConstructor_GetOrganizations"/> class.
        /// </summary>
        /// <param name="urlParameters">
        /// The url parameters.
        /// </param>
        public SqlConstructor_GetOrganizations(NameValueCollection urlParameters)
        {
            this.defaultOrderField = "OrgID";
            this.AllowedFieldNames.AddRange(
                new[]
                    {
                        "OrgID", "FullName", "RegName", "RegID", "TypeId", "TypeName", "KindId", "KindName", "IsPrivate", 
                        "INN"
                    });
            this.m_urlParameters = urlParameters;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The create parameters.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="whereExpr">
        /// The where expr.
        /// </param>
        protected override void CreateParameters(List<SqlParameter> parameters, List<string> whereExpr)
        {
            var searchBy = this.GetVal_Str("RBSearchBy");
            if (searchBy == "INN")
            {
                var inn = this.GetVal_Str("INN");
                if (inn.Length == 10)
                {
                    whereExpr.Add("INN=@INN");
                    AddSqlParam_str(parameters, "INN", inn, 10);
                }
            }
            else
            {
                var fullName = this.GetVal_Str("OrgName");
                if (fullName.Length > 0)
                {
                    whereExpr.Add("FullName like '%'+@FullName+'%'");
                    AddSqlParam_str(parameters, "FullName", fullName, 256);
                }

                var oldTypeId = this.GetVal_int("OldTypeId");
                if (oldTypeId > 0)
                {
                    var opf = OrganizationDataAccessor.GetIsPrivate(oldTypeId);
                    if (opf != OrganizationDataAccessor.OPF.Undefinded)
                    {
                        whereExpr.Add("IsPrivate=@IsPrivate");
                        AddSqlParam_int(parameters, "IsPrivate", (int)opf);
                    }

                    whereExpr.Add("TypeId=@TypeId");
                    AddSqlParam_int(parameters, "TypeId", OrganizationDataAccessor.GetNewTypeId(oldTypeId));
                }

                var typeIds = this.GetVal_Str("TypeId");
                if (typeIds.Length > 0)
                {
                    whereExpr.Add("(TypeId IN (SELECT [nam] FROM ufn_ut_SplitFromString(@TypeId,',')))");
                    AddSqlParam_str(parameters, "TypeId", typeIds, typeIds.Length);
                }

                var regionIds = this.GetVal_Str("RegID");
                if (regionIds.Length > 0)
                {
                    whereExpr.Add("(RegId IN (SELECT [nam] FROM ufn_ut_SplitFromString(@RegionID,',')))");
                    AddSqlParam_str(parameters, "RegionID", regionIds, regionIds.Length);
                }

                
            }
        }

        #endregion
    }
}