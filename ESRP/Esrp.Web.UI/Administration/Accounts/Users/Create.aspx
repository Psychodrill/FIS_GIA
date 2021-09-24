<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Create.aspx.cs" Inherits="Esrp.Web.Administration.Accounts.Users.Create"
    MasterPageFile="~/Common/Templates/Administration.Master" %>

<%@ Import Namespace="Esrp.Core" %>
<%@ Import Namespace="Esrp.Core.Systems" %>
<%@ Import Namespace="Esrp.Web" %>
<%--
    Права доступа к странице определяются в web.config секцией: 
    <location path="Administration/Accounts/Users/Create.aspx">
--%>
<asp:Content runat="server" ContentPlaceHolderID="cphHead">
    <script src="/Common/Scripts/Confirmation.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        jQuery(document).ready(function () {
            var params = {
                changedEl: 'select',
                visRows: 7,
                scrollArrows: true
            }
            cuSel(params);
        });
    </script>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="cphContent">    
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {

            if ($("#<%=this.hiddenOrgTypes.ClientID%>").val() == "1" || $("#<%=this.hiddenOrgTypes.ClientID%>").val() == "2") {
                $("#<%=this.RCModelLabel.ClientID%>").show();
            }
            else {
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
    <div class="left_col">
        <form ID="CreateForm" runat="server">
        <asp:HiddenField runat="server" ID="hiddenOrgTypes" />
        <asp:ValidationSummary CssClass="error_block" runat="server" DisplayMode="BulletList"
            EnableClientScript="false" HeaderText="<p>Произошли следующие ошибки:</p>" />
        <asp:CustomValidator runat="server" ID="customErrors" EnableClientScript="false" Display="None"></asp:CustomValidator>
        <div class="col_in">
            <div class="statement edit">
                <p class="title">
                    Новый пользователь</p>
                <p class="back">
                    <a id="BackLink" runat="server" href="#"><span class="un">Вернуться</span></a></p>
                <p class="statement_menu">
                    <a href="#" class="active" onclick="$('#<%= btnUpdate.ClientID %>').click();" handleclick="false">
                        <span>Сохранить</span>
                    </a>
                </p>
                <div class="clear">
                </div>
                <div class="statement_table">
                    <table style="width: 100%;">
                        <tr>
                            <th>
                                Логин/E-mail
                            </th>
                            <td width="1">
                                <asp:TextBox runat="server" ID="txtEmail" CssClass="txt small" />
                            </td>
                        </tr>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEmail"
                            EnableClientScript="false" Display="None" ErrorMessage='Поле "Логин/E-mail" обязательно для заполнения' />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail"
                            EnableClientScript="false" Display="None" ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*\.{0,1}@([0-9a-zA-Z]*[-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$"
                            ErrorMessage='Поле "Логин/E-mail" заполнено неверно' />
                        <tr>
                            <th>
                                Полное наименование организации
                            </th>
                            <td id="organizationNameParent">
                                <table width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="border-bottom: 0px; border-top: 0px;">
                                            <asp:TextBox runat="server" ID="txtOrganizationName" CssClass="textareaoverflowauto"
                                                autocomplete="off" Width="400px" TextMode="MultiLine" Rows="5" Height="100px"
                                                MaxLength="1000" Enabled="false" />
                                        </td>
                                        <td style="border-bottom: 0px; border-top: 0px; vertical-align: top;">
                                            <asp:Button ID="btnChangeOrg" runat="server" PostBackUrl="/SelectOrg.aspx?BackUrl=./Administration/Accounts/Users/Create.aspx"
                                                Text="Выбрать" Width="100px" ToolTip="Выбор организации" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtOrganizationName"
                            EnableClientScript="false" Display="None" ErrorMessage='Поле "Полное наименование организации" обязательно для заполнения' />
                        <tr>
                            <th colspan="2">
                                &nbsp;
                            </th>
                        </tr>
                        <tr>
                            <th>
                                Доступ к системе
                            </th>
                            <td class="text">
                                <asp:CheckBoxList runat="server" ID="cblSystems" AppendDataBoundItems="true" DataValueField="SystemID"
                                    DataTextField="Name" CssClass="checkbox-box-list selects-systems" />
                                <asp:CustomValidator runat="server" ID="cvSystems" EnableClientScript="false" Display="None"
                                    ErrorMessage='Выберите хотя бы одну информационную систему' />
                            </td>
                        </tr>
                        <tr>
                        <th>Группы</th>
                        <td>
                                <asp:CheckBoxList runat="server" ID="chblGroup" CssClass="selects-groups" AppendDataBoundItems="true" DataValueField="Code"
                                    DataTextField="Name"> 
                                </asp:CheckBoxList>
                        </td>
                        </tr>
                        <tr>
                            <th>
                                Субъект Российской Федерации, на территории которого находится организация
                            </th>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlOrganizationRegion" AppendDataBoundItems="true"
                                    CssClass="sel long" DataSourceID="dsRegion" DataValueField="RegionId" DataTextField="Name"
                                    Enabled="false">
                                    <asp:ListItem Value="">&lt;Не задано&gt;</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlOrganizationRegion"
                            EnableClientScript="false" Display="None" ErrorMessage='Поле "Субъект Российской Федерации" обязательно для заполнения' />
                        <tr>
                            <th>
                                Учредитель (для&nbsp;ссузов&nbsp;и&nbsp;вузов)
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtOrganizationFounderName" CssClass="txt long" Enabled="false" />
                            </td>
                        </tr>
                        <tr runat="server" id="RCModelLabel">
                            <td class="left" colspan="2">
                                <strong>Модель приемной кампании:</strong>
                                <span style="color:  rgb(215, 5, 5) !important;"><%=this.Page.Required()%></span>
                                <asp:RequiredFieldValidator ID="RCModelvalidator" runat="server" ControlToValidate="BehaviorModelList"
                                    Display="Dynamic" EnableClientScript="false" ErrorMessage="Должна быть выбрана модель приемной кампании."
                                    Text="*" />
                                <div class="tablenoborder" id="RCModelsRadioList">
                                    <asp:RadioButtonList runat="server" ID="BehaviorModelList" DataSourceID="RCModelsList"
                                        DataValueField="Id" DataTextField="ModelName" CssClass="radio-button-list"    />
                                    <asp:TextBox runat="server" ID="AnotherRCModelName" CssClass="radio-button-list-text" Rows="3"
                                        Height="50px" Width="99%" TextMode="MultiLine" MaxLength="400" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Адрес
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtOrganizationAddress" CssClass="txt long" />
                            </td>
                        </tr>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtOrganizationAddress"
                            EnableClientScript="false" Display="None" ErrorMessage='Поле "Адрес" обязательно для заполнения' />
                        <tr>
                            <th>
                                Ф. И. О. руководителя организации
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtOrganizationChiefName" CssClass="txt long" />
                            </td>
                        </tr>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtOrganizationChiefName"
                            EnableClientScript="false" Display="None" ErrorMessage='Поле "ФИО руководителя организации" обязательно для заполнения' />
                        <tr>
                            <th>
                                Факс
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtOrganizationFax" CssClass="txt small" />
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Телефон (с указанием кода города) руководителя организации
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtOrganizationPhone" CssClass="txt small" />
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Ф. И. О. лица, ответственного за работу с
                                <%= GeneralSystemManager.GetSystemName(2) %>
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtFullName" CssClass="txt long" />
                            </td>
                        </tr>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFullName" EnableClientScript="false"
                            Display="None" ID="rvFbs1" ErrorMessage='Поле "Ф.И.О. лица, ответственного за работу с {0}" обязательно для заполнения' />
                        <tr>
                            <th>
                                Телефон (с указанием кода города) лица, ответственного за работу с
                                <%= GeneralSystemManager.GetSystemName(2) %>
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtPhone" CssClass="txt small" />
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Должность лица, ответственного за работу с
                                <%= GeneralSystemManager.GetSystemName(2) %>
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
                                <asp:FileUpload ID="fuRegistrationDocument" runat="server" class="long" Width="532" size="61"/>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="border-bottom: 0px">
                                <asp:Button runat="server" ID="btnUpdate" Text="Сохранить" OnClick="btnUpdate_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <input type="hidden" name="state" />
            </div>
        </div>
        </form>
    </div>
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
    <asp:SqlDataSource runat="server" ID="RCModelsList" ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>"
        SelectCommand="SELECT Id, ModelName FROM [dbo].[RecruitmentCampaigns] ORDER BY [Id]" />
    <asp:SqlDataSource runat="server" ID="dsRegion" ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString%>"
        SelectCommandType="StoredProcedure" SelectCommand="dbo.SearchRegion" />
</asp:Content>
