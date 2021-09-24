<%@ Page Title="Title" Language="C#" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.ApplicationExportViewModel>" MasterPageFile="../Shared/Site.Master" %>

<%@ Register TagPrefix="gv" TagName="AdminMenuControl" Src="~/Views/Shared/Controls/AdminMenuControl.ascx" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
    Выгрузка для ГЗГУ
</asp:Content>
<asp:Content ContentPlaceHolderID="PageTitle" runat="server">
    Выгрузка для ГЗГУ
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageHeaderContent">
    <% if (Model.IsExportInProgress) { %>
    <meta http-equiv="refresh" content="10" />
    <% } %>
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
	<% ViewData["MenuItemID"] = 7; %>
	<gv:AdminMenuControl ID="AdminMenuControl1" runat="server" />
    <div style="margin-top: 32px;clear: both">
    <% if (Model.IsDenied) { %>
    
    <p style="font-weight: bold;color: red">
        <%= Html.LabelFor(m => m.IsDenied) %>
    </p>

    <% } else if (Model.IsExportInProgress) { %>
    <p>
        <i>Пожалуйста, подождите &mdash; идёт формирование файла</i>
    </p>
    
    <% } else { %>

    <p>Для формирования файла с выгрузкой для передачи в Центр госзадания и госучета (ГЗГУ) выберите год, за который необходимо выгрузить сведения о заявлениях, и нажмите кнопку &laquo;Сформировать файл выгрузки&raquo;.
    <br />
    Через некоторое время файл будет сформирован и доступен для скачивания на данной странице.
    </p>	
    
    <%= Html.ValidationSummary(true) %>
    
    <% using (Html.BeginForm("Submit", "ApplicationExport", FormMethod.Post)) { %>
    
    <p style="white-space: nowrap">
        <%= Html.LabelFor(m => m.SelectedYear) %>:
        <%= Html.DropDownListFor(m => m.SelectedYear, Model.YearRange.Select(x => new SelectListItem{Text = x.ToString(), Value = x.ToString(), Selected = Model.SelectedYear.HasValue && Model.SelectedYear.Value == x})) %>
    </p>
    <input type="submit" value="Сформировать файл выгрузки"/>
    <% } %>
    
        <% if (Model.IsExportComplete) { %>
            <p style="white-space: nowrap"><b>Последний сформированный файл выгрузки:</b>
            <span style="margin-left: 32px"><%= Model.ExportedFileDate.GetValueOrDefault(DateTime.MinValue).ToString("dd.MM.yyyy HH:mm:ss") %></span>
            <span style="margin-left: 32px"><%= Html.ActionLink("скачать файл", "Download") %></span>
            </p>

        <% } else if (Model.IsExportFailed) { %>
            <p style="color: red">
                При формировании последнего файла выгрузки произошла ошибка. Попробуйте выполнить запрос еще раз.
            </p>
    
        <% } %>
    <% } %>
    </div>
</asp:Content>

