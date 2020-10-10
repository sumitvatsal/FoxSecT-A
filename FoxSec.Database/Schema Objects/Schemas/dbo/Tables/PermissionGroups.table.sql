CREATE TABLE [dbo].[PermissionGroups] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (50) NOT NULL,
    [IsDeleted] BIT           NOT NULL,
    [Timestamp] TIMESTAMP     NOT NULL
);

