<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.HRListViewModel>" %>

<html>
<head>
    <script>
        function SelectedRows() {
            var keys = HRList.GetSelectedKeysOnPage();
            // alert(keys);

            $.ajax({
                type: "Post",
                url: "/User/ImportUsers",
                dataType: 'json',
                traditional: true,
                data: { a: keys },
                beforeSend: function () { ShowDialog("Importing...", 1000, true); },
                success: function (result) {
                    ShowDialog(result, 4000, true);
                }
            });
        }
    </script>
    <meta name="viewport" content="width=device-width" />
    <title></title>
</head>
<body>
    <div>
        <h2>Import users</h2>
        <%      Html.DevExpress().Button(settings =>
            {
                settings.Name = "Button2";
                settings.UseSubmitBehavior = true;
                settings.Text = "Add/Change";
                settings.ClientSideEvents.Click = "SelectedRows";
                //settings.RouteValues = new { Controller = "User", Action = "ImportUsers"};
            }).GetHtml();
        %>
    </div>
    <br />
    <% 
        Html.RenderPartial("HRList", Model.HRItems);%>
</body>
</html>
