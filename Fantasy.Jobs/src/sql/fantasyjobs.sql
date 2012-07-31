drop table  CV_JOB_JOBS;

CREATE TABLE FTS_JOB_JOBS(
	Id CHAR(16) FOR BIT DATA NOT NULL,
	ParentId  CHAR(16) FOR BIT DATA,
	Template varchar(100) NOT NULL,
	Name varchar (100) NOT NULL,
	State integer NOT NULL,
	Priority integer NOT NULL,
	StartTime timestamp,
	EndTime timestamp,
	CreationTime timestamp NOT NULL,
	Application varchar (100) NOT NULL,
	User_ varchar (100) NOT NULL,
	StartInfo varchar (32672),
	Tag varchar (1024),
	CONSTRAINT PK_CV_JOB_JOBS  PRIMARY KEY 
	(
		Id
	)
);


drop table  CV_JOB_ARCHIVEDJOBS;

CREATE TABLE FTS_JOB_ARCHIVEDJOBS(
	Id CHAR(16) FOR BIT DATA NOT NULL,
	ParentId  CHAR(16) FOR BIT DATA,
	Template varchar(100) NOT NULL,
	Name varchar (100) NOT NULL,
	State integer NOT NULL,
	Priority integer NOT NULL,
	StartTime timestamp,
	EndTime timestamp,
	CreationTime timestamp NOT NULL,
	Application varchar (100) NOT NULL,
	User_ varchar (100) NOT NULL,
	StartInfo varchar (32672),
	Tag varchar (1024),
	CONSTRAINT PK_CV_JOB_ARCHIVEDJOBS PRIMARY KEY 
	(
		Id
	)
);


