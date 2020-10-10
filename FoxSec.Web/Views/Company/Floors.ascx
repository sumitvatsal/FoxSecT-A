<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.CompanyBuildingItem>" %>
<% int floorIndex=0; foreach (var floor in Model.CompanyFloors){%>

<%: Html.Hidden(string.Format("Company.CompanyBuildingItems[{0}].CompanyFloors[{1}].Id", Model.Index, floorIndex), floor.Id) %>
<%: Html.Hidden(string.Format("Company.CompanyBuildingItems[{0}].CompanyFloors[{1}].CompanyId", Model.Index, floorIndex), floor.CompanyId)%>
<%: Html.Hidden(string.Format("Company.CompanyBuildingItems[{0}].CompanyFloors[{1}].IsAvailable", Model.Index, floorIndex), floor.IsAvailable)%>
<%: Html.Hidden(string.Format("Company.CompanyBuildingItems[{0}].CompanyFloors[{1}].BuildingObjectId", Model.Index, floorIndex), floor.BuildingObjectId)%>
<% if(floor.IsAvailable ){ %>
<%: Html.CheckBox(string.Format("Company.CompanyBuildingItems[{0}].CompanyFloors[{1}].IsSelected", Model.Index, floorIndex), floor.IsSelected)%>
<%}else{%>
	<%: Html.Hidden(string.Format("Company.CompanyBuildingItems[{0}].CompanyFloors[{1}].IsSelected", Model.Index, floorIndex), floor.IsSelected)%>
	<%: Html.CheckBox("selFloor", floor.IsSelected, new { disabled="disabled"}) %>
<%} %>
&nbsp;<%:ViewResources.SharedStrings.CommonFloor %>&nbsp;<%= Html.Encode(floorIndex+1) %>
<br />
<% floorIndex++;} %>