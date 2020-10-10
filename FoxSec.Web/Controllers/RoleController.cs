using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Threading;
using System.Web.Mvc;
using AutoMapper;
using FoxSec.Authentication;
using FoxSec.Common.Extensions;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Repositories;
using FoxSec.Infrastructure.EntLib.Logging;
using FoxSec.ServiceLayer.Contracts;
using FoxSec.ServiceLayer.Services;
using FoxSec.Web.Helpers;
using FoxSec.Web.ViewModels;
using ViewResources;

namespace FoxSec.Web.Controllers
{
    public class RoleController : PaginatorControllerBase<RoleItem>
    {
        private readonly IRoleService _roleService;
    	private readonly IBuildingRepository _buildingRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IRoleTypeRepository _roleTypeRepository;
    	private ResourceManager _resourceManager;
    	private readonly IUserRepository _userRepository;    	
        public RoleController(  IRoleService roleService,
                                ICurrentUser currentUser,
                                IRoleRepository roleRepository,
			                    IBuildingRepository buildingRepository,
                                IRoleTypeRepository roleTypeRepository,
                                IUserRepository userRepository,
                                ILogger logger) : base(currentUser, logger)
        {
            _roleService = roleService;
            _roleRepository = roleRepository;
			_resourceManager = new ResourceManager("FoxSec.Web.Resources.Views.Shared.SharedStrings", typeof(SharedStrings).Assembly);
        	_buildingRepository = buildingRepository;
            _roleTypeRepository = roleTypeRepository;
        	_userRepository = userRepository;
			RegisterRoleRelatedMap();
        }

    	public ActionResult TabContent()
        {
            var hmv = CreateViewModel<HomeViewModel>();
            return PartialView(hmv);
        }

        [HttpPost]
		public ActionResult List(int filter, int? nav_page, int? rows, int? sort_field, int? sort_direction)
        {
            if (nav_page < 0)
            {
                nav_page = 0;
            }
            var rvm = CreateViewModel<RoleListViewModel>();
        	var building_ids = GetUserBuildings(_buildingRepository, _userRepository);

        	IEnumerable<Role> role_list =
        		_roleRepository.FindAll(
        			x =>
        			x.IsDeleted == false && x.RoleTypeId >= CurrentUser.Get().RoleTypeId &&
        			x.RoleBuildings.Any(rb => !rb.IsDeleted && building_ids.Contains(rb.BuildingId))).OrderBy(r=>r.Name.ToLower());

			if( filter == 0 )
			{
				role_list = role_list.Where(x => !x.Active);
			}

			if( filter == 1)
			{
				role_list = role_list.Where(x => x.Active);
			}
			if( !CurrentUser.Get().IsBuildingAdmin && !CurrentUser.Get().IsSuperAdmin )
			{
				role_list =
					role_list.Where(
						x => x.Id == CurrentUser.Get().RoleId || (x.User != null && CurrentUser.Get().CompanyId == x.User.CompanyId));
			}

        	IEnumerable<RoleItem> role_item_list = new List<RoleItem>();
        	Mapper.Map(role_list, role_item_list);
			if (sort_field.HasValue && sort_direction.HasValue)
			{
				switch (sort_field)
				{
					case 0:
						if (sort_direction.Value == 0) role_item_list = role_item_list.OrderBy(x => x.Name.ToUpper()).ToList();
						else role_item_list = role_item_list.OrderByDescending(x => x.Name.ToUpper()).ToList();
						break;
					case 1:
						if (sort_direction.Value == 0) role_item_list = role_item_list.OrderBy(x => x.RoleTypeId.ToString()).ToList();
						else role_item_list = role_item_list.OrderByDescending(x => x.RoleTypeId.ToString()).ToList();
						break;
					case 2:
						if (sort_direction.Value == 0) role_item_list = role_item_list.OrderBy(x => x.Description.ToUpper()).ToList();
						else role_item_list = role_item_list.OrderByDescending(x => x.Description.ToUpper()).ToList();
						break;
					case 3:
						if (sort_direction.Value == 0) role_item_list = role_item_list.OrderBy(x => x.BuildingsNames.ToUpper()).ToList();
						else role_item_list = role_item_list.OrderByDescending(x => x.BuildingsNames.ToUpper()).ToList();
						break;
					default:
						role_item_list = role_item_list.OrderBy(x => x.Name).OrderBy(x => x.Name).ToList();
						break;
				}
			}
			else
			{
				role_item_list = role_item_list.OrderBy(x => x.Name).ToList();
			}

			rvm.Paginator = SetupPaginator(ref role_item_list, nav_page, rows);
			rvm.Paginator.DivToRefresh = "RolesSearchResult";
			rvm.Paginator.Prefix = "Roles";
			rvm.Roles = role_item_list;
            return PartialView(rvm);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var rvm = CreateViewModel<RoleEditViewModel>();
        	rvm.Role.PermissionItems = from p in EnumExtension.GetAllValues<Permission>()
                                       select
                                       new RoleMenuFoxsecAccessItem()
                                       {
                                           IsSelected = false,
										   IsItemAvailable = true,
										   Text = (string)_resourceManager.GetObject(Enum.GetName(typeof(Permission), p), Thread.CurrentThread.CurrentCulture),
                                           Value = (int)p
                                       };

            rvm.Role.MenuItems = from m in EnumExtension.GetAllValues<Menu>()
                                 select
                                 new RoleMenuFoxsecAccessItem()
                                 {
                                     IsSelected = false,
									 IsItemAvailable = true,
									 Text = (string)_resourceManager.GetObject(Enum.GetName(typeof(Menu), m), Thread.CurrentThread.CurrentCulture),
                                     Value = ((int)m)
                                 };

        	rvm.Role.RoleTypeId = RoleTypeEnum.U;
        	rvm.Role.RoleBuildingItems = GetRoleBuildings(null, true);
            return View(rvm);
        }
       
