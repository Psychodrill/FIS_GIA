<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.CompositionResults.CompositionResultsFilterViewModel>" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<% 
    var ssClass = new {style = "width: 340px"};
    var labelsInsideClass = new {@class = "labelsInside"};
%>

<div class="tableHeader5l" data-bind="css: {tableHeaderCollapsed: !isVisible()}">
    <div>
        <div class="hideTable" style="float: left;" data-bind="css: {nonHideTable: isVisible()}, click: toggleVisible">
            <span data-bind="text: visibleText" />
        </div>
        <%--<% if (ConfigHelper.ShowFilterStatistics()) { %>
        <div class="appCount">Количество записей:&nbsp;<span data-bind="text: $root.filterStats"></span></div>
        <% } %>--%>
        <div class="filterNotif" data-bind="visible: !isDefault()">
            <span>Применен фильтр</span>
        </div>
    </div>
    <div style="clear: both;" data-bind="visible: isVisible">
        <table class="tableForm" >
            <colgroup>
                <col style="width: 20%;"/>
                <col style="width: 13%"/>
                <col style="width: 20%"/>
                <col style="width: 13%"/>
                <col style="width: 20%" />
                <col style="width: 13%" />
            </colgroup>
            <tbody>
                <tr>
                    <td class="labelsInside">
                        <%= Html.TableLabelFor(x => x.SelectedCampaign) %>
                    </td>
                    <td class="valueInside">
                        <%= Html.BoundedSelectFor(x => x.SelectedCampaign, x => x.Campaigns, ssClass) %>
                    </td>
                    <td>
                        <%= Html.TableLabelFor(x => x.RegistrationDateFrom, labelsInsideClass)%>
                    </td>
                    <td class="valueInside">
                        <%= Html.BoundedTextBoxFor(x => x.RegistrationDateFrom, new {@class="shortInput datePicker", maxlength=10}) %>&nbsp;&nbsp;
                        <%= Html.TableLabelFor(x => x.RegistrationDateTo, labelsInsideClass)%>
                        <%= Html.BoundedTextBoxFor(x => x.RegistrationDateTo, new {@class="shortInput datePicker", maxlength=10}) %>
                    </td>
                    <td colspan="2">
                        <input id="hasCompositionResults" type="checkbox" data-bind="checked: HasResults" />
                        <label for="hasCompositionResults"><%= Html.DisplayNameFor(x => x.HasResults) %></label>
                        &nbsp;&nbsp;&nbsp;
                        <input id="notDownloaded" type="checkbox" data-bind="checked: NotDownloaded" />
                        <label for="notDownloaded"><%= Html.DisplayNameFor(x => x.NotDownloaded) %></label>
                    </td>
                </tr>

                <tr>
                    <td class="labelsInside">
                        <%= Html.TableLabelFor(x => x.SelectedCompetitiveGroup) %>
                    </td>
                    <td class="valueInside">
                        <%= Html.BoundedSelectFor(x => x.SelectedCompetitiveGroup, x => x.CompetitiveGroups, ssClass) %>
                    </td>
                    <td class="labelsInside">
                        <%= Html.TableLabelFor(x => x.LastName, labelsInsideClass)%>
                    </td>
                    <td class="valueInside">
                        <%= Html.BoundedTextBoxFor(x => x.LastName)%>
                    </td>
                    <td class="labelsInside">
                        <%= Html.TableLabelFor(x => x.ApplicationNumber, labelsInsideClass)%>
                    </td>
                    <td class="valueInside">
                        <%= Html.BoundedTextBoxFor(x => x.ApplicationNumber)%>
                    </td>
                </tr>


                <tr>
                    <td colspan="6" style="text-align: center">
                        <input type="submit" class="button primary" value="Показать результаты сочинений" data-bind="click: applyFilter" />
                        <input type="button" class="button" value="Сбросить фильтр" style="width: auto" data-bind="click: resetFilter" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</div>