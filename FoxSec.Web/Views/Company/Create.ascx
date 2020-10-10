<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.CompanyEditViewModel>" %>
<style type="text/css">
    .TFtable {
        border-collapse: collapse;
    }

        .TFtable td {
            padding: 3px;
        }
        /* provide some minimal visual accomodation for IE8 and below */
        .TFtable tr {
            background-color: transparent;
        }
            /*  Define the background color for all the ODD background rows  */
            .TFtable tr:nth-child(odd) {
                background: white;
            }
            /*  Define the background color for all the EVEN background rows  */
            .TFtable tr:nth-child(even) {
                background: #CCC;
            }
</style>
<div id="content_add_company" style='margin: 10px; text-align: center; display: none'>
    <form id="createNewCompany" action="">
        <table width="100%">
            <tr>
                <td style='width: 30%; padding: 0 5px; text-align: right;'>
                    <label for='Add_company_title'><%:ViewResources.SharedStrings.CompaniesCompanyTitle %></label></td>
                <td style='width: 70%; padding: 0 5px;'>
                    <%=Html.TextBox("Company.Name", Model.Company.Name, new { @style = "width:80%;text-transform: capitalize;" })%>
                    <%= Html.ValidationMessage("Company.Name", null, new { @class = "error" })%>
                    <%=Html.HiddenFor(m=>m.Company.Id) %>
                </td>
            </tr>
            <tr>
                <td style='width: 30%; padding: 0 5px; text-align: right;'>
                    <label for='Add_company_info'><%:ViewResources.SharedStrings.CompaniesAdditionalInfo %></label></td>
                <td style='width: 70%; padding: 0 5px;'>
                    <%=Html.TextArea("Company.Comment", Model.Company.Comment, new { @style = "width:80%;height:50px;text-transform: capitalize;" })%>
                    <%= Html.ValidationMessage("Company.Comment", null, new { @class = "error" })%>
                </td>
            </tr>
            <tr>
                <td style='width: 30%; padding: 0 5px; text-align: right;'>
                    <label for='Add_can_use_cards'><%:ViewResources.SharedStrings.CompaniesCanUseOwnCards %></label></td>
                <td style='width: 70%; padding: 0 5px;'><%=Html.CheckBox("Company.IsCanUseOwnCards", Model.Company.IsCanUseOwnCards)%></td>
            </tr>
        </table>
        <div id="panel_company_edit">
            <ul>
                <li><a href="#tab_company_buildings"><%:ViewResources.SharedStrings.BuildingsTabName %></a></li>
                <li><a href="#tab_manage_other_company"><%:ViewResources.SharedStrings.ManageOtherCompany %></a></li>
            </ul>
            <div id="tab_company_buildings">
                <div id="buildings_list">
                    <% Html.RenderPartial("BuildingList", Model); %>
                </div>
            </div>
            <div id="tab_manage_other_company">
                <div id="company_list">
                    <%--<% Html.RenderPartial("CompanyList", Model.CompanyItems); %>--%>
                    <table cellpadding="3" cellspacing="3" style="margin: 0; width: 100%; padding: 3px; border-spacing: 3px;" class="TFtable">
                        <%if (Model.CompanyItems != null)
                            { %>
                        <%int index = 0; foreach (var cb in Model.CompanyItems)
                            { %>
                        <tr>
                            <td style="width: 20%">
                                <input type="checkbox" name="chkcomp" value="<%:Html.Encode(cb.Id) %>" />
                            </td>
                            <td>
                                <span><%:Html.Encode(cb.Name) %></span>
                            </td>
                        </tr>
                        <%index++;
                            } %>
                        <%  } %>
                    </table>
                </div>
            </div>
        </div>
    </form>
</div>

<script type="text/javascript" language="javascript">

    $(document).ready(function () {
        $("div#panel_company_edit").tabs({
            beforeLoad: function (event, ui) {
                ui.ajaxSettings.async = false,
                    ui.ajaxSettings.error = function (xhr, status, index, anchor) {
                        $(anchor.hash).html("Couldn't load this tab!");
                    }
            },
            fx: {
                opacity: "toggle",
                duration: "fast"
            },
            active: 0
        });

        $("div#content_add_company").fadeIn("300");

        CheckDeleteButtons();
        SetUpCompanyBuildingsNames();
    });

    function IsLast(id) {
        return $("#buildings_list > div").length == id;
    }

    function SaveNewCompanyFloors(id) {
        var floors = new Array();
        $("input[id^=floorCB_][type='checkbox']").each(function () {
            if ($(this).is(':checked') == true) {
                var s = (id + "_" + $(this).attr('id')).split("_");
                floors.push(s[0] + "." + s[2] + "." + s[3]);
            }
        });

        $.ajax({
            type: "POST",
            url: "/Company/SaveFloors",
            data: { Id: -1, values: floors },
            success: function (result) { },
            dataType: "json",
            traditional: true
        });
    }

</script>
