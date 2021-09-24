<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" 
    Inherits="Fbs.Web.Administration.Documents.View" 
    MasterPageFile="~/Common/Templates/Administration.master" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">
<form runat="server">

    <table class="form">
        <tr>
            <td class="left">Название</td>
            <td class="left">
                <%= CurrentDocument.Name%>
            </td>
        </tr>  
        <tr>
            <td class="left">Описание</td>
            <td class="left">
                <%= CurrentDocument.Description%>
            </td>
        </tr>   
         
        <tr>
            <td style="padding-top:17px;">Контекст</td>
            <td>
                 <asp:DropDownList runat="server" ID="DDLContext" DataSourceID="dsContext" DataTextField="Name"
                    DataValueField="Code" Enabled="false">
                </asp:DropDownList>
                <asp:SqlDataSource runat="server" ID="dsContext" 
                    ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
                    SelectCommand="SearchContext" CancelSelectOnNullParameter="false"
                    SelectCommandType="StoredProcedure"> 
                </asp:SqlDataSource>
            </td>
        </tr> 
        
        <tr>
            <td class="left">Относительный Url</td>
            <td class="left">
                <%=CurrentDocument.RelativeUrl %>
            </td>
        </tr>  
            
        <tr>
            <td class="left">Опубликован</td>
            <td class="left">
                <%= CurrentDocument.IsActive ? "Да" : "Нет" %>
            </td>
        </tr>

        <tr>
            <td class="left">Содержимое</td>
            <td  class="left">
                    <% if (CurrentDocument.ContentSize > 0){%>
                        <a href='Document.aspx?id=<%=CurrentDocument.Id %>'>Просмотр</a>
                    <%} else {%>
                        Нет
                    <%} %>
            </td>
        </tr>
    </table>        
</form>

</asp:Content>
 

