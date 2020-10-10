<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
   <%           
       Html.DevExpress().GridView(
        settings =>
        {


            settings.Name = "GridView";
            settings.CallbackRouteValues = new { Controller = "Home", Action = "GridViewPartialView" };
            settings.SettingsEditing.AddNewRowRouteValues = new { Controller = "Home", Action = "GridViewPartialAddNew" };
            settings.SettingsEditing.UpdateRowRouteValues = new { Controller = "Home", Action = "GridViewPartialUpdate" };
            settings.SettingsEditing.DeleteRowRouteValues = new { Controller = "Home", Action = "GridViewPartialDelete" };
            settings.SettingsEditing.Mode = GridViewEditingMode.EditFormAndDisplayRow;
            settings.SettingsBehavior.ConfirmDelete = true;

            settings.KeyFieldName = "Column1";


            settings.CommandColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.AllPages;
            settings.SettingsEditing.Mode = GridViewEditingMode.EditFormAndDisplayRow;

            settings.SettingsSearchPanel.AllowTextInputTimer = false;
            settings.SettingsSearchPanel.ShowApplyButton = true;
            settings.SettingsSearchPanel.ShowClearButton = false;
            settings.SettingsSearchPanel.HighlightResults = true;
            settings.SettingsSearchPanel.Visible = true;
            settings.ControlStyle.CssClass = "grid";

            settings.SettingsPager.Visible = true;
            settings.Settings.ShowGroupPanel = true;
            settings.Settings.ShowFilterRow = true;
            settings.SettingsBehavior.AllowSelectByRowClick = true;


            settings.CommandColumn.ShowSelectCheckbox = true;
            settings.CommandColumn.Visible = true;
            settings.Columns.Add(column =>
            {
                column.FieldName = "Column1";
                column.Caption = "userid";
            });
            // settings.Columns.Add("Column1");
            settings.Columns.Add("Column3");

            settings.Columns.Add("Column5");
            settings.Columns.Add("Column7");
            settings.Columns.Add("Column9");
            settings.Columns.Add("Column11");

            settings.Columns.Add("Column13");

            settings.Columns.Add("Column15");

            settings.Columns.Add("Column17");

            settings.PreRender = (sender, e) =>
            {
                MVCxGridView gridView = sender as MVCxGridView;
                if ((gridView != null) && (ViewData["selectedRows"] != null))
                {
                    int[] selectedRows = (int[])ViewData["selectedRows"];
                    foreach (int key in selectedRows)
                    {
                        gridView.Selection.SelectRowByKey(key);
                    }
                }

            };


        }).Bind(Model).GetHtml();

        %> 