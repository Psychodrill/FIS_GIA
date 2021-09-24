using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using Fbs.Core.Organizations;

namespace Fbs.Web.Administration.SqlConstructor.Organizations
{
    public abstract class SqlConstructor_GetOrganizations : SqlConstructor_GetData
    {
        public SqlConstructor_GetOrganizations(NameValueCollection urlParameters)
            : base()
        {
            defaultOrderField = "OrgID";
            AllowedFieldNames.AddRange(new string[] { "OrgID", "FullName", "RegName", "RegID", "TypeId", "TypeName", "KindId", "KindName", "IsPrivate", "INN", "StatusId" });
            m_urlParameters = urlParameters;
        }

        protected override void CreateParameters(List<SqlParameter> parameters, List<string> whereExpr)
        {
             string SearchBy = GetVal_Str("RBSearchBy");

             if (SearchBy == "INN")
             {
                 string INN = GetVal_Str("INN");
                 if (INN.Length == 10)
                 {
                     whereExpr.Add("INN=@INN");
                     AddSqlParam_str(parameters, "INN", INN, 10);
                 }
             }
             else
             {
                 string FullName = GetVal_Str("OrgName");
                 if (FullName.Length > 0)
                 {
                     whereExpr.Add("FullName like '%'+@FullName+'%'");
                     AddSqlParam_str(parameters, "FullName", FullName, 256);
                 }

                 int OldTypeId = GetVal_int("OldTypeId");
                 if (OldTypeId > 0)
                 {
                     OrganizationDataAccessor.OPF OPF = OrganizationDataAccessor.GetIsPrivate(OldTypeId);
                     if (OPF != OrganizationDataAccessor.OPF.Undefinded)
                     {
                         whereExpr.Add("IsPrivate=@IsPrivate");
                         AddSqlParam_int(parameters, "IsPrivate", (int)OPF);
                     }

                     whereExpr.Add("TypeId=@TypeId");
                     AddSqlParam_int(parameters, "TypeId", OrganizationDataAccessor.GetNewTypeId(OldTypeId));
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
    }
}