<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.RoleListViewModel>" %>
<%if(Model.Roles.Count() == 0){%>
	<%=Html.Encode(ViewResources.SharedStrings.CommonNoRecordsFound) %>
<%}else{ %>
	<table cellpadding="1" cellspacing="0" style="margin:0; width:100%; padding:1px; border-spacing:0;">
	<thead>
		<tr>
			<th style='width:15%; padding:2px;'><%:ViewResources.SharedStrings.UsersName %>
				<li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:RolesSort(0, 0);'></span></li>
				<li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:RolesSort(0, 1);'></span></li>
			</th>
			<th style='width:15%; padding:2px;'><%:ViewResources.SharedStrings.RoleType %>
				<li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:RolesSort(1, 0);'></span></li>
				<li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:RolesSort(1, 1);'></span></li>
			</th>
			<th style='width:35%; padding:2px;'><%:ViewResources.SharedStrings.CommonDescription %>
				<li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:RolesSort(2, 0);'></span></li>
				<li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:RolesSort(2, 1);'></span></li>
			</th>
			<th style='width:25%; padding:2px;'><%:ViewResources.SharedStrings.BuildingsTabName %>
				<li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:RolesSort(3, 0);'></span></li>
				<li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:RolesSort(3, 1);'></span></li>
			</th>
			<th style='width:5%; padding:2px;'></th>
			<th style='width:5%; padding:2px;'></th>
		</tr>
	</thead>
	<tbody>
	<% var i = 1; foreach (var role in Model.Roles) { var bg = (i++ % 2 == 1) ? " background-color:#CCC;" : ""; %>
		<tr>
			<td style='<% if (!role.Active) {%>text-decoration:line-through; <%}%>width:15%; padding:2px;<%= bg %>'>
				<%= Html.Encode(role.Name != null && role.Name.Length > 30 ? role.Name.Substring(0, 27) + "..." : role.Name)%> 
			</td>
			<td style='<% if (!role.Active) {%>text-decoration:line-through; <%}%>width:15%; padding:2px;<%= bg %>'>
				<%= Html.Encode(role.RoleTypeId != null && role.RoleTypeId.ToString().Length > 30 ? role.RoleTypeId.ToString().Substring(0, 27) + "..." : role.RoleTypeId.ToString())%> 
			</td>
			<td style='<% if (!role.Active) {%>text-decoration:line-through; <%}%>width:35%; padding:2px;<%= bg %>'>
				<%= Html.Encode(role.DescriptionShort) %>
			</td>
			<td style='<% if (!role.Active) {%>text-decoration:line-through; <%}%>width:25%; padding:2px;<%= bg %>'>
				<%= Html.Encode(role.BuildingsNames) %> 
			</td>
			<td style='width:5%; padding:2px; text-align:right;<%= bg %>'>
				<%if((Model.User.RoleId != role.Id && !role.IsReadOnly) || Model.User.IsSuperAdmin) { %>
					<span id='button_role_edit_<%=role.Id %>' class='icon icon_green_go tipsy_we' original-title='<%=string.Format("{0} {1}!", ViewResources.SharedStrings.BtnEdit, Html.Encode(role.Name)) %>' onclick='<%=string.Format("javascript:EditRole( {0}, \"{1}\")", role.Id, Html.Encode(role.Name)) %>' ></span>
				<%} else{%>
					<span id='Span2' class='icon icon_green_go tipsy_we' original-title='<%=string.Format("{0} {1}!", ViewResources.SharedStrings.CommonBtnView, Html.Encode(role.Name)) %>' onclick='<%=string.Format("javascript:ViewRole( {0}, \"{1}\")", role.Id, Html.Encode(role.Name)) %>' ></span>
				<%} %>
			</td>
			<td style='width:5%; padding:2px; text-align:right;<%= bg %>'>
				<%if((Model.User.RoleId != role.Id && !role.IsReadOnly) || Model.User.IsSuperAdmin){ %>
					<span id='button_role_delete_<%= role.Id %>' class="ui-icon ui-icon-closethick tipsy_we" style="cursor:pointer" original-title='<%=string.Format("{0} {1}!", ViewResources.SharedStrings.BtnDelete, Html.Encode(role.Name)) %>' onclick='<%=string.Format("javascript:DeleteRole({0}, \"{1}\")", role.Id, Html.Encode(role.Name)) %>'></span>
				<%} %>
			</td>
		</tr>
	<% } %>
	</tbody>
	<tfoot>
		<tr>
			<td colspan="6">
				<% Html.RenderPartial("Paginator", Model.Paginator); %>
			</td>
		</tr>
	</tfoot>
	</table>
<% }%>

