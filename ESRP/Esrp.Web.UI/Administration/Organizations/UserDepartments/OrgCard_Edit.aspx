<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Templates/Administration.Master"
    AutoEventWireup="true" CodeBehind="OrgCard_Edit.aspx.cs" Inherits="Esrp.Web.Administration.Organizations.UserDepartments.OrgCard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <script type="text/javascript" src="/Common/Scripts/tiny_mce/tiny_mce.js"></script>
	<script type="text/javascript" src="/Common/Scripts/Utils.js"></script>
    <script type="text/javascript" src="/Common/Scripts/CalendarPopup.js"></script>
	<script type="text/javascript">

	    $(document).ready(function () {
	        // Если значение пусто, то скрываем контрол загрузки файла, иначе скрываем контрол с ссылками на файл
	        if ($('#<%=hfLetterToReschedule.ClientID %>').val() != "") {
	            $("#<%=fuLetterToReschedule.ClientID %>").hide();
	        }
	        else {
	            $("#<%=lLetterToReschedule.ClientID %>").hide();
	        }

	        // Если нажата ссылка удалить файл,то скрываем контрол с ссылками на файл и показываем контрол загрузки файла, в скрытое поле заносим значение пусто
	        $("#deleteLetter").click(function () {
	            $("#<%=lLetterToReschedule.ClientID %>").hide();
	            $("#<%=fuLetterToReschedule.ClientID %>").show();
	            $('#<%=hfLetterToReschedule.ClientID %>').val("");
	        });
	        
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
	    });
	    
	    function VisibleFieldsFounder() {
	        $(".displayNone").css("display", "none");
	        $("#noteTd").css("display", "none");
	        $("#<%= DDLOPF.ClientID %>").val("0").attr("disabled", "disabled");
	        $("#<%= DDLOrgKindsFounder.ClientID %>").css("display", "block");
	        $("#<%= DDLOrgKinds.ClientID %>").css("display", "none");
	        $("#<%= hiddenKindsFounder.ClientID %>").val("0"); 
	    }

	    function VisibleAllFields() {
	        $(".displayNone").css("display", "table-row");
	        $("#noteTd").css("display", "table-cell");
	        var DDLOPF = $("#<%= DDLOPF.ClientID %>");
	        if (DDLOPF.attr("disabled")) {
	            DDLOPF.attr("disabled", false);    
	        }
	        else {
	            DDLOPF.attr("enabled", "enabled");
	        }
	        $("#<%= DDLOrgKindsFounder.ClientID %>").css("display", "none");
	        $("#<%= DDLOrgKinds.ClientID %>").css("display", "block");
	        $("#<%= hiddenKindsFounder.ClientID %>").val("1");
	    }

	    function btnUpdateClick() {
	        if ($("#<%=DDLOrgTypes.ClientID %>").val() == 6) {
	            $("#<%= TBINN.ClientID %>").val("0000000000");
	            $("#<%= TBOGRN.ClientID %>").val("0000000000000");
	            $("#<%= txtBxFederalBudget.ClientID %>").val("1");
	        }
	    }
        
	    function EnableDescription() {
	        $("#<%= txtBxDescription.ClientID %>")
	        .removeAttr("disabled")
	        .css("background", "#fff")
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
<asp:Content ID="Content3" ContentPlaceHolderID="cphActions" runat="server">
<div class="h10">
	</div>
	<div class="border-block">
		<div class="tr">
			<div class="tt">
				<div>
				</div>
			</div>
		</div>
		<div class="content">
			<ul>
				<li><a href="/Administration/Organizations/OrganizationHistory.aspx?OrgId=<%= GetParamInt("OrgID") %>"
					title="История изменений" class="gray">История изменений</a></li>
			</ul>
		</div>
		<div class="br">
			<div class="tt">
				<div>
				</div>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphContent" runat="server">
    <form id="Form1" runat="server">
    <asp:ValidationSummary CssClass="error_block"  ID="ValidationSummary1" runat="server" DisplayMode="BulletList"
        EnableClientScript="false" HeaderText="<p>Произошли следующие ошибки:</p>" />
    <asp:Label ID="lblError" Visible="False" runat="server" ForeColor="Red" />
    <table class="form">
        <tr>
            <td class="left" width="300px">
                Регион
            </td>
            <td class="text" colspan="2">
                <asp:DropDownList runat="server" ID="DDLRegions" DataSourceID="DSRegions" CssClass="sel long"
                    DataValueField="Id" DataTextField="Name" Width="370px">
                </asp:DropDownList>
                <asp:SqlDataSource runat="server" ID="DSRegions" SelectCommand="SELECT * FROM dbo.Region WHERE InOrganizationEtalon=1 ORDER BY [Name]"
                    ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>">
                </asp:SqlDataSource>
            </td>
        </tr>
        <tr>
            <td class="left">
                Тип
            </td>
            <td colspan="2">
                <asp:DropDownList runat="server" ID="DDLOrgTypes" DataSourceID="DSOrgTypes" CssClass="sel"
                    DataValueField="Id" DataTextField="Name">
                </asp:DropDownList>
                <asp:SqlDataSource runat="server" ID="DSOrgTypes" SelectCommand="SELECT * FROM dbo.OrganizationType2010 order by SortOrder"
                    ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>">
                </asp:SqlDataSource>
            </td>
          
        </tr>
        <tr><td class="left">Вид</td>
          <td colspan="2">
                <asp:HiddenField ID="hiddenKindsFounder" runat="server"/>
                <asp:DropDownList runat="server" ID="DDLOrgKinds" DataSourceID="DSKinds" CssClass="sel"
                    DataValueField="Id" DataTextField="Name">
                </asp:DropDownList>
                 <asp:SqlDataSource runat="server" ID="DSKinds" SelectCommand="SELECT * FROM dbo.OrganizationKind  WHERE Id <> 9 order by SortOrder"
                    ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>">
                </asp:SqlDataSource>
                <asp:DropDownList runat="server" ID="DDLOrgKindsFounder" DataSourceID="DSKindsFounder" CssClass="sel"
                    DataValueField="Id" DataTextField="Name">
                </asp:DropDownList>
                 <asp:SqlDataSource runat="server" ID="DSKindsFounder" SelectCommand="SELECT * FROM dbo.OrganizationKind WHERE Id in (8,9) order by SortOrder"
                    ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>">
                </asp:SqlDataSource>
            </td>
        </tr>
        <tr>
            <td class="left">
                Организационно-правовая форма
            </td>
            <td colspan="2" style="vertical-align:middle;">
                <asp:DropDownList runat="server" ID="DDLOPF" CssClass="sel">
                    <asp:ListItem Text="Государственный" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Негосударственный" Value="1"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr class="displayNone">
            <td class="left">
                <asp:CheckBox runat="server" ID="cbFil" Text="&nbsp;является филиалом" />
            </td>
            <td colspan="2">
                <asp:Button ID="btnFilial" runat="server" Width="250px" 
                    Text="Выбрать головную организацию" CausesValidation="False" 
                    onclick="btnFilial_Click" />
            </td>
        </tr>
        <tr class="displayNone">
            <td class="left">&nbsp;</td>
            <td id="filname" colspan="2" style="padding-top: 0px;">
                <asp:Label ID="lblMainOrgName" runat="server" Text="" ></asp:Label>
                <asp:HiddenField ID="hfMainOrgId" runat="server" />
            </td>
        </tr>
        <tr>
			<td class="left">
				Статус
			</td>
			<td colspan="2" style="vertical-align:middle;">
				<asp:DropDownList runat="server" ID="DDLOrgStatus" CssClass="sel">
					<asp:ListItem Text="Действующая" Value="1"></asp:ListItem>
					<asp:ListItem Text="Реорганизованная" Value="2"></asp:ListItem>
					<asp:ListItem Text="Ликвидированная" Value="3"></asp:ListItem>
				</asp:DropDownList>
                <asp:CustomValidator runat="server" ID="CustomValidator1" ErrorMessage="Не указана новая организация" />
				<br />
				<div class="smallspacer"></div>
				<div id="reorganizedTo">
					<asp:Button ID="btnReorganizedTo" runat="server" Width="250px" 
					Text="Выбрать новую организацию" CausesValidation="False" 
					onclick="btnReorganizedTo_Click" />
					<div class="smallspacer"></div>
					<asp:Label ID="lblNewOrgName" runat="server" Text="" ></asp:Label>
					<asp:HiddenField ID="hfNewOrgId" runat="server" />
				</div>
			</td>
		</tr>
        <tr id="dateChangeStatus">
            <td class="left">
                Фактическая дата изменения статуса
            </td>
            <td class="text">
                <asp:TextBox runat="server" ID="TbDateChangeStatus" CssClass="txt date" />
                <img src="/Common/Images/ico-datepicker-Esrp.gif" id="BtDateChangeStatus" onclick='return PickDate("<%=TbDateChangeStatus.ClientID%>", "BtDateChangeStatus", "CalendarContainer");' />
            </td>
        </tr>
        <asp:RequiredFieldValidator ID="RequiredDateValidator" runat="server" ControlToValidate="TbDateChangeStatus" Enabled="false"
            EnableClientScript="false" Display="None" ErrorMessage='Поле "Фактическая дата изменения статуса" обязательно для заполнения' />
        <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ErrorMessage="Неверный формат даты"
            ControlToValidate="TbDateChangeStatus" EnableClientScript="false" Display="None" ValidationExpression="^\d{1,2}\.\d{1,2}\.(?:\d{4}|\d{2})$" />     
        <tr id="reasonChangeStatus">
            <td class="left">
                Обоснование            
            </td>
            <td colspan="2">
                <asp:TextBox runat="server" ID="TBReason" CssClass="txt" MaxLength="100" />
            </td>
            <asp:RequiredFieldValidator ID="RequiredReasonValidator" runat="server" ControlToValidate="TBReason" Enabled="false"
            EnableClientScript="false" Display="None" ErrorMessage='Поле "Обоснование" обязательно для заполнения' />
        </tr>
        <tr class="displayNone"><td colspan="3" style="padding-bottom: 0;">
            
            <table id="tblRecruitmentCampaigns" cellpadding="0" cellspacing="0" border="0" style="width: 100%;">

                <tr>
                    <td style="padding-left: 0;">
                        Модель приемной кампании:
                    </td>
                </tr>
                
                <tr>
                    <td style="padding-left: 0; padding-bottom: 0; padding-top: 0;">
                        <asp:RadioButtonList ID="rblRecruitmentCampaigns" DataSourceID="dsRecruitmentCampaigns" runat="server" 
                            DataValueField="Id" DataTextField="ModelName">
                        </asp:RadioButtonList>
                        <asp:SqlDataSource runat="server" ID="dsRecruitmentCampaigns" SelectCommand="SELECT Id, ' ' + ModelName + (case Id when 999 then ':' else '' end)  as ModelName FROM [dbo].[RecruitmentCampaigns] ORDER BY [Id]"
                            ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>">
                        </asp:SqlDataSource>
                    </td>
                </tr>
                
                <tr>
                    <td style="padding-left: 7px; padding-top: 0;">
                        <asp:TextBox runat="server" ID="txtBxDescription" CssClass="txt long" Rows="3" Height="50px" Width="97%"
                            TextMode="MultiLine" MaxLength="400" />
                    </td>
                </tr>
                
            </table>
        </td></tr>

        <%-- Прием по результатам ЕГЭ --%>
        <tr class="left" id="trReceptionOnResultsCNE">
            <td colspan="3" style="padding-bottom: 0;">            
                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                    <tr>
                        <td style="padding-left: 0;">
                            Прием по результатам ЕГЭ (для&nbsp;ССУЗов&nbsp;и&nbsp;ВУЗов)
                        </td>
                    </tr>
                
                    <tr>
                        <td style="padding-left: 0; padding-bottom: 0; padding-top: 0;">
                            <asp:RadioButtonList ID="rblReceptionOnResultsCNE" runat="server" CssClass="radio-button-list" >
                                <asp:ListItem Text="Проводится" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Не проводится" Value="1"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>             
                </table>
            </td>
        </tr>

        <tr class="displayNone">
            <td class="left" colspan="3" style="text-align: center; padding-top: 0;"><asp:CustomValidator ID="vlStruct" Visible="false" runat="server" ErrorMessage="Ошибки при заполнении сведений об объеме и структуре приема" EnableClientScript="true"></asp:CustomValidator></td>
        </tr>
        <tr class="displayNone">
            <td class="left" colspan="3" style="text-align: center; padding-top: 0;">Сведения об объеме и структуре приема<sup>(1)</sup></td>
        </tr>
        <tr class="displayNone"><td colspan="3">
            
            <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">

                <asp:PlaceHolder ID="phCommonError" runat="server" Visible="false">
                    <tr>
                        <td colspan="2" style="vertical-align: middle; width: 100%; padding-left: 0;">
                            <asp:Label ID="lblCommonError" runat="server" Text="" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </asp:PlaceHolder>

                <tr>
                    <td style="vertical-align: middle; width: 100%; padding-left: 0;">
                        <asp:Label ID="lblFederalBudgetError" runat="server" Text="" ForeColor="Red"></asp:Label>
                        Контрольные цифры приема граждан, обучающихся за счет средств федерального бюджета<sup>(2)</sup>
                    </td>
                    <td class="text" style="vertical-align: middle; padding-right: 0; padding-left: 20px;">
                        <asp:TextBox runat="server" ID="txtBxFederalBudget" CssClass="txt" Width="50px" MaxLength="5" />
                    </td>
                </tr>
                
                <tr>
                    <td style="vertical-align: middle; width: 100%; padding-left: 0;">
                        <asp:Label ID="lblTargetedError" runat="server" Text="" ForeColor="Red"></asp:Label>
                        Квоты по целевому приему
                    </td>
                   <td class="text" style="vertical-align: middle; padding-right: 0; padding-left: 20px;">
                        <asp:TextBox runat="server" ID="txtBxTargeted" CssClass="txt" Width="50px"  MaxLength="5" />
                    </td>
                </tr>
                
                <tr>
                    <td style="vertical-align: middle; width: 100%; padding-left: 0;">
                        <asp:Label ID="lblLocalBudgetError" runat="server" Text="" ForeColor="Red"></asp:Label>
                        Объем и структура приема обучающихся за счет средств бюджета субъектов Российской Федерации<sup>(3)</sup>
                    </td>
                    <td class="text" style="vertical-align: middle; padding-right: 0; padding-left: 20px;">
                        <asp:TextBox runat="server" ID="txtBxLocalBudget" CssClass="txt" Width="50px"  MaxLength="5" />
                    </td>
                </tr>
                
                <tr>
                    <td style="vertical-align: middle; width: 100%; padding-left: 0;">
                        <asp:Label ID="lblPayingError" runat="server" Text="" ForeColor="Red"></asp:Label>
                        Количество мест для обучения на основе договоров с оплатой стоимости обучения
                    </td>
                    <td class="text" style="vertical-align: middle; padding-right: 0; padding-left: 20px;">
                        <asp:TextBox runat="server" ID="txtBxPaying" CssClass="txt" Width="50px"  MaxLength="5" />
                    </td>
                </tr>

                <tr>
                    <td colspan="2" style="vertical-align: middle; width: 100%; padding-left: 0;">Количество мест, выделенных для приема на различные формы обучения:</td>
                </tr>

                <tr>
                    <td style="padding-top: 5px; vertical-align: middle; width: 100%; padding-left: 0; text-align: right;">
                        <asp:Label ID="lblFullTimeError" runat="server" Text="" ForeColor="Red"></asp:Label>
                        Очная
                    </td>
                    <td class="text" style="padding-top: 5px; vertical-align: middle; padding-right: 0; padding-left: 20px;">
                        <asp:TextBox runat="server" ID="txtBxFullTime" CssClass="txt" Width="50px"  MaxLength="5" />
                    </td>
                </tr>

                <tr>
                    <td style="padding-top: 5px; vertical-align: middle; width: 100%; padding-left: 0; text-align: right">
                        <asp:Label ID="lblEveningError" runat="server" Text="" ForeColor="Red"></asp:Label>
                        Очно-заочная
                    </td>
                    <td class="text" style="padding-top: 5px; vertical-align: middle; padding-right: 0; padding-left: 20px;">
                        <asp:TextBox runat="server" ID="txtBxEvening" CssClass="txt" Width="50px"  MaxLength="5" />
                    </td>
                </tr>

                <tr>
                    <td style="padding-top: 5px; vertical-align: middle; width: 100%; padding-left: 0; text-align: right">
                        <asp:Label ID="lblPostalError" runat="server" Text="" ForeColor="Red"></asp:Label>
                        Заочная
                    </td>
                    <td class="text" style="padding-top: 5px; vertical-align: middle; padding-right: 0; padding-left: 20px;">
                        <asp:TextBox runat="server" ID="txtBxPostal" CssClass="txt" Width="50px"  MaxLength="5" />
                    </td>
                </tr>

            </table>
            
        </td></tr>

         <tr>
            <td class="left" colspan="3" style="text-align: center; padding-top: 0;">Сведения о защищенном подключении</td>
        </tr>

        <tr>
            <td colspan="3">
                <table>
                    <tr>
                        <td class="left" style="padding-left: 0;">
                            Статус подключения
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlConnectionStatus" DataValueField="Id" DataTextField="Name" CssClass="sel" DataSourceID="sdsConnectionStatus"/>
                             <asp:SqlDataSource runat="server" ID="sdsConnectionStatus" SelectCommand="SELECT * FROM dbo.ConnectionStatus"
                                ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>">
                            </asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td class="left" style="padding-left: 0;">
                            Схема подключения
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlConnectionScheme" DataValueField="Id" DataTextField="Name" CssClass="sel" DataSourceID="sdsConnectionScheme"/>
                            <asp:SqlDataSource runat="server" ID="sdsConnectionScheme" SelectCommand="SELECT * FROM dbo.ConnectionScheme"
                                ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>">
                            </asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td class="left" style="padding-left: 0;">
                            Письмо о переносе сроков
                        </td>
                        <td>
                            <asp:FileUpload runat="server" ID="fuLetterToReschedule" />
                            <asp:HiddenField runat="server" ID="hfLetterToReschedule"/>
                            <asp:Label runat="server" ID="lLetterToReschedule" />
                        </td>
                    </tr>
                    <tr>
                        <td class="left" style="padding-left: 0;">
                            Срок подключения к защищенной сети
                        </td>
                        <td class="text">
                            <asp:TextBox runat="server" ID="tbTimeConnectionToSecureNetwork" CssClass="txt date" />
                            <img src="/Common/Images/ico-datepicker-Esrp.gif" id="BtTimeConnectionToSecureNetwork" onclick='return PickDate("<%=tbTimeConnectionToSecureNetwork.ClientID%>", "BtTimeConnectionToSecureNetwork", "CalendarContainer");' />
                        </td>
                    </tr>
                    <tr>
                        <td class="left" style="padding-left: 0;">
                            Срок внесения сведений в ФИС ЕГЭ и приема
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="tbTimeEnterInformationInFIS" CssClass="txt date" />
                            <img src="/Common/Images/ico-datepicker-Esrp.gif" id="BtTimeEnterInformationInFIS" onclick='return PickDate("<%=tbTimeEnterInformationInFIS.ClientID%>", "BtTimeEnterInformationInFIS", "CalendarContainer");' />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>

        <tr class="displayNone">
            <td class="left" colspan="3" style="text-align: center;">Детальная информация об ОУ</td>
        </tr>        
        <tr>
            <td class="left">
                Полное наименование
            </td>
            <td class="text" colspan="2">
                <asp:TextBox runat="server" ID="TBFullName" CssClass="txt long" Rows="3" Height="50px"
                    TextMode="MultiLine" />
            </td>
        </tr>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TBFullName"
            EnableClientScript="false" Display="None" ErrorMessage='Поле "Полное наименование" обязательно для заполнения' />
        <tr>
            <td class="left">
                Краткое наименование
            </td>
            <td class="text" colspan="2">
                <asp:TextBox runat="server" ID="TBShortName" CssClass="txt long" Rows="3" Height="50px"
                    TextMode="MultiLine" />
            </td>
        </tr>

        <tr class="displayNone">
            <td class="left">
                ОГРН
            </td>
            <td class="text" colspan="2">
                <asp:TextBox runat="server" ID="TBOGRN" CssClass="txt long" MaxLength="13" />
            </td>
        </tr>

        <asp:RequiredFieldValidator ID="VReqOGRN" runat="server" ControlToValidate="TBOGRN"
            EnableClientScript="false" Display="None" ErrorMessage='Поле "ОГРН" обязательно для заполнения' />
        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="TBOGRN"
            EnableClientScript="false" Display="None" ErrorMessage='Поле "ОГРН" заполнено неверно'
            ValidationExpression="[0-9]{13}"></asp:RegularExpressionValidator>

        <tr class="displayNone">
            <td class="left">
                ИНН
            </td>
            <td class="text" colspan="2">
                <asp:TextBox runat="server" ID="TBINN" CssClass="txt long" MaxLength="10" />
            </td>
        </tr>
        <asp:RequiredFieldValidator ID="VReqINN" runat="server" ControlToValidate="TBINN"
            EnableClientScript="false" Display="None" ErrorMessage='Поле "ИНН" обязательно для заполнения' />
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TBINN"
            EnableClientScript="false" Display="None" ErrorMessage='Поле "ИНН" заполнено неверно'
            ValidationExpression="[0-9]{10}"></asp:RegularExpressionValidator>

        <tr>
            <td class="left">
                КПП
            </td>
            <td class="text" colspan="2">
                <asp:TextBox runat="server" ID="tbKPP" CssClass="txt long" MaxLength="9" />
            </td>
        </tr>
        <asp:RegularExpressionValidator ID="revKPP" runat="server" ControlToValidate="tbKPP"
            EnableClientScript="false" Display="None" ErrorMessage='Поле "КПП" должно содержать 9 цифр'
            ValidationExpression="[0-9]{9}"></asp:RegularExpressionValidator>

        <tr class="displayNone">
            <td class="left">
                Учредитель (для&nbsp;ССУЗов&nbsp;и&nbsp;ВУЗов)
            </td>
            <td colspan="2">
                <asp:DropDownList runat="server" ID="DDLOwnerDepartmentId" DataSourceID="DSOwnerDepartmentId" CssClass="sel long"
                    DataValueField="Id" DataTextField="ShortName" Width="370px">
                </asp:DropDownList>
                <asp:SqlDataSource runat="server" ID="DSOwnerDepartmentId" SelectCommand="SELECT -1 Id, '<Не определен>' ShortName UNION ALL SELECT Id, ShortName FROM dbo.Organization2010 WHERE TypeId = 6 ORDER BY Id"
                    ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>">
                </asp:SqlDataSource>
            </td>
        </tr>
        <tr>
            <td class="left">
                Должность&nbsp;руководителя
            </td>
            <td colspan="2">
                <asp:TextBox runat="server" ID="TBDirectorPosition" CssClass="txt long" MaxLength="255" />
            </td>
        </tr>
        <tr>
            <td class="left">
                ФИО&nbsp;руководителя
            </td>
            <td colspan="2">
                <asp:TextBox runat="server" ID="TBDirectorName" CssClass="txt long" MaxLength="255" />
            </td>
        </tr>
        <tr>
            <td class="left">
                Фактический адрес
            </td>
            <td colspan="2">
                <asp:TextBox runat="server" ID="TBFactAddress" CssClass="txt long" Rows="3" Height="50px"
                    TextMode="MultiLine" MaxLength="500" />
            </td>
        </tr>
         <tr>
            <td class="left">
                Юридический адрес
            </td>
            <td colspan="2">
                <asp:TextBox runat="server" ID="TBJurAddress" CssClass="txt long" Rows="3" Height="50px"
                    TextMode="MultiLine" MaxLength="500" />
            </td>
        </tr>
         <tr class="displayNone">
            <td class="left">
                Свидетельство об аккредитации
            </td>
            <td colspan="2">
                <asp:TextBox runat="server" ID="TBAccredCert" CssClass="txt long"  MaxLength="200" />
            </td>
        </tr>
        <tr>
            <td class="left">
                Код города
            </td>
            <td>
                <asp:TextBox runat="server" ID="TBCityCode" CssClass="txt" MaxLength="10" Width="40px" />
            </td>
            <td align="right">
                Телефон&nbsp;
                <asp:TextBox runat="server" ID="TBPhone" CssClass="txt" MaxLength="255" Width="109px" />
            </td>
        </tr>
         <tr>
            <td class="left">
                Факс
            </td>
            <td colspan="2">
                <asp:TextBox runat="server" ID="TBFax" CssClass="txt" MaxLength="100" />
            </td>
        </tr>
        <tr>
            <td class="left">
                E-mail
            </td>
            <td colspan="2">
                <asp:TextBox runat="server" ID="TBEMail" CssClass="txt" MaxLength="100" />
            </td>
            <asp:RegularExpressionValidator runat="server" ID="vldEmailFormat" ControlToValidate="TBEMail"
                EnableClientScript="false" Display="None" ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*\.{0,1}@([0-9a-zA-Z]*[-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$"
                ErrorMessage='Поле "E-mail" заполнено неверно' />
        </tr>
         <tr>
            <td class="left">
                Сайт
            </td>
            <td colspan="2">
                <asp:TextBox runat="server" ID="TBWebPage" CssClass="txt long" MaxLength="100" />
            </td>
        </tr>
        <tr>
            <td class="left">
                Пользователи
            </td>
            <td colspan="2">
                <asp:Repeater runat="server" ID="rptUsers">
                    <HeaderTemplate>
                        <table class="table-th" width="100%">
                            <tr class="th">
                                <td class="left-th" width="25%">
                                    <div>
                                        Логин</div>
                                </td>
                                <td width="45%">
                                    <div>
                                        ФИО</div>
                                </td>
                                <td class="right-th" width="30%">
                                    <div>
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
                                <%# Eval("FIO") %><br />
                                <nobr><a href="mailto:<%# Convert.ToString(Eval("email")) %>"><%# Convert.ToString(Eval("email")) %></a></nobr>
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
            </td>
        </tr>
        <tr>
            <td colspan="2" id="noteTd" class="box-submit" style="text-align: left; font-size: .8em;">
                (1) Сведения об объеме и структуре приема заполняются для ОУ в целом<br />
                (2) Заполняется только для федеральных ОУ<br />
                (3) Заполняется только для региональных ОУ
            </td>
            <td class="box-submit" colspan="3">
                <asp:Button runat="server" ID="btnUpdate" Text="Сохранить" CssClass="bt" OnClick="BtnUpdateClick" OnClientClick="btnUpdateClick()"/>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="HdnOrgStatus" runat="server" />
    </form>
    <div id="CalendarContainer" style="position: absolute; visibility: hidden; background-color: white;
        layer-background-color: white;">
    </div>
</asp:Content>
