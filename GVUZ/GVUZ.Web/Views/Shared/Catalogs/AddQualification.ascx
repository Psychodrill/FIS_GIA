<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.Administration.Catalogs.AddQualificationViewModel>" %>
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
					<%=Html.LabelFor(m => m.QualificationID)%>:
				</td>
				<td>				
					<%= Html.TextBoxExFor(m => m.Name, null)%>
				</td>
			</tr>
			<tr>
				<td class="caption">
					<%=Html.LabelFor(m => m.Code)%>:
				</td>
				<td>				
					<%= Html.TextBoxExFor(m => m.Code, null)%>
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

	function submitData() {
		var model = {
			QualificationID: <%= Model.QualificationID%>,			
			Name: jQuery('#Name').val(),
			Code: jQuery('#Code').val()
		}
		
		doPostAjax("<%= Url.Generate<AdministrationController>(x => x.CreateQualification(null)) %>", JSON.stringify(model), function (data)
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
		submitData()
	})
	jQuery('#btnCancel').click(function () { closeDialog(jQuery('#dialog')); })

</script>

