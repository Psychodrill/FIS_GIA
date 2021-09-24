<%@ Page Title="Приемные кампании" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.DAL.Dapper.ViewModel.Campaign.CampaignViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Security" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Register TagPrefix="gv" TagName="AdminMenuControl" Src="~/Views/Shared/Controls/AdminMenuControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Приемные кампании
</asp:Content>
<asp:Content ContentPlaceHolderID="PageTitle" runat="server">
	Приемные кампании
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="divstatement" id="campaignList" v-cloak>
	<% ViewData["MenuItemID"] = 1; %>
	<gv:AdminMenuControl ID="AdminMenuControl1" runat="server" />	
	
	<div id="tabControl" class="submenu"></div>
	<div>&nbsp;</div>
	<div id="content">
	
	<%--<% if (!GVUZ.Helper.UrlUtils.IsReadOnly(FBDUserSubroles.CampaignDirection))
    { %>--%>
	<div id="createButton">
		<%= Url.GenerateNavLink<CampaignController>(c => c.CampaignEdit(null), "Создать новую", UserRole.AdminUsrEduRole)%>
	</div> 
	<%--<% } %>--%>

		<table class="gvuzDataGrid tableStatement2" cellpadding="3">
            <thead>
                <tr>
                    <th style="width:20%">
	                    <span class="linkSumulator sortable" @click="sortBy('CampaignName')">
                            Наименование
	                    </span>
                        <span v-show="sortKey === 'CampaignName' && reverse === 'desc'" class="sortDown"></span>
                        <span v-show="sortKey === 'CampaignName' && reverse === 'asc'" class="sortUp"></span>
                    </th>
                    <th style="width:20%">
	                    <span class="linkSumulator sortable" @click="sortBy('CampaignTypeName')">
                            Тип приемной кампании
	                    </span>
                        <span v-show="sortKey === 'CampaignTypeName' && reverse === 'desc'" class="sortDown"></span>
                        <span v-show="sortKey === 'CampaignTypeName' && reverse === 'asc'" class="sortUp"></span>
                    </th>
                    <th style="width:10%">
	                    <span class="linkSumulator sortable" @click="sortBy('YearStartToEnd')">
                            Сроки проведения
	                    </span>
                        <span v-show="sortKey === 'YearStartToEnd' && reverse === 'desc'" class="sortDown"></span>
                        <span v-show="sortKey === 'YearStartToEnd' && reverse === 'asc'" class="sortUp"></span>
                    </th>
                    <th style="width:30%">
	                    <span class="linkSumulator sortable" @click="sortBy('LevelsEducation')">
                            Уровни образования
	                    </span>
                        <span v-show="sortKey === 'LevelsEducation' && reverse === 'desc'" class="sortDown"></span>
                        <span v-show="sortKey === 'LevelsEducation' && reverse === 'asc'" class="sortUp"></span>
                    </th>
                    <th style="width:10%">
	                    <span class="linkSumulator sortable" @click="sortBy('CampaignStatusName')">
                            Статус
	                    </span>
                        <span v-show="sortKey === 'CampaignStatusName' && reverse === 'desc'" class="sortDown"></span>
                        <span v-show="sortKey === 'CampaignStatusName' && reverse === 'asc'" class="sortUp"></span>
                    </th>
                    <th style="width:7%">
                        Действия
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr class="trline2">
                    <td><input class="new-search-field" placeholder="Наименование" v-model="filter.CampaignName" type="text"/></td>
                    <td><input class="new-search-field" placeholder="Тип приемной кампании" v-model="filter.CampaignTypeName" type="text"/></td>
                    <td style="display: inline-block;">
                        <input style="width: 30%" class="new-search-field" placeholder="Год" v-model="filter.YearStart" type="text"/>
                        &mdash;
                        <input style="width: 30%" class="new-search-field" placeholder="Год" v-model="filter.YearEnd" type="text"/>
                    </td>
                    <td><input class="new-search-field" placeholder="Уровни образования" v-model="filter.LevelsEducation" type="text"/></td>
                    <td><input class="new-search-field" placeholder="Статус" v-model="filter.CampaignStatusName" type="text"/></td>
                    <td style="text-align: center"><a href="javascript:void(0)" @click="ClearFilter()">Очистить</a></td>
                </tr>
				<tr v-for="(campaign, index) in filteredCampaignList" :class="[index % 2 === 0 ? 'trline1' : 'trline2']">
                    <td><a @click="doEdit(campaign)" href="javascript:void(0)">{{campaign.CampaignName}}</a></td>
                    <td>{{campaign.CampaignTypeName}}</td>
                    <td>{{campaign.YearStartToEnd}}</td>
                    <td>{{campaign.LevelsEducation}}</td>
                    <td>{{campaign.CampaignStatusName}}</td>
                    <%--<td>
                        <a href="#" @click="doSwitchStatus(campaign);return false;">{{campaign.StatusID == 1 ? 'Завершить' : 'Открыть набор'}}</a>
                    </td>--%>
                    <td style="text-align: center;">
                        <a v-if="campaign.StatusID == 1" href="javascript:void(0)" class="btnRevoke" title="Завершить" @click="doSwitchStatus(campaign)"></a>
                        <a v-if="campaign.StatusID == 1" href="javascript:void(0)" class="btnEdit" @click="doEdit(campaign)" title="Редактировать"></a>
			            <a v-if="campaign.StatusID == 1" href="javascript:void(0)" class="btnDelete" @click="doDelete(campaign)" title="Удалить"></a>

                        <a v-if="campaign.StatusID != 1" href="javascript:void(0)" class="btnOpen" title="Открыть набор" @click="doSwitchStatus(campaign)"/>
                        <a v-if="campaign.StatusID != 1" href="javascript:void(0)" class="btnEditGray"/>
			            <a v-if="campaign.StatusID != 1" href="javascript:void(0)" class="btnDeleteGray"/>
                    </td>
				</tr>
			</tbody>

            <tfoot>
                <tr>
                    <th colspan="10">
                        <div style="text-align: center;white-space: nowrap">
                            <span class="pageLink">Записей:&nbsp;{{resultCount}}&emsp;Страниц:&nbsp;{{totalPages}}</span>
                            <a href="javascript:void(0)" class="pageLink pageLinkArrowLeftLeft" @click="setPage(0)">&nbsp;</a>
                            <span v-for="pageNumber in totalPages" 
                                    v-if="Math.abs(pageNumber - currentPage - 1) < 3">
                                <a href="javascript:void(0)" :class="{pageLinkActive: currentPage === pageNumber - 1}" class="pageLink" @click="setPage(pageNumber-1)">{{ pageNumber }}</a>
                            </span>
                            <a href="javascript:void(0)" class="pageLink pageLinkArrowRightRight" @click="setPage(totalPages-1)">&nbsp;</a>
                            <span class="pageLink" style="display: inline;">
                                На странице:&nbsp;
                                <select style="width: 55px;" v-model="itemsPerPage" @change="setPage(0)">
                                    <option>10</option>
                                    <option>30</option>
                                    <option>50</option>
                                    <option>100</option>
                                    <option>200</option>
                                    <option>500</option>
                                </select>
                            </span>
                        </div>
                    </th>
                </tr>
            </tfoot>
		</table>
	</div>
