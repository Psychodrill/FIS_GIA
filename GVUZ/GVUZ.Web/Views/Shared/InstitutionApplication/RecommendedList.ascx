<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.ApplicationsList.RecommendedApplicationsListViewModel>" %>
<%@ Import Namespace="System.Web.Helpers" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.ViewModels.ApplicationsList" %>

<script type="text/javascript">
    function recommendedListLoaded() {

        function RecommendedApplicationsFilterViewModel(data, defaults) {
            this.defaultValuesMap = { 'ignore': ['CompetitiveGroups', 'OriginalDocumentsOptions', 'EducationLevels', 'EducationForms', 'Directions', 'Campaigns', 'Stages'] };
            this.defaultValues = ko.mapping.toJS(defaults, this.defaultValuesMap);
            //DataBoundGrid.FilterViewModelBase.call(this, data);

            DataBoundGrid.FilterViewModelBase.call(this, data,
            {
                'CompetitiveGroups': {
                    create: function(options) {
                        if (options.data == null) {
                            return null;
                        }
                        var list = { ShowUnselectedText: options.data.ShowUnselectedText, UnselectedText: options.data.UnselectedText, Items: []};
                        for (var i = 0; i < options.data.Items.length; i++) {
                            list.Items.push({ Id: options.data.Items[i].Id, DisplayName: options.data.Items[i].DisplayName });
                        }
                        return list;
                    }
                },
                'OriginalDocumentsOptions': {
                    create: function(options) {
                        if (options.data == null) {
                            return null;
                        }
                        var list = { ShowUnselectedText: options.data.ShowUnselectedText, UnselectedText: options.data.UnselectedText, Items: []};
                        for (var i = 0; i < options.data.Items.length; i++) {
                            list.Items.push({ Id: options.data.Items[i].Id, DisplayName: options.data.Items[i].DisplayName });
                        }
                        return list;
                    }
                },
                'EducationLevels': {
                    create: function(options) {
                        if (options.data == null) {
                            return null;
                        }
                        var list = { ShowUnselectedText: options.data.ShowUnselectedText, UnselectedText: options.data.UnselectedText, Items: []};
                        for (var i = 0; i < options.data.Items.length; i++) {
                            list.Items.push({ Id: options.data.Items[i].Id, DisplayName: options.data.Items[i].DisplayName });
                        }
                        return list;
                    }
                },
                'EducationForms': {
                    create: function(options) {
                        if (options.data == null) {
                            return null;
                        }
                        var list = { ShowUnselectedText: options.data.ShowUnselectedText, UnselectedText: options.data.UnselectedText, Items: []};
                        for (var i = 0; i < options.data.Items.length; i++) {
                            list.Items.push({ Id: options.data.Items[i].Id, DisplayName: options.data.Items[i].DisplayName });
                        }
                        return list;
                    }
                },
                'Directions': {
                    create: function(options) {
                        if (options.data == null) {
                            return null;
                        }
                        var list = { ShowUnselectedText: options.data.ShowUnselectedText, UnselectedText: options.data.UnselectedText, Items: []};
                        for (var i = 0; i < options.data.Items.length; i++) {
                            list.Items.push({ Id: options.data.Items[i].Id, DisplayName: options.data.Items[i].DisplayName });
                        }
                        return list;
                    }
                },
                'Campaigns': {
                    create: function(options) {
                        if (options.data == null) {
                            return null;
                        }
                        var list = { ShowUnselectedText: options.data.ShowUnselectedText, UnselectedText: options.data.UnselectedText, Items: []};
                        for (var i = 0; i < options.data.Items.length; i++) {
                            list.Items.push({ Id: options.data.Items[i].Id, DisplayName: options.data.Items[i].DisplayName });
                        }
                        return list;
                    }
                },
                'Stages': {
                    create: function(options) {
                        if (options.data == null) {
                            return null;
                        }
                        var list = { ShowUnselectedText: options.data.ShowUnselectedText, UnselectedText: options.data.UnselectedText, Items: []};
                        for (var i = 0; i < options.data.Items.length; i++) {
                            list.Items.push({ Id: options.data.Items[i].Id, DisplayName: options.data.Items[i].DisplayName });
                        }
                        return list;
                    }
                }
            });
        }

        RecommendedApplicationsFilterViewModel.prototype = DataBoundGrid.FilterViewModelBase.prototype;

        function RecommendedApplicationListViewModel() {
            DataBoundGrid.ListViewModelBase.call(this);
            this.TotalApplicationsCount = ko.observable(0);
            this.filterStats = ko.pureComputed(this.getFilterStats, this);
        }

        RecommendedApplicationListViewModel.prototype = WebUtils.extend({}, DataBoundGrid.ListViewModelBase.prototype);

        RecommendedApplicationListViewModel.prototype.getFilterStats = function () {
            var filteredTotal = this.pager.TotalRecords();
            var unfilteredTotal = this.TotalApplicationsCount();
            if (unfilteredTotal == filteredTotal) {
                return unfilteredTotal.toString();
            } else {
                return filteredTotal.toString() + ' из ' + unfilteredTotal.toString();
            }
        };
        
        RecommendedApplicationListViewModel.prototype.init = function (domElement, modelData, callback) {
            this.filter = new RecommendedApplicationsFilterViewModel(modelData.Filter, modelData.DefaultFilter);
            this.pager = new DataBoundGrid.PagerViewModel(modelData.Pager);
            this.sort({ SortKey: 'ApplicationNumber', SortDescending: false });
            
            DataBoundGrid.ListViewModelBase.prototype.init.call(this, domElement, function () {                            
                this.filter.SelectedCampaign.subscribe(this.forceReload, this);
                this.filter.SelectedStage.subscribe(this.forceReload, this);
                if (callback && typeof callback === 'function') {
                    callback.call(this);
                }
            });
        };

        RecommendedApplicationListViewModel.prototype.reload = function (callback) {
            var me = this;
            var queryModel = {
                Filter: this.filter.appliedValues() || this.filter.defaultValues,
                Pager: ko.mapping.toJS(this.pager, this.pager.mapping),
                Sort: ko.mapping.toJS(this.sort)
            };

            queryModel.Filter.SelectedCampaign = ko.utils.unwrapObservable(this.filter.SelectedCampaign);
            queryModel.Filter.SelectedStage = ko.utils.unwrapObservable(this.filter.SelectedStage);

            doPostAjax('<%= Url.Action("LoadApplicationRecommendedRecords", "InstitutionApplication") %>', ko.toJSON(queryModel), function (result) {
                me.records(result.Records);
                me.pager.update(result.Pager);
                DataBoundGrid.ListViewModelBase.prototype.reload.call(me, callback);
                me.TotalApplicationsCount(result.TotalApplicationsCount);
            }, null, null, true);
        };

        RecommendedApplicationListViewModel.prototype.actions = {
            view: function (record) {
                viewApplicationDetails(record.ApplicationId, record.ApplicationNumber, record.IsCampaignFinished, record.StatusID);
                return false;
            },
            includeInOrder: function (record) {
                if (record.IsCampaignFinished) {  return 'javascript:void(0)'; }
                var id = [];
                id.push(record.ApplicationId);
                appsSelectOrder(id);
                return false;
            },
            removeFromList: function (record) {
                if (record.IsCampaignFinished) {  return 'javascript:void(0)'; }
                var model = { recListId: Number(record.RecommendedListId), applicationId: Number(record.ApplicationId) };
                var me = this;
                doPostAjax('<%= Url.Action("ExcludeRecommendedList", "InstitutionApplication") %>', ko.toJSON(model), function (data) {
                    if (data && data.success === true) {
                        me.forceReload();
                    }
                }, null, null, true);
            }
        };

        var viewModelData = <%= Html.CustomJson(new {Model.Filter, Model.Pager, DefaultFilter = RecommendedApplicationsFilterViewModel.Default }) %>;
        var $container = $('#recommendedApplicationsContainer');
        $(".datePicker", $container).datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-40:+0' });
        var model = new RecommendedApplicationListViewModel();       
        model.init($container[0], viewModelData, function() {  $container.show(); });
    }
    function appsSelectOrder(model) {
        doPostAjax('<%= Url.Action("CheckApplicationsSelectOrder", "InstitutionApplication") %>', JSON.stringify(model),
            function(data) {
                if (data.IsError) {
                    infoDialog(data.Data.ErrorMessage);
                    return;
                } else {
                    window.location = '<%= Url.Generate<OrderOfAdmissionController>(x => x.ApplicationSelectOrder())%>';
                }
            });
    }
