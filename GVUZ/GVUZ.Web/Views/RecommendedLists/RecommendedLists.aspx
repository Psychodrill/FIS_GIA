<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.  ListOfRecommendedListViewModel>" %>
<%@ Import Namespace="GVUZ.Web.ViewModels" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.Security" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Списки рекомендованных к зачислению
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageTitle" runat="server">
    Заявления
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .filterElement
        {
            float: left;
            width: 400px;
        }
        .clearFilterElement
        {
            clear: both;
        }
    </style>
    <div class="divstatement">
        <div id="tabControl" class="submenu"></div>
        <div>&nbsp;</div>
        <div class="content">
            <div id="filter_always_visible">
                <div class="filterElement">
                    <span class="labelsInside">Приёмная кампания:</span>
                    <%= Html.DropDownListExFor(x => x.FilterValues.CampaignId, Model.FilterData.Campaigns, new { @class = "ss", @id="campaignFilter", @style="width: 200px", @onchange="visibleFilterChanged()" })%>
                </div>
                <div class="filterElement">
                    <span class="labelsInside">Этап зачисления:</span>
                    <%= Html.DropDownListExFor(x => x.FilterValues.Stage, Model.FilterData.Stages, new { @class = "ss", @onchange = "visibleFilterChanged()" })%>
                </div>
                <div class="filterElement">
                    <input type="button" id="downloadListButton" value="Выгрузить список" onclick="doListDownload(); return false;" />
                </div>
                <div class="clearFilterElement"></div>
            </div>
            <div></div>
            <div class="tableHeader5l tableHeaderCollapsed">	
            <div id="divFilterPlace">
                <div class="hideTable" onclick=" toggleFilter() " style="float: left"><span id="btnShowFilter">Отобразить фильтр</span> </div>
                <div id="spAppCount" class="appCount">Количество элементов: <span id="spAppCountField"></span></div>

                <div style="display: none; text-align: right;" id="filterInfo">        
                    <span style="color: salmon; font-size: 11px; font-weight: bold; padding-right: 5px;">[Внимание! Применен фильтр]</span>
                </div>
            </div>
                <div id="filter" style="clear: both; display: none;">
                    <table class="tableForm">
                        <tr>
                            <td class="labelInside">Номер заявления:</td>
                            <td><input id="appNumberInput" type="text" /></td>
                            
                            <td class="labelInside">Фамилия:</td>
                            <td><input id="lastNameInput" type="text" /></td>

                            <td class="labelInside">Форма обучения:</td>
                            <td><%= Html.DropDownListExFor(x => x.FilterValues.EduForm, Model.FilterData.EduForms, new { @class = "ss", @id = "eduFormInput" }) %></td>
                        </tr>
                        <tr>
                            <td class="labelInside">Сдал документы:</td>
                            <td><%= Html.DropDownListExFor(x => x.FilterValues.OriginalsReceived, Model.FilterData.OrigsReceived, new { @class = "ss", @id = "origsInput" }) %></td>

                            <td class="labelInside">Имя:</td>
                            <td><input id="firstNameInput" type="text" /></td>

                            <td class="labelInside">Конкурс:</td>
                            <td><%= Html.DropDownListExFor(x => x.FilterValues.CompetitiveGroup, Model.FilterData.CompetitiveGroups, new { @class = "ss", @id = "cgsInput" }) %></td>
                        </tr>
                        <tr>
                            <td class="labelInside">Уровень образования:</td>
                            <td><%= Html.DropDownListExFor(x => x.FilterValues.Edulevel, Model.FilterData.EduLevels, new { @class = "ss", @id = "eduLevelInput" }) %></td>

                            <td class="labelInside">Отчество:</td>
                            <td><input id="middleNameInput" type="text" /></td>

                            <td class="labelInside">Направление подготовки:</td>
                            <td><%= Html.DropDownListExFor(x => x.FilterValues.Direction, Model.FilterData.Directions, new { @class = "ss", @id = "directionInput" }) %></td>
                        </tr>
                        <tr>
                            <td colspan="9" style="text-align: center;">
                                <input type="submit" id="btnApplyFilter" class="button primary" onclick=" applyFilter() " value="Найти" />
                                <input type="button" id="btnClearFilter" class="button" onclick=" clearFilter() " value="Сбросить фильтр" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div id="data" style="clear: both;">
                <table class="gvuzDataGrid tableStatement2">
                    <thead>
                        <tr>
                            <th><span class="linkSumulator" onclick="doSort(this, 1)"><%= Html.TableLabelFor(x => x.ListHeader.CampaignName, false, false) %></span></th>
                            <th><span class="linkSumulator" onclick="doSort(this, 2)"><%= Html.TableLabelFor(x => x.ListHeader.Stage, false, false) %></span></th>
                            <th><span class="linkSumulator" onclick="doSort(this, 3)"><%= Html.TableLabelFor(x => x.ListHeader.ApplicationNumber, false, false) %></span></th>
                            <th><span class="linkSumulator" onclick="doSort(this, 4)"><%= Html.TableLabelFor(x => x.ListHeader.EntrantName, false, false) %></span></th>
                            <th><span class="linkSumulator" onclick="doSort(this, 5)"><%= Html.TableLabelFor(x => x.ListHeader.EduLevel, false, false) %></span></th>
                            <th><span class="linkSumulator" onclick="doSort(this, 6)"><%= Html.TableLabelFor(x => x.ListHeader.EduForm, false, false) %></span></th>
                            <th><span class="linkSumulator" onclick="doSort(this, 7)"><%= Html.TableLabelFor(x => x.ListHeader.CompetitiveGroupName, false, false) %></span></th>
                            <th><span class="linkSumulator" onclick="doSort(this, 8)"><%= Html.TableLabelFor(x => x.ListHeader.DirectionName, false, false) %></span></th>
                            <th><span class="linkSumulator" onclick="doSort(this, 9)"><%= Html.TableLabelFor(x => x.ListHeader.OriginalsReceived, false, false) %></span></th>
                            <th><span class="linkSumulator" onclick="doSort(this, 10)"><%= Html.TableLabelFor(x => x.ListHeader.Rating, false, false) %></span></th>
                            <th>Действие</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr id="trBefore"></tr>
                    </tbody>
                </table>
            </div>
        </div>
        <div id="divIncludeInOrder"></div>
    </div>


