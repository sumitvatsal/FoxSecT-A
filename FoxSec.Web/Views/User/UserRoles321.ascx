<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.UserRoleModel>" %>
<%@ Import Namespace="FoxSec.Web.Helpers" %>

<form id="editUserRoles" action="">
<%= Html.HiddenFor(m=>m.UserId) %>
<%=Html.HiddenFor(m=>m.IsCurrentUser) %>
 
    <table>
        <tr>
	        <td style='width:20%; padding:2px; text-align:right;'><label for='WorkTime'>Card alarm to Email</label></td>
	        <td style='width:40%; padding:2px;'><%=Html.CheckBox("CardAlarm", Model.CardAlarm)%></td>
	        <td style='width:40%; padding:2px;'>
	        </td>
        </tr>
        <tr>
	        <td style='width:20%; padding:2px; text-align:right;'><label for='WorkTime'><%:ViewResources.SharedStrings.UsersESeriviceAllowed %></label></td>
	        <td style='width:40%; padding:2px;'><%=Html.CheckBox("EServiceAllowed", Model.EServiceAllowed)%></td>
	        <td style='width:0%; padding:2px;'>
	        </td>
        </tr>
                <tr>
	        <td style='width:20%; padding:2px; text-align:right;'><label for='WorkTime'>Is Visitor</label></td>
	        <td style='width:40%; padding:2px;'><%=Html.CheckBox("IsVisitor", Model.IsVisitor)%></td>
	        <td style='width:40%; padding:2px;'>
	        </td>
        </tr>
    </table>



<table cellpadding="3" cellspacing="3" style="margin:0; width:100%; padding:3px; border-spacing:3px;">
  
<tr>
	<th style='width:20px; padding:2px;'><%:ViewResources.SharedStrings.CommonId %></th>
	<th style='width:20%; padding:2px;'><%:ViewResources.SharedStrings.UsersRoleTitle %></th>
	<th style='width:60%; padding:2px; text-align:center;'><%:ViewResources.SharedStrings.CommonValidationPeriod %></th>
	<th style='width:20%; padding:2px; text-align:right;'><%:ViewResources.SharedStrings.CommonIsAllowed %></th>
</tr>

<% var i = 1; foreach (var role in Model.Roles) { %>
<tr id="userRoleRow">
	<td style='width:20px; padding:2px;'><%= Html.Encode(i++) %></td>
	<td style='width:20%; padding:2px; cursor:help;' class='tipsy_we' original-title='<%= role.RoleDescription %>'>
		<%= Html.Encode(role.RoleName) %>
		<%=Html.Hidden(ViewHelper.GetPrefixedName("Roles", "RoleName", Model.Roles.IndexOf(role)), role.RoleName) %>
		<%=Html.Hidden(ViewHelper.GetPrefixedName("Roles", "RoleDescription", Model.Roles.IndexOf(role)), role.RoleDescription) %>
		<%=Html.Hidden(ViewHelper.GetPrefixedName("Roles", "Id", Model.Roles.IndexOf(role)), role.Id) %>
	</td>
	<td style='width:60%; padding:2px; text-align:center;'>
		<%=Html.TextBox(ViewHelper.GetPrefixedName("Roles", "ValidFrom", Model.Roles.IndexOf(role)), role.ValidFrom, new { @class = "date_start", style = "width:90px" })%>
		- 
		<%=Html.TextBox(ViewHelper.GetPrefixedName("Roles", "ValidTo", Model.Roles.IndexOf(role)), role.ValidTo, new { @class = "date_end", style = "width:90px" })%>
		<%= Html.ValidationMessage(ViewHelper.GetPrefixedName("Roles", "ValidFrom", Model.Roles.IndexOf(role)), null, new { @class = "error" })%>
		<%= Html.ValidationMessage(ViewHelper.GetPrefixedName("Roles", "ValidTo", Model.Roles.IndexOf(role)), null, new { @class = "error" })%>
	</td>
	<td style='width:20%; padding:2px; text-align:right;'>
		<%=Html.Hidden(ViewHelper.GetPrefixedName("Roles", "RoleId", Model.Roles.IndexOf(role)), role.RoleId) %>
		<%=Html.CheckBox(ViewHelper.GetPrefixedName("Roles", "IsSelected", Model.Roles.IndexOf(role)), role.IsSelected) %>
	</td>
</tr>
<% } %>
</table>
</form>
<br />
<% if(!Model.IsCurrentUser){ %>
<input type='button' value='<%=ViewResources.SharedStrings.BtnSave %>' onclick='EditUserRoles()' />
<%} %>

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
				var d = new Date();
				var year = d.getFullYear();
				if (localStorage.getItem('DateRange') != null)
				{
				    var dt = localStorage.getItem('DateRange');
				    alert(dt);
				    d.setDate(d.getDate() + dt);
				    //d.addDays(dt);
				    
				    var dateString =d.getDate().toString()+"."+ (d.getMonth()+1).toString()+"."+ d.getFullYear().toString();
                       // d.toString('dd.MM.yyyy');
				    alert(dateString);
				    row.find(".date_end").val(dateString);

				} else {

				    year = year + 2;
				    row.find(".date_end").val("31.12." + year);

				}

				//row.find(".date_end").val("31.12.2015");
				
				//row.find(".date_end").val("{0}", dateText.val());
			}
		});

		$(".date_end").datepicker({
			dateFormat: "dd.mm.yy",
			firstDay: 1,
			changeMonth: true,
			changeYear: true,
			showButtonPanel: false,
			minDate: $(".date_end").val()
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

	function EditUserRoles() {
        /*
      var st = $('.date_end').val();
      var pattern = /(\d{2})\.(\d{2})\.(\d{4})/;
      var dt = new Date(st.replace(pattern, '$3-$2-$1'));
      alert(dt);*/
    //  var range = dt - Date.now();
      //alert(range);
      
	    var d1 = $(".date_start").val();
	    var d2 = $(".date_end").val();
	    alert(d1);
	      var minutes = 1000 * 60;
          var hours = minutes * 60;
          var day = hours * 24;
          var pattern = /(\d{2})\.(\d{2})\.(\d{4})/;
          var dt1 = new Date(d1.replace(pattern, '$3-$2-$1'));
          var dt2 = new Date(d2.replace(pattern, '$3-$2-$1'));
         // var startdate1 = getDateFromFormat(d1, "dd.mm.yy");
	    // var enddate1 = getDateFromFormat(d2, "dd.mm.yy");

          var days = 1 + Math.round((dt2 - dt1) / day);

         

          if (!isNaN(days))
          { localStorage.setItem('DateRange', days);}
          alert(days);
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
//		
//		
//		
//		
//		$.post("/User/EditRoles", $("#editUserRoles").serialize());
//		ShowDialog("Saved", 2000, true);
//		ReloadEditPage();
	}



</script>

