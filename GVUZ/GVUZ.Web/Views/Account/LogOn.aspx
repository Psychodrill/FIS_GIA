<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.Models.Account.LogOnModel>" %>

<asp:Content ID="loginTitle" ContentPlaceHolderID="TitleContent" runat="server">
	Вход в систему
</asp:Content>
<asp:Content ID="loginContent" ContentPlaceHolderID="MainContent" runat="server">
	<h2>
		Вход в систему</h2>
	<p>
		Пожалуйста, введите свое имя пользователя и пароль.
		<%: Html.ActionLink("Зарегистрируйтесь", "Register") %>, если у Вас нет учетной
		записи.
	</p>
	<% using (Html.BeginForm("LogOn", "Account", new { ReturnUrl = Request.QueryString["ReturnUrl"] }))
	{ %>
	<%: Html.ValidationSummary(true, "Вход в систему не произведен. Пожалуйста, исправьте ошибки и попробуйте еще раз.") %>
	<div>
		<table>
			<tr>
				<td class="rightAlign label big">
					<%: Html.LabelFor(m => m.UserName) %>:
				</td>
				<td>
					<div class="editor-label">
						<%: Html.TextBoxFor(m => m.UserName) %><%: Html.ValidationMessageFor(m => m.UserName) %></div>
				</td>
			</tr>
			<tr>
				<td class="rightAlign label big">
					<%: Html.LabelFor(m => m.Password) %>:
				</td>
				<td>
					<div class="editor-label">
						<%: Html.PasswordFor(m => m.Password) %><%: Html.ValidationMessageFor(m => m.Password) %></div>
				</td>
			</tr>
			<tr>
				<td>
					&nbsp;
				</td>
				<td class="label big alignLeft">
					<%: Html.CheckBoxFor(m => m.RememberMe) %>
					<%: Html.LabelFor(m => m.RememberMe) %>
				</td>
			</tr>
		</table>
		<p>
			<input class="button3" type="submit" value="Войти" title="Нажмите, чтобы войти в систему" />
		</p>
	</div>
	<% } %>
</asp:Content>
