<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.IndividualAchivementsViewModel>" %>
<%@ Register TagName="ApplicationWizardButtons" TagPrefix="gv" Src="~/Views/Shared/Portlets/Applications/ApplicationWizardButtons.ascx" %>
<%@ Register TagName="EntrantDocumentAddPart" TagPrefix="gv" Src="~/Views/Shared/Controls/EntrantDocumentAddPart.ascx" %>

<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.Models" %>

<gv:ApplicationWizardButtons ID="ApplicationWizardButtons1" runat="server" ApplicationStep="IndividualAchivements" IsTop="true" />

<h4>Сведения об индивидуальных достижениях поступающего</h4>
<p>Внимание! Поле "Дополнительный балл" необходимо заполнять только в том случае, если правилами приема образовательной организации предусмотрено начисление баллов за индивидуальные достижения.</p>
<table class="gvuzDataGrid" cellpadding="3" id="indAchGrid">
    <thead>
        <tr>
            <th><%= Html.TableLabelFor(x => x.FakedAchivement.UID, required: false)%></th>
            <th><%= Html.TableLabelFor(x => x.FakedAchivement.IAName, required: true)%></th>
            <th><%= Html.TableLabelFor(x => x.FakedAchivement.IAMark, required: false)%></th>
            <th><%= Html.TableLabelFor(x => x.FakedAchivement.IADocumentDisplay, required: true)%></th>
            <th>Действия</th>
        </tr>
    </thead>
    <tbody>
        <tr id="trAddNew1" style="display: none;"></tr>
        <tr id="addNewRow">
            <td colspan="5">
                <a href="#" class="add" id="btAddAchievement" onclick="addAchievement(this)">Добавить</a>
            </td>
        </tr>
    </tbody>
</table>
<div id="documentAddDialog"></div>

