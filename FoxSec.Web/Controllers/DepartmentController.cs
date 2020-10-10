using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using FoxSec.Authentication;
using FoxSec.Common.Enums;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Repositories;
using FoxSec.Infrastructure.EntLib.Logging;
using FoxSec.ServiceLayer.Contracts;
using FoxSec.Web.Helpers;
using FoxSec.Web.ViewModels;
using System.Web.Mvc;
using System.Globalization;

namespace FoxSec.Web.Controllers
{
    public class DepartmentController : PaginatorControllerBase<Department>
	{
		private readonly ICompanyService _companyService;
		private readonly ICompanyRepository _companyRepository;
		private readonly IUserRepository _userRepository;
		private readonly IDepartmentRepository _departmentRepository;
		private readonly IUserDepartmentRepository _userDepartmentRepository;
		private readonly IDepartmentService _departmentService;
		private readonly IUserDepartmentService _userDepartmentService;
		private readonly ICurrentUser _currentUser;
		private readonly IBuildingRepository _buildingRepository;

		public DepartmentController(IDepartmentService departmentService,
									ICompanyRepository companyRepository,
									IUserDepartmentService userDepartmentService,
									IDepartmentRepository departmentRepository,
									ICompanyService companyService,
                                    ICurrentUser currentUser,
                                    IUserRepository userRepository,
									IUserDepartmentRepository userDepartmentRepository,
									IBuildingRepository buildingRepository,
                                    ILogger logger) : base(currentUser, logger)
		{
			_companyService = companyService;
			_companyRepository = companyRepository;
			_userDepartmentService = userDepartmentService;
			_userRepository = userRepository;
			_departmentRepository = departmentRepository;
			_departmentService = departmentService;
			_currentUser = currentUser;
			_userDepartmentRepository = userDepartmentRepository;
			_buildingRepository = buildingRepository;
		}

        public ActionResult TabContent()
        {
            var hmv = CreateViewModel<HomeViewModel>();
            return PartialView(hmv);
        }

        #region Search
        //public ActionResult List(int? nav_page, int? rows, int? sort_field, int? sort_direction)
        //{
        //          if (nav_page < 0)
        //          {
        //              nav_page = 0;
        //          }
        //          var dlvm = CreateViewModel<DepartmentListViewModel>();
        //	var role_building_ids = GetUserBuildings(_buildingRepository, _userRepository);			

        //          IEnumerable<Department> departments = null;

        //          if (_currentUser.Get().RoleTypeId == (int)FixedRoleType.CompanyManager)
        //          {
        //              int? companyId = _currentUser.Get().CompanyId;
        //              List<int> companyList = (from c in _companyRepository.FindAll(c => !c.IsDeleted && (c.Id == companyId || c.ParentId == companyId)) select c.Id).ToList();
        //              if (companyId.HasValue)
        //                  departments = _departmentRepository.FindAll(d => !d.IsDeleted && companyList.Contains(d.CompanyId));
        //          }
        //          else
        //          {
        //              var companies =
        //              _companyRepository.FindAll().Where(
        //                  x => !x.IsDeleted &&
        //                  x.CompanyBuildingObjects.Where(cbo => !cbo.IsDeleted).Count() == 0 ||
        //                  x.CompanyBuildingObjects.Any(cbo => !cbo.IsDeleted && role_building_ids.Contains(cbo.BuildingObject.BuildingId))).ToList();

        //              departments = _departmentRepository.FindAll(d => !d.IsDeleted && companies.Any(c => c.Id == d.CompanyId));
        //          }

        //          IEnumerable<Department> dep = null;
        //          departments.ToList().ForEach(x => x.Manager = GetDepartmentManagerName(x.Id));
        //          if (sort_field.HasValue && sort_direction.HasValue)
        //          {
        //              if (sort_field.Value == 0) dep = sort_direction == 0 ? departments.OrderBy(x => x.Number) : departments.OrderByDescending(x => x.Number);
        //              else if (sort_field.Value == 1) dep = sort_direction == 0 ? departments.OrderBy(x => x.Name) : departments.OrderByDescending(x => x.Name);
        //              else dep = sort_direction == 0 ? departments.OrderBy(x => x.Manager) : departments.OrderByDescending(x => x.Manager);
        //          }

