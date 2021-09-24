<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.InstitutionInfo.InstitutionInfoViewModel>"
	MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content ID="registerTitle" ContentPlaceHolderID="TitleContent" runat="server">
	Просмотр общих сведений
</asp:Content>
<asp:Content ID="header" ContentPlaceHolderID="PageTitle" runat="server">Сведения об образовательной организации</asp:Content>
<%@ Register TagPrefix="gv" TagName="TabControl" Src="~/Views/Shared/Common/InstitutionsTabControl.ascx" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">	
    <div class="divstatement">
		<gv:TabControl runat="server" ID="tabControl" />
		<table class="tableAdmin" style="table-layout: fixed">
			<col width="270px" />
			<col />
			<col width="150px" >
			<col />
			<tbody>
				<tr>
					<td class="caption big">
						<%=Html.TableLabelFor(m => m.FullName)%>
					</td>
					<td colspan="3">
						<%= Html.CommonInputReadOnly(Model.FullName)%>
					</td>
				</tr>
				<tr>
					<td class="caption big">
						<%: Html.TableLabelFor(m => m.BriefName)%>
					</td>
					<td colspan="3">
						<%= Html.CommonInputReadOnly(Model.BriefName)%>
					</td>
				</tr>
				<tr class="separat">
					<td class="caption big" style="padding-bottom: 8px; vertical-align: top; padding-top: 10px">
						<%:Html.TableLabelFor(m => m.FormOfLawId)%>
					</td>
					<td colspan="3">
						<%= Html.CommonInputReadOnly(Model.FormOfLawName)%>
					</td>
				</tr>
				<tr class="separat">
					<td class="caption">
						<%:Html.TableLabelFor(m => m.Site)%>
					</td>
					<td colspan="3">
						<%=Html.CommonInputReadOnly(Model.Site) %>
					</td>
				</tr>
				<tr>
					<td class="caption">
						<%:Html.TableLabelFor(m => m.RegionId)%>
					</td>
					<td>
						<%= Html.CommonInputReadOnly(Model.RegionName)%>
					</td>
					<td class="caption w150px">
						<%:Html.TableLabelFor(m => m.City)%>
					</td>
					<td>
						<%=Html.CommonInputReadOnly(Model.City)%>
					</td>
				</tr>
				<tr>
                    <td class="caption">
						<%:Html.TableLabelFor(m => m.Address)%>
					</td>
					<td>
						<%=Html.CommonInputReadOnly(Model.Address)%>
					</td>
					<td class="caption w150px">
						<%:Html.TableLabelFor(m => m.Phone)%>
					</td>
					<td>
                        <table style="width: 100%">
                            <tr>
                                <td><%=Html.CommonInputReadOnly(Model.Phone)%></td>
                                <td align="right" class="caption"><%:Html.TableLabelFor(m => m.Fax)%></td>
                                <td><%=Html.CommonInputReadOnly(Model.Fax) %></td>
                            </tr>
                        </table>
						
					</td>				
					
				</tr>
				<tr>
					<td class="caption" style="vertical-align:top; padding-top:15px;">
						<%:Html.TableLabelFor(m => m.LicenseNumber)%>
					</td>
					<td >
						<%= Html.CommonInputReadOnly(Model.LicenseNumber, new { @class = "inputboxAd", style = "min-width: 200px; width: 200px !important" })%>
                        <span class="licenseDateCaption">от: <%= Html.CommonDateReadOnly(Model.LicenseDate) %></span>
					</td>
					<td class="caption w150px">
						<span>
						<%:Html.TableLabelFor(m => m.AccreditationNumber)%></span>
					</td>
					<td>
						<%=Html.CommonInputReadOnly(Model.AccreditationNumber)%>
					</td>
				</tr>
				<tr class="separat">
					<td class="caption">&nbsp;</td>
					<td>
						<div style="margin-bottom: 10px;">
                            <% if (Model.LicenseDocument != null) { %>
							<%= Html.GenerateFileLink(Url.Generate<InstitutionController>(c => c.GetFile1(Model.LicenseDocument.FileId)), Model.LicenseDocument.DisplayName)%>
							<% } %>
						</div>
					</td>
					<td class="caption w150px">&nbsp;</td>
					<td>
						<div style="margin-bottom: 10px;">
                            <% if (Model.AccreditationDocument != null) { %>
							<%= Html.GenerateFileLink(Url.Generate<InstitutionController>(c => c.GetFile1(Model.AccreditationDocument.FileId)), Model.AccreditationDocument.DisplayName)%>
							<%  } %>
						</div>
					</td>
				</tr>
                <tr class="separat">
		            <td class="caption">
                    <%= Html.DisplayNameFor(m => m.Documents) %>:
		            </td>
                    <td>
                        <% Html.RenderPartial("EditInstitutionDocumentsList", Model.Documents); %>
                    </td>
                </tr>
               	<tr class="separat">
					<td class="caption">
						<%:Html.TableLabelFor(m => m.HasMilitaryDepartment)%>
					</td>
					<td colspan="3">
						<%=Html.CommonInputReadOnly(Model.HasMilitaryDepartment ? "Да" : "Нет")%>
					</td>
                </tr>
				<tr>
					<td class="caption">
						<%:Html.TableLabelFor(m => m.HasHostel)%>
					</td>
					<td>
						<%=Html.CommonInputReadOnly(Model.HasHostel ? "Да" : "Нет")%>
					</td>
					<td class="caption w150px" id="spHostelData">
						<%:Html.TableLabelFor(m => m.HostelCapacity)%>
					</td>
					<td id="spHostelData2">
						<%=Html.CommonInputReadOnly(Model.HostelCapacity.HasValue ? Model.HostelCapacity.Value.ToString() : string.Empty)%>						
					</td>					
				</tr>
				<tr>
					<td class="caption">
						<%:Html.TableLabelFor(m => m.HasHostelForEntrants) %>
					</td>
					<td colspan="3">
						<%=Html.CommonInputReadOnly(Model.HasHostelForEntrants ? "Да" : "Нет")%>
					</td>
				</tr>
				<tr>
					<td class="caption" style="vertical-align: top; padding-top: 10px;">
						Условие предоставления:
					</td>
					<td colspan="3">
						<% if (Model.HostelDocument != null) { %>
							<%= Html.GenerateFileLink(Url.Generate<InstitutionController>(c => c.GetFile1(Model.HostelDocument.FileId)), Model.HostelDocument.DisplayName)%>
						<% } %>
					</td>
				</tr>	
                		
                <tr class="separat">
					<td class="caption">
						Создание условий проведения ВИ <br /> для лиц с ОВЗ:
					</td>
					<td colspan="3">
						<%=Html.CommonInputReadOnly(Model.HasDisabilityEntrance ? "Да" : "Нет")%>
					</td>
                </tr>
			</tbody>
		</table>
		<div style="margin-top: 15px">
			<%= Html.ActionLink("Редактировать", "Edit", "Institution", null, new { @class = "button3" }) %>
		</div>
	</div>
   
    <script type="text/javascript">
		menuItems[0].selected = true;
	</script>
</asp:Content>
