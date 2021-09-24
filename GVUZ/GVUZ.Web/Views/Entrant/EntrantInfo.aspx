<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.EntrantInfoViewModelC>" %>

<%@ Register TagPrefix="gv" TagName="EntrantCard" Src="~/Views/Shared/Entrant/EntrantCard.ascx" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
	Карточка абитуриента
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PageTitle" runat="server">
	Карточка абитуриента: 
</asp:Content>

<asp:Content ID="PageSubtitle" ContentPlaceHolderID="PageSubtitle" runat="server">
	<%: Model.FullName %>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
	<div class="divstatement">
		<div id="content">
			<gv:EntrantCard runat="server" />
		</div>
	</div>
</asp:Content>
