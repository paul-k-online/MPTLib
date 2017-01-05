USE [MPT]
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([Id], [Name], [Email], [SeverityLowLevel]) VALUES (1, N'Paul', N'paul@mail.mpt', NULL)
INSERT [dbo].[Users] ([Id], [Name], [Email], [SeverityLowLevel]) VALUES (2, N'mpt', N'mpt@polymir.by', 10)
INSERT [dbo].[Users] ([Id], [Name], [Email], [SeverityLowLevel]) VALUES (3, N'paul_e', N'paul.k.online@gmail.com', 2)
SET IDENTITY_INSERT [dbo].[Users] OFF
