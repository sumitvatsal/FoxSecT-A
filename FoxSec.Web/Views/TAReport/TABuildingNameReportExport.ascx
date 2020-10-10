<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.UserListViewModel>" %>  

<html>
<head>
    <script>
        function SelectedRows() {
            var keys = TAMoveMounthViewSettings.GetSelectedKeysOnPage();


            $.ajax({
                type: "Post",
                url: "/TAReport/ExportToParamsCustomUser",
                dataType: 'json',
                traditional: true,
                data: {
                    a: keys,
                    Reporttype: $('#Reporttype').val(),
                    ReportFormat: $('#ReportFormat').val(),
                },
                success: function (response) {
                }
            });
        }
    </script>
    <meta name="viewport" content="width=device-width" />
    <title></title>
</head>
<body>
    <div>
         <table>
            <tr> 
                
                <%--<td style='width: 10%; vertical-align: top;'>
					<label for='reportformat'>format</label><br />
					<select id="reportformat"><option value="1">.pdf</option>
                    <option value="2">.xls</option></select>
				</td>--%>
                <td>
                    <%Html.DevExpress().Button(settings =>
                      {
                          settings.Name = "Button2";
                          settings.UseSubmitBehavior = true;
                          settings.Text = "Export To Excel";
                          settings.ClientSideEvents.Click = "SelectedRows";
                          settings.RouteValues = new { Controller = "TAReport", Action = "ExportToExcelNew"};
                      }).GetHtml();
                         %> 
                </td>
            </tr>
        </table>
    </div>
<br />
    <%  Html.RenderPartial("BuildingExportUser", Model.UserNew);%>
</body>
</html>
