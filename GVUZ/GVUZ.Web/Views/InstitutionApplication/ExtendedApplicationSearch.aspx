<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.ApplicationsList.SearchApplicationsListViewModel>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">Расширенный поиск заявлений</asp:Content>
<asp:Content ContentPlaceHolderID="PageHeaderContent" runat="server">
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/libs/knockout-3.3.0.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/libs/knockout.mapping-latest.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/WebUtils.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/grid/FilterViewModelBase.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/grid/PagerViewModel.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/grid/ListViewModelBase.js") %>"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="PageTitle" runat="server">Расширенный поиск заявлений</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="padding:0; margin:0; border:0;clear: both">
    <% Html.RenderPartial("InstitutionApplication/SearchList", Model); %>
    </div>
<script type="text/javascript">
    (function ($) {
        $(function () {
            searchListLoaded();
        });
    })(jQuery);
    
</script>
</asp:Content>
