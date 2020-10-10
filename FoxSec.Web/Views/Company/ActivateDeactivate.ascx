<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.DeactivateCardViewModel>" %>

<table cellpadding="1" cellspacing="0" style="margin: 0; width: 100%; padding: 1px; border-spacing: 0">
    <tr>
		<td style='width:40%; padding:2px; text-align:right;'><label for='TypeId'><%:ViewResources.SharedStrings.CommonReason %>:</label></td>
		<td style='width:60%; padding:2px;'><%=Html.DropDownList("CardChangeStausReaon", Model.Reasons, ViewResources.SharedStrings.DefaultDropDownValue, new {@id = "selectedReasonId"}) %></td>
    </tr>
</table>