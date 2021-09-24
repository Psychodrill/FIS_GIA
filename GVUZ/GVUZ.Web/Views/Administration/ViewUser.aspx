<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
	Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.Administration.UserViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.Security" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">	
	Администирование - пользователь
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

  <h2>Учетная запись пользователя</h2>
	<table class="gvuzData">
		<tbody>
			<tr>
				<td class="caption">
					<%=Html.LabelFor(m => m.FullName)%>:
				</td>
				<td>
					<%= Html.DisplayTextFor(m => m.FullName)%>					
				</td>
			</tr>
			<tr>
				<td class="caption">
					<%=Html.LabelFor(m => m.Position)%>:
				</td>
				<td>
					<%= Html.DisplayTextFor(m => m.Position)%>
				</td>
			</tr>
			<tr>
				<td class="caption">
					<%=Html.LabelFor(m => m.EmailLogin)%>:
				</td>
				<td>
					<%= Html.DisplayTextFor(m => m.EmailLogin )%>					
				</td>
			</tr>
			<tr>
				<td class="caption">
					<%=Html.LabelFor(m => m.Phone)%>:
				</td>
				<td>
					<%= Html.DisplayTextFor(m => m.Phone)%>
				</td>
			</tr>
			<tr>
				<td class="caption-top">
					<%=Html.LabelFor(m => m.Comment)%>:
				</td>
				<td>
					<%= Html.DisplayTextFor(m => m.Comment)%>
				</td>
			</tr>
			<tr>
				<td class="caption">
					<%=Html.LabelFor(m => m.StatusID)%>:
				</td>
				<td>
					<%= Html.DisplayTextFor(m => m.Status) %>
				</td>
			</tr>
			<tr>
				<td class="caption-top">
					<%= Html.LabelFor(m => m.RolesDict)%>:
				</td>
				<td>
					<table>
						<tr>
							<td style="text-align: center"><%: Html.LabelFor(x => x.ExistingRoles) %></td>
							<td>&nbsp;</td>
							<td style="text-align: center"><%: Html.LabelFor(x => x.AssignedRoles) %></td>
						</tr>
						<tr>
							<td>
								<select id="srcRoles" class="roles" multiple="multiple">
									<% foreach (string role in Model.ExistingRoles) { %>
										<option value="<%: role %>"><%: role %></option>
									<% } %>
								</select>
							</td>
							<td>
								<button id="btnLeftToRight">></button><br />
								<button id="btnRightToLeft"><</button>
							</td>
							<td>
								<select id="dstRoles" class="roles" multiple="multiple">
									<% foreach (string role in Model.AssignedRoles) { %>
									<option value="<%: role %>"><%: role %></option>
									<% } %>
								</select>								
							</td>
						</tr>
<%--
						<tr>
							<td colspan="3">
								<p id="rolesInfo" class="field-validation-error"></p>
							</td>
						</tr>
--%>
					</table>
				</td>
			</tr>
		</tbody>
	</table>

	<div style="text-align: center">
		<input type="button" value="Сохранить" id="btnSave" />
		<input type="button" value="Отмена" id="btnCancel" />
	</div>

	<script type="text/javascript">
		
		var userID = '<%= Model.UserID %>'
		var existingRoles = <%= Html.Serialize(Model.ExistingRoles) %>

		function navigateToList()
		{
			<% if(UserOffice.GetOffice() == Office.Edu) { %>
				navigateTo('<%= Url.Generate<AdministrationController>(x => x.EduList()) %>');
			<% } else { %>
				navigateTo('<%= Url.Generate<AdministrationController>(x => x.RonList(null)) %>');
			<% } %>			
		}

		jQuery(function ()
		{
			jQuery('input#btnCancel').click(navigateToList);

			jQuery('button#btnLeftToRight').click(function ()
			{
				moveSelectedOptions('srcRoles', 'dstRoles')
			})

			jQuery('button#btnRightToLeft').click(function ()
			{
				moveSelectedOptions('dstRoles', 'srcRoles');
			})

			jQuery('input#btnSave').click(function ()
			{
				var srcRoles = jQuery('select#srcRoles').get(0);
				var dstRoles = jQuery('select#dstRoles').get(0);
				
				var model = 
					{
						UserID: userID,
						ExistingRoles: getOptionValues('srcRoles'),
						AssignedRoles: getOptionValues('dstRoles')
					}

				model.EmailLogin = '<%= Model.EmailLogin %>'				
<%--
				if(model.AssignedRoles.length == 0)
				{
					jQuery('#rolesInfo').text('Назначьте пользователю роль');
					return;
				}
				else
					jQuery('#rolesInfo').text('');
--%>

				doPostAjax('<%= Url.Generate<AdministrationController>(x => x.SaveUserRoles(null)) %>', JSON.stringify(model), 
				function (data)
				{
					if (!addValidationErrorsFromServerResponse(data, true))
						navigateToList();
				})
			})

			jQuery('input#btnCancel').focus();
		})

	</script>
</asp:Content>
