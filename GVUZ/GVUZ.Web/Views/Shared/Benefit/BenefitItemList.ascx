<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.BenefitViewModelC>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="System.Web.Script.Serialization" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
	<div id="content">
		<table class="gvuzDataGrid" cellpadding="3" id="tableBenefitList">
			<thead>
				<tr>
					<th>
						<%= Html.LabelFor(x => x.UID) %>
					</th>
					<th>
						<%= Html.LabelFor(x => x.DiplomaType) %>
					</th>
					<th>
						<%= Html.LabelFor(x => x.ComptetitionLevel) %>
					</th>
					<th>
						<%= Html.LabelFor(x => x.OlympicYear) %>
					</th>
					<th>
						<%= Html.LabelFor(x => x.OlympicCaption) %><sup>1</sup>
					</th>
					<th>
						<%= Html.LabelFor(x => x.BenefitType) %>
					</th>
                    <th>
                        <%= Html.LabelFor(x => x.Benefit.MinEgeValue) %>
                    </th>
					<th style="width: 40px">
					</th>
				</tr>
				<tr id="trAddNewBenefit">
					<td colspan="5">
						<a href="#" id="btnAddNewBenefit">
							<img src="<%= Url.Images("plus.jpg") %>" alt="add" />
							Добавить льготу</a>
					</td>
				</tr>
			</thead>
			<tbody>
			</tbody>
		</table>
		<div>
			<sup>1</sup> Перечень олимпиад школьников, утвержденный министерством образования и науки РФ
		</div>
	</div>
	<script type="text/javascript">

		var existingItems = JSON.parse('<%= Html.Serialize(Model.Benefits) %>')
		var createdBenefitItem
        var EntranceTestItemId = ('<%=Model.EntranceTestItemID %>');

		function setBenefitItemCount()
		{
			benefitListReturnedCount = jQuery('#tableBenefitList tr[itemID]').length
		}

		function getOlympicNumberText(item)
		{
			if(item.IsAllOlympic)
				return '<%= Html.LabelFor(x => x.IsAllOlympicCaption) %>'
			var res = ''
			var sep = ''
			jQuery.each(item.AttachedOlympic, function ()
			{
				res += sep
				sep = ', '
				res += '<span class="linkSumulator" onmouseout="hideOlympicDetails()" onmouseover="viewOlympicDetails(this, ' + this.OlympicID + ')">' + this.Number.toString() + '</span>'
			})
			return res
		}

		var addNewBenefit = function ($trBefore, item)
		{
        	var className = $trBefore.prev().attr('class')
			if (className == 'trline1') className = 'trline2'; else className = 'trline1';

            var minEge = '';

            if (EntranceTestItemId == 0)
            {
                minEge = '<td>' + item.AttachedSubjectsAsHtml + '</td>'
            }
            else{
                minEge = '<td>' + (item.MinEgeValue == null ? '' : item.MinEgeValue.toString()) + '</td>';
            }

			$trBefore.before('<tr itemID="' + item.BenefitItemID + '" class="' + className + '">'
					+'<td>' + escapeHtml(item.UID == null ? '' : item.UID) + '</td>'
					+'<td>' + escapeHtml(item.DiplomaType) + '</td>' 
			        +'<td  align="center">' + item.CompetitionLevel + '</td>' 
		            +'<td  align="center">' + item.OlympicYear + '</td>' 
			        +'<td>'+ getOlympicNumberText(item) + '</td><td>'
					+ escapeHtml(item.BenefitType) + '</td>'
                    + minEge
					+ '<td align="center"><a href="#" class="btnEdit" onclick="blEditButton(this)"><img src="<%= Url.Images("edit_16.gif") %>" alt="edit" /></a>&nbsp;<a href="#" class="btnDelete"><img src="<%= Url.Images("delete_16.gif") %>" alt="delete" onclick="blDeleteButton(this)" /></a></td></tr>')
			setBenefitItemCount()
		}

		var doAddEdit = function (navUrl, postData, callback)
		{
			createdBenefitItem = null
			doPostAjax(navUrl, postData, function (data)
			{
				if (data[0] == '{')
				{
					var dataJson = JSON.parse(data);
					if (dataJson.IsError)
					{
						var jq = jQuery('<div>' + dataJson.Message + '</div>')
						jq.dialog(
							{ buttons: { OK: function() { jq.dialog("close"); } } });
						return;
					}
				}
				jQuery('#divAddBenefit').html(data);
				jQuery('#divAddBenefit').dialog({
					modal: true,
					width: 800, //<%-- Дропдаун один сильно длинный --%>
					title: (postData == '' ? "Добавление" : "Редактирование") + " льготы",
					buttons: {
						'Сохранить': function () { jQuery('#btnSubmitAB').click() },
						'Отмена': function () { jQuery('#btnCancelAB').click() }
					},
					close: function ()
					{
						if (createdBenefitItem) callback()
					}
				}).dialog('open');
			}, "application/x-www-form-urlencoded", "html")

		}

		function blEditButton(el)
		{
			var $tr = jQuery(el).parents('tr:first')
			var itemID = $tr.attr('itemID')
			doAddEdit('<%= Url.Generate<BenefitController>(x => x.EditBenefitItem(null)) %>', 'benefitItemID=' + itemID,
				function () { addNewBenefit($tr, createdBenefitItem); $tr.remove().detach(); setBenefitItemCount(); })
			return false
		}

		function blDeleteButton(el)
		{
			var $tr = jQuery(el).parents('tr:first');
			var itemID = $tr.attr('itemID')
			confirmDialog('Вы действительно хотите удалить льготу?', function ()
			{
				doPostAjax('<%= Url.Generate<BenefitController>(x => x.DeleteBenefitItem(null)) %>', 'benefitItemID=' + itemID, function (data)
				{
					if (data.IsError) jQuery('<div>' + data.Message + '</div>').dialog(
						{ buttons: { OK: function () { jQuery(el).dialog("close"); } } })
					else
						$tr.remove().detach()
					setBenefitItemCount();
				}, "application/x-www-form-urlencoded")
			})
			return false
		}


		jQuery(document).ready(function ()
		{
			jQuery('#btnAddNewBenefit').click(function ()
			{
				doAddEdit('<%= Url.Generate<BenefitController>(x => x.AddBenefitItem(null, null)) %>', 'entranceTestItemID=<%= Model.EntranceTestItemID %>&competitiveGroupID=<%=Model.CompetitiveGroupID %>',
				function () { addNewBenefit(jQuery('#trAddNewBenefit'), createdBenefitItem) })
				return false
			})

			for (var i = 0; i < existingItems.length; i++)
			{
				addNewBenefit(jQuery('#trAddNewBenefit'), existingItems[i])
			}
			setBenefitItemCount()
		    <% if(!Model.CanEdit)
			   { %>
					jQuery('#trAddNewBenefit').hide()
					jQuery('#tableBenefitList .btnDelete,#tableBenefitList .btnEdit').remove().detach();
			<% } %>
		})

		function viewOlympicDetails(el, olympicID)
		{
			clearTimeout(olTimerID)
			olTimerID = setTimeout(function ()
			{
			doPostAjax('<%= Url.Generate<BenefitController>(x => x.OlympicDetailsView(null)) %>', "olympicID=" + olympicID, function (data)
			{
				jQuery('#divViewOlympic').html(data);
				var p = jQuery(el).offset()
				jQuery('#divViewOlympic').css('position', 'absolute').css('z-index', 1100).css('top', p.top + jQuery(el).height() + 5).css('left', p.left + 10).fadeIn(300)

			}, "application/x-www-form-urlencoded", "html")
			}, 300)
		}

		var olTimerID = 0
		function hideOlympicDetails()
		{
			clearTimeout(olTimerID)
			olTimerID = setTimeout(function ()
			{
			jQuery('#divViewOlympic').fadeOut(300)
			}, 700)
		}
	</script>
