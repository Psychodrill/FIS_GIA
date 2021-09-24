<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.Portlets.Institutions.InstitutionStructureViewModel>" %>
<%@ Import Namespace="GVUZ.Web.Portlets" %>

<div style="margin-top: 20px">
    <div id="gvuzTree"></div>
</div>

<script type="text/javascript">
    jQuery(document).ready(function() {
                <%--
		doPostAjax('<%= PortletService.AjaxLink("test")%>', 'structureItemID=' + "1", 
		function(data) { alert(data); }, "application/x-www-form-urlencoded", "html");
--%>

        var tree = new ClickableTreeView(
            jQuery('#gvuzTree'), '<%= PortletLinkHelper.InstitutionTreeStructureAjaxLink() %>', null,
            {
                ExpandAll: true,
                ExpandUrl: '<%= PortletLinkHelper.InstitutionFullTreeStructureAjaxLink() %>',
                CollapseAll: true
            });
        tree.init();
    });
</script>