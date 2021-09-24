<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.Portlets.Applications.ApplicationCommonInfoViewModel>" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Controllers.Admission" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

<div id="content">
	<table class="data">
		<tbody>
			<tr>
				<td class="caption"><b><%= Html.TableLabelFor(m => m.Status)%></b></td>
				<td><%: Model.Status%></td>
			</tr>
			<tr>
				<td class="caption"><%= Html.TableLabelFor(m => m.Violation)%></td>
				<td><%: Model.Violation%></td>
			</tr>
			<tr>
				<td class="caption"><%= Html.LabelFor(m => m.Institution, ViewData["Institution"].ToString())%>:</td>
				<td><%: Model.Institution %></td>		
			</tr>
			<tr>
				<td class="caption"><%= Html.TableLabelFor(m => m.Direction)%></td>
				<td><%: Model.Direction%></td>
			</tr>
			<tr>
				<td class="caption"><%= Html.TableLabelFor(m => m.Course)%></td>
				<td><%: Model.Course%></td>
			</tr>
			<tr>
				<td class="caption"><%= Html.TableLabelFor(m => m.EducationLevel)%></td>
				<td><%: Model.EducationLevel%></td>
			</tr>
			<tr>
				<td class="caption"><%= Html.TableLabelFor(m => m.EducationalFormList)%></td>
				<td><%: Model.EducationalFormList%></td>
			</tr>
			<%--<tr>
				<td class="caption"><%= Html.TableLabelFor(m => m.Olympics)%></td>
				<td><%: Model.Olympics%></td>
			</tr>--%>
			<tr>
				<td>&nbsp;</td>
				<td>&nbsp;</td>
			</tr>
			<tr>
				<td class="caption"><b>Сведения о конкурсе:</b></td>
				<td>&nbsp;</td>
			</tr>
			<% foreach (var cg in Model.CompetitiveGroups)
			   { %>
			   	
			<tr>
				<td class="caption">										
					<%= Html.TableLabelFor(m => cg.CompetitiveGroupName)%>
				</td>
				<td>
					<span class="linkSumulator" onclick="doViewCompetitiveGroup(<%= cg.CompetitiveGroupID %>)"><%: cg.CompetitiveGroupName%></span>
				</td>
			</tr>
			<tr>
				<td class="caption"><%= Html.TableLabelFor(m => cg.Competition)%></td>
				<td><%: cg.Competition.ToString("0.##") %></td>
			</tr>
			<tr>
				<td class="caption"><%= Html.TableLabelFor(m => cg.Places)%></td>
				<td><%: cg.Places%></td>
			</tr>
			<tr>
				<td class="caption"><%= Html.TableLabelFor(m => cg.Requests)%></td>
				<td><%: cg.Requests%></td>
			</tr>
			<tr>
				<td class="caption"><%= Html.TableLabelFor(m => cg.Points)%></td>
				<td><%: cg.Points%></td>
			</tr>
			<tr>
				<td class="caption">&nbsp;</td>
				<td>&nbsp;</td>
			</tr>

			<%--<tr>
				<td class="caption"><%= Html.TableLabelFor(m => cg.Rate)%></td>
				<td><%: cg.Rate%></td>
			</tr>
			<tr>
				<td class="caption"><%= Html.TableLabelFor(m => cg.EnrollmentOrder)%></td>
				<td><%: cg.EnrollmentOrder%></td>
			</tr>--%>
						<% } %>
		</tbody>
	</table>
</div>
<div id="divCompetitiveGroupView"></div>
<script type="text/javascript">
	function doViewCompetitiveGroup(cgID)
	{
		doPostAjax('<%= Url.Generate<AdmissionController>(x => x.CompetitiveGroupViewPopup(null)) %>?groupID=' + cgID, '', function (data)
		{
			jQuery('#divCompetitiveGroupView').html(data);
			jQuery('#divCompetitiveGroupView').dialog({
				modal: true,
				width: 900,
				title: 'Конкурс',
				buttons:
							{
								"Закрыть": function () { jQuery(this).dialog('close') }
							}
			}).dialog('open');
		}, "application/x-www-form-urlencoded", "html")
		
	}
</script>