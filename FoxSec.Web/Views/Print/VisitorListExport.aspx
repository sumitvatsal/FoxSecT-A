<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<FoxSec.Web.ViewModels.VisitorListViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%:ViewResources.SharedStrings.ViewVisitorsMenu %></title>
</head>
<body>
    <table cellpadding="0" cellspacing="0" style="margin: 0; width: 100%; padding: 0px; border-spacing: 0;">
        <thead>
            <tr>
                <th style="text-align: left;">
                    <label><%:ViewResources.SharedStrings.ViewVisitorsMenu %>:</label>
                </th>
                <th style="text-align: right;">
                    <label><%=Html.Encode(string.Format("{0}: {1}", ViewResources.SharedStrings.PrintDate, DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"))) %></label>
                </th>
            </tr>
        </thead>
    </table>
    <br />
    &nbsp;
    <table cellpadding="0" cellspacing="0" style="margin: 0; width: 100%; padding: 0px; border-spacing: 0;">
        <thead>
            <tr>
                <% if (Model.IsDispalyColumn1)
                    { %>
                <th style="text-align: left; padding: 2px; ">
                    <label><%:ViewResources.SharedStrings.Status %></label><br />
                </th>
                <%} %>
                <% if (Model.IsDispalyColumn2)
                    { %>
                <th style="text-align: left;">
                    <label><%:ViewResources.SharedStrings.UsersName %></label><br />
                </th>
                <%} %>
                <% if (Model.IsDispalyColumn3)
                    { %>
                <th style="text-align: left;">
                    <label><%:ViewResources.SharedStrings.UsersCompany %></label><br />
                </th>
                <%} %>
                <% if (Model.IsDispalyColumn5)
                    { %>
                <th style="text-align: left;">
                    <label><%:ViewResources.SharedStrings.CardsValidFrom %></label><br />
                </th>
                <%} %>
                <% if (Model.IsDispalyColumn4)
                    { %>
                <th style="text-align: left;">
                    <label>Valid To</label><br />
                </th>
                <%} %>
                <% if (Model.IsDispalyColumn6)
                    { %>
                <th style="text-align: left;">
                    <label><%:ViewResources.SharedStrings.ReturnDate %></label><br />
                </th>
                <%} %>
                <% if (Model.IsDispalyColumn7)
                    { %>
                <th style="text-align: left;">
                    <label><%:ViewResources.SharedStrings.LastChange %></label><br />
                </th>
                <%} %>
            </tr>
        </thead>


        <% var i = 1; foreach (var user in Model.Visitors)
            {
                var bg = (i++ % 2 == 1) ? "style='background-color:#CCC;'" : ""; %>
        <tr <%= bg %>>
            <% if (Model.IsDispalyColumn1)
                { %>
            <td style='padding: 5px;'>
                <%= Html.Encode(user.UserStatus) %>
            </td>
            <% } %>
            <% if (Model.IsDispalyColumn2)
                { %>
            <td style='padding: 2px;'>
                <%= Html.Encode(string.Format("{0} {1}", user.FirstName, user.LastName)) %>
            </td>
            <% } %>
            <% if (Model.IsDispalyColumn3)
                { %>
            <td style='padding: 2px;'>
                <%= Html.Encode(user.Company) %>
            </td>
            <% } %>
            <% if (Model.IsDispalyColumn5)
                { %>
            <td style='padding: 2px;'>
                <%= Html.Encode(user.FromDate) %>
            </td>
            <% } %>
            <% if (Model.IsDispalyColumn4)
                { %>
            <td style='padding: 2px;'>
                <%= Html.Encode(user.ToDate) %>
            </td>
            <% } %>
            <% if (Model.IsDispalyColumn6)
                { %>
            <td style='padding: 2px;'>
                <%= Html.Encode(user.DateReturn) %>
            </td>
            <% } %>
            <% if (Model.IsDispalyColumn7)
                { %>
            <td style='padding: 2px;'>
                <%= Html.Encode(user.ChangeLast) %>
            </td>
            <% } %>
        </tr>
        <% } %>
    </table>

</body>
</html>
