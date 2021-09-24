<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.Portlets.Applications.ApplicationSendingViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
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

<table class="personData tableApp2" style="width:90%">
	<tr>
		<td class="caption" width="200px"><%= Html.StrongLabelFor(m=>m.FIO) %></td>
		<td><%= Html.CommonInputReadOnly(Model.FIO)%></td>		
	</tr>
	<tr>
		<td class="caption"><%= Html.TableLabelFor(m => m.DOB)%></td>
		<td><%= Html.CommonInputReadOnly(Model.DOB)%></td>		
	</tr>
	<tr>
		<td class="caption"><%= Html.TableLabelFor(m => m.IdentityDocument)%></td>
		<td><%= Html.CommonInputReadOnly(Model.IdentityDocument)%></td>		
	</tr>
	<tr>
		<td class="caption"><%= Html.TableLabelFor(m => m.Gender)%></td>
		<td><%= Html.CommonInputReadOnly(Model.Gender)%></td>		
	</tr>
	<tr>
		<td class="caption"><%= Html.TableLabelFor(m => m.Citizen)%></td>
		<td><%= Html.CommonInputReadOnly(Model.Citizen) %></td>		
	</tr>
	<tr>
		<td class="caption"><%= Html.TableLabelFor(m => m.POB)%></td>
		<td><%= Html.CommonInputReadOnly(Model.POB) %></td>		
	</tr>
	<tr>
		<td class="caption" style="width:200px"><%= Html.TableLabelFor(m=>m.CustomInformation) %></td>
		<td><%= Html.CommonTextAreaReadOnly(Model.CustomInformation)%><br />&nbsp;</td>
	</tr>
</table><br />
Прошу допустить меня к участию в конкурсе в следующих образовательных учреждениях <%= ViewData["OULevelGenetive"] %> профессионального образования на следующие направления подготовки:<br />
<table class="institutionData tableAdmin" style="width:90%">
	<tr>
		<td class="caption" style="width:200px"><%= Html.TableLabelFor(m => m.Institution, ViewData["Institution"].ToString())%></td>
		<td><%= Html.CommonInputReadOnly(Model.Institution) %></td>		
	</tr>
	<%--<tr>
		<td class="caption"><%= Html.TableLabelFor(m => m.Direction)%></td>
		<td><%= Html.CommonInputReadOnly(Model.Direction) %></td>		
	</tr>
	<tr>
		<td class="caption"><%= Html.TableLabelFor(m => m.Course)%></td>
		<td><%= Html.CommonInputReadOnly(Model.Course) %></td>		
	</tr>
	<tr class="separat">
		<td class="caption"><%= Html.TableLabelFor(m => m.EducationLevel)%></td>
		<td><%= Html.CommonInputReadOnly(Model.EducationLevel) %></td>
	</tr>--%>
	<tr class="separat">
		<td class="caption">
			<%= Html.TableLabelFor(x => x.CampaignID)%>
		</td>
		<td nowrap="nowrap">
			<%= Html.DropDownListExFor(m => m.CampaignID, Model.Campaigns, new { @class = "searchField", @style = "width:100%", onchange = "selectCampaign()" })%> 
		</td>
	</tr>
    <tr class="separat">
        <td class="caption">Выбранные условия приёма</td>
        <td> <button id="changeButton" value="Изменить" onclick="changePriorities()">Изменить</button> </td>
    </tr>
    <tr class="separat">
        <td colspan="2">
            <div id="showDiv">
                <table class="gvuzDataGrid" id="showTable">
                    <% foreach (var priority in Model.Priorities.ApplicationPriorities){ 
                        if (priority.Priority.HasValue) {%>
                    <tr>
                        <td><%=priority.ToString() %></td>
                    </tr>
                    <% } }%>
                </table>
            </div>
            <div id="editDiv">
                <table class="tableApp2">
                    <tr class="separat">
		                <td class="caption">
			                <%= Html.TableLabelFor(x => x.DisplayCompetitiveGroupID)%>
		                </td>
		                <td nowrap="nowrap"><input type="text" id="compGroupComplete" style="width: 100%" /><br/>
			                <div id="divCompGroups" class="tableStatement2"></div>
		                </td>
                	</tr>
	                <tr class="separat">
		                <%--<td class="caption"><%= Html.TableLabelFor(x => x.DisplayDirectionID)%></td>--%>
		                <td><span><input type="text" id="dirComplete" style="width: 600px" /><br/></span>
			                <div id="divDirections" class="tableStatement2"></div>
		                </td>
	                </tr>                    
                    <tr>
                        <td colspan="2">
                            <table id="prioritiesTable" class="gvuzDataGrid">
                                <thead>
                                    <tr>
                                        <th>Конкурс</th>
                                        <th>Направление подготовки</th>
                                        <th>Форма обучения</th>
                                        <th>Источник финансирования</th>
                                        <th>Приоритет</th>
                                    </tr>
                                </thead>
                            </table>
                            <button id="savePriorities" value="Сохранить" onclick="savePriorities(true, true)">Сохранить</button>
                            <button id="cancelPriorities" value="Отмена" onclick="cancelPriorities()">Отмена</button>
                        </td>
                    </tr>
                </table>
            </div>
        </td>
    </tr>
