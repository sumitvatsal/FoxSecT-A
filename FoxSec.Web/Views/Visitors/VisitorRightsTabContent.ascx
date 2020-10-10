<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.HomeViewModel>" %>
<div id='tab_user_rights'>
    <%= Html.Hidden("CurrentUserPermissionGroupId") %>
    <%= Html.Hidden("CurrentUserParentPermissionGroupId") %>
    <%= Html.Hidden("CurrentUserBuildingObjectId") %>

    <span class="ui-icon ui-icon-transferthick-e-w" style="cursor: pointer" onclick="javascript:PermTree();"></span>
    <div id='AreaUserPermissionTree' style='position: absolute; top: 85px; left: 15px; margin: 15px 15px 15px 0; padding: 10px; width: 260px; z-index: 1010; display: none'></div>
    <table cellpadding="3" cellspacing="3" style='margin: 0; width: 100%; padding: 3px; border-spacing: 3px;'>
        <tr>
            <td>&nbsp; &nbsp;<%:ViewResources.SharedStrings.UsersCurUserPermGroupName %>: <b><span style="font-size: 16px; color: Green" id='current_user_permission_name'></span></b>
            </td>
            <td>&nbsp; &nbsp;<%:ViewResources.SharedStrings.JoinedUserName %>: <b><span style="font-size: 16px; color: Green" id='joined_user_name'></span></b>
            </td>
        </tr>
      
        <tr>
            <td style='width: 100%; padding: 2px;' colspan="2">
                <br />
                <div id='userPermissionTimeZoneList' style='display: none'></div>
            </td>
        </tr>
    </table>
</div>

