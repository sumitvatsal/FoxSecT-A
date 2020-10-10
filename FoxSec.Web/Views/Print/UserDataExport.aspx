<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<FoxSec.Web.ViewModels.UserEditViewModel>" %>
<%@ Import Namespace="FoxSec.Web.Helpers" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%: Model.FoxSecUser.FirstName %> <%: Model.FoxSecUser.LastName %></title>
    <style type="text/css">
        .xl65
        {
        	mso-style-parent:style0; 
        	mso-number-format:0; 
        	text-align:left;
        }
        
        th,td
        {
        	padding:3px;
        	border:1px solid #000;
        	word-wrap: break-word;
       	}
       	
       	table
       	{
       		border-collapse:collapse;
       	}
       	
       	.subTabHeader
       	{
       		width:100%; 
       		background-color:Gray;
       	}
       	
       	.lightGrayHeader
       	{
       		background-color:#ddd;
       	}
       	
    </style>
</head>
<body>

<%-- Personal data --%>

<table cellpadding="0" cellspacing="0" style="margin: 0; width: 100%; padding: 0px; border:0px; border-spacing: 0;">
    <thead>
		<tr>
			<th style=" text-align:left; border:0px;">
						<h2><%: ViewResources.SharedStrings.PersonalData %></h2>
			</th>
			<th style=" text-align:right; border:0px;">
						<h4><%=Html.Encode(string.Format("{0}: {1}", ViewResources.SharedStrings.PrintDate, DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"))) %></h4>
			</th>
		</tr>
	</thead>
</table>

<br />

<table style="width:100%;">
	<tr>
		<%-- Personal --%>
		<td style="vertical-align:top;">
			<h3 class="subTabHeader">
				<%: ViewResources.SharedStrings.Personal %>
			</h3>
			<table style="width:100%;">
				<% if (Model.FoxSecUser.Image.Count() > 0){ %>
					<tr>
						<td colspan="2">
							<img src='<%=ConfigurationSettings.AppSettings["domainName"]%>/User/PhotoContent?id=<%=Model.FoxSecUser.Id %>&nocache=<% Random rnd = new Random(); %><%= rnd.Next(100000, 999999) %>' border='0' alt="" />
						</td>
					</tr>
				<% } %>
				<% if (!Model.FoxSecUser.IsBuildingAdmin && !string.IsNullOrEmpty(Model.FoxSecUser.CompanyName)){ %>
					<tr>
						<th>
							<%:ViewResources.SharedStrings.UsersCompany %>:
						</th>
						<td>
							<%: Model.FoxSecUser.CompanyName %>
						</td>
					</tr>
				<% } %>
				<% if(!string.IsNullOrEmpty(Model.FoxSecUser.FirstName)){ %>
					<tr>
						<th>
							<%:ViewResources.SharedStrings.UsersFirstName %> :
						</th>
						<td>
							<%: Model.FoxSecUser.FirstName %>
						</td>
					</tr>
				<%} %>
				<% if(!string.IsNullOrEmpty(Model.FoxSecUser.LastName)){ %>
					<tr>
						<th>
							<%:ViewResources.SharedStrings.UsersLastName %> :
						</th>
						<td>
							<%: Model.FoxSecUser.LastName %>
						</td>
					</tr>
				<%} %>

				<% if(!string.IsNullOrEmpty(Model.FoxSecUser.LoginName)){ %>
					<tr>
						<th>
							<%:ViewResources.SharedStrings.UsersUserName %> :
						</th>
						<td>
							<%: Model.FoxSecUser.LoginName %>
						</td>
					</tr>
				<%} %>

	
				<tr>
					<th>
						<%:ViewResources.SharedStrings.UsersPassword %> :
					</th>
					<td>
						*****
					</td>
				</tr>
	
				<% if(!string.IsNullOrEmpty(Model.FoxSecUser.PersonalId)){ %>
					<tr>
						<th>
							<%:ViewResources.SharedStrings.UsersUserId %> :
						</th>
						<td>
							<%= Model.FoxSecUser.PersonalId %>
						</td>
					</tr>
				<%} %>

				<% if(!string.IsNullOrEmpty(Model.FoxSecUser.Email)){ %>
					<tr>
						<th>
							<%:ViewResources.SharedStrings.UsersEmail %> :
						</th>
						<td>
							<%= Model.FoxSecUser.Email %>
						</td>
					</tr>
				<%} %>

				<% if(!string.IsNullOrEmpty(Model.FoxSecUser.PersonalCode)){ %>
					<tr>
						<th>
							<%:ViewResources.SharedStrings.UsersPersonalCode %> :
						</th>
						<td>
							<%=Model.FoxSecUser.PersonalCode %>
						</td>
					</tr>
				<%} %>

				<% if(!string.IsNullOrEmpty(Model.FoxSecUser.ExternalPersonalCode)){ %>
					<tr>
						<th>
							<%:ViewResources.SharedStrings.UsersExtPersonalCode %> :
						</th>
						<td>
							<%=Model.FoxSecUser.ExternalPersonalCode %>
						</td>
					</tr>
				<%} %>

				<% if(!string.IsNullOrEmpty(Model.FoxSecUser.BirthDayStr)){ %>
					<tr>
						<th>
							<%:ViewResources.SharedStrings.UsersBirthday %> :
						</th>
						<td>
							<%= Model.FoxSecUser.BirthDayStr %>
						</td>
					</tr>
				<%} %>

				<% if(!string.IsNullOrEmpty(Model.FoxSecUser.RegistredDateStr)){ %>
					<tr>
						<th>
							<%:ViewResources.SharedStrings.UsersRegistered%> :
						</th>
						<td>
							<%= Model.FoxSecUser.RegistredDateStr%>
						</td>
					</tr>
				<%} %>


				<% if( !string.IsNullOrEmpty(Model.FoxSecUser.PIN1) ){ %>
					<tr>
						<td>
							PIN 1: ***
						</td>
						<td>
							PIN 2: ***
						</td>
					</tr>
				<%} %>
			</table>
		</td>
		<%-- Roles --%>
		<td style="vertical-align:top;">
			<h3 class="subTabHeader">
				<%: ViewResources.SharedStrings.Roles %>
			</h3>
			<table style="width:100%;">
				<tr>
					<th>
						<%:ViewResources.SharedStrings.CommonId %>
					</th>
					<th>
						<%:ViewResources.SharedStrings.UsersRoleTitle %>
					</th>
					<th>
						<%:ViewResources.SharedStrings.CommonValidationPeriod %>
					</th>
					<th>
						<%:ViewResources.SharedStrings.CommonIsAllowed %>
					</th>
				</tr>
				<% var i = 1; 
					foreach (var usrRole in Model.FoxSecUser.UserRoles) { %>
					<tr>
						<td style="text-align:center;">
							<%=Html.Encode(i++) %>
						</td>
						<td class="xl65">
							<%= usrRole.Role.Name %>
						</td>
						<td>
							<%: usrRole.ValidFrom.ToString("dd.MM.yyyy") %> - <%: usrRole.ValidTo.ToString("dd.MM.yyyy") %>
						</td>
						<td style="text-align:center;">
							<%: usrRole.IsDeleted ? "" : "+"%>
						</td>
					</tr>
				<% } %>
			</table>
		</td>
		<%-- Contact --%>
		<td  style="vertical-align:top;">
			<h3 class="subTabHeader">
				<%: ViewResources.SharedStrings.Contact %>
			</h3>
			<table style="width:100%;">
				<%if(!string.IsNullOrEmpty(Model.FoxSecUser.Residence)) {%>
					<tr>
						<th>
							<%:ViewResources.SharedStrings.UsersResidence %> :
						</th>
						<td>
							<%=Model.FoxSecUser.Residence %>
						</td>
					</tr>
				<%} %>

				<%if(!string.IsNullOrEmpty(Model.FoxSecUser.PhoneNumber)) {%>
					<tr>
						<th>
							<%:ViewResources.SharedStrings.UsersPhone %> :
						</th>
						<td>
							<%=Model.FoxSecUser.PhoneNumber %>
						</td>
					</tr>
				<%} %>
			</table>
		</td>
	</tr>
	<tr>
		<%-- Work --%>
		<td style="vertical-align:top;">
			<h3 class="subTabHeader">
				<%: ViewResources.SharedStrings.Work %>
			</h3>
			<table style="width:100%;">
				<tr>
					<td colspan="2">
						<h4 class="lightGrayHeader" style="text-align:center;">
							<%: ViewResources.SharedStrings.Buildings %>
						</h4>
						<table style="width:100%;">
							<tr>
								<th>
									<%: ViewResources.SharedStrings.CommonBuilding %>
								</th>
								<th>
									<%: ViewResources.SharedStrings.CommonFloor %>
								</th>
							</tr>
							<%foreach (var ubo in Model.FoxSecUser.UserBuildingObjects){%>
								<tr>
									<td>
										<%=Html.Encode(ubo.BuildingName) %>
									</td>
									<td>
										<%=Html.Encode(ubo.FloorName) %>
									</td>
								</tr>
							<%}%>
						</table>
						<br />
					</td>
				</tr>			

				<%if(!string.IsNullOrEmpty(ViewResources.SharedStrings.UsersDirectManager)) {%>
					<tr>
						<th>
							<%:ViewResources.SharedStrings.UsersDirectManager %> :
						</th>
						<td>
							<%:ViewResources.SharedStrings.UsersDirectManager%>
						</td>
					</tr>
				<%} %>

				<%if(!string.IsNullOrEmpty(Model.FoxSecUser.TitleName)) {%>
					<tr>
						<th>
							<%:ViewResources.SharedStrings.UsersTitle %> :
						</th>
						<td>
							<%= Model.FoxSecUser.TitleName%>
						</td>
					</tr>
				<%} %>

				<%if(!string.IsNullOrEmpty(Model.FoxSecUser.ContractNum)) {%>
					<tr>
						<th>
							<%:ViewResources.SharedStrings.UsersContractNr %> :
						</th>
						<td class="xl65">
							<%= Model.FoxSecUser.ContractNum %>
							<i>
								(<%=Model.FoxSecUser.ContractStartDateStr %> - <%=Model.FoxSecUser.ContractEndDateStr %>)
							</i>
						</td>
					</tr>
				<%} %>

				<%if(!string.IsNullOrEmpty(Model.FoxSecUser.PermitOfWorkStr)) {%>
					<tr>
						<th>
							<%:ViewResources.SharedStrings.UsersPermitOfWork%> :
						</th>
						<td>
							<%= Model.FoxSecUser.PermitOfWorkStr%>
						</td>
					</tr>
				<%} %>

				<%if(Model.FoxSecUser.WorkTime.HasValue) {%>
					<tr>
						<th>
							<%:ViewResources.SharedStrings.UsersWorkTime%> :
						</th>
						<td>
							<%: Model.FoxSecUser.WorkTime.Value ? "+" : "-" %>
						</td>
					</tr>
				<%} %>

				<tr>
					<th>
						<%:ViewResources.SharedStrings.UsersESeriviceAllowed%> :
					</th>
					<td>
						<%: Model.FoxSecUser.EServiceAllowed ? "+" : "-"%>
					</td>
				</tr>
		
				<%if(Model.FoxSecUser.TableNumber.HasValue) {%>
					<tr>
						<th>
							<%:ViewResources.SharedStrings.UsersTableNr%> :
						</th>
						<td>
							<%=Model.FoxSecUser.TableNumber %>
						</td>
					</tr>
				<%} %>
			</table>
		</td>
		<%-- Departments --%>
		<td style="vertical-align:top;">
			<h3 class="subTabHeader">
				<%: ViewResources.SharedStrings.Departments %>
			</h3>
			<table style="width:100%;">
				<tr>
					<th>
						<%: ViewResources.SharedStrings.Department %>
					</th>
					<th>
						<%: ViewResources.SharedStrings.Validation %>
					</th>
					<th>
						<%: ViewResources.SharedStrings.Manager %>
					</th>
				</tr>
				<% foreach(var dep in Model.FoxSecUser.UserDepartments){ %>
					<tr>
						<td>
							<%: dep.DepartmentName %>
						</td>
						<td>
						  	<%: dep.ValidFrom %> - <%: dep.ValidTo %> 
						</td>
						<td>
							<%: dep.Manager %>
						</td>
					</tr>
				<%} %>
			</table>
		</td>
		<%-- Other --%>
		<td style="vertical-align:top;">
			<h3 class="subTabHeader">
				<%: ViewResources.SharedStrings.Other %>
			</h3>
			<table style="width:100%;">
				<%if(Model.FoxSecUser.TableNumber.HasValue) {%>
					<tr>
						<th>
							<%:ViewResources.SharedStrings.UsersCoffeCups %> :
						</th>
						<td>
							
						</td>
					</tr>
				<%} %>
				<%if(Model.FoxSecUser.PermissionCallGuests.HasValue) {%>
					<tr>
						<th>
							<%:ViewResources.SharedStrings.UsersPermisionCallQuests %>:
						</th>
						<td>
							<%= Model.FoxSecUser.PermissionCallGuests.Value ? "+" : "-" %>
						</td>
					</tr>
				<%} %>
			</table>
		</td>
	</tr>
</table>

<br />

<h2>
	<%: ViewResources.SharedStrings.Cards %>
</h2>

<table>
	<tr>
		<th>
			<%: ViewResources.SharedStrings.CardType %>
		</th>
		<th>
			<%: ViewResources.SharedStrings.CardCode %>
		</th>
		<th>
			<%: ViewResources.SharedStrings.Validation %>
		</th>
		<th>
			<%: ViewResources.SharedStrings.Status %>
		</th>
	</tr>
	<%foreach(var userCard in Model.FoxSecUser.UserCards.Cards){ %>
		<tr>
			<td>
				<%: userCard.TypeName %>
			</td>
			<td>
				<%: userCard.FullCardCode %>
			</td>
			<td>
				<%: userCard.ValidFromStr %> - <%: userCard.ValidToStr %>
			</td>
			<td>
				<%: userCard.CardStatus %>
			</td>
		</tr>
	<%} %>
</table>


<h2>
	<%: ViewResources.SharedStrings.UserRights %>
</h2>
<table>
	<tr>
		<td>
			<% Html.RenderPartial("UserPermTree", Model.FoxSecUser.UserPermissionTree); %>
		</td>
	</tr>
</table>
<br />

</body>
</html>