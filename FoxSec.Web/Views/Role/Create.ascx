<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.RoleEditViewModel>" %>
<%@ Import Namespace="FoxSec.DomainModel.DomainObjects" %>

<% if (TempData["ERROR"] != null)%>
<%{%>

<div style="clear:left;color:#BB0000 !important;display:block;font-style:normal;font-weight:bold !important;line-height:1.2em;padding:0 0.1em 0.3em 0.2em;">
      User with the same name is alredy exsist
  
  </div>

<%}%>



<div id="content_add_role_form" style='margin:10px; text-align:center;' >
<form id="createNewRole" action="">
<table width="100%">
    <tr>
		<td style='width:30%; padding:0 5px; text-align:right;'><label for='Add_role_title'><%:ViewResources.SharedStrings.UsersRoleTitle %></label></td>
		<td style='width:70%; padding:0 5px;'>
			<%=Html.TextBox("Role.Name", Model.Role.Name, new { @style = "width:80%;text-transform: capitalize;" })%>
			<%= Html.ValidationMessage("Role.Name", null, new { @class = "error" })%>
		</td>
    </tr>
    <tr>
		<td style='width:30%; padding:0 5px; text-align:right; vertical-align:top;'><label for='Add_role_description'><%:ViewResources.SharedStrings.CommonDescription %></label></td>
		<td style='width:70%; padding:0 5px;'>
			<%=Html.TextArea("Role.Description", Model.Role.Description, new { @style="width:80%;text-transform: capitalize;" }) %>
			<%--<%= Html.ValidationMessage("Role.Description", null, new { @class = "error" })%>--%>
		</td>
    </tr>
    <tr>
		<td style='width:30%; padding:0 5px; text-align:right; vertical-align:top;'><label for='Add_role_activation'><%:ViewResources.SharedStrings.FilterActive %></label></td>
		<td style='padding:0 5px;'><%=Html.CheckBox("Role.Active", Model.Role.Active = true) %></td>
    </tr>
	<tr>
		<td  style='width:30%; padding:0 5px; text-align:right; vertical-align:top;'> 
			<label for='Add_role_activation'><%:ViewResources.SharedStrings.RolesRoleType %>:</label>
		</td>
		<td style='padding:0 5px;' nowrap>
			<table style="margin:0px; padding:0px">
				<tr>
					<%if(Model.User.RoleTypeId <= (int)RoleTypeEnum.SA){%>
						<td class = "tipsy_we" original-title = '<%=ViewResources.SharedStrings.RolesRoleTypesSA %>'>
							<label>SA</label>
							<%=Html.RadioButton("Role.RoleTypeId", RoleTypeEnum.SA) %>&nbsp;&nbsp
						</td>
					<%} %>
					<%--<%if(Model.User.RoleTypeId <= (int)RoleTypeEnum.BA){%>
						<td class = "tipsy_we" original-title = '<%=ViewResources.SharedStrings.RolesRoleTypesBA %>'>
							<label >BA</label>
							<%=Html.RadioButton("Role.RoleTypeId", RoleTypeEnum.BA)%>&nbsp;&nbsp
						</td>
					<%} %>--%>
					<%if(Model.User.RoleTypeId <= (int)RoleTypeEnum.CM){%>
						<td class = "tipsy_we" original-title = '<%=ViewResources.SharedStrings.RolesRoleTypesCM %>'>
							<label>CM</label>
							<%=Html.RadioButton("Role.RoleTypeId", RoleTypeEnum.CM)%>&nbsp;&nbsp
						</td>
					<%} %>
					<%if(Model.User.RoleTypeId <= (int)RoleTypeEnum.DM){%>
						<td class = "tipsy_we" original-title = '<%=ViewResources.SharedStrings.RolesRoleTypesDM %>'>
							<label>DM</label>
							<%=Html.RadioButton("Role.RoleTypeId", RoleTypeEnum.DM)%>&nbsp;&nbsp
						</td>
					<%} %>
					<td class = "tipsy_we" original-title = '<%=ViewResources.SharedStrings.RolesRoleTypesUser %>'>
					<label >U</label>
					<%=Html.RadioButton("Role.RoleTypeId", RoleTypeEnum.U)%>
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
			<%=Html.CheckBox(string.Format("Role.RoleBuildingItems[{0}].IsChecked", Model.Role.RoleBuildingItems.IndexOf(roleBuilding)), roleBuilding.IsChecked)%>
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
</form>
</div>

<script type="text/javascript" language="javascript">

	$(document).ready(function () {
		$(".tipsy_we").attr("class", function () {
			$(this).tipsy({ gravity: $.fn.tipsy.autoWE, html: true });
		});
	});
</script>