<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
    function excludeApplicationFromOrderDialog(orderId, applicationItemId, callback, callbackscope) {
        var applicationItemIds = $.isArray(applicationItemId) ? applicationItemId : [applicationItemId];
        if (applicationItemIds.length == 0) {
            return;
        }
        var confirmMessage = (applicationItemIds.length == 1) ? 'Вы уверены, что хотите исключить заявление из приказа?' : 'Вы уверены, что хотите исключить выбранные заявления из приказа?';
        
        confirmDialog(confirmMessage, function () {
            doPostAjax('<%= Url.Action("ExcludeApplicationsFromOrder", "OrderOfAdmission") %>', JSON.stringify({ orderId: orderId, applicationItemIds: applicationItemIds }), function (res) {
                if (res.success) {
                    if (callback && typeof callback === 'function') {
                        callback.call(callbackscope || window, res.data);
                    }
                } else {
                    if (res.errorMessage) {
                        infoDialog(res.errorMessage);
                    }
                }
            }, null, null, true);
        });
    }
</script>