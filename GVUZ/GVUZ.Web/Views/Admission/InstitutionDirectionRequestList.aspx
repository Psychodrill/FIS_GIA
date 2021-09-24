<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.InstitutionDirectionRequestListViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Register TagPrefix="gv" TagName="AdminMenuControl" Src="~/Views/Shared/Controls/AdminMenuControl.ascx" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
    Список ОО
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PageTitle" runat="server">
	Список ОО
</asp:Content>

<asp:Content ID="PageHeaderContent" ContentPlaceHolderID="PageHeaderContent" runat="server">
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/libs/knockout-3.3.0.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/libs/knockout.mapping-latest.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/WebUtils.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/grid/FilterViewModelBase.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/grid/PagerViewModel.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/grid/ListViewModelBase.js") %>"></script>

<style type="text/css">
	textarea{
	    resize:none;
	}
</style>

</asp:Content>


<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

	<div class="divstatement">
	<% ViewData["MenuItemID"] = 6; %>
	<gv:AdminMenuControl ID="AdminMenuControl1" runat="server" />	
        
		<div class="tableHeader5l" id="divFilterRegion" style="display:none">	
	<div id="divFilterPlace">
		<div class="hideTable nonHideTable" style="float:left"><span style="cursor:default;">Расширенный поиск</span></div>
		<div id="spAppCount" class="appCount">Количество заявлений: <span id="spAppCountField"></span></div>
	</div>

	</div>

		<div id="content">
			<% Html.RenderPartial("../Admission/DirectionRequestGrid", Model, ViewContext.ViewData); %>
		</div>
	</div>

	<script language="javascript" type="text/javascript">

	    (function ($) {
	        $(function () {
	            requestListLoaded();
	        });
	    })(jQuery);
	</script>

    <% Html.RenderPartial("../Admission/DirectionRequestDetails", GVUZ.Web.ViewModels.InstitutionDirectionRequestDetailsViewModel.DefaultInstance); %>

</asp:Content>

