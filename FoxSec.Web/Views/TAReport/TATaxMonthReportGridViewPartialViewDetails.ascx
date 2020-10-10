<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>


<%    
	Html.DevExpress().GridView(settings =>
	{
		settings.Name = "gvBands";
		//settings.CallbackRouteValues = new { Controller = "Columns", Action = "Bands" };

		settings.Settings.ShowGroupPanel = false;
		settings.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;

		settings.Columns.Add(c =>
		{
			//c.Caption = "Nr.p.k.";
			c.Caption = "<b>" + ViewResources.SharedStrings.Num + "</b>";
			c.FieldName = "SrNo";
			c.HeaderStyle.Wrap = DefaultBoolean.True;
		});

		settings.Columns.Add(c =>
		{
			//c.Caption = "Informācijas iesniegšanas datums (dd.mm.gggg.)";
			c.Caption = "<b>" + ViewResources.SharedStrings.InfoSentDate + "</b>";
			c.FieldName = "RprtDate";
			c.HeaderStyle.Wrap = DefaultBoolean.True;
		});

		settings.Columns.Add(c =>
		{
			//c.Caption = "<b>Taksācijas gads</b>";
			c.Caption = "<b>" + ViewResources.SharedStrings.TaxYear + "</b>";
			c.FieldName = "Year";
			c.HeaderStyle.Wrap = DefaultBoolean.True;
		});


		settings.Columns.Add(c =>
		{
			//c.Caption = "<b>Taksācijas mēnesis </b>";
			c.Caption = "<b>" + ViewResources.SharedStrings.TaxMonth + "</b>";
			c.FieldName = "Month";
			c.HeaderStyle.Wrap = DefaultBoolean.True;
		});

		settings.Columns.AddBand(info =>
		{
			// info.Caption = "<b>Informācija par būvlaukumā nodarbinātu personu</b>";
			info.Caption = "<b>" + ViewResources.SharedStrings.PersonInformation + "</b>";

			info.Columns.AddBand(empdet =>
			{
				//empdet.Caption = "<b>Nodarbinātā</b>";
				empdet.Caption = "<b>" + ViewResources.SharedStrings.Employee + "</b>";

				empdet.Columns.Add(c =>
				{
					//c.Caption = "<b>Vārds</b>";
					c.Caption = "<b>" + ViewResources.SharedStrings.Name + "</b>";
					c.FieldName = "FirstName";
					c.HeaderStyle.Wrap = DefaultBoolean.True;
				});
				empdet.Columns.Add(c =>
				{
					//c.Caption = "<b>Uzvārds</b>";
					c.Caption = "<b>" + ViewResources.SharedStrings.SurName + "</b>";
					c.FieldName = "LastName";
					c.HeaderStyle.Wrap = DefaultBoolean.True;
				});

				empdet.Columns.Add(c =>
				{
					//c.Caption = "<b>Personas kods</b>";
					c.Caption = "<b>" + ViewResources.SharedStrings.PersonalIdentificationNumber + "</b>";
					c.FieldName = "PersonalCode";
					c.HeaderStyle.Wrap = DefaultBoolean.True;
				});
				empdet.Columns.AddBand(c =>
				{
					//c.Caption = "<b>Ja personai nav personas koda</b>";
					c.Caption = "<b>" + ViewResources.SharedStrings.NoPersonalIdentificationNumber + "</b>";
					c.HeaderStyle.Wrap = DefaultBoolean.True;

					c.Columns.Add(c1 =>
					{
						//c1.Caption = "<b>Dzimšanas datums, mēnesis, gads, vai vīzas vai uzturēšanās atļaujas numurs</b>";
						c1.Caption = "<b>" + ViewResources.SharedStrings.BirthdayDateMonYear + "</b>";
						c1.FieldName = "Birthday";
						c1.HeaderStyle.Wrap = DefaultBoolean.True;
					});
				});

				empdet.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
			});

			info.Columns.AddBand(initialdates =>
			{
				//initialdates.Caption = "<b>Darba devēja</b>";
				initialdates.Caption = "<b>" + ViewResources.SharedStrings.Employer + "</b>";
				initialdates.Columns.Add(c =>
				{
					//c.Caption = "<b>Amats</b>";
					c.Caption = "<b>" + ViewResources.SharedStrings.Position + "</b>";
					c.FieldName = "Name";
					c.HeaderStyle.Wrap = DefaultBoolean.True;
				});
				initialdates.Columns.Add(c =>
				{
					//c.Caption = "<b>Nosaukums</b>";
					c.Caption = "<b>" + ViewResources.SharedStrings.EmployerName + "</b>";
					c.FieldName = "CompanyName";
					c.HeaderStyle.Wrap = DefaultBoolean.True;
				});
				initialdates.Columns.Add(c =>
				{
					//c.Caption = "<b>Vārds, uzvārds, ja darba devējs ir fiziskā persona</b>";
					c.Caption = "<b>" + ViewResources.SharedStrings.IndiEmployer + "</b>";
					c.HeaderStyle.Wrap = DefaultBoolean.True;
				});
				initialdates.Columns.Add(c =>
				{
					//c.Caption = "<b>Reģistrācijas kods vai personas kods</b>";
					c.Caption = "<b>" + ViewResources.SharedStrings.RegNo + "</b>";
					c.FieldName = "RegistrationNo";
					c.HeaderStyle.Wrap = DefaultBoolean.True;
				});
				initialdates.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
			});

			info.Columns.Add(c1 =>
			{
				//c1.Caption = "<b>Būvatļaujas numurs vai nekustamā īpašuma objekta kadastra apzīmējums</b>";
				c1.Caption = "<b>" + ViewResources.SharedStrings.BuildingPermission + "</b>";
				c1.FieldName = "ContractNo";
				c1.HeaderStyle.Wrap = DefaultBoolean.True;
			});

			info.Columns.Add(c2 =>
			{
				//c2.Caption = "<b>Faktiski nostrādātais darba laiks (stundās)</b>";
				c2.Caption = "<b>" + ViewResources.SharedStrings.ActualWorkingHour + "</b>";
				c2.FieldName = "Hours_Min";
				c2.HeaderStyle.Wrap = DefaultBoolean.True;
			});

			info.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
		});


		//settings.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;

	}).Bind(Model).GetHtml();
%>


