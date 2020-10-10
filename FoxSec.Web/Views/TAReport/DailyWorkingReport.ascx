<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<style>
    .myBorder {
        border-right-color: transparent;
        border-top-color: transparent;
        border-bottom-color: transparent;
    }
</style>
<script type="text/javascript">
    function OnClick(s, e) {
        var actionParams = $("form").attr("action").split("?OutputFormat=");
        actionParams[1] = s.GetMainElement().getAttribute("OutputFormatAttribute");
        $("form").attr("action", actionParams.join("?OutputFormat="));
    }
</script>
<%   
    Html.DevExpress().Button(settings =>
    {
        settings.Name = "Button2";
        settings.UseSubmitBehavior = true;
        settings.Text = "Export";
        settings.RouteValues = new { Controller = "TAReport", Action = "ExportTo1" };
    }).GetHtml();

    Html.DevExpress().GridView(settings1 =>
    {
        settings1.Name = "gvmain";
        settings1.Width = Unit.Percentage(100);
        settings1.SettingsText.EmptyDataRow = " ";
        settings1.Columns.AddBand(cc1 =>
        {
            Html.DevExpress().GridView(settings =>
            {
                settings.Name = "gvmain1";
                settings.Width = Unit.Percentage(100);
                settings.Settings.ShowGroupPanel = true;
                settings.SettingsText.GroupPanel = @"<b>Informācija no Elektroniskās darba laika uzskaites sistēmas (EDLUS) par būvobjektā nodarbinātajām personām un nostrādātajām stundām par periodu _________________________<b>(taksācijas gads, mēnesis)";
                settings.Styles.GroupPanel.HorizontalAlign = HorizontalAlign.Center;
                settings.Styles.GroupPanel.Wrap = DefaultBoolean.True;
                settings.Columns.AddBand(cons =>
                {
                    cons.Columns.AddBand(constructor =>
                    {
                        constructor.Caption = "<b>Informācija par galvenā būvdarbu veicēja noslēgto<br> būvdarbu līgumu ar būvniecības ierosinātāju </b>";
                        constructor.Columns.Add(c =>
                        {
                            c.FieldName = "<b>Būvniecības ierosinātāja<br> nosaukums </b>";
                            c.HeaderStyle.Wrap = DefaultBoolean.True;
                            c.Width = Unit.Pixel(60);
                            c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                        });
                        constructor.Columns.AddBand(c =>
                        {
                            c.Caption = "<b>Būvniecības ierosinātāja<br> nodokļu maksātāja<br> reģistrācijas kods </b>";
                            c.HeaderStyle.Wrap = DefaultBoolean.True;
                            c.Width = Unit.Pixel(60);
                            c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;

                        });
                        constructor.Columns.Add(c1 =>
                        {
                            c1.FieldName = "<b>Līguma datums</b>";
                            c1.HeaderStyle.Wrap = DefaultBoolean.True;
                            c1.Width = Unit.Pixel(60);
                            c1.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                        });
                        constructor.Columns.Add(c1 =>
                        {
                            c1.FieldName = "<b>Līguma summa<br> bez PVN</b>";
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
        });

        settings1.Columns.AddBand(cc1 =>
        {
            Html.DevExpress().GridView(settings =>
            {
                settings.Name = "gvBands";
                //settings.CallbackRouteValues = new { Controller = "Columns", Action = "Bands" };
                settings.Settings.ShowGroupPanel = false;
                settings.Columns.AddBand(c =>
                {
                    c.Caption = "<b>Nr.p.k.</b>";
                    //c.FieldName = "Nr.p.k.";
                    c.HeaderStyle.Wrap = DefaultBoolean.True;
                });
                settings.Columns.AddBand(c =>
                {
                    c.Caption = "<b>Informācijas iesniegšanas datums VID </b>";
                    c.HeaderStyle.Wrap = DefaultBoolean.True;
                });

                settings.Columns.AddBand(c =>
                {
                    c.Caption = "<b>Taksācijas gads  </b>";
                    c.HeaderStyle.Wrap = DefaultBoolean.True;
                });

                settings.Columns.AddBand(c =>
                {
                    c.Caption = "<b>Taksācijas mēnesis </b>";
                    c.HeaderStyle.Wrap = DefaultBoolean.True;
                });

                settings.Columns.AddBand(info =>
                {
                    info.Caption = "<b>Informācija par būvlaukumā nodarbinātu personu</b>";
                    info.Columns.AddBand(empdet =>
                    {
                        empdet.Caption = "<b>Nodarbinātā</b>";
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

                    info.Columns.AddBand(initialdates =>
                    {
                        initialdates.Caption = "<b>Darba devēja</b>";
                        initialdates.Columns.Add(c =>
                        {
                            c.FieldName = "<b>Amats</b>";
                            c.HeaderStyle.Wrap = DefaultBoolean.True;
                        });
                        initialdates.Columns.Add(c =>
                        {
                            c.FieldName = "<b>Nosaukums</b>";
                            c.HeaderStyle.Wrap = DefaultBoolean.True;
                        });
                        initialdates.Columns.Add(c =>
                        {
                            c.FieldName = "<b>Vārds, uzvārds, ja darba devējs ir fiziskā persona</b>";
                            c.HeaderStyle.Wrap = DefaultBoolean.True;
                        });
                        initialdates.Columns.Add(c =>
                        {
                            c.FieldName = "<b>Reģistrācijas kods vai personas kods</b>";
                            c.HeaderStyle.Wrap = DefaultBoolean.True;
                        });
                        initialdates.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    });

                    info.Columns.AddBand(c1 =>
                    {
                        c1.Caption = "<b>Būvatļaujas numurs vai nekustamā īpašuma objekta kadastra apzīmējums</b>";
                        c1.HeaderStyle.Wrap = DefaultBoolean.True;
                    });
                    info.Columns.AddBand(c2 =>
                    {
                        c2.Caption = "<b>Faktiski nostrādātais darba laiks (stundās)</b>";
                        c2.HeaderStyle.Wrap = DefaultBoolean.True;
                    });

                    info.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                });
                //settings.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
            }).Bind(Model).GetHtml();
        });
        //settings1.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
    }).Bind(Model).GetHtml();
%>

