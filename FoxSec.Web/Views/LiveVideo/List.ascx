<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.LiveVideoListViewModel>" %>
<%if (Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.SystemConfiguration))
    {%>
<input type='button' id='btnaddcamera' value='<%=ViewResources.SharedStrings.AddNewCamera %>' onclick="javascript: addcamera();" />&nbsp;&nbsp;&nbsp;&nbsp;
    <input type='button' value='<%=ViewResources.SharedStrings.ManageVideoServer %>' onclick="javascript: managevideoserver();" />
<%}
    else
    { %>
<input type='button' value='<%=ViewResources.SharedStrings.AddNewCamera %>' disabled="disabled" />&nbsp;&nbsp;&nbsp;&nbsp;
    <input type='button' value='<%=ViewResources.SharedStrings.ManageVideoServer %>' disabled="disabled" />
<%} %>
<br />
<br />
<div ng-app="myApp" ng-controller="myCtrl">

    <table id="searchedTableUsers" cellpadding="1" cellspacing="0" style="margin: 0; width: 100%; padding: 1px; border-spacing: 0;">

        <%if (Session["allcamera"].ToString() == "All")%>
        <% {%>
        <% }
            else
            {%>
        <tr>
            <td>
                <h3>COMPANY NAME- <%= Html.Encode(Session["companyName"]) %></h3>
                <h5><%=Html.Hidden("hdncompid",Html.Encode(Session["comp_id"]) )%></h5>
            </td>
        </tr>
        <tr>
            <td>
                <h4>Camera:
            <select id='UserFilter'>
                <option value="0">Assigned</option>
                <option value="1">Unassigned</option>
            </select></h4>
            </td>
        </tr>
        <%}%>
        <thead>
            <tr>
                <th style='width: 9%; padding: 2px;'></th>
                <th style='width: 9%; padding: 2px;'>
                    <%--<label for='Search_username'>CAMERAS</label><br />--%>
                </th>
            </tr>
        </thead>
        <thead>
            <tr>
                <%if (Session["allcamera"].ToString() == "All")%>
                <% {%>
                <% }
                    else
                    {%>
                <th style='width: 2%; padding: 2px;'>
                    <% if (Model.FilterCriteria != 2)
                        {%>
                    <input id='check_all' name='check_all' type='checkbox' class='tipsy_we' original-title='Select all!' onclick="javascript: CheckAll();" />
                    <%}%>
                </th>
                <%}%>
                <th style='width: 9%; padding: 2px;'>
                    <input type="checkbox" id="cbx_expUserList_6" checked="checked" class="tipsy_we" original-title="Export this column" style="display: none" />
                    <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:LiveVideoSort(6,0);'></span></li>
                    <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:LiveVideoSort(6,1);'></span></li>
                </th>
            </tr>
        </thead>
        <tbody id="tblCamerAssign">
            <% var i = 1; foreach (var user in Model.Users)
                {
                    var bg = (i++ % 2 == 1) ? "style='background-color:#CCC;'" : ""; %>
            <tr id="userListDataRow" <%= bg %>>

                <%if (Session["allcamera"].ToString() == "All")%>
                <% {%>

                <% }
                    else
                    {%>
                <td style='width: 2%; padding: 2px;'>
                    <% if (Model.FilterCriteria != 2)
                        {%>
                    <input type='checkbox' id='<%= user.Id%>' val="" name='user_checkbox' onclick='javascript: ManageButtons(<%= Model.FilterCriteria %>);' />
                    <% } %>
                </td>
                <%}%>
                <%if (Session["allcamera"].ToString() == "All")%>
                <% {%>
                <% }
                    else
                    {%>
                <td class="" style='width: 9%; padding: 2px;'>
                    <%= Html.Encode(user.status)%>
                </td>
                <%}%>
                <td></td>
                <td class="cameraname" style='width: 9%; padding: 2px;'>
                    <%= Html.Encode(user.Name)%>
                </td>
                <td style='width: 5%; padding: 2px; text-align: right;'>
                    <span id="button" class='icon icon_green_go tipsy_we' original-title='OPEN CAMERA' onclick="fnselect('<%= Html.Encode(user.Name)%>')" />
                    <%if (Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.SystemConfiguration))
                        {%>
                    <span class='icon icon_green_go tipsy_we' data-tooltip="EDIT" title='EDIT' original-title='EDIT' onclick="EditCamera('<%= Html.Encode(user.Id)%>','<%= Html.Encode(user.Name)%>')" />
                    <%}
                        else
                        { %>
                    <%-- <span class='icon icon_green_go tipsy_we' data-tooltip="EDIT" title='EDIT' original-title='EDIT' aria-disabled="true"/>--%>
                    <%} %>
                </td>
                <td id="companyid" class="hdn" style='width: 18%; padding: 2px; display: none; visibility: hidden'>
                    <%= Html.Encode(Session["companyId"]) %>
                </td>
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
</div>
<style>
    .hdn {
        display: none;
    }
