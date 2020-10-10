<%-- <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
  <link rel="stylesheet" href="/resources/demos/style.css">
  <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
  <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
  <script>
  $( function() {
      $(".ValidFrom").datepicker();
      $("#ValidTo").datepicker();
  } );
  </script>--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.BuildingNameViewModel>" %>
<%-- DXCOMMENT: Configure GridView --%>

<%
    Html.DevExpress().GridView(settings =>
    {
        settings.Name = "myGridView";
        settings.CallbackRouteValues = new { Controller = "TAReport", Action = "TABuildingListforChange" };
        settings.SettingsEditing.BatchUpdateRouteValues = new { Controller = "TAReport", Action = "BatchEditingUpdateBuilding_Name" };
        

        settings.SettingsEditing.Mode = GridViewEditingMode.Batch;
        settings.SettingsEditing.BatchEditSettings.EditMode = GridViewBatchEditMode.Cell;
        settings.SettingsEditing.BatchEditSettings.StartEditAction = GridViewBatchStartEditAction.Click;
        settings.CommandColumn.Visible = true;
        settings.CommandColumn.ShowNewButtonInHeader = true;
        settings.CommandColumn.ShowDeleteButton = true;

        settings.SettingsSearchPanel.AllowTextInputTimer = false;
        settings.SettingsSearchPanel.ShowApplyButton = true;
        settings.SettingsSearchPanel.ShowClearButton = false;
        settings.SettingsSearchPanel.HighlightResults = true;
        settings.SettingsSearchPanel.Visible = true;
        settings.ControlStyle.CssClass = "grid";
        settings.SettingsPager.PageSize = 10;

        settings.KeyFieldName = "Id";
        //settings.Columns.Add("BuildingId");
        settings.Columns.Add(column =>
        {
            column.FieldName = "BuildingId";
            column.Caption = ViewResources.SharedStrings.TaHardwareBuildingName;
            column.HeaderStyle.Font.Bold = true;
            column.ColumnType = MVCxGridViewColumnType.ComboBox;
            //column.Width = Unit.Percentage(9);
            var comboBoxProperties = column.PropertiesEdit as ComboBoxProperties;
            comboBoxProperties.DataSource = Model.buildings;
            comboBoxProperties.TextField = "BuildingName";
            comboBoxProperties.ValueField = "BuildingId";
            comboBoxProperties.ValueType = typeof(int);
        });

        settings.Columns.Add(column =>
        {
            column.FieldName = "Customer";
            column.Caption = ViewResources.SharedStrings.TaBuildingcustomer;
            //column.Caption = "Building customer";
            column.HeaderStyle.Font.Bold = true;
            //column.Width = Unit.Percentage(6);
        });

        settings.Columns.Add(column =>
        {
            column.FieldName = "Contractor";
            //column.Caption = "Main Performer of Construction";
            column.Caption = ViewResources.SharedStrings.TABuildingConstruction;
            column.HeaderStyle.Font.Bold = true;
            //column.Width = Unit.Percentage(11);
        });

        settings.Columns.Add(column =>
        {
            column.FieldName = "Contract";
            column.Caption = ViewResources.SharedStrings.TaBuilingDate;
            column.HeaderStyle.Font.Bold = true;
            //column.Width = Unit.Percentage(9);
        });

        settings.Columns.Add(column =>
        {
            column.FieldName = "Sum";
            column.Caption = ViewResources.SharedStrings.TaBuildingAmount;
            column.HeaderStyle.Font.Bold = true;
            //column.Width = Unit.Percentage(8);
        });
        settings.Columns.Add(column =>
        {
            column.FieldName = "Name";
            column.Caption = ViewResources.SharedStrings.TaNewRealBuildingname;
            column.HeaderStyle.Font.Bold = true;
            //column.Width = Unit.Percentage(9);
        });

        settings.Columns.Add(column =>
        {
            column.FieldName = "ValidFrom";
            column.Caption = ViewResources.SharedStrings.TaBuildingValidFrom;
            column.ColumnType = MVCxGridViewColumnType.DateEdit;
            //column.CellStyle.Wrap = DefaultBoolean.False;
            column.HeaderStyle.Font.Bold = true;
            //column.Width = Unit.Percentage(6);
            //column.PropertiesEdit.DisplayFormatString = "dd/MM/yyyy";

            DateEditProperties props = column.PropertiesEdit as DateEditProperties;
            props.CalendarProperties.ShowHeader = true;
            props.UseMaskBehavior = true;
            props.CalendarProperties.ShowWeekNumbers = true;
            props.CalendarProperties.ShowClearButton = true;
            props.CalendarProperties.ShowTodayButton = true;
            props.CalendarProperties.EnableYearNavigation = true;
        });

        settings.Columns.Add(column =>
        {
            column.FieldName = "ValidTo";
            column.Caption = ViewResources.SharedStrings.TabuilidngValidTo;
            column.ColumnType = MVCxGridViewColumnType.DateEdit;
            //column.CellStyle.Wrap = DefaultBoolean.False;
            //column.Width = Unit.Percentage(6);
            column.HeaderStyle.Font.Bold = true;
            //column.PropertiesEdit.DisplayFormatString = "dd/MM/yyyy";

            DateEditProperties props = column.PropertiesEdit as DateEditProperties;
            props.CalendarProperties.ShowHeader = true;
            props.UseMaskBehavior = true;
            props.CalendarProperties.ShowWeekNumbers = true;
            props.CalendarProperties.ShowClearButton = true;
            props.CalendarProperties.ShowTodayButton = true;
            props.CalendarProperties.EnableYearNavigation = true;
        });
        settings.Columns.Add(column =>
        {
            column.FieldName = "Address";
            column.Caption = ViewResources.SharedStrings.TABuildingAddress;
            column.HeaderStyle.Font.Bold = true;
            //column.Width = Unit.Percentage(7);
        });
        settings.Columns.Add(column =>
        {
            column.FieldName = "BuildingLicense";
            column.Caption = ViewResources.SharedStrings.TaBuildingLicense;
            column.HeaderStyle.Font.Bold = true;
            //column.Width = Unit.Percentage(6);
        });
        settings.Columns.Add(column =>
        {
            column.FieldName = "CadastralNr";
            column.Caption = ViewResources.SharedStrings.TaBuildingCadsnrNo;
            column.HeaderStyle.Font.Bold = true;
            //column.Width = Unit.Percentage(6);
        });

        settings.Columns.Add(column =>
        {
            column.FieldName = "Lat";
            column.Caption = ViewResources.SharedStrings.Latitude;
            column.HeaderStyle.Font.Bold = true;
            //column.Width = Unit.Percentage(6);
        });

        settings.Columns.Add(column =>
        {
            column.FieldName = "Lng";
            column.Caption = ViewResources.SharedStrings.Longitude;
            column.HeaderStyle.Font.Bold = true;
            //column.Width = Unit.Percentage(6);
        });



        settings.SettingsBehavior.ConfirmDelete = true;
        settings.SettingsText.ConfirmDelete = "Do you really want to delete this record ?";
        


        settings.CellEditorInitialize = (s, e) =>
        {
            ASPxEdit editor = (ASPxEdit)e.Editor;
            editor.ValidationSettings.Display = Display.None;
            //settings.ClientSideEvents.BeginCallback = "showBuildingNameList";
          
        };
        settings.ClientSideEvents.EndCallback = "function(s,e){RefreshGrid(); }";
        settings.ClientSideEvents.BeginCallback = "function(s,e){testgrid(); }";
        settings.ClientSideEvents.RowClick = "function(s,e){refreshMSG(s,e); }";
    }
).Bind(Model.BuildingData).GetHtml();
%>
<script>

    var i = 0;
    function RefreshGrid() {
        if (i == 0) {
            myGridView.Refresh();


        }
        i++;

    }
    function testgrid() {

        i = 0;

    }

    function refreshMSG(s, e) {
        //alert();
        if (!$("#divmsg").is(':visible')) {
            $("#divmsg").show();
        }
        else {
            $("#divmsg").hide();
        }

    }

</script>

<br />
<div id="divmsg" style="display: none;">
    <label id="alert1" style="color: red">* date format is in mm/dd/yyyy</label>


</div>

<%--<div id="divid2" >
    <p id="abc" style="color:red">* date format is in mm/dd/yyyy</p>
</div>--%>

<%--<script>
    function clearlabel() {
        $("#divid2").hide();
        $("#abc").hide();
        $("#alert1").text("")
        $("#alert1").text("* date format is in mm/dd/yyyy")
    }
</script>--%>