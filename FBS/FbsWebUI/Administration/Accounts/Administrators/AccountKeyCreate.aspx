<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountKeyCreate.aspx.cs" 
    Inherits="Fbs.Web.Administration.Accounts.Administrators.AccountKeyCreate"
    MasterPageFile="~/Common/Templates/Administration.Master" %>

<asp:Content runat="server" ContentPlaceHolderID="cphHead">
   <script type="text/javascript" src="/Common/Scripts/CalendarPopup.js"></script>
   <script type="text/javascript" src="/Common/Scripts/Utils.js"></script>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">
<form runat="server">
    <asp:ValidationSummary runat="server"  DisplayMode="BulletList" EnableClientScript="false"
        HeaderText="<p>Произошли следующие ошибки:</p>"/>

    <table class="form" style="width:auto">
        <tr>
            <td class="left" style="width:100px">Действителен с</td>
            <td>
                <asp:TextBox runat="server" ID="txtDateFrom" CssClass="txt date" />
                <img src="/Common/Images/ico-datepicker-fbs.gif" id="btnDateFrom" alt="выбрать дату"
                    onclick='return PickDate("<%= txtDateFrom.ClientID %>", "btnDateFrom", "CalendarContainer");' />
                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtDateFrom" 
                    EnableClientScript="false" Display="None"
                    ValidationExpression="^\d{1,2}\.\d{1,2}\.\d{4}$" 
                    ErrorMessage='Неверно указана начальная дата' />  
            </td>
        </tr>
        <tr>
            <td class="left">Действителен по</td>
            <td>
                <asp:TextBox runat="server" ID="txtDateTo" CssClass="txt date" />
                <img src="/Common/Images/ico-datepicker-fbs.gif" id="btnDateTo" alt="выбрать дату"
                    onclick='return PickDate("<%= txtDateTo.ClientID %>", "btnDateTo", "CalendarContainer");' />
                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtDateTo" 
                    EnableClientScript="false" Display="None"
                    ValidationExpression="^\d{1,2}\.\d{1,2}\.\d{4}$" 
                    ErrorMessage='Неверно указана конечная дата' />   
            </td>
        </tr>
        <tr>
            <td class="left"></td>
            <td><asp:CheckBox runat="server" ID="cbIsActive" Text="Активировать ключ" /></td>
        </tr> 
        <tr>
		    <td colspan="2" class="box-submit">
		    <asp:Button runat="server" ID="btnCreate" Text="Создать" CssClass="bt" onclick="btnCreate_Click" />
		    </td>
        </tr>   
    </table>  
    <div id="CalendarContainer" style="position:absolute; visibility:hidden; background-color:white; z-index:99;"></div>
</form>    
</asp:Content>

