<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.HolidayEditViewModel>" %>
<div id="content_add_holiday_form" style='margin:10px; text-align:center;' >
<form id="createNewHoliday" action="">
<table width="100%">
<tr>
	<td style='width:30%; padding:0 5px; text-align:right;'><label for='Add_holiday_title'><%:ViewResources.SharedStrings.HolidaysHolidayTitle %></label></td>
	<td style='width:70%; padding:0 5px;'>
		<%=Html.TextBox("Holiday.Name", Model.Holiday.Name, new { @id = "Add_holiday_title", @style = "width:80%;text-transform: capitalize;" })%>
		<%= Html.ValidationMessage("Holiday.Name", null, new { @class = "error" })%>
	</td>
</tr>
<tr>
	<td style='width:30%; padding:0 5px; text-align:right;'><label for='Add_holiday_duration'><%:ViewResources.SharedStrings.HolidaysHolidayDate %></label></td>
	<td style='width:70%; padding:0 5px;'>
		<%=Html.TextBox("Holiday.EventStartStr", Model.Holiday.EventStartStr, new { @id = "Add_holiday_duration", @style = "width:90px", @class = "date_start" })%><%-- - <%=Html.TextBox("Holiday.EventEnd", Model.Holiday.EventEnd.ToString("dd.MM.yyyy"), new { @style = "width:90px", @class = "date_end" })%>--%>
		<%= Html.ValidationMessage("Holiday.EventStartStr", null, new { @class = "error" })%>
	</td>
</tr>
<%--<tr>
    <td style='width:30%; padding:0 5px; text-align:right; vertical-align:top;'><label for='Add_holiday_moving'>Holiday moving</label></td>
    <td style='width:30px; padding:2px;'><%=Html.CheckBox("Holiday.MovingHoliday", Model.Holiday.MovingHoliday)%></td>
</tr>--%>
</table>
</form>
</div>

<script type="text/javascript" language="javascript">

   $(".date_start").datepicker({
        dateFormat: "dd.mm.yy",
        firstDay: 1,
        showButtonPanel: true,
        changeMonth: true,
        changeYear: true,
        gotoCurrent: true,
        minDate: 'Y'
//        onSelect: function (dateText, inst) {
//            $(".date_start").datepicker("option", "minDate", dateText);
//        }
    });

//    $(".date_end").datepicker({
//        dateFormat: "dd-mm-yy",
//        firstDay: 1,
//        showButtonPanel: true,
//        changeMonth: true,
//        changeYear: true,
//        minDate: $(".date_end").val()
//    });

</script>

