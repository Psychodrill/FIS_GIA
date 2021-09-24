<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.ImportDictionaryViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.ViewModels" %>

<asp:Content ID="Title" ContentPlaceHolderID="TitleContent" runat="server">
    Синхронизация справочников с НСИ
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">	
    <h2>Синхронизация справочников с НСИ</h2>
    <button id="btnSyncAll">Синхронизировать все</button>
    <table class="gvuzDataGrid"	style="margin-top: 10px;"	>
        <thead>
            <tr>
                <th>
                    <%= Html.LabelFor(x => x.Description.DictionaryName) %>
                </th>
                <th id="thAction" style="width: 200px;">
                    <%= Html.LabelFor(x => x.Description.DictionaryID) %>
                </th>
                <th >
                    Результат синхронизации
                </th>
            </tr>
        </thead>
        <tbody>
            <% foreach (ImportDictionaryItem dictionaryItem in Model.Dictionaries)
               { %>
                <tr>
                    <td align="center">
                        <%: dictionaryItem.DictionaryName %>
                    </td>
                    <td align="center">
                        <button itemid="<%= dictionaryItem.DictionaryID %>">Синхронизировать</button>					
                    </td>
                    <td align="center">
					
                    </td>
                </tr>
            <% } %>
        </tbody>
    </table>

    <script type="text/javascript">
        var syncAll = null;

        jQuery(function() {
            jQuery('button[itemID]').click(function(event, data) {
                var $linkObj = $(event.currentTarget);
                doPostAjax('<%= Url.Generate<ImportDicController>(x => x.SyncDictionary(null)) %>',
                    'dictionaryID=' + $linkObj.attr('itemID'),
                    function(data) {
                        if (!data.IsError) {
                            $linkObj.next().remove();
                            $linkObj.parent().next().text(data.Data);
                            $linkObj.show();
                        }
                        if (isSyncAllActive()) callSyncForButton();
                    }, 'application/x-www-form-urlencoded', 'json');
                $linkObj.after('<span>Синхронизация...</span>');
                $linkObj.hide();
            });
            jQuery('#btnSyncAll').click(function() {
                callSyncForButton();
                jQuery(this).attr('disabled', 'disabled');
            });
            jQuery('button').button({ icons: { primary: 'ui-icon-refresh' } });
        });

        function callSyncForButton() {
            if (syncAll == null)
                syncAll = 0;
            else
                syncAll++;

            if (syncAll < 0 || syncAll >= jQuery('button[itemid]').length)
                clearSyncAll();
            else
                jQuery(jQuery('button[itemid]')[syncAll]).click();
        }

        function isSyncAllActive() {
            return syncAll != null;
        }

        function clearSyncAll() {
            syncAll = null;
            jQuery('#btnSyncAll').removeAttr('disabled');
        }

    </script>

</asp:Content>