</script>

<div id="recommendedApplicationsContainer" style="display: none">
    
    <%--<!-- ko if: hasRecords -->
    <div style="padding: 8px;padding-left: 0">
        <input type="button" value="Выгрузить список" class="button4" />
    </div>
    <!-- /ko -->--%>

    <!-- ko with: filter -->
    <% Html.RenderPartial("InstitutionApplication/RecommendedListFilter", Model.Filter); %>
    <!-- /ko -->
    
    <!-- ko if: hasRecords -->
    <table class="gvuzDataGrid tableStatement2" cellpadding="3">
        <colgroup>
            <col style="width: 10% "/>
            <col style="width: 5%" />
            <col style="width: 10%"/>
            <col style="width: 5%"/>
            <col style="width: 25%" />
            <col style="width: 15%"/>
            <col style="width: 10%"/>
            <col style="width: 5%"/>
            <col style="width: 5%" />
            <col />
            <col />
        </colgroup>
        <thead>
            <tr>
                <th style="text-align: center;white-space: nowrap">
                    <a data-sort="CampaignName"><%= Html.LabelTextFor(x => x.RecordInfo.CampaignName) %></a>
                </th>
                <th style="text-align: center;white-space: nowrap">
                    <a data-sort="StageName"><%= Html.LabelTextFor(x => x.RecordInfo.StageName) %></a>
                </th>
                <th style="text-align: center;white-space: nowrap">
                    <a data-sort="ApplicationNumber"><%= Html.LabelTextFor(x => x.RecordInfo.ApplicationNumber) %></a>
                </th>
                <th style="text-align: center;white-space: nowrap">
                    <a data-sort="EntrantName"><%= Html.LabelTextFor(x => x.RecordInfo.EntrantFullName) %></a>
                </th>
                <th style="text-align: center;white-space: nowrap">
                    <a data-sort="EducationLevelName"><%= Html.LabelTextFor(x => x.RecordInfo.EducationLevelName) %></a>
                </th>
                <th style="text-align: center;white-space: nowrap">
                    <a data-sort="EducationFormName"><%= Html.LabelTextFor(x => x.RecordInfo.EducationFormName) %></a>
                </th>
                <th style="text-align: center;white-space: nowrap">
                    <a data-sort="CompetitiveGroupName"><%= Html.LabelTextFor(x => x.RecordInfo.CompetitiveGroupName) %></a>
                </th>
                <th style="text-align: center;white-space: nowrap">
                    <a data-sort="DirectionName"><%= Html.LabelTextFor(x => x.RecordInfo.DirectionName) %></a>
                </th>
                <th style="text-align: center;white-space: nowrap">
                    <a data-sort="OriginalDocumentsReceived"><%= Html.LabelTextFor(x => x.RecordInfo.OriginalDocumentsReceived)%></a>
                </th>
                <th style="text-align: center;white-space: nowrap">
                    <a data-sort="Rating"><%= Html.LabelTextFor(x => x.RecordInfo.Rating) %></a>
                </th>
                <th style="text-align: center;white-space: nowrap"><span>Действия</span></th>
            </tr>
        </thead>  
        <tbody data-bind="foreach: records">
            <tr data-bind="css: {trline2: ($index() % 2 == 0), trline1: ($index() % 2 != 0)}">
                <td><span data-bind="text: CampaignName"></span></td>
                <td style="text-align: center"><span data-bind="text: StageName"></span></td>
                <td style="text-align: center"><a href="javascript:void(0);" data-bind="text: ApplicationNumber, modelActionsClick: $root.actions.view" /></td>
                <td style="text-align: center"><span data-bind="text: EntrantFullName"></span></td>
                <td style="text-align: center"><span data-bind="text: EducationLevelName"></span></td>
                <td style="text-align: center"><span data-bind="text: EducationFormName"></span></td>
                <td><span data-bind="text: CompetitiveGroupName"></span></td>
                <td><span data-bind="text: DirectionName"></span></td>
                <td style="text-align: center"><span data-bind="text: (OriginalDocumentsReceived ? 'да' : 'нет')"></span></td>
                <td style="text-align: center"><span data-bind="text: Rating"></span></td>
                <td style="text-align: center;white-space: nowrap">
                    <% if(!ConfigHelper.HideOrderOfAdmissionMenu()) { %>
                    <a  title="Включить в приказ" data-bind="css: {disabled: DisableIncludeAction, btnMove: !DisableIncludeAction, btnMoveGray: DisableIncludeAction}, modelActionsClick: $root.actions.includeInOrder" />
                    <%} %>
                    <a class="btnDelete" href="javascript:void(0)" title="Исключить из списка рекомендованных" data-bind="css: {disabled: IsCampaignFinished, btnDeleteGray: IsCampaignFinished}, modelActionsClick: $root.actions.removeFromList" />
                </td>
            </tr>
        </tbody>
        <tfoot data-bind="with: pager">
            <tr>
                <th colspan="11">
                    <% Html.RenderPartial("InstitutionApplication/PagerView"); %>
                </th>
            </tr>
        </tfoot>
    </table>
    <!-- /ko -->
    
    <p data-bind="visible: !hasRecords()">
        <em>Не обнаружено ни одной записи, удовлетворяющей условиям поиска</em>
    </p>
</div>
<% Html.RenderPartial("InstitutionApplication/Dialogs/ViewApplication"); %>