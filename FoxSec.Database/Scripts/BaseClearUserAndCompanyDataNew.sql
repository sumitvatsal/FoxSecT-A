delete from dbo.UserPermissionGroupTimeZones
delete from dbo.UserPermissionGroups
delete from dbo.UserTimeZoneProperties
delete from dbo.UserTimeZones

delete from dbo.UserRoles where UserId != 1
delete from dbo.UsersAccessUnit
delete from dbo.UserDepartments
delete from dbo.UserBuildings where UserId != 1
delete from dbo.Log
delete from dbo.CompanyManagers

delete from dbo.FsControllersUsersSaveStatus where UserId !=1
delete from dbo.Users where Id > 1

delete from dbo.RoleBuildings where RoleId != 5
delete from dbo.Roles where Id != 5

delete from dbo.Departments

delete from dbo.Titles
delete from dbo.CompanyBuildingObjects
delete from dbo.LogFilters
delete from dbo.Companies