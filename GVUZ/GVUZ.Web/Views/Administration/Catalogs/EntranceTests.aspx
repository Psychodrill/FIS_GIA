<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.Administration.Catalogs.EntranceTestsViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
    Администрирование - Справочники системы
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PageTitle" runat="server">
	Вступительные испытания (Вуз)
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="divstatement">
		
		<div id="dialog">
		</div>

		<div id="content">		
			<div class="navigation">
				<span class="back">
					<%=Url.GenerateLink<AdministrationController>(c=>c.CatalogsList(),"Назад") %>
				</span>
			</div>

			<p class="left10 navigation" style="margin-bottom:0px;"><a href="#" id="btnAddNewTop" class="add">Добавить испытания</a></p>
			
			<table class="gvuzDataGrid">
				<thead>
					<tr>
						<th>
							<%= Html.LabelFor(x => x.TestDecsr.Directions) %>							
						</th>
						<th>
							<%= Html.LabelFor(x => x.TestDecsr.Subjects) %>
						</th>		
						<th id="thAction" style="width:1%;">
							<%= Html.LabelFor(x => x.TestDecsr.ID)%>
						</th>										
					</tr>					
				</thead>
				<tbody>
					<tr id="firstRow" style="display:none">									
					</tr>
				</tbody>
				<tfoot>
					<tr>
						<td colspan='2' class="navigation">
							<a href="#" id="btnAddNewBottom" class="add">Добавить испытания</a>
						</td>
					</tr>
				</tfoot>
			</table>

			<div class="navigation">
				<span class="back">
					<%=Url.GenerateLink<AdministrationController>(c=>c.CatalogsList(),"Назад") %>
				</span>
			</div>
		</div>
	</div>

	<script language="javascript" type="text/javascript">

		var gridItems = JSON.parse('<%= Html.Serialize(Model.Tests) %>')
		var currentSorting = null

		function addItem($trBefore, item) {

			var buttons = '<a href="#" class="btnEdit" title="Редактировать"></a> '
						+ '<a href="#" class="btnDelete" title="Удалить"' + '></a>'

			var className = $trBefore.prev().attr('class')

			if (className == 'trline2') className = 'trline1'; else className = 'trline2';

			$trBefore.before('<tr itemID="' + item.ID + '" class="' + className + '">'
								+ '<td align="left"><span class="btnEdit linkSumulator" title="Редактировать">' + getStringFromArray(item.Directions) + '</span></td>'
								+ '<td align="left">' + getStringFromArray(item.Subjects) + '</td>'								
								+ '<td align="left">' + buttons + '</td></tr>'
							)
		}

		function getStringFromArray(arr) {
			var result = ''
			for (key in arr) {
				result += arr[key] + '; '
			}			
			return result
		}		

		function fillGrid() {
			jQuery('.gvuzDataGrid tbody tr:not(#firstRow)').remove().detach()

			jQuery.each(gridItems, function (index, object) {
				addItem(jQuery('#firstRow'), object)
			})
		}

		function doSort(el, sortID) {
			var isSortedUp = jQuery(el).hasClass('sortedUp')
			jQuery('.sortUp,.sortDown').remove().detach()
			if (isSortedUp)
				jQuery(el).after('<span class="sortDown"></span>')
			else
				jQuery(el).after('<span class="sortUp"></span>')
			jQuery(el).removeClass('sortedUp')
			if (isSortedUp)
				sortID = -sortID;
			else
				jQuery(el).addClass('sortedUp')
			currentSorting = sortID
			updateData()
		}

		var pageNumber = 0

		function movePager(pageID) {
			pageNumber = pageID
			updateData()
		}

		function updateData() {

			var model =
			{
				SortID: currentSorting,
				PageNumber: pageNumber
			}

			doPostAjax('<%= Url.Generate<AdministrationController>(x => x.GetEntranceTests(null)) %>', JSON.stringify(model), function (data) {
				if (!addValidationErrorsFromServerResponse(data, false)) {
					gridItems = data.Data.Tests
					fillGrid()
					fillPager(data.Data.TotalPageCount, pageNumber)
				}
			})
		}

		var doAddEdit = function (navUrl, postData, callback) {
			createdItem = null
			doPostAjax(navUrl, postData, function (data) {
				jQuery('#dialog').html(data);
				jQuery('#dialog').dialog({
					modal: true,
					width: 500,
					title: (postData == '' ? "Добавление" : "Редактирование") + " втупительных испытаний",
					buttons:
							{
								"Сохранить": function () { jQuery('#btnSubmit').click(); },
								"Отмена": function () { jQuery('#btnCancel').click(); }
							},
					close: function () {
						if (createdItem) callback()
					}
				}).dialog('open');
			}, "application/x-www-form-urlencoded", "html")
		}

		jQuery(document).ready(function () {			
			updateData()
		})

		jQuery('#btnAddNewTop,#btnAddNewBottom').click(function () {
			doAddEdit('<%= Url.Generate<AdministrationController>(x => x.AddTest()) %>', '',
				function () { addItem(jQuery('#firstRow'), createdItem) })
		})

		jQuery('.btnEdit').live('click', function () {
			var $tr = jQuery(this).parents('tr:first')
			var itemID = $tr.attr('itemID')
			doAddEdit('<%= Url.Generate<AdministrationController>(x => x.EditTest(null)) %>', 'id=' + itemID,
				function () { addItem($tr, createdItem); $tr.remove().detach() })
		})

		jQuery('.btnDelete').live('click', function () {
			var $tr = jQuery(this).parents('tr:first');
			var itemID = $tr.attr('itemID')
			confirmDialog('Вы действительно хотите удалить испытания?', function () {
									doPostAjax('<%= Url.Generate<AdministrationController>(x => x.DeleteTest(null)) %>', 'id=' + itemID, function (data) {
										if (data.IsError) jQuery('<div>' + data.Message + '</div>').dialog(
										{ buttons: { OK: function () { jQuery(this).dialog("close"); } } })
										else
											$tr.remove().detach()
									}, "application/x-www-form-urlencoded")
			});

		})

	</script>

</asp:Content>
