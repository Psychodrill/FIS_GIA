<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.Portlets.Institutions.EntranceTestTreeViewModel>" %>
<%@ Import Namespace="GVUZ.Web.Portlets" %>

<div style="margin-top: 20px">
    <div id="gvuzTree"></div>
</div>

<div id="treeControlDialog"></div>

<script type="text/javascript">

    function doEditItem(navUrl, itemID) {
        doPostAjax(navUrl, 'structureItemID=' + itemID, function(data) {
            jQuery('#treeControlDialog').html(data);
            jQuery('#treeControlDialog').dialog({
                modal: true,
                width: 600,
                title: jQuery('#dialogCaption').html(),
                close: function() {
                }
            }).dialog('open');
        },
            "application/x-www-form-urlencoded", "html");
    }


    jQuery(document).ready(function() {
        var tree = new TreeView(jQuery('#gvuzTree'),
            '<%= PortletLinkHelper.InstitutionTreeEntranceTestAjaxLink() %>',
            null,
            function editCallback(itemID, callback) { doEditItem("<%= PortletLinkHelper.InstitutionTreeEntranceTestViewAjaxLink() %>", itemID); },
            null,
            '',
            '',
            '',
            null,
            {
                ExpandAll: true,
                ExpandUrl: '<%= PortletLinkHelper.InstitutionFullTreeEntranceTestAjaxLink() %>',
                LoadExpandedRoot: true,
                CollapseAll: true
            });
        tree.init();
    });
</script>