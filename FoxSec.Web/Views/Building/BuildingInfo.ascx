<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.BuildingViewModel>" %>
<table cellpadding="1" cellspacing="0" style="margin: 0; width: 100%; padding: 1px; border-spacing: 0;">
	<thead>
		<tr>
			<th style='width:20%; padding:2px;'><%:ViewResources.SharedStrings.UsersName %></th>
			<th style='width: <%= (Model.User.IsCompanyManager || Model.User.IsSuperAdmin) ? "40%" : "80%" %>; padding: 2px;'><%--<%:ViewResources.SharedStrings.BuildingsAddress %> --%>
			</th>
			<%if(Model.User.IsCompanyManager || Model.User.IsSuperAdmin){%>
				<th style='width:40%; padding:2px;'><%:ViewResources.SharedStrings.BuildingsAdministrator%>
				</th>
			<%} %>
		</tr>
	</thead>
	<tbody>
		<tr>
			<td style='width:20%; padding:2px;'>
				<%= Html.Encode(Model.BuildingName)%> 
			</td>
			<%--<td style='width: <%= Model.User.IsCompanyManager ? "55%" : "80%" %>; padding: 2px;'>
				<%= Html.Encode(Model.Address)%> 
			</td>--%>
			<%if(Model.User.IsCompanyManager || Model.User.IsSuperAdmin){%>
				<td style='width:40%; padding:2px;'>
					<%= Html.Encode(Model.AdminName) %>
				</td>
			<%} %>
		</tr>
	</tbody>
</table>

