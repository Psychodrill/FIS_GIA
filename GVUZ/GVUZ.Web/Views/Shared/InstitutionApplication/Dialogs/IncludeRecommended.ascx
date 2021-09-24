<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.ApplicationsList.IncludeRecommendedListViewModel>" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

<script type="text/javascript">

    function includeRecommendedDialog(recordId, callback, callbackScope) {
    
        function IncludeRecommendedViewModel(modelData) {
            ko.mapping.fromJS(modelData, this.readMapping, this);
            this.stages = [[], []]; // контейнер для хранения id отмеченных условий приема 
                                    // для разных этапов: stages[0] = 1 этап, stages[1] = 2 этап
            this.selectedRecords = ko.observableArray([]);
            this.selectedStageMonitor = this.SelectedStage.subscribe(this.setSelectionSource, this);
        }

        IncludeRecommendedViewModel.prototype = {
            setSelectionSource: function (stage) {
                if (/^1|2$/.test(stage)) {
                    this.selectedRecords(this.stages[stage == 1 ? 0 : 1]);
                }
                else {
                    this.selectedRecords([]);
                }
            },
            readMapping: {
                'copy': ['ApplicationRecords', 'Stages', 'ApplicationNumber', 'EntrantName', 'IdentityDocument', 'ApplicationId'],
                'ignore': ['RecordInfo']
            },
            getSubmitModel: function () {
                var stageSelection = [];
                for (var i = 0; i < this.stages.length; i++) {
                    for (var j = 0; j < this.stages[i].length; j++) {
                        stageSelection.push({ Id: this.stages[i][j], Stage: i + 1 });
                    }
                }

                return {
                    'ApplicationId': this.ApplicationId,
                    'SelectedItems': stageSelection
                };
            },
            accept: function (onSuccess) {
                var submitModel = this.getSubmitModel();
                if (submitModel.SelectedItems.length == 0) {
                    alert('Для включения в список рекомендованных должно быть отмечено хотя бы одно условие приема');
                    return;
                }
                var me = this;
                doPostAjax('<%= Url.Action("IncludeRecommendedList", "InstitutionApplication") %>', ko.toJSON(submitModel), function (result) {
                    if (result.success) {
                        me.dispose();
                        if (onSuccess && typeof onSuccess === "function") {
                            onSuccess.call(this);
                        }
                    }
                    else if (result.message) {
                        alert(result.message);
                    }
                }, null, null, true);
            },
            dispose: function () {
                if (this.selectedStageMonitor) {
                    this.selectedStageMonitor.dispose();
                    this.selectedStageMonitor = null;
                }
            }
        };
        
        var $container = $('#includeRecommendedDialog');
        var existingContext = ko.contextFor($container[0]);
        
        function dialogSubmitted() {
            $container.dialog('close');
            if (callback != null && typeof callback === "function") {
                callback.call(callbackScope || window);
            }
        }

        function initDialog() {
            $container.dialog({
                modal: true,
                title: 'Включение в список рекомендованных',
                width: 800,
                height: 500,
                buttons: [
                    {
                        text: 'Принять',
                        click: function () {
                            var context = ko.contextFor($container[0]);
                            IncludeRecommendedViewModel.prototype.accept.call(context.$rawData(), dialogSubmitted);
                        }
                    },
                    {
                        text: 'Отмена',
                        click: function () {
                            var context = ko.contextFor($container[0]);
                            IncludeRecommendedViewModel.prototype.dispose.call(context.$rawData());
                            $(this).dialog('close');
                        }
                    }
                ]
            });
        }

        var model = { applicationId: Number(recordId) };
        
        doPostAjax('<%= Url.Action("GetIncludeRecommendedList", "InstitutionApplication") %>', ko.toJSON(model), function (data) {

            var viewModel = new IncludeRecommendedViewModel(data);
            
            if (existingContext && ko.isObservable(existingContext.$rawData)) {
                existingContext.$rawData(viewModel);
            } else {
                initDialog();
                ko.applyBindings(ko.observable(viewModel), $container[0]);
            }

            $container.dialog('open');

        }, null, null, true);
    }    

</script>

<div id="includeRecommendedDialog" style="display: none;overflow: auto">
  <table style="width: 100%;border-collapse: collapse;border: 0">
      <colgroup>
          <col style="width: 15%"/>
          <col style="width: 55%"/>
          <col style="width: 10%;vertical-align: middle"/>
          <col style="width: 20%;vertical-align: middle"/>
      </colgroup>
      <tbody>
          <tr>
              <td><%= Html.DisplayNameFor(x => x.ApplicationNumber) %>:</td>
              <td><span style="font-weight: bold" data-bind="text: ApplicationNumber"></span></td>
              <td rowspan="3"><%= Html.DisplayNameFor(x => x.SelectedStage) %>:</td>
              <td rowspan="3"><%= Html.BoundedSelectFor(x => x.SelectedStage, x => x.Stages, new { style="width: 150px;"}) %></td>
          </tr>
          <tr>
              <td><%= Html.DisplayNameFor(x => x.EntrantName) %>:</td>
              <td><span style="font-weight: bold" data-bind="text: EntrantName"></span></td>
          </tr>
          <tr>
              <td><%= Html.DisplayNameFor(x => x.IdentityDocument) %>:</td>
              <td><span style="font-weight: bold" data-bind="text: IdentityDocument"></span></td>
          </tr>
      </tbody>
  </table>
  
  <table class="gvuzDataGrid tableStatement2" style="width: 100%">
      <colgroup>
          <col style="width: 38%"/>
          <col style="width: 30%"/>
          <col style="width: 10%"/>
          <col style="width: 10%"/>
          <col style="width: 10%"/>
          <col style="width: 1%"/>
          <col style="width: 1%"/>
      </colgroup>
      <thead>
          <tr>
              <th><%= Html.DisplayNameFor(x => x.RecordInfo.CompetitiveGroupName) %></th>
              <th><%= Html.DisplayNameFor(x => x.RecordInfo.DirectionName) %></th>
              <th><%= Html.DisplayNameFor(x => x.RecordInfo.EducationLevelName) %></th>
              <th><%= Html.DisplayNameFor(x => x.RecordInfo.EducationFormName) %></th>
              <th><%= Html.DisplayNameFor(x => x.RecordInfo.EducationSourceName) %></th>
              <th><%= Html.DisplayNameFor(x => x.RecordInfo.Priority) %></th>
              <th>Рекомендован</th>
          </tr>
      </thead>
      <tbody data-bind="foreach: ApplicationRecords">
          <tr data-bind="css: {trline1: $index()%2 == 0, trline2: $index()%2 != 0}">
              <td><span data-bind="text: CompetitiveGroupName" /></td>
              <td><span data-bind="text: DirectionName" /></td>
              <td><span data-bind="text: EducationLevelName" /></td>
              <td><span data-bind="text: EducationFormName" /></td>
              <td><span data-bind="text: EducationSourceName" /></td>
              <td><span data-bind="text: Priority" /></td>
              <td><input type="checkbox" data-bind="checkedValue: ApplicationCompetitiveGroupItemId, checked: $parent.selectedRecords, enable: $parent.SelectedStage() > 0"/></td>
          </tr>
      </tbody>
  </table>
    
</div>
