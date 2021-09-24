<%@ Page Title="Title" Language="C#" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.OlympicFiles.OlympicFilesListViewModel>" MasterPageFile="../Shared/Site.Master" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

<%@ Register TagPrefix="gv" TagName="AdminMenuControl" Src="~/Views/Shared/Controls/AdminMenuControl.ascx" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">
    Представление сведений по ОШ
</asp:Content>
<asp:Content ContentPlaceHolderID="PageTitle" runat="server">
    Представление сведений по ОШ
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageHeaderContent">

</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <% ViewData["MenuItemID"] = 9; %>
    <div class="divstatement">
    <gv:AdminMenuControl ID="AdminMenuControl1" runat="server" />
    
    <div id="content" style="overflow: auto">
        <% if (Model.IsEmpty) { %>
        <p>Сведения отсутствуют</p>
        <a class="add uploadLink" href="javascript:void(0)">Загрузить новый файл</a>
        <% } else { %>
        <a class="add uploadLink" href="javascript:void(0)">Загрузить новый файл</a>
        <hr size="1"/>
        <table class="gvuzDataGrid tableStatement2">
            <colgroup>
                <col style="width: 40%;vertical-align: top"/>
                <col style="width: 10%;vertical-align: top"/>
                <col style="width: 50%;vertical-align: top"/>
            </colgroup>
            <thead>
                <tr>
                    <th><%= Html.LabelFor(x => x.Records[0].FileName) %></th>
                    <th><%= Html.LabelFor(x => x.Records[0].UploadDateText) %></th>
                    <th><%= Html.LabelFor(x => x.Records[0].Comments) %></th>
                </tr>
            </thead>
            <tbody>
                <% foreach (var record in Model.Records) { %>
                <tr>
                    <td><%= Html.ActionLink(record.FileName, "Download", new {id = record.Id }) %></td>
                    <td><%= record.UploadDateText  %></td>
                    <td><%= record.Comments %></td>
                </tr>
                <% } %>
            </tbody>
        </table>
        <% } %>
        
        
    </div>
</div>
<div id="uploadDialog" style="display: none"></div>
<script type="text/javascript">
    (function ($) {

        function uploadDialog() {
            doPostAjax('<%= Url.Action("AddDialog") %>', null, function (response) {
                $('#uploadDialog').empty().html(response).dialog({
                    width: 400,
                    height: 250,
                    title: 'Добавление сведений об ОШ',
                    buttons: [
                        {
                            text: 'Загрузить',
                            click: function () {
                                var $this = $(this);
                                uploadDialogSubmit(function () {
                                    $this.dialog('close');
                                    document.location.href = '<%= Url.Action("Index") %>';
                                });
                            }
                        },
                        {
                            text: 'Закрыть',
                            click: function () {
                                var $this = $(this);
                                $this.dialog('close');
                            }
                        }
                    ]
                }).dialog('open');
            }, "application/x-www-form-urlencoded", "html", true);
        }

        $(function () {
            $('a.uploadLink').bind('click', uploadDialog);
        });
    })(jQuery);
</script>
</asp:Content>

