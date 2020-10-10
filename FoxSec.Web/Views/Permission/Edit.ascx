<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.PermissionEditViewModel>" %>
<div id="content_edit_permission_form" style='margin: 10px; text-align: center;'>
    <form id="editPermission" action="">
        <table width="100%">
            <%=Html.Hidden("Permission.Id", Model.Permission.Id)%>
            <%if (Model.User.IsSuperAdmin)
                {%>
            <tr>
                <td style='width: 20%; padding: 0 5px; text-align: right;'>
                    <label>Owner Name</label></td>
                <td style='width: 70%; padding: 0 5px;'>
                    <%--<%=Html.DropDownListFor(M => M.UserList, new SelectList(Model.UserList,"Id", "FirstName",Model.User.Id),new { style="width: 250px;" }) %>--%>

                    <%= Html.DropDownList("Permission.UserID", new SelectList(Model.UserList, "Value", "Text", Model.User.Id), ViewResources.SharedStrings.DefaultDropDownValue, new { @style = "width:90%" })%>
                  
                </td>
            </tr>
            <%} %>
            <%else
            { %>
            <tr style="visibility: hidden">
                <td style='width: 20%; padding: 0 5px; text-align: right;'>
                    <label>Owner Name</label></td>
                <td style='width: 70%; padding: 0 5px;'>
                    <%--<%=Html.DropDownListFor(M => M.UserList, new SelectList(Model.UserList,"Id", "FirstName",Model.User.Id),new { style="width: 250px;" }) %>--%>

                    <%= Html.DropDownList("Permission.UserID", new SelectList(Model.UserList, "Value", "Text", Model.User.Id), ViewResources.SharedStrings.DefaultDropDownValue, new { @style = "width:90%" })%>
                  
                </td>
            </tr>
            <%} %>
            <tr>
                <td style='width: 20%; padding: 0 5px; text-align: right;'>
                    <label for='Permission_Name'><%:ViewResources.SharedStrings.UsersName %></label></td>
                <td style='width: 70%; padding: 0 5px;'>
                    <%= Html.TextBox("Permission.Name", Model.Permission.Name, new { @style = "width:80%", @id = "edit_Permission_Name" })%>
                    <%= Html.ValidationMessage("Permission.Name", null, new { @class = "error" })%>
                </td>
            </tr>
        </table>
    </form>
</div>
