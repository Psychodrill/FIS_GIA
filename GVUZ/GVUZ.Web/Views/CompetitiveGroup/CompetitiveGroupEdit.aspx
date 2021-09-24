<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.DAL.Dapper.ViewModel.CompetitiveGroups.CompetitiveGroupViewModel>" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.ContextExtensions" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Controllers.Admission" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.DAL.Dapper.ViewModel.CompetitiveGroups" %>
<%@ Register TagPrefix="gv" TagName="TabControl" Src="~/Views/Shared/Common/InstitutionsTabControl.ascx" %>
<%@ Register TagPrefix="gv" TagName="DirectionInfoPopup" Src="~/Views/Shared/Admission/DirectionInfoPopup.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= Model.CompetitiveGroupEdit.CompetitiveGroupID > 0 ? (Model.CompetitiveGroupEdit.CanEdit ? "Редактирование" : "Просмотр") : "Добавление"%> Конкурса
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
    .olympic td {
        vertical-align: middle;
        padding: 0px;
        margin: 0px;
        width: 50px;
    }
    .olympic td input {
        height: auto;
        width: auto;
    }
</style>
<table  class="tableAdmin">
	<tbody>
        <tr>
            <td class="caption"><%= Html.TableLabelFor(x => x.CompetitiveGroupEdit.CompetitiveGroupName) %></td>
			<td><%= Html.TextBoxExFor(m => m.CompetitiveGroupEdit.CompetitiveGroupName, new {style = "width: 326px"}) %></td>
			
            <td class="caption"><%= Html.LabelFor(x => x.CompetitiveGroupEdit.Uid) %>:</td>
			<td><%= Html.TextBoxExFor(m => m.CompetitiveGroupEdit.Uid, new {style = "width: 326px"})%></td>
		</tr>

        <tr>
			<td class="caption"><%= Html.LabelFor(x => x.CompetitiveGroupEdit.CampaignYearStart) %>:</td>
            <td><%= Html.DropDownListExFor(x => x.CompetitiveGroupEdit.CampaignYearStart, Model.CampaignStartYears, new {}) %></td>
            
            <td class="caption"><%= Html.LabelFor(x => x.CompetitiveGroupEdit.CampaignID) %>:</td>
            <td>
                <div id="CGE_Campaigns"></div>
            </td>
		</tr>

        <tr>			
            <td class="caption"><%= Html.LabelFor(x => x.CompetitiveGroupEdit.EducationLevelID) %>:</td>
            <td>
                <div id="CGE_EducationLevels"></div>
            </td>
		
            <td class="caption"><%= Html.LabelFor(x => x.CompetitiveGroupEdit.IsFromKrym) %>:</td>
            <td colspan="1">
                <input type="checkbox" id="IsFromKrym" <%= (Model.CompetitiveGroupEdit.IsFromKrym) ? "checked=\"checked\"" : ""%>/>
            </td>
        </tr>
        <tr>			
            <td class="caption"><%= Html.LabelFor(x => x.CompetitiveGroupEdit.EducationSourceID) %>:</td>
            <td><%= Html.DropDownListExFor(x => x.CompetitiveGroupEdit.EducationSourceID, Model.EducationFinanceSources, new {}) %></td>

            <td class="caption"><%= Html.LabelFor(x => x.CompetitiveGroupEdit.IsAdditional) %>:</td>
            <td colspan="1">
                <input type="checkbox" id="IsAdditional" <%= (Model.CompetitiveGroupEdit.IsAdditional) ? "checked=\"checked\"" : "" %> />
            </td>
        </tr>
        <tr>			
            <td class="caption"><%= Html.LabelFor(x => x.CompetitiveGroupEdit.EducationFormID) %>:</td>
            <td>
                <div id="CGE_EducationForms"></div>
            </td>
            
            <td class="caption"><%= Html.LabelFor(x => x.CompetitiveGroupEdit.IdLevelBudget) %>:</td>
            <td colspan="1">
                <div id="CGE_LevelBudgets"></div>
            </td>
        </tr>

        <tr>
            <% if (Model.IsMultiProfile)
                { %>
            
            <td class="caption"><%= Html.LabelFor(x => x.CompetitiveGroupEdit.ParentDirectionID) %>:</td>  
                     
            <% 
            }
            else
            {
                %> 
            <td class="caption"><%= Html.LabelFor(x => x.CompetitiveGroupEdit.DirectionID) %>:</td>
            <%} %>

            <td>
                <div id="CGE_Directions"></div>
            </td>
             

            <td class="caption"><%= Html.LabelFor(x => x.CompetitiveGroupEdit.Value) %>:</td>
            <td><%= Html.TextBoxExFor(m => m.CompetitiveGroupEdit.Value, new { @class = "numeric", style = "width: 326px" }) %></td>
        </tr>
         <tr>
            
            <td class="caption"><%= Html.TableLabelReqFor(x => x.CompetitiveGroupEdit.StudyBeginningDate) %>:</td>

            <td colspan="1"> <%= Html.DatePickerFor(m => m.CompetitiveGroupEdit.StudyBeginningDate) %></td>

            <td class="caption"><%= Html.LabelFor(x => x.CompetitiveGroupEdit.StudyPeriod) %>:</td>

            <td><%= Html.TextBoxExFor(m => m.CompetitiveGroupEdit.StudyPeriod, new { @class = "numeric", style = "width: 326px" }) %></td>
          

        </tr>

         <tr>

             <td class="caption"><%= Html.TableLabelReqFor(x => x.CompetitiveGroupEdit.StudyEndingDate) %>:</td>

            <td colspan="1"><%= Html.DatePickerFor(m => m.CompetitiveGroupEdit.StudyEndingDate) %></td>


        </tr>
		
	</tbody>
</table>
<div id="commonErrors">&nbsp;</div>

<div class="subdivstatement" style="margin-top: 50px; border-top: #c4c4c4 2px solid;">
<div id="cgSubMenu" class="subsubmenu"></div>

<div id="tab0" style="display:block;">
    <h4>Образовательные программы</h4>
    <table class="gvuzDataGrid tableStatement" cellpadding="3" id="tablePrograms">
	    <thead>
		    <tr>
			    <th style="width:80%"><%= Html.LabelFor(x => x.CompetitiveGroupProgramsEdit.Program) %></th>
<%--			<th style="width:30%"><%= Html.LabelFor(x => x.CompetitiveGroupProgramsEdit.Name) %></th>
			    <th style="width:30%"><%= Html.LabelFor(x => x.CompetitiveGroupProgramsEdit.UID) %></th>--%>
			    <th style="width:40px"></th>
		    </tr>
	    </thead>
	    <tbody>
		    <tr id="trAddNewProgram">
			    <td colspan="4">
				    <a href="#" id="btnAddNewProgram" class="button">Добавить</a>
			    </td>
		    </tr>
	    </tbody>
    </table>

    <script type="text/javascript">
        this.programs = JSON.parse('<%= Html.Serialize(Model.CompetitiveGroupProgramsEdit.Programs) %>');
        if (programs == null) programs = [];

        //for new program
        var ProgramID = null;
        var Program = null;

        for(var i = 0; i < programs.length; i++)
        {
            createdSavedProgram(jQuery('#trAddNewProgram'), programs[i]);
        }

        function createdSavedProgram($trBefore, itemData)
        {
            var className = $trBefore.prev().attr('class')
            if(className == 'trline1') className = 'trline2'; else className = 'trline1';
            $trBefore.before('<tr itemID="' + itemData.ProgramID + '" class="' + className + '">' 
                + '<td>' + escapeHtml(itemData.Program) + '</td>' 
                //+ '<td>' + escapeHtml(itemData.Name) + '</td>' 
                //+ '<td>' + escapeHtml(itemData.UID) + '</td>'
                
                + '<td align="center" nowrap="nowrap">'
                + '<a href="#" class="btnEdit" onclick="programEditButton(this);return false;">' 
                + '</a>&nbsp;' 
                + (true //itemData.CanRemove 
                        ? ('<a href="#" class="btnDelete Program" onclick="programDeleteButton(this);return false;"></a>') 
                        : ('<span class="btnDeleteGray"></span>'))
                + '</td></tr>');
        };


        jQuery('#btnAddNewProgram').click(function() {addNewProgram(this, null, null);return false;})
        
        var addEditProgramRow = function($trToAdd, itemID)
        {
            itemID = itemID ? itemID : "";
            if(itemID == null) itemID = ''; //possible fix of strange issue
            var className = $trToAdd.prev().attr('class')
            if(className == 'trline1') className = 'trline2'; else className = 'trline1';
            //var etString = '<input type="text" class="entranceTypeText" maxLength="100" />';
            $trToAdd.before('<tr itemID="' + (itemID) + '"  class="' + className + '">' // <td>' + etString + '</td>' 
            //+ '<td><input type="text" class="program"/></td>' 
            + '<td><div class="ui-widget">'                 
            +      '<input type="text" id="search" class="program">'
            +  '</div></td>'
            //+ '<td align="center"><input type="text" class="programCode" maxlength="10" /></td>'
            //+ '<td align="center"><input type="text" class="programName" maxlength="200" /></td>' 
            //+ '<td align="center"><input type="text" class="programUID" maxlength="200" /></td>' 
            + '<td align="center" nowrap="nowrap">'
            + '<a href="#" style="margin-top: 10px;" class="btnSave" onclick="programSaveButton(this);return false;"></a> ' 
            + '<a href="#" style="margin-bottom: 5px; margin-left: 5px;" class="btnCancel" onclick="programDeleteUButton(this);return false;"></a>'
            + '</td>'
            + '</tr>');
        };

        var addNewProgram = function(el, nonChangeName, itemID){

            jQuery('.btnDeleteU.Program').click();
            addEditProgramRow(jQuery(el).parents('tr'), itemID);

            $("#search").autocomplete({  
                //source: InstitutionPrograms,
                source: function(req, response) {
                    var results = $.ui.autocomplete.filter(InstitutionPrograms, req.term);
                    response(results);
                },
                minLength: 0,
                select: function( event, ui ) {
                    $( "#search" ).val( ui.item.label); 
                    //alert(ui.item.value);
                    ProgramID = ui.item.value;
                    Program = ui.item.label;

                    return false;
                }
            }).focus(function () {
                if ($(this).val().length == 0) {
                    $(this).autocomplete("search");
                }
            });

            jQuery(el).parents('tr:first').hide();
            return false;
        }

        function programEditButton (el)
	    {
		    var $tr = jQuery(el).parents('tr:first');
		    var child = $tr.children('td')
		    var canChange = $tr.find('.btnDeleteGray').length == 0 /*&& $tr.parents('#tableProfile').length == 0*/
		    addNewProgram(el, canChange ? null : child[0].innerHTML, $tr.attr('itemID'))
		    jQuery('.program').val(unescapeHtml(child[0].innerHTML))
		    //jQuery('.programName').val(unescapeHtml(child[1].innerHTML))
		    //jQuery('.programUID').val(unescapeHtml(child[2].innerHTML))
		    $hiddenTrEdited = $tr;
		    $tr.hide();
		    return false;
	    }

	    function programDeleteButton (el)
	    {
		    var $tr = jQuery(el).parents('tr');
		    confirmDialog('Вы действительно хотите удалить образовательную программу?', function () {
		        
		        var row = $tr[0].rowIndex;
		        document.getElementById("tablePrograms").deleteRow(row);
		        this.programs.splice(row - 1, 1);

		        tabControl.menuItems[0].name = TabName(0);
		        tabControl.init();
		    });


	        return false;
	    }

        function programSaveButton(el)
        {
            var $tr = jQuery(el).parents('tr');
            var row = $tr[0].rowIndex;
            clearValidationErrors($tr);
            var isError = false;

            //var program = jQuery('.program').val();
            //var name = jQuery('.programName').val();
            if (!Program || Program=='')
            {
                jQuery('.program').removeClass('input-validation-error-fixed').addClass('input-validation-error');
                jQuery('.program').after('<span class="field-validation-error"><br/>Необходимо выбрать программу!</span>');
                isError = true;
            }

            for(var j = 0; j<this.programs.length; j++)
            {
                if (this.programs[j].ProgramID == ProgramID && j != row - 1)
                {
                    jQuery('.program').removeClass('input-validation-error-fixed').addClass('input-validation-error');
                    jQuery('.program').after('<span class="field-validation-error"><br/>Нельзя выбрать одну и ту же образовательную программу более 1 раза!</span>');
                    isError = true;
                }
            }
            //var uid = jQuery('.programUID').val();
            //for(var j = 0; j<this.programs.length; j++)
            //{
            //    if (this.programs[j].Name == name && j != row - 1)
            //    {
            //        jQuery('.programName').removeClass('input-validation-error-fixed').addClass('input-validation-error');
            //        jQuery('.programName').after('<span class="field-validation-error"><br/>Наименование должно быть уникальным!</span>');
            //        isError = true;                    
            //    }
            //    if (uid != '' && this.programs[j].UID == uid && j != row - 1)
            //    {
            //        jQuery('.programUID').removeClass('input-validation-error-fixed').addClass('input-validation-error');
            //        jQuery('.programUID').after('<span class="field-validation-error"><br/>UID должен быть уникальным в рамках ОО</span>');
            //        isError = true;
            //    }
            //}
            if(isError) 
                return false;

            if (!ProgramID) return false;

            var model =
			    {
			        ProgramID: ProgramID,
			        Program: Program
			        <%--CompetitiveGroupID: <%= Model.CompetitiveGroupEdit.CompetitiveGroupID %>,
			        Code : jQuery('.programCode').val(), 
			        Name : jQuery('.programName').val(), 
			        UID : jQuery('.programUID').val()--%>
			    };

            
            //document.getElementById("tablePrograms").deleteRow(row);
            this.programs.splice(row - 1, 1, model);

            //this.programs.push(model);
            if($hiddenTrEdited) $hiddenTrEdited.remove().detach();
            jQuery('#trAddNewProgram').show();
            createdSavedProgram($tr, model);
            $tr.remove().detach();

            tabControl.menuItems[0].name = TabName(0);
            tabControl.init();
            
            ProgramID = null;
            Program = null;

            return false;
        }

        function programDeleteUButton (el)
        {
            jQuery(el).parents('tr').remove();
            jQuery('#trAddNewProgram').show();

            if($hiddenTrEdited) $hiddenTrEdited.show();
            $hiddenTrEdited = null;
            return false;
        };
    </script>
</div>

