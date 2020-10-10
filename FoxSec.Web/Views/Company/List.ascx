<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.CompanyListViewModel>" %>

<table id="searchedTableCompany" cellpadding="1" cellspacing="0" style="margin: 0; width: 100%; padding: 1px; border-spacing: 0;">
    <thead>
        <tr>
            <th style='width: 30px; padding: 2px;'>
                <% if (Model.FilterCriteria != 2)
                    {%>
                <input id='check_all_company' name='check_all_company' type='checkbox' class='tipsy_we' original-title='<%=ViewResources.SharedStrings.CommonSelectAll %>' onclick="javascript: CheckAll();" />
                <%}%>
            </th>
            <th style='width: 20%; padding: 2px;'>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:CompaniesSort(0, 0);'></span></li>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:CompaniesSort(0, 1);'></span></li>
            </th>
            <th style='width: 25%; padding: 2px;'>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:CompaniesSort(1, 0);'></span></li>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:CompaniesSort(1, 1);'></span></li>
            </th>
            <th style='width: 20%; padding: 2px;'>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:CompaniesSort(2, 0);'></span></li>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:CompaniesSort(2, 1);'></span></li>
            </th>
            <th style='width: 25%; padding: 2px;'>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:CompaniesSort(3, 0);'></span></li>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:CompaniesSort(3, 1);'></span></li>
            </th>
            <th style='width: 5%; padding: 2px;'></th>
        </tr>
    </thead>
    <tbody>
        <% var i = 1; foreach (var company in Model.Companies)
            {
                var bg = (i++ % 2 == 1) ? " background-color:#CCC;" : ""; %>
        <tr>
            <td style='width: 30px; padding: 2px;'>
                <% if (Model.FilterCriteria != 2)
                    { %>
                <input type='checkbox' id='<%= company.Id %>' name='company_checkbox' onclick='javascript: ManageButtons(<%= Model.FilterCriteria %>);' />
                <% } %>
            </td>
            <td style='width: 20%; word-break: break-all; padding: 2px; <%= bg %>'><%= Html.Encode(company.Name != null && company.Name.Length > 30 ? company.Name.Substring(0, 27) + "..." : company.Name)%></td>
            <td style='width: 25%; word-break: break-all; padding: 2px; <%= bg %>'><%= Html.Encode(company.BuidingNames)%></td>
            <td style='width: 20%; word-break: break-all; padding: 2px; <%= bg %>'><%= Html.Encode(company.Floors)%></td>
            <td style='width: 25%; word-break: break-all; padding: 2px; <%= bg %>'><%= Html.Encode(company.Comment)%></td>
            <td style='width: 5%; word-break: break-all; padding: 2px; text-align: right; <%= bg %>'><span id='button_company_edit_<%= company.Id %>' class='icon icon_green_go tipsy_we' original-title='<%=string.Format("{0} {1}!", ViewResources.SharedStrings.BtnEdit, Html.Encode(company.Name)) %>' onclick='<%=string.Format("javascript:EditCompany(\"submit_edit_company\", {0}, \"{1}\")", company.Id, Html.Encode(company.Name.Replace("\"","\'"))) %>'></span></td>
        </tr>
        <% } %>
    </tbody>
    <tfoot>
        <tr>
            <td colspan="5">
                <% Html.RenderPartial("Paginator", Model.Paginator); %>
            </td>
        </tr>
    </tfoot>
</table>

