<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<div id="ClientPager" data-bind="with: pager" style="text-align: center; white-space: nowrap">

    <span class="pageLink">Всего записей: <span data-bind="text: recordCount"></span>&emsp;Страниц: <span data-bind="text: maxPageIndex() + 1"></span></span>
    <a href="#" class="pageLink" data-bind="click: moveFirst, enable: currentPageIndex() > 0">&lt;&lt;</a>
    <a href="#" class="pageLink" data-bind="click: movePrevious, enable: currentPageIndex() > 0">&nbsp;&lt;</a>

    <!-- ko foreach: visiblePages -->
    <a href="#" class="pageLink" data-bind="click: $parent.changePageIndex, text: $data + 1, css: {pageLinkActive: $parent.currentPageIndex() == $data}, style: {fontWeight: $parent.currentPageIndex() == $data ? 'bold' : null}"></a>
    <!-- /ko -->

    <a href="#" class="pageLink" data-bind="click: moveNext, enable: currentPageIndex() < maxPageIndex()">&nbsp;&gt;</a>
    <a href="#" class="pageLink" data-bind="click: moveLast, enable: currentPageIndex() < maxPageIndex()">&gt;&gt;</a>

    <span class="pageLink" style="display: inline;">
        На странице:&nbsp;
        <select style="width: 50px" data-bind="options: pageSizeOptions, value: currentPageSize, event: { change: onPageSizeChange }"></select>
    </span>
</div>