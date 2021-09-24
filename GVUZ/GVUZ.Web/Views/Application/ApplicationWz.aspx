<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.ApplicationWzModel>" %>

<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Обработка заявлений</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PageTitle" runat="server">
    Заявление</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PageSubtitle" runat="server">
    entrantFullName
    <%--<% if ((Model.ApplicationNumber != null) & (Model.ApplicationNumber != ""))
       {%>
    <h2 class="title" >
        Номер заявления:&nbsp </h2>
    <h2 class="subtitle" >
        <%=Model.ApplicationNumber == null ? "" : Model.ApplicationNumber.ToString()%></h2>
    <% } %>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        div.tableStatement2
        {
            border: 0px;
        }
        div.tableStatement2 a.btnDelete
        {
            background-position-y: 2px;
            height: 15px;
            margin-top: 0px;
            margin-bottom: 1px;
        }
        .wizardPanel
        {
            /*float: left;*/
            min-height: 300px;
            width: 100%;
        }
        .stop
        {
            display: none;
        }
    </style>
    <script language="javascript" type="text/javascript">
        // region расширения для объектов времение (Date) Функции форматирования даты
        Date.prototype.DDMMYYYY = function () {
            var dd = this.getDate();
            if (dd < 10) dd = '0' + dd;
            var mm = this.getMonth() + 1;
            if (mm < 10) mm = '0' + mm;
            var yyyy = this.getFullYear();
            var yy = this.getFullYear() % 100;
            if (yy < 10) yy = '0' + yy;
            return dd + '.' + mm + '.' + yyyy;
        };

        Date.prototype.DDMMYY = function () {
            var dd = this.getDate();
            if (dd < 10) dd = '0' + dd;
            var mm = this.getMonth() + 1;
            if (mm < 10) mm = '0' + mm;
            var yy = this.getFullYear() % 100;
            if (yy < 10) yy = '0' + yy;
            return dd + '.' + mm + '.' + yy;
        };

        Date.prototype.YYYYMMDD = function () {
            var dd = this.getDate();
            if (dd < 10) dd = '0' + dd;
            var mm = this.getMonth() + 1;
            if (mm < 10) mm = '0' + mm;
            var yyyy = this.getFullYear();
            return ("" + yyyy + "-" + mm + "-" + dd);
        };
        // REGION  Функции для работы с ajax
        // Конвертор даты \/Date(-606769200000-0500)\/ http://sudarshanbhalerao.wordpress.com/2011/08/14/convert-json-date-into-javascript-date/
        function parseMSJsonDate(jsonDate) {
            if (jsonDate == null) {
                return null;
            }
            //var offset = new Date().getTimezoneOffset() * 60000;
            var offset = 0;
            var parts = /\/Date\((-?\d+)([+-]\d{2})?(\d{2})?.*/.exec(jsonDate);
            if (parts[2] == undefined) {
                parts[2] = 0;
            }
            if (parts[3] == undefined) {
                parts[3] = 0;
            }
            return new Date(+parts[1] + offset + parts[2] * 3600000 + parts[3] * 60000);
        };
        Date.prototype.parseMSJsonDate = parseMSJsonDate;
        // end region        
    </script>
    <script language="javascript" type="text/javascript">
    var ApplicationId=<%=Model.ApplicationID %>;
    var ApplicationNumber = '<%=Model.ApplicationNumber%>';
    var InstitutionID=<%=(Model.InstitutionID!=null?Model.InstitutionID:0)  %>;
    var WizStep= <%=(Model.WizardStepID!=null?Model.WizardStepID:0) %>;
    var WzCount=6;
    var WzObj={};
    var WzObjs=[];
    var ClickOuterElementSave = true; //параметр для отслеживания изменений на странице (по умолчанию false)

    $(document).ready(function() {
        $('.pageHeader a').click(doClickOuterElement);

        if (ApplicationId > 0 && WizStep > 0) {
            ShowWz(WizStep);
        } else {
            NewApplication();
        }
    });
       
    function doClickOuterElement() {
        if (ClickOuterElementSave) {
            var $el = $(this);
            var url = $el.attr('href');
            $('<div>Вы хотите сохранить заявление перед уходом с данной страницы?</div>').dialog({
                width: '400px',
                modal: true,
                buttons: {
                    "Сохранить": function() {
                        SaveWz(
                            function() { //success
                                closeDialog($(this));
                                if (url != "") {
                                    window.location = url;
                                }
                            },
                            function(msg) {
                                infoDialog(msg);
                                return false;
                            }, WizStep); // Какой выставить текущий шаг визарда

                    },
                    "Не сохранять": function() {
                        closeDialog($(this));
                        if (url != "") {
                            window.location = url;
                        }
                    },
                    "Отмена": function() { closeDialog($(this)) }
                }
            });
            return false;
        }
        return true;
    }

    function NewApplication() {
        ApplicationId = 0;
        $('#ApplicationNumber').val('');
        $('#RegistrationDate').val((new Date()).DDMMYYYY());
        ShowWz(0);
    }

    function WzNext() {
        // blockUI();
        if (WizStep < WzCount) {
            SaveWz(
                function() { //success
                    if (WizStep == 0 && ApplicationId > 0) {
                        window.location = '<%= Url.Generate<ApplicationController>(x => x.Wz(null)) %>?id=' + ApplicationId;
                    } else {
                        if (WizStep == 5) {
                            window.location = '<%= Url.Generate<InstitutionApplicationController>(x => x.ApplicationList()) %>';
                        } else {
                            ShowWz(WizStep + 1);
                        }
                    }
                },
                function(msg) { //error
                    infoDialog(msg);
                    return false;
                }, WizStep + 1); // Какой выставить текущий шаг визарда
        } else {
            window.location = '<%= Url.Generate<InstitutionApplicationController>(x => x.ApplicationList()) %>'; // Перейти на общий список
        }
    }

    function WzPrev() {
        // blockUI();
        if (WizStep == 0 || WizStep == 1) {
            window.location = '<%= Url.Generate<InstitutionApplicationController>(x => x.ApplicationList()) %>'; // Перейти на общий список
        }
        if (WizStep <= 4) {
            SaveWz(
                function(model) { //success
                    ShowWz(WizStep - 1); // Какой выставить текущий шаг визарда
                },
                function(msg) { //error
                    infoDialog(msg);
                    return false;
                }, WizStep - 1); // Какой выставить текущий шаг визарда
        }
        if (WizStep == 5) {
            SaveW5z(
                function(model) { //success
                    ShowWz(WizStep - 1); // Какой выставить текущий шаг визарда
                }, function(msg) { //error
                    infoDialog(msg);
                    return false;
                }, WizStep - 1
            );
        }
    }

    function WzCancel() {
        var url = '<%= Url.Generate<InstitutionApplicationController>(x => x.ApplicationList()) %>';

        var rx = /^#tab(\d)$/i;
        if (rx.test(window.location.hash)) {
            url += window.location.hash;
        }

        window.location = url;
    }

    function ShowInfo(msg, type) {
        var e = $('#MessagePlace');
        e.text(msg);
        if (type == 'info') {
            e.css('color', 'blue');
        }
        if (type == 'error') {
            e.css('color', 'red');
        }
        e.show();
    }

    function ShowExWz(step) {
        //debugger;
        WizStep = step;
        $('#wizardPanel').empty();
        $('#btnWz5SaveTop').hide();
        $('#btnWz5SaveBottom').hide();

        if (step == 0) {
            $('#btnAppSaveTop').hide();
            $('#btnAppSaveBottom').hide();
        }
        if (step == 5) {
            $('#btnAppForwardTop').text('Сохранить');
            $('#btnAppForwardBottom').text('Сохранить');

            $('#btnWz5SaveTop').show();
            $('#btnWz5SaveBottom').show();

        } else {
            $('#btnAppForwardTop').text('Далее');
            $('#btnAppForwardBottom').text('Далее');
        }

        $('#btnAppForwardTop').show();
        $('#btnAppForwardBottom').show();

        switch (step) {
        case 0:
            ShowExWz0();
            break;
        case 1:
            ShowExWz1();
            break;
        case 3:
            ShowExWz3();
            break;
        case 2:
            ShowExWz2();
            break;
        case 4:
            ShowExWz4();
            break;
        case 5:
            ShowExWz5();
            break;
        }
    }

    function SaveWz(success, error, step) {
        var res = true;
        if (step == undefined) {
            step = WizStep;
        }
        switch (WizStep) {
        case 0:
            res = Wz0Save(success, error, step);
            break;
        case 1:
            res = Wz1Save(success, error, step);
            break;
        case 3:
            res = Wz3Save(success, error, step);
            break;
        case 2:
            res = Wz2Save(success, error, step);
            break;
        case 4:
            res = Wz4Save(success, error, step);
            break;
        case 5:
            res = Wz5Save(success, error, step);
            break;
        }
        return res;
    }

    function _SaveWz5() {
        SaveWz5(
            function() { // success
                window.location = '<%= Url.Generate<InstitutionApplicationController>(x => x.ApplicationList()) %>'; // Перейти на общий список      
            }, function() { // error

            });
    }

    function ShowWz(step) {
        if (step == undefined) {
            step = WizStep;
        }
        $('#wizardPanel').hide();
        $('#wizardPanel').empty();
        if (ApplicationId == 0) {
            ShowExWz(step);
            return;
        } else {
            ShowExWz(step);
            return;
        }
    }

    function ShowExWz0() {
        <% if ((Model.ApplicationNumber != null) & (Model.ApplicationNumber != ""))
           {%>
            $('#pageTitleH2').text('Ввод заявления № ' + '<%=Model.ApplicationNumber == null ? "" : Model.ApplicationNumber.ToString()%>');
        <% } %>
        $('#pageSubtitleH2').text(' Шаг 1: Создание заявления');
        // Проверить Загружена ли страница. Стоит ли ее перегружать
        doPostAjax('<%= Url.Generate<ApplicationController>(x => x.Wz0(null)) %>', 'InstitutionID=' + InstitutionID,
            function(data) {
                $('#wizardPanel').html(data);
                Wz0Init();
                $('#wizardPanel').show();
              // unblockUI();
            }, "application/x-www-form-urlencoded", "html");
    }

    function ShowExWz1() {
        <% if ((Model.ApplicationNumber != null) & (Model.ApplicationNumber != "")) { %>
            $('#pageTitleH2').text('Ввод заявления № ' + '<%=Model.ApplicationNumber == null ? "" : Model.ApplicationNumber.ToString()%>');
        <% } %>
        $('#pageSubtitleH2').text(' Шаг 2: Ввод личных данных ');
        doPostAjax('<%= Url.Generate<ApplicationController>(x => x.Wz1(null)) %>', 'id=' + ApplicationId,
            function(data) {
                $('#wizardPanel').html(data);
                Wz1Init();
                $('#wizardPanel').show();
            }, "application/x-www-form-urlencoded", "html");
    }

    function ShowExWz3() {
        <% if ((Model.ApplicationNumber != null) & (Model.ApplicationNumber != "")) { %>
            $('#pageTitleH2').text('Ввод заявления № ' + '<%=Model.ApplicationNumber == null ? "" : Model.ApplicationNumber.ToString()%>');
        <% } %>
        $('#pageSubtitleH2').text(' Шаг 4: Вступительные испытания');
        doPostAjax('<%= Url.Generate<ApplicationController>(x => x.Wz3(null)) %>', 'id=' + ApplicationId,
            function(data) {
                $('#wizardPanel').html(data);
                Wz3Init();
                $('#wizardPanel').show();
            }, "application/x-www-form-urlencoded", "html");
    }

    function ShowExWz2() {
        <% if ((Model.ApplicationNumber != null) & (Model.ApplicationNumber != "")) { %>
            $('#pageTitleH2').text('Ввод заявления № ' + '<%=Model.ApplicationNumber == null ? "" : Model.ApplicationNumber.ToString()%>');
        <% } %>
        $('#pageSubtitleH2').text(' Шаг 3: Прилагаемые документы');
        doPostAjax('<%= Url.Generate<ApplicationController>(x => x.Wz2(null)) %>', 'id=' + ApplicationId,
            function(data) {
                $('#wizardPanel').html(data);
                Wz2Init();
                $('#wizardPanel').show();
            }, "application/x-www-form-urlencoded", "html");
    }

    function ShowExWz4() {
        <% if ((Model.ApplicationNumber != null) & (Model.ApplicationNumber != "")) { %>
            $('#pageTitleH2').text('Ввод заявления № ' + '<%=Model.ApplicationNumber == null ? "" : Model.ApplicationNumber.ToString()%>');
        <% } %>
        $('#pageSubtitleH2').text(' Шаг 5: Индивидуальные достижения');
        doPostAjax('<%= Url.Generate<ApplicationController>(x => x.Wz4(null)) %>', 'id=' + ApplicationId,
            function(data) {
                $('#wizardPanel').html(data);
                Wz4Init();
                $('#wizardPanel').show();
            }, "application/x-www-form-urlencoded", "html");
    }

    function ShowExWz5() {
        <% if ((Model.ApplicationNumber != null) & (Model.ApplicationNumber != "")) { %>
            $('#pageTitleH2').text('Ввод заявления № ' + '<%=Model.ApplicationNumber == null ? "" : Model.ApplicationNumber.ToString()%>');
        <% } %>
        $('#pageSubtitleH2').text(' Шаг 6: Общие сведения');
          doPostAjax('<%= Url.Generate<ApplicationController>(x => x.Wz5(null)) %>', 'id=' + ApplicationId,
              function(data) {
                  $('#wizardPanel').html(data);
                  Wz5Init();
                  $('#wizardPanel').show();
              }, "application/x-www-form-urlencoded", "html");
      }
    </script>
    <div class="navigation">
        <a id="btnAppBackTop" href="javascript:void(0);" onclick="WzPrev();" class="wzButton back">Назад</a>
        <a id="btnAppForwardTop" href="javascript:void(0);" onclick="WzNext();" class="wzButton forvard">Далее</a>
        <a id="btnWz5SaveTop" href="javascript:void(0);" onclick="_SaveWz5();" class="wzButton forvard">Сохранить без проверки в списке новых заявлений</a>
        <a id="btnAppCancelTop" href="javascript:void(0);" onclick="WzCancel();" class="wzButton cancel right">Отмена</a>
        <a id="btnAppSaveTop" href="javascript:void(0);" onclick="SaveWz();" class="wzButton stop right">Приостановить добавление заявления</a>
    </div>
    <div id="wizardPanel" class="wizardPanel" style="display: none">
    </div>
    <div class="navigation">
        <a id="btnAppBackBottom" href="javascript:void(0);" onclick="WzPrev();" class="wzButton back">
            Назад</a>
        <a id="btnAppForwardBottom" href="javascript:void(0);" onclick="WzNext();" class="wzButton forvard">Далее</a>
        <a id="btnWz5SaveBottom" href="javascript:void(0);" onclick="_SaveWz5();" class="wzButton forvard">Сохранить без проверки в списке новых заявлений</a>
        <a id="btnAppCancelBottom" href="javascript:void(0);" onclick="WzCancel();" class="wzButton cancel right">Отмена</a>
        <a id="btnAppSaveBottom" href="javascript:void(0);" onclick="SaveWz();" class="wzButton stop">Приостановить добавление заявления</a>
    </div>
    <div id="MessagePlace" style="color: Blue; display: none">
    </div>
</asp:Content>
