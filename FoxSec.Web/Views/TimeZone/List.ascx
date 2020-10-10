<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.TimeZoneListViewModel>" %>
<table cellpadding="0" cellspacing="1" style="margin: 0; width: 100%; padding: 0; border-spacing: 0px; font-size: 8px">
<tr>
<th style="width: 18%"></th>
<th style="width: 18%; text-align: center"><b style = "color:#DF6020"><%:ViewResources.SharedStrings.CommonAllow %> &nbsp; 1</b></th>
<th style="width: 18%; text-align: center"><b style = "color:#DF6020"><%:ViewResources.SharedStrings.CommonAllow %> &nbsp; 2</b></th>
<th style="width: 18%; text-align: center"><b style = "color:#DF6020"><%:ViewResources.SharedStrings.CommonAllow %> &nbsp; 3</b></th>
<th style="width: 18%; text-align: center"><b style = "color:#DF6020"><%:ViewResources.SharedStrings.CommonAllow %> &nbsp; 4</b></th>
<th style="width: 5%; text-align: center"><b style = "color:#DF6020"><%:ViewResources.SharedStrings.FilterActive %></b></th>
<th style="width: 5%"></th>
</tr>
<tr>
<td colspan="7"><hr /></td>
</tr>
<% int i = 1; foreach (var zone in Model.TimeZones) { var bg = (i % 2 == 1) ? "style='background-color:#CCC;'" : ""; %>
<tr id="timeZone_<%= zone.Id %>" <%= bg %>>
<td style="width: 18%"><%=zone.TimeZoneId%>. <%= Html.Encode(zone.Name != null && zone.Name.Length > 30 ? zone.Name.Substring(0, 27) + "..." : zone.Name)%> <%if(zone.CompanyId != 0){%>(Company Special) <%}%> <%if ((Model.User.IsBuildingAdmin || Model.User.IsSuperAdmin) && (zone.CompanyId != 0)){%><%= Html.Encode(zone.CompanyId) %> <%}%>
</td>
<td align="center" style="width: 18%"><% Html.RenderAction("Zone", "TimeZone", new { @timeZoneId = zone.Id, @colNr = 0 }); %></td>
<td align="center" style="width: 18%"><% Html.RenderAction("Zone", "TimeZone", new { @timeZoneId = zone.Id, @colNr = 1 }); %></td>
<td align="center" style="width: 18%"><% Html.RenderAction("Zone", "TimeZone", new { @timeZoneId = zone.Id, @colNr = 2 }); %></td>
<td align="center" style="width: 18%"><% Html.RenderAction("Zone", "TimeZone", new { @timeZoneId = zone.Id, @colNr = 3 }); %></td>

<td style="width: 5%">
<% if (zone.IsInUse) { %><span class='ui-icon ui-icon-check' style='margin:0 3px 0 18px'></span><% } %>
</td>
<td style="width: 5%">
<table>
<tr>
<td>
<% if (!zone.IsInUse && zone.CompanyId == Model.User.CompanyId)
   { %>
<span id='time_zone_delete_<%= zone.Id %>' class="ui-icon ui-icon-circle-close tipsy_we" original-title='<%=string.Format("{0} {1}!", ViewResources.SharedStrings.BtnDelete, Html.Encode(zone.Name)) %>' style="cursor:pointer" onclick='<%=string.Format("javascript:RemoveTimeZone({0}, \"{1}\")", zone.Id, Html.Encode(zone.Name)) %>'></span>
<% } %>

</td>
<td>
<% if ((!Model.User.IsCompanyManager) && (!Model.User.IsCommonUser)) {%>
<span id='button_time_zonr_edit_<%= zone.Id %>' class='icon icon_green_go tipsy_we' original-title='<%=string.Format("{0} {1}!", ViewResources.SharedStrings.BtnEdit, Html.Encode(zone.Name)) %>' onclick='<%=string.Format("javascript:EditTimeZone( {0}, \"{1}\")", zone.Id, Html.Encode(zone.Name)) %>' ></span>
<% } %>
</td>
</tr>
</table>
</td>
</tr>
<tr>
<td colspan="7"><hr /></td>
</tr>
<% i++; } %>
<tfoot>
        <tr>
            <td colspan="5">
                <% Html.RenderPartial("Paginator", Model.Paginator); %>
            </td>
        </tr>
    </tfoot>
</table>


