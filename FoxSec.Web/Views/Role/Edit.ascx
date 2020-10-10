<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.RoleEditViewModel>" %>
<%@ Import Namespace="FoxSec.DomainModel.DomainObjects" %>

<style type="text/css">
	.TFtable{
		width:100%; 
		border-collapse:collapse; 
	}
	.TFtable td{ 
		padding:3px;
	}
	/* provide some minimal visual accomodation for IE8 and below */
	.TFtable tr{
		background:#CCC;
	}
	/*  Define the background color for all the ODD background rows  */
	.TFtable tr:nth-child(odd){ 
		background: white;
	}
	/*  Define the background color for all the EVEN background rows  */
	.TFtable tr:nth-child(even){
		background: #CCC;
	}
</style>


<div id="content_edit_role_form" style='margin: 10px; text-align: center;'>
<form id="editRole" action="">
<table width="100%"  cellpadding='0' cellspacing='0' style='margin: 0; width: 100%; padding: 0; border-spacing: 0; border: none;'>
<%=Html.Hidden("Role.Id", Model.Role.Id)%>
<%=Html.Hidden("IsViewOnlyMode", Model.IsViewOnlyMode) %>
<tr>
    <td style='width: 30%; padding: 0 5px; text-align: right;'>
        <label for='Edit_role_title_<%= Model.Role.Id %>'><%:ViewResources.SharedStrings.UsersRoleTitle %></label>
    </td>
    <td style='width: 70%; padding: 0 5px;'>
        <%=Html.TextBox("Role.Name", Model.Role.Name, new { @id = "Edit_role_title_" + Model.Role.Id, @style = "width:80%;text-transform: capitalize;" })%>
		<%=Html.ValidationMessage("Role.Name", null, new { @class = "error" })%>
    </td>
</tr>
<tr>
    <td style='width: 30%; padding: 0 5px; text-align: right; vertical-align: top;'>
        <label for='Edit_role_description_<%= Model.Role.Id %>'><%:ViewResources.SharedStrings.CommonDescription %></label>
    </td>
    <td style='width: 70%; padding: 0 5px;'>
        <%=Html.TextArea("Role.Description", Model.Role.Description, new { @id = "Edit_role_description_" + Model.Role.Id, @style = "width:80%;height:50px;text-transform: capitalize;"})%>
		<%=Html.ValidationMessage("Role.Description", null, new { @class = "error" })%>
    </td>
</tr>
<tr>
    <td style='width: 30%; padding: 0 5px; text-align: right; vertical-align: top;'>
        <label for='Edit_role_activation_<%= Model.Role.Id %>'><%:ViewResources.SharedStrings.FilterActive %></label>
    </td>
    <td style='padding: 0 5px;'>
        <%=Html.CheckBox("Role.Active", Model.Role.Active, new { @id = "Edit_role_activation_" + Model.Role.Id })%>
    </td>
</tr>
<tr>
<td  style='width:30%; padding:0 5px; text-align:right; vertical-align:top;'> 
	<label for='Edit_role_type'><%:ViewResources.SharedStrings.RolesRoleType %>:</label>
</td>
<td style='padding:0 5px;'>
	<%:Html.Hidden("CurrentRoleId", Model.Role.Id) %>
	<%:Html.Hidden("CurrentRoleTypeId", Model.Role.RoleTypeId) %>
	<table style="margin:0px; padding:0px">
				<tr>
					<%if(Model.User.RoleTypeId <= (int)RoleTypeEnum.SA){%>
						<td class = "tipsy_we" original-title = '<%=ViewResources.SharedStrings.RolesRoleTypesSA %>'>
							<label>SA</label>
							<%=Html.RadioButton("Role.RoleTypeId", RoleTypeEnum.SA, new {onclick = "SwithRole(this)"}) %>&nbsp;&nbsp
						</td>
					<%} %>
					<%if(Model.User.RoleTypeId <= (int)RoleTypeEnum.BA){%>
						<td class = "tipsy_we" original-title = '<%=ViewResources.SharedStrings.RolesRoleTypesBA %>'>
							<label >BA</label>
							<%=Html.RadioButton("Role.RoleTypeId", RoleTypeEnum.BA, new { onclick = "SwithRole(this)" })%>&nbsp;&nbsp
						</td>
					<%} %>
					<%if(Model.User.RoleTypeId <= (int)RoleTypeEnum.CM){%>
						<td class = "tipsy_we" original-title = '<%=ViewResources.SharedStrings.RolesRoleTypesCM %>'>
							<label>CM</label>
							<%=Html.RadioButton("Role.RoleTypeId", RoleTypeEnum.CM, new { onclick = "SwithRole(this)" })%>&nbsp;&nbsp
						</td>
					<%} %>
					<%if(Model.User.RoleTypeId <= (int)RoleTypeEnum.DM){%>
						<td class = "tipsy_we" original-title = '<%=ViewResources.SharedStrings.RolesRoleTypesDM %>'>
							<label>DM</label>
							<%=Html.RadioButton("Role.RoleTypeId", RoleTypeEnum.DM, new { onclick = "SwithRole(this)" })%>&nbsp;&nbsp
						</td>
					<%} %>
					<td class = "tipsy_we" original-title = '<%=ViewResources.SharedStrings.RolesRoleTypesUser %>'>
					<label >U</label>
					<%=Html.RadioButton("Role.RoleTypeId", RoleTypeEnum.U, new { onclick = "SwithRole(this)" })%>
					</td>
			</tr>
		</table>
	</td>
