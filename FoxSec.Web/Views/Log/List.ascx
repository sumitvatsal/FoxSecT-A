<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.LogListViewModel>" %>
<%--<%@ Import Namespace="System.Data.SqlClient" %>--%>
<%--<%@ Import Namespace="System.Data" %>--%>
<%--<%@ Import Namespace="System.Windows" %>--%>
<%--<%@ Import Namespace="System.Windows.Forms" %>--%>
<style>
    #mask {
        position: fixed;
        left: 0;
        top: 0;
        bottom: 0;
        right: 0;
        margin: auto;
        visibility: hidden;
        z-index: -2;
        background: #000;
        background: rgba(0,0,0,0.8);
        overflow: hidden;
        opacity: 0;
        transition: all .5s ease-in-out;
    }

        #mask.showing {
            opacity: 1;
            z-index: 9000;
            visibility: visible;
            overflow: auto;
            transition: all .5s ease-in-out;
        }

    #boxes {
        display: table;
        width: 15%;
        height: 15%;
    }

    .window {
        /*max-width: 400px;*/
        z-index: 9999;
        padding: 20px;
        border-radius: 15px;
        text-align: center;
        margin: auto;
        background-color: #ffffff;
        font-family: 'Segoe UI Light', sans-serif;
        font-size: 15pt;
    }

        .window img {
            width: 100%;
            height: auto;
        }

    .inner {
        display: table-cell;
        vertical-align: middle;
    }

    #popupfoot {
        font-size: 16pt;
    }

    .showImage {
        margin: 0 0 3em;
        display: table;
        text-align: center
    }

        .showImage img {
            display: block
        }

    .hide-scroll, .hide-scroll body {
        overflow: hidden !important
    }
</style>

<div id="mask">
    <div align="center" style="margin-top:10%">
  <div id="boxes">
    <div class="inner">
      <div id="dialog" class="window"> 
      <%--  <div id="popupfoot"> <img src="http://wallpapercave.com/wp/Jp7kTmf.jpg" class="image" alt="Loading..."/> </div>--%>
            <div id="popupfoot"><img class="image" id="imgsrc"/> </div>
          <a href="#" class="close">CLOSE</a>
      </div>
    </div>
  </div>
        </div>
</div>

