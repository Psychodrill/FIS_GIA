<%--Отрисовка пейджера для data-bound JavaScript-модели PagerViewModel (knockoutjs), связана с C#-моделью ViewModels/Shared/PagerViewModel.cs --%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%--
   Контекст binding'a - свойство pager в javascript-модели UncheckedApplicationsListViewModel
--%>

<div style="text-align: center;white-space: nowrap">
    <span class="pageLink">Страниц: <span data-bind="text: TotalPages"></span></span>
            
    <span data-bind="visible: isVisible" style="display: inline">
        <a href="#" class="pageLink pageLinkArrowLeftLeft" data-bind="click: scrollToFirst">&nbsp;</a>

        <!-- ko if: ellipsesLeftVisible -->
        <a href="#" class="pageLink" data-bind="click: scrollLeft">...</a>
        <!-- /ko -->
                    
        <!-- ko foreach: visiblePages -->
        <a href="#" class="pageLink" data-bind="click: $parent.CurrentPage, text: $data, css: {pageLinkActive: $parent.isCurrentPage($data)}, style: {fontWeight: $parent.isCurrentPage($data) ? 'bold' : null}"></a>
        <!-- /ko -->

        <!-- ko if: ellipsesRightVisible -->
        <a href="#" class="pageLink" data-bind="click: scrollRight">...</a>
        <!-- /ko -->
        <a href="#" class="pageLink pageLinkArrowRightRight" data-bind="click: scrollToLast">&nbsp;</a>
    </span>
    <span class="pageLink" style="display: inline;">
        На странице:&nbsp;
        <select style="width: 55px" data-bind="options: pageSizes, value: PageSize" />
    </span>
</div>