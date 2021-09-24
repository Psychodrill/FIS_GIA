<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.OrderOfAdmission.ApplicationOrderListViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.ViewModels.OrderOfAdmission" %>

<%--<script type="text/javascript">
    function orderOfAdmissionApplicationListLoaded() {
        
        function ApplicationOrderFilterViewModel(data, defaults) {
            this.defaultValuesMap = { 'ignore': ['EducationLevels', 'EducationForms', 'EducationSources', 'Benefits', 'CompetitiveGroups'] };
            this.defaultValues = ko.mapping.toJS(defaults, this.defaultValuesMap);
            DataBoundGrid.FilterViewModelBase.call(this, data);
        }

        ApplicationOrderFilterViewModel.prototype = DataBoundGrid.FilterViewModelBase.prototype;

        function ApplicationOrderListViewModel() {
            DataBoundGrid.ListViewModelBase.call(this);
            this.selectedApplicationItemId = ko.pureComputed(this.getSelectedApplicationItemId, this);
            this.isSelectedExcludeAllowed = ko.pureComputed(this.getIsSelectedExcludeAllowed, this);
            this.isRefuseAllowed = ko.pureComputed(this.getIsRefuseAllowed, this);
            this.TotalApplicationOrderCount = ko.observable(0);
            this.filterStats = ko.pureComputed(this.getFilterStats, this); 
        }

        ApplicationOrderListViewModel.prototype = WebUtils.extend({}, DataBoundGrid.ListViewModelBase.prototype);

        ApplicationOrderListViewModel.prototype.getSelectedApplicationItemId = function () {
            var id = [];
            ko.utils.arrayMap(this.selectedRecords(), function (selectedRecord) {
                id.push(selectedRecord.ApplicationItemId); 
            });

            return id;
        };
        
        ApplicationOrderListViewModel.prototype.getIsSelectedExcludeAllowed = function () {
            return this.hasSelectedRecords() && this.selectedRecords().every(function (item) {
                return item.AllowExcludeAction === true;
            });
        };
        
        ApplicationOrderListViewModel.prototype.getIsRefuseAllowed = function () {
            return this.hasSelectedRecords() && this.selectedRecords().every(function (item) {
                return item.AllowRefuseAdmissionAction === true;
            });
        };
        
        ApplicationOrderListViewModel.prototype.getFilterStats = function() {
            var filteredTotal = this.pager.TotalRecords();
            var unfilteredTotal = this.TotalApplicationOrderCount();
            if (unfilteredTotal == filteredTotal) {
                return unfilteredTotal.toString();
            } else {
                return filteredTotal.toString() + ' из ' + unfilteredTotal.toString();
            }
        };

        ApplicationOrderListViewModel.prototype.init = function (domElement, modelData, callback) {
            this.filter = new ApplicationOrderFilterViewModel(modelData.Filter, modelData.DefaultFilter);
            this.pager = new DataBoundGrid.PagerViewModel(modelData.Pager);
            this.sort({ SortKey: 'ApplicationOrderId', SortDescending: false });
            DataBoundGrid.ListViewModelBase.prototype.init.call(this, domElement, callback);
        };

        ApplicationOrderListViewModel.prototype.getSubmitModel = function() {
            return {
                Filter: this.filter.appliedValues() || this.filter.defaultValues,
                Pager: ko.mapping.toJS(this.pager, this.pager.mapping),
                Sort: ko.mapping.toJS(this.sort)
            }; 
        };

        ApplicationOrderListViewModel.prototype.reload = function (callback) {
            var me = this;
            var queryModel = this.getSubmitModel();
            
            doPostAjax('<%= Url.Action("LoadApplicationOrderRecords", "OrderOfAdmission", new {id = Model.OrderId }) %>', ko.toJSON(queryModel), function (result) {
                me.records(result.Records);
                me.pager.update(result.Pager);
                DataBoundGrid.ListViewModelBase.prototype.reload.call(me, callback);
                me.TotalApplicationOrderCount(result.TotalApplicationOrderCount); 

                $(".datePicker", $container).datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-40:+0' });
            }, null, null, true);
        };
        
        ApplicationOrderListViewModel.prototype.getDownloadForm = function () {
            var $frame = $('#fileDownload').contents().find('body');
            var $form = $frame.find('#downloadForm').first();
            if ($form.length == 0) {
                $form = $('<form />').attr('method', 'POST').attr('id', 'downloadForm').append($('<input />').attr('type', 'hidden').attr('name', 'submitModel')).appendTo($frame);
            }
            return $form;
        };

        ApplicationOrderListViewModel.prototype.onApplicationsExcluded = function(res) {
            if (res.reloadOrder) {
                    window.location.href = '<%= Url.Action("EditOrder", "OrderOfAdmission", new { id = Model.OrderId}) %>';
            }
            else if (res.reloadList) {
                this.forceReload();
            }
        };

    ApplicationOrderListViewModel.prototype.actions = {
        excludeApplication: function(record) {
            if (record.AllowExcludeAction) {
                excludeApplicationFromOrderDialog(parseInt('<%= Model.OrderId %>'), record.ApplicationItemId, this.onApplicationsExcluded, this);
                }
                return false;
            },
            excludeSelectedApplications: function() {
                if (this.isSelectedExcludeAllowed()) {
                    excludeApplicationFromOrderDialog(parseInt('<%= Model.OrderId %>'), this.getSelectedApplicationItemId(), this.onApplicationsExcluded, this);
                }
                return false;
            },
            refuseAdmission: function(record) {                
                if (record.AllowRefuseAdmissionAction) {
                    var applicationItemIds=[];
                    applicationItemIds.push(record.ApplicationItemId);
                    RefuseAdmission(applicationItemIds);
                }
                return false;
            },
            refuseAdmissionSelected: function() {
                if (this.isRefuseAllowed()) {
                    RefuseAdmission(this.getSelectedApplicationItemId());
                }
                return false;
            },
            saveDisagreed:function(record)
            { 
                if(record.IsDisagreed && (!record.IsDisagreedDateStr || record.IsDisagreedDateStr==""))
                {
                    infoDialogC("Требуется ввести дату отказа от зачисления", { title: "Ошибка сохранения" });
                }
                else 
                {
                    var me = this;
                    var applicationDisagreeData = {
                        ApplicationItemId:record.ApplicationItemId,
                        IsDisagreed:record.IsDisagreed,
                        IsDisagreedDate:record.IsDisagreedDateStr
                    };
                    doPostAjaxSync("<%= Url.Generate<OrderOfAdmissionController>(x => x.SaveApplicationDisagreeData(null)) %>", JSON.stringify(applicationDisagreeData), function(data) {  
                        if(!data.success)
                        {
                            if(data.errorMessage)
                            {
                                infoDialogC(data.errorMessage, { title: "Ошибка сохранения" });
                            }
                            else 
                            {
                                infoDialogC("Произошла ошибка", { title: "Ошибка сохранения" });
                            }
                        }
                        else 
                        {
                            me.forceReload();
                        }
                    });                    
                }
                return false;
            }, 
            exportCsv: function() {
                var $downloadForm = this.getDownloadForm();
                $downloadForm.attr('action', '<%= Url.Action("ExportApplicationsCsv", "OrderOfAdmission", new { id = Model.OrderId }) %>').find('input').first().val(encodeURIComponent(ko.toJSON(this.getSubmitModel())));
                $downloadForm.submit();
            },
            viewApplication: function (record) {
                viewApplicationDetails(record.ApplicationId, record.ApplicationNumber, true, record.StatusID);
                return false;
            }
        };
        var viewModelData = <%= Html.CustomJson(new {Model.Filter, Model.Pager, DefaultFilter = ApplicationOrderFilterViewModel.Default}) %>;
        var $container = $('#orderOfAdmissionApplicationListContainer');
        
        var model = new ApplicationOrderListViewModel();       
        model.init($container[0], viewModelData, function() { 
            $container.show();
            $(".datePicker", $container).datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-40:+0' });
        });
    }

    function RefuseAdmission(applicationItemIds)
    { 
        var $form= $("#RefuseAdmissionForm");
        for (var i = 0; i < applicationItemIds.length; i++) {
            $form.append("<input type='hidden' name='applicationItemIds["+i+"]' value='"+applicationItemIds[i]+"' />");
        }
        $form.submit();
    }
