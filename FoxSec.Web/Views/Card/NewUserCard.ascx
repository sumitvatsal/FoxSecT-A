<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.UserAccessUnitEditViewModel>" %>
<form id="addNewUserCard" action="">
    <%=Html.Hidden("UserId", Model.Card.UserId)%>
    <%=Html.Hidden("FirstName", Model.Card.FirstName)%>
    <%=Html.Hidden("LastName", Model.Card.LastName)%>
    <%=Html.Hidden("PersonalCode", Model.Card.PersonalCode)%>
    <table cellpadding="3" cellspacing="3" style="margin: 0; width: 100%; padding: 3px; border-spacing: 3px;">
        <tr>
            <td style='width: 30%; padding: 2px; text-align: right;'>
                <label for='SelectedTypeId'><%:ViewResources.SharedStrings.CardsCardType %>:</label></td>
            <td style='width: 70%; padding: 2px'><%=Html.DropDownList("TypeId", new SelectList(Model.Card.CardTypes, "Value", "Text", Model.Card.TypeId) )%></td>
        </tr>
        <tr>
            <td style='width: 30%; padding: 2px; text-align: right;'>
                <label><%:ViewResources.SharedStrings.CardsSerDk %>:</label></td>
            <td style='width: 70%; padding: 2px;'>
                <%--<%=Html.TextBox("Serial", Model.Card.Serial, new { @style = "width:30px",  onkeyup="javascript:UserCardSerValidation();", maxlength = '3' })%>--%>
                <%=Html.TextBox("Serial", Model.Card.Serial, new { @style = "width:30px",onchange="chkserlimit()",  onkeypress="return UserCardSerValidation(event);", maxlength = '3' })%>
			+ <%--<%=Html.TextBox("Dk", Model.Card.Dk, new { @style = "width:47px",   onkeyup="javascript:UserCardDkValidation();", maxlength = '5' })%>--%>
                <%=Html.TextBox("Dk", Model.Card.Dk, new { @style = "width:47px",onchange="chkdklimit()",   onkeypress="return UserCardDkValidation(event);", maxlength = '5' })%>
                <%= Html.ValidationMessage("Serial", null, new { @class = "error" })%>
                <%= Html.ValidationMessage("Dk", null, new { @class = "error" })%>
            </td>
        </tr>
        <tr>
            <td style='width: 30%; padding: 2px; text-align: right;'>
                <label><%:ViewResources.SharedStrings.CardsCardCode %>:</label></td>
            <td style='width: 70%; padding: 2px;'>
                <%=Html.TextBox("Code", Model.Card.Code, new { @style = "width:148px", onkeyup = "javascript:UserCardCodeValidation();" })%>
                <%= Html.ValidationMessage("Code", null, new { @class = "error" })%>
            </td>
        </tr>
        <tr>
            <td style='width: 30%; padding: 2px; text-align: right;'>
                <label><%:ViewResources.SharedStrings.CommonBuilding %>:</label></td>
            <td style='width: 70%; padding: 2px;'><%=Html.DropDownList("BuildingId", new SelectList(Model.Card.Buildings, "Value", "Text", Model.Card.BuildingId))%></td>
        </tr>
        <tr>
            <td style='width: 30%; padding: 2px; text-align: right;'>
                <label><%:ViewResources.SharedStrings.MainCard %>:</label></td>
            <td style='width: 70%; padding: 2px;'>
                <%=Html.CheckBox("IsMainUnit", Model.Card.IsMainUnit)%>
            </td>
        </tr>
        <tr id="cardValidFromRow">
            <td style='width: 30%; padding: 2px; text-align: right;'>
                <label><%:ViewResources.SharedStrings.CardsValidFrom %>:</label></td>
            <td style='width: 70%; padding: 2px;'>
                <%=Html.TextBox("ValidFromStr", Model.Card.ValidFromStr, new { @id = "validFromStr", @style = "width:90px", @class = "date_start" })%>
                <%= Html.ValidationMessage("ValidFromStr", null, new { @class = "error" })%>
               
            </td>
        </tr>
        <tr id="cardValidToRow">
            <td style='width: 30%; padding: 2px; text-align: right;'>
                <label><%:ViewResources.SharedStrings.CardsValidTo %>:</label></td>
            <td style='width: 70%; padding: 2px;'>
                <%=Html.TextBox("ValidToStr", Model.Card.ValidToStr, new { @id = "validToStr", @style = "width:90px", @class = "date_end" })%>
                <%= Html.ValidationMessage("ValidToStr", null, new { @class = "error" })%>
                <input type="checkbox" id="chksetdefault" />&nbsp;
                <label>Save as Default</label>
            </td>
        </tr>
    </table>
</form>

