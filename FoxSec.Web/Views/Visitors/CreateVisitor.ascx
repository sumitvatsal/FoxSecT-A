<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.VisitorEditViewModel>" %>
<div id='create_dialog_content' style="display: none">

    <div id='panel_create_user'>
        <ul>
            <li><a href="#tab_user_default"><%:ViewResources.SharedStrings.PersonalDataTabName %></a></li>
        </ul>
        <div id='tab_user_default'>

            <div id='create_user_personal_data'>
                <ul>
                    <li><a href="#tab_create_user_personal_data"><%:ViewResources.SharedStrings.PersonalTabName %></a></li>
                    <li><a href="#tab_create_user_personal_data"><%:ViewResources.SharedStrings.OtherTabName %></a></li>
                </ul>
                <%--Create User - Personal data Tab--%>

                <div id='tab_create_user_personal_data'>
                    <form id="editVisitorPersonalData">
                        <table cellpadding="3" cellspacing="3" style="margin: 0; width: 100%; padding: 3px; border-spacing: 3px">
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='edit_user_company'><%:ViewResources.SharedStrings.UsersCompany %></label></td>
                                <td style='width: 40%; padding: 2px;'>
                                    <%if (!Model.User.IsCompanyManager)
                                        {%>
                                    <%= Html.DropDownList("CompanyId", new SelectList(Model.Companies, "Value", "Text", Model.FoxSecVisitor.CompanyId), ViewResources.SharedStrings.DefaultDropDownValue, new { @style = "width:83%"})%><span id="Company_val" style="color: Red; font-size: 15px; font-weight: bold"></span>
                                    <%}
                                        else
                                        { %>
                                    <%= Html.DropDownList("CompanyId", new SelectList(Model.Companies, "Value", "Text", Model.FoxSecVisitor.CompanyId), new {@style = "width:83%" })%><span id="Company1_val" style="color: Red; font-size: 15px; font-weight: bold"></span>
                                    <%} %>
                                </td>
                                <td style='width: 40%; padding: 2px;'></td>
                            </tr>
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='FirstName'><%:ViewResources.SharedStrings.UsersFirstName %></label></td>
                                <td style='width: 40%; padding: 2px;'><%=Html.TextBox("FirstName", Model.FoxSecVisitor.FirstName, new { @style = "width:80%" })%> <span id="FirstName_val" style="color: Red; font-size: 15px; font-weight: bold"></span></td>
                                <td style='width: 40%; padding: 0px;' rowspan='5'>
                                    <div id='photo_content'>
                                        <table>
                                            <tr>
                                                <td align="right"><span class="ui-icon ui-icon-circle-close" style="cursor: pointer; visibility: hidden;" onclick="javascript:RemoveUserPhoto();"></span></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div style="width: 90px; height: 100px; border: 1px solid #333; text-align: center; padding: 10px 5px; cursor: pointer;" onclick="javascript:ActivatePhotoUpload();"><%:ViewResources.SharedStrings.UsersUploadFoto%></div>
                                                </td>
                                            </tr>
                                            <tr></tr>
                                            <tr></tr>
                                            <tr>
                                                <td style='width: 20%; padding: 2px;'>
                                                    <div id="divcapturephoto">
                                                        <input type='button' id='take_photo' disabled="disabled" value='<%:ViewResources.SharedStrings.VisitorCapturePhoto %>' />
                                                    </div>
                                                </td>
                                                <td style='width: 20%; padding: 2px;'>
                                                    <div>
                                                        <input type='button' disabled="disabled" value='<%:ViewResources.SharedStrings.UsersUploadFoto %>' />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                            </tr>


                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='Lastname'><%:ViewResources.SharedStrings.UsersLastName %></label></td>
                                <td style='width: 40%; padding: 2px;'><%=Html.TextBox("LastName", Model.FoxSecVisitor.LastName, new { @style = "width:80%" })%> <span id="LastName_val" style="color: Red; font-size: 15px; font-weight: bold"></span>
                                </td>
                            </tr>
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='Lastname'><%:ViewResources.SharedStrings.UsersPersonalCode %></label></td>
                                <td style='width: 40%; padding: 2px;'><%=Html.TextBox("PersonalCode", Model.FoxSecVisitor.PersonalCode, new { @style = "width:80%" })%> <span id="PersonalCode_val" style="color: Red; font-size: 15px; font-weight: bold"></span>
                                </td>
                            </tr>
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='PhoneNumber'><%:ViewResources.SharedStrings.UsersPhone %></label></td>
                                <td style='width: 40%; padding: 2px;'><%=Html.TextBox("PhoneNumber", Model.FoxSecVisitor.PhoneNumber, new {@style = "width:80%"})%> <span id="PhoneNumber_val" style="color: Red; font-size: 15px; font-weight: bold"></span></td>
                                <td style='width: 40%; padding: 2px;'></td>
                            </tr>
                            <tr style="display: none">
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <%=Html.Label(ViewResources.SharedStrings.PhoneNrAccessUnit, new { @for = "IsPhoneNrAccessUnit" }) %>
                                </td>
                                <td style='width: 40%; padding: 2px;'>
                                    <%=Html.CheckBox("IsPhoneNrAccessUnit", false, new { })%>
                                </td>
                                <td style='width: 40%; padding: 2px;'></td>
                            </tr>
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='Email'><%:ViewResources.SharedStrings.UsersEmail %></label></td>
                                <td style='width: 40%; padding: 2px;'><%=Html.TextBox("Email", Model.FoxSecVisitor.Email, new { @style = "width:80%",@onchange="enabledisablebutton()"})%> <span id="Email_val" style="color: Red; font-size: 15px; font-weight: bold"></span></td>
                                <td style='width: 40%; padding: 2px;'></td>
                            </tr>

                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='CarNr'><%:ViewResources.SharedStrings.CarNr %></label></td>
                                <td style='width: 40%; padding: 2px;'><%=Html.TextBox("CarNumber", Model.FoxSecVisitor.CarNr, new { @style = "width:80%" })%> <span id="CarNr_val" style="color: Red; font-size: 15px; font-weight: bold"></span></td>
                            </tr>
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='CarType'><%:ViewResources.SharedStrings.CardType %></label></td>
                                <td style='width: 40%; padding: 2px;'><%=Html.TextBox("CarType", Model.FoxSecVisitor.CarType, new {@style = "width:80%" })%> <span id="CarType_val" style="color: Red; font-size: 15px; font-weight: bold"></span></td>
                                <td style='width: 40%; padding: 0px;' rowspan='4'>
                                    <table>
                                        <tr>
                                            <td style="width: 20%">
                                                <label for='Comment'><%:ViewResources.SharedStrings.CommonComments %></label></td>
                                            <td style="width: 80%"><%=Html.TextArea("Comment", Model.FoxSecVisitor.Comment, new { @style = "width:80%" })%></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='StartDateTime'><%:ViewResources.SharedStrings.CardsValidFrom %></label></td>
                                <td style='width: 40%; padding: 2px;'>
                                    <%:Html.TextBox("StartDateTime",string.Format("{0} 07:00",DateTime.Now.AddHours(-1).ToString("dd.MM.yyyy"), DateTime.Now.AddHours(-1).Hour.ToString("D2"), DateTime.Now.Minute.ToString("D2")),new { @style = "width:80%" })%><span id="StartDayTime_val" style="color: Red; font-size: 15px; font-weight: bold"></span>
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <%=Html.Label(ViewResources.SharedStrings.CarNrAccessUnit, new { @for = "IsCarNrAccessUnit" }) %>
                                </td>
                                <td style="width: 40%; padding: 2px;">
                                    <%=Html.CheckBox("IsCarNrAccessUnit", false, new {})%>
                                </td>
                                <td style='width: 40%; padding: 2px;'></td>
                            </tr>

                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='StopDateTime'><%:ViewResources.SharedStrings.CardsValidTo %></label></td>
                                <td>
                                    <%:Html.TextBox("StopDateTime", string.Format("{0} 18:00", DateTime.Now.AddHours(-1).ToString("dd.MM.yyyy"), DateTime.Now.AddHours(-1).Hour.ToString("D2"), DateTime.Now.Minute.ToString("D2")),new { @onchange="chkvalidto()",@style = "width:80%" })%><span id="StopDayTime_val" style="color: Red; font-size: 15px; font-weight: bold"></span>
                                </td>
                            </tr>
                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='UserPermission'><%:ViewResources.SharedStrings.PermissionName %></label></td>
                                <td style='width: 40%; padding: 2px;'><%=Html.TextBox("UserPermission", "", new { @disabled = "disabled",@style = "width:80%" })%> <span id="Permission_val" style="color: Red; font-size: 15px; font-weight: bold"></span></td>
                            </tr>

                            <tr>
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='CardNeedReturn'><%:ViewResources.SharedStrings.CardNeedReturn %></label></td>
                                <td style='width: 40%; padding: 2px;'>
                                    <%=Html.CheckBox("CardNeedReturn", false, new {})%></td>
                            </tr>
                            <tr id="trreturndate">
                                <td style='width: 20%; padding: 2px; text-align: right;'>
                                    <label for='ReturnDate' id="lblreturndate"><%:ViewResources.SharedStrings.ReturnDate %></label></td>
                                <td>
                                    <%:Html.TextBox("ReturnDate", string.Format("{0} 18:00", DateTime.Now.AddHours(-1).ToString("dd.MM.yyyy"), DateTime.Now.AddHours(-1).Hour.ToString("D2"), DateTime.Now.Minute.ToString("D2")),new {  @style = "width:80%"})%><span id="ReturnDayTime_val" style="color: Red; font-size: 15px; font-weight: bold"></span>
                                </td>
                            </tr>
                            <tr>
                                <td style='width: 40%; padding: 2px;'><%=Html.TextBox("UserId", Model.FoxSecVisitor.UserId, new { @style = "width:80%"})%> <span id="UserId_val" style="color: Red; font-size: 15px; font-weight: bold"></span></td>
                            </tr>

                        </table>
                    </form>
                    <br />
                    <table>
                        <tr>
                            <td>
                                <input type='button' id='button_save_edit_visitor' value="<%:ViewResources.SharedStrings.BtnSave %>" onclick='javascript: SavePersonalData2();' />
                            </td>
                            <td>
                                <div id="divjoinvisitor">
                                    <%--<input type='button' id='joinpermission' value="<%:ViewResources.SharedStrings.JoinPermission %>" onchange="chkvalidto()" />--%>
                                </div>
                            </td>
                            <td>
                                <div id="divprntvscard">
                                    <input type='button' id='print_vistiotr_card' value='<%:ViewResources.SharedStrings.PrintVisitorCard %>' disabled />
                                </div>
                            </td>
                            <td>
                                <div id="divsndqrcode">
                                    <input type='button' id='send_qr_code_by' value='<%:ViewResources.SharedStrings.SendQRCode %>' disabled />
                                </div>
                            </td>
                            <td>
                                <%-- <input type='button' id='card_returned' value='Card Returned' />--%></td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
