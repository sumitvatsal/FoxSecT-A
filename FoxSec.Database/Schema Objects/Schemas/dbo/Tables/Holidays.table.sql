CREATE TABLE [dbo].[Holidays] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (200) NOT NULL,
    [EventStart]    DATETIME       NOT NULL,
    [EventEnd]      DATETIME       NOT NULL,
    [Timestamp]     TIMESTAMP      NOT NULL,
    [ModifiedBy]    NVARCHAR (100) NOT NULL,
    [ModifiedLast]  DATETIME       NOT NULL,
    [MovingHoliday] BIT            NOT NULL,
    [IsDeleted]     BIT            NOT NULL
);

