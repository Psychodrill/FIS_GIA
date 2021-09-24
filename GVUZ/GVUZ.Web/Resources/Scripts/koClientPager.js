function PagerModel(records) {
    var me = this;
    me.pageSizeOptions = ko.observableArray([10, 30, 50, 100, 200, 500]);

    me.records = GetObservableArray(records);
    me.currentPageIndex = ko.observable(me.records().length > 0 ? 0 : -1);
    me.currentPageSize = ko.observable(25);
    me.recordCount = ko.computed(function () {
        return me.records().length;
    });
    me.maxPageIndex = ko.computed(function () {
        return Math.ceil(me.records().length / me.currentPageSize()) - 1;
    });
    me.visiblePages = ko.computed(function () {
        var range = 5;
        var halfRange = Math.floor(range/2)
        var result = [];

        var visiblePageIndexes;
        if (me.maxPageIndex() < range)
            visiblePageIndexes = { start: 0, end: me.maxPageIndex() }
        else if (me.currentPageIndex() <= halfRange)
            visiblePageIndexes = { start: 0, end: range - 1 }
        else if (me.currentPageIndex() >= me.maxPageIndex() - halfRange)
            visiblePageIndexes = { start: me.maxPageIndex() - range, end: me.maxPageIndex() }
        else
            visiblePageIndexes = { start: me.currentPageIndex() - halfRange, end: me.currentPageIndex() + halfRange }

        for (var i = visiblePageIndexes.start; i <= visiblePageIndexes.end; i++)
        {
            if (i >= 0) result.push(i);
        }
        return result;
    });


    me.currentPageRecords = ko.computed(function () {
        var newPageIndex = -1;
        var pageIndex = me.currentPageIndex();
        var maxPageIndex = me.maxPageIndex();
        if (pageIndex > maxPageIndex) {
            newPageIndex = maxPageIndex;
        }
        else if (pageIndex == -1) {
            if (maxPageIndex > -1) {
                newPageIndex = 0;
            }
            else {
                newPageIndex = -2;
            }
        }
        else {
            newPageIndex = pageIndex;
        }

        if (newPageIndex != pageIndex) {
            if (newPageIndex >= -1) {
                me.currentPageIndex(newPageIndex);
            }

            return [];
        }

        var pageSize = me.currentPageSize();
        var startIndex = pageIndex * pageSize;
        var endIndex = startIndex + pageSize;
        return me.records().slice(startIndex, endIndex);
    }).extend({ throttle: 5 });

    me.moveFirst = function () {
        me.changePageIndex(0);
    };
    me.movePrevious = function () {
        me.changePageIndex(me.currentPageIndex() - 1);
    };
    me.moveNext = function () {
        me.changePageIndex(me.currentPageIndex() + 1);
    };
    me.moveLast = function () {
        me.changePageIndex(me.maxPageIndex());
    };
    me.changePageIndex = function (newIndex) {
        if (newIndex < 0
            || newIndex == me.currentPageIndex()
            || newIndex > me.maxPageIndex()) {
            return;
        }
        me.currentPageIndex(newIndex);
    };
    me.onPageSizeChange = function () {
        me.currentPageIndex(0);
    };
    me.renderPagers = function () {
        //var pager = "<div><a href=\"#\" data-bind=\"click: pager.moveFirst, enable: pager.currentPageIndex() > 0\">&lt;&lt;</a><a href=\"#\" data-bind=\"click: pager.movePrevious, enable: pager.currentPageIndex() > 0\">&lt;</a>Page <span data-bind=\"text: pager.currentPageIndex() + 1\"></span> of <span data-bind=\"text: pager.maxPageIndex() + 1\"></span> [<span data-bind=\"text: pager.recordCount\"></span> Record(s)]<select data-bind=\"options: pager.pageSizeOptions, value: pager.currentPageSize, event: { change: pager.onPageSizeChange }\"></select><a href=\"#\" data-bind=\"click: pager.moveNext, enable: pager.currentPageIndex() < pager.maxPageIndex()\">&gt;</a><a href=\"#\" data-bind=\"click: pager.moveLast, enable: pager.currentPageIndex() < pager.maxPageIndex()\">&gt;&gt;</a></div>";
        //$("div.Pager").html( $("#ClientPager").html() ); // div#ClientPager лежит в ~\Web\Resources\ClientPagerView.html
    };
    me.renderNoRecords = function () {
        var message = "<span data-bind=\"visible: pager.recordCount() == 0\">Нет данных</span>";
        $("div.NoRecords").html(message);
    };
    me.renderPagers();
    me.renderNoRecords();
}

