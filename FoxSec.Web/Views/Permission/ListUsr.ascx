<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.UserListViewModel>" %>
<table id="searchedTableUsers" cellpadding="1" cellspacing="0" style="margin: 0; width: 100%; padding: 1px; border-spacing: 0;">
    <thead>
        <tr>
            <th style='width: 2%; padding: 2px;'>
                <% if (Model.FilterCriteria != 2) {%>
                    <input id='check_all' name='check_all' type='checkbox' class='tipsy_we' original-title='Select all!' onclick="javascript:CheckAll();"/>
                <%}%>
            </th>
			<th style='width: 9%; padding: 2px;'>
                <input type="checkbox" id="cbx_expUserList_6" checked="checked" class="tipsy_we" original-title="Export this column" style="display:none" />
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:UserSort(0,0);'></span></li>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:UserSort(0,1);'></span></li>
            </th>
            <th style='width: 16%; padding: 2px;'>
                <input type="checkbox" id="cbx_expUserList_1" checked="checked" class="tipsy_we" original-title="Export this column" style="display:none" />
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:UserSort(1,0);'></span></li>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:UserSort(1,1);'></span></li>
            </th>
			<th style='width: 18%; padding: 2px;'>
                <input type="checkbox" id="cbx_expUserList_2" checked="checked" class="tipsy_we" original-title="Export this column" style="display:none" />
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:UserSort(2,0);'></span></li>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:UserSort(2,1);'></span></li>
            </th>
            <th style='width: <%= (!Model.User.IsCompanyManager && !Model.User.IsDepartmentManager) ? "17%" : Model.User.IsCompanyManager ? "15%" : "34%" %>; padding: 2px;'>
                <input type="checkbox" id="cbx_expUserList_3" checked="checked" class="tipsy_we" original-title="Export this column" style="display:none" />
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:UserSort(3,0);'></span></li>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:UserSort(3,1);'></span></li>
            </th>
            <th style='width: <%= (!Model.User.IsCompanyManager && !Model.User.IsDepartmentManager) ? "15%" : Model.User.IsCompanyManager ? "19%" : "17%" %>; padding: 2px;'>
                <input type="checkbox" id="cbx_expUserList_4" checked="checked" class="tipsy_we" original-title="Export this column" style="display:none" />
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:UserSort(4,0);'></span></li>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:UserSort(4,1);'></span></li>
            </th>
            <% if (!Model.User.IsDepartmentManager) { %>
            <th style='width: <%= Model.User.IsCompanyManager ? "17%" : "18%" %>; padding: 2px;'>
                <input type="checkbox" id="cbx_expUserList_5" checked="checked" class="tipsy_we" original-title="Export this column" style="display:none" />
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:UserSort(5,0);'></span></li>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:UserSort(5,1);'></span></li>
            </th>
            <% }%>
            <th align="right" style='width: 5%'>
                <li class='ui-state-default ui-corner-all ui-icon-custom' style="height:18px; width:18px;"><span class='ui-icon ui-icon-print' style='cursor: pointer; background-position: -161px -96px;' onclick="javascript:TogglePrintUser();"></span></li>
            </th>
        </tr>
    </thead>
    <tbody>
        <% var i = 1; foreach (var user in Model.Users) { var bg = (i++ % 2 == 1) ? "style='background-color:#CCC;'" : ""; %>
        <tr id="userListDataRow" <%= bg %>>
            <td style='width: 2%; padding: 2px;'>
            <% if (Model.FilterCriteria != 2) {%>
                <input type='checkbox' id='<%= user.Id %>' name='user_checkbox' onclick='javascript:ManageButtons(<%= Model.FilterCriteria %>);' />
            <% } %>
            </td>
			<td id="userUserStatus" style='width:9%; padding:2px;'>
                <%= Html.Encode(user.UserStatus)%>
            </td>
            <td id="userListDataName" style='width:16%; padding:2px;'>
                <%= Html.Encode(string.Format("{0} {1}", user.FirstName, user.LastName)) %>
            </td>
			<td id="userListDataCard" style='width:18%; padding:2px;'>
                <%= Html.Encode(user.CardNumber) %>
            </td>
            <td id="userListDataCompany" style='width:<%= (!Model.User.IsCompanyManager && !Model.User.IsDepartmentManager) ? "17%" : Model.User.IsCompanyManager ? "15%" : "34%" %>; padding:2px;'>
            <% if (!Model.User.IsDepartmentManager && !Model.User.IsCompanyManager) { %>
                <%= Html.Encode(user.CompanyName)%>
            <% } else{%>
				<%= Html.Encode(Model.User.IsCompanyManager ? user.DepartmentName : user.TitleName)%>
			<%}%>
            </td>
			<td id="userListDataDepartment" style='width:<%= (!Model.User.IsCompanyManager && !Model.User.IsDepartmentManager) ? "15%" : Model.User.IsCompanyManager ? "19%" : "17%" %>; padding:2px;'>
				<%= Html.Encode((!Model.User.IsDepartmentManager && !Model.User.IsCompanyManager) ? user.DepartmentName : Model.User.IsCompanyManager ? user.TitleName : user.ValidToStr)%>
            </td>
            <% if( !Model.User.IsDepartmentManager) { %>
            <td id="userListDataValidation" style='width:<%= Model.User.IsCompanyManager ? "17%" : "18%" %>; padding:2px;'>
			<%= Html.Encode(Model.User.IsCompanyManager ? user.ValidToStr : user.Roles)%>
            </td>
            <% } %>
			 <td style='width: 5%; padding: 2px; text-align: right;'>
                <span id='button_user_edit_<%= user.Id %>' class='icon icon_green_go tipsy_we' original-title='<%: string.Format("{0} {1} {2}!",ViewResources.SharedStrings.BtnEdit, Html.Encode(user.FirstName), Html.Encode(user.LastName)) %>' onclick= '<%=string.Format("javascript:EditUser(\"submit_edit_user\", {0}, \"{1} {2}\")", user.Id, Html.Encode(user.FirstName), Html.Encode(user.LastName)) %>' ></span>
            </td>
        </tr>
        <% } %>
    </tbody>
    <tfoot>
        <tr>
            <td colspan="5">
               
            </td>
        </tr>
    </tfoot>
