<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.CompanyTreeViewModel>" %>

<ul id="companies" class="treeview-red">
<% foreach (var country in Model.Countries) {%>
    <li>
        &nbsp;<a onclick="UserByCountry(<%= country.MyId %>)"><%= Html.Encode(country.Name)%></a>
        <% foreach (var town in Model.Towns) { if (town.ParentId == country.MyId) { %>
        <ul>
            <li>
                &nbsp;<a onclick="UserByLocation(<%= town.MyId %>)"><%= Html.Encode(town.Name)%> </a>
                <% foreach (var office in Model.Offices) { if (office.ParentId == town.MyId) { %>
                <ul>
                    <li>
                        &nbsp;<a onclick="UserByBuilding(<%= office.MyId %>)"><%= Html.Encode(office.Name)%></a>
                        <% foreach (var company in Model.Companies) { if (company.ParentId == office.MyId) { %>
                        <ul>
                            <li>
                                &nbsp;<a onclick="UserByCompany(<%= company.MyId %>, <%= office.MyId %>)"><%= Html.Encode(company.Name)%></a>
                                <% foreach (var partner in Model.Partners) { if (partner.ParentId == company.MyId){ %>
                                    <ul>
                                        <li>
                                            &nbsp;<a onclick="UserByPartner(<%= partner.MyId %>, <%= office.MyId %>)"><%= Html.Encode(partner.Name)%></a>
											<% foreach (var floor in Model.Floors) { if (floor.ParentId == partner.MyId && floor.BuildingId == office.MyId){ %>
												<ul>
													<li>
														&nbsp;<a onclick="UserByFloor(<%= floor.MyId %>,<%=company.MyId %> )" ><%= Html.Encode(string.Format("{0}", floor.Name))%></a>
													</li>
												</ul>
											<%}}%>
										</li>
                                    </ul>
                                <% }} %>
								<% foreach (var floor in Model.Floors) { if (floor.ParentId == company.MyId && floor.BuildingId == office.MyId){ %>
									<ul>
										<li>
											&nbsp;<a onclick="UserByFloor(<%= floor.MyId %>,<%=company.MyId %>)"><%= Html.Encode(string.Format("{0}",floor.Name))%></a>
										</li>
									</ul>
                                <%}}%>
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