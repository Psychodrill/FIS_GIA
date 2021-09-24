<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.ApplicationsList.UncheckedApplicationsListViewModel>" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.ViewModels.ApplicationsList" %>

<script type="text/javascript">
    function uncheckedListLoaded() {

        <%--  Частная реализация фильтра для заявлений, не прошедших проверку --%>
        function UncheckedApplicationsFilterViewModel(data, defaults) {
            <%--
            Указанные свойства не будут передаваться в модель которая в итоге поедет на сервер
            Полезно, чтобы исключить списки и readonly-свойства серверной view-модели
            --%>
            this.defaultValuesMap = { 'ignore': ['CompetitiveGroups', 'Benefits', 'RecommendedListsOptions', 'Campaigns', 'EducationFormTypes', 'EducationSourceTypes', 'ViolationTypes', 'OriginalDocumentsOptions', 'IncludeInRecommendedLists', 'CampaignYears'] };
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
    'Benefits': {
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
    'RecommendedListsOptions': {
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
    'ViolationTypes': {
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
    'IncludeInRecommendedLists': {
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
       
        UncheckedApplicationsFilterViewModel.prototype = DataBoundGrid.FilterViewModelBase.prototype;

        function UncheckedApplicationListViewModel() {
            DataBoundGrid.ListViewModelBase.call(this);
            this.selectedApplicationsId = ko.pureComputed(this.getSelectedApplicationsId, this);
            this.isSelectedCampaignActive = ko.pureComputed(this.getSelectedCampaignActive, this);
            this.TotalApplicationsCount = ko.observable(0);
            this.filterStats = ko.pureComputed(this.getFilterStats, this);
            this.isSelectedRecords = ko.pureComputed(this.getSelectedRecords, this);
            this.isRecords = ko.pureComputed(this.getRecords, this);
        }

        UncheckedApplicationListViewModel.prototype = WebUtils.extend({}, DataBoundGrid.ListViewModelBase.prototype);

        UncheckedApplicationListViewModel.prototype.getFilterStats = function () {
            var filteredTotal = this.pager.TotalRecords();
            var unfilteredTotal = this.TotalApplicationsCount();
            if (unfilteredTotal == filteredTotal) {
                return unfilteredTotal.toString();
            } else {
                return filteredTotal.toString() + ' из ' + unfilteredTotal.toString();
            }
        };
        
        UncheckedApplicationListViewModel.prototype.isHighlighted = function (record) {
            return (this.HighlightApplicationId && Number(this.HighlightApplicationId) === Number(record.ApplicationId));
        };
        
        UncheckedApplicationListViewModel.prototype.getSelectedApplicationsId = function() {
            var id = [];
            ko.utils.arrayMap(this.selectedRecords(), function(selectedRecord) {
                id.push(selectedRecord.ApplicationId);
            });

            return id;
        };
        
        UncheckedApplicationListViewModel.prototype.getSelectedCampaignActive = function () {
            return this.hasSelectedRecords() && this.selectedRecords().every(function (item) {
                return item.IsCampaignFinished !== true;
            });
        };
        
        UncheckedApplicationListViewModel.prototype.getSelectedRecords = function () {
            return this.hasSelectedRecords();
        };

        UncheckedApplicationListViewModel.prototype.getRecords = function () {
            return this.TotalApplicationsCount > 0;
        };

        UncheckedApplicationListViewModel.prototype.init = function(domElement, modelData, callback) {
            this.filter = new UncheckedApplicationsFilterViewModel(modelData.Filter, modelData.DefaultFilter);
            this.pager = new DataBoundGrid.PagerViewModel(modelData.Pager);
            //go to last page
            if (+getCookie('ApplicationListPage')) {
                this.pager.CurrentPage(+getCookie('ApplicationListPage'));   
            }
            this.sort({ SortKey: 'RegistrationDate', SortDescending: true });
            this.HighlightApplicationId = Number(modelData.HighlightApplicationId);
            DataBoundGrid.ListViewModelBase.prototype.init.call(this, domElement, callback);
        };

        UncheckedApplicationListViewModel.prototype.reload = function(callback) {
            var me = this;
            var queryModel = {
                Filter: this.filter.appliedValues() || this.filter.defaultValues,
                Pager: ko.mapping.toJS(this.pager, this.pager.mapping),
                Sort: ko.mapping.toJS(this.sort)
            };
            //save last page  
            if (queryModel.Pager.CurrentPage != +getCookie('ApplicationListPage')) {
                //console.log(queryModel.Pager.CurrentPage);
                setCookie('ApplicationListPage', JSON.stringify(queryModel.Pager.CurrentPage), 1);
            }

            doPostAjax('<%= Url.Action("LoadApplicationUncheckedRecords", "InstitutionApplication") %>', ko.toJSON(queryModel), function(result) {
                me.records(result.Records);
                me.pager.update(result.Pager);
                DataBoundGrid.ListViewModelBase.prototype.reload.call(me, callback);
                me.TotalApplicationsCount(result.TotalApplicationsCount);
            }, null, null, true);
        };

        UncheckedApplicationListViewModel.prototype.getDownloadForm = function () {
            var $frame = $('#fileDownload').contents().find('body');
            var $form = $frame.find('#downloadForm').first();
            if ($form.length == 0) {
                $form = $('<form />').attr('method', 'POST').attr('id', 'downloadForm').append($('<input />').attr('type', 'hidden').attr('name', 'submitModel')).appendTo($frame);
            }
            return $form;
        };

        UncheckedApplicationListViewModel.prototype.getSubmitModel = function() {
            return {
                Filter: this.filter.appliedValues() || this.filter.defaultValues,
                Pager: ko.mapping.toJS(this.pager, this.pager.mapping),
                Sort: ko.mapping.toJS(this.sort)
            };
        };


        UncheckedApplicationListViewModel.prototype.actions = {
            view: function (record) {
                viewApplicationDetails(record.ApplicationId, record.ApplicationNumber, record.IsCampaignFinished, record.StatusID);
                return false;
            },
            edit: function(record) {
                if (record.IsCampaignFinished) {      return 'javascript:void(0)';}
               return editApplicationUrl(record.ApplicationId);
            },
            check: function(record) {
                if (record.IsCampaignFinished) {      return 'javascript:void(0)';}
                checkApplicationsDialog(record.ApplicationId, this.forceReload, this);
                return false;
            },
            checkSelected: function() {
                checkApplicationsDialog(this.selectedApplicationsId(), this.forceReload, this);
                return false;
            },
            accept: function(record) {
                if (record.IsCampaignFinished) {      return 'javascript:void(0)';}
                acceptApplicationsDialog(record.ApplicationId, this.forceReload, this);
                return false;
            },
            acceptSelected: function() {
                acceptApplicationsDialog(this.selectedApplicationsId(), this.forceReload, this);
                return false;
            },
            revoke: function(record) {
                if (record.IsCampaignFinished) {      return 'javascript:void(0)';}
                revokeApplicationsDialog(record.ApplicationId, this.forceReload, this);
                return false;
            },
            revokeSelected: function() {
                revokeApplicationsDialog(this.selectedApplicationsId(), this.forceReload, this);
                return false;
            },

            deleteApplication: function (record) {
                deleteApplications(record.ApplicationId, this.forceReload, this);
                return false;
            },
            deleteSelected: function () {
                deleteApplications(this.getSelectedApplicationsId(), this.forceReload, this);
                return false;
            },
            exportSelected: function () {
                var $downloadForm = this.getDownloadForm();
                $downloadForm.attr('action', '<%= Url.Action("ExportApplications", "InstitutionApplication", new { id = 2 }) %>').find('input').first().val(encodeURIComponent(ko.toJSON(this.getSubmitModel())));
                $downloadForm.submit();
                return false;
            }

        };
        
        
        // здесь данные для инициализации модели (фильтры, и т.п.) загружаем прямо из контекста ViewModel, заданного контроллером
        // viewModelData можно спокойно грузить ajax-ом отдельно если нужно
        var viewModelData = <%= Html.CustomJson(new {Model.Filter, Model.Pager, Model.HighlightApplicationId, DefaultFilter = UncheckedApplicationsFilterViewModel.Default}) %>;
        // контейнер где будет рендерится контрол
        var $container = $('#uncheckedApplicationsContainer');
        // подключаем datePicker - нельзя вынести в js т.к. используется Url.Images(...)
        $(".datePicker", $container).datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-40:+0' });
        // инстанс
        var model = new UncheckedApplicationListViewModel();       
        // инициализируем конструкцию:
        model.init($container[0], // ссылка на родной DOM-элемент (вытаскиваем из selector'а jQuery через [0])
                   viewModelData, // данные модели как JS-объект
                   function() {  // callback-функцию которая будет вызвана после инициализации модели и загрузки изначального набора записей
                        $container.show();
                   });
            
        $(window).unload(function () {
            var href = document.activeElement.href;
            //console.log(href);
            if (href && href.indexOf("NavigateToEditPage") == -1) {
                document.cookie = 'ApplicationListPage' + '=; expires=Thu, 01-Jan-70 00:00:01 GMT;';
            }
        });
    }


</script>

<div id="uncheckedApplicationsContainer" style="display: none">
    <%--Комментарии тип ko... /ko убирать нельзя - knockoutjs их использует для разметки (чтобы можно было не прописывать scope в data-bind атрибуте)--%>
    
    <!-- ko if: hasRecords -->
    <div style="padding: 8px;padding-left: 0">
        <input type="button" value="Принять" class="button3" data-bind="click: actions.checkSelected, attr: {disabled: !isSelectedCampaignActive() }" />
        <input type="button" value="Отозвать" class="button3" data-bind="click: actions.revokeSelected, attr: {disabled: !isSelectedCampaignActive() }" />
        <input type="button" value="Удалить" class="button3" data-bind="click: actions.deleteSelected, attr: {disabled: !isSelectedCampaignActive()}" />
        <input type="button" value="" class="button3 export excel" data-bind="click: actions.exportSelected, attr: {disabled: isRecords()}" />

<%--        <input type="button" value="Проверить" class="button3" data-bind="click: actions.checkSelected, attr: {disabled: !isSelectedCampaignActive() }" />
        <input type="button" value="Принять" class="button3" data-bind="click: actions.acceptSelected, attr: {disabled: !isSelectedCampaignActive() }" />--%>
        <%--<input type="button" value="Выгрузить список" class="button4" />--%>
    </div>
    <!-- /ko -->

    <%--Фильтр отрисовывается в контексте свойства javascript-модели UncheckedApplicationListViewModel.filter--%>
    <!-- ko with: filter -->
    <% Html.RenderPartial("InstitutionApplication/UncheckedListFilter", Model.Filter); %>
    <!-- /ko -->
    
    <!-- ko if: hasRecords -->
    <table class="gvuzDataGrid tableStatement2" cellpadding="3">
        <colgroup>
            <col style="width: 32px"  /> <%-- чекбокс --%>
            <col style="width: 3% "/> <%-- номер заявления --%>
            <col style="width: 25%" /> <%-- ошибки  --%>
            <col style="width: 10%"/> <%--  статус --%>
            <col style="width: 5%"/> <%-- дата проверки --%>
            <col style="width: 24%" /> <%-- названия КГ --%>
            <col style="width: 15%"/> <%-- ФИО --%>
            <col style="width: 5%"/> <%-- паспорт  --%>
            <col style="width: 1%"/> <%-- дата регистрации --%>
            <col style="width: 1%" /> <%-- рекомендован к заяислению --%>
            <col /> <%--  действия --%>
        </colgroup>
        <thead>
            <tr>
                <th style="text-align: center">
                    <input type="checkbox" data-bind="checked: selectAllRecords"/>
                </th>
                <th>
                    <a class="sortable" data-sort="ApplicationNumber"><%= Html.LabelTextFor(x => x.RecordInfo.ApplicationNumber) %></a>
                </th>
                <th>
                    <a class="sortable" data-sort="ViolationErrors"><%= Html.LabelTextFor(x => x.RecordInfo.ViolationErrors) %></a>
                </th>
                <th>
                    <a class="sortable" data-sort="StatusName"><%= Html.LabelTextFor(x => x.RecordInfo.StatusName) %></a>
                </th>
                <th>
                    <a class="sortable" data-sort="LastCheckDate"><%= Html.LabelTextFor(x => x.RecordInfo.LastCheckDate) %></a>
                </th>
                <th>
                    <span><%= Html.LabelTextFor(x => x.RecordInfo.CompetitiveGroupNames) %></span>
                </th>
                <th>
                    <a class="sortable" data-sort="EntrantName"><%= Html.LabelTextFor(x => x.RecordInfo.EntrantFullName) %></a>
                </th>
                <th>
                    <a class="sortable" data-sort="IdentityDocument"><%= Html.LabelTextFor(x => x.RecordInfo.IdentityDocument) %></a>
                </th>
                <th>
                    <a class="sortable" data-sort="RegistrationDate"><%= Html.LabelTextFor(x => x.RecordInfo.RegistrationDate) %></a>
                </th>
                <th>
                    <a class="sortable" data-sort="IsRecommended"><%= Html.LabelTextFor(x => x.RecordInfo.IsInRecommendedLists) %></a>
                </th>
                <th style="text-align: center"><span>Действия</span></th>
            </tr>
        </thead>  
        <tbody data-bind="foreach: records">
            <tr data-bind="css: {trline2: ($index() % 2 == 0), trline1: ($index() % 2 != 0)}, style: {backgroundColor: $root.isHighlighted($data) ? '#ffffe0': ''}">
                <td><input type="checkbox" data-bind="checkedValue: $data, checked: $parent.selectedRecords"/></td>
                <td><a href="javascript:void(0);" data-bind="text: ApplicationNumber, modelActionsClick: $root.actions.view" /></td>
                <td><span data-bind="text: ViolationErrors"></span></td>
                <td><span data-bind="text: StatusName"></span></td>
                <td><span data-bind="text: LastCheckDate"></span></td>
                <td><span data-bind="text: CompetitiveGroupNames"></span></td>
                <td><span data-bind="text: EntrantFullName"></span></td>
                <td><span data-bind="text: IdentityDocument"></span></td>
                <td><span data-bind="text: RegistrationDate"></span></td>
                <td><span data-bind="text: ($data.IsInRecommendedLists ? 'да' : 'нет')"></span></td>
                <td style="text-align: center;white-space: nowrap">
                    <a class="btnEdit" title="Редактировать" data-bind="css: {disabled: IsCampaignFinished}, attr: {href: $parent.actions.edit($data)}" />
                    <%--<a class="btnCheck" href="javascript:void(0)" title="Проверить" data-bind="css: {disabled: IsCampaignFinished}, modelActionsClick: $root.actions.check" />--%>
                    <%--<a class="btnOk" href="javascript:void(0)" title="Принять" data-bind="css: {disabled: IsCampaignFinished}, modelActionsClick: $root.actions.accept" />--%>
                    <a class="btnOk" href="javascript:void(0)" title="Принять" data-bind="css: {disabled: IsCampaignFinished}, modelActionsClick: $root.actions.check" />
                    <a class="btnRevoke" href="javascript:void(0)" title="Отозвать" data-bind="css: {disabled: IsCampaignFinished}, modelActionsClick: $root.actions.revoke" />
                    <a class="btnDelete" href="javascript:void(0)" title="Удалить" data-bind="css: {disabled: false}, modelActionsClick: $root.actions.deleteApplication" />
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
    <iframe id="fileDownload" style="display: none;width: 0;height: 0"></iframe>
</div>
<% Html.RenderPartial("InstitutionApplication/Dialogs/AcceptApplications", AcceptApplicationsListViewModel.MetadataInstance); %>
<% Html.RenderPartial("InstitutionApplication/Dialogs/RevokeApplications", RevokeApplicationsListViewModel.MetadataInstance); %>
<% Html.RenderPartial("InstitutionApplication/Dialogs/ViewApplication"); %>
<% Html.RenderPartial("InstitutionApplication/Dialogs/EditApplication"); %>
<% Html.RenderPartial("InstitutionApplication/Dialogs/DeleteApplications"); %>