</table>

<script type="text/javascript" language="javascript">

    $(document).ready(function () {
        if ($("div#userPrintControlButtons").is(":visible")) {
        $("input[id^=cbx_expUserList_][type='checkbox']").each(function () {
            $(this).show();
        });}
        $(".tipsy_we").attr("class", function () {
            $(this).tipsy({ gravity: $.fn.tipsy.autoWE, html: true });
        });
		if (newUserCreated == true) { newUserCreated = false; }
    });

	function UpdateUserRow(UserId) {
		$.get('/User/GetUserData', { userId: UserId },
		function (data) {
			$('tr [id*=userListDataRow]').each(function() {
				btnId = $(this).find('[id*=button_user_edit]').attr('id').substr(17);
				if( btnId == UserId) {
					$(this).find('[id*=userListDataName]').html(data.Name);
					$(this).find('[id*=userListDataCard]').html(data.CardNumber);
					if( data.IsCompanyManager == false && data.IsDepartmentManager == false) {
						$(this).find('[id*=userListDataCompany]').html(data.CompanyName);
						$(this).find('[id*=userListDataDepartment]').html(data.DepartmentName);
					}
					else {
						if( data.IsCompanyManager == true ) {
							$(this).find('[id*=userListDataCompany]').html(data.DepartmentName);
							$(this).find('[id*=userListDataDepartment]').html(data.TitleName);
						}
						else {
							$(this).find('[id*=userListDataCompany]').html(data.TitleName);
							$(this).find('[id*=userListDataDepartment]').html(data.ValidToStr);
						}
					}
					if( data.IsDepartmentManager == false ) {
						if(data.IsCompanyManager == true ) {
							$(this).find('[id*=userListDataValidation]').html(data.ValidToStr);
						}
						else {
							$(this).find('[id*=userListDataValidation]').html(data.Roles);
						}
					}
				}
			});
		},'json');
	}

    function EditUser(Action, UserId, Username) {
        $("div#modal-dialog").dialog({
            open: function () {
                $("div#user-modal-dialog").html("");
                $("div#modal-dialog").html("<div id='AreaUserEditWait' style='width: 100%; height:580px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                $.ajax({
      				type: 'GET',
      				url: '/User/Edit',
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
                '<%= ViewResources.SharedStrings.BtnExport %> PDF': function () {
                    PrintUserDataPDF(UserId);
                },
                '<%= ViewResources.SharedStrings.BtnExport %> XLS': function () {
                    PrintUserDataXLS(UserId);
                },
                '<%= ViewResources.SharedStrings.BtnClose %>': function () {
                    $(this).dialog("close");
					UpdateUserRow(UserId);
                }
            }
        });
        return false;
    }

    function ManageButtons(filter) {
        var any = 0;
        $('input[name=user_checkbox]').each(function () {
            if (this.checked) any++;
        });
        if( $("#selectedDepartment").size() > 0 ) {
			if ($("#selectedDepartment").val() != "" && any != 0) {
                if ($('input#button_move_users_to_departament').size() > 0) { $('input#button_move_users_to_departament').fadeIn(); }
			} else {
                if($('input#button_move_users_to_departament').size()>0) { $('input#button_move_users_to_departament').fadeOut(); }
            }
		}
        if (any != 0) {
            switch(filter)
                {
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
            else
            {
                $('input#button_delete_user').fadeOut();
                $('input#button_activate_user').fadeOut();
                $('input#button_deactivate_user').fadeOut();
                $('input#check_all').attr('checked', false);
            }
        return false;
    }

    function DeleteUser() {
        var usersIds = new Array();
        $("#button_delete_users").addClass("Trans");
        $('input[name=user_checkbox]').each(function () {
            if (this.checked) {
                var userId = $(this).attr('id');
                usersIds.push(userId);
            }
        });
        $("div#modal-dialog").dialog({
            open: function () {
                $("div#modal-dialog").html('<%=ViewResources.SharedStrings.CommonConfirmMessage %>');
            },
            resizable: false,
            width: 240,
            height: 140,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>Deleting users",
            buttons: {
                '<%=ViewResources.SharedStrings.BtnOk %>': function () {
                    ShowDialog('<%=ViewResources.SharedStrings.UsersDeletingUsersMessage %>', 2000, true);
                    $.ajax({
                        type: "Post",
                        url: "/User/Delete",
                        data: { usersIds: usersIds },
                        traditional: true,
                        success: function () {
                            $("#button_delete_users").removeClass("Trans");
                            setTimeout(function (){
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
            open: function () {
                $("div#modal-dialog").html('<%=ViewResources.SharedStrings.CommonConfirmMessage %>');
            },
            resizable: false,
            width: 240,
            height: 140,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.UsersActivatingUsersTitle %>',
            buttons: {
                '<%=ViewResources.SharedStrings.BtnOk %>': function () {
                    $(this).dialog("close");
					$("div#modal-dialog").dialog({
						open: function () {
						    $("div#modal-dialog").html("");
						    $.get('/User/Activate', { }, function(html) {
						        $("div#modal-dialog").html(html);
						    });
						},
						resizable: false,
						width: 440,
						height: 180,
						modal: true,
						title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.UsersActivatingUsersTitle %>',
						buttons: {
						  '<%=ViewResources.SharedStrings.BtnActivate %>': function () {
							dlg1=$(this);
							if($('#selectedReasonId').val() == ""){
								ShowDialog("<%=ViewResources.SharedStrings.CommonNoReasonSelectedMessage %>", 2000);
								return false;
							}

                            ShowDialog('<%=ViewResources.SharedStrings.UsersActivatingUsersMessage %>', 2000, true);
							$(this).dialog("close");
							$.ajax({
								type: "Post",
								url: "/User/Activate",
								data: { usersIds: usersIds, reasonId : $('#selectedReasonId').val() },
								traditional: true,
								success: function () {
									$("#button_activate_user").removeClass("Trans");
									setTimeout(function () { SubmitPeopleSearch(); }, 1000);
								}
							});
							},
							'<%=ViewResources.SharedStrings.BtnCancel %>': function () { $(this).dialog("close"); $("#button_activate_user ").removeClass("Trans"); }
						 }
					 });
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
            open: function () {
                $("div#modal-dialog").html('<%=ViewResources.SharedStrings.CommonConfirmMessage %>');
            },
            resizable: false,
            width: 240,
            height: 140,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + "<%=ViewResources.SharedStrings.UsersDeactivatingUsersTitle %>",
            buttons: {
                '<%=ViewResources.SharedStrings.BtnOk %>': function () {
				    $(this).dialog("close");
					$("div#modal-dialog").dialog({
						open: function () {
						    $("div#modal-dialog").html("");
						    $.get('/User/Deactivate', { }, function(html) {
						        $("div#modal-dialog").html(html);
						    });
						},
						resizable: false,
						width: 440,
						height: 140,
						modal: true,
						title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.UsersDeactivatingUsersTitle %>',
						buttons: {
						  '<%=ViewResources.SharedStrings.BtnDeactivate %>': function () {
							dlg1=$(this);
							if($('#selectedReasonId').val() == ""){
								ShowDialog("<%=ViewResources.SharedStrings.CommonNoReasonSelectedMessage %>", 2000);
								return false;
							}

                            ShowDialog('<%=ViewResources.SharedStrings.UsersDeactivatingUsersMessage %>', 2000, true);
							$(this).dialog("close");
							$.ajax({
								type: "Post",
								url: "/User/Deactivate",
								data: { usersIds: usersIds, reasonId : $('#selectedReasonId').val() },
								traditional: true,
								success: function () {
									$("#button_deactivate_user").removeClass("Trans");
									setTimeout(function () { SubmitPeopleSearch(); }, 1000);
								}
							});
							},
							'<%=ViewResources.SharedStrings.BtnCancel %>': function () { $(this).dialog("close"); $("#button_deactivate_user ").removeClass("Trans"); }
						 }
					 });
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
                $.get('/Department/MoveToDepartment', { departmentId: $("#selectedDepartment").val() }, function(html) {
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
                if(selectedDepartmentId == "") return false;
                ShowDialog("Moving users..", 2000, true);
                $.ajax({
                        type: "Post",
                        url: "/Department/MoveToDepartment",
                        data: { usersIds: usersIds,oldDepartmentId : $("#selectedDepartment").val(), departmentId : selectedDepartmentId },
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
        });
        ManageButtons(<%= Model.FilterCriteria %>);
        return false;
    }

</script>