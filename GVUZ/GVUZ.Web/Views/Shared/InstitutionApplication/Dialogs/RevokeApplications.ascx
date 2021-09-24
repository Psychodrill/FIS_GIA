<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.ApplicationsList.RevokeApplicationsListViewModel>" %>
<%@ Import Namespace="System.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

<script type="text/javascript">

    function revokeApplicationsDialog(recordId, callback, callbackScope) {

        function RevokeApplicationsViewModel(modelData) {
            ko.mapping.fromJS(modelData, this.readMapping, this);
        }

        RevokeApplicationsViewModel.prototype = {
            readMapping: {
                'copy': 'ApplicationRecords[0]',
                'observe': 'ApplicationRecords[0].Reason'
            },
            getSubmitModel: function () {
                return {
                    'ApplicationRecords':
                    ko.utils.arrayMap(this.ApplicationRecords, function (record) {
                        return {
                            'ApplicationId': record.ApplicationId,
                            'Reason': ko.utils.unwrapObservable(record.Reason),
                            'ReturnDocumentsTypeId': ko.utils.unwrapObservable(record.ReturnDocumentsTypeId),
                            'ReturnDocumentsDate': ko.utils.unwrapObservable(record.ReturnDocumentsDate)
                        };
                    }),
                };
            },
            accept: function (onSuccess) {
                var submitModel = this.getSubmitModel();
                //console.log(submitModel);
                doPostAjax('<%= Url.Action("RevokeApplications", "InstitutionApplication") %>', ko.toJSON(submitModel), function (result) {
                    if (result.success && onSuccess && typeof onSuccess === "function") {
                        onSuccess.call(this);
                    }
                }, null, null, true);
            },
            getContentPresenter: function () {
                return this.ApplicationRecords.length == 1 ? 'revokeSingleApplicationTemplate' : 'revokeMultipleApplicationsTemplate';
            }
        };
        
        var $container = $('#revokeApplicationsDialog');
        var existingContext = ko.contextFor($container[0]);
        var isSingle = 1;
        function dialogSubmitted() {
            $container.dialog('close');
            //console.log('about to invoke callback ', callback, ' in scope ', callbackScope);
            if (callback != null && typeof callback === "function") {
                callback.call(callbackScope || window);
            }
        }

        function initDialog(length) {
            $container.dialog({
                modal: true,
                title: 'Отзыв заявлений',
                width: length > 1 ? 1500 : 1000,
                height: 500,
                buttons: [
                    {
                        text: 'Отозвать',
                        click: function () {
                            var error = false;
                            $('.dropdown').each(function () {
                                if ($(this).val() == 0) {
                                    $(this).addClass('input-validation-error');
                                    //console.log('Ошибка валидации!');
                                    error = true;
                                }
                                else {
                                    $(this).addClass('input-validation-error-fixed').removeClass('input-validation-error');
                                }
                            });

                            $('.datepicker').each(function () {
                                if ($(this).val() == null || $(this).val() == '') {
                                    $(this).addClass('input-validation-error');
                                    //console.log('Ошибка валидации!');
                                    error = true;
                                }
                                else {
                                    $(this).addClass('input-validation-error-fixed').removeClass('input-validation-error');
                                }
                            });

                            if (error) return;

                            var context = ko.contextFor($container[0]);
                            RevokeApplicationsViewModel.prototype.accept.call(context.$rawData(), dialogSubmitted);
                        }
                    },
                    {
                        text: 'Отмена',
                        click: function () {
                            $(this).dialog('close');
                        }
                    }
                ]
            });
        }

        function initData(viewModel) {
            $('.datepicker').each(function () {
                $(this).datepicker();
            });
            $('.dropdown').each(function () {
                var option = '';
                for (var i = 0; i < viewModel.ReturnDocumentsTypes.length; i++) {
                    option += '<option value="' + viewModel.ReturnDocumentsTypes[i].ID + '">' + viewModel.ReturnDocumentsTypes[i].Name + '</option>';
                }
                $(this).append(option);
            });
        }

        var model = { applicationId: $.isArray(recordId) ? recordId : [recordId] };

        doPostAjax('<%= Url.Action("GetRevokableApplications", "InstitutionApplication") %>', ko.toJSON(model), function (data) {
            var viewModel = new RevokeApplicationsViewModel(data);
            //console.log(viewModel);
            if (existingContext && ko.isObservable(existingContext.$rawData)) {
                initDialog(viewModel.ApplicationRecords.length);
                existingContext.$rawData(viewModel);
                initData(viewModel);
            } else {
                initDialog(viewModel.ApplicationRecords.length);
                ko.applyBindings(ko.observable(viewModel), $container[0]);
                initData(viewModel);
            }

            $container.dialog('option', 700, viewModel.ApplicationRecords.length > 1 ? 500 : 400).dialog({
                open: function () {},
            });
        }, null, null, true);
    }    

