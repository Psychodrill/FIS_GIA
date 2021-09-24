<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="FogSoft.Helpers" %>
<%@ Import Namespace="FogSoft.WSRP" %>
<%@ Import Namespace="GVUZ.Helper" %>

<div id="gvuzMenu" class="gvuzMenu"></div>
<script type="text/javascript">
		jQuery(document).ready(function () 
		{
			new TabControl(jQuery('#gvuzMenu'), menuItems, null, jQuery('#tabControl')).init();
		});

</script>