namespace Ege.Check.App.Web.Blanks.Esrp
{
    public interface IEsrpUrlCreator
    {
        string Login(string returnUrl);
        string Logout(string returnUrl);
    }
}