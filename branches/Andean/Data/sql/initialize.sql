INSERT INTO [Fantasy].[dbo].[BUSINESSPACKAGE]
           ([ID]
           ,[PARENTPACKAGEID]
           ,[NAME]
           ,[CODENAME]
           ,[MODIFICATIONTIME]
           ,[CREATIONTIME]
           ,[ISSYSTEM]
		   ,[BUILDASASSEMBLY])
     VALUES
           ('28f26cdc-9ce5-4d0a-814d-08ce58105e25'
           ,NULL
           ,'System Root'
           ,'Fantasy'
           ,'2011-4-26 21:21:00'
           ,'2011-4-26 21:21:00'
           ,1
		   ,0)
GO

INSERT INTO [Fantasy].[dbo].[BUSINESSPACKAGE]
           ([ID]
           ,[PARENTPACKAGEID]
           ,[NAME]
           ,[CODENAME]
           ,[MODIFICATIONTIME]
           ,[CREATIONTIME]
           ,[ISSYSTEM]
		   ,[BUILDASASSEMBLY])
     VALUES
           ('BDF7BBB0-0991-4F13-AD22-D45BFEC45B66'
           ,'28f26cdc-9ce5-4d0a-814d-08ce58105e25'
           ,'BusinessEngine'
           ,'BusinessEngine'
           ,'2011-5-8 21:21:00'
           ,'2011-5-8 21:21:00'
           ,1
		   ,0)
GO

INSERT INTO [Fantasy].[dbo].[BUSINESSCLASS]
           ([ID]
           ,[NAME]
           ,[CODENAME]
           ,[PACKAGEID]
           ,[PARENTCLASSID]
           ,[TABLENAME]
           ,[TABLESCHEMA]
           ,[TABLESPACE]
           ,[CREATIONTIME]
           ,[MODIFICATIONTIME]
           ,[ISSYSTEM]
           ,[ISSIMPLE])
     VALUES
           ('bf0aa7f4-588f-4556-963d-33242e649d57'
           ,'BusinessObject'
           ,'BusinessObject'
           ,'BDF7BBB0-0991-4F13-AD22-D45BFEC45B66'
           ,NULL
           ,'BusinessObject'
           ,'dbo'
           ,NULL
           ,'2011-5-5'
           ,'2011-5-5'
           ,1
           ,0)
GO
