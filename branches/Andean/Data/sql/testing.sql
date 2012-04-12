USE [master]
GO
/****** Object:  Database [Fantasy]    Script Date: 2012/4/12 22:28:08 ******/
CREATE DATABASE [Fantasy]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Fantasy', FILENAME = N'E:\Development\Projects\Fantasy\Andean\data\fantasy.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Fantasy_log', FILENAME = N'E:\Development\Projects\Fantasy\Andean\data\fantasy_log.LDF' , SIZE = 3136KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%)
GO
ALTER DATABASE [Fantasy] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Fantasy].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Fantasy] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Fantasy] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Fantasy] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Fantasy] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Fantasy] SET ARITHABORT OFF 
GO
ALTER DATABASE [Fantasy] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [Fantasy] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [Fantasy] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Fantasy] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Fantasy] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Fantasy] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Fantasy] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Fantasy] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Fantasy] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Fantasy] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Fantasy] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Fantasy] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Fantasy] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Fantasy] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Fantasy] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Fantasy] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Fantasy] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Fantasy] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Fantasy] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Fantasy] SET  MULTI_USER 
GO
ALTER DATABASE [Fantasy] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Fantasy] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Fantasy] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Fantasy] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [Fantasy]
GO
/****** Object:  UserDefinedDataType [dbo].[T_DBEntity]    Script Date: 2012/4/12 22:28:08 ******/
CREATE TYPE [dbo].[T_DBEntity] FROM [varchar](64) NULL
GO
/****** Object:  UserDefinedDataType [dbo].[T_Guid]    Script Date: 2012/4/12 22:28:08 ******/
CREATE TYPE [dbo].[T_Guid] FROM [uniqueidentifier] NULL
GO
/****** Object:  UserDefinedDataType [dbo].[T_Name]    Script Date: 2012/4/12 22:28:08 ******/
CREATE TYPE [dbo].[T_Name] FROM [varchar](64) NULL
GO
USE [Fantasy]
GO
/****** Object:  Sequence [dbo].[BUSINESSMETAMODESEQ]    Script Date: 2012/4/12 22:28:08 ******/
CREATE SEQUENCE [dbo].[BUSINESSMETAMODESEQ] 
 AS [bigint]
 START WITH 1
 INCREMENT BY 1
 MINVALUE -9223372036854775808
 MAXVALUE 9223372036854775807
 CACHE 
