<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.ApplicationWz1ViewModel>" %>
<%@ Import Namespace="System.Activities.Expressions" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.Models" %>



<%=Html.Hidden("IsFromKrymEntrantDocumentID",Model.IsFromKrymEntrantDocumentID)%>


<div id="actual">

<table id="WzForm1" class="tableApp2">
    <tbody>
        <tr class="mainRow">
            <td class="caption"><%= Html.StrongLabelFor(m => m.LastName, required: true) %></td>
            <td><%= Html.TextBoxExFor(m => m.LastName, new {tabindex = 1, style = "width: 326px"}) %></td>
            <td class="caption"><%= Html.TableLabelFor(m => m.GenderID, required: true) %></td>
            <td style="width: auto" colspan="3">
                <%= Html.DropDownListExFor(m => m.GenderID, Model.GenderList, new {tabindex = 20, disabled = Model.IsEdit, @class = "gender_select"}) %>
                <span class="caption"><%= Html.TableLabelFor(m => m.BirthDate, new {style = "width:auto;margin:0px 0px !important;"}, true) %></span>
                <%= Html.DatePickerFor(m => m.BirthDate, new {tabindex = 21, disabled = Model.IsEdit}) %>
                <br />
                <div id="txtbDateError"></div>
            </td>
        </tr>
        <tr class="mainRow">
            <td class="caption"><%= Html.StrongLabelFor(m => m.FirstName, required: true) %></td>
            <td><%= Html.TextBoxExFor(m => m.FirstName, new {tabindex = 2, style = "width: 326px"}) %></td>
            <td class="caption"><%= Html.TableLabelFor(m => m.NationalityID, required: true) %></td>
            <td colspan="3"><%= Html.DropDownListExFor(m => m.NationalityID, Model.NationalityList, new {tabindex = 22, disabled = Model.IsEdit}) %></td>
        </tr>
        <tr class="mainRow">
            <td class="caption"><%= Html.StrongLabelFor(m => m.MiddleName) %></td>
            <td><%= Html.TextBoxExFor(m => m.MiddleName, new {tabindex = 3, style = "width: 326px"}) %></td>
            <td class="caption"><%= Html.TableLabelFor(m => m.BirthPlace, required: false) %></td>
            <td colspan="3"><%= Html.TextBoxExFor(m => m.BirthPlace, new {tabindex = 23, disabled = Model.IsEdit, style = "width: 326px"}) %></td>           
        </tr>
        <tr class="mainRow">
            <td class="caption"><%= Html.TableLabelFor(m => m.SNILS, required: false) %></td>
            <td colspan="3"><%= Html.TextBoxExFor(m => m.SNILS, new {tabindex = 23, disabled = Model.IsEdit, style = "width: 326px"}) %></td>
        </tr>
        <tr class="mainRow">
            <td class="caption"><%= Html.TableLabelFor(m => m.DocumentTypeID, new {style = "padding-top:0px;padding-bottom:10px;"}, required: true) %></td>
            <td><%= Html.DropDownListExFor(m => m.DocumentTypeID,Model.IdentityDocumentList,new { tabindex=4,disabled=Model.IsEdit })%>            </td>                   
            <%--здесь должен редактироваться почтовый адрес--%>

            <td class="caption"><label>Наименования государств, гражданином которых является абитуриент </label></td>
               
           <td><%=Html.ListBoxFor(m => m.SelectedCitizenships, new SelectList(Model.NationalityList, "ID", "Name", Model.SelectedCitizenships), new { @class = "chosen", multiple = "multiple" })%> </td>
        </tr>

        <tr>
             <td class="caption"><%= Html.TableLabelFor(m => m.DocumentNumber, required: true) %> </td>
            <td>
                <span style="width: 326px;">
                    <%= Html.TextBoxExFor(m => m.DocumentSeries, new {@class = "passSeries", tabindex = 10, disabled = Model.IsEdit || Model.DisableDocumentDataEditing}) %>
                    <%= Html.TextBoxExFor(m => m.DocumentNumber, new {@class = "passNumber", tabindex = 11, disabled = Model.IsEdit || Model.DisableDocumentDataEditing}) %>
                </span>
            </td>
             <td class="caption">              
                <%= Html.TableLabelFor(m => m.RegionID, new {style = "padding-top:0px;padding-bottom:10px;padding-left:40px"}, required: true) %></td>
            <td><%= Html.DropDownListExFor(m => m.RegionID,Model.Regions, new { tabindex=24,disabled=Model.IsEdit })%></td>
            </tr>

        <tr class="mainRow">
            <td class="caption"><%= Html.TableLabelFor(m => m.DocumentOrganization) %></td>
            <td><%= Html.TextBoxFor(m => m.DocumentOrganization,new { tabindex=12, style = "width: 326px" })%></td>
            <%-- disabled=Model.IsEdit УБРАЛ--%>
           
            <td class="caption"><%= Html.TableLabelFor(m => m.TownTypeID, new {style = "padding-top:0px;padding-bottom:10px;"}, required: true) %></td>
            <td><%= Html.DropDownListExFor(m => m.TownTypeID,Model.TownTypes, new { tabindex=25,disabled=Model.IsEdit })%></td>
           
        </tr>


        <tr class="mainRow">
            <td class="caption"><%= Html.TableLabelFor(m => m.DocumentDate, required: true) %></td>
            <td><%= Html.DatePickerFor(m => m.DocumentDate, new { tabindex = 13, disabled = Model.IsEdit })%>
                <span class="caption"><%= Html.TableLabelFor(m => m.SubdivisionCode,new { style="width:140px;margin:0px 0px !important;display:inline-block;position:relative" })%></span>
                <%= Html.TextBoxFor(m => m.SubdivisionCode, new { tabindex = 14, @class = "", style = "width:60px;" })%><br />
                <div id="txtDateError"></div>
            </td>
            <%--disabled = Model.IsEdit, УБРАЛ--%>
            <td></td>
            <td colspan="3"></td>
        </tr>

        <tr class="mainRow">
            <td class="caption">
                <%= Html.TableLabelFor(m => m.ReleaseCountryID, required: true) %>     
            </td>
           
            <td>
                <%= Html.DropDownListExFor(m => m.ReleaseCountryID, Model.ReleaseCountryList, new {tabindex = 22, disabled = Model.IsEdit}) %>               
            </td>          

            <td class="caption"><%= Html.TableLabelFor(m => m.Address, required: true) %></td>           
        </tr>


        <tr class="mainRow">
             <td class="caption">  <%= Html.TableLabelFor(m => m.ReleasePlace, required: true) %></td>

             <td colspan="2" ><%= Html.TextAreaFor(m => m.ReleasePlace, new {tabindex = 26, @class = "app"}) %></td>
            <td ><%= Html.TextAreaFor(m => m.Address, new {tabindex = 26, @class = "app"}) %></td>
        </tr>


        <tr class="mainRow">
            <td class="caption"><%= Html.TableLabelFor(m => m.AttachmentFileID,required :false)%></td>
            <td id='FileLabel'><%= Url.GenerateLinkIf<EntrantController>(x => x.GetFile1(Model.AttachmentFileID), Model.DocumentAttachmentName,Model.AttachmentFileID!=Guid.Empty,Model.IsEdit?"Отсутствует":"")%>
                <%= Model.IsEdit ? "" : Html.FileForm("", new {tabindex = 15}) %></td>
                     <%=Html.Hidden("EntrantID", Model.EntrantID) %>

             <td class="caption"><%= Html.TableLabelFor(m => m.Email) %></td>
            <td><%= Html.TextBoxFor(m => m.Email,new { tabindex=27, style = "width: 326px" })%></td>
        </tr>


        <tr>
            <td colspan="3"></td>
            <td>
                <div style="width: 60%; margin-left: 0px;"><%= Html.CheckBoxFor(m => m.IsFromKrym,new { tabindex=28, onclick="ChektIsFromKrym();" })%> является лицом, признанным гражданином, или лицом постоянно проживавшим на территории Крыма</div>
            </td>
        </tr>

        <tr>
            <td colspan="3"></td>
            <td>
                <div id="doclinks">
                    <a href="#" style="padding-right: 15px;" onclick="SelectorDocument_();">Подтверждающий документ</a>
                    <span id="krymdocnumber"></span>
                </div>
            </td>
        </tr>

        <tr>
              <%--<td class="caption" colspan="3" valign="top"><%= Html.TableLabelFor(m => m.EducationTerm, required: true) %></td>
              <td colspan="3"><%= Html.TextBoxFor(m => m.EducationTerm, new {tabindex = 5, style = "width:60px;"}) %></td>--%>
        </tr>