</tr>
<%foreach (var roleBuilding in Model.Role.RoleBuildingItems){%>
	<tr>
	<td  style='width:30%; padding:0 5px; text-align:right; vertical-align:top;'> 
		<%if( Model.Role.RoleBuildingItems.IndexOf(roleBuilding) == 0){%>
		<label for='Add_role_activation'><%:ViewResources.SharedStrings.BuildingsTabName %>:</label>
		<%}%>
	</td>
	<td style='padding:0 5px;'>
		<%if(roleBuilding.IsAvailable){%>
			<%=Html.CheckBox(string.Format("Role.RoleBuildingItems[{0}].IsChecked", Model.Role.RoleBuildingItems.IndexOf(roleBuilding)), roleBuilding.IsChecked)%>
		<%} else{%>
			<%=Html.CheckBox("FakeBox", roleBuilding.IsChecked, new { disabled = "disabled"}) %>
			<%=Html.Hidden(string.Format("Role.RoleBuildingItems[{0}].IsChecked", Model.Role.RoleBuildingItems.IndexOf(roleBuilding)), roleBuilding.IsChecked) %>
		<%} %>
		<label><%=roleBuilding.BuildingName %></label>
		<%=Html.Hidden(string.Format("Role.RoleBuildingItems[{0}].Id", Model.Role.RoleBuildingItems.IndexOf(roleBuilding)), roleBuilding.Id) %>
		<%=Html.Hidden(string.Format("Role.RoleBuildingItems[{0}].RoleId", Model.Role.RoleBuildingItems.IndexOf(roleBuilding)), roleBuilding.RoleId) %>
		<%=Html.Hidden(string.Format("Role.RoleBuildingItems[{0}].BuildingId", Model.Role.RoleBuildingItems.IndexOf(roleBuilding)), roleBuilding.BuildingId)%>
		<%=Html.Hidden(string.Format("Role.RoleBuildingItems[{0}].BuildingName", Model.Role.RoleBuildingItems.IndexOf(roleBuilding)), roleBuilding.BuildingName)%>
	</td>
</tr>
<%}%>
<tr>
		<td  style='width:30%; padding:0 5px; text-align:right; vertical-align:top;'> 
		</td>
		<td style='padding:0 5px;'>
			<%= Html.ValidationMessage("Role.RoleBuildingItems", null, new { @class = "error" })%>
		</td>
	</tr>
</table>

<div id="panel_role_access">
<ul>
    <li><a href="#tab_role_access_menus"><%:ViewResources.SharedStrings.RolesMenuAccess %></a></li>
    <li><a href="#tab_role_access_service"><%:ViewResources.SharedStrings.RolesFoxsecAccess %></a></li>
</ul>
<div id="tab_role_access_menus">
    <% Html.RenderPartial("MenuAccess", Model); %>
</div>
<div id="tab_role_access_service">
    <table cellpadding="3" cellspacing="3" style="margin: 0; width: 100%; padding: 3px; border-spacing: 4px;" class="TFtable">
    <tr>
        <th style="width: 20px;">ID</th>
        <th style="width: 50%;"><%:ViewResources.SharedStrings.RolesServiceTitle %></th>
        <th style="width: 50%; text-align: right;"><%:ViewResources.SharedStrings.CommonIsAllowed %></th>
    </tr>
    <%int i = 1; int index = 0; foreach (var permission in Model.Role.PermissionItems)
		{%>
    <tr <% if(!permission.IsItemAvailable || (Model.IsViewOnlyMode && ! permission.IsSelected)) {%>  style="display:none" <% } %>>
        <%if(Model.IsViewOnlyMode){%>
					<td style="width: 20px;"><%= permission.IsSelected ? i++ : 0 %></td>
				<%}else{%>
					<td style="width: 20px;"><%= permission.IsItemAvailable ? i++ : 0 %></td>
				<%} %>
        <td style="width: 50%;"><%= permission.Text %></td>
        <td style="width: 50%; text-align: right;">
			<%:Html.CheckBox(string.Format("Role.PermissionItems[{0}].IsSelected", index), permission.IsSelected)%>
			<%:Html.Hidden(string.Format("Role.PermissionItems[{0}].Value", index), permission.Value)%>
			<%:Html.Hidden(string.Format("Role.PermissionItems[{0}].IsItemAvailable", index), permission.IsItemAvailable)%>
			<%:Html.Hidden(string.Format("Role.PermissionItems[{0}].Text", index), permission.Text)%>
		</td>
    </tr>
    <%index++;
		} %>
    </table>
</div>
</div>
</form>
</div>

<script type="text/javascript" language="javascript">

    $(document).ready(function () {
        $("#panel_role_access").attr("id", function () {
            $("#panel_role_access").tabs({
                beforeLoad: function( event, ui )  {
                    ui.ajaxSettings.async= false,
                    ui.ajaxSettings.error= function (xhr, status, index, anchor) {
                        $(anchor.hash).html("Couldn't load this tab!");
                    }
                },
                fx: {
                    opacity: "toggle",
                    duration: "fast"
                },
                active: 0
            });
        });
        if ($("#IsViewOnlyMode").attr('value').toLowerCase() == 'true') {
            $('#editRole').find('input').each(function () {
                $(this).attr('disabled', 'disabled');
            });
            $('#editRole').find('textarea').each(function () {
                $(this).attr('disabled', 'disabled');
            });
              }

              $(".tipsy_we").attr("class", function () {
              	$(this).tipsy({ gravity: $.fn.tipsy.autoWE, html: true });
              });
    });

   function SwithRole(cntr) {
        if ($("#CurrentRoleTypeId").attr('value') != $(cntr).attr('value')) {
            $.get('/Role/GetMenuesByRoleType', { roleType: $(cntr).attr('value') }, function (html) {
                $("#tab_role_access_menus").html(html);
            })
        }
        $("#CurrentRoleTypeId").attr('value', $(cntr).attr('value'));
    }

</script>