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
                        <td style='width: 13%; vertical-align: top'>
                            <label for='Search_username'><%:ViewResources.SharedStrings.UsersName %></label><br />
                            <input type='text' id='Search_username' style='text-transform: capitalize;width: 85%;' value='' onkeypress="javascript:onPressSubmitPeopleSearch(event);"/>
                        </td>
                        <td style='width: 17%; vertical-align: top'>
                            <label for='Search_card_no'><%:ViewResources.SharedStrings.UsersCardNo %></label>
                            <table cellpadding='0' cellspacing='0' style='margin: 0; width: 100%; padding: 0; border-spacing: 0; border: none;'>
                                <tr>
                                    <td style='width: 30%'><%:ViewResources.SharedStrings.UsersSerial %>:</td>
                                    <td style='width: 70%'>
                                        <input type='text' id='Search_card_ser' style='width: 30px;' value='' maxlength='3' onkeyup="javascript:CardSerValidation();" />+<input type='text' id='Search_card_dk' maxlength='5' style='width: 47px;' value='' onkeyup="javascript:CardDkValidation();" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style='width: 30%'><%:ViewResources.SharedStrings.UsersCardCode %>:</td>
                                    <td style='width: 70%'>
                                        <input type='text' id='Search_card_no' style='width: 148px;' value='' onkeyup="javascript:CardCodeValidation();" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <% if (!Model.User.IsCompanyManager && !Model.User.IsDepartmentManager)
                            { %>
                        <td style='width: 10%; vertical-align: top;'>
                            <label for='Search_company'><%:ViewResources.SharedStrings.UsersCompany %></label><br />
                            <input type='text' id='Search_company' style='width: 85%;' value='' onchange="javascript:GetDepartmentByCompany($(this).attr('value'))" onkeypress="javascript:onPressSubmitPeopleSearch(event);" />
                        </td>
                        <% } %>
                        <%if (!Model.User.IsDepartmentManager)
                            {%>
                        <td style='width: 15%; vertical-align: top;'>
                            <label for='Search_department'><%:ViewResources.SharedStrings.UsersDepartment %></label><br />
                            <select id="selectedDepartment"></select>
                        </td>
                        <%}%>
                        <% if (Model.User.IsDepartmentManager)
                            { %>
                        <td style='width: <%= Model.User.IsCompanyManager ? "7%" : "15%" %>; vertical-align: top;'>
                            <label for='Search_title'><%:ViewResources.SharedStrings.UsersTitle %></label><br />
                            <input type='text' id='Search_title' style='width: 85%;' value='' onkeypress="javascript:onPressSubmitPeopleSearch(event);" />
                        </td>
                        <%}
                            else
                            {%>
                        <td style='width: 20%; vertical-align: top;'>
                            <label for='Comment'><%:ViewResources.SharedStrings.CommonComments %></label><br />
                            <input type='text' id='Comment' style='width: 60%;' value='' /><!-- onchange="javascript:GetDepartmentByCompany($(this).attr('value'))" onkeypress="javascript:onPressSubmitPeopleSearch(event);" />-->
                        </td>
                        <%}%>

                        <% if (Model.User.IsCompanyManager || Model.User.IsDepartmentManager)
                            { %>
                        <td style='width: 7%; vertical-align: top;'>
                            <%--<label><%:ViewResources.SharedStrings.UsersValidation %></label>--%><br />
                        </td>
                        <% } %>
                        <td style='width: 2%; vertical-align: top; padding-top: 20px;'>
                            <span id='button_submit_people_search' class='icon icon_find tipsy_we' style="display: none" title='<%:ViewResources.SharedStrings.UsersSearchPeople %>'
                                onclick="tree_country_id = 0; tree_building_id = 0; tree_location_id = 0; tree_company_id = 0; tree_floor_id = 0; javascript:SubmitPeopleSearch();"></span>
                        </td>
                    </tr>
                </table>
                <div style='margin: 10px 0 0 0; text-align: right;'>
                    <table style='margin: 0; width: 100%; padding: 0; border-spacing: 0; border: none;'>
                        <tr>
                            <td style='width: 10px;'></td>
                            <td><%:ViewResources.SharedStrings.UsersUserStatus %>: 
                            <select id='UserFilter'>
                                <option value="1"><%:ViewResources.SharedStrings.FilterActive %></option>
                                <option value="0"><%:ViewResources.SharedStrings.FilterDeactivated %></option>
                                <option value="2"><%:ViewResources.SharedStrings.FilterShowAll %></option>
                            </select>
                            </td>
                            <td style='text-align: right'>
                                <%if (!Model.User.IsDepartmentManager)
                                    {%>
                                <input type='button' id='button_move_users_to_departament' value='<%:ViewResources.SharedStrings.UsersMoveUser %>' onclick="javascript: MoveUserToDepartament();" style='display: none' />
                                <%}%>
                                <input type='button' id='button_delete_user' value='<%:ViewResources.SharedStrings.UsersDeleteUser %>' onclick="javascript: DeleteUser();" style='display: none' />
                                <input type='button' id='button_activate_user' value='<%:ViewResources.SharedStrings.UsersActivateUser %>' onclick="javascript: ActivateUser();" style='display: none' />
                                <input type='button' id='button_deactivate_user' value='<%:ViewResources.SharedStrings.UsersDeactivateUser %>' onclick="javascript: DeactivateUser();" style='display: none' />
                                <%if (Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewTAReportMenu))
                                    {%>
                                <input type='button' id='btnatwork' value='<%=ViewResources.SharedStrings.Atwork %>' onclick='javascript: Atworkmulti();' style='display: none;' />
                                <input type='button' id='btnleaving' value='<%=ViewResources.SharedStrings.Leaving %>' onclick='javascript: leavingmulti();' style='display: none;' />
                                <%} %>
                                <input type='button' id='button_add_user' <% if (Model.DisableAddUser == true) {%> disabled <%} %> value='<%:ViewResources.SharedStrings.UsersAddNewUser %>' onclick="javascript: AddUser();" />
                                <%--                <input type='button' id='button_add_csvb' value='AddUser From Csv' onclick="javascript: Addcsv();" /> --%>

                                <%if (Model.HRService)
                                    {%>
                                <input type='button' id='button_Integrate' value='HR' onclick="javascript: Integration();" />
                                <%}%>                                      
                            </td>
                        </tr>
                    </table>
                </div>
                <div id='userPrintControlButtons' style="display: none; text-align: right">
                    <a style="cursor: pointer;" onclick="javascript:SetUserExportLink(this,'/Print/UserListPDF')">PDF</a> / <a style="cursor: pointer;" onclick="javascript:SetUserExportLink(this,'/Print/UserListExcel')">XLS</a>
                </div>
                <div id='AreaTabPeopleSearchResultsWait' style='display: none; width: 100%; height: 378px; text-align: center'><span style='position: relative,; top: 35%' class='icon loader'></span></div>
                <div id='AreaTabPeopleSearchResults' style='display: none; margin: 15px 0;'></div>
            </td>
        </tr>
    </table>
