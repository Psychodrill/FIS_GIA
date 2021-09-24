<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Register TagPrefix="gv" TagName="TabControl" Src="~/Views/Shared/Common/InstitutionsTabControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Индивидуальные достижения</asp:Content>
<asp:Content ID="header" ContentPlaceHolderID="PageTitle" runat="server">Сведения об образовательной организации</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


<div class="divstatement" id="divstatement" v-cloak>
    <gv:tabcontrol runat="server" id="tabControl" />

    &nbsp;&nbsp;<span style="text-align: right;">Приемная кампания:</span>&nbsp;
    <select @change="loadRecords" :disabled="editMode" v-model="selectedCampaign">
        <option v-for="option in campaigns" v-bind:value="option">
            {{option.CampaignName}}
        </option>
    </select>
    <div style="color: rgb(178, 34, 34);" class="appCount" v-if="selectedCampaign.IsFinished">Чтобы редактировать список ИД требуется открыть набор для данной приемной кампании в разделе "Администрирование ОО"</div>

    <%--Ошибки--%>
    <div style="color: rgb(178, 34, 34);">
        <p v-if="errors.Name">{{errors.Name}}</p>
        <p v-if="errors.SelectedCategory">{{errors.SelectedCategory}}</p>
        <p v-if="errors.MaxValue">{{errors.MaxValue}}</p>
        <p v-if="errors.UID">{{errors.UID}}</p>
        <br />
    </div>

    <%--Панель с кнопками--%>
    <span style="display: block; margin: 15px 10px;">
        <input v-if="!selectedCampaign.IsFinished" :disabled="editMode" class="button3" @click="AddNew()" type="button" value="Добавить">
    </span>

    <%--Фильтр--%>
    <div class="tableHeader22" style="height: 100%">
        <div v-show="!showFilter" class="hideTable"  @click="ToggleFilter()" style="float: left">
            <span id="btnShowFilter">Отобразить фильтр</span>    
        </div>
        <div class="appCount no-filter" v-if="filteredRecords.length == records.length">
            Записей: {{records.length}}
        </div>
        <div class="tableHeader5l" v-show="showFilter">
            <div class="hideTable nonHideTable" style="float: left;" @click="ToggleFilter()">
                <span>Скрыть фильтр</span>
            </div>
            <div class="appCount filter" v-if="filteredRecords.length != records.length">
                Записей {{filteredRecords.length}} из {{records.length}}
            </div>
            <table class="tableForm">
                <tbody>
                    <tr>
                        <td style="width:25%;">
                            UID: &nbsp;
                            <input style="width:80%" type="text" v-model="filter.UID" />
                        </td>
                        <td style="width:45%;">
                            Наименование: &nbsp;
                            <input style="width:80%" type="text" v-model="filter.Name" />
                        </td>
                        <td style="width:25%;">
                            Категория: &nbsp;
                            <select v-model="filter.CategoryId">
                                <option v-for="option in categories" v-bind:value="option.CategoryId">
                                    {{option.CategoryName}}
                                </option>
                            </select>
                        </td>    
                        <tr>
                            <td colspan="3" align="center">
                                <input type="button" class="button" value="Сбросить фильтр" style="width: auto" @click="ClearFilter()" />
                            </td>
                        </tr>                                              
                    </tr>
                </tbody>
            </table>
        </div>
    </div>

    <%--Грид--%>
    <table class="gvuzDataGrid">
        <colgroup>
            <col style="width: 20%;"/>
            <col style="width: 40%"/>
            <col style="width: 30%"/>
            <col style="width: 10%"/>
            <col style="width: 5%"/>
        </colgroup>
        <thead>
            <tr>
                <th style="text-align: left; position: relative;">
                    <span @click="sortBy('UID')" class="sortable linkSumulator" >Идентификатор (UID)</span>
                    <span :class="sortSpan('UID')"></span>
                </th>
                <th style="text-align: left; position: relative;">
                    <span @click="sortBy('Name')" class="sortable linkSumulator" >Наименование достижения</span>
                    <span :class="sortSpan('Name')"></span>
                </th>
                <th style="text-align: left; position: relative;">
                    <span @click="sortBy('CategoryName')" class="sortable linkSumulator" >Категория</span>
                    <span :class="sortSpan('CategoryName')"></span>
                </th>
                <th style="text-align: left; position: relative;">
                    <span @click="sortBy('MaxValue')" class="sortable linkSumulator" >Макс. балл</span>
                    <span :class="sortSpan('MaxValue')"></span>
                </th>
                <th v-if="!selectedCampaign.IsFinished">
                    Действия
                </th>
            </tr>
        </thead>
        <tbody v-for="record in filteredRecords">
            <td>
                <div v-if="!record.IsEdit">{{record.UID}}</div>
                <input type="text" v-if="record.IsEdit" v-model="record.UID" maxlength="200" :class="[errors.UID ? 'input-validation-error' : '']"/>
            </td>
            <td>
                <div v-if="!record.IsEdit">{{record.Name}}</div>
                <input type="text" v-if="record.IsEdit" v-model="record.Name" maxlength="1000" :class="[errors.Name ? 'input-validation-error' : '']"/>
            </td>
            <td>
                <div v-if="!record.IsEdit">{{record.CategoryName}}</div>
                <select v-if="record.IsEdit" v-model="record.CategoryId" :class="[errors.SelectedCategory ? 'input-validation-error' : '']">
                    <option v-for="option in categories" v-bind:value="option.CategoryId">
                        {{option.CategoryName}}
                    </option>
                </select>
            </td>
            <td>
                <div v-if="!record.IsEdit">{{record.MaxValue}}</div>
                <input type="text" v-if="record.IsEdit" v-model="record.MaxValue" maxlength="40" :class="[errors.MaxValue ? 'input-validation-error' : '']"/>
            </td>
            <td v-if="editMode && !selectedCampaign.IsFinished" style="text-align: center;">
                <a v-if="!record.IsEdit" href="" class="btnSave" style="visibility: hidden;">&nbsp;</a>
                <a v-if="record.IsEdit" href="javascript:void(0)" class="btnSave" @click="Save(record)" title="Сохранить">&nbsp;</a>
                <a v-if="record.IsEdit" href="javascript:void(0)" class="btnCancel" @click="CancelEdit(record)" title="Отмена">&nbsp;</a>
            </td>
            <td v-if="!editMode && !selectedCampaign.IsFinished" style="text-align: center;">
                <a href="javascript:void(0)" class="btnEdit" @click="DoEdit(record)" title="Редактировать">&nbsp;</a>
                <a href="javascript:void(0)" class="btnDelete" @click="RemoveRecord(record)" title="Удалить">&nbsp;</a>
            </td>
        </tbody>
        <tfoot>
            <tr>
                <th colspan="5">
                    <div style="text-align: center;white-space: nowrap">
                        <span class="pageLink">Страниц: {{totalPages}}</span>
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


