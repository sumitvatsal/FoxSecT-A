<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.PermissionEditViewModel>" %>
<div id="content_create_permission_form" style='margin:10px; text-align:center;' >
<table width="100%">
    <tr>
		<td style='width:30%; padding:0 5px; text-align:right;'><label for='Permission_Name'><%:ViewResources.SharedStrings.UsersName %>: </label></td>
			<td style='width:70%; padding:0 5px;'><%=Html.TextBox("create_Permission_Name", Model.Permission.Name, new { @style = "width:80%;text-transform: capitalize;", @id = "create_Permission_Name", @maxlength = "50", @onkeydown = "javascript:Limit50Symbols();" })%>
		</td> 
    </tr>
    <tr>
		<td style='width:30%; padding:0 5px; text-align:right;'><label for='Groups'><%:ViewResources.SharedStrings.PermissionsCopyDataFrom %>: </label></td>
		<td style='width:70%; padding:0 5px;'><%=Html.DropDownList("copy_from_GroupId", Model.Groups, ViewResources.SharedStrings.PermissionsSelectPermissionGroupOption )%></td>
    </tr>
</table>
    <script src="../../css/select2.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
    $(document).ready(function () {
        $("#copy_from_GroupId").select2({
            dropdownParent: $("div#modal-dialog")
        });
    })
    function Limit50Symbols() {
        if ($("input#create_Permission_Name").val().length >= 50) { ShowDialog('<%=ViewResources.SharedStrings.CommonMax50symbols %>', 2000); }
    }

</script>
</div>