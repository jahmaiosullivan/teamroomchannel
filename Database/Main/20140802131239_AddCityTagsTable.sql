/****** Object:  Table [dbo].[UserTags]    Script Date: 08/02/2014 13:07:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CityTags](
	[CityId] [bigint] NOT NULL,
	[TagId] [bigint] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_CityTags] PRIMARY KEY CLUSTERED 
(
	[CityId] ASC,
	[TagId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO

ALTER TABLE [dbo].[CityTags]  WITH NOCHECK ADD  CONSTRAINT [FK_CityTags_Tags] FOREIGN KEY([TagId])
REFERENCES [dbo].[Tags] ([Id])
GO

ALTER TABLE [dbo].[CityTags] CHECK CONSTRAINT [FK_CityTags_Tags]
GO

ALTER TABLE [dbo].[CityTags]  WITH CHECK ADD  CONSTRAINT [FK_CityTags_City] FOREIGN KEY([CityId], [TagId])
REFERENCES [dbo].[CityTags] ([CityId], [TagId])
GO

ALTER TABLE [dbo].[CityTags] CHECK CONSTRAINT [FK_CityTags_City]
GO

ALTER TABLE [dbo].[CityTags] ADD  CONSTRAINT [DF_CityTags_CreatedDate]  DEFAULT (getutcdate()) FOR [CreatedDate]
GO


