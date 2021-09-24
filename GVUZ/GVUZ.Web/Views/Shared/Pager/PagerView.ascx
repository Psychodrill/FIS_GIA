<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<div style="text-align: center; white-space: nowrap">
    <span class="pageLink">Всего записей: <span data-bind="text: TotalRecords"></span>, страниц: <span data-bind="text: TotalPages"></span></span>
    <a href="#" class="pageLink pageLinkArrowLeftLeft" data-bind="click: previousPage">&nbsp;</a>
    <!-- ko foreach: allPages -->
            <a href="#" class="pageLink" data-bind="css: { pageLinkActive: $data.pageNumber === ( $root.pageIndex() + 1 ) }, text: $data.pageNumber, click: function () { $root.moveToPage( $data.pageNumber - 1 ); }"></a>
    <!-- /ko -->
    <a href="#" class="pageLink pageLinkArrowRightRight" data-bind="click: nextPage">&nbsp;</a>
</div>