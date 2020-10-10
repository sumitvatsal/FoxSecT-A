<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.UserDepartmentListViewModel>" %>
<div id="userDepartmentList">
    <%int i = 0;%>
    <input type='button' value='<%=ViewResources.SharedStrings.BtnAddNewDepartments %>' style='font-size: 8pt' onclick="javascript: AddUserDepartment('<%= Model.UserId%>');"
        class='ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only' />
    <form id="userDepartmentsForm" action="">
        <%=Html.Hidden("UserId", Model.UserId)%>
        <table cellpadding="3" cellspacing="3" style="margin: 0; width: 100%; padding: 3px; border-spacing: 3px;">
            <thead>
                <tr>
                    <th style='width: 2%; padding: 2px;'></th>
                    <th style='width: 25%; padding: 2px;'>
                        <label>
                            <%:ViewResources.SharedStrings.UsersDepartment %></label>
                    </th>
                    <th style='width: 40%; padding: 2px;'>
                        <label>
                            <%:ViewResources.SharedStrings.UsersValidation %></label>
                    </th>
                    <th style='width: 28%; padding: 2px;'>
                        <label>
                            <%=ViewResources.SharedStrings.DepartmentsManager %></label>
                    </th>
                    <th style='width: 5%; padding: 2px;'></th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>

        <%foreach (var ud in Model.UserDepartments)
            {%>
        <table cellpadding="3" cellspacing="3" style="margin: 0; width: 100%; padding: 3px; border-spacing: 3px;">
            <tr>
                <td style='width: 2%; padding: 2px;'>
                    <%=Html.CheckBox("UserDepartments[" + i + "].IsForDelete", ud.IsForDelete)%>
                </td>
                <td style='width: 25%; padding: 2px;'>
                    <%= Html.DropDownList("UserDepartments[" + i + "].DepartmentId", new SelectList(Model.Departments, "Id", "Name", ud.DepartmentId), ViewResources.SharedStrings.DefaultDropDownValue, new { @id = "selectedDepartment_" + i, onchange = "javascript:SelectDepartment('" + i + "');" })%>
                </td>
                <td style='width: 40%; padding: 2px;'>
                    <%=Html.TextBox("UserDepartments[" + i + "].ValidFrom", ud.ValidFrom, new { placeholder = "dd.MM.yyyy",@style = "width:90px", @class = "date_start"})%>
        -
        <%=Html.TextBox("UserDepartments[" + i + "].ValidTo", ud.ValidTo, new { placeholder = "dd.MM.yyyy",@style = "width:90px", @class = "date_end"})%>
                </td>
                <td style='width: 28%; padding: 2px;'>
                    <div id='<%="userDepartmentManger_" + i++ %>'><%=ud.Manager %></div>
                </td>
                <td style='width: 5%; padding: 2px;'>
                    <span class="ui-icon ui-icon-closethick" style="cursor: pointer" original-title='<%=string.Format("{0}!", ViewResources.SharedStrings.BtnDelete) %>'
                        onclick="javascript:DeleteUserDepartment(<%= ud.Id %>, <%= Model.UserId %>);" />
                </td>
            </tr>
        </table>
        <%}%>
    </form>

    <script type="text/javascript" language="javascript">

        $(document).ready(function () {
            $(".date_start").click(function (e) { e.preventDefault(); });
            $(".date_end").click(function (e) { e.preventDefault(); });
            $(".select_date").datepicker({
                dateFormat: "dd.mm.yy",
                firstDay: 1,
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,
                onSelect: function (dateText, inst) {
                    $(".select_date").datepicker("option", "minDate", dateText);
                }
            });

            $(".date_start").datepicker({
                dateFormat: "dd.mm.yy",
                firstDay: 1,
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,
                onSelect: function (dateText, inst) {
                    $(".date_end").datepicker("option", "minDate", dateText);
                }
            });

            $(".date_end").datepicker({
                dateFormat: "dd.mm.yy",
                firstDay: 1,
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,
                minDate: $(".date_start").val()
            });
            return false;
        });

        function DeleteUserDepartment(userDepartmentId, userId) {
            $("div#delete-modal-dialog").dialog({
                open: function () {
                    $("div#delete-modal-dialog").html("<%=ViewResources.SharedStrings.CommonConfirmMessage %>");
                },
                resizable: false,
                width: 240,
                height: 140,
                modal: true,
                title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.CommonDeleting %>',
                buttons: {
                '<%=ViewResources.SharedStrings.BtnOk %>': function () {
                        $.post("/Department/DeleteUserDepartment", { userDepartmentId: userDepartmentId, userId: userId });
                        ShowDialog('<%=ViewResources.SharedStrings.DepartmentsUserDepartmentDeletingMessage %>', 2000, true);

                        setTimeout(function () {
                            $.get('/Department/UserDepartmentList', { userId: userId }, function (html) {
                                $("div#userDepartmentList").replaceWith(html);
                            });
                        }, 1000);

                        $(this).dialog("close");
                    },
                '<%=ViewResources.SharedStrings.BtnCancel %>': function () {
                        $(this).dialog("close");
                    }
                }
            });
            return false;
        }

        function AddUserDepartment(userId) {

            ShowDialog('<%=ViewResources.SharedStrings.DepartmentsMessageAddUserDepartment %>', 2000, true);
            $.post("/Department/SaveUserDepartments", $("#userDepartmentsForm").serialize(), function (html) {
                $.ajax({
                    type: 'GET',
                    url: '/Department/AddUserDepartment',
                    cache: false,
                    data: { userId: userId },
                    success: function (html) {
                        $("div#userDepartmentList").html(html);
                    }
                });
            });
            return false;
        }

        function DeleteUsersDepartment(userId) {
            $("div#delete-modal-dialog").dialog({
                open: function () {
                    $("div#delete-modal-dialog").html("");
                    $("div#delete-modal-dialog").html('<%=ViewResources.SharedStrings.CommonConfirmMessage %>');
                },
                resizable: false,
                width: 240,
                height: 140,
                modal: true,
                title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.CommonDeleting %>',
                buttons: {
                '<%=ViewResources.SharedStrings.BtnOk %>': function () {
                        $.post("/Department/DeleteUsers", $("#userDepartmentsForm").serialize());
                        ShowDialog('<%=ViewResources.SharedStrings.DepartmentsUserDepartmentDeletingMessage %>', 2000, true);

                        setTimeout(function () {
                            $.get('/Department/UserDepartmentList', { userId: userId }, function (html) {
                                $("div#userDepartmentList").replaceWith(html);
                            });
                        }, 1000);

                        $(this).dialog("close");
                    },
                '<%=ViewResources.SharedStrings.BtnCancel %>': function () {
                        $(this).dialog("close");
                    }
                }
            });
            return false;
        }

        function SelectDepartment(Id) {
            var depid = $("#selectedDepartment_" + Id).val();
            if (depid == null || depid == "" || depid == undefined) {
                depid = "0";
            }
            setTimeout(function () {
                $.get("/Department/GetDepartmentManager", { departmentId: depid }, function (html) {
                    $("div#userDepartmentManger_" + Id).html(html);
                });
            }, 1000);
            return false;
        }

    </script>
</div>
