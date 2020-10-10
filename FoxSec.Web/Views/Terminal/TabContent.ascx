<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.HomeViewModel>" %>

<div id='tab_Terminals'>
    <table cellpadding="0" cellspacing="1" style="margin: 0; width: 100%; padding: 0; border-spacing: 1px;">
        <tr>
            <td style='width: 80%; vertical-align: top'>
                <table cellpadding="0" cellspacing="1" style="margin: 0; width: 100%; padding: 0; border-spacing: 1px;">
                    <tr>
                        <td style='width: 18%; vertical-align: top'>
                            <label for='Search_username'><%:ViewResources.SharedStrings.UsersName %></label><br />
                            <input type='text' id='Search_TName' style='width: 85%;' value='' />
                        </td>
                        <td style='width: 25%; vertical-align: top'>
                            <label for='Search_card_no'><%:ViewResources.SharedStrings.TerminalID %></label><br />
                            <input type='text' id='search_TerminalID' style='width: 85%;' value='' />
                        </td>
                        <td style='width: 20%; vertical-align: top'>
                            <label for='Search_card_no'><%:ViewResources.SharedStrings.UsersCompany %></label><br />
                            <select style="width: 90%; display: none;" name="CompanyId" id="search_company"></select>
                        </td>
                        <td style='width: 20%; vertical-align: top'>
                            <label for='Search_card_no'><%:ViewResources.SharedStrings.MaxUser %></label><br />
                            <input type='text' id='Search_username' style='width: 85%; display: none;' value='' onkeypress="javascript:onPressSubmitPeopleSearch(event);" />
                        </td>
                        <td style='width: 15%; vertical-align: top'>
                            <label for='Search_card_no'><%:ViewResources.SharedStrings.LastLogin %></label><br />
                            <input type='text' id='search_LastLogin' style='width: 85%; display: none;' value='' />
                        </td>
                        <td style='width: 2%; vertical-align: top; padding-top: 20px;'>
                            <span id='button_submit_people_search' class='icon icon_find tipsy_we' title='Search Terminals'
                                onclick="tree_country_id = 0; tree_building_id = 0; tree_location_id = 0; tree_company_id = 0; tree_floor_id = 0; javascript:SubmitPeopleSearch();"></span>
                        </td>
                    </tr>
                </table>
                <div style='margin: 10px 0 0 0; text-align: right;'>
                    <table style='margin: 0; width: 100%; padding: 0; border-spacing: 0; border: none;'>
                        <tr>
                            <td style='width: 10px;'></td>
                            <td><%:ViewResources.SharedStrings.Status %>: 
            <select id='TerminalFilter'>
                <option value="2"><%:ViewResources.SharedStrings.FilterShowAll %></option>
                <option value="1">Approve</option>
                <option value="0">Unapprove</option>

            </select>
                            </td>
                            <td style='text-align: right'>
                                <%if (Model.User.IsSuperAdmin || Model.User.IsCompanyManager)
                                    {%>
                                <input type='button' id='button_activate_user' value='Approve' onclick="javascript: ActivateUser();" style='display: none' />
                                <input type='button' id='button_deactivate_user' value='UnApprove' onclick="javascript: DeactivateUser();" style='display: none' />
                                <%} %>
                            </td>
                        </tr>
                    </table>
                </div>

                <div id='AreaTabPeopleSearchResultsWait' style='display: none; width: 100%; height: 378px; text-align: center'><span style='position: relative,; top: 35%' class='icon loader'></span></div>
                <div id='AreaTabPeopleSearchResults' style='display: none; margin: 15px 0;'></div>



            </td>
        </tr>
    </table>
</div>


