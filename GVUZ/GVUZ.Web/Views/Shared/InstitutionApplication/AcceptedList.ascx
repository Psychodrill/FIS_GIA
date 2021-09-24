<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.ApplicationsList.AcceptedApplicationsListViewModel>" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.ViewModels.ApplicationsList" %>
<script type="text/javascript">

    function acceptedListLoaded() {
        function AcceptedApplicationsFilterViewModel(data, defaults) { 
            this.defaultValuesMap = { 'ignore': ['CompetitiveGroups', 'Benefits', 'RecommendedListsOptions', 'Campaigns', 'EducationFormTypes', 'OriginalDocumentsOptions', 'IncludeInRecommendedLists', 'CampaignYears'] };
            this.defaultValues = ko.mapping.toJS(defaults, this.defaultValuesMap);
            
            //this.defaultValuesMap= null;
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
                'EducationFormTypes': {
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
                'EducationSourceTypes': {
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
            
        AcceptedApplicationsFilterViewModel.prototype = DataBoundGrid.FilterViewModelBase.prototype;

        function AcceptedApplicationListViewModel() {
            DataBoundGrid.ListViewModelBase.call(this);
            this.selectedApplicationsId = ko.pureComputed(this.getSelectedApplicationsId, this);
            this.isSelectedCampaignActive = ko.pureComputed(this.getSelectedCampaignActive, this);
            this.TotalApplicationsCount = ko.observable(0);
            this.filterStats = ko.pureComputed(this.getFilterStats, this);
            this.isSelectedRecords = ko.pureComputed(this.getSelectedRecords, this);
            this.isRecords = ko.pureComputed(this.getRecords, this);
        }

        AcceptedApplicationListViewModel.prototype = WebUtils.extend({}, DataBoundGrid.ListViewModelBase.prototype);

        AcceptedApplicationListViewModel.prototype.getFilterStats = function() {
            var filteredTotal = this.pager.TotalRecords();
            var unfilteredTotal = this.TotalApplicationsCount();
            if (unfilteredTotal == filteredTotal) {
                return unfilteredTotal.toString();
            } else {
                return filteredTotal.toString() + ' из ' + unfilteredTotal.toString();
            }
        };
        
        AcceptedApplicationListViewModel.prototype.isHighlighted = function (record) {
            return (this.HighlightApplicationId && Number(this.HighlightApplicationId) === Number(record.ApplicationId));
        };
        
        AcceptedApplicationListViewModel.prototype.getSelectedApplicationsId = function () {
            var id = [];
            ko.utils.arrayMap(this.selectedRecords(), function (selectedRecord) {
                id.push(selectedRecord.ApplicationId);
            });

            return id;
        };

        AcceptedApplicationListViewModel.prototype.getSelectedCampaignActive = function () {
            return this.hasSelectedRecords() && this.selectedRecords().every(function (item) {
                return item.IsCampaignFinished !== true;
            });
        };

        AcceptedApplicationListViewModel.prototype.getSelectedRecords = function () {
            return this.hasSelectedRecords();
        };

        AcceptedApplicationListViewModel.prototype.getRecords = function () {
            return this.TotalApplicationsCount > 0;
        };

        AcceptedApplicationListViewModel.prototype.init = function(domElement, modelData, callback) {
            this.filter = new AcceptedApplicationsFilterViewModel(modelData.Filter, modelData.DefaultFilter);
            this.pager = new DataBoundGrid.PagerViewModel(modelData.Pager);
            //go to last page
            if (+getCookie('ApplicationListPage')) {
                this.pager.CurrentPage(+getCookie('ApplicationListPage'));   
            }
            this.sort({ SortKey: 'RegistrationDate', SortDescending: true });
            this.HighlightApplicationId = Number(modelData.HighlightApplicationId);
            DataBoundGrid.ListViewModelBase.prototype.init.call(this, domElement, callback);
        };

        AcceptedApplicationListViewModel.prototype.reload = function(callback) {
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

            doPostAjax('<%= Url.Action("LoadApplicationAcceptedRecords", "InstitutionApplication") %>', ko.toJSON(queryModel), function(result) {
                me.records(result.Records);
                me.pager.update(result.Pager);
                DataBoundGrid.ListViewModelBase.prototype.reload.call(me, callback);
                me.TotalApplicationsCount(result.TotalApplicationsCount);
            }, null, null, true);
        };
        
        AcceptedApplicationListViewModel.prototype.getDownloadForm = function () {
            var $frame = $('#fileDownload').contents().find('body');
            var $form = $frame.find('#downloadForm').first();
            if ($form.length == 0) {
                $form = $('<form />').attr('method', 'POST').attr('id', 'downloadForm').append($('<input />').attr('type', 'hidden').attr('name', 'submitModel')).appendTo($frame);
            }
            return $form;
        };

        AcceptedApplicationListViewModel.prototype.getSubmitModel = function() {
            return {
                Filter: this.filter.appliedValues() || this.filter.defaultValues,
                Pager: ko.mapping.toJS(this.pager, this.pager.mapping),
                Sort: ko.mapping.toJS(this.sort)
            };
        };


        AcceptedApplicationListViewModel.prototype.actions = {
            view: function (record) { 
                viewApplicationDetails(record.ApplicationId, record.ApplicationNumber, record.IsCampaignFinished, record.StatusID);
                return false;
            },
            edit: function (record) {
                if (record.IsCampaignFinished) {  return 'javascript:void(0)'; }
                return editApplicationUrl(record.ApplicationId);
            },
            check: function (record) {
                if (record.IsCampaignFinished) {  return 'javascript:void(0)'; }
                checkApplicationsDialog(record.ApplicationId, this.forceReload, this);
                return false;
            },
            checkSelected: function () {
                checkApplicationsDialog(this.getSelectedApplicationsId(), this.forceReload, this);
                return false;
            },
            recommend: function (record) {
                if (record.IsCampaignFinished) {  return 'javascript:void(0)'; }
                if (record.CanIncludeInRecommended === true) {
                    includeRecommendedDialog(record.ApplicationId, this.forceReload, this);    
                }                
                return false;
            },
            include: function(record) {
                if (record.IsCampaignFinished) { return 'javascript:void(0)'; }
                var id = [];
                id.push(record.ApplicationId);
                appsSelectOrder(id);
                return false;
            },
            revoke: function (record) {
                if (record.IsCampaignFinished) {  return 'javascript:void(0)'; }
                revokeApplicationsDialog(record.ApplicationId, this.forceReload, this);
                return false;
            },
            revokeSelected: function () {
                revokeApplicationsDialog(this.getSelectedApplicationsId(), this.forceReload, this);
                return false;
            },
            applicationSelectOrder: function() {
                appsSelectOrder(this.getSelectedApplicationsId());
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
                $downloadForm.attr('action', '<%= Url.Action("ExportApplications", "InstitutionApplication", new { id = 4 }) %>').find('input').first().val(encodeURIComponent(ko.toJSON(this.getSubmitModel())));
                $downloadForm.submit();
                return false;
            }
        };
        
        var viewModelData =<%= Html.CustomJson(new {Model.Filter, Model.Pager, Model.HighlightApplicationId, DefaultFilter = AcceptedApplicationsFilterViewModel.Default}) %>;
        var $container = $('#acceptedApplicationsContainer');
        $(".datePicker", $container).datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-40:+0' });
        
        var model = new AcceptedApplicationListViewModel();
        model.init($container[0], viewModelData, function() { $container.show(); });
    }

    $(window).unload(function () {
        var href = document.activeElement.href;
        console.log(href);
        if (href && href.indexOf("NavigateToEditPage") == -1) {
            document.cookie = 'ApplicationListPage' + '=; expires=Thu, 01-Jan-70 00:00:01 GMT;';
        }
    });

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
<script>
    function GenerateXmlList() {
        var model = {
            Checked: 324,
            ApplicationID: 234234234,
            ApplicationNumber: 123213,
            EntrantFIO: 1111
        }
        doPostAjax('<%= Url.Action("NewGenerateXmlList", "InstitutionApplication") %>', ko.toJSON(model), function (data) {
            alert(data.Data)
        });
    }
</script>
<div id="acceptedApplicationsContainer" style="display: none">
    <!-- ko if: hasRecords -->
    <div style="padding: 8px; padding-left: 0">
        <% if (!ConfigHelper.HideOrderOfAdmissionMenu())
           { %>
        <input type="button" value="Включить в приказ" class="button4" data-bind="click: actions.applicationSelectOrder, attr: {disabled: !isSelectedCampaignActive()}" />
        <%} %>
        <input type="button" value="Проверить" class="button3" data-bind="click: actions.checkSelected, attr: {disabled: !isSelectedCampaignActive()}" />
        <input type="button" value="Отозвать" class="button3" data-bind="click: actions.revokeSelected, attr: {disabled: !isSelectedCampaignActive()}" />
        <%--<input type="button" value="Выгрузить список" class="button4" onclick="GenerateXmlList()" />--%>
        <input type="button" value="Удалить" class="button3" data-bind="click: actions.deleteSelected, attr: {disabled: !isSelectedCampaignActive()}" />
        <input type="button" value="" class="button3 export excel" data-bind="click: actions.exportSelected, attr: {disabled: isRecords()}" />
    </div>
    <!-- /ko -->
    <!-- ko with: filter -->
    <% Html.RenderPartial("InstitutionApplication/AcceptedListFilter", Model.Filter); %>
    <!-- /ko -->
    <!-- ko if: hasRecords -->
    <table class="gvuzDataGrid tableStatement2" cellpadding="3">
        <colgroup>
            <col style="width: 16px" />
            <%-- чекбокс --%>
            <col style="width: 8%" />
            <%-- № заявления --%>
            <col style="width: 8%" />
            <%-- Статус  --%>
            <col style="width: 5%" />
            <%-- дата проверки --%>
            <col style="width: 35%" />
            <%-- название КГ --%>
            <col style="width: 17%" />
            <%-- ФИО --%>
            <col style="width: 5%" />
            <%-- паспорт --%>
            <col style="width: 5%" />
            <%-- дата регистрации --%>
            <col style="width: 1%" />
            <%-- Оригиналы получены --%>
            <%-- <col style="width: 5%" /> <%-- Рейтинг --%>
            <col style="width: 1%" />
            <%-- Рекомендован к зачислению --%>
            <col />
            <%-- Рейтинг --%>
            <col />
            <%-- Действия --%>
        </colgroup>
        <thead>
            <tr>
                <th style="text-align: center">
                    <input type="checkbox" data-bind="checked: selectAllRecords" />
                </th>
                <th>
                    <a class="sortable" data-sort="ApplicationNumber">
                        <%= Html.LabelTextFor(x => x.RecordInfo.ApplicationNumber) %></a>
                </th>
                <th>
                    <a class="sortable" data-sort="StatusName">
                        <%= Html.LabelTextFor(x => x.RecordInfo.StatusName) %></a>
                </th>
                <th>
                    <a class="sortable" data-sort="LastCheckDate">
                        <%= Html.LabelTextFor(x => x.RecordInfo.LastCheckDate) %></a>
                </th>
                <th>
                    <span>
                        <%= Html.LabelTextFor(x => x.RecordInfo.CompetitiveGroupNames) %></span>
                </th>
                <th>
                    <a class="sortable" data-sort="EntrantName">
                        <%= Html.LabelTextFor(x => x.RecordInfo.EntrantFullName) %></a>
                </th>
                <th>
                    <a class="sortable" data-sort="IdentityDocument">
                        <%= Html.LabelTextFor(x => x.RecordInfo.IdentityDocument) %></a>
                </th>
                <th>
                    <a class="sortable" data-sort="RegistrationDate">
                        <%= Html.LabelTextFor(x => x.RecordInfo.RegistrationDate) %></a>
                </th>
                <th>
                    <a class="sortable" data-sort="OriginalDocumentsReceived">
                        <%= Html.LabelTextFor(x => x.RecordInfo.OriginalDocumentsReceived) %></a>
                </th>
                <%--<th style="text-align: center">
                    <a data-sort="Rating"><%= Html.LabelTextFor(x => x.RecordInfo.Rating) %></a>
                </th>--%>
                <th>
                    <a class="sortable" data-sort="IsRecommended">
                        <%= Html.LabelTextFor(x => x.RecordInfo.IsInRecommendedLists) %></a>
                </th>
                <th>
                    <a class="sortable" data-sort="CalculatedRating">
                        <%= Html.LabelTextFor(x => x.RecordInfo.CalculatedRating) %></a>
                </th>
                <th style="text-align: center">
                    <span>Действия</span>
                </th>
            </tr>
        </thead>
        <tbody data-bind="foreach: records">
            <tr data-bind="css: {trline2: ($index() % 2 == 0), trline1: ($index() % 2 != 0)}, style: {backgroundColor: $root.isHighlighted($data) ? '#ffffe0': ''}">
                <td>
                    <input type="checkbox" data-bind="checkedValue: $data, checked: $parent.selectedRecords" />
                </td>
                <td>
                    <a href="javascript:void(0);" data-bind="text: ApplicationNumber, modelActionsClick: $root.actions.view" />
                </td>
                <td>
                    <span data-bind="text: StatusName"></span>
                </td>
                <td>
                    <span data-bind="text: LastCheckDate"></span>
                </td>
                <td>
                    <span data-bind="text: CompetitiveGroupNames"></span>
                </td>
                <td>
                    <span data-bind="text: EntrantFullName"></span>
                </td>
                <td>
                    <span data-bind="text: IdentityDocument"></span>
                </td>
                <td>
                    <span data-bind="text: RegistrationDate"></span>
                </td>
                <td>
                    <span data-bind="text: ($data.OriginalDocumentsReceived ? 'да' : 'нет')"></span>
                </td>
                <%--<td style="text-align: center"><span data-bind="text: Rating"></span></td>--%>
                <td>
                    <span data-bind="text: ($data.IsInRecommendedLists ? 'да' : 'нет')"></span>
                </td>
                <td>
                    <span data-bind="text: CalculatedRating"></span>
                </td>
                <td style="text-align: center; white-space: nowrap">
                    <a class="btnEdit" title="Редактировать заявление" data-bind="css: {disabled: IsCampaignFinished, btnEditGray: IsCampaignFinished}, attr: {href: $parent.actions.edit($data)}" />
                    <a class="btnCheck" href="javascript:void(0)" title="Проверить" data-bind="css: {disabled: IsCampaignFinished, btnCheckGray: IsCampaignFinished}, modelActionsClick: $root.actions.check" />
                    
                    <%--<a href="javascript:void(0)" title="Включить в список рекомендованных к зачислению"
                        data-bind="css: {disabled: (IsCampaignFinished || !CanIncludeInRecommended), btnRecListGray: (IsCampaignFinished || !CanIncludeInRecommended), btnRecList: (!IsCampaignFinished && CanIncludeInRecommended)}, modelActionsClick: $root.actions.recommend" />--%>

                    <% if (!ConfigHelper.HideOrderOfAdmissionMenu())
                       { %>
                    <a class="btnOk" title="Включить в приказ" data-bind="css: {disabled: IsCampaignFinished, btnOkGray: IsCampaignFinished }, modelActionsClick: $root.actions.include" />
                    <% } %>
                    <a class="btnRevoke" href="javascript:void(0)" title="Отозвать" data-bind="css: {disabled: IsCampaignFinished, btnRevokeGray: IsCampaignFinished}, modelActionsClick: $root.actions.revoke" />
                    <a class="btnDelete" href="javascript:void(0)" title="Удалить"  data-bind="css: {disabled: false}, modelActionsClick: $root.actions.deleteApplication" />

                </td>
            </tr>
        </tbody>
        <tfoot data-bind="with: pager">
            <tr>
                <th colspan="12">
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
<% Html.RenderPartial("InstitutionApplication/Dialogs/RevokeApplications", RevokeApplicationsListViewModel.MetadataInstance); %>
<% Html.RenderPartial("InstitutionApplication/Dialogs/IncludeRecommended", IncludeRecommendedListViewModel.MetadataInstance); %>
<% Html.RenderPartial("InstitutionApplication/Dialogs/ViewApplication"); %>
<% Html.RenderPartial("InstitutionApplication/Dialogs/EditApplication"); %>
<% Html.RenderPartial("InstitutionApplication/Dialogs/DeleteApplications"); %>