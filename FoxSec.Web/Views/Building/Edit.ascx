<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.BuildingObjectEditViewModel>" %>
<div id="content_edit_building" style='margin:10px; text-align:center; display:none' >
<form id="editBuilding" action="">
<table width="100%">
    <tr>
		<td style='width:30%; padding:0 5px; text-align:right;'><label for='Add_company_title'><%: ViewResources.SharedStrings.CommonComment %></label></td>
		<td style='width:70%; padding:0 5px;'><%=Html.TextArea("BuildingObject.Comment", Model.BuildingObject.Comment, new { @style = "width:80%" })%>
		<%= Html.ValidationMessage("BuildingObject.Comment", null, new { @class = "error" })%>
		</td>
		<%:Html.Hidden("BuildingObject.Id", Model.BuildingObject.Id) %>
    </tr>
</table>
    
</form>
</div>

<script type="text/javascript" language="javascript">
	$(document).ready(function () {
		$("div#content_edit_building").fadeIn("300");
	});
</script>