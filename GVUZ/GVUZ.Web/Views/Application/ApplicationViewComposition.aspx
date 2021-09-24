<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.CompositionViewModel>" %>

<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="GVUZ.Data.Helpers" %>
<%@ Import Namespace="GVUZ.Web.ViewModels" %>

<script src="../../Resources/Scripts/jquery-1.4.2.min.js"></script>
<script type="text/javascript" src="<%= Url.Scripts("libs/jquery.ui.core.js") %>"></script>
<script type="text/javascript" src="<%= Url.Scripts("libs/jquery.ui.widget.js") %>"></script>
<script type="text/javascript" src="<%= Url.Scripts("libs/jquery.ui.dialog.js") %>"></script>
<script src="<%= Url.Scripts("libs/jQuery.blockUI.js") %>"></script>
<script type="text/javascript" src='<%= Url.Scripts("jquery.unobtrusive-ajax.min.js") %>'></script>
<script src="../../Resources/Scripts/AjaxHelper.js"></script>

<style>
    html {
        width: 100%;
        height: 100%;
        background: url(logo.png) center center no-repeat;
    }
    img {
        position: absolute;
        top: 50%;
        left: 50%;
        width: 700px;
        height: 700px;
        margin-top: -350px;
        margin-left: -350px;
    }
    span.error
    {
        color:red;
    }
</style>

<script type="text/javascript">
    function Show(path) {
        
        if(path == ""){
            $("#image").attr("src", "../../Resources/Images/not-found1.png");
            return;
        }

        doPostAjax('<%= Url.Generate<ApplicationController>(x => x.LoadComposition(null)) %>', 
            JSON.stringify({ path: path }),
            function (data) {
                if (data.IsError) { 
                    $("#errorText").text(data.Data);
                    return; 
                }
                $("#image").attr("src", data.Data);
            }
        );
    }
</script>

<div>
    <h2>Просмотр бланков сочинения для заявления № <%=Model.ApplicationId%></h2>

    <h3>Страницы: 
        <%foreach (var item in Model.CompositionPages) {
            var id = item.Id; 
            var func = "Show('" + item.Path +  "')"; 
        %>
            <input type="button" value=<%=id%> class="button3" width: "35px" onclick=<%=func%> />
        <%}%>
    </h3>
</div>

<img id="image">
<span id="errorText" class="error"></span>


<script type="text/javascript">
    var absoluteAppPath = '<%= Url.Resource("") %>'
    var p = '<%=Model.Path%>';
    Show(p);
</script>

