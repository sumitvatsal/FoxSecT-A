<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.HolidayListViewModel>" %>

<table id="list_table" cellpadding="1" cellspacing="0" style="margin:0; width:100%; padding:1px; border-spacing:0;">
<tr>
    <th style='width:30%; padding:2px; text-align: left;'><%:ViewResources.SharedStrings.HolidaysHolidayDate %></th>
	<th style='width:30%; padding:2px; text-align: left;'><%:ViewResources.SharedStrings.UsersName %></th>
    <th style='width:30%; padding:2px; text-align: left;'><%:ViewResources.SharedStrings.HolidaysMovingHoliday %></th>
    <th style='width:5%; padding:2px; text-align: left;'></th>
    <th style='width:5%; padding:2px; text-align: left;'></th>
</tr>

<% var i = 1; foreach (var holiday in Model.Holidays) { var bg = (i++ % 2 == 1) ? " background-color:#CCC;" : ""; %>

<tr>
    <td style='width:30%; padding:2px;<%= bg %>'><%= Html.Encode(holiday.EventStart.ToString("dd.MM.yyyy"))%></td>
    <td style='width:30%; padding:2px;<%= bg %>'><%= Html.Encode(holiday.Name != null && holiday.Name.Length > 30 ? holiday.Name.Substring(0, 27) + "..." : holiday.Name)%> </td>
    <td style='width:30%; padding:2px;<%= bg %>'><input id = "moving_holiday_check_<%= holiday.Id %>" type="checkbox" onclick="javascript:SaveMoving('<%= holiday.Id %>');" <% if(holiday.MovingHoliday) {%>  checked='checked' <% } %> /> </td>
    <td style='width:5%; padding:2px; text-align:right;<%= bg %>'><span id='button_holiday_edit_<%= holiday.Id %>' class='icon icon_green_go tipsy_we' original-title='<%=string.Format("{0} {1}!", ViewResources.SharedStrings.BtnEdit, Html.Encode(holiday.Name)) %>' onclick='<%=string.Format("javascript:EditHoliday(\"submit_edit_hoilday\", {0}, \"{1}\")", holiday.Id, Html.Encode(holiday.Name)) %>' ></span></td>
    <td style='width:5%; padding:2px; text-align:right;<%= bg %>'><span id='button_holiday_delete_<%= holiday.Id %>' class="ui-icon ui-icon-closethick tipsy_we" style="cursor:pointer" original-title='<%=string.Format("{0} {1}!",ViewResources.SharedStrings.BtnDelete, Html.Encode(holiday.Name)) %>' onclick="javascript:DeleteHoliday(<%= holiday.Id %>,'<%= holiday.Name %>');"></span></td>
</tr>
<% } %>
</table>

<script type="text/javascript" language="javascript">
	$(document).ready(function () {
		$(".tipsy_we").attr("class", function () {
			$(this).tipsy({ gravity: $.fn.tipsy.autoWE, html: true });
		});
	});
	
	function EditHoliday(Action, HolidayId, HolidayTitle) {
	    $("div#modal-dialog").dialog({
	        open: function () {
	            $("div#modal-dialog").html("");
	            $.get('/Holiday/Edit', { id: HolidayId }, function(html) {
	                $("div#modal-dialog").html(html);
	            });
	        },
	        resizable: false,
	        width: 480,
	        height: 340,
	        modal: true,
	        title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + HolidayTitle,
	        buttons: {
	            '<%=ViewResources.SharedStrings.BtnOk %>': function () {

	                dlg = $(this);
	                $.ajax({
	                    type: "Post",
	                    url: "/Holiday/Edit",
	                    dataType: 'json',
	                    traditional: true,
	                    data: $("#editHolidayTable").serialize(),
	                    success: function (data) {
	                        if (data.IsSucceed == false) {
	                            $("div#modal-dialog").html(data.viewData);
	                            if (data.DisplayMessage == true) {
	                                ShowDialog(data.Msg, 2000);
	                            }
	                        }
	                        else {
	                            ShowDialog(data.Msg, 2000, true);
	                            setTimeout(function () {
	                                $.ajax({
	                                    type: 'GET',
	                                    url: '/Holiday/List',
	                                    cache: false,
	                                    success: function (html) {
	                                        $("div#HolidaysList").html(html);
	                                    }
	                                });
	                            }, 1000);
	                            dlg.dialog("close");
	                        }
	                    }
	                });
	            },
	            '<%=ViewResources.SharedStrings.BtnCancel %>': function () {
	                $(this).dialog("close");
	            }
	        }
	    });

        return false;
    }

    function DeleteHoliday(HolidayId, HolidayTitle) {

        $("div#modal-dialog").dialog({
            open: function (event, ui) {
                $("div#modal-dialog").html('<%=ViewResources.SharedStrings.CommonConfirmMessage %>');
            },
            resizable: false,
            width: 240,
            height: 140,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span> Deleting " + HolidayTitle,
            buttons: {
                '<%=ViewResources.SharedStrings.BtnOk %>': function () {
                    $.post("/Holiday/Delete", { id: HolidayId });
                    ShowDialog('<%=ViewResources.SharedStrings.HolidaysDeletingMessage %>' + HolidayTitle + "...", 2000, true);

                    setTimeout(function () {
                        $.get('/Holiday/List', function (html) {
                            $("div#HolidaysList").html(html);
                        });
                    }, 1000);
                    $(this).dialog("close");
                },
                '<%=ViewResources.SharedStrings.BtnCancel %>': function () {
                    $(this).dialog("close");
                }
            }
        });

        return false;
    }

    function SaveMoving(holidayId) {
        var isChecked = $("#moving_holiday_check_" + holidayId).is(":checked");
        $.post('/Holiday/SaveMoving', { holidayId: holidayId, isChecked: isChecked }, function (html) { });
    }

</script>