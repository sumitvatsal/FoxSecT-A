<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.HomeViewModel>" %>
<style type="text/css">
		/* css for timepicker */
		#ui-timepicker-div dl{ text-align: left; }
		#ui-timepicker-div dl dt{ height: 25px; }
		#ui-timepicker-div dl dd{ margin: -25px 0 10px 65px; }
		dl{margin-right:7px;}
		.ui-datepicker { width: 17em; padding: .2em .2em 0; }

.ui-corner-tl { -moz-border-radius-topleft: 4px; -webkit-border-top-left-radius: 4px; border-top-left-radius: 4px; }
.ui-corner-tr { -moz-border-radius-topright: 4px; -webkit-border-top-right-radius: 4px; border-top-right-radius: 4px; }
.ui-corner-bl { -moz-border-radius-bottomleft: 4px; -webkit-border-bottom-left-radius: 4px; border-bottom-left-radius: 4px; }
.ui-corner-br { -moz-border-radius-bottomright: 4px; -webkit-border-bottom-right-radius: 4px; border-bottom-right-radius: 4px; }
.ui-corner-top { -moz-border-radius-topleft: 4px; -webkit-border-top-left-radius: 4px; border-top-left-radius: 4px; -moz-border-radius-topright: 4px; -webkit-border-top-right-radius: 4px; border-top-right-radius: 4px; }
.ui-corner-bottom { -moz-border-radius-bottomleft: 4px; -webkit-border-bottom-left-radius: 4px; border-bottom-left-radius: 4px; -moz-border-radius-bottomright: 4px; -webkit-border-bottom-right-radius: 4px; border-bottom-right-radius: 4px; }
.ui-corner-right {  -moz-border-radius-topright: 4px; -webkit-border-top-right-radius: 4px; border-top-right-radius: 4px; -moz-border-radius-bottomright: 4px; -webkit-border-bottom-right-radius: 4px; border-bottom-right-radius: 4px; }
.ui-corner-left { -moz-border-radius-topleft: 4px; -webkit-border-top-left-radius: 4px; border-top-left-radius: 4px; -moz-border-radius-bottomleft: 4px; -webkit-border-bottom-left-radius: 4px; border-bottom-left-radius: 4px; }
.ui-corner-all { -moz-border-radius: 4px; -webkit-border-radius: 4px; border-radius: 4px; }


.ui-slider { position: relative; text-align: left;  }
.ui-slider .ui-slider-handle { position: absolute; z-index: 2; width: 1.2em; height: 1.2em; cursor: default; }
.ui-slider .ui-slider-range { position: absolute; z-index: 1; font-size: .7em; display: block; border: 0; background-position: 0 0; }

.ui-slider-horizontal { height: .8em; }
.ui-slider-horizontal .ui-slider-handle { top: -.3em; margin-left: -.6em; }
.ui-slider-horizontal .ui-slider-range { top: 0; height: 100%; }
.ui-slider-horizontal .ui-slider-range-min { left: 0; }
.ui-slider-horizontal .ui-slider-range-max { right: 0; }

