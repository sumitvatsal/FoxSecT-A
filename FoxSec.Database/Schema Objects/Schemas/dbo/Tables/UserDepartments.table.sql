CREATE TABLE [dbo].[UserDepartments] (
    [Id]                  INT       IDENTITY (1, 1) NOT NULL,
    [UserId]              INT       NOT NULL,
    [DepartmentId]        INT       NOT NULL,
    [Timestamp]           TIMESTAMP NOT NULL,
    [ValidFrom]           DATETIME  NOT NULL,
    [ValidTo]             DATETIME  NOT NULL,
    [CurrentDep]          BIT       NOT NULL,
    [IsDeleted]           BIT       NOT NULL,
    [IsDepartmentManager] BIT       NOT NULL
);

