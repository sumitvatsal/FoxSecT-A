<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.VisitorListViewModel>" %>
<table id="searchedTableUsers" cellpadding="1" cellspacing="0" style="margin: 0; width: 100%; padding: 1px; border-spacing: 0;">
    <thead>
        <tr style="background-color: black">
            <th style='width: 5%; padding: 2px; color: white'><%:ViewResources.SharedStrings.Status %>
                <input type="checkbox" id="cbx_expVisitorList_1" checked="checked" class="tipsy_we" original-title="Export this column" style="display: none" />
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:VisitorSort(1,0);'></span></li>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:VisitorSort(1,1);'></span></li>
            </th>
            <th style='width: 25%; padding: 2px; color: white'><%:ViewResources.SharedStrings.UsersName %>
                <input type="checkbox" id="cbx_expVisitorList_2" checked="checked" class="tipsy_we" original-title="Export this column" style="display: none" />
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:VisitorSort(2,0);'></span></li>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:VisitorSort(2,1);'></span></li>
            </th>
            
            <th style='color: white; width: <%= (!Model.User.IsCompanyManager && !Model.User.IsDepartmentManager) ? "17%" : Model.User.IsCompanyManager ? "15%" : "34%" %>; padding: 2px;'>
                <%:ViewResources.SharedStrings.UsersCompany %>
                <input type="checkbox" id="cbx_expVisitorList_3" checked="checked" class="tipsy_we" original-title="Export this column" style="display: none" />
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:VisitorSort(3,0);'></span></li>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:VisitorSort(3,1);'></span></li>
            </th>

            <th style='width: 12%; padding: 2px; color: white'><%:ViewResources.SharedStrings.CardsValidFrom %>
                <input type="checkbox" id="cbx_expVisitorList_5" checked="checked" class="tipsy_we" original-title="Export this column" style="display: none" />
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:VisitorSort(5,0);'></span></li>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:VisitorSort(5,1);'></span></li>
            </th>
            <th style='width: 12%; padding: 2px; color: white'><%:ViewResources.SharedStrings.CardsValidTo %>
                <input type="checkbox" id="cbx_expVisitorList_4" checked="checked" class="tipsy_we" original-title="Export this column" style="display: none" />
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:VisitorSort(4,0);'></span></li>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:VisitorSort(4,1);'></span></li>
            </th>

            <th style='width: 13%; padding: 2px; color: white'><%:ViewResources.SharedStrings.ReturnDate %>
                <input type="checkbox" id="cbx_expVisitorList_6" checked="checked" class="tipsy_we" original-title="Export this column" style="display: none" />
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:VisitorSort(6,0);'></span></li>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:VisitorSort(6,1);'></span></li>
            </th>

            <th style='width: 13%; padding: 2px; color: white'><%:ViewResources.SharedStrings.LastChange %>
                <input type="checkbox" id="cbx_expVisitorList_7" checked="checked" class="tipsy_we" original-title="Export this column" style="display: none" />
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:VisitorSort(7,0);'></span></li>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:VisitorSort(7,1);'></span></li>
            </th>

            <th style='width: 5%; color: white' align="center">&nbsp;&nbsp;&nbsp;<li class='ui-state-default ui-corner-all ui-icon-custom' style="height: 18px; width: 18px;"><span class='ui-icon ui-icon-print' style='cursor: pointer; background-position: -161px -96px;' onclick="javascript:TogglePrintUser();"></span></li>
            </th>
        </tr>
    </thead>
    <tbody>
        <% var i = 1; foreach (var user in Model.Visitors)
            {
                var bg = (i++ % 2 == 1) ? "style='background-color:#CCC;'" : ""; %>
        <tr id="VisitorListDataRow" <%= bg %>>

            <td id="VisitorUserStatus" style='padding: 5px;'>

                <%= Html.Encode(user.UserStatus)%>
            </td>
            <td id="VisitorListDataName">
                <%= Html.Encode(string.Format("{0} {1}", user.FirstName, user.LastName)) %>
            </td>

            <td id="VisitorListDataCompany" style='padding: 2px;'>
                <% if (!Model.User.IsDepartmentManager && !Model.User.IsCompanyManager)
                    { %>
                <%= (user.Company == "- select -          ") ? "" : Html.Encode(user.Company)%>
                <% }
                    else
                    {%>
                <%=(user.Company == "- select -          ") ? "" : Html.Encode(user.Company)%>
                <%}%>
            </td>
            <td id="VisitorValidfrom">
                <%= Html.Encode(user.FromDate)%>
            </td>
            <td id="VisitorValidTo">
                <%= Html.Encode(user.ToDate)%>
            </td>
            <td>
                <%= Html.Encode(user.DateReturn)%>
            </td>
            <td>
                <%= Html.Encode(user.ChangeLast)%>
            </td>
            <td style='text-align: right;'>
                <%if (Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewVisitorsReadOnly) && Model.StaticId == -1)
                    {%>
                <span id='button_visi_edit_<%= user.Id %>' class='icon icon_green_go tipsy_we' originaltitle='<%: string.Format("{0} {1} {2}!",ViewResources.SharedStrings.BtnEdit, Html.Encode(user.FirstName), Html.Encode(user.LastName)) %>' onclick='<%=string.Format("javascript:ViewUserReadOnly(\"submit_visitor_user\", {0}, \"{1} {2}\")", user.Id, Html.Encode(user.FirstName), Html.Encode(user.LastName)) %>'></span>
                <%}
                    else
                    { %>
                <span id='button_visi_edit_<%= user.Id %>' class='icon icon_green_go tipsy_we' originaltitle='<%: string.Format("{0} {1} {2}!",ViewResources.SharedStrings.BtnEdit, Html.Encode(user.FirstName), Html.Encode(user.LastName)) %>' onclick='<%=string.Format("javascript:EditUser(\"submit_visitor_user\", {0}, \"{1} {2}\")", user.Id, Html.Encode(user.FirstName), Html.Encode(user.LastName)) %>'></span>
                <%} %>
            </td>
        </tr>
        <% } %>
    </tbody>
    <tfoot>
        <tr>
            <td colspan="5">
                <% Html.RenderPartial("Paginator", Model.Paginator); %>
            </td>
        </tr>
    </tfoot>
