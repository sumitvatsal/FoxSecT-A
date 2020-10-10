CREATE TABLE [dbo].[Log] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [LogTypeId]  INT            NOT NULL,
    [Message]    NVARCHAR (MAX) NOT NULL,
    [UserId]     INT            NOT NULL,
    [EventTime]  DATETIME       NOT NULL,
    [MethodName] NVARCHAR (100) NOT NULL
);

