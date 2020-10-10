<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.PermissionTreeViewModel>" %>

<span class="ui-icon ui-icon-wrench" style="cursor:pointer" onclick="javascript:$('td#buildingObjectStatusIcon').toggle(300);"></span>

<ul id="permissions" class="treeview-red">
<% foreach (var country in Model.Countries) {%>
    <li>
        &nbsp;<%= Html.Encode(country.Name)%>
        <% foreach (var town in Model.Towns) { if (town.ParentId == country.MyId) { %>
        <ul>
            <li>

                &nbsp;<%= Html.Encode(town.Name)%>
                <% foreach (var building in Model.Buildings) { if (building.ParentId == town.MyId) { %>
                <ul>
                    <li>
                        <span <% if(!string.IsNullOrWhiteSpace(building.Comment)){ %> class = "tipsy_we" original-title="<%= building.Comment %>" <% } %> style="color:<%= !string.IsNullOrEmpty(building.Comment) ? "Green" : "#222222"%>">&nbsp;<%= Html.Encode(building.Name)%></span>
                        <% foreach (var floor in Model.Floors) { if (floor.ParentId == building.MyId) { %>
                        <ul>
                            <li>
                                <span <% if(!string.IsNullOrWhiteSpace(floor.Comment)){ %>class = "tipsy_we" original-title="<%= floor.Comment %>" <% } %> style="color:<%= !string.IsNullOrEmpty(floor.Comment) ? "Green" : "#222222"%>">&nbsp;<%= Html.Encode(floor.Name)%></span>
                                <% foreach (var obj in Model.Objects) { if (Model.User.IsBuildingAdmin || Model.User.IsSuperAdmin) { if (obj.ParentId == floor.MyId) { %>
                                 <ul>
                                    <li id='permissionTreeLi_<%= obj.MyId %>'>
                                        <table width="100%">
                                        <tr>
                                        <td id="buildingObjectStatusIcon" style="width:18px;vertical-align:text-top;display:none">
                                        <% if (obj.StatusIcon != String.Empty){ %><img src="<%= obj.StatusIcon %>" alt = ""/><% } %>
                                        </td>
                                        <% if (!obj.IsDefaultTimeZone && Model.ActiveObjectIds.Contains(obj.MyId))
                                           {%><td style="width:15px"><span class="ui-icon ui-icon-person"></span></td><% } %>
                                        <td>
											<span <% if (!string.IsNullOrWhiteSpace(obj.Comment)){ %>class = "tipsy_we" original-title="<%= obj.Comment %>" <% } %>>
												&nbsp;<a onclick="javascript:GetZoneByObject(<%= obj.MyId %>,<%= obj.IsRoom %>,'<%= obj.Name %>');" style="color:<%= !string.IsNullOrEmpty(obj.Comment) ? "Green" : "#222222"%>"><%= Html.Encode(obj.Name)%></a>
												<input id='permissionTreeObject_<%= obj.MyId %>' name='<%= obj.MyId %>' onclick="javascript:CheckRoom(this,<%= obj.IsRoom %>);" type="checkbox" <% if(Model.ActiveObjectIds != null && Model.ActiveObjectIds.Contains(obj.MyId)){ %>checked = "checked" <% } %>/>
											</span>
										</td>
                                        </tr>
                                        </table>
                                        <% if(obj.IsRoom == 1){ %>
                                        <table id="permZoneTreeListRoomNode_<%= obj.MyId %>" width="100%" style = '<%=string.Format("background-color:#C0C0C0; display:{0}", (Model.ActiveObjectIds != null && Model.ActiveObjectIds.Contains(obj.MyId)) ? "block" : "none") %>'>
                                        <tr id="armingDisarminRow">
                                        <td style="font-size: 10px; color:Red">&nbsp;
                                            <input type="checkbox" <% if(obj.IsDisarming){ %>checked="checked"<% } %> onclick="javascript:ToggleDisarming('<%= obj.MyId %>');" /><%:ViewResources.SharedStrings.PermissionsDisarmingShort %>
                                            <input type="checkbox" <% if(obj.IsDefaultDisarming){ %>checked="checked"<% } %> onclick="javascript:ToggleDefaultDisarming('<%= obj.MyId %>');" /><%:ViewResources.SharedStrings.PermissionDefaultDisarmingShort %>
                                            <input type="checkbox" <% if(obj.IsArming){ %>checked="checked"<% } %> onclick="javascript:ToggleArming('<%= obj.MyId %>');" /><%:ViewResources.SharedStrings.PermissionsArmingShort %>
                                            <input type="checkbox" <% if(obj.IsDefaultArming){ %>checked="checked"<% } %> onclick="javascript:ToggleDefaultArming('<%= obj.MyId %>');" /><%:ViewResources.SharedStrings.PermissionsDefaultArmingShort %>

                                        </td>
                                        </tr>
                                        <% } %>
                                        </table>
                                    </li>
                                </ul>
                                <% }} else { if (!Model.IsOriginal || ((Model.ActiveObjectIds != null) && (Model.ActiveObjectIds.Contains(obj.MyId)))) { if (obj.ParentId == floor.MyId) { %>
                                <ul>
                                    <li id='permissionTreeLi_<%= obj.MyId %>'>
                                        <table width="100%">
                                        <tr>
                                        <td id="buildingObjectStatusIcon" style="width:18px;vertical-align:text-top;display:none">
                                        <% if (obj.StatusIcon != String.Empty){ %><img src="<%= obj.StatusIcon %>" alt = ""/><% } %>
                                        </td>
                                        <% if (!obj.IsDefaultTimeZone && Model.ActiveObjectIds.Contains(obj.MyId))
                                           {%><td style="width:15px"><span class="ui-icon ui-icon-person"></span></td><% } %>
                                        <td>
											<span <% if (!string.IsNullOrWhiteSpace(obj.Comment)){ %>class = "tipsy_we" original-title="<%= obj.Comment %>" <% } %>>
												&nbsp;<a onclick="javascript:GetZoneByObject(<%= obj.MyId %>,<%= obj.IsRoom %>,'<%= obj.Name %>');" style="color:<%= !string.IsNullOrEmpty(obj.Comment) ? "Green" : "#222222"%>"><%= Html.Encode(obj.Name)%></a>
												<input id='permissionTreeObject_<%= obj.MyId %>' name='<%= obj.MyId %>' onclick="javascript:CheckRoom(this,<%= obj.IsRoom %>);" type="checkbox" <% if(Model.ActiveObjectIds != null && Model.ActiveObjectIds.Contains(obj.MyId) && Model.IsGroupExist ) { %>checked = "checked" <% } %>
												<% if(Model.IsCurrentUserAssignedGroup ) { %>disabled = "disabled" <% } %> />
											</span>
										</td>
                                        </tr>
                                        </table>
                                        <% if(obj.IsRoom == 1){ %>
                                        <table id="permZoneTreeListRoomNode_<%= obj.MyId %>" width="100%" style = '<%=string.Format("background-color:#C0C0C0; display:{0}", (Model.ActiveObjectIds != null && Model.ActiveObjectIds.Contains(obj.MyId) && Model.IsGroupExist ) ? "block" : "none") %>'>
                                        <tr id="armingDisarminRow">
                                        <td style="font-size: 10px; color:Red">&nbsp;
                                            <input type="checkbox" <% if(Model.IsCurrentUserAssignedGroup ) { %>disabled = "disabled" <% } %> <% if(obj.IsDisarming){ %>checked="checked" <% } %> onclick="javascript:ToggleDisarming('<%= obj.MyId %>');" /><%:ViewResources.SharedStrings.PermissionsDisarmingShort %>
                                            <input type="checkbox" <% if(Model.IsCurrentUserAssignedGroup ) { %>disabled = "disabled" <% } %> <% if(obj.IsDefaultDisarming){ %>checked="checked" <% } %> onclick="javascript:ToggleDefaultDisarming('<%= obj.MyId %>');" /><%:ViewResources.SharedStrings.PermissionDefaultDisarmingShort %>
                                            <input type="checkbox" <% if(Model.IsCurrentUserAssignedGroup ) { %>disabled = "disabled" <% } %> <% if(obj.IsArming){ %>checked="checked" <% } %> onclick="javascript:ToggleArming('<%= obj.MyId %>');" /><%:ViewResources.SharedStrings.PermissionsArmingShort %>
                                            <input type="checkbox" <% if(Model.IsCurrentUserAssignedGroup ) { %>disabled = "disabled" <% } %> <% if(obj.IsDefaultArming){ %>checked="checked" <% } %> onclick="javascript:ToggleDefaultArming('<%= obj.MyId %>');" /><%:ViewResources.SharedStrings.PermissionsDefaultArmingShort %>                                            
                                        </td>
                                        </tr>
                                        </table>
                                        <% } %>
                                    </li>
                                </ul>
                                <% }}}} %>
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

<script type="text/javascript" language="javascript">

    $(document).ready(function () {
        $(".tipsy_we").attr("class", function () {
            $(this).tipsy({ gravity: $.fn.tipsy.autoWE, html: true });
        });
    });

</script>