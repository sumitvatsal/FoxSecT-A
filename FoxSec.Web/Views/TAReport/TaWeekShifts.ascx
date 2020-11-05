<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.TaWeekShiftsModel>" %>

<%--<%
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
            comboSetting.Name = "MondayShiftCombo";
            comboSetting.Properties.TextField = "Name";
            comboSetting.Properties.ValueField = "Id";
            comboSetting.Properties.ValueType = typeof(int);
            comboSetting.Properties.DataSource = Model.TaShiftsModelsList;
   
        });
        //settings.Items.Add(i => i.TuesdayShift);
        settings.Items.Add(i =>
        {
            i.Name = "TuesdayShiftValue";
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
            i.Name = "WednesdayShiftValue";
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
            i.Name = "ThursdayShiftValue";
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
            i.Name = "FridayShiftValue";
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
            i.Name = "SaturdayShiftValue";
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
            i.Name = "SundayShiftValue";
            i.FieldName = "SundayShift";
            i.NestedExtensionType = FormLayoutNestedExtensionItemType.ComboBox;
            var comboSetting = i.NestedExtensionSettings as ComboBoxSettings;
            comboSetting.Properties.TextField = "Name";
            comboSetting.Properties.ValueField = "Id";
            comboSetting.Properties.ValueType = typeof(int);
            comboSetting.Properties.DataSource = Model.TaShiftsModelsList;

        });
      
    }).GetHtml();
 %>--%>

<table>
    <tbody>
        <tr>
            <td style='width:40%; padding:2px;'><label>Name</label></td>
            <td style='width:40%; padding:2px;'><input type="text" id="weekShiftName" /></td>
        </tr>
        <tr>
            <td style='width:40%; padding:2px;'><label>Monday Shift</label></td>
            <td style='width:40%; padding:2px;'><%=Html.DropDownList("MondayDropdownSelectedValue", Model.DropDownItems,"--Select week shift--", new { Id = "MondayDropdownId" }) %></td>
        </tr>
        <tr>
            <td style='width:40%; padding:2px;'><label>Tuesday Shift</label></td>
            <td style='width:40%; padding:2px;'><%=Html.DropDownList("TuesdayDropdownSelectedValue", Model.DropDownItems,"--Select week shift--", new { Id = "TuesdayDropdownId" }) %></td>
        </tr>
        <tr>
            <td style='width:40%; padding:2px;'><label>Wednesday Shift</label></td>
            <td style='width:40%; padding:2px;'><%=Html.DropDownList("WednesdayDropdownSelectedValue", Model.DropDownItems,"--Select week shift--", new { Id = "WednesdayDropdownId" }) %></td>
        </tr>
        <tr>
            <td style='width:40%; padding:2px;'><label>Thursday Shift</label></td>
            <td style='width:40%; padding:2px;'><%=Html.DropDownList("ThursdayDropdownSelectedValue", Model.DropDownItems,"--Select week shift--", new { Id = "ThursdayDropdownId" }) %></td>
        </tr>
        <tr>
            <td style='width:40%; padding:2px;'><label>Friday Shift</label></td>
            <td style='width:40%; padding:2px;'><%=Html.DropDownList("FridayDropdownSelectedValue", Model.DropDownItems,"--Select week shift--", new { Id = "FridayDropdownId" }) %></td>
        </tr>
        <tr>
            <td style='width:40%; padding:2px;'><label>Saturday Shift</label></td>
            <td style='width:40%; padding:2px;'><%=Html.DropDownList("SaturdayDropdownSelectedValue", Model.DropDownItems,"--Select week shift--", new { Id = "SaturdayDropdownId" }) %></td>
        </tr>
        <tr>
            <td style='width:40%; padding:2px;'><label>Sunday Shift</label></td>
            <td style='width:40%; padding:2px;'><%=Html.DropDownList("SundayDropdownSelectedValue", Model.DropDownItems,"--Select week shift--", new { Id = "SundayDropdownId" }) %></td>
        </tr>
    </tbody>
</table>

<%--<%
    Html.DevExpress().Button(settings =>
    {
        settings.Name = "SaveButton";
        settings.Text = "Save";
        settings.ClientSideEvents.Click = "saveValues";
    }).GetHtml();
    %>--%>
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