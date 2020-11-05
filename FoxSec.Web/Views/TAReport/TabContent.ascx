<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.HomeViewModel>" %>

<!-- manoranjan -->
<style type="text/css">
    #loading {
        position: fixed;
        left: 0px;
        top: 0px;
        width: 100%;
        height: 100%;
        z-index: 99;
        opacity: 0.8;
        filter: alpha(opacity=80);
        -moz-opacity: 0.8;
        background: url('../../img/loader7.gif') 50% 50% no-repeat rgb(0, 0, 0);
        /* background: url('../../img/loader1.gif') 50% 50% no-repeat rgb(249,249,249);  */
    }
</style>
<div id="loading" hidden></div>
<!-- manoranjan -->

<div id='panel_tab_jobs'>
    <form id="TASettings">
        <table style="margin: 0; width: 90%; padding: 0; border-spacing: 1px;">
            <col style="width: 10%">
            <%--   <col style="width: 25%">
            <col style="width: 25%">
            <col style="width: 30%">--%>
            <thead>
                <tr>
                    <th></th>
                    <th><%:ViewResources.SharedStrings.CommonDate %></th>
                    <%//if(Model.User.CompanyId == null )
                        if (Model.User.IsCompanyManager || Model.User.IsSuperAdmin)
                        {%>
                    <th style="padding-right: 100px;"><%:ViewResources.SharedStrings.UsersCompany %></th>
                    <%}%>
                    <%if (Model.User.CompanyId == null || Model.User.CompanyId != null)
                        {%>
                    <th style="padding-right: 50px;"><%:ViewResources.SharedStrings.Department %></th>
                    <%} %>
                    <th style="padding-right: 150px;">Building</th>
                    <th style="padding-right: 50px;">Format</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <th style="text-align: right"><%:ViewResources.SharedStrings.CommonFrom %></th>
                    <%-- <td><%:Html.TextBox("FromDateTA",  new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("dd.MM.yyyy"),new { @onchange = "onchangeDateFilter();" })%></td>
                    --%>               <%--  <td><%:Html.TextBox("FromDateTA",new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("dd.MM.yyyy"),new { @onchange = "onchangeDateFilter()" }) %></td>--%>
                    <td><%:Html.TextBox("FromDateTA", new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("dd.MM.yyyy"), new { @readonly = "readonly", @onchange = "onchangefromdate();" }) %></td>
                    <%//if(Model.User.CompanyId == null )
                        if (Model.User.IsCompanyManager || Model.User.IsSuperAdmin)
                        {%>
                    <td>
                        <select style="width: 90%" name="CompanyId" id="CompanyIdta" onchange="CompanyChanged()"></select>
                    </td>
                    <%}%>
                    <%if (Model.User.CompanyId == null || Model.User.CompanyId != null)
                        {%>
                    <td>
                        <select style="width: 90%" name="DepartmentId" id="DepartmentId"></select>
                        <%--onchange="showReportCompany()"--%>                       
                    </td>
                    <%}%>

                    <td style="margin-right: -80px">
                        <select style="width: 90%" name="BuildingId" id="BuildingId"></select>
                    </td>
                    <td>
                        <select style="width: 90%" name="Format" id="Format">
                            <option value="1">12:59</option>
                            <option value="2">12,99</option>
                        </select>
                        <%--onchange="showReportCompany()"--%>                       
                    </td>

                    <%:Html.HiddenFor(m => m.User.CompanyId, new { id = "hdnCompanyID" }) %>
                    <%:Html.HiddenFor(m => m.User.IsDepartmentManager, new { id = "hdnIsDM" }) %>
                    <%:Html.HiddenFor(m => m.User.IsCompanyManager, new { id = "hdnIsCM" }) %>
                </tr>
                <tr>
                    <th style="text-align: right"><%:ViewResources.SharedStrings.CommonTo %></th>
                    <td><%:Html.TextBox("ToDateTA", DateTime.Now.ToString("dd.MM.yyyy"), new { @readonly = "readonly", @onchange = "onchangefromdate();" }) %></td>
                    <td colspan="3">
                        <input type="button" onclick="chkDate();" name="click" id="click" value='<%:ViewResources.SharedStrings.previousmonth %>' />&nbsp;&nbsp;&nbsp;
                        <input type="button" onclick="chkcurrentDate();" name="click1" id="click1" value='<%:ViewResources.SharedStrings.currentmonth %>' />
                    </td>
                    <!-- By manoranjan -->
                    <% if ((Model.User.IsSuperAdmin || Model.User.IsCompanyManager) && (ViewBag.EdlusIntegrationExists == true))
                        {%>
                    <td colspan="3">
                        <input type="button" onclick="vedludbReportPost();" name="vedludbClick" id="vedludbClick" value="Send Report" />
                    </td>
                    <%} %>
                    <!-- -->
                    
                </tr>
            </tbody>
        </table>
        <br />
        <br />
        <script type="text/javascript">


            function chkDate() {

                $.ajax({
                    type: "POST",
                    url: "../TAReport/Checkdate/",
                    dataType: "json",
                    success: function (data) {
                        console.log(data);
                        //$.each(data, function (idx, value) {
                        $("#FromDateTA").val(data.som);
                        $("#ToDateTA").val(data.eom);
                        GetBuilding();
                        // });
                    }
                });
            }

            function onchangefromdate() {
                debugger;
                $.ajax({
                    type: "Get",
                    url: "/TAReport/GetBuildingsOnClick_FromDate",
                    dataType: 'json',
                    data: { datefrom: $("#FromDateTA").val(), dateto: $("#ToDateTA").val() },

                    success: function (data) {
                        if ($("select#BuildingId").size() > 0) {
                            $("select#BuildingId").html(data);
                        }
                    }
                });
                return false;
            }
        </script>
        <script>

            $(document).ready(function () {

                var compID = $("#hdnCompanyID").val();
                var isDM = $("#hdnIsDM").val();

                if (compID && isDM) {
                    $.ajax({
                        type: "Get",
                        url: "/Log/GetDepartmentsByCompany",
                        dataType: 'json',
                        data: { id: compID },//$('#CompanyId').val() },
                        success: function (data) {
                            if ($("select#DepartmentId").size() > 0) {
                                $("select#DepartmentId").html(data);
                            }
                        }
                    });
                }

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
            })


        </script>
        <script>
            $(document).ready(function () {

                $.ajax({
                    type: "Get",
                    url: "/TAReport/GetBuildings",
                    dataType: 'json',
                    data: { datefrom: $("#FromDateTA").val(), dateto: $("#ToDateTA").val() },

                    success: function (data) {
                        if ($("select#BuildingId").size() > 0) {
                            $("select#BuildingId").html(data);
                        }
                    }

                });
            })
        </script>

        <script type="text/javascript">
            function chkcurrentDate() {
                $.ajax({
                    type: "POST",
                    url: "../TAReport/chkcurrentDate/",
                    dataType: "json",
                    success: function (data) {
                        console.log(data);
                        //$.each(data, function (idx, value) {
                        $("#FromDateTA").val(data.som);
                        $("#ToDateTA").val(data.eom);
                        GetBuilding();
                        // });
                    }
                });
            }

        </script>

        <!-- By Manoranjan -->
        <script type="text/javascript">
            function getPostDataToVEDLUDB() {
                $.ajax({
                    type: "GET",
                    url: "../TAReport/SendReportToVedludb/",
                    success: function (data) {

                    }
                });
            }
        </script>
        <!-- -->


        <table style='width: 95%'>
            <col style="width: 20%">
            <col style="width: 70%">
            <col style="width: 5%">

            <thead>
                <tr>
                <tr>
                    <th style='padding: 2px; text-align: left'><%:ViewResources.SharedStrings.UsersName %></th>
                    <th style='padding: 2px; text-align: left'><%:ViewResources.SharedStrings.CommonDescription %></th>
                    <th style='padding: 2px;'></th>
                </tr>
            </thead>
            <tbody>
                <% var i = 1;
                    var bg = "";
                    var issuperadmin = Model.User.IsSuperAdmin;
                    var iscompanymanager = Model.User.IsCompanyManager;
                    var isdepartmentmanager = Model.User.IsDepartmentManager;
                    var canconf = ((Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewTAConfMenu)));
                    var canedit = ((Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewTAReportMenu)));%>

                <%if (issuperadmin)
                    {
                        bg = (i++ % 2 == 1) ? "style='background-color:#CCC;'" : ""; %>

                <%--<tr id="ta_boconf" <%= bg %>>
        <td style='padding:2px; text-align:left'> Entry  </td>
         <td style='padding:2px; text-align:left'> For T&ABuilding Names </td>
        <td style='padding:2px; text-align:right'><span id='button_TA_Bo' class='icon icon_green_go tipsy_we' title='<%= Html.Encode("Create Entry for Building Names") %>' onclick="CreateBuildingNames();" ></span></td>
 </tr> --%>
                <%-- <%  bg = (i++ % 2 == 1) ? "style='background-color:#CCC;'" : "";%>--%>
                 <tr id="ta_Schedule" style="background-color: #CCC" <%= bg %>>
                    <td style='padding: 2px; text-align: left'>TaMovement</td>
                    <td style='padding: 2px; text-align: left'>Add,Edit TaMovement</td>
                    <td style='padding: 2px; text-align: right'><span id='button_TA_Bo' class='icon icon_green_go tipsy_we' title='<%= Html.Encode("TAMovement Details") %>' onclick="TaMovement();"></span></td>
                </tr>

                 <tr id="ta_Schedule" style="background-color: #fff" <%= bg %>>
                    <td style='padding: 2px; text-align: left'>TAShift</td>
                    <td style='padding: 2px; text-align: left'>Add,Edit and Details of shifts</td>
                    <td style='padding: 2px; text-align: right'><span id='button_TA_Bo' class='icon icon_green_go tipsy_we' title='<%= Html.Encode("TAShift Details") %>' onclick="TaShift();"></span></td>
                </tr>

               <%-- <tr id="ta_Schedule" style="background-color: #CCC" <%= bg %>>
                    <td style='padding: 2px; text-align: left'>TAWeekShifts</td>
                    <td style='padding: 2px; text-align: left'>Add,Edit and Details of Week shifts</td>
                    <td style='padding: 2px; text-align: right'><span id='button_TA_Bo' class='icon icon_green_go tipsy_we' title='<%= Html.Encode("TAWeekShifts Details") %>' onclick="TaWeekShifts();"></span></td>
                </tr>--%>

                <tr id="ta_Schedule" style="background-color: #CCC" <%= bg %>>
                    <td style='padding: 2px; text-align: left'>TAUserGroupShifts</td>
                    <td style='padding: 2px; text-align: left'>Add,Edit and Details of group shifts</td>
                    <td style='padding: 2px; text-align: right'><span id='button_TA_Bo' class='icon icon_green_go tipsy_we' title='<%= Html.Encode("TAGroupShifts Details") %>' onclick="TaGroupShifts();"></span></td>
                </tr>

                <%if (Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewTAConfMenu))
              { 
                        %>
                
                <tr id="ta_boconf" style="background-color: #fff" <%= bg %>>
                    <td style='padding: 2px; text-align: left'><%:ViewResources.SharedStrings.BuildingNameScheduleTA %>   </td>
                    <td style='padding: 2px; text-align: left'><%:ViewResources.SharedStrings.BuildingNameScheduleTADescription %> </td>
                    <td style='padding: 2px; text-align: right'><span id='button_TA_Bo' class='icon icon_green_go tipsy_we' title='<%= Html.Encode("TABuilding Name Details") %>' onclick="showBuildingNameList();"></span></td>
                </tr>
                <%}
                    else { %>
                 <tr  id="ta_boconf" style="background-color: #fff;display:none"<%= bg %>>
                    <td style='padding: 2px; text-align: left'><%:ViewResources.SharedStrings.BuildingNameScheduleTA %></td>
                    <td style='padding: 2px; text-align: left'><%:ViewResources.SharedStrings.BuildingNameScheduleTADescription %> </td>
                    <td style='padding: 2px;  text-align: right'><span id='button_TA_Bo' class='icon icon_green_go' onclick='return false;'"></span></td>
                </tr>
                <%}%>  

                               <%if (Model.User.Menues.IsAvailabe((int)FoxSec.DomainModel.DomainObjects.Menu.ViewTAConfMenu))
              {%>
                
                <tr id="ta_boconf" style="background-color: #CCC">
                    <td style='padding: 2px; text-align: left'><%:ViewResources.SharedStrings.TAbulding %> </td>
                    <td style='padding: 2px; text-align: left'><%:ViewResources.SharedStrings.TAlist %> </td>
                    <td style='padding: 2px; text-align: right'><span id='button_TA_Bo' class='icon icon_green_go tipsy_we' title='<%= Html.Encode(ViewResources.SharedStrings.TAlist) %>' onclick='<%=string.Format("javascript:allBuildingObjects(\"submit_edit_user\", {0}, \"{1} {2}\")", 500, Html.Encode("eesn"), Html.Encode("peren")) %>'></span></td>
                </tr>

                <%}
                   else
               { %>
                   <tr  id="ta_boconf" style="background-color: #CCC;display:none">
                    <td style='padding: 2px; text-align: left'><%:ViewResources.SharedStrings.TAbulding %> </td>
                    <td style='padding: 2px; text-align: left'><%:ViewResources.SharedStrings.TAlist %> </td>
                    <td style='padding: 2px; text-align: right'><span id='button_TA_Bo' class='icon icon_green_go' onclick='return false;'"></span></td>
                </tr>
                <%}%>


                <%--<%  bg = (i++ % 2 == 1) ? "style='background-color:#CCC;'" : "";%>
     <tr id="ta_boconf" <%= bg %>>
        <td style='padding:2px; text-align:left'> T&A   </td>
         <td style='padding:2px; text-align:left'> building  Export Report </td>
        <td style='padding:2px; text-align:right'><span id='button_TA_Bo' class='icon icon_green_go tipsy_we' title='<%= Html.Encode("Create Entry for Building Names") %>' onclick= "ExportTaBuildingName();"></span></td>
 </tr>--%>
                <%-- </tr> --%>

             
                <%}%>
                <%--<tr>
        <td style='padding:2px; text-align:left'> 2a. T&A Schedule   </td>
         <td style='padding:2px; text-align:left'> Start from,breaks,Lunch,Until to </td>
        <td style='padding:2px; text-align:right'><span id='button_TA_Bo' class='icon icon_green_go tipsy_we' title='<%= Html.Encode("Create Entry for Building Names") %>' onclick= '<%=string.Format("javascript:EntryFort&A(\"submit_edit_user\", {0}, \"{1} {2}\")", 500, Html.Encode("eesn"), Html.Encode("peren")) %>'   ></span></td>
 </tr> 
      <tr>
        <td style='padding:2px; text-align:left'> Users Schedule   </td>
         <td style='padding:2px; text-align:left'> Activating Users T&A and Schedule </td>
        <td style='padding:2px; text-align:right'><span id='button_TA_Bo' class='icon icon_green_go tipsy_we' title='<%= Html.Encode("Create Entry for Building Names") %>' onclick= '<%=string.Format("javascript:EntryFort&A(\"submit_edit_user\", {0}, \"{1} {2}\")", 500, Html.Encode("eesn"), Html.Encode("peren")) %>'   ></span></td>
 </tr> --%>
                <%if (issuperadmin == false && !iscompanymanager == false && !isdepartmentmanager == false)
                    { %>

                <tr id="ta_detailed_report" style="background-color: #fff" <%= bg %>>
                    <td style='padding: 2px; text-align: left'><%:ViewResources.SharedStrings.TAMyReport %> </td>
                    <td style='padding: 2px; text-align: left'><%:ViewResources.SharedStrings.TAMyReport  %> </td>
                    <td style='padding: 2px; text-align: right'><span id='button_TA_detailed_report' class='icon icon_green_go tipsy_we' title='<%= Html.Encode(ViewResources.SharedStrings.TAreportViewe) %>' onclick='<%=string.Format("javascript:MyTAReport(\"submit_edit_user\", {0}, \"{1} {2}\")", 3, Html.Encode("eesn"), Html.Encode("peren")) %>'></span></td>
                </tr>
                <%} %>
                <%if (issuperadmin || iscompanymanager || isdepartmentmanager)
                    {
                        bg = (i++ % 2 == 1) ? "style='background-color:#fff;'" : "";%>
                <tr id="ta_mounth_report" style="background-color: #fff" <%--<%= bg %>--%>>
                    <td style='padding: 2px; text-align: left'><%:ViewResources.SharedStrings.TAMonthreport %> </td>
                    <td style='padding: 2px; text-align: left'><%:ViewResources.SharedStrings.WorkMontReportDescription %> </td>
                    <td style='padding: 2px; text-align: right'><span class='icon icon_green_go tipsy_we' title='<%= Html.Encode(ViewResources.SharedStrings.WorkMontReportDescription) %>' onclick='<%=string.Format("javascript:OpenMonthReport(\"submit_edit_user\", {0}, \"{1} {2}\")", 500, Html.Encode("eesn"), Html.Encode("peren")) %>'></span></td>
                </tr>
                <%     bg = (i++ % 2 == 1) ? "style='background-color:#CCC;'" : "";%>
                <tr id="ta_mounth_report" style="background-color: #CCC">
                    <td style='padding: 2px; text-align: left'><%:ViewResources.SharedStrings.TAlist %> </td>
                    <td style='padding: 2px; text-align: left'><%:ViewResources.SharedStrings.TAlistDetail %> </td>
                    <td style='padding: 2px; text-align: right'><span id='button_TA_mounthreport' class='icon icon_green_go tipsy_we' title='<%= Html.Encode(ViewResources.SharedStrings.WorkMontReportDescription) %>' onclick='<%=string.Format("javascript:StartEndReport(\"submit_edit_user\", {0}, \"{1} {2}\")", 500, Html.Encode("eesn"), Html.Encode("peren")) %>'></span></td>
                </tr>
                <%bg = (i++ % 2 == 1) ? "style='background-color:#CCC;'" : "";%>
                <tr id="ta_detailed_report" style="background-color: #fff" <%--<%= bg %>--%>>
                    <td style='padding: 2px; text-align: left'><%:ViewResources.SharedStrings.TADetailReport %> </td>
                    <td style='padding: 2px; text-align: left'><%:ViewResources.SharedStrings.TAdetails %> </td>
                    <td style='padding: 2px; text-align: right'><span id='button_TA_detailed_report' class='icon icon_green_go tipsy_we' title='<%= Html.Encode(ViewResources.SharedStrings.TAdetails) %>' onclick='<%=string.Format("javascript:TAReportDetailed(\"submit_edit_user\", {0}, \"{1} {2}\")", 3, Html.Encode("eesn"), Html.Encode("peren")) %>'></span></td>
                </tr>
                <%bg = (i++ % 2 == 1) ? "style='background-color:#CCC;'" : "";%>
                <tr id="ta_detailed_report" style="background-color: #CCC" <%--<%= bg %>--%>>

                    <td style='padding: 2px; text-align: left'><%:ViewResources.SharedStrings.TAExport %> </td>
                    <td style='padding: 2px; text-align: left'><%:ViewResources.SharedStrings.TAExportTo %> </td>
                    <td style='padding: 2px; text-align: right'><span id='button_TA_detailed_report' class='icon icon_green_go tipsy_we' title='<%= Html.Encode(ViewResources.SharedStrings.TAExportTo) %>' onclick='<%=string.Format("javascript:TAReportExport(\"submit_edit_user\", {0}, \"{1} {2}\")", 3, Html.Encode("eesn"), Html.Encode("peren")) %>'></span></td>
                </tr>
                <%} %>
                <%//if (issuperadmin)
                    {%>
                <tr id="daily_work_report" style="background-color: #fff">

                    <td style='padding: 2px; text-align: left'><%:ViewBag.TACompnay%>_1:<%:ViewResources.SharedStrings.TAMonthreport %></td>
                    <td style='padding: 2px; text-align: left'><%:ViewResources.SharedStrings.TaxReportExport %></td>
                    <td style='padding: 2px; text-align: right'><span class='icon icon_green_go tipsy_we' onclick='<%=string.Format("javascript:TaxMonthReport(\"submit_edit_user\", {0}, \"{1} {2}\")", 500, Html.Encode("eesn"), Html.Encode("peren")) %>'></span></td>
                </tr>
                <%bg = (i++ % 2 == 1) ? "style='background-color:#CCC;'" : "";%>
                <tr id="ta_tax_report" style="background-color: #CCC" <%--<%= bg %>--%>>
                    <td style='padding: 2px; text-align: left'><%:ViewBag.TACompnay%>_2: <%:ViewResources.SharedStrings.TADailyWorkingReport %></td>
                    <td style='padding: 2px; text-align: left'><%:ViewResources.SharedStrings.TaxReportExport %></td>
                    <td style='padding: 2px; text-align: right'><span class='icon icon_green_go tipsy_we' onclick='<%=string.Format("javascript:Taxreport(\"submit_edit_user\", {0}, \"{1} {2}\")", 3, Html.Encode("eesn"), Html.Encode("peren")) %>'></span></td>
                </tr>
                <%} %>
                <%-- <tr id="ta_mounth_report" style="background-color: #CCC">
                    <td style='padding: 2px; text-align: left'><%:ViewResources.SharedStrings.TAMonthreportWorkingDays %> </td>
                    <td style='padding: 2px; text-align: left'><%:ViewResources.SharedStrings.TAMonthreportWorkingDaysDesc %> </td>
                    <td style='padding: 2px; text-align: right'><span id='button_TA_mounthreport' class='icon icon_green_go tipsy_we' title='<%= Html.Encode(ViewResources.SharedStrings.TAMonthreportWorkingDaysDesc) %>' onclick='<%=string.Format("javascript:OpenMonthReportWorkingDays(\"submit_edit_user\", {0}, \"{1} {2}\")", 500, Html.Encode("eesn"), Html.Encode("peren")) %>'></span></td>
                </tr>--%>
            </tbody>
        </table>
        <input type="hidden" id="newcompnayid" />
    </form>
    <script type="text/javascript">

        $(document).ready(function () {
            //GetCompanies();
            var i = $('#panel_owner li').index($('#TAReportTab'));
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
            $('div#Work').fadeIn("slow");

            $(".tipsy_we").attr("class", function () {
                $(this).tipsy({ gravity: $.fn.tipsy.autoWE, html: true });
            });

            $("#FromDateTA").datepicker({
                dateFormat: "dd.mm.yy",
                firstDay: 1,
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,
                maxdate: "1d",
                onClose: function (selectedDate) {
                    $("#ToDateTA").datepicker("option", "minDate", selectedDate);
                }
                //onSelect: function (dateText, inst) {
                //    $("#ToDateTA").datepicker("option", "minDate", dateText);
                //}
            });


            $("#ToDateTA").datepicker({
                dateFormat: "dd.mm.yy",
                firstDay: 1,
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,
                onClose: function (selectedDate) {
                    $("#FromDateTA").datepicker("option", "maxDate", selectedDate);
                }
            });

            //bindCampany();
            //GetBuilding();
            //CompanyChanged();
        });

        function showBuildingNameList() {

            $("div#modal-dialog").dialog({
                open: function () {

                    var buildingid = $("#BuildingId").val();
                    if (buildingid == "" || buildingid == null || buildingid == undefined) {
                        buildingid = "0";
                    }
                    $('body').css('overflow', 'auto');
                    $("div#user-modal-dialog").html("");
                    $("div#modal-dialog").html("<div id='AreaUserEditWait' style='width: 100%; height:580px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                    $.ajax({
                        type: 'GET',
                        url: '/TAReport/TABuildingList',
                        cache: false,
                        data: {
                            datefrom: $("#FromDateTA").val(),
                            dateto: $("#ToDateTA").val(),
                            buildingid: buildingid
                        },
                        success: function (html) {
                            $("div#modal-dialog").html(html);
                        }
                    });
                    $(this).parents('.ui-dialog-buttonpane button:eq(0)').focus();
                },
                resizable: false,
                width: '95%',
                height: 710,
                modal: true,
                title: "<span class='ui-icon ui-icon-home' style='float:left; margin:1px 5px 0 0'></span>" + "TABuilding Name List",
                buttons: {
                    '<%= ViewResources.SharedStrings.BtnClose %>': function () {
                        $(this).dialog("close");
                    }
                },
                close: function () {
                    $('body').css('overflow', 'hidden');
                }
            });
            return false;
        }

        function TaMovement() {
            var windowWidth = $(window).width();
            $("div#modal-dialog").dialog({
                open: function () {
                    $("div#modal-dialog").html("<div id='TaMovement' style='width: 100%; height:580px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                    $.ajax({
                        type: 'GET',
                        url: '/TaReport/TAReportLabels',
                        success: function (result) {
                             $("div#modal-dialog").html(result);
                        }
                    });
                },
                resizable: false,
                width: windowWidth * 0.9,
                height: 710,
                modal: true,
                title: "<span class='ui-icon ui-icon-home' style='float:left; margin:1px 5px 0 0'></span> TAMovements",
                button: {
                    'Close': function () {
                        $(this).dialog("close");
                    }
                }
            });
        }

        function TaShift() {
            debugger;
            var windowWidth = $(window).width();
            $("div#modal-dialog").dialog({               
                open: function () {
                    $("div#user-modal-dialog").html();
                    $("div#modal-dialog").html("<div id='TaShiftWait' style='width: 100%; height:580px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                    $.ajax({
                        type: 'GET',
                        url: '/TAReport/TaShift',
                        cache: false,
                        success: function (result) {
                            $("div#modal-dialog").html(result);
                        }
                    });
                },
                resizable: false,
                //width: windowWidth * 0.9,
                width :  1000,
                height: 700,
                modal: true,
                title: "<span class='ui-icon ui-icon-home' style='float:left; margin:1px 5px 0 0'></span> TAShifts",
                buttons: {
                    'Close': function () {
                        $(this).dialog("close");
                        
                    },
                    //"Save": function () {
                    //    var shiftName = $("#shiftName").val();
                    //    var shiftStartTime = $("#shiftStartTime").val();
                    //    var shiftFinishTime = $("#shiftFinishTime").val();
                    //    var shiftModel = {
                    //        ShiftName: shiftName,
                    //        ShiftStartTime: shiftStartTime,
                    //        ShiftFinishTime: shiftFinishTime
                    //    };

                    //    $.ajax({
                    //        type: "Post",
                    //        url: "/TaReport/TaShiftSave",
                    //        data: { ShiftSaveModel: shiftModel },
                    //        dataType: "json",
                    //        success: function () {

                    //        }
                    //    });
                    //}
                }
            });
        }

        function AddNewTaShifts() {
             $("div#AddNewtaShiftsModal").dialog({               
                open: function () {
                    $("div#user-modal-dialog").html();
                    $("div#AddNewtaShiftsModal").html("<div id='TaShiftWait' style='width: 100%; height:580px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                    $.ajax({
                        type: 'GET',
                        url: '/TAReport/AddNewTaShift',
                        cache: false,
                        success: function (result) {
                            $("div#AddNewtaShiftsModal").html(result);
                        }
                    });
                },
                resizable: false,
                //width: windowWidth * 0.9,
                width :  900,
                height: 500,
                modal: true,
                title: "<span class='ui-icon ui-icon-home' style='float:left; margin:1px 5px 0 0'></span> TAShifts",
                buttons: {
                    'Close': function () {
                        $(this).dialog("close");
                        
                    },
                    "Save": function () {
                        debugger;
                        var shiftName = $("#shiftName").val();
                        var shiftStartDate = $("#startFrom").val();
                        var shiftFinishDate = $("#finishAt").val();
                        var breakTime = $("#breakTime").val();
                        var lateAllowed = $("#lateAllowed").val();
                        var breakMinuteInterval = $("#breakMinuteInterval").val();
                        var durationOfBreakOvertime = $("#durationOfBreakOvertime").val();
                        var presence = $("#presence").val();
                        var overtime = $("#overtime").val();
                        var overtimeStartEarlier = $("#overtimeStartEarlier").val();
                        var overtimeStartLater = $("#overtimeStartLater").val();
                        var taShiftTimeIntervalName = $("#taShiftTimeIntervalName").val();
                        var shiftIntervalStartTime = $("#shiftIntervalStartTime").val();
                        var shiftIntervalEndTime = $("#shiftIntervalEndTime").val();
                        var taReportlabel = $("#taReportlabel").val();
                        var taShiftTimeIntervalNameArray = [];
                        $(".taShiftTimeIntervalName").each(function () {
                            taShiftTimeIntervalNameArray.push($(this).val());
                        });
                        var shiftIntervalStartTimeArray = [];
                        $(".shiftIntervalStartTime").each(function () {
                            shiftIntervalStartTimeArray.push($(this).val());
                        });
                        var shiftIntervalEndTimeArray = [];
                        $(".shiftIntervalEndTime").each(function () {
                            shiftIntervalEndTimeArray.push($(this).val());
                        });
                        var TaReportLabel = [];
                        $(".taReportlabel").each(function () {
                            TaReportLabel.push($(this).val());
                        });
                        var shiftModel = {
                            ShiftName: shiftName,
                            ShiftStartTime: shiftStartDate,
                            ShiftFinishTime: shiftFinishDate,
                            DuratOfBreak: breakTime,
                            LateAllowed: lateAllowed,
                            BreakMinInterval: breakMinuteInterval,
                            DuratOfBreakOvertime: durationOfBreakOvertime,
                            BreakMinIntervalOvertime: overtime,
                            Presence: presence,
                            OvertimeStartLater: overtimeStartLater,
                            OvertimeStartsEarlier: overtimeStartEarlier,
                            TaShiftIntervalNamesList: taShiftTimeIntervalNameArray,
                            TaShiftIntervalStartTimeList: shiftIntervalStartTimeArray,
                            TaShiftIntervalEndTimeList: shiftIntervalEndTimeArray,
                            TaShiftIntervalTaReportLabelsIdList: TaReportLabel
                        };

                        $.ajax({
                            type: "Post",
                            url: "/TaReport/TaShiftSave",
                            data: { ShiftSaveModel: shiftModel },
                            dataType: "json",
                            success: function (response) {
                                ShowDialog(response.msg, 2000, response.IsSucceed);
                                $("div#AddNewtaShiftsModal").dialog('close');
                                $("div#modal-dialog").dialog('close');
                                TaShift();
                            }
                        });
                    }
                }
            });
        }

        function EditTaShift(shiftId) {
            debugger;
            var shiftName = $('#' + shiftId.toString()).val();
            $("div#EditTaShiftModal").dialog({
                open: function () {
                     $("div#EditTaShiftModal").html("<div id='TaShiftWait' style='width: 100%; height:580px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                    $.ajax({
                        type: "Get",
                        url: "/TaReport/EditTaShift",
                        data: { ShiftId: shiftId },
                        success: function (result) {
                            $("div#EditTaShiftModal").html(result);
                        }
                    });
                },
                resizable:false,
                modal: true,
                width: 900,
                title: "<span class='ui-icon ui-icon-home' style='float:left; margin:1px 5px 0 0'></span>" + shiftName,
                height: 500,
                buttons: {
                    "Close": function () {
                        $(this).dialog('close');
                    },
                    "Save": function () {
                        debugger;
                        var taShiftId = $("#taShiftId").val();
                        var name = $("#shiftName").val();
                        var shiftStartDate = $("#startFrom").val();
                        var shiftFinishDate = $("#finishAt").val();
                        var breakTime = $("#breakTime").val();
                        var lateAllowed = $("#lateAllowed").val();
                        var breakMinuteInterval = $("#breakMinuteInterval").val();
                        var durationOfBreakOvertime = $("#durationOfBreakOvertime").val();
                        var presence = $("#presence").val();
                        var overtime = $("#overtime").val();
                        var overtimeStartEarlier = $("#overtimeStartEarlier").val();
                        var overtimeStartLater = $("#overtimeStartLater").val();

                        var taShiftIntervalIdsArray = [];
                        $(".shiftIntervalIds").each(function () {
                            taShiftIntervalIdsArray.push($(this).val());
                        });

                       var taShiftTimeIntervalNameArray = [];
                        $(".taShiftTimeIntervalName").each(function () {
                            taShiftTimeIntervalNameArray.push($(this).val());
                        });

                        var shiftIntervalStartTimeArray = [];
                        $(".shiftIntervalStartTime").each(function () {
                            shiftIntervalStartTimeArray.push($(this).val());
                        });

                        var shiftIntervalEndTimeArray = [];
                        $(".shiftIntervalEndTime").each(function () {
                            shiftIntervalEndTimeArray.push($(this).val());
                        });

                         var TaReportLabel = [];
                        $(".taReportlabel").each(function () {
                            TaReportLabel.push($(this).val());
                        });

                        var shiftEditObject = {
                            Id: taShiftId,
                            ShiftName: name,
                            ShiftStartTime: shiftStartDate,
                            ShiftFinishTime: shiftFinishDate,
                            DuratOfBreak: breakTime,
                            LateAllowed: lateAllowed,
                            BreakMinInterval: breakMinuteInterval,
                            DuratOfBreakOvertime: durationOfBreakOvertime,
                            BreakMinIntervalOvertime: overtime,
                            Presence: presence,
                            OvertimeStartLater: overtimeStartLater,
                            OvertimeStartsEarlier: overtimeStartEarlier,
                            TaShiftIntervalIdList: taShiftIntervalIdsArray,
                            TaShiftIntervalNamesList: taShiftTimeIntervalNameArray,
                            TaShiftIntervalStartTimeList: shiftIntervalStartTimeArray,
                            TaShiftIntervalEndTimeList: shiftIntervalEndTimeArray,
                            TaShiftIntervalTaReportLabelsIdList: TaReportLabel
                        };

                        $.ajax({
                            type: "Post",
                            url: "/TaReport/EditTaShiftSave",
                            data: { TaShiftEditSave: shiftEditObject },
                            dataType: "json",
                            success: function (response) {
                                ShowDialog(response.msg, 2000, response.IsSucceed);
                                $("div#EditTaShiftModal").dialog('close');
                                $("div#modal-dialog").dialog('close');
                                TaShift();
                            }
                        });
                    }
                }
            });
        }

        

       function openTaWeekShiftsDialog() {
            debugger;
             $("div#TaWeekShiftDialog").dialog({               
                open: function () {
                    $("div#user-modal-dialog").html();
                    $("div#TaWeekShiftDialog").html("<div id='TaWeekShiftWait' style='width: 100%; height:580px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                    $.ajax({
                        type: 'GET',
                        url: '/TAReport/TaWeekShifts',
                        cache: false,
                        success: function (result) {
                            $("div#TaWeekShiftDialog").html(result);
                        }
                    });
                    

                },
                resizable: false,
                //width: windowWidth * 0.9,
                width :  700,
                height: 300,
                modal: true,
                title: "<span class='ui-icon ui-icon-home' style='float:left; margin:1px 5px 0 0'></span> TAWeekShifts",
                buttons: {
                    'close': function () {
                        $(this).dialog("close");
                        
                    },
                    'Save': function () {
                        debugger;
                        var name = $("#weekShiftName").val();
                        var mondayValue = $("#MondayDropdownId").val();
                        var tuesdayValue = $("#TuesdayDropdownId").val();
                        var wednesdayValue = $("#WednesdayDropdownId").val();
                        var thursdayValue = $("#ThursdayDropdownId").val();
                        var fridayValue = $("#FridayDropdownId").val();
                        var saturdayValue = $("#SaturdayDropdownId").val();
                        var sundayValue = $("#SundayDropdownId").val();
                        var jsonWeekShift = {
                            Name: name,
                            MondayShift: mondayValue,
                            TuesdayShift: tuesdayValue,
                            WednesdayShift: wednesdayValue,
                            ThursdayShift: thursdayValue,
                            FridayShift: fridayValue,
                            SaturdayShift: saturdayValue,
                            SundayShift:sundayValue
                        };
                        $.ajax({
                            type: "Post",
                            url: "/TaReport/AddTaWeekShift",
                            dataType: "json",
                            data: { weekShiftModel: jsonWeekShift },
                            success: function (result) {
                                if (result.IsSaved) {
                                    ShowDialog(result.msg, 2000, true);
                                    $("div#TaWeekShiftDialog").dialog('close');
                                    $("div#TaUserGroupShiftDialog").dialog('close');
                                  
                                }
                                else {
                                    ShowDialog(result.msg, 2000, false);
                                   
                                }
                               
                            }
                        });
                    }
                }
           });
            
    }

        //To Test

            function EditTaUserGroupShift(taUserGroupid) {
        
        debugger;
         var taUserGroupShiftName = $('#' + taUserGroupid.toString()).val();
        $("div#TaUserGroupShiftDialog").dialog({
            modal: true,
            open: function () {
               // $("div#TaUserGroupShiftDialog").html("<div id='AreaUserEditWait' style='width: 100%; height:580px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                $.ajax({
                    type: "Get",
                    data: { TaUserGroupId: taUserGroupid },
                    url: "/TAReport/EditTaUserGroupShift",
                    success: function (result) {
                        $("div#TaUserGroupShiftDialog").html(result);
                    }
                });
                    
            },
            resizable: true,
            title: "<span class='ui-icon ui-icon-home' style='float:left; margin:1px 5px 0 0'></span>" +taUserGroupShiftName ,
            width: 1230,
            height: 900,
            buttons: {
                "Close": function () {

                },
                "Save": function () {

                }
            }
            
        });
    }

    function AddGroupShiftsToUsers(taGroupUserShiftsId) {
        debugger;
        var taUserGroupShiftName = $('#' + taGroupUserShiftsId.toString()).val();
        $("#AddTaUserGroupShiftsToUserDialog").dialog({
            open: function () {
                $.ajax({
                    type: "Get",
                    url: "/TaReport/AssignTaUserGroupShiftToUsers",
                    data: { GroupShiftId: taGroupUserShiftsId },
                    success: function (result) {
                        $("#AddTaUserGroupShiftsToUserDialog").html(result);
                    }

                });
            },
            modal: true,
            resizable: false,
            height: 800,
            title: "<span class='ui-icon ui-icon-home' style='float:left; margin:1px 5px 0 0'></span>" +taUserGroupShiftName ,
            width: 1230,
            buttons: {
                "Save": function () {
                    debugger;
                    $("#startDateErrorMessage").css("display", "none");
                    $("#endDateErrorMessage").css("display", "none");
                    var taUserGroupShiftId = $("#TaUsersGroupShiftId").val();
                    var startDate = $("#startDate").val();
                    var endDate = $("#endDate").val();
                    var selectedUsersId = [];
                    $(".users").each(function () {
                        if ($(this).is(":checked")) {
                            selectedUsersId.push($(this).val());
                        }
                    });
                    var selectedUsersTA = [];
                    $(".isTA").each(function () {
                        if ($(this).is(":checked")) {
                             selectedUsersTA.push($(this).val());
                        } 
                    });
                    var model = {
                        TaUserGroupShiftId: taUserGroupShiftId,
                        StartDate: startDate,
                        EndDate: endDate,
                        SelectedUsersId: selectedUsersId,
                        SelectedUsersIsTA: selectedUsersTA
                    };
                    if (startDate.length > 0 && endDate.length > 0) {
                        $.ajax({
                            type: "Post",
                            url: "/TaReport/AssignTaUserGroupShiftToUsersSave",
                            data: model,
                            dataType: "json",
                            success: function () {

                            }
                        });
                    }
                    else if (startDate.length > 0 && endDate.length == 0) {
                        $("#endDateErrorMessage").css("display", "inline");
                    }
                    else if (startDate.length == 0  && endDate.length > 0) {
                        $("#startDateErrorMessage").css("display", "inline");
                    }
                    else if (startDate.length == 0 && endDate.length == 0) {
                        $("#startDateErrorMessage").css("display", "inline");
                        $("#endDateErrorMessage").css("display", "inline");
                    }
                },
                "Close": function () {
                    $(this).dialog('close');
                }
            }

        });
    }

    function AddNewTaUserGroupShifts() {
        $("#AddTaUserGroupShiftsDialog").dialog({
            modal:true,
            open: function () {
                $.ajax({
                    type: "Get",
                    url: "/TaReport/AddTaUserGroupShifts",
                    success: function (result) {
                        $("#AddTaUserGroupShiftsDialog").html(result);
                    }
                });
            },
            resizable: false,
            width: 1100,
            height: 700,
            title: "<span class='ui-icon ui-icon-home' style='float:left; margin:1px 5px 0 0'></span> Add new TaUser group shift ",
            buttons: {
                "Save": function () {
                    debugger;
                    var taGroupShiftName = $("#taUserGroupShiftName").val();
                    var repeatWeeks = $("#repeatCycle").val();
                    var selectedTaWeeks = [];
                    var toCheck = [];
                    $(".dynamicSelectedValue").each(function () {
                        selectedTaWeeks.push($(this).val());
                    });
                    var taUserGroupShiftStartDate = $("#taUserGroupShiftStartDate").val();
                    var taNewUserGroupShiftsModel = {
                        Name: taGroupShiftName,
                        RepeatAfterWeeks: repeatWeeks,
                        SelectedTaWeeks: selectedTaWeeks,
                        StartFromDate: taUserGroupShiftStartDate
                    };
                    $.ajax({
                        type: "Post",
                        url: "/TaReport/SaveNewTaUserGroupShift",
                        dataType: "json",
                        data: { TaUserGroupShiftSaveModel: taNewUserGroupShiftsModel },
                        success: function (response) {
                            debugger;
                            ShowDialog(response.msg, 2000, response.IsSucceed);
                            $("div#AddTaUserGroupShiftsDialog").dialog('close');
                            $("div#modal-dialog").dialog('close');
                            TaGroupShifts();
                            
                        }
                    });
                },
                "Cancel": function () {
                    $(this).dialog('close');
                }
            }
        });
    }
        //
        function TaWeekShifts() {
            debugger;
            $("div#modal-dialog").dialog({               
                open: function () {
                    $("div#user-modal-dialog").html();
                    $("div#modal-dialog").html("<div id='TaWeekShiftWait' style='width: 100%; height:580px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                    $.ajax({
                        type: 'GET',
                        url: '/TAReport/TaWeekShifts',
                        cache: false,
                        success: function (result) {
                            $("div#modal-dialog").html(result);
                        }
                    });
                    

                },
                resizable: false,
                //width: windowWidth * 0.9,
                width :  700,
                height: 300,
                modal: true,
                title: "<span class='ui-icon ui-icon-home' style='float:left; margin:1px 5px 0 0'></span> TAWeekShifts",
                buttons: {
                    'close': function () {
                        $(this).dialog("close");
                        
                    },
                    'Save': function () {
                        debugger;
                        var mondayValue = $("#MondayDropdownId").val();
                        var tuesdayValue = $("#TuesdayDropdownId").val();
                        var wednesdayValue = $("#WednesdayDropdownId").val();
                        var thursdayValue = $("#ThursdayDropdownId").val();
                        var fridayValue = $("#FridayDropdownId").val();
                        var saturdayValue = $("#SaturdayDropdownId").val();
                        var sundayValue = $("#SundayDropdownId").val();
                        var jsonWeekShift = {
                            mondayValue: mondayValue,
                            tuesdayValue: tuesdayValue,
                            wednesdayValue: wednesdayValue,
                            thursdayValue: thursdayValue,
                            fridayValue: fridayValue,
                            saturdayValue: saturdayValue,
                            sundayValue:sundayValue
                        };
                    }
                }
            });
        }

        function TaGroupShifts() {
            debugger;
            var windowWidth = $(window).width();
              $("div#modal-dialog").dialog({               
                open: function () {
                    $("div#user-modal-dialog").html();
                    $("div#modal-dialog").html("<div id='TaGroupShiftWait' style='width: 100%; height:580px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                    $.ajax({
                        type: 'GET',
                        url: '/TAReport/TaUserGroupShiftList',
                        cache: false,
                        success: function (result) {
                            $("div#modal-dialog").html(result);
                        }
                    });
                },
                resizable: false,
                width: windowWidth * 0.5,
                //width :  700,
                height: 500,
                modal: true,
                title: "<span class='ui-icon ui-icon-home' style='float:left; margin:1px 5px 0 0'></span> TAGroupShifts",
                buttons: {
                    'Close': function () {
                        $(this).dialog("close");
                        
                    }
                }
            });
        }

        function CompanyChanged() {

            var cid = $('#CompanyIdta').val();
            $('#newcompnayid').val(cid);
            $.ajax({
                type: "Get",
                url: "/Log/GetDepartmentsByCompany",
                dataType: 'json',
                data: { id: cid },//$('#CompanyId').val() },
                success: function (data) {
                    if ($("select#DepartmentId").size() > 0) {
                        $("select#DepartmentId").html(data);
                    }
                }
            })


            return false;

        }
        function allBuildingObjects(Action, UserId, Username) {
            $("div#modal-dialog").dialog({
                open: function () {
                    $("div#user-modal-dialog").html("");
                    $("div#modal-dialog").html("<div id='AreaUserEditWait' style='width: 100%; height:580px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                    $.ajax({
                        type: 'GET',
                        url: '/TAReport/OpenAllBuildingsObjects',
                        cache: false,
                        data: {
                            id: UserId
                        },
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
                title: "<span class='ui-icon ui-icon-home' style='float:left; margin:1px 5px 0 0'></span>" + '<%:ViewResources.SharedStrings.TAbulding%>',
                buttons: {
                    '<%= ViewResources.SharedStrings.BtnClose %>': function () {
                        $(this).dialog("close");
                    }
                }
            });
            return false;
        }
        function GetBuilding() {
            $.ajax({
                type: "Get",
                url: "/TAReport/GetBuildings",
                dataType: 'json',
                data: { datefrom: $("#FromDateTA").val(), dateto: $("#ToDateTA").val() },

                success: function (data) {
                    if ($("select#BuildingId").size() > 0) {
                        $("select#BuildingId").html(data);
                    }
                }
            });
            return false;
        }




        function TAReportDetailed(Action, UserId, Username) {

            $("div#modal-dialog").dialog({
                open: function () {
                    $("div#user-modal-dialog").html("");
                    $("div#modal-dialog").html("<div id='AreaUserEditWait' style='width: 100%; height:580px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                    $.ajax({
                        type: 'GET',
                        url: '/TAReport/TAReportDetailed',
                        cache: false,
                        data: {
                            company: $('#CompanyIdta').val(),
                            department: $('#DepartmentId').val(),
                            id: UserId,
                            FromDateTA: $('#FromDateTA').val(),
                            ToDateTA: $("#ToDateTA").val(),
                            BuildingId: $("#BuildingId").val()
                        },
                        success: function (html) {
                            $("div#modal-dialog").html(html);
                        }
                    });
                    $(this).parents('.ui-dialog-buttonpane button:eq(0)').focus();
                },
                resizable: false,
                width: 1000,
                height: 710,
                modal: true,
                title: "<span class='ui-icon ui-icon-home' style='float:left; margin:1px 5px 0 0'></span>" + '<%= ViewResources.SharedStrings.TADetailReport %>',
                buttons: {
                    '<%= ViewResources.SharedStrings.BtnClose %>': function () {
                        $(this).dialog("close");
                    }
                }
            });
            return false;
        }

        function TAReportExport(Action, UserId, Username) {

            var date1 = $('#FromDateTA').val();
            $("div#modal-dialog").dialog({
                open: function () {
                    var BName = $("#BuildingId :selected").text();

                    if (BName == "") {
                        BName = "0";
                    }
                    var cc = $("#newcompnayid").val();

                    $("div#user-modal-dialog").html("");
                    $("div#modal-dialog").html("<div id='AreaUserEditWait' style='width: 100%; height:580px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                    $.ajax({
                        type: 'GET',
                        url: '/TAReport/TAReportExport',
                        cache: false,
                        data: {
                            department: $('#DepartmentId').val(),
                            company: $('#CompanyIdta').val(),
                            FromDateTA: $('#FromDateTA').val(),
                            ToDateTA: $("#ToDateTA").val(),
                            format: $("#Format").val(),
                            BuildingId: $("#BuildingId").val(),
                            BName: BName
                        },
                        success: function (html) {
                            $("div#modal-dialog").html(html);
                        }
                    });
                    $(this).parents('.ui-dialog-buttonpane button:eq(0)').focus();
                },
                resizable: false,
                width: 1000,
                height: 710,
                modal: true,
                title: "<span class='ui-icon ui-icon-home' style='float:left; margin:1px 5px 0 0'></span>" + '<%= ViewResources.SharedStrings.TAExport%>',
                buttons: {
                    '<%= ViewResources.SharedStrings.BtnClose %>': function () {
                        $(this).dialog("close");
                    }
                }
            });
            return false;
        }
        function MyTAReport(Action, UserId, Username) {
            $("div#modal-dialog").dialog({
                open: function () {
                    $("div#user-modal-dialog").html("");
                    $("div#modal-dialog").html("<div id='AreaUserEditWait' style='width: 100%; height:580px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                    $.ajax({
                        type: 'GET',
                        url: '/TAReport/MyTAReport',
                        cache: false,
                        data: {
                            format: $('#Format').val(),
                            company: $('#CompanyIdta').val(),
                            id: UserId,
                            FromDateTA: $('#FromDateTA').val(),
                            ToDateTA: $("#ToDateTA").val()
                        },
                        success: function (html) {
                            $("div#modal-dialog").html(html);
                        }
                    });
                    $(this).parents('.ui-dialog-buttonpane button:eq(0)').focus();
                },
                resizable: false,
                width: 1000,
                height: 710,
                modal: true,
                title: "<span class='ui-icon ui-icon-home' style='float:left; margin:1px 5px 0 0'></span>" + '<%= ViewResources.SharedStrings.TAMyReport %>',
                buttons: {
                    '<%= ViewResources.SharedStrings.BtnClose %>': function () {
                        $(this).dialog("close");
                    }
                }
            });
            return false;
        }

        function StartEndReport(Action, UserId, Username) {
            var wWidth = $(window).width();
            var dWidth = wWidth * 0.9;
            $("div#modal-dialog").dialog({
                open: function () {
                    $("div#user-modal-dialog").html("");
                    $("div#modal-dialog").html("<div id='AreaUserEditWait' style='width: 100%; height:580px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                    $.ajax({
                        type: 'GET',
                        url: '/TAReport/StartEndReport',
                        cache: false,
                        data: {
                            format: $('#Format').val(),
                            company: $('#CompanyIdta').val(),
                            department: $('#DepartmentId').val(),
                            id: UserId,
                            FromDateTA: $('#FromDateTA').val(),
                            ToDateTA: $("#ToDateTA").val(),
                            BuildingId: $("#BuildingId").val()
                        },
                        success: function (html) {
                            $("div#modal-dialog").html(html);
                        }
                    });
                    $(this).parents('.ui-dialog-buttonpane button:eq(0)').focus();
                },
                resizable: false,
                width: dWidth,
                height: 710,
                modal: true,
                title: "<span class='ui-icon ui-icon-home' style='float:left; margin:1px 5px 0 0'></span>" + '<%= ViewResources.SharedStrings.TAlist %>',
                buttons: {
                    '<%= ViewResources.SharedStrings.BtnClose %>': function () {
                        $(this).dialog("close");
                    }
                }
            });
            return false;
        }
        function test() { }
        function OpenMonthReport(Action, UserId, Username) {
            debugger;
            //var buildingID = $('#BuildingId').val();
            //alert("value:" + buildingID);
            var wWidth = $(window).width();
            var dWidth = wWidth * 0.9;
            $("div#modal-dialog").dialog({
                open: function () {
                    $("div#user-modal-dialog").html("");
                    $("div#modal-dialog").html("<div id='AreaUserEditWait'style='width: 100%; height:580px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                    $.ajax({
                        type: 'GET',
                        url: '/TAReport/OpenMounthReport',
                        cache: false,
                        data: {
                            format: $('#Format').val(),
                            company: $('#CompanyIdta').val(),
                            department: $('#DepartmentId').val(),
                            id: UserId,
                            FromDateTA: $('#FromDateTA').val(),
                            ToDateTA: $("#ToDateTA").val(),
                            BuildingId: $('#BuildingId').val()
                        },
                        success: function (html) {

                            $("div#modal-dialog").html(html);
                        }
                    });
                    $(this).parents('.ui-dialog-buttonpane button:eq(0)').focus();
                },
                resizable: true,
                width: dWidth,
                height: 710,
                modal: true,
                title: "<span class='ui-icon ui-icon-home' style='float:left; margin:1px 5px 0 0'></span>" + '<%= ViewResources.SharedStrings.TAMonthreport%>',
                buttons: {
                    '<%= ViewResources.SharedStrings.BtnClose %>': function () {
                        $(this).dialog("close");
                    }
                }
            });
            return false;
        }


        function OpenMonthReportWorkingDays(Action, UserId, Username) {
            //var buildingID = $('#BuildingId').val();
            //alert("value:" + buildingID);

            var wWidth = $(window).width();
            var dWidth = wWidth * 0.9;
            $("div#modal-dialog").dialog({
                open: function () {
                    $("div#user-modal-dialog").html("");
                    $("div#modal-dialog").html("<div id='AreaUserEditWait'style='width: 100%; height:580px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                    $.ajax({
                        type: 'GET',
                        url: '/TAReport/OpenMounthReportWrokingDays',
                        cache: false,
                        data: {
                            format: $('#Format').val(),
                            company: $('#CompanyIdta').val(),
                            department: $('#DepartmentId').val(),
                            id: UserId,
                            FromDateTA: $('#FromDateTA').val(),
                            ToDateTA: $("#ToDateTA").val(),
                            BuildingId: $('#BuildingId').val()
                        },
                        success: function (html) {

                            $("div#modal-dialog").html(html);
                        }
                    });
                    $(this).parents('.ui-dialog-buttonpane button:eq(0)').focus();
                },
                resizable: true,
                width: dWidth,
                height: 710,
                modal: true,
                title: "<span class='ui-icon ui-icon-home' style='float:left; margin:1px 5px 0 0'></span>" + '<%= ViewResources.SharedStrings.TAMonthreportWorkingDays%>',
                buttons: {
                    '<%= ViewResources.SharedStrings.BtnClose %>': function () {
                        $(this).dialog("close");
                    }
                }
            });
            return false;
        }

        function Taxreport(Action, UserId, Username) {
            var date1 = $('#FromDateTA').val();
            $("div#modal-dialog").dialog({
                open: function () {
                    var BName = $("#BuildingId :selected").text();
                    var Bid = $("#BuildingId").val();
                    if (BName == "" || BName == null || BName == undefined) {
                        BName = "";
                    }
                    if (Bid == "" || Bid == null || Bid == undefined) {
                        Bid = "0";
                    }
                    var comp = $("#CompanyIdta").val();
                    if (comp == "" || comp == null || comp == undefined) {
                        comp = "0";
                    }
                    var dep = $("#DepartmentId").val();
                    if (dep == "" || dep == null || dep == undefined) {
                        dep = "0";
                    }
                    var cc = $("#newcompnayid").val();

                    $("div#user-modal-dialog").html("");
                    $("div#modal-dialog").html("<div id='AreaUserEditWait'style='width: 100%; height:580px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                    $.ajax({
                        type: 'GET',
                        url: '/TAReport/TATaxReportExport',
                        cache: false,
                        data: {
                            department: dep,
                            company: comp,
                            FromDateTA: $('#FromDateTA').val(),
                            ToDateTA: $("#ToDateTA").val(),
                            format: $("#Format").val(),
                            BuildingId: Bid,
                            BName: BName
                        },
                        success: function (html) {
                            $("div#modal-dialog").html(html);
                        }
                    });
                    $(this).parents('.ui-dialog-buttonpane button:eq(0)').focus();
                },
                resizable: false,
                width: 1000,
                height: 710,
                modal: true,
                title: "<span class='ui-icon ui-icon-home' style='float:left; margin:1px 5px 0 0'></span>" + '<%:ViewResources.SharedStrings.TADailyWorkingReport %>',
                buttons: {
                    '<%= ViewResources.SharedStrings.BtnClose %>': function () {
                        $(this).dialog("close");
                    }
                }
            });
            return false;
        }


        function DailyWorkingReport() {
            var wWidth = $(window).width();
            var dWidth = wWidth * 0.9;
            $("div#modal-dialog").dialog({
                open: function () {
                    $("div#user-modal-dialog").html("");
                    $("div#modal-dialog").html("<div id='AreaUserEditWait'style='width: 100%; height:580px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                    $.ajax({
                        type: 'GET',
                        url: '/TAReport/DailyWorkingReport',
                        cache: false,
                        data: {},
                        success: function (html) {
                            $("div#modal-dialog").html(html);
                        }
                    });
                    $(this).parents('.ui-dialog-buttonpane button:eq(0)').focus();
                },
                resizable: true,
                width: dWidth,
                height: 710,
                modal: true,
                title: "<span class='ui-icon ui-icon-home' style='float:left; margin:1px 5px 0 0'></span>" + '<%= ViewResources.SharedStrings.TAMonthreportWorkingDays%>',
                buttons: {
                    '<%= ViewResources.SharedStrings.BtnClose %>': function () {
                        $(this).dialog("close");
                    }
                }
            });
            return false;
        }

        function TaxMonthReport(Action, UserId, Username) {
          <%--  var buildid = $("#BuildingId").val();
            if (buildid == "" || buildid == null || buildid == undefined) {
                alert("Please select Building!!");;
                return false;
            }
            var comp = $("#CompanyIdta").val();
            if (comp == "" || comp == null || comp == undefined) {
                comp = "0";
            }
            var dep = $("#DepartmentId").val();
            if (dep == "" || dep == null || dep == undefined) {
                dep = "0";
            }
            var wWidth = $(window).width();
            var dWidth = wWidth * 0.9;
            $("div#modal-dialog").dialog({
                open: function () {
                    $("div#user-modal-dialog").html("");
                    $("div#modal-dialog").html("<div id='AreaUserEditWait'style='width: 100%; height:580px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                    $.ajax({
                        type: 'GET',
                        url: '/TAReport/TaxMonthReport',
                        cache: false,
                        data: {
                            format: $('#Format').val(),
                            company: comp,
                            department: dep,
                            id: UserId,
                            FromDateTA: $('#FromDateTA').val(),
                            ToDateTA: $("#ToDateTA").val(),
                            BuildingId: $('#BuildingId').val()
                        },
                        success: function (html) {

                            $("div#modal-dialog").html(html);
                        }
                    });
                    $(this).parents('.ui-dialog-buttonpane button:eq(0)').focus();
                },
                resizable: true,
                width: dWidth,
                height: 710,
                modal: true,
                title: "<span class='ui-icon ui-icon-home' style='float:left; margin:1px 5px 0 0'></span>" + '<%= ViewResources.SharedStrings.TAMonthreport%>',
                buttons: {
                        '<%= ViewResources.SharedStrings.BtnClose %>': function () {
                        $(this).dialog("close");
                    }
                }
            });
            return false;--%>
            var buildid = $("#BuildingId").val();

            if (buildid == "" || buildid == null || buildid == undefined) {
                alert("Please select Building!!");;
                return false;
            }

            var comp = $("#CompanyIdta").val();
            if (comp == "" || comp == null || comp == undefined) {
                comp = "0";
            }
            var dep = $("#DepartmentId").val();
            if (dep == "" || dep == null || dep == undefined) {
                dep = "0";
            }
            var wWidth = $(window).width();
            var dWidth = wWidth * 0.9;
            var date1 = $('#FromDateTA').val();
            $("div#modal-dialog").dialog({
                open: function () {
                    var BName = $("#BuildingId :selected").text();

                    if (BName == "") {
                        BName = "";
                    }
                    var cc = $("#newcompnayid").val();

                    $("div#user-modal-dialog").html("");
                    $("div#modal-dialog").html("<div id='AreaUserEditWait'style='width: 100%; height:580px; text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                    $.ajax({
                        type: 'GET',
                        url: '/TAReport/TATaxReportExportN',
                        cache: false,
                        data: {
                            department: dep,
                            company: comp,
                            FromDateTA: $('#FromDateTA').val(),
                            ToDateTA: $("#ToDateTA").val(),
                            format: $("#Format").val(),
                            BuildingId: buildid,
                            BName: BName
                        },
                        success: function (html) {
                            $("div#modal-dialog").html(html);
                        }
                    });
                    $(this).parents('.ui-dialog-buttonpane button:eq(0)').focus();
                },
                resizable: false,
                width: 1000,
                height: 710,
                modal: true,
                title: "<span class='ui-icon ui-icon-home' style='float:left; margin:1px 5px 0 0'></span>" + '<%= ViewResources.SharedStrings.TAMonthreport%>',
                buttons: {
                    '<%= ViewResources.SharedStrings.BtnClose %>': function () {
                        $(this).dialog("close");
                    }
                }
            });
            return false;
        }

        //by manoranjan

        function vedludbReportPost() {
            debugger;
            var from = $("#FromDateTA").val();
            var to = $("#ToDateTA").val();
            var company = $("#CompanyIdta").val();
            var department = $("#DepartmentId").val();
            var building = $("#BuildingId").val();
            $('#loading').removeAttr('hidden');
            
            $.ajax({
                type: "POST",
                url: "../TAReport/SendReportToVedludb/",
                data: { from: from,to: to,company: company,department: department,building: building },
                success: function (data) {
                    if (data == null) {
                        ShowDialog("Report not saved", 5000);
                    }
                    else if (data.IsSucceed) {

                        ShowDialog(data.msg, 10000, true);
                    }
                    else {
                        ShowDialog(data.msg, 5000);
                    }
                    //else if (data == "Please add Edlus integration certificate.") {
                    //    ShowDialog(data, 5000);
                    //}

                    //else if (data == "Please select building.") {
                    //    ShowDialog(data, 5000);
                    //}
                    //else if (data == "Building license for the selected building does not match with building permit no in eldus") {
                    //    ShowDialog(data, 5000);
                    //}

                    //else if (data == "Report saved to EDLUS.") {
                    //    ShowDialog(data, 5000);
                    //}
                    //else if (data == "Report not saved to EDLUS.") {
                    //    ShowDialog(data, 5000);
                    //}
                    //else if (data == "Report not saved to EDLUS.EX") {
                    //    ShowDialog("Report not saved to EDLUS.EXCEPTION", 5000);
                    //}
                    //else if (data == "No data present in the report to send") {
                    //    ShowDialog(data, 5000);
                    //}
                    //    else if (data == "No integration certificate found") {
                    //    ShowDialog(data, 5000);
                    //}
                        
                    //else {
                    //    ShowDialog("Report not saved to EDLUS.EL", 5000);
                    //}
                    $('#loading').attr('hidden','true');
                }
            });

        }

        //
    </script>
    
    <%--<style type="text/css">
        .ui-dialog .ui-dialog-title {
            direction: ltr;
            float: left;
            margin: 0.1em 0 0.2em 16px;
        }

        .ui-dialog .ui-dialog-titlebar-close {
            left: 0.3em;
        }

        .ui-dialog .ui-dialog-buttonpane .ui-dialog-buttonset {
            float: none;
        }

        .ui-dialog .ui-dialog-buttonpane {
            text-align:center;/*!important*//* left/center/right */
        }
    </style>--%>
</div>
