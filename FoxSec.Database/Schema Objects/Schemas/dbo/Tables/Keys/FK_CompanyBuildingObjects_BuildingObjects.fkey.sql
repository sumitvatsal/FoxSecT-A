ALTER TABLE [dbo].[CompanyBuildingObjects]
    ADD CONSTRAINT [FK_CompanyBuildingObjects_BuildingObjects] FOREIGN KEY ([BuildingObjectId]) REFERENCES [dbo].[BuildingObjects] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

