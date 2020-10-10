<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.DepartmentEditViewModel>" %>
<div id = <%="managerTable_" + Model.ManagerId%>>
   <table width="100%" id="Table1" cellpadding="3" cellspacing="3" style="border-bottom: 1px solid #CCCCCC; margin: 0; padding: 3px; border-spacing: 3px;">
    <tr>
        <td style="width:40%; padding:2px; vertical-align: top; font-size:13px;"><%= Html.DropDownList("Managers[" + Model.ManagerId + "].UserId", new SelectList(Model.Department.DepartmentManagersList, "Key", "Value"), ViewResources.SharedStrings.DefaultDropDownValue, new { @id = "departmentManager_" + Model.ManagerId, @style = "width:100%", onchange = "javascript:SelectManager('" + Model.ManagerId + "','" + Model.Department.Id + "');" })%></td>
        <td style="width:45%; padding:2px; vertical-align: top;">
            <div id=<%="departmentManagerValidationPeriod_" + Model.ManagerId  %>>
              <%=Html.TextBox("Managers[" + Model.ManagerId + "].ValidFrom", DateTime.Now.ToString("dd.MM.yyyy"), new { @style = "width:90px", @class = "date_start" })%>
                    -
              <%=Html.TextBox("Managers[" + Model.ManagerId + "].ValidTo", DateTime.Now.ToString("dd.MM.yyyy"), new { @style = "width:90px", @class = "date_end" })%>
            </div>
        </td>
        <td style="width:15%; padding:2px; vertical-align: top; text-align:center">
            <span id=<%="deleteManager_" + Model.ManagerId %> class="ui-icon ui-icon-closethick" style="cursor:pointer;; display:none" onclick="javascript:DeleteManager(<%= Model.ManagerId %>, <%= Model.Department.Id %>);"></span>
            <span id=<%="addManager_" + Model.ManagerId %> class="ui-icon ui-icon-circle-plus" style="cursor:pointer; display:none" onclick="javascript:AddManager(<%= Model.ManagerId %>, <%= Model.Department.Id %>);"></span>
        </td>
    </tr>
    </table>
</div>

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