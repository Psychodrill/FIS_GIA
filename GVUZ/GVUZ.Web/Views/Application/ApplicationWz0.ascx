<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.ApplicationWz0ViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

<style hidden="hidden">
    .chosen-container-multi .chosen-choices li {
        float: none;
        list-style: none;
    }
</style>

<script type="text/javascript">

    // Скрипты для визарда 0
    var groupByCampaign = [];
    var tmpID = 0;

    // изменение в комбо Кампания
    function selectCampaign() {
        $("#compGroupComplete").val('');
        $('#dirComplete').html('');
        $('#divCompGroups').html('');

        $("#conditions").empty(); // очистка условий

        var campaignId = $('#CampaignID').val();
        getAdmissionItemTypeByCampaign(campaignId);

        var educationLevelId = $('#ItemTypeID').val();
        getGroupByCampaign(campaignId, educationLevelId);

        //getCompetitiveGroupItems();

        getCampaignTypeId(campaignId);
        toggleColumnCampaignTypeId();
    }

    var campaignTypeID = 0;

    // получение информации о типе ПК
    function getCampaignTypeId(campaignId) {
        doPostAjaxSync('<%= Url.Generate<ApplicationController>(x => x.GetCampaignById(null)) %>',
            JSON.stringify({ campaignId: campaignId }),
        function (data) {
            if (data.isError) return;
            campaignTypeID = data.Data.CampaignTypeID;
        });
    }

    // прятать,показывать колонку с признаком
    function toggleColumnCampaignTypeId() {
        if (campaignTypeID == <%=GVUZ.DAL.Dapper.ViewModel.Dictionary.CampaignTypesView.BachelorAndSpeciality %>)
            $(".cl1").show();
        else
            $(".cl1").hide();
    }

    // изменение в фильтре Уровень образования
    function selectAdmissionItemType() {
        var campaignId = $('#CampaignID').val();
        var educationLevelId = $('#ItemTypeID').val();
        getGroupByCampaign(campaignId, educationLevelId);
        //getCompetitiveGroupItems();
    }

    // заполнение фильтра Уровень образования
    function getAdmissionItemTypeByCampaign(campaignId) {
        doPostAjaxSync('<%= Url.Generate<ApplicationController>(x => x.GetAdmissionItemTypeByCampaign(null)) %>',
            JSON.stringify({ campaignId: campaignId }),
        function (data) {
            if (data.isError) return;
            $('#ItemTypeID').empty();
            $.each(data.Data, function (i, obj) {
                $('#ItemTypeID').append($("<option/>").val(obj.ItemTypeID).text(obj.Name));
            });
        });
    }

    // получение списка конкурсов
    function getGroupByCampaign(CampaignID, EducationLevelID) {

        //if (groupByCampaign[CampaignID] != undefined) {
        //    return groupByCampaign[CampaignID];
        //}

        doPostAjaxSync('<%= Url.Generate<CompetitiveGroupController>(x => x.GetCompetitiveGroupsByCampaign(null, null, null)) %>',
            JSON.stringify({ InstitutionID: InstitutionID, CampaignID: CampaignID, EducationLevelID: EducationLevelID }),
            function (data) {
                if (data.isError)
                    return;

                groupByCampaign[CampaignID] = data.Data;

                var options = $("#SelectedCompetitions");

                options.empty(); // очистка конкурсов

                // заполнение Конкурсов
                $.each(data.Data, function (i, obj) {
                    options.append($("<option />").val(obj.ID).text(obj.Name));
                });
                options.trigger("chosen:updated");
            });

        return groupByCampaign[CampaignID];
    }

    // добавление условия приема
    function addCondition(id) {

        if($('#crow' + id).length > 0 )
            return;

        var model = { competitiveGroupId: id };

        function afterLoad(data) {
            if (data.Data == null)
                return;
             
            var programs = data.Data.CompetitiveGroupProgramRow == null ? " " : data.Data.CompetitiveGroupProgramRow;
            //$.each(data.Data.CompetitiveGroupProgram, function (i, obj) {
            //    programs = programs + (obj.Code ? obj.Code : '') + ' ' + obj.Name + '<br/>';
            //});

            var levelBudget = data.Data.LevelBudgetRow == null ? " " : data.Data.LevelBudgetRow;
                //data.Data.Source.Name          
            //$.each(data.Data.LevelBudget, function (i, obj) {
            //    levelBudget = levelBudget + (obj.BudgetName ? obj.BudgetName : '');
            //});

            var s = "";

            if (data.Data.EducationSourceId == 16) {
                function afterLoad1(info) {
                    //console.log(info)
                    //s = "<select class='idtargetcell' id='targetInput' disabled='disabled'>";
                    s = "<select class='idtargetcell'>";
                    if (info.Data.ApplicationPriorities.length > 0) {
                        for (var j = 0; j < info.Data.ApplicationPriorities[0].TargetOrganizations.length; j++) {
                            s += '<option value="' + info.Data.ApplicationPriorities[0].TargetOrganizations[j].ID + '">' + info.Data.ApplicationPriorities[0].TargetOrganizations[j].Name + '</option>';
                        }
                    }
                    s += '</select>';
                }
                doPostAjaxSync('<%= Url.Generate<InstitutionApplicationDraftController>(x => x.GetPrioritiesData(null)) %>',
                JSON.stringify(model), afterLoad1);
            }

            var c = "";
            if (campaignTypeID == <%=GVUZ.DAL.Dapper.ViewModel.Dictionary.CampaignTypesView.BachelorAndSpeciality %>) {
                function afterLoad2(info) {
                    if (!info.Data)
                        c = "disabled='disabled'";
                }
                doPostAjaxSync('<%= Url.Generate<ApplicationController>(x => x.CheckEntranceTestItemC(null)) %>',
                JSON.stringify(model), afterLoad2);
            }
            

        var directionCode = data.Data.DirectionID == null ? data.Data.ParentDirection.Code : data.Data.Direction.NewCode;
        var directionName = data.Data.DirectionID == null ? data.Data.ParentDirection.Name : data.Data.Direction.Name;
        var isUGS = data.Data.DirectionID == null ? true : false;


            $('#conditionsTable > tbody:last-child').append(
                "<tr id='crow" + id + "'>" +
                    "<td hidden='hidden' class='idcell'>" + id + "</td>" +
                    "<td hidden='hidden' class='idformcell'>" + data.Data.EducationFormId + "</td>" +
                    "<td hidden='hidden' class='idsourcecell'>" + data.Data.EducationSourceId + "</td>" +
                    "<td hidden='hidden' class='IsUGS'>" + isUGS + "</td>" +
                    "<td>" + data.Data.Level.Name + "</td>" +
                    "<td>" + data.Data.Name + "</td>" +
                    "<td><p>" + directionCode + " " + directionName +
                    "</p><p>" + data.Data.Form.Name +
                    "</p><p>" + data.Data.Source.Name + s +
                    "</p></td>" +
                    "<td>" + programs + "</td>" +
                    //"<td>" + data.Data.Form.Name + "</td>" +
                    //"<td>" + data.Data.Source.Name + s + "</td>" +
                    "<td>" + levelBudget + "</td>" +
                    "<td class='cl1'>" + "<input type='checkbox' class='checkcell' " +  c + "/>" + "</td>" +
                    "<td>" + "<a class='delete' href='#' onclick='delRow(this)'></a>" + "</td>" +
                "</tr>");
            
            toggleColumnCampaignTypeId();
        }

        doPostAjax('<%= Url.Generate<InstitutionApplicationDraftController>(x => x.GetCompetitiveGroupInfo(null)) %>',
            JSON.stringify(model), afterLoad);
    }

    // удаление условия приема
    function delCondition(id) {
        $('#crow' + id).remove();
    }

    // удаление в таблице условий приема
    function delRow(a) {
        var row = $(a).closest('tr');
        var id = $(row).find('td:first').html();
        $(row).remove();

        $("#SelectedCompetitions option[value=" + id + "]").removeAttr("selected");
        $("#SelectedCompetitions").trigger("chosen:updated");
    }

    // инициализация шага визарда
    function Wz0Init() {

        selectCampaign();

        jQuery("#RegistrationDate").datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-10:+0', maxDate: new Date()});
        //autocompleteDropdown(jQuery('#compGroupComplete'), {
        //    minLength: 1, source: competitiveGroupComplete,
        //    select: function () { setTimeout(competitiveGroupCompleteSelected, 0) }
        //});

        //autocompleteDropdown(jQuery('#dirComplete'), {
        //    minLength: 1, source: directionComplete,
        //    select: function () { setTimeout(directionCompleteSelected, 0) }
        //});

        /*jQuery("input#SNILS").mask("999-999-999 99");*/
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

        // инициализация chosen
        $(".chosen").chosen({ max_selected_options: 10, width: '340px', placeholder_text: "Выберите один или несколько конкурсов" });
        $(".chosen-deselect").chosen({ allow_single_deselect: true });
        $(".default").css("width", "300px");

        // обработка при изменениях в chosen Конкурсы
        $("#SelectedCompetitions").chosen().change(function (e, param) {
            if (param.selected != undefined)
                addCondition(param.selected);
            if (param.deselected != undefined)
                delCondition(param.deselected);
        });

        $(".chosen").trigger("chosen:updated");


        //var StatusId=<%=Model.StatusID %>;
        //if (StatusId == 4 || StatusId == 6) {
        //}

    }

    // сборка модели перед сохранением
    function Wz0GetModel() {

        var Priorities = { ApplicationId: -1, ApplicationPriorities: [] };

        //var priorityRows = $("#conditionsTable tr");
        //for (var i = 1; i < priorityRows.length; i++) {
        //    var row = priorityRows[i];
        //    //var Priority = {
        //    //    CompetitiveGroupId: $($(priorityRow).children()[0]).attr('dataVal'),
        //    //    CompetitiveGroupItemId: $($(priorityRow).children()[1]).attr('dataVal'),
        //    //    EducationFormId: $($(priorityRow).children()[2]).attr('dataVal'),
        //    //    EducationSourceId: $($(priorityRow).children()[3]).attr('dataVal'),
        //    //    Priority: $($(priorityRow).children()[4]).children('.priorityInput').val(),
        //    //    CompetitiveGroupTargetId: $($(priorityRow).children()[4]).children('#targetInput').val()
        //    //}
        //    Priorities.ApplicationPriorities.push(Priority);
        //}

        $('#conditionsTable tr').each(function (i) {
            if (i > 0) {
                var row = {
                    CompetitiveGroupId: $(this).find(".idcell").html(),
                    EducationFormId: $(this).find(".idformcell").html(),
                    EducationSourceId: $(this).find(".idsourcecell").html(),
                    IsForSPOandVO: $(this).find(".checkcell").is(":checked"),
                    CompetitiveGroupTargetId: $(this).find(".idtargetcell").val(),
                    
                }
                Priorities.ApplicationPriorities.push(row);
                //console.log(Priorities.ApplicationPriorities)
            }
        });

        var model = {
            ApplicationId: ApplicationId,
            InstitutionID: InstitutionID,

            CampaignID: $('#CampaignID').val(),
            DocumentSeries: $('#DocumentSeries').val(),
            DocumentNumber: $('#DocumentNumber').val(),
            IdentityDocumentTypeID: $('#IdentityDocumentTypeID').val(),
            RegistrationDate: $('#RegistrationDate').val(),
            ApplicationNumber: $('#ApplicationNumber').val(),
            Priorities: Priorities,

            SelectedCompetitiveGroupIDs: $('#divCompGroups span[cgID]').attrToArr('cgID'),
            SelectedDirectionIDs: $("#conditionsTable").find(".IsUGS").val() ? null : $('#divDirections span[itemID]').attrToArr('itemID'),
            SelectedParentDirectionIDs: !$("#conditionsTable").find(".IsUGS").val() ? null : $('#divDirections span[itemID]').attrToArr('itemID'),
            Priority: $('#Priority').val(),
            SelectedTargetOrganizationIDO: $("#SelectedTargetOrganizationIDO").val(),
            SelectedTargetOrganizationIDOZ: $("#SelectedTargetOrganizationIDOZ").val(),
            SelectedTargetOrganizationIDZ: $("#SelectedTargetOrganizationIDZ").val(),
            CheckForExistingBeforeCreate: true,
            CheckUniqueBeforeCreate: true,
            CheckZerozBeforeCreate: true,
        }

        if (!model.SelectedTargetOrganizationIDO) model.SelectedTargetOrganizationIDO = 0;
        if (!model.SelectedTargetOrganizationIDOZ) model.SelectedTargetOrganizationIDOZ = 0;
        if (!model.SelectedTargetOrganizationIDZ) model.SelectedTargetOrganizationIDZ = 0;

        return model;
    }

    // подготовка перед сохранением
    function Wz0PrepareModel() {
        var model = Wz0GetModel();
        if (Wz0CheckModel(model)) {
            return model;
        }
        return null;
    }

    // верификация модели перед сохранением
    function Wz0CheckModel(model) {
        var isError = false;
        var listIdentityDocumentType = JSON.parse('<%= Html.Serialize(Model.ListIdentityDocumentType) %>');

        //if ($(".priorityInput").length == 0) {
        //    infoDialog("Отсутствуют Условия приема для выбранных Направлений подготовки!");
        //    return false;
        //}

        if (model.Priorities.ApplicationPriorities.length == 0) {
                infoDialog("Нужно выбрать хотя бы один Конкурс!");
                return false;
        }

        var resul = $.grep(listIdentityDocumentType, function (v) {
            return v.IdentityDocumentTypeId === parseInt($('#IdentityDocumentTypeID').val());
        });

        if (resul[0].IsRussianNationality) {
            if ((resul[0].IdentityDocumentTypeId != 10) || (resul[0].IdentityDocumentTypeId != 9)) {
                $('#DocumentSeries').attr('val-required', 'Серия документа обязательна!');
            }
        } else {
            $('#DocumentSeries').attr('val-required', '');
        }

        $('#EducationForms.input-validation-error').addClass('input-validation-error-fixed').removeClass('input-validation-error');

        if (revalidatePage($('#Wz0Form'))) {
            isError = true;
        }

        if ($('#DocumentNumber').hasClass('input-validation-error')) {
            if (!$('#DocumentNumber').hasClass('input-validation-error')) {
                $('#DocumentNumber').addClass('input-validation-error');
            }
            $('#DocumentNumber').removeClass('input-validation-error-fixed');
        }
        if (isError) { return null; }

        if (model == null) { return false; }

        var res = true;

        //var dubli = false;
        //try {
        //    var priorities = model.Priorities.ApplicationPriorities;
        //    var counts = [];


        //    if (priorities.length == 0) {
        //        $(".priorityInput").each(function () {
        //            $(this).removeClass('input-validation-error-fixed').addClass('input-validation-error');
        //        });
        //        infoDialog("Проставьте приоритеты!");
        //        return false;
        //    }
        //    for (var i = 0; i < priorities.length; i++) {
        //        var p = priorities[i];
        //        if (p.Priority == '') { continue; }
        //        if (isNaN(parseInt(p.Priority))) {
        //            alert("Неверно заданы приоритеты!");
        //            res = false;
        //            break;
        //        }
        //        if (counts[p.Priority] == undefined) {
        //            counts[p.Priority] = 1;
        //        } else {
        //            counts[p.Priority] = counts[p.Priority] + 1;
        //            dubli = true;
        //        }
        //    }
        //    if (counts.length == 0) {
        //        $(".priorityInput").each(function () {
        //            $(this).removeClass('input-validation-error-fixed').addClass('input-validation-error');
        //        });
        //        //alert("Проставьте приоритеты!");
        //        return false;
        //    }

        //    if (dubli) {
        //        if (!confirm("Для нескольких условий приема указаны одинаковые приоритеты. Вы уверены, что хотите продолжить?")) { res = false; }
        //    }
        //    // Проверка на то, что все приоритеты - нули
        //    // Проверка на наличие групп и не нулевые группы
        //} catch (e) {
        //    res = false;
        //}

        return res;
    }

    function proverka(input) {
        var n = parseInt($(input).val().replace(/\D/g, ''), 10);
        $(input).val(n.toLocaleString());
        if (n >= 0) {
        } else {
            $(input).val("");
        }
    }

    function Wz0Save(success, error, step) {
        var model = Wz0PrepareModel();

        if (model == null) {
            return false;
        }
        if (model.ApplicationId < 1) {
            Wz0New(model, success, error);
        }
    }

    function Wz0New(model, success, error) {
        var res = true;
        var changed = false;
        // Проверка на изменение
        if (WzObjs[0] == undefined) {
            WzObjs[0] = model;
        }
        // Проверка на изменение данных.
        doPostAjax('<%= Url.Generate<ApplicationController>(x => x.NewWz0(null)) %>', JSON.stringify(model),
            function (data) {
                $('#txtDateError span').text("");
                if (!data.IsError) {

                    $('#RegistrationDate').removeClass('input-validation-error-fixed');
                    if (data.Extra == "DateError") {
                        $('#RegistrationDate').addClass('input-validation-error');
                        $('#txtDateError').html('<span class="field-validation-error">Введено некорректное значение</span>');
                        return;
                    }
                    if (data.Data.id != undefined) {
                        ApplicationId = data.Data.id;
                        ApplicationID = data.Data.id;
                        model.ApplicationId = ApplicationId;
                        if (data.Data.EntrantIsNew == false) {
                            infoDialog('В системе найден абитуриент, соответствующий введенным параметрам', function () {
                                if (success) {
                                    success(model);
                                }
                            });
                        } else {
                            if (success) {
                                success(model);
                            }
                        }
                    }
                } else { // Обработка ошибки.
                    res = false;
                    //var msg = "Ошибка при создании заявления.<br/>" + data.Message;
                    if (error) {
                        if (data.Message != null) {
                            error(data.Message);
                        }
                    }
                }
            });
        return res;
    }

    function Wz0Cancel() {
        //selectCampaign();
    }

    //function competitiveGroupComplete(ui, response) {
    //    if (!$('#CampaignID').val()) return;
    //    var res = [];
    //    var x = ui.term.toUpperCase();
    //    var selectedGroups = $('#divCompGroups span[cgID]').attrToArr('cgID');
    //    var allowedCourse = '';
    //    if (selectedGroups.length > 0) {
    //        allowedCourse = jQuery('#divCompGroups span[cgID]').attr('course');
    //    }
    //    jQuery.each(groupByCampaign[jQuery('#CampaignID').val()], function (i, e) {
    //        var name = e.Name + ', ' + e.Course + ' курс';
    //        if (name.toUpperCase().indexOf(x) >= 0 && (allowedCourse == e.Course || allowedCourse == '') && selectedGroups.indexOf(e.ID) < 0)
    //            res.push(escapeHtml(e.Name) + ', ' + e.Course + ' курс');
    //    });
    //    response(res);
    //}

    //function competitiveGroupCompleteSelected() {
    //    var val = $("#compGroupComplete").val().toUpperCase();
    //    $.each(groupByCampaign[jQuery('#CampaignID').val()], function (i, e) {
    //        var curID = tmpID++;
    //        var name = e.Name + ', ' + e.Course + ' курс';
    //        if (name.toUpperCase() == val) {
    //            $("#divCompGroups").append('<span id="cbtmpid' + curID + '" cgID="' + e.ID + '" course="' + e.Course + '">' + escapeHtml(e.Name) + ', ' + e.Course + ' курс'
    //                + ' <a class="btnDelete" href="#" onclick="competitiveGroupCompleteRemove(' + e.ID + ');return false;">&nbsp;</a><br/></span>');
    //        }
    //    });
    //    jQuery("#compGroupComplete").val('');
    //    getCompetitiveGroupItems();
    //}

