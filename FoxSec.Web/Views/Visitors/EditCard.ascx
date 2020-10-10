<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.UserAccessUnitEditViewModel>" %>
<form id="editCard" action="">
<%= Html.Hidden("Id", Model.Card.Id) %>
<%= Html.Hidden("Free", Model.Card.Free) %>
<%= Html.Hidden("UserId", Model.Card.UserId) %>
<%= Html.Hidden("Comp_Id", Model.Card.CompanyId)%>
<%= Html.Hidden("ValidFrom", Model.Card.ValidFrom)%>
<%= Html.Hidden("ValidTo", Model.Card.ValidTo)%>
<%= Html.Hidden("IdforCard", Model.Card.Id) %>

<table cellpadding="1" cellspacing="0" style="margin: 0; width: 100%; padding: 1px; border-spacing: 0">
    <tr>
		<td style='width:30%; padding:2px; text-align:right;'><label for='TypeId'><%:ViewResources.SharedStrings.CardsCardType %>:</label></td>
		<td style='width:70%; padding:2px;'><%=Html.DropDownList("TypeId", new SelectList(Model.Card.CardTypes, "Value", "Text", Model.Card.TypeId), new {style = "width: 90%;" })%></td>
    </tr>
    <tr>
        <td style='width: 30%; padding:2px; text-align:right;'><label><%:ViewResources.SharedStrings.CardsSerDk %>:</label></td>
        <td style='width: 70%; padding:2px;'><%=Html.TextBox("Serial", Model.Card.Serial, new { style = "width:30px", maxlength = '3', onkeyup = "javascript:CardSerValidation();" })%>+
		<%=Html.TextBox("Dk", Model.Card.Dk, new { style = "width:47px", maxlength='5', onkeyup = "javascript:CardDkValidation();" })%>
        </td>
    </tr>
    <tr>
        <td style='width: 30%; padding:2px; text-align:right;'><label><%:ViewResources.SharedStrings.CardsCardCode %>:</label></td>
        <td style='width: 70%; padding:2px;'><%=Html.TextBox("Code", Model.Card.Code, new { style = "width:148px", onkeyup = "javascript:CardCodeValidation1();" })%>
        </td>
    </tr>
    <%if (Model.User.IsSuperAdmin)
            { %>
    <tr>
		<td style='width:30%; padding:2px; text-align:right;'><label for='TypeId'><%:ViewResources.SharedStrings.UsersCompany %>:</label></td>
		<td style='width:70%; padding:2px;'><%=Html.DropDownList("CompanyId", new SelectList(Model.Card.Companies, "Value", "Text", Model.Card.CompanyId), new { style = "width: 90%;" })%></td>
    </tr>
    <%} %>
	<tr>
		<td style='width:30%; padding:2px; text-align:right;'><label for='TypeId'><%:ViewResources.SharedStrings.CommonBuilding %>:</label></td>
		<td style='width:70%; padding:2px;'><%=Html.DropDownList("BuildingId", new SelectList(Model.Card.Buildings, "Value", "Text", Model.Card.BuildingId))%></td>
    </tr>
	<tr id="cardValidFromRow">
        <td style='width: 30%; padding:2px; text-align:right;'><label><%:ViewResources.SharedStrings.CardsValidFrom %>:</label></td>
        <td style='width:70%; padding:2px;'>
			<%=Html.TextBox("ValidFromStr", Model.Card.ValidFromStr, new { @id = "validFromStr", @style = "width:90px", @class = "date_start" })%>
			<%= Html.ValidationMessage("ValidFromStr", null, new { @class = "error" })%>
		</td>
    </tr>
	<tr id="cardValidToRow">
        <td style='width: 30%; padding:2px; text-align:right;'><label><%:ViewResources.SharedStrings.CardsValidTo %>:</label></td>
        <td style='width:70%; padding:2px;'>
			<%=Html.TextBox("ValidToStr", Model.Card.ValidToStr, new { @id = "validToStr", @style = "width:90px", @class = "date_end" })%>
			<%= Html.ValidationMessage("ValidToStr", null, new { @class = "error" })%>
		</td>
    </tr>
    <%if (Model.Card.Opened != null && Model.Card.Closed == null)
        { %>
    <tr id="given">
        <td style='width: 30%; padding:2px; text-align:right;'>
            <label>Card is Given:</label>
        </td>
        <td style='width:70%; padding:2px;'>
           <%:Model.Card.Opened.ToString() %>
          <input type='button'  onclick="CardIsBack()" value='<%=ViewResources.SharedStrings.BtnBack %>' />

        </td>
    </tr>
    <%} %>
    <tr id="cardComment">
        <td style='width: 30%; padding:2px; text-align:right;'><label><%:ViewResources.SharedStrings.CommonComment %>:</label></td>
        <td style='width:70%; padding:2px;'>
			<%=Html.TextArea("Comment", Model.Card.Comment, new { @id = "Comment", @style = "width:148px", @class = "Comment"})%>
            <%= Html.ValidationMessage("ValidToStr", null, new { @class = "error" })%>
		</td>
    </tr>
