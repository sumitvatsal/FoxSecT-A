ALTER TABLE [dbo].[UserBuildingObjects]
    ADD CONSTRAINT [FK_UserBuildingObjects_BuildingObjects] FOREIGN KEY ([BuildingObjectId]) REFERENCES [dbo].[BuildingObjects] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

