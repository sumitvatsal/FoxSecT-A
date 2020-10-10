ALTER TABLE [dbo].[CompanyBuildingObjects]
    ADD CONSTRAINT [FK_CompanyBuildings_Companies] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Companies] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

