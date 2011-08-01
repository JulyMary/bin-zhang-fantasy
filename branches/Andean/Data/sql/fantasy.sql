/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2008                    */
/* Created on:     1/08/2011 5:02:07 PM                         */
/*==============================================================*/


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
           where  id    = object_id('BUSINESSOBJECT')
            and   name  = 'IDX_OBJECTCLASS_FK'
            and   indid > 0
            and   indid < 255)
   drop index BUSINESSOBJECT.IDX_OBJECTCLASS_FK
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
           where  id    = object_id('BUSINESSRELATIONSHIP')
            and   name  = 'RELATIONSHIPRIGHT_FK'
            and   indid > 0
            and   indid < 255)
   drop index BUSINESSRELATIONSHIP.RELATIONSHIPRIGHT_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BUSINESSRELATIONSHIP')
            and   name  = 'RELATIONSHIPLEFT_FK'
            and   indid > 0
            and   indid < 255)
   drop index BUSINESSRELATIONSHIP.RELATIONSHIPLEFT_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BUSINESSRELATIONSHIP')
            and   type = 'U')
   drop table BUSINESSRELATIONSHIP
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
   NAME                 T_NAME               not null,
   DIAGRAM              text                 null,
   MODIFICATIONTIME     datetime             not null,
   CREATIONTIME         datetime             not null,
   ISSYSTEM             bit                  not null,
   constraint PK_BUSINESSCLASSDIAGRAM primary key (ID)
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
/* Table: BUSINESSOBJECT                                        */
/*==============================================================*/
create table BUSINESSOBJECT (
   ID                   T_GUID               not null,
   CLASSID              T_GUID               not null,
   NAME                 T_NAME               null,
   CREATIONTIME         datetime             not null,
   MODIFICATIONTIME     datetime             not null,
   ISSYSTEM             bit                  not null,
   constraint PK_BUSINESSOBJECT primary key (ID)
)
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
   BUILDASASSEMBLY      bit                  not null default 0,
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
   "ORDER"              int                  not null,
   ISCALCULATED         bit                  not null,
   CREATIONTIME         datetime             not null,
   MODIFICATIONTIME     datetime             not null,
   ISSYSTEM             bit                  not null,
   constraint PK_BUSINESSPROPERTY primary key (ID)
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
/* Table: BUSINESSRELATIONSHIP                                  */
/*==============================================================*/
create table BUSINESSRELATIONSHIP (
   ID                   T_GUID               not null,
   NAME                 T_NAME               not null,
   CODENAME             T_NAME               not null,
   TABLENAME            T_DBENTITY           not null,
   TABLESCHEMA          T_DBENTITY           not null,
   TABLESPACE           T_DBENTITY           not null,
   LEFTROLENAME         T_NAME               not null,
   LEFTROLECODE         T_NAME               not null,
   LEFTCLASSID          T_GUID               not null,
   LEFTCARDINALITY      int                  not null,
   LEFTNAVIGATABLE      bit                  not null,
   RIGHTROLENAME        T_NAME               not null,
   RIGHTROLECODE        T_NAME               not null,
   RIGHTCLASSID         T_GUID               not null,
   RIGHTCARDINALITY     int                  not null,
   RIGHTNAVIGATABLE     bit                  not null,
   CREATIONTIME         datetime             not null,
   MODIFICATIONTIME     datetime             not null,
   ISSYSTEM             bit                  not null,
   constraint PK_BUSINESSRELATIONSHIP primary key (ID)
)
go

/*==============================================================*/
/* Index: RELATIONSHIPLEFT_FK                                   */
/*==============================================================*/
create index RELATIONSHIPLEFT_FK on BUSINESSRELATIONSHIP (
LEFTCLASSID ASC
)
go

/*==============================================================*/
/* Index: RELATIONSHIPRIGHT_FK                                  */
/*==============================================================*/
create index RELATIONSHIPRIGHT_FK on BUSINESSRELATIONSHIP (
RIGHTCLASSID ASC
)
go

