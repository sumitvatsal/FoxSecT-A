<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.HomeViewModel>" %>
<div id='tab_classifiers'>
<div style='margin:0 0 10px 0; text-align:right;'><input type='button' id='button_add_classifier' value='<%=ViewResources.SharedStrings.ClassifiersAddNew %>' onclick="javascript:AddClassifier('submit_add_classifier','<%=ViewResources.SharedStrings.ClassifiersAddNew %>');" /></div>
<div id="ClassifiersList"></div>

<script type="text/javascript" lang="javascript">

    function AddClassifier(Action, Message) {
    	$("div#modal-dialog").dialog({
    		open: function () {
    			$("div#modal-dialog").html("");
    			$.get('/Classifiers/Create', function (html) {
    				$("div#modal-dialog").html(html);
    			});
    		},
    		resizable: false,
    		width: 640,
    		height: 280,
    		modal: true,
    		title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + Message,
    		buttons: {
    			'<%=ViewResources.SharedStrings.BtnSave %>': function () {
    				dlg = $(this);

    				$.ajax({
    					type: "Post",
    					url: "/Classifiers/Create",
    					dataType: 'json',
    					traditional: true,
    					data: $("#createNewClassificator").serialize(),
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
    								$.get('/Classifiers/List', function (html) {
    									$("div#ClassifiersList").html(html);
    								});
    							}, 1000);
    						}
    					}
    				});
    			},
    			'<%=ViewResources.SharedStrings.ClassifiersBackToList %>': function () {
    				$(this).dialog("close");
    			}
    		}
    	});
        return false;
    }

    $(document).ready(function () {
    	var i = $('#panel_owner_tab_administration li').index($('#classifierTab'));
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
        $.get('/Classifiers/List', function (html) {
            $("div#ClassifiersList").html(html);
        });
    });

</script>
</div>