<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="ViewPage<OlympicDiplomantListViewModel>" %>

<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Data.Model" %>
<%@ Import Namespace="GVUZ.Web.ViewModels.OlympicDiplomant" %>
<%@ Import Namespace="GVUZ.Data.Helpers" %>


<asp:Content ContentPlaceHolderID="TitleContent" runat="server">Призеры олимпиад</asp:Content>
<asp:Content ContentPlaceHolderID="PageTitle" runat="server"></asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript" src="<%= Url.Resource("Resources/Scripts/olympics.js") %>"></script>

    <div style="margin-top: -50px;">

        <div id="dialogImport">
        </div>

        <div id="dialog">
        </div>

        <div>
            <h4>Представление сведений по ОШ/ВОШ</h4>
        </div>

        <div>
            <button class="btnAdd button primary" type="button">Добавить победителя/призера ОШ</button>
            <button class="btnFind button primary" id="btnFind" disabled type="button">Поиск в базе участников ЕГЭ</button>
            <button class="btnLoad button primary" type="button">Загрузить из CSV</button>
            <input name="csvInput" id="csvInput" type="file" style="display: none;" accept=".csv" />
        </div>

        <hr />

        <div style="white-space: nowrap; padding: 8px; padding-left: 0; margin-left:15px;">

            <label for="changeYear"><%= Html.DisplayNameFor(x => x.Filter.Year) %>:</label>&nbsp;
            <%= Html.DropDownListExFor(x=>x.Filter.Year, Model.Years, new { @id="changeYear",  @class = "ss", style="width: 100px;" })%>&nbsp;
            <label for="changeName"><%= Html.DisplayNameFor(x => x.Filter.Name) %>:</label>&nbsp;
            <%= Html.DropDownListExFor(x=>x.Filter.Name, Model.Names, new { @id="changeName", @class = "ss", style="width: 500px;" })%>&nbsp;
            <label for="changeProfile"><%= Html.DisplayNameFor(x => x.Filter.Profile) %>:</label>&nbsp;
            <%= Html.DropDownListExFor(x=>x.Filter.Profile, Model.Profiles, new { @id="changeProfile", @class = "ss", style="width: 300px;" })%>
        </div>

        <div class="tableHeader2l tableHeaderCollapsed" style="height: 100%">
            <div id="divFilterPlace">
                <div class="hideTable" onclick="ToggleFilter()" style="float: left"><span id="btnShowFilter">Отобразить фильтр</span> </div>
            </div>

            <div class="appCount">Записей:&nbsp;<span id="totalcount" style="color: crimson">0</span></div>

            <div id="messageFilter" style="text-align: right; display: none" >
                <span id="btnClear" class="linkSumulator" style="color: salmon; font-size: 11px; font-weight: bold; padding-right: 5px;">[Внимание! Применен фильтр] (сбросить)</span>
            </div>

            <div id="filters" style="display: none; clear: both; padding-left: 0; margin-left:15px;">
                <div>
                    <label for="changeLastName"><%= Html.DisplayNameFor(x => x.Filter.LastName) %>:</label>&nbsp;
                <%= Html.TextBoxExFor(x=>x.Filter.LastName, new { @id="changeLastName",  @class = "ss", style="width: 300px; margin-left: 20px;" })%>&nbsp;

                <span style="margin-left: 250px;">
                    <label for="changeResultLevelID"><%= Html.DisplayNameFor(x => x.Filter.ResultLevelID) %>:</label>&nbsp;
                    <%= Html.DropDownListExFor(x=>x.Filter.ResultLevelID, Model.ResultLevels, new { @id="changeResultLevelID",  @class = "ss", style="width: 300px;" })%>&nbsp;
                </span>
                </div>
                <div>
                    <label for="changeFirstName"><%= Html.DisplayNameFor(x => x.Filter.FirstName) %>:</label>&nbsp;
                <%= Html.TextBoxExFor(x=>x.Filter.FirstName, new { @id="changeFirstName",  @class = "ss", style="width: 300px; margin-left: 53px;" })%>&nbsp;

                <span style="margin-left: 250px;">
                    <label for="changeDiplomaSeries"><%= Html.DisplayNameFor(x => x.Filter.DiplomaSeries) %>:</label>&nbsp;
                    <%= Html.TextBoxExFor(x=>x.Filter.DiplomaSeries, new { @id="changeDiplomaSeries",  @class = "ss", style="width: 80px; margin-left: 15px;" })%>&nbsp;

                    <label for="changeDiplomaNumber"><%= Html.DisplayNameFor(x => x.Filter.DiplomaNumber) %>:</label>&nbsp;
                    <%= Html.TextBoxExFor(x=>x.Filter.DiplomaNumber, new { @id="changeDiplomaNumber",  @class = "ss", style="width: 80px;" })%>&nbsp;
                </span>
                </div>
                <div>
                    <label for="changeMiddleName"><%= Html.DisplayNameFor(x => x.Filter.MiddleName) %>:</label>&nbsp;
                <%= Html.TextBoxExFor(x=>x.Filter.MiddleName, new { @id="changeMiddleName",  @class = "ss", style="width: 300px; margin-left: 19px;" })%>&nbsp;

                <span style="margin-left: 250px;">
                    <label for="changeEndingDate"><%= Html.DisplayNameFor(x => x.Filter.EndingDate) %>:</label>&nbsp;
                    <%= Html.DropDownListExFor(x=>x.Filter.EndingDate, Model.EndingDates, new { @id="changeEndingDate", @class = "ss", style="width: 300px;" })%>
                </span>
                </div>
                <div>
                    <label for="changeDocumentSeries"><%= Html.DisplayNameFor(x => x.Filter.DocumentSeries) %>:</label>&nbsp;
                <%= Html.TextBoxExFor(x=>x.Filter.DocumentSeries, new { @id="changeDocumentSeries",  @class = "ss", style="width: 80px;" })%>&nbsp;

                <span style="margin-left: 16px;">
                    <label for="changeDocumentNumber"><%= Html.DisplayNameFor(x => x.Filter.DocumentNumber) %>:</label>&nbsp;
                    <%= Html.TextBoxExFor(x=>x.Filter.DocumentNumber, new { @id="changeDocumentNumber",  @class = "ss", style="width: 80px;" })%>&nbsp;
                </span>

                    <span style="margin-left: 204px;">
                        <label for="changeStatusID"><%= Html.DisplayNameFor(x => x.Filter.StatusID) %>:</label>&nbsp;
                    <%= Html.DropDownListExFor(x=>x.Filter.StatusID, Model.Status, new { @id="changeStatusID", @class = "ss", style="width: 300px; margin-left: 24px;" })%>&nbsp;
                    </span>
                </div>
            </div>

        </div>

        <div id="tablerows">
            <% Html.RenderPartial("OlympicDiplomant/OlympicDiplomantTableView", Model); %>
        </div>

        <div id="oncomplete"></div>
    </div>
</asp:Content>


