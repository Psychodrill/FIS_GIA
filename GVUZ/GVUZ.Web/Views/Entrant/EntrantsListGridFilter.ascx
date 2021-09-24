<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.Entrants.EntrantRecordListFilterViewModel>" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<style type="text/css">
    .auto-style2 {
        width: 21%;
    }
    .auto-style4 {
        width: 6%;
    }
    .auto-style5 {
        width: 8%;
        text-align:right
    }
    .auto-style8 {
        width: 99px;
    }
    .auto-style9 {
        width: 4%;
    }
    .auto-style10 {
        width: 111px;
    }
    .auto-style11 {
        width: 30px;
    }
    .auto-style13 {
        width: 150px;
    }
    .auto-style14 {
        width: 189px;
    }
    .auto-style15 {
        width: 3%;
    }
    .auto-style16 {
        width: 466px;
        text-align:right
    }
    </style>
<% 
    var ssClass = new {style = "width: 340px"};
    var labelsInsideClass = new {@class = "labelsInside"};
            var yearComboBoxClass = new { style = "width: 100px" };
%>

<div class="tableHeader5l" style="height: auto;" data-bind="css: {tableHeaderCollapsed: !isVisible()}">
    <div>
        <div class="hideTable" style="float: left;" data-bind="css: {nonHideTable: isVisible()}, click: toggleVisible">
            <span data-bind="text: visibleText" />
        </div>
        <% if (ConfigHelper.ShowFilterStatistics()) { %>
        <div class="appCount no-filter" data-bind="visible: isDefault()">
            Записей:&nbsp;<span data-bind="text: $root.filterStats"></span>
        </div>
        <div class="appCount filter" data-bind="visible: !isDefault()">
            Записей с фильтром:&nbsp;<span data-bind="text: $root.filterStats"></span>
        </div>
        <% } %>
    </div>
    <div style="clear: both;" data-bind="visible: isVisible">
        <table class="tableForm" data-bind="visible: simpleFilterVisible">
            <colgroup>
                <col style="width: 5%"/>
                <col style="width: 15%"/>
                <col style="width: 5%"/>
                <col style="width: 15%"/>
                <col style="width: 60%"/>

            </colgroup>
            <tbody>
                <tr>
                    <td class="auto-style16">
                        <%= Html.TableLabelFor(x => x.LastName)%>
                    </td>
                    <td class="auto-style4">
                        <%= Html.BoundedTextBoxFor(x => x.LastName) %>
                    </td>
                    <td class="auto-style5">
                        <%= Html.TableLabelFor(x => x.ApplicationNumber) %>
                    </td>
                    <td class="auto-style9">
                        <%= Html.BoundedTextBoxFor(x => x.ApplicationNumber) %>
                    </td>
                    <td>
                        <input type="submit" class="button primary" value="Найти" data-bind="click: applyFilter" />
                        <input type="button" class="button" value="Сбросить фильтр"data-bind="click: resetFilter" />
                        <input type="button" class="button" value="Расширенный поиск" data-bind="click: showExtendedFilter" />
                    </td>
                </tr>
            </tbody>
        </table>
        <table class="tableForm" data-bind="visible: extendedFilterVisible">
            <colgroup>
                <col style ="width:10%"/>
                <col style ="width:10%"/>
                <col style ="width:10%"/>
                <col style ="width:10%"/>
                <col style="width: 6%"/>
                <col style="width: 10%"/>
                <col style ="width:44%"/>

            </colgroup>
            <tbody>
                <tr>
                    <td class="auto-style14">
                        <%= Html.TableLabelFor(x => x.ApplicationNumber) %>
                    </td>
                    <td class="auto-style2">
                        <%= Html.BoundedTextBoxFor(x => x.ApplicationNumber) %>
                    </td>
                    <td class="auto-style13">
                        <%= Html.TableLabelFor(x => x.LastName)%>
                    </td>
                    <td class="auto-style15">
                        <%= Html.BoundedTextBoxFor(x => x.LastName) %>
                    </td>
                                        <td class="auto-style11">
                        <%= Html.TableLabelFor(x => x.CampaignYear)%>
                    </td>
                    <td class="auto-style8">
                        <%= Html.BoundedSelectFor(x => x.CampaignYear, x => x.CampaignYears, yearComboBoxClass) %>
                    </td>

                    <td class="auto-style10">
                      
                    </td>

                </tr>
                <tr>
                    <td class="auto-style14">
                        <%= Html.TableLabelFor(x => x.RegistrationDateFrom, labelsInsideClass)%>
                    </td>
                    <td style="white-space: nowrap" class="auto-style2">
                        <%= Html.BoundedTextBoxFor(x => x.RegistrationDateFrom, new {@class="shortInput datePicker", maxlength=10}) %>&nbsp;&nbsp;
                        <%= Html.TableLabelFor(x => x.RegistrationDateTo, labelsInsideClass)%>
                        <%= Html.BoundedTextBoxFor(x => x.RegistrationDateTo, new {@class="shortInput datePicker", maxlength=10}) %>
                    </td>
                    <td class="auto-style13">
                        <%= Html.TableLabelFor(x => x.FirstName)%>
                    </td>
                    <td class="auto-style15">
                        <%= Html.BoundedTextBoxFor(x => x.FirstName) %>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style14">
                        <%= Html.TableLabelFor(x => x.SelectedCompetitiveGroup)%>
                    </td>
                    <td class="auto-style2">
                        <%= Html.BoundedSelectFor(x => x.SelectedCompetitiveGroup, x => x.CompetitiveGroups, ssClass) %>
                    </td>
                    <td class="auto-style13"><%= Html.TableLabelFor(x => x.MiddleName)%></td>
                    <td class="auto-style15">
                        <%= Html.BoundedTextBoxFor(x => x.MiddleName) %>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style14">
                        <%= Html.TableLabelFor(x => x.SelectedStatus)%>
                    </td>
                    <td class="auto-style2">
                        <%= Html.BoundedSelectFor(x => x.SelectedStatus, x => x.Statuses, ssClass) %>
                    </td>
                    <td class="auto-style13">
                        <%= Html.TableLabelFor(x => x.DocumentSeries)%>
                    </td>
                    <td style="white-space: nowrap" class="auto-style15">
                        <%= Html.BoundedTextBoxFor(x => x.DocumentSeries, new {style="width: 50px"}) %>
                        &nbsp;
                        <%= Html.TableLabelFor(x => x.DocumentNumber, labelsInsideClass)%> 
                        <%= Html.BoundedTextBoxFor(x => x.DocumentNumber, new {style="width: 150px"}) %>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style14">
                        <%= Html.TableLabelFor(x => x.SelectedCampaign) %>
                    </td>
                    <td class="auto-style2">
                        <%= Html.BoundedSelectFor(x => x.SelectedCampaign, x => x.Campaigns, ssClass) %>
                    </td>
                    <td colspan="2">
                        <input type="submit" class="button primary" value="Найти" data-bind="click: applyFilter" />
                        <input type="button" class="button" value="Сбросить фильтр" data-bind="click: resetFilter" />
                        <input type="button" class="button" value="Скрыть область" data-bind="click: showSimpleFilter" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</div>