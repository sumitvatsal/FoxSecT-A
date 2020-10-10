<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.UserRoleModel>" %>
<%@ Import Namespace="FoxSec.Web.Helpers" %>
<style type="text/css">
	.TFtable{
		border-collapse:collapse; 
	}
	.TFtable td{ 
		padding:3px;
	}
	/* provide some minimal visual accomodation for IE8 and below */
	.TFtable tr{
		background-color:transparent;
	}
	/*  Define the background color for all the ODD background rows  */
	.TFtable tr:nth-child(odd){ 
		background: white;
	}
	/*  Define the background color for all the EVEN background rows  */
	.TFtable tr:nth-child(even){
		background: #CCC;
	}
</style>

<form id="editUserRoles" action="">
    <%= Html.HiddenFor(m=>m.UserId) %>
    <%=Html.HiddenFor(m=>m.IsCurrentUser) %>

    <table>
        <tr>
            <td style='width: 20%; padding: 2px; text-align: right;'>
                <label for='WorkTime'>Card alarm to Email</label></td>
            <td style='width: 40%; padding: 2px;'><%=Html.CheckBox("CardAlarm", Model.CardAlarm)%></td>
            <td style='width: 20%; padding: 2px; text-align: right;'>
                <label for='IsShortTermVisitor'>IsShortTerm Visitor </label>
            </td>
            <td style='width: 40%; padding: 2px;'><%=Html.CheckBox("IsShortTermVisitor", Model.IsShortTermVisitor)%></td>
            <td>
                <% if (!Model.IsCurrentUser)
                    { %>
                <input type='button' value='<%=ViewResources.SharedStrings.BtnSave %>' onclick='EditUserRoles()' />
                <%} %>
            </td>
        </tr>
        <tr>
            <td style='width: 20%; padding: 2px; text-align: right;'>
                <label for='WorkTime'><%:ViewResources.SharedStrings.UsersESeriviceAllowed %></label></td>
            <td style='width: 40%; padding: 2px;'><%=Html.CheckBox("EServiceAllowed", Model.EServiceAllowed)%></td>
            <td style='width: 20%; padding: 2px; text-align: right;'>
                <label for='WorkTime'>Is Visitor</label></td>
            <td style='width: 40%; padding: 2px;'><%=Html.CheckBox("IsVisitor", Model.IsVisitor)%></td>

            <td style='width: 0%; padding: 2px;'></td>
        </tr>
        <tr>
            <td style='width: 20%; padding: 2px; text-align: right;'>
                <label for='ApproveTerminals'>Approve Terminals </label>
            </td>
            <td style='width: 40%; padding: 2px;'><%=Html.CheckBox("ApproveTerminals", Model.ApproveTerminals)%></td>

            <td style='width: 20%; padding: 2px; text-align: right;'>
                <label for='ApproveVisitor'>Approve Visitor</label></td>
            <td style='width: 40%; padding: 2px;'><%=Html.CheckBox("ApproveVisitor", Model.ApproveVisitor)%></td>
           
        </tr>
        <tr>
             <td style='width: 20%; padding: 2px; text-align: right'><label>Cannot Add User</label></td>
            <td style='width: 40%; padding:2px;'><%=Html.CheckBox("CannotAddUsersAndCard", Model.CannotAddUsersAndCard.GetValueOrDefault())%></td>
        </tr>
    </table>
    <br />
    <table cellpadding="3" cellspacing="3" style="margin: 0; width: 100%; padding: 3px; border-spacing: 3px;" class="TFtable">
        <tr>
            <th style='width: 20px; padding: 2px;'><%:ViewResources.SharedStrings.CommonId %></th>
            <th style='width: 20%; padding: 2px;'><%:ViewResources.SharedStrings.UsersRoleTitle %></th>
            <th style='width: 60%; padding: 2px; text-align: center;'><%:ViewResources.SharedStrings.CommonValidationPeriod %></th>
            <th style='width: 20%; padding: 2px; text-align: right;'><%:ViewResources.SharedStrings.CommonIsAllowed %></th>
        </tr>

        <% var i = 1; foreach (var role in Model.Roles)
            { %>
        <tr id="userRoleRow">
            <td style='width: 20px; padding: 2px;'><%= Html.Encode(i++) %></td>
            <td style='width: 20%; padding: 2px; cursor: help;' class='tipsy_we' original-title='<%= role.RoleDescription %>'>
                <%= Html.Encode(role.RoleName) %>
                <%=Html.Hidden(ViewHelper.GetPrefixedName("Roles", "RoleName", Model.Roles.IndexOf(role)), role.RoleName) %>
                <%=Html.Hidden(ViewHelper.GetPrefixedName("Roles", "RoleDescription", Model.Roles.IndexOf(role)), role.RoleDescription) %>
                <%=Html.Hidden(ViewHelper.GetPrefixedName("Roles", "Id", Model.Roles.IndexOf(role)), role.Id) %>
            </td>
            <td style='width: 60%; padding: 2px; text-align: center;'>
                <%=Html.TextBox(ViewHelper.GetPrefixedName("Roles", "ValidFrom", Model.Roles.IndexOf(role)), role.ValidFrom, new { @class = "date_start", style = "width:90px" })%>
		- 
		<%=Html.TextBox(ViewHelper.GetPrefixedName("Roles", "ValidTo", Model.Roles.IndexOf(role)), role.ValidTo, new { @class = "date_end", style = "width:90px" })%>
                <%= Html.ValidationMessage(ViewHelper.GetPrefixedName("Roles", "ValidFrom", Model.Roles.IndexOf(role)), null, new { @class = "error" })%>
                <%= Html.ValidationMessage(ViewHelper.GetPrefixedName("Roles", "ValidTo", Model.Roles.IndexOf(role)), null, new { @class = "error" })%>
            </td>
            <td style='width: 20%; padding: 2px; text-align: right;'>
                <%=Html.Hidden(ViewHelper.GetPrefixedName("Roles", "RoleId", Model.Roles.IndexOf(role)), role.RoleId) %>
                <%=Html.CheckBox(ViewHelper.GetPrefixedName("Roles", "IsSelected", Model.Roles.IndexOf(role)), role.IsSelected,new {@class = "BoxChecked", @onclick = "CheckBox(this)"}) %>
            </td>
        </tr>
        <% } %>
    </table>
