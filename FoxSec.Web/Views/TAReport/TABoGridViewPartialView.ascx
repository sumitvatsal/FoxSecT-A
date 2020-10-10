<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>    <%-- DXCOMMENT: Configure GridView --%>
    <%
        Html.DevExpress().GridView(settings => {
            settings.Name = "TAReportMounthViewSettings";
            settings.KeyFieldName = "Id";

            // hiljem teen   settings.ClientSideEvents.CustomizationWindowCloseUp = "grid_CustomizationWindowCloseUp";
            settings.CallbackRouteValues = new { Controller = "TAReport", Action = "BuildingsObjectsGridViewPartialA" };
            settings.SettingsEditing.BatchUpdateRouteValues = new { Controller = "TAReport", Action = "BuildingsObjectsBatchGridEditA" };


            settings.SettingsEditing.Mode = GridViewEditingMode.Batch;
            settings.SettingsEditing.BatchEditSettings.EditMode = GridViewBatchEditMode.Cell;
            settings.SettingsEditing.BatchEditSettings.StartEditAction = GridViewBatchStartEditAction.Click;
            settings.SettingsEditing.BatchEditSettings.ShowConfirmOnLosingChanges = true;
            settings.SettingsEditing.ShowModelErrorsForEditors = true;

            //settings.CommandColumn.Visible = true;
            //settings.CommandColumn.ShowNewButtonInHeader = true;
            //settings.CommandColumn.ShowDeleteButton = true;


            settings.SettingsSearchPanel.AllowTextInputTimer = false;
            settings.SettingsSearchPanel.ShowApplyButton = true;
            settings.SettingsSearchPanel.ShowClearButton = false;
            settings.SettingsSearchPanel.HighlightResults = true;
            settings.SettingsSearchPanel.Visible = true;


            settings.Width = System.Web.UI.WebControls.Unit.Percentage(80);
            settings.SettingsPager.PageSize = 15;
            //settings.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            //settings.Settings.VerticalScrollableHeight = 350;

            settings.ControlStyle.Paddings.Padding = System.Web.UI.WebControls.Unit.Pixel(0);
            settings.ControlStyle.Border.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(1);
            settings.ControlStyle.BorderBottom.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(2);
            //settings.ControlStyle.Border.BorderColor = System.Drawing.Color.Red; // ok

            // DXCOMMENT: Configure grid's columns in accordance with data model fields

            settings.Columns.Add(column =>
            {
                column.FieldName = "ObjectNr";
                column.Caption = "Object Number";
                column.ColumnType = MVCxGridViewColumnType.TextBox;
                column.EditFormSettings.Visible = DefaultBoolean.False;
            });

            settings.Columns.Add(column => {
                column.FieldName = "Description";
                column.EditFormSettings.Visible = DefaultBoolean.False;
            });

            settings.Columns.Add(column => {
                column.FieldName = "GlobalBuilding";
                column.Caption = "Select";
                column.ColumnType = MVCxGridViewColumnType.CheckBox;
            });


        }).Bind(Model).Render(); %>
