<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.TimeZonePropertiesViewModel>" %>
<table id="timeZoneTable_<%= Model.TimeZoneId %>_<%= Model.OrderInGroup %>" cellpadding="0" cellspacing="0" style="margin: 0; width: 137px; min-width: 137px; border-spacing: 0; text-align: center">
<tr>
<td><%:ViewResources.SharedStrings.CommonMondayShort %></td>
<td><%:ViewResources.SharedStrings.CommonTuesdayShort %></td>
<td><%:ViewResources.SharedStrings.CommonWednesdayShort %></td>
<td><%:ViewResources.SharedStrings.CommonThursdayShort %></td>
<td><%:ViewResources.SharedStrings.CommonFridayShort %></td>
<td><%:ViewResources.SharedStrings.CommonSaturdayShort %></td>
<td><%:ViewResources.SharedStrings.CommonSundayShort %></td>
</tr>
<tr>
<td><input type="checkbox" id="toggleZoneCheckBox1" onclick="javascript:ToggleZone(<%= Model.Id %>,1, this);" <% if(Model.IsMonday) {%>  checked='checked' <% } %> /></td>
<td><input type="checkbox" id="toggleZoneCheckBox2" onclick="javascript:ToggleZone(<%= Model.Id %>,2, this);" <% if(Model.IsTuesday) {%>  checked='checked' <% } %> /></td>
<td><input type="checkbox" id="toggleZoneCheckBox3" onclick="javascript:ToggleZone(<%= Model.Id %>,3, this);" <% if(Model.IsWednesday) {%>  checked='checked' <% } %> /></td>
<td><input type="checkbox" id="toggleZoneCheckBox4" onclick="javascript:ToggleZone(<%= Model.Id %>,4, this);" <% if(Model.IsThursday) {%>  checked='checked' <% } %> /></td>
<td><input type="checkbox" id="toggleZoneCheckBox5" onclick="javascript:ToggleZone(<%= Model.Id %>,5, this);" <% if(Model.IsFriday) {%>  checked='checked' <% } %> /></td>
<td><input type="checkbox" id="toggleZoneCheckBox6" onclick="javascript:ToggleZone(<%= Model.Id %>,6, this);" <% if(Model.IsSaturday) {%>  checked='checked' <% } %> /></td>
<td><input type="checkbox" id="toggleZoneCheckBox7" onclick="javascript:ToggleZone(<%= Model.Id %>,7, this);" <% if(Model.IsSunday) {%>  checked='checked' <% } %> /></td>
</tr>
<tr>
<td colspan="7">
<input id='timeZoneTimeFrom_<%= Model.Id %>' type='text' name='<%= Model.Id %>' class="zone_time_start" style="width:50px" value='<%= Model.ValidFromStr %>' />-<input id='timeZoneTimeTo_<%= Model.Id %>' type='text' name='<%= Model.Id %>' class="zone_time_end" style="width:50px" value='<%= Model.ValidToStr %>' />
</td>
</tr>
</table>