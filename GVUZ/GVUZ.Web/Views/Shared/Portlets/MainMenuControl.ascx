<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.Portlets.MainMenuViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Portlets" %>

<%@ Register TagPrefix="gv" TagName="ResourcesHolder" Src="~/Views/Shared/Controls/ResourcesHolderControl.ascx" %>

<gv:resourcesholder runat="server" />

<div id="menucontainer">
    <ul id="menu">
        <li><a <% if (Model.MenuItems == MenuItemsType.Search)
                  { %> class="activeMenu"<% } %> 
                                                                                       href="<%= PortletLinkHelper.Link(PortletType.SearchModule) %>">Поиск</a></li>
        <li><a <% if (Model.MenuItems == MenuItemsType.Applications)
                  { %> class="activeMenu"<% } %> 
                                                                                             href="<%= Url.Generate<EntrantController>(x => x.ApplicationList()) %>">Список заявлений</a></li>
        <li><a <% if (Model.MenuItems == MenuItemsType.Personal)
                  { %> class="activeMenu"<% } %> 
                                                                                         href="<%= PortletLinkHelper.PersonalRecordsLink() %>">Личное дело</a></li>
    </ul>
</div>
<div id="header">
    <h1><%= Model.Title %></h1>
</div>