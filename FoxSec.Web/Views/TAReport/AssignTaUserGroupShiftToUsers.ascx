<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.AssignShiftUserModel>" %>
<div>
    <label>Start Date</label>
    <input type="date" id="startDate" />
    <label id="startDateErrorMessage" style="display:none; color:red;">*Please select start date</label>
    <label>End Date</label>
    <input type="date" id="endDate" />
    <label id="endDateErrorMessage" style="display:none; color:red;">*Please select end date</label>
    <input type="button" id="removeGroupScheduleFromUsers" value="Remove user Group Schedule" class="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" onclick="RemoveUsersGroupSchedule()" />
</div>
<%=Html.Hidden("TaUsersGroupShiftId",Model.TaUsersGroupShiftId) %>

<table style="width:100%">
    <colgroup>
        <col style="width:5%" />
        <col style="width:40%" />
        <col style="width:20%" />
        <col style="width:15%" />
        <col style="width:15%" />
        <col style="width:5%" />
    </colgroup>
    <thead style="background-color:#000000">
        <tr>
            <th><input type="checkbox" id="selectAllCheckBox" onclick="selectAllCheckBoxes()" /></th>
            <th style="color:#fff">Name</th>
            <th style="color:#fff">GroupShift</th>
            <th style="color:#fff">Start Date</th>
            <th style="color:#fff">End Date</th>
            <th style="color:#fff">Is T&A</th>
        </tr>
    </thead>
   <tbody>
       <% int count = 0; foreach(var user in Model.AssignShiftUserModelsList)
           {
               count = count + 1;
               if(count % 2 == 0)
               {%>
       <tr style="background-color:#fff">
         <td><input type="checkbox" class="users" value="<%=user.Id %>" <%if (user.IsSelected)
                 { %> checked <%} %>/></td>
          <td><label><%=user.FirstName +" "+ user.LastName %></label></td> 
           <td><%if (user.TaUsersShifts != null) {%><label><%=user.TaUsersShifts.TaUserGroupShifts.Name %><%}%></label></td>
           <td><%if(user.StartDate != null){ %><label><%=user.StartDate.Value.Date.ToString() %></label><%} %></td>
           <td><%if(user.EndDate != null){ %><label><%=user.EndDate.Value.Date.ToString() %></label><%} %></td>
           <td><input type="checkbox" class="isTA" value="<%=user.Id %>" <%if (user.WorkTime != null && user.WorkTime.Value) {%> checked <%} %>/></td>
       </tr>
              <% }
                  else
                  {%>
       <tr style="background-color:#ccc">
         <td><input type="checkbox" class="users" value="<%=user.Id %>" <%if (user.IsSelected)
                 { %> checked <%} %> /></td>
          <td><label><%=user.FirstName +" "+ user.LastName %></label></td>  
           <td><%if (user.TaUsersShifts != null) {%><label><%=user.TaUsersShifts.TaUserGroupShifts.Name %><%}%></label></td>
           <td><%if(user.StartDate != null){ %><label><%=user.StartDate.Value.Date.ToString() %></label><%} %></td>
           <td><%if(user.EndDate != null){ %><label><%=user.EndDate.Value.Date.ToString() %></label><%} %></td>
           <td><input type="checkbox" class="isTA" value="<%=user.Id %>" <%if(user.WorkTime !=null && user.WorkTime.Value){ %> checked <%} %>/></td>
       </tr>
                  <%}
               %>
       
       <%} %>
   </tbody>
</table>

<script>
    function selectAllCheckBoxes() {
        debugger;
        if ($("#selectAllCheckBox").is(":checked")) {
            $(".users").each(function () {
                $(".users").prop("checked", true);
            });
        }
        else if ($("#selectAllCheckBox").is(":checked") == false) {
            $(".users").each(function () {
                $(".users").prop("checked", false);
            });
            
        }
    }

    function RemoveUsersGroupSchedule() {
        var selectedUsers = [];
        $(".users").each(function () {
            if ($(this).is(":checked")) {
                selectedUsers.push($(this).val());
            }
        });

        $.ajax({
            type: "Post",
            url: "/TaReport/RemoveUsersGroupSchedule",
            data: { UsersListToRemove: selectedUsers },
            datatype: "json",
            success: function () {

            }
        });
    }
</script>