</style>
<script>
    var Id ='<%= Session["companyId"]%>'
</script>
<script type="text/javascript">
    $(document).ready(function () {
        $("input:button").button();
    });
        function EditCamera(id, camname) {

        $("div#modal-dialog").dialog({
            open: function () {
                $("div#modal-dialog").html("");
                $("div#modal-dialog").html("<div id='AreaUserEditWait' style='width: 90%; height:300px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                $.get('/LiveVideo/Edit', { id: id }, function (html) {
                    $("div#modal-dialog").html(html);
                    $('#Camera_Id').html(id);
                    $.ajax({
                        type: "POST",
                        url: "/LiveVideo/fetchFSVideoServers",
                        data: "{}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (r) {
                            //debugger;
                            var ddlCustomers = $("[id*=_ServerNr]");
                            ddlCustomers.empty().append('<option selected="selected" value="0">Please select</option>');
                            $.each(r, function () {
                                ddlCustomers.append($("<option></option>").val(this['Id']).html(this['Name']));
                            });

                        }
                    });
                    $.get('/LiveVideo/EditDetails', { id: id }, function (data) {
                        $('#_ServerNr').val(data["1"]);
                        $('#_CompanyNr').val(data["2"]);
                        $('#_Name').val(data["3"]);
                        $('#_Port').val(data["4"]);
                        $('#_ResX').val(data["5"]);
                        $('#_ResY').val(data["6"]);
                        $('#_Skip').val(data["7"]);
                        $('#_Delay').val(data["8"]);
                        var elc = data["9"];

                        if (elc == "True" || elc == "true") {
                            $('#_EnableLiveControls').val("1");
                        }
                        else if (elc == "False" || elc == "false") {
                            $('#_EnableLiveControls').val("2");
                        }
                        else {
                            $('#_EnableLiveControls').val("0");
                        }
                        $('#_QuickPreviewSeconds').val(data["10"]);
                    });

                });
            },
            resizable: false,
            width: 500,
            height: 420,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" + camname,
            buttons: {
                '<%=ViewResources.SharedStrings.BtnSave %>': function () {
                    var ServerNr = $('#_ServerNr').val();
                    if (ServerNr == "" || ServerNr == null || ServerNr == "0") {
                        ShowDialog( '<%=ViewResources.SharedStrings.EnterServerNr %>', 4000);
                        return false;
                    }
                    var Name = $('#_Name').val();
                    if (Name == "" || Name == null) {
                        ShowDialog( '<%=ViewResources.SharedStrings.EnterName %>', 4000);
                        return false;
                    }
                    var CameraNr = $('#_CompanyNr').val();
                    var Port = $('#_Port').val();
                    var ResX = $('#_ResX').val();
                    var ResY = $('#_ResY').val();
                    var Skip = $('#_Skip').val();
                    var Delay = $('#_Delay').val();
                    var QuickPreviewSeconds = $('#_QuickPreviewSeconds').val();
                    var EnableLiveControls = $('#_EnableLiveControls').val();

                    $.ajax({
                        type: "Post",
                        url: "/LiveVideo/SaveUpdateCameraDetails",
                        //dataType: 'json',
                        //traditional: true,
                        data: { Name: Name, ServerNr: ServerNr, CameraNr: CameraNr, Port: Port, ResX: ResX, ResY: ResY, Skip: Skip, Delay: Delay, QuickPreviewSeconds: QuickPreviewSeconds, EnableLiveControls: EnableLiveControls, Id: id, type: 2 },
                        success: function (data) {
                            if (data == "1") {
                                ShowDialog('<%=ViewResources.SharedStrings.CommonDataSavedMessage %>', 3000, true);
                                $("div#modal-dialog").html("");
                                $("div#modal-dialog").dialog("close");
                                SubmitPeopleSearch();
                            }
                            else {
                                ShowDialog(data, 5000);
                                $("div#modal-dialog").html("");
                                $("div#modal-dialog").dialog("close");
                            }
                        }
                    });
                },
                '<%=ViewResources.SharedStrings.BtnCancel %>': function () {
                    $("div#modal-dialog").html("");
                    $(this).dialog("close");
                }
            }
        });

        return false;
    }
    function addcamera() {
        $("div#modal-dialog").dialog({
            open: function () {
                $("div#modal-dialog").html("");
                $("div#modal-dialog").html("<div id='AreaUserEditWait' style='width: 90%; height:300px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                $.get('/LiveVideo/Create', function (html) {
                    $("div#modal-dialog").html(html);
                    $.ajax({
                        type: "POST",
                        url: "/LiveVideo/fetchFSVideoServers",
                        data: "{}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (r) {
                            //debugger;
                            var ddlCustomers = $("[id*=_ServerNr]");
                            ddlCustomers.empty().append('<option selected="selected" value="0">Please select</option>');
                            $.each(r, function () {
                                ddlCustomers.append($("<option></option>").val(this['Id']).html(this['Name']));
                            });

                        }
                    });
                });
            },
            resizable: false,
            width: 500,
            height: 420,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" +'<%=ViewResources.SharedStrings.AddNewCamera %>',
            buttons: {
                '<%=ViewResources.SharedStrings.BtnSave %>': function () {
                    var ServerNr = $('#_ServerNr').val();
                    if (ServerNr == "" || ServerNr == null || ServerNr == "0") {
                        ShowDialog( '<%=ViewResources.SharedStrings.EnterServerNr %>', 4000);
                        return false;
                    }
                    var Name = $('#_Name').val();
                    if (Name == "" || Name == null) {
                        ShowDialog( '<%=ViewResources.SharedStrings.EnterName %>', 4000);
                        return false;
                    }

                    var CameraNr = $('#_CompanyNr').val();
                    var Port = $('#_Port').val();
                    var ResX = $('#_ResX').val();
                    var ResY = $('#_ResY').val();
                    var Skip = $('#_Skip').val();
                    var Delay = $('#_Delay').val();
                    var QuickPreviewSeconds = $('#_QuickPreviewSeconds').val();
                    var EnableLiveControls = $('#_EnableLiveControls').val();

                    $.ajax({
                        type: "Post",
                        url: "/LiveVideo/SaveUpdateCameraDetails",
                        //dataType: 'json',
                        //traditional: true,
                        data: { Name: Name, ServerNr: ServerNr, CameraNr: CameraNr, Port: Port, ResX: ResX, ResY: ResY, Skip: Skip, Delay: Delay, QuickPreviewSeconds: QuickPreviewSeconds, EnableLiveControls: EnableLiveControls, Id: 0, type: 1 },
                        success: function (data) {
                            if (data == "1") {
                                ShowDialog('<%=ViewResources.SharedStrings.CommonDataSavedMessage %>', 3000, true);
                                $("div#modal-dialog").html("");
                                $("div#modal-dialog").dialog("close");
                                SubmitPeopleSearch();
                            }
                            else {
                                ShowDialog(data, 5000);
                                $("div#modal-dialog").html("");
                                $("div#modal-dialog").dialog("close");
                            }
                        }
                    });
                },
                '<%=ViewResources.SharedStrings.BtnCancel %>': function () {
                    $("div#modal-dialog").html("");
                    $(this).dialog("close");
                }
            }
        });
        return false;
    }

    function loadAllFSProjects() {

        $.ajax({
            type: "POST",
            url: "/LiveVideo/fetchFSProjects",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (r) {
                //debugger;
                var ddlProject = $("[id*=_Project]");
                ddlProject.empty().append('<option selected="selected" value="0">Please select</option>');
                $.each(r, function () {
                    ddlProject.append($("<option></option>").val(this['Id']).html(this['Name']));
                });

            }
        });
    }

    function managevideoserver() {
        $("div#modal-dialog").dialog({
            open: function () {
                $("div#modal-dialog").html("");
                $("div#modal-dialog").html("<div id='AreaUserEditWait' style='width: 90%; height:400px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                $.get('/LiveVideo/VideoServer', function (html) {
                    $("div#modal-dialog").html(html);
                    loadAllFSProjects();
                });
            },
            resizable: false,
            width: "50%",
            height: 550,
            modal: true,
            title: "<span class='ui-icon ui-icon-pencil' style='float:left; margin:1px 5px 0 0'></span>" +'<%=ViewResources.SharedStrings.ManageVideoServer %>',
            buttons: {
                
                '<%=ViewResources.SharedStrings.BtnCancel %>': function () {
                    $("div#modal-dialog").html("");
                    $(this).dialog("close");
                }
            }
        });
        return false;

    }
    $(function () {
        $("#UserFilter").change(function () {
            var selectedText = $(this).find("option:selected").text();
            var selectedValue = $(this).val();

            if (selectedText == "Assigned") {
                var Id ='<%= Session["companyId"]%>'
                if (Id == null || Id == "" || Id == undefined) {
                    Id = "0";
                }
                document.getElementById("button_deactivate_user").value = "Unassigned";
                $.ajax({
                    type: "Get",
                    url: "/LiveVideo/ByCompany",
                    data: { cameraId: 0, companyId: Id, status: selectedText },
                    beforeSend: function () {
                        $("#button_submit_people_search").fadeOut('fast');
                        $('input#button_delete_user').fadeOut();
                        $('input#button_activate_user').fadeOut();
                        $('input#button_deactivate_user').fadeOut();
                        $("div#AreaTabPeopleSearchResultsWait").fadeIn('fast');
                        // $("div#AreaTabPeopleSearchResults").fadeOut('fast', function () { $("div#AreaTabPeopleSearchResultsWait").fadeIn('slow'); });
                    },
                    success: function (result) {
                        $("div#AreaTabPeopleSearchResultsWait").hide();
                        $("div#AreaTabPeopleSearchResults").html(result);
                        $("div#AreaTabPeopleSearchResults").fadeIn('fast');
                        $("#button_submit_people_search").fadeIn('fast');
                        $("#UserFilter option[value='0']").attr("selected", "Assigned");
                    }
                });
                return false;
            }
            else if (selectedText == "Unassigned") {
                var Id = '<%= Session["companyId"]%>'
                if (Id == null || Id == "" || Id == undefined) {
                    Id = "0";
                }
                document.getElementById("button_deactivate_user").value = "Assigned";
                $.ajax({
                    type: "Get",
                    url: "/LiveVideo/ByCompany",
                    data: { cameraId: 0, companyId: Id, status: 'Unassigned' },
                    beforeSend: function () {
                        $("#button_submit_people_search").fadeOut('fast');
                        $("#UserFilter").val(2);
                        $('input#button_delete_user').fadeOut();
                        $('input#button_activate_user').fadeOut();
                        $('input#button_deactivate_user').fadeOut();
                        $("div#AreaTabPeopleSearchResultsWait").fadeIn('fast');
                        // $("div#AreaTabPeopleSearchResults").fadeOut('fast', function () { $("div#AreaTabPeopleSearchResultsWait").fadeIn('slow'); });
                    },
                    success: function (result) {
                        $("div#AreaTabPeopleSearchResultsWait").hide();
                        $("div#AreaTabPeopleSearchResults").html(result);
                        $("div#AreaTabPeopleSearchResults").fadeIn('fast');
                        $("#button_submit_people_search").fadeIn('fast');
                        $("#UserFilter option[value='1']").attr("selected", "Unassigned");
                    }
                });
                return false;
            }
            else {
                var Id = '<%= Session["companyId"]%>'
                if (Id == null || Id == "" || Id == undefined) {
                    Id = "0";
                }
                document.getElementById("button_deactivate_user").value = "Unassigned";
                $.ajax({
                    type: "Get",
                    url: "/LiveVideo/ByCompany",
                    data: { cameraId: 0, companyId: Id, status: 'All' },
                    beforeSend: function () {
                        $("#button_submit_people_search").fadeOut('fast');
                        $("#UserFilter").val(2);
                        $('input#button_delete_user').fadeOut();
                        $('input#button_activate_user').fadeOut();
                        $('input#button_deactivate_user').fadeOut();
                        $("div#AreaTabPeopleSearchResultsWait").fadeIn('fast');
                        // $("div#AreaTabPeopleSearchResults").fadeOut('fast', function () { $("div#AreaTabPeopleSearchResultsWait").fadeIn('slow'); });
                    },
                    success: function (result) {
                        $("div#AreaTabPeopleSearchResultsWait").hide();
                        $("div#AreaTabPeopleSearchResults").html(result);
                        $("div#AreaTabPeopleSearchResults").fadeIn('fast');
                        $("#button_submit_people_search").fadeIn('fast');
                        $("#UserFilter option[value='0']").attr("selected", "Assigned");
                    }
                });
                return false;
            }
        });
    });
