<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.TimeZoneListViewModel>" %>
<table id="UPTZTable" cellpadding="0" cellspacing="0" style="margin: 0; width: 100%; padding: 0; border-spacing: 0; font-size: 8px">
<tr>
<th style="width: 16%"></th>
<th style="width: 21%; text-align: center"><b style = "color:#DF6020"><%:ViewResources.SharedStrings.CommonAllow %> 1</b></th>
<th style="width: 21%; text-align: center"><b style = "color:#DF6020"><%:ViewResources.SharedStrings.CommonAllow %> 2</b></th>
<th style="width: 21%; text-align: center"><b style = "color:#DF6020"><%:ViewResources.SharedStrings.CommonAllow %> 3</b></th>
<th style="width: 21%; text-align: center"><b style = "color:#DF6020"><%:ViewResources.SharedStrings.CommonAllow %> 4</b></th>
</tr>
<tr>
<td colspan="5"><hr /></td>
</tr>
<% int i = 1; foreach (var zone in Model.TimeZones) { %>
<tr id='userPermissionTimeZoneTableRow_<%= zone.Id %>'>
<td style="width: 16%"><%= Html.Encode(zone.Name) %></td>
<td align="center" style="width: 21%"><% Html.RenderAction("PermZone", "User", new { @timeZoneId = zone.Id, @colNr = 0 }); %></td>
<td align="center" style="width: 21%"><% Html.RenderAction("PermZone", "User", new { @timeZoneId = zone.Id, @colNr = 1 }); %></td>
<td align="center" style="width: 21%"><% Html.RenderAction("PermZone", "User", new { @timeZoneId = zone.Id, @colNr = 2 }); %></td>
<td align="center" style="width: 21%"><% Html.RenderAction("PermZone", "User", new { @timeZoneId = zone.Id, @colNr = 3 }); %></td>
</tr>
<tr>
<td colspan="5"><hr /></td>
</tr>
<% i++; } %>
</table>

<script type="text/javascript" language="javascript">

    $(document).ready(function () {
        ColorUserSearch("<%= Model.SearchStartTime %>");
    });

    function ColorUserSearch(from) {
        if (from == "") return false;
        $("span[id^=userPermissionTimeZoneTime_]").each(function () {
            if ($(this).html() == from) { $(this).css('color', '#BF4000'); }
        });
        return false;
    }

    function ToggleUserPermissionTimeZone(id) {
        if ($("input#userPermissionTimeZoneCheck_" + id).is(':checked') == true) {
            $('tr[id^=userPermissionTimeZoneTableRow_]').each(function () {
                $(this).css("background-color", "#FFF");
            });
            $('input[id^=userPermissionTimeZoneCheck_]').each(function () {
                if ($(this).attr('id') != "userPermissionTimeZoneCheck_" + id) {
                    $(this).removeAttr('checked');
                }
            });
            $("tr#userPermissionTimeZoneTableRow_" + id).css("background-color", "#DDD");
        } else {
            $("tr#userPermissionTimeZoneTableRow_" + id).css("background-color", "#FFF");
        }
        return false;
    }

</script>