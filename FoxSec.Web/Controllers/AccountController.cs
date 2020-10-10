using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FoxSec.Accounts;
using FoxSec.Authentication;
using FoxSec.Common.SendMail;
using FoxSec.Core.SystemEvents;
using FoxSec.Core.SystemEvents.DTOs;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EntLib.Logging;
using FoxSec.ServiceLayer.Contracts;
using FoxSec.Web.ViewModels;
using FoxSec.Infrastructure.EF.Repositories;
using static FoxSec.DomainModel.DomainObjects.User;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using FoxSec.Web.ListModel;
using AutoMapper;

namespace FoxSec.Web.Controllers
{
    public class AccountController : ControllerBase
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString);
        private readonly IMembershipService _membershipService;
        private readonly IFormsAuthenticationService _formsService;
        private readonly ILogService _logService;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
       
        public AccountController(ICurrentUser currentUser, IMembershipService membershipService, IFormsAuthenticationService formsAuthenticationService,
            ILogService logService, ILogger logger, IUserRepository userRepository, IRoleRepository roleRepository)
            : base(currentUser, logger)
        {
            _membershipService = membershipService;
            _formsService = formsAuthenticationService;
            _logService = logService;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }
        FoxSecDBContext db = new FoxSecDBContext();
        public ActionResult LogOn()
        {
            var avm = CreateViewModel<LogOnModel>();

            //var a = new FoxSec.Web.Controllers.License();
            //int aa = 0;
            //int aa1 = 0;
            //aa = a.checkhard();
            //if (aa == 0 || aa == 1) { aa1 = a.checkusb(); }
            //if (aa1 > aa) { aa = aa1; }

            //if (aa == 0)
            //{
            //    return RedirectToAction("DemoError", "Account");
            //}
            //if (aa == 1)
            //{
            //    if (_userRepository.FindAll().Count() > 30)
            //    {
            //        return RedirectToAction("DemoError", "Account"); //illimar 
            //    }
            //}

            return View(avm);
        }

        public ActionResult DemoError()
        {
            return View("DemoError");
        }
        private void CheckUserAuthentication(string UserName)
        {
            int compny_id = 0, roleid1 = 0;
            string getvaliddate1 = @" SELECT ISNULL((u.CompanyID), 0) AS CompanyID,r.RoleID from Users u join UserRoles r on u.Id=r.UserId where u.LoginName={0}";
            var ResultCompany = db.Database.SqlQuery<box8>(getvaliddate1, UserName).FirstOrDefault();

            compny_id = Convert.ToInt32(ResultCompany.CompanyID);
            roleid1 = Convert.ToInt32(ResultCompany.RoleID);

            Session["roll_id"] = roleid1;
            //if (compny_id != 0 || roleid1!=0)
            //{
            //    Session["Visibletab"] = "True";
            //}
            //else
            //{
            //    Session["Visibletab"] = "False";
            //}
            //if (roleid1 != 5)
            //{
            //    string getvalidcompid = @" select  CameraID,CompanyID from CameraPermissions    where CompanyId={0}";
            //    var ResultCompany1 = db.Database.SqlQuery<box9>(getvalidcompid, compny_id).FirstOrDefault();

            //    compny_id = ResultCompany1.CompanyID;
            //    if (compny_id == 0)
            //    {
            //        Session["Visibletab"] = "False";
            //    }
            //    else
            //    {
            //        Session["Visibletab"] = "True";
            //    }

            //}
            //else
            //{
            //    Session["Visibletab"] = "True";
            //}
        }

        [HttpPost]
        //     public ActionResult UserLogOn(string UserName, string Password, string returnUrl)
        //     {

        //     	var err_msg = string.Empty;
        //if (ModelState.IsValid)
        //         {
        //             User user;
        //             FoxSecDBContext db = new FoxSecDBContext();

        //	if (_membershipService.ValidateUser(UserName, Password, out user))
        //	{
        //                 //Send log
        //                 //illi 25.12.1012 Logger4SendingEMail.LogSender.Info(string.Format("User \"{0}\" has entered!", UserName));
        //                 //illi 25.12.1012 Logger4SendingEMail.InitLogger();



        //                 // CheckUserAuthentication(UserName);

        //                 var user_roles = user.UserRoles;
        //		var check_date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //		var role = user_roles.Where(ur => !ur.IsDeleted && ur.ValidTo >= check_date && ur.ValidFrom <= check_date).FirstOrDefault();

        //                 int role_id = role.RoleId;
        //                 Session["Role_ID"] = role_id;
        //                 Session["User_Id"] = role.UserId;
        //                 //int comp_id = role.CompanyId;

        //                 var rolename = db.UserRoles.SqlQuery("select Id,Name from Roles where Id=" + role_id).ToList();
        //                 //rolee = db.Database.ExecuteSqlCommand("select Name from Roles where Id="+role_id);
        //                 if (rolename != null)
        //                 {
        //                     Session["Role_Name"] = rolename[0].Name;
        //                 }
        //                 if (role != null)
        //		{
        //                     //var rvm = CreateViewModel<RoleEditViewModel>();
        //                     //Mapper.Map(_roleRepository.FindById(role_id), rvm.Role);
        //                     var menues = role.Role.Menues.ToList().Contains(1);
        //                     if(menues)
        //                     {
        //                         _formsService.SignIn(UserName, false);

        //                         var host = Request.UserHostAddress;
        //                         var xml_message = new XElement(XMLLogLiterals.LOG_MESSAGE);
        //                         var logon_params = new List<string>();
        //                         logon_params.Add(UserName);
        //                         var xml_user_logon = XMLLogMessageHelper.TemplateToXml("LogMessageUserLogon", logon_params);
        //                         xml_message.Add(xml_user_logon);
        //                         var flag = "";

        //                         _logService.CreateLog(user.Id, "web", flag, host, user.CompanyId, xml_message.ToString());
        //                     }
        //                     else
        //                     {
        //                         err_msg = ViewResources.SharedStrings.AccountNoActiveTab;
        //                         ModelState.AddModelError("", err_msg);
        //                     }
        //                 }
        //		else
        //		{
        //			err_msg = ViewResources.SharedStrings.AccountNoActiveRole;
        //			ModelState.AddModelError("", err_msg);
        //		}
        //	}
        //	else
        //	{
        //		err_msg = ViewResources.SharedStrings.AccountIncorrectPasword;
        //		ModelState.AddModelError("", err_msg);
        //	}
        //         }

        //return Json(new
        //{
        //	IsSucceed = ModelState.IsValid,
        //	Msg = ModelState.IsValid ? string.Empty : err_msg
        //}); 
        //     }

        public ActionResult UserLogOn(string UserName, string Password, string returnUrl)
        {
            var err_msg = string.Empty;
            if (ModelState.IsValid)
            {
                User user;
                FoxSecDBContext db = new FoxSecDBContext();
                if (_membershipService.ValidateUser(UserName, Password, out user))
                {
                    var user_roles = user.UserRoles;
                    var check_date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    var role = user_roles.Where(ur => !ur.IsDeleted && ur.ValidTo >= check_date && ur.ValidFrom <= check_date).FirstOrDefault();
                    
                    if (role == null)
                    {
                        err_msg = ViewResources.SharedStrings.LogonRoleError;
                        ModelState.AddModelError("", err_msg);
                    }
                    else
                    {
                        int role_id = role.RoleId;
                        Session["Role_ID"] = role_id;
                        Session["User_Id"] = role.UserId;
                        //int comp_id = role.CompanyId;
                        var rolename = db.UserRoles.SqlQuery("select Id,Name from Roles where Id=" + role_id).ToList();
                        con.Open();
                        SqlCommand cmd = new SqlCommand("select FirstName+' '+LastName from Users where id='" + role.UserId + "'", con);
                        string firstname = Convert.ToString(cmd.ExecuteScalar());
                        con.Close();
                        //rolee = db.Database.ExecuteSqlCommand("select Name from Roles where Id="+role_id);
                        if (rolename != null)
                        {
                            Session["Role_Name"] = rolename[0].Name;
                        }
                        if (role != null)
                        {
                            //var rvm = CreateViewModel<RoleEditViewModel>();
                            //Mapper.Map(_roleRepository.FindById(role_id), rvm.Role);
                            var menues = role.Role.Menues.ToList().Contains(1);
                            if (menues)
                            {
                                _formsService.SignIn(UserName, false);

                                var host = Request.UserHostAddress;
                                var xml_message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                                var logon_params = new List<string>();
                                logon_params.Add(firstname);
                                var xml_user_logon = XMLLogMessageHelper.TemplateToXml("LogMessageUserLogon", logon_params);
                                xml_message.Add(xml_user_logon);
                                var flag = "";

                                _logService.CreateLog(user.Id, "web", flag, host, user.CompanyId, xml_message.ToString());
                            }
                            else
                            {
                                err_msg = ViewResources.SharedStrings.AccountNoActiveTab;
                                ModelState.AddModelError("", err_msg);
                            }
                        }
                        else
                        {
                            err_msg = ViewResources.SharedStrings.AccountNoActiveRole;
                            ModelState.AddModelError("", err_msg);
                        }
                    }
                }
                else
                {
                    err_msg = ViewResources.SharedStrings.AccountIncorrectPasword;
                    ModelState.AddModelError("", err_msg);
                }
            }
            return Json(new
            {
                IsSucceed = ModelState.IsValid,
                Msg = ModelState.IsValid ? string.Empty : err_msg
            });
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                User user;

                if (_membershipService.ValidateUser(model.UserName, model.Password, out user))
                {
                    _formsService.SignIn(model.UserName, model.RememberMe);

                    if (!string.IsNullOrWhiteSpace(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    //CheckUserAuthentication(model.UserName);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", ViewResources.SharedStrings.AccountIncorrectPasword);
            }
            // If we got this far, something failed, redisplay form
            var avm = CreateViewModel<LogOnModel>();
            avm.UserName = model.UserName;
            avm.Password = model.Password;
            avm.RememberMe = model.RememberMe;
            return RedirectToAction("LogOn", model);
        }

        [HttpPost]
        public ActionResult LogOff()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var userId = ((FoxSecIdentity)System.Web.HttpContext.Current.User.Identity).Id;
                var userName = ((FoxSecIdentity)System.Web.HttpContext.Current.User.Identity).LoginName;
                var companyId = CurrentUser.Get().CompanyId;
                var host = Request.UserHostAddress;
                var xml_message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                var logoff_params = new List<string>();
                logoff_params.Add(userName);
                var xml_user_logoff = XMLLogMessageHelper.TemplateToXml("LogMessageUserLogOff", logoff_params);
                xml_message.Add(xml_user_logoff);
                string flag = "";
                _logService.CreateLog(userId, "web", flag, host, companyId, xml_message.ToString());
                //illi 25.12.1012  Logger4SendingEMail.LogSender.Info(string.Format("User \"{0}\" has left", userName));
                //illi 25.12.1012   Logger4SendingEMail.InitLogger();
                _formsService.SignOut();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
