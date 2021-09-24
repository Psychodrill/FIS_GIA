<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.DAL.Dapper.ViewModel.CompetitiveGroups.CompetitiveGroupViewModel>" %>

<%@ Import Namespace="FogSoft.Helpers" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.Security" %>

<%@ Register TagPrefix="gv" TagName="TabControl" Src="~/Views/Shared/Common/InstitutionsTabControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Конкурсы
</asp:Content>
<asp:Content ID="header" ContentPlaceHolderID="PageTitle" runat="server">Сведения об образовательной организации</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="divstatement" id="CompetitiveGroupList" v-cloak>
        <gv:TabControl runat="server" ID="tabControl" />

        <script type="text/javascript">
        menuItems[4].selected = true;

        <%--function saveFilter() {
            setCookie('campaignListFilter', JSON.stringify(prepareModel()), 1)
        };
        var serverFilterModel = JSON.parse('<%= Html.Serialize(Model.Filter) %>');--%>

        $(window).unload(function () {
            var href = document.activeElement.href;
            if (href) {
                sessionStorage.setItem('competitiveGroupListFilter', JSON.stringify(app.filter));
            }
            //if (href && href != 'javascript:void(0)') {
            //    //document.cookie = 'CompetitiveGroupListPage' + '=; expires=Thu, 01-Jan-70 00:00:01 GMT;';
            //}
        });
    </script>
        <modal v-if="showModal" @close="showModal = false">
            <!--
                you can use custom content here to overwrite
                default content
            -->
            <%--<h3 slot="header">custom header</h3>--%>
            <div slot="header">Скопировать конкурсы в другую приемную кампанию</div>
            <div slot="body">
                <table class="gvuzDataGrid tableStatement2" cellpadding="3" v-if="!copyEndMode">
                    <thead>
                        <tr>
                            <th style="width:20%">
	                            <span class="sortable" >
                                    Наименование
	                            </span>
                            </th>
                            <th style="width:5%">
	                            <span class="sortable" >
                                    Год ПК
	                            </span>
                            </th>
                            <th style="width:10%">
	                            <span class="sortable" >
                                    Приемная кампания
	                            </span>
                            </th>
                            <th  style="width:12%">
	                            <span class="sortable" >
                                    Уровень образования
	                            </span>
                            </th>
                            <th  style="width:10%">
	                            <span class="sortable" >
                                    Направление подготовки
	                            </span>
                            </th>
                            <th  style="width:8%">
	                            <span class="sortable" >
                                    УГС
	                            </span>
                            </th>
                            <th  style="width:10%">
	                            <span class="sortable" >
                                    Источник финансирования
	                            </span>
                            </th>
                            <th  style="width:12%">
	                            <span class="sortable" >
                                    Форма обучения
	                            </span>
                            </th>
                            <th  style="width:7%">
	                            <span class="sortable" >
                                    Доп. набор
	                            </span>
                            </th>
                            <th  style="text-align:center; width:5%">
                                Действия
                            </th>
                        </tr>
                    </thead>
                    <tbody>
	                    <tr v-for="(group, index) in checkedCompetitiveGroupList" :class="[index % 2 === 0 ? 'trline1' : 'trline2']">
                            <td v-tooltip.bottom="{ content: group.CompetitiveGroupName, classes: ['tooltip'], delay: { show: 1000, hide: 400 } }">{{group.CompetitiveGroupName}}</td>
                            <td>{{group.CampaignYearStart}}</td>
                            <td v-tooltip.bottom="{ content: group.CampaignName, classes: ['tooltip'], delay: { show: 1000, hide: 400 } }">{{group.CampaignName}}</td>
                            <td>{{group.EducationLevelName}}</td>
                            <td v-tooltip.bottom="{ content: group.DirectionName, classes: ['tooltip'], delay: { show: 1000, hide: 400 } }">{{group.DirectionName}}</td>
                            <td v-tooltip.bottom="{ content: group.UGSNAME, classes: ['tooltip'], delay: { show: 1000, hide: 400 } }">{{group.UGSNAME}}</td>
                            <td>{{group.EducationSourceName}}</td>
                            <td>{{group.EducationFormName}}</td>
                            <td>{{group.IsAdditionalName}}</td>
                            <td style="text-align: center;">
                                <a href="javascript:void(0)" class="btnCancel" @click="CancelCopy(group)" title="Отменить копирование">&nbsp;</a>
                            </td>
	                    </tr>
                    </tbody>
               </table>
                <%--отображаем если копирование завершено--%>
                <table class="gvuzDataGrid tableStatement2" v-if="copyEndMode">
                    <thead>
                        <tr>
                            <th style="text-align:center; width:25%">
	                            <span class="linkSumulator" >
                                    Наименование
	                            </span>
                            </th>
                            <th style="text-align:center; width:10%">
	                            <span class="linkSumulator" >
                                    Год ПК
	                            </span>
                            </th>
                            <th  style="text-align:center; width:65%">
	                            <span class="linkSumulator">
                                    Результат
	                            </span>
                            </th>
                    </thead>
                    <tbody>
	                    <tr v-for="(group, index) in checkedCompetitiveGroupList" :class="[index % 2 === 0 ? 'trline1' : 'trline2']">
                            <td v-tooltip.bottom="{ content: group.CompetitiveGroupName, classes: ['tooltip'], delay: { show: 1000, hide: 400 } }">{{group.CompetitiveGroupName}}</td>
                            <td>{{group.CampaignYearStart}}</td>
                            <td :class="[group.Message == 'Конкурс скопирован успешно.' ? 'text-success' : 'text-error']">{{group.Message}}</td>
	                    </tr>
                    </tbody>
               </table>
            </div>
            <table slot="footer">
                <tr v-if="!copyEndMode">
                    <td>
                        Год ПК <span class="required">(*)</span>:
                        <select style="margin: 3px;" v-model="copy_year" :class="[(app.copy_error && !copy_year) ? 'input-validation-error' : '']">
                            <option v-bind:value="null">Не выбрано</option>
                            <option v-for="option in сampaignStartYears" v-bind:value="option.ID">
                                {{ option.Name }}
                            </option>
                        </select>
                    </td>
                    <td>
                        Тип ПК <span class="required">(*)</span>:
                        <select style="margin: 3px;" v-model="copy_сampaignType" :class="[(app.copy_error && !copy_сampaignType) ? 'input-validation-error' : '']">
                            <option v-bind:value="null">Не выбрано</option>
                            <option v-for="option in сampaignTypes" v-bind:value="option.ID" >
                                {{ option.Name }}
                            </option>
                        </select>  
                    </td>
                    <td v-tooltip.bottom="{ content: 'Обязательное поле для всех конкурсов, кроме иностранцев по квоте МОН и платного финансирования', classes: ['tooltip'], delay: { show: 1000, hide: 400 } }">
                        Уровень Бюджета <span class="required">(*)</span>:
                        <select style="margin: 3px;" v-model="copy_levelBudget" :class="[(app.copy_error && !copy_levelBudget && copy_сampaignType != 5 && copy_paidCounter != checkedCompetitiveGroupList.length) ? 'input-validation-error' : '']">
                            <option v-bind:value="null">Не выбрано</option>
                            <option v-for="option in levelBudgets" v-bind:value="option.IdLevelBudget">
                                {{ option.BudgetName }}
                            </option>
                        </select>
                    </td>
                </tr>
                <tr v-if="!copyEndMode">
                    <td colspan="3">
                        <input style="margin: 3px;" type="button" value="Копировать" @click="CopyGroups()" class="button" style="width: auto;"/>
                        <input v-if="!copyEndMode" style="margin: 3px;" type="button" value="Отмена" @click="closeCopyDialog()" class="button" style="width: auto;"/>
                    </td>
                </tr>
                <tr v-if="copyEndMode">
                    <td colspan="3">
                        <input style="margin: 3px;" type="button" value="Закрыть" @click="closeCopyDialog()" class="button" style="width: auto;"/>
                    </td>
                </tr>
             </table>
        </modal>

        <div class="content">
            <%--Панель с кнопками--%>
            <input class="button3" value="Добавить" @click="addCG()" type="button">
            <input class="button3" value="Копировать" @click="openCopyDialog()" type="button">
            <input class="button3" value="Добавить многопрофильный" @click="addMultiCG()" type="button">

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
                                <colgroup>
                                    <col style="width: 10%;"/>
                                    <col style="width: 20%"/>
                                    <col style="width: 10%"/>
                                    <col style="width: 25%"/>
                                    <col style="width: 10%"/>
                                    <col style="width: 20%"/>
                                </colgroup>
                                <tbody>
                                    <tr>
                                        <td style="text-align: right;">Наименование:</td>&nbsp;
                                        <td>
                                            <input type="text" style = "width: 326px" v-model="filter.CompetitiveGroupName" />
                                        </td>

                                        <td style="text-align: right;">Год начала проведения ПК:</td>&nbsp;
                                        <td>
                                            <select v-model="filter.CampaignYearStart">
                                                <option v-bind:value="null">[По всем годам обучения]</option>
                                                <option v-for="option in сampaignStartYears" v-bind:value="option.ID">
                                                    {{ option.Name }}
                                                </option>
                                            </select>
                                        </td>

                                        <td style="text-align: right;">Приемная кампания:</td>&nbsp;
                                        <td>
                                            <select v-model="filter.CampaignTypeName">
                                                <option v-bind:value="null">[По всем приемным кампаниям]</option>
                                                <option v-for="option in сampaignTypes" v-bind:value="option.Name">
                                                    {{ option.Name }}
                                                </option>
                                            </select>
                                        </td>                                                                
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">Уровень образования:</td>&nbsp;
                                        <td>
                                            <select v-model="filter.EducationLevelID">
                                                <option v-bind:value="null">[По всем уровням образования]</option>
                                                <option v-for="option in educationLevels" v-bind:value="option.ID">
                                                    {{ option.Name }}
                                                </option>
                                            </select>
                                        </td>

                                        <td style="text-align: right;">Направление подготовки:</td>&nbsp;
                                        <td>
                                            <input type="text" style = "width: 326px" v-model="filter.DirectionName" />
                                        </td>

                                        <td style="text-align: right;">Источник финансирования:</td>&nbsp;
                                        <td>
                                            <select v-model="filter.EducationSourceID">
                                                <option v-bind:value="null">[По всем источникам финансирования]</option>
                                                <option v-for="option in educationFinanceSources" v-bind:value="option.ID">
                                                    {{ option.Name }}
                                                </option>
                                            </select>
                                        </td>                                                               
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">Форма обучения:</td>&nbsp;
                                        <td>
                                            <select v-model="filter.EducationFormID">
                                                <option v-bind:value="null">[По всем формам обучения]</option>
                                                <option v-for="option in educationForms" v-bind:value="option.ID">
                                                    {{ option.Name }}
                                                </option>
                                            </select>
                                        </td>
                                
                                        <td style="text-align: right;">Дополнительный набор:</td>&nbsp;
                                        <td>
                                            <select v-model="filter.IsAdditionalName">
                                                <option v-bind:value="null">[Не важно]</option>
                                                <option v-for="option in isAdditionalCollection" v-bind:value="option.Name">
                                                    {{ option.Name }}
                                                </option>
                                            </select>
                                        </td>

                                        <td style="text-align: right;">Уровень бюджета:</td>&nbsp;
                                        <td>
                                            <select v-model="filter.IdLevelBudget">
                                                <option v-bind:value="null">[По всем уровням бюджета]</option>
                                                <option v-for="option in levelBudgets" v-bind:value="option.IdLevelBudget">
                                                    {{ option.BudgetName }}
                                                </option>
                                            </select>
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
            <%--грид--%>
            <table class="gvuzDataGrid tableStatement2">
                <thead>
                    <tr>
                        <th style="width:3%"></th>
                        <th style="width:20%">
	                        <span class="sortable linkSumulator" @click="sortBy('CompetitiveGroupName')">
                                Наименование
	                        </span>
                            <span v-show="sortKey === 'CompetitiveGroupName' && reverse === 'desc'" class="sortDown"></span>
                            <span v-show="sortKey === 'CompetitiveGroupName' && reverse === 'asc'" class="sortUp"></span>
                        </th>
                        <th style="width:5%">
	                        <span class="sortable linkSumulator" @click="sortBy('CampaignYearStart')">
                                Год ПК
	                        </span>
                            <span v-show="sortKey === 'CampaignYearStart' && reverse === 'desc'" class="sortDown"></span>
                            <span v-show="sortKey === 'CampaignYearStart' && reverse === 'asc'" class="sortUp"></span>
                        </th>
                        <th style="width:12%">
	                        <span class="sortable linkSumulator" @click="sortBy('CampaignName')">
                                Приемная кампания
	                        </span>
                            <span v-show="sortKey === 'CampaignName' && reverse === 'desc'" class="sortDown"></span>
                            <span v-show="sortKey === 'CampaignName' && reverse === 'asc'" class="sortUp"></span>
                        </th>
                        <th style="width:10%">
	                        <span class="sortable linkSumulator" @click="sortBy('EducationLevelName')">
                                Уровень образования
	                        </span>
                            <span v-show="sortKey === 'EducationLevelName' && reverse === 'desc'" class="sortDown"></span>
                            <span v-show="sortKey === 'EducationLevelName' && reverse === 'asc'" class="sortUp"></span>
                        </th>
                        <th style="width:10%">
	                        <span class="sortable linkSumulator" @click="sortBy('DirectionName')">
                                Направление подготовки
	                        </span>
                            <span v-show="sortKey === 'DirectionName' && reverse === 'desc'" class="sortDown"></span>
                            <span v-show="sortKey === 'DirectionName' && reverse === 'asc'" class="sortUp"></span>
                        </th>
                        <th style="width:8%">
	                        <span class="sortable linkSumulator" @click="sortBy('UGSNAME')">
                                УГС
	                        </span>
                            <span v-show="sortKey === 'UGSNAME' && reverse === 'desc'" class="sortDown"></span>
                            <span v-show="sortKey === 'UGSNAME' && reverse === 'asc'" class="sortUp"></span>
                        </th>
                        <th style="width:10%">
	                        <span class="sortable linkSumulator" @click="sortBy('EducationSourceName')">
                                Источник финансирования
	                        </span>
                            <span v-show="sortKey === 'EducationSourceName' && reverse === 'desc'" class="sortDown"></span>
                            <span v-show="sortKey === 'EducationSourceName' && reverse === 'asc'" class="sortUp"></span>
                        </th>
                        <th style="width:10%">
	                        <span class="sortable linkSumulator" @click="sortBy('EducationFormName')">
                                Форма обучения
	                        </span>
                            <span v-show="sortKey === 'EducationFormName' && reverse === 'desc'" class="sortDown"></span>
                            <span v-show="sortKey === 'EducationFormName' && reverse === 'asc'" class="sortUp"></span>
                        </th>
                        <th style="width:5%">
	                        <span class="sortable linkSumulator" @click="sortBy('IsAdditionalName')">
                                Доп. набор
	                        </span>
                            <span v-show="sortKey === 'IsAdditionalName' && reverse === 'desc'" class="sortDown"></span>
                            <span v-show="sortKey === 'IsAdditionalName' && reverse === 'asc'" class="sortUp"></span>
                        </th>
                        <th style="text-align:center; width:11%">
                            Действия
                        </th>
                    </tr>
                </thead>
                <tbody>
				    <tr v-for="(group, index) in filteredCompetitiveGroupList" :class="[index % 2 === 0 ? 'trline1' : 'trline2']">
                        <td><input type="checkbox" v-model="group.checked"/></td>
                        <td v-tooltip.bottom="{ content: group.CompetitiveGroupName, classes: ['tooltip'], delay: { show: 1000, hide: 400 } }">{{group.CompetitiveGroupName}}</td>
                        <td>{{group.CampaignYearStart}}</td>
                        <td v-tooltip.bottom="{ content: group.CampaignName, classes: ['tooltip'], delay: { show: 1000, hide: 400 } }">{{group.CampaignName}}</td>
                        <td>{{group.EducationLevelName}}</td>
                        <td v-tooltip.bottom="{ content: group.DirectionName, classes: ['tooltip'], delay: { show: 1000, hide: 400 } }">{{group.DirectionName}}</td>
                        <td v-tooltip.bottom="{ content: group.UGSNAME, classes: ['tooltip'], delay: { show: 1000, hide: 400 } }">{{group.UGSNAME}}</td>
                        <td>{{group.EducationSourceName}}</td>
                        <td>{{group.EducationFormName}}</td>
                        <td>{{group.IsAdditionalName}}</td>
                        <td style="text-align: center;">
                            <a href="javascript:void(0)" class="btnEdit" @click="doEdit(group)" title="Редактировать"></a>
                            <a href="javascript:void(0)" title="Копировать"  @click="openCopyDialog(group)" class="btnCopy"></a>
                            <a href="javascript:void(0)" class="btnDelete" @click="doDelete(group)" title="Удалить"></a>
                        </td>
				    </tr>
			    </tbody>

                <tfoot>
                    <tr>
                        <th colspan="11">
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
    </div>




    <script type="text/javascript">
        Vue.component('modal', {
            template: modalTemplate
        })
        Vue.component('v-popover', VTooltip.VPopover)
        Vue.directive('tooltip', VTooltip.VTooltip)

        Vue.use(window['vue-easy-toast'].default)
     
        var app = new Vue({
            el: '#CompetitiveGroupList',
            data: function() {
                return {
                    showModal: false,
                    copyEndMode: false,
                    competitiveGroupList: [],
                    //Для копирования
                    checkedCompetitiveGroupList: [],
                    copy_year: null,
                    copy_сampaignType: null,
                    copy_levelBudget: null,
                    copy_error: false,
                    copy_paidCounter: 0,
                    //Справочники
                    сampaignStartYears: [],
                    сampaignTypes: [],
                    educationLevels: [],
                    educationFinanceSources: [],
                    educationForms: [],
                    isAdditionalCollection: [],
                    levelBudget: [],
                    //Сортировка
                    sortKey: null,
                    reverse: 'asc',
                    //Параметры фильтра
                    showFilter: false,
                    //filterButtonText: 'Скрыть фильтр',
                    filter: {
                        CompetitiveGroupName: null,
                        DirectionName: null,
                        ParentDirectionName: null,
                        CampaignYearStart: null,
                        CampaignTypeName: null,
                        EducationLevelID: null,
                        EducationSourceID: null,
                        EducationFormID: null,
                        IsAdditionalName: null,
                        IdLevelBudget: null
                    },
                    //Пейджинг
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
                    this.competitiveGroupList = JSON.parse('<%= Html.Serialize(Model.CompetitiveGroupList) %>');
                    //справочники
                    this.сampaignStartYears = JSON.parse('<%= Html.Serialize(Model.CampaignStartYears) %>');
                    this.сampaignTypes = JSON.parse('<%= Html.Serialize(Model.CampaignTypes) %>');
                    this.educationLevels = JSON.parse('<%= Html.Serialize(Model.EducationLevels) %>');
                    this.educationFinanceSources = JSON.parse('<%= Html.Serialize(Model.EducationFinanceSources) %>');
                    this.educationForms = JSON.parse('<%= Html.Serialize(Model.EducationForms) %>');
                    this.isAdditionalCollection = JSON.parse('<%= Html.Serialize(Model.IsAdditionalCollection.Items) %>');  
                    this.levelBudgets = JSON.parse('<%= Html.Serialize(Model.LevelBudgets) %>');   
                    
                    //load filter
                    var lastFilter = JSON.parse(sessionStorage.getItem('competitiveGroupListFilter')); //JSON.parse(getCookie('competitiveGroupListFilter'));
                    if (lastFilter) {
                        this.filter = lastFilter;
                    }
                },
                addCG: function () {
                    window.location = '<%= Url.Action("CompetitiveGroupEdit") %>';
                },
                addMultiCG: function () {
                    window.location = '<%= Url.Action("MultiProfileCompetitiveGroupEdit") %>';
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
                setPage: function (pageNumber) {
                    this.currentPage = pageNumber;
                },
                ToggleFilter: function () {
                    this.showFilter = !this.showFilter;
                    //this.filterButtonText = this.filterButtonText === 'Скрыть фильтр' ? 'Отобразить фильтр' : 'Скрыть фильтр';
                },
                ClearFilter: function () {
                    $.each(app.filter, function (key, value) {
                        app.filter[key] = null;
                    });
                    sessionStorage.removeItem('competitiveGroupListFilter');
                },
                doEdit: function (group) {
                    //save last page
                    //if (me.pager.currentPageIndex()) {
                    //    setCookie('CompetitiveGroupListPage', JSON.stringify(me.pager.currentPageIndex()), 1)
                    //}
                    //console.log(group.CompetitiveGroupID);
                    if (group.UGSNAME == null) {
                        window.location = '<%= Url.Generate<CompetitiveGroupController>(x => x.CompetitiveGroupEdit(null)) %>?competitiveGroupID=' + group.CompetitiveGroupID;
                    }
                    else {
                        window.location = '<%= Url.Generate<CompetitiveGroupController>(x => x.MultiProfileCompetitiveGroupEdit(null)) %>?competitiveGroupID=' + group.CompetitiveGroupID;
                    }
	            },
                doDelete: function (group) {
                    confirmDialog("Вы действительно хотите удалить конкурс?", function () {
                        doPostAjax('<%= Url.Generate<CompetitiveGroupController>(x => x.CompetitiveGroupDelete(null)) %>?competitiveGroupID=' + group.CompetitiveGroupID, '', function (data) {
                            if (!data.success)
                                _errorToast('При удалении конкурса ' + group.CompetitiveGroupName + ' произошла ошибка! <br/><br/>' + data.errors[0]);
                            else {
                                var index = app.competitiveGroupList.indexOf(group);
                                app.competitiveGroupList.splice(index, 1);

                                _successToast('Конкурс ' + group.CompetitiveGroupName + ' удален успешно!');
                                }
                            }
                        );
                    });
                },
                openCopyDialog: function (group) {
                    app.checkedCompetitiveGroupList = [];
                    app.copyEndMode = false;
                    if (group) {
                        app.checkedCompetitiveGroupList.push(group);
                    }
                    if (!group) {
                        for (var i = 0; i < app.competitiveGroupList.length; i++) {
                            var item = app.competitiveGroupList[i];
                            if (item.checked) {
                                app.checkedCompetitiveGroupList.push(item);
                                item.checked = false;
                            }
                        }
                    }
                    if (app.checkedCompetitiveGroupList.length > 0) {
                        for (var i = 0; i < app.checkedCompetitiveGroupList.length; i++) {
                            if (app.checkedCompetitiveGroupList[i].EducationSourceID == 15) {
                                app.copy_paidCounter++;
                            }
                        }
                        app.showModal = true;
                    }
                    else {
                        _errorToast('Не выбрано ни одного конкурса!');
                    }
                },
                CancelCopy: function (group) {
                    var index = app.checkedCompetitiveGroupList.indexOf(group);
                    app.checkedCompetitiveGroupList.splice(index, 1);
                },
                closeCopyDialog: function () {
                    app.copy_year = null;
                    app.copy_сampaignType = null;
                    app.copy_levelBudget = null;
                    app.copy_error = false;
                    app.copy_paidCounter = 0;

                    app.showModal = false;
                },
                CopyGroups: function () {
                    if (!app.copy_year || !app.copy_сampaignType) {
                        app.copy_error = true;
                        return;
                    }

                    //for (var i = 0; i < app.checkedCompetitiveGroupList.length; i++) {
                    //    if (app.checkedCompetitiveGroupList[i].EducationSourceID == 15) {
                    //        app.copy_paidCounter++;
                    //    }
                    //}
                    if (!app.copy_levelBudget && app.copy_сampaignType != 5 && app.copy_paidCounter != app.checkedCompetitiveGroupList.length) {
                        app.copy_error = true;
                        return;
                    }

                    var competitiveGroupIDs = [];
                    for (var i = 0; i < app.checkedCompetitiveGroupList.length; i++) {
                        competitiveGroupIDs.push(app.checkedCompetitiveGroupList[i].CompetitiveGroupID);
                    }

                    var data = {};
                    data.competitiveGroupIDs = competitiveGroupIDs;
                    data.copy_year = app.copy_year;
                    data.copy_сampaignType = app.copy_сampaignType;
                    data.copy_levelBudget = app.copy_levelBudget;

                    doPostAjax('<%= Url.Action("CompetitiveGroupCopy", "CompetitiveGroup") %>', JSON.stringify(data), function (res) {
                            if (res) {
                                app.copyEndMode = true;
                                if (app.checkedCompetitiveGroupList.length === 1 && res.length === 1 && res[0].Message == 'Конкурс скопирован успешно.') {
                                    app.closeCopyDialog();
                                    window.location = '<%= Url.Generate<CompetitiveGroupController>(x => x.CompetitiveGroupEdit(null)) %>?competitiveGroupID=' + res[0].NewCompetitiveGroupID;
                                }
                                else {
                                    for (var i = 0; i < app.checkedCompetitiveGroupList.length; i++) {
                                        for (var j = 0; j < res.length; j++) {
                                            if (app.checkedCompetitiveGroupList[i].CompetitiveGroupID == res[j].CompetitiveGroupID) {
                                                app.checkedCompetitiveGroupList[i].Message = res[j].Message;
                                            }
                                        }
                                    }

                                    doPostAjax('<%= Url.Generate<CompetitiveGroupController>(x => x.GetCompetitiveGroups()) %>', '', function (data)
		                            {
			                            if (!addValidationErrorsFromServerResponse(data, false))
			                            {
			                                app.competitiveGroupList = data;
			                            }
                                    })
                                }
                            }
                        }
                    );
                }
            },
            computed: {
                filteredCompetitiveGroupList: function () {
                    var results = this.competitiveGroupList;                                 

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

    <div id="divPopupDialog"></div>
    <div id="divBenefitListDialog"></div>
    <div id="divAddBenefit"></div>
    <div id="divViewOlympic" style="padding: 5px; display: none; position: absolute" class="ui-widget ui-widget-content ui-corner-all"></div>
    <div id="divAddOlympicMultiple"></div>
</asp:Content>
