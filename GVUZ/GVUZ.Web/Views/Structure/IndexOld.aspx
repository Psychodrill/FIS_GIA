<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>  
<%@ Register TagPrefix="gv" TagName="TabControl" Src="~/Views/Shared/Common/InstitutionsTabControl.ascx" %>

<asp:Content ID="registerTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Структура образовательной организации
</asp:Content>
<asp:Content ID="header" ContentPlaceHolderID="PageTitle" runat="server">Сведения об образовательной организации</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="divstatement">
        <gv:TabControl runat="server" ID="tabControl" />
        <div id="divContent" style="background-color: white;">
            <table width="99%" cellpadding="1" id="table4Controls">
                <tbody id="TFCBody">
                </tbody>
            </table>
        </div>
        <!-- div id="treeControl"> </div -->
    </div>
    <script type="text/javascript">
        menuItems[1].selected = true;
        var createdItem = null;
        var UserReadonly = <%= UrlUtils.IsReadOnly(FBDUserSubroles.InstitutionDataDirection) ? "true" : "false" %>;
        var filials;
        var currentIID = <%= InstitutionHelper.GetInstitutionID(false) %>;
        var mainIID = <%= InstitutionHelper.MainInstitutionID %>;

        function createAddEditItem(navUrl, itemID, callback) {
            createdItem = null;
            doPostAjax(navUrl, 'structureItemID=' + itemID, function(data) {
                jQuery('#treeControlDialog').html(data);
                jQuery('#treeControlDialog').dialog({
                    modal: true,
                    width: 800,
                    title: jQuery('#dialogCaption').html(),
                    buttons:
                    {
                        "Сохранить": function() { jQuery('#btnSubmit').click(); },
                        "Отмена": function() { jQuery('#btnCancel').click(); }
                    },
                    close: function() {
                        if (createdItem != null)
                            callback(createdItem);
                    }
                }).dialog('open');
            }, "application/x-www-form-urlencoded", "html");
        }

        function deleteItem(navUrl, itemID, callback) {
            confirmDialog('Вы действительно хотите удалить?', function() {
                doPostAjax(navUrl, 'structureItemID=' + itemID, function(data) {
                    // если удалить нельзя, то сообщаем об этом
                    if (data.IsError)
                        jQuery('<div>' + data.Message + '</div>').dialog(
                            {
                                buttons: { OK: function() { jQuery(this).dialog("close"); } }
                            });
                    else
                        callback();
                }, "application/x-www-form-urlencoded");
            });
        }


        function loadTree(divId, institutionID) {
            var tree = new TreeView(jQuery('#' + divId),
                "<%= Url.Generate<StructureController>(c => c.TreeStructure(null, null)) %>?institutionId=" + institutionID,
                function addCallback(itemID, callback) { if (!UserReadonly) createAddEditItem("<%= Url.Generate<StructureController>(c => c.AddItem(null)) %>", itemID, callback); },
                function editCallback(itemID, callback) { if (!UserReadonly) createAddEditItem("<%= Url.Generate<StructureController>(c => c.EditItem(null)) %>", itemID, callback); },
                function deleteCallback(itemID, callback) { if (!UserReadonly) deleteItem("<%= Url.Generate<StructureController>(c => c.DeleteItem(null)) %>", itemID, callback); },
                '<%= Url.Images("plus.jpg") %>',
                '<%= Url.Images("no2.jpg") %>', null, null,
                {
                    ExpandAll: false,
                    ExpandUrl: '<%= Url.Generate<StructureController>(c => c.TreeStructure(-1, null)) %>?institutionId=' + institutionID,
                    LoadExpandedRoot: true,
                    CollapseAll: false
                });
            tree.init();
        }


        function onRadioClick(element) {
            var ibrs = $(":radio");
            for (var i = 0; i < ibrs.length; i++) {
                if ((ibrs[i] != element) && (element.checked)) {
                    ibrs[i].checked = false;
                }

                if (element.checked)
                    ResetInstitution(element.value);
            }
        }


        function ResetInstitution(IdValue) {
            if (IdValue == currentIID)
                return;

            currentIID = IdValue;

            $.ajax({ async: false, type: 'POST', url: '<%= Url.Generate<StructureController>(x => x.SetInstitution(null)) %>?institutionId=' + IdValue });
            document.location.reload();

        }


        jQuery(document).ready(function() {
            var strRows = '';
            var checkedId;

            $.ajax({
                async: false,
                type: 'POST',
                url: '<%= Url.Generate<StructureController>(x => x.GetFilials()) %>',
                success: function(data) {
                    if (data.IsError == true)
                        alert(data.Message);
                    if ((data.IsError == false) && (data.Data.length != 0))
                        filials = data.Data;
                }
            });

            //alert (filials)

            if (filials != null) {
                if (mainIID == currentIID)
                    checkedId = "rbMain";

                strRows = '<tr ><td><input type="radio" id="rbMain" onclick="onRadioClick(this)" value="' + mainIID + '"/></td><td><div id="treeControl"></div></td></tr>';
                for (var j = 0; j < filials.length; j++) {

                    if (filials[j] == currentIID)
                        checkedId = "rbFilial" + j;

                    strRows += '<tr><td><input type="radio" id="rbFilial' + j + '" onclick="onRadioClick(this)" value="' + filials[j] + '" /></td><td><div id="treeFilial' + j + '"></div></td></tr>';
                }
            } else
                strRows = '<tr ><td><div id="treeControl"></div></td></tr>'; //alert (strRows)   

            $('#TFCBody').html(strRows);

            if (filials != null)
                document.getElementById("" + checkedId).checked = true;


            // загрузка структуры головного ОО
            loadTree('treeControl', <%= InstitutionHelper.MainInstitutionID %>); // загрузка структуры филиалов (если они есть)
            if (filials != null) {
                for (j = 0; j < filials.length; j++) {
                    loadTree('treeFilial' + j, filials[j]);
                }
            }
        });
    </script>
</asp:Content>