<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.EntranceTestViewModelC>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Model.Institutions" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

<div id="dialogCaption" style="display:none">Редактирование вступительных испытаний</div>
<% if(Model.EditDisabled) { %> В конкурсе отсутствуют специальности, или для них не указаны требуемые вступительные испытания. <%} else { %>
<div id="content">
<% if (!Model.HideMainType) { %>
 <% if  (Model.CanEdit && (!GVUZ.Helper.UrlUtils.IsReadOnly(FBDUserSubroles.CompetitiveGroupsDirection))) { %>
  <div><a href="#" onclick="addBenefit(null);return false" id="btnAddbenefitGeneral">Общие льготы (<%= Model.GeneralBenefitCount %>)</a></div>
<% } %>
<h4>Вступительные испытания конкурса
</h4><%} %>
<table class="gvuzDataGrid tableStatement2" cellpadding="3" id="tableMain" testTypeID="<%= EntranceTestType.MainType %>" <%= Model.HideMainType ? "style=\"display:none\"" : "" %>>
	<thead>
		<% if(Model.CanCopyFromParent) { %> 
		<tr>
			<td colspan="5">
				<%--<a href="#" id="btnCopyFromParent"><img src="<%= Url.Images("copy.gif") %>" alt="copy" /> Скопировать список с уровня <%: Model.ParentLevelName %></a>--%>
			</td>
		</tr>
		<% } %>
		<tr>
			<th style="width:20%"><%= Html.LabelFor(x => x.EntranceTestID) %></th>
			<th style="width:20%"><%= Html.LabelFor(x => x.UID) %></th>
			<th style="width:20%"><%= Html.LabelFor(x => x.Form) %></th>
			<th style="width:20%"><%= Html.LabelFor(x => x.MinScore) %></th>
            <th style="width:20%"><%= Html.LabelFor(x => x.EntranceTestPriority) %></th>
			<th style="width:40px"></th>
			<th>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</th>
		</tr>
	</thead>
	<tbody>
		<tr id="trAddNew">
			<td colspan="6">
				<a href="#" id="btnAddNew" class="add">Добавить новое испытание</a>
			</td>
		</tr>
	</tbody>
</table>
<%-- Творческие испытания --%>
<div class="divCreative" <%= Model.CreativeTestTypeID == 0 ? "style=\"display:none\"" : "" %>>
<h4><%= Model.CreativeTestItemName %></h4>
<table class="gvuzDataGrid tableSpecific tableStatement2" cellpadding="3" testTypeID="<%= Model.CreativeTestTypeID %>">
	<thead>
		<tr>
			<th style="width:20%"><%= Html.LabelFor(x => x.EntranceTestID) %></th>
			<th style="width:20%"><%= Html.LabelFor(x => x.UID)%></th>
			<th style="width:20%"><%= Html.LabelFor(x => x.Form)%></th>
			<th style="width:20%"><%= Html.LabelFor(x => x.MinScore) %></th>
            <th style="width:20%"><%= Html.LabelFor(x => x.EntranceTestPriority) %></th>
			<th style="width:40px"></th>
			<th>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</th>
		</tr>
	</thead>
	<tbody>
		<tr id="trAddNewCreative">
			<td colspan="5">
				<a href="#" id="btnAddNewCreative" class="add">Добавить новое испытание</a>
			</td>
		</tr>		
	</tbody>
</table>
</div>

<%-- Прочие испытания --%>
<div class="divCustom" <%= Model.CustomTestTypeID == 0 ? "style=\"display:none\"" : "" %>>
<h4><%= Model.CustomTestItemName %></h4>
<table class="gvuzDataGrid tableSpecific tableStatement2" cellpadding="3" testTypeID="<%= Model.CustomTestTypeID %>">
	<thead>
		<tr>
			<th style="width:20%"><%= Model.CustomTestTypeID == EntranceTestType.MainType ? Html.LabelFor(x => x.AttestationType) : Html.LabelFor(x => x.EntranceTestID)%></th>
			<th style="width:20%"><%= Html.LabelFor(x => x.UID)%></th>
			<th style="width:20%"><%= Html.LabelFor(x => x.Form)%></th>
			<th style="width:20%"><%= Html.LabelFor(x => x.MinScore) %></th>
            <th style="width:20%"><%= Html.LabelFor(x => x.EntranceTestPriority) %></th>
			<th style="width:40px"></th>
			<th>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</th>
		</tr>
	</thead>
	<tbody>
		<tr id="trAddNewCustom">
			<td colspan="5">
				<a href="#" id="btnAddNewCustom" class="add">Добавить новое испытание</a>
			</td>
		</tr>		
	</tbody>
</table>
</div>
	<%-- Профильные испытания --%>
<div class="divProfile" <%= Model.ProfileTestItems == null ? "style=\"display:none\"" : "" %>>
	<h4><%= Model.ProfileTestItemName %></h4>
	<table class="gvuzDataGrid tableStatement2" cellpadding="3" id="tableProfile" testTypeID="<%= EntranceTestType.ProfileType %>">
		<thead>
			<tr>
				<th style="width:20%"><%= Html.LabelFor(x => x.EntranceTestID) %></th>
				<th style="width:20%"><%= Html.LabelFor(x => x.UID)%></th>
				<th style="width:20%"><%= Html.LabelFor(x => x.Form)%></th>
				<th style="width:20%"><%= Html.LabelFor(x => x.MinScore) %></th>
                <th style="width:20%"><%= Html.LabelFor(x => x.EntranceTestPriority) %></th>
				<th style="width:40px"></th>
				<th>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</th>
			</tr>
		</thead>
		<tbody>
			<tr id="trAddNewProfile">
				<td colspan="5">
					<a href="#" id="btnAddNewProfile" class="add">Добавить новое испытание</a>
				</td>
			</tr>
		</tbody>
	</table>
</div>
</div>
<script type="text/javascript">
	var entranceForms = JSON.parse('<%= Html.Serialize(Model.EntranceSubjects) %>')
	var currentData = JSON.parse('<%= Html.Serialize(Model.TestItems) %>')
	var currentDataCreative = JSON.parse('<%= Html.Serialize(Model.CreativeTestItems) %>')
	var currentDataCustom = JSON.parse('<%= Html.Serialize(Model.CustomTestItems) %>')
	var currentDataProfile = JSON.parse('<%= Html.Serialize(Model.ProfileTestItems) %>')
	var competitiveGroupID = <%= Model.CompetitiveGroupID %>;
	var $hiddenTrEdited;
	var benefitListReturnedCount;
	var allowAnySubjectsForMain = <%= Model.IsCustomEntranceTestsForMainIsAllowed ? "true" : "false" %>;
	var allowAnyNumberOfProfileItems = <%= Model.IsAnyCountOfProfileItemsAllowed ? "true" : "false" %>;
    var readOnly = <%= ((!Model.CanEdit) || GVUZ.Helper.UrlUtils.IsReadOnly(FBDUserSubroles.CompetitiveGroupsDirection) ? "true" : "false" )%>;

    var addEditRow = function($trToAdd, itemID)
    {
        itemID = itemID ? itemID : "";
        if(itemID == null) itemID = ''; //possible fix of strange issue
        var className = $trToAdd.prev().attr('class')
        if(className == 'trline1') className = 'trline2'; else className = 'trline1';
        var etString = '<input type="text" class="entranceTypeText" maxLength="100" />'
        $trToAdd.before('<tr itemID="' + (itemID) + '"  class="'+className + '"><td>' + etString + '</td>' 
		+ '<td align="center"><input type="text" class="entranceUID" maxlength="200" /></td>' 
		+ '<td align="center"><input type="text" class="entranceForm" maxlength="100" /></td>' 
		+ '<td align="center"><input type="text" class="entranceMinScore numeric" maxlength="8" /></td>'
		+ '<td align="center"><select style="text-align: left;" class="entranceEntranceTestPriority numeric"><option value="-1" label="Без приоритета">Без приоритета</option> <option value="1">1</option><option label="2" value="2">2</option><option label="3" value="3">3</option><option label="4" value="4">4</option><option label="5" value="5">5</option><option label="6" value="6">6</option><option label="7" value="7">7</option><option label="8" value="8">8</option><option label="9" value="9">9</option><option label="10" value="10">10</option></select></td>'
		+ '<td align="center" nowrap="nowrap"><a href="#" class="btnSave" onclick="etSaveButton(this);return false;"></a> ' 
		+ '<a href="#" class="btnDeleteU" onclick="etDeleteUButton(this);return false;"></a></td>'
		+ '<td>&nbsp;</td></tr>');
    };

    var createdSavedRow = function($trBefore, itemData)
    {
        var className = $trBefore.prev().attr('class')
        if(className == 'trline1') className = 'trline2'; else className = 'trline1';
        $trBefore.before('<tr itemID="' + itemData.ItemID + '" class="' + className + '"><td>' +
				itemData.TestName + '</td><td>' +
				escapeHtml(itemData.UID == null ? '' : itemData.UID) + '</td><td>' +
				escapeHtml(itemData.Form) + '</td>'
				+ '<td align="center">' + (itemData.ValueString == null ? '' : itemData.ValueString) + '</td>'
                + '<td align="center">' + (itemData.EntranceTestPriority == null ? '' : itemData.EntranceTestPriority.toString()) + '</td>'
				+ '<td align="center" nowrap="nowrap"><a href="#" class="btnEdit" onclick="etEditButton(this);return false;">' +
				'</a>&nbsp;'+
					(itemData.CanRemove 
					? ('<a href="#" class="btnDelete" onclick="etDeleteButton(this);return false;"></a>') 
					: ('<span class="btnDeleteGray"></span>'))
				+ '</td><td>' 
                + (readOnly ? '<label class="tdBenefits">' : '<a href="#" onclick="addBenefit(this);return false;" class="tdBenefits">')	           
                + 'Льготы&nbsp;(' + itemData.BenefitCount + ')'
                + (readOnly ? '</label>' : '</a>')                
                + '</td></tr>');
    };

	var ddlEntranceTestIDs = []
	var ddlEntranceTestNames = []
	var ddlEntranceTestMinValues = []
	var ddlEntrancePriorities = []
	var ddlEntranceTestUIDs = []

	function addNewEntranceTestRow(el, nonChangeName, itemID)
	{
		jQuery('.btnDeleteU').click();
		addEditRow(jQuery(el).parents('tr'), itemID);

		ddlEntranceTestIDs = []
		ddlEntranceTestNames = []

		if(jQuery('.entranceTypeText').length > 0)
		{
			var isProfile = jQuery(el).parents('#tableProfile').length > 0
			for (var i = 0; i < entranceForms.length; i++)
			{
				if(nonChangeName == null || entranceForms[i].Name == nonChangeName)
				{
					//if(!isProfile || entranceForms[i].ID == <%= Model.ProfileTestSubjectID %>)
					{
						ddlEntranceTestIDs.push(entranceForms[i].ID)
						ddlEntranceTestNames.push(entranceForms[i].Value)
						ddlEntranceTestMinValues.push(entranceForms[i].MinValue)
                        ddlEntrancePriorities.push(entranceForms[i].EntranceTestPriority)
					}
				}
			}
			//jQuery('.entranceType')[0].onchange = function() { jQuery('.entranceMinScore').val(jQuery(this).children('option:selected').attr('minScore')) }
			if(ddlEntranceTestIDs.length == 1)
			{
				jQuery('.entranceTypeText').val(ddlEntranceTestNames[0])
				jQuery('.entranceTypeText').attr('disabled', 'disabled')
				jQuery('.entranceMinScore').val(ddlEntranceTestMinValues[0])
			}
			if(ddlEntranceTestIDs.length > 1 && jQuery(el).parents('.tableSpecific').length == 0)
			{
				autocompleteDropdown(jQuery('.entranceTypeText'), {source: function(ui, response) 
				{
					var res = []
					var x = ui.term.toUpperCase()
					for(var i = 0; i < ddlEntranceTestNames.length; i++)
						if(ddlEntranceTestNames[i].toUpperCase().indexOf(x) >= 0)
						{
							res.push(ddlEntranceTestNames[i])
							//if(res.length > 10) break
						}
					response(res)
				},
				select: function() {setTimeout(function() {jQuery('.entranceTypeText').change()}, 0)},
				 minLength: 2})

				jQuery('.entranceTypeText').change(function() {
					var selVal = jQuery(this).val()
					for(var i = 0; i < ddlEntranceTestNames.length; i++)
					if(ddlEntranceTestNames[i] == selVal)
					{
						jQuery('.entranceMinScore').val(ddlEntranceTestMinValues[i])
						break;
					}
				})
			}
		}
		else
			jQuery('.entranceMinScore').val(0)
		
		jQuery(el).parents('tr:first').hide()
		return false
	}

	jQuery('#btnAddNew,#btnAddNewCustom,#btnAddNewCreative,#btnAddNewProfile').click(function() {addNewEntranceTestRow(this, null, null);return false;})

	function etEditButton (el)
	{
		var $tr = jQuery(el).parents('tr:first');
		var child = $tr.children('td')
		var canChange = $tr.find('.btnDeleteGray').length == 0 /*&& $tr.parents('#tableProfile').length == 0*/
		addNewEntranceTestRow(el, canChange ? null : child[0].innerHTML, $tr.attr('itemID'))
		jQuery('.entranceTypeText').val(unescapeHtml(child[0].innerHTML))
		jQuery('.entranceUID').val(unescapeHtml(child[1].innerHTML))
		jQuery('.entranceForm').val(unescapeHtml(child[2].innerHTML))
		jQuery('.entranceMinScore').val(child[3].innerHTML)
        jQuery('.entranceEntranceTestPriority').val(child[4].innerHTML);
		$hiddenTrEdited = $tr;
		$tr.hide();
		return false
	}

	function etDeleteButton (el)
	{
		var $tr = jQuery(el).parents('tr');
		confirmDialog('Вы действительно хотите удалить испытание?', function () {
		doPostAjax('<%= Url.Generate<EntranceTestController>(x => x.DeleteEntranceTestItem(null)) %>',
				'testItemID=' + $tr.attr('itemID'), function (data, status)
				{
					if(data.IsError) alert(data.Message);
					else
					{
						$tr.remove().detach();
						if(jQuery('#tableProfile tbody tr').length == 1)
							jQuery('#trAddNewProfile').show();
						ensureMainTestCount()
						if(updateEntranceTestCount) updateEntranceTestCount();
					}
				}, "application/x-www-form-urlencoded")
		})
		return false
	}
	
	function etSaveButton(el)
	{
		var $tr = jQuery(el).parents('tr');
		clearValidationErrors($tr)

		var testID = 0
		var isError = false
		var selVal = jQuery('.entranceTypeText').val()
		if(jQuery(el).parents('.tableSpecific').length == 0)
		{
			for(var i = 0; i < ddlEntranceTestNames.length; i++)
				if(ddlEntranceTestNames[i] == selVal)
				{
					testID = ddlEntranceTestIDs[i]
				}
			if(testID == 0 && !allowAnySubjectsForMain && jQuery(el).parents('#tableProfile').length == 0)
			{
				var $el = jQuery('.entranceTypeText')
				$el.removeClass('input-validation-error-fixed').addClass('input-validation-error')
				if(typeof $el.attr('autocomplete') != undefined) $el = $el.next()
				$el.after('<span class="field-validation-error"><br/>Некорректное название дисциплины</span>')
				isError = true
			}
		}
		if(jQuery('.entranceForm').val() == '')
		{
			jQuery('.entranceForm').removeClass('input-validation-error-fixed').addClass('input-validation-error')
			jQuery('.entranceForm').after('<span class="field-validation-error"><br/>Форма проведения обязательна</span>')
			isError = true
		}

		var valFloat = new Number(jQuery('.entranceMinScore').val().replace(',', '.'))
		if(valFloat < 0 || valFloat > 100 || isNaN(valFloat))
		{
			jQuery('.entranceMinScore').removeClass('input-validation-error-fixed').addClass('input-validation-error')
			jQuery('.entranceMinScore').after('<span class="field-validation-error"><br/>Балл должен быть числом</span>')
			isError = true
		}
		if(isError) return false
		var model =
			{
				CompetitiveGroupID: <%= Model.CompetitiveGroupID %>,
				EntranceTestID: testID,
				EntranceTestName: selVal,
				UID: jQuery('.entranceUID').val(),
				Form: jQuery('.entranceForm').val(),
				MinScoreString: jQuery('.entranceMinScore').val(),
				TestItemID: $tr.attr('itemID') ? $tr.attr('itemID') : null,
				TestTypeID: $tr.parents('table.gvuzDataGrid:first').attr('testTypeID'),
                EntranceTestPriority: jQuery('.entranceEntranceTestPriority').val() == -1 ? null : jQuery('.entranceEntranceTestPriority').val()
			}
		doPostAjax('<%= Url.Generate<EntranceTestController>(x => x.CreateEntranceTestItem(null)) %>',
					JSON.stringify(model), function (data, status)
					{
						if(!addValidationErrorsFromServerResponseLocal(data))
						{
							if($hiddenTrEdited) $hiddenTrEdited.remove().detach();
							jQuery('#trAddNew').show();
							ensureMainTestCount()
							jQuery('#trAddNewCreative').show();
							jQuery('#trAddNewCustom').show();
						    if(allowAnyNumberOfProfileItems)
							    jQuery('#trAddNewProfile').show();
							createdSavedRow($tr, data.Data);
							$tr.remove().detach();
							if(updateEntranceTestCount) updateEntranceTestCount();
						}
					})
		return false
	}

	function ensureMainTestCount() {
		if(jQuery('#tableMain tbody tr').length - 1 >= <%= Model.MainTestAllowedCount %>)
			jQuery('#trAddNew').hide();
		else
			jQuery('#trAddNew').show();
	}

	function etDeleteUButton (el)
	{
		jQuery(el).parents('tr').remove();
		jQuery('#trAddNew,#trAddNewCustom,#trAddNewCreative').show()
		ensureMainTestCount()
		if(jQuery('#tableProfile tbody tr').length == 1) jQuery('#trAddNewProfile').show();

		if($hiddenTrEdited) $hiddenTrEdited.show();
		$hiddenTrEdited = null;
		return false
	}
	var initialFillData = function ()
	{
		if(currentData != null)
		for(var i = 0; i < currentData.length; i++)
			createdSavedRow(jQuery('#trAddNew'), currentData[i]);
		ensureMainTestCount()
		if(currentDataCreative != null)
		for(var i = 0; i < currentDataCreative.length; i++)
			createdSavedRow(jQuery('#trAddNewCreative'), currentDataCreative[i]);
		if(currentDataCustom != null)
		for(var i = 0; i < currentDataCustom.length; i++)
			createdSavedRow(jQuery('#trAddNewCustom'), currentDataCustom[i]);
		if(currentDataProfile != null)
		for(var i = 0; i < currentDataProfile.length; i++)
		{
		    createdSavedRow(jQuery('#trAddNewProfile'), currentDataProfile[i]);
		    if(!allowAnyNumberOfProfileItems)
			    jQuery('#trAddNewProfile').hide();
		}

        
        if (readOnly)
		{
			jQuery('.content input').addClass('view').attr('readonly', 'readonly')
			jQuery('#trAddNew,#trAddNewCustom,#trAddNewCreative,#trAddNewProfile').hide();
			jQuery('.content .btnDelete, .content .btnDeleteGray, .content .btnEdit').hide();            
		}
		
	}
	initialFillData();

	function addValidationErrorsFromServerResponseLocal(data)
	{
		if(data.IsError)
		{
			alert(data.Message)
			if(data.Data != null && data.Data.length != null)
			for(var i = 0; i < data.Data.length;i++)
			{
				if(data.Data[i].ControlID == 'Form')
					addValidationError(jQuery('.entranceForm'), '')
				if(data.Data[i].ControlID == 'MinScore')
					addValidationError(jQuery('.entranceMinScore'), '')
				if(data.Data[i].ControlID == 'EntranceTestID')
				{
					addValidationError(jQuery('.entranceTypeText'), '')
				}
				if(data.Data[i].ControlID == 'UID')
				{
					addValidationError(jQuery('.entranceUID'), '')
				}
			}
			return true
		}
		return false
	}

	function addBenefit(el)
	{
		var itemID = '0'
		var tit = 'Общие льготы'
		if(el != null)
		{
			itemID = jQuery(el).parents('tr:first').attr('itemID')
			tit = ('Условия предоставления льгот для предмета ' + jQuery(el).parents('tr:first').children('td')[0].innerHTML)
		}
		if(el == null) 
		{
			el = jQuery('#btnAddbenefitGeneral')[0]
		}
		doPostAjax('<%= Url.Generate<BenefitController>(c => c.BenefitList(null, null)) %>', 'entranceTestItemID=' + itemID + '&competitiveGroupID=<%=Model.CompetitiveGroupID %>', function (data)
		{
			jQuery('#divBenefitListDialog').html(data);
			jQuery('#divBenefitListDialog').dialog({
				modal: true,
				width: 850,
				title: tit,
				close: function () {jQuery(el).html(jQuery(el).html().replace(/\(\d+\)/, '(' + benefitListReturnedCount +')')); }
			}).dialog('open');
		},
		"application/x-www-form-urlencoded", "html")
		return false
	}
</script>
<%} %> <%-- edit allowed --%>