<div id="tab1" style="display:none;">
    <% if(false) { %> В конкурсе отсутствуют специальности, или для них не указаны требуемые вступительные испытания. <%} else { %>
    <div id="content">
        <% if (/*Model.CompetitiveGroupEdit.CanEdit*/ 1==1 && (!GVUZ.Helper.UrlUtils.IsReadOnly(FBDUserSubroles.CompetitiveGroupsDirection))) { %>
            <div><a href="#" onclick="addBenefit(null);return false" id="btnAddbenefitGeneral">Общие льготы (<%= Model.EntranceTestItemsEdit.BenefitItems != null ? Model.EntranceTestItemsEdit.BenefitItems.Count().ToString() : "0" %>)</a></div>
        <% } %>
    
        <h4>Вступительные испытания конкурса
        </h4>
        <table class="gvuzDataGrid tableStatement" cellpadding="3" id="tableMain" testTypeID="<%= EntranceTestItemDataViewModel.EntranceTestType.MainType %>" <%= false ? "style=\"display:none\"" : "" %>>
	        <thead>
		        <tr id="thTableMain">
			        <th style="width:25%"><%= Html.LabelFor(x => x.EntranceTestItemsEdit.EntranceTestName) %></th>
			        <th style="width:25%"><%= Html.LabelFor(x => x.EntranceTestItemsEdit.UID) %></th>
			        <th style="width:25%"><%= Html.LabelFor(x => x.EntranceTestItemsEdit.MinScore) %></th>
                    <th style="width:25%"><%= Html.LabelFor(x => x.EntranceTestItemsEdit.EntranceTestPriority) %></th>
                    <th style="width:40px"></th>
			        <th>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</th>
		        </tr>
	        </thead>
	        <tbody>
		        <tr id="trAddNew">
			        <td colspan="8">
				        <a href="#" id="btnAddNew" class="button">Добавить ВИ</a>
			        </td>
		        </tr>
	        </tbody>
        </table>

        <%-- Творческие испытания  Model.CreativeTestTypeID == 0 --%>
        <div class="divCreative" <%= false ? "style=\"display:none\"" : "" %>>
        <h4>Вступительные испытания творческой и (или) профессиональной направленности</h4>
        <table class="gvuzDataGrid tableSpecific tableStatement" cellpadding="3" id="tableCreative" testTypeID="<%= EntranceTestItemDataViewModel.EntranceTestType.CreativeType %>">
	        <thead>
		        <tr id="thTableCreative">
			        <th style="width:20%"><%= Html.LabelFor(x => x.EntranceTestItemsEdit.EntranceTestName) %></th>
			        <th style="width:20%"><%= Html.LabelFor(x => x.EntranceTestItemsEdit.UID)%></th>
			        <th style="width:20%"><%= Html.LabelFor(x => x.EntranceTestItemsEdit.MinScore) %></th>
                    <th style="width:20%"><%= Html.LabelFor(x => x.EntranceTestItemsEdit.EntranceTestPriority) %></th>
			        <th style="width:40px"></th>
			        <th>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</th>
		        </tr>
	        </thead>
	        <tbody>
		        <tr id="trAddNewCreative">
			        <td colspan="5">
				        <a href="#" id="btnAddNewCreative" class="add">Добавить новое испытание</a>
			        </td>
		        </tr>		
	        </tbody>
        </table>
        </div>

	    <%-- Профильные испытания Model.CompetitiveGroupEdit.ProfileTestItems == null --%>
        <div class="divProfile" <%= false ? "style=\"display:none\"" : "" %>>
	    <h4>Вступительные испытания профильной направленности</h4>
	    <table class="gvuzDataGrid tableStatement" cellpadding="3" id="tableProfile" testTypeID="<%= EntranceTestItemDataViewModel.EntranceTestType.ProfileType %>">
		    <thead>
			    <tr id="thTableProfile">
				    <th style="width:20%"><%= Html.LabelFor(x => x.EntranceTestItemsEdit.EntranceTestName) %></th>
				    <th style="width:20%"><%= Html.LabelFor(x => x.EntranceTestItemsEdit.UID)%></th>
				    <th style="width:20%"><%= Html.LabelFor(x => x.EntranceTestItemsEdit.MinScore) %></th>
                    <th style="width:20%"><%= Html.LabelFor(x => x.EntranceTestItemsEdit.EntranceTestPriority) %></th>
				    <th style="width:40px"></th>
				    <th>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</th>
			    </tr>
		    </thead>
		    <tbody>
			    <tr id="trAddNewProfile">
				    <td colspan="5">
					    <a href="#" id="btnAddNewProfile" class="button">Добавить</a>
				    </td>
			    </tr>
		    </tbody>

	    </table>
    </div>
    </div>
    
    <script type="text/javascript">
        this.entranceTests = JSON.parse('<%= Html.Serialize(Model.EntranceTestItemsEdit.TestItems) %>');
        this.entranceTestsCreative = JSON.parse('<%= Html.Serialize(Model.EntranceTestItemsEdit.CreativeTestItems) %>');
        this.entranceTestsProfile = JSON.parse('<%= Html.Serialize(Model.EntranceTestItemsEdit.ProfileTestItems) %>');
        
        this.benefits = JSON.parse('<%= Html.Serialize(Model.EntranceTestItemsEdit.BenefitItems) %>');
        this.subjects = JSON.parse('<%= Html.Serialize(Model.Subjects) %>');
       
        this.isMVD = JSON.parse('<%= Html.Serialize(Model.CompetitiveGroupEdit.IsMVD) %>');

        if (entranceTests == null) entranceTests = [];
        if (entranceTestsCreative == null) entranceTestsCreative = [];
        if (entranceTestsProfile == null) entranceTestsProfile = [];
        if (benefits == null) benefits = [];
        if (subjects == null) subjects = [];

	    var competitiveGroupID = <%= Model.CompetitiveGroupEdit.CompetitiveGroupID %>;
	    var $hiddenTrEdited;
	    var benefitListReturnedCount;
	    
	    this.bachelorAndSpeciality = false;

        var readOnly = <%= ((!Model.CompetitiveGroupEdit.CanEdit) ? "true" : "false" )%>;
        <%-- убираю условие || GVUZ.Helper.UrlUtils.IsReadOnly(FBDUserSubroles.CompetitiveGroupsDirection) --%>

        this.levelBudgets = JSON.parse('<%= Html.Serialize(Model.LevelBudgets) %>');
        if (levelBudgets == null) levelBudgets = [];

        var FillEntranceTestTableHeaders = function()
        {
            var c = this.campaign;
            
            this.bachelorAndSpeciality =  (c != null && c.CampaignTypeID == <%= GVUZ.DAL.Dapper.ViewModel.Dictionary.CampaignTypesView.BachelorAndSpeciality %>);
            this.foreigners =  (c != null && c.CampaignTypeID == <%= GVUZ.DAL.Dapper.ViewModel.Dictionary.CampaignTypesView.Foreigners %>);
            var width = bachelorAndSpeciality ? '16' : '24';
             
            var sct = '<th style="width:' + width + '%"><%= Html.LabelFor(x => x.EntranceTestItemsEdit.EntranceTestName) %></th>' +
			        '<th style="width:' + width + '%"><%= Html.LabelFor(x => x.EntranceTestItemsEdit.UID) %></th>' + 
			        '<th style="width:' + width + '%"><%= Html.LabelFor(x => x.EntranceTestItemsEdit.MinScore) %></th>' +
                    '<th style="width:' + width + '%"><%= Html.LabelFor(x => x.EntranceTestItemsEdit.EntranceTestPriority) %></th>'; 
            

            if (bachelorAndSpeciality || foreigners)
            {
                 sct += 
                       '<th style="width:' + width + '%"><%= Html.LabelFor(x => x.EntranceTestItemsEdit.ComplexEntranceTestItem) %></th>' 
                       <%--'<th style="width:' + width + '%"><%= Html.LabelFor(x => x.EntranceTestItemsEdit.SecondComplexEntranceTestItem) %></th>'+--%>
            }
               
            if (bachelorAndSpeciality)
            {
                sct += '<th style="width:' + width + '%"><%= Html.LabelFor(x => x.EntranceTestItemsEdit.IsForProfile) %></th>' + 
                       '<th style="width:' + width + '%"><%= Html.LabelFor(x => x.EntranceTestItemsEdit.EntranceTestNameForChange) %></th>';
            }
                       

            sct += '<th style="width:40px"></th><th>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</th>';
            $('#thTableMain').html(sct);
            $('#thTableCreative').html(sct);
            $('#thTableProfile').html(sct);
        }


        var addEditRow = function($trToAdd, itemID)
        {
            itemID = itemID ? itemID : "";
            if(itemID == null) itemID = ''; //possible fix of strange issue
            var className = $trToAdd.prev().attr('class')
            if(className == 'trline1') className = 'trline2'; else className = 'trline1';
            var etString = '<input type="text" class="entranceTypeText" maxLength="100" />'
            var str = '<tr itemID="' + (itemID) + '"  class="'+ className + '"><td>' + etString + '</td>' 
		    + '<td align="center"><input type="text" class="entranceUID" maxlength="200" /></td>' 
		    + '<td align="center"><input type="text" class="entranceMinScore numeric" maxlength="8" /></td>'
		    + '<td align="center"><select style="text-align: left;" class="entranceEntranceTestPriority numeric"><option value="-1" label="Без приоритета">Без приоритета</option> <option value="1">1</option><option label="2" value="2">2</option><option label="3" value="3">3</option><option label="4" value="4">4</option><option label="5" value="5">5</option><option label="6" value="6">6</option><option label="7" value="7">7</option><option label="8" value="8">8</option><option label="9" value="9">9</option><option label="10" value="10">10</option></select></td>';

            if (this.bachelorAndSpeciality || this.foreigners)
            {
                str += '<td align="center"><input type="checkbox" class="isComplexItem1" />' 
            }

            if (this.bachelorAndSpeciality)
            {
                    //'</td><td align="center"><input type="checkbox" class="isComplexItem2" /></td>'
                str +=  '<td align="center"><input type="checkbox" class="isForSPOandVO" /></td>' +
                       '<td align="center"><select style="text-align: left;" class="replacedEntranceTestItemID"><option value="0"></option>';
                            
                var data=[];
                data.push(this.entranceTests);
                data.push(this.entranceTestsCreative);
                data.push(this.entranceTestsProfile);

                for(var j=0; j<data.length; j++)
                {
                    for(var i=0; i< data[j].length; i++)
                    {
                        var datarow = data[j][i];
                        if (!datarow.IsForSPOandVO && datarow.ItemID != itemID)
                            str += '<option value="' + datarow.ItemID + '">' + datarow.TestName + '</option>';
                    }
                }


                str += '</select></td>';
            }

		    str += '<td align="center" nowrap="nowrap"><a href="#" class="btnSave" onclick="etSaveButton(this);return false;"></a> ' 
		    + '<a href="#" class="btnDeleteU" onclick="etDeleteUButton(this);return false;"></a></td>'
		    + '<td>&nbsp;</td></tr>';

		    $trToAdd.before(str);
        };

        var createdSavedRow = function($trBefore, itemData)
        {
            var className = $trBefore.prev().attr('class')
            if(className == 'trline1') className = 'trline2'; else className = 'trline1';
            var str = '<tr itemID="' + itemData.ItemID + '" class="' + className + '"><td>' +
				    itemData.TestName + '</td><td>' +
				    escapeHtml(itemData.UID == null ? '' : itemData.UID) + '</td>'
				    + '<td align="center">' + (itemData.Value == null ? '' : itemData.Value) + '</td>'
                    + '<td align="center">' + (itemData.EntranceTestPriority == null || itemData.EntranceTestPriority == -1 ? '' : itemData.EntranceTestPriority.toString()) + '</td>';
            
            if (this.bachelorAndSpeciality || this.foreigners)
            {

                str += '<td id="FirstComplexEntranceTestItem' + itemData.itemID + '" align="center"><input type="checkbox" ' + (itemData.IsFirst ? ' checked="checked"' : '' ) + ' disabled="disabled"></td>' 
            }
                if (this.bachelorAndSpeciality) 
              {
                    //'<td id="SecondComplexEntranceTestItem' + itemData.itemID + '"align="center"><input type="checkbox" ' + (itemData.IsSecond ? ' checked="checked"' : '' ) + ' disabled="disabled"></td>'
                str += '<td align="center"><input type="checkbox" ' + (itemData.IsForSPOandVO ? ' checked="checked"' : '' ) + ' disabled="disabled"></td>';
                str += '<td id=' + itemData.ReplacedEntranceTestItemID + '>' + 
                    escapeHtml(itemData.ReplacedEntranceTestItemName == null ? '' : itemData.ReplacedEntranceTestItemName) + '</td>';

            }
            

            str += '<td align="center" nowrap="nowrap">'
                + ((itemData.CanRemove) ? '<a href="#" class="btnEdit" onclick="etEditButton(this);return false;"></a>' : '<span class="btnEditGray"></span>')
                + '&nbsp;'
                + (itemData.CanRemove 
                ? ('<a href="#" class="btnDelete" onclick="etDeleteButton(this);return false;"></a>') 
                : ('<span class="btnDeleteGray"></span>'))
            + '</td><td>' 
            + (/*readOnly */ 1==0 ? '<label class="tdBenefits">' : '<a href="#" onclick="addBenefit(this);return false;" class="tdBenefits">')	           
            + 'Льготы&nbsp;(' + itemData.BenefitItems.length + ')'
            + (/*readOnly */ 1==0 ? '</label>' : '</a>')                
            + '</td></tr>';

            $trBefore.before(str);

        };

 

        function addNewEntranceTestRow(el, nonChangeName, itemID)
        {
            // Перед добавлением нового ВИ надо проверить, что можно блокировать все поля на основной форме
            if (itemID == null){
                var modelCG = ValidateMain();
                if (modelCG==null)
                {
                    alert('Добавление вступительных испытаний и целевых организаций доступно только после ввода основных данных конкурса!');
                    return;
                }
            }

            jQuery('.btnDeleteU').click();
            addEditRow(jQuery(el).parents('tr'), itemID);

            if(jQuery('.entranceTypeText').length > 0)
            {
                autocompleteDropdown(jQuery('.entranceTypeText'), {source: function(ui, response) 
                {
                    var res = [];
                    var x = ui.term.toUpperCase();
                    for(var i = 0; i < subjects.length; i++)
                        if(subjects[i].Name.toUpperCase().indexOf(x) >= 0)
                        {
                            res.push(subjects[i].Name);
                        }
                    response(res);
                },
                    select: function() {setTimeout(function() {jQuery('.entranceTypeText').change()}, 0)},
                    minLength: 2});

                jQuery('.entranceTypeText').change(function() {
                    var selVal = jQuery(this).val();
                    for(var i = 0; i < subjects.length; i++)
                        if(subjects[i].Name == selVal)
                        {
                            jQuery('.entranceMinScore').val(subjects[i].MinValue);
                            break;
                        }
                });
            }
            else
                jQuery('.entranceMinScore').val(0);
		
            jQuery(el).parents('tr:first').hide();
            return false;
        };

        jQuery('#btnAddNew,#btnAddNewCustom,#btnAddNewCreative,#btnAddNewProfile').click(function() {addNewEntranceTestRow(this, null, null);return false;});

        function etEditButton (el)
        {
            var datarow = checkEntranceTestIsUsed(el);
            if (datarow)
            {
                alert('Редактирование данного ВИ невозможно, поскольку его заменяет для профильных СПО/ВО ВИ ' + datarow.TestName);
                return;
            }

            var $tr = jQuery(el).parents('tr:first');
            var child = $tr.children('td');
            var canChange = $tr.find('.btnDeleteGray').length == 0; /*&& $tr.parents('#tableProfile').length == 0*/
            addNewEntranceTestRow(el, canChange ? null : child[0].innerHTML, $tr.attr('itemID'));
            jQuery('.entranceTypeText').val(unescapeHtml(child[0].innerHTML));
            jQuery('.entranceUID').val(unescapeHtml(child[1].innerHTML));
            jQuery('.entranceMinScore').val(child[2].innerHTML);
            jQuery('.entranceEntranceTestPriority').val(child[3].innerHTML);
            if (this.bachelorAndSpeciality || this.foreigners){
                jQuery('.isComplexItem1')[0].checked = child[4].children[0].checked;
                //jQuery('.isComplexItem2')[0].checked = child[5].children[0].checked;
            }
            if (this.bachelorAndSpeciality)
            {
                
                jQuery('.isForSPOandVO')[0].checked = child[4].children[0].checked;
                var replacedTestItem = jQuery('.replacedEntranceTestItemID')

                replacedTestItem.change(function() {
                    var selectedTestItemID = parseInt(jQuery('.replacedEntranceTestItemID :selected').val())
                    jQuery('.replacedEntranceTestItemID option[value='+ selectedTestItemID +']"').attr("selected", true);
                })

            }
            $hiddenTrEdited = $tr;
            $tr.hide();
            return false;
            
        };


        function checkEntranceTestIsUsed(el)
        {
            var $tr = jQuery(el).parents('tr');
            var row = $tr[0].rowIndex;
            if (this.bachelorAndSpeciality){
                var model;
                if(jQuery(el).parents('#tableMain').length != 0) {
                    model = this.entranceTests[row-1];
                }else if (jQuery(el).parents('#tableCreative').length != 0){
                    model = this.entranceTestsCreative[row-1];
                }else if (jQuery(el).parents('#tableProfile').length != 0) {
                    model = this.entranceTestsProfile[row-1];
                }
                
                var data=[];
                data.push(this.entranceTests);
                data.push(this.entranceTestsCreative);
                data.push(this.entranceTestsProfile);

                for(var j=0; j<data.length; j++)
                {
                    for(var i=0; i< data[j].length; i++)
                    {
                        var datarow = data[j][i];
                        if (datarow.ReplacedEntranceTestItemID == model.ItemID){

                            //alert('Удаление данного ВИ невозможно, поскольку его заменяет для профильных СПО/ВО ВИ ' + datarow.TestName);
                            return datarow;
                        }
                    }
                }
            }
            return null;
        }

        function etDeleteButton (el)
        {
            var $tr = jQuery(el).parents('tr');
            var row = $tr[0].rowIndex;

            var datarow = checkEntranceTestIsUsed(el);
            if (datarow)
            {
                alert('Удаление данного ВИ невозможно, поскольку его заменяет для профильных СПО/ВО ВИ ' + datarow.TestName);
                return;
            }

            confirmDialog('Вы действительно хотите удалить испытание?', function () {
                
                if(jQuery(el).parents('#tableMain').length != 0)
                {
                    document.getElementById("tableMain").deleteRow(row);
                    this.entranceTests.splice(row - 1, 1);
                }
                else if (jQuery(el).parents('#tableCreative').length != 0)
                {
                    document.getElementById("tableCreative").deleteRow(row);
                    this.entranceTestsCreative.splice(row - 1, 1);
                }
                else if (jQuery(el).parents('#tableProfile').length != 0)
                {
                    document.getElementById("tableProfile").deleteRow(row);
                    this.entranceTestsProfile.splice(row - 1, 1);
                }

                tabControl.menuItems[tabControl.menuItems.length-1].name = TabName(1);
                tabControl.init();
                CheckMainTableEnabled();

            });
            return false;
        };
	
        this.etId = -1;
        this.isError = false;
	    function etSaveButton(el)
        {

		    var $tr = jQuery(el).parents('tr');
		    clearValidationErrors($tr);
		    var itemID = $tr.attr('itemID');
		    if (!itemID || itemID=='')
		    {
		        itemID = this.etId;
		        this.etId--;
		    }
		    
		    this.isError = false;

            var model =
			    {
			        //CompetitiveGroupID: <%= Model.CompetitiveGroupEdit.CompetitiveGroupID %>,
			        ItemID: itemID, // пока не используется, но возможно понадобится для Benefit'ов
			        TestName: jQuery('.entranceTypeText').val(),
			        UID: jQuery('.entranceUID').val(),
			        Value: jQuery('.entranceMinScore').val(),
			        TestType: $tr.parents('table.gvuzDataGrid:first').attr('testTypeID'),
			        EntranceTestPriority: jQuery('.entranceEntranceTestPriority').val(),

			        IsForSPOandVO: this.bachelorAndSpeciality ? jQuery('.isForSPOandVO')[0].checked : false,
			        ReplacedEntranceTestItemID: this.bachelorAndSpeciality ? jQuery('.replacedEntranceTestItemID').val() : 0,
			        IsFirst: this.bachelorAndSpeciality || this.foreigners ? jQuery('.isComplexItem1')[0].checked : false,
			        //IsSecond: this.bachelorAndSpeciality ? jQuery('.isComplexItem2')[0].checked : false,
			        ReplacedEntranceTestItemName: (this.bachelorAndSpeciality && jQuery('.replacedEntranceTestItemID')[0].selectedIndex > 0) ? jQuery('.replacedEntranceTestItemID')[0].options[jQuery('.replacedEntranceTestItemID')[0].selectedIndex].text : '',
			        CanRemove: true,
                    BenefitItems: []
			    };

	        // Название предмета не может быть пустым
		    if (!model.TestName || model.TestName == '')
		    {		        
		        jQuery('.entranceTypeText').removeClass('input-validation-error-fixed').addClass('input-validation-error');
		        jQuery('.entranceTypeText').next().after('<span class="field-validation-error"><br/>Необходимо задать наименование испытания</span>');
		        isError = true;
		    }

            // Мин. балл не может быть не числом, быть меньше 0 или больше 100
		    var valFloat = new Number(jQuery('.entranceMinScore').val().replace(',', '.'));
		    var eduForm = $('#Select_EducationLevels').val();
		    if (eduForm != 4 && eduForm != 18) {
		        if(valFloat < 0 || valFloat > 100 || isNaN(valFloat))
		        {
		            jQuery('.entranceMinScore').removeClass('input-validation-error-fixed').addClass('input-validation-error');
		            jQuery('.entranceMinScore').after('<span class="field-validation-error"><br/>Балл должен быть числом от 0 до 100</span>');
			        isError = true;
		        }
		    }
		    else {
		        if(valFloat < 0 || valFloat > 999 || isNaN(valFloat))
		        {
		            jQuery('.entranceMinScore').removeClass('input-validation-error-fixed').addClass('input-validation-error');
		            jQuery('.entranceMinScore').after('<span class="field-validation-error"><br/>Балл должен быть числом от 0 до 999</span>');
		            isError = true;
		        }
		    }

		    var currentRow = $tr[0].rowIndex;
	        // Предмет, УИД и приоритет должны быть уникальны в рамках ВСЕХ 3 списоков ВИ  // не забыть про i != currentRow - 1 
		    var data=[];
		    data.push(this.entranceTests);
		    data.push(this.entranceTestsCreative);
		    data.push(this.entranceTestsProfile);

		    var isKVK = false;
		    var isSPO = false;
            if (this.educationLevelID == <%= GVUZ.DAL.Dapper.ViewModel.Dictionary.EDLevelConst.HighQualification %>) 
                isKVK = true; // Хитрая конструкция на случай EducationLevelID = null
            
	        if (this.educationLevelID == <%= GVUZ.DAL.Dapper.ViewModel.Dictionary.EDLevelConst.SPO %>) 
                isSPO = true; // Хитрая конструкция на случай EducationLevelID = null

	        var hasPriority = false;

	        // 
	        //if (model.IsFirst && model.IsSecond) 
	        //{
	        //    jQuery('.isComplexItem2').after('<span class="field-validation-error"><br/>Предмет не может состоять одновременно в двух группах</span>');
	        //    jQuery('.isComplexItem1').after('<span class="field-validation-error"><br/>Предмет не может состоять одновременно в двух группах</span>');
	        //    isError = true;
	        //}

		    for(var j=0; j<data.length; j++)
		    {
		        for(var i=0; i< data[j].length; i++)
		        {
		            var row = data[j][i];
                    // Название ВИ должно быть уникально
		            //if (row.TestName == model.TestName && row.ItemID != model.ItemID) // && jQuery(el).parents('#tableMain').length != 0 если бы надо было проверять только в рамках 1 списка
		            //{
		            //    jQuery('.entranceTypeText').removeClass('input-validation-error-fixed').addClass('input-validation-error');
		            //    jQuery('.entranceTypeText').next().after('<span class="field-validation-error"><br/>Наименование испытания должно быть уникальным</span>');
		            //    isError = true;
		            //}

                    // УИД должен быть пустой или уникальный
		            if (model.UID != '' && row.UID == model.UID && row.ItemID != model.ItemID)
		            {
		                jQuery('.entranceUID').removeClass('input-validation-error-fixed').addClass('input-validation-error');
		                jQuery('.entranceUID').after('<span class="field-validation-error"><br/>UID должен быть пустым или уникальным</span>');
		                isError = true;
		            }

		            // Приоритет должен быть уникален или это замена записи с таким же приоритетом или предметы из одной группы ВИ
		            if (row.EntranceTestPriority == model.EntranceTestPriority 
                       && (!(!model.EntranceTestPriority || model.EntranceTestPriority == '' || model.EntranceTestPriority == -1))
                       && row.ItemID != model.ItemID && !model.IsFirst && row.TestName != model.TestName
                       && (model.ReplacedEntranceTestItemName != row.TestName && !row.IsFirst))
		            {
		                jQuery('.entranceEntranceTestPriority').removeClass('input-validation-error-fixed').addClass('input-validation-error');
		                jQuery('.entranceEntranceTestPriority').after('<span class="field-validation-error"><br/>Приоритет должен быть уникальным</span>');
		                isError = true;
		            } 

		            //Приоритет предметов из одной группы ВИ должен быть одинаковым
		            if (model.IsFirst && row.IsFirst && (row.EntranceTestPriority != model.EntranceTestPriority)) 	                
                        
		            {
		                jQuery('.entranceEntranceTestPriority').after('<span class="field-validation-error"><br/>В пределах одной группы ВИ приоритет должен быть одинаковым</span>');
		                isError = true;
		            }              
                    		            
		            
		            // Для КВК
		            //if (!(!row.EntranceTestPriority || row.EntranceTestPriority == '' || row.EntranceTestPriority == -1))
                    //    hasPriority = true;

		             //Проверить, что если заменяем ВИ, то это же ВИ не было заменено ранее
		            //if (model.ReplacedEntranceTestItemID != 0 && model.ReplacedEntranceTestItemID == row.ReplacedEntranceTestItemID && row.ItemID != model.ItemID)
		            //{
		            //    jQuery('.replacedEntranceTestItemID').removeClass('input-validation-error-fixed').addClass('input-validation-error');
		            //    jQuery('.replacedEntranceTestItemID').after('<span class="field-validation-error"><br/>Одно ВИ может быть заменено только 1 раз</span>');
		            //    isError = true;
		            //}

		        }
		    }


            
            // Приоритет должен быть задан, кроме КВК - тогда хватит 1 записи с приоритетом!
            // А если вуз МВД, то можно вообще не задавать
		    if (!(isKVK || isSPO || hasPriority) 
                && (model.EntranceTestPriority == '' || model.EntranceTestPriority == -1) 
                && (!(this.isMVD && model.TestType == 1))
                )
		    {
		        jQuery('.entranceEntranceTestPriority').removeClass('input-validation-error-fixed').addClass('input-validation-error');
		        jQuery('.entranceEntranceTestPriority').after('<span class="field-validation-error"><br/>Необходимо задать приоритет</span>');
		        isError = true;
		    }
            

            // !Не может быть выбрана галочка, но при этом не выбрано заменяемое ВИ и наоборот
		    if ((model.IsForSPOandVO && model.ReplacedEntranceTestItemID == 0) || 
                (!model.IsForSPOandVO && model.ReplacedEntranceTestItemID != 0))
		    {
		        jQuery('.replacedEntranceTestItemID').removeClass('input-validation-error-fixed').addClass('input-validation-error');
		        jQuery('.replacedEntranceTestItemID').after('<span class="field-validation-error"><br/>Вместе с признаком "ВИ для профильных СПО/ВО" необходимо указать заменяемое ВИ</span>');
		        isError = true;
		    }

            

	        // Мин. балл не может быть меньше мин. балла ЕГЭ, для бакалавриата и специалитета, кроме Крыма
		    var isFromKrym = $("#IsFromKrym").is(':checked');
		    if (!isFromKrym &&
                (this.educationLevelID == <%= GVUZ.DAL.Dapper.ViewModel.Dictionary.EDLevelConst.Bachelor %> || this.educationLevelID == <%= GVUZ.DAL.Dapper.ViewModel.Dictionary.EDLevelConst.Speciality %>)
                )
		    {
		        // если это предмет и у него мин балл не 0
                var minValue = 0;
                for(var i=0; i< this.subjects.length; i++)
                {
                    if (this.subjects[i].Name == model.TestName)
                    {
                        minValue = subjects[i].MinValue;
                    }
                }
                if (minValue > 0 && minValue > parseFloat(valFloat))
                {
                    jQuery('.entranceMinScore').removeClass('input-validation-error-fixed').addClass('input-validation-error');
                    jQuery('.entranceMinScore').after('<span class="field-validation-error"><br/>Балл должен быть не менее ' + minValue + '</span>');
                    isError = true;
                }
		    }

		    if(isError) 
		        return false;


            

		    var row = $tr[0].rowIndex;
		    var oldModel;
	        if(jQuery(el).parents('#tableMain').length != 0)
	        {

	            model.BenefitItems = (this.entranceTests[row - 1]) ? this.entranceTests[row - 1].BenefitItems : [];
	            this.entranceTests.splice(row - 1, 1, model);
	        }
	        else if (jQuery(el).parents('#tableCreative').length != 0)
	        {
	            model.BenefitItems = (this.entranceTestsCreative[row - 1]) ? this.entranceTestsCreative[row - 1].BenefitItems : [];
	            this.entranceTestsCreative.splice(row - 1, 1, model);
	        }
	        else if (jQuery(el).parents('#tableProfile').length != 0)
	        {
	            model.BenefitItems = (this.entranceTestsProfile[row - 1]) ? this.entranceTestsProfile[row - 1].BenefitItems : [];
	            this.entranceTestsProfile.splice(row - 1, 1, model);
	        }

	        if($hiddenTrEdited) $hiddenTrEdited.remove().detach();
	        jQuery('#trAddNew').show();
	        jQuery('#trAddNewCreative').show();
	        jQuery('#trAddNewProfile').show();
	        createdSavedRow($tr, model);
	        $tr.remove().detach();

	        tabControl.menuItems[tabControl.menuItems.length-1].name = TabName(1);
	        tabControl.init();
	        CheckMainTableEnabled();

            return false;
        };
        
        function etDeleteUButton (el)
        {
            jQuery(el).parents('tr').remove();
            jQuery('#trAddNew,#trAddNewCustom,#trAddNewCreative').show();
            if(jQuery('#tableProfile tbody tr').length == 1) jQuery('#trAddNewProfile').show();

            if($hiddenTrEdited) $hiddenTrEdited.show();
            $hiddenTrEdited = null;
            return false;
        };
        
        var InitialTestsFillData = function ()
        {
            if(entranceTests != null)
                for(var i = 0; i < entranceTests.length; i++)
                    createdSavedRow(jQuery('#trAddNew'), entranceTests[i]);

            if(entranceTestsCreative != null)
                for(var i = 0; i < entranceTestsCreative.length; i++)
                    createdSavedRow(jQuery('#trAddNewCreative'), entranceTestsCreative[i]);
            if(entranceTestsProfile != null)
                for(var i = 0; i < entranceTestsProfile.length; i++)
                    createdSavedRow(jQuery('#trAddNewProfile'), entranceTestsProfile[i]);

        
            if (readOnly)
            {
                jQuery('.content input').addClass('view').attr('readonly', 'readonly')
                jQuery('#trAddNew,#trAddNewCustom,#trAddNewCreative,#trAddNewProfile').hide();
                jQuery('.content .btnDelete, .content .btnDeleteGray, .content .btnEdit').hide();            
            }

            CheckMainTableEnabled();
        };
        
        

        ////function addValidationErrorsFromServerResponseLocal(data)
        ////{
        ////    if(data.IsError)
        ////    {
        ////        alert(data.Message);
        ////        if (data.Data != null && data.Data.length != null)
        ////            for(var i = 0; i < data.Data.length;i++)
        ////            {
        ////                if(data.Data[i].ControlID == 'Form')
        ////                    addValidationError(jQuery('.entranceForm'), '');
        ////                if(data.Data[i].ControlID == 'MinScore')
        ////                    addValidationError(jQuery('.entranceMinScore'), '');
        ////                if(data.Data[i].ControlID == 'EntranceTestName')
        ////                {
        ////                    addValidationError(jQuery('.entranceTypeText'), '');
        ////                }
        ////                if(data.Data[i].ControlID == 'UID')
        ////                {
        ////                    addValidationError(jQuery('.entranceUID'), '');
        ////                }
        ////            }
        ////        return true;
        ////    }
        ////    return false;
        ////};
        
        this.EntranceTestItemID = 0;
        function addBenefit(el)
        {
            this.EntranceTestItemID = 0;
            var tit = 'Общие льготы';
            if(el != null)
            {
                this.EntranceTestItemID = jQuery(el).parents('tr:first').attr('itemID');
                tit = ('Условия предоставления льгот для предмета ' + jQuery(el).parents('tr:first').children('td')[0].innerHTML);
            }
            if (el == null) 
            {
                el = jQuery('#btnAddbenefitGeneral')[0];
            }
            <%--doPostAjax('<%= Url.Generate<BenefitController>(c => c.BenefitList(null, null)) %>', 'entranceTestItemID=' + itemID + '&competitiveGroupID=<%=Model.CompetitiveGroupEdit.CompetitiveGroupID %>', function (data)
            {
                jQuery('#divBenefitListDialog').html(data);
                jQuery('#divBenefitListDialog').dialog({
                    modal: true,
                    width: 850,
                    title: tit,
                    close: function () 
                    {
                        jQuery(el).html(jQuery(el).html().replace(/\(\d+\)/, '(' + benefitListReturnedCount +')')); 
                    }
                }).dialog('open');
            },
            "application/x-www-form-urlencoded", "html")--%>

            this.benefitCanRemove = true;
            this.isBenefitCreative = false;
            this.data = [];
            var $tr = jQuery(el).parents('tr');
            if (!$tr[0]) {
                data = this.benefits;
            }
            else {
                var row = $tr[0].rowIndex;
                if(jQuery(el).parents('#tableMain').length != 0)
                {
                    data = this.entranceTests[row - 1].BenefitItems;
                    this.benefitCanRemove = this.entranceTests[row - 1].CanRemove;
                }
                else if (jQuery(el).parents('#tableCreative').length != 0)
                {
                    data = this.entranceTestsCreative[row - 1].BenefitItems;
                    this.benefitCanRemove = this.entranceTestsCreative[row - 1].CanRemove;
                    this.isBenefitCreative = true;
                }
                else if (jQuery(el).parents('#tableProfile').length != 0)
                {
                    data = this.entranceTestsProfile[row - 1].BenefitItems;
                    this.benefitCanRemove = this.entranceTestsProfile[row - 1].CanRemove;
                }
            }

            jQuery('#divBenefitListDialog').dialog({
                modal: true,
                width: 950,
                title: tit,
                open: function() {
                    LoadBenefitList();
                },
                dialogClass: 'ui-widget-top',
                close: function() 
                {
                    // TODO: Вернувшиеся данные!!!

                    jQuery(el).html(jQuery(el).html().replace(/\(\d+\)/, '(' + data.length +')')); 
                }
            }).dialog('open');

            return false;
        };

        function DisableLevelBudgetCheck() {
            var c = this.campaign;
            var foreignersCampaign = (c != null && c.CampaignTypeID == <%= GVUZ.DAL.Dapper.ViewModel.Dictionary.CampaignTypesView.Foreigners  %> );
            
            var paidCG = (educationSourceID == <%= GVUZ.DAL.Dapper.ViewModel.Dictionary.EDSourceConst.Paid %>);

            if (foreignersCampaign || paidCG) {
                jQuery('#Select_LevelBudgets').addClass('view').attr('disabled', 'disabled');
                //jQuery('#Select_LevelBudgets').attr('disabled', 'disabled');          
            }
            else {
                jQuery('#Select_LevelBudgets').removeClass('view').removeAttr('disabled');
            }
        }

        function DisableKrymCheck() {
            //Крым
            if ($("#CompetitiveGroupEdit_CampaignYearStart").val() != 2016) {
                jQuery('#IsFromKrym').removeAttr('checked').attr('disabled', 'disabled');
            }
            else {
                jQuery('#IsFromKrym').removeAttr('disabled');
            }
        }

    </script>

    <%} %> <%-- edit allowed --%>
