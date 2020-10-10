CREATE TABLE [dbo].[RoleFormsAccess] (
    [Id]           INT IDENTITY (1, 1) NOT NULL,
    [RoleId]       INT NOT NULL,
    [FieldId]      INT NOT NULL,
    [ForRoleId]    INT NOT NULL,
    [AccessTypeId] INT NOT NULL
);

