using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using Fbs.Core.Organizations;

namespace Fbs.Web.Administration.SqlConstructor.Organizations
{
    public abstract class SqlConstructor_GetSubordOrgs : SqlConstructor_GetData
    {
        public SqlConstructor_GetSubordOrgs(NameValueCollection urlParameters)
            : base()
        {
            defaultOrderField = "Id";
            AllowedFieldNames.AddRange(new string[] { "Id", "FullName", "RegionId", "RegionName",
                "AccreditationSertificate", "DirectorFullName", "CountUser", "UserUpdateDate",
                "CountUniqueChecks",});
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



                int TypeId = GetVal_int("TypeId");
                if (TypeId > 0)
                {
                    whereExpr.Add("(TypeId=@TypeId)");
                    AddSqlParam_int(parameters, "TypeId", TypeId);
                }



                int RegID = GetVal_int("RegionId");
                if (RegID > 0)
                {
                    whereExpr.Add("(RegionId=@RegionID or RegionId is null)");
                    AddSqlParam_int(parameters, "RegionId", RegID);
                }
                else if (RegID == -1)
                {
                    whereExpr.Add("RegionId is null");
                }
            }
        }
    }
}