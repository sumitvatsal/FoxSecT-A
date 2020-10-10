CREATE TABLE [dbo].[AuditLog] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [EventTypeId] INT            NOT NULL,
    [EventTime]   DATETIME       NOT NULL,
    [UserName]    NVARCHAR (500) NOT NULL,
    [OldValue]    NVARCHAR (MAX) NOT NULL,
    [NewValue]    NVARCHAR (MAX) NOT NULL
);

