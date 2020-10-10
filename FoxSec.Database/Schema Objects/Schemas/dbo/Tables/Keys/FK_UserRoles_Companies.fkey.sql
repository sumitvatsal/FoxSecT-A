ALTER TABLE [dbo].[UserRoles]
    ADD CONSTRAINT [FK_UserRoles_Companies] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Companies] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

