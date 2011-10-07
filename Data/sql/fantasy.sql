/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2008                    */
/* Created on:     7/10/2011 10:26:33 AM                        */
/*==============================================================*/


if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('ASSEMBLYREFERENCE') and o.name = 'FK_ASSEMBLYREFERENCEGROUP')
alter table ASSEMBLYREFERENCE
   drop constraint FK_ASSEMBLYREFERENCEGROUP
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BUSINESSAPPLICATION') and o.name = 'FK_BUSINESSAPPLICATION_ENTRYOBJECT')
alter table BUSINESSAPPLICATION
   drop constraint FK_BUSINESSAPPLICATION_ENTRYOBJECT
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BUSINESSAPPLICATION') and o.name = 'FK_BUSINESSAPPLICATION_PACKAGE')
alter table BUSINESSAPPLICATION
   drop constraint FK_BUSINESSAPPLICATION_PACKAGE
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BUSINESSAPPLICATIONACL') and o.name = 'FK_BUSINESS_APPLICATIONACL_PARTICIPANT')
alter table BUSINESSAPPLICATIONACL
   drop constraint FK_BUSINESS_APPLICATIONACL_PARTICIPANT
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BUSINESSAPPLICATIONACL') and o.name = 'FK_BUSINESS_APPLICATIONACL_STATE')
alter table BUSINESSAPPLICATIONACL
   drop constraint FK_BUSINESS_APPLICATIONACL_STATE
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BUSINESSAPPLICATIONACL') and o.name = 'FK_BUSINESS_APPLICATIONACL_ROLE')
alter table BUSINESSAPPLICATIONACL
   drop constraint FK_BUSINESS_APPLICATIONACL_ROLE
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BUSINESSAPPLICATIONPARTICIPANT') and o.name = 'FK_BUSINESSPARICIPANT_CLASS')
alter table BUSINESSAPPLICATIONPARTICIPANT
   drop constraint FK_BUSINESSPARICIPANT_CLASS
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BUSINESSAPPLICATIONPARTICIPANT') and o.name = 'FK_BUSINESS_PATICIPANT_APPLICATION')
alter table BUSINESSAPPLICATIONPARTICIPANT
   drop constraint FK_BUSINESS_PATICIPANT_APPLICATION
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BUSINESSASSOCIATION') and o.name = 'FK_PACKAGEASSOCIATIONS')
alter table BUSINESSASSOCIATION
   drop constraint FK_PACKAGEASSOCIATIONS
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BUSINESSASSOCIATION') and o.name = 'FK_BUSINESS_RELATION_CLASS_LEFT')
alter table BUSINESSASSOCIATION
   drop constraint FK_BUSINESS_RELATION_CLASS_LEFT
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BUSINESSASSOCIATION') and o.name = 'FK_BUSINESS_RELATION_CLASS_RIGHT')
alter table BUSINESSASSOCIATION
   drop constraint FK_BUSINESS_RELATION_CLASS_RIGHT
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BUSINESSCLASS') and o.name = 'FK_BUSINESS_PACKAGE_CLASS')
alter table BUSINESSCLASS
   drop constraint FK_BUSINESS_PACKAGE_CLASS
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BUSINESSCLASS') and o.name = 'FK_BUSINESS_CLASS_PRENTCHILDREN')
alter table BUSINESSCLASS
   drop constraint FK_BUSINESS_CLASS_PRENTCHILDREN
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BUSINESSCLASSDIAGRAM') and o.name = 'FK_BUSINESS_PACKAGECL_BUSINESS')
alter table BUSINESSCLASSDIAGRAM
   drop constraint FK_BUSINESS_PACKAGECL_BUSINESS
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BUSINESSENUM') and o.name = 'FK_BUSINESS_PACKAGE_ENUM')
alter table BUSINESSENUM
   drop constraint FK_BUSINESS_PACKAGE_ENUM
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BUSINESSENUMVALUE') and o.name = 'FK_BUSNIESS_ENUM_ENUMVALUES')
alter table BUSINESSENUMVALUE
   drop constraint FK_BUSNIESS_ENUM_ENUMVALUES
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BUSINESSEXTRASCRIPT') and o.name = 'FK_BUSINESS_PACKAGE_SCRIPT')
alter table BUSINESSEXTRASCRIPT
   drop constraint FK_BUSINESS_PACKAGE_SCRIPT
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BUSINESSOBJECT') and o.name = 'FK_BUSINESS_BUSINESSC_BUSINESS')
alter table BUSINESSOBJECT
   drop constraint FK_BUSINESS_BUSINESSC_BUSINESS
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BUSINESSPACKAGE') and o.name = 'FK_BUSINESS_PACKAGE_PARENTCHILDREN')
alter table BUSINESSPACKAGE
   drop constraint FK_BUSINESS_PACKAGE_PARENTCHILDREN
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BUSINESSPROPERTY') and o.name = 'FK_BUSINESS_CLASS_PROPERTIES')
alter table BUSINESSPROPERTY
   drop constraint FK_BUSINESS_CLASS_PROPERTIES
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BUSINESSPROPERTY') and o.name = 'FK_BUSINESS_CLASS_PROPERTYTYPE')
alter table BUSINESSPROPERTY
   drop constraint FK_BUSINESS_CLASS_PROPERTYTYPE
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BUSINESSPROPERTY') and o.name = 'FK_BUSINESS_ENUM_PROPERTYTYPE')
alter table BUSINESSPROPERTY
   drop constraint FK_BUSINESS_ENUM_PROPERTYTYPE
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BUSINESSPROPERTY') and o.name = 'FK_BUSINESS_TYPE_PROPERTY')
alter table BUSINESSPROPERTY
   drop constraint FK_BUSINESS_TYPE_PROPERTY
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BUSINESSROLE') and o.name = 'FK_BUSINESS_ROLE_PACKAGE')
alter table BUSINESSROLE
   drop constraint FK_BUSINESS_ROLE_PACKAGE
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BUSINESSUSER') and o.name = 'FK_BUSINESS_USER_PACKAGE')
alter table BUSINESSUSER
   drop constraint FK_BUSINESS_USER_PACKAGE
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('ASSEMBLYREFERENCE')
            and   name  = 'GROUPASSEMBLY_FK'
            and   indid > 0
            and   indid < 255)
   drop index ASSEMBLYREFERENCE.GROUPASSEMBLY_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('ASSEMBLYREFERENCE')
            and   type = 'U')
   drop table ASSEMBLYREFERENCE