<%if (Model.Items.Count() > 0)
    { %>
<table id="searchedTableLog" cellpadding="1" cellspacing="0" style="margin: 0; width:auto; padding: 1px; border-spacing: 0;">
    <thead>
        <tr style="background-color:black;color:white">
            <%if (ViewBag.NotMoved != null && ViewBag.NotMoved)
                { %>
            <th style='width: 10px; padding: 0px;color:white'></th>
            <%} %>
            <th style='width: 18%; padding: 2px;color:white'>
                 From-To
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:LogSort(1,0);'></span></li>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:LogSort(1,1);'></span></li>
            </th>
            <th style='width: 9%; padding: 2px;color:white'>Building</th>
            <th style='width: 9%; padding: 2px;color:white'>
                Node
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:LogSort(3,0);'></span></li>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:LogSort(3,1);'></span></li>
            </th>
            <%if (!Model.User.IsCompanyManager)
                { %>
            <th style='width: 12%; padding: 2px;color:white'>
                Company
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:LogSort(4,0);'></span></li>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:LogSort(4,1);'></span></li>
            </th>
            <%} %>
            <th style='width: 12%; padding: 2px;color:white'>
                User
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-s' onclick='javascript:LogSort(5,0);'></span></li>
                <li class='ui-state-default ui-corner-all ui-icon-custom'><span class='ui-icon ui-icon-arrowthick-1-n' onclick='javascript:LogSort(5,1);'></span></li>
            </th>
            <th style='width: <%= Model.User.IsCompanyManager ? "48%" : "36%" %>; padding: 2px;color:white'>Activity</th>
              <% if (Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.LiveVideo))
                  {%> 
            <th align="right" style='width: 4%'>
                <li class='ui-state-default ui-corner-all ui-icon-custom' style="height: 18px; width: 18px;"><span class='ui-icon ui-icon-print' style='cursor: pointer; background-position: -161px -96px;' onclick="javascript:TogglePrintLog();"></span></li>
            </th>
             <%} %>
             <th style='width: 12%; padding: 2px;'>
                <li class='ui-state-default ui-corner-all ui-icon-custom' style="height: 18px; width: 18px;"><span class='ui-icon ui-icon-video' onclick='javascript:LogSort(4,0);'></span></li>
              
            </th>
            <th style='width: 12%; padding: 2px;'>
                <li class='ui-state-default ui-corner-all ui-icon-custom' style="height: 18px; width: 18px;"><span class='ui-icon ui-icon-image' onclick='javascript:LogSort(4,0);'></span></li>
              
            </th>
        </tr>
    </thead>
    <tbody id="myTable">
        <% foreach (var log in Model.Items)
            {
                var bg = string.Format("style='background-color:{0};'", log.LogRecordColor); %>
       
        <tr id="logRecord" <%= bg %>>
             <% if (ViewBag.NotMoved != null && ViewBag.NotMoved) { %>
                <td style='width: 30px; padding-right: 0px'><input class="deactivateUserCheckbox" style='width: 50px; padding-right: 0px' type="checkbox" id="<%= Html.Encode(log.UserId.Value) %>" value="<%= Html.Encode(log.UserId.Value) %>" <%if(log.IsUserDeleted){%>checked<%}%>/></td>  

           <% } %>
            <td style='width: 12%; padding-left: 0px;'>
                <%= Html.Encode(log.EventTimeStr)%>
            </td>
            <td style='width: 9%; padding: 2px;'>
                <%= Html.Encode(log.Building)%>
            </td>
            <td style='width: 9%; padding: 2px;'>
                <%= Html.Encode(log.Node)%>
            </td>
            <%if (!Model.User.IsCompanyManager)
                {%>
            <td style='<% if (log.IsCompanyDeleted) {%>text-decoration: line-through; <%}%>width: 12%; padding: 2px; <%= bg %>'>
                <%= Html.Encode(log.CompanyName) %>
            </td>
            <%} %>
            <td style='<% if (log.IsUserDeleted) {%>text-decoration: line-through; <%}%>width: 12%; padding: 2px; <%= bg %>'>
                <%= Html.Encode(log.UserName)%>
            </td>
            <td style='width: <%= Model.User.IsCompanyManager ? "54%" : "42%" %>; padding: 2px;'>
                <div id="shortAction">
                      <%if (log.LogTypeId != 15)
                          {%>
                    <%= Html.Encode((string.IsNullOrEmpty(log.Action) || log.Action.Length < 101) ? log.Action :
										log.Action.IndexOf('.') > 0 ?
													log.Action.IndexOf('.') < 100 ? log.Action.Substring(0, log.Action.IndexOf('.') + 1) : log.Action.Substring(0, 100)
													: log.Action.Substring(0, 100))%>
                     
                      <%}
                          else
                          { %>
                                       <%= Html.Encode((string.IsNullOrEmpty(log.Action) || log.Action.Length < 101) ? log.Action :
                                            log.Action.IndexOf(';') > 0 ?
                                                        log.Action.IndexOf(';') < 100 ? log.Action.Substring(0, log.Action.IndexOf(';') + 1) : log.Action.Substring(0, 100)
                                                        : log.Action.Substring(0, 100))%>
                    <%} %>
                     
                </div>
                <%:Html.Hidden("isShortDisplayed", "true") %>
                <div id="fullAction" style='display: none'>
                    <%= Html.Encode(log.Action) %>
                </div>
            </td>
          
            <% if (Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.LiveVideo))
             {%>  

            <td style='margin-right:-100PX;'>
                <span id="button" class='icon icon_green_go tipsy_we' original-title='OPEN VIDEO' onclick="fnselect('<%= Html.Encode(log.EventTimeStr)%>','<%= Html.Encode(log.Building)%>','<%= Html.Encode(log.Node)%>')" />

            </td><%}%>
          
              <td style='margin-right:-100PX;'>
                  <%--<button type="button" class="btn btn-info btn-lg" data-toggle="modal" data-target="#myModal">Open Small Modal</button>--%>
                <span id="button1" class='icon icon_green_go tipsy_we' original-title='OPEN PHOTO' onclick="fnselect1(<%= log.UserId %>)"/>

            </td>
              <td style='margin-right:-100PX;'>
                <span id='button_log_detail' class='icon icon_green_go tipsy_we' original-title='<%=ViewResources.SharedStrings.LogsFullShortAction %>' onclick="javascript:ViewLogDetail(this);"></span>

            </td>
          
          
        </tr>
        <% } %>
    </tbody>
    <tfoot>
        <tr>
            <td colspan="5">
                <% Html.RenderPartial("Paginator3", Model.Paginator); %>
            </td>
        </tr>
    </tfoot>
