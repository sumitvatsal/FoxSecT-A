<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.HomeViewModel>" %>
<div id='tab_card_types'>
<div style='margin:0 0 10px 0; text-align:right;'><!--<input type='button' id='button_add_card_type' value='<%=ViewResources.SharedStrings.CardTypesAddNew %>' onclick="javascript:AddCardType('submit_add_card_type','<%=ViewResources.SharedStrings.CardTypesAddNew %>');" />--></div>
<div id="CardTypesList"></div>

<script type="text/javascript" language="javascript">

    function AddCardType(Action, Message) {
        $("div#modal-dialog").dialog({
            open: function () {
                $("div#modal-dialog").html("");
                $.get('/Card/NewType', function (html) { $("div#modal-dialog").html(html); });
            },
            resizable: false,
            width: 640,
            height: 320,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + Message,
            buttons: {
                '<%=ViewResources.SharedStrings.BtnSave %>': function () {
                	dlg = $(this);
                	$.ajax({
                		type: "Post",
                		url: "/Card/CreateNewType",
                		dataType: 'json',
                		traditional: true,
                		data: $("#createNewCardType").serialize(),
                		success: function (data) {
                			if (data.IsSucceed == false) {
                				$("div#modal-dialog").html(data.viewData);
                				if (data.DisplayMessage == true) {
                					ShowDialog(data.Msg, 2000);
                				}
                			}
                			else {
                				ShowDialog(data.Msg, 2000, true);
                				setTimeout(function () { $.get('/Card/TypeList', function (html) { $("div#CardTypesList").html(html); }); }, 1000);
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
    	var i = $('#panel_owner_tab_administration li').index($('#cardTypeTab'));
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
        $.get('/Card/TypeList', function (html) {
            $("div#CardTypesList").html(html);
        });
    });

</script>
</div>