<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.HomeViewModel>" %>
<div id='tab_departments'>
<div id='dep_buttons' style='margin:0 0 10px 0; text-align:right;'>
    <input type='button' id='button_delete_department' value='<%=ViewResources.SharedStrings.DepartmentsBtnDelete %>' onclick="javascript:DeleteDepartment();" style='display:none'/>
    <input type='button' id='button_add_department' value='<%=ViewResources.SharedStrings.DepartmentsBtnAddNew %>' onclick="javascript:AddDepartment('submit_add_department','<%=ViewResources.SharedStrings.DepartmentsBtnAddNew %>');" />
</div>
<div id="DepartmentsList"></div>

<script type="text/javascript" language="javascript">

    function AddDepartment(Action, Message) {
        $("div#modal-dialog").dialog({
            open: function () {
                $("div#modal-dialog").html("");
                $.get('/Department/Create', function (html) {
                    $("div#modal-dialog").html(html);
                });
            },
            resizable: false,
            width: 600,
            height: 340,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + Message,
            buttons: {
                '<%=ViewResources.SharedStrings.BtnOk %>': function () {
					dlg = $(this);
                    $.ajax({
                    	type: "Post",
                    	url: "/Department/Create",
                    	dataType: 'json',
                    	traditional: true,
                    	data: $("#createNewDepartment").serialize(),
                    	success: function (data) {
                    		if (data.IsSucceed == false) {
                    			$("div#modal-dialog").html(data.viewData);
                    			if (data.Msg != "") {
                    				ShowDialog(data.Msg, 2000);
                    			}
                    		}
                    		else {
                    			ShowDialog(data.Msg, 2000, true);
                    			setTimeout(function () {
                    			    SubmitDepartmentSearch();
                    			}, 1000);
                    			GetDepartments();
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

    var d_page = 0;
    var d_rows = 10;
    var d_field = 0;
    var d_direction = 0;

    function HandleDepartmentsPaging(page, rows) {
        d_page = page;
        d_rows = rows;
        SubmitDepartmentSearch();
        return false;
    }
    function HandleDepartmentsSoring(page, rows, field, direction) {
        d_page = page;
        d_rows = rows;
        d_field = field;
        d_direction = direction;
        SubmitDepartmentSearch();
        return false;
    }
    function SubmitDepartmentSearch() {
        $.ajax({
            type: "Get",
            url: "/Department/List",
            data: { nav_page: d_page, rows: d_rows, sort_field: d_field, sort_direction: d_direction },
            success: function (result) {
                $("div#DepartmentsList").html(result);
                $("div#DepartmentsList").fadeIn('slow');
            }
        });
        return false;
       }


    $(document).ready(function () {
    	var i = $('#panel_owner_tab_administration li').index($('#departmentTab'));
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
        SubmitDepartmentSearch();
    });

</script>
</div>