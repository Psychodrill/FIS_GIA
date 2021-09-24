<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.CampaignDateEditViewModel>" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Model.Entrants" %>
<%@ Import Namespace="GVUZ.Web.ContextExtensions" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Register TagPrefix="gv" TagName="AdminMenuControl" Src="~/Views/Shared/Controls/AdminMenuControl.ascx" %>

<script runat="server" lang="c#">
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Редактирование дат приемной кампании
</asp:Content>
<asp:Content ID="header" ContentPlaceHolderID="PageTitle" runat="server">Приемные кампании</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="divstatement">
<% ViewData["MenuItemID"] = 1; %>
	<gv:AdminMenuControl ID="AdminMenuControl1" runat="server" />	


<div>&nbsp;</div>

<div class="subdivstatement">
<div id="cgSubMenu" class="subsubmenu"></div>

<div class="content">
	<div id="errorPlaceHolder"></div>
				<table class="gvuzDataGrid">
					<thead>
						<tr>
							<th></th>
							<th><%= Html.LabelFor(x => x.DisplayInfo.IsActive)%></th>
							<th><%= Html.LabelFor(x => x.DisplayInfo.DateStart)%></th>
							<th><%= Html.LabelFor(x => x.DisplayInfo.DateEnd)%></th>
							<th><%= Html.LabelFor(x => x.DisplayInfo.DateOrder)%></th>
							<th><%= Html.LabelFor(x => x.DisplayInfo.UID)%></th>
						</tr>
					</thead>
					<tbody>
						<% bool bReadOnly = GVUZ.Helper.UrlUtils.IsReadOnly(FBDUserSubroles.CampaignDirection);
                            var idx = 0;
							foreach (var dt in Model.CampaignDates)
						   { idx++; %>
						   <tr dataInfo="<%= dt.CombinedID %>" class="<%= idx % 2 == 0 ? "trline1" : "trline2" %>">
						   	<td><b><%= dt.NameString %></b></td>
							<td><input type="checkbox" id="cb_<%= dt.CombinedID %>" <%= dt.IsActive ? "checked=\"checked\"" : "" %>
							 onclick="cbActiveChanged(this);"
							 <%--<%= ((dt.IsActive && !dt.CanRemoveDate) || bReadOnly) ? "disabled=\"disabled\"" : "" %>--%> <%--FIS-848--%>
							 <%= dt.IsActive && !dt.CanRemoveDate ? "title=\"Существует конкурс\"" : "" %>/></td>
							<td><input type="text" class="datePicker" id="dp_<%= dt.CombinedID %>_0" value="<%= dt.DateStart != DateTime.MinValue ? dt.DateStart.ToString("dd.MM.yyyy") : null%>" /></td>
							<td><input type="text" class="datePicker" id="dp_<%= dt.CombinedID %>_1" value="<%= dt.DateEnd != DateTime.MinValue ? dt.DateEnd.ToString("dd.MM.yyyy") : null%>"  /></td>
							<td><input type="text" class="datePicker" id="dp_<%= dt.CombinedID %>_2" value="<%= dt.DateOrder != DateTime.MinValue ? dt.DateOrder.ToString("dd.MM.yyyy") : null%>" /></td>
							<td><input type="text" id="uid_<%= dt.CombinedID %>" value="<%= dt.UID ?? ""%>" <%= bReadOnly ? " disabled=\"disabled\"" : "" %>/></td>
							</tr>
						 <% } %>
					</tbody>
				</table>

<div>&nbsp;</div>
<div>
<% if (!bReadOnly)
   { %>
	<input type="button" value="Сохранить" id="btnSave" class="button3" />
<% } %>
	<input type="button" value="Отмена" id="btnCancel" class="button3" />
</div>

