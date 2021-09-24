<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.ApplicationV0Model>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<script type="text/javascript" language="javascript">
</script>
<div id="content">
    <table class="data">
        <tbody>
            <tr>
                <td class="caption">
                    <%= Html.TableLabelFor(x => x.StatusName)%>
                </td>
                <td>
                    <%:Model.StatusName %>
                </td>
            </tr>
            <tr>
                <td class="caption">
                    <%= Html.TableLabelFor(x => x.ViolationName)%>
                </td>
                <td>
                    <%:Model.ViolationName %>
                </td>
            </tr>
            <tr>
                <td class="caption">
                    <%= Html.TableLabelFor(x => x.InstitutionName)%>
                </td>
                <td>
                    <%:Model.InstitutionName %>
                </td>
            </tr>
            <tr>
                <td class="caption">
                    <%= Html.TableLabelFor(x => x.SForms.DirectionName)%>
                </td>
                <td>
                    <%foreach (var sf in Model.listESC)
                      {%>
                        <%: sf.DirectionName%>
                    <%} %>
                </td>
            </tr>
            <tr>
                <td class="caption">
                    <%= Html.TableLabelFor(x => x.SForms.Course)%>
                </td>
                <td>
                    <%foreach (var sf in Model.listESC)
                      {%>
                    <%= sf.Course %>
                    <%break; %>
                    <%} %>Курс
                </td>
            </tr>
            <tr>
                <td class="caption">
                    <%= Html.TableLabelFor(x => x.SForms.EduLevelName)%>
                </td>
                <td>
                    <%foreach (var sf in Model.listESC)
                      {%>
                    <%: sf.EduLevelName%>
                    <%} %>
                </td>
            </tr>
            <tr>
                <td class="caption">
                    <%= Html.TableLabelFor(x => x.Priorities)%>
                </td>
                <td>
                    <% foreach (var priority in Model.Priorities.ApplicationPriorities){ %>
                       
                        <%=priority.EduFormAndSource %><br />
                    
                    <%   }%>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="caption">
                    <b>Сведения о конкурсе:</b>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <% foreach (var cg in Model.CompetitiveGroup)
               { %>
            <tr>
                <td class="caption">
                    <%= Html.TableLabelFor(m => cg.CompetitiveGroupName)%>
                </td>
                <td>
                    <%= cg.CompetitiveGroupName %>
                </td>
            </tr>
            <tr>
                <td class="caption">
                    <%= Html.TableLabelFor(m => cg.Competition)%>
                </td>
                <td>
                    <%
                   cg.Competition = Math.Round(cg.Requests.Value / cg.Places.Value, 2);
                   if (Double.IsInfinity(cg.Competition))
                   {
                    %>
                    -
                    <%}
                   else
                   { %>
                    <%: cg.Competition %>
                    <%
                        }
                    %>
                </td>
            </tr>
            <tr>
                <td class="caption">
                    Образовательные программы:
                </td>
                <td>
                    <%: Model.Priorities.ApplicationPriorities.Where(p => p.CompetitiveGroupId == cg.CompetitiveGroupID).FirstOrDefault().CompetitiveGroupProgramRow%>
                </td>
            </tr>
            <tr>
                <td class="caption">
                    Уровень бюджета:
                </td>
                <td>
                    <%: Model.Priorities.ApplicationPriorities.Where(p => p.CompetitiveGroupId == cg.CompetitiveGroupID).FirstOrDefault().LevelBudgetRow%>
                </td>
            </tr>
            <tr>
                <td class="caption">
                    <%= Html.TableLabelFor(m => cg.Places)%>
                </td>
                <td>
                    <%: cg.Places%>
                </td>
            </tr>
            <tr>
                <td class="caption">
                    <%= Html.TableLabelFor(m => cg.Requests)%>
                </td>
                <td>
                    <%: cg.Requests%>
                </td>
            </tr>
            <tr>
                <td class="caption">
                    <%= Html.TableLabelFor(m => cg.Points)%>
                </td>
                <td>
                    <%: cg.Points%>
                </td>
            </tr>
            <tr>
                <td class="caption">
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <% } %>
        </tbody>
    </table>
</div>
