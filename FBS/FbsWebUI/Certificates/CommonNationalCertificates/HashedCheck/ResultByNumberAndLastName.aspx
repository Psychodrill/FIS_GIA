<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResultByNumberAndLastName.aspx.cs"
    Inherits="Fbs.Web.Certificates.CommonNationalCertificates.HashedCheck.ResultByNumberAndLastName"
    MasterPageFile="~/Common/Templates/HashedCertificates.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphCertificateHead" runat="server">

    <script type="text/javascript" src="/Common/Scripts/Utils.js"></script>

    <script type="text/javascript" src="/Common/Scripts/SessVars.js"></script>

    <script type="text/javascript" src="/Common/Scripts/Notice.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphCertificateContent" runat="server">
    <form id="Form1" runat="server">
    <asp:Panel runat="server" ID="pNotExist" Visible="false">
        <div class="notice" id="CheckResultNoticeAbsentee">
            <div class="top">
                <div class="l">
                </div>
                <div class="r">
                </div>
                <div class="m">
                </div>
            </div>
            <div class="cont">
                <div dir="ltr" class="hide" title="Свернуть" onclick="ToggleNoticeState(this);">
                    x<span></span></div>
                <div class="txt-in">
                    <p>
                        За дополнительной информацией о причине отсутствия свидетельства обращайтесь в Региональный
                        Центр Обработки Информации, находящийся на территории региона, в котором было выдано
                        данное свидетельство.</p>
                </div>
            </div>
            <div class="bottom">
                <div class="l">
                </div>
                <div class="r">
                </div>
                <div class="m">
                </div>
            </div>
        </div>
        <p align="center">
            <img src="/Common/Images/spacer.gif" width="32" height="31" style="background-image: url('/Common/Images/important.gif')" />
            Свидетельство с заданными параметрами не найдено.</p>

        <script type="text/javascript">
                    InitNotice();
        </script>

    </asp:Panel>
    <table id="TResults" runat="server" style="width: 100%;" class="form" visible="true">
        <tr>
            <td style="width: 20%;">
                Номер свидетельства
            </td>
            <td>
                <asp:Label runat="server" ID="LNumber" EnableViewState="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 20%;">
                Типографский номер
            </td>
            <td>
                <asp:Label runat="server" ID="LTypographicNumber" EnableViewState="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 20%;">
                Регион
            </td>
            <td>
                <asp:Label runat="server" ID="LRegion" EnableViewState="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 20%;">
                Год выдачи свидетельства
            </td>
            <td>
                <asp:Label runat="server" ID="LYear" EnableViewState="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 20%;">
                Статус свидетельства
            </td>
            <td>
                <asp:Label runat="server" ID="LStatus" EnableViewState="false"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:DataGrid runat="server" ID="DGSubjects" AutoGenerateColumns="false" EnableViewState="false"
        ShowHeader="True" GridLines="None" CssClass="table-th" Width="60%">
        <HeaderStyle CssClass="th" />
        <Columns>
            <asp:TemplateColumn>
                <HeaderStyle Width="25%" CssClass="left-th" />
                <HeaderTemplate>
                    <div>
                        Предмет</div>
                </HeaderTemplate>
                <ItemTemplate>
                    <%# HighlightValue(Eval("SubjectName"), 
                    Convert.ToBoolean(Eval("SubjectMarkCorrect")) || string.IsNullOrEmpty(Convert.ToString(Eval("CheckSubjectMark"))))%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle Width="15%" />
                <HeaderTemplate>
                    <div>
                        Заявлено</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center" />
                <ItemTemplate>
                    <%# HighlightValue(Eval("CheckSubjectMark"), Eval("SubjectMarkCorrect"))%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle Width="15%" />
                <HeaderTemplate>
                    <div>
                        В&nbsp;базе</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center" />
                <ItemTemplate>
                    <%# HighlightValue(Eval("SubjectMark")) %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle Width="10%" CssClass="right-th" />
                <HeaderTemplate>
                    <div>
                        Апелляция&nbsp;</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center" />
                <ItemTemplate>
                    <%# (Convert.IsDBNull(Eval("HasAppeal")) ? string.Empty : (!Convert.IsDBNull(Eval("HasAppeal")) && Convert.ToBoolean(Eval("HasAppeal"))) ? "<span style=\"color:Red\">Да</span>" : "Нет") %>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
    </form>
</asp:Content>

<script runat="server">

    private static string HighlightValue(object valueObj)
    {
        string value = Convert.ToString(valueObj);
        if (value.StartsWith("!") || value.StartsWith("Истек"))
            return String.Format("<font color=\"red\">{0}</font>", value);
        return value;
    }

    private static string HighlightValue(object valueObj, object isCorrectObj)
    {
        string value = Convert.ToString(valueObj);
        if (!Convert.IsDBNull(isCorrectObj) && Convert.ToBoolean(isCorrectObj))
            return value;
        return String.Format("<font color=\"red\">{0}</font>", value);
    }

    private static string HighlightValues(object valueObj, object checkValueObj, object isCorrectObj)
    {
        string value = Convert.ToString(valueObj);
        string checkValue = Convert.ToString(checkValueObj);
        if ((!Convert.IsDBNull(isCorrectObj) && Convert.ToBoolean(isCorrectObj)) ||
                string.IsNullOrEmpty(checkValue))
            return value;

        if (Convert.IsDBNull(valueObj) && Convert.IsDBNull(checkValueObj))
            return string.Empty;

        checkValue = String.IsNullOrEmpty(checkValue) ? "не&nbsp;задано" : checkValue;
        value = String.IsNullOrEmpty(value) ? "не&nbsp;найдено" : value;

        return String.Format("<span title=\"Ошибка: заявленое {0} (в базе {1})\"><span style=\"color:Red\">{0}</span> ({1})</span>",
            checkValue, value);
    }

    private static string FormLinkToNewCertificate(string comment, string newCertificate, string lastName)
    {
        if (string.IsNullOrEmpty(newCertificate))
            return comment;
        return string.Format("<a href=\"/Certificates/CommonNationalCertificates/CheckResult.aspx?number={1}&LastName={2}\">{0}</a>", comment, newCertificate, lastName);
    }    
    
</script>

