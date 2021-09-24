<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.CompetitiveGroupEditViewModel>" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.ContextExtensions" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Register TagPrefix="gv" TagName="TabControl" Src="~/Views/Shared/Common/InstitutionsTabControl.ascx" %>
<%@ Register TagPrefix="gv" TagName="DirectionInfoPopup" Src="~/Views/Shared/Admission/DirectionInfoPopup.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= Model.GroupID > 0 ? (Model.CanEdit ? "Редактирование" : "Просмотр") : "Добавление"%> Конкурса
</asp:Content>
<asp:Content ID="header" ContentPlaceHolderID="PageTitle" runat="server">Сведения об образовательной организации</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="divstatement">
<gv:tabcontrol runat="server" id="tabControl" />
<style>
	.spOrgUID {
		display: none;
	}
	.targetItemUID {
		width: 50px !important;
	}
	.numericTarget {
		width: 50px !important;
	}
	.gvuzDataGrid td {
		vertical-align: top;
	}
	.orgTh a.btnEdit, .orgTh a.btnDelete, .orgTh .btnDeleteGray {
		margin: 1px;
	}
</style>
<table class="data">
	<tbody>
		<tr>
			<td class="caption"><%= Html.TableLabelFor(x => x.Name) %></td>
			<td><%= Html.TextBoxExFor(m => m.Name) %></td>
		</tr>
		<tr>
			<td class="caption"><%= Html.TableLabelFor(x => x.CampaignID) %></td>
			<td><b><%: Model.CampaignName %></b></td>
		</tr>
<%--
		<tr>
			<td class="caption"><%= Html.TableLabelFor(x => x.EducationalLevelID) %></td>
			<td><b><%: Model.EducationLevelName %></b></td>
		</tr>
--%>
		<tr>
			<td class="caption"><%= Html.TableLabelFor(x => x.CourseID) %></td>
			<td><b><%: CompetitiveGroupExtensions.GetCourseName(Model.CourseID) %></b></td>
		</tr>
		<tr>
			<td class="caption"><%= Html.LabelFor(x => x.Uid) %>:</td>
			<td><%= Html.TextBoxExFor(m => m.Uid)%></td>
		</tr>
	</tbody>
</table>
<div>&nbsp;</div>

<div class="subdivstatement">
<div id="cgSubMenu" class="subsubmenu"></div>

<div class="content">
<div>
	<% if (Model.CanEdit)
	   { %>
	<input type="button" value="Сохранить" id="btnSaveTop" class="button3" />
	<input type="button" value="Отмена" id="btnCancelTop" class="button3" />
	
	<div style="padding-top:10px;padding-bottom: 10px">
		Выбор направлений подготовки:&nbsp;&nbsp;
		<input type="radio" name="dirVariants" id="dirVariant0" <%= Model.DirectionFilterType == 0 ? "checked=\"checked\"" : "" %> /><label for="dirVariant0">Согласно приказу № 1204</label>&nbsp;&nbsp;&nbsp;
		<input type="radio" name="dirVariants" id="dirVariant1" <%= Model.DirectionFilterType == 1 ? "checked=\"checked\"" : "" %> /><label for="dirVariant1">Отбирать направления подготовки по выбранным ВИ</label>&nbsp;&nbsp;&nbsp;
		<input type="radio" name="dirVariants" id="dirVariant2" <%= Model.DirectionFilterType == 2 ? "checked=\"checked\"" : "" %> <%= Model.AllowAnyDirectionsFilterType ? "" : "disabled=\"disabled\" title=\"Есть испытание профильной направленности\"" %> />
		<label for="dirVariant2" <%= Model.AllowAnyDirectionsFilterType ? "" : "disabled=\"disabled\" title=\"Есть испытание профильной направленности\"" %>>Любые направления подготовки</label>&nbsp;&nbsp;&nbsp;
	</div>
	<% } %>
</div>


