CREATE TABLE [dbo].[UserBuildingObjects] (
    [Id]               INT       IDENTITY (1, 1) NOT NULL,
    [UserId]           INT       NOT NULL,
    [BuildingObjectId] INT       NOT NULL,
    [Timestamp]        TIMESTAMP NOT NULL,
    [IsDeleted]        BIT       NOT NULL
);

