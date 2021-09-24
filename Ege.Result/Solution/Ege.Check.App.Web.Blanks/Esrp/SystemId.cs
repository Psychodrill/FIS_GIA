namespace Ege.Check.App.Web.Blanks.Esrp
{
    /// <summary>
    ///     Идентификатор системы
    /// </summary>
    public enum SystemId
    {
        Esrp = 1,
        Fbs = 2,
        Fbd = 3,
        StatReports = 4
    }

    public static class BlanksSystemId
    {
        public static SystemId SystemId = SystemId.Fbs;

        public static int IntSystemId { get { return (int)SystemId; } }
    }
}