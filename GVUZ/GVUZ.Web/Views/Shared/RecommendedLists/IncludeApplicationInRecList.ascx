<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.RecommendedLists.ApplicationIncludeInRecListViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.Security" %>
<%@ Import Namespace="GVUZ.Web.ViewModels" %>

<div id="recListContent">
    <table>
        <tbody>
            <tr>
                <td class="labelsInside">
                    <%= Html.LabelFor(x => x.ApplicationNumber) %>
                </td>
                <td style="width: 50%;">
                    <%= Html.DisplayFor(x => x.ApplicationNumber) %>
                </td>
                <td style="vertical-align: middle; text-align: center;" rowspan="3">
                    Этап зачисления:
                </td>
                <td style="vertical-align: middle; text-align: center;" rowspan="3">
                    <select id="stageSelector" class="ss" style="width: 200px;" onchange="doStageChange()">
                        <option value="-1">Выберите этап</option>
                        <option value="1">1 этап</option>
                        <option value="2">2 этап</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td class="labelsInside">
                    <%= Html.LabelFor(x => x.EntrantName) %>
                </td>
                <td>
                    <%= Html.DisplayFor(x => x.EntrantName) %>
                </td>
            </tr>
            <tr>
                <td class="labelsInside">
                    <%= Html.LabelFor(x => x.DocumentData) %>
                </td>
                <td>
                    <%= Html.DisplayFor(x => x.DocumentData) %>
                </td>
            </tr>
        </tbody>
    </table>
    <div id="dataList">
        <table class="gvuzDataGrid">
            <thead>
                <tr>
                    <th><%= Html.LabelFor(x => x.ViewData.CompetitveGroupName) %></th>
                    <th><%= Html.LabelFor(x => x.ViewData.DirectionName) %></th>
                    <th><%= Html.LabelFor(x => x.ViewData.EduLevelName) %></th>
                    <th><%= Html.LabelFor(x => x.ViewData.EduForm) %></th>
                    <th><%= Html.LabelFor(x => x.ViewData.EdSource)%></th>
                    <th><%= Html.LabelFor(x => x.ViewData.Priority) %></th>
                    <th><%= Html.LabelFor(x => x.ViewData.Recommended) %></th>
                </tr>
            </thead>
            <tbody>
                <tr id="trBefore"></tr>
            </tbody>
        </table>
    </div>
<div style="display:none">
	<input type="button" id="btnRecListInclude" />
	<input type="button" id="btnRecListCancel" />
</div>
</div>
<script type="text/javascript">

// ----------------------------------- Начальная загрузка -----------------------------------
    jQuery('#btnRecListCancel').click(function () { includeInRecListCloseDialog() })
    jQuery('#btnRecListInclude').click(function () { doRecListIncludeSubmit() })

    var stage1Data = JSON.parse('<%= Html.Serialize(Model.Stage1List) %>');
    var stage2Data = JSON.parse('<%= Html.Serialize(Model.Stage2List) %>');

    doStageChange();
    // ----------------------------------- Начальная загрузка -----------------------------------

    // ------------------------------------ Смена этапа зачисления ------------------------------

    function addDataRow($tr, dataItem, stage) {
        var htmlRow = '<td>' + dataItem.CompetitveGroupName + '</td>';
        htmlRow += '<td>' + dataItem.DirectionName + '</td>';
        htmlRow += '<td>' + dataItem.EduLevelName + '</td>';
        htmlRow += '<td>' + dataItem.EduForm + '</td>';
        htmlRow += '<td>' + dataItem.EdSource + '</td>';
        htmlRow += '<td>' + dataItem.Priority.toString() + '</td>';
        htmlRow += '<td><input data-itemid="' + dataItem.Id + '" type="checkbox" onchange="doChangeRecommended(this, ' + stage + '); return false;" ' + (stage > 0 && dataItem.Recommended ? 'checked="checked"' : '') + (stage < 0 ? 'disabled="disabled" />' : ' />') + '</td>';

        var className = $tr.prev().attr('class');
        if (className == 'trline2') className = 'trline1';
        else className = 'trline2';

        $tr.before('<tr class="' + className + '">' + htmlRow + '</tr>');
    }

    function doStageChange() {
        var stage = jQuery('#stageSelector').val();
        var items;

        jQuery('#dataList .gvuzDataGrid tbody tr:not(#trBefore)').remove().detach();

        if (stage == -1) { //Этап не выбран
            items = stage1Data;    
        }

        if (stage == 1) { // 1 этап
            items = stage1Data;    
        }

        if (stage == 2) { // 2 этап
            items = stage2Data;
        }

        jQuery.each(items, function (idx, n) { addDataRow(jQuery('#trBefore'), n, stage) });
    }
    // ------------------------------------ Смена этапа зачисления ------------------------------

    // --------------------------------------- Выбор на включение-исключение ----------------------
    function doChangeRecommended(el, stage) {
        var itemId = jQuery(el).attr('data-itemid');
        var val = jQuery(el).attr('checked');

        var itemsToChange = (stage == 1 ? stage1Data : stage2Data);

        for (var i = 0; i < itemsToChange.length; i++) {
            if (itemsToChange[i].Id == itemId) {
                itemsToChange[i].Recommended = val;
                break;
            }
        }
    }
    // --------------------------------------- Выбор на включение-исключение ----------------------

    // --------------------------------------- Сохранение выбора ----------------------------------
    function doRecListIncludeSubmit() {
        var model = {
            ApplicationId: <%= Model.ApplicationId %>,
            Stage1List: stage1Data,
            Stage2List: stage2Data
        };

        doPostAjax('<%= Url.Generate<RecommendedListsController>(x => x.SaveRecommendedLists(null)) %>', JSON.stringify(model), function(data){
            if (data.Message != null && data.Message != '')
            {
                alert(data.Message);
                return false;
            }

            includeInRecListCloseDialog();
        });
    }
    // --------------------------------------- Сохранение выбора ----------------------------------

</script>