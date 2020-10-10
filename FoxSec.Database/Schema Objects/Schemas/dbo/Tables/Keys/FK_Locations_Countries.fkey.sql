ALTER TABLE [dbo].[Locations]
    ADD CONSTRAINT [FK_Locations_Countries] FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Countries] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

