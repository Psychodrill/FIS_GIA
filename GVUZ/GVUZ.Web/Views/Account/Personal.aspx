<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.Models.Account.PersonalModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<asp:Content ID="changePasswordTitle" ContentPlaceHolderID="TitleContent" runat="server">
Личный кабинет
</asp:Content>
<asp:Content ID="header" ContentPlaceHolderID="PageTitle" runat="server">Личный кабинет</asp:Content>


<asp:Content ID="changePasswordContent" ContentPlaceHolderID="MainContent" runat="server">
    <% using (Html.BeginForm())
	{ %>
		<div>
		<fieldset class="fieldsets w500px"><legend>Проверочный код</legend>
		    <div><span class="big">Получение проверочного кода </span><span class="superscript"><a href="#" onclick="doViewPinHelp(this);return false;">[?]</a></span></div>
			<table class="tableAdmin"><colgroup><col  width="40%"/><col  width="*"/></colgroup>
				<thead>
					<tr>
						<th class="caption">
						</th>
						<th>
						</th>
					</tr>
				</thead>
				<tbody>
					<tr>
						<td class="caption"><%: Html.LabelFor(m => m.PinCode) %>:</td>
						<td><%= string.IsNullOrEmpty(Model.PinCode) ? "Не выдавался" : Model.PinCode%></td>
					</tr>
					<% if (!string.IsNullOrEmpty(Model.PinCode))
{%>
					<tr>
						<td class="caption"><%:Html.LabelFor(m => m.AvailableEgeCheckCount)%>:</td>
						<td><%:Html.DisplayTextFor(m => m.AvailableEgeCheckCount)%></td>
					</tr>
					<%
}%>
				</tbody>
			</table>
            <br />
			<table>
			<tr>
				<td>
				<input id="btnGetPinCode" class="button3 w250px" type="submit" 
					value='<%= "Получить " + (string.IsNullOrEmpty(Model.PinCode) ? "": "новый ") + "проверочный код"%>'
					name="Call_GetPinCode"
					title="Нажмите, чтобы получить проверочный код"/></td>
				<td>
					<% if (!String.IsNullOrEmpty(ViewBag.PinCodeStatus))
				   {%>
						<%= ViewBag.PinCodeStatus%> 
				   <% } %>
				</td>
			</tr>
			
			</table>
            
		</fieldset>
		</div>
		<div>
		<% if(false) { %> <%-- теперь всё в есрп --%>
		<fieldset class="fieldsets w500px"><legend>Учетные данные</legend>
				<%--Минимальное число символов для нового пароля: <%: ViewData["PasswordLength"] %>. <br />
				<%: Html.ValidationSummary(true, "Изменение пароля не произведено. Пожалуйста, исправьте ошибки и попробуйте еще раз.")%>--%>
			<div class="big">Изменения пароля</div>
			<table class="tableAdmin">
				<thead>
					<tr>
						<th class="caption">
						</th>
						<th>
						</th>
					</tr>
				</thead>
				<tbody>
					<tr>
						<td class="caption"><%: Html.LabelFor(m => m.OldPassword) %>:</td>
						<td>
							<%: Html.PasswordFor(m => m.OldPassword) %>
							<%: Html.ValidationMessageFor(m => m.OldPassword) %>
						</td>
					</tr>
					<tr>
						<td class="caption"><%: Html.LabelFor(m => m.NewPassword) %>:</td>
						<td>
							<%: Html.PasswordFor(m => m.NewPassword) %>
							<%: Html.ValidationMessageFor(m => m.NewPassword) %>
						</td>
					</tr>
					<tr>
						<td class="caption"><%: Html.LabelFor(m => m.ConfirmPassword) %>:</td>
						<td>
							<%: Html.PasswordFor(m => m.ConfirmPassword) %>
							<%: Html.ValidationMessageFor(m => m.ConfirmPassword) %>
						</td>
					</tr>
				</tbody>
			</table>
            <br />
			<table>
			<tr>
				<td>
				<input id="btnChangePassword" class="button3 w150px" type="submit" value="Изменить пароль" 
					name="Call_ChangePassword"
					title="Нажмите, чтобы изменить пароль"/></td>
				<td>
					<% if (!String.IsNullOrEmpty(ViewBag.PasswordChangeStatus))
				   {%>
						<%= ViewBag.PasswordChangeStatus%> 
				   <% } %>
				</td>
			</tr>
			
			</table>
		</fieldset>
		<%} %>
		</div>
    <% } %>

<div id="divViewPinHelp" style="border:1px solid black;background-color:White;padding: 5px;display:none;position:absolute;width:400px" onclick="doViewPinHelp(this)">
	Проверочный код предназначен для получения доступа к сведениям о результатах единого государственного экзамена на Едином портале государственных и мунициапльных услуг
	(<a href="http://www.gosuslugi.ru/" target="_blank">www.gosuslugi.ru</a>). Результаты единого государственного экзамена предоставляются в рамках государственной услуги
	«Формирование и ведение базы данных об участниках единого государственного экзамена», предоставляемой Федеральной службой по надзору в сфере образования и науки.
</div>
<script language="javascript" type="text/javascript">
	var isPinHelpVisible = false
	var pinHelpHandled = false

	function hidePinHelp()
	{
		if (isPinHelpVisible && !pinHelpHandled)
		{
			doViewPinHelp()
		}
		pinHelpHandled = false
	}

	function doViewPinHelp(el)
	{
		if (isPinHelpVisible)
		{
			jQuery('#divViewPinHelp').fadeOut(300)
			isPinHelpVisible = false
			return
		}
		isPinHelpVisible = true
		var p = jQuery(el).offset()
		jQuery('#divViewPinHelp').css('position', 'absolute').css('z-index', 1100).css('top', p.top + jQuery(el).height() + 5).css('left', p.left + 10).fadeIn(300)
		pinHelpHandled = true
	}
	jQuery(document).ready(function () { jQuery(document).click(hidePinHelp) })
</script>
</asp:Content>
