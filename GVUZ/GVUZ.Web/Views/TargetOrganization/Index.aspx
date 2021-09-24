<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Register TagPrefix="gv" TagName="TabControl" Src="~/Views/Shared/Common/InstitutionsTabControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Целевые организации
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageHeaderContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PageTitle" runat="server">
    Сведения об образовательной организации
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PageSubtitle" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">
    <div class="divstatement">
        <gv:TabControl runat="server" ID="tabControl" />

        <script type="text/javascript">
            menuItems[5].selected = true;

            var actionUrl = {
                loadRecords: '<%= Url.Action("LoadRecords") %>',
                submit: '<%= Url.Action("UpdateRecord") %>',
                remove: '<%= Url.Action("DeleteRecord") %>'
            };
            function loadRecords() {
                return $.parseJSON($.ajax({
                    url: actionUrl.loadRecords,
                    type: "POST",
                    async: false
                }).response);
            }

            var dataModel = { targetOrganizations: loadRecords() }
            function TargetOrganizationModel(data) {
                if (!data) {
                    data = {};                   
                }
 
                
                
                var me = this;
                me.CompetitiveGroupTargetID = ko.observable(data.CompetitiveGroupTargetID);
                me.UID = ko.observable(data.UID);
                me.ContractOrganizationName = ko.observable(data.ContractOrganizationName);
                me.HaveContract = ko.observable(data.HaveContract);
                me.ContractNumber = ko.observable(data.ContractNumber);
                me.ContractDate = ko.observable(data.ContractDate);
                me.ContractOrganizationOGRN = ko.observable(data.ContractOrganizationOGRN);
                me.ContractOrganizationKPP = ko.observable(data.ContractOrganizationKPP);
                me.EmployerOrganizationName = ko.observable(data.EmployerOrganizationName);
                me.EmployerOrganizationOGRN = ko.observable(data.EmployerOrganizationOGRN);
                me.EmployerOrganizationKPP = ko.observable(data.EmployerOrganizationKPP);
                me.LocationEmployerOrganizations = ko.observable(data.LocationEmployerOrganizations);              
                me.CanRemove = ko.observable(data.CanRemove);
            }
            function TargetOrganizationEditModel(record, list) {
                var me = this;
                me.CompetitiveGroupTargetID = ko.observable(record.CompetitiveGroupTargetID());
                me.UID = ko.observable(record.UID());
                me.ContractOrganizationName = ko.observable(record.ContractOrganizationName());
                me.HaveContract = true;/*ko.observable(record.HaveContract());*/
                me.ContractNumber = ko.observable(record.ContractNumber());
                me.ContractDate = ko.observable(record.ContractDate());
                me.ContractOrganizationOGRN = ko.observable(record.ContractOrganizationOGRN());
                me.ContractOrganizationKPP = ko.observable(record.ContractOrganizationKPP());
                me.EmployerOrganizationName = ko.observable(record.EmployerOrganizationName());
                me.EmployerOrganizationOGRN = ko.observable(record.EmployerOrganizationOGRN());
                me.EmployerOrganizationKPP = ko.observable(record.EmployerOrganizationKPP());
                me.LocationEmployerOrganizations = ko.observable(record.LocationEmployerOrganizations())
                me.CanRemove = ko.observable(record.CanRemove());
                me.errors = {
                    UID: ko.observable(null), ContractOrganizationName: ko.observable(null), ContractOrganizationKPP: ko.observable(null), EmployerOrganizationOGRN: ko.observable(null),
                    EmployerOrganizationName: ko.observable(null), ContractNumber: ko.observable(null), ContractDate: ko.observable(null), ContractOrganizationOGRN: ko.observable(null),
                    EmployerOrganizationKPP: ko.observable(null), LocationEmployerOrganizations: ko.observable(null)
                };
                me.allerrors = ko.computed(function () {
                    var message = [];
                    for (var p in me.errors) {
                        var errorText = me.errors[p]();
                        if (errorText) {
                            message.push(errorText);
                        }
                    }

                    return message.length > 0 ? message.join('<br />') : null;
                });

                me.commit = function () {

                    var model = createSubmitDataModel();

                    if (validate(model)) {

                        doPostAjax(actionUrl.submit, JSON.stringify(model), function (result) {
                            if (result.success) {
                                ko.mapping.fromJS(result.record, {}, record);
                                list.selectedRecord(null);
                            } else {
                                if (result.errors) {
                                    me.errors.UID(result.errors.UID || null);
                                    me.errors.ContractOrganizationName(result.errors.ContractOrganizationName || null);

                                    me.errors.ContractNumber(result.errors.ContractNumber || null);
                                    me.errors.ContractDate(result.errors.ContractDate || null);
                                    me.errors.ContractOrganizationOGRN(result.errors.ContractOrganizationOGRN || null);
                                    me.errors.ContractOrganizationKPP(result.errors.ContractOrganizationsKPP || null);
                                    me.errors.EmployerOrganizationName(result.errors.EmployerOrganizationName || null);
                                    me.errors.EmployerOrganizationOGRN(result.errors.EmployerOrganizationOGRN || null);
                                    me.errors.EmployerOrganizationKPP(result.errors.EmployerOrganizationKPP || null);
                                    me.errors.LocationEmployerOrganizations(result.errors.LocationEmployerOrganizations || null);
                                    
                                }
                            }
                        }, null, null, true);
                    }
                };

                me.rollback = function () {
                    list.selectedRecord(null);
                    if (record.CompetitiveGroupTargetID() == 0) {
                        list.targetOrganizations.remove(record);
                    }
                };

                function resetErrors() {
                    for (var p in me.errors) {
                        me.errors[p](null);
                    }
                }

                function createSubmitDataModel() {
                    return {
                        CompetitiveGroupTargetID: me.CompetitiveGroupTargetID(),
                        UID : me.UID(),
                        ContractOrganizationName: me.ContractOrganizationName(),
                        HaveContract: me.HaveContract,
                        ContractNumber: me.ContractNumber(),
                        ContractDate: me.ContractDate(),
                        ContractOrganizationOGRN: me.ContractOrganizationOGRN(),
                        ContractOrganizationKPP: me.ContractOrganizationKPP(),
                        EmployerOrganizationName: me.EmployerOrganizationName(),
                        EmployerOrganizationOGRN: me.EmployerOrganizationOGRN(),
                        EmployerOrganizationKPP: me.EmployerOrganizationKPP(),
                        LocationEmployerOrganizations: me.LocationEmployerOrganizations(),
                        CanRemove: me.CanRemove()
                    };
                }
             

                function hasErrors() {
                    for (var x in me.errors) {
                        if (me.errors[x]())
                            return true;
                    }

                    return false;
                }

                function validate(submitModel) {
                    resetErrors();
                    if (!submitModel.ContractOrganizationName || submitModel.ContractOrganizationName.length == 0 || /^\s+$/.test(submitModel.ContractOrganizationName)) {
                        me.errors.ContractOrganizationName('Поле "Наименование" должно быть заполнено');
                    }
                    return !hasErrors();
                }


            }
            function TargetOrganizationPageModel(data) {
                if (!data) {
                    data = {};
                }
                var me = this;
                me.selectedRecord = ko.observable();
                me.customMessage = ko.observable();
                me.targetOrganizations = ko.observableArray(ExtractModels(me, data.targetOrganizations, TargetOrganizationModel));

                //
                me.selectedRecord.subscribe(function () {
                    me.customMessage(null);
                });

                me.messages = ko.computed(function () {
                    return me.selectedRecord() ? me.selectedRecord().allerrors() : null;
                });

                me.showEditControls = ko.computed(function () {
                    return !me.selectedRecord();
                });

                me.selectPresenter = function (record) {
                    return me.selectedRecord() && me.selectedRecord().CompetitiveGroupTargetID() == record.CompetitiveGroupTargetID() ? 'editTemplate' : 'viewTemplate';
                };

                me.beginEdit = function (record) {
                    var model = new TargetOrganizationEditModel(record, me);
                    me.selectedRecord(model);                   
                    
                };

                me.removeRecord = function (record) {
                    if (!record.CanRemove()) return;
                    confirmDialog('Вы уверены, что хотите удалить целевую организацию?', function () {
                        doPostAjax(actionUrl.remove, JSON.stringify({ CompetitiveGroupTargetID: record.CompetitiveGroupTargetID() }), function (result) {
                            if (result.success) {
                                me.targetOrganizations.remove(record);
                            } else {
                                if (result.errors && result.errors.length > 0) {
                                    me.customMessage(result.errors.join('<br />'));
                                }
                            }
                        }, null, null, true);
                    });
                };

                me.addRecord = function () {
                    var rec = {
                        CompetitiveGroupTargetID: ko.observable(),
                        UID: ko.observable(),                      
                        ContractOrganizationName: ko.observable(),
                        HaveContract: ko.observable(),
                        ContractNumber: ko.observable(),
                        ContractDate: ko.observable(),
                        ContractOrganizationOGRN: ko.observable(),
                        ContractOrganizationKPP: ko.observable(),
                        EmployerOrganizationName: ko.observable(),
                        EmployerOrganizationOGRN: ko.observable(),
                        EmployerOrganizationKPP: ko.observable(),
                        LocationEmployerOrganizations: ko.observable(),             
                        CanRemove: ko.observable()
                    };

                    me.targetOrganizations.push(rec);
                    me.beginEdit(rec);


                };
                //


                var filters = [{
                    Type: "text",
                    Name: "UID",
                    Value: ko.observable(""),
                    RecordValue: function (record) { return record.UID(); }
                }, {
                    Type: "text",
                    Name: "Наименование организации, с которой заключен договор",
                    Value: ko.observable(""),
                    RecordValue: function (record) { return record.ContractOrganizationName(); }
                },
                {
                    Type: "text",
                    Name: "Наименование организации работодателя",
                    Value: ko.observable(""),
                    RecordValue: function (record) { return record.EmployerOrganizationName(); }
                }
                ];//TODO: необходимо сделать интерактивную сортировку по каждому? из столбцов грида
                var sortOptions = [{

                    // добавлено по заявке 24970 
                    Name: "ContractDate",
                    Value: "ContractDate",
                    Sort: function (left, right) { return left.ContractDate() < right.ContractDate(); }
                },
                {
                    Name: "UID",
                    Value: "UID",
                    Sort: function (left, right) { return left.UID() < right.UID(); }
                },

                {
                    Name: "ContractOrganizationName",
                    Value: "ContractOrganizationName",
                    Sort: function (left, right) { return left.ContractOrganizationName() < right.ContractOrganizationName(); }
                },
                {
                    Name: "EmployerOrganizationName",
                    Value: "EmployerOrganizationName",
                    Sort: function (left, right) { return left.EmployerOrganizationName() < right.EmployerOrganizationName(); }
                }];
                var filterCookieName = "flt_TargetOrganizations";
                me.filter = new FilterModel(filters, me.targetOrganizations, filterCookieName);
                me.sorter = new SorterModel(sortOptions, me.filter.filteredRecords);
                me.pager = new PagerModel(me.sorter.orderedRecords);
            }

            function checkContract()
            {
                var checkBox = document.getElementById('checkContract');

                var textboxes = document.getElementsByClassName('textbox');
                for (var i = 0; i < textboxes.length; i++)
                {
                    textboxes[i].disabled = !checkBox.checked;
                    //if (status == "open") {
                    //    // grab the data
                    //}
                }
                
            }

            //function PagerModel(records) {
            //    var me = this;
            //    me.pageSizeOptions = ko.observableArray([1, 5, 25, 50, 100, 250, 500]);
            //    me.records = GetObservableArray(records);
            //    me.currentPageIndex = ko.observable(me.records().length > 0 ? 0 : -1);
            //    me.currentPageSize = ko.observable(25);
            //    me.recordCount = ko.computed(function () {
            //        return me.records().length;
            //    });
            //    me.maxPageIndex = ko.computed(function () {
            //        return Math.ceil(me.records().length / me.currentPageSize()) - 1;
            //    });
            //    me.currentPageRecords = ko.computed(function () {
            //        var newPageIndex = -1;
            //        var pageIndex = me.currentPageIndex();
            //        var maxPageIndex = me.maxPageIndex();
            //        if (pageIndex > maxPageIndex) {
            //            newPageIndex = maxPageIndex;
            //        }
            //        else if (pageIndex == -1) {
            //            if (maxPageIndex > -1) {
            //                newPageIndex = 0;
            //            }
            //            else {
            //                newPageIndex = -2;
            //            }
            //        }
            //        else {
            //            newPageIndex = pageIndex;
            //        }
            //        if (newPageIndex != pageIndex) {
            //            if (newPageIndex >= -1) {
            //                me.currentPageIndex(newPageIndex);
            //            }
            //            return [];
            //        }
            //        var pageSize = me.currentPageSize();
            //        var startIndex = pageIndex * pageSize;
            //        var endIndex = startIndex + pageSize;
            //        return me.records().slice(startIndex, endIndex);
            //    }).extend({ throttle: 5 });
            //    me.moveFirst = function () {
            //        me.changePageIndex(0);
            //    };
            //    me.movePrevious = function () {
            //        me.changePageIndex(me.currentPageIndex() - 1);
            //    };
            //    me.moveNext = function () {
            //        me.changePageIndex(me.currentPageIndex() + 1);
            //    };
            //    me.moveLast = function () {
            //        me.changePageIndex(me.maxPageIndex());
            //    };
            //    me.changePageIndex = function (newIndex) {
            //        if (newIndex < 0
            //            || newIndex == me.currentPageIndex()
            //            || newIndex > me.maxPageIndex()) {
            //            return;
            //        }
            //        me.currentPageIndex(newIndex);
            //    };
            //    me.onPageSizeChange = function () {
            //        me.currentPageIndex(0);
            //    };
            //    me.renderPagers = function () {
            //        var pager = "<div><a href=\"#\" data-bind=\"click: pager.moveFirst, enable: pager.currentPageIndex() > 0\">&lt;&lt;</a><a href=\"#\" data-bind=\"click: pager.movePrevious, enable: pager.currentPageIndex() > 0\">&lt;</a>Page <span data-bind=\"text: pager.currentPageIndex() + 1\"></span> of <span data-bind=\"text: pager.maxPageIndex() + 1\"></span> [<span data-bind=\"text: pager.recordCount\"></span> Record(s)]<select data-bind=\"options: pager.pageSizeOptions, value: pager.currentPageSize, event: { change: pager.onPageSizeChange }\"></select><a href=\"#\" data-bind=\"click: pager.moveNext, enable: pager.currentPageIndex() < pager.maxPageIndex()\">&gt;</a><a href=\"#\" data-bind=\"click: pager.moveLast, enable: pager.currentPageIndex() < pager.maxPageIndex()\">&gt;&gt;</a></div>";
            //        $("div.Pager").html(pager);
            //    };
            //    me.renderNoRecords = function () {
            //        var message = "<span data-bind=\"visible: pager.recordCount() == 0\">No records found.</span>";
            //        $("div.NoRecords").html(message);
            //    };
            //    me.renderPagers();
            //    me.renderNoRecords();
            //}
            //function SorterModel(sortOptions, records) {
            //    var me = this;
            //    me.records = GetObservableArray(records);
            //    me.sortOptions = ko.observableArray(sortOptions);
            //    me.sortDirections = ko.observableArray([
            //        {
            //            Name: "Asc",
            //            Value: "Asc",
            //            Sort: false
            //        },
            //        {
            //            Name: "Desc",
            //            Value: "Desc",
            //            Sort: true
            //        }]);
            //    me.currentSortOption = ko.observable(me.sortOptions()[0]);
            //    me.currentSortDirection = ko.observable(me.sortDirections()[0]);
            //    me.orderedRecords = ko.computed(function () {
            //        var records = me.records();
            //        var sortOption = me.currentSortOption();
            //        var sortDirection = me.currentSortDirection();
            //        if (sortOption == null || sortDirection == null) {
            //            return records;
            //        }
            //        var sortedRecords = records.slice(0, records.length);
            //        SortArray(sortedRecords, sortDirection.Sort, sortOption.Sort);
            //        return sortedRecords;
            //    }).extend({ throttle: 5 });
            //}
            //function FilterModel(filters, records) {
            //    var me = this;
            //    me.records = GetObservableArray(records);
            //    me.filters = ko.observableArray(filters);
            //    me.activeFilters = ko.computed(function () {
            //        var filters = me.filters();
            //        var activeFilters = [];
            //        for (var index = 0; index < filters.length; index++) {
            //            var filter = filters[index];
            //            if (filter.CurrentOption) {
            //                var filterOption = filter.CurrentOption();
            //                if (filterOption && filterOption.FilterValue != null) {
            //                    var activeFilter = {
            //                        Filter: filter,
            //                        IsFiltered: function (filter, record) {
            //                            var filterOption = filter.CurrentOption();
            //                            if (!filterOption) {
            //                                return;
            //                            }
            //                            var recordValue = filter.RecordValue(record);
            //                            return recordValue != filterOption.FilterValue; NoMat
            //                        }
            //                    };
            //                    activeFilters.push(activeFilter);
            //                }
            //            }
            //            else if (filter.Value) {
            //                var filterValue = filter.Value();
            //                if (filterValue && filterValue != "") {
            //                    var activeFilter = {
            //                        Filter: filter,
            //                        IsFiltered: function (filter, record) {
            //                            var filterValue = filter.Value();
            //                            filterValue = filterValue.toUpperCase();
            //                            var recordValue = filter.RecordValue(record);
            //                            recordValue = recordValue.toUpperCase();
            //                            return recordValue.indexOf(filterValue) == -1;
            //                        }
            //                    };
            //                    activeFilters.push(activeFilter);
            //                }
            //            }
            //        }
            //        return activeFilters;
            //    });
            //    me.filteredRecords = ko.computed(function () {
            //        var records = me.records();
            //        var filters = me.activeFilters();
            //        if (filters.length == 0) {
            //            return records;
            //        }
            //        var filteredRecords = [];
            //        for (var rIndex = 0; rIndex < records.length; rIndex++) {
            //            var isIncluded = true;
            //            var record = records[rIndex];
            //            for (var fIndex = 0; fIndex < filters.length; fIndex++) {
            //                var filter = filters[fIndex];
            //                var isFiltered = filter.IsFiltered(filter.Filter, record);
            //                if (isFiltered) {
            //                    isIncluded = false;
            //                    break;
            //                }
            //            }
            //            if (isIncluded) {
            //                filteredRecords.push(record);
            //            }
            //        }
            //        return filteredRecords;
            //    }).extend({ throttle: 200 });
            //}
            //function ExtractModels(parent, data, constructor) {
            //    var models = [];
            //    if (data == null) {
            //        return models;
            //    }
            //    for (var index = 0; index < data.length; index++) {
            //        var row = data[index];
            //        var model = new constructor(row, parent);
            //        models.push(model);
            //    }
            //    return models;
            //}
            //function GetObservableArray(array) {
            //    if (typeof (array) == 'function') {
            //        return array;
            //    }
            //    return ko.observableArray(array);
            //}
            //function CompareCaseInsensitive(left, right) {
            //    if (left == null) {
            //        return right == null;
            //    }
            //    else if (right == null) {
            //        return false;
            //    }
            //    return left.toUpperCase() <= right.toUpperCase();
            //}
            //function GetOption(name, value, filterValue) {
            //    var option = {
            //        Name: name,
            //        Value: value,
            //        FilterValue: filterValue
            //    };
            //    return option;
            //}
            //function SortArray(array, direction, comparison) {
            //    if (array == null) {
            //        return [];
            //    }
            //    for (var oIndex = 0; oIndex < array.length; oIndex++) {
            //        var oItem = array[oIndex];
            //        for (var iIndex = oIndex + 1; iIndex < array.length; iIndex++) {
            //            var iItem = array[iIndex];
            //            var isOrdered = comparison(oItem, iItem);
            //            if (isOrdered == direction) {
            //                array[iIndex] = oItem;
            //                array[oIndex] = iItem;
            //                oItem = iItem;
            //            }
            //        }
            //    }
            //    return array;
            //}

            $(function () {
                ko.applyBindings(new TargetOrganizationPageModel(dataModel));
            });

            function toggleFilter() {
                if (jQuery('#btnShowFilter').hasClass('filterDisplayed')) {
                    jQuery('#btnShowFilter').removeClass('filterDisplayed')
                    jQuery('#btnShowFilter').html('Отобразить фильтр')
                    jQuery('#btnShowFilter').parent().removeClass('nonHideTable')
                    jQuery('#btnShowFilter').parent().parent().parent().addClass('tableHeaderCollapsed')
                    jQuery('#divFilter').hide()
                }
                else {
                    jQuery('#btnShowFilter').addClass('filterDisplayed')
                    jQuery('#btnShowFilter').html('Скрыть фильтр')
                    jQuery('#btnShowFilter').parent().addClass('nonHideTable')
                    jQuery('#btnShowFilter').parent().parent().parent().removeClass('tableHeaderCollapsed')
                    jQuery('#divFilter').show()
                }
            };

          
            
        </script>

        <span style="display: block; margin: 15px 10px;">
            <input type="button" class="button3" data-bind="click: addRecord, visible: !selectedRecord()" value="Добавить" />
            <input type="button" class="button3" disabled data-bind="visible: selectedRecord()" value="Добавить" />
        </span>
        
        <div data-bind="visible: customMessage, html: customMessage" style="color: #b22222"></div>
        <div data-bind="visible: messages, html: messages" style="color: #b22222"></div>


        <%--<div data-bind="foreach: filter.filters">
            <div>
                <span data-bind="text: Name"></span>:<br />
            </div>
            <div data-bind="if: Type == 'select'">
                <select data-bind="options: Options, optionsText: 'Name', value: CurrentOption"></select>
            </div>
            <div data-bind="if: Type == 'text'">
                <input type="text" data-bind="value: Value, valueUpdate: 'afterkeydown'" />
            </div>
        </div>--%>

        <div class="tableHeader2l tableHeaderCollapsed" style="height: 100%">
            <div id="divFilterPlace">
                <div class="hideTable" onclick="toggleFilter()" style="float: left"><span id="btnShowFilter">Отобразить фильтр</span> </div>
                <%--<div id="spAppCount" class="appCount">Количество конкурсов: <% = Model.TotalFilteredCount %> <%= Model.TotalFilteredCount < Model.TotalItemCount ? "из " + Model.TotalItemCount : "" %></div>--%>
            </div>
            <div id="divFilter" style="display: none; clear: both;">
                <div class="nameTable" style="display: none">Фильтр по списку целевых организаций</div>

                <table class="tableForm" data-bind="with: filter">
                    <colgroup>
                        <col style="width: 10%;"/>
                        <col style="width: 20%"/>
                        <col style="width: 10%"/>
                        <col style="width: 25%"/>
                        <col style="width: 10%"/>
                        <col style="width: 20%"/>
                    </colgroup>
                    <tbody>
                        <tr>
                            <!-- ko with: getFilterByName('UID') -->
                            <td style="text-align: right;">
                                <span data-bind="text: Name"></span>:&nbsp;
                            </td>
                            <td>
                                <input type="text" data-bind="value: Value, valueUpdate: 'afterkeydown'" />
                            </td>
                            <!-- /ko -->

                            <!-- ko with: getFilterByName('Наименование') -->
                            <td style="text-align: right;">
                                <span data-bind="text: EmployerOrganizationName"></span>:&nbsp;
                            </td>
                            <td>
                                <input type="text" data-bind="value: Value, valueUpdate: 'afterkeydown'" />
                            </td>
                            <!-- /ko -->
                        </tr>
                        <tr>
                            <td colspan="6" align="center">
                                <input type="button" class="button" value="Сбросить фильтр" style="width: auto" data-bind="click: ResetFilters" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <table class="gvuzDataGrid">
            <thead>
                <tr>
                    <th style="width: 10%">UID</th>
                    <th style="width: 15%">Наименование организации, с которой заключен договор</th>
                    <th style="width: 15%">Наличие договора о целевом обучении</th>
                    <th style="width: 15%">Номер договора</th>
                    <th style="width: 15%">Дата заключения договора</th>
                    <th style="width: 15%">ОГРН организации, с которой заключен договор</th>
                    <th style="width: 30%">КПП организации, с которой заключен договор</th>
                    <th style="width: 15%">Наименование организации работодателя</th>
                    <th style="width: 15%">ОГРН организации работодателя</th>
                    <th style="width: 15%">КПП организации работодателя</th>
                    <th style="width: 10%">Субъект РФ, в котором расположена организация работодателя</th>
                    <th style="text-align: center; width: 10%">Действия</th>
                </tr>
            </thead>
            <tbody data-bind="template: { name: selectPresenter, foreach: pager.currentPageRecords }" >
                <tr>
                    <td><span data-bind="text: UID"></span></td>
                    <td><span data-bind="text: ContractOrganizationName"></span></td>
                    <td><input type="checkbox" data-bind="checked: HaveContract" disabled/>  </td>
                    <td><span data-bind="text: ContractNumber"></span></td>
                    <td><span data-bind="text: ContractDate"></span></td>
                    <td><span data-bind="text: ContractOrganizationOGRN"></span></td>
                    <td><span data-bind="text: ContractOrganizationKPP"></span></td>
                    <td><span data-bind="text: EmployerOrganizationName"></span></td>
                    <td><span data-bind="text: EmployerOrganizationOGRN"></span></td>
                    <td><span data-bind="text: EmployerOrganizationKPP"></span></td>
                    <td><span data-bind="text: LocationEmployerOrganizations"></span></td>
                    
                    <td style="white-space: nowrap">
                        <a class="btnEdit" href="javascript:void(0)" data-bind="click: $root.beginEdit, visible: $root.showEditControls"></a>
                        <a class="btnDelete" href="javascript:void(0)" data-bind="click: $root.removeRecord, visible: $root.showEditControls, css: { disabled: !CanRemove(), btnDeleteGray: !CanRemove() }"></a>
                    </td>
                </tr>

            </tbody>
            <tfoot>
                <tr>
                    <th colspan="12">
                        <% Html.RenderPartial("Pager/ClientPagerView"); %>
                    </th>
                </tr>
            </tfoot>
        </table>
    </div>

    <script type="text/html" id="viewTemplate">
        <tr>
            <td><span data-bind="text: UID"></span></td>
            <td><span data-bind="text: ContractOrganizationName"></span></td>
            <td><input type="checkbox" data-bind="checked: HaveContract" disabled/> </td>
            <td><span data-bind="text: ContractNumber"></span></td>
            <td><span data-bind="text: ContractDate"></span></td>
            <td><span data-bind="text: ContractOrganizationOGRN"></span></td>
            <td><span data-bind="text: ContractOrganizationKPP"></span></td>
            <td><span data-bind="text: EmployerOrganizationName"></span></td>
            <td><span data-bind="text: EmployerOrganizationOGRN"></span></td>
            <td><span data-bind="text: EmployerOrganizationKPP"></span></td>
            <td><span data-bind="text: LocationEmployerOrganizations"></span></td>        
            <td style="text-align: center; white-space: nowrap">
                <a class="btnEdit" href="javascript:void(0)" data-bind="click: $root.beginEdit, visible: $root.showEditControls"></a>
                <a class="btnDelete" href="javascript:void(0)" data-bind="click: $root.removeRecord, visible: $root.showEditControls, css: { disabled: !CanRemove(), btnDeleteGray: !CanRemove() }"></a>
            </td>
        </tr>
    </script>

    <script type="text/html" id="editTemplate">
        <!-- ko with: $root.selectedRecord -->
        <tr>
            <td>
                <input type="text" data-bind="value: UID, css: { 'input-validation-error': errors.UID }, attr: { title: errors.UID }" /></td>
            <td>
                <input type="text" data-bind="value: ContractOrganizationName, css: { 'input-validation-error': errors.ContractOrganizationName }, attr: { title: errors.ContractOrganizationName }" /></td>
            <td>
                <input type="checkbox" onclick = "checkContract()" id= "checkContract" data-bind="checked: HaveContract, css: { 'input-validation-error': errors.HaveContract }, attr: { title: errors.HaveContract }" /></td>
            <td>
                <input type="text" class = "textbox" data-bind="value: ContractNumber, css: { 'input-validation-error': errors.ContractNumber }, attr: { title: errors.ContractNumber }" /></td>
            <td>                   
                <input type="text" class = "textbox" data-bind="value: ContractDate, css: { 'input-validation-error': errors.ContractDate }, attr: { title: errors.ContractDate }" /></td>            
            <td>                    
                <input type="text" class = "textbox" data-bind="value: ContractOrganizationOGRN, css: { 'input-validation-error': errors.ContractOrganizationOGRN }, attr: { title: errors.ContractOrganizationOGRN }" /></td>
            <td>                    
                <input type="text" class = "textbox" data-bind="value: ContractOrganizationKPP, css: { 'input-validation-error': errors.ContractOrganizationKPP }, attr: { title: errors.ContractOrganizationKPP }" /></td>
            <td>                    
                <input type="text" class = "textbox" data-bind="value: EmployerOrganizationName, css: { 'input-validation-error': errors.EmployerOrganizationName }, attr: { title: errors.EmployerOrganizationName }" /></td>
            <td>                    
                <input type="text" class = "textbox" data-bind="value: EmployerOrganizationOGRN, css: { 'input-validation-error': errors.EmployerOrganizationOGRN }, attr: { title: errors.EmployerOrganizationOGRN }" /></td>
            <td>                    
                <input type="text" class = "textbox" data-bind="value: EmployerOrganizationKPP, css: { 'input-validation-error': errors.EmployerOrganizationKPP }, attr: { title: errors.EmployerOrganizationKPP }" /></td>
            <td>                    
                <input type="text" class = "textbox" data-bind="value: LocationEmployerOrganizations, css: { 'input-validation-error': errors.LocationEmployerOrganizations }, attr: { title: errors.LocationEmployerOrganizations }" /></td>
            <td style="white-space: nowrap">
                <a class="btnSave" href="javascript:void(0)" data-bind="click: commit"></a>
                <a class="btnDelete" href="javascript:void(0)" data-bind="click: rollback"></a>
            </td>
        </tr>
        <!-- /ko -->
    </script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/jquery.cookie.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/koClientPager.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/underscore-min.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/libs/knockout-3.3.0.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/libs/knockout.mapping-latest.js") %>"></script>
</asp:Content>