</div>
</div>
</div>
<script type="text/javascript">


	function cbActiveChanged(el) {
		var $tr = jQuery(el).parents('tr:first');
		var isChecked = jQuery(el).is(':checked');
		cbActiveChangedInternal(isChecked, $tr);
		var spl = $tr.attr('dataInfo').split('_');
		var $tr2 = null;
		if(spl[4] == 1) {
			$tr2 = jQuery('.gvuzDataGrid tr[dataInfo="' + spl[0] + '_' + spl[1] + '_' + spl[2] + '_' + spl[3] + '_' + '2"]')
		}
		if(spl[4] == 2) {
			$tr2 = jQuery('.gvuzDataGrid tr[dataInfo="' + spl[0] + '_' + spl[1] + '_' + spl[2] + '_' + spl[3] + '_' + '1"]')
		}
		if($tr2 != null) {
			if(isChecked) $tr2.find('input[type="checkbox"]').attr('checked', 'checked')
			else $tr2.find('input[type="checkbox"]').removeAttr('checked');
			cbActiveChangedInternal(isChecked, $tr2);
		}
	}
	
	function cbActiveChangedInternal(isChecked, $tr) {
    <% if (bReadOnly){ %>

			$tr.find('input[type="text"]').attr('disabled', 'disabled').attr('readonly', 'readonly')
			$tr.find('.gvuz-calendar-icon').hide();
   
   <% } else { %>

		if (isChecked)
		{
			$tr.find('input[type="text"]').removeAttr('disabled').removeAttr('readonly')
			$tr.find('.gvuz-calendar-icon').show();
		}
		else 
        {
			$tr.find('input[type="text"]').attr('disabled', 'disabled').attr('readonly', 'readonly')
			$tr.find('.gvuz-calendar-icon').hide();
		}
   <% } %>

	}

	function saveData() {
		var model = {
			CampaignID: <%= Model.CampaignID %>,
			CampaignDates: []
		};

		jQuery('.gvuzDataGrid tbody tr').each(function(i, e) {
			var spl = jQuery(e).attr('dataInfo').split('_');
			model.CampaignDates.push({
				EducationFormID: spl[0],
				EducationLevelID: spl[1],
				Course: spl[2],
				EducationSourceID: spl[3],
				Stage: spl[4],
				IsActive: jQuery(e).find('input[type="checkbox"]').is(":checked"),
				DateStart: jQuery(e).find('input.datePicker:eq(0)').val(),
				DateEnd: jQuery(e).find('input.datePicker:eq(1)').val(),
				DateOrder: jQuery(e).find('input.datePicker:eq(2)').val(),
				UID: jQuery(e).find('input[type="text"]:not(.datePicker)').val()
			});
		});
		clearValidationErrors(jQuery('.content'))
		jQuery('.content').find('.input-validation-error').addClass('input-validation-error-fixed').removeClass('input-validation-error');
		jQuery('.content').find('.field-validation-error').remove().detach();
		doPostAjax('<%= Url.Generate<CampaignController>(x => x.CampaignDateSave(null)) %>', JSON.stringify(model), function(data) {
			if (!addValidationErrorsFromServerResponse(data, true)) {
				window.location = '<%= Url.Generate<CampaignController>(x => x.CampaignList()) %>?campaignID=' + data.Data;
			} else {
				jQuery('#errorPlaceHolder').after('<span class="field-validation-error">Введены некорректные даты или дублирующиеся значения UID</span>');
			}
		});
	}

	function cancelData() {
		window.location = '<%= Url.Generate<CampaignController>(x => x.CampaignList()) %>'
	}

	jQuery(document).ready(function() {
		//jQuery('a.menuitemr,a.menuitemr1,a.menuiteml,a.menuiteml1').click(doClickOuterElement)
		jQuery('#btnSave').click(saveData)
		jQuery('#btnCancel').click(cancelData);
		
		new TabControl(jQuery('#cgSubMenu'), [{ name: 'Общие данные', link: '<%= Url.Generate<CampaignController>(c => c.CampaignEdit(Model.CampaignID)) %>', enable: true }
			, { name: 'Сроки проведения', link: '<%= Url.Generate<CampaignController>(c => c.CampaignDateEdit(Model.CampaignID)) %>', enable: true, selected: true }
		])
			.init();
		<% if(!Model.CanEdit)
		   { %>
		jQuery('input[type="text"],select').attr('readonly', 'readonly').addClass('view')
		jQuery('input[type="checkbox"]').each(function (i, e) {
			if(jQuery(e).is(':checked'))
				jQuery(e).attr('disabled', 'disabled')
			else {
				jQuery(e).parents('tr:first').hide()
			}
		})
		jQuery('#btnSave').hide()
		jQuery('#btnCancel').val('Вернуться')
		<% } else { %>
        var dat = new Date();
        dat.setFullYear(<%= Model.YearStart %>);

		jQuery(".datePicker").datepicker({ defaultDate: dat, changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '<%= Model.YearStart %>:<%= Model.YearEnd %>' });
		jQuery('.gvuzDataGrid tbody tr').each(function(i, e) {
			cbActiveChanged(jQuery(e).find('input[type="checkbox"]')[0]);
		})
<%} %>

	})
		
</script>
</asp:Content>
