<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserProfileEdit.ascx.cs"
    Inherits="Esrp.Web.Controls.UserProfileEdit" %>
<%@ Import Namespace="Esrp.Core.Systems" %>
<%@ Import Namespace="Esrp.Web" %>
<script type="text/javascript" src="/Common/Scripts/jquery-1.6.1.min.js"></script>
<script type="text/javascript" src="/Common/Scripts/jquery-ui-1.8.18.min.js"></script>
<script type="text/javascript" src="/Common/Scripts/cusel-min-2.5.js"></script>
<script type="text/javascript" src="/Common/Scripts/js.js"></script>
<script type="text/javascript">
    function removeFirstLastLineFtomTable(idtable) {
        $(idtable + ' tr:first td:first').css('border-top-color', '#fff');
        $(idtable + ' tr:last td:first').css('border-bottom-color', '#fff');
    }

    $(document).ready(function () {
        var params = {
            changedEl: 'select',
            visRows: 7,
            scrollArrows: true
        }
        cuSel(params);

        removeFirstLastLineFtomTable("#<% =this.BehaviorModelList.ClientID %>");
        removeFirstLastLineFtomTable("#<% =this.rblReceptionOnResultsCNE.ClientID %>");

        $("#<%=ddlOrgType.ClientID %>").change(function () {
            if ($("#<%=ddlOrgType.ClientID %>").val() == 1 || $("#<%=ddlOrgType.ClientID %>").val() == 2) {
                $('#trReceptionOnResultsCNE').show();
            }
            else {
                $('#trReceptionOnResultsCNE').hide();
            }
        });


        if ($("#<%=this.ddlOrgType.ClientID%>").val() == "1" || $("#<%=this.ddlOrgType.ClientID%>").val() == "2") {
            $('#trReceptionOnResultsCNE').show();
            $("#<%=this.RCModelLabel.ClientID%>").show();
        }
        else {
            $('#trReceptionOnResultsCNE').hide();
            $("#<%=this.RCModelLabel.ClientID%>").hide();
        }

        var flagNeedDisable = true;
        $("#RCModelsRadioList input").each(function () {
            if ($(this).is(":checked")) {
                if ($(this).attr("value") == "999") {
                    flagNeedDisable = false;
                    $("#<%= AnotherRCModelName.ClientID %>")
                        .removeAttr("disabled")
                        .css("background", "rgb(235,243,246)")
                        .css("color", "#333");
                }
            }
        });
        if (flagNeedDisable) {
            DisableDescription();
        }

        // Выбор "Другой" модели приемной кампании
        $("#RCModelsRadioList input").live("click", function () {
            if ($(this).attr("value") != "999") {
                DisableDescription();
            }
            else {
                EnableDescription();
            }
        });

        function EnableDescription() {
            $("#<%= AnotherRCModelName.ClientID %>")
                .removeAttr("disabled")
                .css("background", "rgb(235,243,246)")
                .css("color", "#333")
                .val("");
        }

        function DisableDescription() {
            $("#<%= AnotherRCModelName.ClientID %>")
                .attr("disabled", "disabled")
                .css("background", "#ddd")
                .css("color", "#999")
                .val("Заполняется в случае выбора модели \"другая модель приема\"");
        }

    });
