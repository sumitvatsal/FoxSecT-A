<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.HolidayEditViewModel>" %>

<div id="content_edit_holiday_form" style='margin:10px; text-align:center;' >
<form id="editHolidayTable">
<table width="100%">
<%=Html.Hidden("Holiday.Id", Model.Holiday.Id)%>
<tr>
    <td style='width:30%; padding:0 5px; text-align:right;'>
        <label for='Edit_holiday_title'><%:ViewResources.SharedStrings.HolidaysHolidayTitle %></label>
    </td>
    <td style='width:70%; padding:0 5px;'>
		<%=Html.TextBox("Holiday.Name", Model.Holiday.Name, new { @id = "Add_holiday_title", @style = "width:80%;text-transform: capitalize;" })%>
		<%= Html.ValidationMessage("Holiday.Name", null, new { @class = "error" })%>
	</td>
</tr>
<tr>
    <td style='width:30%; padding:0 5px; text-align:right;'>
        <label for='Edit_holiday_duration'><%:ViewResources.SharedStrings.HolidaysHolidayDate %></label>
    </td>
   <td style='width:70%; padding:0 5px;'>
		<%=Html.TextBox("Holiday.EventStartStr", Model.Holiday.EventStartStr, new { @id = "Add_holiday_duration", @style = "width:90px", @class = "date_start" })%><%-- - <%=Html.TextBox("Holiday.EventEnd", Model.Holiday.EventEnd.ToString("dd.MM.yyyy"), new { @style = "width:90px", @class = "date_end" })%>--%>
		<%= Html.ValidationMessage("Holiday.EventStartStr", null, new { @class = "error" })%>
	</td>
</tr>
<%--<tr>
    <td style='width:30%; padding:0 5px; text-align:right; vertical-align:top;'><label for='Edit_holiday_moving_<%= Model.Holiday.Id%>'>Holiday moving</label></td>
    <td style='width:30px; padding:0 5px;'><%=Html.CheckBox("Holiday.MovingHoliday", Model.Holiday.MovingHoliday)%></td>
</tr>--%>
<tr>
    <td style='width:30%; padding:0 5px; text-align:right;'>
        <label for='Edit_holiday_title'><%:ViewResources.SharedStrings.HolidaysHolidayTitle %></label>
    </td>
    <td style='width:70%; padding:0 5px;'>
         <%=Html.CheckBox(string.Format("AllBuildings"), Model.AllBuildings, new { @id=Model.AllBuildings, @onclick="OnChangeCheckbox(this)"}) %> 

        For all buildings
	</td>
</tr>
<tr id="Detail_buildings">
     <td style='width:30%; padding:0 5px; text-align:right;'>
        
    </td>
    <td style='width:70%; padding:0 5px;'>
            <%int index = 0; foreach (var cb in Model.BuildingItems){  %>
        <%=Html.CheckBox(string.Format("BuildingItems[{0}].Selected", index), cb.Selected, new { @id=cb.Value}) %>   
          <%=Html.Hidden(string.Format("BuildingItems[{0}].Text", index), cb.Text)%>
          <%=Html.Hidden(string.Format("BuildingItems[{0}].Value", index), cb.Value)%>
            <%:Html.Encode(cb.Text)%>
        <br />
            <%index++;} %>
            <%= Html.ValidationMessage("Company.CompanyBuildingItems", null, new { @class = "error" })%>
	</td>
</tr>

</table>
 
</form>
</div>

<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        var hb = <%=Model.BuildingItems.Where(x => x.Selected).Count()%>;
        if(hb == 0){
            document.getElementById("Detail_buildings").style.visibility = "hidden";
        }
    });

    $(".date_start").datepicker({
        dateFormat: "dd.mm.yy",
        firstDay: 1,
        showButtonPanel: true,
        changeMonth: true,
        changeYear: true,
        minDate: 'Y'
        //        onSelect: function (dateText, inst) {
        //            $(".date_start").datepicker("option", "minDate", dateText);
        //        }
    });

    //    $(".date_end").datepicker({
    //        dateFormat: "dd.mm.yy",
    //        firstDay: 1,
    //        showButtonPanel: true,
    //        changeMonth: true,
    //        changeYear: true,
    //        minDate: $(".date_end").val()
    //    });
    function OnChangeCheckbox(checkbox) {
        if (checkbox.checked) {
            document.getElementById("Detail_buildings").style.visibility = "hidden";
        }
        else {
            document.getElementById("Detail_buildings").style.visibility = "visible";
        }
    }

</script>