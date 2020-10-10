<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.CompanyTreeViewModel>" %>

<div id='tab_people_default'>
    <table cellpadding="0" cellspacing="1" style="margin: 0; width: 100%; padding: 0; border-spacing: 1px;">
    <tr>
        <td style='display:none'>
            <% foreach (var country in Model.Companies)
                {%>
                   <%=Html.Hidden("hdncompid", country.MyId )%>
               <% }%>

   <%--<ul id="users" class="hdn" style=" background-color: #CCC;
    width: auto;
    height: auto;
    margin: 15px 15px 15px 0;
    padding: 10px;
   border-radius: 15px;" >
<% foreach (var country in Model.Countries) {%>
    <li>
        &nbsp;<a onclick="UserByCountry(<%= country.MyId %>)"><%= Html.Encode(country.Name)%></a>
        <% foreach (var town in Model.Towns) { if (town.ParentId == country.MyId) { %>
        <ul>
            <li>
                &nbsp;<a onclick="UserByLocation(<%= town.MyId %>)"><%= Html.Encode(town.Name)%> </a>
                <% foreach (var office in Model.Offices) { if (office.ParentId == town.MyId) { %>
                <ul>
                    <li>
                        &nbsp;<a onclick="UserByBuilding(<%= office.MyId %>)"><%= Html.Encode(office.Name)%></a>
                        <% foreach (var company in Model.Companies) { if (company.ParentId == office.MyId) { %>
                        <ul>
                            <li>
                                &nbsp;<a id="anchrclk" onclick="UserByCompany(<%= company.MyId %>, <%= office.MyId %>)"><%= Html.Encode(company.Name)%></a>
                                <% foreach (var partner in Model.Partners) { if (partner.ParentId == company.MyId){ %>
                                    <ul>
                                        <li>
                                            &nbsp;<a onclick="UserByPartner(<%= partner.MyId %>, <%= office.MyId %>)"><%= Html.Encode(partner.Name)%></a>
											<% foreach (var Camera in Model.Cameras) { if (Camera.ParentId == partner.MyId ){ %>
												<ul>
													<li>
														&nbsp;<a onclick="UserByCamera(<%= Camera.MyId %>,<%=company.MyId %> )" ><%= Html.Encode(string.Format(" {0}", Camera.Name))%></a>
													</li>
												</ul>
											<%}}%>
										</li>
                                    </ul>
                                <% }} %>
								<% foreach (var Camera in Model.Cameras) { if (Camera.ParentId == company.MyId){ %>
									 
                                <% }}%>
                            </li>
                        </ul>
                    <% }} %>
                    </li>
                </ul>
                <% }} %>
            </li>
        </ul>
        <% }} %>
    </li>
    <% } %>
</ul>--%>

        </td>
         <td style='width: 80%; vertical-align: top'>
           <table cellpadding="0" cellspacing="1" style="margin: 0; width: 100%; padding: 0; border-spacing: 1px;">
                <tr>

                    <td>
                         <input type='button' id='button_deactivate_user' value='Assign Cameras' onclick="javascript:DeactivateUser();" style='display:none'/>
           <div id='AreaTabPeopleSearchResultsWait' style='display: none; width: 100%; height:378px; text-align:center'><span style='position:relative,; top:35%' class='icon loader'></span></div>
            <div id='AreaTabPeopleSearchResults' style='display: none; margin: 15px 0; margin-left:200px '></div>

                  </td>  </tr>
      </table>
             </td>
        </tr>
        </table>

    </div>
<style>
    .hdn {
        display: none;
    }
</style>
<script type="text/javascript" language="javascript">
 
    $(document).ready(function () {
        $(".tipsy_we").attr("class", function () {
            $(this).tipsy({ gravity: $.fn.tipsy.autoWE, html: true });
        });

        var cid = $("#hdncompid").val();
        
        //  $('#anchrclk').trigger('click');
        UserByCompany(cid);     
    });
    function UserByCompany(Id) {
        
        tree_country_id = 0
        tree_building_id = 0;
        tree_location_id = 0;
        tree_company_id = Id;
         if (Id == null || Id == "" || Id == undefined) {
            Id = "0";
        }

        $.ajax({
            type: "Get",
            url: "/VideoCamera/ByCompany",

            data: { Id: Id },
            success: function (result) {

                $("div#AreaTabPeopleSearchResultsWait").hide();
                $("div#AreaTabPeopleSearchResults").html(result);
                $("div#AreaTabPeopleSearchResults").fadeIn('fast');
                $("#button_submit_people_search").fadeIn('fast');
            },
            error: function (error) {
                   console.log(error);
            }
        });
        return false;
    }

