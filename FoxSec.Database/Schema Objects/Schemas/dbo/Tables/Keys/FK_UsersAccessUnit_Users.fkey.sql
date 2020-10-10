ALTER TABLE [dbo].[UsersAccessUnit]
    ADD CONSTRAINT [FK_UsersAccessUnit_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