<script type="text/javascript">

     var currentTab = 6;
     var currentSorting = 1;
     var currentPage = 0;

    // --------------------------------- Действия при загрузке страницы -----------------------------------
    jQuery(document).ready(function () {
        updateData();
    })

    function updateData() {
        new TabControl(jQuery('#tabControl'), [
                    { name: 'Новые', link: 'javascript:switchTab(1)', enable: true, selected: currentTab == 1, noWrap: true },
                    { name: 'Не прошедшие проверку', link: 'javascript:switchTab(2)', selected: currentTab == 2, enable: true },
                    { name: 'Отозванные', link: 'javascript:switchTab(3)', selected: currentTab == 3, noWrap: true, enable: true },
                    { name: 'Принятые', link: 'javascript:switchTab(4)', enable: true, selected: currentTab == 4, noWrap: true },
                    { name: 'Рекомендованные к зачислению', link: 'javascript:switchTab(6)', enable: true, selected: currentTab == 6, noWrap: true },
                    { name: 'Включенные в приказ', link: 'javascript:switchTab(5)', enable: true, noWrap: false }
                ]
            ).init();

        fillGrid(JSON.parse('<%= Html.Serialize(Model.RecommendedLists) %>'));
        fillPager(<%=Model.PageCount %>, currentPage);
    }
    // --------------------------------- Действия при загрузке страницы -----------------------------------

    // --------------------------------- Отображение данных -----------------------------------------------
    function fillGrid(data) {
        jQuery('.gvuzDataGrid tbody tr:not(#trBefore)').remove().detach();
         var trBefore = jQuery("#trBefore");
        var className = trBefore.prev().attr('class');
        if (className == 'trline2') className = 'trline1';
        else className = 'trline2';

        for (var i = 0; i < data.length; i++) {
            var buttons = '<a href="#" style="margin-right: 10px;" appId="' + data[i].ApplicationId.toString() + '" recListId="' + data[i].RecListId.toString() + '" class="btnOk' + (data[i].ApplicationStatus == 8 || data[i].IsCampaignFinished ? 'Gray" onclick="return false"' : '" onClick="doIncludeInOrder(this);return false;" title="Включить в приказ"') + '"></a>';
            buttons += '<a href="#" recListId="' + data[i].RecListId.toString() + '" class="btnDelete' + (data[i].ApplicationStatus == 8 || data[i].IsCampaignFinished ? 'Gray" onclick="return false"' : '" onClick="doDelete(this);return false;" title="Удалить"') + '></a>'
            var tr = '<td>' + data[i].CampaignName + '</td>';
            tr += '<td>Этап' + data[i].Stage.toString() + '</td>';
            tr += '<td>' + data[i].ApplicationNumber + '</td>';
            tr += '<td>' + data[i].EntrantName + '</td>';
            tr += '<td>' + data[i].EduLevel + '</td>';
            tr += '<td>' + data[i].EduForm + '</td>';
            tr += '<td>' + data[i].CompetitiveGroupName + '</td>';
            tr += '<td>' + data[i].DirectionName + '</td>';
            tr += '<td>' + data[i].OriginalsReceived + '</td>';
            tr += '<td>' + data[i].Rating.toString() + '</td>';
            tr += '<td style="text-align: center;" id="buttons" dataId="' + data[i].RecListId.toString() + '">' + buttons + '</td>';

            if (className == 'trline2') 
                className = 'trline1';
            else className = 'trline2';

            tr = '<tr class="' + className + '">' + tr + '</tr>';

            trBefore.before(tr);
        }
    }

    function switchTab(tabID) {
        currentTab = tabID;
        if (tabID == 5) {
            window.location = '<%= Url.Generate<InstitutionApplicationController>(x => x.IncludeInOrderList()) %>';
            return;
        }

        if (tabID == 6) //Для списков рекомендованных - отдельная обработка.
        {
            window.location = '<%= Url.Generate<RecommendedListsController>(x => x.GetRecommendedList()) %>';
            return;
        }

        window.location = '<%= Url.Generate<InstitutionApplicationController>(x => x.ApplicationList()) %>#tab' + tabID;
    }

    function movePager(pageNumber) {
        currentPage = pageNumber;
        doPostAjax('<%= Url.Generate<RecommendedListsController>(x => x.SortRecommendedList(null)) %>', JSON.stringify(fillShowDataModel(currentSorting, currentPage)),
        function (data) {
            if (data.Data == null)
                return false;
            fillGrid(data.Data.RecommendedLists);
            fillPager(<%=Model.PageCount %>, currentPage);
        });
    }
    // --------------------------------- Отображение данных -----------------------------------------------

    // ----------------------------------------- Сортировка -------------------------------------------------------
    function doSort(el, dir) {
        currentPage = 0;
        var isSortedUp = jQuery(el).hasClass('sortedUp');
        jQuery('.sortUp,.sortDown').remove().detach();
        if (isSortedUp)
            jQuery(el).after('<span class="sortDown"></span>');
        else
            jQuery(el).after('<span class="sortUp"></span>');
        jQuery(el).removeClass('sortedUp');
        if (isSortedUp)
            dir = -dir;
        else
            jQuery(el).addClass('sortedUp');
        currentSorting = dir;

        doPostAjax('<%= Url.Generate<RecommendedListsController>(x => x.SortRecommendedList(null)) %>', JSON.stringify(fillShowDataModel(currentSorting, currentPage)),
            function (data) {
                if (data.Data == null)
                    return false;
                fillGrid(data.Data.RecommendedLists);
                fillPager(<%=Model.PageCount %>, currentPage);
            });
        }

    // ------------------------------------------------------------------------------------------------------------

    // ----------------------------------------- Фильтрация -------------------------------------------------------
    function fillShowDataModel(sortDirection, currentPage) {
        var model = {
            SortDirection: sortDirection,
            PageToShow: currentPage,
            Filter: buildFilter()
        }

        return model;
    }

    function buildFilter()
    {
        var filterData = 
        {
            CampaignId: jQuery('#campaignFilter').val(),
            Stage: jQuery('#FilterValues_Stage').val(),
            EduLevel: jQuery('#eduLevelInput').val(),
            EduForm: jQuery('#eduFormInput').val(),
            OriginalsReceived: jQuery('#origsInput').val(),
            CompetitiveGroup: jQuery('#cgsInput').val(),
            Direction: jQuery('#directionInput').val(),
            ApplicationNumber: jQuery('#appNumberInput').val(),
            LastName: jQuery('#lastNameInput').val(),
            FirstName: jQuery('#firstNameInput').val(),
            MiddleName: jQuery('#middleNameInput').val()
        }

        return filterData;
    }

    function visibleFilterChanged()
    {
        currentPage = 0;
        doPostAjax('<%= Url.Generate<RecommendedListsController>(x => x.FilterRecommendedList(null)) %>', JSON.stringify(fillShowDataModel(currentSorting, currentPage)),
        function(data)
        {
                if (data.Data == null)
                    return false;
                fillGrid(data.Data.RecommendedLists);
                fillPager(<%=Model.PageCount %>, currentPage);
        });
    }
        
    function toggleFilter() {
        if (jQuery('#btnShowFilter').hasClass('filterDisplayed')) {
            jQuery('#btnShowFilter').removeClass('filterDisplayed');
            jQuery('#btnShowFilter').html('Отобразить фильтр');
            jQuery('#btnShowFilter').parent().removeClass('nonHideTable');
            jQuery('#btnShowFilter').parent().parent().parent().addClass('tableHeaderCollapsed');
            jQuery('#filter').hide();
        } else {
            jQuery('#btnShowFilter').addClass('filterDisplayed');
            jQuery('#btnShowFilter').html('Скрыть фильтр');
            jQuery('#btnShowFilter').parent().addClass('nonHideTable');
            jQuery('#btnShowFilter').parent().parent().parent().removeClass('tableHeaderCollapsed');
            jQuery('#filter').show();
        }
    }

    function applyFilter()
    {
        currentPage = 0;
        doPostAjax('<%= Url.Generate<RecommendedListsController>(x => x.FilterRecommendedList(null)) %>', JSON.stringify(fillShowDataModel(currentSorting, currentPage)),
        function(data)
        {
                if (data.Data == null)
                    return false;
                fillGrid(data.Data.RecommendedLists);
                fillPager(<%=Model.PageCount %>, currentPage);
        });
    }

    function clearFilter()
    {
        currentPage = 0;
        jQuery('#eduLevelInput').val(-1),
        jQuery('#eduFormInput').val(-1),
        jQuery('#origsInput').val(-1),
        jQuery('#cgsInput').val(-1),
        jQuery('#directionInput').val(-1),
        jQuery('#appNumberInput').val(''),
        jQuery('#lastNameInput').val(''),
        jQuery('#firstNameInput').val(''),
        jQuery('#middleNameInput').val('')


        doPostAjax('<%= Url.Generate<RecommendedListsController>(x => x.FilterRecommendedList(null)) %>', JSON.stringify(fillShowDataModel(currentSorting, currentPage)),
        function(data)
        {
                if (data.Data == null)
                    return false;
                fillGrid(data.Data.RecommendedLists);
                fillPager(<%=Model.PageCount %>, currentPage);
        });
    }
