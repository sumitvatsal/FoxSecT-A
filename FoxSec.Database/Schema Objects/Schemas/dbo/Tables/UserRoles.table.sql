CREATE TABLE [dbo].[UserRoles] (
    [Id]        INT       IDENTITY (1, 1) NOT NULL,
    [UserId]    INT       NOT NULL,
    [RoleId]    INT       NOT NULL,
    [Timestamp] TIMESTAMP NOT NULL,
    [ValidFrom] DATETIME  NOT NULL,
    [ValidTo]   DATETIME  NOT NULL,
    [CompanyId] INT       NULL,
    [IsDeleted] BIT       NOT NULL
);

