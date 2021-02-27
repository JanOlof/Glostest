USE [WordLearning2]
GO
/****** Object:  Table [dbo].[wordLanguage]    Script Date: 2021-02-14 11:04:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[wordLanguage](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Code] [varchar](50) NULL,
 CONSTRAINT [PK_dbo.wordLanguage] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[wordSynonyms]    Script Date: 2021-02-14 11:04:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[wordSynonyms](
	[SynonymId] [int] NOT NULL,
	[WordId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.wordSynonyms] PRIMARY KEY CLUSTERED 
(
	[SynonymId] ASC,
	[WordId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[wordUser]    Script Date: 2021-02-14 11:04:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[wordUser](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_dbo.wordUser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[wordWord]    Script Date: 2021-02-14 11:04:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[wordWord](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Text] [varchar](150) NOT NULL,
	[LanguageId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.wordWord] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[wordWordGroup]    Script Date: 2021-02-14 11:04:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[wordWordGroup](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](50) NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.wordWordGroup] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[wordWordGroupSynonym]    Script Date: 2021-02-14 11:04:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[wordWordGroupSynonym](
	[WordGroupId] [int] NOT NULL,
	[SynonymId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.wordWordGroupSynonym] PRIMARY KEY CLUSTERED 
(
	[WordGroupId] ASC,
	[SynonymId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[wordSynonyms]  WITH CHECK ADD  CONSTRAINT [FK_dbo.wordSynonyms_dbo.wordWord_WordId] FOREIGN KEY([WordId])
REFERENCES [dbo].[wordWord] ([Id])
GO
ALTER TABLE [dbo].[wordSynonyms] CHECK CONSTRAINT [FK_dbo.wordSynonyms_dbo.wordWord_WordId]
GO
ALTER TABLE [dbo].[wordWord]  WITH CHECK ADD  CONSTRAINT [FK_dbo.wordWord_dbo.wordLanguage_LanguageId] FOREIGN KEY([LanguageId])
REFERENCES [dbo].[wordLanguage] ([Id])
GO
ALTER TABLE [dbo].[wordWord] CHECK CONSTRAINT [FK_dbo.wordWord_dbo.wordLanguage_LanguageId]
GO
ALTER TABLE [dbo].[wordWordGroup]  WITH CHECK ADD  CONSTRAINT [FK_dbo.wordWordGroup_dbo.wordUser_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[wordUser] ([Id])
GO
ALTER TABLE [dbo].[wordWordGroup] CHECK CONSTRAINT [FK_dbo.wordWordGroup_dbo.wordUser_UserId]
GO
ALTER TABLE [dbo].[wordWordGroupSynonym]  WITH CHECK ADD  CONSTRAINT [FK_dbo.wordWordGroupSynonym_dbo.wordWordGroup_WordGroupId] FOREIGN KEY([WordGroupId])
REFERENCES [dbo].[wordWordGroup] ([Id])
GO
ALTER TABLE [dbo].[wordWordGroupSynonym] CHECK CONSTRAINT [FK_dbo.wordWordGroupSynonym_dbo.wordWordGroup_WordGroupId]
GO
