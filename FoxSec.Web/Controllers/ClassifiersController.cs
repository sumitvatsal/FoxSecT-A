using System;
using System.Linq;
using AutoMapper;
using FoxSec.Authentication;
using FoxSec.Infrastructure.EF.Repositories;
using FoxSec.Infrastructure.EntLib.Logging;
using FoxSec.ServiceLayer.Contracts;
using FoxSec.Web.Helpers;
using FoxSec.Web.ViewModels;
using System.Web.Mvc;
using System.Globalization;
using System.Threading;
using System.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using FoxSec.Core.Infrastructure.UnitOfWork;
using System.Collections;
using System.Text.RegularExpressions;
using System.Data;

namespace FoxSec.Web.Controllers
{
    public class ClassifiersController : BusinessCaseController
    {
        private readonly IClassificatorRepository _classificatorRepository;
        private readonly IClassificatorValueRepository _classificatorValueRepository;
        private readonly IClassificatorService _classificatorService;
        private readonly License _license;
        private readonly CheckLicense _chklicense;

        public ClassifiersController(
            IClassificatorRepository classificatorRepository,
            IClassificatorValueRepository classificatorValueRepository,
            IClassificatorService classificatorService,
            License license,
            CheckLicense chklicense,
            ICurrentUser currentUser,
            ILogger logger)
            : base(currentUser, logger)
        {
            _classificatorRepository = classificatorRepository;
            _classificatorValueRepository = classificatorValueRepository;
            _classificatorService = classificatorService;
            _license = license;
            _chklicense = chklicense;
        }

        public ActionResult TabContent()
        {
            var hmv = CreateViewModel<HomeViewModel>();
            return PartialView(hmv);
        }

        [HttpGet]
        public ActionResult List()
        {
            var clvm = CreateViewModel<ClassificatorListViewModel>();
            Mapper.Map(_classificatorRepository.FindAll().OrderBy(x => x.Description), clvm.Classifiers);
            return PartialView(clvm);
        }

        [HttpGet]
        public ActionResult ListValues(int id)
        {
            ViewBag.ClassifierName = _classificatorRepository.FindById(id).Description;
            _classificatorService.InsertLicencePathintbl(id);
            var cvlvm = CreateViewModel<ClassificatorValueListViewModel>();
            Mapper.Map(_classificatorValueRepository.FindByClassificatorId(id).OrderBy(x => x.SortOrder), cvlvm.ClassificatorValues);
            foreach (var obj in cvlvm.ClassificatorValues)
            {
                if (obj.ValidTo != null)
                {
                    obj.ToDateTime = Convert.ToDateTime(obj.ValidTo).ToString("dd.MM.yyyy");
                }
            }
            return PartialView(cvlvm);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var cevm = CreateViewModel<ClassificatorEditViewModel>();
            return View(cevm);
        }

        [HttpPost]
        public ActionResult Create(ClassificatorEditViewModel c)
        {
            var cevm = CreateViewModel<ClassificatorEditViewModel>();
            cevm.Classificator = c.Classificator;
            string err_msg = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    _classificatorService.CreateClassificator(c.Classificator.Description, c.Classificator.Comments, HostName);
                }
                catch (Exception ex)
                {
                    err_msg = ex.Message;
                    ModelState.AddModelError("", err_msg);
                }
            }

