USE [DynamicForm]
GO

/****** Object:  Table [dbo].[FormBasicInfo]    Script Date: 7/15/2024 10:36:17 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FormBasicInfo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](500) NULL,
	[Version] [int] NOT NULL,
	[AtCreatDateTime] [datetime] NOT NULL,
	[EmpNo] [varchar](20) NOT NULL,
 CONSTRAINT [PK__FormBasi__3214EC0738125911] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[FormBasicInfo] ADD  CONSTRAINT [DF_FormBasicInfo_Name]  DEFAULT ('未命名') FOR [Name]
GO

ALTER TABLE [dbo].[FormBasicInfo] ADD  CONSTRAINT [DF_FormBasicInfo_Version]  DEFAULT ((1)) FOR [Version]
GO

ALTER TABLE [dbo].[FormBasicInfo] ADD  CONSTRAINT [DF_FormBasicInfo_AtCreatDateTime]  DEFAULT (getdate()) FOR [AtCreatDateTime]
GO

ALTER TABLE [dbo].[FormBasicInfo] ADD  CONSTRAINT [DF_FormBasicInfo_EmpNo]  DEFAULT ((0)) FOR [EmpNo]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'使用者編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FormBasicInfo', @level2type=N'COLUMN',@level2name=N'EmpNo'
GO


