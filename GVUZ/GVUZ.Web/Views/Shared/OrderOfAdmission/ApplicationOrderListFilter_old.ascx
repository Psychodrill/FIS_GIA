<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.OrderOfAdmission.ApplicationOrderFilterViewModel>" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<% 
    var ssClass = new { style = "width: 340px" };
    var labelsInsideClass = new {@class = "labelsInside"};
%>

<div class="tableHeader5l" style="height: auto;" data-bind="css: {tableHeaderCollapsed: !isVisible()}">
    <div>
        <div class="hideTable" style="float: left;" data-bind="css: {nonHideTable: isVisible()}, click: toggleVisible">
            <span data-bind="text: visibleText"></span>
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
                <col style="width: 60%"/>
            </colgroup>
            <tbody>
                <tr>
                    <td class="labelsInside">
                        <%= Html.TableLabelFor(x => x.ApplicationNumber) %>
                    </td>
                    <td>
                        <%= Html.BoundedTextBoxFor(x => x.ApplicationNumber) %>
                    </td>
                    <td class="labelsInside">
                        <%= Html.TableLabelFor(x => x.SelectedEducationLevel) %>
                    </td>
                    <td>
                        <%= Html.BoundedSelectFor(x => x.SelectedEducationLevel, x => x.EducationLevels, ssClass) %>
                    </td>
                </tr>
                <tr>
                    <td class="labelsInside">
                        <%= Html.TableLabelFor(x => x.LastName) %>
                    </td>
                    <td>
                        <%= Html.BoundedTextBoxFor(x => x.LastName)%>
                    </td>
                    <td class="labelsInside">
                        <%= Html.TableLabelFor(x => x.SelectedEducationForm) %>
                    </td>
                    <td>
                        <%= Html.BoundedSelectFor(x => x.SelectedEducationForm, x => x.EducationForms, ssClass)%>
                    </td>
                </tr>
                <tr>
                    <td class="labelsInside">
                        <%= Html.TableLabelFor(x => x.FirstName) %>
                    </td>
                    <td>
                        <%= Html.BoundedTextBoxFor(x => x.FirstName)%>
                    </td>
                    <td class="labelsInside">
                        <%= Html.TableLabelFor(x => x.SelectedEducationSource) %>
                    </td>
                    <td>
                        <%= Html.BoundedSelectFor(x => x.SelectedEducationSource, x => x.EducationSources, ssClass)%>
                    </td>
                </tr>
                <tr>
                    <td class="labelsInside">
                        <%=Html.TableLabelFor(x => x.MiddleName) %>
                    </td>
                    <td>
                        <%= Html.BoundedTextBoxFor(x => x.MiddleName) %>
                    </td>
                    <td lass="labelsInside">
                        <%= Html.TableLabelFor(x => x.SelectedCompetitiveGroup) %>
                    </td>
                    <td>
                        <%= Html.BoundedSelectFor(x => x.SelectedCompetitiveGroup, x => x.CompetitiveGroups, ssClass) %>
                    </td>
                </tr>
                <tr>
                    <td rowspan="2" valign="top" style="vertical-align: top" class="labelsInside">
                        Документ, удостоверяющий личность
                    </td>
                    <td rowspan="2" valign="top" style="white-space: nowrap;vertical-align: top">
                        <%= Html.TableLabelFor(x => x.DocumentSeries)%>
                        <%= Html.BoundedTextBoxFor(x => x.DocumentSeries, new {style="width: 50px"}) %>
                        &nbsp;
                        <%= Html.TableLabelFor(x => x.DocumentNumber, labelsInsideClass)%> 
                        <%= Html.BoundedTextBoxFor(x => x.DocumentNumber, new {style="width: 150px"}) %>
                    </td>
                    <td class="labelsInside">
                        <%=Html.TableLabelFor(x => x.DirectionName) %>
                    </td>
                    <td>
                        <%= Html.BoundedTextBoxFor(x => x.DirectionName)%>
                    </td>
                </tr>
                <tr>
                    <td class="labelsInside">
                        <%=Html.TableLabelFor(x => x.SelectedBenefit) %>
                    </td>
                    <td>
                        <%= Html.BoundedSelectFor(x => x.SelectedBenefit, x => x.Benefits, ssClass) %>
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