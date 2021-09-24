<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePasswordSuccess.aspx.cs"
    Inherits="Esrp.Web.Personal.Profile.ChangePasswordSuccess" MasterPageFile="~/Common/Templates/Main.master" %>

<%@ Import Namespace="Esrp.Web" %>
<asp:Content runat="server" ContentPlaceHolderID="cphContent">
    <p>
        Ваш пароль успешно изменен. На ваш адрес e-mail выслано уведомление.</p>
    <br/>
    <div class="statement_table" style="margin-top: 0px;">
        <table width="100%">
            <tr>
                <th>
                    <span>Ваш логин</span>
                </th>
                <td class="text">
                    <%=User.Identity.Name%>
                </td>
            </tr>
            <tr>
                <th>
                    <span>Ваш пароль</span>
                </th>
                <td class="text">
                    <%=Utility.GetPasswordFromSession() %>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
