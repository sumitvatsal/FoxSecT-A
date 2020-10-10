ALTER TABLE [dbo].[BuildingObjects]
    ADD CONSTRAINT [FK_BuildingObjects_BuildingObjectTypes] FOREIGN KEY ([TypeId]) REFERENCES [dbo].[BuildingObjectTypes] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

