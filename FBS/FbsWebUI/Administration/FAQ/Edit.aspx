<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" 
    Inherits="Fbs.Web.Administration.FAQ.Edit" 
    MasterPageFile="~/Common/Templates/Administration.master" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="Fbs.Core" %>
<%@ Import Namespace="Fbs.Web" %>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">
<form runat="server">

    <asp:ValidationSummary runat="server" DisplayMode="BulletList" EnableClientScript="false"
        HeaderText="<p>Произошли следующие ошибки:</p>"/>

    <table class="form">
        <tr>
            <td class="left">Вопрос кратко</td>
            <td class="text">
                <asp:TextBox runat="server" ID="txtName" CssClass="txt small" />
            </td>
        </tr>
        
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtName" 
            EnableClientScript="false" Display="None"
            ErrorMessage='Поле "Вопрос кратко" обязательно для заполнения' />  
        
        <tr>
            <td class="left">Вопрос полностью</td>
            <td class="text">
                <asp:TextBox runat="server" ID="txtQuestion" 
                    TextMode="MultiLine" Rows="5" Columns="35" CssClass="txt-area small"/>
            </td>
        </tr>  

        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtQuestion" 
            EnableClientScript="false" Display="None"
            ErrorMessage='Поле "Вопрос полностью" обязательно для заполнения' />  
          
        <tr>
            <td class="left">Ответ</td>
            <td class="text">
                <asp:TextBox runat="server" ID="txtAnswer" 
                    TextMode="MultiLine" Rows="5" Columns="35" CssClass="txt-area small"/>
            </td>
        </tr>  

        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtAnswer" 
            EnableClientScript="false" Display="None" 
            ErrorMessage='Поле "Ответ" обязательно для заполнения' />  
        
        
        <tr>
            <td style="padding-top:17px;">Контекст</td>
            <td>
                <web:CheckBoxList runat="server" ID="cblContext" DataSourceID="dsContext" 
                    DataTextField="Name" DataValueField="Code">
                </web:CheckBoxList>
                <asp:SqlDataSource runat="server" ID="dsContext" 
                    ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
                    SelectCommand="SearchContext" CancelSelectOnNullParameter="false"
                    SelectCommandType="StoredProcedure"> 
                </asp:SqlDataSource>
            </td>
        </tr>

        <tr>
            <td class="left">Опубликован</td>
            <td>
                <asp:DropDownList ID="ddlIsActive" runat="server">
                    <asp:ListItem Text="Да" Value="True" />
                    <asp:ListItem  Text="Нет"  Value="False" />
                </asp:DropDownList>
            </td>
        </tr>  
            
        <tr>
		    <td colspan="2" class="box-submit">
		        <asp:Button runat="server" ID="btnUpdate" Text="Сохранить" CssClass="bt" 
		            onclick="btnUpdate_Click" />
	        </td>
        </tr>      
    </table>        
</form>

</asp:Content>
