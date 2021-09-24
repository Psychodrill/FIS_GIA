<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">

    function cancelRevokeApplications(recordId, callback, callbackScope) {
        
        var model = { applicationId: $.isArray(recordId) ? recordId : [recordId] };

        doPostAjax('<%= Url.Action("CancelRevokeApplications", "InstitutionApplication") %>', JSON.stringify(model), function (data) {

            if (data.success && callback && typeof callback === 'function') {
                callback.call(callbackScope || window);
            }

        }, null, null, true);
    }    

</script>