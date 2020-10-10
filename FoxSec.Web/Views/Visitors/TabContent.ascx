<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.HomeViewModel>" %>
<div id='tab_people_default'>
    <table cellpadding="0" cellspacing="1" style="margin: 0; width: 100%; padding: 0; border-spacing: 1px;">
        <tr>
            <td style='width: 20%; vertical-align: top'>
                <div id='AreaCompanyTree' style='margin: 15px 15px 15px 0; padding: 10px;'></div>
            </td>
            <td style='width: 80%; vertical-align: top'>
                <table cellpadding="0" cellspacing="1" style="margin: 0; width: 100%; padding: 0; border-spacing: 1px;">
                    <tr>

                        <td style='width: 20%; vertical-align: top'>
                            <label for='Search_usrname'><%:ViewResources.SharedStrings.UsersName %></label><br />
                            <input type='text' id='Search_usrname' style='width: 85%;' onkeypress="javascript:onPressSubmitPeopleSearch(event);" />
                        </td>

                        <% if (!Model.User.IsCompanyManager && !Model.User.IsDepartmentManager)
                            { %>
                        <td style='width: 20%; vertical-align: top;'>
                            <label for='CompanyIdta' style="font-weight: bold"><%:ViewResources.SharedStrings.UsersCompany %></label><br />
                            <select style="width: 90%" name="CompanyId" id="CompanyIdta"></select>
                        </td>
                        <% } %>
                        <% if (Model.User.IsDepartmentManager)
                            { %>
                        <td style='width: <%= Model.User.IsCompanyManager ? "7%" : "15%" %>; vertical-align: top;'>
                            <label for='Search_title'><%:ViewResources.SharedStrings.UsersTitle %></label><br />
                            <input type='text' id='Search_title' style='width: 85%;' value='' onkeypress="javascript:onPressSubmitPeopleSearch(event);" />
                        </td>
                        <%}%>

                        <td style='width: 20%;'>
                            <label style="font-weight: bold"><%:ViewResources.SharedStrings.Status %></label>
                            <br />
                            <select id='UserFilter1'>
                                <option value="1"><%:ViewResources.SharedStrings.FilterActive %></option>
                                <option value="0"><%:ViewResources.SharedStrings.FilterDeactivated %></option>
                                <option value="2"><%:ViewResources.SharedStrings.FilterShowAll %></option>
                            </select>
                            <%-- <input type="button" id='button_submit_people_search'
                                onclick="tree_country_id = 0; tree_building_id = 0; tree_location_id = 0; tree_company_id = 0; tree_floor_id = 0; javascript: SubmitPeopleSearch();" value="Search" />--%>
                        </td>
                        <td style='width: 5%;' align="right">
                            <span id='button_submit_people_search' class='icon icon_find tipsy_we' title='<%:ViewResources.SharedStrings.UsersSearchPeople %>'
                                onclick="tree_country_id = 0; tree_building_id = 0; tree_location_id = 0; tree_company_id = 0; tree_floor_id = 0; javascript:SubmitPeopleSearch();"></span>
                        </td>
                    </tr>
                </table>

                <div style='margin: 10px 0 0 0; text-align: right;'>
                    <table style='margin: 0; width: 100%; padding: 0; border-spacing: 0; border: none;'>
                        <tr>

                            <td style='text-align: right'>
                                <%if (!Model.User.IsDepartmentManager)
                                    {%>
                                <input type='button' id='button_move_users_to_departament' value='<%:ViewResources.SharedStrings.UsersMoveUser %>' onclick="javascript: MoveUserToDepartament();" style='display: none' />
                                <%}%>
                                <%if (Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewVisitorsReadOnly) && Model.StaticId == -1)
                                    {%>
                                <%}
                                    else
                                    { %>

                                <input type='button' id='button_add_user' value='<%:ViewResources.SharedStrings.UsersAddNewVisitor %>' onclick="javascript: AddVisitor();" />
                                <%} %>
                                <%if (Model.HRService)
                                    {%>
                                <input type='button' id='button_Integrate' value='HR' onclick="javascript: Integration();" />
                                <%}%>
          
                            </td>
                        </tr>
                    </table>
                </div>
                <div id='VisitorPrintControlButtons' style="display: none; text-align: right">
                    <a style="cursor: pointer;" onclick="javascript:SetUserExportLink(this,'/Print/VisitorListPDF')">PDF</a> / <a style="cursor: pointer;" onclick="javascript:SetUserExportLink(this,'/Print/VisitorListExcel')">XLS</a>
                </div>
                <div id='AreaTabPeopleSearchResultsWait' style='display: none; width: 100%; height: 378px; text-align: center'><span style='position: relative,; top: 35%' class='icon loader'></span></div>
                <div id='AreaTabPeopleSearchResultsVisitor' style='display: none; margin: 15px 0;'></div>
            </td>
        </tr>
    </table>
