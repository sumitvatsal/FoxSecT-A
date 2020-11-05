<%@ Control Language="C#"  Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.TaShiftsModel>" %>
<%@ Import Namespace="DevExtreme.AspNet.Mvc"%>

<%--<div>
    <table>
        <tbody>
            <tr>
                <td><label>Name</label></td>
                <td><input type="text" id="shiftName" /></td>
            </tr>
            <tr>
                <td><label>Start Time</label></td>
                <td><input type="time" id="shiftStartTime" /></td>
            </tr>
            <tr>
                <td><label>Finish Time</label></td>
                <td><input type="time" id="shiftFinishTime" /></td>
            </tr>
            <tr>
                <td><label>Break</label></td>
                <td><input type="text" id="shiftBreak" /></td><td><label>min</label></td>
            </tr>
        </tbody>
    </table>
</div>--%>

<div>
    <table>
        <tbody>
            <tr>
                <td><label>Shift Name</label></td>
                <td><input type="text" id="shiftName" /></td>
            </tr>
            <tr>
                <td><label>Start From</label></td>
                <td><input type="date" id="startFrom" /></td>
            </tr>
       
            <tr>
                <td><label>Finish at</label></td>
                <td><input type="date" id="finishAt" /></td>       
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
                   <td style="padding-right:10px;width:0.1px"><input type="text" id="breakTime" style="width:30px" />min</td>
                    <td style="width:10px"><label>Late allowed</label></td>
                    <td style="padding-right:10px;width:0.1px"><input type="text" id="lateAllowed" style="width:30px" />min</td>
                    <td style="width:10px"><label>Interval</label></td>
                    <td style="width:10px"><input type="text" id="breakMinuteInterval" style="width:30px" />min</td>                    
                </tr>
                <tr>
                    <td style="width:10px;"><label>Duration of break overtime</label></td>
                    <td style="padding-right:10px;width:0.1px"><input type="text" id="durationOfBreakOvertime" style="width:30px" />min</td>
                    <td style="width:10px"><label>Presence</label></td>
                    <td style="width:10px"><input type="text" id="presence" style="width:30px"/>min</td>
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
                    <td style="width:20px"><input type="text" id="overtime" style="width:30px" />min</td>
                    <td style="width:10px"><label>Overtime start earlier</label></td>
                    <td style="width:20px"><input type="text" id="overtimeStartEarlier" style="width:30px" />min</td>
                    <td style="width:10px"><label>Overtime start later</label></td>
                    <td style="width:20px"><input type="text" id="overtimeStartLater" style="width:30px" />min</td>
                </tr>
            </tbody>
        </table>
    </fieldset>
        </div>
</div>
<div style="margin-top:0.3em">
    <input type="button" id="addNewtaShiftTimeIntervals" value="Add Time Intervals" class="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" onclick="AddNewTaShiftTimeIntervals()" />
</div>

<div id="taShiftTimeIntervals">
    <table style="width:100%">
    <tbody id="tbody">
         <tr>
            <th style="width:2%"><label></label></th>
            <th><label>Name</label></th>
            <th><label>Start time</label></th>
            <th><label>End time</label></th>
            <th><label>TaReport label</label></th>   
        </tr>
    </tbody>
</table>
</div>