.ui-slider-vertical { width: .8em; height: 100px; }
.ui-slider-vertical .ui-slider-handle { left: -.3em; margin-left: 0; margin-bottom: -.6em; }
.ui-slider-vertical .ui-slider-range { left: 0; width: 100%; }
.ui-slider-vertical .ui-slider-range-min { bottom: 0; }
.ui-slider-vertical .ui-slider-range-max { top: 0; }
</style>
<div id='LocContent'>
<form id="locFilters" action="">
<table cellpadding="0" cellspacing="1" style="margin: 0; width: 100%; padding: 0; border-spacing: 1px;">
<tr>
	<td style = "width:18%;padding-right:3px">
		<label style="padding-left:45px" for='Search_fromdate'><%:ViewResources.SharedStrings.CommonDate %></label><br />
		<table cellpadding="0" cellspacing="0" style="margin: 0; width: 100%; padding: 0; border-spacing: 0px;">
			<tr>
				<td style = "width:25%">
					<label for='Search_fromdate'><%:ViewResources.SharedStrings.CommonFrom %></label>
				</td>
				<td style = "width:75%;">
					<%:Html.TextBox("FromDateLoc", string.Format("{0} {1}:{2}", DateTime.Now.AddDays(-1).ToString("dd.MM.yyyy"), DateTime.Now.Hour.ToString("D2"), DateTime.Now.Minute.ToString("D2")))%>
				</td>
			</tr>
			<tr>
				<td style = "width:25%">
					<label for='Search_fromdate'><%:ViewResources.SharedStrings.CommonTo %></label>
				</td>
				<td style = "width:75%">
					<%:Html.TextBox("ToDateLoc", string.Format("{0} 00:00", DateTime.Now.AddDays(1).ToString("dd.MM.yyyy"), DateTime.Now.Hour.ToString("D2"), DateTime.Now.Minute.ToString("D2") ))%>
				</td>
			</tr>
           
		</table>
	</td>
	<%if(Model.User.CompanyId == null ) {%>
		<td style = "width:25%; vertical-align:top; padding-top:2px">
			<label for='Search_company'><%:ViewResources.SharedStrings.UsersCompany %></label><br />
			<select style="width:98%" name = "CompanyId" id="CompanyId"  onchange="showReportCompany()"></select>
            <input type='button' id='Button3' style='display:none' value='<%=ViewResources.SharedStrings.ReportToCompany %>' onclick = "ReportCompany()"  /> 

		</td>

	<%} %>
    <td style = "width:25%; vertical-align:top; padding-top:2px">
        <label>Report</label><br />
	    <select style="width:98%" name = "ReportId" id="ReportId" onchange="ChechReport()"></select>
        	<%if(!Model.User.IsCompanyManager ) {%>
        <input type='button' id='Button4' style='display:none' value='Report to all Companies' onclick = "ReportToAll()"  /> 
        <%} %>
    </td>
    <td style='width: 30%;'>
                <div id='logPrintControlButtons' style="display:none; text-align:right">
            <a style="cursor:pointer;" onclick="javascript:SetLocExportLink(this,'/Print/LocListPDF')">PDF</a> / <a style="cursor:pointer; " onclick="javascript:SetLocExportLink(this,'/Print/LocListExcel')">XLS</a>
        </div>

    </td>
	<td style='width: 4%; vertical-align: top; padding-top: 20px;'>
		<span id='button_submit_log_search' class='icon icon_find tipsy_we' original-title='Search log!' onclick="javascript:SubmitlogSearch();"></span>
	</td>
</tr>
</table>
</form>
<div id='AreaLogSearchResultsWait' style='display: none; width: 100%; height:620px; text-align:center'><span style='position:relative; top:40%' class='icon loader'></span></div>
<div id='ArealogSearchResults' style='display: none; margin: 15px 0;'></div>