</div>
<div id='AreaTabPeopleSearchResultsWait' style='display: none; width: 80%; text-align: center'><span style='position: relative; top: 45%' class='icon loader'></span></div>

<div id='userId' style="display: none"></div>
<div id='divuserper' style="display: none"></div>
<%--Create User - User Contact Tab--%>


<%--Create User - User Work Tab--%>

<script type="text/javascript" lang="javascript">

    var glbvid = "";
    $(document).ready(function () {
        glbvid = "";
        $("input#UserId").hide();
        $('#UserPermission').attr('value', "");
        $('#trreturndate,#card_returned').hide();
        $("div#upload_photo").hide();

        passChecked = false;

        $("#panel_create_user").attr("id", function () {
            $("#panel_create_user").tabs({
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

        $("#panel_create_user").tabs("option", "disabled", [1, 2, 3]);

        $("#create_user_personal_data").attr("id", function () {
            $("#create_user_personal_data").tabs({
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

        $("#create_user_personal_data").tabs("option", "disabled", [1, 2, 3, 4]);
        $("input:button").button();

        $(".select_date").datepicker({
            dateFormat: "dd.mm.yy",
            firstDay: 1,
            changeMonth: true,
            changeYear: true,
            showButtonPanel: true,
            gotoCurrent: true,
            onSelect: function (dateText, inst) {
                $(".select_date").datepicker("option", "minDate", dateText);
            }
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

        $(".date_start").datepicker({
            dateFormat: "dd.mm.yy",
            firstDay: 1,
            changeMonth: true,
            changeYear: true,
            showButtonPanel: true,
            gotoCurrent: true,


            onSelect: function (dateText, inst) {
                $(".date_end").datepicker("option", "minDate", dateText);
            }
        });

        $(".date_end").datepicker({
            dateFormat: "dd.mm.yy",
            firstDay: 1,
            changeMonth: true,
            changeYear: true,
            showButtonPanel: true,
            gotoCurrent: true,
            minDate: $(".date_end").val()
        });

        $(".tipsy_we").attr("class", function () {
            $(this).tipsy({ gravity: $.fn.tipsy.autoWE, html: true });
        });
        chkvalidto();
        $("div#create_dialog_content").fadeIn("300");

        return false;
    });


    function sendqrcode() {
        var isValid = new Boolean(true);

        if ($("input#Email").val() == "" || ($("input#Email").val() != "" && !$("input#Email").val().match(/[\w-]+@([\w-]+\.)+[\w-]+/))) { $("#Email_val").text('!').show().fadeOut(3000); isValid = false; }

        if ($("input#UserPermission").val() == "") { $("#Permission_val").text('!').show().fadeOut(3000); isValid = false; }

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

        if (!isValid) {
            ShowDialog('<%=ViewResources.SharedStrings.UsersValidator %>', 5000);
            return false;
        }
        else {
            if (glbvid == "") {
                ShowDialog('<%=ViewResources.SharedStrings.FirstSaveVisitorDetails %>', 5000);
                return false;
            }
            else {
                var userid = $("input#UserId").val();
                $.ajax({
                    type: "Post",
                    url: "/Visitors/SendQRCode",
                    data:
                    {
                        vid: glbvid,
                        uid: userid
                    },

                    beforeSend: function () {
                        $("div#AreaTabPeopleSearchResultsWait").show();
                        $("div#panel_create_user").hide();

                    },
                    success: function (response) {
                        $("div#AreaTabPeopleSearchResultsWait").hide();
                        $("div#panel_create_user").show();
                        if (response == "1") {
                            ShowDialog('<%=ViewResources.SharedStrings.VisitorMessageQRCodesent %>', 2000, true);
                        }
                        else {
                            ShowDialog('<%=ViewResources.SharedStrings.VisitorMessageQRCodenotsent %>', 5000);
                            return false;
                        }

                    }
                });
                return false;
            }
        }
    }

    function printvisitorcard() {
        if (glbvid == "") {
            ShowDialog('Please first save visitor details!', 5000);
            return false;
        }

        var userid = $("input#UserId").val();

        $("div#userId").dialog({
            open: function () {
                $("div#userId").html("");
                $("div#userId").html("<div id='AreaUserEditWait' style='width: 80%; height:400px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                $.ajax({
                    type: "Get",
                    url: "/Visitors/PrintVisitorCard",
                    data: { visitorid: glbvid, userid: userid },
                    success: function (html) {
                        $("div#userId").html(html);
                    }
                });
            },
            resizable: false,
            width: 800,
            height: 500,
            modal: true,
            title: "<span class='ui-icon ui-icon-home' style='float:left; margin:1px 5px 0 0'></span>Print Visitor Card",
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

    $('#joinpermission').click(function (e) {
        var com = $("select#CompanyId").val();
        var FromDate = $("input#StartDateTime").val();

        $("div#divuserper").dialog({
            open: function () {
                $("div#divuserper").html("");
                $("div#divuserper").html("<div id='AreaUserEditWait' style='width: 100%; height:800px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
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
            width: 800,
            height: 500,
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
    });

    function SavePersonalData() {

        var isValid = ValidateFormFields();

        if (!isValid) {
            ShowDialog('<%=ViewResources.SharedStrings.UsersValidator %>', 5000);
            return false;
        }
        else {

            $.post("/Visitors/CreatePersonalData", $("#editVisitorPersonalData").serialize(),
                function (response) {
                    ShowDialog('<%=ViewResources.SharedStrings.VisitorCreatedMessage %>', 2000, true);

                }, "json");
        }
        return false;
    }


    function SavePersonalData2() {

        if ($("input#FirstName").val() == "" || ($("input#FirstName").val() != "" && !$("input#FirstName").val().match(/^.{1,20}$/))) {
            $("#FirstName_val").text('!').show().fadeOut(3000);
            ShowDialog('<%=ViewResources.SharedStrings.VisitorFirstNameMessage %>', 5000);
            return false;
        }

        if ($("input#LastName").val() == "" || ($("input#LastName").val() != "" && !$("input#LastName").val().match(/^.{1,20}$/))) {
            $("#LastName_val").text('!').show().fadeOut(3000);
            ShowDialog('<%=ViewResources.SharedStrings.VisitorLastNameMessage %>', 5000);
            return false;
        }

        if (($("input#PhoneNumber").val() != "" && !$("input#PhoneNumber").val().match(/^[0-9]*$/))) {
            $("#PhoneNumber_val").text('!').show().fadeOut(3000);
            ShowDialog('<%=ViewResources.SharedStrings.VisitorPhoneNrMessage %>', 5000);
            return false;
        }

        if (($("input#Email").val() != "" && !$("input#Email").val().match(/[\w-]+@([\w-]+\.)+[\w-]+/))) {
            $("#Email_val").text('!').show().fadeOut(3000);
            ShowDialog('<%=ViewResources.SharedStrings.VisitorEmailMessage %>', 5000);
            return false;
        }

        var FromDate = $("input#StartDateTime").val();
        var ToDate = $("input#StopDateTime").val();

        if (FromDate != "" && ToDate != "") {
            if ((new Date(FromDate) > new Date(ToDate))) {
                $("input#StartDayTime_val").text('!').show().fadeOut(3000);
                $("input#StopDayTime_val").text('!').show().fadeOut(3000);
                ShowDialog('<%=ViewResources.SharedStrings.VisitorDateErrorMessage %>', 5000);
                return false;
            }
        }
        else {
            $("input#StartDayTime_val").text('!').show().fadeOut(3000);
            $("input#StopDayTime_val").text('!').show().fadeOut(3000);
            ShowDialog('<%=ViewResources.SharedStrings.VisitorDateMessage %>', 5000);
            return false;
        }

        $('input#button_save_edit_visitor').attr("disabled", true);
        $('input#button_save_edit_visitor').addClass('ui-state-disabled');
        $.ajax({
            type: "Get",
            data: {
                firstName: $("input#FirstName").val(),
                lastname: $("input#LastName").val(),
                phoneNumber: $("input#PersonalCode").val(),
                email: $("input#Email").val()
            },
            url: "/Visitors/CheckVisitorAlreadyExist",
            success: function (response) {
                if (response == "True") {
                    ShowDialog('<%=ViewResources.SharedStrings.VisitorAlreadyExist %>', 5000);
                    $('input#button_save_edit_visitor').removeAttr("disabled");
                    $('input#button_save_edit_visitor').removeClass('ui-state-disabled');
                    return false;
                }
                else {

                    var isphonenraccessunit, iscarnraccessunit, cardneedreturn;

                    //Visitor Data
                    var company = $("select#CompanyId option:selected").text();
                    var companyId = $("select#CompanyId").val();
                  
                    var userID = $("input#UserId").val();
                    var firstName = $("input#FirstName").val();
                    var lastName = $("input#LastName").val();
                    var carNumber = $("input#CarNumber").val();
                    var carType = $("input#CarType").val();
                    var from = $("input#StartDateTime").val();
                    var to = $("input#StopDateTime").val();
                    var phoneNumber = $("input#PhoneNumber").val();
                    var email = $("input#Email").val();
                    var Comment = $("textarea#Comment").val();
                    var PersonalCode = $("input#PersonalCode").val();
                    var returndate = "";
                    //var accessUnit=$("select#AccessUnittype option:selected").text();

                    if ($('input#IsPhoneNrAccessUnit').prop("checked") == true) {
                        isphonenraccessunit = "true";
                    }
                    else {
                        isphonenraccessunit = "false";
                    }

                    if ($('input#IsCarNrAccessUnit').prop("checked") == true) {
                        iscarnraccessunit = "true";
                    }
                    else {
                        iscarnraccessunit = "false";
                    }

                    if ($('input#CardNeedReturn').prop("checked") == true) {
                        cardneedreturn = "true";
                        returndate = $("input#ReturnDate").val();
                    }
                    else {
                        cardneedreturn = "false";
                        returndate = "";
                    }
                
                    //alert("Visitor Data Test:" + company + "/ " + userID + "/" + firstName + "/" + lastName + "/" + carNumber + "/" + carType + "/" + from + "/" + to + "/" + phoneNumber + "/" + email + "/" + isphonenraccessunit + "/" + iscarnraccessunit + "/" + cardneedreturn + "/" + returndate);

                    $.ajax({
                        type: "Post",
                        url: "/Visitors/CreatePersonalData_2",
                        data:
                        {
                            Company: company,
                            CompanyId: companyId,
                            UserID: userID,
                            FirstName: firstName,
                            LastName: lastName,
                            CarNumber: carNumber,
                            CarType: carType,
                            From: from,
                            To: to,
                            PhoneNumber: phoneNumber,
                            Email: email,
                            IsPhoneNrAccessUnit: isphonenraccessunit,
                            IsCarNrAccessUnit: iscarnraccessunit,
                            ReturnDate: returndate,
                            CardNeedReturn: cardneedreturn,
                            PersonalCode: PersonalCode,
                            Comment: Comment
                        },
                        success: function (response) {
                            if (response.IsSucceed == false) {
                                ShowDialog('<%=ViewResources.SharedStrings.MaxVisitorCountLicence %> ' + response.Count, 5000);
                                $('input#button_save_edit_visitor').removeAttr("disabled");
                                $('input#button_save_edit_visitor').removeClass('ui-state-disabled');
                                return false;
                            }
                            else {
                                var vid = response.Id;
                                glbvid = vid;
                                ShowDialog('<%=ViewResources.SharedStrings.VisitorCreatedMessage %>', 2000, true);

                                $("div#user-modal-dialog").html("");
                                $("div#modal-dialog").html("<div id='AreaUserEditWait' style='width: 100%; height:580px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");

                                newDialogTitle = "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + firstName + ' ' + lastName;
                                $("div#modal-dialog").dialog({
                                    title: newDialogTitle,
                                    resizable: false,
                                    width: 1100,
                                    height: 680
                                });
                                $.ajax({
                                    type: 'GET',
                                    url: '/Visitors/Edit',
                                    cache: false,
                                    data: {
                                        id: glbvid
                                    },
                                    success: function (html) {
                                        $("div#modal-dialog").html(html);
                                    }
                                });
                                $("input:button").button();
                                SubmitPeopleSearch();
                                //ClearFilterFields();
                            }
                        }
                    });
                }
            }
        });
    }

    function loaddetailsedit(vid, Username) {
        $("div#modal-dialog").dialog({
            open: function () {
                $("div#user-modal-dialog").html("");
                $("div#modal-dialog").html("<div id='AreaUserEditWait' style='width: 100%; height:580px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                $.ajax({
                    type: 'GET',
                    url: '/Visitors/Edit',
                    cache: false,
                    data: {
                        id: vid
                    },
                    success: function (html) {
                        $("div#modal-dialog").html(html);
                    }
                });
                $(this).parents('.ui-dialog-buttonpane button:eq(2)').focus();
            },
            resizable: false,
            width: 1000,
            height: 710,
            modal: true,
            title: "<span class='ui-icon ui-icon-home' style='float:left; margin:1px 5px 0 0'></span>" + Username,
            buttons: {                
                '<%= ViewResources.SharedStrings.BtnClose %>': function () {
                    $(this).dialog("close");
                    UpdateUserRow(UserId);
                }
            }
        });
        return false;
    }

    function ClearFilterFields() {
        $('input#FirstName').attr('value', "");
        $('input#LastName').attr('value', "");
        $('input#PhoneNumber').attr('value', "");
        $('input#Email').attr('value', "");
        $('input#CarNumber').attr('value', "");
        $('input#CarType').attr('value', "");
        $('input#UserPermission').attr('value', "");
        $('input#UserId').attr('value', "");
    }

    function IsVisitorAlreadyExist() {
        //check is visitor already exist
        $.ajax({
            type: "Get",
            data: {
                firstName: $("input#FirstName").val(),
                phoneNumber: $("input#PhoneNumber").val(),
                email: $("input#Email").val(),
                stopDate: $("input#StopDateTime").val()
            },
            url: "/Visitors/CheckVisitorAlreadyExist",
            success: function (response) {

                if (response == "True")
                    return true;
                else
                    return false;
            }
        });
        return false
    }

    function CompanyId() {
        // var com = $("#company :selected").text();
        var com = $("select#companyId").value();
        if (com > 0) {
            //$("#companyId").text = com;
            $("select#companyId").value = com;
        }
    }
    function RemoveUserPhoto() {
        $("div#delete-modal-dialog").dialog({
            open: function () {
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
                        data: { id: 0 },
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

    function FirstNameUp() {
        var f = $("input#LoginName").val();
        if (f == "") { $("input#LoginName").val($("input#FirstName").val() + "."); }
        else
            if (f.indexOf(".") != -1) {
                var s = f.split(".");
                s[0] = $("input#FirstName").val();
                $("input#LoginName").val(s[0] + "." + s[1]);
            }
        return false;
    }

    function LastNameUp() {
        var f = $("input#LoginName").val();
        if (f == "") { $("input#LoginName").val($("input#LastName").val() + "."); }
        else
            if (f.indexOf(".") != -1) {
                var s = f.split(".");
                s[1] = $("input#LastName").val();
                $("input#LoginName").val(s[0] + "." + s[1]);
            }
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

        var email = $("input#Email").val();
        if (($("input#Email").val() != "" && !$("input#Email").val().match(/[\w-]+@([\w-]+\.)+[\w-]+/))) {
            ShowDialog('<%=ViewResources.SharedStrings.VisitorEmailMessage %>', 5000);
            $("input#Email").val("");
            return false;
        }
        var userID = $("input#UserId").val();
        if (userID != "" && userID != null && email != "" && email != null) {
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
            $('#divjoinvisitor').append('<input type="button" onclick="joinper()"  value="<%=ViewResources.SharedStrings.JoinPermission %>" />');
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
                $("div#divuserper").html("<div id='AreaUserEditWait' style='width: 100%;height:400px;text-align:center;'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                $.ajax({
                    type: "Get",
                    url: "/Visitors/GetUserID",
                    data: { fromdatetime: FromDate },
                    success: function (html) {
                        $("div#divuserper").html("<div style='width: 100%;'>" + html + "</div>");
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

