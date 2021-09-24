<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.ApplicationWz2ViewModel>" %>
<%@ Import Namespace="System.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Model.Entrants" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Register TagName="EntrantDocumentJS" TagPrefix="ed" Src="~/Views/Shared/Entrant/EntrantDocumentJS.ascx" %>

<% if(Model.ShowDenyMessage) { %> <div>Невозможно редактировать данное заявление</div>
  <script type="text/javascript">    function doSubmit() { return false; }</script>  
<%} else { %>
	<div id="content" style="margin-bottom: 5px;">
	<div id="divErrorBlock"></div>
	<div id="documentAddDialog"></div>
<!--    <div id="Wz2UniDDialog" style="display:none"></div> -->
	<div id="Div1" style="margin-bottom: 5px;">
	<div id="div2"></div>
	<div class="subHeader">Документы, прикреплённые к заявлению</div>
		<table class="gvuzDataGrid" cellpadding="3" id="docGrid1">
			<thead>
				<tr>
					<th><%= Html.LabelFor(x => x.DocListInfo.DocumentTypeName)%></th>
					<th><%= Html.LabelFor(x => x.DocListInfo.DocumentSeriesNumber)%></th>
					<th><%= Html.LabelFor(x => x.DocListInfo.DocumentDate)%></th>
					<th><%= Html.LabelFor(x => x.DocListInfo.DocumentOrganization)%></th>
					<th><%= Html.LabelFor(x => x.DocListInfo.DocumentAttachmentID)%></th>
					<th><%= Html.LabelFor(x => x.DocListInfo.OriginalReceivedDate)%></th>
					<th><%= Html.LabelFor(x => x.DocListInfo.OriginalReceived)%></th>
					<th style="width: 100px"></th>
				</tr>
			</thead>
			<tbody>
				<tr id="trAddNew1" style="display:none"></tr>
			</tbody>
		</table>
		<div style="margin-top: 5px; margin-bottom: 5px">
			<input type="button" id="btnAddNewDocument" value="Прикрепить новый документ" onclick="NewDocument(1)" />
		</div>
		<div class="subHeader">Существующие документы</div>
		<table class="gvuzDataGrid tableStatement2" cellpadding="3" id="docGrid2">
			<thead>
				<tr><th><%= Html.LabelFor(x => x.DocListInfo.DocumentTypeName)%></th>
					<th><%= Html.LabelFor(x => x.DocListInfo.DocumentSeriesNumber)%></th>
					<th><%= Html.LabelFor(x => x.DocListInfo.DocumentDate)%></th>
					<th><%= Html.LabelFor(x => x.DocListInfo.DocumentOrganization)%></th>
					<th><%= Html.LabelFor(x => x.DocListInfo.DocumentAttachmentID)%></th>
					<th style="width: 100px">	</th>
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
    </div>
<script language="javascript" type="text/javascript">
	//<%-- функции добавления документов  --%>
  var documentTypeLists = JSON.parse('<%= Html.Serialize(Model.DocumentTypes) %>');
  var EntrantID = <%= Model.EntrantID %>;

  var getFileLink = '<%= Url.Generate<EntrantController>(x => x.GetFile1(null)) %>';
  var getFileSep = '<%= Url.Generate<EntrantController>(x => x.GetFile1(null)) %>'.indexOf('?') >= 0 ? '&' : '?';

  //-- Открываем диалог просмотра документа --
  function ViewDocument(EntrantDocumentID) {
    var $Dialog = null;
    $Dialog = $('#Wz2UniDDialog');
    if ($Dialog.length == 0) {
      $Dialog = $('<div id="Wz2UniDDialog" style="display:none, position:fixed"></div>');
      $('body').append($Dialog);
    }
    doPostAjax('<%= Url.Generate<EntrantController>(x => x.getViewDocument(null)) %>', { EntrantDocumentID: EntrantDocumentID, DocTypeID: 1 }, function (data) {
      $Dialog.html(data);
      $Dialog.dialog({
        modal: true,
        width: 800,
        title: "Дополнительный документ",
        buttons: {  "Закрыть": function () { $(this).dialog('close');  }}
        , close: function () { 
            $Dialog.remove();
        }
      }).dialog('open');
    }, "application/x-www-form-urlencoded", "html");
  }

  function Wz2Init() {

      jQuery(".divOriginalDateDialog .datePicker").datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-5:+0', maxDate: new Date() });
      jQuery('#btnAddNewDocument').button();

  //<%-- Существующие данные с сервера --%>
    var existingItems = JSON.parse('<%= Html.Serialize(Model.ExistingDocuments) %>');
    var attachedItems = JSON.parse('<%= Html.Serialize(Model.AttachedDocuments) %>');

      var i;
      //-- Заполняем документы с сервера --
      for (i = 0; i < existingItems.length; i++) { addNewDocument(jQuery('#trAddNew2'), existingItems[i], 0); }
      for (i = 0; i < attachedItems.length; i++) { addNewDocument(jQuery('#trAddNew1'), attachedItems[i], 1); }
      //setTimeout(setDatePicker1, 0);


      var StatusId=<%=Model.StatusID %>;
      if (StatusId == 4 || StatusId == 6) {
          $(".discol").hide();
          $('#btnAddNewDocument').setDisabled();
      }
  }

  function Wz2Save(success, error, step) {
      var model = {};
      if (step == undefined) {step = 3; }

      var cheksPage = 0;
      if (window.ShowWz != undefined) { cheksPage=1 }
      if (window.loadPaView != undefined) { cheksPage=2 }

      doPostAjaxSync('<%= Url.Generate<ApplicationController>(x => x.Wz2Save(0,0,0)) %>', JSON.stringify({ ApplicationID: ApplicationId, Step: step, cheksPage: cheksPage }),
        function (data) {
          if (!data.IsError) { 
            if (success) { 
              success(model); 
            } 
          } else { 
            if (error) { 
              error(data.Message); 
            }
          }
      });
  }

  function Wz2Cancel() {    }
</script>

<script type="text/javascript">
   //<%-- добавление нового документа --%>
    function addNewDocument($trBefore, item, isDetach) {

        if ($trBefore == null) {
            if (isDetach == 1) {
                $trBefore = $('#trAddNew1');
            } else {
                $trBefore = $('#trAddNew2');
            }
        }
        //item.CanBeModified = true; // ToDo

        var className = $trBefore.prev().attr('class')
        if (className == 'trline1') className = 'trline2';
        else className = 'trline1';
        //<%-- Нельзя изменять - вначале покажем варнинг --%>
        var mod = item.CanBeModified
            ? '<a href="#" title="Редактировать документ" class="btnEdit discol" onclick="doEditDocumentWarn(' + item.EntrantDocumentID + ', ' + (item.ShowWarnBeforeModifying ? 1 : 0) + ');return false;"></a>'
            : '<span title="Невозможно редактировать документ (уже используется)" class="btnEditGray"></span>';
        var deleteButton = item.CanBeDeleted
            ? '<a href="#" title="Удалить документ" class="btnDelete discol" onclick="doDeleteDocument(this)"></a>'
            : '<span title="Невозможно удалить документ (уже используется)" class="btnDeleteGray"></span>';
        var res = '<tr itemID="' + item.EntrantDocumentID + '" class="' + className + '">'
            + '<td><a class="linkSumulator" onclick="ViewDocument(' + item.EntrantDocumentID + ')">' + escapeHtml(item.DocumentTypeName) + '</a></td>'
            + '<td>' + escapeHtml(item.DocumentSeriesNumber) + '</td>'
            + '<td>' + (item.DocumentDate != null ? item.DocumentDate : '') + '</td>'
            + '<td>' + escapeHtml(item.DocumentOrganization == null ? '' : item.DocumentOrganization) + '</td>'
            + '<td>' + (item.AttachmentFileID != null ? '<a href="<%= Url.Generate<EntrantController>(x => x.GetFile1(null)) %>' + getFileSep + 'fileID=' + item.AttachmentFileID + '">' + item.AttachmentName + '</a>' : 'Отсутствует') + '</td>';
        //<%-- если детачен - без кнопочек, инчае рисуем кнопки установки даты оригиналов --%>
        if (isDetach) {
            if (item.CanNotSetReceived) {
                res += '<td></td><td></td>';
            } else {
                res += '<td class="OriginalReceivedDate">' + escapeHtml(item.OriginalReceivedDate == null ? 'Не предоставлены' : item.OriginalReceivedDate) + '</td>'
                    + '<td><input type="checkbox" ' + (item.OriginalReceived ? 'checked="checked"' : "")
                    + ' onclick="doDateReceivedDialog(this, ' + item.EntrantDocumentID + ', \'' + escapeHtml(item.OriginalReceivedDate != null ? item.OriginalReceivedDate : '') + '\')" /></td>';
            }
        }
        //<%-- Если приложен - кнопочки убрать --%>
        res += '<td align="center">'
            + (isDetach
                ? (item.CanBeDetached
                    ? '<a href="#" title="Открепить документ от заявления" class="btnDown discol" onclick="btnDownClick(this);return false;"></a>&nbsp;'
                    : '<span title="Документ невозможно открепить (уже используется в другом месте)" class="btnDownGray discol"></span>&nbsp;')
                : '<a href="#" title="Прикрепить докумет к заявлению" class="btnUp discol" onclick="btnUpClick(this);return false;"></a>&nbsp;')
            + mod + (!isDetach ? ('&nbsp'+ deleteButton) : '') + '</td></tr>';
        $trBefore.before(res);
    }

    //-- аттачим документ --
	function btnUpClick(el) {
	    var $tr = $(el).parents('tr:first');
	    var itemID = $tr.attr('itemID');
    // Новая логика тут
		doPostAjaxSync("<%= Url.Generate<ApplicationController>(x => x.AttachDocument(0,0)) %>", JSON.stringify({ApplicationID:ApplicationId, EntrantDocumentID:itemID}), function (data){
			if(data.IsError){    infoDialog(data.Message.replace(/\n/g, '<br/>'));   return;    }
			$tr.remove();      // Удаляем с из общего списка
			addNewDocument($('#trAddNew1'), data.Data, 1);// Добавить к заявке
    });
	}

  //-- детачим документ --
	function btnDownClick(el){
		var $tr = $(el).parents('tr:first');
		var itemID = $tr.attr('itemID');
		doPostAjax("<%= Url.Generate<ApplicationController>(x => x.DetachDocument(0,0)) %>", JSON.stringify({ApplicationID:ApplicationId, EntrantDocumentID:itemID}), function (data){
			if(data.IsError){   infoDialog(data.Message.replace(/\n/g, '<br/>'));   return;   }
			$tr.remove();      // Удаляем с из заявки
			addNewDocument($('#trAddNew2'), data.Data, 0);      // Добавить к общему списку
    });
	}

    //-- удаление --
    function doDeleteDocument (el){ 
        var $tr = $(el).parents('tr:first');
		var itemID = $tr.attr('itemID');
		doPostAjax("<%= Url.Generate<ApplicationController>(x => x.DeleteDocument(0,0)) %>", JSON.stringify({ApplicationID:ApplicationId, EntrantDocumentID:itemID}), function (data){
			if(data.IsError){   infoDialog(data.Message.replace(/\n/g, '<br/>'));   return;   }
			$tr.remove();     
        });
    }
 </script>

<script type="text/javascript">
  //<%-- диалог установки даты оригиналов  --%>
	function doDateReceivedDialog(el, docID, docDate) {
		var isChecked = $(el).is(':checked');
		if(isChecked) $(el).removeAttr('checked');
		else $(el).attr('checked', 'checked');
		if(!isChecked) {
			saveDateReceived(docID, null, false);
			return;
		}
		if(docDate){		$('#tbDateOriginalDialog').val(docDate)	} else {	$('#tbDateOriginalDialog').val('<%= DateTime.Today.ToString("dd.MM.yyyy") %>');		}

	    jQuery(".divOriginalDateDialog").dialog({
	        modal: true,
	        width: 550,
	        title: "Дата предоставления оригинала документа / Заявление с обязательством предоставления оригинала в течение первого учебного года",
	        buttons: {
	            "Сохранить": function() {
	                saveDateReceived(docID, jQuery('#tbDateOriginalDialog').val(), true);
	                closeDialog(jQuery(this));
	            },
	            "Отмена": function() { closeDialog(jQuery(this)); }
	        }
	    }).dialog('open');
	}
    //<%-- сохраняем дату оригиналов  --%>
	function saveDateReceived(docID, date, isChecked) {
		doPostAjax('<%= Url.Generate<ApplicationController>(x => x.SetDocumentOriginalReceived(null, null, null, null)) %>',
			'applicationID=' + ApplicationId + '&entrantDocumentID=' + docID + '&received=' + (isChecked ? 'true' : 'false') + '&receivedDate=' + date,
			function(data) {
				if(data.IsError){ infoDialog(data.Message);
        }	else {
				 var $tr = $('#docGrid1 tr[itemID="' + docID + '"]');
         $tr.find('input[type=checkbox]').attr('checked', data.Data.OriginalReceived);
         $tr.find('.OriginalReceivedDate').text(data.Data.OriginalReceivedDate);
				}
			}, "application/x-www-form-urlencoded");
	}
</script>
<script language="javascript" type="text/javascript">
    function NewDocument(page) {
        var style = "menuitemr";
        var style1 = style; var style2 = style; var style3 = style
        if (page == 1) {
            type = 3;  //Документ об образовании
            style1 = style + " select";
        }
        else if (page == 2) {
            type = 4;  //Документ, подтверждающий льготу
            style2 = style + " select";
        }
        else if (page == 3) {
            type = 0;  //Любой другой документ
            style3 = style + " select";
        }
        // Диалог выбора типа документа
        // Тип документа передать в форме редактирования.    
        var res = '';
        for (var i = 0; i < documentTypeLists.length; i++) {
            if ((documentTypeLists[i].CategoryId == type && type == 3) 
                || (type == 4 && (documentTypeLists[i].CategoryId == 4 || documentTypeLists[i].CategoryId == 7)) 
                || (type == 0 && documentTypeLists[i].CategoryId != 3 && documentTypeLists[i].CategoryId != 4 && documentTypeLists[i].CategoryId != 7)
                ) {
                res += '<div style="padding: 3px; text-align: left"><a href="" onclick="NewDocumentByType(' + documentTypeLists[i].TypeID + '); return false;">' + documentTypeLists[i].Name + '</a></div>';
            }
        }
        res = '<div style="padding-bottom: 50px;"><a href="" onclick="NewDocument(1)" class="' + style1 + '">Документы об образовании</a><a href="" onclick="NewDocument(2)" class="' + style2 + '">Документы, подтверждающие льготы</a><a href="" onclick="NewDocument(3)" class="' + style3 + '">Прочие документы</a></div>' + res;
        var $di = $('#SelDocType');
        if ($di.length == 0) {
            $di = $(
                '<div id="SelDocType" class="dialogSelect">' 
                + res + 
                '</div>').dialog({ resizeable: false, title: 'Выберите тип документа', width: 900, modal: true }
             );
        }
        $di.html(res).dialog({ resizeable: false, title: 'Выберите тип документа', width: 900, modal: true });;
        return false;
    }

    function NewDocumentByType(typeid) {
        EditDocument(0, typeid);
        $('#SelDocType').dialog('close');
        //closeDialog($('#SelDocType'));
    }

    //-- предупреждение  --
  function doEditDocumentWarn(_EntrantDocumentID, showModifiedWarn) {
      //- метод в другой EditDocumentAddPart  --
      if (!showModifiedWarn) {
          EditDocument(_EntrantDocumentID, 0);
          return true;
      } else {
          confirmDialog('Данный документ используется в другом заявлении. Если вы измените документ, то он также изменится и в другом заявлении. Продолжить?',
              function() {
                  EditDocument(_EntrantDocumentID, 0);
              });
      }
  }

  function getDocument(DocId, success){
		doPostAjax("<%= Url.Generate<ApplicationController>(x => x.GetDocumentForAppDocList(0,0)) %>", JSON.stringify({ApplicationID:ApplicationId, EntrantDocumentID:DocId}), function (data){
			if(data.IsError){    infoDialog(data.Message.replace(/\n/g, '<br/>'));   return;    }
      if(success){   success(data.Data);    }
    });  
  }

  function EditDocument(EntrantDocumentID, DocTypeID) { 
    if(DocTypeID==undefined){ DocTypeID=1;}
    var $Dialog = null;
    $Dialog = $('#Wz2UniDDialog');
    if ($Dialog.length == 0) {
      $Dialog = $('<div id="Wz2UniDDialog" style="display:none, position:fixed"></div>');
      $('body').append($Dialog);
    } 
    doPostAjax('<%= Url.Generate<EntrantController>(x => x.getEditDocument(null, null, null)) %>', { EntrantDocumentID: EntrantDocumentID, DocTypeID: DocTypeID, EntrantID: EntrantID }, function (data) {  
      $Dialog.html(data);
      UniDFormInit();
      $Dialog.dialog({
        modal: true,
        width: 800,
        title: "Документ",
        buttons: { 
          "Сохранить": function () { 
            var baseModel={     EntrantID: EntrantID   };
            var doc=UniDPrepareModel(baseModel);
            if(doc!=null || doc!=undefined){
              // Сохранение, если соханилось нормальн то закрыть и обновить.

              UniDSave(doc, function(model){
                // Получить обновленную модель документа
                var docid=model.EntrantDocumentID;
                //GetDocument ForAppDocList //GetDocumentForAppDocList
                getDocument( docid, function(d) {
                    closeDialog($Dialog);

                    var $tr = $('tr[itemID="' + d.EntrantDocumentID + '"]');
                    if ($tr.length == 0) {
                        var isDetach = 0;
                        if (d.ApplicationID == ApplicationId) {
                            isDetach = 1;
                        }
                        addNewDocument(null, d, isDetach); // Добавляем новую версию документа и атачим
                    } else {
                        var isDetach = 0;
                        if (d.ApplicationID == ApplicationId) {
                            isDetach = 1;
                        }
                        addNewDocument($tr, d, isDetach); // Добавляем новую версию документа
                        $tr.remove(); // Удаляем старую версию      
                    }
                });
              }, function(e){
                  infoDialog("Не удалось сохранить документ! "+e);
              });
            }else{
            
            }
          },
          "Закрыть": function () { 
            $(this).dialog('close'); 
          },
        },
        close: function () {
          $Dialog.remove();
        }
      }).dialog('open');
    }, "application/x-www-form-urlencoded", "html");
  }
</script>
<script language="javascript" type="text/javascript">
  function getEgeDocument(REModel, success, error) {
    btnCheckEge(REModel, success, error);
  }

  function btnCheckEge(REModel, success, error) {
      if (REModel == undefined) {
          REModel = {
              Step: WizStep,
              method: "ByCertificate",
              ApplicationID: ApplicationId,
              doc: null,
              regNum: null,
              refr: 1,
              currentYear: 0,
              Typ: 1,
              DocTypeID: 0,
              DocId: null,
              OlympicID: null
          }
      }
      REModel.ApplicationID = ApplicationId;
      //	Признак типа олимпиады (далее – @Typ, 0 – Всероссийская олимпиада школьников / 1 – олимпиада школьников)    
      REModel.Typ = 1;
      //	Номер олимпиады в перечне (OlympicType.OlympicNumber) (далее - @OlympNumber, может быть NULL, если @Typ = 0)
      // REModel.OlympNumber=1;
      var type = parseInt(REModel.DocTypeID);
      var apiurl = "<%= Url.Generate<ApplicationController>(x => x.CheckEGEResults(null)) %>";
      switch (type) {
          case 2: // 2	Свидетельство о результатах ЕГЭ
              apiurl = "<%= Url.Generate<ApplicationController>(x => x.CheckEGEResults(null)) %>";
              break;
          case 9: // 9	Диплом победителя/призера олимпиады школьников
              apiurl = "<%= Url.Generate<ApplicationController>(x => x.CheckOlympicResults(null)) %>";
              REModel.Typ = 1;
              break;
          case 10:  // 10	Диплом победителя/призера всероссийской олимпиады школьников
              apiurl = "<%= Url.Generate<ApplicationController>(x => x.CheckOlympicResults(null)) %>";
              REModel.Typ = 0;
              break;
          default:
              REModel.Typ = 0;
              break;
      }
      blockUI();
      doPostAjax(apiurl, JSON.stringify({ model: REModel }), function (data) {
          unblockUI();
          $("#Wz2UniDDialog").dialog("option", "position", $("#Wz2UniDDialog").dialog("option", "position"));
          if (data.IsError) {
              infoDialog(data.Message);
              if (error) { error(); }
              if (window.ShowWz != undefined) { window.ShowWz(3); }
              if (window.loadPaView != undefined) { window.loadPaView(2); }
              return;
          }
          if (data.Data) {
          if (data.Data.violationId == 0) { //ПЕРЕЗАГРУЗКА СТРАНИЦЫ -- СООБЩЕНИЙ НЕ НАДО!
              closeDialog($('#Wz2UniDDialog'));
              closeDialog($('#divAddETDocument'));
              closeDialog($('#UniDDialog'));
                  if (success) {
                      success(data.Data);
                  }
                  if (window.ShowWz != undefined) {
                      window.ShowWz(3);
                  }
                  if (window.loadPaView != undefined) {
                      window.loadPaView(1);
                  }
              } else {
                  if (success) {
                      success(data.Data);
                  }
                  infoDialog(data.Data.violationMessage);
                  //closeDialog($('#divAddETDocument'));
                  if (window.ShowWz != undefined) {
                      window.ShowWz(3);
                  }
                  if (window.loadPaView != undefined) {
                      window.loadPaView(1);
                  }
              }
          }
      });
  }
</script>

<% } %>
