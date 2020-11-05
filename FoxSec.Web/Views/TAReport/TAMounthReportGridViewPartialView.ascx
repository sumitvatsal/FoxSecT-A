<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>


<%
    Html.DevExpress().PivotGrid(settings =>
    {
        settings.Name = "pivotGrid";

        settings.CallbackRouteValues = new { Controller = "TAReport", Action = "TAMounthReportGridViewPartialA" };
        //  settings.CustomActionRouteValues = new { Controller = "TAReport", Action = "MounthBatchGridEditA" };
        settings.OptionsPager.RowsPerPage = 31;

        //settings.OptionsView.VerticalScrollingMode = PivotScrollingMode.Virtual;
        ////settings.OptionsView.HorizontalScrollingMode = PivotScrollingMode.Virtual;
        //settings.OptionsView.VerticalScrollBarMode = ScrollBarMode.Auto;
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

        settings.Fields.Add(field =>
        {


            field.Caption = ViewResources.SharedStrings.TAUsername;

            field.FieldName = "UserName";
            field.ValueStyle.Wrap = DefaultBoolean.False;
            //  settings.Width = 100;
            // settings.Width = System.Web.UI.WebControls.Unit.Pixel(500);

            //field with property//

            //  settings.Width=Unit.Pixel(30);

            ///////////////////////////////////////////////////
            //public virtual string GetString(string name, CultureInfo culture);
            //field.CellStyle.BackColor = System.Drawing.Color.Red;
            //field.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            //field.CellStyle.VerticalAlign = VerticalAlign.Bottom;
            //field.CellStyle.BorderRight.BorderWidth= System.Web.UI.WebControls.Unit.Pixel(220);
            //field.CellStyle.BorderLeft.BorderColor=System.Drawing.Color.Red;
            field.ValueStyle.HorizontalAlign = HorizontalAlign.Center;
            field.ValueStyle.VerticalAlign = VerticalAlign.Bottom;
            // PivotGridField's CellStyle.HorizontalAlignment property


        });

        settings.Fields.Add(field =>
        {
            field.Area = PivotArea.ColumnArea;

            field.FieldName = "ReportDate";
            field.Caption = ViewResources.SharedStrings.TAReport;
            field.GroupInterval = PivotGroupInterval.DateMonthYear;//.DateMonth;
            field.ValueStyle.HorizontalAlign = HorizontalAlign.Center;
            field.ValueStyle.VerticalAlign = VerticalAlign.Bottom;
            field.Index = 0;
        });

        settings.Fields.Add(field =>
        {
            field.Area = PivotArea.ColumnArea;

            field.FieldName = "ReportDate";
            field.Caption = ViewResources.SharedStrings.TAData;

            field.GroupInterval = PivotGroupInterval.DateDay;

        });


        settings.Fields.Add(field =>
        {
            field.Area = PivotArea.DataArea;

            field.FieldName = "Hours";  //ViewResources.SharedStrings.TAHours;
                                        // field.Caption = field.HeaderDisplayText + " (Multiple)";
            field.Caption = ViewResources.SharedStrings.TAHours;


        });

        settings.CustomCellDisplayText = (sender, e) =>
        {
            if (e.ColumnValueType == PivotGridValueType.Total || e.ColumnValueType == PivotGridValueType.GrandTotal)
            {
                double time1 = Convert.ToDouble(e.Value);
                //time1 = time1 - 1800;
                string hours = Math.Floor((TimeSpan.FromSeconds(time1)).TotalHours) > 9 ? Math.Floor((TimeSpan.FromSeconds(time1)).TotalHours).ToString() : "0" + Math.Floor((TimeSpan.FromSeconds(time1)).TotalHours).ToString();
                string minutes = (TimeSpan.FromSeconds(time1)).Minutes > 9 ? (TimeSpan.FromSeconds(time1)).Minutes.ToString() : "0" + (TimeSpan.FromSeconds(time1)).Minutes;
                e.DisplayText = hours + ":" + minutes;

                return;
            }
            else if ((((DevExpress.Web.ASPxPivotGrid.PivotCellBaseEventArgs)(e)).CellItem.RowValueType).ToString() == "GrandTotal")
            {
                // e.DisplayText = DateTime.MinValue.Add(TimeSpan.FromSeconds(time)).ToString("HH:mm");
                double time1 = Convert.ToDouble(e.Value);
                //time1 = time1 - 1800;
                string hours = Math.Floor((TimeSpan.FromSeconds(time1)).TotalHours) > 9 ? Math.Floor((TimeSpan.FromSeconds(time1)).TotalHours).ToString() : "0" + Math.Floor((TimeSpan.FromSeconds(time1)).TotalHours).ToString();
                string minutes = (TimeSpan.FromSeconds(time1)).Minutes > 9 ? (TimeSpan.FromSeconds(time1)).Minutes.ToString() : "0" + (TimeSpan.FromSeconds(time1)).Minutes;
                e.DisplayText = hours + ":" + minutes;
                return;
            }
            double time = Convert.ToDouble(e.Value);
            //time = time - 1800;
            if (time <= 0)
            {
                //e.DisplayText = "00:00";
                return;
            }

            e.DisplayText = DateTime.MinValue.Add(TimeSpan.FromSeconds(time)).ToString("HH:mm");
        };

        settings.CustomCellStyle = (sender, e) =>
        {
            double time = Convert.ToDouble(e.Value);
            //time = time - 1800;
            if (e.RowIndex % 2 == 0)
                //  e.RowField.CellStyle.BackColor = System.Drawing.Color.LightGray;
                e.CellStyle.BackColor = System.Drawing.Color.LightGray;

            // if (Convert.ToDouble(time) < 28800) //8 hrs = 28800 sec
            // {
            //   e.CellStyle.ForeColor = System.Drawing.Color.Red;
            // }
        };

        settings.ClientSideEvents.CellClick = "function(sender, e) {EditReportValidate(e.RowIndex, e.ColumnIndex, e.Value, e.RowValue, e.ColumnValue);}";
        //window.alert('Row index: '+ e.RowValue +' Column index: '+ e.ColumnValue +' value:'+ e.Value) }";
    }).Bind(Model).GetHtml();

