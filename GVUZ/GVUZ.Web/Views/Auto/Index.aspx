<%@ Page Title="Анализ хода ПК" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.Auto.AutoViewModel>" %>

<%@ Register TagPrefix="gv" TagName="AdminMenuControl" Src="~/Views/Shared/Controls/AdminMenuControl.ascx" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.ViewModels.Auto" %>
<%@ Import Namespace="Plat.WebForms.Components.Reporting" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Автоматическое формирование приказов
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PageHeaderContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PageTitle" runat="server">
    Автоматическое формирование приказов
    <br />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="divstatement">       
        <% using (Html.BeginForm("GenerateReport", "Auto", FormMethod.Post))
           { %>
        <table class="autoTable">
            <tbody>
                <tr>
                    <td>Запустить этап приоритетного зачисления</td>
					<td><input type="checkbox" id="cb3" onclick="saveCheckBox(3);" /></td>
                    <td style="color:red">Соглашаясь, Вы подтверждаете завершение внесения данных в части заявлений жителей Крыма, а также заявелений в пределах квот (в том числе без ВИ). После согласия станет доступной возможность формирования приказов по данным условиям зачисления.
                    </td>
				</tr>
                <tr>
                    <td>Этап приоритетного зачисления завершен</td>
					<td><input type="checkbox" id="cb4" onclick="saveCheckBox(4);" /></td>
                    <td style="color:red">Соглашаясь, Вы подтверждаете завершение этапа приоритетного зачисления, а также совпадение состава данных в сформированных приказах с приказами, изданными организацией.
                    </td>
				</tr>
                <tr>
                    <td>Запустить первый этап зачисления</td>
					<td><input type="checkbox" id="cb1" onclick="saveCheckBox(1);" /></td>
                    <td style="color:red">Соглашаясь, Вы подтверждаете завершение внесения данных в части заявлений по первому этапу зачисления. После согласия станет доступной возможность формирования приказов по данным условиям зачисления.
                    </td>
				</tr>
                <tr>
                    <td>Первый этап зачисления завершен</td>
					<td><input type="checkbox" id="cb2" onclick="saveCheckBox(2);" /></td>
                    <td style="color:red">Соглашаясь, Вы подтверждаете завершение первого этапа зачисления, а также совпадение состава данных в сформированных приказах с приказами, изданными организацией.
                    </td>
				</tr>
            </tbody>
		</table>
        <!--<input type="checkbox" id="cb1" onclick="saveCheckBox(1);" />Завершено внесение данных в части зачислений жителей Крыма
        <br />
        <input type="checkbox" class="gvuzDataGrid" id="cb2" onclick="saveCheckBox(2);" />Завершено внесение данных в части зачислений в пределах квот (в том числе без ВИ)
        <br />
        <input type="checkbox" class="gvuzDataGrid" id="cb3" onclick="saveCheckBox(3);" />Запустить этап приоритетного зачисления
        <br />
        <input type="checkbox" class="gvuzDataGrid" id="cb4" onclick="saveCheckBox(4);" />Этап приоритетного зачисления завершен
        <br />-->

        <table class="autoTable reportsMenu">
            <tbody>
                <% foreach (MenuElement menuElement in Model.MenuElements)
                   { %>
                <tr>
                   <%-- <td style="width:auto;">
                        <input type="checkbox" id="<%= menuElement.ReportCode %>" onclick="saveCheckBox('<%= menuElement.ReportCode %>');" />
                    </td>--%>
                    <td class="caption <%=(Model.ReportCode == menuElement.ReportCode)?"active":"" %>">
                        <%= Url.GenerateLink<AutoController>(c => c.Report(menuElement.ReportCode), menuElement.Text)%>
                    </td>
                </tr>
                <%} %>
            </tbody>
        </table>
        <% if (!String.IsNullOrEmpty(Model.ReportCode))
           { %>
        <table class="autoTable reportForm">
            <tbody>
                <tr hidden="true">
                    <th colspan="2">
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
                <tr hidden="true">
                    <td class="caption">
                        Формат
                    </td>
                    <td>
                        <%=Html.DropDownListFor(x => x.ReportFileFormat, Model.ReportFileFormats, new { @class = "reportParameterInput format" })%>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center">
                        <input type="submit" value="Сформировать" class="button" />
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

            var boxes = JSON.parse('<%= Html.Serialize(Model.CheckBoxes) %>');
            for (var i = 0; i < boxes.length; i++) {
                var box = boxes[i];
                //$('#cb' + box.Id)[0].checked = box.IsAgreed;
                if (box.IsAgreed) {
                    $('#cb' + box.Id).attr('checked', 'checked');
                    $('#cb' + box.Id).attr('disabled', 'disabled');
                }
            }
        });

        function saveCheckBox(code)
        {
            //alert(code);
            var value = $('#cb' + code).is(':checked');
            var data = {Id : code, IsAgreed : value};

            doPostAjax('<%= Url.Generate<AutoController>(x => x.CheckBoxUpdate(null)) %>', JSON.stringify(data), function (data)
            {
                if (data.IsError) { 
                    alert('Ошибка при сохранении изменений!');
                }
            });
        }
    </script>
        <script language="javascript" type="text/javascript">
        $(document).ready(function ()
        {
            var maxWidth = $('#reportHtml').parent().width() - 20;
            $('#reportHtml').css('max-width', maxWidth + 'px');
            $('#reportHtml').show();
        });
    </script>
</asp:Content>
