<%@ Control Language="C#"  Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<FoxSec.Web.ViewModels.TaUserGroupShifts>>" %>
<div style="float:right">
    <input type="button" value="Add TaUserGroup shift" id="AddNewTaUserGroupShifts" class="ui-button ui-widget ui-state-default ui-corner-all" onclick="AddNewTaUserGroupShifts()" />
</div>
<table style="width:100%">
    <colgroup>
         <col style="width: 20%" />
         <col style="width: 70%" />
         <col style="width: 5%" />
    </colgroup>
    
    <tbody>
        <tr>
            <th><label>Name</label></th>
            <th><label>Repeat weeks</label></th>
        </tr>
        <%  int x = 1; foreach (var item in Model)
            {

                if (x % 2 == 0){%>
        
        <tr style="background-color:#fff">
            <%} else {%>
            <tr style="background-color:#ccc">
                <%} %>
           <td><%=item.Name %></td>
            <td><%=item.RepeatAfterWeeks %></td>
            <td><span id='EditGroupShiftButton' title ="Edit" class='icon icon_green_go' onclick='EditTaUserGroupShift(<%=item.Id %>)'"></span></td>
            <%--<td><span id='AddGroupShiftsToUsers' title ="Add users the shifts" class='icon icon_green_go' onclick='AddGroupShiftsToUsers(<%=item.Id %>)'"></span></td>--%>
            <td><span id='AddGroupShiftsToUsers' title ="Add users the shifts" class='icon icon_green_go' onclick='AddGroupShiftsToUsers(<%=item.Id %>)'"></span></td>
           <%-- <td><span id='AddNewTaUserGroupShifts' title ="Add TaUserGroup shift" class='icon icon_green_go' onclick='AddNewTaUserGroupShifts()'"></span></td>--%>
            <%=Html.Hidden(item.Id.ToString(),item.Name, new {id=item.Id.ToString() } )%>
                
        </tr>
        <% x = x + 1; } %>
    </tbody>
</table>

