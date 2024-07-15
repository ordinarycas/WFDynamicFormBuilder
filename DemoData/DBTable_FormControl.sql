USE [DynamicForm]
GO

/****** Object:  Table [dbo].[FormControl]    Script Date: 7/15/2024 10:38:39 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FormControl](
	[FormId] [int] NOT NULL,
	[Version] [int] NOT NULL,
	[BlockId] [int] NOT NULL,
	[BlockColIndex] [int] NOT NULL,
	[ControlId] [varchar](50) NOT NULL,
	[ControlType] [varchar](50) NOT NULL,
	[ControlTitle] [varchar](50) NULL,
	[AtCreatDateTime] [datetime] NOT NULL,
	[AtLastDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_FormControl] PRIMARY KEY CLUSTERED 
(
	[FormId] ASC,
	[ControlId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[FormControl] ADD  CONSTRAINT [DF_FormControl_AtCreatDateTime]  DEFAULT (getdate()) FOR [AtCreatDateTime]
GO

ALTER TABLE [dbo].[FormControl] ADD  CONSTRAINT [DF_FormControl_AtLastDateTime]  DEFAULT (getdate()) FOR [AtLastDateTime]
GO


