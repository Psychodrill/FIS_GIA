<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.InstituteCommonInfoViewModel>"
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
		<table class="tableAdmin" style="table-layout: fixed">
			<col width="270px" />
			<col />
			<col width="150px" />
			<col />
			<tbody>
				<tr>
					<td class="caption big">
						<%=Html.TableLabelFor(m => m.FullName, required: true)%>
					</td>
					<td colspan="3">
						<%= Html.CommonInputReadOnly(Model.FullName)%>
					</td>
				</tr>
				<tr>
					<td class="caption big">
						<%: Html.TableLabelFor(m => m.BriefName, required: true)%>
					</td>
					<td colspan="3">
						<%= Html.CommonInputReadOnly(Model.BriefName)%>
					</td>
				</tr>
				<tr class="separat">
					<td class="caption big" style="padding-bottom: 8px; vertical-align: top; padding-top: 10px">
						<%:Html.TableLabelFor(m => m.FormOfLawID, required: true)%>
					</td>
					<td colspan="3">
						<%= Html.CommonInputReadOnly(Model.FormOfLawText)%>
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
						<%:Html.TableLabelFor(m => m.RegionID, required: true)%>
					</td>
					<td>
						<%= Html.CommonInputReadOnly(Model.RegionText)%>
					</td>
					<td class="caption w150px">
						<%:Html.TableLabelFor(m => m.Address)%>
					</td>
					<td>
						<%=Html.TextBoxExFor(m => m.Address, new { @class = "inputboxAd" })%>
					</td>
				</tr>
				<tr>
					<td class="caption">
						<%:Html.TableLabelFor(m => m.Phone)%>
					</td>
					<td>
						<%=Html.TextBoxExFor(m => m.Phone, new { @class = "inputboxAd" })%>
					</td>				
					<td class="caption w150px">
						<%:Html.TableLabelFor(m => m.Fax)%>
					</td>
					<td>
						<%=Html.TextBoxExFor(m => m.Fax, new { @class = "inputboxAd" })%>
					</td>
				</tr>
				<tr>
					<td class="caption" style="vertical-align:top; padding-top:15px;">
						<%:Html.TableLabelFor(m => m.LicenseNumber, required: true)%>
					</td>
					<td >
						<%= Html.TextBoxExFor(m => m.LicenseNumber, new { @class = "inputboxAd", style="min-width: 200px; width: 200px !important" })%>
						<span class="licenseDateCaption">от: <%= Html.DatePickerFor(m => m.LicenseDate)%></span>
					</td>
					<td class="caption w150px">
						<span>
						<%:Html.TableLabelFor(m => m.Accreditation, required: true)%></span>
					</td>
					<td>
						<%=Html.TextBoxExFor(m => m.Accreditation, new { @class = "inputboxAd" })%>
					</td>
				</tr>
				<tr class="separat">
					<td class="caption">&nbsp;</td>
					<td>
						<div style="margin-bottom: 10px;">
							<%= Html.GenerateFileLink(Url.Generate<InstitutionController>(c => c.GetFile1(Model.LicenseDocumentID)), Model.LicenseDocumentName, "licFileDelete")%>
							<%= Html.FileForm("License") %>
						</div>
					</td>
					<td class="caption w150px">&nbsp;</td>
					<td>
						<div style="margin-bottom: 10px;">
							<%= Html.GenerateFileLink(Url.Generate<InstitutionController>(c => c.GetFile1(Model.AccreditationDocumentID)), Model.AccreditationDocumentName, "accFileDelete")%>
							<%= Html.FileForm("Accreditation") %>
						</div>
					</td>
				</tr>
                <tr class="separat">
		            <td class="caption">
                    Правила приёма:
		            </td>
                    <td id="parent">
                    <%for (int i = 2012; i <= DateTime.Now.AddYears(1).Year; i++)
                      {%>
                      <% if (BaseController.CheckFile(i,Model.InstitutionID)) %>
                    <%= Html.GenerateFileLink(Url.Generate<InstitutionController>(c => c.GetFile2(i)), "Правила приёма "+i/*, "rulesFileDelete"+i*/)%>
                    <%} %>
                    <b id="addButt"></b>
                        </td>
		                <td colspan="3">
                            <input style="width: 200px; height: 30px" type="button" onclick='simdil()' value='Добавить правила приёма'/>
                            <div id="dialog" >
                                 <p class="validateTips">Загрузка правил приёма</p>
                                <form id="default" action="">
                                    <fieldset>
                                    <select id="bass_form"></select>
                                    <br/>
                                    </fieldset>
                                </form>
                                <%= Html.FileForm("Rules") %>
                            </div>
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
						<% if (Model.HostelFecDocumentID != null) { %>
						<span>
							<%= Html.GenerateFileLink(Url.Generate<InstitutionController>(c => c.GetFile1(Model.HostelFecDocumentID)),
										Model.HostelFecDocumentName, "hostelFileDelete")%>
						</span>
						<% } %>
						<%= Html.FileForm("Hostel") %>
					</td>
				</tr>				
			</tbody>
		</table>
		<div style="margin-top: 15px">
			<input id="btnSubmit" class="button3" type="button" value="Сохранить" />
			<input id="btnCancel" class="button3" type="button" value="Отмена" />
		</div>
	</div>
   
	<script type="text/javascript">

		menuItems[0].selected = true;

		var licFileID = null;
		var accFileID = null;
        var rulFileID = null;
		var hosFileID = null;
		var hosFileDeleted = false;
		var licFileDeleted = false;
		var accFileDeleted = false;
        var isRulesAtt = new Array();
        <% for (int i=2012; i <= DateTime.Now.AddYears(1).Year; i++)
        {%>
            isRulesAtt[<%= i %>-2012] = <%= BaseController.CheckFile(i,Model.InstitutionID) ? 1 : 0 %>;
        <% }%>

		function addFileValidationError($control)
		{
			addValidationError($control, 'Размер файла превышает максимальный разрешенный размер в <%= BaseController.GetMaxAllowedFileLength() %>Кб', false)
		}

		function isFilePostError($control, data)
		{
			if(data.BigSize)
			{
				unblockUI()
				addFileValidationError($control)
				return true
			}
			return false
		}

		function checkFileSelectError($control)
		{
			if(!isFileLengthCorrect($control[0], <%= BaseController.GetMaxAllowedFileLength() %> * 1024))
			{
				addFileValidationError($control)
				return true
			}
			return false
		}

		function submitLicFile()
		{
			blockUI()
			if(licFileID != null) {submitAccFile();return;}
			jQuery('#fileLicenseForm').ajaxSubmit({
			method: 'POST',
			dataType: 'json',
			url: '<%= Url.Generate<InstitutionController>(x => x.ReceiveFile1()) %>',
			error: showAsyncFilePostError,
			success: function showResponse(data)  { 
					if(isFilePostError(jQuery('#postLicenseFile'), data)) return
					licFileID = data.FileID;
					submitAccFile();
				}})
		}
		function submitAccFile()
		{
			if(accFileID != null) {submitHosFile();return;}
			jQuery('#fileAccreditationForm').ajaxSubmit({
			method: 'POST',
			dataType: 'json',
			url: '<%= Url.Generate<InstitutionController>(x => x.ReceiveFile1()) %>',
			error: showAsyncFilePostError,
			success: function showResponse(data)  { 
					if(isFilePostError(jQuery('#postAccreditationFile'), data)) return
					accFileID = data.FileID;
					submitHosFile();
				}})
		}
        function submitRulesFile()
		{
            var htmlSelect = document.getElementById("bass_form");
            var YeaRi = parseInt(htmlSelect.options[htmlSelect.selectedIndex].value);
            var Insti = parseInt(<%= Model.InstitutionID %>);
            var ur = '/Institution/ReceiveFile2?year=';
            ur += YeaRi;
            ur += '&instid=';
            ur += Insti;
            
			if(rulFileID != null) {return;}
			jQuery('#fileRulesForm').ajaxSubmit({
			method: 'POST',
			dataType: 'json',
			url: ur,
			error: showAsyncFilePostError,
			success: function showResponse(data)  { 
					if(isFilePostError(jQuery('#postRulesFile'), data)) return
					rulFile = data.FileID;
                     
                    ur = '<div class="doc" id="';
                    ur += YeaRi;
                    ur += '"><a href="/Institution/GetFile2?year=';
                    ur += YeaRi;
                    ur += '">Правила приёма ';
                    ur += YeaRi;
                    ur += '</a>';
                             
                    if (isRulesAtt[YeaRi-2012]==0) {           
                    $(ur).insertBefore($('#addButt'));
                    isRulesAtt[YeaRi-2012]=1; }
                    $("#dialog").dialog("close"); 
				}})

            $('fileRulesForm').value = '';
		}
		function submitHosFile()
		{
			if(hosFileID != null) {doSubmitData();return;}
			jQuery('#fileHostelForm').ajaxSubmit({
			method: 'POST',
			dataType: 'json',
			url: '<%= Url.Generate<InstitutionController>(x => x.ReceiveFile1()) %>',
			error: showAsyncFilePostError,
			success: function showResponse(data)  { 
					if(isFilePostError(jQuery('#postHostelFile'), data)) return
					hosFileID = data.FileID;
					doSubmitData();
				}})
		}

		function doSubmitData()
		{
			clearValidationErrors(jQuery('.tableAdmin'));
			var model = 
			{
<%--
				FullName: jQuery('#FullName').val(),
				BriefName: jQuery('#BriefName').val(),
				FormOfLawID: jQuery('#FormOfLawID').val(),
				RegionID: jQuery('#RegionID').val(),
--%>

				Site: jQuery('#Site').val(),
				Address: jQuery('#Address').val(),
				Phone: jQuery('#Phone').val(),
				Fax: jQuery('#Fax').val(),
				LicenseNumber: jQuery('#LicenseNumber').val(),
				LicenseDate: jQuery('#LicenseDate').val(),
				Accreditation: jQuery('#Accreditation').val(),
				HasMilitaryDepartment: jQuery('#HasMilitaryDepartment').attr('checked'),
				HasHostel: jQuery('#HasHostel').attr('checked'),
				HostelCapacity: jQuery('#HostelCapacity').val(),
				HasHostelForEntrants: jQuery('#HasHostelForEntrants').attr('checked'),
				InstitutionID: <%= Model.InstitutionID %>,
				LicenseDocumentID: licFileID,
				AccreditationDocumentID: accFileID,
                RulesID: rulFileID,
				HostelFecDocumentID: hosFileID,
				HostelFecDocumentDeleted: hosFileDeleted,
				LicenseDocumentDeleted: licFileDeleted,
				AccreditationDocumentDeleted: accFileDeleted
			}
			doPostAjax('<%= Url.Generate<InstitutionController>(c => c.Edit(null)) %>', JSON.stringify(model), function(data)
			{
				if(!addValidationErrorsFromServerResponse(data, false))
				{
					jQuery('#btnCancel').click()
				}
				unblockUI()
			}, null, null, false)
		}

		jQuery(document).ready(function ()
		{
            //alert(<%= Model.InstitutionID %>);
			jQuery('#btnCancel').click(function () { safeNavigate('<%: Url.Generate<InstitutionController>(c => c.View(null)) %>') })			

			jQuery('#btnSubmit').click(function ()
			{
			    var isError = revalidatePage(jQuery('.data'), false)
			    //теперь файлы необязательны
				/*if (accFileID == null && jQuery('#postAccreditationFile').val() == '' && jQuery('#postAccreditationFile').parent().siblings('a').html() == '')
				{
					addValidationError(jQuery('#postAccreditationFile'), 'Необходимо выбрать файл аккредитации', false)
					isError = true
				}
				if (licFileID == null && jQuery('#postLicenseFile').val() == '' && jQuery('#postLicenseFile').parent().siblings('a').html() == '')
				{
					addValidationError(jQuery('#postLicenseFile'), 'Необходимо выбрать файл лицензии', false)
					isError = true
				}*/
				isError |= checkFileSelectError(jQuery('#postLicenseFile'))
				isError |= checkFileSelectError(jQuery('#postAccreditationFile'))
                //isError |= checkFileSelectError(jQuery('#postRulesFile'))
				isError |= checkFileSelectError(jQuery('#postHostelFile'))
				if(jQuery('#HasHostel').attr("checked") && jQuery('#HostelCapacity').val() == '')
				{
					addValidationError(jQuery('#HostelCapacity'), "Поле \"Количество мест\" обязательно для заполнения.", false)
					isError = true
				}
				if(!isError)
					submitLicFile()
			})

			jQuery('#hostelFileDelete').click(function() {
					hosFileDeleted = true
					jQuery('#hostelFileDelete').parent().remove()
					jQuery('#btnSubmit').blur()
					return false
			  })
			jQuery('#licFileDelete').click(function() {
			    licFileDeleted = true
			    jQuery('#licFileDelete').parent().remove()
			    jQuery('#btnSubmit').blur()
			    return false
			})
            <%for (int i = 2012; i <= DateTime.Now.Year; i++)
                {%>
			jQuery('#rulesFileDelete<%=i %>').click(function() {
			    jQuery('#rulesFileDelete<%=i %>').parent().remove()
			    jQuery('#btnSubmit').blur()
                /*if (confirm("ДА??")) {
                jQuery('#default').ajaxSubmit({
			    method: 'POST',
			    url: '<%= Url.Generate<InstitutionController>(x => x.Delete(DateTime.Now.Year,i)) %>',
			    error: showAsyncFilePostError,
			    })
                

			    jQuery('#rulesFileDelete<%=i %>').parent().remove()
			    jQuery('#btnSubmit').blur() }*/
			    return false
			})
            <%} %>
            jQuery('#accFileDelete').click(function() {
			    accFileDeleted = true
			    jQuery('#accFileDelete').parent().remove()
			    jQuery('#btnSubmit').blur()
			    return false
			})
			jQuery('#postLicenseFile').change(function() {licFileID = null })
			jQuery('#postAccreditationFile').change(function() {accFileID = null})
            jQuery('#postRulesFile').change(function() {rulFileID = null})
			jQuery('#postHostelFile').change(function() {accFileID = null})
			jQuery(".datePicker").datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-40:+0' })

			jQuery('#HasHostel').change(function() { if(jQuery(this).attr("checked")) jQuery('#spHostelData,#spHostelData2').show(); else jQuery('#spHostelData,#spHostelData2').hide() })
			if(!jQuery('#HasHostel').attr("checked")) jQuery('#spHostelData,#spHostelData2').hide()
		});

            function BuildSelect() 
    {
        var htmlSelect = document.getElementById("bass_form");
        for (var j = 2012; j <= <%= DateTime.Now.AddYears(1).Year %>; j += 1)
        {
            htmlSelect.options[j-2012] = new Option(j, j)
        }
        return true;
    } 
    $("#dialog").dialog({
        autoOpen: false,
        width : 600,

        modal: true,
        buttons: { "Добавить ": function () {
            submitRulesFile();
        } }

    });
    function simdil() {
        BuildSelect();
        $("#dialog").dialog("open");
    }
	</script>
</asp:Content>