function SorterModel(sortOptions, records) {
    var me = this;
    me.records = GetObservableArray(records);
    me.sortOptions = ko.observableArray(sortOptions);
    me.sortDirections = ko.observableArray([
        {
            Name: "Asc",
            Value: "Asc",
            Sort: false
        },
        {
            Name: "Desc",
            Value: "Desc",
            Sort: true
        }]);
    me.currentSortOption = ko.observable(me.sortOptions()[0]);
    me.sortAscending = ko.observable(true); //true - asc, false - desc
    me.getSortOptionByName = function (optionName) {
        var arrxd = me.sortOptions();
        return _.find(me.sortOptions(), function (elem) { return elem.Name == optionName });
    }
    me.sortIconFor = function (sortOptionName) {
        if (sortOptionName == me.currentSortOption().Name) {
            return me.sortAscending() ? 'sortUpAlignedRight' : 'sortDownAlignedRight' ;
        }
        return null;
    }
    me.setCurrentSort = function (newSortOption) {
        if (typeof (newSortOption) == "undefined") {
            console.log("sort option not found: " + newSortOption);
            return;
        }
        if (newSortOption.Name != me.currentSortOption().Name) {
            me.currentSortOption(newSortOption);
            me.sortAscending(true);
        }
        else {
            me.sortAscending(!me.sortAscending());
        }
    }

    me.orderedRecords = ko.computed(function () {
        var records = me.records();
        var sortOption = me.currentSortOption();
        if (sortOption == null) {
            return records;
        }

        var sortedRecords = records.slice(0, records.length);
        SortArray(sortedRecords, me.sortAscending(), sortOption.Sort);
        return sortedRecords;
    }).extend({ throttle: 5 });
}