<table class="gvuzDataGrid tableStatement2" id="tableData">
	<thead>
		<tr>
			<th rowspan="2"><%: Html.LabelFor(x => x.DisplayData.EducationLevel) %></th>
			<th rowspan="2"><%: Html.LabelFor(x => x.DisplayData.Directions) %></th>
			<th rowspan="2"><%: Html.LabelFor(x => x.DisplayData.UID) %></th>
			<th colspan="<%= 3 %>" <%= Model.HasBudget.Values.Any(x => x.CountOOZZ >= 0) ? "" : "style=\"display:none\"" %>><%: Html.LabelFor(x => x.DisplayData.BudgetName) %></th>
			<th colspan="<%= 3 %>" <%= Model.HasBudget.Values.Any(x => x.CountOOZZ >= 0) ? "" : "style=\"display:none\"" %>><%: Html.LabelFor(x => x.DisplayData.QuotaName) %></th>
			<th colspan="<%= 3 %>" <%= Model.HasPaid.Values.Any(x => x.CountOOZZ >= 0) ? "" : "style=\"display:none\"" %>><%: Html.LabelFor(x => x.DisplayData.PaidName) %></th>
			<th colspan="1" id="thTargetCaption"><%: Html.LabelFor(x => x.DisplayData.TargetName) %></th>
			<th style="border:1px solid #ffffff"></th>
		</tr>
		<tr id="trCaptionSecond">
			<th <%= Model.HasBudget.Values.Any(x => x.HasO) || true ? "" : "style=\"display:none\"" %>><%: Html.LabelFor(x => x.DisplayData.NumberBudgetO) %></th>
			<th <%= Model.HasBudget.Values.Any(x => x.HasOZ) || true ? "" : "style=\"display:none\"" %>><%: Html.LabelFor(x => x.DisplayData.NumberBudgetOZ) %></th>
			<th <%= Model.HasBudget.Values.Any(x => x.HasZ) || true ? "" : "style=\"display:none\"" %>><%: Html.LabelFor(x => x.DisplayData.NumberBudgetZ) %></th>
			<th <%= Model.HasBudget.Values.Any(x => x.HasO) || true ? "" : "style=\"display:none\"" %>><%: Html.LabelFor(x => x.DisplayData.NumberQuotaO) %></th>
			<th <%= Model.HasBudget.Values.Any(x => x.HasOZ) || true ? "" : "style=\"display:none\"" %>><%: Html.LabelFor(x => x.DisplayData.NumberQuotaOZ) %></th>
			<th <%= Model.HasBudget.Values.Any(x => x.HasZ) || true ? "" : "style=\"display:none\"" %>><%: Html.LabelFor(x => x.DisplayData.NumberQuotaZ) %></th>
			<th <%= Model.HasPaid.Values.Any(x => x.HasO) || true ? "" : "style=\"display:none\"" %>><%: Html.LabelFor(x => x.DisplayData.NumberPaidO) %></th>
			<th <%= Model.HasPaid.Values.Any(x => x.HasOZ) || true ? "" : "style=\"display:none\"" %>><%: Html.LabelFor(x => x.DisplayData.NumberPaidOZ) %></th>
			<th <%= Model.HasPaid.Values.Any(x => x.HasZ) || true ? "" : "style=\"display:none\"" %>><%: Html.LabelFor(x => x.DisplayData.NumberPaidZ) %></th>
			<th valign="middle" style="background-color:#f0f0f0;border:1px solid #f0f0f0" id="thAddNewOrg"><a href="#" class="add16" title="Добавить организацию целевого приема" id="btnAddNewOrg"></a></th>
			<th style="border:1px solid #ffffff"></th>
		</tr>
	</thead>
	<tbody>
		<tr id="trAddRow">
			<td colspan="8" style="border:1px solid #ffffff"><a href="#" class="add" id="btnAddNewRow" >Добавить специальность</a></td>
			<td style="border:1px solid #ffffff"></td>
		</tr>
	</tbody>
</table>
<div>&nbsp;</div>
<div>
	<% if (Model.CanEdit)
	   { %>
	<input type="button" value="Сохранить" id="btnSave" class="button3" />
	<input type="button" value="Отмена" id="btnCancel" class="button3" />	
	<% } else { %>
		<input type="button" value="Вернуться" id="btnCancel" class="button3" />
	<%} %>
</div>
<div style="display:none">
	<div id="divOrgSelect">
		<table class="data">
			<tbody>
				<tr>
					<td class="caption">Название органа власти или организации:</td>
				</tr>
				<tr>
					<td><input type="text" id="tbNewOrgName" maxlength="250" style="width:400px" /></td>
				</tr>
				<tr>
					<td class="caption">UID:</td>
				</tr>
				<tr>
					<td><input type="text" id="tbNewOrgUID" maxlength="50" style="width:400px" /></td>
				</tr>
			</tbody>
		</table>
	</div>
	<div id="divDirectionSelect">
		<table class="data">
			<tbody>
				<tr>
					<td class="caption">Уровень образования:</td>
					<td><%= Html.DropDownListExFor(x => x.AddEducationLevelID, Model.AllowedEdLevels, new { onchange = "fillAvailableDirections()" })%></td>
				</tr>
				<tr>
					<td colspan="2">
						<div id="divDirectionPlaceHolder" style="max-height: 250px;overflow-y: auto;width: 560px;"></div>
					</td>
				</tr>
			</tbody>
		</table>
	</div>
