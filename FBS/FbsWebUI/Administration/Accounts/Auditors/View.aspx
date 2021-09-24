<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" 
    Inherits="Fbs.Web.Administration.Accounts.Auditors.View"
    MasterPageFile="~/Common/Templates/Administration.Master" %>
<%@ Import Namespace="Fbs.Web" %>
<%@ Import Namespace="Fbs.Core" %>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">
   <table class="form f600">
        <tr>
            <td class="left">Логин</td>
            <td class="text"><%= CurrentUser.Login %></td>
        </tr>
        <tr>
		    <td colspan="2" class="box-submit"><br/></td>
        </tr>            
        <tr>
            <td class="left">Ф. И. О.</td>
            <td class="text"><%= CurrentUser.GetFullName() %></td>            
        </tr>
        <tr>
            <td class="left">Телефон</td>
            <td class="text"><%= CurrentUser.Phone %></td>
        </tr>
        <tr>
            <td class="left">E-mail</td>
            <td class="text"><%= CurrentUser.Email %></td>
        </tr>

        <tr>
		    <td colspan="2" class="box-submit"><br /></td>
        </tr>            
    </table>
</asp:Content>
