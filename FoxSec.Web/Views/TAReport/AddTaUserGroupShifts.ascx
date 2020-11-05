<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<table>
    <tbody id="taWeekShiftToAppend">
        <tr>
            <td><label>Name</label></td>
            <td><input type="text" id="taUserGroupShiftName" /></td>
        </tr>
        <tr>
            <td><label>Repeat weeks</label></td>
            <td>
                <select id="repeatCycle" onchange="selectedWeek()">
                    <option value="0">--Select weeks--</option>
                    <%for (int i = 1; i <= 52; i++)
                        { %>
                      <option value="<%=i %>"><%=i %></option>
                    <%} %>
                </select>
            </td>
        </tr>
        
    </tbody>
</table>

<script>
    function selectedWeek() {
        var repeatCycleSelectedValue = $("#repeatCycle").val();
        debugger;
        $.ajax({
            type: "Get",
            url: "/TaReport/TaWeeksValues",
            data: { selectedWeek: repeatCycleSelectedValue },
            beforeSend: function () {
                $(".dynamicValues").remove();
            },
            success: function (result) {
                $("#taWeekShiftToAppend").append(result);
                $("#taWeekShiftToAppend").append('<tr><td><label>Start Date</label></td><td><input type="date" id="taUserGroupShiftStartDate" /></td></tr>');
            }
        });
    }
</script>