</script>--%>

<div id="orderOfAdmissionApplicationListContainer" v-show="appList.length > 0" v-cloak>
    <div style="display:none;">
    <% using (Html.BeginForm("ApplicationAdmissionRefuse", "OrderOfAdmission", FormMethod.Post, new { id = "RefuseAdmissionForm" }))
       { %>
        <input type="hidden" name="orderOfAdmissionId" value="<%=Model.OrderId %>" />
        <%} %>
    </div>

    <div style="padding: 8px; padding-left: 0">
        <%if (!Model.IsReadOnly)
          { %>
        <%if (Model.AllowRefuseAdmission)
          { %>
        <input type="button" class="button4" value="Отказ от зачисления" :disabled="!allowRefuseButton" @click="RefuseAdmission()" />
        <% } %>
        <input type="button" class="button4" value="Удалить из приказа" @click="ExcludeSelectedApplications()" :disabled="!allowDeleteButton" />
        <% } %>
        <input type="button" class="button3 export csv" title="Экспорт в CSV" @click="ExportCsv()" />
    </div>

    <%--Параметры фильтра--%>
    <div class="tableHeader22"  style="height: 100%">
        <div v-show="!showFilter" class="hideTable"  @click="ToggleFilter()" style="float: left">
            <span id="btnShowFilter">Отобразить фильтр</span>    
        </div>
        <div class="tableHeader5l" v-show="showFilter">
            <div class="hideTable nonHideTable" style="float: left;" @click="ToggleFilter()">
                <span>Скрыть фильтр</span>
            </div>                    
                <table class="tableForm">
                    <%--<colgroup>
                        <col style="width: 10%;"/>
                        <col style="width: 20%"/>
                        <col style="width: 10%"/>
                    </colgroup>--%>
                    <tbody>
                        <tr>
                            <td style="text-align: right;">Номер заявления:</td>&nbsp;
                            <td>
                                <input type="text" style = "width: 326px" v-model="filter.ApplicationNumber" />
                            </td>

                            <td style="text-align: right;">Уровень образования:</td>&nbsp;
                            <td>
                                <select v-model="filter.EducationLevelName">
                                    <option v-bind:value="null">Не важно</option>
                                    <option v-for="option in educationLevels" v-bind:value="option.Name">
                                        {{ option.Name }}
                                    </option>
                                </select>
                            </td>

                            <td style="text-align: right;">Конкурс:</td>&nbsp;
                            <td>
                                <select v-model="filter.CompetitiveGroupName">
                                    <option v-bind:value="null">Не важно</option>
                                    <option v-for="option in competitiveGroups" v-bind:value="option.Name">
                                        {{ option.Name }}
                                    </option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">ФИО:</td>&nbsp;
                            <td>
                                <input type="text" style = "width: 326px" v-model="filter.EntrantName" />
                            </td>

                            <td style="text-align: right;">Источник финансирования:</td>&nbsp;
                            <td>
                                <select v-model="filter.EducationSourceName">
                                    <option v-bind:value="null">Не важно</option>
                                    <option v-for="option in educationSources" v-bind:value="option.Name">
                                        {{ option.Name }}
                                    </option>
                                </select>
                            </td>  

                            <td style="text-align: right;">Льгота:</td>&nbsp;
                            <td>
                                <select v-model="filter.Benefit">
                                    <option v-bind:value="null">Не важно</option>
                                    <option v-for="option in benefits" v-bind:value="option.Name">
                                        {{ option.Name }}
                                    </option>
                                </select>
                            </td>
                        </tr>

                        <tr>
                            <td style="text-align: right;">Документ, удостоверяющий личность:</td>&nbsp;
                            <td>
                                <input type="text" style = "width: 326px" v-model="filter.IdentityDocument" />
                            </td>

                            <td style="text-align: right;">Форма обучения:</td>&nbsp;
                            <td>
                                <select v-model="filter.EducationFormName">
                                    <option v-bind:value="null">Не важно</option>
                                    <option v-for="option in educationForms" v-bind:value="option.Name">
                                        {{ option.Name }}
                                    </option>
                                </select>
                            </td>

                            <td style="text-align: right;">Направление подготовки:</td>&nbsp;
                            <td>
                                <input type="text" style = "width: 326px" v-model="filter.DirectionName" />
                            </td>
                        </tr>
                        
                        <tr>
                            <td colspan="6" align="center">
                                <input type="button" class="button" value="Сбросить фильтр" style="width: auto" @click="ClearFilter()" />
                            </td>
                        </tr>
                    </tbody>
            </table>
        </div>
    </div>

    <div class="tableContainer" >
        <table class="gvuzDataGrid tableStatement2" cellpadding="3">
        <thead>
            <tr>
                <th style="text-align: center">
                      <input type="checkbox" v-model="selectedAll" @click="selectAll()" />                        
                </th>
                <%--<th><span><%= Html.LabelTextFor(x => x.RecordInfo.ApplicationOrderId) %></span></th>--%>
                <th>
                    <a @click="sortBy('ApplicationNumber')" class="sortable"><%= Html.LabelTextFor(x => x.RecordInfo.ApplicationNumber) %></a>
                    <span :class="sortSpan('ApplicationNumber')"></span></th>
                <th>
                    <a @click="sortBy('EntrantName')" class="sortable"><%= Html.LabelTextFor(x => x.RecordInfo.EntrantName) %></a>
                    <span :class="sortSpan('EntrantName')"></span>
                </th>
                <th><a @click="sortBy('IdentityDocument')" class="sortable"><%= Html.LabelTextFor(x => x.RecordInfo.IdentityDocument) %></a>
                    <span :class="sortSpan('IdentityDocument')"></span>
                </th>
                <th><a @click="sortBy('CompetitiveGroupName')" class="sortable"><%= Html.LabelTextFor(x => x.RecordInfo.CompetitiveGroupName) %></a>
                    <span :class="sortSpan('CompetitiveGroupName')"></span>
                </th>
                <th><a @click="sortBy('Benefit')" class="sortable"><%= Html.LabelTextFor(x => x.RecordInfo.Benefit) %></a>
                    <span :class="sortSpan('Benefit')"></span>
                </th>
                <th><a @click="sortBy('CompetitiveGroupTargetName')" class="sortable"><%= Html.LabelTextFor(x => x.RecordInfo.CompetitiveGroupTargetName) %></a>
                    <span :class="sortSpan('CompetitiveGroupTargetName')"></span>
                </th>
                <th><a @click="sortBy('Rating')" class="sortable"><%= Html.LabelTextFor(x => x.RecordInfo.Rating) %></a>
                    <span :class="sortSpan('Rating')"></span>
                </th>
                <th><a @click="sortBy('LevelBudgetName')" class="sortable"><%= Html.LabelTextFor(x => x.RecordInfo.LevelBudgetName) %></a>
                    <span :class="sortSpan('LevelBudgetName')"></span>
                </th>
                <th><a @click="sortBy('IsAgreed')" class="sortable"><%= Html.LabelTextFor(x => x.RecordInfo.IsAgreed) %></a>
                    <span :class="sortSpan('IsAgreed')"></span></th>
                <th><a @click="sortBy('IsAgreedDate')" class="sortable"><%= Html.LabelTextFor(x => x.RecordInfo.IsAgreedDate) %></a>
                    <span :class="sortSpan('IsAgreedDate')"></span></th></th>
                <th><a @click="sortBy('IsDisagreed')" class="sortable"><%= Html.LabelTextFor(x => x.RecordInfo.IsDisagreed) %></a>
                    <span :class="sortSpan('IsDisagreed')"></span></th></th>
                <th><a @click="sortBy('IsDisagreedDate')" class="sortable"><%= Html.LabelTextFor(x => x.RecordInfo.IsDisagreedDate) %></a>
                    <span :class="sortSpan('IsDisagreedDate')"></span></th>
                <% if (Model.ShowOrderOfAdmissionInfo)
                   { %>
                <th><a class="sortable" data-sort="OrderOfAdmissionInfo"><%= Html.LabelTextFor(x => x.RecordInfo.OrderOfAdmissionInfo) %></th>
                <% } %>
                <%if (!Model.IsReadOnly)
                  { %>
                <th style="text-align: center"><span>Действия</span></th>
                <% } %>
            </tr>
        </thead>
        <tbody data-bind="foreach: records">
            <tr v-for="(app, index) in filteredAppList" :class="[index % 2 === 0 ? 'trline1' : 'trline2']">
                <td><input type="checkbox" v-model="app.checked"/></td>
                <%--<td>{{app.ApplicationOrderId}}</td>--%>
                <td><a href="javascript:void(0);" @click="ViewApplication(app)">{{app.ApplicationNumber}}</a></td>
                <td>{{app.EntrantName}}</td>
                <td>{{app.IdentityDocument}}</td>
