<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.ApplicationIncludeInOrderViewModel>" %>
<%@ Import Namespace="FogSoft.Helpers" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Model.Institutions" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.ViewModels" %>

<div class="content">
<% if(Model.OrderErrorMessage != null) 
{%>
 <p><%: Model.OrderErrorMessage %></p>
 <%} 
 else 
 { %>
 <!-- Данные по заявлениям пользователей (заполняются на клиенте) -->
 <table class="gvuzDataGrid" width="99%" cellpadding="1" id="apps4OrderGrid">
	<thead>
		<tr>
			<th style="text-align:center; width:15%">
				<span><%= Html.LabelFor(x => x.ApplicationNumber) %></span>
			</th>
			<th style="text-align:center; width:30%">
				<span><%= Html.LabelFor(x => x.FIO) %></span>
			</th>
			<th style="text-align:center; width:30%">
				<span><%= Html.LabelFor(x => x.DocumentData) %></span>
			</th>
		</tr>
	</thead>
	<tbody id="bulkApps4OrderBody">
	</tbody>
</table>
<br />
<br />
<br />
  <!-- Общие данные -->
  <table class="data">
	<tbody>
		<tr>
			<td class="caption"><%= Html.TableLabelFor(x => x.DirectionID) %></td>
			<td><%= Html.DropDownListExFor(x => x.DirectionID, Model.Directions, new { onchange = "directionChanged()", style="width:100%;" })%></td>
		</tr>
		<tr>
			<td class="caption"><%= Html.TableLabelFor(x => x.EducationFormID) %></td>
			<td><%= Html.DropDownListExFor(x => x.EducationFormID, Model.EducationForms, new { onchange = "eduFormChanged()" })%></td>
		</tr>
		<tr>
			<td class="caption"><%= Html.TableLabelFor(x => x.EducationSourceID) %></td>
			<td><%= Html.DropDownListExFor(x => x.EducationSourceID, Model.EducationSources, new { onchange = "eduSourceChanged()" })%></td>
		</tr>
		<tr>
			<td class="caption"><%= Html.TableLabelFor(x => x.IsForBeneficiary) %></td>
			<td><%= Html.CheckBoxFor(x => x.IsForBeneficiary)%></td>
		</tr>
        <% if (Model.IsForeignCitizen) { %>
        <tr>
			<td class="caption"><%= Html.TableLabelFor(x => x.IsBudgetForeigner) %></td>
			<td><%= Html.CheckBoxFor(x => x.IsBudgetForeigner)%></td>
		</tr>
        <% } %>
	</tbody>
 </table>
<%} %>
<div style="display:none">
	<input type="button" id="btnOrderInclude" />
	<input type="button" id="btnOrderCancel" />
</div>
<div style="display:none">
	<div id="divOrderTypeSelection">
	</div>