            return Json(new
            {
                IsSucceed = ModelState.IsValid,
                Msg = ModelState.IsValid ? ViewResources.SharedStrings.DataSavingMessage : err_msg,
                DisplayMessage = !string.IsNullOrEmpty(err_msg),
                viewData = this.RenderPartialView("Create", cevm)
            });
        }

        [HttpGet]
        public ActionResult CreateValue(int id)
        {
            var cvevm = CreateViewModel<ClassificatorValueEditViewModel>();
            cvevm.ClassificatorValue.ClassificatorId = id;
            return View(cvevm);
        }

        [HttpPost]
        public ActionResult CreateValue(ClassificatorValueEditViewModel cv)
        {
            var cevm = CreateViewModel<ClassificatorValueEditViewModel>();
            cevm.ClassificatorValue = cv.ClassificatorValue;
            string err_msg = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    _classificatorService.CreateClassificatorValue((int)cevm.ClassificatorValue.ClassificatorId, cevm.ClassificatorValue.Value, HostName);
                }
                catch (Exception ex)
                {
                    err_msg = ex.Message;
                    ModelState.AddModelError("", err_msg);
                }
            }

            return Json(new
            {
                IsSucceed = ModelState.IsValid,
                Msg = ModelState.IsValid ? ViewResources.SharedStrings.ClassifiersSavingClassificatorValue : err_msg,
                DisplayMessage = !string.IsNullOrEmpty(err_msg),
                viewData = this.RenderPartialView("CreateValue", cevm)
            });
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var cevm = CreateViewModel<ClassificatorEditViewModel>();
            Mapper.Map(_classificatorRepository.FindById(id), cevm.Classificator);
            ViewBag.ClassifierName = cevm.Classificator.Description;
            Mapper.Map(_classificatorValueRepository.FindByClassificatorId(id), cevm.ClassificatorValues);
            return PartialView(cevm);
        }

        [HttpPost]
        public ActionResult Edit(ClassificatorEditViewModel c)
        {
            var cevm = CreateViewModel<ClassificatorEditViewModel>();
            cevm.Classificator = c.Classificator;
            string err_msg = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    _classificatorService.EditClassificator((int)c.Classificator.Id, c.Classificator.Description, c.Classificator.Comments, HostName);
                }
                catch (Exception ex)
                {
                    err_msg = ex.Message;
                    ModelState.AddModelError("", err_msg);
                }
            }

            return Json(new
            {
                IsSucceed = ModelState.IsValid,
                Msg = ModelState.IsValid ? string.Format("{0} \"{1}\" ...", ViewResources.SharedStrings.ClassifiersSaving, c.Classificator.Description) : err_msg,
                DisplayMessage = !string.IsNullOrEmpty(err_msg),
                viewData = this.RenderPartialView("Edit", cevm)
            });

        }

        [HttpGet]
        public ActionResult EditValue(int id)
        {
            var cvevm = CreateViewModel<ClassificatorValueEditViewModel>();
            Mapper.Map(_classificatorValueRepository.FindById(id), cvevm.ClassificatorValue);
            return PartialView(cvevm);
        }

        [HttpPost]
        public ActionResult EditValue(ClassificatorValueEditViewModel cv)
        {
            var cevm = CreateViewModel<ClassificatorValueEditViewModel>();
            cevm.ClassificatorValue = cv.ClassificatorValue;
            string err_msg = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    _classificatorService.EditClassificatorValue((int)cv.ClassificatorValue.Id, cv.ClassificatorValue.Value, HostName);
                }
                catch (Exception ex)
                {
                    err_msg = ex.Message;
                    ModelState.AddModelError("", err_msg);
                }
            }
            return Json(new
            {
                IsSucceed = ModelState.IsValid,
                Msg = ModelState.IsValid ? ViewResources.SharedStrings.ClassifiersSavingClassificatorValue : err_msg,
                DisplayMessage = !string.IsNullOrEmpty(err_msg),
                viewData = this.RenderPartialView("EditValue", cevm)
            });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                _classificatorService.DeleteClassificator(id, HostName);
                return Content("True");
            }
            catch (Exception e)
            {
                Logger.Write("Error deleting Classificator Id=" + id, e);
            }
            return Content("False");
        }

        [HttpPost]
        public ActionResult DeleteValue(int id, int classifierId)
        {
            try
            {
                _classificatorService.DeleteClassificatorValue(id, HostName);
            }
            catch (Exception e)
            {
                Logger.Write("Error deleting classificator value Id=" + id, e);
            }
            return RedirectToAction("ListValues", new { id = classifierId });
        }

        [HttpGet]
        public ActionResult UploadImage(int userId, string licencepath)
        {
            Session["licencepath"] = licencepath;
            return PartialView(userId);
        }

        [HttpPost]
        public string SaveLicenceDetails(int Id)
        {
            try
            {
                string fileName = String.Empty;
                string newName = string.Empty;
                string aa = "0";
                int reqid = 0;
                string licencepath = Session["licencepath"].ToString();

                foreach (string inputTagName in Request.Files)
                {
                    HttpPostedFileBase image = Request.Files[inputTagName];

                    if (image.ContentLength > 0)
                    {
                        try
                        {
                            string fullPath = Request.MapPath("../Uploads");
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }

                        fileName = Path.Combine(HttpContext.Server.MapPath("../Uploads"), "FoxSecLicense.ini");
                        image.SaveAs(fileName);

                        aa = _chklicense.checkusb(fileName);
                        if (aa == "0")
                        {
                            return "-1";
                        }
                        else if (aa == "2")
                        {
                            int chk = _license.CheckLicenseLessValidation(Id, HostName, fileName, licencepath);
                            if (chk == 0)
                            {
                                string licpath = Path.Combine(HttpContext.Server.MapPath("../"), "FoxSecLicense.ini");


                                if (System.IO.File.Exists(licpath))
                                {
                                    try
                                    {
                                        System.IO.File.Delete(licpath);
                                    }
                                    catch
                                    {
                                    }
                                }
                                try
                                {
                                    System.IO.File.Copy(fileName, licpath);
                                }
                                catch
                                {
                                }
                                licencepath = licpath;
                                _license.chklicence(Id, HostName, fileName, licencepath);
                                reqid = Id;
                            }
                            else
                            {
                                return "-4";
                            }
                        }
                        else
                        {
                            int chkt = _license.CheckLicenseLessValidation(Id, HostName, fileName, licencepath);
                            if (chkt == 0)
                            {
                                bool chk = Regex.IsMatch(aa.Substring(0, 1), @"^[a-zA-Z]+$");
                                if (chk == true)
                                {
                                    licencepath = aa + @"\FoxSecLicense.ini";
                                    _license.chklicence(Id, HostName, fileName, licencepath);
                                    reqid = Id;
                                }
                                else
                                {
                                    reqid = -1;
                                }
                            }
                            else
                            {
                                return "-4";
                            }
                        }
                    }
                    else
                    {
                        return "-3";
                    }
                }
                return Convert.ToString(reqid);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}