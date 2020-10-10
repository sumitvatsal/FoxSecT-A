<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.TAGlobalBuildingObjectsViewModel>" %>


<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title></title>
    <link href="https://cdn.datatables.net/1.10.13/css/dataTables.bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.datatables.net/1.10.13/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/1.10.13/js/dataTables.bootstrap.min.js"></script>
    <link href="https://cdn.datatables.net/1.10.13/css/dataTables.foundation.min.css" rel="stylesheet" />
</head>
<body>

    <div>
        <br />
        <%:ViewResources.SharedStrings.WorkBuildingObjectsDescription %>
        <br />
    </div>
    <% 
        Html.RenderPartial("TABoGridViewPartialView", Model.BuildingObjects);%>
</body>
</html>
