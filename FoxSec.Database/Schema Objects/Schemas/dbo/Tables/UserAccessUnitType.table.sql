CREATE TABLE [dbo].[UserAccessUnitType] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (125) NOT NULL,
    [Timestamp]   TIMESTAMP      NOT NULL,
    [Description] NVARCHAR (250) NULL,
    [IsDeleted]   BIT            NOT NULL,
    [IsCardCode]  BIT            NOT NULL,
    [IsSerDK]     BIT            NOT NULL
);

