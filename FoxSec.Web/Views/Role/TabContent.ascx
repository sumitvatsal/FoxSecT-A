<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.HomeViewModel>" %>
<div id='tab_roles'>
	<table cellpadding='0' cellspacing='0' style='margin: 0; width: 100%; padding: 0; border-spacing: 0; border: none;'>
		<tr>
			<td style='width: 10px;'></td>
			<td><%:ViewResources.SharedStrings.RolesRoleStatus %>: 
				<select id='RolesFilter' onchange = "javascript:ActiveChanged()">
					<option value="1"><%:ViewResources.SharedStrings.FilterActive %></option>
					<option value="0"><%:ViewResources.SharedStrings.FilterDeactivated %></option>
				</select>
			</td>
			<td style='text-align:right'>
				<div style='margin:0 0 10px 0; text-align:right;' class="footer"><input type='button' id='button_add_role' value='<%=ViewResources.SharedStrings.BtnAddNewRole %>' onclick="javascript:AddRole('submit_add_role','<%=ViewResources.SharedStrings.BtnAddNewRole %>');" />
				</div>
			</td>
		</tr>
	</table>

<div id='AreaTabRoleSearchResultsWait' style='display: none; width: 100%; height:378px; text-align:center'><span style='position:relative; top:35%' class='icon loader'></span></div>
<div id="RoleList"></div>
    <script type="text/javascript" language="javascript">

	var r_page = 0;
	var r_rows = 10;
	var r_field = 0;
	var r_direction = 0; 
	var r_active = 1;

    function AddRole(Action, Message) {
    	$("div#modal-dialog").dialog({
    		open: function () {
    			$("div#modal-dialog").html("");
    			$.get('/Role/Create', function (html) {
    				$("div#modal-dialog").html(html);
    			});
    		},
    		resizable: false,
    		width: 640,
    		height: 480,
    		modal: true,
    		title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + Message,
    		buttons: {
    			'<%=ViewResources.SharedStrings.BtnOk %>': function () {
    				dlg = $(this);
    				$.post("/Role/Create", $("#createNewRole").serialize(),
					function (response) {
						if (response.IsSucceed) {
							msg = response.Msg;
							dlg.dialog("close");
							if (response.IsActive) {
								r_active = 1;
								$('#RolesFilter option:first').attr('selected', 'selected');
							}
							else {
								r_active = 0;
								$('#RolesFilter option:last').attr('selected', 'selected');
							}
							ShowDialog(response.Msg, 2000, true);
							setTimeout(function () { SubmitRolesSearch(); }, 1000);
						}
						else {
							$("div#modal-dialog").html(response.viewData);
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

	$(document).ready(function () {
		var i = $('#panel_owner_tab_administration li').index($('#roleManagmentTab'));

		var opened_tab = '<%: Session["subTabIndex"] %>';

		if (opened_tab != '') {
			i1 = new Number(opened_tab);
			if (i1 != i) {
				SetOpenedSubTab(i);
			}
		}
		else {
			SetOpenedSubTab(i);
		}
		
		
		$("input:button").button();
		SubmitRolesSearch();
	});

	function HandleRolesPaging(page, rows) {
		r_page = page;
		r_rows = rows;
		SubmitRolesSearch();
		return false;
	}
	function HandleRolesSoring(page, rows, field, direction) {
		r_page = page;
		r_rows = rows;
		r_field = field;
		r_direction = direction;
		SubmitRolesSearch();
		return false;
	}

	function ActiveChanged() {
		r_active = $("select#RolesFilter").val();
		SubmitRolesSearch();
	}
	function SubmitRolesSearch() {
		
//		$('input#button_delete_company').fadeOut();
//		$('input#button_activate_company').fadeOut();
//		$('input#button_deactivate_company').fadeOut();

		$.ajax({
			type: "Post",
			url: "/Role/List",
			data: {filter:r_active, nav_page: r_page, rows: r_rows, sort_field: r_field, sort_direction: r_direction },
			beforeSend: function () {
				//$("#button_submit_company_search").addClass("Trans");
				$("div#RoleList").fadeOut('fast', function () { $("div#AreaTabRolesSearchResultsWait").fadeIn('fast'); });
			},
			success: function (result) {
			    
				$("div#AreaTabRolesSearchResultsWait").hide();
				$("div#RoleList").html(result);
				$("div#RoleList").fadeIn('fast');
				//$("#button_submit_company_search").removeClass("Trans");
			}
		});
		return false;
	}




</script>
</div>