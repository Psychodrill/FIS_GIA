<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.CompetitiveGroupEditViewModel>" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.ContextExtensions" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Register TagPrefix="gv" TagName="TabControl" Src="~/Views/Shared/Common/InstitutionsTabControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= Model.GroupID > 0 ? ( GVUZ.Helper.UrlUtils.IsReadOnly(FBDUserSubroles.CompetitiveGroupsDirection) ? "Просмотр" : "Редактирование")  : "Добавление"%> Конкурса
</asp:Content>
<asp:Content ID="header" ContentPlaceHolderID="PageTitle" runat="server">Сведения об образовательной организации</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="divViewOlympic" style="padding: 5px;display:none;position:absolute" class="ui-widget ui-widget-content ui-corner-all"></div>

<div class="divstatement">
<gv:tabcontrol runat="server" id="tabControl" />

<table class="data">
	<tbody>
		<tr>
			<td class="caption"><%= Html.TableLabelFor(x => x.Name) %></td>
			<td><%= Html.TextBoxExFor(m => m.Name, new { @class = "view", @readonly = "true" })%></td>
		</tr>
		<tr>
			<td class="caption"><%= Html.TableLabelFor(x => x.CampaignID) %></td>
			<td><b><%: Model.CampaignName %></b></td>
		</tr>
<%--		<tr>
			<td class="caption"><%= Html.TableLabelFor(x => x.EducationalLevelID) %></td>
			<td><b><%: Model.EducationLevelName %></b></td>
		</tr>--%>
		<tr>
			<td class="caption"><%= Html.TableLabelFor(x => x.CourseID) %></td>
			<td><b><%: CompetitiveGroupExtensions.GetCourseName(Model.CourseID) %></b></td>
		</tr>
		<tr>
			<td class="caption"><%= Html.LabelFor(x => x.Uid) %>:</td>
			<td><%= Html.TextBoxExFor(m => m.Uid, new {@class="view", @readonly="true"})%></td>
		</tr>
	</tbody>
</table>
<div>&nbsp;</div>

<div class="subdivstatement">

<div id="cgSubMenu" class="subsubmenu"></div>

<% Html.RenderAction("AddEntranceTest", "EntranceTest", new { groupID = Model.GroupID }); %>

</div>


</div>
<script type="text/javascript">

	function updateEntranceTestCount()
	{
		doPostAjax('<%= Url.Generate<EntranceTestController>(c => c.GetEntranceTestCount(null)) %>', 'groupID=<%= Model.GroupID %>', function (data)
		{
			if (!data.IsError)
			{
				var $el = jQuery('.subsubmenu a:eq(1)');
				jQuery('.subsubmenu a:eq(1)').html($el.html().replace(/\(\d+\)/, '(' + data.Data + ')'));
			}
		}, "application/x-www-form-urlencoded")
	}

	jQuery(document).ready(function()
	{
	    new TabControl(jQuery('#cgSubMenu'), [{ name: 'Специальности', link: '<%= Url.Generate<AdmissionController>(c => c.CompetitiveGroupEdit(Model.GroupID, Model.CampaignID)) %>', enable: true }
		//<% if(Model.GroupID > 0) { %>

			, { name: 'Вступительные испытания (<%= Model.EntranceTestCount %>)', link: '<%= Url.Generate<AdmissionController>(c => c.CompetitiveGroupEntranceTestEdit(Model.GroupID)) %>', enable: true, selected:true }
		//<%} %>
		])
			.init();
	})
	menuItems[5].selected = true;
</script>
<div id="divPopupDialog"></div>
<div id="divBenefitListDialog"></div>
<div id="divAddBenefit"></div>
<div id="divAddOlympicMultiple"></div>

</asp:Content>
