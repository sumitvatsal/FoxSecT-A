<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.CardNotFoundViewModel>" %>

<form id="cardNotFoundForm" action="">
<table cellpadding="1" cellspacing="0" style="margin: 0; width: 100%; padding: 1px; border-spacing: 0">
    <tr id = "cardCardTypeRow">
		<td style='width:30%; padding:2px; text-align:right;'><label for='SelectedTypeId'><%:ViewResources.SharedStrings.CardsCardType %>:</label></td>
		<td style='width:70%; padding:2px;'><%=Html.DropDownList("TypeId", Model.CardTypes)%></td>
    </tr>
	<tr id="cardBuildingRow">
        <td style='width: 30%; padding:2px; text-align:right;'><label><%:ViewResources.SharedStrings.CommonBuilding %>:</label></td>
        <td style='width:70%; padding:2px;'><%=Html.DropDownList("BuildingId", new SelectList(Model.Buildings, "Value", "Text", Model.BuildingId))%></td>
    </tr>
	<tr id="cardValidFromRow">
        <td style='width: 30%; padding:2px; text-align:right;'><label><%:ViewResources.SharedStrings.CardsValidFrom %>:</label></td>
        <td style='width:70%; padding:2px;'>
			<%=Html.TextBox("ValidFrom", Model.ValidFrom, new { @id = "validFrom", @style = "width:90px", @class = "date_start" })%>
		</td>
    </tr>
	<tr id="cardValidToRow">
        <td style='width: 30%; padding:2px; text-align:right;'><label><%:ViewResources.SharedStrings.CardsValidTo %>:</label></td>
        <td style='width:70%; padding:2px;'>
			<%=Html.TextBox("ValidTo", Model.ValidTo, new { @id = "validTo", @style = "width:90px", @class = "date_end"})%>
		</td>
    </tr>
</table>
<table cellpadding="1" cellspacing="0" style="margin: 0; width: 100%; padding: 1px; border-spacing: 0">
    <tr>
		<td style='width:50%; padding:2px; text-align:left;'><input type='button' id='button_add_to_free_cards' value='<%=ViewResources.SharedStrings.CardsBtnAddToFreeCards %>' onclick="javascript:AddFreeCard();"/></td>
		<td style='width:50%; padding:2px; text-align:right;'>
			<%if( Model.CanCreateCard){ %>
				<input type='button' id='button_add_to_new_user' value='<%=ViewResources.SharedStrings.CardsBtnAddToNewUser %>' onclick="javascript:AddCardToNewUser();"/>
			<%} %>
		</td>
    </tr>
    <tr>
        <td style='width: 60%; padding:2px; text-align:left;'><%=Html.CheckBox("IsHideThisDialog", Model.IsHideThisDialog)%><label><%:ViewResources.SharedStrings.CommonDontShowDialog %></label></td>
        <td style='width: 70%; padding:2px;'></td>
    </tr>
</table>

</form>

<script type="text/javascript" language="javascript">

  $(document).ready(function () {
  	$("input:button").button();

  	$(".date_start").datepicker({
  		dateFormat: "dd.mm.yy",
  		firstDay: 1,
  		showButtonPanel: true,
  		changeMonth: true,
  		changeYear: true,
  		gotoCurrent: true,
  		minDate: 'Y',
  		        onSelect: function (dateText, inst) {
  		            $(".date_end").datepicker("option", "minDate", dateText);
  		        }
  	});

  	$(".date_end").datepicker({
  	        dateFormat: "dd.mm.yy",
  	        firstDay: 1,
  	        showButtonPanel: true,
  	        changeMonth: true,
  	        changeYear: true,
  	        gotoCurrent: true,
  	    });
        
  });
</script>
