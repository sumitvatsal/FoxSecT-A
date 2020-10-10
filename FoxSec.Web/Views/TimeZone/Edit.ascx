<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.TimeZoneEditViewModel>" %>
<div id="content_edit_timezone_form" style='margin:10px; text-align:center;' >
<form id="editTimeZone" action = "">
<table width="100%">
    <%=Html.Hidden("TimeZone.Id", Model.TimeZone.Id)%>
    <tr>
		<td style='width:30%; padding:0 5px; text-align:right;'><label for='TimeZone_Name'><%:ViewResources.SharedStrings.UsersName %></label></td>
		<td style='width:70%; padding:0 5px;'>
			<%=Html.TextBox("TimeZone.Name", Model.TimeZone.Name, new { @style = "width:80%;text-transform: capitalize;", @id = "edit_TimeZone_Name" })%>
			<%= Html.ValidationMessage("TimeZone.Name", null, new { @class = "error" })%>
		</td>
    </tr>
</table>
</form>
</div>