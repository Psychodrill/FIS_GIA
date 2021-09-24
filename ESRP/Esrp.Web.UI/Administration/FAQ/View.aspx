<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" 
    Inherits="Esrp.Web.Administration.FAQ.View" 
    MasterPageFile="~/Common/Templates/Administration.master" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">
<form runat="server">

     <table class="form">
        <tr>
            <td class="left">Вопрос кратко</td>
            <td class="left">
                <%=CurrentAskedQuestion.Name %>
            </td>
        </tr>
        
        <tr>
            <td class="left">Вопрос полностью</td>
            <td class="left">
                <%=CurrentAskedQuestion.Question %>
            </td>
        </tr>  

        <tr>
            <td class="left">Ответ</td>
            <td class="left">
                <%=CurrentAskedQuestion.Answer %>
            </td>
        </tr>  
        
        <tr>
            <td style="padding-top:17px;">Контекст</td>
            <td>
                <web:CheckBoxList runat="server" ID="cblContext" DataSourceID="dsContext" 
                    DataTextField="Name" DataValueField="Code" Enabled="false">
                </web:CheckBoxList>
                <asp:SqlDataSource runat="server" ID="dsContext" 
                    ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>"
                    SelectCommand="SearchContext" CancelSelectOnNullParameter="false"
                    SelectCommandType="StoredProcedure"> 
                </asp:SqlDataSource>
            </td>
        </tr>

        <tr>
            <td class="left">Опубликован</td>
            <td class="left">
                <%= CurrentAskedQuestion.IsActive ? "Да" : "Нет"%>
            </td>
        </tr>
        
    </table>  
          
</form>

</asp:Content>
 