</table>
<% }
    else
    { %>
<table cellpadding="1" cellspacing="0" style="margin: 0; width: 100%; padding: 1px; border-spacing: 0;">
    <tr>
        <td><%=ViewResources.SharedStrings.CommonNoRecordsFound %>
        </td>
    </tr>
</table>
<% } %>


<script type="text/javascript" language="javascript">

    $(document).ready(function () {
        $(".tipsy_we").attr("class", function () {
            $(this).tipsy({ gravity: $.fn.tipsy.autoWE, html: true });
        });
    });


</script>


<script>
    $('.use-address').click(function () {
        var x = $(this).closest("tr").find('td').text();
        //  var x = $(this).text();
        var strarray = x.split("\n");
        for (var i = 0; i < strarray.length; i++) {
        }

        var datetime = (strarray[1]);
        var building = (strarray[3]);
        var node = (strarray[5]);
        var company = (strarray[7]);
        var name = (strarray[9]);
        var worktime = (strarray[16]);

        var datetime1 = datetime.trim();
        var worktime1 = worktime.trim();

        //date section 
        var datetime = datetime1;
        var date = datetime.split(' ')[0];
        var newdate = date.split(".").reverse();
        var finaldate = newdate[0] + newdate[1] + newdate[2];
        var lefwid = 0;
        var timeString = datetime1.split("-").join("");

        $.ajax({
            type: "POST",
            url: "/Log/GetDetail",
            data: "BuildingName=" + building + "&BuildingObject=" + node + "&timeString=" + timeString,
            datatype: "json",

            success: function (data) {

                if (data == "Records Not Matched") {
                    confirmVMR1("Records Not Matched", "", "", "", "", "", "", "");
                }
                if (data.length == "0") {
                    confirmVMR("Video No Availiable For This User", "", "", "", "", "", "", "");
                    //alert("Camera Not Assign In This Click");
                    return false;
                }

                var i = 0;
                var j = 0;
                var l = 0;
                var m = 0;
                var n = 0;
                var w = 0;
                var h = 0;
                var cam = 0;
                var listOfObjects = [];

                var popups = new Array();

                for (i = 0; i <= data.length - 1; i += 10) {

                    var camera = data[i];

                    var port = data[i + 1];
                    if (port == "") {
                        port = "8000";
                    }
                    var starttime = data[i + 2];
                    var playtime = data[i + 3];
                    var width = data[i + 4];
                    if (width == "") {
                        width = "480";
                    }

                    var height = data[i + 5];
                    if (height == "") {
                        height = "640";
                    }
                    var cameranr = data[i + 6];
                    var timediiference = data[i + 7];
                    var IP = data[i + 8];
                    var servern = data[i + 9];
                    var uname = data[i + 10];
                    var password = data[i + 11]


                    str = datetime1.split("-").join("");

                    //  var onlytime = timeString.split(' ')[2]

                    //   var startTime = new Date(timeString);
                    var startTimde = parseDate(timeString);
                    //    alert(startTimde.getTime());



                    startTimde.setSeconds(startTimde.getSeconds() - starttime);

                    var dt = startTimde.toTimeString();
                    str1 = dt.split(" ");
                    var bb = str1[0];

                    var idx = bb.indexOf(" ");
                    var timeStr = bb.substr(idx + 1);
                    var finaltime = timeStr.replace(/[^0-9\.]+/g, "");

                    lefwid += 30;
                    var browserleft = parseInt(300) + parseInt(lefwid);
                    var res = finaltime.substring(0, 2);

                    var tt = parseInt(timediiference) / 60;

                    var tot = res - tt;
                    var findlenth = tot.toString();

                    var len = findlenth.length;
                    var hour = "";
                    if (len == 1) {
                        hour = "0" + tot;
                    }
                    else {
                        hour = tot;
                    }

                    var remainingfour = finaltime.substring(2, 7);

                    var newfinaltime = hour + remainingfour;
                    newfinaltime = data[i + 12];
                    finaldate = data[i + 13];

                    var seconds = playtime * 1000;

                    // http://" + Uname + ":" + Password + "@" + IP
                    var quaterurl = "http://" + uname + ":" + password + "@" + IP + ":" + port + "/archive/media/" + servern + "/DeviceIpint.";
                    //var quaterurl = "http://" + IP + ":" + port + "/archive/media/" + servern + "/DeviceIpint.";
                    var halfurl = quaterurl + cameranr + "/SourceEndpoint.video:0:0/";
                    var mainurl = halfurl + finaldate + "T" + newfinaltime + "." + "000" + "?speed=1";
                    //if (i < 16) {



                    var win = window.open(mainurl, i, "toolbar=no, location=no,status=yes,menubar=no,scrollbars=yes,resizable=no, width=" + width + ",height=" + height + ",left=" + browserleft + ",top=100");
                    popups.push(win);
                    for (k = 0; k < popups.length; k++) {
                        setTimeout(function () { popups[0].close(); }, seconds);
                        setTimeout(function () { popups[1].close(); }, seconds);
                        setTimeout(function () { popups[2].close(); }, seconds);
                        setTimeout(function () { popups[3].close(); }, seconds);
                        setTimeout(function () { popups[4].close(); }, seconds);
                        setTimeout(function () { popups[5].close(); }, seconds);
                        setTimeout(function () { popups[6].close(); }, seconds);
                        setTimeout(function () { popups[7].close(); }, seconds);

                        // popups[k].close();

                        break;
                    }


                    // }

                }
            }
        });


    });
    

    $(".deactivateUserCheckbox").click(function () {
        //debugger;
        if ($(".deactivateUserCheckbox").is(":checked")) {
            var button = "<input type='button' id='deactivateUser' value='Deactivate User' onclick='DeactivateUser()'  class='ui-button ui-widget ui-state-default ui-corner-all' role='button'>";
            $("#deactivateButton").html(button);


        }
        else {
          
             $("#deactivateButton").empty();
        }
    });

    function DeactivateUser() {
        //debugger;
        var checkedUserId = [];
    
        $(".deactivateUserCheckbox").each(function (checked) {
            var thisKey = $(this);
            var thiskeyValue = thisKey[0];
            if ((this).checked) {
                checkedUserId.push(this.id);
            }
            
        });
        //checkedUserId.forEach(function (item) {
        //    alert(item)
        //});
        var log_page = 0;
        var log_rows = 50;
        var log_field = 1;
        var log_direction = 1;
        $.ajax({
            type: "POST",
            url: "/Location/NotMovedAfterToDate",
            data: { usersToDeactivate: checkedUserId },
            beforeSend: function () {
                $("#loading").removeAttr("hidden");
            },
            success: function () {
                
                $.ajax({
                    type: "Get",
                    url : "/Log/LocationList?" + $("#locFilters").serialize() + "&nav_page=" + log_page + "&rows=" + log_rows + "&sort_field=" + log_field + "&sort_direction=" + log_direction,
                     beforeSend: function () {
                    $("#button_submit_log_search").addClass("Trans");
                    //$("div#AreaLogSearchResults").fadeOut('fast', function () { $("div#AreaLogSearchResultsWait").fadeIn('slow'); });
                },
                    success: function (result) {
                        $("#loading").attr("hidden",true);
                    $("#deactivateButton").empty();
                    ShowDialog("Success.", 3000, true);
                    $("div#AreaLogSearchResultsWait").hide();
                    $("div#AreaLogSearchResults").html(result);
                    $("div#AreaLogSearchResults").fadeIn('fast');
                    $("#button_submit_log_search").removeClass("Trans");
                }
                });
            }
        });
    }

    function fnselect(evnttime, buildingn, noden) {
        var x = $(this).closest("tr").find('td').text();
        //  var x = $(this).text();
        var strarray = x.split("\n");
        for (var i = 0; i < strarray.length; i++) {
        }

        var datetime = evnttime;
        var building = buildingn;
        var node = noden;

        var datetime1 = datetime.trim();

        //date section 
        var datetime = datetime1;
        var date = datetime.split(' ')[0];
        var newdate = date.split(".").reverse();
        var finaldate = newdate[0] + newdate[1] + newdate[2];
        var lefwid = 0;
        var timeString = datetime1.split("-").join("");

        $.ajax({
            type: "POST",
            url: "/Log/GetDetail",
            data: "BuildingName=" + building + "&BuildingObject=" + node + "&timeString=" + timeString,
            datatype: "json",

            success: function (data) {

                if (data == "Records Not Matched") {
                    confirmVMR1("Records Not Matched", "", "", "", "", "", "", "");
                }
                if (data.length == "0") {
                    confirmVMR("Video No Availiable For This User", "", "", "", "", "", "", "");
                    //alert("Camera Not Assign In This Click");
                    return false;
                }

                var i = 0;
                var j = 0;
                var l = 0;
                var m = 0;
                var n = 0;
                var w = 0;
                var h = 0;
                var cam = 0;
                var listOfObjects = [];

                var popups = new Array();

                for (i = 0; i <= data.length - 1; i += 10) {

                    var camera = data[i];

                    var port = data[i + 1];
                    if (port == "") {
                        port = "8000";
                    }
                    var starttime = data[i + 2];
                    var playtime = data[i + 3];
                    var width = data[i + 4];
                    if (width == "") {
                        width = "480";
                    }

                    var height = data[i + 5];
                    if (height == "") {
                        height = "640";
                    }
                    var cameranr = data[i + 6];
                    var timediiference = data[i + 7];
                    var IP = data[i + 8];
                    var servern = data[i + 9];
                    var uname = data[i + 10];
                    var password = data[i + 11]


                    str = datetime1.split("-").join("");

                    //  var onlytime = timeString.split(' ')[2]

                    //   var startTime = new Date(timeString);
                    var startTimde = parseDate(timeString);
                    //    alert(startTimde.getTime());



                    startTimde.setSeconds(startTimde.getSeconds() - starttime);

                    var dt = startTimde.toTimeString();
                    str1 = dt.split(" ");
                    var bb = str1[0];

                    var idx = bb.indexOf(" ");
                    var timeStr = bb.substr(idx + 1);
                    var finaltime = timeStr.replace(/[^0-9\.]+/g, "");

                    lefwid += 30;
                    var browserleft = parseInt(300) + parseInt(lefwid);
                    var res = finaltime.substring(0, 2);

                    var tt = parseInt(timediiference) / 60;

                    var tot = res - tt;
                    var findlenth = tot.toString();

                    var len = findlenth.length;
                    var hour = "";
                    if (len == 1) {
                        hour = "0" + tot;
                    }
                    else {
                        hour = tot;
                    }

                    var remainingfour = finaltime.substring(2, 7);

                    var newfinaltime = hour + remainingfour;
                    newfinaltime = data[i + 12];
                    finaldate = data[i + 13];

                    var seconds = playtime * 1000;

                    // http://" + Uname + ":" + Password + "@" + IP
                    var quaterurl = "http://" + uname + ":" + password + "@" + IP + ":" + port + "/archive/media/" + servern + "/DeviceIpint.";
                    //var quaterurl = "http://" + IP + ":" + port + "/archive/media/" + servern + "/DeviceIpint.";
                    var halfurl = quaterurl + cameranr + "/SourceEndpoint.video:0:0/";
                    var mainurl = halfurl + finaldate + "T" + newfinaltime + "." + "000" + "?speed=1";
                    //if (i < 16) {



                    var win = window.open(mainurl, i, "toolbar=no, location=no,status=yes,menubar=no,scrollbars=yes,resizable=no, width=" + width + ",height=" + height + ",left=" + browserleft + ",top=100");
                    popups.push(win);
                    for (k = 0; k < popups.length; k++) {
                        setTimeout(function () { popups[0].close(); }, seconds);
                        setTimeout(function () { popups[1].close(); }, seconds);
                        setTimeout(function () { popups[2].close(); }, seconds);
                        setTimeout(function () { popups[3].close(); }, seconds);
                        setTimeout(function () { popups[4].close(); }, seconds);
                        setTimeout(function () { popups[5].close(); }, seconds);
                        setTimeout(function () { popups[6].close(); }, seconds);
                        setTimeout(function () { popups[7].close(); }, seconds);

                        // popups[k].close();

                        break;
                    }


                    // }

                }
            }
        });
    }

    //     $('.use-address').click(function () {
    //        var x = $(this).closest("tr").find('td').text();


    //            //  var x = $(this).text();

    //            var strarray = x.split("\n");


    //            for (var i = 0; i < strarray.length; i++) {

    //                // alert(strarray[i])


    //            }

    //            var datetime = (strarray[1]);
    //            var building = (strarray[3]);
    //            var node = (strarray[5]);
    //            var company = (strarray[7]);
    //            var name = (strarray[9]);
    //            var worktime = (strarray[16]);



    //            var datetime1 = datetime.trim();

    //            var worktime1 = worktime.trim();



    //            //date section 
    //            var datetime = datetime1;
    //            var date = datetime.split(' ')[0];
    //            var newdate = date.split(".").reverse();
    //            var finaldate = newdate[0] + newdate[1] + newdate[2];
    //            var lefwid = 0;

    //            //time section-(working  )

    //            //var idx = worktime1.indexOf(" ");
    //            //var timeStr = worktime1.substr(idx + 1);
    //        //var finaltime = timeStr.replace(/[^0-9\.]+/g, "");


    //        ////////////////////////////////////////////////////////////////////////////will move in database section for video time loop below code



    ///////////////////////////////////////////////////////////////////////////

    //            $.ajax({
    //                type: "POST",
    //                url: "/Log/GetDetail1",
    //                data: "BuildingName=" + building + "&BuildingObject=" + node,
    //                datatype: "json",

    //                success: function (data) {


    //                    if (data == "Records Not Matched") {
    //                        confirmVMR1("Records Not Matched", "", "", "", "", "", "", "");
    //                    }
    //                    if (data.length == "0")
    //                    {
    //                        confirmVMR("Video No Availiable For This User", "", "", "", "", "", "", "");
    //                        //alert("Camera Not Assign In This Click");
    //                        return false;
    //                    }

    //                    var i = 0;
    //                    var j = 0;
    //                    var l = 0;
    //                    var m = 0;
    //                    var n = 0;
    //                    var w = 0;
    //                    var h = 0;
    //                    var cam = 0;
    //                    var listOfObjects = [];

    //                    var popups = new Array();

    //                      for (i = 0; i <= data.length - 1; i += 10) {

    //                          var camera = data[i];

    //                          var port = data[i+1];
    //                          if (port == "")
    //                          {
    //                              port = "8000";
    //                          }
    //                          var starttime = data[i + 2];
    //                          var playtime = data[i + 3];
    //                          var width = data[i+4];
    //                              if (width == "")
    //                              {
    //                                  width = "480";
    //                              }

    //                          var height = data[i+5];
    //                              if (height == "") {
    //                                  height = "640";
    //                              }
    //                              var cameranr = data[i + 6];
    //                              var timediiference = data[i + 7];
    //                              var IP = data[i + 8];
    //                              var servern = data[i + 9];
    //                              var uname = data[i + 10];
    //                          var password=data[i+11]


    //                        str = datetime1.split("-").join("");

    //                        var timeString = str;

    //                      //  var onlytime = timeString.split(' ')[2]

    //                          //   var startTime = new Date(timeString);
    //                        var startTimde = parseDate(timeString);
    //                    //    alert(startTimde.getTime());



    //                          startTimde.setSeconds(startTimde.getSeconds() - starttime);

    //                          var dt = startTimde.toTimeString();
    //                        str1 = dt.split(" ");
    //                        var bb = str1[0];

    //                        var idx = bb.indexOf(" ");
    //                        var timeStr = bb.substr(idx + 1);
    //                        var finaltime = timeStr.replace(/[^0-9\.]+/g, "");

    //                        lefwid += 30;
    //                        var browserleft = parseInt(300) + parseInt(lefwid);
    //                        var res = finaltime.substring(0, 2);

    //                         var tt =parseInt(timediiference) / 60;

    //                          var tot = res - tt;
    //                          var findlenth = tot.toString();

    //                          var len = findlenth.length;
    //                          var hour = "";
    //                          if (len ==1)
    //                          {
    //                              hour = "0" + tot;
    //                          }
    //                          else
    //                          {
    //                              hour = tot;
    //                          }

    //                          var remainingfour = finaltime.substring(2, 7);

    //                          var newfinaltime = hour + remainingfour;

    //                        var seconds = playtime * 1000;

    //                         // http://" + Uname + ":" + Password + "@" + IP
    //                              var quaterurl = "http://" + uname + ":" + password + "@" + IP + ":" + port + "/archive/media/" + servern + "/DeviceIpint.";
    //                        //var quaterurl = "http://" + IP + ":" + port + "/archive/media/" + servern + "/DeviceIpint.";
    //                        var halfurl = quaterurl + cameranr + "/SourceEndpoint.video:0:0/";
    //                        var mainurl = halfurl + finaldate + "T" + newfinaltime+ "." + "000" + "?speed=1";
    //                          //if (i < 16) {



    //                        var win = window.open(mainurl, i, "toolbar=no, location=no,status=yes,menubar=no,scrollbars=yes,resizable=no, width=" + width + ",height=" + height + ",left=" + browserleft + ",top=100");
    //                            popups.push(win);
    //                            for (k = 0; k < popups.length; k++) {
    //                                setTimeout(function () { popups[0].close(); }, seconds);
    //                                setTimeout(function () { popups[1].close(); }, seconds);
    //                                setTimeout(function () { popups[2].close(); }, seconds);
    //                                setTimeout(function () { popups[3].close(); }, seconds);
    //                                setTimeout(function () { popups[4].close(); }, seconds);
    //                                setTimeout(function () { popups[5].close(); }, seconds);
    //                                setTimeout(function () { popups[6].close(); }, seconds);
    //                                setTimeout(function () { popups[7].close(); }, seconds);

    //                             // popups[k].close();

    //                                break;
    //                            }


    //                         // }

    //                    }
    //                }
    //            });


    //    });


    function parseDate(input) {
        var parts = input.match(/(\d+)/g);
        // new Date(year, month [, date [, hours[, minutes[, seconds[, ms]]]]])
        return new Date(parts[0], parts[1] - 1, parts[2], parts[3], parts[4], parts[5]); // months are 0-based
    }

