<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.HomeViewModel>" %>
<table cellpadding="0" cellspacing="1" style="margin: 0; width: 100%; padding: 0; border-spacing: 1px;">
	<tr>
		<td style='width: 250px; vertical-align: top; text-align: center'><div id='AreaBuildingTree' style='margin: 15px 15px 15px 0; padding: 10px; min-width: 220px; text-align: left'></div>
		</td>
		<td style='vertical-align: top;'>
			<label style="padding-top:20px" id="comentedLabelsCount"></label>
			<div id="BuildingInfo">
			</div>
		</td>
	</tr>
	<tr>
		<td>
			<table style="margin: 0px">
				<tr>
					<td><%:ViewResources.SharedStrings.BuildingsLegend %>:
						<table>
							<tr>
								<td style="background-color: green">&nbsp;&nbsp;&nbsp;&nbsp</td>
								<td> &nbsp; - &nbsp;<%:ViewResources.SharedStrings.BuildingsCommentedBuildings %></td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>


<script type="text/javascript" language="javascript">

    $(document).ready(function () {
        //By Manoranjan
        debugger;
       //var userSelection = window.confirm("Do you want to save new EDLUS certificate?");
       // if (userSelection) {
       //     $("#modal").css("display", "block");
           
       // }

        //

    	var i = $('#panel_owner_tab_administration li').index($('#buildingTab'));
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
        SetNoSelectedGUI();
    });
    

    function GetCommentsCount() {
       	var cnt = 0;
       	$('ul#buildings').find('li span').each(function () {
       		if ($(this).attr('original-title') != undefined) {
       			var str = new String($(this).attr('original-title'));
       			if (str.length > 0) {
       				cnt++;
       			}
       		}
       	});

		$('#comentedLabelsCount').html('<%:ViewResources.SharedStrings.BuildingsActiveCommentsCount %>' + cnt);
    }

    function SetNoSelectedGUI() {
        $.get('/Building/GetTree', function (html) {
            $("div#AreaBuildingTree").html(html);
            $("div#AreaBuildingTree").attr("id", function () {
                $(this).corner("bevelfold");
            });
            $("ul#buildings").treeview({
                animated: "fast",
                persist: "location",
                collapsed: true,
                unique: true
            });
        });
        return false;
    }

    function GetBuildingInfo(building_id,certificate_Value) {
        debugger;
        $.get('/Building/BuildingInfo', { buildingId: building_id, certificateValue: certificate_Value}, function(html) {
            $("div#BuildingInfo").html(html);
        });
    }

    function EditBuilding(cntr, Action, buildingId, buildingDescription) {
        $("div#modal-dialog").dialog({
            open: function () {
                $("div#modal-dialog").html("");
                $.get('/Building/Edit', { id: buildingId }, function(html) {
                    $("div#modal-dialog").html(html);
                });
            },
            resizable: false,
            width: 640,
            height: 240,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + buildingDescription,
            buttons: {
                '<%: ViewResources.SharedStrings.BtnSave %>': function () {
                    var dlg = $(this);
                    $.ajax({
                        type: "POST",
                        url: "/Building/Edit",
                        dataType: "json",
                        data: $("#editBuilding").serialize(),
                        success: function (response) {
                            if (response.IsSucceed == true) {
                                if (response.IsEmpty) {
                                    $(cntr).css('color', '#222222');
                                    $(cntr).parent().unbind('mouseenter mouseleave');
                                    $(cntr).parent().attr('original-title', "");
                                }
                                else {
                                    $(cntr).css('color', "Green");
                                    $(cntr).parent().addClass('tipsy_we');
                                    $(cntr).parent().attr("class", function () {
                                        $(this).tipsy({ gravity: $.fn.tipsy.autoWE, html: true });
                                    });
                                    $(cntr).parent().attr('original-title', response.Comment);
                                }
                                dlg.dialog("close");
                                ShowDialog(response.Msg, 2000, true);
                                GetCommentsCount();
                            }
                            else {
                                $("div#modal-dialog").html(response.viewData);
                            }
                        },
                        error: function () {}
                    });
                },
                '<%: ViewResources.SharedStrings.BtnCancel %>': function () {
                    $(this).dialog("close");
                }
            }
        });
        return false;
    }

</script>