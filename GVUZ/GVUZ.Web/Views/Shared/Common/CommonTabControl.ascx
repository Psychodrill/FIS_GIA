<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="FogSoft.Helpers" %>
<%@ Import Namespace="FogSoft.WSRP" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="Microsoft.Practices.ServiceLocation" %>

<%--<div id="gvuzMenu" class="gvuzMenu"></div>--%>
<div id="gvuzMenu" class="submenu"></div>
<%--
	<a href="#" class="menuitemr select2">Общая<br>информация</a>
	<a href="#" class="menuitemr">Структура ОО</a>
	<a href="#" class="menuitemr">Структура<br>приема</a>
	<a href="#" class="menuitemr">Вступительные<br>испытания</a>
	<a href="#" class="menuitemr">Подготовительные<br>курсы</a>
	<a href="#" class="menuitemr">Олимпиады</a>
	<a href="#" class="menuitemr">Объем и структура<br>приема</a>
	<a href="#" class="menuitemr">Конкурсы</a>
--%>

<script type="text/javascript">
		jQuery(document).ready(function ()
		{
			<%if (Url.IsInsidePortlet())
			{
				string currentTab = "0";
				ISession session = ServiceLocator.Current.GetInstance<ISession>();
				if (session != null)
					currentTab = session.GetValue("currentTab", "0");
				/*object value = HttpContext.Current.Session["currentTab"];
				string currentTab = "0";
				if (value!=null)
					currentTab = value.ToString();*/
				%>
				menuItems[<%= currentTab%>].selected = true;
				<%
			}%>

			new TabControl(jQuery('#gvuzMenu'), menuItems).init();
		});

</script>