<%--    function getCompetitiveGroupItems() {
        var selectedGroups = jQuery('#divCompGroups span[cgID]').attrToArr('cgID');
        var selectedDirs = jQuery('#divDirections span[itemID]').attrToArr('itemID');
        jQuery('#dirComplete').val('');

        doPostAjax('<%= Url.Generate<InstitutionApplicationDraftController>(x => x.GetDirectionsForCompetitiveGroups(null)) %>',
            JSON.stringify(selectedGroups),
			function (data) {
			    if (data.IsError) {
			        infoDialog(data.Message);
			        return;
			    }
			    availableDirections = data.Data;
			    var newDirs = [];
			    jQuery.each(data.Data, function (i, e) { newDirs.push(e.ID); });

			    for (var i = 0; i < selectedDirs.length; i++) {
			        if (newDirs.indexOf(selectedDirs[i]) < 0) {
			            $('#divDirections span[itemID="' + selectedDirs[i] + '"]').remove().detach();
			        }
			    }

			    if (data.Data.length == 0) {
			        jQuery('#divDirections').html('<span class="emptyRow">Отсутствуют доступные направления подготовки</span>')
			        jQuery('#dirComplete').parent().hide();
			    } else {
			        jQuery('#dirComplete').parent().show();
			        jQuery('#divDirections span.emptyRow').remove().detach();
			    }

			    getEducationForms();
			});
    }--%>

    //function directionComplete(ui, response) {
    //    var res = [];
    //    var x = ui.term.toUpperCase();
    //    var selectedDirs = jQuery('#divDirections span[itemID]').attrToArr('itemID');

    //    jQuery.each(availableDirections, function (i, e) {
    //        var name = e.Code + ' - ' + e.Name;
    //        if (name.toUpperCase().indexOf(x) >= 0 && selectedDirs.indexOf(e.ID) < 0)
    //            res.push({ label: escapeHtml(e.Code) + ' - ' + escapeHtml(e.Name), value: e.ID });
    //    });
    //    response(res);
    //}

    //function directionCompleteGetRow(item) {
    //    var curID = tmpID++;
    //    return '<span id="cbtmpid' + curID + '" itemID="' + item.ID + '">' + escapeHtml(item.Code) + ' - ' + escapeHtml(item.Name)
    //        + ' <a class="btnDelete" href="#" onclick="directionCompleteRemove(\'' + item.ID + '\');return false;">&nbsp;</a><br/></span>';
    //}

    //function directionCompleteSelected() {
    //    var val = jQuery("#dirComplete").val().toUpperCase();
    //    jQuery.each(availableDirections, function (i, e) {
    //        var name = e.Code + ' - ' + e.Name;
    //        if (e.ID == val) {
    //            jQuery("#divDirections").append(directionCompleteGetRow(e));
    //        }

    //        //            if (name.toUpperCase() == val) {
    //        //                jQuery("#divDirections").append(directionCompleteGetRow(e));
    //        //            }
    //    });
    //    jQuery("#dirComplete").val('');
    //    getEducationForms();
    //}

    //function directionCompleteRemove(itemID) {
    //    jQuery('#divDirections span[itemID="' + itemID + '"]').remove().detach();
    //    getEducationForms();
    //}

    //function competitiveGroupCompleteRemove(groupID) {
    //    var $el = jQuery('#divCompGroups span[cgID="' + groupID + '"]');
    //    $el.remove().detach();
    //    getCompetitiveGroupItems();
    //}

