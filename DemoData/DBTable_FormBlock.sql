USE [DynamicForm]
GO

/****** Object:  Table [dbo].[FormBlock]    Script Date: 7/15/2024 10:38:09 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FormBlock](
	[FormId] [int] NOT NULL,
	[Id] [int] NOT NULL,
	[Title] [varchar](50) NULL,
	[ColCount] [int] NOT NULL,
	[Version] [int] NOT NULL,
	[AtCreatDateTime] [datetime] NOT NULL,
	[AtLastDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_FormBlock] PRIMARY KEY CLUSTERED 
(
	[FormId] ASC,
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[FormBlock] ADD  CONSTRAINT [DF_FormBlock_AtCreatDateTime]  DEFAULT (getdate()) FOR [AtCreatDateTime]
GO

ALTER TABLE [dbo].[FormBlock] ADD  CONSTRAINT [DF_FormBlock_AtLastDateTime]  DEFAULT (getdate()) FOR [AtLastDateTime]
GO


