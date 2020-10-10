CREATE TABLE [dbo].[Companies] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (125) NOT NULL,
    [Timestamp]    TIMESTAMP      NOT NULL,
    [ModifiedBy]   NVARCHAR (100) NOT NULL,
    [ModifiedLast] DATETIME       NOT NULL,
    [Comment]      NVARCHAR (250) NULL,
    [Active]       BIT            NOT NULL,
    [IsDeleted]    BIT            NOT NULL,
    [ParentId]     INT            NULL
);