</script>

<script>
    function selectoperation() {
        var skillsSelect = document.getElementById("UserFilter");
        var selectedText = skillsSelect.options[skillsSelect.selectedIndex].text;

        if (selectedText == "Assigned") {
            unassigncamera();
        }
        else if (selectedText == "Unassigned") {
            DeactivateUser();
        }
        else {
            DeactivateUser();
        }

        if (selectedText == "Assigned") {
            var Id ='<%= Session["companyId"]%>'
            if (Id == null || Id == "" || Id == undefined) {
                Id = "0";
            }
            document.getElementById("button_deactivate_user").value = "Unassigned";
            $.ajax({
                type: "Get",
                url: "/LiveVideo/ByCompany",
                data: { cameraId: 0, companyId: Id, status: selectedText },
                beforeSend: function () {
                    $("#button_submit_people_search").fadeOut('fast');
                    $('input#button_delete_user').fadeOut();
                    $('input#button_activate_user').fadeOut();
                    $('input#button_deactivate_user').fadeOut();
                    $("div#AreaTabPeopleSearchResultsWait").fadeIn('fast');
                    // $("div#AreaTabPeopleSearchResults").fadeOut('fast', function () { $("div#AreaTabPeopleSearchResultsWait").fadeIn('slow'); });
                },
                success: function (result) {
                    $("div#AreaTabPeopleSearchResultsWait").hide();
                    $("div#AreaTabPeopleSearchResults").html(result);
                    $("div#AreaTabPeopleSearchResults").fadeIn('fast');
                    $("#button_submit_people_search").fadeIn('fast');
                    $("#UserFilter option[value='0']").attr("selected", "Assigned");
                }
            });
            return false;
        }
        else if (selectedText == "Unassigned") {
            var Id = '<%= Session["companyId"]%>'
            if (Id == null || Id == "" || Id == undefined) {
                Id = "0";
            }
            document.getElementById("button_deactivate_user").value = "Assigned";
            $.ajax({
                type: "Get",
                url: "/LiveVideo/ByCompany",
                data: { cameraId: 0, companyId: Id, status: 'Unassigned' },
                beforeSend: function () {
                    $("#button_submit_people_search").fadeOut('fast');
                    $("#UserFilter").val(2);
                    $('input#button_delete_user').fadeOut();
                    $('input#button_activate_user').fadeOut();
                    $('input#button_deactivate_user').fadeOut();
                    $("div#AreaTabPeopleSearchResultsWait").fadeIn('fast');
                    // $("div#AreaTabPeopleSearchResults").fadeOut('fast', function () { $("div#AreaTabPeopleSearchResultsWait").fadeIn('slow'); });
                },
                success: function (result) {
                    $("div#AreaTabPeopleSearchResultsWait").hide();
                    $("div#AreaTabPeopleSearchResults").html(result);
                    $("div#AreaTabPeopleSearchResults").fadeIn('fast');
                    $("#button_submit_people_search").fadeIn('fast');
                    $("#UserFilter option[value='1']").attr("selected", "Unassigned");
                }
            });
            return false;
        }
        else {
            var Id = '<%= Session["companyId"]%>'
            if (Id == null || Id == "" || Id == undefined) {
                Id = "0";
            }
            document.getElementById("button_deactivate_user").value = "Unassigned";
            $.ajax({
                type: "Get",
                url: "/LiveVideo/ByCompany",
                data: { cameraId: 0, companyId: Id, status: 'All' },
                beforeSend: function () {
                    $("#button_submit_people_search").fadeOut('fast');
                    $("#UserFilter").val(2);
                    $('input#button_delete_user').fadeOut();
                    $('input#button_activate_user').fadeOut();
                    $('input#button_deactivate_user').fadeOut();
                    $("div#AreaTabPeopleSearchResultsWait").fadeIn('fast');
                    // $("div#AreaTabPeopleSearchResults").fadeOut('fast', function () { $("div#AreaTabPeopleSearchResultsWait").fadeIn('slow'); });
                },
                success: function (result) {
                    $("div#AreaTabPeopleSearchResultsWait").hide();
                    $("div#AreaTabPeopleSearchResults").html(result);
                    $("div#AreaTabPeopleSearchResults").fadeIn('fast');
                    $("#button_submit_people_search").fadeIn('fast');
                    $("#UserFilter option[value='0']").attr("selected", "Assigned");
                }
            });
            return false;
        }
    }
