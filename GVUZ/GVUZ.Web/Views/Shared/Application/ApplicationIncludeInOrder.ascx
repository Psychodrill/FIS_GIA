<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.ApplicationIncludeInOrderViewModel>" %>
<%@ Import Namespace="FogSoft.Helpers" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Model.Institutions" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.ViewModels" %>

<div class="content">
<% if(Model.NoData) { %> Заявлению не назначен конкурс <%}
   else if (Model.NoOriginalDocuments && !Model.IsVuz)
   {%>В приказ можно включить только заявления, для которых предоставлены оригиналы документов <%}
   else if (!Model.ForListener && Model.NoOriginalDocuments && !Model.CanIncludeInOrder)
   {%>В приказ на бюджетные места можно включить только заявления, для которых предоставлены оригиналы документов или справка об обучении в другом ВУЗе<%}


else if(Model.OrderErrorMessage != null) {%><%: Model.OrderErrorMessage %><%} else { %>
<table class="data">
	<tbody>
		<tr>
			<td class="caption"><%= Html.TableLabelFor(x => x.ApplicationNumber) %></td>
			<td><%= Model.ApplicationNumber ?? Html.TextBoxExFor(x => x.ApplicationNumber).ToHtmlString() %></td>
		</tr>
		<tr>
			<td class="caption"><%= Html.TableLabelFor(x => x.FIO) %></td>
			<td><%: Model.FIO %></td>
		</tr>
		<tr>
			<td class="caption"><%= Html.TableLabelFor(x => x.DocumentData) %></td>
			<td><%: Model.DocumentData %></td>
		</tr>
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

	var orderTypeSelection = 0
	var orderForcePublished = 0

	var availableForms = JSON.parse('<%= Html.Serialize(Model.AvailableForms) %>')
	var availableSources = JSON.parse('<%= Html.Serialize(Model.AvailableSources) %>')
	var allForms = JSON.parse('<%= Html.Serialize(Model.EducationForms) %>')
	var allSources = JSON.parse('<%= Html.Serialize(Model.EducationSources) %>')

	var targetOrgNameO = '<%= (Model.TargetOrganizationNameO ?? "").Replace("'", "\\'") %>';
	var targetOrgNameOZ = '<%= (Model.TargetOrganizationNameOZ ?? "").Replace("'", "\\'") %>';
	var targetOrgNameZ = '<%= (Model.TargetOrganizationNameZ ?? "").Replace("'", "\\'") %>';

	var beneficiaryDirections = JSON.parse('<%= Html.Serialize(Model.BeneficiaryDirections) %>')
    var direction = '<%= Model.DirectionID %>'

	function doOrderIncludeSubmit() {
	    
        if (revalidatePage(jQuery('.data')))
	        return false;

	    var model = {
	        ApplicationID: <%= Model.ApplicationID %>,
	        ApplicationNumber: jQuery('#ApplicationNumber').val(),
	        EducationFormID: jQuery('#EducationFormID').val(),
	        EducationSourceID: jQuery('#EducationSourceID').val(),
	        DirectionID: jQuery('#DirectionID').val(),
	        OrderTypeSelection: orderTypeSelection,
	        OrderForcePublished: orderForcePublished,
	        IsForBeneficiary: jQuery('#IsForBeneficiary').is(':checked'),
	        IsBudgetForeigner: jQuery('#IsBudgetForeigner').length > 0 && jQuery('#IsBudgetForeigner').is(':checked') && jQuery('#IsBudgetForeigner').is(':enabled')
	    };
	    
		if(model.EducationFormID == 0 || model.EducationSourceID == 0) {
		    alert('Заявление невозможно включить в приказ с указанной специальностью.');
		    return false;
		}
	    doPostAjax('<%= Url.Generate<ApplicationController>(x => x.IncludeApplicationInOrder(null))%>', JSON.stringify(model), function(data) {
	        if (data.Data == 'В приказ на бюджетные места можно включить только заявления, для которых предоставлены оригиналы документов об образовании') {
	            alert(data.Data);
	            return false;
	        }

	        if (!addValidationErrorsFromServerResponse(data)) {
	            if (data.Data) {
	                if (data.Data[0] == '0')
	                    doOrderTypeSelection(data.Data);
	                if (data.Data[0] == '1')
	                    doOrderForcePublishQuestion();
	                return false;
	            }
	            if (includeInOrderCloseDialog && typeof(includeInOrderCloseDialog) === 'function') {
	                includeInOrderCloseDialog(1);
	            }
	        }
	    });
	}

	function doOTSChange(el, val) {
	    if (jQuery(el).attr('checked'))
	        orderTypeSelection = val;
	}

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
								"Отмена": function () { closeDialog(jQuery(this)); }
							}
			}).dialog('open');
	}

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

	function directionChanged() {

	    var selectedID = jQuery('#DirectionID').val();

	    var model = {
	        applicationId: <%= Model.ApplicationID %>,
	        directionId: selectedID
	    };

        doPostAjax('<%= Url.Generate<ApplicationController>(x => x.GetFormsForDirection(null, null)) %>', JSON.stringify(model),
            function(data){
                var res = '';
                if (data.Data != null)
                {
                    for (var i = 0; i < data.Data.length; i++)
                    {
                        res += '<option value="' + data.Data[i].ID + '">' + data.Data[i].Name + '</option>';
                    }
                }

                if (res == '') {
                    jQuery('#EducationFormID').attr('disabled', 'disabled');
                    res = '<option value="0">Недоступно</option>';
                } else jQuery('#EducationFormID').removeAttr('disabled');

                jQuery('#EducationFormID').html(res);
                eduFormChanged();
            });
	}

	function eduFormChanged() {
	    var selectedID1 = jQuery('#DirectionID').val();
	    var selectedID2 = jQuery('#EducationFormID').val();

	    var model = {
	        applicationId: <%=Model.ApplicationID %>,
	        directionId: selectedID1,
	        eduFormId: selectedID2
	    };

        doPostAjax('<%= Url.Generate<ApplicationController>(x => x.GetSourcesForDirection(null, null, null)) %>', JSON.stringify(model),
            function(data)
            {
        		var res = '';
                if (data.Data != null)
                {
                    for (var i = 0; i < data.Data.length; i++)
                        res += '<option value="' + data.Data[i].ID + '">' + data.Data[i].Name + '</option>';
                }

		        if(res == '')
		        {
			        jQuery('#EducationSourceID').attr('disabled', 'disabled')
			        res = '<option value="0">Недоступно</option>'
		        } else jQuery('#EducationSourceID').removeAttr('disabled')
		        jQuery('#EducationSourceID').html(res)
		        eduSourceChanged();
            });
	}

	function eduSourceChanged() {
	    var selectedID1 = jQuery('#DirectionID').val();
	    var selectedID3 = jQuery('#EducationSourceID').val();
	    
        jQuery("#IsForBeneficiary").toggleEnabled((selectedID3 == '<%= EDSourceConst.Budget.To(0) %>' || selectedID3 == '<%= EDSourceConst.Target.To(0) %>' || selectedID3 == '20') && beneficiaryDirections.indexOf(parseInt(selectedID1)) >= 0);
	    
	    if (jQuery('#IsBudgetForeigner').length > 0) {
	        jQuery('#IsBudgetForeigner').toggleEnabled(selectedID3 == '<%= EDSourceConst.Budget.To(0) %>');
	    }
	}

	//jQuery('#btnOrderCancel').click(function () { includeInOrderCloseDialog(0) })
	jQuery('#btnOrderInclude').click(function() { doOrderIncludeSubmit(); });
	directionChanged();
</script>