<%{/* %>	<tr class="separat">
		<td class="caption">
			<%= Html.TableLabelFor(x => x.DisplayCompetitiveGroupID)%>
		</td>
		<td nowrap="nowrap"><input type="text" id="compGroupComplete" style="width: 100%" /><br/>
			<div id="divCompGroups" class="tableStatement2"></div>
		</td>
	</tr>
	<tr class="separat">
		<td class="caption"><%= Html.TableLabelFor(x => x.DisplayDirectionID)%></td>
		<td><span><input type="text" id="dirComplete" style="width: 600px" /><br/></span>
			<div id="divDirections" class="tableStatement2"></div>
		</td>
	</tr>

	<tr class="separat">
		<td class="caption"><%= Html.TableLabelFor(m => m.EducationFormsAvailable, required: true)%></td>
		<td><div id="EducationFormsSelected_NotSelected">
			<%= Html.CheckBoxFor(m => m.EducationFormsSelected.BudgetO) %><%= Html.LabelFor(m => m.EducationFormsSelected.BudgetO) %><br />
			<%= Html.CheckBoxFor(m => m.EducationFormsSelected.BudgetOZ) %><%= Html.LabelFor(m => m.EducationFormsSelected.BudgetOZ) %><br />
			<%= Html.CheckBoxFor(m => m.EducationFormsSelected.BudgetZ) %><%= Html.LabelFor(m => m.EducationFormsSelected.BudgetZ) %><br />
			<%= Html.CheckBoxFor(m => m.EducationFormsSelected.PaidO)%><%= Html.LabelFor(m => m.EducationFormsSelected.PaidO) %><br />
			<%= Html.CheckBoxFor(m => m.EducationFormsSelected.PaidOZ)%><%= Html.LabelFor(m => m.EducationFormsSelected.PaidOZ) %><br />
			<%= Html.CheckBoxFor(m => m.EducationFormsSelected.PaidZ) %><%= Html.LabelFor(m => m.EducationFormsSelected.PaidZ) %><br />
			
			<%= Html.CheckBoxFor(m => m.EducationFormsSelected.TargetO, new Dictionary<string, object> { { "onclick", "targetOrgChanged()" } } )%><span style="display: inline-block;width: 270px"><%= Html.LabelFor(m => m.EducationFormsSelected.TargetO) %></span>
			<%= Html.TableLabelFor(x => x.SelectedTargetOrganizationIDO)%> <%= Html.DropDownListExFor(x => x.SelectedTargetOrganizationIDO, Model.EducationFormsAvailable.TargetOrganizationsO, new {disabled = "disabled"}) %>
			<br />

			<%= Html.CheckBoxFor(m => m.EducationFormsSelected.TargetOZ, new Dictionary<string, object> { { "onclick", "targetOrgChanged()" } } )%><span style="display: inline-block;width: 270px"><%= Html.LabelFor(m => m.EducationFormsSelected.TargetOZ) %></span>
			<%= Html.TableLabelFor(x => x.SelectedTargetOrganizationIDOZ)%> <%= Html.DropDownListExFor(x => x.SelectedTargetOrganizationIDOZ, Model.EducationFormsAvailable.TargetOrganizationsOZ, new {disabled = "disabled"}) %>
			<br />

			<%= Html.CheckBoxFor(m => m.EducationFormsSelected.TargetZ, new Dictionary<string, object> { { "onclick", "targetOrgChanged()" } } )%><span style="display: inline-block;width: 270px"><%= Html.LabelFor(m => m.EducationFormsSelected.TargetZ) %></span>
			<%= Html.TableLabelFor(x => x.SelectedTargetOrganizationIDZ)%> <%= Html.DropDownListExFor(x => x.SelectedTargetOrganizationIDZ, Model.EducationFormsAvailable.TargetOrganizationsZ, new {disabled = "disabled"}) %>
			<br />
			
			</div>
		</td>
	</tr>
<% */} %>

	<% if (!Url.IsInsidePortlet()) { %>
	<tr>
		<td class="caption"><%= Html.TableLabelFor(m => m.RegistrationDate, required: true)%></td>	
		<td nowrap="nowrap" id="dateEditor"><%= Html.DatePickerFor(m => m.RegistrationDate)%></td>
	</tr>
    <tr>
        <td class="caption"><%= Html.TableLabelFor(m => m.Priority) %></td>
        <td><%= Html.TextBoxExFor(m => m.Priority) %></td>
    </tr>
	<tr>
		<td class="caption"><%= Html.TableLabelFor(m => m.Uid) %></td>
		<td><%= Html.TextBoxExFor(m => m.Uid)%></td>
	</tr>
	<tr>
		<td class="caption"><%= Html.LabelFor(m => m.NeedHostel)%></td>
		<td><%= Html.CheckBoxFor(m => m.NeedHostel)%></td>
	</tr>
	<tr>
		<td></td>
		<td></td>
	</tr>
	<% } %>

