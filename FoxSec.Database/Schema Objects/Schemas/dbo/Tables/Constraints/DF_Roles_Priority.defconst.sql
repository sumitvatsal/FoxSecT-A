ALTER TABLE [dbo].[Roles]
    ADD CONSTRAINT [DF_Roles_Priority] DEFAULT ((100)) FOR [Priority];

