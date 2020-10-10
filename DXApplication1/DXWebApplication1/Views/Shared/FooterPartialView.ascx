<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<div class="footerMenu">
    
</div>
<div class="copyright">
    <% Html.DevExpress().Label(settings => {
           settings.EncodeHtml = false;
           settings.Text = DateTime.Now.Year + " &copy; Copyright by [company name]";
       }).Render();
    %>
</div>