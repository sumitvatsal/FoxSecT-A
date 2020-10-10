<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.RoleEditViewModel>" %>

<table cellpadding="3" cellspacing="3" style="margin: 0; width: 100%; padding: 3px; border-spacing: 4px;" class="TFtable">
    <tr>
        <th style="width: 20px;">ID</th>
        <th style="width: 50%;"><%:ViewResources.SharedStrings.RolesMenuTitle %></th>
        <th style="width: 50%; text-align: right;"><%:ViewResources.SharedStrings.CommonIsAllowed %></th>
    </tr>
    <% int i = 1; int index = 0; foreach (var menu in Model.Role.MenuItems)
        { %>
    <tr <% if (!menu.IsItemAvailable || (Model.IsViewOnlyMode && !menu.IsSelected))
        {%> style="display: none" <% } %>>
        <%if (Model.IsViewOnlyMode)
            {%>
        <td style="width: 20px;"><%= menu.IsSelected ? i++ : 0 %></td>
        <%}
        else
        {%>
        <td style="width: 20px;"><%= menu.IsItemAvailable ? i++ : 0 %></td>
        <%} %>
        <td style="width: 50%;"><%= menu.Text %></td>
        <td style="width: 50%; text-align: right;">
            <%:Html.CheckBox(string.Format("Role.MenuItems[{0}].IsSelected", index), menu.IsSelected)%>
            <%:Html.Hidden(string.Format("Role.MenuItems[{0}].Value",index), menu.Value)%>
            <%:Html.Hidden(string.Format("Role.MenuItems[{0}].IsItemAvailable", index), menu.IsItemAvailable)%>
            <%:Html.Hidden(string.Format("Role.MenuItems[{0}].Text", index), menu.Text)%>
        </td>
    </tr>
    <%index++;
        } %>
</table>

<script type="text/javascript" language="javascript">

    $(document).ready(function () {
        if ($("#IsViewOnlyMode").attr('value').toLowerCase() == 'true') {
            $('#editRole').find('input').each(function () {
                $(this).attr('disabled', 'disabled');
            });

        }
    });
</script>
