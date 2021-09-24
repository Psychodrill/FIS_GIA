﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.Administration.Catalogs.AddForeignLanguageViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

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
			<tr>
				<td class="caption">
					<%=Html.LabelFor(m => m.LanguageID)%>:
				</td>
				<td>				
					<%= Html.TextBoxExFor(m => m.Name, null)%>
				</td>
			</tr>						
		</tbody>
	</table>
	<div style="display:none">
		<input id="btnSubmit" type="button" value="Сохранить" /> 
		<input id="btnCancel" type="button" value="Отмена" />
	</div>
</div>

<script type="text/javascript">	

	function submitData()
	{
		var model = {
			LanguageID: <%= Model.LanguageID%>,			
			Name: jQuery('#Name').val()			
		}
		
		doPostAjax("<%= Url.Generate<AdministrationController>(x => x.CreateForeignLanguage(null)) %>", JSON.stringify(model), function (data)
		{
			if(!addValidationErrorsFromServerResponse(data))
			{
				createdItem = data.Data
				jQuery('#btnCancel').click()
			}
			unblockUI()
		}, null, null, false)
	}

	jQuery('#btnSubmit').click(function () { 
		clearValidationErrors(jQuery('.gvuzData'))
		submitData(); 
	})
	jQuery('#btnCancel').click(function () { closeDialog(jQuery('#dialog')); })	
</script>