<script type="text/javascript">
    function doSubmit(cmd) {
        if (typeof doAppNavigate != "undefined")
            doAppNavigate()
        else
            jQuery('#btnCancel').click()
    }

    var achivements = JSON.parse('<%= Html.Serialize(Model.Items) %>');
    var entrantID = <%= Model.EntrantID %>
    var applicationID = <%= Model.ApplicationID %>
    var getFileLink = '/Entrant/GetFile1';
    var getFileSep = '?';


		var doView = function (navUrl, postData)
		{
/*			doPostAjax(navUrl, postData, function (data)
			{
				jQuery('#documentAddDialog').html(data)
				jQuery('a.getFileLink').each(function () { jQuery(this).attr('href', getFileLink + getFileSep + 'fileID=' + jQuery(this).attr('fileID')) })
				jQuery('#documentAddDialog').dialog({
					modal: true,
					width: 800,
					title: "Редактирование документа",
					buttons: {
                        "Сохранить" : function() {},
						"Закрыть": function () { jQuery(this).dialog('close'); }
					},
					close: function() {}
				}).dialog('open');
			}, "application/x-www-form-urlencoded", "html")*/
            openAddDocumentDialog(navUrl, postData, function() {onNewDocumentCreated(createdItem, true);}, true);
		}

    var doEditNew = function(navUrl, postData)
    {
            openAddDocumentDialog(navUrl, postData, function() {onNewDocumentCreated(createdItem, false);}, true);
    }
    
    function doAddRow($rowBefore, item)
    {
        var className = $rowBefore.prev().attr('class');
        if(className == 'trline1') className = 'trline2'; else className = 'trline1';

        var htmlRow = '<tr><td>' + (item.UID == null ? '' : item.UID) + '</td><td class="nameCell">' + item.IAName + '</td><td>' + (item.IAMark == null ? '' : item.IAMark.toString()) + '</td><td><a href="#" documentID="' + item.IADocument.EntrantDocumentID + '" onclick="showDocument(this)">' + item.IADocumentDisplay + '</a></td>';
        htmlRow += '<td><a href="#" class="btnDelete Achievement" onclick="deleteAchievement(this)" achievementID="' + item.IAID + '">&nbsp;</a></td>';
        $rowBefore.before(htmlRow);
    }

    function addAchievement(el) {
        jQuery("#addNewRow").hide();
        var newRowHtml = '<tr id="rowAdded"><td><input type="text" id="uidInput" /></td>';
        newRowHtml += '<td><input type="text" id="nameInput" /></td>';
        newRowHtml += '<td><input type="text" class="numeric" id="markInput" /></td>';
        newRowHtml += '<td><a href="#" id="addDocumentLink" onclick="addAchievementDocument()">Добавить подтверждающий документ</a></td>';
        newRowHtml += '<td><a href="#" class="btnSave bthSaveAchievement" onclick="saveAchievement()">&nbsp;</a>&nbsp;<a href="#" class="btnDelete" onclick="cancelAddAchievement()">&nbsp;</a></td></tr>';

        jQuery("#addNewRow").before(newRowHtml);
    }

    function cancelAddAchievement() {
        jQuery('#rowAdded').remove().detach();
        jQuery("#addNewRow").show();
        // Документ пока не удаляем. Понадобится - вставлять код сюда!
    }

    function addAchievementDocument() {
        doAddDocument(15, null, null);
    }

    function onNewDocumentCreated(createdItem, needRefresh) {
        var documentType = '';
        var elements = createdItem.DocumentTypeName.split('(');
        if (elements.length > 1)
            elements = elements[1].split(')');
        
        var linkText;
        if (elements.length > 0)
            linkText = elements[0] + ' №' + createdItem.DocumentSeriesNumber + ' от ' + createdItem.DocumentDate;
        else linkText = createdItem.DocumentTypeName + ' №' + createdItem.DocumentSeriesNumber + ' от ' + createdItem.DocumentDate;

        var newLinkHtml = '<a href="#" id="addDocumentLink" onclick="editNewDocument(this)">' + linkText + '</a>';
        jQuery("#addDocumentLink").after(newLinkHtml).remove();
        jQuery("#addDocumentLink").attr('documentID', createdItem.EntrantDocumentID);

        if (needRefresh)
            window.location.reload(1);
    }

    function editNewDocument(el)
    {
        var documentId = jQuery(el).attr('documentID');
        doEditNew('<%= Url.Generate<EntrantController>(x => x.EditDocument(0)) %>', 'entrantDocumentID=' + documentId);
    }

    function showDocument(el)
    {
        var documentId = jQuery(el).attr('documentID');
		doView('<%= Url.Generate<EntrantController>(x => x.EditDocument(0)) %>', 'entrantDocumentID=' + documentId);
    }

    function deleteAchievement(el)
    {
        var achievementID = jQuery(el).attr('achievementID');
        var achievementName = jQuery('tr:has(td:has(a[achievementID="' + achievementID + '"]))').children('td.nameCell').html();

        if (!confirm('Удалить индивидуальное достижение ' + achievementName + ' и подтверждающий его документ?'))
            return false;
        
        var model = {achievementID: achievementID};
        doPostAjax(
            '<%= Url.Generate<ApplicationController>(x => x.DeleteIndividualAchievement(null)) %>',
            JSON.stringify(model),
            function(data)
            {
                if (data.Message != null)
                {
                    alert(data.Message);
                    return false;
                }
                else window.location.reload(1);
            });
    }

    function saveAchievement() {
        var flagError = false;

        if (jQuery('#nameInput').val() == '') {
            jQuery('#nameInput').addClass('input-validation-error');
            flagError = true;
        }
        else {
            jQuery('#nameInput').removeClass('input-validation-error');
        }

        var markVal = jQuery('#markInput').val();

        if (markVal.split('.').length == 1)
            markVal += '.0';

        if (Number(markVal).toString() == "NaN") {
            jQuery('#markInput').addClass('input-validation-error');
            flagError = true;
        }
        else {
            jQuery('#markInput').removeClass('input-validation-error');
        }

        var documentID = jQuery("#addDocumentLink").attr('documentID');

        if (documentID == '' || documentID == undefined)
        {
            jQuery("td>a#addDocumentLink").addClass('input-validation-error');
            flagError = true;
        }
        else{
            jQuery("td>a#addDocumentLink").removeClass('input-validation-error');
        }

        if (flagError) return false;

        var model = {
            UID: jQuery('#uidInput').val(),
            IAName: jQuery('#nameInput').val(),
            IAMarkString: markVal,
            IADocumentID: documentID
        };

        doPostAjax(
            '<%= Url.Generate<ApplicationController>(x => x.SaveIndividualAchievement(null)) %>',
            JSON.stringify(model),
            function (data) {
                if (data.Message != null) {
                    alert(data.Message);
                    return false;
                }
                else {
                    window.location.reload(1);
                }
            });
    }

    jQuery(document).ready(function () {
        for (var i = 0; i < achivements.length; i++)
            doAddRow(jQuery('#trAddNew1'), achivements[i]);
    });
</script>

<gv:EntrantDocumentAddPart ID="EntrantDocumentAddPart1" runat="server" />
<gv:ApplicationWizardButtons runat="server" ApplicationStep="IndividualAchivements" />

