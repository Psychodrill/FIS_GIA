<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
    Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.Administration.Catalogs.CampaignOrderDateCatalogViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Администрирование - Справочники системы
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

            <table class="gvuzDataGrid">
                <thead>
                    <tr>
                        <th>
                            <span class="linkSumulator"><%= Html.LabelFor(x => x.DataDescr.YearStart) %></span>
                        </th>
                        <th>
                            <span class="linkSumulator"><%= Html.LabelFor(x => x.DataDescr.StartDate) %></span>
                        </th>
                        <th>
                            <span class="linkSumulator"><%= Html.LabelFor(x => x.DataDescr.EndDate) %></span>
                        </th>
                        <th>
                            <span class="linkSumulator"><%= Html.LabelFor(x => x.DataDescr.TargetOrderDate) %></span>
                        </th>
                        <th>
                            <span class="linkSumulator"><%= Html.LabelFor(x => x.DataDescr.Stage1OrderDate) %></span>
                        </th>
                        <th>
                            <span class="linkSumulator"><%= Html.LabelFor(x => x.DataDescr.Stage2OrderDate) %></span>
                        </th>
                        <th>
                            <span class="linkSumulator"><%= Html.LabelFor(x => x.DataDescr.PaidOrderDate) %></span>
                        </th>
                        <th>
                            <span class="linkSumulator"><%= Html.LabelFor(x => x.DataDescr.PreviousUseDepth) %></span>
                        </th>
						<th id="thAction" style="width:1%; white-space:nowrap;">
						</th>												
                    </tr>
                </thead>
				<tbody>
                    <tr id="firstRow" class="trline2">
                        <td colspan="9">
            			    <a href="#" id="btnAddNew" class="add">Добавить даты для следующей ПК</a>
            			    <a href="#" id="btnAdd" class="add">Добавить даты для ПК</a>
                        </td>
                    </tr>
				</tbody>
            </table>

			<div class="navigation">
				<span class="back">
					<%=Url.GenerateLink<AdministrationController>(c=>c.CatalogsList(),"Назад") %>
				</span>
			</div>
		</div>
	</div>

    <script language="javascript" type="text/javascript">
        var gridItems = JSON.parse('<%= Html.Serialize(Model.CampaignOrderDates) %>');
        var $hiddenEditedRow;

        function addGridItem($trBefore, item) {
            var buttons = '<a href="#" class="btnEdit" onclick="createEditRow(this)" title="Редактировать"></a> '
						+ '<a href="#" class="btnDelete" onclick="deleteDateSet(this)" title="Удалить"></a>'

            var className = $trBefore.prev().attr('class')

            if (className == 'trline2') className = 'trline1'; else className = 'trline2';

            $trBefore.before('<tr itemID="' + item.YearStart + '" class="' + className + '">'
   								+ '<td align="left">' + item.YearStart + '</td>'
   								+ '<td align="left">' + item.StartDate + '</td>'
   								+ '<td align="left">' + item.EndDate + '</td>'
   								+ '<td align="left">' + item.TargetOrderDate + '</td>'
   								+ '<td align="left">' + item.Stage1OrderDate + '</td>'
   								+ '<td align="left">' + item.Stage2OrderDate + '</td>'
   								+ '<td align="left">' + item.PaidOrderDate + '</td>'
   								+ '<td align="left">' + item.PreviousUseDepth + '</td>'
                                + '<td align="left" style="width:1%; white-space:nowrap;">' + buttons + '</td></tr>')
        }

		function fillGrid() {
			jQuery('.gvuzDataGrid tbody tr:not(#firstRow)').remove().detach()

			jQuery.each(gridItems, function (index, object) {
				addGridItem(jQuery('#firstRow'), object)
			})
		}

		var createEditRow = function ($trToEdit) {
		    var $tr = jQuery($trToEdit).parents('tr:first');
		    var child = $tr.children('td')
		    var className = $tr.prev().attr('class')
		    if (className == 'trline1') className = 'trline1'; else className = 'trline2';

		    var buttons = '<a href="#" class="btnSave" onclick="saveData(this)" title="Сохранить"></a>'
                            + '<a href="#" class="btnDeleteU" onclick="cancelEdit(this)" title="Отмена"></a>'

		    $tr.before('<tr itemID="' + child[0].innerHTML + '" class="' + className + '">'
                                + '<td align="left">' + child[0].innerHTML + '</td>'
                                + '<td align="left"><input type="text" id="startDatePicker" class="datePicker" value="' + child[1].innerHTML + '" /></td>'
   								+ '<td align="left"><input type="text" id="endDatePicker" class="datePicker" value="' + child[2].innerHTML + '" /></td>'
   								+ '<td align="left"><input type="text" id="order0DatePicker" class="datePicker" value="' + child[3].innerHTML + '" /></td>'
   								+ '<td align="left"><input type="text" id="order1DatePicker" class="datePicker" value="' + child[4].innerHTML + '" /></td>'
   								+ '<td align="left"><input type="text" id="order2DatePicker" class="datePicker" value="' + child[5].innerHTML + '" /></td>'
   								+ '<td align="left"><input type="text" id="order3DatePicker" class="datePicker" value="' + child[6].innerHTML + '" /></td>'
   								+ '<td align="left"><input type="text" class="PreviousUseDepth numeric" value="' + child[7].innerHTML + '" /></td>'
                                + '<td style="width: 1%; white-space:nowrap;">' + buttons + '</td></tr>')
		    $hiddenEditedRow = $tr;
		    $tr.hide();
		    jQuery(".datePicker").datepicker({ defaultDate: child[1].innerHTML, changeMonth: true, changeYear: false, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: child[0].innerHTML + ':' + child[0].innerHTML });
		}

		var cancelEdit = function (el) {
		    var $editedTr = jQuery(el).parents('tr:first');

		    if ($hiddenEditedRow != null) {
		        $hiddenEditedRow.show();
		        $hiddenEditedRow = null;
		    }
		    $editedTr.remove();
		}

		function saveData(el) {
        var $editedRow = jQuery(el).parents('tr:first');
        var child = $editedRow.children('td')
		    var model =
            {
                YearStart: jQuery('#yearStart').val() == null ? child[0].innerHTML : jQuery('#yearStart').val(),
                StartDate: jQuery('#startDatePicker').val(),
                EndDate: jQuery('#endDatePicker').val(),
                TargetOrderDate : jQuery('#order0DatePicker').val(),
                Stage1OrderDate : jQuery('#order1DatePicker').val(),
                Stage2OrderDate : jQuery('#order2DatePicker').val(),
                PaidOrderDate : jQuery('#order3DatePicker').val(),
                PreviousUseDepth : jQuery('.PreviousUseDepth').val()
            }

            doPostAjax('<%= Url.Generate<AdministrationController>(x => x.EditCampaignOrderDateCatalogItem(null)) %>',
                            JSON.stringify(model),
                            function (data, status) {
                                if (!data.IsError) {
                                    addGridItem($editedRow, data.Data);
                                    $editedRow.remove().detach();
                                    if ($hiddenEditedRow != null) $hiddenEditedRow.remove().detach();
                                }
                                else {
                                    jQuery($editedRow).children().children('.datePicker').removeClass('input-validation-error');
                                    jQuery($editedRow).children().children('div').remove().detach();
                                    for (var i = 0; i < data.Data.length; i++) {
                                        jQuery('#' + data.Data[i].ControlID).addClass('input-validation-error');
                                        if (jQuery('#' + data.Data[i].ControlID).parents('td:first').children().last().html() != data.Data[i].ErrorMessage)
                                            jQuery('#' + data.Data[i].ControlID).parents('td:first').children().last().after('<div class="field-validation-error">' + data.Data[i].ErrorMessage + '</div>');
                                    }
                                }
                            }
            );
		}

		var addNewDateSet = function (next) {
		    if (next)
		        doPostAjax('<%= Url.Generate<AdministrationController>(x => x.GetDataForNewDateSet(null)) %>',
                        null,
                        function (data, status) {
                            var $row = jQuery('#firstRow');
                            var className = $row.prev().attr('class')
                            if (className == 'trline1') className = 'trline1'; else className = 'trline2';

                            var buttons = '<a href="#" class="btnSave" onclick="saveData(this)" title="Сохранить"></a>'
                            + '<a href="#" class="btnDeleteU" onclick="cancelEdit(this)" title="Отмена"></a>'
                            $row.before('<tr itemID="' + data.Data.YearStart + '" class="' + className + '">'
                                + '<td align="left">' + data.Data.YearStart + '</td>'
                                + '<td align="left"><input type="text" id="startDatePicker" class="datePicker" value="' + data.Data.StartDate + '" /></td>'
   								+ '<td align="left"><input type="text" id="endDatePicker" class="datePicker" value="' + data.Data.EndDate + '" /></td>'
   								+ '<td align="left"><input type="text" id="order0DatePicker" class="datePicker" value="' + data.Data.TargetOrderDate + '" /></td>'
   								+ '<td align="left"><input type="text" id="order1DatePicker" class="datePicker" value="' + data.Data.Stage1OrderDate + '" /></td>'
   								+ '<td align="left"><input type="text" id="order2DatePicker" class="datePicker" value="' + data.Data.Stage2OrderDate + '" /></td>'
   								+ '<td align="left"><input type="text" id="order3DatePicker" class="datePicker" value="' + data.Data.PaidOrderDate + '" /></td>'
   								+ '<td align="left"><input type="text" class="PreviousUseDepth numeric" value="' + data.Data.PreviousUseDepth + '" /></td>'
                                + '<td style="width: 1%; white-space:nowrap;">' + buttons + '</td></tr>')
                            jQuery(".datePicker").datepicker({ defaultDate: data.Data.YearStart, changeMonth: true, changeYear: false, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: data.Data.YearStart + ':' + data.Data.YearStart });
                        }
            );
		    else {
		        var $row = jQuery('#firstRow');
		        var className = $row.prev().attr('class')
		        if (className == 'trline1') className = 'trline1'; else className = 'trline2';

		        var buttons = '<a href="#" class="btnSave" onclick="saveData(this)" title="Сохранить"></a>'
                            + '<a href="#" class="btnDeleteU" onclick="cancelEdit(this)" title="Отмена"></a>'

		        $row.before('<tr class="' + className + '">'
                                + '<td align="left"><input type="text" id="yearStart"/></td>'
                                + '<td align="left"><input type="text" id="startDatePicker" class="datePicker" value="" /></td>'
   								+ '<td align="left"><input type="text" id="endDatePicker" class="datePicker" value="" /></td>'
   								+ '<td align="left"><input type="text" id="order0DatePicker" class="datePicker" value="" /></td>'
   								+ '<td align="left"><input type="text" id="order1DatePicker" class="datePicker" value="" /></td>'
   								+ '<td align="left"><input type="text" id="order2DatePicker" class="datePicker" value="" /></td>'
   								+ '<td align="left"><input type="text" id="order3DatePicker" class="datePicker" value="" /></td>'
   								+ '<td align="left"><input type="text" class="PreviousUseDepth numeric" value="0" /></td>'
                                + '<td style="width: 1%; white-space:nowrap;">' + buttons + '</td></tr>')
		        jQuery(".datePicker").datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true });
		    }
		}

        var deleteDateSet = function (el) {
            var $row = jQuery(el).parents('tr:first');
            var yearStart = jQuery($row).attr('itemID');

            confirmDialog('Вы действительно хотите удалить данные для ' + yearStart + ' года?', function () {
                doPostAjax('<%= Url.Generate<AdministrationController>(x => x.DeleteCampaignOrderDateCatalogItem(null)) %>',
                'id=' + yearStart, function (data, status) {
                    $row.remove().detach();
                }, "application/x-www-form-urlencoded");
            })
        }

        jQuery(document).ready(function () {
            fillGrid();
            jQuery('#btnAddNew').click(function () { addNewDateSet(true); return false; });
            jQuery('#btnAdd').click(function () { addNewDateSet(false); return false; });
        })
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageTitle" runat="server">
    Даты для создания приёмных кампаний
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="PageSubtitle" runat="server">
</asp:Content>