</script>


<script>
    function fnselect1(usrid) {
        if (usrid == undefined || usrid == null || usrid == "") {
            usrid = 0;
        }

        $.ajax({
            type: "POST",
            url: "/Log/GetphotoNew",
            data: "username=" + usrid,
            datatype: "json",
            success: function (data) {
                if (data == "Image Not Found Along This User") {
                    // alert(data);
                    confirmVMRphoto("No Photo Aviliable For This User", "", "", "", "", "", "", "");
                }
                var i = 0;
                var arr = [];
                arr.push(data);
                for (i = 0; i <= arr.length - 1; i++) {
                    var imgs = arr[i];
                    var base64_string = imgs;
                    var img = document.createElement("img");
                    var ImageSrc = base64_string; //<-- replace with your base64 image, i don't want to clog up the answer
                    //  var $img = $("<img/>");
                    var myImage = new Image;
                    myImage.src = "data:image/png;base64," + ImageSrc;
                    myImage.style.border = 'none';
                    myImage.style.outline = 'none';
                    myImage.style.position = 'fixed';
                    myImage.style.left = '100';
                    myImage.style.top = '100';
                    myImage.style.width = "250";
                    myImage.style.height = "250";
                    //myImage.height = '200';
                    //myImage.width = '175';
                    myImage.onload = function () {
                        //var newWindow = window.open("", i, "toolbar=no, location=no,status=yes,menubar=no,scrollbars=yes,resizable=no, width=250,height=250,left=300,top=100");
                        //newWindow.document.write(myImage.outerHTML);
                        //setTimeout(function () { newWindow.close(); }, 30000);
                        showImage("data:image/png;base64," + ImageSrc);
                        $('#mask').addClass('showing');
                        $('html').addClass('hide-scroll');
                    }
                }
            }
        });
    }

    $('.use-address1').click(function () {

        var x = $(this).closest("tr").find('td').text();
        // var x = $(this).text();

        var strarray = x.split("\n");

        for (var i = 0; i < strarray.length; i++) {
        }
        var company = (strarray[7]);
        var name = (strarray[9]);

        $.ajax({
            type: "POST",
            url: "/Log/Getphoto",
            data: "username=" + name + "&company=" + company,
            datatype: "json",
            success: function (data) {
                if (data == "Image Not Found Along This User") {
                    // alert(data);
                    confirmVMRphoto("No Photo Aviliable For This User", "", "", "", "", "", "", "");
                }

                var i = 0;
                var arr = [];
                arr.push(data);
                for (i = 0; i <= arr.length - 1; i++) {
                    var imgs = arr[i];
                    var base64_string = imgs;
                    var img = document.createElement("img");

                    var ImageSrc = base64_string; //<-- replace with your base64 image, i don't want to clog up the answer
                    //  var $img = $("<img/>");
                    var myImage = new Image;
                    myImage.src = "data:image/png;base64," + ImageSrc;
                    myImage.style.border = 'none';
                    myImage.style.outline = 'none';
                    myImage.style.position = 'fixed';
                    myImage.style.left = '0';
                    myImage.style.top = '0';
                    myImage.height = '200';
                    myImage.width = '175';
                    myImage.onload = function () {
                        var newWindow = window.open("", i, "toolbar=no, location=no,status=yes,menubar=no,scrollbars=yes,resizable=no, width=250,height=250,left=300,top=100");
                        newWindow.document.write(myImage.outerHTML);
                        setTimeout(function () { newWindow.close(); }, 30000);
                    }
                }
            }
        });
    });
