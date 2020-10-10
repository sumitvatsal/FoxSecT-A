ALTER TABLE [dbo].[PermissionGroupTimeZones]  WITH CHECK ADD  CONSTRAINT [FK_PermissionGroups_Groups] FOREIGN KEY([PermissionGroupId])
REFERENCES [dbo].[PermissionGroups] ([Id])


GO
ALTER TABLE [dbo].[PermissionGroupTimeZones] CHECK CONSTRAINT [FK_PermissionGroups_Groups]

