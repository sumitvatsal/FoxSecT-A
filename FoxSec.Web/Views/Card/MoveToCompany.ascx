<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.MoveToCompanyViewModel>" %>
<div id='move_card_to_company_content'>
<table cellpadding="3" cellspacing="3" style="margin:0; width:100%; padding:3px; border-spacing:3px;">
<tr>
<td style='width:30%; padding:2px;'><label for='edit_user_company'><%:ViewResources.SharedStrings.CardsSelectCompany %></label></td>
<td style='width:70%; padding:2px;'><%= Html.DropDownList("MoveToCompanyId", Model.Companies, new { style="width:90%" })%></td>
</tr>
</table>
</div>