        [HttpPost]
        public ActionResult Create(RoleEditViewModel revm)
        {
			string err = string.Empty;
        	var tevm = CreateViewModel<RoleEditViewModel>();
        	revm.User = tevm.User;
			if( !revm.Role.RoleBuildingItems.Any(rb=>rb.IsChecked) )
			{
				ModelState.AddModelError("Role.RoleBuildingItems", ViewResources.SharedStrings.RolesNoBuildingSelectedErrorMessage);
			}

			if (ModelState.IsValid)
			{
				try
				{
					var buildings = new List<RoleBuildingDto>();
					Mapper.Map(revm.Role.RoleBuildingItems, buildings);
					var result = _roleService.CreateRole(revm.Role.Name, revm.Role.Description ?? String.Empty, "admin_test", revm.Role.Active, (int)revm.Role.RoleTypeId, buildings);
					if( (RoleServiceErrorCode)result.ErrorCode == RoleServiceErrorCode.RoleAlreadyExists )
					{
                        TempData["ERROR"] = "TEST";
                        
						err = ViewResources.SharedStrings.RolesAlreadyExists;
						ModelState.AddModelError("", err);
                        RedirectToAction("Index");
					}
				}
				catch( Exception ex)
				{
					ModelState.AddModelError("", ex.Message);
					err = ex.Message;
				}
			}
			return Json(new
			{
				IsSucceed = ModelState.IsValid,
				Msg = ModelState.IsValid ? ViewResources.SharedStrings.DataSavingMessage : err,
				IsActive = revm.Role.Active,
				viewData = ModelState.IsValid ? string.Empty : this.RenderPartialView("Create", revm)
			}); 
        }
        [HttpGet]
        public void role() 
        {
            //int id = 130;
            var rvm = CreateViewModel<RoleEditViewModel>();
            //rvm.Role.Menues = GetMenuesByRoleType(id);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var rvm = CreateViewModel<RoleEditViewModel>();
            var role = _roleRepository.FindById(id);
            Mapper.Map(_roleRepository.FindById(id), rvm.Role);
        	rvm.Role.RoleBuildingItems = GetRoleBuildings(id);
        	var user_building_ids = GetUserBuildings(_buildingRepository, _userRepository);

        	rvm.IsViewOnlyMode = (!CurrentUser.Get().IsSuperAdmin && CurrentUser.Get().RoleId == id) 
				|| (user_building_ids.Count < rvm.Role.RoleBuildingItems.Where(rb=>rb.IsChecked).Count());

			return PartialView(rvm);
        }

