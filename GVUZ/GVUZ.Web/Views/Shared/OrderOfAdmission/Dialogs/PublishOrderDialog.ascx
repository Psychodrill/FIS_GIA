<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
    function publishOrderOfAdmission(orderId, regNumber, regDate, orderName, orderUID, callback, callbackscope) {
        doPostAjax('<%= Url.Action("ValidatePublication", "OrderOfAdmission") %>', JSON.stringify({ orderId: orderId, regNumber: regNumber, regDate: regDate, orderName: orderName, orderUID: orderUID }), function (res) {
            if (res.success) { 
                confirmDialog(res.confirmationMessage, function () {
                    doPostAjax('<%= Url.Action("CommitPublication", "OrderOfAdmission") %>', JSON.stringify({ orderId: orderId, regNumber: regNumber, regDate: regDate, orderName: orderName, orderUID: orderUID }), function (pubResult) {
                        if (pubResult.success) {
                            if (callback && typeof callback === 'function') {
                                callback.call(callbackscope || window);
                            }
                        } else if (pubResult.errorMessage) {
                            infoDialog(pubResult.errorMessage);
                        }
                    });
                });
            } else {
                if (res.errorMessage) {
                    infoDialog(res.errorMessage);
                }
            }
        }, null, null, true);
    }
</script>