<%--        <tr class="mainRow" >

             <td class="caption" colspan="3">
                 <%= Html.TableLabelFor(m => m.DateStartEdu, required: true) %>                 
             </td>
            <td id="eduDates"> 
                <%= Html.DatePickerFor(m => m.DateStartEdu, new { tabindex = 13, disabled = Model.IsEdit })%>
                <span>
                     <%= Html.TableLabelFor(m => m.DateEndEdu, required: true) %>
                    <%= Html.DatePickerFor(m => m.DateEndEdu, new { tabindex = 14, disabled = Model.IsEdit })%>
                 </span>
            </td>
                              
        </tr>--%>

        <tr>
            <td class="caption" colspan="3" valign="top"><%= Html.TableLabelFor(m => m.CustomInformation) %></td>
            <td colspan="3"><%= Html.TextAreaFor(m => m.CustomInformation, new {tabindex = 29, @class = "app notdisabled"}) %></td>
        </tr>
    </tbody>
</table>

<table id="EntityDocs" class="gvuzDataGrid tableStatement2" cellpadding="3">
    <thead>
        <tr>
            <th>Вид документа</th>
            <th>Серия и номер документа</th>
            <th>Дата выдачи</th>
            <th>Кем выдан</th>
            <th style="width: 40px"></th>
        </tr>
    </thead>
    <tbody id="EntityDocsTBody">
        <tr id="trEntityDocsAddNew" style="display: none"></tr>
    </tbody>
</table>

<div style="margin-top: 5px; margin-bottom: 5px">
    <input type="button" id="btnAddNewDocument" value="Прикрепить новый документ" onclick="EditDocument(0, 1);" />
</div>

</div>

