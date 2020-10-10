CREATE TABLE [dbo].[CompanyBuildingObjects] (
    [Id]               INT       IDENTITY (1, 1) NOT NULL,
    [CompanyId]        INT       NOT NULL,
    [BuildingObjectId] INT       NOT NULL,
    [Timestamp]        TIMESTAMP NOT NULL,
    [ValidFrom]        DATETIME  NOT NULL,
    [ValidTo]          DATETIME  NOT NULL,
    [IsDeleted]        BIT       NOT NULL
);

