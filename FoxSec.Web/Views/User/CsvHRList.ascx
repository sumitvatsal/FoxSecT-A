<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
   
<%--<%@ Control Language="C#"   Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.datatableListViewModel>" %>--%>

<%           
    @Html.DevExpress().GridView(
            settings =>
            {
                settings.Name = "CsvHRList";
                settings.KeyFieldName = "ois_id_isik";
                settings.CallbackRouteValues = new { Controller = "User", Action = "CsvHRList" };

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
                    column.FieldName = "ois_id_isik";
                });
                settings.Columns.Add(column =>
                {
                    column.FieldName = "isikukood";
                });
                settings.Columns.Add(column =>
                {
                    column.FieldName = "e_nimi";
                });
                settings.Columns.Add(column =>
                {
                    column.FieldName = "p_nimi";
                });
                settings.Columns.Add(column =>
                {
                    column.FieldName = "kasutajatunnus";
                });
                settings.Columns.Add(column =>
                {
                    column.FieldName = "toosuhte_algus";
                });
                settings.Columns.Add(column =>
                {
                    column.FieldName = "toosuhte_lopp";
                });

                settings.Columns.Add(column =>
                {
                    column.FieldName = "ylikooli_e_post";
                });
                settings.Columns.Add(column =>
                {
                    column.FieldName = "tookoha_aadress";
                });

            }).Bind(Model).Render();
%> 