</script>

<div id="revokeApplicationsDialog" style="display: none;" data-bind="template: getContentPresenter()">
    
    <script type="text/html" id="revokeSingleApplicationTemplate">
        <table class="gvuzDataGrid tableStatement2" style="width: 100%" data-bind="with: ApplicationRecords[0]">
            <colgroup>
                <col style="width: 50%"/>
                <col style="width: 50%;text-align: right" />
            </colgroup>
            <tbody>
                <tr>
                    <td><%= Html.DisplayNameFor(x => x.RecordInfo.ApplicationNumber) %>:</td>
                    <td><span data-bind="text: ApplicationNumber"></span></td>
                </tr>
                <tr>
                    <td><%= Html.DisplayNameFor(x => x.RecordInfo.EntrantName) %>:</td>
                    <td><span data-bind="text: EntrantName"></span></td>
                </tr>
                <tr>
                    <td><%= Html.DisplayNameFor(x => x.RecordInfo.IdentityDocument)%>:</td>
                    <td><span data-bind="text: IdentityDocument"></span></td>
                </tr>
            </tbody>
            <tfoot>
                <tr>
                    <td colspan="2"><%= Html.DisplayNameFor(x => x.RecordInfo.Reason) %></td>
                </tr>
                <tr>
                <td colspan="2">
                    <textarea autocomplete="off" style="width: 100%;resize: none" rows="4" maxlength="1000" data-bind="value: Reason"></textarea>
                </td>
                </tr>
                <tr>
                    <td>
                        <%= Html.DisplayNameFor(x => x.RecordInfo.ReturnDocumentsTypeId) %>
                        <span class="required">*</span>
                    </td>
                    <td>
                        <select data-bind="value: ReturnDocumentsTypeId" style="width: 300px" class="dropdown">
                            <option selected value="0">Не выбрано</option>
                         </select>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%= Html.DisplayNameFor(x => x.RecordInfo.ReturnDocumentsDate) %>
                        <span class="required">*</span>
                    </td>
                    <td>
                        <input data-bind="value: ReturnDocumentsDate" style="width: 100px" class="datepicker" maxlength="10" tabindex="21" type="text"><img class="ui-datepicker-trigger gvuz-calendar-icon" src="/Resources/Images/calendar.jpg" alt="..." title="">
                        <br>
                        <div id="txtbDateError"></div>
                    </td>
                </tr>
            </tfoot>
        </table>
    </script>

    <script  type="text/html" id="revokeMultipleApplicationsTemplate">
        <table class="gvuzDataGrid tableStatement2" style="width: 100%">
            <colgroup>
                <col style="width: 10%"/>
                <col style="width: 10%" />
                <col style="width: 7%" />
                <col style="width: 35%" />
                <col style="width: 25%" />
                <col style="width: 13%" />
            </colgroup>
            <thead>
                <tr>
                    <th style="text-align: center"><%= Html.DisplayNameFor(x => x.RecordInfo.ApplicationNumber) %></th>
                    <th style="text-align: center"><%= Html.DisplayNameFor(x => x.RecordInfo.EntrantName) %></th>
                    <th style="text-align: center"><%= Html.DisplayNameFor(x => x.RecordInfo.IdentityDocument) %></th>
                    <th style="text-align: center"><%= Html.DisplayNameFor(x => x.RecordInfo.Reason) %></th>
                    <th style="text-align: center"><%= Html.DisplayNameFor(x => x.RecordInfo.ReturnDocumentsTypeId) %><span class="required">*</span></th>
                    <th style="text-align: center"><%= Html.DisplayNameFor(x => x.RecordInfo.ReturnDocumentsDate) %><span class="required">*</span></th>
                </tr>
            </thead>
            <tbody data-bind="foreach: ApplicationRecords">
                <tr data-bind="css: {trline1: ($index() % 2 == 0), trline2: ($index() % 2 != 0)}">
                    <td data-bind="text: ApplicationNumber"></td>
                    <td data-bind="text: EntrantName"></td>
                    <td data-bind="text: IdentityDocument"></td>
                    <td><input type="text" data-bind="value: Reason" /></td>
                    <td>
                        <select data-bind="value: ReturnDocumentsTypeId" class="dropdown">
                            <option selected value="0">Не выбрано</option>
                         </select>
                    </td>
                    <td>
                        <input data-bind="value: ReturnDocumentsDate" class="datepicker" maxlength="10" tabindex="21" type="text"><img class="ui-datepicker-trigger gvuz-calendar-icon" src="/Resources/Images/calendar.jpg" alt="..." title="">
                        <br>
                        <div id="txtbDateError"></div>
                    </td>

                </tr>
            </tbody>
        </table>
    </script>
</div>
