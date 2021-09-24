<%@ Page Title="Анализ хода ПК" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.Reports.ReportsViewModel>" %>

<%@ Register TagPrefix="gv" TagName="AdminMenuControl" Src="~/Views/Shared/Controls/AdminMenuControl.ascx" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.ViewModels.Reports" %>
<%@ Import Namespace="Plat.WebForms.Components.Reporting" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Анализ хода ПК
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PageHeaderContent" runat="server">
    <style type="text/css">
        .auto-style1 {
            height: 23px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PageTitle" runat="server">
    Анализ хода ПК
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="divstatement" style="top: 30px;">
        <% using (Html.BeginForm("GenerateReport", "Reports", FormMethod.Post))
           { %>
        <table class="gvuzDataGrid reportsMenu">
            <tbody>
                <% foreach (MenuElement menuElement in Model.MenuElements)
                   { %>
                <tr>
                    <td class="caption <%=(Model.ReportCode == menuElement.ReportCode)?"active":"" %>">
                        <%= Url.GenerateLink<ReportsController>(c => c.Report(menuElement.ReportCode), menuElement.Text)%>
                    </td>
                </tr>
                <%} %>
            </tbody>
        </table>
        <% if (!String.IsNullOrEmpty(Model.ReportCode))
           { %>
        <table class="gvuzDataGrid reportForm">
            <tbody>
                <tr>
                    <th colspan="2" class="auto-style1">
                        <%=Model.ReportName%>
                        <%= Html.HiddenFor(x => x.ReportCode)%>
                    </th>
                </tr>
                <% for (int i = 0; i < Model.ReportParameters.Count; i++)
                   { %>
                <tr>
                    <td class="caption">
                        <%= Model.ReportParameters[i].Text %>
                    </td>
                    <td>
                        <%= Html.HiddenFor(x => x.ReportParameters[i].DBName)%>
                        <% if (Model.ReportParameters[i].InputType == RegisteredParametersInputs.CDropDownInput)
                           {
                               if (Model.ReportParameters[i].ItemsForSelectLoaded)
                               { %>
                        <%=Html.DropDownListFor(x => x.ReportParameters[i].Value, Model.ReportParameters[i].ItemsForSelect)%>
                        <%}
                               else
                               { %>
                        <%=Html.Label("Не удалось загрузить список значений для выбора") %>
                        <%} %>
                        <%}
                           else
                           { %>
                        <%= Html.TextBoxFor(x => x.ReportParameters[i].Value, new { @class = "reportParameterInput " + Model.ReportParameters[i].DBName })%>
                        <%} %>
                    </td>
                </tr>
                <%} %>
                <tr>
                    <td class="caption">
                        Формат
                    </td>
                    <td>
                        <%=Html.DropDownListFor(x => x.ReportFileFormat, Model.ReportFileFormats, new { @class = "reportParameterInput format" })%>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center">
                        <input type="submit" value="Сформировать отчет" class="button" />
                    </td>
                </tr>
                <%if (!String.IsNullOrEmpty(Model.ReportHTML))
                  {%>
                <tr>
                    <td colspan="2">
                        <div id="reportHtml" class="reportHtml" style="display:none;">
                            <%=Html.Raw(Model.ReportHTML) %>
                        </div>
                    </td>
                </tr>
                <%} %>
            </tbody>
        </table>
        <%} %>
        <%} %>
    </div>
    <script language="javascript" type="text/javascript">
        $(document).ready(function ()
        {
            var maxWidth = $('#reportHtml').parent().width() - 20;
            $('#reportHtml').css('max-width', maxWidth + 'px');
            $('#reportHtml').show();
        });
    </script>
</asp:Content>
