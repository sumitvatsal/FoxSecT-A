ALTER TABLE [dbo].[PermissionGroupTimeZones]  WITH CHECK ADD  CONSTRAINT [FK_PermissionGroupTimeZones_BuildingObjects] FOREIGN KEY([BuildingObjectId])
REFERENCES [dbo].[BuildingObjects] ([Id])


GO
ALTER TABLE [dbo].[PermissionGroupTimeZones] CHECK CONSTRAINT [FK_PermissionGroupTimeZones_BuildingObjects]

