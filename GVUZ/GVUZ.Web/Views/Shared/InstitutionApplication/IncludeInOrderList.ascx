<%@ Control Language="C#"  Inherits="System.Web.Mvc.ViewUserControl<IncludeInOrderListViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.ViewModels" %>

<style>
	.orderSelected {
		background-color: #ffffe0;
	}
</style>

<div id="orderFilter">
		<%= Html.TableLabelFor(x => x.SelectedCampaignID)%>
		<%= Html.DropDownListExFor(x => x.SelectedCampaignID, Model.Campaigns, new { onchange = "doChangeOrderFilter()" })%>
		&nbsp;&nbsp;&nbsp;
		<%= Html.TableLabelFor(x => x.SelectedCourse)%>
		<%= Html.DropDownListExFor(x => x.SelectedCourse, Model.Courses, new { onchange = "doChangeOrderFilter()" })%>
		&nbsp;&nbsp;&nbsp;
		<%= Html.TableLabelFor(x => x.SelectedOrderType)%>
		<%= Html.DropDownListExFor(x => x.SelectedOrderType, Model.OrderTypes, new { onchange = "doChangeOrderFilter()" })%>
</div>
<div class="divOrderTable">
<table class="gvuzDataGrid tableStatement2" cellpadding="3" id="orderGrid">
	<thead>
		<tr>
			<th>
				<span><%= Html.LabelFor(x => x.OrderDescr.CampaignName) %></span>
			</th>
			<th>
				<span><%= Html.LabelFor(x => x.OrderDescr.Course) %></span>
			</th>
			<th>
				<span><%= Html.LabelFor(x => x.OrderDescr.OrderTypeName) %></span>
			</th>
			<th>
				<span><%= Html.LabelFor(x => x.OrderDescr.OrderDate) %></span>
			</th>
			<th>
				<span><%= Html.LabelFor(x => x.OrderDescr.ApplicationCount) %></span>
			</th>
			<th>
				<span><%= Html.LabelFor(x => x.OrderDescr.StatusName) %></span>
			</th>
			<th id="thActionOrder" width="40px">
				
			</th>
		</tr>
	</thead>
	<tbody>
		<tr id="trAddNewOrder" style="display:none">
		</tr>
	</tbody>
</table>
</div>
<script type="text/javascript">	
	var orderGridItems = null;
	
	function addOrderItem($trBefore, item)
	{

		var buttons = '<a href="#" class="btnMove" onClick="doSelectOrder(this);return false;">'+ '&nbsp;' +'</a> '

		var className = $trBefore.prev().attr('class')
		if(className == 'trline2') className = 'trline1'; else className = 'trline2';
		if(item.OrderID > 0 && item.OrderID == selectedOrder)
			className += ' orderSelected';
		$trBefore.before('<tr itemID="' + item.OrderID + '" class="' + className 
							+ '" orderUID="' + escapeHtml(item.UID) + '" '
							+ ' + statusID="' + (item.StatusID) + '">'
						+ '<td>' + escapeHtml(item.CampaignName) + '</td>'
						+ '<td>' + escapeHtml(item.Course) + '</td>'
						+ '<td>' + escapeHtml(item.OrderTypeName) + '</td>'
						+ '<td>' + escapeHtml(item.OrderDate) + '</td>'
						+ '<td>' + (item.ApplicationCount) + '</td>'
						+ '<td>' + escapeHtml(item.StatusName) + '</td>'
						+ '<td>' + buttons + '</td>'
						+ '</tr>')
	}

	function doSelectOrder(el) {
	    if (!el) {
	        return;
	    }
		var $tr = jQuery(el).parents('tr:first');
		jQuery("#orderGrid tr").removeClass('orderSelected')
		$tr.addClass('orderSelected')
		selectedOrder = $tr.attr('itemID')
		jQuery('#tableAppList').show();
		getApplicationList(true)
	}


	function fillOrderGrid()
	{
		jQuery('#orderGrid tbody tr:not(#trAddNewOrder)').remove().detach()
		jQuery.each(orderGridItems, function (idx, n) { addOrderItem(jQuery('#trAddNewOrder'), n) })
	}

	function doChangeOrderFilter() {
		updateOrderData()
	}

	function prepareOrderModel()
	{
		var model =
			{
				SelectedCampaignID: jQuery("#SelectedCampaignID").val(),
				SelectedCourse: jQuery("#SelectedCourse").val(),
				SelectedOrderType: jQuery("#SelectedOrderType").val()
			}
		return model;
	}


	function updateOrderData()
	{
		clearValidationErrors(jQuery('#content'))

		doPostAjax('<%= Url.Generate<InstitutionApplicationController>(x => x.GetOrderList(null)) %>', JSON.stringify(prepareOrderModel()), function (data)
		{
			if (!addValidationErrorsFromServerResponse(data, false))
			{
				orderGridItems = data.Data;
				fillOrderGrid()
				onOrdersLoaded();
			}
		})
	}

	jQuery(document).ready(function ()
	{
		updateOrderData()
	})


</script>