ALTER TABLE [dbo].[Buildings]
    ADD CONSTRAINT [DF_Buildings_Floors] DEFAULT ((5)) FOR [Floors];

