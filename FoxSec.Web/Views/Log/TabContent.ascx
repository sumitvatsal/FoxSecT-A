<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.HomeViewModel>" %>

<style type="text/css">
    /* css for timepicker */
    #ui-timepicker-div dl {
        text-align: left;
    }

        #ui-timepicker-div dl dt {
            height: 25px;
        }

        #ui-timepicker-div dl dd {
            margin: -25px 0 10px 65px;
        }

    dl {
        margin-right: 7px;
    }

    .ui-datepicker {
        width: 17em;
        padding: .2em .2em 0;
    }

    .ui-corner-tl {
        -moz-border-radius-topleft: 4px;
        -webkit-border-top-left-radius: 4px;
        border-top-left-radius: 4px;
    }

    .ui-corner-tr {
        -moz-border-radius-topright: 4px;
        -webkit-border-top-right-radius: 4px;
        border-top-right-radius: 4px;
    }

    .ui-corner-bl {
        -moz-border-radius-bottomleft: 4px;
        -webkit-border-bottom-left-radius: 4px;
        border-bottom-left-radius: 4px;
    }

    .ui-corner-br {
        -moz-border-radius-bottomright: 4px;
        -webkit-border-bottom-right-radius: 4px;
        border-bottom-right-radius: 4px;
    }

    .ui-corner-top {
        -moz-border-radius-topleft: 4px;
        -webkit-border-top-left-radius: 4px;
        border-top-left-radius: 4px;
        -moz-border-radius-topright: 4px;
        -webkit-border-top-right-radius: 4px;
        border-top-right-radius: 4px;
    }

    .ui-corner-bottom {
        -moz-border-radius-bottomleft: 4px;
        -webkit-border-bottom-left-radius: 4px;
        border-bottom-left-radius: 4px;
        -moz-border-radius-bottomright: 4px;
        -webkit-border-bottom-right-radius: 4px;
        border-bottom-right-radius: 4px;
    }

    .ui-corner-right {
        -moz-border-radius-topright: 4px;
        -webkit-border-top-right-radius: 4px;
        border-top-right-radius: 4px;
        -moz-border-radius-bottomright: 4px;
        -webkit-border-bottom-right-radius: 4px;
        border-bottom-right-radius: 4px;
    }

    .ui-corner-left {
        -moz-border-radius-topleft: 4px;
        -webkit-border-top-left-radius: 4px;
        border-top-left-radius: 4px;
        -moz-border-radius-bottomleft: 4px;
        -webkit-border-bottom-left-radius: 4px;
        border-bottom-left-radius: 4px;
    }

    .ui-corner-all {
        -moz-border-radius: 4px;
        -webkit-border-radius: 4px;
        border-radius: 4px;
    }


    .ui-slider {
        position: relative;
        text-align: left;
    }

        .ui-slider .ui-slider-handle {
            position: absolute;
            z-index: 2;
            width: 1.2em;
            height: 1.2em;
            cursor: default;
        }

        .ui-slider .ui-slider-range {
            position: absolute;
            z-index: 1;
            font-size: .7em;
            display: block;
            border: 0;
            background-position: 0 0;
        }

    .ui-slider-horizontal {
        height: .8em;
    }

        .ui-slider-horizontal .ui-slider-handle {
            top: -.3em;
            margin-left: -.6em;
        }

        .ui-slider-horizontal .ui-slider-range {
            top: 0;
            height: 100%;
        }

        .ui-slider-horizontal .ui-slider-range-min {
            left: 0;
        }

        .ui-slider-horizontal .ui-slider-range-max {
            right: 0;
        }

    .ui-slider-vertical {
        width: .8em;
        height: 100px;
    }

        .ui-slider-vertical .ui-slider-handle {
            left: -.3em;
            margin-left: 0;
            margin-bottom: -.6em;
        }

        .ui-slider-vertical .ui-slider-range {
            left: 0;
            width: 100%;
        }

        .ui-slider-vertical .ui-slider-range-min {
            bottom: 0;
        }

        .ui-slider-vertical .ui-slider-range-max {
            top: 0;
        }
