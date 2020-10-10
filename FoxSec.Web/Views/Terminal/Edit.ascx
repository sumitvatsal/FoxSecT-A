<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.TerminalModel>" %>
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
<form id="editTerminal" action="">
    <div id="loading"></div>
    <%= Html.Hidden("Term_Id", Model.term.Id) %>
    <%= Html.Hidden("_Terminal_Id", Model.term.TerminalId,new { id="_Terminal_Id"}) %>
    <table cellpadding="1" cellspacing="0" style="margin: 0; width: 100%; padding: 1px; border-spacing: 0">
        <tr>
            <td style='width: 30%; padding: 2px; text-align: right;'>
                <label for='TypeId'>WebApp Id:</label></td>
            <td style='width: 70%; padding: 2px;'><%=Html.Encode(Model.term.TerminalId)%></td>
        </tr>
        <tr>
            <td style='width: 30%; padding: 2px; text-align: right;'>
                <label>WebApp Name:</label></td>
            <td style='width: 70%; padding: 2px;'><%= Html.TextBox("_TerminalName", Model.term.Name, new { style = "width:90%",id="_TerminalName" })%>
            </td>
        </tr>
        <tr>
            <td style='width: 30%; padding: 2px; text-align: right;'>
                <label>Show Screensaver:</label></td>
            <td style='width: 70%; padding: 2px;'><%=Html.CheckBox("_screensaver", Model.term.ShowScreensaver,new { id="_screensaver"})%>
            </td>
        </tr>
        <tr>
            <td style='width: 30%; padding: 2px; text-align: right;'>
                <label>Screensaver show after:</label></td>
            <td style='width: 70%; padding: 2px;'><%= Html.TextBox("_ScreensaverShowAfter", Model.term.ScreenSaverShowAfter, new { style = "width:90%",id="_ScreensaverShowAfter" })%>
            </td>
        </tr>

        <tr>
            <td style='width: 30%; padding: 2px; text-align: right;'>
                <label>Company:</label></td>
            <td style='width: 70%; padding: 2px;'>
                <%=Html.DropDownList("_CompanyId", new SelectList(Model.companies, "Value", "Text", Model.term.CompanyId), new { style = "width: 90%;",id="_CompanyId",onchange="fetchUserNameByCompanyId(this.value)" })%>
            </td>
        </tr>
        <tr>
            <td style='width: 30%; padding: 2px; text-align: right;'>
                <label>Max User:</label></td>
            <td style='width: 70%; padding: 2px;'>

                <input type="hidden" id="txtHiddenRoleID" value="<%=Server.HtmlEncode(HttpContext.Current.Session["Role_ID"].ToString())%>" />
                <input type="hidden" id="txtHiddenRoleName" value="<%=Server.HtmlEncode(HttpContext.Current.Session["Role_Name"].ToString())%>" />
                <%= Html.TextBox("txtMaxUserId", Model.term.MaxUserId, new { style = "width:148px;display:none",id="txtMaxUserId" })%>
                <select id="_MaxUser" style="width: 90%">
                </select>
            </td>
        </tr>
        <tr>
            <td style='width: 30%; padding: 2px; text-align: right;'>
                <label>T&A at work building object:</label></td>

            <td style='width: 70%; padding: 2px;'><%=Html.DropDownList("TARegisterBoId", new SelectList(
    Model.TABuildingList, "Value", "Text", Model.term.TARegisterBoId))%>
            </td>
        </tr>
        <tr>
            <td style='width: 30%; padding: 2px; text-align: right;'>
                <label>Approved Device:</label></td>
            <td style='width: 70%; padding: 2px;'><%=Html.CheckBox("_ApprovedDevice", Model.term.ApprovedDevice==true?true:false, new { id="_ApprovedDevice" })%>
            </td>
        </tr>
        <tr>
            <td style='width: 30%; padding: 2px; text-align: right;'>
                <label>InfoKiosk Mode:</label></td>
            <td style='width: 70%; padding: 2px;'><%=Html.CheckBox("_InfoKioskMode", Model.term.InfoKioskMode==true?true:false, new { id="_InfoKioskMode" })%>
            </td>
        </tr>
        <tr>
            <td style='width: 30%; padding: 2px; text-align: right;'>
                <label>Sound Alarms:</label></td>
            <td style='width: 70%; padding: 2px;'><%= Html.TextBox("_SoundAlarms", Model.term.SoundAlarms, new { style = "width:90%",id="_SoundAlarms"  })%>
            </td>
        </tr>
        <tr>
            <td style='width: 30%; padding: 2px; text-align: right;'>
                <label>MaximumAlarmOnFirstPage:</label></td>
            <td style='width: 70%; padding: 2px;'><%= Html.TextBox("_ShowMaxAlarmsFistPage", Model.term.ShowMaxAlarmsFistPage, new { style = "width:90%",id="_ShowMaxAlarmsFistPage" })%>
            </td>
        </tr>
        <tr>
            <td style='width: 30%; padding: 2px; text-align: right;'>
                <label>LastLogin:</label></td>
            <td style='width: 70%; padding: 2px;'><%= Html.TextBox("_LastLogin", Model.term.LastLogin, new { style = "width:90%",@readonly="readonly" })%>
            </td>
        </tr>
    </table>
