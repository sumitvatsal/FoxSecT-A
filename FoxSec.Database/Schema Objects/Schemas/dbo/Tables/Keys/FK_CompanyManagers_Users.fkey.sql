ALTER TABLE [dbo].[CompanyManagers]
    ADD CONSTRAINT [FK_CompanyManagers_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

