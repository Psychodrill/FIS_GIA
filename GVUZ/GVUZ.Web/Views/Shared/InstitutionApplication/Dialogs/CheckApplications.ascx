<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.ApplicationsList.CheckApplicationsListViewModel>" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<script type="text/javascript">

    function checkApplicationsDialog(applicationIdList, callback, callbackScope) {

        // модель для запроса данных с сервера
        var model = { applicationId: $.isArray(applicationIdList) ? applicationIdList : [applicationIdList] };

        var $container = $('#checkApplicationsDialogContainer');
        var existingContext = ko.contextFor($container[0]);

        function dialogSubmitted() {
            existingContext = ko.contextFor($container[0]);
            // вызываем callback (например для перезагрузки списка) только если при проверки какие-то заявления поменяли статус
            if (existingContext.$rawData().ApplicationsRemoved) {
                if (callback != null && typeof callback === "function") {
                    callback.call(callbackScope || window);
                }
            }
        }

        var topRaz = screen.height;
        var leftRaz = (screen.width - 800) / 2;

        function initDialog() {
            $container.dialog({
                modal: true,
                title: 'Результат проверки заявлений',
                width: 800,
                buttons: [
                    {
                        text: 'Закрыть',
                        click: function() {
                            $container.dialog('close');
                        }
                    }
                ],
                open: function(event, ui) {
                    $(event.target).parent().css('position', 'fixed');
                    $(event.target).parent().css('top', '250px');
                    $(event.target).parent().css('left', leftRaz);
                },
                close: function() {
                    dialogSubmitted();
                }
            });
        }

        doPostAjax('<%= Url.Action("CheckApplications", "InstitutionApplication") %>', JSON.stringify(model), function (data) {
            var viewModel = new CheckApplicationsDialogViewModel(data);

            if (existingContext && ko.isObservable(existingContext.$rawData)) {
                existingContext.$rawData(viewModel);
            } else {
                initDialog();
                ko.applyBindings(ko.observable(viewModel), $container[0]);
            }
            $container.dialog('option', 'height', viewModel.ApplicationRecords.length >= 10 ? 520 : 'auto').dialog('open');

        }, null, null, true);

        function CheckApplicationsDialogViewModel(modelData) {
            ko.mapping.fromJS(modelData, this.propertyFilterMap, this);
        }

        CheckApplicationsDialogViewModel.prototype.getContentPresenter = function () {
            return this.ApplicationRecords.length == 1 ? 'checkResultSingleApplication' : 'checkResultMultipleApplications';
        };

        CheckApplicationsDialogViewModel.prototype.propertyFilterMap = {
            'ignore': 'RecordInfo',
            'copy': ['ApplicationRecords', 'ApplicationsRemoved']
        };
    }
</script>
<div id="checkApplicationsDialogContainer" style="display: none; overflow: auto"
    data-bind="template: getContentPresenter()">
    <script type="text/html" id="checkResultSingleApplication">
        <div data-bind="with: ApplicationRecords[0]">
            <p data-bind="text: ViolationMessage"></p>
        </div>
    </script>
    <script type="text/html" id="checkResultMultipleApplications">
        <table class="gvuzDataGrid tableStatement2" style="width: 100%">
            <colgroup>
                <col style="width: 50%"/>
                <col style="width: 50%"/>
            </colgroup>
            <thead>
                <tr>
                    <th style="text-align: center"><%= Html.DisplayNameFor(x => x.RecordInfo.ApplicationNumber) %></th>
                    <th style="text-align: center"><%= Html.DisplayNameFor(x => x.RecordInfo.ViolationMessage) %></th>
                </tr>
            </thead>
            <tbody data-bind="foreach: ApplicationRecords">
                <tr data-bind="css: {trline1: ($index() % 2 == 0), trline2: ($index() % 2 != 0)}">
                    <td data-bind="text: ApplicationNumber"></td>
                    <td data-bind="text: ViolationMessage"></td>
                </tr>
            </tbody>
        </table>
    </script>
</div>
