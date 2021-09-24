<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.AddBenefitViewModelC>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.ViewModels" %>
<div id="content">
<div id="divErrorBlockOl" style="overflow:hidden"></div>
<table class="gvuzData">
	<tbody>
		<tr>
			<td>
				<b><%=Html.LabelFor(m => m.DiplomaTypeID)%>:</b>
			</td>
			<td>
				<b><%=Html.LabelFor(m => m.OlympicLevelFlags)%>:</b>
			</td>
			<td>
				<b><%=Html.LabelFor(m => m.BenefitTypeID)%>:</b>
			</td>
		</tr>
		<tr>
			<td valign="top" id="tbDiplomaType" nowrap="nowrap">
				<input type="checkbox" id="cbDiplomaTypePrize" <%= Model.DiplomaTypeID == 2 || Model.DiplomaTypeID == 3 ? "checked=\"checked\"" : ""  %> />
				<label for="cbDiplomaTypePrize">Призер</label><br />
				<input type="checkbox" id="cbDiplomaTypeWinner" <%= Model.DiplomaTypeID == 1 || Model.DiplomaTypeID == 3 ? "checked=\"checked\"" : ""  %> />
				<label for="cbDiplomaTypeWinner">Победитель</label><br />

			</td>
			<td valign="top" id="tbOlympic">
				<input type="checkbox" id="cbOlympic0" onclick="doOlympicChange()"  /><label for="cbOlympic0">Все уровни</label><br />
				<input type="checkbox" id="cbOlympic1" onclick="cbResetOlympicList()" /><label for="cbOlympic1">I уровень</label><br />
				<input type="checkbox" id="cbOlympic2" onclick="cbResetOlympicList()"  /><label for="cbOlympic2">II уровень</label><br />
				<input type="checkbox" id="cbOlympic3" onclick="cbResetOlympicList()" /><label for="cbOlympic3">III уровень</label><br />
			</td>
			<td valign="top">
				<div style="display:none"><%= Html.DropDownListFor(m => m.BenefitTypeID, new SelectList(Model.BenefitTypes, "ID", "Name"))%></div>
				<span id="spBenefitTypeID"><%: Model.FirstBenefitTypeName%></span><br /><br />
				<span <%= Model.HideProfileSubject ? "style=\"display:none\"" : "" %>>
				<input type="checkbox" id="IsProfileSubject" name="IsProfileSubject" <%= Model.IsProfileSubject ? "checked=\"checked\"" : "" %> onclick="cbResetOlympicList()" /><%= Html.LabelFor(x => x.IsProfileSubject) %></span>
			</td>
		</tr>
		<tr>
			<td><b><%= Html.TableLabelFor(x => x.UID) %></b></td>
			<td colspan="2"><input type="text" maxlength="200" id="benefitItemUID" value="<%: Model.UID %>"/></td>
		</tr>
		<tr>
			<td colspan="3">
				<b>Олимпиады из утвержденного перечня олимпиад школьников:</b>
			</td>
		</tr>
        <tr>
	        <td colspan="3">
		        Год олимпиады: <%=Html.DropDownListFor(c => c.OlympicYearID, new SelectList(Model.OlympicYears), new { onchange = "doOlympicYearChange();" })%>
	        </td>
        </tr>
		<tr>
			<td colspan="3">
				<input type="checkbox" id="IsForAllOlympic" name="IsForAllOlympic" <%= Model.IsForAllOlympic ? "checked=\"checked\"" : "" %> onclick="cbAllOlympicClick()" /><%= Html.LabelFor(x => x.IsForAllOlympic) %>
			</td>
		</tr>
		<tr>
			<td colspan="3">
				<table class="gvuzDataGrid" cellpadding="3" id="tableOlympic">
					<thead>
						<tr>
							<th ><%= Html.LabelFor(x => x.OlympicDescr.Number) %></th>
							<th ><%= Html.LabelFor(x => x.OlympicDescr.Level)%></th>
                            <th ><%= Html.LabelFor(x => x.OlympicDescr.Year)%></th>
							<th ><%= Html.LabelFor(x => x.OlympicDescr.Name)%></th>
							<th style="width:40px"></th>
						</tr>
					</thead>
					<tbody>
						<tr id="trAddNewOlympic">
							<td colspan="5">
								<a href="#" id="btnAddNewOlympic" class="add" onclick="blAddButton()">Добавить олимпиаду</a><br />
								<a href="#" id="btnAddNewOlympicMany" class="add" onclick="addMultipleOlympic();return false;">Добавить несколько олимпиад</a>
							</td>
						</tr>
					</tbody>
				</table>
				
			</td>
		</tr>
        <% if (Model.EntranceTestItemID > 0)
        { %>
        <tr>
            <td colspan="3">
                <table>
                    <tr>
                        <td>
                            <%= Html.LabelFor(x => x.MinEgeValue) %>
                        </td>
                        <td>
                            <input type="text" id="minEge" value="<%= Model.MinEgeValue.HasValue ? Model.MinEgeValue.ToString() : "" %>" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <% }
          else { %>
          <tr>
            <td colspan="3"><%= Html.LabelFor(x => x.MinEgeValue) %></td>
          </tr>
          <tr>
            <td colspan="3">
                <table class="gvuzDataGrid">
                    <thead>
                        <th><%=Html.LabelFor(x => x.SubjectDescr.SubjectName) %></th>
                        <th><%=Html.LabelFor(x => x.SubjectDescr.EgeMinValue) %></th>
                        <th></th>
                    </thead>
                    <tbody>
                        <tr class="fakeRow"></tr>
                        <tr id="trAddSubject">
                            <td colspan="3">
								<a href="#" id="btAddSubject" class="add" onclick="subjectAddButton()">Добавить предмет</a><br />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
          </tr>
        <% } %>
	</tbody>
