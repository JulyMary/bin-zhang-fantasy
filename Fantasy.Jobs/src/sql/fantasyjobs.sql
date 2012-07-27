CREATE TABLE CV_JOB_ARCHIVEDJOBS(
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
	StartInfo clob (10k),
	Tag varchar (1024),
	PRIMARY KEY
	(
		Id
	)
)

CREATE TABLE CV_JOB_ARCHIVEDJOBS(
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
	StartInfo clob (10k),
	Tag varchar (1024),
	PRIMARY KEY
	(
		Id
	)
)


