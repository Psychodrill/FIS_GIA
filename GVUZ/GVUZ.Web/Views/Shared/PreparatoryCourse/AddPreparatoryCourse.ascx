<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.AddPreparatoryCourseViewModel>" %>
<%@ Import Namespace="System.Web.Script.Serialization" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

<style type="text/css">
    span.ui-tool select {
        margin: 0px;
        padding: 0px;
    }
	
</style>

<div id="content">
    <table class="gvuzDataGrid">
        <tbody>
            <tr>
                <td class="caption">
                    <%= Html.LabelFor(m => m.CourseTypeID) %>:
                </td>
                <td>
                    <%--<%= Html.DropDownListFor(m => m.CourseTypeID, new SelectList(Model.CourseTypeList, "ID", "Name"), new {style = "width:450px"})%>--%>
                    <%= Html.TextBoxExFor(m => m.CourseName, new {style = "width:450px"}) %>
                </td>
            </tr>
            <tr>
                <td class="caption" valign="middle">
                    <%= Html.LabelFor(m => m.SubjectData) %>:
                </td>
                <td>
                    <table class="gvuzDataGrid tablestatement2" cellpadding="3" id="tableSubjects" style="width: 454px">
                        <colgroup>
                            <col width="395" /><col width="50" />
                        </colgroup>
                        <% if (Model.SubjectData != null)
                               foreach (string s in Model.SubjectData)
                               { %>
                            <tr>
                                <td class="itemData"><%: s %></td>
                                <td align="center"><a href="#" class="btnSDelete"><img src="<%= Url.Images("delete_16.gif") %>" alt="delete" /></a></td>
                            </tr>
                        <% } %>

                        <tr id="trAddNewSubject">
                            <td colspan="2">
                                <a href="#" id="btnAddNewSubject" class="add">Добавить предмет</a>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="caption">
                    <%= Html.LabelFor(m => m.Information) %>:
                </td>
                <td>
                    <%= Html.TextAreaFor(m => m.Information, new {style = "width:454px;height:240px"}) %>
                </td>
            </tr>
            <tr id="trFileUpload">
                <td class="caption"><%= Html.LabelFor(m => m.FileID) %>:</td>
                <td><% if (Model.FileID.HasValue)
                       { %><span>
                                                                                  <%= Url.GenerateLink<PreparatoryCourseController>(x => x.GetFile1(Model.FileID), Model.FileName) %>
                                                                                  <a href="" title="Удалить файл" id="btnFileDelete" class="delete"></a></span>
                    <% } %>
                    <%= Html.FileForm("", new {style = "width:454px;"}) %>
                </td>
            </tr>


        </tbody>
    </table>
    <div style="display: none">
        <input id="btnSubmit" type="button" value="Сохранить" /> 
        <input id="btnCancel" type="button" value="Отмена" />
    </div>
</div>

