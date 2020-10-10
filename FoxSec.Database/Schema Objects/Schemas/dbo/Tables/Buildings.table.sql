CREATE TABLE [dbo].[Buildings] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [AdressStreet] NVARCHAR (75) NULL,
    [AdressHouse]  NVARCHAR (15) NULL,
    [AdressIndex]  NVARCHAR (50) NULL,
    [LocationId]   INT           NOT NULL,
    [Name]         NVARCHAR (75) NOT NULL,
    [IsDeleted]    BIT           NOT NULL,
    [Floors]       INT           NOT NULL
);

