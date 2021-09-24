<%@ Page Title="Title" Language="C#" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.OrderOfAdmission.OrderOfAdmissionCreateViewModel>" MasterPageFile="~/Views/Shared/Site.Master" %>

<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

<asp:Content runat="server" ContentPlaceHolderID="PageTitle">
    Добавление приказа  <%=Model.OrderTypeNamePrepositional %> 
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="TitleContent">
    Добавление приказа  <%=Model.OrderTypeNamePrepositional %> 
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="MainContent">
    <div class="divstatement notabs">
        <%= Html.ValidationSummary(true) %>
        <% using (Html.BeginForm("CreateOrderSubmit", "OrderOfAdmission", FormMethod.Post))
           { %>

        <% Html.RenderPartial("CreateOrderFields", Model); %>
        <% if (Model.ApplicationId != null)
           {
               for (int i = 0; i < Model.ApplicationId.Length; i++)
               { %>
        <input type="hidden" name="<%= string.Format("ApplicationId[{0}]", i) %>" value="<%= Model.ApplicationId[i] %>" />
        <% }
           } %>
        <%= Html.HiddenFor(m => m.FromApplication) %>
        <input type="submit" class="button3" value="Сохранить" />
        <input id="cancelOrderForm" type="button" class="button3" value="Отмена" />
        <% } %>
    </div>
    <script type="text/javascript">

        function updateCombo($combo, items, selectedValue) {
            $combo.empty();
            $combo.append(createDefaultSelectOption());
            $.each(items, function(index, item) {
                $combo.append(createOption(item.Id, item.DisplayName));
            });
                
            $combo.val(selectedValue != null && selectedValue.toString().length > 0 ? selectedValue : '');    
        }

        function createOption(value, text) {
            var $option = $('<option />');
            $option.val(value == null || value.toString().length == 0 ? '' : value);
            $option.text(text);
            return $option;
        }

        function createDefaultSelectOption() {
            return createOption('', 'Не выбрано');
        }
        
        <% if (Model.FromApplication)
           { %>
    
        $(function () {

            var $triggerFields = $('select#EducationLevelId, select#EducationFormId, select#EducationSourceId').bind('change', updateOrderParameters);

            var $campaignId = $('input#CampaignId'),
                $educationLevelList = $('select#EducationLevelId'),
                $educationFormList = $('select#EducationFormId'),
                $educationSourceList = $('select#EducationSourceId'), 
                $stageList = $('select#Stage');
            
            $('input#cancelOrderForm').click(function (e) {
                e.preventDefault();
                window.history.back();
                <% if (Model.OrderTypeId == GVUZ.Data.Model.OrderOfAdmissionType.OrderOfAdmission)
                   {
                       %>
                //window.location = '<%= Url.Action("OrdersOfAdmissionList") %>';
                <% }
                   else if (Model.OrderTypeId == GVUZ.Data.Model.OrderOfAdmissionType.OrderOfAdmissionRefuse)
                   { %>
                //window.location = '<%= Url.Action("OrdersOfAdmissionRefuseList") %>';
                <% }
                      %> 
                return false;
            });

            function updateOrderParameters() {
                
                $triggerFields.unbind('change');
                
                var model = {
                    SelectedCampaignId: $campaignId.val(),
                    SelectedEducationLevel: $educationLevelList.val(),
                    SelectedEducationForm: $educationFormList.val(),
                    SelectedEducationSource: $educationSourceList.val(), 
                    SelectedStage: $stageList.val(),
                    FromApplication: true,
                    ApplicationId: <%= Html.CustomJson(Model.ApplicationId) %>
                    };


                doPostAjax('<%= Url.Action("GetOrderParameters", "OrderOfAdmission") %>', JSON.stringify(model), function(data) {
                    
                    updateCombo($educationLevelList, data.EducationLevels.Items, data.SelectedEducationLevel);
                    updateCombo($educationFormList, data.EducationForms.Items, data.SelectedEducationForm);
                    updateCombo($educationSourceList, data.EducationSources.Items, data.SelectedEducationSource);
                    updateCombo($stageList, data.Stages.Items, data.SelectedStage);
                }, null, null, true);
            }

            $(".datePicker").datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-8:+8' });
            
        });


        <% }
           else
           { %>
        $(function () {

            var $triggerFields = $('select#CampaignId, select#EducationLevelId, select#EducationFormId, select#EducationSourceId').bind('change', updateOrderParameters);
            
            var $campaignList = $('select#CampaignId'),
                $educationLevelList = $('select#EducationLevelId'),
                $educationFormList = $('select#EducationFormId'),
                $educationSourceList = $('select#EducationSourceId'), 
                $stageList = $('select#Stage');

            var $isForBeneficiary = $('input#IsForBeneficiary');
            var $isForBeneficiaryName = $('span#IsForBeneficiaryName');
            var $isForeigner = $('input#IsForeigner');
            var $isForeignerName = $('span#IsForeignerName');

            $('input#cancelOrderForm').click(function (e) {
                e.preventDefault();
                <% if (Model.OrderTypeId == GVUZ.Data.Model.OrderOfAdmissionType.OrderOfAdmission)
                   {
                       %>
                window.location = '<%= Url.Action("OrdersOfAdmissionList") %>';
                <% }
                   else if (Model.OrderTypeId == GVUZ.Data.Model.OrderOfAdmissionType.OrderOfAdmissionRefuse)
                   { %>
                window.location = '<%= Url.Action("OrdersOfAdmissionRefuseList") %>';
                <% }
                      %> 
                return false;
            });

            function updateOrderParameters() {
                
                $triggerFields.unbind('change');
                
                var origin = $(this).attr('id');
                if (origin == 'CampaignId') {
                    $educationFormList.val('');
                    $educationSourceList.val('');
                    $educationLevelList.val(''); 
                    $stageList.val('');
                }

                var model = {
                    SelectedCampaignId: $campaignList.val(),
                    SelectedEducationLevel: $educationLevelList.val(),
                    SelectedEducationForm: $educationFormList.val(),
                    SelectedEducationSource: $educationSourceList.val(), 
                    SelectedStage: $stageList.val()
                };


                doPostAjax('<%= Url.Action("GetOrderParameters", "OrderOfAdmission") %>', JSON.stringify(model), function(data) {
                    
                    updateCombo($educationLevelList, data.EducationLevels.Items, data.SelectedEducationLevel);
                    updateCombo($educationFormList, data.EducationForms.Items, data.SelectedEducationForm);
                    updateCombo($educationSourceList, data.EducationSources.Items, data.SelectedEducationSource);
                    updateCombo($stageList, data.Stages.Items, data.SelectedStage);

                    $isForBeneficiary.val(data.IsForBeneficiary);
                    $isForBeneficiaryName.text(data.IsForBeneficiaryName);
                    $isForeigner.val(data.IsForeigner);  
                    $isForeignerName.text(data.IsForeignerName);  

                    $triggerFields.bind('change', updateOrderParameters);

                }, null, null, true);
            }

            $(".datePicker").datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-8:+8' });
            
            <% if (Model.Campaigns.Items.Count == 1)
               { %>
            $('select#CampaignId').val('<%= Model.Campaigns.Items[0].Id %>').trigger("change");         
            <% } %>
        });
        <% } %>
    </script>
</asp:Content>

