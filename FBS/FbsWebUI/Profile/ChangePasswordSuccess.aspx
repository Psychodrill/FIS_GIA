<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePasswordSuccess.aspx.cs" 
    Inherits="Fbs.Web.Personal.Profile.ChangePasswordSuccess"
    MasterPageFile="~/Common/Templates/Personal.master" %>
<%@ Import Namespace="Fbs.Web" %>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">
    <p>Ваш пароль успешно изменен. На ваш адрес e-mail выслано уведомление.</p>

    <table class="form">
    <tr>
        <td class="left"><span>Ваш логин</span></td>
        <td class="text"><%=User.Identity.Name%></td>
    </tr>
    <tr>
        <td class="left"><span>Ваш пароль</span></td>
        <td class="text"><%=Utility.GetPasswordFromSession() %></td>
    </tr>
    </table>
</asp:Content>
