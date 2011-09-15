/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2008                    */
/* Created on:     15/09/2011 5:05:12 PM                        */
/*==============================================================*/


if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('ASSEMBLYREFERENCE') and o.name = 'FK_ASSEMBLYREFERENCEGROUP')
alter table ASSEMBLYREFERENCE
   drop constraint FK_ASSEMBLYREFERENCEGROUP
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
   where r.fkeyid = object_id('BUSINESSOBJECT') and o.name = 'FK_BUSINESS_OBJECTCLA_BUSINESS')
alter table BUSINESSOBJECT
   drop constraint FK_BUSINESS_OBJECTCLA_BUSINESS
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
            from  sysindexes
           where  id    = object_id('ASSEMBLYREFERENCE')
            and   name  = 'GROUPASSEMBLY_FK'
            and   indid > 0
            and   indid < 255)
   drop index ASSEMBLYREFERENCE.GROUPASSEMBLY_FK
go

alter table ASSEMBLYREFERENCE
   drop constraint PK_ASSEMBLYREFERENCE
go

if exists (select 1
            from  sysobjects
           where  id = object_id('ASSEMBLYREFERENCE')
            and   type = 'U')
   drop table ASSEMBLYREFERENCE
go

alter table ASSEMBLYREFERENCEGROUP
   drop constraint PK_ASSEMBLYREFERENCEGROUP
go

if exists (select 1
            from  sysobjects
           where  id = object_id('ASSEMBLYREFERENCEGROUP')
            and   type = 'U')
   drop table ASSEMBLYREFERENCEGROUP
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

alter table BUSINESSASSOCIATION
   drop constraint PK_BUSINESSASSOCIATION
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

alter table BUSINESSCLASS
   drop constraint PK_BUSINESSCLASS
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

alter table BUSINESSCLASSDIAGRAM
   drop constraint PK_BUSINESSCLASSDIAGRAM
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BUSINESSCLASSDIAGRAM')
            and   type = 'U')
   drop table BUSINESSCLASSDIAGRAM
go

alter table BUSINESSDATATYPE
   drop constraint PK_BUSINESSDATATYPE
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

alter table BUSINESSENUM
   drop constraint PK_BUSINESSENUM
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

alter table BUSINESSENUMVALUE
   drop constraint PK_BUSINESSENUMVALUE
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BUSINESSENUMVALUE')
            and   type = 'U')
   drop table BUSINESSENUMVALUE
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BUSINESSOBJECT')
            and   name  = 'IDX_OBJECTCLASS_FK'
            and   indid > 0
            and   indid < 255)
   drop index BUSINESSOBJECT.IDX_OBJECTCLASS_FK
go

alter table BUSINESSOBJECT
   drop constraint PK_BUSINESSOBJECT
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

alter table BUSINESSPACKAGE
   drop constraint PK_BUSINESSPACKAGE
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

alter table BUSINESSPROPERTY
   drop constraint PK_BUSINESSPROPERTY
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BUSINESSPROPERTY')
            and   type = 'U')
   drop table BUSINESSPROPERTY
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
   COPYLOCAL            bit                  not null
)
go

alter table ASSEMBLYREFERENCE
   add constraint PK_ASSEMBLYREFERENCE primary key (ID)
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
   ISSYSTEM             bit                  not null
)
go

alter table ASSEMBLYREFERENCEGROUP
   add constraint PK_ASSEMBLYREFERENCEGROUP primary key (ID)
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
   ISSYSTEM             bit                  not null
)
go

alter table BUSINESSASSOCIATION
   add constraint PK_BUSINESSASSOCIATION primary key (ID)
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
   SCRIPTOPTIONS        int                  null
)
go

alter table BUSINESSCLASS
   add constraint PK_BUSINESSCLASS primary key (ID)
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
   DIAGRAM              text                 null
)
go

alter table BUSINESSCLASSDIAGRAM
   add constraint PK_BUSINESSCLASSDIAGRAM primary key (PACKAGEID, ID)
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
   ISSYSTEM             bit                  not null
)
go

alter table BUSINESSDATATYPE
   add constraint PK_BUSINESSDATATYPE primary key (ID)
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
   ISSYSTEM             bit                  not null
)
go

alter table BUSINESSENUM
   add constraint PK_BUSINESSENUM primary key (ID)
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
   ISSYSTEM             bit                  not null
)
go

alter table BUSINESSENUMVALUE
   add constraint PK_BUSINESSENUMVALUE primary key (ENUMID, ID)
go

/*==============================================================*/
/* Index: IDX_BUSINESSENUMVALUES_FK                             */
/*==============================================================*/
create index IDX_BUSINESSENUMVALUES_FK on BUSINESSENUMVALUE (
ENUMID ASC
)
go

/*==============================================================*/
/* Table: BUSINESSOBJECT                                        */
/*==============================================================*/
create table BUSINESSOBJECT (
   ID                   T_GUID               not null,
   CLASSID              T_GUID               not null,
   NAME                 T_NAME               null,
   CREATIONTIME         datetime             not null,
   MODIFICATIONTIME     datetime             not null,
   ISSYSTEM             bit                  not null
)
go

alter table BUSINESSOBJECT
   add constraint PK_BUSINESSOBJECT primary key (ID)
go

/*==============================================================*/
/* Index: IDX_OBJECTCLASS_FK                                    */
/*==============================================================*/
create index IDX_OBJECTCLASS_FK on BUSINESSOBJECT (
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
   ISSYSTEM             bit                  not null
)
go

alter table BUSINESSPACKAGE
   add constraint PK_BUSINESSPACKAGE primary key (ID)
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
   ISSYSTEM             bit                  not null
)
go

alter table BUSINESSPROPERTY
   add constraint PK_BUSINESSPROPERTY primary key (CLASSID, ID)
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

alter table ASSEMBLYREFERENCE
   add constraint FK_ASSEMBLYREFERENCEGROUP foreign key (GROUPID)
      references ASSEMBLYREFERENCEGROUP (ID)
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

alter table BUSINESSOBJECT
   add constraint FK_BUSINESS_OBJECTCLA_BUSINESS foreign key (CLASSID)
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