</div>


<script type="text/javascript" language="javascript">

    var newUserCreated = false;
    var passChecked = true;
    var pin1Changed = false;
    var pin2Changed = false;
    var visitor_page = 0;
    var visitor_rows = 10;
    var visitor_field = 0;
    var visitor_direction = 0;
    var tree_country_id = 0;
    var tree_location_id = 0;
    var tree_building_id = 0;
    var tree_company_id = 0;
    var tree_floor_id = 0;

    $(document).ready(function () {

        $.ajax({
            type: "Get",
            url: "/Log/GetCompanies",
            dataType: 'json',
            success: function (data) {
                if ($("select#CompanyIdta").size() > 0) {
                    $("select#CompanyIdta").html(data);
                }
            }
        });
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
        $("#button_submit_people_search").show();
        $("input:button").button();
        $('div#Work').fadeIn("slow");
    });

    function onPressSubmitPeopleSearch(e) {
        if (e.keyCode == 13) {
            tree_country_id = 0;
            tree_building_id = 0;
            tree_location_id = 0;
            tree_company_id = 0;
            tree_floor_id = 0;
            SubmitPeopleSearch();
            if ($("input#CompanyIdta").size() > 0) {
                cntr = $("input#CompanyIdta");
                //GetDepartmentByCompany(cntr.attr('value'));
            }
        }
        return false;
    }

    function GetDepartmentByCompany(comp_name) {
        $.get('/Department/GetDepartmentsByCompany', { companyName: comp_name }, function (data) { $("select#selectedDepartment").html(data); }, 'json');
    }

    function SubmitPeopleSearch() {

        var Username = $("input#Search_usrname").val();
        Company = $('#CompanyIdta').val();

        Filter = $("select#UserFilter1").val();
        $('input#button_delete_user').fadeOut();
        $('input#button_activate_user').fadeOut();
        $('input#button_deactivate_user').fadeOut();

        $.ajax({
            type: "Post",
            url: "/Visitors/Search",
            data: {
                name: Username, company: Company, filter: Filter, nav_page: visitor_page, rows: visitor_rows, sort_field: visitor_field, sort_direction: visitor_direction,
                countryId: tree_country_id, locationId: tree_location_id, buildingId: tree_building_id, companyId: tree_company_id, floorId: tree_floor_id
            },
            beforeSend: function () {
                $("#button_submit_people_search").fadeOut('fast');
                $("div#AreaTabPeopleSearchResultsWait").show();
                $("div#AreaTabPeopleSearchResultsVisitor").hide();
            },
            success: function (result) {
                $("div#AreaTabPeopleSearchResultsWait").hide();
                $("div#AreaTabPeopleSearchResultsVisitor").show();
                $("div#AreaTabPeopleSearchResultsVisitor").html(result);
                $("div#AreaTabPeopleSearchResultsVisitor").fadeIn('fast');
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
    function AddVisitor() {
        $("div#modal-dialog").dialog({
            open: function () {
                $("div#modal-dialog").html("");
                $("div#user-modal-dialog").html("");
                $.get('/Visitors/CreateVisitor', { id: -1 }, function (html) {
                    $("div#modal-dialog").html(html);
                });
            },
            resizable: false,
            width: 800,
            height: 670,
            modal: true,
            title: "<%=string.Format("<span class={1}ui-icon ui-icon-pencil{1} style={1}float:left; margin:1px 5px 0 0{1} ></span>{0}",ViewResources.SharedStrings.UsersAddNewVisitor, "'") %>",
            buttons: {

                '<%=ViewResources.SharedStrings.BtnClose %>': function () {
                    $(this).dialog("close");
                    /*  if (newUserCreated)
                      {
                            setTimeout(function () {
                              SubmitPeopleSearch(); }, 1000);
                      }*/
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
            url: "/Visitors/ByCountry",
            data: { id: Id },
            beforeSend: function () {
                $("#button_submit_people_search").fadeOut('fast');
                $("div#AreaTabPeopleSearchResultsWait").show();
                $("div#AreaTabPeopleSearchResultsVisitor").hide();
            },
            success: function (result) {
                $("div#AreaTabPeopleSearchResultsWait").hide();
                $("div#AreaTabPeopleSearchResultsVisitor").show();
                $("div#AreaTabPeopleSearchResultsVisitor").html(result);
                $("div#AreaTabPeopleSearchResultsVisitor").fadeIn('fast');
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
            url: "/Visitors/ByLocation",
            data: { id: Id },
            beforeSend: function () {
                $("#button_submit_people_search").fadeOut('fast');
                $("div#AreaTabPeopleSearchResultsWait").show();
                $("div#AreaTabPeopleSearchResultsVisitor").hide();
            },
            success: function (result) {
                $("div#AreaTabPeopleSearchResultsWait").hide();
                $("div#AreaTabPeopleSearchResultsVisitor").show();
                $("div#AreaTabPeopleSearchResultsVisitor").html(result);
                $("div#AreaTabPeopleSearchResultsVisitor").fadeIn('fast');
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
            url: "/Visitors/ByBuilding",
            data: { id: Id },
            beforeSend: function () {
                $("#button_submit_people_search").fadeOut('fast');
                $("div#AreaTabPeopleSearchResultsWait").show();
                $("div#AreaTabPeopleSearchResultsVisitor").hide();
            },
            success: function (result) {
                $("div#AreaTabPeopleSearchResultsWait").hide();
                $("div#AreaTabPeopleSearchResultsVisitor").show();
                $("div#AreaTabPeopleSearchResultsVisitor").html(result);
                $("div#AreaTabPeopleSearchResultsVisitor").fadeIn('fast');
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
            url: "/Visitors/ByFloor",
            data: { floorId: FloorId, companyId: CompanyId },
            beforeSend: function () {
                $("#button_submit_people_search").fadeOut('fast');
                $("div#AreaTabPeopleSearchResultsWait").show();
                $("div#AreaTabPeopleSearchResultsVisitor").hide();
            },
            success: function (result) {
                $("div#AreaTabPeopleSearchResultsWait").hide();
                $("div#AreaTabPeopleSearchResultsVisitor").show();
                $("div#AreaTabPeopleSearchResultsVisitor").html(result);
                $("div#AreaTabPeopleSearchResultsVisitor").fadeIn('fast');
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
            url: "/Visitors/ByCompany",
            data: { id: Id, buildingId: BuildingId },
            beforeSend: function () {
                $("#button_submit_people_search").fadeOut('fast');
                $("div#AreaTabPeopleSearchResultsWait").show();
                $("div#AreaTabPeopleSearchResultsVisitor").hide();
            },
            success: function (result) {
                $("div#AreaTabPeopleSearchResultsWait").hide();
                $("div#AreaTabPeopleSearchResultsVisitor").show();
                $("div#AreaTabPeopleSearchResultsVisitor").html(result);
                $("div#AreaTabPeopleSearchResultsVisitor").fadeIn('fast');
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
                $("div#AreaTabPeopleSearchResultsWait").show();
                $("div#AreaTabPeopleSearchResultsVisitor").hide();
            },
            success: function (result) {
                $("div#AreaTabPeopleSearchResultsWait").hide();
                $("div#AreaTabPeopleSearchResultsVisitor").show();
                $("div#AreaTabPeopleSearchResultsVisitor").html(result);
                $("div#AreaTabPeopleSearchResultsVisitor").fadeIn('fast');
                $("#button_submit_people_search").fadeIn('fast');
            }
        });
        return false;
    }

    $(document).ready(function () {
        $("input#Search_usrname").attr("id", function () {
            $("#Search_usrname").autocomplete({
                source: function (request, response) { $.getJSON("/Visitors/SearchByNameAutoComplete", { term: request.term, filter: $('select#UserFilter1').val() }, response); },
                minLength: 1
            });
        });

        if ($("input#CompanyIdta").size() > 0) {
            $("#CompanyIdta").attr("id", function () {
                $("#CompanyIdta").autocomplete({
                    source: "/Visitors/SearchByCompanyAutoComplete",
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
                    source: "/Visitors/SearchByTitleAutoComplete",
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

    function TogglePrintUser() {
        $("div#VisitorPrintControlButtons").toggle(500, function () {
            $("input[id^=cbx_expVisitorList_][type='checkbox']").each(function () {
                $(this).toggle();
                $(this).prop("checked", true);
            });
        });
        return false;
    }

    function HandleVisitorPaging(page, rows) {
        visitor_page = page;
        visitor_rows = rows;
        SubmitPeopleSearch();
        return false;
    }

    function HandleVisitorSoring(page, rows, field, direction) {
        visitor_page = page;
        visitor_rows = rows;
        visitor_field = field;
        visitor_direction = direction;
        SubmitPeopleSearch();
        return false;
    }

    function SetUserExportLink(link, base) {
        Username = $("input#Search_usrname").val();
        Company = $("select#CompanyIdta").val();
        Filter = $("select#UserFilter1").val();
        if ($("input#cbx_expVisitorList_1").is(':checked') == true) { Display1 = true; } else { Display1 = false; }
        if ($("input#cbx_expVisitorList_2").is(':checked') == true) { Display2 = true; } else { Display2 = false; }
        if ($("input#cbx_expVisitorList_3").is(':checked') == true) { Display3 = true; } else { Display3 = false; }
        if ($("input#cbx_expVisitorList_4").is(':checked') == true) { Display4 = true; } else { Display4 = false; }
        if ($("input#cbx_expVisitorList_5").is(':checked') == true) { Display5 = true; } else { Display5 = false; }
        if ($("input#cbx_expVisitorList_6").is(':checked') == true) { Display6 = true; } else { Display6 = false; }
        if ($("input#cbx_expVisitorList_7").is(':checked') == true) { Display7 = true; } else { Display7 = false; }

        link.href = base +
            '?name=' + Username +
            '&company=' + Company +
            '&filter=' + Filter +
            '&sort_field=' + visitor_field +
            '&sort_direction=' + visitor_direction +
            '&display1=' + Display1 +
            '&display2=' + Display2 +
            '&display3=' + Display3 +
            '&display5=' + Display5 +
            '&display4=' + Display4 +
            '&display6=' + Display6 +
            '&display7=' + Display7;
        return true;
    }

    function PrintUserDataXLS(id) {
        window.location = '/Print/UserDataExcelVisiotr?id=' + id;
    }

    function PrintUserDataPDF(id) {
        window.location = '/Print/UserDataPdfVisiotr?id=' + id;
    }

</script>
