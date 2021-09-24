<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Fbs.Web.Administration.Accounts.Users.View"
    MasterPageFile="~/Common/Templates/Administration.Master" %>

<%@ Import Namespace="Fbs.Web" %>
<%@ Import Namespace="Fbs.Core" %>
<asp:Content runat="server" ContentPlaceHolderID="cphContent">
    <form id="form1" runat="server">
    <table>
        <tr>
            <td>
                <table class="form f600" style="width: 100%">
                    <tr>
                        <td class="left">
                            Логин
                        </td>
                        <td class="text">
                            <%= CurrentUser.login %>
                        </td>
                    </tr>
                    <tr>
                        <td class="left">
                            Состояние
                        </td>
                        <td class="text">
                            <b>
                                <%= UserAccountExtentions.GetUserAccountStatusName(CurrentUser.status) %></b>
                            <br />
                            <%= UserAccountExtentions.GetAdministartionStatusDescription(CurrentUser)  %>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="box-submit">
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td class="left">
                            <span>Полное наименование организации<br />
                                (без организационно-правовой формы)</span>
                        </td>
                        <td class="text">
                            <% if (CurrentUser.RequestedOrganization.OrganizationId != null)
                               {%>
                            <a href="../../Organizations/Administrators/OrgCard_Edit.aspx?OrgID=<%= CurrentUser.RequestedOrganization.OrganizationId%>">
                                <%= CurrentUser.RequestedOrganization.FullName%></a>
                            <%
                               }
                               else
                               {%><%= CurrentUser.RequestedOrganization.FullName %><%}%>
                            <br />
                            <asp:Label runat="server" ID="lblEtalonOrgName" Style="white-space: normal;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="left">
                            Краткое наименование организации
                        </td>
                        <td class="text">
                            <%= CurrentUser.RequestedOrganization.ShortName %>
                            <br />
                            <asp:Label runat="server" ID="Label1" Style="white-space: normal;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="left">
                            Субъект Российской Федерации, на территории которого находится организация
                        </td>
                        <td class="text">
                            <%= CurrentUser.RequestedOrganization.Region.Name %>
                            <br />
                            <asp:Label runat="server" ID="lblEtalonRegion" Style="white-space: normal;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="left">
                            Тип ОУ
                        </td>
                        <td class="text">
                            <%= (CurrentUser.RequestedOrganization.OrgType.Id ==null) ? "Не задан" : CurrentUser.RequestedOrganization.OrgType.Name %>
                            <br />
                            <asp:Label runat="server" ID="lblEtalonOrgType" Style="white-space: normal;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="left">
                            Вид ОУ
                        </td>
                        <td class="text">
                            <%= (CurrentUser.RequestedOrganization.Kind.Id ==null )?"Не задан":CurrentUser.RequestedOrganization.Kind.Name %>
                            <br />
                            <asp:Label runat="server" ID="Label2" Style="white-space: normal;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="left">
                            Организационно-правовая форма ОУ
                        </td>
                        <td class="text">
                            <%= CurrentUser.RequestedOrganization.IsPrivate ?"Негосударственный":"Государственный" %>
                            <br />
                            <asp:Label runat="server" ID="Label3" Style="white-space: normal;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="left">
                            Является филиалом
                        </td>
                        <td class="text">
                            <%= CurrentUser.RequestedOrganization.IsFilial ?"Да":"Нет" %>
                            <br />
                            <asp:Label runat="server" ID="Label4" Style="white-space: normal;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="left">
                            ИНН
                        </td>
                        <td class="text">
                            <%= CurrentUser.RequestedOrganization.INN %>
                            <br />
                            <asp:Label runat="server" ID="Label5" Style="white-space: normal;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="left">
                            ОГРН
                        </td>
                        <td class="text">
                            <%= CurrentUser.RequestedOrganization.OGRN  %>
                            <br />
                            <asp:Label runat="server" ID="Label6" Style="white-space: normal;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="left">
                            Учредитель (для&nbsp;ССУЗов&nbsp;и&nbsp;ВУЗов)
                        </td>
                        <td class="text">
                            <%= CurrentUser.RequestedOrganization.OwnerDepartment %>
                            <br />
                            <asp:Label runat="server" ID="lblEtalonFounder" Style="white-space: normal;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="left">
                            Должность руководителя организации
                        </td>
                        <td class="text">
                            <%= CurrentUser.RequestedOrganization.DirectorPosition %>
                            <br />
                            <asp:Label runat="server" ID="Label7" Style="white-space: normal;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="left">
                            Ф.И.О. руководителя организации
                        </td>
                        <td class="text">
                            <%= CurrentUser.RequestedOrganization.DirectorFullName %>
                            <br />
                            <asp:Label runat="server" ID="lblEtalonChief" Style="white-space: normal;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="left">
                            Фактический адрес
                        </td>
                        <td class="text">
                            <%= CurrentUser.RequestedOrganization.FactAddress %>
                            <br />
                            <asp:Label runat="server" ID="Label8" Style="white-space: normal;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="left">
                            Юридический адрес
                        </td>
                        <td class="text">
                            <%= CurrentUser.RequestedOrganization.LawAddress %>
                            <br />
                            <asp:Label runat="server" ID="lblEtalonAddress" Style="white-space: normal;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="left">
                            Свидетельсво об аккредитации
                        </td>
                        <td class="text">
                            <%= CurrentUser.RequestedOrganization.AccreditationSertificate %>
                            <br />
                            <asp:Label runat="server" ID="Label9" Style="white-space: normal;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="left">
                            Код города
                        </td>
                        <td class="text">
                            <%= CurrentUser.RequestedOrganization.PhoneCityCode  %>
                            <br />
                            <asp:Label runat="server" ID="Label10" Style="white-space: normal;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="left">
                            Телефон
                        </td>
                        <td class="text">
                            <%= CurrentUser.RequestedOrganization.Phone %>
                            <br />
                            <asp:Label runat="server" ID="lblEtalonOrgPhone" Style="white-space: normal;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="left">
                            Факс
                        </td>
                        <td class="text">
                            <%= CurrentUser.RequestedOrganization.Fax %>
                        </td>
                    </tr>
                    <tr>
                        <td class="left">
                            EMail
                        </td>
                        <td class="text">
                            <%= CurrentUser.RequestedOrganization.EMail  %>
                        </td>
                    </tr>
                    <tr>
                        <td class="left">
                            Сайт
                        </td>
                        <td class="text">
                            <%= CurrentUser.RequestedOrganization.Site  %>
                        </td>
                    </tr>
                </table>
            </td> 
       </tr> 
    </table>
    <% if (IsRegistrationDocumentExists)
       {%>
    <script type="text/javascript">
        window.open('/Profile/ConfirmedDocumentView.aspx?login=<%= CurrentUser.login %>', 'RegZajavView', 'top=0,left=0,width=600,height=560,status=no,toolbar=no,menubar=no,scrollbars=yes,resizable=yes');
    </script>
    <%      }%>
    </form>
</asp:Content>
