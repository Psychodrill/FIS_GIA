<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.CompositionResults.CompositionResultsListViewModel>" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.ViewModels.CompositionResults" %>

<script type="text/javascript">

    function compositionResultsListLoaded() {

        function CompositionResultsFilterViewModel(data, defaults) {
            this.defaultValuesMap = {'ignore': ['Campaigns', 'CompetitiveGroups'] };
            this.defaultValues = ko.mapping.toJS(defaults, this.defaultValuesMap);
            DataBoundGrid.FilterViewModelBase.call(this, data);
        }

        CompositionResultsFilterViewModel.prototype = DataBoundGrid.FilterViewModelBase.prototype;

        function CompositionResultsListViewModel() {
            DataBoundGrid.ListViewModelBase.call(this);
            this.isSelectedRecords = ko.pureComputed(this.getSelectedRecords, this);
        }

        CompositionResultsListViewModel.prototype = WebUtils.extend({}, DataBoundGrid.ListViewModelBase.prototype);

        CompositionResultsListViewModel.prototype.init = function (domElement, modelData, callback) {
            this.filter = new CompositionResultsFilterViewModel(modelData.Filter, modelData.DefaultFilter);
            this.filter.isVisible(true);
            this.pager = new DataBoundGrid.PagerViewModel(modelData.Pager);
            this.sort({ SortKey: 'RegistrationDate', SortDescending: false });
            DataBoundGrid.ListViewModelBase.prototype.init.call(this, domElement, callback);
        };

        CompositionResultsListViewModel.prototype.reload = function (callback) {
            var me = this;

            doPostAjax('<%= Url.Action("LoadCompositionResultsRecords", "CompositionResultsExport") %>', 
                ko.toJSON(this.getSubmitModel()), function (result) {
                me.records(result.Records);
                me.pager.update(result.Pager);
                DataBoundGrid.ListViewModelBase.prototype.reload.call(me, callback);
            }, null, null, true);
        };

        CompositionResultsListViewModel.prototype.getSubmitModel = function() {
            return {
                Filter: this.filter.appliedValues() || this.filter.defaultValues,
                Pager: ko.mapping.toJS(this.pager, this.pager.mapping),
                Sort: ko.mapping.toJS(this.sort)
            };
        };

        CompositionResultsListViewModel.prototype.getDownloadForm = function () {
            var $frame = $('#fileDownload').contents().find('body');
            var $form = $frame.find('#downloadForm').first();
            if ($form.length == 0) {
                $form = $('<form />').attr('method', 'POST').attr('id', 'downloadForm').
                    append($('<input />').attr('type', 'hidden').attr('name', 'submitModel')).appendTo($frame);
            }
            return $form;
        };

        CompositionResultsListViewModel.prototype.getSelectedRecords = function () {
            return this.hasSelectedRecords();
        };

        CompositionResultsListViewModel.prototype.getSelectedApplicationsId = function () {
            var id = [];
            ko.utils.arrayMap(this.selectedRecords(), function(selectedRecord) {
                id.push(selectedRecord.ApplicationId);
            });
            return id;
        };

        CompositionResultsListViewModel.prototype.actions = {
            updateResults: function () {
                var me = this;
                doPostAjax('<%= Url.Action("UpdateCompositionResults", "CompositionResultsExport") %>', null,
                    function (data) {
                        if (!data.success) { 
                            alert('Техническая ошибка при обновлении сведений о сочинениях!');
                            return; 
                        }
                        else{
                            me.forceReload();
                            alert('Результаты сочинений обновлены');
                        }
                    }, null, null, true);
            },
            exportCsv: function () {
                var $downloadForm = this.getDownloadForm();
                $downloadForm.attr('action', '<%= Url.Action("ExportCsv", "CompositionResultsExport") %>').
                    find('input').first().val(encodeURIComponent(ko.toJSON(this.getSubmitModel())));
                $downloadForm.submit();
            },
            exportHtml: function () {
                var $downloadForm = this.getDownloadForm();
                $downloadForm.attr('action', '<%= Url.Action("ExportHtml", "CompositionResultsExport") %>').
                    find('input').first().val(encodeURIComponent(ko.toJSON(this.getSubmitModel())));
                $downloadForm.submit();
            },
            exportCompositions: function () {

                var ids = this.getSelectedApplicationsId();
                if(ids.length > 100)
                {
                    alert("Максимальное разовое кол-во выбранных абитуриентов - 100!");
                    return;
                }

                var $downloadForm = this.getDownloadForm();
                $downloadForm.attr('action', '<%= Url.Action("ExportCompositions", "CompositionResultsExport") %>').
                    find('input').first().val(encodeURIComponent(ko.toJSON(ids)));
                $downloadForm.submit();
            }
        };

        var viewModelData = <%= Html.CustomJson(new {Model.Filter, Model.Pager,
           DefaultFilter = CompositionResultsFilterViewModel.Default}) %>;

        var $container = $('#compositionResultsListContainer');
        $(".datePicker", $container).datepicker({ changeMonth: true, changeYear: true, showOn: "button", 
             buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-40:+0' });
        
        var model = new CompositionResultsListViewModel();
        model.init($container[0], viewModelData, function() { $container.show(); });
    }
</script>

<div id="compositionResultsListContainer" style="display: none">
    
    <div style="padding: 8px;padding-left: 0">
        <input type="button" value="Обновить" class="button4" data-bind="click: actions.updateResults" />
        <input type="button" value="Выгрузить выбранные результаты" class="button4" 
            data-bind="click: actions.exportCompositions, attr: {disabled: !isSelectedRecords()}" />
    </div>

    <!-- ko with: filter -->
    <% Html.RenderPartial("CompositionListFilter", Model.Filter); %>
    <!-- /ko -->
    
    <!-- ko if: hasRecords -->
    <table class="gvuzDataGrid tableStatement2" cellpadding="3">
        <colgroup>
            <col style="width: 16px" />
            <col style="width: 10%" /> <%-- Фамилия --%>
            <col style="width: 10%" /> <%-- Имя --%>
            <col style="width: 10%" /> <%-- Отчество --%>
            <col style="width: 5%" /> <%-- Серия документа --%>
            <col style="width: 5%" /> <%-- Номер документа --%>
            <col style="width: 30%" /> <%-- Тема сочинения --%>
            <col style="width: 5%" /> <%-- Результат --%>
            <col style="width: 5%" /> <%-- ApplicationNumber --%>
            <col style="width: 5%"/> <%-- Дата регистрации --%>
        </colgroup>
        <thead>
            <tr>
                <th>
                    <input type="checkbox" data-bind="checked: selectAllRecords" />
                </th>
                <th>
                    <span><%= Html.LabelTextFor(x => x.RecordInfo.LastName) %></span>
                </th>
                <th>
                    <span><%= Html.LabelTextFor(x => x.RecordInfo.FirstName) %></span>
                </th>
                <th>
                    <span><%= Html.LabelTextFor(x => x.RecordInfo.MiddleName) %></span>
                </th>
                <th>
                    <span><%= Html.LabelTextFor(x => x.RecordInfo.IdentityDocumentSeries) %></span>
                </th>
                <th>
                    <span><%= Html.LabelTextFor(x => x.RecordInfo.IdentityDocumentNumber) %></span>
                </th>

                <th>
                    <span><%= Html.LabelTextFor(x => x.RecordInfo.CompositionTitle) %></span>
                </th>

                <th>
                    <span><%= Html.LabelTextFor(x => x.RecordInfo.CompositionResult) %></span>
                </th>

                <th>
                    <span><%= Html.LabelTextFor(x => x.RecordInfo.ApplicationNumber) %></span>
                </th>
                <th>
                    <span><%= Html.LabelTextFor(x => x.RecordInfo.RegistrationDate) %></span>
                </th>
            </tr>
        </thead>  
        <tbody data-bind="foreach: records">
            <tr data-bind="css: {trline2: ($index() % 2 == 0), trline1: ($index() % 2 != 0)}">
                <td><input type="checkbox" data-bind="checkedValue: $data, checked: $parent.selectedRecords" /></td>
                <td><span data-bind="text: LastName"></span></td>
                <td><span data-bind="text: FirstName"></span></td>
                <td><span data-bind="text: MiddleName"></span></td>
                <td><span data-bind="text: IdentityDocumentSeries"></span></td>
                <td><span data-bind="text: IdentityDocumentNumber"></span></td>
                <td><span data-bind="text: CompositionTitle"></span></td>
                <td style="text-align: center"><span data-bind="text: CompositionResultText"></span></td>
                <td><span data-bind="text: ApplicationNumber"></span></td>
                <td style="text-align: center"><span data-bind="text: RegistrationDate"></span></td>
            </tr>
        </tbody>
        <tfoot data-bind="with: pager">
            <tr><th colspan="10"><% Html.RenderPartial("InstitutionApplication/PagerView"); %></th></tr>
        </tfoot>
    </table>
    <!-- /ko -->
    
    <p data-bind="visible: !hasRecords()">
        <em>Не обнаружено ни одной записи, удовлетворяющей условиям поиска</em>
    </p>
    
    <iframe id="fileDownload" style="display: none;width: 0;height: 0"></iframe>
</div>