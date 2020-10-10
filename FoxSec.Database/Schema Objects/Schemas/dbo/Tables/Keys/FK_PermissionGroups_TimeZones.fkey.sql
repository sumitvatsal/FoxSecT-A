ALTER TABLE [dbo].[PermissionGroupTimeZones]  WITH CHECK ADD  CONSTRAINT [FK_PermissionGroups_TimeZones] FOREIGN KEY([TimeZoneId])
REFERENCES [dbo].[TimeZones] ([Id])


GO
ALTER TABLE [dbo].[PermissionGroupTimeZones] CHECK CONSTRAINT [FK_PermissionGroups_TimeZones]

