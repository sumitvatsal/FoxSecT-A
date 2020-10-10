<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<table cellpadding="0" cellspacing="1" style="margin: 0; width: 100%; padding: 0; border-spacing: 1px; font-size: 8px">
    <tr>
        <td style="width: 250px">
            <label for='SearchTimeZoneByName'><%:ViewResources.SharedStrings.UsersName %>:</label><%= Html.TextBox("SearchTimeZoneName", "", new { @style = "width:180px;text-transform: capitalize;" })%>
        </td>
        <td style="width: 175px">
            <label for='SearchTimeZoneStartTime'><%:ViewResources.SharedStrings.CommonStartTime %>:</label><%= Html.TextBox("SearchTimeZoneStartTime", "", new { @style = "width:74px" })%> 
        </td>
        <td style="width: 50px; vertical-align: top; padding-top: 2px">
            <span id='button_submit_time_zone_search' class='icon icon_find tipsy_we' original-title='<%=ViewResources.SharedStrings.TimeZonesSearch %>' onclick="javascript:SubmitTimeZoneSearch();"></span>
        </td>
        <td style="text-align: right">
            <input type='button' id='button_add_time_zone' value='<%=ViewResources.SharedStrings.TimeZonesNewTimeZone %>' onclick="javascript: AddTimeZone();" /></td>
    </tr>
</table>
<br />
<br />
<div id="TimeZoneList" style="display: none"></div>
<br />

<script type="text/javascript" language="javascript">

    $(document).ready(function () {
        $.ajax({
            type: "Post",
            url: "/TimeZone/UserRole",
        });
        var i = $('#panel_owner li').index($('#timeZoneTab'));

        var opened_tab = '<%: Session["tabIndex"] %>';

        if (opened_tab != '') {
            i1 = new Number(opened_tab);
            if (i1 != i) {
                SetOpenedTab(i);
            }
        }
        else {
            SetOpenedTab(i);
        }
        $("input:button").button();
        $('div#Work').fadeIn("slow");
        SubmitTimeZoneSearch();
    });

    function AddTimeZone() {
        $("div#modal-dialog").dialog({
            open: function () {
                $("div#modal-dialog").html("");
                $.get('/TimeZone/Create', {}, function (html) {
                    $("div#modal-dialog").html(html);
                });
            },
            resizable: false,
            width: 440,
            height: 190,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.TimeZonesAddNewTimeZone %>',
            buttons: {
                '<%=ViewResources.SharedStrings.BtnSave %>': function () {
                    dlg = $(this);
                    $.ajax({
                        type: "Post",
                        url: "/TimeZone/Create",
                        dataType: 'json',
                        traditional: true,
                        data: $("#createNewTimeZone").serialize(),
                        success: function (data) {
                            if (data.IsSucceed == false) {
                                if (data.Msg == "licence error") {
                                    ShowDialog('<%=ViewResources.SharedStrings.CommonRemainigCountErrorMsg %>', 5000);
                                }
                                $("div#modal-dialog").html(data.viewData);
                                if (data.DisplayMessage == true) {
                                    ShowDialog(data.Msg, 2000);
                                }
                            }
                            else {
                                ShowDialog(data.Msg, 2000, true);
                                $("div#modal-dialog").html("");
                                dlg.dialog("close");
                                setTimeout(function () { SubmitTimeZoneSearch(); }, 1000);
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

    function EditTimeZone(id, tzName) {
        $("div#modal-dialog").dialog({
            open: function () {
                $("div#modal-dialog").html("");
                $.get('/TimeZone/Edit', { id: id }, function (html) {
                    $("div#modal-dialog").html(html);
                });
            },
            resizable: false,
            width: 440,
            height: 170,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.TimeZonesChangeNameTitle %>' + ' ' + tzName,
            buttons: {
                '<%=ViewResources.SharedStrings.BtnSave %>': function () {
                    dlg = $(this);
                    $.ajax({
                        type: "Post",
                        url: "/TimeZone/Edit",
                        dataType: 'json',
                        traditional: true,
                        data: $("#editTimeZone").serialize(),
                        success: function (data) {
                            if (data.IsSucceed == false) {
                                $("div#modal-dialog").html(data.viewData);
                                if (data.DisplayMessage == true) {
                                    ShowDialog(data.Msg, 2000);
                                }
                            }
                            else {
                                ShowDialog(data.Msg, 2000, true);
                                $("div#modal-dialog").html("");
                                dlg.dialog("close");
                                setTimeout(function () { SubmitTimeZoneSearch(); }, 1000);
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

    function RemoveTimeZone(id, tzName) {
        $("div#modal-dialog").dialog({
            open: function (event, ui) {
                $("div#modal-dialog").html('<%=ViewResources.SharedStrings.CommonConfirmMessage %>');
            },
            resizable: false,
            width: 240,
            height: 140,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.TimeZinesDeleteTitle %>' + ' ' + tzName,
            buttons: {
            	'<%=ViewResources.SharedStrings.BtnOk %>': function () {
                    $("tr#timeZone_" + id).hide();
                    ShowDialog('<%=ViewResources.SharedStrings.TimeZonesRemovingMessage %>', 2000, true);
                    $.post("/TimeZone/Delete", { id: id }, null);

                    setTimeout(function () { SubmitTimeZoneSearch(); }, 1000);
                    $(this).dialog("close");
                },
                   '<%=ViewResources.SharedStrings.BtnCancel %>': function () {
                    $(this).dialog("close");
                }
            }
        });
        return false;
    }

    var tz_page = 0;
    var tz_rows = 10;

    function SubmitTimeZoneSearch() {
        Name = $("input#SearchTimeZoneName").val();
        StartTime = $("input#SearchTimeZoneStartTime").val();
        if (StartTime.length == 1) { StartTime = "0" + StartTime + ":00"; $("input#SearchTimeZoneStartTime").val(StartTime); }
        else
            if (StartTime.length == 2) { StartTime = StartTime + ":00"; $("input#SearchTimeZoneStartTime").val(StartTime); }
        $.ajax({
            type: "Post",
            url: "/TimeZone/Search",
            data: { name: Name, start: StartTime, nav_page: tz_page, rows: tz_rows },
            beforeSend: function () {
                $("#button_submit_time_zone_search").addClass("Trans");
            },
            success: function (result) {
                $("div#TimeZoneList").html(result);
                $("#button_submit_time_zone_search").removeClass("Trans");
                $("div#TimeZoneList").fadeIn(500);
            }
        });
        return false;
    }

    function HandleTimeZonePaging(page, rows) {
        tz_page = page;
        tz_rows = rows;
        SubmitTimeZoneSearch();
        return false;
    }

</script>