%>



<script type="text/javascript">

    function EditReportValidate(rowIndex, columnIndex, val, rowValue, ColumnValue) {
        if (rowValue != "") {
            MouthReportEditTAReport(rowIndex, columnIndex, rowValue);
        }
        //if (val != "") {
        //    MouthReportEditTAReport(rowIndex, columnIndex, rowValue);
        //} else {
        //    MouthReportEditTAReport(rowIndex, null, rowValue);
        //}
    }

    function MouthReportEditTAReport(rowIndex, columnIndex, rowValue) {
        debugger;
        $("div#modal-dialog-2").dialog({
            open: function () {
                $("div#user-modal-dialog-2").html("");
                $("div#modal-dialog-2").html("<div id='AreaUserEditWait' style='width: 100%; height:10% text-align:center'><span style='position:relative; top:45%' class='icon loader'></span></div>");
                $.ajax({
                    type: 'GET',
                    url: '/TAReport/MouthReportEditTAReport',
                    cache: false,
                    data: {
                        rowIndex: rowIndex,
                        columnIndex: columnIndex,
                        rowValue: rowValue
                    },
                    success: function (html) {
                        $("div#modal-dialog-2").html(html + '<p style="color: red">' +
                            '<br />' +
                            '<b>Note:</b>    Entered date format should be MM/dd/yyyy. For e.g. 12/30/2000.' +
                            '<br /><br />' +
                            '</p>');
                    }
                });
                $(this).parents('.ui-dialog-buttonpane button:eq(0)').focus();
            },
            resizable: false,
            width: 800,
            height: 600,
            modal: true,
            title: "<span class='ui-icon ui-icon-home' style='float:left; margin:1px 5px 0 0'></span>" +  '<%= ViewResources.SharedStrings.TAEdit%>',
            buttons: {
                '<%= ViewResources.SharedStrings.BtnClose %>': function () {
                    $("div#modal-dialog").dialog("close");
                    $(this).dialog("close");

                    OpenMonthReport("submit_edit_user", 500, "eesn peren");
                    //location.reload();

                }
            }
        });
        return false;
    }
</script>


