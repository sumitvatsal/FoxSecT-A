<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.UserListViewModel>" %>

<html>
<head>
    <link href="../../css/sweetalert.css" rel="stylesheet" />
    <script src="../../Scripts/sweetalert.min.js"></script>

    <script>
        $(document).ready(function () {
            var selval = $("#BuildingId").val();
            if (selval == "" || selval == null) {
                $('#lblselbuilding').html("All Buildings");
            }
            else {
                var seltext = $("#BuildingId option:selected").text();
                $('#lblselbuilding').html('<%= ViewResources.SharedStrings.BuildingsName%>'+": " + seltext);
            }
        });
        function SelectedRows() {
            var keys = TAMoveMounthViewSettings.GetSelectedKeysOnPage();

            if (keys == "") {
                $("#chkexport").val("N");
                swal(
                    'Oops...',
                    'You did not Select Any User!')
                return false;
            }

            $.ajax({
                type: "Post",
                url: "/TAReport/TaxExportToParamsTA1",
                dataType: 'json',
                traditional: true,
                data: {
                    a: keys
                },
                success: function (response) {
                }
            });
        }
        <%
        string countryflag = "";
        if (Session["Language"] == null)
        {
            countryflag = "EN";
        }
        else
        {
            countryflag = Session["Language"].ToString();
        }%>
</script>
    <meta name="viewport" content="width=device-width" />
    <title></title>
</head>
<body>
    <div>
        <table align="left">
            <tr>
                <td>
                    <%Html.DevExpress().Button(settings =>
                        {
                            settings.Name = "Button2";
                            settings.UseSubmitBehavior = true;
                            settings.Text = ViewResources.SharedStrings.BtnExport;
                            settings.ClientSideEvents.Click = "SelectedRows";
                            settings.RouteValues = new { Controller = "TAReport", Action = "ExportTo1" };
                        }).GetHtml();
                    %>                     
                </td>
                <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                 <td>
                    <label id="lblselbuilding"></label>
                </td>
            </tr>
            <tr>
                <td colspan="2"><br /></td>
            </tr>
        </table>
    </div>
    <br />
    <%  Html.RenderPartial("TATaxReportExportUsersN", Model.Users);%>
</body>
</html>
