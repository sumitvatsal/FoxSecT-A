<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.TaShiftsModel>" %>

<div style="float:right">
    <input type="button" value="Add TaShift" id="AddNewTaShifts" class="ui-button ui-widget ui-state-default ui-corner-all" onclick="AddNewTaShifts()" />
</div>
<table style="width:100%">
        <colgroup>
            <col style="width:30%" />
            <col style="width:25%" />
            <col style="width:25%" />
            <col style="width:10%" />
            <col style="width:10%" />
        </colgroup>

    <tbody style="width:100%">
        <tr style="background-color:#000000">
            <th style="color:#FFFFFF">TaShift Name</th>
            <th style="color:#FFFFFF">Start Time</th>
            <th style="color:#FFFFFF">Finish Time</th>
            <th></th>
            <th></th>
        </tr>
        <% int count = 1;  foreach (var shift in Model.TAShifts)
            {%>

        <%if (count % 2 == 0)
                { %><tr style="background-color:#fff"><% }
                else
                { %><tr style="background-color:#ccc"><% } %>
        <td><%=shift.Name %></td>
                    <%if (shift.TaShiftTimeIntervals.Count > 0)
                        { %>
        <td> <%if (shift.TaShiftTimeIntervals.Where(x => x.TaShiftId == shift.Id).FirstOrDefault().StartTime.Minutes == 0)
    {%><%=shift.TaShiftTimeIntervals.Where(x => x.TaShiftId == shift.Id).FirstOrDefault().StartTime.Hours + ":" + shift.TaShiftTimeIntervals.Where(x => x.TaShiftId == shift.Id).FirstOrDefault().StartTime.Minutes + "0"%><% }
    else
    {%><%= shift.TaShiftTimeIntervals.Where(x => x.TaShiftId == shift.Id).FirstOrDefault().StartTime.Hours + ":" + shift.TaShiftTimeIntervals.Where(x => x.TaShiftId == shift.Id).FirstOrDefault().StartTime.Minutes%><% }%> </td>
                    <%}
    else
    { %><td></td><%} %>
                   <%if (shift.TaShiftTimeIntervals.Count > 0)
                       {%>
        <td><%if (shift.TaShiftTimeIntervals.Where(x => x.TaShiftId == shift.Id).LastOrDefault().EndTime.Minutes == 0)
    {%><%=shift.TaShiftTimeIntervals.Where(x => x.TaShiftId == shift.Id).LastOrDefault().EndTime.Hours + ":" + shift.TaShiftTimeIntervals.Where(x => x.TaShiftId == shift.Id).LastOrDefault().EndTime.Minutes + "0" %><% }
    else
    {%><%=shift.TaShiftTimeIntervals.Where(x => x.TaShiftId == shift.Id).LastOrDefault().EndTime.Hours + ":" + shift.TaShiftTimeIntervals.Where(x => x.TaShiftId == shift.Id).LastOrDefault().EndTime.Minutes %><%} %></td>
                    <%}
    else
    {%><td></td><%} %>
        <td><span id='EditShiftButton' title ="Edit" class='icon icon_green_go' onclick='EditTaShift(<%=shift.Id %>)'"></span></td>
        <td></td>
         <%=Html.Hidden(shift.Id.ToString(), shift.Name, new { id = shift.Id.ToString() }) %>
        </tr>
        <%count = count + 1; }%>
    </tbody>
</table>