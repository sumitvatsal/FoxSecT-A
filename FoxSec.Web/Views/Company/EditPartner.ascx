<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.PartnerEditViewModel>" %>

<div id="content_edit_partner" style='margin:10px; text-align:center;' >
  
  <form id="editPartner" action="">

<%=Html.Hidden("Partner.CompanyId", Model.Partner.CompanyId)%>
<%=Html.Hidden("Partner.ParentId", Model.Partner.ParentId)%>

<table width="100%">

      <%=Html.Hidden("Partner.Id", Model.Partner.Id.HasValue ?  Model.Partner.Id.Value : 0)%>
      <tr>
		  <td style='width:30%; padding:0 5px; text-align:right;'><label for='Add_partner_name'><%:ViewResources.SharedStrings.UsersName %></label></td>
		  <td style='width:70%; padding:0 5px;'><%=Html.TextBox("Partner.Name", Model.Partner.Name, new { @style = "width:80%" })%></td>
      </tr>
      <tr>
          <td style='width:30%; padding:0 5px; text-align:right;'><label for='Add_partner_comment'><%:ViewResources.SharedStrings.CompaniesAdditionalInfo %></label></td>
          <td style='width:70%; padding:0 5px;'><%=Html.TextArea("Partner.Comment", Model.Partner.Comment, new { @style = "width:80%;height:50px;" })%></td>
      </tr>
       <tr>
          <td style='width:30%; padding:0 5px; text-align:right;'><label for='Add_partner_managers'><%:ViewResources.SharedStrings.DepartmentsManager %></label></td>
          <td style='width:70%; padding:0 5px;'><%=Html.DropDownList("Partner.ManagerId", Model.Managers, ViewResources.SharedStrings.DefaultDropDownValue)%></td>
      </tr>
       <tr>
          <td style='width:30%; padding:0 5px; text-align:right;'><label for='Add_partner_managers'><%:ViewResources.SharedStrings.FilterActive %></label></td>
          <td style='width:70%; padding:0 5px;'><%=Html.CheckBox("Partner.Active", Model.Partner.Active)%></td>
      </tr>
  </table>
  </form>

</div>