<script type="text/javascript">

    var selectedCitz = [];
    <% foreach (var item in Model.SelectedCitizenships) {%>
    selectedCitz.push('<%=item%>')
 <% } %>


    $(".chosen").chosen({ max_selected_options: 10, width: '340px', placeholder_text: "Выберите одну или несколько стран" });
    $(".chosen-deselect").chosen({ allow_single_deselect: true });
    $(".default").css({"width": "300px"});
    $("#SelectedCitizenships").val(selectedCitz).trigger("chosen:updated");

    $('#SelectedCitizenships').change(function () {  
        var citizenship = $('#SelectedCitizenships').val();
        setReleasePlace(citizenship);
       
    });

    var releasePlace =  $('#ReleasePlace').val();
    if(!releasePlace) {
        setReleasePlace(1);
    }

    function setReleasePlace (citizenship) {
        if(citizenship == 1)
        {
            $('#ReleasePlace').val("Россия");

        } else if(citizenship)
        {
            for (var i = 0; i < citizenship.length; i++) {
                if(i == 1)
                {
                    $('#ReleasePlace').val("Россия");
                }     
            }
        }
    }

        // признак наличия документов
    var haveDocuments = false;
    var StatusId=<%=Model.StatusID %>;

    function ChektIsFromKrym() {
        var checked = $('#IsFromKrym').is(":checked");
        if(checked) {
            GetDocInfo_($('#IsFromKrymEntrantDocumentID').val());
            $('#doclinks').show();
        }
        else
            $('#doclinks').hide();


    }

    function Wz1Init() {
        //$("#BirthDate").datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-62:+0' });
        //Фикс бага для хрома (DONOTOUCH)
        $("#BirthDate, #DocumentDate, #DateStartEdu, #DateEndEdu").datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-100:+0', maxDate: new Date() });


        if ('<%=Model.BirthDate.HasValue ? Model.BirthDate.Value.ToString("d") : "" %>' == '<%= DateTime.Now.ToString("d") %>') {
            var Year = new Date().getFullYear();
            Year = Year - 17;
            $("#BirthDate").val('01' + '.' + '01' + '.' + Year);
        }
        $('#DocumentTypeID').change(function () {
            Wz1GetModel()
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

       

        $('#DocumentTypeID').change();

        //        if (!(navigator.userAgent.indexOf('Chrome') != -1)) {
        //            $('#BirthDate').datepicker("option", "defaultDate", '-17y');
        //            $('#DocumentDate').datepicker("option", "maxDate", new Date());
        //        }
        $('#btnAddNewDocument').button();
        //$('#DocumentSeries').attr('val-required', $('#DocumentNumber').attr('val-required'));

        $("input#SubdivisionCode").mask("999-999");
        WzObjs[1] = Wz1GetModel();
        getEntrantDocuments(WzObjs[1].EntrantID);


        ChektIsFromKrym();

        if (StatusId == 4 || StatusId == 6) {
            $("#actual").find("input, select, textarea")
                .not(".notdisabled")
                .not("#btnAppSaveE, #btnAppCancelE, #btnAppSaveETop, #btnAppCancelETop")
                .attr("disabled", "disabled");

            $("a").removeClass("btnEdit");
            $("#doclinks").hide();
        }
    }

    function Wz1GetModel() {
        var model = {
            ApplicationID: ApplicationId,
            InstitutionID: InstitutionID,
            EntrantID: $('#EntrantID').val(),
            LastName: $('#LastName').val(),
            FirstName: $('#FirstName').val(),
            MiddleName: $('#MiddleName').val(),
            SNILS: $('#SNILS').val(),
            GenderID: $('#GenderID').val(),
            BirthDate: $('#BirthDate').val(),
            DocumentTypeID: $('#DocumentTypeID').val(),
            DocumentSeries: $('#DocumentSeries').val(),
            DocumentNumber: $('#DocumentNumber').val(),
            DocumentOrganization: $('#DocumentOrganization').val(),
            DocumentDate: $('#DocumentDate').val(),
            SubdivisionCode: $('#SubdivisionCode').val(),
            NationalityID: $('#NationalityID').val(),
            BirthPlace: $('#BirthPlace').val(),
            CustomInformation: $('#CustomInformation').val(),
            postFile: $('#postFile').val(),
            ReleaseCountryID: $('#ReleaseCountryID').val(),
            ReleasePlace: $('#ReleasePlace').val(),
            SelectedCitizenships: $('#SelectedCitizenships').val(),

            Email: $('#Email').val(),
            RegionID: $('#RegionID').val(),
            TownTypeID: $('#TownTypeID').val(),
            Address: $('#Address').val(),
            IsFromKrym: $('#IsFromKrym').is(":checked"),
            IsFromKrymEntrantDocumentID: $('#IsFromKrymEntrantDocumentID').val(),
        };
        return model;
    }
    function Wz1PrepareModel() {
        if (Wz1CheckModel()) {
            return Wz1GetModel();
        }
        return null;
    }

    function Wz1CheckModel(model) {

        var res = true;
        var isError = false;

        if (revalidatePage($('#WzForm1'))) {
            isError = true;
        }

        $('#GenderID').removeClass('input-validation-error-fixed');
        if (jQuery('#GenderID').val() == "0") {
            $('#GenderID').addClass('input-validation-error');
            isError = true;
        }

        if (isError) {
            return false;
        }

        jQuery('#NationalityID').removeClass('input-validation-error-fixed');
        jQuery('#DocumentTypeID').removeClass('input-validation-error-fixed');
        jQuery('#DocumentSeries').removeClass('input-validation-error-fixed');
        var listIdentityDocumentType = JSON.parse('<%= Html.Serialize(Model.ListIdentityDocumentType) %>');
        var resul = $.grep(listIdentityDocumentType, function (v) {
            return v.IdentityDocumentTypeId === parseInt(jQuery('#DocumentTypeID').val());
        });
        if ((resul[0].IsRussianNationality) & (jQuery('#NationalityID').val() == 1)) {
            if (jQuery('#DocumentSeries').val() == "") {
                jQuery('#DocumentSeries').addClass('input-validation-error');
                jQuery('#NationalityID').removeClass('input-validation-error-fixed');
                jQuery('#DocumentTypeID').removeClass('input-validation-error-fixed');
                return;
            }
            jQuery('#DocumentSeries').removeClass('input-validation-error-fixed');
        }
        if ((!resul[0].IsRussianNationality) & (jQuery('#NationalityID').val() != 1)) {
            jQuery('#DocumentSeries').removeClass('input-validation-error-fixed');
        }

        if ((resul[0].IdentityDocumentTypeId != 9) && (resul[0].IdentityDocumentTypeId != 10)) {
            if ((resul[0].IsRussianNationality) & (jQuery('#NationalityID').val() != 1)) {
                jQuery('#NationalityID').addClass('input-validation-error');
                jQuery('#DocumentTypeID').addClass('input-validation-error');
                jQuery('#DocumentSeries').removeClass('input-validation-error-fixed');
                return;
            }
            if ((!resul[0].IsRussianNationality) & (jQuery('#NationalityID').val() == 1)) {
                jQuery('#NationalityID').addClass('input-validation-error');
                jQuery('#DocumentTypeID').addClass('input-validation-error');
                jQuery('#DocumentSeries').removeClass('input-validation-error-fixed');
                return;
            }
        }
        //
        if (jQuery('#FirstName').val() == "") {
            $('#FirstName').addClass('input-validation-error');
            res = false;
            isError = true;
        }
        else {
            $('#FirstName').removeClass('input-validation-error-fixed');
            res = true;
            isError = false;
        }
        //
        if (jQuery('#LastName').val() == "") {
            $('#LastName').addClass('input-validation-error');
            res = false;
            isError = true;
        }
        else {
            $('#LastName').removeClass('input-validation-error-fixed');
            res = true;
            isError = false;
        }
        //
        
        //
        if (jQuery('#BirthDate').val() == "") {
            $('#BirthDate').addClass('input-validation-error');
            res = false;
            isError = true;
        }
        else {
            $('#BirthDate').removeClass('input-validation-error-fixed');
            res = true;
            isError = false;
        }
        //
        if (jQuery('#DocumentNumber').val() == "") {
            $('#DocumentNumber').addClass('input-validation-error');
            res = false;
            isError = true;
        }
        else {
            $('#DocumentNumber').removeClass('input-validation-error-fixed');
            res = true;
            isError = false;
        }
        //
        if (jQuery('#DocumentDate').val() == "") {
            $('#DocumentDate').addClass('input-validation-error');
            res = false;
            isError = true;
        }
        else {
            $('#DocumentDate').removeClass('input-validation-error-fixed');
            res = true;
            isError = false;
        }

        if(jQuery('#DateStartEdu').val() > jQuery('#DateEndEdu').val())
        {
            $('#DateStartEdu').addClass('input-validation-error');
            res = false;
            isError = true;
        } else{
            $('#DateStartEdu').removeClass('input-validation-error-fixed');
            res = true;
            isError = false;
        }

        if (jQuery('#ReleaseCountryID').val() == "") {
            $('#ReleaseCountryID').addClass('input-validation-error');
            res = false;
            isError = true;
        }
        else {
            $('#ReleaseCountryID').removeClass('input-validation-error-fixed');
            res = true;
            isError = false;
        }

        if (jQuery('#ReleasePlace').val() == "") {
            $('#ReleasePlace').addClass('input-validation-error');
            res = false;
            isError = true;
        }
        else {
            $('#ReleasePlace').removeClass('input-validation-error-fixed');
            res = true;
            isError = false;
        }

        //валидация адресных данных

        var email = $('#Email').val();
        var region = $('#RegionID').val();
        var town = $('#TownTypeID').val();
        var address = $('#Address').val();
        var krym = $('#IsFromKrym').is(":checked");
        var krymDoc = $('#IsFromKrymEntrantDocumentID').val();

        if (StatusId != 4 && StatusId != 6) {
            var documentTypeId = $('#DocumentTypeID').val();
            if(documentTypeId==3)
            {
                if (email == "" && address == "") {
                    infoDialog("Обязательным является указание или почтового адреса, или электронного адреса.");
                    res = false;
                }
            }
            else 
            {
                if (email == "" && (region == "1000" || town == "0" || address == "")) {
                    infoDialog("Обязательным является указание или почтового (в составе полей: региона, типа населенного пункта и адреса), или электронного адреса.");
                    res = false;
                }
            }
        }

        if (krym && krymDoc == "") {
            infoDialog("Задание документа является обязательным, если введен признак 'Является лицом, признанным гражданином, или лицом постоянно проживавшим на территории Крыма'.");
            res = false;
        }

        return res;
    }

    function Wz1Save(success, error, step) {
        $('.entrantFullName').text('');
        $('.entrantFullName').append('<span>Абитуриент:&nbsp;</span>' + $('#LastName').val() + ' ' + $('#FirstName').val() + ' ' + $('#MiddleName').val());
        var model = Wz1PrepareModel();
        if (model == null) { return false; }
        if (WizStep != step) {
            if (Wz1HasChanged(WzObjs[1], model)) {
                if (window.switchAppTab) {
                    confirmDialog("Есть несохраненные изменения. Желаете сохранить?", function () { Wz1SaveX(success, error, step); }, function () { Wz1SaveX(success, error, step, true); });
                } else if (window.ShowWz) {
                    Wz1SaveX(success, error, step);
                }
            } else {
                Wz1SaveX(success, error, step);
            }
        } else {
            Wz1SaveX(success, error, step);
        }
    }

    function Wz1SaveX(success, error, step, noSave) {
        if (step == undefined) { step = 1; }
        var model = Wz1PrepareModel();
        if (model == null) { return false; }
        model.WizardStepID = step;
        if (!Wz1HasChanged(WzObjs[1], model) || noSave) {
            //blockUI(); unblockUI();
            doPostAjaxSync('<%= Url.Generate<ApplicationController>(x => x.SetWzStep(null,null)) %>', JSON.stringify({ ApplicationID: ApplicationId, Step: step }),
                function (data) {
                    if (!data.IsError) { if (success) { success(model); } } else { if (error) { error("При сохранении произошла ошибка."); } }
                });
            return true;
        }
        if (model.postFile == '') {
            Wz1SubmitData(model, success, error, step);
        } else {
            $('#fileForm').ajaxSubmit({
                method: 'POST',
                dataType: 'json',
                url: "/Application/ReceiveFile1",
                error: showAsyncFilePostError,
                success: function (data) {
                    if (isFilePostError($('#postFile'), data)) { return; }
                    model.AttachmentFileID = data.FileID;
                    model.DocumentAttachmentName = model.postFile.substring(model.postFile.lastIndexOf('\\') + 1, model.postFile.length);
                    model.postFile = '';
                    Wz1SubmitData(model, success, error, step);
                }
            });
        }
    }
    function isFilePostError($control, data) {
        if (data.BigSize) {
            unblockUI();
            addFileValidationError($control);
            return true;
        }
        return false;
    }
    function addFileValidationError($control) {
        addValidationError($control, 'Размер файла превышает максимальный разрешенный размер в <%= Model.MaxFileSize %>Кб', true);
    }
    function Wz1SubmitData(model, success, error, step) {
        $('#txtbDateError span').text("");
        $('#txtDateError span').text("");
        doPostAjaxSync('<%= Url.Generate<ApplicationController>(x => x.UpdWz1(null)) %>', JSON.stringify(model),
            function (data) {
                
                if (!data.IsError) {
                    if (data.Extra != null) {

                        $('#BirthDate').removeClass('input-validation-error-fixed');
                        $('#DocumentDate').removeClass('input-validation-error-fixed');

                        if ((data.Extra[0] == "BirthDateError") & (data.Extra[1] == "dDateError")) {
                            $('#BirthDate').addClass('input-validation-error');
                            $('#DocumentDate').addClass('input-validation-error');
                            $('#txtbDateError').html('<span class="field-validation-error">Введено некорректное значение</span>');
                            $('#txtDateError').html('<span class="field-validation-error">Введено некорректное значение</span>');
                            return;
                        }

                        if (data.Extra[0] == "BirthDateError") {
                            $('#BirthDate').addClass('input-validation-error');
                            $('#txtbDateError').html('<span class="field-validation-error">Введено некорректное значение</span>');
                            return;
                        }

                        if (data.Extra[1] == "dDateError") {
                            $('#DocumentDate').addClass('input-validation-error');
                            $('#txtDateError').html('<span class="field-validation-error">Введено некорректное значение</span>');
                            return;
                        }
                    }

                    WzObjs[1] = model;
                    $('#FileLabel').find('a').remove(); //.attr('href','/Entrant/GetFile1?fileID='+model.AttachmentFileID).text(model.DocumentAttachmentName);
                    if (model.AttachmentFileID != undefined && model.DocumentAttachmentName != undefined) {
                        $('#postFile').before('<a href="/Entrant/GetFile1?fileID=' + model.AttachmentFileID + '" >' + model.DocumentAttachmentName + '</a><br/>');
                    }
                    $('#postFile').val('');
                    if (success) { success(model); }
                } else {
                    if (error) { error("При сохранении произошла ошибка."); }
                }
            });
    }
    function doSubmitFile() {
        blockUI();
        if (fileID != null || $('#postFile').val() == '') {
            doSubmitData();
            return;
        }
    }

    function Wz1Cancel() {
        var model = WzObjs[1];
        if (model == undefined) { return; }
        try {
            //ApplicationID: ApplicationId,
            //InstitutionID: InstitutionID,
            $('#LastName').val(model.LastName);
            $('#FirstName').val(model.FirstName);
            $('#MiddleName').val(model.MiddleName);
            $('#SNILS').val(model.SNILS);
            $('#GenderID').val(model.GenderID);
            $('#BirthDate').val(model.BirthDate);
            $('#DocumentTypeID').val(model.DocumentTypeID);
            $('#DocumentSeries').val(model.DocumentSeries);
            $('#DocumentNumber').val(model.DocumentNumber);
            $('#DocumentOrganization').val(model.DocumentOrganization);
            $('#DocumentDate').val(model.DocumentDate);
            $('#SubdivisionCode').val(model.SubdivisionCode);
            $('#NationalityID').val(model.NationalityID);
            $('#BirthPlace').val(model.BirthPlace);
            $('#CustomInformation').val(model.CustomInformation);
            $('#ReleaseCountryID').val(model.ReleaseCountryID);
            $('#ReleasePlace').val(model.ReleasePlace);
            $('#postFile').val('');
            $("#SelectedCitizenships").val(model.SelectedCitizenships);

            $('#Email').val(model.Email);
            $('#RegionID').val(model.RegionID);
            $('#TownTypeID').val(model.TownTypeID);
            $('#Address').val(model.Address);
            $('#IsFromKrym').val(model.IsFromKrym);
            $('#IsFromKrymEntrantDocumentID').val(model.IsFromKrymEntrantDocumentID);

        } catch (e) {

        }
    }
    function Wz1HasChanged(oldModel, newModel) {
        // Проверка изменений
        if (oldModel != undefined && newModel != null) {
            if (oldModel.ApplicationID != newModel.ApplicationID) { return true; }
            if (oldModel.InstitutionID != newModel.InstitutionID) { return true; }
            if (oldModel.LastName != newModel.LastName) { return true; }
            if (oldModel.FirstName != newModel.FirstName) { return true; }
            if (oldModel.MiddleName != newModel.MiddleName) { return true; }
            if (oldModel.SNILS != newModel.SNILS) { return true; }
            if (oldModel.GenderID != newModel.GenderID) { return true; }
            if (oldModel.BirthDate != newModel.BirthDate) { return true; }

            if (oldModel.DocumentTypeID != newModel.DocumentTypeID) { return true; }
            if (oldModel.DocumentSeries != newModel.DocumentSeries) { return true; }
            if (oldModel.DocumentNumber != newModel.DocumentNumber) { return true; }
            if (oldModel.DocumentOrganization != newModel.DocumentOrganization) { return true; }
            if (oldModel.DocumentDate != newModel.DocumentDate) { return true; }
            if (oldModel.SubdivisionCode != newModel.SubdivisionCode) { return true; }

            if (oldModel.SubdivisionCode != newModel.SubdivisionCode) { return true; }
            if (oldModel.NationalityID != newModel.NationalityID) { return true; }
            if (oldModel.BirthPlace != newModel.BirthPlace) { return true; }
            if (oldModel.CustomInformation != newModel.CustomInformation) { return true; }
            if (oldModel.ReleaseCountryID != newModel.ReleaseCountry) { return true; }
            if (oldModel.ReleasePlace != newModel.ReleasePlace) { return true; }
            if (oldModel.postFile != newModel.postFile) { return true; }
            if (oldModel.SelectedCitizenships != newModel.SelectedCitizenships) { return true; }           

            if (oldModel.Email != newModel.Email) { return true; }
            if (oldModel.RegionID != newModel.RegionID) { return true; }
            if (oldModel.TownTypeID != newModel.TownTypeID) { return true; }
            if (oldModel.Address != newModel.Address) { return true; }
            if (oldModel.IsFromKrym != newModel.IsFromKrym) { return true; }
            if (oldModel.IsFromKrymEntrantDocumentID != newModel.IsFromKrymEntrantDocumentID) { return true; }
            
        } else {
            return true;
        }
        return false;
    }

</script>

<script type="text/javascript">
    function EditDocument(EntrantDocumentID, DocTypeID) {
        var EntrantID = <%= Model.EntrantID %>;
        var $edDialog = null;
        $edDialog = $('#Wz1UDialog');

        if ($edDialog.length == 0) {
            $edDialog = $('<div id="Wz1UDialog" style="display:none, position:fixed"></div>');
            $('body').append($edDialog);
        }

        if (DocTypeID == undefined) {
            DocTypeID = 15;
        }

        doPostAjax('<%= Url.Generate<EntrantController>(x => x.getEditDocument(null, null, null)) %>', 
            { EntrantDocumentID: EntrantDocumentID, DocTypeID: DocTypeID, EntrantID: EntrantID }, function (data) {

            $edDialog.html(data);
            UniDFormInit();
            $edDialog.dialog({
                modal: true,
                width: 800,
                title: "Дополнительный документ",
                buttons: {
                    "Сохранить": function () {

                        var baseModel = { EntrantID: $('#EntrantID').val() };
                        var doc = UniDPrepareModel(baseModel);
                        if (doc != null || doc != undefined) {

                            // дата рождения основного документа совпадает с датой рождения дополнительного документа (предупредительного характера)
                            if(doc.DocumentTypeID == 1 && doc.EntDocIdentity != null && doc.EntDocIdentity.BirthDate != $('#BirthDate').val())
                                infoDialog("Дата рождения в документе и в заявлении не совпадают!");

                            // Сохранение, если соханилось нормальн то закрыть и обновить.
                            UniDSave(doc, function (model) {
                                var d = {};
                                d.EntrantDocumentID = doc.EntrantDocumentID;
                                if (DocTypeID == 1)
                                    d.DocumentTypeName = doc.EntDocIdentity.IdentityDocumentTypeName;
                                else
                                    d.DocumentTypeName = 'Иной документ';
                                d.DocumentSeriesNumber = (doc.DocumentSeries + ' ' + doc.DocumentNumber);
                                d.DocumentDate = doc.DocumentDate;
                                d.DocumentOrganization = doc.DocumentOrganization;
                                renderDocument($('#trEntityDocsAddNew'), d);

                                // обрабатываем признак, который был установлен, если вводили документ о крыме
                                if (krymDocument) {
                                    $('#IsFromKrymEntrantDocumentID').val(d.EntrantDocumentID);
                                    krymDocument = false;
                                }

                                // Обновляем данные по model
                                closeDialog($edDialog);
                            }, function (e) {
                                infoDialog("Не удалось сохранить документ! " + e);
                            });
                        } else {

                        }
                    },
                    "Закрыть": function () { $(this).dialog('close'); },
                },
                close: function () {
                    $edDialog.remove();
                }
            }).dialog('open');
        }, "application/x-www-form-urlencoded", "html");
    }

    //-- Открываем диалог просмотра документа --
    function ViewDocument(EntrantDocumentID) {
        var $vdDialog = null;
        $vdDialog = $('#Wz1UDialog');

        if ($vdDialog.length == 0) {
            $vdDialog = $('<div id="Wz1UDialog" style="display:none, position:fixed"></div>');
            $('body').append($vdDialog);
        }

        doPostAjax('<%= Url.Generate<EntrantController>(x => x.getViewDocument(null)) %>', { EntrantDocumentID: EntrantDocumentID, DocTypeID: 1 }, function (data) {
            $vdDialog.html(data);
            $vdDialog.dialog({
                modal: true,
                width: 800,
                title: "Дополнительный документ",
                buttons: { "Закрыть": function () { $(this).dialog('close'); } },
                close: function () { }
            }).dialog('open');
        }, "application/x-www-form-urlencoded", "html");

    }
</script>

<script type="text/javascript">
    function getEntrantDocuments(EntrantID) {
        doPostAjaxSync('<%= Url.Generate<EntrantController>(x => x.getEntrantDocuments(null)) %>', JSON.stringify({ EntrantID: EntrantID }),
            function (data) {
                if (!data.IsError) {
                    for (i = 0; i < data.Data.length; i++) {
                        renderDocument($('#trEntityDocsAddNew'), data.Data[i], 0);
                    }
                    if (data.Data.length == 0) {
                        $('#EntityDocs').hide();
                    } else {
                        $('#EntityDocs').show();
                    }

                } else { // Обработка ошибки.
                    res = false;
                    infoDialog("Не удалось получить список документов. "); //  + data.Message
                }
            });
    }

    //-- добавление нового документа --
    function renderDocument($trBefore, item) {

        haveDocuments = true;

        var iTr = $trBefore.parent().find('tr[itemID=' + item.EntrantDocumentID + ']');
        if (iTr.length > 0) {// Обновление 
            updTrDocument($(iTr), item);
        } else {// Добавление
            var className = $trBefore.prev().attr('class');
            var res = renderTrDocument(item, className);
            $trBefore.before(res);
            $('#EntityDocs').show();
        }
    }
    function renderTrDocument(item, className) {
        if (className == 'trline1') { className = 'trline2'; } else { className = 'trline1'; }
        var res = '<tr itemID="' + item.EntrantDocumentID + '" class="' + className + '">'
            + '<td><span class="linkSumulator" onclick="ViewDocument(' + item.EntrantDocumentID + ')">' + escapeHtml(item.DocumentTypeName) + '</a></td>'
            + '<td>' + escapeHtml(item.DocumentSeriesNumber) + '</td>'
            + '<td>' + item.DocumentDate + '</td>'
            + '<td>' + escapeHtml(item.DocumentOrganization == null ? '' : item.DocumentOrganization) + '</td>'
            + '<td align="center"><a href="#" title="Редактировать документ" class="btnEdit" onclick="EditDocument(' + item.EntrantDocumentID + ', ' + item.DocumentTypeID + ');return false;"></a></td>'
            + '</tr>';
        return res;
    }
    function updTrDocument($iTr, item) {
        var trin = '<td><span class="linkSumulator" onclick="ViewDocument(' + item.EntrantDocumentID + ')">' + escapeHtml(item.DocumentTypeName) + '</a></td>'
            + '<td>' + escapeHtml(item.DocumentSeriesNumber) + '</td>'
            + '<td>' + item.DocumentDate + '</td>'
            + '<td>' + escapeHtml(item.DocumentOrganization == null ? '' : item.DocumentOrganization) + '</td>'
            + '<td align="center"><a href="#" title="Редактировать документ" class="btnEdit" onclick="EditDocument(' + item.EntrantDocumentID + ', ' + item.DocumentTypeID + ');return false;"></a></td></tr>';

        $iTr.html(trin);
    }
</script>

<script type="text/javascript">
    // признак ввода документа о крыме
    var krymDocument = false;

    function GetDocInfo_(docId) {
        doPostAjax('<%= Url.Generate<ApplicationController>(x => x.GetDocInfo(null)) %>',
            JSON.stringify({ docId: docId }),
            function (data) {
                if (data.IsError) {
                    return;
                }
                $('#krymdocnumber').html(data.Data);
            }
        );
    }

    function SelectorDocument_() {
        DialogWz1();
        SelectDocsList_(null, 106, 1, null);
    }

    function doETDocumentSelect_(EntrantDocumentID, EntranceTestItemID, GroupID, SourceID, rq, success) {
        closeDialog($('#divAddETDocumentWZ1'));
        krymDocument = true;
        $('#IsFromKrymEntrantDocumentID').val(EntrantDocumentID);
        GetDocInfo_(EntrantDocumentID);
    }

    function doETDocumentAdd_(DocumentTypeID, EntranceTestItemID, GroupID, SourceID) {
        closeDialog($('#divAddETDocumentWZ1'));
        krymDocument = true;
        EditDocument(0, DocumentTypeID);
    }

    function SelectDocsList_(EntranceTestItemID, SourceID, GroupID, selecedETID) {
        doPostAjax('<%= Url.Generate<ApplicationController>(x => x.SelectEntDocsList(null,null,null,null)) %>',
            JSON.stringify({
                ApplicationID: ApplicationId, EntranceTestItemID: EntranceTestItemID,
                SourceID: SourceID, GroupID: GroupID
            }),

            function (data) {
                if (data.IsError) {
                    infoDialog(data.Message.replace(/\n/g, '<br/>'));
                    return;
                }
                var ds = data.Data;
                if (ds.EntrantDocuments.length == 0 && ds.DocumentTypes.length == 1) {
                    // Если существующих подходящих документов нет, то 
                    // Если тип только один, то сразу предлагаем новый документ
                    doETDocumentAdd_(ds.DocumentTypes[0].ID, EntranceTestItemID, GroupID, SourceID);
                } else {
                    // Составить список документов
                    var $dts;
                    var htm = '';
                    $dts = $('#divAddETDocumentWZ1').find('#divAddDocumentTypes_');
                    $dts.empty();
                    for (var i = 0; i < ds.DocumentTypes.length; i++) {
                        htm += '<span class="linkSumulator" doctypeid="' + ds.DocumentTypes[i].ID + '" onclick="doETDocumentAdd_(' + ds.DocumentTypes[i].ID + ', ' + EntranceTestItemID + ',' + GroupID + ',' + SourceID + ')" >' + ds.DocumentTypes[i].Name + '</span><br/>';
                    }
                    $dts.html(htm);
                    $('#divAddDocumentPart_').show();

                    $dts = $('#divAddETDocumentWZ1').find('#divExistingDocumentTypes_');
                    $dts.empty();
                    htm = '';
                    for (var i = 0; i < ds.EntrantDocuments.length; i++) {
                        htm += '<span class="linkSumulator" docid="' + ds.EntrantDocuments[i].EntrantDocumentID + '" onclick="doETDocumentSelect_(' + ds.EntrantDocuments[i].EntrantDocumentID + ', ' + EntranceTestItemID + ',' + GroupID + ',' + SourceID + ')" >' + ds.EntrantDocuments[i].Description + '</span><br/>';
                    }
                    $dts.html(htm);
                    if (ds.EntrantDocuments.length > 0) {
                        $('#divExistingDocumentPart_').show();
                    } else {
                        $('#divExistingDocumentPart_').hide();
                    }
                    $('#divAddETDocumentWZ1').dialog({ modal: true, width: 600, title: 'Выбор документа' }).dialog('open');
                }
            }
        );
    }

    function DialogWz1() {
        var $Dialog = null;
        $Dialog = $('#divAddETDocumentWZ1');
        if ($Dialog.length == 0) {
            $Dialog = $('<div id="divAddETDocumentWZ1" style="display:none, position:fixed">' +
                '<div id="divCurrentDocumentPart_" style="display: none">' +
                '<b>Выбранный документ:</b>' +
                '<div id="divCurrentDocumentTypes_" style="padding-left: 20px; padding-top: 5px; padding-bottom: 15px">' +
                '</div>' +
                '</div>' +
                '<div id="divAddDocumentPart_" style="display: none">' +
                '<b>Добавить документ:</b>' +
                '<div id="divAddDocumentTypes_" style="padding-left: 20px; padding-top: 5px; padding-bottom: 15px">' +
                '</div>' +
                '</div>' +
                '<div id="divExistingDocumentPart_" style="display: none">' +
                '<b>Выбрать существующий документ:</b>' +
                '<div id="divExistingDocumentTypes_" style="padding-left: 20px; padding-top: 5px; padding-bottom: 15px">' +
                '</div>' +
                '</div>' +
                '</div>');
            $('body').append($Dialog);
        }
        return $Dialog;
    }
</script>

