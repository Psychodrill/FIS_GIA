<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.Administration.ChangeUserPasswordViewModel>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent">����� ������ ������������</asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
	<p>
		����������� ����� ���� ��� ��������� ������.
	</p>
	<p>
		����������� ����� �������� ��� ������ ������:
		<%: ViewData["PasswordLength"] %>.
	</p>

	<% using (Html.BeginForm()) { %>
	<%: Html.ValidationSummary(true, "��������� ������ �� �����������. ����������, ��������� ������ � ���������� ��� ���.")%>
	<div>
		<fieldset>
			<legend>������� ������</legend>
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
				<input type="submit" value="�������� ������" title="�������, ����� �������� ������" />
			</p>
		</fieldset>
	</div>
	<% } %>
</asp:Content>
