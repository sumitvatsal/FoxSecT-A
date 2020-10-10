<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%
    // Html.DevExpress().Button();

    Html.DevExpress().GridView(settings =>
    {
        settings.Name = "TAMoveMounthViewSettings";
        settings.KeyFieldName = "Id";
        settings.CallbackRouteValues = new { Controller = "TAReport", Action = "TATaxReportExportCallBackN" };
        settings.SettingsPager.PageSize = 9999999;
        settings.CommandColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.AllPages;
        settings.SettingsEditing.Mode = GridViewEditingMode.EditFormAndDisplayRow;

        settings.SettingsSearchPanel.AllowTextInputTimer = false;
        settings.SettingsSearchPanel.ShowApplyButton = true;
        settings.SettingsSearchPanel.ShowClearButton = false;
        settings.SettingsSearchPanel.HighlightResults = true;
        settings.SettingsSearchPanel.Visible = true;
        settings.ControlStyle.CssClass = "grid";
        settings.Columns.Add(column =>
        {
            column.FieldName = "Id";
            column.Caption = ViewResources.SharedStrings.UsersUserId;
            column.HeaderStyle.Font.Bold = true;
        });
        settings.Columns.Add(column =>
        {
            column.FieldName = "LastName";
            column.Caption = ViewResources.SharedStrings.SurName;
            column.HeaderStyle.Font.Bold = true;
        });
        settings.Columns.Add(column =>
        {
            column.FieldName = "FirstName";
            column.Caption = ViewResources.SharedStrings.Name;
            column.HeaderStyle.Font.Bold = true;
        });
        settings.Columns.Add(column =>
        {
            column.FieldName = "CompanyName";
            column.Caption = ViewResources.SharedStrings.UsersCompany;
            column.HeaderStyle.Font.Bold = true;
        });
        settings.Columns.Add(column =>
        {
            column.FieldName = "DepartmentName";
            column.Caption = ViewResources.SharedStrings.UsersDepartment;
            column.HeaderStyle.Font.Bold = true;
        });
        settings.CommandColumn.Visible = true;
        settings.CommandColumn.ShowSelectCheckbox = true;
        settings.CommandColumn.Caption = " ";
        settings.CommandColumn.Width = System.Web.UI.WebControls.Unit.Pixel(50);
        settings.CommandColumn.ShowSelectCheckbox = true;


        // settings.Settings.ShowTitlePanel = true;
        // settings.Settings.ShowFilterRow = true;
        settings.SettingsBehavior.AllowDragDrop = false;

        //settings.SettingsPager.Mode = GridViewPagerMode.EndlessPaging;

    }
).Bind(Model).GetHtml();
%>

 