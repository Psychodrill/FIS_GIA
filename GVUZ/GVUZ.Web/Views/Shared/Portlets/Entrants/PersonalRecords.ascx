<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.Portlets.Entrants.PersonalRecordsViewModel>" %>
<%@ Register TagPrefix="gv" TagName="tabcontrol" Src="~/Views/Shared/Common/EntrantsTabControl.ascx" %>
<%@ Register TagPrefix="gv" TagName="newtabcontrol" Src="~/Views/Shared/Common/NewEntrantTabControl.ascx" %>

<div class="gvuzPortlet">

    <div class="gvuzTab">
        <% if (Model.EntrantID == 0)
           { %>
            <gv:newtabcontrol runat="server" id="tabControl" />
        <% }
           else
           { %>
            <gv:tabcontrol runat="server" id="tabControl1" />
        <% } %>

    </div>
    <%= Model.TabContent %>

</div>