<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
   <%           
    @Html.DevExpress().GridView(
            settings =>
            {
                settings.Name = "HRList";
                settings.KeyFieldName = "Id";
                settings.CallbackRouteValues = new { Controller = "User", Action = "HRList" };

                settings.CommandColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.AllPages;
                settings.SettingsEditing.Mode = GridViewEditingMode.EditFormAndDisplayRow;

                settings.SettingsSearchPanel.AllowTextInputTimer = false;
                settings.SettingsSearchPanel.ShowApplyButton = true;
                settings.SettingsSearchPanel.ShowClearButton = false;
                settings.SettingsSearchPanel.HighlightResults = true;
                settings.SettingsSearchPanel.Visible = true;
                settings.ControlStyle.CssClass = "grid";

               settings.CommandColumn.Visible = true;
               settings.CommandColumn.ShowSelectCheckbox = true;
               settings.CommandColumn.Caption = " ";
               settings.CommandColumn.Width = System.Web.UI.WebControls.Unit.Pixel(50);
                settings.CommandColumn.ShowSelectCheckbox = true;


                // settings.Settings.ShowTitlePanel = true;
                // settings.Settings.ShowFilterRow = true;
                settings.SettingsBehavior.AllowDragDrop = false;

                settings.Columns.Add(column =>
                {
                    column.FieldName = "Id";
                });
                settings.Columns.Add(column =>
                {
                    column.FieldName = "Name";
                });
                settings.Columns.Add(column =>
                {
                    column.FieldName = "LastName";
                });
                settings.Columns.Add(column =>
                {
                    column.FieldName = "Department";
                });
                settings.Columns.Add(column =>
                {
                    column.FieldName = "CompanyName";
                });
                settings.Columns.Add(column =>
                {
                    column.FieldName = "LastDateOfWork";
                });


            }).Bind(Model).Render();
%> 