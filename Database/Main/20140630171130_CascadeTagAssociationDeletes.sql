ALTER TABLE [dbo].[CardTags]  DROP  CONSTRAINT [FK_CardTags_Cards]

ALTER TABLE [dbo].[CardTags]  WITH CHECK ADD  CONSTRAINT [FK_CardTags_Cards] FOREIGN KEY([CardId])
REFERENCES [dbo].[Cards] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [dbo].[CardTags] CHECK CONSTRAINT [FK_CardTags_Cards]
GO


ALTER TABLE [dbo].[CardTags]  DROP  CONSTRAINT [FK_CardTags_Tags]
GO

ALTER TABLE [dbo].[CardTags]  WITH CHECK ADD  CONSTRAINT [FK_CardTags_Tags] FOREIGN KEY([TagId])
REFERENCES [dbo].[Tags] ([Id])  ON DELETE CASCADE
GO

ALTER TABLE [dbo].[CardTags] CHECK CONSTRAINT [FK_CardTags_Tags]
GO


