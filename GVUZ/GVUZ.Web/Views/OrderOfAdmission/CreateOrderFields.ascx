<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.OrderOfAdmission.OrderOfAdmissionCreateViewModel>" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%
    var noAutoComplete = new { autocomplete = "off" };     
%>

<%= Html.HiddenFor(m=>m.OrderTypeId) %>

<script language="javascript" type="text/javascript">
    var models = JSON.parse('<%= Html.Serialize(Model.Campaigns) %>');

    window.onload = function () {
        for (var i = 0; i < models.Items.length; i++) {
            if (models.Items[i].Id == $('#CampaignId').val()) {
                if (models.Items[i].Additional) {
                    $('#Stage').attr("disabled", true);
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
                <%= Html.ValidatorTextBoxFor(m => m.OrderName, null, noAutoComplete)%>
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
                <%= Html.ValidatorTextBoxFor(m => m.OrderNumber, null, noAutoComplete)%>
            </td>
            <td class="labelsInside">
                <%= Html.TableLabelFor(m => m.OrderDate) %>
            </td>
            <td>
                <%= Html.ValidatorTextBoxFor(m => m.OrderDate, Model.OrderDateText, new { @class = "shortInput datePicker", autocomplete="off", maxlength = 10 })%>
            </td>
        </tr>
        <tr>
            <td class="labelsInside" style="white-space: nowrap">
                <%= Html.TableLabelFor(m => m.CampaignId) %><span class="required">*</span>
            </td>
            <td>
                <% if (Model.FromApplication)
                   { %>
                <%= Model.SingleCampaignName %>
                <%= Html.HiddenFor(m => m.CampaignId) %>
                <% }
                   else
                   { %>
                <%= Html.CustomDropDownFor(m => m.CampaignId, Model.Campaigns, new { onchange="ClickCampId();", autocomplete = "off" }) %>
                <% } %>
            </td>
            <td class="labelsInside">
                <%= Html.TableLabelFor(m => m.EducationLevelId) %>
            </td>
            <td>
                <%= Html.CustomDropDownFor(m => m.EducationLevelId, Model.EducationLevels, noAutoComplete)%>
            </td>
        </tr>
        <tr>
            <td class="labelsInside">
                <%= Html.TableLabelFor(m => m.EducationSourceId) %>
            </td>
            <td>
                <%= Html.CustomDropDownFor(m => m.EducationSourceId, Model.EducationSource, noAutoComplete)%>
            </td>
            <td class="labelsInside">
                <%= Html.TableLabelFor(m => m.EducationFormId) %>
            </td>
            <td>
                <%= Html.CustomDropDownFor(m => m.EducationFormId, Model.EducationForms, noAutoComplete)%>
            </td>
        </tr>
        <tr>
            <td class="labelsInside">
                <%= Html.TableLabelFor(m => m.Stage) %>
            </td>
            <td>
                <%= Html.CustomDropDownFor(m => m.Stage, Model.Stages)%>
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
    </tbody>
</table>
