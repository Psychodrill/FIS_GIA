<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.DAL.Dapper.ViewModel.Campaign.CampaignViewModel>" %>

<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Model.Entrants" %>
<%@ Import Namespace="GVUZ.Web.ContextExtensions" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Register TagPrefix="gv" TagName="AdminMenuControl" Src="~/Views/Shared/Controls/AdminMenuControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= Model.CampaignEdit.CampaignID > 0 ? "Редактирование" : "Добавление"%> приемной кампании
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
                <div id="errorMessage"></div>
                <table class="data">
                    <tbody>
                        <tr>
                            <td class="caption"><%= Html.TableLabelFor(x => x.CampaignEdit.CampaignName) %></td>
                            <td><%= Html.TextBoxExFor(m => m.CampaignEdit.CampaignName, new { id="CampaignName" }) %></td>

                            <td class="caption">Сроки проведения ПК</td>
                            <td>с <%= Html.DropDownListExFor(x => x.CampaignEdit.YearStart, Model.CampaignEdit.YearRange, new {@class="ss", id="YearStart" }) %>&nbsp;&nbsp; по <%= Html.DropDownListExFor(x => x.CampaignEdit.YearEnd, Model.CampaignEdit.YearRange, new {@class="ss", id="YearEnd"}) %></td>
                        </tr>
                        <tr>
                            <td class="caption"><%= Html.TableLabelFor(x => x.CampaignEdit.UID) %></td>
                            <td><%= Html.TextBoxExFor(m => m.CampaignEdit.UID, new { id="UID" }) %></td>

                            <td class="caption"><%= Html.TableLabelFor(x => x.CampaignEdit.CampaignTypeName, new { id="CampaignTypeName" }) %></td>
                            <td><div id="CE_CampaignType">

                                </div>
                                </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="Label_LevelsEducation">
                                </div>
                            </td>
                            <td>
                                <div id="Model_LevelsEducation">
                                </div>
                            </td>

                        </tr>
                        <tr>
                            <td class="caption"><%= Html.TableLabelFor(x => x.CampaignEdit.EducationFormFlag) %></td>
                            <td colspan="5">
                                <div id="EducationFormFlag">
                                    <input type="checkbox" id="EducationForm_O" <%= (Model.CampaignEdit.EducationFormFlag & 1) > 0 ? "checked=\"checked\"" : "" %> <%= (Model.CampaignEdit.UsedEducationFormFlags & 1) > 0 ? "disabled=\"disabled\"" : "" %> /><label for="EducationForm_O">Очная форма</label><br />
                                    <input type="checkbox" id="EducationForm_OZ" <%= (Model.CampaignEdit.EducationFormFlag & 2) > 0 ? "checked=\"checked\"" : "" %> <%= (Model.CampaignEdit.UsedEducationFormFlags & 2) > 0 ? "disabled=\"disabled\"" : "" %> /><label for="EducationForm_OZ">Очно-заочная форма</label><br />
                                    <input type="checkbox" id="EducationForm_Z" <%= (Model.CampaignEdit.EducationFormFlag & 4) > 0 ? "checked=\"checked\"" : "" %> <%= (Model.CampaignEdit.UsedEducationFormFlags & 4) > 0 ? "disabled=\"disabled\"" : "" %> /><label for="EducationForm_Z">Заочная форма</label><br />
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <div>&nbsp;</div>
                <div>
                    <input type="button" value="Сохранить" id="btnSave" class="button3" />
                    <input type="button" value="Отмена" id="btnCancel" class="button3" />
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function CampaignEdit()
        {
            var cam = this;
            cam.isSomethingChanged = false
            cam.LevelsEducation = <%= Html.Serialize(Model.CampaignEdit.LevelsEducationNames)%>;
            cam.CampaignEducationLevel = <%= Html.Serialize(Model.CampaignEdit.CampaignEducationLevel)%>;
            cam.CampaignTypes = <%= Html.Serialize(Model.CampaignTypes)%>;
            cam.CampaignTypeID = <%=Model.CampaignEdit.CampaignTypeID%>;
            //cam.
            cam.saveData = function() {
                var model = {
                    CampaignID: <%= Model.CampaignEdit.CampaignID %>,
                    CampaignName: $('#CampaignName').val(),
                    YearStart: $('#YearStart').val(),
                    YearEnd: $('#YearEnd').val(),
                    UID: $('#UID').val(),
                    EducationFormFlag: 0,
                    LevelsEducationNames: [],
                    CampaignTypeID: $("#Select_CampaignType").val(),
                };
                $('#Model_LevelsEducation input[type="checkbox"]:checked').each(function(i, e) {
                    model.LevelsEducationNames.push({ ItemTypeID: e.id });
                });
                if ($("#EducationForm_O").is(':checked')) model.EducationFormFlag |= 1;
                if ($("#EducationForm_OZ").is(':checked')) model.EducationFormFlag |= 2;
                if ($("#EducationForm_Z").is(':checked')) model.EducationFormFlag |= 4;

                if (cam.validate(model)) {
                    cam.updateData(model);
                }              
            }
            cam.validate = function(model)
            {
                var isError = true;
                if ($('#CampaignName').val() == "") {
                    $('#CampaignName').addClass('input-validation-error');
                    isError = false;
                } else {
                    $('#CampaignName').removeClass('input-validation-error');
                }
                if (model.EducationFormFlag == 0) {
                    $('#EducationFormFlag').addClass('input-validation-error');
                    isError = false;
                }else {
                    $('#EducationFormFlag').removeClass('input-validation-error');
                }
                if (model.LevelsEducationNames.length == 0) {
                    $('#Model_LevelsEducation').addClass('input-validation-error');
                    isError = false;
                }else {
                    $('#Model_LevelsEducation').removeClass('input-validation-error');
                }

                if (model.YearStart > model.YearEnd){
                    $('#YearStart').addClass('input-validation-error');
                    isError = false;
                }else{
                    $('#YearStart').removeClass('input-validation-error');
                }

                return isError;
            }
            cam.cancelData = function () {
                window.location = '<%= Url.Generate<CampaignController>(x => x.CampaignList()) %>'
            }
            cam.init = function()
            {
                $('#btnSave').click(cam.saveData);
                $('#btnCancel').click(cam.cancelData);              
                $('input').change(function () { cam.isSomethingChanged = true;});
                $('#YearStart').change(function() {
                    $('#CE_CampaignType').html('');
                    cam.CampaignType();
                }); 
                cam.CampaignType();        
                cam.levelsEducation();           
            }
            cam.CreateCampaignType = function(lData)
            {
                cam.ListEditCampaignTypes = lData;
                var selectYearStart = $('#YearStart').val();
                var sct= '<select id="Select_CampaignType">';
                var sys = "";
                for (var i = 0; i < cam.ListEditCampaignTypes.length; i++) {
                    sys = sys + '<option value="'+ cam.ListEditCampaignTypes[i].ID +'">'+ cam.ListEditCampaignTypes[i].Name +'</option>';
                }
                sct = sct + sys +'</select>';
                $('#CE_CampaignType').html(sct);
                //
                <% if (!Model.CampaignEdit.CanChangeType) { %>
                    $('#Select_CampaignType').attr('disabled', 'disabled');
                <% } %>

                $("#Select_CampaignType").change(function() {
                    $('#Model_LevelsEducation').html('');
                    cam.levelsEducation();
                });
                if (cam.CampaignTypeID != 0) {
                    $("#Select_CampaignType").val(cam.CampaignTypeID)
                } else {
                    $('#Select_CampaignType').val();
                }
                cam.levelsEducation();
            }
            cam.CampaignType = function()
            {         
                var YearStart = { yearStart : $('#YearStart').val() }
                doPostAjax('<%= Url.Generate<CampaignController>(x => x.GetEditCampaignTypes(0)) %>', JSON.stringify(YearStart), function(data) {             
                    $('#CE_CampaignType').html('');
                    $('#btnSave').removeAttr("disabled");
                    
                    if (data.Data.length > 0) {
                        var listData = data.Data;
                        if (cam.CampaignTypeID != 0) {
                            for (var i = 0; i < cam.CampaignTypes.length; i++) {
                                if (cam.CampaignTypes[i].CampaignTypeID == cam.CampaignTypeID) {
                                    listData.push(cam.CampaignTypes[i]);
                                    break;
                                }
                            }
                        }
                        
                        cam.CreateCampaignType(data.Data);
                    } else {
                        if (cam.CampaignTypeID != 0) {
                            var listData =[];
                            for (var i = 0; i < cam.CampaignTypes.length; i++) {
                                if (cam.CampaignTypes[i].CampaignTypeID == cam.CampaignTypeID) {
                                    listData.push(cam.CampaignTypes[i]);
                                    break;
                                }
                            }
                            cam.CreateCampaignType(listData);
                        }else {
                            $('#CE_CampaignType').html('<p style="color:red;">В рамках одного года можно создать пять приемных кампаний!</p>');
                            $('#btnSave').attr("disabled", "disabled");
                        }
                    }
                });
            }
            cam.levelsEducation = function()
            {
                var SelectCampaignTypeID = $("#Select_CampaignType").val();
                var le = "";
                for (var i = 0; i < cam.LevelsEducation.length; i++) {
                    if(cam.LevelsEducation[i].CampaignTypeID==SelectCampaignTypeID)
                    {   
                        <%if (Model.CampaignEdit.CampaignEducationLevel != null)
        {%>
                        var checked= "";
                        if ((cam.LevelsEducation[i].CampaignTypeID == <%=GVUZ.DAL.Dapper.ViewModel.Dictionary.CampaignTypesView.Magistracy %>) || (cam.LevelsEducation[i].CampaignTypeID == <%=GVUZ.DAL.Dapper.ViewModel.Dictionary.CampaignTypesView.SPO %>) || (cam.LevelsEducation[i].CampaignTypeID == <%=GVUZ.DAL.Dapper.ViewModel.Dictionary.CampaignTypesView.HighQualification %>)) {
                            checked='checked="checked" disabled="disabled"';
                        }
                        else {
                            for (var j = 0; j < cam.CampaignEducationLevel.length; j++) {
                                if (cam.CampaignEducationLevel[j].EducationLevelID == cam.LevelsEducation[i].ItemTypeID) {
                                    checked='checked="checked"';
                                    if (!cam.CampaignEducationLevel[j].CanRemove)
                                        checked += ' disabled="disabled"';
                                }
                            }
                        }      
                        le = le + '<input id="'+cam.LevelsEducation[i].ItemTypeID +'" type="checkbox" '+checked+'/>'+ cam.LevelsEducation[i].Name + '</br>';
                        <%}
                        else
                        {%>
                        var checked= "";
                        if ((cam.LevelsEducation[i].CampaignTypeID == <%=GVUZ.DAL.Dapper.ViewModel.Dictionary.CampaignTypesView.Magistracy %>) || (cam.LevelsEducation[i].CampaignTypeID == <%=GVUZ.DAL.Dapper.ViewModel.Dictionary.CampaignTypesView.SPO %>) || (cam.LevelsEducation[i].CampaignTypeID == <%=GVUZ.DAL.Dapper.ViewModel.Dictionary.CampaignTypesView.HighQualification %>)) {
                            checked='checked="checked" disabled="disabled"';
                        }
                        le = le + '<input id="'+cam.LevelsEducation[i].ItemTypeID +'"Label_LevelsEducation type="checkbox" '+checked+'/>'+ cam.LevelsEducation[i].Name + '</br>';
                        <% }%>     
                    }
                }
                $('#Label_LevelsEducation').html('<%=Html.TableLabelFor(x => x.CampaignEdit.LevelsEducation) %>');          
                $('#Model_LevelsEducation').html(le);
            }
            cam.updateData = function(model){
                $('.content').find('.input-validation-error').addClass('input-validation-error-fixed').removeClass('input-validation-error');
                $('.content').find('.field-validation-error').remove().detach();
                doPostAjax('<%= Url.Generate<CampaignController>(x => x.CampaignUpdate(null)) %>', JSON.stringify(model), function(data) {
                    if (data.IsError) { 
                        for (var i = 0; i < data.Data.length.length; i++) {
                            $('#errorspan').remove();  
                        }                        
                        for (var i = 0; i < data.Data.length; i++) { 
                            $('#'+data.Data[i].ControlID).addClass('input-validation-error'); //.removeClass('input-validation-error-fixed')
                            $('#'+data.Data[i].ControlID).after( '<div id="errorspan"><span id="" class="field-validation-error">'+data.Data[i].ErrorMessage+'</span></div>' );
                        }
                    } else {
                        window.location = '<%= Url.Generate<CampaignController>(x => x.CampaignList()) %>?campaignID=' + data.Data;
                    }
                    //if (!addValidationErrorsFromServerResponse(data, true)) {
                        
                    //}
                });
            }
        }
        $(document).ready(function() {
            var model = new CampaignEdit();
            model.init();
            <% if (!Model.CampaignEdit.CanEdit) //|| GVUZ.Helper.UrlUtils.IsReadOnly(FBDUserSubroles.CampaignDirection))
                { %>
            $('input[type="text"],select').attr('readonly', 'readonly').addClass('view');
            $('input[type="checkbox"],select').attr('disabled', 'disabled');
            $('#btnSave').hide();
            $('#btnCancel').val('Вернуться');
            <% } %>
            
            <% if (!Model.CampaignEdit.CanChangeType) { %>
            $('#YearStart').attr('disabled', 'disabled');
            $('#YearEnd').attr('disabled', 'disabled');
            $('#Select_CampaignType').attr('disabled', 'disabled');
            <% } %>
        });
    </script>
</asp:Content>