</div>

<div id="tab2">
    Места задаются <input type="radio" id="valueTargetItems" name="competitiveGroupTargetValue" checked="checked" onclick="ChangeTargetValueMode()" /> на каждую целевую организацию или <input type="radio" id="valueCompetitiveGroup" name="competitiveGroupTargetValue" onclick="ChangeTargetValueMode()" /> на весь конкурс.
    <h4>Целевые организации</h4>
    <table class="gvuzDataGrid tableStatement" cellpadding="3" id="tableTargets">
        <thead>
		    <tr>
			    <th style="width:50%"><%= Html.LabelFor(x => x.CompetitiveGroupTargetsEdit.Name) ?? Html.LabelFor(x => x.CompetitiveGroupTargetsEdit.ContractOrganizationName) %></th>
			    <th style="width:50%"><%= Html.LabelFor(x => x.CompetitiveGroupTargetsEdit.Value) %></th>
			    <th style="width:40px"></th>
		    </tr>
	    </thead>
	    <tbody>
		    <tr id="trAddNewTarget">
			    <td colspan="4">
				    <a href="#" id="btnAddNewTarget" class="add">Добавить</a>
			    </td>
		    </tr>
	    </tbody>
    </table>
        
    <script type="text/javascript">
        function InitTargets()
        {
            // Очистить таблицу /  Clear table 
            var row = jQuery('#trAddNewTarget').prev('tr');
            while (row.length > 0 && row[0].rowIndex > 0){
                document.getElementById("tableTargets").deleteRow(row[0].rowIndex);
                row = jQuery('#trAddNewTarget').prev('tr');
            }

            for(var i = 0; i < targets.length; i++)
            {
                targets[i].Value = getTargetValue(targets[i]);
                createdSavedTarget(jQuery('#trAddNewTarget'), targets[i]);
            }
        }

        function getTargetValue(itemData)
        {
            if (this.educationFormID == <%= GVUZ.DAL.Dapper.ViewModel.Dictionary.EDFormsConst.O %>)
                return itemData.NumberTargetO;
            else if (this.educationFormID == <%= GVUZ.DAL.Dapper.ViewModel.Dictionary.EDFormsConst.OZ %>)
                return itemData.NumberTargetOZ;
            else if (this.educationFormID == <%= GVUZ.DAL.Dapper.ViewModel.Dictionary.EDFormsConst.Z %> )
                return itemData.NumberTargetZ;
        }

        function createdSavedTarget($trBefore, itemData)
        {
            var className = $trBefore.prev().attr('class')
            if(className == 'trline1') className = 'trline2'; else className = 'trline1';
            $trBefore.before('<tr itemID="' + itemData.CompetitiveGroupTargetID + '" class="' + className + '">' 
                + '<td>' + escapeHtml(itemData.DisplayName) + '</td>' 
                +  '<td>' + (targetItemsMode ? itemData.Value : '') + '</td>'
                //+ '<td>' + escapeHtml(itemData.Value) + '</td>'
                
                + '<td align="center" nowrap="nowrap">'
                + '<a href="#" class="btnEdit" onclick="targetEditButton(this);return false;">' 
                + '</a>&nbsp;' 
                + (true //itemData.CanRemove 
                        ? ('<a href="#" class="btnDelete Target" onclick="targetDeleteButton(this);return false;"></a>') 
                        : ('<span class="btnDeleteGray"></span>'))
                + '</td></tr>');
        };


        jQuery('#btnAddNewTarget').click(function() {addNewTarget(this, null, null);return false;})
        
        var addEditTargetRow = function($trToAdd, itemID)
        {
            itemID = itemID ? itemID : "";
            if(itemID == null) itemID = ''; //possible fix of strange issue
            var className = $trToAdd.prev().attr('class')

            //<select style="text-align: left;" class="entranceEntranceTestPriority numeric"><option value="-1" label="Без приоритета">Без приоритета</option> <option value="1">1</option><option label="2" value="2">2</option><option label="3" value="3">3</option><option label="4" value="4">4</option><option label="5" value="5">5</option><option label="6" value="6">6</option><option label="7" value="7">7</option><option label="8" value="8">8</option><option label="9" value="9">9</option><option label="10" value="10">10</option></select>
            //<input type="text" class="targetName" maxlength="200" />
            var selectTargets = '<select class="targetName"><option value="0" selected="true"></options>';
            for(var i = 0; i < this.targetOrganizations.length; i++)
            {
                selectTargets += '<option value=' + targetOrganizations[i].CompetitiveGroupTargetID + '>';
                selectTargets += targetOrganizations[i].DisplayName;
                selectTargets += '</option>';
            }
            selectTargets += '</select>';

            if(className == 'trline1') className = 'trline2'; else className = 'trline1';
            $trToAdd.before('<tr itemID="' + (itemID) + '"  class="' + className + '">' 
            + '<td align="center">'

            + selectTargets

            + '</td>' 
            + '<td align="center">'
            + (targetItemsMode ? '<input type="text" class="targetValue numeric" maxlength="200" />' : '')
            + '</td>' 
            + '<td align="center" nowrap="nowrap">'
            + '<a href="#" class="btnSave" onclick="targetSaveButton(this);return false;"></a> ' 
            + '<a href="#" class="btnDeleteU Target" onclick="targetDeleteUButton(this);return false;"></a>'
            + '</td>'
            + '</tr>');
        };

        var addNewTarget = function(el, nonChangeName, itemID){

            // Перед добавлением новой Ц Орг. надо проверить, что можно блокировать все поля на основной форме
            if (itemID == null){
                var modelCG = ValidateMain();
                if (modelCG==null)
                {
                    alert('Добавление вступительных испытаний и целевых организаций доступно только после ввода основных данных конкурса!');
                    return;
                }
            }

            jQuery('.btnDeleteU.Target').click();
            addEditTargetRow(jQuery(el).parents('tr'), itemID);

            jQuery(el).parents('tr:first').hide();
            return false;
        }

        function targetEditButton (el)
	    {
		    var $tr = jQuery(el).parents('tr:first');
		    var child = $tr.children('td');
		    var canChange = $tr.find('.btnDeleteGray').length == 0; /*&& $tr.parents('#tableProfile').length == 0*/
		    addNewTarget(el, canChange ? null : child[0].innerHTML, $tr.attr('itemID'));
		    jQuery('.targetName').val($tr.attr('itemID'));
		    
		    if (this.targetItemsMode){
		        jQuery('.targetValue').val(unescapeHtml(child[1].innerHTML));
		    }else{
		        jQuery('.targetValue').val('');
		    }

		    $hiddenTrEdited = $tr;
		    $tr.hide();
		    return false;
	    }

	    function targetDeleteButton (el)
	    {
		    var $tr = jQuery(el).parents('tr');
		    confirmDialog('Вы действительно хотите удалить целевую организацию?', function () {
		        var row = $tr[0].rowIndex;
		        document.getElementById("tableTargets").deleteRow(row);
		        this.targets.splice(row - 1, 1);

		        tabControl.menuItems[1].name = TabName(2);
		        tabControl.init();
		        TargetValue();
		        
		        CheckMainTableEnabled();
		    });
		    
	        return false;
	    }

        function targetSaveButton(el)
        {
            var $tr = jQuery(el).parents('tr');
            var row = $tr[0].rowIndex;
            clearValidationErrors($tr);
            var isError = false;

            var cgTargetID = parseInt(jQuery('.targetName').val());
            var selIndex = jQuery('.targetName')[0].selectedIndex;
            var name = (selIndex >= 0) ? jQuery('.targetName')[0].options[selIndex].text : '';

            //var name = ((jQuery('.targetName')[0].selectedOptions[0].textContent) ? jQuery('.targetName')[0].selectedOptions[0].textContent : jQuery('.targetName')[0].selectedOptions[0].innerText);
            var value = parseInt(jQuery('.targetValue').val());
            if (!name || name=='')
            {
                jQuery('.targetName').removeClass('input-validation-error-fixed').addClass('input-validation-error');
                jQuery('.targetName').after('<span class="field-validation-error"><br/>Целевая организация должна быть выбрана!</span>');
                isError = true;
            }
            for(var j = 0; j<this.targets.length; j++)
            {
                if (this.targets[j].CompetitiveGroupTargetID == cgTargetID && j != row - 1)
                {
                    jQuery('.targetName').removeClass('input-validation-error-fixed').addClass('input-validation-error');
                    jQuery('.targetName').after('<span class="field-validation-error"><br/>Нельзя выбрать одну и ту же целевую организацию более 1 раза!</span>');
                    isError = true;
                }
            }
            
            if (this.targetItemsMode){
                if (!(Math.round(value) === value) || value < 0){
                    jQuery('.targetValue').removeClass('input-validation-error-fixed').addClass('input-validation-error');
                    jQuery('.targetValue').after('<span class="field-validation-error"><br/>Количество мест должно быть целое неотрицательное число!</span>');
                    isError = true;
                }
            }else{
                if (isNaN(value)) 
                    value = 0;
            }

            if(isError) 
                return false;

            var model =
			    {
			        //CompetitiveGroupID: <%= Model.CompetitiveGroupEdit.CompetitiveGroupID %>,
                    CompetitiveGroupTargetID: cgTargetID,
			        DisplayName : name, 
			        Value : isNaN(value) ? 0 : value

                    //,CompetitiveGroupTargetItemID: 0
                    //,Name: ''
                    //,UID: ''
                    //,NumberTargetO: 0
                    //,NumberTargetOZ: 0
                    //,NumberTargetZ: 0
			    };

            
            this.targets.splice(row - 1, 1, model);

            if($hiddenTrEdited) $hiddenTrEdited.remove().detach();
            jQuery('#trAddNewTarget').show();
            createdSavedTarget($tr, model);
            $tr.remove().detach();

            tabControl.menuItems[1].name = TabName(2);
            tabControl.init();
            TargetValue();

            CheckMainTableEnabled();
            if (!this.targetItemsMode)
                $('#CompetitiveGroupEdit_Value').removeClass('view');

            return false;
        }

        function targetDeleteUButton (el)
        {
            jQuery(el).parents('tr').remove();
            jQuery('#trAddNewTarget').show();

            //if(jQuery('#tableProfile tbody tr').length == 1) jQuery('#trAddNewProfile').show();

            if($hiddenTrEdited) $hiddenTrEdited.show();
            $hiddenTrEdited = null;
            return false;
        };
    </script>

</div>
</div>
</div>

<div>
	<% if (Model.CompetitiveGroupEdit.CanEdit)
	   { %>
	<input type="button" value="Сохранить" id="btnSave" class="button3" />
	<input type="button" value="Отмена" id="btnCancel" class="button3" />	
	<% } else { %>
		<input type="button" value="Вернуться" id="btnCancel" class="button3" />
	<%} %>
</div>

<gv:DirectionInfoPopup runat="server" ID="dirPopup" />

