<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.TerminallViewModel>" %>

<table id="searchedTableUsers" cellpadding="1" cellspacing="0" style="margin: 0; width: 100%; padding: 1px; border-spacing: 0;">
    <thead>
        <tr>
            <th style='width: 2%; padding: 2px;'>
                 <% if (Model.FilterCriteria != 2)
                     {%>
                    <input id='check_all' name='check_all' type='checkbox' class='tipsy_we' original-title='Select all!' onclick="javascript: CheckAll();"/>
                <%}%>
            </th> 
         
            <th style='width: 5%; padding: 2px;'>
                 <ul>
                 <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:TerminalSort(6,0);'></span></li>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:TerminalSort(6,1);'></span></li>
                </ul>
            </th>
			<th style='width: 20%; padding: 2px;'>
                <ul>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:TerminalSort(1,0);'></span></li>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:TerminalSort(1,1);'></span></li>
                    </ul>
            </th>

            <th style='width: 25%; padding: 2px;'>
               
           
            </th>

			<th style='width: 18%; padding: 2px;'>
                <ul>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:TerminalSort(2,0);'></span></li>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:TerminalSort(2,1);'></span></li>
                    </ul>
            </th>
  	<th style='width: 18%; padding: 2px;'>
                <ul>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:TerminalSort(3,0);'></span></li>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:TerminalSort(3,1);'></span></li>
                    </ul>
            </th>
            	<th style='width: 18%; padding: 2px;'>
                <ul>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:TerminalSort(4,0);'></span></li>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:TerminalSort(4,1);'></span></li>
                    </ul>
            </th>
            <th align="right" style='width: 5%'>
                <%--<li class='ui-state-default ui-corner-all ui-icon-custom' style="height:18px; width:18px;"><span class='ui-icon ui-icon-print' style='cursor: pointer; background-position: -161px -96px;' onclick="javascript:TogglePrintUser();"></span></li>--%>
            </th>
        </tr>
    </thead>
    <tbody>
        <% var i = 1; foreach (var user in Model.terminals)
            {
                var bg = (i++ % 2 == 1) ? "style='background-color:#CCC;'" : ""; %>
                <tr id="userListDataRow" <%= bg %>>
                     
           <td style='width: 2%; padding: 2px;'>
           <% if (Model.FilterCriteria != 2)
               {%>
                <input type='checkbox' id='<%= user.term.Id %>'   name='user_checkbox' onclick='javascript: ManageButtons('<%= Model.FilterCriteria %>')' />
            <% } %>
            
            </td> 
            <td id="TerminalStatus" style='width:9%; padding:2px;'>
                <%= Html.Encode(user.status)%>
            </td>
			<td id="userUserStatus" style='width:9%; padding:2px;'>
                <%= Html.Encode(user.term.Name)%>
            </td>
            <td id="userListDataName" style='width:16%; padding:2px;'>
              <%= Html.Encode(user.term.TerminalId) %>
            </td>
			<td id="userListDataCard" style='width:18%; padding:2px;'>
                <%= Html.Encode(user.CompanyName) %>
            </td>
            <td id="userListDataCompany" style='padding:2px;'>
             <%= Html.Encode(string.Format("{0} {1}", user.MaxUserFName, user.MaxUserLName)) %>
            </td>             
            <td id="userListDataValidation" style='padding:2px;'>
			<%= Html.Encode(user.lastLoginDt)%>
            </td>
           
			 <td style='width: 5%; padding: 2px; text-align: right;'>
                <span id='button_user_edit_<%= user.term.Id %>' class='icon icon_green_go tipsy_we'  onclick= '<%=string.Format("javascript:EditTerminal( {0}, \"{1} \")", user.term.Id,user.term.Name) %>' ></span>
            </td>
        </tr>
        <%} %>
    </tbody>
    <tfoot>
        <tr>
            <td colspan="5">
                <% Html.RenderPartial("Paginator2", Model.Paginator); %>
            </td>
        </tr>
    </tfoot>
</table>

