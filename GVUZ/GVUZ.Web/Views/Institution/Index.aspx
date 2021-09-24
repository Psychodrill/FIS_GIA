<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.InstituteCommonInfoViewModel>" 
 MasterPageFile="~/Views/Shared/Site.Master" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.Security" %>

<asp:Content ID="registerTitle" ContentPlaceHolderID="TitleContent" runat="server">
	Просмотр общих сведений
</asp:Content>
<asp:Content ID="header" ContentPlaceHolderID="PageTitle" runat="server">Сведения об образовательной организации</asp:Content>

<%@ Register TagPrefix="gv" TagName="TabControl" Src="~/Views/Shared/Common/InstitutionsTabControl.ascx" %>
<%@ Register TagPrefix="gv" TagName="CommonInfoControl" Src="~/Views/Shared/Common/CommonInfoControl.ascx" %>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
	<div class="divstatement">
		<gv:tabcontrol runat="server" id="tabControl" />	
		<% if (Model.ChangeHistories != null && Model.ChangeHistories.Cast<object>().Count() > 1)
		{ %>
		<div>
			<%= Html.LabelFor(x => x.HistoryID)%>: <%= Html.DropDownListExFor(x => x.HistoryID, Model.ChangeHistories, new { onchange = "instHistoryChange()" })%>
		</div>
		<%} %>
		<gv:CommonInfoControl ID="commonInfo" runat="server" />
        <% if (!GVUZ.Helper.UrlUtils.IsReadOnly(FBDUserSubroles.InstitutionDataDirection)) { %>
		<div>
			<%= Url.GenerateNavLink<InstitutionController>(c => c.Edit(), "Редактировать", UserRole.AdminUsrEduRole, UserRole.FBDReadonly + "," + UserRole.FbdRonUser) %>
		</div>
        <%} %>
	</div>
	<script type="text/javascript">
		menuItems[0].selected = true;
		function instHistoryChange() {
			var histID = jQuery('#HistoryID').val();
			window.location = '<%= Url.Generate<InstitutionController>(x => x.View(null)) %>?historyID=' + histID;
		}
	</script>
</asp:Content>

