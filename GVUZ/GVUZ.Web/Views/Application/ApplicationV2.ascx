<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.ApplicationV2Model>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<div id="documentAddDialog">
</div>
<div id="content">
    <table class="gvuzDataGrid tableStatement2" cellpadding="3" id="docGrid1">
        <thead>
            <tr>
                <th style="width:20%">
                    <%= Html.LabelFor(x => x.BaseDocument.DocumentTypeName) %>
                </th>
                <th>
                    <%= Html.LabelFor(x => x.BaseDocument.DocumentSeriesNumber)%>
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
                <th>
                    <%= Html.LabelFor(x => x.BaseDocument.OriginalReceivedDate) %>
                </th>
                <th>
                    <%= Html.LabelFor(x => x.BaseDocument.OriginalReceived) %>
                </th>
            </tr>
        </thead>
        <tbody>
            <% var cnt = 0; foreach (var doc in Model.AttachedDocuments)
               {
                   cnt++; %>
            <tr itemid="<%: doc.EntrantDocumentID %>" class="<%= cnt % 2 == 0 ? "trline2" : "trline1" %>">
                <td>
                    <span class="linkSumulator" onclick="ViewDocument(this)">
                        <%: doc.DocumentTypeName %>
                    </span>
                </td>
                <td>
                    <%: doc.DocumentSeriesNumber %>
                </td>
                <td>
                    <%: doc.DocumentDate %>
                </td>
                <td>
                    <%: doc.DocumentOrganization %>
                </td>
                <td>
                    <%= Url.GenerateLinkIf<EntrantController>(x => x.GetFile1(doc.DocumentAttachmentID), doc.DocumentAttachmentName, doc.DocumentAttachmentID != Guid.Empty, "Отсутствует") %>
                </td>
                <td>
                    <%: doc.CanNotSetReceived ? " " : (doc.OriginalReceivedDate ?? "Не предоставлены") %>
                </td>
                <td>
                    <%= doc.CanNotSetReceived ? " " : ((GVUZ.Helper.UrlUtils.IsReadOnly(FBDUserSubroles.ApplicationsDirection)) 
								? (doc.OriginalReceived ? "Да" : "Нет")
                                : ("<input type=\"checkbox\" " + (doc.OriginalReceived ? "checked=\"checked\"" : "") + (doc.StatusID==8 ? "disabled=\"disabled\"" : "") +"  onclick=\"doDateReceivedDialog(this, " + doc.EntrantDocumentID + ")\" />"))%>
                </td>
            </tr>
            <%} %>
        </tbody>
    </table>
</div>
<div style="display: none">
    <div class="divOriginalDateDialog">
        <table>
            <tr>
                <td>
                    Дата предоставления:
                </td>
                <td>
                    <input type="text" class="datePicker tbDateOriginalDialog" />
                </td>
            </tr>
        </table>
    </div>
</div>
<div id="UniDDialog" style="display: none">
</div>

<script type="text/javascript">
    var getFileLink = '<%= Url.Generate<EntrantController>(x => x.GetFile1(null)) %>';
    var getFileSep = '<%= Url.Generate<EntrantController>(x => x.GetFile1(null)) %>'.indexOf('?') >= 0 ? '&' : '?';

    var doView = function (navUrl, postData) {
        createdItem = null;
        doPostAjax(navUrl, postData, function (data) {
            jQuery('#documentAddDialog').html(data);
            jQuery('a.getFileLink').each(function () { jQuery(this).attr('href', getFileLink + getFileSep + 'fileID=' + jQuery(this).attr('fileID')) });
            jQuery('#documentAddDialog').dialog({
                modal: true,
                width: 800,
                title: jQuery('#dialogCaption').html(),
                buttons: {
                    "Закрыть": function () { jQuery(this).dialog('close'); }
                }
            }).dialog('open');
        }, "application/x-www-form-urlencoded", "html");
    }

    //-- Открываем диалог просмотра документа --
    function ViewDocument(el) {
        var $tr = jQuery(el).parents('tr:first');
        var EntrantDocumentID = $tr.attr('itemID');
        doPostAjax('<%= Url.Generate<EntrantController>(x => x.getViewDocument(null)) %>', { EntrantDocumentID: EntrantDocumentID, DocTypeID: 1 }, function (data) {
            $('#UniDDialog').html(data);
            $('#UniDDialog').dialog({
                modal: true,
                width: 800,
                title: "Дополнительный документ",
                buttons: { "Закрыть": function () { $(this).dialog('close'); } }
        , close: function () { }
            }).dialog('open');
        }, "application/x-www-form-urlencoded", "html");
    }

    jQuery(function () {
        if (jQuery('#divOriginalDateDialog').length > 0) {
            jQuery('.divOriginalDateDialog').remove().detach();
            return;
        }
        jQuery('body').append(jQuery('.divOriginalDateDialog'));
        jQuery('.divOriginalDateDialog')[0].id = 'divOriginalDateDialog';
        jQuery('#divOriginalDateDialog').removeClass('divOriginalDateDialog');
        jQuery('#divOriginalDateDialog').hide();
        jQuery("#divOriginalDateDialog .datePicker").datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-5:+0', maxDate: new Date() });
    });

    function doDateReceivedDialog(el, docID) {
        var $dateEl = jQuery(el).parent().prev();
        var docDate = $dateEl.text();
        var isChecked = jQuery(el).is(':checked');
        if (isChecked) jQuery(el).removeAttr('checked');
        else jQuery(el).attr('checked', 'checked');
        if (!isChecked) {
            saveDateReceived(docID, null, false, $dateEl, el);
            return;
        }
        if (docDate && docDate != 'Не предоставлены') {
            jQuery('.tbDateOriginalDialog').val(docDate);
        } else {
            jQuery('.tbDateOriginalDialog').val('<%= DateTime.Today.ToString("dd.MM.yyyy") %>');
        }
        jQuery("#divOriginalDateDialog").dialog(
        {
            modal: true,
            width: 550,
            title: "Дата предоставления оригинала документа / Заявление с обязательством предоставления оригинала в течение первого учебного года",
            buttons:
            {
                "Сохранить": function () {
                    saveDateReceived(docID, jQuery('.tbDateOriginalDialog').val(), true, $dateEl, el);
                    closeDialog(jQuery(this));
                },
                "Отмена": function () { closeDialog(jQuery(this)); }
            }
        }).dialog('open');
    }

    function saveDateReceived(docID, date, isChecked, $dateEl, cbEl) {
        doPostAjax('<%= Url.Generate<ApplicationController>(x => x.SetDocumentOriginalReceived(null, null, null, null)) %>',
			'applicationID=<%= Model.ApplicationID %>&entrantDocumentID=' + docID + '&received=' + (isChecked ? 'true' : 'false') + '&receivedDate=' + date,
			function (data) {
			    if (data.IsError) alert(data.Message);
			    else {
			        $dateEl.html(isChecked ? (date ? date : '<%= DateTime.Today.ToString("dd.MM.yyyy") %>') : 'Не предоставлены');
			        if (!isChecked) jQuery(cbEl).removeAttr('checked');
			        else jQuery(cbEl).attr('checked', 'checked');

			        if (typeof OnChangeOriginalsProvided === "function") {
			            OnChangeOriginalsProvided();
			        }

			    }
			}, "application/x-www-form-urlencoded");
    }

</script>
