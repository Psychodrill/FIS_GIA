<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.ApplicationsList.RevokedApplicationsListViewModel>" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.ViewModels.ApplicationsList" %>

<script type="text/javascript">
    function revokedListLoaded() {

        function RevokedApplicationsFilterViewModel(data, defaults) {
            this.defaultValuesMap = { 'ignore': ['CompetitiveGroups', 'Benefits', 'RecommendedListsOptions', 'Campaigns', 'EducationFormTypes', 'EducationSourceTypes', 'OriginalDocumentsOptions', 'IncludeInRecommendedLists'] };
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
       
        RevokedApplicationsFilterViewModel.prototype = DataBoundGrid.FilterViewModelBase.prototype;

        function RevokedApplicationListViewModel() {
            DataBoundGrid.ListViewModelBase.call(this);
            this.selectedApplicationsId = ko.pureComputed(this.getSelectedApplicationsId, this);
            this.isSelectedCampaignActive = ko.pureComputed(this.getSelectedCampaignActive, this);
            this.TotalApplicationsCount = ko.observable(0);
            this.filterStats = ko.pureComputed(this.getFilterStats, this);
            this.isSelectedRecords = ko.pureComputed(this.getSelectedRecords, this);
            this.isRecords = ko.pureComputed(this.getRecords, this);
        }

        RevokedApplicationListViewModel.prototype = WebUtils.extend({}, DataBoundGrid.ListViewModelBase.prototype);

        RevokedApplicationListViewModel.prototype.getFilterStats = function () {
            var filteredTotal = this.pager.TotalRecords();
            var unfilteredTotal = this.TotalApplicationsCount();
            if (unfilteredTotal == filteredTotal) {
                return unfilteredTotal.toString();
            } else {
                return filteredTotal.toString() + ' из ' + unfilteredTotal.toString();
            }
        };
        
        RevokedApplicationListViewModel.prototype.isHighlighted = function (record) {
            return (this.HighlightApplicationId && Number(this.HighlightApplicationId) === Number(record.ApplicationId));
        };
        
        RevokedApplicationListViewModel.prototype.getSelectedApplicationsId = function () {
            var id = [];
            ko.utils.arrayMap(this.selectedRecords(), function (selectedRecord) {
                id.push(selectedRecord.ApplicationId);
            });

            return id;
        };

        RevokedApplicationListViewModel.prototype.getSelectedCampaignActive = function () {
            return this.hasSelectedRecords() && this.selectedRecords().every(function (item) {
                return item.IsCampaignFinished !== true;
            });
        };

        RevokedApplicationListViewModel.prototype.getSelectedRecords = function () {
            return this.hasSelectedRecords();
        };
        
        RevokedApplicationListViewModel.prototype.getRecords = function () {
            return this.TotalApplicationsCount > 0;
        };

        RevokedApplicationListViewModel.prototype.init = function(domElement, modelData, callback) {
            this.filter = new RevokedApplicationsFilterViewModel(modelData.Filter, modelData.DefaultFilter);
            this.pager = new DataBoundGrid.PagerViewModel(modelData.Pager);
            //go to last page
            if (+getCookie('ApplicationListPage')) {
                this.pager.CurrentPage(+getCookie('ApplicationListPage'));   
            }
            this.sort({ SortKey: 'RegistrationDate', SortDescending: true });
            this.HighlightApplicationId = Number(modelData.HighlightApplicationId);
            DataBoundGrid.ListViewModelBase.prototype.init.call(this, domElement, callback);
        };

        RevokedApplicationListViewModel.prototype.reload = function(callback) {
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
            doPostAjax('<%= Url.Action("LoadApplicationRevokedRecords", "InstitutionApplication") %>', ko.toJSON(queryModel), function(result) {
                me.records(result.Records);
                me.pager.update(result.Pager);
                DataBoundGrid.ListViewModelBase.prototype.reload.call(me, callback);
                me.TotalApplicationsCount(result.TotalApplicationsCount);
            }, null, null, true);
        };

        RevokedApplicationListViewModel.prototype.getDownloadForm = function () {
            var $frame = $('#fileDownload').contents().find('body');
            var $form = $frame.find('#downloadForm').first();
            if ($form.length == 0) {
                $form = $('<form />').attr('method', 'POST').attr('id', 'downloadForm').append($('<input />').attr('type', 'hidden').attr('name', 'submitModel')).appendTo($frame);
            }
            return $form;
        };

        RevokedApplicationListViewModel.prototype.getSubmitModel = function() {
            return {
                Filter: this.filter.appliedValues() || this.filter.defaultValues,
                Pager: ko.mapping.toJS(this.pager, this.pager.mapping),
                Sort: ko.mapping.toJS(this.sort)
            };
        };


        RevokedApplicationListViewModel.prototype.actions = {
            view: function (record) {
                viewApplicationDetails(record.ApplicationId, record.ApplicationNumber, record.IsCampaignFinished, record.StatusID);
                return false;
            },
            edit: function (record) {
                if (record.IsCampaignFinished) {      return 'javascript:void(0)';}
                return editApplicationUrl(record.ApplicationId);
            },
            cancelRevoke: function (record) {
                if (record.IsCampaignFinished) {      return 'javascript:void(0)';}
                cancelRevokeApplications(record.ApplicationId, this.forceReload, this);
                return false;
            },
            cancelRevokeSelected: function () {
                cancelRevokeApplications(this.selectedApplicationsId(), this.forceReload, this);
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
                $downloadForm.attr('action', '<%= Url.Action("ExportApplications", "InstitutionApplication", new { id = 3 }) %>').find('input').first().val(encodeURIComponent(ko.toJSON(this.getSubmitModel())));
                $downloadForm.submit();
                return false;
            }

        };

        var viewModelData = <%= Html.CustomJson(new {Model.Filter, Model.Pager, Model.HighlightApplicationId, DefaultFilter = RevokedApplicationsFilterViewModel.Default }) %>;
               
        var $container = $('#revokedApplicationsContainer');
        $(".datePicker", $container).datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-40:+0' });
        
        var model = new RevokedApplicationListViewModel();
        model.init($container[0], viewModelData, function() { $container.show(); });

        $(window).unload(function () {
            var href = document.activeElement.href;
            //console.log(href);
            if (href && href.indexOf("NavigateToEditPage") == -1) {
                document.cookie = 'ApplicationListPage' + '=; expires=Thu, 01-Jan-70 00:00:01 GMT;';
            }
        });
    }
</script>

<div id="revokedApplicationsContainer" style="display: none">
    
    <!-- ko if: hasRecords -->
    <div style="padding: 8px;padding-left: 0">
        <input type="button" value="Отменить отзыв" class="button3" data-bind="click: actions.cancelRevokeSelected, attr: {disabled: !isSelectedCampaignActive()}" />
        <%--<input type="button" value="Выгрузить список" class="button4" />--%>
        <input type="button" value="Удалить" class="button3" data-bind="click: actions.deleteSelected, attr: {disabled: !isSelectedCampaignActive()}" />
        <input type="button" value="" class="button3 export excel" data-bind="click: actions.exportSelected, attr: {disabled: isRecords()}" />
    </div>
    <!-- /ko -->

    <!-- ko with: filter -->
    <% Html.RenderPartial("InstitutionApplication/RevokedListFilter", Model.Filter); %>
    <!-- /ko -->
    
    <!-- ko if: hasRecords -->
    <table class="gvuzDataGrid tableStatement2" cellpadding="3">
        <colgroup>
            <col style="width: 32px"  /> <%-- Чекбокс --%>
            <col style="width: 25%"/> <%-- № заявления --%>
            <col style="width: 5%" /> <%-- Дата отзыва --%>
            <col style="width: 34%"/> <%-- ФИО --%>
            <col style="width: 20%"/> <%-- паспорт --%>
            <col style="width: 5%" /> <%-- дата регистрации --%>
            <col style="width: 1%" /> <%-- рекомендован к зачислению --%>
            <col /> <%-- Действия --%>
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
                    <a class="sortable" data-sort="LastDenyDate"><%= Html.LabelTextFor(x => x.RecordInfo.LastDenyDate) %></a>
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
                <td><span data-bind="text: LastDenyDate"></span></td>
                <td><span data-bind="text: EntrantFullName"></span></td>
                <td><span data-bind="text: IdentityDocument"></span></td>
                <td><span data-bind="text: RegistrationDate"></span></td>
                <td><span data-bind="text: ($data.IsInRecommendedLists ? 'да' : 'нет')"></span></td>
                <td style="white-space: nowrap">
                    <%--movaxcs(1705)--%>
                    <a class="btnEdit" title="Редактировать" data-bind="css: {disabled: IsCampaignFinished}, attr: {href: $parent.actions.edit($data)}" />
                    <a class="btnRevoke" style="cursor: pointer" title="Отменить отзыв" data-bind="css: {disabled: IsCampaignFinished}, modelActionsClick: $root.actions.cancelRevoke" />
                    <a class="btnDelete" style="cursor: pointer" title="Удалить" data-bind="css: {disabled: false}, modelActionsClick: $root.actions.deleteApplication" />
                </td>
            </tr>
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
    <iframe id="fileDownload" style="display: none;width: 0;height: 0"></iframe>
</div>
<% Html.RenderPartial("InstitutionApplication/Dialogs/CancelRevokeApplications"); %>
<% Html.RenderPartial("InstitutionApplication/Dialogs/ViewApplication"); %>
<% Html.RenderPartial("InstitutionApplication/Dialogs/EditApplication"); %>
<% Html.RenderPartial("InstitutionApplication/Dialogs/DeleteApplications"); %>