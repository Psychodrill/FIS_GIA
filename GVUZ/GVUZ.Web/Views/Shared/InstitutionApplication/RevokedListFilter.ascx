<%--Отрисовка фильтра отозванных заявлений для data-bound модели (knockoutjs)--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.ApplicationsList.RevokedApplicationsFilterViewModel>" %>
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
        <div class="appCount no-filter" data-bind="visible: isDefault()">
            Записей:&nbsp;<span data-bind="text: $root.filterStats"></span>
        </div>
        <div class="appCount filter" data-bind="visible: !isDefault()">
            Записей с фильтром:&nbsp;<span data-bind="text: $root.filterStats"></span>
        </div>
        <% } %>
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
                    <td class="labelsInside" width="10%">
                        <%= Html.TableLabelFor(x => x.ApplicationNumber) %>
                    </td>
                    <td width="23%">
                        <%= Html.BoundedTextBoxFor(x => x.ApplicationNumber, new {style = "width: 326px"}) %>
                    </td>
                    <td width="10%">
                        <%= Html.TableLabelFor(x => x.RegistrationDateFrom, labelsInsideClass)%>
                    </td>
                    <td width="23%">
                        <%= Html.BoundedTextBoxFor(x => x.RegistrationDateFrom, new {@class="shortInput datePicker", maxlength=10}) %>&nbsp;&nbsp;
                        <%= Html.TableLabelFor(x => x.RegistrationDateTo, labelsInsideClass)%>
                        <%= Html.BoundedTextBoxFor(x => x.RegistrationDateTo, new {@class="shortInput datePicker", maxlength=10}) %>
                    </td>
                    <td class="labelsInside" width="10%">
                        <%= Html.TableLabelFor(x => x.SelectedBenefitId)%>
                    </td>
                    <td width="23%">
                        <%= Html.BoundedSelectFor(x => x.SelectedBenefitId, x => x.Benefits, ssClass) %>
                    </td>
                </tr>
                <tr>
                    <td class="labelsInside">
                        <%= Html.TableLabelFor(x => x.OriginalDocumentsReceived)%>
                    </td>
                    <td>
                        <%= Html.BoundedSelectFor(x => x.OriginalDocumentsReceived, x => x.OriginalDocumentsOptions, ssClass) %>
                    </td>
                    <td class="labelsInside">
                        <%= Html.TableLabelFor(x => x.SelectedCompetitiveGroup)%>
                    </td>
                    <td>
                        <%= Html.BoundedSelectFor(x => x.SelectedCompetitiveGroup, x => x.CompetitiveGroups, ssClass) %>
                    </td>

                    <td class="labelsInside">
                        <%= Html.TableLabelFor(x => x.SelectedCampaignId) %>
                    </td>
                    <td>
                        <%= Html.BoundedSelectFor(x => x.SelectedCampaignId, x => x.Campaigns, ssClass) %>
                    </td>

                </tr>
                <tr>
                    <td class="labelsInside">
                        <%= Html.TableLabelFor(x => x.LastName)%>
                    </td>
                    <td>
                        <%= Html.BoundedTextBoxFor(x => x.LastName, new {style = "width: 326px"}) %>
                    </td>
                    <td class="labelsInside">
                        <%= Html.TableLabelFor(x => x.FirstName)%>
                    </td>
                    <td>
                        <%= Html.BoundedTextBoxFor(x => x.FirstName, new {style = "width: 326px"}) %>
                    </td>
                    <td class="labelsInside">
                        <%= Html.TableLabelFor(x => x.SelectedEducationFormType) %>
                    </td>
                    <td>
                        <select style= "width: 340px" data-bind="value: SelectedEducationFormType, options: EducationFormTypes.Items, optionsCaption: EducationFormTypes.UnselectedText, optionsValue: 'Id', optionsText: 'DisplayName'"></select>
                    </td>
                </tr>
                <tr>
                    <td class="labelsInside"><%= Html.TableLabelFor(x => x.MiddleName)%></td>
                    <td>
                        <%= Html.BoundedTextBoxFor(x => x.MiddleName, new {style = "width: 326px"}) %>
                    </td>
                    <td class="labelsInside">
                        <%= Html.TableLabelFor(x => x.DocumentSeries)%>
                    </td>
                    <td style="white-space: nowrap">
                        <%= Html.BoundedTextBoxFor(x => x.DocumentSeries, new {style="width: 50px"}) %>
                        &nbsp;
                        <%= Html.TableLabelFor(x => x.DocumentNumber, labelsInsideClass)%> 
                        <%= Html.BoundedTextBoxFor(x => x.DocumentNumber, new { style = "width: 137px" })%>
                    </td>
                    
                    <td class="labelsInside">
                        <%= Html.TableLabelFor(x => x.SelectedEducationSourceType) %>
                    </td>
                    <td>
                        <select style= "width: 340px" data-bind="value: SelectedEducationSourceType, options: EducationSourceTypes.Items, optionsCaption: EducationSourceTypes.UnselectedText, optionsValue: 'Id', optionsText: 'DisplayName'"></select>
                    </td>
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