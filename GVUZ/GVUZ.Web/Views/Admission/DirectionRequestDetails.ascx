<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.InstitutionDirectionRequestDetailsViewModel>" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>

<script type="text/javascript">
    function InstitutionDirectionRequestDetailsViewModel(modelData) {
        this.items = ko.observableArray(modelData.Items || []);
        this.institutionName = modelData.InstitutionName;
        this.institutionId = modelData.InstitutionId;
    }

    InstitutionDirectionRequestDetailsViewModel.prototype = {
        accept: function (item) {
            var me = this;
            var model = { InstitutionId: this.institutionId, RequestId: item.RequestId };
            doPostAjax('<%= Url.Generate<RequestHandlerController>(c => c.ApproveDirectionRequest(null)) %>', JSON.stringify(model), function () {
                me.items.remove(item);
                me._closeDialogIfEmpty();
            });
        },
        _closeDialogIfEmpty: function(){
            if (this.items().length > 0) {
                return;
            }
            $('#requestDetailsDialog').dialog('close');
        },
        _requestCommentDialog: function(cb, scope){
            var $dialog = $('#denyRequestCommentDialog');
            var $comment = $('textarea:first', $dialog);
            $comment.val('');

            if (!$dialog.hasClass('ui-content-dialog')) {
                $dialog.dialog({
                    modal: true,
                    width: 500,
                    title: 'Укажите комментарий к отклоняемой заявке',
                    buttons: {
                        'Отклонить': function () {
                            $(this).dialog('close');
                            window.setTimeout(function () {
                                cb.call(scope || window, $comment.val());
                            }, 500);
                            
                        },
                        'Закрыть': function () {
                            $(this).dialog('close')
                        }
                    }
                });
            }

            $dialog.dialog('open');
        },
        deny: function (item) {
            this._requestCommentDialog(function (comment) {
                var me = this;
                var model = { InstitutionId: this.institutionId, RequestId: item.RequestId, Comment: comment };
                doPostAjax('<%= Url.Generate<RequestHandlerController>(c => c.DenyDirectionRequest(null)) %>', JSON.stringify(model), function () {
                    me.items.remove(item);
                    me._closeDialogIfEmpty();
                });
            }, this);
            
        },
        view: function (item) {
            var $dialog = $('#viewRequestDetailsDialog');
            if ($dialog.hasClass('ui-content-dialog')) {
                $dialog.dialog('destroy');
            }
            doPostAjax('<%= Url.Generate<RequestHandlerController>(c => c.ViewDirectionRequestDialog(0, 0)) %>', JSON.stringify({ requestId: item.RequestId, institutionId: this.institutionId }), function (html) {
                $dialog.empty().html(html);
                $dialog.dialog({
                    modal: true,
                    width: 500,
                    title: 'Сведения о заявке',
                    buttons: {
                        'Закрыть': function () {
                            $(this).dialog('close');
                        }
                    }
                });
                $dialog.dialog('open');
            }, null, 'html');
        }
    };

    function showRequestDetails(institutionId, cb, scope) {
        var $dialog = $('#requestDetailsDialog');
        if (!$dialog.hasClass('ui-content-dialog')) {
            $dialog.dialog({
                modal: true,
                title: 'Список заявок',
                width: 700,
                buttons: {
                    'Закрыть': function () {
                        $(this).dialog('close');
                    }
                },
                close: function () {
                    $(this).unbind('dialogclose');
                }
            });
        }

        if (cb) {
            $dialog.bind('dialogclose', function () { cb.call(scope || window); });
        }
        

        doPostAjax('<%= Url.Generate<RequestHandlerController>(c => c.RequestDetails(0)) %>', JSON.stringify({ institutionId: institutionId }), function (data) {
            
            var model = new InstitutionDirectionRequestDetailsViewModel(data);

            var ctx = ko.contextFor($dialog[0]);
            if (ctx) {
                ctx.$rawData(model);
            }
            else {
                ko.applyBindings(ko.observable(model), $dialog[0]);
            }

            $dialog.dialog('open');
        });
    }
    
</script>

<div id="requestDetailsDialog" style="display: none">
    <p style="font-weight: bold" data-bind="text: institutionName"></p>
    <table class="gvuzDataGrid tableStatement2" cellpadding="3" style="width: 600px">
        <colgroup>
            <col style="width: 350px"  /> <%-- Направление ОО --%>
            <col style="width: 150px"/> <%-- Вид заявки --%>
            <col style="width: 100px"/> <%--  Действия--%>
        </colgroup>
        <thead>
            <tr>
                <th style="text-align: center">Направление</th>
                <th style="text-align: center">Вид заявки</th>
                <th style="text-align: center">&nbsp;</th>
            </tr>
        </thead>  
    </table>
    <div style="max-height: 300px;overflow-y: auto;overflow-x: hidden">
    <table class="gvuzDataGrid tableStatement2" cellpadding="3" style="width: 600px">
        <colgroup>
            <col style="width: 350px"  /> <%-- Направление ОО --%>
            <col style="width: 150px"/> <%-- Вид заявки --%>
            <col style="width: 100px"/> <%--  Действия--%>
        </colgroup>
        <tbody data-bind="foreach: items">
            <tr data-bind="css: {trline2: ($index() % 2 == 0), trline1: ($index() % 2 != 0)}">
                <td style="text-align: left"><a href="javascript:void(0)" title="Подробные сведения о заявке" data-bind="text: DirectionName, click: function () { $root.view.apply($root, arguments); }"></a></td>
                <td style="text-align: center"><span data-bind="text: OperationType"></span></td>
                <td style="text-align: center;white-space: nowrap">
                    <a href="javascript:void(0)" class="btnOk" title="Принять" data-bind="click: function () { $root.accept.apply($root, arguments); }">&nbsp;</a>
                    &nbsp;
                    <a href="javascript:void(0)" class="btnDelete" title="Отклонить" data-bind="click: function () { $root.deny.apply($root, arguments); }">&nbsp;</a>
                </td>
            </tr>
        </tbody>
    </table>
    </div>

    <div id="viewRequestDetailsDialog" style="display: none"></div>
    <div id="denyRequestCommentDialog" style="display: none">
        <textarea autocomplete="off" style="width: 100%;height: 100%"></textarea>
    </div>
</div>
