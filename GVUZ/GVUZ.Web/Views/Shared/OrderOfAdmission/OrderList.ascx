<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.OrderOfAdmission.OrderOfAdmissionListViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.ViewModels.OrderOfAdmission" %>

<script type="text/javascript">
    function orderOfAdmissionListLoaded() {
        
        function OrderOfAdmissionFilterViewModel(data, defaults) {
            this.defaultValuesMap = { 'ignore': ['Campaigns', 'Stages', 'EducationLevels', 'EducationForms', 'EducationSources', 'IsForBeneficiaryList', 'IsForeignerList', 'OrderStatuses'] };
            this.defaultValues = ko.mapping.toJS(defaults, this.defaultValuesMap);
            DataBoundGrid.FilterViewModelBase.call(this, data);
        }

        OrderOfAdmissionFilterViewModel.prototype = DataBoundGrid.FilterViewModelBase.prototype;

        function OrderOfAdmissionListViewModel() {
            DataBoundGrid.ListViewModelBase.call(this);
            this.TotalOrdersCount = ko.observable(0);
            this.filterStats = ko.pureComputed(this.getFilterStats, this);
        }

        OrderOfAdmissionListViewModel.prototype = WebUtils.extend({}, DataBoundGrid.ListViewModelBase.prototype);

        OrderOfAdmissionListViewModel.prototype.getFilterStats = function() {
            var filteredTotal = this.pager.TotalRecords();
            var unfilteredTotal = this.TotalOrdersCount();
            if (unfilteredTotal == filteredTotal) {
                return unfilteredTotal.toString();
            } else {
                return filteredTotal.toString() + ' из ' + unfilteredTotal.toString();
            }
        };

        OrderOfAdmissionListViewModel.prototype.init = function (domElement, modelData, callback) {
            this.filter = new OrderOfAdmissionFilterViewModel(modelData.Filter, modelData.DefaultFilter);
            this.pager = new DataBoundGrid.PagerViewModel(modelData.Pager);
            this.sort({ SortKey: 'OrderNumber', SortDescending: false });
            DataBoundGrid.ListViewModelBase.prototype.init.call(this, domElement, function () {                            
                this.filter.SelectedCampaign.subscribe(this.forceReload, this);
                this.filter.SelectedStage.subscribe(this.forceReload, this);
                if (callback && typeof callback === 'function') {
                    callback.call(this);
                }
            });
        };

        OrderOfAdmissionListViewModel.prototype.reload = function (callback) {
            var me = this;
            var queryModel = {
                Filter: this.filter.appliedValues() || this.filter.defaultValues,
                Pager: ko.mapping.toJS(this.pager, this.pager.mapping),
                Sort: ko.mapping.toJS(this.sort)
            };

            queryModel.Filter.SelectedCampaign = ko.utils.unwrapObservable(this.filter.SelectedCampaign);
            queryModel.Filter.SelectedStage = ko.utils.unwrapObservable(this.filter.SelectedStage);
            
            doPostAjax('<%= Url.Action("LoadOrderOfAdmissionRecords", "OrderOfAdmission") %>', ko.toJSON(queryModel), function (result) {
                me.records(result.Records);
                me.pager.update(result.Pager);
                DataBoundGrid.ListViewModelBase.prototype.reload.call(me, callback);
                me.TotalOrdersCount(result.TotalOrdersCount);
            }, null, null, true);
        };
        
        OrderOfAdmissionListViewModel.prototype.actions = {
            addOrder: function() {
                <% if (Model.Filter.OrderTypeId == GVUZ.Data.Model.OrderOfAdmissionType.OrderOfAdmission)
                   {%>
                window.location = '<%= Url.Action("CreateOrderOfAdmission", "OrderOfAdmission") %>';
                <%}
                   else if (Model.Filter.OrderTypeId == GVUZ.Data.Model.OrderOfAdmissionType.OrderOfAdmissionRefuse)
                   {%>
                window.location = '<%= Url.Action("CreateOrderOfAdmissionRefuse", "OrderOfAdmission") %>';
                <%}%>
                
                return false;
            },
            editOrderUrl: function (record) {
                if (!record.DisableEditAction) {
                    return '<%= Url.Action("EditOrder", "OrderOfAdmission", new { id = "__ORDERID__"}) %>'.replace('__ORDERID__', record.OrderId);
                }
                return 'javascript:void(0)';
            },
            removeOrder: function(record) {
                if (!record.DisableDeleteAction) {
                    removeOrderOfAdmissionDialog(record.OrderId, this.forceReload, this);    
                }
                return false;
            },
            publishOrder: function(record) {
                if (!record.DisablePublishAction) {
                    publishOrderOfAdmission(record.OrderId, null, null, null, null, this.forceReload, this);
                }
                return false;
            },
            viewOrderUrl: function(record) {
                return '<%= Url.Action("ViewOrder", "OrderOfAdmission", new { id = "__ORDERID__"}) %>'.replace('__ORDERID__', record.OrderId);
            }
        };
            var viewModelData = <%= Html.CustomJson(new {Model.Filter, Model.Pager, DefaultFilter = OrderOfAdmissionFilterViewModel.Default}) %>;
        var $container = $('#orderOfAdmissionListContainer');
        $(".datePicker", $container).datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-40:+0' });
        var model = new OrderOfAdmissionListViewModel();       
        model.init($container[0], viewModelData, function() { $container.show(); });
    }
</script>

