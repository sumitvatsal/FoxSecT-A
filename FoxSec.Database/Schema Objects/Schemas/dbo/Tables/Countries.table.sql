CREATE TABLE [dbo].[Countries] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (75) NOT NULL,
    [Timestamp] TIMESTAMP     NOT NULL,
    [IsDeleted] BIT           NOT NULL
);

