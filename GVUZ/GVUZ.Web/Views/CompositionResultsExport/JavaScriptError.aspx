<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<html>
<head runat="server">
    <title>JavaScriptError</title>
    <script type="text/javascript">
        alert('<%= ViewData["JavaScriptErrorMessage"] %>');
    </script>
</head>
<body>
</body>
</html>