go

if exists (select 1
            from  sysobjects
           where  id = object_id('ASSEMBLYREFERENCEGROUP')
            and   type = 'U')
   drop table ASSEMBLYREFERENCEGROUP
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BUSINESSAPPLICATION')
            and   name  = 'BUSINESSPACKAGEAPPLICATIONS_FK'
            and   indid > 0
            and   indid < 255)
   drop index BUSINESSAPPLICATION.BUSINESSPACKAGEAPPLICATIONS_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BUSINESSAPPLICATION')
            and   name  = 'BUSINESSAPPLICATIONENTRYOBJECT_FK'
            and   indid > 0
            and   indid < 255)
   drop index BUSINESSAPPLICATION.BUSINESSAPPLICATIONENTRYOBJECT_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BUSINESSAPPLICATION')
            and   type = 'U')
   drop table BUSINESSAPPLICATION
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BUSINESSAPPLICATIONACL')
            and   name  = 'APPLICATIONPARTICIPANTACL_FK'
            and   indid > 0
            and   indid < 255)
   drop index BUSINESSAPPLICATIONACL.APPLICATIONPARTICIPANTACL_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BUSINESSAPPLICATIONACL')
            and   name  = 'BUSINESSAPPLICATIONACLSTATE_FK'
            and   indid > 0
            and   indid < 255)
   drop index BUSINESSAPPLICATIONACL.BUSINESSAPPLICATIONACLSTATE_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BUSINESSAPPLICATIONACL')
            and   name  = 'CLASSACCESSCONTROLROLE_FK'
            and   indid > 0
            and   indid < 255)
   drop index BUSINESSAPPLICATIONACL.CLASSACCESSCONTROLROLE_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BUSINESSAPPLICATIONACL')
            and   type = 'U')
   drop table BUSINESSAPPLICATIONACL
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BUSINESSAPPLICATIONPARTICIPANT')
            and   name  = 'PARTICIPANTCLASS_FK'
            and   indid > 0
            and   indid < 255)
   drop index BUSINESSAPPLICATIONPARTICIPANT.PARTICIPANTCLASS_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BUSINESSAPPLICATIONPARTICIPANT')
            and   name  = 'PATICIPANTAPPLICATION_FK'
            and   indid > 0
            and   indid < 255)
   drop index BUSINESSAPPLICATIONPARTICIPANT.PATICIPANTAPPLICATION_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BUSINESSAPPLICATIONPARTICIPANT')
            and   type = 'U')
   drop table BUSINESSAPPLICATIONPARTICIPANT
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BUSINESSASSOCIATION')
            and   name  = 'PACKAGEASSOCIATIONS_FK'
            and   indid > 0
            and   indid < 255)
   drop index BUSINESSASSOCIATION.PACKAGEASSOCIATIONS_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BUSINESSASSOCIATION')
            and   name  = 'RELATIONSHIPRIGHT_FK'
            and   indid > 0
            and   indid < 255)
   drop index BUSINESSASSOCIATION.RELATIONSHIPRIGHT_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BUSINESSASSOCIATION')
            and   name  = 'RELATIONSHIPLEFT_FK'
            and   indid > 0
            and   indid < 255)
   drop index BUSINESSASSOCIATION.RELATIONSHIPLEFT_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BUSINESSASSOCIATION')
            and   type = 'U')
   drop table BUSINESSASSOCIATION
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BUSINESSCLASS')
            and   name  = 'IDX_PACKAGECLASSES_FK'
            and   indid > 0
            and   indid < 255)
   drop index BUSINESSCLASS.IDX_PACKAGECLASSES_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BUSINESSCLASS')
            and   name  = 'IDX_PARENTCLASS_FK'
            and   indid > 0
            and   indid < 255)
   drop index BUSINESSCLASS.IDX_PARENTCLASS_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BUSINESSCLASS')
            and   type = 'U')
   drop table BUSINESSCLASS
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BUSINESSCLASSDIAGRAM')
            and   name  = 'PACKAGECLASSDIAGRAM_FK'
            and   indid > 0
            and   indid < 255)
   drop index BUSINESSCLASSDIAGRAM.PACKAGECLASSDIAGRAM_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BUSINESSCLASSDIAGRAM')
            and   type = 'U')
   drop table BUSINESSCLASSDIAGRAM
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BUSINESSDATATYPE')
            and   type = 'U')
   drop table BUSINESSDATATYPE
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BUSINESSENUM')
            and   name  = 'IDX_PACKAGEENUM_FK'
            and   indid > 0
            and   indid < 255)
   drop index BUSINESSENUM.IDX_PACKAGEENUM_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BUSINESSENUM')
            and   type = 'U')
   drop table BUSINESSENUM
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BUSINESSENUMVALUE')
            and   name  = 'IDX_BUSINESSENUMVALUES_FK'
            and   indid > 0
            and   indid < 255)
   drop index BUSINESSENUMVALUE.IDX_BUSINESSENUMVALUES_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BUSINESSENUMVALUE')
            and   type = 'U')
   drop table BUSINESSENUMVALUE
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BUSINESSEXTRASCRIPT')
            and   name  = 'PACKAGEEXTRASCRIPTS_FK'
            and   indid > 0
            and   indid < 255)
   drop index BUSINESSEXTRASCRIPT.PACKAGEEXTRASCRIPTS_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BUSINESSEXTRASCRIPT')
            and   type = 'U')
   drop table BUSINESSEXTRASCRIPT
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BUSINESSOBJECT')
            and   name  = 'BUSINESSCLASSOBJECT_FK'
            and   indid > 0
            and   indid < 255)
   drop index BUSINESSOBJECT.BUSINESSCLASSOBJECT_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BUSINESSOBJECT')
            and   type = 'U')
   drop table BUSINESSOBJECT
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BUSINESSPACKAGE')
            and   name  = 'IDX_CHILDPACKAGES_FK'
            and   indid > 0
            and   indid < 255)
   drop index BUSINESSPACKAGE.IDX_CHILDPACKAGES_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BUSINESSPACKAGE')
            and   type = 'U')
   drop table BUSINESSPACKAGE
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BUSINESSPROPERTY')
            and   name  = 'IDX_PROPERTYCLASSTYPE_FK'
            and   indid > 0
            and   indid < 255)
   drop index BUSINESSPROPERTY.IDX_PROPERTYCLASSTYPE_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BUSINESSPROPERTY')
            and   name  = 'IDX_PROPERTYTYPE_FK'
            and   indid > 0
            and   indid < 255)
   drop index BUSINESSPROPERTY.IDX_PROPERTYTYPE_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BUSINESSPROPERTY')
            and   name  = 'IDX_PROPERTYENUMTYPE_FK'
            and   indid > 0
            and   indid < 255)
   drop index BUSINESSPROPERTY.IDX_PROPERTYENUMTYPE_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BUSINESSPROPERTY')
            and   name  = 'IDX_PROPERTIES_FK'
            and   indid > 0
            and   indid < 255)
   drop index BUSINESSPROPERTY.IDX_PROPERTIES_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BUSINESSPROPERTY')
            and   type = 'U')
   drop table BUSINESSPROPERTY
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BUSINESSROLE')
            and   name  = 'ROLE_PACKAGE_FK'
            and   indid > 0
            and   indid < 255)
   drop index BUSINESSROLE.ROLE_PACKAGE_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BUSINESSROLE')
            and   type = 'U')
   drop table BUSINESSROLE
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BUSINESSUSER')
            and   name  = 'USER_PACKAGE_FK'
            and   indid > 0
            and   indid < 255)
   drop index BUSINESSUSER.USER_PACKAGE_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BUSINESSUSER')
            and   type = 'U')
   drop table BUSINESSUSER
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BUSINESSUSERROLE')
            and   name  = 'BUSINESSUSERROLE_FK2'
            and   indid > 0
            and   indid < 255)
   drop index BUSINESSUSERROLE.BUSINESSUSERROLE_FK2
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BUSINESSUSERROLE')
            and   name  = 'BUSINESSUSERROLE_FK'
            and   indid > 0
            and   indid < 255)
   drop index BUSINESSUSERROLE.BUSINESSUSERROLE_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BUSINESSUSERROLE')
            and   type = 'U')
   drop table BUSINESSUSERROLE
