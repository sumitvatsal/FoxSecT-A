<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.VisitorEditViewModel>" %>
<%@ Import Namespace="FoxSec.Web.Helpers" %>
<%@ Register Src="~/Views/Visitors/VisitorCardsTabContent.ascx" TagPrefix="uc1" TagName="VisitorCardsTabContent" %>

<div id='edit_dialog_content' style="display: none">
    <div id='panel_edit_user'>


        <div id='tab_user_default'>
            <div id='edit_user_personal_data'>
                <ul>
                    <li><a href="#tab_edit_user_personal_data"><%:ViewResources.SharedStrings.PersonalTabName %></a></li>
                </ul>

                <%--Edit User - Personal data Tab--%>

                <div id='tab_edit_user_personal_data'>
                    <form id="editUserPersonalData" action="">
                        <%=Html.Hidden("Id", Model.FoxSecVisitor.Id) %>
                        <table cellpadding="3" cellspacing="3" style="margin: 0; width: 100%; padding: 3px; border-spacing: 3px">
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='edit_user_company'><%:ViewResources.SharedStrings.UsersCompany %></label></td>
                                <td style='width: 40%; padding: 2px;'>
                                    <%if (!Model.User.IsCompanyManager)
                                        {%>
                                    <%= Html.DropDownList("CompanyId", new SelectList(Model.Companies, "Value", "Text", Model.FoxSecVisitor.CompanyId), ViewResources.SharedStrings.DefaultDropDownValue, new { @style = "width:83%",disabled="disabled"})%><span id="Company_val" style="color: Red; font-size: 15px; font-weight: bold"></span>
                                    <%}
                                        else
                                        { %>
                                    <%= Html.DropDownList("CompanyId", new SelectList(Model.Companies, "Value", "Text", Model.FoxSecVisitor.CompanyId), new { @style = "width:83%",disabled="disabled" })%><span id="Company1_val" style="color: Red; font-size: 15px; font-weight: bold"></span>
                                    <%} %>
                                </td>
                                <td style='width: 40%; padding: 2px;' rowspan='7'>
                                    <div id='photo_content'>
                                        <table>
                                            <tr>
                                                <td align="center" style="padding: 2px; color: darkblue"><%:ViewResources.SharedStrings.LiveVideo %></td>
                                                <td style="padding: 2px; color: darkblue"><%:ViewResources.SharedStrings.CapturedPhoto %></td>
                                            </tr>
                                            <tr>
                                                <td style='padding: 2px;' align="center">
                                                    <div style="border-left: 3px dotted red; height: 150px; position: absolute; left: 50%; margin-left: 135px; margin-top: 100px; top: 0;"></div>
                                                    <div style="border-left: 3px dotted red; height: 150px; position: absolute; left: 50%; margin-left: 255px; margin-top: 100px; top: 0;"></div>
                                                    <video id="video" height="200" width="170" autoplay>
                                                    </video>
                                                </td>
                                                <td style='padding: 2px;' align="center">
                                                    <canvas id="imgCapture" height="192" width="192"></canvas>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding: 2px">
                                                    <input type="button" value="<%:ViewResources.SharedStrings.VisitorCapturePhoto %>" id="snap" />
                                                </td>

                                            </tr>
                                            <tr>
                                                <td colspan="2" style="padding: 2px"></td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='FirstName'><%:ViewResources.SharedStrings.UsersFirstName %></label></td>
                                <td style='width: 40%; padding: 2px;'><%=Html.TextBox("FirstName", Model.FoxSecVisitor.FirstName, new { @style = "width:80%",disabled="disabled" })%> <span id="FirstName_val" style="color: Red; font-size: 15px; font-weight: bold"></span></td>
                                <td style='width: 40%; padding: 0px;'></td>
                            </tr>
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='Lastname'><%:ViewResources.SharedStrings.UsersLastName %></label></td>
                                <td style='width: 40%; padding: 2px;'><%=Html.TextBox("LastName", Model.FoxSecVisitor.LastName, new { @style = "width:80%",disabled="disabled" })%> <span id="LastName_val" style="color: Red; font-size: 15px; font-weight: bold"></span>
                                </td>
                            </tr>
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='Lastname'><%:ViewResources.SharedStrings.UsersPersonalCode %></label></td>
                                <td style='width: 40%; padding: 2px;'><%=Html.TextBox("PersonalCode", Model.FoxSecVisitor.PersonalCode, new { @style = "width:80%",disabled="disabled" })%> <span id="PersonalCode_val" style="color: Red; font-size: 15px; font-weight: bold"></span>
                                </td>
                            </tr>
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='CardFirstName'><%:ViewResources.SharedStrings.CardFirstName %></label></td>
                                <td style='width: 40%; padding: 2px;'><%=Html.TextBox("CardFirstName", Model.FoxSecVisitor.CardFirstName, new {@style = "width:80%",disabled="disabled" })%></td>
                                <td style='width: 40%; padding: 2px;'></td>
                            </tr>
                            <tr style="display: none">
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <%=Html.Label(ViewResources.SharedStrings.PhoneNrAccessUnit, new { @for = "IsPhoneNrAccessUnit" }) %>
                                </td>
                                <td style='width: 40%; padding: 2px;'>
                                    <%=Html.CheckBox("IsPhoneNrAccessUnit", Model.FoxSecVisitor.IsPhoneNrAccessUnit, new { })%>
                                </td>
                                <td style='width: 40%; padding: 2px;'></td>
                            </tr>
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='CardLastName'><%:ViewResources.SharedStrings.CardLastName %></label></td>
                                <td style='width: 40%; padding: 2px;'><%=Html.TextBox("CardLastName", Model.FoxSecVisitor.CardLastName, new { @style = "width:80%",disabled="disabled" })%></td>
                                <td style='width: 40%; padding: 2px;'></td>
                            </tr>
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='CarNr'><%:ViewResources.SharedStrings.CarNr%></label></td>
                                <td style='width: 40%; padding: 2px;'><%=Html.TextBox("CarNumber", Model.FoxSecVisitor.CarNr, new { @style = "width:80%",disabled="disabled" })%> <span id="CarNr_val" style="color: Red; font-size: 15px; font-weight: bold"></span></td>
                            </tr>
                            <tr style="display: none">
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <%=Html.Label(ViewResources.SharedStrings.CarNrAccessUnit, new { @for = "IsCarNrAccessUnit" }) %>
                                </td>
                                <td style='width: 40%; padding: 2px;'>
                                    <%=Html.CheckBox("IsCarNrAccessUnit",  Model.FoxSecVisitor.IsCarNrAccessUnit, new {})%>
                                </td>
                                <td style='width: 40%; padding: 2px;'></td>
                            </tr>

                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='StartDateTime'><%:ViewResources.SharedStrings.CardsValidFrom%></label></td>
                                <td>

                                    <%=Html.TextBox("StartDateTime", Model.FoxSecVisitor.FromDate, new { @style = "width:80%",disabled="disabled"})%>
                                    <span id="StartDayTime_val" style="color: Red; font-size: 15px; font-weight: bold"></span>
                                </td>
                                <td style="width: 40%; padding: 2px"></td>
                            </tr>
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='StopDateTime'><%:ViewResources.SharedStrings.CardsValidTo%></label></td>
                                <td>
                                    <%=Html.TextBox("StopDateTime", Model.FoxSecVisitor.ToDate, new {@onchange="chkvalidto()",@style = "width:80%",disabled="disabled" })%>
                                    <span id="StopDayTime_val" style="color: Red; font-size: 15px; font-weight: bold"></span>
                                </td>
                            </tr>
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='Comment'><%:ViewResources.SharedStrings.CommonComments %></label></td>
                                <td style='width: 40%; padding: 2px;'>
                                    <%=Html.TextArea("Comment", Model.FoxSecVisitor.Comment, new { @style = "width:80%",disabled="disabled" })%>
                                </td>
                            </tr>
                            <tr id="trreturndate">
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='ReturnDate' id="lblreturndate"><%:ViewResources.SharedStrings.ReturnDate%></label></td>
                                <td>
                                    <%=Html.TextBox("ReturnDate", Model.FoxSecVisitor.DateReturn, new {@style = "width:80%",disabled="disabled"  })%>
                                    <span id="ReturnDayTime_val" style="color: Red; font-size: 15px; font-weight: bold"></span>
                                </td>
                            </tr>
                            <tr>
                                <td style='width: 40%; padding: 2px;'><%=Html.TextBox("UserId", Model.FoxSecVisitor.UserId, new { @style = "width:80%" })%> <span id="UserId_val" style="color: Red; font-size: 15px; font-weight: bold"></span></td>
                            </tr>
                        </table>
                        <br />
                        <table>
                            <tr>

                                <td>
                                    <%if (Model.FoxSecVisitor.CardBackFlag == true)
                                        { %>
                                    <input type='button' id='card_returned' value='<%:ViewResources.SharedStrings.CardBack %>' onclick="returncard()" />
                                    <%} %>
                                </td>
                            </tr>
                        </table>
                        <div id='userId' style="display: none"></div>
                        <div id='divuserper' style="display: none"></div>
                    </form>
                </div>

            </div>
        </div>


    </div>
