<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Create.aspx.cs" 
    Inherits="Fbs.Web.Administration.Accounts.Users.Create" 
    MasterPageFile="~/Common/Templates/Administration.Master" %>
<%@ Import Namespace="Fbs.Core" %>
<%@ Import Namespace="Fbs.Web" %>

<%--
    Права доступа к странице определяются в web.config секцией: 
    <location path="Administration/Accounts/Users/Create.aspx">
--%>

<asp:Content runat="server" ContentPlaceHolderID="cphHead">
    <script src="/Common/Scripts/Confirmation.js" type="text/javascript"></script>
    <!-- глючный скрипт <script src="/Common/Scripts/SameUserAccounts.js" type="text/javascript"></script> -->
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">
<form runat="server">

    <asp:ValidationSummary runat="server" DisplayMode="BulletList" EnableClientScript="false"
        HeaderText="<p>Произошли следующие ошибки:</p>"/>

    <table class="form">
        <tr>
            <td class="left">Полное&nbsp;наименование&nbsp;организации</td>
            <td id="organizationNameParent"><asp:TextBox runat="server" ID="txtOrganizationName" 
                CssClass="txt long" autocomplete="off"
                    TextMode="MultiLine" Rows="5" Height="100px" MaxLength="1000" Enabled="false"/></td>
            <td> <asp:Button ID="btnChangeOrg" runat="server" PostBackUrl="/SelectOrg.aspx?BackUrl=./Administration/Accounts/Users/Create.aspx"
                    Text="Выбрать" Width="100px" ToolTip="Выбор организации" /></td>
        </tr>

        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtOrganizationName" 
            EnableClientScript="false" Display="None"
            ErrorMessage='Поле "Полное наименование организации" обязательно для заполнения' /> 
        
        <tr>
            <td colspan="2">
            &nbsp;
            </td>
        </tr>        
        
        <tr>
            <td class="left">Субъект Российской Федерации, на территории которого находится организация</td>
            <td><asp:DropDownList runat="server" ID="ddlOrganizationRegion" 
                    AppendDataBoundItems="true" CssClass="sel long"
                    DataSourceID="dsRegion" DataValueField="RegionId" DataTextField="Name" Enabled="false">
                <asp:ListItem Value="">&lt;Не задано&gt;</asp:ListItem>
            </asp:DropDownList>
            </td>
        </tr>

        <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlOrganizationRegion" 
            EnableClientScript="false" Display="None"
            ErrorMessage='Поле "Субъект Российской Федерации" обязательно для заполнения' /> 

        <tr>
            <td class="left">Учредитель (для&nbsp;ссузов&nbsp;и&nbsp;вузов)</td>
            <td><asp:TextBox runat="server" ID="txtOrganizationFounderName" CssClass="txt long" Enabled="false"/></td>
        </tr>

        <tr>
            <td class="left">Адрес</td>
            <td><asp:TextBox runat="server" ID="txtOrganizationAddress" CssClass="txt long"/></td>
        </tr>
        
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtOrganizationAddress" 
            EnableClientScript="false" Display="None"
            ErrorMessage='Поле "Адрес" обязательно для заполнения' /> 
        
        <tr>
            <td class="left">Ф. И. О. руководителя организации</td>
            <td><asp:TextBox runat="server" ID="txtOrganizationChiefName" CssClass="txt long"/></td>
        </tr>

        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtOrganizationChiefName" 
            EnableClientScript="false" Display="None"
            ErrorMessage='Поле "ФИО руководителя организации" обязательно для заполнения' /> 

        <tr>
            <td class="left">Факс</td>
            <td><asp:TextBox runat="server" ID="txtOrganizationFax" CssClass="txt small"/></td>
        </tr>

        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtOrganizationFax" 
            EnableClientScript="false" Display="None"
            ErrorMessage='Поле "Факс" обязательно для заполнения' /> 

        <tr>
            <td class="left">Телефон (с указанием кода города) руководителя организации</td>
            <td><asp:TextBox runat="server" ID="txtOrganizationPhone" CssClass="txt small"/></td>
        </tr>
        
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtOrganizationPhone" 
            EnableClientScript="false" Display="None"
            ErrorMessage='Поле "Телефон руководителя организации" обязательно для заполнения' /> 
        
        <tr>
            <td class="left">Ф. И. О. лица, ответственного за работу с ФБС</td>
            <td><asp:TextBox runat="server" ID="txtFullName" CssClass="txt long"/></td>
        </tr>

        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFullName" 
            EnableClientScript="false" Display="None"
            ErrorMessage='Поле "Ф.И.О. лица, ответственного за работу с ФБС" обязательно для заполнения' /> 

        <tr>
            <td class="left">Телефон (с указанием кода города) лица, ответственного за работу с ФБС</td>
            <td><asp:TextBox runat="server" ID="txtPhone" CssClass="txt small"/></td>
        </tr>

        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPhone" 
            EnableClientScript="false" Display="None"
            ErrorMessage='Поле "Телефон лица, ответственного за работу с ФБС" обязательно для заполнения' /> 

        <tr>
            <td class="left">E-mail лица, ответственного за работу с ФБС</td>
            <td><asp:TextBox runat="server" ID="txtEmail" CssClass="txt small"/></td>
        </tr>

        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail" 
            EnableClientScript="false" Display="None"
            ErrorMessage='Поле "E-mail лица, ответственного за работу с ФБС" обязательно для заполнения' /> 

        <asp:RegularExpressionValidator runat="server" ControlToValidate="txtEmail" 
            EnableClientScript="false" Display="None"
            ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*\.{0,1}@([0-9a-zA-Z]*[-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$" 
            ErrorMessage='Поле "E-mail лица, ответственного за работу с ФБС" заполнено неверно' />
 
        <tr>
            <td class="left">Заявка на регистрацию</td>
            <td><asp:FileUpload ID="fuRegistrationDocument" runat="server" class="long" /></td>
        </tr>
        
        <tr>
			<td colspan="2" class="box-submit">
			<asp:Button runat="server" ID="btnUpdate" Text="Сохранить" CssClass="bt" 
			    onclick="btnUpdate_Click" />
			</td>
        </tr>       
    </table>        

    <input type="hidden" name="state" />

</form>

<script language="javascript" type="text/javascript">
    InitConfirmation('', '<%= Request.Form["state"] %>');
</script>

<asp:SqlDataSource runat="server" id="dsRegion"
    connectionstring="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString%>"
    SelectCommandType="StoredProcedure"  
    SelectCommand="dbo.SearchRegion"/>

</asp:Content>
