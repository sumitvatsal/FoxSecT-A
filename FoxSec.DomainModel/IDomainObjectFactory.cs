using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.DomainModel
{
	public interface IDomainObjectFactory
	{
        BuildingObject CreateBuildingObject();
        BuildingObjectType CreateBuildingObjectType();
        Building CreateBuilding();
	    Company CreateCompany();
        CompanyBuildingObject CreateCompanyBuildingObject();
        Country CreateCountry();
	    Department CreateDepartment();
        Holiday CreateHoliday();
        Location CreateLocation();
        Role CreateRole();
        Title CreateTitle();
        UserDepartment CreateUserDepartment();
		UserRole CreateUserRole();
        HolidayBuilding CreateHolidayBuilding();
		User CreateUser();
        UsersAccessUnit CreateUsersAccessUnit();
        Classificator CreateClassificator();
        ClassificatorValue CreateClassificatorValue();
        UserAccessUnitType CreateUserAccessUnitType();
	    CompanyManager CreateCompanyManager();
        UserPermissionGroup CreateUserPermissionGroup();
        UserPermissionGroupTimeZone CreateUserPermissionGroupTimeZone();
		Log CreateLog();
		LogType CreateLogType();
		LogFilter CreateLogFilter();
		RoleType CreateRoleType();
		RoleBuilding CreateRoleBuilding();
        //TimeZone CreateTimeZone();
        //TimeZoneProperty CreateTimeZoneProperty();
        UserTimeZone CreateUserTimeZone();
        UserTimeZoneProperty CreateUserTimeZoneProperty();
		ControllerUpdate CreateControllerUpdate();
		UserBuilding CreateUserBuilding();
        TAReport CreateTAReport();
        FSINISetting CreateFSINISettings();
        TAMove CreateTAMove();
        //CameraPermissions CreateCameraPermissions();
        Visitor CreateVisitor();
    }
}