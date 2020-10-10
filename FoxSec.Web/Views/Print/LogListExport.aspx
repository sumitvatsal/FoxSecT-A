<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<FoxSec.Web.ViewModels.LogListViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%:ViewResources.SharedStrings.LogTabName %></title>
</head>
<body>
    <table cellpadding="0" cellspacing="0" style="margin: 0; width: 100%; padding: 0px; border: 0px; border-spacing: 0;">
        <thead>
            <tr>
                <th style="text-align: left; border: 0px;">
                    <label><%: ViewResources.SharedStrings.LogTabName %>:</label>
                </th>
                <th style="text-align: right; border: 0px;">
                    <label><%=Html.Encode(string.Format("{0}: {1}", ViewResources.SharedStrings.PrintDate, DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"))) %></label>
                </th>
            </tr>
        </thead>
    </table>

    <br />
    <% if (Model.ReportType == 1)
        {  %>
    <table cellpadding="0" cellspacing="0" style="margin: 0; width: 100%; padding: 0px; border: 1px; border-spacing: 0;" border="1">
        <thead>
            <tr>
                <th style="border: 0px; padding: 3px; width: 30%"></th>
                <th style="text-align: left; border: 0px; padding: 3px; width: 10%">
                    <label><%:ViewResources.SharedStrings.Date %>:  </label>
                </th>
                <th style="text-align: left; border: 0px; padding: 3px; width: 50%">
                    <%= Model.DateFrom %> - <%= Model.DateTo %>
                </th>
            </tr>
            <tr>
                <th style="border: 0px; padding: 3px; width: 30%"></th>
                <th style="text-align: left; border: 0px; padding: 3px; width: 10%">
                    <label><%:ViewResources.SharedStrings.Report %>: </label>
                </th>
                <th style="text-align: left; border: 0px; padding: 3px; width: 50%">
                    <%= Model.Report %>
                </th>
            </tr>
            <% if (!string.IsNullOrEmpty(Model.Company))
                {  %>
            <tr>
                <th style="border: 0px; padding: 3px; width: 30%"></th>
                <th style="text-align: left; border: 0px; padding: 3px; width: 10%">
                    <label for='Search_company'><%:ViewResources.SharedStrings.UsersCompany %>: </label>
                </th>
                <th style="text-align: left; border: 0px; padding: 3px; width: 50%">
                    <%= Model.Company %> 
                </th>
            </tr>
            <% }  %>
            <tr>
                <th style="border: 0px; padding: 3px; width: 30%"></th>
                <th style="text-align: left; border: 0px; padding: 3px; width: 10%">
                    <label><%:ViewResources.SharedStrings.TotalUsersCount %>: </label>
                </th>
                <th style="text-align: left; border: 0px; padding: 3px; width: 50%">
                    <%= Model.TotalUsers %>
                </th>
            </tr>
        </thead>
    </table>
    <% }
        else
        { %>
    <table cellpadding="0" cellspacing="0" style="margin: 0; width: 100%; padding: 0px; border: 1px; border-spacing: 0;">
        <thead>
            <tr>
                <th style="border: 0px; padding: 3px; width: 30%"></th>
                <th style="text-align: left; border: 0px; padding: 3px; width: 10%">
                    <label><%:ViewResources.SharedStrings.Date %>:  </label>
                </th>
                <th style="text-align: left; border: 0px; padding: 3px; width: 50%">
                    <%= Model.DateFrom %> - <%= Model.DateTo %>
                </th>
            </tr>
            <tr>
                <th style="border: 0px; padding: 3px; width: 30%"></th>
                <th style="text-align: left; border: 0px; padding: 3px; width: 10%">
                    <label><%:ViewResources.SharedStrings.Report %>: </label>
                </th>
                <th style="text-align: left; border: 0px; padding: 3px; width: 50%">
                    <%= Model.Report %>
                </th>
            </tr>
            <% if (!string.IsNullOrEmpty(Model.Company))
                {  %>
            <tr>
                <th style="border: 0px; padding: 3px; width: 30%"></th>
                <th style="text-align: left; border: 0px; padding: 3px; width: 10%">
                    <label for='Search_company'><%:ViewResources.SharedStrings.UsersCompany %>: </label>
                </th>
                <th style="text-align: left; border: 0px; padding: 3px; width: 50%">
                    <%= Model.Company %> 
                </th>
            </tr>
            <% }  %>
            <tr>
                <th style="border: 0px; padding: 3px; width: 30%"></th>
                <th style="text-align: left; border: 0px; padding: 3px; width: 10%">
                    <label><%:ViewResources.SharedStrings.TotalUsersCount %>: </label>
                </th>
                <th style="text-align: left; border: 0px; padding: 3px; width: 50%">
                    <%= Model.Items.ToList().Count() %>
                </th>
            </tr>
        </thead>
    </table>
    <% }  %>
            
        &nbsp;
    <table cellpadding="0" cellspacing="0" style="margin: 0; width: 100%; padding: 0px; border-spacing: 0;">
        <thead>
            <tr>
                <th style="text-align: left;">
                    <label><%:ViewResources.SharedStrings.CommonDate %></label><br />
                </th>
                <th style="text-align: left;">
                    <label><%:ViewResources.SharedStrings.CommonBuilding %></label><br />
                </th>
                <th style="text-align: left; padding-left: 2px">
                    <label><%:ViewResources.SharedStrings.LogsNode %></label><br />
                </th>
                <%if (!Model.User.IsCompanyManager)
                    {%>
                <th style="text-align: left;">
                    <label><%:ViewResources.SharedStrings.UsersCompany %></label><br />
                </th>
                <%} %>
                <th style="text-align: left; padding-left: 2px;">
                    <label><%:ViewResources.SharedStrings.UsersName %></label><br />
                </th>
                <th style="text-align: left;">
                    <label><%:ViewResources.SharedStrings.LogsActivity %></label><br />
                </th>
            </tr>
        </thead>
        <% var i = 1; foreach (var log in Model.Items)
            {
                var bg = (i++ % 2 == 1) ? "style='background-color:#CCC;'" : ""; %>
        <tr <%= bg %>>
            <td style='padding: 2px;'>
                <%= Html.Encode(log.EventTimeStr) %>
            </td>
            <td style='padding: 2px;'>
                <%= Html.Encode(log.Building) %>
            </td>
            <td style='padding: 2px;'>
                <%= Html.Encode(log.Node) %>
            </td>
            <%if (!Model.User.IsCompanyManager)
                {%>
            <td style='padding: 2px;'>
                <%= Html.Encode(log.CompanyName) %>
            </td>
            <%} %>
            <td style='padding: 2px;'>
                <%= Html.Encode(log.UserName) %>
            </td>
            <td style='padding: 2px; word-break: break-all;'>
                <%= Html.Encode(log.Action.Trim()) %>
            </td>
        </tr>
        <% } %>
    </table>
</body>
</html>
