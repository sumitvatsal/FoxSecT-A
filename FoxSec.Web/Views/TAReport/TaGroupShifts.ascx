<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.TaShiftsModel>" %>
<table>
    <thead>
        @Html.Action("SchedulerPartial")

    </thead>
    <tbody id="tBody">
        <tr>
            <td><label>Name</label></td>
            <td><input type="text" id="name" /></td>
        </tr>
        <tr>
            <td><label>Repeat Cycle</label></td>
            <td><%= Html.DropDownList("repeatCyle", Model.repeatWeeksList,"--Select week(s)--")%></td>
            <%--<td><select onchange="loadTaWeekShiftsDropdown">
                <option value="0">--Select week(s)--</option>
                <%
                   
                    for (int countWeeks=1; countWeeks <= 52; countWeeks++)
                    {
                       int weeks = countWeeks;
                        %>
                          <option value="<% weeks.ToString(); %>"><% weeks.ToString(); %></option>
                    <%}
                %>
               
                </select></td>--%>
        </tr>
        
    </tbody>
</table>

<%--<%
    Html.DevExpress().Scheduler(settings =>
    {
        settings.Name = "Scheduler";
        settings.ActiveViewType = DevExpress.XtraScheduler.SchedulerViewType.Timeline;
        settings.Views.TimelineView.IntervalCount = 21;
    }).GetHtml();
 %>--%>
<%
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

%>
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