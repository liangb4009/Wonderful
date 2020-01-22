/*
 Navicat Premium Data Transfer

 Source Server         : 10.140.210.165_Pim_Qa
 Source Server Type    : SQL Server
 Source Server Version : 11003513
 Source Host           : 10.140.210.165:1433
 Source Catalog        : MESEFORM
 Source Schema         : dbo

 Target Server Type    : SQL Server
 Target Server Version : 11003513
 File Encoding         : 65001

 Date: 22/01/2020 09:09:33
*/


-- ----------------------------
-- Table structure for TSTB_TODOITEM_IMAGE
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[TSTB_TODOITEM_IMAGE]') AND type IN ('U'))
	DROP TABLE [dbo].[TSTB_TODOITEM_IMAGE]
GO

CREATE TABLE [dbo].[TSTB_TODOITEM_IMAGE] (
  [TaskId] nvarchar(450) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [TokenId] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [RunMode] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [CameraType] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [MachineName] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [CaptureCount] int  NOT NULL,
  [CaptureFrequency] int  NOT NULL,
  [IsComplete] bit  NOT NULL,
  [CreateDate] datetime DEFAULT (getdate()) NULL
)
GO

ALTER TABLE [dbo].[TSTB_TODOITEM_IMAGE] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for TSTB_TODOITEM_PRINT
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[TSTB_TODOITEM_PRINT]') AND type IN ('U'))
	DROP TABLE [dbo].[TSTB_TODOITEM_PRINT]
GO

CREATE TABLE [dbo].[TSTB_TODOITEM_PRINT] (
  [TaskId] nvarchar(450) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [TokenId] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [FileName] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [PrintName] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [PrintType] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [PrintCount] int  NOT NULL,
  [CurrentDirectory] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [TemplateFolder] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [BartendExePath] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [IsComplete] bit  NOT NULL,
  [GenerateExcelFile] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [GeneratePdfFile] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [CreateDate] datetime DEFAULT (getdate()) NULL
)
GO

ALTER TABLE [dbo].[TSTB_TODOITEM_PRINT] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for TSTB_TODOITEM_SCALE
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[TSTB_TODOITEM_SCALE]') AND type IN ('U'))
	DROP TABLE [dbo].[TSTB_TODOITEM_SCALE]
GO

CREATE TABLE [dbo].[TSTB_TODOITEM_SCALE] (
  [TaskId] nvarchar(450) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [TokenId] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [ScaleType] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [ScaleName] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [ScaleCount] int  NOT NULL,
  [ScaleFrequency] int  NOT NULL,
  [IsComplete] bit  NOT NULL
)
GO

ALTER TABLE [dbo].[TSTB_TODOITEM_SCALE] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Primary Key structure for table TSTB_TODOITEM_IMAGE
-- ----------------------------
ALTER TABLE [dbo].[TSTB_TODOITEM_IMAGE] ADD CONSTRAINT [PK_TSTB_TODOITEM_IMAGE] PRIMARY KEY CLUSTERED ([TaskId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table TSTB_TODOITEM_PRINT
-- ----------------------------
ALTER TABLE [dbo].[TSTB_TODOITEM_PRINT] ADD CONSTRAINT [PK_TSTB_TODOITEM_PRINT] PRIMARY KEY CLUSTERED ([TaskId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table TSTB_TODOITEM_SCALE
-- ----------------------------
ALTER TABLE [dbo].[TSTB_TODOITEM_SCALE] ADD CONSTRAINT [PK_TSTB_TODOITEM_SCALE] PRIMARY KEY CLUSTERED ([TaskId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO

