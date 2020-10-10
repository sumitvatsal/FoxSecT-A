CREATE TABLE [dbo].[BuildingObjectTypes] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (150) NOT NULL,
    [Description]  NVARCHAR (250) NULL,
    [Timestamp]    TIMESTAMP      NOT NULL,
    [ModifiedBy]   NVARCHAR (50)  NOT NULL,
    [ModifiedLast] DATETIME       NOT NULL,
    [IsDeleted]    BIT            NOT NULL
);