</table><br />
<br />
<%-- сейчас не выбирается
<table class="checkData tableApp2" style="width:90%;">

<% if (Model.IsVUZ) {%>
<tr><td style="padding-left:30px">
<%= Html.CheckBoxFor(m => m.ApproveInstitutionCount)%>&nbsp;<%= Html.LabelFor(m => m.ApproveInstitutionCount)%>&nbsp;<br />
</td></tr>
<%}%>
<tr><td style="padding-left:30px">
<%= Html.CheckBoxFor(m => m.FirstHigherEducation)%>&nbsp;<%= Html.LabelFor(m => m.FirstHigherEducation, ViewData["FirstHigherEducationLabel"].ToString())%>
</td></tr>
<tr><td style="padding-left:30px">
<%= Html.CheckBoxFor(m => m.ApprovePersonalData)%>&nbsp;<%= Html.LabelFor(m => m.ApprovePersonalData)%>
</td></tr>
<tr><td style="padding-left:30px">
<%= Html.CheckBoxFor(m => m.FamiliarWithLicenseAndRules)%>&nbsp;<%= Html.LabelFor(m => m.FamiliarWithLicenseAndRules, ViewData["FamiliarWithLicenseAndRules"].ToString())%>
</td></tr>
<tr><td style="padding-left:30px">
<%= Html.CheckBoxFor(m => m.FamiliarWithAdmissionType)%>&nbsp;<%= Html.LabelFor(m => m.FamiliarWithAdmissionType)%>
</td></tr>
<tr><td style="padding-left:30px">
<%= Html.LabelFor(m=>m.EducationDocumentDate) %>:&nbsp;<%: Model.EducationDocumentDate %>
</td></tr>
<tr><td style="padding-left:30px">
<%= Html.CheckBoxFor(m => m.FamiliarWithOriginalDocumentDeliveryDate)%>&nbsp;<%= Html.LabelFor(m => m.FamiliarWithOriginalDocumentDeliveryDate)%><br />&nbsp;
</td></tr>
</table>
--%>

<%-- скрытый диалог выбора  --%>
<div style="display:none">
<div id="divDecisionDialog" >
	<table>
		<tbody>
			<tr>
				<td class="caption">При проверки заявления возникли ошибки, если вы хотите включить заявления в принятые, введите причину решения.<br/><br/> <b>Ошибка:</b></td>
			</tr>
			<tr>
				<td id="lbCheckDescription"> </td>
			</tr>
			<tr>
				<td><textarea style="width: 550px" id="tbCheckDecision" cols="80" rows="6"></textarea></td>
			</tr>
		</tbody>
	</table>
