<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" 
    Inherits="Fbs.Web.Administration.Documents.Edit" 
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
            <td class="left">Название</td>
            <td class="text">
                <asp:TextBox runat="server" ID="txtName" CssClass="txt small" />
            </td>
        </tr>  

        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtName" 
            EnableClientScript="false" Display="None"
            ErrorMessage='Поле "Название" обязательно для заполнения' />  

        <tr>
            <td class="left">Описание</td>
            <td class="text">
                <asp:TextBox runat="server" ID="txtDescription" 
                    TextMode="MultiLine" Rows="5" Columns="35" CssClass="txt-area small"/>
            </td>
        </tr>   
         
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDescription" 
            EnableClientScript="false" Display="None"
            ErrorMessage='Поле "Описание" обязательно для заполнения' />      
                     
        <tr>
            <td style="padding-top:17px;">Контекст</td>
            <td>
                 <asp:DropDownList runat="server" ID="DDLContext" DataSourceID="dsContext" DataTextField="Name"
                    DataValueField="Code" AppendDataBoundItems="true">
                    <asp:ListItem Text="Не задан" Value=""></asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ID="VReqContext" ControlToValidate="DDLContext" EnableClientScript="false" Display="None" ErrorMessage="Не выбран контекст"></asp:RequiredFieldValidator>
                <asp:SqlDataSource runat="server" ID="dsContext" 
                    ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
                    SelectCommand="SearchContext" CancelSelectOnNullParameter="false"
                    SelectCommandType="StoredProcedure"> 
                </asp:SqlDataSource>
            </td>
        </tr> 
        
        <asp:RegularExpressionValidator runat="server" ControlToValidate="txtRelativeUrl" 
            EnableClientScript="false" Display="None" ValidationExpression="^(/[a-zA-Z0-9-_./]+)$"
            ErrorMessage='Поле "Относительный Url" заполнено некорректно.'/>  

        <tr>
            <td class="left">Относительный Url</td>
            <td>
                <asp:TextBox runat="server" ID="txtRelativeUrl" CssClass="txt small" />
            </td>
        </tr>  
            
        <tr>
            <td class="left">Опубликован</td>
            <td class="text">
                <asp:DropDownList ID="ddlIsActive" runat="server">
                    <asp:ListItem Text="Да" Value="True" />
                    <asp:ListItem  Text="Нет"  Value="False" />
                </asp:DropDownList>
            </td>
        </tr>  

        <tr>
            <td class="left">Содержимое</td>
            <td style="padding-top:13px;">
                <p>
                    <% if (CurrentDocument.ContentSize > 0){%>
                        <a href='Document.aspx?id=<%=CurrentDocument.Id %>'>Просмотр</a>
                    <%} else {%>
                        Нет
                    <%} %>
                </p>
                <p><asp:FileUpload ID="fuDocument" runat="server" class="long" /></p>
            </td>
        </tr>
            
        <asp:CustomValidator runat="server" ID="validFileSize" EnableClientScript="false"
            ControlToValidate="fuDocument" Display="None" OnServerValidate="validFileSize_ServerValidate"
            EnableViewState="false"  ErrorMessage='Вы пытаетесь загрузить пустой файл' />
            
        <tr>
		    <td colspan="2" class="box-submit">
		        <asp:Button runat="server" ID="btnUpdate" Text="Сохранить" CssClass="bt" 
		            onclick="btnUpdate_Click" />
	        </td>
        </tr>      
    </table>        
</form>

</asp:Content>
 
