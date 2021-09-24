<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrganizationView.ascx.cs"
    Inherits="Esrp.Web.Controls.OrganizationView" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Import Namespace="Esrp.Core.Systems" %>
<%@ Import Namespace="Esrp.Core.Organizations" %>
<%--<form id="Form1" runat="server">--%>
<script type="text/javascript" src="/Common/Scripts/jquery-1.6.1.min.js"> </script>
<script type="text/javascript" src="/Common/Scripts/cusel-min-2.5.js"></script>
<script type="text/javascript">
        <% var arr = OrganizationDataAccessor.GetDirectorPositionsInGenetive(); %>

        // Собираю js массив с должностями руководителя
        var positions = [
                <% var i = 1;
                   foreach (var pos in arr)
                   {%>
                        { name: '<%= pos.DirectorPositionName %>', nameInGenetive: '<%= pos.DirectorPositionNameInGenetive %>', id: <%= pos.Id - 1 %> }
                        <% if (i != arr.Count) { %>
                                ,
                        <% } %>
                 <% i++;
                   } %>
            ];

        $(document).ready(function () {
            var selParams = {
                changedEl: 'select',
                visRows: 7,
                scrollArrows: true
            }
            cuSel(selParams); 

            //Скрываем строки таблиц для Учредителей
            if ($("#<%=this.hiddenKindId.ClientID%>").val() == 6) {
                $(".displayNone").css("display", "none");
            } else {
                $(".displayNone").css("display", "table-row");
            }

            if ($("#<%=this.hfIsChangeStatus.ClientID%>").val() == false) {
                $('#dateChangeStatus').hide();
                $('#reasonChangeStatus').hide();
            }
            
            // Выбор "Другой" модели приемной кампании
            $("#tblRecruitmentCampaigns input").live("click", function () {
                if ($(this).attr("value") != "999") {
                    DisableDescription();
                }
                else {
                    EnableDescription();
                }
            });

            if ($("#<%= txtBxDescription.ClientID %>").attr("disabled")) {
                DisableDescription();
            }

            function EnableDescription() {
                $("#<%= txtBxDescription.ClientID %>")
                    .removeAttr("disabled")
                    .css("background", "rgb(235,243,246)")
                    .css("color", "#333")
                    .val("");
            }

            function DisableDescription() {
                $("#<%= txtBxDescription.ClientID %>")
                    .attr("disabled", "disabled")
                    .css("background", "#ddd")
                    .css("color", "#999")
                    .val("Заполняется в случае выбора модели \"другая модель приема\"");
            }

            // Функция вставляющая название долности в дат. падеже
            function onDirectorPosSelected() {
                var selectedName = $(this).html();

                $('#<%=this.HDNComboBoxDirectorPosition.ClientID%>').val(selectedName);
                
                $.each(positions, function () {
                    if (this.name == $.trim(selectedName)) {
                        $('#<%=this.TBDirectorPosInGenetive.ClientID%>').attr('value', this.nameInGenetive);
                    }
                });
            }
            
            // Вешаю событие на ДД, чтобы по выбору элемента в текстбоксе выводилось в дат. падеже
            if ($('#<%=this.ComboBoxDirectorPosition.ClientID%>').next().next().find('li').size() > 0) {
                $('#<%=this.ComboBoxDirectorPosition.ClientID%>').next().next().find('li').bind('mousedown', onDirectorPosSelected);
            } else {
                // Хак для IE7
                $('#<%=this.ComboBoxDirectorPosition.ClientID%>').find('.ajax__combobox_itemlist li').size();
                $('#<%=this.ComboBoxDirectorPosition.ClientID%>').find('.ajax__combobox_itemlist li').bind('mousedown', onDirectorPosSelected);
            }

            // Если ввожу что-то руками, то текстбокс с дат. падежом очищается
            $('#<%=this.ComboBoxDirectorPosition.ClientID%>').find('input').bind('keyup', function() {
                $('#<%=this.TBDirectorPosInGenetive.ClientID%>').val('');
                $('#<%=this.HDNComboBoxDirectorPosition.ClientID%>').val($(this).val());
            });

            function generateDirectorName() {
                var firstName = $.trim($('#<%=this.TBFirstName.ClientID%>').val());
                var lastName = $.trim($('#<%=this.TBLastName.ClientID%>').val());
                var patronymicName = $.trim($('#<%=this.TBPatronymicName.ClientID%>').val());
                var name = '';

                // До лучших времен! когда придется генерить ФИО сокращенное
                //if (firstName.length > 0) {
                //    firstName = firstName[0] + '.';
                //}

                //if (patronymicName.length > 0) {
                //    patronymicName = patronymicName[0] + '.';
                //}

                name = $.trim(lastName + ' ' + firstName + ' ' + patronymicName);

                $('#<%=this.TBNameDirector.ClientID%>').attr('value', name);
                $('#<%=this.HDNNameDirector.ClientID%>').attr('value', name);
            }

            $('#<%=this.TBLastName.ClientID%>').change(generateDirectorName);
            $('#<%=this.TBFirstName.ClientID%>').change(generateDirectorName);
            $('#<%=this.TBPatronymicName.ClientID%>').change(generateDirectorName);

            setTimeout(function() {
                if ($('#<%=this.HDNComboBoxDirectorPosition.ClientID%>').val()) {
                    $('#<%=this.ComboBoxDirectorPosition.ClientID%>').find('input:first').val($('#<%=this.HDNComboBoxDirectorPosition.ClientID%>').val());
                    $('#<%=this.ComboBoxDirectorPosition.ClientID%>').find('input:last').val(-2);
                    
                    $.each(positions, function () {
                        if (this.name == $('#<%=this.HDNComboBoxDirectorPosition.ClientID%>').val()) {
                            $('#<%=this.ComboBoxDirectorPosition.ClientID%>').children('input').val(this.id);
                        }
                    });
                }
            }, 200);
        });
