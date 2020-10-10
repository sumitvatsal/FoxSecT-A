<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.CompanyTreeViewModel>" %>

<ul id="companies" class="treeview-red">
<% foreach (var country in Model.Countries) {%>
    <li>
        &nbsp;<a onclick="CompanyByCountry(<%= country.MyId %>)"><%= Html.Encode(country.Name)%></a>

        <% foreach (var town in Model.Towns) { if (town.ParentId == country.MyId) { %>
        <ul>
            <li>
                &nbsp;<a onclick="CompanyByLocation(<%= town.MyId %>)"><%= Html.Encode(town.Name)%> </a>

                <% foreach (var office in Model.Offices) { if (office.ParentId == town.MyId) { %>
                <ul>
                    <li>
                        &nbsp;<a onclick="CompanyByBuilding(<%= office.MyId %>)"><%= Html.Encode(office.Name)%></a>

                        <% foreach (var company in Model.Companies) { if (company.ParentId == office.MyId) { %>
                        <ul>
                            <li>
                                &nbsp;<span><%= Html.Encode(company.Name)%></span>
									<% foreach (var partner in Model.Partners) { if (partner.ParentId == company.MyId) { %>
									<ul>
										<li>
											&nbsp;<span><%= Html.Encode(partner.Name)%></span>
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