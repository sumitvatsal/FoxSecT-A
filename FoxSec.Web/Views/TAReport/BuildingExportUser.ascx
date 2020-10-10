<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
    <%
        // Html.DevExpress().Button();

        Html.DevExpress().GridView(settings => {
            settings.Name = "TAMoveMounthViewSettings";
            settings.KeyFieldName = "UserId";
           settings.CallbackRouteValues = new { Controller = "TAReport", Action = "TaBuildingDetailsExportNew" };
            settings.SettingsPager.PageSize = 9999999;
            settings.CommandColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.AllPages;
            settings.SettingsEditing.Mode = GridViewEditingMode.EditFormAndDisplayRow;
            settings.SettingsSearchPanel.AllowTextInputTimer = false;
            settings.SettingsSearchPanel.ShowApplyButton = true;
            settings.SettingsSearchPanel.ShowClearButton= false;
            settings.SettingsSearchPanel.HighlightResults = true;
            settings.SettingsSearchPanel.Visible = true;
            settings.ControlStyle.CssClass = "grid";
           
            settings.Columns.Add(column =>
            {
                column.FieldName = "LastName";
                column.Caption = "Uzvārds";
                column.HeaderStyle.Font.Bold = true;
                column.Width = Unit.Percentage(10.5);
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "FirstName";
                column.Caption = "Vārds";
                column.HeaderStyle.Font.Bold = true;
                column.Width = Unit.Percentage(10.5);
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "PersonalCode";
                column.Caption = "Personas kods";
                column.HeaderStyle.Font.Bold = true;
                column.Width = Unit.Percentage(8.5);
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "Name";
                column.Caption = "Profesija, amats";
                column.HeaderStyle.Font.Bold = true;
                column.Width = Unit.Percentage(8.5);
            });
            
            settings.CommandColumn.Visible = true;
            settings.CommandColumn.ShowSelectCheckbox = true;
            settings.CommandColumn.Caption = " ";
            settings.CommandColumn.Width = System.Web.UI.WebControls.Unit.Pixel(50);
            settings.CommandColumn.ShowSelectCheckbox = true;            
            settings.SettingsBehavior.AllowDragDrop = false;
        }
).Bind(Model).GetHtml();
    %>

 