</script>
<% if (CurrentUser.CanEdit)
   { %>
<asp:ValidationSummary CssClass="error_block" runat="server" DisplayMode="BulletList"
    ID="vsErrors" EnableClientScript="false" ValidationGroup="UserProfile" HeaderText="<p>Произошли следующие ошибки:</p>" />
<div class="left_col">
    <div class="statement_table">
        <table width="100%">
            <tr>
                <th style="border-top-color: #fff;">
                    Логин/E-mail
                </th>
                <td width="1"  style="border-top-color: #fff;">
                    <asp:Literal runat="server" ID="litUserName" />
                </td>
            
            </tr>
            <tr>
                <th>
                    Текущий шаг регистрации
                </th>
                <td colspan="2">
                    <b>
                        <asp:Literal runat="server" ID="litStatus" /></b><br />
                    <asp:Literal runat="server" ID="litStatusDescription" />
                </td>
            </tr>
            <tr>
                <th>
                    Состояние
                </th>
                <td>
                    <b>
                        <asp:Literal runat="server" ID="litNewStatus" /></b><br />
                </td>
          
            </tr>
            <tr>
                <th>
                    Полное&nbsp;наименование&nbsp;организации<br />
                    (без организационно-правовой формы)
                </th>
                <td>
                    <table  cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="border-bottom: 0px; border-top: 0px; padding-right: 10PX;">
                                <asp:TextBox runat="server" ID="txtOrganizationName" CssClass="textareaoverflowauto" TextMode="MultiLine"
                                    Height="60px" Width="410" />
                                <asp:HiddenField runat="server" ID="hfEtalonOrgID" />
                            </td>
                            <td style="border-bottom: 0px; border-top: 0px; vertical-align: top;">
                                <asp:Button ID="btnChangeOrg" runat="server" Text="Выбрать" Width="100px" ToolTip="Выбор организации" />
                            </td>
                        </tr>
                    </table>
            </tr>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtOrganizationName"
                EnableClientScript="false" Display="None" ValidationGroup="UserProfile" ErrorMessage='Поле "Полное наименование организации" обязательно для заполнения' />
            <tr>
                <th>
                    <span>Тип организации</span>
                </th>
                <td colspan="2">
                    <asp:DropDownList runat="server" ID="ddlOrgType" CssClass="sel small" DataTextField="Name"
                        DataValueField="Id">
                        <asp:ListItem Value="">&lt;Не задано&gt;</asp:ListItem>
                        <asp:ListItem Value="0">&lt;Не определён&gt;</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlOrgType"
                EnableClientScript="false" Display="None" ValidationGroup="UserProfile" ErrorMessage='Поле "Тип организации" обязательно для заполнения' />
            <tr>
                <th>
                    Субъект Российской Федерации, на территории которого находится организация
                </th>
                <td colspan="2">
                    <asp:DropDownList runat="server" ID="ddlOrganizationRegion" AppendDataBoundItems="true"
                        CssClass="sel small" DataValueField="Id" DataTextField="Name">
                        <asp:ListItem Value="">&lt;Не задано&gt;</asp:ListItem>
                        <asp:ListItem Value="0">&lt;Не установлен&gt;</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlOrganizationRegion"
                EnableClientScript="false" Display="None" ValidationGroup="UserProfile" ErrorMessage='Поле "Субъект Российской Федерации" обязательно для заполнения' />
            <tr>
                <th>
                    Учредитель (для ССУЗов, ВУЗов и РЦОИ)
                </th>
                <td colspan="2">
                    <asp:TextBox runat="server" ID="txtOrganizationFounderName" CssClass="txt long" />
                </td>
            </tr>
            <tr>
                <th>
                    Юридический адрес
                </th>
                <td colspan="2">
                    <asp:TextBox runat="server" ID="txtOrganizationAddress" CssClass="txt long" />
                </td>
            </tr>
            <tr runat="server" id="RCModelLabel">
                <td colspan="3">
                    <strong>Модель приемной кампании: <span style="color: rgb(215, 5, 5) !important;">
                        <%=this.Page.Required()%></span></strong>
                     <br />
                    <asp:RequiredFieldValidator ID="RCModelvalidator" runat="server" ControlToValidate="BehaviorModelList"
                        Display="Dynamic" EnableClientScript="false" ErrorMessage="Должна быть выбрана модель приемной кампании."
                        Text="*" />
                    <div id="RCModelsRadioList" class="tablenoborder">
                        <asp:RadioButtonList runat="server" Width="950" ID="BehaviorModelList" DataSourceID="RCModelsList"
                            DataValueField="Id" DataTextField="ModelName" CssClass="radio-button-list"/>
                        <asp:TextBox runat="server" ID="AnotherRCModelName" CssClass="textareaoverflowauto" Rows="3"
                            Height="50px" Width="99%" TextMode="MultiLine" MaxLength="400" />
                    </div>
                </td>
            </tr>
            <%--Прием по результатам ЕГЭ--%>
            <tr id="trReceptionOnResultsCNE" visible="false">
                <th>
                    Прием по результатам ЕГЭ (для&nbsp;ССУЗов&nbsp;и&nbsp;ВУЗов)
                </th>
                <td style="padding-left: 0; padding-bottom: 0; padding-top: 0;" colspan="2">
                    <asp:RadioButtonList ID="rblReceptionOnResultsCNE" runat="server" CssClass="radio-button-list">
                        <asp:ListItem Text="Проводится" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Не проводится" Value="1"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtOrganizationAddress"
                EnableClientScript="false" Display="None" ValidationGroup="UserProfile" ErrorMessage='Поле "Адрес" обязательно для заполнения' />
            <tr>
                <th>
                    Ф. И. О. руководителя организации
                </th>
                <td colspan="2">
                    <asp:TextBox runat="server" ID="txtOrganizationChiefName" CssClass="txt long" />
                </td>
            </tr>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtOrganizationChiefName"
                EnableClientScript="false" Display="None" ValidationGroup="UserProfile" ErrorMessage='Поле "ФИО руководителя организации" обязательно для заполнения' />
            <tr>
                <th>
                    Телефон (с указанием кода города) руководителя организации
                </th>
                <td colspan="2">
                    <asp:TextBox runat="server" ID="txtOrganizationPhone" CssClass="txt small" />
                </td>
            </tr>
            <tr>
                <th>
                    Факс
                </th>
                <td colspan="2">
                    <asp:TextBox runat="server" ID="txtOrganizationFax" CssClass="txt small" />
                </td>
            </tr>
            <tr>
                <th>
                    ОГРН<span style="color: rgb(215, 5, 5) !important;"><%=this.Page.Required()%></span>
                </th>
                <td colspan="2">
                    <asp:TextBox runat="server" ID="tbOGRN" CssClass="txt small" MaxLength="13" />
                    <asp:RequiredFieldValidator ID="rfvOGRN" runat="server" ControlToValidate="tbOGRN"
                        EnableClientScript="false" Text="*" Display="Dynamic" ErrorMessage='Поле "ОГРН" обязательно для заполнения' />
                    <asp:RegularExpressionValidator ID="revOGRN" runat="server" ControlToValidate="tbOGRN"
                        EnableClientScript="false" Text="*" Display="Dynamic" ErrorMessage='Поле "ОГРН" заполнено неверно'
                        ValidationExpression="[0-9]{13}"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <th>
                    ИНН<span style="color: rgb(215, 5, 5) !important;"><%=this.Page.Required()%></span>
                </th>
                <td colspan="2">
                    <asp:TextBox runat="server" ID="tbINN" CssClass="txt small" MaxLength="10" />
                    <asp:RequiredFieldValidator ID="rfvINN" runat="server" ControlToValidate="tbINN"
                        EnableClientScript="false" Text="*" Display="Dynamic" ErrorMessage='Поле "ИНН" обязательно для заполнения' />
                    <asp:RegularExpressionValidator ID="revINN" runat="server" ControlToValidate="tbINN"
                        EnableClientScript="false" Text="*" Display="Dynamic" ErrorMessage='Поле "ИНН" заполнено неверно'
                        ValidationExpression="[0-9]{10}"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <th>
                    КПП
                </th>
                <td colspan="2">
                    <asp:TextBox runat="server" ID="tbKPP" CssClass="txt long" MaxLength="9" />
                    <asp:RegularExpressionValidator ID="revKPP" runat="server" ControlToValidate="tbKPP"
                        ValidationGroup="UserProfile" EnableClientScript="false" Display="None" ErrorMessage='Поле "КПП" должно содержать 9 цифр'
                        ValidationExpression="[0-9]{9}"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <th>
                    Ф. И. О. лица, ответственного за работу с
                    <asp:Label runat="server" ID="lblSystemNamesForFio" />
                </th>
                <td colspan="2">
                    <asp:TextBox runat="server" ID="txtFullName" CssClass="txt long" />
                </td>
            </tr>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFullName" ID="vldFullName"
                EnableClientScript="false" Display="None" ValidationGroup="UserProfile" ErrorMessage='Поле "Ф.И.О. лица, ответственного за работу с {0}" обязательно для заполнения' />
            <tr>
                <th>
                    Телефон (с указанием кода города) лица, ответственного за работу с
                    <asp:Label runat="server" ID="lblSystemNamesForPhone" />
                </th>
                <td colspan="2">
                    <asp:TextBox runat="server" ID="txtPhone" CssClass="txt small" />
                </td>
            </tr>
            <tr>
                <td colspan="2" class="box-submit" style="border-bottom-color: #fff;">
                    <asp:Button runat="server" ID="btnUpdate" Text="Сохранить" CssClass="bt" OnClick="btnUpdate_Click"
                        ValidationGroup="UserProfile" />
                </td>
            </tr>
        </table>
    </div>
</div>
<input type="hidden" name="state" />
<script language="javascript" type="text/javascript">
    InitConfirmation('', '<%= Request.Form["state"] %>');
</script>
<% }
   else
   { %>
<p>
    Вы не имеете прав для редактирования регистрационных данных.<br />
    Перейдите на страницу просмотра <a href="/Profile/View.aspx" title="Регистрационные данные">
        регистрационных данных</a>.</p>
<% } %>
<asp:SqlDataSource runat="server" ID="RCModelsList" ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>"
    SelectCommand="SELECT Id, ModelName FROM [dbo].[RecruitmentCampaigns] ORDER BY [Id]" />
