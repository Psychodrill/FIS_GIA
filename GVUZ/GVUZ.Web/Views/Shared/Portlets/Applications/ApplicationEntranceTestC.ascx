<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.Portlets.Applications.ApplicationEntranceTestViewModelC>" %>
<%@ Import Namespace="System.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Model.Entrants" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Register TagName="ApplicationWizardButtons" TagPrefix="gv" Src="~/Views/Shared/Portlets/Applications/ApplicationWizardButtons.ascx" %>
<%@ Register TagName="EntrantDocumentAddPart" TagPrefix="gv" Src="~/Views/Shared/Controls/EntrantDocumentAddPart.ascx" %>
<gv:ApplicationWizardButtons ID="ApplicationWizardButtons2" runat="server" ApplicationStep="EntranceTest"  IsTop="true" />

<% if(Model.ShowDenyMessage) { %>
<div>Невозможно редактировать данное заявление</div>
<script type="text/javascript">  function doSubmit() { return false; }</script>
<%} else { %>
<div id="documentAddDialog"></div>
<div id="content">
  <% foreach(var cgID in Model.EntranceTests.OrderBy(x => x.SubjectName).Select(x => x.CompetitiveGroupID).Distinct()) { %>
  <div class="statementtitle">  Конкурс: <span class="statementsubtitle"><%: Model.EntranceTests.First(x => x.CompetitiveGroupID == cgID).CompetitiveGroupName %></span></div>
  

	<div id="divManualETValue">
    <table class="gvuzData">
      <tr>
<%--            <td class="caption"><%= Html.TableLabelReqFor(x => x.EntranceTests.StudyBeginningDate) %>:</td>
            <td colspan="1"> <%= Html.DatePickerFor(m => m.CompetitiveGroupEdit.StudyBeginningDate) %></td>--%>
		          <td class="caption"><%= Html.TableLabelFor(x => x.DescrInstitutionDocument.DocumentTypeID)%></td>
        <td> <%= Html.DropDownListExFor(x => x.DescrInstitutionDocument.DocumentTypeID, Model.InstitutionDocumentTypes, null)%> </td>
		          <td class="caption"><%= Html.TableLabelFor(x => x.DescrInstitutionDocument.DocumentTypeID)%></td>
        <td> <%= Html.DropDownListExFor(x => x.DescrInstitutionDocument.DocumentTypeID, Model.InstitutionDocumentTypes, null)%> </td>
      </tr>

    </table>
  </div>
	<div class="divGeneralBenefits" style="padding-bottom: 10px">
    <span style="font-size: 12pt; font-weight: 500;" id="commonBenefitsB">Общие льготы:</span>&nbsp;&nbsp;&nbsp;
    <% var globalDocs=Model.GlobalDocs.Where(x => x.CompetitiveGroupID==cgID).ToArray();
       if(globalDocs.Count()>0) {
         foreach(var globalDoc in globalDocs) { %>
    <p benefit="1">
      <% if(globalDoc.BenefitErrorMessage!=null) {%><span class="btnError" style="height: 27px" title="<%: globalDoc.BenefitErrorMessage %>">&nbsp;</span><%} %>
      <span><span style="font-size: 12pt">
        <%= globalDoc.BenefitID == 1 ? "Без вступительных испытаний" : (globalDoc.BenefitID == 4 ? "По квоте приёма лиц, имеющих особое право" : "Преимущественное право на поступление")%>
        </span>
        (<a href="#" onclick="addNoExETValue(this, <%=cgID %>);return false;"><%= globalDoc.DocumentDescription ?? "Документ не указан" %></a>)
      </span><a href="#" class="btnDelete" onclick="detachETDocument(<%= globalDoc.ID %>);return false;" title="Удалить результаты">&nbsp;</a>
    </p>
    <%}

    if(Model.Course==1) { %><br />
      <a href="#" onclick="addNoExETValue(this, <%=cgID %>);return false;">Без вступительных испытаний</a>&nbsp;&nbsp;&nbsp;
    <% } %>
      <a id="quotaRef" href="#" onclick="addDisETValue(this, <%=cgID %>);return false;">По квоте приёма лиц, имеющих особое право</a> &nbsp;&nbsp;&nbsp;
      <a               href="#" onclick="addPrefETValue(this, <%=cgID %>);return false;">Преимущественное право на поступление</a>
    <%} else {%>
    <% if(Model.Course==1) { %>
      <a href="#" onclick="addNoExETValue(this, <%=cgID %>);return false;">Без вступительных испытаний</a>&nbsp;&nbsp;&nbsp;
    <% } %>
      <a id="quotaRef" href="#" onclick="addDisETValue(this, <%=cgID %>);return false;"> По квоте приёма лиц, имеющих особое право</a> &nbsp;&nbsp;&nbsp;
      <a               href="#" onclick="addPrefETValue(this, <%=cgID %>);return false;">Преимущественное право на поступление</a>
    <%} %>
  </div>
  <div class="statementborder">
    <table class="gvuzDataGrid docGrid1" cellpadding="3">
      <thead>
        <tr>
          <th style="width: 20%"><%= Html.LabelFor(x => x.DescrTestData.SubjectName)%></th>
          <th><%= Html.LabelFor(x => x.DescrTestData.Priority) %></th>
          <th style="width: 80px" nowrap="nowrap" id="thResultValue"><%= Html.LabelFor(x => x.DescrAttachedData.ResultValue)%></th>
          <th><%= Html.LabelFor(x => x.DescrAttachedData.EgeResultValue)%></th>
          <th><%= Html.LabelFor(x => x.DescrAttachedData.SourceID)%></th>
        </tr>
      </thead>
      <tbody>
        <% if(Model.EntranceTests.Length==0) { %>
        <tr>
          <td colspan="3" align="center">
            Документы не требуются
          </td>
        </tr>
        <% } %>
        <%
           var idx=0;
           var prevETType=EntranceTestType.MainType;
           foreach(var rs in Model.EntranceTests.Where(x => x.CompetitiveGroupID==cgID).OrderBy(x => x.EntranceTestType).ThenBy(x => x.SubjectName).ToArray()) {
             if(rs.EntranceTestType!=prevETType) {
               idx++;
               prevETType=rs.EntranceTestType;%>
              <tr class="<%= idx % 2 == 0 ? "trline2" : "trline1" %>">
                <td colspan="3">
                <b>
                  <%= rs.EntranceTestType == EntranceTestType.CreativeType ? "Дополнительные вступительные испытания творческой и (или) профессиональной направленности" : ""%>
                  <%= rs.EntranceTestType == EntranceTestType.ProfileType ? "Дополнительные вступительные испытания профильной направленности" : ""%>
                  <%= rs.EntranceTestType == 1 ? "Аттестационные испытания" : ""%>
                </b>
                </td>
              </tr>
         <%  }
           idx++;
      var docData=Model.AttachedDocs.FirstOrDefault(x => x.EntranceTestItemID==rs.EntranceTestItemID); %>
        <tr class="<%= idx % 2 == 0 ? "trline2" : "trline1" %>" itemid="<%= rs.EntranceTestItemID %>">
          <td><%: rs.SubjectName %><% if(rs.IsProfileSubject) { %><br /><span style="font-size: 7pt">(профильный предмет)</span><%} %>
          </td>
          <td><%= rs.Priority == null ? "Без приоритета" : rs.Priority.ToString() %></td>
          <td nowrap="nowrap">
            <input type="text" class="numeric 
              <%= rs.EntranceTestType == EntranceTestType.MainType && rs.IsEgeSubject && (docData == null || (docData.SourceID == 1 && docData.EntrantDocumentID == 0)) ? "" : "view" %>"
              <%= rs.EntranceTestType == EntranceTestType.MainType && rs.IsEgeSubject && (docData == null || (docData.SourceID == 1 && docData.EntrantDocumentID == 0)) ? "" : "readonly=\"readonly\"" %>
              style="width: 70px" value="<%= docData == null ? "" : docData.ResultValue.ToString("0.####") %>"
              maxlength="3" onchange="doManualInput(this)" isege="<%=rs.EntranceTestType == EntranceTestType.MainType ? "1" : "0" %>"
              onfocus="doManualInputStart(this)" onblur="doManualInputEnd(this)" />
          </td>
          <% if(docData!=null && docData.BenefitID==3) { %>
          <td nowrap="nowrap">
            <input type="text" class="numeric" style="width: 70px" value="<%= docData == null ? "" : docData.EgeResultValue.ToString("0.####") %>"
              maxlength="3" onchange="doSaveEgeValueForBenefit(this, '<%=docData.ID %>')" isege="<%=rs.EntranceTestType == EntranceTestType.MainType ? "1" : "0" %>"
              onfocus="doManualInputStart(this)" onblur="doManualInputEnd(this)" />
          </td>
          <% } else {%>
            <td>   </td>
          <% } %>
          <td>
            <% if(docData!=null) { %>
              <span docdescr="1">
              <%= docData.DocumentDescription %></span> <a href="#" class="btnDelete" onclick="detachETDocument(<%= docData.ID %>);return false;" title="Удалить результаты" docdelid="<%= docData.ID %>">&nbsp;</a>
            <% } else {%>
            <% if(rs.EntranceTestType==EntranceTestType.MainType&&rs.IsEgeSubject&&rs.CompetitiveGroupEduLevelCode!="70") { %>
              <a href="#" style="padding-right: 15px;" onclick="addEGEETValue(this);return false;">Свидетельство ЕГЭ</a>
            <% } else { %>
                <span style="color: #606060; display: none;">Свидетельство ЕГЭ</span>
            <%} %>
            <% if(rs.EntranceTestType==EntranceTestType.MainType&&rs.SubjectID>0&&rs.CompetitiveGroupEduLevelCode!="70") { %>
                <a href="#" style="padding-right: 15px;" onclick="addGIAETValue(this);return false;">Справка ГИА</a><% } else { %>
                <span style="color: #606060; display: none;">Справка ГИА</span>
            <%} %>
            <% if(rs.EntranceTestType!=EntranceTestType.MainType||true) { %>
              <a href="#" id="ET_Ref" style="padding-right: 15px;" onclick="addManualETValue(this);return false;">Вступительное испытание ОО</a>
            <% } else { %>
              <span style="color: #606060; display: none;">Вступительное испытание ОО</span>
            <%} %>
            <% if(rs.HasBenefits||(rs.EntranceTestType!=EntranceTestType.CreativeType&&rs.SubjectID>0&&rs.CompetitiveGroupEduLevelCode!="70")) { %>
              <a href="#" style="padding-right: 15px;" onclick="addDiplomaETValue(this, <%= cgID %>, <%= rs.SubjectID %>);return false;">Диплом победителя/призера олимпиады</a>
            <% } else { %>
              <span style="color: #606060; display: none;" title="Для данной дисцпилины отсутствуют льготы">Диплом победителя/призера олимпиады</span>
            <%} %>

            <%} %>
          </td>
        </tr>
        <% } %>
      </tbody>
    </table>
  </div>
  <% } %>
</div>
<% if(Model.Course<2) { %>
<div id="divEgeCheck" style="margin-top: 10px">
  <input type="button" class="button3" style="width: auto" id="btnEgeCheck" value="Получить/проверить результаты ЕГЭ"   onmousedown="btnEgeCheck()" /></div>
<%} %>
<div style="display: none">
  <div id="divManualETValue">
    <table class="gvuzData">
      <tr>
        <td class="caption"><%= Html.TableLabelFor(x => x.DescrInstitutionDocument.DocumentTypeID)%></td>
        <td> <%= Html.DropDownListExFor(x => x.DescrInstitutionDocument.DocumentTypeID, Model.InstitutionDocumentTypes, null)%> </td>
      </tr>
      <tr>
        <td class="caption"><%= Html.TableLabelFor(x => x.DescrInstitutionDocument.DocumentNumber)%></td>
        <td><%= Html.TextBoxExFor(x => x.DescrInstitutionDocument.DocumentNumber) %></td>
      </tr>
      <tr>
        <td class="caption"><%= Html.TableLabelFor(x => x.DescrInstitutionDocument.DocumentDate)%></td>
        <td><%= Html.DatePickerFor(x => x.DescrInstitutionDocument.DocumentDate) %></td>
      </tr>
      <tr>
        <td class="caption"><%= Html.TableLabelFor(x => x.DescrInstitutionDocument.Value) %></td>
        <td><%= Html.TextBoxExFor(x => x.DescrInstitutionDocument.Value, new {maxLength="8"}) %> </td>
      </tr>
    </table>
  </div>
  <div id="divAddETDocument">
    <div id="divCurrentDocumentPart">
      <b>Выбранный документ:</b>
      <div id="divCurrentDocumentTypes" style="padding-left: 20px; padding-top: 5px; padding-bottom: 15px"></div>
    </div>
    <div id="divAddDocumentPart">
      <b>Добавить документ:</b>
      <div id="divAddDocumentTypes" style="padding-left: 20px; padding-top: 5px; padding-bottom: 15px"></div>
    </div>
    <div id="divExistingDocumentPart">
      <b>Выбрать существующий документ:</b>
      <div id="divExistingDocumentTypes" style="padding-left: 20px; padding-top: 5px; padding-bottom: 15px"></div>
    </div>
  </div>
  <div id="divAddETDocumentFromManual">
    <div style="padding-bottom: 10px">
      <b>У данного абитуриента есть сертификат ЕГЭ текущего года, содержащий оценку по дисциплине<span id="spDocumentTypeSubjectForManual"></span></b>
    </div>
    <div>
      <b>Выбрать существующий документ:</b>
      <div id="divExistingDocumentTypesForManual" style="padding-left: 20px; padding-top: 5px; padding-bottom: 15px"></div>
    </div>
    <div style="padding-left: 20px; padding-top: 5px; padding-bottom: 15px">
      <span class="linkSumulator" id="spAddManualETFromEG">Использовать результат внутреннего вступительного испытания</span>
    </div>
  </div>
</div>
<script language="javascript" type="text/javascript">
	var doRefreshOnSubmit = false

	function doSubmit(){
		if(typeof doAppNavigate != "undefined")
			doAppNavigate()
		else
			jQuery('#btnCancel').click()
		return false	
	}
	
	var queryStack = []

	var selectedBenefitID = 0
	var selectedCompetitiveGroupID = 0;
	//var breakReload = false
	function doSaveData(etID, sourceID, docID, resultValue, idocTID, idocDate, idocNumber){
		var model = {
			ApplicationID: <%= Model.ApplicationID %>,
			SourceID: sourceID,
			BenefitID: selectedBenefitID,
			EntrantDocumentID: docID,
			ResultValueString: resultValue.toFixed(4),
			EntranceTestItemID: etID,
			CompetitiveGroupID: selectedCompetitiveGroupID,
			InstitutionDocumentTypeID: idocTID,
			InstitutionDocumentDate: idocDate,
			InstitutionDocumentNumber: idocNumber
		}

		queryStack.push(function () {
			doPostAjax("<%= Url.Generate<ApplicationController>(x => x.SaveApplicationEntranceTestSelectedData(null)) %>", JSON.stringify(model), function (data){
                if (data.Data != null && data.Data.length > 0) // Ошибки с баллами ЕГЭ
                {
                    jQuery('span[benefitError="1"]').remove().detach();
                    jQuery("#input-validation-error").removeClass('input-validation-error');

                    for (var i = 0; i < data.Data.length; i++){
                        jQuery("tr[itemID = '" + data.Data[i].ItemId.toString() + "'] > td > input[type='text']").addClass('input-validation-error');
                        jQuery("tr[itemID = '" + data.Data[i].ItemId.toString() + "'] > td > span[docDescr='1']").before('<span class="field-validation-error" benefitError="1">' + data.Data[i].Message + '</span><br/>');
                    }
                    queryStack.shift()

                    if (data.Extra != null){
                        var htmlString = '<p benefit="1">';
                        if (data.Extra.BenefitErrorMessage != ''){
                            htmlString += '<span class="btnError" style="height:27px" title="' + data.Extra.BenefitErrorMessage + '">&nbsp;</span>';
                         }

                        htmlString += '<span> <span style="font-size:12pt">' + (data.Extra.BenefitId== 1 ? 'Без вступительных испытаний' : (data.Extra.BenefitID == 4 ? 'По квоте приёма лиц, имеющих особое право' : 'Преимущественное право на поступление')) + '</span>';
                        htmlString += '(<a href="#" onclick="addNoExETValue(this, ' + data.Extra.CompetitiveGroupID + ' );return false;">' + (data.Extra.DocumentDescription == '' ? "Документ не указан" : data.Extra.DocumentDescription) + '</a>) </span><a href="#" class="btnDelete" onclick="detachETDocument(' + data.Extra.ID + ');return false;" title="Удалить результаты">&nbsp;</a>';
                        htmlString += '</p>';

                        var elem = jQuery('p[benefit="1"]:last');
                        if (elem.length == 0){        elem = jQuery('#commonBenefitsB');}
                        elem.after(htmlString);
                    }
                } else {
				    if(!addValidationErrorsFromServerResponse(data)){
					    if(queryStack.length == 0) window.location.reload(1)
					    else processQueryStack()
				    }else processQueryStack()
          }
			});
		});
		processQueryStackIfEmpty();
	}

	var getFileLink = '<%= Url.Generate<EntrantController>(x => x.GetFile1(null)) %>'
	var getFileSep = '<%= Url.Generate<EntrantController>(x => x.GetFile1(null)) %>'.indexOf('?') >= 0 ? '&' : '?'

	var doView = function (navUrl, postData){
		createdItem = null
		doPostAjax(navUrl, postData, function (data){
			jQuery('#documentAddDialog').html(data)
			jQuery('a.getFileLink').each(function () { jQuery(this).attr('href', getFileLink + getFileSep + 'fileID=' + jQuery(this).attr('fileID')) })
			jQuery('#documentAddDialog').dialog({
				modal: true,
				width: 600,
				title: jQuery('#dialogCaption').html(),
				buttons: {
					"Закрыть": function () { jQuery(this).dialog('close'); }
				}
			}).dialog('open');
		}, "application/x-www-form-urlencoded", "html")
	}

	function btnViewClick(el){
		doView('<%= Url.Generate<EntrantController>(x => x.ViewDocument(0)) %>', 'entrantDocumentID=' + jQuery(el).parents('tr:first').attr('itemID'))
		return false
	}
	<%-- add document part begin --%>
<%--	var documentTypeLists = JSON.parse('<%= Html.Serialize(Model.DocumentTypes) %>')--%>
	var applicationID = <%= Model.ApplicationID %>
	var entrantID = <%= Model.EntrantID %>

	function onNewDocumentCreated(doc){
		doRefreshOnSubmit = true
		doSaveData(addingDocumentETID, selectedSourceDoc, doc.EntrantDocumentID, 0)
	}
	<%-- add document part end --%>

	jQuery(document).ready(function() {
		var egeData = getCookie('ege_res')
		if(typeof egeData != "undefined"){
			setCookie('ege_res', '', -1)
			fillEgeResults(JSON.parse(egeData))
		}
		jQuery("#DescrInstitutionDocument_DocumentDate").datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-10:+0', maxDate: new Date() });
	});

	function detachETDocument(docID){
		queryStack.push(function () {
			doPostAjax("<%= Url.Generate<ApplicationController>(x => x.DeleteApplicationEntranceTestSelectedData(null)) %>", 'docID=' + docID, function(data) {
				if (!addValidationErrorsFromServerResponse(data)) {
					if (queryStack.length == 0) window.location.reload(1)
					else processQueryStack()
				}
			}, "application/x-www-form-urlencoded", null)
		})
		processQueryStackIfEmpty()
	}

	function addManualETValue(el){
	  var course = <%= Model.Course %>;
		var etID = jQuery(el).parents('tr:first').attr('itemID');
		doPostAjax("<%= Url.Generate<ApplicationController>(x => x.GetAbilityToEnterManualValue(null, null)) %>",	'applicationID=<%= Model.ApplicationID %>&entranceTestItemID=' + etID, function(data) {
				if (!addValidationErrorsFromServerResponse(data)) {
					if(data.Data.ShouldCheckEgeBefore && course == 1) {
						confirmDialog('Перед вводом результатов вступительного испытания ОО необходимо проверить наличие свидетельств ЕГЭ у абитуриента. Проверить сейчас?', 
						    function () {
						        setCookie('call_ET', data.Data.CanAddManualValue ? '1': data.Data, null);
    				        btnEgeCheck(el, etID);
						    });
					} else {
						if(data.Data.CanAddManualValue)
							addManualETValueDialogInternal(el);
						else 
							addManualETDocumentAsEgeInternal(data.Data, etID, el);
					}
				}
		}, "application/x-www-form-urlencoded", null);
	}
	
	function addManualETValueDialogInternal(el){
		var etID = jQuery(el).parents('tr:first').attr('itemID')
		var currentValue = jQuery(el).parents('tr:first').find('input[type="text"]').val()
		jQuery('#DescrInstitutionDocument_Value').val(currentValue)
		jQuery('#DescrInstitutionDocument_Value,#DescrInstitutionDocument_DocumentDate,#DescrInstitutionDocument_Number').removeClass('input-validation-error').removeClass('input-validation-error-fixed')
		jQuery('#divManualETValue').dialog({
			modal: true,
			width: 600,
			title: 'Ввод результатов вступительных испытаний ОО',
			buttons: {
				"Сохранить": function() {
					clearValidationErrors(jQuery('#DescrInstitutionDocument_Value').parents('table:first'))
					var unparsedValue = jQuery('#DescrInstitutionDocument_Value').val().replace(',', '.')
					var bValue = parseFloat(unparsedValue)
					var isError = false;
					if(isNaN(bValue) || bValue < 0 || bValue > 100){
						addValidationError(jQuery('#DescrInstitutionDocument_Value'), 'Балл должен быть числом от 0 до 100', true)
						isError = true;
					}
					if(!jQuery('#DescrInstitutionDocument_DocumentDate').val()) {
						addValidationError(jQuery('#DescrInstitutionDocument_DocumentDate'), 'Дата документа обязательна', true)
						isError = true
					}
					if(!jQuery('#DescrInstitutionDocument_DocumentNumber').val()) {
						addValidationError(jQuery('#DescrInstitutionDocument_DocumentNumber'), 'Номер документа обязателен', true)
						isError = true
					}
					if(isError){			return;}
					doSaveData(etID, 2, null, bValue, jQuery('#DescrInstitutionDocument_DocumentTypeID').val(),
						jQuery('#DescrInstitutionDocument_DocumentDate').val(),
						jQuery('#DescrInstitutionDocument_DocumentNumber').val())
					jQuery(this).dialog('close')
				},
				"Отмена": function () { jQuery(this).dialog('close'); }
			}
		}).dialog('open');
	}
	
	function addManualETDocumentAsEgeInternal(docData, etID, el){
    selectedSourceDoc = 1;//ege
		jQuery('#divExistingDocumentPart').show();
    if (docData.SubjectName != null){	  jQuery('#spDocumentTypeSubjectForManual').text(docData.SubjectName);}
		jQuery("#spAddManualETFromEG")[0].onclick = function () { addManualETValueDialogInternal(el);}
    if (docData.DocExisting != null){
		  fillETDocuments(jQuery('#divExistingDocumentTypesForManual'), docData.DocExisting, etID, null, null);
    }
		jQuery('#divAddETDocumentFromManual').dialog({	modal: true,	width: 600,	title: 'Выбор документа'}).dialog('open');
	}

	var addingDocumentETID;

	function doETDocumentAdd(docTypeID, etID, cgID, subjectId){
		addingDocumentETID = etID
		doAddDocument(docTypeID, cgID, subjectId);
		closeDialog(jQuery('#divAddETDocument'));
	}

	function doETDocumentSelect(docID, etID){
		doSaveData(etID, selectedSourceDoc, docID, 0);
		closeDialog(jQuery('#divAddETDocument'));
	}

	function fillETDocuments($div, docList, etID, cgID, subjectId) {
		var res = ''

		for(var i = 0; i < docList.length;i++){
			if(docList[i].DenyMessage != null){
				res += '<span class="linkSumulatorGray" title="' + docList[i].DenyMessage + '">' + docList[i].Description + '</span><br/>'
			}else{
				if(docList[i].DocumentID > 0)
					res += '<span class="linkSumulator" onclick="doETDocumentSelect(' + docList[i].DocumentID + ', ' + etID + ')">' + docList[i].Description + '</span><br/>'
				else
					res += '<span class="linkSumulator" onclick="doETDocumentAdd(' +   docList[i].TypeID + ', ' + etID + ',' + cgID + ',' + subjectId + ')">' + docList[i].Description + '</span><br/>'
			}
		}
		$div.html(res)
	}

	function fillEgeResults(data) {
		if(data.length > 0 && data[0].EntranceTestItemID == 0) {
			alert(data[0].Result);
        var qres = getCookie('call_ET');
        if ((qres != null) && (qres != '') && (qres != 'undefined')){                   
           var el=document.getElementById('ET_Ref')                   
           if(qres == '1'){
              addManualETValueDialogInternal(el);
           } else {
              var etID = $(el).parents('tr:first').attr('itemID');                      
              addManualETDocumentAsEgeInternal(qres, etID, el);
           }                  
        }
    }
              
		for(var i = 0; i < data.length;i++) {
		  var $tr = jQuery('.docGrid1 tr[itemID="' + data[i].EntranceTestItemID + '"]');
			var error = data[i].Result;

			if(error == null || error == ''){
				$tr.find('input[type="text"]').after('<span class="egeCheckIcon">&nbsp;<span class="btnOk" style="height:27px">&nbsp;</span></span>')
			}else{
				$tr.find('input[type="text"]').after('<span class="egeCheckIcon">&nbsp;<span class="btnError" style="height:27px" title="' + error + '">&nbsp;</span></span>')
      }
		}
    setCookie('call_ET', '', null)
	}

	function btnEgeCheck() {
	    btnEgeCheck(null, null);
	}

	function btnEgeCheck(el, etID) {
  
//		setTimeout(function () {
//		    queryStack.push(function() {
				doPostAjax("<%= Url.Generate<ApplicationController>(x => x.CheckApplicationEGEResults(null, null)) %>", 
				    'applicationID=<%=Model.ApplicationID %>&etID=' + etID, function(data) {
					if (!addValidationErrorsFromServerResponse(data)) {
						jQuery('.docGrid1 .egeCheckIcon').remove().detach()
                        /* Если мы нашли именно то что искали - проставляем */
                        if (data.Data.length > 0 && (etID == null || etIDExists(data.Data, etID))) {
					        setCookie('ege_res', JSON.stringify(data.Data), 1)
					        window.location.reload(1)
                            return
                        /* Если не нашли - предлагаем ручной ввод */
					    } else if (el != null) {
					        addManualETValueDialogInternal(el);
					    }
					    return
					}
					window.breakReload = false
				}, "application/x-www-form-urlencoded", null)
			}
//          ) processQueryStackIfEmpty()
//		}, 0)
//	}

	function etIDExists(data, etID) {
	    if (etID == null) return true;
	    if (data.length == 0) return false;
	    for (var i=0; i < data.length; i++) {
	        if (data[i].EntranceTestItemID == etID)
	            return true;
	    }
	    return false;
	}

	function addPrefETValue(el, cgID)
	{
		selectedBenefitID = 5
		selectedCompetitiveGroupID = cgID;
		addDocETValue(el, 105, cgID)
	}


	function addDisETValue(el, cgID)
	{
		selectedBenefitID = 4
		selectedCompetitiveGroupID = cgID;
		addDocETValue(el, 104, cgID)
	}

	function addNoExETValue(el, cgID) {
	    selectedBenefitID = 1
		selectedCompetitiveGroupID = cgID;
		addDocETValue(el, 102, cgID)
	}

	function addEGEETValue(el)
	{
		addDocETValue(el, 1)
	}
	
	function addGIAETValue(el)
	{
		addDocETValue(el, 4)
	}

	function addDiplomaETValue(el, cgID, subjectId) {
		addDocETValue(el, 3, cgID, subjectId)
	}

	var selectedSourceDoc = false

	function addDocETValue(el, egeSource, cgID, subjectId)
	{
		selectedSourceDoc = egeSource;
		var etID = jQuery(el).parents('tr:first').attr('itemID')
		if(typeof etID == "undefined") etID = 0
		var cgPart = ''
		if(etID == 0) cgPart = '&groupID=' + selectedCompetitiveGroupID

		doPostAjax("<%= Url.Generate<ApplicationController>(x => x.GetAllowedDocumentsForEntranceTest(null, null, null, null)) %>",
				'applicationID=<%= Model.ApplicationID %>&docSourceID=' + egeSource + '&entranceTestItemID=' + etID + cgPart, function (data) {
			if(!addValidationErrorsFromServerResponse(data))
			{
				var docData = data.Data
				var hasAnyDoc = false;
				if(docData.DocAdd == null || docData.DocAdd.length == 0)
					jQuery('#divAddDocumentPart').hide()
				else
				{
					hasAnyDoc = true;
					jQuery('#divAddDocumentPart').show()
					fillETDocuments(jQuery('#divAddDocumentTypes'), docData.DocAdd, etID, cgID, subjectId)
				}
				if(docData.DocExisting == null || docData.DocExisting.length == 0)
					jQuery('#divExistingDocumentPart').hide()
				else
				{
					hasAnyDoc = true;
					jQuery('#divExistingDocumentPart').show()
					fillETDocuments(jQuery('#divExistingDocumentTypes'), docData.DocExisting, etID, cgID, subjectId)
				}
				if(docData.DocCurrent == null || docData.DocCurrent.length == 0)
					jQuery('#divCurrentDocumentPart').hide()
				else
				{
					hasAnyDoc = true;
					jQuery('#divCurrentDocumentPart').show()
					fillETDocuments(jQuery('#divCurrentDocumentTypes'), docData.DocCurrent, etID, cgID, subjectId)
				}
				if(selectedSourceDoc == 1 && (docData.DocExisting == null || docData.DocExisting.length == 0))
				{
					doETDocumentAdd(2, etID, cgID, subjectId)
				}
				else if(selectedSourceDoc == 4 && (docData.DocExisting == null || docData.DocExisting.length == 0))
				{
				    doETDocumentAdd(17, etID, cgID, subjectId)
				}
				else
				{
					if(!hasAnyDoc) {
						infoDialog('Отсутствуют подходящие документы для данного типа результата вступительных испытаний');
					} else {
						jQuery('#divAddETDocument').dialog({
							modal: true,
							width: 600,
							title: 'Выбор документа'
						}).dialog('open');
					}

				}
			}
		}, "application/x-www-form-urlencoded", null)
	}

	function doManualInput(el)
	{
		clearValidationErrors(jQuery(el).parent())
		var etID = jQuery(el).parents('tr:first').attr('itemID')
		jQuery(el).removeClass('input-validation-error').removeClass('input-validation-error-fixed')

		if(jQuery(el).attr('readonly'))
			return
		if(jQuery(el).val() == '')
		{
			var $btnDelete = jQuery(el).parents('tr:first').find('.btnDelete')
			if($btnDelete.length > 0)
			{
				detachETDocument($btnDelete.attr('docDelID'))
			}
			return
		}
		var isEGE = jQuery(el).attr('isEGE')
		var bValue = new Number(jQuery(el).val())
		if(isNaN(bValue) || bValue < 1 || bValue > 100 || bValue != Math.floor(bValue))
		{
			addValidationError(jQuery(el), 'Балл должен быть целым числом от 1 до 100', true)
			setTimeout(function() {jQuery(el).focus()}, 0)
			
			return
		}
		doSaveData(etID, isEGE != 0 ? 1 : 2, null, bValue)
	}
	
	function doManualInputStart(el) {
		//jQuery('#btnEgeCheck').attr('disabled', 'disabled')
	}
	
	function doManualInputEnd(el) {
		//jQuery('#btnEgeCheck').removeAttr('disabled')
	}
	
	function processQueryStack() {
		queryStack.shift()
		if(queryStack.length == 0) window.location.reload(1);
		else {
			var func = queryStack[0]
			func()
		}
	}
	function processQueryStackIfEmpty() {
		(function () {
			if (queryStack.length != 1) return;
			var func = queryStack[0]
			func()
		})()
	}

    // Сохранение значения ЕГЭ для предмета, по которому заявлен диплом победителя/призёра олимпиады для проверки на необходимый минимальный балл
    function doSaveEgeValueForBenefit(el, docId)
    {
		clearValidationErrors(jQuery(el).parent())
		var etID = jQuery(el).parents('tr:first').attr('itemID')
		jQuery(el).removeClass('input-validation-error').removeClass('input-validation-error-fixed')

		if(jQuery(el).attr('readonly'))
			return

		var isEGE = jQuery(el).attr('isEGE')
		var bValue = new Number(jQuery(el).val())
		if(isNaN(bValue) || bValue < 1 || bValue > 100 || bValue != Math.floor(bValue))
		{
			addValidationError(jQuery(el), 'Балл должен быть целым числом от 1 до 100', true)
			setTimeout(function() {jQuery(el).focus()}, 0)
			
			return
		}

        var model = {
            documentId : docId,
            egeResult: bValue
            };

        doPostAjax("<%=Url.Generate<ApplicationController>(x => x.SaveEgeForOlympics(null, null)) %>", JSON.stringify(model),
            function(data) {
                if (data.Message != null)
                {
                    alert(data.Message);
                    return false;
                }

                if (data.Data != null && data.Data.Value != null && data.Data.Value != undefined)
                {
                    var docTd = jQuery('tr[itemId="' + data.Data.Value + '"]').children('td:last');
                    var html = docTd.html();

                    html += '<br/> <span class="field-validation-error" bebefitError="1"> Для использования льготы минимальный балл по предмету должен быть не меньше Х баллов</span>';
                    docTd.html(html);

                    jQuery("tr[itemID = '" + data.Data.Value.toString() + "'] > td > input[type='text']").addClass('input-validation-error');
                }
                else window.location.reload(1);
            });
    }

    jQuery(document).ready(function() {
        if ('<%=Model.IsQuotaBenefitEnabled %>' == 'True')
        {
            jQuery('#quotaRef').show();
            jQuery('#quotaRef').removeClass('disabled');
        }
        else 
        {
            jQuery('#quotaRef').hide();
            jQuery('#quotaRef').addClass('disabled');
        }
    });

</script>
<gv:EntrantDocumentAddPart ID="EntrantDocumentAddPart1" runat="server" />
<% } %>
<gv:ApplicationWizardButtons ID="ApplicationWizardButtons1" runat="server" ApplicationStep="EntranceTest" />
