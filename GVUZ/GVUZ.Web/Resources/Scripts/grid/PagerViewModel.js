;var DataBoundGrid = (function (__module) {

    /* Управление пейджингом - JS-модель для ViewModels/Shared/PagerViewModel */
    function PagerViewModel( data )
    {
        this.update(data);

        this.isVisible = ko.pureComputed(this.getIsVisible, this);
            
        this.firstVisiblePage = ko.pureComputed(this.getFirstVisiblePage, this);

        this.lastVisiblePage = ko.pureComputed(this.getLastVisiblePage, this);

        this.visiblePages = ko.pureComputed(this.getVisiblePages, this);

        this.ellipsesLeftVisible = ko.pureComputed(this.getEllipsesLeftVisible, this);

        this.ellipsesLeftPage = ko.pureComputed(this.getEllipsesLeftPage, this);

        this.ellipsesRightVisible = ko.pureComputed(this.getEllipsesRightVisible, this);

        this.ellipsesRightPage = ko.pureComputed(this.getEllipsesRightPage, this);
    }

    PagerViewModel.prototype = {
        scrollWindow: 5,
        pageSizes: [10, 30, 50, 100, 200, 500],
        mapping: { 'include': ['PageSize', 'CurrentPage', 'TotalRecords', 'TotalPages'] },
        update: function (data) {
            ko.mapping.fromJS(data, this.mapping, this);
        },
        isCurrentPage: function (page) {
            return page == this.CurrentPage();
        },
        scrollLeft: function () {
            if (this.ellipsesLeftVisible() && this.ellipsesLeftPage() < this.firstVisiblePage()) {
                this.CurrentPage(this.ellipsesLeftPage());
            }
        },
        scrollRight: function () {
            if (this.ellipsesRightVisible() && this.ellipsesRightPage() > this.lastVisiblePage()) {
                this.CurrentPage(this.ellipsesRightPage());
            }
        },
        scrollToLast: function () {
            this.CurrentPage(this.TotalPages());
        },
        scrollToFirst: function () {
            this.CurrentPage(1);
        },
        getIsVisible: function() {
            return this.TotalPages() > 1;
        },
        getFirstVisiblePage: function () {
            return Math.max(1, this.CurrentPage() - Math.floor(this.scrollWindow / 2));
        },
        getLastVisiblePage: function () {
            return Math.min(this.TotalPages(), this.firstVisiblePage() + this.scrollWindow - 1);
        },
        getVisiblePages: function () {
            var start = this.firstVisiblePage();
            var end = this.lastVisiblePage();
            var pages = [];
            for (var p = start; p <= end; p++) {
                pages.push(p);
            }

            return pages;
        },
        getEllipsesLeftVisible: function () {
            return this.firstVisiblePage() > 1;
        },
        getEllipsesLeftPage: function () {
            return this.firstVisiblePage() - 1;
        },
        getEllipsesRightVisible: function () {
            return this.lastVisiblePage() < this.TotalPages();
        },
        getEllipsesRightPage: function () {
            return this.lastVisiblePage() + 1;
        }
    };

    WebUtils.extend(__module, { PagerViewModel: PagerViewModel });
    return __module;

})(DataBoundGrid || {});