<script type="text/javascript">

    var newUserCreated = false;
    var passChecked = true;
    var pin1Changed = false;
    var pin2Changed = false;
    var user_page = 0;
    var user_rows = 10;
    var user_field = 1;
    var user_direction = 0;
    //var tree_country_id = 0;
    //var tree_location_id = 0;
    //var tree_building_id = 0;
    //var tree_company_id = 0;
    //var tree_floor_id = 0;

    $(document).ready(function () {
        var i = $('#panel_owner li').index($('#default_tab'));
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

        $.ajax({
            type: "Get",
            url: "/Log/GetCompanies",
            dataType: 'json',

            success: function (data) {
                if ($("select#search_company").size() > 0) {
                    $("select#search_company").html(data);
                }
            }

        });
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
        //debugger
        TerminalName = $("input#Search_TName").val();
        TerminalID = $("input#search_TerminalID").val();
        CompanyID = $("input#search_company").val();
        maxUserName = $("input#Search_username").val();
        LastLogin = $("input#search_LastLogin").val();
        Filter = $("select#TerminalFilter").val();

        $('input#button_delete_user').fadeOut();
        $('input#button_activate_user').fadeOut();
        $('input#button_deactivate_user').fadeOut();

        $.ajax({
            type: "Post",
            url: "/Terminal/Search",
            data: { Tname: TerminalName, TerminalID: TerminalID, companyId: CompanyID, UserName: maxUserName, _lastLogin: LastLogin, nav_page: user_page, rows: user_rows, sort_field: user_field, sort_direction: user_direction, filter: Filter },
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
            width: 1400,
            height: 900,
            modal: true,
            title: "<span class='ui-icon ui-icon-home' style='float:left; margin:1px 5px 0 0'></span>" + 'HR Tab',
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
                $.get('/User/Create', { id: -1 }, function (html) {
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


    function Addcsv() {
        $("div#modal-dialog").dialog({
            open: function () {
                $("div#user-modal-dialog").html("");
                $("div#modal-dialog").html("<div id='AreaUserEditWait' style='width: 100%; height:580px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                $.ajax({
                    type: 'GET',
                    url: '/User/CsvHRTab',
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
            title: "<span class='ui-icon ui-icon-home' style='float:left; margin:1px 5px 0 0'></span>" + 'HR Tab',
            buttons: {
                '<%= ViewResources.SharedStrings.BtnClose %>': function () {
                    $(this).dialog("close");
                }
            }
        });
        return false;

    }



    function UserByCountry(Id) {
        tree_country_id = Id;
        tree_building_id = 0;
        tree_location_id = 0;
        tree_company_id = 0;
        tree_floor_id = 0;
        $.ajax({
            type: "Get",
            url: "/User/ByCountry",
            data: { id: Id },
            beforeSend: function () {
                $("#button_submit_people_search").fadeOut('fast');
            },
            success: function (result) {
                $("div#AreaTabPeopleSearchResults").html(result);
                $("div#AreaTabPeopleSearchResults").fadeIn('fast');
                $("#button_submit_people_search").fadeIn('fast');
            }
        });
        return false;
    }

    function UserByLocation(Id) {
        tree_country_id = 0;
        tree_building_id = 0;
        tree_location_id = Id;
        tree_company_id = 0;
        tree_floor_id = 0;
        $.ajax({
            type: "Get",
            url: "/User/ByLocation",
            data: { id: Id },
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

    function UserByBuilding(Id) {
        tree_country_id = 0;
        tree_building_id = Id;
        tree_location_id = 0;
        tree_company_id = 0;
        tree_floor_id = 0;
        $.ajax({
            type: "Get",
            url: "/User/ByBuilding",
            data: { id: Id },
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

    function UserByFloor(FloorId, CompanyId) {
        tree_country_id = 0
        tree_building_id = 0;
        tree_location_id = 0;
        tree_company_id = CompanyId;
        tree_floor_id = FloorId;
        $.ajax({
            type: "Get",
            url: "/User/ByFloor",
            data: { floorId: FloorId, companyId: CompanyId },
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

    function UserByCompany(Id, BuildingId) {
        tree_country_id = 0;
        tree_building_id = BuildingId;
        tree_location_id = 0;
        tree_company_id = Id;
        tree_floor_id = 0;
        if (Id == null || Id == "" || Id == undefined) {
            Id = "0";
        }
        $.ajax({
            type: "Get",
            url: "/User/ByCompany",
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
            $("div#AreaCompanyTree").html(html);
            $("div#AreaCompanyTree").attr("id", function () {
                $(this).corner("bevelfold");
            });
            $("ul#companies").treeview({
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
            //($('#Search_card_dk').attr('value') > 65535);
            $('#Search_card_dk').attr('value', 65535);
        }
        if ($("input#Search_card_ser").val().length > 0 || $("input#Search_card_dk").val().length > 0) {
            $("input#Search_card_no").val("");
        }
        return false;
    }

    function CardRelationCODE() {

        //   digitsValidate("Search_card_no");
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
        $("div#userPrintControlButtons").toggle(500, function () {
            $("input[id^=cbx_expUserList_][type='checkbox']").each(function () {
                $(this).toggle();
            });
        });
        return false;
    }

    function HandleTerminalPaging(page, rows) {
        user_page = page;
        var x = $('select#TerminalRows option:selected').text();
        user_rows = rows;
        SubmitPeopleSearch();
        return false;
    }

    function HandleTerminalSoring(page, rows, field, direction) {
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
            '&display5=' + Display5 +
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
