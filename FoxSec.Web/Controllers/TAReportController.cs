using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using FoxSec.Common.Enums;
using FoxSec.Authentication;
using System.Globalization;
using FoxSec.Infrastructure.EF.Repositories;
using FoxSec.Infrastructure.EntLib.Logging;
using FoxSec.ServiceLayer.Contracts;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Web.Helpers;
using FoxSec.Web.ViewModels;
using System.Web.Mvc;
using System.Data.Entity.Core.Objects;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.SessionState;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrintingLinks;
using DevExpress.PivotGrid;
using DevExpress.XtraPivotGrid;
using FoxSec.Core.Infrastructure.UnitOfWork;
using System.Data.Entity;
using System.Collections;
using DevExpress.Spreadsheet;
using ClosedXML.Excel;
using System.Data;
using System.IO;
using DevExpress.Printing.ExportHelpers;
using DevExpress.Export;
using DevExpress.Web.Internal;
using DevExpress.Data;
using System.Dynamic;
using DocumentFormat.OpenXml.Wordprocessing;
using DevExpress.Web.DemoUtils;
using DevExpress.Utils;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Reflection;
using DevExpress.Data.PivotGrid;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;

namespace FoxSec.Web.Controllers
{
    public class TAReportController : BusinessCaseController
    {
        FoxSecDBContext db = new FoxSecDBContext();

        string depname;
        string cname;
        string countryflag = "0";
        #region Install
        private readonly ILogRepository _logRepository;
        private readonly ITAReportRepository _reportRepository;
        private readonly ITAReportService _reportService;
        private readonly IBuildingObjectService _buildingObjectService;
        private readonly ICurrentUser _currentUser;
        private readonly ILogger _logger;
        private readonly IBuildingObjectRepository _BuildingObjectRepository;
        private readonly ITAMoveRepository _TAMoveRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserDepartmentRepository _UserDepartmentRepository;
        private readonly ITAMoveService _TAMoveService;
        private readonly ITAReportService _TAReportService;
        private readonly IUserBuildingRepository _userBuildingRepository;
        private readonly ITAReportRepository _taReportRepository;
        private readonly IClassificatorRepository _classificatorRepository;
        private readonly IClassificatorValueRepository _classificatorValueRepository;
        private readonly ILogService _logService;
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString);

        public TAReportController(ITAReportService reportService,
                                  IUserDepartmentRepository UserDepartmentRepository,
                                  IUserRepository userRepository,
                                  ITAMoveService TAMoveService,
                                  ITAReportService TAReportService,
                                  ILogRepository logRepository,
                                  IBuildingObjectService buildingObjectService,
                                  ITAReportRepository reportRepository,
                                  ITAMoveRepository TAMoveRepository,
                                  IBuildingObjectRepository buildingObjectepository,
                                  IUserBuildingRepository userBuildingRepository,
                                  ICurrentUser currentUser,
                                  ILogger logger,
                                  ITAReportRepository tAReportRepository,
                                  IClassificatorRepository classificatorRepository,
                                  IClassificatorValueRepository classificatorValueRepository,
                                  ILogService logService)
            : base(currentUser, logger)
        {
            _logRepository = logRepository;
            _UserDepartmentRepository = UserDepartmentRepository;
            _userRepository = userRepository;
            _TAMoveService = TAMoveService;
            _TAReportService = TAReportService;
            _TAMoveRepository = TAMoveRepository;
            _reportRepository = reportRepository;
            _buildingObjectService = buildingObjectService;
            _reportService = reportService;
            _currentUser = currentUser;
            _logger = logger;
            _BuildingObjectRepository = buildingObjectepository;
            _userBuildingRepository = userBuildingRepository;
            _taReportRepository = tAReportRepository;
            _classificatorRepository = classificatorRepository;
            _classificatorValueRepository = classificatorValueRepository;
            _logService = logService;
        }

        public ActionResult TabContent()
        {
            var hmv = CreateViewModel<HomeViewModel>();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString;
                SqlConnection myConnection = new SqlConnection(connectionString);
                myConnection.Open();
                SqlCommand cmd = new SqlCommand("select top 1 Value from classificatorvalues where ClassificatorId=(select id from Classificators where Description like '%T&A report main company name%')", myConnection);
                ViewBag.TACompnay = Convert.ToString(cmd.ExecuteScalar());
                SqlCommand sqlCommand = new SqlCommand("select * from Classificators where Description = 'foxsec edlus'", myConnection);
                var isIntegrationCertificateExists = sqlCommand.ExecuteScalar();
                if(isIntegrationCertificateExists != null)
                {
                    ViewBag.EdlusIntegrationExists = true;
                }
                else
                {
                    ViewBag.EdlusIntegrationExists = false;
                }
                myConnection.Close();
            }
            catch
            {
            }
            return PartialView(hmv);
        }
        //public JsonResult SaveBuildingnames()
        #endregion

        [ValidateInput(false)]
        public void ExportToParamsCustomUser(List<int> a)
        {
            Session["Users"] = a;
            //return View();
        }

        void options_CustomizeSheetHeader(DevExpress.Export.ContextEventArgs e)
        {
            try
            {
                // Create a new row.
                int Usrid = Convert.ToInt32(Session["User_Id"]);
                int usrrole = Convert.ToInt32(Session["Role_ID"]);
                string ufname = "";
                string Ulname = "";
                string Userdesignation1 = "";
                int Bul_id = Convert.ToInt32(Session["Buidlidngid"]);
                int companyId1 = Convert.ToInt32(Session["company"]);
                try
                {
                    string getvalidcompid = @" select  FirstName,LastName from Users    where Id={0}";
                    var userLogDeatil = db.Database.SqlQuery<specificUser>(getvalidcompid, Usrid).FirstOrDefault();

                    if (userLogDeatil != null)
                    {
                        ufname = userLogDeatil.FirstName;
                        Ulname = userLogDeatil.LastName;
                    }
                    var userDesignation = db.UserRoles.Where(x => x.Id == usrrole).SingleOrDefault();
                    if (userDesignation != null)
                    {
                        Userdesignation1 = userDesignation.Name;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                string ReportPrepareby = ufname + " " + Ulname + "  " + "(" + Userdesignation1 + ")";

                if (Bul_id == 0 && companyId1 == 0)
                {
                    CellObject row1 = new CellObject();
                    if (countryflag == "EN")
                    {
                        row1.Value = "Report Created:" + ReportPrepareby + " " + "->" + " " + System.DateTime.Now;
                    }
                    else
                    {
                        row1.Value = ViewResources.SharedStrings.ReportCreated + ReportPrepareby + " " + "->" + " " + System.DateTime.Now;
                    }
                    try
                    {
                        XlFormattingObject row8Formatting = new XlFormattingObject();
                        row1.Formatting = row8Formatting;
                        row8Formatting.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Right, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
                        // row7Formatting.Font = new XlCellFont { Color = System.Drawing.Color.Red, Size = 12 };

                        row8Formatting.BackColor = System.Drawing.Color.Lime;
                        row8Formatting.Font.Bold = true;
                    }
                    catch
                    {
                    }
                    e.ExportContext.AddRow(new[] { row1 });
                    e.ExportContext.AddRow();
                    e.ExportContext.MergeCells(new DevExpress.Export.Xl.XlCellRange(new DevExpress.Export.Xl.XlCellPosition(0, 0), new DevExpress.Export.Xl.XlCellPosition(5, 0)));
                }

                DateTime From = DateTime.ParseExact(Session["TAStartDate"].ToString(), "dd.MM.yyyy",
                                           System.Globalization.CultureInfo.InvariantCulture);
                DateTime To = DateTime.ParseExact(Session["TAStoptDate"].ToString(), "dd.MM.yyyy",
                                          System.Globalization.CultureInfo.InvariantCulture);

                int companyId = Convert.ToInt32(Session["company"]);
                var CompanyRecords = db.Companies.Where(x => x.Id == companyId).FirstOrDefault();
                string companyName = "", RegistraionNum = "";
                if (CompanyRecords != null)
                {
                    companyName = CompanyRecords.Name;
                    RegistraionNum = CompanyRecords.Comment;
                    if (RegistraionNum == null)
                    {
                        RegistraionNum = "NA";
                    }
                }
                string bname = Session["BuildingName"].ToString();
                var ee = (from Ta in db.TABuildingName
                          join B in db.Buildings on Ta.BuildingId equals B.Id
                          where Ta.Name == bname && Ta.ValidFrom <= From && Ta.ValidTo >= To
                          select new
                          {
                              taname = Ta.Name,
                              Ta.ValidFrom,
                              Ta.ValidTo,
                              Ta.BuildingLicense,
                              Ta.CadastralNr,
                              Ta.Address,
                              B.Name,
                          }).FirstOrDefault();

                if (Bul_id > 0 && companyId1 == 0)
                {
                    CellObject row0 = new CellObject();
                    if (countryflag == "EN")
                    {
                        row0.Value = "Report Created:" + ReportPrepareby + " " + "->" + " " + System.DateTime.Now;
                    }
                    else
                    {
                        row0.Value = ViewResources.SharedStrings.ReportCreated + ReportPrepareby + " " + "->" + " " + System.DateTime.Now;
                    }
                    // Specify row formatting.
                    try
                    {
                        XlFormattingObject rowFormatting0 = new XlFormattingObject();
                        row0.Formatting = rowFormatting0;
                        rowFormatting0.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Right, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
                        rowFormatting0.Font = new XlCellFont { Color = System.Drawing.Color.Black, Size = 12 };
                        rowFormatting0.Font.Bold = true;
                    }
                    catch
                    {
                    }
                    row0.Value = ViewResources.SharedStrings.ReportCreated + ReportPrepareby + " " + "->" + " " + System.DateTime.Now;
                    //row0.Value=row0.Value + ReportPrepareby + " " + "->" + " " + System.DateTime.Now;
                    CellObject row = new CellObject();
                    if (countryflag == "EN")
                    {
                        if (ee.taname == null)
                        {
                            row.Value = "Building Name:" + "";
                        }
                        else
                        {
                            row.Value = "Building Name:" + ee.taname;
                        }
                    }
                    else
                    {
                        row.Value = ViewResources.SharedStrings.BuildingsName + ":" + ee.taname;
                    }
                    // Specify row formatting.
                    try
                    {
                        XlFormattingObject rowFormatting = new XlFormattingObject();
                        row.Formatting = rowFormatting;
                        rowFormatting.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Center, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
                        rowFormatting.Font = new XlCellFont { Color = System.Drawing.Color.Black, Size = 12 };
                        rowFormatting.Font.Bold = true;
                    }
                    catch
                    { }

                    // Add the created row to the output document.

                    CellObject row2 = new CellObject();
                    // Specify row values.
                    if (countryflag == "EN")
                    {
                        row2.Value = "Address:" + ee.Address;
                    }
                    else
                    {
                        row2.Value = ViewResources.SharedStrings.BuildingsAddress + ":" + ee.Address;
                    }
                    try
                    {
                        XlFormattingObject row2Formatting = new XlFormattingObject();
                        row2.Formatting = row2Formatting;
                        row2Formatting.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Center, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
                        row2Formatting.Font = new XlCellFont { Color = System.Drawing.Color.Black, Size = 12 };
                        row2Formatting.Font.Bold = true;
                    }
                    catch
                    {
                    }

                    CellObject row3 = new CellObject();
                    if (countryflag == "EN")
                    {
                        row3.Value = "Building License:" + ee.BuildingLicense;
                    }
                    else
                    {
                        row3.Value = ViewResources.SharedStrings.TaBuildingLicense + ":" + ee.BuildingLicense;
                    }
                    try
                    {
                        XlFormattingObject row3Formatting = new XlFormattingObject();
                        row3.Formatting = row3Formatting;
                        row3Formatting.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Center, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
                        row3Formatting.Font = new XlCellFont { Color = System.Drawing.Color.Black, Size = 12 };
                        row3Formatting.Font.Bold = true;
                    }
                    catch
                    {
                    }

                    CellObject row4 = new CellObject();
                    if (countryflag == "EN")
                    {
                        row4.Value = "Cadastre number:" + ee.CadastralNr;
                    }
                    else
                    {
                        row4.Value = ViewResources.SharedStrings.TaBuildingCadsnrNo + ":" + ee.CadastralNr;
                    }
                    try
                    {
                        XlFormattingObject row4Formatting = new XlFormattingObject();
                        row4.Formatting = row4Formatting;
                        row4Formatting.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Center, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
                        row4Formatting.Font = new XlCellFont { Color = System.Drawing.Color.Black, Size = 12 };
                        row4Formatting.Font.Bold = true;
                    }
                    catch
                    {
                    }

                    CellObject row5 = new CellObject();
                    if (countryflag == "EN")
                    {
                        row5.Value = "Period:" + From.ToString("dd MMM yyyy") + "--" + To.ToString("dd MMM yyyy");
                    }
                    else
                    {
                        row5.Value = ViewResources.SharedStrings.Period + From.ToString("dd MMM yyyy") + "--" + To.ToString("dd MMM yyyy");
                    }
                    try
                    {
                        XlFormattingObject row5Formatting = new XlFormattingObject();
                        row5.Formatting = row5Formatting;
                        row5Formatting.Font = new XlCellFont { Color = System.Drawing.Color.Black, Size = 12 };
                        row5Formatting.Font.Bold = true;
                        row5Formatting.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Right, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
                    }
                    catch
                    {
                    }

                    e.ExportContext.AddRow(new[] { row0 });
                    e.ExportContext.AddRow(new[] { row });
                    e.ExportContext.AddRow(new[] { row2 });
                    e.ExportContext.AddRow(new[] { row3 });
                    e.ExportContext.AddRow(new[] { row4 });
                    e.ExportContext.AddRow(new[] { row5 });
                    // Add an empty row to the output document. Periods:
                    e.ExportContext.AddRow();
                    // Merge cells of two new rows.
                    e.ExportContext.MergeCells(new DevExpress.Export.Xl.XlCellRange(new DevExpress.Export.Xl.XlCellPosition(0, 0), new DevExpress.Export.Xl.XlCellPosition(5, 0)));
                    e.ExportContext.MergeCells(new DevExpress.Export.Xl.XlCellRange(new DevExpress.Export.Xl.XlCellPosition(0, 1), new DevExpress.Export.Xl.XlCellPosition(5, 1)));
                    e.ExportContext.MergeCells(new DevExpress.Export.Xl.XlCellRange(new DevExpress.Export.Xl.XlCellPosition(0, 2), new DevExpress.Export.Xl.XlCellPosition(5, 2)));
                    e.ExportContext.MergeCells(new DevExpress.Export.Xl.XlCellRange(new DevExpress.Export.Xl.XlCellPosition(0, 3), new DevExpress.Export.Xl.XlCellPosition(5, 3)));
                    e.ExportContext.MergeCells(new DevExpress.Export.Xl.XlCellRange(new DevExpress.Export.Xl.XlCellPosition(0, 4), new DevExpress.Export.Xl.XlCellPosition(5, 4)));
                    e.ExportContext.MergeCells(new DevExpress.Export.Xl.XlCellRange(new DevExpress.Export.Xl.XlCellPosition(0, 5), new DevExpress.Export.Xl.XlCellPosition(5, 5)));
                }
                else if (companyId1 > 0 && Bul_id == 0)
                {
                    CellObject row0 = new CellObject();
                    if (countryflag == "EN")
                    {
                        row0.Value = "Report Created:" + ReportPrepareby + " " + "->" + " " + System.DateTime.Now;
                    }
                    else
                    {
                        row0.Value = ViewResources.SharedStrings.ReportCreated + ReportPrepareby + " " + "->" + " " + System.DateTime.Now;
                    }
                    try
                    {
                        // Specify row formatting.
                        XlFormattingObject rowFormatting0 = new XlFormattingObject();
                        row0.Formatting = rowFormatting0;
                        //rowFormatting0.BackColor = System.Drawing.Color.LightGray;
                        rowFormatting0.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Right, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
                        rowFormatting0.Font = new XlCellFont { Color = System.Drawing.Color.Black, Size = 12 };
                        rowFormatting0.Font.Bold = true;
                    }
                    catch
                    {
                    }
                    //rowFormatting0.BackColor = System.Drawing.Color.Lime;
                    CellObject row01 = new CellObject();
                    if (countryflag == "EN")
                    {
                        row01.Value = "Company Details :" + companyName + "  " + "," + RegistraionNum;
                    }
                    else
                    {
                        row01.Value = ViewResources.SharedStrings.CompanyDetails + ":" + companyName + "  " + "," + RegistraionNum;
                    }
                    // Specify row formatting.
                    try
                    {
                        XlFormattingObject row9Formatting = new XlFormattingObject();
                        row01.Formatting = row9Formatting;
                        row9Formatting.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Center, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
                        // row7Formatting.Font = new XlCellFont { Color = System.Drawing.Color.Red, Size = 12 };
                        row9Formatting.BackColor = System.Drawing.Color.Lime;
                    }
                    catch
                    {
                    }
                    e.ExportContext.AddRow(new[] { row0 });
                    e.ExportContext.AddRow(new[] { row01 });
                    e.ExportContext.AddRow();
                    // Merge cells of two new rows.

                    e.ExportContext.MergeCells(new DevExpress.Export.Xl.XlCellRange(new DevExpress.Export.Xl.XlCellPosition(0, 0), new DevExpress.Export.Xl.XlCellPosition(5, 0)));
                    e.ExportContext.MergeCells(new DevExpress.Export.Xl.XlCellRange(new DevExpress.Export.Xl.XlCellPosition(0, 1), new DevExpress.Export.Xl.XlCellPosition(5, 1)));
                }
                else if (Bul_id > 0 && companyId1 > 0)
                {
                    CellObject row0 = new CellObject();
                    if (countryflag == "EN")
                    {
                        row0.Value = "Report Created:" + ReportPrepareby + " " + "->" + " " + System.DateTime.Now;
                    }
                    else
                    {
                        row0.Value = ViewResources.SharedStrings.ReportCreated + ReportPrepareby + " " + "->" + " " + System.DateTime.Now;
                    }
                    // Specify row formatting.
                    try
                    {
                        XlFormattingObject rowFormatting0 = new XlFormattingObject();
                        row0.Formatting = rowFormatting0;
                        rowFormatting0.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Distributed, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
                        rowFormatting0.Font = new XlCellFont { Color = System.Drawing.Color.Black, Size = 12 };
                        rowFormatting0.Font.Bold = true;
                    }
                    catch
                    {
                    }
                    //rowFormatting0.BackColor = System.Drawing.Color.Lime;
                    CellObject row01 = new CellObject();
                    if (countryflag == "EN")
                    {
                        row01.Value = "Company Details :" + companyName + "  " + "," + RegistraionNum;
                    }
                    else
                    {
                        row01.Value = ViewResources.SharedStrings.CompanyDetails + ": " + companyName + "  " + "," + RegistraionNum;
                    }
                    try
                    {
                        // Specify row formatting.
                        XlFormattingObject row9Formatting = new XlFormattingObject();
                        row01.Formatting = row9Formatting;
                        row9Formatting.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Center, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
                        // row7Formatting.Font = new XlCellFont { Color = System.Drawing.Color.Red, Size = 12 };
                        row9Formatting.BackColor = System.Drawing.Color.Lime;
                    }
                    catch
                    {
                    }

                    CellObject row = new CellObject();
                    if (countryflag == "EN")
                    {
                        row.Value = "Building Name:" + ee.taname;
                    }
                    else
                    {
                        row.Value = ViewResources.SharedStrings.BuildingsName + ":" + ee.taname;
                    }
                    try
                    {
                        // Specify row formatting.
                        XlFormattingObject rowFormatting = new XlFormattingObject();
                        row.Formatting = rowFormatting;
                        rowFormatting.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Center, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
                        rowFormatting.Font = new XlCellFont { Color = System.Drawing.Color.Black, Size = 12 };
                    }
                    catch
                    {
                    }
                    // Add the created row to the output document.

                    CellObject row2 = new CellObject();
                    // Specify row values.
                    if (countryflag == "EN")
                    {
                        row2.Value = "Address:" + ee.Address;
                    }
                    else
                    {
                        row2.Value = ViewResources.SharedStrings.BuildingsAddress + ":" + ee.Address;
                    }
                    try
                    {
                        XlFormattingObject row2Formatting = new XlFormattingObject();
                        row2.Formatting = row2Formatting;
                        row2Formatting.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Center, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
                        row2Formatting.Font = new XlCellFont { Color = System.Drawing.Color.Black, Size = 12 };
                        row2Formatting.Font.Bold = true;
                    }
                    catch
                    {
                    }

                    CellObject row3 = new CellObject();
                    if (countryflag == "EN")
                    {
                        row3.Value = "Building License:" + ee.BuildingLicense;
                    }
                    else
                    {
                        row3.Value = ViewResources.SharedStrings.TaBuildingLicense + ":" + ee.BuildingLicense;
                    }
                    try
                    {
                        XlFormattingObject row3Formatting = new XlFormattingObject();
                        row3.Formatting = row3Formatting;
                        row3Formatting.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Center, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
                        row3Formatting.Font = new XlCellFont { Color = System.Drawing.Color.Red, Size = 12 };
                        row3Formatting.Font.Bold = true;
                    }
                    catch
                    {
                    }

                    CellObject row4 = new CellObject();
                    if (countryflag == "EN")
                    {
                        row4.Value = "Cadastre number:" + ee.CadastralNr;
                    }
                    else
                    {
                        row4.Value = ViewResources.SharedStrings.TaBuildingCadsnrNo + ": " + ee.CadastralNr;
                    }
                    try
                    {
                        XlFormattingObject row4Formatting = new XlFormattingObject();
                        row4.Formatting = row4Formatting;
                        row4Formatting.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Center, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
                        row4Formatting.Font = new XlCellFont { Color = System.Drawing.Color.Black, Size = 12 };
                        row4Formatting.Font.Bold = true;
                    }
                    catch
                    {
                    }
                    CellObject row5 = new CellObject();
                    if (countryflag == "EN")
                    {
                        row5.Value = "Period:" + ee.ValidFrom.ToString("dd MMM yyyy") + "--" + ee.ValidTo.ToString("dd MMM yyyy");
                    }
                    else
                    {
                        row5.Value = ViewResources.SharedStrings.Period + ee.ValidFrom.ToString("dd MMM yyyy") + "--" + ee.ValidTo.ToString("dd MMM yyyy");
                    }
                    try
                    {
                        XlFormattingObject row5Formatting = new XlFormattingObject();
                        row5.Formatting = row5Formatting;
                        row5Formatting.Font = new XlCellFont { Color = System.Drawing.Color.Black, Size = 12 };
                        row5Formatting.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Center, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
                        row5Formatting.Font.Bold = true;
                    }
                    catch
                    {
                    }
                    e.ExportContext.AddRow(new[] { row0 });
                    e.ExportContext.AddRow(new[] { row01 });
                    e.ExportContext.AddRow(new[] { row });
                    e.ExportContext.AddRow(new[] { row2 });
                    e.ExportContext.AddRow(new[] { row3 });
                    e.ExportContext.AddRow(new[] { row4 });
                    e.ExportContext.AddRow(new[] { row5 });
                    // Add an empty row to the output document. Periods:
                    e.ExportContext.AddRow();
                    // Merge cells of two new rows.
                    e.ExportContext.MergeCells(new DevExpress.Export.Xl.XlCellRange(new DevExpress.Export.Xl.XlCellPosition(0, 0), new DevExpress.Export.Xl.XlCellPosition(5, 0)));
                    e.ExportContext.MergeCells(new DevExpress.Export.Xl.XlCellRange(new DevExpress.Export.Xl.XlCellPosition(0, 1), new DevExpress.Export.Xl.XlCellPosition(5, 1)));
                    e.ExportContext.MergeCells(new DevExpress.Export.Xl.XlCellRange(new DevExpress.Export.Xl.XlCellPosition(0, 2), new DevExpress.Export.Xl.XlCellPosition(5, 2)));
                    e.ExportContext.MergeCells(new DevExpress.Export.Xl.XlCellRange(new DevExpress.Export.Xl.XlCellPosition(0, 3), new DevExpress.Export.Xl.XlCellPosition(5, 3)));
                    e.ExportContext.MergeCells(new DevExpress.Export.Xl.XlCellRange(new DevExpress.Export.Xl.XlCellPosition(0, 4), new DevExpress.Export.Xl.XlCellPosition(5, 4)));
                    e.ExportContext.MergeCells(new DevExpress.Export.Xl.XlCellRange(new DevExpress.Export.Xl.XlCellPosition(0, 5), new DevExpress.Export.Xl.XlCellPosition(5, 5)));
                    e.ExportContext.MergeCells(new DevExpress.Export.Xl.XlCellRange(new DevExpress.Export.Xl.XlCellPosition(0, 6), new DevExpress.Export.Xl.XlCellPosition(5, 6)));
                }
            }
            catch
            {

            }
        }

        [HttpPost, ValidateInput(false)]
        public string BatchEditingUpdateBuilding_Name(MVCxGridViewBatchUpdateValues<BuildingNameViewModel, object> updateValues)
        {
            var model = db.TABuildingName;
            foreach (var customer in updateValues.Insert)
            {
                if (updateValues.IsValid(customer))
                {
                    try
                    {
                        var e = new TABuildingNames();
                        e.BuildingId = customer.BuildingId;
                        e.Name = customer.Name;
                        e.ValidFrom = Convert.ToDateTime(customer.ValidFrom);
                        e.ValidTo = Convert.ToDateTime(customer.ValidTo);
                        e.Address = customer.Address;
                        e.BuildingLicense = customer.BuildingLicense;
                        e.CadastralNr = customer.CadastralNr;
                        e.IsDeleted = false;
                        e.Customer = customer.Customer;
                        e.Contractor = customer.Contractor;
                        e.Contract = customer.Contract;
                        e.Sum = customer.Sum;
                        e.Lat = customer.Lat;
                        e.Lng = customer.Lng;
                        db.TABuildingName.Add(e);
                        // model.Add(TaModel);
                        int k = db.SaveChanges();

                    }
                    catch (Exception e)
                    {
                        updateValues.SetErrorText(customer, e.Message);
                    }
                }
            }
            foreach (var customer in updateValues.Update)
            {
                if (updateValues.IsValid(customer))
                {
                    try
                    {
                        var modelItem = model.FirstOrDefault(it => it.Id == customer.Id);
                        if (modelItem != null)
                        {
                            modelItem.Address = customer.Address;
                            modelItem.Name = customer.Name;
                            modelItem.BuildingId = customer.BuildingId;
                            modelItem.ValidFrom = customer.ValidFrom;
                            modelItem.ValidTo = customer.ValidTo;
                            modelItem.BuildingLicense = customer.BuildingLicense;
                            modelItem.CadastralNr = customer.CadastralNr;
                            modelItem.Customer = customer.Customer;
                            modelItem.Contractor = customer.Contractor;
                            modelItem.Contract = customer.Contract;
                            modelItem.Sum = customer.Sum;
                            modelItem.Lat = customer.Lat;
                            modelItem.Lng = customer.Lng;
                            this.UpdateModel(modelItem);
                            int k = db.SaveChanges();
                        }
                    }
                    catch (Exception e)
                    {
                        updateValues.SetErrorText(customer, e.Message);
                    }
                }
            }
            foreach (var customer in updateValues.DeleteKeys)
            {
                try
                {
                    int id = Convert.ToInt32(customer);
                    var record = db.TABuildingName.Where(x => x.Id == id).FirstOrDefault();
                    record.IsDeleted = true;
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    updateValues.SetErrorText(customer, e.Message);
                }
            }
            return "";
        }

        #region T&A building

        [HttpGet]
        public ActionResult OpenAllBuildingsObjects(int id)
        {
            int companyid = 0;
            Session["TACompanyId"] = companyid;
            var buildings = CreateViewModel<TAGlobalBuildingObjectsViewModel>();
            buildings.BuildingObjects = GetBuildingObjects(null);
            return PartialView("TABuildingObjects", buildings);
        }
        public JsonResult GetBuildingsOnClick_FromDate(string datefrom, string dateto)
        {
            DateTime From = DateTime.ParseExact(datefrom, "dd.MM.yyyy",
                                                   System.Globalization.CultureInfo.InvariantCulture);

            DateTime To = DateTime.ParseExact(dateto, "dd.MM.yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
            StringBuilder result = new StringBuilder();
            //if (CurrentUser.Get().IsSuperAdmin == true)
            {
                result.Append("<option value=" + '"' + '"' + ">" + ViewResources.SharedStrings.DefaultDropDownValue + "</option>");
                //var buildings = db.Buildings.ToList();
            }
            //var buildings = db.Buildings.ToList();

            // var buildings = db.TABuildingName.Where(x => x.ValidFrom >= From && x.ValidTo <= To).ToList();
            // var buildings= (validfrom >= from && validfrom < to)  or(validto > from && validto < to)
            //var buildings = db.TABuildingName.Where(x => x.ValidFrom >= From && x.ValidFrom < To || x.ValidTo > From && x.ValidTo < To).ToList();

            //working here ----
            var buildings = db.TABuildingName.Where(x => x.ValidFrom <= From && x.ValidTo >= From && x.ValidTo <= To || x.ValidTo >= To && x.ValidFrom <= To && x.ValidFrom >= From || x.ValidFrom <= From && x.ValidTo >= To || x.ValidFrom >= From && x.ValidTo <= To).ToList();
            buildings = buildings.Where(x => x.IsDeleted == false).ToList();
            if (CurrentUser.Get().IsSuperAdmin == false)
            {
                string name = CurrentUser.Get().LoginName;
                int? usrid = _userRepository.FindByLoginName(name).Id;

                List<int> buildid = _userBuildingRepository.FindAll().Where(x => x.IsDeleted == false && x.UserId == usrid).Select(y => y.BuildingId).ToList();
                buildings = buildings.Where(x => x.IsDeleted == false && buildid.Contains(x.BuildingId)).ToList();
            }
            foreach (var cc in buildings)
            {
                result.Append("<option value=" + '"' + cc.BuildingId + '"' + ">" + cc.Name + "</option>");
            }

            return Json(result.ToString(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBuildings(string datefrom, string dateto)
        {
            DateTime From = DateTime.ParseExact(datefrom, "dd.MM.yyyy",
                                        System.Globalization.CultureInfo.InvariantCulture);

            DateTime To = DateTime.ParseExact(dateto, "dd.MM.yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
            StringBuilder result = new StringBuilder();
            //if (CurrentUser.Get().IsSuperAdmin == true)
            {
                result.Append("<option value=" + '"' + '"' + ">" + ViewResources.SharedStrings.DefaultDropDownValue + "</option>");
                //var buildings = db.Buildings.ToList();
            }

            //var buildings = db.TABuildingName.Where(x => x.ValidFrom >= From && x.ValidTo <= To).ToList();
            //var buildings = db.TABuildingName.Where(x => x.ValidFrom >= From && x.ValidFrom < To || x.ValidTo > From && x.ValidTo < To).ToList();
            var buildings = db.TABuildingName.Where(x => x.ValidFrom <= From && x.ValidTo >= From && x.ValidTo <= To || x.ValidTo >= To && x.ValidFrom <= To && x.ValidFrom >= From || x.ValidFrom <= From && x.ValidTo >= To || x.ValidFrom >= From && x.ValidTo <= To).ToList();

            buildings = buildings.Where(x => x.IsDeleted == false).ToList();
            
            if (CurrentUser.Get().IsSuperAdmin == false)
            {
                string name = CurrentUser.Get().LoginName;
                int? usrid = _userRepository.FindByLoginName(name).Id;
                var userRole = CurrentUser.Get().RoleTypeId;
                //if(userRole == Convert.ToInt32(FixedRoleType.CompanyManager))
                //{
                //    try
                //    {
                //        var userId = CurrentUser.Get().Id;
                //        var companyId = db.User.Where(x => x.Id == userId).Select(y => y.CompanyId).FirstOrDefault();
                //        var companyComment = db.Companies.Where(x => companyId != null && x.Id == companyId).Select(y => y.Comment).FirstOrDefault();
                //        var dbAllBuildings = db.TABuildingName.ToList();
                //        List<int> buildingsId = new List<int>();
                //        companyComment = Regex.Match(companyComment, "[0-9]{11}").Value.Trim();
                //        foreach (var item in dbAllBuildings)
                //        {
                            
                //            if(Regex.Match(item.Contractor,"[0-9]{11}").Value.Trim() == companyComment)
                //            {
                //                buildingsId.Add(item.BuildingId);
                //            }

                //        }
                //        buildings = buildings.Where(x => x.IsDeleted == false && buildingsId.Contains(x.BuildingId)).ToList();
                        
                //    }
                //    catch
                //    {
                        
                //    }
                //}
                //else
                //{ 
                List<int> buildid = _userBuildingRepository.FindAll().Where(x => x.IsDeleted == false && x.UserId == usrid).Select(y => y.BuildingId).ToList();
                buildings = buildings.Where(x => x.IsDeleted == false && buildid.Contains(x.BuildingId)).ToList();
                //}
            }
            foreach (var cc in buildings)
            {
               
                result.Append("<option value=" + '"' + cc.BuildingId + '"' + ">" + cc.Name + "</option>");
            }

            return Json(result.ToString(), JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]
        public ActionResult BuildingsObjectsGridViewPartialA()
        {
            int? CompanyId = (int?)Session["TACompanyId"];
            //return PartialView("TABuildingObjects", GetBuildingObjects(CompanyId));
            return PartialView("TABoGridViewPartialView", GetBuildingObjects(CompanyId));
        }
        [ValidateInput(false)]
        public ActionResult BuildingsObjectsGridViewPartialA1()
        {
            int? CompanyId = (int?)Session["TACompanyId"];
            //return PartialView("TABuildingObjects", GetBuildingObjects(CompanyId));
            return PartialView("TABuildingNameList", GetTANames(CompanyId));
        }
        [ValidateInput(false)]
        public ActionResult TABuildingListFilter()
        {
            int? CompanyId = (int?)Session["TACompanyId"];
            //return PartialView("TABuildingObjects", GetBuildingObjects(CompanyId));
            return PartialView("WebUserControl1", GetTANames(CompanyId));
        }
        [ValidateInput(false)]
        public ActionResult BuildingsObjectsBatchGridEditA([ModelBinder(typeof(DevExpressEditorsBinder))]MVCxGridViewBatchUpdateValues<BuildingObject, int> updateValues)
        //public ActionResult BuildingsObjectsBatchGridEditA([ModelBinder(typeof(DevExpressEditorsBinder))]MVCxGridViewBatchUpdateValues<TAReport, int> updateValues)
        {
            /*
            foreach (var product in updateValues.Insert)
            {
                if (updateValues.IsValid(product))
                    InsertProduct(product, updateValues);
            }*/
            foreach (var product in updateValues.Update)
            {

                //int Id = product.Id;
                int done = _buildingObjectService.EditBuilding(product.Id, product.GlobalBuilding, HostName);
                /*
                if (updateValues.IsValid(product))
                    UpdateProduct(product, updateValues);*/
            }/*
            foreach (var productID in updateValues.DeleteKeys)
            {
                DeleteProduct(productID, updateValues);
            }*/
            int? CompanyId = (int?)Session["TACompanyId"];


            return PartialView("TABoGridViewPartialView", GetBuildingObjects(CompanyId));
            //return PartialView("TABuildingObjects", GetBuildingObjects(CompanyId));
        }

        public JsonResult Checkdate()
        {
            DateTime today = DateTime.Today;
            DateTime now = DateTime.Now;
            DateTime lastDayLastMonth = new DateTime(now.Year, now.Month, 1);
            DateTime startOfMonth = new DateTime(today.Year, today.Month, 1).AddMonths(-1);
            lastDayLastMonth = lastDayLastMonth.AddDays(-1);
            string a = startOfMonth.ToShortDateString();
            string b = lastDayLastMonth.ToShortDateString();
            string sdate = Convert.ToDateTime(a).ToString("dd'.'MM'.'yyyy");
            string edate = Convert.ToDateTime(b).ToString("dd'.'MM'.'yyyy");
            var som = sdate;
            var eom = edate;
            var data = new { eom = edate, som = sdate };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult chkcurrentDate()
        {
            string from = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("dd.MM.yyyy");
            string to = DateTime.Now.ToString("dd.MM.yyyy");
            var som = from;
            var eom = to;
            var data = new { eom = to, som = from };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region PivotGridReport
        [HttpGet]
        public ActionResult OpenMounthReport(int format, int? department, int? company, int id, string FromDateTA, string ToDateTA, int? BuildingId)
        {

            DateTime From = DateTime.ParseExact(FromDateTA, "dd.MM.yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
            DateTime To = DateTime.ParseExact(ToDateTA, "dd.MM.yyyy",

                System.Globalization.CultureInfo.InvariantCulture);

            if (CurrentUser.Get().IsCompanyManager) { company = CurrentUser.Get().CompanyId; }
            Session["TAStartDate"] = FromDateTA;
            Session["TAStoptDate"] = ToDateTA;
            Session["TACompanyId"] = company;
            Session["TAdepartmentId"] = department;
            Session["Format"] = format;
            var tamvm = CreateViewModel<TAReportMounthViewModel>();
            if (CurrentUser.Get().IsSuperAdmin || CurrentUser.Get().IsCompanyManager || CurrentUser.Get().IsDepartmentManager)
            {
                Mapper.Map(GetTAreports(From, To, company, department,BuildingId), tamvm.TAReportMounthItems);
            }
            if (format == 1)
            {
                return PartialView("TAMounthReportGridViewPartialView", tamvm.TAReportMounthItems);
            }
            else { return PartialView("TAMounthReportGridViewPartialViewLast", tamvm.TAReportMounthItems); }
        }

        public ActionResult OpenMounthReportWrokingDays(int format, int? department, int? company, int id, string FromDateTA, string ToDateTA,int? BuildingId)
        {

            DateTime From = DateTime.ParseExact(FromDateTA, "dd.MM.yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
            DateTime To = DateTime.ParseExact(ToDateTA, "dd.MM.yyyy",
                System.Globalization.CultureInfo.InvariantCulture);

            if (CurrentUser.Get().IsCompanyManager) { company = CurrentUser.Get().CompanyId; }
            Session["TAStartDate"] = FromDateTA;
            Session["TAStoptDate"] = ToDateTA;
            Session["TACompanyId"] = company;
            Session["TAdepartmentId"] = department;
            Session["Format"] = format;
            var tamvm = CreateViewModel<TAReportMounthViewModel>();
            if (CurrentUser.Get().IsSuperAdmin || CurrentUser.Get().IsCompanyManager || CurrentUser.Get().IsDepartmentManager)
            {
                Mapper.Map(GetTAreports(From, To, company, department,BuildingId), tamvm.TAReportMounthItems);
            }
            if (format == 1)
            {
                return PartialView("TAMonthReportWorkingDaysGridViewPartialView", tamvm.TAReportMounthItems);
            }
            else { return PartialView("TAMounthReportWorkingDaysGridViewPartialViewLast", tamvm.TAReportMounthItems); }
        }

        public ActionResult OpenMounthReportUsers(int? department, int? company, int id, string FromDateTA, string ToDateTA,int? BuildingId)
        {
            if (CurrentUser.Get().IsCompanyManager) { company = CurrentUser.Get().CompanyId; }
            DateTime From = DateTime.ParseExact(FromDateTA, "dd.MM.yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);

            DateTime To = DateTime.ParseExact(ToDateTA, "dd.MM.yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
            Session["TAStartDate"] = FromDateTA;
            Session["TAStoptDate"] = ToDateTA;
            Session["TACompanyId"] = company;
            Session["TAdepartmentId"] = department;
            var tamvm = CreateViewModel<TAReportMounthViewModel>();
            if (CurrentUser.Get().IsSuperAdmin || CurrentUser.Get().IsCompanyManager || CurrentUser.Get().IsDepartmentManager)
            {
                Mapper.Map(GetTAreports(From, To, company, department,BuildingId), tamvm.TAReportMounthItems);
                //   tamvm.TAReportUserTotal = _reportRepository.FindAll();
                //var tam = tamvm.TAReportMounthItems.GroupBy(x => x.UserId).ToList();
            }
            return PartialView("MounthReport", tamvm);
        }

        public ActionResult MouthReportAddTAReport(int? rowIndex, int? columnIndex,int? BuildingId/*, bool? isResetGridViewPageIndex*/)
        {
            Session["TArowIndex"] = rowIndex;
            Session["TAcolumnIndex"] = columnIndex;
            string FromDateTA = (string)Session["TAStartDate"];
            string ToDateTA = (string)Session["TAStoptDate"];
            int? CompanyId = (int?)Session["TACompanyId"];
            int? DepartmentId = (int?)Session["TAdepartmentId"];
            int? format = (int?)Session["Format"];
            DateTime From = DateTime.ParseExact(FromDateTA, "dd.MM.yyyy",
               System.Globalization.CultureInfo.InvariantCulture);
            DateTime To = DateTime.ParseExact(ToDateTA, "dd.MM.yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);

            object dataObject = null;
            if (rowIndex != null && columnIndex != null)
                dataObject = PivotGridExtension.CreateDrillDownDataSource(
                    PivotGridFeaturesDemosHelper.DrillDownPivotGridSettings,
                    GetTAreports(From, To, CompanyId, DepartmentId,BuildingId),
                    columnIndex.Value, rowIndex.Value
                );
            return PartialView("DrillDownGridViewPartial", dataObject);
        }
        public ActionResult MouthReportEditTAReport(int? rowIndex, int? columnIndex, string rowValue,int? BuildingId/*, bool? isResetGridViewPageIndex*/)
        {
            Session["TArowIndex"] = rowIndex;
            Session["TAcolumnIndex"] = columnIndex;
            string FromDateTA = (string)Session["TAStartDate"];
            string ToDateTA = (string)Session["TAStoptDate"];
            int? CompanyId = (int?)Session["TACompanyId"];
            int? DepartmentId = (int?)Session["TAdepartmentId"];
            int? format = (int?)Session["Format"];
            DateTime From = DateTime.ParseExact(FromDateTA, "dd.MM.yyyy",
               System.Globalization.CultureInfo.InvariantCulture);
            DateTime To = DateTime.ParseExact(ToDateTA, "dd.MM.yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
            var names = rowValue.Split(' ');
            //int usr = _userRepository.FindAll(x => x.LastName == names[0] && x.FirstName == names[1] && !x.IsDeleted).First().Id;
            int usr = 0;
            con.Open();
            SqlCommand cmd = new SqlCommand("select id from users where ((LastName+' '+FirstName) = '" + rowValue + "') and IsDeleted=0", con);
            string str = Convert.ToString(cmd.ExecuteScalar());
            if (!string.IsNullOrEmpty(str))
            {
                usr = Convert.ToInt32(str);
            }
            con.Close();

            Session["UserId"] = usr;

            object dataObject = null;
            if (rowIndex != null && columnIndex != null)
            {
                dataObject = PivotGridExtension.CreateDrillDownDataSource(
                    PivotGridFeaturesDemosHelper.DrillDownPivotGridSettings,
                    GetTAreports(From, To, CompanyId, DepartmentId,BuildingId),
                    columnIndex.Value, rowIndex.Value
                );
            }
            else
            {
                dataObject = PivotGridExtension.CreateDrillDownDataSource(
                 PivotGridFeaturesDemosHelper.DrillDownPivotGridSettings,
                 GetTAreports(From, To, usr));
            }

            ViewBag.TotalRowsCount = dataObject;
            if (ViewBag.TotalRowsCount == null)
            {
                ViewBag.TotalRowsCount = "0";
            }
            else
            {
                ViewBag.TotalRowsCount = "1";
            }
            return PartialView("DrillDownGridViewPartial", dataObject);
        }
        [ValidateInput(false)]
        public ActionResult TAReportObjectsBatchGridEdit([ModelBinder(typeof(DevExpressEditorsBinder))]MVCxGridViewBatchUpdateValues<TAReportItem, int> updateValues)
        {
            string result1 = "Date already exist please edit record";
            int usr = (int)Session["UserId"];
            if (updateValues.Insert.Count() != 0)
            {
                User user = _userRepository.FindById(usr);
                try
                {
                    int? departmentId = user.UserDepartments.First().DepartmentId;
                    foreach (var product in updateValues.Insert)
                    {
                        string name = product.ReportDate.DayOfWeek.ToString().Substring(0, 3) + " " + product.ReportDate.Day.ToString();
                        TimeSpan interval = TimeSpan.Parse(product.Hours_Min);
                        string date = product.ReportDate.ToShortDateString().ToString();
                        float hours = (float)interval.TotalSeconds;
                        var result = _reportRepository.FindAll(x => x.ReportDate == Convert.ToDateTime(date) && x.UserId == usr && !x.IsDeleted).Any();
                        if (result == true)
                        {
                            return Json(result1);
                        }
                        else
                        {
                            _TAReportService.CreateTAReport(usr, departmentId, name, product.ReportDate.Date, Int16.Parse(product.ReportDate.Day.ToString()), hours, 1, 2, true, false);
                        }
                    }
                }
                catch (Exception ee)
                {

                    foreach (var product in updateValues.Insert)
                    {
                        string name = product.ReportDate.DayOfWeek.ToString().Substring(0, 3) + " " + product.ReportDate.Day.ToString();
                        TimeSpan interval = TimeSpan.Parse(product.Hours_Min);
                        string date = product.ReportDate.ToShortDateString().ToString();
                        float hours = (float)interval.TotalSeconds;
                        var result = _reportRepository.FindAll(x => x.ReportDate == Convert.ToDateTime(date) && x.UserId == usr && !x.IsDeleted).Any();
                        //var q = _reportRepository.FindAll();
                        if (result == true)
                        {
                            return Json(result1);
                        }
                        else
                        {
                            _TAReportService.CreateTAReport(usr, null, name, product.ReportDate.Date, Int16.Parse(product.ReportDate.Day.ToString()), hours, 1, 2, true, false);
                        }
                        if (usr < 0)
                        {
                            throw ee;
                        }
                    }
                }
            }
            foreach (var product in updateValues.Update)
            {
                TimeSpan interval = TimeSpan.Parse(product.Hours_Min);

                product.Hours = (float)interval.TotalSeconds;
                _TAReportService.EditTAReport(product.Id, product.Hours);
            }

            foreach (var productID in updateValues.DeleteKeys)
            {
                _TAReportService.DeleteTAReport(productID);
            }

            int? rowIndex = (int?)Session["TArowIndex"];
            int? columnIndex = (int?)Session["TAcolumnIndex"];
            string FromDateTA = (string)Session["TAStartDate"];
            string ToDateTA = (string)Session["TAStoptDate"];
            int? CompanyId = (int?)Session["TACompanyId"];
            int? DepartmentId = (int?)Session["TAdepartmentId"];
            int? format = (int?)Session["Format"];
            DateTime From = DateTime.ParseExact(FromDateTA, "dd.MM.yyyy",
               System.Globalization.CultureInfo.InvariantCulture);
            DateTime To = DateTime.ParseExact(ToDateTA, "dd.MM.yyyy",
                 System.Globalization.CultureInfo.InvariantCulture);

            object dataObject = null;
            if (rowIndex != null && columnIndex != null)
            {
                dataObject = PivotGridExtension.CreateDrillDownDataSource(
                    PivotGridFeaturesDemosHelper.DrillDownPivotGridSettings,
                    GetTAreports(From, To, CompanyId, DepartmentId,null),
                    columnIndex.Value, rowIndex.Value
                );
            }
            else
            {
                dataObject = PivotGridExtension.CreateDrillDownDataSource(
                 PivotGridFeaturesDemosHelper.DrillDownPivotGridSettings,
                 GetTAreports(From, To, usr));
            }
            ViewBag.TotalRowsCount = dataObject;
            if (ViewBag.TotalRowsCount == null)
            {
                ViewBag.TotalRowsCount = "0";
            }
            else
            {
                ViewBag.TotalRowsCount = "1";
            }

            return PartialView("DrillDownGridViewPartial", dataObject);
        }
        [ValidateInput(false)]
        public ActionResult TAMounthReportGridViewPartialA()
        {
            string FromDateTA = (string)Session["TAStartDate"];
            string ToDateTA = (string)Session["TAStoptDate"];
            int? CompanyId = (int?)Session["TACompanyId"];
            int? DepartmentId = (int?)Session["TAdepartmentId"];
            int? format = (int?)Session["Format"];
            DateTime From = DateTime.ParseExact(FromDateTA, "dd.MM.yyyy",
                           System.Globalization.CultureInfo.InvariantCulture);
            DateTime To = DateTime.ParseExact(ToDateTA, "dd.MM.yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
            var tamvm = CreateViewModel<TAReportMounthViewModel>();
            Mapper.Map(GetTAreports(From, To, CompanyId, DepartmentId,null), tamvm.TAReportMounthItems);
            if (format == 1)
            {
                return PartialView("TAMounthReportGridViewPartialView", tamvm.TAReportMounthItems);
            }
            else { return PartialView("TAMounthReportGridViewPartialViewLast", tamvm.TAReportMounthItems); }
        }
        #endregion
        #region MyReport
        [HttpGet]
        public ActionResult MyTAReport(int format, int? departmentId, int? company, int id, string FromDateTA, string ToDateTA)
        {
            id = 1119;// CurrentUser.Get().Id;
                      //int companyid = 0;
            DateTime From = DateTime.ParseExact(FromDateTA, "dd.MM.yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
            DateTime To = DateTime.ParseExact(ToDateTA, "dd.MM.yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);

            Session["TAStartDate"] = FromDateTA;
            Session["TAStoptDate"] = ToDateTA;
            Session["TACompanyId"] = company;
            Session["TAdepartmentId"] = departmentId;
            Session["Format"] = format;
            var tamvm = CreateViewModel<TAMsUserViewModel>();
            //var tamvm = CreateViewModel<TAMoveListViewModel>();
            // Mapper.Map(GetUserTAMoves(FromDateTA, ToDateTA, id), tamvm.TAMsUserItems);
            tamvm.TAMsUserItems = GetUserTAMoves(id, From, To); // _reportRepository.FindAll();
                                                                //var tam = tamvm.TAReportMounthItems.GroupBy(x => x.UserId).ToList();
                                                                //var tamvm = GetUserTAMoves(FromDateTA, ToDateTA, id);
            return PartialView("TAMsUserReport", tamvm.TAMsUserItems);
            //return PartialView("OpenMounthReportUser", tamvm);

        }

        [ValidateInput(false)]
        public ActionResult MyTAReportGridViewPartialA()
        {
            string StartDate = (string)Session["TAStartDate"];
            string StoptDate = (string)Session["TAStoptDate"];
            int? CompanyId = (int?)Session["TACompanyId"];
            int? format = (int?)Session["Format"];
            DateTime From = DateTime.ParseExact(StartDate, "dd.MM.yyyy",
                           System.Globalization.CultureInfo.InvariantCulture);
            DateTime To = DateTime.ParseExact(StoptDate, "dd.MM.yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);

            int id = 1119;// CurrentUser.Get().Id;
            var tamvm = CreateViewModel<TAMsUserViewModel>();
            tamvm.TAMsUserItems = GetUserTAMoves(id, From, To); // _reportRepository.FindAll();
            return PartialView("TAMsUserReport", tamvm.TAMsUserItems);
        }
        #endregion
        #region DetailUsersReport

        public ActionResult AddNew(string a)
        {
            string StartDate = (string)Session["TAStartDate"];
            string StoptDate = (string)Session["TAStoptDate"];
            int? company = (int?)Session["TACompanyId"];
            int? department = (int?)Session["TAdepartmentId"];

            DateTime From = DateTime.ParseExact(StartDate, "dd.MM.yyyy",
                                        System.Globalization.CultureInfo.InvariantCulture);

            DateTime To = DateTime.ParseExact(StoptDate, "dd.MM.yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);

            var tamvm = CreateViewModel<TAMsUserViewModel>();
            if (CurrentUser.Get().IsSuperAdmin || CurrentUser.Get().IsCompanyManager || CurrentUser.Get().IsDepartmentManager)
            {
                tamvm.TAMsUserItems = GetUsersTAMoves(From, To, company, department, 0); // _reportRepository.FindAll();
                //   tamvm.TAReportUserTotal = _reportRepository.FindAll();
                //var tam = tamvm.TAReportMounthItems.GroupBy(x => x.UserId).ToList();
            }
            return PartialView("TAReportDetailedGridViewPartialView", tamvm.TAMsUserItems);
        }

        [ValidateInput(false)]
        public ActionResult TAReportDetailedGrid()
        {
            string StartDate = (string)Session["TAStartDate"];
            string StoptDate = (string)Session["TAStoptDate"];
            int? company = (int?)Session["TACompanyId"];
            int? department = (int?)Session["TAdepartmentId"];

            DateTime From = DateTime.ParseExact(StartDate, "dd.MM.yyyy",
                                        System.Globalization.CultureInfo.InvariantCulture);

            DateTime To = DateTime.ParseExact(StoptDate, "dd.MM.yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);

            var tamvm = CreateViewModel<TAMsUserViewModel>();
            if (CurrentUser.Get().IsSuperAdmin || CurrentUser.Get().IsCompanyManager || CurrentUser.Get().IsDepartmentManager)
            {
                //tamvm.TAMsUserItems = GetUsersTAMovesNew(From, To, company, department, 0); // _reportRepository.FindAll();
                try
                {
                    tamvm.TAMsUserItems = (IEnumerable<TAMove>)Session["UserMovesDetails"];
                }
                catch
                {
                    tamvm.TAMsUserItems = GetUsersTAMovesNew(From, To, company, department, 0);
                }

                //   tamvm.TAReportUserTotal = _reportRepository.FindAll();
                //var tam = tamvm.TAReportMounthItems.GroupBy(x => x.UserId).ToList();
            }
            return PartialView("TAReportDetailedGridViewPartialView", tamvm.TAMsUserItems);
        }
        #endregion
        #region StartEnd
        [HttpGet]
        public ActionResult StartEndReport(int format, int? department, int? company, int id, string FromDateTA, string ToDateTA, int? BuildingId)
        {
            DateTime From = DateTime.ParseExact(FromDateTA, "dd.MM.yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
            DateTime To = DateTime.ParseExact(ToDateTA, "dd.MM.yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
            if (CurrentUser.Get().IsCompanyManager) { company = CurrentUser.Get().CompanyId; }
            Session["TAStartDate"] = FromDateTA;
            Session["TAStoptDate"] = ToDateTA;
            Session["TACompanyId"] = company;
            Session["TAdepartmentId"] = department;
            Session["Format"] = format;
            var tamvm = CreateViewModel<TAMsUserViewModel>();
            if (CurrentUser.Get().IsSuperAdmin || CurrentUser.Get().IsCompanyManager || CurrentUser.Get().IsDepartmentManager)
            {
                //tamvm.TAMsUserItems = GetUsersTAMoves(From, To, company, department, 0); // _reportRepository.FindAll();
                tamvm.TAMsUserItems = GetUsersTAMovesNew(From, To, company, department, BuildingId);
            }
            return PartialView("TAMounthReportGridViewPartialView1", tamvm.TAMsUserItems);
        }

        [ValidateInput(false)]
        public ActionResult TAStartEnd([ModelBinder(typeof(DevExpressEditorsBinder))]MVCxGridViewBatchUpdateValues<TAMove, int> updateValues)
        {
            foreach (var product in updateValues.Insert)
            {
                if (updateValues.IsValid(product))
                    InsertProduct(product, updateValues);
            }
            foreach (var product in updateValues.Update)
            {
                if (updateValues.IsValid(product))
                    UpdateProduct(product, updateValues);
            }
            foreach (var ID in updateValues.DeleteKeys)
            {
                _TAMoveService.DeleteTAMove(ID);
            }

            string StartDate = (string)Session["TAStartDate"];
            string StoptDate = (string)Session["TAStoptDate"];
            int? CompanyId = (int?)Session["TACompanyId"];
            int? department = (int?)Session["TAdepartmentId"];
            DateTime From = DateTime.ParseExact(StartDate, "dd.MM.yyyy",
                           System.Globalization.CultureInfo.InvariantCulture);

            DateTime To = DateTime.ParseExact(StoptDate, "dd.MM.yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);

            return PartialView("---", GetUsersTAMoves(From, To, CompanyId, department, 0));
        }
        [HttpGet]
        public ActionResult TAReportDetailed(int? department, int? company, int id, string FromDateTA, string ToDateTA, int? BuildingId)
        {
            DateTime From = DateTime.ParseExact(FromDateTA, "dd.MM.yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);

            DateTime To = DateTime.ParseExact(ToDateTA, "dd.MM.yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);

            Session["TAStartDate"] = FromDateTA;
            Session["TAStoptDate"] = ToDateTA;
            Session["TACompanyId"] = company;
            Session["TAdepartmentId"] = department;
            var tamvm = CreateViewModel<TAMsUserViewModel>();
            if (CurrentUser.Get().IsSuperAdmin || CurrentUser.Get().IsCompanyManager || CurrentUser.Get().IsDepartmentManager)
            {
                if (CurrentUser.Get().IsCompanyManager)
                {
                    company = CurrentUser.Get().CompanyId;
                    //tamvm.TAMsUserItems = GetUsersTAMoves(From, To, company, department, BuildingId);
                    tamvm.TAMsUserItems = GetUsersTAMovesNew(From, To, company, department, BuildingId);
                }
                else
                {
                    //tamvm.TAMsUserItems = GetUsersTAMoves(From, To, company, department, BuildingId); // _reportRepository.FindAll();
                    tamvm.TAMsUserItems = GetUsersTAMovesNew(From, To, company, department, BuildingId);
                }
            }
            return PartialView("TAReportDetailed", tamvm);
        }

        [ValidateInput(false)]
        public ActionResult TAReportDetailedGridEditA([ModelBinder(typeof(DevExpressEditorsBinder))]MVCxGridViewBatchUpdateValues<TAMove, int> updateValues)
        {
            foreach (var product in updateValues.Insert)
            {
                if (updateValues.IsValid(product))
                    InsertProduct(product, updateValues);
            }
            foreach (var product in updateValues.Update)
            {
                if (updateValues.IsValid(product))
                    UpdateProduct(product, updateValues);
            }
            foreach (var ID in updateValues.DeleteKeys)
            {
                _TAMoveService.DeleteTAMove(ID);
            }

            string StartDate = (string)Session["TAStartDate"];
            string StoptDate = (string)Session["TAStoptDate"];
            int? CompanyId = (int?)Session["TACompanyId"];
            int? department = (int?)Session["TAdepartmentId"];
            DateTime From = DateTime.ParseExact(StartDate, "dd.MM.yyyy",
                           System.Globalization.CultureInfo.InvariantCulture);

            DateTime To = DateTime.ParseExact(StoptDate, "dd.MM.yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);

            return PartialView("TAMounthReportGridViewPartialView", GetTAreports(From, To, CompanyId, department,null));
        }
        [ValidateInput(false)]
        public void ExternalEditFormDelete(int? Id)
        {
            if (Id != null)
            {
                _TAMoveService.DeleteTAMove(Id.GetValueOrDefault());
            }
        }

        public ActionResult DrillDownGridViewPartial()
        {
            return PartialView("TAReportDetailed");
        }

        public PdfExportOptions GetPdfExportOptions()
        {
            var exportOptions = new PdfExportOptions();
            exportOptions.DocumentOptions.Title = "Report";
            exportOptions.DocumentOptions.Subject = "Subject";
            exportOptions.DocumentOptions.Author = "Author";
            return exportOptions;
        }
        private GridViewSettings GetPivotSettingsNewBuildingForEnglish(int? timeFormat)
        {
            var settings = new GridViewSettings();
            string StartDate = (string)Session["TAStartDate"];
            string StoptDate = (string)Session["TAStoptDate"];
            int department = Convert.ToInt32(Session["department"]);
            int company = Convert.ToInt32(Session["company"]);
            //  var qry = _UserDepartmentRepository.FindAll(x => x.Id == department).SingleOrDefault();

            var qry = db.Companies.Where(x => x.Id == company).SingleOrDefault();

            if (qry != null)
            {
                cname = qry.Name;
            }
            var qry1 = db.Departments.Where(x => x.Id == department).SingleOrDefault();

            if (qry1 != null)
            {
                depname = qry1.Name;
            }

            settings.Name = "GridView";
            settings.CallbackRouteValues = new { Controller = "Home", Action = "GridViewPartial" };
            // Export-specific settings  
            // settings.SettingsExport.ExportedRowType = DevExpress.Web.Export.GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Report.xls";
            settings.SettingsExport.PaperKind = System.Drawing.Printing.PaperKind.A4;
            settings.Width = System.Web.UI.WebControls.Unit.Percentage(100);
            settings.ControlStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            settings.KeyFieldName = "id";
            settings.Settings.ShowFilterRow = true;
            settings.SettingsExport.Styles.Header.Font.Bold = true;
            //settings.Columns.Add("User.LastName");
            //settings.Columns.Add("User.FirstName");
            settings.Columns.Add(column =>
            {
                //column.FieldName = "User.LastName";
                column.FieldName = "FullName";
                // column.Caption = null;
                //column.Caption = "Name";
                //settings.SettingsExport.Styles.Cell.Font.Bold = true;
                //settings.SettingsExport.Styles.Header.Font.Bold = true;
                column.Caption = "Full Name";
                column.GroupIndex = 1;
                column.Width = 50;
                // column.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
                column.Visible = false;
            });

            settings.Columns.Add(column =>
            {
                //column.FieldName = "User.LastName";
                column.FieldName = "companydeatils";
                column.Caption = "Company Details";
                column.Visible = false;
                //column.Caption = "Name";
                column.GroupIndex = 0;
                column.Width = 50;
                // column.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "FirstName";
                column.Caption = "First Name";
                column.CellStyle.Font.Bold = true;
                //settings.SettingsExport.Styles.Cell.Font.Bold = true;
                //settings.SettingsExport.Styles.Cell.Font.Bold = true;
                //settings.SettingsExport.Styles.Header.Font.Bold = true;
            });


            settings.Columns.Add(column =>
            {
                column.FieldName = "LastName";
                column.Caption = "Last Name";
                //settings.SettingsExport.Styles.Cell.Font.Bold = true;
                //settings.SettingsExport.Styles.Header.Font.Bold = true;
            });



            settings.Columns.Add(column =>
            {
                column.FieldName = "PersonalCode";
                column.Caption = "Personal Code";
                column.CellStyle.Font.Bold = true;
                // settings.SettingsExport.Styles.Cell.Font.Bold = true;
                settings.SettingsExport.Styles.Cell.Font.Bold = true;
                //settings.SettingsExport.Styles.Header.Font.Bold = true;
                // column.PropertiesEdit.DisplayFormatString = "d";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "Name";
                column.Caption = "Designation";
                //settings.SettingsExport.Styles.Cell.Font.Bold = true;
                //settings.SettingsExport.Styles.Header.Font.Bold = true;
            });
            settings.SettingsExport.Styles.Cell.Font.Bold = true;
            settings.SettingsExport.Styles.Header.Font.Bold = true;
            settings.Columns.Add(column =>
            {
                column.FieldName = "Started";
                column.Caption = "Started";
                //settings.SettingsExport.Styles.Header.Font.Bold = true;
                column.PropertiesEdit.DisplayFormatString = "dddd M/d/yyyy ";
                column.Width = 200;

            });
            //settings.Columns.Add(column =>
            //{
            //    column.FieldName = "Started";
            //    column.Caption = " ";
            //    // column.PropertiesEdit.DisplayFormatString = "d";
            //    column.PropertiesEdit.DisplayFormatString = "HH:mm";
            //    column.Width = 50;
            //});
            settings.Columns.Add(column =>
            {
                column.FieldName = "checkin";
                column.Caption = "Check In";
                // column.PropertiesEdit.DisplayFormatString = "d";
                //settings.SettingsExport.Styles.Header.Font.Bold = true;
                //settings.SettingsExport.Styles.Header.Font.Bold = true;
                column.Width = 30;
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "checkout";
                column.Caption = "Check Out";
                // column.PropertiesEdit.DisplayFormatString = "d";
                column.Width = 30;
            });
            settings.SettingsExport.Styles.Header.Font.Bold = true;
            settings.Columns.Add(column =>
            {
                column.FieldName = "CompnyId";
                //column.GroupIndex = 0;
                column.Visible = false;
                // column.PropertiesEdit.DisplayFormatString = "d";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "Hours";
                column.Visible = false;
                // column.PropertiesEdit.DisplayFormatString = "d";
            });

            //settings.Columns.Add(column =>
            //{
            //    column.FieldName = "companycount";

            //    column.Visible = false;
            //    // column.PropertiesEdit.DisplayFormatString = "d";

            //});
            settings.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "Hours");
            //settings.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "totaltime").ShowInGroupFooterColumn = "totaltime";
            //settings.SettingsExport.Styles.Header.Font.Bold = true;
            settings.Settings.ShowGroupedColumns = true;
            settings.Columns.Add(column =>
            {
                column.FieldName = "totaltime";
                column.Caption = "Total Time";
                column.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
                column.Width = 30;
                //column.PropertiesEdit.DisplayFormatString = "HH:mm";
                //column.PropertiesEdit.NullDisplayText = " ";
                //settings.SettingsExport.Styles.Header.Font.Bold = true;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "Remark";
                column.Caption = "Comment";
                column.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
                column.Width = 30;
                //column.PropertiesEdit.DisplayFormatString = "HH:mm";
                //column.PropertiesEdit.NullDisplayText = " ";
                //settings.SettingsExport.Styles.Header.Font.Bold = true;
            });
            settings.SummaryDisplayText = (sender, e) =>
            {
                settings.Styles.Cell.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                settings.SettingsExport.Styles.Header.Font.Bold = true;
                if (e.IsGroupSummary)
                {
                    if (e.Item.FieldName == "Hours")
                    {
                        double time = Double.Parse(e.Value.ToString());
                        string hours = Math.Floor((TimeSpan.FromSeconds(time)).TotalHours) > 9 ? Math.Floor((TimeSpan.FromSeconds(time)).TotalHours).ToString() : "0" + Math.Floor((TimeSpan.FromSeconds(time)).TotalHours).ToString();
                        string minutes = (TimeSpan.FromSeconds(time)).Minutes > 9 ? (TimeSpan.FromSeconds(time)).Minutes.ToString() : "0" + (TimeSpan.FromSeconds(time)).Minutes;
                        //e.Text = "a month together: = " + hours + ":" + minutes;
                        string val1 = "Total Hours: = " + hours + ":" + minutes;
                        string te = val1.Replace(",", "");
                        e.Text = te;
                        //tt = e.Text;
                    }
                }
            };

            //settings.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "totaltime").ShowInGroupFooterColumn = "totaltime";
            //settings.GroupSummary.Add(DevExpress.Data.SummaryItemType.Count, "totaltime").ShowInGroupFooterColumn = "totaltime";
            //string companytoal = "";
            //settings.Settings.ShowFooter = true;
            settings.Settings.ShowGroupFooter = GridViewGroupFooterMode.VisibleIfExpanded;
            settings.Settings.ShowGroupPanel = true;
            //int temp = 0; int i = 0;
            settings.Settings.ShowGroupPanel = true;
            settings.GroupSummary.Add(DevExpress.Data.SummaryItemType.Custom, "totaltime").DisplayFormat = "c";
            settings.GroupSummary.Add(DevExpress.Data.SummaryItemType.Custom, "totaltime").ShowInGroupFooterColumn = "totaltime";
            settings.SettingsBehavior.AutoExpandAllGroups = true;
            return settings;
        }
        private GridViewSettings GetPivotSettingsNewBuilding(int? timeFormat)
        {
            var settings = new GridViewSettings();
            string StartDate = (string)Session["TAStartDate"];
            string StoptDate = (string)Session["TAStoptDate"];
            int department = Convert.ToInt32(Session["department"]);
            int company = Convert.ToInt32(Session["company"]);
            //  var qry = _UserDepartmentRepository.FindAll(x => x.Id == department).SingleOrDefault();
            var qry = db.Companies.Where(x => x.Id == company).SingleOrDefault();
            if (qry != null)
            {
                cname = qry.Name;
            }
            var qry1 = db.Departments.Where(x => x.Id == department).SingleOrDefault();

            if (qry1 != null)
            {
                depname = qry1.Name;
            }

            settings.Name = "GridView";
            settings.CallbackRouteValues = new { Controller = "Home", Action = "GridViewPartial" };
            // Export-specific settings  
            // settings.SettingsExport.ExportedRowType = DevExpress.Web.Export.GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Report.xls";
            settings.SettingsExport.PaperKind = System.Drawing.Printing.PaperKind.A4;
            settings.Width = System.Web.UI.WebControls.Unit.Percentage(100);
            settings.ControlStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            settings.KeyFieldName = "id";
            settings.Settings.ShowFilterRow = true;
            //settings.Columns.Add("User.LastName");
            //settings.Columns.Add("User.FirstName");
            settings.Columns.Add(column =>
            {
                //column.FieldName = "User.LastName";
                column.FieldName = "FullName";
                // column.Caption = null;
                //column.Caption = "Name";
                column.Caption = ViewResources.SharedStrings.FullName;
                column.GroupIndex = 1;
                column.Width = 30;
                // column.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
                column.Visible = false;
            });

            settings.Columns.Add(column =>
            {
                //column.FieldName = "User.LastName";
                column.FieldName = "companydeatils";
                //column.Caption = "Uzņēmuma dati";
                column.Caption = ViewResources.SharedStrings.CompanyDetails;
                column.Visible = false;
                //column.Caption = "Name";
                column.GroupIndex = 0;
                column.Width = 50;
                // column.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "FirstName";
                //column.Caption = "Vārds";
                column.Caption = ViewResources.SharedStrings.Name;
                // column.PropertiesEdit.DisplayFormatString = "d";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "LastName";
                //column.Caption = "Uzvārds";
                column.Caption = ViewResources.SharedStrings.SurName;
                // column.PropertiesEdit.DisplayFormatString = "d";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "PersonalCode";
                //column.Caption = "Personas kods";
                column.Caption = ViewResources.SharedStrings.UsersPersonalCode;
                // column.PropertiesEdit.DisplayFormatString = "d";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "Name";
                //column.Caption = "Profesija, amats";
                column.Caption = ViewResources.SharedStrings.Position;
                // column.PropertiesEdit.DisplayFormatString = "d";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "Started";
                //column.Caption = "Datums";
                column.Caption = ViewResources.SharedStrings.Started;
                column.PropertiesEdit.DisplayFormatString = "dddd M/d/yyyy ";
                column.Width = 200;

            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "checkin";
                //column.Caption = "Iegāja";
                column.Caption = ViewResources.SharedStrings.checkin;
                // column.PropertiesEdit.DisplayFormatString = "d";
                column.Width = 30;
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "checkout";
                //column.Caption = "Izgāja";
                column.Caption = ViewResources.SharedStrings.checkout;
                // column.PropertiesEdit.DisplayFormatString = "d";
                column.Width = 30;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "CompnyId";
                //column.GroupIndex = 0;
                column.Visible = false;
                // column.PropertiesEdit.DisplayFormatString = "d";

            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "Hours";
                column.Visible = false;
                // column.PropertiesEdit.DisplayFormatString = "d";
            });

            //settings.Columns.Add(column =>
            //{
            //    column.FieldName = "companycount";

            //    column.Visible = false;
            //    // column.PropertiesEdit.DisplayFormatString = "d";

            //});
            settings.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "Hours");
            //settings.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "totaltime").ShowInGroupFooterColumn = "totaltime";
            settings.Settings.ShowGroupedColumns = true;
            settings.Columns.Add(column =>
            {
                column.FieldName = "totaltime";
                //column.Caption = "Kopā";
                column.Caption = ViewResources.SharedStrings.TotalTime;
                column.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
                //column.PropertiesEdit.DisplayFormatString = "HH:mm";
                //column.PropertiesEdit.NullDisplayText = " ";
                column.Width = 30;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "Remark";
                //column.Caption = "komentēt";
                column.Caption = ViewResources.SharedStrings.CommonComment;
                column.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
                //column.PropertiesEdit.DisplayFormatString = "HH:mm";
                //column.PropertiesEdit.NullDisplayText = " ";
                column.Width = 30;
            });
            settings.SummaryDisplayText = (sender, e) =>
            {
                settings.Styles.Cell.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                if (e.IsGroupSummary)
                {
                    if (e.Item.FieldName == "Hours")
                    {
                        double time = Double.Parse(e.Value.ToString());
                        string hours = Math.Floor((TimeSpan.FromSeconds(time)).TotalHours) > 9 ? Math.Floor((TimeSpan.FromSeconds(time)).TotalHours).ToString() : "0" + Math.Floor((TimeSpan.FromSeconds(time)).TotalHours).ToString();
                        string minutes = (TimeSpan.FromSeconds(time)).Minutes > 9 ? (TimeSpan.FromSeconds(time)).Minutes.ToString() : "0" + (TimeSpan.FromSeconds(time)).Minutes;
                        //e.Text = "a month together: = " + hours + ":" + minutes;
                        string val1 = ViewResources.SharedStrings.TotalHours + ": = " + hours + ":" + minutes;
                        string te = val1.Replace(",", "");
                        e.Text = te;
                        //tt = e.Text;
                    }
                }
            };

            settings.Settings.ShowGroupFooter = GridViewGroupFooterMode.VisibleIfExpanded;
            settings.Settings.ShowGroupPanel = true;
            settings.Settings.ShowGroupPanel = true;
            settings.GroupSummary.Add(DevExpress.Data.SummaryItemType.Custom, "totaltime").DisplayFormat = "c";
            settings.GroupSummary.Add(DevExpress.Data.SummaryItemType.Custom, "totaltime").ShowInGroupFooterColumn = "totaltime";
            settings.SettingsBehavior.AutoExpandAllGroups = true;
            return settings;
        }

        private PivotGridSettings GetPivotSettings(int? timeFormat)
        {
            var settings = new PivotGridSettings();

            var fileName = CreateFileName("ReportByDays");
            settings.Name = fileName;
            settings.CallbackRouteValues = new { Controller = "TAReport", Action = "TAMounthReportGridViewPartialA" };
            //settings.CustomActionRouteValues = new { Controller = "TAReport", Action = "MounthBatchGridEditA" };
            settings.OptionsView.VerticalScrollBarMode = ScrollBarMode.Auto;
            settings.OptionsView.HorizontalScrollBarMode = ScrollBarMode.Auto;

            settings.ControlStyle.Border.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(1);
            settings.Styles.CellStyle.Border.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(1);
            settings.StylesPager.Pager.Border.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(1);
            settings.ControlStyle.BorderBottom.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(2);

            settings.SettingsExport.OptionsPrint.PageSettings.Landscape = true;//.PaperKind = System.Drawing.Printing.PaperKind.A4;
            settings.SettingsExport.OptionsPrint.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;

            settings.ControlStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            settings.OptionsPager.Visible = false;
            settings.OptionsFilter.NativeCheckBoxes = true;
            settings.SettingsExport.OptionsPrint.MergeRowFieldValues = false;
            //settings.SettingsExport.OptionsPrint.MergeColumnFieldValues = false;

            //settings.OptionsView.ShowColumnTotals = true;
            settings.OptionsView.ShowCustomTotalsForSingleValues = false;
            settings.OptionsView.ShowRowGrandTotals = true;
            //settings.OptionsView.ShowAllTotals();
            settings.OptionsView.ShowColumnGrandTotals = true;
            //settings.OptionsView.HideAllTotals();         

            settings.Fields.Add(field =>
            {
                //field.Area = PivotArea.RowArea;
                field.Caption = "Name";
                field.FieldName = "FullName";
                field.CellStyle.Font.Size = FontUnit.Medium;
            });

            //settings.Fields.Add(field =>
            //{
            //    //field.Area = PivotArea.RowArea;
            //    field.Caption = "Last Name";
            //    field.FieldName = "LastName";
            //    field.Visible = false;
            //});

            //settings.Fields.Add(field =>
            //{
            //    //field.Area = PivotArea.RowArea;

            //    field.Caption = "Name";
            //    field.FieldName = "FirstName";
            //    field.Visible = false;
            //});


            //settings.Fields.Add(field =>
            //{
            //    //field.Area = PivotArea.RowArea;
            //    field.Caption = "UserId";
            //    field.FieldName = "UserId";
            //    //field.Width = 10;
            //    //field.Visible = false;
            //});

            settings.Fields.Add(field =>
            {
                field.Area = PivotArea.ColumnArea;
                field.FieldName = "ReportDate";
                field.Caption = "ReportDate";
                field.GroupInterval = PivotGroupInterval.DateMonth;
                field.UseNativeFormat = DevExpress.Utils.DefaultBoolean.False;
                field.CellStyle.Font.Size = FontUnit.Medium;
            });

            settings.Fields.Add(field =>
            {
                field.Area = PivotArea.ColumnArea;
                field.FieldName = "ReportDate";
                field.Caption = "Quarter";
                field.GroupInterval = PivotGroupInterval.DateDay;
                field.CellStyle.Font.Size = FontUnit.Medium;
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
                field.UseNativeFormat = DevExpress.Utils.DefaultBoolean.False;
                field.CellStyle.Font.Size = FontUnit.Medium;
            });
            if (timeFormat == 1)
            {
                settings.CustomCellDisplayText = (sender, e) =>
                {
                    if (e.ColumnValueType == PivotGridValueType.Total || e.ColumnValueType == PivotGridValueType.GrandTotal)
                    {
                        double time1 = Convert.ToDouble(e.Value);
                        string hours = Math.Floor((TimeSpan.FromSeconds(time1)).TotalHours) > 9 ? Math.Floor((TimeSpan.FromSeconds(time1)).TotalHours).ToString() : "0" + Math.Floor((TimeSpan.FromSeconds(time1)).TotalHours).ToString();
                        string minutes = (TimeSpan.FromSeconds(time1)).Minutes > 9 ? (TimeSpan.FromSeconds(time1)).Minutes.ToString() : "0" + (TimeSpan.FromSeconds(time1)).Minutes;
                        e.DisplayText = hours + ":" + minutes;
                        return;
                    }
                    double time = Convert.ToDouble(e.Value);
                    if (time <= 0)
                    {
                        e.DisplayText = "";
                        return;
                    }
                    try
                    {
                        e.DisplayText = DateTime.MinValue.Add(TimeSpan.FromSeconds(time)).ToString("HH:mm");
                    }
                    catch
                    {
                        return;
                    }
                };
            }
            else
            {
                settings.CustomCellDisplayText = (sender, e) =>
                {
                    double time = Convert.ToDouble(e.Value);
                    //if (time == 0)
                    if (time <= 0)
                        return;
                    double time2 = Math.Floor((TimeSpan.FromSeconds(Convert.ToDouble(e.Value))).TotalSeconds);
                    e.DisplayText = (time2 / 3600).ToString("F2");
                };
            }

            settings.CustomFieldValueCells = (sender, e) =>
            {

                MVCxPivotGrid pivot = (MVCxPivotGrid)sender;
                if (pivot.DataSource == null) return;

                var cell = e.FindCell(false,
                                    dataCellValues =>
                                                dataCellValues
                                                    .All(value => Equals((decimal)0, value)));
                if (cell != null)
                    e.Remove(cell);
            };

            settings.Styles.FieldValueStyle.Font.Size = FontUnit.Medium;
            settings.Styles.CellStyle.Font.Size = FontUnit.Medium;

            return settings;
        }
        private GridViewSettings GetGridSettingsExcels(int? timeFormat)
        {
            var settings = new GridViewSettings();
            //---> my
            string StartDate = (string)Session["TAStartDate"];
            string StoptDate = (string)Session["TAStoptDate"];
            int department = Convert.ToInt32(Session["department"]);
            int company = Convert.ToInt32(Session["company"]);
            //  var qry = _UserDepartmentRepository.FindAll(x => x.Id == department).SingleOrDefault();

            var qry = db.Companies.Where(x => x.Id == company).SingleOrDefault();

            if (qry != null)
            {
                cname = qry.Name;
            }
            var qry1 = db.Departments.Where(x => x.Id == department).SingleOrDefault();
            if (qry1 != null)
            {
                depname = qry1.Name;
            }
            settings.SettingsExport.PageHeader.Center = depname + " - " + cname + " ";
            settings.SettingsExport.PageHeader.Left = StartDate + " - " + StoptDate + " ";
            settings.SettingsExport.PageHeader.Right = DateTime.Now.Date.ToString("dd.MM.yyyy");
            //<--- my
            settings.Name = "GridView";
            settings.CallbackRouteValues = new { Controller = "Home", Action = "GridViewPartial" };
            // Export-specific settings  
            // settings.SettingsExport.ExportedRowType = DevExpress.Web.Export.GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Report.xls";
            settings.SettingsExport.PaperKind = System.Drawing.Printing.PaperKind.A4;
            settings.Width = System.Web.UI.WebControls.Unit.Percentage(100);
            settings.ControlStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            settings.KeyFieldName = "Id";
            //settings.Columns.Add("User.LastName");
            // settings.Columns.Add("User.FirstName");
            settings.Columns.Add(column =>
            {
                //column.FieldName = "User.LastName";
                column.FieldName = "UserId";
                // column.Caption = null;
                column.Caption = "UserId";
                column.GroupIndex = 0;
                column.Width = 50;
                column.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
                //column.Visible = false;
            });

            settings.Columns.Add("UserId");
            //settings.Columns.Add("User.FirstName");

            settings.Columns.Add(column =>
            {
                column.FieldName = "Started";
                column.Caption = " ";
                // column.PropertiesEdit.DisplayFormatString = "d";
                column.PropertiesEdit.DisplayFormatString = "dddd M/d/yyyy ";
                column.Width = 200;
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "Started";
                column.Caption = " ";
                // column.PropertiesEdit.DisplayFormatString = "d";
                column.PropertiesEdit.DisplayFormatString = "HH:mm";
                column.Width = 50;
            });
            //toTime
            settings.Columns.Add(column =>
            {
                column.FieldName = "Finished";
                column.Caption = " ";
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
                column.FieldName = "colorflag";
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
                column.FieldName = "User.LastName";
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
            settings.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "Hours");
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

            return settings;
        }

        private GridViewSettings GetGridSettings(int? timeFormat)
        {
            /* string countryflag = "";
             if (Session["Language"] == null)
             {
                 countryflag = "EN";
             }
             else
             {
                 countryflag = Session["Language"].ToString();
             }
             */
            var settings = new GridViewSettings();
            //---> my
            string StartDate = (string)Session["TAStartDate"];
            string StoptDate = (string)Session["TAStoptDate"];
            int Usrid = Convert.ToInt32(Session["User_Id"]);
            int usrrole = Convert.ToInt32(Session["Role_ID"]);
            string ufname = "";
            string Ulname = "";
            string Userdesignation1 = "";
            int department = Convert.ToInt32(Session["department"]);
            int company = Convert.ToInt32(Session["company"]);
            int Bul_id = Convert.ToInt32(Session["Buidlidngid"]);
            int companyId1 = Convert.ToInt32(Session["company"]);
            var CompanyRecords = db.Companies.Where(x => x.Id == companyId1).FirstOrDefault();
            string companyName = "", RegistraionNum = "";
            if (CompanyRecords != null)
            {
                companyName = CompanyRecords.Name;
                RegistraionNum = CompanyRecords.Comment;
                if (RegistraionNum == null)
                {
                    RegistraionNum = "NA";
                }
            }
            try
            {
                string getvalidcompid = @" select  FirstName,LastName from Users    where Id={0}";
                var userLogDeatil = db.Database.SqlQuery<specificUser>(getvalidcompid, Usrid).FirstOrDefault();

                if (userLogDeatil != null)
                {
                    ufname = userLogDeatil.FirstName;
                    Ulname = userLogDeatil.LastName;
                }
                var userDesignation = db.UserRoles.Where(x => x.Id == usrrole).SingleOrDefault();
                if (userDesignation != null)
                {
                    Userdesignation1 = userDesignation.Name;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (department != 0)
            {
                var dept = db.Departments.SingleOrDefault(x => x.Id == department);
                depname = dept.Name;
                if (company == 0)
                {
                    company = dept.CompanyId;
                }
            }

            DateTime From = DateTime.ParseExact(StartDate, "dd.MM.yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
            DateTime To = DateTime.ParseExact(StoptDate, "dd.MM.yyyy",
                                      System.Globalization.CultureInfo.InvariantCulture);

            string bname = Session["BuildingName"].ToString();
            var ee = (from Ta in db.TABuildingName
                      join B in db.Buildings on Ta.BuildingId equals B.Id
                      where Ta.Name == bname && Ta.ValidFrom <= From && Ta.ValidTo >= To
                      select new
                      {
                          taname = Ta.Name,
                          Ta.ValidFrom,
                          Ta.ValidTo,
                          Ta.BuildingLicense,
                          Ta.CadastralNr,
                          Ta.Address,
                          B.Name,
                      }).FirstOrDefault();

            string ReportPrepareby = ufname + " " + Ulname + "  " + "(" + Userdesignation1 + ")";
            var qry = db.Companies.Where(x => x.Id == company).SingleOrDefault();
            if (qry != null)
            {
                cname = qry.Name;
            }

            string pageHeader = "";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("Report Created: {0} ", ReportPrepareby));
            sb.AppendLine(string.Format("Report Creation Date : {0} ", DateTime.Now.Date.ToString("dd.MM.yyyy")));

            if (company != 0)
            {
                sb.AppendLine(string.Format("Company: {0} ", cname));
            }
            if (department != 0)
            {
                sb.AppendLine(string.Format("Department: {0} ", depname));
            }

            if (ee != null)
            {
                if (!String.IsNullOrEmpty(ee.Name))
                {
                    sb.AppendLine(string.Format("Building Name: {0} ", ee.Name));
                }
                else
                {
                    sb.AppendLine(string.Format("Building Name: xxxx"));
                }

                if (!String.IsNullOrEmpty(ee.Address))
                {
                    sb.AppendLine(string.Format("Address: {0} ", ee.Address));
                }
                else
                {
                    sb.AppendLine(string.Format("Address: xxxx"));
                }

                if (!String.IsNullOrEmpty(ee.BuildingLicense))
                {
                    sb.AppendLine(string.Format("Building License: {0} ", ee.BuildingLicense));
                }
                else
                {
                    sb.AppendLine(string.Format("Building License: xxxx"));
                }

                if (!String.IsNullOrEmpty(ee.CadastralNr))
                {
                    sb.AppendLine(string.Format("Cadastral Number: {0} ", ee.CadastralNr));
                }
                else
                {
                    sb.AppendLine(string.Format("Cadastral Number: xxxx"));
                }
            }
            else
            {
                sb.AppendLine(string.Format("Building Name: All Buildings"));
                //sb.AppendLine(string.Format("Address: xxxx"));
                //sb.AppendLine(string.Format("Building License: xxxx"));
                //sb.AppendLine(string.Format("Cadastral Number: xxxx"));
            }
            sb.AppendLine(" ");
            sb.AppendLine(string.Format("Period: {0} ", StartDate + " - " + StoptDate));

            sb.AppendLine(" ");
            pageHeader = sb.ToString();//.Replace("<br />", Environment.NewLine);
                                       //if (company != 0)
                                       //{
                                       //    pageHeader += "Company : " + cname+"\n";
                                       //}
                                       //if (department!=0)
                                       //{
                                       //    pageHeader+= "Department : " + depname;
                                       //}

            settings.SettingsExport.PageHeader.Center = pageHeader; //ReportPrepareby;//"Department: "+ depname + " - " + cname + " ";

            settings.SettingsExport.PageHeader.Font.Bold = true;
            settings.Name = "GridView";
            settings.CallbackRouteValues = new { Controller = "Home", Action = "GridViewPartial" };
            settings.SettingsExport.FileName = "Report.pdf";
            settings.SettingsExport.PaperKind = System.Drawing.Printing.PaperKind.A3Extra;
            settings.Width = System.Web.UI.WebControls.Unit.Percentage(100);
            settings.ControlStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            settings.KeyFieldName = "Id";

            settings.Columns.Add(column =>
            {
                column.FieldName = "FullName";
                column.Caption = "Full Name";
                column.GroupIndex = (CurrentUser.Get().IsSuperAdmin || CurrentUser.Get().IsCompanyManager) ? 2 : 1;
                column.Visible = false;
            });

            if (CurrentUser.Get().IsCompanyManager || CurrentUser.Get().IsSuperAdmin)
            {
                settings.Columns.Add(column =>
                {
                    column.FieldName = "companydeatils";
                    column.Caption = "Company Details";
                    column.Visible = false;
                    column.GroupIndex = 0;
                });
            }
            settings.Columns.Add(column =>
            {
                column.FieldName = "FirstName";
                column.Caption = "First Name";
                column.CellStyle.Font.Bold = true;
            });


            settings.Columns.Add(column =>
            {
                column.FieldName = "LastName";
                column.Caption = "Last Name";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "UserId";
                column.Caption = "UserId";
                column.Width = 35;
                column.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
                column.Visible = false;
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "PersonalCode";
                column.Caption = "Personal Code";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "Name";
                column.Caption = "Designation";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "Department";

                column.Caption = "Department";

                if (CurrentUser.Get().IsCompanyManager || CurrentUser.Get().IsSuperAdmin)
                {
                    column.GroupIndex = 1;
                }
                else if (CurrentUser.Get().IsDepartmentManager)
                {
                    column.GroupIndex = 0;
                }
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "Started";
                column.Caption = "Started";
                column.PropertiesEdit.DisplayFormatString = "dddd M/d/yyyy ";
                column.Width = 100;
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "Started";
                column.Caption = "Check In";
                column.PropertiesEdit.DisplayFormatString = "HH:mm";
                column.Width = 30;
            });
            //toTime
            settings.Columns.Add(column =>
            {
                column.FieldName = "Finished";
                column.Caption = "Check Out";
                column.PropertiesEdit.DisplayFormatString = "d";
                column.PropertiesEdit.DisplayFormatString = "HH:mm";
                column.Width = 30;
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "Hours";
                column.Visible = false;
                column.Width = 30;
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "Total";
                column.Caption = "Total Time";
                column.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
                column.PropertiesEdit.DisplayFormatString = "HH:mm";
                column.PropertiesEdit.NullDisplayText = " ";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "Comment";
                column.Caption = "Comment";
                column.Width = 50;
            });

            settings.CustomUnboundColumnData = (sender, e) =>
            {
                if (e.Column.FieldName == "Total")
                {
                    double time = Convert.ToDouble(e.GetListSourceFieldValue("Hours"));
                    if (time <= 0) { e.Value = "00:00";/*null;*/ }
                    else
                    {
                        e.Value = DateTime.MinValue.Add(TimeSpan.FromSeconds(time));
                    }
                }
            };

            //Summary
            settings.Settings.ShowFooter = true;
            settings.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "Hours");
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
            return settings;
        }
        #endregion
        #region Export

        [ValidateInput(false)]
        public void ExportToParams(int Reporttype, int ReportFormat, List<int> a)
        {
            Session["Users"] = a;
            Session["Reporttype"] = Reporttype;
            Session["ReportFormat"] = ReportFormat;
        }

        [ValidateInput(false)]

        public ActionResult ExportTo()
        {

            string countryflag = "";
            string StartDate = (string)Session["TAStartDate"];
            string StoptDate = (string)Session["TAStoptDate"];
            List<int> UserIds = (List<int>)Session["Users"];
            int? Reporttype = (int?)Session["Reporttype"];
            int? ReportFormat = (int?)Session["ReportFormat"];
            int? timeFormat = (int?)Session["timeFormat"];

            string dept = "";
            int department = Convert.ToInt32(Session["department"]);
            int company = Convert.ToInt32(Session["company"]);
            int building_id = Convert.ToInt32(Session["Buidlidngid"]);

            DateTime From = DateTime.ParseExact(StartDate, "dd.MM.yyyy",
                          System.Globalization.CultureInfo.InvariantCulture);
            DateTime To = DateTime.ParseExact(StoptDate, "dd.MM.yyyy",
                                           System.Globalization.CultureInfo.InvariantCulture);

            if (department != 0)
            {
                dept = db.Departments.SingleOrDefault(x => x.Id == department).Name;
            }
            if (Session["Language"] == null)
            {
                countryflag = "EN";
            }
            else
            {
                countryflag = Session["Language"].ToString();
            }

            if (Reporttype == 1)
            {
                //DetailedReport New
                var tamvm = CreateViewModel<TAMsUserViewModel>();
                TaReportViewNewCustomoseUser tavm = new TaReportViewNewCustomoseUser();
                List<TaNewUserDetails> listta_user = new List<TaNewUserDetails>();

                TaReportViewNewCustomoseUser tavm1 = new TaReportViewNewCustomoseUser();

                int i = 0;

                DateTime f1 = Convert.ToDateTime(Session["datefrom"]);
                DateTime f2 = Convert.ToDateTime(Session["dateto"]);
                int buildingid = Convert.ToInt32(Session["buildingid"]);

                List<itembuilding> Building_Data = new List<itembuilding>();
                var ctvm = new BuildingNameViewModel();

                if (building_id == 0)
                {
                    if (UserIds != null && UserIds.Count > 0)
                    {
                        DateTime Ton = To.AddDays(1);

                        var result1 = (from Ta in db.NewTaMoves
                                       join U in db.User on Ta.UserId equals U.Id
                                       join C in db.Companies on U.CompanyId equals C.Id
                                       join os in db.Title on U.TitleId equals os.Id into t
                                       from rt in t.DefaultIfEmpty()

                                       where Ta.Started >= From && Ta.Finished < Ton
                                       && UserIds.Contains(Ta.UserId) && Ta.IsDeleted == false
                                       select new
                                       {
                                           companyname = C.Name,
                                           C.Comment,
                                           Ta.UserId,
                                           U.FirstName,
                                           U.LastName,
                                           U.CompanyId,
                                           U.PersonalCode,
                                           rt.Name,
                                           U.Id,
                                           Ta.Started,
                                           Ta.Finished,
                                           Ta.Remark
                                       }).OrderBy(x => x.LastName).ToList();           //OrderBy(x => x.Id)

                        if (company == 0)
                        {
                            result1 = result1.OrderBy(x => x.CompanyId).ToList();
                        }
                        int temp = 0;
                        int temp1 = 0;

                        foreach (var items in result1)
                        {
                            DayOfWeek dow = items.Started.DayOfWeek;

                            var dept_nm = "";
                            if (department == 0)
                            {
                                con.Open();
                                SqlCommand cmd = new SqlCommand("select top 1 (select name from Departments where id=ud.DepartmentId) depname from UserDepartments ud where userid='" + items.UserId + "' and IsDeleted=0 and ValidFrom<='" + To.AddDays(-1).ToString("MM/dd/yyyy") + "' and ValidTo>='" + From.ToString("MM/dd/yyyy") + "'", con);
                                string depname = Convert.ToString(cmd.ExecuteScalar());
                                dept_nm = depname;
                                con.Close();
                            }
                            else
                            {
                                dept_nm = dept;
                            }

                            DateTime date = items.Started;
                            string onlydate = date.ToString("dd/mm/yyyy");

                            DateTime st = items.Started;
                            string checkin1 = st.ToString("HH:mm");

                            DateTime fn = items.Finished;
                            string checkout1 = fn.ToString("HH:mm");

                            TimeSpan checkin = date.TimeOfDay;
                            TimeSpan checkout = fn.TimeOfDay;

                            var tottaltime = checkout.Subtract(checkin);
                            string hh = tottaltime.Hours < 10 ? "0" + tottaltime.Hours : tottaltime.Hours.ToString();
                            string mm = tottaltime.Minutes < 10 ? "0" + tottaltime.Minutes : tottaltime.Minutes.ToString();
                            string rr = hh + ":" + mm;

                            var timeDiff = tottaltime.TotalSeconds;
                            double sec = timeDiff;


                            i = i + 1;
                            int k = 0; k++; int p = 0; p++;
                            Session["FullName"] = items.FirstName + " " + items.LastName;
                            if (temp != items.UserId)
                            {
                                temp = items.UserId;
                                k = 0;
                            }

                            if (temp1 != items.CompanyId)
                            {
                                temp1 = Convert.ToInt32(items.CompanyId);
                                p = 0;
                            }

                            string companydeatils1 = items.companyname + "  " + items.Comment;

                            listta_user.Add(new TaNewUserDetails()
                            {
                                id = i,
                                UserId = items.UserId,
                                CompnyId = Convert.ToInt32(items.CompanyId),
                                FirstName = items.FirstName,
                                LastName = items.LastName,
                                companydeatils = companydeatils1,
                                PersonalCode = items.PersonalCode,
                                Name = items.Name,
                                Started = items.Started,
                                Finished = items.Finished,
                                checkin = checkin1,
                                checkout = checkout1,
                                Hours = sec,
                                totaltime = rr,
                                FullName = items.FirstName + " " + items.LastName,
                                Department = dept_nm,
                                Comment = items.Remark
                            });

                        }
                        ViewBag.Result = result1;
                        tavm.TaUserDetails = listta_user;
                    }
                }
                else
                {
                    if (building_id != 0)
                    {
                        if (UserIds != null && UserIds.Count > 0)
                        {
                            DateTime Ton = To.AddDays(1);
                            var result2 = (from Ta in db.NewTaMoves
                                           join U in db.User on Ta.UserId equals U.Id
                                           join C in db.Companies on U.CompanyId equals C.Id
                                           join b in db.BuildingObject on Ta.FinishedBoId equals b.Id
                                           join os in db.Title on U.TitleId equals os.Id into t
                                           from rt in t.DefaultIfEmpty()
                                           where Ta.Started >= From && Ta.Finished < Ton
                                           && UserIds.Contains(Ta.UserId) && Ta.IsDeleted == false && b.BuildingId == building_id

                                           select new
                                           {
                                               companyname = C.Name,
                                               C.Comment,
                                               Ta.UserId,
                                               U.FirstName,
                                               U.LastName,
                                               U.CompanyId,
                                               U.PersonalCode,
                                               rt.Name,
                                               U.Id,
                                               Ta.Started,
                                               Ta.Finished,
                                               Ta.Remark
                                           }).OrderBy(x => x.LastName).ToList();       //OrderBy(x => x.Id).ToList();

                            if (company == 0)
                            {
                                result2 = result2.OrderBy(x => x.CompanyId).ToList();
                            }
                            int temp2 = 0;
                            int temp3 = 0;
                            foreach (var items in result2)
                            {
                                var dept_nm = "";
                                if (department == 0)
                                {
                                    con.Open();
                                    SqlCommand cmd = new SqlCommand("select top 1 (select name from Departments where id=ud.DepartmentId) depname from UserDepartments ud where userid='" + items.UserId + "' and IsDeleted=0 and ValidFrom<='" + To.AddDays(-1).ToString("MM/dd/yyyy") + "' and ValidTo>='" + From.ToString("MM/dd/yyyy") + "'", con);
                                    string depname = Convert.ToString(cmd.ExecuteScalar());
                                    dept_nm = depname;
                                    con.Close();
                                }
                                else
                                {
                                    dept_nm = dept;
                                }

                                DateTime date = items.Started;
                                string onlydate = date.ToString("dd/mm/yyyy");
                                DateTime st = items.Started;
                                string checkin1 = st.ToString("HH:mm");

                                DateTime fn = items.Finished;
                                string checkout1 = fn.ToString("HH:mm");

                                TimeSpan checkin = date.TimeOfDay;
                                TimeSpan checkout = fn.TimeOfDay;
                                var tottaltime = checkout.Subtract(checkin);
                                string hh = tottaltime.Hours < 10 ? "0" + tottaltime.Hours : tottaltime.Hours.ToString();
                                string mm = tottaltime.Minutes < 10 ? "0" + tottaltime.Minutes : tottaltime.Minutes.ToString();
                                string rr = hh + ":" + mm;

                                var timeDiff = tottaltime.TotalSeconds;
                                double sec = timeDiff;

                                i = i + 1;
                                int k = 0; k++; int p = 0; p++; int n = 0; n++;
                                Session["FullName"] = items.FirstName + " " + items.LastName;
                                if (temp2 != items.UserId)
                                {
                                    temp2 = items.UserId;
                                    k = 0;
                                }

                                if (temp3 != items.CompanyId)
                                {
                                    temp3 = Convert.ToInt32(items.CompanyId);
                                    p = 0;
                                }
                                string companydeatils1 = items.companyname + "  " + items.Comment;
                                listta_user.Add(new TaNewUserDetails()
                                {
                                    id = i,
                                    UserId = items.UserId,
                                    CompnyId = Convert.ToInt32(items.CompanyId),
                                    FirstName = items.FirstName,
                                    LastName = items.LastName,
                                    companydeatils = companydeatils1,
                                    PersonalCode = items.PersonalCode,
                                    Name = items.Name,
                                    Started = items.Started,
                                    Finished = items.Finished,
                                    checkin = checkin1,
                                    checkout = checkout1,
                                    Hours = sec,
                                    totaltime = rr,
                                    FullName = items.FirstName + " " + items.LastName,
                                    Department = dept_nm,
                                    Comment = items.Remark
                                });
                            }

                            ViewBag.Result = result2;
                        }
                        tavm.TaUserDetails = listta_user;
                    }
                }

                if (ReportFormat == 1)
                {
                    if (countryflag == "EN")
                    {
                        return GridViewExtension.ExportToPdf(GetGridSettings(timeFormat), tavm.TaUserDetails, GetPdfExportOptions());//tamvm.TAMsUserItems, GetPdfExportOptions());
                    }
                    else
                    {
                        //Report in Lativain
                        return GridViewExtension.ExportToPdf(GetGridSettingsInLat(timeFormat), tavm.TaUserDetails, GetPdfExportOptions());//tamvm.TAMsUserItems, GetPdfExportOptions());
                    }
                }
                else      //XSL format
                {
                    //In English
                    if (countryflag == "EN")
                    {
                        PrintingSystem ps = new PrintingSystem();
                        PrintableComponentLink link1 = new PrintableComponentLink(ps);
                        link1.Component = GridViewExtension.CreatePrintableObject(GetGridSettings(timeFormat), tavm.TaUserDetails);
                        link1.CreateReportHeaderArea += new CreateAreaEventHandler(Link_CreateMarginalHeaderArea);
                        link1.Landscape = true;
                        CompositeLink compositeLink = new CompositeLink(ps);
                        compositeLink.Links.AddRange(new object[] { link1 });
                        compositeLink.Landscape = true;
                        compositeLink.CreateDocument();
                        FileStreamResult result = CreateExcelExportResult(compositeLink, 0, "Report");
                        ps.Dispose();
                        return result;
                    }
                    else
                    {
                        PrintingSystem ps1 = new PrintingSystem();
                        PrintableComponentLink link2 = new PrintableComponentLink(ps1);
                        link2.Component = GridViewExtension.CreatePrintableObject(GetGridSettingsInLat(timeFormat), tavm.TaUserDetails);
                        link2.CreateReportHeaderArea += new CreateAreaEventHandler(Link_CreateMarginalHeaderAreaInLat);
                        link2.Landscape = true;
                        CompositeLink compositeLink = new CompositeLink(ps1);
                        compositeLink.Links.AddRange(new object[] { link2 });
                        compositeLink.Landscape = true;
                        compositeLink.CreateDocument();
                        FileStreamResult result1 = CreateExcelExportResult(compositeLink, 0, "Report");
                        ps1.Dispose();
                        return result1;
                    }
                }
            }
            else if (Reporttype == 2)
            { // ReportByDays..//PivotGrid

                TaReportViewNewCustomoseUser tavm = new TaReportViewNewCustomoseUser();
                List<TaNewUserDetails> listta_user = new List<TaNewUserDetails>();
                var months = MonthsBetween(From, To);

                var tamvm = CreateViewModel<TAReportMounthViewModel>();

                if (ReportFormat == 1)
                {
                    PrintingSystem ps1 = new PrintingSystem();
                    ps1.Document.AutoFitToPagesWidth = 1;

                    CompositeLink compositeLink1 = new CompositeLink(ps1);

                    int j = 1;
                    for (int i = 0; i < months.ToList().Count; i++)
                    {
                        var item1 = months.ToList()[i];
                        string month = Convert.ToString(item1).Split(',')[0].Replace("(", "").Trim();
                        string yr = Convert.ToString(item1).Split(',')[1].Replace(")", "").Trim();
                        string sdate = yr + "/" + month + "/01";
                        DateTime now = Convert.ToDateTime(sdate);
                        var startDate = new DateTime(now.Year, now.Month, 1);
                        int tc = months.ToList().Count;
                        if (j == tc)
                        {
                            Mapper.Map(GetTAreportsUsersNew(Convert.ToDateTime(startDate), Convert.ToDateTime(To), UserIds,building_id), tamvm.TAReportMounthItems);
                        }
                        else
                        {
                            var endDate = startDate.AddMonths(1).AddDays(-1);
                            Mapper.Map(GetTAreportsUsersNew(Convert.ToDateTime(startDate), Convert.ToDateTime(endDate), UserIds,building_id), tamvm.TAReportMounthItems);
                        }

                        PrintableComponentLink link3 = new PrintableComponentLink(ps1);
                        link3.Component = PivotGridExtension.CreatePrintableObject(GetPivotSettings(timeFormat), tamvm.TAReportMounthItems);
                        if (i == 0)
                        {
                            link3.CreateReportHeaderArea += new CreateAreaEventHandler(Link_CreateMarginalHeaderAreaInLatPivot);
                        }
                        else
                        {
                            link3.CreateReportHeaderArea += new CreateAreaEventHandler(middleLink_CreateDetailArea);
                        }

                        link3.Landscape = true;
                        compositeLink1.Links.AddRange(new object[] { link3 });
                        compositeLink1.Landscape = true;
                        compositeLink1.CreateDocument();

                        j = j + 1;
                    }
                    FileStreamResult result = CreateExcelExportResult(compositeLink1, 1, "Report");
                    ps1.Dispose();
                    return result;
                    /* pdf */
                }
                else
                {
                    Mapper.Map(GetTAreportsUsersNew(From, To, UserIds,building_id), tamvm.TAReportMounthItems);
                    if (countryflag == "EN")
                    {
                        PrintingSystem ps1 = new PrintingSystem();
                        PrintableComponentLink link3 = new PrintableComponentLink(ps1);
                        link3.Component = PivotGridExtension.CreatePrintableObject(GetPivotSettings(timeFormat), tamvm.TAReportMounthItems);
                        link3.CreateReportHeaderArea += new CreateAreaEventHandler(Link_CreateMarginalHeaderAreaPivot);
                        link3.Landscape = true;

                        CompositeLink compositeLink1 = new CompositeLink(ps1);
                        compositeLink1.Links.AddRange(new object[] { link3 });
                        compositeLink1.Landscape = true;
                        compositeLink1.CreateDocument();
                        FileStreamResult result = CreateExcelExportResult(compositeLink1, 0, "Report");
                        ps1.Dispose();
                        return result;
                    }
                    else
                    {
                        PrintingSystem ps1 = new PrintingSystem();
                        PrintableComponentLink link3 = new PrintableComponentLink(ps1);
                        link3.Component = PivotGridExtension.CreatePrintableObject(GetPivotSettings(timeFormat), tamvm.TAReportMounthItems);
                        link3.CreateReportHeaderArea += new CreateAreaEventHandler(Link_CreateMarginalHeaderAreaInLatPivot);
                        link3.Landscape = true;

                        CompositeLink compositeLink1 = new CompositeLink(ps1);
                        compositeLink1.Links.AddRange(new object[] { link3 });
                        compositeLink1.Landscape = true;
                        compositeLink1.CreateDocument();
                        FileStreamResult result = CreateExcelExportResult(compositeLink1, 0, "Report");
                        ps1.Dispose();
                        return result;
                    }
                }
            }
            else
            {                                           //Detailed Report-Old...GridView
                var tamvm = CreateViewModel<TAMsUserViewModel>();
                if (ReportFormat == 1)
                { // pdf
                    //tamvm.TAMsUserItems = GetUsersTAMoves(From, To, UserIds);
                    tamvm.TAMsUserItems = GetUsersTAMovesNew(From, To, UserIds);
                    return GridViewExtension.ExportToPdf(GetGridSettings1(timeFormat), tamvm.TAMsUserItems, GetPdfExportOptions());
                }
                else
                {
                    TaReportViewNewCustomoseUser tavm = new TaReportViewNewCustomoseUser();
                    List<TaNewUserDetails> listta_user = new List<TaNewUserDetails>();
                    int i = 0;
                    //int userId = 0;
                    DateTime f1 = Convert.ToDateTime(Session["datefrom"]);
                    DateTime f2 = Convert.ToDateTime(Session["dateto"]);
                    int buildingid = Convert.ToInt32(Session["buildingid"]);

                    List<itembuilding> Building_Data = new List<itembuilding>();
                    var ctvm = new BuildingNameViewModel();

                    if (building_id == 0)
                    {
                        if (UserIds != null && UserIds.Count > 0)
                        {
                            DateTime Ton = To.AddDays(1);
                            var result1 = (from Ta in db.NewTaMoves
                                           join U in db.User on Ta.UserId equals U.Id
                                           join C in db.Companies on U.CompanyId equals C.Id
                                           join os in db.Title on U.TitleId equals os.Id into t
                                           from rt in t.DefaultIfEmpty()
                                           where Ta.Started >= From && Ta.Finished < Ton
                                           && UserIds.Contains(Ta.UserId) && Ta.IsDeleted == false
                                           select new
                                           {
                                               companyname = C.Name,
                                               C.Comment,
                                               Ta.UserId,
                                               Ta.Remark,
                                               U.FirstName,
                                               U.LastName,
                                               U.CompanyId,
                                               U.PersonalCode,
                                               rt.Name,
                                               U.Id,
                                               Ta.Started,
                                               Ta.Finished
                                           }).OrderBy(x => x.LastName).ToList();  //OrderBy(x => x.Id).ToList();
                            if (company == 0)
                            {
                                result1 = result1.OrderBy(x => x.CompanyId).ToList();
                            }
                            int temp = 0;
                            int temp1 = 0;

                            //var comps1 = result1.Select(x => x.CompanyId).Distinct();
                            foreach (var items in result1)
                            {
                                DayOfWeek dow = items.Started.DayOfWeek;

                                //var compcount = result1.Where(x => x.CompanyId == items.CompanyId).Count();

                                DateTime date = items.Started;
                                string onlydate = date.ToString("dd/mm/yyyy");
                                DateTime st = items.Started;

                                string checkin1 = st.ToString("HH:mm");

                                DateTime fn = items.Finished;
                                string checkout1 = fn.ToString("HH:mm");

                                TimeSpan checkin = date.TimeOfDay;
                                TimeSpan checkout = fn.TimeOfDay;
                                var tottaltime = checkout.Subtract(checkin);
                                string hh = tottaltime.Hours < 10 ? "0" + tottaltime.Hours : tottaltime.Hours.ToString();
                                string mm = tottaltime.Minutes < 10 ? "0" + tottaltime.Minutes : tottaltime.Minutes.ToString();
                                string rr = hh + ":" + mm;

                                var timeDiff = tottaltime.TotalSeconds;
                                double sec = timeDiff;

                                i = i + 1;
                                int k = 0; k++; int p = 0; p++;
                                Session["FullNmae"] = items.FirstName + " " + items.LastName;
                                if (temp != items.UserId)
                                {
                                    temp = items.UserId;
                                    k = 0;
                                }

                                if (temp1 != items.CompanyId)
                                {
                                    temp1 = Convert.ToInt32(items.CompanyId);
                                    p = 0;
                                }
                                string companydeatils1 = items.companyname + "  " + items.Comment;

                                listta_user.Add(new TaNewUserDetails()
                                {
                                    id = i,
                                    UserId = items.UserId,
                                    CompnyId = Convert.ToInt32(items.CompanyId),
                                    FirstName = items.FirstName,
                                    LastName = items.LastName,
                                    companydeatils = companydeatils1,
                                    PersonalCode = items.PersonalCode,
                                    Name = items.Name,
                                    Started = items.Started,
                                    checkin = checkin1,
                                    checkout = checkout1,
                                    Hours = sec,
                                    totaltime = rr,
                                    FullName = items.FirstName + " " + items.LastName,
                                    Remark = items.Remark
                                });
                            }
                            ViewBag.Result = result1;
                            tavm.TaUserDetails = listta_user;
                        }
                    }
                    else
                    {
                        if (building_id != 0)
                        {
                            if (UserIds != null && UserIds.Count > 0)
                            {
                                DateTime Ton = To.AddDays(1);
                                var result2 = (from Ta in db.NewTaMoves
                                               join U in db.User on Ta.UserId equals U.Id
                                               join C in db.Companies on U.CompanyId equals C.Id
                                               join b in db.BuildingObject on Ta.FinishedBoId equals b.Id
                                               join os in db.Title on U.TitleId equals os.Id into t
                                               from rt in t.DefaultIfEmpty()
                                               where Ta.Started >= From && Ta.Finished < Ton
                                               && UserIds.Contains(Ta.UserId) && Ta.IsDeleted == false && b.BuildingId == building_id
                                               select new
                                               {
                                                   companyname = C.Name,
                                                   C.Comment,
                                                   Ta.UserId,
                                                   U.FirstName,
                                                   U.LastName,
                                                   U.CompanyId,
                                                   U.PersonalCode,
                                                   rt.Name,
                                                   U.Id,
                                                   Ta.Started,
                                                   Ta.Finished,
                                                   Ta.Remark
                                               }).OrderBy(x => x.LastName).ToList();           //OrderBy(x => x.Id).ToList();
                                if (company == 0)
                                {
                                    result2 = result2.OrderBy(x => x.CompanyId).ToList();
                                }

                                int temp2 = 0;
                                int temp3 = 0;

                                foreach (var items in result2)
                                {
                                    DateTime date = items.Started;
                                    string onlydate = date.ToString("dd/mm/yyyy");
                                    DateTime st = items.Started;
                                    string checkin1 = st.ToString("HH:mm");

                                    DateTime fn = items.Finished;
                                    string checkout1 = fn.ToString("HH:mm");

                                    TimeSpan checkin = date.TimeOfDay;
                                    TimeSpan checkout = fn.TimeOfDay;
                                    var tottaltime = checkout.Subtract(checkin);
                                    string hh = tottaltime.Hours < 10 ? "0" + tottaltime.Hours : tottaltime.Hours.ToString();
                                    string mm = tottaltime.Minutes < 10 ? "0" + tottaltime.Minutes : tottaltime.Minutes.ToString();
                                    string rr = hh + ":" + mm;

                                    var timeDiff = tottaltime.TotalSeconds;
                                    double sec = timeDiff;

                                    i = i + 1;
                                    int k = 0; k++; int p = 0; p++; int n = 0; n++;
                                    Session["FullNmae"] = items.FirstName + " " + items.LastName;
                                    if (temp2 != items.UserId)
                                    {
                                        temp2 = items.UserId;
                                        k = 0;
                                    }

                                    if (temp3 != items.CompanyId)
                                    {
                                        temp3 = Convert.ToInt32(items.CompanyId);
                                        p = 0;
                                    }

                                    string companydeatils1 = items.companyname + "  " + items.Comment;
                                    listta_user.Add(new TaNewUserDetails()
                                    {
                                        id = i,
                                        UserId = items.UserId,
                                        CompnyId = Convert.ToInt32(items.CompanyId),
                                        FirstName = items.FirstName,
                                        LastName = items.LastName,
                                        companydeatils = companydeatils1,
                                        PersonalCode = items.PersonalCode,
                                        Name = items.Name,
                                        Started = items.Started,
                                        checkin = checkin1,
                                        checkout = checkout1,
                                        Hours = sec,
                                        totaltime = rr,
                                        FullName = items.FirstName + " " + items.LastName,
                                        Remark = items.Remark
                                    });
                                }

                                ViewBag.Result = result2;
                                tavm.TaUserDetails = listta_user;
                            }
                        }
                    }
                    if (countryflag == "EN")
                    {
                        XlsExportOptionsEx exportOptions = new XlsExportOptionsEx();
                        exportOptions.CustomizeSheetHeader += options_CustomizeSheetHeader;
                        CustomSummaryEventArgs custsumm = new CustomSummaryEventArgs();
                        exportOptions.CustomizeCell += new DevExpress.Export.CustomizeCellEventHandler(exportOptions_CustomizeCell);
                        return GridViewExtension.ExportToXls(GetPivotSettingsNewBuildingForEnglish(timeFormat), tavm.TaUserDetails, exportOptions);
                    }
                    else
                    {
                        XlsExportOptionsEx exportOptions = new XlsExportOptionsEx();
                        exportOptions.CustomizeSheetHeader += options_CustomizeSheetHeaderInLat;
                        CustomSummaryEventArgs custsumm = new CustomSummaryEventArgs();
                        exportOptions.CustomizeCell += new DevExpress.Export.CustomizeCellEventHandler(exportOptions_CustomizeCell);
                        return GridViewExtension.ExportToXls(GetPivotSettingsNewBuilding(timeFormat), tavm.TaUserDetails, exportOptions);
                    }
                }
            }
        }
        void middleLink_CreateDetailArea(object sender, CreateAreaEventArgs e)
        {
            e.Graph.PrintingSystem.InsertPageBreak(0);
        }
        FileStreamResult CreatePdfExportResultNew(CompositeLink link)
        {
            MemoryStream stream = new MemoryStream();
            link.PrintingSystem.ExportToPdf(stream);
            stream.Position = 0;
            FileStreamResult result = new FileStreamResult(stream, "application/pdf");
            result.FileDownloadName = "PivotGrid.pdf";
            return result;
        }

        void options_CustomizeSheetHeaderInLat(DevExpress.Export.ContextEventArgs e)
        {
            // Create a new row.

            int Usrid = Convert.ToInt32(Session["User_Id"]);
            int usrrole = Convert.ToInt32(Session["Role_ID"]);
            string ufname = "";
            string Ulname = "";
            string Userdesignation1 = "";
            int Bul_id = Convert.ToInt32(Session["Buidlidngid"]);
            int companyId1 = Convert.ToInt32(Session["company"]);

            DateTime From = DateTime.ParseExact(Session["TAStartDate"].ToString(), "dd.MM.yyyy",
                                     System.Globalization.CultureInfo.InvariantCulture);
            DateTime To = DateTime.ParseExact(Session["TAStoptDate"].ToString(), "dd.MM.yyyy",
                                      System.Globalization.CultureInfo.InvariantCulture);
            /*  if (Session["Language"] == null)
              {
                  countryflag = "EN";
              }
              else
              {
                  countryflag = Session["Language"].ToString();
              }
              */
            try
            {
                string getvalidcompid = @" select  FirstName,LastName from Users    where Id={0}";
                var userLogDeatil = db.Database.SqlQuery<specificUser>(getvalidcompid, Usrid).FirstOrDefault();

                if (userLogDeatil != null)
                {
                    ufname = userLogDeatil.FirstName;
                    Ulname = userLogDeatil.LastName;
                }
                var userDesignation = db.UserRoles.Where(x => x.Id == usrrole).SingleOrDefault();
                if (userDesignation != null)
                {
                    Userdesignation1 = userDesignation.Name;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            string ReportPrepareby = ufname + " " + Ulname + "  " + "(" + Userdesignation1 + ")";
            int companyId = Convert.ToInt32(Session["company"]);
            var CompanyRecords = db.Companies.Where(x => x.Id == companyId).FirstOrDefault();
            string companyName = "", RegistraionNum = "";
            if (CompanyRecords != null)
            {
                companyName = CompanyRecords.Name;
                RegistraionNum = CompanyRecords.Comment;
                if (RegistraionNum == null)
                {
                    RegistraionNum = "NA";
                }
            }
            string bname = Session["BuildingName"].ToString();
            var ee = (from Ta in db.TABuildingName
                      join B in db.Buildings on Ta.BuildingId equals B.Id
                      where Ta.Name == bname && Ta.ValidFrom <= From && Ta.ValidTo >= To
                      select new
                      {
                          taname = Ta.Name,
                          Ta.ValidFrom,
                          Ta.ValidTo,
                          Ta.BuildingLicense,
                          Ta.CadastralNr,
                          Ta.Address,
                          B.Name
                      }).FirstOrDefault();

            if (Bul_id == 0 && companyId == 0)
            {
                CellObject row1 = new CellObject();

                row1.Value = ViewResources.SharedStrings.ReportCreated + ReportPrepareby + " " + "->" + " " + System.DateTime.Now;
                // Specify row formatting.
                XlFormattingObject row8Formatting = new XlFormattingObject();
                row1.Formatting = row8Formatting;
                row8Formatting.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Right, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
                row8Formatting.Font = new XlCellFont { Color = System.Drawing.Color.Red, Size = 12 };
                row8Formatting.BackColor = System.Drawing.Color.Lime;
                //row8Formatting.Font.Bold = true;
                e.ExportContext.AddRow(new[] { row1 });
                e.ExportContext.AddRow();
                e.ExportContext.MergeCells(new DevExpress.Export.Xl.XlCellRange(new DevExpress.Export.Xl.XlCellPosition(0, 0), new DevExpress.Export.Xl.XlCellPosition(5, 0)));
            }

            else if (Bul_id > 0 && companyId == 0)
            {
                CellObject row0 = new CellObject();
                row0.Value = ViewResources.SharedStrings.ReportCreated + ReportPrepareby + " " + "->" + " " + System.DateTime.Now;
                // Specify row formatting.
                XlFormattingObject rowFormatting0 = new XlFormattingObject();
                row0.Formatting = rowFormatting0;
                rowFormatting0.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Right, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
                rowFormatting0.Font = new XlCellFont { Color = System.Drawing.Color.Black, Size = 12 };
                rowFormatting0.Font.Bold = true;
                row0.Value = ViewResources.SharedStrings.ReportCreated + ReportPrepareby + " " + "->" + " " + System.DateTime.Now;
                //row0.Value=row0.Value + ReportPrepareby + " " + "->" + " " + System.DateTime.Now;
                CellObject row = new CellObject();

                if (ee != null)
                {
                    row.Value = "Objekta nosaukums:" + ee.taname;
                }
                else
                {
                    row.Value = ViewResources.SharedStrings.BuildingsName + ":" + "xxxx";
                }

                // Specify row formatting.
                XlFormattingObject rowFormatting = new XlFormattingObject();
                row.Formatting = rowFormatting;
                rowFormatting.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Center, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
                rowFormatting.Font = new XlCellFont { Color = System.Drawing.Color.Black, Size = 12 };
                rowFormatting.Font.Bold = true;
                // Add the created row to the output document.
                CellObject row2 = new CellObject();
                // Specify row values.
                if (ee != null)
                    row2.Value = "Addrese:" + ee.Address;
                else
                    row2.Value = ViewResources.SharedStrings.BuildingsAddress + ":" + "xxxx";

                //row2.Value = "Adrese:" + ee.Address;
                XlFormattingObject row2Formatting = new XlFormattingObject();
                row2.Formatting = row2Formatting;
                row2Formatting.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Center, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
                row2Formatting.Font = new XlCellFont { Color = System.Drawing.Color.Black, Size = 12 };
                row2Formatting.Font.Bold = true;
                CellObject row3 = new CellObject();
                if (ee != null)
                    row3.Value = "Būvatļauja:" + ee.BuildingLicense;
                else
                    row3.Value = ViewResources.SharedStrings.TaBuildingLicense + ":" + "xxxx";

                // row3.Value = "Būvatļauja:" + ee.BuildingLicense;
                XlFormattingObject row3Formatting = new XlFormattingObject();
                row3.Formatting = row3Formatting;
                row3Formatting.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Center, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
                row3Formatting.Font = new XlCellFont { Color = System.Drawing.Color.Black, Size = 12 };
                row3Formatting.Font.Bold = true;
                CellObject row4 = new CellObject();
                if (ee != null)
                    row4.Value = "Kadastra numurs:" + ee.CadastralNr;
                else
                    row4.Value = ViewResources.SharedStrings.TaBuildingCadsnrNo + ":" + "xxxx";

                XlFormattingObject row4Formatting = new XlFormattingObject();
                row4.Formatting = row4Formatting;
                row4Formatting.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Center, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
                row4Formatting.Font = new XlCellFont { Color = System.Drawing.Color.Black, Size = 12 };
                row4Formatting.Font.Bold = true;

                CellObject row5 = new CellObject();
                row5.Value = ViewResources.SharedStrings.Period + From.ToString("dd MMM yyyy") + "--" + To.ToString("dd MMM yyyy");
                XlFormattingObject row5Formatting = new XlFormattingObject();
                row5.Formatting = row5Formatting;
                row5Formatting.Font = new XlCellFont { Color = System.Drawing.Color.Black, Size = 12 };
                row5Formatting.Font.Bold = true;
                row5Formatting.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Right, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };

                e.ExportContext.AddRow(new[] { row0 });
                e.ExportContext.AddRow(new[] { row });
                e.ExportContext.AddRow(new[] { row2 });
                e.ExportContext.AddRow(new[] { row3 });
                e.ExportContext.AddRow(new[] { row4 });
                e.ExportContext.AddRow(new[] { row5 });

                // Add an empty row to the output document. Periods:
                e.ExportContext.AddRow();
                // Merge cells of two new rows.
                e.ExportContext.MergeCells(new DevExpress.Export.Xl.XlCellRange(new DevExpress.Export.Xl.XlCellPosition(0, 0), new DevExpress.Export.Xl.XlCellPosition(5, 0)));
                e.ExportContext.MergeCells(new DevExpress.Export.Xl.XlCellRange(new DevExpress.Export.Xl.XlCellPosition(0, 1), new DevExpress.Export.Xl.XlCellPosition(5, 1)));
                e.ExportContext.MergeCells(new DevExpress.Export.Xl.XlCellRange(new DevExpress.Export.Xl.XlCellPosition(0, 2), new DevExpress.Export.Xl.XlCellPosition(5, 2)));
                e.ExportContext.MergeCells(new DevExpress.Export.Xl.XlCellRange(new DevExpress.Export.Xl.XlCellPosition(0, 3), new DevExpress.Export.Xl.XlCellPosition(5, 3)));
                e.ExportContext.MergeCells(new DevExpress.Export.Xl.XlCellRange(new DevExpress.Export.Xl.XlCellPosition(0, 4), new DevExpress.Export.Xl.XlCellPosition(5, 4)));
                e.ExportContext.MergeCells(new DevExpress.Export.Xl.XlCellRange(new DevExpress.Export.Xl.XlCellPosition(0, 5), new DevExpress.Export.Xl.XlCellPosition(5, 5)));
            }
            else if (companyId1 > 0 && Bul_id == 0)
            {
                CellObject row0 = new CellObject();
                // CellObject row00 = new CellObject();
                row0.Value = ViewResources.SharedStrings.ReportCreated + ReportPrepareby + " " + "->" + " " + System.DateTime.Now;
                // row00.Value = "Atskaites periods: " + From.ToString("dd MMM yyyy") + "--" + To.ToString("dd MMM yyyy");
                // Specify row formatting.
                XlFormattingObject rowFormatting0 = new XlFormattingObject();
                row0.Formatting = rowFormatting0;
                //rowFormatting0.BackColor = System.Drawing.Color.LightGray;
                rowFormatting0.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Right, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
                rowFormatting0.Font = new XlCellFont { Color = System.Drawing.Color.Black, Size = 12 };
                rowFormatting0.Font.Bold = true;
                /* XlFormattingObject row00Formatting = new XlFormattingObject();
                 row00.Formatting = row00Formatting;
                // row00Formatting.Font = new XlCellFont { Color = System.Drawing.Color.Black, Size = 12 };
                 row00Formatting.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Right, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
                 row00Formatting.Font.Bold = true; */
                //rowFormatting0.BackColor = System.Drawing.Color.Lime;
                CellObject row01 = new CellObject();
                row01.Value = ViewResources.SharedStrings.CompanyDetails + ":" + companyName + "  " + "," + RegistraionNum;
                // Specify row formatting.
                XlFormattingObject row9Formatting = new XlFormattingObject();
                row01.Formatting = row9Formatting;
                row9Formatting.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Center, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
                // row7Formatting.Font = new XlCellFont { Color = System.Drawing.Color.Red, Size = 12 };
                row9Formatting.BackColor = System.Drawing.Color.Lime;
                e.ExportContext.AddRow(new[] { row0 });
                e.ExportContext.AddRow(new[] { row01 });
                //e.ExportContext.AddRow(new[] { row00});
                e.ExportContext.AddRow();
                // Merge cells of two new rows.
                e.ExportContext.MergeCells(new DevExpress.Export.Xl.XlCellRange(new DevExpress.Export.Xl.XlCellPosition(0, 0), new DevExpress.Export.Xl.XlCellPosition(5, 0)));
                e.ExportContext.MergeCells(new DevExpress.Export.Xl.XlCellRange(new DevExpress.Export.Xl.XlCellPosition(0, 1), new DevExpress.Export.Xl.XlCellPosition(5, 1)));
            }
            else
            {
                if (Bul_id > 0 && companyId1 > 0)
                {
                    // Specify row values.
                    CellObject row0 = new CellObject();
                    row0.Value = ViewResources.SharedStrings.ReportCreated + ReportPrepareby + " " + "->" + " " + System.DateTime.Now;
                    // Specify row formatting.
                    XlFormattingObject rowFormatting0 = new XlFormattingObject();
                    row0.Formatting = rowFormatting0;
                    rowFormatting0.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Right, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
                    rowFormatting0.Font = new XlCellFont { Color = System.Drawing.Color.Black, Size = 12 };
                    rowFormatting0.Font.Bold = true;
                    //rowFormatting0.BackColor = System.Drawing.Color.Lime;
                    CellObject row01 = new CellObject();
                    row01.Value = ViewResources.SharedStrings.CompanyDetails + ":" + companyName + "  " + "," + RegistraionNum;
                    // Specify row formatting.
                    XlFormattingObject row9Formatting = new XlFormattingObject();
                    row01.Formatting = row9Formatting;
                    row9Formatting.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Center, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
                    // row7Formatting.Font = new XlCellFont { Color = System.Drawing.Color.Red, Size = 12 };
                    row9Formatting.BackColor = System.Drawing.Color.Lime;
                    /*  if (Session["Language"] == null)
                      {
                          countryflag = "EN";
                      }
                      else
                      {
                          countryflag = Session["Language"].ToString();
                      }
                     */
                    CellObject row = new CellObject();
                    if (ee != null)
                        row.Value = "objekta nosaukums:" + ee.taname;
                    else
                        row.Value = ViewResources.SharedStrings.BuildingsName + ":" + "xxxx";
                    //row.Value = "objekta nosaukums:" + ee.taname;

                    // Specify row formatting.
                    XlFormattingObject rowFormatting = new XlFormattingObject();
                    row.Formatting = rowFormatting;
                    rowFormatting.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Center, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
                    rowFormatting.Font = new XlCellFont { Color = System.Drawing.Color.Black, Size = 12 };
                    rowFormatting.Font.Bold = true;
                    // Add the created row to the output document.

                    CellObject row2 = new CellObject();
                    // Specify row values.
                    if (ee != null)
                        row2.Value = "Adrese:" + ee.Address;
                    else
                        row2.Value = ViewResources.SharedStrings.BuildingsAddress + ":" + "xxxx";

                    XlFormattingObject row2Formatting = new XlFormattingObject();
                    row2.Formatting = row2Formatting;
                    row2Formatting.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Center, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
                    row2Formatting.Font = new XlCellFont { Color = System.Drawing.Color.Black, Size = 12 };
                    row2Formatting.Font.Bold = true;
                    CellObject row3 = new CellObject();
                    if (ee != null)
                        row3.Value = "Būvatļauja:" + ee.BuildingLicense;
                    else
                        row3.Value = ViewResources.SharedStrings.TaBuildingLicense + ":" + "xxxx";

                    XlFormattingObject row3Formatting = new XlFormattingObject();
                    row3.Formatting = row3Formatting;
                    row3Formatting.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Center, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
                    row3Formatting.Font = new XlCellFont { Color = System.Drawing.Color.Red, Size = 12 };
                    row3Formatting.Font.Bold = true;

                    CellObject row4 = new CellObject();
                    if (ee != null)
                        row4.Value = "Kadastra numurs:" + ee.CadastralNr;
                    else
                        row4.Value = ViewResources.SharedStrings.TaBuildingCadsnrNo + ":" + "xxxx";

                    XlFormattingObject row4Formatting = new XlFormattingObject();
                    row4.Formatting = row4Formatting;
                    row4Formatting.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Center, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
                    row4Formatting.Font = new XlCellFont { Color = System.Drawing.Color.Black, Size = 12 };
                    row4Formatting.Font.Bold = true;
                    CellObject row5 = new CellObject();

                    if (ee != null)
                        row5.Value = ViewResources.SharedStrings.Period + ee.ValidFrom.ToString("dd MMM yyyy") + "--" + ee.ValidTo.ToString("dd MMM yyyy");
                    else
                        row5.Value = ViewResources.SharedStrings.Period + From.ToString("dd MMM yyyy") + "--" + To.ToString("dd MMM yyyy");

                    XlFormattingObject row5Formatting = new XlFormattingObject();
                    row5.Formatting = row5Formatting;
                    row5Formatting.Font = new XlCellFont { Color = System.Drawing.Color.Black, Size = 12 };
                    row5Formatting.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Right, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
                    row5Formatting.Font.Bold = true;
                    e.ExportContext.AddRow(new[] { row0 });
                    e.ExportContext.AddRow(new[] { row01 });
                    e.ExportContext.AddRow(new[] { row });
                    e.ExportContext.AddRow(new[] { row2 });
                    e.ExportContext.AddRow(new[] { row3 });
                    e.ExportContext.AddRow(new[] { row4 });
                    e.ExportContext.AddRow(new[] { row5 });
                    // Add an empty row to the output document. Periods:
                    e.ExportContext.AddRow();
                    // Merge cells of two new rows.
                    e.ExportContext.MergeCells(new DevExpress.Export.Xl.XlCellRange(new DevExpress.Export.Xl.XlCellPosition(0, 0), new DevExpress.Export.Xl.XlCellPosition(5, 0)));
                    e.ExportContext.MergeCells(new DevExpress.Export.Xl.XlCellRange(new DevExpress.Export.Xl.XlCellPosition(0, 1), new DevExpress.Export.Xl.XlCellPosition(5, 1)));
                    e.ExportContext.MergeCells(new DevExpress.Export.Xl.XlCellRange(new DevExpress.Export.Xl.XlCellPosition(0, 2), new DevExpress.Export.Xl.XlCellPosition(5, 2)));
                    e.ExportContext.MergeCells(new DevExpress.Export.Xl.XlCellRange(new DevExpress.Export.Xl.XlCellPosition(0, 3), new DevExpress.Export.Xl.XlCellPosition(5, 3)));
                    e.ExportContext.MergeCells(new DevExpress.Export.Xl.XlCellRange(new DevExpress.Export.Xl.XlCellPosition(0, 4), new DevExpress.Export.Xl.XlCellPosition(5, 4)));
                    e.ExportContext.MergeCells(new DevExpress.Export.Xl.XlCellRange(new DevExpress.Export.Xl.XlCellPosition(0, 5), new DevExpress.Export.Xl.XlCellPosition(5, 5)));
                    e.ExportContext.MergeCells(new DevExpress.Export.Xl.XlCellRange(new DevExpress.Export.Xl.XlCellPosition(0, 6), new DevExpress.Export.Xl.XlCellPosition(5, 6)));
                }
            }

        }
        void options_CustomizeSheetHeader_test(DevExpress.Export.ContextEventArgs e)
        {
            // Create a new row.
            int Usrid = Convert.ToInt32(Session["User_Id"]);
            int usrrole = Convert.ToInt32(Session["Role_ID"]);
            string ufname = "";
            string Ulname = "";
            string Userdesignation1 = "";
            string Dept_Name = "";
            int Bul_id = Convert.ToInt32(Session["Buidlidngid"]);
            int companyId1 = Convert.ToInt32(Session["company"]);
            int department = Convert.ToInt32(Session["department"]);
            if (department != 0)
            {
                var dept = db.Departments.SingleOrDefault(x => x.Id == department);
                Dept_Name = dept.Name;
                if (companyId1 == 0)
                {
                    companyId1 = dept.CompanyId;
                }
            }
            try
            {
                string getvalidcompid = @" select  FirstName,LastName from Users    where Id={0}";
                var userLogDeatil = db.Database.SqlQuery<specificUser>(getvalidcompid, Usrid).FirstOrDefault();
                if (userLogDeatil != null)
                {
                    ufname = userLogDeatil.FirstName;
                    Ulname = userLogDeatil.LastName;
                }
                var userDesignation = db.UserRoles.Where(x => x.Id == usrrole).SingleOrDefault();
                if (userDesignation != null)
                {
                    Userdesignation1 = userDesignation.Name;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            string ReportPrepareby = ufname + " " + Ulname + "  " + "(" + Userdesignation1 + ")";
            List<List<CellObject>> rowList = new List<List<CellObject>>();
            XlFormattingObject Header_rowFormatting = new XlFormattingObject();
            Header_rowFormatting.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Right, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
            Header_rowFormatting.Font = new XlCellFont { Color = System.Drawing.Color.Black, Size = 11 };
            Header_rowFormatting.Font.Bold = true;
            XlFormattingObject format2 = new XlFormattingObject();
            format2.Alignment = new DevExpress.Export.Xl.XlCellAlignment { HorizontalAlignment = DevExpress.Export.Xl.XlHorizontalAlignment.Left, VerticalAlignment = DevExpress.Export.Xl.XlVerticalAlignment.Top };
            format2.Font = new XlCellFont { Color = System.Drawing.Color.Black, Size = 11 };
            format2.Font.Bold = false;
            List<CellObject> _row1 = new List<CellObject>();
            CellObject emptyCells = new CellObject();//empty cells
            emptyCells.Formatting = Header_rowFormatting;
            CellObject D1 = new CellObject();
            if (countryflag == "EN")
            {
                D1.Value = "Report Created:";
            }
            else
            {
                D1.Value = ViewResources.SharedStrings.ReportCreated;
            }
            D1.Formatting = Header_rowFormatting;
            CellObject E1 = new CellObject();
            E1.Value = ReportPrepareby + " " + "->" + " " + System.DateTime.Now;
            E1.Formatting = format2;
            // _row1.Add(emptyCells);
            _row1.Add(emptyCells);
            _row1.Add(emptyCells);
            _row1.Add(D1);
            _row1.Add(E1);
            rowList.Add(_row1);
            DateTime From = DateTime.ParseExact(Session["TAStartDate"].ToString(), "dd.MM.yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
            DateTime To = DateTime.ParseExact(Session["TAStoptDate"].ToString(), "dd.MM.yyyy",
                                      System.Globalization.CultureInfo.InvariantCulture);
            //int companyId = Convert.ToInt32(Session["company"]);

            var CompanyRecords = db.Companies.Where(x => x.Id == companyId1).FirstOrDefault();
            string companyName = "", RegistraionNum = "";
            if (CompanyRecords != null)
            {
                companyName = CompanyRecords.Name;
                RegistraionNum = CompanyRecords.Comment;
                if (RegistraionNum == null)
                {
                    RegistraionNum = "";
                }

            }
            string bname = Session["BuildingName"].ToString();
            var ee = (from Ta in db.TABuildingName
                      join B in db.Buildings on Ta.BuildingId equals B.Id
                      where Ta.Name == bname && Ta.ValidFrom <= From && Ta.ValidTo >= To
                      select new
                      {
                          taname = Ta.Name,
                          Ta.ValidFrom,
                          Ta.ValidTo,
                          Ta.BuildingLicense,
                          Ta.CadastralNr,
                          Ta.Address,
                          B.Name,
                      }).FirstOrDefault();

            if (companyId1 > 0)
            {
                List<CellObject> _row = new List<CellObject>();
                CellObject D = new CellObject();
                CellObject E = new CellObject();
                if (countryflag == "EN")
                {
                    D.Value = "Company Details :";
                }
                else
                {
                    D.Value = ViewResources.SharedStrings.CompanyDetails + ":";
                }
                if (!String.IsNullOrEmpty(RegistraionNum))
                {
                    E.Value = companyName + "  " + "," + RegistraionNum;
                }
                else
                {
                    E.Value = companyName;
                }
                D.Formatting = Header_rowFormatting;
                E.Formatting = format2;
                //_row.Add(emptyCells);
                _row.Add(emptyCells);
                _row.Add(emptyCells);
                _row.Add(D);
                _row.Add(E);
                rowList.Add(_row);
            }

            if (department > 0)
            {
                List<CellObject> _row = new List<CellObject>();
                CellObject D = new CellObject();
                CellObject E = new CellObject();
                if (countryflag == "EN")
                {
                    D.Value = "Department :";
                }
                else
                {
                    D.Value = ViewResources.SharedStrings.UsersDepartment + ":";
                }
                E.Value = Dept_Name;
                D.Formatting = Header_rowFormatting;
                E.Formatting = format2;
                //_row.Add(emptyCells);
                _row.Add(emptyCells);
                _row.Add(emptyCells);
                _row.Add(D);
                _row.Add(E);
                rowList.Add(_row);
            }

            if (Bul_id > 0)
            {
                List<CellObject> _row2 = new List<CellObject>();
                CellObject D2 = new CellObject();
                CellObject E2 = new CellObject();
                if (countryflag == "EN")
                {
                    D2.Value = "Building Name:";
                }
                else
                {
                    D2.Value = ViewResources.SharedStrings.BuildingsName + ":";
                }
                if (!String.IsNullOrEmpty(ee.taname))
                {
                    E2.Value = ee.taname;
                }
                else
                {
                    E2.Value = "";
                }
                D2.Formatting = Header_rowFormatting;
                E2.Formatting = format2;
                //_row2.Add(emptyCells);
                _row2.Add(emptyCells);
                _row2.Add(emptyCells);
                _row2.Add(D2);
                _row2.Add(E2);
                rowList.Add(_row2);
                List<CellObject> _row3 = new List<CellObject>();
                CellObject D3 = new CellObject();
                CellObject E3 = new CellObject();
                // Specify row values.
                if (countryflag == "EN")
                {
                    D3.Value = "Address:";
                }
                else
                {
                    D3.Value = ViewResources.SharedStrings.BuildingsAddress + ":";
                }
                D3.Formatting = Header_rowFormatting;
                E3.Value = ee.Address;
                E3.Formatting = format2;
                //_row3.Add(emptyCells);
                _row3.Add(emptyCells);
                _row3.Add(emptyCells);
                _row3.Add(D3);
                _row3.Add(E3);
                rowList.Add(_row3);

                List<CellObject> _row4 = new List<CellObject>();
                CellObject D4 = new CellObject();
                CellObject E4 = new CellObject();

                // Specify row values.
                if (countryflag == "EN")
                {
                    D4.Value = "Building License:";
                }
                else
                {
                    D4.Value = ViewResources.SharedStrings.TaBuildingLicense + ":";
                }
                D4.Formatting = Header_rowFormatting;
                E4.Value = ee.BuildingLicense;
                E4.Formatting = format2;
                //_row4.Add(emptyCells);
                _row4.Add(emptyCells);
                _row4.Add(emptyCells);
                _row4.Add(D4);
                _row4.Add(E4);
                rowList.Add(_row4);
                List<CellObject> _row5 = new List<CellObject>();
                CellObject D5 = new CellObject();
                CellObject E5 = new CellObject();

                // Specify row values.
                if (countryflag == "EN")
                {
                    D5.Value = "Cadastre number:";
                }
                else
                {
                    D5.Value = ViewResources.SharedStrings.TaBuildingCadsnrNo + ":";
                }
                D5.Formatting = Header_rowFormatting;
                E5.Value = ee.CadastralNr;
                E5.Formatting = format2;
                //_row5.Add(emptyCells);
                _row5.Add(emptyCells);
                _row5.Add(emptyCells);
                _row5.Add(D5);
                _row5.Add(E5);
                rowList.Add(_row5);
            }

            List<CellObject> _row6 = new List<CellObject>();
            CellObject D6 = new CellObject();
            CellObject E6 = new CellObject();

            // Specify row values.
            if (countryflag == "EN")
            {
                D6.Value = "Period:";
            }
            else
            {
                D6.Value = ViewResources.SharedStrings.Period;
            }
            D6.Formatting = Header_rowFormatting;
            E6.Value = From.ToString("dd MMM yyyy") + "--" + To.ToString("dd MMM yyyy");
            E6.Formatting = format2;
            //_row6.Add(emptyCells);
            _row6.Add(emptyCells);
            _row6.Add(emptyCells);
            _row6.Add(D6);
            _row6.Add(E6);
            rowList.Add(_row6);
            foreach (var r in rowList)
            {
                CellObject[] co = r.ToArray();
                e.ExportContext.AddRow(co);
            }
            e.ExportContext.AddRow();
            for (int i = 0; i < rowList.Count; i++)
            {
                e.ExportContext.MergeCells(new DevExpress.Export.Xl.XlCellRange(new DevExpress.Export.Xl.XlCellPosition(3, i), new DevExpress.Export.Xl.XlCellPosition(6, i)));
            }
        }

        protected FileStreamResult CreateExcelExportResult(CompositeLink link, int ExtType, string Reptype)
        {
            string fileNm = "";
            string contenttype = "", ext = ".xls";
            //string contenttype = "", ext = ".xlx";

            MemoryStream stream = new MemoryStream();
            link.CreateDocument(false);
            BrickEnumerator brickEnum = link.PrintingSystem.Document.Pages[0].GetEnumerator();
            float maxWidth = 0;
            while (brickEnum.MoveNext())
                maxWidth = maxWidth > brickEnum.CurrentBrick.Rect.X + brickEnum.CurrentBrick.Rect.Width ? maxWidth : brickEnum.CurrentBrick.Rect.X + brickEnum.CurrentBrick.Rect.Width;
            float freeSpace = (link.PrintingSystem.Document.Pages[0]).PageSize.Width - maxWidth;
            if (freeSpace > 600)
            {
                link.Margins.Left = (int)(freeSpace / 6);
                link.CreateDocument(false);
            }

            if (ExtType == 0)
            {
                link.PrintingSystem.ExportToXls(stream);
                contenttype = "application/xls";
                ext = ".xls";
            }
            else
            {
                link.PrintingSystem.ExportToPdf(stream);
                contenttype = "application/pdf";
                ext = ".pdf";
            }

            stream.Position = 0;
            FileStreamResult result = new FileStreamResult(stream, contenttype);
            fileNm = CreateFileName(Reptype);
            result.FileDownloadName = fileNm + ext;
            return result;
        }


        MemoryStream stream1 = new MemoryStream();
        protected FileStreamResult CreatePDFExportResult(CompositeLink link, int ExtType, string Reptype)
        {
            string fileNm = "";
            string contenttype = "", ext = ".pdf";
            //string contenttype = "", ext = ".xlx";

            link.CreateDocument(false);
            BrickEnumerator brickEnum = link.PrintingSystem.Document.Pages[0].GetEnumerator();
            float maxWidth = 0;
            while (brickEnum.MoveNext())
                maxWidth = maxWidth > brickEnum.CurrentBrick.Rect.X + brickEnum.CurrentBrick.Rect.Width ? maxWidth : brickEnum.CurrentBrick.Rect.X + brickEnum.CurrentBrick.Rect.Width;
            float freeSpace = (link.PrintingSystem.Document.Pages[0]).PageSize.Width - maxWidth;
            if (freeSpace > 600)
            {
                link.Margins.Left = (int)(freeSpace / 6);
                link.CreateDocument(false);
            }

            link.PrintingSystem.ExportToPdf(stream1);
            contenttype = "application/pdf";
            ext = ".pdf";

            stream1.Position = 0;
            FileStreamResult result = new FileStreamResult(stream1, contenttype);
            fileNm = CreateFileName(Reptype);
            result.FileDownloadName = fileNm + ext;
            return result;
        }

        public string CreateFileName(string type)
        {
            string fileNm = "";
            string StartDate = "";
            string StoptDate = "";
            if (type == "TAReport1")
            {
                StartDate = (string)Session["TAStartDate1"];
                StoptDate = (string)Session["TAStoptDate1"];
            }
            else
            {
                StartDate = (string)Session["TAStartDate"];
                StoptDate = (string)Session["TAStoptDate"];
            }

            string dep = "", comp = "";
            int department = Convert.ToInt32(Session["department"]);
            if (department != 0)
            {
                dep = db.Departments.SingleOrDefault(x => x.Id == department).Name;
            }
            int company = Convert.ToInt32(Session["company"]);
            if (company != 0)
            {
                comp = db.Companies.SingleOrDefault(x => x.Id == company).Name;
            }

            DateTime From = DateTime.ParseExact(StartDate, "dd.MM.yyyy",
                                                   System.Globalization.CultureInfo.InvariantCulture);
            DateTime To = DateTime.ParseExact(StoptDate, "dd.MM.yyyy",
                                      System.Globalization.CultureInfo.InvariantCulture);
            StringBuilder sb = new StringBuilder();
            if (CurrentUser.Get().IsSuperAdmin)
            {
                sb.Append(string.Format("SA_TA_" + type + "_{0}", !string.IsNullOrEmpty(comp) ? !string.IsNullOrEmpty(dep) ? comp + "_" + dep + "_" : comp + "_" : comp.Trim()));
            }
            if (CurrentUser.Get().IsCompanyManager)
            {
                sb.Append(string.Format("CM_TA_" + type + "_{0}", !string.IsNullOrEmpty(comp) ? !string.IsNullOrEmpty(dep) ? comp + "_" + dep + "_" : comp + "_" : null));
            }
            if (CurrentUser.Get().IsDepartmentManager)
            {
                sb.Append(string.Format("DM_TA_" + type + "_{0}", dep + "_"));
            }
            fileNm = sb.ToString() + From.ToString("ddMMyy") + "_to_" + To.ToString("ddMMyy");

            return fileNm;
        }
        private void Link_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string StartDate = (string)Session["TAStartDate"];
            string StoptDate = (string)Session["TAStoptDate"];
            int Usrid = Convert.ToInt32(Session["User_Id"]);
            int usrrole = Convert.ToInt32(Session["Role_ID"]);
            string ufname = "";
            string Ulname = "";
            string Userdesignation1 = "";
            int department = Convert.ToInt32(Session["department"]);
            int company = Convert.ToInt32(Session["company"]);
            try
            {
                string getvalidcompid = @" select  FirstName,LastName from Users    where Id={0}";
                var userLogDeatil = db.Database.SqlQuery<specificUser>(getvalidcompid, Usrid).FirstOrDefault();

                if (userLogDeatil != null)
                {
                    ufname = userLogDeatil.FirstName;
                    Ulname = userLogDeatil.LastName;
                }
                var userDesignation = db.UserRoles.Where(x => x.Id == usrrole).SingleOrDefault();
                if (userDesignation != null)
                {
                    Userdesignation1 = userDesignation.Name;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (department != 0)
            {
                var dept = db.Departments.SingleOrDefault(x => x.Id == department);
                depname = dept.Name;
                if (company == 0)
                {
                    company = dept.CompanyId;
                }
            }

            DateTime From = DateTime.ParseExact(StartDate, "dd.MM.yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
            DateTime To = DateTime.ParseExact(StoptDate, "dd.MM.yyyy",
                                      System.Globalization.CultureInfo.InvariantCulture);

            string bname = Session["BuildingName"].ToString();
            var ee = (from Ta in db.TABuildingName
                      join B in db.Buildings on Ta.BuildingId equals B.Id
                      where Ta.Name == bname && Ta.ValidFrom <= From && Ta.ValidTo >= To
                      select new
                      {
                          taname = Ta.Name,
                          Ta.ValidFrom,
                          Ta.ValidTo,
                          Ta.BuildingLicense,
                          Ta.CadastralNr,
                          Ta.Address,
                          B.Name,
                      }).FirstOrDefault();

            string ReportPrepareby = ufname + " " + Ulname + "  " + "(" + Userdesignation1 + ")";
            var qry = db.Companies.Where(x => x.Id == company).SingleOrDefault();
            if (qry != null)
            {
                cname = qry.Name;
            }
            //var qry1 = db.Departments.Where(x => x.Id == department).SingleOrDefault();

            //if (qry1 != null)
            //{
            //    depname = qry1.Name;
            //}

            //settings.SettingsExport.ReportHeader = "Hare Krishna!!!!";
            string pageHeader = "";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("Report Created: {0} ", ReportPrepareby));
            sb.AppendLine(string.Format("Report Creation Date : {0} ", DateTime.Now.Date.ToString("dd.MM.yyyy")));
            if (company != 0)
            {
                sb.AppendLine(string.Format("Company: {0} ", cname));
            }
            if (department != 0)
            {
                sb.AppendLine(string.Format("Department: {0} ", depname));
            }
            if (ee != null)
            {
                if (!String.IsNullOrEmpty(ee.Name))
                    sb.AppendLine(string.Format("Building Name: {0} ", ee.Name));
                else
                    sb.AppendLine(string.Format("Building Name:xxxx"));

                if (!String.IsNullOrEmpty(ee.Address))
                    sb.AppendLine(string.Format("Address: {0} ", ee.Address));
                else
                    sb.AppendLine(string.Format("Address:xxxx"));

                if (!String.IsNullOrEmpty(ee.BuildingLicense))
                    sb.AppendLine(string.Format("Building License: {0} ", ee.BuildingLicense));
                else
                    sb.AppendLine(string.Format("Building License:xxxx"));

                if (!String.IsNullOrEmpty(ee.CadastralNr))
                    sb.AppendLine(string.Format("Cadastre number: {0} ", ee.CadastralNr));
                else
                    sb.AppendLine(string.Format("Cadastre number:xxxx"));
            }
            else
            {
                sb.AppendLine(string.Format("Building Name: All Buildings"));
                /* sb.AppendLine(string.Format("Address: {0} ", ee.Address));
                 sb.AppendLine(string.Format("Building License: {0} ", ee.BuildingLicense));
                 sb.AppendLine(string.Format("Cadastre number: {0} ", ee.CadastralNr));
                 */
            }

            sb.AppendLine(" ");
            sb.AppendLine(string.Format("Period: {0} ", StartDate + " - " + StoptDate));
            sb.AppendLine(" ");
            pageHeader = sb.ToString();
            DevExpress.XtraPrinting.TextBrick brick;
            SizeF textSize = e.Graph.MeasureString(pageHeader, 600, StringFormat.GenericDefault);
            brick = e.Graph.DrawString(pageHeader, System.Drawing.Color.Black, new RectangleF(0, 0, 600, textSize.Height + 50), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new System.Drawing.Font("Calibri", 10, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Center);
            brick.VertAlignment = DevExpress.Utils.VertAlignment.Center;
            brick.HorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            //      PageInfoBrick brick = e.Graph.DrawPageInfo(PageInfo.DateTime, "", System.Drawing.Color.DarkBlue,

            //new RectangleF(0, 0, 100, 20), BorderSide.None);

            //      brick.LineAlignment = BrickAlignment.Center;
            //      brick.HorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            //      brick.VertAlignment= DevExpress.Utils.VertAlignment.Center;

            //      brick.Alignment = BrickAlignment.Center;

            //      brick.AutoWidth = true;
        }

        private void Link_CreateMarginalHeaderAreaPivot(object sender, CreateAreaEventArgs e)
        {
            string StartDate = (string)Session["TAStartDate"];
            string StoptDate = (string)Session["TAStoptDate"];
            int Usrid = Convert.ToInt32(Session["User_Id"]);
            int usrrole = Convert.ToInt32(Session["Role_ID"]);
            string ufname = "";
            string Ulname = "";
            string Userdesignation1 = "";
            int department = Convert.ToInt32(Session["department"]);
            int company = Convert.ToInt32(Session["company"]);
            try
            {
                string getvalidcompid = @" select  FirstName,LastName from Users    where Id={0}";
                var userLogDeatil = db.Database.SqlQuery<specificUser>(getvalidcompid, Usrid).FirstOrDefault();

                if (userLogDeatil != null)
                {
                    ufname = userLogDeatil.FirstName;
                    Ulname = userLogDeatil.LastName;
                }
                var userDesignation = db.UserRoles.Where(x => x.Id == usrrole).SingleOrDefault();
                if (userDesignation != null)
                {
                    Userdesignation1 = userDesignation.Name;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (department != 0)
            {
                var dept = db.Departments.SingleOrDefault(x => x.Id == department);
                depname = dept.Name;
                if (company == 0)
                {
                    company = dept.CompanyId;
                }
            }

            DateTime From = DateTime.ParseExact(StartDate, "dd.MM.yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
            DateTime To = DateTime.ParseExact(StoptDate, "dd.MM.yyyy",
                                      System.Globalization.CultureInfo.InvariantCulture);

            string bname = Session["BuildingName"].ToString();
            var ee = (from Ta in db.TABuildingName
                      join B in db.Buildings on Ta.BuildingId equals B.Id
                      where Ta.Name == bname && Ta.ValidFrom <= From && Ta.ValidTo >= To
                      select new
                      {
                          taname = Ta.Name,
                          Ta.ValidFrom,
                          Ta.ValidTo,
                          Ta.BuildingLicense,
                          Ta.CadastralNr,
                          Ta.Address,
                          B.Name,
                      }).FirstOrDefault();

            string ReportPrepareby = ufname + " " + Ulname + "  " + "(" + Userdesignation1 + ")";
            var qry = db.Companies.Where(x => x.Id == company).SingleOrDefault();
            if (qry != null)
            {
                cname = qry.Name;
            }
            //var qry1 = db.Departments.Where(x => x.Id == department).SingleOrDefault();

            //if (qry1 != null)
            //{
            //    depname = qry1.Name;
            //}

            //settings.SettingsExport.ReportHeader = "Hare Krishna!!!!";
            string pageHeader = "";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("Report Created: {0} ", ReportPrepareby));
            sb.AppendLine(string.Format("Report Creation Date : {0} ", DateTime.Now.Date.ToString("dd.MM.yyyy")));
            if (company != 0)
            {
                sb.AppendLine(string.Format("Company: {0} ", cname));
            }
            if (department != 0)
            {
                sb.AppendLine(string.Format("Department: {0} ", depname));
            }
            if (ee != null)
            {
                if (!String.IsNullOrEmpty(ee.Name))
                    sb.AppendLine(string.Format("Building Name: {0} ", ee.Name));
                else
                    sb.AppendLine(string.Format("Building Name:"));

                if (!String.IsNullOrEmpty(ee.Address))
                    sb.AppendLine(string.Format("Address: {0} ", ee.Address));
                else
                    sb.AppendLine(string.Format("Address:xxxx"));

                if (!String.IsNullOrEmpty(ee.BuildingLicense))
                    sb.AppendLine(string.Format("Building License: {0} ", ee.BuildingLicense));
                else
                    sb.AppendLine(string.Format("Building License:xxxx"));

                if (!String.IsNullOrEmpty(ee.CadastralNr))
                    sb.AppendLine(string.Format("Cadastre number: {0} ", ee.CadastralNr));
                else
                    sb.AppendLine(string.Format("Cadastre number:xxxx"));
            }
            else
            {
                sb.AppendLine(string.Format("Building Name: All Buildings"));
                /* sb.AppendLine(string.Format("Address: {0} ", ee.Address));
                 sb.AppendLine(string.Format("Building License: {0} ", ee.BuildingLicense));
                 sb.AppendLine(string.Format("Cadastre number: {0} ", ee.CadastralNr));
                 */
            }

            sb.AppendLine(" ");
            sb.AppendLine(string.Format("Period: {0} ", StartDate + " - " + StoptDate));
            sb.AppendLine(" ");
            pageHeader = sb.ToString();
            DevExpress.XtraPrinting.TextBrick brick;
            SizeF textSize = e.Graph.MeasureString(pageHeader, 1800, StringFormat.GenericDefault);
            brick = e.Graph.DrawString(pageHeader, System.Drawing.Color.Black, new RectangleF(0, 0, 1800, textSize.Height + 50), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new System.Drawing.Font("Calibri", 10, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Center);
            brick.VertAlignment = DevExpress.Utils.VertAlignment.Center;
            brick.HorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            //      PageInfoBrick brick = e.Graph.DrawPageInfo(PageInfo.DateTime, "", System.Drawing.Color.DarkBlue,

            //new RectangleF(0, 0, 100, 20), BorderSide.None);

            //      brick.LineAlignment = BrickAlignment.Center;
            //      brick.HorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            //      brick.VertAlignment= DevExpress.Utils.VertAlignment.Center;

            //      brick.Alignment = BrickAlignment.Center;

            //      brick.AutoWidth = true;
        }

        private void Link_CreateMarginalHeaderAreaInLatPivot(object sender, CreateAreaEventArgs e)
        {
            string StartDate = (string)Session["TAStartDate"];
            string StoptDate = (string)Session["TAStoptDate"];
            int Usrid = Convert.ToInt32(Session["User_Id"]);
            int usrrole = Convert.ToInt32(Session["Role_ID"]);
            string ufname = "";
            string Ulname = "";
            string Userdesignation1 = "";
            int department = Convert.ToInt32(Session["department"]);
            int company = Convert.ToInt32(Session["company"]);
            try
            {
                string getvalidcompid = @" select  FirstName,LastName from Users    where Id={0}";
                var userLogDeatil = db.Database.SqlQuery<specificUser>(getvalidcompid, Usrid).FirstOrDefault();
                if (userLogDeatil != null)
                {
                    ufname = userLogDeatil.FirstName;
                    Ulname = userLogDeatil.LastName;
                }
                var userDesignation = db.UserRoles.Where(x => x.Id == usrrole).SingleOrDefault();
                if (userDesignation != null)
                {
                    Userdesignation1 = userDesignation.Name;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (department != 0)
            {
                var dept = db.Departments.SingleOrDefault(x => x.Id == department);
                depname = dept.Name;
                if (company == 0)
                {
                    company = dept.CompanyId;
                }
            }

            DateTime From = DateTime.ParseExact(StartDate, "dd.MM.yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
            DateTime To = DateTime.ParseExact(StoptDate, "dd.MM.yyyy",
                                      System.Globalization.CultureInfo.InvariantCulture);

            string bname = Session["BuildingName"].ToString();
            var ee = (from Ta in db.TABuildingName
                      join B in db.Buildings on Ta.BuildingId equals B.Id
                      where Ta.Name == bname && Ta.ValidFrom <= From && Ta.ValidTo >= To

                      select new
                      {
                          taname = Ta.Name,
                          Ta.ValidFrom,
                          Ta.ValidTo,
                          Ta.BuildingLicense,
                          Ta.CadastralNr,
                          Ta.Address,
                          B.Name,
                      }).FirstOrDefault();

            string ReportPrepareby = ufname + " " + Ulname + "  " + "(" + Userdesignation1 + ")";
            var qry = db.Companies.Where(x => x.Id == company).SingleOrDefault();
            if (qry != null)
            {
                cname = qry.Name;
            }
            //var qry1 = db.Departments.Where(x => x.Id == department).SingleOrDefault();

            //if (qry1 != null)
            //{
            //    depname = qry1.Name;
            //}

            //settings.SettingsExport.ReportHeader = "Hare Krishna!!!!";
            string pageHeader = "";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format(ViewResources.SharedStrings.ReportCreated + "{0} ", ReportPrepareby));//Report Created By
            sb.AppendLine(string.Format(ViewResources.SharedStrings.ReportCreatedOn + " {0} ", DateTime.Now.Date.ToString("dd.MM.yyyy"))); //Report Creation
            if (company != 0)
            {
                sb.AppendLine(string.Format(ViewResources.SharedStrings.CompanyDetails + ": {0} ", cname)); //companyName
            }
            if (department != 0)
            {
                sb.AppendLine(string.Format(ViewResources.SharedStrings.UsersDepartment + ": {0} ", depname)); //departmentName
            }
            if (ee != null)
            {
                if (!String.IsNullOrEmpty(ee.Name))
                    sb.AppendLine(string.Format(ViewResources.SharedStrings.BuildingsName + " : {0} ", ee.Name));//BuildingName
                else
                    sb.AppendLine(string.Format(ViewResources.SharedStrings.BuildingsName + ":"));

                if (!String.IsNullOrEmpty(ee.Address))
                    sb.AppendLine(string.Format(ViewResources.SharedStrings.BuildingsAddress + ": {0} ", ee.Address));//Address
                else
                    sb.AppendLine(string.Format(ViewResources.SharedStrings.BuildingsAddress + ":xxxx"));

                if (!String.IsNullOrEmpty(ee.BuildingLicense))
                    sb.AppendLine(string.Format(ViewResources.SharedStrings.TaBuildingLicense + ": {0} ", ee.BuildingLicense));//Building License
                else
                    sb.AppendLine(string.Format(ViewResources.SharedStrings.TaBuildingLicense + ":xxxx"));

                if (!String.IsNullOrEmpty(ee.CadastralNr))
                    sb.AppendLine(string.Format(ViewResources.SharedStrings.TaBuildingCadsnrNo + ": {0} ", ee.CadastralNr)); //Bulding Cadastral number
                else
                    sb.AppendLine(string.Format(ViewResources.SharedStrings.TaBuildingCadsnrNo + ":xxxx"));
            }
            else
            {
                sb.AppendLine(string.Format(ViewResources.SharedStrings.BuildingsName + ": All Buildings"));
            }
            sb.AppendLine(" ");
            sb.AppendLine(string.Format(ViewResources.SharedStrings.Period + " {0} ", StartDate + " - " + StoptDate));       //Period
            sb.AppendLine(" ");
            pageHeader = sb.ToString();
            DevExpress.XtraPrinting.TextBrick brick;
            SizeF textSize = e.Graph.MeasureString(pageHeader, 1800, StringFormat.GenericDefault);
            brick = e.Graph.DrawString(pageHeader, System.Drawing.Color.Black, new RectangleF(0, 0, 1800, textSize.Height + 50), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new System.Drawing.Font("Calibri", 10, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Center);
            brick.VertAlignment = DevExpress.Utils.VertAlignment.Center;
            brick.HorzAlignment = DevExpress.Utils.HorzAlignment.Center;
        }

        private void Link_CreateMarginalHeaderAreaInLat(object sender, CreateAreaEventArgs e)
        {
            string StartDate = (string)Session["TAStartDate"];
            string StoptDate = (string)Session["TAStoptDate"];
            int Usrid = Convert.ToInt32(Session["User_Id"]);
            int usrrole = Convert.ToInt32(Session["Role_ID"]);
            string ufname = "";
            string Ulname = "";
            string Userdesignation1 = "";
            int department = Convert.ToInt32(Session["department"]);
            int company = Convert.ToInt32(Session["company"]);
            try
            {
                string getvalidcompid = @" select  FirstName,LastName from Users    where Id={0}";
                var userLogDeatil = db.Database.SqlQuery<specificUser>(getvalidcompid, Usrid).FirstOrDefault();
                if (userLogDeatil != null)
                {
                    ufname = userLogDeatil.FirstName;
                    Ulname = userLogDeatil.LastName;
                }
                var userDesignation = db.UserRoles.Where(x => x.Id == usrrole).SingleOrDefault();
                if (userDesignation != null)
                {
                    Userdesignation1 = userDesignation.Name;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (department != 0)
            {
                var dept = db.Departments.SingleOrDefault(x => x.Id == department);
                depname = dept.Name;
                if (company == 0)
                {
                    company = dept.CompanyId;
                }
            }

            DateTime From = DateTime.ParseExact(StartDate, "dd.MM.yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
            DateTime To = DateTime.ParseExact(StoptDate, "dd.MM.yyyy",
                                      System.Globalization.CultureInfo.InvariantCulture);

            string bname = Session["BuildingName"].ToString();
            var ee = (from Ta in db.TABuildingName
                      join B in db.Buildings on Ta.BuildingId equals B.Id
                      where Ta.Name == bname && Ta.ValidFrom <= From && Ta.ValidTo >= To

                      select new
                      {
                          taname = Ta.Name,
                          Ta.ValidFrom,
                          Ta.ValidTo,
                          Ta.BuildingLicense,
                          Ta.CadastralNr,
                          Ta.Address,
                          B.Name,
                      }).FirstOrDefault();

            string ReportPrepareby = ufname + " " + Ulname + "  " + "(" + Userdesignation1 + ")";
            var qry = db.Companies.Where(x => x.Id == company).SingleOrDefault();
            if (qry != null)
            {
                cname = qry.Name;
            }
            //var qry1 = db.Departments.Where(x => x.Id == department).SingleOrDefault();

            //if (qry1 != null)
            //{
            //    depname = qry1.Name;
            //}

            //settings.SettingsExport.ReportHeader = "Hare Krishna!!!!";
            string pageHeader = "";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format(ViewResources.SharedStrings.ReportCreated + "{0} ", ReportPrepareby));//Report Created By
            sb.AppendLine(string.Format(ViewResources.SharedStrings.ReportCreatedOn + " {0} ", DateTime.Now.Date.ToString("dd.MM.yyyy"))); //Report Creation
            if (company != 0)
            {
                sb.AppendLine(string.Format(ViewResources.SharedStrings.CompanyDetails + ": {0} ", cname)); //companyName
            }
            if (department != 0)
            {
                sb.AppendLine(string.Format(ViewResources.SharedStrings.UsersDepartment + ": {0} ", depname)); //departmentName
            }
            if (ee != null)
            {
                if (!String.IsNullOrEmpty(ee.Name))
                    sb.AppendLine(string.Format(ViewResources.SharedStrings.BuildingsName + " : {0} ", ee.Name));//BuildingName
                else
                    sb.AppendLine(string.Format(ViewResources.SharedStrings.BuildingsName + ":"));

                if (!String.IsNullOrEmpty(ee.Address))
                    sb.AppendLine(string.Format(ViewResources.SharedStrings.BuildingsAddress + ": {0} ", ee.Address));//Address
                else
                    sb.AppendLine(string.Format(ViewResources.SharedStrings.BuildingsAddress + ":xxxx"));

                if (!String.IsNullOrEmpty(ee.BuildingLicense))
                    sb.AppendLine(string.Format(ViewResources.SharedStrings.TaBuildingLicense + ": {0} ", ee.BuildingLicense));//Building License
                else
                    sb.AppendLine(string.Format(ViewResources.SharedStrings.TaBuildingLicense + ":xxxx"));

                if (!String.IsNullOrEmpty(ee.CadastralNr))
                    sb.AppendLine(string.Format(ViewResources.SharedStrings.TaBuildingCadsnrNo + ": {0} ", ee.CadastralNr)); //Bulding Cadastral number
                else
                    sb.AppendLine(string.Format(ViewResources.SharedStrings.TaBuildingCadsnrNo + ":xxxx"));
            }
            else
            {
                sb.AppendLine(string.Format(ViewResources.SharedStrings.BuildingsName + ":  All Buildings"));
            }
            sb.AppendLine(" ");
            sb.AppendLine(string.Format(ViewResources.SharedStrings.Period + " {0} ", StartDate + " - " + StoptDate));       //Period
            sb.AppendLine(" ");
            pageHeader = sb.ToString();
            DevExpress.XtraPrinting.TextBrick brick;
            SizeF textSize = e.Graph.MeasureString(pageHeader, 600, StringFormat.GenericDefault);
            brick = e.Graph.DrawString(pageHeader, System.Drawing.Color.Black, new RectangleF(0, 0, 600, textSize.Height + 50), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new System.Drawing.Font("Calibri", 10, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Center);
            brick.VertAlignment = DevExpress.Utils.VertAlignment.Center;
            brick.HorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            //      PageInfoBrick brick = e.Graph.DrawPageInfo(PageInfo.DateTime, "", System.Drawing.Color.DarkBlue,
            //new RectangleF(0, 0, 100, 20), BorderSide.None);
            //      brick.LineAlignment = BrickAlignment.Center;
            //      brick.HorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            //      brick.VertAlignment= DevExpress.Utils.VertAlignment.Center;
            //      brick.Alignment = BrickAlignment.Center;
            //      brick.AutoWidth = true;
        }
        private GridViewSettings GetGridSettingsInLat(int? timeFormat)
        {
            var settings = new GridViewSettings();
            //---> my
            string StartDate = (string)Session["TAStartDate"];
            string StoptDate = (string)Session["TAStoptDate"];
            int Usrid = Convert.ToInt32(Session["User_Id"]);
            int usrrole = Convert.ToInt32(Session["Role_ID"]);
            string ufname = "";
            string Ulname = "";
            string Userdesignation1 = "";
            int department = Convert.ToInt32(Session["department"]);
            int company = Convert.ToInt32(Session["company"]);
            int Bul_id = Convert.ToInt32(Session["Buidlidngid"]);
            int companyId1 = Convert.ToInt32(Session["company"]);
            var CompanyRecords = db.Companies.Where(x => x.Id == companyId1).FirstOrDefault();
            string companyName = "", RegistraionNum = "";

            if (CompanyRecords != null)
            {
                companyName = CompanyRecords.Name;
                RegistraionNum = CompanyRecords.Comment;
                if (RegistraionNum == null)
                {
                    RegistraionNum = "NA";
                }
            }
            try
            {
                string getvalidcompid = @" select  FirstName,LastName from Users    where Id={0}";
                var userLogDeatil = db.Database.SqlQuery<specificUser>(getvalidcompid, Usrid).FirstOrDefault();

                if (userLogDeatil != null)
                {
                    ufname = userLogDeatil.FirstName;
                    Ulname = userLogDeatil.LastName;
                }
                var userDesignation = db.UserRoles.Where(x => x.Id == usrrole).SingleOrDefault();
                if (userDesignation != null)
                {
                    Userdesignation1 = userDesignation.Name;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (department != 0)
            {
                var dept = db.Departments.SingleOrDefault(x => x.Id == department);
                depname = dept.Name;
                if (company == 0)
                {
                    company = dept.CompanyId;
                }
            }

            DateTime From = DateTime.ParseExact(StartDate, "dd.MM.yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
            DateTime To = DateTime.ParseExact(StoptDate, "dd.MM.yyyy",
                                      System.Globalization.CultureInfo.InvariantCulture);
            string bname = Session["BuildingName"].ToString();
            var ee = (from Ta in db.TABuildingName
                      join B in db.Buildings on Ta.BuildingId equals B.Id
                      where Ta.Name == bname && Ta.ValidFrom <= From && Ta.ValidTo >= To
                      select new
                      {
                          taname = Ta.Name,
                          Ta.ValidFrom,
                          Ta.ValidTo,
                          Ta.BuildingLicense,
                          Ta.CadastralNr,
                          Ta.Address,
                          B.Name,
                      }).FirstOrDefault();

            string ReportPrepareby = ufname + " " + Ulname + "  " + "(" + Userdesignation1 + ")";
            var qry = db.Companies.Where(x => x.Id == company).SingleOrDefault();
            if (qry != null)
            {
                cname = qry.Name;
            }
            string pageHeader = "";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format(ViewResources.SharedStrings.ReportCreated + "{0} ", ReportPrepareby));//Report Created By
            sb.AppendLine(string.Format(ViewResources.SharedStrings.ReportCreatedOn + "{0} ", DateTime.Now.Date.ToString("dd.MM.yyyy"))); //Report Creation
            if (company != 0)
            {
                sb.AppendLine(string.Format(ViewResources.SharedStrings.CompanyDetails + ": {0} ", cname)); //companyName
            }
            if (department != 0)
            {
                sb.AppendLine(string.Format(ViewResources.SharedStrings.UsersDepartment + ": {0} ", depname));
            }
            if (ee != null)
            {
                if (!String.IsNullOrEmpty(ee.Name))
                {
                    sb.AppendLine(string.Format(ViewResources.SharedStrings.BuildingsName + ": {0} ", ee.Name));//BuildingName }
                }
                else
                {
                    sb.AppendLine(string.Format(ViewResources.SharedStrings.BuildingsName + ":xxxx"));//BuildingName }
                }

                if (!String.IsNullOrEmpty(ee.Address))
                {
                    sb.AppendLine(string.Format(ViewResources.SharedStrings.BuildingsAddress + ": {0} ", ee.Address));           //Address }
                }
                else
                {
                    sb.AppendLine(string.Format(ViewResources.SharedStrings.BuildingsAddress + ": xxxx"));
                }
                if (!String.IsNullOrEmpty(ee.BuildingLicense))
                {
                    sb.AppendLine(string.Format(ViewResources.SharedStrings.TaBuildingLicense + ": {0} ", ee.BuildingLicense));//Building License}
                }
                else
                {
                    sb.AppendLine(string.Format(ViewResources.SharedStrings.TaBuildingLicense + ": xxxx"));//Building License}
                }
                if (!String.IsNullOrEmpty(ee.CadastralNr))
                {
                    sb.AppendLine(string.Format(ViewResources.SharedStrings.TaBuildingCadsnrNo + ": {0} ", ee.CadastralNr)); //Bulding Cadastral number}
                }
                else
                {
                    sb.AppendLine(string.Format(ViewResources.SharedStrings.TaBuildingCadsnrNo + ":"));
                }
            }

            else
            {
                sb.AppendLine(string.Format(ViewResources.SharedStrings.BuildingsName + ": All Buildings"));//BuildingName
            }
            sb.AppendLine(" ");
            sb.AppendLine(string.Format(ViewResources.SharedStrings.Period + " {0} ", StartDate + " - " + StoptDate));       //Period
            sb.AppendLine(" ");
            pageHeader = sb.ToString();
            settings.SettingsExport.PageHeader.Center = pageHeader; //ReportPrepareby;//"Department: "+ depname + " - " + cname + " ";
            settings.SettingsExport.PageHeader.Font.Bold = true;
            settings.Name = "GridView";
            settings.CallbackRouteValues = new { Controller = "Home", Action = "GridViewPartial" };
            settings.SettingsExport.FileName = "Report.pdf";
            settings.SettingsExport.PaperKind = System.Drawing.Printing.PaperKind.A3Extra;
            settings.Width = System.Web.UI.WebControls.Unit.Percentage(100);
            settings.ControlStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            settings.KeyFieldName = "Id";

            settings.Columns.Add(column =>
            {
                column.FieldName = "FullName";
                column.Caption = ViewResources.SharedStrings.FullName;
                column.GroupIndex = (CurrentUser.Get().IsSuperAdmin || CurrentUser.Get().IsCompanyManager) ? 2 : 1;
                column.Visible = false;
            });

            if (CurrentUser.Get().IsCompanyManager || CurrentUser.Get().IsSuperAdmin)
            {
                settings.Columns.Add(column =>
                {
                    column.FieldName = "companydeatils";
                    column.Caption = ViewResources.SharedStrings.CompanyDetails;
                    column.Visible = false;
                    column.GroupIndex = 0;
                });
            }
            settings.Columns.Add(column =>
            {
                column.FieldName = "FirstName";
                column.Caption = ViewResources.SharedStrings.Name;
                column.CellStyle.Font.Bold = true;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "LastName";
                column.Caption = ViewResources.SharedStrings.SurName;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "UserId";
                column.Caption = ViewResources.SharedStrings.UsersUserId;
                column.Width = 35;
                column.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;

                column.Visible = false;
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "PersonalCode";
                column.Caption = ViewResources.SharedStrings.UsersPersonalCode;
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "Name";
                column.Caption = ViewResources.SharedStrings.Position;
                column.Width = 35;
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "Department";
                column.Caption = ViewResources.SharedStrings.UsersDepartment;
                if (CurrentUser.Get().IsCompanyManager || CurrentUser.Get().IsSuperAdmin)
                {
                    column.GroupIndex = 1;
                }
                else if (CurrentUser.Get().IsDepartmentManager)
                {
                    column.GroupIndex = 0;
                }
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "StartedInLat";
                column.Caption = ViewResources.SharedStrings.Date;
                column.PropertiesEdit.DisplayFormatString = "dddd M/d/yyyy ";
                column.Width = 140;
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "Started";
                column.Caption = ViewResources.SharedStrings.Started;
                column.PropertiesEdit.DisplayFormatString = "HH:mm";
                column.Width = 45;
            });
            //toTime
            settings.Columns.Add(column =>
            {
                column.FieldName = "Finished";
                column.Caption = ViewResources.SharedStrings.Finished;
                column.PropertiesEdit.DisplayFormatString = "d";
                column.PropertiesEdit.DisplayFormatString = "HH:mm";
                column.Width = 45;
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "Hours";
                column.Visible = false;
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "Total";
                column.Caption = ViewResources.SharedStrings.TotalTime;
                column.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
                column.PropertiesEdit.DisplayFormatString = "HH:mm";
                column.PropertiesEdit.NullDisplayText = " ";
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "Comment";
                column.Caption = ViewResources.SharedStrings.CommonComment;
            });
            settings.CustomUnboundColumnData = (sender, e) =>
            {
                if (e.Column.FieldName == "Total")
                {
                    double time = Convert.ToDouble(e.GetListSourceFieldValue("Hours"));
                    if (time <= 0) { e.Value = "00:00";/*null;*/ }
                    else
                    {
                        e.Value = DateTime.MinValue.Add(TimeSpan.FromSeconds(time));
                    }
                }
            };

            //Summary
            settings.Settings.ShowFooter = true;
            settings.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "Hours");
            settings.SummaryDisplayText = (sender, e) =>
            {
                if (e.IsGroupSummary)
                {
                    if (e.Item.FieldName == "Hours")
                    {
                        double time = Double.Parse(e.Value.ToString());
                        string hours = Math.Floor((TimeSpan.FromSeconds(time)).TotalHours) > 9 ? Math.Floor((TimeSpan.FromSeconds(time)).TotalHours).ToString() : "0" + Math.Floor((TimeSpan.FromSeconds(time)).TotalHours).ToString();
                        string minutes = (TimeSpan.FromSeconds(time)).Minutes > 9 ? (TimeSpan.FromSeconds(time)).Minutes.ToString() : "0" + (TimeSpan.FromSeconds(time)).Minutes;
                        // e.Text = "Total hours = " + hours + ":" + minutes;
                        e.Text = ViewResources.SharedStrings.TotalHours + " = " + hours + ":" + minutes;
                    }
                }
            };
            return settings;
        }

        /***/
        private GridViewSettings GetGridSettings1(int? timeFormat)
        {
            var settings = new GridViewSettings();
            //---> my
            string StartDate = (string)Session["TAStartDate"];
            string StoptDate = (string)Session["TAStoptDate"];
            int department = Convert.ToInt32(Session["department"]);
            int company = Convert.ToInt32(Session["company"]);

            var qry = db.Companies.Where(x => x.Id == company).SingleOrDefault();
            if (qry != null)
            {
                cname = qry.Name;
            }
            var qry1 = db.Departments.Where(x => x.Id == department).SingleOrDefault();

            if (qry1 != null)
            {
                depname = qry1.Name;
            }

            settings.SettingsExport.PageHeader.Center = depname + " - " + cname + " ";
            settings.SettingsExport.PageHeader.Left = StartDate + " - " + StoptDate + " ";
            settings.SettingsExport.PageHeader.Right = DateTime.Now.Date.ToString("dd.MM.yyyy");
            //<--- my
            settings.Name = "GridView";
            settings.CallbackRouteValues = new { Controller = "Home", Action = "GridViewPartial" };
            settings.SettingsExport.FileName = "Report.pdf";
            settings.SettingsExport.PaperKind = System.Drawing.Printing.PaperKind.A4;
            settings.Width = System.Web.UI.WebControls.Unit.Percentage(100);
            settings.ControlStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            settings.KeyFieldName = "Id";

            settings.Columns.Add(column =>
            {
                column.FieldName = "UserId";
                column.Caption = "UserId";
                column.GroupIndex = 0;
                column.Width = 50;
                column.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            });

            settings.Columns.Add("FirstName");
            settings.Columns.Add("LastName");

            settings.Columns.Add(column =>
            {
                column.FieldName = "Started";
                column.Caption = "Started";
                column.PropertiesEdit.DisplayFormatString = "dddd M/d/yyyy ";
                column.Width = 200;
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "Started";
                column.Caption = "Check In";
                column.PropertiesEdit.DisplayFormatString = "HH:mm";
                column.Width = 50;
            });
            //toTime
            settings.Columns.Add(column =>
            {
                column.FieldName = "Finished";
                column.Caption = "Check Out";
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
                column.FieldName = "Name";
                column.Caption = "Comment";
            });

            settings.CustomUnboundColumnData = (sender, e) =>
            {
                if (e.Column.FieldName == "Total")
                {
                    double time = Convert.ToDouble(e.GetListSourceFieldValue("Hours"));
                    //if (time == 0)
                    if (time <= 0)
                    { e.Value = "00:00"; }
                    else
                    {
                        e.Value = DateTime.MinValue.Add(TimeSpan.FromSeconds(time));
                    }
                }
            };

            //Summary
            settings.Settings.ShowFooter = true;
            settings.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "Hours");
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
            return settings;
        }

        void exportOptions_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs ea)
        {
            GridViewExcelDataPrinter printer = (GridViewExcelDataPrinter)ea.DataSourceOwner;
            var index = printer.GetVisibleIndex(ea.RowHandle);
            string fullname = Convert.ToString(printer.Grid.GetRowValues(index, "colorflag"));
            string compnayname = Convert.ToString(printer.Grid.GetRowValues(index, "companycolrflag"));

            if (ea.AreaType == DevExpress.Export.SheetAreaType.Header)
            {
                ea.Formatting.Font.Bold = true;
                ea.Formatting.BackColor = System.Drawing.Color.LightGray;
                ea.Handled = true;
            }

            if (ea.AreaType == DevExpress.Export.SheetAreaType.GroupHeader)
            {
                ea.Formatting.Font.Bold = true;
                ea.Formatting.BackColor = System.Drawing.Color.LightGray;
                ea.Handled = true;
            }
            if (fullname == "Yes")
            {
                ea.Formatting.BackColor = System.Drawing.Color.LightGray;
                ea.Formatting.Font.Bold = true;
                ea.Handled = true;
            }
        }
        public ActionResult TABuildingListforChange()
        {
            DateTime f1 = Convert.ToDateTime(Session["datefrom"]);
            DateTime f2 = Convert.ToDateTime(Session["dateto"]);
            int buildingid = Convert.ToInt32(Session["buildingid"]);

            List<itembuilding> Building_Data = new List<itembuilding>();
            var ctvm = new BuildingNameViewModel();

            object result = "";
            if (buildingid == 0)
            {
                result = db.TABuildingName.Where(x => x.IsDeleted == false).OrderByDescending(x => x.Id).ToList();
            }
            else
                result = db.TABuildingName.Where(x => x.Id == buildingid && x.IsDeleted == false).OrderByDescending(x => x.Id).ToList();


            ViewBag.Data = result;
            foreach (var items in ViewBag.Data)
            {
                Building_Data.Add(new itembuilding() { Id = items.Id, Name = items.Name, BuildingId = items.BuildingId, Address = items.Address, BuildingLicense = items.BuildingLicense, CadastralNr = items.CadastralNr, ValidFrom = items.ValidFrom, ValidTo = items.ValidTo, Customer = items.Customer, Contractor = items.Contractor, Contract = items.Contract, Sum = items.Sum, Lat = items.Lat, Lng = items.Lng });
            }

            ctvm.BuildingData = Building_Data;
            ctvm.buildings = GetBuildingsForGrid();

            return PartialView("BindTaBuildName", ctvm);
        }
        public ActionResult TABuildingList(string datefrom, string dateto, int buildingid)
        {
            DateTime From = DateTime.ParseExact(datefrom, "dd.MM.yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);

            DateTime To = DateTime.ParseExact(dateto, "dd.MM.yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
            Session["datefrom"] = From;
            Session["dateto"] = To;
            Session["buildingid"] = buildingid;

            List<itembuilding> Building_Data = new List<itembuilding>();
            var ctvm = new BuildingNameViewModel();
            object result = "";
            if (buildingid == 0)
            {
                result = db.TABuildingName.Where(x => x.IsDeleted == false).OrderByDescending(x => x.Id).ToList();
            }
            else
                result = db.TABuildingName.Where(x => x.IsDeleted == false && x.BuildingId == buildingid).OrderByDescending(x => x.Id).ToList();

            ViewBag.MenuData = result;
            foreach (var items in ViewBag.MenuData)
            {
                Building_Data.Add(new itembuilding() { Id = items.Id, Name = items.Name, BuildingId = items.BuildingId, Address = items.Address, BuildingLicense = items.BuildingLicense, CadastralNr = items.CadastralNr, ValidFrom = items.ValidFrom, ValidTo = items.ValidTo, Customer = items.Customer, Contractor = items.Contractor, Contract = items.Contract, Sum = items.Sum, Lat = items.Lat, Lng = items.Lng });
            }

            ctvm.BuildingData = Building_Data;
            ctvm.buildings = GetBuildingsForGrid();
            ctvm.buildings = (ctvm.buildings).Where(x => x.IsDeleted == false);
            return PartialView("BindTaBuildName", ctvm);
        }

        public IEnumerable<itembuilding> GetBuildingsForGrid()
        {
            List<itembuilding> lib = new List<itembuilding>();
            var query = from b in db.Buildings
                        select new
                        {
                            buildingID = b.Id,
                            building = b.Name
                        };
            //query = query.Where(x => x.IsDeleted == 0);
            itembuilding ib_0 = new ViewModels.itembuilding();
            ib_0.BuildingId = 0;
            ib_0.BuildingName = "--Select--";
            lib.Add(ib_0);
            foreach (var q in query)
            {
                itembuilding ib = new ViewModels.itembuilding();
                ib.BuildingId = q.buildingID;
                ib.BuildingName = q.building;
                lib.Add(ib);
            }
            return lib;
        }

        [ValidateInput(false)]
        public ActionResult TAReportExportCallBack()
        {
            try
            {
                var ULVM = CreateViewModel<UserListViewModel>();
                ULVM = (UserListViewModel)Session["TypedListModel"];
                return PartialView("TAReportExportUsers", ULVM.Users);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult TAReportExport(int? department, int? company, string FromDateTA, string ToDateTA, int format, int? BuildingId, string BName)
        {
            DateTime From = DateTime.ParseExact(FromDateTA, "dd.MM.yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);

            DateTime To = DateTime.ParseExact(ToDateTA, "dd.MM.yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);

            List<User> usrs = new List<User>();
            List<User> usrs1 = new List<User>();
            List<int> usr = new List<int>();
            var ULVM = CreateViewModel<UserListViewModel>();
            Session["TAStartDate"] = FromDateTA;
            Session["TAStoptDate"] = ToDateTA;
            Session["timeFormat"] = format;
            Session["department"] = department;
            Session["company"] = company;
            Session["BuildingName"] = BName;
            Session["Buidlidngid"] = BuildingId;

            if (CurrentUser.Get().IsSuperAdmin || CurrentUser.Get().IsDepartmentManager || CurrentUser.Get().IsCompanyManager)
            {
                if (CurrentUser.Get().IsSuperAdmin)
                {
                    con.Open();
                    if ((company == null || company == 0) && (department == null || department == 0))
                    {
                        DataTable dt = new DataTable();
                        if (BuildingId == null || BuildingId == 0)
                        {
                            SqlDataAdapter da = new SqlDataAdapter("select id from Users where IsDeleted=0 and id in (select userid from TAMoves ta where IsDeleted=0 and Ta.Started >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "')", con);
                            da.Fill(dt);
                        }
                        else
                        {
                            SqlDataAdapter da = new SqlDataAdapter("select id from Users where IsDeleted=0 and id in (select userid from TAMoves ta where IsDeleted=0 and ta.FinishedBoId in (select id from BuildingObjects where IsDeleted=0 and BuildingId='" + BuildingId + "') and Ta.Started >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "')", con);
                            da.Fill(dt);
                        }
                        foreach (DataRow dr in dt.Rows)
                        {
                            usr.Add(Convert.ToInt32(dr["id"]));
                        }
                    }
                    else
                    {
                        DataTable dt = new DataTable();
                        if (department != null && department != 0)
                        {
                            if (BuildingId == null || BuildingId == 0)
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select distinct UserId as id from UserDepartments where IsDeleted=0  and UserId in (select userid from TAMoves ta where IsDeleted=0 and  Ta.Started >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "') and DepartmentId='" + department + "'", con);
                                da.Fill(dt);
                            }
                            else
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select distinct UserId as id from UserDepartments where IsDeleted=0  and UserId in (select userid from TAMoves ta where IsDeleted=0 and ta.FinishedBoId in (select id from BuildingObjects where IsDeleted=0 and BuildingId='" + BuildingId + "') and  Ta.Started >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "') and DepartmentId='" + department + "'", con);
                                da.Fill(dt);
                            }
                        }
                        else if (company != null && company != 0)
                        {
                            if (BuildingId == null || BuildingId == 0)
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select id from Users where IsDeleted=0 and id in (select userid from TAMoves ta where IsDeleted=0 and Ta.Started >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "') and companyid='" + company + "'", con);
                                da.Fill(dt);
                            }
                            else
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select id from Users where IsDeleted=0 and id in (select userid from TAMoves ta where IsDeleted=0  and ta.FinishedBoId in (select id from BuildingObjects where IsDeleted=0 and BuildingId='" + BuildingId + "') and Ta.Started >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "') and companyid='" + company + "'", con);
                                da.Fill(dt);
                            }
                        }
                        foreach (DataRow dr in dt.Rows)
                        {
                            usr.Add(Convert.ToInt32(dr["id"]));
                        }
                    }
                    con.Close();
                }
                else
                {
                    con.Open();
                    if (CurrentUser.Get().IsCompanyManager)
                    {
                        DataTable dt = new DataTable();
                        if (department != null && department != 0)
                        {
                            if (BuildingId == null || BuildingId == 0)
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select distinct UserId as id from UserDepartments where IsDeleted=0  and UserId in (select userid from TAMoves ta where IsDeleted=0 and  Ta.Started >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "') and DepartmentId='" + department + "'", con);
                                da.Fill(dt);
                            }
                            else
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select distinct UserId as id from UserDepartments where IsDeleted=0  and UserId in (select userid from TAMoves ta where IsDeleted=0 and ta.FinishedBoId in (select id from BuildingObjects where IsDeleted=0 and BuildingId='" + BuildingId + "') and  Ta.Started >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "') and DepartmentId='" + department + "'", con);
                                da.Fill(dt);
                            }
                        }
                        else
                        {
                            if (BuildingId == null || BuildingId == 0)
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select id from Users where IsDeleted=0 and id in (select userid from TAMoves ta where IsDeleted=0 and Ta.Started >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "') and companyid='" + company + "'", con);
                                da.Fill(dt);
                            }
                            else
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select id from Users where IsDeleted=0 and id in (select userid from TAMoves ta where IsDeleted=0  and ta.FinishedBoId in (select id from BuildingObjects where IsDeleted=0 and BuildingId='" + BuildingId + "') and Ta.Started >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "') and companyid='" + company + "'", con);
                                da.Fill(dt);
                            }
                        }
                        foreach (DataRow dr in dt.Rows)
                        {
                            usr.Add(Convert.ToInt32(dr["id"]));
                        }
                    }
                    else // if (CurrentUser.Get().IsDepartmentManager)
                    {
                        List<int> dep = _UserDepartmentRepository.FindByUserId(CurrentUser.Get().Id).Select(x => x.DepartmentId).ToList();
                        if (dep.Count > 0 && dep != null)
                        {
                            DataTable dt = new DataTable();
                            var str = String.Join(",", dep);
                            if (BuildingId == null && BuildingId == 0)
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select distinct UserId as id from UserDepartments where IsDeleted=0 and UserId in (select userid from TAMoves ta where IsDeleted=0 and  Ta.Started >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "')  and DepartmentId in(" + str + ")", con);
                                da.Fill(dt);
                            }
                            else
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select distinct UserId as id from UserDepartments where IsDeleted=0 and UserId in (select userid from TAMoves ta where IsDeleted=0 and ta.FinishedBoId in (select id from BuildingObjects where IsDeleted=0 and BuildingId='" + BuildingId + "')  and  Ta.Started >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "')  and DepartmentId in(" + str + ")", con);
                                da.Fill(dt);
                            }
                            foreach (DataRow dr in dt.Rows)
                            {
                                usr.Add(Convert.ToInt32(dr["id"]));
                            }
                        }
                    }
                    con.Close();
                }
            }

            if (usr.Count > 0)
            {
                usrs = _userRepository.FindAll(x => usr.Contains(x.Id) && !x.IsDeleted && x.Active == true).ToList();
            }

            Mapper.Map(usrs, ULVM.Users);
            Session["TypedListModel"] = ULVM;
            return PartialView("TAReportExport", Session["TypedListModel"]);
        }

        public ActionResult TAReportExportReportByDays(int? department, int? company, string FromDateTA, string ToDateTA, int format, int? BuildingId, string BName)
        {
            DateTime From = DateTime.ParseExact(FromDateTA, "dd.MM.yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);

            DateTime To = DateTime.ParseExact(ToDateTA, "dd.MM.yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);

            List<User> usrs = new List<User>();
            List<User> usrs1 = new List<User>();
            List<int> usr = new List<int>();
            var ULVM = CreateViewModel<UserListViewModel>();
            Session["TAStartDate"] = FromDateTA;
            Session["TAStoptDate"] = ToDateTA;
            Session["timeFormat"] = format;
            Session["department"] = department;
            Session["company"] = company;
            Session["BuildingName"] = BName;
            Session["Buidlidngid"] = BuildingId;

            if (CurrentUser.Get().IsSuperAdmin || CurrentUser.Get().IsDepartmentManager || CurrentUser.Get().IsCompanyManager)
            {
                if (CurrentUser.Get().IsSuperAdmin)
                {
                    con.Open();
                    if ((company == null || company == 0) && (department == null || department == 0))
                    {
                        DataTable dt = new DataTable();
                        if (BuildingId == null || BuildingId == 0)
                        {
                            SqlDataAdapter da = new SqlDataAdapter("select id from Users where IsDeleted=0 and id in (select userid from TAReports ta where IsDeleted=0 and Ta.ReportDate >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.ReportDate <= '" + To.ToString("MM/dd/yyyy") + "')", con);
                            da.Fill(dt);
                        }
                        else
                        {
                            SqlDataAdapter da = new SqlDataAdapter("select id from Users where IsDeleted=0 and id in (select userid from TAReports ta where IsDeleted=0 and Ta.ReportDate >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.ReportDate <= '" + To.ToString("MM/dd/yyyy") + "' and BuildingId='" + BuildingId + "')", con);

                            da.Fill(dt);
                        }
                        foreach (DataRow dr in dt.Rows)
                        {
                            usr.Add(Convert.ToInt32(dr["id"]));
                        }
                    }
                    else
                    {
                        DataTable dt = new DataTable();
                        if (department != null && department != 0)
                        {
                            if (BuildingId == null || BuildingId == 0)
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select distinct UserId as id from UserDepartments where IsDeleted=0  and UserId in (select userid from TAReports ta where IsDeleted=0 and Ta.ReportDate >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.ReportDate <= '" + To.ToString("MM/dd/yyyy") + "') and DepartmentId='" + department + "'", con);
                                da.Fill(dt);
                            }
                            else
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select distinct UserId as id from UserDepartments where IsDeleted=0  and UserId in (select userid from TAReports ta where IsDeleted=0 and Ta.ReportDate >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.ReportDate <= '" + To.ToString("MM/dd/yyyy") + "' and BuildingId='" + BuildingId + "') and DepartmentId='" + department + "'", con);
                                da.Fill(dt);
                            }
                        }
                        else if (company != null && company != 0)
                        {
                            if (BuildingId == null || BuildingId == 0)
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select id from Users where IsDeleted=0 and id in (select userid from TAReports ta where IsDeleted=0 and Ta.ReportDate >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.ReportDate <= '" + To.ToString("MM/dd/yyyy") + "') and companyid='" + company + "'", con);
                                da.Fill(dt);
                            }
                            else
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select id from Users where IsDeleted=0 and id in (select userid from TAReports ta where IsDeleted=0 and Ta.ReportDate >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.ReportDate <= '" + To.ToString("MM/dd/yyyy") + "' and BuildingId='" + BuildingId + "') and companyid='" + company + "'", con);
                                da.Fill(dt);
                            }
                        }
                        foreach (DataRow dr in dt.Rows)
                        {
                            usr.Add(Convert.ToInt32(dr["id"]));
                        }
                    }
                    con.Close();
                }
                else
                {
                    con.Open();
                    if (CurrentUser.Get().IsCompanyManager)
                    {
                        DataTable dt = new DataTable();
                        if (department != null && department != 0)
                        {
                            if (BuildingId == null || BuildingId == 0)
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select distinct UserId as id from UserDepartments where IsDeleted=0  and UserId in (select userid from TAReports ta where IsDeleted=0 and Ta.ReportDate >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.ReportDate <= '" + To.ToString("MM/dd/yyyy") + "') and DepartmentId='" + department + "'", con);
                                da.Fill(dt);
                            }
                            else
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select distinct UserId as id from UserDepartments where IsDeleted=0  and UserId in (select userid from TAReports ta where IsDeleted=0 and Ta.ReportDate >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.ReportDate <= '" + To.ToString("MM/dd/yyyy") + "' and BuildingId='" + BuildingId + "') and DepartmentId='" + department + "'", con);
                                da.Fill(dt);
                            }
                        }
                        else
                        {
                            if (BuildingId == null || BuildingId == 0)
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select id from Users where IsDeleted=0 and id in (select userid from TAReports ta where IsDeleted=0 and Ta.ReportDate >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.ReportDate <= '" + To.ToString("MM/dd/yyyy") + "') and companyid='" + company + "'", con);
                                da.Fill(dt);
                            }
                            else
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select id from Users where IsDeleted=0 and id in (select userid from TAReports ta where IsDeleted=0 and Ta.ReportDate >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.ReportDate <= '" + To.ToString("MM/dd/yyyy") + "' and BuildingId='" + BuildingId + "') and companyid='" + company + "'", con);
                                da.Fill(dt);
                            }
                        }
                        foreach (DataRow dr in dt.Rows)
                        {
                            usr.Add(Convert.ToInt32(dr["id"]));
                        }
                    }
                    else // if (CurrentUser.Get().IsDepartmentManager)
                    {
                        List<int> dep = _UserDepartmentRepository.FindByUserId(CurrentUser.Get().Id).Select(x => x.DepartmentId).ToList();
                        if (dep.Count > 0 && dep != null)
                        {
                            DataTable dt = new DataTable();
                            var str = String.Join(",", dep);
                            if (BuildingId == null && BuildingId == 0)
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select distinct UserId as id from UserDepartments where IsDeleted=0 and UserId in (select userid from TAReports ta where IsDeleted=0 and Ta.ReportDate >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.ReportDate <= '" + To.ToString("MM/dd/yyyy") + "')  and DepartmentId in(" + str + ")", con);
                                da.Fill(dt);
                            }
                            else
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select distinct UserId as id from UserDepartments where IsDeleted=0 and UserId in (select userid from TAReports ta where IsDeleted=0 and Ta.ReportDate >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.ReportDate <= '" + To.ToString("MM/dd/yyyy") + "' and BuildingId='" + BuildingId + "')  and DepartmentId in(" + str + ")", con);
                                da.Fill(dt);
                            }
                            foreach (DataRow dr in dt.Rows)
                            {
                                usr.Add(Convert.ToInt32(dr["id"]));
                            }
                        }
                    }
                    con.Close();
                }
            }

            if (usr.Count > 0)
            {
                usrs = _userRepository.FindAll(x => usr.Contains(x.Id) && !x.IsDeleted && x.Active == true).ToList();
            }

            Mapper.Map(usrs, ULVM.Users);
            Session["TypedListModel"] = ULVM;
            return PartialView("TAReportExport", Session["TypedListModel"]);
        }


        #endregion
        #region other

        protected void InsertProduct(TAMove product, MVCxGridViewBatchUpdateValues<TAMove, int> updateValues)
        {
            // try {
            //   NorthwindDataProvider.InsertProduct(product);
            // }
            //catch(Exception e) {
            //    updateValues.SetErrorText(product, e.Message);
            //}
        }
        protected void UpdateProduct(TAMove product, MVCxGridViewBatchUpdateValues<TAMove, int> updateValues)
        {
            try
            {
                //NorthwindDataProvider.UpdateProduct(product);
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(product, e.Message);
            }
        }
        protected void DeleteProduct(int productID, MVCxGridViewBatchUpdateValues<TAMove, int> updateValues)
        {
            //  try {
            //   NorthwindDataProvider.DeleteProduct(productID);
            // }
            //catch(Exception e) {
            //    updateValues.SetErrorText(productID, e.Message);
            //}
        }
        #endregion
        #region GetMethods
        private IEnumerable<BuildingObject> GetBuildingObjects(int? companyId)
        {
            IEnumerable<BuildingObject> BuildingObjects = new List<BuildingObject>();
            //BuildingObjects = _BuildingObjectRepository.FindAll(x => (x.TypeId == 8) && !x.IsDeleted);
            BuildingObjects = _BuildingObjectRepository.FindAll(x => (x.TypeId == 8 || x.TypeId == 1) && !x.IsDeleted);
            return BuildingObjects;
        }
        public IEnumerable<itembuilding> GetTANames(int? bid)
        {
            List<itembuilding> Building_Data = new List<itembuilding>();
            var result = db.TABuildingName.Where(x => x.IsDeleted == false).ToList();
            ViewBag.MenuData = result;
            foreach (var items in ViewBag.MenuData)
            {
                Building_Data.Add(new itembuilding() { Id = items.Id, Name = items.Name, BuildingId = items.BuildingId, Address = items.Address, BuildingLicense = items.BuildingLicense, CadastralNr = items.CadastralNr, ValidFrom = items.ValidFrom.ToString("dd/M/yyyy"), ValidTo = items.ValidTo.ToString("dd/M/yyyy") });
            }
            return Building_Data;
        }

        private IEnumerable<TAMove> GetUserTAMoves(int userId, DateTime From, DateTime To)
        {
            IEnumerable<TAMove> UserTAMoves = new List<TAMove>();
            UserTAMoves = _TAMoveRepository.FindAll(x => x.UserId == userId && !x.IsDeleted && x.Started >= From && x.Finished < To.AddDays(1));
            return UserTAMoves;
        }
        private IEnumerable<TAMove> GetUsersTAMoves(DateTime From, DateTime To, int? company, int? department, int? BuildingId)
        {
            IEnumerable<TAMove> UserTAMoves = new List<TAMove>();
            // IEnumerable<TAMove> UserTAMoves1 = new List<TAMove>();
            List<TAMove> UserTAMoves1 = new List<TAMove>();
            if (company != null)
            {
                List<int> usr = _userRepository.FindAll(x => x.Active == true && x.CompanyId == company.GetValueOrDefault() && x.IsDeleted == false).Select(x => x.Id).ToList();
                if (department != null)
                {
                    List<int> dps = _UserDepartmentRepository.FindAll(x => !x.IsDeleted && x.DepartmentId == department).Select(x => x.UserId).ToList();
                    usr = usr.FindAll(x => dps.Contains(x)).ToList();//               dps.Contains(usr);
                }
                UserTAMoves = _TAMoveRepository.FindAll(x => !x.IsDeleted && x.Started >= From && x.Finished < To.AddDays(1) && usr.Contains(x.UserId));
            }
            else
            {
                UserTAMoves = _TAMoveRepository.FindAll(x => !x.IsDeleted && x.Started >= From && x.Finished < To.AddDays(1));
            }
            if (BuildingId != null && BuildingId != 0)
            {
                foreach (var u in UserTAMoves)
                {
                    //var tr = db.TAreport.Where(x => x.UserId == u.UserId && x.BuildingId == BuildingId && x.IsDeleted == false).Any();
                    var tr = _taReportRepository.FindAll(x => x.UserId == u.UserId && x.BuildingId == BuildingId && x.IsDeleted == false).Any();
                    if (tr)
                    {
                        UserTAMoves1.Add(u);
                    }
                }
                if (UserTAMoves1 != null)
                {
                    UserTAMoves = UserTAMoves1;
                }
            }

            return UserTAMoves;
        }

        private IEnumerable<TAMove> GetUsersTAMovesNew(DateTime From, DateTime To, int? company, int? department, int? BuildingId)
        {

            IEnumerable<TAMove> UserTAMoves = new List<TAMove>();
            List<TAMove> UserTAMoves1 = new List<TAMove>();
            if (company != null && company != 0)
            {
                List<int> usr = new List<int>();
                con.Open();

                if (department != null && department != 0)
                {
                    SqlDataAdapter da1 = new SqlDataAdapter("select distinct UserId as id from UserDepartments where IsDeleted=0  and UserId in (select userid from TAMoves ta where IsDeleted=0 and  Ta.Started >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "') and DepartmentId='" + department + "'", con);
                    DataTable dt1 = new DataTable();
                    da1.Fill(dt1);
                    foreach (DataRow dr in dt1.Rows)
                    {
                        usr.Add(Convert.ToInt32(dr["id"]));
                    }
                }
                else
                {
                    SqlDataAdapter da = new SqlDataAdapter("select id from Users where IsDeleted=0  and CompanyId='" + company + "' and id in (select userid from TAMoves ta where IsDeleted=0 and Ta.Started >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "')", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    foreach (DataRow dr in dt.Rows)
                    {
                        usr.Add(Convert.ToInt32(dr["id"]));
                    }
                }
                con.Close();
                if (usr != null && usr.Count > 0)
                {
                    UserTAMoves = _TAMoveRepository.FindAll(x => !x.IsDeleted && x.Started >= From && x.Finished < To.AddDays(1) && usr.Contains(x.UserId));
                }
            }
            else
            {
                UserTAMoves = _TAMoveRepository.FindAll(x => !x.IsDeleted && x.Started >= From && x.Finished < To.AddDays(1));
            }
            if (BuildingId != null && BuildingId != 0)
            {
                foreach (var u in UserTAMoves)
                {
                    //var tr = _userRepository.FindAll(x => x.Id == u.UserId && x.buildingID == BuildingId).SingleOrDefault();
                    //var tr = db.TAreport.Where(x => x.UserId == u.UserId && x.BuildingId == BuildingId && x.IsDeleted == false).Any();
                   var tr =_taReportRepository.FindAll(x => x.UserId == u.UserId && x.BuildingId == BuildingId.Value && x.IsDeleted == false).Any();
                    if (tr)
                    {
                        UserTAMoves1.Add(u);
                    }
                }
                if (UserTAMoves1 != null)
                {
                    UserTAMoves = UserTAMoves1;
                }
            }

            Session["UserMovesDetails"] = UserTAMoves;

            return UserTAMoves;
        }
        private IEnumerable<TAReport> GetTAreports(DateTime From, DateTime To, int usr)
        {
            IEnumerable<TAReport> retvalue = new List<TAReport>();
            retvalue = _reportRepository.FindAll(x => !x.IsDeleted && x.ReportDate >= From && x.ReportDate < To.AddDays(1) && x.UserId == usr);
            foreach (var obj in retvalue)
            {
                obj.FullName = obj.User.LastName + " " + obj.User.FirstName;
            }
            return retvalue;
        }

        private IEnumerable<TAReport> GetTAreportsUsr(DateTime From, DateTime To, int? SelectedCompanyId, int? department, int usrid)
        {
            IEnumerable<TAReport> retvalue = new List<TAReport>();
            if (SelectedCompanyId != null)
            {
                List<int> usr = _userRepository.FindAll(x => x.Active == true && x.CompanyId == SelectedCompanyId.GetValueOrDefault() && x.IsDeleted == false).Select(x => x.Id).ToList();
                if (department != null)
                {
                    List<int> dps = _UserDepartmentRepository.FindAll(x => !x.IsDeleted && x.DepartmentId == department).Select(x => x.UserId).ToList();
                    //  List<int> dps = _UserDepartmentRepository.FindAll(x => x.DepartmentId == department).Select(x => x.Id).ToList();
                    usr = usr.FindAll(x => dps.Contains(x)).ToList();//               dps.Contains(usr);
                }
                retvalue = _reportRepository.FindAll(x => !x.IsDeleted && x.ReportDate >= From && x.ReportDate < To.AddDays(1) && usr.Contains(x.UserId) && x.UserId == usrid);
                //UserTAMoves = _TAMoveRepository.FindAll(x => !x.IsDeleted && x.Started >= From && x.Finished <= To && usr.Contains(x.UserId));
            }
            else
            {
                if (department != null)
                {
                    retvalue = _reportRepository.FindAll(x => !x.IsDeleted && x.ReportDate >= From && x.ReportDate < To.AddDays(1) && x.DepartmentId == department && x.UserId == usrid);
                }
                else
                {
                    retvalue = _reportRepository.FindAll(x => !x.IsDeleted && x.ReportDate >= From && x.ReportDate < To.AddDays(1) && x.UserId == usrid);
                    //UserTAMoves = _TAMoveRepository.FindAll(x => !x.IsDeleted && x.Started >= From && x.Finished <= To);
                }
            }
            foreach (var obj in retvalue)
            {
                obj.FullName = obj.User.LastName + " " + obj.User.FirstName;
            }
            return retvalue;
        }
        private IEnumerable<TAReport> GetTAreports(DateTime From, DateTime To, int? SelectedCompanyId, int? department,int? BuildingId)
        {
            IEnumerable<TAReport> retvalue = new List<TAReport>();
            if (SelectedCompanyId != null)
            {
                List<int> usr = _userRepository.FindAll(x => x.Active == true && x.CompanyId == SelectedCompanyId.GetValueOrDefault() && x.IsDeleted == false).Select(x => x.Id).ToList();
                if (department != null)
                {
                    List<int> dps = _UserDepartmentRepository.FindAll(x => !x.IsDeleted && x.DepartmentId == department).Select(x => x.UserId).ToList();
                    //  List<int> dps = _UserDepartmentRepository.FindAll(x => x.DepartmentId == department).Select(x => x.Id).ToList();
                    usr = usr.FindAll(x => dps.Contains(x)).ToList();//               dps.Contains(usr);
                }
                if(BuildingId != null)
                {
                   List<int> buildingUsers = _taReportRepository.FindAll(x => !x.IsDeleted && x.BuildingId == BuildingId.Value).Select(y => y.UserId).Distinct().ToList();
                    usr = usr.FindAll(x => buildingUsers.Contains(x)).ToList();
                    retvalue = _reportRepository.FindAll(x => !x.IsDeleted && x.ReportDate >= From && x.ReportDate < To.AddDays(1) && usr.Contains(x.UserId) && x.BuildingId == BuildingId);
                }
                else
                {
                    retvalue = _reportRepository.FindAll(x => !x.IsDeleted && x.ReportDate >= From && x.ReportDate < To.AddDays(1) && usr.Contains(x.UserId));
                }
                
                //UserTAMoves = _TAMoveRepository.FindAll(x => !x.IsDeleted && x.Started >= From && x.Finished <= To && usr.Contains(x.UserId));
            }
            else
            {
                if (department != null)
                {
                    retvalue = _reportRepository.FindAll(x => !x.IsDeleted && x.ReportDate >= From && x.ReportDate < To.AddDays(1) && x.DepartmentId == department);
                }
                else if(BuildingId != null)
                {
                    retvalue = _reportRepository.FindAll(x => !x.IsDeleted && x.ReportDate >= From && x.ReportDate < To.AddDays(1) && x.BuildingId == Convert.ToInt32(BuildingId));
                }
                else
                {
                    retvalue = _reportRepository.FindAll(x => !x.IsDeleted && x.ReportDate >= From && x.ReportDate < To.AddDays(1));
                    //UserTAMoves = _TAMoveRepository.FindAll(x => !x.IsDeleted && x.Started >= From && x.Finished <= To);
                }
             
            }
            foreach (var obj in retvalue)
            {
                obj.FullName = obj.User.LastName + " " + obj.User.FirstName;
            }
            
            return retvalue;
        }

        private IEnumerable<TAReport> GetTAreportsUsers(DateTime From, DateTime To, List<int> UserIds)
        {
            IEnumerable<TAReport> retvalue = new List<TAReport>();
            if (UserIds != null)
            {
                retvalue = _reportRepository.FindAll(x => !x.IsDeleted && UserIds.Contains(x.UserId) && x.ReportDate >= From && x.ReportDate < To.AddDays(1));
            }
            return retvalue;
        }

        private IEnumerable<TAReport> GetTAreportsUsersNew(DateTime From, DateTime To, List<int> UserIds,int BuildingId)
        {
            IEnumerable<TAReport> retvalue = new List<TAReport>();
            IEnumerable<TAReport> retvalue1 = new List<TAReport>();
          
            if (UserIds != null && UserIds.Count > 0)
            {
                var str = String.Join(",", UserIds);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter("select id,UserId,DepartmentId,Name,ReportDate,Day,Hours,Hours_Min,Shift,Status,Completed,IsDeleted,Timestamp,ModifiedLast,ModifiedId,BuildingId,(select FirstName from Users where id=ta.UserId) as FirstName ,(select LastName from Users where id=ta.UserId) as LastName from TAReports ta where IsDeleted=0 and UserId in (" + str + ") and ReportDate>= '" + From.ToString("MM/dd/yyyy") + "' and ReportDate < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "' order by LastName", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach(DataRow dr in dt.Rows)
                {
                    if(dr["BuildingId"] == DBNull.Value)
                    {
                        int taReportsId = Convert.ToInt32(dr["id"]);
                        int userId = Convert.ToInt32(dr["UserId"]);
                        int buildingIdToAssign = db.TAReports.Where(y => y.UserId == userId && y.BuildingId.HasValue).Select(x => x.BuildingId).DefaultIfEmpty(0).Max().Value;
                        var nullBuildingIdTaReportsRecord = db.TAReports.SingleOrDefault(x => x.Id == taReportsId);
                        nullBuildingIdTaReportsRecord.BuildingId = buildingIdToAssign;
                        db.SaveChanges();

                    }
                }
                dt.Clear();
                da.Fill(dt);
                retvalue = (from DataRow dr in dt.Rows
                            select new TAReport()
                            {
                                Id = Convert.ToInt32(dr["id"]),
                                UserId = Convert.ToInt32(dr["UserId"]),
                                ReportDate = Convert.ToDateTime(dr["ReportDate"]),
                                Day = Convert.ToInt16(dr["Day"]),
                                Hours = Convert.ToInt32(dr["Hours"]),
                                Shift = Convert.ToInt32(dr["Shift"]),
                                Status = Convert.ToByte(dr["Status"]),
                                Completed = Convert.ToBoolean(dr["Completed"]),
                                IsDeleted = Convert.ToBoolean(dr["IsDeleted"]),
                                ModifiedLast = Convert.ToDateTime(dr["ModifiedLast"]),
                                ModifiedId = Convert.ToInt32(dr["ModifiedId"]),
                                BuildingId = Convert.ToInt32(dr["BuildingId"]),
                                FirstName = Convert.ToString(dr["FirstName"]),
                                LastName = Convert.ToString(dr["LastName"]),
                                Name = Convert.ToString(dr["Name"]),
                                Hours_Min = Convert.ToString(dr["Hours_Min"]),
                                FullName = Convert.ToString(dr["LastName"]) + " " + Convert.ToString(dr["FirstName"]),
                            });
                if(BuildingId != 0)
                {
                    retvalue = retvalue.Where(x => x.BuildingId == BuildingId);
                }
                if (retvalue.ToList().Count > 0)
                {
                    DataTable dtr = new DataTable();
                    dtr.Columns.Add("datestring");
                    List<DateTime> allDates = new List<DateTime>();
                    for (DateTime date = From; date <= To; date = date.AddDays(1))
                        allDates.Add(date.Date);
                    foreach (var obj in allDates)
                    {
                        DataRow row = dtr.NewRow();
                        row["datestring"] = obj.Date;
                        dtr.Rows.Add(row);
                    }

                    retvalue1 = (from DataRow dr in dtr.Rows
                                 select new TAReport()
                                 {
                                     ReportDate = Convert.ToDateTime(dr["datestring"]),
                                 });
                }
                con.Close();
                retvalue = retvalue.Concat(retvalue1);
            }
            return retvalue;
        }
        private IEnumerable<TAMove> GetUserTAMoves(string StartDate1, string StoptDate1, int UserId)
        {
            IEnumerable<TAMove> retvalue = new List<TAMove>();
            if (StartDate1 != null && StoptDate1 != null)
            {

                retvalue = _TAMoveRepository.FindAll(x => x.UserId == UserId && x.IsDeleted == false);
            }

            return retvalue;
        }
        private IEnumerable<TAMove> GetUsersTAMoves(DateTime From, DateTime To, List<int> UserIds)
        {
            IEnumerable<TAMove> retvalue = new List<TAMove>();
            if (UserIds != null)
            {
                retvalue = _TAMoveRepository.FindAll(x => !x.IsDeleted && x.Started >= From && x.Finished <= To && UserIds.Contains(x.UserId));
            }
            return retvalue;
        }

        private IEnumerable<TAMove> GetUsersTAMovesNew(DateTime From, DateTime To, List<int> UserIds)
        {
            IEnumerable<TAMove> retvalue = new List<TAMove>();
            if (UserIds != null && UserIds.Count > 0)
            {
                var str = String.Join(",", UserIds);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter("select id,UserId,Remark,Hours,Started,Finished,Hours_Min,ModifiedLast,(select FirstName from Users where id=ta.UserId) as FirstName ,(select LastName from Users where id=ta.UserId) as LastName from TAMoves ta where IsDeleted=0 and UserId in (" + str + ") and Started>= '" + From.ToString("MM/dd/yyyy") + "' and Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "'", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                retvalue = (from DataRow dr in dt.Rows
                            select new TAMove()
                            {
                                Id = Convert.ToInt32(dr["id"]),
                                UserId = Convert.ToInt32(dr["UserId"]),
                                Hours = Convert.ToInt32(dr["Hours"]),
                                FirstName = Convert.ToString(dr["FirstName"]),
                                LastName = Convert.ToString(dr["LastName"]),
                                Name = Convert.ToString(dr["Remark"]),
                                Hours_Min = Convert.ToString(dr["Hours_Min"]),
                                Started = Convert.ToDateTime(dr["Started"]),
                                Finished = Convert.ToDateTime(dr["Finished"]),
                            });
                con.Close();
            }
            return retvalue;
        }
        #endregion
        #region Class
        //TAMounthReportGridViewPartialView.ascx
        public class PivotGridFeaturesDemosHelper
        {

            static PivotGridSettings drillDownPivotGridSettings;
            public static PivotGridSettings DrillDownPivotGridSettings
            {
                get
                {
                    if (drillDownPivotGridSettings == null)
                        drillDownPivotGridSettings = CreateDrillDownPivotGridSettings();
                    return drillDownPivotGridSettings;
                }
            }
            static PivotGridSettings CreateDrillDownPivotGridSettings()
            {
                var settings = new PivotGridSettings();
                settings.Name = "pivotGrid";
                settings.CallbackRouteValues = new { Controller = "TAReport", Action = "TAMounthReportGridViewPartialA" };
                //settings.Width = Unit.Percentage(100);

                settings.Fields.Add(field =>
                {
                    field.Area = PivotArea.RowArea;
                    field.Caption = "Last Name";
                    field.FieldName = "UserName";

                });


                settings.Fields.Add(field =>
                {
                    field.Area = PivotArea.ColumnArea;

                    field.FieldName = "ReportDate";
                    field.Caption = "ReportDate";
                    field.GroupInterval = PivotGroupInterval.DateMonthYear;//.DateMonth;

                });

                settings.Fields.Add(field =>
                {
                    field.Area = PivotArea.ColumnArea;
                    field.FieldName = "ReportDate";
                    field.Caption = "Quarter";
                    field.GroupInterval = PivotGroupInterval.DateDay;

                });

                settings.Fields.Add(field =>
                {
                    field.Area = PivotArea.DataArea;
                    field.FieldName = "Hours";  //ViewResources.SharedStrings.TAHours;
                });
                return settings;
            }
        }
        #endregion

        public string ConvertToTime(double timeSeconds)
        {
            int mySeconds = System.Convert.ToInt32(timeSeconds);
            int myHours = mySeconds / 3600; //3600 Seconds in 1 hour
            mySeconds %= 3600;
            int myMinutes = mySeconds / 60; //60 Seconds in a minute
            mySeconds %= 60;
            string mySec = mySeconds.ToString(),
            myMin = myMinutes.ToString(),
            myHou = myHours.ToString();

            if (myHours < 10) { myHou = myHou.Insert(0, "0"); }
            if (myMinutes < 10) { myMin = myMin.Insert(0, "0"); }
            if (mySeconds < 10) { mySec = mySec.Insert(0, "0"); }
            return myHou + ":" + myMin;
        }

        public string ExtractNumbers(string inp)
        {
            string input = inp;

            // Match anything that is NOT a digit 
            string splitPattern = @"[^\d]";

            // Split approach: split on the pattern and exclude the match, hence the reverse logic of 
            // matching on anything that is NOT a digit 
            string[] results = Regex.Split(input, splitPattern);

            StringBuilder sb = new StringBuilder();

            foreach (string s in results)
            {
                sb.Append(s);
            }
            return sb.ToString();
        }

        #region TA Sepcial Reports 1
        public ActionResult TATaxReportExportN(int? department, int? company, string FromDateTA, string ToDateTA, int format, int? BuildingId, string BName)
        {
            try
            {
                Session.Remove("TAStartDate1");
                Session.Remove("TAStoptDate1");
                Session.Remove("timeFormat1");
                Session.Remove("department1");
                Session.Remove("company1");
                Session.Remove("BuildingName1");
                Session.Remove("Buidlidngid1");
                Session.Remove("UsersTA1");
            }
            catch
            {
            }

            DateTime From1 = DateTime.ParseExact(FromDateTA, "dd.MM.yyyy",
                                      System.Globalization.CultureInfo.InvariantCulture);

            string mon = From1.ToString("MM");
            string yr = From1.ToString("yyyy");

            var myDate = From1;
            DateTime From = new DateTime(myDate.Year, myDate.Month, 1);
            DateTime To = From.AddMonths(1).AddDays(-1);

            List<User> usrs = new List<User>();
            List<User> usrs1 = new List<User>();
            List<int> usr = new List<int>();
            List<int> dps = new List<int>();
            var ULVM = CreateViewModel<UserListViewModel>();
            var deptNm = "";
            var compID = CurrentUser.Get().CompanyId;
            Session["TAStartDate1"] = From.ToString("dd.MM.yyyy");
            Session["TAStoptDate1"] = To.ToString("dd.MM.yyyy");
            Session["timeFormat1"] = format;
            Session["department1"] = department;
            Session["company1"] = company;
            Session["BuildingName1"] = BName;
            Session["Buidlidngid1"] = BuildingId;
            if (department != null && department != 0)
            {
                deptNm = db.Departments.SingleOrDefault(x => x.Id == department).Name;
            }
            if (CurrentUser.Get().IsSuperAdmin || CurrentUser.Get().IsDepartmentManager || CurrentUser.Get().IsCompanyManager)
            {
                if (CurrentUser.Get().IsSuperAdmin)
                {
                    con.Open();
                    if ((company == null || company == 0) && (department == null || department == 0))
                    {
                        DataTable dt = new DataTable();
                        if (BuildingId == null || BuildingId == 0)
                        {
                            SqlDataAdapter da = new SqlDataAdapter("select id from Users where IsDeleted=0 and id in (select userid from TAMoves ta where IsDeleted=0 and Ta.Started >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "')", con);
                            da.Fill(dt);
                        }
                        else
                        {
                            SqlDataAdapter da = new SqlDataAdapter("select id from Users where IsDeleted=0 and id in (select userid from TAMoves ta where IsDeleted=0 and ta.FinishedBoId in (select id from BuildingObjects where IsDeleted=0 and BuildingId='" + BuildingId + "') and Ta.Started >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "')", con);
                            da.Fill(dt);
                        }
                        foreach (DataRow dr in dt.Rows)
                        {
                            usr.Add(Convert.ToInt32(dr["id"]));
                        }
                    }
                    else
                    {
                        DataTable dt = new DataTable();
                        if (department != null && department != 0)
                        {
                            if (BuildingId == null || BuildingId == 0)
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select distinct UserId as id from UserDepartments where IsDeleted=0  and UserId in (select userid from TAMoves ta where IsDeleted=0 and  Ta.Started >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "') and DepartmentId='" + department + "'", con);
                                da.Fill(dt);
                            }
                            else
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select distinct UserId as id from UserDepartments where IsDeleted=0  and UserId in (select userid from TAMoves ta where IsDeleted=0 and ta.FinishedBoId in (select id from BuildingObjects where IsDeleted=0 and BuildingId='" + BuildingId + "') and  Ta.Started >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "') and DepartmentId='" + department + "'", con);
                                da.Fill(dt);
                            }
                        }
                        else if (company != null && company != 0)
                        {
                            if (BuildingId == null || BuildingId == 0)
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select id from Users where IsDeleted=0 and id in (select userid from TAMoves ta where IsDeleted=0 and Ta.Started >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "') and companyid='" + company + "'", con);
                                da.Fill(dt);
                            }
                            else
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select id from Users where IsDeleted=0 and id in (select userid from TAMoves ta where IsDeleted=0  and ta.FinishedBoId in (select id from BuildingObjects where IsDeleted=0 and BuildingId='" + BuildingId + "') and Ta.Started >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "') and companyid='" + company + "'", con);
                                da.Fill(dt);
                            }
                        }
                        foreach (DataRow dr in dt.Rows)
                        {
                            usr.Add(Convert.ToInt32(dr["id"]));
                        }
                    }
                    con.Close();
                }
                else
                {
                    con.Open();
                    if (CurrentUser.Get().IsCompanyManager)
                    {
                        DataTable dt = new DataTable();
                        if (department != null && department != 0)
                        {
                            if (BuildingId == null || BuildingId == 0)
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select distinct UserId as id from UserDepartments where IsDeleted=0  and UserId in (select userid from TAMoves ta where IsDeleted=0 and  Ta.Started >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "') and DepartmentId='" + department + "'", con);
                                da.Fill(dt);
                            }
                            else
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select distinct UserId as id from UserDepartments where IsDeleted=0  and UserId in (select userid from TAMoves ta where IsDeleted=0 and ta.FinishedBoId in (select id from BuildingObjects where IsDeleted=0 and BuildingId='" + BuildingId + "') and  Ta.Started >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "') and DepartmentId='" + department + "'", con);
                                da.Fill(dt);
                            }
                        }
                        else
                        {
                            if (BuildingId == null || BuildingId == 0)
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select id from Users where IsDeleted=0 and id in (select userid from TAMoves ta where IsDeleted=0 and Ta.Started >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "') and companyid='" + company + "'", con);
                                da.Fill(dt);
                            }
                            else
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select id from Users where IsDeleted=0 and id in (select userid from TAMoves ta where IsDeleted=0  and ta.FinishedBoId in (select id from BuildingObjects where IsDeleted=0 and BuildingId='" + BuildingId + "') and Ta.Started >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "') and companyid='" + company + "'", con);
                                da.Fill(dt);
                            }
                        }
                        foreach (DataRow dr in dt.Rows)
                        {
                            usr.Add(Convert.ToInt32(dr["id"]));
                        }
                    }
                    else // if (CurrentUser.Get().IsDepartmentManager)
                    {
                        List<int> dep = _UserDepartmentRepository.FindByUserId(CurrentUser.Get().Id).Select(x => x.DepartmentId).ToList();
                        if (dep.Count > 0 && dep != null)
                        {
                            DataTable dt = new DataTable();
                            var str = String.Join(",", dep);
                            if (BuildingId == null || BuildingId == 0)
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select distinct UserId as id from UserDepartments where IsDeleted=0 and UserId in (select userid from TAMoves ta where IsDeleted=0 and  Ta.Started >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "')  and DepartmentId in(" + str + ")", con);
                                da.Fill(dt);
                            }
                            else
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select distinct UserId as id from UserDepartments where IsDeleted=0 and UserId in (select userid from TAMoves ta where IsDeleted=0 and ta.FinishedBoId in (select id from BuildingObjects where IsDeleted=0 and BuildingId='" + BuildingId + "')  and  Ta.Started >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "')  and DepartmentId in(" + str + ")", con);
                                da.Fill(dt);
                            }
                            foreach (DataRow dr in dt.Rows)
                            {
                                usr.Add(Convert.ToInt32(dr["id"]));
                            }
                        }
                    }
                    con.Close();
                }
            }

            if (usr.Count > 0)
            {
                usrs = _userRepository.FindAll(x => usr.Contains(x.Id) && !x.IsDeleted).ToList();
            }
            Mapper.Map(usrs, ULVM.Users);

            if (department != null)
            {
                foreach (var uu in ULVM.Users.ToList())
                {
                    uu.DepartmentName = deptNm;
                }
            }

            Session["TypedListModelTA1"] = ULVM;

            return PartialView("TATaxReportExportN", Session["TypedListModelTA1"]);
        }

        [ValidateInput(false)]
        public void TaxExportToParamsTA1(List<int> a)
        {
            Session["UsersTA1"] = a;
        }

        public ActionResult TATaxReportExportCallBackN()
        {
            try
            {
                var ULVM = CreateViewModel<UserListViewModel>();

                ULVM = (UserListViewModel)Session["TypedListModelTA1"];

                return PartialView("TATaxReportExportUsersN", ULVM.Users);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult ExportTo1()
        {
            string FromDateTA = Session["TAStartDate1"].ToString();
            DateTime From1 = DateTime.ParseExact(FromDateTA, "dd.MM.yyyy",
                                     System.Globalization.CultureInfo.InvariantCulture);

            string mon = From1.ToString("MM");
            string yr = From1.ToString("yyyy");

            var myDate = From1;
            DateTime From = new DateTime(myDate.Year, myDate.Month, 1);
            DateTime To = From.AddMonths(1).AddDays(-1);
            int? company = Convert.ToInt32(Session["company1"]);
            int? department = Convert.ToInt32(Session["department1"]);
            int? buildingId = Convert.ToInt32(Session["Buidlidngid1"]);
            List<int> UserIds = (List<int>)Session["UsersTA1"];

            if (CurrentUser.Get().IsCompanyManager) { company = CurrentUser.Get().CompanyId; }

            List<TAReportMounthItem> obj = new List<TAReportMounthItem>();

            var buildinglist = db.TABuildingName.Where(x => x.ValidFrom <= From && x.ValidTo >= From && x.ValidTo <= To || x.ValidTo >= To && x.ValidFrom <= To && x.ValidFrom >= From || x.ValidFrom <= From && x.ValidTo >= To || x.ValidFrom >= From && x.ValidTo <= To).ToList();
            buildinglist = buildinglist.Where(x => x.IsDeleted == false && x.BuildingId == buildingId).ToList();

            if (buildinglist.Count > 0)
            {
                Session["CadastralNr"] = buildinglist[0].BuildingLicense;
                string taxno = buildinglist[0].Customer;
                string txno = "";
                if (!string.IsNullOrEmpty(taxno))
                {
                    string[] arr = taxno.ToLower().Split(new string[] { "nr." }, StringSplitOptions.None);
                    if (arr != null && arr.Length > 1)
                    {
                        txno = arr[1].Trim();
                    }
                    //else
                    //{
                    //    txno = taxno;
                    //}
                }
                obj.Add(new TAReportMounthItem()
                {
                    ConstructorName = buildinglist[0].Name,
                    ContractAmount = buildinglist[0].Sum,
                    ConstructorTaxNo = txno,
                    ContractNo = buildinglist[0].CadastralNr,
                    ContractDate = buildinglist[0].ValidFrom.ToString("dd.MM.yyyy"),
                });
            }

            if (CurrentUser.Get().IsCompanyManager) { company = CurrentUser.Get().CompanyId; }

            List<TaNewUserDetails> tamvm = new List<TaNewUserDetails>();
            if (CurrentUser.Get().IsSuperAdmin || CurrentUser.Get().IsCompanyManager || CurrentUser.Get().IsDepartmentManager)
            {
                tamvm = ExportUserDetails();
            }
            int srno = 0;
            foreach (var obj1 in tamvm)
            {
                srno = srno + 1;
                obj1.SrNo = srno;
                var usrdet = _userRepository.FindById(obj1.UserId);
                obj1.FirstName = usrdet.FirstName;
                obj1.LastName = usrdet.LastName;
                obj1.PersonalCode = usrdet.PersonalCode;
                obj1.Birthday = usrdet.Birthday;
                if (obj1.Birthday != null)
                {
                    string birthday = Convert.ToString(obj1.Birthday);
                    if (birthday == "01/01/1900 00:00")
                    {
                        obj1.Birthdate = null;
                    }
                    else
                    {
                        DateTime birth = Convert.ToDateTime(obj1.Birthday);
                        obj1.Birthdate = (birth).ToString("dd.MM.yyyy");
                    }
                }
                obj1.Month = mon;
                obj1.Year = yr;
                var titlelist = db.Title.Where(x => x.Id == usrdet.TitleId).ToList();
                if (titlelist.Count > 0)
                {
                    obj1.Name = titlelist[0].Name;
                }
                else
                {
                    obj1.Name = "";
                }
                var complist = db.Companies.Where(x => x.Id == usrdet.CompanyId).ToList();
                if (complist.Count > 0)
                {
                    obj1.CompanyName = complist[0].Name;
                    obj1.RegistrationNo = complist[0].Comment;
                    if (!string.IsNullOrEmpty(obj1.RegistrationNo))
                    {
                        obj1.RegistrationNo = ExtractNumbers(obj1.RegistrationNo);
                    }
                }
                try
                {
                    obj1.ContractNo = Session["CadastralNr"].ToString();
                }
                catch
                {
                    obj1.ContractNo = "";
                }
                obj1.RprtDate = DateTime.Now.ToString("dd.MM.yyyy");
                if (!string.IsNullOrEmpty(obj1.Hours_Min))
                {
                    obj1.Hours_Min = obj1.Hours_Min.Split(':')[0] + ":" + obj1.Hours_Min.Split(':')[1];
                }
                if (!String.IsNullOrEmpty(usrdet.ExternalPersonalCode) && !String.IsNullOrEmpty(obj1.Birthdate))
                {
                    obj1.ExtPersonalCode = usrdet.ExternalPersonalCode + " - " + obj1.Birthdate;
                }
                else
                {
                    obj1.ExtPersonalCode = usrdet.ExternalPersonalCode + "  " + obj1.Birthdate;
                }
            }

            PrintingSystem ps = new PrintingSystem();

            PrintableComponentLink link1 = new PrintableComponentLink(ps);

            link1.Component = GridViewExtension.CreatePrintableObject(GetTaxReportHeader(), obj);
            link1.CreateReportHeaderArea += new CreateAreaEventHandler(Link_CreateMarginalHeaderAreaTax);
            link1.Landscape = true;

            PrintableComponentLink link2 = new PrintableComponentLink(ps);
            link2.Component = GridViewExtension.CreatePrintableObject(GetTaxReportDetails(), tamvm);
            link2.CreateReportHeaderArea += new CreateAreaEventHandler(Link_CreateMarginalHeaderAreaBlank);
            link2.Landscape = true;

            CompositeLink compositeLink = new CompositeLink(ps);

            compositeLink.Links.AddRange(new object[] { link1 });
            compositeLink.Landscape = true;
            compositeLink.CreateDocument();

            compositeLink.Links.AddRange(new object[] { link2 });
            compositeLink.Landscape = true;
            compositeLink.CreateDocument();

            FileStreamResult result = CreateExcelExportResult(compositeLink, 0, "TAReport1");

            string compname = "";
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString;
                SqlConnection myConnection = new SqlConnection(connectionString);
                myConnection.Open();
                SqlCommand cmd = new SqlCommand("select top 1 Value from classificatorvalues where ClassificatorId=(select id from Classificators where Description like '%T&A report main company name%')", myConnection);
                compname = Convert.ToString(cmd.ExecuteScalar());
                myConnection.Close();
            }
            catch
            {

            }
            string filename = "";
            try
            {
                filename = compname + "_" + (buildinglist[0].Name).Replace("\"", "") + "_" + From.ToString("ddMMyy") + "_to_" + To.ToString("ddMMyy");
            }
            catch
            {
                filename = compname + "_" + From.ToString("ddMMyy") + "_to_" + To.ToString("ddMMyy");
            }
            result.FileDownloadName = filename + "_1.xls";

            ps.Dispose();
            return result;

        }

        public List<TaNewUserDetails> ExportUserDetails()
        {
            string countryflag = "";
            string StartDate = (string)Session["TAStartDate1"];
            string StoptDate = (string)Session["TAStoptDate1"];
            List<int> UserIds = (List<int>)Session["UsersTA1"];
            string dept = "";

            int department = Convert.ToInt32(Session["department1"]);
            int company = Convert.ToInt32(Session["company1"]);
            int building_id = Convert.ToInt32(Session["Buidlidngid1"]);

            DateTime From = DateTime.ParseExact(StartDate, "dd.MM.yyyy",
                          System.Globalization.CultureInfo.InvariantCulture);
            DateTime To = DateTime.ParseExact(StoptDate, "dd.MM.yyyy",
                                           System.Globalization.CultureInfo.InvariantCulture);

            if (department != 0)
            {
                dept = db.Departments.SingleOrDefault(x => x.Id == department).Name;
            }
            if (Session["Language"] == null)
            {
                countryflag = "EN";
            }
            else
            {
                countryflag = Session["Language"].ToString();
            }
            //Detailed Report-Old...GridView
            var tamvm = CreateViewModel<TAMsUserViewModel>();

            TaReportViewNewCustomoseUser tavm = new TaReportViewNewCustomoseUser();
            List<TaNewUserDetails> listta_user = new List<TaNewUserDetails>();

            int i = 0;
            List<itembuilding> Building_Data = new List<itembuilding>();
            var ctvm = new BuildingNameViewModel();

            if (building_id != 0)
            {
                if (UserIds != null && UserIds.Count > 0)
                {
                    DateTime Ton = To.AddDays(1);
                    var result2 = (from Ta in db.NewTaMoves
                                   join U in db.User on Ta.UserId equals U.Id
                                   join C in db.Companies on U.CompanyId equals C.Id
                                   join b in db.BuildingObject on Ta.FinishedBoId equals b.Id
                                   join os in db.Title on U.TitleId equals os.Id into t
                                   from rt in t.DefaultIfEmpty()
                                   where Ta.Started >= From && Ta.Finished < Ton
                                   && UserIds.Contains(Ta.UserId) && Ta.IsDeleted == false && b.BuildingId == building_id
                                   select new
                                   {
                                       companyname = C.Name,
                                       C.Comment,
                                       Ta.UserId,
                                       U.FirstName,
                                       U.LastName,
                                       U.CompanyId,
                                       U.PersonalCode,
                                       rt.Name,
                                       U.Id,
                                       Ta.Started,
                                       Ta.Finished,
                                       U.Birthday
                                   }).OrderBy(x => x.Started).ToList();         //OrderBy(x => x.Id).ToList();

                    int temp2 = 0;
                    int temp3 = 0;
                    int m = 0;
                    foreach (var items in result2)
                    {
                        m = m + 1;
                        //var compcount = result2.Where(x => x.CompanyId == items.CompanyId).Count();
                        DateTime date = items.Started;
                        string onlydate = date.ToString("dd.MM.yyyy");
                        DateTime st = items.Started;
                        string checkin1 = st.ToString("HH:mm");

                        DateTime fn = items.Finished;
                        string checkout1 = fn.ToString("HH:mm");

                        TimeSpan checkin = date.TimeOfDay;
                        TimeSpan checkout = fn.TimeOfDay;
                        var tottaltime = checkout.Subtract(checkin);
                        string hh = tottaltime.Hours < 10 ? "0" + tottaltime.Hours : tottaltime.Hours.ToString();
                        string mm = tottaltime.Minutes < 10 ? "0" + tottaltime.Minutes : tottaltime.Minutes.ToString();
                        string rr = hh + ":" + mm;

                        var timeDiff = tottaltime.TotalSeconds;
                        double sec = timeDiff;

                        i = i + 1;
                        int k = 0; k++; int p = 0; p++; int n = 0; n++;
                        Session["FullNmae"] = items.FirstName + " " + items.LastName;
                        if (temp2 != items.UserId)
                        {
                            temp2 = items.UserId;
                            k = 0;
                        }
                        if (temp3 != items.CompanyId)
                        {
                            temp3 = Convert.ToInt32(items.CompanyId);
                            p = 0;
                        }

                        string companydeatils1 = items.companyname + "  " + items.Comment;
                        listta_user.Add(new TaNewUserDetails()
                        {
                            id = i,
                            UserId = items.UserId,
                            CompnyId = Convert.ToInt32(items.CompanyId),
                            FirstName = items.FirstName,
                            LastName = items.LastName,
                            companydeatils = companydeatils1,
                            PersonalCode = items.PersonalCode,
                            Name = items.Name,
                            Started = items.Started,
                            checkin = checkin1,
                            checkout = checkout1,
                            Hours = sec,
                            totaltime = rr,
                            FullName = items.FirstName + " " + items.LastName,
                            Birthday = items.Birthday,
                            ReportDate = DateTime.Now.ToString("dd.MM.yyyy"),
                            SrNo = m,
                            Day = (items.Started).ToString("dd"),
                            Month = (items.Started).ToString("MM"),
                            Year = (items.Started).ToString("yyyy"),
                            Finished = items.Finished,
                        });
                    }
                    ViewBag.Result = result2;
                    tavm.TaUserDetails = listta_user;
                }
            }

            IEnumerable<TaNewUserDetails> retvaluen = new List<TaNewUserDetails>();
            IEnumerable<TaNewUserDetails> retvalue = new List<TaNewUserDetails>();
            IEnumerable<TaNewUserDetails> retvalue2 = new List<TaNewUserDetails>();
            List<TaNewUserDetails> retvalue1 = new List<TaNewUserDetails>();
            List<int> dps = new List<int>();
            retvalue = listta_user;

            IEnumerable<TaNewUserDetails> userids = new List<TaNewUserDetails>();
            retvaluen = retvalue;
            retvaluen = retvaluen.OrderBy(x => x.UserId);
            int uid = 0;

            foreach (var obj in retvaluen)
            {
                IEnumerable<TaNewUserDetails> userid = new List<TaNewUserDetails>();
                if (uid == 0)
                {
                    uid = obj.UserId;
                    userid = new List<TaNewUserDetails>
                    {
                        new TaNewUserDetails { UserId = uid,DepartmentId=obj.DepartmentId,Status=obj.Status,Completed=obj.Completed,ModifiedLast=obj.ModifiedLast},
                    };
                }
                else if (uid != obj.UserId)
                {
                    uid = obj.UserId;
                    userid = new List<TaNewUserDetails>
                    {
                        new TaNewUserDetails { UserId = uid,DepartmentId=obj.DepartmentId,Status=obj.Status,Completed=obj.Completed,ModifiedLast=obj.ModifiedLast},
                    };
                }
                userids = userids.Concat(userid);
            }
            double totalsec = 0;
            int selid = 0;

            foreach (var obj in userids)
            {
                if (selid == 0)
                {
                    selid = obj.UserId;
                }
                else if (selid != obj.UserId)
                {
                    selid = obj.UserId;
                    totalsec = 0;
                }
                foreach (var obj1 in retvaluen)
                {
                    if (selid == obj1.UserId)
                    {
                        DateTime date = obj1.Started;
                        string onlydate = date.ToString("dd.MM.yyyy");
                        DateTime st = obj1.Started;
                        string checkin1 = st.ToString("HH:mm");

                        DateTime fn = Convert.ToDateTime(obj1.Finished);
                        string checkout1 = fn.ToString("HH:mm");

                        TimeSpan checkin = date.TimeOfDay;
                        TimeSpan checkout = fn.TimeOfDay;
                        var tottaltime = checkout.Subtract(checkin);
                        string hh = tottaltime.Hours < 10 ? "0" + tottaltime.Hours : tottaltime.Hours.ToString();
                        string mm = tottaltime.Minutes < 10 ? "0" + tottaltime.Minutes : tottaltime.Minutes.ToString();
                        string rr = hh + ":" + mm;

                        var timeDiff = tottaltime.TotalSeconds;
                        double sec = timeDiff;
                        totalsec = totalsec + sec;
                    }
                }
                int? tc = retvaluen.Where(x => x.UserId == selid).Select(y => y.Started.ToString("dd.MM.yyyy")).Distinct().ToList().Count();
                string tsec = ConvertToTime(Convert.ToDouble(totalsec));
                obj.Hours_Min = tsec;
                obj.ReportDate = DateTime.Now.Date.ToString();
                obj.TotalWorkingDays = tc;
            }
            retvaluen = userids;
            return retvaluen.ToList();
        }

        private GridViewSettings GetTaxReportHeader()
        {

            string FromDateTA = Session["TAStartDate1"].ToString();
            DateTime From1 = DateTime.ParseExact(FromDateTA, "dd.MM.yyyy",
                                     System.Globalization.CultureInfo.InvariantCulture);
            string mon = From1.ToString("MM");
            string yr = From1.ToString("yyyy");

            var settings = new GridViewSettings();
            settings.Name = "gvmain1";
            settings.SettingsExport.PaperKind = System.Drawing.Printing.PaperKind.A4;
            settings.Width = System.Web.UI.WebControls.Unit.Percentage(100);
            settings.ControlStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            settings.Settings.ShowFilterRow = true;
            settings.SettingsExport.Styles.Header.Font.Bold = true;
            settings.SettingsExport.Styles.Header.Font.Size = 10;
            settings.SettingsExport.Styles.Header.ForeColor = System.Drawing.Color.Black;
            settings.SettingsExport.Styles.Header.BackColor = System.Drawing.Color.White;
            settings.SettingsExport.Styles.AlternatingRowCell.BackColor = System.Drawing.ColorTranslator.FromHtml("#DEEBF6");
            settings.SettingsText.EmptyDataRow = " ";
            settings.EnableTheming = true;
            settings.Width = Unit.Percentage(100);
            settings.SettingsPager.PageSize = 50;
            settings.SettingsBehavior.AllowSelectByRowClick = true;
            settings.Styles.AlternatingRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#DEEBF6");
            settings.Styles.SelectedRow.BackColor = System.Drawing.Color.LightCyan;
            settings.Width = Unit.Percentage(100);
            settings.Settings.ShowGroupPanel = true;
            settings.Styles.GroupPanel.HorizontalAlign = HorizontalAlign.Center;
            settings.Styles.GroupPanel.Wrap = DefaultBoolean.True;
            settings.SettingsBehavior.AllowSort = false;
            settings.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
            settings.Styles.Row.Font.Size = 10;
            settings.Columns.AddBand(cons =>
            {
                cons.Columns.AddBand(constructor =>
                {
                    constructor.Caption = ViewResources.SharedStrings.ContractorDetails;
                    constructor.Columns.Add(c =>
                    {
                        c.Caption = ViewResources.SharedStrings.ConstructionInitiator;
                        c.FieldName = "ConstructorName";
                        c.HeaderStyle.Wrap = DefaultBoolean.True;
                        c.Width = Unit.Pixel(60);
                        c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    });
                    constructor.Columns.Add(c =>
                    {
                        c.Caption = ViewResources.SharedStrings.ConstructionInitiatorTaxNo;
                        c.FieldName = "ConstructorTaxNo";
                        c.HeaderStyle.Wrap = DefaultBoolean.True;
                        c.Width = 100;
                        c.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    });
                    constructor.Columns.Add(c1 =>
                    {
                        c1.Caption = ViewResources.SharedStrings.ContractNr;
                        c1.FieldName = "ContractDate";
                        c1.HeaderStyle.Wrap = DefaultBoolean.True;
                        c1.Width = Unit.Pixel(60);
                        c1.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    });
                    constructor.Columns.Add(c1 =>
                    {
                        c1.Caption = ViewResources.SharedStrings.ConstructionAmount;
                        c1.FieldName = "ContractAmount";
                        c1.HeaderStyle.Wrap = DefaultBoolean.True;
                        c1.Width = Unit.Pixel(60);
                        c1.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    });
                    constructor.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    constructor.HeaderStyle.Wrap = DefaultBoolean.True;
                    constructor.Width = Unit.Pixel(240);
                });
            });
            return settings;
        }

        private void Link_CreateMarginalHeaderAreaTax(object sender, CreateAreaEventArgs e)
        {
            string FromDateTA = Session["TAStartDate1"].ToString();
            DateTime From1 = DateTime.ParseExact(FromDateTA, "dd.MM.yyyy",
                                     System.Globalization.CultureInfo.InvariantCulture);

            string mon = From1.ToString("MM");
            string yr = From1.ToString("yyyy");

            string pageHeader = "";
            StringBuilder sb = new StringBuilder();
            string compname = "";
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString;
                SqlConnection myConnection = new SqlConnection(connectionString);
                myConnection.Open();
                SqlCommand cmd = new SqlCommand("select top 1 Value from classificatorvalues where ClassificatorId=(select id from Classificators where Description like '%T&A report main company name%')", myConnection);
                compname = Convert.ToString(cmd.ExecuteScalar());
                myConnection.Close();
            }
            catch
            {
            }
            sb.AppendLine(string.Format(ViewResources.SharedStrings.ElectronicWorking.Replace("{0}", compname) + " " + yr + ", " + mon + ViewResources.SharedStrings.YearMonth));

            pageHeader = sb.ToString();
            DevExpress.XtraPrinting.TextBrick brick;
            SizeF textSize = e.Graph.MeasureString(pageHeader, 1300, StringFormat.GenericDefault);
            brick = e.Graph.DrawString(pageHeader, System.Drawing.Color.Black, new RectangleF(0, 0, 1300, textSize.Height + 30), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new System.Drawing.Font("Times New Roman", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Center);
            brick.VertAlignment = DevExpress.Utils.VertAlignment.Center;
            brick.HorzAlignment = DevExpress.Utils.HorzAlignment.Center;
        }

        private void Link_CreateMarginalHeaderAreaBlank(object sender, CreateAreaEventArgs e)
        {
            string pageHeader = "";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" ");
            pageHeader = sb.ToString();
            DevExpress.XtraPrinting.TextBrick brick;
            SizeF textSize = e.Graph.MeasureString(pageHeader, 1300, StringFormat.GenericDefault);
            brick = e.Graph.DrawString(pageHeader, System.Drawing.Color.Black, new RectangleF(0, 0, 1300, textSize.Height + 7), DevExpress.XtraPrinting.BorderSide.None);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Center);
            brick.VertAlignment = DevExpress.Utils.VertAlignment.Center;
            brick.HorzAlignment = DevExpress.Utils.HorzAlignment.Center;
        }

        private GridViewSettings GetTaxReportDetails()
        {
            var settings = new GridViewSettings();
            settings.Name = "gvBands";
            settings.SettingsExport.PaperKind = System.Drawing.Printing.PaperKind.A4;
            settings.Width = System.Web.UI.WebControls.Unit.Percentage(100);
            settings.ControlStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            settings.Settings.ShowFilterRow = true;
            settings.SettingsExport.Styles.Header.Font.Bold = true;
            settings.SettingsExport.Styles.Header.Font.Size = 10;
            settings.SettingsExport.Styles.Header.ForeColor = System.Drawing.Color.Black;
            settings.SettingsExport.Styles.Header.BackColor = System.Drawing.Color.White;
            settings.SettingsExport.Styles.AlternatingRowCell.BackColor = System.Drawing.ColorTranslator.FromHtml("#DEEBF6");
            settings.SettingsText.EmptyDataRow = " ";
            settings.EnableTheming = true;
            settings.Width = Unit.Percentage(100);
            settings.Settings.ShowGroupPanel = false;
            settings.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
            settings.SettingsPager.PageSize = 50;
            settings.SettingsBehavior.AllowSelectByRowClick = true;
            settings.Styles.AlternatingRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#DEEBF6");
            settings.Styles.SelectedRow.BackColor = System.Drawing.Color.LightCyan;
            settings.Styles.Row.Font.Size = 10;
            settings.Columns.Add(c =>
            {
                c.Caption = ViewResources.SharedStrings.Num;
                c.FieldName = "SrNo";
                c.HeaderStyle.Wrap = DefaultBoolean.True;
            });

            settings.Columns.Add(c =>
            {
                c.Caption = ViewResources.SharedStrings.InfoSentDate;
                c.FieldName = "RprtDate";
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 100;
                c.ExportWidth = 100;
            });

            settings.Columns.Add(c =>
            {
                c.Caption = ViewResources.SharedStrings.TaxYear;
                c.FieldName = "Year";
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 60;
                c.ExportWidth = 60;
            });

            settings.Columns.Add(c =>
            {
                c.Caption = ViewResources.SharedStrings.TaxMonth;
                c.FieldName = "Month";
                c.HeaderStyle.Wrap = DefaultBoolean.True;
                c.Width = 60;
                c.ExportWidth = 60;
            });

            settings.Columns.AddBand(info =>
            {
                info.Caption = ViewResources.SharedStrings.PersonInformation;
                info.Columns.AddBand(empdet =>
                {
                    empdet.Caption = ViewResources.SharedStrings.Employee;
                    empdet.Columns.Add(c =>
                    {
                        c.Caption = ViewResources.SharedStrings.Name;
                        c.FieldName = "FirstName";
                        c.HeaderStyle.Wrap = DefaultBoolean.True;
                    });
                    empdet.Columns.Add(c =>
                    {
                        c.Caption = ViewResources.SharedStrings.SurName;
                        c.FieldName = "LastName";
                        c.HeaderStyle.Wrap = DefaultBoolean.True;
                    });

                    empdet.Columns.Add(c =>
                    {
                        c.Caption = ViewResources.SharedStrings.PersonalIdentificationNumber;
                        c.FieldName = "PersonalCode";
                        c.HeaderStyle.Wrap = DefaultBoolean.True;
                    });
                    empdet.Columns.AddBand(c =>
                    {
                        c.Caption = ViewResources.SharedStrings.NoPersonalIdentificationNumber;
                        c.HeaderStyle.Wrap = DefaultBoolean.True;
                        c.Columns.Add(c1 =>
                        {
                            c1.Caption = ViewResources.SharedStrings.BirthdayDateMonYear;
                            c1.FieldName = "ExtPersonalCode";
                            c1.HeaderStyle.Wrap = DefaultBoolean.True;
                            c.Width = 100;
                            c.ExportWidth = 100;
                        });
                    });
                    empdet.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                });

                info.Columns.AddBand(initialdates =>
                {
                    initialdates.Caption = ViewResources.SharedStrings.Employer;
                    initialdates.Columns.Add(c =>
                    {
                        c.Caption = ViewResources.SharedStrings.Position;
                        c.FieldName = "Name";
                        c.HeaderStyle.Wrap = DefaultBoolean.True;
                    });
                    initialdates.Columns.Add(c =>
                    {
                        c.Caption = ViewResources.SharedStrings.EmployerName;
                        c.FieldName = "CompanyName";
                        c.HeaderStyle.Wrap = DefaultBoolean.True;
                    });
                    initialdates.Columns.Add(c =>
                    {
                        c.Caption = ViewResources.SharedStrings.IndiEmployer;
                        c.HeaderStyle.Wrap = DefaultBoolean.True;
                        c.Width = 100;
                        c.ExportWidth = 100;
                    });
                    initialdates.Columns.Add(c =>
                    {
                        c.Caption = ViewResources.SharedStrings.RegNo;
                        c.FieldName = "RegistrationNo";
                        c.HeaderStyle.Wrap = DefaultBoolean.True;
                        c.Width = 100;
                        c.ExportWidth = 100;
                    });
                    initialdates.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                });

                info.Columns.Add(c1 =>
                {
                    c1.Caption = ViewResources.SharedStrings.BuildingPermission;
                    c1.FieldName = "ContractNo";
                    c1.HeaderStyle.Wrap = DefaultBoolean.True;
                    c1.Width = 100;
                    c1.ExportWidth = 100;
                });

                info.Columns.Add(c2 =>
                {
                    c2.Caption = ViewResources.SharedStrings.ActualWorkingHour;
                    c2.FieldName = "Hours_Min";
                    c2.HeaderStyle.Wrap = DefaultBoolean.True;
                    c2.Width = 100;
                    c2.ExportWidth = 100;
                });
                info.Columns.Add(c2 =>
                {
                    c2.Caption = ViewResources.SharedStrings.TotalWorkingDays;
                    c2.FieldName = "TotalWorkingDays";
                    c2.HeaderStyle.Wrap = DefaultBoolean.True;
                    c2.Width = 100;
                    c2.ExportWidth = 100;
                });
                info.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            });
            return settings;
        }
        #endregion

        #region TA Sepcial Reports 2

        [HttpGet]
        public ActionResult TATaxReportExport(int? department, int? company, string FromDateTA, string ToDateTA, int format, int? BuildingId, string BName)
        {
            try
            {
                Session.Remove("TAStartDate2");
                Session.Remove("TAStoptDate2");
                Session.Remove("timeFormat2");
                Session.Remove("department2");
                Session.Remove("company2");
                Session.Remove("BuildingName2");
                Session.Remove("Buidlidngid2");
                Session.Remove("UsersTA2");
            }
            catch
            {
            }

            DateTime From1 = DateTime.ParseExact(FromDateTA, "dd.MM.yyyy",
                                      System.Globalization.CultureInfo.InvariantCulture);
            string mon = From1.ToString("MM");
            string yr = From1.ToString("yyyy");

            var myDate = From1;
            DateTime From = new DateTime(myDate.Year, myDate.Month, 1);
            DateTime To = From.AddMonths(1).AddDays(-1);

            List<User> usrs = new List<User>();
            List<User> usrs1 = new List<User>();
            List<int> usr = new List<int>();
            List<int> dps = new List<int>();
            var ULVM = CreateViewModel<UserListViewModel>();
            var deptNm = "";
            var compID = CurrentUser.Get().CompanyId;
            Session["TAStartDate2"] = From.ToString("dd.MM.yyyy");
            Session["TAStoptDate2"] = To.ToString("dd.MM.yyyy");
            Session["timeFormat2"] = format;
            Session["department2"] = department;
            Session["company2"] = company;
            Session["BuildingName2"] = BName;
            Session["Buidlidngid2"] = BuildingId;
            if (department != null && department != 0)
            {
                deptNm = db.Departments.SingleOrDefault(x => x.Id == department).Name;
            }
            if (CurrentUser.Get().IsSuperAdmin || CurrentUser.Get().IsDepartmentManager || CurrentUser.Get().IsCompanyManager)
            {
                if (CurrentUser.Get().IsSuperAdmin)
                {
                    con.Open();
                    if ((company == null || company == 0) && (department == null || department == 0))
                    {
                        DataTable dt = new DataTable();
                        if (BuildingId == null || BuildingId == 0)
                        {
                            SqlDataAdapter da = new SqlDataAdapter("select id from Users where IsDeleted=0 and id in (select userid from TAMoves ta where IsDeleted=0 and Ta.Started >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "')", con);
                            da.Fill(dt);
                        }
                        else
                        {
                            SqlDataAdapter da = new SqlDataAdapter("select id from Users where IsDeleted=0 and id in (select userid from TAMoves ta where IsDeleted=0 and ta.FinishedBoId in (select id from BuildingObjects where IsDeleted=0 and BuildingId='" + BuildingId + "') and Ta.Started >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "')", con);
                            da.Fill(dt);
                        }
                        foreach (DataRow dr in dt.Rows)
                        {
                            usr.Add(Convert.ToInt32(dr["id"]));
                        }
                    }
                    else
                    {
                        DataTable dt = new DataTable();
                        if (department != null && department != 0)
                        {
                            if (BuildingId == null || BuildingId == 0)
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select distinct UserId as id from UserDepartments where IsDeleted=0  and UserId in (select userid from TAMoves ta where IsDeleted=0 and  Ta.Started >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "') and DepartmentId='" + department + "'", con);
                                da.Fill(dt);
                            }
                            else
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select distinct UserId as id from UserDepartments where IsDeleted=0  and UserId in (select userid from TAMoves ta where IsDeleted=0 and ta.FinishedBoId in (select id from BuildingObjects where IsDeleted=0 and BuildingId='" + BuildingId + "') and  Ta.Started >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "') and DepartmentId='" + department + "'", con);
                                da.Fill(dt);
                            }
                        }
                        else if (company != null && company != 0)
                        {
                            if (BuildingId == null || BuildingId == 0)
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select id from Users where IsDeleted=0 and id in (select userid from TAMoves ta where IsDeleted=0 and Ta.Started >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "') and companyid='" + company + "'", con);
                                da.Fill(dt);
                            }
                            else
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select id from Users where IsDeleted=0 and id in (select userid from TAMoves ta where IsDeleted=0  and ta.FinishedBoId in (select id from BuildingObjects where IsDeleted=0 and BuildingId='" + BuildingId + "') and Ta.Started >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "') and companyid='" + company + "'", con);
                                da.Fill(dt);
                            }
                        }
                        foreach (DataRow dr in dt.Rows)
                        {
                            usr.Add(Convert.ToInt32(dr["id"]));
                        }
                    }
                    con.Close();
                }
                else
                {
                    con.Open();
                    if (CurrentUser.Get().IsCompanyManager)
                    {
                        DataTable dt = new DataTable();
                        if (department != null && department != 0)
                        {
                            if (BuildingId == null || BuildingId == 0)
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select distinct UserId as id from UserDepartments where IsDeleted=0  and UserId in (select userid from TAMoves ta where IsDeleted=0 and  Ta.Started >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "') and DepartmentId='" + department + "'", con);
                                da.Fill(dt);
                            }
                            else
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select distinct UserId as id from UserDepartments where IsDeleted=0  and UserId in (select userid from TAMoves ta where IsDeleted=0 and ta.FinishedBoId in (select id from BuildingObjects where IsDeleted=0 and BuildingId='" + BuildingId + "') and  Ta.Started >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "') and DepartmentId='" + department + "'", con);
                                da.Fill(dt);
                            }
                        }
                        else
                        {
                            if (BuildingId == null || BuildingId == 0)
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select id from Users where IsDeleted=0 and id in (select userid from TAMoves ta where IsDeleted=0 and Ta.Started >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "') and companyid='" + company + "'", con);
                                da.Fill(dt);
                            }
                            else
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select id from Users where IsDeleted=0 and id in (select userid from TAMoves ta where IsDeleted=0  and ta.FinishedBoId in (select id from BuildingObjects where IsDeleted=0 and BuildingId='" + BuildingId + "') and Ta.Started >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "') and companyid='" + company + "'", con);
                                da.Fill(dt);
                            }
                        }
                        foreach (DataRow dr in dt.Rows)
                        {
                            usr.Add(Convert.ToInt32(dr["id"]));
                        }
                    }
                    else // if (CurrentUser.Get().IsDepartmentManager)
                    {
                        List<int> dep = _UserDepartmentRepository.FindByUserId(CurrentUser.Get().Id).Select(x => x.DepartmentId).ToList();
                        if (dep.Count > 0 && dep != null)
                        {
                            DataTable dt = new DataTable();
                            var str = String.Join(",", dep);
                            if (BuildingId == null || BuildingId == 0)
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select distinct UserId as id from UserDepartments where IsDeleted=0 and UserId in (select userid from TAMoves ta where IsDeleted=0 and  Ta.Started >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "')  and DepartmentId in(" + str + ")", con);
                                da.Fill(dt);
                            }
                            else
                            {
                                SqlDataAdapter da = new SqlDataAdapter("select distinct UserId as id from UserDepartments where IsDeleted=0 and UserId in (select userid from TAMoves ta where IsDeleted=0 and ta.FinishedBoId in (select id from BuildingObjects where IsDeleted=0 and BuildingId='" + BuildingId + "')  and  Ta.Started >= '" + From.ToString("MM/dd/yyyy") + "' and Ta.Finished < '" + To.AddDays(1).ToString("MM/dd/yyyy") + "')  and DepartmentId in(" + str + ")", con);
                                da.Fill(dt);
                            }
                            foreach (DataRow dr in dt.Rows)
                            {
                                usr.Add(Convert.ToInt32(dr["id"]));
                            }
                        }
                    }
                    con.Close();
                }
            }

            if (usr.Count > 0)
            {
                usrs = _userRepository.FindAll(x => usr.Contains(x.Id) && !x.IsDeleted).ToList();
            }
            Mapper.Map(usrs, ULVM.Users);

            if (department != null)
            {
                foreach (var uu in ULVM.Users.ToList())
                {
                    uu.DepartmentName = deptNm;
                }
            }
            Session["TypedListModelTA2"] = ULVM;
            return PartialView("TATaxReportExport", Session["TypedListModelTA2"]);
        }

        [ValidateInput(false)]
        public void TaxExportToParamsTA2(List<int> a)
        {
            Session["UsersTA2"] = a;
        }

        [ValidateInput(false)]
        public ActionResult TATaxReportExportCallBackTA2()
        {
            try
            {
                var ULVM = CreateViewModel<UserListViewModel>();
                ULVM = (UserListViewModel)Session["TypedListModelTA2"];
                return PartialView("TATaxReportExportUsers", ULVM.Users);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult ExportTo2()
        {
            string countryflag = "";
            string StartDate = (string)Session["TAStartDate2"];
            string StoptDate = (string)Session["TAStoptDate2"];
            List<int> UserIds = (List<int>)Session["UsersTA2"];
            string dept = "";

            int department = Convert.ToInt32(Session["department2"]);
            int company = Convert.ToInt32(Session["company2"]);
            int building_id = Convert.ToInt32(Session["Buidlidngid2"]);
            DateTime From = DateTime.ParseExact(StartDate, "dd.MM.yyyy",
                          System.Globalization.CultureInfo.InvariantCulture);
            DateTime To = DateTime.ParseExact(StoptDate, "dd.MM.yyyy",
                                           System.Globalization.CultureInfo.InvariantCulture);
            if (department != 0)
            {
                dept = db.Departments.SingleOrDefault(x => x.Id == department).Name;
            }
            if (Session["Language"] == null)
            {
                countryflag = "EN";
            }
            else
            {
                countryflag = Session["Language"].ToString();
            }
            //Detailed Report-Old...GridView
            var tamvm = CreateViewModel<TAMsUserViewModel>();

            TaReportViewNewCustomoseUser tavm = new TaReportViewNewCustomoseUser();
            List<TaNewUserDetails> listta_user = new List<TaNewUserDetails>();
            int i = 0;
            List<itembuilding> Building_Data = new List<itembuilding>();
            var ctvm = new BuildingNameViewModel();

            if (building_id == 0)
            {
                if (UserIds != null && UserIds.Count > 0)
                {
                    DateTime Ton = To.AddDays(1);
                    var result1 = (from Ta in db.NewTaMoves
                                   join U in db.User on Ta.UserId equals U.Id
                                   join C in db.Companies on U.CompanyId equals C.Id
                                   join os in db.Title on U.TitleId equals os.Id into t
                                   from rt in t.DefaultIfEmpty()
                                   where Ta.Started >= From && Ta.Finished < Ton
                                   && UserIds.Contains(Ta.UserId) && Ta.IsDeleted == false
                                   //group new { Ta, U, C, rt } by new { Ta.UserId } into grp
                                   select new
                                   {
                                       companyname = C.Name,
                                       C.Comment,
                                       Ta.UserId,
                                       U.FirstName,
                                       U.LastName,
                                       U.CompanyId,
                                       U.PersonalCode,
                                       rt.Name,
                                       U.Id,
                                       Ta.Started,
                                       Ta.Finished,
                                       U.Birthday,
                                       U.ExternalPersonalCode
                                   }).OrderBy(x => x.Started).ToList();  //OrderBy(x => x.Id).ToList();

                    int temp = 0;
                    int temp1 = 0;
                    var comps1 = result1.Select(x => x.CompanyId).Distinct();
                    int m = 0;
                    foreach (var items in result1)
                    {
                        m = m + 1;
                        DayOfWeek dow = items.Started.DayOfWeek;

                        DateTime date = items.Started;
                        string onlydate = date.ToString("dd/mm/yyyy");
                        DateTime st = items.Started;

                        string checkin1 = st.ToString("HH:mm:ss");

                        DateTime fn = items.Finished;
                        string checkout1 = fn.ToString("HH:mm:ss");

                        TimeSpan checkin = date.TimeOfDay;
                        TimeSpan checkout = fn.TimeOfDay;
                        var tottaltime = checkout.Subtract(checkin);
                        string hh = tottaltime.Hours < 10 ? "0" + tottaltime.Hours : tottaltime.Hours.ToString();
                        string mm = tottaltime.Minutes < 10 ? "0" + tottaltime.Minutes : tottaltime.Minutes.ToString();
                        string rr = hh + ":" + mm;

                        var timeDiff = tottaltime.TotalSeconds;
                        double sec = timeDiff;

                        i = i + 1;
                        int k = 0; k++; int p = 0; p++;
                        Session["FullNmae"] = items.FirstName + " " + items.LastName;
                        if (temp != items.UserId)
                        {
                            temp = items.UserId;
                            k = 0;
                        }
                        if (temp1 != items.CompanyId)
                        {
                            temp1 = Convert.ToInt32(items.CompanyId);
                            p = 0;
                        }
                        string companydeatils1 = items.companyname + "  " + items.Comment;

                        listta_user.Add(new TaNewUserDetails()
                        {
                            id = i,
                            UserId = items.UserId,
                            CompnyId = Convert.ToInt32(items.CompanyId),
                            FirstName = items.FirstName,
                            LastName = items.LastName,
                            companydeatils = companydeatils1,
                            PersonalCode = items.PersonalCode,
                            Name = items.Name,
                            Started = items.Started,
                            checkin = checkin1,
                            checkout = checkout1,
                            Hours = sec,
                            totaltime = rr,
                            FullName = items.FirstName + " " + items.LastName,
                            Birthday = items.Birthday,
                            ReportDate = DateTime.Now.ToString("dd.MM.yyyy"),
                            SrNo = m,
                            Day = (items.Started).ToString("dd"),
                            Month = (items.Started).ToString("MM"),
                            Year = (items.Started).ToString("yyyy"),
                            ExtPersonalCode = items.ExternalPersonalCode
                        });
                    }
                    ViewBag.Result = result1;
                    tavm.TaUserDetails = listta_user;
                }
            }
            else
            {
                if (building_id != 0)
                {
                    if (UserIds != null && UserIds.Count > 0)
                    {
                        DateTime Ton = To.AddDays(1);
                        var result2 = (from Ta in db.NewTaMoves
                                       join U in db.User on Ta.UserId equals U.Id
                                       join C in db.Companies on U.CompanyId equals C.Id
                                       join b in db.BuildingObject on Ta.FinishedBoId equals b.Id
                                       join os in db.Title on U.TitleId equals os.Id into t
                                       from rt in t.DefaultIfEmpty()
                                       where Ta.Started >= From && Ta.Finished < Ton
                                       && UserIds.Contains(Ta.UserId) && Ta.IsDeleted == false && b.BuildingId == building_id

                                       select new
                                       {
                                           companyname = C.Name,
                                           C.Comment,
                                           Ta.UserId,
                                           U.FirstName,
                                           U.LastName,
                                           U.CompanyId,
                                           U.PersonalCode,
                                           rt.Name,
                                           U.Id,
                                           Ta.Started,
                                           Ta.Finished,
                                           U.Birthday,
                                           U.ExternalPersonalCode
                                       }).OrderBy(x => x.Started).ToList();         //OrderBy(x => x.Id).ToList();

                        int temp2 = 0;
                        int temp3 = 0;
                        int m = 0;
                        foreach (var items in result2)
                        {
                            m = m + 1;
                            //var compcount = result2.Where(x => x.CompanyId == items.CompanyId).Count();
                            DateTime date = items.Started;
                            string onlydate = date.ToString("dd.MM.yyyy");
                            DateTime st = items.Started;
                            string checkin1 = st.ToString("HH:mm:ss");

                            DateTime fn = items.Finished;
                            string checkout1 = fn.ToString("HH:mm:ss");

                            TimeSpan checkin = date.TimeOfDay;
                            TimeSpan checkout = fn.TimeOfDay;
                            var tottaltime = checkout.Subtract(checkin);
                            string hh = tottaltime.Hours < 10 ? "0" + tottaltime.Hours : tottaltime.Hours.ToString();
                            string mm = tottaltime.Minutes < 10 ? "0" + tottaltime.Minutes : tottaltime.Minutes.ToString();
                            string rr = hh + ":" + mm;

                            var timeDiff = tottaltime.TotalSeconds;
                            double sec = timeDiff;

                            i = i + 1;
                            int k = 0; k++; int p = 0; p++; int n = 0; n++;
                            Session["FullNmae"] = items.FirstName + " " + items.LastName;
                            if (temp2 != items.UserId)
                            {
                                temp2 = items.UserId;
                                k = 0;
                            }
                            if (temp3 != items.CompanyId)
                            {
                                temp3 = Convert.ToInt32(items.CompanyId);
                                p = 0;
                            }

                            string companydeatils1 = items.companyname + "  " + items.Comment;
                            listta_user.Add(new TaNewUserDetails()
                            {
                                id = i,
                                UserId = items.UserId,
                                CompnyId = Convert.ToInt32(items.CompanyId),
                                FirstName = items.FirstName,
                                LastName = items.LastName,
                                companydeatils = companydeatils1,
                                PersonalCode = items.PersonalCode,
                                Name = items.Name,
                                Started = items.Started,
                                checkin = checkin1,
                                checkout = checkout1,
                                Hours = sec,
                                totaltime = rr,
                                FullName = items.FirstName + " " + items.LastName,
                                Birthday = items.Birthday,
                                ReportDate = DateTime.Now.ToString("dd.MM.yyyy"),
                                SrNo = m,
                                Day = (items.Started).ToString("dd"),
                                Month = (items.Started).ToString("MM"),
                                Year = (items.Started).ToString("yyyy"),
                                ExtPersonalCode = items.ExternalPersonalCode
                            });
                        }

                        ViewBag.Result = result2;
                        tavm.TaUserDetails = listta_user;
                    }
                }
            }

            foreach (var obj1 in tavm.TaUserDetails)
            {
                if (obj1.Birthday != null)
                {
                    string birthday = Convert.ToString(obj1.Birthday);
                    if (birthday == "01/01/1900 00:00")
                    {
                        obj1.Birthdate = null;
                    }
                    else
                    {
                        DateTime birth = Convert.ToDateTime(obj1.Birthday);
                        obj1.Birthdate = (birth).ToString("dd.MM.yyyy");
                    }
                }
                if (!String.IsNullOrEmpty(obj1.ExtPersonalCode) && !String.IsNullOrEmpty(obj1.Birthdate))
                {
                    obj1.ExtPersonalCode = obj1.ExtPersonalCode + " - " + obj1.Birthdate;
                }
                else
                {
                    obj1.ExtPersonalCode = obj1.ExtPersonalCode + "  " + obj1.Birthdate;
                }
            }

            XlsExportOptionsEx exportOptions = new XlsExportOptionsEx();
            exportOptions.CustomizeSheetHeader += options_CustomizeSheetHeader;
            CustomSummaryEventArgs custsumm = new CustomSummaryEventArgs();
            exportOptions.ExportType = ExportType.WYSIWYG;
            exportOptions.CustomizeCell += new DevExpress.Export.CustomizeCellEventHandler(exportOptions_CustomizeCell);
            return GridViewExtension.ExportToXls(GetWorkingDaysReport(), tavm.TaUserDetails, exportOptions);
        }

        private GridViewSettings GetWorkingDaysReport()
        {
            string compname = "";
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString;
                SqlConnection myConnection = new SqlConnection(connectionString);
                myConnection.Open();
                SqlCommand cmd = new SqlCommand("select top 1 Value from classificatorvalues where ClassificatorId=(select id from Classificators where Description like '%T&A report main company name%')", myConnection);
                compname = Convert.ToString(cmd.ExecuteScalar());
                myConnection.Close();
            }
            catch
            {
                compname = "";
            }
            string filename = "";
            try
            {
                string StartDate = (string)Session["TAStartDate2"];
                string StoptDate = (string)Session["TAStoptDate2"];
                string builname = "";
                DateTime From = DateTime.ParseExact(StartDate, "dd.MM.yyyy",
                       System.Globalization.CultureInfo.InvariantCulture);
                DateTime To = DateTime.ParseExact(StoptDate, "dd.MM.yyyy",
                                               System.Globalization.CultureInfo.InvariantCulture);
                int buildingid = Convert.ToInt32(Session["Buidlidngid2"]);
                if (buildingid > 0)
                {
                    var buildinglist = db.TABuildingName.Where(x => x.ValidFrom <= From && x.ValidTo >= From && x.ValidTo <= To || x.ValidTo >= To && x.ValidFrom <= To && x.ValidFrom >= From || x.ValidFrom <= From && x.ValidTo >= To || x.ValidFrom >= From && x.ValidTo <= To).ToList();
                    buildinglist = buildinglist.Where(x => x.IsDeleted == false && x.BuildingId == buildingid).ToList();

                    if (buildinglist.Count > 0)
                    {
                        builname = buildinglist[0].Name;
                    }

                    filename = compname + "_" + (builname).Replace("\"", "") + "_" + From.ToString("ddMMyyyy") + "_to_" + To.ToString("ddMMyyyy");
                }
                else
                {
                    filename = compname + "_" + From.ToString("ddMMyyyy") + "_to_" + To.ToString("ddMMyyyy");
                }
            }
            catch
            {
                filename = compname;
            }

            string FromDateTA = Session["TAStartDate2"].ToString();
            DateTime From1 = DateTime.ParseExact(FromDateTA, "dd.MM.yyyy",
                                     System.Globalization.CultureInfo.InvariantCulture);

            string mon = From1.ToString("MM");
            string yr = From1.ToString("yyyy");
            var settings = new GridViewSettings();
            settings.SettingsExport.FileName = filename + "_2.xls";
            settings.SettingsExport.PaperKind = System.Drawing.Printing.PaperKind.A4;
            settings.Width = System.Web.UI.WebControls.Unit.Percentage(100);
            settings.ControlStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            settings.KeyFieldName = "id";
            settings.Settings.ShowFilterRow = true;
            settings.SettingsExport.Styles.Header.Font.Bold = true;
            settings.SettingsExport.Styles.Header.ForeColor = System.Drawing.Color.Black;
            settings.SettingsExport.Styles.Header.BackColor = System.Drawing.Color.White;
            settings.SettingsExport.Styles.AlternatingRowCell.BackColor = System.Drawing.ColorTranslator.FromHtml("#DEEBF6");
            settings.SettingsText.EmptyDataRow = " ";
            settings.Name = "gvBands";
            settings.EnableTheming = true;
            settings.Width = Unit.Percentage(100);
            settings.SettingsPager.PageSize = 50;
            settings.SettingsBehavior.AllowSelectByRowClick = true;
            settings.Styles.AlternatingRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#DEEBF6");
            settings.Styles.SelectedRow.BackColor = System.Drawing.Color.LightCyan;

            settings.Columns.AddBand(cap =>
            {
                cap.Caption = ViewResources.SharedStrings.InfoFromElectronic.Replace("{0}", compname) + " " + yr + ", " + mon + " " + ViewResources.SharedStrings.YearMonth;
                cap.HeaderStyle.Wrap = DefaultBoolean.True;
                cap.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;

                cap.Columns.Add(c1 =>
                {
                    c1.Caption = ViewResources.SharedStrings.Num;
                    c1.FieldName = "SrNo";
                });

                cap.Columns.Add(c =>
                {
                    c.Caption = ViewResources.SharedStrings.InfoDateSent;
                    c.FieldName = "ReportDate";
                    c.HeaderStyle.Wrap = DefaultBoolean.True;
                    c.Width = 100;
                    c.ExportWidth = 100;
                });

                cap.Columns.AddBand(info =>
                {
                    info.Caption = ViewResources.SharedStrings.TaxationPeriod;

                    info.Columns.Add(c1 =>
                    {
                        c1.Caption = ViewResources.SharedStrings.Date;
                        c1.HeaderStyle.Wrap = DefaultBoolean.True;
                        c1.FieldName = "Day";
                    });

                    info.Columns.Add(c2 =>
                    {
                        c2.Caption = ViewResources.SharedStrings.Month;
                        c2.HeaderStyle.Wrap = DefaultBoolean.True;
                        c2.FieldName = "Month";
                    });

                    info.Columns.Add(c2 =>
                    {
                        c2.Caption = ViewResources.SharedStrings.Year;
                        c2.HeaderStyle.Wrap = DefaultBoolean.True;
                        c2.FieldName = "Year";
                    });

                    info.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                });

                cap.Columns.AddBand(band1 =>
                {
                    band1.Caption = ViewResources.SharedStrings.CorrecteddatesAudit;
                    band1.Columns.AddBand(empdet =>
                    {
                        empdet.Caption = ViewResources.SharedStrings.Employee;
                        empdet.Columns.Add(c =>
                        {
                            c.Caption = ViewResources.SharedStrings.Name;
                            c.FieldName = "FirstName";
                            c.HeaderStyle.Wrap = DefaultBoolean.True;
                        });

                        empdet.Columns.Add(c =>
                        {
                            c.Caption = ViewResources.SharedStrings.SurName;
                            c.FieldName = "LastName";
                            c.HeaderStyle.Wrap = DefaultBoolean.True;
                        });

                        empdet.Columns.Add(c =>
                        {
                            c.Caption = ViewResources.SharedStrings.PersonalIdentificationNumber;
                            c.FieldName = "PersonalCode";
                            c.HeaderStyle.Wrap = DefaultBoolean.True;
                        });
                        empdet.Columns.AddBand(c =>
                        {
                            c.Caption = ViewResources.SharedStrings.NoPersonalIdentificationNumber;
                            c.HeaderStyle.Wrap = DefaultBoolean.True;

                            c.Columns.Add(c1 =>
                            {
                                c1.Caption = ViewResources.SharedStrings.BirthdayDateMonYear;
                                c1.FieldName = "ExtPersonalCode";
                                c1.HeaderStyle.Wrap = DefaultBoolean.True;
                            });
                        });
                        empdet.Columns.Add(c =>
                        {
                            c.Caption = ViewResources.SharedStrings.Position;
                            c.FieldName = "Name";
                            c.HeaderStyle.Wrap = DefaultBoolean.True;
                        });
                        empdet.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    });

                    band1.Columns.AddBand(initialdates =>
                    {
                        initialdates.Caption = ViewResources.SharedStrings.Initialdates;
                        initialdates.Columns.Add(c =>
                        {
                            c.Caption = ViewResources.SharedStrings.Entry;
                            c.FieldName = "checkin";
                            c.HeaderStyle.Wrap = DefaultBoolean.True;
                            c.Width = 70;
                            c.ExportWidth = 70;
                        });
                        initialdates.Columns.Add(c =>
                        {
                            c.Caption = ViewResources.SharedStrings.Exit;
                            c.FieldName = "checkout";
                            c.HeaderStyle.Wrap = DefaultBoolean.True;
                            c.Width = 70;
                            c.ExportWidth = 70;
                        });
                        initialdates.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    });
                    band1.Columns.AddBand(correcteddates =>
                    {
                        correcteddates.Caption = ViewResources.SharedStrings.Correcteddates;
                        correcteddates.Columns.Add(c =>
                        {
                            c.Caption = ViewResources.SharedStrings.EntryCorrected;
                            c.HeaderStyle.Wrap = DefaultBoolean.True;
                            c.Width = 70;
                            c.ExportWidth = 70;
                        });
                        correcteddates.Columns.Add(c =>
                        {
                            c.Caption = ViewResources.SharedStrings.ExitCorrected;
                            c.HeaderStyle.Wrap = DefaultBoolean.True;
                            c.Width = 70;
                            c.ExportWidth = 70;
                        });
                        correcteddates.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    });
                    band1.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                });

                cap.Columns.AddBand(c =>
                {
                    c.Caption = ViewResources.SharedStrings.Explanation;
                    c.HeaderStyle.Wrap = DefaultBoolean.True;
                });
            });
            return settings;
        }
        #endregion

        public static IEnumerable<Tuple<string, int>> MonthsBetween(DateTime startDate, DateTime endDate)
        {
            DateTime iterator;
            DateTime limit;

            if (endDate > startDate)
            {
                iterator = new DateTime(startDate.Year, startDate.Month, 1);
                limit = endDate;
            }
            else
            {
                iterator = new DateTime(endDate.Year, endDate.Month, 1);
                limit = startDate;
            }

            var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;
            while (iterator <= limit)
            {
                yield return Tuple.Create(
                    dateTimeFormat.GetMonthName(iterator.Month),
                    iterator.Year);
                iterator = iterator.AddMonths(1);
            }
        }


        //ExportToComingLeaving
        #region export coming and leaving report

        [ValidateInput(false)]
        public void ExportToParamsComingLeaving(int Reporttype, int ReportFormat)
        {
            Session["ReporttypeCL"] = Reporttype;
            Session["ReportFormatCL"] = ReportFormat;
        }

        public ActionResult ExportToComingLeaving()
        {
            int? Reporttype = (int?)Session["ReporttypeCL"];
            int? ReportFormat = (int?)Session["ReportFormatCL"];

            PrintingSystem ps1 = new PrintingSystem();
            PrintableComponentLink link3 = new PrintableComponentLink(ps1);
            link3.Component = PivotGridExtension.CreatePrintableObject(GetComingLeavingReport(), Session["UserMovesDetails"]);

            link3.Landscape = true;

            CompositeLink compositeLink1 = new CompositeLink(ps1);
            compositeLink1.Links.AddRange(new object[] { link3 });
            compositeLink1.Landscape = true;
            compositeLink1.CreateDocument();
            FileStreamResult result = CreateExcelExportResultCL(compositeLink1, 0, "Report");
            ps1.Dispose();
            return result;
        }


        private PivotGridSettings GetComingLeavingReport()
        {
            var settings = new PivotGridSettings();
            settings.Name = "pivotGrid";
            settings.CallbackRouteValues = new { Controller = "TAReport", Action = "TAStartEnd" };
            // settings.CustomActionRouteValues = new { Controller = "TAReport", Action = "MounthBatchGridEditA" };

            //settings.OptionsView.VerticalScrollingMode = PivotScrollingMode.Virtual;
            ////settings.OptionsView.HorizontalScrollingMode = PivotScrollingMode.Virtual;
            //settings.OptionsView.VerticalScrollBarMode = ScrollBarMode.Auto;
            //settings.OptionsView.HorizontalScrollBarMode = ScrollBarMode.Auto;

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
            return settings;
        }

        protected FileStreamResult CreateExcelExportResultCL(CompositeLink link, int ExtType, string Reptype)
        {
            string contenttype = "", ext = ".xls";
            //string contenttype = "", ext = ".xlx";

            MemoryStream stream = new MemoryStream();
            link.CreateDocument(false);
            BrickEnumerator brickEnum = link.PrintingSystem.Document.Pages[0].GetEnumerator();
            float maxWidth = 0;
            while (brickEnum.MoveNext())
                maxWidth = maxWidth > brickEnum.CurrentBrick.Rect.X + brickEnum.CurrentBrick.Rect.Width ? maxWidth : brickEnum.CurrentBrick.Rect.X + brickEnum.CurrentBrick.Rect.Width;
            float freeSpace = (link.PrintingSystem.Document.Pages[0]).PageSize.Width - maxWidth;
            if (freeSpace > 600)
            {
                link.Margins.Left = (int)(freeSpace / 6);
                link.CreateDocument(false);
            }

            if (ExtType == 0)
            {
                link.PrintingSystem.ExportToXls(stream);
                contenttype = "application/xls";
                ext = ".xls";
            }
            else
            {
                link.PrintingSystem.ExportToPdf(stream);
                contenttype = "application/pdf";
                ext = ".pdf";
            }

            stream.Position = 0;
            FileStreamResult result = new FileStreamResult(stream, contenttype);

            result.FileDownloadName = "coming_leaving_report" + ext;
            return result;
        }
        #endregion

        public ActionResult SendReportToVedludb(string from, string to, int? company, int? department, int? building)
        {
            
            var userRole = CurrentUser.Get().RoleTypeId;
            //int logEntryCount = 0;
            try
            {
                if (userRole == Convert.ToInt32(FixedRoleType.CompanyManager))
                {
                    if (building.HasValue && company.HasValue)
                  { 
                    var companyRegistrationNumber = db.Companies.Where(x => x.Id == company.Value).Select(y => y.Comment).FirstOrDefault();
                    var buildingRegistrationNumber = db.TABuildingName.Where(x => x.IsDeleted == false && x.Id == building.Value).Select(y => y.Contractor).FirstOrDefault();
                    if (Regex.Match(companyRegistrationNumber, "[0-9]{11}").Value.Trim() != Regex.Match(buildingRegistrationNumber, "[0-9]{11}").Value.Trim())
                    {
                        return Json(new { msg = "Registration number of the company does not match with the registration number of the building.", IsSucceed = false });
                    }
                  }
                }
            }
            catch
            {

            }
           
            var edlusCertificate = _classificatorRepository.FindAll(x => x.Description.ToLower() == "foxsec edlus").Select(y => y.Comments).FirstOrDefault();
            var foxSecEdlusId = _classificatorRepository.FindAll(x => x.Description.ToLower() == "foxsec edlus").Select(y=> y.Id).FirstOrDefault();
            var edlusCertifikateList = _classificatorValueRepository.FindAll(x => x.ClassificatorId == foxSecEdlusId).Select(y => y.Value).ToList();
            int? companyIdForLog;
            if (company.HasValue)
            {
                companyIdForLog = company.Value;
            }
            else
            {
                companyIdForLog = null;
            }
            if (building == null)
            {
                return Json(new { msg = "Please select building.", IsSucceed = false });
            }
            var buildingLicense1 = db.TABuildingName.FirstOrDefault(x => x.IsDeleted == false && x.BuildingId == building).BuildingLicense;
            if(edlusCertifikateList.Count > 0)
            {

            }
            else
            {
                return Json(new { msg = "No integration certificate found",IsSucceed = false });
            }
            VedludbModel vedludb = new VedludbModel();
            var url = "";
            
            DateTime From = DateTime.ParseExact(from, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            DateTime To = DateTime.ParseExact(to, "dd.MM.yyyy",

                System.Globalization.CultureInfo.InvariantCulture);

            edlusCertifikateList.ForEach(x => { if (x.Contains("http")) { url = x; } else { url = "https://bis.gov.lv/vedludb/api/ws/"; } });
            IList<VedludbModel> listOfAllConstructionSitesCertificate=null;
            try
            {
                foreach (var item in edlusCertifikateList)
                {
                    if (item.Contains("http") == false)
                    {
                        using (var http = new HttpClient())
                        {

                            var authorizationKey = item;
                            List<BuvniecibasLietasArray> buvniecibasLietas = new List<BuvniecibasLietasArray>();
                            
                            http.BaseAddress = new Uri(url);
                          
                            http.DefaultRequestHeaders.Add("Authorization", authorizationKey);
                            var sendRequest = http.GetAsync("Buvlaukumi");
                            sendRequest.Wait();
                            if (sendRequest.Result.IsSuccessStatusCode == true)
                            {
                                var result = sendRequest.Result;
                                var valueFromVedludb = result.Content.ReadAsAsync<IList<VedludbModel>>();
                                valueFromVedludb.Wait();
                                listOfAllConstructionSitesCertificate = valueFromVedludb.Result;
                                listOfAllConstructionSitesCertificate.ForEach(x => x.BuvniecibasLietas.ForEach(y => { if (y.buvatlaujasNr.ToLower() == buildingLicense1.ToLower().Trim() || y.bisBuvniecibasLietasNr.ToLower() == buildingLicense1.ToLower().Trim()) { vedludb.sertifikats = x.sertifikats; } }));
                            }

                        }
                    }
                }
               
                var buvniecibasLietasalues = listOfAllConstructionSitesCertificate.Select(x => x.BuvniecibasLietas).ToList();
                List<BuvniecibasLietasArray> buvniecibasLietasArray = new List<BuvniecibasLietasArray>();

                if (vedludb.sertifikats == null)
                {
                    return Json(new { msg = "Building license for the selected building does not match with building permit no in eldus", IsSucceed = false });
                }

                VedludbPostModel vedludbPostModel = new VedludbPostModel();
                
                vedludbPostModel = GetVedludbReportData(From, To, company, department, building.Value, out string usersNotSended, out string usersCount , out string buildingNameForLog);
                if (vedludbPostModel == null || vedludbPostModel.nodarbinatie.Count == 0)
                {
                    return Json(new { msg = "No data present in the report to send", IsSucceed = false });
                }
                
                var currentUserId = CurrentUser.Get().Id;
                var ipAddress = Request.UserHostAddress;
                var buildingName = db.TABuildingName.FirstOrDefault(x => x.IsDeleted == false && x.BuildingId == building).Name;
                if(buildingNameForLog.Length > 49)
                {
                    buildingNameForLog = buildingNameForLog.Substring(0, 48);
                }
                using (var httpPost = new HttpClient())
                {
                    
                    var tamvm = CreateViewModel<TAReportMounthViewModel>();
                    JsonConvert.DeserializeObject<VedludbPostModel>("");

                    string vedludbJsonValue = JsonConvert.SerializeObject(vedludbPostModel, Newtonsoft.Json.Formatting.Indented);
                    
                    httpPost.DefaultRequestHeaders.Clear();
                    
                    httpPost.BaseAddress = new Uri(url);
                    
                    httpPost.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", vedludb.sertifikats);
                    httpPost.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                
                    var resultpostAsync = httpPost.PostAsync("Atskaites", new StringContent(vedludbJsonValue, Encoding.UTF8, "application/json")).Result;

                    if (resultpostAsync.IsSuccessStatusCode)
                    {
                        if(usersNotSended != "" && usersNotSended != null)
                        {
                            
                            _logService.CreateLog(currentUserId, buildingNameForLog, "log", ipAddress, companyIdForLog, "Report saved to EDLUS. Total " + vedludbPostModel.nodarbinatie.Count + "users report data sent out of " + usersCount + " users." + usersNotSended + " Users reports not sended.Please check logs for user names that are not sended.", Convert.ToInt32(LogTypeEnum.CreditInfo));
                            return Json(new { msg = "Report saved to EDLUS. Total " + vedludbPostModel.nodarbinatie.Count + "users report data sent out of " + usersCount + " users." + usersNotSended + " Users reports not sended.Please check logs for user names that are not sended.", IsSucceed = true });
                        }
                        else
                        {
                            _logService.CreateLog(currentUserId, buildingNameForLog, "log", ipAddress, companyIdForLog, "Report saved to EDLUS. Total " + vedludbPostModel.nodarbinatie.Count + "users report data sent out of " + usersCount + " users.", Convert.ToInt32(LogTypeEnum.CreditInfo));
                            return Json(new { msg = "Report saved to EDLUS. Total " + vedludbPostModel.nodarbinatie.Count + "users report data sent out of " + usersCount + " users.", IsSucceed = true });
                        }
                        
                    }
                    else
                    {
                        _logService.CreateLog(currentUserId, buildingNameForLog, "log", ipAddress, companyIdForLog, "Some data error.Report not saved to EDLUS. (" + resultpostAsync.ReasonPhrase + ")", Convert.ToInt32(LogTypeEnum.Errors));
                        return Json(new { msg = "Some data error.Report not saved to EDLUS. (" + resultpostAsync.ReasonPhrase + ")", IsSucceed = false });
                    }
                    
                }
                
            }
            
            catch (Exception ex)
            {

                return Json(new {msg = "Report not saved to EDLUS.Exception(" + ex.Message + ")",IsSucceed = false});
            }
        }

        private VedludbPostModel GetVedludbReportData(DateTime from, DateTime to, int? company, int? department, int building, out string usersNotSended, out string usersCount, out string buildingNameForLog)
        {
            
            try
            {
                int? companyIdForLog;
                if (company.HasValue)
                {
                    companyIdForLog = company.Value;
                }
                else
                {
                    companyIdForLog = null;
                }
                var result = db.TABuildingName.FirstOrDefault(x => x.IsDeleted == false && x.BuildingId == building);
                     buildingNameForLog = result.Name;
                    var buildingName = result.BuildingLicense;
                    usersNotSended = "";
                    var users = _userRepository.FindAll(x => !x.IsDeleted).ToList();
                    
                    var reportbyDate = _taReportRepository.FindAll(x => x.ReportDate >= from && x.ReportDate <= to).Select(y => y.UserId).ToList();
                    var rep = _taReportRepository.FindAll(x => x.ReportDate >= from && x.ReportDate <= to).ToList();
                    var reportByBuilding = rep.Where(x => x.BuildingId.HasValue && x.BuildingId.Value == building).ToList();
                    var userIdInReports = reportByBuilding.Where(x => x.User != null).Select(y => y.User.Id).ToList();
                    userIdInReports = reportByBuilding.Where(x => x.User != null).Select(z => z.User.Id).Distinct().ToList();

                    var toCompareUserName = reportByBuilding.Where(x => x.User != null).Select(v => new { FirstName = v.User.FirstName }).Distinct().ToList();

                    var taMove = _TAMoveRepository.FindAll(x => x.Started >= from && x.Started <= to && x.User != null && userIdInReports.Contains(x.User.Id)).ToList();
                    taMove = taMove.Where(x => x.User.WorkTime.Value == true && x.Started != null && x.Finished != null && x.User.Company.Comment != null && x.Hours_Min != null && x.User.Company !=null && x.User != null).ToList();
                    usersCount = userIdInReports.Count.ToString();

                
                var nodarbinatieListOneByOne = new List<nodarbinatieArray>();
                foreach (var userId in userIdInReports)
                {
                    
                    
                    var totalWorkingHours = taMove.Where(x =>x.User != null && x.User.Company != null && x.Started != null && x.Finished != null && x.User.Id == userId).GroupBy(y => y.User.Id).Select(z => new { totalHours = z.Sum(r => r.Hours) }).FirstOrDefault();
                    var totalHoursTimeConvert = "";
                    string totalWorkingTime = "";
                    if (totalWorkingHours != null)
                    { 
                        totalHoursTimeConvert = TimeSpan.FromSeconds(Convert.ToDouble(totalWorkingHours.totalHours)).ToString();
                        DateTime dateTime = DateTime.UtcNow;
                        TimeSpan span = TimeSpan.FromSeconds(totalWorkingHours.totalHours);
                        totalWorkingTime = String.Format("{0:D3}:{1:D2}:{2:D2}", (span.Days * 24 + span.Hours), span.Minutes, span.Seconds);
                   }
                    else
                    {
                        totalWorkingTime = "00:00:00";
                    }

                    try
                    {
                        var dienasList1 = taMove.Where(p => p.User.Id == userId && p.Hours_Min.LastIndexOf(":") != p.Hours_Min.Length - 1).Select(x => new dienasArray { datums = x.Started.Date.ToString("yyyy-MM-dd"), summaraisDiena = x.Hours_Min.ToString() }).ToList<dienasArray>().OrderBy(x => x.datums).ToList();
                        var ierakstiList1 = taMove.Where(p => p.User.Id == userId).Select(x => new ierakstiArray { ierasanasLaiks = x.Started.Date.ToString("yyyy-MM-dd") + "T" + x.Started.TimeOfDay.ToString().Substring(0, 8), iziesanasLaiks = x.Finished.Value.Date.ToString("yyyy-MM-dd") + "T" + x.Finished.Value.TimeOfDay.ToString().Substring(0, 8), manualsIerasanasIeraksts = "false", manualsIziesanasIeraksts = "false", autonomsIerasanasIeraksts = "false", autonomsIziesanasIeraksts = "false" }).ToList<ierakstiArray>();

                        var nodarbinatieList = taMove.Where(p => p.User.PersonalCode != null && p.User.PersonalCode != "" && p.User.Company.Comment != null && p.User.Company.Comment != "" && p.User.Id == userId && p.User.Title.Name != null && p.User.Title != null).Select(x => new nodarbinatieArray { vards = x.User.FirstName, uzvards = x.User.LastName, personasKods = x.User.PersonalCode, darbaDevejaNosaukums = x.User.Company.Name, darbaDevejaRegistracijasNumurs = Regex.Match(x.User.Company.Comment, "[0-9]{11}", RegexOptions.IgnoreCase).Value, arvalstuDarbaDevejs = "false", summaraisMenesa = totalWorkingTime, amats = x.User.Title.Name, dienas = dienasList1, ieraksti = ierakstiList1 }).FirstOrDefault();

                        nodarbinatieListOneByOne.Add(nodarbinatieList);
                    }

                    catch
                    {

                    }
                    

                }

                
                nodarbinatieListOneByOne.RemoveAll(item => item == null || item.darbaDevejaRegistracijasNumurs == null || item.darbaDevejaRegistracijasNumurs == "");
                var compareNotSendedUsers = nodarbinatieListOneByOne.Select(x => x.vards).Distinct().ToList();
                var usersNotSended1 = "";
                toCompareUserName.ForEach(x => { if (compareNotSendedUsers.Contains(x.FirstName) == false) { usersNotSended1 = usersNotSended1 + x.FirstName + " ,"; } });
                usersNotSended = usersNotSended1;

                var dienasList = taMove.Select(x => new dienasArray { datums = x.Started.Date.ToString("yyyy-MM-dd"), summaraisDiena = x.Hours_Min.ToString() }).ToList<dienasArray>().OrderBy(x => x.datums).ToList();
                 
                    var ierakstiList = taMove.Select(x => new ierakstiArray { ierasanasLaiks = x.Started.Date.ToString("yyyy-MM-dd") + "T" + x.Started.TimeOfDay.ToString().Substring(0, 8), iziesanasLaiks = x.Finished.Value.Date.ToString("yyyy-MM-dd") + "T" + x.Finished.Value.TimeOfDay.ToString().Substring(0, 8), manualsIerasanasIeraksts = "false", manualsIziesanasIeraksts = "false", autonomsIerasanasIeraksts = "false", autonomsIziesanasIeraksts = "false" }).ToList<ierakstiArray>();
                  
                    
                 
                    var companyCommentContainingEmployerRegistrationNumber = taMove.Where(x => x.User.Company.Comment != null).Select(x => x.User.Company.Comment).ToList();
                   
                 var vedluDpPostModel1 = new VedludbPostModel { menesis = from.ToString("yyyy-MM"), callbackUrl = "https://webhook.edlus/qwerty", nodarbinatie = nodarbinatieListOneByOne };
                
                
                return vedluDpPostModel1;
                   
                
               

            }
            catch
            {
                usersNotSended = "";
                usersCount = "";
                buildingNameForLog = "";
                return null;
            }
           

            }

        //[HttpGet]
        //public ActionResult TaGroupShifts()
        //{
        //    var taUserGroupShifts = db.TaUserGroupeShifts.ToList();
        //    TaShiftsModel taShiftsModelList = new TaShiftsModel();
        //    var schedulerData = new List<SchedulerData>();
        //    schedulerData.Add(new SchedulerData { text = "Ranjan", startDate = new DateTime(2020,10,04,9,30,00).ToString(), endDate = "2020-10-04T12:30:00.000" });
        //    schedulerData.Add(new SchedulerData { text = "Ranjan2", startDate = "2020-10-05T08:30:00.000", endDate = "2020-10-05T10:30:00.000" });
        //    schedulerData.Add(new SchedulerData { text = "Ranjan3", startDate = "2020-10-05T10:30:00.000", endDate = "2020-10-05T12:30:00.000" });
        //    schedulerData.Add(new SchedulerData { text = "Ranjan4", startDate = "2020-10-05T05:30:00.000", endDate = "2020-10-05T06:30:00.000" });
        //    List<SelectListItem> repeatWeeks = new List<SelectListItem>()
        //    {
        //       new SelectListItem {Text ="1" , Value ="1"},
        //       new SelectListItem {Text ="2" , Value ="2"},
        //       new SelectListItem {Text ="3" , Value ="3"},
        //       new SelectListItem {Text ="4" , Value ="4"},
        //       new SelectListItem {Text ="5" , Value ="5"},
        //       new SelectListItem {Text ="6" , Value ="6"},
        //       new SelectListItem {Text ="7" , Value ="7"}
        //    };
        //    taShiftsModelList.schedulerData = schedulerData;
        //    taShiftsModelList.repeatWeeksList = repeatWeeks;
        //    return PartialView(taShiftsModelList);
        //}

        [HttpGet]
        public ActionResult TaUserGroupShiftList()
        {
            var taUserGroupShifts = db.TaUserGroupeShifts.Include("TaWeekShifts").ToList();
            List <TaUserGroupShifts> userGroupShifts = new List<TaUserGroupShifts>();

            foreach(var userGroupShift in taUserGroupShifts)
            {
                userGroupShifts.Add(new TaUserGroupShifts { Id = userGroupShift.Id, Name = userGroupShift.Name, RepeatAfterWeeks = userGroupShift.RepeatAfterWeeks });
            }
            return PartialView("TaGroupShiftsList", userGroupShifts.AsEnumerable());
        }

        [HttpGet]
        public ActionResult EditTaUserGroupShift(int TaUserGroupId)
        {
            var taUserGroupShift = db.TaUserGroupeShifts.Where(x => x.Id == TaUserGroupId).Include("TaWeekShifts").Include("TaWeekShifts.TAShifts1.TaShiftTimeIntervals").Include("TaWeekShifts.TAShifts2.TaShiftTimeIntervals").Include("TaWeekShifts.TAShifts3.TaShiftTimeIntervals").Include("TaWeekShifts.TAShifts4.TaShiftTimeIntervals").Include("TaWeekShifts.TAShifts5.TaShiftTimeIntervals").Include("TaWeekShifts.TAShifts6.TaShiftTimeIntervals").Include("TaWeekShifts.TAShifts7.TaShiftTimeIntervals").FirstOrDefault();
            //var taWeekShifts = db.TaWeekShifts.Where(x => x.TaUserGroupeShiftsId == TaUserGroupId).Include("TaShifts1").ToList();
            var taWeekShifts = db.TaWeekShifts.Where(x => x.TaUserGroupeShiftsId == TaUserGroupId).Include("TaShifts1").ToList();
            var allTaWeekShifts = db.TaWeekShifts.Include("TaShifts1").ToList();
            TaUserGroupShifts taUserGroup = new TaUserGroupShifts {Id = taUserGroupShift.Id,Name= taUserGroupShift.Name,RepeatAfterWeeks=taUserGroupShift.RepeatAfterWeeks,TaWeekShifts=taWeekShifts.ToList(),AllTaWeekShifts = allTaWeekShifts };
            List<ShiftSchedulerDisplay> shiftSchedulerDisplay = new List<ShiftSchedulerDisplay>();
            var dateNow = DateTime.Now.Date;
            var dayOfWeek = dateNow.DayOfWeek;
            var day = dateNow.Day;
            var datetime = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
            var dateMonday = datetime.Date;
            DateTime startTimeForDisplay;
            DateTime endTimeForDisplay;

            DateTime startTimeForDisplay1;
            DateTime endTimeForDisplay1;
            double addCount = 0;
            
            foreach (var item in taUserGroup.TaWeekShifts)
            {
                dateMonday = dateMonday.AddDays(addCount);
                //startTimeForDisplay = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts1.StartFrom.Value.Hour, item.TAShifts1.StartFrom.Value.Minute, item.TAShifts1.StartFrom.Value.Second);
                //endTimeForDisplay = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts1.FinishAt.Value.Hour, item.TAShifts1.FinishAt.Value.Minute, item.TAShifts1.FinishAt.Value.Second);

                //
                //startTimeForDisplay = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts1.StartFrom.Value.Hour, item.TAShifts1.StartFrom.Value.Minute, item.TAShifts1.StartFrom.Value.Second);
                //endTimeForDisplay = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts1.FinishAt.Value.Hour, item.TAShifts1.FinishAt.Value.Minute, item.TAShifts1.FinishAt.Value.Second);
                if(item.TAShifts1 != null)
                {
                    startTimeForDisplay = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts1.TaShiftTimeIntervals.First().StartTime.Hours, item.TAShifts1.TaShiftTimeIntervals.First().StartTime.Minutes, item.TAShifts1.TaShiftTimeIntervals.First().StartTime.Seconds);
                    endTimeForDisplay = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts1.TaShiftTimeIntervals.Last().EndTime.Hours, item.TAShifts1.TaShiftTimeIntervals.Last().EndTime.Minutes, item.TAShifts1.TaShiftTimeIntervals.Last().EndTime.Seconds);

                    ////For BreakTimes
                    //startTimeForDisplay1 = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts1.TaShiftTimeIntervals.ElementAt(1).StartTime.Hours, item.TAShifts1.TaShiftTimeIntervals.ElementAt(1).StartTime.Minutes, item.TAShifts1.TaShiftTimeIntervals.ElementAt(1).StartTime.Seconds);
                    //endTimeForDisplay1 = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts1.TaShiftTimeIntervals.ElementAt(1).EndTime.Hours, item.TAShifts1.TaShiftTimeIntervals.ElementAt(1).EndTime.Minutes, item.TAShifts1.TaShiftTimeIntervals.ElementAt(1).EndTime.Seconds);
                    ////

                    shiftSchedulerDisplay.Add(new ShiftSchedulerDisplay { Text = item.TAShifts1.Name, StartDate = startTimeForDisplay, EndDate = item.TAShifts1.StartFrom.Value.Hour > item.TAShifts1.FinishAt.Value.Hour ? endTimeForDisplay.AddDays(1) : endTimeForDisplay });
                    for(int i =0;i < item.TAShifts1.TaShiftTimeIntervals.Count; i++)
                    {
                        if(i != 0 && i != item.TAShifts1.TaShiftTimeIntervals.Count - 1)
                        {
                            //For BreakTimes
                            startTimeForDisplay1 = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts1.TaShiftTimeIntervals.ElementAt(i).StartTime.Hours, item.TAShifts1.TaShiftTimeIntervals.ElementAt(i).StartTime.Minutes, item.TAShifts1.TaShiftTimeIntervals.ElementAt(i).StartTime.Seconds);
                            endTimeForDisplay1 = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts1.TaShiftTimeIntervals.ElementAt(i).EndTime.Hours, item.TAShifts1.TaShiftTimeIntervals.ElementAt(i).EndTime.Minutes, item.TAShifts1.TaShiftTimeIntervals.ElementAt(i).EndTime.Seconds);
                            //
                            shiftSchedulerDisplay.Add(new ShiftSchedulerDisplay { Text = item.TAShifts1.TaShiftTimeIntervals.ElementAt(i).Name, StartDate = item.TAShifts1.StartFrom.Value.Hour > item.TAShifts1.FinishAt.Value.Hour ? startTimeForDisplay1.AddDays(1) : startTimeForDisplay1, EndDate = item.TAShifts1.StartFrom.Value.Hour > item.TAShifts1.FinishAt.Value.Hour ? endTimeForDisplay1.AddDays(1) : endTimeForDisplay1 });
                        }
                        
                    }
                    
                }
                
                //startTimeForDisplay = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts2.StartFrom.Value.Hour, item.TAShifts2.StartFrom.Value.Minute, item.TAShifts2.StartFrom.Value.Second);
                //endTimeForDisplay = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts2.FinishAt.Value.Hour, item.TAShifts2.FinishAt.Value.Minute, item.TAShifts2.FinishAt.Value.Second);
                if(item.TAShifts2 != null)
                {
                    startTimeForDisplay = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts2.TaShiftTimeIntervals.First().StartTime.Hours, item.TAShifts2.TaShiftTimeIntervals.First().StartTime.Minutes, item.TAShifts2.TaShiftTimeIntervals.First().StartTime.Seconds);
                    endTimeForDisplay = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts2.TaShiftTimeIntervals.Last().EndTime.Hours, item.TAShifts2.TaShiftTimeIntervals.Last().EndTime.Minutes, item.TAShifts2.TaShiftTimeIntervals.Last().EndTime.Seconds);

                    ////For BreakTimes
                    //startTimeForDisplay1 = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts2.TaShiftTimeIntervals.ElementAt(1).StartTime.Hours, item.TAShifts2.TaShiftTimeIntervals.ElementAt(1).StartTime.Minutes, item.TAShifts2.TaShiftTimeIntervals.ElementAt(1).StartTime.Seconds);
                    //endTimeForDisplay1 = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts2.TaShiftTimeIntervals.ElementAt(1).EndTime.Hours, item.TAShifts2.TaShiftTimeIntervals.ElementAt(1).EndTime.Minutes, item.TAShifts2.TaShiftTimeIntervals.ElementAt(1).EndTime.Seconds);
                    ////
                    shiftSchedulerDisplay.Add(new ShiftSchedulerDisplay { Text = item.TAShifts2.Name, StartDate = startTimeForDisplay.AddDays(1), EndDate = item.TAShifts2.StartFrom.Value.Hour > item.TAShifts2.FinishAt.Value.Hour ? endTimeForDisplay.AddDays(2) : endTimeForDisplay.AddDays(1) });
                    for (int i = 0; i < item.TAShifts2.TaShiftTimeIntervals.Count; i++)
                    {
                        if (i != 0 && i != item.TAShifts2.TaShiftTimeIntervals.Count - 1)
                        {
                            //For BreakTimes
                            startTimeForDisplay1 = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts2.TaShiftTimeIntervals.ElementAt(i).StartTime.Hours, item.TAShifts2.TaShiftTimeIntervals.ElementAt(i).StartTime.Minutes, item.TAShifts2.TaShiftTimeIntervals.ElementAt(i).StartTime.Seconds);
                            endTimeForDisplay1 = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts2.TaShiftTimeIntervals.ElementAt(i).EndTime.Hours, item.TAShifts2.TaShiftTimeIntervals.ElementAt(i).EndTime.Minutes, item.TAShifts2.TaShiftTimeIntervals.ElementAt(i).EndTime.Seconds);
                            //
                            shiftSchedulerDisplay.Add(new ShiftSchedulerDisplay { Text = item.TAShifts2.TaShiftTimeIntervals.ElementAt(i).Name, StartDate = item.TAShifts2.StartFrom.Value.Hour > item.TAShifts2.FinishAt.Value.Hour ? startTimeForDisplay1.AddDays(2) : startTimeForDisplay1.AddDays(1), EndDate = item.TAShifts2.StartFrom.Value.Hour > item.TAShifts2.FinishAt.Value.Hour ? endTimeForDisplay1.AddDays(2) : endTimeForDisplay1.AddDays(1) });
                        }
                    }
                }
                
                //startTimeForDisplay = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts3.StartFrom.Value.Hour, item.TAShifts3.StartFrom.Value.Minute, item.TAShifts3.StartFrom.Value.Second);
                //endTimeForDisplay = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts3.FinishAt.Value.Hour, item.TAShifts3.FinishAt.Value.Minute, item.TAShifts3.FinishAt.Value.Second);
                if(item.TAShifts3 != null)
                {
                    startTimeForDisplay = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts3.TaShiftTimeIntervals.First().StartTime.Hours, item.TAShifts3.TaShiftTimeIntervals.First().StartTime.Minutes, item.TAShifts3.TaShiftTimeIntervals.First().StartTime.Seconds);
                    endTimeForDisplay = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts3.TaShiftTimeIntervals.Last().EndTime.Hours, item.TAShifts3.TaShiftTimeIntervals.Last().EndTime.Minutes, item.TAShifts3.TaShiftTimeIntervals.Last().EndTime.Seconds);

                    ////For BreakTimes
                    //startTimeForDisplay1 = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts3.TaShiftTimeIntervals.ElementAt(1).StartTime.Hours, item.TAShifts3.TaShiftTimeIntervals.ElementAt(1).StartTime.Minutes, item.TAShifts3.TaShiftTimeIntervals.ElementAt(1).StartTime.Seconds);
                    //endTimeForDisplay1 = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts3.TaShiftTimeIntervals.ElementAt(1).EndTime.Hours, item.TAShifts3.TaShiftTimeIntervals.ElementAt(1).EndTime.Minutes, item.TAShifts3.TaShiftTimeIntervals.ElementAt(1).EndTime.Seconds);
                    ////

                    shiftSchedulerDisplay.Add(new ShiftSchedulerDisplay { Text = item.TAShifts3.Name, StartDate = startTimeForDisplay.AddDays(2), EndDate = item.TAShifts3.StartFrom.Value.Hour > item.TAShifts3.FinishAt.Value.Hour ? endTimeForDisplay.AddDays(3) : endTimeForDisplay.AddDays(2) });
                    for (int i = 0; i < item.TAShifts3.TaShiftTimeIntervals.Count; i++)
                    {
                        if (i != 0 && i != item.TAShifts3.TaShiftTimeIntervals.Count - 1)
                        {
                            //For BreakTimes
                            startTimeForDisplay1 = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts3.TaShiftTimeIntervals.ElementAt(i).StartTime.Hours, item.TAShifts3.TaShiftTimeIntervals.ElementAt(i).StartTime.Minutes, item.TAShifts3.TaShiftTimeIntervals.ElementAt(i).StartTime.Seconds);
                            endTimeForDisplay1 = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts3.TaShiftTimeIntervals.ElementAt(i).EndTime.Hours, item.TAShifts3.TaShiftTimeIntervals.ElementAt(i).EndTime.Minutes, item.TAShifts3.TaShiftTimeIntervals.ElementAt(i).EndTime.Seconds);
                            //
                            shiftSchedulerDisplay.Add(new ShiftSchedulerDisplay { Text = item.TAShifts3.TaShiftTimeIntervals.ElementAt(i).Name, StartDate = item.TAShifts3.StartFrom.Value.Hour > item.TAShifts3.FinishAt.Value.Hour ? startTimeForDisplay1.AddDays(3) : startTimeForDisplay1.AddDays(2), EndDate = item.TAShifts3.StartFrom.Value.Hour > item.TAShifts3.FinishAt.Value.Hour ? endTimeForDisplay1.AddDays(3) : endTimeForDisplay1.AddDays(2) });
                        }
                    }
                }
                
                //startTimeForDisplay = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts4.StartFrom.Value.Hour, item.TAShifts4.StartFrom.Value.Minute, item.TAShifts4.StartFrom.Value.Second);
                //endTimeForDisplay = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts4.FinishAt.Value.Hour, item.TAShifts4.FinishAt.Value.Minute, item.TAShifts4.FinishAt.Value.Second);
                if(item.TAShifts4 != null)
                {
                    startTimeForDisplay = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts4.TaShiftTimeIntervals.First().StartTime.Hours, item.TAShifts4.TaShiftTimeIntervals.First().StartTime.Minutes, item.TAShifts4.TaShiftTimeIntervals.First().StartTime.Seconds);
                    endTimeForDisplay = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts4.TaShiftTimeIntervals.Last().EndTime.Hours, item.TAShifts4.TaShiftTimeIntervals.Last().EndTime.Minutes, item.TAShifts4.TaShiftTimeIntervals.Last().EndTime.Seconds);

                    ////For BreakTimes
                    //startTimeForDisplay1 = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts4.TaShiftTimeIntervals.ElementAt(1).StartTime.Hours, item.TAShifts4.TaShiftTimeIntervals.ElementAt(1).StartTime.Minutes, item.TAShifts4.TaShiftTimeIntervals.ElementAt(1).StartTime.Seconds);
                    //endTimeForDisplay1 = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts4.TaShiftTimeIntervals.ElementAt(1).EndTime.Hours, item.TAShifts4.TaShiftTimeIntervals.ElementAt(1).EndTime.Minutes, item.TAShifts4.TaShiftTimeIntervals.ElementAt(1).EndTime.Seconds);
                    ////

                    shiftSchedulerDisplay.Add(new ShiftSchedulerDisplay { Text = item.TAShifts4.Name, StartDate = startTimeForDisplay.AddDays(3), EndDate = item.TAShifts4.StartFrom.Value.Hour > item.TAShifts4.FinishAt.Value.Hour ? endTimeForDisplay.AddDays(4) : endTimeForDisplay.AddDays(3) });
                    for (int i = 0; i < item.TAShifts4.TaShiftTimeIntervals.Count; i++)
                    {
                        if (i != 0 && i != item.TAShifts4.TaShiftTimeIntervals.Count - 1)
                        {
                            //For BreakTimes
                            startTimeForDisplay1 = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts4.TaShiftTimeIntervals.ElementAt(i).StartTime.Hours, item.TAShifts4.TaShiftTimeIntervals.ElementAt(i).StartTime.Minutes, item.TAShifts4.TaShiftTimeIntervals.ElementAt(i).StartTime.Seconds);
                            endTimeForDisplay1 = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts4.TaShiftTimeIntervals.ElementAt(i).EndTime.Hours, item.TAShifts4.TaShiftTimeIntervals.ElementAt(i).EndTime.Minutes, item.TAShifts4.TaShiftTimeIntervals.ElementAt(i).EndTime.Seconds);
                            //
                            shiftSchedulerDisplay.Add(new ShiftSchedulerDisplay { Text = item.TAShifts4.TaShiftTimeIntervals.ElementAt(i).Name, StartDate = item.TAShifts4.StartFrom.Value.Hour > item.TAShifts4.FinishAt.Value.Hour ? startTimeForDisplay1.AddDays(4) : startTimeForDisplay1.AddDays(3), EndDate = item.TAShifts4.StartFrom.Value.Hour > item.TAShifts4.FinishAt.Value.Hour ? endTimeForDisplay1.AddDays(4) : endTimeForDisplay1.AddDays(3) });
                        }
                    }
                }
                

                //startTimeForDisplay = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts5.StartFrom.Value.Hour, item.TAShifts5.StartFrom.Value.Minute, item.TAShifts5.StartFrom.Value.Second);
                //endTimeForDisplay = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts5.FinishAt.Value.Hour, item.TAShifts5.FinishAt.Value.Minute, item.TAShifts5.FinishAt.Value.Second);
                if(item.TAShifts5 != null)
                {
                    startTimeForDisplay = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts5.TaShiftTimeIntervals.First().StartTime.Hours, item.TAShifts5.TaShiftTimeIntervals.First().StartTime.Minutes, item.TAShifts5.TaShiftTimeIntervals.First().StartTime.Seconds);
                    endTimeForDisplay = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts5.TaShiftTimeIntervals.Last().EndTime.Hours, item.TAShifts5.TaShiftTimeIntervals.Last().EndTime.Minutes, item.TAShifts5.TaShiftTimeIntervals.Last().EndTime.Seconds);

                    ////For BreakTimes
                    //startTimeForDisplay1 = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts5.TaShiftTimeIntervals.ElementAt(1).StartTime.Hours, item.TAShifts5.TaShiftTimeIntervals.ElementAt(1).StartTime.Minutes, item.TAShifts5.TaShiftTimeIntervals.ElementAt(1).StartTime.Seconds);
                    //endTimeForDisplay1 = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts5.TaShiftTimeIntervals.ElementAt(1).EndTime.Hours, item.TAShifts5.TaShiftTimeIntervals.ElementAt(1).EndTime.Minutes, item.TAShifts5.TaShiftTimeIntervals.ElementAt(1).EndTime.Seconds);
                    ////

                    shiftSchedulerDisplay.Add(new ShiftSchedulerDisplay { Text = item.TAShifts5.Name, StartDate = startTimeForDisplay.AddDays(4), EndDate = item.TAShifts5.StartFrom.Value.Hour > item.TAShifts5.FinishAt.Value.Hour ? endTimeForDisplay.AddDays(5) : endTimeForDisplay.AddDays(4) });
                    for (int i = 0; i < item.TAShifts5.TaShiftTimeIntervals.Count; i++)
                    {
                        if (i != 0 && i != item.TAShifts5.TaShiftTimeIntervals.Count - 1)
                        {
                            //For BreakTimes
                            startTimeForDisplay1 = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts5.TaShiftTimeIntervals.ElementAt(i).StartTime.Hours, item.TAShifts5.TaShiftTimeIntervals.ElementAt(i).StartTime.Minutes, item.TAShifts5.TaShiftTimeIntervals.ElementAt(i).StartTime.Seconds);
                            endTimeForDisplay1 = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts5.TaShiftTimeIntervals.ElementAt(i).EndTime.Hours, item.TAShifts5.TaShiftTimeIntervals.ElementAt(i).EndTime.Minutes, item.TAShifts5.TaShiftTimeIntervals.ElementAt(i).EndTime.Seconds);
                            //
                            shiftSchedulerDisplay.Add(new ShiftSchedulerDisplay { Text = item.TAShifts5.TaShiftTimeIntervals.ElementAt(i).Name, StartDate = item.TAShifts5.StartFrom.Value.Hour > item.TAShifts5.FinishAt.Value.Hour ? startTimeForDisplay1.AddDays(5) : startTimeForDisplay1.AddDays(4), EndDate = item.TAShifts5.StartFrom.Value.Hour > item.TAShifts5.FinishAt.Value.Hour ? endTimeForDisplay1.AddDays(5) : endTimeForDisplay1.AddDays(4) });
                        }
                    }
                }
                

                //startTimeForDisplay = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts6.StartFrom.Value.Hour, item.TAShifts6.StartFrom.Value.Minute, item.TAShifts6.StartFrom.Value.Second);
                //endTimeForDisplay = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts6.FinishAt.Value.Hour, item.TAShifts6.FinishAt.Value.Minute, item.TAShifts6.FinishAt.Value.Second);
                if(item.TAShifts6 != null)
                {
                    startTimeForDisplay = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts6.TaShiftTimeIntervals.First().StartTime.Hours, item.TAShifts6.TaShiftTimeIntervals.First().StartTime.Minutes, item.TAShifts6.TaShiftTimeIntervals.First().StartTime.Seconds);
                    endTimeForDisplay = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts6.TaShiftTimeIntervals.Last().EndTime.Hours, item.TAShifts6.TaShiftTimeIntervals.Last().EndTime.Minutes, item.TAShifts6.TaShiftTimeIntervals.Last().EndTime.Seconds);

                    ////For BreakTimes
                    //startTimeForDisplay1 = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts6.TaShiftTimeIntervals.ElementAt(1).StartTime.Hours, item.TAShifts6.TaShiftTimeIntervals.ElementAt(1).StartTime.Minutes, item.TAShifts6.TaShiftTimeIntervals.ElementAt(1).StartTime.Seconds);
                    //endTimeForDisplay1 = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts6.TaShiftTimeIntervals.ElementAt(1).EndTime.Hours, item.TAShifts6.TaShiftTimeIntervals.ElementAt(1).EndTime.Minutes, item.TAShifts6.TaShiftTimeIntervals.ElementAt(1).EndTime.Seconds);
                    ////

                    shiftSchedulerDisplay.Add(new ShiftSchedulerDisplay { Text = item.TAShifts6.Name, StartDate = startTimeForDisplay.AddDays(5), EndDate = item.TAShifts6.StartFrom.Value.Hour > item.TAShifts6.FinishAt.Value.Hour ? endTimeForDisplay.AddDays(6) : endTimeForDisplay.AddDays(5) });
                    for (int i = 0; i < item.TAShifts6.TaShiftTimeIntervals.Count; i++)
                    {
                        if (i != 0 && i != item.TAShifts6.TaShiftTimeIntervals.Count - 1)
                        {
                            //For BreakTimes
                            startTimeForDisplay1 = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts6.TaShiftTimeIntervals.ElementAt(i).StartTime.Hours, item.TAShifts6.TaShiftTimeIntervals.ElementAt(i).StartTime.Minutes, item.TAShifts6.TaShiftTimeIntervals.ElementAt(i).StartTime.Seconds);
                            endTimeForDisplay1 = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts6.TaShiftTimeIntervals.ElementAt(i).EndTime.Hours, item.TAShifts6.TaShiftTimeIntervals.ElementAt(i).EndTime.Minutes, item.TAShifts6.TaShiftTimeIntervals.ElementAt(i).EndTime.Seconds);
                            //
                            shiftSchedulerDisplay.Add(new ShiftSchedulerDisplay { Text = item.TAShifts6.TaShiftTimeIntervals.ElementAt(i).Name, StartDate = item.TAShifts6.StartFrom.Value.Hour > item.TAShifts6.FinishAt.Value.Hour ? startTimeForDisplay1.AddDays(6) : startTimeForDisplay1.AddDays(5), EndDate = item.TAShifts6.StartFrom.Value.Hour > item.TAShifts6.FinishAt.Value.Hour ? endTimeForDisplay1.AddDays(6) : endTimeForDisplay1.AddDays(5) });
                        }
                    }
                }
                

                //startTimeForDisplay = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts7.StartFrom.Value.Hour, item.TAShifts7.StartFrom.Value.Minute, item.TAShifts7.StartFrom.Value.Second);
                //endTimeForDisplay = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts7.FinishAt.Value.Hour, item.TAShifts7.FinishAt.Value.Minute, item.TAShifts7.FinishAt.Value.Second);
                if(item.TAShifts7 != null)
                {
                    startTimeForDisplay = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts7.TaShiftTimeIntervals.First().StartTime.Hours, item.TAShifts7.TaShiftTimeIntervals.First().StartTime.Minutes, item.TAShifts7.TaShiftTimeIntervals.First().StartTime.Seconds);
                    endTimeForDisplay = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts7.TaShiftTimeIntervals.Last().EndTime.Hours, item.TAShifts7.TaShiftTimeIntervals.Last().EndTime.Minutes, item.TAShifts7.TaShiftTimeIntervals.Last().EndTime.Seconds);

                    ////For BreakTimes
                    //startTimeForDisplay1 = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts7.TaShiftTimeIntervals.ElementAt(1).StartTime.Hours, item.TAShifts7.TaShiftTimeIntervals.ElementAt(1).StartTime.Minutes, item.TAShifts7.TaShiftTimeIntervals.ElementAt(1).StartTime.Seconds);
                    //endTimeForDisplay1 = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts7.TaShiftTimeIntervals.ElementAt(1).EndTime.Hours, item.TAShifts7.TaShiftTimeIntervals.ElementAt(1).EndTime.Minutes, item.TAShifts7.TaShiftTimeIntervals.ElementAt(1).EndTime.Seconds);
                    ////

                    shiftSchedulerDisplay.Add(new ShiftSchedulerDisplay { Text = item.TAShifts7.Name, StartDate = startTimeForDisplay.AddDays(6), EndDate = item.TAShifts7.StartFrom.Value.Hour > item.TAShifts7.FinishAt.Value.Hour ? endTimeForDisplay.AddDays(7) : endTimeForDisplay.AddDays(6) });
                    for (int i = 0; i < item.TAShifts7.TaShiftTimeIntervals.Count; i++)
                    {
                        if (i != 0 && i != item.TAShifts7.TaShiftTimeIntervals.Count - 1)
                        {
                            //For BreakTimes
                            startTimeForDisplay1 = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts7.TaShiftTimeIntervals.ElementAt(i).StartTime.Hours, item.TAShifts7.TaShiftTimeIntervals.ElementAt(i).StartTime.Minutes, item.TAShifts7.TaShiftTimeIntervals.ElementAt(i).StartTime.Seconds);
                            endTimeForDisplay1 = new DateTime(dateMonday.Year, dateMonday.Month, dateMonday.Day, item.TAShifts7.TaShiftTimeIntervals.ElementAt(i).EndTime.Hours, item.TAShifts7.TaShiftTimeIntervals.ElementAt(i).EndTime.Minutes, item.TAShifts7.TaShiftTimeIntervals.ElementAt(i).EndTime.Seconds);
                            //
                            shiftSchedulerDisplay.Add(new ShiftSchedulerDisplay { Text = item.TAShifts7.TaShiftTimeIntervals.ElementAt(i).Name, StartDate = item.TAShifts7.StartFrom.Value.Hour > item.TAShifts7.FinishAt.Value.Hour ? startTimeForDisplay1.AddDays(7) : startTimeForDisplay1.AddDays(6), EndDate = item.TAShifts7.StartFrom.Value.Hour > item.TAShifts7.FinishAt.Value.Hour ? endTimeForDisplay1.AddDays(7) : endTimeForDisplay1.AddDays(6) });
                        }
                    }
                }


                

                //shiftSchedulerDisplay.Add(new ShiftSchedulerDisplay { Text = "Lunch", StartDate = new DateTime(2020, 10, 26, 10, 0, 0), EndDate = new DateTime(2020, 10, 26, 11, 0, 0) });
                addCount =  7;
            };
            taUserGroup.ShiftSchedulerDisplays = shiftSchedulerDisplay;
            return PartialView("TaGroupShifts",taUserGroup);
        }

        [HttpPost]
        public ActionResult AddTaWeekShift(AddNewTaWeekShiftModel weekShiftModel)
        {
            var dbTaWeekShift = db.TaWeekShifts;
            var added = db.TaWeekShifts.Add(new TaWeekShift
            {
                Name = weekShiftModel.Name,
                TaShiftIdMonday = weekShiftModel.MondayShift,
                TaShiftIdTuesday = weekShiftModel.TuesdayShift,
                TaShiftIdWednesday = weekShiftModel.WednesdayShift,
                TaShiftIdThursday = weekShiftModel.ThursdayShift,
                TaShiftIdFriday = weekShiftModel.FridayShift,
                TaShiftIdSaturday = weekShiftModel.SaturdayShift,
                TaShiftIdSunday = weekShiftModel.SundayShift,
                IsDeleted = false
            });
            int saveChanges = db.SaveChanges();
            int newRecordId = added.Id;
            if(saveChanges != 0)
            {
                return Json(new { IsSaved = true, msg = "TaWeek shift saved." });
            }
            else
            {
                return Json(new { IsSaved = false, msg = "TaWeek shift not saved." });
            }
        }


        [HttpGet]
        public ActionResult TaWeekShifts()
        {
            var taShifts = db.TAShifts.Include("Company").ToList();
            TaWeekShiftsModel taWeekShifts = new TaWeekShiftsModel();
            taWeekShifts.TaShiftsModelsList = taShifts.AsEnumerable();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            foreach(var item in taShifts)
            {
                selectListItem.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            taWeekShifts.DropDownItems = selectListItem;
            return PartialView(taWeekShifts);
        }

        [HttpGet]
        public ActionResult AssignTaUserGroupShiftToUsers(int GroupShiftId)
        {
            var currentUserName = System.Web.HttpContext.Current.User.Identity.Name;
            //List<UserItem> userItems = new List<UserItem>();
            //var uvm = CreateViewModel<UserListViewModel>();
            //if (CurrentUser.Get().IsSuperAdmin)
            //{
            //    var users = _userRepository.FindAll();

            //    Mapper.Map(users, userItems);
            //    uvm.Users = userItems;
            //}
            //else if (CurrentUser.Get().IsCompanyManager)
            //{
            //    var users = _userRepository.FindAll(x => x.CompanyId != null && x.CompanyId == CurrentUser.Get().CompanyId);
            //    Mapper.Map(users, userItems);
            //    uvm.Users = userItems;
            //}
            AssignShiftUserModel assignShiftUserModel = new AssignShiftUserModel();
            if (CurrentUser.Get().IsSuperAdmin)
            {
                var usersList = db.User.ToList();
                Mapper.CreateMap<Users, AssignShiftUserModel>();
                Mapper.Map(usersList, assignShiftUserModel.AssignShiftUserModelsList);
                //var taUsersShifts = db.TaUsersShifts.Where(x => x.TaUserGroupShiftId == GroupShiftId).Include("TaUserGroupShifts").ToList();
                var taUsersShifts = db.TaUsersShifts.Where(x => !x.IsDeleted).Include("TaUserGroupShifts").ToList();
                assignShiftUserModel.AssignShiftUserModelsList.ForEach(x => {x.TaUsersShifts = taUsersShifts.Where(y => y.UeserId == x.Id).FirstOrDefault();x.TaUsersGroupShiftId = GroupShiftId;x.StartDate = taUsersShifts.Where(z => z.UeserId == x.Id).Select(date => date.StartDate).FirstOrDefault(); x.EndDate = taUsersShifts.Where(l => l.UeserId == x.Id).Select(endDate => endDate.EndDate).FirstOrDefault(); });
                assignShiftUserModel.TaUsersGroupShiftId = GroupShiftId;
                //assignShiftUserModel.TaUsersShifts = taUsersShifts;


            }
            else if (CurrentUser.Get().IsCompanyManager)
            {
                var usersList = db.User.Where(x => x.CompanyId != null && x.CompanyId == CurrentUser.Get().CompanyId).Include("TaUsersShifts").ToList();
                Mapper.Map(usersList, assignShiftUserModel);
                var taUsersShifts = db.TaUsersShifts.Where(x => x.TaUserGroupShiftId == GroupShiftId).Include("TaUserGroupeShifts").ToList();
                assignShiftUserModel.AssignShiftUserModelsList.ForEach(x => { x.TaUsersShifts = taUsersShifts.Where(y => y.UeserId == x.Id).FirstOrDefault();x.IsSelected = taUsersShifts.Where(z => z.UeserId == x.Id).Any(); });

            }

            
            return PartialView(assignShiftUserModel);
        }

        [HttpPost]
        public ActionResult AssignTaUserGroupShiftToUsersSave(AssignShiftUserSaveModel shiftUserSaveModel)
        {
            var taUserShifts = db.TaUsersShifts;
            var users = db.User;
            var queryToUpdate = users.SqlQuery("Update Users set WorkTime = 0");
            if (shiftUserSaveModel.SelectedUsersIsTA.Count > 0)
            {
                foreach(var user in shiftUserSaveModel.SelectedUsersIsTA)
                {
                    var userInLoop = users.Where(x => x.Id == user).FirstOrDefault();
                    userInLoop.WorkTime = true;      
                }
               
                

                int responseResult = db.SaveChanges();
            }
            if(shiftUserSaveModel.SelectedUsersId.Count > 0)
            {
                foreach (var userId in shiftUserSaveModel.SelectedUsersId)
                {
                    if(taUserShifts.Any(x => x.UeserId == userId))
                    {
                        taUserShifts.Where(x => x.UeserId == userId).FirstOrDefault().IsDeleted = true;
                    }
                    taUserShifts.Add(new TaUsersShifts
                    {
                        UeserId = userId,
                        TaUserGroupShiftId = shiftUserSaveModel.TaUserGroupShiftId,
                        StartDate = shiftUserSaveModel.StartDate,
                        EndDate = shiftUserSaveModel.EndDate,
                        IsDeleted = false
                    });
                }
                int saveResponse = db.SaveChanges();
            }
            
            return null;
        }

        [HttpPost]
        public ActionResult RemoveUsersGroupSchedule(List<int> UsersListToRemove)
        {
            if(UsersListToRemove != null)
            {
                var taUsersShift = db.TaUsersShifts;
                foreach (var user in UsersListToRemove)
                {
                    var taUserShift = taUsersShift.Where(x => x.UeserId == user && !x.IsDeleted).FirstOrDefault();
                    taUserShift.IsDeleted = true;
                }
                int response = db.SaveChanges();
            }
            
            return null;
        }

        [HttpGet]
        public ActionResult AddTaUserGroupShifts()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult SaveNewTaUserGroupShift(TaUserGroupShiftSaveModel taUserGroupShiftSaveModel)
        {
            taUserGroupShiftSaveModel.StartFromDate = taUserGroupShiftSaveModel.StartFromDate;
            var tauserGroupeShifts = new TaUserGroupeShifts { Name = taUserGroupShiftSaveModel.Name, RepeatAfterWeeks = taUserGroupShiftSaveModel.RepeatAfterWeeks, IsDeleted = taUserGroupShiftSaveModel.IsDeleted, StartFromDate = taUserGroupShiftSaveModel.StartFromDate };
            var newAddedTaUserGroupShifts = db.TaUserGroupeShifts.Add(tauserGroupeShifts);
            int response = db.SaveChanges();
            var newTaUserGroupShiftId = newAddedTaUserGroupShifts.Id;
            int taUserGroupShiftIdOrder = 1;
            foreach(int taUserGroupShiftId in taUserGroupShiftSaveModel.SelectedTaWeeks)
            {
               var taWeekShifts = db.TaWeekShifts.Where(x => x.Id == taUserGroupShiftId).FirstOrDefault();
                taWeekShifts.InTaUserGroupeShiftsOrder = taUserGroupShiftIdOrder;
                taWeekShifts.TaUserGroupeShiftsId = newTaUserGroupShiftId;
                taUserGroupShiftIdOrder = taUserGroupShiftIdOrder + 1;
            }
            int taWeekResponse = db.SaveChanges();
            return Json(new {msg="TaUserGroup shift saved.",IsSucceed = true });
        }

        [HttpPost]
        public ActionResult TaShiftSave(TaShiftSaveModel ShiftSaveModel)
        {
            var dbTaShifts = db.TAShifts;
            var taNewShift = new TAShifts { Name = ShiftSaveModel.ShiftName, StartFrom = ShiftSaveModel.ShiftStartTime, FinishAt = ShiftSaveModel.ShiftFinishTime,DuratOfBreak= ShiftSaveModel.DuratOfBreak.Value,DuratOfBreakOvertime= ShiftSaveModel.DuratOfBreakOvertime.Value,BreakMinInterval= ShiftSaveModel.BreakMinInterval.Value,BreakMinIntervalOvertime= ShiftSaveModel.BreakMinIntervalOvertime.Value,LateAllowed= ShiftSaveModel.LateAllowed.Value,OvertimeStartLater= ShiftSaveModel.OvertimeStartLater.Value,OvertimeStartsEarlier= ShiftSaveModel.OvertimeStartsEarlier.Value,Presence= ShiftSaveModel.Presence };
            dbTaShifts.Add(taNewShift);
            int response = db.SaveChanges();
            int newlyAddedShiftId = taNewShift.Id;
            var dbTaShiftTimeIntervals = db.TaShiftTimeIntervals;
            for(int i =0; i < ShiftSaveModel.TaShiftIntervalNamesList.Count; i++)
            {
                var taNewTaShiftTimeIntervals = new TaShiftTimeIntervals { Name = ShiftSaveModel.TaShiftIntervalNamesList[i], StartTime = ShiftSaveModel.TaShiftIntervalStartTimeList[i], EndTime = ShiftSaveModel.TaShiftIntervalEndTimeList[i], TaShiftId = newlyAddedShiftId, TaReportLabelId = ShiftSaveModel.TaShiftIntervalTaReportLabelsIdList[i] };
                dbTaShiftTimeIntervals.Add(taNewTaShiftTimeIntervals);
            }
            int shiftIntervalSaveResponse = db.SaveChanges();
            
            return Json(new { msg="TaShift saved successfully.",IsSucceed = true });
        }

        [HttpGet]
        public ActionResult TaShift()
        {
            var taShifts = db.TAShifts.Include("Company").Include("TaShiftTimeIntervals").ToList();
            var companies = db.Companies.ToList();
            var taReportLabels = db.TAReportLabels.ToList();
            TaShiftsModel taShiftsModelList = new TaShiftsModel();
            taShiftsModelList.TAShifts = taShifts;
            taShiftsModelList.CompaniesList = companies;
            taShiftsModelList.TAReportLabels = taReportLabels;
            //return PartialView(taShiftsModelList);
            return PartialView("TaShiftsList", taShiftsModelList);
        }

        [HttpGet]
        public ActionResult AddNewTaShift()
        {
            return PartialView("TaShift");
        }
      
        [HttpGet]
        public ActionResult EditTaShift(int ShiftId)
        {
            var taShift = db.TAShifts.Where(x => x.Id == ShiftId).Include("TaShiftTimeIntervals").FirstOrDefault();
            var taReportLabelList = db.TAReportLabels.ToList();
            var taShifts = new TaShiftsModel();
            List<SelectListItem> taReportLabelListItem = new List<SelectListItem>();
            foreach (var taReportLabel in taReportLabelList)
            {
                SelectListItem taReportLabelItem = new SelectListItem { Text = taReportLabel.Name, Value = taReportLabel.Id.ToString(), Selected = false };
                taReportLabelListItem.Add(taReportLabelItem);
            }
            
            taShifts.TAShift = taShift;
            taShifts.taReportItem = taReportLabelListItem;
            return PartialView(taShifts);
        }

        [HttpPost]
        public ActionResult EditTaShiftSave(TaShiftEditSaveModel TaShiftEditSave)
        {
            try
            {
                var taShiftEdit = db.TAShifts.Where(x => x.Id == TaShiftEditSave.Id).FirstOrDefault();
                taShiftEdit.Name = TaShiftEditSave.ShiftName;
                taShiftEdit.StartFrom = TaShiftEditSave.ShiftStartTime;
                taShiftEdit.FinishAt = TaShiftEditSave.ShiftFinishTime;
                if (TaShiftEditSave.BreakMinInterval.HasValue)
                {
                    taShiftEdit.BreakMinInterval = TaShiftEditSave.BreakMinInterval.Value;
                }
                if (TaShiftEditSave.BreakMinIntervalOvertime.HasValue)
                {
                    taShiftEdit.BreakMinIntervalOvertime = TaShiftEditSave.BreakMinIntervalOvertime.Value;
                }
                if (TaShiftEditSave.DuratOfBreak.HasValue)
                {
                    taShiftEdit.DuratOfBreak = TaShiftEditSave.DuratOfBreak.Value;
                }
                if (TaShiftEditSave.DuratOfBreakOvertime.HasValue)
                {
                    taShiftEdit.DuratOfBreakOvertime = TaShiftEditSave.DuratOfBreakOvertime.Value;
                }

                //taShiftEdit.FirstEntryLastExit =  TaShiftEditSave.FirstEntryLastExit.Value;
                if (TaShiftEditSave.LateAllowed.HasValue)
                {
                    taShiftEdit.LateAllowed = TaShiftEditSave.LateAllowed.Value;
                }
                if (TaShiftEditSave.OvertimeStartLater.HasValue)
                {
                    taShiftEdit.OvertimeStartLater = TaShiftEditSave.OvertimeStartLater.Value;
                }
                if (TaShiftEditSave.OvertimeStartsEarlier.HasValue)
                {
                    taShiftEdit.OvertimeStartsEarlier = TaShiftEditSave.OvertimeStartsEarlier.Value;
                }
                if (TaShiftEditSave.Presence.HasValue)
                {
                    taShiftEdit.Presence = TaShiftEditSave.Presence.Value;
                }
                
                for(int i= 0; i< TaShiftEditSave.TaShiftIntervalNamesList.Count; i++)
                {
                    if(i == TaShiftEditSave.TaShiftIntervalIdList.Count)
                    {
                        var dbTaShiftTimeIntervals = db.TaShiftTimeIntervals;
                        var taTimeShiftIntervals = new TaShiftTimeIntervals { Name = TaShiftEditSave.TaShiftIntervalNamesList[i], StartTime = TaShiftEditSave.TaShiftIntervalStartTimeList[i], EndTime = TaShiftEditSave.TaShiftIntervalEndTimeList[i], TaReportLabelId = TaShiftEditSave.TaShiftIntervalTaReportLabelsIdList[i], TaShiftId = TaShiftEditSave.Id };
                        dbTaShiftTimeIntervals.Add(taTimeShiftIntervals);
                    }
                    else
                    {
                        var taShiftIntervalId = TaShiftEditSave.TaShiftIntervalIdList[i];
                        var dbTaShiftTimeInterval = db.TaShiftTimeIntervals.Where(x => x.Id == taShiftIntervalId).FirstOrDefault();
                        dbTaShiftTimeInterval.Name = TaShiftEditSave.TaShiftIntervalNamesList[i];
                        dbTaShiftTimeInterval.StartTime = TaShiftEditSave.TaShiftIntervalStartTimeList[i];
                        dbTaShiftTimeInterval.EndTime = TaShiftEditSave.TaShiftIntervalEndTimeList[i];
                        dbTaShiftTimeInterval.TaReportLabelId = TaShiftEditSave.TaShiftIntervalTaReportLabelsIdList[i];
                    }
                    
                }
                int response = db.SaveChanges();
            }
            catch 
            {
                return Json(new { msg = "TaShift not Edited successfully", IsSucceed = false });
            }
           
            return Json(new { msg = "TaShift Edited successfully", IsSucceed = true });
        }

        [HttpGet]
        public ActionResult AddNewtaShiftTimeIntervals()
        {
            var taReportLabelList = db.TAReportLabels.ToList();
            AddNewTaShiftTimeInterval addNewTaShiftTimeInterval = new AddNewTaShiftTimeInterval();

            List<SelectListItem> taReportLabelListItem = new List<SelectListItem>();
            foreach (var taReportLabel in taReportLabelList)
            {
                SelectListItem taReportLabelItem = new SelectListItem { Text = taReportLabel.Name, Value = taReportLabel.Id.ToString(), Selected = false };
                taReportLabelListItem.Add(taReportLabelItem);
            }
            addNewTaShiftTimeInterval.taReportItem = taReportLabelListItem;
            return PartialView(addNewTaShiftTimeInterval);
        }

        public ActionResult TAReportLabels()
        {
            var taReportLabelsList = db.TAReportLabels.Include("Companies").ToList();
            var companiesList = db.Companies.ToList();
            var lsit = new TAReportLabelsModel();
            lsit.TAReportLabels = taReportLabelsList;
            lsit.CompaniesList = companiesList;
            return PartialView(lsit);
        }

        

        [HttpPost]
        public string TaReportLabelsUpdate(MVCxGridViewBatchUpdateValues<TAReportLabels,object> updatedValues)
        {
            var dbTaReportLabels = db.TAReportLabels;
            foreach(var tareportLabels in updatedValues.Update)
            {
               var taReportLabelsRecord = dbTaReportLabels.FirstOrDefault(x => x.Id == tareportLabels.Id);
                taReportLabelsRecord.Label = tareportLabels.Label;
                taReportLabelsRecord.Name = tareportLabels.Name;
                taReportLabelsRecord.CompanyId = tareportLabels.CompanyId;
                //taReportLabelsRecord.EnteredEvent = tareportLabels.EnteredEvent;
                taReportLabelsRecord.At_work = tareportLabels.At_work;
                taReportLabelsRecord.Allow_Jobs = tareportLabels.Allow_Jobs;
                taReportLabelsRecord.Admin_only = tareportLabels.Admin_only;
                //taReportLabelsRecord.DaysNotHours = tareportLabels.DaysNotHours;
                taReportLabelsRecord.InBuilding = tareportLabels.InBuilding;
                try
                {
                    db.SaveChanges();
                }
                catch(Exception ex)
                {

                }
            }
            return "";
        }

        public StringBuilder TaWeeksValues(string selectedWeek)
        {
            var taWeeksList = db.TaWeekShifts.Where(x => x.TaUserGroupeShiftsId.HasValue == false).ToList();
            int selectedWeeks = Convert.ToInt32(selectedWeek);

            StringBuilder result = new StringBuilder();
            for(int i = 1; i<= selectedWeeks; i++)
            {
                string valuesToAppend = "<tr class='dynamicValues'><td>Week" + i + "</td><td><select class='dynamicSelectedValue'>" +
                        "<option value='0'>--Select TaWeek--</option>";
                result.Append(valuesToAppend);

                for(int j=0; j< taWeeksList.Count; j++)
                {
                    result.Append("<option value='" + taWeeksList[j].Id + "'>" + taWeeksList[j].Name + "</option>");
                }
                result.Append("</select></td></tr>");
            }
            return result;
        }
    }
    public static class DateTimeExtension
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = 0;
            if (dt.DayOfWeek != 0)
            {
                diff = dt.DayOfWeek - startOfWeek;
                return DateTime.Now.AddDays(-diff);
            }
            
            return DateTime.Now.AddDays(diff + 1);

        }
    }
}



