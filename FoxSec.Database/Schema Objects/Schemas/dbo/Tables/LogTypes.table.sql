CREATE TABLE [dbo].[LogTypes] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [NumberOfError] INT            NOT NULL,
    [Name]          NVARCHAR (200) NOT NULL,
    [Timestamp]     TIMESTAMP      NOT NULL
);

