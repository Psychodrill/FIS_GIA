<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.InstitutionInfo.InstitutionInfoViewModel>"
	MasterPageFile="~/Views/Shared/Site.Master" %>

<asp:Content ID="registerTitle" ContentPlaceHolderID="TitleContent" runat="server">
	Редактирование общих сведений
</asp:Content>
<asp:Content ID="header" ContentPlaceHolderID="PageTitle" runat="server">Сведения об образовательном учреждении</asp:Content>
<%@ Register TagPrefix="gv" TagName="TabControl" Src="~/Views/Shared/Common/InstitutionsTabControl.ascx" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">	
	<div class="divstatement">
		<gv:TabControl runat="server" ID="tabControl" />
        <% using (Html.BeginForm("EditNew", "Institution", FormMethod.Post, new { enctype = "multipart/form-data"})) { %>
		<table class="tableAdmin" style="table-layout: fixed">
			<col width="270px" />
			<col />
			<col width="150px" >
			<col />
			<tbody>
				<tr>
					<td class="caption big">
						<%=Html.TableLabelFor(m => m.FullName)%>
					</td>
					<td colspan="3">
						<%= Html.CommonInputReadOnly(Model.FullName)%>
                        <%= Html.HiddenFor(m => m.FullName) %>
					</td>
				</tr>
				<tr>
					<td class="caption big">
						<%: Html.TableLabelFor(m => m.BriefName)%>
					</td>
					<td colspan="3">
						<%= Html.CommonInputReadOnly(Model.BriefName)%>
                        <%= Html.HiddenFor(m => m.BriefName) %>
					</td>
				</tr>
				<tr class="separat">
					<td class="caption big" style="padding-bottom: 8px; vertical-align: top; padding-top: 10px">
						<%:Html.TableLabelFor(m => m.FormOfLawId)%>
					</td>
					<td colspan="3">
						<%= Html.CommonInputReadOnly(Model.FormOfLawName)%>
                        <%= Html.HiddenFor(m => m.FormOfLawName) %>
					</td>
				</tr>
				<tr class="separat">
					<td class="caption">
						<%:Html.TableLabelFor(m => m.Site)%>
					</td>
					<td colspan="3">
						<%=Html.TextBoxExFor(m => m.Site, new { @class = "inputboxAd" })%>
					</td>
				</tr>
				<tr>
					<td class="caption">
						<%:Html.TableLabelFor(m => m.RegionId)%>
					</td>
					<td>
						<%= Html.CommonInputReadOnly(Model.RegionName)%>
                        <%= Html.HiddenFor(m => m.RegionName) %>
					</td>
					<td class="caption w150px">
						<%:Html.TableLabelFor(m => m.City)%>
					</td>
					<td>
						<%=Html.CommonInputReadOnly(Model.City)%>
                        <%= Html.HiddenFor(m => m.City) %>
					</td>
				</tr>
				<tr>
                    <td class="caption">
						<%:Html.TableLabelFor(m => m.Address)%>
					</td>
					<td>
						<%=Html.CommonInputReadOnly(Model.Address)%>
                        <%= Html.HiddenFor(m => m.Address) %>
					</td>
					<td class="caption w150px">
						<%:Html.TableLabelFor(m => m.Phone)%>
					</td>
					<td>
                        <table style="width: 100%">
                            <tr>
                                <td><%=Html.TextBoxExFor(m => m.Phone, new { @class = "inputboxAd" })%></td>
                                <td align="right" class="caption"><%:Html.TableLabelFor(m => m.Fax)%></td>
                                <td><%=Html.TextBoxExFor(m => m.Fax, new { @class = "inputboxAd" })%></td>
                            </tr>
                        </table>
						
					</td>				
					
				</tr>
				<tr>
					<td class="caption" style="vertical-align:top; padding-top:15px;">
						<%:Html.TableLabelFor(m => m.LicenseNumber)%>
					</td>
					<td >
						<%= Html.CommonInputReadOnly(Model.LicenseNumber, new { @class = "inputboxAd", style = "min-width: 200px; width: 200px !important" })%>
						<%= Html.HiddenFor(m => m.LicenseNumber) %>
                        <span class="licenseDateCaption">от: <%= Html.CommonDateReadOnly(Model.LicenseDate) %></span>
                        <%= Html.HiddenFor(m => m.LicenseDate) %>
					</td>
					<td class="caption w150px">
						<span>
						<%:Html.TableLabelFor(m => m.AccreditationNumber)%></span>
					</td>
					<td>
						<%=Html.TextBoxExFor(m => m.AccreditationNumber, new { @class = "inputboxAd" })%>
					</td>
				</tr>
				<tr class="separat">
					<td class="caption">&nbsp;</td>
					<td>
						<div style="margin-bottom: 10px;">
                            <% if (Model.LicenseDocument != null) { %>
							<%= Html.GenerateFileLink(Url.Generate<InstitutionController>(c => c.GetFile1(Model.LicenseDocument.FileId)), Model.LicenseDocument.DisplayName, "licFileDelete")%>
							<% } %>
                            <%= Html.HiddenFor(m => m.IsLicenseFileDeleted) %>
                            <input type="file" name="UploadedLicenseFile" id="UploadedLicenseFile" />
						</div>
					</td>
					<td class="caption w150px">&nbsp;</td>
					<td>
						<div style="margin-bottom: 10px;">
                            <% if (Model.AccreditationDocument != null) { %>
							<%= Html.GenerateFileLink(Url.Generate<InstitutionController>(c => c.GetFile1(Model.AccreditationDocument.FileId)), Model.AccreditationDocument.DisplayName, "accFileDelete")%>
							<%  } %>
                            <%= Html.HiddenFor(m => m.IsAccreditationFileDeleted) %>
                            <input type="file" name="UploadedAccreditationFile" id="UploadedAccreditationFile" />
						</div>
					</td>
				</tr>
                <tr class="separat">
		            <td class="caption">
                    <%= Html.DisplayNameFor(m => m.Documents) %>:
		            </td>
                    <td colspan="3">
                        <table style="width: 100%;">
                            <colgroup>
                                <col style="width: 50%" />
                                <col style="width: 50%" />
                            </colgroup>
                            <tr>
                                <td id="documentsList">
                                    <% Html.RenderPartial("EditInstitutionDocumentsList", Model.Documents); %>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <a class="button3" style="display: inline-block; width: auto;" href="javascript:void(0)" onclick="return showSubmitDocumentDialog()">Добавить документ</a>
                                </td>
                            </tr>
                        </table>
                        
                    </td>
                </tr>
               	<tr class="separat">
					<td class="caption">
						<%:Html.TableLabelFor(m => m.HasMilitaryDepartment)%>
					</td>
					<td colspan="3">
						<%=Html.CheckBoxFor(m => m.HasMilitaryDepartment)%>
					</td>
                </tr>
				<tr>
					<td class="caption">
						<%:Html.TableLabelFor(m => m.HasHostel)%>
					</td>
					<td>
						<%=Html.CheckBoxFor(m => m.HasHostel)%>
					</td>
					<td class="caption w150px" id="spHostelData">
						<%:Html.TableLabelFor(m => m.HostelCapacity)%>
					</td>
					<td id="spHostelData2">
						<%=Html.TextBoxExFor(m => m.HostelCapacity, new { @class = "inputboxAd" })%>						
					</td>					
				</tr>
				<tr>
					<td class="caption">
						<%:Html.TableLabelFor(m => m.HasHostelForEntrants) %>
					</td>
					<td colspan="3">
						<%=Html.CheckBoxFor(m => m.HasHostelForEntrants)%>
					</td>
				</tr>
				<tr>
					<td class="caption" style="vertical-align: top; padding-top: 10px;">
						Условие предоставления:
					</td>
					<td colspan="3">
						<% if (Model.HostelDocument != null) { %>
							<%= Html.GenerateFileLink(Url.Generate<InstitutionController>(c => c.GetFile1(Model.HostelDocument.FileId)), Model.HostelDocument.DisplayName, "hostelFileDelete")%>
						<% } %>
						<%= Html.HiddenFor(m => m.IsHostelFileDeleted) %>
                        <input type="file" name="UploadedHostelFile" id="UploadedHostelFile" />
					</td>
				</tr>	
                <tr class="separat">
					<td class="caption">
						Создание условий проведения ВИ<br />для лиц с ОВЗ:
					</td>
					<td colspan="3">
						<%=Html.CheckBoxFor(m => m.HasDisabilityEntrance)%>
					</td>
                </tr>			
			</tbody>
		</table>
		<div style="margin-top: 15px">
			<input id="btnSubmit" class="button3" type="submit" value="Сохранить" />
			<%= Html.ActionLink("Отмена", "View", "Institution", null, new { @class = "button3" }) %>
		</div>
        <% } %>
	</div>
   
    <div id="dialog">
        <% using (Html.BeginForm("AddInstitutionDocument", "Institution", FormMethod.Post, new { id="uploadForm", enctype = "multipart/form-data" })) { %>
            <fieldset>
                <table style="width: 550px">
                    <colgroup>
                        <col style="width: 150px" />
                        <col style="width: 400px" />
                    </colgroup>
                    <tr>
                        <td class="caption"><label for="Year">Год:</label></td>
                        <td><%= Html.DropDownList("Year", Model.DocumentYearsList) %></td>
                    </tr>
                    <tr>
                        <td class="caption"><label for="Name">Наименование:</label></td>
                        <td><%= Html.TextBox("Name", string.Empty, new { maxlength = "255" }) %></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <input type="file" name="UploadedFile" id="UploadedFile" autocomplete="off" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        <% } %>
    </div>

	<script type="text/javascript">

		menuItems[0].selected = true;

		function submitDocumentFile() {
		    $('#uploadForm').ajaxSubmit({
		        method: 'POST',
		        dataType: 'json',
		        url: $('#uploadForm').attr('action'),
		        error: showAsyncFilePostError,
		        success: function (res) {
		            if (!res.IsError) {
		                window.setTimeout(refreshDocumentsList, 500);
		                $('#dialog').dialog('close');
		            }
		            else //if (res.Message) 
		            {
		                clearValidationErrors(jQuery('#uploadForm'));
		                //uploadForm
		                for (var i = 0; i < $('#errorspan').length; i++) {
		                    $('#errorspan').remove();
		                }
		                for (var i = 0; i < res.Data.length; i++) {
		                    var control = $('#' + res.Data[i].ControlID)[0];
		                    if (control) {
		                        $('#' + res.Data[i].ControlID).removeClass('input-validation-error-fixed');
		                        $('#' + res.Data[i].ControlID).addClass('input-validation-error');
		                        $('#' + res.Data[i].ControlID).after('<div id="errorspan"><span id="" class="field-validation-error">' + res.Data[i].ErrorMessage + '</span></div>');
		                    }
		                    else {
		                        $('#Name').addClass('input-validation-error').removeClass('input-validation-error-fixed');
		                        $('#Name').after('<div id="errorspan"><span id="" class="field-validation-error">' + res.Data[i].ErrorMessage + '</span></div>');
		                    }



		                }
		            }
		        }

		    });
		}

        $("#dialog").dialog({
            autoOpen: false,
            width : 600,
            modal: true,
            title: 'Загрузка документа',
            buttons: {
                "Добавить ": submitDocumentFile
            } 
        });

        function showSubmitDocumentDialog() {
            $('input#UploadedFile').val('');
            $('select#Year').val($('option:first', $('select#Year')).val());
            $('input#Name').val('');
            $('#dialog').dialog('open');
            return false;
        }

        $('#HasHostel').bind('change', function (e) {
            if (e.target.checked) {
                $("#HostelCapacity").removeAttr("disabled").val("");
            }
            else {
                $("#HostelCapacity").attr({
                    "disabled": true
                }).val("-");
            }
        });

        $('#licFileDelete').bind('click', function (e) {
            e.preventDefault();
            e.stopPropagation();
            $('input#IsLicenseFileDeleted').val('true');
            $(e.target).parent('div').remove();
            return false;
        });

        $('#accFileDelete').bind('click', function (e) {
            e.preventDefault();
            e.stopPropagation();
            $('input#IsAccreditationFileDeleted').val('true');
            $(e.target).parent('div').remove();
            return false;
        });

        $('#hostelFileDelete').bind('click', function (e) {
            e.preventDefault();
            e.stopPropagation();
            $('input#IsHostelFileDeleted').val('true');
            $(e.target).parent('div').remove();
            return false;
        });

        $(document).bind("keypress", ":input:not(textarea)", function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                e.stopPropagation();
                return false;
            }
            return true;
        });

        $('td#documentsList').find('button.fileDelete[data-attachment-id]').bind('click', function (e) {
            e.preventDefault();
            e.stopPropagation();
            if (!confirm('Удалить документ?')) return false;
            var url = '<%= Url.Action("DeleteInstitutionDocument", "Institution", new { attachmentId = "__ATTACHMENTID__"}) %>';
            var attachmentId = $(e.target).attr('data-attachment-id').toString();
            $.post(url.replace('__ATTACHMENTID__', attachmentId), function (html) { $('td#documentsList').html(html);});
            return false;
        });

	    function refreshDocumentsList(){
	        $.post('<%= Url.Action("DocumentsList", "Institution") %>', function (html) { $('td#documentsList').html(html);});
	    }
	</script>
</asp:Content>
