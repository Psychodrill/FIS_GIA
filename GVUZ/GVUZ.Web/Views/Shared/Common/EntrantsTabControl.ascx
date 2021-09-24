<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Register TagPrefix="gv" TagName="TabControl" Src="~/Views/Shared/Common/CommonTabControl.ascx" %>

<gv:TabControl runat="server" />

<script type="text/javascript">

  var menuItems = [
		{ name: 'Личные данные', link: '<%= Url.Generate<EntrantController>(c => c.EntrantViewTab0()) %>', enable: true, noWrap: true },
		{ name: 'Адрес', link: '<%=  Url.Generate<EntrantController>(c => c.EntrantViewTab1()) %>', enable: true, noWrap: true },
		{ name: 'Документы', link: '<%=  Url.Generate<EntrantController>(c => c.EntrantViewTab2()) %>', enable: true, noWrap: true	},
		{ name: 'Иностранные языки', link: '<%=  Url.Generate<EntrantController>(c => c.EntrantViewTab3()) %>', enable: true },
		{ name: 'Заявления', link: '<%=  Url.Generate<EntrantController>(c => c.EntrantViewTab4()) %>', enable: true, noWrap: true }
	]

</script>