<script type="text/javascript" language="javascript">
    var permtreeshow = false;
    $(document).ready(function () {
        $("input:button").button();
        GetUserPermissionGroup();
    });

    function PermTree() {
        if (permtreeshow == false) {
            document.getElementById("UPTZTable").style.width = "70%";
            document.getElementById("UPTZTable").align = "right";

            GetUserPermissionTree($("input#UserId").val(), $("input#CurrentUserPermissionGroupId").val());
            $('div#AreaUserPermissionTree').toggle(500);
            permtreeshow = true;
        } else {
            document.getElementById("UPTZTable").style.width = "100%"
            $('div#AreaUserPermissionTree').toggle(500);
            permtreeshow = false;
        }
    }
    function SetNoSelectedUserGUI() {
        $("#addPermissionToUser").hide('fast');
        $("#ButtonAdd").hide('fast');
        $("#ButtonDel").hide('fast');
        $.ajax({
            type: "Get",
            url: "/User/GetPermissionGroups",
            dataType: 'json',
            data: { id: $("input#UserId").val() },
            success: function (data) {
                $("select#ListOfGroupsForUser").html(data);
            }
        });
        $.ajax({
            type: "Get",
            url: "/User/GetUserPermissionTree",
            data: { userId: $("input#UserId").val() },
            success: function (html) {
                $("div#AreaUserPermissionTree").html(html);
            }
        });
        $("#userPermissionTimeZoneList").html("");
        return false;
    }
    function SelectUserPermissionGroup() {
        $("#ButtonDel").hide('fast');
        $("#ButtonAdd").hide('fast');
        var Id = $("select#ListOfGroupsForUser").val();
        if (Id != "") {
            $.ajax({
                type: "Get",
                url: "/User/GetUserDefaultZone",
                data: { id: Id },
                beforeSend: function () {
                    $("#button_submit_user_permission_time_zone_search").addClass("Trans");
                },
                success: function (result) {
                    $("div#userPermissionTimeZoneList").html(result);
                    $("input[id^=userPermissionTimeZoneCheck_][type='checkbox']").each(function () {
                        $(this).attr('checked', 'checked');
                    });
                    $("#button_submit_user_permission_time_zone_search").removeClass("Trans");
                    $("div#userPermissionTimeZoneList").fadeIn(300);

                    if (permtreeshow == true) {
                        document.getElementById("UPTZTable").style.width = "70%";
                        document.getElementById("UPTZTable").align = "right";
                    }
                }
            });
            //GetUserPermissionTree($("input#UserId").val(), Id);
            if (permtreeshow == true) {
                $('div#AreaUserPermissionTree').toggle(500);
                permtreeshow = false;
            }
            $("#addPermissionToUser").show('fast');
            //$("#ButtonAdd").show('fast');

            var name = $("span#current_user_permission_name").html();
            var selectedPG = $("select#ListOfGroupsForUser option:selected").text();

            if (name.match(selectedPG)) {

                $("#ButtonDel").show('fast');
            }
            else {

                $("#ButtonAdd").show('fast');
            }
        } else { SetNoSelectedUserGUI(); }
        return false;
    }
    function GetUserPermissionGroup() {
        $.ajax({
            type: "Get",
            url: "/Visitors/GetPermName",
            dataType: 'json',
            data: { id: $("input#UserId").val() },
            success: function (data) {
                $("input#CurrentUserPermissionGroupId").val(data[0]);
                $("span#current_user_permission_name").html(data[1]);
                $("input#CurrentUserParentPermissionGroupId").val(data[2]);
                $("span#joined_user_name").html(data[3]);
                if (data[0] != "0") {
                    $.ajax({
                        type: "Get",
                        url: "/Visitors/GetUserDefaultZone",
                        data: { id: data[0] },
                        beforeSend: function () {
                            $("#button_submit_user_permission_time_zone_search").addClass("Trans");
                        },
                        success: function (result) {
                            $("div#userPermissionTimeZoneList").html(result);
                            $("input[id^=userPermissionTimeZoneCheck_][type='checkbox']").each(function () {
                                $(this).attr('checked', 'checked');
                            });
                            $("#button_submit_user_permission_time_zone_search").removeClass("Trans");
                            $("div#userPermissionTimeZoneList").fadeIn(300);
                            if (permtreeshow == true) {
                                document.getElementById("UPTZTable").style.width = "70%";
                                document.getElementById("UPTZTable").align = "right";
                            }
                        }
                    });
                    /*GetUserPermissionTree($("input#UserId").val(), data[0]);*/


                    $.ajax({
                        type: "Get",
                        url: "/Visitors/GetPermissionGroups",
                        dataType: 'json',
                        data: { id: $("input#UserId").val() },
                        success: function (data) {
                            $("select#ListOfGroupsForUser").html(data);
                        }
                    });
                }
                else {
                    if ($("input#CurrentUserPermissionGroupId").val() == "0") { SetNoSelectedUserGUI(); }
                    else {
                        $.ajax({
                            type: "Get",
                            url: "/Visitors/GetPermissionGroups",
                            dataType: 'json',
                            data: { id: $("input#UserId").val() },
                            success: function (data) {
                                $("select#ListOfGroupsForUser").html(data);
                            }
                        });
                    }
                }
            }
        });
        return false;
    }
    function SubmitUserPermissionTimeZoneSearch() {
        Name = $("input#UserPermissionsTimeZoneSearchByName").val();
        StartTime = $("input#UserPermissionsTimeZoneSearchStartTime").val();
        $.ajax({
            type: "Post",
            url: "/User/SearchPerm",
            data: { name: Name, start: StartTime },
            beforeSend: function () {
                $("#button_submit_user_permission_time_zone_search").addClass("Trans");
            },
            success: function (result) {
                $("div#userPermissionTimeZoneList").html(result);
                /*$("div#userPermissionTimeZoneList").attr("id", function () {
                    $(this).corner("bevelfold");
                });*/
                if (permtreeshow == true) {
                    document.getElementById("UPTZTable").style.width = "70%";
                    document.getElementById("UPTZTable").align = "right";

                }
                $("ul#user_permissions").treeview({
                    animated: "fast",
                    persist: "location",
                    collapsed: false,
                    unique: true
                });

                ///
                $("div#userPermissionTimeZoneList").fadeIn(300);
                $("#button_submit_user_permission_time_zone_search").removeClass("Trans");
                if (permtreeshow == true) {
                    document.getElementById("UPTZTable").style.width = "70%";
                    document.getElementById("UPTZTable").align = "right";
                }
            }
        });
        return false;
    }
    function GetUserZoneByObject(Id, IsRoom) {
        if (IsRoom == 1) {
            ShowDialog('<%=ViewResources.SharedStrings.PermissionsCanNotSetTimeZoneToRoom %>', 2000);
            return false;
        }

        var upgId = $("select#ListOfGroupsForUser").val();
        var cupgId = $("input#CurrentUserPermissionGroupId").val();

        $("li[id^=userPermissionTreeLi_]").each(function () { $(this).css('background-color', 'transparent'); });
        $("li#userPermissionTreeLi_" + Id).css('background-color', '#AAAAAA').corner();
        $("input#userPermissionTreeObject_" + Id).attr('checked', 'checked');
        $("input#CurrentUserBuildingObjectId").val(Id);

        if ((cupgId == "0") && (upgId == "")) {
            ShowDialog("Please select time zone for user...", 2000);
            return false;
        }
        if ((cupgId == "0") && (upgId != "")) {
            $.ajax({
                type: "Get",
                url: "/User/GetUserActiveZone",
                data: { id: upgId, objectId: Id },
                beforeSend: function () {
                    $("#button_submit_user_permission_time_zone_search").addClass("Trans");
                },
                success: function (result) {
                    $("div#userPermissionTimeZoneList").html(result);
                    $("div#userPermissionTimeZoneList").fadeIn(300);
                    $("#button_submit_user_permission_time_zone_search").removeClass("Trans");
                    $("input[id^=userPermissionTimeZoneCheck_][type='checkbox']").each(function () {
                        $(this).attr('checked', 'checked');
                    });
                    if (permtreeshow == true) {
                        document.getElementById("UPTZTable").style.width = "70%";
                        document.getElementById("UPTZTable").align = "right";
                    }
                }
            });
        }
        else if (cupgId != "0") {
            $.ajax({
                type: "Get",
                url: "/User/GetUserActiveZone",
                data: { id: cupgId, objectId: Id },
                beforeSend: function () {
                    $("#button_submit_user_permission_time_zone_search").addClass("Trans");
                },
                success: function (result) {
                    $("div#userPermissionTimeZoneList").html(result);
                    $("div#userPermissionTimeZoneList").fadeIn(300);
                    $("#button_submit_user_permission_time_zone_search").removeClass("Trans");
                    $("input[id^=userPermissionTimeZoneCheck_][type='checkbox']").each(function () {
                        $(this).attr('checked', 'checked');
                    });
                    if (permtreeshow == true) {
                        document.getElementById("UPTZTable").style.width = "70%";
                        document.getElementById("UPTZTable").align = "right";
                    }
                }
            });
        }
        //SubmitUserPermissionTimeZoneSearch();
        $("input[id^=userPermissionTimeZoneCheck_][type='checkbox']").each(function () {
            $(this).attr('checked', 'checked');
        });
        return false;
    }

    function RenewPG() {
        var cuserId = $("input#UserId").val();
        var cupgId = $("input#CurrentUserPermissionGroupId").val();
        var parentPg = $("input#CurrentUserParentPermissionGroupId").val();
        if (cupgId != "0") {
            ShowDialog('<%=ViewResources.SharedStrings.UsersMessageSaveUserPermGroup %>', 2000, true);
            $.ajax({
                type: "Get",
                url: "/User/ChangeUserPermissionGroup",
                dataType: 'json',
                data: { userId: cuserId, newUpgId: parentPg },
                success: function (data) {
                    $("input#CurrentUserPermissionGroupId").val(data);
                    //UpdateUserBuildingObjects(data);
                    if (permtreeshow == true) {
                        GetUserPermissionTree($("input#UserId").val(), $("input#CurrentUserPermissionGroupId").val());
                    }
                    GetUserPermissionGroup();
                }

            });

        }
        else {
            ShowDialog('<%=ViewResources.SharedStrings.Error %>', 2000, true);
        }
    }
    function DelUserPermission() {
        var cuserId = $("input#UserId").val();
        var upgId = $("select#ListOfGroupsForUser").val();
        var cupgId = $("input#CurrentUserPermissionGroupId").val();
        var Grp_name = $('#current_user_permission_name').html();

        $.ajax({
            type: "Get",
            url: "/User/CheckBOverlapping",
            dataType: 'json',
            data: { PermissionGroupID: cupgId, Permission_Name: Grp_name },
            success: function (data) {
                if (data == false) {
                    if ((cupgId != "0") && (upgId != "")) {

                        ShowDialog('<%=ViewResources.SharedStrings.UsersMessageSaveUserPermGroup %>', 2000, true);
                        $.ajax({
                            type: "Get",
                            url: "/User/DelUserPermissionGroup",
                            dataType: 'json',
                            data: { userId: cuserId, newUpgId: upgId },
                            success: function (data) {
                                GetUserPermissionGroup();
                                //UpdateUserBuildingObjects(data);
                                if (data == null) { ShowDialog("Can't delete Parent Group", 2000, false); }
                                if (permtreeshow == true) {
                                    GetUserPermissionTree($("input#UserId").val(), cupgId);
                                }
                            }

                        });
                    }
                    else { alert('There is no Permission Group selected.') }

                }
                else {
                    RenewPG();
                }

            }

        });



<%--        if ((cupgId != "0") && (upgId != "")) {

            ShowDialog('<%=ViewResources.SharedStrings.UsersMessageSaveUserPermGroup %>', 2000, true);
            $.ajax({
                type: "Get",
                url: "/User/DelUserPermissionGroup",
                dataType: 'json',
                data: { userId: cuserId, newUpgId: upgId },
                success: function (data) {
                    GetUserPermissionGroup();
                    //UpdateUserBuildingObjects(data);
                    if (data == null) { ShowDialog("Can't delete Parent Group", 2000, false); }
                    if (permtreeshow == true) {
                        GetUserPermissionTree($("input#UserId").val(), cupgId);
                    }
                }

            });
        } else { Alert('There is no Permission Group selected.') }--%>
    }
    function AddUserPermission() {
        var cuserId = $("input#UserId").val();
        var upgId = $("select#ListOfGroupsForUser").val();
        var cupgId = $("input#CurrentUserPermissionGroupId").val();
        if ((cupgId != "0") && (upgId != "")) {

            ShowDialog('<%=ViewResources.SharedStrings.UsersMessageSaveUserPermGroup %>', 2000, true);
            $.ajax({
                type: "Get",
                url: "/User/AddUserPermissionGroup",
                dataType: 'json',
                data: { userId: cuserId, newUpgId: upgId },
                success: function (data) {
                    GetUserPermissionGroup();
                    //UpdateUserBuildingObjects(data);
                    if (permtreeshow == true) {
                        GetUserPermissionTree($("input#UserId").val(), cupgId);
                    }
                }

            });
        } else { alert('There is no Permission Group selected.') }
    }
    function SaveUserPermission() {

        var cuserId = $("input#UserId").val();
        var upgId = $("select#ListOfGroupsForUser").val();
        var cupgId = $("input#CurrentUserPermissionGroupId").val();

        if ((cupgId == "0") && (upgId != "")) {

            ShowDialog('<%=ViewResources.SharedStrings.UsersMessageAddUserPermGroup %>', 2000, true);
            $.ajax({
                type: "Get",
                url: "/User/CreateUserPermissionGroup",
                dataType: 'json',
                data: { userId: cuserId, pgId: upgId },
                success: function (data) {
                    $("input#CurrentUserPermissionGroupId").val(data);
                    UpdateUserBuildingObjects(data);
                    GetUserPermissionGroup();
                    PermTree();
                }
            });
        }
        else if ((cupgId != "0") && (upgId != "")) {
            ShowDialog('<%=ViewResources.SharedStrings.UsersMessageSaveUserPermGroup %>', 2000, true);
            $.ajax({
                type: "Get",
                url: "/User/ChangeUserPermissionGroup",
                dataType: 'json',
                data: { userId: cuserId, newUpgId: upgId },
                success: function (data) {
                    $("input#CurrentUserPermissionGroupId").val(data);
                    UpdateUserBuildingObjects(data);
                    GetUserPermissionGroup();
                    if (permtreeshow == true) {
                        GetUserPermissionTree($("input#UserId").val(), upgId);
                    }
                }

            });
        }
        else {
            if (cupgId != "0") {

                ShowDialog('<%=ViewResources.SharedStrings.UsersMessageSaveUserPermGroup %>', 2000, true);
                UpdateUserBuildingObjects(cupgId);
                GetUserPermissionGroup();
                GetUserPermissionTree($("input#UserId").val(), $("input#CurrentUserPermissionGroupId").val());
            }
        }
        return false;
    }
    function UpdateUserBuildingObjects(cupgId) {
        var bobjs = new Array();
        $("input[id^=userPermissionTreeObject_][type='checkbox']").each(function () {
            bobjs.push($(this).attr('name'));
        });
        var selbobjs = new Array();
        $("input[id^=userPermissionTreeObject_][type='checkbox']").each(function () {
            if ($(this).is(':checked') == true) {
                selbobjs.push($(this).attr('name'));
            }
        });
        var tzones = new Array();
        $("input[id^=userPermissionTimeZoneCheck_][type='checkbox']").each(function () {
            if ($(this).is(':checked') == true) {
                tzones.push($(this).attr('name'));
            }
        });
        var cboId = $("input#CurrentUserBuildingObjectId").val();
        $("input#CurrentUserBuildingObjectId").val("");
        if (cboId != "") {
            if (tzones.length == 1) {
                $.ajax({
                    type: "Post",
                    url: "/User/SetBuildingObjectTimeZone",
                    dataType: 'json',
                    traditional: true,
                    data: { id: cupgId, objectId: cboId, zoneId: tzones[0] },
                    success: function (html) { GetUserPermissionTree($("input#UserId").val(), cupgId); }
                });
            }
            else
                if (tzones.length == 0) {
                    $.ajax({
                        type: "Post",
                        url: "/User/ResetBuildingObjectTimeZone",
                        dataType: 'json',
                        traditional: true,
                        data: { id: cupgId, objectId: cboId },
                        success: function (html) { GetUserPermissionTree($("input#UserId").val(), cupgId); }
                    });
                }
        }
        if (permtreeshow == true) {

            $.ajax({
                type: "Post",
                url: "/Permission/UserSaveGroup",
                dataType: 'json',
                traditional: true,
                data: { id: cupgId, objectIds: bobjs, selectedObjectIds: selbobjs },
                success: function (html) { }
            });
        }

        return false;
    }
    function ToggleUserArming(id) {
        var pId = $("input#CurrentUserPermissionGroupId").val();
        if (pId != "0") {
            $.post('/User/ToggleArming', { boId: id, pgId: pId }, function (html) { });
        }
        else { ShowDialog("Please select Permission Group!", 2000); }
    }
    function ToggleUserDefaultArming(id) {
        var pId = $("input#CurrentUserPermissionGroupId").val();
        if (pId != "0") {
            $.post('/User/ToggleDefaultArming', { boId: id, pgId: pId }, function (html) { });
        }
        else { ShowDialog("Please select Permission Group!", 2000); }
    }
    function ToggleUserDisarming(id) {
        var pId = $("input#CurrentUserPermissionGroupId").val();
        if (pId != "0") {
            $.post('/User/ToggleDisarming', { boId: id, pgId: pId }, function (html) { });
        }
        else { ShowDialog("Please select Permission Group!", 2000); }
    }
    function ToggleUserDefaultDisarming(id) {
        var pId = $("input#CurrentUserPermissionGroupId").val();
        if (pId != "0") {
            $.post('/User/ToggleDefaultDisarming', { boId: id, pgId: pId }, function (html) { });
        }
        else { ShowDialog("Please select Permission Group!", 2000); }
    }
    // !!
    function GetUserPermissionTree(userID, Id) {
        $("#addPermissionToUser").hide('fast');
     
        $.ajax({
            type: "Get",
            url: "/Visitors/GetUserPermissionTree",
            data: { userId: userID, id: Id },
            beforeSend: function () {
                $("div#AreaUserPermissionTree").fadeOut(300);
            },
            success: function (html) {
                $("div#AreaUserPermissionTree").html(html);
                $("div#AreaUserPermissionTree").attr("id", function () {
                    $(this).corner("bevelfold");
                });
                $("ul#user_permissions").treeview({
                    animated: "fast",
                    persist: "location",
                    //collapsed: true,
                    unique: true
                });
                $("div#AreaUserPermissionTree").fadeIn('slow');
                $("#addPermissionToUser").show('fast');
                var name = $("span#current_user_permission_name").html();
                var selectedPG = $("select#ListOfGroupsForUser option:selected").text();

                if (name.match(selectedPG)) {
                    $("#ButtonDel").show('fast');
                }
                else {
                    $("#ButtonAdd").show('fast');
                }

            }
        });

    }

</script>