</table>

<script type="text/javascript" language="javascript">

    $(document).ready(function () {
        if ($("div#VisitorPrintControlButtons").is(":visible")) {
            $("input[id^=cbx_expVisitorList_][type='checkbox']").each(function () {
                $(this).show();
            });
        }
        $(".tipsy_we").attr("class", function () {
            $(this).tipsy({ gravity: $.fn.tipsy.autoWE, html: true });
        });
        if (newUserCreated == true) { newUserCreated = false; }
    });

    function UpdateUserRow(UserId) {
        $.get('/Visitors/GetUserData', { userId: UserId },
            function (data) {
                $('tr [id*=VisitorListDataRow]').each(function () {
                    btnId = $(this).find('[id*=button_visi_edit_]').attr('id');

                    if (btnId.split("_")[3] == UserId) {
                        $(this).find('[id*=VisitorUserStatus]').html(data.UserStatus);
                        $(this).find('[id*=VisitorListDataName]').html(data.Name);
                        $(this).find('[id*=VisitorListDataCompany]').html(data.Company);
                        $(this).find('[id*=VisitorValidTo]').html(data.ToDate);
                    }
                });
            }, 'json');
    }


    function ViewUserReadOnly(Action, UserId, Username) {
         $("div#modal-dialog").dialog({
            open: function () {
                $("div#user-modal-dialog").html("");
                $("div#modal-dialog").html("<div id='AreaUserEditWait' style='width: 100%; height:580px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                $.ajax({
                    type: 'GET',
                    url: '/Visitors/VisitorReadOnly',
                    cache: false,
                    data: {
                        id: UserId
                    },
                    success: function (html) {
                        $("div#modal-dialog").html(html);
                    }
                });
                $(this).parents('.ui-dialog-buttonpane button:eq(2)').focus();
            },
            resizable: false,
            width: 1100,
            height: 675,
            modal: true,
            title: "<span class='ui-icon ui-icon-home' style='float:left; margin:1px 5px 0 0'></span>" + Username,
            buttons: {                
                '<%= ViewResources.SharedStrings.BtnClose %>': function () {
                    $("div#modal-dialog").dialog("close");
                    UpdateUserRow(UserId);
                }
            }
        });
        return false;
    }
    function EditUser(Action, UserId, Username) {
        $("div#modal-dialog").dialog({
            open: function () {
                $("div#user-modal-dialog").html("");
                $("div#modal-dialog").html("<div id='AreaUserEditWait' style='width: 100%; height:580px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                $.ajax({
                    type: 'GET',
                    url: '/Visitors/Edit',
                    cache: false,
                    data: {
                        id: UserId
                    },
                    success: function (html) {
                        $("div#modal-dialog").html(html);
                    }
                });
                $(this).parents('.ui-dialog-buttonpane button:eq(2)').focus();
            },
            resizable: false,
            width: 1100,
            height: 680,
            modal: true,
            title: "<span class='ui-icon ui-icon-home' style='float:left; margin:1px 5px 0 0'></span>" + Username,
            buttons: {                
                '<%= ViewResources.SharedStrings.BtnClose %>': function () {
                    $("div#modal-dialog").dialog("close");
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
                            $.get('/Visitors/Activate', {}, function (html) {
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
                                dlg1 = $(this);
                                if ($('#selectedReasonId').val() == "") {
                                    ShowDialog("<%=ViewResources.SharedStrings.CommonNoReasonSelectedMessage %>", 2000);
                                  return false;
                              }

                              ShowDialog('<%=ViewResources.SharedStrings.UsersActivatingUsersMessage %>', 2000, true);
                                $(this).dialog("close");
                                $.ajax({
                                    type: "Post",
                                    url: "/Visitors/Activate",
                                    data: { usersIds: usersIds, reasonId: $('#selectedReasonId').val() },
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
                            $.get('/Visitors/Deactivate', {}, function (html) {
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
                                dlg1 = $(this);
                                if ($('#selectedReasonId').val() == "") {
                                    ShowDialog("<%=ViewResources.SharedStrings.CommonNoReasonSelectedMessage %>", 2000);
                                  return false;
                              }

                              ShowDialog('<%=ViewResources.SharedStrings.UsersDeactivatingUsersMessage %>', 2000, true);
                                $(this).dialog("close");
                                $.ajax({
                                    type: "Post",
                                    url: "/Visitors/Deactivate",
                                    data: { usersIds: usersIds, reasonId: $('#selectedReasonId').val() },
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


    function CheckAll() {
        $('input[name=user_checkbox]').each(function () {
            this.checked = !this.checked;
            //  alert($('input[name=checkbox_name]').attr('checked'));
        });
        ManageButtons(<%= Model.FilterCriteria %>);
        return false;
    }

</script>
