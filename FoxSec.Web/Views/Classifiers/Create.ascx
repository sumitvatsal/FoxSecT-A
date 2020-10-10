<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.ClassificatorEditViewModel>" %>
<div id="content_add_classificator_form" style='margin:10px; text-align:center;' >
<form id="createNewClassificator">
<table width="100%">
    <tr>
		<td style='width:30%; padding:0 5px; text-align:right;'><label for='Add_classificator_description'><%:ViewResources.SharedStrings.UsersName %></label></td>
		<td style='width:70%; padding:0 5px;'>
			<%=Html.TextBox("Classificator.Description", Model.Classificator.Description, new { @style = "width:80%;text-transform: capitalize;", id = "Edit_classificator_description" })%>
			<%= Html.ValidationMessage("Classificator.Description", null, new { @class = "error" })%>
        </td>
    </tr>
    <tr>
		<td style='width:30%; padding:0 5px; text-align:right; vertical-align:top;'><label for='Add_classificator_comments'><%:ViewResources.SharedStrings.CommonComments %></label></td>
		<td style='width:70%; padding:0 5px;'>
			<%=Html.TextArea("Classificator.Comments", Model.Classificator.Comments, new { @style = "width:80%;", id = "Edit_classificator_comments" })%>
			<%= Html.ValidationMessage("Classificator.Comments", null, new { @class = "error" })%>
        </td>
    </tr>
</table>
</form>
</div>