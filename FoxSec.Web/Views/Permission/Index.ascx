<%--<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.RoleEditViewModel>"%>--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.HomeViewModel>" %>
<%@ Import Namespace="FoxSec.DomainModel.DomainObjects" %>
<style type="text/css">
    #loader {
        position: fixed;
        left: 0px;
        top: 0px;
        width: 100%;
        height: 100%;
        z-index: 99;
        opacity: 0.8;
        filter: alpha(opacity=80);
        -moz-opacity: 0.8;
        background: url('../../img/hourglass.gif') 50% 50% no-repeat rgb(0, 0, 0);
        /* background: url('../../img/loader1.gif') 50% 50% no-repeat rgb(249,249,249);  */
    }
</style>

<div id="loader" style="display: none">
</div>

<table style="margin: 0; width: 100%; padding: 0; border-spacing: 1px;">
    <tr>
        <td style='width: 290px; vertical-align: top; text-align: center'>
            <div id='currentPermissionGroupName' style="min-width: 260px">

                <h2><span class="tipsy_we" title='<%=ViewResources.SharedStrings.PermissionGetDefaultTimeZone%>'>
                    <a onclick="javascript:GetPermissionGroupDefaultTimeZone()"></a>
                </span>
                </h2>
            </div>
            <div id='AreaPermissionTree' style='margin: 15px 15px 15px 0; padding: 10px; min-width: 260px; text-align: left'>
            </div>
        </td>
        <td style='vertical-align: top;'>

            <table style="margin: 0; width: 100%; padding: 0; border-spacing: 10px; font-size: 8px">
                <tr>
                    <td style="text-align: right">
                        <select onchange="javascript:SelectPermissionGroup();" id="ListOfAllGroups" style="width: 210px">
                        </select>&nbsp;&nbsp;&nbsp;
    <input type='button' id='button_add_permission_group' value='<%=ViewResources.SharedStrings.BtnCreateNew %>' onclick="javascript: AddPermissionGroup();" />
                        <input type='button' id='ShowUsers' value='Users' style="display: none" onclick="javascript: ShowUsersInGroup();" />

                        <input type='button' id='button_delete_permission_group' value='<%=ViewResources.SharedStrings.BtnDeleteGroup %>' style="display: none" onclick="javascript: DeletePermissionGroup();" />
                        <input type='button' id='button_delete_permission_group_users' value='<%=ViewResources.SharedStrings.BtnDeletePermissionGroupFromUsers %>' style="display: none" onclick="javascript: DelUsersFromPermissionGroup();" />

                        <%= Html.Hidden("CurrentPermissionGroupId") %>
                        <%= Html.Hidden("CurrentBuildingObjectId") %>
                        <%= Html.Hidden("DefaultTimeZoneId") %>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table style="margin: 0; width: 100%; padding: 0; border-spacing: 1px; font-size: 8px">
                            <tr>
                                <td>
                                    <input type='button' id='button_save_permission_group' value='<%=ViewResources.SharedStrings.BtnSaveChanges %>' style="display: none" onclick="javascript: SavePermissionGroup();" />
                                    <input type='button' id='button_change_object_time_zone' value='TimeZone to ' style="display: none" onclick="javascript: NewDefaultTimeZone();" />

                                </td>
                                <td style="width: 325px">
                                    <label for='PermissionsTimeZoneSearchByName'><%:ViewResources.SharedStrings.CommonTimeZoneName %>:</label><%= Html.TextBox("PermissionsTimeZoneSearchByName", "", new { @style = "width:180px;text-transform: capitalize;" })%></td>
                                <td style="width: 165px">
                                    <label for='PermissionsTimeZoneSearchStartTime'><%:ViewResources.SharedStrings.CommonStartTime %>:</label><%= Html.TextBox("PermissionsTimeZoneSearchStartTime", "", new { @style = "width:74px" })%> </td>
                                <td style="width: 35px; vertical-align: top; padding-top: 2px"><span id='button_submit_permission_time_zone_search' class='icon icon_find tipsy_we' title='<%=ViewResources.SharedStrings.TimeZonesSearch %>' onclick="javascript:SubmitPermissionTimeZoneSearch();"></span></td>
                            </tr>
                        </table>

                    </td>
                </tr>
            </table>

            <br />
            <div id='AreaLogSearchResultsWait' style='display: none; width: 100%; height: 620px; text-align: center'><span style='position: relative; top: 40%' class='icon loader'></span></div>
            <div id="PermissionTimeZoneList" style="display: none"></div>
            <br />
        </td>
    </tr>
</table>

&nbsp;
    <script type="text/javascript" src="../../css/select2.min.js"></script>