</style>
<div id='LogContent'>
    <form id="logFilters" action="">
        <table cellpadding="0" cellspacing="1" style="margin: 0; width: 100%; padding: 0; border-spacing: 1px;">
            <tr>
                <td width="20px"></td>
                <td width="300px">&nbsp; <%:ViewResources.SharedStrings.CommonSearch %>:
                    <input type='text' name='CommonSearch' id='Search_log' style='width: 280px;' value='' /></td>
                <td width="230px">
                    <table cellpadding="0" cellspacing="1" style="margin: 0; width: 230px; padding: 0; border-spacing: 1px;">
                        <tr>
                            <td rowspan="2" style="width: 5px"></td>
                            <td width="20px">
                                <%--<%:Html.Hidden("IsShowFullLog")%>
		<input id='show_full_log' type='checkbox' class='tipsy_we' original-title='<%=ViewResources.SharedStrings.LogsShowFull %>' onclick="javascript:ShowFullLog();" />&nbsp; <%:ViewResources.SharedStrings.LogsShowFull %>--%>
                                <%:Html.RadioButton("IsShowDefaultLog" ,"true") %>&nbsp; <%:ViewResources.SharedStrings.LogsShowFull %>
                            </td>
                        </tr>
                        <tr>
                            <td width="200px">
                                <input type="hidden" id="chk1" value="@ViewBag.NotImageFoung" />
                                <%--<%:Html.Hidden("IsShowDefaultLog")%>
		<input id='show_default_log' type='checkbox' class='tipsy_we' original-title='<%=ViewResources.SharedStrings.LogsShowDefault %>' onclick="javascript:ShowDefaultLog();" />&nbsp; <%:ViewResources.SharedStrings.LogsShowDefault %>--%>
                                <div id="rdbsel">
                                    <%:Html.RadioButton("IsShowDefaultLog", "false", new { @checked = true }) %>&nbsp; <%:ViewResources.SharedStrings.LogsShowDefault %>
                                </div>
                                <input type="hidden" id="chk" value="@ViewBag.SomeValue" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table cellpadding="0" cellspacing="1" style="margin: 0; width: 100%; padding: 0; border-spacing: 1px;">
                        <tr>
                            <td style="width: 15%">
                                <%:ViewResources.SharedStrings.UsersName %> :
                            </td>
                            <td style="width: 45%">
                                <%=Html.TextBox("Name", "", new { @style = "width:85%" })%>
                            </td>
                            <td style="width: 20%">
                                <%:Html.Hidden("LogFilterId") %>
                            </td>
                            <td style="width: 20%"></td>
                        </tr>
                        <tr>
                            <td style="width: 15%">
                                <%:ViewResources.SharedStrings.LogsFilter %> :
                            </td>
                            <td style="width: 45%">
                                <select style="width: 90%" id="selectedFilter" onchange="ChangeFilter()"></select>
                            </td>
                            <td style="width: 20%; text-align: center">
                                <input type='button' id='saveFilterBtn' value='<%=ViewResources.SharedStrings.BtnSaveFilter %>' onclick="SaveFilterData()" />
                            </td>
                            <td style="width: 20%; text-align: center">
                                <input type='button' id='delFilterBtn' onclick="DeleteFilterData()" value='<%=ViewResources.SharedStrings.BtnDeleteFilter %>' />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table cellpadding="0" cellspacing="1" style="margin: 0; width: 100%; padding: 0; border-spacing: 1px;">
            <tr>
                <td style="width: 18%; padding-right: 3px">
                    <label style="padding-left: 45px" for='Search_fromdate'><%:ViewResources.SharedStrings.CommonDate %></label><br />
                    <table cellpadding="0" cellspacing="0" style="margin: 0; width: 100%; padding: 0; border-spacing: 0px;">
                        <tr>
                            <td style="width: 25%">
                                <label for='Search_fromdate'><%:ViewResources.SharedStrings.CommonFrom %></label>
                            </td>
                            <td style="width: 75%;">
                                <%:Html.TextBox("FromDate", string.Format("{0} {1}:{2}", DateTime.Now.AddHours(-1).ToString("dd.MM.yyyy"), DateTime.Now.AddHours(-1).Hour.ToString("D2"), DateTime.Now.Minute.ToString("D2")))%>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 25%">
                                <label for='Search_fromdate'><%:ViewResources.SharedStrings.CommonTo %></label>
                            </td>
                            <td style="width: 75%">
                                <%:Html.TextBox("ToDate", string.Format("{0} 00:00", DateTime.Now.AddDays(1).ToString("dd.MM.yyyy"), DateTime.Now.Hour.ToString("D2"), DateTime.Now.Minute.ToString("D2") ))%>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 9%; vertical-align: top; padding-right: 3px">
                    <label for='Search_building'><%:ViewResources.SharedStrings.CommonBuilding %></label><br />
                    <%:Html.TextBox("Building") %>
                </td>
                <td style="width: 9%; vertical-align: top; padding-right: 3px">
                    <label for='Search_node'><%:ViewResources.SharedStrings.LogsNode %></label><br />
                    <%:Html.TextBox("Node", "", new { @style = "margin-right:5px" })%>
                </td>
                <%if (Model.User.CompanyId == null)
                    {%>
                <td style="width: 12%; vertical-align: top; padding-top: 2px">
                    <label for='Search_company'><%:ViewResources.SharedStrings.UsersCompany %></label><br />
                    <select style="width: 98%" name="CompanyId" id="CompanyId"></select>
                </td>
                <%} %>
                <td style="width: 12%; vertical-align: top">
                    <label for='Search_user'><%:ViewResources.SharedStrings.LogsUser %></label><br />
                    <%:Html.TextBox("UserName") %>
                    <%if (Model.User.IsCompanyManager)
                        {%>
                    <%:Html.Hidden("CompanyId", "") %>
                    <%} %>
                </td>
                <td style="width: <%= Model.User.IsCompanyManager ? "48%" : "36%" %>; vertical-align: top">
                    <label for='Search_activity'><%:ViewResources.SharedStrings.LogsActivity %></label><br />
                    <%:Html.TextBox("Activity") %>
                    <div id='logPrintControlButtons' style="display: none; text-align: right">
                        <a style="cursor: pointer;" onclick="javascript:SetLogExportLink(this,'/Print/LogListPDF')">PDF</a> / <a style="cursor: pointer;" onclick="javascript:SetLogExportLink(this,'/Print/LogListExcel')">XLS</a>
                    </div>
                </td>
                <td style='width: 4%; vertical-align: top; padding-top: 20px;'>
                    <span id='button_submit_log_search' class='icon icon_find tipsy_we' original-title='Search log!' onclick="javascript:SubmitLogSearch();"></span>
                </td>
            </tr>
        </table>
    </form>

    <div id='AreaLogSearchResultsWait' style='display: none; width: 100%; height: 620px; text-align: center'><span style='position: relative; top: 40%' class='icon loader'></span></div>
    <div id='AreaLogSearchResults' style='display: none; margin: 15px 0;'>
    </div>
    <%--<input type="hidden" id="searchlog" value="Search Log" />
     <input type="hidden" id="addfilter" value="Save Filter" />
     <input type="hidden" id="Deletefilter" value="Delete Filter" />--%>
    <script type="text/javascript" language="javascript">

        var log_page = 0;
        var log_rows = 50;
        var log_field = 1;
        var log_direction = 1;

        $(document).ready(function () {
            $("#UserName").attr("id", function () {
                $("#UserName").autocomplete({
                    source: function (request, response) { $.getJSON("/Log/SearchByNameAutoComplete", { term: request.term }, response); },
                    minLength: 1
                });
            });
            var i = $('#panel_owner li').index($('#logTab'));

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

            $(".tipsy_we").attr("class", function () {
                $(this).tipsy({ gravity: $.fn.tipsy.autoWE, html: true });
            });

            $("#FromDate").datetimepicker({

                showSecond: false,
                dateFormat: "dd.mm.yy",
                timeFormat: 'HH:mm',
                firstDay: 1,
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,
                timeText: 'Time ',
                minuteText: 'Minute ',
                hourText: 'Hour'

            });


            $("#ToDate").datetimepicker({
                showSecond: false,
                dateFormat: "dd.mm.yy",
                timeFormat: 'HH:mm',
                firstDay: 1,
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,
                timeText: 'Time ',
                minuteText: 'Minute ',
                hourText: 'Hour'
            });



            GetCompanies();
            GetFilters();
            //$('input:radio[name=IsShowDefaultLog1]').click();


        });

        function GetCompanies() {
            $.ajax({
                type: "Get",
                url: "/Log/GetCompanies",
                dataType: 'json',
                success: function (data) {
                    if ($("select#CompanyId").size() > 0) {
                        $("select#CompanyId").html(data);
                    }
                }
            });
            return false;
        }

        function GetFilters() {

            $.ajax({
                type: "Get",
                url: "/Log/GetFilters",
                dataType: 'json',
                success: function (data) {
                    $("select#selectedFilter").html(data);
                }
            });
            return false;
        }

        function ChangeFilter() {
            filter = $("select#selectedFilter")[0].value;
            $("LogFilterId").attr('value', filter);
            if (filter != "") {
                $.get('/Log/GetFilterData', { filterId: filter },
                    function (data) {
                        if ($("select#CompanyId").size() > 0) {
                            $("select#CompanyId").find('option').each(function () {
                                if ($(this).attr('value') == data.companyId) {
                                    $(this).attr('selected', 'selected');
                                }
                            });
                        }
                        else {
                            $("#CompanyId").attr('value', data.CompanyId);
                        }
                        ClearFilterFields();

                        $('#Activity').attr('value', data.activity);
                        $('#ToDate').val(data.toDate);
                        $('#FromDate').val(data.fromDate);
                        $('#FromDate').attr('value', data.fromDate);
                        $('#ToDate').attr('value', data.toDate);
                        $('#Building').attr('value', data.building);
                        $('#Node').attr('value', data.node);
                        $('#Name').attr('value', data.name);
                        $('#UserName').attr('value', data.userName);
                        $('#LogFilterId').attr('value', data.id);
                        if (data.showFullLog == true) {
                            $('#show_full_log').attr('checked', true);
                            $('#IsShowFullLog').attr('value', true);
                        } else {
                            $('#show_full_log').attr('checked', false);
                            $('#IsShowFullLog').attr('value', false);
                        }
                        if (data.showDefaultLog == true) {
                            $('input:radio[name=IsShowDefaultLog][value=true]').click();

                            //$('input:radio[name="IsShowDefaultLog"]').val('true');
                        } else {
                            $('input:radio[name=IsShowDefaultLog][value=false]').click();
                            //$('input:radio[name="IsShowDefaultLog"]').val('false');
                        }
                    }, 'json');
            }
            else {
                if ($("select#CompanyId").size() > 0) {
                    $("select#CompanyId").find('option').each(function () {
                        if ($(this).attr('value') == "") {
                            $(this).attr('selected', 'selected');
                        }
                    });
                }
                else {
                    $("#CompanyId").attr('value', "");
                }
                ClearFilterFields();
            }
        }

        function ClearFilterFields() {
            $('#Activity').attr('value', "");
            $('#FromDate').attr('value', "");
            $('#ToDate').attr('value', "");
            $('#FromDate').val("");
            $('#ToDate').val("");
            $('#Building').attr('value', "");
            $('#Node').attr('value', "");
            $('#Name').attr('value', "");
            $('#UserName').attr('value', "");
            $('#LogFilterId').attr('value', "");
            $("input[value='false']").prop('checked', true);
        }

        function SaveFilterData() {

            $.post("/Log/EditCreateFilter",
                $("#logFilters").serialize(), function (response) {
                    if (response.IsSucceed == true) {
                        if (response.IsCreate) {
                            $.ajax({
                                type: "Get",
                                url: "/Log/GetFilters",
                                dataType: 'json',
                                success: function (data) {
                                    $("select#selectedFilter").html(data);
                                    $($("select#selectedFilter")).find('option:last').attr('selected', 'selected');

                                }
                            });
                            // saveFilterDetailsinLog();
                            $("#LogFilterId").attr('value', response.Id);
                        }
                        else {
                            $("select#selectedFilter").find('option').each(function () {
                                if ($(this).attr('value') == response.Id) {
                                    $(this).html(response.FilterName);
                                }
                            });
                        }
                        ShowDialog(response.Msg, 2000, true);
                    } else {
                        ShowDialog(response.Msg, 2000);
                    }
                }, 'json');
            return false;
        }

        function DeleteFilterData() {

            $.post("/Log/DeleteFilter",
                { id: $("#LogFilterId").attr('value') }, function (response) {
                    if (response.IsSucceed == true) {
                        $("select#selectedFilter").find('option').each(function () {
                            if ($(this).attr('value') == $("#LogFilterId").attr('value')) {
                                $(this).remove();
                            }

                        });
                        if ($("select#CompanyId").size() > 0) {
                            $("select#CompanyId").find('option').each(function () {
                                if ($(this).attr('value') == "") {
                                    $(this).attr('selected', 'selected');
                                }
                            });
                        }
                        else {
                            $("#CompanyId").attr('value', "");
                        }
                        $('#Activity').attr('value', "");
                        $('#FromDate').attr('value', "");
                        $('#ToDate').attr('value', "");
                        $('#Building').attr('value', "");
                        $('#Node').attr('value', "");
                        $('#Name').attr('value', "");
                        $('#UserName').attr('value', "");
                        $('#LogFilterId').attr('value', "");
                        $('#show_full_log').attr('checked', false);
                        $('#show_default_log').attr('checked', false);
                        $('#IsShowDefaultLog').attr('value', false);
                        $('#IsShowFullLog').attr('value', false);

                        //SaveDeteleFilterInUserLog();
                    }

                    ShowDialog(response.Msg, 2000, true);
                }, 'json');

            return false;
        }

        function ViewLogDetail(cntr) {
            // alert("saSA");
            logRow = $(cntr).parents('tr#logRecord');

            isShort = logRow.find('#isShortDisplayed').attr('value');

            if (isShort == 'true') {
                logRow.find('#shortAction').hide();
                logRow.find('#fullAction').show();
                logRow.find('#isShortDisplayed').attr('value', 'false');
            }
            else {
                logRow.find('#shortAction').show();
                logRow.find('#fullAction').hide();
                logRow.find('#isShortDisplayed').attr('value', 'true');
            }
            return false;
        }
        function ViewLogDetail1(cntr) {


            window.open("http://195.222.11.22:8000/archive/media/FOXSECDEMO/DeviceIpint.1/SourceEndpoint.video:0:0/20170804T071533.581?format=mjpeg&speed=1", "Google", "width=500,height=500");
        }

        function SubmitLogSearch() {

            $.ajax({

                type: "Get",
                url: "/Log/List?" + $("#logFilters").serialize() + "&nav_page=" + log_page + "&rows=" + log_rows + "&sort_field=" + log_field + "&sort_direction=" + log_direction,
                beforeSend: function () {
                    $("#button_submit_log_search").addClass("Trans");
                    // $("div#AreaLogSearchResults").fadeOut('fast', function () { $("div#AreaLogSearchResultsWait").fadeIn('slow'); });
                },
                success: function (result) {
                    $("div#AreaLogSearchResultsWait").hide();
                    $("div#AreaLogSearchResults").html(result);
                    $("div#AreaLogSearchResults").fadeIn('fast');
                    $("#button_submit_log_search").removeClass("Trans");
                    manageLog();
                }
            });
            return false;
        }

        function HandleLogPaging(page, rows) {
            log_page = page;
            log_rows = rows;
            SubmitLogSearch();
            return false;
        }

        function HandleLogSoring(page, rows, field, direction) {
            log_page = page;
            log_rows = rows;
            log_field = field;
            log_direction = direction;
            SubmitLogSearch();
            return false;
        }

        function TogglePrintLog() {
            $("div#logPrintControlButtons").toggle(300);
            return false;
        }

        function SetLogExportLink(link, base) {
            link.href = base +
                '?' + $("#logFilters").serialize() +
                '&sort_field=' + log_field +
                '&sort_direction=' + log_direction;
            return true;
        }

        function manageLog() {
            var FromDate = $("#FromDate").val();
            var ToDate = $("#ToDate").val();
            var Building = $("#Building").val();
            if (Building == "") {
                Building = "NA";
            }
            var IsShowDefaultLog = $('input[name="IsShowDefaultLog"]:checked').parent().text();
            var searchbox = $("#Search_log").val();
            if (searchbox == "") {
                searchbox = "NA";
            }
            var Node = $("#Node").val();
            if (Node == "") {
                Node = "NA";
            }
            var CompanyId = $("#CompanyId option:selected").text();
            var compid = $("#CompanyId").val();
            if (compid == "" || compid == null || compid == undefined) {
                compid = 0;
            }
            //if (compid == "") {
            //    CompanyId = "NA";
            //}
            var Name = $("#Name").val();
            if (Name == "") {
                Name = "NA";
            }
            var selectedFilter = $("#selectedFilter option:selected").text();
            var slct = $("#selectedFilter").val();
            if (slct == "") {
                selectedFilter = "NA";
            }
            var UserName = $("#UserName").val();
            if (UserName == "") {
                UserName = "NA";
            }
            var Activity = $("#Activity").val();
            if (Activity == "") {
                Activity = "NA";
            }


            var data = { FromDate: FromDate, ToDate: ToDate, Building: Building, IsShowDefaultLog: IsShowDefaultLog, searchbox: searchbox, Node: Node, compid: compid, Name: Name, selectedFilter: selectedFilter, UserName: UserName, Activity: Activity }
            $.ajax({
                url: '/Log/ManageUserLog',
                type: "Post",
                data: JSON.stringify(data),
                dataType: "json",
                contentType: "application/json",

                success: function (status) {



                    if (status == "saved") {

                        //updaterowcoun(totalRecordView1,status);
                    }



                }

            });
        }




    </script>
    <script type="text/javascript">

        var x = document.getElementById("chk1").value;

        if (x == "NP") {
            alert("No Image Exits Along this User");



        }
    </script>

</div>
