namespace GVUZ.Helper
{
    public static class NameHelper
    {
        public static string GetFullName(string firstName, string lastName, string middleName = null)
        {
            return lastName + " " + firstName + (string.IsNullOrEmpty(middleName) ? "" : " " + middleName);
        }
    }
}