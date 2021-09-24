<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HistoryVersion.aspx.cs"
    Inherits="Fbs.Web.Administration.Accounts.Users.HistoryVersion" 
    MasterPageFile="~/Common/Templates/Administration.Master" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="Fbs.Core" %>
<%@ Import Namespace="Fbs.Web" %>

<asp:Content runat="server" ContentPlaceHolderID="cphActions">
    <div class="h10"></div>
    <div class="border-block">
        <div class="tr"><div class="tt"><div></div></div></div>
        <div class="content">
        <ul>
        <li><a href="/Administration/Accounts/Users/Edit.aspx?Login=<%= Request.QueryString["login"] %>"
            title="Редактирование" class="gray">Редактирование</a></li>
        </ul>
        </div>
    <div class="br"><div class="tt"><div></div></div></div>
    </div> 
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">
    <form id="form1" runat="server">

    <asp:Repeater runat="server" ID="rpHistoryVersion" DataSourceID="dsHistoryVersion">
        <ItemTemplate>
            <table class="form f600">
                <tr><td colspan="2" class="left">
                    Редакция № <%#Eval("VersionId")%> от <%# (DateTime)Eval("UpdateDate") %>
                </td></tr>
                <tr>
                    <td class="left">Редактировал</td>
                    <td class="left">
                        <%#Eval("EditorLastName")%>
                        <%#Eval("EditorFirstName")%>
                        <%#Eval("EditorPatronymicName")%> 
                        <%# String.IsNullOrEmpty(Eval("EditorLogin").ToString()) ? "" : ("(" + Eval("EditorLogin") +")")%>
                    </td>
                </tr>
                <tr>
                    <td class="left">IP-адрес редактора</td>
                    <td class="left">
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
                    <td class="left">Состояние</td>
                    <td class="text"><b>
                        <%# UserAccountExtentions.GetUserAccountStatusName(UserAccount.ConvertStatusCode(Eval("Status").ToString()))%>
                    </b></td>
                </tr>             
                
                <tr>
		            <td colspan="2" class="box-submit"><br/></td>
                </tr>            
                <tr>
                    <td class="left"><span>Полное наименование организации</span></td>
                    <td class="text"><%#Eval("OrganizationName") %></td>
                </tr>
                <tr>
                    <td class="left">Субъект Российской Федерации, на территории которого находится организация</td>
                    <td class="text"><%#Eval("OrganizationRegionName") %></td>
                </tr>
                <tr>
                    <td class="left">Учредитель</td>
                    <td class="text"><%#Eval("OrganizationFounderName") %></td>
                </tr>
                <tr>
                    <td class="left">Адрес</td>
                    <td class="text"><%#Eval("OrganizationAddress") %></td>
                </tr>
                <tr>
                    <td class="left">Ф. И. О. руководителя организации</td>
                    <td class="text"><%#Eval("OrganizationChiefName") %></td>
                </tr>
                <tr>
                    <td class="left">Факс</td>
                    <td class="text"><%#Eval("OrganizationFax") %></td>
                </tr>
                <tr>
                    <td class="left">Телефон руководителя организации</td>
                    <td class="text"><%#Eval("OrganizationPhone") %></td>
                </tr>
                <tr>
                    <td class="left">Ф. И. О. лица, ответственного за работу с ФБС</td>
                    <td class="text">
                        <%#Eval("LastName")%>
                        <%#Eval("FirstName")%>
                        <%#Eval("PatronymicName")%> 
                    </td>       
                </tr>
                <tr>
                    <td class="left">Телефон лица, ответственного за работу с ФБС</td>
                    <td class="text"><%#Eval("Phone") %></td>
                </tr>
                <tr>
                    <td class="left">E-mail лица, ответственного за работу с ФБС</td>
                    <td class="text"><%#Eval("Email") %></td>
                </tr>
                <tr>
                    <td class="left">Тип ОУ</td>
                    <td class="text"><%# Convert.IsDBNull(Eval("EducationInstitutionTypeName")) ? "Не задан" : Eval("EducationInstitutionTypeName") %></td>
                </tr>
                <tr>
                    <td class="left">Список постоянных внешних IP-адресов компьютеров, с которых будет 
                        осуществляться доступ к ФБС</td>
                    <td class="text"><%# Eval("IpAddresses").ToString().Replace(",", "<br />")%></td>            
                </tr>
                <tr>
                    <td class="left">Интеграция с АИС Экзамен активирована</td>
                    <td class="text"><%# Convert.ToBoolean(Eval("HasCrocEgeIntegration")) ? "Да" : "Нет" %></td>
                </tr>
                
                <tr>
		            <td colspan="2" class="box-submit"><br /></td>
                </tr>            
            </table>
        </ItemTemplate>
        <AlternatingItemTemplate>
            По Ващему запросу ничего не найдено
        </AlternatingItemTemplate>
    </asp:Repeater>
    
    <asp:SqlDataSource runat="server" ID="dsHistoryVersion" ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="GetUserAccountLog" CancelSelectOnNullParameter="false" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter Name="login" Type="String" QueryStringField="login" />
            <asp:QueryStringParameter Name="versionId" Type="String" QueryStringField="version" />
        </SelectParameters>
    </asp:SqlDataSource>
    
    </form>
</asp:Content>