<script>
    function AddNewTaShiftTimeIntervals() {
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
<%--<%
   
    Html.DevExtreme().Scheduler()


        .DataSource(Model.schedulerData)
        .TextExpr("text")
        .StartDateExpr("startDate")
        .EndDateExpr("endDate")
        .Views(new DevExtreme.AspNet.Mvc.SchedulerViewType[]
        {
           DevExtreme.AspNet.Mvc.SchedulerViewType.Week,
           DevExtreme.AspNet.Mvc.SchedulerViewType.Month
        })
        .CurrentView(DevExtreme.AspNet.Mvc.SchedulerViewType.Week)
        .CurrentDate(new DateTime(2020, 10, 05))
        .Height(800)
        .StartDayHour(9)
        .EndDayHour(21)
        .ShowAllDayPanel(true);
    #region oldDevexpress form
    //Html.DevExpress<FoxSec.Web.ViewModels.TaShiftsModel>().GridView(settings =>
    //{
    //    settings.Name = "TaShifts";
    //    settings.SettingsEditing.Mode = GridViewEditingMode.Batch;
    //    settings.CallbackRouteValues = new { Controller = "TAReport", Action = "TaShift" };
    //    settings.CommandColumn.Visible = true;
    //    settings.CommandColumn.ShowNewButtonInHeader = true;
    //    settings.CommandColumn.ShowDeleteButton = true;
    //    settings.KeyFieldName = "Id";

    //    settings.Columns.Add(column =>{
    //        column.FieldName = "Name";
    //        column.Caption = ViewResources.SharedStrings.Name;
    //        column.ColumnType = MVCxGridViewColumnType.TextBox;
    //    });

    //    settings.Columns.Add(column =>
    //    {
    //        column.FieldName = "StartFrom";
    //        column.Caption = ViewResources.SharedStrings.TaShiftStartFrom;
    //        column.ColumnType = MVCxGridViewColumnType.DateEdit;
    //    });

    //    settings.Columns.Add(column =>
    //    {
    //        column.FieldName = "FinishAt";
    //        column.Caption = ViewResources.SharedStrings.TaShiftFinishAt;
    //        column.ColumnType = MVCxGridViewColumnType.DateEdit;
    //    });

    //    settings.Columns.Add(column =>
    //    {
    //        column.FieldName = "WorkBeforeBreak";
    //        column.Caption = ViewResources.SharedStrings.TaShiftWorkBeforeBreak;
    //        column.ColumnType = MVCxGridViewColumnType.TextBox;
    //    });

    //    settings.Columns.Add(column =>
    //    {
    //        column.FieldName = "Breaks";
    //        column.Caption = ViewResources.SharedStrings.TaShiftBreaks;
    //        column.ColumnType = MVCxGridViewColumnType.TextBox;
    //    });

    //    settings.Columns.Add(column =>
    //    {
    //        column.FieldName = "WorkBeforeLunch";
    //        column.Caption = ViewResources.SharedStrings.TaShiftWorkBeforeLunch;
    //        column.ColumnType = MVCxGridViewColumnType.TextBox;
    //    });

    //    settings.Columns.Add(column =>
    //    {
    //        column.FieldName = "Lunch";
    //        column.Name = ViewResources.SharedStrings.TaShiftLunch;
    //        column.ColumnType = MVCxGridViewColumnType.TextBox;
    //    });

    //    settings.Columns.Add(column =>
    //    {
    //        column.FieldName = "OvertimeMin";
    //        column.Caption = ViewResources.SharedStrings.TaShiftsOvertimeMinutes;
    //        column.ColumnType = MVCxGridViewColumnType.TextBox;
    //    });

    //    settings.Columns.Add(column =>
    //    {
    //        column.FieldName = "CompanyId";
    //        column.Caption = ViewResources.SharedStrings.TaReportLabelCompanyId;
    //        column.ColumnType = MVCxGridViewColumnType.ComboBox;
    //        var comboboxProperties = column.PropertiesEdit as ComboBoxProperties;
    //        comboboxProperties.DataSource = Model.CompaniesList;
    //        comboboxProperties.TextField = "Name";
    //        comboboxProperties.ValueField = "Id";
    //        comboboxProperties.ValueType = typeof(int);
    //    });



    //}).Bind(Model.TAShifts).GetHtml();
    #endregion
    //Html.DevExpress().Scheduler(settings =>
    //{
    //    settings.Name = "TaScheduler";
    //    settings.Views.DayView.Styles.ScrollAreaHeight = 400;
    //    settings.Views.DayView.DayCount = 7;
    //    settings.Start = DateTime.Now;
    //    settings.Width = 300;
    //}).Bind(Model.CompaniesList).GetHtml();

%>--%>


