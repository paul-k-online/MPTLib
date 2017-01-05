USE [MPT]
GO
INSERT [dbo].[AlarmTags] ([PlcId], [TagId], [Name], [LowAlarm], [HighAlarm], [Description]) VALUES (2, 1, N'I:0/1', 0, NULL, N'Программа A-I')
INSERT [dbo].[AlarmTags] ([PlcId], [TagId], [Name], [LowAlarm], [HighAlarm], [Description]) VALUES (2, 2, N'I:0/2', 0, NULL, N'Программа A-II')
INSERT [dbo].[AlarmTags] ([PlcId], [TagId], [Name], [LowAlarm], [HighAlarm], [Description]) VALUES (2, 3, N'I:0/15', 0, NULL, N'Программа A-R')
INSERT [dbo].[AlarmTags] ([PlcId], [TagId], [Name], [LowAlarm], [HighAlarm], [Description]) VALUES (2, 4, N'I:0/16', 0, NULL, N'Программа D (дремлющая)')
INSERT [dbo].[AlarmTags] ([PlcId], [TagId], [Name], [LowAlarm], [HighAlarm], [Description]) VALUES (16, 1, N'Data.t[9]', 0.5, NULL, N'F1364 Расход этилена')
INSERT [dbo].[AlarmTags] ([PlcId], [TagId], [Name], [LowAlarm], [HighAlarm], [Description]) VALUES (10522, 1, N'_BP1:3:I.Data.19', 0, NULL, N'K1-2')
INSERT [dbo].[AlarmTags] ([PlcId], [TagId], [Name], [LowAlarm], [HighAlarm], [Description]) VALUES (10522, 2, N'_BP1:3:I.Data.20', 0, NULL, N'K3-3')
