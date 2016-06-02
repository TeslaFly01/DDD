USE DDD
GO

--初始化超级管理员
SET IDENTITY_INSERT dbo.SystemAdmin ON
INSERT INTO [SystemAdmin]
           ([SAID]
           ,[SAName]
           ,[SAPwd]
           ,[SANickName]
           ,[SASex]
           ,[SAMobileNo]
           ,[RegTime]
           ,[IsEnable]
           ,[LastIP]
           ,[LastTime]
           ,[CurrentIP]
           ,[CurrentTime]
           ,[Email]
           ,[LoginTimes])
     VALUES
           (1
           ,'admin'
           ,'E10ADC3949BA59ABBE56E057F20F883E'
           ,'超级管理员'
           ,1
           ,'18236520508'
           ,GETDATE()
           ,1
           ,'::1'
           ,GETDATE()
           ,'::1'
           ,GETDATE()
           ,'1@1.com'
           ,0)
GO
SET IDENTITY_INSERT dbo.SystemAdmin OFF


--初始化超级管理员角色
SET IDENTITY_INSERT dbo.AdminRole ON
INSERT INTO [AdminRole]([ARID],[ARName],[Description])VALUES(1,'超级管理员',NULL)
GO
SET IDENTITY_INSERT dbo.AdminRole OFF


--初始化管理员、角色关联表
INSERT INTO [Admin_Role] ([SAID],[ARID]) VALUES (1,1)
GO

--初始化功能菜单表数据
SET IDENTITY_INSERT [dbo].[AdminModule] ON
INSERT [dbo].[AdminModule] ([AMID], [ModuleName], [PageUrl], [FID], [FormRoleName], [SortFlag], [CSSStyle], [Icon], [IsEnable]) VALUES (1, N'管理员', NULL, 0, N'admin', CAST(0x0000A5940115FA7F AS DateTime), NULL, NULL, 1)
INSERT [dbo].[AdminModule] ([AMID], [ModuleName], [PageUrl], [FID], [FormRoleName], [SortFlag], [CSSStyle], [Icon], [IsEnable]) VALUES (2, N'管理员账户', N'/SystemAdmin/SystemAdmin/', 1, N'admin', CAST(0x00009F4A00B37888 AS DateTime), NULL, NULL, 1)
INSERT [dbo].[AdminModule] ([AMID], [ModuleName], [PageUrl], [FID], [FormRoleName], [SortFlag], [CSSStyle], [Icon], [IsEnable]) VALUES (3, N'系统相关', NULL, 0, N'system', CAST(0x00009F4A00B7B22C AS DateTime), NULL, NULL, 1)
INSERT [dbo].[AdminModule] ([AMID], [ModuleName], [PageUrl], [FID], [FormRoleName], [SortFlag], [CSSStyle], [Icon], [IsEnable]) VALUES (4, N'功能模块设置', N'/SystemAdmin/AdminModule/', 3, N'system', CAST(0x0000A594011643F0 AS DateTime), NULL, NULL, 1)
INSERT [dbo].[AdminModule] ([AMID], [ModuleName], [PageUrl], [FID], [FormRoleName], [SortFlag], [CSSStyle], [Icon], [IsEnable]) VALUES (6, N'管理员角色', N'/SystemAdmin/AdminRole/', 1, N'admin', CAST(0x0000A59500A901A0 AS DateTime), NULL, NULL, 1)
INSERT [dbo].[AdminModule] ([AMID], [ModuleName], [PageUrl], [FID], [FormRoleName], [SortFlag], [CSSStyle], [Icon], [IsEnable]) VALUES (7, N'管理员操作日志', N'/SystemAdmin/AdminLog/', 1, N'admin', CAST(0x0000A59500EA4DC3 AS DateTime), NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[AdminModule] OFF


--初始化角色、功能菜单关联表
INSERT [dbo].[AdminRole_Module] ([ARID], [AMID], [Weights]) VALUES (1, 1, 0)
INSERT [dbo].[AdminRole_Module] ([ARID], [AMID], [Weights]) VALUES (1, 2, 0)
INSERT [dbo].[AdminRole_Module] ([ARID], [AMID], [Weights]) VALUES (1, 3, 0)
INSERT [dbo].[AdminRole_Module] ([ARID], [AMID], [Weights]) VALUES (1, 4, 1)
INSERT [dbo].[AdminRole_Module] ([ARID], [AMID], [Weights]) VALUES (1, 6, 0)
INSERT [dbo].[AdminRole_Module] ([ARID], [AMID], [Weights]) VALUES (1, 7, 0)