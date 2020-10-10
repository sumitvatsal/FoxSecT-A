<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.UserListViewModel>" %>

<html>
<head>
    <link href="../../css/sweetalert.css" rel="stylesheet" />
    <script src="../../Scripts/sweetalert.min.js"></script>
    <script>
        function OnClick(s, e) {
            var keys = VisitorsUserDetails.GetSelectedKeysOnPage();
            if (keys == "") {
                $("#chkexport").val("N");
                swal(
                    'Oops...',
                    'You did not Select User Id!')
                return false;
            }

            else {

                VisitorsUserDetails.GetSelectedFieldValues('UserPermissionGroupName', function (value) {
                    $.ajax({
                        url: "/Visitors/SelectedUserIdParam",
                        type: "POST",
                        dataType: "text",
                        traditional: true,
                        data: { PermissionName: value },
                        success: function (data) {
                            //ShowDialog('UserId Selected', 2000, true); 
                            //alert("Selected User Id:" + data);
                            SaveUserPermission(data);
                        }
                    });
                });
            }

        }

        function OnClick2() {
            var keys = VisitorsUserDetails.GetSelectedKeysOnPage();

            if (keys == "") {
                $("#chkexport").val("N");
                swal(
                    'Oops...',
                    'You did not Select User Id!')
                return false;
            }
            else {
                VisitorsUserDetails.GetSelectedFieldValues("Id;UserPermissionGroupName", OnGetSelectedFieldValues);
            }
        }

        function OnGetSelectedFieldValues(result) {
            for (var i = 0; i < result.length; i++) {
                for (var j = 0; j < result[i].length; j++) {

                    // alert(result[i][j]);
                    if (j == 0)
                        var userId = result[i][j];
                    else
                        var userPermission = result[i][j];

                }
            }
            SaveUsersParamDetails(userId, userPermission);
        }

        function SaveUserPermission(data) {
            alert(data);
            $("div#userId").dialog("close");
            $("input#UserPermission").val(data);
        }

        function SaveUserId(data) {
            alert(data);
            $("div#userId").dialog("close");
        }
        function SaveUsersParamDetails(userId, userPermission) {
            $("input#UserId").val(userId);
            $("input#UserPermission").val(userPermission);
            $("div#divuserper").dialog("close");
            enabledisablebutton();
        }
    </script>

    <title>List of Users</title>
</head>
<body>
    <div>
        <table>
            <tr>
                <td style='width: 10%; vertical-align: top;'>
                    <input type="hidden" id="chkexport" />

                    <%Html.DevExpress().Button(settings =>
                        {
                            settings.Name = "Button2";
                            settings.UseSubmitBehavior = true;
                            settings.Text =ViewResources.SharedStrings.JoinPermission;
                            settings.ClientSideEvents.Click = "OnClick2";
                            //settings.ClientSideEvents.Click = "SelectedRows";
                            //settings.RouteValues = new { Controller = "Visitors", Action = "SelectedUserID"};
                        }).GetHtml();
                    %> 
                    
                </td>
            </tr>
        </table>
    </div>
    <br />
    <%Html.RenderPartial("VisitorsExportUsersDetails", Model.Users);%>
</body>
</html>
