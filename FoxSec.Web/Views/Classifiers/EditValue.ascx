<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.ClassificatorValueEditViewModel>" %>
<div id="content_classificator_form" style='margin: 10px; text-align: center;'>
  <form id="editClassificatorValue">
  <table width="100%">
      <%=Html.Hidden("ClassificatorValue.Id", Model.ClassificatorValue.Id)%>
      <%=Html.Hidden("ClassificatorValue.ClassificatorId", Model.ClassificatorValue.ClassificatorId)%>
      <tr>
		  <td style='width:30%; padding:0 5px; text-align:right;'><label for='Add_classificator_value'><%:ViewResources.SharedStrings.CommonValue %></label></td>
      <td style='width:70%; padding:0 5px;'>
		<%=Html.TextBox("ClassificatorValue.Value", Model.ClassificatorValue.Value, new { @style = "width:80%", id = "Add_classificator_value" })%>
		<%= Html.ValidationMessage("ClassificatorValue.Value", null, new { @class = "error" })%>
      </td>
      </tr>
  </table>
  </form>
</div>