go

if exists(select 1 from systypes where name='T_DBENTITY')
   drop type T_DBENTITY
go

if exists(select 1 from systypes where name='T_GUID')
   drop type T_GUID
go

if exists(select 1 from systypes where name='T_NAME')
   drop type T_NAME
go

/*==============================================================*/
/* Domain: T_DBENTITY                                           */
/*==============================================================*/
create type T_DBENTITY
   from varchar(64)
go

/*==============================================================*/
/* Domain: T_GUID                                               */
/*==============================================================*/
create type T_GUID
   from uniqueidentifier
go

/*==============================================================*/
/* Domain: T_NAME                                               */
/*==============================================================*/
create type T_NAME
   from varchar(64)
go

/*==============================================================*/
/* Table: ASSEMBLYREFERENCE                                     */
/*==============================================================*/
create table ASSEMBLYREFERENCE (
   ID                   T_GUID               not null,
   GROUPID              T_GUID               not null,
   MODIFICATIONTIME     datetime             not null,
   CREATIONTIME         datetime             not null,
   ISSYSTEM             bit                  not null,
   FULLNAME             varchar(255)         not null,
   RAWASSEMBLY          image                null,
   RAWHASH              varchar(32)          null,
   SOURCE               int                  not null,
   constraint PK_ASSEMBLYREFERENCE primary key (ID)
)
go

