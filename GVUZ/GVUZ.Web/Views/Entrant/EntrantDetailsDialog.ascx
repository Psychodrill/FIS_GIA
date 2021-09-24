<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.Entrants.EntrantDetailsViewModel>" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

<script type="text/javascript">
    function showEntrantDetails(entrantId) {

        function EntrantDetailsViewModel(modelData) {
            ko.mapping.fromJS(modelData, this.readMapping, this);
            this.message = ko.observable(null);
        }

        EntrantDetailsViewModel.prototype.readMapping = {
            'observe': 'UID'
        };

        EntrantDetailsViewModel.prototype.actions = {
            viewApplication: function (record) {
                viewApplicationDetails(record.ApplicationId, record.ApplicationNumber, record.IsCampaignFinished, record.StatusID);
                return false;
            },
            editApplication: function (record) {
                return record.AllowEdit ? editApplicationUrl(record.ApplicationId) : '#';
            },
            navigate: function (record) {
                if (record.AllowEdit != true) {
                    return '#';
                }
                var url = '<%= Url.Action("NavigateToList", "InstitutionApplication", new { applicationId = "APPID"}) %>';
                return url.replace("APPID", record.ApplicationId.toString());
            },
            updateUid: function () {
                var me = this;
                var submitModel = { entrantId: me.EntrantId, UID: me.UID() };
                me.message(null);
                doPostAjax('<%= Url.Action("UpdateUID", "Entrant") %>', ko.toJSON(submitModel), function (res) {
                    if (res.success !== true && res.message) {
                        me.message(res.message);
                    }
                }, null, null, true);
            }
        };
        var $container = $('#entrantDetailsDialog');
        var existingContext = ko.contextFor($container[0]);
        
        function initDialog() {
            $container.dialog({
                modal: true,
                width: 800,
                height: 400,
                buttons: [
                    {
                        text: 'Закрыть',
                        click: function() {
                            $(this).dialog('close');
                        }
                    }
                ]
            });
        }

        var model = { id: entrantId };
        
        doPostAjax('<%= Url.Action("EntrantDetails", "Entrant") %>', ko.toJSON(model), function (data) {

            var viewModel = new EntrantDetailsViewModel(data);
            
            if (existingContext && ko.isObservable(existingContext.$rawData)) {
                existingContext.$rawData(viewModel);
            } else {
                initDialog();
                ko.applyBindings(ko.observable(viewModel), $container[0]);
            }

            $container.dialog('option', 'title', viewModel.EntrantName).dialog('open');

        }, null, null, true);
    }    

</script>

<div id="entrantDetailsDialog" style="display: none;overflow: auto">
    <table style="width: 100%" cellpadding="2">
        <colgroup>
            <col style="width: 10%"/>
            <col style="width: 20%"/>
            <col style="width: 30%"/>
            <col style="width: 40%"/>
        </colgroup>
        <tr>
            <td style="white-space: nowrap"><%= Html.DisplayNameFor(x => x.DateOfBirth) %>:</td>
            <td style="white-space: nowrap"><span data-bind="text: DateOfBirth"></span></td>
            <td colspan="2" style="white-space: nowrap;font-weight: bold;text-align: center">Документ, удостоверяющий личность</td>
        </tr>
        <tr>
            <td style="white-space: nowrap"><%= Html.DisplayNameFor(x => x.Gender) %>:</td>
            <td style="white-space: nowrap"><span data-bind="text: Gender"></span></td>
            <td style="text-align: right"><%= Html.DisplayNameFor(x => x.IdentityDocumentType) %>:</td>
            <td style="white-space: nowrap"><span data-bind="text: IdentityDocumentType"></span></td>
        </tr>
        <tr>
            <td style="white-space: nowrap"><%= Html.DisplayNameFor(x => x.PlaceOfBirth) %>:</td>
            <td style="white-space: nowrap"><span data-bind="text: PlaceOfBirth"></span></td>
            <td style="text-align: right"><%= Html.DisplayNameFor(x => x.IdentityDocumentNumber) %>:</td>
            <td style="white-space: nowrap"><span data-bind="text: IdentityDocumentNumber"></span></td>
        </tr>
        <tr>
            <td colspan="4" style="text-align: center;white-space: nowrap">
                <span style="color: salmon" data-bind="text: message, visible: message"></span>
                <br>
                <%= Html.DisplayNameFor(x => x.UID) %>:&nbsp;
                <input type="text" data-bind="value: UID">&nbsp;
                <a href="javascript:void(0)" class="btnSave" data-bind="click: actions.updateUid">&nbsp;</a>
            </td>
        </tr>
    </table>
    <table class="gvuzDataGrid tableStatement2">
        <colgroup>
            <col style="width: 10%"/> <%-- № заявления--%>
            <col style="width: 10%"/> <%-- статус--%>
            <col style="width: 30%"/><%-- приемная кампания--%>
            <col style="width: 25%"/> <%-- конкурс --%>
            <col style="width: 5%"/> <%-- дата регистрации --%>
            <col style="width: 15%"/> <%-- льгота --%>
            <col style="width: 5%"/> <%-- действия --%> 
        </colgroup>
        <thead>
            <tr>
                <th><%= Html.DisplayNameFor(x => x.RecordInfo.ApplicationNumber) %></th>
                <th><%= Html.DisplayNameFor(x => x.RecordInfo.StatusName) %></th>
                <th><%= Html.DisplayNameFor(x => x.RecordInfo.Campaign) %></th>
                <th><%= Html.DisplayNameFor(x => x.RecordInfo.CompetitiveGroup) %></th>
                <th><%= Html.DisplayNameFor(x => x.RecordInfo.RegistrationDate) %></th>
                <th><%= Html.DisplayNameFor(x => x.RecordInfo.Benefit) %></th>
                <th>Действия</th>
            </tr>
        </thead>
        <tbody data-bind="foreach: ApplicationList">
            <tr data-bind="css: {trline1: $index()%2 == 0, trline2: $index()%2 != 0}">
                <td>
                    <a href="javascript:void(0)" data-bind="visible: AllowEdit, text: ApplicationNumber, modelActionsClick: $root.actions.viewApplication"></a>
                    <span data-bind="text: ApplicationNumber, visible: !AllowEdit"></span>
                </td>
                <td><span data-bind="text: StatusName"></span></td>
                <td><span data-bind="text: Campaign"></span></td>
                <td><span data-bind="text: CompetitiveGroup"></span></td>
                <td><span data-bind="text: RegistrationDate"></span></td>
                <td style="text-align: center"><span data-bind="text: Benefit ? Benefit : '&mdash;'"></span></td>
                <td>
                    <a title="Редактировать" data-bind="attr: {href: $root.actions.editApplication($data)}, css: {disabled: !AllowEdit, btnEdit: AllowEdit, btnEditGray: !AllowEdit}">&nbsp;</a>
                    <a title="К заявлению" data-bind="attr: { href: $root.actions.navigate($data) }, css: { disabled: !AllowEdit, btnGoStat: AllowEdit, btnGoStatGray: !AllowEdit }">&nbsp;</a>
                </td>
            </tr>
        </tbody>
    </table>
</div>
