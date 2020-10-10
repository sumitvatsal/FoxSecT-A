ALTER TABLE [dbo].[UsersAccessUnit]
    ADD CONSTRAINT [FK_UsersAccessUnit_Companies] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Companies] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

