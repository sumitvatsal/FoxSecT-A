<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.TitleEditViewModel>" %>

<div id="content_edit_title_form" style='margin:10px; text-align:center;' >
<form id="editTitleTable">
<table width="100%">
<%=Html.Hidden("Title.Id", Model.Title.Id)%>
<tr>
	<td style='width:30%; padding:0 5px; text-align:right;'>
        <label for='Edit_title_companyId'><%:ViewResources.SharedStrings.TitlesCompany %></label>
    </td>
	<td style='width:70%; padding:0 5px;'>
        <%= Html.DropDownList("Title.CompanyId", Model.Companies, new { @style = "width:241px", id = "Edit_title_companyId_" + Model.Title.Id })%>
    </td>
</tr>
<tr>
    <td style='width:30%; padding:0 5px; text-align:right;'>
        <label for='Add_title_title'><%:ViewResources.SharedStrings.TitlesName %></label>
        
    </td>
    <td style='width:70%; padding:0 5px;'>
        <%=Html.TextBox("Title.Name", Model.Title.Name, new { @id = "Add_title_title", @style = "width:80%;text-transform: capitalize;" })%>
        <%= Html.ValidationMessage("Title.Name", null, new { @class = "error" })%>
    </td>
</tr>
<tr>
    <td style='width:30%; padding:0 5px; text-align:right; vertical-align:top;'>
        <label for='Add_title_description'><%:ViewResources.SharedStrings.CommonDescription %></label>
        
    </td>
    <td style='width:70%; padding:0 5px;'>
        <%=Html.TextArea("Title.Description", Model.Title.Description, new { @style = "width:80%;text-transform: capitalize;", id = "Add_title_description" })%>
        <%= Html.ValidationMessage("Title.Description", null, new { @class = "error" })%>
    </td>
</tr>
</table>
</form>
</div>