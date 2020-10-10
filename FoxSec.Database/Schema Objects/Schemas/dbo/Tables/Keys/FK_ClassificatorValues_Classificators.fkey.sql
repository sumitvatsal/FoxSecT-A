ALTER TABLE [dbo].[ClassificatorValues]
    ADD CONSTRAINT [FK_ClassificatorValues_Classificators] FOREIGN KEY ([ClassificatorId]) REFERENCES [dbo].[Classificators] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

