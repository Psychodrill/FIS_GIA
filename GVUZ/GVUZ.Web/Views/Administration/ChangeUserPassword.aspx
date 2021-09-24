<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.Administration.ChangeUserPasswordViewModel>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent">Смена пароля пользователя</asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
	<p>
		Используйте форму ниже для изменения пароля.
	</p>
	<p>
		Минимальное число символов для нового пароля:
		<%: ViewData["PasswordLength"] %>.
	</p>

	<% using (Html.BeginForm()) { %>
	<%: Html.ValidationSummary(true, "Изменение пароля не произведено. Пожалуйста, исправьте ошибки и попробуйте еще раз.")%>
	<div>
		<fieldset>
			<legend>Учетные данные</legend>
			<%: Html.HiddenFor(m => m.UserID) %>
			<div class="editor-label" style="margin-bottom: 20px">
				<b><%: Html.LabelFor(m => m.UserName) %>: </b> <%: Html.DisplayTextFor(m => m.UserName) %>
			</div>
			<div class="editor-label">
				<%: Html.LabelFor(m => m.NewPassword) %>
			</div>
			<div class="editor-field">
				<%: Html.PasswordFor(m => m.NewPassword) %>
				<%: Html.ValidationMessageFor(m => m.NewPassword) %>
			</div>
			<div class="editor-label">
				<%: Html.LabelFor(m => m.ConfirmPassword) %>
			</div>
			<div class="editor-field">
				<%: Html.PasswordFor(m => m.ConfirmPassword) %>
				<%: Html.ValidationMessageFor(m => m.ConfirmPassword) %>
			</div>
			<p>
				<input type="submit" value="Изменить пароль" title="Нажмите, чтобы изменить пароль" />
			</p>
		</fieldset>
	</div>
	<% } %>
</asp:Content>
