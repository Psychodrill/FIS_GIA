<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.InstitutionDirectionRequestListViewModel>" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

<script type="text/javascript">
    function requestListLoaded() {

        function InstitutionDirectionRequestListViewModel() {
            DataBoundGrid.ListViewModelBase.call(this);
        }

        InstitutionDirectionRequestListViewModel.prototype = WebUtils.extend({}, DataBoundGrid.ListViewModelBase.prototype);
        
        InstitutionDirectionRequestListViewModel.prototype.init = function (domElement, modelData, callback) {
            this.pager = new DataBoundGrid.PagerViewModel(modelData.Pager);
            this.sort({ SortKey: 'InstitutionName', SortDescending: false });
            DataBoundGrid.ListViewModelBase.prototype.init.call(this, domElement, callback);
        };

        InstitutionDirectionRequestListViewModel.prototype.reload = function (callback) {
            var me = this;
            var queryModel = {
                Pager: ko.mapping.toJS(this.pager, this.pager.mapping),
                Sort: ko.mapping.toJS(this.sort)
            };

            doPostAjax('<%= Url.Generate<GVUZ.Web.Controllers.RequestHandlerController>(c => c.LoadRequestListData(null)) %>', ko.toJSON(queryModel), function (result) {
                me.records(result.Records);
                me.pager.update(result.Pager);
                DataBoundGrid.ListViewModelBase.prototype.reload.call(me, callback);
            }, null, null, true);
        };
        
        InstitutionDirectionRequestListViewModel.prototype.actions = {
            view: function (record) { 
                showRequestDetails(record.InstitutionId, this.forceReload, this);
                return false;
            }
        };
        var viewModelData = <%= Html.CustomJson(new {Model.Pager}) %>;

        var $container = $('#directionRequestListContainer');
        var model = new InstitutionDirectionRequestListViewModel();       
        model.init($container[0], viewModelData, function() { $container.show(); });
    }
    
</script>

<div id="directionRequestListContainer" style="display: none">
    
    <!-- ko if: hasRecords -->
    <table class="gvuzDataGrid tableStatement2" cellpadding="3">
        <colgroup>
            <col style="width: 70%"  /> <%-- Наименование ОО --%>
            <col style="width: 10% "/> <%-- Общее количество заявок --%>
            <col style="width: 20%"/> <%--  Дата отправки запроса --%>
        </colgroup>
        <thead>
            <tr>
                <th style="text-align: left">
                    <a data-sort="InstitutionName"><%= Html.LabelTextFor(x => x.RecordInfo.InstitutionName) %></a>
                </th>
                <th style="text-align: center">
                    <a data-sort="NumRequests"><%= Html.LabelTextFor(x => x.RecordInfo.NumRequests) %></a>
                </th>
                <th style="text-align: center">
                    <a data-sort="LastRequestDate"><%= Html.LabelTextFor(x => x.RecordInfo.LastRequestDate) %></a>
                </th>
            </tr>
        </thead>  
        <tbody data-bind="foreach: records">
            <tr data-bind="css: {trline2: ($index() % 2 == 0), trline1: ($index() % 2 != 0)}">
                <td style="text-align: left"><a href="javascript:void(0)" data-bind="text: InstitutionName, modelActionsClick: $root.actions.view" /></td>
                <td style="text-align: center"><span data-bind="text: NumRequests"></span></td>
                <td style="text-align: center"><span data-bind="text: LastRequestDate"></span></td>
            </tr>
        </tbody>
        <tfoot data-bind="with: pager">
            <tr>
                <th colspan="10">
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