</div>
<script language="javascript" type="text/javascript">
    Vue.use(window['vue-easy-toast'].default)

    var app = new Vue({
        el: '#campaignList',
        data: function() {
            return {
                campaignList: [],
                //userReadonly: null,
                sortKey: null,
                reverse: 'desc',
                //Параметры фильтра 
                filter: {
                    CampaignName: null,
                    CampaignTypeName: null,
                    YearStart: null,
                    YearEnd: null,
                    LevelsEducation: null,
                    CampaignStatusName: null
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
            loadData: function ()
	        {
                clearValidationErrors(jQuery('#content'));
		        pageNumber = 0;

		        doPostAjax('<%= Url.Generate<CampaignController>(x => x.GetCampaignList(null)) %>', '', function (data)
		        {
                    //app.userReadonly = <%= GVUZ.Helper.UrlUtils.IsReadOnly(FBDUserSubroles.CampaignDirection) ? "true" : "false" %>;
			        if (!addValidationErrorsFromServerResponse(data, false))
			        {
			            app.campaignList = data.Data.CampaignList;
			            app.resultCount = app.campaignList.length;
			            app.sortBy('YearStartToEnd');
			        }
		        })
            },
            doSwitchStatus: function (campaign) {
                doPostAjax('<%= Url.Generate<CampaignController>(x => x.CampaignSwitchStatus(null)) %>?campaignID=' + campaign.CampaignID, '', function (data) {
				    if(data.IsError)
					    alert(data.Message);
				    else
				    {
				        var index = app.campaignList.indexOf(campaign);
				        Vue.set(app.campaignList, index, data.Data);
				    }
			    })
            },
            doEdit : function (campaign) {
                window.location = '<%= Url.Generate<CampaignController>(x => x.CampaignEdit(null)) %>?campaignID=' + campaign.CampaignID;
	        },
            doDelete: function (campaign) {
                confirmDialog(('Вы действительно хотите удалить кампанию ' + campaign.CampaignName + '?'), function () {
                    doPostAjax('<%= Url.Generate<CampaignController>(x => x.CampaignDelete(null)) %>?campaignID=' + campaign.CampaignID, '', function (data) {
                        if (data.IsError) {
                                _errorToast('При удалении кампании ' + campaign.CampaignName + ' произошла ошибка! <br/><br/>' + data.Message);
                            }
	                        else {
	                            var index = app.campaignList.indexOf(campaign);
	                            app.campaignList.splice(index, 1);
	                            
	                            _successToast('Кампания ' + campaign.CampaignName + ' удалена успешно!');
	                        }
	                    }
	                );
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
            setPage: function(pageNumber) {
                this.currentPage = pageNumber;
            },
            ClearFilter: function() {
                $.each(app.filter, function (key, value) {
                    app.filter[key] = null;
                });
            }
        },
        computed: {
            filteredCampaignList: function () {                
                var results = this.campaignList;

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
        }
    })


</script>

</asp:Content>