</script>

<%--button for unassign cameras list script start--%>
<script>

    function unassigncamera() {
        var usersIds = new Array();
        var cameraname = new Array();
        var uniqueNames = [];
        var uniqueNames1 = [];
        var k = $('#companyid').text();
        var v = k.trim();
        $("#button_deactivate_user").addClass("Trans");
        $('input[name=user_checkbox]').each(function () {

            if (this.checked) {
                var userId = $(this).attr('id');
                usersIds.push(userId);
                var k = $(this).parents('tr:first').find('td.cameraname').text();
                // var k= $('#cameraname').text() ;
                var v = k.trim();
                cameraname.push(v);
            }
            $.each(usersIds, function (i, el) {
                if ($.inArray(el, uniqueNames) === -1) uniqueNames.push(el);
            })
            $.each(cameraname, function (i, el) {
                if ($.inArray(el, uniqueNames1) === -1) uniqueNames1.push(el);
            })
        });

        $.ajax({
            type: "Post",
            url: "/LiveVideo/Unassigncamera",
            //  data: { usersIds: usersIds, reasonId : $('#selectedReasonId').val() },
            data: { uniqueNames: uniqueNames, companyid: k, uniqueNames1: uniqueNames1 },
            traditional: true,
            async: false,
            success: function (data) {

                // console.log(data);
                if (data != "") {
                    ShowDialog("already Unassign-" + data, 2000);
                    $("#button_deactivate_user").removeClass("Trans");
                }
                else {
                    ShowDialog('Unassign sucessfully', 2000, true);
                    //  $("#button_deactivate_user").removeClass("Trans");
                    $("#button_deactivate_user").removeClass("Trans");
                }
            }
        });
    }
