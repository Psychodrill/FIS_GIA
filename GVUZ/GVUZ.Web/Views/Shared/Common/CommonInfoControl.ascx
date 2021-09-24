<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.InstituteCommonInfoViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

<table class="tableAdmin" style="table-layout: fixed">	
	<col width="270px" />
	<col />
	<col width="150px" />
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
			<%=Html.TableLabelFor(m => m.BriefName)%>
		</td>
		<td colspan="3">
			<%=Html.CommonInputReadOnly(Model.BriefName)%>
		</td>
	</tr>
	<tr class="separat">
		<td class="caption big" style="padding-bottom: 8px; vertical-align: top; padding-top: 10px">
			<%=Html.TableLabelFor(m => m.FormOfLawID)%>
		</td>
		<td colspan="3">
			<%=Html.CommonInputReadOnly(Model.FormOfLawText)%>
		</td>
	</tr>
	<tr class="separat">
		<td class="caption">
			<%=Html.TableLabelFor(m => m.Site)%>
		</td>
		<td colspan="3">
			<%=Html.CommonInputReadOnly(Model.Site)%>
		</td>
	</tr>
	<tr>
		<td class="caption">
			<%=Html.TableLabelFor(m => m.RegionID)%>
		</td>
		<td>
			<%=Html.CommonInputReadOnly(Model.RegionText)%>
		</td>
		<td class="caption w150px">
			<%=Html.TableLabelFor(m => m.Address)%>
		</td>
		<td>
			<%=Html.CommonInputReadOnly(Model.Address)%>
		</td>
	</tr>
	<tr>
		<td class="caption">
			<%=Html.TableLabelFor(m => m.Phone)%>
		</td>
		<td>
			<%=Html.CommonInputReadOnly(Model.Phone)%>
		</td>
		<td class="caption w150px">
			<%=Html.TableLabelFor(m => m.Fax)%>
		</td>
		<td>
			<%=Html.CommonInputReadOnly(Model.Fax)%>
		</td>
	</tr>
	<tr>
		<td class="caption">
			<%=Html.TableLabelFor(m => m.LicenseNumber)%>
		</td>
		<td>
			<%=Html.CommonInputReadOnly(Model.LicenseNumber + " от: " + (Model.LicenseDate == DateTime.MinValue ? "" : Model.LicenseDate.ToString("dd.MM.yyyy")))%>				 
		</td>
		<td class="caption w150px">
			<%=Html.TableLabelFor(m => m.Accreditation)%>
		</td>
		<td>
			<%=Html.CommonInputReadOnly(Model.Accreditation)%>			
		</td>
	</tr>
	<tr class="separat">
		<td class="caption">&nbsp;</td>
		<td>
			<%= Html.GenerateFileLink(Url.Generate<BaseController>(c => c.GetFile1(Model.LicenseDocumentID)),
														Model.LicenseDocumentName)%>
		</td>
		<td class="caption w150px"></td>
		<td>
			<%= Html.GenerateFileLink(Url.Generate<BaseController>(c => c.GetFile1(Model.AccreditationDocumentID)),
										Model.AccreditationDocumentName)%>
		</td>
	</tr>
    <tr class="separat">
		            <td class="caption">
                    Правила приёма:
		            </td>
                    <td>
                    <%for (int i = 2012; i <= DateTime.Now.AddYears(1).Year; i++)
                      {%>
                      <% if (BaseController.CheckFile(i,Model.InstitutionID)) %>
                    <%= Html.GenerateFileLink(Url.Generate<InstitutionController>(c => c.GetFile2(i)), "Правила приёма "+i)%>
                    <%} %>
                        </td>
    </tr>
	<tr class="separat">
		<td class="caption">
			<%= Html.TableLabelFor(m => m.HasMilitaryDepartment)%>
		</td>
		<td>
			<%=Html.CommonInputReadOnly(Model.HasMilitaryDepartment ? "Да" : "Нет")%>			
		</td>
		<td>
			&nbsp;
		</td>
		<td>
			&nbsp;
		</td>
	</tr>
	<tr>
		<td class="caption">
			<%=Html.TableLabelFor(m => m.HasHostel)%>
		</td>
		<td>
			<%=Html.CommonInputReadOnly(Model.HasHostel ? "Да" : "Нет")%>
		</td>		
		<td class="caption w150px">
			<% if (Model.HasHostel) { %>	<%= Html.TableLabelFor(m => m.HostelCapacity)%> <% } else { %>
				&nbsp;
			<% } %>
		</td>
		<td>
			<% if (Model.HasHostel) { %> <%=Html.CommonInputReadOnly(Model.HostelCapacity.ToString())%> <% } else { %>
			  &nbsp;
			<% } %>
		</td>		
	</tr>
	<tr>
		<td class="caption">
			<%=Html.TableLabelFor(m => m.HasHostelForEntrants)%>
		</td>	
		<td>
			<%=Html.CommonInputReadOnly(Model.HasHostelForEntrants ? "Да" : "Нет")%>			
		</td>
		<td>&nbsp;</td>
		<td>&nbsp;</td>
	</tr>
	<% if (Model.HostelFecDocumentName != null) { %>
	<tr>
		<td class="caption">
			<%=Html.TableLabelFor(m => m.HostelFecDocumentName)%>
		</td>
		<td>
			<%= Html.GenerateFileLink(Url.Generate<BaseController>(c => c.GetFile1(Model.HostelFecDocumentID)),
																Model.HostelFecDocumentName)%>
		</td>
		<td>&nbsp;</td>
		<td>&nbsp;</td>
	</tr>
	<% } %>
	</tbody>
</table>
<script type="text/javascript">


</script>