/*==============================================================*/
/* Index: GROUPASSEMBLY_FK                                      */
/*==============================================================*/
create index GROUPASSEMBLY_FK on ASSEMBLYREFERENCE (
GROUPID ASC
)
go

/*==============================================================*/
/* Table: ASSEMBLYREFERENCEGROUP                                */
/*==============================================================*/
create table ASSEMBLYREFERENCEGROUP (
   ID                   T_GUID               not null,
   MODIFICATIONTIME     datetime             not null,
   CREATIONTIME         datetime             not null,
   ISSYSTEM             bit                  not null,
   constraint PK_ASSEMBLYREFERENCEGROUP primary key (ID)
)
go

/*==============================================================*/
/* Table: BUSINESSAPPLICATION                                   */
/*==============================================================*/
create table BUSINESSAPPLICATION (
   ID                   T_GUID               not null,
   MODIFICATIONTIME     datetime             not null,
   CREATIONTIME         datetime             not null,
   ISSYSTEM             bit                  not null,
   PACKAGEID            T_GUID               not null,
   ENTRYOBJECTID        T_GUID               null,
   NAME                 T_NAME               not null,
   CODENAME             T_NAME               not null,
   SCRIPT               text                 null,
   constraint PK_BUSINESSAPPLICATION primary key (ID)
)
go

/*==============================================================*/
/* Index: BUSINESSAPPLICATIONENTRYOBJECT_FK                     */
/*==============================================================*/
create index BUSINESSAPPLICATIONENTRYOBJECT_FK on BUSINESSAPPLICATION (
ENTRYOBJECTID ASC
)
go

/*==============================================================*/
/* Index: BUSINESSPACKAGEAPPLICATIONS_FK                        */
/*==============================================================*/
create index BUSINESSPACKAGEAPPLICATIONS_FK on BUSINESSAPPLICATION (
PACKAGEID ASC
)
go

/*==============================================================*/
/* Table: BUSINESSAPPLICATIONACL                                */
/*==============================================================*/
create table BUSINESSAPPLICATIONACL (
   ID                   T_GUID               not null,
   STATEID              T_GUID               null,
   ROLEID               T_GUID               not null,
   PARTICIPANTID        T_GUID               not null,
   BUS_ID               T_GUID               null,
   MODIFICATIONTIME     datetime             not null,
   CREATIONTIME         datetime             not null,
   ISSYSTEM             bit                  not null,
   ACL                  text                 null,
   constraint PK_BUSINESSAPPLICATIONACL primary key (ID)
)
go

/*==============================================================*/
/* Index: CLASSACCESSCONTROLROLE_FK                             */
/*==============================================================*/
create index CLASSACCESSCONTROLROLE_FK on BUSINESSAPPLICATIONACL (
BUS_ID ASC
)
go

/*==============================================================*/
/* Index: BUSINESSAPPLICATIONACLSTATE_FK                        */
/*==============================================================*/
create index BUSINESSAPPLICATIONACLSTATE_FK on BUSINESSAPPLICATIONACL (
STATEID ASC
)
go

/*==============================================================*/
/* Index: APPLICATIONPARTICIPANTACL_FK                          */
/*==============================================================*/
create index APPLICATIONPARTICIPANTACL_FK on BUSINESSAPPLICATIONACL (
PARTICIPANTID ASC
)
go

/*==============================================================*/
/* Table: BUSINESSAPPLICATIONPARTICIPANT                        */
/*==============================================================*/
create table BUSINESSAPPLICATIONPARTICIPANT (
   ID                   T_GUID               not null,
   CLASSID              T_GUID               not null,
   APPLICATIONID        T_GUID               not null,
   MODIFICATIONTIME     datetime             not null,
   CREATIONTIME         datetime             not null,
   ISSYSTEM             bit                  not null,
   ISENTRY              bit                  not null,
   constraint PK_BUSINESSAPPLICATIONPARTICIP primary key (ID)
)
go

