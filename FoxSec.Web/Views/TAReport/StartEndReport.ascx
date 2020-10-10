<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
    <%
    @Html.DevExpress().PivotGrid(
        settings =>
        {
            settings.Name = "pivotGrid";
            settings.CallbackRouteValues = new { Controller = "TAReport", Action = "TAMounthReportGridViewPartialA" };
            settings.OptionsView.ShowFilterHeaders = false;
            //settings.OptionsView.ShowColumnGrandTotalHeader = false;
            //settings.OptionsView.ShowColumnGrandTotals = false;
            settings.OptionsView.ShowRowGrandTotalHeader = false;
            settings.OptionsView.ShowRowGrandTotals = false;
            //settings.OptionsView.ShowColumnHeaders = false;
            //settings.OptionsView.ShowDataHeaders = false;
            // settings.OptionsView.VerticalScrollingMode = PivotScrollingMode.Virtual;
            settings.OptionsView.HorizontalScrollingMode = PivotScrollingMode.Virtual;
            // settings.OptionsView.VerticalScrollBarMode = ScrollBarMode.Auto;
            settings.OptionsView.HorizontalScrollBarMode = ScrollBarMode.Auto;
            settings.ControlStyle.Border.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(1);
            settings.Styles.CellStyle.Border.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(1);
            settings.StylesPager.Pager.Border.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(1);
            settings.ControlStyle.BorderBottom.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(2);
            settings.Height = Unit.Pixel(600);
            //settings.Width = System.Web.UI.WebControls.Unit.Percentage(90);
            settings.OptionsPager.Visible = false;
            settings.OptionsFilter.NativeCheckBoxes = true;
            //settings.OptionsView.ShowColumnTotals = false;
            settings.OptionsView.ShowCustomTotalsForSingleValues = false;
            //settings.OptionsView.ShowAllTotals();
            //settings.OptionsView.HideAllTotals();

            settings.Fields.Add(field =>
            {
                field.Area = PivotArea.RowArea;
                field.Caption = "Last Name";
                field.FieldName = "User.LastName";
            });
            settings.Fields.Add(field =>
            {
                field.Area = PivotArea.RowArea;

                field.Caption = "Name";
                field.FieldName = "User.FirstName";
            });

            settings.Fields.Add(field =>
            {
                field.Area = PivotArea.ColumnArea;

                field.FieldName = "ReportDate";
                field.Caption = "ReportDate";
                field.GroupInterval = PivotGroupInterval.DateMonthYear;
            });

            settings.Fields.Add(field =>
            {
                field.Area = PivotArea.ColumnArea;
                field.FieldName = "ReportDate";
                field.Caption = "Quarter";
                field.GroupInterval = PivotGroupInterval.DateDay;
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
                field.FieldName = "Hours";
            });
            settings.CustomCellDisplayText = (sender, e) =>
            {
                double time = Convert.ToDouble(e.Value);
                //   if (time == 0)
                if (time <= 0)
                    return;
                double time2 = Math.Floor((TimeSpan.FromSeconds(Convert.ToDouble(e.Value))).TotalSeconds);
                e.DisplayText = (time2 / 3600).ToString("F2");
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
            */
            /*
            settings.CustomCellValue =(sender, e) =>
            {
            decimal sum = 0;
            if (e.ColumnValueType == PivotGridValueType.Value)
            for (int i = 0; i < e.RowIndex ; i++)
            {
            sum += 1;
            }
            e.Value = sum;
            };*/
        }).Bind(Model).GetHtml();
    %>