        //          dlvm.Paginator = SetupPaginator(ref dep, nav_page, rows);
        //          dlvm.Paginator.DivToRefresh = "DepartmentsList";
        //          dlvm.Paginator.Prefix = "Departments";
        //          Mapper.Map(dep, dlvm.Departments);
        //	foreach (var d in dlvm.Departments)
        //	{
        //		d.Manger = GetDepartmentManagerName(d.Id);
        //	}
        //	return PartialView(dlvm);
        //}
        public ActionResult List(int? nav_page, int? rows, int? sort_field, int? sort_direction)
        {
            if (nav_page < 0)
            {
                nav_page = 0;
            }
            var dlvm = CreateViewModel<DepartmentListViewModel>();
            var role_building_ids = GetUserBuildings(_buildingRepository, _userRepository);

            IEnumerable<Department> departments = null;

            if (_currentUser.Get().RoleTypeId == (int)FixedRoleType.CompanyManager)
            {
                int? companyId = _currentUser.Get().CompanyId;
                List<int> companyList = (from c in _companyRepository.FindAll(c => !c.IsDeleted && (c.Id == companyId || c.ParentId == companyId)) select c.Id).ToList();
                if (companyId.HasValue)
                    departments = _departmentRepository.FindAll(d => !d.IsDeleted && companyList.Contains(d.CompanyId));
            }
            else
            {
                var companies =
                _companyRepository.FindAll().Where(
                    x => !x.IsDeleted &&
                    x.CompanyBuildingObjects.Where(cbo => !cbo.IsDeleted).Count() == 0 ||
                    x.CompanyBuildingObjects.Any(cbo => !cbo.IsDeleted && role_building_ids.Contains(cbo.BuildingObject.BuildingId))).ToList();

                departments = _departmentRepository.FindAll(d => !d.IsDeleted && companies.Any(c => c.Id == d.CompanyId));
            }

            IEnumerable<Department> dep = departments;
            //departments.ToList().ForEach(x => x.Manager = GetDepartmentManagerName(x.Id));            

            dlvm.Paginator = SetupPaginator(ref dep, nav_page, rows);
            Mapper.Map(dep, dlvm.Departments);
            foreach (var d in dlvm.Departments)
            {
                d.Manger = GetDepartmentManagerName(d.Id);
            }
            if (sort_field.HasValue && sort_direction.HasValue)
            {
                if (sort_field.Value == 0) dlvm.Departments = sort_direction == 0 ? dlvm.Departments.OrderBy(x => x.Number) : dlvm.Departments.OrderByDescending(x => x.Number);
                else if (sort_field.Value == 1) dlvm.Departments = sort_direction == 0 ? dlvm.Departments.OrderBy(x => x.Name) : dlvm.Departments.OrderByDescending(x => x.Name);
                else dlvm.Departments = sort_direction == 0 ? dlvm.Departments.OrderBy(x => x.Manger) : dlvm.Departments.OrderByDescending(x => x.Manger);
            }
           
            dlvm.Paginator.DivToRefresh = "DepartmentsList";
            dlvm.Paginator.Prefix = "Departments";
            return PartialView(dlvm);
        }

        #endregion

        #region Create

        [HttpGet]
		public ActionResult Create()
		{
			var dvm = CreateViewModel<DepartmentEditViewModel>();
			dvm.Department.CompanyId = _currentUser.Get().CompanyId == null ? -1 : CurrentUser.Get().CompanyId.Value;
			var building_ids = GetUserBuildings(_buildingRepository, _userRepository);

			var company_ids = from comp in
				_companyRepository.FindAll(
					c =>
					!c.IsDeleted && c.Active &&
					c.CompanyBuildingObjects.Any(cbo => !cbo.IsDeleted && building_ids.Contains(cbo.BuildingObject.BuildingId)))
								  select comp.Id;

			dvm.CompanyList = new SelectList(_companyRepository.FindAll(c => !c.IsDeleted && c.Active && company_ids.Contains(c.Id)).OrderBy(x=>x.Name.ToLower()), "Id", "Name", dvm.Department.CompanyId);
			return View(dvm);
		}

