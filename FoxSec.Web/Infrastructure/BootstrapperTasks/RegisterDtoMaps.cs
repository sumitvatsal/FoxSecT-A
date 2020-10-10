using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using FoxSec.Audit.Data;
using FoxSec.Common.Extensions;
using FoxSec.Core.Infrastructure.Bootstrapper;
using FoxSec.Core.SystemEvents;
using FoxSec.Core.SystemEvents.DTOs;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.ServiceLayer.Services;
using FoxSec.Web.ViewModels;
//using TimeZone = FoxSec.DomainModel.DomainObjects.TimeZone;

namespace FoxSec.Web.Infrastructure.BootstrapperTasks
{
	public class RegisterDtoMaps : IBootstrapperTask
	{
		public void Execute()
		{
			RegisterBuildingRelatedMap();
			
			RegisterBuildingObjectRelatedMaps();

			RegisterUserBuildingRelatedMap(); 

			RegisterLogFilterRelatedMap();

			RegisterUserRelatedMaps();

			RegisterClassificatorRelatedMaps();

			RegisterDepartmentRelatedMaps();

		    RegisterHolidayRelatedMaps();

            RegisterTAReportRelatedMaps();

            RegisterFSINISettingsRelatedMaps();

            RegisterRoleRelatedMaps();

		    RegisterTitleRelatedMaps();

		    RegisterUserAccessUnitTypeRelatedMaps();

			RegisterUserDepartmentRelatedMaps();

			RegisterUserAccessUnitRelatedMaps();

		    RegisterCompanyRelatedMap();

            RegisterVisitorRelatedMaps();//Added for visitor

            Mapper.CreateMap<AuditEventArgsBase, AuditLog>()
				.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => string.Format("{0} - {1} {2}", src.LoginName, src.FirstName, src.LastName)));

			Mapper
				.CreateMap<UserRole, string>()
				.ConvertUsing(userRole => userRole.Role.Description);

			Mapper
				.CreateMap<RoleBuilding, string>()
				.ConvertUsing(rb=> rb.Building.Name);

			Mapper.CreateMap<User, AuditEventUser>();