</div>
</div>
<script type="text/javascript">

	jQuery(".datePicker:not(:disabled)").datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-10:+0' });

    //<%-- Ставим энейбл/дизейбл выбора организации ЦП в завимости от чекбоксика  --%>
	function targetOrgChanged() {
		var fSet = function(suff) {
			jQuery("#SelectedTargetOrganizationID" + suff).toggleEnabled(jQuery('#EducationFormsSelected_Target' + suff).is(':checked'))
		};
		fSet('O');fSet('OZ');fSet('Z');
	}

    // <%-- данные с сервера --%>
	var groupByCampaign = []
	var tmpID = 0
	var initialCompGroups = JSON.parse('<%= Html.Serialize(Model.SelectedCompetitiveGroupIDs) %>')
	var initialDirections = JSON.parse('<%= Html.Serialize(Model.SelectedDirectionIDs) %>')

    // <%-- выбрали ПК. Грузим КГ --%>
	function selectCampaign() {
		var res = '';
		jQuery.each(groupByCampaign[jQuery('#CampaignID').val()], function (i, e) {
			var curID = tmpID++;
			if(initialCompGroups.indexOf(e.ID) >= 0)
			res += '<span id="cbtmpid' + curID + '" cgID="' + e.ID + '" course="' + e.Course + '">' + escapeHtml(e.Name) + ', ' + e.Course + ' курс' 
					+ ' <a class="btnDelete" href="#" onclick="competitiveGroupCompleteRemove('+e.ID + ');return false;">&nbsp;</a><br/></span>'
		})
		jQuery('#divCompGroups').html(res)
		jQuery("#compGroupComplete").val('')
		jQuery('#dirComplete').html('')
		getCompetitiveGroupItems()
	}
	
    // <%-- автокомплит КГ (поиск подходящих вариантов) --%>
	function competitiveGroupComplete(ui, response) {
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
	
    // <%-- выбрали автокомплитом КГ. добавляем КГ. Грузим направления --%>
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
	
    // <%-- удалили КГ. Грузим направления --%>
	function competitiveGroupCompleteRemove(groupID) {
		jQuery('#divCompGroups span[cgID="' + groupID + '"]').remove().detach()
		getCompetitiveGroupItems()
	}

    // <%-- автокомплит направлений. Находим подходящие --%>
	function directionComplete(ui, response) {
		var res = []
		var x = ui.term.toUpperCase()
		var selectedDirs = jQuery('#divDirections span[itemID]').attrToArr('itemID');
		
		jQuery.each(availableDirections, function (i, e) {
			var name = e.Code + ' - ' + e.Name;
			if(name.toUpperCase().indexOf(x) >= 0 && selectedDirs.indexOf(e.ID) < 0)
            {
				res.push(escapeHtml(e.Code) + ' - ' + escapeHtml(e.Name))
            }
		})
		response(res)
	}
	
    // <%-- генерации строчки с направлением --%>
	function directionCompleteGetRow(item) {
		var curID = tmpID++;
		return '<span id="cbtmpid' + curID + '" itemID="' + item.ID + '">' + escapeHtml(item.Code) + ' - ' + escapeHtml(item.Name)
					+ ' <a class="btnDelete" href="#" onclick="directionCompleteRemove(\'' + item.ID + '\');return false;">&nbsp;</a><br/></span>'
	}

    // <%-- выбрали направление в автокомплите. Добавляем в список. грузим формы --%>
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
	
    // <%-- убрали направление из КГ. грузим формы --%>
	function directionCompleteRemove(itemID) { 
		jQuery('#divDirections span[itemID="' + itemID + '"]').remove().detach()
		getEducationForms();
	}

	var availableDirections = []

    // <%-- берём допустимые направления --%>
	function getCompetitiveGroupItems() {
		
		var selectedGroups = jQuery('#divCompGroups span[cgID]').attrToArr('cgID');
		var selectedDirs = jQuery('#divDirections span[itemID]').attrToArr('itemID');
		jQuery('#dirComplete').val('');

	    // <%-- идём на сервер. Он всё знает. А мы ему скажем, что пользователь уже выбрал --%>
		doPostAjax('<%= Url.Generate<InstitutionApplicationDraftController>(x => x.GetDirectionsForCompetitiveGroups(null)) %>', JSON.stringify(selectedGroups),
			function (data) {
				if(data.IsError) {
					alert(data.Message);
					return;
				}
			    // <%-- сохраняем --%>
				availableDirections = data.Data;
				
			    // <%-- добавляем/удаляем строчки. Если есть начальные направления, их добавляем --%>

                jQuery.each(jQuery('#divDirections span'), function(i,e){
                    jQuery(e).remove().detach();
                });

				jQuery.each(data.Data, function (i, e) {
					if(selectedDirs.indexOf(e.ID) >= 0)
					    jQuery('#divDirections').append(directionCompleteGetRow(e));
                    else if(initialDirections.indexOf(e.ID) >= 0)
						jQuery('#divDirections').append(directionCompleteGetRow(e))
				})


				if(data.Data.length == 0) {
					jQuery('#divDirections').html('<span class="emptyRow">Отсутствуют доступные направления подготовки</span>')
					jQuery('#dirComplete').parent().hide();
				}
				else {
					jQuery('#dirComplete').parent().show();
					jQuery('#divDirections span.emptyRow').remove().detach();
				}
				
			    // <%-- первый раз загрузили, начальные направления не актуальны --%>
				initialDirections = [] //больше не нужны
				getEducationForms()
		});
	}

    // <%-- грузим доступные формы --%>
	function getEducationForms() {
		var selectedGroups = jQuery('#divCompGroups span[cgID]').attrToArr('cgID')
		var selectedDirs = jQuery('#divDirections span[itemID]').attrToArr('itemID')
		
		var model = {
            applicationId : <%= Model.ApplicationID %>, 
			competitiveGroupIDs: selectedGroups,
			directionKeys: selectedDirs
		};
	    // <%-- спросим у сервера, что есть подходящего --%>
/*		doPostAjax('<%= Url.Generate<InstitutionApplicationDraftController>(x => x.GetAvailableFormsForCompetitiveGroups(null,null)) %>', JSON.stringify(model),
			function (data) {
				if(data.IsError) {
					alert(data.Message);
					return;
				}
				
				var fSet = function (name) {
					jQuery("#EducationFormsSelected_" + name).toggleEnabled(data.Data[name])
					if(!data.Data[name]) jQuery("#EducationFormsSelected_" + name).removeAttr('checked')
				}
			    // <%-- проставляем галочки для всех форм --%>
				fSet('BudgetO');
				fSet('BudgetOZ');
				fSet('BudgetZ');
				fSet('PaidO');
				fSet('PaidOZ');
				fSet('PaidZ');
				fSet('TargetO');
				fSet('TargetOZ');
				fSet('TargetZ');

			    // <%-- грузим список целевых организаций --%>
				var fTSet = function (suff) {
					var selTargetOrg = jQuery('#SelectedTargetOrganizationID' + suff).val()

					var res = '';
					jQuery.each(data.Data['TargetOrganizations' + suff], function(i, e) {
						res += '<option value="' + e.ID + '">' + escapeHtml(e.Name) + "</option>"
					})
					jQuery('#SelectedTargetOrganizationID' + suff).html(res)
					jQuery('#SelectedTargetOrganizationID' + suff).val(selTargetOrg);
				}
				fTSet('O');fTSet('OZ');fTSet('Z');
				targetOrgChanged()
		});*/

                doPostAjax('<%= Url.Generate<ApplicationController>(x => x.GetAvailiableAndExistingPriorities(null, null, null)) %>', JSON.stringify(model),
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

                    var html = 
                            '<td dataVal="' + data.Data.ApplicationPriorities[i].CompetitiveGroupId + '">' + data.Data.ApplicationPriorities[i].CompetitiveGroupName + '</td>' +
                            '<td dataVal="' + data.Data.ApplicationPriorities[i].CompetitiveGroupItemId + '">' + data.Data.ApplicationPriorities[i].CompetitiveGroupItemName + '</td>' +
                            '<td dataVal="' + data.Data.ApplicationPriorities[i].EducationFormId + '">' + data.Data.ApplicationPriorities[i].EducationFormName + '</td>' +
                            '<td dataVal="' + data.Data.ApplicationPriorities[i].EducationSourceId +'">' + data.Data.ApplicationPriorities[i].EducationSourceName + '</td>' + 
                            '<td><input type="text" class="priorityInput" value="' + (data.Data.ApplicationPriorities[i].Priority == null ? '' : data.Data.ApplicationPriorities[i].Priority.toString()) + '">'

                    if (data.Data.ApplicationPriorities[i].TargetOrganizations != null)
                    {
                        html = html + '<br/><select id="targetInput">';
                        for (var j = 0; j < data.Data.ApplicationPriorities[i].TargetOrganizations.length; j++)
                        {
                            html += '<option value="' + data.Data.ApplicationPriorities[i].TargetOrganizations[j].ID + '"';
                            if (data.Data.ApplicationPriorities[i].TargetOrganizations[j].ID ==  data.Data.ApplicationPriorities[i].CompetitiveGroupTargetId)
                                html += ' selected=""';
                             html += '>' + data.Data.ApplicationPriorities[i].TargetOrganizations[j].Name + '</option>';
                        }
                        html += '</select>';
                    }
                    html += '</td>';

                    row.html(html);
                    tab.append(row);
                }
            });

	}

    // <%-- изменилась ли КГ. Показывать ли пользователю сообщение о сносе РВИ --%>
	var isCompetitiveGroupChanged = false

    // <%-- сейчас уже никогда не в портлете. --%>
	<% if(!Url.IsInsidePortlet()) { %>
		jQuery(function ()
		{
			jQuery('#origCompetitiveGroup').hide()
		
		    // <%-- инициализируем данные с сервера. Так проще --%>
		//<% foreach (var pair in Model.CompetitiveGroupNamesByCampaign)
		   {%>
		   	groupByCampaign[<%= pair.Key %>] = JSON.parse('<%= Html.Serialize(pair.Value) %>')//<%} %>
			selectCampaign()

		    // <%-- инициализируем автокомплиты --%>
			autocompleteDropdown(jQuery('#compGroupComplete'), {minLength:1, source: competitiveGroupComplete,
				select: function() {setTimeout(competitiveGroupCompleteSelected, 0)}})
			autocompleteDropdown(jQuery('#dirComplete'), {minLength:1, source: directionComplete,
			    select: function() {setTimeout(directionCompleteSelected, 0)}})
		})
	<% } %>

    // <%-- сохрнаяем данные --%>
		function doSubmit(action) {
			var alertCGDisplayed = false;

		    // <%-- вытаскиваем выбранные кг. смотрим, изменились ли они относительно изначальной --%>
			var selectedGroups = jQuery('#divCompGroups span[cgID]').attrToArr('cgID');
			var isGroupChanged = selectedGroups.length != initialCompGroups.length;
			if(!isGroupChanged) {
				jQuery.each(selectedGroups, function(i, e) {
					if (initialCompGroups.indexOf(parseInt(e)) < 0) {
						isGroupChanged = true;
						return false;
					}
				});
			}

		    // <%-- если изменились, спрашиваем пользователя --%>
			if(isGroupChanged)
			{
				alertCGDisplayed = true;
				if(!confirm('Вы изменили конкурс. При этом сбросятся все сведения о результатах вступительных испытаний. Продолжить?'))
				{
					return false
				}
				action = 'save'
			}

		    // <%-- заполняем модельку данными --%>
			var model =
			{
				ApproveInstitutionCount: jQuery('#ApproveInstitutionCount').attr("checked"),
				NeedHostel: jQuery('#NeedHostel').attr("checked"),
				FirstHigherEducation: jQuery('#FirstHigherEducation').attr("checked"),
				ApprovePersonalData: jQuery('#ApprovePersonalData').attr("checked"),
				FamiliarWithLicenseAndRules: jQuery('#FamiliarWithLicenseAndRules').attr("checked"),
				FamiliarWithAdmissionType: jQuery('#FamiliarWithAdmissionType').attr("checked"),
				FamiliarWithOriginalDocumentDeliveryDate: jQuery('#FamiliarWithOriginalDocumentDeliveryDate').attr("checked"),
				SelectedCompetitiveGroupIDs: jQuery('#divCompGroups span[cgID]').attrToArr('cgID'),
				SelectedDirectionIDs: jQuery('#divDirections span[itemID]').attrToArr('itemID'),
				CampaignID: jQuery('#CampaignID').val(),
				ApplicationID: <%= Model.ApplicationID %>,
				Uid: jQuery('#Uid').val(),
				RegistrationDate: jQuery('#RegistrationDate').val(),
				ActionCommand: action,
				SelectedTargetOrganizationIDO: jQuery("#SelectedTargetOrganizationIDO").val(),
				SelectedTargetOrganizationIDOZ: jQuery("#SelectedTargetOrganizationIDOZ").val(),
				SelectedTargetOrganizationIDZ: jQuery("#SelectedTargetOrganizationIDZ").val(),
                Priority: jQuery('#Priority').val(),
				EducationFormsSelected: 
				{
					BudgetO: jQuery('#EducationFormsSelected_BudgetO').attr("checked"),
					BudgetOZ: jQuery('#EducationFormsSelected_BudgetOZ').attr("checked"),
					BudgetZ: jQuery('#EducationFormsSelected_BudgetZ').attr("checked"),
					PaidO: jQuery('#EducationFormsSelected_PaidO').attr("checked"),
					PaidOZ: jQuery('#EducationFormsSelected_PaidOZ').attr("checked"),
					PaidZ: jQuery('#EducationFormsSelected_PaidZ').attr("checked"),
					TargetO: jQuery('#EducationFormsSelected_TargetO').attr("checked"),
					TargetOZ: jQuery('#EducationFormsSelected_TargetOZ').attr("checked"),
					TargetZ: jQuery('#EducationFormsSelected_TargetZ').attr("checked")
				}
			};
			if(!model.SelectedTargetOrganizationIDO) model.SelectedTargetOrganizationIDO = 0;
			if(!model.SelectedTargetOrganizationIDOZ) model.SelectedTargetOrganizationIDOZ = 0;
			if(!model.SelectedTargetOrganizationIDZ) model.SelectedTargetOrganizationIDZ = 0;
			clearValidationErrors(jQuery('.institutionData'))
			clearValidationErrors(jQuery('.checkData'))
			clearValidationErrors(jQuery('#EducationFormsSelected_NotSelected').parent())
			
			clearValidationErrors(jQuery('#divErrorTop,#divErrorBottom'))
		    // <%-- сохраняем --%>
			doPostAjax("<%= Url.Generate<ApplicationController>(x => x.SaveApplicationCheck(null)) %>", 'model=' + JSON.stringify(model), 
				function (data) {
					//<%-- если уходим назад, ошибки не показываем, но если изменили КГ, то сохраняем, иначе будут проблемы с логикой  --%>
					if((action == "back" || (action == "save" && <%= Model.IsDraft ? "true" : "false" %>)) && !alertCGDisplayed) {
						if (typeof doAppNavigate != "undefined")
							doAppNavigate()
						return false;
					}
				    // <%-- есть ошибки, рисуем их вручную --%>
				if (!addValidationErrorsFromServerResponse(data, false))
				{
				    var isError = false;
				    // <%-- закомментарено. поля скрыты --%>
					/*if ((action == 'next' || (action == "save" && <%= Model.IsDraft ? "false" : "true" %>)) && !(<% if(Model.IsVUZ) { %>model.ApproveInstitutionCount && <%} %>
							model.FirstHigherEducation &&
							model.ApprovePersonalData && 
							model.FamiliarWithLicenseAndRules &&
							model.FamiliarWithAdmissionType && 
							model.FamiliarWithOriginalDocumentDeliveryDate) ) {
						
						<% if(Model.IsVUZ) { %>if(!model.ApproveInstitutionCount) addValidationError(jQuery('#ApproveInstitutionCount'), '') <%} %>
						if(!model.FirstHigherEducation) addValidationError(jQuery('#FirstHigherEducation'), '')
						if(!model.ApprovePersonalData) addValidationError(jQuery('#ApprovePersonalData'), '')
						if(!model.FamiliarWithLicenseAndRules) addValidationError(jQuery('#FamiliarWithLicenseAndRules'), '')
						if(!model.FamiliarWithAdmissionType) addValidationError(jQuery('#FamiliarWithAdmissionType'), '')
						if(!model.FamiliarWithOriginalDocumentDeliveryDate) addValidationError(jQuery('#FamiliarWithOriginalDocumentDeliveryDate'), '')
						jQuery('#divErrorTop,#divErrorBottom').html('<span class="field-validation-error">Не все обязательные чекбоксы отмечены</span>')
						isError = true
						return false
					}*/
					if (isError == true)
					    return false;
				    // <%-- если идём дальше --%>
					if (action == 'next')
					{
					    if (revalidatePage(jQuery('.institutionData'))) return false
					    // <%-- отправляем заявление --%>
						doPostAjax("<%= Url.Generate<ApplicationController>(x => x.SendEntrantApplication(null)) %>", 'model=' + JSON.stringify(model), function (data)
						{					
							if (!addValidationErrorsFromServerResponse(data) && !data.IsError)
							{
								if(alertCGDisplayed) {
									window.location = '<%= Url.Generate<ApplicationController>(x => x.ApplicationEntranceTest()) + "?applicationID=" + Model.ApplicationID %>'
									return false;
								}
								doCheckApplication(<%= Model.ApplicationID %>)
								//if (typeof doAppNavigate != "undefined")
								//	doAppNavigate()
								//else
								//	jQuery('#btnCancel').click()
							}
					
						}, "application/x-www-form-urlencoded")
					}
					else
					{
					    // <%-- если показали алёрт об изменени КГ, идём на РВИ. --%>
						if(alertCGDisplayed) {
							window.location = '<%= Url.Generate<ApplicationController>(x => x.ApplicationEntranceTest()) + "?applicationID=" + Model.ApplicationID %>'
							return false;
						}
					    // <%-- проверяем заявление --%>
						doCheckApplication(<%= Model.ApplicationID %>)
						//if (typeof doAppNavigate != "undefined")
						//	doAppNavigate()
					}
				}
			}, "application/x-www-form-urlencoded", null)
			return false
		}

    // <%-- инициализация --%>
		jQuery(function () {
			targetOrgChanged();
		})
		
    // <%-- проверка заявления --%>
	function doCheckApplication(appID)
	{
		var navFunc = function () {
			if (typeof doAppNavigate != "undefined")
					doAppNavigate()
			else jQuery("#btnCancel").click();
		}
		doPostAjax('<%= Url.Generate<InstitutionApplicationController>(x => x.CheckApplication(null)) %>', JSON.stringify({ApplicationID: appID}), function (data)
		{
			if(data.Extra) {
				navFunc();
			} else { // <%-- есть ошибки, спрашиваем пользователя, что ему делать --%>
				jQuery("#lbCheckDescription").html((data.Message).replace('\n', '<br/>'))
				jQuery("#divDecisionDialog").dialog({
					resizeable: false,
					title: 'Введите причину решения',
					width: 600,
					modal: true,
					buttons: {
						"Принять": function () {
							var model = {ApplicationID: appID, StatusDecision: jQuery("#tbCheckDecision").val()}
							clearValidationErrors(jQuery("#tbCheckDecision").parent())
							if (model.StatusDecision == '')
							{
								addValidationError(jQuery('#tbCheckDecision'), 'Укажите причину решения', true)
								return false
							}
							doPostAjax('<%= Url.Generate<InstitutionApplicationController>(x => x.ApproveApplication(null)) %>', JSON.stringify(model),
								function(data2) {
									navFunc();
									closeDialog(jQuery('#divDecisionDialog'))
								});
							return false;
						},
						"Оставить непрошедшим проверку": function () { navFunc();closeDialog(jQuery('#divDecisionDialog')) }
					},
					close: function () {navFunc()}
				})
			}
		})
		return false
	}


    jQuery(document).ready(function(){
            jQuery('#showDiv').show();
            jQuery('#editDiv').hide();
            if ('<%= Model.RegistrationDate.ToString("HH:mm:ss") == "00:00:00" %>' == "True")
            {
                jQuery('#RegistrationDate').setEnabled()
                jQuery('#dateEditor>.ui-datepicker-trigger').setEnabled();
            }
            else 
            {
                jQuery('#RegistrationDate').setDisabled()
                jQuery('#dateEditor>.ui-datepicker-trigger').setDisabled();
            }
        }
    );

    function changePriorities()
    {
        jQuery('#showDiv').hide();
        jQuery('#editDiv').show();
        jQuery('#changeButton').hide();
    }

    function savePriorities(checkUnique, checkZeroes)
    {
     //Пока - только отображение-сокрытие редактора
            var Priorities = {
                ApplicationId : <%= Model.ApplicationID %>,
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

            if (checkUnique == null)
                checkUniqueValues = true;
            else checkUniqueValues = checkUnique;

            if (checkZeroes == null)
                checkZeroValues = true;
            else checkZeroValues = checkZeroes;

            var model = {
                data: Priorities,
                checkUnique: checkUniqueValues,
                checkZeroes: checkZeroValues
                }

        doPostAjax('<%= Url.Generate<ApplicationController>(x => x.SavePriorities(null, null, null)) %>', JSON.stringify(model),
            function(data)
            {
                if (data.Data == null) 
                {
                    jQuery('#showDiv').show();
                    jQuery('#editDiv').hide();
                    jQuery('#changeButton').show();
                    createPrioritiesShowTable();
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
//								model.CheckUniqueBeforeCreate = !(data.Data[0].ControlID == "NonUniquePriorities");
//								model.CheckZerozBeforeCreate = !(data.Data[0].ControlID == "zeroMessage");
								savePriorities(!(data.Data[0].ControlID == "NonUniquePriorities"), !(data.Data[0].ControlID == "zeroMessage"))
							},
                            "Отмена": function()
                            {
                                jQuery(this).dialog('close');
                            }
        				}
                    });
                }
            });
    }

    function cancelPriorities()
    {
	    <% if(!Url.IsInsidePortlet()) { %>
		    jQuery(function ()
		    {
			    jQuery('#origCompetitiveGroup').hide()
		
		        // <%-- инициализируем данные с сервера. Так проще --%>
		    //<% foreach (var pair in Model.CompetitiveGroupNamesByCampaign)
		       {%>
		   	    groupByCampaign[<%= pair.Key %>] = JSON.parse('<%= Html.Serialize(pair.Value) %>')//<%} %>

		        // <%-- инициализируем автокомплиты --%>
			    autocompleteDropdown(jQuery('#compGroupComplete'), {minLength:1, source: competitiveGroupComplete,
				    select: function() {setTimeout(competitiveGroupCompleteSelected, 0)}})
			    autocompleteDropdown(jQuery('#dirComplete'), {minLength:1, source: directionComplete,
			        select: function() {setTimeout(directionCompleteSelected, 0)}})
		    })
	    <% } %>

	    tmpID = 0;
	    initialCompGroups = JSON.parse('<%= Html.Serialize(Model.SelectedCompetitiveGroupIDs) %>');
	    initialDirections = JSON.parse('<%= Html.Serialize(Model.SelectedDirectionIDs) %>');

        selectCampaign();

        jQuery('#showDiv').show();
        jQuery('#editDiv').hide();
        jQuery('#changeButton').show();
    }

    function createPrioritiesShowTable()
    {

		var selectedGroups = jQuery('#divCompGroups span[cgID]').attrToArr('cgID')
		var selectedDirs = jQuery('#divDirections span[itemID]').attrToArr('itemID')

		var model = {
            applicationId : <%= Model.ApplicationID %>, 
			competitiveGroupIDs: selectedGroups,
			directionKeys: selectedDirs
		};

        doPostAjax('<%= Url.Generate<ApplicationController>(x => x.GetAvailiableAndExistingPriorities(null, null, null)) %>', JSON.stringify(model),
            function (data) {
                var tab = jQuery("#showTable");
                if (tab.length == 0) return; // Почему-то нет такой таблицы

                tab.empty();
                if (data.Data == null) return;

                for (var i = 0; i < data.Data.ApplicationPriorities.length; i++)
                {
                    if (data.Data.ApplicationPriorities[i].Priority == null) continue;

                    var columnData = data.Data.ApplicationPriorities[i].CompetitiveGroupName + ' - ' + data.Data.ApplicationPriorities[i].CompetitiveGroupItemName + ' - ' + data.Data.ApplicationPriorities[i].EducationFormName + ' - ' + data.Data.ApplicationPriorities[i].EducationSourceName + (data.Data.ApplicationPriorities[i].EducationSourceId == 16 ? '(' + data.Data.ApplicationPriorities[i].TargetOrganizationName + ')' : '') + ' - ' + (data.Data.ApplicationPriorities[i].Priority == null ? 'Без приоритета' : data.Data.ApplicationPriorities[i].Priority.toString());
                    tab.append(jQuery('<tr>').html('<td>' + columnData + '</td>'));
                }
            });
    }
</script>