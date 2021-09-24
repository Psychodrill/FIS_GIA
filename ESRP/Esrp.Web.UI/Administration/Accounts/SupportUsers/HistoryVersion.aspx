<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HistoryVersion.aspx.cs"
    Inherits="Esrp.Web.Administration.Accounts.SupportUsers.HistoryVersion" 
    MasterPageFile="~/Common/Templates/Administration.Master" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="Esrp.Core" %>
<%@ Import Namespace="Esrp.Core.Systems" %>
<%@ Import Namespace="Esrp.Web" %>

<asp:Content runat="server" ContentPlaceHolderID="cphActions">
    <div class="h10"></div>
    <div class="border-block">
        <div class="tr"><div class="tt"><div></div></div></div>
        <div class="content">
        <ul>
        <li><a href="/Administration/Accounts/SupportUsers/Edit.aspx?Login=<%= Request.QueryString["login"] %>"
            title="Редактирование" class="gray">Редактирование</a></li>
        </ul>
        </div>
    <div class="br"><div class="tt"><div></div></div></div>
    </div> 
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">
    <form runat="server">

    <asp:Repeater runat="server" ID="rpHistoryVersion" DataSourceID="dsHistoryVersion">
        <ItemTemplate>
            <table class="form f600">
                <tr><td colspan="2" class="left">
                    Редакция № <%#Eval("VersionId")%> от <%#(DateTime)Eval("UpdateDate") %>
                </td></tr>
                <tr>
                    <td class="left">Редактировал</td>
                    <td class="left">
                        <%#Eval("EditorLastName")%>
                        <%#Eval("EditorFirstName")%>
                        <%#Eval("EditorPatronymicName")%> 
                        <%# String.IsNullOrEmpty(Eval("EditorLogin").ToString()) ? "" : ("(" + Eval("EditorLogin") + ")")%>
                    </td>
                </tr>
                <tr>
                    <td class="left">IP-адрес редактора</td>
                    <td class="text">
                        <%# Convert.ToBoolean(Eval("IsVpnEditorIp")) ? "VPN&nbsp;" : String.Empty%><%#Eval("EditorIp")%>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="box-submit"><br /></td>
                </tr>
                
                <tr>
                    <td class="left">Логин</td>
                    <td class="text"><%#Eval("Login")%></td>
                </tr>  
                              
                <tr>
                    <td class="left">
                        Ф. И. О.
                    </td>
                    <td class="text">
                        <%#Eval("LastName")%>
                        <%#Eval("FirstName")%>
                        <%#Eval("PatronymicName")%> 
                    </td>
                </tr>
                <tr>
                    <td class="left">
                        Телефон
                    </td>
                    <td class="text">
                        <%#Eval("Phone") %>
                    </td>
                </tr>
                <tr>
                    <td class="left">
                        E-mail
                    </td>
                    <td class="text">
                        <%#Eval("Email") %>
                    </td>
                </tr>
                <tr>
                    <td class="left">
                        Список постоянных внешних IP-адресов компьютеров, с которых будет осуществляться
                        доступ к <%= GeneralSystemManager.GetSystemName(2) %>
                    </td>
                    <td class="text">
                        <%# Eval("IpAddresses").ToString().Replace(",", "<br />")%>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="box-submit">
                        <br />
                    </td>
                </tr>
            </table>
        </ItemTemplate>
        <AlternatingItemTemplate>
            По Ващему запросу ничего не найдено
        </AlternatingItemTemplate>
    </asp:Repeater>
    
    <asp:SqlDataSource runat="server" ID="dsHistoryVersion" 
        ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="GetAccountLog" 
        CancelSelectOnNullParameter="false" 
        SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter Name="login" Type="String" QueryStringField="login" />
            <asp:QueryStringParameter Name="versionId" Type="String" QueryStringField="version" />
        </SelectParameters>
    </asp:SqlDataSource>
    
    </form>
</asp:Content>
