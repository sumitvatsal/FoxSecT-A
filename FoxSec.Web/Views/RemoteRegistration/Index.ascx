<%--<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.RoleEditViewModel>"%>--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.HomeViewModel>" %>
<%@ Import Namespace="FoxSec.DomainModel.DomainObjects" %>
<style type="text/css">
    #loader {
        position: fixed;
        left: 0px;
        top: 0px;
        width: 100%;
        height: 100%;
        z-index: 99;
        opacity: 0.8;
        filter: alpha(opacity=80);
        -moz-opacity: 0.8;
        background: url('../../img/hourglass.gif') 50% 50% no-repeat rgb(0, 0, 0);
        /* background: url('../../img/loader1.gif') 50% 50% no-repeat rgb(249,249,249);  */
    }
</style>
<br />
<br />
<div id="loader" style="display: none">
</div>

<table style="margin: 0; width: 100%; padding: 0; border-spacing: 1px;">
    <tr>
        <td style='width: 290px; vertical-align: top; text-align: center'>

            <div id='AreaPermissionTree' style='margin: 15px 15px 15px 0; padding: 10px; min-width: 260px; text-align: left'>
            </div>
        </td>
        <td style='vertical-align: top;'>

            <table style="margin: 0; width: 100%; padding: 0; border-spacing: 10px; font-size: 8px">
                <tr>
                    <td style="text-align: center">
                        <h4>
                            <label id="lblselbuildingobject" style="word-wrap: initial; text-wrap: normal; font-size: 18px"></label>
                        </h4>
                    </td>
                    <td style="text-align: center">
                        <label for='Search_company'><%:ViewResources.SharedStrings.UsersCompany %></label>
                    </td>
                    <td style="text-align: center">
                        <select name="CompanyId" id="CompanyId" style="width: 250px"></select>
                    </td>

                    <td align="center">
                        <span id='button_submit_people_search' class='icon icon_find tipsy_we' title='<%:ViewResources.SharedStrings.UsersSearchPeople %>'
                            onclick="javascript:GetZoneByObjectDetails();"></span>
                    </td>
                    <td>
                        <input type='button' id='btnatwork' value='<%=ViewResources.SharedStrings.Atwork %>' onclick='javascript: Atworkmulti();' style="display: none" />
                        <input type='button' id='btnleaving' value='<%=ViewResources.SharedStrings.Leaving %>' onclick='javascript: leavingmulti();' style="display: none" />
                    </td>
                </tr>

            </table>

            <br />
            <br />
            <div id='AreaLogSearchResultsWait' style='display: none; width: 100%; height: 620px; text-align: center'><span style='position: relative; top: 40%' class='icon loader'></span></div>
            <div id="PermissionTimeZoneList" style="display: none"></div>
            <br />
        </td>
    </tr>
</table>

