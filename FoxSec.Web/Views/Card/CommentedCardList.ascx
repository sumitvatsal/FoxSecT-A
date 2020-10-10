<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<FoxSec.Web.ViewModels.ComentedCardListModel>>"%>
<%

    Html.DevExpress().GridView(settings => {
        settings.Name = "gridview";
        settings.CallbackRouteValues = new { Controller = "Card", Action = "CommentedCardsList" };
        settings.SettingsEditing.BatchUpdateRouteValues = new { Controller = "Card", Action = "CardEditingUpdateComment" };
        settings.CommandColumn.Visible = false;
        settings.CommandColumn.ShowNewButtonInHeader = false;
        settings.CommandColumn.ShowDeleteButton = false;

        settings.SettingsSearchPanel.AllowTextInputTimer = false;
        settings.SettingsSearchPanel.ShowApplyButton = true;
        settings.SettingsSearchPanel.ShowClearButton = false;
        settings.SettingsSearchPanel.HighlightResults = true;
        settings.SettingsSearchPanel.Visible = true;
        settings.ControlStyle.CssClass = "grid";
        settings.SettingsPager.PageSize = 10;
        

        settings.SettingsEditing.Mode = GridViewEditingMode.Batch;
        settings.SettingsEditing.BatchEditSettings.EditMode = GridViewBatchEditMode.Cell;
        settings.SettingsEditing.BatchEditSettings.StartEditAction = GridViewBatchStartEditAction.Click;
        //settings.CommandColumn.Visible = true;
        //settings.CommandColumn.ShowNewButtonInHeader = true;
        //settings.CommandColumn.ShowDeleteButton = false;

        settings.KeyFieldName = "Id";
        //settings.Columns.Add(column =>
        //{
        //    column.Name = "Id";
        //    column.Caption = "Id";
        //    column.HeaderStyle.Font.Bold = true;
        //    column.FieldName = "Id";


        //});
        settings.Columns.Add(column =>
        {
            column.Name = "FirstName";
            column.Caption = "FirstName";
            column.HeaderStyle.Font.Bold = true;
            column.FieldName = "FirstName";
            column.ReadOnly = true;
            column.Width = 120;


        });

        settings.Columns.Add(column =>
        {
            column.Name = "LastName";
            column.Caption = "LastName";
            column.HeaderStyle.Font.Bold = true;
            column.FieldName = "LastName";
            column.ReadOnly = true;
            column.Width = 120;

        });

        settings.Columns.Add(column =>
        {
           // column.Name = "ValidFrom";
            column.Caption = "ValidFrom";
            column.HeaderStyle.Font.Bold = true;
            column.FieldName = "ValidFrom";
            column.ColumnType = MVCxGridViewColumnType.DateEdit;
            column.Width = 120;


        });

        settings.Columns.Add(column =>
        {
            //column.Name = "ValidTo";
            column.Caption = "ValidTo";
            column.HeaderStyle.Font.Bold = true;
            column.FieldName = "ValidTo";
            column.ColumnType = MVCxGridViewColumnType.DateEdit;
            column.Width = 120;


        });

        settings.Columns.Add(column =>
        {
            column.Name = "Comment";
            column.Caption = "Comment";
            column.HeaderStyle.Font.Bold = true;
            column.FieldName = "Comment";


        });
         settings.CellEditorInitialize = (s, e) =>
        {
            ASPxEdit editor = (ASPxEdit)e.Editor;
            editor.ValidationSettings.Display = Display.None;
            //settings.ClientSideEvents.BeginCallback = "showBuildingNameList";
          
        };

        settings.ClientSideEvents.EndCallback = "function(s,e){RefreshGrid(); }";
         settings.ClientSideEvents.BeginCallback = "function(s,e){testgrid(); }";


    }).Bind(Model).GetHtml();

        %>

<script>
    var i = 0;
    function RefreshGrid() {

        if (i == 0) {
            gridview.Refresh();


        }
        i++;

    }
    function testgrid() {

        i = 0;

    }

</script>