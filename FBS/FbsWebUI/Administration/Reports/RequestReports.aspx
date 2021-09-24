<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RequestReports.aspx.cs"
    Inherits="Fbs.Web.Administration.Reports.RequestReports" MasterPageFile="~/Common/Templates/Administration.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <form id="Form1" runat="server">
    <div runat="server" id="DSelectReports">
        <asp:ValidationSummary runat="server" ID="VResult" DisplayMode="BulletList" EnableClientScript="false"
            HeaderText="<p>Произошли следующие ошибки:</p>" />
        <hr />
        <span>Периодические отчеты<br />
            <br />
        </span>
        <asp:CheckBoxList runat="server" ID="ChReports">
            <asp:ListItem Text=" Отчет о загрузках свидетельств" Value="ReportCertificateLoadTVF"></asp:ListItem>
            <asp:ListItem Text=" Отчет об уникальных проверках свидетельств в разрезе регионов"
                Value="ReportCheckStatisticsTVF"></asp:ListItem>
            <asp:ListItem Text=" Отчет о наиболее активных организациях" Value="ReportTopCheckingOrganizationsTVF"></asp:ListItem>
            <asp:ListItem Text=" Отчет о пользователях, превысивших лимит неправильных проверок"
                Value="ReportPotentialAbusersTVF"></asp:ListItem>
            <asp:ListItem Text=" Сводный отчет по типам запросов и уникальным проверкам" Value="ReportChecksByPeriodTVF"></asp:ListItem>
            <asp:ListItem Text=" Отчет о регистрации пользователей"
                Value="ReportUserStatusWithAccredTVF"></asp:ListItem>
                <asp:ListItem Text=" Отчет о регистрации организаций"
                Value="ReportOrgsStatusWithAccredTVF"></asp:ListItem>
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
        <asp:LinkButton  ID="LBCheckedCNEsDetailedTVF" runat="server" Text="Детальный отчет по свидетельствам, поданным в различные ОУ за все время"
            CommandArgument="ReportCheckedCNEsDetailedTVF" OnClick="LBOrgsInfo_Click"></asp:LinkButton>(Доступен
        только для скачивания)<br />
        <asp:LinkButton  ID="LBCheckedCNEsAggregatedTVF" runat="server" Text="Сводный отчет по свидетельствам, поданным в различные ОУ за все время"
            CommandArgument="ReportCheckedCNEsAggregatedTVF" OnClick="LBOrgsInfo_Click"></asp:LinkButton><br />
        <asp:LinkButton  ID="LBReportCheckedCNEsAggregatedTVF_WithOrgType" runat="server" Text="Сводный отчет по свидетельствам, поданным в различные ОУ за все время (с детализаций по типам ОУ)"
            CommandArgument="ReportCheckedCNEsAggregatedTVF_WithOrgType" OnClick="LBOrgsInfo_Click"></asp:LinkButton>(Доступен
        только для скачивания)<br />
        <asp:LinkButton  ID="LBCheckedCNEsTVF" runat="server" Text="Отчет по свидетельствам, поданным в различные ОУ за все время"
            CommandArgument="ReportCheckedCNEsTVF" OnClick="LBOrgsInfo_Click"></asp:LinkButton><br />
        <asp:LinkButton ID="LBChecksByOrgsTVF" runat="server" Text="Отчет о проверках свидетельств в разрезе ОУ и регионов за все время"
            CommandArgument="ReportChecksByOrgsTVF" OnClick="LBOrgsInfo_Click"></asp:LinkButton><br />
        <asp:LinkButton ID="LBEditedOrgsTVF" runat="server" Text="Отчет об измененных или созданных ОУ за все время"
            CommandArgument="ReportEditedOrgsTVF" OnClick="LBOrgsInfo_Click"></asp:LinkButton><br />
              <asp:LinkButton ID="LBNotRegistredOrgs" runat="server" Text="Отчет о незарегистрированных ОУ за все время"
            CommandArgument="ReportNotRegistredOrgsTVF" OnClick="LBOrgsInfo_Click"></asp:LinkButton><br />
             <asp:LinkButton ID="LBRegistredAggregOrgs" runat="server" Text="Сводный отчет о незарегистрированных ОУ за все время"
            CommandArgument="ReportRegistrationShortTVF" OnClick="LBOrgsInfo_Click"></asp:LinkButton><br />
        <br />
        <br />
        <asp:CheckBoxList runat="server" ID="ChReportsWithoutPeriod">
            <asp:ListItem Text=" Отчет по свидетельствам, поданным в различные ОУ за все время"
                Value="ReportCheckedCNEsTVF"></asp:ListItem>
            <asp:ListItem  Text=" Сводный отчет по свидетельствам, поданным в различные ОУ за все время"
                Value="ReportCheckedCNEsAggregatedTVF"></asp:ListItem>
            <asp:ListItem Text=" Отчет о проверках свидетельств в разрезе ОУ и регионов за все время"
                Value="ReportChecksByOrgsTVF"></asp:ListItem>
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
