<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.InstitutionPrepareApplicationViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Ввод заявления
</asp:Content>

<asp:Content ContentPlaceHolderID="PageTitle" runat="server">Ввод заявления</asp:Content>
<asp:Content ContentPlaceHolderID="PageSubtitle" runat="server">Шаг 1: Создание заявления</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<style type="text/css">
	div.tableStatement2 {
		border: 0px;
	}
	div.tableStatement2 a.btnDelete {
		background-position-y: 2px;
		height: 15px;
		margin-top: 0px;
		margin-bottom: 1px;
	}
</style>
	
	<div class="navigation">
		<% if (Model.CompetitiveGroupNamesByCampaign.Any()) 
         {%> 
         <a id="btnAppSaveTop" class="forvard" href="">Продолжить</a> 
         <% } %>
        <a id="btnAppCancelTop" class="cancel"	href="">Отмена</a>
	</div>
<div style="clear:both"></div>
	<%--<div>Если присутствует СНИЛС абитуриента, то рекомендуется ввести его, чтобы использовать ранее введённую информацию об абитуриенте в этом или последующем заявлениях</div>--%>
	<% if(!Model.CompetitiveGroupNamesByCampaign.Any()) { %>
        <% if (Model.ExistsGroupsWithoutEntranceTests) { %><div class="field-validation-error">Отсутствуют конкурсы с заданными вступительными испытаниями</div><%} %>
        <% else { %><div class="field-validation-error">Отсутствуют конкурсы с наличием мест</div><%} %>
    <% } else { %>
	<div id="content">
		<table class="data tableAdmin" cellpadding="3">
			<tbody>
				<tr class="separat">
					<td class="caption big"><%= Html.TableLabelFor(x => x.CampaignID, required:true) %></td>
					<td>
						<%= Html.DropDownListExFor(m => m.CampaignID, Model.Campaigns, new {onchange = "selectCampaign()"})%></td>
				</tr>
				<tr class="separat">
					<td class="caption big"><%= Html.TableLabelFor(x => x.DisplayCompetitiveGroupID, required: true)%></td>
					<td><input type="text" id="compGroupComplete" style="width: 600px" /><br/>
						<div id="divCompGroups" class="tableStatement2"></div>
					</td>
				</tr>
				<tr class="separat">
					<td class="caption big"><%= Html.TableLabelFor(x => x.DisplayDirectionID, required: true)%></td>
					<td><span><input type="text" id="dirComplete" style="width: 600px" /><br/></span>
						<div id="divDirections" class="tableStatement2"></div>
					</td>
				</tr>

                <tr class="separat">
                    <td class="caption big" rowspan="2">Условия приёма:</td>
                    <td>Для выбора условия приема проставьте его приоритет в заявлении абитуриента в поле "Приоритет". Приоритет может быть целым положительным числом, 1 - максимальный приоритет. Если прием проводится без приоритетов, то для выбора условий приема необходимо поставить 0</td>
                </tr>

				<tr class="separat">
<%{ /* %>
					<td class="caption big"><%= Html.TableLabelFor(x => x.EducationFormsDisp, required: true) %></td>
					<td>
						<div id="EducationForms">
			<%= Html.CheckBoxFor(m => m.EducationForms.BudgetO) %><%= Html.LabelFor(m => m.EducationForms.BudgetO) %><br />
			<%= Html.CheckBoxFor(m => m.EducationForms.BudgetOZ) %><%= Html.LabelFor(m => m.EducationForms.BudgetOZ) %><br />
			<%= Html.CheckBoxFor(m => m.EducationForms.BudgetZ) %><%= Html.LabelFor(m => m.EducationForms.BudgetZ) %><br />
			<%= Html.CheckBoxFor(m => m.EducationForms.PaidO) %><%= Html.LabelFor(m => m.EducationForms.PaidO)%><br />
			<%= Html.CheckBoxFor(m => m.EducationForms.PaidOZ)%><%= Html.LabelFor(m => m.EducationForms.PaidOZ)%><br />
			<%= Html.CheckBoxFor(m => m.EducationForms.PaidZ)%><%= Html.LabelFor(m => m.EducationForms.PaidZ) %><br />
			
			<%= Html.CheckBoxFor(m => m.EducationForms.TargetO, new Dictionary<string, object> { { "onclick", "setTargetOrgVisibility()" } } )%><span style="display: inline-block;width: 270px"><%= Html.LabelFor(m => m.EducationForms.TargetO) %></span>
			<%= Html.TableLabelFor(x => x.SelectedTargetOrganizationIDO)%> <%= Html.DropDownListExFor(x => x.SelectedTargetOrganizationIDO, new[]{new {ID = "", Name="     "}}, new {disabled = "disabled"}) %>
			<br />

			<%= Html.CheckBoxFor(m => m.EducationForms.TargetOZ, new Dictionary<string, object> { { "onclick", "setTargetOrgVisibility()" } } )%><span style="display: inline-block;width: 270px"><%= Html.LabelFor(m => m.EducationForms.TargetOZ) %></span>
			<%= Html.TableLabelFor(x => x.SelectedTargetOrganizationIDOZ)%> <%= Html.DropDownListExFor(x => x.SelectedTargetOrganizationIDOZ, new[]{new {ID = "", Name="     "}}, new {disabled = "disabled"}) %>
			<br />

			<%= Html.CheckBoxFor(m => m.EducationForms.TargetZ, new Dictionary<string, object> { { "onclick", "setTargetOrgVisibility()" } } )%><span style="display: inline-block;width: 270px"><%= Html.LabelFor(m => m.EducationForms.TargetZ) %></span>
			<%= Html.TableLabelFor(x => x.SelectedTargetOrganizationIDZ)%> <%= Html.DropDownListExFor(x => x.SelectedTargetOrganizationIDZ, new[]{new {ID = "", Name="     "}}, new {disabled = "disabled"}) %>
			<br />

			</div>
					</td>
<%*/ }%>
                    <td>
                        <table id="prioritiesTable" class="gvuzDataGrid">
                        </table>
                    </td>
				</tr>
				<tr>
					<td class="caption"><%= Html.TableLabelFor(m => m.ApplicationNumber, required: true)%></td>
					<td><%= Html.TextBoxExFor(m => m.ApplicationNumber)%></td>
          <td class="caption"><%= Html.TableLabelFor(m => m.Priority) %></td>
          <td><%= Html.TextBoxExFor(m => m.Priority) %></td>
				</tr>
				<tr>
					<td class="caption"><%= Html.TableLabelFor(m => m.RegistrationDate, required: true)%></td>	
					<td><%= Html.DatePickerFor(m => m.RegistrationDate)%></td>
				</tr>
			<tr>
				<td class="caption"><%= Html.TableLabelFor(m => m.IdentityDocumentTypeID, new {style="padding-top:0px;padding-bottom:10px;"},required: true)%></td>
				<td><%= Html.DropDownListExFor(m => m.IdentityDocumentTypeID, Model.IdentityDocumentList, new {})%></td>
			</tr>

				<tr>
					<td class="caption"><%= Html.TableLabelFor(m => m.DocumentNumber, required: true)%></td>
					<td colspan="3"><%= Html.TextBoxExFor(m => m.DocumentSeries, new { @class="passSeries" })%><%= Html.TextBoxExFor(m => m.DocumentNumber, new { @class = "passNumber" })%></td>
				</tr>
			</tbody>
		</table>
	</div>
    <%} %>
	<div class="navigation">
		<% if (Model.CompetitiveGroupNamesByCampaign.Any()) 
         {%> 
         <a id="btnAppSave" class="forvard" href="">Продолжить</a> 
         <% } %>
		<a id="btnAppCancel" class="cancel" href="" >Отмена</a>
	</div>
	<div style="clear:both"></div>
	<div id="addGroupDialog"></div>

    <div style="display:none">