<script type="text/javascript" >

    $(document).ready(function () {
        if ($("div#userPrintControlButtons").is(":visible")) {
            $("input[id^=cbx_expUserList_][type='checkbox']").each(function () {
                $(this).show();
            });
        }
        $(".tipsy_we").attr("class", function () {
            $(this).tipsy({ gravity: $.fn.tipsy.autoWE, html: true });
        });
        if (newUserCreated == true) { newUserCreated = false; }
    });

    function UpdateUserRow(UserId) {
        $.get('/User/GetUserData', { userId: UserId },
            function (data) {
                $('tr [id*=userListDataRow]').each(function () {
                    btnId = $(this).find('[id*=button_user_edit]').attr('id').substr(17);
                    if (btnId == UserId) {
                        $(this).find('[id*=userListDataName]').html(data.Name);
                        $(this).find('[id*=userListDataCard]').html(data.CardNumber);
                        if (data.IsCompanyManager == false && data.IsDepartmentManager == false) {
                            $(this).find('[id*=userListDataCompany]').html(data.CompanyName);
                            $(this).find('[id*=userListDataDepartment]').html(data.DepartmentName);
                        }
                        else {
                            if (data.IsCompanyManager == true) {
                                $(this).find('[id*=userListDataCompany]').html(data.DepartmentName);
                                $(this).find('[id*=userListDataDepartment]').html(data.TitleName);
                            }
                            else {
                                $(this).find('[id*=userListDataCompany]').html(data.TitleName);
                                $(this).find('[id*=userListDataDepartment]').html(data.ValidToStr);
                            }
                        }
                        if (data.IsDepartmentManager == false) {
                            if (data.IsCompanyManager == true) {
                                $(this).find('[id*=userListDataValidation]').html(data.ValidToStr);
                            }
                            else {
                                $(this).find('[id*=userListDataValidation]').html(data.Roles);
                            }
                        }
                    }
                });
            }, 'json');
    }

    function EditTerminal(UserId, Username) {
        $("div#modal-dialog").dialog({
            open: function () {
                $("div#modal-dialog").html("<div id='AreaUserEditWait' style='width: 350px; height:280px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                $("div#user-modal-dialog").html("");

                $.get('/Terminal/Edit', { id: UserId }, function (html) {
                    $("div#modal-dialog").html(html);
                    
                    var rolename = $("#txtHiddenRoleName").val();
                    var res = rolename.match(/Company Manager/g);
                    if (res) {
                        $("#_CompanyId").attr("disabled", "disabled");
                        setTimeout(function () {
                            fetchUserNameByCompanyId($("#_CompanyId").val());
                        }, 1000)
                    }
                    else {
                        $("#_CompanyId").removeAttr("disabled");
                        loadAllUserForSuperAdmin();
                    }
                });
            },
            resizable: true,
            width: 650,
            height: 440,
            modal: true,
            stack: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + Username,
            buttons: {
                '<%=ViewResources.SharedStrings.BtnSave %>': function () {
                    //debugger;

                    var terminal = {
                        ShowScreensaver: $("#_screensaver").prop("checked"),
                        ScreenSaverShowAfter: $("#_ScreensaverShowAfter").val(),
                        TerminalId: $("#_Terminal_Id").val(),
                        //MaxUserId	:		$("#_MaxUser").val(),
                        CompanyId: $("#_CompanyId").val(),
                        ApprovedDevice: $("#_ApprovedDevice").prop("checked"),
                        Name: $("#_TerminalName").val(),
                        InfoKioskMode: $("#_InfoKioskMode").prop("checked"),
                        SoundAlarms: $("#_SoundAlarms").val(),
                        ShowMaxAlarmsFistPage: $("#_ShowMaxAlarmsFistPage").val(),
                        TARegisterBoId: $("#TARegisterBoId").val(),
                    }
                    var TerminalModel = {
                        term: terminal,
                        MaxUserFName: $("#_MaxUser").val()
                    }
                    var terminal_table = JSON.stringify(TerminalModel);
                    $.ajax({
                        type: 'POST',
                        url: '/Terminal/EditTerminal',

                        data: {
                            id: $("#Term_Id").val(),
                            ShowScreensaver: terminal.ShowScreensaver,
                            ScreenSaverShowAfter: terminal.ScreenSaverShowAfter,
                            CompanyId: terminal.CompanyId,
                            ApprovedDevice: terminal.ApprovedDevice,
                            Name: terminal.Name,
                            InfoKioskMode: terminal.InfoKioskMode,
                            SoundAlarms: terminal.SoundAlarms,
                            ShowMaxAlarmsFistPage: terminal.ShowMaxAlarmsFistPage,
                            MaxUser: TerminalModel.MaxUserFName,
                            TerminalId: terminal.TerminalId,
                            TARegisterBoId: terminal.TARegisterBoId
                        },
                        //             dataType: "json",
                        //contentType: "application/json",

                        success: function (a) {
                            if (a == true) {
                                ShowDialog('<%=ViewResources.SharedStrings.TerminalUpdatedAlert %>', 2000, true);
                                SubmitPeopleSearch();
                            }
                            else {
                                ShowDialog('<%=ViewResources.SharedStrings.Error %>', 2000, false);
                            }
                            $("div#modal-dialog").dialog("close");
                        }
                    });
                },
            '<%=ViewResources.SharedStrings.BtnDelete %>': function () {

                    $("div#delete-modal-dialog").dialog({
                        open: function () {
                            $("div#delete-modal-dialog").html('<%=ViewResources.SharedStrings.TerminalDeletionAlert %>');
                        },
                        resizable: false,
                        width: 300,
                        height: 140,
                        modal: true,
                        title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" +'<%=ViewResources.SharedStrings.TerminalDeletingAlert %>',
                        buttons: {
                            '<%=ViewResources.SharedStrings.BtnDelete %>': function () {
                                $("div#delete-modal-dialog").html("<div id='AreaUserEditWait' style='width: 300; height:140; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                                $.ajax({
                                    type: "Post",
                                    url: "/Terminal/DeleteTerminal",
                                    data: { id: UserId },
                                    traditional: true,
                                    success: function () {
                                        ShowDialog('<%=ViewResources.SharedStrings.TerminalDeletedAlert %>', 2000, true);
                                        $("div#delete-modal-dialog").dialog("close");
                                        $("div#modal-dialog").dialog("close");
                                        SubmitPeopleSearch();
                                        $(this).dialog("close");
                                    }
                                });
                            },
          '<%=ViewResources.SharedStrings.BtnCancel %>': function () { $(this).dialog("close"); }
                        }
                    });
                    return false;
                },
            '<%=ViewResources.SharedStrings.BtnCancel %>': function () {
                    $(this).dialog("close");
                }
            },
            close: function () {
                $("div#modal-dialog").html("");
            }
        });
        return false;

       <%-- $("div#modal-dialog").dialog({
            open: function () {//
                $("div#user-modal-dialog").html("");
                $("div#modal-dialog").html("<div id='AreaUserEditWait' style='width: 50%; height:280px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                $.ajax({
      				type: 'GET',
      				url: '/Terminal/Edit',
      				cache: false,
      				data: {
      					id : UserId
      				},
      				success: function (html) {
      					$("div#modal-dialog").html(html);
      				}
      			});
                $(this).parents('.ui-dialog-buttonpane button:eq(2)').focus();
            },
            resizable: false,
            width: 1000,
            height: 710,
            modal: true,
            title: "<span class='ui-icon ui-icon-home' style='float:left; margin:1px 5px 0 0'></span>" + Username,
            buttons: {
               '<%= ViewResources.SharedStrings.BtnClose %>': function () {
                    $(this).dialog("close");
					UpdateUserRow(UserId);
                }
            }
        });
        return false;--%>
    }

    function ManageButtons(filter) {
        var any = 0;
        $('input[name=user_checkbox]').each(function () {
            if (this.checked) any++;
        });
        if ($("#selectedDepartment").size() > 0) {
            if ($("#selectedDepartment").val() != "" && any != 0) {
                if ($('input#button_move_users_to_departament').size() > 0) { $('input#button_move_users_to_departament').fadeIn(); }
            } else {
                if ($('input#button_move_users_to_departament').size() > 0) { $('input#button_move_users_to_departament').fadeOut(); }
            }
        }
        if (any != 0) {
            switch (filter) {
                case 0:
                    $('input#button_delete_user').fadeIn();
                    $('input#button_activate_user').fadeIn();
                    $('input#button_deactivate_user').fadeOut();
                    break;
                case 1:
                    $('input#button_deactivate_user').fadeIn();
                    $('input#button_delete_user').fadeOut();
                    $('input#button_activate_user').fadeOut();
                    break;
                case 2:
                    $('input#button_delete_user').fadeOut();
                    $('input#button_activate_user').fadeOut();
                    $('input#button_deactivate_user').fadeOut();
                    break;
            }
        }
        else {
            $('input#button_delete_user').fadeOut();
            $('input#button_activate_user').fadeOut();
            $('input#button_deactivate_user').fadeOut();
            $('input#check_all').attr('checked', false);
        }
        return false;
    }

    function DeleteUser() {
        var usersIds = new Array();
        var cards = null;
        var msg = '<%=ViewResources.SharedStrings.CommonConfirmMessage %>';

        $("#button_delete_users").addClass("Trans");
        $('input[name=user_checkbox]').each(function () {
            if (this.checked) {
                if ($(this).attr('val') > 0) { cards = false }
                var userId = $(this).attr('id');
                usersIds.push(userId);
            }
        });
        if (cards != null) {
            msg = 'What to do with cards?</br><select id="CardChange"><option value ="1">Move to Free cards</option><option value ="2">Delete them</option></select>';

        }
        $("div#modal-dialog").dialog({
            open: function () {
                $("div#modal-dialog").html(msg);
            },
            resizable: false,
            width: 300,
            height: 200,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>Deleting users",
            buttons: {
                '<%=ViewResources.SharedStrings.BtnOk %>': function () {
                    ShowDialog('<%=ViewResources.SharedStrings.UsersDeletingUsersMessage %>', 2000, true);
                    if ($('#CardChange').val() != "1") {
                        cards = true;
                    }
                    $.ajax({
                        type: "Post",
                        url: "/User/Delete",
                        data: { usersIds: usersIds, cards: cards },
                        traditional: true,
                        success: function () {
                            $("#button_delete_users").removeClass("Trans");
                            setTimeout(function () {
                                SubmitPeopleSearch();
                            }, 1000);
                        }
                    });
                    $(this).dialog("close");
                },
                '<%=ViewResources.SharedStrings.BtnCancel %>': function () { $(this).dialog("close"); $("#button_delete_users").removeClass("Trans"); }
            }
        });
        return false;
    }

    function ActivateUser() {
        var usersIds = new Array();
        $("#button_activate_user").addClass("Trans");
        $('input[name=user_checkbox]').each(function () {
            if (this.checked) {
                var userId = $(this).attr('id');
                usersIds.push(userId);
            }
        });
        $("div#modal-dialog").dialog({
           <%-- open: function () {
                $("div#modal-dialog").html('<%=ViewResources.SharedStrings.CommonConfirmMessage %>');
            },--%>
            resizable: false,
            width: 240,
            height: 140,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + "Approving Devices...",
            buttons: {
                '<%=ViewResources.SharedStrings.BtnOk %>': function () {

                    //ShowDialog('Unapproving devices...', 2000, true);
                    //$(this).dialog("close");
                    $.ajax({
                        type: "Post",
                        url: "/Terminal/Approve_Unapprove",
                        data: { terminals: usersIds, status: 1 },
                        traditional: true,
                        success: function (data) {
                            ShowDialog(data, 2000, true);
                            $("#button_activate_user").removeClass("Trans");
                            setTimeout(function () { SubmitPeopleSearch(); }, 1000);
                        }
                    });

                    $(this).dialog("close");

                },
                '<%=ViewResources.SharedStrings.BtnCancel %>': function () { $(this).dialog("close"); $("#button_activate_user").removeClass("Trans"); }
            }
        });
        return false;
    }

    function DeactivateUser() {
        var usersIds = new Array();
        $("#button_deactivate_user").addClass("Trans");
        $('input[name=user_checkbox]').each(function () {
            if (this.checked) {
                var userId = $(this).attr('id');
                usersIds.push(userId);
            }
        });
        $("div#modal-dialog").dialog({
           <%-- open: function () {
                $("div#modal-dialog").html('<%=ViewResources.SharedStrings.CommonConfirmMessage %>');
            },--%>
            resizable: false,
            width: 240,
            height: 140,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + "Unapproving Devices...",
            buttons: {
                '<%=ViewResources.SharedStrings.BtnOk %>': function () {

                    //ShowDialog('Unapproving devices...', 2000, true);
                    //$(this).dialog("close");
                    $.ajax({
                        type: "Post",
                        url: "/Terminal/Approve_Unapprove",
                        data: { terminals: usersIds, status: 0 },
                        traditional: true,
                        success: function (data) {
                            ShowDialog('Unapproving devices...', 2000, true);
                            $("#button_deactivate_user").removeClass("Trans");
                            setTimeout(function () { SubmitPeopleSearch(); }, 1000);
                        }
                    });

                    $(this).dialog("close");

                },
                '<%=ViewResources.SharedStrings.BtnCancel %>': function () { $(this).dialog("close"); $("#button_deactivate_user").removeClass("Trans"); }
            }
        });
        return false;
    }

    function MoveUserToDepartament() {
        var usersIds = new Array();
        $("#button_move_users_to_departament").addClass("Trans");
        $('input[name=user_checkbox]').each(function () {
            if (this.checked) {
                var userId = $(this).attr('id');
                usersIds.push(userId);
            }
        });
        $("div#modal-dialog").dialog({
            open: function () {
                $("div#modal-dialog").html("");
                $.get('/Department/MoveToDepartment', { departmentId: $("#selectedDepartment").val() }, function (html) {
                    $("div#modal-dialog").html(html);
                });
            },
            resizable: false,
            width: 240,
            height: 140,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>Moving users",
            buttons: {
                "Move": function () {
                    selectedDepartmentId = $("#selectedDepartmentForMove").val();
                    if (selectedDepartmentId == "") return false;
                    ShowDialog("Moving users..", 2000, true);
                    $.ajax({
                        type: "Post",
                        url: "/Department/MoveToDepartment",
                        data: { usersIds: usersIds, oldDepartmentId: $("#selectedDepartment").val(), departmentId: selectedDepartmentId },
                        traditional: true,
                        success: function () {
                            $("#button_move_users_to_departament").removeClass("Trans");
                            setTimeout(function () { SubmitPeopleSearch(); }, 1000);
                        }
                    });
                    $(this).dialog("close");
                },
                Cancel: function () { $(this).dialog("close"); $("#button_move_users_to_departament").removeClass("Trans"); }
            }
        });
        return false;
    }

    function CheckAll() {
        $('input[name=user_checkbox]').each(function () {
            this.checked = !this.checked;
            //  alert($('input[name=checkbox_name]').attr('checked'));
        });
        ManageButtons(<%= Model.FilterCriteria %>);
        return false;
    }

    function ManageButtons(filter) {
        var any = 0;
        $('input[name=user_checkbox]').each(function () {
            if (this.checked) any++;
        });

        if (any != 0) {
            switch (filter) {
                case 0:
                    //$('input#button_delete_user').fadeIn();
                    $('input#button_activate_user').fadeIn();
                    $('input#button_deactivate_user').fadeOut();
                    break;
                case 1:
                    $('input#button_deactivate_user').fadeIn();
                    //$('input#button_delete_user').fadeOut();
                    $('input#button_activate_user').fadeOut();
                    break;
                case 2:
                    //$('input#button_delete_user').fadeOut();
                    $('input#button_activate_user').fadeOut();
                    $('input#button_deactivate_user').fadeOut();
                    break;
            }
        }
        else {
            //$('input#button_delete_user').fadeOut();
            $('input#button_activate_user').fadeOut();
            $('input#button_deactivate_user').fadeOut();
            $('input#check_all').attr('checked', false);
        }
        return false;
    }

</script>

<style type="text/css">
    #modal-dialog {
        height: 410px !important;
    }

    .modal-dialog {
        width: 512px !important;
    }
</style>