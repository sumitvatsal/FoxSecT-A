<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.TAMsUserViewModel>" %>  
<html>
<body>
    <% 
      Html.RenderPartial("TAReportDetailedGridViewPartialView", Model.TAMsUserItems);
    %>  
</body>
</html>