</table>
<div style="display:none">
	<input id="btnSubmitAB" type="button" value="Сохранить" /> 
	<input id="btnCancelAB" type="button" value="Отмена" />
</div>
</div>
<script type="text/javascript">

	var allOlympic = JSON.parse('<%= Html.Serialize(Model.AllOlympic) %>')
	var attachedOlympic = JSON.parse('<%= Html.Serialize(Model.AttachedOlympic) %>')
    var allSubjects = JSON.parse('<%=Html.Serialize(Model.BenefitItemSubjects) %>')
    var allAvailiableSubjects = JSON.parse('<%= Html.Serialize(Model.AllSubjects) %>')

	function prepareModel() {
        var model = {
	        BenefitItemID: <%= Model.BenefitItemID %>,
	        EntranceTestItemID: <%= Model.EntranceTestItemID %>,
	        CompetitiveGroupID: <%= Model.CompetitiveGroupID %>,
	        SubjectID: <%= Model.SubjectID %>,
	        OlympicYearID: jQuery('#OlympicYearID').val(),
	        UID: jQuery('#benefitItemUID').val(),
	        DiplomaTypeID: (jQuery('#cbDiplomaTypePrize').attr('checked') * 2 + jQuery('#cbDiplomaTypeWinner').attr('checked')),
	        BenefitTypeID: jQuery('#BenefitTypeID').val(),
	        IsForAllOlympic: jQuery('#IsForAllOlympic').attr('checked'),
	        IsProfileSubject: jQuery('#IsProfileSubject').attr('checked'),
            MinEgeValue: jQuery('#minEge').val(),
	        attachedOlympic: [],
            BenefitItemSubjects: allSubjects
	    };
	    return model;
	}

	jQuery('#btnCancelAB').click(function () { closeDialog(jQuery('#divAddBenefit')) })
	jQuery('#btnSubmitAB').click(function () {
	    var model = prepareModel();
		model.OlympicLevelFlags = 0
		if(jQuery('#cbOlympic1').attr('checked')) model.OlympicLevelFlags |= 1
		if(jQuery('#cbOlympic2').attr('checked')) model.OlympicLevelFlags |= 2
		if(jQuery('#cbOlympic3').attr('checked')) model.OlympicLevelFlags |= 4
		if(jQuery('#cbOlympic0').attr('checked')) model.OlympicLevelFlags = <%= AddBenefitViewModelC.OLYMPIC_ALL %>

		jQuery('#tableOlympic tr[olympicID]').each(function() { model.attachedOlympic.push(jQuery(this).attr('olympicID')) })

		var isError = false
		jQuery('#divErrorBlockOl').html()
		clearValidationErrors(jQuery('#divErrorBlockOl').parent())
		if(model.OlympicLevelFlags == 0)
		{
			jQuery('#tbOlympic').addClass('input-validation-error')
			jQuery('#divErrorBlockOl').html('<span class="field-validation-error">Необходимо выбрать уровень</span>')
			isError = true
		}
		else
			jQuery('#tbOlympic').removeClass('input-validation-error')
		if(model.DiplomaTypeID == 0)
		{
			jQuery('#tbDiplomaType').addClass('input-validation-error')
			jQuery('#divErrorBlockOl').html('<span class="field-validation-error">Необходимо выбрать тип диплома</span>')
			isError = true
		}
		else
			jQuery('#tbDiplomaType').removeClass('input-validation-error')
		if(isError)
			return false
		doPostAjax("<%= Url.Generate<BenefitController>(x => x.SaveBenefitItem(null)) %>", JSON.stringify(model), function (data)
		{
			if(!addValidationErrorsFromServerResponse(data))
			{
				createdBenefitItem = data.Data
				jQuery('#btnCancelAB').click()
			}
		})
		return false
	})

	function doOlympicChange()
	{
		if(jQuery('#cbOlympic0').attr('checked'))
		{
			jQuery('#cbOlympic1,#cbOlympic2,#cbOlympic3').removeAttr('checked').attr('disabled', 'disabled')
		}
		else
			jQuery('#cbOlympic1,#cbOlympic2,#cbOlympic3').removeAttr('disabled').attr('checked', 'checked')
	}

	function cbResetOlympicList() {
		jQuery('#tableOlympic tr[olympicID]').each(function() 
		{
			var olympicID = jQuery(this).attr('olympicID')
			var ol = null
			jQuery.each(allOlympic, function() {if(this.OlympicID == olympicID) {ol = this;return false;}})
			if(ol != null) {
			    if (!canCaddOlympic(ol)) {
			        jQuery(this).remove().detach();
			    }
			}
		})
	}

	function initialFill()
	{
		var olympicFlags = <%= Model.OlympicLevelFlags %>
		if(olympicFlags == <%= AddBenefitViewModelC.OLYMPIC_ALL %>)
		{
			jQuery('#cbOlympic0').attr('checked', 'checked')
			jQuery('#cbOlympic1,#cbOlympic2,#cbOlympic3').removeAttr('checked').attr('disabled', 'disabled')
		}
		else
		{
			if((olympicFlags & 1) != 0)
				jQuery('#cbOlympic1').attr('checked', 'checked')
			if((olympicFlags & 2) != 0)
				jQuery('#cbOlympic2').attr('checked', 'checked')
			if((olympicFlags & 4) != 0)
				jQuery('#cbOlympic3').attr('checked', 'checked')
		}

		for(var i = 0; i < attachedOlympic.length; i++)
			addOlympicRow(jQuery('#trAddNewOlympic'), attachedOlympic[i]);
		cbAllOlympicClick()

        fillSubjects();
	}

	function getStringLevel(lev) {

	    var result = '';
	    var finded = false;

	    if ($.inArray(2, lev) != -1) {
	        finded = true;
	        result = result + 'I'
	    }
	    if ($.inArray(3, lev) != -1) {
            result = result + (finded ? ', ' : '') + 'II'
	        finded = true;
	    }
	    if ($.inArray(4, lev) != -1)
	        result = result + (finded ? ', ' : '') + 'III'
	    return result;
	}

	function addOlympicRow($trBefore, olympicID)
	{
		var ol = null
		jQuery.each(allOlympic, function() {if(this.OlympicID == olympicID) {ol = this;return false;}})
		if(ol != null)
		{
			var className = $trBefore.prev().attr('class')
			if(className == 'trline1') className = 'trline2'; else className = 'trline1';

		    $trBefore.before('<tr olympicID="' + ol.OlympicID + '" class="' + className + '"><td>' +
				ol.Number + '</td><td  align="center">' 
				+ getStringLevel(ol.Level) + '</td>'
		        + '<td>' + ol.Year + '</td>'
				+ '<td>' + escapeHtml(ol.Name) + '</td>'
				+ '<td align="center" nowrap="nowrap"><a href="#" class="btnDelete" onclick="blDeleteButton(this)">&nbsp;</a>' 
				+ '</td></tr>');
		}
	}

	function blDeleteButton(el)
	{
		var $tr = jQuery(el).parents('tr:first');
		$tr.remove().detach()
		jQuery('#trAddNewOlympic').show()
		return false
	}

	function blSaveButton(el)
	{
		var $tr = jQuery(el).parents('tr:first');
		var selVal = jQuery('.blOlympicSelect').val()
		//var olympicID = jQuery('.blOlympicSelect > option:selected').val()
		clearValidationErrors($tr)
		var olympicID = 0
		for(var i = 0; i < ddlOlympicNames.length; i++)
			if(ddlOlympicNames[i] == selVal)
			{
				olympicID = ddlOlympicIDs[i]
			}
		if(olympicID == 0)
		{
			jQuery('.blOlympicSelect').addClass('input-validation-error')
			jQuery('.blOlympicSelect').next().after('<br/><span class="field-validation-error">Необходимо ввести корректное название олимпиады</span>')
			return false
		}
		$tr.remove().detach()
		jQuery('#trAddNewOlympic').show()
		addOlympicRow(jQuery('#trAddNewOlympic'), olympicID)
		return false
	}

	function canCaddOlympic(item) {

	    if(jQuery('#IsProfileSubject').attr('checked') && !item.IsProfileSubject)
			return false
		if(jQuery('#cbOlympic0').attr('checked'))
			return true
		if(jQuery('#cbOlympic1').attr('checked') && $.inArray(2, item.Level) != -1)
			return true
		if(jQuery('#cbOlympic2').attr('checked') && $.inArray(3, item.Level) != -1)
			return true
		if(jQuery('#cbOlympic3').attr('checked') && $.inArray(4, item.Level) != -1)
			return true
		return false
	}

	var ddlOlympicIDs = []
	var ddlOlympicNames = []

	function blAddButton()
	{
		jQuery('#trAddNewOlympic').before('<tr><td colspan="4">' 
				//+ '<select class="blOlympicSelect"></select></td>'
				+ '<input type="text" class="blOlympicSelect"></input></td>'
				+ '<td align="center" class="trUnsaved" nowrap="nowrap"><a href="#" class="btnSave btnSaveBI" onclick="blSaveButton(this)">&nbsp;</a>&nbsp;<a href="#" class="btnDelete" onclick="blDeleteButton(this)">&nbsp;</a>'
				+ '</td></tr>');
		ddlOlympicIDs = []
		ddlOlympicNames = []
		for (var i = 0; i < allOlympic.length; i++)
			if(canCaddOlympic(allOlympic[i]))
			if(jQuery('#tableOlympic tr[olympicID="' + allOlympic[i].OlympicID + '"]').length == 0)
			{
				ddlOlympicIDs.push(allOlympic[i].OlympicID)
				ddlOlympicNames.push(allOlympic[i].Number +'.' + escapeHtml(allOlympic[i].Name))
			}

		if(ddlOlympicIDs.length == 0)
		{
			jQuery('.blOlympicSelect').val('Нет доступных олимпиад')
			jQuery('.blOlympicSelect').attr('disabled', 'disabled')
			jQuery('.btnSaveBI').hide()
		}
		else
		{
			autocompleteDropdown(jQuery('.blOlympicSelect'), {source: function(ui, response) 
			{
				var res = []
				var x = ui.term.toUpperCase()
				for(var i = 0; i < ddlOlympicNames.length; i++)
					if(ddlOlympicNames[i].toUpperCase().indexOf(x) >= 0)
					{
						res.push(ddlOlympicNames[i])
						//if(res.length > 10) break
					}
				response(res)
			}, minLength: 2});
		}
				//jQuery('.blOlympicSelect').append("<option value='" + allOlympic[i].OlympicID + "'>" 
				//	+ allOlympic[i].Number +'.' + escapeHtml(allOlympic[i].Name)  + "</option>")
		//if(jQuery('.blOlympicSelect > option').length == 0)
		//{
		//	jQuery('.blOlympicSelect').append('<option>Нет доступных олимпиад</option>')
		//	jQuery('.blOlympicSelect').attr('disabled', 'disabled')
		//	jQuery('.btnSaveBI').hide()
		//}
		jQuery('#trAddNewOlympic').hide()
	}

	function cbAllOlympicClick()
	{
		if(jQuery('#IsForAllOlympic').attr('checked'))
		{
			isOlympicExists	= jQuery('#tableOlympic tr[olympicID]').length
			if(isOlympicExists)
			{
				if(confirm('Введенные олимпиады будут удалены. Продолжить?'))
				{
					jQuery('#tableOlympic tr[olympicID]').remove().detach()
					jQuery('#tableOlympic').hide()
				}
				else
					jQuery('#IsForAllOlympic').removeAttr('checked')
			}
			else
				jQuery('#tableOlympic').hide()
		}
		else
			jQuery('#tableOlympic').show()
	}

	function addMultipleOlympic()
	{
		var res = '<div style="padding: 0px;overflow:hidden"></div>'
		var isAny = false
		for (var i = 0; i < allOlympic.length; i++)
			if(canCaddOlympic(allOlympic[i]))
			{
				if(jQuery('#tableOlympic tr[olympicID="' + allOlympic[i].OlympicID + '"]').length == 0)
				{
					res += ('<div style="padding: 3px;"><input type="checkbox" olympicID="'+ allOlympic[i].OlympicID 
						+ '" id="cbSelectOlympic' + allOlympic[i].OlympicID  + '">'
						+ '<label for="cbSelectOlympic' +allOlympic[i].OlympicID +'">'
						+ allOlympic[i].Number +'.' + escapeHtml(allOlympic[i].Name)
						+ '</label></div>')
					isAny = true
				}
			}
		if(!isAny) res += 'Все доступные олимпиады выбраны'
		jQuery('#divAddOlympicMultiple').html(res)
		jQuery('#divAddOlympicMultiple').dialog(
		{
			resizeable: false,
			title: 'Выберите олимпиады',
			width: 600,
			buttons: {
				"Выбрать": function() 
				{
					jQuery('#divAddOlympicMultiple input[type="checkbox"]:checked').each(function()
					{
						var olID = jQuery(this).attr('olympicID')
						addOlympicRow(jQuery('#trAddNewOlympic'), olID)
					})
					
					//jQuery(this).dialog('close')
					closeDialog(jQuery('#divAddOlympicMultiple'))
					jQuery('#divAddOlympicMultiple').children().remove().detach()
					//closeDialog(jQuery('.dialogSelectOl'))
				},
				"Отмена": function() {closeDialog(jQuery('#divAddOlympicMultiple'));jQuery('#divAddOlympicMultiple').children().remove().detach();}
			},
			modal: true
		})
		return false
	}
	
	function doOlympicYearChange() {
        doPostAjax('<%= Url.Generate<BenefitController>(x => x.GetOlympicData(null)) %>', JSON.stringify(prepareModel()), function(data) {
			if (!addValidationErrorsFromServerResponse(data, false)) {
			    allOlympic = data.Data;
			}
		});

		jQuery('#tableOlympic tr[olympicID]').each(function() 
		{
			var olympicID = jQuery(this).attr('olympicID')
			var ol = null
			jQuery.each(allOlympic, function() {if(this.OlympicID == olympicID) {ol = this;return false;}})
			if(ol != null) {
			    jQuery(this).remove().detach();
			}
		})

		var $tr = jQuery('.trUnsaved').parents('tr:first');
		$tr.remove().detach()
		jQuery('#trAddNewOlympic').show()
	}

    function fillSubjects()
    {
        var fakedRow = jQuery('.fakeRow');
        for (var i = 0; i < allSubjects.length; i++)
        {
            fakedRow.before(
                '<tr itemId="' + allSubjects[i].Id + '"><td subjectId="' + allSubjects[i].SubjectId + '">' + allSubjects[i].SubjectName + '</td><td class="egemin">' + allSubjects[i].EgeMinValue.toString() + '</td>' +
                '<td align="center"><a href="#" class="btnEdit" onclick="subjectEditButton(this)"><img src="<%= Url.Images("edit_16.gif") %>" alt="edit" /></a>&nbsp;<a href="#" class="btnDelete"><img src="<%= Url.Images("delete_16.gif") %>" alt="delete" onclick="subjectDeleteButton(this)" /></a></td></tr>'
                );
        }
        fakedRow.remove().detach();
    }

    function subjectEditButton(row)
    {
        var jRow = jQuery(row).parents('tr:first');
        var itemId = jRow.attr('itemid');
        var currentSubjectId = jRow.children('td[subjectid]').attr('subjectid');
        
        var minEge = jRow.children('.egemin').text();
        var options = '';

        var btns = '<td align="center" class="trUnsaved" nowrap="nowrap"><a href="#" class="btnSave btnSaveSubject" onclick="subjectSaveButton(this)">&nbsp;</a>&nbsp;<a href="#" class="btnDelete" onclick="subjectCancelButton(this)">&nbsp;</a></td>'

        for (var i = 0; i < allAvailiableSubjects.length; i++)
            if (allAvailiableSubjects[i].Id == currentSubjectId)
                options += '<option value="' + allAvailiableSubjects[i].Id + '" selected="selected">' + allAvailiableSubjects[i].Name + '</option>';
            else options += '<option value="' + allAvailiableSubjects[i].Id + '">' + allAvailiableSubjects[i].Name + '</option>';

        var rowToAdd = '<tr itemid="' + itemId + '"><td><select id="subjectSelect"><options>' + options + '</options></select></td><td><input type="text" id="subjectMinEge" value="' + minEge + '"/></td>' + btns + '</tr>';
        jRow.before(rowToAdd);
        jRow.remove().detach();
    }

    function subjectDeleteButton(row)
    {
        var jRow = jQuery(row).parents('tr:first');
        allSubjects.pop(jRow.attr('itemid'));
        jRow.remove().detach();

        return false;
    }

    function subjectAddButton()
    {
        var addRow = jQuery('#trAddSubject');
        var options = '';
        var btns = '<td align="center" class="trUnsaved" nowrap="nowrap"><a href="#" class="btnSave btnSaveSubject" onclick="subjectSaveButton(this)">&nbsp;</a>&nbsp;<a href="#" class="btnDelete" onclick="subjectCancelButton(this)">&nbsp;</a></td>'

        for (var i = 0; i < allAvailiableSubjects.length; i++)
            options += '<option value="' + allAvailiableSubjects[i].Id + '">' + allAvailiableSubjects[i].Name + '</option>';

        var rowToAdd = '<tr><td><select id="subjectSelect"><options>' + options + '</options></select></td><td><input type="text" id="subjectMinEge"/></td>' + btns + '</tr>';

        addRow.before(rowToAdd);
        addRow.hide();
    }

    function subjectCancelButton(el)
    {
        var jRow = jQuery(el).parents('tr:first');
        var itemId = jRow.attr('itemid');

        if (itemId == null)
        {
            jRow.remove().detach();
            var addRow = jQuery('#trAddSubject');
            addRow.show();
        }
        else{
            addRowWithItemId(itemId, jRow);
            jRow.remove().detach();
        }

        return false;
    }

    function subjectSaveButton(el)
    {
        var jRow = jQuery(el).parents('tr:first');
        var itemid = jRow.attr('itemid');

        var subjectSelector = jQuery('#subjectSelect');
        var subjectId = subjectSelector.val();
        var subjectName = subjectSelector.children('option[value=' + subjectId + ']').text();
        var minEgeValue = jQuery('#subjectMinEge').val();

        var addedSubject;

        if (itemid == null){
            jRow.before(
                    '<tr itemId="-1"><td subjectId="' + subjectId + '">' + subjectName + '</td><td class="egemin">' + minEgeValue.toString() + '</td>'  +
                    '<td align="center"><a href="#" class="btnEdit" onclick="subjectEditButton(this)"><img src="<%= Url.Images("edit_16.gif") %>" alt="edit" /></a>&nbsp;<a href="#" class="btnDelete"><img src="<%= Url.Images("delete_16.gif") %>" alt="delete" onclick="subjectDeleteButton(this)" /></a></td></tr>'
                    );

            jRow.remove().detach();
            jQuery('#trAddSubject').show();
            addedSubject = {
                Id: -1,
                SubjectId: subjectId,
                SubjectName: subjectName,
                EgeMinValue: minEgeValue
            }

            allSubjects.push(addedSubject);
        }
        else{
            jRow.before(
                    '<tr itemId="' + itemid + '"><td subjectId="' + subjectId + '">' + subjectName + '</td><td class="egemin">' + minEgeValue.toString() + '</td>'  +
                    '<td align="center"><a href="#" class="btnEdit" onclick="subjectEditButton(this)"><img src="<%= Url.Images("edit_16.gif") %>" alt="edit" /></a>&nbsp;<a href="#" class="btnDelete"><img src="<%= Url.Images("delete_16.gif") %>" alt="delete" onclick="subjectDeleteButton(this)" /></a></td></tr>'
                    );

            for (var i = 0; i < allSubjects.length; i++)
            {
                if (allSubjects[i].Id == itemid)
                {
                    addedSubject =allSubjects[i];
                    break;
                }
            }

            if (addedSubject != null)
            {
                addedSubject.SubjectId = subjectId;
                addedSubject.SubjectName = subjectName;
                addedSubject.EgeMinValue = minEgeValue;
            }
            else{
                addedSubject = {
                    Id: -1,
                    SubjectId: subjectId,
                    SubjectName: subjectName,
                    EgeMinValue: minEgeValue
                }
                allSubjects.push(addedSubject);
            }
            jRow.remove().detach();
        }
    }

    function addRowWithItemId(itemId, jRow)
    {
        var subject = null;
        for (var i = 0; i < allSubjects.length; i++)
        {
            if (allSubjects[i].Id == itemId)
            {
                subject = allSubjects[i];
                break;
            }
        }

        if (subject == null) return false;

        jRow.before(
            '<tr itemId="' + subject.Id + '"><td subjectId="' + subject.SubjectId + '">' + subject.SubjectName + '</td><td class="egemin">' + subject.EgeMinValue.toString() + '</td>' +
            '<td align="center"><a href="#" class="btnEdit" onclick="subjectEditButton(this)"><img src="<%= Url.Images("edit_16.gif") %>" alt="edit" /></a>&nbsp;<a href="#" class="btnDelete"><img src="<%= Url.Images("delete_16.gif") %>" alt="delete" onclick="subjectDeleteButton(this)" /></a></td></tr>'
        );
    }

	jQuery(document).ready(function() { initialFill() })
</script>


