<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%
    // Html.DevExpress().Button();

    Html.DevExpress().GridView(settings =>
    {
        settings.Name = "VisitorsUserDetails";
        settings.KeyFieldName = "Id";
        settings.CallbackRouteValues = new { Controller = "Visitors", Action = "VisitorUserExportCallBack" };
        settings.SettingsPager.PageSize = 9999999;
        //settings.CommandColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.AllPages;
        settings.SettingsEditing.Mode = GridViewEditingMode.EditFormAndDisplayRow;
        settings.SettingsSearchPanel.AllowTextInputTimer = true;
        settings.SettingsSearchPanel.ShowApplyButton = true;
        settings.SettingsSearchPanel.ShowClearButton = false;
        settings.SettingsSearchPanel.HighlightResults = true;
        settings.SettingsSearchPanel.Visible = true;

        settings.ControlStyle.CssClass = "grid";

        /* settings.Columns.Add(column =>
         {
             column.FieldName = "Id";
             column.Caption = "UserId";
             column.HeaderStyle.Font.Bold = true;
         });
         */
        settings.Columns.Add(column =>
        {
            column.FieldName = "Id";
            column.Caption = "UserId";
            column.HeaderStyle.Font.Bold = true;
            column.Visible = false;
        });
        settings.Columns.Add(column =>
        {
            column.FieldName = "FirstName";
            column.Caption = "First Name";
            column.HeaderStyle.Font.Bold = true;
        });

        settings.Columns.Add(column =>
        {
            column.FieldName = "LastName";
            column.Caption = "Last Name";
            column.HeaderStyle.Font.Bold = true;
        });

        settings.Columns.Add(column =>
        {
            column.FieldName = "UserPermissionGroupName";
            column.Caption = "Permissions";
            column.HeaderStyle.Font.Bold = true;
        });

        settings.Columns.Add(column =>
        {
            column.FieldName = "CompanyName";
            column.Caption = "Company";
            column.HeaderStyle.Font.Bold = true;
        });
        settings.CommandColumn.Visible = true;
        settings.CommandColumn.Caption = " ";
        settings.CommandColumn.Width = System.Web.UI.WebControls.Unit.Pixel(50);
        settings.CommandColumn.ShowSelectCheckbox = true;

        // settings.Settings.ShowTitlePanel = true;
        // settings.Settings.ShowFilterRow = true;
        settings.SettingsBehavior.AllowDragDrop = false;
        settings.SettingsBehavior.AllowSelectSingleRowOnly = true;

        //settings.SettingsPager.Mode = GridViewPagerMode.EndlessPaging;

    }
).Bind(Model).GetHtml();
%>

 