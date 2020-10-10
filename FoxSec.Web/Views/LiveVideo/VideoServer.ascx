<%@ Control Language="C#" %>
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
<style type="text/css">
    .TFtable {
        width: 100%;
        border-collapse: collapse;
    }

        .TFtable td {
            padding: 3px;
        }
        /* provide some minimal visual accomodation for IE8 and below */
        .TFtable tr {
            background: #CCC;
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
<form id="createCamera">
    <div id="loading"></div>

    <table cellpadding="1" cellspacing="0" style="margin: 0; width: 100%; padding: 1px; border-spacing: 0">
        <tr>
            <td style='width: 30%; padding: 2px; text-align: right;'>
                <label><%:ViewResources.SharedStrings.ProjectName %>:</label></td>
            <td style='width: 60%; padding: 2px;'>
                <select id="_Project" style="width: 90%">
                </select>
            </td>
        </tr>
        <tr>
            <td style='width: 30%; padding: 2px; text-align: right;'>
                <label><%:ViewResources.SharedStrings.ServerName %>:</label></td>
            <td style='width: 60%; padding: 2px;'>
                <input type="text" id="_Name" style="width: 90%;text-transform: capitalize;" />
            </td>
        </tr>

        <tr>
            <td style='width: 30%; padding: 2px; text-align: right;'>
                <label><%:ViewResources.SharedStrings.ServerIP %>:</label></td>
            <td style='width: 60%; padding: 2px;'>
                <input type="text" id="_IP" style="width: 90%" />
            </td>
        </tr>
        <tr>
            <td style='width: 30%; padding: 2px; text-align: right;'>
                <label><%:ViewResources.SharedStrings.ServerUID %>:</label></td>
            <td style='width: 60%; padding: 2px;'>
                <input type="text" id="_UID" style="width: 90%" />
            </td>
        </tr>
        <tr>
            <td style='width: 30%; padding: 2px; text-align: right;'>
                <label><%:ViewResources.SharedStrings.ServerPWD %>:</label></td>
            <td style='width: 60%; padding: 2px;'>
                <input type="password" id="_PWD" style="width: 90%" />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <input type='button' value='<%=ViewResources.SharedStrings.BtnSave %>' onclick="javascript: savedetails();" />
            </td>
        </tr>
    </table>
    <br />
    <br />
    <table cellpadding="3" cellspacing="3" style="margin: 0; width: 100%; padding: 3px; border-spacing: 4px;" class="TFtable">
        <thead>
            <tr style="background-color: black;">
                <th style='padding: 5px; color: white;width:30%'>
                    <%:ViewResources.SharedStrings.ProjectName %>
                </th>
                <th style='padding: 5px; color: white;width:20%'>
                    <%:ViewResources.SharedStrings.ServerName %>
                </th>
                <th style='padding: 5px; color: white;width:15%'>
                    <%:ViewResources.SharedStrings.ServerIP %>
                </th>
                <th style='padding: 5px; color: white;width:15%'>
                    <%:ViewResources.SharedStrings.ServerUID %>
                </th>
             <%--   <th style='padding: 5px; color: white;width:15%'>
                    <%:ViewResources.SharedStrings.ServerPWD %>
                </th>--%>
                <th style='padding: 5px; color: white;width:5%'>
                    
                </th>
            </tr>
        </thead>
        <tbody id="_tblVideoServer">
        </tbody>
    </table>
</form>

<script type="text/javascript" language="javascript">
    var glbaddedit = "";
    var glbid = 0;
    $(document).ready(function () {
        $("input:button").button();
        loadAllFSVideoServers();
        $('#loading').fadeOut();
    });

    function loadAllFSVideoServers() {
        $.ajax({
            type: "POST",
            url: "/LiveVideo/GetAllFSVideoServers",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (r) {
               // debugger;
                var ddlfs = $("[id*=_tblVideoServer]");
                ddlfs.empty();
                $.each(r, function () {
                    if (this['Name'] == null || this['Name'] == "null") {
                        this['Name'] = "";
                    }
                    if (this['IP'] == null || this['IP'] == "null") {
                        this['IP'] = "";
                    }
                    if (this['UID'] == null || this['UID'] == "null") {
                        this['UID'] = "";
                    }
                    if (this['PWD'] == null || this['PWD'] == "null") {
                        this['PWD'] = "";
                    }
                    var id = this['Id'];
                    //var str = "<tr><td>" + this['ProjectName'] + "</td><td>" + this['Name'] + "</td><td>" + this['IP'] + "</td><td>" + this['UID'] + "</td><td>" + this['PWD'] + "</td><td><span id='button'  class='icon icon_green_go tipsy_we' data-tooltip='EDIT'  title='EDIT'  original-title='EDIT' onclick='EditFSServer(" + id + ")' /></td></tr>";
                    var str = "<tr><td>" + this['ProjectName'] + "</td><td>" + this['Name'] + "</td><td>" + this['IP'] + "</td><td>" + this['UID'] + "</td><td><span id='button'  class='icon icon_green_go tipsy_we' data-tooltip='EDIT'  title='EDIT'  original-title='EDIT' onclick='EditFSServer(" + id + ")' /></td></tr>";
                    ddlfs.append(str);
                });

            }
        });
    }

    function savedetails() {

        var Name = $('#_Name').val();
        if (Name == "" || Name == null) {
            ShowDialog( '<%=ViewResources.SharedStrings.EnterName %>', 4000);
            return false;
        }

        var Project = $('#_Project').val();
        var IP = $('#_IP').val();
        var UID = $('#_UID').val();
        var PWD = $('#_PWD').val();
        var flgtype = glbaddedit;
        var id = glbid;
        $.ajax({
            type: "Post",
            url: "/LiveVideo/SaveUpdateFSServerDetails",
            //dataType: 'json',
            //traditional: true,
            data: { Name: Name, Project: Project, IP: IP, UID: UID, PWD: PWD, id: id, flgtype: flgtype },
            success: function (data) {
                if (data == "1") {
                    ShowDialog('<%=ViewResources.SharedStrings.CommonDataSavedMessage %>', 3000, true);
                    loadAllFSVideoServers();
                }
                else {
                    ShowDialog(data, 5000);
                }
                $('#_Project').val("0");
                $('#_IP').val("");
                $('#_UID').val("");
                $('#_PWD').val("");
                $('#_Name').val("");
                glbaddedit = "";
                glbid = 0;
            }
        });
        //$("div#modal-dialog").html("");
        //$("div#modal-dialog").dialog("close");
    }
    function EditFSServer(id) {
        glbid = id;
        glbaddedit = "1";
        $.ajax({
            type: "Post",
            url: "/LiveVideo/GetSelFSVideoServers",
            data: { id: id },
            success: function (data) {
                var req = data;
                $('#_Project').val(req["ProjectId"]);
                $('#_IP').val(req["IP"]);
                $('#_UID').val(req["UID"]);
                $('#_PWD').val(req["PWD"]);
                $('#_Name').val(req["Name"]);
            }
        });
    }
</script>