</div>


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
            if (i1 != i) {
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
        $('input#btnatwork').fadeOut();
        $('input#btnleaving').fadeOut();

        $.ajax({
            type: "Post",
            url: "/User/Search",
            data: {
                comment: Comment, name: Username, cardSer: CardSer, cardDk: CardDk, cardCode: CardNo, company: Company, title: Title, filter: Filter, departmentId: DepartmentID, nav_page: user_page, rows: user_rows, sort_field: user_field, sort_direction: user_direction,
                countryId: tree_country_id, locationId: tree_location_id, buildingId: tree_building_id, companyId: tree_company_id, floorId: tree_floor_id
            },
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
            width: 1200,
            height: 600,
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
            height: 650,
            modal: true,
            title: "<%=string.Format("<span id='spanusername' class={1}ui-icon ui-icon-pencil{1} style={1}float:left; margin:1px 5px 0 0{1} ></span>{0}",ViewResources.SharedStrings.DialogTitleNewUser, "'") %>",
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
            height: 680,
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
        if ($('#Search_card_ser').val() > 255) {
            $('#Search_card_ser').val("255");
        }
        CardRelationSERDK();
        return false;
    }

    function CardRelationSERDK() {
        digitsValidate("Search_card_dk");
        if ($('#Search_card_dk').val() > 65535) {
            //($('#Search_card_dk').attr('value') > 65535);
            $('#Search_card_dk').val("65535");
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

    function HandleUserPaging(page, rows) {
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

    function CardSerValidation() {

        digitsValidate('Search_card_ser');
        var val = $('#Search_card_ser').val();
        if (val > 255) {
            $('#Search_card_ser').val("255");
        }
        if ($('#Search_card_ser').val() != null) {
            if ($('#Search_card_ser').val().length == 3) {
                $("#Search_card_dk").focus();
            }
            if ($('#Search_card_ser').val().length != 0) {
                $("#Search_card_no").val("");
            }
        }
        return false;
    }


    function CardDkValidation() {
        digitsValidate('Search_card_dk');
        if ($('#Search_card_dk').val() > 65535) {
            $('#Search_card_dk').val("65535");
        }
        if ($('#Search_card_dk').val() != null) {
            if ($('#Search_card_dk').val().length != 0) {
                $("#Search_card_no").val("");
            }
        }
        return false;
    }

    function CardCodeValidation() {
        //digitsValidate('Code');
        if ($('#Search_card_no').val().length != 0) {
            $("#Search_card_ser").val("");
            $("#Search_card_dk").val("");
        }
        return false;
    }


</script>
