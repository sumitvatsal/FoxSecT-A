<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<%
    Html.DevExpress().GridView(
    settings =>
    {
        settings.Name = "drillDownGridView";
        settings.CallbackRouteValues = new { Action = "TAReportObjectsBatchGridEdit", Controller = "TAReport" };
        settings.SettingsEditing.BatchUpdateRouteValues = new { Controller = "TAReport", Action = "TAReportObjectsBatchGridEdit" };

        settings.SettingsEditing.Mode = GridViewEditingMode.Batch;
        settings.SettingsEditing.BatchEditSettings.EditMode = GridViewBatchEditMode.Cell;
        settings.SettingsEditing.BatchEditSettings.StartEditAction = GridViewBatchStartEditAction.Click;
        settings.SettingsEditing.BatchEditSettings.ShowConfirmOnLosingChanges = true;
        settings.SettingsEditing.ShowModelErrorsForEditors = true;
        settings.SettingsPager.PageSize = 31;
        settings.CommandColumn.Visible = true;
        settings.CommandColumn.ShowDeleteButton = true;

        //if (ViewBag.TotalRowsCount == "0")
        //{
        //    settings.CommandColumn.ShowNewButtonInHeader = true;
        //}
        //else
        //{
        //    settings.CommandColumn.ShowNewButtonInHeader = false;
        //}
        settings.CommandColumn.ShowNewButtonInHeader = true;
        settings.KeyFieldName = "Id";
        settings.Columns.Add(column =>
        {
            column.Caption = "Name";
            column.FieldName = "UserName";
            column.EditFormSettings.Visible = DefaultBoolean.False;
        });

        settings.Columns.Add(column =>
        {
            column.FieldName = "ReportDate";
            column.Caption = "Date";
            column.PropertiesEdit.DisplayFormatString = "d";// "dd/MM/yyyy";
            column.Width = 200;
            column.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
            /*
            column.ColumnType = MVCxGridViewColumnType.DateEdit;
            var deProperties = column.PropertiesEdit as DateEditProperties;
            deProperties.DisplayFormatString = "d";*/
            // column.EditFormSettings.Visible = DefaultBoolean.False;
        });

        settings.Columns.Add(column =>
        {
            column.Caption = "Hours";
            column.FieldName = "Hours_Min";
        });

        settings.BeforeGetCallbackResult = (sender, e) =>
        {
            if (ViewBag.IsResetGridViewPageIndex != null && ViewBag.IsResetGridViewPageIndex)
                ((MVCxGridView)sender).PageIndex = 0;
        };

        settings.HtmlRowPrepared = (sender, e) =>
        {

            int status = Convert.ToInt32(e.GetValue("Status"));
            {
                if (status == 2)
                {
                    e.Row.BackColor = System.Drawing.Color.LightBlue;//.LightGreen;
                }
                else
                {
                    e.Row.Controls[0].Controls[0].Visible = false;
                }
            }
        };

        //settings.CustomJSProperties = (sender, e) =>
        //{
        //    MVCxGridView grid = (MVCxGridView)sender;
        //    e.Properties["cpVisibleRowCount"] = grid.VisibleRowCount;
        //    int tc = grid.VisibleRowCount;
        //    if (tc > 0)
        //    {
        //        settings.CommandColumn.ShowNewButtonInHeader = false;
        //    }
        //    else
        //    {
        //        settings.CommandColumn.ShowNewButtonInHeader = true;
        //    }
        //};

        //settings.CommandButtonInitialize = (sender, e) =>
        //{
        //   // var fieldValue = (sender as MVCxGridView).GetRowValues(e.VisibleIndex, "Status");
        //    object fieldValue = (sender as MVCxGridView).GetRowValues(e.VisibleIndex, "Status");
        //    bool processedValue = Convert.ToBoolean(sender.GetRowValues(visibleIndex, "processed"));
        //    if (e.ButtonType == ColumnCommandButtonType.Delete)
        //    {


        //        if (fieldValue.ToString() == "2")
        //            e.Visible = false;
        //    }
        //};
    }).Bind(Model).GetHtml();
%>


