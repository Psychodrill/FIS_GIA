<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.Models.Account.RegisterModel>" %>
<%@ Import Namespace="GVUZ.Model.Institutions" %>

<asp:Content ID="registerTitle" ContentPlaceHolderID="TitleContent" runat="server">
Регистрация
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Регистрация образовательного учреждения</h2>
    <p>
        Шаг 1: регистрация учетной записи уполномоченного сотрудника ОО
    </p>
    <p>
        Минимальное число символов для пароля: <%: ViewData["PasswordLength"] %>.
    </p>

    <% using (Html.BeginForm()) { %>
        <%: Html.ValidationSummary(true, "Регистрация учетной записи не произведена. Пожалуйста, исправьте ошибки и попробуйте еще раз.")%>
        <div>
            <fieldset>
                <legend>Учетные данные</legend>

				<div class="editor-label">
                    <%: Html.LabelFor(m => m.InstitutionType) %>
                </div>
                <div class="editor-field">
                    <%: Html.RadioButtonFor(m => m.InstitutionType, InstitutionType.VUZ, new { id = "vuz", CHECKED = "checked" })%> ВУЗ
					<%: Html.RadioButtonFor(m => m.InstitutionType, InstitutionType.SSUZ, new { id = "ssuz" })%> ССУЗ
                    <%: Html.ValidationMessageFor(m => m.InstitutionType)%>
                </div>

				<%--<div class="editor-label">
                    <%: Html.LabelFor(m => m.Type) %>
                </div>
                <div class="editor-field">
                    <%: Html.RadioButtonFor(m => m.Type, "ВУЗ", new { id = "vuz", CHECKED = "checked" })%> ВУЗ
					<%: Html.RadioButtonFor(m => m.Type, "ССУЗ", new { id = "ssuz" })%> ССУЗ
                    <%: Html.ValidationMessageFor(m => m.Type)%>
                </div>--%>

				<div class="editor-label">
                    <%: Html.LabelFor(m => m.InstitutionName) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.InstitutionName)%>
                    <%: Html.ValidationMessageFor(m => m.InstitutionName)%>
                </div>

				<div class="editor-label">
                    <%: Html.LabelFor(m => m.Inn) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.Inn)%>
                    <%: Html.ValidationMessageFor(m => m.Inn)%>
                </div>

				<div class="editor-label">
                    <%: Html.LabelFor(m => m.Ogrn) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.Ogrn)%>
                    <%: Html.ValidationMessageFor(m => m.Ogrn)%>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.AdministratorName) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.AdministratorName) %>
                    <%: Html.ValidationMessageFor(m => m.AdministratorName) %>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Email) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.Email) %>
                    <%: Html.ValidationMessageFor(m => m.Email) %>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Password) %>
                </div>
                <div class="editor-field">
                    <%: Html.PasswordFor(m => m.Password) %>
                    <%: Html.ValidationMessageFor(m => m.Password) %>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(m => m.ConfirmPassword) %>
                </div>
                <div class="editor-field">
                    <%: Html.PasswordFor(m => m.ConfirmPassword) %>
                    <%: Html.ValidationMessageFor(m => m.ConfirmPassword) %>
                </div>
                
                <p>
                    <input type="submit" value="Отправить" title="Нажмите, чтобы зарегистрировать учетную запись"/>
                </p>
            </fieldset>
        </div>
    <% } %>
</asp:Content>
