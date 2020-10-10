ALTER TABLE [dbo].[TimeZoneProperties]
    ADD CONSTRAINT [FK_TimeZoneProperties_TimeZones] FOREIGN KEY ([TimeZoneId]) REFERENCES [dbo].[TimeZones] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

