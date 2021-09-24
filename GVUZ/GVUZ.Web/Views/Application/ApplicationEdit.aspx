<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.ApplicationEditModel>" %>

<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Ввод заявления
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PageTitle" runat="server">
    Редактирование заявления:
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PageSubtitle" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        div.tableStatement2 {
            border: 0px;
        }

            div.tableStatement2 a.btnDelete {
                background-position-y: 2px;
                height: 15px;
                margin-top: 0px;
                margin-bottom: 1px;
            }

        .wizardPanel {
            /*float: left;*/
            min-height: 300px;
        }

        .stop {
            display: none;
        }
    </style>
    <div class="divstatement">
        <div id="appEditTabPart" class="submenu">
        </div>
        <div class="navigation">
            <input class="button3" id="btnAppSaveETop" type="button" value="Сохранить" onclick="saveBut()" />
            <input class="button3" id="btnWz5SaveETop" type="button" value="Сохранить без проверки"
                onclick="_SaveWz5()" />
            <input class="button3" id="btnAppCancelETop" type="button" value="Отмена" onclick="cancelTab()" />
        </div>
        <div id="partialV" style="display: none">
        </div>
        <div class="navigation">
            <input class="button3" id="btnAppSaveE" type="button" value="Сохранить" onclick="saveBut()" />
            <input class="button3" id="btnWz5SaveE" type="button" value="Сохранить без проверки"
                onclick="_SaveWz5()" />
            <input class="button3" id="btnAppCancelE" type="button" value="Отмена" onclick="cancelTab()" />
        </div>
    </div>
    <script language="javascript" type="text/javascript">
        function cancelTab() {
            
            var url = '<%= Url.Generate<InstitutionApplicationController>(x => x.ApplicationList()) %>';
            
            var rx = /^#tab(\d)$/i;
            if (rx.test(window.location.hash)) {
                url += window.location.hash;
            }
            window.location = url;
        }

        function saveBut(success, error, newStep) {

            if (newStep == undefined) { newStep = WizStep; }
            if (success == undefined) {
                success = function () 
                {
                    cancelTab();
                    <%--var url = '<%= Url.Generate<InstitutionApplicationController>(x => x.ApplicationList()) %>';
                    window.location = url;--%>
                }
            }
            switch (WizStep) {
                case 1:
                    if (Wz1Save) {
                        Wz1Save(success, error, newStep);
                    }
                    break;
                case 3:
                    if (Wz3Save) {
                        Wz3Save(success, error, newStep);
                    }
                    break;
                case 2:
                    if (Wz2Save) {
                        Wz2Save(success, error, newStep);
                    }
                    break;
                case 4:
                    if (Wz4Save) {
                        Wz4Save(success, error, newStep);
                    }
                    break;
                case 5:
                    if (Wz5Save) {
                        Wz5Save(success, error, newStep);
                    }
                    break;
            }
        }

        function saveTab(success, error, newStep) {
            if (newStep == undefined) {
                newStep = WizStep;
            }
            switch (WizStep) {
                case 1:
                    if (Wz1Save) {
                        Wz1Save(success, error, newStep);
                    }
                    break;
                case 3:
                    if (Wz3Save) {
                        Wz3Save(success, error, newStep);
                    }
                    break;
                case 2:
                    if (Wz2Save) {
                        Wz2Save(success, error, newStep);
                    }
                    break;
                case 4:
                    if (W4zSave) {
                        W4zSave(success, error, newStep);
                    }
                    break;
                case 5:
                    if (SaveW5z) {
                        SaveW5z(success, error, newStep);
                    }
                    break;
            }
        }
        function _SaveWz5() {
            SaveWz5(
      function () { // success
          //window.location = '<%= Url.Generate<InstitutionApplicationController>(x => x.ApplicationList()) %>';          // Перейти на общий список      
          //history.back();
          cancelTab();
      }, function () {  // error
      }, 5);
        }
        <%-- Переопределение, cancelTab объявлена ранее --%>
        //        function cancelTab() {
        //            window.location = '<%= Url.Generate<InstitutionApplicationController>(x => x.ApplicationList()) %>';          // Перейти на общий список      
        //        }
    </script>

    <script language="javascript" type="text/javascript">
        var ApplicationId=<%=Model.ApplicationID %>;
        var InstitutionID=<%=(Model.InstitutionID!=null?Model.InstitutionID:0)  %>;

        var WizStep= <%=(Model.WizardStepID!=null?Model.WizardStepID:0) %>;
        var WzCount=6;
        var WzObj={};
        var WzObjs=[];


        $('#pageTitleH2').text('Редактирование заявления: ' + '<%=Model.ApplicationNumber %>');
        //$('#pageSubtitleH2').text('<%=Model.ApplicationNumber %>');

  var tabControl = new TabControl($('#appEditTabPart'), [
          { name: 'Ввод личных данных', link: 'javascript:switchAppTab(0)', enable: true, selected: true, noWrap: true },
         { name: 'Прилагаемые документы', link: 'javascript:switchAppTab(1)', enable: true, noWrap: true },
          { name: 'Вступительные испытания', link: 'javascript:switchAppTab(2)', enable: true, noWrap: true },
          { name: 'Индивидуальные достижения', link: 'javascript:switchAppTab(3)', enable: true, noWrap: true },
          { name: 'Общие сведения', link: 'javascript:switchAppTab(4)', enable: true, noWrap: true }
  ], { prefix: 'popup' }
  );
  function loadPaView(tabId) {
      WizStep=tabId+1;
      //if (WizStep == 5) {WizStep = WizStep + 1}
      $('#partialV').hide();
      $('#partialV').empty();
      $('#btnWz5SaveETop').hide();
      $('#btnWz5SaveE').hide();

      //movaxcs(1705)
      // эти кнопки могут дизэблиться на некоторых вкладках в зависимости от статуса заявления, 
      // а здесь делаем их доступными
      $('#btnAppSaveETop').setEnabled();
      $('#btnAppSaveE').setEnabled();

      switch (tabId) {
          case 0: doPostAjax('<%= Url.Generate<ApplicationController>(x => x.Wz1(null)) %>?id=' + ApplicationId, {}
         , function (data){
             $('#partialV').html(data);
             Wz1Init();   
             $('#partialV').show();
         }, "application/x-www-form-urlencoded", "html");
             break;
         case 2: doPostAjax('<%= Url.Generate<ApplicationController>(x => x.Wz3(null)) %>?id=' + ApplicationId, {}
         , function (data){
             $('#partialV').hide();
             $('#partialV').html(data);
             Wz3Init();   
             $('#partialV').show();       }, "application/x-www-form-urlencoded", "html");
             break;
         case 1: doPostAjax('<%= Url.Generate<ApplicationController>(x => x.Wz2(null)) %>?id=' + ApplicationId, {}
         , function (data){
             $('#partialV').hide();
             $('#partialV').html(data);
             Wz2Init();   
             $('#partialV').show();
         }, "application/x-www-form-urlencoded", "html");
             break;
         case 3: doPostAjax('<%= Url.Generate<ApplicationController>(x => x.Wz4(null)) %>?id=' + ApplicationId, {}
         , function (data){
             $('#partialV').hide();
             $('#partialV').html(data);
             Wz4Init();   
             $('#partialV').show();
         }, "application/x-www-form-urlencoded", "html");
             break;
         case 4: doPostAjax('<%= Url.Generate<ApplicationController>(x => x.Wz5(null)) %>?id=' + ApplicationId, {}
         , function (data){
             $('#partialV').hide();
             $('#partialV').html(data);
             $('#btnWz5SaveETop').show();
             $('#btnWz5SaveE').show();
             Wz5Init();   
             $('#partialV').show();
         }, "application/x-www-form-urlencoded", "html");
             break;
         default:
             return;
     }
 }
 $(document).ready(function ()  {
     $('.pageHeader a').click(doClickOuterElement);
     switchAppTab(0, false);
 });
 function doClickOuterElement(){
     var $el = $(this);
     var url=$el.attr('href');
     $('<div>Вы хотите сохранить заявление перед уходом с данной страницы?</div>').dialog({
         width: '400px',
         modal: true,
         buttons: {
             "Сохранить": function() {
                 saveBut(  
                   function(){  //success
                       closeDialog($(this));
                       if(url!=""){  
                           window.location=url;
                       }
                   },
                   function(msg){
                       infoDialog(msg);
                       return false;
                   });  // Какой выставить текущий шаг визарда
             },
             "Не сохранять": function() {
                 closeDialog($(this));
                 if(url!=""){  
                     window.location=url;
                 }
             },
             "Отмена": function() { closeDialog($(this)) }
         }
     });
     return false;
 }
    </script>
    <script language="javascript" type="text/javascript">
        function switchAppTab(tab, withSave) {
            var newStep = tab+1;
            if (withSave == undefined) { withSave = true; }
            if (withSave) {
                saveTab(function() { // success
                    for (var i = 0; i < tabControl.menuItems.length; i++) {
                        tabControl.menuItems[i].selected = false;
                    }
                    tabControl.menuItems[tab].selected = true;
                    tabControl.init();
                    loadPaView(tab);
                }, function(msg) { // error
                    if (msg == undefined) {
                        msg = '';
                    }
                    infoDialog('Ошибка сохранения! ' + msg);
                }, newStep);
            } else {
                for (var i = 0; i < tabControl.menuItems.length; i++) { tabControl.menuItems[i].selected = false; }
                tabControl.menuItems[tab].selected = true;
                tabControl.init();
                loadPaView(tab);
            }
        }
    </script>
</asp:Content>
