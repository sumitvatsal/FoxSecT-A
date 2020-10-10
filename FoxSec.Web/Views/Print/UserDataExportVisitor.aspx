<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<FoxSec.Web.ViewModels.VisitorEditViewModel>" %>
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

				
				<% if(!string.IsNullOrEmpty(Model.FoxSecUser.PhoneNumber)){ %>
					<tr>
						<th>
							<%:ViewResources.SharedStrings.UsersPhone %> :
						</th>
						<td>
							<%= Model.FoxSecUser.PhoneNumber %>
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
			</table>
		</td>
	</tr>
</table>

<br />

<h2>
	<%: ViewResources.SharedStrings.Cards %>
</h2>




<h2>
	<%: ViewResources.SharedStrings.UserRights %>
</h2>

<br />

</body>
</html>