</div>
<div id='AreaTabPeopleSearchResultsWait' style='display: none; width: 80%; text-align: center'><span style='position: relative;' class='icon loader'></span></div>

<script>
    function ActivatePhotoUpload() {
        $.get('/Visitors/UploadVisitorImage',
            { VisitorId: "<%= Model.FoxSecVisitor.Id %>" },
            function (html) {
                $("div#upload_photo").html(html);
                $("div#upload_photo").toggle("slow");
            });
        return false;
    }

    // Grab elements, create settings, etc.
    var video = document.getElementById('video');

    // Get access to the camera!
    if (navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {
        // Not adding `{ audio: true }` since we only want video now
        navigator.mediaDevices.getUserMedia({ video: true }).then(function (stream) {
            //video.src = window.URL.createObjectURL(stream);
            video.srcObject = stream;
            video.play();
        });
    }

    var canvas = document.getElementById('imgCapture');
    var context = canvas.getContext('2d');
    var video = document.getElementById('video');
    var w, h, ratio;
    // Trigger photo take
    document.getElementById("snap").addEventListener("click", function () {
        context.clearRect(0, 0, 192, 192);
        var isIE = /*@cc_on!@*/false || !!document.documentMode;
        var isEdge = !isIE && !!window.StyleMedia;
        if (isEdge == true) {
            context.drawImage(video, 100, 0, 480, 360, 0, 0, 192, 192);
        }
        else {
            context.drawImage(video, 100, 0, 460, 478, 0, 0, 192, 192);
        }
        var image = document.getElementById("imgCapture").toDataURL("image/png");
        image = image.replace('data:image/png;base64,', '');
        $.ajax({
            type: "POST",
            dataType: "json",
            data: '{ "reqimg" : "' + image + '" }',
            contentType: 'application/json; charset=utf-8',
            url: "/Visitors/Capture",
            contentType: 'application/json; charset=utf-8',
            success: function (html) {
            }
        });

    });
</script>
<script type="text/javascript" language="javascript">

    function deletevisitor() {
        var vid = $('#tab_edit_user_personal_data').find('#Id').attr('value');
        var msg = '<%=ViewResources.SharedStrings.DeleteVisitor %>';

        $("div#divuserper").dialog({

            open: function () {
                $("div#divuserper").html("");
                $("div#divuserper").html(msg);
            },
            resizable: false,
            width: 300,
            height: 200,
            modal: true,
            title: "<span class='ui-icon ui-icon-trash' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.BtnDelete %>',
            buttons: {
                '<%=ViewResources.SharedStrings.BtnOk %>': function () {
                    $("div#divuserper").html("");
                    $("div#divuserper").html("<div id='AreaUserEditWait' style='width: 80%; height:400px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                    $.ajax({
                        type: "Post",
                        url: "/Visitors/Delete",
                        data: { vid: vid },
                        traditional: true,
                        success: function () {
                            ShowDialog('<%=ViewResources.SharedStrings.VisitorDelete %>', 2000, true);
                            SubmitPeopleSearch();
                        }
                    });
                    $(this).dialog("destroy");
                    $("div#modal-dialog").dialog("destroy");
                },
                close: function (event, ui) {
                    $(this).dialog("destroy");
                }
            }
        });
        return false;
    }

    $(document).ready(function () {
        $('input#UserId').hide();
        $('#input#ReturnDate').val("");
        enabledisablebutton();
        $("#panel_edit_user").attr("id", function () {
            passChecked = true;
            $("#panel_edit_user").tabs({
                beforeLoad: function (event, ui) {
                    ui.ajaxSettings.async = false,
                        ui.ajaxSettings.error = function (xhr, status, index, anchor) {
                            $(anchor.hash).html("Couldn't load this tab!");
                        }
                },
                fx: {
                    opacity: "toggle",
                    duration: "fast"
                },
                active: 0
            });
        });

        $("#panel_edit_user").tabs("option", "disabled", [3]);

        $("#edit_user_personal_data").tabs({
            beforeLoad: function (event, ui) {
                ui.ajaxSettings.async = false,
                    ui.ajaxSettings.error = function (xhr, status, index, anchor) {
                        $(anchor.hash).html("Couldn't load this tab!");
                    }
            },
            fx: {
                opacity: "toggle",
                duration: "fast"
            },
            active: 0
        });

        $("#edit_user_card_data").tabs({
            beforeLoad: function (event, ui) {
                ui.ajaxSettings.async = false,
                    ui.ajaxSettings.error = function (xhr, status, index, anchor) {
                        $(anchor.hash).html("Couldn't load this tab!");
                    }
            },
            fx: {
                opacity: "toggle",
                duration: "fast"
            },
            active: 0
        });

        $("#StartDateTime").datetimepicker({
            showSecond: false,
            dateFormat: "dd.mm.yy",
            timeFormat: 'HH:mm',
            firstDay: 1,
            changeMonth: true,
            changeYear: true,
            showButtonPanel: true,
            timeText: 'Time',
            minuteText: 'Minute',
            hourText: 'Hour'

        });

        $("#StopDateTime").datetimepicker({
            showSecond: false,
            dateFormat: "dd.mm.yy",
            timeFormat: 'HH:mm',
            firstDay: 1,
            changeMonth: true,
            changeYear: true,
            showButtonPanel: true,
            timeText: 'Time',
            minuteText: 'Minute',
            hourText: 'Hour'
        });

        $("#ReturnDate").datetimepicker({
            showSecond: false,
            dateFormat: "dd.mm.yy",
            timeFormat: 'HH:mm',
            firstDay: 1,
            changeMonth: true,
            changeYear: true,
            showButtonPanel: true,
            timeText: 'Time',
            minuteText: 'Minute',
            hourText: 'Hour'
        });

        $(".tipsy_we").attr("class", function () {
            $(this).tipsy({ gravity: $.fn.tipsy.autoWE, html: true });
        });

        $("div#create_dialog_content").fadeIn("300");

        var visitorid = $('#tab_edit_user_personal_data').find('#Id').attr('value');
        $.get('/Visitors/VisitorCardsList', { vid: visitorid, filter: 1 }, function (html) { $("div#VisitorCardsList").html(html); });

        $("div#edit_dialog_content").fadeIn("fast");
        $("input:button").button();

    <%if (ViewBag.VisitorImage == "" || ViewBag.VisitorImage == null) { }
    else
    {%>
        var myCanvas = document.getElementById('imgCapture');
        var ctx = myCanvas.getContext('2d');
        var img = new Image();
        img.onload = function () {
            ctx.clearRect(0, 0, 192, 192);
            ctx.drawImage(img, 0, 0, 192, 192);
        };
        img.src = '<%:ViewBag.VisitorImage %>';
    <%}%>
        return false;
    });


    function ValidateFormFields() {
        var isValid = new Boolean(true);

        if ($("input#FirstName").val() == "" || ($("input#FirstName").val() != "" && !$("input#FirstName").val().match(/^.{1,20}$/))) { $("#FirstName_val").text('!').show().fadeOut(3000); isValid = false; }

        if ($("input#LastName").val() == "" || ($("input#LastName").val() != "" && !$("input#LastName").val().match(/^.{1,20}$/))) { $("#LastName_val").text('!').show().fadeOut(3000); isValid = false; }

        if ($("select#CompanyId").val() == "") {
            $("select#Company_val").text('!').show().fadeOut(3000);
            $("select#Company1_val").text('!').show().fadeOut(3000);
            isValid = false;
        }

        var FromDate = $("input#StartDateTime").val();
        var ToDate = $("input#StopDateTime").val();
        if (FromDate != "" && ToDate != "") {

            if ((new Date(FromDate) > new Date(ToDate))) {

                $("input#StartDayTime_val").text('!').show().fadeOut(3000);
                $("input#StopDayTime_val").text('!').show().fadeOut(3000);
                isValid = false;
            }
        }
        else {
            $("input#StartDayTime_val").text('!').show().fadeOut(3000);
            $("input#StopDayTime_val").text('!').show().fadeOut(3000);
            isValid = false;
        }

        return isValid;

    }

    function ClearFilterFields() {
        $('input#FirstName').attr('value', "");
        $('input#LastName').attr('value', "");
        $('input#CarNumber').attr('value', "");
        $('input#CarType').attr('value', "");
        $('input#UserPermission').attr('value', "");
        $('input#UserId').attr('value', "");
    }

    function ReloadEditPage() {
        $.get('/Visitors/Edit', { id: $('#tab_edit_user_personal_data').find('#Id').attr('value') }, function (html) { $("div#modal-dialog").html(html); });
        return false;
    }

    function RemoveUserPhoto() {
        $("div#delete-modal-dialog").dialog({
            open: function (event, ui) {
                $("div#delete-modal-dialog").html('<%=ViewResources.SharedStrings.CommonConfirmMessage %>');
            },
            resizable: false,
            width: 300,
            height: 140,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.UsersDeletingPhoto %>',
            buttons: {
                '<%=ViewResources.SharedStrings.BtnDelete %>': function () {
                    $(this).dialog("close");
                    $.ajax({
                        type: "Post",
                        url: "/User/RemovePhoto",
                        data: { id: '<%= Model.FoxSecVisitor.Id %>' },
                        success: function () {
                            $("div#photo_content").html('<table><tr><td align="right"><span class="ui-icon ui-icon-circle-close" style="cursor:pointer;visibility:hidden;" onclick="javascript:RemoveUserPhoto();"></span></td></tr><tr><td><div style="width:90px; height:100px; border:1px solid #333; text-align:center; padding:10px 5px; cursor:pointer;" onclick="javascript:ActivatePhotoUpload();"><%:ViewResources.SharedStrings.UsersUploadFoto%></div></td></tr></table>');
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
    function showretuendate() {
        if ($('input#CardNeedReturn').prop("checked") == true) {
            $('#trreturndate,#card_returned').show();
        }
        else {
            $('#trreturndate,#card_returned').hide();
        }
        $('#input#ReturnDate').val("");
    }

    function enabledisablebutton() {

        var userID = $("input#UserId").val();
        if (userID != "" && userID != null) {
            $('#divprntvscard').html("");
            $('#divsndqrcode').html("");
            $('#divprntvscard').append("<input type='button' id='print_vistiotr_card' value='<%:ViewResources.SharedStrings.PrintVisitorCard %>' onclick='printvisitorcard()'/>");
            $('#divsndqrcode').append("<input type='button' id='send_qr_code_by' value='<%:ViewResources.SharedStrings.SendQRCode %>' onclick='sendqrcode()'/>");
        }
        else if (userID != "" && userID != null) {
            $('#divprntvscard').html("");
            $('#divprntvscard').append("<input type='button' id='print_vistiotr_card' value='<%:ViewResources.SharedStrings.PrintVisitorCard %>' onclick='printvisitorcard()'/>");
            $('#divsndqrcode').html("");
            $('#divsndqrcode').append("<input type='button' value='<%:ViewResources.SharedStrings.SendQRCode %>' disabled/>");
        }
        else {
            $('#divprntvscard').html("");
            $('#divsndqrcode').html("");
            $('#divprntvscard').append("<input type='button' value='<%:ViewResources.SharedStrings.PrintVisitorCard %>' disabled/>");
            $('#divsndqrcode').append("<input type='button' value='<%:ViewResources.SharedStrings.SendQRCode %>' disabled/>");
        }

        $("input:button").button();
        $(".tipsy_we").attr("class", function () {
            $(this).tipsy({ gravity: $.fn.tipsy.autoWE, html: true });
        });
    }

    function returncard() {

        var cardneedreturn;

        var usId = $('#tab_edit_user_personal_data').find('#Id').attr('value');
        //Visitor Data

        var returndate = $("input#ReturnDate").val();

        if ($('input#CardNeedReturn').prop("checked") == true) {
            cardneedreturn = "true";
        }
        else {
            cardneedreturn = "false";
        }

        if (returndate == "" || returndate == null) {
            ShowDialog('Please select Return Date!', 5000);
            $("input#ReturnDayTime_val").text('!').show().fadeOut(3000);
            return false;
        }

        $.ajax({
            type: "Post",
            url: "/Visitors/GiveCardBack",
            data:
            {
                id: usId,
                returndate: returndate,
                cardneedreturn: cardneedreturn
            },
            success: function (response) {
                ShowDialog('<%=ViewResources.SharedStrings.CommonDataSavedMessage %>', 2000, true);
                UpdateUserRow(usId);
                SubmitPeopleSearch();
                ReloadEditPage();
            }
        });
    }

    function chkvalidto() {
        var ToDate = $("input#StopDateTime").val();
        var datentime = ToDate.split(' ');
        var dateval = datentime[0].split('.');
        var timeval = datentime[1].split(':');
        ToDate = dateval[2] + "-" + dateval[1] + "-" + dateval[0] + " " + timeval[0] + ":" + timeval[1];

        var currdatetime = "<%=DateTime.Now.ToString("yyyy.MM.dd HH:mm")%>";
        var datentime1 = currdatetime.split(' ');
        var dateval1 = datentime1[0].split('.');
        var timeval1 = datentime1[1].split(':');
        currdatetime = dateval1[0].trim() + "-" + dateval1[1].trim() + "-" + dateval1[2].trim() + " " + timeval1[0].trim() + ":" + timeval1[1].trim();

        $('#divjoinvisitor').empty("");
        if (ToDate < currdatetime) {
            $("input#UserId").val("");
            $("input#UserPermission").val("");
            $('#divjoinvisitor').append('<input type="button" value="<%=ViewResources.SharedStrings.JoinPermission %>" disabled="disabled" />');
        }
        else {
            $('#divjoinvisitor').append('<input type="button" onclick="joinper()" value="<%=ViewResources.SharedStrings.JoinPermission %>" />');
        }
        $("input:button").button();
        $(".tipsy_we").attr("class", function () {
            $(this).tipsy({ gravity: $.fn.tipsy.autoWE, html: true });
        });
    }
    function joinper() {
        var com = $("select#CompanyId").val();
        var FromDate = $("input#StartDateTime").val();

        $("div#divuserper").dialog({
            open: function () {
                $("div#divuserper").html("");
                $("div#divuserper").html("<div id='AreaUserEditWait' style='width: 80%; height:400px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                $.ajax({
                    type: "Get",
                    url: "/Visitors/GetUserID",
                    data: { fromdatetime: FromDate },
                    success: function (html) {
                        $("div#divuserper").html(html);
                    }
                });
                $(this).parents('.ui-dialog-buttonpane button:eq(0)').focus();
            },
            resizable: false,
            width: 850,
            height: 600,
            modal: true,
            title: "<span class='ui-icon ui-icon-home' style='float:left; margin:1px 5px 0 0'></span>" + '<%= ViewResources.SharedStrings.VisitorsUsers%>',
            buttons:
            {
                    '<%= ViewResources.SharedStrings.BtnClose %>': function () {

                    $(this).dialog("close");
                }
            },
            close: function (event, ui) {
                $(this).dialog("destroy");
            }
        });
        return false;
    }
</script>