/*==============================================================*/
/* Index: PATICIPANTAPPLICATION_FK                              */
/*==============================================================*/
create index PATICIPANTAPPLICATION_FK on BUSINESSAPPLICATIONPARTICIPANT (
APPLICATIONID ASC
)
go

/*==============================================================*/
/* Index: PARTICIPANTCLASS_FK                                   */
/*==============================================================*/
create index PARTICIPANTCLASS_FK on BUSINESSAPPLICATIONPARTICIPANT (
CLASSID ASC
)
go

/*==============================================================*/
/* Table: BUSINESSASSOCIATION                                   */
/*==============================================================*/
create table BUSINESSASSOCIATION (
   ID                   T_GUID               not null,
   PACKAGEID            T_GUID               null,
   NAME                 T_NAME               not null,
   CODENAME             T_NAME               not null,
   TABLENAME            T_DBENTITY           not null,
   TABLESCHEMA          T_DBENTITY           not null,
   TABLESPACE           T_DBENTITY           null,
   LEFTROLENAME         T_NAME               null,
   LEFTROLECODE         T_NAME               null,
   LEFTCLASSID          T_GUID               null,
   LEFTCARDINALITY      varchar(16)          not null,
   LEFTNAVIGATABLE      bit                  not null,
   LEFTROLEDISPLAYORDER bigint               null,
   RIGHTROLENAME        T_NAME               null,
   RIGHTROLECODE        T_NAME               null,
   RIGHTCLASSID         T_GUID               null,
   RIGHTCARDINALITY     varchar(16)          not null,
   RIGHTNAVIGATABLE     bit                  not null,
   RIGHTROLEDISPLAYORDER bigint               null,
   CREATIONTIME         datetime             not null,
   MODIFICATIONTIME     datetime             not null,
   ISSYSTEM             bit                  not null,
   constraint PK_BUSINESSASSOCIATION primary key (ID)
)
go

/*==============================================================*/
/* Index: RELATIONSHIPLEFT_FK                                   */
/*==============================================================*/
create index RELATIONSHIPLEFT_FK on BUSINESSASSOCIATION (
LEFTCLASSID ASC
)
go

/*==============================================================*/
/* Index: RELATIONSHIPRIGHT_FK                                  */
/*==============================================================*/
create index RELATIONSHIPRIGHT_FK on BUSINESSASSOCIATION (
RIGHTCLASSID ASC
)
go

/*==============================================================*/
/* Index: PACKAGEASSOCIATIONS_FK                                */
/*==============================================================*/
create index PACKAGEASSOCIATIONS_FK on BUSINESSASSOCIATION (
PACKAGEID ASC
)
go

/*==============================================================*/
/* Table: BUSINESSCLASS                                         */
/*==============================================================*/
create table BUSINESSCLASS (
   ID                   T_GUID               not null,
   NAME                 T_NAME               not null,
   CODENAME             T_NAME               not null,
   PACKAGEID            T_GUID               not null,
   PARENTCLASSID        T_GUID               null,
   TABLENAME            T_DBENTITY           not null,
   TABLESCHEMA          T_DBENTITY           not null,
   TABLESPACE           T_DBENTITY           null,
   CREATIONTIME         datetime             not null,
   MODIFICATIONTIME     datetime             not null,
   ISSYSTEM             bit                  not null,
   ISSIMPLE             bit                  not null,
   IMPLEMENTS           varchar(4096)        null,
   AUTOSCRIPT           text                 null,
   SCRIPT               text                 null,
   SCRIPTOPTIONS        int                  null,
   ISABSTRACT           bit                  not null,
   constraint PK_BUSINESSCLASS primary key (ID)
)
go

/*==============================================================*/
/* Index: IDX_PARENTCLASS_FK                                    */
/*==============================================================*/
create index IDX_PARENTCLASS_FK on BUSINESSCLASS (
PARENTCLASSID ASC
)
go

/*==============================================================*/
/* Index: IDX_PACKAGECLASSES_FK                                 */
/*==============================================================*/
create index IDX_PACKAGECLASSES_FK on BUSINESSCLASS (
PACKAGEID ASC
)
go

/*==============================================================*/
/* Table: BUSINESSCLASSDIAGRAM                                  */
/*==============================================================*/
create table BUSINESSCLASSDIAGRAM (
   ID                   T_GUID               not null,
   PACKAGEID            T_GUID               not null,
   MODIFICATIONTIME     datetime             not null,
   CREATIONTIME         datetime             not null,
   ISSYSTEM             bit                  not null,
   NAME                 T_NAME               not null,
   DIAGRAM              text                 null,
   constraint PK_BUSINESSCLASSDIAGRAM primary key (PACKAGEID, ID)
)
go

/*==============================================================*/
/* Index: PACKAGECLASSDIAGRAM_FK                                */
/*==============================================================*/
create index PACKAGECLASSDIAGRAM_FK on BUSINESSCLASSDIAGRAM (
PACKAGEID ASC
)
go

