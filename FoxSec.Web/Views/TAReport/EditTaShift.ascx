<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.TaShiftsModel>" %>

<%=Html.Hidden("taShiftId",Model.TAShift.Id,new { @id = "taShiftId" }) %>
<div>
    <table>
        <tbody>
            <tr>
                <td><label>Shift Name</label></td>
                <td><input type="text" id="shiftName" value="<%=Model.TAShift.Name %>" /></td>
            </tr>
            <tr>
                <td><label>Start From</label></td>
                <td><input type="date" id="startFrom" value="<%=Model.TAShift.StartFrom.Value.Date.ToString("yyyy-MM-dd") %>"/></td>
            </tr>
       
            <tr>
                <td><label>Finish at</label></td>
                <td><input type="date" id="finishAt" value="<%=Model.TAShift.FinishAt.Value.Date.ToString("yyyy-MM-dd") %>" /></td>       
            </tr>
        </tbody>
    </table>
    <div>
    <fieldset>
        <legend>WorkTime</legend>
        <table style="width:100%">
            <tbody>
                <tr>
                   <td style="width:10px"><label>Break</label></td>
                   <td style="padding-right:10px;width:0.1px"><input type="text" id="breakTime" style="width:30px" <%if (Model.TAShift.DuratOfBreak.HasValue)
                                                                  { %> value="<%=Model.TAShift.DuratOfBreak.Value  %><%} %>"/>min</td>
                    <td style="width:10px"><label>Late allowed</label></td>
                    <td style="padding-right:10px;width:0.1px"><input type="text" id="lateAllowed" style="width:30px" <%if (Model.TAShift.LateAllowed.HasValue)
                                                                   { %> value="<%=Model.TAShift.LateAllowed.Value %><%} %>" />min</td>
                    <td style="width:10px"><label>Interval</label></td>
                    <td style="width:10px"><input type="text" id="breakMinuteInterval" style="width:30px" <%if (Model.TAShift.BreakMinInterval.HasValue)
                                               { %> value="<%=Model.TAShift.BreakMinInterval.Value %><%} %>"/>min</td>                    
                </tr>
                <tr>
                    <td style="width:10px;"><label>Duration of break overtime</label></td>
                    <td style="padding-right:10px;width:0.1px"><input type="text" id="durationOfBreakOvertime" style="width:30px" <%if (Model.TAShift.DuratOfBreakOvertime.HasValue)
                                                                   { %> value="<%=Model.TAShift.DuratOfBreakOvertime.Value %><%} %>"/>min</td>
                    <td style="width:10px"><label>Presence</label></td>
                    <td style="width:10px"><input type="text" id="presence" style="width:30px" <%if(Model.TAShift.Presence.HasValue){ %> value="<%=Model.TAShift.Presence.Value %><%} %>"/>min</td>
                    <td></td>
                    <td></td>
                </tr>
            </tbody>
        </table>
    </fieldset>
        </div>
    <div style="margin-top:0.2em">
    <fieldset>
        <legend>Overtime</legend>
        <table style="width:100%">
            <tbody>
                <tr>
                    <td style="width:10px"><label>Break Interval</label></td>
                    <td style="width:20px"><input type="text" id="overtime" style="width:30px" <%if (Model.TAShift.BreakMinIntervalOvertime.HasValue)
                                               { %> value="<%=Model.TAShift.BreakMinIntervalOvertime.Value %><%} %>"/>min</td>
                    <td style="width:10px"><label>Overtime start earlier</label></td>
                    <td style="width:20px"><input type="text" id="overtimeStartEarlier" style="width:30px" <%if (Model.TAShift.OvertimeStartsEarlier.HasValue)
                                               { %> value="<%=Model.TAShift.OvertimeStartsEarlier.Value %><%} %>"/>min</td>
                    <td style="width:10px"><label>Overtime start later</label></td>
                    <td style="width:20px"><input type="text" id="overtimeStartLater" style="width:30px" <%if (Model.TAShift.OvertimeStartLater.HasValue)
                                               { %> value="<%=Model.TAShift.OvertimeStartLater.Value %><%} %>"/>min</td>
                </tr>
            </tbody>
        </table>
    </fieldset>
        </div>
</div>
<div style="margin-top:0.3em">
    <input type="button" id="addNewtaShiftTimeIntervals" value="Add Time Intervals" class="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" onclick="AddEditTaShiftTimeIntervals()" />
</div>

<div id="taShiftTimeIntervals">
    <table style="width:100%">
    <tbody id="tbody">
         <tr>
            <th style="width:2%"><label></label></th>
            <th><label>Name</label></th>
            <th><label>Start time</label></th>
            <th><label>End time</label></th>
            <th><label>TaReport Label</label></th>
       
        </tr>
        <%foreach (var shiftInterval in Model.TAShift.TaShiftTimeIntervals)
            { Model.taReportItem.Where(x => Convert.ToInt32(x.Value) == shiftInterval.TaReportLabelId).SingleOrDefault().Selected = true;  %>
        <tr class="NewtaShiftTimeIntervals" id="lalaLand">
            <%=Html.Hidden("ShiftIntervalId",shiftInterval.Id,new { @class = "shiftIntervalIds" }) %>
            <td></td>
            <td><input type="text" class="taShiftTimeIntervalName" value="<%=shiftInterval.Name %>"/></td>
            <td><input type="time" class="shiftIntervalStartTime" value="<%=shiftInterval.StartTime %>" /></td>
            <td><input type="time" class="shiftIntervalEndTime" value="<%=shiftInterval.EndTime %>"/></td>
            
            <td><%=Html.DropDownList("TaReportLabel", Model.taReportItem, "--Select TaReportLabel--", new { @class = "taReportlabel" }) %></td>
            
        </tr>
        <%Model.taReportItem.Where(x => Convert.ToInt32(x.Value) == shiftInterval.TaReportLabelId).SingleOrDefault().Selected = false;  } %>
    </tbody>
</table>
</div>

<script>
     function AddEditTaShiftTimeIntervals() {
        debugger;
        $.ajax({
            type: "Get",
            url: "/TAReport/AddNewtaShiftTimeIntervals",
            success: function (result) {
                $("#tbody").append(result);
            }
        });
    }

    function deleteTaShiftTimeIntervalRow() {
       $('#findParentClass').closest('tr').remove();
    }
</script>