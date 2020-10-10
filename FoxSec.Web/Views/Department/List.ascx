<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.DepartmentListViewModel>" %>

<table id="searchedTableDep" width="100%" style="margin:0; width:100%; padding:2px; border-spacing:0;">
<thead>
<tr>
    <th style='width: 30px; padding: 2px;'>
        <input id='check_all_dep' name='check_all_dep' type='checkbox' class='tipsy_we' original-title='Select all!' onclick="javascript:CheckAll();"/>
    </th>
    <th style='width:25%; padding:2px; vertical-align:middle'>
       <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:DepartmentsSort(0, 0);'></span></li>
       <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:DepartmentsSort(0, 1);'></span></li> 
       <%:ViewResources.SharedStrings.CommonNumber %>
    </th>
	<th style='width:35%; padding:2px;'>       
       <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:DepartmentsSort(1, 0);'></span></li>
       <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:DepartmentsSort(1, 1);'></span></li> 
       <%:ViewResources.SharedStrings.UsersName %>
    </th>
    <th style='width:35%; padding:2px;'>       
       <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:DepartmentsSort(2, 0);'></span></li>
       <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:DepartmentsSort(2, 1);'></span></li> 
       <%:ViewResources.SharedStrings.DepartmentsManager %>
    </th>
    <th style='width:5%; padding:2px;'></th>
</tr>
</thead>
<tbody>
<% var i = 1; foreach (var department in Model.Departments)
   {
       var bg = (i++ % 2 == 1) ? " background-color:#CCC;" : "";%>
<tr>
    <td style='width:30px; padding: 2px;<%= bg %>'>
          <input type='checkbox' id='<%= department.Id %>' name='dep_checkbox' onclick='javascript:ManageButtons();' />
    </td>
    <td style='width:25%; padding:2px;<%= bg %>'><%= Html.Encode(department.Number != null && department.Number.Length > 30 ? department.Number.Substring(0, 27) + "..." : department.Number)%></td>
    <td style='width:35%; padding:2px;<%= bg %>'><%= Html.Encode(department.Name != null && department.Name.Length > 50 ? department.Name.Substring(0, 47) + "..." : department.Name)%> </td>
    <td style='width:35%; padding:2px;<%= bg %>'><%= Html.Encode(department.Manger)%></td>
    <td style='width:5%; padding:2px; text-align:right;<%= bg %>'><span id='button_department_edit_<%= department.Id %>' class='icon icon_green_go tipsy_we' original-title='<%=string.Format("{0} {1}!", ViewResources.SharedStrings.BtnEdit, Html.Encode(department.Name)) %>' onclick='<%=string.Format("javascript:EditDepartment({0}, \"{1}\")", department.Id, Html.Encode(department.Name)) %>' ></span></td>
</tr>
<% } %>
 </tbody>
 <tfoot>
     <tr>
         <td colspan="4">
            <% Html.RenderPartial("Paginator", Model.Paginator); %>
         </td>
     </tr>
 </tfoot>
</table>

