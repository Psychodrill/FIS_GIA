<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.AdmissionVolume.VolumeTransferViewModel>" %>

<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Model.Institutions" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Controllers.Admission" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.Security" %>
<%@ Import Namespace="GVUZ.DAL.Dto" %>

<%@ Register TagPrefix="gv" TagName="TabControl" Src="~/Views/Shared/Common/InstitutionsTabControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Объем и структура приема</asp:Content>
<asp:Content ID="header" ContentPlaceHolderID="PageTitle" runat="server">
    Сведения об образовательной организации</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageHeaderContent">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="divstatement" id="VolumeTransfer">        
        <gv:TabControl runat="server" ID="tabControl" />
            
        <% if (Model.HasPlan) { %>
        <h3 style="display: inline-block; padding-left:30px">Приемная кампания: {{campaign.Name}} ({{campaign.YearStart}})</h3>
        <div style="display: block; padding-left:50px" >
            <input type="button" class="button3" value="Назад" title="Перейти к объему приема" onclick="goToAV();" />
            <input type="button" class="button3" @click="openTransferDialog()" value="Переброс мест">
        </div>       
        <div class="field-validation-error" style="padding-left: 60px"> 
            <span v-if="transferVolumeCheck.length !== 0" class="linkSumulator" @click="modalMode = 'check'" v-tooltip.right="{ content: 'Посмотреть детализацию', delay: { show: 1000, hide: 100 } }">
                Внимание! Выделенные в объеме приема места не распределены в конкурсах
            </span>
            <br />
            <span v-if="!transferBenefitCheck">
                Внимание! Отсутствуют зачисленные на обучение на приоритетном этапе зачисления
            </span>
        </div>
        <div style="display: inline-block; padding-left: 60px">Направление подготовки/специальность: 
            <input type="text" v-model="filter.Direction"/>
        </div>
        <div v-for="form in filter.EducationForms" style="display: inline-block; padding-left:50px">
            <input type="checkbox" @change="filterForm(form)" v-model="form.checked" :value="form.Name"/> {{form.Name}}
        </div>
        <input style="margin-left:100px" v-model="noFreePlaces" type="checkbox"  /> Нет свободных мест
        <table class="gvuzDataGrid tableStatement2">
            <thead>
                <tr>
                    <th style="text-align: center; width: 3%" rowspan="2">
                        <input type="checkbox" v-model="selectedAll" @click="selectAll()" />                        
                    </th>
                    <th style="width:25%" rowspan="2">
	                    <span class="sortable linkSumulator" @click="sortBy('Direction')">
                            Направление подготовки/специальность
	                    </span>
                        <span v-show="sortKey === 'Direction' && reverse === 'desc'" class="sortDown"></span>
                        <span v-show="sortKey === 'Direction' && reverse === 'asc'" class="sortUp"></span>
                    </th>
                    <th style="width:10%" rowspan="2">
	                    <span class="sortable linkSumulator" @click="sortBy('EducationForm')">
                            Форма обучения
	                    </span>
                        <span v-show="sortKey === 'EducationForm' && reverse === 'desc'" class="sortDown"></span>
                        <span v-show="sortKey === 'EducationForm' && reverse === 'asc'" class="sortUp"></span>
                    </th>
                    <th style="width:25%; text-align: center" colspan="2">
                         Свободные места
                    </th>
                    <th style="width:25%; text-align: center" rowspan="2">
                         Контрольные цифры приема <br /> (Общий конкурс)
                    </th>
                </tr>
                <tr>
                    <th style=" text-align: center">Особая квота</th>
                    <th style=" text-align: center">Целевой прием</th>
                </tr>
            </thead>
            <tbody>
				<tr v-for="(transfer, index) in filteredTransferList" :class="[index % 2 === 0 ? 'trline1' : 'trline2']">
                    <td><input type="checkbox" :disabled="((transfer.CGTargetTotal - transfer.OrderTargetTotal) + (transfer.CGQuotaTotal - transfer.OrderQuotaTotal)) === 0" v-model="transfer.checked"/></td>
                    <td>
                        <span class="linkSumulator" @click="viewCompetitiveGroups(transfer.CompetitiveGroups)">
                            {{transfer.Direction}}
                        </span>
                    </td>
                    <td>{{transfer.EducationForm}}</td>
                    <td style="text-align: center">
                        <%--<div v-if="(transfer.CGQuotaTotal - transfer.OrderQuotaTotal) === 0">
                            Отсутствуют свободные места
                            <br />
                            Занято
                            <span v-tooltip.bottom="{ 
                                content: ('Федеральный: ' + transfer.CGQuotaFed 
                                + '<br/>Региональный: ' + transfer.CGQuotaReg 
                                + '<br/>Муниципальный: ' + transfer.CGQuotaMun),
                                classes: ['tooltip'], delay: { show: 400, hide: 400 } }">{{transfer.CGQuotaTotal}}
                            </span>
                        </div>--%>
                        <div v-if="(transfer.CGQuotaTotal - transfer.OrderQuotaTotal) !== 0">
                            <span v-tooltip.bottom="{ 
                                content: ('Федеральный: ' + (transfer.CGQuotaFed - transfer.OrderQuotaFed)
                                + '<br/>Региональный: ' + (transfer.CGQuotaReg - transfer.OrderQuotaReg) 
                                + '<br/>Муниципальный: ' + (transfer.CGQuotaMun - transfer.OrderQuotaMun)),
                                classes: ['tooltip'], delay: { show: 400, hide: 400 } }">{{transfer.CGQuotaTotal - transfer.OrderQuotaTotal}} 
                            </span>
                            из 
                            <span v-tooltip.bottom="{ 
                                content: ('Федеральный: ' + transfer.CGQuotaFed 
                                + '<br/>Региональный: ' + transfer.CGQuotaReg 
                                + '<br/>Муниципальный: ' + transfer.CGQuotaMun),
                                classes: ['tooltip'], delay: { show: 400, hide: 400 } }">{{transfer.CGQuotaTotal}}
                            </span>
                        </div>
                    </td>
                    <td style="text-align: center">
                        <%--<div v-if="(transfer.CGTargetTotal - transfer.OrderTargetTotal) === 0">
                            Отсутствуют свободные места
                            <br />
                            Занято
                            <span v-tooltip.bottom="{ 
                                content: ('Федеральный: ' + transfer.CGTargetFed 
                                + '<br/>Региональный: ' + transfer.CGTargetReg 
                                + '<br/>Муниципальный: ' + transfer.CGTargetMun),
                                classes: ['tooltip'], delay: { show: 400, hide: 400 } }">{{transfer.CGTargetTotal}}
                            </span>
                        </div>--%>
                        <div v-if="(transfer.CGTargetTotal - transfer.OrderTargetTotal) !== 0">
                            <span v-tooltip.bottom="{ 
                                content: ('Федеральный: ' + (transfer.CGTargetFed - transfer.OrderTargetFed)
                                + '<br/>Региональный: ' + (transfer.CGTargetReg - transfer.OrderTargetReg) 
                                + '<br/>Муниципальный: ' + (transfer.CGTargetMun - transfer.OrderTargetMun)),
                                classes: ['tooltip'], delay: { show: 400, hide: 400 } }">{{transfer.CGTargetTotal - transfer.OrderTargetTotal}}
                            </span> 
                            из 
                            <span v-tooltip.bottom="{ 
                                content: ('Федеральный: ' + transfer.CGTargetFed 
                                + '<br/>Региональный: ' + transfer.CGTargetReg 
                                + '<br/>Муниципальный: ' + transfer.CGTargetMun),
                                classes: ['tooltip'], delay: { show: 400, hide: 400 } }">{{transfer.CGTargetTotal}}
                            </span>
                         </div>
                    </td>
                    <td style="text-align: center">
                        <span 
                            v-tooltip.bottom="{ 
                            content: ('Федеральный: ' + transfer.BudgetFed 
                            + '<br/>Региональный: ' + transfer.BudgetReg 
                            + '<br/>Муниципальный: ' + transfer.BudgetMun),
                            classes: ['tooltip'], delay: { show: 400, hide: 400 } }">{{transfer.BudgetTotal}} 
                        </span>
                        <div style="display: inline-block" v-if="((transfer.CGTargetTotal - transfer.OrderTargetTotal) + (transfer.CGQuotaTotal - transfer.OrderQuotaTotal)) !== 0">
                            => 
                            <span v-tooltip.bottom="{ 
                                content: ('Федеральный: ' + (transfer.BudgetFed + (transfer.CGTargetFed - transfer.OrderTargetFed) + (transfer.CGQuotaFed - transfer.OrderQuotaFed))
                                + '<br/>Региональный: ' + (transfer.BudgetReg + (transfer.CGTargetReg - transfer.OrderTargetReg) + (transfer.CGQuotaReg - transfer.OrderQuotaReg))
                                + '<br/>Муниципальный: ' + (transfer.BudgetMun + (transfer.CGTargetMun - transfer.OrderTargetMun) + (transfer.CGQuotaMun - transfer.OrderQuotaMun))),
                                classes: ['tooltip'], delay: { show: 400, hide: 400 } }">{{transfer.BudgetTotal + (transfer.CGTargetTotal - transfer.OrderTargetTotal) + (transfer.CGQuotaTotal - transfer.OrderQuotaTotal)}}
                            </span>
                        </div>
                    </td>
				</tr>
			</tbody>

            <tfoot>
                <tr>
                    <th colspan="10">
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
        <% } %>

        <modal v-if="modalMode === 'check'" @close="modalMode !== 'check'">
            <div slot="header">Выделенные в объеме приема места не распределены в конкурсах</div>
            <div slot="body">
                <table class="gvuzDataGrid tableStatement2">
                    <thead>
                        <tr>
                            <th style="width:25%">
                                 Направление подготовки
                            </th>
                            <th style="width:10%">
                                 Уровень образование
                            </th>
                            <th style="width:10%">
                                Источник финансирования
                            </th>
                            <th style="width:12%">
                                Форма обучения
                            </th>
                            <th style="width:10%; text-align:center;">
                                Мест в объеме приема
                            </th>
                            <th style="width:10%; text-align:center;">
                                Мест в конкурсах
                            </th>
                        </tr>
                    </thead>
                    <tbody>
	                    <tr v-for="(item, index) in transferVolumeCheck" :class="[index % 2 === 0 ? 'trline1' : 'trline2']">
                            <td>{{item.DirectionCode}} {{item.DirectionName}}</td>
                            <td>{{item.EducationLevelName}}</td>
                            <td>{{item.EducationSourceName}}</td>
                            <td>{{item.EducationFormName}}</td>
                            <td style="text-align:center">{{item.AVVolume}}</td>
                            <td style="text-align:center">{{item.CGVolume}}</td>
	                    </tr>
                    </tbody>
               </table>
            </div>
            <div slot="footer">
                <input style="margin: 3px;" type="button" value="Закрыть" @click="closeDialog()" class="button" style="width: auto;"/>
             </div>
        </modal>

        <modal v-if="modalMode === 'transfer'" @close="modalMode !== 'transfer'">
            <div slot="header">Конкурсы</div>
            <div slot="body">
                <table class="gvuzDataGrid tableStatement2">
                    <thead>
                        <tr>
                            <th style="width:15%">
                                 Наименование
                            </th>
                            <th style="width:20%">
                                 Образовательные программы
                            </th>
                            <th style="width:15%">
                                 Уровень бюджета
                            </th>
                            <th style="width:15%; text-align:center;">
                                Свободные места
                            </th>f
                            <th style="width:15%; text-align:center;">
                                Контрольные цифры приема <br /> (Общий конкурс)
                            </th>
                        </tr>
                    </thead>
                    <tbody>
	                    <tr v-for="(group, index) in modalCompetitiveGroups" :class="[index % 2 === 0 ? 'trline1' : 'trline2']">
                            <td>{{group.Name}}</td>
                            <td>
                                <p v-for="(program, idx) in group.Programs">{{idx+1}}) {{program.Code}} {{program.Name}}</p>
                            </td>
                            <td>{{group.BudgetName}}</td>
                            <td style="text-align:center;">
                                <div v-if="(group.CGTotal - group.OrderTotal !== 0)">
                                    <div v-if="group.EducationSourceId === 16" style="display: inline-block; text-align:center; padding: 5px; width: 45%">
                                        <b v-tooltip.bottom="{ content: ('Конкурс: ' + (group.Name)), classes: ['tooltip'], delay: { show: 400, hide: 400 } }">Целевой прием</b>
                                        <br />
                                        {{group.CGTotal - group.OrderTotal}} из {{group.CGTotal}}
                                    </div>
                                    <div v-if="group.EducationSourceId === 20" style="display: inline-block; text-align:center; padding: 5px; width: 45%">
                                        <b v-tooltip.bottom="{ content: ('Конкурс: ' + (group.Name)), classes: ['tooltip'], delay: { show: 400, hide: 400 } }">Особая квота</b>
                                        <br />
                                        {{group.CGTotal - group.OrderTotal}} из {{group.CGTotal}}
                                    </div>
                                </div>
                            </td>
                            <td style="text-align:center;">
                                <div style="display: inline-block; text-align:center; padding: 5px; width: 45%" v-tooltip.bottom="{ content: ('Конкурс: ' + (group.budget_Name)), classes: ['tooltip'], delay: { show: 400, hide: 400 } }">
                                    <%--<b >Бюджетные места</b>
                                    <br />--%>
                                    {{group.budget_CGTotal}} 
                                    <span v-if="group.budget_CGTotal !== (group.budget_CGTotal + (group.CGTotal - group.OrderTotal))">
                                        => {{group.budget_CGTotal + (group.CGTotal - group.OrderTotal)}}
                                    </span>
                                </div>
                            </td>
	                    </tr>
                    </tbody>
               </table>
            </div>
            <div slot="footer">
                <input style="margin: 3px;" type="button" value="Закрыть" @click="closeDialog()" class="button" style="width: auto;"/>
             </div>
        </modal>
    
    
        <modal v-if="modalMode === 'prompt'" @close="modalMode !== 'prompt'">
            <div slot="header">Переброс мест</div>
            <div slot="body">
                <div>Будет выполнен переброс мест по следующим направлениям:</div>
                <br />
                <div v-for="(dir, index) in checkedTransferList"> {{index+1}}) {{dir.Direction}} ({{dir.EducationForm}}) </div>
                <br />
                <div class="field-validation-error">Внимание! Отменить это действие будет невозможно!</div>
            </div>
            <div slot="footer">
                <input style="margin: 3px;" type="button" value="Выполнить переброс мест" @click="beginTransfer()" class="button" style="width: auto;"/>
                <input style="margin: 3px;" type="button" value="Отмена" @click="closeDialog()" class="button" style="width: auto;"/>
            </div>
        </modal>
    </div>

    <script type="text/javascript">
        menuItems[2].selected = true;

        Vue.component('modal', {
            template: modalTemplate
        })
        Vue.component('v-popover', VTooltip.VPopover)
        Vue.directive('tooltip', VTooltip.VTooltip)

        Vue.use(window['vue-easy-toast'].default)

        var app = new Vue({
            el: '#VolumeTransfer',
            data: function() {
                return {
                    modalMode: null,
                    campaignId: null,
                    selectedAll: false,
                    hasPlan: true,
                    transferBenefitCheck: null,
                    transferVolumeCheck: [],
                    campaign: {},
                    transferList: [],
                    checkedTransferList: [],
                    modalCompetitiveGroups: [],
                    //Справочники
                    educationLevels: [],
                    educationSources: [],
                    educationForms: [],

                    //Сортировка
                    sortKey: null,
                    reverse: 'asc',
                    filter: {
                        Direction: null,
                        EducationForms: {},
                    },
                    noFreePlaces: true,
                    ////Пейджинг
                    totalPages: 0,
                    currentPage: 0,
                    itemsPerPage: 10,
                    resultCount: 0
                };
            },
            created: function () {
                this.loadData();
            },
            methods: {
                loadData: function () {
                    this.transferList = [];
                    this.campaignId = JSON.parse('<%= Html.Serialize(Model.CampaignId) %>');
                    this.educationForms = JSON.parse('<%= Html.Serialize(Model.EducationForms) %>');
                    this.campaign = JSON.parse('<%= Html.Serialize(Model.Campaign) %>');
                    this.transferVolumeCheck = JSON.parse('<%= Html.Serialize(Model.TransferVolumeCheck) %>');
                    this.transferBenefitCheck = JSON.parse('<%= Html.Serialize(Model.TransferBenefitCheck) %>');
                    this.filter.EducationForms = this.educationForms;
                    for (var j = 0; j < this.filter.EducationForms.length; j++) {
                        this.filter.EducationForms[j].checked = true;
                    }

                    <% if (Model.HasPlan) { %>
                    doPostAjax('<%= Url.Action("LoadVolumeTransfer")%>', JSON.stringify({ campaignId: this.campaignId }), function (result) {
                        if (result && result.IsError) {
                            //app.error = result;
                            infoDialog(result.Message);
                        }
                        else if (result && !result.isError) {
                            app.transferList = result;
                        }
                    });
                    <% } %>
                    <% else { %>
                        infoDialog('Не задан плановый объем приема');
                    <% } %>
                },
                viewCompetitiveGroups: function (competitiveGroups) {
                    this.modalCompetitiveGroups = competitiveGroups;
                    this.modalMode = 'transfer';
                },
                selectAll: function () {
                    app.selectedAll = !app.selectedAll;

                    for (var i = 0; i < app.filteredTransferList.length; i++) {
                        var item = app.filteredTransferList[i];
                        if (((item.CGTargetTotal - item.OrderTargetTotal) + (item.CGQuotaTotal - item.OrderQuotaTotal)) !== 0) {                          
                            item.checked = app.selectedAll;
                        }
                    }
                    app.filteredTransferList = app.filteredTransferList.slice(0);
                    app.transferList = app.transferList.slice(0);
                },
                openTransferDialog: function () {
                    app.checkedTransferList = [];
                    for (var i = 0; i < app.transferList.length; i++) {
                        var item = app.transferList[i];
                        if (item.checked) {
                            app.checkedTransferList.push(item);
                            //item.checked = false;
                        }
                    }                   
                    if (app.checkedTransferList.length > 0) {
                        app.modalMode = 'prompt';
                    }
                    else {
                        _errorToast('Не выбрано ни одного условия для переброса мест!');
                    }
                },
                beginTransfer: function() {
                    var competitiveGroupIDs = [];
                    for (var i = 0; i < app.checkedTransferList.length; i++) {
                        var transfer = app.checkedTransferList[i];
                        for (var j = 0; j < transfer.CompetitiveGroups.length; j++) {
                            competitiveGroupIDs.push(transfer.CompetitiveGroups[j].CompetitiveGroupID);
                        }
                    }
                    var data = {};
                    data.campaignId = app.campaignId;
                    data.competitiveGroupIDs = competitiveGroupIDs;

                    doPostAjax('<%= Url.Action("BeginVolumeTransfer") %>', JSON.stringify(data), function (res) {
                            if (res) {
                                doPostAjax('<%= Url.Action("LoadVolumeTransfer")%>', JSON.stringify({ campaignId: app.campaignId }), function (result) {
                                    if (result) {
                                        app.transferList = result;
                                        app.closeDialog();
                                        _successToast('Переброс мест для приемной кампании ' + app.campaign.Name + ' (' + app.campaign.YearStart + ') завершен');
                                    }
                                });
                            }
                        }
                    );

                },
                closeDialog: function () {
                    this.modalCompetitiveGroups = [];
                    this.modalMode = null;
                },
                sortBy: function (sortKey) {
                    if (this.sortKey === sortKey) {
                        this.reverse = this.reverse === 'asc' ? 'desc' : 'asc';
                    }
                    else {
                        this.reverse = 'desc';
                        this.sortKey = sortKey;
                    }                   
                },
                setPage: function (pageNumber) {
                    this.currentPage = pageNumber;
                },
                filterForm: function (form) {
                    //for computed reaction...
                    app.filter.EducationForms = app.filter.EducationForms.slice(0);
                }                
            },
            watch: {
                'filter': {
                    handler: function () {
                        //if (this.filteredTransferList.length !== this.transferList.length && this.selectedAll) {
                            this.selectedAll = false;
                            for (var i = 0; i < app.transferList.length; i++) {                   
                                app.transferList[i].checked = false;
                            }
                        //}
                    },
                    deep: true
                }
            },
            computed: {
                filteredTransferList: function () {
                    var results = [];

                    //фильтр 
                    for (var j = 0; j < this.filter.EducationForms.length; j++) {

                        if (this.filter.EducationForms[j].checked) {
                            var value = this.filter.EducationForms[j].Name
                            var t = _filterByKey(this.transferList, 'EducationFormId', this.filter.EducationForms[j].Id);
                            results = results.concat(t);
                        }
                    }
                    if (!this.noFreePlaces) {
                        for (var j = 0; j < results.length; j++) {
                            var item = results[j];
                            if ((item.CGTargetTotal - item.OrderTargetTotal) === 0
                                && (item.CGQuotaTotal - item.OrderQuotaTotal) === 0) {
                                results.splice(j--, 1);
                            }
                        }
                    }
                    results = _filterByKey(results, 'Direction', this.filter.Direction);
                        
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