</script>
<style>
    .myvideo {
        overflow: inherit !important;
    }

    .rwd-media {
        position: relative;
        width: 100%;
        height: 0;
        padding-bottom: 56.25%; /* 16:9 */
    }

        .rwd-media iframe,
        .rwd-media video {
            position: absolute;
            width: 100%;
            height: 100%;
        }

    body {
        background: #ddd;
        margin: 0;
    }

    .content {
        width: 50%;
        padding: 2em;
        background: #fff;
    }
</style>

<%--button for save cameras list script start--%>
<script>

    function DeactivateUser() {
        var usersIds = new Array();
        var cameraname = new Array();

        var uniqueNames = [];
        var uniqueNames1 = [];

        var cid = $("#hdncompid").val();
        //var k= $('#companyid').text() ;
        var k = $("#hdncompid").val();
        var v = k.trim();

        $("#button_deactivate_user").addClass("Trans");

        $('input[name=user_checkbox]').each(function () {

            if (this.checked) {
                var userId = $(this).attr('id');
                usersIds.push(userId);

                var k = $(this).parents('tr:first').find('td.cameraname').text();
                // var k= $('#cameraname').text() ;
                var v = k.trim();
                cameraname.push(v);
            }

            $.each(usersIds, function (i, el) {
                if ($.inArray(el, uniqueNames) === -1) uniqueNames.push(el);
            })

            $.each(cameraname, function (i, el) {
                if ($.inArray(el, uniqueNames1) === -1) uniqueNames1.push(el);
            })
        });

        $.ajax({
            type: "Post",
            url: "/LiveVideo/Deactivate",
            //  data: { usersIds: usersIds, reasonId : $('#selectedReasonId').val() },
            data: { uniqueNames: uniqueNames, companyid: k, uniqueNames1: uniqueNames1 },
            traditional: true,
            async: false,
            success: function (data) {
                // console.log(data);
                if (data != "") {
                    ShowDialog("already Assign-" + data, 2000);
                    $("#button_deactivate_user").removeClass("Trans");
                }
                else {
                    ShowDialog('Assign sucessfully', 2000, true);
                    //  $("#button_deactivate_user").removeClass("Trans");
                    $("#button_deactivate_user").removeClass("Trans");
                }
            }
        });
    }

    function SubmitPeopleSearch() {

        var compId = $("#companyid").html();
        //  var usecf=$("#UserFilter").val();
        //  alert($("#UserFilter").val());
        Username = $("input#Search_username").val();
        CardSer = $("input#Search_card_ser").val();
        CardDk = $("input#Search_card_dk").val();
        CardNo = $("input#Search_card_no").val();
        Comment = $("input#Comment").val();
        status = $("#UserFilter").val();
        if (status == "0") {
            status = "Assigned";
        }
        else {
            status = "Unassigned";
        }
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
            data: {
                comment: Comment, name: Username, cardSer: CardSer, cardDk: CardDk, cardCode: CardNo, company: Company, title: Title, filter: Filter, departmentId: DepartmentID, nav_page: user_page, rows: user_rows, sort_field: user_field, sort_direction: user_direction,
                countryId: tree_country_id, locationId: tree_location_id, buildingId: tree_building_id, companyId: tree_company_id, floorId: tree_floor_id, status: status
            },
            beforeSend: function () {
                $("#button_submit_people_search").fadeOut('fast');
                //  $("div#AreaTabPeopleSearchResults").fadeOut('fast', function () { $("div#AreaTabPeopleSearchResultsWait").fadeIn('slow'); });
            },
            success: function (result) {
                $("div#AreaTabPeopleSearchResultsWait").hide();
                $("div#AreaTabPeopleSearchResults").html(result);
                $("div#AreaTabPeopleSearchResults").fadeIn('fast');
                $("#button_submit_people_search").fadeIn('fast');//Unassigned
                status == "Assigned" ? $("#UserFilter").val("0") : $("#UserFilter").val("1");
                $("#UserFilter").val();
            }
        });
        return false;
    }
    function HandleLiveVideoPaging(page, rows) {
        user_page = page;
        user_rows = rows;
        SubmitPeopleSearch();
        return false;
    }


    function HandleLiveVideoSoring(page, rows, field, direction) {
        user_page = page;
        user_rows = rows;
        user_field = field;
        user_direction = direction;
        SubmitPeopleSearch();
        return false;
    }
