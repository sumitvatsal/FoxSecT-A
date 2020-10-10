<%@ Control Language="C#"  Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.TaShiftsModel>" %>
<%@ Import Namespace="DevExtreme.AspNet.Mvc"%>

<label>Name</label>

<%
   
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

%>
