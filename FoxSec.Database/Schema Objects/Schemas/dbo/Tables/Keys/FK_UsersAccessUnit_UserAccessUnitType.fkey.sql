ALTER TABLE [dbo].[UsersAccessUnit]
    ADD CONSTRAINT [FK_UsersAccessUnit_UserAccessUnitType] FOREIGN KEY ([TypeId]) REFERENCES [dbo].[UserAccessUnitType] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

