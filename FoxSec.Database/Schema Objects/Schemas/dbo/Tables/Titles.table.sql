CREATE TABLE [dbo].[Titles] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (150) NOT NULL,
    [Description] NCHAR (200)    NULL,
    [Timestamp]   TIMESTAMP      NOT NULL,
    [CompanyId]   INT            NOT NULL,
    [IsDeleted]   BIT            NOT NULL
);

