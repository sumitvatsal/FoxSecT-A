CREATE TABLE [dbo].[Roles] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (50)  NOT NULL,
    [Permissions]  BINARY (400)   NOT NULL,
    [Timestamp]    TIMESTAMP      NOT NULL,
    [ModifiedLast] DATETIME       NOT NULL,
    [ModifiedBy]   NVARCHAR (100) NOT NULL,
    [Description]  NCHAR (100)    NOT NULL,
    [Active]       BIT            NOT NULL,
    [Menues]       BINARY (400)   NOT NULL,
    [IsDeleted]    BIT            NOT NULL,
    [StaticId]     INT            NOT NULL,
    [Priority]     INT            NOT NULL
);

