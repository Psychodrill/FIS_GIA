<%--Отрисовка фильтра расширенного поиска заявлений для data-bound модели (knockoutjs)--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.ApplicationsList.SearchApplicationsFilterViewModel>" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<% 
    var ssClass = new { style = "width: 340px" };
    var labelsInsideClass = new {@class = "labelsInside"};
%>

<div class="tableHeader5l" style="height: auto;" data-bind="css: {tableHeaderCollapsed: !isVisible()}">
    <div>
        <div class="hideTable" style="float: left;" data-bind="css: {nonHideTable: isVisible()}, click: toggleVisible">
            <span data-bind="text: visibleText" />
        </div>
        <% if (ConfigHelper.ShowFilterStatistics()) { %>
        <div class="appCount">Записей:&nbsp;<span data-bind="text: $root.filterStats"></span></div>
        <% } %>
        <div style="text-align: right;" data-bind="visible: !isDefault()">
            <span style="color: salmon; font-size: 11px; font-weight: bold; padding-right: 5px;">[Внимание! Применен фильтр]</span>
        </div>
    </div>
    <div style="clear: both;" data-bind="visible: isVisible">
        <table class="tableForm" >
            <colgroup>
                <col style="width: 10%;"/>
                <col style="width: 40%"/>
                <col style="width: 10%"/>
                <col style="width: 40%"/>
            </colgroup>
            <tbody>
                <tr>
                    <td class="labelsInside">
                        <%= Html.TableLabelFor(x => x.ApplicationNumber) %>
                    </td>
                    <td>
                        <%= Html.BoundedTextBoxFor(x => x.ApplicationNumber, new {style = "width: 100px"}) %>
                    </td>
                    <td class="labelsInside">
                        <%= Html.TableLabelFor(x => x.LastName)%>
                    </td>
                    <td>
                        <%= Html.BoundedTextBoxFor(x => x.LastName) %>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%= Html.TableLabelFor(x => x.RegistrationDateFrom, labelsInsideClass)%>
                    </td>
                    <td>
                        <%= Html.BoundedTextBoxFor(x => x.RegistrationDateFrom, new {@class="shortInput datePicker", maxlength=10}) %>&nbsp;&nbsp;
                        <%= Html.TableLabelFor(x => x.RegistrationDateTo, labelsInsideClass)%>
                        <%= Html.BoundedTextBoxFor(x => x.RegistrationDateTo, new {@class="shortInput datePicker", maxlength=10}) %>
                    </td>
                    <td class="labelsInside">
                        <%= Html.TableLabelFor(x => x.FirstName)%>
                    </td>
                    <td>
                        <%= Html.BoundedTextBoxFor(x => x.FirstName) %>
                    </td>
                </tr>
                <tr>
                   <td class="labelsInside">
                        <%= Html.TableLabelFor(x => x.SelectedCompetitiveGroup)%>
                    </td>
                    <td>
                        <%= Html.BoundedSelectFor(x => x.SelectedCompetitiveGroup, x => x.CompetitiveGroups, ssClass) %>
                    </td>
                    <td class="labelsInside">
                        <%= Html.TableLabelFor(x => x.MiddleName)%>
                    </td>
                    <td>
                        <%= Html.BoundedTextBoxFor(x => x.MiddleName) %>
                    </td>
                </tr>
                <tr>
                    <td class="labelsInside">
                        <%= Html.TableLabelFor(x => x.SelectedStatus) %>
                    </td>
                    <td>
                        <%= Html.BoundedSelectFor(x => x.SelectedStatus, x => x.Statuses, ssClass) %>
                    </td>
                    <td class="labelsInside">
                        <%= Html.TableLabelFor(x => x.DocumentSeries)%>
                    </td>
                    <td style="white-space: nowrap">
                        <%= Html.BoundedTextBoxFor(x => x.DocumentSeries, new {style="width: 50px"}) %>
                        &nbsp;
                        <%= Html.TableLabelFor(x => x.DocumentNumber, labelsInsideClass)%> 
                        <%= Html.BoundedTextBoxFor(x => x.DocumentNumber, new {style="width: 150px"}) %>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="text-align: center;">
                        <input type="submit" class="button primary" value="Найти" data-bind="click: applyFilter" />
                        <input type="button" class="button" value="Сбросить фильтр" data-bind="click: resetFilter" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</div>