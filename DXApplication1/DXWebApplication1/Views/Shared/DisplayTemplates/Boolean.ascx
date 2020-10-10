<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<System.Boolean?>" %>
<% Html.DevExpress().CheckBoxFor(m => m, s => {
       s.ReadOnly = true;
}).Render(); %>