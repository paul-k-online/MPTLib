USE [MPT]
GO
SET IDENTITY_INSERT [dbo].[OpcDataLogger] ON 

INSERT [dbo].[OpcDataLogger] ([Id], [Description], [ConnectionString]) VALUES (1, N'OPCServer', N'opcda:///OPCServer.WinCC/{75d00bbb-dda5-11d1-b944-9e614d000000}')
INSERT [dbo].[OpcDataLogger] ([Id], [Description], [ConnectionString]) VALUES (2, N'RSLinx', N'opcda:///RSLinx OPC Server/{a05bb6d5-2f8a-11d1-9bb0-080009d01446}')
SET IDENTITY_INSERT [dbo].[OpcDataLogger] OFF
