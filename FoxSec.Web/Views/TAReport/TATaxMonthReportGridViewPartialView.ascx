<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<html>
<head>
	<title></title>
</head>
<body>
	<%
		Html.DevExpress().Button(settings =>
		{
			settings.Name = "Button2";
			settings.UseSubmitBehavior = true;
			settings.Text = ViewResources.SharedStrings.BtnExport;
			settings.RouteValues = new { Controller = "TAReport", Action = "ExportTo1" };
		}).GetHtml();
	%>
	<br />
	<br />
	<%   Html.DevExpress().GridView(settings =>
		{
			settings.Name = "gvmain1";
			settings.Width = Unit.Percentage(100);
			settings.Settings.ShowGroupPanel = true;
			//settings.SettingsText.GroupPanel = @"<b>Informācija no Elektroniskās darba laika uzskaites sistēmas (EDLUS) par būvobjektā nodarbinātajām personām un nostrādātajām stundām par periodu " + ViewBag.RepYear+", "+ViewBag.RepMonth + "<b> (taksācijas gads, mēnesis)";
			settings.SettingsText.GroupPanel = "<b>" + string.Format((ViewResources.SharedStrings.ElectronicWorking).Replace("{0}",ViewBag.TACompany)) + " " + ViewBag.RepYear + ", " + ViewBag.RepMonth + "<b> (taksācijas gads, mēnesis)";
			settings.Styles.GroupPanel.HorizontalAlign = HorizontalAlign.Center;
			settings.Styles.GroupPanel.Wrap = DefaultBoolean.True;
			settings.SettingsBehavior.AllowSort = false;
			settings.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;

			settings.Columns.AddBand(cons =>
			{
				cons.Columns.AddBand(constructor =>
				{
					//constructor.Caption = "<b>Informācija par galvenā būvdarbu veicēja noslēgto<br> būvdarbu līgumu ar būvniecības ierosinātāju </b>";
					constructor.Caption = "<b>" + string.Format(ViewResources.SharedStrings.ContractorDetails) + "</b>";
					constructor.Columns.Add(c =>
					{
						//c.Caption = "<b>Būvniecības ierosinātāja<br> nosaukums </b>";
						c.Caption = "<b>" + ViewResources.SharedStrings.ConstructionInitiator + "</b>";
						c.FieldName = "ConstructorName";
						c.HeaderStyle.Wrap = DefaultBoolean.True;
						c.Width = Unit.Pixel(60);
						c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
					});
					constructor.Columns.Add(c =>
					{
						//c.Caption = "<b>Būvniecības ierosinātāja<br> nodokļu maksātāja<br> reģistrācijas kods </b>";						
						c.Caption = "<b>" + ViewResources.SharedStrings.ConstructionInitiatorTaxNo + "</b>";
						c.FieldName = "ConstructorTaxNo";
						c.HeaderStyle.Wrap = DefaultBoolean.True;
						c.Width = Unit.Pixel(60);
						c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;

					});
					constructor.Columns.Add(c1 =>
					{
						//c1.Caption = "<b>Līguma datums</b>";					
						c1.Caption = "<b>" + ViewResources.SharedStrings.ContractNr + "</b>";
						c1.FieldName = "ContractDate";
						c1.HeaderStyle.Wrap = DefaultBoolean.True;
						c1.Width = Unit.Pixel(60);
						c1.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
					});
					constructor.Columns.Add(c1 =>
					{
						//c1.Caption = "<b>Līguma summa<br> bez PVN</b>";				
						c1.Caption = "<b>" + ViewResources.SharedStrings.ConstructionAmount + "</b>";
						c1.FieldName = "ContractAmount";
						c1.HeaderStyle.Wrap = DefaultBoolean.True;
						c1.Width = Unit.Pixel(60);
						c1.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
					});
					constructor.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
					constructor.HeaderStyle.Wrap = DefaultBoolean.True;
					constructor.Width = Unit.Pixel(240);
				});

				cons.Columns.AddBand(constructor => { constructor.HeaderStyle.CssClass = "myBorder"; });
				cons.Columns.AddBand(constructor => { constructor.HeaderStyle.CssClass = "myBorder"; });
				cons.Columns.AddBand(constructor => { constructor.HeaderStyle.CssClass = "myBorder"; });
				cons.Columns.AddBand(constructor => { constructor.HeaderStyle.CssClass = "myBorder"; });
				cons.Columns.AddBand(constructor => { constructor.HeaderStyle.CssClass = "myBorder"; });
			});


		}).Bind(Model).GetHtml();
	%>
	<br />
	<% Html.RenderAction("TATaxMonthReportGridViewPartialViewDetails");%>
</body>
</html>