<script type="text/javascript" language="javascript">

    $(document).ready(function () {
        $(".tipsy_we").attr("class", function () {
            $(this).tipsy({ gravity: $.fn.tipsy.autoWE, html: true });
        });
        $("input[id^=timeZoneTimeFrom_][type='text']").each(function () {
            $(this).focusout(function () {
                var id = $(this).attr('id');
                $.ajax({
                    type: "Post",
                    url: "/TimeZone/TimeFromChange",
                    dataType: 'json',
                    data: { id: $(this).attr('name'), time: $(this).val() },
                    success: function (data) {
                        if (data == 'not') $("input#" + id).css('color', 'red');
                        if (data == 'ok') $("input#" + id).css('color', 'green');
                    }
                });
            });
        });
        $("input[id^=timeZoneTimeTo_][type='text']").each(function () {
            $(this).focusout(function () {
                var id = $(this).attr('id');
                $.ajax({
                    type: "Post",
                    url: "/TimeZone/TimeToChange",
                    dataType: 'json',
                    data: { id: $(this).attr('name'), time: $(this).val() },
                    success: function (data) {
                        if (data == 'not') $("input#" + id).css('color', 'red');
                        if (data == 'ok') $("input#" + id).css('color', 'green');
                    }
                });
            });
        });
        ColorSearch("<%= Model.SearchStartTime %>");
    });

    function ToggleZone(Id, dayNr, cntr) {
        parent_td = $(cntr).parent();
        parent_row = parent_td.parent();
        any = 0;
        parent_row.find('input[id*=toggleZoneCheckBox]').each(function () {
            if (this.checked) any++;
        });

        if (any == 0) {
            ShowDialog('<%=ViewResources.SharedStrings.TimeZonesNoCheckedError %>', 2000);
            $(cntr).attr("checked", true);
        }
        else {
            $.ajax({
                type: "Post",
                url: "/TimeZone/ToggleZone",
                dataType: 'json',
                data: { id: Id, day: dayNr, CreateNew: false },
                success: function (data) {
                    if (data != null) {
                        $("div#modal-dialog").dialog({
                            open: function (event, ui) {
                                $("div#modal-dialog").html('<%=ViewResources.SharedStrings.CommonConfirmMessage %>');
                            },
                            resizable: false,
                            width: 240,
                            height: 140,
                            modal: true,
                            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.TimeZoneAddCompanyTimeZone %>' + ' ',
                            buttons: {
                                '<%=ViewResources.SharedStrings.BtnOk %>': function () {
                                    parent_td = $(cntr).parent();
                                    parent_row = parent_td.parent();
                                    any = 0;
                                    parent_row.find('input[id*=toggleZoneCheckBox]').each(function () {
                                        if (this.checked) any++;
                                    });

                                    if (any == 0) {
                                        ShowDialog('<%=ViewResources.SharedStrings.TimeZonesNoCheckedError %>', 2000);
                                        $(cntr).attr("checked", true);
                                    }
                                    else {
                                        $.post('/TimeZone/ToggleZone', { id: Id, day: dayNr, CreateNew: true }, function (html) { });
                                    }
                                    $(this).dialog("close");
                                    setTimeout(function () { SubmitTimeZoneSearch(); }, 500);
                                },
                                '<%=ViewResources.SharedStrings.BtnCancel %>': function () {
                                    $(this).dialog("close");
                                }
                            }
                        });
                    }
                }
            });
          // $.post('/TimeZone/ToggleZone', { id: Id, day: dayNr, CreateNew: true}, function (html) { });
        }

        //document.getElementById('TimeZoneList').style.visibility = 'visible';
        //setTimeout(function () { SubmitTimeZoneSearch(); }, 1000);
        /*
        $("div#modal-dialog").dialog({
            open: function (event, ui) {
                $("div#modal-dialog").html('<%=ViewResources.SharedStrings.CommonConfirmMessage %>');
            },
            resizable: false,
            width: 240,
            height: 140,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.TimeZinesDeleteTitle %>' + ' ',
            buttons: {
                '<%=ViewResources.SharedStrings.BtnOk %>': function () {
                    parent_td = $(cntr).parent();
                    parent_row = parent_td.parent();
                    any = 0;
                    parent_row.find('input[id*=toggleZoneCheckBox]').each(function () {
                        if (this.checked) any++;
                    });

                    if (any == 0) {
                        ShowDialog('<%=ViewResources.SharedStrings.TimeZonesNoCheckedError %>', 2000);
                        $(cntr).attr("checked", true);
                    }
                    else {
                        $.post('/TimeZone/ToggleZone', { id: Id, day: dayNr, CreateNew: true }, function (html) { });
                    }

                    setTimeout(function () { SubmitTimeZoneSearch(); }, 1000);
                    $(this).dialog("close");


                },
                '<%=ViewResources.SharedStrings.BtnCancel %>': function () {
                    $(this).dialog("close");
                }
            }
        });
        //document.getElementById('TimeZoneList').style.visibility = 'hidden';
      
        */
    }

    function ColorSearch(from) {
        if (from == "") return false;
        $("input[id^=timeZoneTimeFrom_][type='text']").each(function () {
            if ($(this).val() == from) { $(this).css('color', '#BF4000'); }
        });
        return false;
    }

</script>