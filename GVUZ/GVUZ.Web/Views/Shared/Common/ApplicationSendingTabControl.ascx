<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Portlets" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>

<div class="divstatement" style="width:90%">
<div id="tabControl" class="submenu"></div>
<div id="contentTabArea" style="clear:both;padding:5px;" ></div>
</div>

<script type="text/javascript">
    var menuItems = [
		/*{ name: 'Сведения о родителях', link: "javascript:doTabAjaxLoad('<%= Url.Generate<ApplicationController>(c => c.ApplicationSendingParentsTab()) %>',0)", enable: true  },
		{ name: 'Адрес', link: "javascript:doTabAjaxLoad('<%= Url.Generate<ApplicationController>(c => c.ApplicationSendingAddressTab()) %>',0)", enable: true, noWrap: true },*/
		{ name: 'Документы', link: "javascript:doTabAjaxLoad('<%= Url.Generate<ApplicationController>(c => c.ApplicationSendingDocumentsTab()) %>',0)", enable: true, noWrap: true },
		{ name: 'Испытания', link: "javascript:doTabAjaxLoad('<%= Url.Generate<ApplicationController>(c => c.ApplicationSendingTestsTab()) %>',1)", enable: true, noWrap: true }/*,*/
		/*{ name: 'Иностранные языки', link: "javascript:doTabAjaxLoad('<%= Url.Generate<ApplicationController>(c => c.ApplicationSendingLanguagesTab()) %>',4)", enable: true }*/
	]

	jQuery(document).ready(function()
	{
		//doTabAjaxLoad('<%= Url.Generate<ApplicationController>(c => c.ApplicationSendingParentsTab()) %>', 0)
		doTabAjaxLoad('<%= Url.Generate<ApplicationController>(c => c.ApplicationSendingDocumentsTab()) %>', 0)
	})

	function doTabAjaxLoad(url, id)
	{
		for (var i = 0; i < menuItems.length; i++ )
			menuItems[i].selected = false
		menuItems[id].selected = true
		new TabControl(jQuery('#tabControl'), menuItems).init();
		doPostAjax(url, null, 
					function (eventData) {
							jQuery('#contentTabArea').html(eventData);
						}, 'text/html', 'html');
	}
</script>