&nbsp;
<script type="text/javascript" lang="javascript">
    var ButtonName = document.getElementById('button_change_object_time_zone');
    $(document).ready(function () {
        $('input#btnatwork').hide();
        $('input#btnleaving').hide();

        var i = $('#panel_owner li').index($('#permissionsTab'));

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
        GetCompanies();
        SetNoSelectedGUI();

        $('div#Work').fadeIn("slow");
    });
    function SetNoSelectedGUI() {
        $.get('/RemoteRegistration/GetTree', function (html) {
            $("div#AreaPermissionTree").html(html);
            $("div#AreaPermissionTree").attr("id", function () {
                $(this).corner("bevelfold");
            });
            $("ul#permissions").treeview({
                animated: "fast",
                persist: "location",
                collapsed: false,
                unique: true
            });
        });
        $("div#PermissionTimeZoneList").fadeOut(300);
        return false;
    }

    function CheckAll() {
        $('input[name=sel_checkbox]').each(function () {
            this.checked = !this.checked;
            //  alert($('input[name=checkbox_name]').attr('checked'));
        });

        return false;
    }

    function GetCompanies() {
        $.ajax({
            type: "Get",
            url: "/RemoteRegistration/GetCompanies",
            dataType: 'json',
            success: function (data) {
                if ($("select#CompanyId").size() > 0) {
                    $("select#CompanyId").html(data);
                }
            }
        });
        return false;
    }

    var glbselobid = "";
    function GetZoneByObject(Id, isRoom, name) {
        glbselobid = Id;
        $("label#lblselbuildingobject").html(name);

    }

    function GetZoneByObjectDetails() {
        var glbtxt = $("label#lblselbuildingobject").html();
        if (glbselobid == "" || glbselobid == null) {
            ShowDialog('<%=ViewResources.SharedStrings.SelectBuildingObject %>', 3000, false);
            return false;
        }
        else if (glbtxt == "" || glbtxt == null) {
            ShowDialog('<%=ViewResources.SharedStrings.SelectBuildingObject %>', 3000, false);
            return false;
        }

        var compid = $("#CompanyId").val();
        if (compid == "" || compid == null || compid == "0") {
            ShowDialog('<%=ViewResources.SharedStrings.SelectCompany %>', 3000, false);
            return false;
        }
        $("div#PermissionTimeZoneList").html("");
        $.ajax({
            type: "Get",
            url: "/RemoteRegistration/CheckObjectPermission",
            data: { id: glbselobid, compid: compid },
            beforeSend: function () {
                $("div#AreaLogSearchResultsWait").fadeIn('slow');
            },
            success: function (result) {
                $("div#AreaLogSearchResultsWait").hide();
                $("div#PermissionTimeZoneList").html(result);
                $("div#PermissionTimeZoneList").fadeIn(300);
                $('input#btnatwork').fadeIn();
                $('input#btnleaving').fadeIn();
            }
        });
    }

    function leavingmulti() {
        var usersIds = new Array();

        $('input[name=sel_checkbox]').each(function () {
            if (this.checked) {
                var userId = $(this).attr('id');
                usersIds.push(userId);
            }
        });
        if (usersIds.length == 0) {
            ShowDialog('<%=ViewResources.SharedStrings.SelectWorkers %>', 3000, false);
            return false;
        }
        $('input#btnatwork').attr("disabled", true);
        $('input#btnleaving').attr("disabled", true);

        $.ajax({
            type: "Post",
            url: "/User/SaveWorkLeavingUsers",
            data: { usersIds: usersIds, boid: 0, type: 2 },
            traditional: true,
            success: function () {
                ShowDialog('<%=ViewResources.SharedStrings.CommonSaving %>', 2000, true);
                $("div#PermissionTimeZoneList").html("");
                GetZoneByObjectDetails();
                $('input#btnatwork').removeAttr("disabled");
                $('input#btnleaving').removeAttr("disabled");
            }
        })
        return false;
    }
    function Atworkmulti() {
        var id = 0;
        var usersIds = new Array();

        var glbtxt = $("label#lblselbuildingobject").html();
        if (glbselobid == "" || glbselobid == null) {
            ShowDialog('<%=ViewResources.SharedStrings.SelectBuildingObject %>', 3000, false);
            return false;
        }
        else if (glbtxt == "" || glbtxt == null) {
            ShowDialog('<%=ViewResources.SharedStrings.SelectBuildingObject %>', 3000, false);
            return false;
        }
        $('input[name=sel_checkbox]').each(function () {
            if (this.checked) {
                var userId = $(this).attr('id');
                usersIds.push(userId);
            }
        });
        if (usersIds.length == 0) {
            ShowDialog('<%=ViewResources.SharedStrings.SelectWorkers %>', 3000, false);
            return false;
        }
        $('input#btnatwork').attr("disabled", true);
        $('input#btnleaving').attr("disabled", true);
        $.ajax({
            type: "Post",
            url: "/RemoteRegistration/SaveWorkLeavingUsers",
            data: { usersIds: usersIds, boid: glbselobid, type: 1 },
            traditional: true,
            success: function () {
                ShowDialog('<%=ViewResources.SharedStrings.CommonSaving %>', 2000, true);
                $("div#PermissionTimeZoneList").html("");
                GetZoneByObjectDetails();
                $('input#btnatwork').removeAttr("disabled");
                $('input#btnleaving').removeAttr("disabled");
            }
        })
        return false;
    }
</script>
