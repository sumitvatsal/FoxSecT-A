<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.CompanyBuildingItem>" %>
<div id='companyBuilding'>
	<div>
		<table cellpadding="3" cellspacing="3" style="margin: 0; width: 100%; padding: 3px; border-spacing: 3px;">
			<tr id="companyBuildingRow">
				<td style="width:50%">
					<%if(Model.IsAvailable ){ %>
						<%=Html.DropDownList(string.Format("Company.CompanyBuildingItems[{0}].BuildingId", Model.Index), new SelectList(Model.BuildingItems, "Value", "Text", Model.BuildingId ), ViewResources.SharedStrings.DefaultDropDownValue, new{ @onchange = "javascript:ChangeBuilding(this)"}) %>
					<%} else{%>
						<%=Html.Hidden(string.Format("Company.CompanyBuildingItems[{0}].BuildingId", Model.Index), Model.BuildingId)%>
						<%:Html.Encode(Model.BuildingName) %>
					<%} %>
					<%=Html.Hidden(string.Format("Company.CompanyBuildingItems[{0}].IsAvailable", Model.Index), Model.IsAvailable)%>
				</td>
				<td  style="width:20%">
					<div id="buildingFloors">
						<% Html.RenderPartial("Floors", Model); %>
					</div>
				</td>
				<td style="width:15%">
					<%if(Model.IsAvailable ){ %>
						<span id = 'building_delete' class="ui-icon ui-icon-circle-close" style="cursor:pointer;" onclick="javascript:RemoveBuilding(this);"></span>
					<%} %>
				</td>
				<td>
					<span id = 'building_add' class="ui-icon ui-icon-circle-plus" style="cursor:pointer;" onclick="javascript:AddBuilding();"></span>
				</td>
			</tr>
		</table>
	</div>
	<hr />
</div>

<script type="text/javascript" language="javascript">

	$(document).ready(function () {
		if ($("[id*=companyBuildingRow]").size() > 1) {
			$("[id*=companyBuildingRow]").each(function () {
				$(this).find("[id*=building_delete_]").show();
			});
		} 
	});
</script>