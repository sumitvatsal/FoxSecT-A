<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.DeactivateCardViewModel>" %>

<table cellpadding="1" cellspacing="0" style="margin: 0; width: 100%; padding: 1px; border-spacing: 0">
    <tr>
		<td style='width:40%; padding:2px; text-align:right;'><label for='TypeId'><%:ViewResources.SharedStrings.CommonReason %>:</label></td>
		<td style='width:60%; padding:2px;'>

<select id="CardChangeStausReaon">
  <option value ="0">-</option>
  <option value ="1">Change Card company</option>
  <option value ="2">Move to Free cards</option>  
</select>
		</td>
    </tr>
    
</table>