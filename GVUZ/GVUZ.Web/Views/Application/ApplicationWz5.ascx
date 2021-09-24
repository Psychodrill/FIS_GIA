<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.ApplicationWz5ViewModel.Wz5SendingViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.DAL.Model" %>

<script type="text/javascript">
    // основные переменные
    var applicationId = <%=Model.ApplicationID%>;
    var campaignId = <%=Model.CampaignID %>;
    var campaignTypeId = <%=Model.CampaignTypeID %>;
    var statusId = <%=Model.StatusID %>;
    var originalsProvided = '<%=Model.OriginalsProvided %>' === 'True';

    var editMode = true; // включен ли режим редактирования
    var changed = false; // были ли изменена Кампания
    var oldModel = null; // для сохранения копии главной модели при старте

    var notEdit = (statusId == 4 || statusId == 6); // возможно ли редактирование
    
    // табконтрол - Документы, Испытания
    var Wz5Documents = new TabControl(jQuery('#appDetailsTab'), [
            { name: 'Документы', link: 'javascript:SwitchWz5Documents(0)', enable: true, selected: true, noWrap: true },
            { name: 'Испытания', link: 'javascript:SwitchWz5Documents(1)', enable: true, noWrap: true },
    ], { prefix: 'popup' });

    // инициализация закладки, вызывается автоматически при входе на закладку
    function Wz5Init() {
        // сохранение копии главной модели
        oldModel = GetModel();

        // загрузка документов и испытаний
        Wz5Documents.init();
        Wz5LoadDocuments(0);

        Initialization();
    }

    function Initialization() {
        // инициализация календарей
        $(".datePicker").datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-10:+0', maxDate: new Date() });
    
        // инициализация в блоке редактирования условий приема
        ConditionsInit();

        if (notEdit) { 
            $(".edit_control").attr("disabled", "disabled");
            $(".editor").hide();
        }
    }


    // обновление при отмене изменений
    function RefreshConditions() {

        doPostAjax('<%= Url.Generate<InstitutionApplicationDraftController>(x => x.RefreshConditions(null)) %>?id=' + applicationId, {},
            function (data) {
                $("#conditions_container").html(data);
                Initialization();
            }, "application/x-www-form-urlencoded", "html");
    }


    // вызывается автоматически при нажатии кнопки "Отмена"
    function Wz5Cancel() {
        if (editMode) {
            infoDialog("Для продолжения сохраните условия приема или отмените изменения");
            return false;
        }
    }

    // функция сборки главной модели
    GetModel = function () 
    {
        var model = {
            Step: 5,
            method: "ByIdentityDocument",
            ApplicationID: ApplicationId,
            doc: null,
            regNum: null,
            refr: 1,
            currentYear: 0,
            docId: 0,
            typ: 1,
            OlympicID: 0,
            RegistrationDate: $('#RegistrationDate').val(),
            Priority: $('#Priority').val(),
            Uid: $('#Uid').val(),
            ApplicationNumber: $('#ApplicationNumber').val(),
            NeedHostel: $('input[name="NeedHostel"]').attr('checked'),
        }
        return model;
    }

    // функция сборки модели с условиями приема для отправки на сервер при сохранении
    GetConditions = function () 
    {
        var model = [];
        var verify = true;

        model.ApplicationId = applicationId;


        $('#conditionsTable tr').each(function (i) {
            if (i > 0) {
                var row = {
                    ApplicationId: applicationId,
                    CompetitiveGroupId: $(this).attr("data-value"),
                    EducationFormId: $(this).find("#EducationFormId").val(),
                    EducationSourceId: $(this).find("#EducationSourceId").val(),
                    IsForSPOandVO: $(this).find("#IsForSPOandVO").is(":checked"),
                    IsAgreed: $(this).find("#IsAgreed").is(":checked"),
                    IsDisagreed: $(this).find("#IsDisagreed").is(":checked"),
                    IsAgreedDate: $(this).find(".IsAgreedDate").val(),
                    IsDisagreedDate: $(this).find(".IsDisagreedDate").val(),
                    CalculatedRating: $(this).find("#CalculatedRating").val(),
                    CompetitiveGroupTargetId: $(this).find("#CompetitiveGroupTargetId").val(),
                } 

                if((row.IsAgreed && row.IsAgreedDate == "") || (row.IsDisagreed && row.IsDisagreedDate == "")){
                    verify = false;
                    return false;
                }

                model.push(row);
            }
        });
        if(!verify){
            infoDialog("При проставлении согласия на зачисление или отказа от него поля даты обязательны для заполнения.");
            model = null;
        }
        return model;
    }

    // сохранение с проверками, вызывается автоматически при нажатии кнопки "Сохранить"
    function Wz5Save(success, error, step) { 
        $('#txtUidError span').text("");
        $('#txtDateError span').text("");

        if (editMode) {
            infoDialog("Для продолжения сохраните условия приема или отмените изменения");
            return false;
        }

        // собираем модель с условиями приема
        var conditionsModel = GetConditions();
        if(conditionsModel == null)
            return false;

        var model = GetModel();
        model.ApplicationPriorities = { ApplicationId: ApplicationId, ApplicationPriorities: conditionsModel, ChangeCg: changed };
        model.changePage = Wz5HasChanged(model, oldModel),

        blockUI();

        doPostAjaxSync('<%= Url.Generate<ApplicationController>(x => x.Wz5Save(null)) %>', JSON.stringify({ model: model }), function (data) {
            unblockUI();
            
            $('#Uid').removeClass('input-validation-error-fixed');
            if (data.Extra == "UidList") {
                $('#Uid').addClass('input-validation-error');
                $('#txtUidError').html('<span class="field-validation-error">Значение в поле Идентификатор должно быть уникальным среди всех индивидуальных достижений, определенных для образовательной организации</span>');
                return;
            }
            
            $('#RegistrationDate').removeClass('input-validation-error-fixed');
            if (data.Extra == "DateError") {
                $('#RegistrationDate').addClass('input-validation-error');
                $('#txtDateError').html('<span class="field-validation-error">Введено некорректное значение</span>');
                return;
            }

            if (!data.IsError && data.Message == null) {
                if (success) {
                    success(model);
                }
                return;
            }

            jQuery("#lbCheckDescription").html((data.Message).replace('\n', '<br/>'));

            if(data.Extra == true){   
                $("#CriticalErrorsNotice").hide();
                $("#AllowSkipErrorsNotice").show();
                jQuery("#divDecisionDialog").dialog({
                    resizeable: false,
                    title: 'Ошибка сохранения',
                    width: 600,
                    modal: true,
                    buttons: {
                        "Принять принудительно": function () {
                            var model = {
                                ApplicationID: ApplicationId,
                                StatusDecision: jQuery("#tbCheckDecision").val(),
                                RegistrationDate: $('#RegistrationDate').val(),
                                Priority: $('#Priority').val(),
                                Uid: $('#Uid').val(),
                                NeedHostel: $('input[name="NeedHostel"]').attr('checked')
                            }
                            clearValidationErrors(jQuery("#tbCheckDecision").parent());
                            $('form[reason-file-type]').each(function() {
                                var $fileForm = $(this);  
                                clearValidationErrors($fileForm);
                            });
                             
                            closeDialog(jQuery('#divDecisionDialog'));

                            var hasEgeCheckError = false;
                            if((data.Data)&&(data.Data.EgeError))
                            {
                                hasEgeCheckError = true;
                            } 
                            forceAdmissionDialogPrepare(hasEgeCheckError);

                            jQuery("#divAcceptDialog").dialog({
                                resizeable: false,
                                title: 'Принудительное принятие заявления',
                                width: 800,
                                modal: true,
                                buttons: {
                                    "Принять принудительно": function () {  
                                        //получить условия приема
                                        //conditionsModel
                                        //var conditionsModel = GetConditions();
                                        //if(conditionsModel == null)
                                        //    return false;
                                        var applicationPriorities = { ApplicationId: ApplicationId, ApplicationPriorities: conditionsModel, ChangeCg: changed };

                                        var forceAdmissionModel = {
                                            ApplicationID: ApplicationId,
                                            Comment: jQuery("#tbCheckDecision").val(),
                                            ApplicationRegistrationDate: $('#RegistrationDate').val(),
                                            ApplicationPriority: $('#Priority').val(),
                                            ApplicationUid: $('#Uid').val(),
                                            ApplicationNeedHostel: $('input[name="NeedHostel"]').attr('checked'),
                                            ReasonID: $('#selForceAdmissionReason').val(),
                                            ApplicationPriorities: applicationPriorities,
                                            Attachments: []
                                        }

                                        clearValidationErrors(jQuery("#tbCheckDecision").parent());
                                        $('form[reason-file-type]').each(function() {
                                            var $fileForm = $(this);  
                                            clearValidationErrors($fileForm);
                                        });

                                        var hasError=false;
                                        if (forceAdmissionModel.Comment == '') { 
                                            addValidationError(jQuery('#tbCheckDecision'), 'Укажите причину решения', true);
                                            hasError = true;
                                        }
                                        if(hasEgeCheckError) {
                                            $('form[reason-file-type]:visible').each(function() {
                                                var $fileForm = $(this); 
                                                var $fileInput = $fileForm.find('input[type=file]');
                                                if($fileInput.get(0).files.length==0){
                                                    addValidationError($fileInput, 'Приложите файл', true);
                                                    hasError = true;
                                                }
                                            }); 
                                        }

                                        if(hasError)
                                            return false; 

                                        if(!hasEgeCheckError)
                                        {
                                            forceAdmissionModel.ReasonID = ''; 
                                            doPostAjaxSync('/Application/ForceAdmission', JSON.stringify({ model: forceAdmissionModel }),
                                                function (data2) {
                                                    closeDialog(jQuery('#divDecisionDialog'));
                                                });
                                            if (success) {
                                                success(forceAdmissionModel);
                                            }
                                        }
                                        else 
                                        {
                                            var filesCount=$('form[reason-file-type]:visible').length;
                                            var filesSubmittedCount = 0;
                                            $('form[reason-file-type]:visible').each(function() {
                                                var $fileForm = $(this);
                                                var attachmentType = $fileForm.attr('reason-file-type');

                                                $fileForm.ajaxSubmit({
                                                    method: 'POST',
                                                    dataType: 'json',
                                                    url: "/Application/ReceiveFile1", 
                                                    error: function (data){
                                                        filesSubmittedCount++;
                                                        if(filesSubmittedCount==filesCount)
                                                        {
                                                            doPostAjaxSync('<%= Url.Generate<ApplicationController>(x => x.ForceAdmission(null)) %>', JSON.stringify({ model: forceAdmissionModel }),
                                                            function (data2) {
                                                                closeDialog(jQuery('#divDecisionDialog'));
                                                            });
                                                            if (success) {
                                                                success(forceAdmissionModel);
                                                            }
                                                        }
                                                    },
                                                    success: function (data) {
                                                        if((data.FileID) && (data.FileID != 0))
                                                        { 
                                                            forceAdmissionModel.Attachments.push({ AttachmentFileID: data.FileID, AttachmentType: attachmentType });
                                                        }
                                                        filesSubmittedCount++;
                                                        if(filesSubmittedCount==filesCount)
                                                        {
                                                            doPostAjaxSync('<%= Url.Generate<ApplicationController>(x => x.ForceAdmission(null)) %>', JSON.stringify({ model: forceAdmissionModel }),
                                                            function (data2) {
                                                                closeDialog(jQuery('#divDecisionDialog'));
                                                            });
                                                            if (success) {
                                                                success(forceAdmissionModel);
                                                            }
                                                        }
                                                    }
                                                });
                                            });
                                        }
                                    }                               
                                }
                            });
                        },
                        "Оставить непрошедшим проверку": function () {
                            doPostAjax('<%= Url.Generate<ApplicationController>(x => x.Wz5SaveUnauthenticated(null)) %>', JSON.stringify({ model: model }),
                                    function (data2) {
                                        closeDialog(jQuery('#divDecisionDialog'));
                                    });
                                if (success) {
                                    success(model);
                                }
                            }
                    },
                    close: function () { }
                });
            }
            else{
                $("#CriticalErrorsNotice").show();
                $("#AllowSkipErrorsNotice").hide();
                jQuery("#divDecisionDialog").dialog({
                    resizeable: false,
                    title: 'Введите причину решения',
                    width: 600,
                    modal: true,
                    buttons: {
                        "Оставить непрошедшим проверку": function () {
                            doPostAjax('<%= Url.Generate<ApplicationController>(x => x.Wz5SaveUnauthenticated(null)) %>', JSON.stringify({ model: model }),
                                    function (data2) {
                                        closeDialog(jQuery('#divDecisionDialog'));
                                    });
                            if (success) {
                                success(model);
                            }
                        }
                    },
                    close: function () { }
                });               
                }
        });
    }

    // сохранение при попытке уйти с закладки, вызывается автоматически
    function SaveW5z(success, error, step) {
        SaveWz5(success, error, step);
    }

    // сохранение без проверки, вызывается автоматически при нажатии кнопки "Сохранить без проверки...."
    function SaveWz5(success, error, step) {
        $('#txtUidError span').text("");
        $('#txtDateError span').text("");

        if (editMode) {
            infoDialog("Для продолжения сохраните условия приема или отмените изменения");
            return false;
        }

        // собираем модель с условиями приема
        var conditionsModel = GetConditions();
        if(conditionsModel == null)
            return false;

        var model = {
            Step: step,
            ApplicationID: ApplicationId,
            RegistrationDate: $('#RegistrationDate').val(),
            Priority: $('#Priority').val(),
            Uid: $('#Uid').val(),
            ApplicationNumber: $('#ApplicationNumber').val(),
            NeedHostel: $('input[name="NeedHostel"]').attr('checked'), 
            changePage: false,
            ApplicationPriorities: { ApplicationId: ApplicationId, ApplicationPriorities: conditionsModel, ChangeCg: changed }
        }

        doPostAjaxSync('<%= Url.Generate<ApplicationController>(x => x.SaveWz5(null)) %>', JSON.stringify({ model: model }),
            function (data) {
                $('#Uid').removeClass('input-validation-error-fixed');
                if (data.Extra == "UidList") {
                    $('#Uid').addClass('input-validation-error');
                    $('#txtUidError').html('<span class="field-validation-error">Значение в поле Идентификатор должно быть уникальным среди всех индивидуальных достижений, определенных для образовательной организации</span>');
                    return;
                }
                $('#RegistrationDate').removeClass('input-validation-error-fixed');
                if (data.Extra == "DateError") {
                    $('#RegistrationDate').addClass('input-validation-error');
                    $('#txtDateError').html('<span class="field-validation-error">Введено некорректное значение</span>');
                    return;
                }
                if (!data.IsError) {
                    if (success) {
                        success(model);
                    }
                } else {
                    if (error) {
                        error("При сохранении произошла ошибка.");
                    }
                }
        });
    }

    // загрузка содержимого 2х закладок с документами и испытаниями
    function Wz5LoadDocuments(tabId) {
        switch (tabId) {
            case 0:
                doPostAjax('<%= Url.Generate<ApplicationController>(x => x.ApplicationV2(null)) %>?id=' + applicationId, {}, function(data) { $('#Wz5partialV').html(data); }, "application/x-www-form-urlencoded", "html");
                break;
            case 1:
                doPostAjax('<%= Url.Generate<ApplicationController>(x => x.ApplicationV3(null)) %>?id=' + applicationId, {}, function(data) { $('#Wz5partialV').html(data); }, "application/x-www-form-urlencoded", "html");
                break;
            default:
                return;
        }
    }

    // переключение закладок с документами и испытаниями
    function SwitchWz5Documents(tab) {
        for (var i = 0; i < Wz5Documents.menuItems.length; i++) {
            Wz5Documents.menuItems[i].selected = false;
        }
        Wz5Documents.menuItems[tab].selected = true;
        Wz5Documents.init();
        Wz5LoadDocuments(tab);
    }

    // метод проверки на изменения
    function Wz5HasChanged(model, oldModel) {
        if (model.RegistrationDate != oldModel.RegistrationDate || model.Priority != oldModel.Priority ||
            model.Uid != oldModel.Uid || model.NeedHostel != oldModel.NeedHostel )
            return true;
        return false;
    }

    function forceAdmissionDialogPrepare(hasEgeCheckError){
        if(hasEgeCheckError){
            forceAdmissionReasonChange();
        }
        else 
        {
            $("#rowForcedAdmissionReason").hide();
            $("#rowEgeResults").hide();
            $("#rowDocumentScan").hide();
            $("#rowFisScreenshot").hide();
        }
    }

    function forceAdmissionReasonChange(){
        var reasonId = $('#selForceAdmissionReason').val();
        $("#rowEgeResults").show();
        $("#rowDocumentScan").hide();
        $("#rowFisScreenshot").hide();
        if(reasonId == '<%= ApplicationForcedAdmissionReasons.IdentityDocumentError %>'){ 
            $("#rowDocumentScan").show();
        }
        else if(reasonId == '<%= ApplicationForcedAdmissionReasons.EgeResultsError %>'){
            $("#rowFisScreenshot").show();

        }
    }
