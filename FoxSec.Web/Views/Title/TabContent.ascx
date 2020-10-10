<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.HomeViewModel>" %>
<div id='tab_titles'>
<div style='margin:0 0 10px 0; text-align:right;'><input type='button' id='button_add_title' value='<%=ViewResources.SharedStrings.BtnAddNewTitle %>' onclick="javascript:AddTitle('submit_add_title','<%=ViewResources.SharedStrings.BtnAddNewTitle %>');" /></div>
<div id="TitlesList"></div>

<script type="text/javascript" language="javascript">

    function AddTitle(Action, Message) {
        $("div#modal-dialog").dialog({
            open: function () {
                $("div#modal-dialog").html("");
                $.get('/Title/Create', function (html) {
                    $("div#modal-dialog").html(html);
                });
            },
            resizable: false,
            width: 480,
            height: 340,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + Message,
            buttons: {
                '<%=ViewResources.SharedStrings.BtnOk %>': function () {
                    dlg = $(this);
                	$.ajax({
                		type: "Post",
                		url: "/Title/Create",
                		dataType: 'json',
                		traditional: true,
                		data: $("#createNewTitle").serialize(),
                		success: function (data) {
                			if (data.IsSucceed == false) {
                				$("div#modal-dialog").html(data.viewData);
                				if (data.DisplayMessage == true) {
                					ShowDialog(data.Msg, 2000);
                				}
                			}
                			else {
                				ShowDialog(data.Msg, 2000, true);
                				$("div#modal-dialog").html("");
                				dlg.dialog("close");
                				setTimeout(function () {
                					$.get('/Title/List', function (html) {
                						$("div#TitlesList").html(html);
                					});
                				}, 1000);
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
    	var i = $('#panel_owner_tab_administration li').index($('#titleTab'));
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
    	$.get('/Title/List', function (html) {
    		$("div#TitlesList").html(html);
    	});
    });

</script>
</div>