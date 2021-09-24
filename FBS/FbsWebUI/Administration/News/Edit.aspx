<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs"
    Inherits="Fbs.Web.Administration.News.Edit" ValidateRequest="false"
    MasterPageFile="~/Common/Templates/Administration.master" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="Fbs.Core" %>
<%@ Import Namespace="Fbs.Web" %>

<asp:Content runat="server" ContentPlaceHolderID="cphHead">
    <script type="text/javascript" src="/Common/Scripts/tiny_mce/tiny_mce.js"></script>
    <script type="text/javascript" src="/Common/Scripts/CalendarPopup.js"></script>
    <script type="text/javascript" src="/Common/Scripts/Utils.js"></script>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">
<form runat="server">
    <asp:ValidationSummary runat="server" DisplayMode="BulletList" EnableClientScript="false"
        HeaderText="<p>Произошли следующие ошибки:</p>"/>
        
    <table class="form">
        <tr>
            <td class="left">Дата</td>
            <td class="text">
                <asp:TextBox runat="server" ID="txtDate" CssClass="txt date" />
                <img src="/Common/Images/ico-datepicker-fbs.gif" id="btnDate" onclick='return PickDate("<%=txtDate.ClientID%>", "btnDate", "CalendarContainer");' />
            </td>
        </tr>

        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDate" 
            EnableClientScript="false" Display="None"
            ErrorMessage='Поле "Дата" обязательно для заполнения' /> 
                     
        <asp:RegularExpressionValidator runat="server" ErrorMessage="Неверный формат даты" 
            ControlToValidate="txtDate" EnableClientScript="false" Display="None"
            ValidationExpression="^\d{1,2}\.\d{1,2}\.(?:\d{4}|\d{2})$" />
        
        <tr>
            <td class="left">Название</td>
            <td class="text">
                <asp:TextBox runat="server" ID="txtName" CssClass="txt small" />
            </td>
        </tr>  
       
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtName" 
            EnableClientScript="false" Display="None"
            ErrorMessage='Поле "Название" обязательно для заполнения' /> 
            
        <tr>
            <td class="left">Краткое писание</td>
            <td class="text">
                <asp:TextBox runat="server" ID="txtDescription" 
                    TextMode="MultiLine" Rows="5" Columns="35" CssClass="txt-area small"/>
            </td>
        </tr>   
        
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDescription" 
            EnableClientScript="false" Display="None"
            ErrorMessage='Поле "Краткое писание" обязательно для заполнения' /> 
            
        <tr>
            <td class="left">Опубликовать новость</td>
            <td class="text">
                <asp:DropDownList ID="ddlIsActive" runat="server">
                    <asp:ListItem Text="Да" Value="True" />
                    <asp:ListItem  Text="Нет"  Value="False" />
                </asp:DropDownList>
            </td>
        </tr>      
                 
        <tr>
            <td colspan="2" class="text">
                <asp:TextBox runat="server" ID="txtNews" TextMode="MultiLine" Columns="60" 
                    Rows="15"  CssClass="txt-area" />
                <script type="text/javascript">InitTinyMCE("<%= txtNews.ClientID %>");</script>                    
            </td>
        </tr>  
        
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNews" 
            EnableClientScript="false" Display="None"
            ErrorMessage='Поле "Текст" обязательно для заполнения' /> 
                                

            
        <tr>
		    <td colspan="2" class="box-submit">
		        <asp:Button runat="server" ID="btnUpdate" Text="Сохранить" CssClass="bt" 
		            onclick="btnUpdate_Click" />
	        </td>
        </tr>      
    </table>        
</form>
<div id="CalendarContainer" style="position:absolute;visibility:hidden;background-color:white;layer-background-color:white;"></div>
</asp:Content>