<script type="text/javascript">
    var $trEditedRow;
    var fileID = null;
    var fileDeleted = false;
    var predefinedSubjects = JSON.parse('<%= new JavaScriptSerializer().Serialize(Model.PredefinedSubjects) %>');

    function addFileValidationError($control) {
        addValidationError($control, 'Размер файла превышает максимальный разрешенный размер в <%= BaseController.GetMaxAllowedFileLength() %>Кб');
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
        if (!isFileLengthCorrect($control[0], <%= BaseController.GetMaxAllowedFileLength() %> * 1024)) {
            addFileValidationError($control);
            return true;
        }
        return false;
    }

    jQuery('#postFile').change(function() { fileID = null; });
    jQuery('#fileForm').submit(function() {
        if (revalidatePage(jQuery('.gvuzDataGrid'))) return false;
        if (checkFileSelectError(jQuery('#postFile'))) return false;
        blockUI();
        if (fileID != null) {
            submitData();
            return false;
        }
        jQuery(this).ajaxSubmit({
            method: 'POST',
            dataType: 'json',
            url: '<%= Url.Generate<AdmissionController>(x => x.ReceiveFile1()) %>',
            error: showAsyncFilePostError,
            success: function showResponse(data) {
                if (isFilePostError(jQuery('#postFile'), data)) return;
                fileID = data.FileID;
                submitData();
            }
        });
        return false;
    });

    function submitData() {
        var model = {
            CourseID: <%= Model.CourseID %>,
            InstitutionID: <%= Model.InstitutionID %>,
                        			<%--CourseTypeID: jQuery('#CourseTypeID').val(),--%>
            CourseName: jQuery('#CourseName').val(),
            Information: /*$.htmlClean(*/jQuery('#Information').val()/*)*/,
            FileID: fileID,
            FileDeleted: fileDeleted,
            SubjectData: []
        };
        jQuery('#tableSubjects tr td.itemData').each(function() { model.SubjectData.push(unescapeHtml(jQuery(this).html())); });
        doPostAjax("<%= Url.Generate<PreparatoryCourseController>(x => x.CreatePreparatoryCourse(null)) %>", JSON.stringify(model), function(data) {
            if (!addValidationErrorsFromServerResponse(data)) {
                createdItem = data.Data;
                jQuery('#btnCancel').click();
            }
            unblockUI();
        }, null, null, false);
    }

    jQuery('#btnCancel').click(function() { closeDialog(jQuery('#treeControlDialog')); });
    jQuery('#btnSubmit').click(function() { jQuery('#fileForm').submit(); });
    var addNewSubjectRow = function($trBefore) {
        //var inSelectData = '';
        var isPredefinedEmpty = predefinedSubjects.length == 0;
        if (isPredefinedEmpty) predefinedSubjects.push('');
        $trBefore.before('<tr><td align="center">'
            + '<input type="text" style="width:366px" id="inSubject" maxlength="50" value="' + ''
            + '" /><br /></td><td align="center" nowrap="nowrap"><a href="#" class="btnSSave"><img src="<%= Url.Images("save_16.gif") %>" alt="save" /></a> '
            + '<a href="#" class="btnSDeleteU"><img src="<%= Url.Images("delete_16.gif") %>" alt="delete" /></a></td></tr>'); //jQuery('#inSubject').autocomplete({source: predefinedSubjects, minLength: 0, delay: 0})
        autocompleteDropdown(jQuery('#inSubject'), { source: predefinedSubjects, minLength: 0, delay: 0 });
        //jQuery('#inSubject').focus(function() {jQuery(this).autocomplete('search', jQuery(this).val())}).focus()
        jQuery('#trAddNewSubject').hide();
    };

    function doSubjectChange() {
        jQuery('#inSubject').val(jQuery('#inSelect').children('option:selected').text());
        jQuery('#inSubject').select();
    }

    if (!document.addPreparatoryCourseCreated)
        jQuery('.btnSDeleteU').live('click', function() {
            jQuery(this).parents('tr:first').remove().detach();
            jQuery('#trAddNewSubject').show();
        });
    if (!document.addPreparatoryCourseCreated)
        jQuery('.btnSDelete').live('click', function() {
            jQuery(this).parents('tr:first').remove().detach();
        });
    if (!document.addPreparatoryCourseCreated)
        jQuery('.btnSSave').live('click', function() {
            var $tr = jQuery(this).parents('tr:first');
            var selVar = $tr.find('#inSubject').val();
            if (selVar == '') {
                alert('Необходимо ввести название предмета');
                return;
            }
            $tr.before('<tr><td class="itemData">' + escapeHtml(selVar) + '</td>'
                + '<td align="center">'
                + '<a href="#" class="btnSDelete"><img src="<%= Url.Images("delete_16.gif") %>" alt="delete" /></a></td></tr>');
            $tr.remove().detach();
            jQuery('#trAddNewSubject').show();
        });
    jQuery('#btnAddNewSubject').click(function() {
        addNewSubjectRow(jQuery('#trAddNewSubject'));
    });
    jQuery('#btnFileDelete').click(function() {
        fileDeleted = true;
        jQuery('#btnFileDelete').parent().remove();
        return false;
    });
    document.addPreparatoryCourseCreated = 1;
//setTimeout(function() {jQuery("textarea#Information").htmlInput() }, 12)
</script>