</script>







<script>



    $('.use-address').click(function () {
        var x = $(this).closest("tr").find('td').text();
        //  var x = $(this).text();
        var camname = x.trim();

        var strarray = x.trim().split("\n");
        var myArrayNew = [];
        for (var i = 0; i < strarray.length; i++) {

            // alert(strarray[i])
            if (strarray[i] == null || strarray[i] == "") {
            }
            else {
                myArrayNew.push(strarray[i]);
            }
        }
        strarray = [];
        strarray = myArrayNew;

        alert(strarray.length);

        var camera1 = (strarray[3]).trim();
        var camera = (strarray[9]);

        if (camera1 == "") {
            var cid = camera;

        }
        else {
            var cid = camera1;
        }



        $.ajax({
            type: "POST",
            url: "/LiveVideo/GetDetail",
            data: "CameraName=" + cid,
            datatype: "json",
            success: function (data) {
                var cameraid = data[0];
                var IP = data[1];
                var Port = data[2];
                var height = data[3];
                var width = data[4];
                var cameranr = data[5];
                var servername = data[6];
                var Uname = data[7];
                var Password = data[8];
                if (height == "") {
                    height = "480";
                }
                if (width == "") {
                    width = "640"
                }

                if (Port == "") {
                    alert("There is no port no for this camera");
                    Port = "8000";
                }

                var halfurl = "http://" + Uname + ":" + Password + "@" + IP + ":" + Port + "/live/media/" + servername + "/DeviceIpint.";

                //var halfurl = "http://"+IP+":"+Port+"/live/media/"+servername+"/DeviceIpint.";
                // var halfurl = "http://"+IP+"/live/media/FOXSECDEMO/DeviceIpint.";
                var fullurl = halfurl + cameranr + "/SourceEndpoint.video:0:0?w=2000&h=2000";


                var win = window.open(fullurl, "_blank", "toolbar=no, location=no,status=yes,menubar=no,scrollbars=yes,resizable=no, width=" + height + ",height=" + width + ",left=300,top=100");
                <%-- $("div#modal-dialogLiveVideo").dialog({
                        open: function () {
                            $("div#modal-dialogLiveVideo").html("");
                            $("div#user-modal-dialog").html("");
    	                   
                            $("#modal-dialogLiveVideo").addClass("myvideo");
                            //$("div#modal-dialogLiveVideo").html("<iframe width=" + height + " id='divFrame' height=" + width + " src=" + fullurl + " style='width:100%'   frameborder='0' gesture='media' allow='encrypted-media' allowfullscreen></iframe>");
                            $("div#modal-dialogLiveVideo").html("<div class='rwd-media'><img width=" + height + " id='divFrame' height=" + width + " src=" + fullurl + " style='max-width:100%'   /></div>");

                        },
                        resizable: false,
                        width: height,
                        height: width,
                        modal: true,
                        title: "<%=string.Format("<span class={1}ui-icon ui-icon-pencil{1} style={1}float:left; margin:1px 5px 0 0{1} ></span>{0}","Live Stream", "'") %>",
                        buttons: {
    	                    '<%=ViewResources.SharedStrings.BtnClose %>': function () {
			                    $(this).dialog("close");
    		                    if (newUserCreated) {
    			                    setTimeout(function () { SubmitPeopleSearch(); }, 1000);
    		                    }
    	                    }
                        }
                });

              $(".ui-dialog").attr("style", "max-width: 90% !important; left: 5% !important; z-index:99999; top:10%;");--%>
                //  setTimeout(function () {alert("asdfafd") ;alert($("#divFrame").find("body").html()); }, 4000)






            }





        });


    });




