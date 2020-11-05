<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.AddNewTaShiftTimeInterval>" %>
   
        <tr class="NewtaShiftTimeIntervals" id="lalaLand">
            <td style="width:2%"><span class="ui-icon ui-icon-closethick" id="findParentClass" style="cursor:pointer;" onclick="deleteTaShiftTimeIntervalRow()"></span></td>
            <td><input type="text" class="taShiftTimeIntervalName" /></td>
            <td><input type="time" class="shiftIntervalStartTime" /></td>
            <td><input type="time" class="shiftIntervalEndTime" /></td>
            <td><%=Html.DropDownList("TaReportLabel", Model.taReportItem, "--Select TaReportLabel--", new { @class = "taReportlabel" }) %></td>
        </tr>
 