// ----------------------------------------- Фильтрация -------------------------------------------------------

// ----------------------------------------- Действия ---------------------------------------------------------
    function doIncludeInOrder(el)
    {
        var appId = jQuery(el).attr('appId');
        var recListId = jQuery(el).attr('recListId');

        includeInOrder(appId, recListId, 0);
    }

    function includeInOrderCloseDialog(isOk) {
        closeDialog(jQuery('#divIncludeInOrder'));
        if (isOk)
            applyFilter();
//            includeInOrderSelectedRow.remove().detach();
    }

    // Включение заявления в приказ 

    function includeInOrder(appID, recListid, index) {
        var btnInclude = true;
        doPostAjax('<%= Url.Generate<ApplicationController>(c => c.IncludeInOrderFromRecListPage(null, null)) %>', "applicationID=" + appID + "&recListId=" + recListid, function(data) {
            jQuery('#divIncludeInOrder').html(data);
            if (data.lastIndexOf('В приказ можно включить только заявления, для которых предоставлены оригиналы документов', 0) >= 0)
                btnInclude = false;
            if (btnInclude)
                jQuery('#divIncludeInOrder').dialog({
                    modal: true,
                    width: 800,
                    title: 'Включение в приказ',
                    buttons:
                    {
                        "Включить в приказ": function() { jQuery('#btnOrderInclude').click(); },
                        "Отмена": function() { jQuery('#btnOrderCancel').click(); }
                    }
                }).dialog('open');
            else
                jQuery('#divIncludeInOrder').dialog({
                    modal: true,
                    width: 800,
                    title: 'Включение в приказ',
                    buttons:
                    {
                        "Отмена": function() { jQuery('#btnOrderCancel').click(); }
                    }
                }).dialog('open');

        }, "application/x-www-form-urlencoded", "html");
    }

    function doDelete(el)
    {
        var listId = jQuery(el).attr('recListId');
        var deleteData = { recListId: listId };
        doPostAjax('<%= Url.Generate<RecommendedListsController>(x => x.DeleteListElement(null)) %>', JSON.stringify(deleteData),
            function(data)
            {
                if (data.Message != null)
                {
                    alert(data.Message);
                    return;
                }

                applyFilter();
            }
        );
    }

    function doListDownload()
    {
        doPostAjax('<%= Url.Generate<RecommendedListsController>(x => x.DownloadRecommendedList(null)) %>', JSON.stringify(buildFilter()),
            function(data)
            {
                location.href = '<%= Url.Generate<RecommendedListsController>(x => x.GetList(null)) %>?filePath=' + encodeURI(data.Data);
            }
        );
    }
// ----------------------------------------- Действия ---------------------------------------------------------
</script>
</asp:Content>


