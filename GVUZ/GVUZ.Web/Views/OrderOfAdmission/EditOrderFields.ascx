<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.OrderOfAdmission.OrderOfAdmissionEditViewModel>" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<% var noAutoComplete = new { autocomplete = "off" }; %>

<%= Html.HiddenFor(m => m.OrderId) %>
<%= Html.HiddenFor(m => m.OrderTypeId) %>

<script language="javascript" type="text/javascript">
    var models = JSON.parse('<%= Html.Serialize(Model.Campaigns) %>');

    window.onload = function () {
        for (var i = 0; i < models.Items.length; i++) {
            if (models.Items[i].Id == $('#CampaignId').val()) {
                if (models.Items[i].Additional) {
                    $('#Stage').attr("disabled", true);
                    $('#Stage').val("");
                    break;
                } else {
                    $('#Stage').attr("disabled", false);
                }
            }
        }
    }

    function ClickCampId() {
        for (var i = 0; i < models.Items.length; i++) {
            if (models.Items[i].Id == parseInt($('#CampaignId').val())) {
                if (models.Items[i].Additional) {
                    $('#Stage').attr("disabled", true);
                    $('#Stage').val("");
                    break;
                } else {
                    $('#Stage').attr("disabled", false);
                }
            }
        }
    }
</script>

<table>
    <colgroup>
        <col style="width: 15%" />
        <col style="width: 20%" />
        <col style="width: 15%" />
        <col style="width: 50%" />
    </colgroup>
    <tbody>
        <tr>
            <td class="labelsInside">
                <%= Html.TableLabelFor(m => m.OrderName) %>    
            </td>
            <td>
                <% if (Model.IsPublished)
                   { %>
                <span><%= Model.OrderName %></span>
                <% }
                   else
                   { %>
                <%= Html.ValidatorTextBoxFor(m => m.OrderName, null, noAutoComplete) %>
                <% } %>
            </td>
            <td class="labelsInside">
                <%= Html.TableLabelFor(m => m.UID) %>
            </td>
            <td>
                <%= Html.TextBoxFor(m => m.UID) %>
            </td>
        </tr>
        <tr>
            <td class="labelsInside">
                <%= Html.TableLabelFor(m => m.OrderNumber) %>
            </td>
            <td>
                <% if (Model.IsPublished)
                   { %>
                <span><%= Model.OrderNumber %></span>
                <% }
                   else
                   { %>
                <%= Html.ValidatorTextBoxFor(m => m.OrderNumber, null, noAutoComplete)%>
                <% } %>
            </td>
            <td class="labelsInside">
                <%= Html.TableLabelFor(m => m.OrderDate) %>
            </td>
            <td>
                <% if (Model.IsPublished)
                   { %>
                <span><%= Model.OrderDateText %></span>
                <% }
                   else
                   { %>
                <%= Html.ValidatorTextBoxFor(m => m.OrderDate, Model.OrderDateText, new { @class = "shortInput datePicker", autocomplete = "off", maxlength = 10 })%>
                <% } %>
            </td>
        </tr>
        <tr>
            <td class="labelsInside">
                <%= Html.TableLabelFor(m => m.CampaignId) %>
            </td>
            <td>
                <% if (Model.IsNoApplications)
                   { %>
                <%= Html.CustomDropDownFor(m => m.CampaignId, Model.Campaigns, noAutoComplete) %>
                <% }
                   else
                   { %>
                <span><%= Model.CampaignName %></span>
                <% } %>
            </td>
            <td class="labelsInside">
                <%= Html.TableLabelFor(m => m.EducationLevelId) %>
            </td>
            <td>
                <% if (Model.IsNoApplications)
                   { %>
                <%= Html.CustomDropDownFor(m => m.EducationLevelId, Model.EducationLevels, noAutoComplete) %>
                <% }
                   else
                   { %>
                <span><%= Model.EducationLevelName %></span>
                <% } %>
            </td>
        </tr>
        <tr>
            <td class="labelsInside">
                <%= Html.TableLabelFor(m => m.EducationSourceId) %>
            </td>
            <td>
                <% if (Model.IsNoApplications)
                   { %>
                <%= Html.CustomDropDownFor(m => m.EducationSourceId, Model.EducationSource, noAutoComplete) %>
                <% }
                   else
                   { %>
                <span><%= Model.EducationSourceName %></span>
                <% } %>
            </td>
            <td class="labelsInside">
                <%= Html.TableLabelFor(m => m.EducationFormId) %>
            </td>
            <td>
                <% if (Model.IsNoApplications)
                   { %>
                <%= Html.CustomDropDownFor(m => m.EducationFormId, Model.EducationForms, noAutoComplete) %>
                <% }
                   else
                   { %>
                <span><%= Model.EducationFormName %></span>
                <% } %>
            </td>
        </tr>
        <tr>
            <td class="labelsInside">
                <%= Html.TableLabelFor(m => m.Stage) %>
            </td>
            <td>
                <% if (Model.IsNoApplications)
                   { %>
                <%= Html.CustomDropDownFor(m => m.Stage, Model.Stages, noAutoComplete) %>
                <% }
                   else
                   { %>
                <span><%= Model.StageName %></span>
                <% } %>
            </td>
            <td class="labelsInside"></td>
            <td></td>
        </tr>
        <tr>
            <td class="labelsInside">
                <%= Html.TableLabelFor(m => m.IsForBeneficiary) %>
            </td>
            <td>
                <%=Html.HiddenFor(m=>m.IsForBeneficiary) %>
                <span id="IsForBeneficiaryName"><%= Model.IsForBeneficiaryName %></span>
            </td>
            <td class="labelsInside">
                <%= Html.TableLabelFor(m => m.IsForeigner) %>
            </td>
            <td>
                <%=Html.HiddenFor(m=>m.IsForeigner) %>
                <span id="IsForeignerName"><%= Model.IsForeignerName %></span>
            </td>
        </tr>
        <tr>
            <td class="labelsInside">
                <%= Html.TableLabelFor(m => m.DateCreated) %>
            </td>
            <td>
                <span><%= Model.DateCreated %></span>
            </td>
            <td class="labelsInside">
                <%= Html.TableLabelFor(m => m.DateEdited) %>
            </td>
            <td>
                <span><%= Model.DateEdited %></span>
            </td>
        </tr>
        <tr>
            <td class="labelsInside">
                <%= Html.TableLabelFor(m => m.OrderStatus) %>
            </td>
            <td>
                <% if (Model.IsPublished)
                   { %>
                <%= Html.CustomDropDownFor(m => m.OrderStatus, Model.OrderStatusList, noAutoComplete) %>
                <% }
                   else
                   { %>
                <span><%= Model.OrderStatusName %></span>
               <% } %>
            </td>
            <td class="labelsInside">
                <%= Html.TableLabelFor(m => m.DatePublished) %>
            </td>
            <td>
                <span><%= Model.DatePublished %></span>
            </td>
        </tr>
    </tbody>
</table>
