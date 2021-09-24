<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserProfileEdit.ascx.cs" Inherits="Fbs.Web.Controls.UserProfileEdit" %>

<% if (CurrentUser.CanEdit) { %>

<asp:ValidationSummary runat="server" DisplayMode="BulletList" 
    EnableClientScript="false" ValidationGroup="UserProfile"
    HeaderText="<p>Произошли следующие ошибки:</p>"/>

<table class="form">
    <tr>
        <td class="left">Логин</td>
        <td class="text"><asp:Literal runat="server" ID="litUserName"/></td>
        <td></td>
    </tr>
        <tr>
        <td class="left">Текущий шаг регистрации</td>
        <td class="text"><b>
            <asp:Literal runat="server" ID="litStatus"/></b><br/>
            <asp:Literal runat="server" ID="litStatusDescription"/>
        </td>
    </tr>   
    <tr>
        <td class="left">Состояние</td>
        <td class="text"><b>
            <asp:Literal runat="server" ID="litNewStatus"/></b><br/>
            
        </td>
        <td></td>
    </tr>      
    <tr>
        <td class="left">Полное&nbsp;наименование&nbsp;организации<BR/>(без организационно-правовой формы)</td>
        <td>
            <asp:TextBox runat="server" ID="txtOrganizationName" CssClass="txt long"/>
            <asp:HiddenField runat="server" ID="hfEtalonOrgID" />
        </td>
        <td colspan=11><asp:Button ID="btnChangeOrg" runat="server" Text="Выбрать" Width="100px" ToolTip="Выбор организации" /></td>
    </tr>
    
    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtOrganizationName" 
        EnableClientScript="false" Display="None" ValidationGroup="UserProfile"
        ErrorMessage='Поле "Полное наименование организации" обязательно для заполнения' /> 
        
    <tr>
        <td class="left"><span>Тип организации</span></td>
        <td>
            <asp:DropDownList runat="server" ID="ddlOrgType" CssClass="sel small" DataTextField="Name" DataValueField="Id" >
                <asp:ListItem Value="">&lt;Не задано&gt;</asp:ListItem>
                <asp:ListItem Value="0">&lt;Не определён&gt;</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlOrgType"
            EnableClientScript="false" Display="None" ValidationGroup="UserProfile"
            ErrorMessage='Поле "Тип организации" обязательно для заполнения' /> 
    
    <tr>
        <td class="left">Субъект Российской Федерации, на территории которого находится организация</td>
        <td><asp:DropDownList runat="server" ID="ddlOrganizationRegion" 
                AppendDataBoundItems="true" CssClass="sel small"
                DataValueField="Id" DataTextField="Name">
            <asp:ListItem Value="">&lt;Не задано&gt;</asp:ListItem>
            <asp:ListItem Value="0">&lt;Не установлен&gt;</asp:ListItem>
        </asp:DropDownList>
        </td>
    </tr>

    <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlOrganizationRegion" 
        EnableClientScript="false" Display="None" ValidationGroup="UserProfile"
        ErrorMessage='Поле "Субъект Российской Федерации" обязательно для заполнения' /> 

    <tr>
        <td class="left">Учредитель (для ссузов, вузов и РЦОИ)</td>
        <td><asp:TextBox runat="server" ID="txtOrganizationFounderName" CssClass="txt long"/></td>
    </tr>

    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtOrganizationFounderName" 
        EnableClientScript="false" Display="None" ValidationGroup="UserProfile"
        ErrorMessage='Поле "Учредитель" обязательно для заполнения' /> 

    <tr>
        <td class="left">Юридический адрес</td>
        <td><asp:TextBox runat="server" ID="txtOrganizationAddress" CssClass="txt long"/></td>
    </tr>
    
    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtOrganizationAddress" 
        EnableClientScript="false" Display="None" ValidationGroup="UserProfile"
        ErrorMessage='Поле "Адрес" обязательно для заполнения' /> 
    
    <tr>
        <td class="left">Ф. И. О. руководителя организации</td>
        <td><asp:TextBox runat="server" ID="txtOrganizationChiefName" CssClass="txt long"/></td>
    </tr>

    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtOrganizationChiefName" 
        EnableClientScript="false" Display="None" ValidationGroup="UserProfile"
        ErrorMessage='Поле "ФИО руководителя организации" обязательно для заполнения' /> 

    

    <tr>
        <td class="left">Телефон (с указанием кода города) руководителя организации</td>
        <td><asp:TextBox runat="server" ID="txtOrganizationPhone" CssClass="txt small"/></td>
    </tr>
    
    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtOrganizationPhone" 
        EnableClientScript="false" Display="None" ValidationGroup="UserProfile"
        ErrorMessage='Поле "Телефон руководителя организации" обязательно для заполнения' /> 
        
    <asp:RegularExpressionValidator runat="server" ID="vldOrganizationPhone" ControlToValidate="txtOrganizationPhone" 
            EnableClientScript="false" Display="None" ValidationGroup="UserProfile"
            ValidationExpression="( *\+?\d?[ ]*(\([\d ]+\))?[\d- ]*[;,]?)*" 
            ErrorMessage='Поле "Телефон руководителя организации" заполнено неверно. Пример: (495) 125-45-67; +7 (901) 123-45-67' />
    <tr>
        <td class="left">Факс</td>
        <td><asp:TextBox runat="server" ID="txtOrganizationFax" CssClass="txt small"/></td>
    </tr>

    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtOrganizationFax" 
        EnableClientScript="false" Display="None" ValidationGroup="UserProfile"
        ErrorMessage='Поле "Факс" обязательно для заполнения' /> 

    <asp:RegularExpressionValidator runat="server" ID="vldOrganizationFax" ControlToValidate="txtOrganizationFax" 
            EnableClientScript="false" Display="None" ValidationGroup="UserProfile"
            ValidationExpression="( *\+?\d?[ ]*(\([\d ]+\))?[\d- ]*[;,]?)*" 
            ErrorMessage='Поле "Факс" заполнено неверно. Пример: (495) 125-45-67; +7 (901) 123-45-67' />    
    <tr>
        <td class="left">Ф. И. О. лица, ответственного за работу с ФБС</td>
        <td><asp:TextBox runat="server" ID="txtFullName" CssClass="txt long"/></td>
    </tr>

    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFullName" 
        EnableClientScript="false" Display="None" ValidationGroup="UserProfile"
        ErrorMessage='Поле "Ф.И.О. лица, ответственного за работу с ФБС" обязательно для заполнения' /> 

    <tr>
        <td class="left">Телефон (с указанием кода города) лица, ответственного за работу с ФБС</td>
        <td><asp:TextBox runat="server" ID="txtPhone" CssClass="txt small"/></td>
    </tr>

    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPhone" 
        EnableClientScript="false" Display="None" ValidationGroup="UserProfile"
        ErrorMessage='Поле "Телефон лица, ответственного за работу с ФБС" обязательно для заполнения' /> 
        
    <asp:RegularExpressionValidator runat="server" ID="vldPhone" ControlToValidate="txtPhone" 
            EnableClientScript="false" Display="None" ValidationGroup="UserProfile"
            ValidationExpression="( *\+?\d?[ ]*(\([\d ]+\))?[\d- ]*[;,]?)*" 
            ErrorMessage='Поле "Телефон лица, ответственного за работу с ФБС" заполнено неверно. Пример: (495) 125-45-67; +7 (901) 123-45-67' />
    <tr>
        <td class="left">E-mail лица, ответственного за работу с ФБС</td>
        <td><asp:TextBox runat="server" ID="txtEmail" CssClass="txt small"/></td>
    </tr>

    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail" 
        EnableClientScript="false" Display="None" ValidationGroup="UserProfile"
        ErrorMessage='Поле "E-mail лица, ответственного за работу с ФБС" обязательно для заполнения' /> 

    <asp:RegularExpressionValidator runat="server" ControlToValidate="txtEmail" 
        EnableClientScript="false" Display="None" ValidationGroup="UserProfile"
        ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*\.{0,1}@([0-9a-zA-Z]*[-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$" 
        ErrorMessage='Поле "E-mail лица, ответственного за работу с ФБС" заполнено неверно' />     
        
    <asp:CustomValidator runat="server" ID="vldIsEmailUniq" EnableClientScript="false" Display="None" 
        ControlToValidate="txtEmail"  OnServerValidate="vldIsEmailUniq_ServerValidate" ValidationGroup="UserProfile"
        ErrorMessage='Регистрация в системе невозможна. Пользователь с таким e-mail уже зарегистрирован в системе. Введите уникальный e-mail' />


    <tr>
		<td colspan="2" class="box-submit">
		<asp:Button runat="server" ID="btnUpdate" Text="Сохранить" CssClass="bt" 
		    onclick="btnUpdate_Click" ValidationGroup="UserProfile" />
		</td>
    </tr>
</table>        

<input type="hidden" name="state" />

<script language="javascript" type="text/javascript">
    InitConfirmation('', '<%= Request.Form["state"] %>');
</script>

<% } else { %>

<p>Вы не имеете прав для редактирования регистрационных данных.<br />
    Перейдите на страницу просмотра <a href="/Profile/View.aspx" 
    title="Регистрационные данные">регистрационных данных</a>.</p>

<% } %>

