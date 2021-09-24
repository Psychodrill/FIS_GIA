<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Register TagPrefix="gv" TagName="AdminMenuControl" Src="~/Views/Shared/Controls/AdminMenuControl.ascx" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Models" %>

<asp:Content ID="Title" ContentPlaceHolderID="TitleContent" runat="server">
    Администрирование - Справочники системы
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
	<div class="divstatement">
		<% ViewData["MenuItemID"] = 3; %>
		<gv:AdminMenuControl runat="server" />				

		<table class="gvuzDataGrid navigation">			
			<tbody>			
				<tr>
					<td class="caption">
						<%= Url.GenerateLink<AdministrationController>(c=>c.LanguagesCatalog(), "Иностранные языки") %>
					</td>					
				</tr>				
				<tr>
					<td class="caption">
						<%= Url.GenerateLink<AdministrationController>(c => c.OlympicsCatalog(null), "Олимпиады")%>
					</td>					
				</tr>				
				<tr>
					<td class="caption">
						<%= Url.GenerateLink<AdministrationController>(c => c.GeneralSujectsCatalog(), "Общеобразовательные предметы")%>
					</td>					
				</tr>				
				<tr>
					<td class="caption">
						<%= Url.GenerateLink<AdministrationController>(c => c.QualificationsCatalog(), "Квалификация/степень")%>
					</td>					
				</tr>

				<!--конфигурирование приемных компаний-->
				<tr>
					<td class="caption">
						<%= Url.GenerateLink<AdministrationController>(c => c.SubjectsAndEgeMinScore(), "Минимальное количество баллов по результатам ЕГЭ")%>
					</td>					
				</tr>
				<tr>
					<td class="caption">
						<%= Url.GenerateLink<AdministrationController>(c => c.EntranceTestCreativeDirections(), "Перечень направлений для творческих и проф. вступительных испытаний (ВУЗ)")%>
					</td>
				</tr>
				<tr>
					<td class="caption">
						<%= Url.GenerateLink<AdministrationController>(c => c.EntranceTestProfileDirections(), "Перечень вузов и направлений для профильных вступительных испытаний")%>
					</td>
				</tr>
                <tr>
                    <td class="caption">
                        <%= Url.GenerateLink<AdministrationController>(c => c.CampaignOrderDateCatalog(), "Даты приёмных кампаний") %>
                    </td>
                </tr>
<%--				<tr>
					<td class="caption">
						<%= Url.GenerateLink<AdministrationController>(c => c.EntranceTests(), "Вступительные испытания (ВУЗ)")%>
					</td>
				</tr>--%>
			</tbody>
		</table>
	</div>
</asp:Content>

