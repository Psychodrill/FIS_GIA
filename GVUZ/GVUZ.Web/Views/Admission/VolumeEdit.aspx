<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.DAL.Dapper.ViewModel.Admission.AdmissionVolumeViewModel>" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Model.Institutions" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Controllers.Admission" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Register TagPrefix="gv" TagName="TabControl" Src="~/Views/Shared/Common/InstitutionsTabControl.ascx" %>
<%@ Register TagPrefix="gv" TagName="DirectionInfoPopup" Src="~/Views/Shared/Admission/DirectionInfoPopup.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Объем и структура приема
</asp:Content>
<asp:Content ID="header" ContentPlaceHolderID="PageTitle" runat="server">Сведения об образовательной организации</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<style>
	.doubleBorder
	{
		border-right-width: 2px !important;
	}
	.doubleTopBorder
	{
		border-top-width: 2px !important;
	}
	.dirCaption
	{
		font-weight:bold;
		background-color:#f0f0f0;
	}
	.rowOdd
	{
		background-color:White;
	}
	.rowEven
	{
		background-color:#f7f7ff;
	}
</style>

<div class="divstatement">
<gv:tabcontrol runat="server" id="tabControl" />
<div class="content">
	<div style="padding-bottom: 10px">
		<%= Html.TableLabelFor(x => x.SelectedCampaignID) %>
		<b><%: Model.AllowedCampaigns.First(x => x.ID == Model.SelectedCampaignID).Name %></b>
	</div>
<div>
	<input type="button" value="Сохранить" class="button3" id="btnSaveTop" />
	<input type="button" value="Отмена" class="button3" id="btnCancelTop" />
</div>
<div id="errorPlaceTop"></div>

