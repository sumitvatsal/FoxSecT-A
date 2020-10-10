using System;
using FoxSec.DomainModel.DomainObjects;
//using TimeZone = FoxSec.DomainModel.DomainObjects.TimeZone;

namespace FoxSec.DomainModel
{
	internal sealed class DefaultDomainObjectFactory : IDomainObjectFactory
	{
        public Holiday CreateHoliday()
        {
            return new Holiday();
        }

        public HolidayBuilding CreateHolidayBuilding()
        {
            return new HolidayBuilding();
        }

		public User CreateUser()
		{
			return new User();
		}

        public TAReport CreateTAReport()
        {
            return new TAReport();
        }

        public FSINISetting CreateFSINISettings()
        {
            return new FSINISetting();
        }

        public TAMove CreateTAMove()
        {
            return new TAMove();
        }

        public Title CreateTitle()
        {
            return new Title();
        }

        public UserDepartment CreateUserDepartment()
        {
            return new UserDepartment();
        }

        public Department CreateDepartment()
        {
            return new Department();
        }


	    public Classificator CreateClassificator()
	    {
	        return new Classificator();
	    }

        public ClassificatorValue CreateClassificatorValue()
        {
            return new ClassificatorValue();
        }

		public Role CreateRole()
		{
			return new Role();
		}

		public UserRole CreateUserRole()
		{
			return new UserRole();
		}

        public Company CreateCompany()
        {
            return new Company();
        }

        public CompanyManager CreateCompanyManager()
        {
            return new CompanyManager();
        }

        public CompanyBuildingObject CreateCompanyBuildingObject()
        {
            return new CompanyBuildingObject();
        }

        public BuildingObjectType CreateBuildingObjectType()
        {
            return new BuildingObjectType();
        }

        public BuildingObject CreateBuildingObject()
        {
            return new BuildingObject();
        }

        public Building CreateBuilding()
        {
            return new Building();
        }

        public Location CreateLocation()
        {
            return new Location();
        }

        public Country CreateCountry()
        {
            return new Country();
        }

        public UserAccessUnitType CreateUserAccessUnitType()
        {
            return new UserAccessUnitType();
        }
        /*
        public TimeZone CreateTimeZone()
        {
            return new TimeZone();
        }

        public TimeZoneProperty CreateTimeZoneProperty()
        {
            return new TimeZoneProperty();
        }
        */
        public UserPermissionGroup CreateUserPermissionGroup()
        {
            return new UserPermissionGroup();
        }

        public UserPermissionGroupTimeZone CreateUserPermissionGroupTimeZone()
        {
            return new UserPermissionGroupTimeZone();
        }

		public Log CreateLog()
		{
			return new Log();
		}

		public LogType CreateLogType()
		{
			return new LogType();
		}

		public LogFilter CreateLogFilter()
		{
			return new LogFilter();
		}

		public RoleType CreateRoleType()
		{
			return new RoleType();
		}

		public RoleBuilding CreateRoleBuilding()
		{
			return new RoleBuilding();
		}

        public UserTimeZone CreateUserTimeZone()
        {
            return new UserTimeZone();
        }

        public UserTimeZoneProperty CreateUserTimeZoneProperty()
        {
            return new UserTimeZoneProperty();
        }

		public ControllerUpdate CreateControllerUpdate()
		{
			return new ControllerUpdate();
		}

		public UserBuilding CreateUserBuilding()
		{
			return new UserBuilding();
		}

        public UsersAccessUnit CreateUsersAccessUnit()
        {
            return new UsersAccessUnit();
        }
        public Visitor CreateVisitor()
        {
            return new Visitor();
        }
    }
}