</script>

<style type="text/css">
    div.tableStatement2 {
        border: 0px;
    }

        div.tableStatement2 a.btnDelete {
            background-position-y: 2px;
            height: 15px;
            margin-top: 0px;
            margin-bottom: 1px;
        }

    .chosen-container-multi .chosen-choices li {
        float: none;
        list-style: none;
    }
</style>

<div id="actual">
    <table class="personData tableApp2" style="width: 90%">
        <tr>
            <td class="caption" style="width: 200px">
                <%= Html.StrongLabelFor(m=>m.FIO) %>
            </td>
            <td>
                <%= Html.CommonInputReadOnly(Model.FIO, new { @class = "edit_control" })%>
            </td>
        </tr>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.DOB)%>
            </td>
            <td>
                <%= Html.CommonInputReadOnly(Model.DOB, new { @class = "edit_control" })%>
            </td>
        </tr>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.IdentityDocument)%>
            </td>
            <td>
                <%= Html.CommonInputReadOnly(Model.IdentityDocument, new { @class = "edit_control" })%>
            </td>
        </tr>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.Gender)%>
            </td>
            <td>
                <%= Html.CommonInputReadOnly(Model.Gender, new { @class = "edit_control" })%>
            </td>
        </tr>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.Citizen)%>
            </td>
            <td>
                <%= Html.CommonInputReadOnly(Model.Citizen, new { @class = "edit_control" }) %>
            </td>
        </tr>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.POB)%>
            </td>
            <td>
                <%= Html.CommonInputReadOnly(Model.POB, new { @class = "edit_control" }) %>
            </td>
        </tr>

        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.Email)%>
            </td>
            <td>
                <%= Html.CommonInputReadOnly(Model.Email, new { @class = "edit_control" }) %>
            </td>
        </tr>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.Region)%>
            </td>
            <td>
                <%= Html.CommonInputReadOnly(Model.Region, new { @class = "edit_control" }) %>
            </td>
        </tr>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.TownType)%>
            </td>
            <td>
                <%= Html.CommonInputReadOnly(Model.TownType, new { @class = "edit_control" }) %>
            </td>
        </tr>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.Address)%>
            </td>
            <td>
                <%= Html.CommonInputReadOnly(Model.Address, new { @class = "edit_control" }) %>
            </td>
        </tr>
        <tr>
            <td class="caption" style="width: 200px">
                <%= Html.TableLabelFor(m=>m.CustomInformation) %>
            </td>
            <td>
                <%= Html.CommonTextAreaReadOnly(Model.CustomInformation)%><br />
                &nbsp;
            </td>
        </tr>
    </table>

    <br />

    <div id="conditions_container">
        <%= Html.Partial("Application/ApplicationWz5_Conditions", Model) %>
    </div>

    <br />

    <table class="institutionData tableAdmin" style="width: 43%">

        <% if (!Url.IsInsidePortlet())
            { %>
        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.ApplicationNumber) %>
            </td>
            <td>
                <%= Html.TextBoxExFor(m => m.ApplicationNumber)%>
            </td>
        </tr>

        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.RegistrationDate, required: true)%>
            </td>
            <td nowrap="nowrap" id="dateEditor">
                <%= Html.DatePickerFor(m => m.RegistrationDate, new { @class = "edit_control" })%><br />
                <div id="txtDateError">
                </div>
            </td>
        </tr>

        <tr>
            <td class="caption">
                <%= Html.TableLabelFor(m => m.Uid) %>
            </td>
            <td>
                <%= Html.TextBoxExFor(m => m.Uid)%><div id="txtUidError">
                </div>
            </td>
        </tr>
        <tr>
            <td class="caption">
                <%= Html.LabelFor(m => m.NeedHostel)%>
            </td>
            <td>
                <%= Html.CheckBoxFor(m => m.NeedHostel, new { @class = "edit_control" })%>
            </td>
        </tr>
        <tr>
            <td></td>
            <td></td>
        </tr>
        <% } %>
    </table>

    <br />

    <div style="display: none">
        <div id="divDecisionDialog">
            <table>
                <tbody>
                    <tr>
                        <td class="caption"><span>При проверки заявления возникли ошибки</span><span style="display:none;" id="AllowSkipErrorsNotice">, если вы хотите включить заявление в принятые,
                        введите причину решения.</span><span style="display:none;" id="CriticalErrorsNotice">, включение заявления в принятые невозможно.</span><br />
                            <br />
                            <b>Ошибка:</b>
                        </td>
                    </tr>
                    <tr>
                        <td id="lbCheckDescription"></td>
                    </tr>
                    <%--<tr>
                        <td>
                            <textarea style="width: 550px" id="tbCheckDecision" cols="80" rows="6"></textarea>
                        </td>
                    </tr>--%>
                </tbody>
            </table>
        </div>
    </div>

    <br />

    <div style="display: none">
        <div id="divAcceptDialog">
            <table>
                <tbody>
                    <tr>
                        <td colspan="2" class="caption">При проверки заявления возникли ошибки, если вы хотите включить заявления в принятые,
                        введите причину решения.
                            <br />
                        </td>
                    </tr>
                    <tr id="rowForcedAdmissionReason">
                        <td style="width:200px;min-width:200px;">Причина:</td>
                        <td>
                            <select style="width: 550px"  id="selForceAdmissionReason" onchange="forceAdmissionReasonChange();">
                                <% foreach (var item in Model.ForcedAdmissionReasons)
                                   { 
                                %>
                                <option value="<%=item.Id %>"><%=item.Name %></option>
                                <%    
                                   }
                                %>
                            </select>
                        </td>
                    </tr>
                    <tr id="rowEgeResults">
                        <td style="width:200px;min-width:200px;">Данные в подсистеме "Результаты ЕГЭ" (снимок экрана/справка ЕГЭ):</td>
                        <td><form id="formEgeResults" reason-file-type="1" action="" method="post"><input type="file" name="fileEgeResults" /><span></span></form></td>
                    </tr>
                    <tr id="rowDocumentScan">
                        <td style="width:200px;min-width:200px;">Скан-копия актуального документа:</td>
                        <td><form id="formDocumentScan" reason-file-type="2" action="" method="post"><input type="file" name="fileDocumentScan" /><span></span></form></td>
                    </tr>
                    <tr id="rowFisScreenshot">
                        <td style="width:200px;min-width:200px;">Данные в ФИС ГИА и Приема (снимок экрана):</td>
                        <td><form id="formFisScreenshot" reason-file-type="3" action="" method="post"><input type="file" name="fileFisScreenshot" /><span></span></form></td>
                    </tr>
                    <tr id="rowComment">
                        <td>Комментарий:</td>
                        <td>
                            <textarea style="width: 550px" id="tbCheckDecision" cols="80" rows="6"></textarea>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>

    <br />

    <div class="ui-dialog-content ui-widget-content" id="divPopupApplication" style="width: auto; height: auto; min-height: 40px;">
        <div class="gvuzPortlet divstatement" style="top: 45px; margin-bottom: 50px;">
            <div id="appDetailsTab" class="gvuzTab submenu">
            </div>
            <div id="Wz5partialV">
            </div>
        </div>
    </div>
</div>