/*==============================================================*/
/* Table: BUSINESSDATATYPE                                      */
/*==============================================================*/
create table BUSINESSDATATYPE (
   ID                   T_GUID               not null,
   NAME                 T_NAME               not null,
   CODENAME             T_NAME               not null,
   DEFAULTDBTYPE        T_NAME               null,
   DEFAULTLENGTH        int                  null,
   DEFAULTPRECISION     int                  null,
   MODIFICATIONTIME     datetime             not null,
   CREATIONTIME         datetime             not null,
   ISSYSTEM             bit                  not null,
   constraint PK_BUSINESSDATATYPE primary key (ID)
)
go

/*==============================================================*/
/* Table: BUSINESSENUM                                          */
/*==============================================================*/
create table BUSINESSENUM (
   ID                   T_GUID               not null,
   NAME                 T_NAME               not null,
   CODENAME             T_NAME               not null,
   PACKAGEID            T_GUID               not null,
   ISFLAGS              bit                  not null,
   ISEXTERNAL           bit                  null,
   EXTERNALASSEMBLY     T_NAME               null,
   EXTERNALNAMESPACE    varchar(255)         null,
   CREATIONTIME         datetime             not null,
   MODIFICATIONTIME     datetime             not null,
   ISSYSTEM             bit                  not null,
   constraint PK_BUSINESSENUM primary key (ID)
)
go

/*==============================================================*/
/* Index: IDX_PACKAGEENUM_FK                                    */
/*==============================================================*/
create index IDX_PACKAGEENUM_FK on BUSINESSENUM (
PACKAGEID ASC
)
go

/*==============================================================*/
/* Table: BUSINESSENUMVALUE                                     */
/*==============================================================*/
create table BUSINESSENUMVALUE (
   ID                   T_GUID               not null,
   ENUMID               T_GUID               not null,
   NAME                 T_NAME               not null,
   CODENAME             T_NAME               not null,
   VALUE                int                  not null,
   CREATIONTIME         datetime             not null,
   MODIFICATIONTIME     datetime             not null,
   ISSYSTEM             bit                  not null,
   constraint PK_BUSINESSENUMVALUE primary key (ID)
)
go

/*==============================================================*/
/* Index: IDX_BUSINESSENUMVALUES_FK                             */
/*==============================================================*/
create index IDX_BUSINESSENUMVALUES_FK on BUSINESSENUMVALUE (
ENUMID ASC
)
go

/*==============================================================*/
/* Table: BUSINESSEXTRASCRIPT                                   */
/*==============================================================*/
create table BUSINESSEXTRASCRIPT (
   ID                   T_GUID               not null,
   PACKAGEID            T_GUID               not null,
   MODIFICATIONTIME     datetime             not null,
   CREATIONTIME         datetime             not null,
   ISSYSTEM             bit                  not null,
   NAME                 T_NAME               not null,
   PROJECTTYPE          int                  not null,
   SCRIPT               text                 null,
   BUILDACTION          T_NAME               null,
   METADATA             varchar(1024)        null,
   constraint PK_BUSINESSEXTRASCRIPT primary key (ID)
)
go

/*==============================================================*/
/* Index: PACKAGEEXTRASCRIPTS_FK                                */
/*==============================================================*/
create index PACKAGEEXTRASCRIPTS_FK on BUSINESSEXTRASCRIPT (
PACKAGEID ASC
)
go

/*==============================================================*/
/* Table: BUSINESSOBJECT                                        */
/*==============================================================*/
create table BUSINESSOBJECT (
   ID                   T_GUID               not null,
   CLASSID              T_GUID               not null,
   NAME                 T_NAME               null,
   MODIFICATIONTIME     datetime             not null,
   CREATIONTIME         datetime             not null,
   ISSYSTEM             bit                  not null,
   constraint PK_BUSINESSOBJECT primary key nonclustered (ID)
)
go

/*==============================================================*/
/* Index: BUSINESSCLASSOBJECT_FK                                */
/*==============================================================*/
create index BUSINESSCLASSOBJECT_FK on BUSINESSOBJECT (
CLASSID ASC
)
go

/*==============================================================*/
/* Table: BUSINESSPACKAGE                                       */
/*==============================================================*/
create table BUSINESSPACKAGE (
   ID                   T_GUID               not null,
   PARENTPACKAGEID      T_GUID               null,
   NAME                 T_NAME               not null,
   CODENAME             T_NAME               not null,
   MODIFICATIONTIME     datetime             not null,
   CREATIONTIME         datetime             not null,
   ISSYSTEM             bit                  not null,
   constraint PK_BUSINESSPACKAGE primary key (ID)
)
go

/*==============================================================*/
/* Index: IDX_CHILDPACKAGES_FK                                  */
/*==============================================================*/
create index IDX_CHILDPACKAGES_FK on BUSINESSPACKAGE (
PARENTPACKAGEID ASC
)
go