</script>
<asp:HiddenField ID="hiddenKindId" runat="server" />
<asp:HiddenField ID="hfIsChangeStatus" runat="server" />
<table width="100%">
    <tr>
        <th>
            Регион
        </th>
        <td colspan="2" style="vertical-align: middle;">
            <asp:Literal runat="server" ID="LRegion"></asp:Literal>
        </td>
    </tr>
    <tr>
        <th>
            Город
        </th>
        <td colspan="2" style="vertical-align: middle;">
            <asp:Literal runat="server" ID="lTownName"></asp:Literal>
        </td>
    </tr>
    <tr>
        <th>
            Тип
        </th>
        <td colspan="2" style="vertical-align: middle;">
            <asp:Literal runat="server" ID="lblOrgTypes"></asp:Literal>
        </td>
    </tr>
    <tr>
        <th>
            Вид
        </th>
        <td colspan="2" style="vertical-align: middle;">
            <asp:DropDownList runat="server" ID="ddlOrgKind" CssClass="sel" DataValueField="Id"
                DataTextField="Name" AppendDataBoundItems="true">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <th>
            Организационно-правовая форма
        </th>
        <td colspan="2" style="vertical-align: middle;">
            <asp:Literal runat="server" ID="LOPF"></asp:Literal>
        </td>
    </tr>
    <tr class="displayNone">
        <th>
            Является филиалом
        </th>
        <td colspan="2" style="vertical-align: middle;">
            <asp:Literal runat="server" ID="LIsFilial"></asp:Literal>
        </td>
    </tr>
    <tr>
        <th>
            Статус
        </th>
        <td colspan="2" style="vertical-align: middle;">
            <asp:Literal runat="server" ID="OrgStatusLabel"></asp:Literal>
            <br />
            <asp:Label ID="NewOrgNameLabel" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr id="dateChangeStatus">
        <th>
            Фактическая дата изменения статуса
        </th>
        <td colspan="2" class="text" style="vertical-align: middle;">
            <asp:Literal runat="server" ID="lblDateChangeStatus"></asp:Literal>
        </td>
    </tr>
    <tr id="reasonChangeStatus">
        <th>
            Обоснование
        </th>
        <td colspan="2" class="text" style="vertical-align: middle;">
            <asp:Literal runat="server" ID="lblReasonChangeStatus"></asp:Literal>
        </td>
    </tr>
    <asp:PlaceHolder ID="phMainOrgName" runat="server">
        <tr>
            <th>
                Головная организация
            </th>
            <td colspan="2" style="vertical-align: middle;">
                <asp:Literal runat="server" ID="lblMainOrgName"></asp:Literal>
            </td>
        </tr>
    </asp:PlaceHolder>
    <tr>
        <th>
            Какая информационная система (ИС) используется в Вашей ОО для ведения приемной кампании помимо ФИС ГИА и Приема
        </th>
        <td colspan="2" style="vertical-align: middle;">
            <asp:DropDownList runat="server" ID="ddlIS" CssClass="sel" DataValueField="Id"
                DataTextField="Name" AppendDataBoundItems="true">
            </asp:DropDownList>
        </td>
    </tr>
    <tr class="displayNone">
        <%if (GeneralSystemManager.CanChangeRCModel(this.User.Identity.Name) && this.CanChangeRCModel)
          { %>
        <td colspan="3" style="border: none;">
            <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                <tr>
                    <th colspan="2" style="vertical-align: top; width: 70%; padding-left: 0; border-bottom-color: #fff;
                        border-top-color: #fff;">
                        <strong>Модель приемной кампании:</strong>
                    </th>
                </tr>
                <tr>
                    <td style="border-bottom-color: #fff;">
                        <div class="statement_table" style="padding-bottom: 0px;">
                            <table id="tblRecruitmentCampaigns" cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                                <tr>
                                    <td style="padding-left: 0; padding-bottom: 0; padding-top: 0; border-top-color: #fff;
                                        border-bottom-color: #fff;">
                                        <div class="tablenoborder">
                                            <asp:RadioButtonList ID="rblRecruitmentCampaigns" DataSourceID="dsRecruitmentCampaigns"
                                                runat="server" DataValueField="Id" DataTextField="ModelName" CssClass="radio-button-list">
                                            </asp:RadioButtonList>
                                        </div>
                                        <asp:SqlDataSource runat="server" ID="dsRecruitmentCampaigns" SelectCommand="SELECT Id, ' ' + ModelName + (case Id when 999 then ':' else '' end)  as ModelName FROM [dbo].[RecruitmentCampaigns] ORDER BY [Id]"
                                            ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>">
                                        </asp:SqlDataSource>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 20px; padding-top: 0; border-top-color: #fff!important;
                                        border-bottom-color: #fff!important;">
                                        <asp:TextBox runat="server" ID="txtBxDescription" CssClass="textareaoverflowauto"
                                            Rows="3" Height="50px" Width="99%" TextMode="MultiLine" MaxLength="400" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </td>
        <%}
          else
          { %>
        <th>
            Модель приемной кампании
        </th>
        <td colspan="2" style="vertical-align: top;">
            <asp:Literal runat="server" ID="lblModelName"></asp:Literal>
        </td>
        <%}%>
    </tr>            
                
     <tr>
        <th colspan="2" style="vertical-align: top; width: 70%; padding-left: 0; border-bottom-color: #fff;
            border-top-color: #fff;">
            <strong>Сведения о защищенном подключении</strong>
        </th>
    </tr>
    <asp:PlaceHolder runat="server" Visible="false">
        <tr>
            <th style="font-weight: normal; padding-left: 6px;">
                Статус подключения
            </th>
            <td>
                <asp:Literal runat="server" ID="lConnectionStatus" />
            </td>
        </tr>
        <tr>
            <th style="font-weight: normal; padding-left: 6px;">
                Схема подключения
            </th>
            <td>
                <asp:Literal runat="server" ID="lConnectionScheme" />
            </td>
        </tr>
    </asp:PlaceHolder>
    <tr>
        <th style="font-weight: normal; padding-left: 6px;">
            Письмо о переносе сроков
        </th>
        <td>
            <asp:Literal runat="server" ID="lLetterToReschedule" />
        </td>
    </tr>
    <tr>
        <th style="font-weight: normal; padding-left: 6px;">
            Срок подключения к защищенной сети
        </th>
        <td>
            <asp:Literal runat="server" ID="lTimeConnectionToSecureNetwork" />
        </td>
    </tr>
    <tr>
        <th style="font-weight: normal; padding-left: 6px; border-bottom-color: #fff;">
            Срок вноса сведений в ФИС ЕГЭ и приема
        </th>
        <td style="border-bottom-color: #fff;">
            <asp:Literal runat="server" ID="lTimeEnterInformationInFIS" />
        </td>
    </tr>
   

    <tr>
        <th style="border-bottom-color: #fff; padding-left: 0; text-align: center;
            padding-top: 30px;" colspan="3">
            Детальная информация об ОУ
        </th>
    </tr>
    <%if (GeneralSystemManager.CanChangeRCModel(this.User.Identity.Name) && this.CanChangeRCModel)
      { %>
    <tr>
        <td colspan="3">
            <asp:Label ID="Instruction" Style="color: rgb(215, 5, 5) !important;" runat="server"></asp:Label>
        </td>
    </tr>
    <%}%>
    <tr>
        <th style="font-weight: normal;">
            Полное наименование
        </th>
        <td colspan="2" style="vertical-align: middle;">
            <asp:Literal runat="server" ID="LFullName"></asp:Literal>
        </td>
    </tr>
    <tr>
        <th style="font-weight: normal;">
            Краткое наименование
        </th>
        <td colspan="2" style="vertical-align: middle;">
            <asp:TextBox ID="tbShortName" runat="server"></asp:TextBox> 
        </td>
    </tr>
    <tr class="displayNone">
        <th style="font-weight: normal;">
            ИНН
        </th>
        <td colspan="2" style="vertical-align: middle;">
            <asp:Literal runat="server" ID="LINN"></asp:Literal>
        </td>
    </tr>
    <tr class="displayNone">
        <th style="font-weight: normal;">
            ОГРН
        </th>
        <td colspan="2" style="vertical-align: middle;">
            <asp:Literal runat="server" ID="LOGRN"></asp:Literal>
        </td>
    </tr>
    <tr>
        <th style="font-weight: normal;">
            КПП
        </th>
        <td colspan="2" style="vertical-align: middle">
            <asp:TextBox ID="tbKPP" runat="server" MaxLength="9"></asp:TextBox> 
        </td>
    </tr>
    <tr class="displayNone">
        <td>
            Учредитель (для&nbsp;ССУЗов&nbsp;и&nbsp;ВУЗов)
        </td>
        <td runat="server" id="tdFounders" colspan="2" class='founders-cell'>
                 
        </td>
    </tr>
    <%if (GeneralSystemManager.CanChangeRCModel(this.User.Identity.Name) && this.CanChangeRCModel)
      { %>
    <tr>
        <td colspan="3">
            <p>
                <strong>Руководитель организации</strong>
            </p>
            <p>
                <table class="director-position" style="width: 100%">
                    <tr>
                        <td class="labels">
                            Должность руководителя
                        </td>
                        <td class="inputs">
                            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="Server" />
                            <ajaxToolkit:ComboBox ID="ComboBoxDirectorPosition" runat="server" DataSourceID="dsComboBoxDirectorPosition"
                                DataValueField="Id" DataTextField="Name" AutoPostBack="False" DropDownStyle="DropDown"
                                AutoCompleteMode="None" CaseSensitive="False" CssClass="sel" Width="171" MaxLength="100"
                                ItemInsertLocation="OrdinalValue" />
                            <asp:HiddenField ID="HDNComboBoxDirectorPosition" runat="server" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ComboBoxDirectorPosition"
                                EnableClientScript="false" Display="None" ErrorMessage='Поле "Должность руководителя" обязательно для заполнения' />
                            <asp:SqlDataSource ID="dsComboBoxDirectorPosition" ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>"
                                SelectCommand="SELECT Id, PositionName as Name, PositionNameInGenetive as NameInGenetive  FROM [dbo].[DirectorPosition]"
                                runat="server"></asp:SqlDataSource>
                        </td>
                        <td class="labels">
                            Должность руководителя (дат. падеж)
                        </td>
                        <td class="inputs">
                            <asp:TextBox runat="server" class="email" ID="TBDirectorPosInGenetive" CssClass="txt"
                                MaxLength="100" Width="200" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="TBDirectorPosInGenetive"
                                EnableClientScript="false" Display="None" ErrorMessage='Поле "Должность руководителя (дат. падеж)" обязательно для заполнения' />
                        </td>
                    </tr>
                    <tr>
                        <td class="labels">
                            Фамилия руководителя
                        </td>
                        <td class="inputs">
                            <asp:TextBox runat="server" class="email" ID="TBLastName" CssClass="txt" MaxLength="100"
                                Width="200" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="TBLastName"
                                EnableClientScript="false" Display="None" ErrorMessage='Поле "Фамилия руководителя" обязательно для заполнения' />
                        </td>
                        <td class="labels">
                            ФИО руководителя
                        </td>
                        <td class="inputs">
                            <asp:TextBox runat="server" class="email" ID="TBNameDirector" CssClass="txt" MaxLength="100"
                                Width="200" ReadOnly="True" />
                            <asp:HiddenField ID="HDNNameDirector" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labels">
                            Имя руководителя
                        </td>
                        <td class="inputs">
                            <asp:TextBox runat="server" class="email" ID="TBFirstName" CssClass="txt" MaxLength="100"
                                Width="200" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="TBFirstName"
                                EnableClientScript="false" Display="None" ErrorMessage='Поле "Имя руководителя" обязательно для заполнения' />
                        </td>
                        <td class="labels">
                            ФИО руководителя (дат. падеж)
                        </td>
                        <td class="inputs">
                            <asp:TextBox runat="server" class="email" ID="TBNameInGenetive" CssClass="txt" MaxLength="100"
                                Width="200" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="TBNameInGenetive"
                                EnableClientScript="false" Display="None" ErrorMessage='Поле "ФИО руководителя (дат. падеж)" обязательно для заполнения' />
                        </td>
                    </tr>
                    <tr>
                        <td class="labels">
                            Отчество руководителя
                        </td>
                        <td class="inputs">
                            <asp:TextBox runat="server" class="email" ID="TBPatronymicName" CssClass="txt" MaxLength="100"
                                Width="200" />
                        </td>
                        <td class="labels">
                        </td>
                        <td class="inputs">
                        </td>
                    </tr>
                </table>
            </p>
        </td>
    </tr>
    <%}
      else
      { %>
    <tr>
        <th style="font-weight: normal;">
            Должность&nbsp;руководителя
        </th>
        <td colspan="2" style="vertical-align: middle;">
            <asp:Literal runat="server" ID="LDirectorPosition"></asp:Literal>
        </td>
    </tr>
    <tr>
        <th style="font-weight: normal;">
            Должность&nbsp;руководителя&nbsp;(дат.&nbsp;падеж)
        </th>
        <td colspan="2" style="vertical-align: middle;">
            <asp:Literal runat="server" ID="LDirectorPositionInGenetive"></asp:Literal>
        </td>
    </tr>
    <tr>
        <th style="font-weight: normal;">
            ФИО&nbsp;руководителя
        </th>
        <td colspan="2" style="vertical-align: middle;">
            <asp:Literal runat="server" ID="LDirectorName"></asp:Literal>
        </td>
    </tr>
    <tr>
        <th style="font-weight: normal;">
            ФИО&nbsp;руководителя&nbsp;(дат.&nbsp;падеж)
        </th>
        <td colspan="2" style="vertical-align: middle;">
            <asp:Literal runat="server" ID="LDirectorNameInGenetive"></asp:Literal>
        </td>
    </tr>
    <%}%>
    <tr>
        <%if (GeneralSystemManager.CanChangeRCModel(this.User.Identity.Name) && this.CanChangeRCModel)
          { %>
        <td colspan="3">
            <div class="statement_in" style="border-bottom-color: #fff;">
                <p>
                    <strong>Фактический адрес</strong></p>
                <p>
                    <asp:TextBox runat="server" ID="TBFactAddress" Rows="3" Height="50px" TextMode="MultiLine"
                        MaxLength="500" CssClass="textareaoverflowauto" Width="1050px" />
                </p>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TBFactAddress"
                    EnableClientScript="false" Display="None" ErrorMessage='Поле "Фактический адрес" обязательно для заполнения' />
            </div>
        </td>
        <%}
          else
          { %>
        <th style="font-weight: normal;">
            Фактический адрес
        </th>
        <td colspan="2" style="vertical-align: middle;">
            <asp:Literal runat="server" ID="LFactAddress"></asp:Literal>
        </td>
        <%}%>
    </tr>
    <tr>
        <%if (GeneralSystemManager.CanChangeRCModel(this.User.Identity.Name) && this.CanChangeRCModel)
          { %>
        <td colspan="3">
            <div class="statement_in" style="border-bottom-color: #fff;">
                <p>
                    <strong>Юридический адрес</strong></p>
                <p>
                    <asp:TextBox runat="server" ID="TBJurAddress" CssClass="textareaoverflowauto" Rows="3"
                        Height="50px" TextMode="MultiLine" MaxLength="500" Width="1050px" />
                </p>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TBJurAddress"
                    EnableClientScript="false" Display="None" ErrorMessage='Поле "Юридический адрес" обязательно для заполнения' />
            </div>
        </td>
        <%}
          else
          { %>
        <th style="font-weight: normal;">
            Юридический адрес
        </th>
        <td colspan="2" style="vertical-align: middle;">
            <asp:Literal runat="server" ID="LJurAddress"></asp:Literal>
        </td>
        <%}%>
    </tr>

    <tr>
        <td colspan="2">
            <table class="license-supplement" style="width: 100%">
                <thead>
                    <tr>
                        <td colspan="3">
                            <strong>Лицензия</strong>
                        </td>
                        <td colspan="3">
                            <strong>Приложение к лицензии</strong>
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td style="font-weight: normal;">
                            Номер лицензии
                        </td>
                        <td colspan="2" style="vertical-align: middle;">
                            <asp:Literal runat="server" ID="lLicenseNumber"></asp:Literal>
                        </td>

                        <td style="font-weight: normal;">
                            Номер приложения к лицензии
                        </td>
                        <td colspan="2" style="vertical-align: middle;">
                            <asp:Literal runat="server" ID="lSupplementNumber"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td style="font-weight: normal;">
                            Дата выдачи лицензии
                        </td>
                        <td colspan="2" style="vertical-align: middle;">
                            <asp:Literal runat="server" ID="lLicenseIssueDate"></asp:Literal>
                        </td>

                        <td style="font-weight: normal;">
                            Дата выдачи приложения к лицензии
                        </td>
                        <td colspan="2" style="vertical-align: middle;">
                            <asp:Literal runat="server" ID="lSupplementOrderDocumentDate"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td style="font-weight: normal;">
                            Статус лицензии
                        </td>
                        <td colspan="2" style="vertical-align: middle;">
                            <asp:Literal runat="server" ID="lLicenseStatus"></asp:Literal>
                            <br />
                            <asp:Label runat="server" ID="lLicenseStatusWarning" ForeColor="Red"></asp:Label>
                        </td>

                        <td style="font-weight: normal;">
                            Статус приложения к лицензии
                        </td>
                        <td colspan="2" style="vertical-align: middle;">
                            <asp:Literal runat="server" ID="lSupplementStatusName"></asp:Literal>
                        </td>
                    </tr>
                </tbody>
            </table>
        </td>
    </tr>

    <%--<tr>
        <th style="font-weight: normal;">
            Номер лицензии
        </th>
        <td colspan="2" style="vertical-align: middle;">
            <asp:Literal runat="server" ID="lLicenseNumber"></asp:Literal>
        </td>
    </tr>
    <tr>
        <th style="font-weight: normal;">
            Дата выдачи лицензии
        </th>
        <td colspan="2" style="vertical-align: middle;">
            <asp:Literal runat="server" ID="lLicenseIssueDate"></asp:Literal>
        </td>
    </tr>
    <tr>
        <th style="font-weight: normal;">
            Статус лицензии
        </th>
        <td colspan="2" style="vertical-align: middle;">
            <asp:Literal runat="server" ID="lLicenseStatus"></asp:Literal>
            <br />
            <asp:Label runat="server" ID="lLicenseStatusWarning" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <th style="font-weight: normal;">
            Номер приложения к лицензии
        </th>
        <td colspan="2" style="vertical-align: middle;">
            <asp:Literal runat="server" ID="lSupplementNumber"></asp:Literal>
        </td>
    </tr>
    <tr>
        <th style="font-weight: normal;">
            Дата выдачи приложения к лицензии
        </th>
        <td colspan="2" style="vertical-align: middle;">
            <asp:Literal runat="server" ID="lSupplementOrderDocumentDate"></asp:Literal>
        </td>
    </tr>
    <tr>
        <th style="font-weight: normal;">
            Статус приложения к лицензии
        </th>
        <td colspan="2" style="vertical-align: middle;">
            <asp:Literal runat="server" ID="lSupplementStatusName"></asp:Literal>
            <br />
            <asp:Label runat="server" ID="Label1" ForeColor="Red"></asp:Label>
        </td>
    </tr>--%>

    <%if (GeneralSystemManager.CanChangeRCModel(this.User.Identity.Name) && this.CanChangeRCModel)
      { %>
    <tr>
        <th colspan="3">
            Код города
            <asp:TextBox runat="server" ID="TBCityCode" CssClass="txt" MaxLength="10" Width="40px"
                Style="margin-left: 35px; margin-right: 80px;" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="TBCityCode"
                EnableClientScript="false" Display="None" ErrorMessage='Поле "Код города" обязательно для заполнения' />
            Телефон&nbsp;
            <asp:TextBox runat="server" ID="TBPhone" CssClass="txt" MaxLength="255" Width="675"
                Style="float: right; margin-right: 2px; margin-left: 35px; padding-right: 0;" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="TBPhone"
                EnableClientScript="false" Display="None" ErrorMessage='Поле "Телефон" обязательно для заполнения' />
        </th>
    </tr>
    <tr>
        <th colspan="3">
            Факс
            <asp:TextBox runat="server" ID="TBFax" CssClass="txt" MaxLength="100" Width="930"
                Style="margin-left: 77px; padding-right: 0;" />
        </th>
    </tr>
    <tr>
        <th colspan="3">
            E-mail
            <asp:TextBox runat="server" class="email" ID="TBEMail" CssClass="txt" MaxLength="100"
                Width="922" Style="margin-left: 69px;" />
        </th>
        <asp:RegularExpressionValidator runat="server" ID="vldEmailFormat" ControlToValidate="TBEMail"
            EnableClientScript="false" Display="None" ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*\.{0,1}@([0-9a-zA-Z]*[-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$"
            ErrorMessage='Поле "E-mail" заполнено неверно' />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="TBEMail"
            EnableClientScript="false" Display="None" ErrorMessage='Поле "E-mail" обязательно для заполнения' />
    </tr>
    <tr>
        <th colspan="3">
            Сайт
            <asp:TextBox runat="server" ID="TBWebPage" CssClass="txt long" MaxLength="100" Width="922"
                Style="margin-left: 79px;" />
        </th>
    </tr>
    <%}
      else
      { %>
    <tr>
        <th style="font-weight: normal;">
            Код города
        </th>
        <td style="vertical-align: middle;" colspan="2">
            <asp:Literal runat="server" ID="LCityCode"></asp:Literal>
        </td>
    </tr>
    <tr>
        <th style="font-weight: normal;">
            Телефон
        </th>
        <td colspan="2" style="vertical-align: middle;">
            <asp:Literal runat="server" ID="LPhone"></asp:Literal>
        </td>
    </tr>
    <tr>
        <th style="font-weight: normal;">
            Факс
        </th>
        <td colspan="2" style="vertical-align: middle;">
            <asp:Literal runat="server" ID="LFax"></asp:Literal>
        </td>
    </tr>
    <tr>
        <th style="font-weight: normal;">
            E-mail
        </th>
        <td colspan="2" style="vertical-align: middle;">
            <asp:Literal runat="server" ID="LEMail"></asp:Literal>
        </td>
    </tr>
    <tr>
        <th style="font-weight: normal;">
            Сайт
        </th>
        <td colspan="2" style="vertical-align: middle;">
            <asp:Literal runat="server" ID="LSite"></asp:Literal>
        </td>
    </tr>
    <%}%>
    <tr>
        <td colspan="3" class="box-submit" style="border-bottom-color: #fff;">
        </td>
    </tr>
</table>
<%--</form>--%>