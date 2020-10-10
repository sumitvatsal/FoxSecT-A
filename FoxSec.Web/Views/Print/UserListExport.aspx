<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<FoxSec.Web.ViewModels.UserListViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%:ViewResources.SharedStrings.UsersTabName %></title>
</head>
<body>
    <table cellpadding="0" cellspacing="0" style="margin: 0; width: 100%; padding: 0px; border-spacing: 0;">
        <thead>
            <tr>
                <th style="text-align: left;">
                    <label><%:ViewResources.SharedStrings.UsersTabName %>:</label>
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
                <% if (Model.IsDispalyColumn6)
                    { %>
                <th style="text-align: left;">
                    <label><%:ViewResources.SharedStrings.UsersUserStatus %></label><br />
                </th>
                <%} %>
                <% if (Model.IsDispalyColumn1)
                    { %>
                <th style="text-align: left;">
                    <label><%:ViewResources.SharedStrings.UsersName %></label><br />
                </th>
                <%} %>
                <th style="text-align: left;">
                    <label><%:ViewResources.SharedStrings.PermissionName %></label>
                </th>
                <% if (Model.IsDispalyColumn2)
                    { %>
                <th style="text-align: left;">
                    <label><%:ViewResources.SharedStrings.UsersCardNo %></label>
                    <br />
                </th>
                <%} %>
                <% if (Model.IsDispalyColumn3)
                    { %>
                <th style="text-align: left;">
                    <% if (!Model.User.IsDepartmentManager && !Model.User.IsCompanyManager)
                        { %>
                    <label><%:ViewResources.SharedStrings.UsersCompany %></label><br />
                    <% }
                        else
                        {%>
                    <label><%: Model.User.IsCompanyManager ? ViewResources.SharedStrings.UsersDepartment : ViewResources.SharedStrings.UsersTitle%></label><br />
                    <%}%>
                </th>
                <% } %>
                <% if (Model.IsDispalyColumn4)
                    { %>
                <th style="text-align: left;">
                    <label><%: (!Model.User.IsDepartmentManager && !Model.User.IsCompanyManager) ? ViewResources.SharedStrings.UsersDepartment : Model.User.IsCompanyManager ? ViewResources.SharedStrings.UsersTitle : ViewResources.SharedStrings.Validation%></label><br />
                </th>
                <% } %>
                <% if (Model.IsDispalyColumn5)
                    { %>
                <th style="text-align: left;">
                    <label><%: Model.User.IsCompanyManager ? ViewResources.SharedStrings.Validation : ViewResources.SharedStrings.UsersRole%></label><br />
                </th>
                <% } %>
            </tr>
        </thead>


        <% var i = 1; foreach (var user in Model.Users)
            {
                var bg = (i++ % 2 == 1) ? "style='background-color:#CCC;'" : ""; %>
        <tr <%= bg %>>
            <% if (Model.IsDispalyColumn6)
                { %>
            <td style='padding: 2px;'>
                <%= Html.Encode(user.UserStatus) %>
            </td>
            <% } %>
            <% if (Model.IsDispalyColumn1)
                { %>
            <td style='padding: 2px;'>
                <%= Html.Encode(string.Format("{0} {1}", user.FirstName, user.LastName)) %>
            </td>
            <% } %>
            <td style='padding: 2px;'>
                <%= Html.Encode(user.UserPermissionGroupName) %>
            </td>
            <% if (Model.IsDispalyColumn2)
                { %>
            <td style='padding: 2px;'>
                <%= Html.Encode(user.CardNumber) %>
            </td>
            <% } %>
            <% if (Model.IsDispalyColumn3)
                { %>
            <td style='padding: 2px;'>
                <% if (!Model.User.IsDepartmentManager && !Model.User.IsCompanyManager)
                    { %>
                <%= Html.Encode(user.CompanyName)%>
                <% }
                    else
                    {%>
                <%= Html.Encode(Model.User.IsCompanyManager ? user.DepartmentName : user.TitleName)%>
                <%}%>
            </td>
            <% } %>
            <% if (Model.IsDispalyColumn4)
                { %>
            <td style='padding: 2px;'>
                <%= Html.Encode((!Model.User.IsDepartmentManager && !Model.User.IsCompanyManager) ? user.DepartmentName : Model.User.IsCompanyManager ? user.TitleName : user.ValidToStr)%>
            </td>
            <% } %>
            <% if (Model.IsDispalyColumn5)
                { %>
            <td style='padding: 2px;'>
                <%= Html.Encode(Model.User.IsCompanyManager ? user.ValidToStr : user.Roles)%>
            </td>
            <% } %>
        </tr>
        <% } %>
    </table>

</body>
</html>