/*==============================================================*/
/* Table: BUSINESSPROPERTY                                      */
/*==============================================================*/
create table BUSINESSPROPERTY (
   ID                   T_GUID               not null,
   NAME                 T_NAME               not null,
   CODENAME             T_NAME               not null,
   CLASSID              T_GUID               not null,
   DATATYPEID           T_GUID               null,
   ENUMTYPEID           T_GUID               null,
   CLASSTYPEID          T_GUID               null,
   FIELDNAME            T_DBENTITY           null,
   FIELDTYPE            T_DBENTITY           null,
   LENGTH               int                  null,
   "PRECISION"          int                  null,
   ISNULLABLE           bit                  not null,
   DEFAULTVALUE         varchar(64)          null,
   SCRIPTOPTIONS        int                  null,
   DISPLAYORDER         bigint               not null,
   ISCALCULATED         bit                  not null,
   CREATIONTIME         datetime             not null,
   MODIFICATIONTIME     datetime             not null,
   ISSYSTEM             bit                  not null,
   constraint PK_BUSINESSPROPERTY primary key (CLASSID, ID)
)
go

/*==============================================================*/
/* Index: IDX_PROPERTIES_FK                                     */
/*==============================================================*/
create index IDX_PROPERTIES_FK on BUSINESSPROPERTY (
CLASSID ASC
)
go

/*==============================================================*/
/* Index: IDX_PROPERTYENUMTYPE_FK                               */
/*==============================================================*/
create index IDX_PROPERTYENUMTYPE_FK on BUSINESSPROPERTY (
ENUMTYPEID ASC
)
go

/*==============================================================*/
/* Index: IDX_PROPERTYTYPE_FK                                   */
/*==============================================================*/
create index IDX_PROPERTYTYPE_FK on BUSINESSPROPERTY (
DATATYPEID ASC
)
go

/*==============================================================*/
/* Index: IDX_PROPERTYCLASSTYPE_FK                              */
/*==============================================================*/
create index IDX_PROPERTYCLASSTYPE_FK on BUSINESSPROPERTY (
CLASSTYPEID ASC
)
go

/*==============================================================*/
/* Table: BUSINESSROLE                                          */
/*==============================================================*/
create table BUSINESSROLE (
   ID                   T_GUID               not null,
   MODIFICATIONTIME     datetime             not null,
   CREATIONTIME         datetime             not null,
   ISSYSTEM             bit                  not null,
   PACKAGEID            T_GUID               not null,
   NAME                 T_NAME               not null,
   CODENAME             T_NAME               not null,
   DESCRIPTION          varchar(1024)        null,
   ISCOMPUTED           bit                  not null,
   ISDISABLED           bit                  null,
   SCRIPT               text                 null,
   constraint PK_BUSINESSROLE primary key (ID)
)
go

/*==============================================================*/
/* Index: ROLE_PACKAGE_FK                                       */
/*==============================================================*/
create index ROLE_PACKAGE_FK on BUSINESSROLE (
PACKAGEID ASC
)
go

/*==============================================================*/
/* Table: BUSINESSUSER                                          */
/*==============================================================*/
create table BUSINESSUSER (
   ID                   T_GUID               not null,
   MODIFICATIONTIME     datetime             not null,
   CREATIONTIME         datetime             not null,
   ISSYSTEM             bit                  not null,
   PACKAGEID            T_GUID               not null,
   NAME                 T_NAME               not null,
   CODENAME             T_NAME               not null,
   FULLNAME             T_NAME               not null,
   DESCRIPTION          varchar(1024)        null,
   PASSWORD             varchar(128)         null,
   ISDISABLED           bit                  not null,
   SCRIPT               text                 null,
   constraint PK_BUSINESSUSER primary key (ID)
)
go

/*==============================================================*/
/* Index: USER_PACKAGE_FK                                       */
/*==============================================================*/
create index USER_PACKAGE_FK on BUSINESSUSER (
PACKAGEID ASC
)
go

/*==============================================================*/
/* Table: BUSINESSUSERROLE                                      */
/*==============================================================*/
create table BUSINESSUSERROLE (
   USERID               T_GUID               not null,
   ROLEID               T_GUID               not null,
   constraint PK_BUSINESSUSERROLE primary key (USERID, ROLEID)
)
go

/*==============================================================*/
/* Index: BUSINESSUSERROLE_FK                                   */
/*==============================================================*/
create index BUSINESSUSERROLE_FK on BUSINESSUSERROLE (
ROLEID ASC
)
go

/*==============================================================*/
/* Index: BUSINESSUSERROLE_FK2                                  */
/*==============================================================*/
create index BUSINESSUSERROLE_FK2 on BUSINESSUSERROLE (
USERID ASC
)
go

alter table ASSEMBLYREFERENCE
   add constraint FK_ASSEMBLYREFERENCEGROUP foreign key (GROUPID)
      references ASSEMBLYREFERENCEGROUP (ID)
go

