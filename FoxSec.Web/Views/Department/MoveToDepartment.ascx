<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.MoveToDepartmentViewModel>" %>

<%=Html.DropDownList("Departments", Model.Departments, ViewResources.SharedStrings.DefaultDropDownValue, new {@id = "selectedDepartmentForMove"}) %>