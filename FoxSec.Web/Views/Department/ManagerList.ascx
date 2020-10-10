<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.DepartmentEditViewModel>" %>
    <%int i = 0; foreach (var manager in Model.Managers){%>
        <div id = <%="managerTable_" + i%>>
            <table width="100%"cellpadding="3" cellspacing="3" style="border-bottom: 1px solid #CCCCCC; margin: 0; padding: 3px; border-spacing: 3px;">
                  <tr>
                      <td style="width:40%; padding:2px;"><%= Html.DropDownList("Managers[" + i + "].UserId", new SelectList(Model.Department.DepartmentManagersList, "Key", "Value", manager.UserId), ViewResources.SharedStrings.DefaultDropDownValue, new { @id = "departmentManager_" + i, @style = "width:100%", onchange = "javascript:SelectManager('" + i + "','" + Model.Department.Id + "');" })%></td>
                      <td style="width:45%; padding:2px;">
                          <div id=<%="departmentManagerValidationPeriod_" + i  %>>
                              <%=Html.TextBox("Managers[" + i + "].ValidFrom", manager.ValidFrom, new { @style = "width:90px", @class = "date_start" })%>
                                    -
                              <%=Html.TextBox("Managers[" + i + "].ValidTo", manager.ValidTo, new { @style = "width:90px", @class = "date_end" })%>
                          </div>
                      </td>
                      <td style="width:15%; padding:2px; vertical-align: top;">
                        <span class="ui-icon ui-icon-closethick" style="cursor:pointer;" onclick="javascript:DeleteManager(<%= manager.UserId %>, <%= Model.Department.Id %>);"></span>
                      </td>
                  </tr>
            </table>
          </div>
          <%i++;%>
    <%}%>
  <div id = <%="managerAddTable_" + i%>>
    <table width="100%" cellpadding="3" cellspacing="3" style="border-bottom: 1px solid #CCCCCC; margin: 0; padding: 3px; border-spacing: 3px;">
        <tr>
            <td style="width:40%; padding:2px; vertical-align: top; font-size:13px;"><%= Html.DropDownList("Managers[" + i + "].UserId", new SelectList(Model.Department.DepartmentManagersList, "Key", "Value"), ViewResources.SharedStrings.DefaultDropDownValue, new { @id = "departmentManager_" + i, @style = "width:100%;display:none", onchange = "javascript:SelectManager('" + i + "','" + Model.Department.Id + "');" })%></td>
            <td style="width:45%; padding:2px; vertical-align: top;">
 
                      <div id=<%="departmentManagerValidationPeriod_" + i  %> style = "display:none">
                          <%=Html.TextBox("Managers[" + i + "].ValidFrom", DateTime.Now.ToString("dd.MM.yyyy"), new { @style = "width:90px", @class = "date_start" })%>
                                -
                          <%=Html.TextBox("Managers[" + i + "].ValidTo", DateTime.Now.ToString("dd.MM.yyyy"), new { @style = "width:90px", @class = "date_end" })%>
                      </div>
            </td>
            <td style="width:15%; padding:2px; vertical-align: top;">
              <span id=<%="addManager_" + i %> class="ui-icon ui-icon-circle-plus" style="cursor:pointer;" onclick="javascript:AddManager(<%= i %>, <%= Model.Department.Id %>);"></span>
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