<script type="text/javascript" language="javascript">

    $(document).ready(function () {
        $('input#button_delete_department').fadeOut();
        $(".tipsy_we").attr("class", function () {
            $(this).tipsy({ gravity: $.fn.tipsy.autoWE, html: true });
        });
    });

    function EditDepartment(DepartmentId, DepartmentTitle) {
        $("div#modal-dialog").dialog({
            open: function () {
                $("div#modal-dialog").html("");
                $.get('/Department/Edit', { departmentId: DepartmentId }, function(html) {
                    $("div#modal-dialog").html(html);
                });
            },
            resizable: false,
            width: 640,
            height: 480,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + DepartmentTitle,
            buttons: {
                '<%=ViewResources.SharedStrings.BtnSave %>': function () {
                    NewDepartmentTitle = $("input#Edit_department_title_" + DepartmentId).val();
                    dlg = $(this);
                    $.ajax({
                        type: "Post",
                        url: "/Department/Edit",
                        dataType: 'json',
                        traditional: true,
                        data: $("#editDepartmentTable").serialize(),
                        success: function (data) {
                            if (data.IsSucceed == false) {
                                $("div#modal-dialog").html(data.viewData);
                                if (data.Msg != "") {
                                    ShowDialog(data.Msg, 2000);
                                }
                            }
                            else {
                                ShowDialog(data.Msg, 2000, true);
                                SubmitDepartmentSearch();
                                $("div#modal-dialog").html("");
                                dlg.dialog("close");
                            }
                        }
                    });
                },
                '<%=ViewResources.SharedStrings.BtnCancel %>': function () {
                    $("div#modal-dialog").html("");
                    $(this).dialog("close");
                }
            }
        });

        return false;
    }

    function DeleteDepartment() {
        var departmentsIds = new Array();
        $("#button_delete_department").addClass("Trans");

        $('input[name=dep_checkbox]').each(function () {
            if (this.checked) {
                var departmentId = $(this).attr('id');
                departmentsIds.push(departmentId);
            }
        });

        $("div#modal-dialog").dialog({
            open: function (event, ui) {
            	$("div#modal-dialog").html('<%=ViewResources.SharedStrings.CommonConfirmMessage %>');
            },
            resizable: false,
            width: 240,
            height: 140,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>Deleting departments",
            buttons: {
            	'<%=ViewResources.SharedStrings.BtnOk %>': function () {
            		$.ajax({
                        type: "Post",
                        url: "/Department/Delete",
                        data: { departmentsIds: departmentsIds },
                        traditional: true,
                        success: function (result) {
                            $("#button_delete_department").removeClass("Trans");
                            setTimeout(function () { SubmitDepartmentSearch(); }, 1000);
                        	if (result.IsSucceed) {
                        		ShowDialog('<%=ViewResources.SharedStrings.DepartmentsDeletingMessage %>', 2000, true);
                        	}
                            else {
                        		ShowDialog(result.Msg, 5000);
                        	}
                        }
                    });
                    $(this).dialog("close");
                },
              '<%=ViewResources.SharedStrings.BtnCancel %>': function () { $(this).dialog("close"); $("#button_delete_department").removeClass("Trans"); }
            }
        });
        return false;
    }

    function CheckAll() {
        $('input[name=dep_checkbox]').each(function () {
            this.checked = !this.checked;
        });
        ManageButtons();
        return false;
    }

    function ManageButtons() {
        var any = 0;

        $('input[name=dep_checkbox]').each(function () {
            if (this.checked) any++;
        });

        if (any != 0) {
            $('input#button_delete_department').fadeIn();
        }
        else {
            $('input#button_delete_department').fadeOut();
            $('input#check_all_dep').attr('checked', false);
        }
        return false;
    }

    function SelectManager(userId, depId) {
        $.get('/Department/GetValidFromForUser', { userId: $("#departmentManager_" + userId).val(), departmentId: depId }, function (html) {
            $("input#Managers_" + userId + "__ValidFrom").val(html);
        });
        $.get('/Department/GetValidToForUser', { userId: $("#departmentManager_" + userId).val(), departmentId: depId }, function (html) {
            $("input#Managers_" + userId + "__ValidTo").val(html);
        });

        if ($("#departmentManager_" + userId).val()) {
            $("span#addManager_" + userId).show();
            $("div#departmentManagerValidationPeriod_" + userId).show();
        }
        else {
            $("span#addManager_" + userId).hide();
            $("div#departmentManagerValidationPeriod_" + userId).hide();
        }
        return false;
    };

    function DeleteManager(userId, depId) {
        $.post('/Department/DeleteManager', { userId: userId, departmentId: depId });

        setTimeout(function () {
            $.get('/Department/ManagerList', { departmentId: depId }, function (html) {
                $("div#departmentManagerList").html(html);
            });
        }, 1000);
        return false;
    };

    function AddManager(id, depId) {
        var nid = id + 1;
        $("div#departmentManagerList").append($("<div id='managerRow_" + nid + "'></div>"));
        $.get('/Department/NextManager', { id: nid, departmentId: depId }, function (html) {
            $("div#managerRow_" + nid).html(html);
            $("span#addManager_" + id).remove();
            $("span#deleteManager_" + id).fadeIn(300);
            $("div#managerAddTable_" + id).fadeOut(300);
        });
        return false;
    };

</script>