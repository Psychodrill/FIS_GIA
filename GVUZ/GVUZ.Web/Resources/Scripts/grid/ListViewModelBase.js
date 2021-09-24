;var DataBoundGrid = (function (__module) {

    // кастомный биндинг для вызова действий по click для viewModel.actions
    // чтобы правильно передавался scope this = инстанс viewModel
    // wrapper для click binding
    ko.bindingHandlers.modelActionsClick = {
        init: function () {
            ko.bindingHandlers.click.init.apply(this, arguments);
        }
    };

    ko.bindingHandlers.modelActionsClick.preprocess = function (value, name, addBindingCallback) {
        // если click: $root.actions.actionName, то переопределяем значение
        if (/^\$root\.actions\./.test(value)) {
            return 'function($data, $event){ ' + value + '.call($root, $data, $event);}';
        }
        // иначе поведение ничем не отличается от обычного click
        return value;
    };
    
    function ListViewModelBase() {
        this.records = ko.observableArray([]);
        this.hasRecords = ko.computed(function () {
            return this.records().length > 0;
        }, this);
        this.sort = ko.observable({ SortKey: null, SortDescending: false });
        this.selectedRecords = ko.observableArray([]);
        this.selectAllRecords = ko.pureComputed({
            read: function () {
                return this.records().length === this.selectedRecords().length;
            },
            write: function (value) {
                this.selectedRecords(value ? this.records().slice(0) : []);
            },
            owner: this
        });

        this.hasSelectedRecords = ko.pureComputed(function () {
            return this.selectedRecords().length > 0;
        }, this);
    }

    ListViewModelBase.prototype = {
        sortBy: function (key) {
            if (this.sort().SortKey != key) {
                this.sort({ SortKey: key, SortDescending: false });
            } else {
                var current = this.sort();
                this.sort({ SortKey: current.SortKey, SortDescending: !current.SortDescending });
            }
        },
        sortIconFor: function (key) {
            if (key == this.sort().SortKey) {
                return this.sort().SortDescending ? 'sortDown' : 'sortUp';
            }
            return null;
        },
        reload: function (callback) {
            this.selectAllRecords(false);
            if (callback && typeof callback === 'function') {
                callback.call(this);
            }
        },
        forceReload: function () {
            if (this.pager.CurrentPage() !== 1) {
                this.pager.CurrentPage(1);
            } else {
                this.reload();
            }
        },
        setupControls: function (domContainer) {
            var $container = $(domContainer);

            $("a[data-sort]", $container).each(function (index, item) {
                var $item = $(item);
                var key = $item.attr('data-sort');
                $item.attr('data-bind', "click: function(){ $data.sortBy('" + key + "');}").removeAttr('data-sort').attr('href', 'javascript:void(0)');
                $('<span />').attr('data-bind', "css: sortIconFor('" + key + "')").insertAfter($item);
            });
        },
        init: function (domElement, callback) {
            this.setupControls(domElement);
            ko.applyBindings(this, domElement);
            this.reload(function () {
                if (this.filter) {
                    this.filter.appliedValues.subscribe(this.forceReload, this);
                }
                this.pager.CurrentPage.subscribe(this.reload, this);
                this.pager.PageSize.subscribe(this.forceReload, this);
                this.sort.subscribe(this.forceReload, this);
                this.initialized = true;
                if (callback && typeof callback === 'function') {
                    callback.call(this);
                }
            });
        }
    };

    WebUtils.extend(__module, { ListViewModelBase: ListViewModelBase });
    return __module;

})(DataBoundGrid || {});