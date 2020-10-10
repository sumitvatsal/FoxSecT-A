<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.TaWeekShiftsModel>" %>

<%
    Html.DevExpress().FormLayout(settings =>
    {
        settings.Name = "formLayout";
        settings.Items.Add(i => i.Name);
        //settings.Items.Add(i => i.MondayShift);
         settings.Items.Add(i =>
        {
            i.Name = "MondayShiftValue";
            i.FieldName = "MondayShift";
            i.NestedExtensionType = FormLayoutNestedExtensionItemType.ComboBox;
            var comboSetting = i.NestedExtensionSettings as ComboBoxSettings;
            comboSetting.Properties.TextField = "Name";
            comboSetting.Properties.ValueField = "Id";
            comboSetting.Properties.ValueType = typeof(int);
            comboSetting.Properties.DataSource = Model.TaShiftsModelsList;

        });
        //settings.Items.Add(i => i.TuesdayShift);
         settings.Items.Add(i =>
        {
            i.FieldName = "TuesdayShift";
            i.NestedExtensionType = FormLayoutNestedExtensionItemType.ComboBox;
            var comboSetting = i.NestedExtensionSettings as ComboBoxSettings;
            comboSetting.Properties.TextField = "Name";
            comboSetting.Properties.ValueField = "Id";
            comboSetting.Properties.ValueType = typeof(int);
            comboSetting.Properties.DataSource = Model.TaShiftsModelsList;

        });
        //settings.Items.Add(i => i.WednesdayShift);
        settings.Items.Add(i =>
        {
            i.FieldName = "WednesdayShift";
            i.NestedExtensionType = FormLayoutNestedExtensionItemType.ComboBox;
            var comboSetting = i.NestedExtensionSettings as ComboBoxSettings;
            comboSetting.Properties.TextField = "Name";
            comboSetting.Properties.ValueField = "Id";
            comboSetting.Properties.ValueType = typeof(int);
            comboSetting.Properties.DataSource = Model.TaShiftsModelsList;

        });
        //settings.Items.Add(i => i.ThursdayShift);
        settings.Items.Add(i =>
        {
            i.FieldName = "ThursdayShift";
            i.NestedExtensionType = FormLayoutNestedExtensionItemType.ComboBox;
            var comboSetting = i.NestedExtensionSettings as ComboBoxSettings;
            comboSetting.Properties.TextField = "Name";
            comboSetting.Properties.ValueField = "Id";
            comboSetting.Properties.ValueType = typeof(int);
            comboSetting.Properties.DataSource = Model.TaShiftsModelsList;

        });
        //settings.Items.Add(i => i.FridayShift);
        settings.Items.Add(i =>
        {
            i.FieldName = "FridayShift";
            i.NestedExtensionType = FormLayoutNestedExtensionItemType.ComboBox;
            var comboSetting = i.NestedExtensionSettings as ComboBoxSettings;
            comboSetting.Properties.TextField = "Name";
            comboSetting.Properties.ValueField = "Id";
            comboSetting.Properties.ValueType = typeof(int);
            comboSetting.Properties.DataSource = Model.TaShiftsModelsList;

        });
        //settings.Items.Add(i => i.SaturdayShift);
        settings.Items.Add(i =>
        {
            i.FieldName = "SaturdayShift";
            i.NestedExtensionType = FormLayoutNestedExtensionItemType.ComboBox;
            var comboSetting = i.NestedExtensionSettings as ComboBoxSettings;
            comboSetting.Properties.TextField = "Name";
            comboSetting.Properties.ValueField = "Id";
            comboSetting.Properties.ValueType = typeof(int);
            comboSetting.Properties.DataSource = Model.TaShiftsModelsList;

        });
        //settings.Items.Add(i => i.SundayShift);
        settings.Items.Add(i =>
        {
            i.FieldName = "SundayShift";
            i.NestedExtensionType = FormLayoutNestedExtensionItemType.ComboBox;
            var comboSetting = i.NestedExtensionSettings as ComboBoxSettings;
            comboSetting.Properties.TextField = "Name";
            comboSetting.Properties.ValueField = "Id";
            comboSetting.Properties.ValueType = typeof(int);
            comboSetting.Properties.DataSource = Model.TaShiftsModelsList;

        });

    }).GetHtml();
 %>
<%
    Html.DevExpress().Button(settings =>
    {
        settings.Name = "SaveButton";
        settings.Text = "Save";
        settings.ClientSideEvents.Click = "saveValues";
    }).GetHtml();
    %>
<%--<table>
    <thead>

    </thead>
    <tbody>
        <tr>
            <td></td>
            <td><input type="button" id="saveButton" value="Save" /></td>
        </tr>
    </tbody>
</table>--%>

<script>
    function saveValues(s, e) {
        alert(MondayShiftValue.GetValue());
    }
</script>