        [HttpGet]
        public ActionResult GetMenuesByRoleType(RoleTypeEnum roleType)
        {
            var rvm = CreateViewModel<RoleEditViewModel>();
            var role_menues = _roleTypeRepository.FindById((int) roleType).Menues;
            var test1 = Enum.GetName(typeof(Menu), 17);
            var test2 = Thread.CurrentThread.CurrentCulture;
            rvm.Role.MenuItems = from m in EnumExtension.GetAllValues<Menu>()
                                 select
                                 new RoleMenuFoxsecAccessItem()
                                 {
                                     IsSelected = false,
                                     IsItemAvailable = (role_menues[(int)m] != 0 && CurrentUser.Get().Menues.IsAvailabe((int)m)) || (roleType == RoleTypeEnum.CM && m == Menu.ViewMyCompanyMenu) || (((int)m) >= 17),
									 Text = (string)_resourceManager.GetObject(Enum.GetName(typeof(Menu), m), Thread.CurrentThread.CurrentCulture),
                                     Value = ((int)m)
                                 };            
           return PartialView("MenuAccess", rvm);
        }

        [HttpPost]
        public ActionResult Edit(RoleEditViewModel revm)
        {
			string err = string.Empty;
			revm.IsViewOnlyMode = !CurrentUser.Get().IsSuperAdmin && CurrentUser.Get().RoleId == revm.Role.Id;
			if (!revm.Role.RoleBuildingItems.Any(rb => rb.IsChecked))
			{
				ModelState.AddModelError("Role.RoleBuildingItems", ViewResources.SharedStrings.RolesNoBuildingSelectedErrorMessage);
			}
			if (ModelState.IsValid)
			{
				try
				{
					var buildings = new List<RoleBuildingDto>();
					Mapper.Map(revm.Role.RoleBuildingItems, buildings);
				    
				    var perm_list = (from perm in revm.Role.PermissionItems where perm.IsSelected select perm.Value).ToList();
                    var menues_list = (from menu in revm.Role.MenuItems.Where(x=>x.IsItemAvailable && x.IsSelected) select menu.Value).ToList();
                   
                    var result = _roleService.EditRole((int) revm.Role.Id, revm.Role.Name, string.IsNullOrEmpty(revm.Role.Description) ? "" : revm.Role.Description, "admin_test", revm.Role.Active, (int)revm.Role.RoleTypeId, buildings, perm_list, menues_list);
					if ((RoleServiceErrorCode)result.ErrorCode == RoleServiceErrorCode.RoleAlreadyExists)
					{
						err = ViewResources.SharedStrings.RolesAlreadyExists;
						ModelState.AddModelError("", err);
					}
				}
				catch (Exception ex)
				{
					ModelState.AddModelError("", ex.Message);
					err = ex.Message;
				}
			}
			if( !ModelState.IsValid )
			{
				var tmp = CreateViewModel<RoleEditViewModel>();
				Mapper.Map(_roleRepository.FindById((int)revm.Role.Id), tmp.Role);
				revm.Role.MenuItems = tmp.Role.MenuItems;
			    revm.Role.PermissionItems = tmp.Role.PermissionItems;
				revm.User = tmp.User;
			}

        	return Json(new
			{
				IsSucceed = ModelState.IsValid,
				IsActive = revm.Role.Active,
				DisplayError = !string.IsNullOrWhiteSpace(err),
				Msg = ModelState.IsValid ? ViewResources.SharedStrings.DataSavingMessage : err,
				viewData = ModelState.IsValid ? string.Empty : this.RenderPartialView("Edit", revm)
			}); 
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            string err_msg = null;
            var role = _roleRepository.FindById(id);
            if (role.UserRoles.Where(x => !x.IsDeleted && !x.User.IsDeleted ).Count() > 0)
            {
                err_msg = string.Join(",",
                                      from ur in role.UserRoles.Where(x => !x.IsDeleted && !x.User.IsDeleted)
                                      select string.Format("{0} {1}", ur.User.FirstName, ur.User.LastName));

            	err_msg = string.Format("{0} {1}", ViewResources.SharedStrings.RolesRoleAlreadyAssigned, err_msg);
            }
            else
            {
                try
                {
                    _roleService.DeleteRole(id);
                }
                catch (Exception e)
                {
                    err_msg = ViewResources.SharedStrings.RolesDeleteRoleErrorMessage;
                    Logger.Write("Error deleting role Id=" + id, e);
                }
            }

            if (err_msg != null)
            {
                this.ModelState.AddModelError(string.Empty, err_msg);
            }
            return Json(new
            {
                IsSucceed = ModelState.IsValid,
                Msg = ModelState.IsValid ? ViewResources.SharedStrings.DataSavingMessage : err_msg
            });
        }

