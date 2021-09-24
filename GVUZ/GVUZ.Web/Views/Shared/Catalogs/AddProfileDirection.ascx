<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.Administration.Catalogs.AddProfileDirectionViewModel>" %>

<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

<style type="text/css">
	span.ui-tool select
	{
		margin: 0px;
		padding: 0px;
	}
	
</style>

<div id="content">
	<table class="gvuzData">
		<tbody>
			<tr>
				<td class="caption">
					<%= Html.LabelFor(m => m.InstitutionID) %>:
				</td>
				<td>
					<%= Html.TextBoxExFor(m => m.Name, new { style = "width: 640px;"})%>
				</td>
			</tr>

			<tr>
				<td class="caption"></td>
				<td>
					<table class="directionList gvuzData" style="width: 640px;">
						<thead>
							<tr>
								<th colspan="2" align="left">
									<%= Html.LabelFor(x => x.Directions)%>
								</th><th></th>
							</tr>
						</thead>
						<tbody>
							<tr id="trAddNew">
								<td colspan="3">
									<a href="javascript:void(0)" id="btnAddNew" class="add">Добавить дисциплину</a>
								</td>
							</tr>
						</tbody>
					</table>
					<input type="text" id="directionNameTrick" style="display:none" />				
				</td>
			</tr>
		</tbody>
	</table>
	<div style="display:none">
		<input id="btnSubmit" type="button" value="Сохранить" />
		<input id="btnCancel" type="button" value="Отмена" />
	</div>
</div>

<script type="text/javascript">	

	var editMode = JSON.parse('<%= Html.Serialize(Model.IsEditMode) %>')
	var directionNames = JSON.parse('<%= Html.Serialize(Model.DirectionNameList) %>')
	var institutionNames = JSON.parse('<%= Html.Serialize(Model.InstitutionNameList) %>')
	var directionData = JSON.parse('<%= Html.Serialize(Model.Directions) %>')	

	var $trEdited = null
	var isError = false
	function fillModel()
	{		
		var model = 
			{
				InstitutionID: <%= Model.InstitutionID %>,
				Name: jQuery('#Name').val()	,
				Directions: []
			}

			jQuery('.directionList tr:.saved').each(function() 
			{
				model.Directions.push(
				{
					Name: unescapeHtml(jQuery(this).find('td:first').html())
				})
			})
			return model
	}

	function submitData()
	{
		var model = fillModel();
		
		doPostAjax("<%= Url.Generate<AdministrationController>(x => x.CreateProfileDirection(null)) %>", JSON.stringify(model), function (data)
		{
			if(!addValidationErrorsFromServerResponse(data))
			{
				createdItem = data.Data
				jQuery('#btnCancel').click()
			}
			unblockUI()
		}, null, null, false)
	}

	jQuery('#btnSubmit').click(function () {		
		if(!isError){
			clearValidationErrors(jQuery('.gvuzData'))
			submitData(); 
		}
	})
	jQuery('#btnCancel').click(function () { closeDialog(jQuery('#dialog')); })

	

	function addEditRow($trToAdd)
	{
		$trToAdd.before(
		'<tr class="trUnsaved"><td><input type="text" style="width:600px" id="inDirection" maxlength="255" value="" class="directionNames" onkeyup="checkIsValid(this);"/></td>' 
		+ '<td align="center" nowrap="nowrap"><a href="#" title="Сохранить" class="btnSaveS" onclick="saveNewRow(this);return false"></a>&nbsp;' 
		+ '<a href="#" class="btnDeleteUS" title="Удалить" onclick="cancelEditing();return false"></a></td></tr>')
		
		autocompleteDropdown(jQuery('.directionNames'), {source: directionNames, minLength: 0, delay: 0});

		$('.directionNames').change(function() {
			checkIsValid($('.directionNames'));
		});		
	}

	function createExistedRow($trBefore, item)
	{
		$trBefore.before('<tr class="saved"><td width="600px">' +
				escapeHtml(item.Code + ' ' + item.Name) + '</td>'
				+ '<td align="center" nowrap="nowrap"><a href="#" class="btnEditS"  title="Редактировать" onclick="editRow(this);return false"></a>&nbsp;'+
				'<a href="#" class="btnDeleteS" title="Удалить" onclick="deleteRow(this);return false"></a></td></tr>');
		jQuery('#tbEmpty').val('1')
	}

	function createAddedRow($trBefore, item)
	{
		$trBefore.before('<tr class="saved"><td width="600px">' +
				escapeHtml(item.Name) + '</td>'
				+ '<td align="center" nowrap="nowrap"><a href="#" class="btnEditS"  title="Редактировать" onclick="editRow(this);return false"></a>&nbsp;'+
				'<a href="#" class="btnDeleteS" title="Удалить" onclick="deleteRow(this);return false"></a></td></tr>');
		jQuery('#tbEmpty').val('1')
	}

	function cancelEditing()
	{
		jQuery('#trAddNew').show()
		jQuery('.trUnsaved').remove().detach()
		if($trEdited != null) $trEdited.show()
		$trEdited = null
	}

	function deleteRow(el)
	{
		jQuery(el).parents('tr:first').remove().detach()
		if(jQuery('.directionList tr:.saved').length == 0)
			jQuery('#tbEmpty').val('')
		else
			jQuery('#tbEmpty').val('1')
		jQuery('#btnAddNew').focus().blur()
	}

	function checkIsValid(el) {
		var $tr = jQuery(el).parents('tr:first')
		var $el = jQuery('#inDirection')
		var newDirectionName = $el.val();		

		if(jQuery.inArray(newDirectionName, directionNames) >= 0){	
			clearValidationErrors($tr)
		}else{
			$el.removeClass('input-validation-error-fixed').addClass('input-validation-error')
		}
	}

	function saveNewRow(el)
	{
		var $tr = jQuery(el).parents('tr:first')
		clearValidationErrors($tr)
		var newDirectionName = $tr.find('.directionNames').val();
		
		if(jQuery.inArray(newDirectionName, directionNames) >= 0){			
			createAddedRow($tr, {Name: newDirectionName})
			$tr.remove().detach()
			jQuery('#trAddNew').show()
			if($trEdited != null) $trEdited.remove().detach()
			$trEdited = null
			isError = false
		}else{
			var $el = jQuery('#inDirection')
			$el.removeClass('input-validation-error-fixed').addClass('input-validation-error')
			if(typeof $el.attr('autocomplete') != undefined) $el = $el.next()
			$el.after('<span class="field-validation-error"><br/>Некорректное название направления</span>')			
			isError = true
		}		
	}

	function editRow(el)
	{
		cancelEditing()
		var $tr = jQuery(el).parents('tr:first')
		$trEdited = $tr
		addEditRow($tr)
		jQuery('.directionNames').val(unescapeHtml($tr.find('td:first').html()))
		jQuery('.directionValue').val(unescapeHtml($tr.find('td:first').next().html()))
		$trEdited.hide()
		return false
	}

	jQuery('#btnAddNew').click(function() 
	{
		cancelEditing()
		addEditRow(jQuery('#trAddNew'))
		jQuery('#trAddNew').hide()
	})

	function initialFillData()
	{	
		for(var i = 0; i < directionData.length; i++)
			createExistedRow(jQuery('#trAddNew'), directionData[i])
		if(!editMode)
			autocompleteDropdown(jQuery('#Name'), {source: institutionNames, minLength: 0, delay: 0});
		
		jQuery('#Name').attr('readonly',editMode)
		if(editMode){
			jQuery('#Name').addClass('view')			
		}
	}

	initialFillData()	

</script>