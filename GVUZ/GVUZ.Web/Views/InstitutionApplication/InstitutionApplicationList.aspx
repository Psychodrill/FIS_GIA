<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.ApplicationsList.NewInstitutionApplicationListViewModel>" %>
<%@ Import Namespace="GVUZ.Web.ViewModels.ApplicationsList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"> Обработка заявлений</asp:Content>
<asp:Content ContentPlaceHolderID="PageHeaderContent" runat="server">
    <%-- Подгружаем скрипты для работы с data-bound гридом, для отладки свои скрипты грузим отдельными файлами, потом надо сделать bundle --%>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/libs/knockout-3.3.0.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/libs/knockout.mapping-latest.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/WebUtils.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/grid/FilterViewModelBase.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/grid/PagerViewModel.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/grid/ListViewModelBase.js") %>"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="PageTitle" runat="server"> Заявления</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script language="javascript" type="text/javascript">
      var tabControl;
      var initialPageNumber = <%= Model.InitialPage.GetValueOrDefault(1) %>;
      var initialHighlightValue = <%= Model.HighlightApplicationId.GetValueOrDefault(0) %>;
      
      function LoadTab(tabId, noClear) {

          function initialPage() {
              return initialPageNumber || '';
          }

          function initialHighlight() {
              return initialHighlightValue || '';
          }

          function tabUrl(template) {
              return template.replace('INITIALPAGE', initialPage()).replace('HIGHLIGHT', initialHighlight());
          }

          switch(tabId){
              case 1:
                  tabControl.loadHtml(tabUrl('<%= Url.Action("UncheckedList", new { highlightApplicationId = "HIGHLIGHT", initialPage = "INITIALPAGE" }) %>'), null, function () {
                      window.setTimeout(uncheckedListLoaded, 100);
                  });
                  break;
              case 2:
                  tabControl.loadHtml(tabUrl('<%= Url.Action("RevokedList", new { highlightApplicationId = "HIGHLIGHT", initialPage = "INITIALPAGE" }) %>'), null, function () {
                      window.setTimeout(revokedListLoaded, 100);
                  });
                  break;
              case 3:
                  tabControl.loadHtml(tabUrl('<%= Url.Action("AcceptedList", new { highlightApplicationId = "HIGHLIGHT", initialPage = "INITIALPAGE" }) %>'), null, function () {
                      window.setTimeout(acceptedListLoaded, 100);
                  });
                  break;
                  <%-- case 4:  
              tabControl.loadHtml('<%= Url.Action("RecommendedList") %>', null, function() {
                  window.setTimeout(recommendedListLoaded, 100);
              });
              break;--%>

                  <%-- case 0:
              tabControl.loadHtml(tabUrl('<%= Url.Action("NewList", new { highlightApplicationId = "HIGHLIGHT", initialPage = "INITIALPAGE" }) %>'), null, function() {
                  window.setTimeout(newListLoaded, 100);
              });
              break;--%>
              default:
                  tabId = 0;
                  tabControl.loadHtml(tabUrl('<%= Url.Action("NewList", new { highlightApplicationId = "HIGHLIGHT", initialPage = "INITIALPAGE" }) %>'), null, function() {
                      window.setTimeout(newListLoaded, 100);
                  });
                  break;
          }  
          
          tabControl.setSelectedTab(tabId);
          window.location.hash = '#tab' + tabId.toString();
          initialPageNumber = null;
          initialHighlightValue = null;
          
          if (!noClear) {
            //console.log('clear !')
            document.cookie = 'ApplicationListPage' + '=; expires=Thu, 01-Jan-70 00:00:01 GMT;';
          }
      }

      (function($) {
          $(document).ready(function () {
              TabControl.prototype.loadHtml = function(url, urlParameters, callback) {
                  doGetAjax(url, urlParameters || {},
                      function (data) {
                          $('#TabPlace').html(data); 
                          if (callback && typeof callback === 'function') {
                              callback.call(this);
                          }
                      }, "application/x-www-form-urlencoded", "html"
                  );
              };

              TabControl.prototype.setSelectedTab = function(tabIndex) {
                  $('#filterTab > a.select').removeClass('select');
                  $('#filterTab > a:eq(' + Number(tabIndex).toString() + ')').addClass('select');
              };
          
              tabControl = new TabControl($('#filterTab'), [
                      { name: 'Новые', link: 'javascript:LoadTab(0)', enable: true, selected: true, noWrap: true },
                      { name: 'Не прошедшие проверку', link: 'javascript:LoadTab(1)', enable: true, noWrap: true },
                      { name: 'Отозванные', link: 'javascript:LoadTab(2)', enable: true, noWrap: true },
                      { name: 'Принятые', link: 'javascript:LoadTab(3)', enable: true, noWrap: true }
                      //,{ name: 'Рекомендованные к зачислению', link: 'javascript:LoadTab(4)', enable: true, noWrap: true }
                  ]
              );
              tabControl.init();
              $('#filterTab > a:first').addClass('menuiteml');
              
              var requestedTab = null;
              var rx = /^#tab(\d)$/i;
              if (rx.test(window.location.hash)) 
              {
                 requestedTab = parseInt(window.location.hash.match(rx)[1]);
              }
              if (!requestedTab || isNaN(requestedTab)) {
                  requestedTab = <%= Model.InitialTab %>;
              }
               
              LoadTab(requestedTab, true);
          });
      })(jQuery);
  </script>
  <% Html.RenderPartial("InstitutionApplication/Dialogs/CheckApplications", CheckApplicationsListViewModel.MetadataInstance); %>
  <div class="divstatement">
    <div id="filterTab" class="gvuzTab submenu"></div>
    <div id="temporary content">

                      <tb colspan="6" style="text-align: center;">
							<font size="4" color="red" face="Arial">
Уважаемые пользователи, в связи с возросшей нагрузкой на Систему возможно увеличение срока ожидания при создании Заявления. Рекомендуем повторить создание заявление через 30 минут.
							</font>
                     </tb>

	</div>
    <div id="TabPlace" style="padding:0px; margin:0px; border:0px;"></div>
  </div>
  
</asp:Content>
