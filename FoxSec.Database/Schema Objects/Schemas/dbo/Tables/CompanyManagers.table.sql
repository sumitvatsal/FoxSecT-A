CREATE TABLE [dbo].[CompanyManagers] (
    [Id]        INT IDENTITY (1, 1) NOT NULL,
    [CompanyId] INT NOT NULL,
    [UserId]    INT NOT NULL,
    [IsDeleted] BIT NOT NULL
);

