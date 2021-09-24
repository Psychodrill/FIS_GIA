<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RequestReports.aspx.cs"
    Inherits="Esrp.Web.Administration.Reports.RequestReports" MasterPageFile="~/Common/Templates/Administration.Master" %>
<%@ Register TagPrefix="esrp" Namespace="Esrp.Web.Controls" Assembly="Esrp.Web.UI" %>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphThirdLevelMenu">
    <div class="third_line">
        <div class="max_width">
            <esrp:TopMenu ID="SecondLevelMenu1" runat="server" RootResourceKey="report"
                HeaderTemplate="<ul>" FooterTemplate="</ul>" />
            <div class="clear">
            </div>
        </div>
    </div>
    <!--bottom_line-->
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <form id="Form1" runat="server">
    <div runat="server" id="DSelectReports">
        <asp:ValidationSummary CssClass="error_block"  runat="server" ID="VResult" DisplayMode="BulletList" EnableClientScript="false"
            HeaderText="<p>Произошли следующие ошибки:</p>" />
        <hr />
        <span>Периодические отчеты<br />
            <br />
        </span>
        <asp:CheckBoxList runat="server" ID="ChReports">
            <asp:ListItem Text=" Отчет о наиболее активных организациях" Value="ReportTopCheckingOrganizationsTVF"></asp:ListItem>
            <asp:ListItem Text=" Отчет о регистрации пользователей" Value="ReportUserStatusWithAccredTVF"></asp:ListItem>
            <asp:ListItem Text=" Отчет о регистрации организаций" Value="ReportOrgsStatusWithAccredTVF"></asp:ListItem>
        </asp:CheckBoxList>
        <asp:RadioButtonList runat="server" ID="RBPeriods">
            <asp:ListItem Text=" за 24 часа" Value="1" Selected="True"></asp:ListItem>
            <asp:ListItem Text=" за неделю" Value="7"></asp:ListItem>
        </asp:RadioButtonList>
        <hr />
        <span>Отчеты за все время эксплуатации<br />
            <br />
        </span>
        <asp:LinkButton ID="LBOrgsInfo" runat="server" Text="Общий отчет по организациям за все время"
            CommandArgument="ReportOrgsInfoTVF" OnClick="LBOrgsInfo_Click"></asp:LinkButton>(Доступен
        только для скачивания)<br />
        <asp:LinkButton ID="LBEditedOrgsTVF" runat="server" Text="Отчет об измененных или созданных ОУ за все время"
            CommandArgument="ReportEditedOrgsTVF" OnClick="LBOrgsInfo_Click"></asp:LinkButton><br />
        <asp:LinkButton ID="LBNotRegistredOrgs" runat="server" Text="Отчет о незарегистрированных ОУ за все время"
            CommandArgument="ReportNotRegistredOrgsTVF" OnClick="LBOrgsInfo_Click"></asp:LinkButton><br />
        <asp:LinkButton ID="LBRegistredAggregOrgs" runat="server" Text="Сводный отчет о незарегистрированных ОУ за все время"
            CommandArgument="ReportRegistrationShortTVF" OnClick="LBOrgsInfo_Click"></asp:LinkButton><br />
        <br />
        <br />
        <asp:CheckBoxList runat="server" ID="ChReportsWithoutPeriod">
            <asp:ListItem Text=" Отчет об измененных или созданных ОУ за все время" Value="ReportEditedOrgsTVF"></asp:ListItem>
            <asp:ListItem Text=" Отчет о незарегистрированных ОУ за все время" Value="ReportNotRegistredOrgsTVF"></asp:ListItem>
            <asp:ListItem Text=" Сводный отчет о незарегистрированных ОУ за все время" Value="ReportRegistrationShortTVF"></asp:ListItem>
        </asp:CheckBoxList>
        <hr />
        <span>Отчеты по регионам<br />
            <br />
        </span>
        <asp:CheckBoxList runat="server" ID="ChReportsByRegion">
            <asp:ListItem Text=" Общий отчет по организациям за все время (по региону)" Value="ReportOrgsInfoByRegionTVF"></asp:ListItem>
        </asp:CheckBoxList>
        <br />
        <asp:DropDownList runat="server" ID="DDLRegions" DataValueField="Code" DataTextField="Name">
        </asp:DropDownList>
        <asp:CustomValidator runat="server" ID="VReqReports" ErrorMessage="Не выбран ни один отчет"
            Display="None" OnServerValidate="VReqReports_ServerValidate"></asp:CustomValidator>
        <br />
        <br />
        <hr />
        <br />
        EMail:&nbsp;<asp:TextBox runat="server" ID="TBEMail"></asp:TextBox>
        <asp:RequiredFieldValidator runat="server" ID="VReqEMail" ErrorMessage="Поле 'EMail' обязательно для заполнения"
            EnableClientScript="false" Display="None" ControlToValidate="TBEMail"></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator runat="server" ID="VRegEMail" ControlToValidate="TBEMail"
            EnableClientScript="false" Display="None" ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$"
            ErrorMessage="Поле 'EMail' заполнено неверно" />
        <br />
        <br />
        <asp:Button runat="server" ID="BtSend" Text="Отправить отчет" OnClick="BtSend_Click" />
    </div>
    <asp:Literal runat="server" ID="LResult"></asp:Literal>
    </form>
</asp:Content>