		[HttpPost]
		public ActionResult Create(DepartmentEditViewModel model)
		{
			var devm = CreateViewModel<DepartmentEditViewModel>();

			devm.Department.CompanyId = model.Department.CompanyId;
			var building_ids = GetUserBuildings(_buildingRepository, _userRepository);

			var company_ids = from comp in
								  _companyRepository.FindAll(
									  c =>
									  !c.IsDeleted && c.Active &&
									  c.CompanyBuildingObjects.Any(cbo => !cbo.IsDeleted && building_ids.Contains(cbo.BuildingObject.BuildingId)))
							  select comp.Id;

			devm.CompanyList = new SelectList(_companyRepository.FindAll(c => !c.IsDeleted && c.Active && company_ids.Contains(c.Id)), "Id", "Name", devm.Department.CompanyId);
			devm.Department.DepartmentManagersList = _companyService.GetCompanyManagers(devm.Department.CompanyId);

			devm.Department = model.Department;
			devm.Managers = model.Managers;

			string err = string.Empty;
			if (ModelState.IsValid && model.Department.CompanyId != -1)
			{
				try
				{
					devm.Department.Id = _departmentService.CreateDepartment(devm.Department.Number, devm.Department.Name,
													_currentUser.Get().LoginName, devm.Department.CompanyId);
					SaveManagers(model);
				}
				catch (Exception ex)
				{
					ModelState.AddModelError("", ex.Message);
					err = ex.Message;
				}
			}

			var res = ModelState.IsValid ? "" : this.RenderPartialView("Create", devm);

			return Json(new
			{
				IsSucceed = ModelState.IsValid,
				Msg = ModelState.IsValid ? ViewResources.SharedStrings.DataSavingMessage : err,
				viewData = res
			});
		}

		#endregion

		#region Edit

		DepartmentEditViewModel CreateDepartmentEditViewModel(int departmentId)
		{
			var devm = CreateViewModel<DepartmentEditViewModel>();
			Mapper.Map(_departmentRepository.FindById(departmentId), devm.Department);
			Mapper.Map(GetDepartmentManagers(departmentId), devm.Managers);
			var building_ids = GetUserBuildings(_buildingRepository, _userRepository);
			var company_ids = from comp in
								  _companyRepository.FindAll(
									  c =>
									  !c.IsDeleted && c.Active &&
									  c.CompanyBuildingObjects.Any(cbo => !cbo.IsDeleted && building_ids.Contains(cbo.BuildingObject.BuildingId)))
							  select comp.Id;
			devm.CompanyList = new SelectList(_companyRepository.FindAll(c => !c.IsDeleted && c.Active && company_ids.Contains(c.Id)).OrderBy(x=>x.Name.ToLower()), "Id", "Name", devm.Department.CompanyId);
			devm.Department.DepartmentManagersList = _companyService.GetCompanyManagers(devm.Department.CompanyId);
			foreach (var manager in devm.Managers)
			{
				if (manager.UserId.HasValue)
				{
					var period = GetValidationPeriod(manager.UserId.Value, departmentId);
					manager.ValidFrom = period.Key.ToString("dd.MM.yyyy");
                    manager.ValidTo = period.Value.ToString("dd.MM.yyyy");
				}
			}
			return devm;
		}

		[HttpGet]
		public ActionResult Edit(int departmentId)
		{
			var devm = CreateDepartmentEditViewModel(departmentId);
			return PartialView(devm);
		}

		[HttpPost]
		public ActionResult Edit(DepartmentEditViewModel model)
		{
			var devm = CreateDepartmentEditViewModel(model.Department.Id);
			devm.Department = model.Department;
			devm.Managers = model.Managers;

			string err = string.Empty;
			if (ModelState.IsValid)
			{
				try
				{
					_departmentService.EditDepartment(model.Department.Id, model.Department.Number, model.Department.Name,
											  _currentUser.Get().LoginName, model.Department.CompanyId);
					SaveManagers(model);
				}
				catch (Exception ex)
				{
					ModelState.AddModelError("", ex.Message);
					err = ex.Message;
				}
			}
			var res = this.RenderPartialView("Edit", devm);
			return Json(new
			{
				IsSucceed = ModelState.IsValid,
				Msg = ModelState.IsValid ? ViewResources.SharedStrings.DepartmentsSavingMessage : err,
				viewData = res
			});
		}

