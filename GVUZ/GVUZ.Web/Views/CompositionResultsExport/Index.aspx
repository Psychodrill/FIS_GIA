<%@ Page Title="Title" Language="C#" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.CompositionResults.CompositionResultsListViewModel>" MasterPageFile="../Shared/Site.Master" %>

<%@ Register TagPrefix="gv" TagName="AdminMenuControl" Src="~/Views/Shared/Controls/AdminMenuControl.ascx" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
    Выгрузка сведений о сочинениях
</asp:Content>
<asp:Content ContentPlaceHolderID="PageTitle" runat="server">
    Выгрузка сведений о сочинениях
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageHeaderContent">
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/libs/knockout-3.3.0.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/libs/knockout.mapping-latest.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/WebUtils.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/grid/FilterViewModelBase.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/grid/PagerViewModel.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/grid/ListViewModelBase.js") %>"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
	<% ViewData["MenuItemID"] = 8; %>
    <div class="divstatement">
	<gv:AdminMenuControl runat="server" />
    <%Html.RenderPartial("CompositionList", Model); %>
    </div>
    
    <script type="text/javascript">
        $(function() {
            compositionResultsListLoaded();
        });
    </script>
</asp:Content>

