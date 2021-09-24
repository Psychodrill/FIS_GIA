<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.OrderOfAdmission.OrderOfAdmissionViewModel>" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<% 
    var timer = new { style = "width: 340px" };
    var labelsInsideClass = new {@class = "labelsInside"};
    var yearComboBoxClass = new { style = "width: 100px" };
%>
<table>
    <colgroup>
        <col style="width: 15%" />
        <col style="width: 20%" />
        <col style="width: 15%" />
        <col style="width: 50%" />
    </colgroup>
    <tbody>
                       <tr>      
    <td colspan="4">
<hr align="center" size="2" with ="100%" class="auto-style1" />

</td>
        </tr>
        <tr>
            <td class="labelsInside">
                <%= Html.TableLabelFor(m => m.OrderName) %>    
            </td>
            <td>
                <span><%= Model.OrderName %></span>
            </td>
            <td class="labelsInside">
                <%= Html.TableLabelFor(m => m.UID) %>
            </td>
            <td>
                <span><%= Model.UID %></span>
            </td>
        </tr>
        <tr>
            <td class="labelsInside">
                <%= Html.TableLabelFor(m => m.OrderNumber) %>
            </td>
            <td>
                <span><%= Model.OrderNumber %></span>
            </td>
            <td class="labelsInside">
                <%= Html.TableLabelFor(m => m.OrderDateText) %>
            </td>
            <td>
                <span><%= Model.OrderDateText %></span>
            </td>
        </tr>
        <tr>
            <td class="labelsInside">
                <%= Html.TableLabelFor(m => m.CampaignName) %>
            </td>
            <td>
                <span><%= Model.CampaignName %></span>
            </td>
            <td class="labelsInside">
                <%= Html.TableLabelFor(m => m.EducationLevelName) %>
            </td>
            <td>
                <span><%= Model.EducationLevelName %></span>
            </td>
        </tr>
        <tr>
            <td class="labelsInside">
                <%= Html.TableLabelFor(m => m.EducationSourceName) %>
            </td>
            <td>
                <span><%= Model.EducationSourceName %></span>
            </td>
            <td class="labelsInside">
                <%= Html.TableLabelFor(m => m.EducationFormName) %>
            </td>
            <td>
                <span><%= Model.EducationFormName %></span>
            </td>
        </tr>
        <tr>
            <td class="labelsInside">
                <%= Html.TableLabelFor(m => m.Stage) %>
            </td>
            <td>
                <span><%= Model.StageName %></span>
            </td>
            <td class="labelsInside"></td>
            <td></td>
        </tr>
        <tr>
            <td class="labelsInside">
                <%= Html.TableLabelFor(m => m.IsForBeneficiary) %>
            </td>
            <td>
                <span><%= Model.IsForBeneficiaryName %></span>
            </td>
            <td class="labelsInside">
                <%= Html.TableLabelFor(m => m.IsForeigner) %>
            </td>
            <td>
                <span><%= Model.IsForeignerName %></span>
            </td>
        </tr>
        <tr>
            <td class="labelsInside">
                <%= Html.TableLabelFor(m => m.DateCreated) %>
            </td>
            <td>
                <span><%= Model.DateCreated %></span>
            </td>
            <td class="labelsInside">
                <%= Html.TableLabelFor(m => m.DateEdited) %>
            </td>
            <td>
                <span><%= Model.DateEdited %></span>
            </td>
        </tr>
        <tr>
            <td class="labelsInside">
                <%= Html.TableLabelFor(m => m.OrderStatus) %>
            </td>
            <td>
                <span><%= Model.OrderStatusName %></span>
            </td>
            <td class="labelsInside">
                <%= Html.TableLabelFor(m => m.DatePublished) %>
            </td>
            <td>
                <span><%= Model.DatePublished %></span>
            </td>
        </tr>
                <tr>
            <td class="labelsInside">
                <%= Html.TableLabelFor(m => m.CheckOrderUID) %>
            </td>
            <td>
                <span><%= Model.CheckOrderUID %></span>
            </td>
            <td class="labelsInside">
                <%= Html.TableLabelFor(m => m.CheckOrderStatus) %>
            </td>
            <td class ="checkOrderStatus">
                <span><%= Model.CheckOrderStatus %></span>
            </td>
        </tr>
                <tr>
            <td class="labelsInside" >
                <%= Html.TableLabelFor(m => m.CheckOrderDate) %>
            </td>
            <td >
                <span><%= Model.CheckOrderDate %></span>
            </td>
            <td class="timerLabel" >
  
            </td>
            <td class="timer" id="timer" style = "width: 340px">

                    <div id="countdown" class="countdown" >
<%--                      <div class="countdown-number" style="display:inline-block">
                        <span class="days countdown-time"></span>
                        <span class="countdown-text">Дней</span>
                      </div>--%>
                      <div class="countdown-number" style="display:inline-block">
                        <span class="hours countdown-time"></span>
                        <span class="countdown-text"></span>
                      </div>
                      <div class="countdown-number"  style="display:inline-block">
                        <span class="minutes countdown-time"></span>
                        <span class="countdown-text"></span>
                      </div>
                      <div class="countdown-number" style="display:inline-block">
                        <span class="seconds countdown-time" ></span>
                        <span class="countdown-text"></span>
                      </div>
                    </div>

            </td>
        </tr>
         <tr>      
    <td colspan="4">
<hr align="center" size="2" with ="100%" class="auto-style1" />

</td>
        </tr>
    </tbody>
</table>
