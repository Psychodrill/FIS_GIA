<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.Entrants.EntrantRecordListViewModel>" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.ViewModels.Entrants" %>

<script type="text/javascript">

    function entrantsListLoaded() {

        function EntrantRecordListFilterViewModel(data, defaults) {
            this.defaultValuesMap = { 'ignore': ['CompetitiveGroups', 'Campaigns', 'Statuses', 'CampaignYears'] };
            this.defaultValues = ko.mapping.toJS(defaults, this.defaultValuesMap);
            DataBoundGrid.FilterViewModelBase.call(this, data);
            this.isVisible(true);
            this.extendedFilterVisible = ko.observable(false);
            this.simpleFilterVisible = ko.pureComputed(function () {
                return !this.extendedFilterVisible();
            }, this);
        }

        EntrantRecordListFilterViewModel.prototype = WebUtils.extend({}, DataBoundGrid.FilterViewModelBase.prototype);
        EntrantRecordListFilterViewModel.prototype.showExtendedFilter = function () {
            this.extendedFilterVisible(true);
        };
        
        EntrantRecordListFilterViewModel.prototype.showSimpleFilter = function () {
            this.extendedFilterVisible(false);
        };

        function EntrantRecordListViewModel() {
            DataBoundGrid.ListViewModelBase.call(this);
            this.TotalRecordsCount = ko.observable(0);
            this.filterStats = ko.pureComputed(this.getFilterStats, this);
        }

        EntrantRecordListViewModel.prototype = WebUtils.extend({}, DataBoundGrid.ListViewModelBase.prototype);

//        EntrantRecordListViewModel.prototype.firstApplication = function(record) {
//            return record.Records[0];
//        };
//        EntrantRecordListViewModel.prototype.otherApplications = function(record) {
//            var others = [];
//            for (var i = 1; i < record.Records.length; i++) {
//                others.push(record.Records[i]);
//            }
//            others.reverse();
//            return others;
//        };

        EntrantRecordListViewModel.prototype.getFilterStats = function () {
            var filteredTotal = this.pager.TotalRecords();
            var unfilteredTotal = this.TotalRecordsCount();
            if (unfilteredTotal == filteredTotal) {
                return unfilteredTotal.toString();
            } else {
                return filteredTotal.toString() + ' из ' + unfilteredTotal.toString();
            }
        };

        EntrantRecordListViewModel.prototype.init = function (domElement, modelData, callback) {
            this.filter = new EntrantRecordListFilterViewModel(modelData.Filter, modelData.DefaultFilter);
            this.pager = new DataBoundGrid.PagerViewModel(modelData.Pager);
            this.sort({ SortKey: 'EntrantName', SortDescending: false });
            DataBoundGrid.ListViewModelBase.prototype.init.call(this, domElement, callback);
        };

        EntrantRecordListViewModel.prototype.reload = function (callback) {
            var me = this;
            var queryModel = {
                Filter: this.filter.appliedValues() || this.filter.defaultValues,
                Pager: ko.mapping.toJS(this.pager, this.pager.mapping),
                Sort: ko.mapping.toJS(this.sort)
            };

            doPostAjax('<%= Url.Action("LoadEntrantListRecords", "Entrant") %>', ko.toJSON(queryModel), function(result) {
                me.records(result.Records);
                me.pager.update(result.Pager);
                DataBoundGrid.ListViewModelBase.prototype.reload.call(me, callback);
                me.TotalRecordsCount(result.TotalRecordsCount);
            }, null, null, true);
        };

        EntrantRecordListViewModel.prototype.actions = {
            editApplication: function (record) {
                return record.AllowEdit === true ? editApplicationUrl(record.ApplicationId) : '#';
            },
            navigate: function (record) {
                if (record.AllowEdit != true) {
                    return '#';
                }
                var url = '<%= Url.Action("NavigateToList", "InstitutionApplication", new { applicationId = "APPID"}) %>';
                return url.replace("APPID", record.ApplicationId.toString());
            },
            entrantDetails: function (record) {
                showEntrantDetails(record.EntrantId);
                return false;
            }
        };

        var viewModelData = <%= Html.CustomJson(new {Model.Filter, Model.Pager, DefaultFilter = EntrantRecordListFilterViewModel.Default}) %>;

        var $container = $('#entrantListContainer');
        $(".datePicker", $container).datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-40:+0' });

        var model = new EntrantRecordListViewModel();
        model.init($container[0], viewModelData, function() { $container.show(); });
    }
</script>

