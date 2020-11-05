<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>


<html>
<head>
    <link href="../../css/sweetalert.css" rel="stylesheet" />
    <script src="../../Scripts/sweetalert.min.js"></script>

    <script>       
    function SelectedRows() {
     
            $.ajax({
                type: "Post",
                url: "/TAReport/ExportToParamsComingLeaving",
                dataType: 'json',
                traditional: false,
                async: false,
                data: {
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
        <table align="left">
            <tr>
                <td>
                    <%Html.DevExpress().Button(settings =>
                        {
                            settings.Name = "Button2";
                            settings.UseSubmitBehavior = true;
                            settings.Text = ViewResources.SharedStrings.BtnExport;
                            settings.ClientSideEvents.Click = "SelectedRows";
                           settings.RouteValues = new { Controller = "TAReport", Action = "ExportToComingLeaving" };
                        }).GetHtml();
                    %>                     
                </td>               
            </tr>
            <tr>
                <td colspan="2"><br /></td>
            </tr>
        </table>
    </div>
    <br />
   <%

       Html.DevExpress().PivotGrid(settings =>
       {
           settings.Name = "pivotGrid";
           settings.CallbackRouteValues = new { Controller = "TAReport", Action = "TAStartEnd" };
           // settings.CustomActionRouteValues = new { Controller = "TAReport", Action = "MounthBatchGridEditA" };

           //settings.OptionsView.VerticalScrollingMode = PivotScrollingMode.Virtual;
           ////settings.OptionsView.HorizontalScrollingMode = PivotScrollingMode.Virtual;
           //settings.OptionsView.VerticalScrollBarMode = ScrollBarMode.Auto;
           //settings.OptionsView.HorizontalScrollBarMode = ScrollBarMode.Auto;

           settings.Width = Unit.Percentage(100);
           settings.OptionsView.HorizontalScrollBarMode = ScrollBarMode.Visible;
           settings.OptionsView.HorizontalScrollingMode = PivotScrollingMode.Standard;
           settings.ControlStyle.Border.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(1);
           settings.Styles.CellStyle.Border.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(1);
           settings.StylesPager.Pager.Border.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(1);
           settings.ControlStyle.BorderBottom.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(2);

           //settings.Width = Unit.Percentage(100);
           //settings.Height = Unit.Percentage(100);
           //settings.OptionsView.HorizontalScrollBarMode = ScrollBarMode.Auto;
           //settings.OptionsView.VerticalScrollingMode = DevExpress.Web.ASPxPivotGrid.PivotScrollingMode.Virtual;
           //settings.OptionsView.VerticalScrollBarMode = ScrollBarMode.Auto;

           // settings.CustomCellDisplayText = new DevExpress.Web.ASPxPivotGrid.PivotCellDisplayTextEventHandler(CustomCellDisplayText);
           settings.OptionsView.ShowDataHeaders = false;
           settings.OptionsView.ShowFilterHeaders = false;

           settings.OptionsView.ShowColumnHeaders = false;

           settings.OptionsPager.Visible = false;

           settings.ControlStyle.Border.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(1);
           //settings.ControlStyle.BorderBottom.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(1);
           settings.ControlStyle.Border.BorderColor = System.Drawing.Color.Gray;
           //settings.OptionsView.ShowColumnTotals = false;
           settings.OptionsView.ShowCustomTotalsForSingleValues = false;
           //settings.OptionsView.ShowAllTotals();
           settings.OptionsView.HideAllTotals();
           settings.Fields.Add(field =>
           {

               field.ValueStyle.Wrap = DefaultBoolean.False;
               field.Area = PivotArea.RowArea;
               field.Caption = ViewResources.SharedStrings.TAUsername;
               field.FieldName = "UserName";
               field.ValueStyle.HorizontalAlign = HorizontalAlign.Center;
               field.ValueStyle.VerticalAlign = VerticalAlign.Bottom;
               // field.AreaIndex = 0;

           });



           settings.Fields.Add(field =>
           {
               
               field.Area = PivotArea.ColumnArea;

               field.FieldName = "Started";
               field.Caption = "ReportDate";
               field.GroupInterval = PivotGroupInterval.DateMonthYear;
               field.ValueStyle.HorizontalAlign = HorizontalAlign.Center;
               field.ValueStyle.VerticalAlign = VerticalAlign.Bottom;


           });

           settings.Fields.Add(field =>
           {
               
               field.Area = PivotArea.ColumnArea;
               field.FieldName = "Started";
               field.Caption = "Quarter";
               field.GroupInterval = PivotGroupInterval.DateDay;
               field.ValueStyle.HorizontalAlign = HorizontalAlign.Center;
               field.ValueStyle.VerticalAlign = VerticalAlign.Bottom;
           });
           /*
           settings.Fields.Add(field =>
           {

               field.Area = PivotArea.DataArea;
               field.FieldName = "ValidityPeriods";
               //field.AreaIndex = 1;


               field.TotalValueFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
               field.TotalValueFormat.FormatString = "{0:n2}";

               //field.SummaryType = PivotSummaryType.Max;
               field.CellFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
               //field.CellFormat.FormatString = "{0:hh\\:mm}";
           });
           */

           settings.Fields.Add(field =>
           {
               
               field.Area = PivotArea.DataArea;
               field.FieldName = "Started";
               field.SummaryType = PivotSummaryType.Min;
               field.CellFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
               field.CellFormat.FormatString = "HH:mm";
               field.Caption = "In";
               //field.CellStyle.ForeColor = System.Drawing.Color.LightGreen;
               field.CellStyle.BorderRight.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(1);
               field.ValueStyle.HorizontalAlign = HorizontalAlign.Center;
               field.ValueStyle.VerticalAlign = VerticalAlign.Bottom;
           });

           settings.Fields.Add(field =>
           {

               field.Area = PivotArea.DataArea;
               field.FieldName = "Finished";
               field.SummaryType = PivotSummaryType.Max;
               field.CellFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
               field.CellFormat.FormatString = "HH:mm";
               field.Caption = "Out";
               //field.CellStyle.ForeColor = System.Drawing.Color.LightPink;
               field.CellStyle.BorderLeft.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(0);
               field.ValueStyle.HorizontalAlign = HorizontalAlign.Center;
               field.ValueStyle.VerticalAlign = VerticalAlign.Bottom;
           });
           settings.CustomCellDisplayText = (sender, e) =>
           {

               if (e.DataField.Caption.ToLower() == "in" || e.DataField.Caption.ToLower() == "out")
               {
                   if (e.DisplayText == "00:00" || e.DisplayText == "23:59")
                   {
                       e.DisplayText = "";
                   }
               }

           };
           settings.CustomCellStyle = (sender, e) =>
           {
               if (e.RowIndex % 2 == 0)
                   //  e.RowField.CellStyle.BackColor = System.Drawing.Color.LightGray;
                   e.CellStyle.BackColor = System.Drawing.Color.LightGray;
           };

           /*
           settings.Fields.Add(field => {
               field.Area = PivotArea.DataArea;
               field.SummaryType = PivotSummaryType.Min;
               field.UnboundFieldName = "Hour";
               field.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
               field.FieldName = "Hour";
               field.CellFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
               field.CellFormat.FormatString = "HH:mm";
           });

           settings.CustomUnboundFieldData = (sender, e) => {
               if (e.Field.FieldName == "Hour") {
                   double time = Convert.ToDouble(e.GetListSourceColumnValue("Hours"));
                   if (time == 0) { e.Value = null; }
                   else
                   {
                       e.Value = DateTime.MinValue.Add(TimeSpan.FromSeconds(time));
                   }
               }
           };
           *//*
           settings.CustomUnboundFieldData = (sender, e) => {

               GridView view = sender as GridView;
               int rowIndex = e.ListSourceRowIndex;
               if(e.Field.FieldName == "User.LastName")
               {

                   string name = Convert.ToString(e.Field.FieldName.("User.FirstName"));
                   e.Value = name;
               }

           };*/


       }).Bind(Model).GetHtml();
%>
</body>
</html>
