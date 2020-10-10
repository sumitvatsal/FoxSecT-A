namespace FoxSec.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
        //    CreateTable(
        //        "dbo.Terminals",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                ShowScreensaver = c.Boolean(nullable: false),
        //                ScreenSaverShowAfter = c.Time(nullable: false, precision: 7),
        //                MaxUserId = c.Int(nullable: false),
        //                CompanyId = c.String(),
        //                TerminalId = c.String(),
        //                ApprovedDevice = c.Boolean(),
        //                Name = c.String(),
        //                IsDeleted = c.Boolean(),
        //                InfoKioskMode = c.Boolean(),
        //                SoundAlarms = c.String(),
        //                ShowMaxAlarmsFistPage = c.Int(),
        //                LastLogin = c.DateTime(),
        //            })
        //        .PrimaryKey(t => t.Id);
            
        //    CreateTable(
        //        "dbo.BuildingObjects",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                BuildingId = c.Int(nullable: false),
        //            })
        //        .PrimaryKey(t => t.Id);
            
        //    CreateTable(
        //        "dbo.Buildings",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                Name = c.String(),
        //                TimediffGMTMinutes = c.Int(),
        //                AdressStreet = c.String(),
        //                AdressHouse = c.String(),
        //                AdressIndex = c.String(),
        //                LocationId = c.Int(nullable: false),
        //                IsDeleted = c.Boolean(nullable: false),
        //                Floors = c.Int(nullable: false),
        //                TimezoneId = c.String(),
        //            })
        //        .PrimaryKey(t => t.Id);
            
        //    CreateTable(
        //        "dbo.CamPermission",
        //        c => new
        //            {
        //                CameraID = c.Int(nullable: false, identity: true),
        //                CameraName = c.String(),
        //                CompanyID = c.Int(),
        //            })
        //        .PrimaryKey(t => t.CameraID);
            
        //    CreateTable(
        //        "dbo.Companies",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                ParentId = c.Int(),
        //                ClassificatorValueId = c.Int(),
        //                Name = c.String(),
        //                ModifiedBy = c.String(),
        //                ModifiedLast = c.DateTime(nullable: false),
        //                Comment = c.String(),
        //                Active = c.Boolean(nullable: false),
        //                IsDeleted = c.Boolean(nullable: false),
        //                IsCanUseOwnCards = c.Boolean(nullable: false),
        //                Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
        //            })
        //        .PrimaryKey(t => t.Id);
            
        //    CreateTable(
        //        "dbo.Countries",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                Name = c.String(),
        //                ISONumber = c.Int(nullable: false),
        //                IsDeleted = c.Boolean(nullable: false),
        //            })
        //        .PrimaryKey(t => t.Id);
            
        //    CreateTable(
        //        "dbo.Custormers",
        //        c => new
        //            {
        //                CustomerId = c.Int(nullable: false, identity: true),
        //                CustomerName = c.String(),
        //                CountryId = c.Int(),
        //                CityId = c.Int(),
        //            })
        //        .PrimaryKey(t => t.CustomerId);
            
        //    CreateTable(
        //        "dbo.Departments",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                Name = c.String(),
        //                IsDeleted = c.Boolean(nullable: false),
        //                CompanyId = c.Int(nullable: false),
        //            })
        //        .PrimaryKey(t => t.Id);
            
        //    CreateTable(
        //        "dbo.FSBuildingObjectCameras",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                BuildingObjectId = c.Int(nullable: false),
        //                CameraId = c.Int(nullable: false),
        //                IsDeleted = c.Boolean(nullable: false),
        //            })
        //        .PrimaryKey(t => t.Id);
            
        //    CreateTable(
        //        "dbo.FSCameras",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                Name = c.String(),
        //                status = c.String(),
        //                ServerNr = c.Int(nullable: false),
        //                CameraNr = c.Int(nullable: false),
        //                Port = c.Int(nullable: false),
        //                ResX = c.Int(nullable: false),
        //                ResY = c.Int(nullable: false),
        //                Skip = c.Int(nullable: false),
        //                Delay = c.Int(nullable: false),
        //                QuickPreviewSeconds = c.Int(nullable: false),
        //            })
        //        .PrimaryKey(t => t.Id);
            
        //    CreateTable(
        //        "dbo.FSHRs",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                FoxSecFieldName = c.String(),
        //                HRFieldname = c.String(),
        //                FoxSecTableId = c.Int(nullable: false),
        //                IsIndex = c.Boolean(nullable: false),
        //                AutoUpdate = c.Boolean(nullable: false),
        //                FieldType = c.Int(nullable: false),
        //                IsDeleted = c.Boolean(nullable: false),
        //                IndexFilename = c.String(),
        //                FoxsecTableName = c.String(),
        //                FSHR_Id = c.Int(),
        //            })
        //        .PrimaryKey(t => t.Id)
        //        .ForeignKey("dbo.FSHRs", t => t.FSHR_Id)
        //        .Index(t => t.FSHR_Id);
            
        //    CreateTable(
        //        "dbo.FSINISettings",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                SoftType = c.Int(nullable: false),
        //                Value = c.String(),
        //                IsDeleted = c.Boolean(nullable: false),
        //            })
        //        .PrimaryKey(t => t.Id);
            
        //    CreateTable(
        //        "dbo.FSVideoServers",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                Name = c.String(),
        //                IP = c.String(),
        //                UID = c.String(),
        //                PWD = c.String(),
        //            })
        //        .PrimaryKey(t => t.Id);
            
        //    CreateTable(
        //        "dbo.Hr_Clone",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                ois_id_isik = c.String(),
        //                Personal_code = c.String(),
        //                f_name = c.String(),
        //                l_name = c.String(),
        //                username = c.String(),
        //                dateform = c.String(),
        //                dateto = c.String(),
        //                email = c.String(),
        //                Address = c.String(),
        //            })
        //        .PrimaryKey(t => t.Id);
            
        //    CreateTable(
        //        "dbo.Locations",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                Name = c.String(),
        //                CountryId = c.Int(nullable: false),
        //                IsDeleted = c.Boolean(nullable: false),
        //            })
        //        .PrimaryKey(t => t.Id);
            
        //    CreateTable(
        //        "dbo.Logs",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                CompanyId = c.Int(),
        //                UserId = c.Int(),
        //                BuildingObjectId = c.Int(),
        //                LogTypeId = c.Int(nullable: false),
        //                EventTime = c.DateTime(nullable: false),
        //                Action = c.String(),
        //                Building = c.String(),
        //                Node = c.String(),
        //                EventKey = c.String(),
        //                TAReportLabelId = c.Int(nullable: false),
        //                Timestamp = c.Binary(),
        //                Visitor_Id = c.Int(),
        //            })
        //        .PrimaryKey(t => t.Id)
        //        .ForeignKey("dbo.Users1", t => t.UserId)
        //        .ForeignKey("dbo.Visitors1", t => t.Visitor_Id)
        //        .ForeignKey("dbo.Companies1", t => t.CompanyId)
        //        .ForeignKey("dbo.LogTypes", t => t.LogTypeId, cascadeDelete: true)
        //        .Index(t => t.CompanyId)
        //        .Index(t => t.UserId)
        //        .Index(t => t.LogTypeId)
        //        .Index(t => t.Visitor_Id);
            
        //    CreateTable(
        //        "dbo.Companies1",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                ModifiedBy = c.String(),
        //                ModifiedLast = c.DateTime(nullable: false),
        //                Comment = c.String(),
        //                Active = c.Boolean(nullable: false),
        //                ParentId = c.Int(),
        //                IsCanUseOwnCards = c.Boolean(nullable: false),
        //                ClassificatorValueId = c.Int(),
        //                BuidingNames = c.String(),
        //                Floors = c.String(),
        //                Name = c.String(),
        //                IsDeleted = c.Boolean(nullable: false),
        //                Timestamp = c.Binary(),
        //            })
        //        .PrimaryKey(t => t.Id)
        //        .ForeignKey("dbo.ClassificatorValues", t => t.ClassificatorValueId)
        //        .Index(t => t.ClassificatorValueId);
            
        //    CreateTable(
        //        "dbo.ClassificatorValues",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                ClassificatorId = c.Int(nullable: false),
        //                Value = c.String(),
        //                Comments = c.String(),
        //                SortOrder = c.Int(nullable: false),
        //                DisplayValue = c.String(),
        //                Timestamp = c.Binary(),
        //            })
        //        .PrimaryKey(t => t.Id)
        //        .ForeignKey("dbo.Classificators", t => t.ClassificatorId, cascadeDelete: true)
        //        .Index(t => t.ClassificatorId);
            
        //    CreateTable(
        //        "dbo.Classificators",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                Description = c.String(),
        //                Comments = c.String(),
        //                Timestamp = c.Binary(),
        //            })
        //        .PrimaryKey(t => t.Id);
            
        //    CreateTable(
        //        "dbo.Users1",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                LoginName = c.String(),
        //                Password = c.String(),
        //                FirstName = c.String(),
        //                LastName = c.String(),
        //                Email = c.String(),
        //                PersonalId = c.String(),
        //                Active = c.Boolean(nullable: false),
        //                Comment = c.String(),
        //                ModifiedBy = c.String(),
        //                ModifiedLast = c.DateTime(nullable: false),
        //                OccupationName = c.String(),
        //                PhoneNumber = c.String(),
        //                WorkHours = c.Short(),
        //                GroupId = c.Int(),
        //                Image = c.Binary(),
        //                Birthday = c.DateTime(),
        //                Birthplace = c.String(),
        //                FamilyState = c.String(),
        //                Citizenship = c.String(),
        //                Residence = c.String(),
        //                Nation = c.String(),
        //                TitleId = c.Int(),
        //                CompanyId = c.Int(),
        //                ContractNum = c.String(),
        //                ContractStartDate = c.DateTime(),
        //                ContractEndDate = c.DateTime(),
        //                PermitOfWork = c.DateTime(),
        //                PermissionCallGuests = c.Boolean(),
        //                MillitaryAssignment = c.Boolean(),
        //                PersonalCode = c.String(),
        //                ExternalPersonalCode = c.String(),
        //                LanguageId = c.Int(),
        //                RegistredStartDate = c.DateTime(nullable: false),
        //                RegistredEndDate = c.DateTime(nullable: false),
        //                TableNumber = c.Int(),
        //                WorkTime = c.Boolean(),
        //                IsDeleted = c.Boolean(nullable: false),
        //                CreatedBy = c.String(),
        //                PIN1 = c.String(),
        //                PIN2 = c.String(),
        //                EServiceAllowed = c.Boolean(),
        //                ClassificatorValueId = c.Int(),
        //                IsVisitor = c.Boolean(),
        //                CardAlarm = c.Boolean(),
        //                IsShortTermVisitor = c.Boolean(),
        //                buildingID = c.Int(),
        //                buildingName = c.String(),
        //                Timestamp = c.Binary(),
        //            })
        //        .PrimaryKey(t => t.Id)
        //        .ForeignKey("dbo.ClassificatorValues", t => t.ClassificatorValueId)
        //        .ForeignKey("dbo.Companies1", t => t.CompanyId)
        //        .ForeignKey("dbo.Titles1", t => t.TitleId)
        //        .Index(t => t.TitleId)
        //        .Index(t => t.CompanyId)
        //        .Index(t => t.ClassificatorValueId);
            
        //    CreateTable(
        //        "dbo.CompanyManagers",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                CompanyId = c.Int(nullable: false),
        //                UserId = c.Int(nullable: false),
        //                IsDeleted = c.Boolean(nullable: false),
        //                Timestamp = c.Binary(),
        //                Visitor_Id = c.Int(),
        //            })
        //        .PrimaryKey(t => t.Id)
        //        .ForeignKey("dbo.Companies1", t => t.CompanyId, cascadeDelete: true)
        //        .ForeignKey("dbo.Users1", t => t.UserId, cascadeDelete: true)
        //        .ForeignKey("dbo.Visitors1", t => t.Visitor_Id)
        //        .Index(t => t.CompanyId)
        //        .Index(t => t.UserId)
        //        .Index(t => t.Visitor_Id);
            
        //    CreateTable(
        //        "dbo.LogFilters",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                CompanyId = c.Int(),
        //                UserId = c.Int(nullable: false),
        //                FromDate = c.DateTime(),
        //                ToDate = c.DateTime(),
        //                Name = c.String(),
        //                UserName = c.String(),
        //                Building = c.String(),
        //                Node = c.String(),
        //                IsDeleted = c.Boolean(nullable: false),
        //                IsShowDefaultLog = c.Boolean(),
        //                Activity = c.String(),
        //                Timestamp = c.Binary(),
        //                Visitor_Id = c.Int(),
        //            })
        //        .PrimaryKey(t => t.Id)
        //        .ForeignKey("dbo.Companies1", t => t.CompanyId)
        //        .ForeignKey("dbo.Users1", t => t.UserId, cascadeDelete: true)
        //        .ForeignKey("dbo.Visitors1", t => t.Visitor_Id)
        //        .Index(t => t.CompanyId)
        //        .Index(t => t.UserId)
        //        .Index(t => t.Visitor_Id);
            
        //    CreateTable(
        //        "dbo.Roles1",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                StaticId = c.Int(nullable: false),
        //                Active = c.Boolean(nullable: false),
        //                Permissions = c.Binary(),
        //                Menues = c.Binary(),
        //                Priority = c.Int(nullable: false),
        //                RoleTypeId = c.Int(),
        //                UserId = c.Int(),
        //                Description = c.String(),
        //                ModifiedLast = c.DateTime(nullable: false),
        //                ModifiedBy = c.String(),
        //                Name = c.String(),
        //                IsDeleted = c.Boolean(nullable: false),
        //                Timestamp = c.Binary(),
        //            })
        //        .PrimaryKey(t => t.Id)
        //        .ForeignKey("dbo.RoleTypes", t => t.RoleTypeId)
        //        .ForeignKey("dbo.Users1", t => t.UserId)
        //        .Index(t => t.RoleTypeId)
        //        .Index(t => t.UserId);
            
        //    CreateTable(
        //        "dbo.RoleBuildings",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                RoleId = c.Int(nullable: false),
        //                BuildingId = c.Int(nullable: false),
        //                IsDeleted = c.Boolean(nullable: false),
        //                Timestamp = c.Binary(),
        //            })
        //        .PrimaryKey(t => t.Id)
        //        .ForeignKey("dbo.Buildings1", t => t.BuildingId, cascadeDelete: true)
        //        .ForeignKey("dbo.Roles1", t => t.RoleId, cascadeDelete: true)
        //        .Index(t => t.RoleId)
        //        .Index(t => t.BuildingId);
            
        //    CreateTable(
        //        "dbo.Buildings1",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                AdressStreet = c.String(),
        //                AdressHouse = c.String(),
        //                AdressIndex = c.String(),
        //                LocationId = c.Int(nullable: false),
        //                Floors = c.Int(nullable: false),
        //                TimediffGMTMinutes = c.Int(),
        //                TimezoneId = c.String(),
        //                Name = c.String(),
        //                IsDeleted = c.Boolean(nullable: false),
        //                Timestamp = c.Binary(),
        //            })
        //        .PrimaryKey(t => t.Id)
        //        .ForeignKey("dbo.Locations1", t => t.LocationId, cascadeDelete: true)
        //        .Index(t => t.LocationId);
            
        //    CreateTable(
        //        "dbo.BuildingObjects1",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                TypeId = c.Int(nullable: false),
        //                BuildingId = c.Int(nullable: false),
        //                ParentObjectId = c.Int(),
        //                FloorNr = c.Int(),
        //                Description = c.String(),
        //                StatusIconId = c.Int(),
        //                ObjectNr = c.Int(),
        //                Comment = c.String(),
        //                FSControllerNodeId = c.Int(),
        //                FSControllerObjectNr = c.Int(),
        //                FSTypeId = c.Int(),
        //                BOOrder = c.Int(),
        //                GlobalBuilding = c.Int(),
        //                ParentArea = c.Int(),
        //                Name = c.String(),
        //                IsDeleted = c.Boolean(nullable: false),
        //                Timestamp = c.Binary(),
        //                BuildingObjectType_Id = c.Int(),
        //            })
        //        .PrimaryKey(t => t.Id)
        //        .ForeignKey("dbo.Buildings1", t => t.BuildingId, cascadeDelete: true)
        //        .ForeignKey("dbo.BuildingObjectTypes", t => t.BuildingObjectType_Id)
        //        .Index(t => t.BuildingId)
        //        .Index(t => t.BuildingObjectType_Id);
            
        //    CreateTable(
        //        "dbo.BuildingObjectTypes",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                Description = c.String(),
        //                ModifiedBy = c.String(),
        //                ModifiedLast = c.DateTime(nullable: false),
        //                Name = c.String(),
        //                IsDeleted = c.Boolean(nullable: false),
        //                Timestamp = c.Binary(),
        //            })
        //        .PrimaryKey(t => t.Id);
            
        //    CreateTable(
        //        "dbo.CompanyBuildingObjects",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                CompanyId = c.Int(nullable: false),
        //                BuildingObjectId = c.Int(nullable: false),
        //                ValidFrom = c.DateTime(nullable: false),
        //                ValidTo = c.DateTime(nullable: false),
        //                IsDeleted = c.Boolean(nullable: false),
        //                Timestamp = c.Binary(),
        //            })
        //        .PrimaryKey(t => t.Id)
        //        .ForeignKey("dbo.BuildingObjects1", t => t.BuildingObjectId, cascadeDelete: true)
        //        .ForeignKey("dbo.Companies1", t => t.CompanyId, cascadeDelete: true)
        //        .Index(t => t.CompanyId)
        //        .Index(t => t.BuildingObjectId);
            
        //    CreateTable(
        //        "dbo.UserBuildings",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                UserId = c.Int(nullable: false),
        //                BuildingId = c.Int(nullable: false),
        //                BuildingObjectId = c.Int(),
        //                IsDeleted = c.Boolean(nullable: false),
        //                Timestamp = c.Binary(),
        //                Visitor_Id = c.Int(),
        //            })
        //        .PrimaryKey(t => t.Id)
        //        .ForeignKey("dbo.Buildings1", t => t.BuildingId, cascadeDelete: true)
        //        .ForeignKey("dbo.BuildingObjects1", t => t.BuildingObjectId)
        //        .ForeignKey("dbo.Users1", t => t.UserId, cascadeDelete: true)
        //        .ForeignKey("dbo.Visitors1", t => t.Visitor_Id)
        //        .Index(t => t.UserId)
        //        .Index(t => t.BuildingId)
        //        .Index(t => t.BuildingObjectId)
        //        .Index(t => t.Visitor_Id);
            
        //    CreateTable(
        //        "dbo.UserPermissionGroupTimeZones",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                UserPermissionGroupId = c.Int(nullable: false),
        //                UserTimeZoneId = c.Int(nullable: false),
        //                BuildingObjectId = c.Int(nullable: false),
        //                IsArming = c.Boolean(nullable: false),
        //                IsDefaultArming = c.Boolean(nullable: false),
        //                IsDisarming = c.Boolean(nullable: false),
        //                IsDefaultDisarming = c.Boolean(nullable: false),
        //                Active = c.Boolean(nullable: false),
        //                IsDeleted = c.Boolean(nullable: false),
        //                Timestamp = c.Binary(),
        //            })
        //        .PrimaryKey(t => t.Id)
        //        .ForeignKey("dbo.BuildingObjects1", t => t.BuildingObjectId, cascadeDelete: true)
        //        .ForeignKey("dbo.UserPermissionGroups", t => t.UserPermissionGroupId, cascadeDelete: true)
        //        .ForeignKey("dbo.UserTimeZones", t => t.UserTimeZoneId, cascadeDelete: true)
        //        .Index(t => t.UserPermissionGroupId)
        //        .Index(t => t.UserTimeZoneId)
        //        .Index(t => t.BuildingObjectId);
            
        //    CreateTable(
        //        "dbo.UserPermissionGroups",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                UserId = c.Int(nullable: false),
        //                DefaultUserTimeZoneId = c.Int(nullable: false),
        //                ParentUserPermissionGroupId = c.Int(),
        //                Name = c.String(),
        //                IsOriginal = c.Boolean(nullable: false),
        //                PermissionIsActive = c.Boolean(nullable: false),
        //                IsDeleted = c.Boolean(nullable: false),
        //                Timestamp = c.Binary(),
        //                UserTimeZone_Id = c.Int(),
        //                Visitor_Id = c.Int(),
        //            })
        //        .PrimaryKey(t => t.Id)
        //        .ForeignKey("dbo.Users1", t => t.UserId, cascadeDelete: true)
        //        .ForeignKey("dbo.UserTimeZones", t => t.UserTimeZone_Id)
        //        .ForeignKey("dbo.Visitors1", t => t.Visitor_Id)
        //        .Index(t => t.UserId)
        //        .Index(t => t.UserTimeZone_Id)
        //        .Index(t => t.Visitor_Id);
            
        //    CreateTable(
        //        "dbo.UserTimeZones",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                UserId = c.Int(nullable: false),
        //                TimeZoneId = c.Int(),
        //                ParentUserTimeZoneId = c.Int(),
        //                Name = c.String(),
        //                Uid = c.Guid(nullable: false),
        //                IsOriginal = c.Boolean(nullable: false),
        //                IsDeleted = c.Boolean(nullable: false),
        //                IsCompanySpecific = c.Boolean(nullable: false),
        //                CompanyId = c.Int(),
        //                Timestamp = c.Binary(),
        //                Visitor_Id = c.Int(),
        //            })
        //        .PrimaryKey(t => t.Id)
        //        .ForeignKey("dbo.Users1", t => t.UserId, cascadeDelete: true)
        //        .ForeignKey("dbo.Visitors1", t => t.Visitor_Id)
        //        .Index(t => t.UserId)
        //        .Index(t => t.Visitor_Id);
            
        //    CreateTable(
        //        "dbo.UserTimeZoneProperties",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                TimeZoneId = c.Int(),
        //                UserTimeZoneId = c.Int(nullable: false),
        //                OrderInGroup = c.Int(nullable: false),
        //                ValidFrom = c.DateTime(),
        //                ValidTo = c.DateTime(),
        //                IsMonday = c.Boolean(nullable: false),
        //                IsTuesday = c.Boolean(nullable: false),
        //                IsWednesday = c.Boolean(nullable: false),
        //                IsThursday = c.Boolean(nullable: false),
        //                IsFriday = c.Boolean(nullable: false),
        //                IsSaturday = c.Boolean(nullable: false),
        //                IsSunday = c.Boolean(nullable: false),
        //                Timestamp = c.Binary(),
        //            })
        //        .PrimaryKey(t => t.Id)
        //        .ForeignKey("dbo.UserTimeZones", t => t.UserTimeZoneId, cascadeDelete: true)
        //        .Index(t => t.UserTimeZoneId);
            
        //    CreateTable(
        //        "dbo.Locations1",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                CountryId = c.Int(nullable: false),
        //                Name = c.String(),
        //                IsDeleted = c.Boolean(nullable: false),
        //                Timestamp = c.Binary(),
        //            })
        //        .PrimaryKey(t => t.Id)
        //        .ForeignKey("dbo.Countries1", t => t.CountryId, cascadeDelete: true)
        //        .Index(t => t.CountryId);
            
        //    CreateTable(
        //        "dbo.Countries1",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                ISONumber = c.Short(),
        //                Name = c.String(),
        //                IsDeleted = c.Boolean(nullable: false),
        //                Timestamp = c.Binary(),
        //            })
        //        .PrimaryKey(t => t.Id);
            
        //    CreateTable(
        //        "dbo.TAReports",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                UserId = c.Int(nullable: false),
        //                DepartmentId = c.Int(),
        //                ReportDate = c.DateTime(nullable: false),
        //                Day = c.Short(nullable: false),
        //                Hours = c.Single(nullable: false),
        //                Hours_Min = c.String(),
        //                Shift = c.Int(nullable: false),
        //                Status = c.Byte(),
        //                Completed = c.Boolean(nullable: false),
        //                ModifiedLast = c.DateTime(nullable: false),
        //                ModifiedId = c.Int(nullable: false),
        //                BuildingId = c.Int(),
        //                UserName = c.String(),
        //                Name = c.String(),
        //                IsDeleted = c.Boolean(nullable: false),
        //                Timestamp = c.Binary(),
        //            })
        //        .PrimaryKey(t => t.Id)
        //        .ForeignKey("dbo.Buildings1", t => t.BuildingId)
        //        .ForeignKey("dbo.Departments1", t => t.DepartmentId)
        //        .ForeignKey("dbo.Users1", t => t.UserId, cascadeDelete: true)
        //        .Index(t => t.UserId)
        //        .Index(t => t.DepartmentId)
        //        .Index(t => t.BuildingId);
            
        //    CreateTable(
        //        "dbo.Departments1",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                Manager = c.String(),
        //                Number = c.String(),
        //                ModifiedBy = c.String(),
        //                ModifiedLast = c.DateTime(nullable: false),
        //                CompanyId = c.Int(nullable: false),
        //                Name = c.String(),
        //                IsDeleted = c.Boolean(nullable: false),
        //                Timestamp = c.Binary(),
        //            })
        //        .PrimaryKey(t => t.Id)
        //        .ForeignKey("dbo.Companies1", t => t.CompanyId, cascadeDelete: true)
        //        .Index(t => t.CompanyId);
            
        //    CreateTable(
        //        "dbo.TAMoves1",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                UserId = c.Int(nullable: false),
        //                DepartmentId = c.Int(),
        //                Label = c.String(),
        //                Remark = c.String(),
        //                Started = c.DateTime(nullable: false),
        //                Finished = c.DateTime(),
        //                Hours = c.Single(nullable: false),
        //                Hours_Min = c.String(),
        //                Schedule = c.Int(nullable: false),
        //                Status = c.Byte(),
        //                JobNotMove = c.Boolean(nullable: false),
        //                Completed = c.Boolean(nullable: false),
        //                ModifiedLast = c.DateTime(nullable: false),
        //                ModifiedBy = c.String(),
        //                StartedBoId = c.Int(),
        //                FinishedBoId = c.Int(),
        //                UserName = c.String(),
        //                Name = c.String(),
        //                IsDeleted = c.Boolean(nullable: false),
        //                Timestamp = c.Binary(),
        //            })
        //        .PrimaryKey(t => t.Id)
        //        .ForeignKey("dbo.Departments1", t => t.DepartmentId)
        //        .ForeignKey("dbo.Users1", t => t.UserId, cascadeDelete: true)
        //        .Index(t => t.UserId)
        //        .Index(t => t.DepartmentId);
            
        //    CreateTable(
        //        "dbo.UserDepartments1",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                UserId = c.Int(nullable: false),
        //                DepartmentId = c.Int(nullable: false),
        //                ValidFrom = c.DateTime(nullable: false),
        //                ValidTo = c.DateTime(nullable: false),
        //                CurrentDep = c.Boolean(nullable: false),
        //                IsDeleted = c.Boolean(nullable: false),
        //                IsDepartmentManager = c.Boolean(nullable: false),
        //                Timestamp = c.Binary(),
        //                Visitor_Id = c.Int(),
        //            })
        //        .PrimaryKey(t => t.Id)
        //        .ForeignKey("dbo.Departments1", t => t.DepartmentId, cascadeDelete: true)
        //        .ForeignKey("dbo.Users1", t => t.UserId, cascadeDelete: true)
        //        .ForeignKey("dbo.Visitors1", t => t.Visitor_Id)
        //        .Index(t => t.UserId)
        //        .Index(t => t.DepartmentId)
        //        .Index(t => t.Visitor_Id);
            
        //    CreateTable(
        //        "dbo.UsersAccessUnits",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                UserId = c.Int(),
        //                TypeId = c.Int(),
        //                CompanyId = c.Int(),
        //                Serial = c.String(),
        //                Code = c.String(),
        //                Active = c.Boolean(nullable: false),
        //                Free = c.Boolean(nullable: false),
        //                Opened = c.DateTime(),
        //                Closed = c.DateTime(),
        //                ValidFrom = c.DateTime(),
        //                ValidTo = c.DateTime(),
        //                Dk = c.String(maxLength: 5),
        //                CreatedBy = c.Int(nullable: false),
        //                IsDeleted = c.Boolean(nullable: false),
        //                ClassificatorValueId = c.Int(),
        //                BuildingId = c.Int(nullable: false),
        //                Classificator_dt = c.DateTime(),
        //                Comment = c.String(),
        //                CardFullBuildings = c.String(),
        //                Timestamp = c.Binary(),
        //                UserAccessUnitType_Id = c.Int(),
        //                Visitor_Id = c.Int(),
        //            })
        //        .PrimaryKey(t => t.Id)
        //        .ForeignKey("dbo.Buildings1", t => t.BuildingId, cascadeDelete: true)
        //        .ForeignKey("dbo.ClassificatorValues", t => t.ClassificatorValueId)
        //        .ForeignKey("dbo.Companies1", t => t.CompanyId)
        //        .ForeignKey("dbo.Users1", t => t.UserId)
        //        .ForeignKey("dbo.UserAccessUnitTypes", t => t.UserAccessUnitType_Id)
        //        .ForeignKey("dbo.Visitors1", t => t.Visitor_Id)
        //        .Index(t => t.UserId)
        //        .Index(t => t.CompanyId)
        //        .Index(t => t.ClassificatorValueId)
        //        .Index(t => t.BuildingId)
        //        .Index(t => t.UserAccessUnitType_Id)
        //        .Index(t => t.Visitor_Id);
            
        //    CreateTable(
        //        "dbo.UserAccessUnitTypes",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                Description = c.String(),
        //                IsCardCode = c.Boolean(nullable: false),
        //                IsSerDK = c.Boolean(nullable: false),
        //                Name = c.String(),
        //                IsDeleted = c.Boolean(nullable: false),
        //                Timestamp = c.Binary(),
        //            })
        //        .PrimaryKey(t => t.Id);
            
        //    CreateTable(
        //        "dbo.RoleTypes",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                Menues = c.Binary(),
        //                Name = c.String(),
        //                IsDeleted = c.Boolean(nullable: false),
        //                Timestamp = c.Binary(),
        //            })
        //        .PrimaryKey(t => t.Id);
            
        //    CreateTable(
        //        "dbo.UserRoles",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                UserId = c.Int(nullable: false),
        //                RoleId = c.Int(nullable: false),
        //                CompanyId = c.Int(),
        //                BuildingId = c.Int(),
        //                ValidFrom = c.DateTime(nullable: false),
        //                ValidTo = c.DateTime(nullable: false),
        //                IsDeleted = c.Boolean(nullable: false),
        //                Timestamp = c.Binary(),
        //                Visitor_Id = c.Int(),
        //                Visitor_Id1 = c.Int(),
        //            })
        //        .PrimaryKey(t => t.Id)
        //        .ForeignKey("dbo.Companies1", t => t.CompanyId)
        //        .ForeignKey("dbo.Roles1", t => t.RoleId, cascadeDelete: true)
        //        .ForeignKey("dbo.Users1", t => t.UserId, cascadeDelete: true)
        //        .ForeignKey("dbo.Visitors1", t => t.Visitor_Id)
        //        .ForeignKey("dbo.Visitors1", t => t.Visitor_Id1)
        //        .Index(t => t.UserId)
        //        .Index(t => t.RoleId)
        //        .Index(t => t.CompanyId)
        //        .Index(t => t.Visitor_Id)
        //        .Index(t => t.Visitor_Id1);
            
        //    CreateTable(
        //        "dbo.Titles1",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                Description = c.String(),
        //                CompanyId = c.Int(nullable: false),
        //                Name = c.String(),
        //                IsDeleted = c.Boolean(nullable: false),
        //                Timestamp = c.Binary(),
        //            })
        //        .PrimaryKey(t => t.Id)
        //        .ForeignKey("dbo.Companies1", t => t.CompanyId, cascadeDelete: true)
        //        .Index(t => t.CompanyId);
            
        //    CreateTable(
        //        "dbo.Visitors1",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                CarNr = c.String(),
        //                UserId = c.Int(),
        //                FirstName = c.String(),
        //                CarType = c.String(),
        //                StartDateTime = c.DateTime(),
        //                StopDateTime = c.DateTime(),
        //                IsDeleted = c.Boolean(nullable: false),
        //                IsUpdated = c.Boolean(nullable: false),
        //                UpdateDatetime = c.DateTime(),
        //                LastChange = c.DateTime(),
        //                CompanyId = c.Int(),
        //                Timestamp = c.Binary(),
        //                Accept = c.Boolean(nullable: false),
        //                AcceptUserId = c.Int(),
        //                AcceptDateTime = c.DateTime(),
        //                LastName = c.String(),
        //                Active = c.Boolean(nullable: false),
        //                Company = c.String(),
        //                ParentVisitorsId = c.Int(),
        //                Comment = c.String(),
        //                ReturnDate = c.DateTime(),
        //                Email = c.String(),
        //                PhoneNumber = c.String(),
        //                IsCarNrAccessUnit = c.Boolean(nullable: false),
        //                IsPhoneNrAccessUnit = c.Boolean(nullable: false),
        //                ResponsibleUserId = c.Int(),
        //                CardNeedReturn = c.Boolean(nullable: false),
        //                ClassificatorValue_Id = c.Int(),
        //            })
        //        .PrimaryKey(t => t.Id)
        //        .ForeignKey("dbo.ClassificatorValues", t => t.ClassificatorValue_Id)
        //        .ForeignKey("dbo.Users1", t => t.UserId)
        //        .Index(t => t.UserId)
        //        .Index(t => t.ClassificatorValue_Id);
            
        //    CreateTable(
        //        "dbo.LogTypes",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                NumberOfError = c.Int(nullable: false),
        //                Name = c.String(),
        //                Color = c.String(),
        //                IsDefault = c.Boolean(nullable: false),
        //                Timestamp = c.Binary(),
        //            })
        //        .PrimaryKey(t => t.Id);
            
        //    CreateTable(
        //        "dbo.TAMoves",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                UserId = c.Int(nullable: false),
        //                DepartmentId = c.Int(nullable: false),
        //                Label = c.String(),
        //                Remark = c.String(),
        //                Started = c.DateTime(nullable: false),
        //                Finished = c.DateTime(nullable: false),
        //                Hours = c.Single(nullable: false),
        //                Hours_Min = c.String(),
        //                Schedule = c.Int(nullable: false),
        //                Status = c.Byte(),
        //                JobNotMove = c.Boolean(nullable: false),
        //                Completed = c.Boolean(nullable: false),
        //                ModifiedLast = c.DateTime(nullable: false),
        //                ModifiedBy = c.String(),
        //                StartedBoId = c.Int(),
        //                FinishedBoId = c.Int(),
        //                UserName = c.String(),
        //                IsDeleted = c.Boolean(nullable: false),
        //                BuildingID = c.Int(nullable: false),
        //            })
        //        .PrimaryKey(t => t.Id)
        //        .ForeignKey("dbo.Departments1", t => t.DepartmentId, cascadeDelete: true)
        //        .ForeignKey("dbo.Users1", t => t.UserId, cascadeDelete: true)
        //        .Index(t => t.UserId)
        //        .Index(t => t.DepartmentId);
            
        //    CreateTable(
        //        "dbo.TABuildingNames",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                BuildingId = c.Int(nullable: false),
        //                Name = c.String(),
        //                ValidFrom = c.DateTime(nullable: false),
        //                ValidTo = c.DateTime(nullable: false),
        //                Address = c.String(),
        //                BuildingLicense = c.String(),
        //                CadastralNr = c.String(),
        //                IsDeleted = c.Boolean(nullable: false),
        //                Customer = c.String(),
        //                Contractor = c.String(),
        //                Contract = c.String(),
        //                Sum = c.String(),
        //            })
        //        .PrimaryKey(t => t.Id);
            
        //    CreateTable(
        //        "dbo.Titles",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                Name = c.String(),
        //            })
        //        .PrimaryKey(t => t.Id);
            
        //    CreateTable(
        //        "dbo.Users",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                LoginName = c.String(),
        //                FirstName = c.String(),
        //                LastName = c.String(),
        //                Email = c.String(),
        //                Password = c.String(),
        //                PersonalId = c.String(),
        //                Active = c.Boolean(nullable: false),
        //                Comment = c.String(),
        //                ModifiedBy = c.String(),
        //                ModifiedLast = c.DateTime(nullable: false),
        //                OccupationName = c.String(),
        //                PhoneNumber = c.String(),
        //                WorkHours = c.Short(),
        //                GroupId = c.Int(),
        //                Image = c.Binary(),
        //                Birthday = c.DateTime(),
        //                Birthplace = c.String(),
        //                FamilyState = c.String(),
        //                Citizenship = c.String(),
        //                Residence = c.String(),
        //                Nation = c.String(),
        //                ContractNum = c.String(),
        //                ContractStartDate = c.DateTime(),
        //                ContractEndDate = c.DateTime(),
        //                PermitOfWork = c.DateTime(),
        //                PermissionCallGuests = c.Boolean(),
        //                MillitaryAssignment = c.Boolean(),
        //                PersonalCode = c.String(),
        //                ExternalPersonalCode = c.String(),
        //                LanguageId = c.Int(),
        //                RegistredStartDate = c.DateTime(nullable: false),
        //                RegistredEndDate = c.DateTime(nullable: false),
        //                TableNumber = c.Int(),
        //                WorkTime = c.Boolean(),
        //                EServiceAllowed = c.Boolean(),
        //                IsVisitor = c.Boolean(),
        //                CardAlarm = c.Boolean(),
        //                CreatedBy = c.String(),
        //                IsDeleted = c.Boolean(nullable: false),
        //                Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
        //                CompanyId = c.Int(),
        //                TitleId = c.Int(),
        //                IsShortTermVisitor = c.Boolean(nullable: false),
        //            })
        //        .PrimaryKey(t => t.Id);
            
        //    CreateTable(
        //        "dbo.UserDepartments",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                UserId = c.Int(nullable: false),
        //                DepartmentId = c.Int(nullable: false),
        //                IsDepartmentManager = c.Boolean(nullable: false),
        //            })
        //        .PrimaryKey(t => t.Id);
            
        //    CreateTable(
        //        "dbo.UserLogs",
        //        c => new
        //            {
        //                id = c.Int(nullable: false, identity: true),
        //                UserId = c.Int(nullable: false),
        //                UserFullName = c.String(),
        //                UserRole = c.String(),
        //                SearchBOx = c.String(),
        //                Datefrom = c.DateTime(nullable: false),
        //                dateto = c.DateTime(nullable: false),
        //                buildingfilter = c.String(),
        //                ShowDefaultOrfullLog = c.String(),
        //                Node = c.String(),
        //                CompanyFiter = c.String(),
        //                Name = c.String(),
        //                Filter_text = c.String(),
        //                user_text = c.String(),
        //                Activity = c.String(),
        //                button_clicked = c.String(),
        //                totalRecordView = c.Int(nullable: false),
        //                VisitDate = c.DateTime(nullable: false),
        //            })
        //        .PrimaryKey(t => t.id);
            
        //    CreateTable(
        //        "dbo.Roles",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                Name = c.String(),
        //            })
        //        .PrimaryKey(t => t.Id);
            
        //    CreateTable(
        //        "dbo.Visitors",
        //        c => new
        //            {
        //                Id = c.Int(nullable: false, identity: true),
        //                CarNr = c.String(),
        //                UserId = c.Int(),
        //                FirstName = c.String(),
        //                CarType = c.String(),
        //                StartDateTime = c.DateTime(),
        //                StopDateTime = c.DateTime(),
        //                IsDeleted = c.Boolean(nullable: false),
        //                IsUpdated = c.Boolean(nullable: false),
        //                UpdateDatetime = c.DateTime(),
        //                LastChange = c.DateTime(),
        //                CompanyId = c.Int(),
        //                Timestamp = c.Binary(),
        //                Accept = c.Boolean(nullable: false),
        //                AcceptUserId = c.Int(),
        //                AcceptDateTime = c.DateTime(),
        //                LastName = c.String(),
        //                Active = c.Boolean(nullable: false),
        //                Company = c.String(),
        //                ParentVisitorsId = c.Int(),
        //                Comment = c.String(),
        //                ReturnDate = c.DateTime(),
        //                Email = c.String(),
        //                PhoneNumber = c.String(),
        //                IsCarNrAccessUnit = c.Boolean(nullable: false),
        //                IsPhoneNrAccessUnit = c.Boolean(nullable: false),
        //                ResponsibleUserId = c.Int(),
        //                CardNeedReturn = c.Boolean(nullable: false),
        //            })
        //        .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.TAMoves", "UserId", "dbo.Users1");
            //DropForeignKey("dbo.TAMoves", "DepartmentId", "dbo.Departments1");
            //DropForeignKey("dbo.Logs", "LogTypeId", "dbo.LogTypes");
            //DropForeignKey("dbo.Logs", "CompanyId", "dbo.Companies1");
            //DropForeignKey("dbo.UserTimeZones", "Visitor_Id", "dbo.Visitors1");
            //DropForeignKey("dbo.UsersAccessUnits", "Visitor_Id", "dbo.Visitors1");
            //DropForeignKey("dbo.UserRoles", "Visitor_Id1", "dbo.Visitors1");
            //DropForeignKey("dbo.UserPermissionGroups", "Visitor_Id", "dbo.Visitors1");
            //DropForeignKey("dbo.UserDepartments1", "Visitor_Id", "dbo.Visitors1");
            //DropForeignKey("dbo.UserBuildings", "Visitor_Id", "dbo.Visitors1");
            //DropForeignKey("dbo.UserRoles", "Visitor_Id", "dbo.Visitors1");
            //DropForeignKey("dbo.Visitors1", "UserId", "dbo.Users1");
            //DropForeignKey("dbo.Logs", "Visitor_Id", "dbo.Visitors1");
            //DropForeignKey("dbo.LogFilters", "Visitor_Id", "dbo.Visitors1");
            //DropForeignKey("dbo.CompanyManagers", "Visitor_Id", "dbo.Visitors1");
            //DropForeignKey("dbo.Visitors1", "ClassificatorValue_Id", "dbo.ClassificatorValues");
            //DropForeignKey("dbo.Users1", "TitleId", "dbo.Titles1");
            //DropForeignKey("dbo.Titles1", "CompanyId", "dbo.Companies1");
            //DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users1");
            //DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles1");
            //DropForeignKey("dbo.UserRoles", "CompanyId", "dbo.Companies1");
            //DropForeignKey("dbo.Roles1", "UserId", "dbo.Users1");
            //DropForeignKey("dbo.Roles1", "RoleTypeId", "dbo.RoleTypes");
            //DropForeignKey("dbo.RoleBuildings", "RoleId", "dbo.Roles1");
            //DropForeignKey("dbo.UsersAccessUnits", "UserAccessUnitType_Id", "dbo.UserAccessUnitTypes");
            //DropForeignKey("dbo.UsersAccessUnits", "UserId", "dbo.Users1");
            //DropForeignKey("dbo.UsersAccessUnits", "CompanyId", "dbo.Companies1");
            //DropForeignKey("dbo.UsersAccessUnits", "ClassificatorValueId", "dbo.ClassificatorValues");
            //DropForeignKey("dbo.UsersAccessUnits", "BuildingId", "dbo.Buildings1");
            //DropForeignKey("dbo.TAReports", "UserId", "dbo.Users1");
            //DropForeignKey("dbo.UserDepartments1", "UserId", "dbo.Users1");
            //DropForeignKey("dbo.UserDepartments1", "DepartmentId", "dbo.Departments1");
            //DropForeignKey("dbo.TAReports", "DepartmentId", "dbo.Departments1");
            //DropForeignKey("dbo.TAMoves1", "UserId", "dbo.Users1");
            //DropForeignKey("dbo.TAMoves1", "DepartmentId", "dbo.Departments1");
            //DropForeignKey("dbo.Departments1", "CompanyId", "dbo.Companies1");
            //DropForeignKey("dbo.TAReports", "BuildingId", "dbo.Buildings1");
            //DropForeignKey("dbo.RoleBuildings", "BuildingId", "dbo.Buildings1");
            //DropForeignKey("dbo.Locations1", "CountryId", "dbo.Countries1");
            //DropForeignKey("dbo.Buildings1", "LocationId", "dbo.Locations1");
            //DropForeignKey("dbo.UserTimeZoneProperties", "UserTimeZoneId", "dbo.UserTimeZones");
            //DropForeignKey("dbo.UserPermissionGroupTimeZones", "UserTimeZoneId", "dbo.UserTimeZones");
            //DropForeignKey("dbo.UserPermissionGroups", "UserTimeZone_Id", "dbo.UserTimeZones");
            //DropForeignKey("dbo.UserTimeZones", "UserId", "dbo.Users1");
            //DropForeignKey("dbo.UserPermissionGroupTimeZones", "UserPermissionGroupId", "dbo.UserPermissionGroups");
            //DropForeignKey("dbo.UserPermissionGroups", "UserId", "dbo.Users1");
            //DropForeignKey("dbo.UserPermissionGroupTimeZones", "BuildingObjectId", "dbo.BuildingObjects1");
            //DropForeignKey("dbo.UserBuildings", "UserId", "dbo.Users1");
            //DropForeignKey("dbo.UserBuildings", "BuildingObjectId", "dbo.BuildingObjects1");
            //DropForeignKey("dbo.UserBuildings", "BuildingId", "dbo.Buildings1");
            //DropForeignKey("dbo.CompanyBuildingObjects", "CompanyId", "dbo.Companies1");
            //DropForeignKey("dbo.CompanyBuildingObjects", "BuildingObjectId", "dbo.BuildingObjects1");
            //DropForeignKey("dbo.BuildingObjects1", "BuildingObjectType_Id", "dbo.BuildingObjectTypes");
            //DropForeignKey("dbo.BuildingObjects1", "BuildingId", "dbo.Buildings1");
            //DropForeignKey("dbo.Logs", "UserId", "dbo.Users1");
            //DropForeignKey("dbo.LogFilters", "UserId", "dbo.Users1");
            //DropForeignKey("dbo.LogFilters", "CompanyId", "dbo.Companies1");
            //DropForeignKey("dbo.CompanyManagers", "UserId", "dbo.Users1");
            //DropForeignKey("dbo.CompanyManagers", "CompanyId", "dbo.Companies1");
            //DropForeignKey("dbo.Users1", "CompanyId", "dbo.Companies1");
            //DropForeignKey("dbo.Users1", "ClassificatorValueId", "dbo.ClassificatorValues");
            //DropForeignKey("dbo.Companies1", "ClassificatorValueId", "dbo.ClassificatorValues");
            //DropForeignKey("dbo.ClassificatorValues", "ClassificatorId", "dbo.Classificators");
            //DropForeignKey("dbo.FSHRs", "FSHR_Id", "dbo.FSHRs");
            //DropIndex("dbo.TAMoves", new[] { "DepartmentId" });
            //DropIndex("dbo.TAMoves", new[] { "UserId" });
            //DropIndex("dbo.Visitors1", new[] { "ClassificatorValue_Id" });
            //DropIndex("dbo.Visitors1", new[] { "UserId" });
            //DropIndex("dbo.Titles1", new[] { "CompanyId" });
            //DropIndex("dbo.UserRoles", new[] { "Visitor_Id1" });
            //DropIndex("dbo.UserRoles", new[] { "Visitor_Id" });
            //DropIndex("dbo.UserRoles", new[] { "CompanyId" });
            //DropIndex("dbo.UserRoles", new[] { "RoleId" });
            //DropIndex("dbo.UserRoles", new[] { "UserId" });
            //DropIndex("dbo.UsersAccessUnits", new[] { "Visitor_Id" });
            //DropIndex("dbo.UsersAccessUnits", new[] { "UserAccessUnitType_Id" });
            //DropIndex("dbo.UsersAccessUnits", new[] { "BuildingId" });
            //DropIndex("dbo.UsersAccessUnits", new[] { "ClassificatorValueId" });
            //DropIndex("dbo.UsersAccessUnits", new[] { "CompanyId" });
            //DropIndex("dbo.UsersAccessUnits", new[] { "UserId" });
            //DropIndex("dbo.UserDepartments1", new[] { "Visitor_Id" });
            //DropIndex("dbo.UserDepartments1", new[] { "DepartmentId" });
            //DropIndex("dbo.UserDepartments1", new[] { "UserId" });
            //DropIndex("dbo.TAMoves1", new[] { "DepartmentId" });
            //DropIndex("dbo.TAMoves1", new[] { "UserId" });
            //DropIndex("dbo.Departments1", new[] { "CompanyId" });
            //DropIndex("dbo.TAReports", new[] { "BuildingId" });
            //DropIndex("dbo.TAReports", new[] { "DepartmentId" });
            //DropIndex("dbo.TAReports", new[] { "UserId" });
            //DropIndex("dbo.Locations1", new[] { "CountryId" });
            //DropIndex("dbo.UserTimeZoneProperties", new[] { "UserTimeZoneId" });
            //DropIndex("dbo.UserTimeZones", new[] { "Visitor_Id" });
            //DropIndex("dbo.UserTimeZones", new[] { "UserId" });
            //DropIndex("dbo.UserPermissionGroups", new[] { "Visitor_Id" });
            //DropIndex("dbo.UserPermissionGroups", new[] { "UserTimeZone_Id" });
            //DropIndex("dbo.UserPermissionGroups", new[] { "UserId" });
            //DropIndex("dbo.UserPermissionGroupTimeZones", new[] { "BuildingObjectId" });
            //DropIndex("dbo.UserPermissionGroupTimeZones", new[] { "UserTimeZoneId" });
            //DropIndex("dbo.UserPermissionGroupTimeZones", new[] { "UserPermissionGroupId" });
            //DropIndex("dbo.UserBuildings", new[] { "Visitor_Id" });
            //DropIndex("dbo.UserBuildings", new[] { "BuildingObjectId" });
            //DropIndex("dbo.UserBuildings", new[] { "BuildingId" });
            //DropIndex("dbo.UserBuildings", new[] { "UserId" });
            //DropIndex("dbo.CompanyBuildingObjects", new[] { "BuildingObjectId" });
            //DropIndex("dbo.CompanyBuildingObjects", new[] { "CompanyId" });
            //DropIndex("dbo.BuildingObjects1", new[] { "BuildingObjectType_Id" });
            //DropIndex("dbo.BuildingObjects1", new[] { "BuildingId" });
            //DropIndex("dbo.Buildings1", new[] { "LocationId" });
            //DropIndex("dbo.RoleBuildings", new[] { "BuildingId" });
            //DropIndex("dbo.RoleBuildings", new[] { "RoleId" });
            //DropIndex("dbo.Roles1", new[] { "UserId" });
            //DropIndex("dbo.Roles1", new[] { "RoleTypeId" });
            //DropIndex("dbo.LogFilters", new[] { "Visitor_Id" });
            //DropIndex("dbo.LogFilters", new[] { "UserId" });
            //DropIndex("dbo.LogFilters", new[] { "CompanyId" });
            //DropIndex("dbo.CompanyManagers", new[] { "Visitor_Id" });
            //DropIndex("dbo.CompanyManagers", new[] { "UserId" });
            //DropIndex("dbo.CompanyManagers", new[] { "CompanyId" });
            //DropIndex("dbo.Users1", new[] { "ClassificatorValueId" });
            //DropIndex("dbo.Users1", new[] { "CompanyId" });
            //DropIndex("dbo.Users1", new[] { "TitleId" });
            //DropIndex("dbo.ClassificatorValues", new[] { "ClassificatorId" });
            //DropIndex("dbo.Companies1", new[] { "ClassificatorValueId" });
            //DropIndex("dbo.Logs", new[] { "Visitor_Id" });
            //DropIndex("dbo.Logs", new[] { "LogTypeId" });
            //DropIndex("dbo.Logs", new[] { "UserId" });
            //DropIndex("dbo.Logs", new[] { "CompanyId" });
            //DropIndex("dbo.FSHRs", new[] { "FSHR_Id" });
            //DropTable("dbo.Visitors");
            //DropTable("dbo.Roles");
            //DropTable("dbo.UserLogs");
            //DropTable("dbo.UserDepartments");
            //DropTable("dbo.Users");
            //DropTable("dbo.Titles");
            //DropTable("dbo.TABuildingNames");
            //DropTable("dbo.TAMoves");
            //DropTable("dbo.LogTypes");
            //DropTable("dbo.Visitors1");
            //DropTable("dbo.Titles1");
            //DropTable("dbo.UserRoles");
            //DropTable("dbo.RoleTypes");
            //DropTable("dbo.UserAccessUnitTypes");
            //DropTable("dbo.UsersAccessUnits");
            //DropTable("dbo.UserDepartments1");
            //DropTable("dbo.TAMoves1");
            //DropTable("dbo.Departments1");
            //DropTable("dbo.TAReports");
            //DropTable("dbo.Countries1");
            //DropTable("dbo.Locations1");
            //DropTable("dbo.UserTimeZoneProperties");
            //DropTable("dbo.UserTimeZones");
            //DropTable("dbo.UserPermissionGroups");
            //DropTable("dbo.UserPermissionGroupTimeZones");
            //DropTable("dbo.UserBuildings");
            //DropTable("dbo.CompanyBuildingObjects");
            //DropTable("dbo.BuildingObjectTypes");
            //DropTable("dbo.BuildingObjects1");
            //DropTable("dbo.Buildings1");
            //DropTable("dbo.RoleBuildings");
            //DropTable("dbo.Roles1");
            //DropTable("dbo.LogFilters");
            //DropTable("dbo.CompanyManagers");
            //DropTable("dbo.Users1");
            //DropTable("dbo.Classificators");
            //DropTable("dbo.ClassificatorValues");
            //DropTable("dbo.Companies1");
            //DropTable("dbo.Logs");
            //DropTable("dbo.Locations");
            //DropTable("dbo.Hr_Clone");
            //DropTable("dbo.FSVideoServers");
            //DropTable("dbo.FSINISettings");
            //DropTable("dbo.FSHRs");
            //DropTable("dbo.FSCameras");
            //DropTable("dbo.FSBuildingObjectCameras");
            //DropTable("dbo.Departments");
            //DropTable("dbo.Custormers");
            //DropTable("dbo.Countries");
            //DropTable("dbo.Companies");
            //DropTable("dbo.CamPermission");
            //DropTable("dbo.Buildings");
            //DropTable("dbo.BuildingObjects");
            //DropTable("dbo.Terminals");
        }
    }
}
