<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% Html.DevExpress().TextBoxFor(m => m, s => {
       s.Properties.Password = true;
   }).Render(); %>