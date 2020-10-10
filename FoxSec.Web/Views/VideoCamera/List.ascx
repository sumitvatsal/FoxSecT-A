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
<table id="searchedTableUsers" cellpadding="1" cellspacing="0" style="margin: 0; width: 100%; padding: 1px; border-spacing: 0;">

    <thead>
        <tr>

            <th style='width: 9%; padding: 2px;'>
                <label for='Search_username'><%=ViewResources.SharedStrings.CameraName %></label><br />
                <input type="checkbox" id="cbx_expUserList_6" checked="checked" class="tipsy_we" original-title="Export this column" style="display: none" />
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:UsersssSort(6,0);'></span></li>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:UsersssSort(6,1);'></span></li>
            </th>

        </tr>
    </thead>
    <tbody>
        <% var i = 1; foreach (var user in Model.Users1)
            {
                var bg = (i++ % 2 == 1) ? "style='background-color:#CCC;'" : ""; %>
        <tr id="userListDataRow" <%= bg %>>
            <td id="cameraname2" style='width: 20%; padding: 2px;'>
                <%= Html.Encode(user.Name)%>
            </td>
            <td style='width: 5%; padding: 2px; text-align: right;'>
                <span id="button" class='icon icon_green_go tipsy_we' data-tooltip="OPEN VIDEO" title='OPEN VIDEO' original-title='OPEN VIDEO' onclick="fnselect('<%= Html.Encode(user.Name)%>')" />
                <%if (Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.SystemConfiguration))
                    {%>
                <span class='icon icon_green_go tipsy_we' data-tooltip="EDIT" title='EDIT' original-title='EDIT' onclick="EditCamera('<%= Html.Encode(user.Id)%>','<%= Html.Encode(user.Name)%>')" />
                <%}
                else
                { %>
              <%-- <span class='icon icon_green_go tipsy_we' data-tooltip="EDIT" title='EDIT' original-title='EDIT' aria-disabled="true"/>--%>
                <%} %>
            </td>
        </tr>
        <% } %>
    </tbody>
    <tfoot>
        <tr>
            <td colspan="5">
                <% Html.RenderPartial("Paginator2", Model.Paginator); %>
            </td>
        </tr>
    </tfoot>
</table>


<style>
    .hdn {
        display: none;
    }
</style>

<script>

    $(document).ready(function () {
        $("input:button").button();
    });

    $('.use-address').click(function () {
        var x = $(this).closest("tr").find('td').text();

        //  var x = $(this).text();

        var strarray = x.split("\n");


        for (var i = 0; i < strarray.length; i++) {

            // alert(strarray[i])


        }

        var camera1 = (strarray[1]).trim();
        var camera = (strarray[4]);



        if (camera1 == "") {
            var cid = camera;


        }
        else {
            var cid = camera1;

        }

        $.ajax({
            type: "POST",
            url: "/VideoCamera/GetDetail",
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

                // var halfurl = "http://"+IP+":"+Port+"/live/media/"+servername+"/DeviceIpint.";
                // var halfurl = "http://"+IP+"/live/media/FOXSECDEMO/DeviceIpint.";
                var fullurl = halfurl + cameranr + "/SourceEndpoint.video:0:0?w=2000&h=2000";


                var win = window.open(fullurl, "_blank", "toolbar=no, location=no,status=yes,menubar=no,scrollbars=yes,resizable=no, width=" + height + ",height=" + width + ",left=300,top=100");

                <%--$("div#modal-dialogLiveVideo").dialog({
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





                //  var win = window.open(fullurl, "_blank", "toolbar=no, location=no,status=yes,menubar=no,scrollbars=yes,resizable=no, width=" + height + ",height=" + width + ",left=300,top=100");

            }




        });


    });



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
            url: "/VideoCamera/Search",
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
    function HandleUsersssPaging(page, rows) {

        user_page = page;
        user_rows = rows;
        SubmitPeopleSearch();
        return false;
    }


    function HandleUsersssSoring(page, rows, field, direction) {
        user_page = page;
        user_rows = rows;
        user_field = field;
        user_direction = direction;
        SubmitPeopleSearch();
        return false;
    }
    function fnselect(name) {
        $.ajax({
            type: "POST",
            url: "/VideoCamera/GetDetail",
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

                // var halfurl = "http://"+IP+":"+Port+"/live/media/"+servername+"/DeviceIpint.";
                // var halfurl = "http://"+IP+"/live/media/FOXSECDEMO/DeviceIpint.";
                var fullurl = halfurl + cameranr + "/SourceEndpoint.video:0:0?w=2000&h=2000";

                var win = window.open(fullurl, "_blank", "toolbar=no, location=no,status=yes,menubar=no,scrollbars=yes,resizable=no, width=" + height + ",height=" + width + ",left=300,top=100");

                <%--$("div#modal-dialogLiveVideo").dialog({
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





                //  var win = window.open(fullurl, "_blank", "toolbar=no, location=no,status=yes,menubar=no,scrollbars=yes,resizable=no, width=" + height + ",height=" + width + ",left=300,top=100");

            }




        });
    }

    function loadAllFSVideoServers() {

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
    }



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
               // debugger;
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
