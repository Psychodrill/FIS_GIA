<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.ApplicationPriorityViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

<tr data-value="<%: Model.CompetitiveGroupId%>" data-unlimitedAgreements="<%=Model.UnlimitedAgreements %>">
    <td><%: Model.LevelName %></td>
    <td><%: Model.CompetitiveGroupName %></td>
    <%--<td><%: Model.CompetitiveGroupItemName %></td>--%>
    <td>
        <p><%: Model.CompetitiveGroupItemName %></p>
        <p><%: Model.EducationFormName %></p>
        <p><%: Model.EducationSourceName %>
        <%if (Model.AllowSourceSelect){%><%= Html.DropDownListFor(m => m.CompetitiveGroupTargetId, Model.Sources)%><%}%>
        </p>
    </td>
<%--    <td><%: Model.Programs %></td>
    <td><%: Model.EducationFormName %></td>

    <td><%: Model.EducationSourceName %>
        <%if (Model.AllowSourceSelect){%><%= Html.DropDownListFor(m => m.CompetitiveGroupTargetId, Model.Sources)%><%}%>
    </td>--%>

    <td><%: Model.CompetitiveGroupProgramRow %></td>
    <td><%: Model.LevelBudgetRow %></td>
    <%--<td>
        <%if (Model.CompetitiveGroupProgram != null)
        {
            foreach (var pr in Model.CompetitiveGroupProgram)
            { %>
                <p><%= pr.Code + ' ' + pr.Name %></p>
            <% }
        } %>
    </td>
    <td>
        <%if (Model.LevelBudget != null)
        {
            foreach (var lev in Model.LevelBudget)
            { %>
                <p><%= lev.BudgetName %></p>
            <% }
        } %>
    </td>--%>
    <td class="spo_vo"><%= Html.CheckBox("IsForSPOandVO", Model.IsForSPOandVO.GetValueOrDefault()) %></td>

    <td><%= Html.CheckBox("IsAgreed", Model.IsAgreed.GetValueOrDefault(), new { onClick = "if(CheckManyAgreed($(this))) ModifyControls(); else return false;" }) %></td>
    <td><%= Html.TextBox("IsAgreedDate_"+Model.CompetitiveGroupId, Model.IsAgreedDate, new { @class = "datePicker IsAgreedDate", @style = "width:100px"}) %></td>
    <td><%= Html.CheckBox("IsDisagreed", Model.IsDisagreed.GetValueOrDefault(), new { onClick = "if(CheckManyAgreed($(this))) ModifyControls(); else return false;" }) %></td>
    <td><%= Html.TextBox("IsDisagreedDate_"+Model.CompetitiveGroupId, Model.IsDisagreedDate, new { @class = "datePicker IsDisagreedDate", @style = "width:100px"}) %></td>
    
    <td class="editor_conditions"><a class='delete' href='#' onclick='DeleteConditionRow(this); return false;'></a></td>

    <td hidden='hidden'><%= Html.Hidden("EducationFormId", Model.EducationFormId) %></td>
    <td hidden='hidden'><%= Html.Hidden("EducationSourceId", Model.EducationSourceId) %></td>
    <td hidden='hidden'><%= Html.Hidden("CalculatedRating", Model.CalculatedRating) %></td>
    <td hidden='hidden'><%= Html.Hidden("CompetitiveGroupTargetId", Model.CompetitiveGroupTargetId) %></td>
    <td hidden='hidden'><%= Html.Hidden("AllowEdit", Model.AllowEdit) %></td> 
</tr>