<table class="gvuzDataGrid" style="border:0px solid black">
	<thead>
		<tr>
			<th colspan="5" style="border-left:1px solid white;border-top:1px solid white;" class="doubleBorder">&nbsp;</th>
			<th colspan="3" class="doubleBorder doubleTopBorder"><%: Html.LabelFor(x => x.DisplayData.BudgetName) %></th>
            <th colspan="3" class="doubleBorder doubleTopBorder"><%: Html.LabelFor(x => x.DisplayData.QuotaName) %></th>
			<th colspan="3" class="doubleBorder doubleTopBorder"><%: Html.LabelFor(x => x.DisplayData.PaidName) %></th>
			<th colspan="3" class="doubleBorder doubleTopBorder"><%: Html.LabelFor(x => x.DisplayData.TargetName) %></th>
		</tr>
		<tr>
			<th><%: Html.LabelFor(x => x.DisplayData.AdmissionItemTypeID) %></th>
			<th class="doubleBorder"><%: Html.LabelFor(x => x.DisplayData.DirectionID) %></th>
			<th><%: Html.LabelFor(x => x.DisplayData.DirectionCode)%></th>
            <th>По УГС </th>
			<th class="doubleBorder doubleTopBorder"><%: Html.LabelFor(x => x.DisplayData.UID) %></th>
			<th><%: Html.LabelFor(x => x.DisplayData.NumberBudgetO) %></th>
			<th><%: Html.LabelFor(x => x.DisplayData.NumberBudgetOZ) %></th>
			<th class="doubleBorder"><%: Html.LabelFor(x => x.DisplayData.NumberBudgetZ) %></th>
			<th><%: Html.LabelFor(x => x.DisplayData.NumberQuotaO) %></th>
			<th><%: Html.LabelFor(x => x.DisplayData.NumberQuotaOZ) %></th>
			<th class="doubleBorder"><%: Html.LabelFor(x => x.DisplayData.NumberQuotaZ) %></th>
			<th><%: Html.LabelFor(x => x.DisplayData.NumberPaidO) %></th>
			<th><%: Html.LabelFor(x => x.DisplayData.NumberPaidOZ) %></th>
			<th class="doubleBorder"><%: Html.LabelFor(x => x.DisplayData.NumberPaidZ) %></th>
			<th><%: Html.LabelFor(x => x.DisplayData.NumberTargetO) %></th>
			<th><%: Html.LabelFor(x => x.DisplayData.NumberTargetOZ) %></th>
			<th class="doubleBorder"><%: Html.LabelFor(x => x.DisplayData.NumberTargetZ) %></th>
		</tr>
	</thead>
	<tbody>
        
		<%
			int i = 0;
			int newAdmIdx = 1;
			int groupID = 0;
			foreach (var admItem in Model.TreeItems)
			{
				var isNewAdm = true; 
				foreach (var pdItem in admItem)
				{
                    var isUGS = Convert.ToBoolean(pdItem[0].ParentDirectionID);
					groupID++;
					%>
		<tr grID="<%= groupID %>" isUGSData="1" admItemType="<%= pdItem[0].AdmissionItemTypeID %>" >
		<% if(isNewAdm) { newAdmIdx++;%>
			<td class="<%= newAdmIdx % 2 == 0 ? "trline1" : "trline2" %>" isName="1" rowspan="<%= Model.Items.Where(x => x.AdmissionItemTypeID == pdItem[0].AdmissionItemTypeID).Count() + admItem.Count %>" ><%: pdItem[0].AdmissionItemTypeName%></td>
		<%} %>

         
			<td class="noBottomBorder doubleBorder trline3"nocalc="1" parentID="1"><%: pdItem[0].ParentDirectionName %>
                 <%: Html.HiddenFor(x => x.Items[i].ParentDirectionID, new {@class = "ParentDirectionID" }) %>             
			</td>
			<td class="noBottomBorder trline3" nocalc="1" Parent ="1" >
                 <%: Html.HiddenFor(x => x.Items[i].ParentID) %>                
                <%: pdItem[0].ParentDirectionCode %>
			</td>
            <td class="noBottomBorder trline3" nocalc="1" isUGS="1">
                <%: Html.CheckBoxFor(x => x.Items[i].IsUGS, new {@class = "isUGS" , @checked = false }) %>
                 <%: Html.HiddenFor(x => x.Items[i].AdmissionVolumeId, new {@class = "AdmissionVolumeId" }) %>        
            </td>   
              <%-- TextBoxes для УГС --%>
            
          
					<td  bgcolor="#e0e0e0" nocalc="1" uid="1"><%: Html.TextBoxExFor(x => x.Items[i].UID, new {@class="uid", disabled = (pdItem[0].DisableForEditing || !isUGS), nocalc="1"})%></td>
                    <td bgcolor="#e0e0e0" nocalc="1" ugs="1"><%: Html.TextBoxExFor(x => x.Items[i].NumberBudgetO, new { @class = "ugs", disabled = pdItem[0].DisableForEditing || pdItem[0].DisableFormO || !isUGS, style = Model.IsFormAvail(pdItem[0].AdmissionItemTypeID, EDSourceConst.Budget, EDFormsConst.O) ? "" : "display:none" })%></td>
                    <td bgcolor="#e0e0e0" nocalc="1" ugs="1"><%: Html.TextBoxExFor(x => x.Items[i].NumberBudgetOZ, new { @class = "ugs", disabled = pdItem[0].DisableForEditing || pdItem[0].DisableFormOZ || !isUGS, style = Model.IsFormAvail(pdItem[0].AdmissionItemTypeID, EDSourceConst.Budget, EDFormsConst.OZ) ? "" : "display:none" })%></td>
			        <td bgcolor="#e0e0e0" ugs="1"><%: Html.TextBoxExFor(x => x.Items[i].NumberBudgetZ, new { @class = "ugs", disabled = pdItem[0].DisableForEditing || pdItem[0].DisableFormZ || !isUGS, style = Model.IsFormAvail(pdItem[0].AdmissionItemTypeID, EDSourceConst.Budget, EDFormsConst.Z) ? "" : "display:none"})%></td>

                    <td bgcolor="#e0e0e0" nocalc="1" ugs="1"><%: Html.TextBoxExFor(x => x.Items[i].NumberQuotaO, new { @class = "ugs", disabled = pdItem[0].DisableForEditing || pdItem[0].DisableFormO || !isUGS, style = Model.IsFormAvail(pdItem[0].AdmissionItemTypeID, EDSourceConst.Quota, EDFormsConst.O) && Model.IsQuotaEnabled(pdItem[0].AdmissionItemTypeID) ? "" : "display:none"})%></td>
                    <td bgcolor="#e0e0e0" nocalc="1" ugs="1"><%: Html.TextBoxExFor(x => x.Items[i].NumberQuotaOZ, new { @class = "ugs", disabled = pdItem[0].DisableForEditing || pdItem[0].DisableFormOZ || !isUGS, style = Model.IsFormAvail(pdItem[0].AdmissionItemTypeID, EDSourceConst.Quota, EDFormsConst.OZ) && Model.IsQuotaEnabled(pdItem[0].AdmissionItemTypeID) ? "" : "display:none" })%></td>
                    <td bgcolor="#e0e0e0" nocalc="1" ugs="1"><%: Html.TextBoxExFor(x => x.Items[i].NumberQuotaZ, new { @class = "ugs", disabled = pdItem[0].DisableForEditing || pdItem[0].DisableFormZ || !isUGS, style = Model.IsFormAvail(pdItem[0].AdmissionItemTypeID, EDSourceConst.Quota, EDFormsConst.Z) && Model.IsQuotaEnabled(pdItem[0].AdmissionItemTypeID) ? "" : "display:none" })%></td>

			        <td bgcolor="#e0e0e0" nocalc="1" ugs="1"><%: Html.TextBoxExFor(x => x.Items[i].NumberPaidO, new { @class = "ugs", disabled = pdItem[0].DisableForEditing || pdItem[0].DisableFormO || !isUGS, style = Model.IsFormAvail(pdItem[0].AdmissionItemTypeID, EDSourceConst.Paid, EDFormsConst.O) ? "" : "display:none" })%></td>
			        <td bgcolor="#e0e0e0" nocalc="1" ugs="1"><%: Html.TextBoxExFor(x => x.Items[i].NumberPaidOZ, new { @class = "ugs", disabled = pdItem[0].DisableForEditing || pdItem[0].DisableFormOZ || !isUGS, style = Model.IsFormAvail(pdItem[0].AdmissionItemTypeID, EDSourceConst.Paid, EDFormsConst.OZ) ? "" : "display:none" })%></td>
			        <td bgcolor="#e0e0e0" ugs="1"><%: Html.TextBoxExFor(x => x.Items[i].NumberPaidZ, new { @class = "ugs", disabled = pdItem[0].DisableForEditing || pdItem[0].DisableFormZ || !isUGS, style = Model.IsFormAvail(pdItem[0].AdmissionItemTypeID, EDSourceConst.Paid, EDFormsConst.Z) ? "" : "display:none" })%></td>
			
			        <td bgcolor="#e0e0e0" nocalc="1" ugs="1"><%: Html.TextBoxExFor(x => x.Items[i].NumberTargetO, new { @class = "ugs", disabled = pdItem[0].DisableForEditing || pdItem[0].DisableFormO || !isUGS, style = Model.IsFormAvail(pdItem[0].AdmissionItemTypeID, EDSourceConst.Target, EDFormsConst.O) ? "" : "display:none" })%></td>
			        <td bgcolor="#e0e0e0" nocalc="1" ugs="1"><%: Html.TextBoxExFor(x => x.Items[i].NumberTargetOZ, new { @class = "ugs", disabled = pdItem[0].DisableForEditing || pdItem[0].DisableFormOZ || !isUGS, style = Model.IsFormAvail(pdItem[0].AdmissionItemTypeID, EDSourceConst.Target, EDFormsConst.OZ) ? "" : "display:none" })%></td>
			        <td bgcolor="#e0e0e0" nocalc="1" ugs="1"><%: Html.TextBoxExFor(x => x.Items[i].NumberTargetZ, new { @class = "ugs", disabled = pdItem[0].DisableForEditing || pdItem[0].DisableFormZ || !isUGS, style = Model.IsFormAvail(pdItem[0].AdmissionItemTypeID, EDSourceConst.Target, EDFormsConst.Z) ? "" : "display:none" })%></td>
              </tr>
					<%        foreach (var item in pdItem)
                        {

                            var k = i;
                            i++;
                            var borderClass =  "noBottomBorder ";
                            var backClass = i % 2 == 0 ? "trline1" : "trline2";
                                                       
						 %>
		<tr admID="<%= item.AdmissionItemTypeID %>" dirID="<%= item.DirectionID %>" isDataRow="1" class="<%= backClass %>" grID="<%= groupID %>">

             <%-- TextBoxes для отдельных направлений --%>			

			<td class="<%= borderClass + backClass %> doubleBorder" nocalc="1" ><div style="padding-left:30px;<%= item.DisableForEditing ? "font-style:italic;color:gray": "" %>" <%--
			--%> title='<%= item.DisableForEditing ? "Есть конкурсы. Невозможно изменить данные" : "" %>' onmouseout="hideDirectionDetails()" onmouseover="viewDirectionDetails(this, <%= item.DirectionID %>)"><%: Model.Items[k].DirectionName %></div></td>
            <td nocalc="1"><%: (Model.Items[k].DirectionNewCode == null ? "" : Model.Items[k].DirectionNewCode.Trim()) %></td>

            <td AdmissionVolumeId ="1"> <%: Html.HiddenFor(x => x.Items[k].AdmissionVolumeId, new {@class = "AdmissionVolumeId" }) %>   </td>

			<td class="<%= borderClass + backClass %> doubleBorder" nocalc="1" uid="1"><%: Html.TextBoxExFor(x => x.Items[k].UID, new {@class="uid", disabled = (item.DisableForEditing || isUGS), nocalc="1"})%></td>			

            <td class="<%= borderClass + backClass %>" data="i"><%: Html.TextBoxExFor(x => x.Items[k].NumberBudgetO, new { @class = "numeric", disabled = item.DisableForEditing || item.DisableFormO || isUGS, style = Model.IsFormAvail(item.AdmissionItemTypeID, EDSourceConst.Budget, EDFormsConst.O) ? "" : "opacity: 0;", value = 0})%></td>
			<td class="<%= borderClass + backClass %>" data="1"><%: Html.TextBoxExFor(x => x.Items[k].NumberBudgetOZ, new { @class = "numeric", disabled = item.DisableForEditing || item.DisableFormOZ || isUGS, style = Model.IsFormAvail(item.AdmissionItemTypeID, EDSourceConst.Budget, EDFormsConst.OZ)  ? "" : "opacity: 0;" })%></td>
			<td class="<%= borderClass + backClass %> doubleBorder" data="1"><%: Html.TextBoxExFor(x => x.Items[k].NumberBudgetZ, new { @class = "numeric", disabled = item.DisableForEditing || item.DisableFormZ || isUGS, style = Model.IsFormAvail(item.AdmissionItemTypeID, EDSourceConst.Budget, EDFormsConst.Z) ? "" : "opacity: 0;" })%></td>

			<td class="<%= borderClass + backClass %>" data="1"><%: Html.TextBoxExFor(x => x.Items[k].NumberQuotaO, new { @class = "numeric", disabled = item.DisableForEditing || item.DisableFormO || isUGS, style = Model.IsFormAvail(item.AdmissionItemTypeID, EDSourceConst.Quota, EDFormsConst.O) && Model.IsQuotaEnabled(item.AdmissionItemTypeID)   ? "" : "opacity: 0;"})%></td>
            <td class="<%= borderClass + backClass %>" data="1"><%: Html.TextBoxExFor(x => x.Items[k].NumberQuotaOZ, new { @class = "numeric", disabled = item.DisableForEditing || item.DisableFormOZ || isUGS, style = Model.IsFormAvail(item.AdmissionItemTypeID, EDSourceConst.Quota, EDFormsConst.OZ) && Model.IsQuotaEnabled(item.AdmissionItemTypeID)   ? "" : "opacity: 0;"})%></td>
            <td class="<%= borderClass + backClass %> doubleBorder" data="1"><%: Html.TextBoxExFor(x => x.Items[k].NumberQuotaZ, new { @class = "numeric", disabled = item.DisableForEditing || item.DisableFormZ || Convert.ToBoolean(item.ParentDirectionID), style = Model.IsFormAvail(item.AdmissionItemTypeID, EDSourceConst.Quota, EDFormsConst.Z) && Model.IsQuotaEnabled(item.AdmissionItemTypeID)  ? "" : "opacity: 0;" })%></td>

			<td class="<%= borderClass + backClass %>" data="1"><%: Html.TextBoxExFor(x => x.Items[k].NumberPaidO, new { @class = "numeric", disabled = item.DisableForEditing || item.DisableFormO || isUGS, style = Model.IsFormAvail(item.AdmissionItemTypeID, EDSourceConst.Paid, EDFormsConst.O) ? "" : "opacity: 0;" })%></td>
			<td class="<%= borderClass + backClass %>" data="1"><%: Html.TextBoxExFor(x => x.Items[k].NumberPaidOZ, new { @class = "numeric", disabled = item.DisableForEditing || item.DisableFormOZ || isUGS, style = Model.IsFormAvail(item.AdmissionItemTypeID, EDSourceConst.Paid, EDFormsConst.OZ) ? "" : "opacity: 0;"})%></td>
			<td class="<%= borderClass + backClass %> doubleBorder" data="1"><%: Html.TextBoxExFor(x => x.Items[k].NumberPaidZ, new { @class = "numeric", disabled = item.DisableForEditing || item.DisableFormZ || Convert.ToBoolean(item.ParentDirectionID), style = Model.IsFormAvail(item.AdmissionItemTypeID, EDSourceConst.Paid, EDFormsConst.Z) ? "" : "opacity: 0;" })%></td>
			
			<td class="<%= borderClass + backClass %>" data="1"><%: Html.TextBoxExFor(x => x.Items[k].NumberTargetO, new { @class = "numeric", disabled = item.DisableForEditing || item.DisableFormO || isUGS, style = Model.IsFormAvail(item.AdmissionItemTypeID, EDSourceConst.Target, EDFormsConst.O) ? "" : "opacity: 0;"})%></td>
			<td class="<%= borderClass + backClass %>" data="1"><%: Html.TextBoxExFor(x => x.Items[k].NumberTargetOZ, new { @class = "numeric", disabled = item.DisableForEditing || item.DisableFormOZ || isUGS, style = Model.IsFormAvail(item.AdmissionItemTypeID, EDSourceConst.Target, EDFormsConst.OZ) ? "" : "opacity: 0;" })%></td>
			<td class="<%= borderClass + backClass %> doubleBorder" data="1"><%: Html.TextBoxExFor(x => x.Items[k].NumberTargetZ, new { @class = "numeric", disabled = item.DisableForEditing || item.DisableFormZ || isUGS, style = Model.IsFormAvail(item.AdmissionItemTypeID, EDSourceConst.Target, EDFormsConst.Z)  ? "" : "opacity: 0;" })%></td>

		</tr>
				<%
					isNewAdm = false;
					}
				%><%
				}
                if (admItem != Model.TreeItems.Last())
          { %><tr><td colspan="15"><hr></td></tr><% }
			}
			%>
	</tbody>
