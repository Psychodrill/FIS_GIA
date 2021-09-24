<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.Portlets.Entrants.PersonalRecordsDataViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.Models" %>

<% if (Model.ShowDenyMessage)
   { %> <div>Невозможно редактировать данное заявление</div> <script type="text/javascript"> function doSubmit() { return false; } </script>  <% }
   else
   { %>	
    <table class="tableApp2">
        <tbody>
            <% if (Model.ApplicationStep != ApplicationStepType.ParentData)
               { %>
                <% if (Model.ApplicationStep == ApplicationStepType.PersonalData && !Url.IsInsidePortlet())
                   { %>
                    <tr class="mainRow">
                        <td class="caption" width="200px"><%= Html.StrongLabelFor(m => m.Entrant.LastName, required: true) %></td>
                        <td><%= Html.TextBoxExFor(m => m.Entrant.LastName, new {tabindex = 1}) %></td>
                        <td class="caption"><%= Html.TableLabelFor(m => m.Entrant.GenderID, required: true) %></td>
                        <td style="width: auto" colspan="3">					
                            <%= Html.DropDownListExFor(m => m.Entrant.GenderID, Model.GenderList, new {tabindex = 20, disabled = Model.IsEdit, @class = "gender_select"}) %>
                            <span class="caption">
                                <%= Html.TableLabelFor(m => m.BirthDate, new {style = "width:auto;margin:0px 0px !important;"}, true) %></span>					
                            <%= Html.DatePickerFor(m => m.BirthDate, new {tabindex = 21, disabled = Model.IsEdit}) %>					
                        </td>				
                    </tr>
                <% } %>
                <tr class="mainRow">
                    <td class="caption"><%= Html.StrongLabelFor(m => m.Entrant.FirstName, required: true) %></td>
                    <td><%= Html.TextBoxExFor(m => m.Entrant.FirstName, new {tabindex = 2}) %></td>
                    <td class="caption"><%= Html.TableLabelFor(m => m.NationalityID, required: true) %></td>
                    <td colspan="3"><%= Html.DropDownListExFor(m => m.NationalityID, Model.NationalityList, new {tabindex = 22, disabled = Model.IsEdit}) %></td>
                </tr>
                <tr class="mainRow">
                    <td class="caption"><%= Html.StrongLabelFor(m => m.Entrant.MiddleName) %></td>
                    <td><%= Html.TextBoxExFor(m => m.Entrant.MiddleName, new {tabindex = 3}) %></td>
                    <td class="caption"><%= Html.TableLabelFor(m => m.BirthPlace, required: false) %></td>
                    <td colspan="3"><%= Html.TextBoxExFor(m => m.BirthPlace, new {tabindex = 23, disabled = Model.IsEdit}) %></td>
                </tr>
                <tr class="mainRow">
                    <td class="caption"><%= Html.TableLabelFor(m => m.DocumentTypeID, new {style = "padding-top:0px;padding-bottom:10px;"}, required: true) %></td>
                    <td><%= Html.DropDownListExFor(m => m.DocumentTypeID, Model.IdentityDocumentList, new {tabindex = 4, disabled = Model.IsEdit, onchange = "identityDocumentTypeChanged()"}) %></td>
                    <td class="caption" rowspan="3" valign="top"><%= Html.TableLabelFor(m => m.CustomInformation) %></td>
                    <td colspan="3" rowspan="3"><%= Html.TextAreaFor(m => m.CustomInformation, new {tabindex = 24, @class = "app"}) %></td>
                </tr>
                <tr class="mainRow">
                    <td class="caption"><%= Html.TableLabelFor(m => m.DocumentNumber, required: true) %></td>
                    <td ><%= Html.TextBoxExFor(m => m.DocumentSeries, new {@class = "passSeries", tabindex = 10, disabled = Model.IsEdit || Model.DisableDocumentDataEditing}) %><%= Html.TextBoxExFor(m => m.DocumentNumber, new {@class = "passNumber", tabindex = 11, disabled = Model.IsEdit || Model.DisableDocumentDataEditing}) %></td>
                </tr>
                <tr class="mainRow">
                    <td class="caption"><%= Html.TableLabelFor(m => m.DocumentOrganization, required: false) %></td>
                    <td><%= Html.TextBoxExFor(m => m.DocumentOrganization, new {tabindex = 12, disabled = Model.IsEdit}) %></td>
                </tr>
                <tr class="mainRow">
                    <td class="caption"><%= Html.TableLabelFor(m => m.DocumentDate, required: true) %></td>
                    <td><%= Html.DatePickerFor(m => m.DocumentDate, new {tabindex = 13, disabled = Model.IsEdit}) %>
                        <span class="caption"><%= Html.TableLabelFor(m => m.SubdivisionCode, new {style = "width:106px;margin:0px 0px !important;display:inline-block;position:relative;top:6px;"}) %></span>
                        <%= Html.TextBoxExFor(m => m.SubdivisionCode, new {tabindex = 14, disabled = Model.IsEdit, @class = "passSeries"}) %></td>
                    <td></td>
                    <td colspan="3"></td>
                </tr>
                <tr class="mainRow">
                    <td class="caption"><%= Html.TableLabelFor(m => m.DocumentAttachmentID, required: false) %></td>
                    <td><%= Url.GenerateLinkIf<EntrantController>(x => x.GetFile1(Model.DocumentAttachmentID), Model.DocumentAttachmentName, Model.DocumentAttachmentID != Guid.Empty, Model.IsEdit ? "Отсутствует" : "") %>
                        <%= Model.IsEdit ? "" : Html.FileForm("", new {tabindex = 15}) %></td>
                    <td></td>
                    <td colspan="3"></td>
                    <%--<td class="caption"><%= Html.TableLabelFor(m => m.RegistrationDate, required: true)%></td>
				<td colspan="3"><%= Html.DatePickerFor(m => m.RegistrationDate, new { tabindex = 26 })%></td>--%>
                </tr><% } %>
            <% if (Model.ApplicationStep == 0)
               { %>
                <tr>
                    <td colspan="4" style="height: 40px"></td>
                </tr>
            <% } %>
            <% if (Model.ApplicationStep != ApplicationStepType.PersonalData)
               { %>
                <% if (Model.ApplicationStep != ApplicationStepType.ParentData)
                   { %>
                    <tr>
                        <td colspan="4">Сведения о родителях</td>
                    </tr>
                <% } %>
                <tr>
                    <th colspan="2">отец</th>
                    <th colspan="2">мать</th>
                </tr>
                <tr>
                    <td class="caption"><%= Html.TableLabelFor(m => m.Father.PersonData.LastName) %></td>
                    <td class="fatherData"><%= Html.TextBoxExFor(m => m.Father.PersonData.LastName, new {tabindex = 30}) %></td>
                    <td class="caption"><%= Html.TableLabelFor(m => m.Mother.PersonData.LastName) %></td>
                    <td class="motherData"><%= Html.TextBoxExFor(m => m.Mother.PersonData.LastName, new {tabindex = 36}) %></td>
                </tr>
                <tr>
                    <td class="caption"><%= Html.TableLabelFor(m => m.Father.PersonData.FirstName) %></td>
                    <td class="fatherData"><%= Html.TextBoxExFor(m => m.Father.PersonData.FirstName, new {tabindex = 31}) %></td>
                    <td class="caption"><%= Html.TableLabelFor(m => m.Mother.PersonData.FirstName) %></td>
                    <td class="motherData"><%= Html.TextBoxExFor(m => m.Mother.PersonData.FirstName, new {tabindex = 37}) %></td>
                </tr>
                <tr>
                    <td class="caption"><%= Html.TableLabelFor(m => m.Father.PersonData.MiddleName) %></td>
                    <td class="fatherData"><%= Html.TextBoxExFor(m => m.Father.PersonData.MiddleName, new {tabindex = 32}) %></td>
                    <td class="caption"><%= Html.TableLabelFor(m => m.Mother.PersonData.MiddleName) %></td>
                    <td class="motherData"><%= Html.TextBoxExFor(m => m.Mother.PersonData.MiddleName, new {tabindex = 38}) %></td>
                </tr>
                <tr>
                    <td class="caption"><%= Html.TableLabelFor(m => m.Father.WorkPlace) %></td>
                    <td class="fatherData"><%= Html.TextBoxExFor(m => m.Father.WorkPlace, new {tabindex = 33}) %></td>
                    <td class="caption"><%= Html.TableLabelFor(m => m.Mother.WorkPlace) %></td>
                    <td class="motherData"><%= Html.TextBoxExFor(m => m.Mother.WorkPlace, new {tabindex = 39}) %></td>
                </tr>
                <tr>
                    <td class="caption"><%= Html.TableLabelFor(m => m.Father.Position) %></td>
                    <td class="fatherData"><%= Html.TextBoxExFor(m => m.Father.Position, new {tabindex = 34}) %></td>
                    <td class="caption"><%= Html.TableLabelFor(m => m.Mother.Position) %></td>
                    <td class="motherData"><%= Html.TextBoxExFor(m => m.Mother.Position, new {tabindex = 40}) %></td>
                </tr>
                <tr>
                    <td class="caption"><%= Html.TableLabelFor(m => m.Father.WorkPhone) %></td>
                    <td class="fatherData"><%= Html.TextBoxExFor(m => m.Father.WorkPhone, new {tabindex = 35}) %></td>
                    <td class="caption"><%= Html.TableLabelFor(m => m.Mother.WorkPhone) %></td>
                    <td class="motherData"><%= Html.TextBoxExFor(m => m.Mother.WorkPhone, new {tabindex = 41}) %></td>
                </tr> <% } %>
        </tbody>
    </table>
    <% if (Model.ApplicationStep == 0)
       { %>
        <div>
            <a id="btnSubmit" href="<%= Url.Generate<EntrantController>(c => c.Index()) %>">Сохранить</a>
            <a id="btnCancel" href="<%= Url.Generate<EntrantController>(c => c.Index()) %>">Отмена</a>
        </div>

    <% } %>


    <script type="text/javascript">
		//<%--var fileID = <%= Model.DocumentAttachmentID == Guid.Empty ? null : "\"" + Model.DocumentAttachmentID + "\"" %>--%>
        var fileID = <%= "\"" + Model.DocumentAttachmentID + "\"" %>;
        var noFile = <%= Model.IsEdit ? 1 : 0 %>;
        var actionCommand = '';
        var russianDocs = JSON.parse('<%= Html.Serialize(Model.RussianDocs) %>');

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

        function doValidation() {
            var res = !revalidatePage(jQuery('.mainRow'), true);
            clearValidationErrors(jQuery('.fatherData'));
            clearValidationErrors(jQuery('.motherData'));
            if (jQuery('#DocumentNumber').hasClass('input-validation-error') || jQuery('#DocumentSeries').val() == '') {
                jQuery('#DocumentNumber').addClass('input-validation-error');
                jQuery('#DocumentSeries').addClass('input-validation-error');
            }
            var changed = false;
            jQuery('.fatherData > input').each(function() { if (jQuery(this).val() != '') changed = true; });
            if (changed)
                res &= !revalidatePage(jQuery('.fatherData'));
            changed = false;
            jQuery('.motherData > input').each(function() { if (jQuery(this).val() != '') changed = true; });
            if (changed)
                res &= !revalidatePage(jQuery('.motherData'));
            if (!noFile)
                res &= !checkFileSelectError(jQuery('#postFile'));
            if (!noFile && jQuery('#postFile').val() == '' && fileID == null) {
                addValidationError(jQuery('#postFile'), 'Необходимо выбрать файл', false);
                res = false;
            }

            return res;
        }

        function doSubmitFile() {
            blockUI();
            if (fileID != null || noFile) {
                doSubmitData();
                return;
            }
            jQuery('#fileForm').ajaxSubmit({
                method: 'POST',
                dataType: 'json',
                url: '<%= Url.Generate<EntrantController>(x => x.ReceiveFile1()) %>',
                error: showAsyncFilePostError,
                success: function(data) {
                    if (isFilePostError(jQuery('#postFile'), data)) return;
                    fileID = data.FileID;
                    doSubmitData();
                }
            });
            return false;
        }


        function doSubmitData() {
            var model =
            {
                <% if (Model.ApplicationStep != ApplicationStepType.ParentData)
                   { %>
                Entrant:
                {
                    LastName: jQuery('#Entrant_LastName').val(),
                    FirstName: jQuery('#Entrant_FirstName').val(),
                    MiddleName: jQuery('#Entrant_MiddleName').val(),
                    GenderID: jQuery('#Entrant_GenderID').val()
                },
                BirthDate: jQuery('#BirthDate').val(),
                DocumentTypeID: jQuery('#DocumentTypeID').val(),
                DocumentDate: jQuery('#DocumentDate').val(),
                SubdivisionCode: jQuery('#SubdivisionCode').val(),
                DocumentSeries: jQuery('#DocumentSeries').val(),
                DocumentNumber: jQuery('#DocumentNumber').val(),
                DocumentOrganization: jQuery('#DocumentOrganization').val(),
                DocumentAttachmentID: fileID,
                NationalityID: jQuery('#NationalityID').val(),
                BirthPlace: jQuery('#BirthPlace').val(),
                CustomInformation: jQuery('#CustomInformation').val(), <% } %>
                <% if (Model.ApplicationStep == ApplicationStepType.PersonalData && !Url.IsInsidePortlet())
                   { %>
                                					<%--ApplicationNumber: jQuery('#ApplicationNumber').val(),
					RegistrationDate: jQuery('#RegistrationDate').val(),--%>
                ApplicationID: <%= Model.ApplicationID %>,
                <% } %>
                <% if (Model.ApplicationStep != ApplicationStepType.PersonalData)
                   { %>
                Father:
                {
                    PersonData:
                    {
                        LastName: jQuery('#Father_PersonData_LastName').val(),
                        FirstName: jQuery('#Father_PersonData_FirstName').val(),
                        MiddleName: jQuery('#Father_PersonData_MiddleName').val()
                    },
                    WorkPlace: jQuery('#Father_WorkPlace').val(),
                    Position: jQuery('#Father_Position').val(),
                    WorkPhone: jQuery('#Father_WorkPhone').val()
                },
                Mother:
                {
                    PersonData:
                    {
                        LastName: jQuery('#Mother_PersonData_LastName').val(),
                        FirstName: jQuery('#Mother_PersonData_FirstName').val(),
                        MiddleName: jQuery('#Mother_PersonData_MiddleName').val()
                    },
                    WorkPlace: jQuery('#Mother_WorkPlace').val(),
                    Position: jQuery('#Mother_Position').val(),
                    WorkPhone: jQuery('#Mother_WorkPhone').val()
                },
                <% } %>
                EntrantID: <%= Model.EntrantID %>,
                ActionCommand: actionCommand,
                ApplicationStepInt: <%= (int) Model.ApplicationStep %>
            };
            doPostAjax('<%= Url.Generate<EntrantController>(c => c.SavePersonalData(null)) %>', 'model=' + encodeURIComponent(JSON.stringify(model)), function(data) {
                if (!addValidationErrorsFromServerResponse(data, true)) {
                    if (typeof doAppNavigate != "undefined")
                        doAppNavigate();
                    else
                        jQuery('#btnCancel').click();
                }
                unblockUI();
            }, "application/x-www-form-urlencoded", null, false);
            return false;
        }

        function doSubmit(cmd) {
            actionCommand = cmd;
            if (cmd == 'back' || cmd == 'save')   <%-- если не заполнено ни одно из полей, даём уйти со страницы --%>
            {
                var isFilled = false;
                jQuery('.mainRow input[type="text"]:not(.datePicker)').each(function() { if (jQuery(this).val() != '') isFilled = true; });
                if (jQuery('#postFile').val() != '') isFilled = true;
                jQuery('.fatherData input').each(function() { if (jQuery(this).val() != '') isFilled = true; });
                jQuery('.motherData input').each(function() { if (jQuery(this).val() != '') isFilled = true; });
                if (!isFilled && typeof doAppNavigate != "undefined") {
                    doAppNavigate();
                    return false;
                }
            }
            if (cmd == 'back' || cmd == 'save') {   <%-- если идём назад или сохраняем, то даём некорректные данные сохранить --%>
                doSubmitFile();
                return false;
            }
            if (!doValidation()) return false;
            doSubmitFile();
            return false;
        }

        function identityDocumentTypeChanged() {
            var docTypeID = jQuery('#DocumentTypeID').val();
            if (russianDocs.indexOf(docTypeID) >= 0)
                jQuery('#NationalityID').val('1').attr('disabled', 'disabled');
            else
                jQuery('#NationalityID').removeAttr('disabled');
        }

        jQuery(document).ready(function() {

            jQuery("input#SubdivisionCode").mask("999-999");

            jQuery('#btnCancel, #btnSubmit').button();

            jQuery('#postFile').change(function() { fileID = null; });
            jQuery(".datePicker:not(:disabled)").datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-62:+0' });

            //Фикс бага для хрома (DONOTOUCH)
            if (!(navigator.userAgent.indexOf('Chrome') != -1)) {
                jQuery('#BirthDate').datepicker("option", "defaultDate", '-17y');
                jQuery('#DocumentDate').datepicker("option", "maxDate", new Date());
            }

            jQuery('#btnSubmit').click(function() {    return doSubmit();      });
            jQuery('#btnCancel').click(function() { window.location.href = jQuery(this).attr('href'); });
            jQuery('#Father_WorkPhone').mask('89999999999');
            jQuery('#Mother_WorkPhone').mask('89999999999');
            identityDocumentTypeChanged();
        });
    </script>
<% } %>