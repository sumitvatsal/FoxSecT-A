CREATE TABLE [dbo].[ControllerUpdate](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ControllerId] [int] NOT NULL,
	[ParameterId] [int] NOT NULL,
	[Value] [varchar](255) NOT NULL,
	[MemoryStartAddress] [nchar](10) NULL,
	[Status] [int] NOT NULL,
	[DateAdded] [datetime] NOT NULL,
	[DateLastChanged] [datetime] NOT NULL,
	[AddedByUserId] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[Timestamp] [timestamp] NOT NULL
) ON [PRIMARY]


