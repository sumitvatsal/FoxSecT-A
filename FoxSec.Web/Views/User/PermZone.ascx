<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.TimeZonePropertiesViewModel>" %>
<table id='userPermissionTimeZoneTable_<%= Model.TimeZoneId %>_<%= Model.OrderInGroup %>' cellpadding="0" cellspacing="0" style="margin: 0; width: 130px; min-width: 130px; border-spacing: 0; text-align: center">
<tr>
<td>Mo</td><td>Tu</td><td>We</td><td>Th</td><td>Fr</td><td>Sa</td><td>Su</td>
</tr>
<tr>
<td><% if(Model.IsMonday) {%><span class='ui-icon ui-icon-check' style='margin:0 0 0 0'></span><% } %></td>
<td><% if(Model.IsTuesday) {%><span class='ui-icon ui-icon-check' style='margin:0 0 0 0'></span><% } %></td>
<td><% if(Model.IsWednesday) {%><span class='ui-icon ui-icon-check' style='margin:0 0 0 0'></span><% } %></td>
<td><% if(Model.IsThursday) {%><span class='ui-icon ui-icon-check' style='margin:0 0 0 0'></span><% } %></td>
<td><% if(Model.IsFriday) {%><span class='ui-icon ui-icon-check' style='margin:0 0 0 0'></span><% } %></td>
<td><% if(Model.IsSaturday) {%><span class='ui-icon ui-icon-check' style='margin:0 0 0 0'></span><% } %></td>
<td><% if(Model.IsSunday) {%><span class='ui-icon ui-icon-check' style='margin:0 0 0 0'></span><% } %></td>
</tr>
<tr>
<td colspan="7"><span id='userPermissionTimeZoneTime_<%= Model.Id %>_<%= Model.ValidFromStr %>'><%= Model.ValidFromStr %></span> - <%= Model.ValidToStr %></td>
</tr>
</table>