function FilterModel(filters, records, filterCookieName) {
    var me = this;
    if (filterCookieName) {
        me.filterCookieName = filterCookieName;
    }
    me.UpdateFilterCookie = function () {
        //чтобы работало куки для фильтра, 
        //при инициализации FilterModel нужно передать 3й параметр (filterCookieName)
        if (!me.filterCookieName || me.filterCookieName === "") {
            try{
                if (window.console && window.console.log) {
                    console.log("filter cookie name not set");
                }
            } catch (e) { }
            return;
        }
        var filterCookie = {};
        _.each(me.activeFilters(), function (activeFilter) {
            var filter = activeFilter.Filter;
            if (filter.CurrentOption) {
                filterCookie[filter.Name] = filter.CurrentOption().FilterValue;
            }
            else if (filter.Value) {
                filterCookie[filter.Name] = filter.Value();
            }
        });
        $.cookie(me.filterCookieName, JSON.stringify(filterCookie));
    }

    me.FillFromCookie = function () {
        //чтобы работало куки для фильтра, 
        //при инициализации FilterModel нужно передать 3й параметр (filterCookieName)
        if (!me.filterCookieName || me.filterCookieName === "") {
            try {
                if (window.console && window.console.log) {
                    console.log("filter cookie name not set");
                }
            } catch (e) { }
            return;
        }
        var cookieValue = $.cookie(me.filterCookieName);
        if (!cookieValue){
            return false;
        }
        var cookieFilterModel = JSON.parse(cookieValue);
        //в куке лежит объект такого вида: 
        //{ 
        //    'filter1.Name' : 'filter1.Value', (для текстового фильтра)
        //    'filter2.Name' : 'filter2.CurrentOption().FilterValue', (для дропдауна)
        //    ......
        //}

        _.each(cookieFilterModel, function (filterValue, filterName) {
            var filter = me.getFilterByName(filterName);
            if (filter.CurrentOption) {
                filter.CurrentOption(_.find(filter.Options, function (option) { return option.FilterValue == filterValue; }));
            }
            else if (filter.Value) {
                filter.Value(filterValue);
            }
        })



        for (var filterName in cookieFilterModel) {
            var filter = me.getFilterByName(filterName);
            var filterValue = cookieFilterModel[filterName];
            if (filter.CurrentOption) {
                filter.CurrentOption(_.find(filter.Options, function (option) { return option.FilterValue == filterValue; }));
            }
            else if (filter.Value) {
                filter.Value(filterValue);
            }
        }
        return true;
    }

    me.records = GetObservableArray(records);
    _.each(filters, function (filterItem) {
        //добавляем [не важно] для всех дропдаунов
        if (filterItem.Options && !_.find(filterItem.Options, function(option){ return option.FilterValue == null; })) {
            filterItem.Options.unshift(GetOption('[Не важно]', '[Не важно]', null));
            filterItem.CurrentOption.extend({ rateLimit: { timeout: 300, method: "notifyWhenChangesStop" } });
        }
        else if (filterItem.Value) {
            filterItem.Value.extend({ rateLimit: { timeout: 300, method: "notifyWhenChangesStop" } });
        }
    });
    me.filters = ko.observableArray(filters);
    me.getFilterByName = function (filterName) {
        return _.find(me.filters(), function (filterItem) { return filterItem.Name == filterName });
    }
    me.activeFilters = ko.computed(function () {
        var filters = me.filters();
        var activeFilters = [];
        for (var index = 0; index < filters.length; index++) {
            var filter = filters[index];
            if (filter.CurrentOption) {
                var filterOption = filter.CurrentOption();
                if (filterOption && filterOption.FilterValue != null) {
                    var activeFilter = {
                        Filter: filter,
                        IsFiltered: function (filter, record) {
                            var filterOption = filter.CurrentOption();
                            if (!filterOption) {
                                return;
                            }

                            var recordValue = filter.RecordValue(record);
                            return recordValue != filterOption.FilterValue;
                        }
                    };
                    activeFilters.push(activeFilter);
                }
            }
            else if (filter.Value) {
                var filterValue = filter.Value();
                if (filterValue && filterValue != "") {
                    var activeFilter = {
                        Filter: filter,
                        IsFiltered: function (filter, record) {
                            var filterValue = filter.Value();
                            filterValue = filterValue.toUpperCase();

                            var recordValue = filter.RecordValue(record);
                            if (recordValue == null || recordValue == "") {
                                return true;
                            }
                            recordValue = recordValue.toUpperCase();
                            return recordValue.indexOf(filterValue) == -1;
                        }
                    };
                    activeFilters.push(activeFilter);
                }
            }
        }
        return activeFilters;
    }).extend({ rateLimit: { timeout: 500, method: "notifyWhenChangesStop" } });

    me.filteredRecords = ko.computed(function () {
        var records = me.records();
        var filters = me.activeFilters();
        if (filters.length == 0) {
            return records;
        }

        var filteredRecords = [];
        for (var rIndex = 0; rIndex < records.length; rIndex++) {
            var isIncluded = true;
            var record = records[rIndex];
            for (var fIndex = 0; fIndex < filters.length; fIndex++) {
                var filter = filters[fIndex];
                var isFiltered = filter.IsFiltered(filter.Filter, record);
                if (isFiltered) {
                    isIncluded = false;
                    break;
                }
            }

            if (isIncluded) {
                filteredRecords.push(record);
            }
        }

        return filteredRecords;
    }).extend({ throttle: 200 });

    me.ResetFilters = function () {
        _.each(me.filters(), function (filter) {
            if (filter.CurrentOption) {
                filter.CurrentOption(_.find(filter.Options, function (option) { return option.FilterValue == null; }));
            }
            else if (filter.Value) {
                filter.Value("");
            }
        });
    }

    me.activeFilters.subscribe(me.UpdateFilterCookie);
    me.FillFromCookie();
}

function ExtractModels(parent, data, constructor) {
    var models = [];
    if (data == null) {
        return models;
    }
    for (var index = 0; index < data.length; index++) {
        var row = data[index];
        var model = new constructor(row, parent);
        models.push(model);
    }
    return models;
}

function GetObservableArray(array) {
    if (typeof (array) == 'function') {
        return array;
    }

    return ko.observableArray(array);
}

function CompareCaseInsensitive(left, right) {
    if (left == null) {
        return right == null;
    }
    else if (right == null) {
        return false;
    }

    return left.toUpperCase() <= right.toUpperCase();
}

function GetOption(name, value, filterValue) {
    var option = {
        Name: name,
        Value: value,
        FilterValue: filterValue
    };
    return option;
}

function SortArray(array, direction, comparison) {
    if (array == null) {
        return [];
    }
    for (var oIndex = 0; oIndex < array.length; oIndex++) {
        var oItem = array[oIndex];
        for (var iIndex = oIndex + 1; iIndex < array.length; iIndex++) {
            var iItem = array[iIndex];
            var isOrdered = comparison(oItem, iItem);
            if (isOrdered != direction) {
                array[iIndex] = oItem;
                array[oIndex] = iItem;
                oItem = iItem;
            }
        }
    }
    return array;
}
