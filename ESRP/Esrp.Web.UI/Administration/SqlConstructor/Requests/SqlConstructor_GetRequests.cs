namespace Esrp.Web.Administration.SqlConstructor.Organizations
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data.SqlClient;
using System;
    using Esrp.Core.DataAccess;
    using System.Data;

    /// <summary>
    /// The sql constructor_ get requests.
    /// </summary>
    public class SqlConstructor_GetRequests : SqlConstructor_GetData
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlConstructor_GetRequests"/> class.
        /// </summary>
        /// <param name="urlParameters">
        /// The url parameters.
        /// </param>
        public SqlConstructor_GetRequests(NameValueCollection urlParameters)
        {
            this.defaultOrderField = "Number";
            this.AllowedFieldNames.AddRange(
                new[] { "Number", "FullName", "CreateDate", "StatusName", "OperatorWhoSetComment", "HasComment" });
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
            var statusIds = this.GetVal_Str("StatusID");
            if (statusIds.Length > 0)
            {
                whereExpr.Add("(StatusID IN (SELECT [nam] FROM ufn_ut_SplitFromString(@StatusID,',')))");
                AddSqlParam_str(parameters, "StatusID", statusIds, statusIds.Length);
            }

            var years = this.GetVal_Str("YearInRequests");
            if (years.Length > 0)
            {
                whereExpr.Add("(year(CreateDate) in (SELECT [nam] FROM ufn_ut_SplitFromString(@YearInRequests,',')))");
                AddSqlParam_str(parameters, "YearInRequests", years, years.Length);
            }
        }

        
        /// <summary>
        /// The get main sql.
        /// </summary>
        /// <returns>
        /// The get main sql.
        /// </returns>
        protected override string getMainSQL()
        {
            return
                @"SELECT  or1.ID Number,
                    or1.FullName,
                    or1.CreateDate,
                    or1.StatusID,
                    as1.[Name] StatusName,
                    or1.OrganizationId,
                    ISNULL(acc.LastName, '') OperatorWhoSetComment,
                    CASE WHEN LEN(ISNULL(orol.Comments, '')) = 0 THEN 0
                         ELSE 1
                    END HasComment
                    FROM OrganizationRequest2010 or1
                    JOIN AccountStatus as1 ON as1.StatusID = or1.StatusID
                    LEFT JOIN OrganizationRequestOperatorLog orol ON or1.id = orol.OrganizationRequestID
                    LEFT JOIN Account acc ON orol.OperatorID = acc.Id";
        }

        #endregion
    }
}