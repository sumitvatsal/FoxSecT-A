<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% Html.DevExpress().MemoFor(m => m, s => {
       s.Width = 170;
   }).Render(); %>