</table>
</form>
<script type="text/javascript"  >
    function CardIsBack() {
        var Id = $("#IdforCard").val();
        $.ajax({
            type: "Post",
            url: "/Card/CardIsBack",
            data: { id: Id },
            traditional: true,
            success: function (result) {
                if (!result) {
                    alert("error");
                } else
                {
                    $("#given").hide();

                    var d = new Date();
                    d.setDate(d.getDate() - 1);
                    var dateString = d.getDate().toString().replace(/^([0-9])$/, '0$1') + "." + (d.getMonth() + 1).toString().replace(/^([0-9])$/, '0$1') + "." + d.getFullYear().toString();

                    $(".date_end").val(dateString);
                    $(".Comment").val("");
                }
            }
        });
    }
</script>
<script type="text/javascript" language="javascript">
	$(document).ready(function () {
		$(".date_start").datepicker({
			dateFormat: "dd.mm.yy",
			firstDay: 1,
			showButtonPanel: true,
			changeMonth: true,
			changeYear: true,
			minDate: 'Y',
			onSelect: function (dateText, inst) {
			    $(".date_end").datepicker("option", "minDate", dateText);
			    row = $(this).parents("#editCard");
			   
                /// zakon4il zdessssss!!!!

			    var dt1 = parseInt(dateText.substring(0, 2));
			    var mon1 = parseInt(dateText.substring(3, 5));
			    var yr1 = parseInt(dateText.substring(6, 10));
			    var d = new Date(yr1, mon1 - 1, dt1);


			    var year = d.getFullYear();
			    if (localStorage.getItem('DateRange') != null) {
			        var dt = localStorage.getItem('DateRange');

			        dt = parseInt(dt, 10);
			        d.setDate(d.getDate() + dt);
			        var dateString = d.getDate().toString().replace(/^([0-9])$/, '0$1') + "." + (d.getMonth() + 1).toString().replace(/^([0-9])$/, '0$1') + "." + d.getFullYear().toString();
			        row.find(".date_end").val(dateString);
			    } else {

			        year = year + 2;
			        $(".date_end").val("31.12." + year);

			    }
			}
		});

		$(".date_end").datepicker({
			dateFormat: "dd.mm.yy",
			firstDay: 1,
			showButtonPanel: true,
			changeMonth: true,
			changeYear: true,
			minDate: $(".date_start").attr('value'),
			onSelect: function (dateText, inst) {

			    //row = $(this).parents("#userRoleRow");
			    var d1 = $(".date_start").val();
			    var d2 = $(".date_end").val();

			    var minutes = 1000 * 60;
			    var hours = minutes * 60;
			    var day = hours * 24;
			    var pattern = /(\d{2})\.(\d{2})\.(\d{4})/;
			    var dt1 = new Date(d1.replace(pattern, '$3-$2-$1'));
			    var dt2 = new Date(d2.replace(pattern, '$3-$2-$1'));
			    var days = 1 + Math.round((dt2 - dt1) / day);

			    if (!isNaN(days))
			    { localStorage.setItem('DateRange', days); }

			}
		});
	});

	function CardSerValidation() {
	    digitsValidate('Serial');
	    if ($('#Serial').attr('value') > 255) {
	        $('#Serial').attr('value', 255);
	    }/*
		if($('#Serial').attr('value').length == 3) {
			$("#Dk").focus();
		}*/
		if ($('#Serial').attr('value').length != 0) {
			$("#Code").attr('value', "");
		}
		return false;
	}

	function CardDkValidation() {
	    digitsValidate('Dk');
	    if ($('#Dk').attr('value') > 65535) {
	        $('#Dk').attr('value', 65535);
	    }
		if ($('#Dk').attr('value').length != 0) {
			$("#Code").attr('value', "");
		}
		return false;
		
	}

	function CardCodeValidation() {
	    digitsValidate('Code');
	    if ($('#Code').attr('value').length != 0) {
	        $("#Serial").attr('value', "");
	        $("#Dk").attr('value', "");
	    }
	    return false;
	}
	function CardCodeValidation1() {
	    //digitsValidate('Code');
	    if ($('#Code').attr('value').length != 0) {
	        $("#Serial").attr('value', "");
	        $("#Dk").attr('value', "");
	    }
	    return false;
	}
</script>