		private List<RoleBuildingItem> GetRoleBuildings(int? roleId, bool isCreateMode=false)
		{
			var result = new List<RoleBuildingItem>();
			var buildings = _buildingRepository.FindAll().Where(x => !x.IsDeleted);
			var role = roleId.HasValue ? _roleRepository.FindById(roleId.Value) : null;
			var building_ids = GetUserBuildings(_buildingRepository, _userRepository);

			foreach (var building in buildings)
			{
				var item = new RoleBuildingItem { BuildingId = building.Id, BuildingName = building.Name, RoleId = roleId, IsAvailable = building_ids.Contains(building.Id) };
				if (role != null)
				{
					var roleBuilding = role.RoleBuildings.Where(
						rb => rb.RoleId == roleId && rb.BuildingId == building.Id).FirstOrDefault();
					item.IsChecked = roleBuilding == null ? false : !roleBuilding.IsDeleted;
					item.Id = roleBuilding != null ? (int?) roleBuilding.Id : null;
				}
				result.Add(item);
			}
			if( isCreateMode )
			{
				result = result.Where(x => building_ids.Contains(x.BuildingId.Value)).ToList();
			}			
			return result;
		}

		private void RegisterRoleRelatedMap()
		{
			var user_build_Ids = GetUserBuildings(_buildingRepository, _userRepository);
			var activeRole = _roleRepository.FindById(CurrentUser.Get().RoleId);
			Mapper
			   .CreateMap<Role, RoleItem>()
			   .ForMember(dest => dest.PermissionItems, opt => opt.MapFrom(src => from p in src.GetPermissionSet()
																				  select
																				  new RoleMenuFoxsecAccessItem()
																				  {
																					  IsSelected = p.Value,
																					  IsItemAvailable = activeRole.Permissions[(int)p.Key] != 0 ,
																					  Text = (string)_resourceManager.GetObject(Enum.GetName(typeof(Permission), p.Key), Thread.CurrentThread.CurrentCulture),
																					  Value = (int)p.Key
																				  }))
			   .ForMember(dest => dest.MenuItems, opt => opt.MapFrom(src => from m in src.GetMenuSet()
																			select
																			new RoleMenuFoxsecAccessItem()
																			{
																				IsSelected = src.RoleType.Menues[(int)m.Key] == 0 ? false : m.Value,
																				IsItemAvailable = (src.RoleType.Menues[(int)m.Key] != 0 && CurrentUser.Get().Menues.IsAvailabe((int)m.Key)) || 
																				((RoleTypeEnum)src.RoleTypeId == RoleTypeEnum.CM && m.Key == Menu.ViewMyCompanyMenu),
																				Text = (string)_resourceManager.GetObject(Enum.GetName(typeof(Menu), m.Key), Thread.CurrentThread.CurrentCulture),
																				Value = (int)m.Key
																			}))
				.ForMember(dest => dest.DescriptionShort, opt => opt.MapFrom(src => 
					string.IsNullOrEmpty(src.Description) ? ""
					: src.Description.Length < 50 ? src.Description : string.Format("{0}...", src.Description.Substring(0,50))))
				.ForMember(dest=>dest.Description, opt=>opt.MapFrom(src=>src.Description.TrimEnd()))
			   .ForMember(dest => dest.RoleTypeId, opt => opt.MapFrom(src => src.RoleTypeId.HasValue ? (RoleTypeEnum)src.RoleTypeId.Value : RoleTypeEnum.U))
			   .ForMember(dest=>dest.BuildingsNames, opt=>opt.MapFrom(src=>src.RoleBuildings.Count == 0 ? string.Empty : 
				   string.Join(",", from rb in src.RoleBuildings.Where(x=>!x.IsDeleted) select rb.Building.Name)))
				.ForMember(dest=>dest.IsReadOnly, opt=>opt.MapFrom(src=>src.RoleBuildings.Where(rb=>!rb.IsDeleted).Count() > user_build_Ids.Count));

			Mapper.CreateMap<RoleBuildingItem, RoleBuildingDto>();
		}
    }
}