<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.OrderOfAdmission.ApplicationSelectOrderOfAdmissionRefuseViewModel>" %>

<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.ViewModels.OrderOfAdmission" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<asp:Content ID="Content3" ContentPlaceHolderID="PageTitle" runat="server">
    Включение заявления в приказ об отказе от зачисления
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageHeaderContent" runat="server">
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/libs/knockout-3.3.0.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/libs/knockout.mapping-latest.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/WebUtils.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/grid/FilterViewModelBase.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/grid/PagerViewModel.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/grid/ListViewModelBase.js") %>"></script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Выберите приказ
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <div id="applicationSelectOrderListContainer" style="display: none" class="divstatement notabs">
        <div style="padding: 8px; padding-left: 0">
            <% using (Html.BeginForm("CreateOrderOfAdmissionRefuse", "OrderOfAdmission", FormMethod.Post, new { id = "createOrderForm" }))
               {%>
                   <input type="hidden" name="OrderOfAdmissionId" value="<%=Model.OrderOfAdmissionId %>" />
                  <% for (int i = 0; i < Model.ApplicationItemIds.Length; i++)
                   { %>
            <input type="hidden" data-application-item-ids="" name="<%= string.Format("applicationItemIds[{0}]", i) %>" value="<%= Model.ApplicationItemIds[i] %>" />
            <%} %>
            <input type="button" value="Добавить приказ" class="button3" data-bind="click: actions.addOrder" />
            <% } %>
        </div>
        <div style="padding: 8px; padding-left: 0">
            <p data-bind="visible: !hasRecords()" style="color: red">
                Для условия приема в заявлении отсутствуют соответствующие приказы о зачислении
            </p>
        </div>
        <!-- ko if: hasRecords() -->
        <table class="gvuzDataGrid tableStatement2" cellpadding="3">
            <colgroup>
                <col style="width: 2%" />
                <%--  ИД --%>
                <col style="width: 20%" />
                <%-- Наименование --%>
                <col style="width: 5%" />
                <%-- Уровень образования --%>
                <col style="width: 1%" />
                <%-- Курс --%>
                <col style="width: 10%" />
                <%-- Форма обучения --%>
                <col style="width: 5%" />
                <%-- Источник финансирования --%>
                <col style="width: 1%" />
                <%-- Этап --%>
                <col style="width: 1%" />
                <%-- Льготный приказ --%>
                <col style="width: 1%" />
                <%-- Прием по направлениям Минобрнауки --%>
            </colgroup>
            <thead>
                <tr>
                    <%--  ИД --%>
                    <th style="text-align: center"></th>
                    <%--  Наименование --%>
                    <th style="text-align: center">
                        <a data-sort="OrderName">
                            <%= Html.LabelTextFor(x => x.RecordInfo.OrderName) %></a>
                    </th>
                    <%--  Уровень образования --%>
                    <th style="text-align: center">
                        <a data-sort="EducationLevel">
                            <%= Html.LabelTextFor(x => x.RecordInfo.EducationLevel) %></a>
                    </th>
                    <%--  Форма обучения --%>
                    <th style="text-align: center">
                        <a data-sort="EducationForm">
                            <%= Html.LabelTextFor(x => x.RecordInfo.EducationForm) %></a>
                    </th>
                    <%--  Источник финансирования --%>
                    <th style="text-align: center">
                        <a data-sort="EducationSource">
                            <%= Html.LabelTextFor(x => x.RecordInfo.EducationSource) %></a>
                    </th>
                    <%--  Этап --%>
                    <th style="text-align: center">
                        <a data-sort="Stage">
                            <%= Html.LabelTextFor(x => x.RecordInfo.Stage) %></a>
                    </th>
                    <%--  Льготный приказ --%>
                    <th style="text-align: center">
                        <a data-sort="IsForBeneficiary">
                            <%= Html.LabelTextFor(x => x.RecordInfo.IsForBeneficiary) %></a>
                    </th>
                    <%--  Прием по направлениям Минобрнауки --%>
                    <th style="text-align: center">
                        <a data-sort="IsForeigner">
                            <%= Html.LabelTextFor(x => x.RecordInfo.IsForeigner) %></a>
                    </th>
                </tr>
            </thead>
            <tbody data-bind="foreach: records">
                <tr data-bind="css: {trline2: ($index() % 2 == 0), trline1: ($index() % 2 != 0)}">
                    <td>
                        <input type="radio" name="group1" data-bind="text: OrderId" />
                    </td>
                    <td>
                        <span data-bind="text: OrderName"></span>
                    </td>
                    <td style="text-align: center">
                        <span data-bind="text: EducationLevel"></span>
                    </td>
                    <td style="text-align: center">
                        <span data-bind="text: EducationForm"></span>
                    </td>
                    <td style="text-align: center">
                        <span data-bind="text: EducationSource"></span>
                    </td>
                    <td style="text-align: center">
                        <span data-bind="text: Stage"></span>
                    </td>
                    <td style="text-align: center">
                        <span data-bind="text: IsForBeneficiary ? 'да' : 'нет'"></span>
                    </td>
                    <td style="text-align: center">
                        <span data-bind="text: IsForeigner ? 'да' : 'нет'"></span>
                    </td>
                </tr>
            </tbody>
            <tfoot data-bind="with: pager">
                <tr>
                    <th colspan="14">
                        <% Html.RenderPartial("InstitutionApplication/PagerView"); %>
                    </th>
                </tr>
            </tfoot>
        </table>
        <div style="padding: 8px; padding-left: 0">
            <input type="button" value="Выбрать" class="button3" data-bind="click: actions.selectOrder, orderId" />
            <input type="button" value="Отмена" class="button3" data-bind="click: actions.cancelOrder" />
            <!-- /ko -->
        </div>
        <!-- /ko -->
    </div>
    <%--<% Html.RenderPartial("OrderOfAdmission/Dialogs/EditOrder", OrderOfAdmissionEditViewModel.MetadataInstance); %>--%>
    <script type="text/javascript">
        (function ($) {
            $(function () {
                applicationSelectOrderListLoaded();
            });
        })(jQuery);

        function applicationSelectOrderListLoaded() {

            function ApplicationSelectOrderFilterViewModel(data, defaults) {
                this.defaultValuesMap = { 'ignore': [ 'Stages', 'EducationLevels', 'EducationForms', 'EducationSources', 'IsForBeneficiaryList', 'IsForeignerList'] };
                this.defaultValues = ko.mapping.toJS(defaults, this.defaultValuesMap);
                DataBoundGrid.FilterViewModelBase.call(this, data);
            }

            ApplicationSelectOrderFilterViewModel.prototype = DataBoundGrid.FilterViewModelBase.prototype;

            function ApplicationSelectOrderListViewModel() {
                DataBoundGrid.ListViewModelBase.call(this);
                this.TotalOrdersCount = ko.observable(0);
                this.filterStats = ko.pureComputed(this.getFilterStats, this);
            }

            ApplicationSelectOrderListViewModel.prototype = WebUtils.extend({}, DataBoundGrid.ListViewModelBase.prototype);

            ApplicationSelectOrderListViewModel.prototype.getFilterStats = function() {
                var filteredTotal = this.pager.TotalRecords();
                var unfilteredTotal = this.TotalOrdersCount();
                if (unfilteredTotal == filteredTotal) {
                    return unfilteredTotal.toString();
                } else {
                    return filteredTotal.toString() + ' из ' + unfilteredTotal.toString();
                }
            };

            ApplicationSelectOrderListViewModel.prototype.init = function (domElement, modelData, callback) {
                this.filter = new ApplicationSelectOrderFilterViewModel(modelData.Filter, modelData.DefaultFilter);
                this.pager = new DataBoundGrid.PagerViewModel(modelData.Pager);
                this.sort({ SortKey: 'OrderNumber', SortDescending: false });
                DataBoundGrid.ListViewModelBase.prototype.init.call(this, domElement, function () {                            
                    //this.filter.SelectedStage.subscribe(this.forceReload, this);
                    if (callback && typeof callback === 'function') {
                        callback.call(this);
                    }
                });
            };

            ApplicationSelectOrderListViewModel.prototype.reload = function(callback) {
                var me = this;


                var queryModel = {
                    ApplicationID: <%= Model.ApplicationId %>,
                    Filter: this.filter.appliedValues() || this.filter.defaultValues,
                    Pager: ko.mapping.toJS(this.pager, this.pager.mapping),
                    Sort: ko.mapping.toJS(this.sort)
                };
                queryModel.Filter.SelectedStage = ko.utils.unwrapObservable(this.filter.SelectedStage);
                doPostAjax('<%= Url.Generate<OrderOfAdmissionController>(x => x.LoadApplicationSelectRefuseOrderRecords(null)) %>', JSON.stringify({ model: queryModel }), function(result) { 
                    me.records(result.Records);
                    me.pager.update(result.Pager);
                    DataBoundGrid.ListViewModelBase.prototype.reload.call(me, callback);
                    me.TotalOrdersCount(result.TotalOrdersCount);
                }, null, null, true);
            };            

            ApplicationSelectOrderListViewModel.prototype.actions = {
                addOrder: function() {
                    $('#createOrderForm').submit();
                    return false;
                },
                selectOrder: function() {
                    if ($('input:radio[name=group1]:checked').text() != "") 
                    {
                        var orderOfAdmissionRefuseId = $('input:radio[name=group1]:checked').text();
                        var orderOfAdmissionId = $('input[name=OrderOfAdmissionId]').val();
                        var applicationItemIds =[];
                        $('input[data-application-item-ids]').each(function()
                        {
                            applicationItemIds.push($(this).val());
                        });

                        doPostAjax('<%= Url.Generate<OrderOfAdmissionController>(x => x.IncludeToOrderOfAdmissionRefuse(null, null)) %>', JSON.stringify({ applicationItemIds:applicationItemIds, orderOfAdmissionRefuseId: orderOfAdmissionRefuseId }), function(result) { 
                            if(!result.IsError)
                            {
                                infoDialogC("Заявление(я) включены в приказ об отказе от зачисления",{title:"Результат операции",closeCallback:function()
                                {
                                    ReturnToOrderOfAdmission();
                                },
                                closeDialogCallback: function()
                                {
                                    ReturnToOrderOfAdmission();
                                }});
                            }
                            else 
                            {
                                var errorMessage = result.Data;
                                infoDialogC("Ошибка включения заявлений в приказ об отказе от зачисления: "+errorMessage, { title: "Ошибка" });
                            }
                        }); 
                    } 
                    else 
                    {
                        infoDialog('Выбирите приказ!');
                    }
        },
        cancelOrder: function() {
            <% if (Model.OrderOfAdmissionId.HasValue)
               {%>
                    ReturnToOrderOfAdmission();
            <% }
               else
               { %>
                    window.location = '<%= Url.Generate<OrderOfAdmissionController>(x => x.OrdersOfAdmissionList())%>';
            <% } %>
            return false;
        }
        };
        var viewModelData = <%= Html.CustomJson(new {Model.Filter, Model.Pager, DefaultFilter = ApplicationSelectOrderFilterViewModel.Default}) %>;
        var $container = $('#applicationSelectOrderListContainer');
        $(".datePicker", $container).datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-40:+0' });
            var model = new ApplicationSelectOrderListViewModel();       
            model.init($container[0], viewModelData, function() { $container.show(); });
        }

        function ReturnToOrderOfAdmission()
        {        
            window.location = '<%= Url.Generate<OrderOfAdmissionController>(x => x.EditOrder(Model.OrderOfAdmissionId))%>';
        }
    </script>
</asp:Content>
