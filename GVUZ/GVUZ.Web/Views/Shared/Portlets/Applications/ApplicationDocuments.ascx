<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.Portlets.Applications.ApplicationEntrantDocumentsViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Model.Entrants" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Register TagName="EntrantLanguageEdit" TagPrefix="gv" Src="~/Views/Shared/Portlets/Entrants/EntrantLanguageEdit.ascx" %>
<%@ Register TagName="EntrantDocumentAddPart" TagPrefix="gv" Src="~/Views/Shared/Controls/EntrantDocumentAddPart.ascx" %>
<%@ Register TagName="ApplicationWizardButtons" TagPrefix="gv" Src="~/Views/Shared/Portlets/Applications/ApplicationWizardButtons.ascx" %>
	
<gv:ApplicationWizardButtons ID="ApplicationWizardButtons1" runat="server" ApplicationStep="Documents" IsTop="true" />
<% if(Model.ShowDenyMessage) { %> <div>Невозможно редактировать данное заявление</div>
  <script type="text/javascript"> 	function doSubmit() { return false; }</script>  
<%} else { %>
	<div id="documentAddDialog"></div>
	<div id="content" style="margin-bottom: 5px;">
	<div id="divErrorBlock"></div>
	<h4>Документы, прикреплённые к заявлению</h4>
		<table class="gvuzDataGrid" cellpadding="3" id="docGrid1">
			<thead>
				<tr>
					<th><%= Html.LabelFor(x => x.BaseDocument.DocumentTypeName) %></th>
					<th><%= Html.LabelFor(x => x.BaseDocument.DocumentSeriesNumber)%></th>
					<th><%= Html.LabelFor(x => x.BaseDocument.DocumentDate) %></th>
					<th><%= Html.LabelFor(x => x.BaseDocument.DocumentOrganization) %></th>
					<th><%= Html.LabelFor(x => x.BaseDocument.DocumentAttachmentID) %></th>
					<th><%= Html.LabelFor(x => x.BaseDocument.OriginalReceivedDate) %></th>
					<th><%= Html.LabelFor(x => x.BaseDocument.OriginalReceived) %></th>
					<th style="width: 80px"></th>
				</tr>
			</thead>
			<tbody>
				<tr id="trAddNew1" style="display:none"></tr>
			</tbody>
		</table>
		<div style="margin-top: 5px; margin-bottom: 5px">
			<input type="button" id="btnAddNewDocument" value="Прикрепить новый документ" onclick="beginAddDocument()" />
		</div>
		<h4>Существующие документы</h4>
		<table class="gvuzDataGrid" cellpadding="3" id="docGrid2">
			<thead>
				<tr><th><%= Html.LabelFor(x => x.BaseDocument.DocumentTypeName) %></th>
					<th><%= Html.LabelFor(x => x.BaseDocument.DocumentSeriesNumber)%></th>
					<th><%= Html.LabelFor(x => x.BaseDocument.DocumentDate) %></th>
					<th><%= Html.LabelFor(x => x.BaseDocument.DocumentOrganization) %></th>
					<th><%= Html.LabelFor(x => x.BaseDocument.DocumentAttachmentID) %></th>
					<th style="width: 80px">	</th>
				</tr>
			</thead>
			<tbody>
				<tr id="trAddNew2" style="display:none"></tr>
			</tbody>
		</table>
	</div>
	<div style="display: none">
		<div class="divOriginalDateDialog">
			<table>
				<tr>
					<td>Дата предоставления:</td>
					<td><input type="text" class="datePicker" id="tbDateOriginalDialog"/></td>
				</tr>
			</table>
		</div>
	</div>
	<script type="text/javascript">

        //<%-- Существующие данные с сервера --%>
		var existingItems = JSON.parse('<%= Html.Serialize(Model.ExistingDocuments) %>')
		var attachedItems = JSON.parse('<%= Html.Serialize(Model.AttachedDocuments) %>')
		var getFileLink = '<%= Url.Generate<EntrantController>(x => x.GetFile1(null)) %>'
		var getFileSep = '<%= Url.Generate<EntrantController>(x => x.GetFile1(null)) %>'.indexOf('?') >= 0 ? '&' : '?'
		var applicationID = <%= Model.ApplicationID %>

        //<%-- добавление нового документа --%>
		var addNewDocument = function ($trBefore, item, isDetach){
			var className = $trBefore.prev().attr('class')
			if(className == 'trline1') className = 'trline2'; else className = 'trline1';
		    //<%-- Нельзя изменять - вначале покажем варнинг --%>
			var mod = item.CanBeModified
						? '<a href="#" title="Редактировать документ" class="btnEdit" onclick="doEditDocumentWarn(this, ' + (item.ShowWarnBeforeModifying ? 1 : 0) + ');return false;"></a>'
						: '<span title="Невозможно редактировать документ (уже используется)" class="btnEditGray"></span>'
			var res = '<tr itemID="' + item.EntrantDocumentID + '" class="' + className + '"><td><span class="btnView linkSumulator" onclick="btnViewClick(this)">' + escapeHtml(item.DocumentTypeName)
				+ '</a></td><td>' + escapeHtml(item.DocumentSeriesNumber) + '</td><td>'
				+ item.DocumentDate + '</td><td> '
				+ escapeHtml(item.DocumentOrganization == null ? '' : item.DocumentOrganization) + '</td><td>' +
					(item.DocumentAttachmentID != '<%= Guid.Empty %>' ? '<a href="<%= Url.Generate<EntrantController>(x => x.GetFile1(null)) %>' + getFileSep + 'fileID=' + item.DocumentAttachmentID + '">' + item.DocumentAttachmentName + '</a>' : 'Отсутствует')
				+ '</td>';
		    //<%-- если детачен - без кнопочек, инчае рисуем кнопки установки даты оригиналов --%>
			if(isDetach) {
				if(item.CanNotSetReceived)
					res += '<td> </td><td> </td>';
				else
					res += '<td>' + escapeHtml(item.OriginalReceivedDate == null ? 'Не предоставлены' : item.OriginalReceivedDate) + '</td>'
						+'<td><input type="checkbox" ' + (item.OriginalReceived ? 'checked="checked"' : "") 
							+ ' onclick="doDateReceivedDialog(this, ' + item.EntrantDocumentID + ', \'' + escapeHtml(item.OriginalReceivedDate != null ? item.OriginalReceivedDate : '') + '\')" /></td>'
			}
		    //<%-- Если приложен - кнопочки убрать --%>
			res += '<td align="center">'
					+ (isDetach 
					? (item.CanBeDetached 
						? '<a href="#" title="Открепить документ от заявления" class="btnDown" onclick="btnDownClick(this);return false;"></a>&nbsp;'
						: '<span title="Документ невозможно открепить (уже используется в другом месте)" class="btnDownGray"></span>&nbsp;')
					: '<a href="#" title="Прикрепить докумет к заявлению" class="btnUp" onclick="btnUpClick(this);return false;"></a>&nbsp;')
					+ mod + '</td></tr>';
			$trBefore.before(res);
		}

	    //<%-- Инициализируем дейтпикеры --%>
		function setDatePicker1()
		{
			jQuery(".divOriginalDateDialog .datePicker").datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-5:+0', maxDate: new Date() });
		}

		jQuery(document).ready(function () {
		    var i;
		    //<%-- Заполняем документы с сервера --%>
			for (i = 0; i < existingItems.length; i++)
				addNewDocument(jQuery('#trAddNew2'), existingItems[i], 0)
			for (i = 0; i < attachedItems.length; i++)
				addNewDocument(jQuery('#trAddNew1'), attachedItems[i], 1)
			setTimeout(setDatePicker1, 0);
		})

	    //<%-- Открываем диалог просмотра документа --%>
		var doView = function (navUrl, postData)
		{
			doPostAjax(navUrl, postData, function (data)
			{
				jQuery('#documentAddDialog').html(data)
				jQuery('a.getFileLink').each(function () { jQuery(this).attr('href', getFileLink + getFileSep + 'fileID=' + jQuery(this).attr('fileID')) })
				jQuery('#documentAddDialog').dialog({
					modal: true,
					width: 800,
					title: jQuery('#dialogCaption').html(),
					buttons: {
						"Закрыть": function () { jQuery(this).dialog('close'); }
					},
					close: function() {}
				}).dialog('open');
			}, "application/x-www-form-urlencoded", "html")
		}

	function btnViewClick(el)
	{
		var $tr = jQuery(el).parents('tr:first')
		var itemID = $tr.attr('itemID')
		doView('<%= Url.Generate<EntrantController>(x => x.ViewDocument(0)) %>', 'entrantDocumentID=' + itemID)
		return false
	}

    //<%-- аттачим документ --%>
	function btnUpClick(el)
	{
		var $tr = jQuery(el).parents('tr:first')
		var itemID = $tr.attr('itemID')
		for (var i = 0; i < existingItems.length; i++)
			if (existingItems[i] != null && existingItems[i].EntrantDocumentID == itemID)
			{
				attachedItems.push(existingItems[i])
				$tr.remove().detach()
				addNewDocument(jQuery('#trAddNew1'), existingItems[i], 1)
				existingItems[i] = null
				break
			}
	    //<%-- сразу сохраняем данные --%>
		doSubmit('norefresh')
	}

    //<%-- детачим документ --%>
	function btnDownClick(el)
	{
		var $tr = jQuery(el).parents('tr:first')
		var itemID = $tr.attr('itemID')
		for (var i = 0; i < attachedItems.length; i++)
			if (attachedItems[i] != null && attachedItems[i].EntrantDocumentID == itemID)
			{
				existingItems.push(attachedItems[i])
				$tr.remove().detach()
				addNewDocument(jQuery('#trAddNew2'), attachedItems[i], 0)
				attachedItems[i] = null
				break
			}
		doSubmit('norefresh')
	}

    //<%-- сохраняем данные --%>
	function doSubmit(cmd)
	{
	    clearValidationErrors(jQuery('#divErrorBlock'));
	    //<%-- набираем данные  --%>
		var model = {
			ApplicationID: <%= Model.ApplicationID %>, 
			EntrantID: <%= Model.EntrantID %>,
			StepDirection: cmd,
			AttachedDocumentIDs: []
		}
		jQuery('#docGrid1 tr[itemID]').each(function() 
		{
			model.AttachedDocumentIDs.push(jQuery(this).attr('itemID'))
		})
		doPostAjax("<%= Url.Generate<ApplicationController>(x => x.SaveApplicationDocuments(null)) %>", 'model=' + JSON.stringify(model), function (data)
		{
			if(data.IsError)
				jQuery('#divErrorBlock').append('<span class="field-validation-error">' + data.Message.replace(/\n/g, '<br/>') + '</span>')
			
			//после изменения данных, прозрачно сохраняем, но не уходим
			if(cmd == 'refresh') {
				window.location.reload(1);
				return false;
			}
			if(cmd == 'norefresh') {
				return false;
			}
		    //<%-- если есть метод, значит мы в визарде и вызываем его. Визард сам решит, куда идти дальше --%>
			if(!data.IsError)
			{
				if(typeof doAppNavigate != "undefined")
					doAppNavigate()
				else
					jQuery('#btnCancel').click()
			}
			else
			{
				if(<%= Model.ApplicationStatus == ApplicationStatusType.Draft ? "true" : "false" %>)
				if(cmd == 'save' || cmd == 'back')
					if(typeof doAppNavigate != "undefined")
					{
						confirmDialog('Данные не будут сохранены. Вы действительно хотите уйти с данной страницы?', function() {doAppNavigate()})
					}
			}
		}, "application/x-www-form-urlencoded", null)
		return false
	}

	    <%-- add document part begin --%>
	
	    //<%-- функции добавления документов  --%>
	    var documentTypeLists = JSON.parse('<%= Html.Serialize(Model.DocumentTypes) %>')
	    var entrantID = <%= Model.EntrantID %>
	    
        //<%-- создан/отредактирован новый документ --%>
	function onNewDocumentCreated(doc, el)
	{
		if(el != null)
		{
		    //<%-- находим место, куда добавлять и добавляем. старую строчку удаляем  --%>
			var $tr = jQuery(el).parents('tr:first')
			addNewDocument($tr, doc, $tr.parents('table#docGrid1').length == 1)
			$tr.remove().detach()
		}
		else
		{
		    //<%-- добавляем последней строкой  --%>
			attachedItems.push(doc)
			addNewDocument(jQuery('#trAddNew1'), doc, 1)
		}
		doSubmit('refresh')
	}
	<%-- add document part end --%>

	jQuery(document).ready(function() {
	    jQuery('#btnAddNewDocument').button() //<%-- стили  --%>
	});
	
	    //<%-- диалог установки даты оригиналов  --%>
	function doDateReceivedDialog(el, docID, docDate) {
		var isChecked = jQuery(el).is(':checked');
		if(isChecked) jQuery(el).removeAttr('checked');
		else jQuery(el).attr('checked', 'checked');
		if(!isChecked) {
			saveDateReceived(docID, null, false);
			return;
		}
		if(docDate)
			jQuery('#tbDateOriginalDialog').val(docDate)
		else {
			jQuery('#tbDateOriginalDialog').val('<%= DateTime.Today.ToString("dd.MM.yyyy") %>');
		}
		jQuery(".divOriginalDateDialog").dialog(
			{
				modal: true,
				width: 550,
				title: "Дата предоставления оригинала документа / Заявление с обязательством предоставления оригинала в течение первого учебного года",
				buttons:{
					"Сохранить": function () { saveDateReceived(docID, jQuery('#tbDateOriginalDialog').val(), true);closeDialog(jQuery(this));  },
					"Отмена": function () { closeDialog(jQuery(this)); }
				}
			}).dialog('open')
	}
	
	    //<%-- сохраняем дату оригиналов  --%>
	function saveDateReceived(docID, date, isChecked) {
		doPostAjax('<%= Url.Generate<ApplicationController>(x => x.SetDocumentOriginalReceived(null, null, null, null)) %>',
			'applicationID=' + applicationID + '&entrantDocumentID=' + docID + '&received=' + (isChecked ? 'true' : 'false') + '&receivedDate=' + date,
			function(data) {
				if(data.IsError) alert(data.Message);
				else {
					for (var i = 0; i < attachedItems.length; i++)
						if (attachedItems[i] != null && attachedItems[i].EntrantDocumentID == docID){
							attachedItems[i].OriginalReceived = isChecked;
							attachedItems[i].OriginalReceivedDate = isChecked ? (date ? date : '<%= DateTime.Today.ToString("dd.MM.yyyy") %>') : null;
						  var $tr = jQuery('#docGrid1 tr[itemID="' + docID + '"]');
						    //<%-- обновляем строчку  --%>
							addNewDocument($tr, attachedItems[i], 1)
							$tr.remove().detach()
							break
						}
				}
			}, "application/x-www-form-urlencoded");
	}

	    //<%-- предупреждение  --%>
	function doEditDocumentWarn(el, showModifiedWarn) {
	  //<%-- метод в другой EditDocumentAddPart  --%>
    if(!showModifiedWarn){ return doEditDocument(el);
    } else {
            confirmDialog('Данный документ используется в другом заявлении. Если вы измените документ, то он также изменится и в другом заявлении. Продолжить?', function () {  doEditDocument(el);});
    }
  }
	</script>
<gv:EntrantDocumentAddPart runat="server" />
<% } %>
<gv:ApplicationWizardButtons runat="server" ApplicationStep="Documents" />
