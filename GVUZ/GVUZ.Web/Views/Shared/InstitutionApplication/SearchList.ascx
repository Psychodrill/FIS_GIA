<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.ApplicationsList.SearchApplicationsListViewModel>" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.ViewModels.ApplicationsList" %>
<%@ Import Namespace="GVUZ.Web.ViewModels.Entrants" %>

<script type="text/javascript">
    function searchListLoaded() {

        function SearchApplicationsFilterViewModel(data, defaults) {
            this.defaultValuesMap = { 'ignore': ['CompetitiveGroups', 'Statuses'] };
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
                'Statuses': {
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
       
        SearchApplicationsFilterViewModel.prototype = DataBoundGrid.FilterViewModelBase.prototype;

        function SearchApplicationListViewModel() {
            DataBoundGrid.ListViewModelBase.call(this);
            this.TotalApplicationsCount = ko.observable(0);
            this.filterStats = ko.pureComputed(this.getFilterStats, this);
        }

        SearchApplicationListViewModel.prototype = WebUtils.extend({}, DataBoundGrid.ListViewModelBase.prototype);

        SearchApplicationListViewModel.prototype.getFilterStats = function () {
            var filteredTotal = this.pager.TotalRecords();
            var unfilteredTotal = this.TotalApplicationsCount();
            if (unfilteredTotal == filteredTotal) {
                return unfilteredTotal.toString();
            } else {
                return filteredTotal.toString() + ' из ' + unfilteredTotal.toString();
            }
        };
        
        SearchApplicationListViewModel.prototype.init = function(domElement, modelData, callback) {
            this.filter = new SearchApplicationsFilterViewModel(modelData.Filter, modelData.DefaultFilter);
            this.pager = new DataBoundGrid.PagerViewModel(modelData.Pager);
            this.sort({ SortKey: 'RegistrationDate', SortDescending: true });
            DataBoundGrid.ListViewModelBase.prototype.init.call(this, domElement, callback);
        };

        SearchApplicationListViewModel.prototype.reload = function(callback) {
            var me = this;
            var queryModel = {
                Filter: this.filter.appliedValues() || this.filter.defaultValues,
                Pager: ko.mapping.toJS(this.pager, this.pager.mapping),
                Sort: ko.mapping.toJS(this.sort)
            };

            doPostAjax('<%= Url.Action("LoadApplicationSearchRecords", "InstitutionApplication") %>', ko.toJSON(queryModel), function(result) {
                me.records(result.Records);
                me.pager.update(result.Pager);
                DataBoundGrid.ListViewModelBase.prototype.reload.call(me, callback);
                me.TotalApplicationsCount(result.TotalApplicationsCount);
            }, null, null, true);
        };

        SearchApplicationListViewModel.prototype.actions = {
            view: function (record) {
                viewApplicationDetails(record.ApplicationId, record.ApplicationNumber, record.IsCampaignFinished, record.StatusID);
                return false;
            },
            edit: function (record) {
                if (!record.AllowEdit) {
                    return 'javascript:void(0)';
                }
                
                return editApplicationUrl(record.ApplicationId);
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
            },
            navigateToOrderUrl: function(record) {
                if (record.IsInOrder) {
                    var urlTemplate = '<%= Url.Action("EditOrder", "OrderOfAdmission", new { id = "__ORDERID__"}) %>';
                    if(record.IsCampaignFinished){
                        urlTemplate = '<%= Url.Action("ViewOrder", "OrderOfAdmission", new { id = "__ORDERID__"}) %>';
                    }
                    return urlTemplate.replace('__ORDERID__', record.OrderId);
                }
                return 'javascript:void(0)';
            }
        };

        var viewModelData = <%= Html.CustomJson(new {Model.Filter, Model.Pager, DefaultFilter = SearchApplicationsFilterViewModel.Default }) %>;
        var $container = $('#searchApplicationsContainer');
        $(".datePicker", $container).datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-40:+0' });
        var model = new SearchApplicationListViewModel();       
        
        model.init($container[0], viewModelData, function() { $container.show(); });
    }
    
</script>

<div id="searchApplicationsContainer" style="display: none" class="divstatement notabs">
    <%--<!-- ko if: hasRecords -->
    <div style="padding: 8px;padding-left: 0">
        <input type="button" value="Выгрузить список" class="button4" />
    </div>
    <!-- /ko -->--%>

    <!-- ko with: filter -->
    <% Html.RenderPartial("InstitutionApplication/SearchListFilter", Model.Filter); %>
    <!-- /ko -->
    
    <!-- ko if: hasRecords -->
    <table class="gvuzDataGrid tableStatement2" cellpadding="3">
        <colgroup>
            <col style="width: 10% "/> <%-- номер заявления --%>
            <col style="width: 5%"/> <%-- дата регистрации --%>
            <col style="width: 35%" /> <%-- названия КГ --%>
            <col style="width: 10%"/> <%--  статус --%>
            <col /> <%--  действия --%>
            <col style="border-left-style: double;border-left-color: #ededed;width: 25%"/> <%-- ФИО --%>
            <col style="width: 10%"/> <%-- паспорт  --%>
        </colgroup>
        <thead>
            <tr>
                <th class="header" colspan="5">Сведения о заявлении</th>
                <th class="header" colspan="2">Сведения об абитуриенте</th>
            </tr>
            <tr>
                <th>
                    <a class="sortable" data-sort="ApplicationNumber"><%= Html.LabelTextFor(x => x.RecordInfo.ApplicationNumber) %></a>
                </th>
                <th>
                    <a class="sortable" data-sort="RegistrationDate"><%= Html.LabelTextFor(x => x.RecordInfo.RegistrationDate) %></a>
                </th>
                <th>
                    <span><%= Html.LabelTextFor(x => x.RecordInfo.CompetitiveGroupNames) %></span>
                </th>
                <th>
                    <a class="sortable" data-sort="StatusName"><%= Html.LabelTextFor(x => x.RecordInfo.StatusName) %></a>
                </th>
                <th style="text-align: center"><span>Действия</span></th>
                <th>
                    <a class="sortable" data-sort="EntrantName"><%= Html.LabelTextFor(x => x.RecordInfo.EntrantFullName) %></a>
                </th>
                <th>
                    <a class="sortable" data-sort="IdentityDocument"><%= Html.LabelTextFor(x => x.RecordInfo.IdentityDocument) %></a>
                </th>
            </tr>
        </thead>  
        <tbody data-bind="foreach: records">
            <tr data-bind="css: {trline2: ($index() % 2 == 0), trline1: ($index() % 2 != 0)}">
                <td><a href="javascript:void(0);" data-bind="text: ApplicationNumber, modelActionsClick: $root.actions.view" /></td>
                <td><span data-bind="text: RegistrationDate"></span></td>
                <td><span data-bind="text: CompetitiveGroupNames"></span></td>
                <td><span data-bind="text: StatusName"></span></td>
                <td style="text-align: center;white-space: nowrap">
                    <a title="Редактировать" data-bind="attr: {href: $parent.actions.edit($data)}, css: {disabled: !AllowEdit, btnEdit: AllowEdit, btnEditGray: !AllowEdit}"></a>

                    <a title="К заявлению" data-bind="attr: {href: $parent.actions.navigate($data)}, css: {disabled: !AllowEdit, btnGoStat: AllowEdit, btnGoStatGray: !AllowEdit}"></a>

                    <a title="К приказу" data-bind="attr: {href: $parent.actions.navigateToOrderUrl($data)}, css: {disabled: !IsInOrder, btnGoOrder: IsInOrder, btnGoOrderGray: !IsInOrder}"></a>
                </td>
                <td><a href="javascript:void(0)" data-bind="text: EntrantFullName, modelActionsClick: $root.actions.entrantDetails"></a></td>
                <td><span data-bind="text: IdentityDocument"></span></td>
            </tr>
        </tbody>
        <tfoot data-bind="with: pager">
            <tr>
                <th colspan="7">
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
<% Html.RenderPartial("InstitutionApplication/Dialogs/EditApplication"); %>
<% Html.RenderPartial("~/Views/Entrant/EntrantDetailsDialog.ascx", EntrantDetailsViewModel.MetadataInstance); %>