<script type="text/javascript" lang="javascript">
    var ButtonName = document.getElementById('button_change_object_time_zone');
    $(document).ready(function () {
        
        var i = $('#panel_owner li').index($('#permissionsTab'));

        var opened_tab = '<%: Session["tabIndex"] %>';

        if (opened_tab != '') {
            i1 = new Number(opened_tab);
            if (i1 != i) {
                SetOpenedTab(i);
            }
        }
        else {
            SetOpenedTab(i);
        }
        $("input:button").button();
        SetNoSelectedGUI();
        GetPermissionGroups();
        $('div#Work').fadeIn("slow");
        $("#ListOfAllGroups").select2();
    });

    function NotCompanyUser() {
        ShowDialog('<%=ViewResources.SharedStrings.NotCompanyUser %>', 4000);
        return;
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
                        id: UserId
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
        });
        return false;
    }


    function SubmitPermissionTimeZoneSearch() {
        Name = $("input#PermissionsTimeZoneSearchByName").val();
        StartTime = $("input#PermissionsTimeZoneSearchStartTime").val();
        if (StartTime.length == 1) { StartTime = "0" + StartTime + ":00"; $("input#PermissionsTimeZoneSearchStartTime").val(StartTime); }
        else
            if (StartTime.length == 2) { StartTime = StartTime + ":00"; $("input#PermissionsTimeZoneSearchStartTime").val(StartTime); }
        $.ajax({
            type: "Post",
            url: "/Permission/Search",
            data: { name: Name, start: StartTime },
            beforeSend: function () {
                $("#button_submit_permission_time_zone_search").addClass("Trans");
            },
            success: function (result) {
                $("div#PermissionTimeZoneList").html(result);
                $("div#PermissionTimeZoneList").fadeIn(300);
                $("#button_submit_permission_time_zone_search").removeClass("Trans");
            }
        });
        return false;
    }

    function AddPermissionGroup() {
        $("input#button_change_object_time_zone").fadeOut();
        $("div#modal-dialog").dialog({
            open: function () {
                $("div#modal-dialog").html("");
                $.get('/Permission/Create', {}, function (html) {
                    $("div#modal-dialog").html(html);
                });
            },
            resizable: false,
            width: 550,
            height: 430,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.PermissionGropupsAddNew %>',
            buttons: {
                '<%=ViewResources.SharedStrings.BtnSave %>': function () {
                    var nname = $.trim($("input#create_Permission_Name").val());
                    if (nname.length == 0) {
                        $("input#create_Permission_Name").val("");
                        $("input#create_Permission_Name").focus();
                        ShowDialog('<%=ViewResources.SharedStrings.PermissionsNoNameError %>', 2000);
                        return;
                    }
                    $.ajax({
                        type: "Post",
                        url: "/Permission/CheckName",
                        dataType: 'json',
                        data: { name: nname },
                        success: function (data) {
                            if (data != "ok") {
                                ShowDialog('<%=ViewResources.SharedStrings.PermissionsNameExistsError %>', 2000);
                                return;
                            }
                            else {
                                InitPermissionGroup();
                                $("div#modal-dialog").dialog("close");
                            }
                        }
                    });
                },
                '<%=ViewResources.SharedStrings.BtnCancel %>': function () {
                    $(this).dialog("close");
                }
            }

        });
        
        return false;
    }

    function InitPermissionGroup() {
        $("input#CurrentPermissionGroupId").val("");
        var Id = $("select#copy_from_GroupId").val();
        var newName = $("input#create_Permission_Name").val();
        if (Id == "") {
            ShowDialog('<%=ViewResources.SharedStrings.PermissionsNoBuildingObjectsTimeZonesSelected %>', 3000);
            $("input#PermissionsTimeZoneSearchByName").val("");
            $("input#PermissionsTimeZoneSearchStartTime").val("");
            SubmitPermissionTimeZoneSearch();
            $("div#currentPermissionGroupName").find("a").html(newName);
            $("input#button_add_permission_group").fadeOut(300, function () { $("input#button_save_permission_group").fadeIn(); });
            $.get('/Permission/GetTree', function (html) {
                $("div#AreaPermissionTree").html(html);
                $("div#AreaPermissionTree").attr("id", function () {
                    $(this).corner("bevelfold");
                });
                $("ul#permissions").treeview({
                    animated: "fast",
                    persist: "location",
                    collapsed: false,
                    unique: true
                });
            });
        }
        else {
            var bobjs = new Array();
            $("input[id^=permissionTreeObject_][type='checkbox']").each(function () {
                bobjs.push($(this).attr('name'));
            });
            ShowDialog('<%=ViewResources.SharedStrings.PermissionsNewPermissionGroupCreatingMessage %>', 2000, true);
            $.ajax({
                type: "Post",
                url: "/Permission/CreateGroup",
                dataType: 'json',
                traditional: true,
                data: { name: newName, copyId: Id, defTzId: "", objectIds: bobjs },
                success: function (data) {
                    
                    $("input#button_save_permission_group").fadeIn();
                    $("input#CurrentPermissionGroupId").val(data);
                    $("div#currentPermissionGroupName").find("a").html(newName);
                    GetPermissionGroups();
                    SetNoSelectedGUI();
                }
            });
        }
    }

    function GetPermissionGroupDefaultTimeZone() {
        var Id = $("select#ListOfAllGroups").val();
        if (Id != "") {
            EditPermission(Id);
            $.ajax({
                type: "Get",
                url: "/Permission/GetDefaultZone",
                data: { id: Id },
                beforeSend: function () {
                    $("#button_submit_permission_time_zone_search").addClass("Trans");
                },
                success: function (result) {
                    $("div#PermissionTimeZoneList").html(result);
                    $("div#PermissionTimeZoneList").fadeIn(300);
                    $("#button_submit_permission_time_zone_search").removeClass("Trans");
                    $("input[id^=permissionTimeZoneCheck_][type='checkbox']").each(function () {
                        $(this).attr('checked', 'checked');
                    });
                }
            });
        }
    }
    function EditPermission(Id) {
        $.ajax({
            type: "Post",
            url: "/Permission/CanRename",
            dataType: 'json',
            traditional: true,
            data: { id: Id },
            success: function (data) {
                if (data == "yes") {
                    $("div#modal-dialog").dialog({
                        open: function () {
                            $("div#modal-dialog").html("");
                            $.get('/Permission/Edit', { id: Id }, function (html) {
                                $("div#modal-dialog").html(html);
                            });
                        },
                        resizable: false,
                        width: 650,
                        height: 200,
                        modal: true,
                        title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.PermissionsChangeNameTitle %>',
                        buttons: {
                            '<%=ViewResources.SharedStrings.BtnSave %>': function () {
                                var nam = $("input#edit_Permission_Name").val();
                                $.ajax({
                                    type: "Post",
                                    url: "/Permission/CheckName",
                                    dataType: 'json',
                                    data: { name: nam, id: Id },
                                    success: function (data) {
                                        if (data != "ok") {
                                            ShowDialog('<%=ViewResources.SharedStrings.PermissionsNameExistsError %>', 2000);
                                        }
                                        else {
                                            {
                                                var dt = $("#editPermission").serialize();
                                                var usrid = dt.split('&');
                                                var uid = usrid[1].split('=')[1];
                                                if (uid == "") {
                                                    alert("Please select owner name!!");
                                                    return false;
                                                }
                                                $.ajax({
                                                    type: "Post",
                                                    url: "/Permission/Edit",
                                                    dataType: 'json',
                                                    traditional: true,
                                                    data: $("#editPermission").serialize(),
                                                    success: function (data) {
                                                        if (data.IsSucceed == false) {
                                                            $("div#modal-dialog").html(data.viewData);
                                                            if (data.DisplayMessage == true) {
                                                                ShowDialog(data.Msg, 2000);
                                                            }
                                                        }
                                                        else {
                                                            ShowDialog(data.Msg, 2000, true);
                                                            $("div#modal-dialog").html("");
                                                            $("div#modal-dialog").dialog("close");
                                                            //dlg.dialog("close");
                                                            setTimeout(function () { UpdateName(Id); GetPermissionGroups(); $("select#ListOfAllGroups").val(Id); }, 1000);


                                                        }
                                                    }
                                                });
                                            }
                                        }
                                    }
                                });
                            },
                            '<%=ViewResources.SharedStrings.BtnCancel %>': function () {
                                $(this).dialog("close");
                            }
                        }
                    });
                }
            }
        });
        return false;
    }

    function SelectPermissionGroup() {
        var Id = $("select#ListOfAllGroups").val();
        $("input#button_change_object_time_zone").fadeOut();
        if (Id != "") {
            //$("input#ShowUsers").fadeIn();
            GetPermissionTree(Id);
            UpdateName(Id);
            $.ajax({
                type: "Get",
                url: "/Permission/GetDefaultZone",
                data: { id: Id },
                beforeSend: function () {
                    $("#button_submit_permission_time_zone_search").addClass("Trans");
                },
                success: function (result) {
                    $("div#PermissionTimeZoneList").html(result);
                    $("div#PermissionTimeZoneList").fadeIn(300);
                    $("#button_submit_permission_time_zone_search").removeClass("Trans");
                    $("input[id^=permissionTimeZoneCheck_][type='checkbox']").each(function () {
                        $(this).attr('checked', 'checked');
                        $("input#DefaultTimeZoneId").val($(this).attr('name'));
                    });
                }
            });
            $.ajax({
                type: "Get",
                url: "/Permission/CheckUsage",
                dataType: 'json',
                data: { id: Id },
                success: function (data) {
                    if (data != "0") {
                        ShowDialog(data + '<%=ViewResources.SharedStrings.PermissionsPermGroupInUse %>', 3000);
                        $("input#ShowUsers").fadeIn();
                        $("input#button_delete_permission_group").fadeOut();
                        //$("input#button_delete_permission_group_users").fadeIn();

                        //enable deleteUsers functionality for specific Role
                         <% if (Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewDeletePermGroupMenu))
    { %>
                        $("input#button_delete_permission_group_users").fadeIn();
                    <%}%>
                    }
                    else {
                        $("input#button_delete_permission_group").fadeIn();
                        $("input#ShowUsers").fadeIn();
                        $("input#button_delete_permission_group_users").fadeOut();
                    }
                }
            });

            /*   $.ajax({
               type: "Get",
               url: "/Permission/EnableDeleteUserFromPermGroup",
               dataType: 'json',
               data: { id: Id },
                   success: function (data){
                       if (data != null) {
                           alert("enable dlt:" + data);
                       }
                       else {
                           alert("no data reterived");
                       }
               }
               });
          */

        } else {
            $("input#ShowUsers").fadeOut();
            $("input#CurrentPermissionGroupId").val("");
            $("input#button_delete_permission_group_users").fadeOut();
            SetNoSelectedGUI();
        }
        
        return false;
    }

    function UpdateName(Id) {
        $.ajax({
            type: "Get",
            url: "/Permission/GetName",
            dataType: 'json',
            data: { id: Id },
            success: function (data) {
                $("input#CurrentPermissionGroupId").val(Id);
                SetSaveButon(Id);
                $("div#currentPermissionGroupName").find("a").html(data);
            }
        });
    }

    function SetSaveButon(Id) {
        $.ajax({
            type: "Get",
            url: "/Permission/IsGroupEditable",
            dataType: 'json',
            data: { id: Id },
            success: function (data) {
                if (data == false) {
                    $('#button_save_permission_group').fadeOut();
                }
                else {
                    $('#button_save_permission_group').fadeIn();
                }
            }
        });
    }

    function GetPermissionGroups() {
        $.ajax({
            type: "Get",
            url: "/Permission/GetPermissionGroups",
            dataType: 'json',
            async: false,
            success: function (data) {
                $("select#ListOfAllGroups").html(data);
            }
        });
        return false;
    }

    function SavePermissionGroup() {
        var x = 0;
        $("input#button_change_object_time_zone").fadeOut();
        if ($("input#CurrentPermissionGroupId").val() == "") {  //permission group ne vqbrana
            var bobjs = new Array();
            $("input[id^=permissionTreeObject_][type='checkbox']").each(function () {
                bobjs.push($(this).attr('name'));
            });
            var selbobjs = new Array();
            $("input[id^=permissionTreeObject_][type='checkbox']").each(function () {
                if ($(this).is(':checked') == true) {
                    selbobjs.push($(this).attr('name'));
                }
            });
            var tzones = new Array();
            $("input[id^=permissionTimeZoneCheck_][type='checkbox']").each(function () {
                if ($(this).is(':checked') == true) {
                    tzones.push($(this).attr('name'));
                }
            });
            if (selbobjs.length == 0) {
                ShowDialog('<%=ViewResources.SharedStrings.PermissionsNoBuildingObjectsSelected %>', 3000);
                return false;
            }
            if (tzones.length == 0) {
                ShowDialog('<%=ViewResources.SharedStrings.PermissionsNoTimeZoneSelected %>', 3000);
                return false;
            }
            ShowDialog('<%=ViewResources.SharedStrings.PermissionsNewPermissionGroupCreatingMessage %>', 2000, true);
            $.ajax({
                type: "Get",
                url: "/Permission/CreateGroup",
                dataType: 'json',
                traditional: true,
                data: { name: $("div#currentPermissionGroupName").find("a").html(), copyId: "", defTzId: tzones[0], objectIds: bobjs, selectedObjectIds: selbobjs },
                success: function (result) {
                    $("input#button_add_permission_group").fadeIn();
                    $("input#CurrentPermissionGroupId").val(result);
                    GetPermissionGroups();
                    UpdateName(result);//newly added
                    $("input#button_change_object_time_zone").fadeIn();

                }
            });
        }
        else { //permission group vqbrana
            var bobjs = new Array();
            var Id = $("input#CurrentPermissionGroupId").val();  // new default timezone
            ShowDialog('<%=ViewResources.SharedStrings.PermissionsChangingDefaultTimeZone %>', 2000);
            var tzones = new Array();
            $("input[id^=permissionTimeZoneCheck_][type='checkbox']").each(function () {
                if ($(this).is(':checked') == true) {
                    tzones.push($(this).attr('name'));
                }
            });


            var Id = $("input#CurrentPermissionGroupId").val();
            var bobjs = new Array();

            $("input[id^=permissionTreeObject_][type='checkbox']").each(function () {
                bobjs.push($(this).attr('name'));
            });
            var selbobjs = new Array();
            $("input[id^=permissionTreeObject_][type='checkbox']").each(function () {
                if ($(this).is(':checked') == true) {
                    selbobjs.push($(this).attr('name'));
                }
            });
            if (tzones.length == 1) {


                // new default timezone
                var Id = $("input#CurrentPermissionGroupId").val();
                ShowDialog('<%=ViewResources.SharedStrings.PermissionsChangingDefaultTimeZone %>', 2000);
                var tzones = new Array();
                $("input[id^=permissionTimeZoneCheck_][type='checkbox']").each(function () {
                    if ($(this).is(':checked') == true) {
                        tzones.push($(this).attr('name'));
                    }
                });



                var dtz = $("input#DefaultTimeZoneId").val();

                if (tzones != dtz) {
                    x = 1;
                    $("div#modal-dialog").dialog({
                        open: function () {
                            $("div#modal-dialog").html("");
                            $.get('/Permission/Message', {}, function (html) {
                                $("div#modal-dialog").html(html);
                            });
                        },
                        resizable: false,
                        width: 440,
                        height: 190,
                        modal: true,
                        title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.TimeZonesAddNewTimeZone %>',
                        buttons: {
                            '<%=ViewResources.SharedStrings.BtnSave %>': function () {
                                dlg = $(this);
                                $.ajax({
                                    type: "Post",
                                    url: "/Permission/ChangeDefaultTimeZone",
                                    dataType: 'json',
                                    traditional: true,
                                    data: { id: Id, zoneId: tzones[0] },
                                    success: function (data) {
                                        $("input#DefaultTimeZoneId").val(tzones[0])
                                        GetPermissionTree(Id);
                                        UpdateName(Id);

                                    }
                                });
                                $(this).dialog("close");

                                $.ajax({
                                    type: "Post",
                                    url: "/Permission/SaveGroup",
                                    dataType: 'json',
                                    traditional: true,
                                    data: { id: Id, objectIds: bobjs, selectedObjectIds: selbobjs },
                                    success: function (html) {
                                        UpdateName(Id);
                                        GetPermissionTree(Id);
                                    }
                                });
                            },

                        '<%=ViewResources.SharedStrings.BtnCancel %>': function () {

                                $(this).dialog("close");
                            }
                        }
                    });


                }
            }
            else
                if (tzones.length == 0) {
                    ShowDialog('<%=ViewResources.SharedStrings.PermissionsPermissionGroupNoSelected %>', 2000);
                }
            if (x == 0) {
                $.ajax({
                    type: "Post",
                    url: "/Permission/SaveGroup",
                    dataType: 'json',
                    traditional: true,
                    data: { id: Id, objectIds: bobjs, selectedObjectIds: selbobjs },
                    success: function (html) {
                        UpdateName(Id);
                        GetPermissionTree(Id);
                    }
                });
            }



        }
        ShowDialog('<%=ViewResources.SharedStrings.CommonSaving %>' + "...", 2000, true);

        return false;
    }

    function SavePermissionGroup1() {

        $("input#button_change_object_time_zone").fadeOut();

        if ($("input#CurrentPermissionGroupId").val() == "") {  //permission group ne vqbrana
            var bobjs = new Array();
            $("input[id^=permissionTreeObject_][type='checkbox']").each(function () {
                bobjs.push($(this).attr('name'));
            });
            var selbobjs = new Array();
            $("input[id^=permissionTreeObject_][type='checkbox']").each(function () {
                if ($(this).attr('checked')) {
                    selbobjs.push($(this).attr('name'));
                }
            });
            var tzones = new Array();
            $("input[id^=permissionTimeZoneCheck_][type='checkbox']").each(function () {
                if ($(this).attr('checked')) {
                    tzones.push($(this).attr('name'));
                }
            });
            if (selbobjs.length == 0) {
                ShowDialog('<%=ViewResources.SharedStrings.PermissionsNoBuildingObjectsSelected %>', 3000);
                return false;
            }
            if (tzones.length == 0) {
                ShowDialog('<%=ViewResources.SharedStrings.PermissionsNoTimeZoneSelected %>', 3000);
                return false;
            }
            ShowDialog('<%=ViewResources.SharedStrings.PermissionsNewPermissionGroupCreatingMessage %>', 2000, true);
            $.ajax({
                type: "Get",
                url: "/Permission/CreateGroup",
                dataType: 'json',
                traditional: true,
                data: { name: $("div#currentPermissionGroupName").find("a").html(), copyId: "", defTzId: tzones[0], objectIds: bobjs, selectedObjectIds: selbobjs },
                success: function (result) {
                    $("input#button_add_permission_group").fadeIn();
                    $("input#CurrentPermissionGroupId").val(result);
                    GetPermissionGroups();
                    //					$("select#ListOfAllGroups option").each(function () {
                    //            			if ($(this).attr('value') == result) {
                    //            				$(this).attr('selected', 'selected');
                    //            			}
                    //            		});

                    $("input#button_change_object_time_zone").fadeIn();

                }
            });
        }
        else { //permission group vqbrana
            var bobjs = new Array();
            var Id = $("input#CurrentPermissionGroupId").val();  // new default timezone
            ShowDialog('<%=ViewResources.SharedStrings.PermissionsChangingDefaultTimeZone %>', 2000);
            var tzones = new Array();
            $("input[id^=permissionTimeZoneCheck_][type='checkbox']").each(function () {
                if ($(this).attr('checked')) {
                    tzones.push($(this).attr('name'));
                }
            });

            /*
    	    $.post('/Permission/ChangeDefaultTimeZone', { id: Id, zoneId: tzones[0] },
            function (html) {
                //UpdateName(Id);
                //GetPermissionTree(Id);
            });
            ////////-----------------
            */
            var Id = $("input#CurrentPermissionGroupId").val();
            var bobjs = new Array();

            $("input[id^=permissionTreeObject_][type='checkbox']").each(function () {
                bobjs.push($(this).attr('name'));
            });
            var selbobjs = new Array();
            $("input[id^=permissionTreeObject_][type='checkbox']").each(function () {
                if ($(this).attr('checked')) {
                    selbobjs.push($(this).attr('name'));
                }
            });/*
            var tzones = new Array();
            $("input[id^=permissionTimeZoneCheck_][type='checkbox']").each(function () {
                if ($(this).attr('checked')) {
                    tzones.push($(this).attr('name'));
                }
            });
            var cboId = $("input#CurrentBuildingObjectId").val();
            $("input#CurrentBuildingObjectId").val("");
            if (cboId != "") {*/
            if (tzones.length == 1) {


                // new default timezone
                var Id = $("input#CurrentPermissionGroupId").val();
                ShowDialog('<%=ViewResources.SharedStrings.PermissionsChangingDefaultTimeZone %>', 2000);
                var tzones = new Array();
                $("input[id^=permissionTimeZoneCheck_][type='checkbox']").each(function () {
                    if ($(this).attr('checked')) {
                        tzones.push($(this).attr('name'));
                    }
                });/*
                    $.post('/Permission/ChangeDefaultTimeZone', { id: Id, zoneId: tzones[0] },
                    function (html) {
                       // UpdateName(Id);
                        //GetPermissionTree(Id);
                    });
                    */



                var dtz = $("input#DefaultTimeZoneId").val();

                if (tzones != dtz) {
                    $("div#modal-dialog").dialog({
                        open: function () {
                            $("div#modal-dialog").html("");
                            $.get('/Permission/Message', {}, function (html) {
                                $("div#modal-dialog").html(html);
                            });
                        },
                        resizable: false,
                        width: 440,
                        height: 190,
                        modal: true,
                        title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.TimeZonesAddNewTimeZone %>',
                        buttons: {
                            '<%=ViewResources.SharedStrings.BtnSave %>': function () {
                                dlg = $(this);
                                $.ajax({
                                    type: "Post",
                                    url: "/Permission/ChangeDefaultTimeZone",
                                    dataType: 'json',
                                    traditional: true,
                                    data: { id: Id, zoneId: tzones[0] },
                                    success: function (data) {
                                        $("input#DefaultTimeZoneId").val(tzones[0])
                                        GetPermissionTree(Id);
                                        UpdateName(Id);
                                    }
                                });
                                $(this).dialog("close");
                            },
                        '<%=ViewResources.SharedStrings.BtnCancel %>': function () {

                                $(this).dialog("close");
                            }
                        }
                    });
                }
    	        /*
                $.ajax({
                    type: "Post",
                    url: "/Permission/SetBuildingObjectTimeZone",
                    dataType: 'json',
                    traditional: true,
                    data: { id: Id, objectId: cboId, zoneId: tzones[0] },
                    success: function (html) { GetPermissionTree(Id); }
                });*/
            }
            else
                if (tzones.length == 0) {
                    ShowDialog('<%=ViewResources.SharedStrings.PermissionsPermissionGroupNoSelected %>', 2000);
                    /*
                $.ajax({
                    type: "Post",
                    url: "/Permission/ResetBuildingObjectTimeZone",
                    dataType: 'json',
                    traditional: true,
                    data: { id: Id, objectId: cboId },
                    success: function (html) { GetPermissionTree(Id); }
                });*/
                }
            // }
            $.ajax({
                type: "Post",
                url: "/Permission/SaveGroup",
                dataType: 'json',
                traditional: true,
                data: { id: Id, objectIds: bobjs, selectedObjectIds: selbobjs },
                success: function (html) {
                    UpdateName(Id);
                    GetPermissionTree(Id);
                }
            });

        }
        ShowDialog('<%=ViewResources.SharedStrings.CommonSaving %>' + "...", 2000, true);
        return false;
    }

    function DeletePermissionGroup() {
        $("div#modal-dialog").dialog({
            open: function (event, ui) {
                $("div#modal-dialog").html('<%=ViewResources.SharedStrings.CommonConfirmMessage %>');
            },
            resizable: false,
            width: 300,
            height: 140,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.PermissionsDeletingPermission %>',
            buttons:
            {
                    '<%=ViewResources.SharedStrings.BtnDelete %>': function () {
                    $(this).dialog("close");
                    $.post('/Permission/DeleteGroup', { id: $("input#CurrentPermissionGroupId").val() }, function (html) {
                        GetPermissionGroups();
                        $("input#CurrentPermissionGroupId").val("");
                        ShowDialog('<%=ViewResources.SharedStrings.PermissionsPermissionGroupDeletingMessage %>', 2000, true);
                        SetNoSelectedGUI();
                    });
                },
                    '<%=ViewResources.SharedStrings.BtnCancel %>': function () {
                    $(this).dialog("close");
                }
            }
        });
        return false;
    }

    function DelUsersFromPermissionGroup() {
        if ($("input#CurrentPermissionGroupId").val() != " ") {
            var GrpName = $("select#ListOfAllGroups option:selected").text();
            $("div#modal-dialog").dialog({
                open: function (event, ui) {
                    $("div#modal-dialog").html('<%=ViewResources.SharedStrings.CommonConfirmMessage %>' + GrpName);
                },
                resizable: false,
                width: 300,
                height: 180,
                modal: true,
                title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span><%=ViewResources.SharedStrings.PermissionsDeletingUsers%>",
                buttons:
                {
                        '<%=ViewResources.SharedStrings.DltUsersFrmPermGrpYesBtn %>': function () {
                        $(this).dialog("close");

                        DelUsersFromPermGrp();
                        //location.reload(true);
                    },
                        '<%=ViewResources.SharedStrings.DltUsersFrmPermGrpNoBtn %>': function () {
                        $(this).dialog("close");
                    }
                }
            });
            return false;
        }
        else { ShowDialog("There is no permission group selected", 2000, false); }
    }

    function DelUsersFromPermGrp() {
        var Id = $("input#CurrentPermissionGroupId").val();
        var upgId = $("select#ListOfAllGroups").val();
        //var dltGrpName = $("select#ListOfAllGroups option:selected").text();
        $.ajax({
            type: "Get",
            url: "/Permission/UsersInPermissionGroup",//get all users
            data: { Id: Id },
            success: function (data) {
                if (data != null) {
                    if (data.length > 20) {
                        $("div#modal-dialog").dialog({
                            open: function (event, ui) {
                                $("div#modal-dialog").html('<%=ViewResources.SharedStrings.MessageToUsers %>');
                            },
                            resizable: false,
                            width: 300,
                            height: 140,
                            modal: true,
                            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.PermissionsDeletingUsers%>',
                            buttons:
                            {
                                    '<%=ViewResources.SharedStrings.OkBtnText %>': function () { $(this).dialog("close"); }
                            }
                        });
                    }
                    if (data != null) {
                        $.each(data, function (index, val) {
                            var cuserId = val;
                            $.ajax({
                                type: "Get",
                                url: "/User/GetPermName", //get user permision details
                                dataType: 'json',
                                data: { id: cuserId },
                                success: function (data) {
                                    if (data != null) {
                                        var permGrpId = data[0];//User Permission Group Id
                                        var Grp_name = data[1];//User Permission Group name
                                        var parentPg = data[2];//ParentPermisionId for the group
                                        // $.ajax({
                                        // type: "Get",
                                        // url: "/User/CheckBOverlapping",
                                        //  dataType: 'json',
                                        //  data: { PermissionGroupID: permGrpId, Permission_Name: Grp_name },
                                        //  success: function (data) {
                                        // if (data == false) {
                                        if ((permGrpId != "0") && (upgId != "")) {
                                            $.ajax({
                                                type: "Get",
                                                url: "/Permission/DelPermissionGroupFromUsers",
                                                dataType: 'json',
                                                data: { userId: cuserId, dltGroupId: upgId },
                                                success: function (data) {
                                                    //GetUserPermissionGroup();
                                                    //UpdateUserBuildingObjects(data);
                                                    //  if (data == null) { ShowDialog("Can't delete Parent Group", 2000, false); }
                                                    /*if (permtreeshow == true) {
                                                      GetUserPermissionTree($("input#UserId").val(), cupgId);*/

                                                }
                                            });
                                        }
                                        else { alert('There is no Permission Group selected.') }
                                        //  }
                                        /* else {
                                             RenewPG(cuserId, permGrpId, parentPg); //?
                                         }*/
                                        //}

                                        //});
                                    }
                                }
                            });
                        });
                        ShowDialog('<%=ViewResources.SharedStrings.UsersMessageSaveUserPermGroup %>', 2000, true);
                    }
                }
            }
        });

    }


    function DelUsersFromPermGrp() {
        $("#loader").fadeIn();
        var Id = $("input#CurrentPermissionGroupId").val();
        var upgId = $("select#ListOfAllGroups").val();
        //var dltGrpName = $("select#ListOfAllGroups option:selected").text();
        $('input#button_delete_permission_group_users').attr("disabled", true);
        //$("input#button_delete_permission_group_users").fadeIn();
        $.ajax({
            type: "Get",
            url: "/Permission/UsersInPermissionGroup",//get all users
            data: { Id: Id },
            success: function (data) {
                if (data != null) {
                    if (data.length > 20) {
                        $("div#modal-dialog").dialog({
                            open: function (event, ui) {
                                $("div#modal-dialog").html('<%=ViewResources.SharedStrings.MessageToUsers %>');
                            },
                            resizable: false,
                            width: 300,
                            height: 140,
                            modal: true,
                            async: false,
                            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.PermissionsDeletingUsers%>',
                            buttons:
                            {
                              '<%=ViewResources.SharedStrings.OkBtnText %>': function () { $(this).dialog("close"); }
                            }
                        });
                    }
                    if (data != null) {

                        var str = data.join();
                        ShowDialog('<%=ViewResources.SharedStrings.UsersMessageSaveUserPermGroup %>', 2000, true);
                        $.ajax({
                            type: "Get",
                            url: "/Permission/DelPermissionGroupFromUsersNew", //get user permision details
                            dataType: 'json',
                            data: { userId: str, dltGroupId: upgId },
                            async: false,
                            success: function (data) {
                            }
                        });
                        SelectPermissionGroup();
                        $("#loader").fadeOut();
                    }
                }
            }
        });
        $('#button_delete_permission_group_users').removeAttr("disabled");
        $("input#button_delete_permission_group_users").fadeOut();
    }

    function RenewPG(cuser, cupg, parentP) {
        var cuserId = cuser;
        var cupgId = cupg;
        var parentPg = parentP
        if (cupgId != "0") {
            //alert("executing:Renew()");
            ShowDialog('<%=ViewResources.SharedStrings.UsersMessageSaveUserPermGroup %>', 2000, true);
            $.ajax({
                type: "Get",
                url: "/User/ChangeUserPermissionGroup",
                dataType: 'json',
                data: { userId: cuserId, newUpgId: parentPg },
                success: function (data) {
                    $("input#CurrentUserPermissionGroupId").val(data);
                }
            });
        }
        else {
            ShowDialog('<%=ViewResources.SharedStrings.Error %>', 2000, true);
        }
    }

    function SetNoSelectedGUI() {
        $("input#button_add_permission_group").fadeIn();
        $("input#button_save_permission_group").fadeOut();
        $("input#button_delete_permission_group").fadeOut();
        $("input#button_delete_permission_group").fadeOut();
        $("div#currentPermissionGroupName").find("a").html('<%=ViewResources.SharedStrings.PermissionsNoGroupSelected %>');
        if ($("input#CurrentPermissionGroupId").val() != "") {
            GetPermissionTree($("input#CurrentPermissionGroupId").val());
        } else {
            $.get('/Permission/GetTree', function (html) {
                $("div#AreaPermissionTree").html(html);
                $("div#AreaPermissionTree").attr("id", function () {
                    $(this).corner("bevelfold");
                });
                $("ul#permissions").treeview({
                    animated: "fast",
                    persist: "location",
                    collapsed: true,
                    unique: true
                });
            });
        }
        $("div#PermissionTimeZoneList").fadeOut(300);
        return false;
    }

    function CheckRoom(cntr, isRoom) {
        if (isRoom == 1) {
            par_table = $(cntr).parents('#permZoneTreeListRoomNode');
            arm_row = par_table.find('#armingDisarminRow');
            if ($(cntr).is(':checked') == true) { arm_row.show(); }
            else { arm_row.hide(); }
        }
    }
    function ShowUsersInGroup() {
        if ($("input#CurrentPermissionGroupId").val() != "") {
            var Id = $("input#CurrentPermissionGroupId").val();
            $.ajax({
                type: "Get",
                url: "/User/CheckPermissionUsers",
                data: { id: Id },
                beforeSend: function () {
                    $("div#AreaLogSearchResultsWait").fadeIn('slow');
                },
                success: function (result) {
                    $("div#AreaLogSearchResultsWait").hide();
                    $("div#PermissionTimeZoneList").html(result);
                    $("div#PermissionTimeZoneList").fadeIn(300);
                }
            });
        }
    }

    function GetZoneByObject(Id, isRoom, name) {
        if ($("input#CurrentPermissionGroupId").val() == "") {
             //ShowDialog('<%=ViewResources.SharedStrings.PermissionsPermissionGroupNoSelected %>', 2000);
            $.ajax({
                type: "Get",
                url: "/User/CheckObjectPermission",
                data: { id: Id },
                beforeSend: function () {
                    $("div#AreaLogSearchResultsWait").fadeIn('slow');
                },
                success: function (result) {
                    $("div#AreaLogSearchResultsWait").hide();
                    $("div#PermissionTimeZoneList").html(result);
                    $("div#PermissionTimeZoneList").fadeIn(300);
                }
            });
            //alert("Who can use that");
            return true;
        }
        if (isRoom == 1) {
            ShowDialog('<%=ViewResources.SharedStrings.PermissionsCanNotSetTimeZoneToRoom %>', 2000);
            return false;
        }
        $("li[id^=permissionTreeLi_]").each(function () { $(this).css('background-color', 'transparent'); });
        $("li#permissionTreeLi_" + Id).css('background-color', '#AAAAAA').corner();
        $("input#permissionTreeObject_" + Id).attr('checked', 'checked');
        $("input#CurrentBuildingObjectId").val(Id);
        $.ajax({
            type: "Get",
            url: "/Permission/GetActiveZone",
            data: { id: $("input#CurrentPermissionGroupId").val(), objectId: Id },
            beforeSend: function () {
                $("#button_submit_permission_time_zone_search").addClass("Trans");
            },
            success: function (result) {
                $("div#PermissionTimeZoneList").html(result);
                $("div#PermissionTimeZoneList").fadeIn(300);
                $("#button_submit_permission_time_zone_search").removeClass("Trans");
                $("input[id^=permissionTimeZoneCheck_][type='checkbox']").each(function () {
                    $(this).attr('checked', 'checked');
                });
            }
        });
        $("input#button_change_object_time_zone").fadeIn();
        ButtonName.value = "TimeZone to " + name;
        //SubmitPermissionTimeZoneSearch();
        return false;
    }

    function ToggleArming(id) {
        var pId = $("input#CurrentPermissionGroupId").val();
        if (pId != "") {
            $.post('/Permission/ToggleArming', { boId: id, pgId: pId }, function (html) { });
        }
        else { ShowDialog('<%=ViewResources.SharedStrings.PermissionsPermissionGroupNoSelected %>', 2000); }
    }

    function ToggleDefaultArming(id) {
        var pId = $("input#CurrentPermissionGroupId").val();
        if (pId != "") {
            $.post('/Permission/ToggleDefaultArming', { boId: id, pgId: pId }, function (html) { });
        }
        else { ShowDialog('<%=ViewResources.SharedStrings.PermissionsPermissionGroupNoSelected %>', 2000); }
    }

    function ToggleDisarming(id) {
        var pId = $("input#CurrentPermissionGroupId").val();
        if (pId != "") {
            $.post('/Permission/ToggleDisarming', { boId: id, pgId: pId }, function (html) { });
        }
        else { ShowDialog('<%=ViewResources.SharedStrings.PermissionsPermissionGroupNoSelected %>', 2000); }
    }

    function ToggleDefaultDisarming(id) {
        var pId = $("input#CurrentPermissionGroupId").val();
        if (pId != "") {
            $.post('/Permission/ToggleDefaultDisarming', { boId: id, pgId: pId }, function (html) { });
        }
        else { ShowDialog('<%=ViewResources.SharedStrings.PermissionsPermissionGroupNoSelected %>', 2000); }
    }

    function NewDefaultTimeZone() {
        $("input#button_change_object_time_zone").fadeOut();
        if ($("input#CurrentPermissionGroupId").val() != "") {
            var Id = $("input#CurrentPermissionGroupId").val();
            var bobjs = new Array();

            $("input[id^=permissionTreeObject_][type='checkbox']").each(function () {
                bobjs.push($(this).attr('name'));
            });
            var selbobjs = new Array();
            $("input[id^=permissionTreeObject_][type='checkbox']").each(function () {
                if ($(this).is(':checked') == true) {
                    selbobjs.push($(this).attr('name'));
                }
            });
            var tzones = new Array();
            $("input[id^=permissionTimeZoneCheck_][type='checkbox']").each(function () {
                if ($(this).is(':checked') == true) {
                    tzones.push($(this).attr('name'));
                }
            });
            var cboId = $("input#CurrentBuildingObjectId").val();
            $("input#CurrentBuildingObjectId").val("");
            if (cboId != "") {
                if (tzones.length == 1) {

                    $.ajax({
                        type: "Post",
                        url: "/Permission/SetBuildingObjectTimeZone",
                        dataType: 'json',
                        traditional: true,
                        data: { id: Id, objectId: cboId, zoneId: tzones[0] },
                        success: function (html) { GetPermissionTree(Id); }
                    });
                } else {
                    $.ajax({
                        type: "Post",
                        url: "/Permission/ResetBuildingObjectTimeZone",
                        dataType: 'json',
                        traditional: true,
                        data: { id: Id, objectId: cboId },
                        success: function (html) { GetPermissionTree(Id); }
                    });
                }
            }
            $.ajax({
                type: "Post",
                url: "/Permission/SaveGroup",
                dataType: 'json',
                traditional: true,
                data: { id: Id, objectIds: bobjs, selectedObjectIds: selbobjs },
                success: function (html) { }
            });
        }
    }

    function GetPermissionTree(Id) {
        $.ajax({
            type: "Get",
            url: "/Permission/GetTree",
            data: { id: Id },
            beforeSend: function () {
                $('#button_save_permission_group').fadeOut();
                $("div#AreaPermissionTree").fadeOut(300);
            },
            success: function (html) {
                $("div#AreaPermissionTree").html(html);
                $("div#AreaPermissionTree").attr("id", function () {
                    $(this).corner("bevelfold");
                });
                $("ul#permissions").treeview({
                    animated: "fast",
                    persist: "location",
                    collapsed: false,
                    unique: true
                });
                $("div#AreaPermissionTree").fadeIn(300, function () {
                    $('#button_save_permission_group').fadeIn();
                });
            }
        });
    }
</script>
<p>
</p>
&nbsp;