<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.Administration.Catalogs.AddTestViewModel>" %>

<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>

<style type="text/css">
	span.ui-tool select
	{
		margin: 0px;
		padding: 0px;
	}
	
</style>

<div id="content">
	<table class="gvuzData">
		<tbody>
			
		</tbody>
	</table>
	<div style="display:none">
		<input id="btnSubmit" type="button" value="Сохранить" />
		<input id="btnCancel" type="button" value="Отмена" />
	</div>
</div>

<script type="text/javascript">
	
	jQuery('#btnCancel').click(function () { closeDialog(jQuery('#dialog')); })

</script>