		private void SaveManagers(DepartmentEditViewModel model)
		{
			_userDepartmentService.DeleteDepartmentUserWithRole(model.Department.Id, (int)FixedRoleType.DepartmentManager);
			int companyId = _departmentRepository.FindById(model.Department.Id).CompanyId;
			var managers = _companyService.GetCompanyManagers(companyId);
			foreach (var manager in model.Managers)
			{
				if( manager.UserId.HasValue && managers.ContainsKey(manager.UserId.Value))
				{
					_userDepartmentService.AddDepartmentManager(manager.UserId.Value, model.Department.Id, DateTime.ParseExact(manager.ValidFrom, "dd.MM.yyyy", CultureInfo.InvariantCulture),
																DateTime.ParseExact(manager.ValidTo, "dd.MM.yyyy", CultureInfo.InvariantCulture));
				}
			}
		}

		[HttpGet]
		public ActionResult ManagerList(int departmentId)
		{
			var devm = CreateDepartmentEditViewModel(departmentId);
			return PartialView(devm);
		}

		[HttpGet]
		public ActionResult Manager(int companyId)
		{
			var devm = CreateViewModel<DepartmentEditViewModel>();
			devm.Department.CompanyId = companyId;
			var building_ids = GetUserBuildings(_buildingRepository, _userRepository);
			var company_ids = from comp in
								  _companyRepository.FindAll(
									  c =>
									  !c.IsDeleted && c.Active &&
									  c.CompanyBuildingObjects.Any(cbo => !cbo.IsDeleted && building_ids.Contains(cbo.BuildingObject.BuildingId)))
							  select comp.Id;

			devm.CompanyList = new SelectList(_companyRepository.FindAll(c => !c.IsDeleted && c.Active && company_ids.Contains(c.Id)), "Id", "Name", devm.Department.CompanyId);
			devm.Department.DepartmentManagersList = _companyService.GetCompanyManagers(devm.Department.CompanyId);
			devm.ManagerId = 0;
			return PartialView("Manager", devm);
		}

		[HttpGet]
		public ActionResult NextManager(int id, int departmentId)
		{
			var devm = CreateDepartmentEditViewModel(departmentId);
			devm.ManagerId = id;
			return PartialView("Manager", devm);
		}

		#endregion

		#region Delete

		[HttpPost]
		public ActionResult Delete(int[] departmentsIds)
		{
			var err = string.Empty;
			List<string> non_deleted_departments_list = (from departmentId in departmentsIds
			                                             select _departmentRepository.FindById(departmentId)
			                                             into department where department.UserDepartments.Any(x => !x.IsDeleted) select department.Name).ToList();
			if( non_deleted_departments_list.Count > 0 )
			{
				var non_deleted_departments = string.Join(",", non_deleted_departments_list);
				err = string.Format(ViewResources.SharedStrings.DepartmentsDepartmentsContainsUsersError, non_deleted_departments);
				ModelState.AddModelError("", err);
			}
			foreach (int id in departmentsIds)
			{
				try
				{
					_departmentService.DeleteDepartment(id);
				}
				catch( Exception e )
				{
					Logger.Write("Error deleting Department Id=" + id, e);
					err = string.Format("{0}. {1}", "Error deleting Department Id=" + id, e.Message);
					ModelState.AddModelError("", e);
				}
			}
			return Json(new
			{
				IsSucceed = ModelState.IsValid,
				Msg = ModelState.IsValid ? ViewResources.SharedStrings.DataSavingMessage : err
			}); 
		}

		#endregion

		#region Helpers

		[HttpGet]
		public ActionResult GetValidFromForUser(int userId, int departmentId)
		{
			return Content(GetValidationPeriod(userId, departmentId).Key.ToString("dd.MM.yyyy"));
		}

		public ActionResult GetValidToForUser(int userId, int departmentId)
		{
            return Content(GetValidationPeriod(userId, departmentId).Value.ToString("dd.MM.yyyy"));
		}

