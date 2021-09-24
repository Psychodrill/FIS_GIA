<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Model.Entrants.Documents.BaseDocumentViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<div style="display: none">
    <input id="btnSubmit" type="button" value="Сохранить" /> 
    <input id="btnCancel" type="button" value="Отмена" />
</div>
<script type="text/javascript">
    var fileID = <%= Model.DocumentAttachmentID == Guid.Empty ? "null" : "\"" + Model.DocumentAttachmentID + "\"" %>;

    function addFileValidationError($control) {
        addValidationError($control, 'Размер файла превышает максимальный разрешенный размер в <%= Model.MaxFileSize %>Кб', true);
    }

    function isFilePostError($control, data) {
        if (data.BigSize) {
            unblockUI();
            addFileValidationError($control);
            return true;
        }
        return false;
    }

    function checkFileSelectError($control) {
        if (!isFileLengthCorrect($control[0], <%= Model.MaxFileSize %> * 1024)) {
            addFileValidationError($control);
            return true;
        }
        return false;
    }

    function detachAttachedDocument(el) {
        fileID = null;
        jQuery(el).parent().hide();
    }

    var validateDocumentSeriesAndNumber = true;

    function doValidation() {
        var res = !revalidatePage(jQuery('.data'), true); //Теперь не обязателен
        //if(jQuery('#postFile').val() == '' && fileID == null)
        //{
        //	addValidationError(jQuery('#postFile'), 'Необходимо выбрать файл', false)
        //	res = false
        //}
        if (validateDocumentSeriesAndNumber)
            if (jQuery('#DocumentNumber').hasClass('input-validation-error') || 
                jQuery('#DocumentSeries').hasClass('input-validation-error')) {
                jQuery('#DocumentNumber').addClass('input-validation-error');
                jQuery('#DocumentSeries').addClass('input-validation-error');
                res = false; // display validation info for document series under document number field
                var $valMessageSpan = jQuery('#DocumentSeries').next('span.field-validation-error');
                if ($valMessageSpan) {
                    $valMessageSpan = $valMessageSpan.detach();
                    jQuery('#DocumentNumber').after($valMessageSpan);
                }
            }
        return res;
    }

    function doSubmitFile() {
        blockUI();
        if (fileID != null || jQuery('#postFile').val() == '') {
            doSubmitData();
            return;
        }
        jQuery('#fileForm').ajaxSubmit({
            method: 'POST',
            dataType: 'json',
            url: receiveFileUrl,
            error: showAsyncFilePostError,
            success: function(data) {
                if (isFilePostError(jQuery('#postFile'), data)) return;
                fileID = data.FileID;
                doSubmitData();
            }
        });
    }


    function doSubmitData() {
        var model = fillModel();
        model.EntrantID = <%= Model.EntrantID %>;
        model.EntrantDocumentID = <%= Model.EntrantDocumentID %>;
        model.ApplicationID = applicationID ? applicationID : 0;
        model.DocumentAttachmentID = fileID == null ? "<%= Guid.Empty %>" : fileID;
        doPostAjax(saveUrl, 'model=' + JSON.stringify(model), function(data) {
            if (!addValidationErrorsFromServerResponse(data, true)) {
                createdItem = data.Data;
                jQuery('#btnCancel').click();
            }
            unblockUI();
        }, "application/x-www-form-urlencoded", null, false);
    }

    var prevDocSeries, prevDocNumber;

    function checkDocumentOnExisting(docSeries, docNumber, isPost) {
        //if(<%= Model.EntrantDocumentID %> > 0) return;

        if (docSeries == prevDocSeries && prevDocNumber == docNumber && !isPost)
            return;
        prevDocNumber = docNumber;
        prevDocSeries = docSeries;
        var model = {
            EntrantID: <%= Model.EntrantID %>,
            EntrantDocumentID: <%= Model.EntrantDocumentID %>,
            DocumentTypeID: <%= Model.DocumentTypeID %>,
            ApplicationID: applicationID ? applicationID : 0,
            DocumentSeries: docSeries,
            DocumentNumber: docNumber
        };
        var func = checkDocumentSeriesNumber;
        if (isPost) func = checkDocumentSeriesNumberPost;
        doPostAjax('<%= Url.Generate<EntrantController>(x => x.CheckDocumentOnExists(null)) %>', JSON.stringify(model), func);
    }

    function checkDocumentSeriesNumber(data) {
        if (data.Data.IsFound > 0 && <%= Model.EntrantDocumentID %> > 0) {
            jQuery('<div>У абитуриента найден документ с теми же данными. Убедитесь, что у этого документа корректные данные.</div>').dialog({
                width: '600px',
                modal: true,
                buttons: {
                    "Продолжить": function() {
                        closeDialog(jQuery(this));
                    }
                }
            });
            return;
        }
        if (data.Data.IsFound > 0) {
            if (!data.Data.CanBeModified) {
                jQuery('<div>У абитуриента найден документ с теми же данными. При этом его невозможно изменить, так как он уже используется в заявлении. Продолжить добавление нового документа?</div>').dialog({
                    width: '600px',
                    modal: true,
                    buttons: {
                        "Продолжить": function() {
                            closeDialog(jQuery(this));
                        },
                        "Не добавлять": function() {
                            closeDialog(jQuery(this));
                            jQuery('#btnCancel').click();
                        }
                    }
                });
            } else {
                jQuery('<div>У абитуриента найден документ с теми же данными. Перейти к редактированию существующего?</div>').dialog({
                    width: '600px',
                    modal: true,
                    buttons: {
                        "Продолжить добавление": function() {
                            closeDialog(jQuery(this));
                        },
                        "Редактировать": function() {
                            closeDialog(jQuery(this));
                            if (callEditDocument) callEditDocument(data.Data.IsFound);
                        },
                        "Не добавлять": function() {
                            closeDialog(jQuery(this));
                            jQuery('#btnCancel').click();
                        }
                    }
                });
            }
        }
    }

    function checkDocumentSeriesNumberPost(data) {
        if (data.Data.IsFound > 0) {
            jQuery('<div>У абитуриента найден документ с теми же данными. Исправьте данные перед сохранением.</div>').dialog({
                width: '600px',
                modal: true,
                buttons: {
                    "Закрыть": function() {
                        closeDialog(jQuery(this));
                    }
                }
            });
            return;
        } else {
            doSubmitFile();
        }
    }


    jQuery('#postFile').change(function() { fileID = null; });
    setDatePicker();

    jQuery('#btnSubmit').click(function() {
        if (!doValidation()) return;
        checkDocumentOnExisting(prevDocSeries, prevDocNumber, true);
    });
    prevDocNumber = null;
    prevDocSeries = null;
    jQuery('#btnCancel').click(function() { closeDialog(jQuery('#documentAddDialog')); });
</script>