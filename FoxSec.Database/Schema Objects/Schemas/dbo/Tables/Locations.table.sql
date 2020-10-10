CREATE TABLE [dbo].[Locations] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (75) NOT NULL,
    [Timestamp] TIMESTAMP     NOT NULL,
    [CountryId] INT           NOT NULL,
    [IsDeleted] BIT           NOT NULL
);

