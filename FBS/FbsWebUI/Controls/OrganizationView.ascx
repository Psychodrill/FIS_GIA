<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrganizationView.ascx.cs"
            Inherits="Fbs.Web.Controls.OrganizationView" %>

<%@ Register src="~/Controls/OrgFilialsList.ascx" TagName="FilialsList" TagPrefix="fl" %>
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Controls" Assembly="FbsWebUI" %>
<%@ Register TagPrefix="asp" Namespace="WebControls" Assembly="WebControls" %>

<form id="Form1" runat="server">
    <script type="text/javascript" src="/Common/Scripts/jquery.min.js"> </script>
    <script type="text/javascript">
    $(document).ready(function() {
        //Скрываем строки таблиц для Учредителей
        if ($("#<%=this.hiddenKindId.ClientID%>").val() == 6) {
            $(".displayNone").css("display", "none");
        } else {
            $(".displayNone").css("display", "table-row");
        }
    });
</script>
    <asp:HiddenField ID="hiddenKindId" runat="server" />
    <table class="form" style="width: 573px;">
        <tr>
            <td colspan="3">
                <b>
                    <%=this.Message%>
                </b>
            </td>
        </tr>

        <tr>
            <td colspan="3">
                <b>
                    <asp:HyperLink runat="server" ID="EditLink">Перейти к редактированию карточки организации</asp:HyperLink>
                </b>
            </td>
        </tr>

        <tr>
            <td class="left" width="300px">
                Регион
            </td>
            <td class="text" colspan="2" style="vertical-align: middle;">
                <asp:Literal runat="server" ID="LRegion"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="left">
                Уровень
            </td>
            <td colspan="2" class="text" style="vertical-align: middle;">
                <asp:Literal runat="server" ID="LOrgLevel"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="left">
                Тип
            </td>
            <td colspan="2" class="text" style="vertical-align: middle;">
                <asp:Literal runat="server" ID="LOrgKind"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="left">
                Организационно-правовая форма
            </td>
            <td colspan="2" style="vertical-align: middle;" class="text">
                <asp:Literal runat="server" ID="LOPF"></asp:Literal>
            </td>
        </tr>
        <tr class="displayNone">
            <td class="left">
                Является филиалом
            </td>
            <td colspan="2" style="vertical-align: middle;">
                <asp:Literal runat="server" ID="LIsFilial"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="left">
                Статус
            </td>
            <td colspan="2" style="vertical-align: middle;">
                <asp:Literal runat="server" ID="OrgStatusLabel"></asp:Literal>
                <br />
                <asp:Label ID="NewOrgNameLabel" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <asp:PlaceHolder ID="phMainOrgName" runat="server">
            <tr>
                <td class="left">
                    Головная организация
                </td>
                <td colspan="2" style="vertical-align: middle;">
                    <asp:Literal runat="server" ID="lblMainOrgName"></asp:Literal>
                </td>
            </tr>
        </asp:PlaceHolder>
        <tr class="displayNone">
            <td class="left">
                Модель приемной кампании
            </td>
            <td colspan="2" style="vertical-align: top;" class="text">
                <asp:Literal runat="server" ID="lblModelName"></asp:Literal>
            </td>
        </tr>
        <%if (this.Page.User.IsInRole("ViewAdministrationSection"))
        { %>
        <tr>
            <td class="left">
                Отключить журналирование
            </td>
            <td colspan="2" style="vertical-align: middle;" class="text">
                <asp:Literal runat="server" ID="lblIsLogCheckEvent"></asp:Literal>
            </td>
        </tr>
        <% } %>
        <!-- Сведения об объеме и структуре приема -->
        <tr class="displayNone">
            <td colspan="3">
                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                    <tr>
                        <td colspan="2" class="text" style="vertical-align: top; width: 70%; padding-left: 0;">
                            <strong>Сведения об объеме и структуре приема</strong>
                        </td>
                    </tr>
                    <tr>
                        <td class="text" style="vertical-align: top; width: 70%; padding-left: 0;">
                            Общее количество мест обучающихся за счет бюджета по различным формам обучения:
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="text" style="text-align: right; vertical-align: top; width: 70%; padding-left: 0;">
                            Очная
                        </td>
                        <td class="text" style="vertical-align: top; padding-right: 0; padding-left: 20px;">
                            <asp:Literal runat="server" ID="lblFBFullTime" Text=""></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td class="text" style="text-align: right; vertical-align: top; width: 70%; padding-left: 0;">
                            Очно-заочная
                        </td>
                        <td class="text" style="vertical-align: top; padding-right: 0; padding-left: 20px;">
                            <asp:Literal runat="server" ID="lblFBEvening" Text=""></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td class="text" style="text-align: right; vertical-align: top; width: 70%; padding-left: 0;">
                            Заочная
                        </td>
                        <td class="text" style="vertical-align: top; padding-right: 0; padding-left: 20px;">
                            <asp:Literal runat="server" ID="lblFBPostal" Text=""></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td class="text" style="vertical-align: top; width: 70%; padding-left: 0;">
                            Общее количество мест обучающихся на основе договоров с оплатой стоимости обучения,
                            установленное учредителем по различным формам обучения:
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="text" style="text-align: right; vertical-align: top; width: 70%; padding-left: 0;">
                            Очная
                        </td>
                        <td class="text" style="vertical-align: top; padding-right: 0; padding-left: 20px;">
                            <asp:Literal runat="server" ID="lblPayFullTime" Text=""></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td class="text" style="text-align: right; vertical-align: top; width: 70%; padding-left: 0; padding-top: 0;">
                            Очно-заочная
                        </td>
                        <td class="text" style="vertical-align: top; padding-right: 0; padding-left: 20px; padding-top: 0;">
                            <asp:Literal runat="server" ID="lblPayEvening" Text=""></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td class="text" style="text-align: right; vertical-align: top; width: 70%; padding-left: 0; padding-top: 0;">
                            Заочная
                        </td>
                        <td class="text" style="vertical-align: top; padding-right: 0; padding-left: 20px; padding-top: 0;">
                            <asp:Literal runat="server" ID="lblPayPostal" Text=""></asp:Literal>
                        </td>
                    </tr>
                    <tr class="displayNone">
                        <td colspan="2" class="text" style="vertical-align: top; width: 70%; padding-left: 0;">
                            <strong>Детальная информация об ОУ</strong>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <!--  -->
        <tr>
            <td class="left">
                Полное наименование
            </td>
            <td class="text" colspan="2" style="vertical-align: middle;">
                <asp:Literal runat="server" ID="LFullName"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="left">
                Краткое наименование
            </td>
            <td class="text" colspan="2" style="vertical-align: middle;">
                <asp:Literal runat="server" ID="LShortName"></asp:Literal>
            </td>
        </tr>
        <tr class="displayNone">
            <td class="left">
                ИНН
            </td>
            <td class="text" colspan="2" style="vertical-align: middle;">
                <asp:Literal runat="server" ID="LINN"></asp:Literal>
            </td>
        </tr>
        <tr class="displayNone">
            <td class="left">
                ОГРН
            </td>
            <td class="text" colspan="2" style="vertical-align: middle;">
                <asp:Literal runat="server" ID="LOGRN"></asp:Literal>
            </td>
        </tr>
        <tr class="displayNone">
            <td class="left">
                Учредитель (для&nbsp;ССУЗов&nbsp;и&nbsp;ВУЗов)
            </td>
            <td colspan="2" style="vertical-align: middle;">
                <asp:Literal runat="server" ID="LOwnerDepartment"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="left">
                Должность&nbsp;руководителя
            </td>
            <td colspan="2" style="vertical-align: middle;">
                <asp:Literal runat="server" ID="LDirectorPosition"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="left">
                ФИО&nbsp;руководителя
            </td>
            <td colspan="2" style="vertical-align: middle;">
                <asp:Literal runat="server" ID="LDirectorName"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="left">
                Фактический адрес
            </td>
            <td colspan="2" style="vertical-align: middle;">
                <asp:Literal runat="server" ID="LFactAddress"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="left">
                Юридический адрес
            </td>
            <td colspan="2" style="vertical-align: middle;">
                <asp:Literal runat="server" ID="LJurAddress"></asp:Literal>
            </td>
        </tr>
        <tr class="displayNone">
            <td class="left">
                Свидетельство об аккредитации
            </td>
            <td colspan="2" style="vertical-align: middle;" class="text">
                <asp:Literal runat="server" ID="LAccredCert"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="left">
                Код города
            </td>
            <td style="vertical-align: middle;" colspan="2">
                <asp:Literal runat="server" ID="LCityCode"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="left">
                Телефон
            </td>
            <td colspan="2" style="vertical-align: middle;">
                <asp:Literal runat="server" ID="LPhone"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="left">
                Факс
            </td>
            <td colspan="2" style="vertical-align: middle;">
                <asp:Literal runat="server" ID="LFax"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="left">
                E-mail
            </td>
            <td colspan="2" style="vertical-align: middle;">
                <asp:Literal runat="server" ID="LEMail"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td class="left">
                Сайт
            </td>
            <td colspan="2" style="vertical-align: middle;">
                <asp:Literal runat="server" ID="LSite"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td colspan="3" class="box-submit">
            </td>
        </tr>
    </table>
    <table class="form" style="width: 573px;">
        <tr class="displayNone">
            <td class="text" style="vertical-align: top; width: 70%; padding-left: 0;">
                <strong>Филиалы организации</strong>
            </td>
        </tr>
        <tr>
            <td style="text-align: center;">
                <fl:FilialsList runat="server" ID="filialsList" />
            </td>
        </tr>
    </table>
</form>