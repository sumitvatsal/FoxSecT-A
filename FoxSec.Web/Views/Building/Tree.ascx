`<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.PermissionTreeViewModel>" %>

<ul id="buildings" class="treeview-red">
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
						<% foreach (var floor in Model.Floors) { if (floor.ParentId == building.MyId) { %>
                        <ul>
                            <li>
								<span <% if(!string.IsNullOrWhiteSpace(floor.Comment)){ %>class = "tipsy_we" original-title="<%= floor.Comment %>" <% } %>>
									&nbsp;<a onclick="javascript:EditBuilding(this,'submit_edit_building',<%= floor.MyId %>, '<%= floor.Name %>');" style="color:<%= !string.IsNullOrEmpty(floor.Comment) ? "Green" : "#222222"%>" ><%= Html.Encode(floor.Name)%></a> &nbsp;
								</span>
                                <% foreach (var obj in Model.Objects) { if (obj.ParentId == floor.MyId){ %>
                                 <ul>
                                    <li>
                                        <span <% if(!string.IsNullOrWhiteSpace(obj.Comment)){ %> class = "tipsy_we" original-title="<%= obj.Comment %>" <% } %>>
											&nbsp;<a onclick="javascript:EditBuilding(this,'submit_edit_building',<%= obj.MyId %>, '<%= obj.Name %>');" style="color:<%= !string.IsNullOrEmpty(obj.Comment) ? "Green" : "#222222"%>" ><%= Html.Encode(obj.Name)%></a> &nbsp;
										</span>
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

		GetCommentsCount();
	});

</script>