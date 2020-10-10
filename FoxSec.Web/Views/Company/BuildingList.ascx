<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.CompanyEditViewModel>" %>

<%int index = 0; foreach (var cb in Model.Company.CompanyBuildingItems){ cb.Index = index; %>
	<% Html.RenderPartial("Building", cb); %>
<%index++;} %>

<%= Html.ValidationMessage("Company.CompanyBuildingItems", null, new { @class = "error" })%>

