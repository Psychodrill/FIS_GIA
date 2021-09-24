<%--Отрисовка фильтра заявлений, не прошедших проверку для data-bound модели (knockoutjs)--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.ApplicationsList.RecommendedApplicationsFilterViewModel>" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<% 
    var ssClass = new { style = "width: 340px" };
    var labelsInsideClass = new {@class = "labelsInside"};
%>

<div style="white-space: nowrap;padding: 8px;padding-left: 0">
    <label for="quickSelectCampaign"><%= Html.DisplayNameFor(x => x.SelectedCampaign) %>:</label>
    &nbsp;
    <%= Html.BoundedSelectFor(x => x.SelectedCampaign, x => x.Campaigns, new {@class = "ss", style="width: 200px;", id = "quickSelectCampaign"}) %>
    &nbsp;
    <label for="quickSelectStage"><%= Html.DisplayNameFor(x => x.SelectedStage) %>:</label>
    &nbsp;
    <%= Html.BoundedSelectFor(x => x.SelectedStage, x => x.Stages, new { @class = "ss", style="width: 100px;", id = "quickSelectStage" })%>
</div>

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
                <col style="width: 20%"/>
                <col style="width: 10%"/>
                <col style="width: 25%"/>
                <col style="width: 10%"/>
                <col style="width: 20%"/>
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
                        <%= Html.TableLabelFor(x => x.SelectedCompetitiveGroup)%>
                    </td>
                    <td>
                        <%= Html.BoundedSelectFor(x => x.SelectedCompetitiveGroup, x => x.CompetitiveGroups, ssClass) %>
                    </td>
                    <td class="labelsInside">
                        <%= Html.TableLabelFor(x => x.SelectedDirection, labelsInsideClass)%>
                    </td>
                    <td>
                        <%= Html.BoundedSelectFor(x => x.SelectedDirection, x => x.Directions, ssClass) %>
                    </td>
                </tr>
                <tr>
                    <td class="labelsInside">
                        <%= Html.TableLabelFor(x => x.SelectedEducationForm)%>
                    </td>
                    <td>
                        <%= Html.BoundedSelectFor(x => x.SelectedEducationForm, x => x.EducationForms, ssClass) %>
                    </td>
                    <td class="labelsInside">
                        <%= Html.TableLabelFor(x => x.SelectedEducationLevel)%>
                    </td>
                    <td>
                        <%= Html.BoundedSelectFor(x => x.SelectedEducationLevel, x => x.EducationLevels, ssClass) %>
                    </td>  
                    <td colspan="2">&nbsp;</td>   
                </tr>
                <tr>
                    <td class="labelsInside">
                        <%= Html.TableLabelFor(x => x.LastName)%>
                    </td>
                    <td>
                        <%= Html.BoundedTextBoxFor(x => x.LastName) %>
                    </td>
                    <td class="labelsInside">
                        <%= Html.TableLabelFor(x => x.FirstName)%>
                    </td>
                    <td>
                        <%= Html.BoundedTextBoxFor(x => x.FirstName) %>
                    </td>
                    <td class="labelsInside">
                        <%= Html.TableLabelFor(x => x.OriginalDocumentsReceived)%>
                    </td>
                    <td>
                        <%= Html.BoundedSelectFor(x => x.OriginalDocumentsReceived, x => x.OriginalDocumentsOptions, ssClass) %>
                    </td>
                </tr>
                <tr>
                    <td class="labelsInside"><%= Html.TableLabelFor(x => x.MiddleName)%></td>
                    <td>
                        <%= Html.BoundedTextBoxFor(x => x.MiddleName) %>
                    </td>
                    <td colspan="4">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="6" style="text-align: center;">
                        <input type="submit" class="button primary" value="Найти" data-bind="click: applyFilter" />
                        <input type="button" class="button" value="Сбросить фильтр" data-bind="click: resetFilter" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</div>