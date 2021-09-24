<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Model.Entrants.Documents.EntrantDocumentListViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<div id="documentAddDialog"></div>
<div id="content">
    <table class="gvuzDataGrid" cellpadding="3">
        <thead>
            <tr>
                <th>
                    <%= Html.LabelFor(x => x.BaseDocument.DocumentTypeName) %>
                </th>
                <th>
                    <%= Html.LabelFor(x => x.BaseDocument.DocumentSeriesNumber) %>
                </th>
                <th>
                    <%= Html.LabelFor(x => x.BaseDocument.DocumentDate) %>
                </th>
                <th>
                    <%= Html.LabelFor(x => x.BaseDocument.DocumentOrganization) %>
                </th>
                <th>
                    <%= Html.LabelFor(x => x.BaseDocument.DocumentAttachmentID) %>
                </th>
                <th style="width: 40px">
                </th>
            </tr>
            <tr id="trAddNew">
                <td colspan="6"><a href="#" id="btnAddNew" class="add">Добавить новый документ</a></td>
            </tr>
        </thead>
        <tbody>
        </tbody>
    </table>
</div>
<script type="text/javascript">

    var existingItems = JSON.parse('<%= Html.Serialize(Model.Documents) %>');
    var documentTypeLists = JSON.parse('<%= Html.Serialize(Model.DocumentTypes) %>');
    var receiveFileUrl = '<%= Url.Generate<EntrantController>(x => x.ReceiveFile1()) %>';
    var saveUrl = '<%= Url.Generate<EntrantController>(c => c.SaveDocumentAuto(null)) %>';
    var getFileLink = '<%= Url.Generate<EntrantController>(x => x.GetFile1(null)) %>';
    var getFileSep = '<%= Url.Generate<EntrantController>(x => x.GetFile1(null)) %>'.indexOf('?') >= 0 ? '&' : '?';
    var createdItem;
    var addNewDocument = function($trBefore, item) {
        $trBefore.before('<tr itemID="' + item.EntrantDocumentID + '"><td><span class="btn'
            + (item.CanBeModified ? 'Edit' : 'View')
            + ' linkSumulator">' + escapeHtml(item.DocumentTypeName)
            + '</a></td><td>' + escapeHtml(item.DocumentSeriesNumber) + '</td><td>'
            + item.DocumentDate + '</td><td> '
            + escapeHtml(item.DocumentOrganization == null ? '' : item.DocumentOrganization) + '</td><td>' +
            (item.DocumentAttachmentID != null ? '<a href="<%= Url.Generate<EntrantController>(x => x.GetFile1(null)) %>' + getFileSep + 'fileID=' + item.DocumentAttachmentID + '">' + item.DocumentAttachmentName + '</a>' : '')
            + '</td>'
            + (item.CanBeModified
                ? '<td align="center"><a href="#" title="Редактировать документ" class="btnEdit"></a>&nbsp;<a href="#" title="Удалить документ" class="btnDelete"></a></td></tr>'
                : '<td align="center"><span title="Невозможно редактировать документ (уже используется)" class="btnEditGray"></span>&nbsp;<span title="Невозможно удалить документ (уже используется)" class="btnDeleteGray"></span></td></tr>'));
    };
    var doAddEdit = function(navUrl, postData, callback) {
        createdItem = null;
        doPostAjax(navUrl, postData, function(data) {
            jQuery('#documentAddDialog').html(data);
            jQuery('a.getFileLink').each(function() { jQuery(this).attr('href', getFileLink + getFileSep + 'fileID=' + jQuery(this).attr('fileID')); });
            jQuery('#documentAddDialog').dialog({
                modal: true,
                width: 800,
                title: jQuery('#dialogCaption').html(),
                buttons:
                {
                    "Сохранить": function() { jQuery('#btnSubmit').click(); },
                    "Отмена": function() { jQuery('#btnCancel').click(); }
                },
                close: function() {
                    if (createdItem) callback();
                }
            }).dialog('open');
        }, "application/x-www-form-urlencoded", "html");
    };

    function doAddDocument(typeID, competitiveGroupId, subjectId) {
        jQuery('.dialogSelect').dialog('close');
        doAddEdit('<%= Url.Generate<EntrantController>(x => x.AddDocument(null, null, null, null)) %>', 
            'entrantID=' + <%= Model.EntrantID %> + '&documentTypeID=' + typeID+ 
                '&competitiveGroupId = ' + competitiveGroupId + '&subjectId=' + subjectId,
            function() { addNewDocument(jQuery('#trAddNew'), createdItem); });
        return false;
    }

    function setDatePicker() {
        jQuery(".datePicker").datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-30:+0' });
    }

    jQuery(document).ready(function() {
        jQuery('#btnAddNew').click(function() {
            var res = '';
            for (var i = 0; i < documentTypeLists.length; i++) {
                res += '<div style="padding: 3px;"><a href="" onclick="doAddDocument(' + documentTypeLists[i].TypeID + ');return false;">' + documentTypeLists[i].Name + '</a></div>';
            }
            jQuery('<div class="dialogSelect">' + res + '</div>').dialog(
                {
                    resizeable: false,
                    title: 'Выберите тип документа',
                    width: 900,
                    modal: true
                });
            return false;
        });
        jQuery('.btnEdit').live('click', function() {
            var $tr = jQuery(this).parents('tr:first');
            var itemID = $tr.attr('itemID');
            doAddEdit('<%= Url.Generate<EntrantController>(x => x.EditDocument(null)) %>', 'entrantDocumentID=' + itemID,
                function() {
                    addNewDocument($tr, createdItem);
                    $tr.remove().detach();
                });
            return false;
        });

        function doViewDocument(navUrl, postData) {
            createdItem = null;
            doPostAjax(navUrl, postData, function(data) {
                jQuery('#documentAddDialog').html(data);
                jQuery('a.getFileLink').each(function() { jQuery(this).attr('href', getFileLink + getFileSep + 'fileID=' + jQuery(this).attr('fileID')); });
                jQuery('#documentAddDialog').dialog({
                    modal: true,
                    width: 600,
                    title: jQuery('#dialogCaption').html(),
                    buttons: {
                        "Закрыть": function() { jQuery(this).dialog('close'); }
                    }
                }).dialog('open');
            }, "application/x-www-form-urlencoded", "html");
        }

        jQuery('.btnView').live('click', function() {
            var $tr = jQuery(this).parents('tr:first');
            var itemID = $tr.attr('itemID');
            doViewDocument('<%= Url.Generate<EntrantController>(x => x.ViewDocument(0)) %>', 'entrantDocumentID=' + itemID);
            return false;
        });
        jQuery('.btnDelete').live('click', function() {
            var $tr = jQuery(this).parents('tr:first');
            var itemID = $tr.attr('itemID');
            confirmDialog('Вы действительно хотите удалить документ?', function() {
                doPostAjax('<%= Url.Generate<EntrantController>(x => x.DeleteDocument(null)) %>', 'entrantDocumentID=' + itemID, function(data) {
                    if (data.IsError)
                        jQuery('<div>' + data.Message + '</div>').dialog(
                            { buttons: { OK: function() { jQuery(this).dialog("close"); } } });
                    else
                        $tr.remove().detach();
                }, "application/x-www-form-urlencoded");
            });
            return false;
        });
        for (var i = 0; i < existingItems.length; i++) {
            addNewDocument(jQuery('#trAddNew'), existingItems[i]);
        }
    });
</script>