<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.TimeZoneListViewModel>" %>
<table cellpadding="0" cellspacing="0" style="margin: 0; width: 100%; padding: 0; border-spacing: 0; font-size: 8px">
<tr>
<th style="width: 16%"></th>
<th style="width: 21%; text-align: center"><b style = "color:#DF6020"><%:ViewResources.SharedStrings.CommonAllow %> &nbsp; 1</b></th>
<th style="width: 21%; text-align: center"><b style = "color:#DF6020"><%:ViewResources.SharedStrings.CommonAllow %> &nbsp; 2</b></th>
<th style="width: 21%; text-align: center"><b style = "color:#DF6020"><%:ViewResources.SharedStrings.CommonAllow %> &nbsp; 3</b></th>
<th style="width: 21%; text-align: center"><b style = "color:#DF6020"><%:ViewResources.SharedStrings.CommonAllow %> &nbsp; 4</b></th>
</tr>
<tr>
<td colspan="5"><hr /></td>
</tr>
<% int i = 1; 
foreach (var zone in Model.TimeZones) { %>
<tr id='permissionTimeZoneTableRow_<%= zone.Id %>'>
<td style="width: 16%"><input type="checkbox" id='permissionTimeZoneCheck_<%= zone.Id %>' <% if(Model.IsModelReadOnly) { %>disabled = "Enabled" <% } %>  name='<%= zone.Id %>' onclick="javascript:TogglePermissionTimeZone(<%= zone.Id %>);" /> <%= Html.Encode(zone.Name)%><%if(zone.CompanyId != 0){%>(Company Special) <%}%> <%if ((Model.User.IsBuildingAdmin || Model.User.IsSuperAdmin) && (zone.CompanyId != 0)){%><%= Html.Encode(zone.CompanyId) %> <%}%></td>
<td align="center" style="width: 21%"><% Html.RenderAction("Zone", "Permission", new { @timeZoneId = zone.Id, @colNr = 0 }); %></td>
<td align="center" style="width: 21%"><% Html.RenderAction("Zone", "Permission", new { @timeZoneId = zone.Id, @colNr = 1 }); %></td>
<td align="center" style="width: 21%"><% Html.RenderAction("Zone", "Permission", new { @timeZoneId = zone.Id, @colNr = 2 }); %></td>
<td align="center" style="width: 21%"><% Html.RenderAction("Zone", "Permission", new { @timeZoneId = zone.Id, @colNr = 3 }); %></td>
</tr>
<tr>
<td colspan="5"><hr /></td>
</tr>
<% i++; } %>
</table>

<script type="text/javascript" language="javascript">

    $(document).ready(function () {
        ColorSearch("<%= Model.SearchStartTime %>");
    });

    function ColorSearch(from) {
        if (from == "") return false;
        $("span[id^=permissionTimeZoneTime_]").each(function () {
            if ($(this).html() == from) { $(this).css('color', '#BF4000'); }
        });
        return false;
    }

    function TogglePermissionTimeZone(id) {
        if ($("input#permissionTimeZoneCheck_" + id).is(':checked') == true) {
        	if ($("input#CurrentPermissionGroupId").val() != "") {
        		//$("input#button_change_object_time_zone").fadeIn();
        	}
            $('tr[id^=permissionTimeZoneTableRow_]').each(function () {
                $(this).css("background-color", "#FFF");
            });
            $('input[id^=permissionTimeZoneCheck_]').each(function () {
                if ($(this).attr('id') != "permissionTimeZoneCheck_" + id) {
                    $(this).removeAttr('checked');
                }
            });
            $("tr#permissionTimeZoneTableRow_" + id).css("background-color", "#DDD");
        } else {
           // $("input#button_change_object_time_zone").fadeOut();
            $("tr#permissionTimeZoneTableRow_" + id).css("background-color", "#FFF");
        }
        return false;
    }

</script>