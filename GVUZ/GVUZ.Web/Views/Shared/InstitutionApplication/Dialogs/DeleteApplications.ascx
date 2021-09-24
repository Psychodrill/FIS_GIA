<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    function deleteApplications(recordId, callback, callbackScope) {
        confirmDialog('Вы действительно хотите удалить это заявление?', function () {
            var model = { applicationId: $.isArray(recordId) ? recordId : [recordId] };
            doPostAjax('<%= Url.Action("DeleteApplications", "InstitutionApplication") %>',
            JSON.stringify(model), function (data) {
                if (data.success && callback && typeof callback === 'function') {
                    callback.call(callbackScope || window);
                }
            }, null, null, true);
        });
}
</script>