</script>

<style>
    .myvideo {
        overflow: inherit !important;
    }
</style>

<script type="text/javascript" language="javascript">

    function ManageButtons(filter) {
        var any = 0;

        $('input[name=user_checkbox]').each(function () {
            if (this.checked) any++;

        });

        if ($("#selectedDepartment").size() > 0) {
            if ($("#selectedDepartment").val() != "" && any != 0) {
                if ($('input#button_move_users_to_departament').size() > 0) { $('input#button_move_users_to_departament').fadeIn(); }
            } else {
                if ($('input#button_move_users_to_departament').size() > 0) { $('input#button_move_users_to_departament').fadeOut(); }
            }
        }
        if (any != 0) {

            switch (filter) {
                case 0:

                    //$('input#button_delete_user').fadeIn();
                    //$('input#button_activate_user').fadeIn();
                    //$('input#button_deactivate_user').fadeOut();
                    $('input#button_deactivate_user').fadeIn();
                    $('input#button_delete_user').fadeOut();
                    $('input#button_activate_user').fadeOut();

                    break;
                case 1:
                    $('input#button_deactivate_user').fadeIn();
                    $('input#button_delete_user').fadeOut();
                    $('input#button_activate_user').fadeOut();
                    break;
                case 2:
                    $('input#button_delete_user').fadeOut();
                    $('input#button_activate_user').fadeOut();
                    $('input#button_deactivate_user').fadeOut();
                    break;
            }
        }
        else {
            $('input#button_delete_user').fadeOut();
            $('input#button_activate_user').fadeOut();
            $('input#button_deactivate_user').fadeOut();
            $('input#check_all').attr('checked', false);
        }
        return false;
    }
    function CheckAll() {

        $('input[name=user_checkbox]').each(function () {
            {
                this.checked = !this.checked;
                //  this.checked = true;
                // alert($('input[name=checkbox_name]').attr('checked'));
            }
        });

        ManageButtons(<%= Model.FilterCriteria %>);
        return false;
    }

    function fnselect(name) {
        $.ajax({
            type: "POST",
            url: "/LiveVideo/GetDetail",
            data: "CameraName=" + name,
            datatype: "json",
            success: function (data) {
                var cameraid = data[0];
                var IP = data[1];
                var Port = data[2];
                var height = data[3];
                var width = data[4];
                var cameranr = data[5];
                var servername = data[6];
                var Uname = data[7];
                var Password = data[8];
                if (height == "") {
                    height = "480";
                }
                if (width == "") {
                    width = "640"
                }

                if (Port == "") {
                    alert("There is no port no for this camera");
                    Port = "8000";
                }

                var halfurl = "http://" + Uname + ":" + Password + "@" + IP + ":" + Port + "/live/media/" + servername + "/DeviceIpint.";

                //var halfurl = "http://"+IP+":"+Port+"/live/media/"+servername+"/DeviceIpint.";
                // var halfurl = "http://"+IP+"/live/media/FOXSECDEMO/DeviceIpint.";
                var fullurl = halfurl + cameranr + "/SourceEndpoint.video:0:0?w=2000&h=2000";


                var win = window.open(fullurl, "_blank", "toolbar=no, location=no,status=yes,menubar=no,scrollbars=yes,resizable=no, width=" + height + ",height=" + width + ",left=300,top=100");
                <%-- $("div#modal-dialogLiveVideo").dialog({
                        open: function () {
                            $("div#modal-dialogLiveVideo").html("");
                            $("div#user-modal-dialog").html("");
    	                   
                            $("#modal-dialogLiveVideo").addClass("myvideo");
                            //$("div#modal-dialogLiveVideo").html("<iframe width=" + height + " id='divFrame' height=" + width + " src=" + fullurl + " style='width:100%'   frameborder='0' gesture='media' allow='encrypted-media' allowfullscreen></iframe>");
                            $("div#modal-dialogLiveVideo").html("<div class='rwd-media'><img width=" + height + " id='divFrame' height=" + width + " src=" + fullurl + " style='max-width:100%'   /></div>");

                        },
                        resizable: false,
                        width: height,
                        height: width,
                        modal: true,
                        title: "<%=string.Format("<span class={1}ui-icon ui-icon-pencil{1} style={1}float:left; margin:1px 5px 0 0{1} ></span>{0}","Live Stream", "'") %>",
                        buttons: {
    	                    '<%=ViewResources.SharedStrings.BtnClose %>': function () {
			                    $(this).dialog("close");
    		                    if (newUserCreated) {
    			                    setTimeout(function () { SubmitPeopleSearch(); }, 1000);
    		                    }
    	                    }
                        }
                });

              $(".ui-dialog").attr("style", "max-width: 90% !important; left: 5% !important; z-index:99999; top:10%;");--%>
                //  setTimeout(function () {alert("asdfafd") ;alert($("#divFrame").find("body").html()); }, 4000)






            }





        });
    }
</script>


