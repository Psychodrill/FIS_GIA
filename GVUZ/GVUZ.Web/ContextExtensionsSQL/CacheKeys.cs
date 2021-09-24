namespace GVUZ.Web.SQLDB
{
    public static class CacheKeys
    {
        public const string ViolationTypes = "FIS.Cache.ViolationTypes";
        public const string BenefitTypes = "FIS.Cache.BenefitTypes";
        public const string EducationForms = "FIS.Cache.EducationForms";
        public const string EducationFormsForOrders = "FIS.Cache.EducationFormsForOrders";
        public const string EducationLevels = "FIS.Cache.EducationLevels";
        public const string BudgetLevels = "FIS.Cache.BudgetLevels";
        private const string CampaignsTemplate = "FIS.Cache.Campaigns_{0}";
        private const string CompetitiveGroupsTemplate = "FIS.Cache.CompetitiveGroups_{0}";

        public const string ApplicationStatuses = "FIS.Cache.ApplicationStatuses";

        public const string EducationSources = "FIS.Cache.EducationSources";
        public const string EducationSourcesForOrders = "FIS.Cache.EducationSourcesForOrders";

        public const string OrderOfAdmissionStatuses = "FIS.Cache.OrderOfAdmissionStatuses";

        public static string Campaigns(int institutionId)
        {
            return string.Format(CampaignsTemplate, institutionId);
        }

        public static string CompetitiveGroups(int institutionId)
        {
            return string.Format(CompetitiveGroupsTemplate, institutionId);
        }
    }
}