<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.datatableListViewModel>" %>


<html>
<head>
    <style type="text/css">
        #loader {
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
    <script>
        var keys;
        function SelectedRows() {
            $("#loader").fadeIn();
            //var keys = CsvHRList.GetSelectedKeysOnPage();//GetSelectedFieldValues
            CsvHRList.GetSelectedFieldValues('ois_id_isik;isikukood;e_nimi;p_nimi;kasutajatunnus;toosuhte_algus;toosuhte_lopp;ylikooli_e_post;tookoha_aadress', OnGetSelectedFieldValues);

            // alert(keys);
        }

        function OnGetSelectedFieldValues(selectedValues) {
            var list = new Array();
            for (i = 0; i < selectedValues.length; i++) {
                var datatbl = {
                    ois_id_isik: selectedValues[i][0],
                    isikukood: selectedValues[i][1],
                    e_nimi: selectedValues[i][2],
                    p_nimi: selectedValues[i][3],
                    kasutajatunnus: selectedValues[i][4],
                    toosuhte_algus: selectedValues[i][5],
                    toosuhte_lopp: selectedValues[i][6],
                    ylikooli_e_post: selectedValues[i][7],
                    tookoha_aadress: selectedValues[i][8]
                }
                list.push(datatbl);
            }
            keys = JSON.stringify(list);
            $.ajax({
                type: "Post",
                url: "/User/ImportCsvUsers",
                //dataType: 'json',
                contentType: "application/json; charset=utf-8",
                //traditional: true,
                data: keys,
                beforeSend: function () { ShowDialog("Importing...", 1000, true); },
                success: function (result) {
                    ShowDialog(result, 4000, true);
                    $("#loader").fadeOut();
                }
            });
            //return selectedValues;

        }
    </script>
    <meta name="viewport" content="width=device-width" />
    <title></title>
</head>
<body>
    <div>
        <div id="loader" style="display: none">
        </div>
        <h2>Import users</h2>
        <%      Html.DevExpress().Button(settings =>
            {
                settings.Name = "Button2";
                settings.UseSubmitBehavior = true;
                settings.Text = "Add/Change";
                settings.ClientSideEvents.Click = "SelectedRows";
                // settings.RouteValues = new { Controller = "User", Action = "ImportUsers"};
            }).GetHtml();
        %>
    </div>
    <br />
    <% 
        Html.RenderPartial("CsvHRList", Model.datatables);%>
</body>
</html>
