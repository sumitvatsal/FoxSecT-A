<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Windows" %>
<%@ Import Namespace="FoxSec.Web.ViewModels" %>
<%@ Import Namespace="TimeZoneConverter" %>

<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v17.2, Version=17.2.15.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPivotGrid" TagPrefix="dx" %>
<%-- DXCOMMENT: Configure GridView --%>

<%      Html.DevExpress().GridView(settings =>
    {
        FoxSecDBContext db = new FoxSecDBContext();
        TimeZoneModel tmz = new TimeZoneModel();
        settings.Name = "TAReportMounthViewSettings";
        settings.KeyFieldName = "Id";
        settings.CallbackRouteValues = new { Controller = "TAReport", Action = "TAReportDetailedGrid" };
        settings.SettingsEditing.BatchUpdateRouteValues = new { Controller = "TAReport", Action = "TAReportDetailedGridEditA" };
        settings.SettingsEditing.Mode = GridViewEditingMode.EditFormAndDisplayRow;
        settings.SettingsEditing.AddNewRowRouteValues = new { Controller = "TAReport", Action = "AddNew" };
        settings.SettingsEditing.UpdateRowRouteValues = new { Controller = "TAReport", Action = "TAReportDetailedGridEditA" };
        settings.SettingsEditing.BatchEditSettings.EditMode = GridViewBatchEditMode.Cell;
        settings.SettingsEditing.BatchEditSettings.StartEditAction = GridViewBatchStartEditAction.Click;
        settings.SettingsEditing.BatchEditSettings.ShowConfirmOnLosingChanges = true;
        settings.SettingsEditing.ShowModelErrorsForEditors = true;
        settings.SettingsSearchPanel.AllowTextInputTimer = false;
        settings.SettingsSearchPanel.ShowApplyButton = true;
        settings.SettingsSearchPanel.ShowClearButton = false;
        settings.SettingsSearchPanel.HighlightResults = true;
        settings.SettingsSearchPanel.Visible = true;
        settings.Width = System.Web.UI.WebControls.Unit.Percentage(100);
        settings.Height = System.Web.UI.WebControls.Unit.Percentage(100);
        settings.SettingsPager.PageSize = 32;
        settings.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
        settings.Settings.VerticalScrollableHeight = 480;
        settings.ControlStyle.Paddings.Padding = System.Web.UI.WebControls.Unit.Pixel(0);
        settings.ControlStyle.Border.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(1);
        settings.ControlStyle.BorderBottom.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(2);
        settings.Styles.Cell.Border.BorderColor = System.Drawing.Color.Green; //???
        settings.Styles.Row.Border.BorderColor = System.Drawing.Color.Green; //???
        settings.Styles.DetailCell.Border.BorderColor = System.Drawing.Color.Green; //???
        settings.Styles.DetailRow.Border.BorderColor = System.Drawing.Color.Green; //???
        settings.ControlStyle.BorderBottom.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(2);

        settings.Columns.Add(column =>
        {
            column.FieldName = "UserName";
            column.Caption = "User";
            column.GroupIndex = 0;
            column.Width = 50;
            column.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
        });
        settings.Columns.Add("UserName");
        settings.Columns.Add(column =>
        {
            column.FieldName = "Started";
            column.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
            column.Caption = "Short By Date";
            column.PropertiesEdit.DisplayFormatString = "dddd M/d/yyyy ";
            column.Width = 200;
        });
        settings.Columns.Add(column =>
        {
            column.FieldName = "Started";
            column.Caption = "In";
            column.PropertiesEdit.DisplayFormatString = "HH:mm";
            column.Width = 50;
        });
        //toTime
        settings.Columns.Add(column =>
        {
            column.FieldName = "Finished";
            column.Caption = "Out";
            column.SetEditItemTemplateContent(c =>
            {
                Html.DevExpress().TextBox(textbox =>
                {
                    textbox.Name = "textbox";
                    textbox.Enabled = c.Grid.IsNewRowEditing;
                }).GetHtml();
            });

            column.PropertiesEdit.DisplayFormatString = "d";
            column.PropertiesEdit.DisplayFormatString = "HH:mm";
            column.Width = 50;
        });
        settings.Columns.Add(column =>
        {
            column.FieldName = "Hours";
            column.Visible = false;
        });
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

        settings.Columns.Add("", "Entry Camera").SetDataItemTemplateContent(c =>
        {
            Html.DevExpress().HyperLink(lnkSettings =>
            {
                string binddatetime = DataBinder.Eval(c.DataItem, "Started").ToString();
                ArrayList list = new ArrayList();
                string camera = " ";
                string Date = DateTime.Parse(binddatetime).ToShortDateString(); // For Date
                string time = DateTime.Parse(binddatetime).ToShortTimeString();
                //string TimeZoneID = "";
                DateTime dt = DateTime.Parse(time);

                string dt1 = dt.ToString("HH:mm");

                DateTime formtdate = DateTime.Parse(binddatetime);

                TimeSpan ts = formtdate.TimeOfDay;

                DateTime utcDateTime = formtdate;

                string finaldate = formtdate.ToString("yyyyMMdd").ToString();
                string _timeString = "";
                string timediiference = "";
                var array = dt1.Split(new string[] { ":", " " }, StringSplitOptions.RemoveEmptyEntries);
                string hhh = array[0];
                int hh = Convert.ToInt32(hhh);
                string h = "";
                if (hh < 10)
                {
                    h = "0" + hh.ToString();

                }
                else
                {
                    h = hh.ToString();
                }

                string m = array[1];
                int buildingid = 0;
                string inobject = DataBinder.Eval(c.DataItem, "StartedBoId").ToString();
                int buildingobjectid = Convert.ToInt32(inobject);
                var getbuilding = db.BuildingObject.Where(x => x.Id == buildingobjectid).FirstOrDefault();
                if (getbuilding != null)
                {
                    buildingid = getbuilding.BuildingId;
                    var builds = db.Buildings.FirstOrDefault(x => x.Id == buildingid);

                    if (builds != null)
                    {
                        //var building_tz = db..FirstOrDefault();
                        if (!String.IsNullOrEmpty(builds.TimezoneId))
                        {
                            var info = TimeZoneInfo.FindSystemTimeZoneById(builds.TimezoneId);//tzone);

                            utcDateTime = TimeZoneInfo.ConvertTimeToUtc(formtdate, info);
                        }
                        else
                        {
                            DateTime TimeDiffDate = formtdate;
                            double mins = ts.TotalMinutes;
                            timediiference = builds.TimediffGMTMinutes.ToString();
                            double td = (builds.TimediffGMTMinutes == null ? 0 : Convert.ToDouble(builds.TimediffGMTMinutes));
                            double totMins = mins - td;
                            TimeSpan timeS = TimeSpan.FromMinutes(totMins);
                            formtdate = formtdate.Date + timeS;
                            utcDateTime = formtdate;
                        }
                    }
                    else { }
                }

                List<string> value = new List<string>();
                string fsboc_query = @"select  ISNULL(CameraId,0) as CameraId   from FSBuildingObjectCameras where BuildingObjectId='" + Convert.ToInt32(inobject) + "'and IsDeleted='" + 0 + "'";
                var fsboc_list = db.Database.SqlQuery<FSBOC>(fsboc_query).ToList();
                foreach (var f in fsboc_list)
                {
                    int Entrycameraid = f.CameraId;

                    int? servernr = 0;
                    string starttime = "";
                    string playtime = "";
                    string portno = "";

                    string Regwidth = "";
                    string RegHeight = "";
                    string cameraNr = "";
                    string Uname = "";
                    string Password = "";
                    string IP = "";
                    string serverName = "";

                    var fsc = db.Database.SqlQuery<FSCamera_test>("select Port, ServerNr,QuickPreviewSeconds,Delay,ResX,ResY,CameraNr from FSCameras where Id='" + Entrycameraid + "'").SingleOrDefault();//FSCameras.SingleOrDefault(x => x.Id == Cameraid);
                    if (fsc != null)
                    {
                        portno = fsc.Port == null ? "8000" : fsc.Port.ToString();

                        servernr = fsc.ServerNr;
                        starttime = fsc.QuickPreviewSeconds.ToString();
                        playtime = fsc.Delay.ToString();
                        RegHeight = fsc.ResX == null ? "640" : fsc.ResX.ToString();
                        Regwidth = fsc.ResY == null ? "480" : fsc.ResY.ToString();
                        cameraNr = fsc.CameraNr.ToString();
                    }

                    var fsvs = db.FSVideoServers.SingleOrDefault(x => x.Id == servernr);
                    if (fsvs != null)
                    {
                        IP = fsvs.IP;
                        serverName = fsvs.Name;
                        Uname = fsvs.UID;
                        Password = fsvs.PWD;
                    }

                    value.Add(Entrycameraid.ToString());
                    value.Add(IP);
                    value.Add(starttime);
                    value.Add(playtime);
                    value.Add(portno);
                    value.Add(Regwidth);
                    value.Add(RegHeight);
                    value.Add(cameraNr);
                    value.Add(serverName);
                    value.Add(Uname);
                    value.Add(Password);

                    DateTime dtt = utcDateTime;

                    var utcSeconds = dtt.TimeOfDay.TotalSeconds - Convert.ToDouble(starttime);
                    TimeSpan uTC_time = TimeSpan.FromSeconds(utcSeconds);
                    dtt = dtt.Date + uTC_time;
                    var HH = dtt.Hour < 10 ? "0" + dtt.Hour : dtt.Hour.ToString();
                    var mm = dtt.Minute < 10 ? "0" + dtt.Minute : dtt.Minute.ToString();
                    var ss = dtt.Second < 10 ? "0" + dtt.Second : dtt.Second.ToString();
                    _timeString = HH + mm + ss;
                    //value.Add(_timeString);

                    var yy = dtt.Year.ToString();
                    var mn = dtt.Month < 10 ? "0" + dtt.Month : dtt.Month.ToString();
                    var dd = dtt.Day < 10 ? "0" + dtt.Day : dtt.Day.ToString();
                    finaldate = yy + mn + dd;

                }

                string ipaddress = "";
                string timetoplay = "";
                string duration = "";
                string port = "";
                string regwidth = "";
                string regheight = "";
                string camnrId = "";
                string serverna = "";
                string username = "";
                string paswd = "";
                int ii = 0;
                int j = 0;
                int M = 0;
                int n = 0;
                int p = 0;
                int rh = 0;
                int rw = 0;
                int cm = 0;
                for (ii = 0; ii <= value.Count - 1; ii += 11)
                {
                    camera = value[ii].ToString();
                    port = value[ii + 4].ToString();
                    if (port == "")
                    {
                        port = "8000";
                    }
                    timetoplay = value[ii + 2].ToString();
                    duration = value[ii + 3].ToString();
                    regwidth = value[ii + 5];
                    if (regwidth == "")
                    {
                        regwidth = "480";
                    }

                    regheight = value[ii + 6];
                    if (regheight == "")
                    {
                        regheight = "640";
                    }
                    camnrId = value[ii + 7].ToString();
                    ipaddress = value[ii + 1].ToString();
                    serverna = value[ii + 8].ToString();
                    username = value[ii + 9].ToString();
                    paswd = value[ii + 10].ToString();

                    double timevalue = Convert.ToDouble(duration);

                    DateTime date1 = Convert.ToDateTime(binddatetime);
                    DateTime date2 = date1.AddMinutes(-timevalue);
                    string timeto = date2.ToString();

                    string[] Separate = timeto.Split(' ');
                    string desiredTime = Separate[1].ToString();

                    string[] Separate2 = desiredTime.Split(':');
                    int Hour = Convert.ToInt32(Separate2[0]);

                    string correcthour = "";
                    if (Hour < 10)
                    {
                        correcthour = "0" + Hour;
                    }

                    else
                    {
                        correcthour = Hour.ToString();
                    }
                    string Min = Separate2[1];
                    string sec = Separate2[2];
                    
                    var playbefore = int.Parse(sec) - int.Parse(timetoplay);

                    if (playbefore < 0)
                    {
                        var newmintue = int.Parse(Min) - 1;
                        var nmin = int.Parse(Min) - 1;
                        Min = nmin.ToString();
                        var newsec = int.Parse(sec) + 60;
                        playbefore = newsec - int.Parse(timetoplay);
                    }
                    string quaterurl = "http://" + username + ":" + paswd + "@" + ipaddress + ":" + port + "/archive/media/" + serverna + "/DeviceIpint.";
                    int second = int.Parse(duration) * 1000;
                    string halfurl = quaterurl + camnrId + "/SourceEndpoint.video:0:0/";
                    string reghw = "width=" + regheight + ",height=" + regwidth + "@@";
                    string mainurl = reghw + halfurl + finaldate + "T" + _timeString + "." + "000" + "?format=mjpeg&speed=1***" + second + "";
                    list.Add(mainurl);
                }
                int c1 = 0;
                foreach (string i in list)
                {
                    string url = i.ToString();
                    Html.DevExpress().HyperLink(lnkSettings1 =>
                    {
                        c1++;
                        HyperLinkProperties properties1 = lnkSettings1.Properties as HyperLinkProperties;
                        //properties1.Target = "_blank";
                        lnkSettings1.NavigateUrl = "javascript:ViewLogDetail1('" + url + "')";
                        lnkSettings1.Properties.Text = "C" + c1 + "|";
                    }).Render();
                    c1 = 0 + c1;
                }
            }).Render();
        });



        settings.Columns.Add("", "Exit Camera").SetDataItemTemplateContent(c =>
        {
            Html.DevExpress().HyperLink(lnkSettings =>
            {
                string binddatetime = DataBinder.Eval(c.DataItem, "Finished").ToString();
                ArrayList list = new ArrayList();
                string camera = " ";
                string Date = DateTime.Parse(binddatetime).ToShortDateString(); // For Date
                string time = DateTime.Parse(binddatetime).ToShortTimeString();

                DateTime dt = DateTime.Parse(time);
                string dt1 = dt.ToString("HH:mm");
                DateTime formtdate = DateTime.Parse(binddatetime);
                string TimeZoneID = "";
                TimeSpan ts = formtdate.TimeOfDay;
                string timediiference = "";

                DateTime utcDateTime = formtdate;
                string finaldate = formtdate.ToString("yyyyMMdd").ToString();
                string _timeString = "";
                var array = dt1.Split(new string[] { ":", " " }, StringSplitOptions.RemoveEmptyEntries);
                string hhh = array[0];
                int hh = Convert.ToInt32(hhh);
                string h = "";
                if (hh < 10)
                {
                    h = "0" + hh.ToString();
                }
                else
                {
                    h = hh.ToString();
                }
                string m = array[1];
                bool k1 = (DataBinder.Eval(c.DataItem, "FinishedBoId") == null) ? true : false;
                if (k1 == true)
                {
                    Html.DevExpress().HyperLink(lnkSettings1 =>
                    {
                        HyperLinkProperties properties1 = lnkSettings1.Properties as HyperLinkProperties;
                        properties1.Target = "_blank";
                        lnkSettings1.NavigateUrl = HttpUtility.UrlDecode("");
                        properties1.Text = "Not Available";
                    }).Render();
                }
                else
                {
                    string inobject = DataBinder.Eval(c.DataItem, "FinishedBoId").ToString();
                    int buildingobjectid2 = Convert.ToInt32(inobject);
                    int buildingid1 = 0;

                    var getbuilding = db.BuildingObject.Where(x => x.Id == buildingobjectid2).FirstOrDefault();

                    if (getbuilding != null)
                    {
                        buildingid1 = getbuilding.BuildingId;

                        var builds = db.Buildings.FirstOrDefault(x => x.Id == buildingid1);

                        if (builds != null)
                        {
                            if (!String.IsNullOrEmpty(builds.TimezoneId))
                            {
                                var info = TimeZoneInfo.FindSystemTimeZoneById(builds.TimezoneId);
                                utcDateTime = TimeZoneInfo.ConvertTimeToUtc(formtdate, info);
                            }
                            else
                            {
                                DateTime TimeDiffDate = formtdate;
                                double mins = ts.TotalMinutes;
                                timediiference = builds.TimediffGMTMinutes.ToString();
                                double td = (builds.TimediffGMTMinutes == null ? 0 : Convert.ToDouble(builds.TimediffGMTMinutes));
                                double totMins = mins - td;
                                TimeSpan timeS = TimeSpan.FromMinutes(totMins);
                                formtdate = formtdate.Date + timeS;
                                utcDateTime = formtdate;
                            }
                        }
                        else { }

                    }

                    List<string> value = new List<string>();
                    string fsboc_query = @"select  ISNULL(CameraId,0) as CameraId   from FSBuildingObjectCameras where BuildingObjectId='" + Convert.ToInt32(inobject) + "' and IsDeleted='" + 0 + "'";
                    var fsboc_list = db.Database.SqlQuery<FSBOC>(fsboc_query).ToList();
                    foreach (var f in fsboc_list)
                    {
                        int Entrycameraid = f.CameraId;

                        int? servernr = 0;
                        string starttime = "";
                        string playtime = "";
                        string portno = "";

                        string Regwidth = "";
                        string RegHeight = "";
                        string cameraNr = "";
                        string Uname = "";
                        string Password = "";
                        string IP = "";
                        string serverName = "";

                        var fsc = db.Database.SqlQuery<FSCamera_test>("select Port, ServerNr,QuickPreviewSeconds,Delay,ResX,ResY,CameraNr from FSCameras where Deleted=0 and Id='" + Entrycameraid + "'").SingleOrDefault();//FSCameras.SingleOrDefault(x => x.Id == Cameraid);
                        if (fsc != null)
                        {
                            portno = fsc.Port == null ? "8000" : fsc.Port.ToString();

                            servernr = fsc.ServerNr;
                            starttime = fsc.QuickPreviewSeconds.ToString();
                            playtime = fsc.Delay.ToString();
                            RegHeight = fsc.ResX == null ? "640" : fsc.ResX.ToString();
                            Regwidth = fsc.ResY == null ? "480" : fsc.ResY.ToString();
                            cameraNr = fsc.CameraNr.ToString();

                        }

                        var fsvs = db.FSVideoServers.SingleOrDefault(x => x.Id == servernr);
                        if (fsvs != null)
                        {
                            IP = fsvs.IP;
                            serverName = fsvs.Name;
                            Uname = fsvs.UID;
                            Password = fsvs.PWD;
                        }

                        value.Add(Entrycameraid.ToString());
                        value.Add(IP);
                        value.Add(starttime);
                        value.Add(playtime);
                        value.Add(portno);
                        value.Add(Regwidth);
                        value.Add(RegHeight);
                        value.Add(cameraNr);
                        value.Add(serverName);
                        value.Add(Uname);
                        value.Add(Password);

                        DateTime dtt = utcDateTime;

                        var utcSeconds = dtt.TimeOfDay.TotalSeconds - Convert.ToDouble(starttime);
                        TimeSpan uTC_time = TimeSpan.FromSeconds(utcSeconds);
                        dtt = dtt.Date + uTC_time;
                        var HH = dtt.Hour < 10 ? "0" + dtt.Hour : dtt.Hour.ToString();
                        var mm = dtt.Minute < 10 ? "0" + dtt.Minute : dtt.Minute.ToString();
                        var ss = dtt.Second < 10 ? "0" + dtt.Second : dtt.Second.ToString();
                        _timeString = HH + mm + ss;
                        //value.Add(_timeString);

                        var yy = dtt.Year.ToString();
                        var mn = dtt.Month < 10 ? "0" + dtt.Month : dtt.Month.ToString();
                        var dd = dtt.Day < 10 ? "0" + dtt.Day : dtt.Day.ToString();
                        finaldate = yy + mn + dd;

                    }

                    string ipaddress = "";
                    string timetoplay = "";
                    string duration = "";
                    string port = "";
                    string regwidth = "";
                    string regheight = "";
                    string camnrId = "";
                    string servernam = "";
                    string us1 = "";
                    string pwd1 = "";
                    int ii = 0;
                    int j = 0;
                    int M = 0;
                    int n = 0;
                    int p = 0;
                    int rh = 0;
                    int rw = 0;
                    int cm = 0;
                    for (ii = 0; ii <= value.Count - 1; ii += 11)
                    {
                        camera = value[ii].ToString();
                        port = value[ii + 4].ToString();
                        if (port == "")
                        {
                            port = "8000";
                        }

                        duration = value[ii + 2].ToString();
                        timetoplay = value[ii + 3].ToString();

                        regwidth = value[ii + 5];
                        if (regwidth == "")
                        {
                            regwidth = "480";
                        }

                        regheight = value[ii + 6];
                        if (regheight == "")
                        {
                            regheight = "640";
                        }
                        camnrId = value[ii + 7].ToString();
                        ipaddress = value[ii + 1].ToString();
                        servernam = value[ii + 8].ToString();
                        us1 = value[ii + 9].ToString();
                        pwd1 = value[ii + 10].ToString();
                        double timevalue = Convert.ToDouble(timetoplay);
                        DateTime date1 = Convert.ToDateTime(binddatetime);
                        DateTime date2 = date1.AddMinutes(-timevalue);
                        string dt6 = date2.ToString("HH:mm:ss");
                        string timeto = date2.ToString();

                        string[] Separate = timeto.Split(' ');
                        string[] Separate1 = dt6.Split(':');
                        string desiredTime = Separate[1].ToString();
                        string desiredTime1 = Separate1[0].ToString();


                        string[] Separate2 = desiredTime.Split(':');
                        string[] Separate3 = desiredTime1.Split(':');

                        int Hour = Convert.ToInt32(Separate2[0]);
                        int Hour1 = Convert.ToInt32(Separate3[0]);
                        string correcthour = "";
                        if (Hour1 < 10)
                        {
                            correcthour = "0" + Hour1;
                        }
                        else
                        {
                            correcthour = Hour1.ToString();
                        }
                        string Min = Separate2[1];
                        string sec = Separate2[2];
                       
                        var playbefore = int.Parse(sec) - int.Parse(duration);

                        if (playbefore < 0)
                        {
                            var newmintue = int.Parse(Min) - 1;
                            var nmin = int.Parse(Min) - 1;
                            Min = nmin.ToString();
                            var newsec = int.Parse(sec) + 60;
                            playbefore = newsec - int.Parse(duration);
                        }
                        string quaterurl = "http://" + us1 + ":" + pwd1 + "@" + ipaddress + ":" + port + "/archive/media/" + servernam + "/DeviceIpint.";
                        int second = int.Parse(timetoplay) * 1000;
                        string halfurl = quaterurl + camnrId + "/SourceEndpoint.video:0:0/";
                        string reghw = "width=" + regheight + ",height=" + regwidth + "@@";
                        string mainurl = reghw + halfurl + finaldate + "T" + _timeString + "." + "000" + "?format=mjpeg&speed=1***" + second + "";
                        list.Add(mainurl);
                    }
                    int c1 = 0;
                    foreach (string i in list)
                    {

                        string url = i.ToString();
                        Html.DevExpress().HyperLink(lnkSettings1 =>
                        {
                            c1++;
                            HyperLinkProperties properties1 = lnkSettings1.Properties as HyperLinkProperties;
                            //properties1.Target = "_blank";
                            lnkSettings1.Properties.Text = "C" + c1 + "|";
                            lnkSettings1.NavigateUrl = "javascript:ViewLogDetail1('" + url + "')";
                        }).Render();
                        c1 = 0 + c1;
                    }
                }
            }).Render();
        });

        settings.CustomUnboundColumnData = (sender, e) =>
        {
            if (e.Column.FieldName == "Total")
            {
                double time = Convert.ToDouble(e.GetListSourceFieldValue("Hours"));
                if (time <= 0) { e.Value = "00:00"; }
                else
                {
                    e.Value = DateTime.MinValue.Add(TimeSpan.FromSeconds(time));
                }
            }
        };

        //Summary
        settings.Settings.ShowFooter = true;
        settings.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "Hours");
        settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Sum, "Hours");
        settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Custom, "Total");

        settings.SummaryDisplayText = (sender, e) =>
        {
            if (e.IsGroupSummary)
            {
                if (e.Item.FieldName == "Hours")
                {
                    double time = Double.Parse(e.Value.ToString());
                    string hours = Math.Floor((TimeSpan.FromSeconds(time)).TotalHours) > 9 ? Math.Floor((TimeSpan.FromSeconds(time)).TotalHours).ToString() : "0" + Math.Floor((TimeSpan.FromSeconds(time)).TotalHours).ToString();
                    string minutes = (TimeSpan.FromSeconds(time)).Minutes > 9 ? (TimeSpan.FromSeconds(time)).Minutes.ToString() : "0" + (TimeSpan.FromSeconds(time)).Minutes;
                    e.Text = "Total hours = " + hours + ":" + minutes;
                }
            }
        };
        settings.CustomSummaryCalculate = (sender, e) =>
        {
            double time;
            if (e.IsGroupSummary)
            {
            }
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
            {
                ASPxSummaryItem incomeSummary = (sender as ASPxGridView).TotalSummary["Hours"];

                Decimal income = Convert.ToDecimal(((ASPxGridView)sender).GetTotalSummaryValue(incomeSummary));
                time = Convert.ToDouble(income);

                string hours = Math.Floor((TimeSpan.FromSeconds(time)).TotalHours) > 9 ? Math.Floor((TimeSpan.FromSeconds(time)).TotalHours).ToString() : "0" + Math.Floor((TimeSpan.FromSeconds(time)).TotalHours).ToString();
                string minutes = (TimeSpan.FromSeconds(time)).Minutes > 9 ? (TimeSpan.FromSeconds(time)).Minutes.ToString() : "0" + (TimeSpan.FromSeconds(time)).Minutes;
                e.TotalValue = "Total hours = " + hours + ":" + minutes;
            }
        };
        settings.HtmlRowPrepared = (sender, e) =>
        {
            if (e.RowType != GridViewRowType.Data) return;
            int status = Convert.ToInt32(e.GetValue("Status"));
            {
                if (status == 2)
                {
                    e.Row.BackColor = System.Drawing.Color.LightBlue;// LightGreen;
                }
            }
        };

    }).Bind(Model).Render();
%>
<script>
    function ViewLogDetail1(cntr) {
        var popups = new Array();
        var regheigwidth = cntr.split('@@')[0];
        var cntr2 = cntr.split('@@')[1];
        var cntrr = cntr2.split('***')[0];
        var seconds = cntr2.split('***')[1];
        var win = window.open(cntrr, " ", "'directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no, " + regheigwidth + ",left=300,top=100");
        popups.push(win);
        for (k = 0; k < popups.length; k++) {
            setTimeout(function () { popups[0].close(); }, seconds);

            break;
        }
    }
</script>