<div id="entrantInfo">В системе найден абитуриент, соответствующий введенным параметрам</div></div>

<script language="javascript" type="text/javascript">

	function sendApplication(model)
	{
		doPostAjax('<%= Url.Generate<InstitutionApplicationDraftController>(x => x.CreateApplication(null)) %>', JSON.stringify(model), 
				function (data)
				{	
					if(data.Extra == <%= InstitutionApplicationDraftController.ShowCompetitiveGroupAddDialog %>)
						if(confirm('Указанный конкурс отсутствует. Перейти на добавление конкурса с указанным именем?'))
							{
								doPostAjax('<%= Url.Generate<AdmissionController>(c => c.CompetitiveGroupAddWithName(null)) %>', 
									'groupName=' + encodeURIComponent(jQuery('#CompetitionGroup').val()), function (data2)
								{
										jQuery('#addGroupDialog').html(data2);
										jQuery('#addGroupDialog').dialog({
											modal: true,
											width: 800,
											title: 'Добавление конкурса',
											buttons:
													{
														"Сохранить": function () { cg_save(function () { jQuery(this).dialog('close'); }.bind(this) ); },
														"Отмена": function () { jQuery(this).dialog('close'); }
													}
										}).dialog('open');
									}, "application/x-www-form-urlencoded", "html")
							}
					if(data.Extra == <%= InstitutionApplicationDraftController.ShowMessageExistingEntrant %>)
					{
							jQuery('<div>В системе найден абитуриент, соответствующий введенным параметрам</div>').dialog(
							{
								resizable: false,
								modal: true,
								buttons:
								{
									"Ок": function ()
									{
										jQuery(this).dialog("close");
										model.CheckForExistingBeforeCreate = false
									//	sendApplication(model)
                                        window.location = '<%= Url.Generate<ApplicationController>(x => x.ApplicationPersonalDataByApp(null, null)) %>?applicationID=' + data.Data
									}
								}
							});
                            return;
						
					}
					if (!addValidationErrorsFromServerResponse(data, false))
					{
						window.location = '<%= Url.Generate<ApplicationController>(x => x.ApplicationPersonalDataByApp(null, null)) %>?applicationID=' + data.Data
                        return;
					}

                    if (data.Data.length > 1)
                    {
                        // Обработка ситуации с незаданными приоритетами
                        for (var i = 0; i < data.Data.length; i++)
                        {
                            if (data.Data[i].ControlID == "Fake") continue;

                            var cells = jQuery('td[dataVal=' + data.Data[i].ControlID + ']');

                            cells.each(function()
                            {
                                jQuery(jQuery(jQuery(this.parentElement).children()[4]).children()[0]).removeClass('input-validation-error-fixed').addClass('input-validation-error');
                            });
                        }
                    }
                    else
                    {
                        //Обработка ситуаций с нулевыми или дублирующими приоритетами

                        if (data.Data[0].ControlID == "zeroMessage")
                        {
                            jQuery(".priorityInput").each(function()
                            {
                                if (this.value == '0')
                                    jQuery(this).removeClass('input-validation-error-fixed').addClass('input-validation-error');
                            });
                        }

						jQuery('<div>' + data.Data[0].ErrorMessage + '</div>').dialog(
						{
							resizable: false,
							modal: true,
							buttons:
							{
								"Продолжить": function ()
								{
									jQuery(this).dialog("close");
									model.CheckUniqueBeforeCreate = !(data.Data[0].ControlID == "NonUniquePriorities");
									model.CheckZerozBeforeCreate = !(data.Data[0].ControlID == "zeroMessage");
									sendApplication(model)
								},
                                "Отмена": function()
                                {
                                    jQuery(this).dialog('close');
                                }
							}
						});

						return
                    }
				})
	}

	var groupByCampaign = []
	var edForms = JSON.parse('<%= Html.Serialize(Model.CompetitiveGroupEducationForms) %>')
	var tmpID = 0

	function selectCampaign() {
		jQuery("#compGroupComplete").val('')
		jQuery('#dirComplete').html('')
		jQuery('#divCompGroups').html('')
		getCompetitiveGroupItems()
	}
	
	function competitiveGroupComplete(ui, response) {
	    if(!jQuery('#CampaignID').val()) return;
		var res = []
		var x = ui.term.toUpperCase()
		var selectedGroups = jQuery('#divCompGroups span[cgID]').attrToArr('cgID');
		var allowedCourse = ''
		if(selectedGroups.length > 0) {
			allowedCourse = jQuery('#divCompGroups span[cgID]').attr('course')
		}
		jQuery.each(groupByCampaign[jQuery('#CampaignID').val()], function (i, e) {
			var name = e.Name + ', ' + e.Course + ' курс';
			if(name.toUpperCase().indexOf(x) >= 0 && (allowedCourse == e.Course || allowedCourse == '') && selectedGroups.indexOf(e.ID) < 0)
				res.push(escapeHtml(e.Name) + ', ' + e.Course + ' курс')
		})
		response(res)
	}
	
	function competitiveGroupCompleteSelected() {
		var val = jQuery("#compGroupComplete").val().toUpperCase();
		jQuery.each(groupByCampaign[jQuery('#CampaignID').val()], function (i, e) {
			var curID = tmpID++;
			var name = e.Name + ', ' + e.Course + ' курс';
			if(name.toUpperCase() == val) {
				jQuery("#divCompGroups").append('<span id="cbtmpid' + curID + '" cgID="' + e.ID + '" course="' + e.Course + '">' + escapeHtml(e.Name) + ', ' + e.Course + ' курс' 
					+ ' <a class="btnDelete" href="#" onclick="competitiveGroupCompleteRemove('+e.ID + ');return false;">&nbsp;</a><br/></span>')
			}
		})
		jQuery("#compGroupComplete").val('')
		getCompetitiveGroupItems()
	}
	
	function competitiveGroupCompleteRemove(groupID) {
		var $el = jQuery('#divCompGroups span[cgID="' + groupID + '"]');
		$el.remove().detach();
		
		getCompetitiveGroupItems()
	}
	
	function directionComplete(ui, response) {
		var res = []
		var x = ui.term.toUpperCase()
		var selectedDirs = jQuery('#divDirections span[itemID]').attrToArr('itemID');
		
		jQuery.each(availableDirections, function (i, e) {
			var name = e.Code + ' - ' + e.Name;
			if(name.toUpperCase().indexOf(x) >= 0 && selectedDirs.indexOf(e.ID) < 0)
				res.push(escapeHtml(e.Code) + ' - ' + escapeHtml(e.Name))
		})
		response(res)
	}
	
	function directionCompleteGetRow(item) {
		var curID = tmpID++;
		return '<span id="cbtmpid' + curID + '" itemID="' + item.ID + '">' + escapeHtml(item.Code) + ' - ' + escapeHtml(item.Name)
					+ ' <a class="btnDelete" href="#" onclick="directionCompleteRemove(\'' + item.ID + '\');return false;">&nbsp;</a><br/></span>'
	}

	function directionCompleteSelected() {
		var val = jQuery("#dirComplete").val().toUpperCase();
		jQuery.each(availableDirections, function (i, e) {
			var name = e.Code + ' - ' + e.Name;
			if(name.toUpperCase() == val) {
				jQuery("#divDirections").append(directionCompleteGetRow(e))
			}
		})
		jQuery("#dirComplete").val('')
		getEducationForms();
	}
	
	function directionCompleteRemove(itemID) { 
		jQuery('#divDirections span[itemID="' + itemID + '"]').remove().detach()
		getEducationForms();
	}

	var availableDirections = []

	function getCompetitiveGroupItems() {
		var selectedGroups = jQuery('#divCompGroups span[cgID]').attrToArr('cgID');
		var selectedDirs = jQuery('#divDirections span[itemID]').attrToArr('itemID');
		jQuery('#dirComplete').val('');

		doPostAjax('<%= Url.Generate<InstitutionApplicationDraftController>(x => x.GetDirectionsForCompetitiveGroups(null)) %>', JSON.stringify(selectedGroups),
			function (data) {
				if(data.IsError) {
					alert(data.Message);
					return;
				}
				availableDirections = data.Data;
				
				jQuery.each(data.Data, function (i, e) {
					if(selectedDirs.indexOf(e.ID) < 0)
						jQuery('#divDirections span[itemID="' + e.ID + '"]').remove().detach()
				})
				if(data.Data.length == 0) {
					jQuery('#divDirections').html('<span class="emptyRow">Отсутствуют доступные направления подготовки</span>')
					jQuery('#dirComplete').parent().hide();
				}
				else {
					jQuery('#dirComplete').parent().show();
					jQuery('#divDirections span.emptyRow').remove().detach();
				}
				
				getEducationForms()
		});
	}

	function getEducationForms() {
		var selectedGroups = jQuery('#divCompGroups span[cgID]').attrToArr('cgID')
		var selectedDirs = jQuery('#divDirections span[itemID]').attrToArr('itemID')
		
		var model = {
			competitiveGroupIDs: selectedGroups,
			directionKeys: selectedDirs
		};

        doPostAjax('<%= Url.Generate<InstitutionApplicationDraftController>(x => x.CreatePrioritiesData(null, null)) %>', JSON.stringify(model),
            function (data) {
                var tab = jQuery("#prioritiesTable");
                if (tab == null) return;

                tab.empty();
                if (data.Data == null) return;

                tab.append
                (
                    jQuery('<thead>').html('<tr><th>Конкурс</th><th>Направление подготовки</th><th>Форма обучения</th><th>Источник финансирования</th><th>Приоритет</th></tr>')
                );

                for (var i = 0; i < data.Data.ApplicationPriorities.length; i++)
                {
                    var row = jQuery("<tr>");
                    var html = '<td dataVal="' + data.Data.ApplicationPriorities[i].CompetitiveGroupId + '">' + data.Data.ApplicationPriorities[i].CompetitiveGroupName + '</td>' +
                               '<td dataVal="' + data.Data.ApplicationPriorities[i].CompetitiveGroupItemId + '">' + data.Data.ApplicationPriorities[i].CompetitiveGroupItemName + '</td>' +
                               '<td dataVal="' + data.Data.ApplicationPriorities[i].EducationFormId + '">' + data.Data.ApplicationPriorities[i].EducationFormName + '</td>' +
                               '<td dataVal="' + data.Data.ApplicationPriorities[i].EducationSourceId +'">' + data.Data.ApplicationPriorities[i].EducationSourceName + '</td>' +
                               '<td><input type="text" class="priorityInput" onchange=priorityChanged(this,' + (data.Data.ApplicationPriorities[i].TargetOrganizations != null ? '1' : '0') + ')>';

                    if (data.Data.ApplicationPriorities[i].TargetOrganizations != null)
                    {
                        html = html + '<br/><select id="targetInput" disabled="disabled">';
                        for (var j = 0; j < data.Data.ApplicationPriorities[i].TargetOrganizations.length; j++)
                        {
                            html += '<option value="' + data.Data.ApplicationPriorities[i].TargetOrganizations[j].ID + '">' + data.Data.ApplicationPriorities[i].TargetOrganizations[j].Name + '</option>';
                        }
                        html+= '</select>';
                    }
                    html += '</td>';

                    row.html(html);

                    tab.append(row);
                }
            });
	}

	function setTargetOrgVisibility() {
		var fSet = function(suff) {
			jQuery("#SelectedTargetOrganizationID" + suff).toggleEnabled(jQuery('#EducationForms_Target' + suff).is(':checked'))
		};
		fSet('O');fSet('OZ');fSet('Z');
	}

	jQuery(document).ready(function ()
	{
		jQuery(".datePicker:not(:disabled)").datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-10:+0' });

		//<% foreach (var pair in Model.CompetitiveGroupNamesByCampaign)
		   {%>
		
		   	groupByCampaign[<%= pair.Key %>] = JSON.parse('<%= Html.Serialize(pair.Value) %>')//<%} %>

		selectCampaign()
		
		autocompleteDropdown(jQuery('#compGroupComplete'), {minLength:1, source: competitiveGroupComplete,
				select: function() {setTimeout(competitiveGroupCompleteSelected, 0)}})
		autocompleteDropdown(jQuery('#dirComplete'), {minLength:1, source: directionComplete,
				select: function() {setTimeout(directionCompleteSelected, 0)}})

		/*jQuery("input#SNILS").mask("999-999-999 99");*/
        $('#IdentityDocumentTypeID').change(function() {
             switch ($(this).val()) {
             case '3':
                 $("#DocumentNumber").attr('maxlength','50');
                 $("#DocumentSeries").attr('maxlength','20');
                 break;
             case '9':
                 $("#DocumentNumber").attr('maxlength','50');
                 $("#DocumentSeries").attr('maxlength','20');
                 break;
             case '1':
                 $("#DocumentNumber").attr('maxlength','6');
                 $("#DocumentNumber").val($("#DocumentNumber").val().length > 6 ? $("#DocumentNumber").val().substr(0, 6) : $("#DocumentNumber").val());
                 $("#DocumentSeries").attr('maxlength','4');
                 $("#DocumentSeries").val($("#DocumentSeries").val().length > 4 ? $("#DocumentSeries").val().substr(0, 4) : $("#DocumentSeries").val());
                 break;
             default:
                 $("#DocumentNumber").attr('maxlength','10');
                 $("#DocumentNumber").val($("#DocumentNumber").val().length > 10 ? $("#DocumentNumber").val().substr(0, 10) : $("#DocumentNumber").val());
                 $("#DocumentSeries").attr('maxlength','6');
                 $("#DocumentSeries").val($("#DocumentSeries").val().length > 6 ? $("#DocumentSeries").val().substr(0, 6) : $("#DocumentSeries").val());
                 break;
             }
         });
	    $('#IdentityDocumentTypeID').change();
		jQuery('#btnAppCancelTop, #btnAppCancel').click(function () { window.location = '<%= Url.Generate<InstitutionApplicationController>(x => x.ApplicationList()) %>'; return false; })
		
        jQuery('#btnAppSaveTop, #btnAppSave').click(function ()
		{
            return SaveData();
		})


         function SaveData()
         {
            var isError = false
			    jQuery('#EducationForms.input-validation-error').addClass('input-validation-error-fixed').removeClass('input-validation-error');
			    if (revalidatePage(jQuery('.data')))
				    isError = true
			    if (jQuery('#DocumentNumber').hasClass('input-validation-error') || jQuery('#DocumentSeries').hasClass('input-validation-error'))
			    {
				    if(!jQuery('#DocumentNumber').hasClass('input-validation-error'))
					    jQuery('#DocumentNumber').addClass('input-validation-error')
				    if(!jQuery('#DocumentSeries').hasClass('input-validation-error'))
					    jQuery('#DocumentSeries').addClass('input-validation-error')
				    jQuery('#DocumentNumber,#DocumentSeries').removeClass('input-validation-error-fixed')
			    }

			    if(isError) return false

                var Priorities = {
                    ApplicationId : -1,
                    ApplicationPriorities: []
                };
                var priorityRows = jQuery("#prioritiesTable tr");

                for (var i = 1; i < priorityRows.length; i++)
                {
                    var priorityRow = priorityRows[i];
                    var Priority = {
                        CompetitiveGroupId: jQuery(jQuery(priorityRow).children()[0]).attr('dataVal'),
                        CompetitiveGroupItemId: jQuery(jQuery(priorityRow).children()[1]).attr('dataVal'),
                        EducationFormId: jQuery(jQuery(priorityRow).children()[2]).attr('dataVal'),
                        EducationSourceId: jQuery(jQuery(priorityRow).children()[3]).attr('dataVal'),
                        Priority: jQuery(jQuery(priorityRow).children()[4]).children('.priorityInput').val(),
                        CompetitiveGroupTargetId: jQuery(jQuery(priorityRow).children()[4]).children('#targetInput').val()
                    }

                    Priorities.ApplicationPriorities.push(Priority);
                }

			    var model = {
				    DocumentSeries: jQuery('#DocumentSeries').val(),
				    DocumentNumber: jQuery('#DocumentNumber').val(),
				    IdentityDocumentTypeID: jQuery('#IdentityDocumentTypeID').val(),
				    SelectedCompetitiveGroupIDs: jQuery('#divCompGroups span[cgID]').attrToArr('cgID'),
				    SelectedDirectionIDs: jQuery('#divDirections span[itemID]').attrToArr('itemID'),
				    RegistrationDate: jQuery('#RegistrationDate').val(),
				    ApplicationNumber: jQuery('#ApplicationNumber').val(),
                    Priority: jQuery('#Priority').val(),
				    CampaignID: jQuery('#CampaignID').val(),

    /*				EducationForms: {
					    BudgetO: jQuery('#EducationForms_BudgetO').attr("checked"),
					    BudgetOZ: jQuery('#EducationForms_BudgetOZ').attr("checked"),
					    BudgetZ: jQuery('#EducationForms_BudgetZ').attr("checked"),
					    PaidO: jQuery('#EducationForms_PaidO').attr("checked"),
					    PaidOZ: jQuery('#EducationForms_PaidOZ').attr("checked"),
					    PaidZ: jQuery('#EducationForms_PaidZ').attr("checked"),
					    TargetO: jQuery('#EducationForms_TargetO').attr("checked"),
					    TargetOZ: jQuery('#EducationForms_TargetOZ').attr("checked"),
					    TargetZ: jQuery('#EducationForms_TargetZ').attr("checked")
					    },*/
				    SelectedTargetOrganizationIDO: jQuery("#SelectedTargetOrganizationIDO").val(),
				    SelectedTargetOrganizationIDOZ: jQuery("#SelectedTargetOrganizationIDOZ").val(),
				    SelectedTargetOrganizationIDZ: jQuery("#SelectedTargetOrganizationIDZ").val(),
				    CheckForExistingBeforeCreate: true,
                    CheckUniqueBeforeCreate: true,
                    CheckZerozBeforeCreate: true,
                    Priorities : Priorities
			    }
			    if(!model.SelectedTargetOrganizationIDO) model.SelectedTargetOrganizationIDO = 0;
			    if(!model.SelectedTargetOrganizationIDOZ) model.SelectedTargetOrganizationIDOZ = 0;
			    if(!model.SelectedTargetOrganizationIDZ) model.SelectedTargetOrganizationIDZ = 0;
			    sendApplication(model)
			    return false
        }
	})




   



    function priorityChanged(el, flag)
    {
        if (flag == '0') return false;

        var selector = jQuery(el).parent().children('select:first');

        if (jQuery(el).val() == '')
            selector.setDisabled();
        else selector.setEnabled();
    }
</script>

</asp:Content>
