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
                $('#lblselbuilding').html('<%= ViewResources.SharedStrings.BuildingsName%>' + ": " + seltext);
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
                url: "/TAReport/ExportToParams",
                dataType: 'json',
                traditional: false,
                async: false,
                data: {
                    a: keys,
                    Reporttype: $('#Reporttype').val(),
                    ReportFormat: $('#ReportFormat').val(),
                },
                success: function (response) {
                }
            });
        }

        $('#Reporttype1').change(function () {
            var date1 = $('#FromDateTA').val();

            var BName = $("#BuildingId :selected").text();

            if (BName == "") {
                BName = "0";
            }
            var cc = $("#newcompnayid").val();

            if ($('#Reporttype').val() == 2) {
                $.ajax({
                    type: 'GET',
                    url: '/TAReport/TAReportExportReportByDays',
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
                return false;
            }
            else {

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
                return false;
            }

        });
    </script>
    <meta name="viewport" content="width=device-width" />
    <title></title>
</head>
<body>
    <div>
        <table>
            <tr>
                <td style='width: 10%; vertical-align: top;'>
                    <input type="hidden" id="chkexport" />
                    <label for='Reporttype'>Type</label><br />
                    <select id="Reporttype">
                        <option value="1"><%:ViewResources.SharedStrings.TADetailReport%></option>
                        <option value="2"><%:ViewResources.SharedStrings.TAReport%></option>
                        <option value="3"><%:ViewResources.SharedStrings.TADetailReportOld%></option>
                    </select>
                </td>
                <td style='width: 10%; vertical-align: top;'>
                    <label for='ReportFormat'>Format</label><br />
                    <select id="ReportFormat">
                        <option value="1">.PDF</option>
                        <option value="2">.XLS</option>
                    </select>
                </td>
                <td>
                    <br />
                    <%Html.DevExpress().Button(settings =>
                        {
                            settings.Name = "Button2";
                            settings.UseSubmitBehavior = true;
                            settings.Text = "Export";
                            settings.ClientSideEvents.Click = "SelectedRows";
                            settings.RouteValues = new { Controller = "TAReport", Action = "ExportTo" };
                        }).GetHtml();
                    %>                     
                </td>

                <td style='width: 60%; vertical-align: top;'>
                    <label>&nbsp;</label><br />
                    <label id="lblselbuilding"></label>
                </td>
            </tr>
        </table>
    </div>
    <br />
    <%  Html.RenderPartial("TAReportExportUsers", Model.Users);%>
</body>
</html>
