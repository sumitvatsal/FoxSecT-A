<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<FoxSec.Web.ViewModels.LogOnModel>" %>

<asp:Content ID="loginTitle" ContentPlaceHolderID="TitleContent" runat="server">
    FoxSec WEB
</asp:Content>

<asp:Content ID="loginContent" ContentPlaceHolderID="MainContent" runat="server">

    <div id="content_login_form" style='display: none;'>
        <table>
            <tr>
                <td style='width: 35%; text-align: right; padding: 5px; vertical-align: middle;'>
                    <%: ViewResources.SharedStrings.AccountUserName%>:
                </td>
                <td style='width: 65%; text-align: justify; padding: 5px;'>
                    <input id='username' type='text' style='width: 140px;' />
                </td>
            </tr>
            <tr>
                <td style='width: 35%; text-align: right; padding: 5px; vertical-align: middle;'>
                    <%: ViewResources.SharedStrings.AccountPassword%>:
                </td>
                <td style='width: 65%; text-align: justify; padding: 5px;'>
                    <input id='password' type='password' style='width: 140px;' />
                </td>
            </tr>
        </table>
        <br />
        <hr />
        <div align="right">
            <input type="button" value="<%:ViewResources.SharedStrings.AccountLogOn %>" onclick="LogIn()" />
            <input type="button" value="<%:ViewResources.SharedStrings.BtnCancel %>" onclick="closedialog()" />
        </div>
    </div>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $("input:button").button();
            if ($("div#top-dialog").is(":visible")) $("div#top-dialog").parent().hide();
            if ($("div#modal-dialog").is(":visible")) $("div#modal-dialog").parent().hide();
            if ($("div#user-modal-dialog").is(":visible")) $("div#user-modal-dialog").parent().hide();
            if ($("div#delete-modal-dialog").is(":visible")) $("div#delete-modal-dialog").parent().hide();
            $("div#user-logon-modal-dialog").dialog({
                autoOpen: true,
                open: function (event, ui) {
                    $(this).append($("div#content_login_form").show());
                    $("input#username").focus();
                },
                resizable: false,
                width: 400,
                height: 190,
                modal: true,
                title: "<span class='ui-icon ui-icon-power' style='float:left; margin:1px 5px 0 0'></span><%: ViewResources.SharedStrings.AccountLogOn %>",
            });
            return false;
        });
        function closedialog() {
            $("div#user-logon-modal-dialog").dialog('close');
        }
        $("#password").keyup(function (event) {
            if (event.keyCode == 13) {
                LogIn();
            }
        });
        function LogIn() {
            name = $("input#username").val();
            pass = $("input#password").val();
            if (name.length == 0) {
                $("input#username").focus();
                return;
            }
            if (pass.length == 0) {
                $("input#password").focus();
                return;
            }
            var user = { UserName: name, Password: pass };
            $.ajax({
                type: "POST",
                url: '/Account/UserLogOn',
                dataType: "json",
                data: user,
                success: function (result) {
                    if (!result.IsSucceed) {
                        $("input#username").val("").focus();
                        $("input#password").val("");
                        ShowDialog(result.Msg, 4000);
                    }
                    else {
                        $("div#content_login_form").remove();
                        $("div#user-logon-modal-dialog").dialog("close");
                        window.location.href = '/Home/Index';
                    }
                }
            });
        }
    </script>

</asp:Content>
