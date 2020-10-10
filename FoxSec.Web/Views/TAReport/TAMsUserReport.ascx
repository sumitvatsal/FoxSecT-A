<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
    <%
		Html.DevExpress().GridView(settings => {
			settings.Name = "TAMoveMounthViewSettings";
			settings.KeyFieldName = "Id";

			// hiljem teen   settings.ClientSideEvents.CustomizationWindowCloseUp = "grid_CustomizationWindowCloseUp";
			settings.CallbackRouteValues = new { Controller = "TAReport", Action = "MyTAReportGridViewPartialA" };
			settings.SettingsEditing.BatchUpdateRouteValues = new { Controller = "TAReport", Action = "MyTAReportGridViewPartialA" };
			settings.Width = Unit.Percentage(100);
			settings.SettingsPager.PageSize = 19;
			/*
        settings.Columns.Add(column =>
        {
            column.FieldName = "Started";
            column.Caption = " ";
            // column.PropertiesEdit.DisplayFormatString = "d";
            column.PropertiesEdit.DisplayFormatString = "dddd ";

        });*/

			settings.Columns.Add(column =>
			{
				column.FieldName = "Started";
				column.Caption = " ";
				// column.PropertiesEdit.DisplayFormatString = "d";
				column.PropertiesEdit.DisplayFormatString = "dddd M/d/yyyy ";
			});
			settings.Columns.Add(column =>
			{
				column.FieldName = "Started";
				column.Caption = " ";
				// column.PropertiesEdit.DisplayFormatString = "d";
				column.PropertiesEdit.DisplayFormatString = "HH:mm";
			});
			//toTime
			settings.Columns.Add(column =>
			{
				column.FieldName = "Finished";
				column.Caption = " ";
				column.PropertiesEdit.DisplayFormatString = "d";
				column.PropertiesEdit.DisplayFormatString = "HH:mm";
			});
			settings.Columns.Add(column =>
			{
				column.FieldName = "Hours";
				column.Visible = false;

			});

			//Total
			settings.Columns.Add(column =>
			{
				column.FieldName = "Total";
				column.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
				column.PropertiesEdit.DisplayFormatString = "HH:mm";
				column.PropertiesEdit.NullDisplayText = " ";
			});

			settings.Columns.Add(column =>
			{
				column.FieldName = "Remark";
				column.Caption = "Comment";
			});
			settings.CustomUnboundColumnData = (sender, e) =>
			{
				if (e.Column.FieldName == "Total")
				{
					double time = Convert.ToDouble(e.GetListSourceFieldValue("Hours"));
					//if (time == 0)
					if (time <= 0)
					{ e.Value = null; }
					else
					{
						e.Value = DateTime.MinValue.Add(TimeSpan.FromSeconds(time));
					}
				}
			};

			//Summary
			settings.Settings.ShowFooter = true;
			settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Sum, "Hours");
			settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Custom, "Total");

			settings.CustomSummaryCalculate = (sender, e) =>
			{
				double time;
				if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
				{
					ASPxSummaryItem incomeSummary = (sender as ASPxGridView).TotalSummary["Hours"];
					Decimal income = Convert.ToDecimal(((ASPxGridView)sender).GetTotalSummaryValue(incomeSummary));
					time = Convert.ToDouble(income);

					string hours = Math.Floor((TimeSpan.FromSeconds(time)).TotalHours) > 9 ? Math.Floor((TimeSpan.FromSeconds(time)).TotalHours).ToString() : "0" + Math.Floor((TimeSpan.FromSeconds(time)).TotalHours).ToString();
					string minutes = (TimeSpan.FromSeconds(time)).Minutes > 9 ? (TimeSpan.FromSeconds(time)).Minutes.ToString() : "0" + (TimeSpan.FromSeconds(time)).Minutes;
					e.TotalValue = hours + ":" + minutes;
				}
			};
		}
).Bind(Model).GetHtml();
    %>
