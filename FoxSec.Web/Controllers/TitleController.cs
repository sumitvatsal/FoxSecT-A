using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using FoxSec.Authentication;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Repositories;
using FoxSec.Infrastructure.EntLib.Logging;
using FoxSec.ServiceLayer.Contracts;
using FoxSec.Web.Helpers;
using FoxSec.Web.ViewModels;

namespace FoxSec.Web.Controllers
{
    public class TitleController : BusinessCaseController
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly ITitleRepository _titleRepository;
        private readonly ITitleService _titleService;
    	private readonly IBuildingRepository _buildingRepository;
    	private readonly IUserRepository _userRepository;

		public TitleController( ITitleService titleService,
                                ITitleRepository titleRepository,
                                ICurrentUser currentUser,
                                ICompanyRepository companyRepository,
								IBuildingRepository buildingRepository,
                                IUserRepository userRepository,
                                ILogger logger)
			: base(currentUser, logger)
		{
			_companyRepository = companyRepository;
			_titleRepository = titleRepository;
			_buildingRepository = buildingRepository;
			_userRepository = userRepository;
			_titleService = titleService;
		}

        public ActionResult TabContent()
        {
            var hmv = CreateViewModel<HomeViewModel>();
            return PartialView(hmv);
        }

        [HttpGet]
        public ActionResult List()
        {
            var tlvm = CreateViewModel<TitleListViewModel>();

			IEnumerable<Title> titles =
        		_titleRepository.FindAll(
        			x =>
        			x.IsDeleted == false &&
        			(CurrentUser.Get().CompanyId == null || x.CompanyId.Equals(CurrentUser.Get().CompanyId))).OrderBy(t=>t.Name);

        	var full_company_ids = GetCompaniesIds();
        	titles = titles.Where(x => full_company_ids.Contains(x.CompanyId));

            Mapper.Map(titles, tlvm.Titles);

            return PartialView(tlvm);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var tevm = CreateViewModel<TitleEditViewModel>();
            Mapper.Map(_titleRepository.FindById(id), tevm.Title);
            tevm.Companies = new SelectList(GetCompanies(), "Id", "Name", tevm.Title.CompanyId);
            return PartialView(tevm);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var rvm = CreateViewModel<TitleEditViewModel>();
        	var companies = GetCompanies();
            rvm.Companies = new SelectList(companies, "Id", "Name");
            return View(rvm);
        }

    	private IEnumerable<Company> GetCompanies()
    	{
    		var full_company_ids = GetCompaniesIds();
			var companies =
				_companyRepository.FindAll(
					c =>
					!c.IsDeleted && c.Active && (CurrentUser.Get().CompanyId == null || c.Id.Equals(CurrentUser.Get().CompanyId)));
			companies = companies.Where(x => full_company_ids.Contains(x.Id)).OrderBy(cc=>cc.Name.ToLower());

    		return companies;
    	}

		private IEnumerable<int> GetCompaniesIds()
		{
			var building_ids = GetUserBuildings(_buildingRepository, _userRepository);

			var company_ids = from comp in
								  _companyRepository.FindAll(
									  c =>
									  !c.IsDeleted && c.Active &&
									  c.CompanyBuildingObjects.Any(cbo => !cbo.IsDeleted && building_ids.Contains(cbo.BuildingObject.BuildingId)))
							  select comp.Id;

			var full_company_ids = from cc in _companyRepository.FindAll(x => !x.IsDeleted && x.Active
																			  &&
																			  (company_ids.Contains(x.Id) ||
																			   (x.ParentId != null &&
																				company_ids.Contains(x.ParentId.Value))))
								   select cc.Id;

			return full_company_ids;
		}

		[HttpPost]
		public ActionResult Create(TitleEditViewModel tevm)
		{
			var out_tevm = CreateViewModel<TitleEditViewModel>();
			out_tevm.Title = tevm.Title;
			string err_msg = string.Empty;
			if (ModelState.IsValid)
			{
				try
				{
					_titleService.CreateTitle(tevm.Title.Name, tevm.Title.Description, tevm.Title.CompanyId);
				}
				catch (Exception ex)
				{
					err_msg = ex.Message;
					ModelState.AddModelError("", err_msg);
				}
			}
			else
			{
				var companies = GetCompanies();
				out_tevm.Companies = new SelectList(companies, "Id", "Name");
			}

			return Json(new
			{
				IsSucceed = ModelState.IsValid,
				Msg = ModelState.IsValid ? ViewResources.SharedStrings.DataSavingMessage : err_msg,
				DisplayMessage = !string.IsNullOrEmpty(err_msg),
				viewData = ModelState.IsValid ? null : this.RenderPartialView("Create", out_tevm)
			});
		}

        [HttpPost]
        public ActionResult Edit(TitleEditViewModel tevm)
        {
			var out_tevm = CreateViewModel<TitleEditViewModel>();
			out_tevm.Title = tevm.Title;
			string err_msg = string.Empty;
			if (ModelState.IsValid)
			{
				try
				{
					_titleService.EditTitle((int)tevm.Title.Id, tevm.Title.Name, tevm.Title.Description, tevm.Title.CompanyId);
				}
				catch (Exception ex)
				{
					err_msg = ex.Message;
					ModelState.AddModelError("", err_msg);
				}
			}
			else
			{
				var companies = GetCompanies();
				out_tevm.Companies = new SelectList(companies, "Id", "Name");
			}

			return Json(new
			{
				IsSucceed = ModelState.IsValid,
				Msg = ModelState.IsValid ? string.Format(ViewResources.SharedStrings.TitlesTitleSaving, tevm.Title.Name) : err_msg,
				DisplayMessage = !string.IsNullOrEmpty(err_msg),
				viewData = ModelState.IsValid ? null : this.RenderPartialView("Edit", out_tevm)
			});
		}

		[HttpPost]
		public ActionResult Delete(int id)
		{
			try
			{
				_titleService.DeleteTitle(id);
			}
			catch( Exception e )
			{
				Logger.Write("Error deleting Title Id=" + id, e);
			}

			return RedirectToAction("List");
		}
    }
}