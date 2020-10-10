ALTER TABLE [dbo].[BuildingObjects]
    ADD CONSTRAINT [FK_BuildingObjects_Buildings] FOREIGN KEY ([BuildingId]) REFERENCES [dbo].[Buildings] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

