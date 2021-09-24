<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.ApplicationWz5ViewModel.Wz5SendingViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

<script type="text/javascript">

    function ConditionsInit() {
        // инициализация chosen, в нем выбираются конкурсы  
        $(".chosen").chosen({ max_selected_options: 10, width: '340px', placeholder_text: "Выберите один или несколько конкурсов" });
        $(".chosen-deselect").chosen({ allow_single_deselect: true });
        $(".default").css("width", "300px");
        $(".chosen").change(function (e, param) {
            ChangeCompetition(param);
        });

        // нужно заполнить все комбо для редактирования условий приема, 
        // но без очистки сетки с условиями, т.к. она уже заполнена в модели
        ChangeCampaign(false);

        // выключаем режим редактирования при старте
        ToggleEditor(false);
    }

    // Переключение режима редактирования
    function ToggleEditor(edit) {
        if (edit) {
            $(".editor_conditions").show();
        }
        else {
            $(".editor_conditions").hide();
        }
        editMode = edit;

        ModifyControls();
    }

    // нажатие кнопки Изменить
    function EditConditions() {
        htmlCopy = $("#conditionsTable").html();
        ToggleEditor(true);
    }

    // нажатие кнопки Сохранить
    function SaveConditions() {
        // собираем модель с условиями приема
        var conditionsModel = GetConditions();
        if (conditionsModel == null)
            return false;

        var model = {
            ApplicationId: ApplicationId,
            ApplicationPriorities: conditionsModel
        }

        doPostAjax('<%= Url.Generate<ApplicationController>(x=>x.UpdateWz5(null)) %>', JSON.stringify(model),
            function (data) {
                if (data.IsError) {
                    infoDialog(data.Data);
                    return false;
                }
                ToggleEditor(false);
            });
    }

    // нажатие кнопки Отменить
    function CancelConditions() {
        RefreshConditions();
        ToggleEditor(false);
    }

    // изменение в комбо Приемная кампания
    function ChangeCampaign(erase) {
        // очистка всей таблицы условий
        if (erase) {
            $("#conditions").empty();
            changed = true;
        }

        campaignId = $('#CampaignID').val();

        PopulateLevels(campaignId);
        PopulateCompetitions(campaignId, $('#LevelId').val());

        // обработка типа Приемной кампании, от него зависит возможность редактирования колонки СПО/ВО
        // при изменениях в комбо обрабатываем тип выбранной кампании
        PrepareCampaignType(campaignId);
    }

    // изменение в комбо Уровень образования
    function ChangeLevel() {
        PopulateCompetitions($('#CampaignID').val(), $('#LevelId').val());
    }

    // изменение в комбо Конкурс
    function ChangeCompetition(param) {
        if (param.selected != undefined)
            AddCondition(param.selected);
        if (param.deselected != undefined)
            DeleteCondition(param.deselected);
    }

    // удаление строки таблицы условий приема из chosen
    function DeleteCondition(id) {
        $('#conditions tr[data-value=' + id + ']').remove();

        ModifyControls();
    }

    // добавление строки в таблицы условий приема из chosen
    function AddCondition(id) {
        // уже есть запись с таким id
        if ($('#conditions tr[data-value=' + id + ']').length > 0)
            return;

        doPostAjax('<%= Url.Generate<InstitutionApplicationDraftController>(x => x.GetConditionRow(null)) %>?id=' + id, {},
            function (data) {
                //console.log(data);
                $('#conditionsTable > tbody:last-child').append(data);

                // инициализация календарей
                $(".datePicker").datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-10:+0', maxDate: new Date() });

                // нужно спрятать или показать колонку spo_vo
                CheckVisibleSpoVo();

                ModifyControls();

            }, "application/x-www-form-urlencoded", "html");
        }

        // удаление строки таблицы условий приема крестиком
        function DeleteConditionRow(a) {
            var row = $(a).closest('tr');
            var id = $(row).attr("data-value");
            $(row).remove();

            // при удалении строки нужно "освободить" этот конкурс для выбора в choisen с Конкурсами
            $("#SelectedCompetitions option[value=" + id + "]").removeAttr("selected");
            $("#SelectedCompetitions").trigger("chosen:updated");

            ModifyControls();
        }

        // заполнение комбо Уровень образования
        function PopulateLevels(campaignId) {
            doPostAjaxSync('<%= Url.Generate<ApplicationController>(x => x.GetAdmissionItemTypeByCampaign(0)) %>', JSON.stringify({ campaignId: campaignId }),
            function (data) {
                if (data.isError)
                    return;
                $('#LevelId').empty();
                $.each(data.Data, function (i, obj) {
                    $('#LevelId').append($("<option/>").val(obj.ItemTypeID).text(obj.Name));
                });
            });
        }

        // заполнение комбо - choisen Конкурс
        function PopulateCompetitions(campaignId, levelId) {
            doPostAjaxSync('<%= Url.Generate<CompetitiveGroupController>(x => x.GetCompetitiveGroupsByCampaign(null, null, null)) %>',
            JSON.stringify({ InstitutionID: InstitutionID, CampaignID: campaignId, EducationLevelID: levelId }),
            function (data) {
                if (data.isError)
                    return;

                // очистка и заполнение комбо с Конкурсами
                var options = $("#SelectedCompetitions");
                options.empty();
                $.each(data.Data, function (i, obj) { options.append($("<option />").val(obj.ID).text(obj.Name)); });

                // необходимо выбрать в chosen все значения из таблицы, она может быть не пустая!
                var ar = [];
                $('#conditions tr').each(function () { ar.push($(this).attr("data-value")) });
                options.val(ar);

                options.trigger("chosen:updated");
            });
    }

    // получение информации о типе Приемной кампании, от него зависит возможность редактирования колонки СПО/ВО
    function PrepareCampaignType(campaignId) {
        doPostAjaxSync('<%= Url.Generate<ApplicationController>(x => x.GetCampaignById(0)) %>',
            JSON.stringify({ campaignId: campaignId }),
            function (data) {
                if (data.isError)
                    return;

                campaignTypeId = data.Data.CampaignTypeID;

                // нужно спрятать или показать колонку spo_vo
                CheckVisibleSpoVo();
            });
    }

    function CheckVisibleSpoVo() {
        if (campaignTypeId == '<%=GVUZ.DAL.Dapper.ViewModel.Dictionary.CampaignTypesView.BachelorAndSpeciality %>')
            $(".spo_vo").show();
        else
            $(".spo_vo").hide();
    }

    // обработчик изменений, если произошли именения с оригиналами документов
    function OnChangeOriginalsProvided() {
        originalsProvided = false;
        $('#docGrid1 tr').find('input[type=checkbox]').each(function (i) {
            if ($(this).is(":checked"))
                originalsProvided = true;
        });
        ModifyControls();
    }

    // проверки на несколько согласий и отказов
    function CheckManyAgreed($sender) { 
        if ($sender.closest("tr").attr("data-unlimitedAgreements") == 'True')
            return true;

        var limitIsAgreed = $("tr[data-unlimitedAgreements='False'] input[id='IsAgreed']:checked").length;
        var limitIsDisagreed = $("tr[data-unlimitedAgreements='False'] input[id='IsDisagreed']:checked").length;

        if (Math.abs(limitIsAgreed - limitIsDisagreed) > 1)
        {
            infoDialog("Нельзя выбрать согласие по данному конкурсу (требуется выбрать отказ в других конкурсах)");
            return false;
        }
        if (limitIsAgreed > 2)
        {
            infoDialog("Нельзя выбрать согласие по данному конкурсу (уже выбрано 2 согласия)"); 
            return false;
        }
        return true;
    }

    // этот метод дергается при любой модификации контролов и при старте
    function ModifyControls() {
        $('#conditions tr').each(function () { PrepareRow(this) });
    }

    // здесь логика работы с контролами строки таблицы условий
    function PrepareRow(row) { 
        var agreed = $(row).find('#IsAgreed');
        var disagreed = $(row).find('#IsDisagreed');
        var agreedDate = $(row).find('.IsAgreedDate');
        var disagreedDate = $(row).find('.IsDisagreedDate');
        var isForSPOandVO = $(row).find('#IsForSPOandVO');
        var competitiveGroupTargetId = $(row).find('#CompetitiveGroupTargetId');

        var isAgreed = $(agreed).is(":checked");
        var isDisagreed = $(disagreed).is(":checked");

        var allowEdit = $(row).find('#AllowEdit').val() === 'True'; 
        var limitIsAgreed = $("input[id='IsAgreed']:checked").length > 1;
        var limitIsDisagreed = $("input[id='IsDisagreed']:checked").length > 1;
    
        if (!allowEdit || !originalsProvided) {
            agreed.setDisabled();
            disagreed.setDisabled();
            agreedDate.setDisabled().datepicker('disable');
            disagreedDate.setDisabled().datepicker('disable');

            if (!allowEdit) {
                isForSPOandVO.setDisabled();
                competitiveGroupTargetId.setDisabled();
            }
            return;
        }

        if (!editMode) {
            competitiveGroupTargetId.setDisabled();
            isForSPOandVO.setDisabled();
        }
        else {
            competitiveGroupTargetId.setEnabled();
            isForSPOandVO.setEnabled();
        }

        if (isAgreed) {
            if (isDisagreed) {
                agreed.setDisabled();
                agreedDate.setDisabled().datepicker('disable');

                disagreed.setEnabled();
                disagreedDate.setEnabled().datepicker('enable');
            }
            else {
                disagreedDate.datepicker('setDate', null);
                disagreedDate.setDisabled().datepicker('disable');

                agreed.setEnabled();
                agreedDate.setEnabled().datepicker('enable');
                disagreed.setEnabled();
            }
        }
        else {
            agreedDate.datepicker('setDate', null);
            if (isDisagreed) {
                disagreed.setDisabled();
                disagreedDate.setDisabled().datepicker('disable');

                agreed.setEnabled();
                agreedDate.setEnabled().datepicker('enable');
            }
            else {
                agreedDate.setDisabled().datepicker('disable');
                disagreed.setDisabled();

                disagreedDate.datepicker('setDate', null);
                disagreedDate.setDisabled().datepicker('disable');

                agreed.setEnabled();
            }
        }
    }
