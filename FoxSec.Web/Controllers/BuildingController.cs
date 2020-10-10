using System;
using System.Collections.Generic;
using System.Linq;
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
using FoxSec.Core.Infrastructure.UnitOfWork;
using System.Data.Entity;

namespace FoxSec.Web.Controllers
{
    public class BuildingController : BusinessCaseController
    {
        private readonly IBuildingObjectRepository _buildingObjectRepository;
        private readonly IBuildingObjectService _buildingObjectService;
        private readonly IBuildingRepository _buildingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserPermissionGroupTimeZoneRepository _userPermissionGroupTimeZoneRepository;
        private readonly IUserPermissionGroupService _userPermissionGroupService;
        private readonly IUserPermissionGroupRepository _userPermissionGroupRepository;
        public BuildingController(ICurrentUser currentUser,
                                    ILogger logger,
                                    IBuildingObjectRepository buildingObjectRepository,
                                    IBuildingObjectService buildingObjectService,
                                    IBuildingRepository buildingRepository,
                                    IUserRepository userRepository,
                                    IUserPermissionGroupTimeZoneRepository userPermissionGroupTimeZoneRepository,
                                    IUserPermissionGroupService userPermissionGroupService,
                                    IUserPermissionGroupRepository userPermissionGroupRepository)
            : base(currentUser, logger)
        {
            _buildingObjectRepository = buildingObjectRepository;
            _buildingObjectService = buildingObjectService;
            _buildingRepository = buildingRepository;
            _userRepository = userRepository;
            _userPermissionGroupTimeZoneRepository = userPermissionGroupTimeZoneRepository;
            _userPermissionGroupService = userPermissionGroupService;
            _userPermissionGroupRepository = userPermissionGroupRepository;
        }

        public ActionResult Index()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult BuildingInfo(int buildingId)
        {
            var model = CreateViewModel<BuildingViewModel>();
            var building = _buildingRepository.FindById(buildingId);
            model.BuildingName = building.Name;

            var users =
                _userRepository.FindAll().Where(
                    x =>
                    !x.IsDeleted && x.Active &&
                    x.UserRoles.Any(ur => ur.ValidFrom < DateTime.Now && !ur.IsDeleted && ur.ValidTo.AddDays(1) > DateTime.Now && ur.Role.RoleTypeId == (int)RoleTypeEnum.BA) && x.UserBuildings.Any(ub => !ub.IsDeleted && ub.BuildingId == buildingId));

            var user_names = from us in users select string.Format("{0} {1}", us.FirstName, us.LastName);

            if (user_names.Count() > 0)
            {
                model.AdminName = string.Join(",", user_names);
            }
            return PartialView("BuildingInfo", model);
        }




        #region Tree

