<%@ Page Title="Title" Language="C#" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.OrderOfAdmission.OrderOfAdmissionEditViewModel>" MasterPageFile="~/Views/Shared/Site.Master" %>

<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

<asp:Content runat="server" ContentPlaceHolderID="PageTitle">
    Редактирование приказа <%=Model.OrderTypeNamePrepositional %> 
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageHeaderContent">
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/libs/knockout-3.3.0.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/libs/knockout.mapping-latest.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/WebUtils.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/grid/FilterViewModelBase.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/grid/PagerViewModel.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/grid/ListViewModelBase.js") %>"></script>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="TitleContent">
    Редактирование приказа <%=Model.OrderTypeNamePrepositional %>  
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="MainContent">
    <div class="divstatement notabs">
        <%= Html.ValidationSummary(true) %>
        <% using (Html.BeginForm("EditOrderSubmit", "OrderOfAdmission", FormMethod.Post))
           { %>

        <% Html.RenderPartial("EditOrderFields", Model); %>

        <input type="submit" class="button3" value="Сохранить" />
        <input id="cancelOrderForm" type="button" class="button3" value="Отмена" />
        <input type="button" class="button3" value="Опубликовать" id="publishOrder" />
        <% if (Model.AllowPublish)
           { %>
             <script type="text/javascript">
                 document.getElementById("publishOrder").disabled=false;
              </script>
        <% }
           else
           { %>
            <script type="text/javascript">
                document.getElementById("publishOrder").disabled = true;
            </script>

        <% } %>

        <% } %>
    </div>

    <h3>Заявления, включенные в приказ <%=Model.OrderTypeNamePrepositional %> </h3>
    <% Html.RenderPartial("OrderOfAdmission/ApplicationOrderList", Model.ApplicationList); %>

    <script type="text/javascript">

        $(function () {

            var orderId = parseInt('<%= Model.OrderId %>');
            
            $('#publishOrder').click(function(e) {
                e.preventDefault();
                var $regNumber = $('input#OrderNumber');
                var $regDate = $('input#OrderDate');
                
                var errors = false;
                if (!$regNumber.val() || $regNumber.val().length == 0) {
                    $regNumber.addClass('input-validation-error');
                    $regNumber.attr('title', 'Не указан регистрационный номер приказа');
                    $regNumber.one('change', function() {
                        $regNumber.attr('title', null);
                        $regNumber.removeClass('input-validation-error');
                    });
                    errors = true;
                }
                
                if (!$regDate.val() || $regDate.val().length == 0) {
                    $regDate.addClass('input-validation-error');
                    $regDate.attr('title', 'Не указана дата регистрации приказа');
                    $regDate.one('change', function() {
                        $regDate.attr('title', null);
                        $regDate.removeClass('input-validation-error');
                    });
                    errors = true;
                }

                if (!errors) {
                    publishOrderOfAdmission(orderId, $regNumber.val(), $regDate.val(), $('input#OrderName').val(), $('input#UID').val(), function() {
                        window.location.href = '<%= Url.Action("EditOrder", "OrderOfAdmission", new { id = Model.OrderId }) %>';
                    });
                }
                return false;
            });
            
            var reposted = <%= Model.Reposted.ToString().ToLowerInvariant() %>;
            var $triggerFields = $('select#CampaignId, select#EducationLevelId, select#EducationFormId, select#EducationSourceId');
            
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

            function updateCombo($combo, items, selectedValue) {
                $combo.empty();
                $combo.append(createDefaultSelectOption());
                $.each(items, function(index, item) {
                    $combo.append(createOption(item.Id, item.DisplayName));
                });
                
                if (!reposted && items.length == 1 && !$combo.data('dirty')) {
                    $combo.val(items[0].Id);
                } else {
                    $combo.val(selectedValue != null && selectedValue.toString().length > 0 ? selectedValue : '');    
                }
                
                $combo.data('dirty', true);
                //$combo.val(selectedValue != null && selectedValue.toString().length > 0 ? selectedValue : '');    
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

            function updateOrderParameters() {
                
                $triggerFields.unbind('change');
                
                var origin = $(this).attr('id');
                if (origin == 'CampaignId') {
                    reposted = false;
                    $triggerFields.data('dirty', false);
                    $stageList.data('dirty', false);
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
                    
                    //if (origin != 'EducationLevelId')
                    updateCombo($educationLevelList, data.EducationLevels.Items, data.SelectedEducationLevel);
                    //if (origin != 'EducationFormId')
                    updateCombo($educationFormList, data.EducationForms.Items, data.SelectedEducationForm);
                    //if (origin != 'EducationSourceId')
                    updateCombo($educationSourceList, data.EducationSources.Items, data.SelectedEducationSource);
                    
                    updateCombo($stageList, data.Stages.Items, data.SelectedStage);
                    if (data.Stages.Items && data.Stages.Items.length == 0) {
                        $stageList.attr('disabled', 'disabled');
                    } else {
                        $stageList.removeAttr('disabled');
                    } 
                    
                    $isForBeneficiary.val(data.IsForBeneficiary);
                    $isForBeneficiaryName.text(data.IsForBeneficiaryName);
                    $isForeigner.val(data.IsForeigner);  
                    $isForeignerName.text(data.IsForeignerName);  

                    $triggerFields.bind('change', updateOrderParameters);

                }, null, null, true);
            }


            <% if (!Model.IsPublished)
               { %>
            $(".datePicker").datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-0:+0' });
            <% } %>
            
            <% if (Model.IsNoApplications)
               { %>    
            $triggerFields.bind('change', updateOrderParameters);
            <% } %>

            //orderOfAdmissionApplicationListLoaded();
        });
    </script>
    <% Html.RenderPartial("OrderOfAdmission/Dialogs/PublishOrderDialog"); %>
</asp:Content>

