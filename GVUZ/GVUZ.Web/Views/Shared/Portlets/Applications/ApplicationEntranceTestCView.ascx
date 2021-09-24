<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.Portlets.Applications.ApplicationEntranceTestViewModelC>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Model.Entrants" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Register TagName="PersonalRecordsData" TagPrefix="gv" Src="~/Views/Shared/Portlets/Entrants/PersonalRecordsData.ascx" %>
<%@ Register TagName="ApplicationWizardButtons" TagPrefix="gv" Src="~/Views/Shared/Portlets/Applications/ApplicationWizardButtons.ascx" %>
<%@ Register TagName="EntrantDocumentAddPart" TagPrefix="gv" Src="~/Views/Shared/Controls/EntrantDocumentAddPart.ascx" %>
<div id="content">
	<% foreach (var cgID in Model.EntranceTests.Select(x => x.CompetitiveGroupID).Distinct())
	{ %>

	<div class="statementtitle">Конкурс: <span class="statementsubtitle"><%: Model.EntranceTests.First(x => x.CompetitiveGroupID == cgID).CompetitiveGroupName%></span></div>
	<div class="divGeneralBenefits" style="padding-bottom:10px">
	    
	<span style="font-size:12pt; font-weight: 500;">Общие льготы:</span>
	<% var globalDocs = Model.GlobalDocs.Where(x => x.CompetitiveGroupID == cgID).ToList();
    if (globalDocs != null)
    {
        foreach (var globalDoc in globalDocs)
        {%>
		<% if (globalDoc.BenefitErrorMessage != null)
     {%><p><span class="btnError" style="height:27px" title="<%: globalDoc.BenefitErrorMessage.Replace("\"", "&quot;") %>">&nbsp;</span><%} %> 
		<%= globalDoc.BenefitID == 1 ? "Без вступительных испытаний" : (globalDoc.BenefitID == 4 ? "По квоте приёма лиц, имеющих особое право" : "Преимущественное право на поступление")%>
		(<%= globalDoc.DocumentDescription ?? "Документ не указан"%>) </p>
	<%}
    }
    else
    {%>
		Нет
	<%} %>
</div>

	<div class="statementborder">

			<table class="gvuzDataGrid" cellpadding="3" id="docGrid1">
			<thead>
				<tr>
					<th>
						<%= Html.LabelFor(x => x.DescrTestData.SubjectName)%>
					</th>
                    <th>
                        <%= Html.LabelFor(x => x.DescrTestData.Priority) %>
                    </th>
					<th style="width:80px" nowrap="nowrap" id="thResultValue">
						<%= Html.LabelFor(x => x.DescrAttachedData.ResultValue)%>
					</th>
					<th>
						<%= Html.LabelFor(x => x.DescrAttachedData.SourceID)%>
					</th>
				</tr>
			</thead>
			<tbody>
			<% if (Model.EntranceTests.Length == 0)
	  { %>
				<tr><td colspan="3" align="center">Документы не требуются</td></tr>
			<% } %>
		<%
	  var idx = 0;
	  var prevETType = EntranceTestType.MainType;

	  foreach (var rs in Model.EntranceTests.Where(x => x.CompetitiveGroupID == cgID).ToArray())
	  {
		  if (rs.EntranceTestType != prevETType)
		  {
			  idx++;
			  prevETType = rs.EntranceTestType;%>
			<tr class="<%= idx % 2 == 0 ? "trline2" : "trline1" %>"><td colspan="3"><b>
			<%= rs.EntranceTestType == EntranceTestType.AttestationType ? "Аттестационные испытания" : ""%>
			<%= rs.EntranceTestType == EntranceTestType.CreativeType ? "Дополнительные вступительные испытания творческой и (или) профессиональной направленности" : ""%>
			<%= rs.EntranceTestType == EntranceTestType.ProfileType ? "Дополнительные вступительные испытания профильной направленности" : ""%>
			                                                                        </b></td></tr>
		<%}
		  idx++;
		  var docData = Model.AttachedDocs.Where(x => x.EntranceTestItemID == rs.EntranceTestItemID).FirstOrDefault(); %>
			<tr class="<%= idx % 2 == 0 ? "trline2" : "trline1" %>" itemID="<%= rs.EntranceTestItemID %>">
				<td><%: rs.SubjectName%><% if (rs.IsProfileSubject)
								{ %><br /><span style="font-size:7pt">(профильный предмет)</span><%} %></td>
                <td><%= rs.Priority == null ? "Без приоритета" : rs.Priority.ToString() %></td>
				<td nowrap="nowrap"><input type="text" class="numeric view" readonly="readonly" value="<%= docData == null ? "" : docData.ResultValue.ToString("0.####") %>" /></td>
				<td>
				<% if (docData != null)
	   { %>
					<span><%= docData.DocumentDescription%> </span>
				<% }
	   else
	   {%>
				Нет
				<% } %>
				</td>
			</tr>
<% } %>
			</tbody>
		</table>
	</div>
	<%} %>
</div>
<div id="divEgeCheck" <%= "style=\"display:none\"" %>>
	<input type="button" class="button3" style="width:auto" id="btnEgeCheck" value="Получить/проверить результаты ЕГЭ" onclick="btnEgeCheck()" />
</div>

