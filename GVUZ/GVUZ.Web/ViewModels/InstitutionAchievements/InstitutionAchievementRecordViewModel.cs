namespace GVUZ.Web.ViewModels.InstitutionAchievements
{
    public class InstitutionAchievementRecordViewModel
    {
        public int Id { get; set; }
        public string UID { get; set; }
        public string Name { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal? MaxValue { get; set; }
    }
}