</table>

<div>&nbsp;</div>
<div id="errorPlaceBottom"></div>
<div>
	<input type="button" value="Сохранить" id="btnSave" class="button3" />
	<input type="button" value="Отмена" id="btnCancel" class="button3" />
</div>
</div>
</div>
<gv:DirectionInfoPopup runat="server" ID="dirPopup" />

<script type="text/javascript">
    var cachedDirections = JSON.parse('<%= Html.Serialize(Model.CachedDirections) %>');

    var checkbox = jQuery('table.gvuzDataGrid tbody tr[isUGSData]').find('[type=checkbox]')

    function doSubmit()
    {
        if (revalidatePage(jQuery('.gvuzDataGrid')))
        {
            jQuery('.gvuzDataGrid').find('.field-validation-error').remove().detach();
            alert('Ошибки в введённых данных. Пожалуйста, проверьте заполненные поля');
            return;
        }

        var model = { Items: [] }

        model.SelectedCampaignID = <%= Model.SelectedCampaignID %>;
        model.InstitutionId = <%=Model.InstitutionID%>;
            fillData()

        var checkbox = jQuery('table.gvuzDataGrid tbody tr[isUGSData]').find('[type=checkbox]')
        
        // Чтение данных из полей и запись в модель
        for (var i = 0; i < checkbox.length; i++) {

            // Если объем задан по УГС
            if (jQuery(checkbox[i]).is(':checked')) {

                var ugsData = jQuery('table.gvuzDataGrid tbody tr[isUGSData]')
                var els = jQuery(ugsData[i]).find('td:[ugs] input')
                var uid = jQuery(ugsData[i]).find('td:[uid] input')
                var par = jQuery(ugsData[i]).find('td:[Parent] input')
                var parId = jQuery(ugsData).find('td:[parentid] input')[0]

                model.Items.push({
                    AdmissionVolumeId:jQuery(ugsData[i]).find('.AdmissionVolumeId').val(),
                    AdmissionItemTypeID: (jQuery(ugsData[i])).attr('admItemType'),
                    IsForUGS: jQuery(checkbox[i]).is(':checked'),
                    UID: uid.val().trim(),
                    NumberBudgetO: jQuery(els[0]).val(),
                    NumberBudgetOZ: jQuery(els[1]).val(),
                    NumberBudgetZ: jQuery(els[2]).val(),
                    NumberQuotaO: isNaN(jQuery(els[3]).val()) ? null : jQuery(els[3]).val(),
                    NumberQuotaOZ: isNaN(jQuery(els[4]).val()) ? null : jQuery(els[4]).val(),
                    NumberQuotaZ: isNaN(jQuery(els[5]).val()) ? null : jQuery(els[5]).val(),
                    NumberPaidO: jQuery(els[6]).val(),
                    NumberPaidOZ: jQuery(els[7]).val(),
                    NumberPaidZ: jQuery(els[8]).val(),
                    NumberTargetO: jQuery(els[9]).val(),
                    NumberTargetOZ: jQuery(els[10]).val(),
                    NumberTargetZ: jQuery(els[11]).val(),
                    ParentDirectionID: jQuery(parId).val(),
                    ParentID: jQuery(par).val(),
                    Dummy: 0
                })

            }
                // объем задан по каждому направлению
            else {
                
                id = i + 1;
                el = jQuery('table.gvuzDataGrid tbody tr[grID="' + id + '"]')

                for (var j = 1; j < el.length; j++) {


                    var els = jQuery(el[j]).find('td:not([isName]) input')
                    model.Items.push({
                        AdmissionVolumeId: jQuery(el[j]).find('.AdmissionVolumeId').val(),
                        BudgetName: jQuery(el[j]).find('.BudgetName').val(),
                        AdmissionItemTypeID: jQuery(el[j]).attr('admID'),
                        IsForUGS: jQuery(el[j]).attr('IsUGS'),
                        DirectionID: jQuery(el[j]).attr('dirID'),
                        UID: jQuery(el[j]).find('td:[uid] input').val(),
                        NumberBudgetO: jQuery(els[2]).val(),
                        NumberBudgetOZ: jQuery(els[3]).val(),
                        NumberBudgetZ: jQuery(els[4]).val(),
                        NumberQuotaO: isNaN(jQuery(els[5]).val()) ? null : jQuery(els[5]).val(),
                        NumberQuotaOZ: isNaN(jQuery(els[6]).val()) ? null : jQuery(els[6]).val(),
                        NumberQuotaZ: isNaN(jQuery(els[7]).val()) ? null : jQuery(els[7]).val(),
                        NumberPaidO: jQuery(els[8]).val(),
                        NumberPaidOZ: jQuery(els[9]).val(),
                        NumberPaidZ: jQuery(els[10]).val(),
                        NumberTargetO: jQuery(els[11]).val(),
                        NumberTargetOZ: jQuery(els[12]).val(),
                        NumberTargetZ: jQuery(els[13]).val(),

                        Dummy: 0
                    })

                }

            }
        }
                                 
        clearValidationErrors(jQuery('#btnCancel').parent())
        clearValidationErrors(jQuery('#btnCancelTop').parent())
     <%--   doPostAjax('<%= Url.Generate<AdmissionController>(x => x.VolumeSave(null)) %>', JSON.stringify(model), function (data)--%>
        doPostAjax('<%= Url.Generate<AdmissionController>(x => x.VolumeSave(null)) %>', JSON.stringify(model), function (data)
        {
            if(data.IsError && data.Message == 'df')
            {
                var textItems = 'Количество мест, указанное для направления подготовки, не должно быть меньше суммарного количества мест, выделенных для соответствующего направления в конкурсах. Ошибочные поля выделены красным';
                var textUids = 'Введённые UID\'ы совпдают с уже существующими. Ошибочные поля выделены красным';
               // console.log(data)
                var errFlags = 0;
                for(var i = 0; i < data.Data.length; i++)
                {                    
                    var $iArr = jQuery('.gvuzDataGrid tr[dirID="' + data.Data[i].DirectionID + '"][admID="' + data.Data[i].AdmID + '"] input')
                    for(var k = 0; k < data.Data[i].ErrorIdx.length;k++)
                    {
                        var idx = data.Data[i].ErrorIdx[k].Item1 + 1;
                        if(idx == 0) errFlags |= 1; 
                        else errFlags |= 2;
                        addValidationError(jQuery($iArr[idx]), '')

                        jQuery($iArr[idx]).attr('title', 'Значение не может быть меньше суммы по конкурсам: ' + data.Data[i].ErrorIdx[k].Item2);
                    }
                }
                var err = '';
                if((errFlags & 2) > 0) err += textItems;
                if(errFlags == 3) err += '<br/>';
                if((errFlags & 1) > 0) err += textUids;
                jQuery('#errorPlaceTop,#errorPlaceBottom').html('<span class="field-validation-error">'+err+'</span>')
                return
            }
            if (!addValidationErrorsFromServerResponse(data))
            {
                goBack()
            }
            else
            {
                if(data.Message == null)
                    jQuery('#errorPlaceTop,#errorPlaceBottom').html('<span class="field-validation-error">Ошибки во введённых данных. Ошибочные поля выделены красным</span>')
            }
        })
    }
    function fillData() {
        isChanged = false
    }

    function goBack()
    {
        window.location = '<%= Url.Generate<AdmissionController>(x => x.VolumeView(Model.SelectedCampaignID, 1)) %>'
    }

    jQuery(document).ready(function ()
    {
        // Сумма по всем направлениям
        var txtInput = $('.numeric');
        

            txtInput.change(function (e) {

            var grID = $(this).closest('tr').attr('grID')
            var ugsArr = jQuery('.gvuzDataGrid tr[grID="' + grID + '"]').find('td:[ugs]')
            var parGroupArr = jQuery('.gvuzDataGrid tr[grID="' + grID + '"]').find('td.noBottomBorder:not([nocalc])').children()
            var checkboxArr = document.querySelectorAll("[type=checkbox]")

            var arrSumm = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]
            var arrVis = [1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1]

            for (var i = 0; i < parGroupArr.length; i++) {
                if (!jQuery(parGroupArr[i]).is(':visible'))
                    arrVis[i % 12] = 0;
                arrSumm[i % 12] += parseInt(jQuery(parGroupArr[i]).val() == "" ? 0 : jQuery(parGroupArr[i]).val())
            }
            for (var i = 0; i < 12; i++) {
                if (arrVis[i])
                    jQuery(ugsArr[i]).html((isNaN(arrSumm[i]) ? '0' : arrSumm[i]) + '&nbsp;')

                else
                    jQuery(ugsArr[i + 1]).html('&nbsp;')
            }
        });

        

       // Объем приема по УГС: при загрузке страницы нужно очищать поля для отдельных направлений, входящих в состав УГС
            var emptyVolumes = jQuery('.gvuzDataGrid tr').find('td.noBottomBorder:not([nocalc])').children();
            var allUids = jQuery('.gvuzDataGrid tr').find('td:[uid]').children();

            for (var i = 0; i < allUids.length; i++) {
                emptyVolumes.push(allUids[i])
            }

            for (var i = 0; i < emptyVolumes.length; i++) {
                if (jQuery(emptyVolumes[i]).is(':disabled'))
            {
                    emptyVolumes[i].value = " ";
            }
        }
     
        // При клике на чекбокс поля становятся активными для УГС и неактивными для отдельных направлений  
        var checkboxArr = document.querySelectorAll("[type=checkbox]")

        $(checkboxArr).click(function (e) {
            // находим все инпуты
            var grID = $(this).closest('tr').attr('grID')
            var parGroupArr = jQuery('.gvuzDataGrid tr[grID="' + grID + '"]').find('td.noBottomBorder:not([nocalc])').children()            
            var parGroupUids = jQuery('.gvuzDataGrid tr[grID="' + grID + '"]').find('td.noBottomBorder:[uid]').children()
            var ugsArr = jQuery('.gvuzDataGrid tr[grID="' + grID + '"]').find('td:[ugs]').children()
            var ugsUids = jQuery('.gvuzDataGrid tr[grID="' + grID + '"]').find('td:[uid]').children()

            ugsArr.push(ugsUids[0])

            for (var i = 0; i < parGroupUids.length; i++) {
                parGroupArr.push(parGroupUids[i])
            }

            //Если по УГС
            if (jQuery(checkboxArr[grID - 1]).is(':checked')) {
                for (var i = 0; i < parGroupArr.length; i++) {

                    // Все инпуты для отдельного направления делаем неактивными
                    if (jQuery(parGroupArr[i]).is(':disabled')) {

                        jQuery(parGroupArr[i]).attr('canEdit', 'false')
                        jQuery(ugsArr[i]).attr('canEdit', 'false')
                    }
                                       
                    jQuery(parGroupArr[i]).attr('disabled', 'disabled')
                    parGroupArr[i].value = " ";                                   
                }

                for (var i = 0; i < ugsArr.length; i++)
                {
                    if (!ugsArr[i].hasAttribute('canEdit')) {
                        jQuery(ugsArr[i]).removeAttr('disabled');
                    }
                    ugsArr[i].value = " ";                         
                }

            } else {
                for (var i = 0; i < ugsArr.length; i++) {
                    ugsArr[i].value = " ";
                    if (jQuery(ugsArr[i]).is(':disabled') || ugsArr[i].hasAttribute('canEdit')) {
                        jQuery(ugsArr[i]).attr('canEdit', 'false')
                        jQuery(parGroupArr[i]).attr('canEdit', 'false')
                        jQuery(parGroupArr[i + 12 * grID]).attr('canEdit', 'false')
                    }
                    jQuery(ugsArr[i]).attr('disabled', 'disabled');
                }
               
                for (var i = 0; i < parGroupArr.length; i++) {
                    parGroupArr[i].value = " ";
                    if (!parGroupArr[i].hasAttribute('canEdit')) {
                        jQuery(parGroupArr[i]).removeAttr('disabled');
                    } 
                }              
            }                      
        })


        jQuery('#btnSave,#btnSaveTop').click(doSubmit)
        jQuery('#btnCancel,#btnCancelTop').click(function ()
        {
            if (isChanged)
                confirmDialog('На странице есть несохраненные данные. Вы действительно хотите отменить редактирование?', goBack)
            else
                goBack()
        })
        jQuery('.gvuzDataGrid input[type="text"]').change(function ()
        {
            jQuery(this).addClass('input-changed');
        })
        jQuery('.gvuzDataGrid td:has(input)').css('text-align', 'center')
        fillData()
    })
    menuItems[2].selected = true;
</script>
</asp:Content>
