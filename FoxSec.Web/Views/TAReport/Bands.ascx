<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%   
        Html.DevExpress().Button(settings =>
    {
        settings.Name = "Button2";
        settings.UseSubmitBehavior = true;
        settings.Text = "Export";

        settings.RouteValues = new { Controller = "TAReport", Action = "ExportTo2" };
    }).GetHtml();
    Html.DevExpress().GridView(settings =>
    {
        settings.Name = "gvBands";
        //settings.CallbackRouteValues = new { Controller = "Columns", Action = "Bands" };

        settings.Width = Unit.Percentage(100);

        settings.Settings.ShowGroupPanel = true;
        settings.SettingsText.GroupPanel = @"<b>Informācija no Elektroniskās darba laika uzskaites sistēmas (EDLUS) reģistrētajiem auditācijas pierakstiem  par periodu _________________________<b>(taksācijas gads, mēnesis)";
        //settings.Styles.GroupPanel.Font.Bold = true;
        settings.Styles.GroupPanel.HorizontalAlign = HorizontalAlign.Center;

        settings.Columns.AddBand(c =>
        {
            c.Caption = "<b>Nr.p.k.</b>";
            //c.FieldName = "Nr.p.k.";
            c.HeaderStyle.Wrap = DefaultBoolean.True;
        });
        settings.Columns.AddBand(c =>
        {
            c.Caption = "<b>Informācijas iesniegšanas datums (dd.mm.gggg.)</b>";
            c.HeaderStyle.Wrap = DefaultBoolean.True;
        });

        settings.Columns.AddBand(info =>
        {
            info.Caption = "<b>Taksācijas periods</b>";
            info.Columns.AddBand(c1 =>
            {
                c1.Caption = "<b>Datums</b>";
                c1.HeaderStyle.Wrap = DefaultBoolean.True;
            });
            info.Columns.AddBand(c2 =>
            {
                c2.Caption = "<b>Mēnesis</b>";
                c2.HeaderStyle.Wrap = DefaultBoolean.True;
            });
            info.Columns.AddBand(c3 =>
            {
                c3.Caption = "<b>Gads</b>";
                c3.HeaderStyle.Wrap = DefaultBoolean.True;
            });
            info.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        });

        settings.Columns.AddBand(band1 =>
        {
            band1.Caption = "<b>Labotie dati jeb auditācijas pieraksti</b>";
            band1.Columns.AddBand(empdet =>
            {
                empdet.Caption = "<b>Nodarbinātais</b>";
                empdet.Columns.Add("<b>Vārds</b>");
                empdet.Columns.Add("<b>Uzvārds</b>");
                empdet.Columns.Add(c =>
                {
                    c.FieldName = "<b>Personas kods</b>";
                    c.HeaderStyle.Wrap = DefaultBoolean.True;
                });
                empdet.Columns.AddBand(c =>
                {
                    c.Caption = "<b>Ja personai nav personas koda</b>";
                    c.HeaderStyle.Wrap = DefaultBoolean.True;

                    c.Columns.Add(c1 =>
                    {
                        c1.FieldName = "<b>Dzimšanas datums, mēnesis, gads, vai vīzas vai uzturēšanās atļaujas numurs</b>";
                        c1.HeaderStyle.Wrap = DefaultBoolean.True;
                    });
                });
                empdet.Columns.Add("<b>Amats</b>");
                empdet.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            });

            band1.Columns.AddBand(initialdates =>
            {
                initialdates.Caption = "<b>Sākotnējie dati</b>";
                initialdates.Columns.Add(c =>
                {
                    c.FieldName = "<b>Ienākšanas (hh:mm:ss)</b>";
                    c.HeaderStyle.Wrap = DefaultBoolean.True;
                });
                initialdates.Columns.Add(c =>
                {
                    c.FieldName = "<b>Iziešanas (hh:mm:ss)</b>";
                    c.HeaderStyle.Wrap = DefaultBoolean.True;
                });
                initialdates.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            });
            band1.Columns.AddBand(correcteddates =>
            {
                correcteddates.Caption = "<b>Labotie dati</b>";
                correcteddates.Columns.Add(c =>
                {
                    c.FieldName = "<b>Ienākšanas (hh:mm:ss)</b>";
                    c.HeaderStyle.Wrap = DefaultBoolean.True;
                });
                correcteddates.Columns.Add(c =>
                {
                    c.FieldName = "<b>Iziešanas (hh:mm:ss)</b>";
                    c.HeaderStyle.Wrap = DefaultBoolean.True;
                });
                correcteddates.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            });
            band1.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        });


        settings.Columns.AddBand(c =>
        {
            c.Caption = "<b>Paskaidrojums</b>";
            c.HeaderStyle.Wrap = DefaultBoolean.True;
        });
        //settings.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;

    }).Bind(Model).GetHtml();
%>

