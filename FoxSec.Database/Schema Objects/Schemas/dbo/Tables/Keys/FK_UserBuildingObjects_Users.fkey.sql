﻿ALTER TABLE [dbo].[UserBuildingObjects]
    ADD CONSTRAINT [FK_UserBuildingObjects_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

