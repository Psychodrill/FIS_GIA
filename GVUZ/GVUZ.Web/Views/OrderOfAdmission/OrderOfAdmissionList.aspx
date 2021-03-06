<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.OrderOfAdmission.OrderOfAdmissionListViewModel>" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">Приказы</asp:Content>
<asp:Content ContentPlaceHolderID="PageHeaderContent" runat="server">
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/libs/knockout-3.3.0.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/libs/knockout.mapping-latest.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/WebUtils.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/grid/FilterViewModelBase.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/grid/PagerViewModel.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/grid/ListViewModelBase.js") %>"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="PageTitle" runat="server">Приказы</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="divstatement">
        <div class="gvuzTab submenu" id="filterTab" style="display: block;">
            <a class="menuitemr menuiteml <%=Model.Filter.OrderTypeId==GVUZ.Data.Model.OrderOfAdmissionType.OrderOfAdmission?"select":"" %>" href="<%=Url.Action("OrdersOfAdmissionList") %>">Приказы о зачислении</a>
            <a class="menuitemr <%=Model.Filter.OrderTypeId==GVUZ.Data.Model.OrderOfAdmissionType.OrderOfAdmissionRefuse?"select":"" %>" href="<%=Url.Action("OrdersOfAdmissionRefuseList") %>">Приказы об отказе от зачисления</a>
        </div> 
        <div style="padding:0; margin:0; border:0;clear: both">
        <% Html.RenderPartial("OrderOfAdmission/OrderList", Model); %>
        </div>
    </div>
<script type="text/javascript">
    (function ($) {
        $(function () {
            orderOfAdmissionListLoaded();
        });
    })(jQuery);
</script>
</asp:Content>
