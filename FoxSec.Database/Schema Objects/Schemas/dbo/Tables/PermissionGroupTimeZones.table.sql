CREATE TABLE [dbo].[PermissionGroupTimeZones](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PermissionGroupId] [int] NOT NULL,
	[TimeZoneId] [int] NOT NULL,
	[Timestamp] [timestamp] NOT NULL,
	[BuildingObjectId] [int] NOT NULL,
 CONSTRAINT [PK_PermissionGroups] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