<%--    function getEducationForms() {
        var selectedGroups = jQuery('#divCompGroups span[cgID]').attrToArr('cgID');
        var selectedDirs = jQuery('#divDirections span[itemID]').attrToArr('itemID');

        var model = { competitiveGroupIDs: selectedGroups, directionKeys: selectedDirs };

        function AfterLoad(data) {
            var tab = jQuery("#prioritiesTable");
            if (tab == null) return;

            tab.empty();
            if (data.Data == null) return;

            tab.append(jQuery('<thead>').html('<tr><th>Конкурс</th><th>Направление подготовки</th><th>Форма обучения</th><th>Источник финансирования</th><th>Приоритет</th></tr>'));


            for (var i = 0; i < data.Data.ApplicationPriorities.length; i++) {
                var row = jQuery("<tr>");
                var html = '<td dataVal="' + data.Data.ApplicationPriorities[i].CompetitiveGroupId + '">' + data.Data.ApplicationPriorities[i].CompetitiveGroupName + '</td>' +
                              '<td dataVal="' + data.Data.ApplicationPriorities[i].CompetitiveGroupItemId + '">' + data.Data.ApplicationPriorities[i].CompetitiveGroupItemName + '</td>' +
                              '<td dataVal="' + data.Data.ApplicationPriorities[i].EducationFormId + '">' + data.Data.ApplicationPriorities[i].EducationFormName + '</td>' +
                              '<td dataVal="' + data.Data.ApplicationPriorities[i].EducationSourceId + '">' + data.Data.ApplicationPriorities[i].EducationSourceName + '</td>' +
                              '<td><input type="text" class="priorityInput" onchange=priorityChanged(this,' + (data.Data.ApplicationPriorities[i].TargetOrganizations != null ? '1' : '0') + ')>';

                if (data.Data.ApplicationPriorities[i].TargetOrganizations != null) {
                    html = html + '<br/><select id="targetInput" disabled="disabled">';
                    for (var j = 0; j < data.Data.ApplicationPriorities[i].TargetOrganizations.length; j++) {
                        html += '<option value="' + data.Data.ApplicationPriorities[i].TargetOrganizations[j].ID + '">' + data.Data.ApplicationPriorities[i].TargetOrganizations[j].Name + '</option>';
                    }
                    html += '</select>';
                }
                html += '</td>';
                row.html(html);
                tab.append(row);
            }
        }
        if (model.competitiveGroupIDs.length == 0 || model.directionKeys.length == 0) {
            AfterLoad({ Data: null }); return;
        }

        doPostAjax('<%= Url.Generate<InstitutionApplicationDraftController>(x => x.CreatePrioritiesData(null, null)) %>',
            JSON.stringify(model), AfterLoad);
    }--%>

    //function priorityChanged(el, flag) {
    //    if (flag == '0') return false;
    //    var selector = $(el).parent().children('select:first');
    //    if ($(el).val() == '') { selector.setDisabled(); } else { selector.setEnabled(); }
    //}