<%--            <td>{{app.EducationLevelName}}</td>
                <td>{{app.EducationFormName}}</td>
                <td>{{app.EducationSourceName}}</td>--%>
                <td v-tooltip.bottom="{ content: app.EducationLevelName + '<br/>' + app.EducationFormName + '<br/>' + app.EducationSourceName + '<br/>' + app.DirectionName, classes: ['tooltip'], delay: { show: 1000, hide: 400 } }">
                    {{app.CompetitiveGroupName}}</td>
                <%--<td>{{app.DirectionName}}</td>--%>
                <td>{{app.Benefit}}</td>
                <td>{{app.CompetitiveGroupTargetName}}</td>
                <td>{{app.Rating}}</td>
                <td>{{app.LevelBudgetName}}</td>
                <td>{{app.IsAgreedStr}}</td>
                <td>{{app.IsAgreedDateStr}}</td>
                <td v-if="allowEditDisagreed">
                    <input type="checkbox" v-model="app.IsDisagreed" :checked="ChangeDisagreed(app)"/>
                </td>
                <td v-if="allowEditDisagreed" style="white-space: nowrap;">
                    <datepicker :disabled="!app.IsDisagreed" v-model="app.IsDisagreedDateStr"></datepicker>
                    <a href="javascript:void(0)" 
                       style="vertical-align:middle;" 
                       class="btnSave" 
                       @click="SaveDisagreed(app)"/>
                </td>
                <td v-if="!allowEditDisagreed">{{app.IsDisagreedStr}}</td>
                <td v-if="!allowEditDisagreed">{{app.IsDisagreedDateStr}}</td>
                <% if (Model.ShowOrderOfAdmissionInfo)
                   { %>
                <td>{{app.OrderOfAdmissionInfo}}</td>
                <% } %>
                <%if (!Model.IsReadOnly)
                  { %>
                <td style="text-align: center; white-space: nowrap;">
                    <a v-if="allowRefuseAdmission && app.AllowRefuseAdmissionAction" @click="RefuseAdmission(app)" class="btnRefuseAdmission" title="Отказ от зачисления" href="javascript:void(0)"/> &nbsp;
                    <a v-if="allowRefuseAdmission && !app.AllowRefuseAdmissionAction" class="btnRefuseAdmissionGray disabled" title="Отказ от зачисления" href="javascript:void(0)"/> &nbsp;
                    <a v-if="app.AllowExcludeAction" @click="ExcludeSelectedApplications(app)" class="btnDelete" title="Удалить заявление из приказа" href="javascript:void(0)"/> &nbsp;
                    <a v-if="!app.AllowExcludeAction" class="btnDeleteGray disabled" title="Удалить заявление из приказа" href="javascript:void(0)"/> &nbsp;
                </td>
                <% } %>
            </tr>
        </tbody>
        <%--<thead>
            <tr>
                <th style="text-align: center">
                    <input type="checkbox" data-bind="checked: selectAllRecords" />
                </th>
                <th><span><%= Html.LabelTextFor(x => x.RecordInfo.ApplicationOrderId) %></span></th>
                <th><a class="sortable" data-sort="ApplicationNumber"><%= Html.LabelTextFor(x => x.RecordInfo.ApplicationNumber) %></a></th>
                <th><a class="sortable" data-sort="EntrantName"><%= Html.LabelTextFor(x => x.RecordInfo.EntrantName) %></a></th>
                <th><a class="sortable" data-sort="IdentityDocument"><%= Html.LabelTextFor(x => x.RecordInfo.IdentityDocument) %></a></th>
                <th><a class="sortable" data-sort="EducationLevelName"><%= Html.LabelTextFor(x => x.RecordInfo.EducationLevelName) %></a></th>
                <th><a class="sortable" data-sort="EducationFormName"><%= Html.LabelTextFor(x => x.RecordInfo.EducationFormName) %></a></th>
                <th><a class="sortable" data-sort="EducationSourceName"><%= Html.LabelTextFor(x => x.RecordInfo.EducationSourceName) %></a></th>
                <th><a class="sortable" data-sort="CompetitiveGroupName"><%= Html.LabelTextFor(x => x.RecordInfo.CompetitiveGroupName) %></a></th>
                <th><a class="sortable" data-sort="DirectionName"><%= Html.LabelTextFor(x => x.RecordInfo.DirectionName) %></a></th>
                <th><a class="sortable" data-sort="Benefit"><%= Html.LabelTextFor(x => x.RecordInfo.Benefit) %></a></th>
                <th><a class="sortable" data-sort="CompetitiveGroupTargetName"><%= Html.LabelTextFor(x => x.RecordInfo.CompetitiveGroupTargetName) %></a></th>
                <th><a class="sortable" data-sort="Rating"><%= Html.LabelTextFor(x => x.RecordInfo.Rating) %></a></th>
                <th><a class="sortable" data-sort="LevelBudgetName"><%= Html.LabelTextFor(x => x.RecordInfo.LevelBudgetName) %></a></th>
                <th><a class="sortable" data-sort="IsAgreed"><%= Html.LabelTextFor(x => x.RecordInfo.IsAgreed) %></th>
                <th><a class="sortable" data-sort="IsAgreedDate"><%= Html.LabelTextFor(x => x.RecordInfo.IsAgreedDate) %></th>
                <th><a class="sortable" data-sort="IsDisagreed"><%= Html.LabelTextFor(x => x.RecordInfo.IsDisagreed) %></th>
                <th><a class="sortable" data-sort="IsDisagreedDated"><%= Html.LabelTextFor(x => x.RecordInfo.IsDisagreedDate) %></th>
                <% if (Model.ShowOrderOfAdmissionInfo)
                   { %>
                <th><a class="sortable" data-sort="OrderOfAdmissionInfo"><%= Html.LabelTextFor(x => x.RecordInfo.OrderOfAdmissionInfo) %></th>
                <% } %>

                <%if (!Model.IsReadOnly)
                  { %>
                <th style="text-align: center"><span>Действия</span></th>
                <% } %>
            </tr>
        </thead>
        <tbody data-bind="foreach: records">
            <tr  v-for="app in appList" data-bind="css: {trline2: ($index() % 2 == 0), trline1: ($index() % 2 != 0)}">
                <td>
                    <input type="checkbox" data-bind="checkedValue: $data, checked: $parent.selectedRecords" /></td>
                <td><span data-bind="text: ApplicationOrderId"></span></td>
                <td><a href="javascript:void(0);" data-bind="text: ApplicationNumber, modelActionsClick: $root.actions.viewApplication"></a></td>
                <td><span data-bind="text: EntrantName"></span></td>
                <td><span data-bind="text: IdentityDocument"></span></td>
                <td><span data-bind="text: EducationLevelName"></span></td>
                <td><span data-bind="text: EducationFormName"></span></td>
                <td><span data-bind="text: EducationSourceName"></span></td>
                <td><span data-bind="text: CompetitiveGroupName"></span></td>
                <td><span data-bind="text: DirectionName"></span></td>
                <td><span data-bind="text: Benefit"></span></td>
                <td><span data-bind="text: CompetitiveGroupTargetName"></span></td>
                <td><span data-bind="text: RatingText"></span></td>
                <td><span data-bind="text: LevelBudgetName"></span></td>
                <td><span data-bind="text: IsAgreedStr"></span></td>
                <td><span data-bind="text: IsAgreedDateStr"></span></td>
                <% if (Model.AllowEditDisagreedInfo)
                   { %>
                <td style="text-align: center">
                    <input type="checkbox" data-bind="checked: IsDisagreed, attr: { id: 'isDisagreed_' + ApplicationItemId, name: 'isDisagreed_' + ApplicationItemId }" />
                </td>
                <td style="white-space: nowrap;">
                    <input class="shortInput datePicker" type="text" autocomplete="off" maxlength="10" data-bind="value: IsDisagreedDateStr, attr: { id: 'disagreedDate_' + ApplicationItemId, name: 'disagreedDate_' + ApplicationItemId }" />
                    <a href="javascript:void(0)" style="vertical-align:middle;" class="btnSave" data-bind="modelActionsClick: $root.actions.saveDisagreed"></a>
                </td>
                <% }
                   else
                   { %>
                <td><span data-bind="text: IsDisagreedStr"></span></td>
                <td><span data-bind="text: IsDisagreedDateStr"></span></td>
                <% } %>
                <% if (Model.ShowOrderOfAdmissionInfo)
                   { %>
                <td><span data-bind="text: OrderOfAdmissionInfo"></span></td>
                <% } %>

                <%if (!Model.IsReadOnly)
                  { %>
                <td style="text-align: center; white-space: nowrap">
                    <%if (Model.AllowRefuseAdmission)
                      { %>
                    <a title="Отказ от зачисления" href="javascript:void(0)" data-bind="css: {disabled: !AllowRefuseAdmissionAction, btnRefuseAdmission: AllowRefuseAdmissionAction, btnRefuseAdmissionGray: !AllowRefuseAdmissionAction}, modelActionsClick: $root.actions.refuseAdmission">&nbsp;</a>
                    <%} %>
                    <a title="Удалить заявление из приказа" href="javascript:void(0)" data-bind="css: {disabled: !AllowExcludeAction, btnDelete: AllowExcludeAction, btnDeleteGray: !AllowExcludeAction}, modelActionsClick: $root.actions.excludeApplication">&nbsp;</a>
                </td>
                <% } %>
            </tr>
        </tbody>--%>
        <tfoot>
            <tr>
                <th colspan="14">
                    <div style="text-align: center;white-space: nowrap">
                        <span class="pageLink">Записей:&nbsp;{{resultCount}}&emsp;Страниц: {{totalPages}}</span>
                        <a href="javascript:void(0)" class="pageLink pageLinkArrowLeftLeft" @click="setPage(0)">&nbsp;</a>
                        <span v-for="pageNumber in totalPages" 
                                v-if="Math.abs(pageNumber - currentPage - 1) < 3">
                            <a href="javascript:void(0)" :class="{pageLinkActive: currentPage === pageNumber - 1}" class="pageLink" @click="setPage(pageNumber-1)">{{ pageNumber }}</a>
                        </span>
                        <a href="javascript:void(0)" class="pageLink pageLinkArrowRightRight" @click="setPage(totalPages-1)">&nbsp;</a>
                        <span class="pageLink" style="display: inline;">
                            На странице:&nbsp;
                            <select style="width: 55px" v-model="itemsPerPage" @change="setPage(0)">
                                <option v-bind:value="10">10</option>
                                <option v-bind:value="30">30</option>
                                <option v-bind:value="50">50</option>
                                <option v-bind:value="100">100</option>
                                <option v-bind:value="200">200</option>
                                <option v-bind:value="500">500</option>
                            </select>
                        </span>
                    </div>
                </th>
            </tr>
        </tfoot>
    </table>
    </div>
    
    <!-- /ko -->

    <%--<p data-bind="visible: !hasRecords()">
        <em>Не обнаружено ни одной записи, удовлетворяющей условиям поиска</em>
    </p>--%>
    <iframe id="fileDownload" style="display: none; width: 0; height: 0"></iframe>