</form>

<script type="text/javascript">
    $(document).ready(function () {
        $('#loading').fadeOut();    
        //var str = "Company Manager"; 
    });

    function loadAllUserForSuperAdmin() {
        $('#loading').fadeIn();
        $.ajax({
            type: "POST",
            url: "/Terminal/fetchUserNameBySuperAdmin",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (r) {
               // debugger;
                var ddlCustomers = $("[id*=_MaxUser]");
                ddlCustomers.empty().append('<option selected="selected" value="0">Please select</option>');
                $.each(r, function () {
                    ddlCustomers.append($("<option></option>").val(this['Id']).html(this['FirstName'] + ' ' + this['LastName']));
                });
                if ($("#txtMaxUserId").val() != "") {
                    setTimeout(function () {
                        $("#_MaxUser").val($("#txtMaxUserId").val());
                    }, 1000)
                }
                $('#loading').fadeOut();              
            }
        });      
    }
    function fetchUserNameByCompanyId(val) {
        var rolename = $("#txtHiddenRoleName").val();
        var res = rolename.match(/Company Manager/g);
        if (res) {
            $.ajax({
                type: "POST",
                url: "/Terminal/fetchUserNameByCompanyId",
                data: "{'Id':" + val + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (r) {
                 //   debugger;
                    var ddlCustomers = $("[id*=_MaxUser]");
                    ddlCustomers.empty().append('<option selected="selected" value="0">Please select</option>');
                    $.each(r, function () {
                        ddlCustomers.append($("<option></option>").val(this['Id']).html(this['FirstName'] + ' ' + this['LastName']));
                    });
                    if ($("#txtMaxUserId").val() != "") {
                        setTimeout(function () {
                            $("#_MaxUser").val($("#txtMaxUserId").val());
                        }, 1000)
                    }

                }
            });
        }

    }
</script>
<script type="text/javascript">
    $(document).ready(function () {
        $("#_ShowMaxAlarmsFistPage").keydown(function (e) {
            // Allow: backspace, delete, tab, escape, enter and .
            if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110]) !== -1 ||
                // Allow: Ctrl+A, Command+A
                (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
                // Allow: home, end, left, right, down, up
                (e.keyCode >= 35 && e.keyCode <= 40)) {
                // let it happen, don't do anything
                return;
            }
            // Ensure that it is a number and stop the keypress
            if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                e.preventDefault();
            }
        });
    });
</script>
<style type="text/css">
    .ui-dialog {
        width: 500px !important;
    }
</style>
