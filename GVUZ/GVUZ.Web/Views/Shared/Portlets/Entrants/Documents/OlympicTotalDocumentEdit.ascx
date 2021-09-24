<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Model.Entrants.Documents.OlympicTotalDocumentViewModel>" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Register TagPrefix="gv" TagName="BaseDocumentEditCommonPart" Src="~/Views/Shared/Portlets/Entrants/Documents/BaseDocumentEditCommonPart.ascx" %>
<div id="content">
    <table class="data">
        <thead>
            <tr>
                <th class="caption"></th><th></th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.DocumentTypeID) %></td>
                <td><b><%: Model.DocumentTypeName %></b></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelReqFor(m => m.UID) %></td>
                <td><%= Html.TextBoxExFor(m => m.UID) %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelReqFor(m => m.DocumentSeries) %></td>
                <td><%= Html.TextBoxExFor(m => m.DocumentSeries, new {style = "width:50px"}) %>&nbsp;<%= Html.TableLabelFor(m => m.DocumentNumber) %><%= Html.TextBoxExFor(m => m.DocumentNumber, new {style = "width:150px"}) %>
                    <span id="documentSeriessValidationMsg" />
                </td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelReqFor(m => m.DiplomaTypeID) %></td>
                <td><%= Html.DropDownListFor(m => m.DiplomaTypeID, new SelectList(Model.DiplomaList, "ID", "Name")) %></td>
            </tr>
            <tr>
                <td class="caption"></td>
                <td>
                    <table class="subjectList gvuzDataGrid" cellpadding="3" style="width: 400px">
                        <thead>
                            <tr>
                                <th style="width: 88%">
                                    <%= Html.LabelFor(x => x.SubjectDetails) %>
                                </th>
                                <th style="width: 40px">
                                </th>
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
                    <span>
                        <input type="text" val-required="Необходимо выбрать хотя бы одну дисциплину" val-new-line="0" id="tbEmptySubjects" style="display: none" />
                    </span>
                </td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelReqFor(m => m.DocumentDate) %></td>
                <td><%= Html.DatePickerFor(m => m.DocumentDate) %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelReqFor(m => m.DocumentOrganization) %></td>
                <td><%= Html.TextBoxExFor(m => m.DocumentOrganization) %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.DocumentAttachmentID) %></td>
                <td><% if (Model.DocumentAttachmentID != Guid.Empty)
                       { %><div><a fileID="<%= Model.DocumentAttachmentID %>" class="getFileLink"><%: Model.DocumentAttachmentName
    %></a><button class="fileDelete" onclick=" detachAttachedDocument(this);return false; ">&nbsp;</button></div><% } %>
                    <%= Html.FileForm("") %></td>
            </tr>
        </tbody>
    </table>
    <gv:BaseDocumentEditCommonPart ID="commonPart" runat="server" />
    <script type="text/javascript">
        var subjectNames = JSON.parse('<%= Html.Serialize(Model.SubjectNameList) %>');
        var subjectData = JSON.parse('<%= Html.Serialize(Model.Subjects) %>');
        var $trEdited = null;
        jQuery(function() {
            $('#DocumentSeries,#DocumentNumber').blur(function() {
                checkDocumentOnExisting(jQuery('#DocumentSeries').val(), jQuery('#DocumentNumber').val());
            });
        });

        function fillModel() {
            var model =
            {
                DocumentTypeID: <%= Model.DocumentTypeID %>,
                UID: jQuery("#UID").val(),
                DocumentSeries: jQuery('#DocumentSeries').val(),
                DocumentNumber: jQuery('#DocumentNumber').val(),
                DiplomaTypeID: jQuery('#DiplomaTypeID').val(),
                DocumentDate: jQuery('#DocumentDate').val(),
                DocumentOrganization: jQuery('#DocumentOrganization').val(),
                Subjects: []
            };
            jQuery('.subjectList tr:.saved').each(function() {
                model.Subjects.push(
                    {
                        SubjectName: unescapeHtml(jQuery(this).find('td:first').html())
                    });
            });
            return model;
        }

        function addEditRow($trToAdd) {
            $trToAdd.before('<tr class="trUnsaved"><td><select class="subjectNames" /></td>'
                + '<td align="center" nowrap="nowrap"><a href="#" title="Сохранить" class="btnSaveS" onclick="saveNewRow(this);return false"></a>&nbsp;'
                + '<a href="#" class="btnDeleteUS" title="Удалить" onclick="cancelEditing();return false"></a></td></tr>');
            for (var i = 0; i < subjectNames.length; i++)
                jQuery('.subjectNames').append("<option>" + subjectNames[i] + "</option>");
        }

        function createAddedRow($trBefore, item) {
            var className = $trBefore.prev().hasClass('trline1') ? 'trline2' : 'trline1';
            $trBefore.before('<tr class="saved ' + className + '"><td>' +
                escapeHtml(item.SubjectName) + '</td>'
                + '<td align="center" nowrap="nowrap"><a href="#" class="btnEditS"  title="Редактировать" onclick="editRow(this);return false"></a>&nbsp;' +
                '<a href="#" class="btnDeleteS" title="Удалить" onclick="deleteRow(this);return false"></a></td></tr>');
            jQuery('#tbEmptySubjects').val('1');
        }

        function cancelEditing() {
            jQuery('#trAddNewSubject').show();
            jQuery('.trUnsaved').remove().detach();
            if ($trEdited != null) $trEdited.show();
            $trEdited = null;
        }

        function deleteRow(el) {
            jQuery(el).parents('tr:first').remove().detach();
            if (jQuery('.subjectList tr:.saved').length == 0)
                jQuery('#tbEmptySubjects').val('');
            else
                jQuery('#tbEmptySubjects').val('1');
            jQuery('#btnAddNewSubject').focus().blur();
        }

        function saveNewRow(el) {
            var $tr = jQuery(el).parents('tr:first');
            createAddedRow($tr, { SubjectName: $tr.find('.subjectNames').val() });
            $tr.remove().detach();
            jQuery('#trAddNewSubject').show();
            if ($trEdited != null) $trEdited.remove().detach();
            $trEdited = null;
        }

        function editRow(el) {
            cancelEditing();
            var $tr = jQuery(el).parents('tr:first');
            $trEdited = $tr;
            addEditRow($tr);
            jQuery('.subjectNames').val(unescapeHtml($tr.find('td:first').html()));
            jQuery('.subjectValue').val(unescapeHtml($tr.find('td:first').next().html()));
            $trEdited.hide();
            return false;
        }

        jQuery('#btnAddNewSubject').click(function() {
            cancelEditing();
            addEditRow(jQuery('#trAddNewSubject'));
            jQuery('#trAddNewSubject').hide();
        });

        function initialFillData() {
            for (var i = 0; i < subjectData.length; i++)
                createAddedRow(jQuery('#trAddNewSubject'), subjectData[i]);
        }

        initialFillData();
    </script>
</div>