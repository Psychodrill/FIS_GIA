<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.Portlets.Entrants.EntrantLanguageViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<div id="content">
    <table class="gvuzDataGrid" style="width: 400px">
        <thead>
            <tr><th class="caption" align="center">Иностранные Языки</th></tr>
        </thead>
        <tbody>
            <% int cnt = 0;
               foreach (string s in Model.LanguageDataView)
               {
                   cnt ++; %>
                <tr class="<%= cnt%2 == 0 ? "trline2" : "trline1" %>"><td><%: s %></td></tr>
            <% } %>
            <% if (Model.LanguageDataView.Length == 0)
               { %><tr><td align="center" class="trline1">Нет языков</td></tr> <% } %>
        </tbody>
    </table>
    <% if (Model.ApplicationStep == 0)
       { %>
        <br />
        <div>
            <a id="btnEdit" href="<%= Url.Generate<EntrantController>(c => c.LanguagesEdit()) %>">Редактировать</a>
        </div>

        <script type="text/javascript">
            jQuery(document).ready(function() {
                jQuery('#btnEdit').button();
            })
        </script>
    <% } %>
</div>