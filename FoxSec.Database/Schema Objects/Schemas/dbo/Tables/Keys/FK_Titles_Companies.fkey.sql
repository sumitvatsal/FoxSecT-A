ALTER TABLE [dbo].[Titles]
    ADD CONSTRAINT [FK_Titles_Companies] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Companies] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