GO
/****** Object:  Table [dbo].[AssemblyReference]    Script Date: 2012/4/12 22:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AssemblyReference](
	[ID] [dbo].[T_Guid] NOT NULL,
	[GROUPID] [dbo].[T_Guid] NOT NULL,
	[ModificationTime] [datetime] NOT NULL,
	[CreationTime] [datetime] NOT NULL,
	[IsSystem] [bit] NOT NULL,
	[FullName] [varchar](255) NOT NULL,
	[Source] [int] NOT NULL,
	[RawAssembly] [varbinary](max) NULL,
	[RawHash] [varchar](32) NULL,
 CONSTRAINT [PK_ASSEMBLYREFERENCE] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AssemblyReferenceGroup]    Script Date: 2012/4/12 22:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AssemblyReferenceGroup](
	[ID] [dbo].[T_Guid] NOT NULL,
	[ModificationTime] [datetime] NOT NULL,
	[CreationTime] [datetime] NOT NULL,
	[IsSystem] [bit] NOT NULL,
 CONSTRAINT [PK_ASSEMBLYREFERENCEGROUP] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ASSN_DEPARTMENT_LEADER]    Script Date: 2012/4/12 22:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ASSN_DEPARTMENT_LEADER](
	[LEFTID] [uniqueidentifier] NOT NULL,
	[RIGHTID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ASSN_DEPARTMENT_LEADER] PRIMARY KEY CLUSTERED 
(
	[LEFTID] ASC,
	[RIGHTID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ASSN_DEPARTMENT_STAFF]    Script Date: 2012/4/12 22:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ASSN_DEPARTMENT_STAFF](
	[LEFTID] [uniqueidentifier] NOT NULL,
	[RIGHTID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ASSN_DEPARTMENT_STAFF] PRIMARY KEY CLUSTERED 
(
	[LEFTID] ASC,
	[RIGHTID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BusinessApplication]    Script Date: 2012/4/12 22:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BusinessApplication](
	[ID] [dbo].[T_Guid] NOT NULL,
	[ModificationTime] [datetime] NOT NULL,
	[CreationTime] [datetime] NOT NULL,
	[IsSystem] [bit] NOT NULL,
	[PACKAGEID] [dbo].[T_Guid] NOT NULL,
	[EntryObjectId] [dbo].[T_Guid] NULL,
	[Name] [dbo].[T_Name] NOT NULL,
	[CodeName] [dbo].[T_Name] NOT NULL,
	[Script] [varchar](max) NULL,
	[ScriptOptions] [int] NULL,
	[ExternalType] [varchar](1024) NULL,
 CONSTRAINT [PK_BUSINESSAPPLICATION] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BusinessApplicationACL]    Script Date: 2012/4/12 22:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BusinessApplicationACL](
	[ID] [dbo].[T_Guid] NOT NULL,
	[STATEID] [dbo].[T_Guid] NULL,
	[PARTICIPANTID] [dbo].[T_Guid] NOT NULL,
	[ROLEID] [dbo].[T_Guid] NOT NULL,
	[ModificationTime] [datetime] NOT NULL,
	[CreationTime] [datetime] NOT NULL,
	[IsSystem] [bit] NOT NULL,
	[ACL] [text] NULL,
 CONSTRAINT [PK_BUSINESSAPPLICATIONACL] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BusinessApplicationParticipant]    Script Date: 2012/4/12 22:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BusinessApplicationParticipant](
	[ID] [dbo].[T_Guid] NOT NULL,
	[CLASSID] [dbo].[T_Guid] NOT NULL,
	[APPLICATIONID] [dbo].[T_Guid] NOT NULL,
	[ModificationTime] [datetime] NOT NULL,
	[CreationTime] [datetime] NOT NULL,
	[IsSystem] [bit] NOT NULL,
	[IsEntry] [bit] NOT NULL,
 CONSTRAINT [PK_BUSINESSAPPLICATIONPARTICIP] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BusinessAssociation]    Script Date: 2012/4/12 22:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BusinessAssociation](
	[ID] [dbo].[T_Guid] NOT NULL,
	[PackageID] [dbo].[T_Guid] NOT NULL,
	[LEFTCLASSID] [dbo].[T_Guid] NOT NULL,
	[ModificationTime] [datetime] NOT NULL,
	[CreationTime] [datetime] NOT NULL,
	[IsSystem] [bit] NOT NULL,
	[RightClassID] [dbo].[T_Guid] NOT NULL,
	[Name] [dbo].[T_Name] NOT NULL,
	[CodeName] [dbo].[T_Name] NOT NULL,
	[TableName] [dbo].[T_DBEntity] NOT NULL,
	[TableSchema] [dbo].[T_DBEntity] NOT NULL,
	[TableSpace] [dbo].[T_DBEntity] NULL,
	[LeftRoleName] [dbo].[T_Name] NULL,
	[LeftRoleCode] [dbo].[T_Name] NULL,
	[LeftCardinality] [varchar](16) NOT NULL,
	[LeftNavigatable] [bit] NOT NULL,
	[LeftRoleDisplayOrder] [bigint] NULL,
	[RightRoleName] [dbo].[T_Name] NULL,
	[RightRoleCode] [dbo].[T_Name] NULL,
	[RightCardinality] [varchar](16) NOT NULL,
	[RightNavigatable] [bit] NOT NULL,
	[RightRoleDisplayOrder] [bigint] NULL,
 CONSTRAINT [PK_BUSINESSASSOCIATION] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BusinessClass]    Script Date: 2012/4/12 22:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BusinessClass](
	[ID] [dbo].[T_Guid] NOT NULL,
	[ModificationTime] [datetime] NOT NULL,
	[CreationTime] [datetime] NOT NULL,
	[IsSystem] [bit] NOT NULL,
	[PackageID] [dbo].[T_Guid] NOT NULL,
	[ParentClassID] [dbo].[T_Guid] NULL,
	[Name] [dbo].[T_Name] NOT NULL,
	[CodeName] [dbo].[T_Name] NOT NULL,
	[TableName] [dbo].[T_DBEntity] NOT NULL,
	[TableSchema] [dbo].[T_DBEntity] NOT NULL,
	[TableSpace] [dbo].[T_DBEntity] NULL,
	[IsSimple] [bit] NOT NULL,
	[Implements] [varchar](4096) NULL,
	[AutoScript] [varchar](max) NULL,
	[Script] [varchar](max) NULL,
	[ScriptOptions] [int] NULL,
	[IsAbstract] [bit] NOT NULL,
	[ExternalType] [varchar](1024) NULL,
 CONSTRAINT [PK_BUSINESSCLASS] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BusinessClassDiagram]    Script Date: 2012/4/12 22:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BusinessClassDiagram](
	[ID] [dbo].[T_Guid] NOT NULL,
	[PACKAGEID] [dbo].[T_Guid] NOT NULL,
	[ModificationTime] [datetime] NOT NULL,
	[CreationTime] [datetime] NOT NULL,
	[IsSystem] [bit] NOT NULL,
	[Name] [dbo].[T_Name] NOT NULL,
	[Diagram] [text] NULL,
 CONSTRAINT [PK_BUSINESSCLASSDIAGRAM] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[PACKAGEID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BusinessDataType]    Script Date: 2012/4/12 22:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BusinessDataType](
	[ID] [dbo].[T_Guid] NOT NULL,
	[DefaultDBType] [dbo].[T_Name] NULL,
	[DefaultLength] [int] NULL,
	[DefaultPrecision] [int] NULL,
	[NHType] [dbo].[T_Name] NULL,
	[Name] [dbo].[T_Name] NOT NULL,
	[CodeName] [dbo].[T_Name] NOT NULL,
	[ModificationTime] [datetime] NOT NULL,
	[CreationTime] [datetime] NOT NULL,
	[IsSystem] [bit] NOT NULL,
 CONSTRAINT [PK_BUSINESSDATATYPE] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BusinessEnum]    Script Date: 2012/4/12 22:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BusinessEnum](
	[ID] [dbo].[T_Guid] NOT NULL,
	[ModificationTime] [datetime] NOT NULL,
	[CreationTime] [datetime] NOT NULL,
	[IsSystem] [bit] NOT NULL,
	[PACKAGEID] [dbo].[T_Guid] NOT NULL,
	[Name] [dbo].[T_Name] NOT NULL,
	[CodeName] [dbo].[T_Name] NOT NULL,
	[IsFlags] [bit] NOT NULL,
	[IsExternal] [bit] NULL,
	[ExternalAssembly] [dbo].[T_Name] NULL,
	[ExternalNamespace] [varchar](255) NULL,
 CONSTRAINT [PK_BUSINESSENUM] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BusinessEnumValue]    Script Date: 2012/4/12 22:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BusinessEnumValue](
	[ID] [dbo].[T_Guid] NOT NULL,
	[ModificationTime] [datetime] NOT NULL,
	[CreationTime] [datetime] NOT NULL,
	[IsSystem] [bit] NOT NULL,
	[ENUMID] [dbo].[T_Guid] NOT NULL,
	[Name] [dbo].[T_Name] NOT NULL,
	[CodeName] [dbo].[T_Name] NOT NULL,
	[Value] [int] NOT NULL,
 CONSTRAINT [PK_BUSINESSENUMVALUE] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BusinessExtraScript]    Script Date: 2012/4/12 22:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BusinessExtraScript](
	[ID] [dbo].[T_Guid] NOT NULL,
	[PackageID] [dbo].[T_Guid] NOT NULL,
	[ModificationTime] [datetime] NOT NULL,
	[CreationTime] [datetime] NOT NULL,
	[IsSystem] [bit] NOT NULL,
	[Name] [dbo].[T_Name] NOT NULL,
	[Script] [varchar](max) NULL,
	[MetaData] [varchar](1024) NULL,
 CONSTRAINT [PK_BUSINESSEXTRASCRIPT] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BusinessLastUpdateTimestamp]    Script Date: 2012/4/12 22:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BusinessLastUpdateTimestamp](
	[Name] [dbo].[T_Name] NULL,
	[Seconds] [bigint] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BusinessObject]    Script Date: 2012/4/12 22:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BusinessObject](
	[ID] [dbo].[T_Guid] NOT NULL,
	[CLASSID] [dbo].[T_Guid] NOT NULL,
	[ModificationTime] [datetime] NOT NULL,
	[CreationTime] [datetime] NOT NULL,
	[IsSystem] [bit] NOT NULL,
	[Name] [dbo].[T_Name] NULL,
 CONSTRAINT [PK_BUSINESSOBJECT] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BusinessPackage]    Script Date: 2012/4/12 22:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BusinessPackage](
	[ID] [dbo].[T_Guid] NOT NULL,
	[ParentPackageID] [dbo].[T_Guid] NULL,
	[PackageType] [int] NULL,
	[Name] [dbo].[T_Name] NOT NULL,
	[CodeName] [dbo].[T_Name] NOT NULL,
	[ModificationTime] [datetime] NOT NULL,
	[CreationTime] [datetime] NOT NULL,
	[IsSystem] [bit] NOT NULL,
 CONSTRAINT [PK_BUSINESSPACKAGE] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BusinessProperty]    Script Date: 2012/4/12 22:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BusinessProperty](
	[ID] [dbo].[T_Guid] NOT NULL,
	[CLASSID] [dbo].[T_Guid] NOT NULL,
	[ModificationTime] [datetime] NOT NULL,
	[CreationTime] [datetime] NOT NULL,
	[IsSystem] [bit] NOT NULL,
	[ClassTypeID] [dbo].[T_Guid] NULL,
	[EnumTypeID] [dbo].[T_Guid] NULL,
	[DataTypeID] [dbo].[T_Guid] NULL,
	[Name] [dbo].[T_Name] NOT NULL,
	[CodeName] [dbo].[T_Name] NOT NULL,
	[FieldName] [dbo].[T_DBEntity] NULL,
	[FieldType] [dbo].[T_DBEntity] NULL,
	[Length] [int] NULL,
	[Precision] [int] NULL,
	[IsCalculated] [bit] NOT NULL,
	[IsNullable] [bit] NOT NULL,
	[DefaultValue] [varchar](64) NULL,
	[ScriptOptions] [int] NULL,
	[DisplayOrder] [bigint] NOT NULL,
 CONSTRAINT [PK_BUSINESSPROPERTY] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[CLASSID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BusinessRole]    Script Date: 2012/4/12 22:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BusinessRole](
	[ID] [dbo].[T_Guid] NOT NULL,
	[ModificationTime] [datetime] NOT NULL,
	[CreationTime] [datetime] NOT NULL,
	[IsSystem] [bit] NOT NULL,
	[PACKAGEID] [dbo].[T_Guid] NOT NULL,
	[Name] [dbo].[T_Name] NOT NULL,
	[CodeName] [dbo].[T_Name] NOT NULL,
	[Description] [varchar](1024) NULL,
	[IsComputed] [bit] NOT NULL,
	[IsDisabled] [bit] NULL,
	[Script] [varchar](max) NULL,
	[ScriptOptions] [int] NULL,
	[ExternalType] [varchar](1024) NULL,
 CONSTRAINT [PK_BUSINESSROLE] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BusinessUser]    Script Date: 2012/4/12 22:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BusinessUser](
	[ID] [dbo].[T_Guid] NOT NULL,
	[ModificationTime] [datetime] NOT NULL,
	[CreationTime] [datetime] NOT NULL,
	[IsSystem] [bit] NOT NULL,
	[PACKAGEID] [dbo].[T_Guid] NOT NULL,
	[Name] [dbo].[T_Name] NOT NULL,
	[CodeName] [dbo].[T_Name] NOT NULL,
	[FullName] [dbo].[T_Name] NOT NULL,
	[Description] [varchar](1024) NULL,
	[Password] [varchar](128) NULL,
	[IsDisabled] [bit] NOT NULL,
	[Script] [varchar](max) NULL,
	[ScriptOptions] [int] NULL,
	[ExternalType] [varchar](1024) NULL,
 CONSTRAINT [PK_BUSINESSUSER] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BusinessUserRole]    Script Date: 2012/4/12 22:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BusinessUserRole](
	[USERID] [dbo].[T_Guid] NOT NULL,
	[ROLEID] [dbo].[T_Guid] NOT NULL,
 CONSTRAINT [PK_BUSINESSUSERROLE] PRIMARY KEY CLUSTERED 
(
	[USERID] ASC,
	[ROLEID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CLS_DEPARTMENT]    Script Date: 2012/4/12 22:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CLS_DEPARTMENT](
	[Id] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_CLS_DEPARTMENT] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CLS_HUMAN]    Script Date: 2012/4/12 22:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CLS_HUMAN](
	[Id] [uniqueidentifier] NOT NULL,
	[AGE] [numeric](18, 0) NULL,
	[GENDER] [numeric](18, 0) NULL,
 CONSTRAINT [PK_CLS_HUMAN] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'7a9d373b-e5e0-4a79-9461-00fbeeac66bf', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C146E8 AS DateTime), 0, N'System.Web.Helpers, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35', 2, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'743c4ddf-d900-4c55-bbf9-01ed6d07e1d4', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C13EB4 AS DateTime), 0, N'Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a', 1, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'fe2884ce-31da-418b-9dc7-01fe8972dfbf', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C13FE0 AS DateTime), 0, N'System.Data.Entity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089', 1, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'a3564cd5-a263-4167-8955-063020772c68', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C14238 AS DateTime), 0, N'System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35', 1, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'3b47e10a-f423-44f0-8eac-0cdd65c33d78', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C14364 AS DateTime), 0, N'System.Xml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089', 1, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'12594465-dd93-4dcb-897c-0db7f7f576b8', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C14364 AS DateTime), 0, N'Fantasy.Base, Version=1.0.0.125, Culture=neutral, PublicKeyToken=null', 2, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'193c85fa-6430-4a99-8a7c-10101b21f62a', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C14364 AS DateTime), 0, N'Accessibility, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a', 1, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'88232d3b-1425-4793-912a-1b0e671af0b4', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C14238 AS DateTime), 0, N'System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089', 1, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'3d861c4e-199b-4cc0-a246-204ee58c9e2b', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C13FE0 AS DateTime), 0, N'System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a', 1, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'ba8c4b4a-0a18-4dbb-9760-2764d20ab9ae', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C145BC AS DateTime), 0, N'System.Json, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35', 2, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'6e84a339-9abb-42cf-a953-32ab3e9d7350', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C14238 AS DateTime), 0, N'System.Web.ApplicationServices, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35', 1, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'ae776c9e-939f-4171-bad8-37a09e2ffe28', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C14490 AS DateTime), 0, N'Microsoft.WindowsAPICodePack, Version=1.1.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35', 2, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'5d240cbf-aa8c-4da7-b929-383cc4c037ce', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C13FE0 AS DateTime), 0, N'System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089', 1, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'3b92c84b-c870-43de-9954-3b290550f2d5', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C146E8 AS DateTime), 0, N'System.Web.Optimization, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35', 2, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'97b678c3-0148-486a-98b5-40d5f1e9f245', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C146E8 AS DateTime), 0, N'System.Web.Mvc, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35', 2, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'899dbb48-05e0-47a1-8383-49bf5da8554e', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C14364 AS DateTime), 0, N'System.Configuration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a', 1, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'fc72f1cc-830b-4ea3-bbce-4dd25f9e32fd', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C14238 AS DateTime), 0, N'System.Xml.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089', 1, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'f801c25e-0ffc-46b7-bfca-54b2f3c1386a', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C14490 AS DateTime), 0, N'FluentNHibernate, Version=1.2.0.712, Culture=neutral, PublicKeyToken=8aa435e3cb308880', 2, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'ec34fc57-1558-4f5e-9dac-69476f9874df', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C14490 AS DateTime), 0, N'Iesi.Collections, Version=1.0.1.0, Culture=neutral, PublicKeyToken=aa95f207798dfdb4', 2, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'5dee904f-c623-44b2-9326-713ae69c790c', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C145BC AS DateTime), 0, N'System.Net.Http.WebRequest, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a', 2, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'b6352afc-5b3e-4171-9a25-71e652f47700', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C14238 AS DateTime), 0, N'System.Data.DataSetExtensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089', 1, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'7ae3bc52-00e9-47ac-b459-7ed03af15a6f', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C13FE0 AS DateTime), 0, N'System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089', 1, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'9a1ec627-5088-443d-9e54-877d49b7aa4e', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C14364 AS DateTime), 0, N'System.Web.Services, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a', 1, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'297d804d-defd-4dd0-b088-8cbd21eb8783', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C14238 AS DateTime), 0, N'System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a', 1, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'2cfdbad0-d83a-4bb4-a2fe-8ff475a4ffe4', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C14238 AS DateTime), 0, N'System.Web.Entity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089', 1, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'fca33e8a-494d-4bdd-8052-978d72c1476b', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C146E8 AS DateTime), 0, N'System.Web.Http.Common, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35', 2, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'a7c63a54-1fb8-43fc-9c2a-97ff2b4bf19d', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C14364 AS DateTime), 0, N'System.EnterpriseServices, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a', 1, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'8b326ed0-159d-45a6-8755-99d0a3e5fefa', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C14814 AS DateTime), 0, N'System.Web.Razor, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35', 2, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'90be7f54-72e1-4011-a525-9bdc6bd9946e', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C145BC AS DateTime), 0, N'System.Net.Http.Formatting, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35', 2, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'b43d491e-1fb9-4bc6-8804-a2b9b3a18171', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C145BC AS DateTime), 0, N'NHibernate, Version=3.1.0.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4', 2, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'036493b9-1946-4b3b-a237-a5fabbb1ea39', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C14814 AS DateTime), 0, N'System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35', 2, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'0cf05f70-3963-4e10-8eee-adb1beb77f91', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C146E8 AS DateTime), 0, N'System.Web.Http, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35', 2, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'1a6736b3-b612-4a27-bebb-ae576cb77795', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C14490 AS DateTime), 0, N'Microsoft.Web.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35', 2, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'362cf0ae-ec2f-4f8a-9b05-b3aceb9e63f4', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C14490 AS DateTime), 0, N'Microsoft.WindowsAPICodePack.Shell, Version=1.1.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35', 2, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'7d825be6-b3dc-4fe9-8fa3-c3c1ee8c8941', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C1410C AS DateTime), 0, N'System.Web.DynamicData, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35', 1, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'108674af-c5c1-41c5-837e-cd0a533da4fa', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C14814 AS DateTime), 0, N'System.Web.WebPages.Deployment, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35', 2, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'db988bd5-f2a7-4f60-96c8-d002907f0504', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C14814 AS DateTime), 0, N'System.Web.Providers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35', 2, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'1f24df2d-2ba1-4be1-86ba-d61b4ff6973f', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C145BC AS DateTime), 0, N'System.Net.Http, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a', 2, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'6e7fd1cd-aa1c-490a-9783-e019b56c1052', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C146E8 AS DateTime), 0, N'System.Web.Http.WebHost, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35', 2, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'abeae453-2640-428e-b398-e159c2c30323', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C14490 AS DateTime), 0, N'Fantasy.BusinessEngine, Version=1.0.0.125, Culture=neutral, PublicKeyToken=null', 2, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'82d4dd06-09dc-4025-81b9-e73806c51998', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C14238 AS DateTime), 0, N'System.Web.Abstractions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35', 1, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'37abf9e8-cf41-4160-9c00-eca4f3e52e0b', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C14814 AS DateTime), 0, N'System.Web.WebPages.Razor, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35', 2, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'52825ce6-2a39-4ad2-980c-ed9e25c7197a', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C14238 AS DateTime), 0, N'System.ComponentModel.DataAnnotations, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35', 1, NULL, NULL)
INSERT [dbo].[AssemblyReference] ([ID], [GROUPID], [ModificationTime], [CreationTime], [IsSystem], [FullName], [Source], [RawAssembly], [RawHash]) VALUES (N'38c34e2c-029f-495f-84fc-ff4ed590ff18', N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02800C14364 AS DateTime), 0, N'System.Web.Routing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35', 1, NULL, NULL)
INSERT [dbo].[AssemblyReferenceGroup] ([ID], [ModificationTime], [CreationTime], [IsSystem]) VALUES (N'90390416-a147-4d0a-aa59-837bdb4a5228', CAST(0x00009F5601617BC5 AS DateTime), CAST(0x00009F5601617BC5 AS DateTime), 1)
INSERT [dbo].[BusinessApplication] ([ID], [ModificationTime], [CreationTime], [IsSystem], [PACKAGEID], [EntryObjectId], [Name], [CodeName], [Script], [ScriptOptions], [ExternalType]) VALUES (N'bea7575d-3ff2-4e99-94b1-6691b516d503', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A031016385AC AS DateTime), 1, N'09196be5-c015-46ba-84b5-fd4811352ffd', N'9ffb15da-dcac-400f-a188-afb0bd8c16c2', N'AdministrativeApplication', N'AdministrativeApplication', NULL, 2, N'Fantasy.BusinessEngine.Application.AdministrativeApplication, Fantasy.BusinessEngine')
INSERT [dbo].[BusinessApplication] ([ID], [ModificationTime], [CreationTime], [IsSystem], [PACKAGEID], [EntryObjectId], [Name], [CodeName], [Script], [ScriptOptions], [ExternalType]) VALUES (N'ddbac066-d1b4-4a7d-8366-d21bf974280c', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A03101695E28 AS DateTime), 0, N'1bb4fafe-794a-43a1-aa70-bca32d59a64f', NULL, N'OgnizationApplication', N'OgnizationApplication', N'using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy;
using Fantasy.BusinessEngine;
namespace Fantasy.Ognization
{
	public partial class OgnizationApplication : Fantasy.BusinessEngine.BusinessApplication
	{
	}
}
', 0, NULL)
INSERT [dbo].[BusinessApplicationACL] ([ID], [STATEID], [PARTICIPANTID], [ROLEID], [ModificationTime], [CreationTime], [IsSystem], [ACL]) VALUES (N'52af8724-c549-42ec-a88e-136108f18900', NULL, N'0793d949-86dd-4ceb-b1c5-eafcfedf6d6d', N'0dab7123-f09f-4e04-9ea4-4dcfa8dfa040', CAST(0x0000A031016DA000 AS DateTime), CAST(0x0000A031016D75D0 AS DateTime), 0, N'<object canCreate="True" canDelete="True" xmlns="urn:schema-fantasy:business-engine-security">
  <property id="c9b092be-fce4-4793-9bba-9f3300ac9427" name="Id" canRead="True" type="Property" order="6" />
  <property id="e5434005-55e8-482f-a46a-9f3300ac942f" name="Name" canRead="True" type="Property" order="7" />
  <property id="57b4a057-35b6-4e09-89f4-9f3300ac942f" name="Class" canRead="True" type="Property" order="8" />
  <property id="0be3b780-5b3d-4840-8aaa-9f3300ac942f" name="Creation Time" canRead="True" type="Property" order="9" />
  <property id="4ed247a4-7e5a-4b2d-9444-9f3300ac942f" name="Modification Time" canRead="True" type="Property" order="10" />
  <property id="9171981d-4d61-499d-b8a9-9f3300ac942f" name="Is System" canRead="True" type="Property" order="11" />
  <property id="23cd771b-980b-4477-8db9-dd5160b51ac8" name="Age" canRead="True" type="Property" order="258" />
  <property id="facc2bf7-899c-4ff8-b88e-68032dfebcab" name="Gender" canRead="True" type="Property" order="259" />
  <property id="050703bb-8931-42f9-9193-75eb04c76981" name="Department" canRead="True" type="RightAssociation" order="261" />
  <property id="0dc5842e-9b59-43ea-9a39-2ba7c8ed89fc" name="LeadingDepartment" canRead="True" type="RightAssociation" order="263" />
</object>')
INSERT [dbo].[BusinessApplicationACL] ([ID], [STATEID], [PARTICIPANTID], [ROLEID], [ModificationTime], [CreationTime], [IsSystem], [ACL]) VALUES (N'c52045dc-1c46-4613-ac4d-5bbaad5fd742', NULL, N'5ba3a341-6929-46cb-be31-97913c763704', N'0dab7123-f09f-4e04-9ea4-4dcfa8dfa040', CAST(0x0000A031016D68EC AS DateTime), CAST(0x0000A031016CF164 AS DateTime), 0, N'<object xmlns="urn:schema-fantasy:business-engine-security">
  <property id="c9b092be-fce4-4793-9bba-9f3300ac9427" name="Id" canRead="True" type="Property" order="6" />
  <property id="e5434005-55e8-482f-a46a-9f3300ac942f" name="Name" canRead="True" type="Property" order="7" />
  <property id="57b4a057-35b6-4e09-89f4-9f3300ac942f" name="Class" canRead="True" type="Property" order="8" />
  <property id="0be3b780-5b3d-4840-8aaa-9f3300ac942f" name="Creation Time" canRead="True" type="Property" order="9" />
  <property id="4ed247a4-7e5a-4b2d-9444-9f3300ac942f" name="Modification Time" canRead="True" type="Property" order="10" />
  <property id="9171981d-4d61-499d-b8a9-9f3300ac942f" name="Is System" canRead="True" type="Property" order="11" />
  <property id="050703bb-8931-42f9-9193-75eb04c76981" name="Staffs" canRead="True" type="LeftAssociation" order="261" />
  <property id="0dc5842e-9b59-43ea-9a39-2ba7c8ed89fc" name="Leader" canRead="True" type="LeftAssociation" order="263" />
</object>')
INSERT [dbo].[BusinessApplicationACL] ([ID], [STATEID], [PARTICIPANTID], [ROLEID], [ModificationTime], [CreationTime], [IsSystem], [ACL]) VALUES (N'b9d85258-57ce-4021-9d83-ba7922388da3', NULL, N'5ba3a341-6929-46cb-be31-97913c763704', N'966f50ab-1672-4e74-80ce-cdf950990eb8', CAST(0x0000A031016D68EC AS DateTime), CAST(0x0000A031016CB474 AS DateTime), 0, N'<object canCreate="True" canDelete="True" xmlns="urn:schema-fantasy:business-engine-security">
  <property id="c9b092be-fce4-4793-9bba-9f3300ac9427" name="Id" canRead="True" type="Property" order="6" />
  <property id="e5434005-55e8-482f-a46a-9f3300ac942f" name="Name" canRead="True" type="Property" order="7" />
  <property id="57b4a057-35b6-4e09-89f4-9f3300ac942f" name="Class" canRead="True" type="Property" order="8" />
  <property id="0be3b780-5b3d-4840-8aaa-9f3300ac942f" name="Creation Time" canRead="True" type="Property" order="9" />
  <property id="4ed247a4-7e5a-4b2d-9444-9f3300ac942f" name="Modification Time" canRead="True" type="Property" order="10" />
  <property id="9171981d-4d61-499d-b8a9-9f3300ac942f" name="Is System" canRead="True" type="Property" order="11" />
  <property id="050703bb-8931-42f9-9193-75eb04c76981" name="Staffs" canRead="True" type="LeftAssociation" order="261" />
  <property id="0dc5842e-9b59-43ea-9a39-2ba7c8ed89fc" name="Leader" canRead="True" type="LeftAssociation" order="263" />
</object>')
INSERT [dbo].[BusinessApplicationParticipant] ([ID], [CLASSID], [APPLICATIONID], [ModificationTime], [CreationTime], [IsSystem], [IsEntry]) VALUES (N'5ba3a341-6929-46cb-be31-97913c763704', N'6bff5379-1fe8-4591-a8e4-0cdae30cd019', N'ddbac066-d1b4-4a7d-8366-d21bf974280c', CAST(0x0000A031016CA1B4 AS DateTime), CAST(0x0000A031016C9980 AS DateTime), 0, 1)
INSERT [dbo].[BusinessApplicationParticipant] ([ID], [CLASSID], [APPLICATIONID], [ModificationTime], [CreationTime], [IsSystem], [IsEntry]) VALUES (N'0793d949-86dd-4ceb-b1c5-eafcfedf6d6d', N'b523d059-25a3-45da-b1ab-0ef4b39828b6', N'ddbac066-d1b4-4a7d-8366-d21bf974280c', CAST(0x0000A031016CA1B4 AS DateTime), CAST(0x0000A031016C9BD8 AS DateTime), 0, 0)
INSERT [dbo].[BusinessAssociation] ([ID], [PackageID], [LEFTCLASSID], [ModificationTime], [CreationTime], [IsSystem], [RightClassID], [Name], [CodeName], [TableName], [TableSchema], [TableSpace], [LeftRoleName], [LeftRoleCode], [LeftCardinality], [LeftNavigatable], [LeftRoleDisplayOrder], [RightRoleName], [RightRoleCode], [RightCardinality], [RightNavigatable], [RightRoleDisplayOrder]) VALUES (N'0dc5842e-9b59-43ea-9a39-2ba7c8ed89fc', N'1bb4fafe-794a-43a1-aa70-bca32d59a64f', N'6bff5379-1fe8-4591-a8e4-0cdae30cd019', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A031016BB754 AS DateTime), 0, N'b523d059-25a3-45da-b1ab-0ef4b39828b6', N'Department_Leader', N'Department_Leader', N'ASSN_DEPARTMENT_LEADER', N'dbo', NULL, N'LeadingDepartment', N'LeadingDepartment', N'0..1', 0, 262, N'Leader', N'Leader', N'0..1', 1, 263)
INSERT [dbo].[BusinessAssociation] ([ID], [PackageID], [LEFTCLASSID], [ModificationTime], [CreationTime], [IsSystem], [RightClassID], [Name], [CodeName], [TableName], [TableSchema], [TableSpace], [LeftRoleName], [LeftRoleCode], [LeftCardinality], [LeftNavigatable], [LeftRoleDisplayOrder], [RightRoleName], [RightRoleCode], [RightCardinality], [RightNavigatable], [RightRoleDisplayOrder]) VALUES (N'050703bb-8931-42f9-9193-75eb04c76981', N'1bb4fafe-794a-43a1-aa70-bca32d59a64f', N'6bff5379-1fe8-4591-a8e4-0cdae30cd019', CAST(0x0000A031016AB200 AS DateTime), CAST(0x0000A031016A64A8 AS DateTime), 0, N'b523d059-25a3-45da-b1ab-0ef4b39828b6', N'Department_Staff', N'Department_Staff', N'ASSN_DEPARTMENT_STAFF', N'dbo', NULL, N'Department', N'Department', N'0..1', 1, 260, N'Staffs', N'Staffs', N'*', 1, 261)
INSERT [dbo].[BusinessClass] ([ID], [ModificationTime], [CreationTime], [IsSystem], [PackageID], [ParentClassID], [Name], [CodeName], [TableName], [TableSchema], [TableSpace], [IsSimple], [Implements], [AutoScript], [Script], [ScriptOptions], [IsAbstract], [ExternalType]) VALUES (N'6bff5379-1fe8-4591-a8e4-0cdae30cd019', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A0310169B60C AS DateTime), 0, N'1bb4fafe-794a-43a1-aa70-bca32d59a64f', N'bf0aa7f4-588f-4556-963d-33242e649d57', N'Department', N'Department', N'CLS_DEPARTMENT', N'dbo', NULL, 0, NULL, N'//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Collections;


namespace Fantasy.Ognization
{
    public partial class Department : Fantasy.BusinessEngine.BusinessObject
	{
        public virtual BusinessObjectCollection<Fantasy.Ognization.Human> Staffs
        {
            get
            {
               return GetCollection<Fantasy.Ognization.Human>("Staffs");
            }
        }
 
		public virtual Fantasy.Ognization.Human Leader
		{
		    get
            {
                return this.GetManyToOneValue<Fantasy.Ognization.Human>("Leader");
            }
            set
            {
			    Fantasy.Ognization.Human old = this.Leader;
			    if(!Object.Equals(old, value))
				{
				    EntityPropertyChangingEventArgs e1 = new EntityPropertyChangingEventArgs(this, "Leader", value, old);
				    this.OnLeaderChanging(e1);
					if(!e1.Cancel)
					{
                        if(this.SetManyToOneValue("Leader", value))
						{
					        this.OnLeaderChanged(new EntityPropertyChangedEventArgs(this, "Leader", value, old));
						}
					}
				}
            }
		}
		
		partial void OnLeaderChanged(EntityPropertyChangedEventArgs e);
		
		partial void OnLeaderChanging(EntityPropertyChangingEventArgs e);
	

	}

}

', N'using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy;
using Fantasy.BusinessEngine;
namespace Fantasy.Ognization
{
	partial class Department
	{
	}
}
', 0, 0, NULL)
INSERT [dbo].[BusinessClass] ([ID], [ModificationTime], [CreationTime], [IsSystem], [PackageID], [ParentClassID], [Name], [CodeName], [TableName], [TableSchema], [TableSpace], [IsSimple], [Implements], [AutoScript], [Script], [ScriptOptions], [IsAbstract], [ExternalType]) VALUES (N'b523d059-25a3-45da-b1ab-0ef4b39828b6', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A0310169CB24 AS DateTime), 0, N'1bb4fafe-794a-43a1-aa70-bca32d59a64f', N'bf0aa7f4-588f-4556-963d-33242e649d57', N'Human', N'Human', N'CLS_HUMAN', N'dbo', NULL, 0, NULL, N'//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Collections;


namespace Fantasy.Ognization
{
    public partial class Human : Fantasy.BusinessEngine.BusinessObject
	{
  
		
		public virtual int? Age
		{
		    get
            {
                return (int?)this.GetValue("Age",default(int?));
            }
            set
            {
			    int? old = this.Age;
			    if(!Object.Equals(old, value))
				{
				    EntityPropertyChangingEventArgs e1 = new EntityPropertyChangingEventArgs(this, "Age", value, old);
				    this.OnAgeChanging(e1);
					if(!e1.Cancel)
					{
                        if(this.SetValue("Age", value))
						{
					        this.OnAgeChanged(new EntityPropertyChangedEventArgs(this, "Age", value, old));
						}
					}
				}
            }
		}
		
		partial void OnAgeChanged(EntityPropertyChangedEventArgs e);
		
		partial void OnAgeChanging(EntityPropertyChangingEventArgs e);
		
  
		
		public virtual Fantasy.Ognization.Gender? Gender
		{
		    get
            {
                return (Fantasy.Ognization.Gender?)this.GetValue("Gender",default(Fantasy.Ognization.Gender?));
            }
            set
            {
			    Fantasy.Ognization.Gender? old = this.Gender;
			    if(!Object.Equals(old, value))
				{
				    EntityPropertyChangingEventArgs e1 = new EntityPropertyChangingEventArgs(this, "Gender", value, old);
				    this.OnGenderChanging(e1);
					if(!e1.Cancel)
					{
                        if(this.SetValue("Gender", value))
						{
					        this.OnGenderChanged(new EntityPropertyChangedEventArgs(this, "Gender", value, old));
						}
					}
				}
            }
		}
		
		partial void OnGenderChanged(EntityPropertyChangedEventArgs e);
		
		partial void OnGenderChanging(EntityPropertyChangingEventArgs e);
		
 
		public virtual Fantasy.Ognization.Department Department
		{
		    get
            {
                return this.GetManyToOneValue<Fantasy.Ognization.Department>("Department");
            }
            set
            {
			    Fantasy.Ognization.Department old = this.Department;
			    if(!Object.Equals(old, value))
				{
				    EntityPropertyChangingEventArgs e1 = new EntityPropertyChangingEventArgs(this, "Department", value, old);
				    this.OnDepartmentChanging(e1);
					if(!e1.Cancel)
					{
                        if(this.SetManyToOneValue("Department", value))
						{
					        this.OnDepartmentChanged(new EntityPropertyChangedEventArgs(this, "Department", value, old));
						}
					}
				}
            }
		}
		
		partial void OnDepartmentChanged(EntityPropertyChangedEventArgs e);
		
		partial void OnDepartmentChanging(EntityPropertyChangingEventArgs e);
	

	}

}

', N'using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy;
using Fantasy.BusinessEngine;
namespace Fantasy.Ognization
{
	partial class Human
	{
	}
}
', 0, 0, NULL)
INSERT [dbo].[BusinessClass] ([ID], [ModificationTime], [CreationTime], [IsSystem], [PackageID], [ParentClassID], [Name], [CodeName], [TableName], [TableSchema], [TableSpace], [IsSimple], [Implements], [AutoScript], [Script], [ScriptOptions], [IsAbstract], [ExternalType]) VALUES (N'bf0aa7f4-588f-4556-963d-33242e649d57', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x00009EDA00000000 AS DateTime), 1, N'bdf7bbb0-0991-4f13-ad22-d45bfec45b66', NULL, N'BusinessObject', N'BusinessObject', N'BusinessObject', N'dbo', NULL, 0, NULL, NULL, N'', 2, 0, N'Fantasy.BusinessEngine.BusinessObject, Fantasy.BusinessEngine')
INSERT [dbo].[BusinessClassDiagram] ([ID], [PACKAGEID], [ModificationTime], [CreationTime], [IsSystem], [Name], [Diagram]) VALUES (N'152a9ffd-df38-4002-baf5-e95a6fabca09', N'1bb4fafe-794a-43a1-aa70-bca32d59a64f', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A0310169A7FC AS DateTime), 0, N'Ognization Class Diagram', N'<diagram xmlns="urn:schema-fantasy:classdiagram">
  <classes>
    <class id="246dd0f7-d38b-4f66-935c-353309a73a5b" left="110" top="316.04" width="180" class="6bff5379-1fe8-4591-a8e4-0cdae30cd019" showMember="True" showInherited="False" />
    <class id="0c36e010-bb8c-48e7-aaac-3840998594dd" left="552" top="72.0399999999999" width="180" class="b523d059-25a3-45da-b1ab-0ef4b39828b6" showMember="True" showInherited="False" />
  </classes>
  <enums>
    <enum id="ac14ff44-9595-4790-8130-68303cd1a745" left="529" top="395.04" width="180" enum="0108ad08-bc64-43fc-8dd5-ba59213e9f4b" showMember="True" />
  </enums>
  <inheritances />
  <associations>
    <association left="246dd0f7-d38b-4f66-935c-353309a73a5b" right="0c36e010-bb8c-48e7-aaac-3840998594dd" association="050703bb-8931-42f9-9193-75eb04c76981">
      <point x="200" y="286.040008544922" />
      <point x="642" y="286.040008544922" />
    </association>
    <association left="246dd0f7-d38b-4f66-935c-353309a73a5b" right="0c36e010-bb8c-48e7-aaac-3840998594dd" association="0dc5842e-9b59-43ea-9a39-2ba7c8ed89fc">
      <point x="33.0000152587891" y="373.540008544922" />
      <point x="33.0000152587891" y="67.04" />
      <point x="263" y="67.04" />
      <point x="263" y="149.540008544922" />
    </association>
  </associations>
</diagram>')
INSERT [dbo].[BusinessDataType] ([ID], [DefaultDBType], [DefaultLength], [DefaultPrecision], [NHType], [Name], [CodeName], [ModificationTime], [CreationTime], [IsSystem]) VALUES (N'0ff5fd67-2cad-48ed-bdc3-042b5dd321a1', N'image', 0, 0, N'BinaryBlob', N'Bitmap', N'System.Drawing.Bitmap', CAST(0x00009F2E00B581AB AS DateTime), CAST(0x00009F2E00B581AB AS DateTime), 1)
INSERT [dbo].[BusinessDataType] ([ID], [DefaultDBType], [DefaultLength], [DefaultPrecision], [NHType], [Name], [CodeName], [ModificationTime], [CreationTime], [IsSystem]) VALUES (N'a2b55595-6959-4ec7-8daf-23d2ba6b2c99', N'uniqueidentifier', 0, 0, N'Guid', N'Guid', N'Guid', CAST(0x00009F2E00AF6CF8 AS DateTime), CAST(0x00009F2E00AF6CF8 AS DateTime), 1)
INSERT [dbo].[BusinessDataType] ([ID], [DefaultDBType], [DefaultLength], [DefaultPrecision], [NHType], [Name], [CodeName], [ModificationTime], [CreationTime], [IsSystem]) VALUES (N'b9ceb0d0-ef32-44ff-8cdc-3b609a65d27f', N'bit', 0, 0, N'Boolean', N'Boolean', N'bool', CAST(0x00009F2E00AB96FA AS DateTime), CAST(0x00009F2E00AB96FA AS DateTime), 1)
INSERT [dbo].[BusinessDataType] ([ID], [DefaultDBType], [DefaultLength], [DefaultPrecision], [NHType], [Name], [CodeName], [ModificationTime], [CreationTime], [IsSystem]) VALUES (N'24473090-539e-4c13-be25-46e6f0dd9051', N'uniqueidentifier', 0, 0, N'Guid', N'BusinessClass', N'BusinessClass', CAST(0x00009F2E00B1DA46 AS DateTime), CAST(0x00009F2E00B1DA46 AS DateTime), 1)
INSERT [dbo].[BusinessDataType] ([ID], [DefaultDBType], [DefaultLength], [DefaultPrecision], [NHType], [Name], [CodeName], [ModificationTime], [CreationTime], [IsSystem]) VALUES (N'1110c74f-5716-4a2a-8787-49fb2bff75f3', N'date', 0, 0, N'DateTime', N'Date', N'DateTime', CAST(0x00009F2E00B06EB3 AS DateTime), CAST(0x00009F2E00B06EB3 AS DateTime), 1)
INSERT [dbo].[BusinessDataType] ([ID], [DefaultDBType], [DefaultLength], [DefaultPrecision], [NHType], [Name], [CodeName], [ModificationTime], [CreationTime], [IsSystem]) VALUES (N'adf5890d-3bbd-4375-ad11-4e0594c5e776', N'float', 24, 0, N'Single', N'Float', N'float', CAST(0x00009F2E00ADC568 AS DateTime), CAST(0x00009F2E00ADC568 AS DateTime), 1)
INSERT [dbo].[BusinessDataType] ([ID], [DefaultDBType], [DefaultLength], [DefaultPrecision], [NHType], [Name], [CodeName], [ModificationTime], [CreationTime], [IsSystem]) VALUES (N'192d0683-3c32-44b2-bf6f-51cb88e98445', N'varbinary', 0, 0, N'BinaryBlob', N'Binary', N'byte[]', CAST(0x00009F2E00B15834 AS DateTime), CAST(0x00009F2E00B15834 AS DateTime), 1)
INSERT [dbo].[BusinessDataType] ([ID], [DefaultDBType], [DefaultLength], [DefaultPrecision], [NHType], [Name], [CodeName], [ModificationTime], [CreationTime], [IsSystem]) VALUES (N'5d09014d-c22c-4daa-9657-5c92a4081c36', N'datetimeoffset', 0, 0, N'DateTimeOffset', N'DateTimeOffset', N'DateTimeOffset', CAST(0x00009F32011DC2A4 AS DateTime), CAST(0x00009F32011DC2A4 AS DateTime), 1)
INSERT [dbo].[BusinessDataType] ([ID], [DefaultDBType], [DefaultLength], [DefaultPrecision], [NHType], [Name], [CodeName], [ModificationTime], [CreationTime], [IsSystem]) VALUES (N'1e12aa32-dd17-4f37-8d4c-5ffe9b53dfe1', N'nvarchar', 255, 0, N'String', N'String', N'string', CAST(0x00009F2E00A996C5 AS DateTime), CAST(0x00009F2E00A996C5 AS DateTime), 1)
INSERT [dbo].[BusinessDataType] ([ID], [DefaultDBType], [DefaultLength], [DefaultPrecision], [NHType], [Name], [CodeName], [ModificationTime], [CreationTime], [IsSystem]) VALUES (N'89d45b78-72ce-4a13-8aa9-80fa0da4a4c3', N'time', 0, 0, N'DateTime', N'Time', N'DateTime', CAST(0x00009F2E00B0815E AS DateTime), CAST(0x00009F2E00B0815E AS DateTime), 1)
INSERT [dbo].[BusinessDataType] ([ID], [DefaultDBType], [DefaultLength], [DefaultPrecision], [NHType], [Name], [CodeName], [ModificationTime], [CreationTime], [IsSystem]) VALUES (N'a2883f92-473f-4a07-8d96-85e3fcf31588', N'datetime2', 0, 0, N'DateTime', N'DateTime', N'DateTime', CAST(0x00009F2E00B04E76 AS DateTime), CAST(0x00009F2E00B04E76 AS DateTime), 1)
INSERT [dbo].[BusinessDataType] ([ID], [DefaultDBType], [DefaultLength], [DefaultPrecision], [NHType], [Name], [CodeName], [ModificationTime], [CreationTime], [IsSystem]) VALUES (N'5baf8a91-4204-420c-b888-8bd26e9289bf', N'float', 0, 0, N'Double', N'Double', N'double', CAST(0x00009F2E00AE5044 AS DateTime), CAST(0x00009F2E00AE5044 AS DateTime), 1)
INSERT [dbo].[BusinessDataType] ([ID], [DefaultDBType], [DefaultLength], [DefaultPrecision], [NHType], [Name], [CodeName], [ModificationTime], [CreationTime], [IsSystem]) VALUES (N'f1e72c1d-2432-4da6-82d6-aa2ddeda84ed', N'int', 0, 0, N'PersistedEnum', N'Enumeration', N'enum', CAST(0x00009F2E00B1A045 AS DateTime), CAST(0x00009F2E00B1A045 AS DateTime), 1)
INSERT [dbo].[BusinessDataType] ([ID], [DefaultDBType], [DefaultLength], [DefaultPrecision], [NHType], [Name], [CodeName], [ModificationTime], [CreationTime], [IsSystem]) VALUES (N'ac55ad0f-eb2e-4082-9404-ae145cf8ff15', N'tinyint', 0, 0, N'Byte', N'Byte', N'byte', CAST(0x00009F32011D6E58 AS DateTime), CAST(0x00009F32011D6E58 AS DateTime), 1)
INSERT [dbo].[BusinessDataType] ([ID], [DefaultDBType], [DefaultLength], [DefaultPrecision], [NHType], [Name], [CodeName], [ModificationTime], [CreationTime], [IsSystem]) VALUES (N'd5b4d680-c2ec-4294-982d-b1bf3b89b81e', N'numberic', 18, 0, N'Int64', N'Long', N'long', CAST(0x00009F2E00AC761F AS DateTime), CAST(0x00009F2E00AC761F AS DateTime), 1)
INSERT [dbo].[BusinessDataType] ([ID], [DefaultDBType], [DefaultLength], [DefaultPrecision], [NHType], [Name], [CodeName], [ModificationTime], [CreationTime], [IsSystem]) VALUES (N'1d101c89-cd94-4203-a571-b6c3ddcff4ec', N'numberic', 0, 0, N'Int64', N'ULong', N'ulong', CAST(0x00009F32011F3348 AS DateTime), CAST(0x00009F32011F3348 AS DateTime), 1)
INSERT [dbo].[BusinessDataType] ([ID], [DefaultDBType], [DefaultLength], [DefaultPrecision], [NHType], [Name], [CodeName], [ModificationTime], [CreationTime], [IsSystem]) VALUES (N'44288a16-267e-4fda-a588-b82fba83bb13', N'numeric', 18, 0, N'Int32', N'Integer', N'int', CAST(0x00009F2E00AAA3EB AS DateTime), CAST(0x00009F2E00AAA3EB AS DateTime), 1)
INSERT [dbo].[BusinessDataType] ([ID], [DefaultDBType], [DefaultLength], [DefaultPrecision], [NHType], [Name], [CodeName], [ModificationTime], [CreationTime], [IsSystem]) VALUES (N'1bd22188-8558-432f-9c8d-c01b748a2b65', N'int', 0, 0, N'int16', N'UShort', N'ushort', CAST(0x00009F32011EB552 AS DateTime), CAST(0x00009F32011EB552 AS DateTime), 1)
INSERT [dbo].[BusinessDataType] ([ID], [DefaultDBType], [DefaultLength], [DefaultPrecision], [NHType], [Name], [CodeName], [ModificationTime], [CreationTime], [IsSystem]) VALUES (N'2d3b89de-6ced-4421-9216-d311ea4fbbee', N'numberic', 18, 0, N'int32', N'UInt', N'uint', CAST(0x00009F2E00AC2BA4 AS DateTime), CAST(0x00009F2E00AC2BA4 AS DateTime), 1)
INSERT [dbo].[BusinessDataType] ([ID], [DefaultDBType], [DefaultLength], [DefaultPrecision], [NHType], [Name], [CodeName], [ModificationTime], [CreationTime], [IsSystem]) VALUES (N'e10218c6-1b0b-4269-a879-df0617f1e6e0', N'text', 0, 0, N'StringClob', N'Text', N'string', CAST(0x00009F2E00B2B92B AS DateTime), CAST(0x00009F2E00B2B92B AS DateTime), 1)
INSERT [dbo].[BusinessDataType] ([ID], [DefaultDBType], [DefaultLength], [DefaultPrecision], [NHType], [Name], [CodeName], [ModificationTime], [CreationTime], [IsSystem]) VALUES (N'54be637f-8fe7-4872-a471-e8bd1c902995', N'smallint', 0, 0, N'int16', N'Short', N'short', CAST(0x00009F32011E4C0E AS DateTime), CAST(0x00009F32011E4C0E AS DateTime), 1)
INSERT [dbo].[BusinessDataType] ([ID], [DefaultDBType], [DefaultLength], [DefaultPrecision], [NHType], [Name], [CodeName], [ModificationTime], [CreationTime], [IsSystem]) VALUES (N'abc026e2-a379-44a4-a59c-ebe636b00f11', N'float', 0, 0, N'Decimal', N'Decimal', N'decimal', CAST(0x00009F2E00AEEB7D AS DateTime), CAST(0x00009F2E00AEEB7D AS DateTime), 1)
INSERT [dbo].[BusinessEnum] ([ID], [ModificationTime], [CreationTime], [IsSystem], [PACKAGEID], [Name], [CodeName], [IsFlags], [IsExternal], [ExternalAssembly], [ExternalNamespace]) VALUES (N'0108ad08-bc64-43fc-8dd5-ba59213e9f4b', CAST(0x0000A031016A4630 AS DateTime), CAST(0x0000A0310169EE4C AS DateTime), 0, N'1bb4fafe-794a-43a1-aa70-bca32d59a64f', N'Gender', N'Gender', 0, 0, NULL, NULL)
INSERT [dbo].[BusinessEnumValue] ([ID], [ModificationTime], [CreationTime], [IsSystem], [ENUMID], [Name], [CodeName], [Value]) VALUES (N'fedef0c4-ba61-4039-9f60-e4a318c5e27a', CAST(0x0000A031016A4630 AS DateTime), CAST(0x0000A031016A0814 AS DateTime), 0, N'0108ad08-bc64-43fc-8dd5-ba59213e9f4b', N'Female', N'Female', 1)
INSERT [dbo].[BusinessEnumValue] ([ID], [ModificationTime], [CreationTime], [IsSystem], [ENUMID], [Name], [CodeName], [Value]) VALUES (N'467a77e6-9745-40b9-9629-f24d7a43fc8c', CAST(0x0000A031016A4630 AS DateTime), CAST(0x0000A0310169FC5C AS DateTime), 0, N'0108ad08-bc64-43fc-8dd5-ba59213e9f4b', N'Male', N'Male', 0)
INSERT [dbo].[BusinessExtraScript] ([ID], [PackageID], [ModificationTime], [CreationTime], [IsSystem], [Name], [Script], [MetaData]) VALUES (N'f2beef52-f944-47ce-b602-2acc60d46249', N'8d3281fc-d0c4-4ca5-b370-595060488a6c', CAST(0x0000A02C01802D9C AS DateTime), CAST(0x0000A02800C14814 AS DateTime), 0, N'Resources.resx', N'<?xml version="1.0" encoding="utf-8"?>
<root>
	<!-- 
		Microsoft ResX Schema

		Version 1.3

		The primary goals of this format is to allow a simple XML format 
		that is mostly human readable. The generation and parsing of the 
		various data types are done through the TypeConverter classes 
		associated with the data types.

		Example:

		... ado.net/XML headers & schema ...
		<resheader name="resmimetype">text/microsoft-resx</resheader>
		<resheader name="version">1.3</resheader>
		<resheader name="reader">System.Resources.ResXResourceReader, System.Windows.Forms, ...</resheader>
		<resheader name="writer">System.Resources.ResXResourceWriter, System.Windows.Forms, ...</resheader>
		<data name="Name1">this is my long string</data>
		<data name="Color1" type="System.Drawing.Color, System.Drawing">Blue</data>
		<data name="Bitmap1" mimetype="application/x-microsoft.net.object.binary.base64">
			[base64 mime encoded serialized .NET Framework object]
		</data>
		<data name="Icon1" type="System.Drawing.Icon, System.Drawing" mimetype="application/x-microsoft.net.object.bytearray.base64">
			[base64 mime encoded string representing a byte array form of the .NET Framework object]
		</data>

		There are any number of "resheader" rows that contain simple 
		name/value pairs.

		Each data row contains a name, and value. The row also contains a 
		type or mimetype. Type corresponds to a .NET class that support 
		text/value conversion through the TypeConverter architecture. 
		Classes that don''t support this are serialized and stored with the 
		mimetype set.

		The mimetype is used for serialized objects, and tells the 
		ResXResourceReader how to depersist the object. This is currently not 
		extensible. For a given mimetype the value must be set accordingly:

		Note - application/x-microsoft.net.object.binary.base64 is the format 
		that the ResXResourceWriter will generate, however the reader can 
		read any of the formats listed below.

		mimetype: application/x-microsoft.net.object.binary.base64
		value   : The object must be serialized with 
			: System.Serialization.Formatters.Binary.BinaryFormatter
			: and then encoded with base64 encoding.

		mimetype: application/x-microsoft.net.object.soap.base64
		value   : The object must be serialized with 
			: System.Runtime.Serialization.Formatters.Soap.SoapFormatter
			: and then encoded with base64 encoding.

		mimetype: application/x-microsoft.net.object.bytearray.base64
		value   : The object must be serialized into a byte array 
			: using a System.ComponentModel.TypeConverter
			: and then encoded with base64 encoding.
	-->
	
	<xsd:schema id="root" xmlns="" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:msdata="urn:schemas-microsoft-com:xml-msdata">
		<xsd:element name="root" msdata:IsDataSet="true">
			<xsd:complexType>
				<xsd:choice maxOccurs="unbounded">
					<xsd:element name="data">
						<xsd:complexType>
							<xsd:sequence>
								<xsd:element name="value" type="xsd:string" minOccurs="0" msdata:Ordinal="1" />
								<xsd:element name="comment" type="xsd:string" minOccurs="0" msdata:Ordinal="2" />
							</xsd:sequence>
							<xsd:attribute name="name" type="xsd:string" msdata:Ordinal="1" />
							<xsd:attribute name="type" type="xsd:string" msdata:Ordinal="3" />
							<xsd:attribute name="mimetype" type="xsd:string" msdata:Ordinal="4" />
						</xsd:complexType>
					</xsd:element>
					<xsd:element name="resheader">
						<xsd:complexType>
							<xsd:sequence>
								<xsd:element name="value" type="xsd:string" minOccurs="0" msdata:Ordinal="1" />
							</xsd:sequence>
							<xsd:attribute name="name" type="xsd:string" use="required" />
						</xsd:complexType>
					</xsd:element>
				</xsd:choice>
			</xsd:complexType>
		</xsd:element>
	</xsd:schema>
	<resheader name="resmimetype">
		<value>text/microsoft-resx</value>
	</resheader>
	<resheader name="version">
		<value>1.3</value>
	</resheader>
	<resheader name="reader">
		<value>System.Resources.ResXResourceReader, System.Windows.Forms, Version=2.0.3500.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
	</resheader>
	<resheader name="writer">
		<value>System.Resources.ResXResourceWriter, System.Windows.Forms, Version=2.0.3500.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
	</resheader>
</root>', N'<EmbeddedResource>
  <Generator>ResXFileCodeGenerator</Generator>
  <LastGenOutput>Resources.Designer.cs</LastGenOutput>
</EmbeddedResource>')
INSERT [dbo].[BusinessExtraScript] ([ID], [PackageID], [ModificationTime], [CreationTime], [IsSystem], [Name], [Script], [MetaData]) VALUES (N'f6a2a071-6433-4337-bd8c-8d4ce581ca58', N'8d3281fc-d0c4-4ca5-b370-595060488a6c', CAST(0x0000A02800C14814 AS DateTime), CAST(0x0000A02800C13EB4 AS DateTime), 0, N'AssemblyInfo.cs', N'using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Fantasy")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("Fantasy")]
[assembly: AssemblyCopyright("Copyright ©  2012")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("64c6525c-87b5-44ff-b925-f88e765cedcc")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
', N'<Compile />')
INSERT [dbo].[BusinessExtraScript] ([ID], [PackageID], [ModificationTime], [CreationTime], [IsSystem], [Name], [Script], [MetaData]) VALUES (N'2b015202-6f25-4c87-8de0-b7fd3258ed16', N'8d3281fc-d0c4-4ca5-b370-595060488a6c', CAST(0x0000A02800C43E48 AS DateTime), CAST(0x0000A02800C43E48 AS DateTime), 0, N'Settings.settings', N'<?xml version=''1.0'' encoding=''utf-8''?>
<SettingsFile xmlns="http://schemas.microsoft.com/VisualStudio/2004/01/settings" CurrentProfile="(Default)">
  <Profiles>
    <Profile Name="(Default)" />
  </Profiles>
</SettingsFile>
', N'<None>
  <Generator>SettingsSingleFileGenerator</Generator>
  <LastGenOutput>Settings.Designer.cs</LastGenOutput>
</None>')
INSERT [dbo].[BusinessExtraScript] ([ID], [PackageID], [ModificationTime], [CreationTime], [IsSystem], [Name], [Script], [MetaData]) VALUES (N'e790eff2-185f-442f-b6ad-dbcfeb35d5be', N'8d3281fc-d0c4-4ca5-b370-595060488a6c', CAST(0x0000A02800C43E48 AS DateTime), CAST(0x0000A02800C43AC4 AS DateTime), 0, N'Settings.Designer.cs', N'//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.488
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Fantasy.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
    }
}
', N'<Compile>
  <AutoGen>True</AutoGen>
  <DesignTimeSharedInput>True</DesignTimeSharedInput>
  <DependentUpon>Settings.settings</DependentUpon>
</Compile>')
INSERT [dbo].[BusinessExtraScript] ([ID], [PackageID], [ModificationTime], [CreationTime], [IsSystem], [Name], [Script], [MetaData]) VALUES (N'3b8e8a2e-7b06-4ece-8514-ff9b17a7d021', N'8d3281fc-d0c4-4ca5-b370-595060488a6c', CAST(0x0000A02C01802D9C AS DateTime), CAST(0x0000A02800C13EB4 AS DateTime), 0, N'Resources.Designer.cs', N'//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.261
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Fantasy.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Fantasy.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread''s CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
    }
}
', N'<Compile>
  <AutoGen>True</AutoGen>
  <DesignTime>True</DesignTime>
  <DependentUpon>Resources.resx</DependentUpon>
</Compile>')
INSERT [dbo].[BusinessLastUpdateTimestamp] ([Name], [Seconds]) VALUES (N'USERROLES', 0)
INSERT [dbo].[BusinessObject] ([ID], [CLASSID], [ModificationTime], [CreationTime], [IsSystem], [Name]) VALUES (N'9ffb15da-dcac-400f-a188-afb0bd8c16c2', N'bf0aa7f4-588f-4556-963d-33242e649d57', CAST(0x0000A03101598DF6 AS DateTime), CAST(0x0000A03101598DF6 AS DateTime), 1, N'Singularity')
INSERT [dbo].[BusinessPackage] ([ID], [ParentPackageID], [PackageType], [Name], [CodeName], [ModificationTime], [CreationTime], [IsSystem]) VALUES (N'28f26cdc-9ce5-4d0a-814d-08ce58105e25', NULL, NULL, N'System Root', N'Fantasy', CAST(0x00009ED1015FD650 AS DateTime), CAST(0x00009ED1015FD650 AS DateTime), 1)
INSERT [dbo].[BusinessPackage] ([ID], [ParentPackageID], [PackageType], [Name], [CodeName], [ModificationTime], [CreationTime], [IsSystem]) VALUES (N'8d3281fc-d0c4-4ca5-b370-595060488a6c', N'28f26cdc-9ce5-4d0a-814d-08ce58105e25', NULL, N'Properties', N'Properties', CAST(0x0000A02800C14814 AS DateTime), CAST(0x0000A02800C13EB4 AS DateTime), 1)
INSERT [dbo].[BusinessPackage] ([ID], [ParentPackageID], [PackageType], [Name], [CodeName], [ModificationTime], [CreationTime], [IsSystem]) VALUES (N'ba17d31c-8cdd-46f8-80be-663d07197c2e', N'1bb4fafe-794a-43a1-aa70-bca32d59a64f', NULL, N'Roles', N'Roles', CAST(0x0000A03101698AB0 AS DateTime), CAST(0x0000A031016983A8 AS DateTime), 0)
INSERT [dbo].[BusinessPackage] ([ID], [ParentPackageID], [PackageType], [Name], [CodeName], [ModificationTime], [CreationTime], [IsSystem]) VALUES (N'aa2b7117-1332-447c-b013-8eeeeaabffde', N'bdf7bbb0-0991-4f13-ad22-d45bfec45b66', NULL, N'Users', N'Users', CAST(0x0000A0310163602C AS DateTime), CAST(0x0000A03101634664 AS DateTime), 1)
INSERT [dbo].[BusinessPackage] ([ID], [ParentPackageID], [PackageType], [Name], [CodeName], [ModificationTime], [CreationTime], [IsSystem]) VALUES (N'bcbe8988-8ac3-43cc-8d59-a5d398ec35ba', N'1bb4fafe-794a-43a1-aa70-bca32d59a64f', NULL, N'Users', N'Users', CAST(0x0000A03101697DCC AS DateTime), CAST(0x0000A03101697598 AS DateTime), 0)
INSERT [dbo].[BusinessPackage] ([ID], [ParentPackageID], [PackageType], [Name], [CodeName], [ModificationTime], [CreationTime], [IsSystem]) VALUES (N'1bb4fafe-794a-43a1-aa70-bca32d59a64f', N'28f26cdc-9ce5-4d0a-814d-08ce58105e25', NULL, N'Ognization', N'Ognization', CAST(0x0000A031016955F4 AS DateTime), CAST(0x0000A03101693074 AS DateTime), 0)
INSERT [dbo].[BusinessPackage] ([ID], [ParentPackageID], [PackageType], [Name], [CodeName], [ModificationTime], [CreationTime], [IsSystem]) VALUES (N'fe8c21e4-a3c5-46ff-ae2e-d29b72f21ee2', N'bdf7bbb0-0991-4f13-ad22-d45bfec45b66', NULL, N'Roles', N'Roles', CAST(0x0000A0310163602C AS DateTime), CAST(0x0000A031016355A0 AS DateTime), 1)
INSERT [dbo].[BusinessPackage] ([ID], [ParentPackageID], [PackageType], [Name], [CodeName], [ModificationTime], [CreationTime], [IsSystem]) VALUES (N'bdf7bbb0-0991-4f13-ad22-d45bfec45b66', N'28f26cdc-9ce5-4d0a-814d-08ce58105e25', NULL, N'BusinessEngine', N'BusinessEngine', CAST(0x00009F3601493364 AS DateTime), CAST(0x00009EDD015FD650 AS DateTime), 1)
INSERT [dbo].[BusinessPackage] ([ID], [ParentPackageID], [PackageType], [Name], [CodeName], [ModificationTime], [CreationTime], [IsSystem]) VALUES (N'09196be5-c015-46ba-84b5-fd4811352ffd', N'bdf7bbb0-0991-4f13-ad22-d45bfec45b66', NULL, N'Applications', N'Applications', CAST(0x0000A031016378C8 AS DateTime), CAST(0x0000A03101636860 AS DateTime), 1)
INSERT [dbo].[BusinessProperty] ([ID], [CLASSID], [ModificationTime], [CreationTime], [IsSystem], [ClassTypeID], [EnumTypeID], [DataTypeID], [Name], [CodeName], [FieldName], [FieldType], [Length], [Precision], [IsCalculated], [IsNullable], [DefaultValue], [ScriptOptions], [DisplayOrder]) VALUES (N'facc2bf7-899c-4ff8-b88e-68032dfebcab', N'b523d059-25a3-45da-b1ab-0ef4b39828b6', CAST(0x0000A031016A4630 AS DateTime), CAST(0x0000A031016A1750 AS DateTime), 0, NULL, N'0108ad08-bc64-43fc-8dd5-ba59213e9f4b', N'f1e72c1d-2432-4da6-82d6-aa2ddeda84ed', N'Gender', N'Gender', N'GENDER', N'numeric', 18, 0, 0, 1, NULL, NULL, 259)
INSERT [dbo].[BusinessProperty] ([ID], [CLASSID], [ModificationTime], [CreationTime], [IsSystem], [ClassTypeID], [EnumTypeID], [DataTypeID], [Name], [CodeName], [FieldName], [FieldType], [Length], [Precision], [IsCalculated], [IsNullable], [DefaultValue], [ScriptOptions], [DisplayOrder]) VALUES (N'c9b092be-fce4-4793-9bba-9f3300ac9427', N'bf0aa7f4-588f-4556-963d-33242e649d57', CAST(0x00009F3300AC93B0 AS DateTime), CAST(0x00009F3300AAF604 AS DateTime), 1, NULL, NULL, N'a2b55595-6959-4ec7-8daf-23d2ba6b2c99', N'Id', N'Id', N'Id', N'uniqueidentifier', 0, 0, 0, 0, NULL, NULL, 6)
INSERT [dbo].[BusinessProperty] ([ID], [CLASSID], [ModificationTime], [CreationTime], [IsSystem], [ClassTypeID], [EnumTypeID], [DataTypeID], [Name], [CodeName], [FieldName], [FieldType], [Length], [Precision], [IsCalculated], [IsNullable], [DefaultValue], [ScriptOptions], [DisplayOrder]) VALUES (N'57b4a057-35b6-4e09-89f4-9f3300ac942f', N'bf0aa7f4-588f-4556-963d-33242e649d57', CAST(0x00009F3300CA3668 AS DateTime), CAST(0x00009F3300AC1C28 AS DateTime), 1, NULL, NULL, N'a2b55595-6959-4ec7-8daf-23d2ba6b2c99', N'Class', N'Class', N'ClassId', N'uniqueidentifier', 0, 0, 0, 0, NULL, NULL, 8)
INSERT [dbo].[BusinessProperty] ([ID], [CLASSID], [ModificationTime], [CreationTime], [IsSystem], [ClassTypeID], [EnumTypeID], [DataTypeID], [Name], [CodeName], [FieldName], [FieldType], [Length], [Precision], [IsCalculated], [IsNullable], [DefaultValue], [ScriptOptions], [DisplayOrder]) VALUES (N'0be3b780-5b3d-4840-8aaa-9f3300ac942f', N'bf0aa7f4-588f-4556-963d-33242e649d57', CAST(0x00009F3300CA3668 AS DateTime), CAST(0x00009F3300AB561C AS DateTime), 1, NULL, NULL, N'a2883f92-473f-4a07-8d96-85e3fcf31588', N'Creation Time', N'CreationTime', N'CreationTime', N'datetime2', 0, 0, 0, 0, NULL, NULL, 9)
INSERT [dbo].[BusinessProperty] ([ID], [CLASSID], [ModificationTime], [CreationTime], [IsSystem], [ClassTypeID], [EnumTypeID], [DataTypeID], [Name], [CodeName], [FieldName], [FieldType], [Length], [Precision], [IsCalculated], [IsNullable], [DefaultValue], [ScriptOptions], [DisplayOrder]) VALUES (N'4ed247a4-7e5a-4b2d-9444-9f3300ac942f', N'bf0aa7f4-588f-4556-963d-33242e649d57', CAST(0x00009F3300CA3668 AS DateTime), CAST(0x00009F3300AB8754 AS DateTime), 1, NULL, NULL, N'a2883f92-473f-4a07-8d96-85e3fcf31588', N'Modification Time', N'ModificationTime', N'ModificationTime', N'datetime2', 0, 0, 0, 0, NULL, NULL, 10)
INSERT [dbo].[BusinessProperty] ([ID], [CLASSID], [ModificationTime], [CreationTime], [IsSystem], [ClassTypeID], [EnumTypeID], [DataTypeID], [Name], [CodeName], [FieldName], [FieldType], [Length], [Precision], [IsCalculated], [IsNullable], [DefaultValue], [ScriptOptions], [DisplayOrder]) VALUES (N'e5434005-55e8-482f-a46a-9f3300ac942f', N'bf0aa7f4-588f-4556-963d-33242e649d57', CAST(0x00009F3300AC93B0 AS DateTime), CAST(0x00009F3300AB192C AS DateTime), 1, NULL, NULL, N'1e12aa32-dd17-4f37-8d4c-5ffe9b53dfe1', N'Name', N'Name', N'Name', N'nvarchar', 255, 0, 0, 0, NULL, NULL, 7)
INSERT [dbo].[BusinessProperty] ([ID], [CLASSID], [ModificationTime], [CreationTime], [IsSystem], [ClassTypeID], [EnumTypeID], [DataTypeID], [Name], [CodeName], [FieldName], [FieldType], [Length], [Precision], [IsCalculated], [IsNullable], [DefaultValue], [ScriptOptions], [DisplayOrder]) VALUES (N'9171981d-4d61-499d-b8a9-9f3300ac942f', N'bf0aa7f4-588f-4556-963d-33242e649d57', CAST(0x00009F3300CA3668 AS DateTime), CAST(0x00009F3300ABC444 AS DateTime), 1, NULL, NULL, N'b9ceb0d0-ef32-44ff-8cdc-3b609a65d27f', N'Is System', N'IsSystem', N'IsSystem', N'bit', 0, 0, 0, 0, N'0', NULL, 11)
INSERT [dbo].[BusinessProperty] ([ID], [CLASSID], [ModificationTime], [CreationTime], [IsSystem], [ClassTypeID], [EnumTypeID], [DataTypeID], [Name], [CodeName], [FieldName], [FieldType], [Length], [Precision], [IsCalculated], [IsNullable], [DefaultValue], [ScriptOptions], [DisplayOrder]) VALUES (N'23cd771b-980b-4477-8db9-dd5160b51ac8', N'b523d059-25a3-45da-b1ab-0ef4b39828b6', CAST(0x0000A031016A4630 AS DateTime), CAST(0x0000A0310169E03C AS DateTime), 0, NULL, NULL, N'44288a16-267e-4fda-a588-b82fba83bb13', N'Age', N'Age', N'AGE', N'numeric', 18, 0, 0, 1, NULL, NULL, 258)
INSERT [dbo].[BusinessRole] ([ID], [ModificationTime], [CreationTime], [IsSystem], [PACKAGEID], [Name], [CodeName], [Description], [IsComputed], [IsDisabled], [Script], [ScriptOptions], [ExternalType]) VALUES (N'a51973b2-7ce8-440e-8a9c-1b48b59b0499', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x00009F6400BAB544 AS DateTime), 1, N'fe8c21e4-a3c5-46ff-ae2e-d29b72f21ee2', N'Everyone', N'EveryOne', N'Built-in role for representing everyone in system. ', 1, 0, N'', 0, NULL)
INSERT [dbo].[BusinessRole] ([ID], [ModificationTime], [CreationTime], [IsSystem], [PACKAGEID], [Name], [CodeName], [Description], [IsComputed], [IsDisabled], [Script], [ScriptOptions], [ExternalType]) VALUES (N'e8700b4b-7911-470e-8e0e-458507d1f51d', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x00009F6400BA5D60 AS DateTime), 1, N'fe8c21e4-a3c5-46ff-ae2e-d29b72f21ee2', N'Users', N'Users', N'Built-in role for representing authenticated users in the system.', 1, 0, N'
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy;
using Fantasy.BusinessEngine;

namespace Fantasy.BusinessEngine
{
    public partial class Users : Fantasy.BusinessEngine.BusinessRole
	{
	   
    }
}
', 0, NULL)
INSERT [dbo].[BusinessRole] ([ID], [ModificationTime], [CreationTime], [IsSystem], [PACKAGEID], [Name], [CodeName], [Description], [IsComputed], [IsDisabled], [Script], [ScriptOptions], [ExternalType]) VALUES (N'0dab7123-f09f-4e04-9ea4-4dcfa8dfa040', CAST(0x0000A031016C5330 AS DateTime), CAST(0x0000A031016C3E18 AS DateTime), 0, N'ba17d31c-8cdd-46f8-80be-663d07197c2e', N'HR', N'HR', NULL, 0, 0, N'using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy;
using Fantasy.BusinessEngine;
namespace Fantasy.Ognization.Roles
{
	public partial class HR : Fantasy.BusinessEngine.BusinessRole
	{
	}
}
', 0, NULL)
INSERT [dbo].[BusinessRole] ([ID], [ModificationTime], [CreationTime], [IsSystem], [PACKAGEID], [Name], [CodeName], [Description], [IsComputed], [IsDisabled], [Script], [ScriptOptions], [ExternalType]) VALUES (N'966f50ab-1672-4e74-80ce-cdf950990eb8', CAST(0x0000A031016C752C AS DateTime), CAST(0x0000A031016C64C4 AS DateTime), 0, N'ba17d31c-8cdd-46f8-80be-663d07197c2e', N'HRManager', N'HRManager', NULL, 0, 0, N'using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy;
using Fantasy.BusinessEngine;
namespace Fantasy.Ognization.Roles
{
	public partial class HRManager : Fantasy.BusinessEngine.BusinessRole
	{
	}
}
', 0, NULL)
INSERT [dbo].[BusinessRole] ([ID], [ModificationTime], [CreationTime], [IsSystem], [PACKAGEID], [Name], [CodeName], [Description], [IsComputed], [IsDisabled], [Script], [ScriptOptions], [ExternalType]) VALUES (N'6d21bc2a-870a-4928-b219-fae9c4c9fd15', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x00009F6400B9CD3C AS DateTime), 1, N'fe8c21e4-a3c5-46ff-ae2e-d29b72f21ee2', N'Administrators', N'Administrators', N'Administrators have complete and unrestricted access to the system.', 0, 0, N'
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy;
using Fantasy.BusinessEngine;

namespace Fantasy.BusinessEngine.Roles
{
    public partial class Administrators : Fantasy.BusinessEngine.BusinessRole
	{
	   
    }
}
', 0, NULL)
INSERT [dbo].[BusinessUser] ([ID], [ModificationTime], [CreationTime], [IsSystem], [PACKAGEID], [Name], [CodeName], [FullName], [Description], [Password], [IsDisabled], [Script], [ScriptOptions], [ExternalType]) VALUES (N'cee4f357-6985-4beb-bcea-0bcabd232c4a', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x00009F6400C028F8 AS DateTime), 1, N'aa2b7117-1332-447c-b013-8eeeeaabffde', N'Administrator', N'Administrator', N'Administrator', N'Built-in account for administring the system.', N'/JXYlYVtZLIrwW0SjpcFlA==', 0, N'using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy;
using Fantasy.BusinessEngine;
namespace Fantasy.BusinessEngine.Users
{
	public partial class Administrator : Fantasy.BusinessEngine.BusinessUser
	{
	}
}
', 0, NULL)
INSERT [dbo].[BusinessUser] ([ID], [ModificationTime], [CreationTime], [IsSystem], [PACKAGEID], [Name], [CodeName], [FullName], [Description], [Password], [IsDisabled], [Script], [ScriptOptions], [ExternalType]) VALUES (N'4f2accae-e0ed-4605-adfe-3c1a226a0c0c', CAST(0x0000A031016C2EDC AS DateTime), CAST(0x0000A02C017D10BC AS DateTime), 1, N'aa2b7117-1332-447c-b013-8eeeeaabffde', N'Guest', N'Guest', N'Guest', N'Built-in account for anonymous user.', NULL, 0, N'using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy;
using Fantasy.BusinessEngine;
namespace Fantasy.BusinessEngine.Users
{
	public partial class Guest : Fantasy.BusinessEngine.BusinessUser
	{
	}
}
', 0, NULL)
INSERT [dbo].[BusinessUserRole] ([USERID], [ROLEID]) VALUES (N'cee4f357-6985-4beb-bcea-0bcabd232c4a', N'6d21bc2a-870a-4928-b219-fae9c4c9fd15')
/****** Object:  Index [ASSEMBLYGROUPREFERENCE_FK]    Script Date: 2012/4/12 22:28:08 ******/
CREATE NONCLUSTERED INDEX [ASSEMBLYGROUPREFERENCE_FK] ON [dbo].[AssemblyReference]
(
	[GROUPID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [BUSINESSAPPLICATIONENTRYOBJECT_FK]    Script Date: 2012/4/12 22:28:08 ******/
CREATE NONCLUSTERED INDEX [BUSINESSAPPLICATIONENTRYOBJECT_FK] ON [dbo].[BusinessApplication]
(
	[EntryObjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [BUSINESSPACKAGEAPPLICATIONS_FK]    Script Date: 2012/4/12 22:28:08 ******/
CREATE NONCLUSTERED INDEX [BUSINESSPACKAGEAPPLICATIONS_FK] ON [dbo].[BusinessApplication]
(
	[PACKAGEID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [APPLICATIONPARTICIPANTACL_FK]    Script Date: 2012/4/12 22:28:08 ******/
CREATE NONCLUSTERED INDEX [APPLICATIONPARTICIPANTACL_FK] ON [dbo].[BusinessApplicationACL]
(
	[PARTICIPANTID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [BUSINESSAPPLICATIONACLSTATE_FK]    Script Date: 2012/4/12 22:28:08 ******/
CREATE NONCLUSTERED INDEX [BUSINESSAPPLICATIONACLSTATE_FK] ON [dbo].[BusinessApplicationACL]
(
	[STATEID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [CLASSACCESSCONTROLROLE_FK]    Script Date: 2012/4/12 22:28:08 ******/
CREATE NONCLUSTERED INDEX [CLASSACCESSCONTROLROLE_FK] ON [dbo].[BusinessApplicationACL]
(
	[ROLEID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PARTICIPANTCLASS_FK]    Script Date: 2012/4/12 22:28:08 ******/
CREATE NONCLUSTERED INDEX [PARTICIPANTCLASS_FK] ON [dbo].[BusinessApplicationParticipant]
(
	[CLASSID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PATICIPANTAPPLICATION_FK]    Script Date: 2012/4/12 22:28:08 ******/
CREATE NONCLUSTERED INDEX [PATICIPANTAPPLICATION_FK] ON [dbo].[BusinessApplicationParticipant]
(
	[APPLICATIONID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PACKAGEASSOCIATIONS_FK]    Script Date: 2012/4/12 22:28:08 ******/
CREATE NONCLUSTERED INDEX [PACKAGEASSOCIATIONS_FK] ON [dbo].[BusinessAssociation]
(
	[PackageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [RELATIONSHIPLEFT_FK]    Script Date: 2012/4/12 22:28:08 ******/
CREATE NONCLUSTERED INDEX [RELATIONSHIPLEFT_FK] ON [dbo].[BusinessAssociation]
(
	[LEFTCLASSID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [RELATIONSHIPRIGHT_FK]    Script Date: 2012/4/12 22:28:08 ******/
CREATE NONCLUSTERED INDEX [RELATIONSHIPRIGHT_FK] ON [dbo].[BusinessAssociation]
(
	[RightClassID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PACKAGECLASSES_FK]    Script Date: 2012/4/12 22:28:08 ******/
CREATE NONCLUSTERED INDEX [PACKAGECLASSES_FK] ON [dbo].[BusinessClass]
(
	[PackageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PARENTCLASS_FK]    Script Date: 2012/4/12 22:28:08 ******/
CREATE NONCLUSTERED INDEX [PARENTCLASS_FK] ON [dbo].[BusinessClass]
(
	[ParentClassID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PACKAGECLASSDIAGRAMS_FK]    Script Date: 2012/4/12 22:28:08 ******/
CREATE NONCLUSTERED INDEX [PACKAGECLASSDIAGRAMS_FK] ON [dbo].[BusinessClassDiagram]
(
	[PACKAGEID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PACKAGEENUM_FK]    Script Date: 2012/4/12 22:28:08 ******/
CREATE NONCLUSTERED INDEX [PACKAGEENUM_FK] ON [dbo].[BusinessEnum]
(
	[PACKAGEID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ENUMVALUES_FK]    Script Date: 2012/4/12 22:28:08 ******/
CREATE NONCLUSTERED INDEX [ENUMVALUES_FK] ON [dbo].[BusinessEnumValue]
(
	[ENUMID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PACKAGEEXTRASCRIPTS_FK]    Script Date: 2012/4/12 22:28:08 ******/
CREATE NONCLUSTERED INDEX [PACKAGEEXTRASCRIPTS_FK] ON [dbo].[BusinessExtraScript]
(
	[PackageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [BUSINESSCLASSOBJECT_FK]    Script Date: 2012/4/12 22:28:08 ******/
CREATE NONCLUSTERED INDEX [BUSINESSCLASSOBJECT_FK] ON [dbo].[BusinessObject]
(
	[CLASSID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [CHILDPACKAGES_FK]    Script Date: 2012/4/12 22:28:08 ******/
CREATE NONCLUSTERED INDEX [CHILDPACKAGES_FK] ON [dbo].[BusinessPackage]
(
	[ParentPackageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PROPERTIES_FK]    Script Date: 2012/4/12 22:28:08 ******/
CREATE NONCLUSTERED INDEX [PROPERTIES_FK] ON [dbo].[BusinessProperty]
(
	[CLASSID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PROPERTYCLASSTYPE_FK]    Script Date: 2012/4/12 22:28:08 ******/
CREATE NONCLUSTERED INDEX [PROPERTYCLASSTYPE_FK] ON [dbo].[BusinessProperty]
(
	[ClassTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PROPERTYENUMTYPE_FK]    Script Date: 2012/4/12 22:28:08 ******/
CREATE NONCLUSTERED INDEX [PROPERTYENUMTYPE_FK] ON [dbo].[BusinessProperty]
(
	[EnumTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PROPERTYTYPE_FK]    Script Date: 2012/4/12 22:28:08 ******/
CREATE NONCLUSTERED INDEX [PROPERTYTYPE_FK] ON [dbo].[BusinessProperty]
(
	[DataTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [BUSINESSPACKAGEROLES_FK]    Script Date: 2012/4/12 22:28:08 ******/
CREATE NONCLUSTERED INDEX [BUSINESSPACKAGEROLES_FK] ON [dbo].[BusinessRole]
(
	[PACKAGEID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [BUSINESSPACKAGEUSERS_FK]    Script Date: 2012/4/12 22:28:08 ******/
CREATE NONCLUSTERED INDEX [BUSINESSPACKAGEUSERS_FK] ON [dbo].[BusinessUser]
(
	[PACKAGEID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ROLES_FK]    Script Date: 2012/4/12 22:28:08 ******/
CREATE NONCLUSTERED INDEX [ROLES_FK] ON [dbo].[BusinessUserRole]
(
	[USERID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [USERS_FK]    Script Date: 2012/4/12 22:28:08 ******/
CREATE NONCLUSTERED INDEX [USERS_FK] ON [dbo].[BusinessUserRole]
(
	[ROLEID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AssemblyReference]  WITH CHECK ADD  CONSTRAINT [FK_ASSEMBLYGROUP_ASSEMBLY] FOREIGN KEY([GROUPID])
REFERENCES [dbo].[AssemblyReferenceGroup] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AssemblyReference] CHECK CONSTRAINT [FK_ASSEMBLYGROUP_ASSEMBLY]
GO
ALTER TABLE [dbo].[ASSN_DEPARTMENT_LEADER]  WITH CHECK ADD  CONSTRAINT [FK_ASSN_DEPARTMENT_LEADER_LEFT] FOREIGN KEY([LEFTID])
REFERENCES [dbo].[CLS_DEPARTMENT] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ASSN_DEPARTMENT_LEADER] CHECK CONSTRAINT [FK_ASSN_DEPARTMENT_LEADER_LEFT]
GO
ALTER TABLE [dbo].[ASSN_DEPARTMENT_LEADER]  WITH CHECK ADD  CONSTRAINT [FK_ASSN_DEPARTMENT_LEADER_RIGHT] FOREIGN KEY([RIGHTID])
REFERENCES [dbo].[CLS_HUMAN] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ASSN_DEPARTMENT_LEADER] CHECK CONSTRAINT [FK_ASSN_DEPARTMENT_LEADER_RIGHT]
GO
ALTER TABLE [dbo].[ASSN_DEPARTMENT_STAFF]  WITH CHECK ADD  CONSTRAINT [FK_ASSN_DEPARTMENT_STAFF_LEFT] FOREIGN KEY([LEFTID])
REFERENCES [dbo].[CLS_DEPARTMENT] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ASSN_DEPARTMENT_STAFF] CHECK CONSTRAINT [FK_ASSN_DEPARTMENT_STAFF_LEFT]
GO
ALTER TABLE [dbo].[ASSN_DEPARTMENT_STAFF]  WITH CHECK ADD  CONSTRAINT [FK_ASSN_DEPARTMENT_STAFF_RIGHT] FOREIGN KEY([RIGHTID])
REFERENCES [dbo].[CLS_HUMAN] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ASSN_DEPARTMENT_STAFF] CHECK CONSTRAINT [FK_ASSN_DEPARTMENT_STAFF_RIGHT]
GO
ALTER TABLE [dbo].[BusinessApplication]  WITH CHECK ADD  CONSTRAINT [FK_BUSINESS_APPLICATION_ENTRYOBJECT] FOREIGN KEY([EntryObjectId])
REFERENCES [dbo].[BusinessObject] ([ID])
GO
ALTER TABLE [dbo].[BusinessApplication] CHECK CONSTRAINT [FK_BUSINESS_APPLICATION_ENTRYOBJECT]
GO
ALTER TABLE [dbo].[BusinessApplication]  WITH CHECK ADD  CONSTRAINT [FK_BUSINESS_PACKAGE_APPLICATION] FOREIGN KEY([PACKAGEID])
REFERENCES [dbo].[BusinessPackage] ([ID])
GO
ALTER TABLE [dbo].[BusinessApplication] CHECK CONSTRAINT [FK_BUSINESS_PACKAGE_APPLICATION]
GO
ALTER TABLE [dbo].[BusinessApplicationACL]  WITH CHECK ADD  CONSTRAINT [FK_BUSINESS_PARTICIPANT_ACL] FOREIGN KEY([PARTICIPANTID])
REFERENCES [dbo].[BusinessApplicationParticipant] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BusinessApplicationACL] CHECK CONSTRAINT [FK_BUSINESS_PARTICIPANT_ACL]
GO
ALTER TABLE [dbo].[BusinessApplicationACL]  WITH CHECK ADD  CONSTRAINT [FK_BUSINESS_ROLE_ACL] FOREIGN KEY([ROLEID])
REFERENCES [dbo].[BusinessRole] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BusinessApplicationACL] CHECK CONSTRAINT [FK_BUSINESS_ROLE_ACL]
GO
ALTER TABLE [dbo].[BusinessApplicationACL]  WITH CHECK ADD  CONSTRAINT [FK_BUSINESSAPPACL_STATE] FOREIGN KEY([STATEID])
REFERENCES [dbo].[BusinessEnumValue] ([ID])
GO
ALTER TABLE [dbo].[BusinessApplicationACL] CHECK CONSTRAINT [FK_BUSINESSAPPACL_STATE]
GO
ALTER TABLE [dbo].[BusinessApplicationParticipant]  WITH CHECK ADD  CONSTRAINT [FK_BUSINESS_APPLICATION_PARTICIPANT] FOREIGN KEY([APPLICATIONID])
REFERENCES [dbo].[BusinessApplication] ([ID])
GO
ALTER TABLE [dbo].[BusinessApplicationParticipant] CHECK CONSTRAINT [FK_BUSINESS_APPLICATION_PARTICIPANT]
GO
ALTER TABLE [dbo].[BusinessApplicationParticipant]  WITH CHECK ADD  CONSTRAINT [FK_BUSINESS_PARTICIPANT_CLASS] FOREIGN KEY([CLASSID])
REFERENCES [dbo].[BusinessClass] ([ID])
GO
ALTER TABLE [dbo].[BusinessApplicationParticipant] CHECK CONSTRAINT [FK_BUSINESS_PARTICIPANT_CLASS]
GO
ALTER TABLE [dbo].[BusinessAssociation]  WITH CHECK ADD  CONSTRAINT [FK_BUSINESS_PACKAGEA_ASSOCIATION] FOREIGN KEY([PackageID])
REFERENCES [dbo].[BusinessPackage] ([ID])
GO
ALTER TABLE [dbo].[BusinessAssociation] CHECK CONSTRAINT [FK_BUSINESS_PACKAGEA_ASSOCIATION]
GO
ALTER TABLE [dbo].[BusinessClass]  WITH CHECK ADD  CONSTRAINT [FK_BUSINESS_CHILDCLASS] FOREIGN KEY([ParentClassID])
REFERENCES [dbo].[BusinessClass] ([ID])
GO
ALTER TABLE [dbo].[BusinessClass] CHECK CONSTRAINT [FK_BUSINESS_CHILDCLASS]
GO
ALTER TABLE [dbo].[BusinessClass]  WITH CHECK ADD  CONSTRAINT [FK_BUSINESS_PACKAGE_CLASS] FOREIGN KEY([PackageID])
REFERENCES [dbo].[BusinessPackage] ([ID])
GO
ALTER TABLE [dbo].[BusinessClass] CHECK CONSTRAINT [FK_BUSINESS_PACKAGE_CLASS]
GO
ALTER TABLE [dbo].[BusinessClassDiagram]  WITH CHECK ADD  CONSTRAINT [FK_BUSINESS_PACKAGE_CLASSDIAGRAM] FOREIGN KEY([PACKAGEID])
REFERENCES [dbo].[BusinessPackage] ([ID])
GO
ALTER TABLE [dbo].[BusinessClassDiagram] CHECK CONSTRAINT [FK_BUSINESS_PACKAGE_CLASSDIAGRAM]
GO
ALTER TABLE [dbo].[BusinessEnum]  WITH CHECK ADD  CONSTRAINT [FK_BUSINESS_PACKAGE_ENUM] FOREIGN KEY([PACKAGEID])
REFERENCES [dbo].[BusinessPackage] ([ID])
GO
ALTER TABLE [dbo].[BusinessEnum] CHECK CONSTRAINT [FK_BUSINESS_PACKAGE_ENUM]
GO
ALTER TABLE [dbo].[BusinessEnumValue]  WITH CHECK ADD  CONSTRAINT [FK_BUSINESS_ENUM_VALUE] FOREIGN KEY([ENUMID])
REFERENCES [dbo].[BusinessEnum] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BusinessEnumValue] CHECK CONSTRAINT [FK_BUSINESS_ENUM_VALUE]
GO
ALTER TABLE [dbo].[BusinessExtraScript]  WITH CHECK ADD  CONSTRAINT [FK_BUSINESS_PACKAGE_SCRIPT] FOREIGN KEY([PackageID])
REFERENCES [dbo].[BusinessPackage] ([ID])
GO
ALTER TABLE [dbo].[BusinessExtraScript] CHECK CONSTRAINT [FK_BUSINESS_PACKAGE_SCRIPT]
GO
ALTER TABLE [dbo].[BusinessObject]  WITH CHECK ADD  CONSTRAINT [FK_BUSINESS_CLASS_OBJECT] FOREIGN KEY([CLASSID])
REFERENCES [dbo].[BusinessClass] ([ID])
GO
ALTER TABLE [dbo].[BusinessObject] CHECK CONSTRAINT [FK_BUSINESS_CLASS_OBJECT]
GO
ALTER TABLE [dbo].[BusinessPackage]  WITH CHECK ADD  CONSTRAINT [FK_BUSINESS_CHILDPACKAGES] FOREIGN KEY([ParentPackageID])
REFERENCES [dbo].[BusinessPackage] ([ID])
GO
ALTER TABLE [dbo].[BusinessPackage] CHECK CONSTRAINT [FK_BUSINESS_CHILDPACKAGES]
GO
ALTER TABLE [dbo].[BusinessProperty]  WITH CHECK ADD  CONSTRAINT [FK_BUSINESS_CLASS_PROPERTY] FOREIGN KEY([CLASSID])
REFERENCES [dbo].[BusinessClass] ([ID])
GO
ALTER TABLE [dbo].[BusinessProperty] CHECK CONSTRAINT [FK_BUSINESS_CLASS_PROPERTY]
GO
ALTER TABLE [dbo].[BusinessProperty]  WITH CHECK ADD  CONSTRAINT [FK_BUSINESS_DATATYPE_PROPERTY] FOREIGN KEY([DataTypeID])
REFERENCES [dbo].[BusinessDataType] ([ID])
GO
ALTER TABLE [dbo].[BusinessProperty] CHECK CONSTRAINT [FK_BUSINESS_DATATYPE_PROPERTY]
GO
ALTER TABLE [dbo].[BusinessProperty]  WITH CHECK ADD  CONSTRAINT [FK_BUSINESS_PROPTYPE_CLASS] FOREIGN KEY([ClassTypeID])
REFERENCES [dbo].[BusinessClass] ([ID])
GO
ALTER TABLE [dbo].[BusinessProperty] CHECK CONSTRAINT [FK_BUSINESS_PROPTYPE_CLASS]
GO
ALTER TABLE [dbo].[BusinessProperty]  WITH CHECK ADD  CONSTRAINT [FK_BUSINESS_PROPTYPE_ENUM] FOREIGN KEY([EnumTypeID])
REFERENCES [dbo].[BusinessEnum] ([ID])
GO
ALTER TABLE [dbo].[BusinessProperty] CHECK CONSTRAINT [FK_BUSINESS_PROPTYPE_ENUM]
GO
ALTER TABLE [dbo].[BusinessRole]  WITH CHECK ADD  CONSTRAINT [FK_BUSINESS_PACKAGE_ROLE] FOREIGN KEY([PACKAGEID])
REFERENCES [dbo].[BusinessPackage] ([ID])
GO
ALTER TABLE [dbo].[BusinessRole] CHECK CONSTRAINT [FK_BUSINESS_PACKAGE_ROLE]
GO
ALTER TABLE [dbo].[BusinessUser]  WITH CHECK ADD  CONSTRAINT [FK_BUSINESS_PACKAGE_USER] FOREIGN KEY([PACKAGEID])
REFERENCES [dbo].[BusinessPackage] ([ID])
GO
ALTER TABLE [dbo].[BusinessUser] CHECK CONSTRAINT [FK_BUSINESS_PACKAGE_USER]
GO
ALTER TABLE [dbo].[BusinessUserRole]  WITH CHECK ADD  CONSTRAINT [FK_BUSINESS_ROLE_USERS] FOREIGN KEY([ROLEID])
REFERENCES [dbo].[BusinessRole] ([ID])
GO
ALTER TABLE [dbo].[BusinessUserRole] CHECK CONSTRAINT [FK_BUSINESS_ROLE_USERS]
GO
ALTER TABLE [dbo].[BusinessUserRole]  WITH CHECK ADD  CONSTRAINT [FK_BUSINESS_USER_ROLES] FOREIGN KEY([USERID])
REFERENCES [dbo].[BusinessUser] ([ID])
GO
ALTER TABLE [dbo].[BusinessUserRole] CHECK CONSTRAINT [FK_BUSINESS_USER_ROLES]
GO
USE [master]
GO
ALTER DATABASE [Fantasy] SET  READ_WRITE 
GO
