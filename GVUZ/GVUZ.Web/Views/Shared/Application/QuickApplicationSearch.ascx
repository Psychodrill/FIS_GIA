<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Security" %>

<%
    if (Request.IsAuthenticated) {
%>
	<% if(UserRole.CurrentUserInRole(UserRole.StringValue(Role.EduUsr))) { %>
		<%--<span class="line2e"><%:  Html.ActionLink("Расширенный поиск", "ExtendedApplicationList", "InstitutionApplication") %>&nbsp;&nbsp;</span>--%>
            <div id="divFastSearch">
			    <input type="text" placeholder="Фамилия или № заявления" id="inpFastSearch" />
                <button></button>
		    </div>
		    <script type="text/javascript">
			    jQuery(document).ready(function ()
			    {
				    jQuery('#divFastSearch').keypress(function (evt)
				    {
					    if (evt.keyCode == 13)
					    {
						    window.location = '<%= Url.Generate<InstitutionApplicationController>(x => x.ExtendedApplicationList(null)) %>?searchTerm='
						    + encodeURIComponent(jQuery('#inpFastSearch').val())
					    } 
				    })

                    jQuery('#divFastSearch button').click(function (evt)
				    {
						    window.location = '<%= Url.Generate<InstitutionApplicationController>(x => x.ExtendedApplicationList(null)) %>?searchTerm='
						    + encodeURIComponent(jQuery('#inpFastSearch').val())
				    })

			    })
		    </script>
	<%} %>
<%
    }
%>
<% if(!UserRole.CurrentUserInRole(UserRole.FBDAdmin)) { %>
        <span style="display: none"></span>
    <% } else { %>
        <span id="connectedUsers"></span>
    <% } %>