</div>
</div>
</div>
</div>
<gv:DirectionInfoPopup runat="server" ID="dirPopup" />
<script type="text/javascript">
    
    var cachedDirections = JSON.parse('<%= Html.Serialize(Model.CachedDirections) %>');
    //<%--var allowedDirections = JSON.parse('<%= Html.Serialize(Model.AllowedDirections) %>')--%>
    var allowedEdLevels = JSON.parse('<%= Html.Serialize(Model.AllowedEdLevels) %>')
    var existingOrgs = JSON.parse('<%= Html.Serialize(Model.Organizations) %>')
    var existingRows = JSON.parse('<%= Html.Serialize(Model.Rows) %>')
    

    var orgs = 0
    var orgIncIdx = 0

    var hasBO  = [];
    var hasBOZ = [];
    var hasBZ  = []; 
    var hasPO  = []; 
    var hasPOZ = [];
    var hasPZ  = [];
    var hasTO  = [];
    var hasTOZ = [];
    var hasTZ  = [];


    function addNewOrg(orgName, orgUID, orgID, canDelete)
    {
        orgs++
        orgIncIdx++
        jQuery('#thTargetCaption').attr('colspan', jQuery('#thTargetCaption').attr('colspan') + 1)
        jQuery('#btnAddNewOrg').parent().before('<th class="orgTh"><span class="orgName" orgIdx="' + orgIncIdx  + '" orgID="' + orgID + '"><span class="spOrgName">' + escapeHtml(orgName) + '</span>'
            + '<span class="spOrgUID">' + escapeHtml(orgUID == null ? '' : orgUID) + '</span>'
            + '</span>'
            + '<br/>' +
            '<a href="#" class="btnEdit" onclick="doEditOrg(this);return false"></a> ' +
            (canDelete ? '<a href="#" class="btnDelete" onclick="doDeleteOrg(this);return false"></a>' : '<span class="btnDeleteGray"></span>') +
            '</th>')
        var $trAdd = jQuery('#trAddRow td:first')
        $trAdd.attr('colspan', $trAdd.attr('colspan') + 1)
		
        var genTTb = function (has, text) {
            return '<input type="text" class="' 
                + (has ? 'numeric' : 'view') 
                + ' numericTarget" onchange="valueChanged(this)" maxlength="5" '+(has ? 'value="0"' : 'readonly="readonly"')
                + ' title="' + text + '" />';
        }

        jQuery('#tableData tbody tr[dirID]').each(function () 
        {
            var edLevelID = $(this).attr('edID');
            jQuery(this).children('td:last').prev().before('<td align="left" nowrap="nowrap" orgIdx="' + orgIncIdx + '">'
                + genTTb(hasTO[edLevelID], '<%: Html.LabelTextFor(x => x.DisplayData.NumberBudgetO) %>')
                + genTTb(hasTOZ[edLevelID], '<%: Html.LabelTextFor(x => x.DisplayData.NumberBudgetOZ) %>')
                + genTTb(hasTZ[edLevelID], '<%: Html.LabelTextFor(x => x.DisplayData.NumberBudgetZ) %>')
                +'<br/> UID: <input type="text" class="targetItemUID" maxlength="50" value="" /></td>') 
        })
    }

    function doDeleteOrg(el)
    {
        var $orgName = jQuery(el).siblings('.orgName')
        var orgIdx = $orgName.attr('orgIdx')
        orgs--
        jQuery('#thTargetCaption').attr('colspan', jQuery('#thTargetCaption').attr('colspan') - 1)
        var $trAdd = jQuery('#trAddRow td:first')
        $trAdd.attr('colspan', $trAdd.attr('colspan') - 1)
        jQuery('#tableData tbody tr[dirID]').each(function () { jQuery(this).children('td[orgIdx="' + orgIdx + '"]').remove().detach() })
        $orgName.parents('th:first').remove().detach()
        isSomethingChanged = true;
    }

    function doEditOrg(el)
    {
        var $orgName = jQuery(el).siblings('.orgName')
        showAddEditOrgDialog($orgName.attr('orgID'), unescapeHtml($orgName.find('.spOrgName').html()), unescapeHtml($orgName.find('.spOrgUID').html()), function (orgName, orgUID) {
            $orgName.html('<span class="spOrgName">' + escapeHtml(orgName) + '</span><span class="spOrgUID">' + orgUID + '</span>')
        })
    }

    function valueChanged(el)
    {
        var v = el.value;
        var n = new Number(v)
        if(isNaN(n) || n < 0 || n > 99999 || v.replace(/\s+/g, '') == '')
        {
            jQuery(el).attr('invalidv', '1')
            addValidationError(jQuery(el), "")
        }
        else
        {
            jQuery(el).removeAttr('invalidv')
            clearValidationErrors(jQuery(el).parent())
        }
    }


    function doAddRow(directionID, edLevelID)
    {
        var selDirName = ''
        //jQuery.each(allowedDirections, function () { if (this.ID == directionID) { selDirName = this.Name; return false } })
        selDirName = cachedDirections[directionID];
        if(!selDirName) selDirName = '';
        else selDirName = selDirName.DirectionName; // + ' (' + (selDirName.DirectionCode == null ? '' : selDirName.DirectionCode.trim() + '.' + selDirName.QualificationCode.trim()) + '/' + (selDirName.NewCode == null ? '' : selDirName.NewCode.trim()) + ')';
		
        var selEdName = ''
        jQuery.each(allowedEdLevels, function () { if (this.ID == edLevelID) { selEdName = this.Name; return false } })

        var className = jQuery('#trAddRow').prev().attr('class')
        if(className == 'trline1') className = 'trline2'; else className = 'trline1';
        var newRow = '<tr dirID="' + directionID + '" edID="' + edLevelID + '" class="' + className + '">'
            +'<td>' + selEdName + '</td>'
            +'<td><span onmouseout="hideDirectionDetails()" onmouseover="viewDirectionDetails(this, ' + directionID + ')">' + selDirName + '</span></td>'
        newRow += '<td><input type="text" class="numeric" maxlength="50" value="" uidfield="1" /></td>'
        for (var i = 0; i < 9; i++) {
            var isVisible = false;
            if(hasBO[edLevelID] && (i == 0)) isVisible = true;
            if(hasBOZ[edLevelID] && (i == 1)) isVisible = true;
            if(hasBZ[edLevelID] && (i == 2)) isVisible = true;
            if(hasPO[edLevelID] && (i == 6)) isVisible = true;
            if(hasPOZ[edLevelID] && (i == 7)) isVisible = true;
            if(hasPZ[edLevelID] && (i == 8)) isVisible = true;
            if(hasBO[edLevelID] && (i == 3) && (edLevelID == 2 || edLevelID == 5 || edLevelID == 3 || edLevelID == 19)) isVisible = true;
            if(hasBOZ[edLevelID] && (i == 4) && (edLevelID == 2 || edLevelID == 5 || edLevelID == 3 || edLevelID == 19)) isVisible = true;
            if(hasBZ[edLevelID] && (i == 5) && (edLevelID == 2 || edLevelID == 5 || edLevelID == 3 || edLevelID == 19)) isVisible = true;
            newRow += '<td align="center"><input type="text" class="'+ (isVisible ? 'numeric' : 'view') + '" onchange="valueChanged(this)" maxlength="5" ' + (isVisible ? 'value="0"' : 'readonly="readonly"') + ' /></td>'
        }

        var $orgNames = jQuery('#trCaptionSecond th span.orgName')

        var genTTb = function (has, text) {
            return '<input type="text" class="' 
                + (has ? 'numeric' : 'view') 
                + ' numericTarget" onchange="valueChanged(this)" maxlength="5" '+(has ? 'value="0"' : 'readonly="readonly"')
                + ' title="' + text + '" />';
        }

        for (var i = 0; i < orgs; i++) {
            newRow += '<td align="left" nowrap="nowrap" orgIdx="' + jQuery($orgNames[i]).attr('orgIdx') + '">'
                + genTTb(hasTO[edLevelID], '<%: Html.LabelTextFor(x => x.DisplayData.NumberBudgetO) %>')
                + genTTb(hasTOZ[edLevelID], '<%: Html.LabelTextFor(x => x.DisplayData.NumberBudgetOZ) %>')
                + genTTb(hasTZ[edLevelID], '<%: Html.LabelTextFor(x => x.DisplayData.NumberBudgetZ) %>')
                +'<br/> UID: <input type="text" class="targetItemUID" maxlength="50" value="" /></td>'
        }
        newRow += '<td style="background-color:#f0f0f0;border:1px solid #f0f0f0"></td>' +
            '<td align="center" style="border:1px solid #ffffff"><a href="#" class="btnDelete" onclick="doDeleteRow(this);return false"></a></td></tr>'
        jQuery('#trAddRow').before(newRow)
        return false
    }

    function getDataAndAddRow(directionID, edLevelID)
    {
        doPostAjax('<%= Url.Generate<AdmissionController>(x => x.CompetitiveGroupGetCount(null, null, null)) %>',
            "groupID=<%=Model.GroupID %>&directionID=" + directionID + "&edLevelID=" + edLevelID,
            function(data)
            {
                if(!addValidationErrorsFromServerResponse(data))
                {
                    doAddRow(directionID, edLevelID)
                    jQuery('#tableData tbody tr[dirID="' + directionID + '"][edID="' + edLevelID + '"]').find('input[type="text"]:not([uidfield])').each(function(idx, el) 
                    {
                        if(idx < 6 && !jQuery(el).is('[readonly]'))
                            jQuery(el).val(data.Data[idx]) 
                    })
                }
            }, "application/x-www-form-urlencoded")
        //jQuery('#divDirectionSelect').dialog('close')
        return false
    }

    function doDeleteRow(el)
    {
        var directionID = jQuery(el).parents('tr:first').attr('dirID')
        var edLevelID = jQuery(el).parents('tr:first').attr('edID')
        var existingDirections = []
        jQuery('#tableData tbody tr[dirID]').each(function () { existingDirections.push(jQuery(this).attr('dirID')) })
        doPostAjax('<%= Url.Generate<AdmissionController>(x => x.CanDeleteCompetitiveGroupDirection(null, null, null, null)) %>',
            "groupID=<%=Model.GroupID %>&directionID=" + directionID + "&dirCount=" + existingDirections.length + "&edLevelID=" + edLevelID,
            function(data)
            {
                if(!addValidationErrorsFromServerResponse(data))
                {
                    jQuery(el).parents('tr:first').remove().detach()
                }
            }, "application/x-www-form-urlencoded")
        return false
    }

    var tmpID = 0;
	
    function showAddDirectionDialog() {
        jQuery('#divDirectionSelect').dialog(
            {
                resizeable: false,
                title: 'Выберите специальность',
                width: 600,
                modal: true,
                buttons: {
                    "Добавить": function () {
                        jQuery('.dirCb:checked').each(function (i,e) {
                            var dirID = jQuery(e).attr('dirID');
                            var edID = jQuery(e).attr('edID');
                            getDataAndAddRow(dirID, edID);
                        })
                        jQuery('#divDirectionSelect').dialog('close');
                    },
                    "Отмена": function() {jQuery('#divDirectionSelect').dialog('close');}
                }
            });
        fillAvailableDirections()
    }
	
    function fillAvailableDirections() {
        var edLevelID = jQuery("#AddEducationLevelID").val();
        var selType = -1;
        if(jQuery('#dirVariant0').is(':checked')) selType = 0;
        if(jQuery('#dirVariant1').is(':checked')) selType = 1;
        if(jQuery('#dirVariant2').is(':checked')) selType = 2;
        if(selType == -1) {
            selType = 0;
            jQuery('#dirVariant0').attr('checked', 'checked');
        }
        var existingDirections = []
        jQuery('#tableData tbody tr[dirID][edID="' + edLevelID + '"]').each(function () { existingDirections.push(jQuery(this).attr('dirID')) })

        var model = {
            CompetitiveGroupID: <%= Model.GroupID %>,
            EducationLevelID: edLevelID,
            FilterType: selType,
            SelectedDirections: existingDirections
        }
        jQuery("#divDirectionPlaceHolder").html('Доступные специальности загружаются');
        doPostAjax('<%= Url.Generate<AdmissionController>(x => x.GetAvailableDirectionsForCompetitiveGroup(null)) %>', JSON.stringify(model),
            function(data) {
                if (addValidationErrorsFromServerResponse(data, false))
                    return false;
                var availableDirections = data.Data;
                if (availableDirections.length == 0) 
                {
                    jQuery("#divDirectionPlaceHolder").html('Все доступные специальности уже добавлены');
                    return false
                }
                var res = ''
                jQuery.each(availableDirections, function() {
                    var curID = tmpID++;
                    res += '<div><input type="checkbox" dirID="' + this.ID + '" edID="' + edLevelID + '" id="' + curID + '" class="dirCb"/>'
                        + '<label for="' + curID + '" onmouseout="hideDirectionDetails()" onmouseover="viewDirectionDetails(this, ' + this.ID + ')">' + (this.Code == null ? '' : this.Code.trim() + '.' + this.QualificationCode.trim()) + '/' + (this.NewCode == null ? '' : this.NewCode.trim()) + ' ' + this.Name + '</label></div>';
                    //	+ this.Code + ' ' + this.Name + '</a></div>';
                    //res += '<div style="padding: 3px 0px 3px 0px;"><a href="" onclick="getDataAndAddRow(' + this.ID + ', ' + edLevelID + ');return false;">' 
                    //	+ this.Code + ' ' + this.Name + '</a></div>';
                })
                jQuery("#divDirectionPlaceHolder").html(res);
            });
    }

    jQuery('#btnAddNewRow').click(function ()
    {
        showAddDirectionDialog();
        return false;
    })

    function doSelectDirectionsType(el) {
        var $div = $(el).parent()
        if($(el).is(':checked')) {
            $div.find('div[type="ne"]').show();
            $div.find('div[type="e"]').hide();
        }
        else {
            $div.find('div[type="e"]').show();
            $div.find('div[type="ne"]').hide();
        }
    }
	
    function showAddEditOrgDialog(orgID, currentOrgName, orgUID, callback)
    {
        jQuery('#tbNewOrgName').val(currentOrgName)
        jQuery('#tbNewOrgUID').val(orgUID)
        jQuery('#divOrgSelect').dialog(
            {
                resizeable: false,
                title: 'Целевой прием',
                width: 450,					
                modal: true,
                buttons:
                    {
                        "Сохранить": function ()
                        {
                            var orgName = jQuery('#tbNewOrgName').val()
                            if (orgName == '')
                                addValidationError(jQuery('#tbNewOrgName'), 'Название не может быть пустым', true)
                            else
                            {
                                var m = { CompetitiveGroupID: <%= Model.GroupID%>, org: {
	                                ID: orgID,
	                                Name: orgName,
	                                UID: jQuery('#tbNewOrgUID').val()
                                } };
                                clearValidationErrors(jQuery('#divOrgSelect'))
                                doPostAjax('<%= Url.Generate<AdmissionController>(x => x.CompetitiveGroupCheckTargetOrganizationsUnique(null, null)) %>', JSON.stringify(m), function(data) {
	                                if (data.IsError) {
                                        addValidationErrorsFromServerResponse(data)
	                                } else {
	                                    clearValidationErrors(jQuery('#divOrgSelect'))
	                                    callback(jQuery('#tbNewOrgName').val(), jQuery('#tbNewOrgUID').val())
	                                    closeDialog(jQuery('#divOrgSelect'))
	                                }
                                });
                            }
                        },
                        "Отмена": function () { closeDialog(jQuery('#divOrgSelect')) }
                    }
            })
    }

    function doCancel(navUrl)
    {
        window.location =  !!navUrl ? navUrl : '<%= Url.Generate<AdmissionController>(x => x.CompetitiveGroupList(null, null, null, null, null, null, null)) %>' + '?fCampaignID=' + '<%= Model.FilterCampaign %>';
    }

    function doSubmit(navUrl)
    {
        if (jQuery('#tableData').find('.input-validation-error').length > 0)
        {
            jQuery('#tableData tr:has(.input-validation-error) td:first-child').each(function() {
                if(jQuery(this).children('.field-validation-error').length == 0)
                    jQuery(this).append('<span class="field-validation-error"><br/>Неверное количество мест</span>') 
            })
            //alert('Для данных специальностей есть ошибки в данных: \r\n' + spError)
        }
        if(jQuery('#tableData input[invalidv]').length > 0)
            return false
        jQuery('#tableData .field-validation-error').remove().detach()
        var dirFilterType = 0;
        if(jQuery('#dirVariant0').is(':checked')) dirFilterType = 0;
        if(jQuery('#dirVariant1').is(':checked')) dirFilterType = 1;
        if(jQuery('#dirVariant2').is(':checked')) dirFilterType = 2;
		
        var model =
            {
                GroupID: <%= Model.GroupID %>,
                Name: jQuery('#Name').val(),
                Uid: jQuery('#Uid').val(),
                DirectionFilterType: dirFilterType,
                Organizations: [],
                Rows: []
            }

        jQuery('#tableData tbody tr[dirID]').each(function () 
        {
            var row = {
                DirectionID: jQuery(this).attr('dirID'),
                EducationLevelID: jQuery(this).attr('edID'),
                UID: jQuery(this).find('input[type="text"][uidfield]').val(),
                Data: [],
                DataTargetUIDs: []
            }
            jQuery(this).find('input[type="text"]:not([uidfield]):not(.targetItemUID)').each(function() { row.Data.push(jQuery(this).val()) })
            jQuery(this).find('input[type="text"].targetItemUID').each(function() { row.DataTargetUIDs.push(jQuery(this).val()) })
            model.Rows.push(row)
        })
        jQuery('#trCaptionSecond th span.orgName').each(function() {
            model.Organizations.push({
                ID: jQuery(this).attr('orgID'),
                Name: unescapeHtml(jQuery(this).find('.spOrgName').html()),
                UID: unescapeHtml(jQuery(this).find('.spOrgUID').html())
            })
        })
        clearValidationErrors(jQuery('.data'))
        doPostAjax('<%= Url.Generate<AdmissionController>(x => x.CompetitiveGroupSave(null)) %>', JSON.stringify(model), function (data)
        {
            if(data.IsError && data.Message == 'df') //<%-- красивое подсвечивание ошибок --%>
            {
                var errorsList = data.Data.Errors;
                for(var i = 0; i < errorsList.length; i++)
                {
                    jQuery('#tableData tr[dirID="' + errorsList[i].DirectionID + '"][edID="' + errorsList[i].EducationLevelID + '"] td:first-child').append('<span class="field-validation-error"><br/>' + errorsList[i].Error.replace(/\n/g, '<br/>') + '</span>')
                    var $iArr = jQuery('#tableData tr[dirID="' + errorsList[i].DirectionID + '"][edID="' + errorsList[i].EducationLevelID + '"] input:not(.targetItemUID)')
                    for(var k = 0; k < errorsList[i].ErrorIdx.length;k++)
                    {
                        var idx = errorsList[i].ErrorIdx[k] + 1
                        if(idx >= 6 + 1)
                        {
                            for(var l = idx; l < $iArr.length;l+=3)
                                addValidationError(jQuery($iArr[l]), '')
                        }
                        else
                            addValidationError(jQuery($iArr[idx]), '')
                    }
                }
                var targetUIDErrors = data.Data.TargetUIDErrors;
                for(var i = 0; i < targetUIDErrors.length; i++) {
                    jQuery('#tableData tr[dirID="' + targetUIDErrors[i].DirectionID + '"][edID="' + errorsList[i].EducationLevelID + '"] td:first-child').append('<span class="field-validation-error"><br/>' + targetUIDErrors[i].Error.replace(/\n/g, '<br/>') + '</span>')
                    var $iArr2 = jQuery('#tableData tr[dirID="' + targetUIDErrors[i].DirectionID + '"][edID="' + errorsList[i].EducationLevelID + '"] input:.targetItemUID')
                    for(var k = 0; k < targetUIDErrors[i].ErrorIdx.length;k++)
                    {
                        var idx = targetUIDErrors[i].ErrorIdx[k]
                        addValidationError(jQuery($iArr2[idx]), '')
                    }
                }
				
                return
            }
            if (!addValidationErrorsFromServerResponse(data, false))
            {
                doCancel(navUrl)
            }
        })
    }

    function fillData()
    {
        jQuery.each(existingOrgs, function() { addNewOrg(this.Name, this.UID, this.ID, this.CanDelete) })
        jQuery.each(existingRows, function()
        {
            var row = this
            doAddRow(row.DirectionID, row.EducationLevelID)
            var $sel = jQuery('#tableData tbody tr[dirID="' + row.DirectionID + '"][edID="' + row.EducationLevelID + '"]');
            $sel.find('input[type="text"][uidfield]').val(row.UID != null ? row.UID : '');
            $sel.find('input[type="text"]:not([uidfield]):not(.targetItemUID)')
                .each(function(idx, el) { if(!jQuery(el).is('[readonly]')) jQuery(el).val(row.Data[idx]) })
            $sel.find('input[type="text"].targetItemUID')
                .each(function(idx, el) { if(!jQuery(el).is('[readonly]')) jQuery(el).val(row.DataTargetUIDs[idx] != null ? row.DataTargetUIDs[idx] : '') })
        })
    }

    jQuery('#btnAddNewOrg').click(function ()
    {
        showAddEditOrgDialog(0, '', '', function (orgName, orgUID) { addNewOrg(orgName, orgUID, 0, true) })
        return false
    })

    jQuery('#btnSave,#btnSaveTop').click(function ()
    {
        doSubmit()
        return false
    })

    jQuery('#btnCancel,#btnCancelTop').click(function ()
    {
        doCancel()
    })
	
    jQuery(document).ready(function()
    {
        new TabControl(jQuery('#cgSubMenu'), [{ name: 'Специальности', link: '<%= Url.Generate<AdmissionController>(c => c.CompetitiveGroupEdit(Model.GroupID, Model.CampaignID)) %>', enable: true }
            //<% if(Model.GroupID > 0) { %>
		
            ,{ name: 'Вступительные испытания (<%= Model.EntranceTestCount %>)', link: '<%= Url.Generate<AdmissionController>(c => c.CompetitiveGroupEntranceTestEdit(Model.GroupID)) %>', enable: true}
            //<%} %>
        ])
            .init();
        fillData()
        <% if(!Model.CanEdit)
		   { %>
        jQuery('.content input').addClass('view').attr('readonly', 'readonly')
        jQuery('#trAddRow,#btnAddNewOrg').hide();
        jQuery('.content .btnDelete,.content .btnDeleteGray,.content .btnEdit').remove().detach();
        <% } %>

    })
	
    function doClickOuterElement()
    {
        if(!isSomethingChanged)
            return true;
        var $el = jQuery(this)
        jQuery('<div>Вы хотите сохранить данные перед уходом с данной страницы?</div>').dialog({
            width: '400px',
            modal: true,
            buttons: {
                "Сохранить": function() {
                    doSubmit($el.attr('href'));
                    closeDialog(jQuery(this))
                },
                "Не сохранять": function() {
                    window.location = $el.attr('href');
                    closeDialog(jQuery(this))
                },
                "Отмена": function() { closeDialog(jQuery(this)) }
            }
        })
        /*if(confirm('Вы хотите сохранить заявление перед уходом с данной страницы?'))
		{
			$outerClickedElement = jQuery(this)
			doSubmit('save')
			return false
		}
		return true*/
        return false
    }
    var isSomethingChanged = false
    jQuery(document).ready(function() {
        jQuery('a.menuitemr,a.menuitemr1,a.menuiteml,a.menuiteml1').click(doClickOuterElement)
        jQuery('input').change(function () { isSomethingChanged = true;})
        jQuery('.gvuzDataGrid a:not(.btnDelete)').click(function () { isSomethingChanged = true;})
		
        <% if((!Model.CanEdit) || GVUZ.Helper.UrlUtils.IsReadOnly(FBDUserSubroles.CampaignDirection))
		   { %>
        jQuery('.content input').addClass('view').attr('readonly', 'readonly')
        <% } %>
        if(!hasTO && !hasTOZ && !hasTZ) {
            jQuery("#thTargetCaption").html('')
            jQuery("#btnAddNewOrg").hide()
			
        }
    })

    function setAllowedForms() 
    {
        <% foreach (var oozzInfo in Model.HasBudget) { %> hasBO[<%= oozzInfo.Key %>] = <%= oozzInfo.Value.HasO ? "1" : "0" %>;<%  } %>
        <% foreach (var oozzInfo in Model.HasBudget) { %> hasBOZ[<%= oozzInfo.Key %>] = <%= oozzInfo.Value.HasOZ ? "1" : "0" %>;<%  } %>
        <% foreach (var oozzInfo in Model.HasBudget) { %> hasBZ[<%= oozzInfo.Key %>] = <%= oozzInfo.Value.HasZ ? "1" : "0" %>;<%  } %>
        <% foreach (var oozzInfo in Model.HasPaid) { %> hasPO[<%= oozzInfo.Key %>] = <%= oozzInfo.Value.HasO ? "1" : "0" %>;<%  } %>
        <% foreach (var oozzInfo in Model.HasPaid) { %> hasPOZ[<%= oozzInfo.Key %>] = <%= oozzInfo.Value.HasOZ ? "1" : "0" %>;<%  } %>
        <% foreach (var oozzInfo in Model.HasPaid) { %> hasPZ[<%= oozzInfo.Key %>] = <%= oozzInfo.Value.HasZ ? "1" : "0" %>;<%  } %>
        <% foreach (var oozzInfo in Model.HasTarget) { %> hasTO[<%= oozzInfo.Key %>] = <%= oozzInfo.Value.HasO ? "1" : "0" %>;<%  } %>
        <% foreach (var oozzInfo in Model.HasTarget) { %> hasTOZ[<%= oozzInfo.Key %>] = <%= oozzInfo.Value.HasOZ ? "1" : "0" %>;<%  } %>
        <% foreach (var oozzInfo in Model.HasTarget) { %> hasTZ[<%= oozzInfo.Key %>] = <%= oozzInfo.Value.HasZ ? "1" : "0" %>;<%  } %>
    }
    setAllowedForms()
    menuItems[5].selected = true;
</script>
</asp:Content>
