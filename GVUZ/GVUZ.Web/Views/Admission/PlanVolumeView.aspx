<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.AdmissionVolume.PlanAdmissionVolumeViewModel>" %>

<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Model.Institutions" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Controllers.Admission" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.Security" %>
<%@ Import Namespace="GVUZ.DAL.Dto" %>
<%@ Import Namespace="GVUZ.Web.ViewModels.AdmissionVolume" %>

<%@ Register TagPrefix="gv" TagName="TabControl" Src="~/Views/Shared/Common/InstitutionsTabControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Объем и структура приема
</asp:Content>
<asp:Content ID="header" ContentPlaceHolderID="PageTitle" runat="server">
    Сведения об образовательной организации
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageHeaderContent">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="divstatement">
        <gv:TabControl runat="server" ID="tabControl" />
        <div class="content">
            <div>
                <% if (UserRole.CurrentUserInRole(UserRole.EduAdmin))
                   { %>
                <input id="btnEditTop" type="button" class="button3" value="Редактировать" onclick="goToEdit();" style="width: auto" />
                <% } %>
                <input id="btnBackTop" type="button" class="button3 fact" value="&nbsp;" title="Перейти к фактическому объему приема" onclick="goToFact();" />
            </div>
            <div>
                <table class="gvuzDataGrid" style="border: 0px solid black">
                    <thead>
                        <tr>
                            <th colspan="4" style="border-left: 1px solid white; border-top: 1px solid white;"></th>
                            <% foreach (AdmissionVolumeClassificatorItemViewModel financeSource in Model.FinanceSources)
                               { %>
                            <th colspan="3">
                                <label>
                                    <%=financeSource.Name%>
                                </label>
                            </th>
                            <% } %>

                            <th style="text-align: center; border-left-width: 4px; border-left-style: double; border-left-color: #c0c0c0"
                                colspan="2">Контрольные цифры приема
                            </th>
                            <th></th>
                        </tr>
                        <tr>
                            <th>
                                <label>Уровень образования</label></th>
                            <th>
                                <label>Специальность</label></th>
                            <th>
                                <label>Код</label></th>
                            <th><label>ПО УГС</label></th>
                            <% foreach (AdmissionVolumeClassificatorItemViewModel financeSource in Model.FinanceSources)
                               {
                                   foreach (AdmissionVolumeClassificatorItemViewModel educationForm in Model.EducationForms)
                                   {%>
                            <th>
                                <label>
                                    <%=educationForm.Name%>
                                </label>
                            </th>
                            <% }
                               }%>

                            <th style="border-left-color: rgb(192, 192, 192); border-left-width: 4px; border-left-style: double;">
                                <label>Доступно для распределения</label>
                            </th>
                            <th>
                                <label>Из них распределено</label>
                            </th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        <% 
                            foreach (AdmissionVolumeLevelViewModel level in Model.Levels)
                            {
                                if (level != Model.Levels.First())
                                { %>
                        <tr>
                            <td colspan="<%=3+Model.EducationForms.Count()*Model.FinanceSources.Count()+3 %>">
                                <hr size="1" />
                            </td>
                        </tr>
                        <% }

                               int levelSubItemsCount = level.DirectionGroups.Count() + level.DirectionGroups.Sum(x => x.Directions.Count());
                               foreach (AdmissionVolumeDirectionGroupViewModel directionGroup in level.DirectionGroups)
                               {
                                   bool isFirstDirectionGroupInLevel = (directionGroup == level.DirectionGroups.First());
                        %>

                        <tr class="trline3"  data-levelId="<%=level.Id %>" data-directionGroupId="<%=directionGroup.Id %>">
                            <% if (isFirstDirectionGroupInLevel)
                               { %>
                            <td class="trline1" data-levelCell="" data-levelId="<%=level.Id %>" rowspan="<%=levelSubItemsCount %>"><%=level.Name %></td>
                            <% } %>
                            <td><%=directionGroup.Name %></td>
                            <td><%=directionGroup.Code %></td>
                            <td><%: Html.CheckBox("IsForUGS", directionGroup.IsForUGS, new { @class = "IsForUGS", disabled = true}) %></td>

                         
                          
                            <% foreach (AdmissionVolumeClassificatorItemViewModel financeSource in Model.FinanceSources)
                               {
                                   foreach (AdmissionVolumeClassificatorItemViewModel educationForm in Model.EducationForms)
                                   {
                                       PlanAdmissionVolumeItemViewModel item = Model.GetDirectionGroupItem(financeSource.Id, educationForm.Id, level.Id, directionGroup.Id);
                            %>
                        
                            <td style="text-align: right;">
                                <%=(item != null)?item.Number:0%>
                            </td>
                            <% }
                               }%>

                               <%if (directionGroup.IsForUGS) { %>
                                     <td style="text-align: right; border-left-color: rgb(192, 192, 192); border-left-width: 4px; border-left-style: double;"><%=directionGroup.AvailableForDistribution %></td>
                            <td data-originalValue="<%=directionGroup.TotalDistributed %>" data-aggregate="" data-levelId="<%=level.Id %>" data-directionGroupId="<%=directionGroup.Id %>" style="text-align: right;"><%=directionGroup.TotalDistributed %></td>
                            <td>
                                <% if (directionGroup.AvailableForDistribution > 0)
                                   { %>
                                <a class="btnEdit" href="javascript:void(0)" title="Редактировать" onclick="doEditDistribution(this);">&nbsp;</a>
                                <% } %></td>
                               <% } %>

                            <td style="border-left-color: rgb(192, 192, 192); border-left-width: 4px; border-left-style: double;" colspan="3"></td>
                        </tr>
                        <% foreach (AdmissionVolumeDirectionViewModel direction in directionGroup.Directions)
                           {%>
                        <tr class="trline2" data-levelId="<%=level.Id %>" data-directionId="<%=direction.Id %>">
                            <td style="padding-left: 30px;"><%=direction.Name %></td>
                            <td><%=direction.Code %></td>
                            <td>  </td>

                            <%if(directionGroup.IsForUGS)
                                {%> 
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>                                                                                      
                               <% } else
                                {%> 
                                        <% foreach (AdmissionVolumeClassificatorItemViewModel financeSource in Model.FinanceSources)
                               {
                                   foreach (AdmissionVolumeClassificatorItemViewModel educationForm in Model.EducationForms)
                                   {
                                       PlanAdmissionVolumeItemViewModel item = Model.GetDirectionItem(financeSource.Id, educationForm.Id, level.Id, direction.Id);
                            %>
                            <td style="text-align: right;">
                                <%=(item != null)?item.Number:0%>
                            </td>
                            <% }
                                }                                                        
                               }%> 
                            <% if (!directionGroup.IsForUGS)
                                   { %>
                                <td style="text-align: right; border-left-color: rgb(192, 192, 192); border-left-width: 4px; border-left-style: double;"><%=direction.AvailableForDistribution %></td>
                            <td data-originalValue="<%=direction.TotalDistributed %>" data-aggregate="" data-levelId="<%=level.Id %>" data-IsUGS="<%=!directionGroup.IsForUGS%>"
                                 data-directionId="<%=direction.Id %>" style="text-align: right;"><%=direction.TotalDistributed %></td>
                            <td>
                                <% } %>
                            
                                <% if (direction.AvailableForDistribution > 0 && !directionGroup.IsForUGS)
                                   { %>
                                <a class="btnEdit" href="javascript:void(0)" title="Редактировать" onclick="doEditDistribution(this);">&nbsp;</a>
                                <% } %></td>
                        </tr>
                        <% }
                               }
                           }%>
                    </tbody>
                </table>
            </div>