<div id="divPopupDialog"></div>
<div id="divBenefitListDialog" style="display:none;">
    <table class="gvuzDataGrid" cellpadding="3" id="tableBenefitList">
		<thead>
			<tr>
				<th>
					<%= Html.LabelFor(x => x.EntranceTestItemsEdit.BenefitItemColumns.UID ) %>
				</th>
				<th>
					<%= Html.LabelFor(x => x.EntranceTestItemsEdit.BenefitItemColumns.DiplomType) %>
				</th>
                <%--<th>
					<%= Html.LabelFor(x => x.EntranceTestItemsEdit.BenefitItemColumns.OlympicYear) %>
				</th>--%>
                <th>
					<%= Html.LabelFor(x => x.EntranceTestItemsEdit.BenefitItemColumns.IsForAllOlympic) %>
				</th>

				<th>
					<%= Html.LabelFor(x => x.EntranceTestItemsEdit.BenefitItemColumns.OlympicLevelFlags) %>
				</th>
				<th>
					<%= Html.LabelFor(x => x.EntranceTestItemsEdit.BenefitItemColumns.ClassFlags) %>
				</th>					
					
				<th>
					<%= Html.LabelFor(x => x.EntranceTestItemsEdit.BenefitItemColumns.BenefitName) %>
				</th>
                <th>
                    <%= Html.LabelFor(x => x.EntranceTestItemsEdit.BenefitItemColumns.EgeMinValue) %>
                </th>
				<th style="width: 40px">
				</th>
			</tr>
			<tr id="trAddNewBenefit">
				<td colspan="5">
					<a href="#" id="btnAddNewBenefit" class="button">Добавить льготу</a>
				</td>
			</tr>
		</thead>
		<tbody>
		</tbody>
	</table>
	<div>
		<sup>1</sup> Перечень олимпиад школьников, утвержденный министерством образования и науки РФ
	</div>

    <script type="text/javascript">

        jQuery(document).ready(function ()
		{
            jQuery('#btnAddNewBenefit').click(function ()
            {
                blEditButton(null, false);

            });
		    
            <% if(!Model.CompetitiveGroupEdit.CanEdit) { %>
                jQuery('#trAddNewBenefit').hide();
				jQuery('#tableBenefitList .btnDelete,#tableBenefitList .btnEdit').remove().detach();
			<% } %>
        })

        function LoadBenefitList()
        {
            // Очистить таблицу /  Clear table 
            var row = jQuery('#trAddNewBenefit').prev('tr');
            while (row[0].rowIndex > 0){
                document.getElementById("tableBenefitList").deleteRow(row[0].rowIndex);
                row = jQuery('#trAddNewBenefit').prev('tr');
            }
            
            for (var i = 0; i < data.length; i++)
            {
                addNewBenefit(jQuery('#trAddNewBenefit'), data[i], benefitCanRemove);
            }
            
            if (!benefitCanRemove || readOnly)
                $('#trAddNewBenefit').hide();
            else 
                $('#trAddNewBenefit').show();
        }

        function addNewBenefit($trBefore, item, canRemove)
		{
        	var className = $trBefore.prev().attr('class')
			if (className == 'trline1') className = 'trline2'; else className = 'trline1';

			var canR = canRemove && !readOnly;
            //+ '<td align="center">' + item.OlympicYear + '</td>' 
			$trBefore.before('<tr itemID="' + item.BenefitItemID + '" class="' + className + '">'
					+ '<td>' + escapeHtml(item.UID == null ? '' : item.UID) + '</td>'
					+ '<td>' + escapeHtml(item.DiplomType) + '</td>' 
                    + '<td>'+ GetOlympicNumbers(item) + '</td>'
			        + '<td  align="center">' + GetOlympicLevels(item.OlympicLevelFlags) + '</td>' 
			        + '<td>'+ GetOlympicClasses(item.ClassFlags) + '</td>'
                    + '<td>' + escapeHtml(item.BenefitName) + '</td>'
                    + '<td>' + GetMinEge(item) + '</td>'

                    + '<td align="center" nowrap="nowrap">'
                        + (1==1 ? '<a href="#" class="btnEdit" onclick="blEditButton(this, ' + canR + ');return false;"></a>' : '<span class="btnEditGray"></span>')
                        + '&nbsp;'
                        + ((canRemove && !readOnly) ? '<a href="#" class="btnDelete" onclick="blDeleteBenefitButton(this);return false;"></a>' : '<span class="btnDeleteGray"></span>')
                    + '</td></tr>');




        }

        function GetOlympicNumbers(item)
		{
            if (item.IsAllOlympic)
                return 'Все';
            var res = '';
            var sep = '';
            jQuery.each(item.BenefitItemOlympics, function ()
			{
                res += sep;
                sep = ', ';
                res += '<span class="linkSumulator" onmouseout="hideOlympicDetails()" onmouseover="viewOlympicDetails(this, ' 
                    + this.OlympicID + ','
                    + "'" + escapeHtml(this.Name).replace( /"/g , '').replace( /'/g , '') + "'" + ','
                    + "'" + this.ProfileNames + "'" + ','  //!= '' ? this.ProfileNames : ''  
                    + this.OlympicLevelFlags + ','
                    + this.ClassFlags + ','
                    + this.OlympicYear 
                    + ')">' 
                    + this.OlympicNumber.toString() + '</span>';
			})
			return res;
        }

        function GetOlympicLevels(flag) 
        {
            if (flag == 255)
                return "Все уровни";
            else
            {
                var res = '';
                if ((flag & 1) != 0) {
                    res += ", I";
                }
                if ((flag & 2) != 0){
                    res += ", II";
                }
                if ((flag & 4) != 0) {
                    res += ", III";
                }
                return res.substring(2);
            }
        }

        function GetOlympicClasses(flag) {
            if (flag == 255)
                return "Все классы";
            else
            {
                var res = '';
                if ((flag & 1) != 0) {
                    res += ", 7";
                }
                if ((flag & 2) != 0){
                    res += ", 8";
                }
                if ((flag & 4) != 0) {
                    res += ", 9";
                }
                if ((flag & 8) != 0) {
                    res += ", 10";
                }
                if ((flag & 16) != 0) {
                    res += ", 11";
                }
                return res.substring(2);
            }
        }

        function GetMinEge(item) {
            var minEge = '';
            if (item.EntranceTestItemID != 0)
            {
                minEge = item.EgeMinValue == null ? '' : item.EgeMinValue.toString();
            }
            else
            {
                for(var i=0; i< item.BenefitItemSubjects.length; i++){
                    minEge += item.BenefitItemSubjects[i].SubjectName + " - " + item.BenefitItemSubjects[i].EgeMinValue +  "<br/>";
                }
            }

            return minEge;
        }
        
        var olTimerID = 0;
        function viewOlympicDetails(el, olympicID, Name, ProfileNames, OlympicLevel, OlympicClass, OlympicYear)
        {
            clearTimeout(olTimerID);
			olTimerID = setTimeout(function ()
			{
			    var res = "Наименование: " + Name.replace( /"/g , '').replace( /'/g , '') + '<br>' 
			                + "Профиль: " + ProfileNames + '<br>' 
                            + "Уровень: " + GetOlympicLevels(OlympicLevel) + '<br>' 
                            + "Класс: " + GetOlympicClasses(OlympicClass) + '<br>' 
                            + "Год: " + OlympicYear + '<br>';


			    jQuery('#divViewOlympic').html(res);
			    var p = jQuery(el).offset();
			    jQuery('#divViewOlympic').css('position', 'absolute').css('z-index', 1100).css('top', p.top + jQuery(el).height() + 5).css('left', p.left + 10).fadeIn(300);
			}, 300);
		}

		function hideOlympicDetails()
		{
		    clearTimeout(olTimerID);
		    olTimerID = setTimeout(function ()
		    {
		        jQuery('#divViewOlympic').fadeOut(300)
		    }, 700);
		}

		this.benefitItemID = -1;
        // Добавление новой льготы
        function blEditButton(el, canRemove)
		{
            if (!canRemove)
                canRemove = !readOnly;

            var itemID = 0;
            if (el){
                var $tr = jQuery(el).parents('tr:first');
                itemID = parseInt($tr.attr('itemID'));
            }

            this.benefitItemData = {
                BenefitItemID: this.benefitItemID--,
                UID: '',
                IsForAllOlympic: true,
                EgeMinValue: 0,
                OlympicLevelFlags: <%= BenefitItemViewModel.OLYMPIC_ALL %>,
                ClassFlags: <%= BenefitItemViewModel.CLASS_ALL %>,
                EntranceTestItemID: this.EntranceTestItemID,
                BenefitItemProfiles: [],
                BenefitItemOlympics: [],
                BenefitItemSubjects: [],
                BenefitID: this.EntranceTestItemID == 0 ? 1 : 3,
                BenefitName: this.EntranceTestItemID == 0 ? '<%= Model.EntranceTestItemsEdit.GetBenefit1Name %>' : '<%= Model.EntranceTestItemsEdit.GetBenefit3Name %>' ,
                IsCreative: false,
                IsAthletic: false,
                doSave: false
            };
            benefitItemData.BenefitItemProfiles.push({ID: 0, OlympicProfileID: <%= BenefitItemViewModel.PROFILE_ALL %>, ProfileName: 'Все профили'});

            if (itemID != 0){
                for(var i=0; i < this.data.length; i++){
                    if (this.data[i].BenefitItemID == itemID)
                        //this.benefitItemData = this.data[i];
                        this.benefitItemData = jQuery.extend(true, {}, this.data[i]);
                }
            }
            this.benefitItemData.doSave = false;
            
            var buttons = canRemove ? {
                'Сохранить': function () { jQuery('#btnSubmitAB').click() },
                'Отмена': function () { jQuery('#btnCancelAB').click() }
            } : {};

            jQuery('#divAddBenefit').dialog({
                modal: true,
                width: 800,
                title: 'Редактирование льготы',
                buttons: buttons,
                dialogClass: 'ui-widget-top',
                //{
                //    'Сохранить': function () { jQuery('#btnSubmitAB').click() },
                //    'Отмена': function () { jQuery('#btnCancelAB').click() }
                //},
                open: function() {
                    LoadBenefitItemData();
                },
                close: function() 
                {
                    if (benefitItemData != null && benefitItemData.doSave){
                        if (benefitItemData.IsForAllOlympic)
                            benefitItemData.BenefitItemOlympics = [];

                        for(var i=0; i < data.length; i++){
                            if (data[i].BenefitItemID == itemID){
                                data.splice(i, 1, benefitItemData);
                                LoadBenefitList();
                                return;
                            }
                        }
                        // Или это новая льгота, раз ее нет в списке уже имеющихся
                        data.push(benefitItemData);
                        LoadBenefitList();
                        return;
                    }

                    
                    //jQuery(el).html(jQuery(el).html().replace(/\(\d+\)/, '(' + data.length +')')); 
                }
            }).dialog('open');

            
            return false;
		}

        function blDeleteBenefitButton(el)
        {
            var $tr = jQuery(el).parents('tr:first');

            var row = $tr[0].rowIndex; //$tr.attr('itemID');
            if (row && row >= 0){
                this.data.splice(row - 1, 1);
            }

            $tr.remove().detach();
            jQuery('#trAddNewBenefit').show();

            return false;
        }

    </script> 
</div>

<div id="divAddBenefit" style="display:none;">
    <div id="divErrorBlockOl" style="overflow:hidden"></div>
    <table class="gvuzData">
	    <tbody>
            <tr>
			    <td colspan="4" style="padding-bottom: 8px;">
				    <b><%=Html.LabelFor(m => m.EntranceTestItemsEdit.BenefitItemColumns.BenefitName)%>:</b>&nbsp;<label id="lblBenefitTypeID"></label><br />
			    </td>
		    </tr>
		    <tr>
			    <td>
				    <b><%=Html.LabelFor(m => m.EntranceTestItemsEdit.BenefitItemColumns.DiplomType )%>:</b>
			    </td>
			    <td>
				    <b><%=Html.LabelFor(m => m.EntranceTestItemsEdit.BenefitItemColumns.OlympicLevelFlags)%>:</b>
			    </td>
                <td>
				    <b><%=Html.LabelFor(m => m.EntranceTestItemsEdit.BenefitItemColumns.ClassFlags)%>:</b>
			    </td>
			    <td>
				    <%--<b><%=Html.LabelFor(m => m.EntranceTestItemsEdit.BenefitItemColumns.BenefitName)%>:</b>--%>
                    <b>Профили:</b>
			    </td>
		    </tr>
		    <tr>
			    <td valign="top" id="tbDiplomaType" nowrap="nowrap" style="width:130px;">
				    <input type="checkbox" id="cbDiplomaTypePrize" />
				    <label for="cbDiplomaTypePrize">Призер</label><br />
				    <input type="checkbox" id="cbDiplomaTypeWinner" />
				    <label for="cbDiplomaTypeWinner">Победитель</label><br />

			    </td>
			    <td valign="top" id="tbOlympic" style="width:160px;">
				    <input type="checkbox" id="cbOlympic0" onclick="doLevelChange(this.id)"  /><label for="cbOlympic0">Все уровни</label><br />
				    <input type="checkbox" id="cbOlympic1" onclick="cbResetOlympicList()" /><label for="cbOlympic1">I уровень</label><br />
				    <input type="checkbox" id="cbOlympic2" onclick="cbResetOlympicList()"  /><label for="cbOlympic2">II уровень</label><br />
				    <input type="checkbox" id="cbOlympic3" onclick="cbResetOlympicList()" /><label for="cbOlympic3">III уровень</label><br />
			    </td>

                <td valign="top" id="tbClass" style="width:130px;">
				    <input type="checkbox" id="cbClass0" onclick="doClassChange(this.id)"  /><label for="cbClass0">Все уровни</label><br />
				    <input type="checkbox" id="cbClass1" onclick="cbResetOlympicList()" /><label for="cbClass1">7 класс</label><br />
                    <input type="checkbox" id="cbClass2" onclick="cbResetOlympicList()" /><label for="cbClass2">8 класс</label><br />
                    <input type="checkbox" id="cbClass4" onclick="cbResetOlympicList()" /><label for="cbClass4">9 класс</label><br />
                    <input type="checkbox" id="cbClass8" onclick="cbResetOlympicList()" /><label for="cbClass8">10 класс</label><br />
                    <input type="checkbox" id="cbClass16" onclick="cbResetOlympicList()" /><label for="cbClass16">11 класс</label><br />
			    </td>

			    <td valign="top">
                    <input type="checkbox" id="chkAllProfiles" name="chkAllProfiles" onclick="doProfilesChange()" />Все профили
                    <div id="divProfiles" style="display:none; overflow:scroll; height:200px;">
                    </div>
			    </td>
		    </tr>
		    <tr>
			    <td><b>UID:</b></td>
			    <td colspan="3"><input type="text" maxlength="200" id="benefitItemUID" value=""/></td>
		    </tr>
           <%-- <tr>
			    <td><b>Год олимпиады:</b></td>
			    <td colspan="3">
                    <span id="olympicYear"></span>
			    </td>
		    </tr>--%>
            <%--<tr>
	            <td colspan="3">
		            Год олимпиады: <%=Html.DropDownListFor(c => c.OlympicYearID, new SelectList(Model.OlympicYears), new { onchange = "doOlympicYearChange();" })%>
	            </td>
            </tr>--%>
		    <tr>
			    <td colspan="4">
				    <input type="checkbox" id="IsForAllOlympic" name="IsForAllOlympic" <%--<%= Model.IsForAllOlympic ? "checked=\"checked\"" : "" %>--%/> onclick="cbAllOlympicClick()" />Все олимпиады
			    </td>
		    </tr>
		    <tr>
			    <td colspan="4">
				    <table class="gvuzDataGrid" cellpadding="3" id="tableOlympic">
					    <thead>
						    <tr>
							    <%--<th><%= Html.LabelFor(x => x.OlympicDescr.Number) %></th>
							    <th><%= Html.LabelFor(x => x.OlympicDescr.Level)%></th>
                                <th><%= Html.LabelFor(x => x.OlympicDescr.Year)%></th>
							    <th><%= Html.LabelFor(x => x.OlympicDescr.Name)%></th>--%>
                                <th>Год олимпиады</th>
                                <th>Номер в перечне</th>
                                <th>Наименование</th>
                                <th>Уровень</th>
                                <th>Профиль</th>
                                <th>Класс</th>
							    <th style="width:40px"></th>
						    </tr>
                            <tr id="trAddNewOlympic">
							    <td colspan="7">
								    <%--<a href="#" id="btnAddNewOlympic" class="add" onclick="addOlympic()">Добавить олимпиаду</a><br />--%>
								    <a href="#" id="btnAddNewOlympicMany" class="button" onclick="addMultipleOlympic();return false;">Добавить несколько олимпиад</a>
							    </td>
						    </tr>
					    </thead>
					    <tbody>
						    
					    </tbody>
				    </table>
				
			    </td>
		    </tr>

            <tr id="BenefitItemMinEgeValue">
                <td colspan="4">
                    <table>
                        <tr>
                            <td>
                                Минимальный балл ЕГЭ, необходимый для использования льготы:&nbsp; 
                            </td>
                            <td>
                                <input type="text" id="minEgeValue" value="" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            
            <tr id="BenefitItemSubjects">
                <td colspan="4">
                    <%--Минимальный балл ЕГЭ, необходимый для использования льготы<br />--%>
                    <table style="width:100%">
                        <tr>
                            <td style="width:50%">
                                <input type="checkbox" id="IsCreative" style="width: 20px;" onchange="subjectChanged()" />Творческие олимпиады
                            </td>
                            <td style="width:50%">
                                <input type="checkbox" id="IsAthletic" style="width: 20px;" onchange="subjectChanged()" />Олимпиады в области спорта 
                            </td>
                        </tr>
                        <tr id="CreativeAthleticWarning">
                            <td colspan="2">
                                Для всех олимпиад, указанных в льготе, не будет осуществляться проверка на наличие у абитуриентов результатов ЕГЭ не ниже 75 баллов
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table class="gvuzDataGrid" id="tableSubjects">
                        <thead>
                            <tr>
                                <th>Общеобразовательный предмет</th>
                                <th>Минимальный балл</th>
                                <%--<th style="width:40px"></th>--%>
                            </tr>
                            <%--<tr id="trAddSubject">
                                <td colspan="3">
								    <a href="#" id="btAddSubject" class="add" onclick="subjectAddButton()">Добавить предмет</a><br />
                                </td>
                            </tr>--%>
                            <tr>
                                <td>
                                    <span id="spanSubjectSelect"></span>
                                </td>
                                <td>
                                    <input type="text" id="SubjectMinEge"/>
                                </td>
                            </tr>
                        </thead>
                    </table>
                </td>
              </tr>
	    </tbody>
    </table>
    <div style="display:none">
	    <input id="btnSubmitAB" type="button" value="Сохранить" /> 
	    <input id="btnCancelAB" type="button" value="Отмена" />
    </div>

    <script type="text/javascript" id="/">
        jQuery('#btnCancelAB').click(function () { 
            benefitItemData.doSave = false;
            closeDialog(jQuery('#divAddBenefit')); 
        });
        jQuery('#btnSubmitAB').click(function () {
            if (!benefitItemData.doSave){
                benefitItemData.doSave = true;
                SaveBenefit();
            }
        });

        function SaveBenefit(){
            if (this.benefitItemData != null){
                FillBenefitItemModel();
                FillBenefitItemsOlympics();

                if (!ValidateBenefitItem()){
                    benefitItemData.doSave = false;
                    return;
                }

                // No need to do something special to Save here
                closeDialog(jQuery('#divAddBenefit'));
            }
        }

        // Загрузить необходимые справончики
        function InitBenefitData()
        { 
            this.Profiles = JSON.parse('<%= Html.Serialize(Model.EntranceTestItemsEdit.OlympicProfiles) %>');
            this.OlympicLevels = JSON.parse('<%= Html.Serialize(Model.EntranceTestItemsEdit.OlympicLevels) %>');
            this.OlympicDiplomTypes = JSON.parse('<%= Html.Serialize(Model.EntranceTestItemsEdit.OlympicDiplomTypes) %>');
            this.OlympicTypes = JSON.parse('<%= Html.Serialize(Model.EntranceTestItemsEdit.OlympicTypes) %>');
            this.OlympicTypeYears = JSON.parse('<%= Html.Serialize(Model.EntranceTestItemsEdit.OlympicTypeYears) %>');
            this.GlobalMinEge = JSON.parse('<%= Html.Serialize(Model.EntranceTestItemsEdit.GlobalMinEge) %>');            

            //var selectYearStart = $('#CompetitiveGroupEdit_CampaignYearStart').val();
            //var sct= '<select id="Select_Profile">';
            //var sys = "";
            //var campaignsDone = [];
            //for (var i = 0; i < Campaigns.length; i++) {
            //    if (Campaigns[i].YearStart == selectYearStart && campaignsDone.indexOf(Campaigns[i].CampaignID) < 0)
            //    {
            //        sys = sys + '<option value="'+ Campaigns[i].CampaignID +'">'+ Campaigns[i].Name +'</option>';
            //        campaignsDone.push(Campaigns[i].CampaignID);
            //    }
            //}
            //sct = sct + sys +'</select>';
            //$('#CGE_Campaigns').html(sct);
            
            var sct= '<select id="SubjectSelect" onchange="subjectChanged()"><option value="0"></option>';
            for (var i = 0; i < this.subjects.length; i++)
                if (this.subjects[i].IsEge)
                    sct += '<option value="' + this.subjects[i].SubjectID + '">' + this.subjects[i].Name + '</option>';
            sct = sct + '</select>';
            $('#spanSubjectSelect').html(sct);

            var src = '';
            for (var i = 0; i < Profiles.length; i++) {
                if (Profiles[i].Id != <%= BenefitItemViewModel.PROFILE_ALL %>){
                    src += '<br><input type="checkbox" id="cbProfile' + Profiles[i].Id + '" />' + Profiles[i].Name; 
                }
            }
            if (src.length>4)
                src = src.substring(4);
            $('#divProfiles').html(src);
            
            //src= '<select id="Select_OlympicYear">';
            //for (var i = 0; i < OlympicTypeYears.length; i++) {
            //    src += '<option value="'+ OlympicTypeYears[i] +'">'+ OlympicTypeYears[i] +'</option>';
            //}
            //src += '</select>';
            //$('#olympicYear').html(src);

        }
        

        this.benefitToInit = true;
        function LoadBenefitItemData()
        {
            // На первом открытии надо инициализировать вспомогательные списки и контролы списки
            if (this.benefitToInit){
                InitBenefitData();
                this.benefiitToInit = false;
            }


            // Загрузить данные
            if ((this.benefitItemData.OlympicDiplomTypeID & 1) > 0) 
                jQuery('#cbDiplomaTypeWinner').attr('checked', 'checked');
            else
                jQuery('#cbDiplomaTypeWinner').removeAttr('checked');

            if ((this.benefitItemData.OlympicDiplomTypeID & 2) > 0) 
                jQuery('#cbDiplomaTypePrize').attr('checked', 'checked');
            else
                jQuery('#cbDiplomaTypePrize').removeAttr('checked');

            jQuery('#cbOlympic0,#cbOlympic1,#cbOlympic2,#cbOlympic3').removeAttr('checked');
            if(!this.benefitItemData.OlympicLevelFlags || this.benefitItemData.OlympicLevelFlags == <%= BenefitItemViewModel.OLYMPIC_ALL %>)
            {
			    jQuery('#cbOlympic0').attr('checked', 'checked');
                jQuery('#cbOlympic1,#cbOlympic2,#cbOlympic3').removeAttr('checked').attr('disabled', 'disabled');
		    }
		    else
            {
			    if ((this.benefitItemData.OlympicLevelFlags & 1) != 0)
			        jQuery('#cbOlympic1').attr('checked', 'checked');
				if((this.benefitItemData.OlympicLevelFlags & 2) != 0)
			        jQuery('#cbOlympic2').attr('checked', 'checked');
				if((this.benefitItemData.OlympicLevelFlags & 4) != 0)
				    jQuery('#cbOlympic3').attr('checked', 'checked');
            }
			//doLevelChange('cbOlympic0');

			//jQuery('#cbClass0,#cbClass1,#cbClass2,#cbClass4,#cbClass8,#cbClass16').removeAttr('checked');
			$("input[id*='cbClass']").removeAttr('checked');
			if(!this.benefitItemData.ClassFlags || this.benefitItemData.ClassFlags == <%= BenefitItemViewModel.CLASS_ALL %>)
            {
			    jQuery('#cbClass0').attr('checked', 'checked');
                jQuery('#cbClass1,#cbClass2,#cbClass4,#cbClass8,#cbClass16').removeAttr('checked').attr('disabled', 'disabled');
		    }
		    else
		    {
		        if ((this.benefitItemData.ClassFlags & 1) != 0)
		            jQuery('#cbClass1').attr('checked', 'checked');
		        if((this.benefitItemData.ClassFlags & 2) != 0)
		            jQuery('#cbClass2').attr('checked', 'checked');
		        if((this.benefitItemData.ClassFlags & 4) != 0)
		            jQuery('#cbClass4').attr('checked', 'checked');
		        if((this.benefitItemData.ClassFlags & 8) != 0)
		            jQuery('#cbClass8').attr('checked', 'checked');
		        if((this.benefitItemData.ClassFlags & 16) != 0)
		            jQuery('#cbClass16').attr('checked', 'checked');
            }
		    //doClassChange('cbClass0');

		    jQuery('#lblBenefitTypeID')[0].innerText = (this.benefitItemData.BenefitName ? this.benefitItemData.BenefitName : '');
            
		    $("input[id*='cbProfile']").removeAttr('checked');
		    if (!this.benefitItemData.BenefitItemProfiles || this.benefitItemData.BenefitItemProfiles.length == 0 || 
                (this.benefitItemData.BenefitItemProfiles.length == 1 && this.benefitItemData.BenefitItemProfiles[0].OlympicProfileID == <%= BenefitItemViewModel.PROFILE_ALL %>)
                ) {
		        jQuery('#chkAllProfiles').attr('checked', 'checked');
		    }
		    else{

		        // заполняем выбранные профили
		        for(var i=0; i < this.benefitItemData.BenefitItemProfiles.length; i++ ){
		            if (this.benefitItemData.BenefitItemProfiles[i].OlympicProfileID != <%= BenefitItemViewModel.PROFILE_ALL %>){
		                var n = '#cbProfile' + this.benefitItemData.BenefitItemProfiles[i].OlympicProfileID.toString();
		                jQuery(n).attr('checked', 'checked');
		            }
		        }
		    }
            doProfilesChange();

            jQuery('#benefitItemUID').val(this.benefitItemData.UID);
            
            
            // Очистить таблицу /  Clear table 
            var row = jQuery('#trAddNewOlympic').prev('tr');
            while (row[0].rowIndex > 0){
                document.getElementById("tableOlympic").deleteRow(row[0].rowIndex);
                row = jQuery('#trAddNewOlympic').prev('tr');
            }

		    if (!this.benefitItemData.BenefitItemOlympics || this.benefitItemData.IsForAllOlympic){
		        jQuery('#IsForAllOlympic').attr('checked', 'checked');
		    }
		    else{
		        jQuery('#IsForAllOlympic').removeAttr('checked');
		        for(var i = 0; i < this.benefitItemData.BenefitItemOlympics.length; i++)
		            addOlympicRow(jQuery('#trAddNewOlympic'), this.benefitItemData.BenefitItemOlympics[i]);
		    }
		    cbAllOlympicClick();


		    // Очистить таблицу /  Clear table 
		    //var row = jQuery('#trAddSubject').prev('tr');
		    //while (row[0].rowIndex > 0){
		    //    document.getElementById("tableSubjects").deleteRow(row[0].rowIndex);
		    //    row = jQuery('#trAddSubject').prev('tr');
            //}

            // BenefitItemMinEgeValue // BenefitItemSubjects
		    if (this.benefitItemData.EntranceTestItemID == 0){
		        jQuery('#BenefitItemSubjects').show();
		        jQuery('#BenefitItemMinEgeValue').hide();

		        //for(var i = 0; i < this.benefitItemData.BenefitItemSubjects.length; i++)
		        //    addSubjectRow(jQuery('#trAddSubject'), this.benefitItemData.BenefitItemSubjects[i]);

		        // IsCreative, IsAthletic
		        if (this.benefitItemData.BenefitItemSubjects.length > 0){
		            jQuery('#SubjectSelect').val(this.benefitItemData.BenefitItemSubjects[0].SubjectID);
		            jQuery('#SubjectMinEge').val(this.benefitItemData.BenefitItemSubjects[0].EgeMinValue);
		        }
		        if (this.benefitItemData.IsCreative)
		            jQuery('#IsCreative').attr('checked', 'checked');
		        if (this.benefitItemData.IsAthletic)
		            jQuery('#IsAthletic').attr('checked', 'checked');

		    }
		    else{
		        jQuery('#BenefitItemSubjects').hide();
		        jQuery('#BenefitItemMinEgeValue').show();

		        jQuery('#minEgeValue').val(this.benefitItemData.EgeMinValue);
		    }
		    subjectChanged();

		    //jQuery('#trAddSubject').show();
		    jQuery('#trAddNewOlympic').show();
        }

		function subjectChanged(){
		    var subjectSelected = jQuery('#SubjectSelect').val();

		    if (subjectSelected > 0){
		        jQuery('#IsCreative').removeAttr('checked').attr('disabled', 'disabled');
		        jQuery('#IsAthletic').removeAttr('checked').attr('disabled', 'disabled');
		    }else{
		        jQuery('#IsCreative').removeAttr('disabled');
		        jQuery('#IsAthletic').removeAttr('disabled');
		    }
		    
		    if(jQuery('#IsCreative').attr('checked') || jQuery('#IsAthletic').attr('checked')){
		        jQuery('#SubjectSelect').val(0);
		        jQuery('#SubjectSelect').attr('disabled', 'disabled');
		        jQuery('#SubjectMinEge').val('');
		        jQuery('#SubjectMinEge').attr('disabled', 'disabled');
		        
		        jQuery('#CreativeAthleticWarning').show();
		    }else{
		        jQuery('#SubjectSelect').removeAttr('disabled');
		        jQuery('#SubjectMinEge').removeAttr('disabled');
		        
		        jQuery('#CreativeAthleticWarning').hide();
		    }

		}

        function cbResetOlympicList(){ /* Не надо сбрасывать выбранные ранее олимпиады! */ }

        // id checkbox-ов должно содержать подстроку Olympic0, Olympic1,..
        function doLevelChange(id){
            var strId = ('#' + id + ",#" + id + ",#" + id).replace('Olympic0', 'Olympic1').replace('Olympic0', 'Olympic2').replace('Olympic0', 'Olympic3');
            if(jQuery('#' + id).attr('checked'))
                jQuery(strId).removeAttr('checked').attr('disabled', 'disabled');
            else
                jQuery(strId).removeAttr('disabled').attr('checked', 'checked');
        }
        // id checkbox-ов должно содержать подстроку Class0, Class1, 2, 4, 8, 16!
        function doClassChange(id){
            var strId = ('#' + id + ",#" + id + ",#" + id + ",#" + id + ",#" + id).replace('Class0', 'Class1').replace('Class0', 'Class2').replace('Class0', 'Class4').replace('Class0', 'Class8').replace('Class0', 'Class16');
            if(jQuery('#' + id).attr('checked'))
                jQuery(strId).removeAttr('checked').attr('disabled', 'disabled');
            else
                jQuery(strId).removeAttr('disabled').attr('checked', 'checked');
        }

        function doProfilesChange(){
            var allProfiles = jQuery('#chkAllProfiles').attr('checked');
            $('#divProfiles').removeAttr('style').attr('style', 'display: ' + (allProfiles ? 'none' : 'block;border-style: groove;overflow-y: scroll;height: 150px;width:320px;margin-top: 10px;'));
        }

        function cbAllOlympicClick(){
            if(jQuery('#IsForAllOlympic').attr('checked'))
                jQuery('#tableOlympic').hide();
            else
                jQuery('#tableOlympic').show();
        }

        // BenefitItemOlympicType
        function addOlympicRow($trBefore, benefitItemOlympicType)
        {
            var ol = null;
            jQuery.each(this.OlympicTypes, function() {if(this.OlympicID == benefitItemOlympicType.OlympicID) {ol = this;return false;}});
            if (ol != null)
            {
                var className = $trBefore.prev().attr('class')
                if (className == 'trline1') className = 'trline2'; else className = 'trline1';

                $trBefore.before('<tr itemID="' + benefitItemOlympicType.ID + '" olympicID="' + ol.OlympicID + '" class="' + className + '">' 
                    + '<td>' + ol.OlympicYear + '</td>'
                    + '<td>' + ((ol.OlympicNumber != 0) ? ol.OlympicNumber : '') + '</td>'
                    + '<td>' + escapeHtml(ol.Name) + '</td>'
                    + '<td align="center" style="padding: 0px; width:100px;">' 
                        + ((ol.OlympicNumber != 0) ? GetOlympicTableLevels(benefitItemOlympicType.OlympicLevelFlags, benefitItemOlympicType.ID, true) : '')
                        + '</td>'
                    + '<td align="left" style="padding: 0px; width:150px;">' + GetOlympicTableProfiles(ol.OlympicTypeProfiles, benefitItemOlympicType.BenefitItemOlympicProfiles, benefitItemOlympicType.ID, true)  + '</td>'
                    + '<td align="center" style="padding: 0px; width:100px;">' + GetOlympicTableClasses(benefitItemOlympicType.ClassFlags, benefitItemOlympicType.ID, true, ol.OlympicNumber) + '</td>'
                    + '<td align="center" nowrap="nowrap"><a href="#" class="btnDelete" onclick="blDeleteButton(this)">&nbsp;</a>' 
                    + '</td></tr>');

                //doLevelChange('cbOlympic0TableLevel_' + benefitItemOlympicType.ID);
                //doClassChange('cbClass0TableLevel_' + benefitItemOlympicType.ID);
                doProfilesTableChange('cbProfile_' + benefitItemOlympicType.ID + '_255');

                jQuery('#Select_OlympicYear').attr('disabled', 'disabled');
            }
        }
        function GetOlympicTableLevels(level, biotId, readonly)
        {
            var res = '<table class="olympic" id="OlympicTableLevel' + biotId + '" style="font-size: x-small;padding: 0px;">'
            + '<tr><td><input type="checkbox" id="cbOlympic0TableLevel_' + biotId + '" onclick="doLevelChange(this.id)"' + ((level == 255) ? 'checked="checked"' : '' ) + ' />Все</td>'
            + '<td><input type="checkbox" id="cbOlympic1TableLevel_' + biotId + '" ' + ((level != 255 && (level & 1) > 0) ? 'checked="checked "' : '' ) + ((level == 255) ? ' disabled="disabled" ' : '' ) + ' />I</td></tr>'
            + '<tr><td></td><td><input type="checkbox" id="cbOlympic2TableLevel_' + biotId + '" ' + ((level != 255 && (level & 2) > 0) ? 'checked="checked"' : '' ) + ((level == 255) ? ' disabled="disabled" ' : '' ) + ' />II</td></tr>'
            + '<tr><td></td><td><input type="checkbox" id="cbOlympic3TableLevel_' + biotId + '" ' + ((level != 255 && (level & 4) > 0) ? 'checked="checked"' : '' ) + ((level == 255) ? ' disabled="disabled" ' : '' ) + ' />III</td></tr>'
            + '</table>';
            return res;
        }
        function GetOlympicTableClasses(classFlags, biotId, readonly, olympicNumber)
        {
            var vsosh = !olympicNumber;

            var res = '<table class="olympic" id="OlympicTableClass' + biotId + '" style="font-size: x-small;padding: 0px;">'
            + '<tr><td>'
                + (1==0 ? '' : '<input type="checkbox" id="cbClass0TableLevel_' + biotId + '" onclick="doClassChange(this.id)"' + ((classFlags == 255) ? 'checked="checked"' : '' ) + ' />Все') 
            + '</td>'
            + '<td><input type="checkbox" id="cbClass4TableLevel_' + biotId + '" ' + ((classFlags != 255 && (classFlags & 4) > 0) ? 'checked="checked"' : '' ) + ((classFlags == 255) ? ' disabled="disabled" ' : '' ) + ' />9</td></tr>'
            + '<tr><td>' 
                + (vsosh ? '' : '<input type="checkbox" id="cbClass1TableLevel_' + biotId + '" ' + ((classFlags != 255 && (classFlags & 1) > 0 && !vsosh) ? 'checked="checked"' : '' ) + ((classFlags == 255) ? ' disabled="disabled" ' : '' ) + ' />7')
            + '</td> '
            + '<td><input type="checkbox" id="cbClass8TableLevel_' + biotId + '" ' + ((classFlags != 255 && (classFlags & 8) > 0) ? 'checked="checked"' : '' ) + ((classFlags == 255) ? ' disabled="disabled" ' : '' ) + ' />10</td></tr>'
            + '<tr><td>' 
                + (vsosh ? '' : '<input type="checkbox" id="cbClass2TableLevel_' + biotId + '" ' + ((classFlags != 255 && (classFlags & 2) > 0 && !vsosh) ? 'checked="checked"' : '' ) + ((classFlags == 255) ? ' disabled="disabled" ' : '' ) + ' />8')
            + '</td> '
            + '<td><input type="checkbox" id="cbClass16TableLevel_' + biotId + '" ' + ((classFlags != 255 && (classFlags & 16) > 0) ? 'checked="checked"' : '' ) + ((classFlags == 255) ? ' disabled="disabled" ' : '' ) + ' />11</td></tr>'
            + '</table>';

            return res;
        }
        function GetOlympicTableProfiles(olympicTypeProfiles, benefitItemOlympicProfiles, biotId, readonly)
        {
            var profileAll = <%= BenefitItemViewModel.PROFILE_ALL %>;
            var benefitProfiles = [];
            if(!benefitItemOlympicProfiles || benefitItemOlympicProfiles.length==0)
                benefitProfiles.push(profileAll);
            else
                jQuery.each(benefitItemOlympicProfiles, function() {
                    if (this.OlympicProfileID != profileAll)
                        benefitProfiles.push(this.OlympicProfileID);
                    else{
                        benefitProfiles = [];
                        benefitProfiles.push(profileAll);
                        return false;
                    }
                });

            var res = '<table class="olympic" id="OlympicTableProfile' + biotId + '" style="font-size: x-small;padding: 0px; width:100%">'
            + '<tr><td><input type="checkbox" id="cbProfile_' + biotId + '_255" onclick="doProfilesTableChange(this.id)"' + ((benefitProfiles[0] == profileAll) ? 'checked="checked"' : '' ) + ' />Все</td></tr>'
            for(var i = 0; i < olympicTypeProfiles.length; i++ ){
                if (olympicTypeProfiles[i].OlympicProfileID != 0){
                    var checked = benefitProfiles.indexOf(olympicTypeProfiles[i].OlympicProfileID) >= 0;
                    res+= '<tr><td><input type="checkbox" id="cbProfile_' + biotId + '__' + olympicTypeProfiles[i].OlympicProfileID + '" ' + ((checked) ? 'checked="checked"' : '' ) + ' />' + olympicTypeProfiles[i].ProfileName + '</td></tr>';
                }
            }
            res += '</table>';
            return res;
        }
        function doProfilesTableChange(id){
            // $("input[id*='cbProfile']").removeAttr('checked');
            // id = cbProfile_1_255
            var strId = id.replace('_255', '__');
            if(jQuery('#' + id).attr('checked'))
                $("input[id*='" + strId + "']").removeAttr('checked').attr('disabled', 'disabled');
            else
                $("input[id*='" + strId + "']").removeAttr('disabled'); // .attr('checked', 'checked') ? Если нужно при снятии галки Все ставить галки всем чекбоскам 
        }

        function GetOlympicsToAdd(){

            var res = [];
            for(var i=0; i< this.OlympicTypes.length; i++){
                var o = this.OlympicTypes[i];
                var check = true;
                
                //if (o.OlympicYear != this.benefitItemData.OlympicYear) {continue; }

                for (var n=0; n< this.benefitItemData.BenefitItemOlympics.length; n++){
                    if (o.OlympicID == this.benefitItemData.BenefitItemOlympics[n].OlympicID){
                        check = false;
                        break;
                    }
                }
                if (!check) continue;

                // Проверить, что есть хотя бы один из профилей из тех, что выбраны и собрать доступные уровни
                var anyProfile = (this.benefitItemData.BenefitItemProfiles.length==1 && this.benefitItemData.BenefitItemProfiles[0].OlympicProfileID == <%= BenefitItemViewModel.PROFILE_ALL %> );
                var allowedLevels = [];

                // #warning: Хардкод OlympicLevelID!
                var mustHaveLevels = [];
                if ((this.benefitItemData.OlympicLevelFlags & 1) > 0) mustHaveLevels.push(2);
                if ((this.benefitItemData.OlympicLevelFlags & 2) > 0) mustHaveLevels.push(3);
                if ((this.benefitItemData.OlympicLevelFlags & 4) > 0) mustHaveLevels.push(4);
                if (this.benefitItemData.OlympicLevelFlags == <%= BenefitItemViewModel.OLYMPIC_ALL %>) mustHaveLevels.push(1);
                    

                for(var j=0; j < o.OlympicTypeProfiles.length; j++){
                    var level = o.OlympicTypeProfiles[j].OlympicLevelID;
                    if (level != 0){
                        if (mustHaveLevels.indexOf(level) >= 0)
                            if (allowedLevels.indexOf(level) == -1)
                                allowedLevels.push(level);

                        if (!anyProfile){
                            for(var k=0; k < this.benefitItemData.BenefitItemProfiles.length; k++){
                                if (o.OlympicTypeProfiles[j].OlympicProfileID == this.benefitItemData.BenefitItemProfiles[k].OlympicProfileID)
                                    anyProfile = true;
                            }
                        }
                    }
                }
                if (!anyProfile) {check = false; continue;}
                if (allowedLevels.length==0 && mustHaveLevels.indexOf(1) < 0) {check = false; continue; }

                if (check){ res.push(o);}
            }

            this.OlympicsToAdd = res;
            return res;
        }

        function blDeleteButton(el)
        {
            var $tr = jQuery(el).parents('tr:first');

            var row = $tr[0].rowIndex; //$tr.attr('itemID');
            if (row && row >= 0){
                this.benefitItemData.BenefitItemOlympics.splice(row - 1, 1);
            }

            $tr.remove().detach();
            jQuery('#trAddNewOlympic').show();

            if (this.benefitItemData.BenefitItemOlympics.length == 0)
                jQuery('#Select_OlympicYear').removeAttr('disabled');
            return false;
        }

        this.BenefitItemOlympicID = -1;
        function blSaveButton(el)
        {
            var $tr = jQuery(el).parents('tr:first');
            
            var selVal = jQuery('.blOlympicSelect').val();
            clearValidationErrors($tr);
            var olympic = null;
            for(var i = 0; i < this.OlympicTypes.length; i++){
                var olympicName = ((this.OlympicTypes[i].OlympicNumber != 0) ? this.OlympicTypes[i].OlympicNumber.toString() + '.' : '') + escapeHtml(this.OlympicTypes[i].Name);
                if(olympicName == selVal){
                    olympic = this.OlympicTypes[i];
                }
            }
            if(olympic == null)
            {
                jQuery('.blOlympicSelect').addClass('input-validation-error');
                jQuery('.blOlympicSelect').next().after('<br/><span class="field-validation-error">Необходимо ввести корректное название олимпиады</span>');
                return false;
            }
            $tr.remove().detach();
            jQuery('#trAddNewOlympic').show();

            AddBenefitItemOlympic(olympic);
            return false;
        }

        function AddBenefitItemOlympic(olympic)
        {

            var benefitOlympicData = {
                ID: this.BenefitItemOlympicID--,
                OlympicID: olympic.OlympicID,
                OlympicNumber: olympic.OlympicNumber,
                Name: olympic.Name,
                OlympicLevelFlags: ((olympic.OlympicNumber != 0) ? this.benefitItemData.OlympicLevelFlags : 0),
                ClassFlags: this.benefitItemData.ClassFlags,
                OlympicYear: olympic.OlympicYear,
                ProfileNames: '',
                BenefitItemOlympicProfiles: this.benefitItemData.BenefitItemProfiles
            };
            if (!this.benefitItemData.IsForAllOlympic)
                this.benefitItemData.BenefitItemOlympics.push(benefitOlympicData);
            addOlympicRow(jQuery('#trAddNewOlympic'), benefitOlympicData);
        }

        //function addOlympic()
        //{
        //    jQuery('#trAddNewOlympic').before('<tr><td colspan="6">' 
        //            //+ '<select class="blOlympicSelect"></select></td>'
        //            + '<input type="text" class="blOlympicSelect"></input></td>'
        //            + '<td align="center" class="trUnsaved" nowrap="nowrap">'
        //            + '<a href="#" class="btnSave btnSaveBI" onclick="blSaveButton(this)">&nbsp;</a>&nbsp;'
        //            + '<a href="#" class="btnDelete" onclick="blDeleteButton(this)">&nbsp;</a>'
        //            + '</td></tr>');


        //    // Отобрать все доступные олимпиады по полям формы
        //    FillBenefitItemModel();
        //    //olympicsToAdd = GetOlympicsToAdd();
        //    GetOlympicsToAdd();

        //    if(OlympicsToAdd.length == 0)
        //    {
        //        jQuery('.blOlympicSelect').val('Нет доступных олимпиад');
        //        jQuery('.blOlympicSelect').attr('disabled', 'disabled');
        //        jQuery('.btnSaveBI').hide();
        //    }
        //    else
        //    {
        //        autocompleteDropdown(jQuery('.blOlympicSelect'), {source: function(ui, response) 
        //        {
        //            var res = [];
        //            var x = ui.term.toUpperCase();
        //            for(var i = 0; i < OlympicsToAdd.length; i++)
        //                if(OlympicsToAdd[i].Name.toUpperCase().indexOf(x) >= 0)
        //                {
        //                    var olympicName = ((OlympicsToAdd[i].OlympicNumber != 0) ? OlympicsToAdd[i].OlympicNumber.toString() + '.' : '') + escapeHtml(OlympicsToAdd[i].Name) + ' (' + OlympicsToAdd[i].OlympicYear + ')';
        //                    res.push(olympicName);
        //                    //if(res.length > 10) break;
        //                }
        //            response(res);
        //        }, minLength: 2});
        //    }
        //    jQuery('#trAddNewOlympic').hide();
        //}

        function addMultipleOlympic()
        {
            var res = '<div style="padding: 0px;overflow:hidden"></div>';
            res += '<div style="height:600px; overflow-y: scroll;">'
            var isAny = false

            // Отобрать все доступные олимпиады по полям формы
            FillBenefitItemModel();
            GetOlympicsToAdd();
            //olympicsToAdd = GetOlympicsToAdd();

            for (var i = 0; i < OlympicsToAdd.length; i++){
                //if(jQuery('#tableOlympic tr[olympicID="' + allOlympic[i].OlympicID + '"]').length == 0)
                //{
                var olympicName = ((OlympicsToAdd[i].OlympicNumber != 0) ? OlympicsToAdd[i].OlympicNumber.toString() + '.' : '') + escapeHtml(OlympicsToAdd[i].Name) + ' (' + OlympicsToAdd[i].OlympicYear + ')';

                res += ('<div style="padding: 3px;"><input type="checkbox" olympicID="'+ OlympicsToAdd[i].OlympicID 
                    + '" id="cbSelectOlympic' + OlympicsToAdd[i].OlympicID  + '">'
                    + '<label for="cbSelectOlympic' + OlympicsToAdd[i].OlympicID +'">'
                    + olympicName
                    + '</label></div>');
                isAny = true;
                //}
            }
            res += '</div>';

            if(!isAny) res += 'Все доступные олимпиады выбраны'

            jQuery('#divAddOlympicMultiple').dialog(
            {
                resizeable: false,
                title: 'Выберите олимпиады',
                width: 600,
                open: function() {
                    LoadOlympicFilter();
                },
                buttons: {
                    "Выбрать": function() 
                    {
                        jQuery('#divAddOlympicTable input[type="checkbox"]:checked').each(function()
                        {
                            var olID = jQuery(this).attr('olympicID');
                            // Сформировать строку для таблицы
                            var olympic = null;
                            for(var i = 0; i < OlympicTypes.length; i++)
                                if(OlympicTypes[i].OlympicID == olID)
                                {
                                    olympic = OlympicTypes[i];
                                    break;
                                }
                            if (olympic != null)
                                AddBenefitItemOlympic(olympic);
                        });
					
                        closeDialog(jQuery('#divAddOlympicMultiple'));
                        //jQuery('#divAddOlympicTable').children().remove().detach();
                    },
                    "Отмена": function() {
                        closeDialog(jQuery('#divAddOlympicMultiple'));
                        //jQuery('#divAddOlympicTable').children().remove().detach();
                        //jQuery('#divAddOlympicMultiple').children().remove().detach();
                    }
                },
                modal: true
            })
            return false;
        }
        
        // BenefitItemSubject
        <%--function addSubjectRow($trBefore, benefitItemSubject)
        {
            $trBefore.before(
                '<tr itemId="' + benefitItemSubject.ID + '"><td subjectId="' + benefitItemSubject.SubjectID + '">' + benefitItemSubject.SubjectName + '</td><td class="egemin">' + benefitItemSubject.EgeMinValue.toString() + '</td>' +
                '<td align="center"><a href="#" class="btnEdit" onclick="subjectEditButton(this)"><img src="<%= Url.Images("edit_16.gif") %>" alt="edit" /></a>&nbsp;<a href="#" class="btnDelete"><img src="<%= Url.Images("delete_16.gif") %>" alt="delete" onclick="subjectDeleteButton(this)" /></a></td></tr>'
            );
        }

        function subjectDeleteButton(el)
        {
            var $tr = jQuery(el).parents('tr:first');

            var row = $tr[0].rowIndex; //$tr.attr('itemID');
            if (row && row >= 0){
                this.benefitItemData.BenefitItemSubjects.splice(row - 1, 1);
            }

            $tr.remove().detach();
            jQuery('#trAddSubject').show();
            return false;
        }

        function subjectEditButton(row)
        {
            var jRow = jQuery(row).parents('tr:first');
            var itemId = jRow.attr('itemid');
            var currentSubjectId = jRow.children('td[subjectid]').attr('subjectid');
        
            var minEge = jRow.children('.egemin').text();
            var options = '';

            var btns = '<td align="center" class="trUnsaved" nowrap="nowrap"><a href="#" class="btnSave btnSaveSubject" onclick="subjectSaveButton(this)">&nbsp;</a>&nbsp;<a href="#" class="btnDelete" onclick="subjectCancelButton(this)">&nbsp;</a></td>'

            for (var i = 0; i < this.subjects.length; i++)
                if (this.subjects[i].IsEge)
                    options += '<option value="' + this.subjects[i].SubjectID + '"' + ((this.subjects[i].SubjectID == currentSubjectId) ? ' selected="selected">' : '>') + this.subjects[i].Name + '</option>';

            var rowToAdd = '<tr itemid="' + itemId + '"><td><select id="subjectSelect"><options>' + options + '</options></select></td><td><input type="text" id="subjectMinEge" value="' + minEge + '"/></td>' + btns + '</tr>';
            jRow.before(rowToAdd);
            jRow.remove().detach();
        }
        function subjectAddButton()
        {
            var addRow = jQuery('#trAddSubject');
            var options = '';
            var btns = '<td align="center" class="trUnsaved" nowrap="nowrap"><a href="#" class="btnSave btnSaveSubject" onclick="subjectSaveButton(this)">&nbsp;</a>&nbsp;<a href="#" class="btnDelete" onclick="subjectCancelButton(this)">&nbsp;</a></td>'

            for (var i = 0; i < this.subjects.length; i++)
                if (this.subjects[i].IsEge)
                    options += '<option value="' + this.subjects[i].SubjectID + '">' + this.subjects[i].Name + '</option>';

            var rowToAdd = '<tr><td><select id="subjectSelect"><options>' + options + '</options></select></td><td><input type="text" id="subjectMinEge"/></td>' + btns + '</tr>';

            addRow.before(rowToAdd);
            addRow.hide();
        }

        function subjectCancelButton(el)
        {
            var jRow = jQuery(el).parents('tr:first');
            var itemId = jRow.attr('itemid');

            var index = jRow[0].rowIndex;

            if (itemId == null)
            {
                jRow.remove().detach();
                jQuery('#trAddSubject').show();
            }
            else{
                addSubjectRow(jRow, this.benefitItemData.BenefitItemSubjects[index-1]);
                jRow.remove().detach();
            }

            return false;
        }--%>

        //benefitItemSubjectID = -1;
        //function subjectSaveButton(el)
        //{
        //    var jRow = jQuery(el).parents('tr:first');
        //    var itemid = jRow.attr('itemid');
        //    var rowIndex = jRow[0].rowIndex;
        //    //var subjectSelector = jQuery('#subjectSelect');
        //    //var subjectId = subjectSelector.val();
        //    //var subjectName = subjectSelector.children('option[value=' + subjectId + ']').text();
        //    //var minEgeValue = jQuery('#subjectMinEge').val();

        //    var benefitItemSubject = {
        //        ID: (itemid != null) ? itemid : benefitItemSubjectID--,
        //        SubjectID: jQuery('#SubjectSelect').val(),
        //        SubjectName: jQuery('#SubjectSelect').children('option[value=' + jQuery('#SubjectSelect').val() + ']').text(),
        //        EgeMinValue: jQuery('#SubjectMinEge').val()
        //    };
        //    // Валидация
        //    var isError = false;
        //    if (benefitItemSubject.SubjectID == 0){
        //        jQuery('#SubjectSelect').removeClass('input-validation-error-fixed').addClass('input-validation-error');
        //        jQuery('#SubjectSelect').after('<span class="field-validation-error"><br/>Предмет должен быть выбран</span>');
        //        isError = true;
        //    }
            
        //    // Дважды один предмет?
        //    jQuery.each(benefitItemData.BenefitItemSubjects, function() {
        //        if(this.SubjectName == benefitItemSubject.SubjectName && this.ID != benefitItemSubject.ID) {
        //            jQuery('#SubjectSelect').removeClass('input-validation-error-fixed').addClass('input-validation-error');
        //            jQuery('#SubjectSelect').after('<span class="field-validation-error"><br/>Предмет уже выбран, выберите другой!</span>');
        //            isError = true;
        //            return false;
        //        }
        //    });

        //    // Мин. балл не может быть не числом, быть меньше 0 или больше 100
        //    var valMinEge = parseInt(jQuery('#SubjectMinEge').val());
        //    if(valMinEge < 0 || valMinEge > 100 || isNaN(valMinEge))
        //    {
        //        jQuery('#SubjectMinEge').removeClass('input-validation-error-fixed').addClass('input-validation-error');
        //        jQuery('#SubjectMinEge').after('<span class="field-validation-error"><br/>Балл должен быть целым числом от 0 до 100</span>');
        //        isError = true;
        //    }

        //    // Миниимальный балл ЕГЭ за данный год
        //    var isFromKrym = $("#IsFromKrym").is(':checked');
        //    var onlyCreative = isBenefitCreative;
        //    var year = new Date().getFullYear(); // parseInt(jQuery('#Select_OlympicYear').val());
        //    if (!isFromKrym && !onlyCreative && !isNaN(year) && !isNaN(valMinEge)) {
        //        var g = null;
        //        jQuery.each(this.GlobalMinEge, function() {if(this.EgeYear == year) {g = this;return false;}});
        //        if (g !=null && g.MinEgeScore > valMinEge){
        //            jQuery('#SubjectMinEge').removeClass('input-validation-error-fixed').addClass('input-validation-error');
        //            jQuery('#SubjectMinEge').after('<span class="field-validation-error"><br/>Минимальный балл ЕГЭ для использования льготы не может быть меньше общесистемного минимального балла ЕГЭ: ' + g.MinEgeScore + '</span>');
        //            isError = true;
        //        }
        //    }

        //    if(isError) 
        //        return false;

        //    // Прошли валидацию
        //    this.benefitItemData.BenefitItemSubjects.splice(rowIndex-1, 1, benefitItemSubject);

        //    addSubjectRow(jRow, benefitItemSubject);
        //    jRow.remove().detach();
        //    jQuery('#trAddSubject').show();

        //}

        

        function ValidateBenefitItem(){
            var isError = false;

            jQuery('#divErrorBlockOl').html();
            clearValidationErrors(jQuery('#divErrorBlockOl').parent());
            jQuery('#divErrorBlockOl').html('');
            
            // Хотя бы 1 тип диплома
            if(this.benefitItemData.DiplomType == 0 || !this.benefitItemData.DiplomType)
            {
                jQuery('#tbDiplomaType').addClass('input-validation-error');
                jQuery('#divErrorBlockOl')[0].innerHTML += '<span class="field-validation-error">Необходимо выбрать тип диплома</span><br>';
                isError = true;
            }
            else
                jQuery('#tbDiplomaType').removeClass('input-validation-error');
            
            // Хотя бы 1 уровень
            if(this.benefitItemData.OlympicLevelFlags == 0)
            {
                jQuery('#tbOlympic').addClass('input-validation-error');
                jQuery('#divErrorBlockOl')[0].innerHTML += '<span class="field-validation-error">Необходимо выбрать уровень</span><br>';
                isError = true;
            }
            else
                jQuery('#tbOlympic').removeClass('input-validation-error');

            // Хотя бы 1 класс
            if(this.benefitItemData.ClassFlags == 0)
            {
                jQuery('#tbClass').addClass('input-validation-error');
                jQuery('#divErrorBlockOl')[0].innerHTML += '<span class="field-validation-error">Необходимо выбрать класс</span><br>';
                isError = true;
            }
            else
                jQuery('#tbClass').removeClass('input-validation-error');

            // Хотя бы 1 профиль
            if (this.benefitItemData.BenefitItemProfiles.length==0){
                jQuery('#divProfiles').addClass('input-validation-error');
                jQuery('#divErrorBlockOl')[0].innerHTML += '<span class="field-validation-error">Необходимо выбрать профиль</span><br>';
                isError = true;
            }
            else 
                jQuery('#divProfiles').removeClass('input-validation-error');


            // Если не все олимпиады, то хотя бы 1 должна быть выбрана
            if (!this.benefitItemData.IsForAllOlympic && this.benefitItemData.BenefitItemOlympics.length==0){
                jQuery('#tableOlympic').addClass('input-validation-error');
                jQuery('#divErrorBlockOl')[0].innerHTML += '<span class="field-validation-error">Необходимо выбрать олимпиаду</span><br>';
                isError = true;
            }
            else 
                jQuery('#tableOlympic').removeClass('input-validation-error');
            
            // Если выбраны только олимпиады, являющиеся ВсОШ (OlympicType.OlympicNumber is NULL), то не требовать заполнения поля Минимальный балл ЕГЭ. (FIS-1476)
            var isFsosh = !this.benefitItemData.IsForAllOlympic;

            // Если олимпиады выбраны, но у них пустые уровни, классы или профили - это тоже ошибка!
            if (!this.benefitItemData.IsForAllOlympic){
                for(var i = 0; i < this.benefitItemData.BenefitItemOlympics.length; i++ ){
                    var o =  this.benefitItemData.BenefitItemOlympics[i];
                    // OlympicLevelFlags, ClassFlags, BenefitItemOlympicProfiles.length
                    
                    // id="OlympicTableLevel' + biotId + '"

                    if (o.OlympicLevelFlags == 0 && o.OlympicNumber != 0){
                        jQuery('#OlympicTableLevel' + o.ID).addClass('input-validation-error');
                        jQuery('#divErrorBlockOl')[0].innerHTML += '<span class="field-validation-error">У выбранной олимпиады необходимо указать уровень</span><br>';
                        isError = true;
                    }
                    else 
                        jQuery('#OlympicTableLevel' + o.ID).removeClass('input-validation-error');
                    

                    if (o.ClassFlags == 0){
                        jQuery('#OlympicTableClass' + o.ID).addClass('input-validation-error');
                        jQuery('#divErrorBlockOl')[0].innerHTML += '<span class="field-validation-error">У выбранной олимпиады необходимо указать класс</span><br>';
                        isError = true;
                    }
                    else 
                        jQuery('#OlympicTableClass' + o.ID).removeClass('input-validation-error');
                    
                    if (o.BenefitItemOlympicProfiles.length == 0){
                        jQuery('#OlympicTableProfile' + o.ID).addClass('input-validation-error');
                        jQuery('#divErrorBlockOl')[0].innerHTML += '<span class="field-validation-error">У выбранной олимпиады необходимо указать профиль</span><br>';
                        isError = true;
                    }
                    else 
                        jQuery('#OlympicTableProfile' + o.ID).removeClass('input-validation-error');
                    
                    if (o.OlympicNumber)
                        isFsosh = false;
                }
            }

            


            // Если общая льгота, то должен быть хотя бы 1 предмет
            if (this.benefitItemData.EntranceTestItemID == 0){
                this.benefitItemData.BenefitItemSubjects = [];
                var benefitItemSubject = {
                    //ID: (itemid != null) ? itemid : benefitItemSubjectID--,
                    SubjectID: jQuery('#SubjectSelect').val(),
                    SubjectName: jQuery('#SubjectSelect').children('option[value=' + jQuery('#SubjectSelect').val() + ']').text(),
                    EgeMinValue: parseInt(jQuery('#SubjectMinEge').val())
                };
                if (benefitItemSubject.SubjectID != 0)
                    this.benefitItemData.BenefitItemSubjects.push(benefitItemSubject);
                
                this.benefitItemData.IsCreative = jQuery('#IsCreative').is(':checked');
                this.benefitItemData.IsAthletic = jQuery('#IsAthletic').is(':checked');


                if (!isFsosh){

                    if (benefitItemSubject.SubjectID == 0 && !this.benefitItemData.IsCreative && !this.benefitItemData.IsAthletic){
                        jQuery('#tableSubjects').addClass('input-validation-error');
                        jQuery('#divErrorBlockOl')[0].innerHTML += '<span class="field-validation-error">Для олимпиад должен быть задан общеобразовательный предмет, минимально необходимый балл по которому даст право на использование льготы, или указания, что олимпиада является творческой и/или в области спорта</span><br>';
                        isError = true;
                    }

                    if (benefitItemSubject.SubjectID != 0 ){
                        // Мин. балл не может быть не числом, быть меньше 0 или больше 100
                        var valMinEge = benefitItemSubject.EgeMinValue;
                        if (valMinEge < 0 || valMinEge > 100 || isNaN(valMinEge))
                        {
                            jQuery('#SubjectMinEge').removeClass('input-validation-error-fixed').addClass('input-validation-error');
                            jQuery('#SubjectMinEge').after('<span class="field-validation-error"><br/>Балл должен быть целым числом от 0 до 100</span>');
                            isError = true;
                        }

                        // Миниимальный балл ЕГЭ за данный год
                        var isFromKrym = $("#IsFromKrym").is(':checked');
                        var onlyCreative = isBenefitCreative;
                        var year = new Date().getFullYear(); // parseInt(jQuery('#Select_OlympicYear').val());
                        if (!isFromKrym && !onlyCreative && !isNaN(year) && !isNaN(valMinEge)) {
                            var g = null;
                            jQuery.each(this.GlobalMinEge, function() {if(this.EgeYear == year) {g = this;return false;}});
                            if (g !=null && g.MinEgeScore > valMinEge){
                                jQuery('#SubjectMinEge').removeClass('input-validation-error-fixed').addClass('input-validation-error');
                                jQuery('#SubjectMinEge').after('<span class="field-validation-error"><br/>Минимальный балл ЕГЭ для использования льготы не может быть меньше общесистемного минимального балла ЕГЭ: ' + g.MinEgeScore + '</span>');
                                isError = true;
                            }
                        }
                    }
                }
            }
            else 
                jQuery('#tableSubjects').removeClass('input-validation-error');

            // Льгота "100 баллов" (по конкретному предмету)
            if (this.benefitItemData.EntranceTestItemID != 0 && !isFsosh){
                var valMinEge = parseInt(jQuery('#minEgeValue').val());
                if(valMinEge < 0 || valMinEge > 100 || (isNaN(valMinEge) /*&& (!isFsosh)*/))
                {
                    jQuery('#minEgeValue').removeClass('input-validation-error-fixed').addClass('input-validation-error');
                    jQuery('#minEgeValue').after('<span class="field-validation-error"><br/>Балл должен быть целым числом от 0 до 100</span>');
                    isError = true;
                }

                // Миниимальный балл ЕГЭ за данный год
                var isFromKrym = $("#IsFromKrym").is(':checked');
                var onlyCreative = isBenefitCreative;
                var year = new Date().getFullYear();  //parseInt(jQuery('#Select_OlympicYear').val());
                if (!isFromKrym && !onlyCreative && !isNaN(year) && !isNaN(valMinEge) /*&& !isFsosh*/) {
                    var g = null;
                    jQuery.each(this.GlobalMinEge, function() {if(this.EgeYear == year) {g = this;return false;}});
                    if (g !=null && g.MinEgeScore > valMinEge){
                        jQuery('#minEgeValue').removeClass('input-validation-error-fixed').addClass('input-validation-error');
                        jQuery('#minEgeValue').after('<span class="field-validation-error"><br/>Минимальный балл ЕГЭ для использования льготы не может быть меньше общесистемного минимального балла ЕГЭ: ' + g.MinEgeScore + '</span>');
                        isError = true;
                    }
                }
            }

            return !isError;
        }
    
        function FillBenefitItemModel(){
            // А зачем создавать новую модель, если у нас уже есть this.BenefitItemData
             
            this.benefitItemData.OlympicYear = new Date().getFullYear(); //parseInt(jQuery('#Select_OlympicYear').val()); // строку в число
            this.benefitItemData.OlympicDiplomTypeID = (jQuery('#cbDiplomaTypePrize').attr('checked') * 2 + jQuery('#cbDiplomaTypeWinner').attr('checked'));
            for(var i = 0; i< this.OlympicDiplomTypes.length; i++)
                if (this.OlympicDiplomTypes[i].Id == this.benefitItemData.OlympicDiplomTypeID)
                    this.benefitItemData.DiplomType = this.OlympicDiplomTypes[i].Name;

            this.benefitItemData.UID = jQuery('#benefitItemUID').val();
            
            this.benefitItemData.OlympicLevelFlags = 0;
            if(jQuery('#cbOlympic1').attr('checked')) this.benefitItemData.OlympicLevelFlags |= 1;
            if(jQuery('#cbOlympic2').attr('checked')) this.benefitItemData.OlympicLevelFlags |= 2;
            if(jQuery('#cbOlympic3').attr('checked')) this.benefitItemData.OlympicLevelFlags |= 4;
            if(jQuery('#cbOlympic0').attr('checked')) this.benefitItemData.OlympicLevelFlags = <%= BenefitItemViewModel.OLYMPIC_ALL %>;

            this.benefitItemData.ClassFlags = 0;
            if(jQuery('#cbClass1').attr('checked')) this.benefitItemData.ClassFlags |= 1;
            if(jQuery('#cbClass2').attr('checked')) this.benefitItemData.ClassFlags |= 2;
            if(jQuery('#cbClass4').attr('checked')) this.benefitItemData.ClassFlags |= 4;
            if(jQuery('#cbClass8').attr('checked')) this.benefitItemData.ClassFlags |= 8;
            if(jQuery('#cbClass16').attr('checked')) this.benefitItemData.ClassFlags |= 16;
            if(jQuery('#cbClass0').attr('checked')) this.benefitItemData.ClassFlags |= <%= BenefitItemViewModel.CLASS_ALL %>;

            this.benefitItemData.BenefitItemProfiles = [];
            if(jQuery('#chkAllProfiles').attr('checked'))
                this.benefitItemData.BenefitItemProfiles.push({OlympicProfileID : <%= BenefitItemViewModel.PROFILE_ALL %>});
            else {
                var checkboxes = jQuery('#divProfiles').children('input');
                for(var i=0; i< checkboxes.length; i++){
                    if (checkboxes[i].checked){
                        this.benefitItemData.BenefitItemProfiles.push({OlympicProfileID : parseInt(checkboxes[i].id.replace('cbProfile','') )});
                    }
                }
            }

            this.benefitItemData.IsForAllOlympic = jQuery('#IsForAllOlympic').attr('checked');

            var value = parseInt(jQuery('#minEgeValue').val());
            this.benefitItemData.EgeMinValue = (isNaN(value) ? 0 : value);


        }

        function FillBenefitItemsOlympics()
        {
            var rows = jQuery('#tableOlympic').children('thead').children('tr');
            for(var i = 0; i < this.benefitItemData.BenefitItemOlympics.length; i++ ){
                var o = this.benefitItemData.BenefitItemOlympics[i];
                var row = rows[i+1];

                o.OlympicLevelFlags = 0;
                // cbOlympic0TableLevel_{ID}
                if($("input[id*='cbOlympic1TableLevel_" + o.ID)){
                    if($("input[id*='cbOlympic1TableLevel_" + o.ID).attr('checked')) o.OlympicLevelFlags |= 1;
                    if($("input[id*='cbOlympic2TableLevel_" + o.ID).attr('checked')) o.OlympicLevelFlags |= 2;
                    if($("input[id*='cbOlympic3TableLevel_" + o.ID).attr('checked')) o.OlympicLevelFlags |= 4;
                    if($("input[id*='cbOlympic0TableLevel_" + o.ID).attr('checked')) o.OlympicLevelFlags = <%= BenefitItemViewModel.OLYMPIC_ALL %>;
                }

                o.ClassFlags = 0;
                // cbClass0TableLevel_{ID}
                var isVsosh = !o.OlympicNumber;

                if($("input[id*='cbClass1TableLevel_" + o.ID).attr('checked')) o.ClassFlags |= 1;
                if($("input[id*='cbClass2TableLevel_" + o.ID).attr('checked')) o.ClassFlags |= 2;
                if($("input[id*='cbClass4TableLevel_" + o.ID).attr('checked')) o.ClassFlags |= 4;
                if($("input[id*='cbClass8TableLevel_" + o.ID).attr('checked')) o.ClassFlags |= 8;
                if($("input[id*='cbClass16TableLevel_" + o.ID).attr('checked')) o.ClassFlags |= 16;
                if($("input[id*='cbClass0TableLevel_" + o.ID).attr('checked')) o.ClassFlags = (isVsosh ?  <%= BenefitItemViewModel.CLASS_ALL_VSOSH %> : <%= BenefitItemViewModel.CLASS_ALL %>);
            

                o.BenefitItemOlympicProfiles = [];
                // cbProfile_2__2 (два символа _)  // cbProfile_2_255
                if($("input[id^='cbProfile_" + o.ID + "_255']").attr('checked')){
                    o.BenefitItemOlympicProfiles.push({OlympicProfileID : <%= BenefitItemViewModel.PROFILE_ALL %>});
                    o.ProfileNames = 'Все';
                }
                else {
                    var checkboxes =  $("input[id^='cbProfile_" + o.ID + "_']");
                    o.ProfileNames = '';
                    for(var j=0; j < checkboxes.length; j++){
                        if (checkboxes[j].checked){
                            var profileID = parseInt(checkboxes[j].id.replace('cbProfile_' + o.ID + '__',''));
                            o.BenefitItemOlympicProfiles.push({ OlympicProfileID : profileID });
                            o.ProfileNames += ', ' + $('#' + checkboxes[j].id).parent('td')[0].innerText;
                        }
                    }
                    if (o.ProfileNames)
                        o.ProfileNames = o.ProfileNames.substring(2);
                }

                

            }
        }


    </script>
</div>


<div id="divViewOlympic" style="padding: 5px;display:none;position:absolute" class="ui-widget ui-widget-content ui-corner-all">
</div>
<div id="divAddOlympicMultiple" style="padding: 5px;display:none;"> 
    Наименование: <input id="filterName" type="text" onkeyup="filterOlympic()" onpaste="filterOlympic()" style="width:200px;"/>
    &nbsp;&nbsp;&nbsp;Год: <select id="filterYear" onchange="filterOlympic()" style="width:80px;"></select>
    
    <div id="divAddOlympicTable" style="width: 100%" >
    </div>
        <script language="javascript" type="text/javascript">
            var LoadOlympicFilter = function(){ 
                var olympicYearStart = GetOlympicYearStart(); 
               
                var src='';
                for (var i = 0; i < OlympicTypeYears.length; i++) {
                    if(OlympicTypeYears[i] > olympicYearStart)//#FIS-1723
                    {
                        src += '<option value="'+ OlympicTypeYears[i] +'">'+ OlympicTypeYears[i] +'</option>';
                    }
                }
                jQuery('#filterYear').html(src); 

                filterOlympic();
            }

            var filterOlympic = function()
            {
                if (!OlympicsToAdd) return;

                var year = jQuery('#filterYear').val();
                var name = jQuery('#filterName').val();

                var res = '<div style="padding: 0px;overflow:hidden"></div>';
                res += '<div style="height:600px; overflow-y: scroll;">'
                var isAny = false;
                for (var i = 0; i < OlympicsToAdd.length; i++){
                    //if(jQuery('#tableOlympic tr[olympicID="' + allOlympic[i].OlympicID + '"]').length == 0)
                    //{
                    var olympicName = ((OlympicsToAdd[i].OlympicNumber != 0) ? OlympicsToAdd[i].OlympicNumber.toString() + '.' : '') + escapeHtml(OlympicsToAdd[i].Name) + ' (' + OlympicsToAdd[i].OlympicYear + ')';

                    if (
                            (!year || OlympicsToAdd[i].OlympicYear.toString() == year)
                            && 
                            (!name || olympicName.toUpperCase().indexOf(name.toUpperCase()) >= 0)
                        )
                    {                        
                        res += ('<div style="padding: 3px;"><input type="checkbox" olympicID="'+ OlympicsToAdd[i].OlympicID 
                            + '" id="cbSelectOlympic' + OlympicsToAdd[i].OlympicID  + '">'
                            + '<label for="cbSelectOlympic' + OlympicsToAdd[i].OlympicID +'">'
                            + olympicName
                            + '</label></div>');
                        isAny = true;
                    }
                }
                res += '</div>';

                if(!isAny) res += 'Все доступные олимпиады выбраны'
                jQuery('#divAddOlympicTable').html(res);

                //for(var i = 0; i< OlympicsToAdd.length; i++)
                //{

                //}
                

            }
        </script> 
</div>

<script type="text/javascript">
    function CheckMainTableEnabled()
    {
        var total = this.entranceTests.length + this.entranceTestsCreative.length + this.entranceTestsProfile.length + this.targets.length;
        //if (total > 0 )
        //{
        //    jQuery('.tableAdmin input').addClass('view').attr('disabled', 'disabled');
        //    jQuery('.tableAdmin select').addClass('view').attr('disabled', 'disabled'); 

        //    if (!readOnly) {
        //        $('#CompetitiveGroupEdit_Uid').removeClass('view').removeAttr('disabled');
        //        $('#CompetitiveGroupEdit_CompetitiveGroupName').removeClass('view').removeAttr('disabled');

        //        if (!this.targetItemsMode){
        //            $('#CompetitiveGroupEdit_Value')[0].disabled = false;
        //            $('#CompetitiveGroupEdit_Value').removeClass('view');
        //        }
        //    }
        //}
        //else 
        //{
        //    jQuery('.tableAdmin input').removeClass('view').removeAttr('disabled');
        //    jQuery('.tableAdmin select').removeClass('view').removeAttr('disabled');
        //    DisableLevelBudgetCheck();
        //    DisableKrymCheck();
        //}

        
        //$('#CompetitiveGroupEdit_Value')[0].disabled = (this.isTargetVisible ? true : false); // Это вовсе не говнокод, это js! isTargetVisible м.б. undefined 
    }

    function doCancel(navUrl)
    {
        window.location =  !!navUrl ? navUrl : '<%= Url.Generate<CompetitiveGroupController>(x => x.CompetitiveGroupList()) %>'; 
    }

    // Валидация полей главной формы
    // Возвращает null есть есть ошибки и модель, если нет ошибок
    function ValidateMain()
    {
        clearValidationErrors(jQuery('.tableAdmin'));
        var c = this.campaign;

        var isError = false;
        var modelCG =
         {
                CompetitiveGroupID: <%= Model.CompetitiveGroupEdit.CompetitiveGroupID %>,
                CompetitiveGroupName: $('#CompetitiveGroupEdit_CompetitiveGroupName').val(),
                Uid: $('#CompetitiveGroupEdit_Uid').val(),
                CampaignID: $("#Select_Campaigns").val(),
                IsFromKrym: $("#IsFromKrym").is(':checked'),
                IsAdditional: $("#IsAdditional").is(':checked'),
                EducationFormID: $('#Select_EducationForms').val(), 
                EducationSourceID: $('#CompetitiveGroupEdit_EducationSourceID').val(),
                EducationLevelID: $("#Select_EducationLevels").val(), 
                ParentDirectionID:  $("#Select_Directions").val(),
                
                DirectionID:  $("#Select_Directions").val() == null ? null : $("#Select_Directions").val(),
                Value: parseInt($('#CompetitiveGroupEdit_Value').val()),
                IdLevelBudget: $('#Select_LevelBudgets').val(), 

                StudyBeginningDate: $("#CompetitiveGroupEdit_StudyBeginningDate").val(),
                StudyEndingDate: $("#CompetitiveGroupEdit_StudyEndingDate").val(),
                StudyPeriod: $("#CompetitiveGroupEdit_StudyPeriod").val(),
                Properties: this.Properties,

                IsMVD: this.isMVD,
                IsMultiProfile: this.IsMultiProfile,


        };



        //console.log(modelCG)
        
        //this.isMVD = JSON.parse('<%= Html.Serialize(Model.CompetitiveGroupEdit.IsMVD) %>');

        // наименование - обязательное
        if (!modelCG.CompetitiveGroupName || modelCG.CompetitiveGroupName == ''){
            $('#CompetitiveGroupEdit_CompetitiveGroupName').removeClass('input-validation-error-fixed').addClass('input-validation-error');
            $('#CompetitiveGroupEdit_CompetitiveGroupName').after('<span class="field-validation-error"><br/>Необходимо заполнить наименование</span>');
            isError = true;
        }
        // год и тип ПК - обязательное
        if (!modelCG.CampaignID || modelCG.CampaignID == 0){
            $('#Select_Campaigns').removeClass('input-validation-error-fixed').addClass('input-validation-error');
            $('#Select_Campaigns').after('<span class="field-validation-error"><br/>Необходимо выбрать приемную кампанию</span>');
            isError = true;
        }
        // уровень образования - обязательное
        if (!modelCG.EducationLevelID || modelCG.EducationLevelID == 0){
            $('#Select_EducationLevels').removeClass('input-validation-error-fixed').addClass('input-validation-error');
            $('#Select_EducationLevels').after('<span class="field-validation-error"><br/>Необходимо указать уровень образования</span>');
            isError = true;
        }
        // источник финансирвоания - обязательное 
        if (!modelCG.EducationSourceID || modelCG.EducationSourceID == 0){
            $('#CompetitiveGroupEdit_EducationSourceID').removeClass('input-validation-error-fixed').addClass('input-validation-error');
            $('#CompetitiveGroupEdit_EducationSourceID').after('<span class="field-validation-error"><br/>Необходимо указать источник финансирования</span>');
            isError = true;
        }
        // форма обучения - обязательное 
        if (!modelCG.EducationFormID || modelCG.EducationFormID == 0){
            $('#Select_EducationForms').removeClass('input-validation-error-fixed').addClass('input-validation-error');
            $('#Select_EducationForms').after('<span class="field-validation-error"><br/>Необходимо указать форму обучения</span>');
            isError = true;
        }

        // Направление подготовки - обязательное 
        if (!modelCG.DirectionID || modelCG.DirectionID == 0){
            $('#Select_Directions').removeClass('input-validation-error-fixed').addClass('input-validation-error');
            $('#Select_Directions').after('<span class="field-validation-error"><br/>Необходимо указать направление подготовки</span>');
            isError = true;
        }

        // Уровень бюджета - обязательное, если не платка и не иностранцы!
        if (
            (!modelCG.IdLevelBudget || modelCG.IdLevelBudget == 0)
            && modelCG.EducationSourceID != 15 
            && c.CampaignTypeID != <%= GVUZ.DAL.Dapper.ViewModel.Dictionary.CampaignTypesView.Foreigners  %>
            && $("#CompetitiveGroupEdit_CampaignYearStart").val() > 2017
            ) {
            $('#Select_LevelBudgets').removeClass('input-validation-error-fixed').addClass('input-validation-error');
            $('#Select_LevelBudgets').after('<span class="field-validation-error"><br/>Необходимо указать уровень бюджета</span>');
            isError = true;
        }

        // UID должен быть уникален
        if (modelCG.Uid && modelCG.Uid != ''){
            var sameUid = null;
            for (var i = 0; i< this.CompetitiveGroupUIDs.length; i++){
                if (modelCG.CompetitiveGroupID != this.CompetitiveGroupUIDs[i].ID && modelCG.Uid == this.CompetitiveGroupUIDs[i].UID)
                    sameUid = this.CompetitiveGroupUIDs[i];
            }

            if (sameUid != null){
                $('#CompetitiveGroupEdit_Uid').removeClass('input-validation-error-fixed').addClass('input-validation-error');
                $('#CompetitiveGroupEdit_Uid').after('<span class="field-validation-error"><br/>В данной ОО уже имеется конкурс с таким же UID</span>');
                isError = true;
            }
        }

        // CompetitiveGroupEdit_Value
        if ((!(modelCG.Value === parseInt(modelCG.Value))) || parseInt(modelCG.Value)  < 0 ){
            $('#CompetitiveGroupEdit_Value').removeClass('input-validation-error-fixed').addClass('input-validation-error');
            $('#CompetitiveGroupEdit_Value').after('<span class="field-validation-error"><br/>Количество мест должно быть целым неотрицательным числом!</span>');
            isError = true;

        }

        //Период  обучения  - обязательное 
        if (modelCG.StudyPeriod == 0 || modelCG.StudyPeriod== '')
        {
            $('#CompetitiveGroupEdit_StudyPeriod').removeClass('input-validation-error-fixed').addClass('input-validation-error');
            $('#CompetitiveGroupEdit_StudyPeriod').after('<span class="field-validation-error"><br/>Необходимо указать количество месяцев</span>');
            isError = true;
        }
        if ((!(modelCG.StudyPeriod == parseInt(modelCG.StudyPeriod))) || parseInt(modelCG.StudyPeriod) < 0)
        {
            $('#CompetitiveGroupEdit_StudyPeriod').removeClass('input-validation-error-fixed').addClass('input-validation-error');
            $('#CompetitiveGroupEdit_StudyPeriod').after('<span class="field-validation-error"><br/>Количество месяцев должно быть целым неотрицательным числом!</span>');
            isError = true;
        }

        for (var propertyIndex = 0; propertyIndex < modelCG.Properties.length; propertyIndex++) {


            if (modelCG.Properties[propertyIndex].PropertyTypeCode == 1) modelCG.Properties[propertyIndex].PropertyValue = modelCG.StudyPeriod;
            if (modelCG.Properties[propertyIndex].PropertyTypeCode == 2) modelCG.Properties[propertyIndex].PropertyValue = modelCG.StudyBeginningDate;
            if (modelCG.Properties[propertyIndex].PropertyTypeCode == 3) modelCG.Properties[propertyIndex].PropertyValue = modelCG.StudyEndingDate;
        }
        ////Период обучения - проверка на вхождение в диапазон
        ////if (modelCG.StudyBeginningDate != StringIsEmpty)
        ////{
        //var dt1 = new Date($("#CompetitiveGroupEdit_StudyBeginningDate").val());
        //var dt2 = new Date($("#CompetitiveGroupEdit_StudyEndingDate").val());
        

        ////var period = modelCG.StudyEndingDate - modelCG.StudyBeginningDate;
        ////var period = dt2 - dt1;
        ////var diffMonths = Math.ceil(period / (1000 * 3600 * 24 * 30));

        //var studyPeriod = parseInt($("#CompetitiveGroupEdit_StudyPeriod").val());
        //if ((Math.ceil((dt2-dt1) / (1000 * 3600 * 24 * 30))) < studyPeriod)
        //{
        //    $('#CompetitiveGroupEdit_StudyPeriod').removeClass('input-validation-error-fixed').addClass('input-validation-error');
        //    $('#CompetitiveGroupEdit_StudyPeriod').after('<span class="field-validation-error"><br/>Необходимо указать количество месяцев входящее в диапазон указанных дат</span>');
        //    isError = true;
        //}
                

        //}


        //#FIS-1723 Проверим сроки действия олимпиад
        var olympicYearStart = GetOlympicYearStart();
        var hasOlympicYearError = false;
         
        for(var testIndex = 0; testIndex < this.entranceTests.length; testIndex++)
        {
            for (var benefitIndex = 0; benefitIndex < this.entranceTests[testIndex].BenefitItems.length; benefitIndex++)
            {
                for (var olympicIndex = 0; olympicIndex < this.entranceTests[testIndex].BenefitItems[benefitIndex].BenefitItemOlympics.length; olympicIndex++)
                {
                    var olympicYear = this.entranceTests[testIndex].BenefitItems[benefitIndex].BenefitItemOlympics[olympicIndex].OlympicYear;
                    if(olympicYear <= olympicYearStart)
                    {
                        hasOlympicYearError = true;
                        break;
                    }
                }
                if(hasOlympicYearError) 
                    break; 
            }
        }
         
        for (var benefitIndex = 0; benefitIndex < this.benefits.length; benefitIndex++)
        {
            for (var olympicIndex = 0; olympicIndex < this.benefits[benefitIndex].BenefitItemOlympics.length; olympicIndex++)
            {
                var olympicYear = this.benefits[benefitIndex].BenefitItemOlympics[olympicIndex].OlympicYear;
                if(olympicYear <= olympicYearStart)
                {
                    hasOlympicYearError = true;
                    break;
                }
            }
            if(hasOlympicYearError) 
                break; 
        }
       
        if(hasOlympicYearError)
        {
            isError = true;
            $("#commonErrors").addClass("field-validation-error");
            $("#commonErrors").text('В качестве льгот выбраны олимпиады, срок действия которых истек');
        }
        else 
        {
            $("#commonErrors").removeClass("field-validation-error");
            $("#commonErrors").text('');
        } 


        return isError ? null : modelCG;
    }

    function doSubmit(navUrl)
    { 
        // Валидация полей главной формы
        var modelCG = ValidateMain();
        if (modelCG == null)
            return;

        var modelPrograms = 
            {
                Programs: this.programs
            };

        var modelTargets = 
            {
                Targets: this.targets
            };

        var modelEntranceTestItems = 
            {
                TestItems: this.entranceTests,
                CreativeTestItems: this.entranceTestsCreative,
                ProfileTestItems: this.entranceTestsProfile,
                BenefitItems: this.benefits
        };


        var model = 
            {
                CompetitiveGroupEdit: modelCG
                ,CompetitiveGroupProgramsEdit: modelPrograms
                //,CompetitiveGroupTargetsEdit: modelTargets
                ,CompetitiveGroupTargetsEditResult: this.targets
                //,EntranceTestItemsEditResult: modelEntranceTestItems
                ,EntranceTestItemsEdit: modelEntranceTestItems
                //,IsMultiProfile: modelCG.IsMultiProfile

            };

        
        clearValidationErrors(jQuery('.data'))
        doPostAjax('<%= Url.Generate<CompetitiveGroupController>(x => x.CompetitiveGroupUpdate(null)) %>', JSON.stringify(model), function (data)
        {            

            if (data.IsError) { 
                for (var i = 0; i < data.Data.length.length; i++) {
                    $('#errorspan').remove();  
                }                        
                for (var i = 0; i < data.Data.length; i++) { 
                    var control = $('#'+data.Data[i].ControlID)[0];
                    if (control){

                        $('#'+data.Data[i].ControlID).addClass('input-validation-error').removeClass('input-validation-error-fixed');
                        $('#'+data.Data[i].ControlID).after( '<div id="errorspan"><span id="" class="field-validation-error">'+data.Data[i].ErrorMessage+'</span></div>' );
                    }
                    else{
                        $('#CompetitiveGroupEdit_CompetitiveGroupName').addClass('input-validation-error').removeClass('input-validation-error-fixed');
                        $('#CompetitiveGroupEdit_CompetitiveGroupName').after( '<div id="errorspan"><span id="" class="field-validation-error">'+data.Data[i].ErrorMessage+'</span></div>' );
                    }

                }

            <%--if(data.IsError && data.Message == 'df') //< %-- красивое подсвечивание ошибок --% >
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
                }--%>
                //var targetUIDErrors = data.Data.TargetUIDErrors;
                //for(var i = 0; i < targetUIDErrors.length; i++) {
                //    jQuery('#tableData tr[dirID="' + targetUIDErrors[i].DirectionID + '"][edID="' + errorsList[i].EducationLevelID + '"] td:first-child').append('<span class="field-validation-error"><br/>' + targetUIDErrors[i].Error.replace(/\n/g, '<br/>') + '</span>')
                //    var $iArr2 = jQuery('#tableData tr[dirID="' + targetUIDErrors[i].DirectionID + '"][edID="' + errorsList[i].EducationLevelID + '"] input:.targetItemUID')
                //    for(var k = 0; k < targetUIDErrors[i].ErrorIdx.length;k++)
                //    {
                //        var idx = targetUIDErrors[i].ErrorIdx[k]
                //        addValidationError(jQuery($iArr2[idx]), '')
                //    }
                //}
				
                return
            }

            if (!addValidationErrorsFromServerResponse(data, false))
            {
                doCancel(navUrl)
            }
        })
    }


    jQuery('#btnSave,#btnSaveTop').click(function ()
    {
        doSubmit();
        return false;
    })

    jQuery('#btnCancel,#btnCancelTop').click(function ()
    {
        doCancel();
    })
	
    jQuery(document).ready(function()
    {
        InitData();

        $('#CompetitiveGroupEdit_CampaignYearStart').change(function() {
            $('#CGE_Campaigns').html('');
            FillCampaigns();          
            //if ($("#CompetitiveGroupEdit_CampaignYearStart").val() != 2016) {
            //    jQuery('#IsFromKrym').removeAttr('checked').attr('disabled', 'disabled');
            //}
            //else {
            //    jQuery('#IsFromKrym').removeAttr('disabled');
            //}

            //#FIS-1723 при изменении года - ошибки про олимпиады надо спрятать
            $("#commonErrors").removeClass("field-validation-error");
            $("#commonErrors").text('');

            DisableKrymCheck();
        }); 

        $('#CGE_Campaigns').change(function() {
            DisableLevelBudgetCheck();
        }); 
        
        FillCampaigns();

        var EducationSourceID = <%= Html.Serialize(Model.CompetitiveGroupEdit.EducationSourceID) %>;
        if (EducationSourceID != 0) {
            $("#CompetitiveGroupEdit_EducationSourceID").val(EducationSourceID);
        } else {
            $('#CompetitiveGroupEdit_EducationSourceID')[0].value = 0;
        }
        $("#CompetitiveGroupEdit_EducationSourceID").change(function() {
            FillTab();
            DisableLevelBudgetCheck();
        });

        FillTab();

        //InitTargets();
        InitialTestsFillData();
       
        //if (CampaignYearStart != 2016 && CampaignYearStart != 0) {
        //    jQuery('#IsFromKrym').attr('disabled', 'disabled');          
        //}

        //DisableLevelBudgetCheck();
        
        

        <%--<% if(!Model.CompetitiveGroupEdit.CanEdit)
		   { %>
        jQuery('.content input').addClass('view').attr('readonly', 'readonly')
        jQuery('#trAddRow,#btnAddNewOrg').hide();
        jQuery('.content .btnDelete,.content .btnDeleteGray,.content .btnEdit').remove().detach();
        <% } %>--%>
    });

    function InitData()
    {
        this.Campaigns = <%= Html.Serialize(Model.Campaigns)%>;
        this.CampaignYearStart = <%= Html.Serialize(Model.CompetitiveGroupEdit.CampaignYearStart)%>;
        this.Forms = <%= Html.Serialize(Model.EducationForms)%>;
        this.Directions = <%= Html.Serialize(Model.Directions)%>;
        this.IsMultiProfile = <%= Html.Serialize(Model.IsMultiProfile) %>;
        
        this.DirectionID = <%= Html.Serialize(Model.CompetitiveGroupEdit.DirectionID) %>;
        this.ParentDirectionID = <%= Html.Serialize(Model.CompetitiveGroupEdit.ParentDirectionID) %>;
        this.educationFormID = <%= Html.Serialize(Model.CompetitiveGroupEdit.EducationFormID) %>;
        this.educationLevelID = <%= Html.Serialize(Model.CompetitiveGroupEdit.EducationLevelID) %>;
        this.EducationSourceID = <%= Html.Serialize(Model.CompetitiveGroupEdit.EducationSourceID) %>;

        this.CompetitiveGroupUIDs = <%= Html.Serialize(Model.CompetitiveGroupEdit.Uids)%>;

        this.targets = JSON.parse('<%= Html.Serialize(Model.CompetitiveGroupTargetsEdit.Targets) %>');
        this.targetOrganizations = JSON.parse('<%= Html.Serialize(Model.CompetitiveGroupTargetsEdit.TargetOrganizations) %>');
        if (targets == null) targets = [];
        if (targetOrganizations == null) targetOrganizations = [];

        this.InstitutionPrograms = JSON.parse('<%= Html.Serialize(Model.CompetitiveGroupProgramsEdit.InstitutionPrograms) %>');
        if (InstitutionPrograms == null) InstitutionPrograms = [];

        this.StudyBeginningDate = <%= Html.Serialize(Model.CompetitiveGroupEdit.StudyBeginningDate) %>;
        this.StudyEndingDate = <%= Html.Serialize(Model.CompetitiveGroupEdit.StudyEndingDate) %>;
        this.StudyPeriod = <%=Html.Serialize(Model.CompetitiveGroupEdit.StudyPeriod)%>;
        this.Properties = <%=Html.Serialize(Model.CompetitiveGroupEdit.Properties)%>;

        $("#CompetitiveGroupEdit_StudyBeginningDate").datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-2:+10'/*, maxDate: new Date()*/ });
        $("#CompetitiveGroupEdit_StudyEndingDate").datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-2:+10'/*, maxDate: new Date()*/ });

    }

    function FillTab()
    {
        this.educationSourceID = $("#CompetitiveGroupEdit_EducationSourceID").val();
        isTargetVisible = educationSourceID == <%= GVUZ.DAL.Dapper.ViewModel.Dictionary.EDSourceConst.Target %>;
        tabControl = new TabControl($('#cgSubMenu'), [
            { id: 'header-tab-programs', name: TabName(0), link: 'javascript:LoadTab(0)', enable: true, selected: true, noWrap: true },
            { id: 'header-tab-testitems', name: TabName(1), link: 'javascript:LoadTab(1)', enable: true, noWrap: true }
        ]
        );
        
        var targetTab = { id: 'header-tab-targets', name: TabName(2), link: 'javascript:LoadTab(2)', visible: isTargetVisible, enable: true, noWrap: true };
        if (isTargetVisible)
            tabControl.menuItems.splice(1,0, targetTab);

        tabControl.init();
        LoadTab(0);

        //$('#CompetitiveGroupEdit_Value')[0].disabled = isTargetVisible;
        if (isTargetVisible){

            var value = 0;
            for(var i=0; i<this.targets.length; i++) { 
                targets[i].Value = getTargetValue(targets[i]);
                value += parseInt(this.targets[i].Value); 
            }

            if (value == <%= Model.CompetitiveGroupEdit.Value %>){
                $("#valueTargetItems").attr('checked', 'checked');
            }else{
                $("#valueCompetitiveGroup").attr('checked', 'checked');
            }

            ChangeTargetValueMode();
        }
    }
    function TabName(tab)
    {
        if (tab==0) 
            return 'Образовательные программы (' + this.programs.length + ')';
        else if (tab==1)
        { 
            var total = this.entranceTests.length + this.entranceTestsCreative.length + this.entranceTestsProfile.length;
            return 'Вступительные испытания (' +  total  + ')';
        }
        else if (tab==2) 
            return 'Целевые организации (' + this.targets.length + ')';
    }

    function ChangeTargetValueMode()
    {
        this.targetItemsMode = $("#valueTargetItems").is(':checked');
        $('#CompetitiveGroupEdit_Value')[0].disabled = this.targetItemsMode;

        if (!this.targetItemsMode){
            for(var i=0; i<this.targets.length; i++) { 
                this.targets[i].NumberTargetO = 0;
                this.targets[i].NumberTargetOZ = 0;
                this.targets[i].NumberTargetZ = 0;
                this.targets[i].Value = 0; 
            }
        }


        InitTargets();

        TargetValue();
    }

    function TargetValue()
    {
        
        if (this.targetItemsMode){
            //jQuery('.targetValue').removeAttr('disabled');

            var value = 0;
            for(var i=0; i<this.targets.length; i++) { 
                value += parseInt(this.targets[i].Value); 
            }
            $('#CompetitiveGroupEdit_Value').val(value);


        }else{
            $('#CompetitiveGroupEdit_Value').removeClass('view');
            //jQuery('.targetValue').val('');
            //jQuery('.targetValue').attr('disabled', 'disabled');
            //.hide();
        }
    }

    function LoadTab(tab)
    {
        jQuery('#tab2').hide();
        for(var i=0; i < tabControl.menuItems.length; i++){
            jQuery('#tab'+i).hide();
            tabControl.menuItems[i].selected = false;
        }
       
        jQuery('#tab'+tab).show();
        var activeTab = parseInt(tab);
        if (tabControl.menuItems.length == 3 && activeTab > 0)
            activeTab = 3 - activeTab;
        tabControl.menuItems[activeTab].selected = true;
        tabControl.init();
    };

    function FillCampaigns()
    {
        var selectYearStart = $('#CompetitiveGroupEdit_CampaignYearStart').val();
        var sct= '<select id="Select_Campaigns">';
        var sys = "";


        var campaignsDone = [];
        for (var i = 0; i < Campaigns.length; i++) {
            if (Campaigns[i].YearStart == selectYearStart && campaignsDone.indexOf(Campaigns[i].CampaignID) < 0)
            {
                sys = sys + '<option value="'+ Campaigns[i].CampaignID +'">'+ Campaigns[i].Name +'</option>';
                campaignsDone.push(Campaigns[i].CampaignID);
            }
        }
        sct = sct + sys +'</select>';
        $('#CGE_Campaigns').html(sct);

        var campaignID = <%= Html.Serialize(Model.CompetitiveGroupEdit.CampaignID) %>;
        if (campaignID != 0) {
            $("#Select_Campaigns").val(campaignID);
        } else if (campaignsDone.length > 1) {
            $('#Select_Campaigns')[0].value = 0;
        }

        $("#Select_Campaigns").change(function() {
            $('#CGE_EducationLevels').html('');
            FillEducationLevel();
            FillEducationForms();
            FillEntranceTestTableHeaders();
            FillLevelBudgets();
        });
        FillEducationLevel();
        FillEducationForms();
        FillEntranceTestTableHeaders();
        FillLevelBudgets();
    };

    function FillEducationForms()
    {
        this.campaignID = $('#Select_Campaigns').val();
        this.campaign = null;
        for(var c = 0; c < Campaigns.length; c++)
        {
            if (Campaigns[c].CampaignID == campaignID)
                campaign = Campaigns[c];
        }

        var sct= '<select id="Select_EducationForms">';
        var sys = "";

        var edFormsDone = [];
        for (var i = 0; i < Forms.length; i++) {
            if (campaign != null && 
                    (
                        (Forms[i].ID == <%= GVUZ.DAL.Dapper.ViewModel.Dictionary.EDFormsConst.O %>  && (campaign.EducationFormFlag & 1) == 1 ) ||
                        (Forms[i].ID == <%= GVUZ.DAL.Dapper.ViewModel.Dictionary.EDFormsConst.OZ %>  && (campaign.EducationFormFlag & 2) == 2 ) ||
                        (Forms[i].ID == <%= GVUZ.DAL.Dapper.ViewModel.Dictionary.EDFormsConst.Z %>  && (campaign.EducationFormFlag & 4) == 4 )
                    )
                )
            {
                sys = sys + '<option value="'+ Forms[i].ID +'">'+ Forms[i].Name +'</option>';
                edFormsDone.push(Forms[i].ID);
            }
        }
        sct = sct + sys +'</select>';
        $('#CGE_EducationForms').html(sct);

        this.educationFormID = <%= Html.Serialize(Model.CompetitiveGroupEdit.EducationFormID) %>;
        if (this.educationFormID != 0) {
            $("#Select_EducationForms").val(this.educationFormID);
        } else if (edFormsDone.length > 1) {
            $('#Select_EducationForms')[0].value = 0;
        }

        $("#Select_EducationForms").change(function() {
            this.educationFormID = $('#Select_EducationForms').val();
        });
    }

    function FillEducationLevel()
    {
        var campaignID = $('#Select_Campaigns').val();
        var sct= '<select id="Select_EducationLevels">';
        var sys = "";

        var edLevelsDone = [];
        for (var i = 0; i < Campaigns.length; i++) {
            if (Campaigns[i].CampaignID == campaignID && edLevelsDone.indexOf(Campaigns[i].EducationLevelID) < 0)
            {
                sys = sys + '<option value="'+ Campaigns[i].EducationLevelID +'">'+ Campaigns[i].EducationLevelName +'</option>';
                edLevelsDone.push(Campaigns[i].EducationLevelID);
            }
        }
        sct = sct + sys +'</select>';
        $('#CGE_EducationLevels').html(sct);

        this.educationLevelID = <%= Html.Serialize(Model.CompetitiveGroupEdit.EducationLevelID) %>;
        if (this.educationLevelID != 0) {
            $("#Select_EducationLevels").val(this.educationLevelID);
        } else if (edLevelsDone.length > 1) {
            $('#Select_EducationLevels')[0].value = 0;
        }

        $("#Select_EducationLevels").change(function() {
            FillDirections();
            CheckEducationLevelLicense();
        });
        FillDirections();
        CheckEducationLevelLicense();
    };

    function CheckEducationLevelLicense()
    {
        //  ОО не обладает лицензией на осуществление образовательной деятельности по направлениям подготовки данного уровня образования
    };
    

    function FillDirections()
    {
        this.educationLevelID = $('#Select_EducationLevels').val();
        var sct= '<select id="Select_Directions">';
        var sys = "";

        var directionsDone = [];

        if (!IsMultiProfile)
        {
            for (var i = 0; i < Directions.length; i++) {
                if (Directions[i].EducationLevelID == this.educationLevelID && directionsDone.indexOf(Directions[i].DirectionID) < 0)
                {
                    sys = sys + '<option value="'+ Directions[i].DirectionID +'">'+ Directions[i].Name +'</option>';
                    directionsDone.push(Directions[i].DirectionID);
                }
            }
        }
        else {
            for (var i = 0; i < Directions.length; i++) {
                if (Directions[i].EducationLevelID == this.educationLevelID && directionsDone.indexOf(Directions[i].ParentID) < 0)
                {
                    sys = sys + '<option value="'+ Directions[i].ParentID +'">'+ Directions[i].UGSCODE + ' ' + Directions[i].UGSNAME +'</option>';
                    directionsDone.push(Directions[i].ParentID);
                }
            }
        }
        

        sct = sct + sys +'</select>';
        $('#CGE_Directions').html(sct);
        
        if (DirectionID != 0) {
            $("#Select_Directions").val(DirectionID);
        } 
        // if (directionsDone.length > 1) {
        //    $('#Select_Directions')[0].value = 0;
        //} 
        if (ParentDirectionID != null) {
            $("#Select_Directions").val(ParentDirectionID);
        }

        $("#Select_Directions").change(function() {
            // TODO: Проверять, что вообще можно это менять!
            // CheckEducationLevelLicense();
            SetEntranceTestTablesVisible();
        });
        //CheckEducationLevelLicense();
        SetEntranceTestTablesVisible();
    };

    function FillLevelBudgets()
    {
        this.campaignID = $('#Select_Campaigns').val();
        this.campaign = null;
        for(var c = 0; c < Campaigns.length; c++)
        {
            if (Campaigns[c].CampaignID == campaignID)
                campaign = Campaigns[c];
        }

        var sct= '<select id="Select_LevelBudgets">';
        var sys = "";

        var cLevels = [];
        sys = sys + '<option value="">'+ 'Не выбрано' +'</option>';
        for (var i = 0; i < levelBudgets.length; i++) {
            if (campaign != null)
            {
                sys = sys + '<option value="'+ levelBudgets[i].IdLevelBudget +'">'+ levelBudgets[i].BudgetName +'</option>';
                cLevels.push(levelBudgets[i].IdLevelBudget);
            }
        }
        sct = sct + sys +'</select>';
        $('#CGE_LevelBudgets').html(sct);

        this.IdLevelBudget = <%= Html.Serialize(Model.CompetitiveGroupEdit.IdLevelBudget) %>;
        if (this.IdLevelBudget != 0) {
            $("#Select_LevelBudgets").val(this.IdLevelBudget);
        } 
        else if (cLevels.length > 1) {
            $('#Select_LevelBudgets')[0].value = 0;
        }

        $("#Select_LevelBudgets").change(function() {
            this.IdLevelBudget = $('#Select_LevelBudgets').val();
        });
    }

    function SetEntranceTestTablesVisible()
    {
        this.directionID = $('#Select_Directions').val();
        for (var i = 0; i < Directions.length; i++) {
            if (Directions[i].ID == this.directionID){
                //if (Directions[i].IsCreative)
                $('.divCreative').removeAttr('style').attr('style', 'display: ' + (Directions[i].IsCreative ? 'block' : 'none'));
                $('.divProfile').removeAttr('style').attr('style', 'display: ' + (Directions[i].IsProfile ? 'block' : 'none'));

                // $('#Select_Directions').after('<span class="field-validation-error"><br/>Необходимо указать направление подготовки</span>');
                
                $('#Select_Directions').removeClass('input-validation-warning');
                
                // Yellow Warning Message ON custom AllowedDirection
                <%--$('.field-validation-warning').html('');
                if (Directions[i].AllowedDirectionStatusID == <%= DirectionViewModel.DIRECTION_STATUS_USERADD %>){
                    $('#Select_Directions').addClass('input-validation-warning');
                    $('#Select_Directions').after('<span class="field-validation-warning"><br/>ОО не обладает лицензией на осуществление образовательной деятельности<br/> по данному направлению подготовки.<br/> Вы не сможете создать приказы по данному конкурсу, если не будет лицензии!</span>');
                } --%>
                
                //else{
                //    $('#Select_Directions').removeClass('input-validation-warning');
                //    $('.field-validation-warning').html('');
                //}
                break;
            }
        }

    }

    
    
    //function doClickOuterElement()
    //{
    //    if(!isSomethingChanged)
    //        return true;
    //    var $el = jQuery(this)
    //    jQuery('<div>Вы хотите сохранить данные перед уходом с данной страницы?</div>').dialog({
    //        width: '400px',
    //        modal: true,
    //        buttons: {
    //            "Сохранить": function() {
    //                doSubmit($el.attr('href'));
    //                closeDialog(jQuery(this))
    //            },
    //            "Не сохранять": function() {
    //                window.location = $el.attr('href');
    //                closeDialog(jQuery(this))
    //            },
    //            "Отмена": function() { closeDialog(jQuery(this)) }
    //        }
    //    })
    //    /*if(confirm('Вы хотите сохранить заявление перед уходом с данной страницы?'))
	//	{
	//		$outerClickedElement = jQuery(this)
	//		doSubmit('save')
	//		return false
	//	}
	//	return true*/
    //    return false
    //}

    // Скорее всего не используется больше!
   <%-- var isSomethingChanged = false
    jQuery(document).ready(function() {
        //jQuery('a.menuitemr,a.menuitemr1,a.menuiteml,a.menuiteml1').click(doClickOuterElement)
        jQuery('input').change(function () { isSomethingChanged = true;})
        jQuery('select').change(function () { isSomethingChanged = true;})
        jQuery('.gvuzDataGrid a:not(.btnDelete)').click(function () { isSomethingChanged = true;})
		
        <% if((!Model.CompetitiveGroupEdit.CanEdit) || GVUZ.Helper.UrlUtils.IsReadOnly(FBDUserSubroles.CampaignDirection)){ %>
            jQuery('.content input').addClass('view').attr('readonly', 'readonly');
        <% } %>
    })--%>

    menuItems[4].selected = true;

    function GetOlympicYearStart()//#FIS-1723
    {
        var selectYearStart = $('#CompetitiveGroupEdit_CampaignYearStart').val();
        var olympicYearStart = selectYearStart - <%=Model.CompetitiveGroupEdit.OlympicValidityYears%>; 
        return olympicYearStart;
    }
    
</script>
</asp:Content>
