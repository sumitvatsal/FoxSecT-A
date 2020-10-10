using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using FoxSec.DomainModel;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;
//using TimeZone = FoxSec.DomainModel.DomainObjects.TimeZone;

namespace FoxSec.Infrastructure.EF
{
    internal sealed class ProxyBasedDomainObjectFactory : IDomainObjectFactory
    {
        private readonly IDatabase _database;

        public ProxyBasedDomainObjectFactory(IDatabaseFactory factory)
        {
            _database = factory.Get();
        }
        public HolidayBuilding CreateHolidayBuilding()
        {
            return _database.CreateObject<HolidayBuilding>();
        }
        public User CreateUser()
        {
            return _database.CreateObject<User>();
        }

        public Role CreateRole()
        {
            var role = _database.CreateObject<Role>();

            role.Permissions = new byte[400];
            role.Menues = new byte[400];

            return role;
        }

        public UserRole CreateUserRole()
        {
            return _database.CreateObject<UserRole>();
        }

        public Title CreateTitle()
        {
            return _database.CreateObject<Title>();
        }

        public Holiday CreateHoliday()
        {
            return _database.CreateObject<Holiday>();
        }

        public TAReport CreateTAReport()
        {
            return _database.CreateObject<TAReport>();
        }

        public FSINISetting CreateFSINISettings()
        {
            return _database.CreateObject<FSINISetting>();
        }

        public TAMove CreateTAMove()
        {
            return _database.CreateObject<TAMove>();
        }

        public Company CreateCompany()
        {
            return _database.CreateObject<Company>();
        }

        public CompanyManager CreateCompanyManager()
        {
            return _database.CreateObject<CompanyManager>();
        }

        public Department CreateDepartment()
        {
            return _database.CreateObject<Department>();
        }

        public Country CreateCountry()
        {
            return _database.CreateObject<Country>();
        }

        public UsersAccessUnit CreateusersAccessUnit()
        {
            return _database.CreateObject<UsersAccessUnit>();
        }

        public BuildingObject CreateBuildingObject()
        {
            return _database.CreateObject<BuildingObject>();
        }

        public BuildingObjectType CreateBuildingObjectType()
        {
            return _database.CreateObject<BuildingObjectType>();
        }

        public Building CreateBuilding()
        {
            return _database.CreateObject<Building>();
        }

        public CompanyBuildingObject CreateCompanyBuildingObject()
        {
            return _database.CreateObject<CompanyBuildingObject>();
        }

        public Location CreateLocation()
        {
            return _database.CreateObject<Location>();
        }

        public UserDepartment CreateUserDepartment()
        {
            return _database.CreateObject<UserDepartment>();
        }

        public UsersAccessUnit CreateUsersAccessUnit()
        {
            return _database.CreateObject<UsersAccessUnit>();
        }

        public Classificator CreateClassificator()
        {
            return _database.CreateObject<Classificator>();
        }

        public ClassificatorValue CreateClassificatorValue()
        {
            return _database.CreateObject<ClassificatorValue>();
        }

        public UserAccessUnitType CreateUserAccessUnitType()
        {
            return _database.CreateObject<UserAccessUnitType>();
        }
        /*
        public TimeZone CreateTimeZone()
        {
            return _database.CreateObject<TimeZone>();
        }
        
        public TimeZoneProperty CreateTimeZoneProperty()
        {
            return _database.CreateObject<TimeZoneProperty>();
        }
        */
        public UserPermissionGroup CreateUserPermissionGroup()
        {
            return _database.CreateObject<UserPermissionGroup>();
        }

        public UserPermissionGroupTimeZone CreateUserPermissionGroupTimeZone()
        {
            return _database.CreateObject<UserPermissionGroupTimeZone>();
        }

        public Log CreateLog()
        {
            return _database.CreateObject<Log>();
        }

        public LogType CreateLogType()
        {
            return _database.CreateObject<LogType>();
        }

        public LogFilter CreateLogFilter()
        {
            return _database.CreateObject<LogFilter>();
        }

        public RoleType CreateRoleType()
        {
            return _database.CreateObject<RoleType>();
        }

        public RoleBuilding CreateRoleBuilding()
        {
            return _database.CreateObject<RoleBuilding>();
        }

        public UserTimeZone CreateUserTimeZone()
        {
            return _database.CreateObject<UserTimeZone>();
        }

        public UserTimeZoneProperty CreateUserTimeZoneProperty()
        {
            return _database.CreateObject<UserTimeZoneProperty>();
        }

        public ControllerUpdate CreateControllerUpdate()
        {
            return _database.CreateObject<ControllerUpdate>();
        }

        public UserBuilding CreateUserBuilding()
        {
            return _database.CreateObject<UserBuilding>();
        }

        public Visitor CreateVisitor()
        {
            return _database.CreateObject<Visitor>();
        }
    }
}