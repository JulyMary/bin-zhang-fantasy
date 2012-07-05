
CREATE TABLE CV_JOB_JOBS(
	[Id] [uniqueidentifier] NOT NULL,
	[ParentId] [uniqueidentifier] NULL,
	[Template] [nvarchar](100) NOT NULL,
	[Name] [nvarchar] (100) NOT NULL,
	[State] [int] NOT NULL,
	[Priority] [int] NOT NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[CreationTime] [datetime] NOT NULL,
	[Application] [nvarchar] (100) NOT NULL,
	[User] [nvarchar] (100) NOT NULL,
	[StartInfo] [ntext] NULL,
	[Tag] [nvarchar] (1024)
	 CONSTRAINT [PK_CV_JOB_JOBS] PRIMARY KEY
	(
		Id
	)
);


CREATE TABLE CV_JOB_ARCHIVEDJOBS(
	[Id] [uniqueidentifier] NOT NULL,
	[ParentId] [uniqueidentifier] NULL,
	[Template] [nvarchar](100) NOT NULL,
	[Name] [nvarchar] (100) NOT NULL,
	[State] [int] NOT NULL,
	[Priority] [int] NOT NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[CreationTime] [datetime] NOT NULL,
	[Application] [nvarchar] (100) NOT NULL,
	[User] [nvarchar] (100) NOT NULL,
	[StartInfo] [ntext] NULL,
	[Tag] [nvarchar] (1024)
	 CONSTRAINT [PK_CV_JOB_ARCHIVEDJOBS] PRIMARY KEY
	(
		Id
	)
);