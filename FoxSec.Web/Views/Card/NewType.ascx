<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.UserAccessUnitTypeEditViewModel>" %>
<div id="content_add_card_type" style='margin:10px; text-align:center; display:none' >
<form id="createNewCardType" action="">
<table width="100%">
    <tr>
		<td style='width:30%; padding:0 5px; text-align:right;'><label for='Name'><%:ViewResources.SharedStrings.UsersName %></label></td>
		<td style='width:70%; padding:0 5px;'>
			<%=Html.TextBox("CardType.Name", Model.CardType.Name, new { @style = "width:80%" })%>
			<%= Html.ValidationMessage("CardType.Name", null, new { @class = "error" })%>
        </td>
    </tr>
    <tr>
		<td style='width:30%; padding:0 5px; text-align:right; vertical-align:top;'><label for='Description'><%:ViewResources.SharedStrings.CommonDescription %></label></td>
		<td style='width:70%; padding:0 5px;'>
			<%=Html.TextArea("CardType.Description", Model.CardType.Description, new { @style = "width:80%" })%>
			<%= Html.ValidationMessage("CardType.Description", null, new { @class = "error" })%>
        </td>
    </tr>
    <tr>
		<td style='width:30%; padding:0 5px; text-align:right; vertical-align:top;'><label for='IsCardCode'><%:ViewResources.SharedStrings.CardsCardCode %></label></td>
		<td style='width:70%; padding:0 5px;'><%=Html.CheckBox("CardType.IsCardCode", false)%></td>
    </tr>
    <tr>
		<td style='width:30%; padding:0 5px; text-align:right; vertical-align:top;'><label for='IsSerDK'><%:ViewResources.SharedStrings.CardsSerDk %>&nbsp; nr</label></td>
		<td style='width:70%; padding:0 5px;'><%=Html.CheckBox("CardType.IsSerDK", false)%></td>
    </tr>
</table>
</form>

<script type="text/javascript" language="javascript">
    $("div#content_add_card_type").fadeIn("300");
</script>

</div>