</script>


<link href="../../css/sweetalert.css" rel="stylesheet" />
<script src="../../Scripts/sweetalert.min.js"></script>

<script type="text/javascript">

    function confirmVMR(title1, confirmMethodToCall, param1, param2, param3, param4, text1, confirmButtonText1) {
        //if (typeof text1 == "undefined") { text1 = ""; }
        //if (typeof confirmButtonText1 == "undefined") { confirmButtonText1 = "Confirm"; }
        //swal({
        //    title: title1,
        //    text: text1,
        //    type: "warning",
        //    showCancelButton: true,
        //    confirmButtonColor: '#dd1533',
        //    confirmButtonText: confirmButtonText1,
        //    closeOnConfirm: true,
        //    closeOnCancel: true
        //},
        //      function (isConfirm) {
        //          if (isConfirm) {
        //              confirmMethodToCall(param1, param2, param3, param4);
        //              //swal("Success!", "You have refused the invitation!", "success");
        //          }
        //          else {

        //          }
        //      });
        swal(
            'Oops...',
            'Video Not Available!'

        )
        //swal('Video Not Aviliable for This User')
    }



</script>

<script ="text/javascript">
    function confirmVMRphoto(title1, confirmMethodToCall, param1, param2, param3, param4, text1, confirmButtonText1) {
        swal(
            'Oops...',
            'Photo Not Available!')
    }

</script>

<script ="text/javascript">
    function confirmVMR1(title1, confirmMethodToCall, param1, param2, param3, param4, text1, confirmButtonText1) {
        swal(
            'Oops...',
            'Video Not Available!')
    }

</script>

<script>
    function showImage(fullPath) {
        $('#imgsrc').attr({
            'src': fullPath
        });

        //if close button is clicked
        $('.window .close').click(function (e) {
            //Cancel the link behavior
            e.preventDefault();
            $('#mask').removeClass('showing');
            $('html').removeClass('hide-scroll');
        });

    };

    $(".showImage").on("click", function () {
        showImage($(this).text());
        $('#mask').addClass('showing');
        $('html').addClass('hide-scroll');
    });

</script>