<div id="entrantListContainer" style="display: none">
    
    <!-- ko with: filter -->
    <% Html.RenderPartial("EntrantsListGridFilter", Model.Filter); %>
    <!-- /ko -->
    
    <!-- ko if: hasRecords -->
    <table class="gvuzDataGrid tableStatement2" cellpadding="3">
        <colgroup>
            <col style="width: 20%"  /> <%-- ФИО --%>
            <col style="width: 10%"  /> <%-- Документ --%>
            <col style="width: 15% "/> <%-- № заявления --%>
            <col style="width: 5%"/> <%-- дата регистрации --%>
            <col style="width: 20%"/> <%-- название ПК --%>
            <col style="width: 2%"/> <%-- название КГ --%>
            <col style="width: 8%" /> <%-- Статус  --%>
            <col style="width: 3%" /> <%-- Действия --%>
        </colgroup>
        <thead>
            <tr>
                <th>
                    <a class="sortable" data-sort="EntrantName"><%= Html.LabelTextFor(x => x.RecordInfo.EntrantName) %></a>
                </th>
                <th>
                    <a class="sortable" data-sort="IdentityDocument"><%= Html.LabelTextFor(x => x.RecordInfo.IdentityDocument)%></a>
                </th>
                <th>
                    <span><%= Html.LabelTextFor(x => x.RecordInfo.ApplicationNumber)%></span>
                </th>
                <th>
                    <span><%= Html.LabelTextFor(x => x.RecordInfo.RegistrationDate)%></span>
                </th>
                <th>
                    <span><%= Html.LabelTextFor(x => x.RecordInfo.CampaignName)%></span>
                </th>
                <th>
                    <span><%= Html.LabelTextFor(x => x.RecordInfo.CompetitiveGroupNames)%></span>
                </th>
                <th>
                    <span><%= Html.LabelTextFor(x => x.RecordInfo.StatusName)%></span>
                </th>
                <th style="text-align: center"><span>Действия</span></th>
            </tr>
        </thead>  
        <tbody data-bind="foreach: records">
            <tr data-bind="css: {trline2: ($index() % 2 == 0), trline1: ($index() % 2 != 0)}">
                <td><a href="javascript:void(0)" data-bind="text: EntrantName, modelActionsClick: $root.actions.entrantDetails"></a></td>
                <td><span data-bind="text: IdentityDocument"></span></td>
                <%-- ko template: {name: 'entrantApplicationFields', with: $root.firstApplication($data)} --%>
                <%-- /ko --%>
                <td><span data-bind="text: ApplicationNumber"></span></td>
                <td><span data-bind="text: RegistrationDate"></span></td>
                <td><span data-bind="text: CampaignName"></span></td>
                <td><span data-bind="text: CompetitiveGroupNames"></span></td>
                <td><span data-bind="text: StatusName"></span></td>
                <td>
                    <a title="Редактировать" data-bind="attr: {href: $root.actions.editApplication($data)}, css: {disabled: !AllowEdit, btnEdit: AllowEdit, btnEditGray: !AllowEdit}" >&nbsp;</a>
                    <a title="К заявлению" data-bind="attr: {href: $root.actions.navigate($data)}, css: {disabled: !AllowEdit, btnGoStat: AllowEdit, btnGoStatGray: !AllowEdit}" >&nbsp;</a>
                </td>
            </tr>
            <%-- ko template: { name: 'additionalRows', foreach: $root.otherApplications($data)} --%>
            <%-- /ko --%>
        </tbody>
        <tfoot data-bind="with: pager">
            <tr>
                <th colspan="8">
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
<% Html.RenderPartial("EntrantDetailsDialog", EntrantDetailsViewModel.MetadataInstance); %>
<% Html.RenderPartial("InstitutionApplication/Dialogs/EditApplication"); %> <%--  вызывается из грида и из диалога --%>
<% Html.RenderPartial("InstitutionApplication/Dialogs/ViewApplication"); %> <%-- вызывается из диалога --%>
<%--
<script type="text/html" id="additionalRows">
    <tr data-bind="template: {name: 'entrantApplicationFields'}">
    </tr>
</script>
<script type="text/html" id="entrantApplicationFields">
    <td><span data-bind="text: ApplicationNumber"></span></td>
    <td><span data-bind="text: RegistrationDate"></span></td>
    <td><span data-bind="text: CampaignName"></span></td>
    <td><span data-bind="text: CompetitiveGroupNames"></span></td>
    <td><span data-bind="text: StatusName"></span></td>
    <td>
        <a title="Редактировать заявление" data-bind="attr: {href: $root.actions.editApplication($data)}, css: {disabled: !AllowEdit, btnEdit: AllowEdit, btnEditGray: !AllowEdit}" >&nbsp;</a>
        <a title="Перейти к заявлению в списке" class="btnView" data-bind="attr: {href: $root.actions.navigateApplication($data)}" >&nbsp;</a>
    </td>
</script>--%>