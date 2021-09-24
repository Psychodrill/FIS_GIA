<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Portlets" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Register TagPrefix="gv" TagName="TabControl" Src="~/Views/Shared/Common/CommonTabControl.ascx" %>

<gv:TabControl ID="TabControl1" runat="server" />

<script type="text/javascript">
	var menuItems = [
		{ name: 'Общие сведения', link: '<%= Url.Generate<ApplicationController>(c => c.ApplicationViewTab0()) %>', enable: true, noWrap: true },
		{ name: 'Личные данные', link: '<%= Url.Generate<ApplicationController>(c => c.ApplicationViewTab1()) %>', enable: true, noWrap: true },
		/*{ name: 'Адрес', link: '<%= Url.Generate<ApplicationController>(c => c.ApplicationViewTab2()) %>', enable: true, noWrap: true },*/
		{ name: 'Документы', link: '<%= Url.Generate<ApplicationController>(c => c.ApplicationViewTab3()) %>', enable: true, noWrap: true },
		{ name: 'Испытания', link: '<%= Url.Generate<ApplicationController>(c => c.ApplicationViewTab4()) %>', enable: true, noWrap: true },
		/*{ name: 'Иностранные языки', link: '<%= Url.Generate<ApplicationController>(c => c.ApplicationViewTab5()) %>', enable: true },*/
		{ name: 'Печатные формы', link: '<%= Url.Generate<ApplicationController>(c => c.ApplicationViewTab6()) %>', enable: true }
	]
</script>