<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.TAMsUserViewModel>" %>

<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title></title>
</head>
<body>
    <div>
        <h1 id="MyH1">My T&A report </h1>
    </div>
    <% 
        Html.RenderPartial("TAMsUserReport", Model.TAMsUserItems);
    %>
</body>
<script type="text/javascript">
    document.getElementById("MyH1").innerHTML = "My T&A report <" + $('#FromDateTA').val() + "-" + $("#ToDateTA").val() + ">";
</script>
</html>