<div id="orderOfAdmissionListContainer" style="display: none">

    <div style="padding: 8px; padding-left: 0">
        <input type="button" class="button3" value="Добавить" data-bind="click: actions.addOrder" />
    </div>

    <!-- ko with: filter -->
    <% Html.RenderPartial("OrderOfAdmission/OrderListFilter", Model.Filter); %>
    <!-- /ko -->

    <!-- ko if: hasRecords -->
    <table class="gvuzDataGrid tableStatement2" cellpadding="3">
        <colgroup>
            <col style="width: 20%" />
            <%-- Наименование приказа --%>
            <col style="width: 5%" />
            <%-- Номер приказа --%>
            <col style="width: 5%" />
            <%-- Дата приказа --%>
            <col style="width: 10%" />
            <%-- Статус приказа --%>
            <col style="width: 15%" />
            <%-- Приемная кампания --%>
            <col style="width: 1%" />
            <%-- Этап приема --%>
            <col style="width: 5%" />
            <%-- Уровень образования --%>
            <col style="width: 11%" />
            <%-- Форма обучения --%>
            <col style="width: 5%" />
            <%-- Источник финансирования --%>
            <col style="width: 5%" />
            <%-- Кол-во абитуриентов --%>
            <col style="width: 1%" />
            <%-- Льготный приказ --%>
            <col style="width: 1%" />
            <%-- Прием по направлениям Минобрнауки --%>
            <col />
            <%--  действия --%>
        </colgroup>
        <thead>
            <tr>
                <th><a class="sortable" data-sort="OrderName"><%= Html.LabelTextFor(x => x.RecordInfo.OrderName) %></a></th>
                <th><a class="sortable" data-sort="OrderNumber"><%= Html.LabelTextFor(x => x.RecordInfo.OrderNumber) %></a></th>
                <th><a class="sortable" data-sort="OrderDate"><%= Html.LabelTextFor(x => x.RecordInfo.OrderDate) %></a></th>
                <th><a class="sortable" data-sort="OrderStatusId"><%= Html.LabelTextFor(x => x.RecordInfo.OrderStatusName) %></a></th>
                <th><a class="sortable" data-sort="CampaignName"><%= Html.LabelTextFor(x => x.RecordInfo.CampaignName) %></a></th>
                <th><a class="sortable" data-sort="Stage"><%= Html.LabelTextFor(x => x.RecordInfo.Stage) %></a></th>
                <th><a class="sortable" data-sort="EducationLevel"><%= Html.LabelTextFor(x => x.RecordInfo.EducationLevel) %></a></th>
                <th><a class="sortable" data-sort="EducationForm"><%= Html.LabelTextFor(x => x.RecordInfo.EducationForm) %></a></th>
                <th><a class="sortable" data-sort="EducationSource"><%= Html.LabelTextFor(x => x.RecordInfo.EducationSource) %></a></th>
                <th><a class="sortable" data-sort="NumberOfApplicants"><%= Html.LabelTextFor(x => x.RecordInfo.NumberOfApplicants) %></a></th>
                <th><a class="sortable" data-sort="IsForBeneficiary"><%= Html.LabelTextFor(x => x.RecordInfo.IsForBeneficiary) %></a></th>
                <th><a class="sortable" data-sort="IsForeigner"><%= Html.LabelTextFor(x => x.RecordInfo.IsForeigner) %></a></th>
                <th style="text-align: center; white-space: nowrap"><span>Действия</span></th>
            </tr>
        </thead>
        <tbody data-bind="foreach: records">
            <tr data-bind="css: {trline2: ($index() % 2 == 0), trline1: ($index() % 2 != 0)}">
                <td><span data-bind="text: OrderName"></span></td>
                <td ><a data-bind="text: OrderNumber, attr: { href: $root.actions.viewOrderUrl($data) }"></a></td>
                <td ><span data-bind="text: OrderDate"></span></td>
                <td ><span data-bind="text: OrderStatusName"></span></td>
                <td><span data-bind="text: CampaignName"></span></td>
                <td ><span data-bind="text: Stage"></span></td>
                <td ><span data-bind="text: EducationLevel"></span></td>
                <td ><span data-bind="text: EducationForm"></span></td>
                <td ><span data-bind="text: EducationSource"></span></td>
                <td ><span data-bind="text: NumberOfApplicants"></span></td>
                <td ><span data-bind="text: IsForBeneficiary ? 'да' : 'нет'"></span></td>
                <td ><span data-bind="text: IsForeigner ? 'да' : 'нет'"></span></td>
                <td style="text-align: center; white-space: nowrap">
                    <a title="Просмотреть" data-bind="attr: {href: $root.actions.viewOrderUrl($data)}, css: {btnView: true }"></a>
                    <a title="Опубликовать" href="javascript:void(0)" data-bind="css: {disabled: DisablePublishAction, btnPublish: !DisablePublishAction, btnPublishGray: DisablePublishAction}, modelActionsClick: $root.actions.publishOrder"></a>
                    <a title="Редактировать" data-bind="attr: {href: $root.actions.editOrderUrl($data)}, css: {btnEdit: !DisableEditAction, btnEditGray: DisableEditAction, disabled: DisableEditAction}"></a>
                    <a title="Удалить" href="javascript:void(0)" data-bind="css: {btnDelete: !DisableDeleteAction, btnDeleteGray: DisableDeleteAction, disabled: DisableDeleteAction}, modelActionsClick: $root.actions.removeOrder"></a>
                </td>
            </tr>
        </tbody>
        <tfoot data-bind="with: pager">
            <tr>
                <th colspan="13">
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
<% Html.RenderPartial("OrderOfAdmission/Dialogs/RemoveOrderDialog"); %>
<% Html.RenderPartial("OrderOfAdmission/Dialogs/PublishOrderDialog"); %>