<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.CompanyEditViewModel>" %>
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
<form id="createCamera">
    <div id="loading"></div>

    <table cellpadding="1" cellspacing="0" style="margin: 0; width: 100%; padding: 1px; border-spacing: 0">
        <tr>
            <td style='width: 40%; padding: 2px; text-align: right;'>
                <label><%:ViewResources.SharedStrings.ServerNr %>:</label></td>
            <td style='width: 60%; padding: 2px;'>
                <select id="_ServerNr" style="width: 90%">
                </select>
            </td>
        </tr>
        <tr>
            <td style='width: 40%; padding: 2px; text-align: right;'>
                <label><%:ViewResources.SharedStrings.CompanyNr %>:</label></td>
            <td style='width: 60%; padding: 2px;'>
                <input type="number" id="_CompanyNr" style="width: 90%" />
            </td>
        </tr>
        
        <tr>
            <td style='width: 40%; padding: 2px; text-align: right;'>
                <label><%:ViewResources.SharedStrings.CameraName %>:</label></td>
            <td style='width: 60%; padding: 2px;'>
                <input type="text" id="_Name" style="width: 90%;text-transform: capitalize;" />
            </td>
        </tr>


        <tr>
            <td style='width: 40%; padding: 2px; text-align: right;'>
                <label><%:ViewResources.SharedStrings.Port %>:</label></td>
            <td style='width: 60%; padding: 2px;'>
                <input type="number" id="_Port" style="width: 90%" />
            </td>
        </tr>
        <tr>
            <td style='width: 40%; padding: 2px; text-align: right;'>
                <label><%:ViewResources.SharedStrings.ResX %>:</label></td>
            <td style='width: 60%; padding: 2px;'>
                <input type="number" id="_ResX" style="width: 90%" />
            </td>
        </tr>
        <tr>
            <td style='width: 40%; padding: 2px; text-align: right;'>
                <label><%:ViewResources.SharedStrings.ResY %>:</label></td>
            <td style='width: 60%; padding: 2px;'>
                <input type="number" id="_ResY" style="width: 90%" />
            </td>
        </tr>
        <tr>
            <td style='width: 40%; padding: 2px; text-align: right;'>
                <label><%:ViewResources.SharedStrings.Skip %>:</label></td>
            <td style='width: 60%; padding: 2px;'>
                <input type="number" id="_Skip" style="width: 90%" />
            </td>
        </tr>
        <tr>
            <td style='width: 40%; padding: 2px; text-align: right;'>
                <label><%:ViewResources.SharedStrings.Delay %>:</label></td>
            <td style='width: 60%; padding: 2px;'>
                <input type="number" id="_Delay" style="width: 90%" />
            </td>
        </tr>
        <tr>
            <td style='width: 40%; padding: 2px; text-align: right;'>
                <label><%:ViewResources.SharedStrings.EnableLiveControls %>:</label></td>
            <td style='width: 60%; padding: 2px;'>
                <select id="_EnableLiveControls" style="width: 90%">
                    <option value="0">Select</option>
                    <option value="1">Yes</option>
                    <option value="2">No</option>
                </select>
            </td>
        </tr>
        <tr>
            <td style='width: 40%; padding: 2px; text-align: right;'>
                <label><%:ViewResources.SharedStrings.QuickPreviewSeconds %>:</label></td>
            <td style='width: 60%; padding: 2px;'>
                <input type="number" id="_QuickPreviewSeconds" style="width: 90%" />
            </td>
        </tr>
    </table>
</form>

<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        $('#loading').fadeOut();
    });


</script>
