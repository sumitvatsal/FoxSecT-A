<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<% if(!Request.IsAuthenticated) { %>
<%= Html.ActionLink("Register", "Register", "Account") %>
|
<%= Html.ActionLink("Login", "Login", "Account") %>
<% }
   else { %>
Welcome <b>
    <%: Page.User.Identity.Name%>
</b>! |
<%= Html.ActionLink("Logout", "LogOff", "Account") %>
<% } %>