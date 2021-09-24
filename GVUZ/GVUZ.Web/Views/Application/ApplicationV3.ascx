<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.ApplicationV3Model>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Model.Entrants" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<div id="content">
    <%if (Model != null)
      {%>
    <div class="statementtitle">
        Результаты итогового сочинения <span class="statementsubtitle"></span>
    </div>
    <%if (Model.CompositionResult.Count > 0)
      { %>
    <div class="statementborder">
        <table class="gvuzDataGrid" cellpadding="3" id="docGrid">
            <thead>
                <tr>
                    <th>
                        <%= Html.LabelFor(x => x.BaseComposition.Year)%>
                    </th>
                    <th>
                        <%= Html.LabelFor(x => x.BaseComposition.acrName)%>
                    </th>
                    <th>
                        <%= Html.LabelFor(x => x.BaseComposition.acrResult)%>
                    </th>
                </tr>
            </thead>
            <tbody>
                <%foreach (var cr in Model.CompositionResult)
                  {%>
                <tr>
                    <td>
                        <%= cr.strYear %>
                    </td>
                    <td>
                        <%= cr.acrName %>
                    </td>
                    <td>
                        <%if (cr.acrResult)
                          {%>Зачет<%}
                          else
                          {%>Незачет<%}%>
                    </td>
                </tr>
                <% } %>
            </tbody>
        </table>
    </div>
    <br />
    <% } %>
    <%
          foreach (var CG in Model.ListCG)
          { %>
    <div class="statementtitle">
        Конкурс: <span class="statementsubtitle">
            <%:CG.CompetitiveGroupName%></span></div>
    <div class="divGeneralBenefits" style="padding-bottom: 10px">
        <span style="font-size: 12pt; font-weight: 500;">Общие льготы:</span>
        <% var globalDocs = Model.listGeneralBenefit.Where(x => x.CompetitiveGroupID == CG.CompetitiveGroupID).ToList();

           if (globalDocs.Count != 0)
           {
               foreach (var globalDoc in globalDocs)
               {       
        %>
        <p>
            <%=globalDoc.BenefitDisplay%></p>
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
                    <th style="width: 20%;">
                        <%= Html.LabelFor(x => x.BaseDocument.SubjectName)%>
                    </th>
                    <th style="width: 20%;">
                        <%= Html.LabelFor(x => x.BaseDocument.Priority)%>
                    </th>
                    <th style="width: 80px" nowrap="nowrap" id="thResultValue">
                        <%= Html.LabelFor(x => x.BaseDocument.ResultValue)%>
                    </th>
                    <th style="width: 80px" nowrap="nowrap" id="thEgeResultValue">
                        <%= Html.LabelFor(x => x.BaseDocument.EgeResultValue)%>
                    </th>
                    <th>
                        <%= Html.LabelFor(x => x.BaseDocument.Source)%>
                    </th>
                </tr>
            </thead>
            <tbody>
                <%
           var idx = 0;
           var prevETType = EntranceTestType.MainType;

           foreach (var rs in Model.ListTest.Where(x => x.CompetitiveGroupID == CG.CompetitiveGroupID).ToArray())
           {
               var docData = Model.AttachedDocs.Where(x => x.EntranceTestItemID == rs.EntranceTestItemID).FirstOrDefault();
               if (rs.EntranceTestItemID == null)
               {
                   docData = null;
               }
                %>
                <% if (rs.EntranceTestTypeID != prevETType)
                   {
                       idx++;
                       prevETType = rs.EntranceTestTypeID.Value;                   
                %>
                <tr class="<%= idx % 2 == 0 ? "trline2" : "trline1" %>">
                    <td colspan="5">
                        <b>
                            <%= rs.EntranceTestTypeID==EntranceTestType.CreativeType?"Дополнительные вступительные испытания творческой и (или) профессиональной направленности":""%>
                            <%= rs.EntranceTestTypeID==EntranceTestType.ProfileType?"Дополнительные вступительные испытания профильной направленности":""%>
                            <%= rs.EntranceTestTypeID==1?"Аттестационные испытания":""%>
                        </b>
                    </td>
                </tr>
                <% }
                   idx++;                 
                %>
                <tr>
                    <td>
                        <%: rs.SubjectName%><% if (rs.IsProfileSubject)
                                               { %><br />
                        <span style="font-size: 7pt">(профильный предмет)</span><%} %>
                    </td>
                    <td>
                        <%= rs.Priority == null ? "Без приоритета" : rs.Priority.ToString() %>
                    </td>
                    <td nowrap="nowrap">
                        <input type="text" class="numeric view" readonly="readonly" value="<%= rs.ResultValue %>" />
                    </td>
                    <td>
                        <% if (rs.EgeResultValue != null)
                           {%>
                        <input type="text" class="numeric view" readonly="readonly" value="<%= rs.EgeResultValue %>" />
                        <% } %>
                    </td>
                    <td>
                        <% if (docData != null)
                           { %>
                        <span>
                            <%= docData.AttachedDisplay%>
                        </span>
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
</div>
<% }
      } %>
