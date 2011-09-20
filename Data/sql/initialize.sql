USE [Fantasy]
GO
INSERT [dbo].[ASSEMBLYREFERENCEGROUP] ([ID], [MODIFICATIONTIME], [CREATIONTIME], [ISSYSTEM]) VALUES (N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x00009F5601617BC5 AS DateTime), CAST(0x00009F5601617BC5 AS DateTime), 1)
INSERT [dbo].[BUSINESSPACKAGE] ([ID], [PARENTPACKAGEID], [NAME], [CODENAME], [MODIFICATIONTIME], [CREATIONTIME], [ISSYSTEM]) VALUES (N'28f26cdc-9ce5-4d0a-814d-08ce58105e25', NULL, N'System Root', N'Fantasy', CAST(0x00009ED1015FD650 AS DateTime), CAST(0x00009ED1015FD650 AS DateTime), 1)
INSERT [dbo].[BUSINESSPACKAGE] ([ID], [PARENTPACKAGEID], [NAME], [CODENAME], [MODIFICATIONTIME], [CREATIONTIME], [ISSYSTEM]) VALUES (N'bdf7bbb0-0991-4f13-ad22-d45bfec45b66', N'28f26cdc-9ce5-4d0a-814d-08ce58105e25', N'BusinessEngine', N'BusinessEngine', CAST(0x00009F3601493364 AS DateTime), CAST(0x00009EDD015FD650 AS DateTime), 1)
INSERT [dbo].[BUSINESSCLASS] ([ID], [NAME], [CODENAME], [PACKAGEID], [PARENTCLASSID], [TABLENAME], [TABLESCHEMA], [TABLESPACE], [CREATIONTIME], [MODIFICATIONTIME], [ISSYSTEM], [ISSIMPLE], [IMPLEMENTS], [AUTOSCRIPT], [SCRIPT], [SCRIPTOPTIONS]) VALUES (N'bf0aa7f4-588f-4556-963d-33242e649d57', N'BusinessObject', N'BusinessObject', N'bdf7bbb0-0991-4f13-ad22-d45bfec45b66', NULL, N'BusinessObject', N'dbo', NULL, CAST(0x00009EDA00000000 AS DateTime), CAST(0x00009F3300FF6C48 AS DateTime), 1, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[BUSINESSROLE] ([ID], [MODIFICATIONTIME], [CREATIONTIME], [ISSYSTEM], [PACKAGEID], [NAME], [DESCRIPTION], [ISCOMPUTED], [ISDISABLED]) VALUES (N'a51973b2-7ce8-440e-8a9c-1b48b59b0499', CAST(0x00009F6400BAF48C AS DateTime), CAST(0x00009F6400BAB544 AS DateTime), 1, N'bdf7bbb0-0991-4f13-ad22-d45bfec45b66', N'Everyone', N'Built-in role for representing everyone in system. ', 1, 0)
INSERT [dbo].[BUSINESSROLE] ([ID], [MODIFICATIONTIME], [CREATIONTIME], [ISSYSTEM], [PACKAGEID], [NAME], [DESCRIPTION], [ISCOMPUTED], [ISDISABLED]) VALUES (N'e8700b4b-7911-470e-8e0e-458507d1f51d', CAST(0x00009F6400BAE550 AS DateTime), CAST(0x00009F6400BA5D60 AS DateTime), 1, N'bdf7bbb0-0991-4f13-ad22-d45bfec45b66', N'Users', N'Built-in role for representing authenticated users in the system.', 0, 0)
INSERT [dbo].[BUSINESSROLE] ([ID], [MODIFICATIONTIME], [CREATIONTIME], [ISSYSTEM], [PACKAGEID], [NAME], [DESCRIPTION], [ISCOMPUTED], [ISDISABLED]) VALUES (N'6d21bc2a-870a-4928-b219-fae9c4c9fd15', CAST(0x00009F6400BA59DC AS DateTime), CAST(0x00009F6400B9CD3C AS DateTime), 1, N'bdf7bbb0-0991-4f13-ad22-d45bfec45b66', N'Administrators', N'Administrators have complete and unrestricted access to the system.', 0, 0)
INSERT [dbo].[BUSINESSDATATYPE] ([ID], [NAME], [CODENAME], [DEFAULTDBTYPE], [DEFAULTLENGTH], [DEFAULTPRECISION], [MODIFICATIONTIME], [CREATIONTIME], [ISSYSTEM]) VALUES (N'0ff5fd67-2cad-48ed-bdc3-042b5dd321a1', N'Bitmap', N'System.Drawing.Bitmap', N'image', 0, 0, CAST(0x00009F2E00B581AB AS DateTime), CAST(0x00009F2E00B581AB AS DateTime), 1)
INSERT [dbo].[BUSINESSDATATYPE] ([ID], [NAME], [CODENAME], [DEFAULTDBTYPE], [DEFAULTLENGTH], [DEFAULTPRECISION], [MODIFICATIONTIME], [CREATIONTIME], [ISSYSTEM]) VALUES (N'a2b55595-6959-4ec7-8daf-23d2ba6b2c99', N'Guid', N'Guid', N'uniqueidentifier', 0, 0, CAST(0x00009F2E00AF6CF8 AS DateTime), CAST(0x00009F2E00AF6CF8 AS DateTime), 1)
INSERT [dbo].[BUSINESSDATATYPE] ([ID], [NAME], [CODENAME], [DEFAULTDBTYPE], [DEFAULTLENGTH], [DEFAULTPRECISION], [MODIFICATIONTIME], [CREATIONTIME], [ISSYSTEM]) VALUES (N'b9ceb0d0-ef32-44ff-8cdc-3b609a65d27f', N'Boolean', N'bool', N'bit', 0, 0, CAST(0x00009F2E00AB96FA AS DateTime), CAST(0x00009F2E00AB96FA AS DateTime), 1)
INSERT [dbo].[BUSINESSDATATYPE] ([ID], [NAME], [CODENAME], [DEFAULTDBTYPE], [DEFAULTLENGTH], [DEFAULTPRECISION], [MODIFICATIONTIME], [CREATIONTIME], [ISSYSTEM]) VALUES (N'24473090-539e-4c13-be25-46e6f0dd9051', N'BusinessClass', N'BusinessClass', N'uniqueidentifier', 0, 0, CAST(0x00009F2E00B1DA46 AS DateTime), CAST(0x00009F2E00B1DA46 AS DateTime), 1)
INSERT [dbo].[BUSINESSDATATYPE] ([ID], [NAME], [CODENAME], [DEFAULTDBTYPE], [DEFAULTLENGTH], [DEFAULTPRECISION], [MODIFICATIONTIME], [CREATIONTIME], [ISSYSTEM]) VALUES (N'1110c74f-5716-4a2a-8787-49fb2bff75f3', N'Date', N'DateTime', N'date', 0, 0, CAST(0x00009F2E00B06EB3 AS DateTime), CAST(0x00009F2E00B06EB3 AS DateTime), 1)
INSERT [dbo].[BUSINESSDATATYPE] ([ID], [NAME], [CODENAME], [DEFAULTDBTYPE], [DEFAULTLENGTH], [DEFAULTPRECISION], [MODIFICATIONTIME], [CREATIONTIME], [ISSYSTEM]) VALUES (N'adf5890d-3bbd-4375-ad11-4e0594c5e776', N'Float', N'float', N'float', 24, 0, CAST(0x00009F2E00ADC568 AS DateTime), CAST(0x00009F2E00ADC568 AS DateTime), 1)
INSERT [dbo].[BUSINESSDATATYPE] ([ID], [NAME], [CODENAME], [DEFAULTDBTYPE], [DEFAULTLENGTH], [DEFAULTPRECISION], [MODIFICATIONTIME], [CREATIONTIME], [ISSYSTEM]) VALUES (N'192d0683-3c32-44b2-bf6f-51cb88e98445', N'Binary', N'byte[]', N'varbinary', 0, 0, CAST(0x00009F2E00B15834 AS DateTime), CAST(0x00009F2E00B15834 AS DateTime), 1)
INSERT [dbo].[BUSINESSDATATYPE] ([ID], [NAME], [CODENAME], [DEFAULTDBTYPE], [DEFAULTLENGTH], [DEFAULTPRECISION], [MODIFICATIONTIME], [CREATIONTIME], [ISSYSTEM]) VALUES (N'5d09014d-c22c-4daa-9657-5c92a4081c36', N'DateTimeOffset', N'DateTimeOffset', N'datetimeoffset', 0, 0, CAST(0x00009F32011DC2A4 AS DateTime), CAST(0x00009F32011DC2A4 AS DateTime), 1)
INSERT [dbo].[BUSINESSDATATYPE] ([ID], [NAME], [CODENAME], [DEFAULTDBTYPE], [DEFAULTLENGTH], [DEFAULTPRECISION], [MODIFICATIONTIME], [CREATIONTIME], [ISSYSTEM]) VALUES (N'1e12aa32-dd17-4f37-8d4c-5ffe9b53dfe1', N'String', N'string', N'nvarchar', 255, 0, CAST(0x00009F2E00A996C5 AS DateTime), CAST(0x00009F2E00A996C5 AS DateTime), 1)
INSERT [dbo].[BUSINESSDATATYPE] ([ID], [NAME], [CODENAME], [DEFAULTDBTYPE], [DEFAULTLENGTH], [DEFAULTPRECISION], [MODIFICATIONTIME], [CREATIONTIME], [ISSYSTEM]) VALUES (N'89d45b78-72ce-4a13-8aa9-80fa0da4a4c3', N'Time', N'DateTime', N'time', 0, 0, CAST(0x00009F2E00B0815E AS DateTime), CAST(0x00009F2E00B0815E AS DateTime), 1)
INSERT [dbo].[BUSINESSDATATYPE] ([ID], [NAME], [CODENAME], [DEFAULTDBTYPE], [DEFAULTLENGTH], [DEFAULTPRECISION], [MODIFICATIONTIME], [CREATIONTIME], [ISSYSTEM]) VALUES (N'a2883f92-473f-4a07-8d96-85e3fcf31588', N'DateTime', N'DateTime', N'datetime2', 0, 0, CAST(0x00009F2E00B04E76 AS DateTime), CAST(0x00009F2E00B04E76 AS DateTime), 1)
INSERT [dbo].[BUSINESSDATATYPE] ([ID], [NAME], [CODENAME], [DEFAULTDBTYPE], [DEFAULTLENGTH], [DEFAULTPRECISION], [MODIFICATIONTIME], [CREATIONTIME], [ISSYSTEM]) VALUES (N'5baf8a91-4204-420c-b888-8bd26e9289bf', N'Double', N'double', N'float', 0, 0, CAST(0x00009F2E00AE5044 AS DateTime), CAST(0x00009F2E00AE5044 AS DateTime), 1)
INSERT [dbo].[BUSINESSDATATYPE] ([ID], [NAME], [CODENAME], [DEFAULTDBTYPE], [DEFAULTLENGTH], [DEFAULTPRECISION], [MODIFICATIONTIME], [CREATIONTIME], [ISSYSTEM]) VALUES (N'f1e72c1d-2432-4da6-82d6-aa2ddeda84ed', N'Enumeration', N'enum', N'int', 0, 0, CAST(0x00009F2E00B1A045 AS DateTime), CAST(0x00009F2E00B1A045 AS DateTime), 1)
INSERT [dbo].[BUSINESSDATATYPE] ([ID], [NAME], [CODENAME], [DEFAULTDBTYPE], [DEFAULTLENGTH], [DEFAULTPRECISION], [MODIFICATIONTIME], [CREATIONTIME], [ISSYSTEM]) VALUES (N'ac55ad0f-eb2e-4082-9404-ae145cf8ff15', N'Byte', N'byte', N'tinyint', 0, 0, CAST(0x00009F32011D6E58 AS DateTime), CAST(0x00009F32011D6E58 AS DateTime), 1)
INSERT [dbo].[BUSINESSDATATYPE] ([ID], [NAME], [CODENAME], [DEFAULTDBTYPE], [DEFAULTLENGTH], [DEFAULTPRECISION], [MODIFICATIONTIME], [CREATIONTIME], [ISSYSTEM]) VALUES (N'd5b4d680-c2ec-4294-982d-b1bf3b89b81e', N'Long', N'long', N'numberic', 18, 0, CAST(0x00009F2E00AC761F AS DateTime), CAST(0x00009F2E00AC761F AS DateTime), 1)
INSERT [dbo].[BUSINESSDATATYPE] ([ID], [NAME], [CODENAME], [DEFAULTDBTYPE], [DEFAULTLENGTH], [DEFAULTPRECISION], [MODIFICATIONTIME], [CREATIONTIME], [ISSYSTEM]) VALUES (N'1d101c89-cd94-4203-a571-b6c3ddcff4ec', N'ULong', N'ulong', N'numberic', 0, 0, CAST(0x00009F32011F3348 AS DateTime), CAST(0x00009F32011F3348 AS DateTime), 1)
INSERT [dbo].[BUSINESSDATATYPE] ([ID], [NAME], [CODENAME], [DEFAULTDBTYPE], [DEFAULTLENGTH], [DEFAULTPRECISION], [MODIFICATIONTIME], [CREATIONTIME], [ISSYSTEM]) VALUES (N'44288a16-267e-4fda-a588-b82fba83bb13', N'Integer', N'int', N'numeric', 18, 0, CAST(0x00009F2E00AAA3EB AS DateTime), CAST(0x00009F2E00AAA3EB AS DateTime), 1)
INSERT [dbo].[BUSINESSDATATYPE] ([ID], [NAME], [CODENAME], [DEFAULTDBTYPE], [DEFAULTLENGTH], [DEFAULTPRECISION], [MODIFICATIONTIME], [CREATIONTIME], [ISSYSTEM]) VALUES (N'1bd22188-8558-432f-9c8d-c01b748a2b65', N'UShort', N'ushort', N'int', 0, 0, CAST(0x00009F32011EB552 AS DateTime), CAST(0x00009F32011EB552 AS DateTime), 1)
INSERT [dbo].[BUSINESSDATATYPE] ([ID], [NAME], [CODENAME], [DEFAULTDBTYPE], [DEFAULTLENGTH], [DEFAULTPRECISION], [MODIFICATIONTIME], [CREATIONTIME], [ISSYSTEM]) VALUES (N'2d3b89de-6ced-4421-9216-d311ea4fbbee', N'UInt', N'uint', N'numberic', 18, 0, CAST(0x00009F2E00AC2BA4 AS DateTime), CAST(0x00009F2E00AC2BA4 AS DateTime), 1)
INSERT [dbo].[BUSINESSDATATYPE] ([ID], [NAME], [CODENAME], [DEFAULTDBTYPE], [DEFAULTLENGTH], [DEFAULTPRECISION], [MODIFICATIONTIME], [CREATIONTIME], [ISSYSTEM]) VALUES (N'e10218c6-1b0b-4269-a879-df0617f1e6e0', N'Text', N'string', N'text', 0, 0, CAST(0x00009F2E00B2B92B AS DateTime), CAST(0x00009F2E00B2B92B AS DateTime), 1)
INSERT [dbo].[BUSINESSDATATYPE] ([ID], [NAME], [CODENAME], [DEFAULTDBTYPE], [DEFAULTLENGTH], [DEFAULTPRECISION], [MODIFICATIONTIME], [CREATIONTIME], [ISSYSTEM]) VALUES (N'54be637f-8fe7-4872-a471-e8bd1c902995', N'Short', N'short', N'smallint', 0, 0, CAST(0x00009F32011E4C0E AS DateTime), CAST(0x00009F32011E4C0E AS DateTime), 1)
INSERT [dbo].[BUSINESSDATATYPE] ([ID], [NAME], [CODENAME], [DEFAULTDBTYPE], [DEFAULTLENGTH], [DEFAULTPRECISION], [MODIFICATIONTIME], [CREATIONTIME], [ISSYSTEM]) VALUES (N'abc026e2-a379-44a4-a59c-ebe636b00f11', N'Decimal', N'decimal', N'float', 0, 0, CAST(0x00009F2E00AEEB7D AS DateTime), CAST(0x00009F2E00AEEB7D AS DateTime), 1)
INSERT [dbo].[BUSINESSPROPERTY] ([ID], [NAME], [CODENAME], [CLASSID], [DATATYPEID], [ENUMTYPEID], [CLASSTYPEID], [FIELDNAME], [FIELDTYPE], [LENGTH], [PRECISION], [ISNULLABLE], [DEFAULTVALUE], [SCRIPTOPTIONS], [DISPLAYORDER], [ISCALCULATED], [CREATIONTIME], [MODIFICATIONTIME], [ISSYSTEM]) VALUES (N'c9b092be-fce4-4793-9bba-9f3300ac9427', N'Id', N'Id', N'bf0aa7f4-588f-4556-963d-33242e649d57', N'a2b55595-6959-4ec7-8daf-23d2ba6b2c99', NULL, NULL, N'Id', N'uniqueidentifier', 0, 0, 0, NULL, NULL, 6, 0, CAST(0x00009F3300AAF604 AS DateTime), CAST(0x00009F3300AC93B0 AS DateTime), 1)
INSERT [dbo].[BUSINESSPROPERTY] ([ID], [NAME], [CODENAME], [CLASSID], [DATATYPEID], [ENUMTYPEID], [CLASSTYPEID], [FIELDNAME], [FIELDTYPE], [LENGTH], [PRECISION], [ISNULLABLE], [DEFAULTVALUE], [SCRIPTOPTIONS], [DISPLAYORDER], [ISCALCULATED], [CREATIONTIME], [MODIFICATIONTIME], [ISSYSTEM]) VALUES (N'57b4a057-35b6-4e09-89f4-9f3300ac942f', N'Class', N'Class', N'bf0aa7f4-588f-4556-963d-33242e649d57', N'a2b55595-6959-4ec7-8daf-23d2ba6b2c99', NULL, NULL, N'ClassId', N'uniqueidentifier', 0, 0, 0, NULL, NULL, 8, 0, CAST(0x00009F3300AC1C28 AS DateTime), CAST(0x00009F3300CA3668 AS DateTime), 1)
INSERT [dbo].[BUSINESSPROPERTY] ([ID], [NAME], [CODENAME], [CLASSID], [DATATYPEID], [ENUMTYPEID], [CLASSTYPEID], [FIELDNAME], [FIELDTYPE], [LENGTH], [PRECISION], [ISNULLABLE], [DEFAULTVALUE], [SCRIPTOPTIONS], [DISPLAYORDER], [ISCALCULATED], [CREATIONTIME], [MODIFICATIONTIME], [ISSYSTEM]) VALUES (N'0be3b780-5b3d-4840-8aaa-9f3300ac942f', N'Creation Time', N'CreationTime', N'bf0aa7f4-588f-4556-963d-33242e649d57', N'a2883f92-473f-4a07-8d96-85e3fcf31588', NULL, NULL, N'CreationTime', N'datetime2', 0, 0, 0, NULL, NULL, 9, 0, CAST(0x00009F3300AB561C AS DateTime), CAST(0x00009F3300CA3668 AS DateTime), 1)
INSERT [dbo].[BUSINESSPROPERTY] ([ID], [NAME], [CODENAME], [CLASSID], [DATATYPEID], [ENUMTYPEID], [CLASSTYPEID], [FIELDNAME], [FIELDTYPE], [LENGTH], [PRECISION], [ISNULLABLE], [DEFAULTVALUE], [SCRIPTOPTIONS], [DISPLAYORDER], [ISCALCULATED], [CREATIONTIME], [MODIFICATIONTIME], [ISSYSTEM]) VALUES (N'4ed247a4-7e5a-4b2d-9444-9f3300ac942f', N'Modification Time', N'ModificationTime', N'bf0aa7f4-588f-4556-963d-33242e649d57', N'a2883f92-473f-4a07-8d96-85e3fcf31588', NULL, NULL, N'ModificationTime', N'datetime2', 0, 0, 0, NULL, NULL, 10, 0, CAST(0x00009F3300AB8754 AS DateTime), CAST(0x00009F3300CA3668 AS DateTime), 1)
INSERT [dbo].[BUSINESSPROPERTY] ([ID], [NAME], [CODENAME], [CLASSID], [DATATYPEID], [ENUMTYPEID], [CLASSTYPEID], [FIELDNAME], [FIELDTYPE], [LENGTH], [PRECISION], [ISNULLABLE], [DEFAULTVALUE], [SCRIPTOPTIONS], [DISPLAYORDER], [ISCALCULATED], [CREATIONTIME], [MODIFICATIONTIME], [ISSYSTEM]) VALUES (N'e5434005-55e8-482f-a46a-9f3300ac942f', N'Name', N'Name', N'bf0aa7f4-588f-4556-963d-33242e649d57', N'1e12aa32-dd17-4f37-8d4c-5ffe9b53dfe1', NULL, NULL, N'Name', N'nvarchar', 255, 0, 0, NULL, NULL, 7, 0, CAST(0x00009F3300AB192C AS DateTime), CAST(0x00009F3300AC93B0 AS DateTime), 1)
INSERT [dbo].[BUSINESSPROPERTY] ([ID], [NAME], [CODENAME], [CLASSID], [DATATYPEID], [ENUMTYPEID], [CLASSTYPEID], [FIELDNAME], [FIELDTYPE], [LENGTH], [PRECISION], [ISNULLABLE], [DEFAULTVALUE], [SCRIPTOPTIONS], [DISPLAYORDER], [ISCALCULATED], [CREATIONTIME], [MODIFICATIONTIME], [ISSYSTEM]) VALUES (N'9171981d-4d61-499d-b8a9-9f3300ac942f', N'Is System', N'IsSystem', N'bf0aa7f4-588f-4556-963d-33242e649d57', N'b9ceb0d0-ef32-44ff-8cdc-3b609a65d27f', NULL, NULL, N'IsSystem', N'bit', 0, 0, 0, N'0', NULL, 11, 0, CAST(0x00009F3300ABC444 AS DateTime), CAST(0x00009F3300CA3668 AS DateTime), 1)
INSERT [dbo].[BUSINESSUSER] ([ID], [MODIFICATIONTIME], [CREATIONTIME], [ISSYSTEM], [PACKAGEID], [NAME], [FULLNAME], [DESCRIPTION], [PASSWORD], [ISDISABLED]) VALUES (N'cee4f357-6985-4beb-bcea-0bcabd232c4a', CAST(0x00009F6400C08460 AS DateTime), CAST(0x00009F6400C028F8 AS DateTime), 1, N'bdf7bbb0-0991-4f13-ad22-d45bfec45b66', N'Administrator', N'Administrator', N'Built-in account for administring the system.', N'/JXYlYVtZLIrwW0SjpcFlA==', 0)
INSERT [dbo].[BUSINESSUSERROLE] ([USERID], [ROLEID]) VALUES (N'cee4f357-6985-4beb-bcea-0bcabd232c4a', N'6d21bc2a-870a-4928-b219-fae9c4c9fd15')