</script>


<table class="institutionData tableAdmin" style="width: 90%">
    <tr>
        <td class="caption" style="width: 200px">
            <%= Html.TableLabelFor(m => m.Institution)%>
        </td>
        <td>
            <%= Html.CommonInputReadOnly(Model.Institution) %>
        </td>
    </tr>

    <tr class="separat editor">
        <td class="caption">Выбранные условия приёма</td>
        <td>
            <button id="changeButton" class="button primary" value="Изменить" onclick="EditConditions(); return false;">Изменить</button>
        </td>
    </tr>

    <tr class="separat editor_conditions" id="comboCampaign">
        <td class="caption"><%= Html.TableLabelFor(x => x.CampaignID, required:true) %></td>
        <td><%= Html.DropDownListExFor(m => m.CampaignID, Model.Campaigns, new {onchange = "ChangeCampaign(true);  return false;"})%></td>
    </tr>

    <tr class="separat editor_conditions" id="comboAdmission">
        <td class="caption"><%= Html.TableLabelFor(x => x.LevelId, required:true) %></td>
        <td><%= Html.DropDownListExFor(m => m.LevelId, Model.Levels, new {onchange = "ChangeLevel(); return false;"})%></td>
    </tr>

    <tr class="separat editor_conditions" id="comboCompetitive">
        <td class="caption"><%= Html.TableLabelFor(x => x.DisplayCompetitiveGroupID, required: true)%></td>
        <td><%= Html.ListBoxFor(m => m.SelectedCompetitions, Model.Competitions, new { @class = "chosen", multiple = "multiple" })%></td>
    </tr>

    <tr class="separat">
        <td class="caption">Условия приёма: </td>
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
                        <th class="spo_vo">Профильное СПО/ВО</th>
                        <th>Согласие на зачисление</th>
                        <th>Дата согласия</th>
                        <th>Отказ от согласия</th>
                        <th>Дата отказа</th>
                        <th class="editor_conditions"></th>
                    </tr>
                </thead>
                <tbody id="conditions">
                    <% foreach (var row in Model.Priorities.ApplicationPriorities)
                        { %>
                    <%= Html.Partial("Application/ApplicationWz5_Conditions_Row", row) %>
                    <% } %>
                </tbody>
            </table>
        </td>
    </tr>

    <tr class="separat editor_conditions" id="buttonsModify">
        <td class="caption"></td>
        <td>
            <button id="savePriorities" value="Сохранить" onclick="SaveConditions(); return false;">
                Сохранить</button>
            <button id="cancelPriorities" value="Отмена" onclick="CancelConditions();  return false;">
                Отмена</button>
        </td>
    </tr>

</table>