        public ActionResult GetTree(int? id)
        {
            if (CurrentUser.Get().IsCompanyManager && !_userPermissionGroupService.IsUserHasActivePermission(CurrentUser.Get().Id)) return null;

            var ctvm = new PermissionTreeViewModel();
            var role_buildingIds = GetUserBuildings(_buildingRepository, _userRepository, CurrentUser.Get().Id);

            if (CurrentUser.Get().IsCompanyManager || CurrentUser.Get().IsCommonUser)
            {
                UserPermissionGroup upg;
                if (id.HasValue)
                {
                    upg = _userPermissionGroupRepository.FindById(id.Value);
                }
                else
                {
                    if (_userPermissionGroupRepository.FindAll(x => !x.IsDeleted && x.IsOriginal && x.UserId == CurrentUser.Get().Id).ToList().Count() == 0) return null;
                    upg = _userPermissionGroupRepository.FindAll(x => !x.IsDeleted && x.IsOriginal && x.UserId == CurrentUser.Get().Id).FirstOrDefault();
                    id = upg.Id;
                }

                ctvm.IsOriginal = upg.IsOriginal;

                List<int> companyBuildingObjectsIds = (from cbo in upg.UserPermissionGroupTimeZones.Where(x => !x.IsDeleted) select cbo.BuildingObjectId).ToList();
                var companyBuildingObjects = _buildingObjectRepository.FindAll(x => !x.IsDeleted && role_buildingIds.Contains(x.BuildingId)
                    && companyBuildingObjectsIds.Contains(x.Id));

                List<int> floorsId = (from cbo in companyBuildingObjects.Where(x => x.ParentObjectId.HasValue) select cbo.ParentObjectId.Value).ToList();
                var companyFloorObjects = _buildingObjectRepository.FindAll(x => !x.IsDeleted && floorsId.Contains(x.Id));

                ctvm.Buildings =
                    (from b in
                         (from cbo in companyBuildingObjects select cbo.Building)
                     where !b.IsDeleted
                     select
                         new Node
                         {
                             ParentId = b.LocationId,
                             MyId = b.Id,
                             Name = b.Name
                         }).Distinct(new NodeEqualityComparer()).ToList();

                ctvm.Towns =
                    (from t in
                         (from cbo in companyBuildingObjects select cbo.Building.Location)
                     where !t.IsDeleted
                     select
                         new Node
                         {
                             ParentId = t.CountryId,
                             MyId = t.Id,
                             Name = t.Name
                         }).Distinct(new NodeEqualityComparer()).ToList();

                ctvm.Countries =
                    (from c in
                         (from cbo in companyBuildingObjects select cbo.Building.Location.Country).OrderBy(cc => cc.Name)
                     where !c.IsDeleted
                     select
                         new Node
                         {
                             ParentId = 0,
                             MyId = c.Id,
                             Name = c.Name
                         }).Distinct(new NodeEqualityComparer()).ToList();

                ctvm.Floors =
                    (from f in companyFloorObjects
                     where f.TypeId == (int)BuildingObjectTypes.Floor
                     select
                         new Node
                         {

                             ParentId = f.BuildingId,
                             MyId = f.Id,
                             Name = f.Description,
                             Comment = f.Comment
                         }).Distinct(new NodeEqualityComparer()).ToList();

                ctvm.Objects =
                    (from o in companyBuildingObjects
                     where o.ParentObjectId.HasValue && (o.TypeId == (int)BuildingObjectTypes.Door || o.TypeId == (int)BuildingObjectTypes.Lift || o.TypeId == (int)BuildingObjectTypes.Room)
                     orderby o.TypeId descending
                     select
                         new Node
                         {
                             ParentId = o.ParentObjectId.Value,
                             MyId = o.Id,
                             Name = o.Description,
                             Comment = o.Comment,
                             IsDefaultTimeZone = _userPermissionGroupService.IsDefaultUserTimeZone(o.Id, id.Value),
                             IsRoom = o.TypeId == (int)BuildingObjectTypes.Room ? 1 : 0,
                             StatusIcon = o.StatusIconId.HasValue ? String.Format("../../img/status/{0}.ico", o.StatusIconId) : String.Empty
                         }).Distinct(new NodeEqualityComparer()).ToList();

                ctvm.ActiveObjectIds = _userPermissionGroupService.GetUserBuildingObjectIds(id.Value);
                foreach (var obn in ctvm.Objects)
                {
                    if (obn.IsRoom == 1)
                    {
                        // illi 08.05.2018 UserPermissionGroupTimeZone upgtz = _userPermissionGroupTimeZoneRepository.FindAll(x => !x.IsDeleted && x.BuildingObjectId == obn.MyId && x.UserPermissionGroupId == id).FirstOrDefault();
                        int idd = id.GetValueOrDefault();
                        UserPermissionGroupTimeZone upgtz = _userPermissionGroupTimeZoneRepository.FindByPGId(idd).Where(x => !x.IsDeleted && x.BuildingObjectId == obn.MyId && x.UserPermissionGroupId == id).FirstOrDefault();

                        obn.IsArming = upgtz.IsArming;
                        obn.IsDefaultArming = upgtz.IsDefaultArming;
                        obn.IsDisarming = upgtz.IsDisarming;
                        obn.IsDefaultDisarming = upgtz.IsDefaultDisarming;
                    }
                }

                ctvm.Towns = ctvm.Towns.Where(town => ctvm.Buildings.Any(b => b.ParentId == town.MyId));
                ctvm.Countries = ctvm.Countries.Where(country => ctvm.Towns.Any(town => town.ParentId == country.MyId));
            }
            else
            if (!(CurrentUser.Get().IsCompanyManager || CurrentUser.Get().IsCommonUser))
            {
                IEnumerable<Building> userBuildings;

                if (CurrentUser.Get().IsSuperAdmin)
                {
                    userBuildings = _buildingRepository.FindAll(x => !x.IsDeleted).ToList();
                }
                else
                    if (CurrentUser.Get().IsBuildingAdmin)
                {
                    userBuildings = _buildingRepository.FindAll(x => role_buildingIds.Contains(x.Id));
                }
                else return null;

                ctvm.Buildings =
                    (from b in userBuildings
                     select
                         new Node
                         {
                             ParentId = b.LocationId,
                             MyId = b.Id,
                             Name = b.Name
                         }).Distinct(new NodeEqualityComparer()).ToList();

                ctvm.Towns =
                    (from t in
                            (from ub in userBuildings select ub.Location)
                     where !t.IsDeleted
                     select
                         new Node
                         {
                             ParentId = t.CountryId,
                             MyId = t.Id,
                             Name = t.Name
                         }).Distinct(new NodeEqualityComparer()).ToList();

                ctvm.Countries =
                    (from c in
                            (from cbo in userBuildings select cbo.Location.Country).OrderBy(cc => cc.Name)
                     where !c.IsDeleted
                     select
                         new Node
                         {
                             ParentId = 0,
                             MyId = c.Id,
                             Name = c.Name
                         }).Distinct(new NodeEqualityComparer()).ToList();

                List<int> userBuildingIds = (from ub in userBuildings select ub.Id).ToList();
                var userBuildingObjects = _buildingObjectRepository.FindAll(x => !x.IsDeleted && userBuildingIds.Contains(x.BuildingId)).ToList();

                ctvm.Floors =
                    (from f in userBuildingObjects
                     where f.TypeId == (int)BuildingObjectTypes.Floor
                     select
                         new Node
                         {
                             ParentId = f.BuildingId,
                             MyId = f.Id,
                             Name = f.Description,
                             Comment = f.Comment
                         }).Distinct(new NodeEqualityComparer()).ToList();

                ctvm.Objects =
                    (from o in userBuildingObjects
                     where o.ParentObjectId.HasValue && (o.TypeId == (int)BuildingObjectTypes.Door || o.TypeId == (int)BuildingObjectTypes.Lift || o.TypeId == (int)BuildingObjectTypes.Room)
                     orderby o.TypeId descending
                     select
                         new Node
                         {
                             ParentId = o.ParentObjectId.Value,
                             MyId = o.Id,
                             Name = o.ObjectNr.HasValue ? "#" + o.ObjectNr + " " + o.Description : o.Description,
                             Comment = o.Comment,
                             IsDefaultTimeZone = id.HasValue ? _userPermissionGroupService.IsDefaultUserTimeZone(o.Id, id.Value) : true,
                             IsRoom = o.TypeId == (int)BuildingObjectTypes.Room ? 1 : 0,
                             StatusIcon = o.StatusIconId.HasValue ? String.Format("../../img/status/{0}.ico", o.StatusIconId) : String.Empty
                         }).Distinct(new NodeEqualityComparer()).ToList();

                if (id.HasValue)
                {
                    ctvm.ActiveObjectIds = _userPermissionGroupService.GetUserBuildingObjectIds(id.Value);

                    foreach (var obn in ctvm.Objects)
                    {
                        if (obn.IsRoom == 1)
                        {
                            // illi 08.05.2018  UserPermissionGroupTimeZone pgtz = _userPermissionGroupTimeZoneRepository.FindAll(x => !x.IsDeleted && x.BuildingObjectId == obn.MyId && x.UserPermissionGroupId == id).FirstOrDefault();
                            int idd = id.GetValueOrDefault();
                            UserPermissionGroupTimeZone pgtz = _userPermissionGroupTimeZoneRepository.FindByPGId(idd).Where(x => !x.IsDeleted && x.BuildingObjectId == obn.MyId && x.UserPermissionGroupId == id).FirstOrDefault();

                            obn.IsArming = pgtz == null ? false : pgtz.IsArming;
                            obn.IsDefaultArming = pgtz == null ? false : pgtz.IsDefaultArming;
                            obn.IsDisarming = pgtz == null ? false : pgtz.IsDisarming;
                            obn.IsDefaultDisarming = pgtz == null ? false : pgtz.IsDefaultDisarming;
                        }
                    }
                }

                ctvm.Towns = ctvm.Towns.Where(town => ctvm.Buildings.Any(b => b.ParentId == town.MyId));
                ctvm.Countries = ctvm.Countries.Where(country => ctvm.Towns.Any(town => town.ParentId == country.MyId));
            }
            return PartialView("Tree", ctvm);
        }

        #endregion

        #region Create/Edit

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var bevm = CreateViewModel<BuildingObjectEditViewModel>();
            Mapper.Map(_buildingObjectRepository.FindById(id), bevm.BuildingObject);
            return PartialView(bevm);
        }

        [HttpPost]
        public ActionResult Edit(BuildingObjectEditViewModel bevm)
        {
            string err = string.Empty;
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(bevm.BuildingObject.Comment))
                {
                    bevm.BuildingObject.Comment = string.Empty;
                }
                try
                {
                    _buildingObjectService.SetComment(bevm.BuildingObject.Id.Value, bevm.BuildingObject.Comment, HostName);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    err = ex.Message;
                }
            }
            else
            {
                err = ViewResources.SharedStrings.FieldLengthErrorComment;
            }
            return Json(new
            {
                IsSucceed = ModelState.IsValid,
                Msg = ModelState.IsValid ? ViewResources.SharedStrings.CommonDataSavedMessage : err,
                IsEmpty = string.IsNullOrEmpty(bevm.BuildingObject.Comment),
                Comment = bevm.BuildingObject.Comment,
                viewData = this.RenderPartialView("Edit", bevm)
            });
        }
        #endregion
    }
}