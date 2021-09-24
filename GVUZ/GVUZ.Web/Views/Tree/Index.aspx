<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Register TagPrefix="gv" TagName="TabControl" Src="~/Views/Shared/TabControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <link rel="stylesheet" type="text/css" href="<%= Url.Resource("Resources/ScrollableTree.css") %>" />
	
    <gv:TabControl runat="server" ID="tabControl" />
    <h2>Образовательные учреждения: иерархическая таблица</h2>

    <h3>Скроллируемое дерево по горизонтали</h3>
    <div id="scrTableTree">
    </div>

    <hr />
    <div id="tableTree"></div>
    <!--hr />
        <div id="treeView"></div-->

    <script type="text/javascript">

        jQuery(document).ready(function() {
            doPostAjax('<%= Url.Generate<TreeController>(c => c.Test()) %>', null,
                function(servData) {
                    data = servData;
                    var tableTree = new TableTreeView(jQuery('#tableTree'), data['objects'], data['children'],
                        ['Образовательные учреждения', 'Кол-во мест', 'Конкурсы', 'Олимпиады', 'Кол-во заявлений',
                            'Подготов. курсы', 'Воен. кафедра', ' '],
                        function nodeClickCallback(itemID) {
                            alert(itemID);
                        },
                        {
                            CollapseAll: true,
                            ExpandAll: true,
                            ColumnRenderer:
                            {
                                '6': function(dataItem) {
                                    dataItem.Applicable = true;
                                    dataItem.CanBeChecked = true;
                                    var result;
                                    // Applicable,	CanBeChecked,	Accepted,	ApplicationNumber
                                    if (dataItem.Accepted)
                                        return sprintf('Принято <a href="%1$s">Перейти к заявлению</a>', '#' + dataItem.ApplicationNumber);
                                    if (dataItem.Applicable) {
                                        if (dataItem.CanBeChecked) return "<input type='checkbox' />";
                                        return "<a href=''>Подать заявление<a/>";
                                    }
                                    return result;
                                }
                            }
                        });
                    tableTree.init();
                });

            /*var data =
			{ "Objects":
			{
			"1": { ItemID: 1, "Name": "МГУ", IsLeaf: true, a: 10, b: 20, c: 30, d: true },
			"2": { ItemID: 2, "Name": "Государственный университет управления", a: 15, b: 25, c: 35, d: true },
			"3": { ItemID: 3, "Name": "Институт инноватики и логистики", a: 1, b: 4, c: null, d: true, IsLeaf: true }
			},
			"Children":
			{
			"0": [1, 2],
			"2": [3]
			}
			}*/

            /*var treeView = new ClickableTreeView($('#treeView'),
			'<%= Url.Generate<TreeController>(c => c.TreeStructure(null)) %>',
			function (itemID)
			{
			alert(data.objects[itemID]);
			alert(data.objects[itemID].NodeUrl);
			},
			{
			CollapseAll : true, ExpandAll : true
			});
			treeView.init();*/

            doPostAjax('<%= Url.Generate<TreeController>(c => c.Test()) %>', null,
                function(servData) {
                    data = servData;
                    var tableTree = new TableTreeView(jQuery('#scrTableTree'), data['objects'], data['children'],
                        ['Образовательные учреждения', 'Кол-во мест', 'Конкурсы', 'Олимпиады', 'Кол-во заявлений',
                            'Подготов. курсы', 'Воен. кафедра', ' '],
                        function nodeClickCallback(itemID) {
                            alert(itemID);
                        });
                    tableTree.init();
                });
        });

    </script>
</asp:Content>