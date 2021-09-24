<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%-- Required from parent Control:
	documentTypeLists - array of available types
	function onNewDocumentCreated - called when new document added
	getFileLink - url for getting files
	getFileSep - url separator (& or ?)
--%>
<script type="text/javascript">
  //<%-- ccskrb  --%>
  var receiveFileUrl = '<%= Url.Generate<EntrantController>(x => x.ReceiveFile1()) %>'
  var saveUrl = '<%= Url.Generate<EntrantController>(c => c.SaveDocumentAuto(null)) %>'
  var createdItem

  //<%-- справшиваем пользователя о типе документа (список доступных типов снаружи сохранён)  --%>
  function beginAddDocument() {
    var res = ''
    for (var i = 0; i < documentTypeLists.length; i++) {
      res += '<div style="padding: 3px;"><a href="" onclick="doAddDocument(' +  documentTypeLists[i].TypeID + ');return false;">' + documentTypeLists[i].Name + '</a></div>';
    }
    jQuery('<div class="dialogSelect">' + res + '</div>').dialog({ resizeable: false, title: 'Выберите тип документа', width: 900, modal: true });
    return false
  }

  //<%--  открываем диалог добавления/редактирования --%>
  function openAddDocumentDialog(navUrl, postData, callback, isEdit) {
    createdItem = null
    var caption = jQuery('#dialogCaption').html();
    if (caption == null || caption == '') caption = isEdit ? 'Редактирование документа' : 'Добавление документа'
    doPostAjax(navUrl, postData, function (data) {
      jQuery('#documentAddDialog').html(data);
      //<%-- ставим ссылки забора файлов --%>
      jQuery('a.getFileLink').each(function () { jQuery(this).attr('href', getFileLink + getFileSep + 'fileID=' + jQuery(this).attr('fileID')); });
      jQuery('#documentAddDialog').dialog({
        modal: true,
        width: 800,
        title: caption,
        buttons:
                {
                  "Сохранить": function () { jQuery('#btnSubmit').click(); },
                  "Отмена": function () { jQuery('#btnCancel').click(); }
                },
        close: function () {
          if (createdItem) callback();
        }
      }).dialog('open');
      $('#IdentityDocumentTypeID').change(function () {
        switch ($(this).val()) {
          case '3':
            $("#DocumentNumber").attr('maxlength', '50');
            $("#DocumentSeries").attr('maxlength', '20');
            break;
          case '9':
            $("#DocumentNumber").attr('maxlength', '50');
            $("#DocumentSeries").attr('maxlength', '20');
            break;
          case '1':
            $("#DocumentNumber").attr('maxlength', '6');
            $("#DocumentNumber").val($("#DocumentNumber").val().length > 6 ? $("#DocumentNumber").val().substr(0, 6) : $("#DocumentNumber").val());
            $("#DocumentSeries").attr('maxlength', '4');
            $("#DocumentSeries").val($("#DocumentSeries").val().length > 4 ? $("#DocumentSeries").val().substr(0, 4) : $("#DocumentSeries").val());
            break;
          default:
            $("#DocumentNumber").attr('maxlength', '10');
            $("#DocumentNumber").val($("#DocumentNumber").val().length > 10 ? $("#DocumentNumber").val().substr(0, 10) : $("#DocumentNumber").val());
            $("#DocumentSeries").attr('maxlength', '6');
            $("#DocumentSeries").val($("#DocumentSeries").val().length > 6 ? $("#DocumentSeries").val().substr(0, 6) : $("#DocumentSeries").val());
            break;
        }
      });
      $('#IdentityDocumentTypeID').change();
    }, "application/x-www-form-urlencoded", "html");

  }

  //<%-- вызываем диалог редактирования (находим строчку с документом если есть и открываем диалог)  --%>
  function callEditDocument(docID) {
    var $el = jQuery('.gvuzDataGrid tr[itemID="' + docID + '"] a').length;
    if ($el.length > 0)
      doEditDocument($el[0]);
    else {
      openAddDocumentDialog('<%= Url.Generate<EntrantController>(x => x.EditDocument(null)) %>', 'entrantDocumentID=' + docID,
                function () { onNewDocumentCreated(createdItem, null); }, true);
    }
  }

  //<%--  редактирование документа. Данные из строки с ним --%>
  function doEditDocument(el) {
    var $tr = jQuery(el).parents('tr:first');
      openAddDocumentDialog('<%= Url.Generate<EntrantController>(x => x.EditDocument(null)) %>', 'entrantDocumentID=' + $tr.attr('itemID'),
       function () { onNewDocumentCreated(createdItem, el); }, true);
    return false
  }

  //<%-- добавление документа  --%>
  function doAddDocument(typeID, cgID, subjectId) {
    jQuery('.dialogSelect').dialog('close')
    openAddDocumentDialog('<%= Url.Generate<EntrantController>(x => x.AddDocument(null, null, null, null)) %>',
            'entrantID=' + WZ4.entrantID + '&documentTypeID=' + typeID + '&competitiveGroupId=' + cgID + '&subjectId=' + subjectId,
            function () { onNewDocumentCreated(createdItem); });
    return false
  }

  //<%-- инициализация дейтпикера  --%>
  function setDatePicker() {
    jQuery(".datePicker").datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-63:+0' });
  }
</script>