</div>

<script type="text/javascript">
        Vue.use(window['vue-easy-toast'].default)
        Vue.directive('tooltip', VTooltip.VTooltip)

        var app = new Vue({
            el: '#orderOfAdmissionApplicationListContainer',
            components: {
                'datepicker': datepicker
            },
            data: function () {
                return {
                    orderId: null,
                    appList: [],
                    selectedAll: false,
                    allowEditDisagreed: null,
                    allowRefuseAdmission: null,
                    transferList: [],
                    checkedTransferList: [],
                    //Сортировка
                    sortKey: null,
                    reverse: 'asc',
                    //Пейджинг
                    totalPages: 0,
                    currentPage: 0,
                    itemsPerPage: 10,
                    resultCount: 0,
                    //Параметры фильтра
                    showFilter: false,
                    filter: {

                    },
                    educationLevels: [],
                }
            },
            created: function () {
                this.LoadData();
            },
            methods: {
                LoadData: function () {
                    this.orderId = JSON.parse('<%= Html.Serialize(Model.OrderId) %>');
                    doPostAjax('<%= Url.Action("LoadApplicationOrderRecords", "OrderOfAdmission", new {id = Model.OrderId }) %>', '', function (res) {
                        if (res) {
                            app.appList = res.Data.Records;
                        }
                    });
                    this.allowEditDisagreed = JSON.parse('<%= Html.Serialize(Model.AllowEditDisagreedInfo) %>');
                    this.allowRefuseAdmission = JSON.parse('<%= Html.Serialize(Model.AllowRefuseAdmission) %>');

                    this.educationLevels = JSON.parse('<%= Html.Serialize(Model.Filter.EducationLevels.Items) %>');
                    this.educationForms = JSON.parse('<%= Html.Serialize(Model.Filter.EducationForms.Items) %>');
                    this.educationSources = JSON.parse('<%= Html.Serialize(Model.Filter.EducationSources.Items) %>');
                    this.competitiveGroups = JSON.parse('<%= Html.Serialize(Model.Filter.CompetitiveGroups.Items) %>');
                    this.benefits = JSON.parse('<%= Html.Serialize(Model.Filter.Benefits.Items) %>');
                },
                ExportCsv: function () {
                    var $frame = $('#fileDownload').contents().find('body');
                    var $form = $frame.find('#downloadForm').first();
                    if ($form.length == 0) {
                        $form = $('<form />').attr('method', 'POST').attr('id', 'downloadForm').append($('<input />').attr('type', 'hidden').attr('name', 'submitModel')).appendTo($frame);
                    }

                    //var $downloadForm = this.getDownloadForm();
                    $form.attr('action', '<%= Url.Action("ExportApplicationsCsv", "OrderOfAdmission", new { id = Model.OrderId }) %>').find('input').first().val();
                    $form.submit();
                },
                ViewApplication: function (record) {
                    viewApplicationDetails(record.ApplicationId, record.ApplicationNumber, true, record.StatusID);
                    return false;
                },
                ChangeDisagreed: function(record) {
                    record.IsDisagreedDateStr = record.IsDisagreed ? record.IsDisagreedDateStr : null;
                    record.IsDisagreedDate = record.IsDisagreed ? record.IsDisagreedDate : null;
                },
                SaveDisagreed: function(record) { 
                    if(record.IsDisagreed && (!record.IsDisagreedDateStr || record.IsDisagreedDateStr==""))
                    {
                        _errorToast("Требуется ввести дату отказа от зачисления! &nbsp");
                    }
                    else 
                    {
                        var applicationDisagreeData = {
                            ApplicationItemId: record.ApplicationItemId,
                            IsDisagreed: record.IsDisagreed,
                            IsDisagreedDate: record.IsDisagreedDateStr
                        };
                        doPostAjaxSync("<%= Url.Generate<OrderOfAdmissionController>(x => x.SaveApplicationDisagreeData(null)) %>", JSON.stringify(applicationDisagreeData), function(data) {  
                            if(!data.success) {
                                _errorToast("Произошла ошибка");
                            }
                            else {
                                if (!record.IsDisagreed || !record.IsDisagreedDateStr || record.IsDisagreedDateStr === '') {
                                    record.AllowRefuseAdmissionAction = false
                                }
                                else record.AllowRefuseAdmissionAction = true;
                                Vue.set(app.appList, app.appList.indexOf(record), record);
                                _successToast('Изменения сохранены (заявление: ' + record.ApplicationNumber + ')');
                            }
                        });                    
                    }
                    return false;
                },
                RefuseAdmission: function (record)
                {
                    var $form = $("#RefuseAdmissionForm");
                    if (record) { //single
                        $form.append("<input type='hidden' name='applicationItemIds[0]' value='" + record.ApplicationItemId + "' />");
                        $form.submit();
                    }
                    else {  //multiple
                        for (var i = 0; i < this.checkedAppIds.length; i++) {
                            $form.append("<input type='hidden' name='applicationItemIds[" + i + "]' value='" + this.checkedAppIds[i] + "' />");
                        }
                        $form.submit();
                    }
                },
                ExcludeSelectedApplications: function (record) {
                    if (record) {
                        excludeApplicationFromOrderDialog(this.orderId, record.ApplicationItemId, this.OnApplicationsExcluded, this);
                    }
                    else {
                        excludeApplicationFromOrderDialog(this.orderId, this.checkedAppIds, this.OnApplicationsExcluded, this);
                    }
                },
                OnApplicationsExcluded: function(res) {
                    if (res.reloadOrder) {
                        window.location.href = '<%= Url.Action("EditOrder", "OrderOfAdmission", new { id = Model.OrderId}) %>';
                    }
                    else if (res.reloadList) {
                        //this.forceReload();
                        this.LoadData();
                    }
                },
                ToggleFilter: function () {
                    this.showFilter = !this.showFilter;
                    //this.filterButtonText = this.filterButtonText === 'Скрыть фильтр' ? 'Отобразить фильтр' : 'Скрыть фильтр';
                },
                selectAll: function () {
                    app.selectedAll = !app.selectedAll;
                    for (var i = 0; i < app.filteredAppList.length; i++) {
                        var item = app.filteredAppList[i];
                        item.checked = app.selectedAll;
                    }
                    app.filteredAppList = app.filteredAppList.slice(0);
                    app.filteredAppList = app.filteredAppList.slice(0);
                },
                ClearFilter: function () {
                    if (app.selectedAll) {
                        this.selectAll();
                    }
                    $.each(app.filter, function (key, value) {
                        app.filter[key] = null;
                    });
                    sessionStorage.removeItem('competitiveGroupListFilter');
                },
                sortBy: function (sortKey) {
                    if (this.sortKey === sortKey) {
                        this.reverse = this.reverse === 'asc' ? 'desc' : 'asc';
                    }
                    else {
                        this.reverse === 'desc';
                        this.sortKey = sortKey;
                    }
                },
                sortSpan: function (value) {
                    return {
                        'sortDown': this.sortKey === value && this.reverse === 'desc',
                        'sortUp': this.sortKey === value && this.reverse === 'asc',
                        'hidden': this.sortKey !== value
                    }
                },
                setPage: function (pageNumber) {
                    this.currentPage = pageNumber;
                },
            },
            watch: {
                'filter': {
                    handler: function () {
                        //if (this.filteredTransferList.length !== this.transferList.length && this.selectedAll) {
                        this.selectedAll = false;
                        for (var i = 0; i < app.filteredAppList.length; i++) {
                            app.filteredAppList[i].checked = false;
                        }
                        //}
                    },
                    deep: true
                }
            },
            computed: {
                filteredAppList: function () {
                    var results = this.appList;

                    //фильтр       
                    $.each(this.filter, function (key, value) {
                        results = _filterByKey(results, key, value);
                    });

                    //сортировка
                    if (this.sortKey) {
                        results = _sortByKey(results, this.sortKey, this.reverse);
                    }

                    //пейджинг
                    this.resultCount = results.length;
                    this.totalPages = Math.ceil(this.resultCount / this.itemsPerPage);

                    if (this.itemsPerPage) {
                        var index = this.currentPage * this.itemsPerPage;
                        var tmp = results.slice(index, index + this.itemsPerPage);
                        if (tmp.length !== 0) {
                            results = tmp;
                        }
                        if (tmp.length === 0 && this.currentPage !== 0) {
                            this.setPage(0);
                            index = this.currentPage * this.itemsPerPage;
                            results = results.slice(index, index + this.itemsPerPage);
                        }
                    }

                    return results;
                },
                checkedAppIds: function () {
                    var res = [];
                    for (var i = 0; i < this.appList.length; i++) {
                        if (this.appList[i].checked) {
                            res.push(app.appList[i].ApplicationItemId);
                        }
                    }
                    return res;
                },
                allowRefuseButton: function () {
                    if (!this.allowEditDisagreed) return false;
                    var res = true;
                    var counter = 0;
                    for (var i = 0; i < this.appList.length; i++) {
                        var record = this.appList[i];
                        if (record.checked) counter++;
                        if (record.checked && (!record.IsDisagreed || !record.IsDisagreedDateStr || record.IsDisagreedDateStr==='')) {
                            res = false;
                            break;
                        }
                    }
                    if (counter === 0) res = false;
                    return res;
                },
                allowDeleteButton: function () {
                    var res = false
                    for (var i = 0; i < this.appList.length; i++) {
                        var record = this.appList[i];
                        if (record.checked && record.AllowExcludeAction) {
                            res = true;
                            break;
                        }
                    }
                    return res;
                }
            }
        });
</script>
<% Html.RenderPartial("OrderOfAdmission/Dialogs/ExcludeApplicationsDialog"); %>
<% Html.RenderPartial("InstitutionApplication/Dialogs/ViewApplication"); %>