<script type="text/javascript" language="javascript">

    var log_page = 0;
    var log_rows = 50;
    var log_field = 1;
    var log_direction = 1;

    $(document).ready(function () {
    	$("#UserName").attr("id", function () {
    		$("#UserName").autocomplete({
    			source: function (request, response) { $.getJSON("/Log/SearchByNameAutoComplete", { term: request.term }, response); },
    			minLength: 1
    		});
    	});
    	var i = $('#panel_owner li').index($('#logTab'));

    	var opened_tab = '<%: Session["tabIndex"] %>';

    	if (opened_tab != '') {
    		i1 = new Number(opened_tab);
    		if (i1 != i) {
    			SetOpenedTab(i);
    		}
    	}
    	else {
    		SetOpenedTab(i);
    	}
		
    	$("input:button").button();
    	$('div#Work').fadeIn("slow");

    	$(".tipsy_we").attr("class", function () {
    		$(this).tipsy({ gravity: $.fn.tipsy.autoWE, html: true });
    	});

    	    	$("#FromDateLoc").datetimepicker({
    	    		showSecond: false,
    	    		dateFormat: "dd.mm.yy",
    	    		timeFormat: 'HH:mm',
    	    		firstDay: 1,
    	    		changeMonth: true,
    	    		changeYear: true,
    	    		showButtonPanel: true,
    	    		timeText: 'Time ',
    	    		minuteText: 'Minute ',
    	    		hourText: 'Hour'
    	    		
    	    	});
    	

    	        $("#ToDateLoc").datetimepicker({
    	        	showSecond: false,
    				dateFormat: "dd.mm.yy",
    	        	timeFormat: 'HH:mm',
    	            firstDay: 1,
    	            changeMonth: true,
    	            changeYear: true,
    	            showButtonPanel: true,
    	            timeText: 'Time ',
    				minuteText: 'Minute ',
    				hourText: 'Hour'
    	        });

    	$('input:radio[name=IsShowDefaultLog][value=false]').click();

    	GetCompanies();
    	GetReports();
    	GetFilters();
    });
    function Day()
    { }
  function Month()
    {   
        var dt = new Date();
        dt.setMonth(dt.getMonth() - 1);

        date = String();
        date = dt.getDay()+"."+dt.getMonth()+"."+dt.getYear() + " " + dt.getHours() + ":" + dt.getMinutes();

        $('#FromDateLoc').attr('value', date);
      //  $("#FromDate").value(string.Format("{0} 00:00", DateTime.Now.AddDays(-1).ToString("dd.MM.yyyy"), DateTime.Now.Hour.ToString("D2"), DateTime.Now.Minute.ToString("D2")) );

        return false;
  }
  function ReportToAll()
  {
      $.ajax({
          type: "Get",
          url: "/Log/ReportToAllCompanies?" + $("#locFilters").serialize(),
          success: function (data) { ChechReport(); }
      });
  }

  function ChechReport()
  {
      $.ajax({
          type: "Get",
          url: "/Log/ToAllCompanies?" + $("#locFilters").serialize(),
          success: function (data) {
              if (data > 1)
              {
                  $("#Button4").fadeOut('fast');
              } else {
                  $("#Button4").fadeIn('slow');

              }
          }
      });
  }
  function showReportCompany() {
      var e = document.getElementById("CompanyId");
      var comp = e.options[e.selectedIndex].value;
      if (comp <= null) {
         
          $("#Button3").fadeOut('fast');
      } else
      {
          $("#Button3").fadeIn('slow');
          
      }
  }

  function ReportCompany()
  {
      $.ajax({
          type: "Get",
          url: "/Log/ReportCompany?" + $("#locFilters").serialize(),
          success: function (result) {
              if (result = true) {
                  var msg = "Report added to company!";
                  ShowDialog(msg, 2000, true);
                  ChechReport();
              }
              else
              {
                  var msg1 = "Error!";
                  ShowDialog(msg1, 2000);
              }
          }
      });
      return false;
  }
    function GetCompanies() {
        $.ajax({
            type: "Get",
            url: "/Log/GetCompanies",
            dataType: 'json',
            success: function (data) {
                if ($("select#CompanyId").size() > 0) {
                    $("select#CompanyId").html(data);
                }
            }
        });
        return false;
    }
    function GetReports() {
        $.ajax({
            type: "Get",
            url: "/Log/GetReports",
            dataType: 'json',
            success: function (data) {
                if ($("select#ReportId").size() > 0) {
                    $("select#ReportId").html(data);
                }
            }
        });
        return false;
    }

    function GetFilters() {
        $.ajax({
            type: "Get",
            url: "/Log/GetFilters",
            dataType: 'json',
            success: function (data) {
                $("select#selectedFilter").html(data);
            }
        });
        return false;
    }
    
    function ChangeFilter() {
        filter = $("select#selectedFilter")[0].value;
        $("LogFilterId").attr('value', filter);
        if (filter != "") {
        	$.get('/Log/GetFilterData', { filterId: filter },
				function (data) {
					if ($("select#CompanyId").size() > 0) {
						$("select#CompanyId").find('option').each(function () {
							if ($(this).attr('value') == data.companyId) {
								$(this).attr('selected', 'selected');
							}
						});
					}
					else {
						$("#CompanyId").attr('value', data.CompanyId);
					}
					ClearFilterFields();
					$('#Activity').attr('value', data.activity);
					$('#FromDateLoc').attr('value', data.fromDate);
					$('#ToDateLoc').attr('value', data.toDate);
					$('#Building').attr('value', data.building);
					$('#Node').attr('value', data.node);
					$('#Name').attr('value', data.name);
					$('#UserName').attr('value', data.userName);
					$('#LogFilterId').attr('value', data.id);
					if (data.showFullLog == true) {
						$('#show_full_log').attr('checked', true);
						$('#IsShowFullLog').attr('value', true);
					} else {
						$('#show_full_log').attr('checked', false);
						$('#IsShowFullLog').attr('value', false);
					}
					if (data.showDefaultLog == true) {
						$('input:radio[name=IsShowDefaultLog][value=true]').click();

						//$('input:radio[name="IsShowDefaultLog"]').val('true');
					} else {
						$('input:radio[name=IsShowDefaultLog][value=false]').click();
						//$('input:radio[name="IsShowDefaultLog"]').val('false');
					}
				}, 'json');
        }
        else {
            if ($("select#CompanyId").size() > 0) {
                $("select#CompanyId").find('option').each(function () {
                    if ($(this).attr('value') == "") {
                        $(this).attr('selected', 'selected');
                    }
                });
            }
            else {
                $("#CompanyId").attr('value', "");
            }
            ClearFilterFields();
        }
    }

    function ClearFilterFields() {
        $('#Activity').attr('value', "");
        $('#FromDateLoc').attr('value', "");
        $('#ToDateLoc').attr('value', "");
        $('#Building').attr('value', "");
        $('#Node').attr('value', "");
        $('#Name').attr('value', "");
        $('#UserName').attr('value', "");
        $('#LogFilterId').attr('value', "");
        $('input:radio[name=IsShowDefaultLog][value=false]').click();
		//$('input:radio[name="IsShowDefaultLog"]').filter('[value="true"]').attr('checked', true);

		//$('#IsShowDefaultLog').attr('value', false);
     }

