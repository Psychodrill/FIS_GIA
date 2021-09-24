<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.ApplicationsList.AcceptApplicationsListViewModel>" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

<script type="text/javascript">

    function acceptApplicationsDialog(recordId, callback, callbackScope) {
    
        function AcceptApplicationsViewModel(modelData) {
            ko.mapping.fromJS(modelData, this.readMapping, this);
        }

        AcceptApplicationsViewModel.prototype = {
            <%-- при формировании списка принимаемых заявлений делаем observable только для поля Причина решения, остальные read-only --%>
            readMapping: {   
                    'copy': 'ApplicationRecords[0]',
                    'observe': 'ApplicationRecords[0].Reason'
            },
            <%-- при формировании модели данных для приема заявлений включаем в нее только id заявления и текст причины решения --%>
            getSubmitModel: function() {
                return {
                    'ApplicationRecords':
                        ko.utils.arrayMap(this.ApplicationRecords, function(record) {
                            return { 'ApplicationId': record.ApplicationId, 'Reason': ko.utils.unwrapObservable(record.Reason) };
                        })
                };
            },
            accept: function (onSuccess) {
                var submitModel = this.getSubmitModel();
                //console.log(submitModel);
                doPostAjax('<%= Url.Action("AcceptApplications", "InstitutionApplication") %>', ko.toJSON(submitModel), function (result) {
                    if (result.success && onSuccess && typeof onSuccess === "function") {
                        onSuccess.call(this);
                    }
                }, null, null, true);
            },
            getContentPresenter: function () {
                return this.ApplicationRecords.length == 1 ? 'acceptSingleApplicationTemplate' : 'acceptMultipleApplicationsTemplate';
            }
        };
        
        var $container = $('#acceptApplicationsDialog');
        var existingContext = ko.contextFor($container[0]);
        
        function dialogSubmitted() {
            $container.dialog('close');
            //console.log('about to invoke callback ', callback, ' in scope ', callbackScope);
            if (callback != null && typeof callback === "function") {
                callback.call(callbackScope || window);
            }
        }

        function initDialog() {
            //console.log('initDialog');

            $container.dialog({
                modal: true,
                title: 'Прием заявлений',
                width: 650,
                height: 400,
                buttons: [
                    {
                        text: 'Принять',
                        click: function() {
                            var context = ko.contextFor($container[0]);
                            AcceptApplicationsViewModel.prototype.accept.call(context.$rawData(), dialogSubmitted);
                        }
                    },
                    {
                        text: 'Отмена',
                        click: function() {
                            $(this).dialog('close');
                        }
                    }
                ]
            });
        }

        var model = { applicationId: $.isArray(recordId) ? recordId : [recordId] };
        
        doPostAjax('<%= Url.Action("GetAcceptableApplications", "InstitutionApplication") %>', ko.toJSON(model), function (data) {

            var viewModel = new AcceptApplicationsViewModel(data);
            
            if (existingContext && ko.isObservable(existingContext.$rawData)) {
                existingContext.$rawData(viewModel);
            } else {
                initDialog();
                ko.applyBindings(ko.observable(viewModel), $container[0]);
            }

            $container.dialog('option', 'height', viewModel.ApplicationRecords.length > 1 ? 500 : 400).dialog('open');

        }, null, null, true);
    }    

</script>

<div id="acceptApplicationsDialog" style="display: none;overflow: auto" data-bind="template: getContentPresenter()">
  
    <script type="text/html" id="acceptSingleApplicationTemplate">
        <table class="gvuzDataGrid tableStatement2" style="width: 100%"  data-bind="with: ApplicationRecords[0]">
            <colgroup>
                <col style="width: 50%"/>
                <col style="width: 50%;text-align: right" />
            </colgroup>
            <tbody>
                <tr>
                    <td><%= Html.DisplayNameFor(x => x.RecordInfo.ApplicationNumber) %>:</td>
                    <td style="text-align: right"><span data-bind="text: ApplicationNumber"></span></td>
                </tr>
                <tr>
                    <td><%= Html.DisplayNameFor(x => x.RecordInfo.EntrantName) %>:</td>
                    <td style="text-align: right"><span data-bind="text: EntrantName"></span></td>
                </tr>
                <tr>
                    <td><%= Html.DisplayNameFor(x => x.RecordInfo.IdentityDocument)%>:</td>
                    <td style="text-align: right"><span data-bind="text: IdentityDocument"></span></td>
                </tr>
            </tbody>
            <tfoot>
                <tr>
                    <td colspan="2"><%= Html.DisplayNameFor(x => x.RecordInfo.Reason) %></td>
                </tr>
                <tr>
                <td colspan="2">
                    <textarea autocomplete="off" rows="4" style="width: 100%;resize: none" maxlength="1000" data-bind="value: Reason"></textarea>
                </td>
                </tr>
            </tfoot>
        </table>
    </script>

    <script  type="text/html" id="acceptMultipleApplicationsTemplate">
        <table class="gvuzDataGrid tableStatement2" style="width: 100%">
            <colgroup>
                <col style="width: 10%"/>
                <col style="width: 20%" />
                <col style="width: 10%" />
                <col style="width: 60%" />
            </colgroup>
            <thead>
                <tr>
                    <th style="text-align: center"><%= Html.DisplayNameFor(x => x.RecordInfo.ApplicationNumber) %></th>
                    <th style="text-align: center"><%= Html.DisplayNameFor(x => x.RecordInfo.EntrantName) %></th>
                    <th style="text-align: center"><%= Html.DisplayNameFor(x => x.RecordInfo.IdentityDocument) %></th>
                    <th style="text-align: center"><%= Html.DisplayNameFor(x => x.RecordInfo.Reason) %></th>
                </tr>
            </thead>
            <tbody data-bind="foreach: ApplicationRecords">
                <tr data-bind="css: {trline1: ($index() % 2 == 0), trline2: ($index() % 2 != 0)}">
                    <td data-bind="text: ApplicationNumber"></td>
                    <td data-bind="text: EntrantName"></td>
                    <td data-bind="text: IdentityDocument"></td>
                    <td><input type="text" data-bind="value: Reason" /></td>
                </tr>
            </tbody>
        </table>
    </script>
</div>
