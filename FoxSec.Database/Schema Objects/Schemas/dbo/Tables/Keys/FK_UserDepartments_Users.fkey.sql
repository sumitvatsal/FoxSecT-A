ALTER TABLE [dbo].[UserDepartments]
    ADD CONSTRAINT [FK_UserDepartments_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

