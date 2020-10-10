<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.UserAccessUnitEditViewModel>" %>
<form id="createNewCard" action="">
    <table cellpadding="1" cellspacing="0" style="margin: 0; width: 100%; padding: 1px; border-spacing: 0">
        <tr>
            <td style='width: 30%; padding: 2px; text-align: right;'>
                <label for='SelectedTypeId'><%:ViewResources.SharedStrings.CardsCardType %>:</label></td>
            <td style='width: 70%; padding: 2px;'><%=Html.DropDownList("TypeId", Model.Card.CardTypes)%></td>
        </tr>
        <tr>
            <td style='width: 30%; padding: 2px; text-align: right;'>
                <label><%:ViewResources.SharedStrings.CardsSerDk %>:</label></td>
            <td style='width: 70%; padding: 2px;'>
                <%=Html.TextBox("Serial", Model.Card.Serial, new { @style = "width:30px",onchange="chkserlimit()",  onkeypress="return UserCardSerValidation(event);", maxlength = '3' })%>
			+<%=Html.TextBox("Dk", Model.Card.Dk, new { @style = "width:47px",onchange="chkdklimit()",   onkeypress="return UserCardDkValidation(event);", maxlength = '5' })%>
                <%= Html.ValidationMessage("Serial", null, new { @class = "error" })%>
                <%= Html.ValidationMessage("Dk", null, new { @class = "error" })%>
			
            </td>
        </tr>
        <tr>
            <td style='width: 30%; padding: 2px; text-align: right;'>
                <label><%:ViewResources.SharedStrings.CardsCardCode %>:</label></td>
            <td style='width: 70%; padding: 2px;'>
                <%=Html.TextBox("Code", Model.Card.Code, new { @style = "width:148px", onkeyup = "javascript:UserCardCodeValidation();"  })%>
                <%= Html.ValidationMessage("Code", null, new { @class = "error" })%>
            </td>
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
            </td>
        </tr>
        <% if (!Model.isCompanyManager)
            { %><tr>
                <% }
                    else
                    { %>
        <tr style='display: none'>
            <% } %>
            <td style='width: 30%; padding: 2px; text-align: right;'>
                <label for='SelectedCompanyId'><%:ViewResources.SharedStrings.UsersCompany %>:</label></td>
            <td style='width: 70%; padding: 2px;'><%=Html.DropDownList("CompanyId", Model.Card.Companies, new { onchange = "javascript:ChangeCompany(this)", style = "width:72%" })%></td>
        </tr>
        <tr>
            <td style='width: 30%; padding: 2px; text-align: right;'>
                <label><%:ViewResources.SharedStrings.CommonBuilding %>:</label></td>
            <td style='width: 70%; padding: 2px;'><%=Html.DropDownList("BuildingId", new SelectList(Model.Card.Buildings, "Value", "Text", Model.Card.BuildingId))%></td>
        </tr>
        <tr>
            <td style='width: 30%; padding: 2px; text-align: right; vertical-align: top;'>
                <label for='FirstName'><%:ViewResources.SharedStrings.UsersFirstName %>:</label></td>
            <td style='width: 70%; padding: 2px;'>
                <%=Html.TextBox("FirstName", Model.Card.FirstName, new { @style = "width:70%" })%>
                <%= Html.ValidationMessage("FirstName", null, new { @class = "error" })%>
            </td>
        </tr>
        <tr>
            <td style='width: 30%; padding: 2px; text-align: right; vertical-align: top;'>
                <label for='LastName'><%:ViewResources.SharedStrings.UsersLastName %>:</label></td>
            <td style='width: 70%; padding: 2px;'>
                <%=Html.TextBox("LastName", Model.Card.LastName, new { @style = "width:70%" })%>
                <%= Html.ValidationMessage("LastName", null, new { @class = "error" })%>
            </td>
        </tr>
        <tr>
            <td style='width: 30%; padding: 2px; text-align: right; vertical-align: top;'>
                <label for='PersonalCode'><%:ViewResources.SharedStrings.UsersPersonalCode %>:</label></td>
            <td style='width: 70%; padding: 2px;'>
                <%=Html.TextBox("PersonalCode", Model.Card.PersonalCode, new { @style = "width:148px" })%>
                <%= Html.ValidationMessage("PersonalCode", null, new { @class = "error" })%>
            </td>
        </tr>
    </table>
</form>

<script type="text/javascript" language="javascript">

    $(document).ready(function () {
        $('#BuildingId option[value="' + BuildingId + '"]').attr('selected', 'selected');
        $("#CompanyId").val(BuildingId);
        if (CardSer != "" && $("input#Serial").attr('value') == "") {
            $("input#Serial").val(CardSer);
        }
        if (CardDk != "" && $("input#Dk").attr('value') == "") {
            $("input#Dk").val(CardDk);
        }
        if (CardNo != "" && $("input#Code").attr('value') == "") {
            $("input#Code").val(CardNo);
        }
        if (ValidFrom != "" && $("#validFromStr").attr('value') == "") {
            $("#validFromStr").val(ValidFrom);
        }
        if (ValidTo != "" && $("#validToStr").attr('value') == "") {
            $("#validToStr").val(ValidTo);
        }

        $(".date_start").datepicker({
            dateFormat: "dd.mm.yy",
            firstDay: 1,
            showButtonPanel: true,
            changeMonth: true,
            changeYear: true,
            gotoCurrent: ValidFrom == "",
            minDate: 'Y',
            onSelect: function (dateText, inst) {
                alert(dateText);
                $(".date_end").datepicker("option", "minDate", dateText);
            }
        });

        $(".date_end").datepicker({
            dateFormat: "dd.mm.yy",
            firstDay: 1,
            showButtonPanel: true,
            changeMonth: true,
            changeYear: true,
            gotoCurrent: ValidTo == ""
        });
    });

    function CardSerValidation() {
        digitsValidate('Serial');
        if ($('#Serial').val() > 255) {
            $('#Serial').val(255);
        }
        if ($('#Serial').val().length == 3) {
            $("#Dk").focus();
        }
        if ($('#Serial').val().length != 0) {
            $("#Code").val("");
        }
        return false;
    }

    function CardDkValidation() {
        digitsValidate('Dk');
        if ($('#Dk').val() > 65535) {
            $('#Dk').val(65535);
        }
        if ($('#Dk').val().length != 0) {
            $("#Code").val("");
        }
        return false;
    }

    function CardCodeValidation() {
        //digitsValidate('Code');
        if ($('#Code').attr('value').length != 0) {
            $("#Serial").val("");
            $("#Dk").val("");
        }
        return false;
    }

    function ChangeCompany(cntr) {

        comp_id = $(cntr).val();
        $.get('/Card/GetBuildingsByCompany', { companyId: comp_id }, function (data) { $("select#BuildingId").html(data); }, 'json');
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