</form>

<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        $(".date_start").datepicker({
            dateFormat: "dd.mm.yy",
            firstDay: 1,
            changeMonth: true,
            changeYear: true,
            showButtonPanel: false,
            onSelect: function (dateText, inst) {
                row = $(this).parents("#userRoleRow");
                row.find(".date_end").datepicker("option", "minDate", dateText);

                var dt1 = parseInt(dateText.substring(0, 2));
                var mon1 = parseInt(dateText.substring(3, 5));
                var yr1 = parseInt(dateText.substring(6, 10));
                var d = new Date(yr1, mon1 - 1, dt1);

                var year = d.getFullYear();
                if (localStorage.getItem('DateRange1') != null) {
                    var dt = localStorage.getItem('DateRange1');

                    dt = parseInt(dt, 10);
                    d.setDate(d.getDate() + dt);
                    var dateString = d.getDate().toString().replace(/^([0-9])$/, '0$1') + "." + (d.getMonth() + 1).toString().replace(/^([0-9])$/, '0$1') + "." + d.getFullYear().toString();
                    row.find(".date_end").val(dateString);
                } else {
                    year = year + 2;
                    row.find(".date_end").val("31.12." + year);
                }
            }
        });

        $(".date_end").datepicker({
            dateFormat: "dd.mm.yy",
            firstDay: 1,
            changeMonth: true,
            changeYear: true,
            showButtonPanel: false,
            minDate: $(".date_end").val(),
            onSelect: function (dateText, inst) {
                row = $(this).parents("#userRoleRow");
                var d1 = row.find(".date_start").val();
                var d2 = row.find(".date_end").val();
                var minutes = 1000 * 60;
                var hours = minutes * 60;
                var day = hours * 24;
                var pattern = /(\d{2})\.(\d{2})\.(\d{4})/;
                var dt1 = new Date(d1.replace(pattern, '$3-$2-$1'));
                var dt2 = new Date(d2.replace(pattern, '$3-$2-$1'));
                var days = 1 + Math.round((dt2 - dt1) / day);

                if (!isNaN(days)) { localStorage.setItem('DateRange1', days); }
            }
        });

        $(".tipsy_we").attr("class", function () {
            $(this).tipsy({ gravity: $.fn.tipsy.autoWE, html: true });
        });

        roles_form = $('#editUserRoles');
        if (roles_form.find("#IsCurrentUser").attr('value').toLowerCase() == 'true') {
            roles_form.find('input').each(function () {
                $(this).attr('disabled', 'disabled');
            });
        }
        $("input:button").button();
    });

    function CheckBox(varr) {
        debugger;
        row = $(varr).parents("#userRoleRow");
        var d = new Date();
        var year = d.getFullYear();
        var nowdate = d.getDate().toString().replace(/^([0-9])$/, '0$1') + "." + (d.getMonth() + 1).toString().replace(/^([0-9])$/, '0$1') + "." + d.getFullYear().toString();
        row.find(".date_start").val(nowdate);
        var insta = varr.id;
        var x = document.getElementById(insta).checked;

        //alert(x);
        if (x) {
            if (localStorage.getItem('DateRange') != null) {
                var dt = localStorage.getItem('DateRange');

                dt = parseInt(dt, 10);
                d.setDate(d.getDate() + dt);
                var dateString = d.getDate().toString().replace(/^([0-9])$/, '0$1') + "." + (d.getMonth() + 1).toString().replace(/^([0-9])$/, '0$1') + "." + d.getFullYear().toString();
                row.find(".date_end").val(dateString);
            } else {

                year = year + 2;
                row.find(".date_end").val("31.12." + year);
            }
        } else {
            row.find(".date_end").val("");
            row.find(".date_start").val("");
        }
    }
    function EditUserRoles() {
        debugger;
        var serial = $("#editUserRoles").serialize();
        var cannotAddUserAndCard = $("#CannotAddUsersAndCard").is(":checked");
        $.ajax({
            type: "POST",
            url: "/User/EditRoles",
            dataType: "json",
            data: $("#editUserRoles").serialize(),
            success: function (response) {
                if (response.IsSucceed == true) {
                     ShowDialog(response.Msg, 2000, true);
                     ReloadEditPage();
                }
                else {
                    $("div#tab_edit_user_roles").html(response.viewData);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    }
</script>

