namespace Ege.Hsc.Logic.Models.Servers
{
    using System.ComponentModel;

    public enum BlankServerErrorType
    {
        [Description("����� ����������� �� ������� ����")]
        MissingOnServer,
        [Description("����� ����������� � ��")]
        MissingInDb,
    }
}