</div>
</div>
<script type="text/javascript">

    var orderTypeSelection = 0;
    var orderForcePublished = 0;

    var availableForms = JSON.parse('<%= Html.Serialize(Model.AvailableForms) %>');
    var availableSources = JSON.parse('<%= Html.Serialize(Model.AvailableSources) %>');
    var allForms = JSON.parse('<%= Html.Serialize(Model.EducationForms) %>');
    var allSources = JSON.parse('<%= Html.Serialize(Model.EducationSources) %>');
    var beneficiaryDirections = JSON.parse('<%= Html.Serialize(Model.BeneficiaryDirections) %>');
    var applicationIDs = JSON.parse('<%= Html.Serialize(Model.applicationIds) %>');

    // Хэндлер кнопки "включить в приказ"
	function doOrderIncludeSubmit()
	{
        
		if(revalidatePage(jQuery('.data')))
        {
            alert('Нельзя добавить в приказ');
		    return false;
		}

	    var model = {
	        EducationFormID: jQuery('#EducationFormID').val(),
	        EducationSourceID: jQuery('#EducationSourceID').val(),
	        DirectionID: jQuery('#DirectionID').val(),
	        OrderTypeSelection: orderTypeSelection,
	        OrderForcePublished: orderForcePublished,
	        IsForBeneficiary: jQuery('#IsForBeneficiary').is(':checked'),
	        applicationIds: applicationIDs,
	        IsBudgetForeigner: jQuery('#IsBudgetForeigner').length > 0 && jQuery('#IsBudgetForeigner').is(':checked') && jQuery('#IsBudgetForeigner').is(':enabled')
	    };
        
		if(model.EducationFormID == 0 || model.EducationSourceID == 0) {
		    alert('Заявление невозможно включить в приказ с указанной специальностью.');
		    return false;
		}

		doPostAjax('<%= Url.Generate<ApplicationController>(x => x.IncludeApplicationsInOrder(null))%>', JSON.stringify(model), function (data) {
		    if (!addValidationErrorsFromServerResponse(data)) {
		        if (data.Data) {
		            if (data.Data[0] == '0')
		                doOrderTypeSelection(data.Data);
		            if (data.Data[0] == '1')
		                doOrderForcePublishQuestion();
		            return;
		        }
		        if (closeBulkOrderDlg && typeof(closeBulkOrderDlg) === 'function') {
		            closeBulkOrderDlg(); // from parent page
		        } 
		    }
		});
	}

    // Выбор конкретного приказа
	function doOTSChange(el, val) {
	    if (jQuery(el).attr('checked'))
	        orderTypeSelection = val;
	}

    // Диалог выбора приказа
	function doOrderTypeSelection(arr) {
	    var dialogText = '<input type="radio" name="rbOTS" id="rbOTS1" onclick="doOTSChange(this, 1)" /><label for="rbOTS1" id="lbOTS1">' + arr[1] + '</label><br />' +
	        '<input type="radio" name="rbOTS" id="rbOTS2" onclick="doOTSChange(this, 2)" /><label for="rbOTS2" id="lbOTS2">' + arr[2] + '</label>';
	    jQuery('#divOrderTypeSelection').html(dialogText);
		jQuery('#divOrderTypeSelection').dialog({
				modal: true,
				width: 600,
				title: 'Выберите нужный приказ',
				buttons:
							{
								"Включить в приказ": function () {
								    if (orderTypeSelection == 0)
								        alert('Приказ не выбран');
								    else {
								        doOrderIncludeSubmit();
								        closeDialog(jQuery(this));
								    }
								},
								"Отмена": function() {
								     closeDialog(jQuery(this));
								}
							}
			}).dialog('open');
	}


    // Подтверждение изменений в опубликованном приказе
	function doOrderForcePublishQuestion() {
	    var tmp = orderTypeSelection;
	    orderTypeSelection = 0;
	    confirmDialog('Данный приказ уже опубликован. Вы уверены, что хотите включить в него заявление? (Приказ изменит статус)',
	        function() {
	            orderTypeSelection = tmp;
	            orderForcePublished = 1;
	            doOrderIncludeSubmit();
	        });
	}

    // Хэндлер изменений в списке направлений (специальностей)
	function directionChanged() {
	    var selectedID = jQuery('#DirectionID').val();
	    var res = '';
		for(var i = 0; i < availableForms.length;i++)
		{
			if(availableForms[i].indexOf('' + selectedID + '.') == 0) {
			    var spl = availableForms[i].split('.');
			    jQuery.each(allForms, function() {
			        if (this.ID == spl[1]) {
			            res += '<option value="' + this.ID + '">' + this.Name + '</option>';
			            return false;
			        }
			    });
			}
		}
	    if (res == '') {
	        jQuery('#EducationFormID').attr('disabled', 'disabled');
	        res = '<option value="0">Недоступно</option>';
	    } else jQuery('#EducationFormID').removeAttr('disabled');

	    jQuery('#EducationFormID').html(res);
	    eduFormChanged();
	}

    // Хэндлер изменений в списке форм обучения
	function eduFormChanged() {
	    var selectedID1 = jQuery('#DirectionID').val();
	    var selectedID2 = jQuery('#EducationFormID').val();
	    var res = '';
		for(var i = 0; i < availableSources.length;i++)
		{
			if(availableSources[i].indexOf('' + selectedID1 + '.' + selectedID2 + '.') == 0) {
			    var spl = availableSources[i].split('.');
			    jQuery.each(allSources, function() {
			        if (this.ID == spl[2]) {
			            res += '<option value="' + this.ID + '">' + this.Name + '</option>';
			            return false;
			        }
			    });
			}
		}
	    if (res == '') {
	        jQuery('#EducationSourceID').attr('disabled', 'disabled');
	        res = '<option value="0">Недоступно</option>';
	    } else jQuery('#EducationSourceID').removeAttr('disabled');
	    jQuery('#EducationSourceID').html(res);
		eduSourceChanged();
	}

    // Хэндлер изменений в списке источников финансирования
	function eduSourceChanged() {
	    var selectedID1 = jQuery('#DirectionID').val();
	    var selectedID3 = jQuery('#EducationSourceID').val();
	    if (beneficiaryDirections != null)
	        jQuery("#IsForBeneficiary").toggleEnabled(selectedID3 == '<%= EDSourceConst.Budget.To(0) %>' && beneficiaryDirections.indexOf(parseInt(selectedID1)) >= 0);
	    else
	        jQuery("#IsForBeneficiary").toggleEnabled(false);
	    if (jQuery('#IsBudgetForeigner').length > 0) {
	        jQuery('#IsBudgetForeigner').toggleEnabled(selectedID3 == '<%= EDSourceConst.Budget.To(0) %>');
	    }
	}

	//jQuery('#btnOrderCancel').click(function () { /*includeInOrderCloseDialog(0)*/  })
	jQuery('#btnOrderInclude').click(function() { doOrderIncludeSubmit(); });
    if ((availableForms != null) && (allForms != null)) {
        directionChanged();
    }
</script>