		private KeyValuePair<DateTime, DateTime> GetValidationPeriod(int userId, int departmentId)
		{
			UserDepartment userDepartment = _userDepartmentRepository.FindByUserId(userId).Where(ud => ud.DepartmentId == departmentId).SingleOrDefault();
			if (userDepartment == null)
			{
				User user = _userRepository.FindById(userId);

				foreach (UserRole ur in user.UserRoles)
				{
					if (ur.Role.RoleTypeId == (int)FixedRoleType.DepartmentManager && !ur.IsDeleted && ur.ValidFrom < DateTime.Now && ur.ValidTo > DateTime.Now.AddDays(1) )
					{
						return new KeyValuePair<DateTime, DateTime>(ur.ValidFrom, ur.ValidTo);
					}
				}
			}
            return new KeyValuePair<DateTime, DateTime>(userDepartment.ValidFrom, userDepartment.ValidTo);
		}

		[HttpPost]
		public ActionResult DeleteManager(int userId, int departmentId)
		{
			_userDepartmentService.DeleteUserDepartment(userId, departmentId);
			return null;
		}

		#endregion

		#region Department users

		public UserDepartmentListViewModel GetUserDepartmentListViewModel(int userId)
		{
			var udlvm = CreateViewModel<UserDepartmentListViewModel>();
			var user = _userRepository.FindById(userId);

			Mapper.Map(_departmentRepository.FindAll(d => !d.IsDeleted && d.CompanyId == user.CompanyId), udlvm.Departments);

			if (_currentUser.Get().RoleTypeId == (int)FixedRoleType.CompanyManager)
			{
				int? companyId = CurrentUser.Get().CompanyId;
				List<int> companyList = (from c in _companyRepository.FindAll(c => !c.IsDeleted && (c.Id == companyId || c.ParentId == companyId)) select c.Id).ToList();
				Mapper.Map(_departmentRepository.FindAll(d => !d.IsDeleted && companyList.Contains(d.CompanyId)), udlvm.Departments);
			}

			Mapper.Map(_userDepartmentRepository.FindByUserId(userId).Where(ud => !ud.IsDeleted), udlvm.UserDepartments);

			foreach (var ud in udlvm.UserDepartments)
			{
				ud.Manager = GetDepartmentManagerName(ud.DepartmentId);
                ud.ValidFrom = DateTime.Parse(ud.ValidFrom).ToString("dd.MM.yyyy");
                ud.ValidTo = DateTime.Parse(ud.ValidTo).ToString("dd.MM.yyyy");
			}
			udlvm.UserId = userId;
			return udlvm;
		}

		[HttpGet]
		public ActionResult UserDepartmentList(int userId)
		{
			return PartialView(GetUserDepartmentListViewModel(userId));
		}

		[HttpPost]
		public ActionResult DeleteUserDepartment(int userDepartmentId, int userId)
		{
			try
			{
				_userDepartmentService.DeleteUserDepartment(userDepartmentId);
			}
			catch (Exception e)
			{
				Logger.Write("Error deleting User department Id=" + userDepartmentId, e);
			}
			return RedirectToAction("UserDepartmentList", new { userId });
		}

		public IEnumerable<UserDepartment> GetDepartmentManagers(int departmentId)
		{
			int companyId = _departmentRepository.FindById(departmentId).CompanyId;
			var companyManagers = _companyService.GetCompanyManagers(companyId);
			IEnumerable<UserDepartment> managersList = _userDepartmentService.GetDepartmentManagers(departmentId).Where(ud => ud.Department.CompanyId == companyId);
			managersList = managersList.Where(ud => companyManagers.ContainsKey(ud.UserId)).OrderBy(ud=>ud.User.FirstName.ToLower()).ThenBy(ud=>ud.User.LastName.ToLower());
			return managersList;
		}

		public string GetDepartmentManagerName(int departmentId)
		{
			return string.Join(", ", (from m in GetDepartmentManagers(departmentId) select m.User.FirstName + " " + m.User.LastName).ToArray());
		}

		[HttpGet]
		public ActionResult GetDepartmentManager(int departmentId)
		{
			return Content(GetDepartmentManagerName(departmentId));
		}

		[HttpGet]
		public ActionResult AddUserDepartment(int userId)
		{
			UserDepartmentListViewModel udlvm = GetUserDepartmentListViewModel(userId);
            ((ICollection<UserDepartmentItem>)udlvm.UserDepartments).Add(new UserDepartmentItem() { UserId = userId, ValidFrom = DateTime.Now.ToString("dd.MM.yyyy"), ValidTo = DateTime.Now.ToString("dd.MM.yyyy") });
			return PartialView("UserDepartmentList", udlvm);
		}

