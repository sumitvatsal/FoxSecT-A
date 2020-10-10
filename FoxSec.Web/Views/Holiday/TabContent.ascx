<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.HomeViewModel>" %>
<div id='tab_holidays'>
<div style='margin:0 0 10px 0; text-align:right;'><input type='button' id='button_add_holiday' value='<%=ViewResources.SharedStrings.HolidaysBtnAddNew %>' onclick="javascript:AddHoliday('submit_add_holiday','<%=ViewResources.SharedStrings.HolidaysBtnAddNew %>');" /></div>
<div id="HolidaysList"></div>

<script type="text/javascript" language="javascript">

    function AddHoliday(Action, Message) {
        $("div#modal-dialog").dialog({
            open: function () {
                $("div#modal-dialog").html("");
                $.get('/Holiday/Create', function (html) {
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
                	$.ajax({
                		type: "Post",
                		url: "/Holiday/Create",
                		dataType: 'json',
                		traditional: true,
                		data: $("#createNewHoliday").serialize(),
                		success: function (data) {
                			if (data.IsSucceed == false) {
                				$("div#modal-dialog").html(data.viewData);
                				if (data.DisplayMessage == true) {
                					ShowDialog(data.Msg, 2000);
                				}
                			}
                			else {
                				ShowDialog(data.Msg, 2000, true);
                				setTimeout(function () {
                					$.ajax({
                						type: 'GET',
                						url: '/Holiday/List',
                						cache: false,
                						success: function (html) {
                							$("div#HolidaysList").html(html);
                						}
                					});
								}, 1000);
                				dlg.dialog("close");
                			}
                		}
                	});
                },
                '<%=ViewResources.SharedStrings.BtnCancel %>': function () {
                    $(this).dialog("close");
                }
            }
        });
        return false;
    }

    $(document).ready(function () {
    	var i = $('#panel_owner_tab_administration li').index($('#holidayTab'));
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
        $.get('/Holiday/List', function (html) {
            $("div#HolidaysList").html(html);
        });
    });

</script>
</div>