<script type="text/javascript">

    $(document).ready(function () {

        $(".date_start").datepicker({
            dateFormat: "dd.mm.yy",
            firstDay: 1,
            showButtonPanel: true,
            changeMonth: true,
            changeYear: true,
            gotoCurrent: true,
            minDate: 'Y',
            onSelect: function (dateText, inst) {
                row = $(this).parents("#addNewUserCard");
                row.find(".date_end").datepicker("option", "minDate", dateText);

                var dt1 = parseInt(dateText.substring(0, 2));
                var mon1 = parseInt(dateText.substring(3, 5));
                var yr1 = parseInt(dateText.substring(6, 10));
                var d = new Date(yr1, mon1 - 1, dt1);

                var year = d.getFullYear();

                var dt = localStorage.getItem('DateRange');

                if (dt != null) {

                    var dt = localStorage.getItem('DateRange');

                    dt = parseInt(dt, 10);
                    d.setDate(d.getDate() + dt);
                    var dateString = d.getDate().toString().replace(/^([0-9])$/, '0$1') + "." + (d.getMonth() + 1).toString().replace(/^([0-9])$/, '0$1') + "." + d.getFullYear().toString();
                    row.find(".date_end").val(dateString);
                } else {

                    year = year + 2;
                    row.find(".date_end").val("31.12." + year);
                }
            }
        });

        $(".date_end").datepicker({

            dateFormat: "dd.mm.yy",

            firstDay: 1,
            showButtonPanel: true,
            changeMonth: true,
            changeYear: true,
            gotoCurrent: true,
            //onSelect: function (dateText, inst) {


            //    row = $(this).parents("#addNewUserCard");
            //    var d1 = row.find(".date_start").val();
            //    var d2 = row.find(".date_end").val();

            //    var minutes = 1000 * 60;
            //    var hours = minutes * 60;
            //    var day = hours * 24;
            //    var pattern = /(\d{2})\.(\d{2})\.(\d{4})/;
            //    var dt1 = new Date(d1.replace(pattern, '$3-$2-$1'));
            //    var dt2 = new Date(d2.replace(pattern, '$3-$2-$1'));
            //    var days = 1 + Math.round((dt2 - dt1) / day);

            //    if (!isNaN(days)) { localStorage.setItem('DateRange', days); }

            //}
        });

        $('#chksetdefault').click(function () {
            if ($(this).is(":checked")) {
                row = $(this).parents("#addNewUserCard");
                var d1 = row.find(".date_start").val();
                var d2 = row.find(".date_end").val();

                var minutes = 1000 * 60;
                var hours = minutes * 60;
                var day = hours * 24;
                var pattern = /(\d{2})\.(\d{2})\.(\d{4})/;
                var dt1 = new Date(d1.replace(pattern, '$3-$2-$1'));
                var dt2 = new Date(d2.replace(pattern, '$3-$2-$1'));
                var days = Math.round((dt2 - dt1) / day);
                if (!isNaN(days)) { localStorage.setItem('DateRange', days); }
            }
            else if ($(this).is(":not(:checked)")) {
                localStorage.clear('DateRange');
            }
        });

        row = $(this).parents("#addNewUserCard");

        var d = new Date();
        var year = d.getFullYear();
        if (localStorage.getItem('DateRange') != null) {
            var dt = localStorage.getItem('DateRange');

            //rabotaet
            dt = parseInt(dt, 10);
            d.setDate(d.getDate() + dt);
            var dateString = d.getDate().toString().replace(/^([0-9])$/, '0$1') + "." + (d.getMonth() + 1).toString().replace(/^([0-9])$/, '0$1') + "." + d.getFullYear().toString();
            row.find(".date_end").val(dateString);
            $("#validToStr").val(dateString);

        } else {

            year = year + 2;
            row.find(".date_end").val("31.12." + year);

        }

        $("#Serial").focus();
    });

    function ValidateSer() {

        var srno = $('#Serial').val();
        if (srno == "" || srno == null) {
        }
        else {
            if (parseInt(srno) > 255) {
                alert("SER should not greater than 255!!");
                $('#Serial').val("");
                $('#Serial').focus();
                return false;
            }
            else {
                if (($('#Serial').length > 0) && ($('#Serial').length < 3)) {
                    var ser;
                    ser = $('#Serial').attr('value');
                    while (ser.length < 3) {
                        ser = '0' + ser;
                    }
                    $('#Serial').attr('value', ser);
                }
            }
        }
    }
    function ValidateDk() {

        if (($('#Dk').length > 0) && ($('#Dk').length < 5)) {
            var dk;
            dk = $('#Dk').attr('value');
            while (dk.length < 5) {
                dk = '0' + dk;
            }
            $('#Dk').attr('value', dk);
        }
    }

    function UserCardSerValidation(evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            if (charCode == 44) { // KeyCode For comma is 188
                $('#Dk').focus();
                return false;
            }
            else {
                return false;
            }
        }
        else {
            if ($('#Serial').val().length == 3) {
                $("#Dk").focus();
            }
            return true;
        }

        //digitsValidate('Serial');

    }

    function chkserlimit() {
        var sr = $('#Serial').val();

        if (parseInt(sr) > 255) {
            $('#Serial').val("255");
        }
        if (sr.length != 0) {
            $("#Code").val("");
        }
    }

    function chkdklimit() {
        var sr = $('#Dk').val();

        if (parseInt(sr) > 65535) {
            $('#Dk').val("65535");
        }
        if (sr.length != 0) {
            $("#Code").val("");
        }
    }

    function UserCardDkValidation(evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        else {
            return true;
        }
    }

    $('#TypeId').change(function () {
        if ($('#TypeId').val() == "7") {
            $('#Code').val("");
        }
    });
    function UserCardCodeValidation() {
        if ($('#TypeId').val() == "7") {
            var $th = $('#Code');
            $th.val($th.val().replace(/[^a-zA-Z0-9]/g, function (str) { return ''; }));
            var cval = $('#Code').val();
            $('#Code').val(cval.toUpperCase());
        }
        //digitsValidate('Code');
        if ($('#Code').val().length != 0) {
            $("#Serial").val("");
            $("#Dk").val("");
        }
        return false;
    }

</script>