alter table BUSINESSAPPLICATION
   add constraint FK_BUSINESSAPPLICATION_ENTRYOBJECT foreign key (ENTRYOBJECTID)
      references BUSINESSOBJECT (ID)
go

alter table BUSINESSAPPLICATION
   add constraint FK_BUSINESSAPPLICATION_PACKAGE foreign key (PACKAGEID)
      references BUSINESSPACKAGE (ID)
go

alter table BUSINESSAPPLICATIONACL
   add constraint FK_BUSINESS_APPLICATIONACL_PARTICIPANT foreign key (PARTICIPANTID)
      references BUSINESSAPPLICATIONPARTICIPANT (ID)
         on delete cascade
go

alter table BUSINESSAPPLICATIONACL
   add constraint FK_BUSINESS_APPLICATIONACL_STATE foreign key (STATEID)
      references BUSINESSENUMVALUE (ID)
         on delete cascade
go

alter table BUSINESSAPPLICATIONACL
   add constraint FK_BUSINESS_APPLICATIONACL_ROLE foreign key (BUS_ID)
      references BUSINESSROLE (ID)
         on delete cascade
go

alter table BUSINESSAPPLICATIONPARTICIPANT
   add constraint FK_BUSINESSPARICIPANT_CLASS foreign key (CLASSID)
      references BUSINESSCLASS (ID)
         on delete cascade
go

alter table BUSINESSAPPLICATIONPARTICIPANT
   add constraint FK_BUSINESS_PATICIPANT_APPLICATION foreign key (APPLICATIONID)
      references BUSINESSAPPLICATION (ID)
         on delete cascade
go

alter table BUSINESSASSOCIATION
   add constraint FK_PACKAGEASSOCIATIONS foreign key (PACKAGEID)
      references BUSINESSPACKAGE (ID)
go

alter table BUSINESSASSOCIATION
   add constraint FK_BUSINESS_RELATION_CLASS_LEFT foreign key (LEFTCLASSID)
      references BUSINESSCLASS (ID)
go

alter table BUSINESSASSOCIATION
   add constraint FK_BUSINESS_RELATION_CLASS_RIGHT foreign key (RIGHTCLASSID)
      references BUSINESSCLASS (ID)
go

alter table BUSINESSCLASS
   add constraint FK_BUSINESS_PACKAGE_CLASS foreign key (PACKAGEID)
      references BUSINESSPACKAGE (ID)
go

alter table BUSINESSCLASS
   add constraint FK_BUSINESS_CLASS_PRENTCHILDREN foreign key (PARENTCLASSID)
      references BUSINESSCLASS (ID)
go

alter table BUSINESSCLASSDIAGRAM
   add constraint FK_BUSINESS_PACKAGECL_BUSINESS foreign key (PACKAGEID)
      references BUSINESSPACKAGE (ID)
         on delete cascade
go

alter table BUSINESSENUM
   add constraint FK_BUSINESS_PACKAGE_ENUM foreign key (PACKAGEID)
      references BUSINESSPACKAGE (ID)
go

alter table BUSINESSENUMVALUE
   add constraint FK_BUSNIESS_ENUM_ENUMVALUES foreign key (ENUMID)
      references BUSINESSENUM (ID)
         on delete cascade
go

alter table BUSINESSEXTRASCRIPT
   add constraint FK_BUSINESS_PACKAGE_SCRIPT foreign key (PACKAGEID)
      references BUSINESSPACKAGE (ID)
go

alter table BUSINESSOBJECT
   add constraint FK_BUSINESS_BUSINESSC_BUSINESS foreign key (CLASSID)
      references BUSINESSCLASS (ID)
go

alter table BUSINESSPACKAGE
   add constraint FK_BUSINESS_PACKAGE_PARENTCHILDREN foreign key (PARENTPACKAGEID)
      references BUSINESSPACKAGE (ID)
go

alter table BUSINESSPROPERTY
   add constraint FK_BUSINESS_CLASS_PROPERTIES foreign key (CLASSID)
      references BUSINESSCLASS (ID)
         on delete cascade
go

alter table BUSINESSPROPERTY
   add constraint FK_BUSINESS_CLASS_PROPERTYTYPE foreign key (CLASSTYPEID)
      references BUSINESSCLASS (ID)
go

alter table BUSINESSPROPERTY
   add constraint FK_BUSINESS_ENUM_PROPERTYTYPE foreign key (ENUMTYPEID)
      references BUSINESSENUM (ID)
go

alter table BUSINESSPROPERTY
   add constraint FK_BUSINESS_TYPE_PROPERTY foreign key (DATATYPEID)
      references BUSINESSDATATYPE (ID)
go

alter table BUSINESSROLE
   add constraint FK_BUSINESS_ROLE_PACKAGE foreign key (PACKAGEID)
      references BUSINESSPACKAGE (ID)
go

alter table BUSINESSUSER
   add constraint FK_BUSINESS_USER_PACKAGE foreign key (PACKAGEID)
      references BUSINESSPACKAGE (ID)
go

