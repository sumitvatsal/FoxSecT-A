<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.TaUserGroupShifts>" %>
<link href="../../css/default.css" rel="stylesheet" />
<link href="../../Scripts/jquery-ui-1_11_3.custom/jquery-ui.min.css" rel="stylesheet" />
<%--<table>
    <thead>
       
    </thead>
    <tbody id="tBody">
        <tr>
            <td><label>Name</label></td>
            <td><input type="text" id="name" /></td>
        </tr>
        <tr>
            <td><label>Repeat Cycle</label></td>
            <td><%= Html.DropDownList("repeatCyle", Model.repeatWeeksList,"--Select week(s)--")%></td>
            <td><select onchange="loadTaWeekShiftsDropdown">
                <option value="0">--Select week(s)--</option>
                <%
                   
                    for (int countWeeks=1; countWeeks <= 52; countWeeks++)
                    {
                       int weeks = countWeeks;
                        %>
                          <option value="<% weeks.ToString(); %>"><% weeks.ToString(); %></option>
                    <%}
                %>
               
                </select></td>
        </tr>
        
    </tbody>
</table>--%>

<%--<%
    Html.DevExpress().Scheduler(settings =>
    {
        settings.Name = "Scheduler";
        settings.ActiveViewType = DevExpress.XtraScheduler.SchedulerViewType.Timeline;
        settings.Views.TimelineView.IntervalCount = 21;
    }).GetHtml();
 %>--%>
<%Html.Hidden("taUserGroupShiftId", Model.Id);%>
<table>
    <tr>
        <td><label>Name</label></td>
        <td><input type="text" id="name" value="<%=Model.Name %>" readonly/></td>
    </tr>
    <tr>
        <td><label>Repeat After Weeks</label></td>
        <td><input type="text" id="repeatAfterWeeks" value="<%=Model.RepeatAfterWeeks %>" readonly/></td>
        <td style="width:50%"><button class="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" onclick="openTaWeekShiftsDialog()"><span class="ui-button-text">Add week shift</span></button></td>
        
    </tr>
    <% int weekCount = 0; foreach (var weekShift in Model.TaWeekShifts)
        {
            weekCount = weekCount + 1; %>
    <tr>
        <td><label>Week<%=weekCount %></label></td>
        <td>
            <select>
                <% foreach (var allTaWeekShiftItem in Model.AllTaWeekShifts)
                    { if (allTaWeekShiftItem.Name.ToString().Equals(weekShift.Name.ToString())){%>
                <option value="<%=allTaWeekShiftItem.Id %>" selected><%=allTaWeekShiftItem.Name %></option>
                <%} else { %>
                            <option value="<%=allTaWeekShiftItem.Id %>"><%=allTaWeekShiftItem.Name %></option>
                    <%} } %>
            </select>
        </td>
        
    </tr>
    <%} %>
    <%--<tr>
        <td></td>
        <td></td>
        <td style="width:50%"><button class="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" onclick=""><span class="ui-button-text">Add-Remove users</span></button></td>
    </tr>--%>
</table>

<%
    Html.DevExtreme().Scheduler()
        .ID("schedulerId")
        .DataSource(Model.ShiftSchedulerDisplays)
        .TextExpr("Text")
        .StartDateExpr("StartDate")
        .EndDateExpr("EndDate")
        //.Views(new[] { DevExtreme.AspNet.Mvc.SchedulerViewType.Week, DevExtreme.AspNet.Mvc.SchedulerViewType.Month })
        .Views(views =>
        {
            views.Add()
            .Name("Workview")
            .Type(DevExtreme.AspNet.Mvc.SchedulerViewType.WorkWeek)
            .IntervalCount(4)
            .StartDate(new DateTime(2020, 10, 14));


        })
        .CurrentView(DevExtreme.AspNet.Mvc.SchedulerViewType.Week)
        .CurrentDate(DateTime.Now.Date)
        .Height(800)
        .Width(1200)
        .StartDayHour(0)
        .EndDayHour(24)
        .FirstDayOfWeek(DevExtreme.AspNet.Mvc.FirstDayOfWeek.Monday)
        .Visible(true)
        .Render();
        %>

<%--<%
    Html.DevExpress().GetStyleSheets(
        new StyleSheet { ExtensionSuite = ExtensionSuite.Scheduler, Theme = "Aqua" }
    );
    Html.DevExpress().Scheduler(settings =>
    {
        settings.Name = "schedulerTA";

        settings.LimitInterval.AllDay = true;
        settings.OptionsView.ShowOnlyResourceAppointments = true;
        settings.Width = 600;
        settings.Views.WeekView.Enabled = true;
        settings.ActiveViewType = DevExpress.XtraScheduler.SchedulerViewType.Timeline;
        
    }).Bind(Model.schedulerData).GetHtml();

%>--%>
<script>
    $(document).ready(function () {
        $("#repeatCyle").change(function () {
            
            debugger;
            var selectedRepeatWeeksValue = $("#repeatCyle").val();
            $.ajax({
                type: "Get",
                data: { selectedWeek: selectedRepeatWeeksValue },
                url: "/TAReport/TaWeeksValues",
                cache: false,
                beforeSend: function () {
                    $(".dynamicValues").remove();
                },
                success: function (result) {
                    $("#tBody").append(result);
                }

            });
        });
    });

     
</script>