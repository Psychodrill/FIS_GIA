<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" 
    Inherits="Esrp.Web.Administration.News.View" 
    MasterPageFile="~/Common/Templates/Administration.master" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
   
<asp:Content runat="server" ContentPlaceHolderID="cphContent">
<form runat="server">

    <table class="form">
        <tr>
            <td class="left">Дата</td>
            <td class="left">
                <%=  CurrentNews.Date.ToShortDateString() %>
            </td>
        </tr>
        
        <tr>
            <td class="left">Название</td>
            <td class="left">
                <%= CurrentNews.Name%>
            </td>
        </tr>  
        
        <tr>
            <td class="left">Краткое писание</td>
            <td class="left">
                <%= CurrentNews.Description %>
            </td>
        </tr> 
                    
        <tr>
            <td class="left">Опубликована</td>
            <td class="left">
                <%= CurrentNews.IsActive ? "Да" : "Нет"%>
            </td>
        </tr>
        
        <tr>
            <td colspan="2" class="text">
                <%= CurrentNews.Text %>
            </td>
        </tr> 
    </table>        
</form>

</asp:Content>
 


