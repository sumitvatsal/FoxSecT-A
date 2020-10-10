CREATE TABLE [dbo].[TimeZoneProperties] (
    [Id]           INT       IDENTITY (1, 1) NOT NULL,
    [TimeZoneId]   INT       NOT NULL,
    [OrderInGroup] INT       NOT NULL,
    [ValidFrom]    DATETIME  NULL,
    [ValidTo]      DATETIME  NULL,
    [IsMonday]     BIT       NOT NULL,
    [IsTuesday]    BIT       NOT NULL,
    [IsWednesday]  BIT       NOT NULL,
    [IsThursday]   BIT       NOT NULL,
    [IsFriday]     BIT       NOT NULL,
    [IsSaturday]   BIT       NOT NULL,
    [IsSunday]     BIT       NOT NULL,
    [Timestamp]    TIMESTAMP NOT NULL
);

