CREATE TABLE [dbo].[UsersAccessUnit] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [UserId]    INT           NULL,
    [TypeId]    INT           NULL,
    [CompanyId] INT           NULL,
    [Serial]    NVARCHAR (3)  NULL,
    [Code]      NVARCHAR (50) NULL,
    [Active]    BIT           NOT NULL,
    [Free]      BIT           NOT NULL,
    [Opened]    DATETIME      NULL,
    [Closed]    DATETIME      NULL,
    [ValidFrom] DATETIME      NULL,
    [ValidTo]   DATETIME      NULL,
    [Timestamp] TIMESTAMP     NOT NULL,
    [Dk]        NVARCHAR (5)  NULL,
    [IsDeleted] BIT           NOT NULL,
    [CreatedBy] INT           NOT NULL
);

