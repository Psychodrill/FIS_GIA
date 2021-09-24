<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
    function editApplicationUrl(applicationId) {
        applicationId = parseInt(applicationId);
        if (isNaN(applicationId)) {
            return '#error';
        }
        var url = '<%= Url.Action("NavigateToEditPage", "InstitutionApplication", 
            new { applicationId = "APPID", hashTag = "HASHTAG" }) %>';
        url = url.replace("APPID", applicationId.toString());

        var rx = /^#tab(\d)$/i;
        var requestedTab = 0;
        if (rx.test(window.location.hash)) {
            requestedTab = parseInt(window.location.hash.match(rx)[1]);
        }

        url = url.replace("HASHTAG", requestedTab.toString());

        return url;
    }
</script>

