<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.Administration.UserViewModel>"
	MasterPageFile="~/Views/Shared/Site.Master" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent">
	Администирование - пользователь ОО
</asp:Content>
<asp:Content runat="server" ID="PageTitle" ContentPlaceHolderID="PageTitle">
	Учетная запись пользователя ОО
</asp:Content>
<asp:Content runat="server" ID="PageSubtitle" ContentPlaceHolderID="PageSubtitle">
	
</asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
    <br />
    <br />
	<table class="tableAdmin">
		<colgroup>
			<col width="200px" />
		</colgroup>
		<tbody>
			<tr>
				<td class="caption">
					<%=Html.LabelFor(m => m.FullName)%>:
				</td>
				<td>
					<%= Html.CommonInputReadOnly(Model.FullName) %>
				</td>
			</tr>
			<tr>
				<td class="caption">
					<%=Html.LabelFor(m => m.EmailLogin)%>:
				</td>
				<td>
					<%= Html.CommonInputReadOnly(Model.EmailLogin) %>					
				</td>
			</tr>
			<tr>
				<td class="caption">
					<%=Html.LabelFor(m => m.Position)%>:
				</td>
				<td>
					<%= Html.CommonInputReadOnly(Model.Position) %>					
				</td>
			</tr>
        <% if (Model.MainRole == "User")
        { %>
        <tr><td colspan="3">&nbsp;</td></tr>   
        <tr>            
            <td colspan="3" align="center" style="font-size:16px; font-weight:bold; color:#0a4b9c"  >
            Роли пользователя ОО
            </td>            
        </tr>

        <tr><td colspan="3">&nbsp;</td></tr>   

        <tr>
        <td colspan="3" align="center">

        <table width="80%" >
		<colgroup>
			<col width="50%" />            
            <col width="20%" />
            <col width="30%" />
		</colgroup>


        
        <tr>            
            <td  class="caption">
            Управление сведениями об ОО 
            </td>
            <td>
              <input type="checkbox"  id="chbInstitutionDataDirection" onclick="CHBClick(this)" />
            </td>
            <td></td>
         </tr>
        <tr>
         <td  class="caption">
          Управление приемными кампаниями         
         </td>
         <td>
          <input type="checkbox"  id="chbCampaignDirection" onclick="CHBClick(this)" />
         </td>
         <td></td>
        </tr>
        <tr>
        <td  class="caption">
         Управление объемом и структурой приема  
       </td>
       <td>
         <input type="checkbox" id="chbCampaignDataDirection" onclick="CHBClick(this)" />
       </td>
       <td></td>
       </tr>
       <tr>
       <td class="caption">
        Управление конкурсами
       </td>
       <td>
        <input type="checkbox" id="chbCompetitiveGroupsDirection" onclick="CHBClick(this)" />
       </td>
       </tr>
       <tr>
       <td class="caption">
        Обработка заявлений                     
       </td>
       <td>
        <input type="checkbox" id="chbApplicationsDirection"  onclick="CHBClick(this)" />
       </td>
       </tr>
       <tr>
       <td class="caption">
        Управление приказами                    
       </td>
       <td>
        <input type="checkbox" id="chbOrderDirection" onclick="CHBClick(this)" />
       </td>
       </tr>
       <tr>
       <td class="caption">
       Только чтение                           
       </td>
       <td>
        <input type="checkbox" id="chbReadOnly"  onclick="CHBClick(this)"/>
      </td>
       </tr>

   <tr>
   <td colspan="3" align="center">
   <span>
   <br />
   <br />
   <input type="checkbox" id="chbFilials" />&nbsp;

    Ограничить филиалом (выберите филиал):   

    &nbsp;&nbsp;

    <select id="Filials" style="width: 230px">
    </select>    
   </span>
   </td>
   </tr> 



       </table>

       </td></tr>   
    <% }
       else
       { %>
        <tr><td colspan="3">&nbsp;</td></tr>   
        <tr>            
            <td colspan="3" align="center" style="font-size:16px; font-weight:bold; color:#0a4b9c"  >
            <%= Model.MainRole %>
            </td>            
        </tr>
        <tr><td colspan="3">&nbsp;</td></tr>   
    <% } %>
        </tbody>
	</table>

	<div style="margin-top: 15px">		
		<input id="btnSubmit" class="button3" type="button" value="Сохранить" />
		<input id="btnCancel" class="button3" type="button" value="Отмена" />
	</div>

	<script type="text/javascript">
	    var userID = '<%= Model.UserID %>';
	    var userRole = '<%= Model.MainRole %>';
	    

	    function CHBClick(el) {
	        if (el.id == "chbReadOnly")
	            if (el.checked) {
	                $('#chbOrderDirection')[0].checked = false
	                $('#chbApplicationsDirection')[0].checked = false	                
	                $('#chbCompetitiveGroupsDirection')[0].checked = false	          
	                $('#chbCampaignDataDirection')[0].checked = false	          
	                $('#chbCampaignDirection')[0].checked = false
	                $('#chbInstitutionDataDirection')[0].checked = false

	                DisableAllOtherChecks() 	                
	            }
	            else                 
                  EnableAllOtherChecks()
          }

          function DisableAllOtherChecks() 
          {
              $('#chbOrderDirection').attr('disabled', 'disabled');
              $('#chbApplicationsDirection').attr('disabled', 'disabled');
              $('#chbCompetitiveGroupsDirection').attr('disabled', 'disabled');
              $('#chbCampaignDataDirection').attr('disabled', 'disabled');
              $('#chbCampaignDirection').attr('disabled', 'disabled');
              $('#chbInstitutionDataDirection').attr('disabled', 'disabled');
          }


            function EnableAllOtherChecks()
            {
	                $('#chbOrderDirection').removeAttr('disabled')
	                $('#chbApplicationsDirection').removeAttr('disabled')
	                $('#chbCompetitiveGroupsDirection').removeAttr('disabled')
	                $('#chbCampaignDataDirection').removeAttr('disabled')
	                $('#chbCampaignDirection').removeAttr('disabled')
	                $('#chbInstitutionDataDirection').removeAttr('disabled')                                 
            }

            function ShowFilialsAndSubroles() 
            {
                var filials = JSON.parse('<%= Html.Serialize(Model.GetFilials()) %>')
                if ((filials == null) || (filials == 0)) {
                            $("#chbFilials").attr('disabled', 'disabled');
                            $("#Filials").attr('disabled', 'disabled');
                    }
                    else 
                    { 
                           

                        var content = ""
                        var filialid

                            $.each(filials, function (i, e) {
                                content += '<option value="'+ e.ID + '">' +  e.Name + '</option>'
                            })                


                            if (content != "") {
                                $("#Filials").html(content)
                                filialid = '<%= Model.GetUserFilial() %>'
                                if (filialid != "") {
                                    $("#chbFilials")[0].checked = true;
                                    //document.getElementById('Filials').selectedValue = filialid;
                                    $("#Filials")[0].value = filialid;
                                } 

                            }

                    }



	            var userSubroles = '<%= Model.Subroles %>'
	            if (userSubroles == 1) {
	                $('#chbReadOnly')[0].checked = true;
	                DisableAllOtherChecks()
	            }
	            else {	                
	                if (userSubroles & 2)
	                    $('#chbOrderDirection')[0].checked = true;
	                if (userSubroles & 4)
	                    $('#chbApplicationsDirection')[0].checked = true;
	                if (userSubroles & 8)
	                    $('#chbCompetitiveGroupsDirection')[0].checked = true;
	                if (userSubroles & 16)
	                    $('#chbCampaignDataDirection')[0].checked = true;
	                if (userSubroles & 32)
	                    $('#chbCampaignDirection')[0].checked = true;
	                if (userSubroles & 64)
	                    $('#chbInstitutionDataDirection')[0].checked = true;
	            }
	        }

	        function GetSubroles() {
	            
	            if ($('#chbReadOnly')[0].checked == true)
	                return 1;

	            var subroles = 0;
	            if ($('#chbOrderDirection')[0].checked == true)
	                subroles += 2;
	            if ($('#chbApplicationsDirection')[0].checked == true)
	                subroles += 4;
	            if ($('#chbCompetitiveGroupsDirection')[0].checked == true)
	                subroles += 8;
	            if ($('#chbCampaignDataDirection')[0].checked == true)
	                subroles += 16;
	            if ($('#chbCampaignDirection')[0].checked == true)
	                subroles += 32;
	            if ($('#chbInstitutionDataDirection')[0].checked == true)
	                subroles += 64;

	            return subroles;
	        }



	        jQuery(function () {
	            if (userRole == 'User')
	                ShowFilialsAndSubroles()
	            else
	                jQuery('#btnSubmit').attr('disabled', 'disabled');


	            jQuery('#btnCancel').click(function () { safeNavigate('<%: Url.Generate<AdministrationController>(c => c.EduList()) %>') })
	            jQuery('#btnSubmit').click(function () {
	                var model =
				{
				    UserID: userID
				};




	                model.Subroles = GetSubroles()
	                if (model.Subroles == 0) {
	                    alert('Нужно выбрать роль для пользователя')
	                    return
	                }

	                if ($('#chbFilials')[0].checked == true)
	                    model.FilialID = $('#Filials')[0].value
	                else
	                    model.FilialID = 0;


	              doPostAjax('<%= Url.Generate<AdministrationController>(c => c.SaveFbdUser(null)) %>',
				  JSON.stringify(model), function (data) {
				    if (!addValidationErrorsFromServerResponse(data, false)) {
				        jQuery('#btnCancel').click()
				    }
				}, null, null, false);
	            })
	        })
	</script>
</asp:Content>