		[HttpPost]
		public ActionResult SaveUserDepartments(UserDepartmentListViewModel udlvm)
		{
			_userDepartmentService.DeleteUserFromDepartments(udlvm.UserId);
			foreach (var ud in udlvm.UserDepartments)
			{
				if (ud.DepartmentId != 0)
                    _userDepartmentService.UpdateUserDepartment(udlvm.UserId, ud.DepartmentId, DateTime.ParseExact(ud.ValidFrom, "dd.MM.yyyy", CultureInfo.InvariantCulture), DateTime.ParseExact(ud.ValidTo, "dd.MM.yyyy", CultureInfo.InvariantCulture));
			}
			return null;
		}

		[HttpPost]
		public ActionResult DeleteUsers(UserDepartmentListViewModel udlvm)
		{
			foreach (var ud in udlvm.UserDepartments)
			{
                _userDepartmentService.UpdateUserDepartment(udlvm.UserId, ud.DepartmentId, DateTime.ParseExact(ud.ValidFrom, "dd.MM.yyyy", CultureInfo.InvariantCulture), DateTime.ParseExact(ud.ValidTo, "dd.MM.yyyy", CultureInfo.InvariantCulture), ud.IsForDelete);
			}
			return null;
		}

		#endregion

		#region Move User

		[HttpGet]
		public ActionResult MoveToDepartment(int departmentId)
		{
			var mtdvm = CreateViewModel<MoveToDepartmentViewModel>();
			int companyId = _departmentRepository.FindById(departmentId).CompanyId;
			mtdvm.Departments = new SelectList(_departmentRepository.FindAll(x => !x.IsDeleted && x.CompanyId == companyId), "Id", "Name");
			return PartialView(mtdvm);
		}

		[HttpPost]
		public ActionResult MoveToDepartment(int[] usersIds, int oldDepartmentId, int departmentId)
		{
			foreach (int id in usersIds)
			{
				_userDepartmentService.MoveToDepartment(id, oldDepartmentId, departmentId);
			}
			return null;
		}

		#endregion

        public JsonResult GetDepartments()
        {
            StringBuilder result = new StringBuilder();
            result.Append("<option value=" + '"' + '"' + ">"+ ViewResources.SharedStrings.DefaultDropDownValue +"</option>");
            List<Department> deps = new List<Department>();
			if (CurrentUser.Get().RoleTypeId == (int)FixedRoleType.CompanyManager)
            {
                int? companyId = CurrentUser.Get().CompanyId;
                if (companyId.HasValue)
                {
                    List<int> companyList = (from c in _companyRepository.FindAll(c => !c.IsDeleted && (c.Id == companyId || c.ParentId == companyId)) select c.Id).ToList();
                    deps = _departmentRepository.FindAll(d => !d.IsDeleted && companyList.Contains(d.CompanyId)).ToList();
                }
            }
            else
                deps = _departmentRepository.FindAll(d => !d.IsDeleted).ToList();
            foreach (var dp in deps)
            {
                result.Append("<option value=" + '"' + dp.Id + '"' + ">" + dp.Name + "</option>");
            }
            return Json(result.ToString(), JsonRequestBehavior.AllowGet);
        }

		public JsonResult GetDepartmentsByCompany(string companyName)
		{
			StringBuilder result = new StringBuilder();
			result.Append("<option value=" + '"' + '"' + ">" + ViewResources.SharedStrings.DefaultDropDownValue + "</option>");
			Company comp = null;
			if( !string.IsNullOrWhiteSpace(companyName) )
			{
				comp = _companyRepository.FindAll().Where(cc => cc.Name.ToLower() == companyName.ToLower()).FirstOrDefault();
			}
			List<Department> deps = new List<Department>();
			if( comp == null )
			{
				deps = _departmentRepository.FindAll().Where(dep => !dep.IsDeleted).ToList();
			}
			else
			{
				deps = _departmentRepository.FindAll().Where(dep => !dep.IsDeleted && dep.CompanyId == comp.Id).ToList();
			}			

			foreach (var dp in deps)
			{
				result.Append("<option value=" + '"' + dp.Id + '"' + ">" + dp.Name + "</option>");
			}
			return Json(result.ToString(), JsonRequestBehavior.AllowGet);
		}
	}
}