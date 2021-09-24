<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.OlympicsCheckResultViewModel>" MasterPageFile="~/Views/Shared/Site.Master" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent">
    Олимпиады школьников
</asp:Content>

<asp:Content runat="server" ID="PageSubtitle" ContentPlaceHolderID="PageSubtitle">
    Сведения об олимпиадах школьников:
</asp:Content>

<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
    <% if (!Model.HasResults)
       { %>
       <span>По вашему запросу сведения о победителях и призерах олимпиад школьников не найдены.</span>
    <% }
       else
       { %>
    <style type="text/css">
        td.field-header { padding: 8px;white-space: nowrap;font-weight: bold}
        td.field-value { white-space: nowrap}
        table.result-table { border-collapse: collapse;border: 1px solid black;margin-left: 8px;}
        table.result-table td, table.result-table th { border: 1px solid black;padding: 8px;text-align: center}
        table.result-table thead { background-color: lightgray}
    </style>
    <table style="border: none;border-collapse: collapse">
        <tr>
            <td class="field-header">
                <%: Html.LabelFor(m => m.DocumentNumber) %>:
            </td>
            <td class="field-value">
                <%: Html.DisplayFor(m => m.DocumentNumber) %>
            </td>
        </tr>
        <tr>
            <td class="field-header">
                <%: Html.LabelFor(m => m.DiplomantName) %>:
            </td>
            <td class="field-value">
                <%: Html.DisplayFor(m => m.DiplomantName) %>
            </td>
        </tr>
        <tr>
            <td class="field-header">
                <%: Html.LabelFor(m => m.OlympiadFullName) %>:
            </td>
            <td class="field-value">
                <%: Html.DisplayFor(m => m.OlympiadFullName) %>
            </td>
        </tr>
         <tr>
            <td class="field-header">
                <%: Html.LabelFor(m => m.OlympiadYear) %>:
            </td>
            <td class="field-value">
                <%: Html.DisplayFor(m => m.OlympiadYear) %>
            </td>
        </tr>
        <tr>
            <td class="field-header">
                <%: Html.LabelFor(m => m.OlympiadNumber) %>:
            </td>
            <td class="field-value">
                <%: Html.DisplayFor(m => m.OlympiadNumber) %>
            </td>
        </tr>
        <tr>
            <td class="field-header">
                <%: Html.LabelFor(m => m.OlympiadSubjectName) %>:
            </td>
            <td class="field-value">
                <%: Html.DisplayFor(m => m.OlympiadSubjectName) %>
            </td>
        </tr>
        <tr>
            <td class="field-header">
                <%: Html.LabelFor(m => m.OlympiadLevel) %>:
            </td>
            <td class="field-value">
                <%: Html.DisplayFor(m => m.OlympiadLevel) %>
            </td>
        </tr>
        <tr>
            <td class="field-header">
                <%: Html.LabelFor(m => m.DiplomaLevel) %>:
            </td>
            <td class="field-value">
                <%: Html.DisplayFor(m => m.DiplomaLevel) %>
            </td>
        </tr>
        
    </table>
    
    <% if (false)
       { %>
    <p style="font-weight: bold;padding-left: 8px;"><%: Html.LabelFor(m => m.EgeResults) %>:</p>
    
    <% if (Model.EgeResults.Count == 0)
       { %>

    <p style="padding-left: 8px;"><i>Данные о результатах ЕГЭ по профильным общеобразовательным предметам отсутствуют</i></p>
    
    <% }
       else
       { %>
    <table class="result-table">
        <thead>
            <tr>
                <th><%: Html.LabelFor(m => m.EgeResultsHeader.SubjectName) %></th>
                <th><%: Html.LabelFor(m => m.EgeResultsHeader.Mark) %></th>
                <th><%: Html.LabelFor(m => m.EgeResultsHeader.Year) %></th>
                <th><%: Html.LabelFor(m => m.EgeResultsHeader.StatusName) %></th>
            </tr>
        </thead>
        <tbody>
            <% foreach (var group in Model.EgeResults.OrderBy(x => x.SubjectName).GroupBy(x => x.SubjectName))
               {
                   var resultCount = group.Count();
                   var firstResult = group.First();
            %>
               <tr>
                   <td style="text-align: left;vertical-align: top" rowspan="<%: resultCount %>"><%: Html.ValueOrSpace(firstResult.SubjectName) %></td>
                   <td><%: Html.ValueOrMdash(firstResult.Mark) %></td>
                   <td><%: Html.ValueOrMdash(firstResult.Year) %></td>
                   <td><%: Html.ValueOrMdash(firstResult.StatusName) %></td>
                </tr>
                <% foreach (var result in group.Skip(1))
                   { %>
                    <tr>
                        <td><%: Html.ValueOrMdash(result.Mark) %></td>
                        <td><%: Html.ValueOrMdash(result.Year) %></td>
                        <td><%: Html.ValueOrMdash(result.StatusName) %></td>
                    </tr>
                <% } %>
            <% } %>
        </tbody>
    </table>
    <% } %>
    <% } %>
    <% } %>
    <p style="margin-left: 8px">
    <%:Html.ActionLink("Проверить другого участника", "Index") %>
    </p>
</asp:Content>
