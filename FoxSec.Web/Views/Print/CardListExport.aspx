<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<FoxSec.Web.ViewModels.UserAccessUnitListViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%:ViewResources.SharedStrings.CardsTabName %></title>
    <style type="text/css">
        .xl65 {
            mso-style-parent: style0;
            mso-number-format: 0;
            text-align: left;
        }
    </style>
</head>
<body>
    <table cellpadding="0" cellspacing="0" style="margin: 0; width: 100%; padding: 0px; border: 0px; border-spacing: 0;">
        <thead>
            <tr>
                <th style="text-align: left; border: 0px;">
                    <label><%: ViewResources.SharedStrings.CardsTabName %>:</label>
                </th>
                <th style="text-align: right; border: 0px;">
                    <label><%=Html.Encode(string.Format("{0}: {1}", ViewResources.SharedStrings.PrintDate, DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"))) %></label>
                </th>
            </tr>
        </thead>
    </table>
    &nbsp;
    <table cellpadding="0" cellspacing="0" style="margin: 0; width: 100%; padding: 0px; border-spacing: 0;">
        <thead>
            <tr>
                <% if (Model.IsDispalyColumn1)
                    { %>
                <th style="padding: 2px;text-align: left;">
                    <label><%:ViewResources.SharedStrings.CardsCardStatus %></label><br />
                </th>
                <%} %>
                <% if (Model.IsDispalyColumn8)
                    { %>
                <th style="padding: 2px;text-align: left;">
                    <label><%:ViewResources.SharedStrings.CardsCardReason %></label><br />
                </th>
                <%} %>
                <% if (Model.IsDispalyColumn7)
                    { %>
                <th style="padding: 2px;text-align: left;">
                    <label><%:ViewResources.SharedStrings.CardsCardType %></label><br />
                </th>
                <%} %>
                <% if (Model.IsDispalyColumn2)
                    { %>
                <th style="padding: 2px;text-align: left;">
                    <label><%:ViewResources.SharedStrings.CardsCardCode %></label><br />
                </th>
                <%} %>
                <% if (Model.IsDispalyColumn3)
                    { %>
                <th style="padding: 2px;text-align: left;">
                    <label><%:ViewResources.SharedStrings.UsersName %></label><br />
                </th>
                <%} %>
                <% if (Model.IsDispalyColumn4)
                    { %>
                <th style="padding: 2px;text-align: left;">
                    <label><%:ViewResources.SharedStrings.CommonBuilding %></label><br />
                </th>
                <%} %>
                <% if (Model.IsDispalyColumn5)
                    { %>
                <% if (!Model.User.IsCompanyManager)
                    { %>
                <th style="padding: 2px;text-align: left;">
                    <label><%:ViewResources.SharedStrings.UsersCompany %></label><br />
                </th>
                <% } %>
                <% } %>

                <% if (Model.IsDispalyColumn6)
                    { %>
                <th style="padding: 2px;text-align: left;">
                    <label><%:ViewResources.SharedStrings.CardsValidTo %></label><br />
                </th>
                <% } %>
                <% if (Model.IsDispalyColumn9)
                    { %>
                <th style="padding: 4px;text-align: left;">
                    <label><%:ViewResources.SharedStrings.UsersDeactivationDate %></label><br />
                </th>
                <%} %>
            </tr>
        </thead>
        <% var i = 1; foreach (var card in Model.Cards)
            {
                var bg = (i++ % 2 == 1) ? "style='background-color:#CCC;'" : ""; %>
        <tr <%= bg %>>
            <% if (Model.IsDispalyColumn1)
                { %>
            <td style='padding: 2px;'>
                <%= Html.Encode(card.CardStatus)%>
            </td>
            <% } %>
            <% if (Model.IsDispalyColumn8)
                { %>
            <td style='padding: 2px;'>
                <%= Html.Encode((!card.Active  && !card.Free) ? card.DeactivationReason : "")%>
            </td>
            <% } %>
            <% if (Model.IsDispalyColumn7)
                { %>
            <td style='padding: 2px;'>
                <%= Html.Encode(card.TypeName != null && card.TypeName.Length > 30 ? card.TypeName.Substring(0, 27) + "..." : card.TypeName)%>
            </td>
            <% } %>
            <% if (Model.IsDispalyColumn2)
                { %>
            <td style='padding: 2px;' class="xl65">
                <%= Html.Encode(card.FullCardCode)%>
            </td>
            <% } %>
            <% if (Model.IsDispalyColumn3)
                { %>
            <td style='padding: 2px;'>
                <%= Html.Encode(card.Name)%>
            </td>
            <% } %>
            <% if (Model.IsDispalyColumn4)
                { %>
            <td style='padding: 2px;'>
                <%= Html.Encode(card.Building)%>
            </td>
            <% } %>
            <% if (Model.IsDispalyColumn5)
                { %>
            <% if (!Model.User.IsCompanyManager)
                { %>
            <td style='padding: 2px;'>
                <%= Html.Encode(card.CompanyName)%>
            </td>
            <% } %>
            <% } %>
            <% if (Model.IsDispalyColumn6)
                { %>
            <td style='padding: 2px;'>
                <%= Html.Encode(card.ValidToStr)%>
            </td>
            <% } %>
            <% if (Model.IsDispalyColumn9)
                { %>
            <td style='padding: 4px;'>
                <%= Html.Encode((!card.Active  && !card.Free) ? card.DeactivationDateTime : "")%>
            </td>
            <% } %>
        </tr>
        <% } %>
    </table>
</body>
</html>
