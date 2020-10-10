<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.CompanyEditViewModel>" %>

<table cellpadding="3" cellspacing="3" style="margin: 0; width: 100%; padding: 3px; border-spacing: 3px;" class="TFtable">
			<tr id="companylistRow">				
				<td  style="width:20%">
					<input type="checkbox" name="chkcomp" checked="checked" value="1" />
				</td>				
				<td>
					<span></span>
				</td>
			</tr>
		</table>
<%int index = 0; foreach (var cb in Model.CompanyItems){ %>
	<% Html.RenderPartial("Companies", cb); %>
<%index++;} %>


