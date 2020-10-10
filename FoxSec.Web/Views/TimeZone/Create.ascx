<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.TimeZoneEditViewModel>" %>
<div id="content_create_timezone_form" style='margin:10px; text-align:center;' >
<form id="createNewTimeZone" action = "">
<table width="100%">
    <tr>
		<td style='width:30%; padding:0 5px; text-align:right;'><label for='TimeZone_Name'><%:ViewResources.SharedStrings.UsersName %></label></td>
		<td style='width:70%; padding:0 5px;'>
			<%= Html.TextBox("TimeZone.Name", Model.TimeZone.Name, new { @style = "width:80%;text-transform: capitalize;", @id = "create_TimeZone_Name", @maxlength = "50", @onkeydown = "javascript:Limit50Symbols();" })%>
			<%= Html.ValidationMessage("TimeZone.Name", null, new { @class = "error" })%>
		</td>
    </tr>
</table>
</form>

<script type="text/javascript" language="javascript">

    function Limit50Symbols() {
        if ($("input#create_TimeZone_Name").val().length >= 50) { ShowDialog('<%=ViewResources.SharedStrings.CommonMax50symbols %>', 2000); }
    }

</script>
</div>