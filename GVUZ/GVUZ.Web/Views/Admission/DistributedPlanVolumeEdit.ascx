<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.AdmissionVolume.DistributedPlanAdmissionVolumeViewModel>" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.ViewModels.AdmissionVolume" %>
<%@ Import Namespace="GVUZ.DAL.Dapper.ViewModel.Dictionary" %>
<%@ Import Namespace="GVUZ.Web.Controllers.Admission" %>

<% 
    foreach (AdmissionVolumeClassificatorItemViewModel budget in Model.Budgets)
    {
        bool isFirstBudget = (budget == Model.Budgets.First());
%>
<tr data-distributionEditor="">
    <% if (isFirstBudget)
       { %>
    <td style="padding:0px;" colspan="2" rowspan="<%=Model.Budgets.Count() %>">
        <table style="width: 100%;">
            <tbody>
                <%foreach (AdmissionVolumeClassificatorItemViewModel budgetForName in Model.Budgets)
                  {
                      bool isFirstBudgetForName = (budgetForName == Model.Budgets.First());
                %>
                <tr>
                    <%if (isFirstBudgetForName)
                      { %>
                    <td style="background-color:#f0f0f0;font-weight:bold;color:#0a4b9c" rowspan="<%=Model.Budgets.Count() %>">Уровень бюджета</td>
                    <%} %>
                    <td style="height:22px;"><%=budgetForName.Name %></td>
                </tr>
                <%} %>
            </tbody>
        </table>
    </td>
    <% } %> 
    <% foreach (AdmissionVolumeClassificatorItemViewModel financeSource in Model.FinanceSources)
       {
           if (financeSource.Id == EDSourceConst.Paid)
           {
    %>
    <td colspan="<%=Model.EducationForms.Count() %>"></td>
    <%
               continue;
           }
           foreach (AdmissionVolumeClassificatorItemViewModel educationForm in Model.EducationForms)
           {
               DistributedPlanAdmissionVolumeItemViewModel item = Model.GetItem(budget.Id, financeSource.Id, educationForm.Id);
    %>
    <td style="text-align:center;padding:0px;">
        <% if(Model.CanDistributeItem(financeSource.Id,educationForm.Id)) { %>
        <input type="text" class="numeric" style="width:48px;" onchange="handleNumberInput(this);" data-editor="" data-budgetId="<%=budget.Id %>" data-financesourceid="<%=financeSource.Id %>" data-educationformid="<%=educationForm.Id %>" value="<%=(item != null)?item.Number:0%>" />
        <% } %>
    </td>
    <% }
       }%>
    <td style="text-align: right;border-left-color: rgb(192, 192, 192); border-left-width: 4px; border-left-style: double;"></td>
    <td style="text-align: right;padding-top:0px;padding-bottom:0px;" data-aggregate="" data-budgetId="<%=budget.Id %>"><%=Model.GetTotalDistributed(budget.Id) %></td>
    <% if (isFirstBudget)
       { %>
    <td style="padding-top:0px;padding-bottom:0px;" rowspan="<%=Model.Budgets.Count() %>">
        <a href="javascript:void(0)" onclick="doSaveDistribution();" style="margin-bottom: 8px" class="btnSave" onclick="doSave" title="Сохранить">&nbsp;</a><br />
        <a href="javascript:void(0)" onclick="doCancelEditDistribution();" class="btnCancel" title="Отменить">&nbsp;</a>
    </td>
    <%} %>
</tr>

<%} %>

<script type="text/javascript">
    //removeDistributionEditorRows - описан в PlanVolumeView.aspx
    var ISCHANGED = false;

    function handleNumberInput(sender) {
        ISCHANGED = true;
        refreshBudgetAggregate(sender);
        refreshDirectionAggregate();
    }

    function refreshBudgetAggregate(sender) {
        var $sender = $(sender);
        var $row = $sender.closest('tr');

        var budgetId = $sender.attr('data-budgetId');

        var sum = 0;
        var $editors = $row.find('[data-editor=""][data-budgetId="' + budgetId + '"]');
        $editors.each(function () {
            var $editor = $(this);
            sum += parseInt($editor.val());
        });

        $budgetAggregate = $row.find('[data-aggregate=""][data-budgetId="' + budgetId + '"]');
        $budgetAggregate.text(sum); 
    }

    function refreshDirectionAggregate()
    { 
        var sum = 0;
        var $allBudgetAggregates = $('[data-aggregate=""][data-budgetId]');
        $allBudgetAggregates.each(function () {
            var $allBudgetAggregate = $(this);
            var aggragateVal = parseInt($allBudgetAggregate.text());
            if(!isNaN(aggragateVal)){
                sum += aggragateVal;
            }
        });

        $directionAggregate = $('[data-aggregate=""][data-levelId="<%=Model.LevelId%>"][data-directionId="<%=Model.DirectionId%>"]');
        $directionAggregate.text(sum); 
    } 

    function doSaveDistribution() { 
        var saveModel = {};

        saveModel.CampaignId = '<%=Model.CampaignId%>';
        saveModel.LevelId = '<%=Model.LevelId%>';
        saveModel.DirectionId = '<%=Model.DirectionId%>';
        saveModel.ParentDirectionId = '<%=Model.ParentDirectionId%>';

        saveModel.Items = [];

        var $editors = $('[data-distributionEditor=""]').find('[data-editor=""]');
        $editors.each(function () {
            var $editor = $(this);

            $editor.removeClass('input-validation-error');
            $editor.removeAttr('title');

            var saveModelItem = {};
            saveModelItem.BudgetId = $editor.attr('data-budgetId');
            saveModelItem.FinanceSourceId = $editor.attr('data-financeSourceId');
            saveModelItem.EducationFormId = $editor.attr('data-educationFormId');
            saveModelItem.Number = $editor.val();
            saveModel.Items.push(saveModelItem);
        });

        doPostAjax('<%= Url.Generate<AdmissionController>(c => c.DistributedPlanVolumeSave(null)) %>', JSON.stringify(saveModel), function (ajaxResult) {
            if (ajaxResult.IsError) {
                if (ajaxResult.Data && ajaxResult.Data.length) {
                    for (var i = 0; i < ajaxResult.Data.length; i++) {
                        var $editor = $('[data-distributionEditor=""]').find('[data-editor=""][data-budgetId="' + ajaxResult.Data[i].BudgetId + '"][data-financeSourceId="' + ajaxResult.Data[i].FinanceSourceId + '"][data-educationFormId="' + ajaxResult.Data[i].EducationFormId + '"]');
                        $editor.addClass('input-validation-error');
                        $editor.attr('title', ajaxResult.Message);
                    }
                }
            }
            else {
                removeDistributionEditorRows(false);
            }
        });
    }

    function doCancelEditDistribution() { 
        if (ISCHANGED) {
            confirmDialog('На странице есть несохраненные данные. Вы действительно хотите отменить редактирование?', function () { removeDistributionEditorRows(true); });
        }
        else {
            removeDistributionEditorRows(true);
        }
    }
</script>