</script>

<script type="text/javascript" language="javascript">

	var newUserCreated = false;
	var passChecked = true;
	var pin1Changed = false;
	var pin2Changed = false;
    var user_page = 0;
    var user_rows = 10;
    var user_field = 1;
    var user_direction = 0;
	var tree_country_id = 0;
	var tree_location_id = 0;
	var tree_building_id = 0;
	var tree_company_id = 0;
	var tree_floor_id = 0;

	$(document).ready(function () {
		var i = $('#panel_owner li').index($('#default_tab'));
		var opened_tab = '<%: Session["tabIndex"] %>';

        if (opened_tab != '') {
        	i1 = new Number(opened_tab);
        	if( i1 != i )
			{
				SetOpenedTab(i);
			}
        }
		else {
    		SetOpenedTab(i);
    	}
	});

    function onPressSubmitPeopleSearch(e) {
        if (e.keyCode == 13) {
			tree_country_id = 0;
			tree_building_id = 0;
			tree_location_id = 0;
			tree_company_id = 0;
			tree_floor_id = 0;
        	SubmitPeopleSearch();
        	if ($("input#Search_company").size() > 0) {
        		cntr = $("input#Search_company");
        		GetDepartmentByCompany(cntr.attr('value'));
        	}
        }
        return false;
    }

    function GetDepartmentByCompany(comp_name) {
        $.get('/Department/GetDepartmentsByCompany', { companyName: comp_name }, function (data) { $("select#selectedDepartment").html(data); }, 'json');
    }
 
    function SubmitPeopleSearch() {
        Username = $("input#Search_username").val();
        CardSer = $("input#Search_card_ser").val();
        CardDk = $("input#Search_card_dk").val();
        CardNo = $("input#Search_card_no").val();
        Comment = $("input#Comment").val();

        if ((CardSer.length > 0) && (CardSer.length < 3)) {
            while (CardSer.length < 3) {
                CardSer = '0' + CardSer;
            }
            $("input#Search_card_ser").val(CardSer);
        }
        if ((CardDk.length > 0) && (CardDk.length < 5)) {
            while (CardDk.length < 5) {
                CardDk = '0' + CardDk;
            }
            $("input#Search_card_dk").val(CardDk);
        }

        if ($("input#Search_company").size() > 0) {
        	Company = $("input#Search_company").val();
        }
        else {
        	Company = "";
        }
        if ($("input#Search_title").size() > 0) {
        	Title = $("input#Search_title").val();
        }
        else {
        	Title = "";
        }
        if ($("#selectedDepartment").size() > 0) {
        	DepartmentID = $("#selectedDepartment").val();
        }
        else {
        	DepartmentID = "";
        }
        if (DepartmentID == "") DepartmentID = 0;
        Filter = $("select#UserFilter").val();

        $('input#button_delete_user').fadeOut();
        $('input#button_activate_user').fadeOut();
        $('input#button_deactivate_user').fadeOut();

        $.ajax({
            type: "Post",
            url: "/LiveVideo/Search",
            data: {comment:Comment, name: Username, cardSer : CardSer, cardDk : CardDk, cardCode : CardNo,  company: Company, title: Title, filter: Filter, departmentId: DepartmentID, nav_page: user_page, rows: user_rows, sort_field: user_field, sort_direction: user_direction,
				countryId: tree_country_id, locationId: tree_location_id, buildingId: tree_building_id, companyId: tree_company_id, floorId: tree_floor_id },
            beforeSend: function () {
                $("#button_submit_people_search").fadeOut('fast');
              //  $("div#AreaTabPeopleSearchResults").fadeOut('fast', function () { $("div#AreaTabPeopleSearchResultsWait").fadeIn('slow'); });
            },
            success: function (result) {
                $("div#AreaTabPeopleSearchResultsWait").hide();
                $("div#AreaTabPeopleSearchResults").html(result);
                $("div#AreaTabPeopleSearchResults").fadeIn('fast');
                $("#button_submit_people_search").fadeIn('fast');
            }
        });

       
        return false;
    }
    function Integration() {
     $("div#modal-dialog").dialog({
            open: function () {
                $("div#user-modal-dialog").html("");
                $("div#modal-dialog").html("<div id='AreaUserEditWait' style='width: 100%; height:580px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                $.ajax({
                    type: 'GET',
                    url: '/User/Tab',
                    cache: false,
                    success: function (html) {
                        $("div#modal-dialog").html(html);
                    }
                });
                $(this).parents('.ui-dialog-buttonpane button:eq(0)').focus();
            },
            resizable: false,
            width: 900,
            height: 710,
            modal: true,
            title: "<span class='ui-icon ui-icon-home' style='float:left; margin:1px 5px 0 0'></span>" +  'HR Tab',
            buttons: {
                '<%= ViewResources.SharedStrings.BtnClose %>': function () {
                    $(this).dialog("close");
                }
            }
        });
        return false;
        
    }
    function AddUser() {
    	$("div#modal-dialog").dialog({
    		open: function () {
    			$("div#modal-dialog").html("");
                $("div#user-modal-dialog").html("");
    		    $.get('/User/Create', { id: -1 }, function(html) {
    		        $("div#modal-dialog").html(html);
    		    });
    		},
    		resizable: false,
    		width: 800,
    		height: 710,
    		modal: true,
    		title: "<%=string.Format("<span class={1}ui-icon ui-icon-pencil{1} style={1}float:left; margin:1px 5px 0 0{1} ></span>{0}",ViewResources.SharedStrings.DialogTitleNewUser, "'") %>",
    		buttons: {
    			'<%=ViewResources.SharedStrings.BtnClose %>': function () {
					$(this).dialog("close");
    				if (newUserCreated) {
    					setTimeout(function () { SubmitPeopleSearch(); }, 1000);
    				}
    			}
    		}
    	});
        return false;
    }


    



  





    //function UserByCompany(Id) {
    //    tree_country_id = 0
    //    tree_building_id = 0;
    //    tree_location_id = 0;
    //    tree_company_id = Id;


    //    $.ajax({
    //        type: "Get",
    //        url: "/VideoCamera/ByCompany",
            
    //        data: { Id: Id },
    //        success: function (result) {

               
              
    //                $("div#AreaTabPeopleSearchResultsWait").hide();
    //                $("div#AreaTabPeopleSearchResults").html(result);
    //                $("div#AreaTabPeopleSearchResults").fadeIn('fast');
    //                $("#button_submit_people_search").fadeIn('fast');
                
  
    //        },
    //        error: function (error) {
       //            console.log(error);
    //        }
    //    });
    //    return false;
    //}


    function UserByPartner(Id, BuildingId) {
        $.ajax({
            type: "Get",
            url: "/User/ByPartner",
            data: { id: Id, buildingId: BuildingId },
            beforeSend: function () {
                $("#button_submit_people_search").fadeOut('fast');
                $("div#AreaTabPeopleSearchResults").fadeOut('fast', function () { $("div#AreaTabPeopleSearchResultsWait").fadeIn('slow'); });
            },
            success: function (result) {
                $("div#AreaTabPeopleSearchResultsWait").hide();
                $("div#AreaTabPeopleSearchResults").html(result);
                $("div#AreaTabPeopleSearchResults").fadeIn('fast');
                $("#button_submit_people_search").fadeIn('fast');
            }
        });
        return false;
    }

    $(document).ready(function () {
    	$("#Search_username").attr("id", function () {
    		$("#Search_username").autocomplete({
    			source: function (request, response) { $.getJSON("/User/SearchByNameAutoComplete", { term: request.term, filter: $('select#UserFilter').val() }, response); },
    			minLength: 1
    		});
    	});

    	if ($("input#Search_company").size() > 0) {
    		$("#Search_company").attr("id", function () {
    			$("#Search_company").autocomplete({
    				source: "/User/SearchByCompanyAutoComplete",
    				minLength: 1,
    				select: function (event, ui) {
    					GetDepartmentByCompany(ui.item.value);
    				}
    			});
    		});
    	}

    	if ($("#Search_title").size() > 0) {
    		$("#Search_title").attr("id", function () {
    			$("#Search_title").autocomplete({
    				source: "/User/SearchByTitleAutoComplete",
    				minLength: 1
    			});
    		});
    	}

    	$("div#AreaPeopleTree").attr("id", function () {
    		$(this).corner("bevelfold");
    	});

    	$.get('/User/GetTree', function (html) {
    		$("div#AreaPeopleTree").html(html);
    		$("div#AreaPeopleTree").attr("id", function () {
    			$(this).corner("bevelfold");
    		});


    		$("ul#users").treeview({
    			animated: "fast",
    			persist: "location",
    			collapsed: true,
    			unique: true
    		});
    	    $("#button_submit_people_search").fadeIn('fast');
    	});
    	if ($('#selectedDepartment').size() > 0) {
    		GetDepartments();
    	}
    	$("input:button").button();
    	$('div#Work').fadeIn("slow");
    	return false;
    });

    function CardValidation() {
        digitsValidate('Search_card_ser');
        if ($('#Search_card_ser').attr('value') > 255) {
            $('#Search_card_ser').attr('value', 255);
        }
        CardRelationSERDK();
        return false;
    }

    function CardRelationSERDK() {
        digitsValidate("Search_card_dk");
        if ($('#Search_card_dk').attr('value') > 65535) {
            $('#Search_card_dk').attr('value', 65535);
        }
        if ($("input#Search_card_ser").val().length > 0 || $("input#Search_card_dk").val().length > 0) {
            $("input#Search_card_no").val("");
        }
        return false;
    }

    function CardRelationCODE() {
        digitsValidate("Search_card_no");
        if ($("input#Search_card_no").val().length > 0) {
            $("input#Search_card_ser").val("");
            $("input#Search_card_dk").val("");
        }
        return false;
    }

    function GetDepartments() {
        $.ajax({
            type: "Get",
            url: "/Department/GetDepartments",
            dataType: 'json',
            success: function (data) {
                $("select#selectedDepartment").html(data);
            }
        });
        return false;
    }

    function TogglePrintUser() {
        $("div#userPrintControlButtons").toggle(500, function() {
            $("input[id^=cbx_expUserList_][type='checkbox']").each(function () {
                $(this).toggle();
            });
        });
        return false;
    }

    function HandleLiveVideoPaging(page, rows) {
         user_page = page;
        user_rows = rows;
        SubmitPeopleSearch();
        return false;
    }

 
    function HandleUserSoring(page, rows, field, direction) {
        user_page = page;
        user_rows = rows;
        user_field = field;
        user_direction = direction;
        SubmitPeopleSearch();
        return false;
    }

    function SetUserExportLink(link, base) {
        Username = $("input#Search_username").val();
        CardSer = $("input#Search_card_ser").val();
        CardDk = $("input#Search_card_dk").val();
        CardNo = $("input#Search_card_no").val();
        if ($("input#Search_company").size() > 0) { Company = $("input#Search_company").val(); } else { Company = ""; }
        if ($("input#Search_title").size() > 0) { Title = $("input#Search_title").val(); } else { Title = ""; }
        if ($("#selectedDepartment").size() > 0) { DepartmentID = $("#selectedDepartment").val(); } else { DepartmentID = ""; }
        if (DepartmentID == "") DepartmentID = 0;
        Filter = $("select#UserFilter").val();
        if ($("input#cbx_expUserList_1").is(':checked') == true) { Display1 = true; } else { Display1 = false; }
        if ($("input#cbx_expUserList_2").is(':checked') == true) { Display2 = true; } else { Display2 = false; }
        if ($("input#cbx_expUserList_3").is(':checked') == true) { Display3 = true; } else { Display3 = false; }
        if ($("input#cbx_expUserList_4").is(':checked') == true) { Display4 = true; } else { Display4 = false; }
        if ($("input#cbx_expUserList_5").is(':checked') == true) { Display5 = true; } else { Display5 = false; }
        if ($("input#cbx_expUserList_6").is(':checked') == true) { Display6 = true; } else { Display6 = false; }
        link.href = base +
		    '?name=' + Username +
            '&cardSer=' + CardSer +
            '&cardDk=' + CardDk +
            '&cardCode=' + CardNo +
            '&company=' + Company +
            '&title=' + Title +
            '&filter=' + Filter +
            '&departmentId=' + DepartmentID +
		    '&sort_field=' + user_field +
		    '&sort_direction=' + user_direction +
            '&display1=' + Display1 +
            '&display2=' + Display2 +
            '&display3=' + Display3 +
            '&display4=' + Display4 +
            '&display5=' + Display5+
            '&display6=' + Display6;
        return true;
    }

    function PrintUserDataXLS(id) {
        window.location = '/Print/UserDataExcel?id=' + id;
    }

    function PrintUserDataPDF(id) {
        window.location = '/Print/UserDataPdf?id=' + id;
    }

</script>


