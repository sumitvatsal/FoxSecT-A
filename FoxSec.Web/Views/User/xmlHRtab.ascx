<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FoxSec.Web.ViewModels.HRmodel>" %>
<%@ Import Namespace="System.Data" %>
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
        var fieldsArray = new Array();
        var hdn = new Array();
        $.ajax({
            url: "/User/GetFSHRlist",
            type: "GET",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                hdn = data;
            },
            error: function (result) {
            }
        });
        function SelectedRows() {
            $("#loader").fadeIn();

            setTimeout(function () {
                var a = hdn;
                var fieldNames = '';
                for (var i = 1; i < xmlHRlist.columns.length; i++) {
                    //fieldNames.push(xmlHRlist.Columns[i].FieldName);
                    if (i == 1) {

                        fieldNames = xmlHRlist.columns[i].fieldName;
                    }
                    else {
                        fieldNames = fieldNames + ';' + xmlHRlist.columns[i].fieldName;
                    }
                    fieldsArray.push(xmlHRlist.columns[i].fieldName);
                }

                //var keys = CsvHRList.GetSelectedKeysOnPage();//GetSelectedFieldValues
                xmlHRlist.GetSelectedFieldValues(fieldNames, OnGetSelectedFieldValues);

            }, 2000)
        }

        function OnGetSelectedFieldValues(selectedValues) {

            var SelectedKeys = new Array();
            var ar = fieldsArray;
            var list = new Array();
            for (i = 0; i < selectedValues.length; i++) {
                var RowVal = new Array();
                for (j = 0; j < selectedValues[i].length; j++) {
                    var colNm = "";
                    $.each(hdn, function (index, item) {
                        if (item.HRFieldname == ar[j]) {
                            colNm = item.FoxSecFieldName;
                            var xmlfieldVal = {
                                columnName: colNm,
                                ColVal: selectedValues[i][j]
                            }
                            RowVal.push(xmlfieldVal);
                        }
                    })

                }
                var xmlHR = {
                    xmlRowVal: RowVal
                }
                //var datatbl = {
                //    PersonalCode: selectedValues[i][0],
                //    FirstName: selectedValues[i][1],
                //    LastName: selectedValues[i][2],
                //    LoginName: selectedValues[i][3],
                //    Email: selectedValues[i][4],
                //    CreatedBy: selectedValues[i][5],
                //    Name: selectedValues[i][6],
                //    ValidFrom: selectedValues[i][7],
                //    ValidTo: selectedValues[i][8],
                //    Serial: selectedValues[i][9],
                //    Dk: selectedValues[i][10]
                //}
                list.push(xmlHR);
                SelectedKeys.push(selectedValues[i][0]);
            }
            keys = JSON.stringify(list);

            if (selectedValues.length > 100) {
                $("#loader").fadeOut();
                ShowDialog("Error! More than 100 records can't be selected.", 4000, false);
            }
            else {

                $.ajax({
                    type: "Post",
                    url: "/User/ImportUsers_xml",
                    contentType: "application/json; charset=utf-8",
                    data: keys,
                    beforeSend: function () { ShowDialog("Importing...", 1000, true); },
                    success: function (result) {
                        if (result.toLocaleLowerCase().indexOf("error") != -1) {
                            ShowDialog(result, 4000, false);
                            if (result == "Error! More than 100 records can't be selected.") {
                                xmlHRlist.UnselectRowsByKey(SelectedKeys);
                            }
                        }
                        else {
                            ShowDialog(result, 4000, true);
                            //var keys = xmlHRlist.GetSelectedKeysOnPage();
                            xmlHRlist.UnselectRowsByKey(SelectedKeys);
                        }
                        $("#loader").fadeOut();
                    }
                });
            }
            //return selectedValues;

        }
        //function SelectedRows() {
        //    var keys = HRList.GetSelectedKeysOnPage();
        //   // alert(keys);

        //    $.ajax({
        //        type: "Post",
        //        url: "/User/ImportUsers",
        //        dataType: 'json',
        //        traditional: true,
        //        data: { a: keys },
        //        beforeSend: function () { ShowDialog("Importing...", 1000, true); },
        //        success: function (result) {
        //            ShowDialog(result, 4000, true);
        //        }
        //    });
        //}

        function OnBulkAllCheckedChanged(s, e) {
            xmlHRlist.GetSelectedFieldValues("ID", FindKeysToDeselect);
        }

        function FindKeysToDeselect(selectedValues) {
            xmlHRlist.UnselectRowsByKey(selectedValues);
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
        <%--<%Html.HiddenFor(m => m.fshrlist, new { @id = "hdnFSHR"}); %>--%>
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
        Html.RenderPartial("xmlHRlist", Model);%>
</body>
</html>
