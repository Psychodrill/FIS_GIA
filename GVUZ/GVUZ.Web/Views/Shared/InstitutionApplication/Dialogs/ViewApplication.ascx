<%-- Диалог просмотра заявления --%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>

<div id="DetailsDialog" style="display: none"> </div>
<script language="javascript" type="text/javascript">

    function viewApplicationDetails(id, appNumber, campaignIsFinished, statusID) {
        var $container = $('#DetailsDialog');

        doPostAjax('<%= Url.Generate<ApplicationController>(x => x.ApplicationView(null)) %>/?id=' + id, '', function (data) {
            $container.html(data).dialog({          
                modal: true,
                width: 1000,
                height: 600,
                title: "Заявление №" + appNumber,
                buttons: [
                    {
                        id: "appEdit",
                        disabled: campaignIsFinished || statusID == 6,  //movaxcs(1705)
                        text: "Редактировать",
                        click: function ()
                        {
                            var rx = /^#tab(\d)$/i;
                            var requestedTab = 0;
                            if (rx.test(window.location.hash)) {
                                requestedTab = parseInt(window.location.hash.match(rx)[1]);
                            }

                            var tabHash = '';
                            if (requestedTab != '0') {
                                tabHash = '#tab' + requestedTab;
                            }
                            window.location = '/Application/' + (statusID == 1 ? 'Wz' : 'Edit') + '/' + id + tabHash;
                        }
                    },
                    {
                        text: "Закрыть", 
                        click: function () { $(this).dialog("close"); }
                    }],
                    close: function () {
                        $container.html("");
                    },
                    open: function () { }
                }).dialog('open');
        }, "application/x-www-form-urlencoded", "html");
    }
</script>