<script type="text/javascript">
    menuItems[3].selected = true;

    Vue.use(window['vue-easy-toast'].default)
    var app = new Vue({
        el: '#divstatement',
        data: function () {
            return {
                campaigns: [],
                categories: [],
                selectedCampaign: {},
                records: [],
                editMode: false,
                lastRrogram: {},
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
                    //campaign: null
                },
                errors: {
                    Name: null,
                    SelectedCategory: null,
                    MaxValue: null,
                    UID: null
                },
                actionUrl: {
                    loadRecords: '<%= Url.Action("LoadRecords") %>',
                    loadAssets: '<%= Url.Action("LoadCampaignsAndCategories") %>',
                    submit: '<%= Url.Action("UpdateRecord") %>',
                    remove: '<%= Url.Action("DeleteRecord") %>'
                }
            }
        },
        created: function () {
            this.LoadData();
        },
        methods: {
            LoadData: function () {
                doPostAjax(this.actionUrl.loadAssets, null, function (result) {
                    if (result) {
                        app.campaigns = result.Campaigns;
                        app.categories = result.Categories;
                        //console.log(result);
                        app.selectedCampaign = result.Campaigns.length > 0 ? result.Campaigns[0] : null;

                        app.loadRecords();
                    }
                })
            },
            loadRecords: function () {
                var loadParams = {campaignId: app.selectedCampaign.CampaignId.toString()};
                doPostAjax(app.actionUrl.loadRecords, JSON.stringify(loadParams), function (result) {
                    if (result) {
                        app.records = result;
                    }
                })
            },
            SetPage: function (pageNumber) {
                this.currentPage = pageNumber;
            },
            ToggleFilter: function () {
                this.showFilter = !this.showFilter;
            },
            ClearFilter: function () {
                $.each(app.filter, function (key, value) {
                    app.filter[key] = null;
                });
            },
            DoEdit: function (record) {
                app.lastRecord = jQuery.extend({}, record);

                var index = app.records.indexOf(record);
                record.IsEdit = true;
                app.records.splice(index, 1, record);

                app.editMode = true;
            },
            CancelEdit: function (record) {
                var index = app.records.indexOf(record);
                if (record.New) {
                    app.records.splice(index, 1);
                }
                if (!record.New) {
                    record.IsEdit = false;
                    app.records.splice(index, 1, app.lastRecord);
                }
                app.editMode = false;

                for (var x in app.errors) {
                    app.errors[x] = null;
                }
            },
            AddNew: function () {
                var newVal = {
                    IsEdit: true,
                    CanRemove: true,
                    New: true
                };
                app.records.unshift(newVal);
                app.editMode = true;
            },
            Validate: function (record) {
                for (var x in app.errors) {
                    app.errors[x] = null;
                }

                if (!record.Name || record.Name.length == 0 || /^\s+$/.test(record.Name)) {
                    app.errors.Name = 'Поле "Наименование" должно быть заполнено';
                }
                var num = parseInt(record.CategoryId);
                if (isNaN(num) || num <= 0) {
                    app.errors.SelectedCategory = 'Поле "Категория" должно быть заполнено';
                }
                if (!record.MaxValue || record.MaxValue.length == 0 || /^\s+$/.test(record.MaxValue)) {
                    app.errors.MaxValue = 'Поле "Макс. балл" должно быть заполнено';
                }
                else {
                    num = Number(record.MaxValue);
                    if (isNaN(num) || num < 0) {
                        app.errors.MaxValue = 'В поле "Макс. балл" указано некорректное значение. В качестве разделителя дробной части используйте точку.';
                    }
                }
                
                for (var x in app.errors) {
                    if (app.errors[x])
                        return false;
                }
                return true;

            },
            Save: function (record) {
                if (app.Validate(record)) {
                    record.campaignId = app.selectedCampaign.CampaignId;
                    doPostAjax(app.actionUrl.submit, JSON.stringify(record), function (result) {
                        if (result.success) {
                            var index = app.records.indexOf(record);
                            Vue.set(app.records, index, result.record);

                            app.editMode = false;
                            _successToast('Достижение ' + record.Name + ' сохранено успешно!');
                            //ko.mapping.fromJS(result.record, {}, record);
                            //list.selectedRecord(null);
                        } else {
                            if (result.errors) {
                                app.errors.UID = result.errors.UID || null;
                                app.errors.Name = result.errors.Name || null;
                                app.errors.SelectedCategory = result.errors.CategoryId || null;
                                app.errors.MaxValue = result.errors.MaxValue || null;
                            }
                        }
                    }, null, null, true);
                }
            },
            RemoveRecord: function (record) {
                confirmDialog('Вы уверены, что хотите удалить индивидуальное достижение?', function () {
                    doPostAjax(app.actionUrl.remove, JSON.stringify({ id: record.Id }), function (result) {
                        if (result.success) {
                            var index = app.records.indexOf(record);
                            app.records.splice(index, 1);
                            _successToast('Достижение ' + record.Name + ' удалено успешно!');
                        } else {
                            if (result.errors && result.errors.length > 0) {
                                _errorToast(result.errors.join('<br />'));
                            }
                        }
                    }, null, null, true);
                });
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
            }
        },
        computed: {
            filteredRecords: function () {
                var results = this.records;
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
            }
        }
    })
</script>
</asp:Content>
