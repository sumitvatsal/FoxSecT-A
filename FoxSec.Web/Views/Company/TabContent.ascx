<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.HomeViewModel>" %>
<div id='tab_companies'>
    <table cellpadding="0" cellspacing="1" style="margin: 0; width: 100%; padding: 0; border-spacing: 1px;">
        <tr>
            <td style='width: 20%; vertical-align: top;'>
                <div id='AreaCompanyTree' style='margin: 15px 15px 15px 0; padding: 10px;'></div>
            </td>
            <td style='width: 80%; vertical-align: top;'>
                <table cellpadding="0" cellspacing="1" style="margin: 0; width: 100%; padding: 0; border-spacing: 1px;">
                    <tr>
                        <td style='width: 30px;'></td>
                        <td style='width: 25%; vertical-align: top;'>
                            <label for='Search_companyname'><%:ViewResources.SharedStrings.UsersName %></label><br />
                            <input type='text' id='Search_companyname' style='width: 85%;;text-transform: capitalize;' value='' onkeypress="javascript:onPressSubmitCompanySearch(event);" />
                        </td>
                        <td style='width: 25%; vertical-align: top;'>
                            <label for='Search_building'><%:ViewResources.SharedStrings.CommonBuilding %></label><br />
                            <input type='text' id='Search_building' style='width: 85%;' value='' onkeypress="javascript:onPressSubmitCompanySearch(event);" />
                        </td>
                        <td style='width: 10%; vertical-align: top;'>
                            <label for='Search_floor'><%:ViewResources.SharedStrings.CommonFloor %></label><br />
                            <input type='text' id='Search_floor' style='width: 85%;' value='' onkeypress="javascript:onPressSubmitCompanySearch(event);" />
                        </td>
                        <td style='width: 30%; vertical-align: top;'>
                            <label for='Search_additional_info'><%:ViewResources.SharedStrings.CompaniesAdditionalInfo %></label><br />
                            <input type='text' id='Search_additional_info' style='width: 85%;' value='' onkeypress="javascript:onPressSubmitCompanySearch(event);" />
                        </td>
                        <td style='width: 5%; vertical-align: top; padding-top: 20px;'>
                            <span id='button_submit_company_search' class='icon icon_find tipsy_we' original-title='<%=ViewResources.SharedStrings.CompaniesSearch %>'
                                onclick="javascript:c_country_id = 0; c_location_id = 0; c_building_id = 0; SubmitCompanySearch();"></span>
                        </td>
                    </tr>
                </table>
                <div style='margin: 10px 0 0 0; text-align: right;'>
                    <table cellpadding='0' cellspacing='0' style='margin: 0; width: 100%; padding: 0; border-spacing: 0; border: none;'>
                        <tr>
                            <td style='width: 10px;'></td>
                            <td><%:ViewResources.SharedStrings.CompaniesCompanyStatus %>: 
                    <select id='CompanyFilter'>
                        <option value="1"><%:ViewResources.SharedStrings.FilterActive %></option>
                        <option value="0"><%:ViewResources.SharedStrings.FilterDeactivated %></option>
                        <option value="2"><%:ViewResources.SharedStrings.FilterShowAll %></option>
                    </select>
                            </td>
                            <td style='text-align: right'>
                                <input type='button' id='button_delete_company' value='<%=ViewResources.SharedStrings.CompaniesBtnDeleteCompany %>' onclick="javascript: DeleteCompany();" style='display: none' />
                                <input type='button' id='button_activate_company' value='<%=ViewResources.SharedStrings.CompaniesBtnActivateCompany %>' onclick="javascript: ActivateCompany();" style='display: none' />
                                <input type='button' id='button_deactivate_company' value='<%=ViewResources.SharedStrings.CompaniesBtnDeactivateCompany %>' onclick="javascript: DeactivateCompany();" style='display: none' />
                                <input type='button' id='button_add_company' value='<%=ViewResources.SharedStrings.BtnAddNewCompany %>' onclick="javascript: AddCompany('submit_add_comapny', '<%=ViewResources.SharedStrings.BtnAddNewCompany %>');" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id='AreaTabCompanySearchResultsWait' style='display: none; width: 100%; height: 378px; text-align: center'><span style='position: relative; top: 35%' class='icon loader'></span></div>
                <div id='CompaniesList' style='display: none; margin: 15px 0;'></div>
            </td>
        </tr>
    </table>

    <script type="text/javascript" language="javascript">

        var c_page = 0;
        var c_rows = 10;
        var c_field = 0;
        var c_direction = 0;
        var c_country_id = 0;
        var c_location_id = 0;
        var c_building_id = 0;
        function onPressSubmitCompanySearch(e) {
            if (e.keyCode == 13) {
                c_country_id = 0;
                c_location_id = 0;
                c_building_id = 0;
                SubmitCompanySearch();
            }
            return false;
        }
        function HandleCompaniesPaging(page, rows) {
            c_page = page;
            c_rows = rows;
            SubmitCompanySearch();
            return false;
        }
        function HandleCompaniesSoring(page, rows, field, direction) {
            c_page = page;
            c_rows = rows;
            c_field = field;
            c_direction = direction;
            SubmitCompanySearch();
            return false;
        }
        function SubmitCompanySearch() {
            Companyname = $("input#Search_companyname").val();
            Building = $("input#Search_building").val();
            Floor = $("input#Search_floor").val();
            Info = $("input#Search_additional_info").val();
            Filter = $("select#CompanyFilter").val();

            $('input#button_delete_company').fadeOut();
            $('input#button_activate_company').fadeOut();
            $('input#button_deactivate_company').fadeOut();

            $.ajax({
                type: "Post",
                url: "/Company/List",
                data: {
                    name: Companyname, building: Building, floor: Floor, info: Info, filter: Filter, nav_page: c_page, rows: c_rows, sort_field: c_field, sort_direction: c_direction,
                    countryId: c_country_id, locationId: c_location_id, buildingId: c_building_id
                },
                beforeSend: function () {
                    $("#button_submit_company_search").addClass("Trans");
                    //  $("div#CompaniesList").fadeOut('fast', function () { $("div#AreaTabCompanySearchResultsWait").fadeIn('fast'); });
                },
                success: function (result) {
                    $("div#AreaTabCompanySearchResultsWait").hide();
                    $("div#CompaniesList").html(result);
                    $("div#CompaniesList").fadeIn('fast');
                    $("#button_submit_company_search").removeClass("Trans");
                }
            });
            return false;
        }

        function AddCompany(Action, Message) {
            $("div#modal-dialog").dialog({
                open: function () {
                    $("div#modal-dialog").html("");
                    $.get('/Company/Create', function (html) {
                        $("div#modal-dialog").html(html);
                    });
                },
                resizable: false,
                width: 640,
                height: 620,
                modal: true,
                title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + Message,
                buttons: {
                    '<%=ViewResources.SharedStrings.BtnSave %>': function () {
                        var selecteditems = [];

                        $("#company_list").find("input:checked").each(function (i, ob) {
                            selecteditems.push($(ob).val());
                        });

                        dlg = $(this);

                        $.post("/Company/Add", $("#createNewCompany").serialize(),
                            function (response) {
                               
                                if (response.IsSucceed) {
                                    if (selecteditems.length > 0) {
                                        $.ajax({
                                            type: "POST",
                                            url: "/Company/SaveSubComapnyDetails",
                                            data: { compid: response.Id, complist: selecteditems },
                                            async: false,
                                            success: function (result) {
                                            }
                                        });
                                    }
                                    dlg.dialog("close");
                                    ShowDialog(response.Msg, 2000, true);
                                    setTimeout(function () { SubmitCompanySearch(); }, 1000);
                                }
                                else {
                                    if (response.Msg == "licence error") {
                                        ShowDialog('<%=ViewResources.SharedStrings.MaxCompanyCountLicence %> ' + response.Count, 5000);
                                    }
                                    $("div#modal-dialog").html(response.viewData);
                                }
                            },
                            "json");
                    },
    			'<%=ViewResources.SharedStrings.BtnCancel %>': function () {
                        $(this).dialog("close");
                    }
                }
            });
            return false;
        }

        function CompanyByCountry(Id) {
            c_country_id = Id;
            c_location_id = 0;
            c_building_id = 0;
            $.ajax({
                type: "Get",
                url: "/Company/ByCountry",
                data: { id: Id },
                beforeSend: function () {
                    $("#button_submit_company_search").addClass("Trans");
                },
                success: function (result) {
                    $("div#CompaniesList").html(result);
                    $("div#CompaniesList").fadeIn('slow');
                    $("#button_submit_company_search").removeClass("Trans");
                }
            });
            return false;
        }

        function CompanyByLocation(Id) {
            c_country_id = 0;
            c_location_id = Id;
            c_building_id = 0;
            $.ajax({
                type: "Get",
                url: "/Company/ByLocation",
                data: { id: Id },
                beforeSend: function () {
                    $("#button_submit_company_search").addClass("Trans");
                },
                success: function (result) {
                    $("div#CompaniesList").html(result);
                    $("div#CompaniesList").fadeIn('slow');
                    $("#button_submit_company_search").removeClass("Trans");
                }
            });
            return false;
        }

        function CompanyByBuilding(Id) {
            c_country_id = 0;
            c_location_id = 0;
            c_building_id = Id;
            $.ajax({
                type: "Get",
                url: "/Company/ByBuilding",
                data: { id: Id },
                beforeSend: function () {
                    $("#button_submit_company_search").addClass("Trans");
                },
                success: function (result) {
                    $("div#CompaniesList").html(result);
                    $("div#CompaniesList").fadeIn('slow');
                    $("#button_submit_company_search").removeClass("Trans");
                }
            });
            return false;
        }

        function ChangeBuilding(cntr) {
            building_id = $(cntr).val();
            company_id = $('Company_Id').attr('value');
            parent_row = $(cntr).parents('tr');
            floors_div = parent_row.find('#buildingFloors');
            floors_div.html("");
            if ($(cntr).val() != "") {
                $.get('/Company/GetBuildingFloors', { buildingId: building_id, companyId: company_id }, function (html) {
                    floors_div.html(html);
                    SetUpCompanyBuildingsNames();
                });
            }
            return false;
        }

        function RemoveBuilding(cntr) {
            building_div = $(cntr).parents('#companyBuilding');
            $("div#delete-modal-dialog").dialog({
                open: function (event, ui) {
                    $("div#delete-modal-dialog").html('<%=ViewResources.SharedStrings.CommonConfirmMessage %>');
                },
                resizable: false,
                width: 240,
                height: 140,
                modal: true,
                title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%= ViewResources.SharedStrings.CommonDeleting %>',
                buttons: {
					'<%=ViewResources.SharedStrings.BtnOk %>': function () {

                        building_div.remove();
                        CheckDeleteButtons();
                        SetUpCompanyBuildingsNames();
                        $(this).dialog("close");
                    },
					'<%=ViewResources.SharedStrings.BtnCancel %>': function () {
                        $(this).dialog("close");
                    }
                }
            });
        }

        function AddBuilding() {
            $.get('/Company/FakeBuilding', function (html) {
                //$(html).insertAfter($('#companyBuilding :last'));
                $(html).appendTo('#buildings_list');
                SetUpCompanyBuildingsNames();
            });

            $('[id*=building_delete]').each(function () {
                $(this).show();
            });
            return false;
        }


        function CheckDeleteButtons() {
            cnt = $('[id*=companyBuildingRow]').size();
            $('[id*=building_delete]').each(function () {
                if (cnt == 1) {
                    $(this).hide();
                }
                else {
                    $(this).show();
                }
            });
        }

        function SetUpCompanyBuildingsNames() {
            index = 0;
            $('div #companyBuilding').each(function () {
                cntr = $(this).find('[id*=CompanyBuildingItems]:last');
                cntr_name = $(cntr).attr('name');
                cur_index_start = cntr_name.indexOf('[');
                cur_index_end = cntr_name.indexOf(']');
                cur_index = cntr_name.substr(cur_index_start + 1, cur_index_end - cur_index_start - 1);

                old_replace_part = 'CompanyBuildingItems[' + cur_index + ']';
                new_replace_part = 'CompanyBuildingItems[' + index + ']';
                $(this).find('[id*=CompanyBuildingItems]').each(function () {
                    current_cntr_name = $(this).attr('name');
                    new_name = current_cntr_name.replace(old_replace_part, new_replace_part);
                    $(this).attr('name', new_name);
                });
                index++;
            });
        }

        $(document).ready(function () {
            var i = $('#panel_owner_tab_administration li').index($('#companyTab'));
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
            $.get('/Company/GetTree', function (html) {
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
            });
        });

    </script>
</div>