<%--<script>
    //function EditTaUserGroupShift(taUserGroupid) {
        
    //    debugger;
    //     var taUserGroupShiftName = $('#' + taUserGroupid.toString()).val();
    //    $("div#TaUserGroupShiftDialog").dialog({
    //        modal: true,
    //        open: function () {
    //           // $("div#TaUserGroupShiftDialog").html("<div id='AreaUserEditWait' style='width: 100%; height:580px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
    //            $.ajax({
    //                type: "Get",
    //                data: { TaUserGroupId: taUserGroupid },
    //                url: "/TAReport/EditTaUserGroupShift",
    //                success: function (result) {
    //                    $("div#TaUserGroupShiftDialog").html(result);
    //                }
    //            });
                    
    //        },
    //        resizable: true,
    //        title: "<span class='ui-icon ui-icon-home' style='float:left; margin:1px 5px 0 0'></span>" +taUserGroupShiftName ,
    //        width: 1230,
    //        height: 900,
    //        buttons: {
    //            "Close": function () {

    //            },
    //            "Save": function () {

    //            }
    //        }
            
    //    });
    //}

    //function AddGroupShiftsToUsers(taGroupUserShiftsId) {
    //    debugger;
    //    var taUserGroupShiftName = $('#' + taGroupUserShiftsId.toString()).val();
    //    $("#AddTaUserGroupShiftsToUserDialog").dialog({
    //        open: function () {
    //            $.ajax({
    //                type: "Get",
    //                url: "/TaReport/AssignTaUserGroupShiftToUsers",
    //                data: { GroupShiftId: taGroupUserShiftsId },
    //                success: function (result) {
    //                    $("#AddTaUserGroupShiftsToUserDialog").html(result);
    //                }

    //            });
    //        },
    //        modal: true,
    //        resizable: false,
    //        height: 800,
    //        title: "<span class='ui-icon ui-icon-home' style='float:left; margin:1px 5px 0 0'></span>" +taUserGroupShiftName ,
    //        width: 1230,
    //        buttons: {
    //            "Save": function () {
    //                debugger;
    //                $("#startDateErrorMessage").css("display", "none");
    //                $("#endDateErrorMessage").css("display", "none");
    //                var taUserGroupShiftId = $("#TaUsersGroupShiftId").val();
    //                var startDate = $("#startDate").val();
    //                var endDate = $("#endDate").val();
    //                var selectedUsersId = [];
    //                $(".users").each(function () {
    //                    if ($(this).is(":checked")) {
    //                        selectedUsersId.push($(this).val());
    //                    }
    //                });
    //                var selectedUsersTA = [];
    //                $(".isTA").each(function () {
    //                    if ($(this).is(":checked")) {
    //                         selectedUsersTA.push($(this).val());
    //                    } 
    //                });
    //                var model = {
    //                    TaUserGroupShiftId: taUserGroupShiftId,
    //                    StartDate: startDate,
    //                    EndDate: endDate,
    //                    SelectedUsersId: selectedUsersId,
    //                    SelectedUsersIsTA: selectedUsersTA
    //                };
    //                if (startDate.length > 0 && endDate.length > 0) {
    //                    $.ajax({
    //                        type: "Post",
    //                        url: "/TaReport/AssignTaUserGroupShiftToUsersSave",
    //                        data: model,
    //                        dataType: "json",
    //                        success: function () {

    //                        }
    //                    });
    //                }
    //                else if (startDate.length > 0 && endDate.length == 0) {
    //                    $("#endDateErrorMessage").css("display", "inline");
    //                }
    //                else if (startDate.length == 0  && endDate.length > 0) {
    //                    $("#startDateErrorMessage").css("display", "inline");
    //                }
    //                else if (startDate.length == 0 && endDate.length == 0) {
    //                    $("#startDateErrorMessage").css("display", "inline");
    //                    $("#endDateErrorMessage").css("display", "inline");
    //                }
    //            },
    //            "Close": function () {
    //                $(this).dialog('close');
    //            }
    //        }

    //    });
    //}

    //function AddNewTaUserGroupShifts() {
    //    $("#AddTaUserGroupShiftsDialog").dialog({
    //        modal:true,
    //        open: function () {
    //            $.ajax({
    //                type: "Get",
    //                url: "/TaReport/AddTaUserGroupShifts",
    //                success: function (result) {
    //                    $("#AddTaUserGroupShiftsDialog").html(result);
    //                }
    //            });
    //        },
    //        resizable: false,
    //        width: 1100,
    //        height: 700,
    //        title: "<span class='ui-icon ui-icon-home' style='float:left; margin:1px 5px 0 0'></span> Add new TaUser group shift ",
    //        buttons: {
    //            "Save": function () {
    //                debugger;
    //                var taGroupShiftName = $("#taUserGroupShiftName").val();
    //                var repeatWeeks = $("#repeatCycle").val();
    //                var selectedTaWeeks = [];
    //                var toCheck = [];
    //                $(".dynamicSelectedValue").each(function () {
    //                    selectedTaWeeks.push($(this).val());
    //                });
    //                var taNewUserGroupShiftsModel = {
    //                    Name: taGroupShiftName,
    //                    RepeatAfterWeeks: repeatWeeks,
    //                    SelectedTaWeeks: selectedTaWeeks
    //                };
    //                $.ajax({
    //                    type: "Post",
    //                    url: "/TaReport/SaveNewTaUserGroupShift",
    //                    dataType: "json",
    //                    data: { TaUserGroupShiftSaveModel: taNewUserGroupShiftsModel },
    //                    success: function (response) {
    //                        debugger;
    //                        ShowDialog(response.msg, 2000, response.IsSucceed);
    //                        $(this).dialog('close');
    //                        $("div#modal-dialog").dialog('close');
    //                    }
    //                });
    //            },
    //            "Cancel": function () {
    //                $(this).dialog('close');
    //            }
    //        }
    //    });
    //}
</script>--%>