</script>

<table id="Wz0Form" class="data tableAdmin" cellpadding="3">
    <tbody>
        <tr class="separat">
            <td class="caption big"><%= Html.TableLabelFor(x => x.CampaignID, required:true) %></td>
            <td><%= Html.DropDownListExFor(m => m.CampaignID, Model.Campaigns, new {onchange = "selectCampaign()"})%></td>
        </tr>

        <%-- фильтр Уровень образования --%>
        <tr class="separat">
            <td class="caption big"><%= Html.TableLabelFor(x => x.ItemTypeID, required:true) %></td>
            <td><%= Html.DropDownListExFor(m => m.ItemTypeID, Model.AdmissionItemTypes, new {onchange = "selectAdmissionItemType()"})%></td>
        </tr>

        <tr class="separat">
            <td class="caption big"><%= Html.TableLabelFor(x => x.DisplayCompetitiveGroupID, required: true)%></td>
            <td>
                <%--                <input type="text" id="compGroupComplete" style="width: 600px" /><br />
                <div id="divCompGroups" class="tableStatement2"></div>--%>

                <%= Html.ListBoxFor(m => m.SelectedCompetitions, Model.Competitions, new { @class = "chosen", multiple = "multiple" })%>
            </td>
        </tr>

        <%--<tr class="separat">
            <td class="caption big"><%= Html.TableLabelFor(x => x.DisplayDirectionID, required: true)%></td>
            <td>
                <span>
                    <input type="text" id="dirComplete" style="width: 600px" /><br />
                </span>
                <div id="divDirections" class="tableStatement2"></div>
            </td>
        </tr>--%>

        <tr class="separat">
            <td class="caption big" rowspan="2">Условия приёма: </td>
            <%--<td>Для выбора условия приема проставьте его приоритет в заявлении абитуриента в поле
            "Приоритет". Приоритет может быть целым положительным числом, 1 - максимальный приоритет.
            Если прием проводится без приоритетов, то для выбора условий приема необходимо поставить 0
            </td>--%>
            <td>
                <table id="conditionsTable" class="gvuzDataGrid">
                    <thead>
                        <tr>
                            <th>Уровень</th>
                            <th>Конкурс</th>
                            <th>Атрибуты</th>
                            <%--<th>Направление подготовки</th>--%>
                            <th>ОП</th>
                            <%--<th>Форма обучения</th>--%>
                            <%--<th>Источник финансирования</th>--%>
                            <th>Уровень бюджета</th>
                            <th class="cl1">Профильное СПО/ВО</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody id="conditions" />
                </table>
            </td>
        </tr>

        <tr class="separat">
            <td>
                <table id="prioritiesTable" class="gvuzDataGrid"></table>
            </td>
        </tr>

        <tr>
            <td class="caption"><%= Html.TableLabelFor(m => m.ApplicationNumber, required: true)%></td>
            <td><%= Html.TextBoxExFor(m => m.ApplicationNumber)%></td>
            <%--<td class="caption"><%= Html.TableLabelFor(m => m.Priority) %></td>
            <td><%= Html.TextBoxExFor(m => m.Priority, new { onkeyup="return proverka(this);" }) %></td>--%>
        </tr>
        <tr>
            <td class="caption"><%= Html.TableLabelFor(m => m.RegistrationDate, required: true)%></td>
            <td><%= Html.DatePickerFor(m => m.RegistrationDate)%><br />
                <div id="txtDateError"></div>
            </td>
        </tr>
        <tr>
            <td class="caption"><%= Html.TableLabelFor(m => m.IdentityDocumentTypeID, new {style="padding-top:0px;padding-bottom:10px;"},required: true)%></td>
            <td><%= Html.DropDownListExFor(m => m.IdentityDocumentTypeID, Model.IdentityDocumentList, new {})%></td>
        </tr>
        <tr>
            <td class="caption"><%= Html.TableLabelFor(m => m.DocumentNumber, required: true)%></td>
            <td colspan="3"><%= Html.TextBoxExFor(m => m.DocumentSeries, new { @class="passSeries" })%><%= Html.TextBoxExFor(m => m.DocumentNumber, new { @class = "passNumber" })%></td>
        </tr>
    </tbody>
</table>

