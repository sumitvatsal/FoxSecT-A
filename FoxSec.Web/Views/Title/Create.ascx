<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.TitleEditViewModel>" %>
<div id="content_add_title_form" style='margin:10px; text-align:center;' >
<form id="createNewTitle">
<table width="100%">
    <tr>
		<td style='width:30%; padding:0 5px; text-align:right;'><label for='Add_title_companyId'><%:ViewResources.SharedStrings.TitlesCompany %></label></td>
		<td style='width:70%; padding:0 5px;'><%= Html.DropDownList("Title.CompanyId", Model.Companies as SelectList,"-Select Company-", new { @style = "width:241px", id = "Add_title_companyId", })%></td>
    </tr>
    <tr>
		<td style='width:30%; padding:0 5px; text-align:right;'><label for='Add_title_title'><%:ViewResources.SharedStrings.TitlesName %></label></td>
		<td style='width:70%; padding:0 5px;'>
			<%=Html.TextBox("Title.Name", Model.Title.Name, new { @style = "width:80%;text-transform: capitalize", id = "Add_title_title" })%>
			<%= Html.ValidationMessage("Title.Name", null, new { @class = "error" })%>
		</td>
    </tr>
    <tr>
		<td style='width:30%; padding:0 5px; text-align:right; vertical-align:top;'><label for='Add_title_description'><%:ViewResources.SharedStrings.CommonDescription %></label></td>
		<td style='width:70%; padding:0 5px;'>
			<%=Html.TextArea("Title.Description", Model.Title.Description, new { @style = "width:80%;text-transform: capitalize", id = "Add_title_description" })%>
			<%= Html.ValidationMessage("Title.Description", null, new { @class = "error" })%>
		</td>
    </tr>
</table>
</form>
</div>