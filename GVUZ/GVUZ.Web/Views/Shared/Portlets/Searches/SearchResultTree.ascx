<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.Portlets.Searches.SearchResultTreeViewModel>" %>
<%@ Import Namespace="System.Web.Script.Serialization" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Model.Entrants" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Portlets" %>

<link rel="stylesheet" type="text/css" href="<%= Url.Resource("Resources/ScrollableTree.css") %>" />
<script type="text/javascript">

    function setPage(pageN) {
        var form = document.getElementById('Pager');
        var elems = form.getElementsByTagName('input');
        for (var i = 0; i < elems.length; i++)
            if (elems[i].id == 'PageNumber')
                elems[i].value = pageN;
    }

    jQuery(function() {
        var data =
        {
            "Objects": <%= new JavaScriptSerializer().Serialize(Model.TreeResult.Objects) %>,
            "Children": <%= new JavaScriptSerializer().Serialize(Model.TreeResult.Children) %>
        };

        var tableTree = new TableTreeView(jQuery('#tableTree'), data['Objects'], data['Children'],
            ['Образовательные учреждения', 'Кол-во мест', 'Конкурс', 'Олимпиады', 'Кол-во заявлений',
                'Подг. курсы', 'Военная кафедра', 'Заявление'],
            function(itemID) {
                navigateTo(data.Objects[itemID].NodeUrl);
            },
            {
                ExpandAll: true,
                CollapseAll: true,
                ColumnRenderer:
                {
                    '6': function(dataItem, extraDataItem) {
                        /*dataItem.ApplicationStatus = 'Черновик'
										dataItem.ApplicationStatusID = 1;
										dataItem.ApplicationID = 80;*/
                        /*dataItem.ApplicationStatus = 'Подано'
										dataItem.ApplicationID = 100;*/
                        // Applicable,	CanBeChecked,	ApplicationStatus,	ApplicationID
                        if (!isStringNullOrEmpty(dataItem.ApplicationStatus))
                            return sprintf('%1$s <a appID="%2$s" %3$s>Перейти к заявлению</a>', dataItem.ApplicationStatus, dataItem.ApplicationID,
                                (dataItem.ApplicationStatusID == <%= ApplicationStatusType.Draft %>) ? 'class="draftAppLink"' : '');
                        if (dataItem.Applicable) {
                            //if (dataItem.CanBeChecked) return "<input type='checkbox' />";
                            return '<a class="addAppLink">Подать заявление</a>';
                        }
                        return "";
                    }
                }
            });
        tableTree.init();

        jQuery('#tableTree a[appID]').live('click',
            function viewApplication() {
                var appID = jQuery(this).attr('appID');
                var isDraft = jQuery(this).hasClass('draftAppLink');
                doPostAjax('<%= Url.Generate<ApplicationController>(c => c.SaveSelectedApplicationID(null)) %>', 'viewAppID=' + appID,
                    function(serverData, status) {
                        if (isDraft)
                            navigateTo('<%= Url.Generate<ApplicationController>(c => c.ApplicationPersonalData(null)) %>');
                        else
                            navigateTo('<%= Url.Generate<ApplicationController>(c => c.ApplicationViewTab0App(0)) %>');
                    }, "application/x-www-form-urlencoded");
            });

        jQuery('#tableTree a.addAppLink').live('click', tableTree,
            function addApplication(eventData) {
                var treeModel = eventData.data;
                var itemID = treeModel.getStructItemID(this);
                doPostAjax('<%= Url.Generate<ApplicationController>(c => c.SaveStructureItemID(null)) %>', 'structureItemID=' + itemID,
                    function(data, status) {
                        navigateTo('<%= Url.Generate<ApplicationController>(c => c.AddApplication()) %>');
                    }, "application/x-www-form-urlencoded");
            });

    });
</script>
<div style="margin-top: 20px;">
    <div id="tableTree">
    </div>
</div>
<div class="pager">
    <%= Html.SimplePager(Model.CurrentPage, Model.ResultCount,
                         "/contacts/page/{0}",
                         "pager",
                         PortletLinkHelper.SearchPage())
    %>
</div>