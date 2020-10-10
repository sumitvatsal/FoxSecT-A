ALTER TABLE [dbo].[UserDepartments]
    ADD CONSTRAINT [FK_UserDepartments_Departments] FOREIGN KEY ([DepartmentId]) REFERENCES [dbo].[Departments] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

