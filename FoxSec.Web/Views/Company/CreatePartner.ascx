<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.PartnerEditViewModel>" %>

<div id="content_add_partner" style='margin:10px; text-align:center;' >
  
  <form id="createNewPartner" action="">

<%=Html.Hidden("Partner.CompanyId", Model.Partner.CompanyId)%>

<table width="100%">

      <%=Html.Hidden("Partner.Id", Model.Partner.Id.HasValue ?  Model.Partner.Id.Value : 0)%>
      <%=Html.Hidden("Partner.Active", Model.Partner.Active)%>
      <tr>
		  <td style='width:30%; padding:0 5px; text-align:right;'><label for='Add_partner_name'>Name</label></td>
		  <td style='width:70%; padding:0 5px;'><%=Html.TextBox("Partner.Name", Model.Partner.Name, new { @style = "width:80%" })%></td>
      </tr>
      <tr>
          <td style='width:30%; padding:0 5px; text-align:right;'><label for='Add_partner_comment'>Additional info</label></td>
          <td style='width:70%; padding:0 5px;'><%=Html.TextArea("Partner.Comment", Model.Partner.Comment, new { @style = "width:80%;height:50px;" })%></td>
      </tr>
       <tr>
          <td style='width:30%; padding:0 5px; text-align:right;'><label for='Add_partner_managers'>Manager</label></td>
          <td style='width:70%; padding:0 5px;'><%=Html.DropDownList("Partner.ManagerId", Model.Managers, ViewResources.SharedStrings.DefaultDropDownValue)%></td>
      </tr>
  </table>
  </form>

</div>
