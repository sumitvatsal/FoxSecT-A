ALTER TABLE [dbo].[CompanyManagers]
    ADD CONSTRAINT [FK_CompanyManagers_Companies] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Companies] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

