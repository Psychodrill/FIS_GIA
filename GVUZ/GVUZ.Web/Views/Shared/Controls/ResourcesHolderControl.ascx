<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<%@ Import Namespace="GVUZ.Helper" %>
<%@ Register TagPrefix="gv" TagName="IncludeImageStyles" Src="~/Views/Shared/Controls/ImageStyles.ascx" %>

<%if (Url.IsInsidePortlet())
  {%>
<div class="resourcesDiv"></div>

<%} %>
<%if (!Url.IsInsidePortlet()) {%>
	<link rel="stylesheet" type="text/css" href="<%= Url.Resource("Resources/skin.css") %>" />
<% } else { %>
	<link rel="stylesheet" type="text/css" href="<%= Url.Resource("Resources/Portlet.css") %>" />
<% } %>
<link rel="stylesheet" type="text/css" href="<%= Url.Resource("Resources/controls.css") %>" />
<%--<link rel="stylesheet" type="text/css" href="<%= Url.Resource("Resources/Tree.css") %>" />--%>
<link rel="stylesheet" type="text/css" href="<%= Url.Resource("Resources/Themes/ui-lightness/jquery-ui-1.8.5.custom.css") %>" />

<link rel="stylesheet" type="text/css" href="<%= Url.Resource("Resources/Chosen/chosen.min.css") %>" />
<link rel="stylesheet" type="text/css" href="<%= Url.Resource("Resources/PagedList.css") %>" />

<!--[if lt IE 8]>
	<link rel="stylesheet" type="text/css" href="<%= Url.Resource("Resources/ie7-and-down.css") %>" />
<![endif]-->

<%if (Url.IsInsidePortlet()) {%>
	<gv:IncludeImageStyles runat="server" />
<% } else { %>
	<link rel="stylesheet" type="text/css" href="<%= Url.Resource("Resources/Images.css") %>" />
<% } %>

<%if (!Url.IsInsidePortlet()) {%>
<script type="text/javascript" src="<%= Url.Scripts("jquery-1.4.2.js?117321") %>"></script>
<% } %>

<script type="text/javascript" src="<%= Url.Scripts("libs/jquery.ui.core.js") %>"></script>
<script type="text/javascript" src="<%= Url.Scripts("libs/jquery.ui.widget.js") %>"></script>
<script type="text/javascript" src="<%= Url.Scripts("libs/jquery.ui.button.js") %>"></script>
<script type="text/javascript" src="<%= Url.Scripts("libs/jquery.ui.datepicker.js") %>"></script>
<script type="text/javascript" src="<%= Url.Scripts("libs/jquery.ui.datepicker-ru.js") %>"></script>
<script type="text/javascript" src="<%= Url.Scripts("libs/jquery.ui.dialog.js") %>"></script>
<script type="text/javascript" src="<%= Url.Scripts("libs/jquery.ui.position.js") %>"></script>
<script type="text/javascript" src="<%= Url.Scripts("libs/jquery.form.js") %>"></script>
<script type="text/javascript" src="<%= Url.Scripts("libs/jquery.maskedinput-1.2.2.min.js") %>"></script>
<script type="text/javascript" src="<%= Url.Scripts("libs/sprintf.js") %>"></script>
<script type="text/javascript" src="<%= Url.Scripts("libs/json2.js") %>"></script>
<script type="text/javascript" src="<%= Url.Scripts("libs/jQuery.blockUI.js") %>"></script>
<script type="text/javascript" src="<%= Url.Scripts("ajaxhelper.js") %>"></script>
<script type="text/javascript" src="<%= Url.Scripts("DataUtils.js") %>"></script>
<script type="text/javascript" src='<%= Url.Scripts("utils.js") %>'></script>
<%--<script type="text/javascript" src="<%= Url.Scripts("tree/BaseTreeView.js") %>"></script>
<script type="text/javascript" src="<%= Url.Scripts("tree/ClickableTreeView.js") %>"></script>
<script type="text/javascript" src="<%= Url.Scripts("tree/TreeView.js") %>"></script>
<script type="text/javascript" src="<%= Url.Scripts("tree/TableTreeView.js") %>"></script>--%>
<script type="text/javascript" src="<%= Url.Scripts("TabControl.js") %>"></script>
<script type="text/javascript" src="<%= Url.Scripts("FogSoft.Common.js") %>"></script>

<script type="text/javascript" src="<%= Url.Scripts("Chosen/chosen.jquery.min.js") %>"></script>
<script type="text/javascript" src='<%= Url.Scripts("jquery.unobtrusive-ajax.min.js") %>'></script>
<%--<script type="text/javascript" src='<%= Url.Scripts("jquery.validate.min.js") %>'></script>
<script type="text/javascript" src='<%= Url.Scripts("jquery.validate.unobtrusive.min.js") %>'></script>--%>

<script type="text/javascript" src="<%= Url.Scripts("vue/vue.min.js") %>"></script>
<%--<script type="text/javascript" src="<%= Url.Scripts("vue/httpVueLoader.js") %>"></script>--%>
<script type="text/javascript" src="<%= Url.Scripts("vue/vue-easy-toast.min.js") %>"></script>
<script type="text/javascript" src="<%= Url.Scripts("vue/v-tooltip.min.js") %>"></script>
<script type="text/javascript" src="<%= Url.Scripts("vue/vue.helper.js") %>"></script>

<%if (!Url.IsInsidePortlet()) {%>
<script type="text/javascript" src="<%= Url.Scripts("libs/jquery.ui.autocomplete.js") %>"></script>
<script type="text/javascript" src="<%= Url.Scripts("libs/jquery.htmlinput-min.js") %>"></script>
<script type="text/javascript" src="<%= Url.Scripts("libs/jquery.htmlclean-min.js") %>"></script>
<link rel="stylesheet" type="text/css" href="<%= Url.Resource("Resources/ui.htmlinput.css") %>" />
<% } %>

<%if (Url.IsInsidePortlet()) {%>
    <div class="resourcesDiv">
        
    </div>
<%} %>