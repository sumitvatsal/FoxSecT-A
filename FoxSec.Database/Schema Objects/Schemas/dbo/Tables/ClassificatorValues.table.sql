CREATE TABLE [dbo].[ClassificatorValues] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [ClassificatorId] INT            NOT NULL,
    [Value]           NVARCHAR (500) NULL,
    [Comments]        NVARCHAR (500) NULL,
    [SortOrder]       INT            NULL,
    [DisplayValue]    NVARCHAR (200) NULL
);

