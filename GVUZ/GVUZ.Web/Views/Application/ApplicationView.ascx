<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.ApplicationModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

<script type="text/javascript">
    var Application=<%=Model.ApplicationID%>;

    var tabControl = new TabControl(jQuery('#appDetailsTab'), [
            { name: 'Общие сведения', link: 'javascript:switchAppTab(0)', enable: true, selected: true, noWrap: true },
            { name: 'Личные данные', link: 'javascript:switchAppTab(1)', enable: true, noWrap: true },
            { name: 'Документы', link: 'javascript:switchAppTab(2)', enable: true, noWrap: true },
            { name: 'Испытания', link: 'javascript:switchAppTab(3)', enable: true, noWrap: true },
            { name: 'Индивидуальные достижения', link: 'javascript:switchAppTab(4)', enable: true }
            //,{ name: 'Печатные формы', link: 'javascript:switchAppTab(5)', enable: true }
    ], { prefix: 'popup' });

    tabControl.init();
    
    function loadPaView(tabId) {
        switch (tabId) {

            case 0:
                doPostAjax('<%= Url.Generate<ApplicationController>(x => x.ApplicationV0(null)) %>?id=' + Application, {}, function(data) { jQuery('#partialV').html(data); }, "application/x-www-form-urlencoded", "html");
                break;

            case 1:
                doPostAjax('<%= Url.Generate<ApplicationController>(x => x.ApplicationV1(null)) %>?id=' + Application, {}, function(data) { jQuery('#partialV').html(data); }, "application/x-www-form-urlencoded", "html");
                break;

            case 2:
                doPostAjax('<%= Url.Generate<ApplicationController>(x => x.ApplicationV2(null)) %>?id=' + Application, {}, function(data) { jQuery('#partialV').html(data); }, "application/x-www-form-urlencoded", "html");
                break;

            case 3:
                doPostAjax('<%= Url.Generate<ApplicationController>(x => x.ApplicationV3(null)) %>?id=' + Application, {}, function(data) { jQuery('#partialV').html(data); }, "application/x-www-form-urlencoded", "html");
                break;

            case 4:
                doPostAjax('<%= Url.Generate<ApplicationController>(x => x.ApplicationV4(null)) %>?id=' + Application, {}, function(data) { jQuery('#partialV').html(data); }, "application/x-www-form-urlencoded", "html");
                break;

            case 5:
                doPostAjax('<%= Url.Generate<ApplicationController>(x => x.ApplicationV5(null)) %>?id=' + Application, {}, function(data) { jQuery('#partialV').html(data); }, "application/x-www-form-urlencoded", "html");
                break;

            default:
                return;
        }
    }

    function switchAppTab(tab) {
        for (var i = 0; i < tabControl.menuItems.length; i++) { tabControl.menuItems[i].selected = false; }
        tabControl.menuItems[tab].selected = true;
        tabControl.init();
        loadPaView(tab);
    }

    loadPaView(0);
</script>

<style type="text/css">
    *:focus {
        outline: none
    }

    #partialV .tableApp2 td, .tableApp2 td label {
        padding-top: 0px;
        padding-bottom: 0px;
    }

        #partialV .tableApp2 td.caption label, .tableApp2 td input {
            margin-top: 0px !important;
            margin-bottom: 0px !important;
        }

    #partialV .menuitemr, .menuiteml {
        padding-left: 10px;
        padding-right: 10px;
    }

    #partialV .gvuzDataGrid input.numeric {
        padding-top: 2px;
        padding-bottom: 2px;
        margin-top: 0px;
        margin-bottom: 0px;
    }

    .TabContent > table tr > td:first-child {
        width: 200px;
    }
</style>

<div class="ui-dialog-content ui-widget-content" id="divPopupApplication" style="width: auto; height: auto; min-height: 40px; overflow: visible">
    <div class="gvuzPortlet divstatement" style="top: 45px; margin-bottom: 50px;">
        <div id="appDetailsTab" class="gvuzTab submenu">
        </div>
        <div id="partialV">
        </div>
    </div>
</div>
