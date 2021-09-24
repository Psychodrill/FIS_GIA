<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.Administration.AppCampaignViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Register TagPrefix="gv" TagName="AdminMenuControl" Src="~/Views/Shared/Controls/AdminMenuControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Администрирование - управление приемной кампанией
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="PageTitle">Управление приемной компанией</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<script type="text/javascript">

		jQuery(function ()
		{
			jQuery('#btnReceivingApps').click(receivingAppsHandler);
			ShowReceivingAppBlock('<%= Model.ReceiveApplicationsDate %>');
		});

		function receivingAppsHandler()
		{
			confirmDialog('Вы действительно хотите начать прием заявлений?',
				function ()
				{
					doPostAjax('<%= Url.Generate<AdministrationController>(c => c.BeginReceiveApplications()) %>', '',
					function (data, status)
					{
						if (data.IsError) infoDialog(data.Message)
						else ShowReceivingAppBlock(data);
					});
				});
			}

			function ShowReceivingAppBlock(rcvAppDate)
			{
				if (!isStringNullOrEmpty(rcvAppDate))
				{
					var btnRcvApp = jQuery('#btnReceivingApps');
					jQuery('#rcvAppInfo').text('Дата и время начала приема заявлений: ' + rcvAppDate);
					btnRcvApp.attr('disabled', 'disabled');
					btnRcvApp.addClass('buttonDisabled');
				}
				else
				{
					jQuery('#rcvAppInfo').text('На данный момент заявления от абитуриентов через Систему не принимаются');
				}
			}
	</script>
	<% ViewData["MenuItemID"] = 1; %>
	<div class="divstatement">
	<gv:AdminMenuControl runat="server" />
	<table>
		<tr>
			<td>
				<input type="button" class="button3 w200px" value="Начать прием заявлений" id="btnReceivingApps" />
			</td>
			<td>
				<span id="rcvAppInfo"></span>
			</td>
		</tr>
	</table>
	</div>
</asp:Content>
