<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Register TagPrefix="gv" TagName="TabControl" Src="~/Views/Shared/Common/InstitutionsTabControl.ascx" %>
<%@ Register TagPrefix="gv" TagName="CompetitiveGroupView" Src="~/Views/Shared/Admission/CompetitiveGroupView.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Просмотр конкурса
</asp:Content>
<asp:Content ID="header" ContentPlaceHolderID="PageTitle" runat="server">Сведения об образовательной организации</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="divstatement">
<gv:tabcontrol runat="server" id="tabControl" />
<gv:CompetitiveGroupView runat="server" />
</div>
<script type="text/javascript">
	menuItems[5].selected = true;
</script>
</asp:Content>
