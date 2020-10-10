<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.DepartmentEditViewModel>" %>
<%@ Import Namespace="FoxSec.Web.Helpers" %>

<div id="content_edit_department_form" style='margin:10px; text-align:center;' >
<form id="editDepartmentTable">
<table width="100%" cellpadding="3" cellspacing="3" style="margin: 0; padding: 3px; border-spacing: 3px;">
<%=Html.Hidden("Department.Id", Model.Department.Id)%>
    <%if (Model.User.IsSuperAdmin || Model.User.IsBuildingAdmin){%>
    <tr>
		<td style='width:30%; padding:2px; text-align:right;'><label for='Edit_department_companyId'><%:ViewResources.SharedStrings.UsersCompany %></label></td>
		<td style='width:70%; padding:2px;'><%= Html.DropDownList("Department.CompanyId", Model.CompanyList, new { @style = "width:80%", id = "Edit_department_companyId_" + Model.Department.Id })%></td>
    </tr>
    <%} else { %> 
                <%=Html.Hidden("Department.CompanyId", Model.Department.CompanyId)%> 
           <%}%>
    <tr>
		<td style='width:30%; padding:2px; text-align:right; vertical-align:top;'><label for='Edit_department_number_<%= Model.Department.Id %>'><%:ViewResources.SharedStrings.CommonNumber %></label></td>
		<td style='width:70%; padding:2px;'>
			<%=Html.TextBox("Department.Number", Model.Department.Number, new { @style = "width:20%", id = "Edit_department_number_" + Model.Department.Id })%>
			<%= Html.ValidationMessage("Department.Number", null, new { @class = "error" })%>
		</td>
    </tr>
    <tr>
		<td style='width:30%; padding:2px; text-align:right;'><label for='Edit_department_title_<%= Model.Department.Id %>'><%:ViewResources.SharedStrings.UsersName %></label></td>
		<td style='width:70%; padding:2px;'>
			<%=Html.TextBox("Department.Name", Model.Department.Name, new { @style = "width:80%", id = "Edit_department_title_" + Model.Department.Id })%>
			<%= Html.ValidationMessage("Department.Name", null, new { @class = "error" })%>
		</td>
    </tr>
</table>
<div  style='margin: 10px 0px 10px 0px;' id="panel_company_buildings" class="ui-tabs ui-widget ui-widget-content ui-corner-all">
        <ul class="ui-tabs-nav ui-helper-reset ui-helper-clearfix ui-widget-header ui-corner-all">
            <li class="ui-state-default ui-corner-top ui-tabs-selected ui-state-active"><a href="#tab_dep_managers_menu"><%:ViewResources.SharedStrings.DepartmentsDepartmentManagers %></a></li>
        </ul>

<table width="100%" id="add_dep_manager" cellpadding="3" cellspacing="3" style="border-bottom: 1px solid #CCCCCC; margin: 0; padding: 3px; border-spacing: 3px;">
    <tr>
        <th style="width:40%; padding:2px;"><%:ViewResources.SharedStrings.DepartmentsDepartmentManagers %></th>
        <th style="width:45%; padding:2px;"><%:ViewResources.SharedStrings.CommonValidationPeriod %></th>
        <th style='width:15%; padding:2px;'><%:ViewResources.SharedStrings.BtnDelete %></th>
    </tr>
</table>
            <div id = "departmentManagerList"> </div>

</div>
</form>
</div>

<script type="text/javascript" language="javascript">
  $(document).ready(function () {
        $.get('/Department/ManagerList', { departmentId: <%= Model.Department.Id %> }, function (html) {
                $("div#departmentManagerList").html(html);
        });
  });  
</script>

