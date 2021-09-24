<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.InstitutionApplicationListViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.ViewModels" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Незаконченные заявления
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div>&nbsp;</div>
	<div id="content">
	<% if (Model.CanCreateNewApplication)
						 { %>
	<div id="divPublish"><a id="btnCreateNewApp" href="<%= Url.Generate<InstitutionApplicationDraftController>(c => c.PrepareNewApplication()) %>" >Создать новое</a><div>&nbsp;</div></div> <%-- Добавить выбор структуры --%>
		<table class="gvuzDataGrid" cellpadding="3">
			<thead>
				<tr>
					<th>
						<%= Html.LabelFor(x => x.AppDescr.ApplicationNumber) %>
					</th>
					<th>
						<%= Html.LabelFor(x => x.AppDescr.EntrantFIO)%>
					</th>
					<th>
						<%= Html.LabelFor(x => x.AppDescr.EntrantDocData) %>
					</th>
					<th>
						<%= Html.LabelFor(x => x.AppDescr.ApplicationDate) %>
					</th>
					<th id="thAction">
						<%= Html.LabelFor(x => x.AppDescr.ApplicationID) %>
					</th>
				</tr>
			</thead>
			<tbody>
				<% foreach (var app in Model.Applications) { %>
				<tr>
					<td align="center"><%: app.ApplicationNumber %></td>
					<td><%: app.EntrantFIO %></td>
					<td><%: app.EntrantDocData %></td>
					<td><%: app.ApplicationDate %></td>
					<td align="center"><a href="<%= Url.Generate<ApplicationController>(x => x.ApplicationPersonalDataByApp(app.ApplicationID, null)) %>" class="btnEdit" title="Редактировать заявление"></a></td>
				</tr>
<% } %>
			</tbody>
		</table>
<%} else { %><div>Структура приема не опубликована. Невозможно подать заявление.</div>
<%} %>
	</div>
<script language="javascript" type="text/javascript">

	jQuery(document).ready(function ()
	{
		jQuery('#btnCreateNewApp').button()
	})
</script>

</asp:Content>
