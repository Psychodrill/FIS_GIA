<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.ExtendedApplicationListViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Расширенный поиск заявлений
</asp:Content>
<asp:Content ContentPlaceHolderID="PageTitle" runat="server">
    Расширенный поиск заявлений
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="divstatement">
        <div id="content"> 
            <div class="tableHeader5l tableHeaderCollapsed">
                <div id="divFilterPlace">
                    <div class="hideTable" onclick=" toggleFilter() " style="float: left">
                        <span id="btnShowFilter">Отобразить фильтр</span></div>
                    <div id="spAppCount" class="appCount">
                        Количество заявлений: <span id="spAppCountField"></span>
                    </div>
                </div>
                <div id="divFilter" style="display: none">
                    <table class="tableForm">
                        <br />
                        <tbody>
                            <tr>
                                <td class="labelsInside">
                                    <%= Html.TableLabelFor(x => x.Filter.ApplicationNumber) %>
                                </td>
                                <td>
                                    <%= Html.TextBoxExFor(x => x.Filter.ApplicationNumber) %>
                                </td>
                                <td class="labelsInside">
                                    <%= Html.TableLabelFor(x => x.Filter.EntrantLastName) %>
                                </td>
                                <td>
                                    <%= Html.TextBoxExFor(x => x.Filter.EntrantLastName) %>
                                </td>
                            </tr>
                            <tr>
                                <td class="labelsInside">
                                    <%= Html.TableLabelFor(x => x.Filter.DateBegin) %>
                                </td>
                                <td>
                                    <%= Html.DatePickerFor(x => x.Filter.DateBegin) %>&nbsp;
                                    <%= Html.TableLabelFor(x => x.Filter.DateEnd, new {@class = "labelsInside"}) %>
                                    <%= Html.DatePickerFor(x => x.Filter.DateEnd) %>
                                </td>
                                <td class="labelsInside">
                                    <%= Html.TableLabelFor(x => x.Filter.EntrantFirstName) %>
                                </td>
                                <td>
                                    <%= Html.TextBoxExFor(x => x.Filter.EntrantFirstName) %>
                                </td>
                            </tr>
                            <tr>
                                <td class="labelsInside">
                                    <%= Html.TableLabelFor(x => x.Filter.CompetitiveGroupID) %>
                                </td>
                                <td>
                                    <%= Html.TextBoxExFor(x => x.Filter.CompetitiveGroupID) %>
                                </td>
                                <td class="labelsInside">
                                    <%= Html.TableLabelFor(x => x.Filter.EntrantMiddleName) %>
                                </td>
                                <td>
                                    <%= Html.TextBoxExFor(x => x.Filter.EntrantMiddleName) %>
                                </td>
                            </tr>
                            <tr>
                                <td class="labelsInside">
                                    <%= Html.TableLabelFor(x => x.Filter.ApplicationStatusID) %>
                                </td>
                                <td>
                                    <%= Html.TextBoxExFor(x => x.Filter.ApplicationStatusID, new {@readonly = "readonly"}) %>
                                    <div id="divApplicationStatuses" style="display: none; padding: 5px; position: absolute"
                                         class="ui-widget ui-widget-content ui-corner-all">
                                        fdsfdsfdss
                                    </div>
                                </td>
                                <td class="labelsInside">
                                    <%= Html.TableLabelFor(x => x.Filter.EntrantDocSeries) %>
                                </td>
                                <td>
                                    <%= Html.TextBoxExFor(x => x.Filter.EntrantDocSeries, new {style = "width: 50px"}) %>&nbsp;
                                    <%= Html.TableLabelFor(x => x.Filter.EntrantDocNumber, new {@class = "labelsInside"}) %>
                                    <%= Html.TextBoxExFor(x => x.Filter.EntrantDocNumber, new {style = "width: 150px"}) %>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="text-align: center;">
                                    <input type="button" id="btnApplyFilter" class="button primary" onclick=" applyFilter() " value="Найти" />
                                    <input type="button" id="btnClearFilter" class="button" onclick=" clearFilter() " value="Сбросить фильтр" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
            <table class="gvuzDataGrid tableStatement2" cellpadding="3">
                <thead>
                    <tr>
                        <th colspan="5" align="center">
                            <%= Html.LabelFor(x => x.AppPartDescr) %>
                        </th>
                        <th colspan="2" align="center">
                            <%= Html.LabelFor(x => x.EntrantPartDescr) %>
                        </th>
                    </tr>
                    <tr>
                        <th style="text-align: center">
                            <span class="linkSumulator" onclick=" doSort(this, 1) ">
                                <%= Html.LabelFor(x => x.AppDescr.ApplicationNumber) %></span>
                        </th>
                        <th>
                            <span class="linkSumulator" onclick=" doSort(this, 2) ">
                                <%= Html.LabelFor(x => x.AppDescr.ApplicationDate) %></span>
                        </th>
                        <th>
                            <span>
                                <%= Html.LabelFor(x => x.AppDescr.CompetitiveGroupName) %></span>
                        </th>
                        <th>
                            <span class="linkSumulator" onclick=" doSort(this, 4) ">
                                <%= Html.LabelFor(x => x.AppDescr.StatusName) %></span>
                        </th>
                        <th style="text-align: center">
                            <%= Html.LabelFor(x => x.AppDescr.ApplicationID) %>
                        </th>
                        <th>
                            <span class="linkSumulator" onclick=" doSort(this, 5) ">
                                <%= Html.LabelFor(x => x.AppDescr.EntrantFIO) %></span>
                        </th>
                        <th>
                            <span class="linkSumulator" onclick=" doSort(this, 6) ">
                                <%= Html.LabelFor(x => x.AppDescr.EntrantDocData) %></span>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr id="trAddNew" style="display: none">
                    </tr>
                </tbody>
            </table>
        </div>
        <div style="display: none">
            <div id="divDecisionDialog">
                <table>
                    <tbody>
                        <tr>
                            <td class="caption">
                                <%= Html.TableLabelFor(x => x.StatusDecision) %>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%= Html.TextAreaFor(x => x.StatusDecision, new {style = "width:550px"}) %>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div id="divPopupApplication">
            </div>
        </div>
        <div id="divIncludeInOrder">
        </div>
    </div>
    <script language="javascript" type="text/javascript">
        var gridItems = JSON.parse('<%= Html.Serialize(Model.Applications) %>');
        var currentSorting = null;
        var currentTab = 10;
        var UserReadonly = <%= UrlUtils.IsReadOnly(FBDUserSubroles.ApplicationsDirection) ? "true" : "false" %>;

        function addItem($trBefore, item) {
            var buttons = '';
            var appViewLinkStart = '<a href="#" onClick="doApplicationView(this);return false;">';
            var appviewLinkEnd = '</a>';
            var btnViewCode = '';
            if (!UserReadonly)
                if (item.StatusID == 1 || item.StatusID == 6 || item.StatusID == 3 || item.StatusID == 4 || item.StatusID == 5)
                    buttons = '<a href="<%= Url.Generate<ApplicationController>(x => x.ApplicationPersonalDataByApp(null, null)) %>?tabID=10&applicationID='
                        + item.ApplicationID
                        + '" class="btnEdit" title="Редактировать заявление" onclick="doApplicationEditByAppID(' + item.ApplicationID + ');return false;"></a> ';
            if (item.StatusID == 1) {
                appViewLinkStart = '';
                appviewLinkEnd = '';
            }
            buttons += btnViewCode +
                '<a href="<%= Url.Generate<InstitutionApplicationController>(x => x.FindApplicationInList(null)) %>?applicationID='
                + item.ApplicationID + '" class="btnGoStat" title="К заявлению"></a>';
            var className = $trBefore.prev().attr('class');
            if (className == 'trline2') className = 'trline1';
            else className = 'trline2';
            $trBefore.before('<tr itemID="' + item.ApplicationID + '" class="' + className + '"><td align="center">' + appViewLinkStart + item.ApplicationNumber + appviewLinkEnd + '</td>'
                + '<td>' + escapeHtml(item.ApplicationDate) + '</td>'
                + '<td>' + escapeHtml(item.CompetitiveGroupName) + '</td>'
                + '<td>' + escapeHtml(item.StatusName) + '</td>'
                + '<td align="center">' + buttons + '</td>'
                + '<td><a href="<%= Url.Generate<EntrantController>(x => x.EntrantInfo(null)) %>?entrantID=' + item.EntrantID
                + '">' + escapeHtml(item.EntrantFIO) + '</a></td>'
                + '<td>' + escapeHtml(item.EntrantDocData) + '</td>'
                + '</tr>');
        }

        function fillGrid() {
            jQuery('.gvuzDataGrid tbody tr:not(#trAddNew)').remove().detach();
            if (gridItems.length > 0)
                jQuery.each(gridItems, function(idx, n) { addItem(jQuery('#trAddNew'), n); });
            else {
                jQuery('#trAddNew').before('<tr><td colspan="7" align="center">Не найдено ни одного заявления</td></tr>');
            }
        }

        function doSort(el, sortID) {
            var isSortedUp = jQuery(el).hasClass('sortedUp');
            jQuery('.sortUp,.sortDown').remove().detach();
            if (isSortedUp)
                jQuery(el).after('<span class="sortDown"></span>');
            else
                jQuery(el).after('<span class="sortUp"></span>');
            jQuery(el).removeClass('sortedUp');
            if (isSortedUp)
                sortID = -sortID;
            else
                jQuery(el).addClass('sortedUp');
            currentSorting = sortID;
            updateData();
        }

        function prepareModel() {
            var model =
            {
                SortID: currentSorting,
                PageNumber: pageNumber
            };
            if (filterModel != null)
                model.Filter = filterModel;
            return model;
        }

        function updateData() {
            clearValidationErrors(jQuery('#content'));
            doPostAjax('<%= Url.Generate<InstitutionApplicationController>(x => x.GetExtendedApplicationList(null)) %>', JSON.stringify(prepareModel()), function(data) {
                if (!addValidationErrorsFromServerResponse(data, false)) {
                    gridItems = data.Data.Applications;
                    fillGrid();
                    setFilterAppCount(data.Data.TotalItemFilteredCount, data.Data.TotalItemCount);
                    fillPager(data.Data.TotalPageCount, pageNumber);
                    doAppViewFromReturn();
                }
            });
        }

        function setFilterAppCount(filteredCount, totalCount) {
            var res = totalCount;
            if (filteredCount < totalCount)
                res = filteredCount + ' из ' + res;
            jQuery('#spAppCountField').html(res);
        }

        var competitiveGroups = JSON.parse('<%= Html.Serialize(Model.CompetitiveGroups) %>');
        var applicationStatuses = JSON.parse('<%= Html.Serialize(Model.ApplicationStatuses) %>');

        function toggleFilter() {
            if (jQuery('#btnShowFilter').hasClass('filterDisplayed')) {
                jQuery('#btnShowFilter').removeClass('filterDisplayed');
                jQuery('#btnShowFilter').html('Отобразить фильтр');
                jQuery('#btnShowFilter').parent().removeClass('nonHideTable');
                jQuery('#btnShowFilter').parent().parent().parent().addClass('tableHeaderCollapsed');
                jQuery('#divFilter').hide();
            } else {
                jQuery('#btnShowFilter').addClass('filterDisplayed');
                jQuery('#btnShowFilter').html('Скрыть фильтр');
                jQuery('#btnShowFilter').parent().addClass('nonHideTable');
                jQuery('#btnShowFilter').parent().parent().parent().removeClass('tableHeaderCollapsed');
                jQuery('#divFilter').show();
            }
        }

        var filterModel = null;

        function applyFilter() {
            setTimeout(applyFilter1, 0);
        }

        function applyFilter1() {
            var compGroupID = 0;
            var selCompGroupName = jQuery('#Filter_CompetitiveGroupID').val();
            jQuery.each(competitiveGroups, function() {
                if (this.Name == selCompGroupName) {
                    compGroupID = this.ID;
                    return false;
                }
            });
            var stIDs = [];
            var v = jQuery('#Filter_ApplicationStatusID').attr('stIDs');
            if (v != '' && (typeof v != "undefined"))
                stIDs = v.split(',');
            filterModel = {
                DateBegin: jQuery('#Filter_DateBegin').val(),
                DateEnd: jQuery('#Filter_DateEnd').val(),
                ApplicationNumber: jQuery('#Filter_ApplicationNumber').val(),
                EntrantLastName: jQuery('#Filter_EntrantLastName').val(),
                EntrantFirstName: jQuery('#Filter_EntrantFirstName').val(),
                EntrantMiddleName: jQuery('#Filter_EntrantMiddleName').val(),
                EntrantDocSeries: jQuery('#Filter_EntrantDocSeries').val(),
                EntrantDocNumber: jQuery('#Filter_EntrantDocNumber').val(),
                CompetitiveGroupID: compGroupID,
                ApplicationStatusID: stIDs
            };
            pageNumber = 0;
            updateData();
        }

        function clearFilter() {
            filterModel = null;
            jQuery('#Filter_DateBegin').val('');
            jQuery('#Filter_DateEnd').val('');
            jQuery('#Filter_EntrantLastName').val('');
            jQuery('#Filter_EntrantFirstName').val('');
            jQuery('#Filter_EntrantMiddleName').val('');
            jQuery('#Filter_EntrantDocSeries').val('');
            jQuery('#Filter_EntrantDocNumber').val('');
            jQuery('#Filter_CompetitiveGroupID').val('');
            jQuery('#Filter_ApplicationNumber').val('');
            jQuery('#divApplicationStatuses input').removeAttr('checked');
            jQuery('#Filter_ApplicationStatusID').attr('stIDs', '');
            jQuery('#Filter_ApplicationStatusID').val('');
            applyFilter();
        }

        function saveFilter() {
            setCookie('appListFilter<%=Model.InstitutionID %>', JSON.stringify(prepareModel()), 1);
        }

        function restoreFilter() {
            var appListFilterString = getCookie('appListFilter<%=Model.InstitutionID %>' );
            if (typeof appListFilterString != "undefined") {
                setCookie('appListFilter<%=Model.InstitutionID %>', '', -1);
                if (document.location.toString().indexOf('back=1') > 0) {
                    var model = JSON.parse(appListFilterString);
                    currentSorting = model.SortID;
                    pageNumber = model.PageNumber;
                    if (model.Filter != null) {
                        jQuery('#Filter_DateBegin').val(model.Filter.DateBegin);
                        jQuery('#Filter_DateEnd').val(model.Filter.DateEnd);
                        jQuery('#Filter_EntrantLastName').val(model.Filter.EntrantLastName);
                        jQuery('#Filter_EntrantFirstName').val(model.Filter.EntrantFirstName);
                        jQuery('#Filter_EntrantMiddleName').val(model.Filter.EntrantMiddleName);
                        jQuery('#Filter_EntrantDocSeries').val(model.Filter.EntrantDocSeries);
                        jQuery('#Filter_EntrantDocNumber').val(model.Filter.EntrantDocNumber);
                        jQuery('#Filter_ApplicationNumber').val(model.Filter.ApplicationNumber);
                        var compGroupName = '';
                        jQuery.each(competitiveGroups, function() {
                            if (this.ID == model.Filter.CompetitiveGroupID) {
                                compGroupName = this.Name;
                                return false;
                            }
                        });
                        jQuery('#Filter_CompetitiveGroupID').val(compGroupName);
                        var stIDs = model.Filter.ApplicationStatusID;
                        var stIDsStr = '';
                        jQuery.each(stIDs, function() {
                            stIDsStr += this;
                            jQuery('#divApplicationStatuses input[stID="' + this + '"]').attr('checked', 'checked');
                        });
                        jQuery('#Filter_ApplicationStatusID').attr('stIDs', stIDsStr);
                        filterModel = model.Filter;
                    }
                    return true;
                }
            }
            return false;
        }

        function doApplicationEditByAppID(appID) {
            saveFilter();
            window.location = '<%= Url.Generate<ApplicationController>(x => x.ApplicationPersonalDataByApp(null, null)) %>?applicationID='
                + appID + '&tabID=' + currentTab;
        }

        var viewApplicationID = 0;

        function doApplicationView(el) {
            var $tr = jQuery(el).parents('tr:first');
            var appID = $tr.attr('itemID');
            doApplicationViewByAppID(appID);
            return false;
        }

        function doApplicationViewByAppID(appID) {
            saveFilter();
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
        }

        function doAppViewFromReturn() {
            var appID = getCookie('viewAppID');
            if (typeof appID != "undefined") {
                setCookie('viewAppID', '', -1);
                if (document.location.toString().indexOf('back=1') > 0)
                    if (jQuery('.gvuzDataGrid tbody tr[itemID="' + appID + '"]').length > 0)
                        doApplicationViewByAppID(appID);
            }
        }

        jQuery(document).ready(function() {
            jQuery('#divFilter').hide();
            jQuery(".datePicker").datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-40:+0' });
            var compGroupNames = jQuery.map(competitiveGroups, function(n, i) { return n.Name; });
            autocompleteDropdown(jQuery("#Filter_CompetitiveGroupID"), {
                source: compGroupNames,
                delay: 200
            });
            fillApplicationStatuses();
            if (!restoreFilter())
                applyFilter1();
            else
                updateData();
        });
        var pageNumber = 0;

        function movePager(pageID) {
            pageNumber = pageID;
            updateData();
        }

        function fillApplicationStatuses() {
            var res = '';
            for (var i = 0; i < applicationStatuses.length; i++) {
                res += '<input type="checkbox" id="cbAppSt_' + applicationStatuses[i].ID + '" stID="' + applicationStatuses[i].ID + '">'
                    + '<label for="cbAppSt_' + applicationStatuses[i].ID + '">'
                    + applicationStatuses[i].Name + "</label><br/>";
            }
            jQuery('#divApplicationStatuses').html(res);
            jQuery('#Filter_ApplicationStatusID').after('<img title="" id="Filter_ApplicationStatusID_Sel" class="ui-datepicker-trigger gvuz-calendar-icon" alt="..." src="' + absoluteAppPath + 'Resources/Images/ddl.png"/>');
            jQuery('#Filter_ApplicationStatusID').next().click(function() { jQuery(this).prev().focus(); });
            jQuery('#Filter_ApplicationStatusID').focus(function() {
                var p = jQuery(this).position();
                jQuery('#divApplicationStatuses').css('position', 'absolute').css('z-index', 1100).css('top', p.top + jQuery(this).height() + 20)
                    .css('left', p.left).css('width', jQuery(this).width()).fadeIn(300);
            });
            jQuery('body').click(function(evt) {
                if (evt.target == null || evt.target.parentNode == null) return;
                if (evt.target.id == 'divApplicationStatuses' || evt.target.parentNode.id == 'divApplicationStatuses'
                    || evt.target.id == 'Filter_ApplicationStatusID'
                    || evt.target.id == 'Filter_ApplicationStatusID_Sel')
                    return;
                var res1 = '';
                var res2 = '';
                jQuery('#divApplicationStatuses input').each(function() {
                    if (jQuery(this).attr('checked')) {
                        res1 += jQuery(this).attr('stID') + ',';
                        res2 += jQuery(this).next().html() + ', ';
                    }
                });
                if (res1.length > 0)
                    res1 = res1.substr(0, res1.length - 1);
                if (res2.length > 0)
                    res2 = res2.substr(0, res2.length - 2);
                jQuery('#Filter_ApplicationStatusID').attr('stIDs', res1);
                jQuery('#Filter_ApplicationStatusID').val(res2);
                jQuery('#divApplicationStatuses').fadeOut(300);
            });
        } 
    </script>
</asp:Content>