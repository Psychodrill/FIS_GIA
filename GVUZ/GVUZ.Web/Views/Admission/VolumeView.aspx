<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.DAL.Dapper.ViewModel.Admission.AdmissionVolumeViewModel>" %>

<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Model.Institutions" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Controllers.Admission" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.Security" %>
<%@ Import Namespace="GVUZ.DAL.Dto" %>

<%@ Register TagPrefix="gv" TagName="TabControl" Src="~/Views/Shared/Common/InstitutionsTabControl.ascx" %>
<%@ Register TagPrefix="gv" TagName="DirectionInfoPopup" Src="~/Views/Shared/Admission/DirectionInfoPopup.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Объем и структура приема</asp:Content>
<asp:Content ID="header" ContentPlaceHolderID="PageTitle" runat="server">
    Сведения об образовательной организации</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageHeaderContent">
    <%-- Стили для редактора КЦП --%>
    <style type="text/css">
        tr.kcpEdit input.kcpInput
        {
            margin: 0;
            width: 48px;
        }
        tr.kcpEdit > td
        {
            padding: 0;
        }
        td.showBottomBorder, tr.showBottomBorder > td
        {
            border-bottom: 1px solid #ededed;
        }
        input.kcpInput.disabled
        {
            background: none;
            color: gray;
            background-color: #fafafa;
        }
    </style>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/libs/knockout-3.3.0.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/libs/knockout.mapping-latest.js") %>"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="divstatement">
        <gv:TabControl runat="server" ID="tabControl" />
        <div class="content">
            <% if (!Model.AllowedCampaigns.Any()) { %>
            Для данного ОО отсутствуют приемные кампании. Объем и структуру приема задать невозможно.
            <br />
            <div>
                <% if (UserRole.CurrentUserInRole(UserRole.FBDAdmin)) { %>
                <input type="button" class="button3" value="Добавить специальности" onclick="addDirectionsDialog()" style="width: auto" />
                <% } else if (UserRole.CurrentUserInRole(UserRole.EduAdmin)) { %>
                <input type="button" class="button3" value="Разрешённые направления" onclick="addDirectionsRequest()" style="width: auto" />
                <%--<input type="button" class="button3" value="Управление списком направлений с профильными/творческими испытаниями" onclick="addProfDirectionsRequest()" style="width: auto" />--%>
                <% } %><br />
            </div>
            <% } else { %>
            <div style="margin: 10px; margin-bottom: 20px;">
                <%= Html.TableLabelFor(x => x.SelectedCampaignID)%>
                <%= Html.DropDownListExFor(x => x.SelectedCampaignID, Model.AllowedCampaigns, new { onchange = "doChangeCampaign()" })%>
            </div>
            <div>
                <% if (Model.CanEdit/* && (!GVUZ.Helper.UrlUtils.IsReadOnly(FBDUserSubroles.CampaignDataDirection))*/) { %>
                <span data-ui="editDirections">
                    <%= Url.GenerateNavLink<AdmissionController>(c => c.VolumeEdit(Model.SelectedCampaignID, 1), "Редактировать", UserRole.AdminUsrEduRole, UserRole.FbdRonUser)%>
                </span>
                <% } %>
                <% if (UserRole.CurrentUserInRole(UserRole.FBDAdmin)) { %>
                    <input type="button" class="button3" value="Добавить специальности" onclick="addDirectionsDialog()" style="width: auto" />
                <% } else if (UserRole.CurrentUserInRole(UserRole.EduAdmin)) { %>
                <input type="button" class="button3" value="Разрешенные направления" onclick="addDirectionsRequest()" style="width: auto" />
                <%--<input type="button" class="button3" value="Управление списком направлений с профильными/творческими испытаниями" onclick="addProfDirectionsRequest()" style="width: auto" />--%>
                <% } %>
                <% if (UserRole.CurrentUserInRole(UserRole.EduAdmin)) { %>
                <input id="btnSavePlan" type="button" class="button3" value="Сохранить как план" onclick="doSaveAsPlan()" style="width: auto" />
                <% if (Model.HasPlan) { %>
                <input id="btnGoToPlan" type="button" class="button3 plan" value="&nbsp;" title="Перейти к плановому объему приема" onclick="goToPlan()" />
                <% } else { %>
                <input id="btnGoToPlan" type="button" class="button3 plan disabled" value="&nbsp;" disabled="disabled" title="Перейти к плановому объему приема" onclick="goToPlan()" />
                <% } %>
                    <% if (Model.CanTransfer) { %>
                        <input type="button" class="button3 disabled" value="Переброс мест" onclick="goToTransfer()" style="width: auto"  />
                    <% } %>
             <% } %>
                <div id="errorDiv" style="color: red;"></div>
                <br />
            </div>
        </div>
        <% 
            bool showExtraColumn = UserRole.CurrentUserInRole(UserRole.FBDAdmin);
            bool showDeleteButton = showExtraColumn && Model.CanEdit/* && (!GVUZ.Helper.UrlUtils.IsReadOnly(FBDUserSubroles.CampaignDataDirection))*/;
            bool showEditButton = Model.CanEdit/* && (!GVUZ.Helper.UrlUtils.IsReadOnly(FBDUserSubroles.CampaignDataDirection))*/;
            int hrspan = showExtraColumn ? 19 : 18;
        %>
        <div class="tableContainer">
            <table class="gvuzDataGrid" style="border: 0px solid black">
            <thead>
                <tr>
                    <th colspan="3" style="border-left: 1px solid white; border-top: 1px solid white;"> &nbsp;</th>
                    <th></th>
                    <th colspan="3" class="header"> <%: Html.LabelFor(x => x.DisplayData.BudgetName)%></th>
                    <th colspan="3" class="header"> <%: Html.LabelFor(x => x.DisplayData.QuotaName) %></th>
                    <th colspan="3" class="header"> <%: Html.LabelFor(x => x.DisplayData.PaidName)%></th>
                    <th colspan="3" class="header"> <%: Html.LabelFor(x => x.DisplayData.TargetName)%></th>
                    <th class="header" style="border-left-width: 4px; border-left-style: double; border-left-color: #c0c0c0" colspan="2">Контрольные цифры приема</th>
                    <th colspan="<%= showExtraColumn ? 2 : 1 %>"></th>
                </tr>
                <tr>
                    <th><%: Html.LabelFor(x => x.DisplayData.AdmissionItemTypeID)%></th>
                    <th><%: Html.LabelFor(x => x.DisplayData.DirectionID)%></th>
                    <th><%: Html.LabelFor(x => x.DisplayData.DirectionCode)%></th>
                    <th>По УГС </th>
                    <th><%: Html.LabelFor(x => x.DisplayData.NumberBudgetO)%></th>
                    <th><%: Html.LabelFor(x => x.DisplayData.NumberBudgetOZ)%></th>
                    <th><%: Html.LabelFor(x => x.DisplayData.NumberBudgetZ)%></th>
                    <th><%: Html.LabelFor(x => x.DisplayData.NumberQuotaO)%></th>
                    <th><%: Html.LabelFor(x => x.DisplayData.NumberQuotaOZ)%></th>
                    <th><%: Html.LabelFor(x => x.DisplayData.NumberQuotaZ)%></th>
                    <th><%: Html.LabelFor(x => x.DisplayData.NumberPaidO)%></th>
                    <th><%: Html.LabelFor(x => x.DisplayData.NumberPaidOZ)%></th>
                    <th><%: Html.LabelFor(x => x.DisplayData.NumberPaidZ)%></th>
                    <th><%: Html.LabelFor(x => x.DisplayData.NumberTargetO)%></th>
                    <th><%: Html.LabelFor(x => x.DisplayData.NumberTargetOZ)%></th>
                    <th><%: Html.LabelFor(x => x.DisplayData.NumberTargetZ)%></th>
                    <th style="text-align: center; border-left-width: 4px; border-left-style: double;
                        border-left-color: #c0c0c0">
                        <%= Html.LabelFor(x => x.DisplayData.AvailableForDistribution) %>
                    </th>
                    <th style="text-align: center">
                        <%= Html.LabelFor(x => x.DisplayData.TotalDistributed) %>
                    </th>
                    <th colspan="<%= showExtraColumn ? 2 : 1 %>"></th>
                </tr>
            </thead>
            <tbody>
                <%
   int i = 0, rowIdx = 1, newAdmIdx = 1;
   foreach (var admItem in Model.TreeItems)
   {
       bool isNewAdm = true;
       foreach (var pdItem in admItem)
       {
           rowIdx++;
           string className = rowIdx % 2 == 0 ? "trline1" : "trline2";

                %>
                <tr class="trline3 <%= isNewAdm ? "spanControl" : string.Empty %>">
                    <% if (isNewAdm)
                       {
                           newAdmIdx++; %>
                    <td class="<%= newAdmIdx % 2 == 0 ? "trline1" : "trline2" %> spanControl" 
                        rowspan="<%= Model.Items.Where(x => x.AdmissionItemTypeID == pdItem[0].AdmissionItemTypeID).Count() + admItem.Count %>">
                        <%: pdItem[0].AdmissionItemTypeName%>
                    </td>
                    <% } %>
                    <td class="noBottomBorder">
                        <%: pdItem[0].ParentDirectionName %>
                    </td>
                    <td class="noBottomBorder">
                        <%: pdItem[0].ParentDirectionCode %>
                    </td>

                    <% if (Model.IsForUGS(pdItem[0].ParentDirectionID))
                         { 
                    %>
                        <td align="center">
                            <input type="checkbox" class="isForUGS"  disabled="disabled" checked="checked"/>
                                <%--<%= Model.IsForUGS(pdItem[0].ParentDirectionID) ? "checked=\"checked\"" : "" %> --%>
                        </td>
                       
                        <td class="noBottomBorder" align="right">
                                <%: Model.IsFormAvail(pdItem[0].AdmissionItemTypeID, EDSourceConst.Budget, EDFormsConst.O) ? pdItem[0].NumberBudgetO.ToString() : ""%>
                        </td>
                        <td class="noBottomBorder" align="right">
                            <%: Model.IsFormAvail(pdItem[0].AdmissionItemTypeID, EDSourceConst.Budget, EDFormsConst.OZ) ? pdItem[0].NumberBudgetOZ.ToString() : ""%>
                        </td>
                        <td class="noBottomBorder" align="right">
                            <%: Model.IsFormAvail(pdItem[0].AdmissionItemTypeID, EDSourceConst.Budget, EDFormsConst.Z) ? pdItem[0].NumberBudgetZ.ToString() : ""%>
                        </td>
                        <td class="noBottomBorder" align="right">
                            <%: Model.IsFormAvail(pdItem[0].AdmissionItemTypeID, EDSourceConst.Quota, EDFormsConst.O) ? (pdItem[0].NumberQuotaO.HasValue ?  pdItem[0].NumberQuotaO.ToString() : "0") : ""%>
                        </td>
                        <td class="noBottomBorder" align="right">
                            <%: Model.IsFormAvail(pdItem[0].AdmissionItemTypeID, EDSourceConst.Quota, EDFormsConst.OZ) ? (pdItem[0].NumberQuotaOZ.HasValue ?  pdItem[0].NumberQuotaOZ.ToString() : "0") : ""%>
                        </td>
                        <td class="noBottomBorder" align="right">
                            <%: Model.IsFormAvail(pdItem[0].AdmissionItemTypeID, EDSourceConst.Quota, EDFormsConst.Z) ? (pdItem[0].NumberQuotaZ.HasValue ?  pdItem[0].NumberQuotaZ.ToString() : "0") : ""%>
                        </td>
                        <td class="noBottomBorder" align="right">
                            <%: Model.IsFormAvail(pdItem[0].AdmissionItemTypeID, EDSourceConst.Paid, EDFormsConst.O) ? pdItem[0].NumberPaidO.ToString() : ""%>
                        </td>
                        <td class="noBottomBorder" align="right">
                            <%: Model.IsFormAvail(pdItem[0].AdmissionItemTypeID, EDSourceConst.Paid, EDFormsConst.OZ) ? pdItem[0].NumberPaidOZ.ToString() : ""%>
                        </td>
                        <td class="noBottomBorder" align="right">
                            <%: Model.IsFormAvail(pdItem[0].AdmissionItemTypeID, EDSourceConst.Paid, EDFormsConst.Z) ? pdItem[0].NumberPaidZ.ToString() : ""%>
                        </td>
                        <td class="noBottomBorder" align="right">
                            <%: Model.IsFormAvail(pdItem[0].AdmissionItemTypeID, EDSourceConst.Target, EDFormsConst.O) ? pdItem[0].NumberTargetO.ToString() : ""%>
                        </td>
                        <td class="noBottomBorder" align="right">
                            <%: Model.IsFormAvail(pdItem[0].AdmissionItemTypeID, EDSourceConst.Target, EDFormsConst.OZ) ? pdItem[0].NumberTargetOZ.ToString() : ""%>
                        </td>
                        <td class="noBottomBorder" align="right">
                            <%: Model.IsFormAvail(pdItem[0].AdmissionItemTypeID, EDSourceConst.Target, EDFormsConst.Z) ? pdItem[0].NumberTargetZ.ToString() : ""%>
                        </td>
                        <td style="border-left-width: 4px; border-left-style: double; border-left-color: #c0c0c0"
                            class="noBottomBorder" align="right">
                            <%= pdItem[0].AvailableForDistribution %>
                        </td>
                        <td class="noBottomBorder" align="right">
                            <%= pdItem[0].TotalDistributed %>
                        </td>
                        <td class="noBottomBorder">
                            <% if (pdItem[0].AvailableForDistribution > 0)
                               { %>
                            <a class="btnEdit" data-kcp="<%= pdItem[0].AdmissionVolumeId %>" href="javascript:void(0)"
                                title="Редактировать">&nbsp;</a>
                            <% } %>
                        </td>
                        <% if (showExtraColumn)
                           { %>
                        <td class="noBottomBorder">
                            <% if (showDeleteButton)
                               { %>
                            <a class="btnDelete" href="#" onclick="doDeleteDirection(<%= pdItem[0].DirectionID %>, <%= pdItem[0].AdmissionItemTypeID %>)"
                                title="Удалить">&nbsp;</a>
                            <% } %>
                        </td>
                        <% } %>
                </tr>
                    <% 
                        }
                        else
                        {
                    %> 
                    <td >
                        <input type="checkbox" class="isForUGS"  disabled="disabled" "" /> 
                    </td>
                    <td class="noBottomBorder" align="right">
                        <%:Model.IsFormAvail(pdItem[0].AdmissionItemTypeID, EDSourceConst.Budget, EDFormsConst.O) ? pdItem.Sum(x => x.NumberBudgetO).ToString() : ""%>
                    </td>
                    <td class="noBottomBorder" align="right">
                        <%:Model.IsFormAvail(pdItem[0].AdmissionItemTypeID, EDSourceConst.Budget, EDFormsConst.OZ) ? pdItem.Sum(x => x.NumberBudgetOZ).ToString() : ""%>
                    </td>
                    <td class="noBottomBorder" align="right">
                        <%:Model.IsFormAvail(pdItem[0].AdmissionItemTypeID, EDSourceConst.Budget, EDFormsConst.Z) ? pdItem.Sum(x => x.NumberBudgetZ).ToString() : ""%>
                    </td>
                    <td class="noBottomBorder" align="right">
                        <%:Model.IsFormAvail(pdItem[0].AdmissionItemTypeID, EDSourceConst.Quota, EDFormsConst.O) ?  pdItem.Sum(x => x.NumberQuotaO.HasValue ? x.NumberQuotaO.Value : 0).ToString() : ""%>
                    </td>
                    <td class="noBottomBorder" align="right">
                        <%:Model.IsFormAvail(pdItem[0].AdmissionItemTypeID, EDSourceConst.Quota, EDFormsConst.OZ) ? pdItem.Sum(x => x.NumberQuotaOZ.HasValue ? x.NumberQuotaOZ.Value : 0).ToString() : ""%>
                    </td>
                    <td class="noBottomBorder" align="right">
                        <%:Model.IsFormAvail(pdItem[0].AdmissionItemTypeID, EDSourceConst.Quota, EDFormsConst.Z) ?  pdItem.Sum(x => x.NumberQuotaZ.HasValue ? x.NumberQuotaZ.Value : 0).ToString() : ""%>
                    </td>
                    <td class="noBottomBorder" align="right">
                        <%:Model.IsFormAvail(pdItem[0].AdmissionItemTypeID, EDSourceConst.Paid, EDFormsConst.O) ?  pdItem.Sum(x => x.NumberPaidO).ToString() : ""%>
                    </td>
                    <td class="noBottomBorder" align="right">
                        <%:Model.IsFormAvail(pdItem[0].AdmissionItemTypeID, EDSourceConst.Paid, EDFormsConst.OZ) ? pdItem.Sum(x => x.NumberPaidOZ).ToString() : ""%>
                    </td>
                    <td class="noBottomBorder" align="right">
                        <%:Model.IsFormAvail(pdItem[0].AdmissionItemTypeID, EDSourceConst.Paid, EDFormsConst.Z) ?  pdItem.Sum(x => x.NumberPaidZ).ToString() : ""%>
                    </td>
                    <td class="noBottomBorder" align="right">
                        <%:Model.IsFormAvail(pdItem[0].AdmissionItemTypeID, EDSourceConst.Target, EDFormsConst.O) ? pdItem.Sum(x => x.NumberTargetO).ToString() : ""%>
                    </td>
                    <td class="noBottomBorder" align="right">
                        <%:Model.IsFormAvail(pdItem[0].AdmissionItemTypeID, EDSourceConst.Target, EDFormsConst.OZ) ? pdItem.Sum(x => x.NumberTargetOZ).ToString() : ""%>
                    </td>
                    <td class="noBottomBorder" align="right">
                        <%:Model.IsFormAvail(pdItem[0].AdmissionItemTypeID, EDSourceConst.Target, EDFormsConst.Z) ? pdItem.Sum(x => x.NumberTargetZ).ToString() : ""%>
                    </td>
                    <%--<td class="noBottomBorder" align="right"><%= pdItem.Sum(x => x.AvailableForDistribution) %></td>
            <td class="noBottomBorder" align="right"><%= pdItem.Sum(x => x.TotalDistributed) %></td>--%>
                    <td style="border-left-width: 4px; border-left-style: double; border-left-color: #c0c0c0"
                        class="noBottomBorder" colspan="<%= showExtraColumn ? 4 : 3 %>">
                        &nbsp;
                    </td>

                <%
                } 

              foreach (var item in pdItem)
              {
                  rowIdx++;
                  className = rowIdx % 2 == 0 ? "trline1" : "trline2";

                  i++;
                  var borderClass = item.GroupLast ? "" : "noBottomBorder";
                %>
                <tr class="<%= className %>">
                    <td class="<%= borderClass %>">
                        <div style="padding-left: 30px" onmouseout="hideDirectionDetails()" onmouseover="viewDirectionDetails(this, <%= item.DirectionID %>)">
                            <%: item.DirectionName %></div>
                        <td class="<%= borderClass %>">
                            <%: (item.DirectionNewCode == null ? "" : item.DirectionNewCode.Trim()) %>
                        </td>
                        <td 
                            align="center">
                        </td>

                        <% if (Model.IsForUGS(pdItem[0].ParentDirectionID))
                             { 
                        %>
                        <td class="<%= borderClass %>" align="right"> </td>
                        <td class="<%= borderClass %>" align="right"> </td>
                        <td class="<%= borderClass %>" align="right"> </td>
                        <td class="<%= borderClass %>" align="right"> </td>
                        <td class="<%= borderClass %>" align="right"> </td>
                        <td class="<%= borderClass %>" align="right"> </td>
                        <td class="<%= borderClass %>" align="right"> </td>
                        <td class="<%= borderClass %>" align="right"> </td>
                        <td class="<%= borderClass %>" align="right"> </td>
                        <td class="<%= borderClass %>" align="right"> </td>
                        <td class="<%= borderClass %>" align="right"> </td>
                        <td class="<%= borderClass %>" align="right"> </td>
                        <td class="<%= borderClass %>" align="right"> </td>
                        <td class="<%= borderClass %>" align="right"> </td>
                        <td class="<%= borderClass %>" align="right"> </td>

                         <% 
                            }
                            else
                            {
                        %> 

                        <td class="<%= borderClass %>" align="right">
                            <%: Model.IsFormAvail(item.AdmissionItemTypeID, EDSourceConst.Budget, EDFormsConst.O) ? item.NumberBudgetO.ToString() : ""%>
                        </td>
                        <td class="<%= borderClass %>" align="right">
                            <%: Model.IsFormAvail(item.AdmissionItemTypeID, EDSourceConst.Budget, EDFormsConst.OZ) ? item.NumberBudgetOZ.ToString() : ""%>
                        </td>
                        <td class="<%= borderClass %>" align="right">
                            <%: Model.IsFormAvail(item.AdmissionItemTypeID, EDSourceConst.Budget, EDFormsConst.Z) ? item.NumberBudgetZ.ToString() : ""%>
                        </td>
                        <td class="<%= borderClass %>" align="right">
                            <%: Model.IsFormAvail(item.AdmissionItemTypeID, EDSourceConst.Quota, EDFormsConst.O) ? (item.NumberQuotaO.HasValue ?  item.NumberQuotaO.ToString() : "0") : ""%>
                        </td>
                        <td class="<%= borderClass %>" align="right">
                            <%: Model.IsFormAvail(item.AdmissionItemTypeID, EDSourceConst.Quota, EDFormsConst.OZ) ? (item.NumberQuotaOZ.HasValue ?  item.NumberQuotaOZ.ToString() : "0") : ""%>
                        </td>
                        <td class="<%= borderClass %>" align="right">
                            <%: Model.IsFormAvail(item.AdmissionItemTypeID, EDSourceConst.Quota, EDFormsConst.Z) ? (item.NumberQuotaZ.HasValue ?  item.NumberQuotaZ.ToString() : "0") : ""%>
                        </td>
                        <td class="<%= borderClass %>" align="right">
                            <%: Model.IsFormAvail(item.AdmissionItemTypeID, EDSourceConst.Paid, EDFormsConst.O) ? item.NumberPaidO.ToString() : ""%>
                        </td>
                        <td class="<%= borderClass %>" align="right">
                            <%: Model.IsFormAvail(item.AdmissionItemTypeID, EDSourceConst.Paid, EDFormsConst.OZ) ? item.NumberPaidOZ.ToString() : ""%>
                        </td>
                        <td class="<%= borderClass %>" align="right">
                            <%: Model.IsFormAvail(item.AdmissionItemTypeID, EDSourceConst.Paid, EDFormsConst.Z) ? item.NumberPaidZ.ToString() : ""%>
                        </td>
                        <td class="<%= borderClass %>" align="right">
                            <%: Model.IsFormAvail(item.AdmissionItemTypeID, EDSourceConst.Target, EDFormsConst.O) ? item.NumberTargetO.ToString() : ""%>
                        </td>
                        <td class="<%= borderClass %>" align="right">
                            <%: Model.IsFormAvail(item.AdmissionItemTypeID, EDSourceConst.Target, EDFormsConst.OZ) ? item.NumberTargetOZ.ToString() : ""%>
                        </td>
                        <td class="<%= borderClass %>" align="right">
                            <%: Model.IsFormAvail(item.AdmissionItemTypeID, EDSourceConst.Target, EDFormsConst.Z) ? item.NumberTargetZ.ToString() : ""%>
                        </td>
                        <td style="border-left-width: 4px; border-left-style: double; border-left-color: #c0c0c0"
                            class="<%= borderClass %>" align="right">
                            <%= item.AvailableForDistribution %>
                        </td>
                        <td class="<%= borderClass %>" align="right">
                            <%= item.TotalDistributed %>
                        </td>
                        <td class="<%= borderClass %>">
                            <% if (item.AvailableForDistribution > 0)
                               { %>
                            <a class="btnEdit" data-kcp="<%= item.AdmissionVolumeId %>" href="javascript:void(0)"
                                title="Редактировать">&nbsp;</a>
                            <% } %>
                        </td>

                        <%
                          } 
                        %>

                        <% if (showExtraColumn)
                           { %>
                        <td class="<%= borderClass %>">
                            <% if (showDeleteButton)
                               { %>
                            <a class="btnDelete" href="#" onclick="doDeleteDirection(<%= item.DirectionID %>, <%= item.AdmissionItemTypeID %>)"
                                title="Удалить">&nbsp;</a>
                            <% } %>
                        </td>
                        <% } %>
                </tr>
                <%
                           isNewAdm = false;
            }
          }
          if (admItem != Model.TreeItems.Last())
          { %>
                <tr>
                    <td colspan="<%= hrspan %>">
                        <hr size="1" />
                    </td>
                </tr>
                <% }
      } %>
            </tbody>
        </table>
        </div>
        
        <div>
            &nbsp;</div>
        <div>
            <% if (showEditButton)
               { %>
            <span data-ui="editDirections">
                <%= Url.GenerateNavLink<AdmissionController>(c => c.VolumeEdit(Model.SelectedCampaignID, 1), "Редактировать", UserRole.AdminUsrEduRole, UserRole.FbdRonUser)%>
            </span>
            <%} %>
        </div>
    </div>
    <%} %>
    <!--/div-->
    <div id="dialog">
        <table class="gvuzDataGrid" style="border: 0px solid black">
            <tbody>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <input type="button" class="button3" value="Удалить направления" onclick="doDeleteAllowedDirectionEDU()"
                            style="width: auto" />
                        Список направлений на удаление
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td id="todeleteID">
                    </td>
                </tr>
                <tr>
                    <td>
                        <input type="button" class="button3" value="Добавить направления" onclick="doAddAllowedDirectionEDU()"
                            style="width: auto" />
                        Список направлений на добавление
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td id="allowedID">
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        Отклонённые
                    </td>
                    <td>
                        <input type="button" class="button3" value="Очистить" onclick="clearAddDenied();"
                            style="width: auto" />
                    </td>
                </tr>
                <tr id="denied">
                </tr>
            </tbody>
        </table>
    </div>

    <div id="dialogProf">
        <table class="gvuzDataGrid" style="border: 0px solid black">
            <tbody>
                <tr>
                    <td>
                        Год начала проведения ПК: &nbsp;
                        <select style="width: 10%;">
                            <option value="2016">2016</option>
                        </select>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        Список направлений с профильными вступительными испытаниями <br/>
                        <input type="button" class="button3" value="Добавить направления" onclick="doAddAllowedDirectionProf()"
                            style="width: auto" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td id="todeleteID">
                    </td>
                </tr>
                <tr>
                    <td>
                        Список направлений с творческими вступительными испытаниями <br/>
                        <input type="button" class="button3" value="Добавить направления" onclick="doAddAllowedDirectionArt()"
                            style="width: auto" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td id="allowedID">
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        Отклонённые
                    </td>
                    <td>
                        <input type="button" class="button3" value="Очистить" onclick="clearAddDenied();"
                            style="width: auto" />
                    </td>
                </tr>
                <tr id="denied">
                </tr>
            </tbody>
        </table>
    </div>
    <gv:DirectionInfoPopup runat="server" ID="dirPopup" />
    <script type="text/javascript">
        var cachedDirections = JSON.parse('<%= Html.Serialize(Model.CachedDirections) %>');

        function doChangeCampaign() {
            var campaignID = $('#SelectedCampaignID').val();
            window.location = '<%= Url.Generate<AdmissionController>(x => x.VolumeView(null, null)) %>?campaignID=' + campaignID;
        }
        //<% if(UserRole.CurrentUserInRole(UserRole.FBDAdmin)) { %>	
        function doAddAllowedDirection() {
            doPostAjax('<%= Url.Generate<AdmissionController>(c => c.AllowedDirectionsAdd()) %>', '', function(data) {
                    //alert(data);
                    jQuery('#divAllowedDirectionAdd').html(data);
                    jQuery('#divAllowedDirectionAdd').dialog({
                        modal: true,
                        width: 800,
                        title: 'Добавление специальностей',
                        buttons: {
                            "Добавить": doAddDirections,
                            "Отмена": function() { closeDialog(jQuery('#divAllowedDirectionAdd')) }
                        }
                    })
                },
                "application/x-www-form-urlencoded", "html");
        }



        function doDeleteDirection(dirID, edID) {
            confirmDialog('Вы действительно хотите удалить направление для данного ОО (из <b>всех</b> объемов приема) ?', function () {
                doPostAjax('<%= Url.Generate<AdmissionController>(c => c.AllowedDirectionsDelete(null, null)) %>', 'directionID=' + dirID + "&educationLevelID=" + edID, function (data) {
                    if (data.IsError)
                        alert(data.Message);
                    else {
                        window.location.reload(1);
                    }
                }, "application/x-www-form-urlencoded");
            });
        }
        //<%} %>

        function doAddAllowedDirectionEDU() {
            doPostAjax('<%= Url.Generate<AdmissionController>(c => c.AllowedDirectionsAddEdu()) %>', '', function (data) {
                //alert(data);
                jQuery('#divAllowedDirectionAddEdu').html(data);
                jQuery('#divAllowedDirectionAddEdu').dialog(
                {
                    modal: true,
                    width: 800,
                    title: 'Добавление специальностей',
                    buttons:
                    {
                        "Добавить": doDialog,
                        "Отмена": function () { closeDialog(jQuery('#divAllowedDirectionAddEdu')) }
                    }
                }).dialog('open');
            },
            "application/x-www-form-urlencoded", "html")
        }


        function doDeleteAllowedDirectionEDU() {
            doPostAjax('<%= Url.Generate<AdmissionController>(c => c.AllowedDirectionsDeleteEdu()) %>', '', function (data) {
                //alert(data);
                jQuery('#divAllowedDirectionAddEdu').html(data);
                jQuery('#divAllowedDirectionAddEdu').dialog({
                    modal: true,
                    width: 800,
                    title: 'Удаление специальностей',
                    buttons: {
                        "Добавить": doDeleteDirectionsEduDil,
                        "Отмена": function () { closeDialog(jQuery('#divAllowedDirectionAddEdu')) }
                    }
                }).dialog('open');
            },
            "application/x-www-form-urlencoded", "html")
        }

        function doAddAllowedDirectionProf() {
            doPostAjax('<%= Url.Generate<AdmissionController>(c => c.AllowedDirectionsAddProf()) %>', '', function (data) {
                //alert(data);
                jQuery('#divAllowedDirectionAddEdu').html(data);
                jQuery('#divAllowedDirectionAddEdu').dialog(
                {
                    modal: true,
                    width: 800,
                    title: 'Добавление направления с профильными вступительными испытаниями',
                    buttons:
                    {
                        "Добавить": doDialog,
                        "Отмена": function () { closeDialog(jQuery('#divAllowedDirectionAddEdu')) }
                    }
                }).dialog('open');
            },
            "application/x-www-form-urlencoded", "html")
        }

        function doAddAllowedDirectionArt() {
            doPostAjax('<%= Url.Generate<AdmissionController>(c => c.AllowedDirectionsAddArt()) %>', '', function (data) {
                //alert(data);
                jQuery('#divAllowedDirectionAddEdu').html(data);
                jQuery('#divAllowedDirectionAddEdu').dialog(
                {
                    modal: true,
                    width: 800,
                    title: 'Добавление направления с творческими вступительными испытаниями',
                    buttons:
                    {
                        "Добавить": doDialog,
                        "Отмена": function () { closeDialog(jQuery('#divAllowedDirectionAddEdu')) }
                    }
                }).dialog('open');
            },
            "application/x-www-form-urlencoded", "html")
        }

        //<%--menuItems[6].selected = true;--%>
        menuItems[2].selected = true;
        var dirArrAdd = [];
        var dirArrDel = [];
        var dircAdd = 0;
        var dircDel = 0;
        var dirEduAdd = [];
        var dirEduDel = [];
        var res1 = '';
        var res2 = '';
        var res3 = '';
        var commentadd = [];
        var commentdel = [];

        function doDialog() {
            doAddDirections();
            closeDialog(jQuery('#divAllowedDirectionAddEdu'));
        }

        function doDeleteDirectionsEduDil() {
            doDeleteDirectionsEdu();
            closeDialog(jQuery('#divAllowedDirectionAddEdu'));
        }

        function addDial() {
            $("#dialog").dialog({
                autoOpen: false,
                width: 1000,
                modal: true,
                buttons: { "Оставить заявку": function () {
                    // Заявка на добавление

                    for (var counti = 0; counti < dirArrAdd.length; counti++) {
                        for (var countj = 0; countj < dirArrAdd[counti].length; countj++) {
                            var model =
                            {
                                directionid: dirArrAdd[counti][countj],
                                comment: commentadd[counti],
                                admissiontype: dirEduAdd[counti]
                            };
                            <%--/*doPostAjax('<%= Url.Generate<AdmissionController>(c => c.AddDirectionRequest(null,null,null)) %>', JSON.stringify(model), function (data1) {
                            if (data1.IsError)
                            alert(data1.Message);
                            else {
                            ;
                            }
                            });*/--%>
                            jQuery.ajax({
                                url: "<%= Url.Generate<AdmissionController>(c => c.AddDirectionRequest(0,null,0)) %>",
                                contentType: 'application/json; charset=utf-8',
                                type: "POST",
                                data: JSON.stringify(model),
                                dataType: "json",
                                async: false,
                                success: function (data1) {
                                    if (data1.IsError) { }
                                    //alert(data1.Message);
                                    else {
                                        ;
                                    }
                                }
                            });
                        }
                    }
                    // На удаление
                    var denied = [];
                    for (var counti = 0; counti < dirArrDel.length; counti++) {
                        for (var countj = 0; countj < dirArrDel[counti].length; countj++) {
                            var model = {
                                directionid: dirArrDel[counti][countj],
                                comment: commentdel[counti],
                                admissiontype: dirEduDel[counti]
                            };
                            jQuery.ajax({
                                url: "<%= Url.Generate<AdmissionController>(c => c.DeleteDirectionRequest(0,null,0)) %>",
                                contentType: 'application/json; charset=utf-8',
                                type: "POST",
                                data: JSON.stringify(model),
                                dataType: "json",
                                async: false,
                                success: function (data1) {
                                    if (data1.IsError)
                                    //alert(data1.Message);
                                        denied.push(data1.Message);
                                    else {
                                        ;
                                    }
                                }
                            });
                        }
                    }
                    if (denied.length > 0) {
                        var mess = "Невозможно удалить направления: ";
                        while (denied.length > 0) {
                            mess += '"' + denied.pop() + '", ';
                        }
                        mess += "так как они находятся в конкурсах.";
                        alert(mess);
                    }

                    dirArrAdd = [];
                    dirArrDel = [];
                    dircAdd = 0;
                    dircDel = 0;
                    closeDialog(jQuery('#dialog'));

                    generateDirections();

                }
                }
            });
            $("#dialogProf").dialog({
                autoOpen: false,
                width: 1000,
                modal: true,
                title: "Редактирование списка направлений с профильными/творческими испытаниями",
                buttons: { "Оставить заявку": function () {} }
            });
        }
        var listtoAdd = [];
        var listtoDel = [];

        function cancel(number, i, ind) {
            var reqcan =
        {
            directionID: number
        };
            doPostAjax('<%= Url.Generate<AdmissionController>(x => x.CancelRequest(null)) %>', JSON.stringify(reqcan), function (data) {
                if (data.IsError) alert(data.Message);
                else {

                    var str = '#s' + number;
                    $(str).html('');
                    closeDialog(jQuery('#dialog'));
                    simdil();
                }
            });
        }

        var allComments = [];
        var textComments = [];
        var firstload = false;

        function generateDirections() {

            doPostAjax('<%= Url.Generate<AdmissionController>(x => x.RequestDirectionListToAdd()) %>', 1, function (data) {
                var tempr = '';
                jQuery.each(data.Data, function (i, e) {
                    tempr = '<div id="s' + e.ID + '"><span class=""  onclick="">' + 
                        //(e.Code == null ? '' : e.Code.trim() + '.' + (e.QualificationCode == null ? "" : e.QualificationCode.trim())) + '/' + (e.NewCode == null ? '' : 
                        e.NewCode.trim() + ' - ' + e.Name + '</span>&nbsp;&nbsp;&nbsp;<a class="btnCancel" href="#" onclick="cancel(' + e.ID + ',' + i + ',' + 0 + ')">&nbsp;</a><br/></div>';
                    //listtoAdd[i] = tempr;
                    if (res1.indexOf(tempr) == -1) {
                        res1 += tempr;
                    }
                });
                res1 += '<br/>';
                $('#allowedID').html(res1);
            });

            doPostAjax('<%= Url.Generate<AdmissionController>(x => x.RequestDirectionListToDelete()) %>', 1, function (data) {
                var tempr = '';
                jQuery.each(data.Data, function (i, e) {
                    tempr = '<div id="s' + e.ID + '"><span class="" onclick="">' 
                        //+ (e.Code == null ? '' : e.Code.trim() + '.' + (e.QualificationCode == null ? "" : e.QualificationCode.trim())) + '/' 
                        + (e.NewCode == null ? '' : e.NewCode.trim()) + ' - ' + e.Name + '</span>&nbsp;&nbsp;&nbsp;<a class="btnCancel" href="#" onclick="cancel(' + e.ID + ',' + i + ',' + 1 + ')">&nbsp;</a><br/></div>';
                    //listtoDel[i] = tempr;
                    if (res2.indexOf(tempr) == -1) {
                        res2 += tempr;
                    }
                });
                res2 += '<br/>';
                $('#todeleteID').html(res2);
            });

            doPostAjax('<%= Url.Generate<AdmissionController>(x => x.RequestDirectionListDenied()) %>', 1, function (data) {
                var tempr = '';

                jQuery.each(data.Data.Direction, function (i, e) {
                    tempr = '<div id="d' + e.ID + '"><span class="" onclick="">' 
                        //+ (e.Code == null ? '' : e.Code.trim() + '.' + (e.QualificationCode == null ? "" : e.QualificationCode.trim())) + '/' 
                        + (e.NewCode == null ? '' : e.NewCode.trim()) + ' - ' + e.Name + '</span>&nbsp;&nbsp;&nbsp;<a href="#" onclick="viewcomment(' + e.ID + ')">Посмотреть&nbspкомментарий</a><br/></div>';
                    //listtoDel[i] = tempr;
                    if (res3.indexOf(tempr) == -1) {
                        res3 += tempr;
                    }
                });
                res3 += '<br/>';
                $('#denied').html(res3);

                $.each(data.Data.Comment, function (i, e) {
                    allComments[i] = e.ID;
                    textComments[i] = e.Comment;
                });

                doPostAjax('<%= Url.Generate<AdmissionController>(x => x.RequestDisabled()) %>', 1, function (data) {
                    if (data.IsError) alert(data.Message);
                    if (data.Data && firstload) alert("В данный момент заявка, которую Вы отправили ранее, обрабатывается администратором. Её просмотр и редактирование заблокированы.");
                    if (!firstload) firstload = true;
                });
            });
        }

        function viewcomment(id) {
            jQuery.each(allComments, function (i, e) {
                if (e == id)
                    alert(textComments[i]);
            });
        }

        function simdil() {
            res1 = '';
            res2 = '';
            res3 = '';
            generateDirections();
            $("#dialog").dialog("open");
        }

        function simdilProf() {
            res1 = '';
            res2 = '';
            res3 = '';
            generateDirections();
            $("#dialogProf").dialog("open");
        }

        function clearAddDenied() {
            doPostAjax('<%= Url.Generate<AdmissionController>(x => x.ClearAllDenied()) %>', 1, function (data) {
                if (data.IsError) alert(data.Message);
                else {
                    $('#denied').html('');
                }
            });

        }

        function canceldel(id, k) {
            var str;
            jQuery.each(dirArrDel[k], function (i, e) {
                if (id == e) {
                    str = /* '#' + */e + 'href' + k;
                    dirArrDel[k].splice(i, 1);
                }
            });
            var a = document.getElementById(str);
            a.parentNode.removeChild(a);

            //$(str).html('');
        }

        function canceladd(id, k) {
            var str;
            jQuery.each(dirArrAdd[k], function (i, e) {
                if (id == e) {
                    str = /* '#' + */e + 'href' + k;
                    dirArrAdd[k].splice(i, 1);
                }
            });
            var a = document.getElementById(str)
            a.parentNode.removeChild(a);
            //$(str).html('');
        }

        //    window.onload += addDial();
        //    window.onload += generateDirections();

        window.onreadystatechahge += addDial();
        window.onreadystatechahge += generateDirections();
    </script>
    <div id="divAllowedDirectionAdd">
    </div>
    <div id="divAllowedDirectionAddEdu">
    </div>
    <div id="dlgCreatePlanSuccess" style="display: none">
        <span>Сохранение плана завершено</span>
    </div>
    <%-- Редактор КЦП (инлайновый в таблице) --%>
    <script type="text/javascript">

        var errorDisplay = $('#errorDiv');

    $(function () {

        <%-- Кастомный handler чтобы не засорять атрибут data-bind в шаблоне --%>
        ko.bindingHandlers.kcpEditor = {
            init: function() {
            }
        };

        ko.bindingHandlers.kcpEditor.preprocess = function (value, name, addBindingCallback) {
            //addBindingCallback('enable', value + '.isAvailable');
            //addBindingCallback('css', '{disabled: !' + value + '.isAvailable}');
            addBindingCallback('value', value + '.value');
            addBindingCallback('visible', value + '.isAvailable');
            addBindingCallback('css', "{'input-validation-error': " + value + ".hasError()}");
            addBindingCallback('attr', '{title: ' + value + '.errorMessage() }');
        };
        
         <%-- Элементы UI которые нужно скрывать или дизейблить при отображении редактора  --%>
        if (!window.KCPSELECTORS) {

            window.KCPSELECTORS = {
                $disableElements: $('#SelectedCampaignID, #SelectedCourse, input[data-ui], span[data-ui]>input, #btnSavePlan, #btnGoToPlan'),
                $hideElements: $('a[data-kcp], a.btnCancel'),
                setUnavailable: function () {
                    window.KCPSELECTORS.$hideElements.hide();
                    window.KCPSELECTORS.$disableElements.attr('disabled', 'disabled');
                },
                setAvailable: function () {
                    window.KCPSELECTORS.$hideElements.show();
                    window.KCPSELECTORS.$disableElements.removeAttr('disabled');
                }
            };
        }
        
         <%-- view-модель редактора (input type=textbox) для ввода значений --%>
        function BudgetFormViewModel(value, isVisible, isAvailable) {
            this.value = ko.observable(this.getValue(value));
            this.isVisible = isVisible === true;
            this.isAvailable = isVisible && isAvailable === true;
            this.hasError = ko.pureComputed(this.getHasError, this);
            this.errorMessage = ko.observable(null);
            this.value.subscribe(this.validateInput, this);
            this.invalidInput = ko.observable(false);
        }


        BudgetFormViewModel.prototype = {
            validateInput: function (v) {
                if (isNaN(Number(v)) || Number(v) < 0) {
                    this.errorMessage('Необходимо указывать только целые положительные числа');
                    this.invalidInput(true);
                } else {
                    this.errorMessage(null);
                    this.invalidInput(false);
                }
            },
            getValue: function (n) {
                if (typeof n === "undefined") {
                    n = Number(this.value());
                }

                return Number(Math.max(0, isNaN(n) ? 0 : n));
            },
            getHasError: function () {
                return this.errorMessage() != null;
            }
        };

        function BudgetLevelViewModel() {
            this.total = ko.pureComputed(this.computeBudgetLevelTotal, this);
        }

        BudgetLevelViewModel.prototype = {
            computeBudgetLevelTotal: function () {
                return this.O.getValue() + this.OZ.getValue() + this.Z.getValue();
            }
        };

        function BudgetRowViewModel() {
            this.total = ko.pureComputed(this.computeRowTotal, this);
        };

        BudgetRowViewModel.prototype = {
            computeRowTotal: function () {
                return this.Budget.total() + this.Quota.total() + this.Target.total();
            }
        };

        function BudgetEditorViewModel($dialogOpenButton, rows, disposeCallback) {
            this.$dialogOpenButton = $dialogOpenButton;
            this.rows = rows || [];
            this.modelTotal = ko.pureComputed(this.computeModelTotal, this);
            this.columnTotals = ko.pureComputed(this.computeColumnTotals, this);
            this.disposeCallback = disposeCallback;
        }

        BudgetEditorViewModel.prototype = {
            init: function () {

                if (this.initialized || this.disposed) {
                    return;
                }

                var me = this;

                this.$parent = this.$dialogOpenButton.parents('tr:first');
                this.$parent.addClass('showBottomBorder').children('td').removeClass('noBottomBorder');
                this.$spanControl = this.$parent.prevAll('tr.spanControl:first').children('td.spanControl:first');
                this.$template = $($('script#kcpInlineTemplate').html()).filter('tr.kcpEdit');
                //this.$template.hide();
                this.$template.insertAfter(this.$parent);
                //this.$template.show();
                this.$spanControl.attr('rowspan', (Number(this.$spanControl.attr('rowspan')) + this.$template.length));
                this.$totalDistributionCount = this.$dialogOpenButton.parents('td:first').prevAll('td:first');
                this.originalDistributionCountValue = this.$totalDistributionCount.text();
                this.monitors = [];
                this.$btnSave = $('a.btnSave', this.$template);
                this.$btnCancel = $('a.btnCancel', this.$template);

                this.$btnSave.bind('click', function () {
                    me.commit();
                    return false;
                });

                this.$btnCancel.bind('click', function () {
                    me.rollback();
                    errorDisplay.html('');
                    return false;
                });

                this.monitors.push(this.modelTotal.subscribe(function (total) {
                    this.$totalDistributionCount.text(total);
                }, this));

                //this.monitors.push(this.columnTotals.subscribe(console.log));

                this.$template.each(function (index, item) {
                    ko.applyBindings(me.rows[index], item);
                });

                this.$totalDistributionCount.text(this.modelTotal());

                this.initialized = true;
            },
            dispose: function () {

                if (!this.initialized || this.disposed) {
                    return;
                }

                ko.cleanNode(this.$template[0]);
                this.$spanControl.attr('rowspan', (Number(this.$spanControl.attr('rowspan')) - this.$template.length));
                this.$template.remove();
                this.$parent.removeClass('showBottomBorder').children('td').addClass('noBottomBorder');

                this.$spanControl = null;
                this.$template = null;
                this.$parent = null;
                this.$totalDistributionCount = null;
                this.$btnSave.unbind();
                this.$btnSave = null;
                this.$btnCancel.unbind();
                this.$btnCancel = null;

                for (var i = 0; i < this.monitors.length; i++) {
                    this.monitors[i].dispose();
                }

                this.monitors = null;

                this.modelTotal.dispose();
                this.columnTotals.dispose();
                this.modelTotal = null;
                this.columnTotals = null;
                this.rows = [];

                var f = this.disposeCallback;

                this.disposeCallback = null;

                if (f && typeof f === 'function') {
                    window.setTimeout(f, 100);
                }

                this.disposed = true;
            },
            computeModelTotal: function () {
                var total = Number(0);
                ko.utils.arrayForEach(this.rows, function (row) {
                    total += row.total();
                });

                return total;
            },
            computeColumnTotals: function () {
                var totals = {
                    Budget: { O: Number(0), OZ: Number(0), Z: Number(0) },
                    Quota: { O: Number(0), OZ: Number(0), Z: Number(0) },
                    Target: { O: Number(0), OZ: Number(0), Z: Number(0) }
                };

                ko.utils.arrayForEach(this.rows, function (row) {
                    for (var x in totals) {
                        for (var y in totals[x]) {
                            totals[x][y] += Number(row[x][y].getValue());
                        }
                    }
                });

                return totals;
            },
            updateViewModel: function (viewModelData) {
                <%-- Обновление данных на основе KcpUpdateViewModel возвращенной сервером при ошибке сохранения (валидации) --%>
                var sources = ['Budget', 'Quota', 'Target'];
                var forms = ['O', 'OZ', 'Z'];

                for (var levelId = 0; levelId < this.rows.length; levelId++) {
                    var r = this.rows[levelId];

                    for (var s = 0; s < sources.length; s++) {
                        var source = sources[s];

                        for (var f = 0; f < forms.length; f++) {
                            var form = forms[f];

                            if (viewModelData.BudgetLevels[levelId][source][form].ErrorMessage != null) {
                                r[source][form].value(viewModelData.BudgetLevels[levelId][source][form].Value);   
                                r[source][form].errorMessage(viewModelData.BudgetLevels[levelId][source][form].ErrorMessage);
                                errorDisplay.html(viewModelData.BudgetLevels[levelId][source][form].ErrorMessage);
                                
                            }
                        }
                    }
                }
            },
            createSubmitViewModel: function () {
                <%-- Подготовка данных для KcpUpdateViewModel для сохранения значений на сервере --%>
                var sources = ['Budget', 'Quota', 'Target'];
                var forms = ['O', 'OZ', 'Z'];
                var isValid = true;
                var submitRows = [];

                for (var r = 0; r < this.rows.length; r++) {
                    var submitRow = { BudgetLevelId: this.rows[r].BudgetLevelId, DistributedAdmissionVolumeId: this.rows[r].DistributedAdmissionVolumeId };

                    for (var s = 0; s < sources.length; s++) {
                        var source = sources[s];
                        submitRow[source] = {};
                        for (var f = 0; f < forms.length; f++) {
                            var form = forms[f];
                            submitRow[source][form] = {};
                            submitRow[source][form].Value = this.rows[r][source][form].getValue();

                            if (this.rows[r][source][form].isAvailable && this.rows[r][source][form].invalidInput()) {

                                isValid = false;
                            }
                        }
                    }

                    submitRows[r] = submitRow;
                }

                if (isValid === true) {
                    return { BudgetLevels: submitRows, AdmissionVolumeId: this.AdmissionVolumeId };                  
                }

                return null;
            },
            commit: function () {
                
                var me = this;
                var submitModel = me.createSubmitViewModel();

                if (submitModel == null) {
                    return;
                }

                doPostAjax('<%= Url.Action("UpdateDistribution") %>', ko.toJSON(submitModel), function (res) {
                    if (res.success) {
                        me.$totalDistributionCount.text(res.data.TotalDistributed);
                        me.dispose();
                        errorDisplay.html(' ');
                    } else {
                        me.updateViewModel(res.data);
                    }
                }, null, null, true);

            },
            rollback: function () {
                this.$totalDistributionCount.text(this.originalDistributionCountValue);
                this.dispose();
            },
            create: function ($btnOpen, viewModelData, callback) {
                var rows = [];
                var sources = ['Budget', 'Quota', 'Target'];
                var forms = ['O', 'OZ', 'Z'];

                for (var rowIndex = 0; rowIndex < viewModelData.BudgetLevels.length; rowIndex++) {

                    var row = new BudgetRowViewModel();

                    row.BudgetLevelId = viewModelData.BudgetLevels[rowIndex].BudgetLevelId;
                    row.DistributedAdmissionVolumeId = viewModelData.BudgetLevels[rowIndex].DistributedAdmissionVolumeId;

                    for (var s = 0; s < sources.length; s++) {
                        var source = sources[s];

                        row[source] = new BudgetLevelViewModel();

                        for (var f = 0; f < forms.length; f++) {
                            var form = forms[f];

                            var v = viewModelData.BudgetLevels[rowIndex][source][form].Value || 0;
                            var vset = viewModelData.Limits[source][form].IsSet === true;
                            var vlim = viewModelData.Limits[source][form].Limit || 0;
                            row[source][form] = new BudgetFormViewModel(v, vset, vlim > 0);
                        }
                    }

                    rows[rowIndex] = row;
                }

                var model = new BudgetEditorViewModel($btnOpen, rows, callback);
                model.AdmissionVolumeId = viewModelData.AdmissionVolumeId;
                return model;
            }
        };

        $('a[data-kcp]').bind('click', function () {
            var $btn = $(this);

            var query = { admissionVolumeId: $btn.attr('data-kcp') };

            doPostAjax('<%= Url.Action("GetDistributionOptions") %>', ko.toJSON(query), function (res) {                
                if (res.success) {
                    var model = BudgetEditorViewModel.prototype.create($btn, res.data, window.KCPSELECTORS.setAvailable);
                    window.KCPSELECTORS.setUnavailable();
                    model.init();
                }

            });
        });
    });
    </script>
    <%--Шаблон редактора КЦП --%>
    <script id="kcpInlineTemplate" type="text/html">            
    <%
    int n = Model.BudgetLevels.Select(x=>x.BudgetName).ToArray().Length;
    for (int i = 0; i < n; i++)  { %>
    <tr class="kcpEdit <%= i == (n - 1) ? "showBottomBorder" : string.Empty %>">
       <% if (i == 0) { %>
        <td class="showBottomBorder" colspan="2" rowspan="<%= n %>">
            <table class="gvuzDataGrid" style="width: 100%;border-collapse: collapse;padding: 0;margin: 0;border: 0" cellpadding="8" cellspacing="0">
                <thead>
                    <% for (int j = 0; j < n; j ++) {
                          string className = j % 2 == 0 ? "trline1" : "trline2";
                          if (j == 0) { %>
                              <tr class="<%= className %>">
                                <th rowspan="<%= n %>" style="width: 60%;font-weight: bold;text-align: right;padding-right: 8px">Уровень бюджета</th>
                                <td style="width: 40%;text-align: right;padding-right: 8px;border-right: 0"><div style="height: 18px;overflow: hidden"><%= Model.BudgetLevels.Select(x=>x.BudgetName).ToArray()[j] %></div></td>
                              </tr>                              
                          <% } else { %>
                              <tr class="<%= className %>">
                                <td style="text-align: right;padding-right: 8px;border-right: 0"><div style="height: 18px;overflow: hidden"><%= Model.BudgetLevels.Select(x=>x.BudgetName).ToArray()[j] %></div></td>
                              </tr>                          
                          <% } %>
                    <% }%>
                </thead>
            </table>  
        </td>
        <% } %>
        
        <td> </td>
		<td style="text-align: center"><input class="kcpInput" type="text" data-bind="kcpEditor: Budget.O" /></td>
		<td style="text-align: center"><input class="kcpInput" type="text" data-bind="kcpEditor: Budget.OZ" /></td>
		<td style="text-align: center"><input class="kcpInput" type="text" data-bind="kcpEditor: Budget.Z" /></td>
        
        <td style="text-align: center"><input class="kcpInput" type="text" data-bind="kcpEditor: Quota.O" /></td>
		<td style="text-align: center"><input class="kcpInput" type="text" data-bind="kcpEditor: Quota.OZ" /></td>
		<td style="text-align: center"><input class="kcpInput" type="text" data-bind="kcpEditor: Quota.Z" /></td>
                
        <% if (i == 0) { %>
            <td class="showBottomBorder" colspan="3" rowspan="<%= n %>" style="background-color: #ffffff">&nbsp;</td>
        <% } %>
        
        <td style="text-align: center"><input class="kcpInput" type="text" data-bind="kcpEditor: Target.O" /></td>
		<td style="text-align: center"><input class="kcpInput" type="text" data-bind="kcpEditor: Target.OZ" /></td>
		<td style="text-align: center"><input class="kcpInput" type="text" data-bind="kcpEditor: Target.Z" /></td>
        
        <td style="border-left-width: 4px;border-left-style: double;border-left-color: #c0c0c0;"></td>
        <td style="text-align: right;padding: 10px"><span data-bind="text: total"></span></td>
        <% if (i == 0) { %>
            <td class="showBottomBorder" style="padding: 10px;text-align: center;white-space: normal;vert-align: middle" rowspan="<%= n %>" colspan="<%= UserRole.CurrentUserInRole(UserRole.FBDAdmin) ? 2 : 1 %>">
                <a href="#" style="margin-bottom: 8px" class="btnSave" title="Сохранить">&nbsp;</a><br />
                <a href="#" class="btnCancel" title="Отменить"></a>
            </td>
        <% } %>
    </tr>
    <% } %>
    </script>

    <script type="text/javascript">
        function getSearchDirectionsDialog(cb) {
            var dialogId = '#dlg_search_directions';
            var $dialog = $(dialogId);
            
            if ($dialog.length == 0) {
                doPostAjax('<%= Url.Generate<AdmissionController>(c => c.SearchDirectionsDialog()) %>', '', function (html) {
                    $('body').append(html);
                    window.setTimeout(cb, 500, $(dialogId));
                },
                "application/x-www-form-urlencoded", "html");
            }
            else {
                window.setTimeout(cb, 500, $dialog);
            }
        }
        
        function searchDirectionsDialog(dialogOptions, modelOptions){
            getSearchDirectionsDialog(function ($dialog) {
                
                var viewModel = new SearchDirectionsDialogViewModel(modelOptions);
                
                var existingContext = ko.contextFor($dialog[0]);

                if (existingContext && ko.isObservable(existingContext.$rawData)) {
                    //ko.cleanNode($dialog[0]);
                    existingContext.$rawData().dispose();
                    existingContext.$rawData(viewModel);
                } else {
                    ko.applyBindings(ko.observable(viewModel), $dialog[0]);
                }

                if ($dialog.hasClass('ui-content-dialog')) {
                    $dialog.dialog('destroy');
                    $dialog = $('#' + $dialog.attr('id'));
                }

                $dialog.dialog(dialogOptions);
            });
        }

        function addDirectionsDialog() {

            function addSelectedDirections(selectedId){
                var model = { Items: ko.utils.arrayMap(selectedId, function(id){ return {DirectionId: id } }) };
                doPostAjax('<%= Url.Generate<AdmissionController>(c => c.AddAllowedDirections(null)) %>', JSON.stringify(model), function(){});
            }

            var dialogOptions = {
                modal: true,
                width: 700,
                title: 'Добавление специальностей',
                buttons: {
                    "Добавить": function () {
                        var selectedId = ko.contextFor(this).$rawData().checkedId();
                        window.setTimeout(addSelectedDirections, 500, selectedId);
                        $(this).dialog('close');
                    },
                    "Отмена": function () {
                        $(this).dialog('close');
                    }
                }
            };

            var modelOptions = {searchType: <%= InstitutionDirectionSearchType.IncludeAllowedDirectionAdmin.ToString("d") %>};

            searchDirectionsDialog(dialogOptions, modelOptions);
        }

        function goToPlan() {    
            window.location = '<%= Url.Generate<AdmissionController>(x => x.PlanVolumeView(Model.SelectedCampaignID)) %>';        
        }

        function goToTransfer() {
            <% if (Model.HasPlan) { %>
                window.location = '<%= Url.Generate<AdmissionController>(x => x.VolumeTransfer(Model.SelectedCampaignID)) %>';  
            <% } %>
            <% else { %>
                infoDialog('Не задан плановый объем приема');
            <% } %>
        }

        function doSaveAsPlan() {
            doPostAjax('<%= Url.Generate<AdmissionController>(c => c.CreateAdmissionPlan(Model.SelectedCampaignID)) %>',null, function(){
                var $dialog = $('#dlgCreatePlanSuccess');
        
                $dialog.dialog({
                    modal: true,
                    title: 'Сохранение плана',
                    width: 700,
                    buttons: {
                        'OK': function () {
                            $(this).dialog('close');
                        }
                    }
                });

                $('#btnGoToPlan').removeClass('disabled');
                $('#btnGoToPlan').removeAttr('disabled');
            });
        }

    </script>

    <% Html.RenderPartial("Admission/RequestDirectionsDialog", new GVUZ.Web.ViewModels.RequestDirectionsDataViewModel()); %>
    <% Html.RenderPartial("Admission/RequestProfDirectionsDialog", new GVUZ.Web.ViewModels.RequestProfDirectionsDataViewModel()); %>
</asp:Content>
