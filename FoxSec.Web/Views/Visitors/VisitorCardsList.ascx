<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.UserAccessUnitListViewModel>" %>
<table id="userCardsTable" cellpadding="1" cellspacing="0" style="margin: 0; width: 100%; padding: 1px; border-spacing: 0">
    <tr>
        <%--  <th style='width:5%; padding: 2px;'></th>--%>
        <th style='width: 20%; padding: 5px;'><%:ViewResources.SharedStrings.CardsCardType %></th>
        <th style='width: 20%; padding: 5px;'><%:ViewResources.SharedStrings.CardCode %></th>
        <th style='width: 20%; padding: 5px;'><%:ViewResources.SharedStrings.UsersValidation %></th>
        <th style='width: 10%; padding: 5px;'><%:ViewResources.SharedStrings.CommomStatus %></th>
       <%-- <th style='width: 10%; padding: 2px;'></th>--%>
    </tr>
    <% foreach (var card in Model.Cards)
        { %>
    <tr>
        <td style='width: 20%; padding: 5px;'>
            <%= Html.Encode(card.TypeName != null && card.TypeName.Length > 30 ? card.TypeName.Substring(0, 27) + "..." : card.TypeName)%>
        </td>
        <td style='width: 20%; padding: 5px;'>
              <%= Html.Encode(card.FullCardCode) %>
        </td>
        <td style='width: 20%; padding: 5px;'>
            <%= Html.Encode(card.ValidFromStr) %> - <%= Html.Encode(card.ValidToStr) %>
        </td>
        <td style='width: 10%; padding: 5px;'>
            <%= Html.Encode(card.CardStatus) %>
        </td>
      <%--  <td style='width: 10%; padding: 2px;'>
            <span id='button_card_edit_<%= card.Id %>' class='icon icon_green_go tipsy_we' original-title='<%=string.Format("{0} {1}!", ViewResources.SharedStrings.BtnEdit, card.Name) %>' onclick="javascript:EditCard(<%= card.Id %>);"></span>
        </td>--%>
    </tr>
    <% 
        } %>
</table>