			Mapper
				.CreateMap<Role, AuditEventRole>()
				.ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => (from p in src.GetPermissionSet() where p.Value select p.Key.GetDisplayName()).ToList()));

            Mapper.CreateMap<Company, PartnerItem>();

            Mapper.CreateMap<TimeZone, TimeZoneItem>();

            Mapper.CreateMap<UserPermissionGroup, PermissionItem>();
           
            Mapper.CreateMap<CameraPermissions, value>();

            Mapper.CreateMap<FSCameras, item>();
            Mapper.CreateMap<FSCameras, value>();

            /*Mapper.CreateMap<TimeZoneProperty, TimeZonePropertiesViewModel>()
                .ForMember(dest => dest.ValidFromStr, opt => opt.MapFrom(src => src.ValidFrom != null ? src.ValidFrom.Value.ToString("HH:mm") : string.Empty))
                .ForMember(dest => dest.ValidToStr, opt => opt.MapFrom(src => src.ValidTo != null ? src.ValidTo.Value.ToString("HH:mm") : string.Empty));
            */
            Mapper.CreateMap<UserTimeZone, TimeZoneItem>();

            Mapper.CreateMap<UserTimeZoneProperty, TimeZonePropertiesViewModel>()
                .ForMember(dest => dest.ValidFromStr, opt => opt.MapFrom(src => src.ValidFrom != null ? src.ValidFrom.Value.ToString("HH:mm") : string.Empty))
                .ForMember(dest => dest.ValidToStr, opt => opt.MapFrom(src => src.ValidTo != null ? src.ValidTo.Value.ToString("HH:mm") : string.Empty));
        }

        private void RegisterVisitorRelatedMaps()
        {
            //Visitor

            Mapper.CreateMap<Visitor, VisitorEntity>()
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company))
                .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.CompanyId));
                    //.ForMember(dest => dest.StartDateTime, opt => opt.MapFrom(src => src.StartDateTime == null ? "Empty" : src.StopDateTime.Value.ToString("dd.MM.yyyy HH:mm")))
                    //.ForMember(dest => dest.StopDateTime, opt => opt.MapFrom(src => src.StopDateTime == null ? "Empty" : src.StopDateTime.Value.ToString("dd.MM.yyyy HH:mm")))
                   // .ForMember(dest => dest.ReturnDate, opt => opt.MapFrom(src => src.ReturnDate));
            
        }

        private void RegisterCompanyRelatedMap()
	    {
	        Mapper.CreateMap<Company, CompanyItem>()
                .ForMember(dest => dest.CompanyBuildingObjects, opt => opt.MapFrom(src => src.CompanyBuildingObjects))
                .ForMember(dest=>dest.Comment, opt=>opt.MapFrom(src=>src.Active ? src.Comment : 
                    src.ClassificatorValueId.HasValue ? src.ClassificatorValue.Value : src.Comment));
	    }

	    private static void RegisterUserAccessUnitRelatedMaps()
		{
			Mapper.CreateMap<UsersAccessUnit, UserAccessUnitListItem>()
				.ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.TypeId != null ? src.UserAccessUnitType.Name : string.Empty))
                .ForMember(dest => dest.DeactivationReason, opt => opt.MapFrom(src => src.ClassificatorValue != null ? src.ClassificatorValue.Value : String.Empty))
                .ForMember(dest => dest.DeactivationDateTime, opt => opt.MapFrom(src => src.Classificator_dt.HasValue ? src.Classificator_dt.Value.ToString("dd.MM.yyyy") : String.Empty))
				.ForMember(dest => dest.ValidFromStr, opt => opt.MapFrom(src => src.ValidFrom != null ? src.ValidFrom.Value.ToString("dd.MM.yyyy") : string.Empty))
				.ForMember(dest => dest.ValidToStr, opt => opt.MapFrom(src => src.ValidTo != null ? src.ValidTo.Value.ToString("dd.MM.yyyy") : string.Empty))
				.ForMember(dest=>dest.CompanyName, opt=>opt.MapFrom(src=>src.CompanyId == null ? "-" : src.Company.Name))
                .ForMember(src=>src.Name, opt=>opt.MapFrom(src=>src.UserId == null ? "-" : string.Format("{0} {1}", src.User.FirstName, src.User.LastName)))
				.ForMember(dest=>dest.Building, opt=>opt.MapFrom(src=>src.Building.Name));

			Mapper.CreateMap<UsersAccessUnit, UserAccessUnitItem>()
                .ForMember(dest => dest.ValidFromStr, opt => opt.MapFrom(src => src.ValidFrom != null ? src.ValidFrom.Value.ToString("dd.MM.yyyy") : string.Empty))
                .ForMember(dest => dest.ValidToStr, opt => opt.MapFrom(src => src.ValidTo != null ? src.ValidTo.Value.ToString("dd.MM.yyyy") : string.Empty));

			Mapper.CreateMap<UsersAccessUnit, UserAccessUnitEntity>()
				.ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.TypeId != null ? src.UserAccessUnitType.Name : "empty"))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserId != null ? src.User.LoginName : "empty"))
				.ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyId != null ? src.Company.Name : "empty"))
				.ForMember(dest => dest.ValidFrom, opt => opt.MapFrom(src => src.ValidFrom != null ? src.ValidFrom.Value.ToString("dd.MM.yyyy") : "empty"))
				.ForMember(dest => dest.ValidTo, opt => opt.MapFrom(src => src.ValidTo != null ? src.ValidTo.Value.ToString("dd.MM.yyyy") : "empty"))
				//.ForMember(dest => dest.Opened, opt => opt.MapFrom(src => src.Opened != null ? src.Opened.Value.ToString("dd.MM.yyyy") : "empty"))
				//.ForMember(dest => dest.Closed, opt => opt.MapFrom(src => src.Closed != null ? src.Closed.Value.ToString("dd.MM.yyyy") : "empty"))
                ;

	        Mapper.CreateMap<CompanyFloorItem, CompanyBuildingDto>();

	        Mapper.CreateMap<CompanyBuildingObject, CompanyFloorItem>()
	            .ForMember(dest => dest.IsSelected, opt => opt.MapFrom(src => !src.IsDeleted));

	    	Mapper.CreateMap<UserAccessUnitType, SelectListItem>()
	    		.ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
	    		.ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));
		}

		//private static string GetBuildingsString(IEnumerable<UserBuildingObject> userBuildingObjects)
		//{
		//    var buildings = new List<string>();
		//    foreach (var ubo in userBuildings.Where(ubo => !buildings.Contains(ubo.BuildingObject.Building.Name)))
		//    {
		//        buildings.Add(ubo.BuildingObject.Building.Name);
		//    }
		//    return string.Join(", ", buildings);
		//}

		private static void RegisterUserDepartmentRelatedMaps()
		{
			Mapper.CreateMap<UserDepartment, DepartmentItem>();
			Mapper.CreateMap<UserDepartment, SelectListItem>()
				.ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : string.Empty))
				.ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.DepartmentId.ToString()));

			Mapper.CreateMap<UserDepartment, UserDepartmentItem>()
				.ForMember(dest=>dest.DepartmentName, opt=>opt.MapFrom(src=>src.Department.Name));
			Mapper.CreateMap<UserDepartment, DepartmentManager>();

			Mapper.CreateMap<UserDepartment, UserDepartmentEntity>()
				.ForMember(dest=>dest.UserName, opt=>opt.MapFrom(src=>src.User.LoginName))
				.ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
				.ForMember(dest=>dest.ValidFrom, opt=>opt.MapFrom(src=>src.ValidFrom.ToString("dd.MM.yyyy")))
				.ForMember(dest => dest.ValidTo, opt => opt.MapFrom(src => src.ValidTo.ToString("dd.MM.yyyy")));
				
		}

		private static void RegisterUserAccessUnitTypeRelatedMaps()
	    {
            Mapper.CreateMap<UserAccessUnitType, CardTypeItem>();

	        Mapper.CreateMap<UserAccessUnitType, CardTypeEntity>();
	    }

	    private static void RegisterTitleRelatedMaps()
	    {
            Mapper.CreateMap<Title, TitleItem>();
            Mapper.CreateMap<Title, TitleEntity>()
                .ForMember(dest=>dest.CompanyName, opt=>opt.MapFrom(src=>src.Company.Name));
	    }

	    private static void RegisterRoleRelatedMaps()
	    {
           Mapper
                .CreateMap<Role, SelectListItem>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id.ToString()));

	        Mapper.CreateMap<Role, RoleEntity>()
				.ForMember(dest=>dest.RoleType, opt=>opt.MapFrom(src=>src.RoleType.Name));

	    	Mapper.CreateMap<RoleBuilding, RoleBuildingEntity>()
				.ForMember(dest=>dest.BuildingName, opt=>opt.MapFrom(src=>src.Building.Name));
	    }

	    private static void RegisterHolidayRelatedMaps()
	    {
            Mapper.CreateMap<Holiday, HolidayItem>();
	        Mapper.CreateMap<Holiday, HolidayEntity>()
                .ForMember(dest=>dest.EventStart, opt=>opt.MapFrom(src=>src.EventStart.ToString("dd.MM.yyyy")))
                .ForMember(dest=>dest.EventEnd, opt=>opt.MapFrom(src=>src.EventEnd.ToString("dd.MM.yyyy")));
	    }


        private static void RegisterTAReportRelatedMaps()
        {
            Mapper.CreateMap<TAReport, TAReportItem>();   //  TAPivotRowItem
            Mapper.CreateMap<TAReport, TAReportEntity>();
            /*              .ForMember(dest => dest.Started, opt => opt.MapFrom(src => src.Started.ToString("dd.MM.yyyy")))
                            .ForMember(dest => dest.Ended, opt => opt.MapFrom(src => src.Ended.ToString("dd.MM.yyyy")));*/
            Mapper.CreateMap<TAReport, TAReportMounthItem>();

        }
        private static void RegisterFSINISettingsRelatedMaps()
        {
            Mapper.CreateMap<FSINISetting, FSINISettingsItem>();
            Mapper.CreateMap<FSINISetting, FSINISettingsEntity>();

            /*              .ForMember(dest => dest.Started, opt => opt.MapFrom(src => src.Started.ToString("dd.MM.yyyy")))
                            .ForMember(dest => dest.Ended, opt => opt.MapFrom(src => src.Ended.ToString("dd.MM.yyyy")));*/


        }
        
        private static void RegisterTAMoveRelatedMaps()
        {
            Mapper.CreateMap<TAMove, TAMsUserItem>();   //  TAPivotRowItem
            Mapper.CreateMap<TAMove, TAMoveEntity>();
            /*              .ForMember(dest => dest.Started, opt => opt.MapFrom(src => src.Started.ToString("dd.MM.yyyy")))
                            .ForMember(dest => dest.Ended, opt => opt.MapFrom(src => src.Ended.ToString("dd.MM.yyyy")));*/
            Mapper.CreateMap<TAMove, TAMsUserViewModel>();

        }

        private void RegisterDepartmentRelatedMaps()
		{
			Mapper.CreateMap<Department, DepartmentItem>();

			Mapper.CreateMap<Department, DepartmentEntity>()
				.ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name));
		}

		private static void RegisterClassificatorRelatedMaps()
		{
			Mapper.CreateMap<Classificator, ClassificatorItem>();
			Mapper.CreateMap<ClassificatorValue, ClassificatorValueItem>();
			Mapper.CreateMap<Classificator, ClassificatorEntity>();
		}

		private static void RegisterUserRelatedMaps()
		{
			Mapper
				.CreateMap<User, UserItem>()
				.ForMember(dest => dest.UserDepartments, opt => opt.MapFrom(src => src.UserDepartments))
				.ForMember(dest => dest.RoleItems, opt => opt.MapFrom(src => from r in src.UserRoles
																			 select
																			 new SelectListItem
																			 {
																				 Selected = true,
																				 Text = r.Role.Name,
																				 Value = r.RoleId.ToString()
																			 }))
				.ForMember(dest => dest.IsCompanyManager, opt => opt.MapFrom(src => src.UserRoles != null && src.UserRoles.Any(ur => ur.Role.RoleTypeId == (int)RoleTypeEnum.CM && !ur.IsDeleted && ur.ValidFrom < DateTime.Now && ur.ValidTo > DateTime.Now)))
				.ForMember(dest => dest.IsBuildingAdmin, opt => opt.MapFrom(src => src.UserRoles != null && src.UserRoles.Any(ur => ur.Role.RoleTypeId == (int)RoleTypeEnum.BA && !ur.IsDeleted && ur.ValidFrom < DateTime.Now && ur.ValidTo > DateTime.Now)))
				.ForMember(dest => dest.IsSuperAdmin, opt => opt.MapFrom(src => src.UserRoles != null && src.UserRoles.Any(ur => ur.Role.RoleTypeId == (int)RoleTypeEnum.SA && !ur.IsDeleted && ur.ValidFrom < DateTime.Now && ur.ValidTo > DateTime.Now)))
				.ForMember(dest => dest.CardNumber, opt => opt.MapFrom(src => (src.UsersAccessUnits != null && src.UsersAccessUnits.Where(uau => !uau.IsDeleted && uau.Active && !uau.Free).Count() > 0) ?
					(!string.IsNullOrEmpty(src.UsersAccessUnits.Where(uau => !uau.IsDeleted && uau.Active && !uau.Free).First().Code) ? src.UsersAccessUnits.Where(uau => !uau.IsDeleted && uau.Active && !uau.Free).First().Code :
					 string.Format("{0}+{1}", src.UsersAccessUnits.Where(uau => !uau.IsDeleted && uau.Active && !uau.Free).First().Serial, src.UsersAccessUnits.Where(uau => !uau.IsDeleted && uau.Active && !uau.Free).First().Dk))
				: string.Empty))
				.ForMember(dest => dest.Roles, opt => opt.MapFrom(src => (src.UserRoles != null && src.UserRoles.Where(ur => !ur.IsDeleted && ur.ValidFrom < DateTime.Now && ur.ValidTo.AddDays(1)> DateTime.Now).Count() > 0) ?
					string.Join(", ", (from ur in src.UserRoles where ur.IsDeleted == false && ur.ValidFrom < DateTime.Now && ur.ValidTo.AddDays(1) > DateTime.Now select ur.Role.Name).ToArray())
					: string.Empty))
				.ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.UserDepartments != null && src.UserDepartments.Where(ud => !ud.IsDeleted).Count() ==1 ?
						src.UserDepartments.Where(ud => !ud.IsDeleted).FirstOrDefault().Department.Name
						: string.Empty))
						.ForMember(dest => dest.TitleName, opt => opt.MapFrom(src => src.Title == null ? string.Empty : src.Title.CompanyId == src.CompanyId ? src.Title.Name : string.Empty))
				.ForMember(dest => dest.ValidToStr, opt => opt.MapFrom(src => (src.UserRoles != null && src.UserRoles.Count > 0) ?
					src.UserRoles.First().ValidTo.ToString("dd.MM.yyyy") : string.Empty))
				.ForMember(dest => dest.ValidTo, opt => opt.MapFrom(src => (src.UserRoles != null && src.UserRoles.Count > 0) ?
					(DateTime?)src.UserRoles.First().ValidTo : null))
				.ForMember(dest=>dest.CompanyName, opt=>opt.MapFrom(src=>src.CompanyId == null ? "" : src.Company.Name))
				.ForMember(dest=>dest.EServiceAllowed, opt=>opt.MapFrom(src=>src.EServiceAllowed == null ? true : src.EServiceAllowed.Value));

			Mapper.CreateMap<User, SelectListItem>()
				.ForMember(dest => dest.Text, opt => opt.MapFrom(src => string.Format("{0} {1}", src.FirstName, src.LastName)))
				.ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id));


			Mapper.CreateMap<UserItem, User>();

			Mapper.CreateMap<User, UserEntity>()
				.ForMember(dest=>dest.TitleName, opt=>opt.MapFrom(src=>src.TitleId == null ? "Empty" : src.Title.Name))
				.ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyId == null ? "Empty" : src.Company.Name))
				.ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => src.Birthday == null ? "Empty" : src.Birthday.Value.ToString("dd.MM.yyyy")))
				.ForMember(dest => dest.ContractStartDate, opt => opt.MapFrom(src => src.ContractStartDate == null ? "Empty" : src.ContractStartDate.Value.ToString("dd.MM.yyyy")))
				.ForMember(dest => dest.ContractEndDate, opt => opt.MapFrom(src => src.ContractEndDate == null ? "Empty" : src.ContractEndDate.Value.ToString("dd.MM.yyyy")))
				.ForMember(dest => dest.PermitOfWork, opt => opt.MapFrom(src => src.PermitOfWork == null ? "Empty" : src.PermitOfWork.Value.ToString("dd.MM.yyyy")))
				.ForMember(dest => dest.RegistredStartDate, opt => opt.MapFrom(src => src.RegistredStartDate.ToString("dd.MM.yyyy")));

			Mapper.CreateMap<UserRoleItem, UserRoleDto>()
				.ForMember(dest => dest.ValidFrom, opt => opt.MapFrom(src => DateTime.ParseExact(src.ValidFrom.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture)))
				.ForMember(dest => dest.ValidTo, opt => opt.MapFrom(src => DateTime.ParseExact(src.ValidTo.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture)));
		}

       
        private static void RegisterUserBuildingRelatedMap()
		{
			Mapper.CreateMap<UserBuilding, UserBuildingItem>()
				.ForMember(dest=>dest.BuildingId, opt=>opt.MapFrom(src=>src.BuildingId))
				.ForMember(dest=>dest.BuildingName, opt=>opt.MapFrom(src=>src.Building.Name))
				.ForMember(dest=>dest.FloorName, opt=>opt.MapFrom(src=>src.BuildingObject == null ? "" : src.BuildingObject.Description)); 
		}

		private static void RegisterBuildingObjectRelatedMaps()
		{
			Mapper.CreateMap<BuildingObject, BuildingObjectItem>();
		}

		private static void RegisterBuildingRelatedMap()
		{
			Mapper.CreateMap<Building, SelectListItem>()
				.ForMember(dest=>dest.Text, opt=>opt.MapFrom(src=>src.Name))
				.ForMember(dest=>dest.Value, opt=>opt.MapFrom(src=>src.Id));
		}

		private static void RegisterLogFilterRelatedMap()
		{
			Mapper.CreateMap<LogFilter, LogFilterEntity>()
				.ForMember(dest=>dest.Company, opt=>opt.MapFrom(src=>src.Company == null ? "" : src.Company.Name))
				.ForMember(dest => dest.FromDate, opt => opt.MapFrom(src => src.FromDate != null ? src.FromDate.Value.ToString("dd.MM.yyyy") : string.Empty))
				.ForMember(dest => dest.ToDate, opt => opt.MapFrom(src => src.ToDate != null ? src.ToDate.Value.ToString("dd.MM.yyyy") : string.Empty))
				.ForMember(dest=>dest.Activity, opt=>opt.MapFrom(src=> string.IsNullOrEmpty(src.Activity) ? string.Empty : src.Activity.TrimEnd()));

        }
	}
}