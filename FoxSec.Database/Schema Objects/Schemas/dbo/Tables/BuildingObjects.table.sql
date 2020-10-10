CREATE TABLE [dbo].[BuildingObjects] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [TypeId]         INT            NOT NULL,
    [BuildingId]     INT            NOT NULL,
    [ParentObjectId] INT            NULL,
    [Timestamp]      TIMESTAMP      NOT NULL,
    [Description]    NVARCHAR (250) NULL,
    [IsDeleted]      BIT            NOT NULL
);

