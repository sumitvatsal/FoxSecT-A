CREATE TABLE [dbo].[Departments] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Number]       INT            NOT NULL,
    [Name]         NVARCHAR (150) NOT NULL,
    [ModifiedLast] DATETIME       NOT NULL,
    [ModifiedBy]   NVARCHAR (100) NOT NULL,
    [Timestamp]    TIMESTAMP      NOT NULL,
    [CompanyId]    INT            NOT NULL,
    [IsDeleted]    BIT            NOT NULL
);

