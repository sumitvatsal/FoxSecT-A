<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.TAReportLabelsModel>" %> 

<% 
    Html.DevExpress<FoxSec.Web.ViewModels.TAReportLabelsModel>().GridView(settings =>
    {

        settings.Name = "TaReportLabelsGridView";
        settings.CallbackRouteValues = new { Controller = "TAReport", Action = "TAReportLabels" };
        settings.SettingsSearchPanel.Visible = true;
        settings.CommandColumn.ShowEditButton = true;
        settings.SettingsSearchPanel.ShowClearButton = true;
        settings.SettingsEditing.Mode = GridViewEditingMode.Batch;
        //settings.SettingsEditing.BatchEditSettings.EditMode = GridViewBatchEditMode.Cell;
        settings.SettingsEditing.BatchEditSettings.StartEditAction = GridViewBatchStartEditAction.Click;
        settings.CommandColumn.Visible = true;
        settings.CommandColumn.ShowNewButtonInHeader = true;
        settings.CommandColumn.ShowDeleteButton = true;
        settings.SettingsBehavior.ConfirmDelete = true;
        settings.SettingsText.ConfirmDelete = ViewResources.SharedStrings.TaReportLabelDeleteConfirmMessage;
        settings.SettingsEditing.BatchUpdateRouteValues = new { Controller = "TAReport", Action = "TaReportLabelsUpdate" };
        settings.SettingsEditing.DeleteRowRouteValues = new { Controller = "TAReport", Action = "TaReportLabelsDelete" };
        settings.SettingsEditing.AddNewRowRouteValues = new { controller = "TAReport", Action = "TaReportLabelAddNewRecord" };
        settings.ClientSideEvents.RowClick = "function(s,e){showLabelMessage(s,e);}";
        //settings.SettingsEditing.Mode = GridViewEditingMode.PopupEditForm;
        //settings.SettingsPopup.EditForm.SettingsAdaptivity.Mode = PopupControlAdaptivityMode.Always;
        settings.SettingsPopup.CustomizationWindow.ShowCloseButton = true;
        settings.SettingsPopup.EditForm.PopupAnimationType = DevExpress.Web.AnimationType.Slide;
        settings.SettingsPopup.EditForm.SettingsAdaptivity.Mode = PopupControlAdaptivityMode.Off;
        settings.SettingsPopup.EditForm.Width = 900;
        settings.SettingsPopup.CustomizationWindow.HorizontalAlign = PopupHorizontalAlign.WindowCenter;
        settings.SettingsEditing.Mode = GridViewEditingMode.PopupEditForm;

        settings.KeyFieldName = "Id";

        //settings.Columns.Add("Label");
        settings.Columns.Add(column =>
        {
            column.FieldName = "Label";
            column.Caption = ViewResources.SharedStrings.TaReportLabel;
            column.ColumnType = MVCxGridViewColumnType.TextBox;
        });
        //settings.Columns.Add("Name");
        settings.Columns.Add(column =>
        {
            column.FieldName = "Name";
            column.Caption = ViewResources.SharedStrings.Name;
            column.ColumnType = MVCxGridViewColumnType.TextBox;
        });
        //settings.Columns.Add("JobNotMove").ColumnType = MVCxGridViewColumnType.CheckBox;
        settings.Columns.Add(column =>
        {
            column.FieldName = "JobNotMove";
            column.Caption = ViewResources.SharedStrings.TaReportLabelJobNotMove;
            column.ColumnType = MVCxGridViewColumnType.CheckBox;
        });
        //settings.Columns.Add("Fixed").ColumnType = MVCxGridViewColumnType.CheckBox;
        settings.Columns.Add(column =>
        {
            column.FieldName = "Fixed";
            column.Caption = ViewResources.SharedStrings.TaReportLabelFixed;
            column.ColumnType = MVCxGridViewColumnType.CheckBox;
        });
        //settings.Columns.Add("ValidFrom").ColumnType = MVCxGridViewColumnType.DateEdit;
        settings.Columns.Add(column =>
        {
            column.FieldName = "ValidFrom";
            column.Caption = ViewResources.SharedStrings.TaReportLabelValidFrom;
            column.ColumnType = MVCxGridViewColumnType.DateEdit;
        });

        //settings.Columns.Add("ValidTo").ColumnType = MVCxGridViewColumnType.DateEdit;
        settings.Columns.Add(column =>
        {
            column.FieldName = "ValidTo";
            column.Caption = ViewResources.SharedStrings.TaReportLabelValidTo;
            column.ColumnType = MVCxGridViewColumnType.DateEdit;
        });
        //settings.Columns.Add("At_work").ColumnType = MVCxGridViewColumnType.CheckBox;
        settings.Columns.Add(column =>
        {
            column.FieldName = "At_work";
            column.Caption = ViewResources.SharedStrings.Atwork;
            column.ColumnType = MVCxGridViewColumnType.CheckBox;
        });
        //settings.Columns.Add("InBuilding").ColumnType = MVCxGridViewColumnType.CheckBox;
        settings.Columns.Add(column =>
        {
            column.FieldName = "InBuilding";
            column.Caption = ViewResources.SharedStrings.TaReportLabelInBuilding;
            column.ColumnType = MVCxGridViewColumnType.CheckBox;
        });
        //settings.Columns.Add("Allow_Jobs").ColumnType = MVCxGridViewColumnType.CheckBox;
        settings.Columns.Add(column =>
        {
            column.FieldName = "Allow_Jobs";
            column.Caption = ViewResources.SharedStrings.TaReportLabelAllowJobs;
            column.ColumnType = MVCxGridViewColumnType.CheckBox;
        });
        //settings.Columns.Add("DaysNotHours").ColumnType = MVCxGridViewColumnType.CheckBox;
        settings.Columns.Add(column =>
        {
            column.FieldName = "DaysNotHours";
            column.Caption = ViewResources.SharedStrings.TaReportLabelDaysNotHours;
            column.ColumnType = MVCxGridViewColumnType.CheckBox;
        });
        //settings.Columns.Add("Admin_only").ColumnType = MVCxGridViewColumnType.CheckBox;
        settings.Columns.Add(column =>
        {
            column.FieldName = "Admin_only";
            column.Caption = ViewResources.SharedStrings.TaReportLabelAdminonly;
            column.HeaderStyle.Font.Bold = true;
            column.ColumnType = MVCxGridViewColumnType.CheckBox;
        });
        settings.Columns.Add(column =>
        {
            column.FieldName = "CompanyId";
            column.Caption = ViewResources.SharedStrings.TaReportLabelCompanyId;
            column.ColumnType = MVCxGridViewColumnType.ComboBox;
            var comboBoxProperties = column.PropertiesEdit as ComboBoxProperties;
            comboBoxProperties.DataSource = Model.CompaniesList;
            comboBoxProperties.TextField = "Name";
            comboBoxProperties.ValueField = "Id";
            comboBoxProperties.ValueType = typeof(int);
            comboBoxProperties.ClientSideEvents.ButtonClick = "function(s,e){s.ShowDropown();}";

        });

        settings.ClientSideEvents.EndCallback = "function(){ RefreshGrid(); }";
        settings.ClientSideEvents.BeginCallback = "function(){ SetCountToZeroForRefreshAgain(); }";
        settings.ClientSideEvents.RowClick = "function(){ showLabelMessage(); }";

    }).Bind(Model.TAReportLabels).GetHtml();
%>
<script type="text/javascript">
     var count = 0;
    function RefreshGrid() {
        if (count == 0) {
            TaReportLabelsGridView.Refresh();
        }
        count++;
    }

    function SetCountToZeroForRefreshAgain() {
        count = 0;
    }

    function showLabelMessage(s,e) {
        if ($("#dateMessage").is(":visible")) {
            $("#dateMessage").show();
        }
        else {
            $("#dateMessage").hide();
        }
    }
</script>
<div id="dateMessage" style="display:none">
    <label style="color:red">* Date Format is in "mm/dd/yyyy"</label>
</div>