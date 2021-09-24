<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Model.Entrants.Documents.EGEDocumentViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
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
                <td class="caption"><%= Html.TableLabelReqFor(m => m.DocumentNumber) %></td>
                <td><%= Html.TextBoxExFor(m => m.DocumentNumber, new {style = "width:120px"}) %> 
                <a href="#" id="btnGetEge" onclick=" getEgeDocument() ">Получить данные по номеру</a></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelReqFor(m => m.TypographicNumber) %></td>
                <td><%= Html.TextBoxExFor(m => m.TypographicNumber, new {style = "width:100px"}) %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelReqFor(m => m.DocumentYear) %></td>
                <td><%= Html.TextBoxExFor(m => m.DocumentYear, new {style = "width:100px"}) %></td>
            </tr>
            <%--<tr>
				<td class="caption"><%= Html.TableLabelFor(m => m.Decision) %></td>
				<td><%= Html.TextBoxExFor(m => m.Decision) %></td>
			</tr>--%>
            <%--<tr>
				<td class="caption"><%= Html.TableLabelFor(m => m.ProfessionTypeID) %></td>
				<td><%= Html.DropDownListFor(m => m.ProfessionTypeID, new SelectList(Model.ProfessionList, "ID", "Name"), new {style = "width: 500px"})%></td>
			</tr>--%>
            <%--<tr>
				<td class="caption"><%= Html.TableLabelFor(m => m.DecisionNumber) %></td>
				<td><%= Html.TextBoxExFor(m => m.DecisionNumber)%></td>
			</tr>
			<tr>
				<td class="caption"><%= Html.TableLabelFor(m => m.DecisionDate) %></td>
				<td><%= Html.DatePickerFor(m => m.DecisionDate) %></td>
			</tr>--%>
            <tr>
                <td class="caption"></td>
                <td><div id="divErrorPlace"></div>
                    <table class="subjectList gvuzDataGrid" cellpadding="3" style="width: 400px">
                        <thead>
                            <tr>
                                <th style="width: 60%">
                                    <%= Html.LabelFor(x => x.SubjectDetails.SubjectName) %>
                                </th>
                                <th style="width: 25%">
                                    <%= Html.LabelFor(x => x.SubjectDetails.Value) %>
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
            $('#DocumentNumber').blur(function() {
                checkDocumentOnExisting(null, jQuery('#DocumentNumber').val());
            });
        });

        function fillModel() {
            var model =
            {
                DocumentTypeID: <%= Model.DocumentTypeID %>,
                UID: jQuery("#UID").val(),
                DocumentNumber: jQuery('#DocumentNumber').val(),
                DocumentYear: jQuery('#DocumentYear').val(),
                Decision: jQuery('#Decision').val(),
                                					<%--ProfessionTypeID: jQuery('#ProfessionTypeID').val(),
					DecisionNumber: jQuery('#DecisionNumber').val(),
					DecisionDate: jQuery('#DecisionDate').val(),--%>
                TypographicNumber: jQuery('#TypographicNumber').val(),
                Subjects: []
            };
            jQuery('.subjectList tr:.saved').each(function() {
                model.Subjects.push(
                    {
                        SubjectName: unescapeHtml(jQuery(this).find('td:first').html()),
                        Value: jQuery(this).find('td:first').next().html().replace('.', ',')
                    });
            });
            return model;
        }

        function addEditRow($trToAdd) {
            $trToAdd.before('<tr class="trUnsaved"><td><select class="subjectNames" /></td>'
                + '<td align="center"><input type="text" class="subjectValue numeric" maxlength="7" style="width:84px;padding-right:1px;" /></td>'
                + '<td align="center" nowrap="nowrap"><a href="#" title="Сохранить" class="btnSaveS" onclick="saveNewRow(this);return false"></a>&nbsp;'
                + '<a href="#" class="btnDeleteUS" title="Удалить" onclick="cancelEditing();return false"></a></td></tr>');
            for (var i = 0; i < subjectNames.length; i++)
                jQuery('.subjectNames').append("<option>" + subjectNames[i] + "</option>");
        }

        function createAddedRow($trBefore, item) {
            var className = $trBefore.prev().attr('class');
            if ((typeof className != "undefined") && className.indexOf('trline1') >= 0) className = 'trline2';
            else className = 'trline1';

            $trBefore.before('<tr class="saved ' + className + '"><td>' +  escapeHtml(item.SubjectName) + '</td>'
                + '<td align="center">' +  (item.Value == null ? '' : item.Value) + '</td>'
                +'<td align="center"><a href="#" class="btnEditS"  title="Редактировать" onclick="editRow(this);return false"></a>&nbsp;' +
                '<a href="#" class="btnDeleteS" title="Удалить" onclick="deleteRow(this);return false"></a></td></tr>');
            jQuery('#tbEmptySubjects').val('1');
        }

        function cancelEditing() {
            jQuery('#trAddNewSubject').show();
            jQuery('.trUnsaved').remove().detach();
            jQuery('#divErrorPlace').html();
            if ($trEdited != null) $trEdited.show();
            $trEdited = null;
        }

        function deleteRow(el) {
            jQuery(el).parents('tr:first').remove().detach();
            if (jQuery('.subjectList tr:.saved').length == 0)
                jQuery('#tbEmptySubjects').val('');
            else
                jQuery('#tbEmptySubjects').val('1');
            jQuery('#btnAddNewSubject').focus().blur(); //refresh page
        }

        function saveNewRow(el) {
            var $tr = jQuery(el).parents('tr:first');
            var val = $tr.find('.subjectValue').val().replace(',', '.');
            var valFloat = new Number(val);
            clearValidationErrors($tr);
            jQuery('#divErrorPlace').html('');
            if (valFloat <= 0 || valFloat > 100 || isNaN(valFloat)) {
                //$tr.find('.subjectValue').addClass('input-validation-error').after('<span class="field-validation-error">Балл должен быть числом от 0 до 100</span>')
                jQuery('#divErrorPlace').html('<span class="field-validation-error">Балл должен быть числом от 1 до 100</span>');
                $tr.find('.subjectValue').removeClass('input-validation-error-fixed').addClass('input-validation-error');
                return;
            }
            createAddedRow($tr, { SubjectName: $tr.find('.subjectNames').val(), Value: val });
            $tr.remove().detach();
            jQuery('#trAddNewSubject').show();
            if ($trEdited != null) $trEdited.remove().detach();
            $trEdited = null;
        }

        function editRow(el) {
            cancelEditing();
            var $tr = jQuery(el).parents('tr:first');
            $trEdited = $tr; //jQuery('#trAddNewSubject').hide()
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
        jQuery(function() {
            $("input#DocumentNumber").mask("99-999999999-99");
            $('#btnGetEge').button();
        });

        function checkAbilityToCheckEgeByNumber() {
            var certNumber = jQuery('#DocumentNumber').val();
            if (certNumber.match(/\d\d-\d\d\d\d\d\d\d\d\d-\d\d/))
                $('#btnGetEge').removeAttr('disabled');
            else {
                $('#btnGetEge').attr('disabled', 'disabled');
            }
        }

        checkAbilityToCheckEgeByNumber();

        jQuery('#DocumentNumber').keyup(checkAbilityToCheckEgeByNumber);

        function getEgeDocument() {
            var certNumber = jQuery('#DocumentNumber').val();
            var appID = applicationID;
            if (!certNumber.match(/\d\d-\d\d\d\d\d\d\d\d\d-\d\d/))
                return false;
            doPostAjax('<%= Url.Generate<ApplicationController>(x => x.GetApplicationEGEResults(null, null)) %>', JSON.stringify({ applicationID: appID, certificateNumber: certNumber }),
                function(data) {
                    if (data.IsError)
                        infoDialog(escapeHtml(data.Message));
                    else {
                        var m = data.Data;
                        jQuery('#DocumentYear').val(m.DocumentYear);
                        jQuery('#TypographicNumber').val(m.TypographicNumber);

                        jQuery('.subjectList .btnDeleteUS').each(function() { deleteRow(this); });
                        jQuery('.subjectList .btnDeleteS').each(function() { deleteRow(this); });
                        for (var i = 0; i < m.Subjects.length; i++)
                            createAddedRow(jQuery('#trAddNewSubject'), m.Subjects[i]);
                    }
                });
            return false;
        }
    </script>
</div>