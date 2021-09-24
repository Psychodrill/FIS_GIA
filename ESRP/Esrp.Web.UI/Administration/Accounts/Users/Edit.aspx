<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Esrp.Web.Administration.Accounts.Users.Edit"
    MasterPageFile="~/Common/Templates/Administration.Master" %>

<%@ Import Namespace="Esrp.Core" %>
<%@ Import Namespace="Esrp.Web" %>
<%--
    Права доступа к странице определяются в web.config секцией: 
    <location path="Administration/Accounts/Users/Edit.aspx">
--%>
<asp:Content runat="server" ContentPlaceHolderID="cphHead">
    <script language="javascript" type="text/javascript">
        $(document).ready(function ()
        {
            removeFirstLastLineFtomTable("#<% =this.cblSystems.ClientID %>");
            removeFirstLastLineFtomTable("#<% =this.BehaviorModelList.ClientID %>");
            removeFirstLastLineFtomTable("#<% =this.rblReceptionOnResultsCNE.ClientID %>");

            var params = {
                changedEl: 'select',
                visRows: 7,
                scrollArrows: true
            };
            cuSel(params);


            if ($("#<%=this.ddlEducationInstitutionType.ClientID%>").val() == "1" || $("#<%=this.ddlEducationInstitutionType.ClientID%>").val() == "2")
            {
                $("#<%=this.RCModelLabel.ClientID%>").show();
                $('#trReceptionOnResultsCNE').show();
            }
            else
            {
                $("#<%=this.RCModelLabel.ClientID%>").hide();
                $('#trReceptionOnResultsCNE').hide();
            }

            var flagNeedDisable = true;
            $("#RCModelsRadioList input").each(function ()
            {
                if ($(this).is(":checked"))
                {
                    if ($(this).attr("value") == "999")
                    {
                        flagNeedDisable = false;
                        $("#<%= AnotherRCModelName.ClientID %>")
                            .removeAttr("disabled")
                            .css("background", "#fff")
                            .css("color", "#333");
                    }
                }
            });
            if (flagNeedDisable)
            {
                DisableDescription();
            }

            // Выбор "Другой" модели приемной кампании
            $("#RCModelsRadioList input").live("click", function ()
            {
                if ($(this).attr("value") != "999")
                {
                    DisableDescription();
                }
                else
                {
                    EnableDescription();
                }
            });

            function removeFirstLastLineFtomTable(idtable)
            {
                $(idtable + ' tr:first td:first').css('border-top-color', '#fff');
                $(idtable + ' tr:last td:first').css('border-bottom-color', '#fff');
            }

            function EnableDescription()
            {
                $("#<%= AnotherRCModelName.ClientID %>")
	            .removeAttr("disabled")
	            .css("background", "#fff")
	            .css("color", "#333")
	            .val("");
            }

            function DisableDescription()
            {
                $("#<%= AnotherRCModelName.ClientID %>")
	            .attr("disabled", "disabled")
	            .css("background", "#ddd")
	            .css("color", "#999")
	            .val("Заполняется в случае выбора модели \"другая модель приема\"");
            }

        });
    </script>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="cphContent">
    <form runat="server">
    <div class="left_col">
        <asp:ValidationSummary CssClass="error_block" ID="ValidationSummary2" runat="server"
            DisplayMode="BulletList" EnableClientScript="false" HeaderText="<p>Произошли следующие ошибки:</p>" />
        <div class="col_in">
            <div class="statement edit">
                <p class="title">
                    Логин/Е-mail&nbsp;<%= CurrentUser.login %></p>
                <p class="back">
                    <a id="BackLink" runat="server" href="#"><span class="un">Вернуться</span></a></p>
                <p class="statement_menu">
                    <a href="#" onclick="$('#<%= btnUpdate.ClientID %>').click();" class="active" title="Сохранить">
                        <span>Сохранить</span></a>
                    <% if (User.IsInRole("ActivateDeactivateUsers"))
                       {
                           if (CurrentUser.CanBeActivated())
                           { %>
                    <a href="/Administration/Accounts/Users/Activate.aspx?Login=<%= Login %>" title="Активировать"
                        class="gray">Активировать</a>
                    <%     }
                           if (CurrentUser.CanBeDeactivated())
                           { %>
                    <a href="/Administration/Accounts/Users/Deactivate.aspx?Login=<%= Login %>" title="Отключить"
                        class="gray">Отключить</a>
                    <%     }
                       } %>
                    <% if (CurrentUser.status != UserAccount.UserAccountStatusEnum.Deactivated)
                       { %>
                    <a href="/Administration/Accounts/Users/RemindPassword.aspx?Login=<%= Login %>" title="Запросить смену пароля"
                        class="gray"><span>Запросить смену пароля</span></a>
                    <% } %>
                    <a href="/Administration/Accounts/Users/ChangePassword.aspx?Login=<%= Login %>" title="Изменить пароль"
                        class="gray"><span>Изменить пароль</span></a> <a href="/Administration/Accounts/Users/History.aspx?Login=<%= Login %>"
                            title="История изменений" class="gray"><span>История изменений</span></a>
                    <a href="/Administration/Accounts/Users/AuthenticationHistory.aspx?Login=<%= Login %>"
                        title="История аутентификаци" class="gray"><span>История аутентификаций</span></a>
                </p>
                <div class="clear">
                </div>
                <div class="statement_table">
                    <table width="100%">
                        <tr>
                            <th>
                                Состояние
                            </th>
                            <td width="1">
                                <b>
                                    <asp:Literal runat="server" ID="litStatus" /></b>
                                <br />
                                <asp:Literal runat="server" ID="litStatusDescription" />
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Номер заявки на регистрацию №
                            </th>
                            <td class="text">
                                <%= (CurrentUser.RequestedOrganization != null ?
									String.Format("<a href=\"/Administration/Requests/RequestForm.aspx?RequestID={0}\" title=\"Перейти на карточку заявки\">{0}</a>", CurrentUser.RequestedOrganization.Id) : 
									"отсутствует")  %>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Полное наименование организации
                            </th>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="border-bottom: 0px; border-top: 0px; padding-right: 5px;">
                                            <asp:HyperLink runat="server" Visible="false" ID="OrganizationName_hyperlynk"></asp:HyperLink>
                                            <asp:TextBox runat="server" ID="OrganizationName_txt" CssClass="txt long" MaxLength="1000" />
                                            <asp:HiddenField runat="server" ID="OrganizationName_hiddenField" />
                                        </td>
                                        <td width="100px" style="border-bottom: 0px; border-top: 0px; vertical-align: top;">
                                            <asp:Button ID="btnChangeOrg" runat="server" Text="Выбрать" Width="100px" ToolTip="Выбор организации" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="OrganizationName_txt"
                            EnableClientScript="false" Display="None" ID="OrganizationName_txt_validator"
                            ErrorMessage='Поле "Полное наименование организации" обязательно для заполнения' />
                        <tr>
                            <th>
                                Краткое наименование организации
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="TBShortName" Rows="5" Height="100" MaxLength="500"
                                    TextMode="MultiLine" CssClass="textareaoverflowauto" />
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Доступ к системе
                            </th>
                            <td>
                                <asp:CheckBoxList runat="server" ID="cblSystems" AppendDataBoundItems="true" DataValueField="SystemID"
                                    DataTextField="Name" CssClass="checkbox-box-list selects-systems" />
                                <asp:CustomValidator runat="server" ID="cvSystems" EnableClientScript="false" Display="None"
                                    ErrorMessage='Выберите хотя бы одну информационную систему' />
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Группы
                            </th>
                            <td>
                                <asp:CheckBoxList runat="server" ID="chblGroup" CssClass="selects-groups" AppendDataBoundItems="true"
                                    DataValueField="Code" DataTextField="Name">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Ф.И.О. лица, ответственного за работу с системами
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtFullName" CssClass="txt long" MaxLength="255" />
                            </td>
                        </tr>
                        <asp:RequiredFieldValidator runat="server" ID="VReqUserName" ControlToValidate="txtFullName"
                            EnableClientScript="false" Display="None" ErrorMessage='Поле "Ф.И.О. лица, ответственного за работу с {0}" обязательно для заполнения' />
                        <tr>
                            <th>
                                Телефон лица, ответственного за работу с системами
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtPhone" CssClass="txt small" MaxLength="255" />
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Должность лица, ответственного за работу с системами
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtPosition" CssClass="txt long" />
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Заявка на регистрацию
                            </th>
                            <td>
                                <%= (IsRegistrationDocumentExists ? String.Format("<a target=_blank href=\"/Profile/ConfirmedDocumentView.aspx?login={0}\" title=\"Просмотр заявки на регистрацию\">просмотр</a>", HttpUtility.UrlEncode(CurrentUser.login)) : "не загружен")  %>
                                <div runat="server" id="divChangeDoc">
                                    <asp:FileUpload ID="fuRegistrationDocument" runat="server" CssClass="long" Width="530" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Субъект Российской Федерации, на территории которого находится организация
                            </th>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlOrganizationRegion" AppendDataBoundItems="true"
                                    CssClass="sel long" DataValueField="Id" DataTextField="Name">
                                    <asp:ListItem Value="0">&lt;Не задано&gt;</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlOrganizationRegion"
                            EnableClientScript="false" Display="None" ErrorMessage='Поле "Субъект Российской Федерации" обязательно для заполнения' />
                        <tr>
                            <th>
                                Тип ОУ
                            </th>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlEducationInstitutionType" CssClass="sel long"
                                    AppendDataBoundItems="true" DataValueField="Id" DataTextField="Name">
                                    <asp:ListItem Value="">&lt;Не задано&gt;</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlEducationInstitutionType"
                            EnableClientScript="false" Display="None" ErrorMessage='Поле "Тип ОУ" обязательно для заполнения' />
                        <tr>
                            <th>
                                Вид ОУ
                            </th>
                            <td>
                                <asp:DropDownList runat="server" ID="DLLOrgKind" CssClass="sel small" AppendDataBoundItems="true"
                                    DataValueField="Id" DataTextField="Name">
                                    <asp:ListItem Value="">&lt;Не задано&gt;</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="VReqOrgKind" ControlToValidate="DLLOrgKind"
                                    ErrorMessage='Поле "Вид ОУ" обязательно для заполнения' Display="None" EnableClientScript="false"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Организационно-правовая форма ОУ
                            </th>
                            <td>
                                <asp:DropDownList runat="server" ID="DDLOPF" CssClass="sel small">
                                    <asp:ListItem Value="0">Государственный</asp:ListItem>
                                    <asp:ListItem Value="1">Негосударственный</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Является филиалом
                            </th>
                            <td>
                                <asp:CheckBox runat='server' ID="ChIsFilial" />
                            </td>
                        </tr>
                        <tr>
                            <th>
                                ОГРН
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="tbOGRN" CssClass="txt small" MaxLength="13" />
                                <asp:RequiredFieldValidator ID="rfvOGRN" runat="server" ControlToValidate="tbOGRN"
                                    EnableClientScript="false" Display="None" ErrorMessage='Поле "ОГРН" обязательно для заполнения' />
                                <asp:RegularExpressionValidator runat="server" ID="revOGRN" ValidationExpression="[0-9]{13}"
                                    ControlToValidate="tbOGRN" EnableClientScript="false" Display="None" ErrorMessage='Поле "ОГРН" заполнено неверно' />
                            </td>
                        </tr>
                        <tr>
                            <th>
                                ИНН
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="tbINN" CssClass="txt small" MaxLength="10" />
                                <asp:RequiredFieldValidator ID="rfvINN" runat="server" ControlToValidate="tbINN"
                                    EnableClientScript="false" Display="None" ErrorMessage='Поле "ИНН" обязательно для заполнения' />
                                <asp:RegularExpressionValidator runat="server" ID="revINN" ValidationExpression="[0-9]{10}"
                                    ControlToValidate="tbINN" EnableClientScript="false" Display="None" ErrorMessage='Поле "ИНН" заполнено неверно' />
                            </td>
                        </tr>
                        <tr>
                            <th>
                                КПП
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="tbKPP" CssClass="txt small" MaxLength="9" />
                                <asp:RegularExpressionValidator ID="revKPP" runat="server" ControlToValidate="tbKPP"
                                    EnableClientScript="false" Display="None" ErrorMessage='Поле "КПП" должно содержать 9 цифр'
                                    ValidationExpression="[0-9]{9}"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Учредитель (для&nbsp;ССУЗов&nbsp;и&nbsp;ВУЗов)
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtOrganizationFounderName" CssClass="txt long" MaxLength="255" />
                            </td>
                        </tr>
                        <asp:RequiredFieldValidator runat="server" ID="VReqFounderName" ControlToValidate="txtOrganizationFounderName"
                            EnableClientScript="false" Display="None" ErrorMessage='Поле "Учредитель" обязательно для заполнения' />
                        <tr runat="server" id="RCModelLabel">
                            <td colspan="2">
                                <strong>Модель приемной кампании: <span style="color: rgb(215, 5, 5) !important;">
                                    <%=this.Page.Required()%></span> </strong>
                                <br />
                                <div id="RCModelsRadioList" class="tablenoborder">
                                    <asp:RadioButtonList runat="server" ID="BehaviorModelList" DataSourceID="RCModelsList"
                                        DataValueField="Id" DataTextField="ModelName" CssClass="radio-button-list" />
                                    <asp:TextBox runat="server" ID="AnotherRCModelName" CssClass="radio-button-list-text"
                                        Rows="3" Height="50px" TextMode="MultiLine" MaxLength="400" Width="99%" />
                                </div>
                                <asp:RequiredFieldValidator ID="RCModelvalidator" runat="server" ControlToValidate="BehaviorModelList"
                                    Display="Dynamic" EnableClientScript="false" ErrorMessage="Должна быть выбрана модель приемной кампании."
                                    Text="*" />
                            </td>
                        </tr>
                        <%--Прием по результатам ЕГЭ--%>
                        <tr id="trReceptionOnResultsCNE" visible="false">
                            <th>
                                Прием по результатам ЕГЭ (для&nbsp;ССУЗов&nbsp;и&nbsp;ВУЗов)
                            </th>
                            <td>
                                <asp:RadioButtonList ID="rblReceptionOnResultsCNE" runat="server" CssClass="radio-button-list">
                                    <asp:ListItem Text="Проводится" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Не проводится" Value="1"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Должность руководителя организации
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="TbDirectorPosition" CssClass="txt long" MaxLength="255" />
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Ф.И.О. руководителя организации
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtOrganizationChiefName" CssClass="txt long" MaxLength="255" />
                            </td>
                        </tr>
                        <asp:RequiredFieldValidator ID="VReqDirectorName" runat="server" ControlToValidate="txtOrganizationChiefName"
                            EnableClientScript="false" Display="None" ErrorMessage='Поле "ФИО руководителя организации" обязательно для заполнения' />
                        <tr>
                            <th>
                                Фактический адрес
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="TbFactAddress" CssClass="txt long" MaxLength="255" />
                                <asp:RequiredFieldValidator ID="VReqFactAddress" runat="server" ControlToValidate="TbFactAddress"
                                    EnableClientScript="false" Display="None" ErrorMessage='Поле "Фактический адрес" обязательно для заполнения' />
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Юридический адрес
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtOrganizationAddress" CssClass="txt long" MaxLength="255" />
                            </td>
                        </tr>
                        <asp:RequiredFieldValidator runat="server" ID="VReqLawAddress" ControlToValidate="txtOrganizationAddress"
                            EnableClientScript="false" Display="None" ErrorMessage='Поле "Юридический адрес" обязательно для заполнения' />
                        <tr>
                            <th>
                                Свидетельство об аккредитации
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="TBAccred" CssClass="txt long" MaxLength="255" />
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Код города
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="TbPhoneCode" CssClass="txt small" MaxLength="10" />
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Телефон
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtOrganizationPhone" CssClass="txt small" MaxLength="100" />
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Факс
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtOrganizationFax" CssClass="txt small" MaxLength="100" />
                            </td>
                        </tr>
                        <tr>
                            <th>
                                EMail
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="TBEMail" CssClass="txt small" MaxLength="100" />
                                <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" ControlToValidate="TBEMail"
                                    EnableClientScript="false" Display="None" ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$"
                                    ErrorMessage='Поле "EMail" заполнено неверно' />
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Сайт
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="TBSite" CssClass="txt small" MaxLength="40" />
                            </td>
                        </tr>
                    </table>
                </div>
                <p>
                    <asp:Button runat="server" ID="BtAddToOrgs" Text="Добавить организацию" CssClass="bt"
                        OnClick="BtAddToOrgs_Click" />
                    <asp:Button runat="server" ID="btnUpdate" Text="Сохранить" CssClass="bt" OnClick="btnUpdate_Click" />
                </p>
            </div>
        </div>
    </div>
    <input type="hidden" name="state" />
    <script language="javascript" type="text/javascript">
        InitConfirmation('', '<%= Request.Form["state"] %>');
        $(document).ready(function ()
        {
            ToggleGroupsInput();
            $(".selects-systems input[type=checkbox]").click(function ()
            {
                ToggleGroupsInput();
            });
        });
        function ToggleGroupsInput()
        {
            //Некрасиво, но CheckBoxList не отображает value
            var fisChecked = false;
            $(".selects-systems input[type=checkbox]:checked + label").each(function (index, element)
            {
                if ($(element).text() == "ФИС ЕГЭ и приема")
                {
                    fisChecked = true;
                }
            });

            if (fisChecked)
            {
                $(".selects-groups").closest("tr").show();
            }
            else
            {
                $(".selects-groups").closest("tr").hide();
            }
        }
    </script>
    </form>
    <asp:SqlDataSource runat="server" ID="RCModelsList" ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>"
        SelectCommand="SELECT Id, ModelName FROM [dbo].[RecruitmentCampaigns] ORDER BY [Id]" />
</asp:Content>
