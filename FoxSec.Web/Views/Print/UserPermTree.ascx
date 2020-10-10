<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.PermissionTreeViewModel>" %>

<ul>
<% foreach (var country in Model.Countries) {%>
    <li>
		<%= Html.Encode(country.Name)%>
        <% foreach (var town in Model.Towns) { if (town.ParentId == country.MyId) { %>
        <ul>
            <li>
				<%= Html.Encode(town.Name)%>
                <% foreach (var building in Model.Buildings) { if (building.ParentId == town.MyId) { %>
                <ul>
                    <li>
						<%= Html.Encode(building.Name)%>
                        <% foreach (var floor in Model.Floors) { if (floor.ParentId == building.MyId) { %>
                        <ul>
                            <li>
								<%= Html.Encode(floor.Name)%>
                                <% foreach (var obj in Model.Objects) { if (obj.ParentId == floor.MyId){ %>
                                 <ul>
                                    <% if(Model.ActiveObjectIds != null && Model.ActiveObjectIds.Contains(obj.MyId)) { %>
                                    <li>
                                        <%= Html.Encode(obj.Name)%> - <%= obj.TimeZoneName %>
                                        <% if(obj.IsRoom == 1){ %> (
                                            <% if(obj.IsArming){ %> <%:ViewResources.SharedStrings.PermissionsArmingShort %> <% } %>
                                            <% if(obj.IsDefaultArming){ %> <%:ViewResources.SharedStrings.PermissionsDefaultArmingShort %> <% } %>
                                            <% if(obj.IsDisarming){ %> <%:ViewResources.SharedStrings.PermissionsDisarmingShort %> <% } %>
                                            <% if(obj.IsDefaultDisarming){ %> <%:ViewResources.SharedStrings.PermissionDefaultDisarmingShort %> <% } %> )
                                        <% } %>
                                    </li>
                                    <% } %>
                                </ul>
                                <% }} %>
                            </li>
                        </ul>
                    <% }} %>
                    </li>
                </ul>
                <% }} %>
            </li>
        </ul>
        <% }} %>
    </li>
    <% } %>
</ul>
