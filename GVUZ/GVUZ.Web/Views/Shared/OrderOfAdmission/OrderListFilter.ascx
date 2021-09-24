<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.OrderOfAdmission.OrderOfAdmissionFilterViewModel>" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<% 
    var ssClass = new { style = "width: 340px" };
    var labelsInsideClass = new {@class = "labelsInside"};
%>

<div style="white-space: nowrap;padding: 8px;padding-left: 10px">
    <label for="quickSelectCampaign"><%= Html.DisplayNameFor(x => x.SelectedCampaign) %>:</label>
    &nbsp;
    <%= Html.BoundedSelectFor(x => x.SelectedCampaign, x => x.Campaigns, new {@class = "ss", style="width: 220px;", id = "quickSelectCampaign"}) %>
    &nbsp;
    <label for="quickSelectStage"><%= Html.DisplayNameFor(x => x.SelectedStage) %>:</label>
    &nbsp;
    <%= Html.BoundedSelectFor(x => x.SelectedStage, x => x.Stages, new { @class = "ss", style="width: 180px;", id = "quickSelectStage" })%>
</div>

<div class="tableHeader5l" style="height: auto;" data-bind="css: {tableHeaderCollapsed: !isVisible()}">
    <div>
        <div class="hideTable" style="float: left;" data-bind="css: {nonHideTable: isVisible()}, click: toggleVisible">
            <span data-bind="text: visibleText"></span>
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
                <col style="width: 25%"/>
            </colgroup>
            <tbody>
                <tr>
                    <td class="labelsInside">
                        <%= Html.TableLabelFor(x => x.OrderName) %>
                    </td>
                    <td>
                        <%= Html.BoundedTextBoxFor(x => x.OrderName) %>
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
                        <%= Html.TableLabelFor(x => x.OrderNumber) %>
                    </td>
                    <td>
                        <%= Html.BoundedTextBoxFor(x => x.OrderNumber) %>
                    </td>
                    <td class="labelsInside">
                        <%= Html.TableLabelFor(x => x.SelectedEducationForm) %>
                    </td>
                    <td>
                        <%= Html.BoundedSelectFor(x => x.SelectedEducationForm, x => x.EducationForms, ssClass)%>
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
                        <%=Html.TableLabelFor(x => x.OrderDateFrom) %>
                    </td>
                    <td>
                        <%= Html.BoundedTextBoxFor(x => x.OrderDateFrom, new {@class="shortInput datePicker", maxlength=10}) %>&nbsp;&nbsp;
                        <%= Html.TableLabelFor(x => x.OrderDateTo, labelsInsideClass)%>
                        <%= Html.BoundedTextBoxFor(x => x.OrderDateTo, new { @class = "shortInput datePicker", maxlength = 10 })%>
                    </td>
                    <td rowspan="2" class="labelsInside">
                        <%= Html.TableLabelFor(x => x.IsForBeneficiary) %>
                    </td>
                    <td rowspan="2">
                        <%= Html.BoundedSelectFor(x => x.IsForBeneficiary, x => x.IsForBeneficiaryList, ssClass) %>
                    </td>
                    <td rowspan="2" class="labelsInside">
                        <%= Html.TableLabelFor(x => x.IsForeigner) %>
                    </td>
                    <td rowspan="2">
                        <%= Html.BoundedSelectFor(x => x.IsForeigner, x => x.IsForeignerList, ssClass) %>
                    </td>
                </tr>
                <tr>
                    <td class="labelsInside">
                        <%=Html.TableLabelFor(x => x.OrderPublishDateFrom) %>
                    </td>
                    <td>
                        <%= Html.BoundedTextBoxFor(x => x.OrderPublishDateFrom, new { @class = "shortInput datePicker", maxlength = 10 })%>&nbsp;&nbsp;
                        <%= Html.TableLabelFor(x => x.OrderPublishDateTo, labelsInsideClass)%>
                        <%= Html.BoundedTextBoxFor(x => x.OrderPublishDateTo, new { @class = "shortInput datePicker", maxlength = 10 })%>
                    </td>
                </tr>
                <tr>
                    <td class="labelsInside">
                        <%= Html.TableLabelFor(x => x.SelectedOrderStatus) %>
                    </td>
                    <td>
                        <%= Html.BoundedSelectFor(x => x.SelectedOrderStatus, x => x.OrderStatuses, ssClass) %>
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