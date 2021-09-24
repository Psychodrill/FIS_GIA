<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.Portlets.Applications.ApplicationViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Register TagPrefix="gv" TagName="ResourcesHolder" Src="~/Views/Shared/Controls/ResourcesHolderControl.ascx" %>

<% if (Model.CanEdit && Model.CanView && (!GVUZ.Helper.UrlUtils.IsReadOnly(FBDUserSubroles.ApplicationsDirection))) {%>
	<div>
		<input type="button" class="button3" value="Редактировать" id="btnViewAppEdit" />
	</div>
<%} %>

<div class="gvuzPortlet divstatement" style="top:45px;margin-bottom:50px;">

<style type="text/css">
	#appContent .select2, .select
	{
		border-bottom: 1px solid #EEE
	}
	#appContent .tableApp2 td, .tableApp2 td label
	{
		padding-top: 0px;
		padding-bottom: 0px;
	}
	#appContent .tableApp2 td.caption label, .tableApp2 td input
	{
		margin-top: 0px !important;
		margin-bottom: 0px !important;
	}
	#appDetailsTab .menuitemr, .menuiteml
	{
		padding-left: 10px;
		padding-right: 10px;
	}
	#appContent .gvuzDataGrid input.numeric
	{
		padding-top: 2px;
		padding-bottom: 2px;
		margin-top: 0px;
		margin-bottom: 0px;
	}
</style>

<% if (Model.CanView) {%>
<div class="gvuzTab submenu" id="appDetailsTab" style="background-color: #EEE !important;">
</div>
<div id="appContent">
<%= Model.Content %>
</div>
<%} else {%>
<div class="field-validation-error"><%: Model.DenyMessage %></div>
<%}%>
</div>
<script type="text/javascript">
	var tabControl = new TabControl(jQuery('#appDetailsTab'), [
		{ name: 'Общие сведения', link: 'javascript:switchAppTab(0,0)', enable: true, selected: true, noWrap: true },
		{ name: 'Личные данные', link: 'javascript:switchAppTab(1,1)', enable: true, noWrap: true },
		/*{ name: 'Адрес', link: 'javascript:switchAppTab(2,2)', enable: true, noWrap: true },*/
		{ name: 'Документы', link: 'javascript:switchAppTab(3,2)', enable: true, noWrap: true },
		{ name: 'Испытания', link: 'javascript:switchAppTab(4,3)', enable: true, noWrap: true },
		/*{ name: 'Иностранные языки', link: 'javascript:switchAppTab(5,3)', enable: true },*/
		{ name: 'Индивидуальные достижения', link: 'javascript:switchAppTab(5,4)', enable: true },
		{ name: 'Печатные формы', link: 'javascript:switchAppTab(6,5)', enable: true }
			], {prefix: 'popup'}
		)
	tabControl.init();
	function switchAppTab(tabID, selTabID) {
		doPostAjax('<%= Url.Generate<ApplicationController>(x => x.ApplicationViewPopup(0, 0, true)) %>', 'tabNumber=' + tabID + '&applicationID=' + viewApplicationID + '&onlyContent=true', function (data)
		{
			for (var i = 0; i < tabControl.menuItems.length; i++) { tabControl.menuItems[i].selected = false; }
			tabControl.menuItems[selTabID].selected = true;
			tabControl.init();
			jQuery('#appContent').html(data)
		}, "application/x-www-form-urlencoded", "html")
	}

	jQuery('#btnViewAppEdit').click(function ()
	{
		setCookie('viewAppID', '<%= Model.ApplicationID %>', 1)
		window.location = '<%= Url.Generate<ApplicationController>(x => x.ApplicationPersonalDataByApp(null, null)) %>?applicationID=<%=Model.ApplicationID %>'
					+ '&tabID=' + currentTab
	})
</script>