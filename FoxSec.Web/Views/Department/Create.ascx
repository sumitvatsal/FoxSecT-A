<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.DepartmentEditViewModel>" %>
<%@ Import Namespace="FoxSec.Web.Helpers" %>

<div id="content_add_department_form" style='margin:10px; text-align:center;' >
<form id="createNewDepartment">
<table width="100%" cellpadding="3" cellspacing="3" style="margin: 0; padding: 3px; border-spacing: 3px;">
    <%if (Model.User.IsSuperAdmin || Model.User.IsBuildingAdmin){%>
    <tr>
		<td style='width:30%; padding:2px; text-align:right;'><label for='Add_department_companyId'><%:ViewResources.SharedStrings.UsersCompany %></label></td>
		<td style='width:70%; padding:2px;'><%= Html.DropDownList("Department.CompanyId", Model.CompanyList, new { @style = "width:80%", id = "Add_department_companyId", onchange = "javascript:OnChangeCompany();" })%></td>
    </tr>
    <%} else { %> 
                <%=Html.Hidden("Department.CompanyId", Model.Department.CompanyId, new { id = "Add_department_companyId" })%> 
           <%}%>
    <tr>
		<td style='width:30%; padding:2px; text-align:right; vertical-align:top;'><label for='Add_department_number'><%:ViewResources.SharedStrings.CommonNumber %></label></td>
		<td style='width:70%; padding:2px;'>
			<%=Html.TextBox("Department.Number", Model.Department.Number, new { @style = "width:20%", id = "Add_department_number" })%>
			<%= Html.ValidationMessage("Department.Number", null, new { @class = "error" })%>
		</td>
    </tr>
    <tr>
		<td style='width:30%; padding:2px; text-align:right;'><label for='Add_department_title'><%:ViewResources.SharedStrings.UsersName %></label></td>
		<td style='width:70%; padding:2px;'>
			<%=Html.TextBox("Department.Name", Model.Department.Name, new { @style = "width:80%;text-transform: capitalize;", id = "Add_department_title" })%>
			<%= Html.ValidationMessage("Department.Name", null, new { @class = "error" })%>
		</td>
    </tr>
</table>
<div id="panel_company_buildings" class="ui-tabs ui-widget ui-widget-content ui-corner-all">
        <ul class="ui-tabs-nav ui-helper-reset ui-helper-clearfix ui-widget-header ui-corner-all">
            <li class="ui-state-default ui-corner-top ui-tabs-selected ui-state-active"><a href="#tab_dep_managers_menu"><%:ViewResources.SharedStrings.DepartmentsDepartmentManagers %></a></li>
        </ul>
      
        <div id = "departmentManagerList">
		<%if(ViewContext.ViewData.ModelState.IsValid)
		{
    		Html.RenderAction("Manager", "Department", new { companyId = 0 });
		}else
		{
			Html.RenderPartial("Manager", Model);
		}%>
		</div>

</div>
</form>
</div>

<script type="text/javascript" language="javascript">

    $(document).ready(function () {
        $("input#add_new_dep_manager").addClass("Trans");
        OnChangeCompany();
    });
	
	    function OnChangeCompany()
    {
        if($("#Add_department_companyId").val())
        {
            $.get('/Department/Manager', { companyId: $("#Add_department_companyId").val() }, function (html) {
                      $("div#departmentManagerList").html(html);
            });
        }
    }
</script>

  <script type="text/javascript" language="javascript">
    $(document).ready(function () {
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
        minDate: $(".date_end").val()
      });
    });
</script>

