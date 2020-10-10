<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.UserEditViewModel>" %>
<%@ Import Namespace="FoxSec.Web.Helpers" %>

<table cellpadding="1" cellspacing="0" style="margin: 0; width: 100%; padding: 1px; border-spacing: 0">
    <tr>
		<td style='width:40%; padding:2px; text-align:right;'><label for='TypeId'><%:ViewResources.SharedStrings.Buildingobject %>:</label></td>
		<td style='width:60%; padding:2px;'>
            <%=Html.DropDownList("UserBuildingObjectsItems",new SelectList(Model.FoxSecUser.UserBuildingObjectsItems, "Id", "BuildingObjectName"), new { @style = "width:120px" })%>
		</td>
    </tr>    
</table>