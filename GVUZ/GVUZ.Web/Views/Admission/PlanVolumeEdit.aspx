<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.AdmissionVolume.PlanAdmissionVolumeViewModel>" %>

<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Model.Institutions" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Controllers.Admission" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.ViewModels.AdmissionVolume" %>

<%@ Register TagPrefix="gv" TagName="TabControl" Src="~/Views/Shared/Common/InstitutionsTabControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Объем и структура приема
</asp:Content>
<asp:Content ID="header" ContentPlaceHolderID="PageTitle" runat="server">
    Сведения об образовательной организации
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="divstatement">
        <gv:TabControl runat="server" ID="tabControl" />
        <div class="content">
            <div>
                <input type="button" value="Сохранить" class="button3" onclick="doSave();" />
                <input type="button" value="Отмена" class="button3" onclick="doCancel();" />
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
                        </tr>
                    </thead>
                    <tbody>
                        <% 
                            foreach (AdmissionVolumeLevelViewModel level in Model.Levels)
                            {
                                if (level != Model.Levels.First())
                                { %>
                        <tr>
                            <td colspan="<%=3+Model.EducationForms.Count()*Model.FinanceSources.Count() %>">
                                <hr size="1" />
                            </td>
                        </tr>
                        <% }

                               int levelSubItemsCount = level.DirectionGroups.Count() + level.DirectionGroups.Sum(x => x.Directions.Count());
                               foreach (AdmissionVolumeDirectionGroupViewModel directionGroup in level.DirectionGroups)
                               {
                                   bool isFirstDirectionGroupInLevel = (directionGroup == level.DirectionGroups.First());
                        %>

                        <tr class="trline3">
                            <% if (isFirstDirectionGroupInLevel)
                               { %>
                            <td class="trline1" rowspan="<%=levelSubItemsCount %>"><%=level.Name %></td>
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

                            <td style="text-align: right;"  data-aggregate="" data-levelId="<%=level.Id %>" data-directionGroupId="<%=directionGroup.Id %>" data-financeSourceId="<%=financeSource.Id %>" data-educationFormId="<%=educationForm.Id %>">
                               <%if(directionGroup.IsForUGS)
                                {%> 
                                        <input type="text" data-ugs="" data-aggregate=""
                                              <% if (!Model.AvailableEducationForms.Any(x => educationForm.Id == x.Id)) { %> disabled <% } %>
                                             value="  <%=(item != null)?item.Number:0%>" data-levelId="<%=level.Id %>" data-directionGroupId="<%=directionGroup.Id %>" 
                                            data-financeSourceId="<%=financeSource.Id %>" data-educationFormId="<%=educationForm.Id %>"/>                                                                                     
                               <% } else
                                {%> 
                                
                                 <%=(item != null)?item.Number:0%>
                            
                            <% }
                               } %></td>
                           <% }%>

                                   
                        </tr>
                        <% foreach (AdmissionVolumeDirectionViewModel direction in directionGroup.Directions)
                           {%>
                        <tr class="trline2">
                            <td style="padding-left: 30px;"><%=direction.Name %></td>
                            <td><%=direction.Code %></td>
                            <td></td>
                            
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
                                <input type="text" 
                                    <% if (!Model.AvailableEducationForms.Any(x => educationForm.Id == x.Id)) { %> disabled <% } %>
                                    class="numeric" onchange="handleNumberInput(this);" data-editor="" data-levelId="<%=level.Id %>" data-directionGroupId="<%=directionGroup.Id %>" data-financeSourceId="<%=financeSource.Id %>" data-educationFormId="<%=educationForm.Id %>" data-directionId="<%=direction.Id %>" value="<%=(item != null)?item.Number:0%>" />
                            </td>
                            <% }
                               }%>
                        </tr>
                        <% }
                               }
                           }%>
                              <% }%>
                          }

                            
                    </tbody>
                </table>
            </div>
            <div>
                <input type="button" value="Сохранить" class="button3" onclick="doSave();" />
                <input type="button" value="Отмена" class="button3" onclick="doCancel();" />
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var ISCHANGED = false;

        $(document).ready(function () {
            preventNotNumericInput();
        });

        function preventNotNumericInput() {
            $('.numeric[data-editor=""]').keypress(function (e) {
                if ((e.which < 48) || (e.which > 57)) {
                    e.preventDefault();
                }
            });
        }

        function doSave() {
            var saveModel = {};
            saveModel.CampaignId = '<%=Model.CampaignId %>';
            saveModel.Items = [];

            var $dataTable = $('table.gvuzDataGrid');

            var $editors = $dataTable.find('[data-editor=""]');
           
            $editors.each(function () {
                var $editor = $(this);

                var saveModelItem = {};
                saveModelItem.LevelId = $editor.attr('data-levelId');
                saveModelItem.DirectionId = $editor.attr('data-directionId');
                saveModelItem.FinanceSourceId = $editor.attr('data-financeSourceId');
                saveModelItem.EducationFormId = $editor.attr('data-educationFormId');
                saveModelItem.Number = $editor.val();
                saveModel.Items.push(saveModelItem);
            });

            var $ugs = $dataTable.find('[data-ugs=""]');
            $ugs.each(function () {
                var $ugs = $(this);

                var saveModelItem = {};
                saveModelItem.LevelId = $ugs.attr('data-levelId');
                saveModelItem.FinanceSourceId = $ugs.attr('data-financeSourceId');
                saveModelItem.EducationFormId = $ugs.attr('data-educationFormId');
                saveModelItem.ParentDirectionId = $ugs.attr('data-directionGroupId');
                saveModelItem.Number = $ugs.val();
                saveModel.Items.push(saveModelItem);
            });

           doPostAjax('<%= Url.Generate<AdmissionController>(c => c.PlanVolumeSave(null)) %>', JSON.stringify(saveModel), function () {
                goToView();
            });
        }

        function handleNumberInput(sender) {
            ISCHANGED = true;
            refreshAggregate(sender);
        } 

        function refreshAggregate(sender) {
            var $sender = $(sender);
            var $dataTable = $('table.gvuzDataGrid');

            var levelId = $sender.attr('data-levelId');
            var directionGroupId = $sender.attr('data-directionGroupId');
            var financeSourceId = $sender.attr('data-financeSourceId');
            var educationFormId = $sender.attr('data-educationFormId');

            var sum = 0;
            var $editors = $dataTable.find('[data-editor=""][data-levelId="' + levelId + '"][data-directionGroupId="' + directionGroupId + '"][data-financeSourceId="' + financeSourceId + '"][data-educationFormId="' + educationFormId + '"]');
            $editors.each(function () {
                var $editor = $(this);
                sum += parseInt($editor.val());
            });

            $aggregate = $dataTable.find('[data-aggregate=""][data-levelId="' + levelId + '"][data-directionGroupId="' + directionGroupId + '"][data-financeSourceId="' + financeSourceId + '"][data-educationFormId="' + educationFormId + '"]');
            $aggregate.text(sum);
        }

        function doCancel() { 
            if (ISCHANGED) {
                confirmDialog('На странице есть несохраненные данные. Вы действительно хотите отменить редактирование?', goToView);
            }
            else {
                goToView();
            }
        }

        function goToView() {
            window.location = '<%= Url.Generate<AdmissionController>(x => x.PlanVolumeView(Model.CampaignId)) %>';
        }
        menuItems[2].selected = true;
    </script>
</asp:Content>