<script type="text/javascript" language="javascript">
	$(document).ready(function () {
		$(".tipsy_we").attr("class", function () {
			$(this).tipsy({ gravity: $.fn.tipsy.autoWE, html: true });
		});
	});

    function ViewRole(RoleId, RoleTitle) {
        $("div#modal-dialog").dialog({
            open: function () {
                $("div#modal-dialog").html("");
                $.get('/Role/Edit', { id: RoleId }, function(html) {
                    $("div#modal-dialog").html(html);
                });
            },
            resizable: false,
            width: 640,
            height: 740,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + RoleTitle,
            buttons: {
                '<%=ViewResources.SharedStrings.BtnClose %>': function () {
                    $(this).dialog("close");
                }
            }
        });
        return false;
    }

    function EditRole(RoleId, RoleTitle)
    {
        $("div#modal-dialog").dialog({
            open: function () {
                $("div#modal-dialog").html("");
                $.get('/Role/Edit', { id: RoleId }, function (html) {
                    $("div#modal-dialog").html(html);
                });
            },
            resizable: false,
            width: 640,
            height: 740,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + RoleTitle,
            buttons: {
                '<%=ViewResources.SharedStrings.BtnSave %>': function () {
                    dlg = $(this);
                    NewRoleTitle = $("input#Edit_role_title_" + RoleId).val();
                    $.post("/Role/Edit", $("#editRole").serialize(),
					    function (response) {				       
					      
					        if (response.IsSucceed) {
					            dlg.dialog("close");
					            ShowDialog("Role " + NewRoleTitle + " saving..", 2000, true);

					            if (response.IsActive) {
					                r_active = 1;
					                $('#RolesFilter option:first').attr('selected', 'selected');
					            }
					            else {
					                r_active = 0;
					                $('#RolesFilter option:last').attr('selected', 'selected');
					            }
					            setTimeout(function () { SubmitRolesSearch(); }, 1000);
					        }
					        else {
					            $("div#modal-dialog").html(response.viewData);
					            if (response.DisplayError) {
					                ShowDialog(response.Msg, 2000);
					            }
					        }
					    },
					    "json");
                },
                '<%=ViewResources.SharedStrings.BtnCancel %>': function () {
                    $(this).dialog("close");
                }
            }
        });
	    return false;
    }

    function DeleteRole(roleId, roleTitle) {
	    $("div#modal-dialog").dialog({
		    open: function (event, ui) {
			    $("div#modal-dialog").html('<%=ViewResources.SharedStrings.CommonConfirmMessage %>');
		    },
		    resizable: false,
		    width: 240,
		    height: 140,
		    modal: true,
		    title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span> Deleting " + roleTitle,
		    buttons: {
			    '<%=ViewResources.SharedStrings.BtnOk %>': function () {
				    dlg = $(this);
				    $.post("/Role/Delete", { id: roleId },
						    function (response) {
							    if (response.IsSucceed) {
								    ShowDialog('<%=ViewResources.SharedStrings.RolesDeletingMessage %>' + roleTitle + "...", 2000, true);
								    setTimeout(function () { SubmitRolesSearch(); }, 1000);
								    dlg.dialog("close");
							    } else {
								    ShowDialog(response.Msg, 2000);
								    dlg.dialog("close");
							    }
						    },"json");
			    },
			    '<%=ViewResources.SharedStrings.BtnCancel %>': function () {
				    $(this).dialog("close");
			    }
		    }
	    });
    }

</script>