ALTER TABLE [dbo].[Buildings]
    ADD CONSTRAINT [FK_Buildings_Locations] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Locations] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