//    function ShowFullLog() {
//        if ($('#IsShowFullLog').attr('value') == true) {
//            $('#IsShowFullLog').attr('value', false);
//        }
//        else {
//            $('#IsShowFullLog').attr('value', true);
//        }
//    }

//    function ShowDefaultLog() {
//        if ($('#IsShowDefaultLog').attr('value') == true) {
//            $('#IsShowDefaultLog').attr('value', false);
//        }
//        else {
//            $('#IsShowDefaultLog').attr('value', true);
//        }
//    }

    function SaveFilterData() {
        $.post("/Log/EditCreateFilter",
			$("#locFilters").serialize(), function (response) {
			    if (response.IsSucceed == true) {
			        if (response.IsCreate) {
			            $.ajax({
			                type: "Get",
			                url: "/Log/GetFilters",
			                dataType: 'json',
			                success: function (data) {
			                    $("select#selectedFilter").html(data);
			                    $($("select#selectedFilter")).find('option:last').attr('selected', 'selected');
			                }
			            });
			            $("#LogFilterId").attr('value', response.Id);
			        }
			        else {
			            $("select#selectedFilter").find('option').each(function () {
			                if ($(this).attr('value') == response.Id) {
			                    $(this).html(response.FilterName);
			                }
			            });
			        }
			        ShowDialog(response.Msg, 2000, true);
			    } else {
			        ShowDialog(response.Msg, 2000);
			    }
			}, 'json');
        return false;
    }

    function DeleteFilterData() {
        $.post("/Log/DeleteFilter",
		    { id: $("#LogFilterId").attr('value') }, function (response) {
		        if (response.IsSucceed == true) {
		            $("select#selectedFilter").find('option').each(function () {
		                if ($(this).attr('value') == $("#LogFilterId").attr('value')) {
		                    $(this).remove();
		                }
		            });
		            if ($("select#CompanyId").size() > 0) {
		                $("select#CompanyId").find('option').each(function () {
		                    if ($(this).attr('value') == "") {
		                        $(this).attr('selected', 'selected');
		                    }
		                });
		            }
		            else {
		                $("#CompanyId").attr('value', "");
		            }
		            $('#Activity').attr('value', "");
		            $('#FromDateLoc').attr('value', "");
		            $('#ToDateLoc').attr('value', "");
		            $('#Building').attr('value', "");
		            $('#Node').attr('value', "");
		            $('#Name').attr('value', "");
		            $('#UserName').attr('value', "");
		            $('#LogFilterId').attr('value', "");
		            $('#show_full_log').attr('checked', false);
		            $('#show_default_log').attr('checked', false);
		            $('#IsShowDefaultLog').attr('value', false);
		            $('#IsShowFullLog').attr('value', false);
		        }
		        ShowDialog(response.Msg, 2000, true);
		    }, 'json');
        return false;
    }

    function ViewLogDetail(cntr) {
        logRow = $(cntr).parents('tr#logRecord');

        isShort = logRow.find('#isShortDisplayed').attr('value');

        if (isShort == 'true') {
            logRow.find('#shortAction').hide();
            logRow.find('#fullAction').show();
            logRow.find('#isShortDisplayed').attr('value', 'false');
        }
        else {
            logRow.find('#shortAction').show();
            logRow.find('#fullAction').hide();
            logRow.find('#isShortDisplayed').attr('value', 'true');
        }
        return false;
    }

    function SubmitLogSearch() {
        $.ajax({
            type: "Get",
            url: "/Log/LocationList?" + $("#locFilters").serialize() + "&nav_page=" + log_page + "&rows=" + log_rows + "&sort_field=" + log_field + "&sort_direction=" + log_direction,
            beforeSend: function () {
                $("#button_submit_log_search").addClass("Trans");
                $("div#AreaLogSearchResults").fadeOut('fast', function () { $("div#AreaLogSearchResultsWait").fadeIn('slow'); });
            },
            success: function (result) {
                $("div#AreaLogSearchResultsWait").hide();
                $("div#AreaLogSearchResults").html(result);
                $("div#AreaLogSearchResults").fadeIn('fast');
                $("#button_submit_log_search").removeClass("Trans");
            }
        });
        return false;
    }

    function HandleLogPaging(page, rows) {
        log_page = page;
        log_rows = rows;
        SubmitLogSearch();
        return false;
    }

    function HandleLogSoring(page, rows, field, direction) {
        log_page = page;
        log_rows = rows;
        log_field = field;
        log_direction = direction;
        SubmitLogSearch();
        return false;
    }

    function TogglePrintLog() {
        $("div#logPrintControlButtons").toggle(300);
        return false;
    }

    function SetLocExportLink(link, base) {
        link.href = base +
		    '?' + $("#locFilters").serialize() +
		    '&sort_field=' + log_field +
		    '&sort_direction=' + log_direction;
            
        return true;
    }

</script>
</div>