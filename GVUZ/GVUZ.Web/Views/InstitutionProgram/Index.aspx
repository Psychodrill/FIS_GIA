<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="FogSoft.Helpers" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.Security" %>

<%@ Register TagPrefix="gv" TagName="TabControl" Src="~/Views/Shared/Common/InstitutionsTabControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Образовательные программы
</asp:Content>
<asp:Content ID="header" ContentPlaceHolderID="PageTitle" runat="server">Сведения об образовательной организации</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="divstatement" id="InstitutionProgramList" v-cloak>
        <gv:TabControl runat="server" ID="tabControl" />
        <script type="text/javascript">
            menuItems[6].selected = true;
        </script>

        <%--Ошибки--%>
        <div style="color: rgb(178, 34, 34);">
            <a v-if="error.Name">Поле "Наименование" должно быть заполнено! <br></a>
            <a v-if="error.UID"> Значение в поле UID должно быть уникальным среди всех образовательных программ ОО!<br></a>
            <a v-if="error.CodeName">Совокупность значений в полях "Код ОП" и "Наименование ОП" должна быть уникальной среди всех образовательных программ ОО!<br></a>
            <br />
        </div>

        <%--Панель с кнопками--%>
        <input :disabled="editMode" class="button3" @click="AddNew()" type="button" value="Добавить">

        <%--Фильтр--%>
        <div class="tableHeader22" style="height: 100%">
            <div v-show="!showFilter" class="hideTable"  @click="ToggleFilter()" style="float: left">
                <span id="btnShowFilter">Отобразить фильтр</span>    
            </div>
            <div class="tableHeader5l" v-show="showFilter">
                <div class="hideTable nonHideTable" style="float: left;" @click="ToggleFilter()">
                    <span>Скрыть фильтр</span>
                </div>
                <table class="tableForm">
                    <tbody>
                        <tr>
                            <td style="width:25%;">
                                Код: &nbsp;
                                <input style="width:80%" type="text" v-model="filter.Code" />
                            </td>
                            <td style="width:45%;">
                                Наименование: &nbsp;
                                <input style="width:80%" type="text" v-model="filter.Name" />
                            </td>
                            <td style="width:25%;">
                                UID: &nbsp;
                                <input style="width:80%" type="text" v-model="filter.UID" />
                            </td>    
                            <td style="width:5%; text-align: center;">
                                <input type="button" class="button" value="Сбросить" style="width: auto" @click="ClearFilter()" />
                            </td>                                                  
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>

        <%--грид--%>
        <table class="gvuzDataGrid tableStatement2">
            <thead>
                <tr>
                    <th style="width:25%">
	                    <span class="sortable linkSumulator" @click="SortBy('Code')">
                            Код ОП
	                    </span>
                        <span v-show="sortKey === 'Code' && reverse === 'desc'" class="sortDown"></span>
                        <span v-show="sortKey === 'Code' && reverse === 'asc'" class="sortUp"></span>
                    </th>
                    <th style="width:45%">
	                    <span class="sortable linkSumulator" @click="SortBy('Name')">
                            Наименование ОП
	                    </span>
                        <span v-show="sortKey === 'Name' && reverse === 'desc'" class="sortDown"></span>
                        <span v-show="sortKey === 'Name' && reverse === 'asc'" class="sortUp"></span>
                    </th>
                    <th style="width:25%">
	                    <span class="sortable linkSumulator" @click="SortBy('UID')">
                            UID
	                    </span>
                        <span v-show="sortKey === 'UID' && reverse === 'desc'" class="sortDown"></span>
                        <span v-show="sortKey === 'UID' && reverse === 'asc'" class="sortUp"></span>
                    </th>
                    <th style="text-align:center; width:5%">
                        Действия
                    </th>
                </tr>
            </thead>
            <tbody>
				<tr v-for="(program, index) in filteredProgramList" :class="[index % 2 === 0 ? 'trline1' : 'trline2']">
                    <td>
                        <div v-if="!program.IsEdit">{{program.Code}}</div>
                        <input type="text" v-if="program.IsEdit" v-model="program.Code" maxlength="10" :class="[error.CodeName ? 'input-validation-error' : '']">
                    </td>
                    <td>
                        <div v-if="!program.IsEdit && program.CanRemove">{{program.Name}}</div>
                        <a v-if="!program.IsEdit && !program.CanRemove" @click="showGroups(program)"  href="javascript:void(0)">{{program.Name}}</a>
                        <input type="text" v-if="program.IsEdit" v-model="program.Name" maxlength="200" :class="[error.Name || error.CodeName ? 'input-validation-error' : '']"/>
                    </td>
                    <td>
                        <div v-if="!program.IsEdit">{{program.UID}}</div>
                        <input type="text" v-if="program.IsEdit" v-model="program.UID" maxlength="200" :class="[error.UID ? 'input-validation-error' : '']"/>
                    </td>
                    <td v-if="editMode" style="text-align: center;">
                        <a v-if="!program.IsEdit" href="" class="btnSave" style="visibility: hidden;">&nbsp;</a>
                        <a v-if="program.IsEdit" href="javascript:void(0)" class="btnSave" @click="Save(program)" title="Сохранить">&nbsp;</a>
                        <a v-if="program.IsEdit" href="javascript:void(0)" class="btnCancel" @click="CancelEdit(program)" title="Отмена">&nbsp;</a>
                    </td>
                    <td v-if="!editMode" style="text-align: center;">
                        <a href="javascript:void(0)" class="btnEdit" @click="DoEdit(program)" title="Редактировать">&nbsp;</a>
                        <a v-if="program.CanRemove" href="javascript:void(0)" class="btnDelete" @click="DoDelete(program)" title="Удалить">&nbsp;</a>
                        <a v-if="!program.CanRemove" href="javascript:void(0)" class="btnDeleteGray" title="Используется в конкурсе">&nbsp;</a>
                    </td>
				</tr>
			</tbody>

            <tfoot>
                <tr>
                    <th colspan="10">
                        <div style="text-align: center;white-space: nowrap">
                            <span class="pageLink">Всего записей: {{resultCount}}, страниц: {{totalPages}}</span>
                            <a href="javascript:void(0)" class="pageLink pageLinkArrowLeftLeft" @click="SetPage(0)">&nbsp;</a>
                            <span v-for="pageNumber in totalPages" 
                                    v-if="Math.abs(pageNumber - currentPage - 1) < 3">
                                <a href="javascript:void(0)" :class="{pageLinkActive: currentPage === pageNumber - 1}" class="pageLink" @click="SetPage(pageNumber-1)">{{ pageNumber }}</a>
                            </span>
                            <a href="javascript:void(0)" class="pageLink pageLinkArrowRightRight" @click="SetPage(totalPages-1)">&nbsp;</a>
                            <span class="pageLink" style="display: inline;">
                                Записей на странице:&nbsp;
                                <select style="width: 55px; height: 23px; font-size: 8pt" v-model="itemsPerPage" @change="SetPage(0)">
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

        <modal v-if="showModal" @close="showModal = false">
            <div slot="header">Связанные конкурсы</div>
            <div slot="body">
                <table class="gvuzDataGrid tableStatement2">
                    <thead>
                        <tr>
                            <th style="width:45%">
	                            <span class="sortable">
                                    Конкурс
	                            </span>
                            </th>
                            <th style="width:45%">
	                            <span class="sortable">
                                    Кампания
	                            </span>
                            </th>
                            <th style="width:2%">
	                            <%--<span class="sortable">
                                    Перейти
	                            </span>--%>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
	                    <tr v-for="(group, index) in selectedProgramGroups" :class="[index % 2 === 0 ? 'trline1' : 'trline2']">
                            <td>{{group.CompetitiveGroupName}}</td>
                            <td>{{group.CampaignName}}</td>
                            <td><a style="text-align: center;" href="javascript:void(0)" @click="openGroup(group)" class="btnMove"></a></td>
	                    </tr>
                    </tbody>
                </table>
            </div>
            <div slot="footer">
                 <input style="margin: 3px;" type="button" value="Закрыть" @click="closeDialog()" class="button" style="width: auto;"/>
            </div>
        </modal>
    </div>

    

    <script type="text/javascript">
        Vue.use(window['vue-easy-toast'].default)
        Vue.component('modal', {
            template: modalTemplate
        })

        var app = new Vue({
            el: '#InstitutionProgramList',
            data: function () {
                return {
                    programList: [],
                    lastProgram: {},
                    editMode: false,
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
                        Name: null,
                        Code: null,
                        UID: null
                    },
                    error: {
                        Name: false,
                        CodeName: false,
                        UID: false
                    },
                    selectedProgramGroups: [],
                    showModal: false
                }
            },
            created: function () {
                this.LoadData();
            },
            methods: {
                LoadData: function () {
                    doPostAjax('<%= Url.Action("LoadRecords", "InstitutionProgram") %>', '', function (res) {
                        if (res) {
                            app.programList = res;
                        }
                    });
                },
                SetPage: function (pageNumber) {
                    input - validation - error
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
                SortBy: function (sortKey) {
                    if (this.sortKey === sortKey) {
                        this.reverse = this.reverse === 'asc' ? 'desc' : 'asc';
                    }
                    else {
                        this.reverse === 'desc';
                        this.sortKey = sortKey;
                    }
                },
                DoDelete: function (program) {
                    confirmDialog("Вы действительно хотите удалить программу?", function () {
                        doPostAjax('<%= Url.Generate<InstitutionProgramController>(x => x.ProgramDelete(null)) %>?institutionProgramID=' + program.InstitutionProgramID, '', function (data) {
                            if (!data.success)
                                _errorToast('При удалении образовательной программы ' + program.Name + ' произошла ошибка! <br/><br/>' + data.errors[0]);
                            else {
                                var index = app.programList.indexOf(program);
                                app.programList.splice(index, 1);

                                _successToast('Образовательная программа ' + program.Name + ' удалена успешно!');
                            }
                        }
                        );
                    });
                },
                DoEdit: function (program) {
                    app.lastProgram = {};
                    app.lastProgram = jQuery.extend({}, program);

                    var index = app.programList.indexOf(program);
                    program.IsEdit = true;
                    app.programList.splice(index, 1, program);

                    app.editMode = true;
                },
                CancelEdit: function (program) {
                    var index = app.programList.indexOf(program);
                    if (program.New) {
                        app.programList.splice(index, 1);
                    }
                    if (!program.New) {
                        program.IsEdit = false;
                        app.programList.splice(index, 1, app.lastProgram);
                    }
                    app.editMode = false;
                    app.error.CodeName = false;
                    app.error.UID = false;
                    app.error.Name = false;
                },
                Save: function (program) {
                    app.error.Name = false;
                    if (!program.Name || program.Name.length == 0 || /^\s+$/.test(program.Name)) {
                        app.error.Name = true;
                        return;
                    }
                    doPostAjax('<%= Url.Action("UpdateProgram", "InstitutionProgram") %>', JSON.stringify(program), function (res) { 
                        if (res && res[0].error == 0) {
                            app.error.UID = false;
                            app.error.CodeName = false;
                            app.error.Name = false;
                            Vue.toast(
                            'Образовательная программа ' + (program.Code ? program.Code : '')  + ' ' + program.Name + ' сохранена успешно!',
                            {
                                className: ['toast-info'],
                                duration: 3000,
                                horizontalPosition: 'center',
                                mode: 'override'
                            })

                            doPostAjax('<%= Url.Action("LoadRecords", "InstitutionProgram") %>', '', function (res) {
                                if (res) {
                                    app.programList = res;
                                }
                            });

                            app.editMode = false;
                        }
                        else if (res && res[0].error != 0) {

                            if (res.length == 2) {
                                app.error.CodeName = true;
                                app.error.UID = true;
                            }
                            else if (res[0].error == 1) { app.error.CodeName = true; app.error.UID = false; }
                            else if (res[0].error == 2) { app.error.UID = true; app.error.CodeName = false; }
                            else { app.error.UID = false; app.error.CodeName = false; }
  
                        }
                    })
                },
                AddNew: function () {
                    var newPr = {
                        IsEdit: true,
                        CanRemove: true,
                        New: true
                    };
                    app.programList.unshift(newPr);
                    app.editMode = true;
                },
                showGroups: function (program) {
                    if (program.CompetitiveGroups && program.CompetitiveGroups.length > 0) {
                        app.selectedProgramGroups = program.CompetitiveGroups;
                        app.showModal = true;
                    }
                },
                openGroup: function (group) {
                    window.location = '<%= Url.Generate<CompetitiveGroupController>(x => x.CompetitiveGroupEdit(null)) %>?competitiveGroupID=' + group.CompetitiveGroupID;
                },
                closeDialog: function () {
                    app.showModal = false;
                }
            },
            computed: {
                filteredProgramList: function () {
                    var results = this.programList;
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
                            this.SetPage(0);
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
