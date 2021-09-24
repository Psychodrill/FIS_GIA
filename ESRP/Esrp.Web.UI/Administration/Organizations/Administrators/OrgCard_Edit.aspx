<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Templates/Administration.Master"
    AutoEventWireup="true" CodeBehind="OrgCard_Edit.aspx.cs" Inherits="Esrp.Web.Administration.Organizations.OrgCard" %>

<%@ Import Namespace="Esrp.Core.Organizations" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register Src="~/Controls/DatePickerControl.ascx" TagPrefix="uc" TagName="DatePicker" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <script type="text/javascript" src="/Common/Scripts/tiny_mce/tiny_mce.js"></script>
    <script type="text/javascript" src="/Common/Scripts/Utils.js"></script>
    <script type="text/javascript" src="/Common/Scripts/CalendarPopup.js"></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            var params = {
                changedEl: 'select',
                visRows: 7,
                scrollArrows: true
            };
            cuSel(params);
        });
        
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

        function removeFirstLastLineFromTable(idtable) {

            $(idtable + ' tr:first td:first').css('border-top-color', '#fff');
            $(idtable + ' tr:last td:first').css('border-bottom-color', '#fff');
            //alert($(idtable + ' tr:first td:first').attr('border-top-color'));

        }
        $(document).ready(function () {
            removeFirstLastLineFromTable("#<% =this.rblRecruitmentCampaigns.ClientID %>");
            removeFirstLastLineFromTable("#<% =this.rblReceptionOnResultsCNE.ClientID %>");
            //$("div.cusel sel ").css("width","100%");
            $('div[id ^= "cuselFrame"]').css("cssText", "width: 97% !important;");

            // Если значение пусто, то скрываем контрол загрузки файла, иначе скрываем контрол с ссылками на файл
            if ($('#<%=hfLetterToReschedule.ClientID %>').val() != "") {
                $("#<%=fuLetterToReschedule.ClientID %>").hide();
            }
            else {
                $("#<%=lLetterToReschedule.ClientID %>").hide();
            }   
            // Если Тип ВУЗ или ССУЗ, то появляются радиобаттоны "Прием по результатам ЕГЭ"
            if ($("#<%=DDLOrgTypes.ClientID %>").val() == 1 || $("#<%=DDLOrgTypes.ClientID %>").val() == 2) {
                $('#trReceptionOnResultsCNE').show();
            }
            else {
                $('#trReceptionOnResultsCNE').hide();
            }

            $("#<%=DDLOrgTypes.ClientID %>").change(function () {
                if ($("#<%=DDLOrgTypes.ClientID %>").val() == 1 || $("#<%=DDLOrgTypes.ClientID %>").val() == 2) {
                    $('#trReceptionOnResultsCNE').show();
                }
                else {
                    $('#trReceptionOnResultsCNE').hide();
                }
            });

            $("#<%= cbFil.ClientID %>").click(function () {
                if ($(this).attr('checked')) {
                    EnableFilial();
                }
                else {
                    DisableFilial();
                }
            });

            // Если нажата ссылка удалить файл,то скрываем контрол с ссылками на файл и показываем контрол загрузки файла, в скрытое поле заносим значение пусто
            $("#deleteLetter").click(function () {
                $("#<%=lLetterToReschedule.ClientID %>").hide();
                $("#<%=fuLetterToReschedule.ClientID %>").show();
                $('#<%=hfLetterToReschedule.ClientID %>').val("");
            });

            // изменение статуса организации
            $("#<%=DDLOrgStatus.ClientID %>").change(function () {

                var status = $('#<%= HdnOrgStatus.ClientID %>').val();
                if ($(this).val() != status && status != 0) {
                    $('#dateChangeStatus').show();
                    $('#reasonChangeStatus').show();
                    
                }
                else {
                    $('#dateChangeStatus').hide();
                    $('#reasonChangeStatus').hide();
                    
                }

                if ($(this).val() == 2) {
                    $('#reorganizedTo').show();
                }
                else {
                    $('#reorganizedTo').hide();
                }
            });
            //  Выбор "Другой" модели приемной кампании 
            $("#tblRecruitmentCampaigns input").live("click", function () {
                console.log($(this));
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
            
            //дизейбл поля ИС, если не выбрана "ИС другого производителя
            $("#AnotherNameDisable").live("click", function () {
                var SelectedValue = document.getElementById('ctl00_ctl00_cphContent_cphContent_ddlIS');
                console.log(SelectedValue.value);
                if (SelectedValue.value != "5") {
                    DisableAnotherName();
                }
                else {
                    EnableAnotherName();
                }
            });

            if ($("#<%= AnotherNameIs.ClientID %>").attr("disabled")) {
                DisableAnotherName();
            }

            //Скрытие полей, если выбран Учредитель(Id = 6)
            if ($("#<%=DDLOrgTypes.ClientID %>").val() == 6) {
                VisibleFieldsFounder();
            }
            else {
                VisibleAllFields();
            }

            //Скрытие полей, если выбран Учредитель(Id = 6)
            $("#<%=DDLOrgTypes.ClientID %>").change(function () {
                if ($(this).val() == 6) {
                    VisibleFieldsFounder();
                }
                else {
                    VisibleAllFields();
                }
            });

            // статус орг-ии "реорганизованная" => показать выбор новой организации
            if ($("#<%=DDLOrgStatus.ClientID %>").val() != 2) {
                $('#reorganizedTo').hide();
            }

            var status = $('#<%= HdnOrgStatus.ClientID %>').val();
            if (status == $('#<%= DDLOrgStatus.ClientID %>').val() || status == 0) {
                $('#dateChangeStatus').hide();
                $('#reasonChangeStatus').hide();
                
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

        function VisibleFieldsFounder() {
            $(".displayNone").css("display", "none");
            $("#noteTd").css("display", "none");

            $("#<%= DDLOPF.ClientID %>").val("0").attr("disabled", "disabled");
            var DDLOPFcusel = '#cuselFrame-' + "<%=this.DDLOPF.ClientID %>";
            $(DDLOPFcusel).addClass("classDisCusel");
            var firstText = $(DDLOPFcusel).find("span[val = '0']").text();
            DDLOPFcusel = DDLOPFcusel + ' .cuselText';
            $(DDLOPFcusel).html(firstText);

            var DDLOrgKindsFounder = '#cuselFrame-' + "<%=this.DDLOrgKindsFounder.ClientID %>";
            $(DDLOrgKindsFounder).css("display", "block");
            $("#<%= DDLOrgKindsFounder.ClientID %>").css("display", "block");

            var DDLOrgKinds = '#cuselFrame-' + "<%=this.DDLOrgKinds.ClientID %>";
            $(DDLOrgKinds).css("display", "none");
            $("#<%= DDLOrgKinds.ClientID %>").css("display", "none");
            $("#<%= hiddenKindsFounder.ClientID %>").val("0");


        }
        /*
        var params = {
        changedEl: "#<%= DDLOPF.ClientID %>",
        visRows: 5,
        scrollArrows: true                
        };

        cuSel(params);
        */

        function VisibleAllFields() {
            $(".displayNone").css("display", "table-row");
            $("#noteTd").css("display", "table-cell");
            var DDLOPF = $("#<%= DDLOPF.ClientID %>");

            var DDLOPFcusel = '#cuselFrame-' + "<%=this.DDLOPF.ClientID %>";
            if (DDLOPF.attr("disabled")) {
                DDLOPF.attr("disabled", false);
                $(DDLOPFcusel).removeClass("classDisCusel");
            }
            else {
                DDLOPF.attr("enabled", "enabled");
                $(DDLOPFcusel).removeClass("classDisCusel");
            }

            var DDLOrgKindsFounder = '#cuselFrame-' + "<%=this.DDLOrgKindsFounder.ClientID %>";
            $(DDLOrgKindsFounder).css("display", "none");
            $("#<%= DDLOrgKindsFounder.ClientID %>").css("display", "none");

            var DDLOrgKinds = '#cuselFrame-' + "<%=this.DDLOrgKinds.ClientID %>";
            $(DDLOrgKinds).css("display", "block");
            $("#<%= DDLOrgKinds.ClientID %>").css("display", "block");
            $("#<%= hiddenKindsFounder.ClientID %>").val("1");
        }

        function btnUpdateClick() {
            if ($("#<%=DDLOrgTypes.ClientID %>").val() == 6) {
                //$("#<%= TBINN.ClientID %>").val("0000000000");
                //$("#<%= TBOGRN.ClientID %>").val("0000000000000");
                $("#<%= txtBxFederalBudget.ClientID %>").val("1");
            }
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

        function DisableAnotherName() {
            $("#<%= AnotherNameIs.ClientID %>")
                  .attr("disabled", "disabled")
                  .css("background", "#ddd")
                  .css("color", "#999")
                  .val("Заполняется в случае выбора ИС \"ИС другого производителя\"");
                }
        function EnableAnotherName() {
            $("#<%= AnotherNameIs.ClientID %>")
                  .removeAttr("disabled")
                  .css("background", "rgb(235,243,246)")
                  .css("color", "#333")
                  .val("");
        }

        function EnableFilial() {
            $("#<%= btnFilial.ClientID %>").removeAttr("disabled");

            $("#<%= lblMainOrgName.ClientID %>").css("color", "#333");
        }

        function DisableFilial() {
            $("#<%= btnFilial.ClientID %>").attr("disabled", "disabled");

            $("#<%= lblMainOrgName.ClientID %>")
              .css("color", "#999")
              .text("Нет головной организации");

            $("#<%= hfMainOrgId.ClientID %>").val("");
        }
    </script>
    <style type="text/css">
        #filname span
        {
            white-space: normal;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphLeftMenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphContent" runat="server">
    <div class="left_col">
        <form id="Form2" runat="server">
        <asp:ValidationSummary CssClass="error_block" ID="ValidationSummary2" runat="server"
            DisplayMode="BulletList" EnableClientScript="false" HeaderText="<p>Произошли следующие ошибки:</p>" />
        <asp:Label ID="lblError" Visible="False" runat="server" ForeColor="Red" />
        <div class="col_in">
            <div class="statement edit">
                <p class="title">
                    <%= this.CustomTitle%></p>
                <p class="back">
                    <a id="BackLink" runat="server" href="#"><span class="un">Вернуться</span></a></p>
                <p class="statement_menu">
                    <a href="#" class="active" onclick="$('#<%= btnUpdate.ClientID %>').click();"><span>
                        Сохранить</span></a>
                    <% if (Request.QueryString["OrgID"] != "0")
                       { %>
                    <a href="/Administration/Organizations/OrganizationHistory.aspx?OrgId=<%= GetParamInt("OrgID") %>"
                        title="История изменений" class="gray"><span>История изменений</span></a>
                    <% } %>
                </p>
                <div class="clear">
                </div>
                <div class="statement_table">
                    <table width="840">
                        <tr>
                            <th style="width: 300px;">
                                Регион
                            </th>
                            <td>
                                <div>
                                    <asp:DropDownList runat="server" ID="DDLRegions" DataSourceID="DSRegions" DataValueField="Id"
                                        DataTextField="Name" Width="370px">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource runat="server" ID="DSRegions" SelectCommand="SELECT * FROM dbo.Region ORDER BY [Name]"
                                        ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>">
                                    </asp:SqlDataSource>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <th style="width: 300px;">
                                Город
                            </th>
                            <td>
                                <div>
                                    <asp:TextBox runat="server" ID="tbTownName" Width="370px"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Тип
                            </th>
                            <td>
                                <asp:DropDownList runat="server" ID="DDLOrgTypes" DataSourceID="DSOrgTypes" CssClass="sel"
                                    DataValueField="Id" DataTextField="Name">
                                </asp:DropDownList>
                                <asp:SqlDataSource runat="server" ID="DSOrgTypes" SelectCommand="SELECT * FROM dbo.OrganizationType2010 order by SortOrder"
                                    ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>">
                                </asp:SqlDataSource>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Вид
                            </th>
                            <td>
                                <asp:HiddenField ID="hiddenKindsFounder" runat="server" />
                                <asp:DropDownList runat="server" ID="DDLOrgKinds" DataSourceID="DSKinds" CssClass="sel"
                                    DataValueField="Id" DataTextField="Name">
                                </asp:DropDownList>
                                <asp:SqlDataSource runat="server" ID="DSKinds" SelectCommand="SELECT * FROM dbo.OrganizationKind  WHERE Id <> 9 order by SortOrder"
                                    ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>">
                                </asp:SqlDataSource>
                                <asp:DropDownList runat="server" ID="DDLOrgKindsFounder" DataSourceID="DSKindsFounder"
                                    CssClass="sel" DataValueField="Id" DataTextField="Name">
                                </asp:DropDownList>
                                <asp:SqlDataSource runat="server" ID="DSKindsFounder" SelectCommand="SELECT * FROM dbo.OrganizationKind WHERE Id in (8,9) order by SortOrder"
                                    ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>">
                                </asp:SqlDataSource>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Организационно-правовая форма
                            </th>
                            <td style="vertical-align: middle;">
                                <asp:DropDownList runat="server" ID="DDLOPF" CssClass="sel">
                                    <asp:ListItem Text="Государственный" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Негосударственный" Value="1"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr class="displayNone">
                            <th>
                                <asp:CheckBox runat="server" ID="cbFil" Text="&nbsp;является филиалом" />
                            </th>
                            <td>
                                <asp:Button ID="btnFilial" runat="server" Width="250px" Text="Выбрать головную организацию"
                                    CausesValidation="False" OnClick="btnFilial_Click" />
                                <p>
                                    <strong>Головная организация</strong></p>
                                <p>
                                    <asp:Label ID="lblMainOrgName" runat="server" Text=""></asp:Label>
                                    <asp:HiddenField ID="hfMainOrgId" runat="server" />
                                </p>
                            </td>
                        </tr>
                        <tr class="displayNone">
                            <th>
                                Статус
                            </th>
                            <td>
                                <asp:DropDownList runat="server" ID="DDLOrgStatus" CssClass="sel">
                                    <asp:ListItem Text="Действующая" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Реорганизованная" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Ликвидированная" Value="3"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:CustomValidator runat="server" ID="CustomValidator1" ErrorMessage="Не указана новая организация" />
                                <p>
                                    <div id="reorganizedTo">
                                        <asp:Button ID="btnReorganizedTo" runat="server" Width="250px" Text="Выбрать новую организацию"
                                            CausesValidation="False" OnClick="btnReorganizedTo_Click" />
                                        <div class="smallspacer">
                                        </div>
                                        <asp:Label ID="lblNewOrgName" runat="server" Text=""></asp:Label>
                                        <asp:HiddenField ID="hfNewOrgId" runat="server" />
                                    </div>
                                </p>
                            </td>
                        </tr>
                        <tr id="dateChangeStatus">
                            <th>
                                Фактическая дата изменения статуса
                            </th>
                            <td>
                                <div style="white-space: nowrap;">
                                    <uc:DatePicker runat="server" ID="TbDateChangeStatus" MinDate="01.01.2000" />
                                </div>
                            </td>
                        </tr>
                        <tr id="reasonChangeStatus">
                            <th>
                                Обоснование
                            </th>
                            <td colspan="2">
                                <asp:TextBox runat="server" ID="TBReason" CssClass="txt" MaxLength="100" />
                            </td>
                            <asp:RequiredFieldValidator ID="RequiredReasonValidator" runat="server" ControlToValidate="TBReason"
                                Enabled="false" EnableClientScript="false" Display="None" ErrorMessage='Поле "Обоснование" обязательно для заполнения' />
                        </tr>
                        
                    </table>
                                    </div>
                    <div class="statement_in" style="width: 80%;">
                    <div class="statement_table" style="padding-bottom: 0px;"> 
                    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                        <tr>
                            <th>
                                Какая информационная система (ИС) используется в Вашей ОО для ведения приемной кампании помимо ФИС ГИА и Приема?
                            </th>
                            <td>
                                <div id="AnotherNameDisable">
                                    <asp:DropDownList runat="server" ID="ddlIS" DataSourceID="dslIS" DataValueField="Id"
                                        DataTextField="Name" Width="370px">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource runat="server" ID="dslIS" SelectCommand="SELECT * FROM dbo.OrganizationIS ORDER BY [Name]"
                                        ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>">
                                    </asp:SqlDataSource>
                                </div>
                            </td>
                            
                        </tr>

                        <tr>  
                              <th>
                                Название используемой ИС
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="AnotherNameIs" CssClass="txt" MaxLength="100" />
                            </td>
                        </tr>
                    </table>
                        </div>
                        </div>

                <div class="statement_in" style="width: 80%;">
                    <p>
                        <strong>Модель приемной кампании:</strong></p>
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
                </div>
                <div class="statement_table">
                    <table width="840">
                        <%-- Прием по результатам ЕГЭ --%>
                        <tr class="left" id="trReceptionOnResultsCNE">
                            <td colspan="3" style="padding-bottom: 0; border-color: #fff;">
                                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                                    <tr>
                                        <th style="border-color: #fff;">
                                            Прием по результатам ЕГЭ (для&nbsp;ССУЗов&nbsp;и&nbsp;ВУЗов)
                                        </th>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 0; padding-bottom: 0; padding-top: 0; border-color: #fff;">
                                            <div class="tablenoborder">
                                                <asp:RadioButtonList ID="rblReceptionOnResultsCNE" runat="server" CssClass="radio-button-list">
                                                    <asp:ListItem Text="Проводится" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Не проводится" Value="1"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible="false">
                            <tr class="displayNone">
                                <td class="left" colspan="3" style="text-align: center; padding-top: 0; padding-bottom: 0;
                                    border-top-color: #fff;">
                                    <asp:CustomValidator ID="vlStruct" Visible="false" runat="server" ErrorMessage="Ошибки при заполнении сведений об объеме и структуре приема"
                                        EnableClientScript="true"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr class="displayNone">
                                <th class="left" colspan="3" style="text-align: center; padding-top: 22px; border-bottom-color: #fff;">
                                    Сведения об объеме и структуре приема<sup>(1)</sup>
                                </th>
                            </tr>
                            <tr class="displayNone" style="border-top-color: #fff;">
                                <td colspan="3" style="border-top-color: #fff;">
                                    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                                        <asp:PlaceHolder ID="phCommonError" runat="server" Visible="false">
                                            <tr>
                                                <td colspan="2" style="border-color: #fff; vertical-align: middle; width: 100%; padding-left: 0;">
                                                    <asp:Label ID="lblCommonError" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                        </asp:PlaceHolder>
                                        <tr>
                                            <th style="border-color: #fff; vertical-align: middle; width: 100%; padding-left: 0;
                                                border-top-color: #fff; font-weight: normal;">
                                                <asp:Label ID="lblFederalBudgetError" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                Контрольные цифры приема граждан, обучающихся за счет средств федерального бюджета<sup>(2)</sup>
                                            </th>
                                            <td class="text" style="border-color: #fff; vertical-align: middle; padding-right: 0;
                                                padding-left: 20px; border-top-color: #fff;">
                                                <asp:TextBox runat="server" ID="txtBxFederalBudget" CssClass="txt" Width="50px" MaxLength="5" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <th style="border-color: #fff; vertical-align: middle; width: 100%; padding-left: 0;
                                                font-weight: normal;">
                                                <asp:Label ID="lblTargetedError" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                Квоты по целевому приему
                                            </th>
                                            <td class="text" style="border-color: #fff; vertical-align: middle; padding-right: 0;
                                                padding-left: 20px;">
                                                <asp:TextBox runat="server" ID="txtBxTargeted" CssClass="txt" Width="50px" MaxLength="5" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <th style="border-color: #fff; vertical-align: middle; width: 100%; padding-left: 0;
                                                font-weight: normal;">
                                                <asp:Label ID="lblLocalBudgetError" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                Объем и структура приема обучающихся за счет средств бюджета субъектов Российской
                                                Федерации<sup>(3)</sup>
                                            </th>
                                            <td class="text" style="border-color: #fff; vertical-align: middle; padding-right: 0;
                                                padding-left: 20px;">
                                                <asp:TextBox runat="server" ID="txtBxLocalBudget" CssClass="txt" Width="50px" MaxLength="5" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <th style="border-color: #fff; vertical-align: middle; width: 100%; padding-left: 0;
                                                font-weight: normal;">
                                                <asp:Label ID="lblPayingError" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                Количество мест для обучения на основе договоров с оплатой стоимости обучения
                                            </th>
                                            <td class="text" style="border-color: #fff; vertical-align: middle; padding-right: 0;
                                                padding-left: 20px;">
                                                <asp:TextBox runat="server" ID="txtBxPaying" CssClass="txt" Width="50px" MaxLength="5" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <th colspan="2" style="vertical-align: middle; width: 100%; padding-left: 0; border-bottom-color: #fff;
                                                font-weight: normal;">
                                                Количество мест, выделенных для приема на различные формы обучения:
                                            </th>
                                        </tr>
                                        <tr>
                                            <th style="padding-top: 5px; vertical-align: middle; width: 100%; padding-left: 0;
                                                text-align: right; border-color: #fff; font-weight: normal;">
                                                <asp:Label ID="lblFullTimeError" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                Очная
                                            </th>
                                            <td class="text" style="border-color: #fff; padding-top: 5px; vertical-align: middle;
                                                padding-right: 0; padding-left: 20px;">
                                                <asp:TextBox runat="server" ID="txtBxFullTime" CssClass="txt" Width="50px" MaxLength="5" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <th style="padding-top: 5px; vertical-align: middle; width: 100%; padding-left: 0;
                                                text-align: right; border-color: #fff; font-weight: normal;">
                                                <asp:Label ID="lblEveningError" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                Очно-заочная
                                            </th>
                                            <td class="text" style="border-color: #fff; padding-top: 5px; vertical-align: middle;
                                                padding-right: 0; padding-left: 20px;">
                                                <asp:TextBox runat="server" ID="txtBxEvening" CssClass="txt" Width="50px" MaxLength="5" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <th style="padding-top: 5px; vertical-align: middle; width: 100%; padding-left: 0;
                                                text-align: right; border-color: #fff; font-weight: normal;">
                                                <asp:Label ID="lblPostalError" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                Заочная
                                            </th>
                                            <td class="text" style="border-color: #fff; padding-top: 5px; vertical-align: middle;
                                                padding-right: 0; padding-left: 20px;">
                                                <asp:TextBox runat="server" ID="txtBxPostal" CssClass="txt" Width="50px" MaxLength="5" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </asp:PlaceHolder>
                        <tr>
                            <th class="left" colspan="3" style="text-align: center; padding-top: 20; border-bottom-color: #fff;
                                border-bottom-width: ">
                                Сведения о защищенном подключении
                            </th>
                        </tr>
                        <tr>
                            <td colspan="3" style="border-top-color: #fff;">
                                <table>
                                    <%-- <tr>
                                        <th style="padding-left: 0; border-top-color: #fff;">
                                            Статус подключения
                                        </th>
                                        <td style="border-top-color: #fff; padding-right: 12px;">
                                            <asp:DropDownList runat="server" ID="ddlConnectionStatus" DataValueField="Id" DataTextField="Name"
                                                CssClass="sel" DataSourceID="sdsConnectionStatus" />
                                            <asp:SqlDataSource runat="server" ID="sdsConnectionStatus" SelectCommand="SELECT * FROM dbo.ConnectionStatus"
                                                ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>">
                                            </asp:SqlDataSource>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th style="padding-left: 0;">
                                            Схема подключения
                                        </th>
                                        <td style=" padding-right: 12px;">
                                            <asp:DropDownList runat="server" ID="ddlConnectionScheme" DataValueField="Id" DataTextField="Name"
                                                CssClass="sel" DataSourceID="sdsConnectionScheme" />
                                            <asp:SqlDataSource runat="server" ID="sdsConnectionScheme" SelectCommand="SELECT * FROM dbo.ConnectionScheme"
                                                ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>">
                                            </asp:SqlDataSource>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <th style="padding-left: 0;">
                                            Письмо о переносе сроков
                                        </th>
                                        <td>
                                            <asp:FileUpload runat="server" ID="fuLetterToReschedule" Width="300" />
                                            <asp:HiddenField runat="server" ID="hfLetterToReschedule" />
                                            <asp:Label runat="server" ID="lLetterToReschedule" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <th style="padding-left: 0;">
                                            Срок подключения к защищенной сети
                                        </th>
                                        <td>
                                            <div style="white-space: nowrap; padding: 0;">
                                                <table style="border-width: 0; padding: 0; width: 100%;">
                                                    <tr>
                                                        <td style="border-width: 0; padding: 0; padding-right: 10px;">
                                                            <uc:DatePicker runat="server" ID="tbTimeConnectionToSecureNetwork" MinDate="01.01.2000" />
                                                        </td>
                                                        <td style="border-width: 0; padding: 0; vertical-align: middle;">
                                                            Согласовано:
                                                        </td>
                                                        <td style="border-width: 0; padding: 0; vertical-align: middle; padding-top: 2px;">
                                                            <asp:CheckBox ID="chbConnect" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th style="padding-left: 0; border-bottom-color: #fff;">
                                            Срок внесения сведений в ФИС ЕГЭ и приема
                                        </th>
                                        <td style="border-bottom-color: #fff;">
                                            <div style="white-space: nowrap; padding: 0;">
                                                <table style="border-width: 0; padding: 0; width: 100%;">
                                                    <tr>
                                                        <td style="border-width: 0; padding: 0; padding-right: 10px;">
                                                            <uc:DatePicker runat="server" ID="tbTimeEnterInformationInFIS" MinDate="01.01.2000" />
                                                        </td>
                                                        <td style="border-width: 0; padding: 0; vertical-align: middle;">
                                                            Согласовано:
                                                        </td>
                                                        <td style="border-width: 0; padding: 0; vertical-align: middle; padding-top: 2px;">
                                                            <asp:CheckBox ID="chbEnterInf" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr class="displayNone">
                            <td class="left" colspan="3" style="padding-top: 20px; text-align: center; border-bottom-color: #fff;">
                                <strong>Детальная информация об ОУ</strong>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="statement_in" style="border-bottom-color: #fff;">
                    <p>
                        <strong>Полное наименование</strong></p>
                    <p>
                        <asp:TextBox runat="server" ID="TBFullName" CssClass="textareaoverflowauto" Rows="3"
                            Height="50px" TextMode="MultiLine" /></p>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TBFullName"
                        EnableClientScript="false" Display="None" ErrorMessage='Поле "Полное наименование" обязательно для заполнения' />
                    <p>
                        <strong>Краткое наименование</strong></p>
                    <p>
                        <asp:TextBox runat="server" ID="TBShortName" CssClass="textareaoverflowauto" Rows="3"
                            Height="50px" TextMode="MultiLine" /></p>
                </div>
                <div class="statement_table">
                    <table>
                        <tr>
                            <th>
                                ОГРН
                            </th>
                            <td colspan="2">
                                <asp:TextBox runat="server" ID="TBOGRN" CssClass="txt long" MaxLength="13" Width="585" />
                            </td>
                        </tr>
                        <asp:RequiredFieldValidator ID="VReqOGRN" runat="server" ControlToValidate="TBOGRN"
                            EnableClientScript="false" Display="None" ErrorMessage='Поле "ОГРН" обязательно для заполнения' />
                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="TBOGRN"
            EnableClientScript="false" Display="None" ErrorMessage='Поле "ОГРН" заполнено неверно'
            ValidationExpression="[0-9]{13}"></asp:RegularExpressionValidator>--%>
                        <tr>
                            <th>
                                ИНН
                            </th>
                            <td colspan="2">
                                <asp:TextBox runat="server" ID="TBINN" CssClass="txt long" MaxLength="10" Width="585" />
                            </td>
                        </tr>
                        <asp:RequiredFieldValidator ID="VReqINN" runat="server" ControlToValidate="TBINN"
                            EnableClientScript="false" Display="None" ErrorMessage='Поле "ИНН" обязательно для заполнения' />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TBINN"
                            EnableClientScript="false" Display="None" ErrorMessage='Поле "ИНН" заполнено неверно'
                            ValidationExpression="[0-9]{10}"></asp:RegularExpressionValidator>
                        <tr>
                            <th>
                                КПП
                            </th>
                            <td colspan="2">
                                <asp:TextBox runat="server" ID="tbKPP" CssClass="txt long" MaxLength="9" Width="585" />
                            </td>
                        </tr>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbKPP"
                            EnableClientScript="false" Display="None" ErrorMessage='Поле "КПП" обязательно для заполнения' />
                        <asp:RegularExpressionValidator ID="revKPP" runat="server" ControlToValidate="tbKPP"
                            EnableClientScript="false" Display="None" ErrorMessage='Поле "КПП" должно содержать 9 цифр'
                            ValidationExpression="[0-9]{9}"></asp:RegularExpressionValidator>
                        <tr class="displayNone">
                            <th>
                                Учредитель (для&nbsp;ССУЗов&nbsp;и&nbsp;ВУЗов)
                            </th>
                            <td runat="server" id="tdFounders" colspan="2" class='founders-cell'>
                                 
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="width: 820px;">
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
                    </table>
                </div>
                <div class="statement_in" style="border-bottom-color: #fff;">
                    <p>
                        <strong>Фактический адрес</strong></p>
                    <p>
                        <asp:TextBox runat="server" ID="TBFactAddress" Rows="3" Height="50px" TextMode="MultiLine"
                            MaxLength="500" CssClass="textareaoverflowauto" Width="820" />
                    </p>
                    <p>
                        <strong>Юридический адрес</strong></p>
                    <p>
                        <asp:TextBox runat="server" ID="TBJurAddress" CssClass="textareaoverflowauto" Rows="3"
                            Height="50px" TextMode="MultiLine" MaxLength="500" Width="820" />
                    </p>
                </div>
                <div class="statement_table">
                    <table>
                        <tr class="displayNone">
                            <th>
                                Код организации в системе ИСЛОД
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="TBIslodGUID" CssClass="txt" Width="575"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="statement_table">
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
                    <table>
                        <tr class="displayNone">
                            <th style="padding-right: 14px;">
                                Свидетельство об аккредитации
                            </th>
                            <td colspan="2">
                                <asp:TextBox runat="server" ID="TBAccredCert" CssClass="txt long" MaxLength="200"
                                    Width="585" />
                            </td>
                        </tr>
                        <%--<tr>
                            <th style="padding-right: 14px;">
                                Номер лицензии
                            </th>
                            <td colspan="2" style="vertical-align: middle;">
                                <asp:Literal runat="server" ID="lLicenseNumber"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <th style="padding-right: 14px;">
                                Дата выдачи лицензии
                            </th>
                            <td colspan="2" style="vertical-align: middle;">
                                <asp:Literal runat="server" ID="lLicenseIssueDate"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <th style="padding-right: 14px;">
                                Статус лицензии
                            </th>
                            <td colspan="2" style="vertical-align: middle;">
                                <asp:Literal runat="server" ID="lLicenseStatus"></asp:Literal>
                                <br />
                                <asp:Label runat="server" ID="lLicenseStatusWarning" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <th style="padding-right: 14px;">
                                Номер приложения к лицензии
                            </th>
                            <td colspan="2" style="vertical-align: middle;">
                                <asp:Literal runat="server" ID="lSupplementNumber"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <th style="padding-right: 14px;">
                                Дата выдачи приложения к лицензии
                            </th>
                            <td colspan="2" style="vertical-align: middle;">
                                <asp:Literal runat="server" ID="lSupplementOrderDocumentDate"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <th style="padding-right: 14px;">
                                Статус приложения к лицензии
                            </th>
                            <td colspan="2" style="vertical-align: middle;">
                                <asp:Literal runat="server" ID="lSupplementStatusName"></asp:Literal>
                                <br />
                                <asp:Label runat="server" ID="Label1" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>--%>
                        <tr>
                            <th>
                                Код города
                            </th>
                            <td style="padding-right: 10px;">
                                <asp:TextBox runat="server" ID="TBCityCode" CssClass="txt" MaxLength="10" Width="40px" />
                            </td>
                            <th>
                                Телефон&nbsp;
                                <asp:TextBox runat="server" ID="TBPhone" CssClass="txt" MaxLength="255" Width="450px" />
                            </th>
                        </tr>
                        <tr>
                            <th>
                                Факс
                            </th>
                            <td colspan="2">
                                <asp:TextBox runat="server" ID="TBFax" CssClass="txt" MaxLength="100" Width="585" />
                            </td>
                        </tr>
                        <tr>
                            <th>
                                E-mail
                            </th>
                            <td colspan="2">
                                <asp:TextBox runat="server" class="email" ID="TBEMail" CssClass="txt" MaxLength="100"
                                    Width="585" />
                            </td>
                            <asp:RegularExpressionValidator runat="server" ID="vldEmailFormat" ControlToValidate="TBEMail"
                                EnableClientScript="false" Display="None" ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*\.{0,1}@([0-9a-zA-Z]*[-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$"
                                ErrorMessage='Поле "E-mail" заполнено неверно' />
                        </tr>
                        <tr>
                            <th>
                                Сайт
                            </th>
                            <td colspan="2">
                                <asp:TextBox runat="server" ID="TBWebPage" CssClass="txt long" MaxLength="100" Width="585" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="statement_in" style="border-bottom-color: #fff; padding-top: 20px;">
                    <p>
                        <strong>Пользователи</strong></p>
                    <div class="statement_table">
                        <asp:Repeater runat="server" ID="rptUsers">
                            <HeaderTemplate>
                                <table class="table-th" width="100%">
                                    <tr class="th">
                                        <td class="left-th" width="25%" style="vertical-align: middle;">
                                            <div style="font-weight: bold;">
                                                Логин</div>
                                        </td>
                                        <td width="45%" style="vertical-align: middle;">
                                            <div style="font-weight: bold;">
                                                ФИО</div>
                                        </td>
                                        <td class="right-th" width="30%" style="vertical-align: middle;">
                                            <div style="font-weight: bold;">
                                                Статус</div>
                                        </td>
                                    </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <a href="../../Accounts/Users/View.aspx?login=<%# Eval("Login") %>">
                                            <%# Eval("Login") %></a>
                                    </td>
                                    <td>
                                        <%# Eval("FIO") %>
                                        <%# String.IsNullOrEmpty(Eval("FIO").ToString()) ? "" : "<br>"%>
                                        <a href="mailto:<%# Convert.ToString(Eval("email")) %>">
                                            <%# Convert.ToString(Eval("email")) %></a>
                                    </td>
                                    <td>
                                        <%# GetUserStatus((string)Eval("Status")) %>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                        <asp:Label ID="lblNoUsers" runat="server" Visible="false" Text="Нет пользователей" />
                    </div>
                </div>
                <div class="statement_table">
                    <table width="840">
                        <tr>
                            <td colspan="2" id="noteTd" class="box-submit" style="text-align: left; font-size: .8em;">
                                <%--(1) Сведения об объеме и структуре приема заполняются для ОУ в целом<br />
                                (2) Заполняется только для федеральных ОУ<br />
                                (3) Заполняется только для региональных ОУ--%>
                            </td>
                            <td class="box-submit" colspan="3">
                                <div style="float: right; margin-right: 5px;">
                                    <asp:Button runat="server" ID="btnUpdate" Text="Сохранить" CssClass="bt" OnClick="BtnUpdateClick"
                                        OnClientClick="btnUpdateClick()" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="HdnOrgStatus" runat="server" />
        </form>
    </div>
    <div id="CalendarContainer" style="position: absolute; visibility: hidden; background-color: white;
        layer-background-color: white;">
    </div
</asp:Content>

