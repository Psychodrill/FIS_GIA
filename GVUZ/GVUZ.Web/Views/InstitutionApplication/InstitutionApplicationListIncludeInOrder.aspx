<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<InstitutionApplicationListIncludeInOrderViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.ViewModels" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Включение в приказ
</asp:Content>
<asp:Content ContentPlaceHolderID="PageTitle" runat="server">
    Включение в приказ
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="divstatement">
        <div class="content">
            <div id="tabControl" class="submenu"></div>
            <div>&nbsp;</div>
            <div id="divOrders">
                <% Html.RenderPartial("InstitutionApplication/IncludeInOrderList", Model.Order); %>
            </div>
            <div id="divOrderEdit" style="display: none; padding-top: 5px;">
                <%= Html.LabelFor(m => m.OrderUID) %>:
                <% if (!UrlUtils.IsReadOnly(FBDUserSubroles.OrderDirection))
                   { %> <%= Html.TextBoxExFor(m => m.OrderUID, new {@class = "w100px view", @readonly = "readonly"}) %> 
                <% }
                   else
                   { %>
                    <%= Html.TextBoxExFor(m => m.OrderUID, new {@class = "w100px"}) %>&nbsp;<a href="#" id="saveUid" title="Сохранить UID" class="btnSave" style="vertical-align: middle"></a>
                <% } %>
                <span style="display: inline-block; width: 20px">&nbsp;</span>

                <% if (!UrlUtils.IsReadOnly(FBDUserSubroles.OrderDirection))
                   { %>
                    <input type="button" value="Редактировать" class="button3" id="btnOrderEdit" />
                    <input type="button" value="Опубликовать" class="button3" id="btnOrderPublish" />
                <% } %>
                <%--<input type="button" value="Выгрузить список" class="button3" id="btnLoadApplicationList" style="margin-top: 1px"/>--%>

            </div>
            <div style="clear: both; height: 10px;"></div>
            <div id="divFilterBlock" style="width: 100%">
                <div style="float: left" id="divCompGroups"><span style="font-size: 120%">Конкурс: <span id="compGroupFilterName">Все группы</span></span></div>
                <div style="clear: both; height: 1px;"></div>

                <div class="tableHeader5l tableHeaderCollapsed">
                    <div id="divFilterPlace">
                        <div class="hideTable" onclick=" toggleFilter() " style="float: left"><span id="btnShowFilter">Отобразить фильтр</span></div>
                        <div id="spAppCount" class="appCount">Количество заявлений: <span id="spAppCountField"></span></div>
                    </div>
                    <br/>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <div id="divFilter">
                   
                        <div><span style="display: none; font-size: 120%;">Фильтр по списку заявлений</span> 
                            <table class="tableForm">
                                <tbody>
                                    <tr>
                                        <td><%= Html.TableLabelFor(x => x.Filter.ApplicationNumber) %></td>
                                        <td><%= Html.TextBoxExFor(x => x.Filter.ApplicationNumber) %></td>
                                        <td><%= Html.TableLabelFor(x => x.Filter.DocumentLabel) %></td>
                                        <td><%= Html.TableLabelFor(x => x.Filter.DocumentSeries) %>&nbsp;<%= Html.TextBoxExFor(x => x.Filter.DocumentSeries, new {style = "width:40px"}) %>
                                            &nbsp;<%= Html.TableLabelFor(x => x.Filter.DocumentNumber) %>&nbsp;<%= Html.TextBoxExFor(x => x.Filter.DocumentNumber, new {style = "width:100px"}) %></td>

                                    </tr>
                                    <tr>
                                        <td><%= Html.TableLabelFor(x => x.Filter.LastName) %></td>
                                        <td><%= Html.TextBoxExFor(x => x.Filter.LastName) %></td>
                                        <td><%= Html.TableLabelFor(x => x.Filter.DateBegin) %></td>
                                        <td><%= Html.DatePickerFor(x => x.Filter.DateBegin) %>&nbsp; <%= Html.TableLabelFor(x => x.Filter.DateEnd) %>&nbsp;<%= Html.DatePickerFor(x => x.Filter.DateEnd) %></td>
                                    </tr>
                                    <tr>
                                        <td><%= Html.TableLabelFor(x => x.Filter.FirstName) %></td>
                                        <td><%= Html.TextBoxExFor(x => x.Filter.FirstName) %></td>
                                        <td><%= Html.TableLabelFor(x => x.Filter.DirectionName) %></td>
                                        <td><%= Html.TextBoxExFor(x => x.Filter.DirectionName) %></td>
                                    </tr>
                                    <tr>
                                        <td><%= Html.TableLabelFor(x => x.Filter.MiddleName) %></td>
                                        <td><%= Html.TextBoxExFor(x => x.Filter.MiddleName) %></td>
                                        <td><%= Html.TableLabelFor(x => x.Filter.CompetitiveGroupName) %></td>
                                        <td><%= Html.TextBoxExFor(x => x.Filter.CompetitiveGroupName) %></td>
                                    </tr>
                                    <tr>	
                                        <td colspan="4" style="text-align: center;">
                                            <input type="button" class="button primary" onclick=" applyFilter() " value="Найти" />
                                            <input type="button" class="button" onclick=" clearFilter() " value="Сбросить фильтр" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
                <div style="overflow: scroll">
                <table class="gvuzDataGrid tableStatement2" cellpadding="3" id="tableAppList">
                    <thead>
                        <tr>
                            <th style="text-align: center"><span class="linkSumulator" onclick=" doSort(this, 1) "><%= Html.LabelFor(x => x.AppDescr.ApplicationNumber) %></span></th>
                            <th><span class="linkSumulator" onclick=" doSort(this, 2) "><%= Html.LabelFor(x => x.AppDescr.EntrantFIO) %></span></th>
                            <th><span class="linkSumulator" onclick=" doSort(this, 3) "><%= Html.LabelFor(x => x.AppDescr.EntrantDocData) %></span></th>
                            <th><span class="linkSumulator" onclick=" doSort(this, 4) "><%= Html.LabelFor(x => x.AppDescr.EducationLevel) %></span></th>
                            <th><span class="linkSumulator" onclick=" doSort(this, 5) "><%= Html.LabelFor(x => x.AppDescr.EducationForm) %></span></th>
                            <th><span class="linkSumulator" onclick=" doSort(this, 6) "><%= Html.LabelFor(x => x.AppDescr.DirectionName) %></span></th>
                            <th><span class="linkSumulator" onclick=" doSort(this, 7) "><%= Html.LabelFor(x => x.AppDescr.BenefitName) %></span></th>
                            <th><span class="linkSumulator" id="TargetSpan"><%= Html.LabelFor(x => x.AppDescr.TargetOrganisationName) %></span></th>
                            <th><span class="linkSumulator" onclick=" doSort(this, 8) "><%= Html.LabelFor(x => x.AppDescr.BallCount) %></span></th>
                            <th id="thAction" style="text-align: center">
                                <%= Html.LabelFor(x => x.AppDescr.ApplicationID) %>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr id="trAddNew" style="display: none">
                        </tr>
                    </tbody>
                </table>
                </div>
            </div>
        </div>
    </div>
    <div id="divPopupApplication"></div>
    <script type="text/javascript">
        var gridItems = JSON.parse('<%= Html.Serialize(Model.Applications) %>');
        var eSearchedAppID = <%= ViewBag.ESearchApplicationID ?? 0 %>;
        var UserReadonly = <%= UrlUtils.IsReadOnly(FBDUserSubroles.OrderDirection) ? "true" : "false" %>;
        jQuery(function() {
            jQuery('#saveUid').bind('click', function() {
                var parameters = 'orderId=' + selectedOrder + '&uid=' + jQuery('#OrderUID').val();
                doPostAjax('<%= Url.Generate<InstitutionApplicationController>(x => x.SaveUid(null, null)) %>', parameters,
                    function(data) {
                        if (addValidationErrorsFromServerResponse(data, false)) {
                            return false;
                        }
                        jQuery('#orderGrid tr[itemID="' + selectedOrder + '"]').attr('orderUID', escapeHtml(jQuery('#OrderUID').val()));
                        FogSoft.Common.showSuccessMessage('UID сохранён успешно.');
                        return true;
                    }, "application/x-www-form-urlencoded", null);
            });

        });

        function addItem($trBefore, item) {
            var className = $trBefore.prev().attr('class');
            if (className == 'trline2') className = 'trline1';
            else className = 'trline2';

            var isTarget = (item.TargetOrganisationName != null) 
            isTarget ? $('#TargetSpan').hide : $('#TargetSpan').show; 

            var buttons = item.CanBeDeleted ? '<a href="#" class="btnDelete" onClick="doRemoveApplication(this);return false;" title="Исключить из приказа"></a>'
                : '<span class="btnDeleteGray"></span>';
            if (UserReadonly) buttons = '';
            var forceSearchedStyle = '';
            if (item.ApplicationID == eSearchedAppID)
                forceSearchedStyle = 'style="background-color:#ffffe0;"';
            $trBefore.before('<tr itemID="' + item.ApplicationID + '" ' + forceSearchedStyle + ' class="' + className + '"><td align="center"><a href="#" onClick="doApplicationView(this);return false;">'
                + item.ApplicationNumber + '</a></td><td>' +
                escapeHtml(item.EntrantFIO) + '</td><td>' +
                escapeHtml(item.EntrantDocData) + '</td><td>' +
                escapeHtml(item.EducationLevel) + '</td><td>' +
                escapeHtml(item.EducationForm) + '</td><td>' +
                escapeHtml(item.DirectionName) + '</td><td>' +
                escapeHtml(item.BenefitName) + '</td><td>' +
                (isTarget ? (escapeHtml(item.TargetOrganisationName)) : '')  + '</td><td>' +
                (item.BallCountString) + '</td><td align="center">' +
                buttons + '</td></tr>');
        }

        function doRemoveApplication(el) {
            var $tr = jQuery(el).parents('tr:first');
            confirmDialog('Вы действительно хотите исключить заявление из приказа?', function() {
                doPostAjax('<%= Url.Generate<InstitutionApplicationController>(x => x.ExcludeApplicationFromOrder(null)) %>', "applicationID=" + $tr.attr('itemID'),
                    function(data) {
                        if (!addValidationErrorsFromServerResponse(data)) {
                            $tr.remove().detach();
                            if (jQuery('#tableAppList tr[itemID]').length == 0)
                                location.reload(1);
                        }
                    }, "application/x-www-form-urlencoded");
            });
        }

        function fillGrid() {
            jQuery('#tableAppList tbody tr:not(#trAddNew)').remove().detach();
            jQuery.each(gridItems, function(idx, n) { addItem(jQuery('#trAddNew'), n); });
        }

        var viewApplicationID = 0;

        function doApplicationView(el) {
            var $tr = jQuery(el).parents('tr:first');
            var appID = $tr.attr('itemID');
            viewApplicationID = appID;
            doPostAjax('<%= Url.Generate<ApplicationController>(x => x.ApplicationViewPopup(0, 0, false)) %>', "tabID=0&applicationID=" + appID, function(data) {
                jQuery('#divPopupApplication').html(data);
                jQuery('#divPopupApplication').dialog({
                    modal: true,
                    width: 900,
                    title: 'Просмотр заявления',
                    buttons: {
                        "Закрыть": function() { jQuery(this).dialog('close'); }
                    }
                }).dialog('open');
            }, "application/x-www-form-urlencoded", "html");
            return false;
        }


        function publishOrder(el, target) {
            var orderID = jQuery(el).attr('orderID');
            doPostAjax('<%= Url.Generate<InstitutionApplicationController>(x => x.PublishOrder(null, null)) %>', "orderID=" + orderID + '&typeID=' + target,
                function(data) {
                    if (!addValidationErrorsFromServerResponse(data)) {
                        location.reload(1);
                    }
                }, "application/x-www-form-urlencoded");
        }

        var selectedOrder = null;
        var eSelectedOrder = <%= ViewBag.ESearchOrderID ?? "null" %>;
        var pageNumber = <%= ViewBag.ESearchPageNumber ?? 0 %>;

        function getApplicationList(isRefresh) {
            isRefresh = isRefresh ? true : false;
            if (isRefresh) {
                pageNumber = 0;
            }

            var model = {
                OrderID: selectedOrder,
                PageNumber: pageNumber,
                SortID: currentSorting
            };
            if (filterModel != null)
                model.Filter = filterModel;

            if (parseInt(model.OrderID) < 1)
                return;
            
            doPostAjax('<%= Url.Generate<InstitutionApplicationController>(x => x.GetIncludeInOrderAppList(null)) %>', JSON.stringify(model),
                function(data) {
                    if (!addValidationErrorsFromServerResponse(data)) {
                        gridItems = data.Data.Applications;
                        fillPager(data.Data.TotalPageCount, pageNumber);
                        setFilterAppCount(data.Data.TotalItemFilteredCount, data.Data.TotalItemCount);
                        if (isRefresh) {
                            jQuery('#tableAppList,#divFilterBlock,#divOrderEdit').show();
                            document.location.hash = '#order' + selectedOrder;
                            var $orderRow = jQuery('#orderGrid tr[itemID="' + selectedOrder + '"]');
                            jQuery('#OrderUID').val($orderRow.attr('orderUID'));
                            var orderStatus = $orderRow.attr('statusID');
                            if (!UserReadonly) {
                                if (orderStatus == 2) jQuery('#btnOrderEdit').show();
                                else jQuery('#btnOrderEdit').hide();
                                if (orderStatus == 1) jQuery('#btnOrderPublish').show();
                                else jQuery('#btnOrderPublish').hide();
                                if (UserReadonly) jQuery('#btnOrderPublish,#btnOrderEdit').hide();

                                jQuery('#btnOrderEdit').attr('orderID', selectedOrder);
                                jQuery('#btnOrderPublish').attr('orderID', selectedOrder);
                                jQuery('#btnOrderEdit').click(function() { publishOrder(this, 0); });
                                jQuery('#btnOrderPublish').click(function() { publishOrder(this, 1); });
                            }

                        }
                        fillGrid();
                    }
                });
        }

        function movePager(pageID) {
            pageNumber = pageID;
            getApplicationList(null);
        }

        var filterModel = null;

        function toggleFilter() {
            if (jQuery('#btnShowFilter').hasClass('filterDisplayed')) {
                jQuery('#btnShowFilter').removeClass('filterDisplayed');
                jQuery('#btnShowFilter').html('Отобразить фильтр');
                jQuery('#btnShowFilter').parent().removeClass('nonHideTable');
                jQuery('#btnShowFilter').parent().parent().parent().addClass('tableHeaderCollapsed');
                jQuery('#divFilter').hide();
                setTimeout(function() { jQuery('#divCompGroups').show(); }, 0);
            } else {
                jQuery('#btnShowFilter').addClass('filterDisplayed');
                jQuery('#btnShowFilter').html('Скрыть фильтр');
                jQuery('#btnShowFilter').parent().addClass('nonHideTable');
                jQuery('#divCompGroups').hide();
                jQuery('#btnShowFilter').parent().parent().parent().removeClass('tableHeaderCollapsed');
                jQuery('#divFilter').show();
            }
        }

        function applyFilter() {
            filterModel = {
                ApplicationNumber: jQuery('#Filter_ApplicationNumber').val(),
                DateBegin: jQuery('#Filter_DateBegin').val(),
                DateEnd: jQuery('#Filter_DateEnd').val(),
                LastName: jQuery('#Filter_LastName').val(),
                FirstName: jQuery('#Filter_FirstName').val(),
                MiddleName: jQuery('#Filter_MiddleName').val(),
                DocumentSeries: jQuery('#Filter_DocumentSeries').val(),
                DocumentNumber: jQuery('#Filter_DocumentNumber').val(),
                DirectionName: jQuery('#Filter_DirectionName').val(),
                CompetitiveGroupName: jQuery('#Filter_CompetitiveGroupName').val()
            };
            jQuery('#compGroupFilterName').text(filterModel.CompetitiveGroupName == '' ? "Все группы" : filterModel.CompetitiveGroupName);
            getApplicationList(null);
        }

        function clearFilter() {
            filterModel = null;
            jQuery('#Filter_ApplicationNumber').val('');
            jQuery('#Filter_DateBegin').val('');
            jQuery('#Filter_DateEnd').val('');
            jQuery('#Filter_LastName').val('');
            jQuery('#Filter_FirstName').val('');
            jQuery('#Filter_MiddleName').val('');
            jQuery('#Filter_DocumentSeries').val('');
            jQuery('#Filter_DocumentNumber').val('');
            jQuery('#Filter_DirectionName').val('');
            jQuery('#Filter_CompetitiveGroupName').val('');
            jQuery('#compGroupFilterName').text("Все группы");
            applyFilter();
        }

        var currentSorting = null;

        function doSort(el, sortID) {
            var isSortedUp = jQuery(el).hasClass('sortedUp');
            jQuery('.sortUp,.sortDown').remove().detach();
            if (isSortedUp)
                jQuery(el).after('<span class="sortDown"></span>');
            else
                jQuery(el).after('<span class="sortUp"></span>');
            jQuery(el).parent().parent().find('.sortedUp').removeClass('sortedUp');
            if (isSortedUp)
                sortID = -sortID;
            else
                jQuery(el).addClass('sortedUp');
            currentSorting = sortID;
            getApplicationList(null);
        }


        function switchTab(tabID) {
            if (tabID == 6) //Для списков рекомендованных - отдельная обработка.
            {
                window.location = '<%= Url.Generate<RecommendedListsController>(x => x.GetRecommendedList()) %>';
                return;
            }
            window.location = '<%= Url.Generate<InstitutionApplicationController>(x => x.ApplicationList()) %>#tab' + tabID;
        }

        var filterDirections = JSON.parse('<%= Html.Serialize(Model.Directions) %>');
        var filterCompetitiveGroups = JSON.parse('<%= Html.Serialize(Model.CompetitiveGroups) %>');
        jQuery(document).ready(function() {
            new TabControl(jQuery('#tabControl'), [
                    { name: 'Новые', link: 'javascript:switchTab(1)', enable: true, noWrap: true },
                    { name: 'Не прошедшие проверку', link: 'javascript:switchTab(2)', enable: true },
                    { name: 'Отозванные', link: 'javascript:switchTab(3)', enable: true, noWrap: true },
                    { name: 'Принятые', link: 'javascript:switchTab(4)', enable: true, noWrap: true },
                    { name: 'Рекомендованные к зачислению', link: 'javascript:switchTab(6)', enable: true, noWrap: true },
                    { name: 'Включенные в приказ', link: 'javascript:void(0)', enable: true, noWrap: false, selected: true }
                ]
            ).init();

            jQuery('#tableAppList').hide();
            jQuery('#btnShowOrderList').hide();
            jQuery('#divFilter').hide();
            jQuery('#divFilterBlock').hide();
            jQuery('#divOrderStatusInfo,#divOrderStatusButton').hide();
            jQuery(".datePicker").datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-40:+0' }); //jQuery('#Filter_DirectionName').autocomplete({ source: filterDirections, minLength: 1 })
            autocompleteDropdown(jQuery('#Filter_DirectionName'), { source: filterDirections, minLength: 1 });
            autocompleteDropdown(jQuery('#Filter_CompetitiveGroupName'), { source: filterCompetitiveGroups, minLength: 1 }); //		setTimeout(function() {
//			jQuery('#tableOrderList tbody tr td input[doDisable="1"]').attr('disabled', 'disabled')
            //		}, 0)

        });
        var isFirstTimeOrderLoad = true;

        function onOrdersLoaded() {
            if (!isFirstTimeOrderLoad) return;
            isFirstTimeOrderLoad = false;
            if (document.location.hash != null && document.location.hash.indexOf('#order') == 0) {
                var currentTab = parseInt(document.location.hash.substring(6));
                doSelectOrder(jQuery('#orderGrid tbody tr[itemID="' + currentTab + '"] a.btnMove')[0]);
            }
            if (eSelectedOrder != null && eSelectedOrder > 0) {
                doSelectOrder(jQuery('#orderGrid tbody tr[itemID="' + eSelectedOrder + '"] a.btnMove')[0]);
            }
        }

        function setFilterAppCount(filteredCount, totalCount) {
            var res = totalCount;
            if (filteredCount < totalCount)
                res = filteredCount + ' из ' + res;
            jQuery('#spAppCountField').html(res);
        }

        jQuery("#btnLoadApplicationList").click(function() {

            clearValidationErrors(jQuery('#content'));
            var model = {
                OrderID: selectedOrder,
                PageNumber: pageNumber,
                SortID: currentSorting
            };
            if (filterModel != null)
                model.Filter = filterModel;
            doPostAjax('<%= Url.Generate<InstitutionApplicationController>(x => x.GenerateXmlListForIncludedInOrder(null)) %>', JSON.stringify(model), function(data) {
                if (!addValidationErrorsFromServerResponse(data, false)) {
                    location.href = '<%= Url.Generate<InstitutionApplicationController>(x => x.GetXmlList(null, null)) %>?xmlListName=' + encodeURI(data.Data) + '&tabId=5';
                }
            });
        });
    </script>
</asp:Content>