<script type="text/javascript" language="javascript">

    $(document).ready(function () {
        $(".tipsy_we").attr("class", function () {
            $(this).tipsy({ gravity: $.fn.tipsy.autoWE, html: true });
        });
    });

    function ManageButtons(filter) {
        var any = 0;

        $('input[name=company_checkbox]').each(function () {
            if (this.checked) any++;
        });

        if (any != 0) {
            switch (filter) {
                case 0:
                    $('input#button_delete_company').fadeIn();
                    $('input#button_activate_company').fadeIn();
                    $('input#button_deactivate_company').fadeOut();
                    break;
                case 1:
                    $('input#button_deactivate_company').fadeIn();
                    $('input#button_delete_company').fadeOut();
                    $('input#button_activate_company').fadeOut();
                    break;
                case 2:
                    $('input#button_delete_company').fadeOut();
                    $('input#button_activate_company').fadeOut();
                    $('input#button_deactivate_company').fadeOut();
                    break;
            }
        }
        else {
            $('input#button_delete_company').fadeOut();
            $('input#button_activate_company').fadeOut();
            $('input#button_deactivate_company').fadeOut();
            $('input#check_all_company').attr('checked', false);
        }

        return false;
    }

    function DeleteCompany() {
        var companiesIds = new Array();
        $("#button_delete_company").addClass("Trans");

        $('input[name=company_checkbox]').each(function () {
            if (this.checked) {
                var companyId = $(this).attr('id');
                companiesIds.push(companyId);
            }
        });

        $("div#modal-dialog").dialog({
            open: function (event, ui) {
                $("div#modal-dialog").html('<%=ViewResources.SharedStrings.CommonConfirmMessage %>');
            },
            resizable: false,
            width: 240,
            height: 140,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.CompaniesDeletingTitle %>',
            buttons: {
                '<%=ViewResources.SharedStrings.BtnOk %>': function () {
                    $.ajax({
                        type: "Post",
                        url: "/Company/Delete",
                        data: { companiesIds: companiesIds },
                        traditional: true,
                        success: function (result) {

                            $("#button_delete_company").removeClass("Trans");
                            if (result.IsSucceed) {
                                ShowDialog('<%=ViewResources.SharedStrings.CompaniesDeletingMessage %>', 2000, true);
                                setTimeout(function () { SubmitCompanySearch(); }, 1000);
                            }
                            else {
                                ShowDialog(result.Msg, 5000);
                            }
                        }
                    });
                    $(this).dialog("close");
                },
                '<%=ViewResources.SharedStrings.BtnCancel %>': function () { $(this).dialog("close"); $("#button_delete_company").removeClass("Trans"); }
            }
        });

        return false;
    }

    function ActivateCompany() {
        var companiesIds = new Array();
        $("#button_activate_company").addClass("Trans");

        $('input[name=company_checkbox]').each(function () {
            if (this.checked) {
                var companyId = $(this).attr('id');
                companiesIds.push(companyId);
            }
        });

        $("div#modal-dialog").dialog({
            open: function (event, ui) {
                $("div#modal-dialog").html('<%=ViewResources.SharedStrings.CommonConfirmMessage %>');
            },
            resizable: false,
            width: 240,
            height: 140,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.CompaniesActivatingTitle %>',
            buttons: {
					'<%=ViewResources.SharedStrings.BtnOk %>': function () {
                    $(this).dialog("close");
                    $("div#modal-dialog").dialog({
                        open: function () {
                            $("div#modal-dialog").html("");
                            $.get('/Company/Activate', {}, function (html) {
                                $("div#modal-dialog").html(html);
                            });
                        },
                        resizable: false,
                        width: 440,
                        height: 180,
                        modal: true,
                        title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.CompaniesActivatingTitle %>',
                        buttons: {
						  '<%=ViewResources.SharedStrings.BtnActivate %>': function () {
                                if ($('#selectedReasonId').val() == "") {
                                    ShowDialog("<%=ViewResources.SharedStrings.CommonNoReasonSelectedMessage %>", 2000);
                                  return false;
                              }
                              $(this).dialog("close");
                              ShowDialog('<%=ViewResources.SharedStrings.CompaniesActivatingMessage %>', 2000, true);
                                $.ajax({
                                    type: "Post",
                                    url: "/Company/Activate",
                                    data: { companiesIds: companiesIds, reasonId: $('#selectedReasonId').val() },
                                    traditional: true,
                                    success: function (result) {
                                        $("#button_deactivate_company").removeClass("Trans");
                                        setTimeout(function () { SubmitCompanySearch(); }, 1000);
                                    }
                                });
                            },
							'<%=ViewResources.SharedStrings.BtnCancel %>': function () { $(this).dialog("close"); $("#button_activate_company ").removeClass("Trans"); }
                        }
                    });
                },
                '<%=ViewResources.SharedStrings.BtnCancel %>': function () { $(this).dialog("close"); $("#button_activate_company").removeClass("Trans"); }
            }
        });

        return false;
    }

    function DeactivateCompany() {
        var companiesIds = new Array();
        $("#button_deactivate_company").addClass("Trans");

        $('input[name=company_checkbox]').each(function () {
            if (this.checked) {
                var companyId = $(this).attr('id');
                companiesIds.push(companyId);
            }
        });

        $("div#modal-dialog").dialog({
            open: function (event, ui) {
                $("div#modal-dialog").html('<%=ViewResources.SharedStrings.CompaniesDeactivateConfirmation %>');
            },
            resizable: false,
            width: 240,
            height: 160,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.CompaniesDeactivatingTitle %>',
            buttons: {
                '<%=ViewResources.SharedStrings.BtnOk %>': function () {
                    $(this).dialog("close");
                    $("div#modal-dialog").dialog({
                        open: function () {
                            $("div#modal-dialog").html("");
                            $.get('/Company/Deactivate', {}, function (html) {
                                $("div#modal-dialog").html(html);
                            });
                        },
                        resizable: false,
                        width: 440,
                        height: 180,
                        modal: true,
                        title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + '<%=ViewResources.SharedStrings.CompaniesDeactivatingTitle %>',
                        buttons: {
						  '<%=ViewResources.SharedStrings.BtnDeactivate %>': function () {
                                if ($('#selectedReasonId').val() == "") {
                                    ShowDialog("<%=ViewResources.SharedStrings.CommonNoReasonSelectedMessage %>", 2000);
                                  return false;
                              }
                              $(this).dialog("close");
                              ShowDialog('<%=ViewResources.SharedStrings.CompaniesDeactivatingCompaniesMessage %>', 2000, true);
                                $.ajax({
                                    type: "Post",
                                    url: "/Company/Deactivate",
                                    data: { companiesIds: companiesIds, reasonId: $('#selectedReasonId').val() },
                                    traditional: true,
                                    success: function (result) {
                                        $("#button_deactivate_company").removeClass("Trans");
                                        setTimeout(function () { SubmitCompanySearch(); }, 1000);
                                    }
                                });
                            },
							'<%=ViewResources.SharedStrings.BtnCancel %>': function () { $(this).dialog("close"); $("#button_deactivate_company ").removeClass("Trans"); }
                        }
                    });
                },
                '<%=ViewResources.SharedStrings.BtnCancel %>': function () { $(this).dialog("close"); $("#button_deactivate_company").removeClass("Trans"); }
            }
        });

        return false;
    }

    function CheckAll() {
        $('input[name=company_checkbox]').each(function () {
            this.checked = !this.checked;
        });

        ManageButtons(<%= Model.FilterCriteria %>);
        return false;
    }

    function EditCompany(Action, CompanyId, CompanyTitle) {
        debugger;
        $("div#modal-dialog").dialog({
            open: function () {
                $("div#modal-dialog").html("");
                $.get('/Company/Edit', { id: CompanyId }, function (html) {
                    $("div#modal-dialog").html(html);
                });
            },
            resizable: false,
            width: 640,
            height: 620,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + CompanyTitle,
            buttons: {
                '<%=ViewResources.SharedStrings.BtnSave %>': function () {
                    var selecteditems = [];

                    $("#company_list").find("input:checked").each(function (i, ob) {
                        selecteditems.push($(ob).val());
                    });

                    //By manoranjan
                    var selectedRoleItems = [];
                    $("#role_list").find("input:checked").each(function (i, ob) {
                        selectedRoleItems.push($(ob).val());
                    });
                    debugger;
                    
                    var editSerialize = $("#role_list").serialize();
                    $.ajax({
                        type: "Post",
                        url: "/Company/CompanieUserRoleSave",
                        data: { roleItems: selectedRoleItems,CompanyId: CompanyId },
                        success: function (response) {

                        }
                    });
                    //
                    dlg = $(this);
                    $.post("/Company/Update", $("#editCompany").serialize(),
                        function (response) {
                            if (response.IsSucceed) {
                                $.ajax({
                                    type: "POST",
                                    url: "/Company/SaveSubComapnyDetails",
                                    data: { compid: CompanyId, complist: selecteditems },
                                    async: false,
                                    success: function (result) {
                                    }
                                });
                                dlg.dialog("close");
                                ShowDialog(response.Msg, 2000, true);
                                setTimeout(function () { SubmitCompanySearch(); }, 1000);
                            }
                            else {
                                $("div#modal-dialog").html(response.viewData);
                            }
                        },
                        "json");
                },
                '<%=ViewResources.SharedStrings.BtnClose %>': function () {
                    $(this).dialog("close");
                }
            }
        });

        return false;
    }

</script>
