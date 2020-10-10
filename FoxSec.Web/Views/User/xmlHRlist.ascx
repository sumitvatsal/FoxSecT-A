<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>"%>
   <%  

    @Html.DevExpress().GridView(
            settings =>
            {
                settings.Name = "xmlHRlist";
                settings.KeyFieldName = "ID";
                settings.CallbackRouteValues = new { Controller = "User", Action = "xmlHRlist" };

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
                settings.CommandColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.None;//hide "select all" checkbox
                settings.CommandColumn.Caption = " ";
                settings.CommandColumn.Width = System.Web.UI.WebControls.Unit.Pixel(50);


                // settings.Settings.ShowTitlePanel = true;
                // settings.Settings.ShowFilterRow = true;
                settings.SettingsBehavior.AllowDragDrop = false;
                settings.SettingsPager.PageSize = 8;

                settings.CommandColumn.SetHeaderTemplateContent(container => Html.DevExpress().CheckBox(checkSettings =>
                {
                    checkSettings.Name = "selectAll";
                    checkSettings.Properties.ClientSideEvents.CheckedChanged = "OnBulkAllCheckedChanged";
                }).Render());

                //settings.Columns.Add(column =>
                //{
                //    column.FieldName = "id";
                //    column.Caption = "Personal Code";
                //});
                //settings.Columns.Add(column =>
                //{
                //    column.FieldName = "FirstName";
                //});
                //settings.Columns.Add(column =>
                //{
                //    column.FieldName = "LastName";
                //});
                //settings.Columns.Add(column =>
                //{
                //    column.FieldName = "LoginName";
                //});
                //settings.Columns.Add(column =>
                //{
                //    column.FieldName = "Email";
                //});
                //settings.Columns.Add(column =>
                //{
                //    column.FieldName = "CreatedBy";
                //});
                //settings.Columns.Add(column =>
                //{
                //    column.FieldName = "Name";
                //    column.Caption = "Company";
                //});
                //settings.Columns.Add(column =>
                //{
                //    column.FieldName = "ValidFrom";
                //});
                //settings.Columns.Add(column =>
                //{
                //    column.FieldName = "ValidTo";
                //});
                //settings.Columns.Add(column =>
                //{
                //    column.FieldName = "Serial";
                //});
                //settings.Columns.Add(column =>
                //{
                //    column.FieldName = "Dk";
                //});
                //            settings.DataBound = (sender, e) => {
                //    MVCxGridView grid = sender as MVCxGridView;
                //    if (grid.Columns.IndexOf(grid.Columns["CommandColumn"]) != -1)
                //        return;
                //    GridViewCommandColumn col = new GridViewCommandColumn();
                //    col.Name = "CommandColumn";
                //    col.ShowSelectCheckbox = true;
                //    col.VisibleIndex = 0;
                //    grid.Columns.Add(col);
                //};
                //             settings.Columns.Add(unboundColumn => {
                //    unboundColumn.FieldName = "UniqueFieldName";
                //    unboundColumn.UnboundType = DevExpress.Data.UnboundColumnType.String;
                //});
                foreach (System.Data.DataColumn column in Model.xmlTable.Columns)
                {
                    if(column.ColumnName=="ID")
                    {
                        settings.Columns.Add(Columns =>
                        {
                            Columns.FieldName = "ID";
                            Columns.Visible = false;
                        });
                    }
                    else
                    {
                        settings.Columns.Add(column.ColumnName);
                    }
                }
            }).Bind(Model.xmlTable).Render();
%> 