<%--            <div>
                <% if (UserRole.CurrentUserInRole(UserRole.EduAdmin))
                   { %>
                <input id="btnEditBottom" type="button" class="button3" value="Редактировать" onclick="goToEdit();" style="width: auto" />
                <% } %>
                <input id="btnBackBottom" type="button" class="button3 img-button-back" title="Перейти к фактическому объему приема" onclick="goToFact();" />
            </div>--%>
        </div>
    </div>
    <script type="text/javascript">
        var EDITORLEVELID = 0;

        $(document).ready(function () {
            preventNotNumericInput();
        });

        function preventNotNumericInput() {
            $('.numeric[data-editor=""]').live('keypress', function (e) {
                if ((e.which < 48) || (e.which > 57)) {
                    e.preventDefault();
                }
            });
        }

        function doEditDistribution(sender) {
            var $sender = $(sender);
            var $row = $sender.closest('tr[data-directionId]');

            if ($row.length == 0) {
                var $row = $sender.closest('tr[data-directionGroupId]');
            }

            EDITORLEVELID = $row.attr('data-levelId');

            var argsModel = {};
            argsModel.campaignId = '<%=Model.CampaignId %>';
            argsModel.levelId = $row.attr('data-levelId');
            argsModel.directionId = $row.attr('data-directionId');

            if (!argsModel.directionId)
            {
                argsModel.directionGroupId = $row.attr('data-directionGroupId');
            }
           // console.log(argsModel)

            doPostAjax('<%= Url.Generate<AdmissionController>(c => c.DistributedPlanVolumeEdit(0,0,0,0)) %>', JSON.stringify(argsModel), function (ajaxResult) {
                insertDistributionEditorRows(ajaxResult,$row);
            }, null, 'html');
        }



        function insertDistributionEditorRows(insertableRowsHtml, afterRow) { 
            var $levelCell = $('td[data-levelCell=""][data-levelId="' + EDITORLEVELID + '"]');
            var rowSpan = parseInt($levelCell.attr('rowspan'));
            rowSpan += parseInt('<%=Model.BudgetsCount %>');
            $levelCell.attr('rowspan', rowSpan);

            var $formButtons = getFormButtons();
            $formButtons.attr('disabled', 'disabled');
            $formButtons.addClass('disabled');
            
            var $rowEditButtons = getRowEditButtons();
            $rowEditButtons.hide();

            var $afterRow = $(afterRow);
            $(afterRow).after(insertableRowsHtml);
        }

        function removeDistributionEditorRows(resetAggregateValues) {
            var $distributionEditorRows = $('tr[data-distributionEditor=""]');
            $distributionEditorRows.remove();

            var $levelCell = $('td[data-levelCell=""][data-levelId="' + EDITORLEVELID + '"]');
            var rowSpan = parseInt($levelCell.attr('rowspan'));
            rowSpan -= parseInt('<%=Model.BudgetsCount %>');
            $levelCell.attr('rowspan', rowSpan);

            var $formButtons = getFormButtons();
            $formButtons.removeAttr('disabled');
            $formButtons.removeClass('disabled');

            var $rowEditButtons = getRowEditButtons();
            $rowEditButtons.show();

            if (resetAggregateValues) {
                $('[data-aggregate=""]').each(function () {
                    $aggregate = $(this);
                    var originalValue = $aggregate.attr('data-originalValue');
                    if (originalValue && originalValue != '') {
                        $aggregate.text(originalValue);
                    }
                });
            }
        }
        
        function getFormButtons() {
            return $('#btnBackTop, #btnBackBottom, #btnEditTop, #btnEditBottom');
        }

        function getRowEditButtons() {
            return $('table.gvuzDataGrid').find('.btnEdit');
        }

        function goToFact() {
            window.location = '<%= Url.Generate<AdmissionController>(x => x.VolumeView(Model.CampaignId, 1)) %>';
        }

        function goToEdit() {
            window.location = '<%= Url.Generate<AdmissionController>(x => x.PlanVolumeEdit(Model.CampaignId)) %>';
        }
        menuItems[2].selected = true;
    </script>
</asp:Content>
