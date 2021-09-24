<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.Administration.Catalogs.AddOlympicViewModel>" %>
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
					<%=Html.LabelFor(m => m.OlympicNumber)%>:
				</td>
				<td>
					<%= Html.TextBoxExFor(m => m.OlympicNumber, null)%>
				</td>
			</tr><tr>
				<td class="caption">
					<%=Html.LabelFor(m => m.OlympicID)%>:
				</td>
				<td>
					<%= Html.TextBoxExFor(m => m.Name, null)%>
				</td>
			</tr><tr>
				<td class="caption">
					<%=Html.LabelFor(m => m.OrganizerName)%>:
				</td>
				<td>
					<%= Html.TextBoxExFor(m => m.OrganizerName, null)%>
				</td>
			</tr>
            <!-- оставляем грабли для 2012 года -->
            <% if (Model.OlympicYear == 2012) { %>
            <tr> 
				<td class="caption">
					<%= Html.LabelFor(m => m.OlympicLevelID) %>:
				</td>
				<td>
					<%= Html.DropDownListExFor(x => x.OlympicLevelID, Model.Levels, null) %>					
				</td>
			</tr>
            <% } %>
            <tr>
				<td class="caption">
					<%=Html.LabelFor(m => m.OlympicYear)%>:
				</td>
				<td>
					<%= Html.TextBoxExFor(m => m.OlympicYear, null)%>
				</td>
			</tr>
			<tr>
				<td class="caption"></td>
				<td>
					<table class="subjectList gvuzDataGrid" style="width: 380px">
						<thead>
							<tr>
								<th align="left">
									<%= Html.LabelFor(x => x.Subjects)%>
								</th>
								<th align="left" <% if (Model.OlympicYear == 2012) { %> style="display: none" <% } %>>
									Уровень
								</th>
                                <th width="40px"></th>
							</tr>
						</thead>								
						<tbody>
							<tr id="trAddNewSubject">
								<td colspan="3">
									<a href="javascript:void(0)" id="btnAddNewSubject" class="add">Добавить дисциплину</a>
								</td>
							</tr>
						</tbody>
					</table>
					<input type="text" id="subjectNameTrick" style="display:none" />				
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

	var subjectNames = JSON.parse('<%= Html.Serialize(Model.SubjectNameList) %>')
	var subjectData = JSON.parse('<%= Html.Serialize(Model.Subjects) %>')
	var subjectLevels = JSON.parse('<%= Html.Serialize(Model.SubjectLevels) %>')
	var is2012 = '<%= Model.OlympicYear == 2012 %>'
	var $trEdited = null
	var isError = false
	
    function fillModel() {
		var model = 
			{
				OlympicID: <%= Model.OlympicID%>,
				Name: jQuery('#Name').val()	,
				OlympicLevelID: jQuery('#OlympicLevelID').length > 0 ? jQuery('#OlympicLevelID').val() : null,
				OlympicYear: jQuery('#OlympicYear').val(),
				OrganizerName: jQuery('#OrganizerName').val(),
				OlympicNumber: jQuery('#OlympicNumber').val(),
				Subjects: []
			}

			jQuery('.subjectList tr:.saved').each(function() 
			{
				model.Subjects.push(
				{
					SubjectName: unescapeHtml(jQuery(this).find('td:first').html()),
					SubjectLevelName: unescapeHtml(jQuery(this).find('td:first').next().html())
				})
			})
			return model
	}

	function submitData()
	{
		var model = fillModel();
		
		doPostAjax("<%= Url.Generate<AdministrationController>(x => x.CreateOlympic(null)) %>", JSON.stringify(model), function (data)
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

	

	function addEditRow($trToAdd) {
	    var stub = '';
	    if (is2012) stub ='style="display:none"';
	    
		$trToAdd.before(
		'<tr class="trUnsaved"><td><input type="text" style="width:282px" id="inSubject" maxlength="255" value="" class="subjectNames" onkeyup="checkIsValid(this);"/></td>' 
		+ '<td ' + stub + '>' 
		+ '<input type="text" style="width:50px" id="inLevel" maxlength="10" value="" class="subjectLevels" onkeyup="checkIsValid(this);"/></td>'
        + '<td align="center" nowrap="nowrap"><a href="#" title="Сохранить" class="btnSaveS" onclick="saveNewRow(this);return false"></a>&nbsp;' 
		+ '<a href="#" class="btnDeleteUS" title="Удалить" onclick="cancelEditing();return false"></a></td></tr>')
		
		autocompleteDropdown(jQuery('.subjectNames'), {source: subjectNames, minLength: 0, delay: 0});
	    autocompleteDropdown(jQuery('.subjectLevels'), {source: subjectLevels, minLength: 0, delay: 0});

		$('.subjectNames').change(function() {
			checkIsValid($('.subjectNames'));
		});		
		$('.subjectLevels').change(function() {
			checkIsValidLevels($('.subjectLevels'));
		});		
	}

	function createAddedRow($trBefore, item)
	{
	    var className = $trBefore.prev().attr('class')
	    if((typeof className != "undefined") && className.indexOf('trline1') >= 0) className = 'trline2'; else className = 'trline1';
	    $trBefore.before('<tr class="saved ' + className + '"><td>' +
	        escapeHtml(item.SubjectName) + '</td>'
	        + '<td>' + item.SubjectLevelName  + '</td>'
				+ '<td align="center" nowrap="nowrap"><a href="#" class="btnEditS"  title="Редактировать" onclick="editRow(this);return false"></a>&nbsp;'
		        + '<a href="#" class="btnDeleteS" title="Удалить" onclick="deleteRow(this);return false"></a></td></tr>');
		jQuery('#tbEmptySubjects').val('1')
	}

	function cancelEditing()
	{
		jQuery('#trAddNewSubject').show()
		jQuery('.trUnsaved').remove().detach()
		if($trEdited != null) $trEdited.show()
		$trEdited = null
	}

	function deleteRow(el)
	{
		jQuery(el).parents('tr:first').remove().detach()
		if(jQuery('.subjectList tr:.saved').length == 0)
			jQuery('#tbEmptySubjects').val('')
		else
			jQuery('#tbEmptySubjects').val('1')
		jQuery('#btnAddNewSubject').focus().blur()
	}

	function checkIsValid(el) {
		var $tr = jQuery(el).parents('tr:first')
		var $el = jQuery('#inSubject')
		var newSubjectName = $el.val();		

		if(jQuery.inArray(newSubjectName, subjectNames) >= 0){	
			clearValidationErrors($tr)
		}else{
			$el.removeClass('input-validation-error-fixed').addClass('input-validation-error')
		}
	}

	function checkIsValidLevels(el) {
		var $tr = jQuery(el).parents('tr:first')
		var $el = jQuery('#inLevel')
		var newSubjectName = $el.val();		

		if(jQuery.inArray(newSubjectName, subjectLevels) >= 0){	
			clearValidationErrors($tr)
		}else{
			$el.removeClass('input-validation-error-fixed').addClass('input-validation-error')
		}
	}

	function saveNewRow(el)
	{
		var $tr = jQuery(el).parents('tr:first')
		clearValidationErrors($tr)
		var newSubjectName = $tr.find('.subjectNames').val();
	    var newSubjectLevel = $tr.find('.subjectLevels').val();
		
		if(jQuery.inArray(newSubjectName, subjectNames) >= 0){			
			createAddedRow($tr, {SubjectName: newSubjectName, SubjectLevelName: newSubjectLevel})
			$tr.remove().detach()
			jQuery('#trAddNewSubject').show()
			if($trEdited != null) $trEdited.remove().detach()
			$trEdited = null
			isError = false
		}else{
			var $el = jQuery('#inSubject')
			$el.removeClass('input-validation-error-fixed').addClass('input-validation-error')
			if(typeof $el.attr('autocomplete') != undefined) $el = $el.next()
			$el.after('<span class="field-validation-error"><br/>Некорректное название дисциплины</span>')			
			isError = true
		}		
	}

	function editRow(el)
	{
		cancelEditing();
	    var $tr = jQuery(el).parents('tr:first');
	    $trEdited = $tr;
	    addEditRow($tr);

	    jQuery('.subjectNames').val(unescapeHtml($tr.find('td:first').html()));
	    jQuery('.subjectLevels').val(unescapeHtml($tr.find('td:first').next().html()));
	    $trEdited.hide();
	    return false;
	}

	jQuery('#btnAddNewSubject').click(function() 
	{
		cancelEditing()
		addEditRow(jQuery('#trAddNewSubject'))
		jQuery('#trAddNewSubject').hide()
	})

	function initialFillData()
	{	
		for(var i = 0; i < subjectData.length; i++)
			createAddedRow(jQuery('#trAddNewSubject'), subjectData[i])
	}

	initialFillData();
		
</script>
