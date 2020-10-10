<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.TAReportMounthViewModel>" %>

<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title></title>
</head>
<body>
    <div>
        <h1>pealkiri!</h1>
    </div>
    <% 
        Html.RenderPartial("TABoGridViewPartialView1", Model.TAReportMounthItems);
    %>
</body>
</html>
