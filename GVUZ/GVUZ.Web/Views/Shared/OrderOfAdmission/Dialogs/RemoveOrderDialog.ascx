<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<script type="text/javascript">
    function removeOrderOfAdmissionDialog(orderId, callback, callbackscope) {
        confirmDialog('Вы уверены, что хотите удалить приказ?', function () {
            doPostAjax('<%= Url.Action("RemoveOrder", "OrderOfAdmission") %>', JSON.stringify({ orderId: orderId }), function (res) {
                if (res.success) {
                    if (callback && typeof callback === 'function') {
                        callback.call(callbackscope || window);
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